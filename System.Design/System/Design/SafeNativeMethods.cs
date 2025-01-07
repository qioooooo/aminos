using System;
using System.Internal;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Design
{
	[SuppressUnmanagedCodeSecurity]
	internal static class SafeNativeMethods
	{
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool DeleteObject(HandleRef hObject);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetMessagePos();

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int RegisterWindowMessage(string msg);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetTextMetrics(HandleRef hdc, NativeMethods.TEXTMETRIC tm);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr CreateSolidBrush(int crColor);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetWindowTextLength(HandleRef hWnd);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetTickCount();

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool RedrawWindow(IntPtr hwnd, NativeMethods.COMRECT rcUpdate, IntPtr hrgnUpdate, int flags);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int flags);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int DrawText(HandleRef hDC, string lpszString, int nCount, ref NativeMethods.RECT lpRect, int nFormat);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr SelectObject(HandleRef hDC, HandleRef hObject);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool IsChild(HandleRef parent, HandleRef child);

		[DllImport("comctl32.dll", ExactSpelling = true)]
		private static extern bool _TrackMouseEvent(NativeMethods.TRACKMOUSEEVENT tme);

		public static bool TrackMouseEvent(NativeMethods.TRACKMOUSEEVENT tme)
		{
			return SafeNativeMethods._TrackMouseEvent(tme);
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetCurrentProcessId();

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool RoundRect(HandleRef hDC, int left, int top, int right, int bottom, int width, int height);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool Rectangle(HandleRef hdc, int left, int top, int right, int bottom);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreatePen", ExactSpelling = true)]
		private static extern IntPtr IntCreatePen(int nStyle, int nWidth, int crColor);

		public static IntPtr CreatePen(int nStyle, int nWidth, int crColor)
		{
			return global::System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreatePen(nStyle, nWidth, crColor), NativeMethods.CommonHandles.GDI);
		}

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetROP2(HandleRef hDC, int nDrawMode);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetBkColor(HandleRef hDC, int clr);
	}
}
