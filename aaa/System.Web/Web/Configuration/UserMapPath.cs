using System;
using System.Configuration;
using System.IO;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x02000261 RID: 609
	internal class UserMapPath : IConfigMapPath
	{
		// Token: 0x06002018 RID: 8216 RVA: 0x0008C75A File Offset: 0x0008B75A
		internal UserMapPath(ConfigurationFileMap fileMap)
			: this(fileMap, true)
		{
		}

		// Token: 0x06002019 RID: 8217 RVA: 0x0008C764 File Offset: 0x0008B764
		internal UserMapPath(ConfigurationFileMap fileMap, bool pathsAreLocal)
		{
			this._pathsAreLocal = pathsAreLocal;
			this._machineConfigFilename = HttpConfigurationSystem.MachineConfigurationFilePath;
			this._rootWebConfigFilename = HttpConfigurationSystem.RootWebConfigurationFilePath;
			if (!string.IsNullOrEmpty(fileMap.MachineConfigFilename))
			{
				if (this._pathsAreLocal)
				{
					this._machineConfigFilename = Path.GetFullPath(fileMap.MachineConfigFilename);
				}
				else
				{
					this._machineConfigFilename = fileMap.MachineConfigFilename;
				}
			}
			this._webFileMap = fileMap as WebConfigurationFileMap;
			if (this._webFileMap != null)
			{
				if (!string.IsNullOrEmpty(this._webFileMap.Site))
				{
					this._siteName = this._webFileMap.Site;
					this._siteID = this._webFileMap.Site;
				}
				else
				{
					this._siteName = WebConfigurationHost.DefaultSiteName;
					this._siteID = "1";
				}
				if (this._pathsAreLocal)
				{
					foreach (object obj in this._webFileMap.VirtualDirectories)
					{
						string text = (string)obj;
						VirtualDirectoryMapping virtualDirectoryMapping = this._webFileMap.VirtualDirectories[text];
						virtualDirectoryMapping.Validate();
					}
				}
				VirtualDirectoryMapping virtualDirectoryMapping2 = this._webFileMap.VirtualDirectories[null];
				if (virtualDirectoryMapping2 != null)
				{
					this._rootWebConfigFilename = Path.Combine(virtualDirectoryMapping2.PhysicalDirectory, virtualDirectoryMapping2.ConfigFileBaseName);
					this._webFileMap.VirtualDirectories.Remove(null);
				}
			}
		}

		// Token: 0x0600201A RID: 8218 RVA: 0x0008C8D8 File Offset: 0x0008B8D8
		private bool IsSiteMatch(string site)
		{
			return string.IsNullOrEmpty(site) || StringUtil.EqualsIgnoreCase(site, this._siteName) || StringUtil.EqualsIgnoreCase(site, this._siteID);
		}

		// Token: 0x0600201B RID: 8219 RVA: 0x0008C900 File Offset: 0x0008B900
		private VirtualDirectoryMapping GetPathMapping(VirtualPath path, bool onlyApps)
		{
			if (this._webFileMap == null)
			{
				return null;
			}
			string text = path.VirtualPathStringNoTrailingSlash;
			VirtualDirectoryMapping virtualDirectoryMapping;
			for (;;)
			{
				virtualDirectoryMapping = this._webFileMap.VirtualDirectories[text];
				if (virtualDirectoryMapping != null && (!onlyApps || virtualDirectoryMapping.IsAppRoot))
				{
					break;
				}
				if (text == "/")
				{
					goto Block_4;
				}
				int num = text.LastIndexOf('/');
				if (num == 0)
				{
					text = "/";
				}
				else
				{
					text = text.Substring(0, num);
				}
			}
			return virtualDirectoryMapping;
			Block_4:
			return null;
		}

		// Token: 0x0600201C RID: 8220 RVA: 0x0008C970 File Offset: 0x0008B970
		private string GetPhysicalPathForPath(string path, VirtualDirectoryMapping mapping)
		{
			int length = mapping.VirtualDirectory.Length;
			string text;
			if (path.Length == length)
			{
				text = mapping.PhysicalDirectory;
			}
			else
			{
				string text2;
				if (path[length] == '/')
				{
					text2 = path.Substring(length + 1);
				}
				else
				{
					text2 = path.Substring(length);
				}
				text2 = text2.Replace('/', '\\');
				text = Path.Combine(mapping.PhysicalDirectory, text2);
			}
			if (this._pathsAreLocal && FileUtil.IsSuspiciousPhysicalPath(text))
			{
				throw new HttpException(SR.GetString("Cannot_map_path", new object[] { path }));
			}
			return text;
		}

		// Token: 0x0600201D RID: 8221 RVA: 0x0008C9FF File Offset: 0x0008B9FF
		public string GetMachineConfigFilename()
		{
			return this._machineConfigFilename;
		}

		// Token: 0x0600201E RID: 8222 RVA: 0x0008CA07 File Offset: 0x0008BA07
		public string GetRootWebConfigFilename()
		{
			return this._rootWebConfigFilename;
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x0008CA0F File Offset: 0x0008BA0F
		public void GetPathConfigFilename(string siteID, string path, out string directory, out string baseName)
		{
			this.GetPathConfigFilename(siteID, VirtualPath.Create(path), out directory, out baseName);
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x0008CA24 File Offset: 0x0008BA24
		private void GetPathConfigFilename(string siteID, VirtualPath path, out string directory, out string baseName)
		{
			directory = null;
			baseName = null;
			if (!this.IsSiteMatch(siteID))
			{
				return;
			}
			VirtualDirectoryMapping pathMapping = this.GetPathMapping(path, false);
			if (pathMapping == null)
			{
				return;
			}
			directory = this.GetPhysicalPathForPath(path.VirtualPathString, pathMapping);
			if (directory == null)
			{
				return;
			}
			baseName = pathMapping.ConfigFileBaseName;
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x0008CA6C File Offset: 0x0008BA6C
		public void GetDefaultSiteNameAndID(out string siteName, out string siteID)
		{
			siteName = this._siteName;
			siteID = this._siteID;
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x0008CA7E File Offset: 0x0008BA7E
		public void ResolveSiteArgument(string siteArgument, out string siteName, out string siteID)
		{
			if (this.IsSiteMatch(siteArgument))
			{
				siteName = this._siteName;
				siteID = this._siteID;
				return;
			}
			siteName = siteArgument;
			siteID = null;
		}

		// Token: 0x06002023 RID: 8227 RVA: 0x0008CAA0 File Offset: 0x0008BAA0
		public string MapPath(string siteID, string path)
		{
			return this.MapPath(siteID, VirtualPath.Create(path));
		}

		// Token: 0x06002024 RID: 8228 RVA: 0x0008CAB0 File Offset: 0x0008BAB0
		private string MapPath(string siteID, VirtualPath path)
		{
			string text;
			string text2;
			this.GetPathConfigFilename(siteID, path, out text, out text2);
			return text;
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x0008CACC File Offset: 0x0008BACC
		public string GetAppPathForPath(string siteID, string path)
		{
			VirtualPath appPathForPath = this.GetAppPathForPath(siteID, VirtualPath.Create(path));
			if (appPathForPath == null)
			{
				return null;
			}
			return appPathForPath.VirtualPathString;
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x0008CAF8 File Offset: 0x0008BAF8
		private VirtualPath GetAppPathForPath(string siteID, VirtualPath path)
		{
			if (!this.IsSiteMatch(siteID))
			{
				return null;
			}
			VirtualDirectoryMapping pathMapping = this.GetPathMapping(path, true);
			if (pathMapping == null)
			{
				return null;
			}
			return pathMapping.VirtualDirectoryObject;
		}

		// Token: 0x04001A80 RID: 6784
		private string _machineConfigFilename;

		// Token: 0x04001A81 RID: 6785
		private string _rootWebConfigFilename;

		// Token: 0x04001A82 RID: 6786
		private string _siteName;

		// Token: 0x04001A83 RID: 6787
		private string _siteID;

		// Token: 0x04001A84 RID: 6788
		private WebConfigurationFileMap _webFileMap;

		// Token: 0x04001A85 RID: 6789
		private bool _pathsAreLocal;
	}
}
