using System;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020001DE RID: 478
	[SwitchLevel(typeof(TraceLevel))]
	public class TraceSwitch : Switch
	{
		// Token: 0x06000F2C RID: 3884 RVA: 0x00030D58 File Offset: 0x0002FD58
		public TraceSwitch(string displayName, string description)
			: base(displayName, description)
		{
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x00030D62 File Offset: 0x0002FD62
		public TraceSwitch(string displayName, string description, string defaultSwitchValue)
			: base(displayName, description, defaultSwitchValue)
		{
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000F2E RID: 3886 RVA: 0x00030D6D File Offset: 0x0002FD6D
		// (set) Token: 0x06000F2F RID: 3887 RVA: 0x00030D75 File Offset: 0x0002FD75
		public TraceLevel Level
		{
			get
			{
				return (TraceLevel)base.SwitchSetting;
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			set
			{
				if (value < TraceLevel.Off || value > TraceLevel.Verbose)
				{
					throw new ArgumentException(SR.GetString("TraceSwitchInvalidLevel"));
				}
				base.SwitchSetting = (int)value;
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000F30 RID: 3888 RVA: 0x00030D96 File Offset: 0x0002FD96
		public bool TraceError
		{
			get
			{
				return this.Level >= TraceLevel.Error;
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000F31 RID: 3889 RVA: 0x00030DA4 File Offset: 0x0002FDA4
		public bool TraceWarning
		{
			get
			{
				return this.Level >= TraceLevel.Warning;
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000F32 RID: 3890 RVA: 0x00030DB2 File Offset: 0x0002FDB2
		public bool TraceInfo
		{
			get
			{
				return this.Level >= TraceLevel.Info;
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000F33 RID: 3891 RVA: 0x00030DC0 File Offset: 0x0002FDC0
		public bool TraceVerbose
		{
			get
			{
				return this.Level == TraceLevel.Verbose;
			}
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x00030DCC File Offset: 0x0002FDCC
		protected override void OnSwitchSettingChanged()
		{
			int switchSetting = base.SwitchSetting;
			if (switchSetting < 0)
			{
				Trace.WriteLine(SR.GetString("TraceSwitchLevelTooLow", new object[] { base.DisplayName }));
				base.SwitchSetting = 0;
				return;
			}
			if (switchSetting > 4)
			{
				Trace.WriteLine(SR.GetString("TraceSwitchLevelTooHigh", new object[] { base.DisplayName }));
				base.SwitchSetting = 4;
			}
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x00030E37 File Offset: 0x0002FE37
		protected override void OnValueChanged()
		{
			base.SwitchSetting = (int)Enum.Parse(typeof(TraceLevel), base.Value, true);
		}
	}
}
