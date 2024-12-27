using System;
using System.Collections;
using System.Text;
using System.Web.Caching;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x02000212 RID: 530
	internal class MetabaseServerConfig : IServerConfig, IConfigMapPath, IConfigMapPath2
	{
		// Token: 0x06001C77 RID: 7287 RVA: 0x00082984 File Offset: 0x00081984
		internal static IServerConfig GetInstance()
		{
			if (MetabaseServerConfig.s_instance == null)
			{
				lock (MetabaseServerConfig.s_initLock)
				{
					if (MetabaseServerConfig.s_instance == null)
					{
						MetabaseServerConfig.s_instance = new MetabaseServerConfig();
					}
				}
			}
			return MetabaseServerConfig.s_instance;
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x000829D4 File Offset: 0x000819D4
		private MetabaseServerConfig()
		{
			HttpRuntime.ForceStaticInit();
			this._mapPathCacheLock = new object();
			this.MBGetSiteNameFromSiteID("1", out this._defaultSiteName);
			this._siteIdForCurrentApplication = HostingEnvironment.SiteID;
			if (this._siteIdForCurrentApplication == null)
			{
				this._siteIdForCurrentApplication = "1";
			}
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x00082A28 File Offset: 0x00081A28
		string IServerConfig.GetSiteNameFromSiteID(string siteID)
		{
			if (StringUtil.EqualsIgnoreCase(siteID, "1"))
			{
				return this._defaultSiteName;
			}
			string text;
			this.MBGetSiteNameFromSiteID(siteID, out text);
			return text;
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x00082A54 File Offset: 0x00081A54
		string IServerConfig.MapPath(IApplicationHost appHost, VirtualPath path)
		{
			string text = ((appHost == null) ? this._siteIdForCurrentApplication : appHost.GetSiteID());
			return this.MapPathCaching(text, path);
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x00082A7C File Offset: 0x00081A7C
		string[] IServerConfig.GetVirtualSubdirs(VirtualPath path, bool inApp)
		{
			string aboPath = this.GetAboPath(this._siteIdForCurrentApplication, path.VirtualPathString);
			return this.MBGetVirtualSubdirs(aboPath, inApp);
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x00082AA4 File Offset: 0x00081AA4
		bool IServerConfig.GetUncUser(IApplicationHost appHost, VirtualPath path, out string username, out string password)
		{
			string aboPath = this.GetAboPath(appHost.GetSiteID(), path.VirtualPathString);
			return this.MBGetUncUser(aboPath, out username, out password);
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x00082ACE File Offset: 0x00081ACE
		long IServerConfig.GetW3WPMemoryLimitInKB()
		{
			return (long)this.MBGetW3WPMemoryLimitInKB();
		}

		// Token: 0x06001C7E RID: 7294 RVA: 0x00082AD7 File Offset: 0x00081AD7
		string IConfigMapPath.GetMachineConfigFilename()
		{
			return HttpConfigurationSystem.MachineConfigurationFilePath;
		}

		// Token: 0x06001C7F RID: 7295 RVA: 0x00082ADE File Offset: 0x00081ADE
		string IConfigMapPath.GetRootWebConfigFilename()
		{
			return HttpConfigurationSystem.RootWebConfigurationFilePath;
		}

		// Token: 0x06001C80 RID: 7296 RVA: 0x00082AE5 File Offset: 0x00081AE5
		private void GetPathConfigFilenameWorker(string siteID, VirtualPath path, out string directory, out string baseName)
		{
			directory = this.MapPathCaching(siteID, path);
			if (directory != null)
			{
				baseName = "web.config";
				return;
			}
			baseName = null;
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x00082B02 File Offset: 0x00081B02
		void IConfigMapPath.GetPathConfigFilename(string siteID, string path, out string directory, out string baseName)
		{
			this.GetPathConfigFilenameWorker(siteID, VirtualPath.Create(path), out directory, out baseName);
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x00082B14 File Offset: 0x00081B14
		void IConfigMapPath2.GetPathConfigFilename(string siteID, VirtualPath path, out string directory, out string baseName)
		{
			this.GetPathConfigFilenameWorker(siteID, path, out directory, out baseName);
		}

		// Token: 0x06001C83 RID: 7299 RVA: 0x00082B21 File Offset: 0x00081B21
		void IConfigMapPath.GetDefaultSiteNameAndID(out string siteName, out string siteID)
		{
			siteName = this._defaultSiteName;
			siteID = "1";
		}

		// Token: 0x06001C84 RID: 7300 RVA: 0x00082B34 File Offset: 0x00081B34
		void IConfigMapPath.ResolveSiteArgument(string siteArgument, out string siteName, out string siteID)
		{
			if (string.IsNullOrEmpty(siteArgument) || StringUtil.EqualsIgnoreCase(siteArgument, "1") || StringUtil.EqualsIgnoreCase(siteArgument, this._defaultSiteName))
			{
				siteName = this._defaultSiteName;
				siteID = "1";
				return;
			}
			siteName = string.Empty;
			siteID = string.Empty;
			bool flag = false;
			if (IISMapPath.IsSiteId(siteArgument))
			{
				flag = this.MBGetSiteNameFromSiteID(siteArgument, out siteName);
			}
			if (flag)
			{
				siteID = siteArgument;
				return;
			}
			flag = this.MBGetSiteIDFromSiteName(siteArgument, out siteID);
			if (flag)
			{
				siteName = siteArgument;
				return;
			}
			siteName = siteArgument;
			siteID = string.Empty;
		}

		// Token: 0x06001C85 RID: 7301 RVA: 0x00082BB6 File Offset: 0x00081BB6
		string IConfigMapPath.MapPath(string siteID, string vpath)
		{
			return this.MapPathCaching(siteID, VirtualPath.Create(vpath));
		}

		// Token: 0x06001C86 RID: 7302 RVA: 0x00082BC5 File Offset: 0x00081BC5
		string IConfigMapPath2.MapPath(string siteID, VirtualPath vpath)
		{
			return this.MapPathCaching(siteID, vpath);
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x00082BD0 File Offset: 0x00081BD0
		private VirtualPath GetAppPathForPathWorker(string siteID, VirtualPath vpath)
		{
			string aboPath = this.GetAboPath(siteID, vpath.VirtualPathString);
			string text = this.MBGetAppPath(aboPath);
			if (text == null)
			{
				return VirtualPath.RootVirtualPath;
			}
			string rootAppIDFromSiteID = this.GetRootAppIDFromSiteID(siteID);
			if (StringUtil.EqualsIgnoreCase(rootAppIDFromSiteID, text))
			{
				return VirtualPath.RootVirtualPath;
			}
			string text2 = text.Substring(rootAppIDFromSiteID.Length);
			return VirtualPath.CreateAbsolute(text2);
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x00082C28 File Offset: 0x00081C28
		string IConfigMapPath.GetAppPathForPath(string siteID, string vpath)
		{
			VirtualPath appPathForPathWorker = this.GetAppPathForPathWorker(siteID, VirtualPath.Create(vpath));
			return appPathForPathWorker.VirtualPathString;
		}

		// Token: 0x06001C89 RID: 7305 RVA: 0x00082C49 File Offset: 0x00081C49
		VirtualPath IConfigMapPath2.GetAppPathForPath(string siteID, VirtualPath vpath)
		{
			return this.GetAppPathForPathWorker(siteID, vpath);
		}

		// Token: 0x06001C8A RID: 7306 RVA: 0x00082C54 File Offset: 0x00081C54
		private string MatchResult(VirtualPath path, string result)
		{
			if (string.IsNullOrEmpty(result))
			{
				return result;
			}
			result = result.Replace('/', '\\');
			if (path.HasTrailingSlash)
			{
				if (!UrlPath.PathEndsWithExtraSlash(result) && !UrlPath.PathIsDriveRoot(result))
				{
					result += "\\";
				}
			}
			else if (UrlPath.PathEndsWithExtraSlash(result) && !UrlPath.PathIsDriveRoot(result))
			{
				result = result.Substring(0, result.Length - 1);
			}
			return result;
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x00082CC0 File Offset: 0x00081CC0
		private string MapPathCaching(string siteID, VirtualPath path)
		{
			string text = "f" + siteID + path.VirtualPathString;
			MapPathCacheInfo mapPathCacheInfo = (MapPathCacheInfo)HttpRuntime.CacheInternal.Get(text);
			if (mapPathCacheInfo == null)
			{
				lock (this._mapPathCacheLock)
				{
					mapPathCacheInfo = (MapPathCacheInfo)HttpRuntime.CacheInternal.Get(text);
					if (mapPathCacheInfo == null)
					{
						mapPathCacheInfo = new MapPathCacheInfo();
						HttpRuntime.CacheInternal.UtcInsert(text, mapPathCacheInfo, null, DateTime.UtcNow.AddMinutes(10.0), Cache.NoSlidingExpiration);
					}
				}
			}
			if (!mapPathCacheInfo.Evaluated)
			{
				lock (mapPathCacheInfo)
				{
					if (!mapPathCacheInfo.Evaluated)
					{
						string text2 = null;
						try
						{
							text2 = this.MapPathActual(siteID, path);
							if (FileUtil.IsSuspiciousPhysicalPath(text2))
							{
								throw new HttpException(SR.GetString("Cannot_map_path", new object[] { path }));
							}
						}
						catch (Exception ex)
						{
							mapPathCacheInfo.CachedException = ex;
							mapPathCacheInfo.Evaluated = true;
							throw;
						}
						if (text2 != null)
						{
							mapPathCacheInfo.MapPathResult = text2;
							mapPathCacheInfo.Evaluated = true;
						}
					}
				}
			}
			if (mapPathCacheInfo.CachedException != null)
			{
				throw mapPathCacheInfo.CachedException;
			}
			return this.MatchResult(path, mapPathCacheInfo.MapPathResult);
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x00082E10 File Offset: 0x00081E10
		private string MapPathActual(string siteID, VirtualPath path)
		{
			string rootAppIDFromSiteID = this.GetRootAppIDFromSiteID(siteID);
			return this.MBMapPath(rootAppIDFromSiteID, path.VirtualPathString);
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x00082E34 File Offset: 0x00081E34
		private string GetRootAppIDFromSiteID(string siteId)
		{
			return "/LM/W3SVC/" + siteId + "/ROOT";
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x00082E48 File Offset: 0x00081E48
		private string GetAboPath(string siteID, string path)
		{
			string rootAppIDFromSiteID = this.GetRootAppIDFromSiteID(siteID);
			return rootAppIDFromSiteID + this.FixupPathSlash(path);
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x00082E6C File Offset: 0x00081E6C
		private string FixupPathSlash(string path)
		{
			if (path == null)
			{
				return null;
			}
			int length = path.Length;
			if (length == 0 || path[length - 1] != '/')
			{
				return path;
			}
			return path.Substring(0, length - 1);
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x00082EA4 File Offset: 0x00081EA4
		private bool MBGetSiteNameFromSiteID(string siteID, out string siteName)
		{
			string rootAppIDFromSiteID = this.GetRootAppIDFromSiteID(siteID);
			StringBuilder stringBuilder = new StringBuilder(261);
			int num = UnsafeNativeMethods.IsapiAppHostGetSiteName(rootAppIDFromSiteID, stringBuilder, stringBuilder.Capacity);
			if (num == 1)
			{
				siteName = stringBuilder.ToString();
				return true;
			}
			siteName = string.Empty;
			return false;
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x00082EE8 File Offset: 0x00081EE8
		private bool MBGetSiteIDFromSiteName(string siteName, out string siteID)
		{
			StringBuilder stringBuilder = new StringBuilder(261);
			int num = UnsafeNativeMethods.IsapiAppHostGetSiteId(siteName, stringBuilder, stringBuilder.Capacity);
			if (num == 1)
			{
				siteID = stringBuilder.ToString();
				return true;
			}
			siteID = string.Empty;
			return false;
		}

		// Token: 0x06001C92 RID: 7314 RVA: 0x00082F24 File Offset: 0x00081F24
		private string MBMapPath(string appID, string path)
		{
			int num = 261;
			StringBuilder stringBuilder;
			int num2;
			for (;;)
			{
				stringBuilder = new StringBuilder(num);
				num2 = UnsafeNativeMethods.IsapiAppHostMapPath(appID, path, stringBuilder, stringBuilder.Capacity);
				if (num2 != -2)
				{
					break;
				}
				num *= 2;
			}
			if (num2 == -1)
			{
				throw new HostingEnvironmentException(SR.GetString("Cannot_access_mappath_title"), SR.GetString("Cannot_access_mappath_details"));
			}
			string text;
			if (num2 == 1)
			{
				text = stringBuilder.ToString();
			}
			else
			{
				text = null;
			}
			return text;
		}

		// Token: 0x06001C93 RID: 7315 RVA: 0x00082F88 File Offset: 0x00081F88
		private string[] MBGetVirtualSubdirs(string aboPath, bool inApp)
		{
			StringBuilder stringBuilder = new StringBuilder(261);
			int num = 0;
			ArrayList arrayList = new ArrayList();
			for (;;)
			{
				stringBuilder.Length = 0;
				int num2 = UnsafeNativeMethods.IsapiAppHostGetNextVirtualSubdir(aboPath, inApp, ref num, stringBuilder, stringBuilder.Capacity);
				if (num2 == 0)
				{
					break;
				}
				string text = stringBuilder.ToString();
				arrayList.Add(text);
			}
			string[] array = new string[arrayList.Count];
			arrayList.CopyTo(array);
			return array;
		}

		// Token: 0x06001C94 RID: 7316 RVA: 0x00082FF0 File Offset: 0x00081FF0
		private bool MBGetUncUser(string aboPath, out string username, out string password)
		{
			StringBuilder stringBuilder = new StringBuilder(261);
			StringBuilder stringBuilder2 = new StringBuilder(261);
			int num = UnsafeNativeMethods.IsapiAppHostGetUncUser(aboPath, stringBuilder, stringBuilder.Capacity, stringBuilder2, stringBuilder2.Capacity);
			if (num == 1)
			{
				username = stringBuilder.ToString();
				password = stringBuilder2.ToString();
				return true;
			}
			username = null;
			password = null;
			return false;
		}

		// Token: 0x06001C95 RID: 7317 RVA: 0x00083045 File Offset: 0x00082045
		private int MBGetW3WPMemoryLimitInKB()
		{
			return UnsafeNativeMethods.GetW3WPMemoryLimitInKB();
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x0008304C File Offset: 0x0008204C
		private string MBGetAppPath(string aboPath)
		{
			StringBuilder stringBuilder = new StringBuilder(aboPath.Length + 1);
			int num = UnsafeNativeMethods.IsapiAppHostGetAppPath(aboPath, stringBuilder, stringBuilder.Capacity);
			string text;
			if (num == 1)
			{
				text = stringBuilder.ToString();
			}
			else
			{
				text = null;
			}
			return text;
		}

		// Token: 0x040018E8 RID: 6376
		private const string DEFAULT_SITEID = "1";

		// Token: 0x040018E9 RID: 6377
		private const string DEFAULT_ROOTAPPID = "/LM/W3SVC/1/ROOT";

		// Token: 0x040018EA RID: 6378
		private const int MAX_PATH = 260;

		// Token: 0x040018EB RID: 6379
		private const int BUFSIZE = 261;

		// Token: 0x040018EC RID: 6380
		private const string LMW3SVC_PREFIX = "/LM/W3SVC/";

		// Token: 0x040018ED RID: 6381
		private const string ROOT_SUFFIX = "/ROOT";

		// Token: 0x040018EE RID: 6382
		private static MetabaseServerConfig s_instance;

		// Token: 0x040018EF RID: 6383
		private static object s_initLock = new object();

		// Token: 0x040018F0 RID: 6384
		private object _mapPathCacheLock;

		// Token: 0x040018F1 RID: 6385
		private string _defaultSiteName;

		// Token: 0x040018F2 RID: 6386
		private string _siteIdForCurrentApplication;
	}
}
