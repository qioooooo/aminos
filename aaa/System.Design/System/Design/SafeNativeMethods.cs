using System;
using System.Internal;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Design
{
	// Token: 0x02000059 RID: 89
	[SuppressUnmanagedCodeSecurity]
	internal static class SafeNativeMethods
	{
		// Token: 0x060003C5 RID: 965
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool DeleteObject(HandleRef hObject);

		// Token: 0x060003C6 RID: 966
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetMessagePos();

		// Token: 0x060003C7 RID: 967
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int RegisterWindowMessage(string msg);

		// Token: 0x060003C8 RID: 968
		[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetTextMetrics(HandleRef hdc, NativeMethods.TEXTMETRIC tm);

		// Token: 0x060003C9 RID: 969
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

		// Token: 0x060003CA RID: 970
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr CreateSolidBrush(int crColor);

		// Token: 0x060003CB RID: 971
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetWindowTextLength(HandleRef hWnd);

		// Token: 0x060003CC RID: 972
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetTickCount();

		// Token: 0x060003CD RID: 973
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool RedrawWindow(IntPtr hwnd, NativeMethods.COMRECT rcUpdate, IntPtr hrgnUpdate, int flags);

		// Token: 0x060003CE RID: 974
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int flags);

		// Token: 0x060003CF RID: 975
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int DrawText(HandleRef hDC, string lpszString, int nCount, ref NativeMethods.RECT lpRect, int nFormat);

		// Token: 0x060003D0 RID: 976
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr SelectObject(HandleRef hDC, HandleRef hObject);

		// Token: 0x060003D1 RID: 977
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool IsChild(HandleRef parent, HandleRef child);

		// Token: 0x060003D2 RID: 978
		[DllImport("comctl32.dll", ExactSpelling = true)]
		private static extern bool _TrackMouseEvent(NativeMethods.TRACKMOUSEEVENT tme);

		// Token: 0x060003D3 RID: 979 RVA: 0x00002E6A File Offset: 0x00001E6A
		public static bool TrackMouseEvent(NativeMethods.TRACKMOUSEEVENT tme)
		{
			return SafeNativeMethods._TrackMouseEvent(tme);
		}

		// Token: 0x060003D4 RID: 980
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetCurrentProcessId();

		// Token: 0x060003D5 RID: 981
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool RoundRect(HandleRef hDC, int left, int top, int right, int bottom, int width, int height);

		// Token: 0x060003D6 RID: 982
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool Rectangle(HandleRef hdc, int left, int top, int right, int bottom);

		// Token: 0x060003D7 RID: 983
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreatePen", ExactSpelling = true)]
		private static extern IntPtr IntCreatePen(int nStyle, int nWidth, int crColor);

		// Token: 0x060003D8 RID: 984 RVA: 0x00002E72 File Offset: 0x00001E72
		public static IntPtr CreatePen(int nStyle, int nWidth, int crColor)
		{
			return global::System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreatePen(nStyle, nWidth, crColor), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x060003D9 RID: 985
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetROP2(HandleRef hDC, int nDrawMode);

		// Token: 0x060003DA RID: 986
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetBkColor(HandleRef hDC, int clr);
	}
}
