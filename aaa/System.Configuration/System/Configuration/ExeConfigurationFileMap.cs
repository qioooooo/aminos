using System;

namespace System.Configuration
{
	// Token: 0x02000066 RID: 102
	public sealed class ExeConfigurationFileMap : ConfigurationFileMap
	{
		// Token: 0x060003D7 RID: 983 RVA: 0x000136F7 File Offset: 0x000126F7
		public ExeConfigurationFileMap()
		{
			this._exeConfigFilename = string.Empty;
			this._roamingUserConfigFilename = string.Empty;
			this._localUserConfigFilename = string.Empty;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00013720 File Offset: 0x00012720
		private ExeConfigurationFileMap(string machineConfigFilename, string exeConfigFilename, string roamingUserConfigFilename, string localUserConfigFilename)
			: base(machineConfigFilename)
		{
			this._exeConfigFilename = exeConfigFilename;
			this._roamingUserConfigFilename = roamingUserConfigFilename;
			this._localUserConfigFilename = localUserConfigFilename;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0001373F File Offset: 0x0001273F
		public override object Clone()
		{
			return new ExeConfigurationFileMap(base.MachineConfigFilename, this._exeConfigFilename, this._roamingUserConfigFilename, this._localUserConfigFilename);
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060003DA RID: 986 RVA: 0x0001375E File Offset: 0x0001275E
		// (set) Token: 0x060003DB RID: 987 RVA: 0x00013766 File Offset: 0x00012766
		public string ExeConfigFilename
		{
			get
			{
				return this._exeConfigFilename;
			}
			set
			{
				this._exeConfigFilename = value;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060003DC RID: 988 RVA: 0x0001376F File Offset: 0x0001276F
		// (set) Token: 0x060003DD RID: 989 RVA: 0x00013777 File Offset: 0x00012777
		public string RoamingUserConfigFilename
		{
			get
			{
				return this._roamingUserConfigFilename;
			}
			set
			{
				this._roamingUserConfigFilename = value;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060003DE RID: 990 RVA: 0x00013780 File Offset: 0x00012780
		// (set) Token: 0x060003DF RID: 991 RVA: 0x00013788 File Offset: 0x00012788
		public string LocalUserConfigFilename
		{
			get
			{
				return this._localUserConfigFilename;
			}
			set
			{
				this._localUserConfigFilename = value;
			}
		}

		// Token: 0x040002F9 RID: 761
		private string _exeConfigFilename;

		// Token: 0x040002FA RID: 762
		private string _roamingUserConfigFilename;

		// Token: 0x040002FB RID: 763
		private string _localUserConfigFilename;
	}
}
