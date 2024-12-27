using System;

namespace System.Windows.Forms
{
	// Token: 0x0200020F RID: 527
	public interface IContainerControl
	{
		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06001830 RID: 6192
		// (set) Token: 0x06001831 RID: 6193
		Control ActiveControl { get; set; }

		// Token: 0x06001832 RID: 6194
		bool ActivateControl(Control active);
	}
}
