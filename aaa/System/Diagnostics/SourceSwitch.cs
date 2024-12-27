using System;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020001CF RID: 463
	public class SourceSwitch : Switch
	{
		// Token: 0x06000E65 RID: 3685 RVA: 0x0002DCC7 File Offset: 0x0002CCC7
		public SourceSwitch(string name)
			: base(name, string.Empty)
		{
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x0002DCD5 File Offset: 0x0002CCD5
		public SourceSwitch(string displayName, string defaultSwitchValue)
			: base(displayName, string.Empty, defaultSwitchValue)
		{
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000E67 RID: 3687 RVA: 0x0002DCE4 File Offset: 0x0002CCE4
		// (set) Token: 0x06000E68 RID: 3688 RVA: 0x0002DCEC File Offset: 0x0002CCEC
		public SourceLevels Level
		{
			get
			{
				return (SourceLevels)base.SwitchSetting;
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			set
			{
				base.SwitchSetting = (int)value;
			}
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x0002DCF5 File Offset: 0x0002CCF5
		public bool ShouldTrace(TraceEventType eventType)
		{
			return (base.SwitchSetting & (int)eventType) != 0;
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x0002DD05 File Offset: 0x0002CD05
		protected override void OnValueChanged()
		{
			base.SwitchSetting = (int)Enum.Parse(typeof(SourceLevels), base.Value, true);
		}
	}
}
