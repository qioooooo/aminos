using System;
using System.IO;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x02000262 RID: 610
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class VirtualDirectoryMapping
	{
		// Token: 0x06002027 RID: 8231 RVA: 0x0008CB24 File Offset: 0x0008BB24
		public VirtualDirectoryMapping(string physicalDirectory, bool isAppRoot)
			: this(null, physicalDirectory, isAppRoot, "web.config")
		{
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x0008CB34 File Offset: 0x0008BB34
		public VirtualDirectoryMapping(string physicalDirectory, bool isAppRoot, string configFileBaseName)
			: this(null, physicalDirectory, isAppRoot, configFileBaseName)
		{
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x0008CB40 File Offset: 0x0008BB40
		private VirtualDirectoryMapping(VirtualPath virtualDirectory, string physicalDirectory, bool isAppRoot, string configFileBaseName)
		{
			this._virtualDirectory = virtualDirectory;
			this._isAppRoot = isAppRoot;
			this.PhysicalDirectory = physicalDirectory;
			this.ConfigFileBaseName = configFileBaseName;
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x0008CB65 File Offset: 0x0008BB65
		internal VirtualDirectoryMapping Clone()
		{
			return new VirtualDirectoryMapping(this._virtualDirectory, this._physicalDirectory, this._isAppRoot, this._configFileBaseName);
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x0600202B RID: 8235 RVA: 0x0008CB84 File Offset: 0x0008BB84
		public string VirtualDirectory
		{
			get
			{
				if (!(this._virtualDirectory != null))
				{
					return string.Empty;
				}
				return this._virtualDirectory.VirtualPathString;
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x0600202C RID: 8236 RVA: 0x0008CBA5 File Offset: 0x0008BBA5
		internal VirtualPath VirtualDirectoryObject
		{
			get
			{
				return this._virtualDirectory;
			}
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x0008CBAD File Offset: 0x0008BBAD
		internal void SetVirtualDirectory(VirtualPath virtualDirectory)
		{
			this._virtualDirectory = virtualDirectory;
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x0600202E RID: 8238 RVA: 0x0008CBB6 File Offset: 0x0008BBB6
		// (set) Token: 0x0600202F RID: 8239 RVA: 0x0008CBC0 File Offset: 0x0008BBC0
		public string PhysicalDirectory
		{
			get
			{
				return this._physicalDirectory;
			}
			set
			{
				string text = value;
				if (string.IsNullOrEmpty(text))
				{
					text = null;
				}
				else
				{
					if (UrlPath.PathEndsWithExtraSlash(text))
					{
						text = text.Substring(0, text.Length - 1);
					}
					if (FileUtil.IsSuspiciousPhysicalPath(text))
					{
						throw ExceptionUtil.ParameterInvalid("PhysicalDirectory");
					}
				}
				this._physicalDirectory = text;
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06002030 RID: 8240 RVA: 0x0008CC0D File Offset: 0x0008BC0D
		// (set) Token: 0x06002031 RID: 8241 RVA: 0x0008CC15 File Offset: 0x0008BC15
		public bool IsAppRoot
		{
			get
			{
				return this._isAppRoot;
			}
			set
			{
				this._isAppRoot = value;
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06002032 RID: 8242 RVA: 0x0008CC1E File Offset: 0x0008BC1E
		// (set) Token: 0x06002033 RID: 8243 RVA: 0x0008CC26 File Offset: 0x0008BC26
		public string ConfigFileBaseName
		{
			get
			{
				return this._configFileBaseName;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw ExceptionUtil.PropertyInvalid("ConfigFileBaseName");
				}
				this._configFileBaseName = value;
			}
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x0008CC44 File Offset: 0x0008BC44
		internal void Validate()
		{
			if (this._physicalDirectory != null)
			{
				string text = Path.Combine(this._physicalDirectory, this._configFileBaseName);
				string fullPath = Path.GetFullPath(text);
				if (Path.GetDirectoryName(fullPath) != this._physicalDirectory || Path.GetFileName(fullPath) != this._configFileBaseName || FileUtil.IsSuspiciousPhysicalPath(text))
				{
					throw ExceptionUtil.ParameterInvalid("configFileBaseName");
				}
			}
		}

		// Token: 0x04001A86 RID: 6790
		private const string DEFAULT_BASE_NAME = "web.config";

		// Token: 0x04001A87 RID: 6791
		private VirtualPath _virtualDirectory;

		// Token: 0x04001A88 RID: 6792
		private string _physicalDirectory;

		// Token: 0x04001A89 RID: 6793
		private string _configFileBaseName;

		// Token: 0x04001A8A RID: 6794
		private bool _isAppRoot;
	}
}
