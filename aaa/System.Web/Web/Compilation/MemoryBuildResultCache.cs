using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Web.Caching;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x02000154 RID: 340
	internal class MemoryBuildResultCache : BuildResultCache
	{
		// Token: 0x06000F92 RID: 3986 RVA: 0x0004567B File Offset: 0x0004467B
		internal MemoryBuildResultCache(CacheInternal cache)
		{
			this._cache = cache;
			AppDomain.CurrentDomain.AssemblyLoad += this.OnAssemblyLoad;
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x000456AC File Offset: 0x000446AC
		private void OnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
		{
			Assembly loadedAssembly = args.LoadedAssembly;
			if (loadedAssembly.GlobalAssemblyCache)
			{
				return;
			}
			string name = loadedAssembly.GetName().Name;
			if (!StringUtil.StringStartsWith(name, "App_"))
			{
				return;
			}
			foreach (AssemblyName assemblyName in loadedAssembly.GetReferencedAssemblies())
			{
				if (StringUtil.StringStartsWith(assemblyName.Name, "App_"))
				{
					lock (this._dependentAssemblies)
					{
						ArrayList arrayList = this._dependentAssemblies[assemblyName.Name] as ArrayList;
						if (arrayList == null)
						{
							arrayList = new ArrayList();
							this._dependentAssemblies[assemblyName.Name] = arrayList;
						}
						arrayList.Add(name);
					}
				}
			}
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x00045778 File Offset: 0x00044778
		internal override BuildResult GetBuildResult(string cacheKey, VirtualPath virtualPath, long hashCode)
		{
			string memoryCacheKey = MemoryBuildResultCache.GetMemoryCacheKey(cacheKey);
			BuildResult buildResult = (BuildResult)this._cache.Get(memoryCacheKey);
			if (buildResult == null)
			{
				return null;
			}
			if (!buildResult.UsesCacheDependency && !buildResult.IsUpToDate(virtualPath))
			{
				this._cache.Remove(memoryCacheKey);
				return null;
			}
			return buildResult;
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x000457C4 File Offset: 0x000447C4
		internal override void CacheBuildResult(string cacheKey, BuildResult result, long hashCode, DateTime utcStart)
		{
			ICollection virtualPathDependencies = result.VirtualPathDependencies;
			CacheDependency cacheDependency = null;
			if (virtualPathDependencies != null)
			{
				cacheDependency = result.VirtualPath.GetCacheDependency(virtualPathDependencies, utcStart);
				if (cacheDependency != null)
				{
					result.UsesCacheDependency = true;
				}
			}
			if (!result.CacheToMemory)
			{
				return;
			}
			BuildResultCompiledAssemblyBase buildResultCompiledAssemblyBase = result as BuildResultCompiledAssemblyBase;
			if (buildResultCompiledAssemblyBase != null && buildResultCompiledAssemblyBase.ResultAssembly != null && !buildResultCompiledAssemblyBase.UsesExistingAssembly)
			{
				string assemblyCacheKey = BuildResultCache.GetAssemblyCacheKey(buildResultCompiledAssemblyBase.ResultAssembly);
				if ((Assembly)this._cache.Get(assemblyCacheKey) == null)
				{
					this._cache.UtcInsert(assemblyCacheKey, buildResultCompiledAssemblyBase.ResultAssembly, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
				}
				CacheDependency cacheDependency2 = new CacheDependency(0, null, new string[] { assemblyCacheKey });
				if (cacheDependency != null)
				{
					AggregateCacheDependency aggregateCacheDependency = new AggregateCacheDependency();
					aggregateCacheDependency.Add(new CacheDependency[] { cacheDependency, cacheDependency2 });
					cacheDependency = aggregateCacheDependency;
				}
				else
				{
					cacheDependency = cacheDependency2;
				}
			}
			string memoryCacheKey = MemoryBuildResultCache.GetMemoryCacheKey(cacheKey);
			CacheItemPriority cacheItemPriority;
			if (result.IsUnloadable)
			{
				cacheItemPriority = CacheItemPriority.Normal;
			}
			else
			{
				cacheItemPriority = CacheItemPriority.NotRemovable;
			}
			CacheItemRemovedCallback cacheItemRemovedCallback = null;
			if (result.ShutdownAppDomainOnChange || result is BuildResultCompiledAssemblyBase)
			{
				if (this._onRemoveCallback == null)
				{
					this._onRemoveCallback = new CacheItemRemovedCallback(this.OnCacheItemRemoved);
				}
				cacheItemRemovedCallback = this._onRemoveCallback;
			}
			this._cache.UtcInsert(memoryCacheKey, result, cacheDependency, result.MemoryCacheExpiration, result.MemoryCacheSlidingExpiration, cacheItemPriority, cacheItemRemovedCallback);
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x00045918 File Offset: 0x00044918
		private void OnCacheItemRemoved(string key, object value, CacheItemRemovedReason reason)
		{
			if (reason == CacheItemRemovedReason.DependencyChanged)
			{
				if (HostingEnvironment.ShutdownInitiated)
				{
					this.RemoveAssemblyAndCleanupDependenciesShuttingDown(value as BuildResultCompiledAssembly);
					return;
				}
				this.RemoveAssemblyAndCleanupDependencies(value as BuildResultCompiledAssemblyBase);
				if (((BuildResult)value).ShutdownAppDomainOnChange)
				{
					HttpRuntime.SetShutdownReason(ApplicationShutdownReason.BuildManagerChange, "BuildResult change, cache key=" + key);
					HostingEnvironment.InitiateShutdown();
				}
			}
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x00045970 File Offset: 0x00044970
		internal void RemoveAssemblyAndCleanupDependenciesShuttingDown(BuildResultCompiledAssemblyBase compiledResult)
		{
			if (compiledResult == null)
			{
				return;
			}
			if (compiledResult != null && compiledResult.ResultAssembly != null && !compiledResult.UsesExistingAssembly)
			{
				string name = compiledResult.ResultAssembly.GetName().Name;
				lock (this._dependentAssemblies)
				{
					this.RemoveAssemblyAndCleanupDependenciesNoLock(name);
				}
			}
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x000459D4 File Offset: 0x000449D4
		internal void RemoveAssemblyAndCleanupDependencies(BuildResultCompiledAssemblyBase compiledResult)
		{
			if (compiledResult == null)
			{
				return;
			}
			if (compiledResult != null && compiledResult.ResultAssembly != null && !compiledResult.UsesExistingAssembly)
			{
				this.RemoveAssemblyAndCleanupDependencies(compiledResult.ResultAssembly.GetName().Name);
			}
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x00045A04 File Offset: 0x00044A04
		private void RemoveAssemblyAndCleanupDependencies(string assemblyName)
		{
			bool flag = false;
			try
			{
				CompilationLock.GetLock(ref flag);
				lock (this._dependentAssemblies)
				{
					this.RemoveAssemblyAndCleanupDependenciesNoLock(assemblyName);
				}
			}
			finally
			{
				if (flag)
				{
					CompilationLock.ReleaseLock();
				}
				DiskBuildResultCache.ShutDownAppDomainIfRequired();
			}
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x00045A64 File Offset: 0x00044A64
		private void RemoveAssemblyAndCleanupDependenciesNoLock(string assemblyName)
		{
			string assemblyCacheKeyFromName = BuildResultCache.GetAssemblyCacheKeyFromName(assemblyName);
			Assembly assembly = (Assembly)this._cache[assemblyCacheKeyFromName];
			if (assembly == null)
			{
				return;
			}
			string assemblyCodeBase = Util.GetAssemblyCodeBase(assembly);
			this._cache.Remove(assemblyCacheKeyFromName);
			ICollection collection = this._dependentAssemblies[assemblyName] as ICollection;
			if (collection != null)
			{
				foreach (object obj in collection)
				{
					string text = (string)obj;
					this.RemoveAssemblyAndCleanupDependenciesNoLock(text);
				}
				this._dependentAssemblies.Remove(assemblyCacheKeyFromName);
			}
			DiskBuildResultCache.RemoveAssembly(new FileInfo(assemblyCodeBase));
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x00045B20 File Offset: 0x00044B20
		private static string GetMemoryCacheKey(string cacheKey)
		{
			return "c" + cacheKey;
		}

		// Token: 0x040015F8 RID: 5624
		private CacheInternal _cache;

		// Token: 0x040015F9 RID: 5625
		private CacheItemRemovedCallback _onRemoveCallback;

		// Token: 0x040015FA RID: 5626
		private Hashtable _dependentAssemblies = new Hashtable();
	}
}
