using System;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x0200002D RID: 45
	public class ConfigurationFileMap : ICloneable
	{
		// Token: 0x0600024F RID: 591 RVA: 0x0000F5A8 File Offset: 0x0000E5A8
		public ConfigurationFileMap()
		{
			this._machineConfigFilename = ClientConfigurationHost.MachineConfigFilePath;
			this._requirePathDiscovery = true;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000F5C2 File Offset: 0x0000E5C2
		public ConfigurationFileMap(string machineConfigFilename)
		{
			this._machineConfigFilename = machineConfigFilename;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000F5D1 File Offset: 0x0000E5D1
		public virtual object Clone()
		{
			return new ConfigurationFileMap(this._machineConfigFilename);
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0000F5E0 File Offset: 0x0000E5E0
		// (set) Token: 0x06000253 RID: 595 RVA: 0x0000F609 File Offset: 0x0000E609
		public string MachineConfigFilename
		{
			get
			{
				string machineConfigFilename = this._machineConfigFilename;
				if (this._requirePathDiscovery)
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, machineConfigFilename).Demand();
				}
				return machineConfigFilename;
			}
			set
			{
				this._requirePathDiscovery = false;
				this._machineConfigFilename = value;
			}
		}

		// Token: 0x04000259 RID: 601
		private string _machineConfigFilename;

		// Token: 0x0400025A RID: 602
		private bool _requirePathDiscovery;
	}
}
