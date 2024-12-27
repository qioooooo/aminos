using System;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020001B9 RID: 441
	[SwitchLevel(typeof(bool))]
	public class BooleanSwitch : Switch
	{
		// Token: 0x06000D7B RID: 3451 RVA: 0x0002B402 File Offset: 0x0002A402
		public BooleanSwitch(string displayName, string description)
			: base(displayName, description)
		{
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x0002B40C File Offset: 0x0002A40C
		public BooleanSwitch(string displayName, string description, string defaultSwitchValue)
			: base(displayName, description, defaultSwitchValue)
		{
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000D7D RID: 3453 RVA: 0x0002B417 File Offset: 0x0002A417
		// (set) Token: 0x06000D7E RID: 3454 RVA: 0x0002B424 File Offset: 0x0002A424
		public bool Enabled
		{
			get
			{
				return base.SwitchSetting != 0;
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			set
			{
				base.SwitchSetting = (value ? 1 : 0);
			}
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x0002B434 File Offset: 0x0002A434
		protected override void OnValueChanged()
		{
			bool flag;
			if (bool.TryParse(base.Value, out flag))
			{
				base.SwitchSetting = (flag ? 1 : 0);
				return;
			}
			base.OnValueChanged();
		}
	}
}
