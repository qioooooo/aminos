using System;

namespace System.Windows.Forms
{
	// Token: 0x020001F0 RID: 496
	public interface IWindowTarget
	{
		// Token: 0x060016EA RID: 5866
		void OnHandleChange(IntPtr newHandle);

		// Token: 0x060016EB RID: 5867
		void OnMessage(ref Message m);
	}
}
