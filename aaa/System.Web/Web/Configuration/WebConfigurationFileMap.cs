using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x02000266 RID: 614
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class WebConfigurationFileMap : ConfigurationFileMap
	{
		// Token: 0x06002047 RID: 8263 RVA: 0x0008CEEA File Offset: 0x0008BEEA
		public WebConfigurationFileMap()
		{
			this._site = string.Empty;
			this._virtualDirectoryMapping = new VirtualDirectoryMappingCollection();
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x0008CF08 File Offset: 0x0008BF08
		private WebConfigurationFileMap(string machineConfigFilename, string site, VirtualDirectoryMappingCollection VirtualDirectoryMapping)
			: base(machineConfigFilename)
		{
			this._site = site;
			this._virtualDirectoryMapping = VirtualDirectoryMapping;
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x0008CF20 File Offset: 0x0008BF20
		public override object Clone()
		{
			VirtualDirectoryMappingCollection virtualDirectoryMappingCollection = this._virtualDirectoryMapping.Clone();
			return new WebConfigurationFileMap(base.MachineConfigFilename, this._site, virtualDirectoryMappingCollection);
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x0600204A RID: 8266 RVA: 0x0008CF4B File Offset: 0x0008BF4B
		// (set) Token: 0x0600204B RID: 8267 RVA: 0x0008CF53 File Offset: 0x0008BF53
		internal string Site
		{
			get
			{
				return this._site;
			}
			set
			{
				if (!WebConfigurationHost.IsValidSiteArgument(value))
				{
					throw ExceptionUtil.PropertyInvalid("Site");
				}
				this._site = value;
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x0600204C RID: 8268 RVA: 0x0008CF6F File Offset: 0x0008BF6F
		public VirtualDirectoryMappingCollection VirtualDirectories
		{
			get
			{
				return this._virtualDirectoryMapping;
			}
		}

		// Token: 0x04001A8F RID: 6799
		private string _site;

		// Token: 0x04001A90 RID: 6800
		private VirtualDirectoryMappingCollection _virtualDirectoryMapping;
	}
}
