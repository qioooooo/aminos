using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Web.Caching;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x02000221 RID: 545
	internal sealed class ProcessHostMapPath : IConfigMapPath, IConfigMapPath2
	{
		// Token: 0x06001D37 RID: 7479 RVA: 0x00084E0C File Offset: 0x00083E0C
		static ProcessHostMapPath()
		{
			HttpRuntime.ForceStaticInit();
		}

		// Token: 0x06001D38 RID: 7480 RVA: 0x00084E14 File Offset: 0x00083E14
		internal ProcessHostMapPath(IProcessHostSupportFunctions functions)
		{
			if (functions == null)
			{
				ProcessHostConfigUtils.InitStandaloneConfig();
			}
			if (functions != null)
			{
				this._functions = Misc.CreateLocalSupportFunctions(functions);
			}
			if (this._functions != null)
			{
				IntPtr nativeConfigurationSystem = this._functions.GetNativeConfigurationSystem();
				if (IntPtr.Zero != nativeConfigurationSystem)
				{
					UnsafeIISMethods.MgdSetNativeConfiguration(nativeConfigurationSystem);
				}
			}
			this._mapPathCacheLock = new object();
		}

		// Token: 0x06001D39 RID: 7481 RVA: 0x00084E71 File Offset: 0x00083E71
		string IConfigMapPath.GetMachineConfigFilename()
		{
			return HttpConfigurationSystem.MachineConfigurationFilePath;
		}

		// Token: 0x06001D3A RID: 7482 RVA: 0x00084E78 File Offset: 0x00083E78
		string IConfigMapPath.GetRootWebConfigFilename()
		{
			string text = null;
			if (this._functions != null)
			{
				text = this._functions.GetRootWebConfigFilename();
			}
			if (string.IsNullOrEmpty(text))
			{
				text = HttpConfigurationSystem.RootWebConfigurationFilePath;
			}
			return text;
		}

		// Token: 0x06001D3B RID: 7483 RVA: 0x00084EAA File Offset: 0x00083EAA
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

		// Token: 0x06001D3C RID: 7484 RVA: 0x00084EC7 File Offset: 0x00083EC7
		void IConfigMapPath.GetPathConfigFilename(string siteID, string path, out string directory, out string baseName)
		{
			this.GetPathConfigFilenameWorker(siteID, VirtualPath.Create(path), out directory, out baseName);
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x00084ED9 File Offset: 0x00083ED9
		void IConfigMapPath2.GetPathConfigFilename(string siteID, VirtualPath path, out string directory, out string baseName)
		{
			this.GetPathConfigFilenameWorker(siteID, path, out directory, out baseName);
		}

		// Token: 0x06001D3E RID: 7486 RVA: 0x00084EE6 File Offset: 0x00083EE6
		void IConfigMapPath.GetDefaultSiteNameAndID(out string siteName, out string siteID)
		{
			siteID = "1";
			siteName = ProcessHostConfigUtils.GetSiteNameFromId(1U);
		}

		// Token: 0x06001D3F RID: 7487 RVA: 0x00084EF8 File Offset: 0x00083EF8
		void IConfigMapPath.ResolveSiteArgument(string siteArgument, out string siteName, out string siteID)
		{
			if (string.IsNullOrEmpty(siteArgument) || StringUtil.EqualsIgnoreCase(siteArgument, "1") || StringUtil.EqualsIgnoreCase(siteArgument, ProcessHostConfigUtils.GetSiteNameFromId(1U)))
			{
				siteName = ProcessHostConfigUtils.GetSiteNameFromId(1U);
				siteID = "1";
				return;
			}
			siteName = string.Empty;
			siteID = string.Empty;
			string text = null;
			if (IISMapPath.IsSiteId(siteArgument))
			{
				uint num;
				if (uint.TryParse(siteArgument, out num))
				{
					text = ProcessHostConfigUtils.GetSiteNameFromId(num);
				}
			}
			else
			{
				uint num2 = UnsafeIISMethods.MgdResolveSiteName(siteArgument);
				if (num2 != 0U)
				{
					siteID = num2.ToString(CultureInfo.InvariantCulture);
					siteName = siteArgument;
					return;
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				siteName = text;
				siteID = siteArgument;
				return;
			}
			siteName = siteArgument;
			siteID = string.Empty;
		}

		// Token: 0x06001D40 RID: 7488 RVA: 0x00084F98 File Offset: 0x00083F98
		private string MapPathWorker(string siteID, VirtualPath path)
		{
			return this.MapPathCaching(siteID, path);
		}

		// Token: 0x06001D41 RID: 7489 RVA: 0x00084FA2 File Offset: 0x00083FA2
		string IConfigMapPath2.MapPath(string siteID, VirtualPath path)
		{
			return this.MapPathWorker(siteID, path);
		}

		// Token: 0x06001D42 RID: 7490 RVA: 0x00084FAC File Offset: 0x00083FAC
		string IConfigMapPath.MapPath(string siteID, string path)
		{
			return this.MapPathWorker(siteID, VirtualPath.Create(path));
		}

		// Token: 0x06001D43 RID: 7491 RVA: 0x00084FBC File Offset: 0x00083FBC
		string IConfigMapPath.GetAppPathForPath(string siteID, string path)
		{
			VirtualPath appPathForPathWorker = this.GetAppPathForPathWorker(siteID, VirtualPath.Create(path));
			return appPathForPathWorker.VirtualPathString;
		}

		// Token: 0x06001D44 RID: 7492 RVA: 0x00084FDD File Offset: 0x00083FDD
		VirtualPath IConfigMapPath2.GetAppPathForPath(string siteID, VirtualPath path)
		{
			return this.GetAppPathForPathWorker(siteID, path);
		}

		// Token: 0x06001D45 RID: 7493 RVA: 0x00084FE8 File Offset: 0x00083FE8
		private VirtualPath GetAppPathForPathWorker(string siteID, VirtualPath path)
		{
			uint num = 0U;
			if (!uint.TryParse(siteID, out num))
			{
				return VirtualPath.RootVirtualPath;
			}
			IntPtr zero = IntPtr.Zero;
			int num2 = 0;
			string text;
			try
			{
				text = ((UnsafeIISMethods.MgdGetAppPathForPath(num, path.VirtualPathString, out zero, out num2) == 0 && num2 > 0) ? StringUtil.StringFromWCharPtr(zero, num2) : null);
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					Marshal.FreeBSTR(zero);
				}
			}
			if (text == null)
			{
				return VirtualPath.RootVirtualPath;
			}
			return VirtualPath.Create(text);
		}

		// Token: 0x06001D46 RID: 7494 RVA: 0x0008506C File Offset: 0x0008406C
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
						try
						{
							string text2 = null;
							uint num;
							if (uint.TryParse(siteID, out num))
							{
								string siteNameFromId = ProcessHostConfigUtils.GetSiteNameFromId(num);
								text2 = ProcessHostConfigUtils.MapPathActual(siteNameFromId, path);
							}
							if (text2 != null && text2.Length == 2 && text2[1] == ':')
							{
								text2 += "\\";
							}
							if (FileUtil.IsSuspiciousPhysicalPath(text2))
							{
								throw new HttpException(SR.GetString("Cannot_map_path", new object[] { path }));
							}
							mapPathCacheInfo.MapPathResult = text2;
						}
						catch (Exception ex)
						{
							mapPathCacheInfo.CachedException = ex;
							mapPathCacheInfo.Evaluated = true;
							throw;
						}
						mapPathCacheInfo.Evaluated = true;
					}
				}
			}
			if (mapPathCacheInfo.CachedException != null)
			{
				throw mapPathCacheInfo.CachedException;
			}
			return this.MatchResult(path, mapPathCacheInfo.MapPathResult);
		}

		// Token: 0x06001D47 RID: 7495 RVA: 0x000851F4 File Offset: 0x000841F4
		private string MatchResult(VirtualPath path, string result)
		{
			if (string.IsNullOrEmpty(result))
			{
				return result;
			}
			result = result.Replace('/', '\\');
			if (path.HasTrailingSlash)
			{
				if (!UrlPath.PathEndsWithExtraSlash(result))
				{
					result += "\\";
				}
			}
			else if (UrlPath.PathEndsWithExtraSlash(result))
			{
				result = result.Substring(0, result.Length - 1);
			}
			return result;
		}

		// Token: 0x0400194A RID: 6474
		private IProcessHostSupportFunctions _functions;

		// Token: 0x0400194B RID: 6475
		private object _mapPathCacheLock;
	}
}
