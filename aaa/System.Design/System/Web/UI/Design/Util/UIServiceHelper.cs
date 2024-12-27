using System;
using System.Collections;
using System.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Web.UI.Design.Util
{
	// Token: 0x020003CE RID: 974
	internal static class UIServiceHelper
	{
		// Token: 0x060023BD RID: 9149 RVA: 0x000BFC50 File Offset: 0x000BEC50
		public static Font GetDialogFont(IServiceProvider serviceProvider)
		{
			if (serviceProvider != null)
			{
				IUIService iuiservice = (IUIService)serviceProvider.GetService(typeof(IUIService));
				if (iuiservice != null)
				{
					IDictionary styles = iuiservice.Styles;
					if (styles != null)
					{
						return (Font)styles["DialogFont"];
					}
				}
			}
			return null;
		}

		// Token: 0x060023BE RID: 9150 RVA: 0x000BFC98 File Offset: 0x000BEC98
		public static IWin32Window GetDialogOwnerWindow(IServiceProvider serviceProvider)
		{
			if (serviceProvider != null)
			{
				IUIService iuiservice = (IUIService)serviceProvider.GetService(typeof(IUIService));
				if (iuiservice != null)
				{
					return iuiservice.GetDialogOwnerWindow();
				}
			}
			return null;
		}

		// Token: 0x060023BF RID: 9151 RVA: 0x000BFCCC File Offset: 0x000BECCC
		public static ToolStripRenderer GetToolStripRenderer(IServiceProvider serviceProvider)
		{
			if (serviceProvider != null)
			{
				IUIService iuiservice = (IUIService)serviceProvider.GetService(typeof(IUIService));
				if (iuiservice != null)
				{
					IDictionary styles = iuiservice.Styles;
					if (styles != null)
					{
						return (ToolStripRenderer)styles["VsRenderer"];
					}
				}
			}
			return null;
		}

		// Token: 0x060023C0 RID: 9152 RVA: 0x000BFD14 File Offset: 0x000BED14
		public static DialogResult ShowDialog(IServiceProvider serviceProvider, Form form)
		{
			if (serviceProvider != null)
			{
				IUIService iuiservice = (IUIService)serviceProvider.GetService(typeof(IUIService));
				if (iuiservice != null)
				{
					return iuiservice.ShowDialog(form);
				}
			}
			return form.ShowDialog();
		}

		// Token: 0x060023C1 RID: 9153 RVA: 0x000BFD4C File Offset: 0x000BED4C
		public static void ShowError(IServiceProvider serviceProvider, string message)
		{
			if (serviceProvider != null)
			{
				IUIService iuiservice = (IUIService)serviceProvider.GetService(typeof(IUIService));
				if (iuiservice != null)
				{
					iuiservice.ShowError(message);
					return;
				}
			}
			RTLAwareMessageBox.Show(null, message, SR.GetString("UIServiceHelper_ErrorCaption"), MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
		}

		// Token: 0x060023C2 RID: 9154 RVA: 0x000BFD94 File Offset: 0x000BED94
		public static void ShowError(IServiceProvider serviceProvider, Exception ex, string message)
		{
			if (ex != null)
			{
				message = message + Environment.NewLine + Environment.NewLine + ex.Message;
			}
			if (serviceProvider != null)
			{
				IUIService iuiservice = (IUIService)serviceProvider.GetService(typeof(IUIService));
				if (iuiservice != null)
				{
					iuiservice.ShowError(message);
					return;
				}
			}
			RTLAwareMessageBox.Show(null, message, SR.GetString("UIServiceHelper_ErrorCaption"), MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
		}

		// Token: 0x060023C3 RID: 9155 RVA: 0x000BFDF8 File Offset: 0x000BEDF8
		public static void ShowMessage(IServiceProvider serviceProvider, string message)
		{
			if (serviceProvider != null)
			{
				IUIService iuiservice = (IUIService)serviceProvider.GetService(typeof(IUIService));
				if (iuiservice != null)
				{
					iuiservice.ShowMessage(message);
					return;
				}
			}
			RTLAwareMessageBox.Show(null, message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
		}

		// Token: 0x060023C4 RID: 9156 RVA: 0x000BFE3C File Offset: 0x000BEE3C
		public static DialogResult ShowMessage(IServiceProvider serviceProvider, string message, string caption, MessageBoxButtons buttons)
		{
			if (serviceProvider != null)
			{
				IUIService iuiservice = (IUIService)serviceProvider.GetService(typeof(IUIService));
				if (iuiservice != null)
				{
					return iuiservice.ShowMessage(message, caption, buttons);
				}
			}
			return RTLAwareMessageBox.Show(null, message, caption, buttons, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
		}
	}
}
