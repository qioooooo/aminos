using System;
using System.Collections;
using System.Web.Caching;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x02000086 RID: 134
	internal struct ResponseDependencyList
	{
		// Token: 0x06000669 RID: 1641 RVA: 0x0001B838 File Offset: 0x0001A838
		internal void AddDependency(string item, string argname)
		{
			if (item == null)
			{
				throw new ArgumentNullException(argname);
			}
			this._dependencyArray = null;
			if (this._dependencies == null)
			{
				this._dependencies = new ArrayList(1);
			}
			DateTime utcNow = DateTime.UtcNow;
			this._dependencies.Add(new ResponseDependencyInfo(new string[] { item }, utcNow));
			if (this._oldestDependency == DateTime.MinValue || utcNow < this._oldestDependency)
			{
				this._oldestDependency = utcNow;
			}
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x0001B8B8 File Offset: 0x0001A8B8
		internal void AddDependencies(ArrayList items, string argname)
		{
			if (items == null)
			{
				throw new ArgumentNullException(argname);
			}
			string[] array = (string[])items.ToArray(typeof(string));
			this.AddDependencies(array, argname, false);
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0001B8EE File Offset: 0x0001A8EE
		internal void AddDependencies(string[] items, string argname)
		{
			this.AddDependencies(items, argname, true);
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0001B8F9 File Offset: 0x0001A8F9
		internal void AddDependencies(string[] items, string argname, bool cloneArray)
		{
			this.AddDependencies(items, argname, cloneArray, DateTime.UtcNow);
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0001B909 File Offset: 0x0001A909
		internal void AddDependencies(string[] items, string argname, bool cloneArray, string requestVirtualPath)
		{
			if (requestVirtualPath == null)
			{
				throw new ArgumentNullException("requestVirtualPath");
			}
			this._requestVirtualPath = requestVirtualPath;
			this.AddDependencies(items, argname, cloneArray, DateTime.UtcNow);
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x0001B930 File Offset: 0x0001A930
		internal void AddDependencies(string[] items, string argname, bool cloneArray, DateTime utcDepTime)
		{
			if (items == null)
			{
				throw new ArgumentNullException(argname);
			}
			string[] array;
			if (cloneArray)
			{
				array = (string[])items.Clone();
			}
			else
			{
				array = items;
			}
			foreach (string text in array)
			{
				if (string.IsNullOrEmpty(text))
				{
					throw new ArgumentNullException(argname);
				}
			}
			this._dependencyArray = null;
			if (this._dependencies == null)
			{
				this._dependencies = new ArrayList(1);
			}
			this._dependencies.Add(new ResponseDependencyInfo(array, utcDepTime));
			if (this._oldestDependency == DateTime.MinValue || utcDepTime < this._oldestDependency)
			{
				this._oldestDependency = utcDepTime;
			}
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x0001B9D5 File Offset: 0x0001A9D5
		internal bool HasDependencies()
		{
			return this._dependencyArray != null || this._dependencies != null;
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0001B9EC File Offset: 0x0001A9EC
		internal string[] GetDependencies()
		{
			if (this._dependencyArray == null && this._dependencies != null)
			{
				int num = 0;
				foreach (object obj in this._dependencies)
				{
					ResponseDependencyInfo responseDependencyInfo = (ResponseDependencyInfo)obj;
					num += responseDependencyInfo.items.Length;
				}
				this._dependencyArray = new string[num];
				int num2 = 0;
				foreach (object obj2 in this._dependencies)
				{
					ResponseDependencyInfo responseDependencyInfo2 = (ResponseDependencyInfo)obj2;
					int num3 = responseDependencyInfo2.items.Length;
					Array.Copy(responseDependencyInfo2.items, 0, this._dependencyArray, num2, num3);
					num2 += num3;
				}
			}
			return this._dependencyArray;
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0001BAE8 File Offset: 0x0001AAE8
		internal CacheDependency CreateCacheDependency(CacheDependencyType dependencyType, CacheDependency dependency)
		{
			if (this._dependencies != null)
			{
				if (dependencyType == CacheDependencyType.Files || dependencyType == CacheDependencyType.CacheItems)
				{
					using (IEnumerator enumerator = this._dependencies.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							ResponseDependencyInfo responseDependencyInfo = (ResponseDependencyInfo)obj;
							using (CacheDependency cacheDependency = dependency)
							{
								if (dependencyType == CacheDependencyType.Files)
								{
									dependency = new CacheDependency(0, responseDependencyInfo.items, null, cacheDependency, responseDependencyInfo.utcDate);
								}
								else
								{
									dependency = new CacheDependency(null, responseDependencyInfo.items, cacheDependency, DateTimeUtil.ConvertToLocalTime(responseDependencyInfo.utcDate));
								}
							}
						}
						return dependency;
					}
				}
				CacheDependency cacheDependency2 = null;
				VirtualPathProvider virtualPathProvider = HostingEnvironment.VirtualPathProvider;
				if (virtualPathProvider != null && this._requestVirtualPath != null)
				{
					cacheDependency2 = virtualPathProvider.GetCacheDependency(this._requestVirtualPath, this.GetDependencies(), this._oldestDependency);
				}
				if (cacheDependency2 != null)
				{
					AggregateCacheDependency aggregateCacheDependency = new AggregateCacheDependency();
					aggregateCacheDependency.Add(new CacheDependency[] { cacheDependency2 });
					if (dependency != null)
					{
						aggregateCacheDependency.Add(new CacheDependency[] { dependency });
					}
					dependency = aggregateCacheDependency;
				}
			}
			return dependency;
		}

		// Token: 0x040010E8 RID: 4328
		private ArrayList _dependencies;

		// Token: 0x040010E9 RID: 4329
		private string[] _dependencyArray;

		// Token: 0x040010EA RID: 4330
		private DateTime _oldestDependency;

		// Token: 0x040010EB RID: 4331
		private string _requestVirtualPath;
	}
}
