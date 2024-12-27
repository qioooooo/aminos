using System;
using System.IO;
using System.Threading;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x02000155 RID: 341
	internal abstract class DiskBuildResultCache : BuildResultCache
	{
		// Token: 0x06000F9C RID: 3996 RVA: 0x00045B2D File Offset: 0x00044B2D
		internal DiskBuildResultCache(string cacheDir)
		{
			this._cacheDir = cacheDir;
			if (DiskBuildResultCache.s_maxRecompilations < 0)
			{
				DiskBuildResultCache.s_maxRecompilations = CompilationUtil.GetRecompilationsBeforeAppRestarts();
			}
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x00045B50 File Offset: 0x00044B50
		protected void EnsureDiskCacheDirectoryCreated()
		{
			if (!FileUtil.DirectoryExists(this._cacheDir))
			{
				try
				{
					Directory.CreateDirectory(this._cacheDir);
				}
				catch (IOException ex)
				{
					throw new HttpException(SR.GetString("Failed_to_create_temp_dir", new object[] { HttpRuntime.GetSafePath(this._cacheDir) }), ex);
				}
			}
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x00045BB0 File Offset: 0x00044BB0
		internal override BuildResult GetBuildResult(string cacheKey, VirtualPath virtualPath, long hashCode)
		{
			string preservedDataFileName = this.GetPreservedDataFileName(cacheKey);
			PreservationFileReader preservationFileReader = new PreservationFileReader(this, this.PrecompilationMode);
			return preservationFileReader.ReadBuildResultFromFile(virtualPath, preservedDataFileName, hashCode);
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x00045BE0 File Offset: 0x00044BE0
		internal override void CacheBuildResult(string cacheKey, BuildResult result, long hashCode, DateTime utcStart)
		{
			if (!result.CacheToDisk)
			{
				return;
			}
			if (HostingEnvironment.ShutdownInitiated)
			{
				BuildResultCompiledAssemblyBase buildResultCompiledAssemblyBase = result as BuildResultCompiledAssemblyBase;
				if (buildResultCompiledAssemblyBase != null)
				{
					this.MarkAssemblyAndRelatedFilesForDeletion(buildResultCompiledAssemblyBase.ResultAssembly.GetName().Name);
				}
				return;
			}
			string preservedDataFileName = this.GetPreservedDataFileName(cacheKey);
			PreservationFileWriter preservationFileWriter = new PreservationFileWriter(this.PrecompilationMode);
			preservationFileWriter.SaveBuildResultToFile(preservedDataFileName, result, hashCode);
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x00045C3C File Offset: 0x00044C3C
		private void MarkAssemblyAndRelatedFilesForDeletion(string assemblyName)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(this._cacheDir);
			string text = assemblyName.Substring("App_Web_".Length);
			FileInfo[] files = directoryInfo.GetFiles("*" + text + ".*");
			foreach (FileInfo fileInfo in files)
			{
				DiskBuildResultCache.CreateDotDeleteFile(fileInfo);
			}
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x00045C9F File Offset: 0x00044C9F
		private string GetPreservedDataFileName(string cacheKey)
		{
			cacheKey = Util.MakeValidFileName(cacheKey);
			cacheKey = Path.Combine(this._cacheDir, cacheKey);
			cacheKey = FileUtil.TruncatePathIfNeeded(cacheKey, 9);
			return cacheKey + ".compiled";
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06000FA2 RID: 4002 RVA: 0x00045CCC File Offset: 0x00044CCC
		protected virtual bool PrecompilationMode
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06000FA3 RID: 4003 RVA: 0x00045CCF File Offset: 0x00044CCF
		internal static bool InUseAssemblyWasDeleted
		{
			get
			{
				return DiskBuildResultCache.s_inUseAssemblyWasDeleted;
			}
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x00045CD6 File Offset: 0x00044CD6
		internal static void ResetAssemblyDeleted()
		{
			DiskBuildResultCache.s_inUseAssemblyWasDeleted = false;
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x00045CE0 File Offset: 0x00044CE0
		internal virtual void RemoveAssemblyAndRelatedFiles(string assemblyName)
		{
			if (!assemblyName.StartsWith("App_Web_", StringComparison.Ordinal))
			{
				return;
			}
			string text = assemblyName.Substring("App_Web_".Length);
			bool flag = false;
			try
			{
				CompilationLock.GetLock(ref flag);
				DirectoryInfo directoryInfo = new DirectoryInfo(this._cacheDir);
				FileInfo[] files = directoryInfo.GetFiles("*" + text + ".*");
				foreach (FileInfo fileInfo in files)
				{
					if (fileInfo.Extension == ".dll")
					{
						string assemblyCacheKey = BuildResultCache.GetAssemblyCacheKey(fileInfo.FullName);
						HttpRuntime.CacheInternal.Remove(assemblyCacheKey);
						DiskBuildResultCache.RemoveAssembly(fileInfo);
						StandardDiskBuildResultCache.RemoveSatelliteAssemblies(assemblyName);
					}
					else if (fileInfo.Extension == ".delete")
					{
						DiskBuildResultCache.CheckAndRemoveDotDeleteFile(fileInfo);
					}
					else
					{
						DiskBuildResultCache.TryDeleteFile(fileInfo);
					}
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

		// Token: 0x06000FA6 RID: 4006 RVA: 0x00045DD8 File Offset: 0x00044DD8
		internal static void RemoveAssembly(FileInfo f)
		{
			if (HostingEnvironment.ShutdownInitiated)
			{
				DiskBuildResultCache.CreateDotDeleteFile(f);
				return;
			}
			if (DiskBuildResultCache.HasDotDeleteFile(f.FullName))
			{
				return;
			}
			if (DiskBuildResultCache.TryDeleteFile(f))
			{
				return;
			}
			if (++DiskBuildResultCache.s_recompilations == DiskBuildResultCache.s_maxRecompilations)
			{
				DiskBuildResultCache.s_shutdownStatus = 1;
			}
			DiskBuildResultCache.s_inUseAssemblyWasDeleted = true;
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x00045E2A File Offset: 0x00044E2A
		internal static void ShutDownAppDomainIfRequired()
		{
			if (DiskBuildResultCache.s_shutdownStatus == 1 && Interlocked.Exchange(ref DiskBuildResultCache.s_shutdownStatus, 2) == 1)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(DiskBuildResultCache.ShutdownCallBack));
			}
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x00045E54 File Offset: 0x00044E54
		private static void ShutdownCallBack(object state)
		{
			HttpRuntime.ShutdownAppDomain(ApplicationShutdownReason.MaxRecompilationsReached, "Recompilation limit of " + DiskBuildResultCache.s_maxRecompilations + " reached");
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x00045E77 File Offset: 0x00044E77
		internal static bool TryDeleteFile(string s)
		{
			return DiskBuildResultCache.TryDeleteFile(new FileInfo(s));
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x00045E84 File Offset: 0x00044E84
		internal static bool TryDeleteFile(FileInfo f)
		{
			if (f.Extension == ".delete")
			{
				return DiskBuildResultCache.CheckAndRemoveDotDeleteFile(f);
			}
			try
			{
				f.Delete();
				return true;
			}
			catch
			{
			}
			DiskBuildResultCache.CreateDotDeleteFile(f);
			return false;
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x00045ED0 File Offset: 0x00044ED0
		internal static bool CheckAndRemoveDotDeleteFile(FileInfo f)
		{
			if (f.Extension != ".delete")
			{
				return false;
			}
			string text = Path.GetDirectoryName(f.FullName) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(f.FullName);
			if (FileUtil.FileExists(text))
			{
				try
				{
					File.Delete(text);
				}
				catch
				{
					return false;
				}
			}
			try
			{
				f.Delete();
			}
			catch
			{
			}
			return true;
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x00045F58 File Offset: 0x00044F58
		internal static bool HasDotDeleteFile(string s)
		{
			return File.Exists(s + ".delete");
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x00045F6C File Offset: 0x00044F6C
		private static void CreateDotDeleteFile(FileInfo f)
		{
			if (f.Extension == ".delete")
			{
				return;
			}
			string text = f.FullName + ".delete";
			if (!File.Exists(text))
			{
				try
				{
					new StreamWriter(text).Close();
				}
				catch
				{
				}
			}
		}

		// Token: 0x040015FB RID: 5627
		protected const string preservationFileExtension = ".compiled";

		// Token: 0x040015FC RID: 5628
		protected const string dotDelete = ".delete";

		// Token: 0x040015FD RID: 5629
		private const int SHUTDOWN_NEEDED = 1;

		// Token: 0x040015FE RID: 5630
		private const int SHUTDOWN_STARTED = 2;

		// Token: 0x040015FF RID: 5631
		protected string _cacheDir;

		// Token: 0x04001600 RID: 5632
		private static int s_recompilations;

		// Token: 0x04001601 RID: 5633
		private static int s_maxRecompilations = -1;

		// Token: 0x04001602 RID: 5634
		private static bool s_inUseAssemblyWasDeleted;

		// Token: 0x04001603 RID: 5635
		private static int s_shutdownStatus;
	}
}
