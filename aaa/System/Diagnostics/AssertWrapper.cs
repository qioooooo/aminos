using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x020001B7 RID: 439
	internal static class AssertWrapper
	{
		// Token: 0x06000D65 RID: 3429 RVA: 0x0002AE27 File Offset: 0x00029E27
		public static void ShowAssert(string stackTrace, StackFrame frame, string message, string detailMessage)
		{
			AssertWrapper.ShowMessageBoxAssert(stackTrace, message, detailMessage);
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x0002AE34 File Offset: 0x00029E34
		private static void ShowMessageBoxAssert(string stackTrace, string message, string detailMessage)
		{
			string text = string.Concat(new string[] { message, "\r\n", detailMessage, "\r\n", stackTrace });
			text = AssertWrapper.TruncateMessageToFitScreen(text);
			int num = 262674;
			if (!Environment.UserInteractive)
			{
				num |= 2097152;
			}
			if (AssertWrapper.IsRTLResources)
			{
				num = num | 524288 | 1048576;
			}
			switch (SafeNativeMethods.MessageBox(NativeMethods.NullHandleRef, text, SR.GetString("DebugAssertTitle"), num))
			{
			case 3:
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
				try
				{
					Environment.Exit(1);
					return;
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				break;
			case 4:
				break;
			default:
				return;
			}
			if (!Debugger.IsAttached)
			{
				Debugger.Launch();
			}
			Debugger.Break();
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000D67 RID: 3431 RVA: 0x0002AF04 File Offset: 0x00029F04
		private static bool IsRTLResources
		{
			get
			{
				return SR.GetString("RTL") != "RTL_False";
			}
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x0002AF1C File Offset: 0x00029F1C
		private static string TruncateMessageToFitScreen(string message)
		{
			IntPtr intPtr = SafeNativeMethods.GetStockObject(17);
			IntPtr intPtr2 = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
			NativeMethods.TEXTMETRIC textmetric = new NativeMethods.TEXTMETRIC();
			intPtr = UnsafeNativeMethods.SelectObject(new HandleRef(null, intPtr2), new HandleRef(null, intPtr));
			SafeNativeMethods.GetTextMetrics(new HandleRef(null, intPtr2), textmetric);
			UnsafeNativeMethods.SelectObject(new HandleRef(null, intPtr2), new HandleRef(null, intPtr));
			UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, intPtr2));
			intPtr2 = IntPtr.Zero;
			int systemMetrics = UnsafeNativeMethods.GetSystemMetrics(1);
			int num = systemMetrics / textmetric.tmHeight - 15;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			while (num2 < num && num4 < message.Length - 1)
			{
				char c = message[num4];
				num3++;
				if (c == '\n' || c == '\r' || num3 > 80)
				{
					num2++;
					num3 = 0;
				}
				if (c == '\r' && message[num4 + 1] == '\n')
				{
					num4 += 2;
				}
				else if (c == '\n' && message[num4 + 1] == '\r')
				{
					num4 += 2;
				}
				else
				{
					num4++;
				}
			}
			if (num4 < message.Length - 1)
			{
				message = SR.GetString("DebugMessageTruncated", new object[] { message.Substring(0, num4) });
			}
			return message;
		}
	}
}
