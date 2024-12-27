using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Internal;
using System.Drawing.Text;
using System.Internal;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Text;
using System.Threading;

namespace System.Drawing
{
	// Token: 0x02000090 RID: 144
	[SuppressUnmanagedCodeSecurity]
	internal class SafeNativeMethods
	{
		// Token: 0x06000815 RID: 2069
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateCompatibleBitmap", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntCreateCompatibleBitmap(HandleRef hDC, int width, int height);

		// Token: 0x06000816 RID: 2070 RVA: 0x0001EF6B File Offset: 0x0001DF6B
		public static IntPtr CreateCompatibleBitmap(HandleRef hDC, int width, int height)
		{
			return global::System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateCompatibleBitmap(hDC, width, height), SafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06000817 RID: 2071
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateBitmap", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntCreateBitmap(int width, int height, int planes, int bpp, IntPtr bitmapData);

		// Token: 0x06000818 RID: 2072 RVA: 0x0001EF7F File Offset: 0x0001DF7F
		public static IntPtr CreateBitmap(int width, int height, int planes, int bpp, IntPtr bitmapData)
		{
			return global::System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateBitmap(width, height, planes, bpp, bitmapData), SafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x06000819 RID: 2073
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int BitBlt(HandleRef hDC, int x, int y, int nWidth, int nHeight, HandleRef hSrcDC, int xSrc, int ySrc, int dwRop);

		// Token: 0x0600081A RID: 2074
		[DllImport("gdi32.dll")]
		public static extern int GetDIBits(HandleRef hdc, HandleRef hbm, int arg1, int arg2, IntPtr arg3, ref NativeMethods.BITMAPINFO_FLAT bmi, int arg5);

		// Token: 0x0600081B RID: 2075
		[DllImport("gdi32.dll")]
		public static extern uint GetPaletteEntries(HandleRef hpal, int iStartIndex, int nEntries, byte[] lppe);

		// Token: 0x0600081C RID: 2076
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateDIBSection", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntCreateDIBSection(HandleRef hdc, ref NativeMethods.BITMAPINFO_FLAT bmi, int iUsage, ref IntPtr ppvBits, IntPtr hSection, int dwOffset);

		// Token: 0x0600081D RID: 2077 RVA: 0x0001EF96 File Offset: 0x0001DF96
		public static IntPtr CreateDIBSection(HandleRef hdc, ref NativeMethods.BITMAPINFO_FLAT bmi, int iUsage, ref IntPtr ppvBits, IntPtr hSection, int dwOffset)
		{
			return global::System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateDIBSection(hdc, ref bmi, iUsage, ref ppvBits, hSection, dwOffset), SafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x0600081E RID: 2078
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GlobalFree(HandleRef handle);

		// Token: 0x0600081F RID: 2079
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int StartDoc(HandleRef hDC, SafeNativeMethods.DOCINFO lpDocInfo);

		// Token: 0x06000820 RID: 2080
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int StartPage(HandleRef hDC);

		// Token: 0x06000821 RID: 2081
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int EndPage(HandleRef hDC);

		// Token: 0x06000822 RID: 2082
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int AbortDoc(HandleRef hDC);

		// Token: 0x06000823 RID: 2083
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int EndDoc(HandleRef hDC);

		// Token: 0x06000824 RID: 2084
		[DllImport("comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool PrintDlg([In] [Out] SafeNativeMethods.PRINTDLG lppd);

		// Token: 0x06000825 RID: 2085
		[DllImport("comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool PrintDlg([In] [Out] SafeNativeMethods.PRINTDLGX86 lppd);

		// Token: 0x06000826 RID: 2086
		[DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int DeviceCapabilities(string pDevice, string pPort, short fwCapabilities, IntPtr pOutput, IntPtr pDevMode);

		// Token: 0x06000827 RID: 2087
		[DllImport("winspool.drv", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int DocumentProperties(HandleRef hwnd, HandleRef hPrinter, string pDeviceName, IntPtr pDevModeOutput, HandleRef pDevModeInput, int fMode);

		// Token: 0x06000828 RID: 2088
		[DllImport("winspool.drv", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int DocumentProperties(HandleRef hwnd, HandleRef hPrinter, string pDeviceName, IntPtr pDevModeOutput, IntPtr pDevModeInput, int fMode);

		// Token: 0x06000829 RID: 2089
		[DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int EnumPrinters(int flags, string name, int level, IntPtr pPrinterEnum, int cbBuf, out int pcbNeeded, out int pcReturned);

		// Token: 0x0600082A RID: 2090
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GlobalLock(HandleRef handle);

		// Token: 0x0600082B RID: 2091
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr ResetDC(HandleRef hDC, HandleRef lpDevMode);

		// Token: 0x0600082C RID: 2092
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool GlobalUnlock(HandleRef handle);

		// Token: 0x0600082D RID: 2093
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateRectRgn", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreateRectRgn(int x1, int y1, int x2, int y2);

		// Token: 0x0600082E RID: 2094 RVA: 0x0001EFAF File Offset: 0x0001DFAF
		public static IntPtr CreateRectRgn(int x1, int y1, int x2, int y2)
		{
			return global::System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateRectRgn(x1, y1, x2, y2), SafeNativeMethods.CommonHandles.GDI);
		}

		// Token: 0x0600082F RID: 2095
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetClipRgn(HandleRef hDC, HandleRef hRgn);

		// Token: 0x06000830 RID: 2096
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SelectClipRgn(HandleRef hDC, HandleRef hRgn);

		// Token: 0x06000831 RID: 2097
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int AddFontResourceEx(string lpszFilename, int fl, IntPtr pdv);

		// Token: 0x06000832 RID: 2098 RVA: 0x0001EFC4 File Offset: 0x0001DFC4
		public static int AddFontFile(string fileName)
		{
			if (Marshal.SystemDefaultCharSize == 1)
			{
				return 0;
			}
			return SafeNativeMethods.AddFontResourceEx(fileName, 16, IntPtr.Zero);
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0001EFE0 File Offset: 0x0001DFE0
		internal static IntPtr SaveClipRgn(IntPtr hDC)
		{
			IntPtr intPtr = SafeNativeMethods.CreateRectRgn(0, 0, 0, 0);
			IntPtr intPtr2 = IntPtr.Zero;
			try
			{
				int clipRgn = SafeNativeMethods.GetClipRgn(new HandleRef(null, hDC), new HandleRef(null, intPtr));
				if (clipRgn > 0)
				{
					intPtr2 = intPtr;
					intPtr = IntPtr.Zero;
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
				}
			}
			return intPtr2;
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0001F04C File Offset: 0x0001E04C
		internal static void RestoreClipRgn(IntPtr hDC, IntPtr hRgn)
		{
			try
			{
				SafeNativeMethods.SelectClipRgn(new HandleRef(null, hDC), new HandleRef(null, hRgn));
			}
			finally
			{
				if (hRgn != IntPtr.Zero)
				{
					SafeNativeMethods.DeleteObject(new HandleRef(null, hRgn));
				}
			}
		}

		// Token: 0x06000835 RID: 2101
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int ExtEscape(HandleRef hDC, int nEscape, int cbInput, ref int inData, int cbOutput, out int outData);

		// Token: 0x06000836 RID: 2102
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int ExtEscape(HandleRef hDC, int nEscape, int cbInput, byte[] inData, int cbOutput, out int outData);

		// Token: 0x06000837 RID: 2103
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int IntersectClipRect(HandleRef hDC, int x1, int y1, int x2, int y2);

		// Token: 0x06000838 RID: 2104
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "GlobalAlloc", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr IntGlobalAlloc(int uFlags, UIntPtr dwBytes);

		// Token: 0x06000839 RID: 2105 RVA: 0x0001F09C File Offset: 0x0001E09C
		public static IntPtr GlobalAlloc(int uFlags, uint dwBytes)
		{
			return SafeNativeMethods.IntGlobalAlloc(uFlags, new UIntPtr(dwBytes));
		}

		// Token: 0x0600083A RID: 2106
		[DllImport("kernel32.dll")]
		internal static extern void ZeroMemory(IntPtr destination, UIntPtr length);

		// Token: 0x0600083B RID: 2107
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "DeleteObject", ExactSpelling = true, SetLastError = true)]
		internal static extern int IntDeleteObject(HandleRef hObject);

		// Token: 0x0600083C RID: 2108 RVA: 0x0001F0AA File Offset: 0x0001E0AA
		public static int DeleteObject(HandleRef hObject)
		{
			global::System.Internal.HandleCollector.Remove((IntPtr)hObject, SafeNativeMethods.CommonHandles.GDI);
			return SafeNativeMethods.IntDeleteObject(hObject);
		}

		// Token: 0x0600083D RID: 2109
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr SelectObject(HandleRef hdc, HandleRef obj);

		// Token: 0x0600083E RID: 2110
		[DllImport("user32.dll", EntryPoint = "CreateIconFromResourceEx", SetLastError = true)]
		private unsafe static extern IntPtr IntCreateIconFromResourceEx(byte* pbIconBits, int cbIconBits, bool fIcon, int dwVersion, int csDesired, int cyDesired, int flags);

		// Token: 0x0600083F RID: 2111 RVA: 0x0001F0C3 File Offset: 0x0001E0C3
		public unsafe static IntPtr CreateIconFromResourceEx(byte* pbIconBits, int cbIconBits, bool fIcon, int dwVersion, int csDesired, int cyDesired, int flags)
		{
			return global::System.Internal.HandleCollector.Add(SafeNativeMethods.IntCreateIconFromResourceEx(pbIconBits, cbIconBits, fIcon, dwVersion, csDesired, cyDesired, flags), SafeNativeMethods.CommonHandles.Icon);
		}

		// Token: 0x06000840 RID: 2112
		[DllImport("shell32.dll", BestFitMapping = false, CharSet = CharSet.Auto, EntryPoint = "ExtractAssociatedIcon")]
		public static extern IntPtr IntExtractAssociatedIcon(HandleRef hInst, StringBuilder iconPath, ref int index);

		// Token: 0x06000841 RID: 2113 RVA: 0x0001F0DE File Offset: 0x0001E0DE
		public static IntPtr ExtractAssociatedIcon(HandleRef hInst, StringBuilder iconPath, ref int index)
		{
			return global::System.Internal.HandleCollector.Add(SafeNativeMethods.IntExtractAssociatedIcon(hInst, iconPath, ref index), SafeNativeMethods.CommonHandles.Icon);
		}

		// Token: 0x06000842 RID: 2114
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "LoadIcon", SetLastError = true)]
		private static extern IntPtr IntLoadIcon(HandleRef hInst, IntPtr iconId);

		// Token: 0x06000843 RID: 2115 RVA: 0x0001F0F2 File Offset: 0x0001E0F2
		public static IntPtr LoadIcon(HandleRef hInst, int iconId)
		{
			return SafeNativeMethods.IntLoadIcon(hInst, new IntPtr(iconId));
		}

		// Token: 0x06000844 RID: 2116
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "DestroyIcon", ExactSpelling = true, SetLastError = true)]
		private static extern bool IntDestroyIcon(HandleRef hIcon);

		// Token: 0x06000845 RID: 2117 RVA: 0x0001F100 File Offset: 0x0001E100
		public static bool DestroyIcon(HandleRef hIcon)
		{
			global::System.Internal.HandleCollector.Remove((IntPtr)hIcon, SafeNativeMethods.CommonHandles.Icon);
			return SafeNativeMethods.IntDestroyIcon(hIcon);
		}

		// Token: 0x06000846 RID: 2118
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "CopyImage", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCopyImage(HandleRef hImage, int uType, int cxDesired, int cyDesired, int fuFlags);

		// Token: 0x06000847 RID: 2119 RVA: 0x0001F11C File Offset: 0x0001E11C
		public static IntPtr CopyImage(HandleRef hImage, int uType, int cxDesired, int cyDesired, int fuFlags)
		{
			int num;
			if (uType == 1)
			{
				num = SafeNativeMethods.CommonHandles.Icon;
			}
			else
			{
				num = SafeNativeMethods.CommonHandles.GDI;
			}
			return global::System.Internal.HandleCollector.Add(SafeNativeMethods.IntCopyImage(hImage, uType, cxDesired, cyDesired, fuFlags), num);
		}

		// Token: 0x06000848 RID: 2120
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetObject(HandleRef hObject, int nSize, [In] [Out] SafeNativeMethods.BITMAP bm);

		// Token: 0x06000849 RID: 2121
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetObject(HandleRef hObject, int nSize, [In] [Out] SafeNativeMethods.LOGFONT lf);

		// Token: 0x0600084A RID: 2122 RVA: 0x0001F14E File Offset: 0x0001E14E
		public static int GetObject(HandleRef hObject, SafeNativeMethods.LOGFONT lp)
		{
			return SafeNativeMethods.GetObject(hObject, Marshal.SizeOf(typeof(SafeNativeMethods.LOGFONT)), lp);
		}

		// Token: 0x0600084B RID: 2123
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool GetIconInfo(HandleRef hIcon, [In] [Out] SafeNativeMethods.ICONINFO info);

		// Token: 0x0600084C RID: 2124
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetSysColor(int nIndex);

		// Token: 0x0600084D RID: 2125
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool DrawIconEx(HandleRef hDC, int x, int y, HandleRef hIcon, int width, int height, int iStepIfAniCursor, HandleRef hBrushFlickerFree, int diFlags);

		// Token: 0x0600084E RID: 2126
		[DllImport("oleaut32.dll", PreserveSig = false)]
		public static extern SafeNativeMethods.IPicture OleCreatePictureIndirect(SafeNativeMethods.PICTDESC pictdesc, [In] ref Guid refiid, bool fOwn);

		// Token: 0x04000654 RID: 1620
		public const int ERROR_CANCELLED = 1223;

		// Token: 0x04000655 RID: 1621
		public const int RASTERCAPS = 38;

		// Token: 0x04000656 RID: 1622
		public const int RC_PALETTE = 256;

		// Token: 0x04000657 RID: 1623
		public const int SIZEPALETTE = 104;

		// Token: 0x04000658 RID: 1624
		public const int SYSPAL_STATIC = 1;

		// Token: 0x04000659 RID: 1625
		public const int BS_SOLID = 0;

		// Token: 0x0400065A RID: 1626
		public const int HOLLOW_BRUSH = 5;

		// Token: 0x0400065B RID: 1627
		public const int R2_BLACK = 1;

		// Token: 0x0400065C RID: 1628
		public const int R2_NOTMERGEPEN = 2;

		// Token: 0x0400065D RID: 1629
		public const int R2_MASKNOTPEN = 3;

		// Token: 0x0400065E RID: 1630
		public const int R2_NOTCOPYPEN = 4;

		// Token: 0x0400065F RID: 1631
		public const int R2_MASKPENNOT = 5;

		// Token: 0x04000660 RID: 1632
		public const int R2_NOT = 6;

		// Token: 0x04000661 RID: 1633
		public const int R2_XORPEN = 7;

		// Token: 0x04000662 RID: 1634
		public const int R2_NOTMASKPEN = 8;

		// Token: 0x04000663 RID: 1635
		public const int R2_MASKPEN = 9;

		// Token: 0x04000664 RID: 1636
		public const int R2_NOTXORPEN = 10;

		// Token: 0x04000665 RID: 1637
		public const int R2_NOP = 11;

		// Token: 0x04000666 RID: 1638
		public const int R2_MERGENOTPEN = 12;

		// Token: 0x04000667 RID: 1639
		public const int R2_COPYPEN = 13;

		// Token: 0x04000668 RID: 1640
		public const int R2_MERGEPENNOT = 14;

		// Token: 0x04000669 RID: 1641
		public const int R2_MERGEPEN = 15;

		// Token: 0x0400066A RID: 1642
		public const int R2_WHITE = 16;

		// Token: 0x0400066B RID: 1643
		public const int UOI_FLAGS = 1;

		// Token: 0x0400066C RID: 1644
		public const int WSF_VISIBLE = 1;

		// Token: 0x0400066D RID: 1645
		public const int E_UNEXPECTED = -2147418113;

		// Token: 0x0400066E RID: 1646
		public const int E_NOTIMPL = -2147467263;

		// Token: 0x0400066F RID: 1647
		public const int E_OUTOFMEMORY = -2147024882;

		// Token: 0x04000670 RID: 1648
		public const int E_INVALIDARG = -2147024809;

		// Token: 0x04000671 RID: 1649
		public const int E_NOINTERFACE = -2147467262;

		// Token: 0x04000672 RID: 1650
		public const int E_POINTER = -2147467261;

		// Token: 0x04000673 RID: 1651
		public const int E_HANDLE = -2147024890;

		// Token: 0x04000674 RID: 1652
		public const int E_ABORT = -2147467260;

		// Token: 0x04000675 RID: 1653
		public const int E_FAIL = -2147467259;

		// Token: 0x04000676 RID: 1654
		public const int E_ACCESSDENIED = -2147024891;

		// Token: 0x04000677 RID: 1655
		public const int PM_NOREMOVE = 0;

		// Token: 0x04000678 RID: 1656
		public const int PM_REMOVE = 1;

		// Token: 0x04000679 RID: 1657
		public const int PM_NOYIELD = 2;

		// Token: 0x0400067A RID: 1658
		public const int GMEM_FIXED = 0;

		// Token: 0x0400067B RID: 1659
		public const int GMEM_MOVEABLE = 2;

		// Token: 0x0400067C RID: 1660
		public const int GMEM_NOCOMPACT = 16;

		// Token: 0x0400067D RID: 1661
		public const int GMEM_NODISCARD = 32;

		// Token: 0x0400067E RID: 1662
		public const int GMEM_ZEROINIT = 64;

		// Token: 0x0400067F RID: 1663
		public const int GMEM_MODIFY = 128;

		// Token: 0x04000680 RID: 1664
		public const int GMEM_DISCARDABLE = 256;

		// Token: 0x04000681 RID: 1665
		public const int GMEM_NOT_BANKED = 4096;

		// Token: 0x04000682 RID: 1666
		public const int GMEM_SHARE = 8192;

		// Token: 0x04000683 RID: 1667
		public const int GMEM_DDESHARE = 8192;

		// Token: 0x04000684 RID: 1668
		public const int GMEM_NOTIFY = 16384;

		// Token: 0x04000685 RID: 1669
		public const int GMEM_LOWER = 4096;

		// Token: 0x04000686 RID: 1670
		public const int GMEM_VALID_FLAGS = 32626;

		// Token: 0x04000687 RID: 1671
		public const int GMEM_INVALID_HANDLE = 32768;

		// Token: 0x04000688 RID: 1672
		public const int DM_UPDATE = 1;

		// Token: 0x04000689 RID: 1673
		public const int DM_COPY = 2;

		// Token: 0x0400068A RID: 1674
		public const int DM_PROMPT = 4;

		// Token: 0x0400068B RID: 1675
		public const int DM_MODIFY = 8;

		// Token: 0x0400068C RID: 1676
		public const int DM_IN_BUFFER = 8;

		// Token: 0x0400068D RID: 1677
		public const int DM_IN_PROMPT = 4;

		// Token: 0x0400068E RID: 1678
		public const int DM_OUT_BUFFER = 2;

		// Token: 0x0400068F RID: 1679
		public const int DM_OUT_DEFAULT = 1;

		// Token: 0x04000690 RID: 1680
		public const int DT_PLOTTER = 0;

		// Token: 0x04000691 RID: 1681
		public const int DT_RASDISPLAY = 1;

		// Token: 0x04000692 RID: 1682
		public const int DT_RASPRINTER = 2;

		// Token: 0x04000693 RID: 1683
		public const int DT_RASCAMERA = 3;

		// Token: 0x04000694 RID: 1684
		public const int DT_CHARSTREAM = 4;

		// Token: 0x04000695 RID: 1685
		public const int DT_METAFILE = 5;

		// Token: 0x04000696 RID: 1686
		public const int DT_DISPFILE = 6;

		// Token: 0x04000697 RID: 1687
		public const int TECHNOLOGY = 2;

		// Token: 0x04000698 RID: 1688
		public const int DC_FIELDS = 1;

		// Token: 0x04000699 RID: 1689
		public const int DC_PAPERS = 2;

		// Token: 0x0400069A RID: 1690
		public const int DC_PAPERSIZE = 3;

		// Token: 0x0400069B RID: 1691
		public const int DC_MINEXTENT = 4;

		// Token: 0x0400069C RID: 1692
		public const int DC_MAXEXTENT = 5;

		// Token: 0x0400069D RID: 1693
		public const int DC_BINS = 6;

		// Token: 0x0400069E RID: 1694
		public const int DC_DUPLEX = 7;

		// Token: 0x0400069F RID: 1695
		public const int DC_SIZE = 8;

		// Token: 0x040006A0 RID: 1696
		public const int DC_EXTRA = 9;

		// Token: 0x040006A1 RID: 1697
		public const int DC_VERSION = 10;

		// Token: 0x040006A2 RID: 1698
		public const int DC_DRIVER = 11;

		// Token: 0x040006A3 RID: 1699
		public const int DC_BINNAMES = 12;

		// Token: 0x040006A4 RID: 1700
		public const int DC_ENUMRESOLUTIONS = 13;

		// Token: 0x040006A5 RID: 1701
		public const int DC_FILEDEPENDENCIES = 14;

		// Token: 0x040006A6 RID: 1702
		public const int DC_TRUETYPE = 15;

		// Token: 0x040006A7 RID: 1703
		public const int DC_PAPERNAMES = 16;

		// Token: 0x040006A8 RID: 1704
		public const int DC_ORIENTATION = 17;

		// Token: 0x040006A9 RID: 1705
		public const int DC_COPIES = 18;

		// Token: 0x040006AA RID: 1706
		public const int PD_ALLPAGES = 0;

		// Token: 0x040006AB RID: 1707
		public const int PD_SELECTION = 1;

		// Token: 0x040006AC RID: 1708
		public const int PD_PAGENUMS = 2;

		// Token: 0x040006AD RID: 1709
		public const int PD_CURRENTPAGE = 4194304;

		// Token: 0x040006AE RID: 1710
		public const int PD_NOSELECTION = 4;

		// Token: 0x040006AF RID: 1711
		public const int PD_NOPAGENUMS = 8;

		// Token: 0x040006B0 RID: 1712
		public const int PD_NOCURRENTPAGE = 8388608;

		// Token: 0x040006B1 RID: 1713
		public const int PD_COLLATE = 16;

		// Token: 0x040006B2 RID: 1714
		public const int PD_PRINTTOFILE = 32;

		// Token: 0x040006B3 RID: 1715
		public const int PD_PRINTSETUP = 64;

		// Token: 0x040006B4 RID: 1716
		public const int PD_NOWARNING = 128;

		// Token: 0x040006B5 RID: 1717
		public const int PD_RETURNDC = 256;

		// Token: 0x040006B6 RID: 1718
		public const int PD_RETURNIC = 512;

		// Token: 0x040006B7 RID: 1719
		public const int PD_RETURNDEFAULT = 1024;

		// Token: 0x040006B8 RID: 1720
		public const int PD_SHOWHELP = 2048;

		// Token: 0x040006B9 RID: 1721
		public const int PD_ENABLEPRINTHOOK = 4096;

		// Token: 0x040006BA RID: 1722
		public const int PD_ENABLESETUPHOOK = 8192;

		// Token: 0x040006BB RID: 1723
		public const int PD_ENABLEPRINTTEMPLATE = 16384;

		// Token: 0x040006BC RID: 1724
		public const int PD_ENABLESETUPTEMPLATE = 32768;

		// Token: 0x040006BD RID: 1725
		public const int PD_ENABLEPRINTTEMPLATEHANDLE = 65536;

		// Token: 0x040006BE RID: 1726
		public const int PD_ENABLESETUPTEMPLATEHANDLE = 131072;

		// Token: 0x040006BF RID: 1727
		public const int PD_USEDEVMODECOPIES = 262144;

		// Token: 0x040006C0 RID: 1728
		public const int PD_USEDEVMODECOPIESANDCOLLATE = 262144;

		// Token: 0x040006C1 RID: 1729
		public const int PD_DISABLEPRINTTOFILE = 524288;

		// Token: 0x040006C2 RID: 1730
		public const int PD_HIDEPRINTTOFILE = 1048576;

		// Token: 0x040006C3 RID: 1731
		public const int PD_NONETWORKBUTTON = 2097152;

		// Token: 0x040006C4 RID: 1732
		public const int DI_MASK = 1;

		// Token: 0x040006C5 RID: 1733
		public const int DI_IMAGE = 2;

		// Token: 0x040006C6 RID: 1734
		public const int DI_NORMAL = 3;

		// Token: 0x040006C7 RID: 1735
		public const int DI_COMPAT = 4;

		// Token: 0x040006C8 RID: 1736
		public const int DI_DEFAULTSIZE = 8;

		// Token: 0x040006C9 RID: 1737
		public const int IDC_ARROW = 32512;

		// Token: 0x040006CA RID: 1738
		public const int IDC_IBEAM = 32513;

		// Token: 0x040006CB RID: 1739
		public const int IDC_WAIT = 32514;

		// Token: 0x040006CC RID: 1740
		public const int IDC_CROSS = 32515;

		// Token: 0x040006CD RID: 1741
		public const int IDC_UPARROW = 32516;

		// Token: 0x040006CE RID: 1742
		public const int IDC_SIZE = 32640;

		// Token: 0x040006CF RID: 1743
		public const int IDC_ICON = 32641;

		// Token: 0x040006D0 RID: 1744
		public const int IDC_SIZENWSE = 32642;

		// Token: 0x040006D1 RID: 1745
		public const int IDC_SIZENESW = 32643;

		// Token: 0x040006D2 RID: 1746
		public const int IDC_SIZEWE = 32644;

		// Token: 0x040006D3 RID: 1747
		public const int IDC_SIZENS = 32645;

		// Token: 0x040006D4 RID: 1748
		public const int IDC_SIZEALL = 32646;

		// Token: 0x040006D5 RID: 1749
		public const int IDC_NO = 32648;

		// Token: 0x040006D6 RID: 1750
		public const int IDC_APPSTARTING = 32650;

		// Token: 0x040006D7 RID: 1751
		public const int IDC_HELP = 32651;

		// Token: 0x040006D8 RID: 1752
		public const int IMAGE_BITMAP = 0;

		// Token: 0x040006D9 RID: 1753
		public const int IMAGE_ICON = 1;

		// Token: 0x040006DA RID: 1754
		public const int IMAGE_CURSOR = 2;

		// Token: 0x040006DB RID: 1755
		public const int IMAGE_ENHMETAFILE = 3;

		// Token: 0x040006DC RID: 1756
		public const int IDI_APPLICATION = 32512;

		// Token: 0x040006DD RID: 1757
		public const int IDI_HAND = 32513;

		// Token: 0x040006DE RID: 1758
		public const int IDI_QUESTION = 32514;

		// Token: 0x040006DF RID: 1759
		public const int IDI_EXCLAMATION = 32515;

		// Token: 0x040006E0 RID: 1760
		public const int IDI_ASTERISK = 32516;

		// Token: 0x040006E1 RID: 1761
		public const int IDI_WINLOGO = 32517;

		// Token: 0x040006E2 RID: 1762
		public const int IDI_WARNING = 32515;

		// Token: 0x040006E3 RID: 1763
		public const int IDI_ERROR = 32513;

		// Token: 0x040006E4 RID: 1764
		public const int IDI_INFORMATION = 32516;

		// Token: 0x040006E5 RID: 1765
		public const int SRCCOPY = 13369376;

		// Token: 0x040006E6 RID: 1766
		public const int PLANES = 14;

		// Token: 0x040006E7 RID: 1767
		public const int PS_SOLID = 0;

		// Token: 0x040006E8 RID: 1768
		public const int PS_DASH = 1;

		// Token: 0x040006E9 RID: 1769
		public const int PS_DOT = 2;

		// Token: 0x040006EA RID: 1770
		public const int PS_DASHDOT = 3;

		// Token: 0x040006EB RID: 1771
		public const int PS_DASHDOTDOT = 4;

		// Token: 0x040006EC RID: 1772
		public const int PS_NULL = 5;

		// Token: 0x040006ED RID: 1773
		public const int PS_INSIDEFRAME = 6;

		// Token: 0x040006EE RID: 1774
		public const int PS_USERSTYLE = 7;

		// Token: 0x040006EF RID: 1775
		public const int PS_ALTERNATE = 8;

		// Token: 0x040006F0 RID: 1776
		public const int PS_STYLE_MASK = 15;

		// Token: 0x040006F1 RID: 1777
		public const int PS_ENDCAP_ROUND = 0;

		// Token: 0x040006F2 RID: 1778
		public const int PS_ENDCAP_SQUARE = 256;

		// Token: 0x040006F3 RID: 1779
		public const int PS_ENDCAP_FLAT = 512;

		// Token: 0x040006F4 RID: 1780
		public const int PS_ENDCAP_MASK = 3840;

		// Token: 0x040006F5 RID: 1781
		public const int PS_JOIN_ROUND = 0;

		// Token: 0x040006F6 RID: 1782
		public const int PS_JOIN_BEVEL = 4096;

		// Token: 0x040006F7 RID: 1783
		public const int PS_JOIN_MITER = 8192;

		// Token: 0x040006F8 RID: 1784
		public const int PS_JOIN_MASK = 61440;

		// Token: 0x040006F9 RID: 1785
		public const int PS_COSMETIC = 0;

		// Token: 0x040006FA RID: 1786
		public const int PS_GEOMETRIC = 65536;

		// Token: 0x040006FB RID: 1787
		public const int PS_TYPE_MASK = 983040;

		// Token: 0x040006FC RID: 1788
		public const int BITSPIXEL = 12;

		// Token: 0x040006FD RID: 1789
		public const int ALTERNATE = 1;

		// Token: 0x040006FE RID: 1790
		public const int LOGPIXELSX = 88;

		// Token: 0x040006FF RID: 1791
		public const int LOGPIXELSY = 90;

		// Token: 0x04000700 RID: 1792
		public const int PHYSICALWIDTH = 110;

		// Token: 0x04000701 RID: 1793
		public const int PHYSICALHEIGHT = 111;

		// Token: 0x04000702 RID: 1794
		public const int PHYSICALOFFSETX = 112;

		// Token: 0x04000703 RID: 1795
		public const int PHYSICALOFFSETY = 113;

		// Token: 0x04000704 RID: 1796
		public const int WINDING = 2;

		// Token: 0x04000705 RID: 1797
		public const int VERTRES = 10;

		// Token: 0x04000706 RID: 1798
		public const int HORZRES = 8;

		// Token: 0x04000707 RID: 1799
		public const int DM_SPECVERSION = 1025;

		// Token: 0x04000708 RID: 1800
		public const int DM_ORIENTATION = 1;

		// Token: 0x04000709 RID: 1801
		public const int DM_PAPERSIZE = 2;

		// Token: 0x0400070A RID: 1802
		public const int DM_PAPERLENGTH = 4;

		// Token: 0x0400070B RID: 1803
		public const int DM_PAPERWIDTH = 8;

		// Token: 0x0400070C RID: 1804
		public const int DM_SCALE = 16;

		// Token: 0x0400070D RID: 1805
		public const int DM_COPIES = 256;

		// Token: 0x0400070E RID: 1806
		public const int DM_DEFAULTSOURCE = 512;

		// Token: 0x0400070F RID: 1807
		public const int DM_PRINTQUALITY = 1024;

		// Token: 0x04000710 RID: 1808
		public const int DM_COLOR = 2048;

		// Token: 0x04000711 RID: 1809
		public const int DM_DUPLEX = 4096;

		// Token: 0x04000712 RID: 1810
		public const int DM_YRESOLUTION = 8192;

		// Token: 0x04000713 RID: 1811
		public const int DM_TTOPTION = 16384;

		// Token: 0x04000714 RID: 1812
		public const int DM_COLLATE = 32768;

		// Token: 0x04000715 RID: 1813
		public const int DM_FORMNAME = 65536;

		// Token: 0x04000716 RID: 1814
		public const int DM_LOGPIXELS = 131072;

		// Token: 0x04000717 RID: 1815
		public const int DM_BITSPERPEL = 262144;

		// Token: 0x04000718 RID: 1816
		public const int DM_PELSWIDTH = 524288;

		// Token: 0x04000719 RID: 1817
		public const int DM_PELSHEIGHT = 1048576;

		// Token: 0x0400071A RID: 1818
		public const int DM_DISPLAYFLAGS = 2097152;

		// Token: 0x0400071B RID: 1819
		public const int DM_DISPLAYFREQUENCY = 4194304;

		// Token: 0x0400071C RID: 1820
		public const int DM_PANNINGWIDTH = 8388608;

		// Token: 0x0400071D RID: 1821
		public const int DM_PANNINGHEIGHT = 16777216;

		// Token: 0x0400071E RID: 1822
		public const int DM_ICMMETHOD = 33554432;

		// Token: 0x0400071F RID: 1823
		public const int DM_ICMINTENT = 67108864;

		// Token: 0x04000720 RID: 1824
		public const int DM_MEDIATYPE = 134217728;

		// Token: 0x04000721 RID: 1825
		public const int DM_DITHERTYPE = 268435456;

		// Token: 0x04000722 RID: 1826
		public const int DM_ICCMANUFACTURER = 536870912;

		// Token: 0x04000723 RID: 1827
		public const int DM_ICCMODEL = 1073741824;

		// Token: 0x04000724 RID: 1828
		public const int DMORIENT_PORTRAIT = 1;

		// Token: 0x04000725 RID: 1829
		public const int DMORIENT_LANDSCAPE = 2;

		// Token: 0x04000726 RID: 1830
		public const int DMPAPER_LETTER = 1;

		// Token: 0x04000727 RID: 1831
		public const int DMPAPER_LETTERSMALL = 2;

		// Token: 0x04000728 RID: 1832
		public const int DMPAPER_TABLOID = 3;

		// Token: 0x04000729 RID: 1833
		public const int DMPAPER_LEDGER = 4;

		// Token: 0x0400072A RID: 1834
		public const int DMPAPER_LEGAL = 5;

		// Token: 0x0400072B RID: 1835
		public const int DMPAPER_STATEMENT = 6;

		// Token: 0x0400072C RID: 1836
		public const int DMPAPER_EXECUTIVE = 7;

		// Token: 0x0400072D RID: 1837
		public const int DMPAPER_A3 = 8;

		// Token: 0x0400072E RID: 1838
		public const int DMPAPER_A4 = 9;

		// Token: 0x0400072F RID: 1839
		public const int DMPAPER_A4SMALL = 10;

		// Token: 0x04000730 RID: 1840
		public const int DMPAPER_A5 = 11;

		// Token: 0x04000731 RID: 1841
		public const int DMPAPER_B4 = 12;

		// Token: 0x04000732 RID: 1842
		public const int DMPAPER_B5 = 13;

		// Token: 0x04000733 RID: 1843
		public const int DMPAPER_FOLIO = 14;

		// Token: 0x04000734 RID: 1844
		public const int DMPAPER_QUARTO = 15;

		// Token: 0x04000735 RID: 1845
		public const int DMPAPER_10X14 = 16;

		// Token: 0x04000736 RID: 1846
		public const int DMPAPER_11X17 = 17;

		// Token: 0x04000737 RID: 1847
		public const int DMPAPER_NOTE = 18;

		// Token: 0x04000738 RID: 1848
		public const int DMPAPER_ENV_9 = 19;

		// Token: 0x04000739 RID: 1849
		public const int DMPAPER_ENV_10 = 20;

		// Token: 0x0400073A RID: 1850
		public const int DMPAPER_ENV_11 = 21;

		// Token: 0x0400073B RID: 1851
		public const int DMPAPER_ENV_12 = 22;

		// Token: 0x0400073C RID: 1852
		public const int DMPAPER_ENV_14 = 23;

		// Token: 0x0400073D RID: 1853
		public const int DMPAPER_CSHEET = 24;

		// Token: 0x0400073E RID: 1854
		public const int DMPAPER_DSHEET = 25;

		// Token: 0x0400073F RID: 1855
		public const int DMPAPER_ESHEET = 26;

		// Token: 0x04000740 RID: 1856
		public const int DMPAPER_ENV_DL = 27;

		// Token: 0x04000741 RID: 1857
		public const int DMPAPER_ENV_C5 = 28;

		// Token: 0x04000742 RID: 1858
		public const int DMPAPER_ENV_C3 = 29;

		// Token: 0x04000743 RID: 1859
		public const int DMPAPER_ENV_C4 = 30;

		// Token: 0x04000744 RID: 1860
		public const int DMPAPER_ENV_C6 = 31;

		// Token: 0x04000745 RID: 1861
		public const int DMPAPER_ENV_C65 = 32;

		// Token: 0x04000746 RID: 1862
		public const int DMPAPER_ENV_B4 = 33;

		// Token: 0x04000747 RID: 1863
		public const int DMPAPER_ENV_B5 = 34;

		// Token: 0x04000748 RID: 1864
		public const int DMPAPER_ENV_B6 = 35;

		// Token: 0x04000749 RID: 1865
		public const int DMPAPER_ENV_ITALY = 36;

		// Token: 0x0400074A RID: 1866
		public const int DMPAPER_ENV_MONARCH = 37;

		// Token: 0x0400074B RID: 1867
		public const int DMPAPER_ENV_PERSONAL = 38;

		// Token: 0x0400074C RID: 1868
		public const int DMPAPER_FANFOLD_US = 39;

		// Token: 0x0400074D RID: 1869
		public const int DMPAPER_FANFOLD_STD_GERMAN = 40;

		// Token: 0x0400074E RID: 1870
		public const int DMPAPER_FANFOLD_LGL_GERMAN = 41;

		// Token: 0x0400074F RID: 1871
		public const int DMPAPER_ISO_B4 = 42;

		// Token: 0x04000750 RID: 1872
		public const int DMPAPER_JAPANESE_POSTCARD = 43;

		// Token: 0x04000751 RID: 1873
		public const int DMPAPER_9X11 = 44;

		// Token: 0x04000752 RID: 1874
		public const int DMPAPER_10X11 = 45;

		// Token: 0x04000753 RID: 1875
		public const int DMPAPER_15X11 = 46;

		// Token: 0x04000754 RID: 1876
		public const int DMPAPER_ENV_INVITE = 47;

		// Token: 0x04000755 RID: 1877
		public const int DMPAPER_RESERVED_48 = 48;

		// Token: 0x04000756 RID: 1878
		public const int DMPAPER_RESERVED_49 = 49;

		// Token: 0x04000757 RID: 1879
		public const int DMPAPER_LETTER_EXTRA = 50;

		// Token: 0x04000758 RID: 1880
		public const int DMPAPER_LEGAL_EXTRA = 51;

		// Token: 0x04000759 RID: 1881
		public const int DMPAPER_TABLOID_EXTRA = 52;

		// Token: 0x0400075A RID: 1882
		public const int DMPAPER_A4_EXTRA = 53;

		// Token: 0x0400075B RID: 1883
		public const int DMPAPER_LETTER_TRANSVERSE = 54;

		// Token: 0x0400075C RID: 1884
		public const int DMPAPER_A4_TRANSVERSE = 55;

		// Token: 0x0400075D RID: 1885
		public const int DMPAPER_LETTER_EXTRA_TRANSVERSE = 56;

		// Token: 0x0400075E RID: 1886
		public const int DMPAPER_A_PLUS = 57;

		// Token: 0x0400075F RID: 1887
		public const int DMPAPER_B_PLUS = 58;

		// Token: 0x04000760 RID: 1888
		public const int DMPAPER_LETTER_PLUS = 59;

		// Token: 0x04000761 RID: 1889
		public const int DMPAPER_A4_PLUS = 60;

		// Token: 0x04000762 RID: 1890
		public const int DMPAPER_A5_TRANSVERSE = 61;

		// Token: 0x04000763 RID: 1891
		public const int DMPAPER_B5_TRANSVERSE = 62;

		// Token: 0x04000764 RID: 1892
		public const int DMPAPER_A3_EXTRA = 63;

		// Token: 0x04000765 RID: 1893
		public const int DMPAPER_A5_EXTRA = 64;

		// Token: 0x04000766 RID: 1894
		public const int DMPAPER_B5_EXTRA = 65;

		// Token: 0x04000767 RID: 1895
		public const int DMPAPER_A2 = 66;

		// Token: 0x04000768 RID: 1896
		public const int DMPAPER_A3_TRANSVERSE = 67;

		// Token: 0x04000769 RID: 1897
		public const int DMPAPER_A3_EXTRA_TRANSVERSE = 68;

		// Token: 0x0400076A RID: 1898
		public const int DMPAPER_DBL_JAPANESE_POSTCARD = 69;

		// Token: 0x0400076B RID: 1899
		public const int DMPAPER_A6 = 70;

		// Token: 0x0400076C RID: 1900
		public const int DMPAPER_JENV_KAKU2 = 71;

		// Token: 0x0400076D RID: 1901
		public const int DMPAPER_JENV_KAKU3 = 72;

		// Token: 0x0400076E RID: 1902
		public const int DMPAPER_JENV_CHOU3 = 73;

		// Token: 0x0400076F RID: 1903
		public const int DMPAPER_JENV_CHOU4 = 74;

		// Token: 0x04000770 RID: 1904
		public const int DMPAPER_LETTER_ROTATED = 75;

		// Token: 0x04000771 RID: 1905
		public const int DMPAPER_A3_ROTATED = 76;

		// Token: 0x04000772 RID: 1906
		public const int DMPAPER_A4_ROTATED = 77;

		// Token: 0x04000773 RID: 1907
		public const int DMPAPER_A5_ROTATED = 78;

		// Token: 0x04000774 RID: 1908
		public const int DMPAPER_B4_JIS_ROTATED = 79;

		// Token: 0x04000775 RID: 1909
		public const int DMPAPER_B5_JIS_ROTATED = 80;

		// Token: 0x04000776 RID: 1910
		public const int DMPAPER_JAPANESE_POSTCARD_ROTATED = 81;

		// Token: 0x04000777 RID: 1911
		public const int DMPAPER_DBL_JAPANESE_POSTCARD_ROTATED = 82;

		// Token: 0x04000778 RID: 1912
		public const int DMPAPER_A6_ROTATED = 83;

		// Token: 0x04000779 RID: 1913
		public const int DMPAPER_JENV_KAKU2_ROTATED = 84;

		// Token: 0x0400077A RID: 1914
		public const int DMPAPER_JENV_KAKU3_ROTATED = 85;

		// Token: 0x0400077B RID: 1915
		public const int DMPAPER_JENV_CHOU3_ROTATED = 86;

		// Token: 0x0400077C RID: 1916
		public const int DMPAPER_JENV_CHOU4_ROTATED = 87;

		// Token: 0x0400077D RID: 1917
		public const int DMPAPER_B6_JIS = 88;

		// Token: 0x0400077E RID: 1918
		public const int DMPAPER_B6_JIS_ROTATED = 89;

		// Token: 0x0400077F RID: 1919
		public const int DMPAPER_12X11 = 90;

		// Token: 0x04000780 RID: 1920
		public const int DMPAPER_JENV_YOU4 = 91;

		// Token: 0x04000781 RID: 1921
		public const int DMPAPER_JENV_YOU4_ROTATED = 92;

		// Token: 0x04000782 RID: 1922
		public const int DMPAPER_P16K = 93;

		// Token: 0x04000783 RID: 1923
		public const int DMPAPER_P32K = 94;

		// Token: 0x04000784 RID: 1924
		public const int DMPAPER_P32KBIG = 95;

		// Token: 0x04000785 RID: 1925
		public const int DMPAPER_PENV_1 = 96;

		// Token: 0x04000786 RID: 1926
		public const int DMPAPER_PENV_2 = 97;

		// Token: 0x04000787 RID: 1927
		public const int DMPAPER_PENV_3 = 98;

		// Token: 0x04000788 RID: 1928
		public const int DMPAPER_PENV_4 = 99;

		// Token: 0x04000789 RID: 1929
		public const int DMPAPER_PENV_5 = 100;

		// Token: 0x0400078A RID: 1930
		public const int DMPAPER_PENV_6 = 101;

		// Token: 0x0400078B RID: 1931
		public const int DMPAPER_PENV_7 = 102;

		// Token: 0x0400078C RID: 1932
		public const int DMPAPER_PENV_8 = 103;

		// Token: 0x0400078D RID: 1933
		public const int DMPAPER_PENV_9 = 104;

		// Token: 0x0400078E RID: 1934
		public const int DMPAPER_PENV_10 = 105;

		// Token: 0x0400078F RID: 1935
		public const int DMPAPER_P16K_ROTATED = 106;

		// Token: 0x04000790 RID: 1936
		public const int DMPAPER_P32K_ROTATED = 107;

		// Token: 0x04000791 RID: 1937
		public const int DMPAPER_P32KBIG_ROTATED = 108;

		// Token: 0x04000792 RID: 1938
		public const int DMPAPER_PENV_1_ROTATED = 109;

		// Token: 0x04000793 RID: 1939
		public const int DMPAPER_PENV_2_ROTATED = 110;

		// Token: 0x04000794 RID: 1940
		public const int DMPAPER_PENV_3_ROTATED = 111;

		// Token: 0x04000795 RID: 1941
		public const int DMPAPER_PENV_4_ROTATED = 112;

		// Token: 0x04000796 RID: 1942
		public const int DMPAPER_PENV_5_ROTATED = 113;

		// Token: 0x04000797 RID: 1943
		public const int DMPAPER_PENV_6_ROTATED = 114;

		// Token: 0x04000798 RID: 1944
		public const int DMPAPER_PENV_7_ROTATED = 115;

		// Token: 0x04000799 RID: 1945
		public const int DMPAPER_PENV_8_ROTATED = 116;

		// Token: 0x0400079A RID: 1946
		public const int DMPAPER_PENV_9_ROTATED = 117;

		// Token: 0x0400079B RID: 1947
		public const int DMPAPER_PENV_10_ROTATED = 118;

		// Token: 0x0400079C RID: 1948
		public const int DMPAPER_LAST = 118;

		// Token: 0x0400079D RID: 1949
		public const int DMPAPER_USER = 256;

		// Token: 0x0400079E RID: 1950
		public const int DMBIN_UPPER = 1;

		// Token: 0x0400079F RID: 1951
		public const int DMBIN_ONLYONE = 1;

		// Token: 0x040007A0 RID: 1952
		public const int DMBIN_LOWER = 2;

		// Token: 0x040007A1 RID: 1953
		public const int DMBIN_MIDDLE = 3;

		// Token: 0x040007A2 RID: 1954
		public const int DMBIN_MANUAL = 4;

		// Token: 0x040007A3 RID: 1955
		public const int DMBIN_ENVELOPE = 5;

		// Token: 0x040007A4 RID: 1956
		public const int DMBIN_ENVMANUAL = 6;

		// Token: 0x040007A5 RID: 1957
		public const int DMBIN_AUTO = 7;

		// Token: 0x040007A6 RID: 1958
		public const int DMBIN_TRACTOR = 8;

		// Token: 0x040007A7 RID: 1959
		public const int DMBIN_SMALLFMT = 9;

		// Token: 0x040007A8 RID: 1960
		public const int DMBIN_LARGEFMT = 10;

		// Token: 0x040007A9 RID: 1961
		public const int DMBIN_LARGECAPACITY = 11;

		// Token: 0x040007AA RID: 1962
		public const int DMBIN_CASSETTE = 14;

		// Token: 0x040007AB RID: 1963
		public const int DMBIN_FORMSOURCE = 15;

		// Token: 0x040007AC RID: 1964
		public const int DMBIN_LAST = 15;

		// Token: 0x040007AD RID: 1965
		public const int DMBIN_USER = 256;

		// Token: 0x040007AE RID: 1966
		public const int DMRES_DRAFT = -1;

		// Token: 0x040007AF RID: 1967
		public const int DMRES_LOW = -2;

		// Token: 0x040007B0 RID: 1968
		public const int DMRES_MEDIUM = -3;

		// Token: 0x040007B1 RID: 1969
		public const int DMRES_HIGH = -4;

		// Token: 0x040007B2 RID: 1970
		public const int DMCOLOR_MONOCHROME = 1;

		// Token: 0x040007B3 RID: 1971
		public const int DMCOLOR_COLOR = 2;

		// Token: 0x040007B4 RID: 1972
		public const int DMDUP_SIMPLEX = 1;

		// Token: 0x040007B5 RID: 1973
		public const int DMDUP_VERTICAL = 2;

		// Token: 0x040007B6 RID: 1974
		public const int DMDUP_HORIZONTAL = 3;

		// Token: 0x040007B7 RID: 1975
		public const int DMTT_BITMAP = 1;

		// Token: 0x040007B8 RID: 1976
		public const int DMTT_DOWNLOAD = 2;

		// Token: 0x040007B9 RID: 1977
		public const int DMTT_SUBDEV = 3;

		// Token: 0x040007BA RID: 1978
		public const int DMTT_DOWNLOAD_OUTLINE = 4;

		// Token: 0x040007BB RID: 1979
		public const int DMCOLLATE_FALSE = 0;

		// Token: 0x040007BC RID: 1980
		public const int DMCOLLATE_TRUE = 1;

		// Token: 0x040007BD RID: 1981
		public const int DMDISPLAYFLAGS_TEXTMODE = 4;

		// Token: 0x040007BE RID: 1982
		public const int DMICMMETHOD_NONE = 1;

		// Token: 0x040007BF RID: 1983
		public const int DMICMMETHOD_SYSTEM = 2;

		// Token: 0x040007C0 RID: 1984
		public const int DMICMMETHOD_DRIVER = 3;

		// Token: 0x040007C1 RID: 1985
		public const int DMICMMETHOD_DEVICE = 4;

		// Token: 0x040007C2 RID: 1986
		public const int DMICMMETHOD_USER = 256;

		// Token: 0x040007C3 RID: 1987
		public const int DMICM_SATURATE = 1;

		// Token: 0x040007C4 RID: 1988
		public const int DMICM_CONTRAST = 2;

		// Token: 0x040007C5 RID: 1989
		public const int DMICM_COLORMETRIC = 3;

		// Token: 0x040007C6 RID: 1990
		public const int DMICM_USER = 256;

		// Token: 0x040007C7 RID: 1991
		public const int DMMEDIA_STANDARD = 1;

		// Token: 0x040007C8 RID: 1992
		public const int DMMEDIA_TRANSPARENCY = 2;

		// Token: 0x040007C9 RID: 1993
		public const int DMMEDIA_GLOSSY = 3;

		// Token: 0x040007CA RID: 1994
		public const int DMMEDIA_USER = 256;

		// Token: 0x040007CB RID: 1995
		public const int DMDITHER_NONE = 1;

		// Token: 0x040007CC RID: 1996
		public const int DMDITHER_COARSE = 2;

		// Token: 0x040007CD RID: 1997
		public const int DMDITHER_FINE = 3;

		// Token: 0x040007CE RID: 1998
		public const int DMDITHER_LINEART = 4;

		// Token: 0x040007CF RID: 1999
		public const int DMDITHER_GRAYSCALE = 5;

		// Token: 0x040007D0 RID: 2000
		public const int DMDITHER_USER = 256;

		// Token: 0x040007D1 RID: 2001
		public const int PRINTER_ENUM_DEFAULT = 1;

		// Token: 0x040007D2 RID: 2002
		public const int PRINTER_ENUM_LOCAL = 2;

		// Token: 0x040007D3 RID: 2003
		public const int PRINTER_ENUM_CONNECTIONS = 4;

		// Token: 0x040007D4 RID: 2004
		public const int PRINTER_ENUM_FAVORITE = 4;

		// Token: 0x040007D5 RID: 2005
		public const int PRINTER_ENUM_NAME = 8;

		// Token: 0x040007D6 RID: 2006
		public const int PRINTER_ENUM_REMOTE = 16;

		// Token: 0x040007D7 RID: 2007
		public const int PRINTER_ENUM_SHARED = 32;

		// Token: 0x040007D8 RID: 2008
		public const int PRINTER_ENUM_NETWORK = 64;

		// Token: 0x040007D9 RID: 2009
		public const int PRINTER_ENUM_EXPAND = 16384;

		// Token: 0x040007DA RID: 2010
		public const int PRINTER_ENUM_CONTAINER = 32768;

		// Token: 0x040007DB RID: 2011
		public const int PRINTER_ENUM_ICONMASK = 16711680;

		// Token: 0x040007DC RID: 2012
		public const int PRINTER_ENUM_ICON1 = 65536;

		// Token: 0x040007DD RID: 2013
		public const int PRINTER_ENUM_ICON2 = 131072;

		// Token: 0x040007DE RID: 2014
		public const int PRINTER_ENUM_ICON3 = 262144;

		// Token: 0x040007DF RID: 2015
		public const int PRINTER_ENUM_ICON4 = 524288;

		// Token: 0x040007E0 RID: 2016
		public const int PRINTER_ENUM_ICON5 = 1048576;

		// Token: 0x040007E1 RID: 2017
		public const int PRINTER_ENUM_ICON6 = 2097152;

		// Token: 0x040007E2 RID: 2018
		public const int PRINTER_ENUM_ICON7 = 4194304;

		// Token: 0x040007E3 RID: 2019
		public const int PRINTER_ENUM_ICON8 = 8388608;

		// Token: 0x040007E4 RID: 2020
		public const int DC_BINADJUST = 19;

		// Token: 0x040007E5 RID: 2021
		public const int DC_EMF_COMPLIANT = 20;

		// Token: 0x040007E6 RID: 2022
		public const int DC_DATATYPE_PRODUCED = 21;

		// Token: 0x040007E7 RID: 2023
		public const int DC_COLLATE = 22;

		// Token: 0x040007E8 RID: 2024
		public const int DCTT_BITMAP = 1;

		// Token: 0x040007E9 RID: 2025
		public const int DCTT_DOWNLOAD = 2;

		// Token: 0x040007EA RID: 2026
		public const int DCTT_SUBDEV = 4;

		// Token: 0x040007EB RID: 2027
		public const int DCTT_DOWNLOAD_OUTLINE = 8;

		// Token: 0x040007EC RID: 2028
		public const int DCBA_FACEUPNONE = 0;

		// Token: 0x040007ED RID: 2029
		public const int DCBA_FACEUPCENTER = 1;

		// Token: 0x040007EE RID: 2030
		public const int DCBA_FACEUPLEFT = 2;

		// Token: 0x040007EF RID: 2031
		public const int DCBA_FACEUPRIGHT = 3;

		// Token: 0x040007F0 RID: 2032
		public const int DCBA_FACEDOWNNONE = 256;

		// Token: 0x040007F1 RID: 2033
		public const int DCBA_FACEDOWNCENTER = 257;

		// Token: 0x040007F2 RID: 2034
		public const int DCBA_FACEDOWNLEFT = 258;

		// Token: 0x040007F3 RID: 2035
		public const int DCBA_FACEDOWNRIGHT = 259;

		// Token: 0x040007F4 RID: 2036
		public const int SRCPAINT = 15597702;

		// Token: 0x040007F5 RID: 2037
		public const int SRCAND = 8913094;

		// Token: 0x040007F6 RID: 2038
		public const int SRCINVERT = 6684742;

		// Token: 0x040007F7 RID: 2039
		public const int SRCERASE = 4457256;

		// Token: 0x040007F8 RID: 2040
		public const int NOTSRCCOPY = 3342344;

		// Token: 0x040007F9 RID: 2041
		public const int NOTSRCERASE = 1114278;

		// Token: 0x040007FA RID: 2042
		public const int MERGECOPY = 12583114;

		// Token: 0x040007FB RID: 2043
		public const int MERGEPAINT = 12255782;

		// Token: 0x040007FC RID: 2044
		public const int PATCOPY = 15728673;

		// Token: 0x040007FD RID: 2045
		public const int PATPAINT = 16452105;

		// Token: 0x040007FE RID: 2046
		public const int PATINVERT = 5898313;

		// Token: 0x040007FF RID: 2047
		public const int DSTINVERT = 5570569;

		// Token: 0x04000800 RID: 2048
		public const int BLACKNESS = 66;

		// Token: 0x04000801 RID: 2049
		public const int WHITENESS = 16711778;

		// Token: 0x04000802 RID: 2050
		public const int CAPTUREBLT = 1073741824;

		// Token: 0x04000803 RID: 2051
		public const int SM_CXSCREEN = 0;

		// Token: 0x04000804 RID: 2052
		public const int SM_CYSCREEN = 1;

		// Token: 0x04000805 RID: 2053
		public const int SM_CXVSCROLL = 2;

		// Token: 0x04000806 RID: 2054
		public const int SM_CYHSCROLL = 3;

		// Token: 0x04000807 RID: 2055
		public const int SM_CYCAPTION = 4;

		// Token: 0x04000808 RID: 2056
		public const int SM_CXBORDER = 5;

		// Token: 0x04000809 RID: 2057
		public const int SM_CYBORDER = 6;

		// Token: 0x0400080A RID: 2058
		public const int SM_CXDLGFRAME = 7;

		// Token: 0x0400080B RID: 2059
		public const int SM_CYDLGFRAME = 8;

		// Token: 0x0400080C RID: 2060
		public const int SM_CYVTHUMB = 9;

		// Token: 0x0400080D RID: 2061
		public const int SM_CXHTHUMB = 10;

		// Token: 0x0400080E RID: 2062
		public const int SM_CXICON = 11;

		// Token: 0x0400080F RID: 2063
		public const int SM_CYICON = 12;

		// Token: 0x04000810 RID: 2064
		public const int SM_CXCURSOR = 13;

		// Token: 0x04000811 RID: 2065
		public const int SM_CYCURSOR = 14;

		// Token: 0x04000812 RID: 2066
		public const int SM_CYMENU = 15;

		// Token: 0x04000813 RID: 2067
		public const int SM_CXFULLSCREEN = 16;

		// Token: 0x04000814 RID: 2068
		public const int SM_CYFULLSCREEN = 17;

		// Token: 0x04000815 RID: 2069
		public const int SM_CYKANJIWINDOW = 18;

		// Token: 0x04000816 RID: 2070
		public const int SM_MOUSEPRESENT = 19;

		// Token: 0x04000817 RID: 2071
		public const int SM_CYVSCROLL = 20;

		// Token: 0x04000818 RID: 2072
		public const int SM_CXHSCROLL = 21;

		// Token: 0x04000819 RID: 2073
		public const int SM_DEBUG = 22;

		// Token: 0x0400081A RID: 2074
		public const int SM_SWAPBUTTON = 23;

		// Token: 0x0400081B RID: 2075
		public const int SM_RESERVED1 = 24;

		// Token: 0x0400081C RID: 2076
		public const int SM_RESERVED2 = 25;

		// Token: 0x0400081D RID: 2077
		public const int SM_RESERVED3 = 26;

		// Token: 0x0400081E RID: 2078
		public const int SM_RESERVED4 = 27;

		// Token: 0x0400081F RID: 2079
		public const int SM_CXMIN = 28;

		// Token: 0x04000820 RID: 2080
		public const int SM_CYMIN = 29;

		// Token: 0x04000821 RID: 2081
		public const int SM_CXSIZE = 30;

		// Token: 0x04000822 RID: 2082
		public const int SM_CYSIZE = 31;

		// Token: 0x04000823 RID: 2083
		public const int SM_CXFRAME = 32;

		// Token: 0x04000824 RID: 2084
		public const int SM_CYFRAME = 33;

		// Token: 0x04000825 RID: 2085
		public const int SM_CXMINTRACK = 34;

		// Token: 0x04000826 RID: 2086
		public const int SM_CYMINTRACK = 35;

		// Token: 0x04000827 RID: 2087
		public const int SM_CXDOUBLECLK = 36;

		// Token: 0x04000828 RID: 2088
		public const int SM_CYDOUBLECLK = 37;

		// Token: 0x04000829 RID: 2089
		public const int SM_CXICONSPACING = 38;

		// Token: 0x0400082A RID: 2090
		public const int SM_CYICONSPACING = 39;

		// Token: 0x0400082B RID: 2091
		public const int SM_MENUDROPALIGNMENT = 40;

		// Token: 0x0400082C RID: 2092
		public const int SM_PENWINDOWS = 41;

		// Token: 0x0400082D RID: 2093
		public const int SM_DBCSENABLED = 42;

		// Token: 0x0400082E RID: 2094
		public const int SM_CMOUSEBUTTONS = 43;

		// Token: 0x0400082F RID: 2095
		public const int SM_CXFIXEDFRAME = 7;

		// Token: 0x04000830 RID: 2096
		public const int SM_CYFIXEDFRAME = 8;

		// Token: 0x04000831 RID: 2097
		public const int SM_CXSIZEFRAME = 32;

		// Token: 0x04000832 RID: 2098
		public const int SM_CYSIZEFRAME = 33;

		// Token: 0x04000833 RID: 2099
		public const int SM_SECURE = 44;

		// Token: 0x04000834 RID: 2100
		public const int SM_CXEDGE = 45;

		// Token: 0x04000835 RID: 2101
		public const int SM_CYEDGE = 46;

		// Token: 0x04000836 RID: 2102
		public const int SM_CXMINSPACING = 47;

		// Token: 0x04000837 RID: 2103
		public const int SM_CYMINSPACING = 48;

		// Token: 0x04000838 RID: 2104
		public const int SM_CXSMICON = 49;

		// Token: 0x04000839 RID: 2105
		public const int SM_CYSMICON = 50;

		// Token: 0x0400083A RID: 2106
		public const int SM_CYSMCAPTION = 51;

		// Token: 0x0400083B RID: 2107
		public const int SM_CXSMSIZE = 52;

		// Token: 0x0400083C RID: 2108
		public const int SM_CYSMSIZE = 53;

		// Token: 0x0400083D RID: 2109
		public const int SM_CXMENUSIZE = 54;

		// Token: 0x0400083E RID: 2110
		public const int SM_CYMENUSIZE = 55;

		// Token: 0x0400083F RID: 2111
		public const int SM_ARRANGE = 56;

		// Token: 0x04000840 RID: 2112
		public const int SM_CXMINIMIZED = 57;

		// Token: 0x04000841 RID: 2113
		public const int SM_CYMINIMIZED = 58;

		// Token: 0x04000842 RID: 2114
		public const int SM_CXMAXTRACK = 59;

		// Token: 0x04000843 RID: 2115
		public const int SM_CYMAXTRACK = 60;

		// Token: 0x04000844 RID: 2116
		public const int SM_CXMAXIMIZED = 61;

		// Token: 0x04000845 RID: 2117
		public const int SM_CYMAXIMIZED = 62;

		// Token: 0x04000846 RID: 2118
		public const int SM_NETWORK = 63;

		// Token: 0x04000847 RID: 2119
		public const int SM_CLEANBOOT = 67;

		// Token: 0x04000848 RID: 2120
		public const int SM_CXDRAG = 68;

		// Token: 0x04000849 RID: 2121
		public const int SM_CYDRAG = 69;

		// Token: 0x0400084A RID: 2122
		public const int SM_SHOWSOUNDS = 70;

		// Token: 0x0400084B RID: 2123
		public const int SM_CXMENUCHECK = 71;

		// Token: 0x0400084C RID: 2124
		public const int SM_CYMENUCHECK = 72;

		// Token: 0x0400084D RID: 2125
		public const int SM_SLOWMACHINE = 73;

		// Token: 0x0400084E RID: 2126
		public const int SM_MIDEASTENABLED = 74;

		// Token: 0x0400084F RID: 2127
		public const int SM_MOUSEWHEELPRESENT = 75;

		// Token: 0x04000850 RID: 2128
		public const int SM_XVIRTUALSCREEN = 76;

		// Token: 0x04000851 RID: 2129
		public const int SM_YVIRTUALSCREEN = 77;

		// Token: 0x04000852 RID: 2130
		public const int SM_CXVIRTUALSCREEN = 78;

		// Token: 0x04000853 RID: 2131
		public const int SM_CYVIRTUALSCREEN = 79;

		// Token: 0x04000854 RID: 2132
		public const int SM_CMONITORS = 80;

		// Token: 0x04000855 RID: 2133
		public const int SM_SAMEDISPLAYFORMAT = 81;

		// Token: 0x04000856 RID: 2134
		public const int SM_CMETRICS = 83;

		// Token: 0x04000857 RID: 2135
		public const int GM_COMPATIBLE = 1;

		// Token: 0x04000858 RID: 2136
		public const int GM_ADVANCED = 2;

		// Token: 0x04000859 RID: 2137
		public const int MWT_IDENTITY = 1;

		// Token: 0x0400085A RID: 2138
		public const int FW_DONTCARE = 0;

		// Token: 0x0400085B RID: 2139
		public const int FW_NORMAL = 400;

		// Token: 0x0400085C RID: 2140
		public const int FW_BOLD = 700;

		// Token: 0x0400085D RID: 2141
		public const int ANSI_CHARSET = 0;

		// Token: 0x0400085E RID: 2142
		public const int DEFAULT_CHARSET = 1;

		// Token: 0x0400085F RID: 2143
		public const int OUT_DEFAULT_PRECIS = 0;

		// Token: 0x04000860 RID: 2144
		public const int OUT_TT_PRECIS = 4;

		// Token: 0x04000861 RID: 2145
		public const int OUT_TT_ONLY_PRECIS = 7;

		// Token: 0x04000862 RID: 2146
		public const int CLIP_DEFAULT_PRECIS = 0;

		// Token: 0x04000863 RID: 2147
		public const int DEFAULT_QUALITY = 0;

		// Token: 0x04000864 RID: 2148
		public const int MM_TEXT = 1;

		// Token: 0x04000865 RID: 2149
		public const int OBJ_FONT = 6;

		// Token: 0x04000866 RID: 2150
		public const int TA_DEFAULT = 0;

		// Token: 0x04000867 RID: 2151
		public const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 256;

		// Token: 0x04000868 RID: 2152
		public const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x04000869 RID: 2153
		public const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x0400086A RID: 2154
		public const int FORMAT_MESSAGE_DEFAULT = 4608;

		// Token: 0x0400086B RID: 2155
		public const int NOMIRRORBITMAP = -2147483648;

		// Token: 0x0400086C RID: 2156
		public const int QUERYESCSUPPORT = 8;

		// Token: 0x0400086D RID: 2157
		public const int CHECKJPEGFORMAT = 4119;

		// Token: 0x0400086E RID: 2158
		public const int CHECKPNGFORMAT = 4120;

		// Token: 0x0400086F RID: 2159
		public const int ERROR_ACCESS_DENIED = 5;

		// Token: 0x04000870 RID: 2160
		public const int ERROR_INVALID_PARAMETER = 87;

		// Token: 0x04000871 RID: 2161
		public const int ERROR_PROC_NOT_FOUND = 127;

		// Token: 0x04000872 RID: 2162
		public static IntPtr InvalidIntPtr = (IntPtr)(-1);

		// Token: 0x02000091 RID: 145
		[SuppressUnmanagedCodeSecurity]
		internal class Gdip
		{
			// Token: 0x06000851 RID: 2129 RVA: 0x0001F17B File Offset: 0x0001E17B
			static Gdip()
			{
				SafeNativeMethods.Gdip.Initialize();
			}

			// Token: 0x170002F6 RID: 758
			// (get) Token: 0x06000852 RID: 2130 RVA: 0x0001F1B6 File Offset: 0x0001E1B6
			private static string AtomName
			{
				get
				{
					if (SafeNativeMethods.Gdip.atomName == null)
					{
						SafeNativeMethods.Gdip.atomName = VersioningHelper.MakeVersionSafeName("GDI+Atom", ResourceScope.Machine, ResourceScope.AppDomain);
					}
					return SafeNativeMethods.Gdip.atomName;
				}
			}

			// Token: 0x06000853 RID: 2131 RVA: 0x0001F1D5 File Offset: 0x0001E1D5
			private static bool EnsureAtomInitialized()
			{
				if (SafeNativeMethods.Gdip.FindAtom(SafeNativeMethods.Gdip.AtomName) != 0)
				{
					return true;
				}
				SafeNativeMethods.Gdip.hAtom = SafeNativeMethods.Gdip.AddAtom(SafeNativeMethods.Gdip.AtomName);
				return false;
			}

			// Token: 0x170002F7 RID: 759
			// (get) Token: 0x06000854 RID: 2132 RVA: 0x0001F1F5 File Offset: 0x0001E1F5
			private static bool IsShutdown
			{
				get
				{
					return SafeNativeMethods.Gdip.FindAtom(SafeNativeMethods.Gdip.AtomName) == 0;
				}
			}

			// Token: 0x170002F8 RID: 760
			// (get) Token: 0x06000855 RID: 2133 RVA: 0x0001F204 File Offset: 0x0001E204
			internal static IDictionary ThreadData
			{
				get
				{
					LocalDataStoreSlot namedDataSlot = Thread.GetNamedDataSlot("system.drawing.threaddata");
					IDictionary dictionary = (IDictionary)Thread.GetData(namedDataSlot);
					if (dictionary == null)
					{
						dictionary = new Hashtable();
						Thread.SetData(namedDataSlot, dictionary);
					}
					return dictionary;
				}
			}

			// Token: 0x06000856 RID: 2134 RVA: 0x0001F239 File Offset: 0x0001E239
			private static void DestroyAtom()
			{
				if (SafeNativeMethods.Gdip.hAtom != 0)
				{
					SafeNativeMethods.Gdip.DeleteAtom(SafeNativeMethods.Gdip.hAtom);
				}
			}

			// Token: 0x06000857 RID: 2135 RVA: 0x0001F250 File Offset: 0x0001E250
			private static void Initialize()
			{
				if (!SafeNativeMethods.Gdip.EnsureAtomInitialized())
				{
					string text = null;
					IntPtr intPtr;
					SafeNativeMethods.Gdip.LoadLibraryShim("Gdiplus.dll", text, (IntPtr)0, out intPtr);
					SafeNativeMethods.Gdip.StartupInput @default = SafeNativeMethods.Gdip.StartupInput.GetDefault();
					SafeNativeMethods.Gdip.StartupOutput startupOutput;
					int num = SafeNativeMethods.Gdip.GdiplusStartup(out SafeNativeMethods.Gdip.initToken, ref @default, out startupOutput);
					if (num != 0)
					{
						throw SafeNativeMethods.Gdip.StatusException(num);
					}
				}
				AppDomain.CurrentDomain.ProcessExit += SafeNativeMethods.Gdip.OnProcessExit;
			}

			// Token: 0x06000858 RID: 2136 RVA: 0x0001F2B4 File Offset: 0x0001E2B4
			private static void Shutdown()
			{
				SafeNativeMethods.Gdip.DestroyAtom();
				if (!SafeNativeMethods.Gdip.IsShutdown)
				{
					LocalDataStoreSlot namedDataSlot = Thread.GetNamedDataSlot("system.drawing.threaddata");
					Thread.SetData(namedDataSlot, null);
					GC.Collect();
					GC.WaitForPendingFinalizers();
					if (SafeNativeMethods.Gdip.initToken != IntPtr.Zero)
					{
						SafeNativeMethods.Gdip.GdiplusShutdown(new HandleRef(null, SafeNativeMethods.Gdip.initToken));
					}
				}
			}

			// Token: 0x06000859 RID: 2137 RVA: 0x0001F30A File Offset: 0x0001E30A
			[PrePrepareMethod]
			private static void OnProcessExit(object sender, EventArgs e)
			{
				SafeNativeMethods.Gdip.Shutdown();
			}

			// Token: 0x0600085A RID: 2138
			[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			private static extern int GetCurrentProcessId();

			// Token: 0x0600085B RID: 2139
			[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			private static extern ushort AddAtom(string lpString);

			// Token: 0x0600085C RID: 2140
			[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			private static extern ushort DeleteAtom(ushort hAtom);

			// Token: 0x0600085D RID: 2141
			[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			private static extern ushort FindAtom(string lpString);

			// Token: 0x0600085E RID: 2142
			[DllImport("mscoree.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
			private static extern int LoadLibraryShim(string dllName, string version, IntPtr reserved, out IntPtr dllModule);

			// Token: 0x0600085F RID: 2143
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			private static extern int GdiplusStartup(out IntPtr token, ref SafeNativeMethods.Gdip.StartupInput input, out SafeNativeMethods.Gdip.StartupOutput output);

			// Token: 0x06000860 RID: 2144
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			private static extern void GdiplusShutdown(HandleRef token);

			// Token: 0x06000861 RID: 2145
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreatePath(int brushMode, out IntPtr path);

			// Token: 0x06000862 RID: 2146
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreatePath2(HandleRef points, HandleRef types, int count, int brushMode, out IntPtr path);

			// Token: 0x06000863 RID: 2147
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreatePath2I(HandleRef points, HandleRef types, int count, int brushMode, out IntPtr path);

			// Token: 0x06000864 RID: 2148
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipClonePath(HandleRef path, out IntPtr clonepath);

			// Token: 0x06000865 RID: 2149
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDeletePath", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDeletePath(HandleRef path);

			// Token: 0x06000866 RID: 2150 RVA: 0x0001F314 File Offset: 0x0001E314
			internal static int GdipDeletePath(HandleRef path)
			{
				if (SafeNativeMethods.Gdip.IsShutdown)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDeletePath(path);
			}

			// Token: 0x06000867 RID: 2151
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipResetPath(HandleRef path);

			// Token: 0x06000868 RID: 2152
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPointCount(HandleRef path, out int count);

			// Token: 0x06000869 RID: 2153
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathTypes(HandleRef path, byte[] types, int count);

			// Token: 0x0600086A RID: 2154
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathPoints(HandleRef path, HandleRef points, int count);

			// Token: 0x0600086B RID: 2155
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathFillMode(HandleRef path, out int fillmode);

			// Token: 0x0600086C RID: 2156
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPathFillMode(HandleRef path, int fillmode);

			// Token: 0x0600086D RID: 2157
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathData(HandleRef path, IntPtr pathData);

			// Token: 0x0600086E RID: 2158
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipStartPathFigure(HandleRef path);

			// Token: 0x0600086F RID: 2159
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipClosePathFigure(HandleRef path);

			// Token: 0x06000870 RID: 2160
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipClosePathFigures(HandleRef path);

			// Token: 0x06000871 RID: 2161
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPathMarker(HandleRef path);

			// Token: 0x06000872 RID: 2162
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipClearPathMarkers(HandleRef path);

			// Token: 0x06000873 RID: 2163
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipReversePath(HandleRef path);

			// Token: 0x06000874 RID: 2164
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathLastPoint(HandleRef path, GPPOINTF lastPoint);

			// Token: 0x06000875 RID: 2165
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathLine(HandleRef path, float x1, float y1, float x2, float y2);

			// Token: 0x06000876 RID: 2166
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathLine2(HandleRef path, HandleRef memorypts, int count);

			// Token: 0x06000877 RID: 2167
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathArc(HandleRef path, float x, float y, float width, float height, float startAngle, float sweepAngle);

			// Token: 0x06000878 RID: 2168
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathBezier(HandleRef path, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4);

			// Token: 0x06000879 RID: 2169
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathBeziers(HandleRef path, HandleRef memorypts, int count);

			// Token: 0x0600087A RID: 2170
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathCurve(HandleRef path, HandleRef memorypts, int count);

			// Token: 0x0600087B RID: 2171
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathCurve2(HandleRef path, HandleRef memorypts, int count, float tension);

			// Token: 0x0600087C RID: 2172
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathCurve3(HandleRef path, HandleRef memorypts, int count, int offset, int numberOfSegments, float tension);

			// Token: 0x0600087D RID: 2173
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathClosedCurve(HandleRef path, HandleRef memorypts, int count);

			// Token: 0x0600087E RID: 2174
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathClosedCurve2(HandleRef path, HandleRef memorypts, int count, float tension);

			// Token: 0x0600087F RID: 2175
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathRectangle(HandleRef path, float x, float y, float width, float height);

			// Token: 0x06000880 RID: 2176
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathRectangles(HandleRef path, HandleRef rects, int count);

			// Token: 0x06000881 RID: 2177
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathEllipse(HandleRef path, float x, float y, float width, float height);

			// Token: 0x06000882 RID: 2178
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathPie(HandleRef path, float x, float y, float width, float height, float startAngle, float sweepAngle);

			// Token: 0x06000883 RID: 2179
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathPolygon(HandleRef path, HandleRef memorypts, int count);

			// Token: 0x06000884 RID: 2180
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathPath(HandleRef path, HandleRef addingPath, bool connect);

			// Token: 0x06000885 RID: 2181
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathString(HandleRef path, string s, int length, HandleRef fontFamily, int style, float emSize, ref GPRECTF layoutRect, HandleRef format);

			// Token: 0x06000886 RID: 2182
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathStringI(HandleRef path, string s, int length, HandleRef fontFamily, int style, float emSize, ref GPRECT layoutRect, HandleRef format);

			// Token: 0x06000887 RID: 2183
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathLineI(HandleRef path, int x1, int y1, int x2, int y2);

			// Token: 0x06000888 RID: 2184
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathLine2I(HandleRef path, HandleRef memorypts, int count);

			// Token: 0x06000889 RID: 2185
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathArcI(HandleRef path, int x, int y, int width, int height, float startAngle, float sweepAngle);

			// Token: 0x0600088A RID: 2186
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathBezierI(HandleRef path, int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4);

			// Token: 0x0600088B RID: 2187
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathBeziersI(HandleRef path, HandleRef memorypts, int count);

			// Token: 0x0600088C RID: 2188
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathCurveI(HandleRef path, HandleRef memorypts, int count);

			// Token: 0x0600088D RID: 2189
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathCurve2I(HandleRef path, HandleRef memorypts, int count, float tension);

			// Token: 0x0600088E RID: 2190
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathCurve3I(HandleRef path, HandleRef memorypts, int count, int offset, int numberOfSegments, float tension);

			// Token: 0x0600088F RID: 2191
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathClosedCurveI(HandleRef path, HandleRef memorypts, int count);

			// Token: 0x06000890 RID: 2192
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathClosedCurve2I(HandleRef path, HandleRef memorypts, int count, float tension);

			// Token: 0x06000891 RID: 2193
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathRectangleI(HandleRef path, int x, int y, int width, int height);

			// Token: 0x06000892 RID: 2194
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathRectanglesI(HandleRef path, HandleRef rects, int count);

			// Token: 0x06000893 RID: 2195
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathEllipseI(HandleRef path, int x, int y, int width, int height);

			// Token: 0x06000894 RID: 2196
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathPieI(HandleRef path, int x, int y, int width, int height, float startAngle, float sweepAngle);

			// Token: 0x06000895 RID: 2197
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipAddPathPolygonI(HandleRef path, HandleRef memorypts, int count);

			// Token: 0x06000896 RID: 2198
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFlattenPath(HandleRef path, HandleRef matrixfloat, float flatness);

			// Token: 0x06000897 RID: 2199
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipWidenPath(HandleRef path, HandleRef pen, HandleRef matrix, float flatness);

			// Token: 0x06000898 RID: 2200
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipWarpPath(HandleRef path, HandleRef matrix, HandleRef points, int count, float srcX, float srcY, float srcWidth, float srcHeight, WarpMode warpMode, float flatness);

			// Token: 0x06000899 RID: 2201
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTransformPath(HandleRef path, HandleRef matrix);

			// Token: 0x0600089A RID: 2202
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathWorldBounds(HandleRef path, ref GPRECTF gprectf, HandleRef matrix, HandleRef pen);

			// Token: 0x0600089B RID: 2203
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsVisiblePathPoint(HandleRef path, float x, float y, HandleRef graphics, out int boolean);

			// Token: 0x0600089C RID: 2204
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsVisiblePathPointI(HandleRef path, int x, int y, HandleRef graphics, out int boolean);

			// Token: 0x0600089D RID: 2205
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsOutlineVisiblePathPoint(HandleRef path, float x, float y, HandleRef pen, HandleRef graphics, out int boolean);

			// Token: 0x0600089E RID: 2206
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsOutlineVisiblePathPointI(HandleRef path, int x, int y, HandleRef pen, HandleRef graphics, out int boolean);

			// Token: 0x0600089F RID: 2207
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreatePathIter(out IntPtr pathIter, HandleRef path);

			// Token: 0x060008A0 RID: 2208
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDeletePathIter", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDeletePathIter(HandleRef pathIter);

			// Token: 0x060008A1 RID: 2209 RVA: 0x0001F334 File Offset: 0x0001E334
			internal static int GdipDeletePathIter(HandleRef pathIter)
			{
				if (SafeNativeMethods.Gdip.IsShutdown)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDeletePathIter(pathIter);
			}

			// Token: 0x060008A2 RID: 2210
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPathIterNextSubpath(HandleRef pathIter, out int resultCount, out int startIndex, out int endIndex, out bool isClosed);

			// Token: 0x060008A3 RID: 2211
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPathIterNextSubpathPath(HandleRef pathIter, out int resultCount, HandleRef path, out bool isClosed);

			// Token: 0x060008A4 RID: 2212
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPathIterNextPathType(HandleRef pathIter, out int resultCount, out byte pathType, out int startIndex, out int endIndex);

			// Token: 0x060008A5 RID: 2213
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPathIterNextMarker(HandleRef pathIter, out int resultCount, out int startIndex, out int endIndex);

			// Token: 0x060008A6 RID: 2214
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPathIterNextMarkerPath(HandleRef pathIter, out int resultCount, HandleRef path);

			// Token: 0x060008A7 RID: 2215
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPathIterGetCount(HandleRef pathIter, out int count);

			// Token: 0x060008A8 RID: 2216
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPathIterGetSubpathCount(HandleRef pathIter, out int count);

			// Token: 0x060008A9 RID: 2217
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPathIterHasCurve(HandleRef pathIter, out bool hasCurve);

			// Token: 0x060008AA RID: 2218
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPathIterRewind(HandleRef pathIter);

			// Token: 0x060008AB RID: 2219
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPathIterEnumerate(HandleRef pathIter, out int resultCount, IntPtr memoryPts, [In] [Out] byte[] types, int count);

			// Token: 0x060008AC RID: 2220
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPathIterCopyData(HandleRef pathIter, out int resultCount, IntPtr memoryPts, [In] [Out] byte[] types, int startIndex, int endIndex);

			// Token: 0x060008AD RID: 2221
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateMatrix(out IntPtr matrix);

			// Token: 0x060008AE RID: 2222
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateMatrix2(float m11, float m12, float m21, float m22, float dx, float dy, out IntPtr matrix);

			// Token: 0x060008AF RID: 2223
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateMatrix3(ref GPRECTF rect, HandleRef dstplg, out IntPtr matrix);

			// Token: 0x060008B0 RID: 2224
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateMatrix3I(ref GPRECT rect, HandleRef dstplg, out IntPtr matrix);

			// Token: 0x060008B1 RID: 2225
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCloneMatrix(HandleRef matrix, out IntPtr cloneMatrix);

			// Token: 0x060008B2 RID: 2226
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDeleteMatrix", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDeleteMatrix(HandleRef matrix);

			// Token: 0x060008B3 RID: 2227 RVA: 0x0001F354 File Offset: 0x0001E354
			internal static int GdipDeleteMatrix(HandleRef matrix)
			{
				if (SafeNativeMethods.Gdip.IsShutdown)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDeleteMatrix(matrix);
			}

			// Token: 0x060008B4 RID: 2228
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetMatrixElements(HandleRef matrix, float m11, float m12, float m21, float m22, float dx, float dy);

			// Token: 0x060008B5 RID: 2229
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipMultiplyMatrix(HandleRef matrix, HandleRef matrix2, MatrixOrder order);

			// Token: 0x060008B6 RID: 2230
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTranslateMatrix(HandleRef matrix, float offsetX, float offsetY, MatrixOrder order);

			// Token: 0x060008B7 RID: 2231
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipScaleMatrix(HandleRef matrix, float scaleX, float scaleY, MatrixOrder order);

			// Token: 0x060008B8 RID: 2232
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRotateMatrix(HandleRef matrix, float angle, MatrixOrder order);

			// Token: 0x060008B9 RID: 2233
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipShearMatrix(HandleRef matrix, float shearX, float shearY, MatrixOrder order);

			// Token: 0x060008BA RID: 2234
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipInvertMatrix(HandleRef matrix);

			// Token: 0x060008BB RID: 2235
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTransformMatrixPoints(HandleRef matrix, HandleRef pts, int count);

			// Token: 0x060008BC RID: 2236
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTransformMatrixPointsI(HandleRef matrix, HandleRef pts, int count);

			// Token: 0x060008BD RID: 2237
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipVectorTransformMatrixPoints(HandleRef matrix, HandleRef pts, int count);

			// Token: 0x060008BE RID: 2238
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipVectorTransformMatrixPointsI(HandleRef matrix, HandleRef pts, int count);

			// Token: 0x060008BF RID: 2239
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetMatrixElements(HandleRef matrix, IntPtr m);

			// Token: 0x060008C0 RID: 2240
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsMatrixInvertible(HandleRef matrix, out int boolean);

			// Token: 0x060008C1 RID: 2241
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsMatrixIdentity(HandleRef matrix, out int boolean);

			// Token: 0x060008C2 RID: 2242
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsMatrixEqual(HandleRef matrix, HandleRef matrix2, out int boolean);

			// Token: 0x060008C3 RID: 2243
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateRegion(out IntPtr region);

			// Token: 0x060008C4 RID: 2244
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateRegionRect(ref GPRECTF gprectf, out IntPtr region);

			// Token: 0x060008C5 RID: 2245
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateRegionRectI(ref GPRECT gprect, out IntPtr region);

			// Token: 0x060008C6 RID: 2246
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateRegionPath(HandleRef path, out IntPtr region);

			// Token: 0x060008C7 RID: 2247
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateRegionRgnData(byte[] rgndata, int size, out IntPtr region);

			// Token: 0x060008C8 RID: 2248
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateRegionHrgn(HandleRef hRgn, out IntPtr region);

			// Token: 0x060008C9 RID: 2249
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCloneRegion(HandleRef region, out IntPtr cloneregion);

			// Token: 0x060008CA RID: 2250
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDeleteRegion", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDeleteRegion(HandleRef region);

			// Token: 0x060008CB RID: 2251 RVA: 0x0001F374 File Offset: 0x0001E374
			internal static int GdipDeleteRegion(HandleRef region)
			{
				if (SafeNativeMethods.Gdip.IsShutdown)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDeleteRegion(region);
			}

			// Token: 0x060008CC RID: 2252
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetInfinite(HandleRef region);

			// Token: 0x060008CD RID: 2253
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetEmpty(HandleRef region);

			// Token: 0x060008CE RID: 2254
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCombineRegionRect(HandleRef region, ref GPRECTF gprectf, CombineMode mode);

			// Token: 0x060008CF RID: 2255
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCombineRegionRectI(HandleRef region, ref GPRECT gprect, CombineMode mode);

			// Token: 0x060008D0 RID: 2256
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCombineRegionPath(HandleRef region, HandleRef path, CombineMode mode);

			// Token: 0x060008D1 RID: 2257
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCombineRegionRegion(HandleRef region, HandleRef region2, CombineMode mode);

			// Token: 0x060008D2 RID: 2258
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTranslateRegion(HandleRef region, float dx, float dy);

			// Token: 0x060008D3 RID: 2259
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTranslateRegionI(HandleRef region, int dx, int dy);

			// Token: 0x060008D4 RID: 2260
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTransformRegion(HandleRef region, HandleRef matrix);

			// Token: 0x060008D5 RID: 2261
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetRegionBounds(HandleRef region, HandleRef graphics, ref GPRECTF gprectf);

			// Token: 0x060008D6 RID: 2262
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetRegionHRgn(HandleRef region, HandleRef graphics, out IntPtr hrgn);

			// Token: 0x060008D7 RID: 2263
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsEmptyRegion(HandleRef region, HandleRef graphics, out int boolean);

			// Token: 0x060008D8 RID: 2264
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsInfiniteRegion(HandleRef region, HandleRef graphics, out int boolean);

			// Token: 0x060008D9 RID: 2265
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsEqualRegion(HandleRef region, HandleRef region2, HandleRef graphics, out int boolean);

			// Token: 0x060008DA RID: 2266
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetRegionDataSize(HandleRef region, out int bufferSize);

			// Token: 0x060008DB RID: 2267
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetRegionData(HandleRef region, byte[] regionData, int bufferSize, out int sizeFilled);

			// Token: 0x060008DC RID: 2268
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsVisibleRegionPoint(HandleRef region, float X, float Y, HandleRef graphics, out int boolean);

			// Token: 0x060008DD RID: 2269
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsVisibleRegionPointI(HandleRef region, int X, int Y, HandleRef graphics, out int boolean);

			// Token: 0x060008DE RID: 2270
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsVisibleRegionRect(HandleRef region, float X, float Y, float width, float height, HandleRef graphics, out int boolean);

			// Token: 0x060008DF RID: 2271
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsVisibleRegionRectI(HandleRef region, int X, int Y, int width, int height, HandleRef graphics, out int boolean);

			// Token: 0x060008E0 RID: 2272
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetRegionScansCount(HandleRef region, out int count, HandleRef matrix);

			// Token: 0x060008E1 RID: 2273
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetRegionScans(HandleRef region, IntPtr rects, out int count, HandleRef matrix);

			// Token: 0x060008E2 RID: 2274
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCloneBrush(HandleRef brush, out IntPtr clonebrush);

			// Token: 0x060008E3 RID: 2275
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDeleteBrush", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDeleteBrush(HandleRef brush);

			// Token: 0x060008E4 RID: 2276 RVA: 0x0001F394 File Offset: 0x0001E394
			internal static int GdipDeleteBrush(HandleRef brush)
			{
				if (SafeNativeMethods.Gdip.IsShutdown)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDeleteBrush(brush);
			}

			// Token: 0x060008E5 RID: 2277
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateHatchBrush(int hatchstyle, int forecol, int backcol, out IntPtr brush);

			// Token: 0x060008E6 RID: 2278
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetHatchStyle(HandleRef brush, out int hatchstyle);

			// Token: 0x060008E7 RID: 2279
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetHatchForegroundColor(HandleRef brush, out int forecol);

			// Token: 0x060008E8 RID: 2280
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetHatchBackgroundColor(HandleRef brush, out int backcol);

			// Token: 0x060008E9 RID: 2281
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateTexture(HandleRef bitmap, int wrapmode, out IntPtr texture);

			// Token: 0x060008EA RID: 2282
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateTexture2(HandleRef bitmap, int wrapmode, float x, float y, float width, float height, out IntPtr texture);

			// Token: 0x060008EB RID: 2283
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateTextureIA(HandleRef bitmap, HandleRef imageAttrib, float x, float y, float width, float height, out IntPtr texture);

			// Token: 0x060008EC RID: 2284
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateTexture2I(HandleRef bitmap, int wrapmode, int x, int y, int width, int height, out IntPtr texture);

			// Token: 0x060008ED RID: 2285
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateTextureIAI(HandleRef bitmap, HandleRef imageAttrib, int x, int y, int width, int height, out IntPtr texture);

			// Token: 0x060008EE RID: 2286
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetTextureTransform(HandleRef brush, HandleRef matrix);

			// Token: 0x060008EF RID: 2287
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetTextureTransform(HandleRef brush, HandleRef matrix);

			// Token: 0x060008F0 RID: 2288
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipResetTextureTransform(HandleRef brush);

			// Token: 0x060008F1 RID: 2289
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipMultiplyTextureTransform(HandleRef brush, HandleRef matrix, MatrixOrder order);

			// Token: 0x060008F2 RID: 2290
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTranslateTextureTransform(HandleRef brush, float dx, float dy, MatrixOrder order);

			// Token: 0x060008F3 RID: 2291
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipScaleTextureTransform(HandleRef brush, float sx, float sy, MatrixOrder order);

			// Token: 0x060008F4 RID: 2292
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRotateTextureTransform(HandleRef brush, float angle, MatrixOrder order);

			// Token: 0x060008F5 RID: 2293
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetTextureWrapMode(HandleRef brush, int wrapMode);

			// Token: 0x060008F6 RID: 2294
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetTextureWrapMode(HandleRef brush, out int wrapMode);

			// Token: 0x060008F7 RID: 2295
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetTextureImage(HandleRef brush, out IntPtr image);

			// Token: 0x060008F8 RID: 2296
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateSolidFill(int color, out IntPtr brush);

			// Token: 0x060008F9 RID: 2297
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetSolidFillColor(HandleRef brush, int color);

			// Token: 0x060008FA RID: 2298
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetSolidFillColor(HandleRef brush, out int color);

			// Token: 0x060008FB RID: 2299
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateLineBrush(GPPOINTF point1, GPPOINTF point2, int color1, int color2, int wrapMode, out IntPtr lineGradient);

			// Token: 0x060008FC RID: 2300
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateLineBrushI(GPPOINT point1, GPPOINT point2, int color1, int color2, int wrapMode, out IntPtr lineGradient);

			// Token: 0x060008FD RID: 2301
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateLineBrushFromRect(ref GPRECTF rect, int color1, int color2, int lineGradientMode, int wrapMode, out IntPtr lineGradient);

			// Token: 0x060008FE RID: 2302
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateLineBrushFromRectI(ref GPRECT rect, int color1, int color2, int lineGradientMode, int wrapMode, out IntPtr lineGradient);

			// Token: 0x060008FF RID: 2303
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateLineBrushFromRectWithAngle(ref GPRECTF rect, int color1, int color2, float angle, bool isAngleScaleable, int wrapMode, out IntPtr lineGradient);

			// Token: 0x06000900 RID: 2304
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateLineBrushFromRectWithAngleI(ref GPRECT rect, int color1, int color2, float angle, bool isAngleScaleable, int wrapMode, out IntPtr lineGradient);

			// Token: 0x06000901 RID: 2305
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetLineColors(HandleRef brush, int color1, int color2);

			// Token: 0x06000902 RID: 2306
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetLineColors(HandleRef brush, int[] colors);

			// Token: 0x06000903 RID: 2307
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetLineRect(HandleRef brush, ref GPRECTF gprectf);

			// Token: 0x06000904 RID: 2308
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetLineGammaCorrection(HandleRef brush, out bool useGammaCorrection);

			// Token: 0x06000905 RID: 2309
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetLineGammaCorrection(HandleRef brush, bool useGammaCorrection);

			// Token: 0x06000906 RID: 2310
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetLineSigmaBlend(HandleRef brush, float focus, float scale);

			// Token: 0x06000907 RID: 2311
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetLineLinearBlend(HandleRef brush, float focus, float scale);

			// Token: 0x06000908 RID: 2312
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetLineBlendCount(HandleRef brush, out int count);

			// Token: 0x06000909 RID: 2313
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetLineBlend(HandleRef brush, IntPtr blend, IntPtr positions, int count);

			// Token: 0x0600090A RID: 2314
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetLineBlend(HandleRef brush, HandleRef blend, HandleRef positions, int count);

			// Token: 0x0600090B RID: 2315
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetLinePresetBlendCount(HandleRef brush, out int count);

			// Token: 0x0600090C RID: 2316
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetLinePresetBlend(HandleRef brush, IntPtr blend, IntPtr positions, int count);

			// Token: 0x0600090D RID: 2317
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetLinePresetBlend(HandleRef brush, HandleRef blend, HandleRef positions, int count);

			// Token: 0x0600090E RID: 2318
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetLineWrapMode(HandleRef brush, int wrapMode);

			// Token: 0x0600090F RID: 2319
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetLineWrapMode(HandleRef brush, out int wrapMode);

			// Token: 0x06000910 RID: 2320
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipResetLineTransform(HandleRef brush);

			// Token: 0x06000911 RID: 2321
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipMultiplyLineTransform(HandleRef brush, HandleRef matrix, MatrixOrder order);

			// Token: 0x06000912 RID: 2322
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetLineTransform(HandleRef brush, HandleRef matrix);

			// Token: 0x06000913 RID: 2323
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetLineTransform(HandleRef brush, HandleRef matrix);

			// Token: 0x06000914 RID: 2324
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTranslateLineTransform(HandleRef brush, float dx, float dy, MatrixOrder order);

			// Token: 0x06000915 RID: 2325
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipScaleLineTransform(HandleRef brush, float sx, float sy, MatrixOrder order);

			// Token: 0x06000916 RID: 2326
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRotateLineTransform(HandleRef brush, float angle, MatrixOrder order);

			// Token: 0x06000917 RID: 2327
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreatePathGradient(HandleRef points, int count, int wrapMode, out IntPtr brush);

			// Token: 0x06000918 RID: 2328
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreatePathGradientI(HandleRef points, int count, int wrapMode, out IntPtr brush);

			// Token: 0x06000919 RID: 2329
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreatePathGradientFromPath(HandleRef path, out IntPtr brush);

			// Token: 0x0600091A RID: 2330
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientCenterColor(HandleRef brush, out int color);

			// Token: 0x0600091B RID: 2331
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPathGradientCenterColor(HandleRef brush, int color);

			// Token: 0x0600091C RID: 2332
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientSurroundColorsWithCount(HandleRef brush, int[] color, ref int count);

			// Token: 0x0600091D RID: 2333
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPathGradientSurroundColorsWithCount(HandleRef brush, int[] argb, ref int count);

			// Token: 0x0600091E RID: 2334
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientCenterPoint(HandleRef brush, GPPOINTF point);

			// Token: 0x0600091F RID: 2335
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPathGradientCenterPoint(HandleRef brush, GPPOINTF point);

			// Token: 0x06000920 RID: 2336
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientRect(HandleRef brush, ref GPRECTF gprectf);

			// Token: 0x06000921 RID: 2337
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientPointCount(HandleRef brush, out int count);

			// Token: 0x06000922 RID: 2338
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientSurroundColorCount(HandleRef brush, out int count);

			// Token: 0x06000923 RID: 2339
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientBlendCount(HandleRef brush, out int count);

			// Token: 0x06000924 RID: 2340
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientBlend(HandleRef brush, IntPtr blend, IntPtr positions, int count);

			// Token: 0x06000925 RID: 2341
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPathGradientBlend(HandleRef brush, HandleRef blend, HandleRef positions, int count);

			// Token: 0x06000926 RID: 2342
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientPresetBlendCount(HandleRef brush, out int count);

			// Token: 0x06000927 RID: 2343
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientPresetBlend(HandleRef brush, IntPtr blend, IntPtr positions, int count);

			// Token: 0x06000928 RID: 2344
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPathGradientPresetBlend(HandleRef brush, HandleRef blend, HandleRef positions, int count);

			// Token: 0x06000929 RID: 2345
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPathGradientSigmaBlend(HandleRef brush, float focus, float scale);

			// Token: 0x0600092A RID: 2346
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPathGradientLinearBlend(HandleRef brush, float focus, float scale);

			// Token: 0x0600092B RID: 2347
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPathGradientWrapMode(HandleRef brush, int wrapmode);

			// Token: 0x0600092C RID: 2348
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientWrapMode(HandleRef brush, out int wrapmode);

			// Token: 0x0600092D RID: 2349
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPathGradientTransform(HandleRef brush, HandleRef matrix);

			// Token: 0x0600092E RID: 2350
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientTransform(HandleRef brush, HandleRef matrix);

			// Token: 0x0600092F RID: 2351
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipResetPathGradientTransform(HandleRef brush);

			// Token: 0x06000930 RID: 2352
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipMultiplyPathGradientTransform(HandleRef brush, HandleRef matrix, MatrixOrder order);

			// Token: 0x06000931 RID: 2353
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTranslatePathGradientTransform(HandleRef brush, float dx, float dy, MatrixOrder order);

			// Token: 0x06000932 RID: 2354
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipScalePathGradientTransform(HandleRef brush, float sx, float sy, MatrixOrder order);

			// Token: 0x06000933 RID: 2355
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRotatePathGradientTransform(HandleRef brush, float angle, MatrixOrder order);

			// Token: 0x06000934 RID: 2356
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPathGradientFocusScales(HandleRef brush, float[] xScale, float[] yScale);

			// Token: 0x06000935 RID: 2357
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPathGradientFocusScales(HandleRef brush, float xScale, float yScale);

			// Token: 0x06000936 RID: 2358
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreatePen1(int argb, float width, int unit, out IntPtr pen);

			// Token: 0x06000937 RID: 2359
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreatePen2(HandleRef brush, float width, int unit, out IntPtr pen);

			// Token: 0x06000938 RID: 2360
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipClonePen(HandleRef pen, out IntPtr clonepen);

			// Token: 0x06000939 RID: 2361
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDeletePen", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDeletePen(HandleRef Pen);

			// Token: 0x0600093A RID: 2362 RVA: 0x0001F3B4 File Offset: 0x0001E3B4
			internal static int GdipDeletePen(HandleRef pen)
			{
				if (SafeNativeMethods.Gdip.IsShutdown)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDeletePen(pen);
			}

			// Token: 0x0600093B RID: 2363
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenMode(HandleRef pen, PenAlignment penAlign);

			// Token: 0x0600093C RID: 2364
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenMode(HandleRef pen, out PenAlignment penAlign);

			// Token: 0x0600093D RID: 2365
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenWidth(HandleRef pen, float width);

			// Token: 0x0600093E RID: 2366
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenWidth(HandleRef pen, float[] width);

			// Token: 0x0600093F RID: 2367
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenLineCap197819(HandleRef pen, int startCap, int endCap, int dashCap);

			// Token: 0x06000940 RID: 2368
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenStartCap(HandleRef pen, int startCap);

			// Token: 0x06000941 RID: 2369
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenEndCap(HandleRef pen, int endCap);

			// Token: 0x06000942 RID: 2370
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenStartCap(HandleRef pen, out int startCap);

			// Token: 0x06000943 RID: 2371
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenEndCap(HandleRef pen, out int endCap);

			// Token: 0x06000944 RID: 2372
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenDashCap197819(HandleRef pen, out int dashCap);

			// Token: 0x06000945 RID: 2373
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenDashCap197819(HandleRef pen, int dashCap);

			// Token: 0x06000946 RID: 2374
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenLineJoin(HandleRef pen, int lineJoin);

			// Token: 0x06000947 RID: 2375
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenLineJoin(HandleRef pen, out int lineJoin);

			// Token: 0x06000948 RID: 2376
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenCustomStartCap(HandleRef pen, HandleRef customCap);

			// Token: 0x06000949 RID: 2377
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenCustomStartCap(HandleRef pen, out IntPtr customCap);

			// Token: 0x0600094A RID: 2378
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenCustomEndCap(HandleRef pen, HandleRef customCap);

			// Token: 0x0600094B RID: 2379
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenCustomEndCap(HandleRef pen, out IntPtr customCap);

			// Token: 0x0600094C RID: 2380
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenMiterLimit(HandleRef pen, float miterLimit);

			// Token: 0x0600094D RID: 2381
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenMiterLimit(HandleRef pen, float[] miterLimit);

			// Token: 0x0600094E RID: 2382
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenTransform(HandleRef pen, HandleRef matrix);

			// Token: 0x0600094F RID: 2383
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenTransform(HandleRef pen, HandleRef matrix);

			// Token: 0x06000950 RID: 2384
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipResetPenTransform(HandleRef brush);

			// Token: 0x06000951 RID: 2385
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipMultiplyPenTransform(HandleRef brush, HandleRef matrix, MatrixOrder order);

			// Token: 0x06000952 RID: 2386
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTranslatePenTransform(HandleRef brush, float dx, float dy, MatrixOrder order);

			// Token: 0x06000953 RID: 2387
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipScalePenTransform(HandleRef brush, float sx, float sy, MatrixOrder order);

			// Token: 0x06000954 RID: 2388
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRotatePenTransform(HandleRef brush, float angle, MatrixOrder order);

			// Token: 0x06000955 RID: 2389
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenColor(HandleRef pen, int argb);

			// Token: 0x06000956 RID: 2390
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenColor(HandleRef pen, out int argb);

			// Token: 0x06000957 RID: 2391
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenBrushFill(HandleRef pen, HandleRef brush);

			// Token: 0x06000958 RID: 2392
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenBrushFill(HandleRef pen, out IntPtr brush);

			// Token: 0x06000959 RID: 2393
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenFillType(HandleRef pen, out int pentype);

			// Token: 0x0600095A RID: 2394
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenDashStyle(HandleRef pen, out int dashstyle);

			// Token: 0x0600095B RID: 2395
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenDashStyle(HandleRef pen, int dashstyle);

			// Token: 0x0600095C RID: 2396
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenDashArray(HandleRef pen, HandleRef memorydash, int count);

			// Token: 0x0600095D RID: 2397
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenDashOffset(HandleRef pen, float[] dashoffset);

			// Token: 0x0600095E RID: 2398
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenDashOffset(HandleRef pen, float dashoffset);

			// Token: 0x0600095F RID: 2399
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenDashCount(HandleRef pen, out int dashcount);

			// Token: 0x06000960 RID: 2400
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenDashArray(HandleRef pen, IntPtr memorydash, int count);

			// Token: 0x06000961 RID: 2401
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenCompoundCount(HandleRef pen, out int count);

			// Token: 0x06000962 RID: 2402
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPenCompoundArray(HandleRef pen, float[] array, int count);

			// Token: 0x06000963 RID: 2403
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPenCompoundArray(HandleRef pen, float[] array, int count);

			// Token: 0x06000964 RID: 2404
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateCustomLineCap(HandleRef fillpath, HandleRef strokepath, LineCap baseCap, float baseInset, out IntPtr customCap);

			// Token: 0x06000965 RID: 2405
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDeleteCustomLineCap", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDeleteCustomLineCap(HandleRef customCap);

			// Token: 0x06000966 RID: 2406 RVA: 0x0001F3D4 File Offset: 0x0001E3D4
			internal static int GdipDeleteCustomLineCap(HandleRef customCap)
			{
				if (SafeNativeMethods.Gdip.IsShutdown)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDeleteCustomLineCap(customCap);
			}

			// Token: 0x06000967 RID: 2407
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCloneCustomLineCap(HandleRef customCap, out IntPtr clonedCap);

			// Token: 0x06000968 RID: 2408
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetCustomLineCapType(HandleRef customCap, out CustomLineCapType capType);

			// Token: 0x06000969 RID: 2409
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetCustomLineCapStrokeCaps(HandleRef customCap, LineCap startCap, LineCap endCap);

			// Token: 0x0600096A RID: 2410
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetCustomLineCapStrokeCaps(HandleRef customCap, out LineCap startCap, out LineCap endCap);

			// Token: 0x0600096B RID: 2411
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetCustomLineCapStrokeJoin(HandleRef customCap, LineJoin lineJoin);

			// Token: 0x0600096C RID: 2412
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetCustomLineCapStrokeJoin(HandleRef customCap, out LineJoin lineJoin);

			// Token: 0x0600096D RID: 2413
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetCustomLineCapBaseCap(HandleRef customCap, LineCap baseCap);

			// Token: 0x0600096E RID: 2414
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetCustomLineCapBaseCap(HandleRef customCap, out LineCap baseCap);

			// Token: 0x0600096F RID: 2415
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetCustomLineCapBaseInset(HandleRef customCap, float inset);

			// Token: 0x06000970 RID: 2416
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetCustomLineCapBaseInset(HandleRef customCap, out float inset);

			// Token: 0x06000971 RID: 2417
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetCustomLineCapWidthScale(HandleRef customCap, float widthScale);

			// Token: 0x06000972 RID: 2418
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetCustomLineCapWidthScale(HandleRef customCap, out float widthScale);

			// Token: 0x06000973 RID: 2419
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateAdjustableArrowCap(float height, float width, bool isFilled, out IntPtr adjustableArrowCap);

			// Token: 0x06000974 RID: 2420
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetAdjustableArrowCapHeight(HandleRef adjustableArrowCap, float height);

			// Token: 0x06000975 RID: 2421
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetAdjustableArrowCapHeight(HandleRef adjustableArrowCap, out float height);

			// Token: 0x06000976 RID: 2422
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetAdjustableArrowCapWidth(HandleRef adjustableArrowCap, float width);

			// Token: 0x06000977 RID: 2423
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetAdjustableArrowCapWidth(HandleRef adjustableArrowCap, out float width);

			// Token: 0x06000978 RID: 2424
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetAdjustableArrowCapMiddleInset(HandleRef adjustableArrowCap, float middleInset);

			// Token: 0x06000979 RID: 2425
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetAdjustableArrowCapMiddleInset(HandleRef adjustableArrowCap, out float middleInset);

			// Token: 0x0600097A RID: 2426
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetAdjustableArrowCapFillState(HandleRef adjustableArrowCap, bool fillState);

			// Token: 0x0600097B RID: 2427
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetAdjustableArrowCapFillState(HandleRef adjustableArrowCap, out bool fillState);

			// Token: 0x0600097C RID: 2428
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipLoadImageFromStream(UnsafeNativeMethods.IStream stream, out IntPtr image);

			// Token: 0x0600097D RID: 2429
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipLoadImageFromFile(string filename, out IntPtr image);

			// Token: 0x0600097E RID: 2430
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipLoadImageFromStreamICM(UnsafeNativeMethods.IStream stream, out IntPtr image);

			// Token: 0x0600097F RID: 2431
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipLoadImageFromFileICM(string filename, out IntPtr image);

			// Token: 0x06000980 RID: 2432
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCloneImage(HandleRef image, out IntPtr cloneimage);

			// Token: 0x06000981 RID: 2433
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDisposeImage", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDisposeImage(HandleRef image);

			// Token: 0x06000982 RID: 2434 RVA: 0x0001F3F4 File Offset: 0x0001E3F4
			internal static int GdipDisposeImage(HandleRef image)
			{
				if (SafeNativeMethods.Gdip.IsShutdown)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDisposeImage(image);
			}

			// Token: 0x06000983 RID: 2435
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSaveImageToFile(HandleRef image, string filename, ref Guid classId, HandleRef encoderParams);

			// Token: 0x06000984 RID: 2436
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSaveImageToStream(HandleRef image, UnsafeNativeMethods.IStream stream, ref Guid classId, HandleRef encoderParams);

			// Token: 0x06000985 RID: 2437
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSaveAdd(HandleRef image, HandleRef encoderParams);

			// Token: 0x06000986 RID: 2438
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSaveAddImage(HandleRef image, HandleRef newImage, HandleRef encoderParams);

			// Token: 0x06000987 RID: 2439
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageGraphicsContext(HandleRef image, out IntPtr graphics);

			// Token: 0x06000988 RID: 2440
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageBounds(HandleRef image, ref GPRECTF gprectf, out GraphicsUnit unit);

			// Token: 0x06000989 RID: 2441
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageDimension(HandleRef image, out float width, out float height);

			// Token: 0x0600098A RID: 2442
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageType(HandleRef image, out int type);

			// Token: 0x0600098B RID: 2443
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageWidth(HandleRef image, out int width);

			// Token: 0x0600098C RID: 2444
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageHeight(HandleRef image, out int height);

			// Token: 0x0600098D RID: 2445
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageHorizontalResolution(HandleRef image, out float horzRes);

			// Token: 0x0600098E RID: 2446
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageVerticalResolution(HandleRef image, out float vertRes);

			// Token: 0x0600098F RID: 2447
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageFlags(HandleRef image, out int flags);

			// Token: 0x06000990 RID: 2448
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageRawFormat(HandleRef image, ref Guid format);

			// Token: 0x06000991 RID: 2449
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImagePixelFormat(HandleRef image, out int format);

			// Token: 0x06000992 RID: 2450
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageThumbnail(HandleRef image, int thumbWidth, int thumbHeight, out IntPtr thumbImage, Image.GetThumbnailImageAbort callback, IntPtr callbackdata);

			// Token: 0x06000993 RID: 2451
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetEncoderParameterListSize(HandleRef image, ref Guid clsid, out int size);

			// Token: 0x06000994 RID: 2452
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetEncoderParameterList(HandleRef image, ref Guid clsid, int size, IntPtr buffer);

			// Token: 0x06000995 RID: 2453
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipImageGetFrameDimensionsCount(HandleRef image, out int count);

			// Token: 0x06000996 RID: 2454
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipImageGetFrameDimensionsList(HandleRef image, IntPtr buffer, int count);

			// Token: 0x06000997 RID: 2455
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipImageGetFrameCount(HandleRef image, ref Guid dimensionID, int[] count);

			// Token: 0x06000998 RID: 2456
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipImageSelectActiveFrame(HandleRef image, ref Guid dimensionID, int frameIndex);

			// Token: 0x06000999 RID: 2457
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipImageRotateFlip(HandleRef image, int rotateFlipType);

			// Token: 0x0600099A RID: 2458
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImagePalette(HandleRef image, IntPtr palette, int size);

			// Token: 0x0600099B RID: 2459
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetImagePalette(HandleRef image, IntPtr palette);

			// Token: 0x0600099C RID: 2460
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImagePaletteSize(HandleRef image, out int size);

			// Token: 0x0600099D RID: 2461
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPropertyCount(HandleRef image, out int count);

			// Token: 0x0600099E RID: 2462
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPropertyIdList(HandleRef image, int count, int[] list);

			// Token: 0x0600099F RID: 2463
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPropertyItemSize(HandleRef image, int propid, out int size);

			// Token: 0x060009A0 RID: 2464
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPropertyItem(HandleRef image, int propid, int size, IntPtr buffer);

			// Token: 0x060009A1 RID: 2465
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPropertySize(HandleRef image, out int totalSize, ref int count);

			// Token: 0x060009A2 RID: 2466
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetAllPropertyItems(HandleRef image, int totalSize, int count, IntPtr buffer);

			// Token: 0x060009A3 RID: 2467
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRemovePropertyItem(HandleRef image, int propid);

			// Token: 0x060009A4 RID: 2468
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPropertyItem(HandleRef image, PropertyItemInternal propitem);

			// Token: 0x060009A5 RID: 2469
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipImageForceValidation(HandleRef image);

			// Token: 0x060009A6 RID: 2470
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageDecodersSize(out int numDecoders, out int size);

			// Token: 0x060009A7 RID: 2471
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageDecoders(int numDecoders, int size, IntPtr decoders);

			// Token: 0x060009A8 RID: 2472
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageEncodersSize(out int numEncoders, out int size);

			// Token: 0x060009A9 RID: 2473
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageEncoders(int numEncoders, int size, IntPtr encoders);

			// Token: 0x060009AA RID: 2474
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateBitmapFromStream(UnsafeNativeMethods.IStream stream, out IntPtr bitmap);

			// Token: 0x060009AB RID: 2475
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateBitmapFromFile(string filename, out IntPtr bitmap);

			// Token: 0x060009AC RID: 2476
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateBitmapFromStreamICM(UnsafeNativeMethods.IStream stream, out IntPtr bitmap);

			// Token: 0x060009AD RID: 2477
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateBitmapFromFileICM(string filename, out IntPtr bitmap);

			// Token: 0x060009AE RID: 2478
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateBitmapFromScan0(int width, int height, int stride, int format, HandleRef scan0, out IntPtr bitmap);

			// Token: 0x060009AF RID: 2479
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateBitmapFromGraphics(int width, int height, HandleRef graphics, out IntPtr bitmap);

			// Token: 0x060009B0 RID: 2480
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateBitmapFromHBITMAP(HandleRef hbitmap, HandleRef hpalette, out IntPtr bitmap);

			// Token: 0x060009B1 RID: 2481
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateBitmapFromHICON(HandleRef hicon, out IntPtr bitmap);

			// Token: 0x060009B2 RID: 2482
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateBitmapFromResource(HandleRef hresource, HandleRef name, out IntPtr bitmap);

			// Token: 0x060009B3 RID: 2483
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateHBITMAPFromBitmap(HandleRef nativeBitmap, out IntPtr hbitmap, int argbBackground);

			// Token: 0x060009B4 RID: 2484
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateHICONFromBitmap(HandleRef nativeBitmap, out IntPtr hicon);

			// Token: 0x060009B5 RID: 2485
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCloneBitmapArea(float x, float y, float width, float height, int format, HandleRef srcbitmap, out IntPtr dstbitmap);

			// Token: 0x060009B6 RID: 2486
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCloneBitmapAreaI(int x, int y, int width, int height, int format, HandleRef srcbitmap, out IntPtr dstbitmap);

			// Token: 0x060009B7 RID: 2487
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipBitmapLockBits(HandleRef bitmap, ref GPRECT rect, ImageLockMode flags, PixelFormat format, [In] [Out] BitmapData lockedBitmapData);

			// Token: 0x060009B8 RID: 2488
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipBitmapUnlockBits(HandleRef bitmap, BitmapData lockedBitmapData);

			// Token: 0x060009B9 RID: 2489
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipBitmapGetPixel(HandleRef bitmap, int x, int y, out int argb);

			// Token: 0x060009BA RID: 2490
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipBitmapSetPixel(HandleRef bitmap, int x, int y, int argb);

			// Token: 0x060009BB RID: 2491
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipBitmapSetResolution(HandleRef bitmap, float dpix, float dpiy);

			// Token: 0x060009BC RID: 2492
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateImageAttributes(out IntPtr imageattr);

			// Token: 0x060009BD RID: 2493
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCloneImageAttributes(HandleRef imageattr, out IntPtr cloneImageattr);

			// Token: 0x060009BE RID: 2494
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDisposeImageAttributes", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDisposeImageAttributes(HandleRef imageattr);

			// Token: 0x060009BF RID: 2495 RVA: 0x0001F414 File Offset: 0x0001E414
			internal static int GdipDisposeImageAttributes(HandleRef imageattr)
			{
				if (SafeNativeMethods.Gdip.IsShutdown)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDisposeImageAttributes(imageattr);
			}

			// Token: 0x060009C0 RID: 2496
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetImageAttributesColorMatrix(HandleRef imageattr, ColorAdjustType type, bool enableFlag, ColorMatrix colorMatrix, ColorMatrix grayMatrix, ColorMatrixFlag flags);

			// Token: 0x060009C1 RID: 2497
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetImageAttributesThreshold(HandleRef imageattr, ColorAdjustType type, bool enableFlag, float threshold);

			// Token: 0x060009C2 RID: 2498
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetImageAttributesGamma(HandleRef imageattr, ColorAdjustType type, bool enableFlag, float gamma);

			// Token: 0x060009C3 RID: 2499
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetImageAttributesNoOp(HandleRef imageattr, ColorAdjustType type, bool enableFlag);

			// Token: 0x060009C4 RID: 2500
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetImageAttributesColorKeys(HandleRef imageattr, ColorAdjustType type, bool enableFlag, int colorLow, int colorHigh);

			// Token: 0x060009C5 RID: 2501
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetImageAttributesOutputChannel(HandleRef imageattr, ColorAdjustType type, bool enableFlag, ColorChannelFlag flags);

			// Token: 0x060009C6 RID: 2502
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetImageAttributesOutputChannelColorProfile(HandleRef imageattr, ColorAdjustType type, bool enableFlag, string colorProfileFilename);

			// Token: 0x060009C7 RID: 2503
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetImageAttributesRemapTable(HandleRef imageattr, ColorAdjustType type, bool enableFlag, int mapSize, HandleRef map);

			// Token: 0x060009C8 RID: 2504
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetImageAttributesWrapMode(HandleRef imageattr, int wrapmode, int argb, bool clamp);

			// Token: 0x060009C9 RID: 2505
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetImageAttributesAdjustedPalette(HandleRef imageattr, HandleRef palette, ColorAdjustType type);

			// Token: 0x060009CA RID: 2506
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFlush(HandleRef graphics, FlushIntention intention);

			// Token: 0x060009CB RID: 2507
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateFromHDC(HandleRef hdc, out IntPtr graphics);

			// Token: 0x060009CC RID: 2508
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateFromHDC2(HandleRef hdc, HandleRef hdevice, out IntPtr graphics);

			// Token: 0x060009CD RID: 2509
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateFromHWND(HandleRef hwnd, out IntPtr graphics);

			// Token: 0x060009CE RID: 2510
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDeleteGraphics", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDeleteGraphics(HandleRef graphics);

			// Token: 0x060009CF RID: 2511 RVA: 0x0001F434 File Offset: 0x0001E434
			internal static int GdipDeleteGraphics(HandleRef graphics)
			{
				if (SafeNativeMethods.Gdip.IsShutdown)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDeleteGraphics(graphics);
			}

			// Token: 0x060009D0 RID: 2512
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetDC(HandleRef graphics, out IntPtr hdc);

			// Token: 0x060009D1 RID: 2513
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipReleaseDC", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipReleaseDC(HandleRef graphics, HandleRef hdc);

			// Token: 0x060009D2 RID: 2514 RVA: 0x0001F454 File Offset: 0x0001E454
			internal static int GdipReleaseDC(HandleRef graphics, HandleRef hdc)
			{
				if (SafeNativeMethods.Gdip.IsShutdown)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipReleaseDC(graphics, hdc);
			}

			// Token: 0x060009D3 RID: 2515
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetCompositingMode(HandleRef graphics, int compositeMode);

			// Token: 0x060009D4 RID: 2516
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetTextRenderingHint(HandleRef graphics, TextRenderingHint textRenderingHint);

			// Token: 0x060009D5 RID: 2517
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetTextContrast(HandleRef graphics, int textContrast);

			// Token: 0x060009D6 RID: 2518
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetInterpolationMode(HandleRef graphics, int mode);

			// Token: 0x060009D7 RID: 2519
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetCompositingMode(HandleRef graphics, out int compositeMode);

			// Token: 0x060009D8 RID: 2520
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetRenderingOrigin(HandleRef graphics, int x, int y);

			// Token: 0x060009D9 RID: 2521
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetRenderingOrigin(HandleRef graphics, out int x, out int y);

			// Token: 0x060009DA RID: 2522
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetCompositingQuality(HandleRef graphics, CompositingQuality quality);

			// Token: 0x060009DB RID: 2523
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetCompositingQuality(HandleRef graphics, out CompositingQuality quality);

			// Token: 0x060009DC RID: 2524
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetSmoothingMode(HandleRef graphics, SmoothingMode smoothingMode);

			// Token: 0x060009DD RID: 2525
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetSmoothingMode(HandleRef graphics, out SmoothingMode smoothingMode);

			// Token: 0x060009DE RID: 2526
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPixelOffsetMode(HandleRef graphics, PixelOffsetMode pixelOffsetMode);

			// Token: 0x060009DF RID: 2527
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPixelOffsetMode(HandleRef graphics, out PixelOffsetMode pixelOffsetMode);

			// Token: 0x060009E0 RID: 2528
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetTextRenderingHint(HandleRef graphics, out TextRenderingHint textRenderingHint);

			// Token: 0x060009E1 RID: 2529
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetTextContrast(HandleRef graphics, out int textContrast);

			// Token: 0x060009E2 RID: 2530
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetInterpolationMode(HandleRef graphics, out int mode);

			// Token: 0x060009E3 RID: 2531
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetWorldTransform(HandleRef graphics, HandleRef matrix);

			// Token: 0x060009E4 RID: 2532
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipResetWorldTransform(HandleRef graphics);

			// Token: 0x060009E5 RID: 2533
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipMultiplyWorldTransform(HandleRef graphics, HandleRef matrix, MatrixOrder order);

			// Token: 0x060009E6 RID: 2534
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTranslateWorldTransform(HandleRef graphics, float dx, float dy, MatrixOrder order);

			// Token: 0x060009E7 RID: 2535
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipScaleWorldTransform(HandleRef graphics, float sx, float sy, MatrixOrder order);

			// Token: 0x060009E8 RID: 2536
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRotateWorldTransform(HandleRef graphics, float angle, MatrixOrder order);

			// Token: 0x060009E9 RID: 2537
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetWorldTransform(HandleRef graphics, HandleRef matrix);

			// Token: 0x060009EA RID: 2538
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPageUnit(HandleRef graphics, out int unit);

			// Token: 0x060009EB RID: 2539
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetPageScale(HandleRef graphics, float[] scale);

			// Token: 0x060009EC RID: 2540
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPageUnit(HandleRef graphics, int unit);

			// Token: 0x060009ED RID: 2541
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetPageScale(HandleRef graphics, float scale);

			// Token: 0x060009EE RID: 2542
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetDpiX(HandleRef graphics, float[] dpi);

			// Token: 0x060009EF RID: 2543
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetDpiY(HandleRef graphics, float[] dpi);

			// Token: 0x060009F0 RID: 2544
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTransformPoints(HandleRef graphics, int destSpace, int srcSpace, IntPtr points, int count);

			// Token: 0x060009F1 RID: 2545
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTransformPointsI(HandleRef graphics, int destSpace, int srcSpace, IntPtr points, int count);

			// Token: 0x060009F2 RID: 2546
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetNearestColor(HandleRef graphics, ref int color);

			// Token: 0x060009F3 RID: 2547
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern IntPtr GdipCreateHalftonePalette();

			// Token: 0x060009F4 RID: 2548
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawLine(HandleRef graphics, HandleRef pen, float x1, float y1, float x2, float y2);

			// Token: 0x060009F5 RID: 2549
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawLineI(HandleRef graphics, HandleRef pen, int x1, int y1, int x2, int y2);

			// Token: 0x060009F6 RID: 2550
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawLines(HandleRef graphics, HandleRef pen, HandleRef points, int count);

			// Token: 0x060009F7 RID: 2551
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawLinesI(HandleRef graphics, HandleRef pen, HandleRef points, int count);

			// Token: 0x060009F8 RID: 2552
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawArc(HandleRef graphics, HandleRef pen, float x, float y, float width, float height, float startAngle, float sweepAngle);

			// Token: 0x060009F9 RID: 2553
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawArcI(HandleRef graphics, HandleRef pen, int x, int y, int width, int height, float startAngle, float sweepAngle);

			// Token: 0x060009FA RID: 2554
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawBezier(HandleRef graphics, HandleRef pen, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4);

			// Token: 0x060009FB RID: 2555
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawBeziers(HandleRef graphics, HandleRef pen, HandleRef points, int count);

			// Token: 0x060009FC RID: 2556
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawBeziersI(HandleRef graphics, HandleRef pen, HandleRef points, int count);

			// Token: 0x060009FD RID: 2557
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawRectangle(HandleRef graphics, HandleRef pen, float x, float y, float width, float height);

			// Token: 0x060009FE RID: 2558
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawRectangleI(HandleRef graphics, HandleRef pen, int x, int y, int width, int height);

			// Token: 0x060009FF RID: 2559
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawRectangles(HandleRef graphics, HandleRef pen, HandleRef rects, int count);

			// Token: 0x06000A00 RID: 2560
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawRectanglesI(HandleRef graphics, HandleRef pen, HandleRef rects, int count);

			// Token: 0x06000A01 RID: 2561
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawEllipse(HandleRef graphics, HandleRef pen, float x, float y, float width, float height);

			// Token: 0x06000A02 RID: 2562
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawEllipseI(HandleRef graphics, HandleRef pen, int x, int y, int width, int height);

			// Token: 0x06000A03 RID: 2563
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawPie(HandleRef graphics, HandleRef pen, float x, float y, float width, float height, float startAngle, float sweepAngle);

			// Token: 0x06000A04 RID: 2564
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawPieI(HandleRef graphics, HandleRef pen, int x, int y, int width, int height, float startAngle, float sweepAngle);

			// Token: 0x06000A05 RID: 2565
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawPolygon(HandleRef graphics, HandleRef pen, HandleRef points, int count);

			// Token: 0x06000A06 RID: 2566
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawPolygonI(HandleRef graphics, HandleRef pen, HandleRef points, int count);

			// Token: 0x06000A07 RID: 2567
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawPath(HandleRef graphics, HandleRef pen, HandleRef path);

			// Token: 0x06000A08 RID: 2568
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawCurve(HandleRef graphics, HandleRef pen, HandleRef points, int count);

			// Token: 0x06000A09 RID: 2569
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawCurveI(HandleRef graphics, HandleRef pen, HandleRef points, int count);

			// Token: 0x06000A0A RID: 2570
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawCurve2(HandleRef graphics, HandleRef pen, HandleRef points, int count, float tension);

			// Token: 0x06000A0B RID: 2571
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawCurve2I(HandleRef graphics, HandleRef pen, HandleRef points, int count, float tension);

			// Token: 0x06000A0C RID: 2572
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawCurve3(HandleRef graphics, HandleRef pen, HandleRef points, int count, int offset, int numberOfSegments, float tension);

			// Token: 0x06000A0D RID: 2573
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawCurve3I(HandleRef graphics, HandleRef pen, HandleRef points, int count, int offset, int numberOfSegments, float tension);

			// Token: 0x06000A0E RID: 2574
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawClosedCurve(HandleRef graphics, HandleRef pen, HandleRef points, int count);

			// Token: 0x06000A0F RID: 2575
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawClosedCurveI(HandleRef graphics, HandleRef pen, HandleRef points, int count);

			// Token: 0x06000A10 RID: 2576
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawClosedCurve2(HandleRef graphics, HandleRef pen, HandleRef points, int count, float tension);

			// Token: 0x06000A11 RID: 2577
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawClosedCurve2I(HandleRef graphics, HandleRef pen, HandleRef points, int count, float tension);

			// Token: 0x06000A12 RID: 2578
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGraphicsClear(HandleRef graphics, int argb);

			// Token: 0x06000A13 RID: 2579
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillRectangle(HandleRef graphics, HandleRef brush, float x, float y, float width, float height);

			// Token: 0x06000A14 RID: 2580
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillRectangleI(HandleRef graphics, HandleRef brush, int x, int y, int width, int height);

			// Token: 0x06000A15 RID: 2581
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillRectangles(HandleRef graphics, HandleRef brush, HandleRef rects, int count);

			// Token: 0x06000A16 RID: 2582
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillRectanglesI(HandleRef graphics, HandleRef brush, HandleRef rects, int count);

			// Token: 0x06000A17 RID: 2583
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillPolygon(HandleRef graphics, HandleRef brush, HandleRef points, int count, int brushMode);

			// Token: 0x06000A18 RID: 2584
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillPolygonI(HandleRef graphics, HandleRef brush, HandleRef points, int count, int brushMode);

			// Token: 0x06000A19 RID: 2585
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillEllipse(HandleRef graphics, HandleRef brush, float x, float y, float width, float height);

			// Token: 0x06000A1A RID: 2586
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillEllipseI(HandleRef graphics, HandleRef brush, int x, int y, int width, int height);

			// Token: 0x06000A1B RID: 2587
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillPie(HandleRef graphics, HandleRef brush, float x, float y, float width, float height, float startAngle, float sweepAngle);

			// Token: 0x06000A1C RID: 2588
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillPieI(HandleRef graphics, HandleRef brush, int x, int y, int width, int height, float startAngle, float sweepAngle);

			// Token: 0x06000A1D RID: 2589
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillPath(HandleRef graphics, HandleRef brush, HandleRef path);

			// Token: 0x06000A1E RID: 2590
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillClosedCurve(HandleRef graphics, HandleRef brush, HandleRef points, int count);

			// Token: 0x06000A1F RID: 2591
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillClosedCurveI(HandleRef graphics, HandleRef brush, HandleRef points, int count);

			// Token: 0x06000A20 RID: 2592
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillClosedCurve2(HandleRef graphics, HandleRef brush, HandleRef points, int count, float tension, int mode);

			// Token: 0x06000A21 RID: 2593
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillClosedCurve2I(HandleRef graphics, HandleRef brush, HandleRef points, int count, float tension, int mode);

			// Token: 0x06000A22 RID: 2594
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipFillRegion(HandleRef graphics, HandleRef brush, HandleRef region);

			// Token: 0x06000A23 RID: 2595
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawImage(HandleRef graphics, HandleRef image, float x, float y);

			// Token: 0x06000A24 RID: 2596
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawImageI(HandleRef graphics, HandleRef image, int x, int y);

			// Token: 0x06000A25 RID: 2597
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawImageRect(HandleRef graphics, HandleRef image, float x, float y, float width, float height);

			// Token: 0x06000A26 RID: 2598
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawImageRectI(HandleRef graphics, HandleRef image, int x, int y, int width, int height);

			// Token: 0x06000A27 RID: 2599
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawImagePoints(HandleRef graphics, HandleRef image, HandleRef points, int count);

			// Token: 0x06000A28 RID: 2600
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawImagePointsI(HandleRef graphics, HandleRef image, HandleRef points, int count);

			// Token: 0x06000A29 RID: 2601
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawImagePointRect(HandleRef graphics, HandleRef image, float x, float y, float srcx, float srcy, float srcwidth, float srcheight, int srcunit);

			// Token: 0x06000A2A RID: 2602
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawImagePointRectI(HandleRef graphics, HandleRef image, int x, int y, int srcx, int srcy, int srcwidth, int srcheight, int srcunit);

			// Token: 0x06000A2B RID: 2603
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawImageRectRect(HandleRef graphics, HandleRef image, float dstx, float dsty, float dstwidth, float dstheight, float srcx, float srcy, float srcwidth, float srcheight, int srcunit, HandleRef imageAttributes, Graphics.DrawImageAbort callback, HandleRef callbackdata);

			// Token: 0x06000A2C RID: 2604
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawImageRectRectI(HandleRef graphics, HandleRef image, int dstx, int dsty, int dstwidth, int dstheight, int srcx, int srcy, int srcwidth, int srcheight, int srcunit, HandleRef imageAttributes, Graphics.DrawImageAbort callback, HandleRef callbackdata);

			// Token: 0x06000A2D RID: 2605
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawImagePointsRect(HandleRef graphics, HandleRef image, HandleRef points, int count, float srcx, float srcy, float srcwidth, float srcheight, int srcunit, HandleRef imageAttributes, Graphics.DrawImageAbort callback, HandleRef callbackdata);

			// Token: 0x06000A2E RID: 2606
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawImagePointsRectI(HandleRef graphics, HandleRef image, HandleRef points, int count, int srcx, int srcy, int srcwidth, int srcheight, int srcunit, HandleRef imageAttributes, Graphics.DrawImageAbort callback, HandleRef callbackdata);

			// Token: 0x06000A2F RID: 2607
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEnumerateMetafileDestPoint(HandleRef graphics, HandleRef metafile, GPPOINTF destPoint, Graphics.EnumerateMetafileProc callback, HandleRef callbackdata, HandleRef imageattributes);

			// Token: 0x06000A30 RID: 2608
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEnumerateMetafileDestPointI(HandleRef graphics, HandleRef metafile, GPPOINT destPoint, Graphics.EnumerateMetafileProc callback, HandleRef callbackdata, HandleRef imageattributes);

			// Token: 0x06000A31 RID: 2609
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEnumerateMetafileDestRect(HandleRef graphics, HandleRef metafile, ref GPRECTF destRect, Graphics.EnumerateMetafileProc callback, HandleRef callbackdata, HandleRef imageattributes);

			// Token: 0x06000A32 RID: 2610
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEnumerateMetafileDestRectI(HandleRef graphics, HandleRef metafile, ref GPRECT destRect, Graphics.EnumerateMetafileProc callback, HandleRef callbackdata, HandleRef imageattributes);

			// Token: 0x06000A33 RID: 2611
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEnumerateMetafileDestPoints(HandleRef graphics, HandleRef metafile, IntPtr destPoints, int count, Graphics.EnumerateMetafileProc callback, HandleRef callbackdata, HandleRef imageattributes);

			// Token: 0x06000A34 RID: 2612
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEnumerateMetafileDestPointsI(HandleRef graphics, HandleRef metafile, IntPtr destPoints, int count, Graphics.EnumerateMetafileProc callback, HandleRef callbackdata, HandleRef imageattributes);

			// Token: 0x06000A35 RID: 2613
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEnumerateMetafileSrcRectDestPoint(HandleRef graphics, HandleRef metafile, GPPOINTF destPoint, ref GPRECTF srcRect, int pageUnit, Graphics.EnumerateMetafileProc callback, HandleRef callbackdata, HandleRef imageattributes);

			// Token: 0x06000A36 RID: 2614
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEnumerateMetafileSrcRectDestPointI(HandleRef graphics, HandleRef metafile, GPPOINT destPoint, ref GPRECT srcRect, int pageUnit, Graphics.EnumerateMetafileProc callback, HandleRef callbackdata, HandleRef imageattributes);

			// Token: 0x06000A37 RID: 2615
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEnumerateMetafileSrcRectDestRect(HandleRef graphics, HandleRef metafile, ref GPRECTF destRect, ref GPRECTF srcRect, int pageUnit, Graphics.EnumerateMetafileProc callback, HandleRef callbackdata, HandleRef imageattributes);

			// Token: 0x06000A38 RID: 2616
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEnumerateMetafileSrcRectDestRectI(HandleRef graphics, HandleRef metafile, ref GPRECT destRect, ref GPRECT srcRect, int pageUnit, Graphics.EnumerateMetafileProc callback, HandleRef callbackdata, HandleRef imageattributes);

			// Token: 0x06000A39 RID: 2617
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEnumerateMetafileSrcRectDestPoints(HandleRef graphics, HandleRef metafile, IntPtr destPoints, int count, ref GPRECTF srcRect, int pageUnit, Graphics.EnumerateMetafileProc callback, HandleRef callbackdata, HandleRef imageattributes);

			// Token: 0x06000A3A RID: 2618
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEnumerateMetafileSrcRectDestPointsI(HandleRef graphics, HandleRef metafile, IntPtr destPoints, int count, ref GPRECT srcRect, int pageUnit, Graphics.EnumerateMetafileProc callback, HandleRef callbackdata, HandleRef imageattributes);

			// Token: 0x06000A3B RID: 2619
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPlayMetafileRecord(HandleRef graphics, EmfPlusRecordType recordType, int flags, int dataSize, byte[] data);

			// Token: 0x06000A3C RID: 2620
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetClipGraphics(HandleRef graphics, HandleRef srcgraphics, CombineMode mode);

			// Token: 0x06000A3D RID: 2621
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetClipRect(HandleRef graphics, float x, float y, float width, float height, CombineMode mode);

			// Token: 0x06000A3E RID: 2622
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetClipRectI(HandleRef graphics, int x, int y, int width, int height, CombineMode mode);

			// Token: 0x06000A3F RID: 2623
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetClipPath(HandleRef graphics, HandleRef path, CombineMode mode);

			// Token: 0x06000A40 RID: 2624
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetClipRegion(HandleRef graphics, HandleRef region, CombineMode mode);

			// Token: 0x06000A41 RID: 2625
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipResetClip(HandleRef graphics);

			// Token: 0x06000A42 RID: 2626
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipTranslateClip(HandleRef graphics, float dx, float dy);

			// Token: 0x06000A43 RID: 2627
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetClip(HandleRef graphics, HandleRef region);

			// Token: 0x06000A44 RID: 2628
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetClipBounds(HandleRef graphics, ref GPRECTF rect);

			// Token: 0x06000A45 RID: 2629
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsClipEmpty(HandleRef graphics, out int boolean);

			// Token: 0x06000A46 RID: 2630
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetVisibleClipBounds(HandleRef graphics, ref GPRECTF rect);

			// Token: 0x06000A47 RID: 2631
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsVisibleClipEmpty(HandleRef graphics, out int boolean);

			// Token: 0x06000A48 RID: 2632
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsVisiblePoint(HandleRef graphics, float x, float y, out int boolean);

			// Token: 0x06000A49 RID: 2633
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsVisiblePointI(HandleRef graphics, int x, int y, out int boolean);

			// Token: 0x06000A4A RID: 2634
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsVisibleRect(HandleRef graphics, float x, float y, float width, float height, out int boolean);

			// Token: 0x06000A4B RID: 2635
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsVisibleRectI(HandleRef graphics, int x, int y, int width, int height, out int boolean);

			// Token: 0x06000A4C RID: 2636
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSaveGraphics(HandleRef graphics, out int state);

			// Token: 0x06000A4D RID: 2637
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRestoreGraphics(HandleRef graphics, int state);

			// Token: 0x06000A4E RID: 2638
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipBeginContainer(HandleRef graphics, ref GPRECTF dstRect, ref GPRECTF srcRect, int unit, out int state);

			// Token: 0x06000A4F RID: 2639
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipBeginContainer2(HandleRef graphics, out int state);

			// Token: 0x06000A50 RID: 2640
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipBeginContainerI(HandleRef graphics, ref GPRECT dstRect, ref GPRECT srcRect, int unit, out int state);

			// Token: 0x06000A51 RID: 2641
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipEndContainer(HandleRef graphics, int state);

			// Token: 0x06000A52 RID: 2642
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetMetafileHeaderFromWmf(HandleRef hMetafile, WmfPlaceableFileHeader wmfplaceable, [In] [Out] MetafileHeaderWmf metafileHeaderWmf);

			// Token: 0x06000A53 RID: 2643
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetMetafileHeaderFromEmf(HandleRef hEnhMetafile, [In] [Out] MetafileHeaderEmf metafileHeaderEmf);

			// Token: 0x06000A54 RID: 2644
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetMetafileHeaderFromFile(string filename, IntPtr header);

			// Token: 0x06000A55 RID: 2645
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetMetafileHeaderFromStream(UnsafeNativeMethods.IStream stream, IntPtr header);

			// Token: 0x06000A56 RID: 2646
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetMetafileHeaderFromMetafile(HandleRef metafile, IntPtr header);

			// Token: 0x06000A57 RID: 2647
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetHemfFromMetafile(HandleRef metafile, out IntPtr hEnhMetafile);

			// Token: 0x06000A58 RID: 2648
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateMetafileFromWmf(HandleRef hMetafile, WmfPlaceableFileHeader wmfplacealbeHeader, bool deleteWmf, out IntPtr metafile);

			// Token: 0x06000A59 RID: 2649
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateMetafileFromEmf(HandleRef hEnhMetafile, bool deleteEmf, out IntPtr metafile);

			// Token: 0x06000A5A RID: 2650
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateMetafileFromFile(string file, out IntPtr metafile);

			// Token: 0x06000A5B RID: 2651
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateMetafileFromStream(UnsafeNativeMethods.IStream stream, out IntPtr metafile);

			// Token: 0x06000A5C RID: 2652
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRecordMetafile(HandleRef referenceHdc, int emfType, ref GPRECTF frameRect, int frameUnit, string description, out IntPtr metafile);

			// Token: 0x06000A5D RID: 2653
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRecordMetafile(HandleRef referenceHdc, int emfType, HandleRef pframeRect, int frameUnit, string description, out IntPtr metafile);

			// Token: 0x06000A5E RID: 2654
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRecordMetafileI(HandleRef referenceHdc, int emfType, ref GPRECT frameRect, int frameUnit, string description, out IntPtr metafile);

			// Token: 0x06000A5F RID: 2655
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRecordMetafileFileName(string fileName, HandleRef referenceHdc, int emfType, ref GPRECTF frameRect, int frameUnit, string description, out IntPtr metafile);

			// Token: 0x06000A60 RID: 2656
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRecordMetafileFileName(string fileName, HandleRef referenceHdc, int emfType, HandleRef pframeRect, int frameUnit, string description, out IntPtr metafile);

			// Token: 0x06000A61 RID: 2657
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRecordMetafileFileNameI(string fileName, HandleRef referenceHdc, int emfType, ref GPRECT frameRect, int frameUnit, string description, out IntPtr metafile);

			// Token: 0x06000A62 RID: 2658
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRecordMetafileStream(UnsafeNativeMethods.IStream stream, HandleRef referenceHdc, int emfType, ref GPRECTF frameRect, int frameUnit, string description, out IntPtr metafile);

			// Token: 0x06000A63 RID: 2659
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRecordMetafileStream(UnsafeNativeMethods.IStream stream, HandleRef referenceHdc, int emfType, HandleRef pframeRect, int frameUnit, string description, out IntPtr metafile);

			// Token: 0x06000A64 RID: 2660
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipRecordMetafileStreamI(UnsafeNativeMethods.IStream stream, HandleRef referenceHdc, int emfType, ref GPRECT frameRect, int frameUnit, string description, out IntPtr metafile);

			// Token: 0x06000A65 RID: 2661
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipComment(HandleRef graphics, int sizeData, byte[] data);

			// Token: 0x06000A66 RID: 2662
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipNewInstalledFontCollection(out IntPtr fontCollection);

			// Token: 0x06000A67 RID: 2663
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipNewPrivateFontCollection(out IntPtr fontCollection);

			// Token: 0x06000A68 RID: 2664
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDeletePrivateFontCollection", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDeletePrivateFontCollection(out IntPtr fontCollection);

			// Token: 0x06000A69 RID: 2665 RVA: 0x0001F474 File Offset: 0x0001E474
			internal static int GdipDeletePrivateFontCollection(out IntPtr fontCollection)
			{
				if (SafeNativeMethods.Gdip.IsShutdown)
				{
					fontCollection = IntPtr.Zero;
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDeletePrivateFontCollection(out fontCollection);
			}

			// Token: 0x06000A6A RID: 2666
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetFontCollectionFamilyCount(HandleRef fontCollection, out int numFound);

			// Token: 0x06000A6B RID: 2667
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetFontCollectionFamilyList(HandleRef fontCollection, int numSought, IntPtr[] gpfamilies, out int numFound);

			// Token: 0x06000A6C RID: 2668
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPrivateAddFontFile(HandleRef fontCollection, string filename);

			// Token: 0x06000A6D RID: 2669
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipPrivateAddMemoryFont(HandleRef fontCollection, HandleRef memory, int length);

			// Token: 0x06000A6E RID: 2670
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateFontFamilyFromName(string name, HandleRef fontCollection, out IntPtr FontFamily);

			// Token: 0x06000A6F RID: 2671
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetGenericFontFamilySansSerif(out IntPtr fontfamily);

			// Token: 0x06000A70 RID: 2672
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetGenericFontFamilySerif(out IntPtr fontfamily);

			// Token: 0x06000A71 RID: 2673
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetGenericFontFamilyMonospace(out IntPtr fontfamily);

			// Token: 0x06000A72 RID: 2674
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDeleteFontFamily", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDeleteFontFamily(HandleRef fontFamily);

			// Token: 0x06000A73 RID: 2675 RVA: 0x0001F4A0 File Offset: 0x0001E4A0
			internal static int GdipDeleteFontFamily(HandleRef fontFamily)
			{
				if (SafeNativeMethods.Gdip.IsShutdown)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDeleteFontFamily(fontFamily);
			}

			// Token: 0x06000A74 RID: 2676
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCloneFontFamily(HandleRef fontfamily, out IntPtr clonefontfamily);

			// Token: 0x06000A75 RID: 2677
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetFamilyName(HandleRef family, StringBuilder name, int language);

			// Token: 0x06000A76 RID: 2678
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipIsStyleAvailable(HandleRef family, FontStyle style, out int isStyleAvailable);

			// Token: 0x06000A77 RID: 2679
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetEmHeight(HandleRef family, FontStyle style, out int EmHeight);

			// Token: 0x06000A78 RID: 2680
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetCellAscent(HandleRef family, FontStyle style, out int CellAscent);

			// Token: 0x06000A79 RID: 2681
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetCellDescent(HandleRef family, FontStyle style, out int CellDescent);

			// Token: 0x06000A7A RID: 2682
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetLineSpacing(HandleRef family, FontStyle style, out int LineSpaceing);

			// Token: 0x06000A7B RID: 2683
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateFontFromDC(HandleRef hdc, ref IntPtr font);

			// Token: 0x06000A7C RID: 2684
			[DllImport("gdiplus.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateFontFromLogfontA(HandleRef hdc, [MarshalAs(UnmanagedType.AsAny)] [In] [Out] object lf, out IntPtr font);

			// Token: 0x06000A7D RID: 2685
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateFontFromLogfontW(HandleRef hdc, [MarshalAs(UnmanagedType.AsAny)] [In] [Out] object lf, out IntPtr font);

			// Token: 0x06000A7E RID: 2686
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateFont(HandleRef fontFamily, float emSize, FontStyle style, GraphicsUnit unit, out IntPtr font);

			// Token: 0x06000A7F RID: 2687
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetLogFontW(HandleRef font, HandleRef graphics, [MarshalAs(UnmanagedType.AsAny)] [In] [Out] object lf);

			// Token: 0x06000A80 RID: 2688
			[DllImport("gdiplus.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetLogFontA(HandleRef font, HandleRef graphics, [MarshalAs(UnmanagedType.AsAny)] [In] [Out] object lf);

			// Token: 0x06000A81 RID: 2689
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCloneFont(HandleRef font, out IntPtr cloneFont);

			// Token: 0x06000A82 RID: 2690
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDeleteFont", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDeleteFont(HandleRef font);

			// Token: 0x06000A83 RID: 2691 RVA: 0x0001F4C0 File Offset: 0x0001E4C0
			internal static int GdipDeleteFont(HandleRef font)
			{
				if (SafeNativeMethods.Gdip.IsShutdown)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDeleteFont(font);
			}

			// Token: 0x06000A84 RID: 2692
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetFamily(HandleRef font, out IntPtr family);

			// Token: 0x06000A85 RID: 2693
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetFontStyle(HandleRef font, out FontStyle style);

			// Token: 0x06000A86 RID: 2694
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetFontSize(HandleRef font, out float size);

			// Token: 0x06000A87 RID: 2695
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetFontHeight(HandleRef font, HandleRef graphics, out float size);

			// Token: 0x06000A88 RID: 2696
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetFontHeightGivenDPI(HandleRef font, float dpi, out float size);

			// Token: 0x06000A89 RID: 2697
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetFontUnit(HandleRef font, out GraphicsUnit unit);

			// Token: 0x06000A8A RID: 2698
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipDrawString(HandleRef graphics, string textString, int length, HandleRef font, ref GPRECTF layoutRect, HandleRef stringFormat, HandleRef brush);

			// Token: 0x06000A8B RID: 2699
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipMeasureString(HandleRef graphics, string textString, int length, HandleRef font, ref GPRECTF layoutRect, HandleRef stringFormat, [In] [Out] ref GPRECTF boundingBox, out int codepointsFitted, out int linesFilled);

			// Token: 0x06000A8C RID: 2700
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipMeasureCharacterRanges(HandleRef graphics, string textString, int length, HandleRef font, ref GPRECTF layoutRect, HandleRef stringFormat, int characterCount, [In] [Out] IntPtr[] region);

			// Token: 0x06000A8D RID: 2701
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetStringFormatMeasurableCharacterRanges(HandleRef format, int rangeCount, [In] [Out] CharacterRange[] range);

			// Token: 0x06000A8E RID: 2702
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCreateStringFormat(StringFormatFlags options, int language, out IntPtr format);

			// Token: 0x06000A8F RID: 2703
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipStringFormatGetGenericDefault(out IntPtr format);

			// Token: 0x06000A90 RID: 2704
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipStringFormatGetGenericTypographic(out IntPtr format);

			// Token: 0x06000A91 RID: 2705
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, EntryPoint = "GdipDeleteStringFormat", ExactSpelling = true, SetLastError = true)]
			private static extern int IntGdipDeleteStringFormat(HandleRef format);

			// Token: 0x06000A92 RID: 2706 RVA: 0x0001F4E0 File Offset: 0x0001E4E0
			internal static int GdipDeleteStringFormat(HandleRef format)
			{
				if (SafeNativeMethods.Gdip.IsShutdown)
				{
					return 0;
				}
				return SafeNativeMethods.Gdip.IntGdipDeleteStringFormat(format);
			}

			// Token: 0x06000A93 RID: 2707
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipCloneStringFormat(HandleRef format, out IntPtr newFormat);

			// Token: 0x06000A94 RID: 2708
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetStringFormatFlags(HandleRef format, StringFormatFlags options);

			// Token: 0x06000A95 RID: 2709
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetStringFormatFlags(HandleRef format, out StringFormatFlags result);

			// Token: 0x06000A96 RID: 2710
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetStringFormatAlign(HandleRef format, StringAlignment align);

			// Token: 0x06000A97 RID: 2711
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetStringFormatAlign(HandleRef format, out StringAlignment align);

			// Token: 0x06000A98 RID: 2712
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetStringFormatLineAlign(HandleRef format, StringAlignment align);

			// Token: 0x06000A99 RID: 2713
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetStringFormatLineAlign(HandleRef format, out StringAlignment align);

			// Token: 0x06000A9A RID: 2714
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetStringFormatHotkeyPrefix(HandleRef format, HotkeyPrefix hotkeyPrefix);

			// Token: 0x06000A9B RID: 2715
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetStringFormatHotkeyPrefix(HandleRef format, out HotkeyPrefix hotkeyPrefix);

			// Token: 0x06000A9C RID: 2716
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetStringFormatTabStops(HandleRef format, float firstTabOffset, int count, float[] tabStops);

			// Token: 0x06000A9D RID: 2717
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetStringFormatTabStops(HandleRef format, int count, out float firstTabOffset, [In] [Out] float[] tabStops);

			// Token: 0x06000A9E RID: 2718
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetStringFormatTabStopCount(HandleRef format, out int count);

			// Token: 0x06000A9F RID: 2719
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetStringFormatMeasurableCharacterRangeCount(HandleRef format, out int count);

			// Token: 0x06000AA0 RID: 2720
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetStringFormatTrimming(HandleRef format, StringTrimming trimming);

			// Token: 0x06000AA1 RID: 2721
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetStringFormatTrimming(HandleRef format, out StringTrimming trimming);

			// Token: 0x06000AA2 RID: 2722
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipSetStringFormatDigitSubstitution(HandleRef format, int langID, StringDigitSubstitute sds);

			// Token: 0x06000AA3 RID: 2723
			[DllImport("gdiplus.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
			internal static extern int GdipGetStringFormatDigitSubstitution(HandleRef format, out int langID, out StringDigitSubstitute sds);

			// Token: 0x06000AA4 RID: 2724 RVA: 0x0001F500 File Offset: 0x0001E500
			internal static Exception StatusException(int status)
			{
				switch (status)
				{
				case 1:
					return new ExternalException(SR.GetString("GdiplusGenericError"), -2147467259);
				case 2:
					return new ArgumentException(SR.GetString("GdiplusInvalidParameter"));
				case 3:
					return new OutOfMemoryException(SR.GetString("GdiplusOutOfMemory"));
				case 4:
					return new InvalidOperationException(SR.GetString("GdiplusObjectBusy"));
				case 5:
					return new OutOfMemoryException(SR.GetString("GdiplusInsufficientBuffer"));
				case 6:
					return new NotImplementedException(SR.GetString("GdiplusNotImplemented"));
				case 7:
					return new ExternalException(SR.GetString("GdiplusGenericError"), -2147467259);
				case 8:
					return new InvalidOperationException(SR.GetString("GdiplusWrongState"));
				case 9:
					return new ExternalException(SR.GetString("GdiplusAborted"), -2147467260);
				case 10:
					return new FileNotFoundException(SR.GetString("GdiplusFileNotFound"));
				case 11:
					return new OverflowException(SR.GetString("GdiplusOverflow"));
				case 12:
					return new ExternalException(SR.GetString("GdiplusAccessDenied"), -2147024891);
				case 13:
					return new ArgumentException(SR.GetString("GdiplusUnknownImageFormat"));
				case 14:
					return new ArgumentException(SR.GetString("GdiplusFontFamilyNotFound", new object[] { "?" }));
				case 15:
					return new ArgumentException(SR.GetString("GdiplusFontStyleNotFound", new object[] { "?", "?" }));
				case 16:
					return new ArgumentException(SR.GetString("GdiplusNotTrueTypeFont_NoName"));
				case 17:
					return new ExternalException(SR.GetString("GdiplusUnsupportedGdiplusVersion"), -2147467259);
				case 18:
					return new ExternalException(SR.GetString("GdiplusNotInitialized"), -2147467259);
				case 19:
					return new ArgumentException(SR.GetString("GdiplusPropertyNotFoundError"));
				case 20:
					return new ArgumentException(SR.GetString("GdiplusPropertyNotSupportedError"));
				default:
					return new ExternalException(SR.GetString("GdiplusUnknown"), -2147418113);
				}
			}

			// Token: 0x06000AA5 RID: 2725 RVA: 0x0001F708 File Offset: 0x0001E708
			internal static PointF[] ConvertGPPOINTFArrayF(IntPtr memory, int count)
			{
				if (memory == IntPtr.Zero)
				{
					throw new ArgumentNullException("memory");
				}
				PointF[] array = new PointF[count];
				GPPOINTF gppointf = new GPPOINTF();
				int num = Marshal.SizeOf(gppointf.GetType());
				for (int i = 0; i < count; i++)
				{
					gppointf = (GPPOINTF)UnsafeNativeMethods.PtrToStructure((IntPtr)((long)memory + (long)(i * num)), gppointf.GetType());
					array[i] = new PointF(gppointf.X, gppointf.Y);
				}
				return array;
			}

			// Token: 0x06000AA6 RID: 2726 RVA: 0x0001F794 File Offset: 0x0001E794
			internal static Point[] ConvertGPPOINTArray(IntPtr memory, int count)
			{
				if (memory == IntPtr.Zero)
				{
					throw new ArgumentNullException("memory");
				}
				Point[] array = new Point[count];
				GPPOINT gppoint = new GPPOINT();
				int num = Marshal.SizeOf(gppoint.GetType());
				for (int i = 0; i < count; i++)
				{
					gppoint = (GPPOINT)UnsafeNativeMethods.PtrToStructure((IntPtr)((long)memory + (long)(i * num)), gppoint.GetType());
					array[i] = new Point(gppoint.X, gppoint.Y);
				}
				return array;
			}

			// Token: 0x06000AA7 RID: 2727 RVA: 0x0001F820 File Offset: 0x0001E820
			internal static IntPtr ConvertPointToMemory(PointF[] points)
			{
				if (points == null)
				{
					throw new ArgumentNullException("points");
				}
				int num = Marshal.SizeOf(typeof(GPPOINTF));
				int num2 = points.Length;
				IntPtr intPtr = Marshal.AllocHGlobal(checked(num2 * num));
				for (int i = 0; i < num2; i++)
				{
					checked
					{
						Marshal.StructureToPtr(new GPPOINTF(points[i]), (IntPtr)((long)intPtr + unchecked((long)(checked(i * num)))), false);
					}
				}
				return intPtr;
			}

			// Token: 0x06000AA8 RID: 2728 RVA: 0x0001F88C File Offset: 0x0001E88C
			internal static IntPtr ConvertPointToMemory(Point[] points)
			{
				if (points == null)
				{
					throw new ArgumentNullException("points");
				}
				int num = Marshal.SizeOf(typeof(GPPOINT));
				int num2 = points.Length;
				IntPtr intPtr = Marshal.AllocHGlobal(checked(num2 * num));
				for (int i = 0; i < num2; i++)
				{
					checked
					{
						Marshal.StructureToPtr(new GPPOINT(points[i]), (IntPtr)((long)intPtr + unchecked((long)(checked(i * num)))), false);
					}
				}
				return intPtr;
			}

			// Token: 0x06000AA9 RID: 2729 RVA: 0x0001F8F8 File Offset: 0x0001E8F8
			internal static IntPtr ConvertRectangleToMemory(RectangleF[] rect)
			{
				if (rect == null)
				{
					throw new ArgumentNullException("rect");
				}
				int num = Marshal.SizeOf(typeof(GPRECTF));
				int num2 = rect.Length;
				IntPtr intPtr = Marshal.AllocHGlobal(checked(num2 * num));
				for (int i = 0; i < num2; i++)
				{
					checked
					{
						Marshal.StructureToPtr(new GPRECTF(rect[i]), (IntPtr)((long)intPtr + unchecked((long)(checked(i * num)))), false);
					}
				}
				return intPtr;
			}

			// Token: 0x06000AAA RID: 2730 RVA: 0x0001F96C File Offset: 0x0001E96C
			internal static IntPtr ConvertRectangleToMemory(Rectangle[] rect)
			{
				if (rect == null)
				{
					throw new ArgumentNullException("rect");
				}
				int num = Marshal.SizeOf(typeof(GPRECT));
				int num2 = rect.Length;
				IntPtr intPtr = Marshal.AllocHGlobal(checked(num2 * num));
				for (int i = 0; i < num2; i++)
				{
					checked
					{
						Marshal.StructureToPtr(new GPRECT(rect[i]), (IntPtr)((long)intPtr + unchecked((long)(checked(i * num)))), false);
					}
				}
				return intPtr;
			}

			// Token: 0x04000873 RID: 2163
			private const string ThreadDataSlotName = "system.drawing.threaddata";

			// Token: 0x04000874 RID: 2164
			internal const int Ok = 0;

			// Token: 0x04000875 RID: 2165
			internal const int GenericError = 1;

			// Token: 0x04000876 RID: 2166
			internal const int InvalidParameter = 2;

			// Token: 0x04000877 RID: 2167
			internal const int OutOfMemory = 3;

			// Token: 0x04000878 RID: 2168
			internal const int ObjectBusy = 4;

			// Token: 0x04000879 RID: 2169
			internal const int InsufficientBuffer = 5;

			// Token: 0x0400087A RID: 2170
			internal const int NotImplemented = 6;

			// Token: 0x0400087B RID: 2171
			internal const int Win32Error = 7;

			// Token: 0x0400087C RID: 2172
			internal const int WrongState = 8;

			// Token: 0x0400087D RID: 2173
			internal const int Aborted = 9;

			// Token: 0x0400087E RID: 2174
			internal const int FileNotFound = 10;

			// Token: 0x0400087F RID: 2175
			internal const int ValueOverflow = 11;

			// Token: 0x04000880 RID: 2176
			internal const int AccessDenied = 12;

			// Token: 0x04000881 RID: 2177
			internal const int UnknownImageFormat = 13;

			// Token: 0x04000882 RID: 2178
			internal const int FontFamilyNotFound = 14;

			// Token: 0x04000883 RID: 2179
			internal const int FontStyleNotFound = 15;

			// Token: 0x04000884 RID: 2180
			internal const int NotTrueTypeFont = 16;

			// Token: 0x04000885 RID: 2181
			internal const int UnsupportedGdiplusVersion = 17;

			// Token: 0x04000886 RID: 2182
			internal const int GdiplusNotInitialized = 18;

			// Token: 0x04000887 RID: 2183
			internal const int PropertyNotFound = 19;

			// Token: 0x04000888 RID: 2184
			internal const int PropertyNotSupported = 20;

			// Token: 0x04000889 RID: 2185
			private static readonly TraceSwitch GdiPlusInitialization = new TraceSwitch("GdiPlusInitialization", "Tracks GDI+ initialization and teardown");

			// Token: 0x0400088A RID: 2186
			private static readonly BooleanSwitch GdiPlusIgnoreAtom = new BooleanSwitch("GdiPlusIgnoreAtom", "Ignores the use of global atoms for startup/shutdown");

			// Token: 0x0400088B RID: 2187
			private static IntPtr initToken;

			// Token: 0x0400088C RID: 2188
			private static string atomName = null;

			// Token: 0x0400088D RID: 2189
			private static ushort hAtom = 0;

			// Token: 0x02000092 RID: 146
			private struct StartupInput
			{
				// Token: 0x06000AAC RID: 2732 RVA: 0x0001F9E8 File Offset: 0x0001E9E8
				public static SafeNativeMethods.Gdip.StartupInput GetDefault()
				{
					return new SafeNativeMethods.Gdip.StartupInput
					{
						GdiplusVersion = 1,
						SuppressBackgroundThread = false,
						SuppressExternalCodecs = false
					};
				}

				// Token: 0x0400088E RID: 2190
				public int GdiplusVersion;

				// Token: 0x0400088F RID: 2191
				public IntPtr DebugEventCallback;

				// Token: 0x04000890 RID: 2192
				public bool SuppressBackgroundThread;

				// Token: 0x04000891 RID: 2193
				public bool SuppressExternalCodecs;
			}

			// Token: 0x02000093 RID: 147
			private struct StartupOutput
			{
				// Token: 0x04000892 RID: 2194
				public IntPtr hook;

				// Token: 0x04000893 RID: 2195
				public IntPtr unhook;
			}

			// Token: 0x02000094 RID: 148
			private enum DebugEventLevel
			{
				// Token: 0x04000895 RID: 2197
				Fatal,
				// Token: 0x04000896 RID: 2198
				Warning
			}
		}

		// Token: 0x02000095 RID: 149
		[StructLayout(LayoutKind.Sequential)]
		public class ENHMETAHEADER
		{
			// Token: 0x04000897 RID: 2199
			public int iType;

			// Token: 0x04000898 RID: 2200
			public int nSize = 40;

			// Token: 0x04000899 RID: 2201
			public int rclBounds_left;

			// Token: 0x0400089A RID: 2202
			public int rclBounds_top;

			// Token: 0x0400089B RID: 2203
			public int rclBounds_right;

			// Token: 0x0400089C RID: 2204
			public int rclBounds_bottom;

			// Token: 0x0400089D RID: 2205
			public int rclFrame_left;

			// Token: 0x0400089E RID: 2206
			public int rclFrame_top;

			// Token: 0x0400089F RID: 2207
			public int rclFrame_right;

			// Token: 0x040008A0 RID: 2208
			public int rclFrame_bottom;

			// Token: 0x040008A1 RID: 2209
			public int dSignature;

			// Token: 0x040008A2 RID: 2210
			public int nVersion;

			// Token: 0x040008A3 RID: 2211
			public int nBytes;

			// Token: 0x040008A4 RID: 2212
			public int nRecords;

			// Token: 0x040008A5 RID: 2213
			public short nHandles;

			// Token: 0x040008A6 RID: 2214
			public short sReserved;

			// Token: 0x040008A7 RID: 2215
			public int nDescription;

			// Token: 0x040008A8 RID: 2216
			public int offDescription;

			// Token: 0x040008A9 RID: 2217
			public int nPalEntries;

			// Token: 0x040008AA RID: 2218
			public int szlDevice_cx;

			// Token: 0x040008AB RID: 2219
			public int szlDevice_cy;

			// Token: 0x040008AC RID: 2220
			public int szlMillimeters_cx;

			// Token: 0x040008AD RID: 2221
			public int szlMillimeters_cy;

			// Token: 0x040008AE RID: 2222
			public int cbPixelFormat;

			// Token: 0x040008AF RID: 2223
			public int offPixelFormat;

			// Token: 0x040008B0 RID: 2224
			public int bOpenGL;
		}

		// Token: 0x02000096 RID: 150
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class DOCINFO
		{
			// Token: 0x040008B1 RID: 2225
			public int cbSize = 20;

			// Token: 0x040008B2 RID: 2226
			public string lpszDocName;

			// Token: 0x040008B3 RID: 2227
			public string lpszOutput;

			// Token: 0x040008B4 RID: 2228
			public string lpszDatatype;

			// Token: 0x040008B5 RID: 2229
			public int fwType;
		}

		// Token: 0x02000097 RID: 151
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class PRINTDLG
		{
			// Token: 0x040008B6 RID: 2230
			public int lStructSize;

			// Token: 0x040008B7 RID: 2231
			public IntPtr hwndOwner;

			// Token: 0x040008B8 RID: 2232
			public IntPtr hDevMode;

			// Token: 0x040008B9 RID: 2233
			public IntPtr hDevNames;

			// Token: 0x040008BA RID: 2234
			public IntPtr hDC;

			// Token: 0x040008BB RID: 2235
			public int Flags;

			// Token: 0x040008BC RID: 2236
			public short nFromPage;

			// Token: 0x040008BD RID: 2237
			public short nToPage;

			// Token: 0x040008BE RID: 2238
			public short nMinPage;

			// Token: 0x040008BF RID: 2239
			public short nMaxPage;

			// Token: 0x040008C0 RID: 2240
			public short nCopies;

			// Token: 0x040008C1 RID: 2241
			public IntPtr hInstance;

			// Token: 0x040008C2 RID: 2242
			public IntPtr lCustData;

			// Token: 0x040008C3 RID: 2243
			public IntPtr lpfnPrintHook;

			// Token: 0x040008C4 RID: 2244
			public IntPtr lpfnSetupHook;

			// Token: 0x040008C5 RID: 2245
			public string lpPrintTemplateName;

			// Token: 0x040008C6 RID: 2246
			public string lpSetupTemplateName;

			// Token: 0x040008C7 RID: 2247
			public IntPtr hPrintTemplate;

			// Token: 0x040008C8 RID: 2248
			public IntPtr hSetupTemplate;
		}

		// Token: 0x02000098 RID: 152
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
		public class PRINTDLGX86
		{
			// Token: 0x040008C9 RID: 2249
			public int lStructSize;

			// Token: 0x040008CA RID: 2250
			public IntPtr hwndOwner;

			// Token: 0x040008CB RID: 2251
			public IntPtr hDevMode;

			// Token: 0x040008CC RID: 2252
			public IntPtr hDevNames;

			// Token: 0x040008CD RID: 2253
			public IntPtr hDC;

			// Token: 0x040008CE RID: 2254
			public int Flags;

			// Token: 0x040008CF RID: 2255
			public short nFromPage;

			// Token: 0x040008D0 RID: 2256
			public short nToPage;

			// Token: 0x040008D1 RID: 2257
			public short nMinPage;

			// Token: 0x040008D2 RID: 2258
			public short nMaxPage;

			// Token: 0x040008D3 RID: 2259
			public short nCopies;

			// Token: 0x040008D4 RID: 2260
			public IntPtr hInstance;

			// Token: 0x040008D5 RID: 2261
			public IntPtr lCustData;

			// Token: 0x040008D6 RID: 2262
			public IntPtr lpfnPrintHook;

			// Token: 0x040008D7 RID: 2263
			public IntPtr lpfnSetupHook;

			// Token: 0x040008D8 RID: 2264
			public string lpPrintTemplateName;

			// Token: 0x040008D9 RID: 2265
			public string lpSetupTemplateName;

			// Token: 0x040008DA RID: 2266
			public IntPtr hPrintTemplate;

			// Token: 0x040008DB RID: 2267
			public IntPtr hSetupTemplate;
		}

		// Token: 0x02000099 RID: 153
		public enum StructFormat
		{
			// Token: 0x040008DD RID: 2269
			Ansi = 1,
			// Token: 0x040008DE RID: 2270
			Unicode,
			// Token: 0x040008DF RID: 2271
			Auto
		}

		// Token: 0x0200009A RID: 154
		public struct RECT
		{
			// Token: 0x040008E0 RID: 2272
			public int left;

			// Token: 0x040008E1 RID: 2273
			public int top;

			// Token: 0x040008E2 RID: 2274
			public int right;

			// Token: 0x040008E3 RID: 2275
			public int bottom;
		}

		// Token: 0x0200009B RID: 155
		public struct MSG
		{
			// Token: 0x040008E4 RID: 2276
			public IntPtr hwnd;

			// Token: 0x040008E5 RID: 2277
			public int message;

			// Token: 0x040008E6 RID: 2278
			public IntPtr wParam;

			// Token: 0x040008E7 RID: 2279
			public IntPtr lParam;

			// Token: 0x040008E8 RID: 2280
			public int time;

			// Token: 0x040008E9 RID: 2281
			public int pt_x;

			// Token: 0x040008EA RID: 2282
			public int pt_y;
		}

		// Token: 0x0200009C RID: 156
		[StructLayout(LayoutKind.Sequential)]
		public class ICONINFO
		{
			// Token: 0x040008EB RID: 2283
			public int fIcon;

			// Token: 0x040008EC RID: 2284
			public int xHotspot;

			// Token: 0x040008ED RID: 2285
			public int yHotspot;

			// Token: 0x040008EE RID: 2286
			public IntPtr hbmMask = IntPtr.Zero;

			// Token: 0x040008EF RID: 2287
			public IntPtr hbmColor = IntPtr.Zero;
		}

		// Token: 0x0200009D RID: 157
		[StructLayout(LayoutKind.Sequential)]
		public class BITMAP
		{
			// Token: 0x040008F0 RID: 2288
			public int bmType;

			// Token: 0x040008F1 RID: 2289
			public int bmWidth;

			// Token: 0x040008F2 RID: 2290
			public int bmHeight;

			// Token: 0x040008F3 RID: 2291
			public int bmWidthBytes;

			// Token: 0x040008F4 RID: 2292
			public short bmPlanes;

			// Token: 0x040008F5 RID: 2293
			public short bmBitsPixel;

			// Token: 0x040008F6 RID: 2294
			public IntPtr bmBits = IntPtr.Zero;
		}

		// Token: 0x0200009E RID: 158
		[StructLayout(LayoutKind.Sequential)]
		public class BITMAPINFOHEADER
		{
			// Token: 0x040008F7 RID: 2295
			public int biSize = 40;

			// Token: 0x040008F8 RID: 2296
			public int biWidth;

			// Token: 0x040008F9 RID: 2297
			public int biHeight;

			// Token: 0x040008FA RID: 2298
			public short biPlanes;

			// Token: 0x040008FB RID: 2299
			public short biBitCount;

			// Token: 0x040008FC RID: 2300
			public int biCompression;

			// Token: 0x040008FD RID: 2301
			public int biSizeImage;

			// Token: 0x040008FE RID: 2302
			public int biXPelsPerMeter;

			// Token: 0x040008FF RID: 2303
			public int biYPelsPerMeter;

			// Token: 0x04000900 RID: 2304
			public int biClrUsed;

			// Token: 0x04000901 RID: 2305
			public int biClrImportant;
		}

		// Token: 0x0200009F RID: 159
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class LOGFONT
		{
			// Token: 0x06000AB4 RID: 2740 RVA: 0x0001FA87 File Offset: 0x0001EA87
			public LOGFONT()
			{
			}

			// Token: 0x06000AB5 RID: 2741 RVA: 0x0001FA90 File Offset: 0x0001EA90
			public LOGFONT(SafeNativeMethods.LOGFONT lf)
			{
				this.lfHeight = lf.lfHeight;
				this.lfWidth = lf.lfWidth;
				this.lfEscapement = lf.lfEscapement;
				this.lfOrientation = lf.lfOrientation;
				this.lfWeight = lf.lfWeight;
				this.lfItalic = lf.lfItalic;
				this.lfUnderline = lf.lfUnderline;
				this.lfStrikeOut = lf.lfStrikeOut;
				this.lfCharSet = lf.lfCharSet;
				this.lfOutPrecision = lf.lfOutPrecision;
				this.lfClipPrecision = lf.lfClipPrecision;
				this.lfQuality = lf.lfQuality;
				this.lfPitchAndFamily = lf.lfPitchAndFamily;
				this.lfFaceName = lf.lfFaceName;
			}

			// Token: 0x06000AB6 RID: 2742 RVA: 0x0001FB4C File Offset: 0x0001EB4C
			public override string ToString()
			{
				return string.Concat(new object[]
				{
					"lfHeight=", this.lfHeight, ", lfWidth=", this.lfWidth, ", lfEscapement=", this.lfEscapement, ", lfOrientation=", this.lfOrientation, ", lfWeight=", this.lfWeight,
					", lfItalic=", this.lfItalic, ", lfUnderline=", this.lfUnderline, ", lfStrikeOut=", this.lfStrikeOut, ", lfCharSet=", this.lfCharSet, ", lfOutPrecision=", this.lfOutPrecision,
					", lfClipPrecision=", this.lfClipPrecision, ", lfQuality=", this.lfQuality, ", lfPitchAndFamily=", this.lfPitchAndFamily, ", lfFaceName=", this.lfFaceName
				});
			}

			// Token: 0x04000902 RID: 2306
			public int lfHeight;

			// Token: 0x04000903 RID: 2307
			public int lfWidth;

			// Token: 0x04000904 RID: 2308
			public int lfEscapement;

			// Token: 0x04000905 RID: 2309
			public int lfOrientation;

			// Token: 0x04000906 RID: 2310
			public int lfWeight;

			// Token: 0x04000907 RID: 2311
			public byte lfItalic;

			// Token: 0x04000908 RID: 2312
			public byte lfUnderline;

			// Token: 0x04000909 RID: 2313
			public byte lfStrikeOut;

			// Token: 0x0400090A RID: 2314
			public byte lfCharSet;

			// Token: 0x0400090B RID: 2315
			public byte lfOutPrecision;

			// Token: 0x0400090C RID: 2316
			public byte lfClipPrecision;

			// Token: 0x0400090D RID: 2317
			public byte lfQuality;

			// Token: 0x0400090E RID: 2318
			public byte lfPitchAndFamily;

			// Token: 0x0400090F RID: 2319
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string lfFaceName;
		}

		// Token: 0x020000A0 RID: 160
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct TEXTMETRIC
		{
			// Token: 0x04000910 RID: 2320
			public int tmHeight;

			// Token: 0x04000911 RID: 2321
			public int tmAscent;

			// Token: 0x04000912 RID: 2322
			public int tmDescent;

			// Token: 0x04000913 RID: 2323
			public int tmInternalLeading;

			// Token: 0x04000914 RID: 2324
			public int tmExternalLeading;

			// Token: 0x04000915 RID: 2325
			public int tmAveCharWidth;

			// Token: 0x04000916 RID: 2326
			public int tmMaxCharWidth;

			// Token: 0x04000917 RID: 2327
			public int tmWeight;

			// Token: 0x04000918 RID: 2328
			public int tmOverhang;

			// Token: 0x04000919 RID: 2329
			public int tmDigitizedAspectX;

			// Token: 0x0400091A RID: 2330
			public int tmDigitizedAspectY;

			// Token: 0x0400091B RID: 2331
			public char tmFirstChar;

			// Token: 0x0400091C RID: 2332
			public char tmLastChar;

			// Token: 0x0400091D RID: 2333
			public char tmDefaultChar;

			// Token: 0x0400091E RID: 2334
			public char tmBreakChar;

			// Token: 0x0400091F RID: 2335
			public byte tmItalic;

			// Token: 0x04000920 RID: 2336
			public byte tmUnderlined;

			// Token: 0x04000921 RID: 2337
			public byte tmStruckOut;

			// Token: 0x04000922 RID: 2338
			public byte tmPitchAndFamily;

			// Token: 0x04000923 RID: 2339
			public byte tmCharSet;
		}

		// Token: 0x020000A1 RID: 161
		public struct TEXTMETRICA
		{
			// Token: 0x04000924 RID: 2340
			public int tmHeight;

			// Token: 0x04000925 RID: 2341
			public int tmAscent;

			// Token: 0x04000926 RID: 2342
			public int tmDescent;

			// Token: 0x04000927 RID: 2343
			public int tmInternalLeading;

			// Token: 0x04000928 RID: 2344
			public int tmExternalLeading;

			// Token: 0x04000929 RID: 2345
			public int tmAveCharWidth;

			// Token: 0x0400092A RID: 2346
			public int tmMaxCharWidth;

			// Token: 0x0400092B RID: 2347
			public int tmWeight;

			// Token: 0x0400092C RID: 2348
			public int tmOverhang;

			// Token: 0x0400092D RID: 2349
			public int tmDigitizedAspectX;

			// Token: 0x0400092E RID: 2350
			public int tmDigitizedAspectY;

			// Token: 0x0400092F RID: 2351
			public byte tmFirstChar;

			// Token: 0x04000930 RID: 2352
			public byte tmLastChar;

			// Token: 0x04000931 RID: 2353
			public byte tmDefaultChar;

			// Token: 0x04000932 RID: 2354
			public byte tmBreakChar;

			// Token: 0x04000933 RID: 2355
			public byte tmItalic;

			// Token: 0x04000934 RID: 2356
			public byte tmUnderlined;

			// Token: 0x04000935 RID: 2357
			public byte tmStruckOut;

			// Token: 0x04000936 RID: 2358
			public byte tmPitchAndFamily;

			// Token: 0x04000937 RID: 2359
			public byte tmCharSet;
		}

		// Token: 0x020000A2 RID: 162
		[StructLayout(LayoutKind.Sequential, Pack = 2)]
		public struct ICONDIR
		{
			// Token: 0x04000938 RID: 2360
			public short idReserved;

			// Token: 0x04000939 RID: 2361
			public short idType;

			// Token: 0x0400093A RID: 2362
			public short idCount;

			// Token: 0x0400093B RID: 2363
			public SafeNativeMethods.ICONDIRENTRY idEntries;
		}

		// Token: 0x020000A3 RID: 163
		public struct ICONDIRENTRY
		{
			// Token: 0x0400093C RID: 2364
			public byte bWidth;

			// Token: 0x0400093D RID: 2365
			public byte bHeight;

			// Token: 0x0400093E RID: 2366
			public byte bColorCount;

			// Token: 0x0400093F RID: 2367
			public byte bReserved;

			// Token: 0x04000940 RID: 2368
			public short wPlanes;

			// Token: 0x04000941 RID: 2369
			public short wBitCount;

			// Token: 0x04000942 RID: 2370
			public int dwBytesInRes;

			// Token: 0x04000943 RID: 2371
			public int dwImageOffset;
		}

		// Token: 0x020000A4 RID: 164
		public class Ole
		{
			// Token: 0x04000944 RID: 2372
			public const int PICTYPE_UNINITIALIZED = -1;

			// Token: 0x04000945 RID: 2373
			public const int PICTYPE_NONE = 0;

			// Token: 0x04000946 RID: 2374
			public const int PICTYPE_BITMAP = 1;

			// Token: 0x04000947 RID: 2375
			public const int PICTYPE_METAFILE = 2;

			// Token: 0x04000948 RID: 2376
			public const int PICTYPE_ICON = 3;

			// Token: 0x04000949 RID: 2377
			public const int PICTYPE_ENHMETAFILE = 4;

			// Token: 0x0400094A RID: 2378
			public const int STATFLAG_DEFAULT = 0;

			// Token: 0x0400094B RID: 2379
			public const int STATFLAG_NONAME = 1;
		}

		// Token: 0x020000A5 RID: 165
		[StructLayout(LayoutKind.Sequential)]
		public class PICTDESC
		{
			// Token: 0x06000AB8 RID: 2744 RVA: 0x0001FCB4 File Offset: 0x0001ECB4
			public static SafeNativeMethods.PICTDESC CreateIconPICTDESC(IntPtr hicon)
			{
				return new SafeNativeMethods.PICTDESC
				{
					cbSizeOfStruct = 12,
					picType = 3,
					union1 = hicon
				};
			}

			// Token: 0x06000AB9 RID: 2745 RVA: 0x0001FCDE File Offset: 0x0001ECDE
			public virtual IntPtr GetHandle()
			{
				return this.union1;
			}

			// Token: 0x0400094C RID: 2380
			internal int cbSizeOfStruct;

			// Token: 0x0400094D RID: 2381
			public int picType;

			// Token: 0x0400094E RID: 2382
			internal IntPtr union1;

			// Token: 0x0400094F RID: 2383
			internal int union2;

			// Token: 0x04000950 RID: 2384
			internal int union3;
		}

		// Token: 0x020000A6 RID: 166
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class DEVMODE
		{
			// Token: 0x06000ABB RID: 2747 RVA: 0x0001FCF0 File Offset: 0x0001ECF0
			public override string ToString()
			{
				return string.Concat(new object[]
				{
					"[DEVMODE: dmDeviceName=", this.dmDeviceName, ", dmSpecVersion=", this.dmSpecVersion, ", dmDriverVersion=", this.dmDriverVersion, ", dmSize=", this.dmSize, ", dmDriverExtra=", this.dmDriverExtra,
					", dmFields=", this.dmFields, ", dmOrientation=", this.dmOrientation, ", dmPaperSize=", this.dmPaperSize, ", dmPaperLength=", this.dmPaperLength, ", dmPaperWidth=", this.dmPaperWidth,
					", dmScale=", this.dmScale, ", dmCopies=", this.dmCopies, ", dmDefaultSource=", this.dmDefaultSource, ", dmPrintQuality=", this.dmPrintQuality, ", dmColor=", this.dmColor,
					", dmDuplex=", this.dmDuplex, ", dmYResolution=", this.dmYResolution, ", dmTTOption=", this.dmTTOption, ", dmCollate=", this.dmCollate, ", dmFormName=", this.dmFormName,
					", dmLogPixels=", this.dmLogPixels, ", dmBitsPerPel=", this.dmBitsPerPel, ", dmPelsWidth=", this.dmPelsWidth, ", dmPelsHeight=", this.dmPelsHeight, ", dmDisplayFlags=", this.dmDisplayFlags,
					", dmDisplayFrequency=", this.dmDisplayFrequency, ", dmICMMethod=", this.dmICMMethod, ", dmICMIntent=", this.dmICMIntent, ", dmMediaType=", this.dmMediaType, ", dmDitherType=", this.dmDitherType,
					", dmICCManufacturer=", this.dmICCManufacturer, ", dmICCModel=", this.dmICCModel, ", dmPanningWidth=", this.dmPanningWidth, ", dmPanningHeight=", this.dmPanningHeight, "]"
				});
			}

			// Token: 0x04000951 RID: 2385
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string dmDeviceName;

			// Token: 0x04000952 RID: 2386
			public short dmSpecVersion;

			// Token: 0x04000953 RID: 2387
			public short dmDriverVersion;

			// Token: 0x04000954 RID: 2388
			public short dmSize;

			// Token: 0x04000955 RID: 2389
			public short dmDriverExtra;

			// Token: 0x04000956 RID: 2390
			public int dmFields;

			// Token: 0x04000957 RID: 2391
			public short dmOrientation;

			// Token: 0x04000958 RID: 2392
			public short dmPaperSize;

			// Token: 0x04000959 RID: 2393
			public short dmPaperLength;

			// Token: 0x0400095A RID: 2394
			public short dmPaperWidth;

			// Token: 0x0400095B RID: 2395
			public short dmScale;

			// Token: 0x0400095C RID: 2396
			public short dmCopies;

			// Token: 0x0400095D RID: 2397
			public short dmDefaultSource;

			// Token: 0x0400095E RID: 2398
			public short dmPrintQuality;

			// Token: 0x0400095F RID: 2399
			public short dmColor;

			// Token: 0x04000960 RID: 2400
			public short dmDuplex;

			// Token: 0x04000961 RID: 2401
			public short dmYResolution;

			// Token: 0x04000962 RID: 2402
			public short dmTTOption;

			// Token: 0x04000963 RID: 2403
			public short dmCollate;

			// Token: 0x04000964 RID: 2404
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string dmFormName;

			// Token: 0x04000965 RID: 2405
			public short dmLogPixels;

			// Token: 0x04000966 RID: 2406
			public int dmBitsPerPel;

			// Token: 0x04000967 RID: 2407
			public int dmPelsWidth;

			// Token: 0x04000968 RID: 2408
			public int dmPelsHeight;

			// Token: 0x04000969 RID: 2409
			public int dmDisplayFlags;

			// Token: 0x0400096A RID: 2410
			public int dmDisplayFrequency;

			// Token: 0x0400096B RID: 2411
			public int dmICMMethod;

			// Token: 0x0400096C RID: 2412
			public int dmICMIntent;

			// Token: 0x0400096D RID: 2413
			public int dmMediaType;

			// Token: 0x0400096E RID: 2414
			public int dmDitherType;

			// Token: 0x0400096F RID: 2415
			public int dmICCManufacturer;

			// Token: 0x04000970 RID: 2416
			public int dmICCModel;

			// Token: 0x04000971 RID: 2417
			public int dmPanningWidth;

			// Token: 0x04000972 RID: 2418
			public int dmPanningHeight;
		}

		// Token: 0x020000A7 RID: 167
		public sealed class CommonHandles
		{
			// Token: 0x04000973 RID: 2419
			public static readonly int Accelerator = global::System.Internal.HandleCollector.RegisterType("Accelerator", 80, 50);

			// Token: 0x04000974 RID: 2420
			public static readonly int Cursor = global::System.Internal.HandleCollector.RegisterType("Cursor", 20, 500);

			// Token: 0x04000975 RID: 2421
			public static readonly int EMF = global::System.Internal.HandleCollector.RegisterType("EnhancedMetaFile", 20, 500);

			// Token: 0x04000976 RID: 2422
			public static readonly int Find = global::System.Internal.HandleCollector.RegisterType("Find", 0, 1000);

			// Token: 0x04000977 RID: 2423
			public static readonly int GDI = global::System.Internal.HandleCollector.RegisterType("GDI", 50, 500);

			// Token: 0x04000978 RID: 2424
			public static readonly int HDC = global::System.Internal.HandleCollector.RegisterType("HDC", 100, 2);

			// Token: 0x04000979 RID: 2425
			public static readonly int Icon = global::System.Internal.HandleCollector.RegisterType("Icon", 20, 500);

			// Token: 0x0400097A RID: 2426
			public static readonly int Kernel = global::System.Internal.HandleCollector.RegisterType("Kernel", 0, 1000);

			// Token: 0x0400097B RID: 2427
			public static readonly int Menu = global::System.Internal.HandleCollector.RegisterType("Menu", 30, 1000);

			// Token: 0x0400097C RID: 2428
			public static readonly int Window = global::System.Internal.HandleCollector.RegisterType("Window", 5, 1000);
		}

		// Token: 0x020000A8 RID: 168
		public class StreamConsts
		{
			// Token: 0x0400097D RID: 2429
			public const int LOCK_WRITE = 1;

			// Token: 0x0400097E RID: 2430
			public const int LOCK_EXCLUSIVE = 2;

			// Token: 0x0400097F RID: 2431
			public const int LOCK_ONLYONCE = 4;

			// Token: 0x04000980 RID: 2432
			public const int STATFLAG_DEFAULT = 0;

			// Token: 0x04000981 RID: 2433
			public const int STATFLAG_NONAME = 1;

			// Token: 0x04000982 RID: 2434
			public const int STATFLAG_NOOPEN = 2;

			// Token: 0x04000983 RID: 2435
			public const int STGC_DEFAULT = 0;

			// Token: 0x04000984 RID: 2436
			public const int STGC_OVERWRITE = 1;

			// Token: 0x04000985 RID: 2437
			public const int STGC_ONLYIFCURRENT = 2;

			// Token: 0x04000986 RID: 2438
			public const int STGC_DANGEROUSLYCOMMITMERELYTODISKCACHE = 4;

			// Token: 0x04000987 RID: 2439
			public const int STREAM_SEEK_SET = 0;

			// Token: 0x04000988 RID: 2440
			public const int STREAM_SEEK_CUR = 1;

			// Token: 0x04000989 RID: 2441
			public const int STREAM_SEEK_END = 2;
		}

		// Token: 0x020000A9 RID: 169
		[Guid("7BF80980-BF32-101A-8BBB-00AA00300CAB")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IPicture
		{
			// Token: 0x06000AC0 RID: 2752
			[SuppressUnmanagedCodeSecurity]
			IntPtr GetHandle();

			// Token: 0x06000AC1 RID: 2753
			[SuppressUnmanagedCodeSecurity]
			IntPtr GetHPal();

			// Token: 0x06000AC2 RID: 2754
			[SuppressUnmanagedCodeSecurity]
			[return: MarshalAs(UnmanagedType.I2)]
			short GetPictureType();

			// Token: 0x06000AC3 RID: 2755
			[SuppressUnmanagedCodeSecurity]
			int GetWidth();

			// Token: 0x06000AC4 RID: 2756
			[SuppressUnmanagedCodeSecurity]
			int GetHeight();

			// Token: 0x06000AC5 RID: 2757
			[SuppressUnmanagedCodeSecurity]
			void Render();

			// Token: 0x06000AC6 RID: 2758
			[SuppressUnmanagedCodeSecurity]
			void SetHPal([In] IntPtr phpal);

			// Token: 0x06000AC7 RID: 2759
			[SuppressUnmanagedCodeSecurity]
			IntPtr GetCurDC();

			// Token: 0x06000AC8 RID: 2760
			[SuppressUnmanagedCodeSecurity]
			void SelectPicture([In] IntPtr hdcIn, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] phdcOut, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] phbmpOut);

			// Token: 0x06000AC9 RID: 2761
			[SuppressUnmanagedCodeSecurity]
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetKeepOriginalFormat();

			// Token: 0x06000ACA RID: 2762
			[SuppressUnmanagedCodeSecurity]
			void SetKeepOriginalFormat([MarshalAs(UnmanagedType.Bool)] [In] bool pfkeep);

			// Token: 0x06000ACB RID: 2763
			[SuppressUnmanagedCodeSecurity]
			void PictureChanged();

			// Token: 0x06000ACC RID: 2764
			[SuppressUnmanagedCodeSecurity]
			[PreserveSig]
			int SaveAsFile([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStream pstm, [In] int fSaveMemCopy, out int pcbSize);

			// Token: 0x06000ACD RID: 2765
			[SuppressUnmanagedCodeSecurity]
			int GetAttributes();

			// Token: 0x06000ACE RID: 2766
			[SuppressUnmanagedCodeSecurity]
			void SetHdc([In] IntPtr hdc);
		}

		// Token: 0x020000AA RID: 170
		public struct OBJECTHEADER
		{
			// Token: 0x0400098A RID: 2442
			public short signature;

			// Token: 0x0400098B RID: 2443
			public short headersize;

			// Token: 0x0400098C RID: 2444
			public short objectType;

			// Token: 0x0400098D RID: 2445
			public short nameLen;

			// Token: 0x0400098E RID: 2446
			public short classLen;

			// Token: 0x0400098F RID: 2447
			public short nameOffset;

			// Token: 0x04000990 RID: 2448
			public short classOffset;

			// Token: 0x04000991 RID: 2449
			public short width;

			// Token: 0x04000992 RID: 2450
			public short height;

			// Token: 0x04000993 RID: 2451
			public IntPtr pInfo;
		}

		// Token: 0x020000AB RID: 171
		internal enum Win32SystemColors
		{
			// Token: 0x04000995 RID: 2453
			ActiveBorder = 10,
			// Token: 0x04000996 RID: 2454
			ActiveCaption = 2,
			// Token: 0x04000997 RID: 2455
			ActiveCaptionText = 9,
			// Token: 0x04000998 RID: 2456
			AppWorkspace = 12,
			// Token: 0x04000999 RID: 2457
			ButtonFace = 15,
			// Token: 0x0400099A RID: 2458
			ButtonHighlight = 20,
			// Token: 0x0400099B RID: 2459
			ButtonShadow = 16,
			// Token: 0x0400099C RID: 2460
			Control = 15,
			// Token: 0x0400099D RID: 2461
			ControlDark,
			// Token: 0x0400099E RID: 2462
			ControlDarkDark = 21,
			// Token: 0x0400099F RID: 2463
			ControlLight,
			// Token: 0x040009A0 RID: 2464
			ControlLightLight = 20,
			// Token: 0x040009A1 RID: 2465
			ControlText = 18,
			// Token: 0x040009A2 RID: 2466
			Desktop = 1,
			// Token: 0x040009A3 RID: 2467
			GradientActiveCaption = 27,
			// Token: 0x040009A4 RID: 2468
			GradientInactiveCaption,
			// Token: 0x040009A5 RID: 2469
			GrayText = 17,
			// Token: 0x040009A6 RID: 2470
			Highlight = 13,
			// Token: 0x040009A7 RID: 2471
			HighlightText,
			// Token: 0x040009A8 RID: 2472
			HotTrack = 26,
			// Token: 0x040009A9 RID: 2473
			InactiveBorder = 11,
			// Token: 0x040009AA RID: 2474
			InactiveCaption = 3,
			// Token: 0x040009AB RID: 2475
			InactiveCaptionText = 19,
			// Token: 0x040009AC RID: 2476
			Info = 24,
			// Token: 0x040009AD RID: 2477
			InfoText = 23,
			// Token: 0x040009AE RID: 2478
			Menu = 4,
			// Token: 0x040009AF RID: 2479
			MenuBar = 30,
			// Token: 0x040009B0 RID: 2480
			MenuHighlight = 29,
			// Token: 0x040009B1 RID: 2481
			MenuText = 7,
			// Token: 0x040009B2 RID: 2482
			ScrollBar = 0,
			// Token: 0x040009B3 RID: 2483
			Window = 5,
			// Token: 0x040009B4 RID: 2484
			WindowFrame,
			// Token: 0x040009B5 RID: 2485
			WindowText = 8
		}

		// Token: 0x020000AC RID: 172
		public enum BackgroundMode
		{
			// Token: 0x040009B7 RID: 2487
			TRANSPARENT = 1,
			// Token: 0x040009B8 RID: 2488
			OPAQUE
		}
	}
}
