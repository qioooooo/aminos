using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Drawing.Design
{
	// Token: 0x02000026 RID: 38
	[SuppressUnmanagedCodeSecurity]
	internal class UnsafeNativeMethods
	{
		// Token: 0x06000118 RID: 280 RVA: 0x00007A2C File Offset: 0x00006A2C
		private UnsafeNativeMethods()
		{
		}

		// Token: 0x06000119 RID: 281
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int ClientToScreen(HandleRef hWnd, [In] [Out] NativeMethods.POINT pt);

		// Token: 0x0600011A RID: 282
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int ScreenToClient(HandleRef hWnd, [In] [Out] NativeMethods.POINT pt);

		// Token: 0x0600011B RID: 283
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr SetFocus(HandleRef hWnd);

		// Token: 0x0600011C RID: 284
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetFocus();

		// Token: 0x0600011D RID: 285
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern void NotifyWinEvent(int winEvent, HandleRef hwnd, int objType, int objID);

		// Token: 0x040000F5 RID: 245
		public const int OBJID_CLIENT = -4;
	}
}
