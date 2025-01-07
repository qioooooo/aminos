using System;
using System.Design;

namespace System.Windows.Forms.Design
{
	internal static class RTLAwareMessageBox
	{
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
		{
			if (RTLAwareMessageBox.IsRTLResources)
			{
				options |= MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;
			}
			return MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, options);
		}

		public static bool IsRTLResources
		{
			get
			{
				return SR.GetString("RTL") != "RTL_False";
			}
		}
	}
}
