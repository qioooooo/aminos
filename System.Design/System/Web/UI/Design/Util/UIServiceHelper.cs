using System;
using System.Collections;
using System.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Web.UI.Design.Util
{
	internal static class UIServiceHelper
	{
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
