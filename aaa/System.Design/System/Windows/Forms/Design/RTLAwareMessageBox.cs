using System;
using System.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000282 RID: 642
	internal static class RTLAwareMessageBox
	{
		// Token: 0x060017D7 RID: 6103 RVA: 0x0007BF89 File Offset: 0x0007AF89
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
		{
			if (RTLAwareMessageBox.IsRTLResources)
			{
				options |= MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;
			}
			return MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, options);
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x060017D8 RID: 6104 RVA: 0x0007BFAB File Offset: 0x0007AFAB
		public static bool IsRTLResources
		{
			get
			{
				return SR.GetString("RTL") != "RTL_False";
			}
		}
	}
}
