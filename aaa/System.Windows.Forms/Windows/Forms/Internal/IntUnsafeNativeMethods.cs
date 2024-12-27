using System;
using System.Internal;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Windows.Forms.Internal
{
	// Token: 0x02000025 RID: 37
	[SuppressUnmanagedCodeSecurity]
	internal static class IntUnsafeNativeMethods
	{
		// Token: 0x06000071 RID: 113
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetDC", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntGetDC(HandleRef hWnd);

		// Token: 0x06000072 RID: 114 RVA: 0x000033E0 File Offset: 0x000023E0
		public static IntPtr GetDC(HandleRef hWnd)
		{
			return global::System.Internal.HandleCollector.Add(IntUnsafeNativeMethods.IntGetDC(hWnd), IntSafeNativeMethods.CommonHandles.HDC);
		}

		// Token: 0x06000073 RID: 115
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "DeleteDC", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntDeleteDC(HandleRef hDC);

		// Token: 0x06000074 RID: 116 RVA: 0x00003400 File Offset: 0x00002400
		public static bool DeleteDC(HandleRef hDC)
		{
			global::System.Internal.HandleCollector.Remove((IntPtr)hDC, IntSafeNativeMethods.CommonHandles.GDI);
			return IntUnsafeNativeMethods.IntDeleteDC(hDC);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003428 File Offset: 0x00002428
		public static bool DeleteHDC(HandleRef hDC)
		{
			global::System.Internal.HandleCollector.Remove((IntPtr)hDC, IntSafeNativeMethods.CommonHandles.HDC);
			return IntUnsafeNativeMethods.IntDeleteDC(hDC);
		}

		// Token: 0x06000076 RID: 118
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "ReleaseDC", ExactSpelling = true, SetLastError = true)]
		public static extern int IntReleaseDC(HandleRef hWnd, HandleRef hDC);

		// Token: 0x06000077 RID: 119 RVA: 0x0000344E File Offset: 0x0000244E
		public static int ReleaseDC(HandleRef hWnd, HandleRef hDC)
		{
			global::System.Internal.HandleCollector.Remove((IntPtr)hDC, IntSafeNativeMethods.CommonHandles.HDC);
			return IntUnsafeNativeMethods.IntReleaseDC(hWnd, hDC);
		}

		// Token: 0x06000078 RID: 120
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateDC", SetLastError = true)]
		public static extern IntPtr IntCreateDC(string lpszDriverName, string lpszDeviceName, string lpszOutput, HandleRef lpInitData);

		// Token: 0x06000079 RID: 121 RVA: 0x00003468 File Offset: 0x00002468
		public static IntPtr CreateDC(string lpszDriverName, string lpszDeviceName, string lpszOutput, HandleRef lpInitData)
		{
			return global::System.Internal.HandleCollector.Add(IntUnsafeNativeMethods.IntCreateDC(lpszDriverName, lpszDeviceName, lpszOutput, lpInitData), IntSafeNativeMethods.CommonHandles.HDC);
		}

		// Token: 0x0600007A RID: 122
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateIC", SetLastError = true)]
		public static extern IntPtr IntCreateIC(string lpszDriverName, string lpszDeviceName, string lpszOutput, HandleRef lpInitData);

		// Token: 0x0600007B RID: 123 RVA: 0x0000348C File Offset: 0x0000248C
		public static IntPtr CreateIC(string lpszDriverName, string lpszDeviceName, string lpszOutput, HandleRef lpInitData)
		{
			return global::System.Internal.HandleCollector.Add(IntUnsafeNativeMethods.IntCreateIC(lpszDriverName, lpszDeviceName, lpszOutput, lpInitData), IntSafeNativeMethods.CommonHandles.HDC);
		}

		// Token: 0x0600007C RID: 124
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateCompatibleDC", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntCreateCompatibleDC(HandleRef hDC);

		// Token: 0x0600007D RID: 125 RVA: 0x000034B0 File Offset: 0x000024B0
		public static IntPtr CreateCompatibleDC(HandleRef hDC)
		{
			return global::System.Internal.HandleCollector.Add(IntUnsafeNativeMethods.IntCreateCompatibleDC(hDC), IntSafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x0600007E RID: 126
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SaveDC", ExactSpelling = true, SetLastError = true)]
		public static extern int IntSaveDC(HandleRef hDC);

		// Token: 0x0600007F RID: 127 RVA: 0x000034D0 File Offset: 0x000024D0
		public static int SaveDC(HandleRef hDC)
		{
			return IntUnsafeNativeMethods.IntSaveDC(hDC);
		}

		// Token: 0x06000080 RID: 128
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "RestoreDC", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntRestoreDC(HandleRef hDC, int nSavedDC);

		// Token: 0x06000081 RID: 129 RVA: 0x000034E8 File Offset: 0x000024E8
		public static bool RestoreDC(HandleRef hDC, int nSavedDC)
		{
			return IntUnsafeNativeMethods.IntRestoreDC(hDC, nSavedDC);
		}

		// Token: 0x06000082 RID: 130
		[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr WindowFromDC(HandleRef hDC);

		// Token: 0x06000083 RID: 131
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetDeviceCaps(HandleRef hDC, int nIndex);

		// Token: 0x06000084 RID: 132
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "OffsetViewportOrgEx", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntOffsetViewportOrgEx(HandleRef hDC, int nXOffset, int nYOffset, [In] [Out] IntNativeMethods.POINT point);

		// Token: 0x06000085 RID: 133 RVA: 0x00003500 File Offset: 0x00002500
		public static bool OffsetViewportOrgEx(HandleRef hDC, int nXOffset, int nYOffset, [In] [Out] IntNativeMethods.POINT point)
		{
			return IntUnsafeNativeMethods.IntOffsetViewportOrgEx(hDC, nXOffset, nYOffset, point);
		}

		// Token: 0x06000086 RID: 134
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SetGraphicsMode", ExactSpelling = true, SetLastError = true)]
		public static extern int IntSetGraphicsMode(HandleRef hDC, int iMode);

		// Token: 0x06000087 RID: 135 RVA: 0x00003518 File Offset: 0x00002518
		public static int SetGraphicsMode(HandleRef hDC, int iMode)
		{
			iMode = IntUnsafeNativeMethods.IntSetGraphicsMode(hDC, iMode);
			return iMode;
		}

		// Token: 0x06000088 RID: 136
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetGraphicsMode(HandleRef hDC);

		// Token: 0x06000089 RID: 137
		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern int GetROP2(HandleRef hdc);

		// Token: 0x0600008A RID: 138
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetROP2(HandleRef hDC, int nDrawMode);

		// Token: 0x0600008B RID: 139
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CombineRgn", ExactSpelling = true, SetLastError = true)]
		public static extern IntNativeMethods.RegionFlags IntCombineRgn(HandleRef hRgnDest, HandleRef hRgnSrc1, HandleRef hRgnSrc2, RegionCombineMode combineMode);

		// Token: 0x0600008C RID: 140 RVA: 0x00003524 File Offset: 0x00002524
		public static IntNativeMethods.RegionFlags CombineRgn(HandleRef hRgnDest, HandleRef hRgnSrc1, HandleRef hRgnSrc2, RegionCombineMode combineMode)
		{
			if (hRgnDest.Wrapper == null || hRgnSrc1.Wrapper == null || hRgnSrc2.Wrapper == null)
			{
				return IntNativeMethods.RegionFlags.ERROR;
			}
			return IntUnsafeNativeMethods.IntCombineRgn(hRgnDest, hRgnSrc1, hRgnSrc2, combineMode);
		}

		// Token: 0x0600008D RID: 141
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetClipRgn", ExactSpelling = true, SetLastError = true)]
		public static extern int IntGetClipRgn(HandleRef hDC, HandleRef hRgn);

		// Token: 0x0600008E RID: 142 RVA: 0x0000354C File Offset: 0x0000254C
		public static int GetClipRgn(HandleRef hDC, HandleRef hRgn)
		{
			return IntUnsafeNativeMethods.IntGetClipRgn(hDC, hRgn);
		}

		// Token: 0x0600008F RID: 143
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SelectClipRgn", ExactSpelling = true, SetLastError = true)]
		public static extern IntNativeMethods.RegionFlags IntSelectClipRgn(HandleRef hDC, HandleRef hRgn);

		// Token: 0x06000090 RID: 144 RVA: 0x00003564 File Offset: 0x00002564
		public static IntNativeMethods.RegionFlags SelectClipRgn(HandleRef hDC, HandleRef hRgn)
		{
			return IntUnsafeNativeMethods.IntSelectClipRgn(hDC, hRgn);
		}

		// Token: 0x06000091 RID: 145
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetRgnBox", ExactSpelling = true, SetLastError = true)]
		public static extern IntNativeMethods.RegionFlags IntGetRgnBox(HandleRef hRgn, [In] [Out] ref IntNativeMethods.RECT clipRect);

		// Token: 0x06000092 RID: 146 RVA: 0x0000357C File Offset: 0x0000257C
		public static IntNativeMethods.RegionFlags GetRgnBox(HandleRef hRgn, [In] [Out] ref IntNativeMethods.RECT clipRect)
		{
			return IntUnsafeNativeMethods.IntGetRgnBox(hRgn, ref clipRect);
		}

		// Token: 0x06000093 RID: 147
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateFontIndirect", SetLastError = true)]
		public static extern IntPtr IntCreateFontIndirect([MarshalAs(UnmanagedType.AsAny)] [In] [Out] object lf);

		// Token: 0x06000094 RID: 148 RVA: 0x00003594 File Offset: 0x00002594
		public static IntPtr CreateFontIndirect(object lf)
		{
			return global::System.Internal.HandleCollector.Add(IntUnsafeNativeMethods.IntCreateFontIndirect(lf), IntSafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06000095 RID: 149
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "DeleteObject", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntDeleteObject(HandleRef hObject);

		// Token: 0x06000096 RID: 150 RVA: 0x000035B4 File Offset: 0x000025B4
		public static bool DeleteObject(HandleRef hObject)
		{
			global::System.Internal.HandleCollector.Remove((IntPtr)hObject, IntSafeNativeMethods.CommonHandles.GDI);
			return IntUnsafeNativeMethods.IntDeleteObject(hObject);
		}

		// Token: 0x06000097 RID: 151
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetObject", SetLastError = true)]
		public static extern int IntGetObject(HandleRef hBrush, int nSize, [In] [Out] IntNativeMethods.LOGBRUSH lb);

		// Token: 0x06000098 RID: 152 RVA: 0x000035DC File Offset: 0x000025DC
		public static int GetObject(HandleRef hBrush, IntNativeMethods.LOGBRUSH lb)
		{
			return IntUnsafeNativeMethods.IntGetObject(hBrush, Marshal.SizeOf(typeof(IntNativeMethods.LOGBRUSH)), lb);
		}

		// Token: 0x06000099 RID: 153
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetObject", SetLastError = true)]
		public static extern int IntGetObject(HandleRef hFont, int nSize, [In] [Out] IntNativeMethods.LOGFONT lf);

		// Token: 0x0600009A RID: 154 RVA: 0x00003604 File Offset: 0x00002604
		public static int GetObject(HandleRef hFont, IntNativeMethods.LOGFONT lp)
		{
			return IntUnsafeNativeMethods.IntGetObject(hFont, Marshal.SizeOf(typeof(IntNativeMethods.LOGFONT)), lp);
		}

		// Token: 0x0600009B RID: 155
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SelectObject", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntSelectObject(HandleRef hdc, HandleRef obj);

		// Token: 0x0600009C RID: 156 RVA: 0x0000362C File Offset: 0x0000262C
		public static IntPtr SelectObject(HandleRef hdc, HandleRef obj)
		{
			return IntUnsafeNativeMethods.IntSelectObject(hdc, obj);
		}

		// Token: 0x0600009D RID: 157
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetCurrentObject", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntGetCurrentObject(HandleRef hDC, int uObjectType);

		// Token: 0x0600009E RID: 158 RVA: 0x00003644 File Offset: 0x00002644
		public static IntPtr GetCurrentObject(HandleRef hDC, int uObjectType)
		{
			return IntUnsafeNativeMethods.IntGetCurrentObject(hDC, uObjectType);
		}

		// Token: 0x0600009F RID: 159
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetStockObject", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntGetStockObject(int nIndex);

		// Token: 0x060000A0 RID: 160 RVA: 0x0000365C File Offset: 0x0000265C
		public static IntPtr GetStockObject(int nIndex)
		{
			return IntUnsafeNativeMethods.IntGetStockObject(nIndex);
		}

		// Token: 0x060000A1 RID: 161
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetNearestColor(HandleRef hDC, int color);

		// Token: 0x060000A2 RID: 162
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetTextColor(HandleRef hDC, int crColor);

		// Token: 0x060000A3 RID: 163
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetTextAlign(HandleRef hdc);

		// Token: 0x060000A4 RID: 164
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetTextColor(HandleRef hDC);

		// Token: 0x060000A5 RID: 165
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetBkColor(HandleRef hDC, int clr);

		// Token: 0x060000A6 RID: 166
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SetBkMode", ExactSpelling = true, SetLastError = true)]
		public static extern int IntSetBkMode(HandleRef hDC, int nBkMode);

		// Token: 0x060000A7 RID: 167 RVA: 0x00003674 File Offset: 0x00002674
		public static int SetBkMode(HandleRef hDC, int nBkMode)
		{
			return IntUnsafeNativeMethods.IntSetBkMode(hDC, nBkMode);
		}

		// Token: 0x060000A8 RID: 168
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetBkMode", ExactSpelling = true, SetLastError = true)]
		public static extern int IntGetBkMode(HandleRef hDC);

		// Token: 0x060000A9 RID: 169 RVA: 0x0000368C File Offset: 0x0000268C
		public static int GetBkMode(HandleRef hDC)
		{
			return IntUnsafeNativeMethods.IntGetBkMode(hDC);
		}

		// Token: 0x060000AA RID: 170
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetBkColor(HandleRef hDC);

		// Token: 0x060000AB RID: 171
		[DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		public static extern int DrawTextW(HandleRef hDC, string lpszString, int nCount, ref IntNativeMethods.RECT lpRect, int nFormat);

		// Token: 0x060000AC RID: 172
		[DllImport("user32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern int DrawTextA(HandleRef hDC, byte[] lpszString, int byteCount, ref IntNativeMethods.RECT lpRect, int nFormat);

		// Token: 0x060000AD RID: 173 RVA: 0x000036A4 File Offset: 0x000026A4
		public static int DrawText(HandleRef hDC, string text, ref IntNativeMethods.RECT lpRect, int nFormat)
		{
			int num2;
			if (Marshal.SystemDefaultCharSize == 1)
			{
				lpRect.top = Math.Min(32767, lpRect.top);
				lpRect.left = Math.Min(32767, lpRect.left);
				lpRect.right = Math.Min(32767, lpRect.right);
				lpRect.bottom = Math.Min(32767, lpRect.bottom);
				int num = IntUnsafeNativeMethods.WideCharToMultiByte(0, 0, text, text.Length, null, 0, IntPtr.Zero, IntPtr.Zero);
				byte[] array = new byte[num];
				IntUnsafeNativeMethods.WideCharToMultiByte(0, 0, text, text.Length, array, array.Length, IntPtr.Zero, IntPtr.Zero);
				num = Math.Min(num, 8192);
				num2 = IntUnsafeNativeMethods.DrawTextA(hDC, array, num, ref lpRect, nFormat);
			}
			else
			{
				num2 = IntUnsafeNativeMethods.DrawTextW(hDC, text, text.Length, ref lpRect, nFormat);
			}
			return num2;
		}

		// Token: 0x060000AE RID: 174
		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int DrawTextExW(HandleRef hDC, string lpszString, int nCount, ref IntNativeMethods.RECT lpRect, int nFormat, [In] [Out] IntNativeMethods.DRAWTEXTPARAMS lpDTParams);

		// Token: 0x060000AF RID: 175
		[DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern int DrawTextExA(HandleRef hDC, byte[] lpszString, int byteCount, ref IntNativeMethods.RECT lpRect, int nFormat, [In] [Out] IntNativeMethods.DRAWTEXTPARAMS lpDTParams);

		// Token: 0x060000B0 RID: 176 RVA: 0x00003780 File Offset: 0x00002780
		public static int DrawTextEx(HandleRef hDC, string text, ref IntNativeMethods.RECT lpRect, int nFormat, [In] [Out] IntNativeMethods.DRAWTEXTPARAMS lpDTParams)
		{
			int num2;
			if (Marshal.SystemDefaultCharSize == 1)
			{
				lpRect.top = Math.Min(32767, lpRect.top);
				lpRect.left = Math.Min(32767, lpRect.left);
				lpRect.right = Math.Min(32767, lpRect.right);
				lpRect.bottom = Math.Min(32767, lpRect.bottom);
				int num = IntUnsafeNativeMethods.WideCharToMultiByte(0, 0, text, text.Length, null, 0, IntPtr.Zero, IntPtr.Zero);
				byte[] array = new byte[num];
				IntUnsafeNativeMethods.WideCharToMultiByte(0, 0, text, text.Length, array, array.Length, IntPtr.Zero, IntPtr.Zero);
				num = Math.Min(num, 8192);
				num2 = IntUnsafeNativeMethods.DrawTextExA(hDC, array, num, ref lpRect, nFormat, lpDTParams);
			}
			else
			{
				num2 = IntUnsafeNativeMethods.DrawTextExW(hDC, text, text.Length, ref lpRect, nFormat, lpDTParams);
			}
			return num2;
		}

		// Token: 0x060000B1 RID: 177
		[DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		public static extern int GetTextExtentPoint32W(HandleRef hDC, string text, int len, [In] [Out] IntNativeMethods.SIZE size);

		// Token: 0x060000B2 RID: 178
		[DllImport("gdi32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern int GetTextExtentPoint32A(HandleRef hDC, byte[] lpszString, int byteCount, [In] [Out] IntNativeMethods.SIZE size);

		// Token: 0x060000B3 RID: 179 RVA: 0x00003860 File Offset: 0x00002860
		public static int GetTextExtentPoint32(HandleRef hDC, string text, [In] [Out] IntNativeMethods.SIZE size)
		{
			int num = text.Length;
			int num2;
			if (Marshal.SystemDefaultCharSize == 1)
			{
				num = IntUnsafeNativeMethods.WideCharToMultiByte(0, 0, text, text.Length, null, 0, IntPtr.Zero, IntPtr.Zero);
				byte[] array = new byte[num];
				IntUnsafeNativeMethods.WideCharToMultiByte(0, 0, text, text.Length, array, array.Length, IntPtr.Zero, IntPtr.Zero);
				num = Math.Min(text.Length, 8192);
				num2 = IntUnsafeNativeMethods.GetTextExtentPoint32A(hDC, array, num, size);
			}
			else
			{
				num2 = IntUnsafeNativeMethods.GetTextExtentPoint32W(hDC, text, text.Length, size);
			}
			return num2;
		}

		// Token: 0x060000B4 RID: 180
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool ExtTextOut(HandleRef hdc, int x, int y, int options, ref IntNativeMethods.RECT rect, string str, int length, int[] spacing);

		// Token: 0x060000B5 RID: 181
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "LineTo", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntLineTo(HandleRef hdc, int x, int y);

		// Token: 0x060000B6 RID: 182 RVA: 0x000038E8 File Offset: 0x000028E8
		public static bool LineTo(HandleRef hdc, int x, int y)
		{
			return IntUnsafeNativeMethods.IntLineTo(hdc, x, y);
		}

		// Token: 0x060000B7 RID: 183
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "MoveToEx", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntMoveToEx(HandleRef hdc, int x, int y, IntNativeMethods.POINT pt);

		// Token: 0x060000B8 RID: 184 RVA: 0x00003900 File Offset: 0x00002900
		public static bool MoveToEx(HandleRef hdc, int x, int y, IntNativeMethods.POINT pt)
		{
			return IntUnsafeNativeMethods.IntMoveToEx(hdc, x, y, pt);
		}

		// Token: 0x060000B9 RID: 185
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "Rectangle", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntRectangle(HandleRef hdc, int left, int top, int right, int bottom);

		// Token: 0x060000BA RID: 186 RVA: 0x00003918 File Offset: 0x00002918
		public static bool Rectangle(HandleRef hdc, int left, int top, int right, int bottom)
		{
			return IntUnsafeNativeMethods.IntRectangle(hdc, left, top, right, bottom);
		}

		// Token: 0x060000BB RID: 187
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "FillRect", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntFillRect(HandleRef hdc, [In] ref IntNativeMethods.RECT rect, HandleRef hbrush);

		// Token: 0x060000BC RID: 188 RVA: 0x00003934 File Offset: 0x00002934
		public static bool FillRect(HandleRef hDC, [In] ref IntNativeMethods.RECT rect, HandleRef hbrush)
		{
			return IntUnsafeNativeMethods.IntFillRect(hDC, ref rect, hbrush);
		}

		// Token: 0x060000BD RID: 189
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SetMapMode", ExactSpelling = true, SetLastError = true)]
		public static extern int IntSetMapMode(HandleRef hDC, int nMapMode);

		// Token: 0x060000BE RID: 190 RVA: 0x0000394C File Offset: 0x0000294C
		public static int SetMapMode(HandleRef hDC, int nMapMode)
		{
			return IntUnsafeNativeMethods.IntSetMapMode(hDC, nMapMode);
		}

		// Token: 0x060000BF RID: 191
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "GetMapMode", ExactSpelling = true, SetLastError = true)]
		public static extern int IntGetMapMode(HandleRef hDC);

		// Token: 0x060000C0 RID: 192 RVA: 0x00003964 File Offset: 0x00002964
		public static int GetMapMode(HandleRef hDC)
		{
			return IntUnsafeNativeMethods.IntGetMapMode(hDC);
		}

		// Token: 0x060000C1 RID: 193
		[DllImport("gdi32.dll", EntryPoint = "GetViewportExtEx", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntGetViewportExtEx(HandleRef hdc, [In] [Out] IntNativeMethods.SIZE lpSize);

		// Token: 0x060000C2 RID: 194 RVA: 0x0000397C File Offset: 0x0000297C
		public static bool GetViewportExtEx(HandleRef hdc, [In] [Out] IntNativeMethods.SIZE lpSize)
		{
			return IntUnsafeNativeMethods.IntGetViewportExtEx(hdc, lpSize);
		}

		// Token: 0x060000C3 RID: 195
		[DllImport("gdi32.dll", EntryPoint = "GetViewportOrgEx", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntGetViewportOrgEx(HandleRef hdc, [In] [Out] IntNativeMethods.POINT lpPoint);

		// Token: 0x060000C4 RID: 196 RVA: 0x00003994 File Offset: 0x00002994
		public static bool GetViewportOrgEx(HandleRef hdc, [In] [Out] IntNativeMethods.POINT lpPoint)
		{
			return IntUnsafeNativeMethods.IntGetViewportOrgEx(hdc, lpPoint);
		}

		// Token: 0x060000C5 RID: 197
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SetViewportExtEx", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntSetViewportExtEx(HandleRef hDC, int x, int y, [In] [Out] IntNativeMethods.SIZE size);

		// Token: 0x060000C6 RID: 198 RVA: 0x000039AC File Offset: 0x000029AC
		public static bool SetViewportExtEx(HandleRef hDC, int x, int y, [In] [Out] IntNativeMethods.SIZE size)
		{
			return IntUnsafeNativeMethods.IntSetViewportExtEx(hDC, x, y, size);
		}

		// Token: 0x060000C7 RID: 199
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "SetViewportOrgEx", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntSetViewportOrgEx(HandleRef hDC, int x, int y, [In] [Out] IntNativeMethods.POINT point);

		// Token: 0x060000C8 RID: 200 RVA: 0x000039C4 File Offset: 0x000029C4
		public static bool SetViewportOrgEx(HandleRef hDC, int x, int y, [In] [Out] IntNativeMethods.POINT point)
		{
			return IntUnsafeNativeMethods.IntSetViewportOrgEx(hDC, x, y, point);
		}

		// Token: 0x060000C9 RID: 201
		[DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		public static extern int GetTextMetricsW(HandleRef hDC, [In] [Out] ref IntNativeMethods.TEXTMETRIC lptm);

		// Token: 0x060000CA RID: 202
		[DllImport("gdi32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern int GetTextMetricsA(HandleRef hDC, [In] [Out] ref IntNativeMethods.TEXTMETRICA lptm);

		// Token: 0x060000CB RID: 203 RVA: 0x000039DC File Offset: 0x000029DC
		public static int GetTextMetrics(HandleRef hDC, ref IntNativeMethods.TEXTMETRIC lptm)
		{
			int num;
			if (Marshal.SystemDefaultCharSize == 1)
			{
				IntNativeMethods.TEXTMETRICA textmetrica = default(IntNativeMethods.TEXTMETRICA);
				num = IntUnsafeNativeMethods.GetTextMetricsA(hDC, ref textmetrica);
				lptm.tmHeight = textmetrica.tmHeight;
				lptm.tmAscent = textmetrica.tmAscent;
				lptm.tmDescent = textmetrica.tmDescent;
				lptm.tmInternalLeading = textmetrica.tmInternalLeading;
				lptm.tmExternalLeading = textmetrica.tmExternalLeading;
				lptm.tmAveCharWidth = textmetrica.tmAveCharWidth;
				lptm.tmMaxCharWidth = textmetrica.tmMaxCharWidth;
				lptm.tmWeight = textmetrica.tmWeight;
				lptm.tmOverhang = textmetrica.tmOverhang;
				lptm.tmDigitizedAspectX = textmetrica.tmDigitizedAspectX;
				lptm.tmDigitizedAspectY = textmetrica.tmDigitizedAspectY;
				lptm.tmFirstChar = (char)textmetrica.tmFirstChar;
				lptm.tmLastChar = (char)textmetrica.tmLastChar;
				lptm.tmDefaultChar = (char)textmetrica.tmDefaultChar;
				lptm.tmBreakChar = (char)textmetrica.tmBreakChar;
				lptm.tmItalic = textmetrica.tmItalic;
				lptm.tmUnderlined = textmetrica.tmUnderlined;
				lptm.tmStruckOut = textmetrica.tmStruckOut;
				lptm.tmPitchAndFamily = textmetrica.tmPitchAndFamily;
				lptm.tmCharSet = textmetrica.tmCharSet;
			}
			else
			{
				num = IntUnsafeNativeMethods.GetTextMetricsW(hDC, ref lptm);
			}
			return num;
		}

		// Token: 0x060000CC RID: 204
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "BeginPath", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntBeginPath(HandleRef hDC);

		// Token: 0x060000CD RID: 205 RVA: 0x00003B14 File Offset: 0x00002B14
		public static bool BeginPath(HandleRef hDC)
		{
			return IntUnsafeNativeMethods.IntBeginPath(hDC);
		}

		// Token: 0x060000CE RID: 206
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "EndPath", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntEndPath(HandleRef hDC);

		// Token: 0x060000CF RID: 207 RVA: 0x00003B2C File Offset: 0x00002B2C
		public static bool EndPath(HandleRef hDC)
		{
			return IntUnsafeNativeMethods.IntEndPath(hDC);
		}

		// Token: 0x060000D0 RID: 208
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "StrokePath", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntStrokePath(HandleRef hDC);

		// Token: 0x060000D1 RID: 209 RVA: 0x00003B44 File Offset: 0x00002B44
		public static bool StrokePath(HandleRef hDC)
		{
			return IntUnsafeNativeMethods.IntStrokePath(hDC);
		}

		// Token: 0x060000D2 RID: 210
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "AngleArc", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntAngleArc(HandleRef hDC, int x, int y, int radius, float startAngle, float endAngle);

		// Token: 0x060000D3 RID: 211 RVA: 0x00003B5C File Offset: 0x00002B5C
		public static bool AngleArc(HandleRef hDC, int x, int y, int radius, float startAngle, float endAngle)
		{
			return IntUnsafeNativeMethods.IntAngleArc(hDC, x, y, radius, startAngle, endAngle);
		}

		// Token: 0x060000D4 RID: 212
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "Arc", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntArc(HandleRef hDC, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nXStartArc, int nYStartArc, int nXEndArc, int nYEndArc);

		// Token: 0x060000D5 RID: 213 RVA: 0x00003B78 File Offset: 0x00002B78
		public static bool Arc(HandleRef hDC, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nXStartArc, int nYStartArc, int nXEndArc, int nYEndArc)
		{
			return IntUnsafeNativeMethods.IntArc(hDC, nLeftRect, nTopRect, nRightRect, nBottomRect, nXStartArc, nYStartArc, nXEndArc, nYEndArc);
		}

		// Token: 0x060000D6 RID: 214
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetTextAlign(HandleRef hDC, int nMode);

		// Token: 0x060000D7 RID: 215
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "Ellipse", ExactSpelling = true, SetLastError = true)]
		public static extern bool IntEllipse(HandleRef hDc, int x1, int y1, int x2, int y2);

		// Token: 0x060000D8 RID: 216 RVA: 0x00003B9C File Offset: 0x00002B9C
		public static bool Ellipse(HandleRef hDc, int x1, int y1, int x2, int y2)
		{
			return IntUnsafeNativeMethods.IntEllipse(hDc, x1, y1, x2, y2);
		}

		// Token: 0x060000D9 RID: 217
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern int WideCharToMultiByte(int codePage, int flags, [MarshalAs(UnmanagedType.LPWStr)] string wideStr, int chars, [In] [Out] byte[] pOutBytes, int bufferBytes, IntPtr defaultChar, IntPtr pDefaultUsed);
	}
}
