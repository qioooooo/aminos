using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000258 RID: 600
	[ComVisible(true)]
	internal interface ISupportInSituService
	{
		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x060016DA RID: 5850
		bool IgnoreMessages { get; }

		// Token: 0x060016DB RID: 5851
		void HandleKeyChar();

		// Token: 0x060016DC RID: 5852
		IntPtr GetEditWindow();
	}
}
