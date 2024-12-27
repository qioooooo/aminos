using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x020004BF RID: 1215
	public class MessageBox
	{
		// Token: 0x0600489E RID: 18590 RVA: 0x00107CEC File Offset: 0x00106CEC
		private MessageBox()
		{
		}

		// Token: 0x0600489F RID: 18591 RVA: 0x00107CF4 File Offset: 0x00106CF4
		private static DialogResult Win32ToDialogResult(int value)
		{
			switch (value)
			{
			case 1:
				return DialogResult.OK;
			case 2:
				return DialogResult.Cancel;
			case 3:
				return DialogResult.Abort;
			case 4:
				return DialogResult.Retry;
			case 5:
				return DialogResult.Ignore;
			case 6:
				return DialogResult.Yes;
			case 7:
				return DialogResult.No;
			default:
				return DialogResult.No;
			}
		}

		// Token: 0x17000E84 RID: 3716
		// (get) Token: 0x060048A0 RID: 18592 RVA: 0x00107D38 File Offset: 0x00106D38
		internal static HelpInfo HelpInfo
		{
			get
			{
				if (MessageBox.helpInfoTable != null && MessageBox.helpInfoTable.Length > 0)
				{
					return MessageBox.helpInfoTable[MessageBox.helpInfoTable.Length - 1];
				}
				return null;
			}
		}

		// Token: 0x060048A1 RID: 18593 RVA: 0x00107D5C File Offset: 0x00106D5C
		private static void PopHelpInfo()
		{
			if (MessageBox.helpInfoTable == null)
			{
				return;
			}
			if (MessageBox.helpInfoTable.Length == 1)
			{
				MessageBox.helpInfoTable = null;
				return;
			}
			int num = MessageBox.helpInfoTable.Length - 1;
			HelpInfo[] array = new HelpInfo[num];
			Array.Copy(MessageBox.helpInfoTable, array, num);
			MessageBox.helpInfoTable = array;
		}

		// Token: 0x060048A2 RID: 18594 RVA: 0x00107DA8 File Offset: 0x00106DA8
		private static void PushHelpInfo(HelpInfo hpi)
		{
			int num = 0;
			HelpInfo[] array;
			if (MessageBox.helpInfoTable == null)
			{
				array = new HelpInfo[num + 1];
			}
			else
			{
				num = MessageBox.helpInfoTable.Length;
				array = new HelpInfo[num + 1];
				Array.Copy(MessageBox.helpInfoTable, array, num);
			}
			array[num] = hpi;
			MessageBox.helpInfoTable = array;
		}

		// Token: 0x060048A3 RID: 18595 RVA: 0x00107DF0 File Offset: 0x00106DF0
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, bool displayHelpButton)
		{
			return MessageBox.ShowCore(null, text, caption, buttons, icon, defaultButton, options, displayHelpButton);
		}

		// Token: 0x060048A4 RID: 18596 RVA: 0x00107E04 File Offset: 0x00106E04
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
		{
			HelpInfo helpInfo = new HelpInfo(helpFilePath);
			return MessageBox.ShowCore(null, text, caption, buttons, icon, defaultButton, options, helpInfo);
		}

		// Token: 0x060048A5 RID: 18597 RVA: 0x00107E28 File Offset: 0x00106E28
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
		{
			HelpInfo helpInfo = new HelpInfo(helpFilePath);
			return MessageBox.ShowCore(owner, text, caption, buttons, icon, defaultButton, options, helpInfo);
		}

		// Token: 0x060048A6 RID: 18598 RVA: 0x00107E50 File Offset: 0x00106E50
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
		{
			HelpInfo helpInfo = new HelpInfo(helpFilePath, keyword);
			return MessageBox.ShowCore(null, text, caption, buttons, icon, defaultButton, options, helpInfo);
		}

		// Token: 0x060048A7 RID: 18599 RVA: 0x00107E78 File Offset: 0x00106E78
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
		{
			HelpInfo helpInfo = new HelpInfo(helpFilePath, keyword);
			return MessageBox.ShowCore(owner, text, caption, buttons, icon, defaultButton, options, helpInfo);
		}

		// Token: 0x060048A8 RID: 18600 RVA: 0x00107EA0 File Offset: 0x00106EA0
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
		{
			HelpInfo helpInfo = new HelpInfo(helpFilePath, navigator);
			return MessageBox.ShowCore(null, text, caption, buttons, icon, defaultButton, options, helpInfo);
		}

		// Token: 0x060048A9 RID: 18601 RVA: 0x00107EC8 File Offset: 0x00106EC8
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
		{
			HelpInfo helpInfo = new HelpInfo(helpFilePath, navigator);
			return MessageBox.ShowCore(owner, text, caption, buttons, icon, defaultButton, options, helpInfo);
		}

		// Token: 0x060048AA RID: 18602 RVA: 0x00107EF0 File Offset: 0x00106EF0
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param)
		{
			HelpInfo helpInfo = new HelpInfo(helpFilePath, navigator, param);
			return MessageBox.ShowCore(null, text, caption, buttons, icon, defaultButton, options, helpInfo);
		}

		// Token: 0x060048AB RID: 18603 RVA: 0x00107F18 File Offset: 0x00106F18
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param)
		{
			HelpInfo helpInfo = new HelpInfo(helpFilePath, navigator, param);
			return MessageBox.ShowCore(owner, text, caption, buttons, icon, defaultButton, options, helpInfo);
		}

		// Token: 0x060048AC RID: 18604 RVA: 0x00107F41 File Offset: 0x00106F41
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
		{
			return MessageBox.ShowCore(null, text, caption, buttons, icon, defaultButton, options, false);
		}

		// Token: 0x060048AD RID: 18605 RVA: 0x00107F52 File Offset: 0x00106F52
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
		{
			return MessageBox.ShowCore(null, text, caption, buttons, icon, defaultButton, (MessageBoxOptions)0, false);
		}

		// Token: 0x060048AE RID: 18606 RVA: 0x00107F62 File Offset: 0x00106F62
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			return MessageBox.ShowCore(null, text, caption, buttons, icon, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0, false);
		}

		// Token: 0x060048AF RID: 18607 RVA: 0x00107F71 File Offset: 0x00106F71
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
		{
			return MessageBox.ShowCore(null, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0, false);
		}

		// Token: 0x060048B0 RID: 18608 RVA: 0x00107F80 File Offset: 0x00106F80
		public static DialogResult Show(string text, string caption)
		{
			return MessageBox.ShowCore(null, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0, false);
		}

		// Token: 0x060048B1 RID: 18609 RVA: 0x00107F8F File Offset: 0x00106F8F
		public static DialogResult Show(string text)
		{
			return MessageBox.ShowCore(null, text, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0, false);
		}

		// Token: 0x060048B2 RID: 18610 RVA: 0x00107FA2 File Offset: 0x00106FA2
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
		{
			return MessageBox.ShowCore(owner, text, caption, buttons, icon, defaultButton, options, false);
		}

		// Token: 0x060048B3 RID: 18611 RVA: 0x00107FB4 File Offset: 0x00106FB4
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
		{
			return MessageBox.ShowCore(owner, text, caption, buttons, icon, defaultButton, (MessageBoxOptions)0, false);
		}

		// Token: 0x060048B4 RID: 18612 RVA: 0x00107FC5 File Offset: 0x00106FC5
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			return MessageBox.ShowCore(owner, text, caption, buttons, icon, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0, false);
		}

		// Token: 0x060048B5 RID: 18613 RVA: 0x00107FD5 File Offset: 0x00106FD5
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
		{
			return MessageBox.ShowCore(owner, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0, false);
		}

		// Token: 0x060048B6 RID: 18614 RVA: 0x00107FE4 File Offset: 0x00106FE4
		public static DialogResult Show(IWin32Window owner, string text, string caption)
		{
			return MessageBox.ShowCore(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0, false);
		}

		// Token: 0x060048B7 RID: 18615 RVA: 0x00107FF3 File Offset: 0x00106FF3
		public static DialogResult Show(IWin32Window owner, string text)
		{
			return MessageBox.ShowCore(owner, text, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0, false);
		}

		// Token: 0x060048B8 RID: 18616 RVA: 0x00108008 File Offset: 0x00107008
		private static DialogResult ShowCore(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, HelpInfo hpi)
		{
			DialogResult dialogResult = DialogResult.None;
			try
			{
				MessageBox.PushHelpInfo(hpi);
				dialogResult = MessageBox.ShowCore(owner, text, caption, buttons, icon, defaultButton, options, true);
			}
			finally
			{
				MessageBox.PopHelpInfo();
			}
			return dialogResult;
		}

		// Token: 0x060048B9 RID: 18617 RVA: 0x00108048 File Offset: 0x00107048
		private static DialogResult ShowCore(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, bool showHelp)
		{
			if (!ClientUtils.IsEnumValid(buttons, (int)buttons, 0, 5))
			{
				throw new InvalidEnumArgumentException("buttons", (int)buttons, typeof(MessageBoxButtons));
			}
			if (!WindowsFormsUtils.EnumValidator.IsEnumWithinShiftedRange(icon, 4, 0, 4))
			{
				throw new InvalidEnumArgumentException("icon", (int)icon, typeof(MessageBoxIcon));
			}
			if (!WindowsFormsUtils.EnumValidator.IsEnumWithinShiftedRange(defaultButton, 8, 0, 2))
			{
				throw new InvalidEnumArgumentException("defaultButton", (int)defaultButton, typeof(DialogResult));
			}
			if (!SystemInformation.UserInteractive && (options & (MessageBoxOptions.ServiceNotification | MessageBoxOptions.DefaultDesktopOnly)) == (MessageBoxOptions)0)
			{
				throw new InvalidOperationException(SR.GetString("CantShowModalOnNonInteractive"));
			}
			if (owner != null && (options & (MessageBoxOptions.ServiceNotification | MessageBoxOptions.DefaultDesktopOnly)) != (MessageBoxOptions)0)
			{
				throw new ArgumentException(SR.GetString("CantShowMBServiceWithOwner"), "options");
			}
			if (showHelp && (options & (MessageBoxOptions.ServiceNotification | MessageBoxOptions.DefaultDesktopOnly)) != (MessageBoxOptions)0)
			{
				throw new ArgumentException(SR.GetString("CantShowMBServiceWithHelp"), "options");
			}
			if ((options & ~(MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading)) != (MessageBoxOptions)0)
			{
				IntSecurity.UnmanagedCode.Demand();
			}
			IntSecurity.SafeSubWindows.Demand();
			int num = (showHelp ? 16384 : 0);
			num |= (int)(buttons | (MessageBoxButtons)icon | (MessageBoxButtons)defaultButton | (MessageBoxButtons)options);
			IntPtr intPtr = IntPtr.Zero;
			if (showHelp || (options & (MessageBoxOptions.ServiceNotification | MessageBoxOptions.DefaultDesktopOnly)) == (MessageBoxOptions)0)
			{
				if (owner == null)
				{
					intPtr = UnsafeNativeMethods.GetActiveWindow();
				}
				else
				{
					intPtr = Control.GetSafeHandle(owner);
				}
			}
			IntPtr intPtr2 = IntPtr.Zero;
			if (Application.UseVisualStyles)
			{
				intPtr2 = UnsafeNativeMethods.ThemingScope.Activate();
			}
			Application.BeginModalMessageLoop();
			DialogResult dialogResult;
			try
			{
				dialogResult = MessageBox.Win32ToDialogResult(SafeNativeMethods.MessageBox(new HandleRef(owner, intPtr), text, caption, num));
			}
			finally
			{
				Application.EndModalMessageLoop();
				UnsafeNativeMethods.ThemingScope.Deactivate(intPtr2);
			}
			UnsafeNativeMethods.SendMessage(new HandleRef(owner, intPtr), 7, 0, 0);
			return dialogResult;
		}

		// Token: 0x04002250 RID: 8784
		private const int IDOK = 1;

		// Token: 0x04002251 RID: 8785
		private const int IDCANCEL = 2;

		// Token: 0x04002252 RID: 8786
		private const int IDABORT = 3;

		// Token: 0x04002253 RID: 8787
		private const int IDRETRY = 4;

		// Token: 0x04002254 RID: 8788
		private const int IDIGNORE = 5;

		// Token: 0x04002255 RID: 8789
		private const int IDYES = 6;

		// Token: 0x04002256 RID: 8790
		private const int IDNO = 7;

		// Token: 0x04002257 RID: 8791
		private const int HELP_BUTTON = 16384;

		// Token: 0x04002258 RID: 8792
		[ThreadStatic]
		private static HelpInfo[] helpInfoTable;
	}
}
