using System;

namespace System.Windows.Forms
{
	// Token: 0x02000259 RID: 601
	public interface IButtonControl
	{
		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06001F9A RID: 8090
		// (set) Token: 0x06001F9B RID: 8091
		DialogResult DialogResult { get; set; }

		// Token: 0x06001F9C RID: 8092
		void NotifyDefault(bool value);

		// Token: 0x06001F9D RID: 8093
		void PerformClick();
	}
}
