using System;
using System.Drawing;
using System.Internal;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Design
{
	// Token: 0x0200000F RID: 15
	internal class NativeMethods
	{
		// Token: 0x06000029 RID: 41
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern int MultiByteToWideChar(int CodePage, int dwFlags, byte[] lpMultiByteStr, int cchMultiByte, char[] lpWideCharStr, int cchWideChar);

		// Token: 0x0600002A RID: 42
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetClientRect(IntPtr hWnd, [In] [Out] NativeMethods.COMRECT rect);

		// Token: 0x0600002B RID: 43
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool PeekMessage([In] [Out] ref NativeMethods.MSG msg, IntPtr hwnd, int msgMin, int msgMax, int remove);

		// Token: 0x0600002C RID: 44
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetCursor();

		// Token: 0x0600002D RID: 45
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetCursorPos([In] [Out] NativeMethods.POINT pt);

		// Token: 0x0600002E RID: 46
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr WindowFromPoint(int x, int y);

		// Token: 0x0600002F RID: 47
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000030 RID: 48
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, [In] [Out] NativeMethods.HDHITTESTINFO lParam);

		// Token: 0x06000031 RID: 49
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

		// Token: 0x06000032 RID: 50
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);

		// Token: 0x06000033 RID: 51
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, [In] [Out] NativeMethods.TV_HITTESTINFO lParam);

		// Token: 0x06000034 RID: 52
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr DefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000035 RID: 53
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetWindow(IntPtr hWnd, int uCmd);

		// Token: 0x06000036 RID: 54
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern short GetKeyState(int keyCode);

		// Token: 0x06000037 RID: 55
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [In] [Out] ref NativeMethods.RECT rect, int cPoints);

		// Token: 0x06000038 RID: 56
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [In] [Out] NativeMethods.POINT pt, int cPoints);

		// Token: 0x06000039 RID: 57
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool ValidateRect(IntPtr hwnd, IntPtr prect);

		// Token: 0x0600003A RID: 58
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateRectRgn", ExactSpelling = true)]
		private static extern IntPtr IntCreateRectRgn(int x1, int y1, int x2, int y2);

		// Token: 0x0600003B RID: 59 RVA: 0x00002890 File Offset: 0x00001890
		public static IntPtr CreateRectRgn(int x1, int y1, int x2, int y2)
		{
			return global::System.Internal.HandleCollector.Add(NativeMethods.IntCreateRectRgn(x1, y1, x2, y2), NativeMethods.CommonHandles.GDI);
		}

		// Token: 0x0600003C RID: 60
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetUpdateRect(IntPtr hwnd, [In] [Out] ref NativeMethods.RECT rc, bool fErase);

		// Token: 0x0600003D RID: 61
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetUpdateRgn(IntPtr hwnd, IntPtr hrgn, bool fErase);

		// Token: 0x0600003E RID: 62
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "DeleteObject", ExactSpelling = true)]
		public static extern bool ExternalDeleteObject(HandleRef hObject);

		// Token: 0x0600003F RID: 63
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "DeleteObject", ExactSpelling = true)]
		private static extern bool IntDeleteObject(IntPtr hObject);

		// Token: 0x06000040 RID: 64 RVA: 0x000028A5 File Offset: 0x000018A5
		public static bool DeleteObject(IntPtr hObject)
		{
			global::System.Internal.HandleCollector.Remove(hObject, NativeMethods.CommonHandles.GDI);
			return NativeMethods.IntDeleteObject(hObject);
		}

		// Token: 0x06000041 RID: 65
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr SetParent(IntPtr hWnd, IntPtr hWndParent);

		// Token: 0x06000042 RID: 66
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool TranslateMessage([In] [Out] ref NativeMethods.MSG msg);

		// Token: 0x06000043 RID: 67
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int DispatchMessage([In] ref NativeMethods.MSG msg);

		// Token: 0x06000044 RID: 68
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetWindowRect(IntPtr hWnd, [In] [Out] ref NativeMethods.RECT rect);

		// Token: 0x06000045 RID: 69
		[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int RevokeDragDrop(IntPtr hwnd);

		// Token: 0x06000046 RID: 70
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr ChildWindowFromPointEx(IntPtr hwndParent, int x, int y, int uFlags);

		// Token: 0x06000047 RID: 71
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool IsWindowVisible(IntPtr hWnd);

		// Token: 0x06000048 RID: 72
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetFocus();

		// Token: 0x06000049 RID: 73 RVA: 0x000028B9 File Offset: 0x000018B9
		public static bool Succeeded(int hr)
		{
			return hr >= 0;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000028C2 File Offset: 0x000018C2
		public static bool Failed(int hr)
		{
			return hr < 0;
		}

		// Token: 0x0600004B RID: 75
		[DllImport("oleaut32.dll", PreserveSig = false)]
		public static extern ITypeLib LoadRegTypeLib(ref Guid clsid, short majorVersion, short minorVersion, int lcid);

		// Token: 0x0600004C RID: 76
		[DllImport("oleaut32.dll", PreserveSig = false)]
		public static extern ITypeLib LoadTypeLib([MarshalAs(UnmanagedType.LPWStr)] [In] string typelib);

		// Token: 0x0600004D RID: 77
		[DllImport("oleaut32.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.BStr)]
		public static extern string QueryPathOfRegTypeLib(ref Guid guid, short majorVersion, short minorVersion, int lcid);

		// Token: 0x04000788 RID: 1928
		public const int HOLLOW_BRUSH = 5;

		// Token: 0x04000789 RID: 1929
		public const int WM_USER = 1024;

		// Token: 0x0400078A RID: 1930
		public const int WM_CLOSE = 16;

		// Token: 0x0400078B RID: 1931
		public const int WM_GETDLGCODE = 135;

		// Token: 0x0400078C RID: 1932
		public const int WM_MOUSEMOVE = 512;

		// Token: 0x0400078D RID: 1933
		public const int WM_NOTIFY = 78;

		// Token: 0x0400078E RID: 1934
		public const int DLGC_WANTALLKEYS = 4;

		// Token: 0x0400078F RID: 1935
		public const int NM_CLICK = -2;

		// Token: 0x04000790 RID: 1936
		public const int WM_REFLECT = 8192;

		// Token: 0x04000791 RID: 1937
		public const int BM_SETIMAGE = 247;

		// Token: 0x04000792 RID: 1938
		public const int IMAGE_ICON = 1;

		// Token: 0x04000793 RID: 1939
		public const int WM_DESTROY = 2;

		// Token: 0x04000794 RID: 1940
		public const int BS_ICON = 64;

		// Token: 0x04000795 RID: 1941
		public const int VK_PROCESSKEY = 229;

		// Token: 0x04000796 RID: 1942
		public const int STGM_READ = 0;

		// Token: 0x04000797 RID: 1943
		public const int STGM_WRITE = 1;

		// Token: 0x04000798 RID: 1944
		public const int STGM_READWRITE = 2;

		// Token: 0x04000799 RID: 1945
		public const int STGM_SHARE_EXCLUSIVE = 16;

		// Token: 0x0400079A RID: 1946
		public const int STGM_CREATE = 4096;

		// Token: 0x0400079B RID: 1947
		public const int STGM_TRANSACTED = 65536;

		// Token: 0x0400079C RID: 1948
		public const int STGM_CONVERT = 131072;

		// Token: 0x0400079D RID: 1949
		public const int STGM_DELETEONRELEASE = 67108864;

		// Token: 0x0400079E RID: 1950
		public const int RECO_PASTE = 0;

		// Token: 0x0400079F RID: 1951
		public const int RECO_DROP = 1;

		// Token: 0x040007A0 RID: 1952
		public const int TCM_HITTEST = 4877;

		// Token: 0x040007A1 RID: 1953
		public const int S_OK = 0;

		// Token: 0x040007A2 RID: 1954
		public const int S_FALSE = 1;

		// Token: 0x040007A3 RID: 1955
		public const int E_NOTIMPL = -2147467263;

		// Token: 0x040007A4 RID: 1956
		public const int E_NOINTERFACE = -2147467262;

		// Token: 0x040007A5 RID: 1957
		public const int E_INVALIDARG = -2147024809;

		// Token: 0x040007A6 RID: 1958
		public const int E_FAIL = -2147467259;

		// Token: 0x040007A7 RID: 1959
		public const int WS_EX_STATICEDGE = 131072;

		// Token: 0x040007A8 RID: 1960
		public const int OLEIVERB_PRIMARY = 0;

		// Token: 0x040007A9 RID: 1961
		public const int OLEIVERB_SHOW = -1;

		// Token: 0x040007AA RID: 1962
		public const int OLEIVERB_OPEN = -2;

		// Token: 0x040007AB RID: 1963
		public const int OLEIVERB_HIDE = -3;

		// Token: 0x040007AC RID: 1964
		public const int OLEIVERB_UIACTIVATE = -4;

		// Token: 0x040007AD RID: 1965
		public const int OLEIVERB_INPLACEACTIVATE = -5;

		// Token: 0x040007AE RID: 1966
		public const int OLEIVERB_DISCARDUNDOSTATE = -6;

		// Token: 0x040007AF RID: 1967
		public const int OLEIVERB_PROPERTIES = -7;

		// Token: 0x040007B0 RID: 1968
		public const int OLECLOSE_SAVEIFDIRTY = 0;

		// Token: 0x040007B1 RID: 1969
		public const int OLECLOSE_NOSAVE = 1;

		// Token: 0x040007B2 RID: 1970
		public const int OLECLOSE_PROMPTSAVE = 2;

		// Token: 0x040007B3 RID: 1971
		public const int PM_NOREMOVE = 0;

		// Token: 0x040007B4 RID: 1972
		public const int PM_REMOVE = 1;

		// Token: 0x040007B5 RID: 1973
		public const int WM_CHAR = 258;

		// Token: 0x040007B6 RID: 1974
		public const int DT_CALCRECT = 1024;

		// Token: 0x040007B7 RID: 1975
		public const int WM_CAPTURECHANGED = 533;

		// Token: 0x040007B8 RID: 1976
		public const int WM_PARENTNOTIFY = 528;

		// Token: 0x040007B9 RID: 1977
		public const int WM_CREATE = 1;

		// Token: 0x040007BA RID: 1978
		public const int WM_SETREDRAW = 11;

		// Token: 0x040007BB RID: 1979
		public const int WM_NCACTIVATE = 134;

		// Token: 0x040007BC RID: 1980
		public const int WM_HSCROLL = 276;

		// Token: 0x040007BD RID: 1981
		public const int WM_VSCROLL = 277;

		// Token: 0x040007BE RID: 1982
		public const int WM_SHOWWINDOW = 24;

		// Token: 0x040007BF RID: 1983
		public const int WM_WINDOWPOSCHANGING = 70;

		// Token: 0x040007C0 RID: 1984
		public const int WM_WINDOWPOSCHANGED = 71;

		// Token: 0x040007C1 RID: 1985
		public const int WS_DISABLED = 134217728;

		// Token: 0x040007C2 RID: 1986
		public const int WS_CLIPSIBLINGS = 67108864;

		// Token: 0x040007C3 RID: 1987
		public const int WS_CLIPCHILDREN = 33554432;

		// Token: 0x040007C4 RID: 1988
		public const int WS_EX_TOOLWINDOW = 128;

		// Token: 0x040007C5 RID: 1989
		public const int WS_POPUP = -2147483648;

		// Token: 0x040007C6 RID: 1990
		public const int WS_BORDER = 8388608;

		// Token: 0x040007C7 RID: 1991
		public const int CS_DROPSHADOW = 131072;

		// Token: 0x040007C8 RID: 1992
		public const int CS_DBLCLKS = 8;

		// Token: 0x040007C9 RID: 1993
		public const int NOTSRCCOPY = 3342344;

		// Token: 0x040007CA RID: 1994
		public const int SRCCOPY = 13369376;

		// Token: 0x040007CB RID: 1995
		public const int LVM_SETCOLUMNWIDTH = 4126;

		// Token: 0x040007CC RID: 1996
		public const int LVM_GETHEADER = 4127;

		// Token: 0x040007CD RID: 1997
		public const int LVM_CREATEDRAGIMAGE = 4129;

		// Token: 0x040007CE RID: 1998
		public const int LVM_GETVIEWRECT = 4130;

		// Token: 0x040007CF RID: 1999
		public const int LVM_GETTEXTCOLOR = 4131;

		// Token: 0x040007D0 RID: 2000
		public const int LVM_SETTEXTCOLOR = 4132;

		// Token: 0x040007D1 RID: 2001
		public const int LVM_GETTEXTBKCOLOR = 4133;

		// Token: 0x040007D2 RID: 2002
		public const int LVM_SETTEXTBKCOLOR = 4134;

		// Token: 0x040007D3 RID: 2003
		public const int LVM_GETTOPINDEX = 4135;

		// Token: 0x040007D4 RID: 2004
		public const int LVM_GETCOUNTPERPAGE = 4136;

		// Token: 0x040007D5 RID: 2005
		public const int LVM_GETORIGIN = 4137;

		// Token: 0x040007D6 RID: 2006
		public const int LVM_UPDATE = 4138;

		// Token: 0x040007D7 RID: 2007
		public const int LVM_SETITEMSTATE = 4139;

		// Token: 0x040007D8 RID: 2008
		public const int LVM_GETITEMSTATE = 4140;

		// Token: 0x040007D9 RID: 2009
		public const int LVM_GETITEMTEXTA = 4141;

		// Token: 0x040007DA RID: 2010
		public const int LVM_GETITEMTEXTW = 4211;

		// Token: 0x040007DB RID: 2011
		public const int LVM_SETITEMTEXTA = 4142;

		// Token: 0x040007DC RID: 2012
		public const int LVM_SETITEMTEXTW = 4212;

		// Token: 0x040007DD RID: 2013
		public const int LVSICF_NOINVALIDATEALL = 1;

		// Token: 0x040007DE RID: 2014
		public const int LVSICF_NOSCROLL = 2;

		// Token: 0x040007DF RID: 2015
		public const int LVM_SETITEMCOUNT = 4143;

		// Token: 0x040007E0 RID: 2016
		public const int LVM_SORTITEMS = 4144;

		// Token: 0x040007E1 RID: 2017
		public const int LVM_SETITEMPOSITION32 = 4145;

		// Token: 0x040007E2 RID: 2018
		public const int LVM_GETSELECTEDCOUNT = 4146;

		// Token: 0x040007E3 RID: 2019
		public const int LVM_GETITEMSPACING = 4147;

		// Token: 0x040007E4 RID: 2020
		public const int LVM_GETISEARCHSTRINGA = 4148;

		// Token: 0x040007E5 RID: 2021
		public const int LVM_GETISEARCHSTRINGW = 4213;

		// Token: 0x040007E6 RID: 2022
		public const int LVM_SETICONSPACING = 4149;

		// Token: 0x040007E7 RID: 2023
		public const int LVM_SETEXTENDEDLISTVIEWSTYLE = 4150;

		// Token: 0x040007E8 RID: 2024
		public const int LVM_GETEXTENDEDLISTVIEWSTYLE = 4151;

		// Token: 0x040007E9 RID: 2025
		public const int LVS_EX_GRIDLINES = 1;

		// Token: 0x040007EA RID: 2026
		public const int HDM_HITTEST = 4614;

		// Token: 0x040007EB RID: 2027
		public const int HDM_GETITEMRECT = 4615;

		// Token: 0x040007EC RID: 2028
		public const int HDM_SETIMAGELIST = 4616;

		// Token: 0x040007ED RID: 2029
		public const int HDM_GETIMAGELIST = 4617;

		// Token: 0x040007EE RID: 2030
		public const int HDM_ORDERTOINDEX = 4623;

		// Token: 0x040007EF RID: 2031
		public const int HDM_CREATEDRAGIMAGE = 4624;

		// Token: 0x040007F0 RID: 2032
		public const int HDM_GETORDERARRAY = 4625;

		// Token: 0x040007F1 RID: 2033
		public const int HDM_SETORDERARRAY = 4626;

		// Token: 0x040007F2 RID: 2034
		public const int HDM_SETHOTDIVIDER = 4627;

		// Token: 0x040007F3 RID: 2035
		public const int HDN_ITEMCHANGINGA = -300;

		// Token: 0x040007F4 RID: 2036
		public const int HDN_ITEMCHANGINGW = -320;

		// Token: 0x040007F5 RID: 2037
		public const int HDN_ITEMCHANGEDA = -301;

		// Token: 0x040007F6 RID: 2038
		public const int HDN_ITEMCHANGEDW = -321;

		// Token: 0x040007F7 RID: 2039
		public const int HDN_ITEMCLICKA = -302;

		// Token: 0x040007F8 RID: 2040
		public const int HDN_ITEMCLICKW = -322;

		// Token: 0x040007F9 RID: 2041
		public const int HDN_ITEMDBLCLICKA = -303;

		// Token: 0x040007FA RID: 2042
		public const int HDN_ITEMDBLCLICKW = -323;

		// Token: 0x040007FB RID: 2043
		public const int HDN_DIVIDERDBLCLICKA = -305;

		// Token: 0x040007FC RID: 2044
		public const int HDN_DIVIDERDBLCLICKW = -325;

		// Token: 0x040007FD RID: 2045
		public const int HDN_BEGINTRACKA = -306;

		// Token: 0x040007FE RID: 2046
		public const int HDN_BEGINTRACKW = -326;

		// Token: 0x040007FF RID: 2047
		public const int HDN_ENDTRACKA = -307;

		// Token: 0x04000800 RID: 2048
		public const int HDN_ENDTRACKW = -327;

		// Token: 0x04000801 RID: 2049
		public const int HDN_TRACKA = -308;

		// Token: 0x04000802 RID: 2050
		public const int HDN_TRACKW = -328;

		// Token: 0x04000803 RID: 2051
		public const int HDN_GETDISPINFOA = -309;

		// Token: 0x04000804 RID: 2052
		public const int HDN_GETDISPINFOW = -329;

		// Token: 0x04000805 RID: 2053
		public const int HDN_BEGINDRAG = -310;

		// Token: 0x04000806 RID: 2054
		public const int HDN_ENDDRAG = -311;

		// Token: 0x04000807 RID: 2055
		public const int HC_ACTION = 0;

		// Token: 0x04000808 RID: 2056
		public const int HIST_BACK = 0;

		// Token: 0x04000809 RID: 2057
		public const int HHT_ONHEADER = 2;

		// Token: 0x0400080A RID: 2058
		public const int HHT_ONDIVIDER = 4;

		// Token: 0x0400080B RID: 2059
		public const int HHT_ONDIVOPEN = 8;

		// Token: 0x0400080C RID: 2060
		public const int HHT_ABOVE = 256;

		// Token: 0x0400080D RID: 2061
		public const int HHT_BELOW = 512;

		// Token: 0x0400080E RID: 2062
		public const int HHT_TORIGHT = 1024;

		// Token: 0x0400080F RID: 2063
		public const int HHT_TOLEFT = 2048;

		// Token: 0x04000810 RID: 2064
		public const int HWND_TOP = 0;

		// Token: 0x04000811 RID: 2065
		public const int HWND_BOTTOM = 1;

		// Token: 0x04000812 RID: 2066
		public const int HWND_TOPMOST = -1;

		// Token: 0x04000813 RID: 2067
		public const int HWND_NOTOPMOST = -2;

		// Token: 0x04000814 RID: 2068
		public const int CWP_SKIPINVISIBLE = 1;

		// Token: 0x04000815 RID: 2069
		public const int RDW_FRAME = 1024;

		// Token: 0x04000816 RID: 2070
		public const int WM_KILLFOCUS = 8;

		// Token: 0x04000817 RID: 2071
		public const int WM_STYLECHANGED = 125;

		// Token: 0x04000818 RID: 2072
		public const int TVM_GETITEMRECT = 4356;

		// Token: 0x04000819 RID: 2073
		public const int TVM_GETCOUNT = 4357;

		// Token: 0x0400081A RID: 2074
		public const int TVM_GETINDENT = 4358;

		// Token: 0x0400081B RID: 2075
		public const int TVM_SETINDENT = 4359;

		// Token: 0x0400081C RID: 2076
		public const int TVM_GETIMAGELIST = 4360;

		// Token: 0x0400081D RID: 2077
		public const int TVSIL_NORMAL = 0;

		// Token: 0x0400081E RID: 2078
		public const int TVSIL_STATE = 2;

		// Token: 0x0400081F RID: 2079
		public const int TVM_SETIMAGELIST = 4361;

		// Token: 0x04000820 RID: 2080
		public const int TVM_GETNEXTITEM = 4362;

		// Token: 0x04000821 RID: 2081
		public const int TVGN_ROOT = 0;

		// Token: 0x04000822 RID: 2082
		public const int TVHT_ONITEMICON = 2;

		// Token: 0x04000823 RID: 2083
		public const int TVHT_ONITEMLABEL = 4;

		// Token: 0x04000824 RID: 2084
		public const int TVHT_ONITEMINDENT = 8;

		// Token: 0x04000825 RID: 2085
		public const int TVHT_ONITEMBUTTON = 16;

		// Token: 0x04000826 RID: 2086
		public const int TVHT_ONITEMRIGHT = 32;

		// Token: 0x04000827 RID: 2087
		public const int TVHT_ONITEMSTATEICON = 64;

		// Token: 0x04000828 RID: 2088
		public const int TVHT_ABOVE = 256;

		// Token: 0x04000829 RID: 2089
		public const int TVHT_BELOW = 512;

		// Token: 0x0400082A RID: 2090
		public const int TVHT_TORIGHT = 1024;

		// Token: 0x0400082B RID: 2091
		public const int TVHT_TOLEFT = 2048;

		// Token: 0x0400082C RID: 2092
		public const int GW_HWNDFIRST = 0;

		// Token: 0x0400082D RID: 2093
		public const int GW_HWNDLAST = 1;

		// Token: 0x0400082E RID: 2094
		public const int GW_HWNDNEXT = 2;

		// Token: 0x0400082F RID: 2095
		public const int GW_HWNDPREV = 3;

		// Token: 0x04000830 RID: 2096
		public const int GW_OWNER = 4;

		// Token: 0x04000831 RID: 2097
		public const int GW_CHILD = 5;

		// Token: 0x04000832 RID: 2098
		public const int GW_MAX = 5;

		// Token: 0x04000833 RID: 2099
		public const int GWL_HWNDPARENT = -8;

		// Token: 0x04000834 RID: 2100
		public const int SB_HORZ = 0;

		// Token: 0x04000835 RID: 2101
		public const int SB_VERT = 1;

		// Token: 0x04000836 RID: 2102
		public const int SB_CTL = 2;

		// Token: 0x04000837 RID: 2103
		public const int SB_BOTH = 3;

		// Token: 0x04000838 RID: 2104
		public const int SB_LINEUP = 0;

		// Token: 0x04000839 RID: 2105
		public const int SB_LINELEFT = 0;

		// Token: 0x0400083A RID: 2106
		public const int SB_LINEDOWN = 1;

		// Token: 0x0400083B RID: 2107
		public const int SB_LINERIGHT = 1;

		// Token: 0x0400083C RID: 2108
		public const int SB_PAGEUP = 2;

		// Token: 0x0400083D RID: 2109
		public const int SB_PAGELEFT = 2;

		// Token: 0x0400083E RID: 2110
		public const int SB_PAGEDOWN = 3;

		// Token: 0x0400083F RID: 2111
		public const int SB_PAGERIGHT = 3;

		// Token: 0x04000840 RID: 2112
		public const int SB_THUMBPOSITION = 4;

		// Token: 0x04000841 RID: 2113
		public const int SB_THUMBTRACK = 5;

		// Token: 0x04000842 RID: 2114
		public const int SB_TOP = 6;

		// Token: 0x04000843 RID: 2115
		public const int SB_LEFT = 6;

		// Token: 0x04000844 RID: 2116
		public const int SB_BOTTOM = 7;

		// Token: 0x04000845 RID: 2117
		public const int SB_RIGHT = 7;

		// Token: 0x04000846 RID: 2118
		public const int SB_ENDSCROLL = 8;

		// Token: 0x04000847 RID: 2119
		public const int MK_LBUTTON = 1;

		// Token: 0x04000848 RID: 2120
		public const int TVM_HITTEST = 4369;

		// Token: 0x04000849 RID: 2121
		public const int MK_RBUTTON = 2;

		// Token: 0x0400084A RID: 2122
		public const int MK_SHIFT = 4;

		// Token: 0x0400084B RID: 2123
		public const int MK_CONTROL = 8;

		// Token: 0x0400084C RID: 2124
		public const int MK_MBUTTON = 16;

		// Token: 0x0400084D RID: 2125
		public const int MK_XBUTTON1 = 32;

		// Token: 0x0400084E RID: 2126
		public const int MK_XBUTTON2 = 64;

		// Token: 0x0400084F RID: 2127
		public const int LB_ADDSTRING = 384;

		// Token: 0x04000850 RID: 2128
		public const int LB_INSERTSTRING = 385;

		// Token: 0x04000851 RID: 2129
		public const int LB_DELETESTRING = 386;

		// Token: 0x04000852 RID: 2130
		public const int LB_SELITEMRANGEEX = 387;

		// Token: 0x04000853 RID: 2131
		public const int LB_RESETCONTENT = 388;

		// Token: 0x04000854 RID: 2132
		public const int LB_SETSEL = 389;

		// Token: 0x04000855 RID: 2133
		public const int LB_SETCURSEL = 390;

		// Token: 0x04000856 RID: 2134
		public const int LB_GETSEL = 391;

		// Token: 0x04000857 RID: 2135
		public const int LB_GETCURSEL = 392;

		// Token: 0x04000858 RID: 2136
		public const int LB_GETTEXT = 393;

		// Token: 0x04000859 RID: 2137
		public const int LB_GETTEXTLEN = 394;

		// Token: 0x0400085A RID: 2138
		public const int LB_GETCOUNT = 395;

		// Token: 0x0400085B RID: 2139
		public const int LB_SELECTSTRING = 396;

		// Token: 0x0400085C RID: 2140
		public const int LB_DIR = 397;

		// Token: 0x0400085D RID: 2141
		public const int LB_GETTOPINDEX = 398;

		// Token: 0x0400085E RID: 2142
		public const int LB_FINDSTRING = 399;

		// Token: 0x0400085F RID: 2143
		public const int LB_GETSELCOUNT = 400;

		// Token: 0x04000860 RID: 2144
		public const int LB_GETSELITEMS = 401;

		// Token: 0x04000861 RID: 2145
		public const int LB_SETTABSTOPS = 402;

		// Token: 0x04000862 RID: 2146
		public const int LB_GETHORIZONTALEXTENT = 403;

		// Token: 0x04000863 RID: 2147
		public const int LB_SETHORIZONTALEXTENT = 404;

		// Token: 0x04000864 RID: 2148
		public const int LB_SETCOLUMNWIDTH = 405;

		// Token: 0x04000865 RID: 2149
		public const int LB_ADDFILE = 406;

		// Token: 0x04000866 RID: 2150
		public const int LB_SETTOPINDEX = 407;

		// Token: 0x04000867 RID: 2151
		public const int LB_GETITEMRECT = 408;

		// Token: 0x04000868 RID: 2152
		public const int LB_GETITEMDATA = 409;

		// Token: 0x04000869 RID: 2153
		public const int LB_SETITEMDATA = 410;

		// Token: 0x0400086A RID: 2154
		public const int LB_SELITEMRANGE = 411;

		// Token: 0x0400086B RID: 2155
		public const int LB_SETANCHORINDEX = 412;

		// Token: 0x0400086C RID: 2156
		public const int LB_GETANCHORINDEX = 413;

		// Token: 0x0400086D RID: 2157
		public const int LB_SETCARETINDEX = 414;

		// Token: 0x0400086E RID: 2158
		public const int LB_GETCARETINDEX = 415;

		// Token: 0x0400086F RID: 2159
		public const int LB_SETITEMHEIGHT = 416;

		// Token: 0x04000870 RID: 2160
		public const int LB_GETITEMHEIGHT = 417;

		// Token: 0x04000871 RID: 2161
		public const int LB_FINDSTRINGEXACT = 418;

		// Token: 0x04000872 RID: 2162
		public const int LB_SETLOCALE = 421;

		// Token: 0x04000873 RID: 2163
		public const int LB_GETLOCALE = 422;

		// Token: 0x04000874 RID: 2164
		public const int LB_SETCOUNT = 423;

		// Token: 0x04000875 RID: 2165
		public const int LB_INITSTORAGE = 424;

		// Token: 0x04000876 RID: 2166
		public const int LB_ITEMFROMPOINT = 425;

		// Token: 0x04000877 RID: 2167
		public const int LB_MSGMAX = 432;

		// Token: 0x04000878 RID: 2168
		public const int HTHSCROLL = 6;

		// Token: 0x04000879 RID: 2169
		public const int HTVSCROLL = 7;

		// Token: 0x0400087A RID: 2170
		public const int HTERROR = -2;

		// Token: 0x0400087B RID: 2171
		public const int HTTRANSPARENT = -1;

		// Token: 0x0400087C RID: 2172
		public const int HTNOWHERE = 0;

		// Token: 0x0400087D RID: 2173
		public const int HTCLIENT = 1;

		// Token: 0x0400087E RID: 2174
		public const int HTCAPTION = 2;

		// Token: 0x0400087F RID: 2175
		public const int HTSYSMENU = 3;

		// Token: 0x04000880 RID: 2176
		public const int HTGROWBOX = 4;

		// Token: 0x04000881 RID: 2177
		public const int HTSIZE = 4;

		// Token: 0x04000882 RID: 2178
		public const int PRF_NONCLIENT = 2;

		// Token: 0x04000883 RID: 2179
		public const int PRF_CLIENT = 4;

		// Token: 0x04000884 RID: 2180
		public const int PRF_ERASEBKGND = 8;

		// Token: 0x04000885 RID: 2181
		public const int PRF_CHILDREN = 16;

		// Token: 0x04000886 RID: 2182
		public const int SWP_NOSIZE = 1;

		// Token: 0x04000887 RID: 2183
		public const int SWP_NOMOVE = 2;

		// Token: 0x04000888 RID: 2184
		public const int SWP_NOZORDER = 4;

		// Token: 0x04000889 RID: 2185
		public const int SWP_NOREDRAW = 8;

		// Token: 0x0400088A RID: 2186
		public const int SWP_NOACTIVATE = 16;

		// Token: 0x0400088B RID: 2187
		public const int SWP_FRAMECHANGED = 32;

		// Token: 0x0400088C RID: 2188
		public const int SWP_SHOWWINDOW = 64;

		// Token: 0x0400088D RID: 2189
		public const int SWP_HIDEWINDOW = 128;

		// Token: 0x0400088E RID: 2190
		public const int SWP_NOCOPYBITS = 256;

		// Token: 0x0400088F RID: 2191
		public const int SWP_NOOWNERZORDER = 512;

		// Token: 0x04000890 RID: 2192
		public const int SWP_NOSENDCHANGING = 1024;

		// Token: 0x04000891 RID: 2193
		public const int SWP_DRAWFRAME = 32;

		// Token: 0x04000892 RID: 2194
		public const int SWP_NOREPOSITION = 512;

		// Token: 0x04000893 RID: 2195
		public const int SWP_DEFERERASE = 8192;

		// Token: 0x04000894 RID: 2196
		public const int SWP_ASYNCWINDOWPOS = 16384;

		// Token: 0x04000895 RID: 2197
		public const int WA_INACTIVE = 0;

		// Token: 0x04000896 RID: 2198
		public const int WA_ACTIVE = 1;

		// Token: 0x04000897 RID: 2199
		public const int WH_MOUSE = 7;

		// Token: 0x04000898 RID: 2200
		public const int WM_IME_STARTCOMPOSITION = 269;

		// Token: 0x04000899 RID: 2201
		public const int WM_IME_ENDCOMPOSITION = 270;

		// Token: 0x0400089A RID: 2202
		public const int WM_IME_COMPOSITION = 271;

		// Token: 0x0400089B RID: 2203
		public const int WM_ACTIVATE = 6;

		// Token: 0x0400089C RID: 2204
		public const int WM_NCMOUSEMOVE = 160;

		// Token: 0x0400089D RID: 2205
		public const int WM_NCLBUTTONDOWN = 161;

		// Token: 0x0400089E RID: 2206
		public const int WM_NCLBUTTONUP = 162;

		// Token: 0x0400089F RID: 2207
		public const int WM_NCLBUTTONDBLCLK = 163;

		// Token: 0x040008A0 RID: 2208
		public const int WM_NCRBUTTONDOWN = 164;

		// Token: 0x040008A1 RID: 2209
		public const int WM_NCRBUTTONUP = 165;

		// Token: 0x040008A2 RID: 2210
		public const int WM_NCRBUTTONDBLCLK = 166;

		// Token: 0x040008A3 RID: 2211
		public const int WM_NCMBUTTONDOWN = 167;

		// Token: 0x040008A4 RID: 2212
		public const int WM_NCMBUTTONUP = 168;

		// Token: 0x040008A5 RID: 2213
		public const int WM_NCMBUTTONDBLCLK = 169;

		// Token: 0x040008A6 RID: 2214
		public const int WM_NCXBUTTONDOWN = 171;

		// Token: 0x040008A7 RID: 2215
		public const int WM_NCXBUTTONUP = 172;

		// Token: 0x040008A8 RID: 2216
		public const int WM_NCXBUTTONDBLCLK = 173;

		// Token: 0x040008A9 RID: 2217
		public const int WM_MOUSEHOVER = 673;

		// Token: 0x040008AA RID: 2218
		public const int WM_MOUSELEAVE = 675;

		// Token: 0x040008AB RID: 2219
		public const int WM_MOUSEFIRST = 512;

		// Token: 0x040008AC RID: 2220
		public const int WM_MOUSEACTIVATE = 33;

		// Token: 0x040008AD RID: 2221
		public const int WM_LBUTTONDOWN = 513;

		// Token: 0x040008AE RID: 2222
		public const int WM_LBUTTONUP = 514;

		// Token: 0x040008AF RID: 2223
		public const int WM_LBUTTONDBLCLK = 515;

		// Token: 0x040008B0 RID: 2224
		public const int WM_RBUTTONDOWN = 516;

		// Token: 0x040008B1 RID: 2225
		public const int WM_RBUTTONUP = 517;

		// Token: 0x040008B2 RID: 2226
		public const int WM_RBUTTONDBLCLK = 518;

		// Token: 0x040008B3 RID: 2227
		public const int WM_MBUTTONDOWN = 519;

		// Token: 0x040008B4 RID: 2228
		public const int WM_MBUTTONUP = 520;

		// Token: 0x040008B5 RID: 2229
		public const int WM_MBUTTONDBLCLK = 521;

		// Token: 0x040008B6 RID: 2230
		public const int WM_NCMOUSEHOVER = 672;

		// Token: 0x040008B7 RID: 2231
		public const int WM_NCMOUSELEAVE = 674;

		// Token: 0x040008B8 RID: 2232
		public const int WM_MOUSEWHEEL = 522;

		// Token: 0x040008B9 RID: 2233
		public const int WM_MOUSELAST = 522;

		// Token: 0x040008BA RID: 2234
		public const int WM_NCHITTEST = 132;

		// Token: 0x040008BB RID: 2235
		public const int WM_SETCURSOR = 32;

		// Token: 0x040008BC RID: 2236
		public const int WM_GETOBJECT = 61;

		// Token: 0x040008BD RID: 2237
		public const int WM_CANCELMODE = 31;

		// Token: 0x040008BE RID: 2238
		public const int WM_SETFOCUS = 7;

		// Token: 0x040008BF RID: 2239
		public const int WM_KEYFIRST = 256;

		// Token: 0x040008C0 RID: 2240
		public const int WM_KEYDOWN = 256;

		// Token: 0x040008C1 RID: 2241
		public const int WM_KEYUP = 257;

		// Token: 0x040008C2 RID: 2242
		public const int WM_DEADCHAR = 259;

		// Token: 0x040008C3 RID: 2243
		public const int WM_SYSKEYDOWN = 260;

		// Token: 0x040008C4 RID: 2244
		public const int WM_SYSKEYUP = 261;

		// Token: 0x040008C5 RID: 2245
		public const int WM_SYSCHAR = 262;

		// Token: 0x040008C6 RID: 2246
		public const int WM_SYSDEADCHAR = 263;

		// Token: 0x040008C7 RID: 2247
		public const int WM_KEYLAST = 264;

		// Token: 0x040008C8 RID: 2248
		public const int WM_CONTEXTMENU = 123;

		// Token: 0x040008C9 RID: 2249
		public const int WM_PAINT = 15;

		// Token: 0x040008CA RID: 2250
		public const int WM_PRINTCLIENT = 792;

		// Token: 0x040008CB RID: 2251
		public const int WM_NCPAINT = 133;

		// Token: 0x040008CC RID: 2252
		public const int WM_SIZE = 5;

		// Token: 0x040008CD RID: 2253
		public const int WM_TIMER = 275;

		// Token: 0x040008CE RID: 2254
		public const int WM_PRINT = 791;

		// Token: 0x040008CF RID: 2255
		public const int CHILDID_SELF = 0;

		// Token: 0x040008D0 RID: 2256
		public const int OBJID_WINDOW = 0;

		// Token: 0x040008D1 RID: 2257
		public const int OBJID_CLIENT = -4;

		// Token: 0x040008D2 RID: 2258
		public const string uuid_IAccessible = "{618736E0-3C3D-11CF-810C-00AA00389B71}";

		// Token: 0x040008D3 RID: 2259
		public const string uuid_IEnumVariant = "{00020404-0000-0000-C000-000000000046}";

		// Token: 0x040008D4 RID: 2260
		public const int QS_KEY = 1;

		// Token: 0x040008D5 RID: 2261
		public const int QS_MOUSEMOVE = 2;

		// Token: 0x040008D6 RID: 2262
		public const int QS_MOUSEBUTTON = 4;

		// Token: 0x040008D7 RID: 2263
		public const int QS_POSTMESSAGE = 8;

		// Token: 0x040008D8 RID: 2264
		public const int QS_TIMER = 16;

		// Token: 0x040008D9 RID: 2265
		public const int QS_PAINT = 32;

		// Token: 0x040008DA RID: 2266
		public const int QS_SENDMESSAGE = 64;

		// Token: 0x040008DB RID: 2267
		public const int QS_HOTKEY = 128;

		// Token: 0x040008DC RID: 2268
		public const int QS_ALLPOSTMESSAGE = 256;

		// Token: 0x040008DD RID: 2269
		public const int QS_MOUSE = 6;

		// Token: 0x040008DE RID: 2270
		public const int QS_INPUT = 7;

		// Token: 0x040008DF RID: 2271
		public const int QS_ALLEVENTS = 191;

		// Token: 0x040008E0 RID: 2272
		public const int QS_ALLINPUT = 255;

		// Token: 0x040008E1 RID: 2273
		public const int MWMO_INPUTAVAILABLE = 4;

		// Token: 0x040008E2 RID: 2274
		public const int GWL_EXSTYLE = -20;

		// Token: 0x040008E3 RID: 2275
		public const int GWL_STYLE = -16;

		// Token: 0x040008E4 RID: 2276
		public const int WS_EX_LAYOUTRTL = 4194304;

		// Token: 0x040008E5 RID: 2277
		public const int SPI_GETNONCLIENTMETRICS = 41;

		// Token: 0x040008E6 RID: 2278
		public static HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);

		// Token: 0x040008E7 RID: 2279
		public static int PS_SOLID = 0;

		// Token: 0x040008E8 RID: 2280
		public static IntPtr InvalidIntPtr = (IntPtr)(-1);

		// Token: 0x040008E9 RID: 2281
		public static int TME_HOVER = 1;

		// Token: 0x040008EA RID: 2282
		public static readonly int WM_MOUSEENTER = NativeMethods.Util.RegisterWindowMessage("WinFormsMouseEnter");

		// Token: 0x040008EB RID: 2283
		public static readonly int HDN_ENDTRACK = ((Marshal.SystemDefaultCharSize == 1) ? (-307) : (-327));

		// Token: 0x02000010 RID: 16
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class TEXTMETRIC
		{
			// Token: 0x040008EC RID: 2284
			public int tmHeight;

			// Token: 0x040008ED RID: 2285
			public int tmAscent;

			// Token: 0x040008EE RID: 2286
			public int tmDescent;

			// Token: 0x040008EF RID: 2287
			public int tmInternalLeading;

			// Token: 0x040008F0 RID: 2288
			public int tmExternalLeading;

			// Token: 0x040008F1 RID: 2289
			public int tmAveCharWidth;

			// Token: 0x040008F2 RID: 2290
			public int tmMaxCharWidth;

			// Token: 0x040008F3 RID: 2291
			public int tmWeight;

			// Token: 0x040008F4 RID: 2292
			public int tmOverhang;

			// Token: 0x040008F5 RID: 2293
			public int tmDigitizedAspectX;

			// Token: 0x040008F6 RID: 2294
			public int tmDigitizedAspectY;

			// Token: 0x040008F7 RID: 2295
			public char tmFirstChar;

			// Token: 0x040008F8 RID: 2296
			public char tmLastChar;

			// Token: 0x040008F9 RID: 2297
			public char tmDefaultChar;

			// Token: 0x040008FA RID: 2298
			public char tmBreakChar;

			// Token: 0x040008FB RID: 2299
			public byte tmItalic;

			// Token: 0x040008FC RID: 2300
			public byte tmUnderlined;

			// Token: 0x040008FD RID: 2301
			public byte tmStruckOut;

			// Token: 0x040008FE RID: 2302
			public byte tmPitchAndFamily;

			// Token: 0x040008FF RID: 2303
			public byte tmCharSet;
		}

		// Token: 0x02000011 RID: 17
		// (Invoke) Token: 0x06000052 RID: 82
		public delegate bool EnumChildrenCallback(IntPtr hwnd, IntPtr lParam);

		// Token: 0x02000012 RID: 18
		[StructLayout(LayoutKind.Sequential)]
		public class NMHEADER
		{
			// Token: 0x04000900 RID: 2304
			public int hwndFrom;

			// Token: 0x04000901 RID: 2305
			public int idFrom;

			// Token: 0x04000902 RID: 2306
			public int code;

			// Token: 0x04000903 RID: 2307
			public int iItem;

			// Token: 0x04000904 RID: 2308
			public int iButton;

			// Token: 0x04000905 RID: 2309
			public int pItem;
		}

		// Token: 0x02000013 RID: 19
		[StructLayout(LayoutKind.Sequential)]
		public class POINT
		{
			// Token: 0x06000056 RID: 86 RVA: 0x0000293C File Offset: 0x0000193C
			public POINT()
			{
			}

			// Token: 0x06000057 RID: 87 RVA: 0x00002944 File Offset: 0x00001944
			public POINT(int x, int y)
			{
				this.x = x;
				this.y = y;
			}

			// Token: 0x04000906 RID: 2310
			public int x;

			// Token: 0x04000907 RID: 2311
			public int y;
		}

		// Token: 0x02000014 RID: 20
		public struct WINDOWPOS
		{
			// Token: 0x04000908 RID: 2312
			public IntPtr hwnd;

			// Token: 0x04000909 RID: 2313
			public IntPtr hwndInsertAfter;

			// Token: 0x0400090A RID: 2314
			public int x;

			// Token: 0x0400090B RID: 2315
			public int y;

			// Token: 0x0400090C RID: 2316
			public int cx;

			// Token: 0x0400090D RID: 2317
			public int cy;

			// Token: 0x0400090E RID: 2318
			public int flags;
		}

		// Token: 0x02000015 RID: 21
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
		public class TV_ITEM
		{
			// Token: 0x0400090F RID: 2319
			public int mask;

			// Token: 0x04000910 RID: 2320
			public int hItem;

			// Token: 0x04000911 RID: 2321
			public int state;

			// Token: 0x04000912 RID: 2322
			public int stateMask;

			// Token: 0x04000913 RID: 2323
			public int pszText;

			// Token: 0x04000914 RID: 2324
			public int cchTextMax;

			// Token: 0x04000915 RID: 2325
			public int iImage;

			// Token: 0x04000916 RID: 2326
			public int iSelectedImage;

			// Token: 0x04000917 RID: 2327
			public int cChildren;

			// Token: 0x04000918 RID: 2328
			public int lParam;
		}

		// Token: 0x02000016 RID: 22
		[StructLayout(LayoutKind.Sequential)]
		public class NMHDR
		{
			// Token: 0x04000919 RID: 2329
			public int hwndFrom;

			// Token: 0x0400091A RID: 2330
			public int idFrom;

			// Token: 0x0400091B RID: 2331
			public int code;
		}

		// Token: 0x02000017 RID: 23
		[StructLayout(LayoutKind.Sequential)]
		public class NMTREEVIEW
		{
			// Token: 0x0400091C RID: 2332
			public NativeMethods.NMHDR nmhdr;

			// Token: 0x0400091D RID: 2333
			public int action;

			// Token: 0x0400091E RID: 2334
			public NativeMethods.TV_ITEM itemOld;

			// Token: 0x0400091F RID: 2335
			public NativeMethods.TV_ITEM itemNew;

			// Token: 0x04000920 RID: 2336
			public NativeMethods.POINT ptDrag;
		}

		// Token: 0x02000018 RID: 24
		[StructLayout(LayoutKind.Sequential)]
		public class TCHITTESTINFO
		{
			// Token: 0x04000921 RID: 2337
			public Point pt;

			// Token: 0x04000922 RID: 2338
			public NativeMethods.TabControlHitTest flags;
		}

		// Token: 0x02000019 RID: 25
		[Flags]
		public enum TabControlHitTest
		{
			// Token: 0x04000924 RID: 2340
			TCHT_NOWHERE = 1,
			// Token: 0x04000925 RID: 2341
			TCHT_ONITEMICON = 2,
			// Token: 0x04000926 RID: 2342
			TCHT_ONITEMLABEL = 4
		}

		// Token: 0x0200001A RID: 26
		[StructLayout(LayoutKind.Sequential)]
		public class TRACKMOUSEEVENT
		{
			// Token: 0x04000927 RID: 2343
			public int cbSize = Marshal.SizeOf(typeof(NativeMethods.TRACKMOUSEEVENT));

			// Token: 0x04000928 RID: 2344
			public int dwFlags;

			// Token: 0x04000929 RID: 2345
			public IntPtr hwndTrack;

			// Token: 0x0400092A RID: 2346
			public int dwHoverTime;
		}

		// Token: 0x0200001B RID: 27
		[ComVisible(false)]
		public enum StructFormat
		{
			// Token: 0x0400092C RID: 2348
			Ansi = 1,
			// Token: 0x0400092D RID: 2349
			Unicode,
			// Token: 0x0400092E RID: 2350
			Auto
		}

		// Token: 0x0200001C RID: 28
		public struct MOUSEHOOKSTRUCT
		{
			// Token: 0x0400092F RID: 2351
			public int pt_x;

			// Token: 0x04000930 RID: 2352
			public int pt_y;

			// Token: 0x04000931 RID: 2353
			public IntPtr hWnd;

			// Token: 0x04000932 RID: 2354
			public int wHitTestCode;

			// Token: 0x04000933 RID: 2355
			public int dwExtraInfo;
		}

		// Token: 0x0200001D RID: 29
		public struct MSG
		{
			// Token: 0x04000934 RID: 2356
			public IntPtr hwnd;

			// Token: 0x04000935 RID: 2357
			public int message;

			// Token: 0x04000936 RID: 2358
			public IntPtr wParam;

			// Token: 0x04000937 RID: 2359
			public IntPtr lParam;

			// Token: 0x04000938 RID: 2360
			public int time;

			// Token: 0x04000939 RID: 2361
			public int pt_x;

			// Token: 0x0400093A RID: 2362
			public int pt_y;
		}

		// Token: 0x0200001E RID: 30
		[StructLayout(LayoutKind.Sequential)]
		public class COMRECT
		{
			// Token: 0x0600005D RID: 93 RVA: 0x00002997 File Offset: 0x00001997
			public COMRECT()
			{
			}

			// Token: 0x0600005E RID: 94 RVA: 0x0000299F File Offset: 0x0000199F
			public COMRECT(int left, int top, int right, int bottom)
			{
				this.left = left;
				this.top = top;
				this.right = right;
				this.bottom = bottom;
			}

			// Token: 0x0400093B RID: 2363
			public int left;

			// Token: 0x0400093C RID: 2364
			public int top;

			// Token: 0x0400093D RID: 2365
			public int right;

			// Token: 0x0400093E RID: 2366
			public int bottom;
		}

		// Token: 0x0200001F RID: 31
		[StructLayout(LayoutKind.Sequential)]
		public sealed class FORMATETC
		{
			// Token: 0x0400093F RID: 2367
			[MarshalAs(UnmanagedType.I4)]
			public int cfFormat;

			// Token: 0x04000940 RID: 2368
			[MarshalAs(UnmanagedType.I4)]
			public IntPtr ptd = IntPtr.Zero;

			// Token: 0x04000941 RID: 2369
			[MarshalAs(UnmanagedType.I4)]
			public int dwAspect;

			// Token: 0x04000942 RID: 2370
			[MarshalAs(UnmanagedType.I4)]
			public int lindex;

			// Token: 0x04000943 RID: 2371
			[MarshalAs(UnmanagedType.I4)]
			public int tymed;
		}

		// Token: 0x02000020 RID: 32
		[StructLayout(LayoutKind.Sequential)]
		public class STGMEDIUM
		{
			// Token: 0x04000944 RID: 2372
			[MarshalAs(UnmanagedType.I4)]
			public int tymed;

			// Token: 0x04000945 RID: 2373
			public IntPtr unionmember = IntPtr.Zero;

			// Token: 0x04000946 RID: 2374
			public IntPtr pUnkForRelease = IntPtr.Zero;
		}

		// Token: 0x02000021 RID: 33
		public struct RECT
		{
			// Token: 0x06000061 RID: 97 RVA: 0x000029F5 File Offset: 0x000019F5
			public RECT(int left, int top, int right, int bottom)
			{
				this.left = left;
				this.top = top;
				this.right = right;
				this.bottom = bottom;
			}

			// Token: 0x04000947 RID: 2375
			public int left;

			// Token: 0x04000948 RID: 2376
			public int top;

			// Token: 0x04000949 RID: 2377
			public int right;

			// Token: 0x0400094A RID: 2378
			public int bottom;
		}

		// Token: 0x02000022 RID: 34
		[StructLayout(LayoutKind.Sequential)]
		public sealed class OLECMD
		{
			// Token: 0x0400094B RID: 2379
			[MarshalAs(UnmanagedType.U4)]
			public int cmdID;

			// Token: 0x0400094C RID: 2380
			[MarshalAs(UnmanagedType.U4)]
			public int cmdf;
		}

		// Token: 0x02000023 RID: 35
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagOIFI
		{
			// Token: 0x0400094D RID: 2381
			[MarshalAs(UnmanagedType.U4)]
			public int cb;

			// Token: 0x0400094E RID: 2382
			[MarshalAs(UnmanagedType.I4)]
			public int fMDIApp;

			// Token: 0x0400094F RID: 2383
			public IntPtr hwndFrame;

			// Token: 0x04000950 RID: 2384
			public IntPtr hAccel;

			// Token: 0x04000951 RID: 2385
			[MarshalAs(UnmanagedType.U4)]
			public int cAccelEntries;
		}

		// Token: 0x02000024 RID: 36
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagSIZE
		{
			// Token: 0x04000952 RID: 2386
			[MarshalAs(UnmanagedType.I4)]
			public int cx;

			// Token: 0x04000953 RID: 2387
			[MarshalAs(UnmanagedType.I4)]
			public int cy;
		}

		// Token: 0x02000025 RID: 37
		[ComVisible(true)]
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagSIZEL
		{
			// Token: 0x04000954 RID: 2388
			[MarshalAs(UnmanagedType.I4)]
			public int cx;

			// Token: 0x04000955 RID: 2389
			[MarshalAs(UnmanagedType.I4)]
			public int cy;
		}

		// Token: 0x02000026 RID: 38
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagLOGPALETTE
		{
			// Token: 0x04000956 RID: 2390
			[MarshalAs(UnmanagedType.U2)]
			public short palVersion;

			// Token: 0x04000957 RID: 2391
			[MarshalAs(UnmanagedType.U2)]
			public short palNumEntries;
		}

		// Token: 0x02000027 RID: 39
		public class DOCHOSTUIDBLCLICK
		{
			// Token: 0x04000958 RID: 2392
			public const int DEFAULT = 0;

			// Token: 0x04000959 RID: 2393
			public const int SHOWPROPERTIES = 1;

			// Token: 0x0400095A RID: 2394
			public const int SHOWCODE = 2;
		}

		// Token: 0x02000028 RID: 40
		public class DOCHOSTUIFLAG
		{
			// Token: 0x0400095B RID: 2395
			public const int DIALOG = 1;

			// Token: 0x0400095C RID: 2396
			public const int DISABLE_HELP_MENU = 2;

			// Token: 0x0400095D RID: 2397
			public const int NO3DBORDER = 4;

			// Token: 0x0400095E RID: 2398
			public const int SCROLL_NO = 8;

			// Token: 0x0400095F RID: 2399
			public const int DISABLE_SCRIPT_INACTIVE = 16;

			// Token: 0x04000960 RID: 2400
			public const int OPENNEWWIN = 32;

			// Token: 0x04000961 RID: 2401
			public const int DISABLE_OFFSCREEN = 64;

			// Token: 0x04000962 RID: 2402
			public const int FLAT_SCROLLBAR = 128;

			// Token: 0x04000963 RID: 2403
			public const int DIV_BLOCKDEFAULT = 256;

			// Token: 0x04000964 RID: 2404
			public const int ACTIVATE_CLIENTHIT_ONLY = 512;

			// Token: 0x04000965 RID: 2405
			public const int DISABLE_COOKIE = 1024;
		}

		// Token: 0x02000029 RID: 41
		[ComVisible(true)]
		[StructLayout(LayoutKind.Sequential)]
		public class DOCHOSTUIINFO
		{
			// Token: 0x04000966 RID: 2406
			[MarshalAs(UnmanagedType.U4)]
			public int cbSize;

			// Token: 0x04000967 RID: 2407
			[MarshalAs(UnmanagedType.I4)]
			public int dwFlags;

			// Token: 0x04000968 RID: 2408
			[MarshalAs(UnmanagedType.I4)]
			public int dwDoubleClick;

			// Token: 0x04000969 RID: 2409
			[MarshalAs(UnmanagedType.I4)]
			public int dwReserved1;

			// Token: 0x0400096A RID: 2410
			[MarshalAs(UnmanagedType.I4)]
			public int dwReserved2;
		}

		// Token: 0x0200002A RID: 42
		[ComVisible(true)]
		[Guid("BD3F23C0-D43E-11CF-893B-00AA00BDCE1A")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IDocHostUIHandler
		{
			// Token: 0x0600006A RID: 106
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int ShowContextMenu([MarshalAs(UnmanagedType.U4)] [In] int dwID, [In] NativeMethods.POINT pt, [MarshalAs(UnmanagedType.Interface)] [In] object pcmdtReserved, [MarshalAs(UnmanagedType.Interface)] [In] object pdispReserved);

			// Token: 0x0600006B RID: 107
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int GetHostInfo([In] [Out] NativeMethods.DOCHOSTUIINFO info);

			// Token: 0x0600006C RID: 108
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int ShowUI([MarshalAs(UnmanagedType.I4)] [In] int dwID, [In] NativeMethods.IOleInPlaceActiveObject activeObject, [In] NativeMethods.IOleCommandTarget commandTarget, [In] NativeMethods.IOleInPlaceFrame frame, [In] NativeMethods.IOleInPlaceUIWindow doc);

			// Token: 0x0600006D RID: 109
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int HideUI();

			// Token: 0x0600006E RID: 110
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int UpdateUI();

			// Token: 0x0600006F RID: 111
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int EnableModeless([MarshalAs(UnmanagedType.Bool)] [In] bool fEnable);

			// Token: 0x06000070 RID: 112
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int OnDocWindowActivate([MarshalAs(UnmanagedType.Bool)] [In] bool fActivate);

			// Token: 0x06000071 RID: 113
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int OnFrameWindowActivate([MarshalAs(UnmanagedType.Bool)] [In] bool fActivate);

			// Token: 0x06000072 RID: 114
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int ResizeBorder([In] NativeMethods.COMRECT rect, [In] NativeMethods.IOleInPlaceUIWindow doc, bool fFrameWindow);

			// Token: 0x06000073 RID: 115
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int TranslateAccelerator([In] ref NativeMethods.MSG msg, [In] ref Guid group, [MarshalAs(UnmanagedType.I4)] [In] int nCmdID);

			// Token: 0x06000074 RID: 116
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int GetOptionKeyPath([MarshalAs(UnmanagedType.LPArray)] [Out] string[] pbstrKey, [MarshalAs(UnmanagedType.U4)] [In] int dw);

			// Token: 0x06000075 RID: 117
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int GetDropTarget([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IOleDropTarget pDropTarget, [MarshalAs(UnmanagedType.Interface)] out NativeMethods.IOleDropTarget ppDropTarget);

			// Token: 0x06000076 RID: 118
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int GetExternal([MarshalAs(UnmanagedType.Interface)] out object ppDispatch);

			// Token: 0x06000077 RID: 119
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int TranslateUrl([MarshalAs(UnmanagedType.U4)] [In] int dwTranslate, [MarshalAs(UnmanagedType.LPWStr)] [In] string strURLIn, [MarshalAs(UnmanagedType.LPWStr)] out string pstrURLOut);

			// Token: 0x06000078 RID: 120
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int FilterDataObject(IDataObject pDO, out IDataObject ppDORet);
		}

		// Token: 0x0200002B RID: 43
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComVisible(true)]
		[Guid("00000122-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IOleDropTarget
		{
			// Token: 0x06000079 RID: 121
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int OleDragEnter(IDataObject pDataObj, [MarshalAs(UnmanagedType.U4)] [In] int grfKeyState, [MarshalAs(UnmanagedType.U8)] [In] long pt, [MarshalAs(UnmanagedType.I4)] [In] [Out] ref int pdwEffect);

			// Token: 0x0600007A RID: 122
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int OleDragOver([MarshalAs(UnmanagedType.U4)] [In] int grfKeyState, [MarshalAs(UnmanagedType.U8)] [In] long pt, [MarshalAs(UnmanagedType.I4)] [In] [Out] ref int pdwEffect);

			// Token: 0x0600007B RID: 123
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int OleDragLeave();

			// Token: 0x0600007C RID: 124
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int OleDrop(IDataObject pDataObj, [MarshalAs(UnmanagedType.U4)] [In] int grfKeyState, [MarshalAs(UnmanagedType.U8)] [In] long pt, [MarshalAs(UnmanagedType.I4)] [In] [Out] ref int pdwEffect);
		}

		// Token: 0x0200002C RID: 44
		[Guid("B722BCCB-4E68-101B-A2BC-00AA00404770")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComVisible(true)]
		[ComImport]
		public interface IOleCommandTarget
		{
			// Token: 0x0600007D RID: 125
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int QueryStatus(ref Guid pguidCmdGroup, int cCmds, [In] [Out] NativeMethods.OLECMD prgCmds, [In] [Out] string pCmdText);

			// Token: 0x0600007E RID: 126
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int Exec(ref Guid pguidCmdGroup, int nCmdID, int nCmdexecopt, [MarshalAs(UnmanagedType.LPArray)] [In] object[] pvaIn, IntPtr pvaOut);
		}

		// Token: 0x0200002D RID: 45
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComVisible(true)]
		[Guid("00000116-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IOleInPlaceFrame
		{
			// Token: 0x0600007F RID: 127
			IntPtr GetWindow();

			// Token: 0x06000080 RID: 128
			void ContextSensitiveHelp([MarshalAs(UnmanagedType.I4)] [In] int fEnterMode);

			// Token: 0x06000081 RID: 129
			void GetBorder([Out] NativeMethods.COMRECT lprectBorder);

			// Token: 0x06000082 RID: 130
			void RequestBorderSpace([In] NativeMethods.COMRECT pborderwidths);

			// Token: 0x06000083 RID: 131
			void SetBorderSpace([In] NativeMethods.COMRECT pborderwidths);

			// Token: 0x06000084 RID: 132
			void SetActiveObject([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IOleInPlaceActiveObject pActiveObject, [MarshalAs(UnmanagedType.LPWStr)] [In] string pszObjName);

			// Token: 0x06000085 RID: 133
			void InsertMenus([In] IntPtr hmenuShared, [In] [Out] object lpMenuWidths);

			// Token: 0x06000086 RID: 134
			void SetMenu([In] IntPtr hmenuShared, [In] IntPtr holemenu, [In] IntPtr hwndActiveObject);

			// Token: 0x06000087 RID: 135
			void RemoveMenus([In] IntPtr hmenuShared);

			// Token: 0x06000088 RID: 136
			void SetStatusText([MarshalAs(UnmanagedType.BStr)] [In] string pszStatusText);

			// Token: 0x06000089 RID: 137
			void EnableModeless([MarshalAs(UnmanagedType.I4)] [In] int fEnable);

			// Token: 0x0600008A RID: 138
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int TranslateAccelerator([In] ref NativeMethods.MSG lpmsg, [MarshalAs(UnmanagedType.U2)] [In] short wID);
		}

		// Token: 0x0200002E RID: 46
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComVisible(true)]
		[Guid("00000115-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IOleInPlaceUIWindow
		{
			// Token: 0x0600008B RID: 139
			IntPtr GetWindow();

			// Token: 0x0600008C RID: 140
			void ContextSensitiveHelp([MarshalAs(UnmanagedType.I4)] [In] int fEnterMode);

			// Token: 0x0600008D RID: 141
			void GetBorder([Out] NativeMethods.COMRECT lprectBorder);

			// Token: 0x0600008E RID: 142
			void RequestBorderSpace([In] NativeMethods.COMRECT pborderwidths);

			// Token: 0x0600008F RID: 143
			void SetBorderSpace([In] NativeMethods.COMRECT pborderwidths);

			// Token: 0x06000090 RID: 144
			void SetActiveObject([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IOleInPlaceActiveObject pActiveObject, [MarshalAs(UnmanagedType.LPWStr)] [In] string pszObjName);
		}

		// Token: 0x0200002F RID: 47
		[Guid("00000117-0000-0000-C000-000000000046")]
		[ComVisible(true)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleInPlaceActiveObject
		{
			// Token: 0x06000091 RID: 145
			int GetWindow(out IntPtr hwnd);

			// Token: 0x06000092 RID: 146
			void ContextSensitiveHelp([MarshalAs(UnmanagedType.I4)] [In] int fEnterMode);

			// Token: 0x06000093 RID: 147
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int TranslateAccelerator([In] ref NativeMethods.MSG lpmsg);

			// Token: 0x06000094 RID: 148
			void OnFrameWindowActivate([MarshalAs(UnmanagedType.I4)] [In] int fActivate);

			// Token: 0x06000095 RID: 149
			void OnDocWindowActivate([MarshalAs(UnmanagedType.I4)] [In] int fActivate);

			// Token: 0x06000096 RID: 150
			void ResizeBorder([In] NativeMethods.COMRECT prcBorder, [In] NativeMethods.IOleInPlaceUIWindow pUIWindow, [MarshalAs(UnmanagedType.I4)] [In] int fFrameWindow);

			// Token: 0x06000097 RID: 151
			void EnableModeless([MarshalAs(UnmanagedType.I4)] [In] int fEnable);
		}

		// Token: 0x02000030 RID: 48
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComVisible(true)]
		[Guid("0000011B-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IOleContainer
		{
			// Token: 0x06000098 RID: 152
			void ParseDisplayName([MarshalAs(UnmanagedType.Interface)] [In] object pbc, [MarshalAs(UnmanagedType.BStr)] [In] string pszDisplayName, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pchEaten, [MarshalAs(UnmanagedType.LPArray)] [Out] object[] ppmkOut);

			// Token: 0x06000099 RID: 153
			void EnumObjects([MarshalAs(UnmanagedType.U4)] [In] int grfFlags, [MarshalAs(UnmanagedType.Interface)] out object ppenum);

			// Token: 0x0600009A RID: 154
			void LockContainer([MarshalAs(UnmanagedType.I4)] [In] int fLock);
		}

		// Token: 0x02000031 RID: 49
		[Guid("00000118-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComVisible(true)]
		[ComImport]
		public interface IOleClientSite
		{
			// Token: 0x0600009B RID: 155
			void SaveObject();

			// Token: 0x0600009C RID: 156
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetMoniker([MarshalAs(UnmanagedType.U4)] [In] int dwAssign, [MarshalAs(UnmanagedType.U4)] [In] int dwWhichMoniker);

			// Token: 0x0600009D RID: 157
			[PreserveSig]
			int GetContainer(out NativeMethods.IOleContainer ppContainer);

			// Token: 0x0600009E RID: 158
			void ShowObject();

			// Token: 0x0600009F RID: 159
			void OnShowWindow([MarshalAs(UnmanagedType.I4)] [In] int fShow);

			// Token: 0x060000A0 RID: 160
			void RequestNewObjectLayout();
		}

		// Token: 0x02000032 RID: 50
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComVisible(true)]
		[Guid("B722BCC7-4E68-101B-A2BC-00AA00404770")]
		[ComImport]
		public interface IOleDocumentSite
		{
			// Token: 0x060000A1 RID: 161
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int ActivateMe([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IOleDocumentView pViewToActivate);
		}

		// Token: 0x02000033 RID: 51
		[Guid("B722BCC6-4E68-101B-A2BC-00AA00404770")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComVisible(true)]
		[ComImport]
		public interface IOleDocumentView
		{
			// Token: 0x060000A2 RID: 162
			void SetInPlaceSite([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IOleInPlaceSite pIPSite);

			// Token: 0x060000A3 RID: 163
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IOleInPlaceSite GetInPlaceSite();

			// Token: 0x060000A4 RID: 164
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetDocument();

			// Token: 0x060000A5 RID: 165
			void SetRect([In] NativeMethods.COMRECT prcView);

			// Token: 0x060000A6 RID: 166
			void GetRect([Out] NativeMethods.COMRECT prcView);

			// Token: 0x060000A7 RID: 167
			void SetRectComplex([In] NativeMethods.COMRECT prcView, [In] NativeMethods.COMRECT prcHScroll, [In] NativeMethods.COMRECT prcVScroll, [In] NativeMethods.COMRECT prcSizeBox);

			// Token: 0x060000A8 RID: 168
			void Show([MarshalAs(UnmanagedType.I4)] [In] int fShow);

			// Token: 0x060000A9 RID: 169
			void UIActivate([MarshalAs(UnmanagedType.I4)] [In] int fUIActivate);

			// Token: 0x060000AA RID: 170
			void Open();

			// Token: 0x060000AB RID: 171
			void CloseView([MarshalAs(UnmanagedType.U4)] [In] int dwReserved);

			// Token: 0x060000AC RID: 172
			void SaveViewState([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IStream pstm);

			// Token: 0x060000AD RID: 173
			void ApplyViewState([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IStream pstm);

			// Token: 0x060000AE RID: 174
			void Clone([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IOleInPlaceSite pIPSiteNew, [MarshalAs(UnmanagedType.LPArray)] [Out] NativeMethods.IOleDocumentView[] ppViewNew);
		}

		// Token: 0x02000034 RID: 52
		[ComVisible(true)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("00000119-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IOleInPlaceSite
		{
			// Token: 0x060000AF RID: 175
			IntPtr GetWindow();

			// Token: 0x060000B0 RID: 176
			void ContextSensitiveHelp([MarshalAs(UnmanagedType.I4)] [In] int fEnterMode);

			// Token: 0x060000B1 RID: 177
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int CanInPlaceActivate();

			// Token: 0x060000B2 RID: 178
			void OnInPlaceActivate();

			// Token: 0x060000B3 RID: 179
			void OnUIActivate();

			// Token: 0x060000B4 RID: 180
			void GetWindowContext(out NativeMethods.IOleInPlaceFrame ppFrame, out NativeMethods.IOleInPlaceUIWindow ppDoc, [Out] NativeMethods.COMRECT lprcPosRect, [Out] NativeMethods.COMRECT lprcClipRect, [In] [Out] NativeMethods.tagOIFI lpFrameInfo);

			// Token: 0x060000B5 RID: 181
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int Scroll([MarshalAs(UnmanagedType.U4)] [In] NativeMethods.tagSIZE scrollExtant);

			// Token: 0x060000B6 RID: 182
			void OnUIDeactivate([MarshalAs(UnmanagedType.I4)] [In] int fUndoable);

			// Token: 0x060000B7 RID: 183
			void OnInPlaceDeactivate();

			// Token: 0x060000B8 RID: 184
			void DiscardUndoState();

			// Token: 0x060000B9 RID: 185
			void DeactivateAndUndo();

			// Token: 0x060000BA RID: 186
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int OnPosRectChange([In] NativeMethods.COMRECT lprcPosRect);
		}

		// Token: 0x02000035 RID: 53
		[ComVisible(true)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("0000000C-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IStream
		{
			// Token: 0x060000BB RID: 187
			[return: MarshalAs(UnmanagedType.I4)]
			int Read([In] IntPtr buf, [MarshalAs(UnmanagedType.I4)] [In] int len);

			// Token: 0x060000BC RID: 188
			[return: MarshalAs(UnmanagedType.I4)]
			int Write([In] IntPtr buf, [MarshalAs(UnmanagedType.I4)] [In] int len);

			// Token: 0x060000BD RID: 189
			[return: MarshalAs(UnmanagedType.I8)]
			long Seek([MarshalAs(UnmanagedType.I8)] [In] long dlibMove, [MarshalAs(UnmanagedType.I4)] [In] int dwOrigin);

			// Token: 0x060000BE RID: 190
			void SetSize([MarshalAs(UnmanagedType.I8)] [In] long libNewSize);

			// Token: 0x060000BF RID: 191
			[return: MarshalAs(UnmanagedType.I8)]
			long CopyTo([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IStream pstm, [MarshalAs(UnmanagedType.I8)] [In] long cb, [MarshalAs(UnmanagedType.LPArray)] [Out] long[] pcbRead);

			// Token: 0x060000C0 RID: 192
			void Commit([MarshalAs(UnmanagedType.I4)] [In] int grfCommitFlags);

			// Token: 0x060000C1 RID: 193
			void Revert();

			// Token: 0x060000C2 RID: 194
			void LockRegion([MarshalAs(UnmanagedType.I8)] [In] long libOffset, [MarshalAs(UnmanagedType.I8)] [In] long cb, [MarshalAs(UnmanagedType.I4)] [In] int dwLockType);

			// Token: 0x060000C3 RID: 195
			void UnlockRegion([MarshalAs(UnmanagedType.I8)] [In] long libOffset, [MarshalAs(UnmanagedType.I8)] [In] long cb, [MarshalAs(UnmanagedType.I4)] [In] int dwLockType);

			// Token: 0x060000C4 RID: 196
			void Stat([In] IntPtr pStatstg, [MarshalAs(UnmanagedType.I4)] [In] int grfStatFlag);

			// Token: 0x060000C5 RID: 197
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IStream Clone();
		}

		// Token: 0x02000036 RID: 54
		[ComVisible(true)]
		[Guid("00000112-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleObject
		{
			// Token: 0x060000C6 RID: 198
			[PreserveSig]
			int SetClientSite([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IOleClientSite pClientSite);

			// Token: 0x060000C7 RID: 199
			[PreserveSig]
			int GetClientSite(out NativeMethods.IOleClientSite site);

			// Token: 0x060000C8 RID: 200
			[PreserveSig]
			int SetHostNames([MarshalAs(UnmanagedType.LPWStr)] [In] string szContainerApp, [MarshalAs(UnmanagedType.LPWStr)] [In] string szContainerObj);

			// Token: 0x060000C9 RID: 201
			[PreserveSig]
			int Close([MarshalAs(UnmanagedType.I4)] [In] int dwSaveOption);

			// Token: 0x060000CA RID: 202
			[PreserveSig]
			int SetMoniker([MarshalAs(UnmanagedType.U4)] [In] int dwWhichMoniker, [MarshalAs(UnmanagedType.Interface)] [In] object pmk);

			// Token: 0x060000CB RID: 203
			[PreserveSig]
			int GetMoniker([MarshalAs(UnmanagedType.U4)] [In] int dwAssign, [MarshalAs(UnmanagedType.U4)] [In] int dwWhichMoniker, out object moniker);

			// Token: 0x060000CC RID: 204
			[PreserveSig]
			int InitFromData(IDataObject pDataObject, [MarshalAs(UnmanagedType.I4)] [In] int fCreation, [MarshalAs(UnmanagedType.U4)] [In] int dwReserved);

			// Token: 0x060000CD RID: 205
			[PreserveSig]
			int GetClipboardData([MarshalAs(UnmanagedType.U4)] [In] int dwReserved, out IDataObject data);

			// Token: 0x060000CE RID: 206
			[PreserveSig]
			int DoVerb([MarshalAs(UnmanagedType.I4)] [In] int iVerb, [In] IntPtr lpmsg, [MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IOleClientSite pActiveSite, [MarshalAs(UnmanagedType.I4)] [In] int lindex, [In] IntPtr hwndParent, [In] NativeMethods.COMRECT lprcPosRect);

			// Token: 0x060000CF RID: 207
			[PreserveSig]
			int EnumVerbs(out NativeMethods.IEnumOLEVERB e);

			// Token: 0x060000D0 RID: 208
			[PreserveSig]
			int OleUpdate();

			// Token: 0x060000D1 RID: 209
			[PreserveSig]
			int IsUpToDate();

			// Token: 0x060000D2 RID: 210
			[PreserveSig]
			int GetUserClassID([In] [Out] ref Guid pClsid);

			// Token: 0x060000D3 RID: 211
			[PreserveSig]
			int GetUserType([MarshalAs(UnmanagedType.U4)] [In] int dwFormOfType, [MarshalAs(UnmanagedType.LPWStr)] out string userType);

			// Token: 0x060000D4 RID: 212
			[PreserveSig]
			int SetExtent([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, [In] NativeMethods.tagSIZEL pSizel);

			// Token: 0x060000D5 RID: 213
			[PreserveSig]
			int GetExtent([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, [Out] NativeMethods.tagSIZEL pSizel);

			// Token: 0x060000D6 RID: 214
			[PreserveSig]
			int Advise([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IAdviseSink pAdvSink, out int cookie);

			// Token: 0x060000D7 RID: 215
			[PreserveSig]
			int Unadvise([MarshalAs(UnmanagedType.U4)] [In] int dwConnection);

			// Token: 0x060000D8 RID: 216
			[PreserveSig]
			int EnumAdvise(out object e);

			// Token: 0x060000D9 RID: 217
			[PreserveSig]
			int GetMiscStatus([MarshalAs(UnmanagedType.U4)] [In] int dwAspect, out int misc);

			// Token: 0x060000DA RID: 218
			[PreserveSig]
			int SetColorScheme([In] NativeMethods.tagLOGPALETTE pLogpal);
		}

		// Token: 0x02000037 RID: 55
		[ComVisible(true)]
		[Guid("0000010F-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IAdviseSink
		{
			// Token: 0x060000DB RID: 219
			void OnDataChange([In] NativeMethods.FORMATETC pFormatetc, [In] NativeMethods.STGMEDIUM pStgmed);

			// Token: 0x060000DC RID: 220
			void OnViewChange([MarshalAs(UnmanagedType.U4)] [In] int dwAspect, [MarshalAs(UnmanagedType.I4)] [In] int lindex);

			// Token: 0x060000DD RID: 221
			void OnRename([MarshalAs(UnmanagedType.Interface)] [In] object pmk);

			// Token: 0x060000DE RID: 222
			void OnSave();

			// Token: 0x060000DF RID: 223
			void OnClose();
		}

		// Token: 0x02000038 RID: 56
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComVisible(true)]
		[Guid("7FD52380-4E07-101B-AE2D-08002B2EC713")]
		[ComImport]
		public interface IPersistStreamInit
		{
			// Token: 0x060000E0 RID: 224
			void GetClassID([In] [Out] ref Guid pClassID);

			// Token: 0x060000E1 RID: 225
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int IsDirty();

			// Token: 0x060000E2 RID: 226
			void Load([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IStream pstm);

			// Token: 0x060000E3 RID: 227
			void Save([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IStream pstm, [MarshalAs(UnmanagedType.Bool)] [In] bool fClearDirty);

			// Token: 0x060000E4 RID: 228
			void GetSizeMax([MarshalAs(UnmanagedType.LPArray)] [Out] long pcbSize);

			// Token: 0x060000E5 RID: 229
			void InitNew();
		}

		// Token: 0x02000039 RID: 57
		[ComVisible(true)]
		[Guid("25336920-03F9-11CF-8FD0-00AA00686F13")]
		[ComImport]
		public class HTMLDocument
		{
			// Token: 0x060000E6 RID: 230
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			public extern HTMLDocument();
		}

		// Token: 0x0200003A RID: 58
		[Guid("626FC520-A41E-11CF-A731-00A0C9082637")]
		[ComVisible(true)]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComImport]
		public interface IHTMLDocument
		{
			// Token: 0x060000E7 RID: 231
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetScript();
		}

		// Token: 0x0200003B RID: 59
		[Guid("332C4425-26CB-11D0-B483-00C04FD90119")]
		[ComVisible(true)]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComImport]
		public interface IHTMLDocument2
		{
			// Token: 0x060000E8 RID: 232
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetScript();

			// Token: 0x060000E9 RID: 233
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElementCollection GetAll();

			// Token: 0x060000EA RID: 234
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElement GetBody();

			// Token: 0x060000EB RID: 235
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElement GetActiveElement();

			// Token: 0x060000EC RID: 236
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElementCollection GetImages();

			// Token: 0x060000ED RID: 237
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElementCollection GetApplets();

			// Token: 0x060000EE RID: 238
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElementCollection GetLinks();

			// Token: 0x060000EF RID: 239
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElementCollection GetForms();

			// Token: 0x060000F0 RID: 240
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElementCollection GetAnchors();

			// Token: 0x060000F1 RID: 241
			void SetTitle([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060000F2 RID: 242
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTitle();

			// Token: 0x060000F3 RID: 243
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElementCollection GetScripts();

			// Token: 0x060000F4 RID: 244
			void SetDesignMode([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060000F5 RID: 245
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetDesignMode();

			// Token: 0x060000F6 RID: 246
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetSelection();

			// Token: 0x060000F7 RID: 247
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetReadyState();

			// Token: 0x060000F8 RID: 248
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetFrames();

			// Token: 0x060000F9 RID: 249
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElementCollection GetEmbeds();

			// Token: 0x060000FA RID: 250
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElementCollection GetPlugins();

			// Token: 0x060000FB RID: 251
			void SetAlinkColor([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060000FC RID: 252
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetAlinkColor();

			// Token: 0x060000FD RID: 253
			void SetBgColor([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060000FE RID: 254
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBgColor();

			// Token: 0x060000FF RID: 255
			void SetFgColor([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000100 RID: 256
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetFgColor();

			// Token: 0x06000101 RID: 257
			void SetLinkColor([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000102 RID: 258
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetLinkColor();

			// Token: 0x06000103 RID: 259
			void SetVlinkColor([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000104 RID: 260
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetVlinkColor();

			// Token: 0x06000105 RID: 261
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetReferrer();

			// Token: 0x06000106 RID: 262
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetLocation();

			// Token: 0x06000107 RID: 263
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetLastModified();

			// Token: 0x06000108 RID: 264
			void SetURL([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000109 RID: 265
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetURL();

			// Token: 0x0600010A RID: 266
			void SetDomain([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x0600010B RID: 267
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetDomain();

			// Token: 0x0600010C RID: 268
			void SetCookie([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x0600010D RID: 269
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetCookie();

			// Token: 0x0600010E RID: 270
			void SetExpando([MarshalAs(UnmanagedType.Bool)] [In] bool p);

			// Token: 0x0600010F RID: 271
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetExpando();

			// Token: 0x06000110 RID: 272
			void SetCharset([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000111 RID: 273
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetCharset();

			// Token: 0x06000112 RID: 274
			void SetDefaultCharset([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000113 RID: 275
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetDefaultCharset();

			// Token: 0x06000114 RID: 276
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetMimeType();

			// Token: 0x06000115 RID: 277
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFileSize();

			// Token: 0x06000116 RID: 278
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFileCreatedDate();

			// Token: 0x06000117 RID: 279
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFileModifiedDate();

			// Token: 0x06000118 RID: 280
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFileUpdatedDate();

			// Token: 0x06000119 RID: 281
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetSecurity();

			// Token: 0x0600011A RID: 282
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetProtocol();

			// Token: 0x0600011B RID: 283
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetNameProp();

			// Token: 0x0600011C RID: 284
			void DummyWrite([MarshalAs(UnmanagedType.I4)] [In] int psarray);

			// Token: 0x0600011D RID: 285
			void DummyWriteln([MarshalAs(UnmanagedType.I4)] [In] int psarray);

			// Token: 0x0600011E RID: 286
			[return: MarshalAs(UnmanagedType.Interface)]
			object Open([MarshalAs(UnmanagedType.BStr)] [In] string URL, [MarshalAs(UnmanagedType.Struct)] [In] object name, [MarshalAs(UnmanagedType.Struct)] [In] object features, [MarshalAs(UnmanagedType.Struct)] [In] object replace);

			// Token: 0x0600011F RID: 287
			void Close();

			// Token: 0x06000120 RID: 288
			void Clear();

			// Token: 0x06000121 RID: 289
			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandSupported([MarshalAs(UnmanagedType.BStr)] [In] string cmdID);

			// Token: 0x06000122 RID: 290
			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandEnabled([MarshalAs(UnmanagedType.BStr)] [In] string cmdID);

			// Token: 0x06000123 RID: 291
			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandState([MarshalAs(UnmanagedType.BStr)] [In] string cmdID);

			// Token: 0x06000124 RID: 292
			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandIndeterm([MarshalAs(UnmanagedType.BStr)] [In] string cmdID);

			// Token: 0x06000125 RID: 293
			[return: MarshalAs(UnmanagedType.BStr)]
			string QueryCommandText([MarshalAs(UnmanagedType.BStr)] [In] string cmdID);

			// Token: 0x06000126 RID: 294
			[return: MarshalAs(UnmanagedType.Struct)]
			object QueryCommandValue([MarshalAs(UnmanagedType.BStr)] [In] string cmdID);

			// Token: 0x06000127 RID: 295
			[return: MarshalAs(UnmanagedType.Bool)]
			bool ExecCommand([MarshalAs(UnmanagedType.BStr)] [In] string cmdID, [MarshalAs(UnmanagedType.Bool)] [In] bool showUI, [MarshalAs(UnmanagedType.Struct)] [In] object value);

			// Token: 0x06000128 RID: 296
			[return: MarshalAs(UnmanagedType.Bool)]
			bool ExecCommandShowHelp([MarshalAs(UnmanagedType.BStr)] [In] string cmdID);

			// Token: 0x06000129 RID: 297
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElement CreateElement([MarshalAs(UnmanagedType.BStr)] [In] string eTag);

			// Token: 0x0600012A RID: 298
			void SetOnhelp([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600012B RID: 299
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnhelp();

			// Token: 0x0600012C RID: 300
			void SetOnclick([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600012D RID: 301
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnclick();

			// Token: 0x0600012E RID: 302
			void SetOndblclick([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600012F RID: 303
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndblclick();

			// Token: 0x06000130 RID: 304
			void SetOnkeyup([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000131 RID: 305
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnkeyup();

			// Token: 0x06000132 RID: 306
			void SetOnkeydown([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000133 RID: 307
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnkeydown();

			// Token: 0x06000134 RID: 308
			void SetOnkeypress([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000135 RID: 309
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnkeypress();

			// Token: 0x06000136 RID: 310
			void SetOnmouseup([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000137 RID: 311
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnmouseup();

			// Token: 0x06000138 RID: 312
			void SetOnmousedown([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000139 RID: 313
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnmousedown();

			// Token: 0x0600013A RID: 314
			void SetOnmousemove([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600013B RID: 315
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnmousemove();

			// Token: 0x0600013C RID: 316
			void SetOnmouseout([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600013D RID: 317
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnmouseout();

			// Token: 0x0600013E RID: 318
			void SetOnmouseover([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600013F RID: 319
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnmouseover();

			// Token: 0x06000140 RID: 320
			void SetOnreadystatechange([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000141 RID: 321
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnreadystatechange();

			// Token: 0x06000142 RID: 322
			void SetOnafterupdate([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000143 RID: 323
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnafterupdate();

			// Token: 0x06000144 RID: 324
			void SetOnrowexit([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000145 RID: 325
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnrowexit();

			// Token: 0x06000146 RID: 326
			void SetOnrowenter([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000147 RID: 327
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnrowenter();

			// Token: 0x06000148 RID: 328
			void SetOndragstart([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000149 RID: 329
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndragstart();

			// Token: 0x0600014A RID: 330
			void SetOnselectstart([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600014B RID: 331
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnselectstart();

			// Token: 0x0600014C RID: 332
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElement ElementFromPoint([MarshalAs(UnmanagedType.I4)] [In] int x, [MarshalAs(UnmanagedType.I4)] [In] int y);

			// Token: 0x0600014D RID: 333
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetParentWindow();

			// Token: 0x0600014E RID: 334
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetStyleSheets();

			// Token: 0x0600014F RID: 335
			void SetOnbeforeupdate([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000150 RID: 336
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnbeforeupdate();

			// Token: 0x06000151 RID: 337
			void SetOnerrorupdate([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000152 RID: 338
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnerrorupdate();

			// Token: 0x06000153 RID: 339
			[return: MarshalAs(UnmanagedType.BStr)]
			string toString();

			// Token: 0x06000154 RID: 340
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLStyleSheet CreateStyleSheet([MarshalAs(UnmanagedType.BStr)] [In] string bstrHref, [MarshalAs(UnmanagedType.I4)] [In] int lIndex);
		}

		// Token: 0x0200003C RID: 60
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[Guid("3050F1FF-98B5-11CF-BB82-00AA00BDCE0B")]
		[ComVisible(true)]
		[ComImport]
		public interface IHTMLElement
		{
			// Token: 0x06000155 RID: 341
			void SetAttribute([MarshalAs(UnmanagedType.BStr)] [In] string strAttributeName, [MarshalAs(UnmanagedType.Struct)] [In] object AttributeValue, [MarshalAs(UnmanagedType.I4)] [In] int lFlags);

			// Token: 0x06000156 RID: 342
			void GetAttribute([MarshalAs(UnmanagedType.BStr)] [In] string strAttributeName, [MarshalAs(UnmanagedType.I4)] [In] int lFlags, [MarshalAs(UnmanagedType.LPArray)] [Out] object[] pvars);

			// Token: 0x06000157 RID: 343
			[return: MarshalAs(UnmanagedType.Bool)]
			bool RemoveAttribute([MarshalAs(UnmanagedType.BStr)] [In] string strAttributeName, [MarshalAs(UnmanagedType.I4)] [In] int lFlags);

			// Token: 0x06000158 RID: 344
			void SetClassName([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000159 RID: 345
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetClassName();

			// Token: 0x0600015A RID: 346
			void SetId([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x0600015B RID: 347
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetId();

			// Token: 0x0600015C RID: 348
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTagName();

			// Token: 0x0600015D RID: 349
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElement GetParentElement();

			// Token: 0x0600015E RID: 350
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLStyle GetStyle();

			// Token: 0x0600015F RID: 351
			void SetOnhelp([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000160 RID: 352
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnhelp();

			// Token: 0x06000161 RID: 353
			void SetOnclick([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000162 RID: 354
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnclick();

			// Token: 0x06000163 RID: 355
			void SetOndblclick([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000164 RID: 356
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndblclick();

			// Token: 0x06000165 RID: 357
			void SetOnkeydown([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000166 RID: 358
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnkeydown();

			// Token: 0x06000167 RID: 359
			void SetOnkeyup([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000168 RID: 360
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnkeyup();

			// Token: 0x06000169 RID: 361
			void SetOnkeypress([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600016A RID: 362
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnkeypress();

			// Token: 0x0600016B RID: 363
			void SetOnmouseout([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600016C RID: 364
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnmouseout();

			// Token: 0x0600016D RID: 365
			void SetOnmouseover([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600016E RID: 366
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnmouseover();

			// Token: 0x0600016F RID: 367
			void SetOnmousemove([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000170 RID: 368
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnmousemove();

			// Token: 0x06000171 RID: 369
			void SetOnmousedown([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000172 RID: 370
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnmousedown();

			// Token: 0x06000173 RID: 371
			void SetOnmouseup([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000174 RID: 372
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnmouseup();

			// Token: 0x06000175 RID: 373
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDocument2 GetDocument();

			// Token: 0x06000176 RID: 374
			void SetTitle([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000177 RID: 375
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTitle();

			// Token: 0x06000178 RID: 376
			void SetLanguage([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000179 RID: 377
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetLanguage();

			// Token: 0x0600017A RID: 378
			void SetOnselectstart([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600017B RID: 379
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnselectstart();

			// Token: 0x0600017C RID: 380
			void ScrollIntoView([MarshalAs(UnmanagedType.Struct)] [In] object varargStart);

			// Token: 0x0600017D RID: 381
			[return: MarshalAs(UnmanagedType.Bool)]
			bool Contains([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IHTMLElement pChild);

			// Token: 0x0600017E RID: 382
			[return: MarshalAs(UnmanagedType.I4)]
			int GetSourceIndex();

			// Token: 0x0600017F RID: 383
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetRecordNumber();

			// Token: 0x06000180 RID: 384
			void SetLang([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000181 RID: 385
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetLang();

			// Token: 0x06000182 RID: 386
			[return: MarshalAs(UnmanagedType.I4)]
			int GetOffsetLeft();

			// Token: 0x06000183 RID: 387
			[return: MarshalAs(UnmanagedType.I4)]
			int GetOffsetTop();

			// Token: 0x06000184 RID: 388
			[return: MarshalAs(UnmanagedType.I4)]
			int GetOffsetWidth();

			// Token: 0x06000185 RID: 389
			[return: MarshalAs(UnmanagedType.I4)]
			int GetOffsetHeight();

			// Token: 0x06000186 RID: 390
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElement GetOffsetParent();

			// Token: 0x06000187 RID: 391
			void SetInnerHTML([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000188 RID: 392
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetInnerHTML();

			// Token: 0x06000189 RID: 393
			void SetInnerText([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x0600018A RID: 394
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetInnerText();

			// Token: 0x0600018B RID: 395
			void SetOuterHTML([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x0600018C RID: 396
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetOuterHTML();

			// Token: 0x0600018D RID: 397
			void SetOuterText([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x0600018E RID: 398
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetOuterText();

			// Token: 0x0600018F RID: 399
			void InsertAdjacentHTML([MarshalAs(UnmanagedType.BStr)] [In] string where, [MarshalAs(UnmanagedType.BStr)] [In] string html);

			// Token: 0x06000190 RID: 400
			void InsertAdjacentText([MarshalAs(UnmanagedType.BStr)] [In] string where, [MarshalAs(UnmanagedType.BStr)] [In] string text);

			// Token: 0x06000191 RID: 401
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElement GetParentTextEdit();

			// Token: 0x06000192 RID: 402
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetIsTextEdit();

			// Token: 0x06000193 RID: 403
			void Click();

			// Token: 0x06000194 RID: 404
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetFilters();

			// Token: 0x06000195 RID: 405
			void SetOndragstart([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000196 RID: 406
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndragstart();

			// Token: 0x06000197 RID: 407
			[return: MarshalAs(UnmanagedType.BStr)]
			string toString();

			// Token: 0x06000198 RID: 408
			void SetOnbeforeupdate([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000199 RID: 409
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnbeforeupdate();

			// Token: 0x0600019A RID: 410
			void SetOnafterupdate([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600019B RID: 411
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnafterupdate();

			// Token: 0x0600019C RID: 412
			void SetOnerrorupdate([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600019D RID: 413
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnerrorupdate();

			// Token: 0x0600019E RID: 414
			void SetOnrowexit([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600019F RID: 415
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnrowexit();

			// Token: 0x060001A0 RID: 416
			void SetOnrowenter([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001A1 RID: 417
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnrowenter();

			// Token: 0x060001A2 RID: 418
			void SetOndatasetchanged([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001A3 RID: 419
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndatasetchanged();

			// Token: 0x060001A4 RID: 420
			void SetOndataavailable([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001A5 RID: 421
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndataavailable();

			// Token: 0x060001A6 RID: 422
			void SetOndatasetcomplete([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001A7 RID: 423
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndatasetcomplete();

			// Token: 0x060001A8 RID: 424
			void SetOnfilterchange([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001A9 RID: 425
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnfilterchange();

			// Token: 0x060001AA RID: 426
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetChildren();

			// Token: 0x060001AB RID: 427
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetAll();
		}

		// Token: 0x0200003D RID: 61
		[Guid("3050F434-98B5-11CF-BB82-00AA00BDCE0B")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComImport]
		public interface IHTMLElement2
		{
			// Token: 0x060001AC RID: 428
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetScopeName();

			// Token: 0x060001AD RID: 429
			void SetCapture([MarshalAs(UnmanagedType.Bool)] [In] bool containerCapture);

			// Token: 0x060001AE RID: 430
			void ReleaseCapture();

			// Token: 0x060001AF RID: 431
			void SetOnlosecapture([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001B0 RID: 432
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnlosecapture();

			// Token: 0x060001B1 RID: 433
			[return: MarshalAs(UnmanagedType.BStr)]
			string ComponentFromPoint([MarshalAs(UnmanagedType.I4)] [In] int x, [MarshalAs(UnmanagedType.I4)] [In] int y);

			// Token: 0x060001B2 RID: 434
			void DoScroll([MarshalAs(UnmanagedType.Struct)] [In] object component);

			// Token: 0x060001B3 RID: 435
			void SetOnscroll([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001B4 RID: 436
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnscroll();

			// Token: 0x060001B5 RID: 437
			void SetOndrag([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001B6 RID: 438
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndrag();

			// Token: 0x060001B7 RID: 439
			void SetOndragend([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001B8 RID: 440
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndragend();

			// Token: 0x060001B9 RID: 441
			void SetOndragenter([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001BA RID: 442
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndragenter();

			// Token: 0x060001BB RID: 443
			void SetOndragover([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001BC RID: 444
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndragover();

			// Token: 0x060001BD RID: 445
			void SetOndragleave([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001BE RID: 446
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndragleave();

			// Token: 0x060001BF RID: 447
			void SetOndrop([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001C0 RID: 448
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndrop();

			// Token: 0x060001C1 RID: 449
			void SetOnbeforecut([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001C2 RID: 450
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnbeforecut();

			// Token: 0x060001C3 RID: 451
			void SetOncut([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001C4 RID: 452
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOncut();

			// Token: 0x060001C5 RID: 453
			void SetOnbeforecopy([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001C6 RID: 454
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnbeforecopy();

			// Token: 0x060001C7 RID: 455
			void SetOncopy([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001C8 RID: 456
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOncopy();

			// Token: 0x060001C9 RID: 457
			void SetOnbeforepaste([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001CA RID: 458
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnbeforepaste();

			// Token: 0x060001CB RID: 459
			void SetOnpaste([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001CC RID: 460
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnpaste();

			// Token: 0x060001CD RID: 461
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLCurrentStyle GetCurrentStyle();

			// Token: 0x060001CE RID: 462
			void SetOnpropertychange([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001CF RID: 463
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnpropertychange();

			// Token: 0x060001D0 RID: 464
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLRectCollection GetClientRects();

			// Token: 0x060001D1 RID: 465
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLRect GetBoundingClientRect();

			// Token: 0x060001D2 RID: 466
			void SetExpression([MarshalAs(UnmanagedType.BStr)] [In] string propname, [MarshalAs(UnmanagedType.BStr)] [In] string expression, [MarshalAs(UnmanagedType.BStr)] [In] string language);

			// Token: 0x060001D3 RID: 467
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetExpression([MarshalAs(UnmanagedType.BStr)] [In] object propname);

			// Token: 0x060001D4 RID: 468
			[return: MarshalAs(UnmanagedType.Bool)]
			bool RemoveExpression([MarshalAs(UnmanagedType.BStr)] [In] string propname);

			// Token: 0x060001D5 RID: 469
			void SetTabIndex([MarshalAs(UnmanagedType.I2)] [In] short p);

			// Token: 0x060001D6 RID: 470
			[return: MarshalAs(UnmanagedType.I2)]
			short GetTabIndex();

			// Token: 0x060001D7 RID: 471
			void Focus();

			// Token: 0x060001D8 RID: 472
			void SetAccessKey([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060001D9 RID: 473
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetAccessKey();

			// Token: 0x060001DA RID: 474
			void SetOnblur([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001DB RID: 475
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnblur();

			// Token: 0x060001DC RID: 476
			void SetOnfocus([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001DD RID: 477
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnfocus();

			// Token: 0x060001DE RID: 478
			void SetOnresize([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001DF RID: 479
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnresize();

			// Token: 0x060001E0 RID: 480
			void Blur();

			// Token: 0x060001E1 RID: 481
			void AddFilter([MarshalAs(UnmanagedType.Interface)] [In] object pUnk);

			// Token: 0x060001E2 RID: 482
			void RemoveFilter([MarshalAs(UnmanagedType.Interface)] [In] object pUnk);

			// Token: 0x060001E3 RID: 483
			[return: MarshalAs(UnmanagedType.I4)]
			int GetClientHeight();

			// Token: 0x060001E4 RID: 484
			[return: MarshalAs(UnmanagedType.I4)]
			int GetClientWidth();

			// Token: 0x060001E5 RID: 485
			[return: MarshalAs(UnmanagedType.I4)]
			int GetClientTop();

			// Token: 0x060001E6 RID: 486
			[return: MarshalAs(UnmanagedType.I4)]
			int GetClientLeft();

			// Token: 0x060001E7 RID: 487
			[return: MarshalAs(UnmanagedType.Bool)]
			bool AttachEvent([MarshalAs(UnmanagedType.BStr)] [In] string ev, [MarshalAs(UnmanagedType.Interface)] [In] object pdisp);

			// Token: 0x060001E8 RID: 488
			void DetachEvent([MarshalAs(UnmanagedType.BStr)] [In] string ev, [MarshalAs(UnmanagedType.Interface)] [In] object pdisp);

			// Token: 0x060001E9 RID: 489
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetReadyState();

			// Token: 0x060001EA RID: 490
			void SetOnreadystatechange([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001EB RID: 491
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnreadystatechange();

			// Token: 0x060001EC RID: 492
			void SetOnrowsdelete([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001ED RID: 493
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnrowsdelete();

			// Token: 0x060001EE RID: 494
			void SetOnrowsinserted([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001EF RID: 495
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnrowsinserted();

			// Token: 0x060001F0 RID: 496
			void SetOncellchange([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001F1 RID: 497
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOncellchange();

			// Token: 0x060001F2 RID: 498
			void SetDir([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060001F3 RID: 499
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetDir();

			// Token: 0x060001F4 RID: 500
			[return: MarshalAs(UnmanagedType.Interface)]
			object CreateControlRange();

			// Token: 0x060001F5 RID: 501
			[return: MarshalAs(UnmanagedType.I4)]
			int GetScrollHeight();

			// Token: 0x060001F6 RID: 502
			[return: MarshalAs(UnmanagedType.I4)]
			int GetScrollWidth();

			// Token: 0x060001F7 RID: 503
			void SetScrollTop([MarshalAs(UnmanagedType.I4)] [In] int p);

			// Token: 0x060001F8 RID: 504
			[return: MarshalAs(UnmanagedType.I4)]
			int GetScrollTop();

			// Token: 0x060001F9 RID: 505
			void SetScrollLeft([MarshalAs(UnmanagedType.I4)] [In] int p);

			// Token: 0x060001FA RID: 506
			[return: MarshalAs(UnmanagedType.I4)]
			int GetScrollLeft();

			// Token: 0x060001FB RID: 507
			void ClearAttributes();

			// Token: 0x060001FC RID: 508
			void MergeAttributes([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IHTMLElement mergeThis);

			// Token: 0x060001FD RID: 509
			void SetOncontextmenu([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060001FE RID: 510
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOncontextmenu();

			// Token: 0x060001FF RID: 511
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElement InsertAdjacentElement([MarshalAs(UnmanagedType.BStr)] [In] string where, [MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IHTMLElement insertedElement);

			// Token: 0x06000200 RID: 512
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElement ApplyElement([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IHTMLElement apply, [MarshalAs(UnmanagedType.BStr)] [In] string where);

			// Token: 0x06000201 RID: 513
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetAdjacentText([MarshalAs(UnmanagedType.BStr)] [In] string where);

			// Token: 0x06000202 RID: 514
			[return: MarshalAs(UnmanagedType.BStr)]
			string ReplaceAdjacentText([MarshalAs(UnmanagedType.BStr)] [In] string where, [MarshalAs(UnmanagedType.BStr)] [In] string newText);

			// Token: 0x06000203 RID: 515
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetCanHaveChildren();

			// Token: 0x06000204 RID: 516
			[return: MarshalAs(UnmanagedType.I4)]
			int AddBehavior([MarshalAs(UnmanagedType.BStr)] [In] string bstrUrl, [In] ref object pvarFactory);

			// Token: 0x06000205 RID: 517
			[return: MarshalAs(UnmanagedType.Bool)]
			bool RemoveBehavior([MarshalAs(UnmanagedType.I4)] [In] int cookie);

			// Token: 0x06000206 RID: 518
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLStyle GetRuntimeStyle();

			// Token: 0x06000207 RID: 519
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetBehaviorUrns();

			// Token: 0x06000208 RID: 520
			void SetTagUrn([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000209 RID: 521
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTagUrn();

			// Token: 0x0600020A RID: 522
			void SetOnbeforeeditfocus([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600020B RID: 523
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnbeforeeditfocus();

			// Token: 0x0600020C RID: 524
			[return: MarshalAs(UnmanagedType.I4)]
			int GetReadyStateValue();

			// Token: 0x0600020D RID: 525
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElementCollection GetElementsByTagName([MarshalAs(UnmanagedType.BStr)] [In] string v);

			// Token: 0x0600020E RID: 526
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLStyle GetBaseStyle();

			// Token: 0x0600020F RID: 527
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLCurrentStyle GetBaseCurrentStyle();

			// Token: 0x06000210 RID: 528
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLStyle GetBaseRuntimeStyle();

			// Token: 0x06000211 RID: 529
			void SetOnmousehover([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000212 RID: 530
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnmousehover();

			// Token: 0x06000213 RID: 531
			void SetOnkeydownpreview([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000214 RID: 532
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnkeydownpreview();

			// Token: 0x06000215 RID: 533
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetBehavior([MarshalAs(UnmanagedType.BStr)] [In] string bstrName, [MarshalAs(UnmanagedType.BStr)] [In] string bstrUrn);
		}

		// Token: 0x0200003E RID: 62
		[ComVisible(true)]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[Guid("3050F673-98B5-11CF-BB82-00AA00BDCE0B")]
		[ComImport]
		public interface IHTMLElement3
		{
			// Token: 0x06000216 RID: 534
			void MergeAttributes([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IHTMLElement mergeThis, [MarshalAs(UnmanagedType.Struct)] [In] object pvarFlags);

			// Token: 0x06000217 RID: 535
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetIsMultiLine();

			// Token: 0x06000218 RID: 536
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetCanHaveHTML();

			// Token: 0x06000219 RID: 537
			void SetOnLayoutComplete([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600021A RID: 538
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnLayoutComplete();

			// Token: 0x0600021B RID: 539
			void SetOnPage([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600021C RID: 540
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnPage();

			// Token: 0x0600021D RID: 541
			void SetInflateBlock([MarshalAs(UnmanagedType.Bool)] [In] bool inflate);

			// Token: 0x0600021E RID: 542
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetInflateBlock();

			// Token: 0x0600021F RID: 543
			void SetOnBeforeDeactivate([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000220 RID: 544
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnBeforeDeactivate();

			// Token: 0x06000221 RID: 545
			void SetActive();

			// Token: 0x06000222 RID: 546
			void SetContentEditable([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000223 RID: 547
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetContentEditable();

			// Token: 0x06000224 RID: 548
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetIsContentEditable();

			// Token: 0x06000225 RID: 549
			void SetHideFocus([MarshalAs(UnmanagedType.Bool)] [In] bool v);

			// Token: 0x06000226 RID: 550
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetHideFocus();

			// Token: 0x06000227 RID: 551
			void SetDisabled([MarshalAs(UnmanagedType.Bool)] [In] bool v);

			// Token: 0x06000228 RID: 552
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetDisabled();

			// Token: 0x06000229 RID: 553
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetIsDisabled();

			// Token: 0x0600022A RID: 554
			void SetOnMove([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600022B RID: 555
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnMove();

			// Token: 0x0600022C RID: 556
			void SetOnControlSelect([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600022D RID: 557
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnControlSelect();

			// Token: 0x0600022E RID: 558
			[return: MarshalAs(UnmanagedType.Bool)]
			bool FireEvent([MarshalAs(UnmanagedType.BStr)] [In] string eventName, [MarshalAs(UnmanagedType.Struct)] [In] object eventObject);

			// Token: 0x0600022F RID: 559
			void SetOnResizeStart([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000230 RID: 560
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnResizeStart();

			// Token: 0x06000231 RID: 561
			void SetOnResizeEnd([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000232 RID: 562
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnResizeEnd();

			// Token: 0x06000233 RID: 563
			void SetOnMoveStart([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000234 RID: 564
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnMoveStart();

			// Token: 0x06000235 RID: 565
			void SetOnMoveEnd([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000236 RID: 566
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnMoveEnd();

			// Token: 0x06000237 RID: 567
			void SetOnMouseEnter([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000238 RID: 568
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnMouseEnter();

			// Token: 0x06000239 RID: 569
			void SetOnMouseLeave([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600023A RID: 570
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnMouseLeave();

			// Token: 0x0600023B RID: 571
			void SetOnActivate([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600023C RID: 572
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnActivate();

			// Token: 0x0600023D RID: 573
			void SetOnDeactivate([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600023E RID: 574
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnDeactivate();

			// Token: 0x0600023F RID: 575
			[return: MarshalAs(UnmanagedType.Bool)]
			bool DragDrop();

			// Token: 0x06000240 RID: 576
			[return: MarshalAs(UnmanagedType.I4)]
			int GetGlyphMode();
		}

		// Token: 0x0200003F RID: 63
		[ComVisible(true)]
		[Guid("3050F1D8-98B5-11CF-BB82-00AA00BDCE0B")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComImport]
		public interface IHTMLBodyElement
		{
			// Token: 0x06000241 RID: 577
			void SetBackground([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000242 RID: 578
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackground();

			// Token: 0x06000243 RID: 579
			void SetBgProperties([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000244 RID: 580
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBgProperties();

			// Token: 0x06000245 RID: 581
			void SetLeftMargin([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000246 RID: 582
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetLeftMargin();

			// Token: 0x06000247 RID: 583
			void SetTopMargin([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000248 RID: 584
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetTopMargin();

			// Token: 0x06000249 RID: 585
			void SetRightMargin([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600024A RID: 586
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetRightMargin();

			// Token: 0x0600024B RID: 587
			void SetBottomMargin([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600024C RID: 588
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBottomMargin();

			// Token: 0x0600024D RID: 589
			void SetNoWrap([MarshalAs(UnmanagedType.Bool)] [In] bool p);

			// Token: 0x0600024E RID: 590
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetNoWrap();

			// Token: 0x0600024F RID: 591
			void SetBgColor([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000250 RID: 592
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBgColor();

			// Token: 0x06000251 RID: 593
			void SetText([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000252 RID: 594
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetText();

			// Token: 0x06000253 RID: 595
			void SetLink([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000254 RID: 596
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetLink();

			// Token: 0x06000255 RID: 597
			void SetVLink([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000256 RID: 598
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetVLink();

			// Token: 0x06000257 RID: 599
			void SetALink([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000258 RID: 600
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetALink();

			// Token: 0x06000259 RID: 601
			void SetOnload([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600025A RID: 602
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnload();

			// Token: 0x0600025B RID: 603
			void SetOnunload([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600025C RID: 604
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnunload();

			// Token: 0x0600025D RID: 605
			void SetScroll([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x0600025E RID: 606
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetScroll();

			// Token: 0x0600025F RID: 607
			void SetOnselect([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000260 RID: 608
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnselect();

			// Token: 0x06000261 RID: 609
			void SetOnbeforeunload([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000262 RID: 610
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnbeforeunload();

			// Token: 0x06000263 RID: 611
			[return: MarshalAs(UnmanagedType.Interface)]
			object CreateTextRange();
		}

		// Token: 0x02000040 RID: 64
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComVisible(true)]
		[Guid("3050F2E3-98B5-11CF-BB82-00AA00BDCE0B")]
		[ComImport]
		public interface IHTMLStyleSheet
		{
			// Token: 0x06000264 RID: 612
			void SetTitle([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000265 RID: 613
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTitle();

			// Token: 0x06000266 RID: 614
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLStyleSheet GetParentStyleSheet();

			// Token: 0x06000267 RID: 615
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElement GetOwningElement();

			// Token: 0x06000268 RID: 616
			void SetDisabled([MarshalAs(UnmanagedType.Bool)] [In] bool p);

			// Token: 0x06000269 RID: 617
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetDisabled();

			// Token: 0x0600026A RID: 618
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetReadOnly();

			// Token: 0x0600026B RID: 619
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetImports();

			// Token: 0x0600026C RID: 620
			void SetHref([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x0600026D RID: 621
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetHref();

			// Token: 0x0600026E RID: 622
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetStyleSheetType();

			// Token: 0x0600026F RID: 623
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetId();

			// Token: 0x06000270 RID: 624
			[return: MarshalAs(UnmanagedType.I4)]
			int AddImport([MarshalAs(UnmanagedType.BStr)] [In] string bstrURL, [MarshalAs(UnmanagedType.I4)] [In] int lIndex);

			// Token: 0x06000271 RID: 625
			[return: MarshalAs(UnmanagedType.I4)]
			int AddRule([MarshalAs(UnmanagedType.BStr)] [In] string bstrSelector, [MarshalAs(UnmanagedType.BStr)] [In] string bstrStyle, [MarshalAs(UnmanagedType.I4)] [In] int lIndex);

			// Token: 0x06000272 RID: 626
			void RemoveImport([MarshalAs(UnmanagedType.I4)] [In] int lIndex);

			// Token: 0x06000273 RID: 627
			void RemoveRule([MarshalAs(UnmanagedType.I4)] [In] int lIndex);

			// Token: 0x06000274 RID: 628
			void SetMedia([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000275 RID: 629
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetMedia();

			// Token: 0x06000276 RID: 630
			void SetCssText([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000277 RID: 631
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetCssText();

			// Token: 0x06000278 RID: 632
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetRules();
		}

		// Token: 0x02000041 RID: 65
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComVisible(true)]
		[Guid("3050F25E-98B5-11CF-BB82-00AA00BDCE0B")]
		[ComImport]
		public interface IHTMLStyle
		{
			// Token: 0x06000279 RID: 633
			void SetFontFamily([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x0600027A RID: 634
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontFamily();

			// Token: 0x0600027B RID: 635
			void SetFontStyle([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x0600027C RID: 636
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontStyle();

			// Token: 0x0600027D RID: 637
			void SetFontObject([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x0600027E RID: 638
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontObject();

			// Token: 0x0600027F RID: 639
			void SetFontWeight([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000280 RID: 640
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontWeight();

			// Token: 0x06000281 RID: 641
			void SetFontSize([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000282 RID: 642
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetFontSize();

			// Token: 0x06000283 RID: 643
			void SetFont([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000284 RID: 644
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFont();

			// Token: 0x06000285 RID: 645
			void SetColor([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000286 RID: 646
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetColor();

			// Token: 0x06000287 RID: 647
			void SetBackground([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000288 RID: 648
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackground();

			// Token: 0x06000289 RID: 649
			void SetBackgroundColor([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600028A RID: 650
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBackgroundColor();

			// Token: 0x0600028B RID: 651
			void SetBackgroundImage([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x0600028C RID: 652
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundImage();

			// Token: 0x0600028D RID: 653
			void SetBackgroundRepeat([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x0600028E RID: 654
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundRepeat();

			// Token: 0x0600028F RID: 655
			void SetBackgroundAttachment([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000290 RID: 656
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundAttachment();

			// Token: 0x06000291 RID: 657
			void SetBackgroundPosition([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000292 RID: 658
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundPosition();

			// Token: 0x06000293 RID: 659
			void SetBackgroundPositionX([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000294 RID: 660
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBackgroundPositionX();

			// Token: 0x06000295 RID: 661
			void SetBackgroundPositionY([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000296 RID: 662
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBackgroundPositionY();

			// Token: 0x06000297 RID: 663
			void SetWordSpacing([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000298 RID: 664
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetWordSpacing();

			// Token: 0x06000299 RID: 665
			void SetLetterSpacing([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600029A RID: 666
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetLetterSpacing();

			// Token: 0x0600029B RID: 667
			void SetTextDecoration([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x0600029C RID: 668
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTextDecoration();

			// Token: 0x0600029D RID: 669
			void SetTextDecorationNone([MarshalAs(UnmanagedType.Bool)] [In] bool p);

			// Token: 0x0600029E RID: 670
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTextDecorationNone();

			// Token: 0x0600029F RID: 671
			void SetTextDecorationUnderline([MarshalAs(UnmanagedType.Bool)] [In] bool p);

			// Token: 0x060002A0 RID: 672
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTextDecorationUnderline();

			// Token: 0x060002A1 RID: 673
			void SetTextDecorationOverline([MarshalAs(UnmanagedType.Bool)] [In] bool p);

			// Token: 0x060002A2 RID: 674
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTextDecorationOverline();

			// Token: 0x060002A3 RID: 675
			void SetTextDecorationLineThrough([MarshalAs(UnmanagedType.Bool)] [In] bool p);

			// Token: 0x060002A4 RID: 676
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTextDecorationLineThrough();

			// Token: 0x060002A5 RID: 677
			void SetTextDecorationBlink([MarshalAs(UnmanagedType.Bool)] [In] bool p);

			// Token: 0x060002A6 RID: 678
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTextDecorationBlink();

			// Token: 0x060002A7 RID: 679
			void SetVerticalAlign([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060002A8 RID: 680
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetVerticalAlign();

			// Token: 0x060002A9 RID: 681
			void SetTextTransform([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060002AA RID: 682
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTextTransform();

			// Token: 0x060002AB RID: 683
			void SetTextAlign([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060002AC RID: 684
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTextAlign();

			// Token: 0x060002AD RID: 685
			void SetTextIndent([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060002AE RID: 686
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetTextIndent();

			// Token: 0x060002AF RID: 687
			void SetLineHeight([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060002B0 RID: 688
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetLineHeight();

			// Token: 0x060002B1 RID: 689
			void SetMarginTop([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060002B2 RID: 690
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetMarginTop();

			// Token: 0x060002B3 RID: 691
			void SetMarginRight([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060002B4 RID: 692
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetMarginRight();

			// Token: 0x060002B5 RID: 693
			void SetMarginBottom([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060002B6 RID: 694
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetMarginBottom();

			// Token: 0x060002B7 RID: 695
			void SetMarginLeft([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060002B8 RID: 696
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetMarginLeft();

			// Token: 0x060002B9 RID: 697
			void SetMargin([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060002BA RID: 698
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetMargin();

			// Token: 0x060002BB RID: 699
			void SetPaddingTop([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060002BC RID: 700
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetPaddingTop();

			// Token: 0x060002BD RID: 701
			void SetPaddingRight([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060002BE RID: 702
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetPaddingRight();

			// Token: 0x060002BF RID: 703
			void SetPaddingBottom([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060002C0 RID: 704
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetPaddingBottom();

			// Token: 0x060002C1 RID: 705
			void SetPaddingLeft([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060002C2 RID: 706
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetPaddingLeft();

			// Token: 0x060002C3 RID: 707
			void SetPadding([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060002C4 RID: 708
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPadding();

			// Token: 0x060002C5 RID: 709
			void SetBorder([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060002C6 RID: 710
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorder();

			// Token: 0x060002C7 RID: 711
			void SetBorderTop([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060002C8 RID: 712
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderTop();

			// Token: 0x060002C9 RID: 713
			void SetBorderRight([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060002CA RID: 714
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderRight();

			// Token: 0x060002CB RID: 715
			void SetBorderBottom([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060002CC RID: 716
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderBottom();

			// Token: 0x060002CD RID: 717
			void SetBorderLeft([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060002CE RID: 718
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderLeft();

			// Token: 0x060002CF RID: 719
			void SetBorderColor([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060002D0 RID: 720
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderColor();

			// Token: 0x060002D1 RID: 721
			void SetBorderTopColor([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060002D2 RID: 722
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderTopColor();

			// Token: 0x060002D3 RID: 723
			void SetBorderRightColor([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060002D4 RID: 724
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderRightColor();

			// Token: 0x060002D5 RID: 725
			void SetBorderBottomColor([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060002D6 RID: 726
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderBottomColor();

			// Token: 0x060002D7 RID: 727
			void SetBorderLeftColor([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060002D8 RID: 728
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderLeftColor();

			// Token: 0x060002D9 RID: 729
			void SetBorderWidth([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060002DA RID: 730
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderWidth();

			// Token: 0x060002DB RID: 731
			void SetBorderTopWidth([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060002DC RID: 732
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderTopWidth();

			// Token: 0x060002DD RID: 733
			void SetBorderRightWidth([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060002DE RID: 734
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderRightWidth();

			// Token: 0x060002DF RID: 735
			void SetBorderBottomWidth([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060002E0 RID: 736
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderBottomWidth();

			// Token: 0x060002E1 RID: 737
			void SetBorderLeftWidth([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060002E2 RID: 738
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderLeftWidth();

			// Token: 0x060002E3 RID: 739
			void SetBorderStyle([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060002E4 RID: 740
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderStyle();

			// Token: 0x060002E5 RID: 741
			void SetBorderTopStyle([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060002E6 RID: 742
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderTopStyle();

			// Token: 0x060002E7 RID: 743
			void SetBorderRightStyle([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060002E8 RID: 744
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderRightStyle();

			// Token: 0x060002E9 RID: 745
			void SetBorderBottomStyle([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060002EA RID: 746
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderBottomStyle();

			// Token: 0x060002EB RID: 747
			void SetBorderLeftStyle([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060002EC RID: 748
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderLeftStyle();

			// Token: 0x060002ED RID: 749
			void SetWidth([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060002EE RID: 750
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetWidth();

			// Token: 0x060002EF RID: 751
			void SetHeight([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x060002F0 RID: 752
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetHeight();

			// Token: 0x060002F1 RID: 753
			void SetStyleFloat([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060002F2 RID: 754
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetStyleFloat();

			// Token: 0x060002F3 RID: 755
			void SetClear([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060002F4 RID: 756
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetClear();

			// Token: 0x060002F5 RID: 757
			void SetDisplay([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060002F6 RID: 758
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetDisplay();

			// Token: 0x060002F7 RID: 759
			void SetVisibility([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060002F8 RID: 760
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetVisibility();

			// Token: 0x060002F9 RID: 761
			void SetListStyleType([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060002FA RID: 762
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStyleType();

			// Token: 0x060002FB RID: 763
			void SetListStylePosition([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060002FC RID: 764
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStylePosition();

			// Token: 0x060002FD RID: 765
			void SetListStyleImage([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x060002FE RID: 766
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStyleImage();

			// Token: 0x060002FF RID: 767
			void SetListStyle([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000300 RID: 768
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStyle();

			// Token: 0x06000301 RID: 769
			void SetWhiteSpace([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000302 RID: 770
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetWhiteSpace();

			// Token: 0x06000303 RID: 771
			void SetTop([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000304 RID: 772
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetTop();

			// Token: 0x06000305 RID: 773
			void SetLeft([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000306 RID: 774
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetLeft();

			// Token: 0x06000307 RID: 775
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPosition();

			// Token: 0x06000308 RID: 776
			void SetZIndex([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x06000309 RID: 777
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetZIndex();

			// Token: 0x0600030A RID: 778
			void SetOverflow([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x0600030B RID: 779
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetOverflow();

			// Token: 0x0600030C RID: 780
			void SetPageBreakBefore([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x0600030D RID: 781
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPageBreakBefore();

			// Token: 0x0600030E RID: 782
			void SetPageBreakAfter([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x0600030F RID: 783
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPageBreakAfter();

			// Token: 0x06000310 RID: 784
			void SetCssText([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000311 RID: 785
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetCssText();

			// Token: 0x06000312 RID: 786
			void SetPixelTop([MarshalAs(UnmanagedType.I4)] [In] int p);

			// Token: 0x06000313 RID: 787
			[return: MarshalAs(UnmanagedType.I4)]
			int GetPixelTop();

			// Token: 0x06000314 RID: 788
			void SetPixelLeft([MarshalAs(UnmanagedType.I4)] [In] int p);

			// Token: 0x06000315 RID: 789
			[return: MarshalAs(UnmanagedType.I4)]
			int GetPixelLeft();

			// Token: 0x06000316 RID: 790
			void SetPixelWidth([MarshalAs(UnmanagedType.I4)] [In] int p);

			// Token: 0x06000317 RID: 791
			[return: MarshalAs(UnmanagedType.I4)]
			int GetPixelWidth();

			// Token: 0x06000318 RID: 792
			void SetPixelHeight([MarshalAs(UnmanagedType.I4)] [In] int p);

			// Token: 0x06000319 RID: 793
			[return: MarshalAs(UnmanagedType.I4)]
			int GetPixelHeight();

			// Token: 0x0600031A RID: 794
			void SetPosTop([MarshalAs(UnmanagedType.R4)] [In] float p);

			// Token: 0x0600031B RID: 795
			[return: MarshalAs(UnmanagedType.R4)]
			float GetPosTop();

			// Token: 0x0600031C RID: 796
			void SetPosLeft([MarshalAs(UnmanagedType.R4)] [In] float p);

			// Token: 0x0600031D RID: 797
			[return: MarshalAs(UnmanagedType.R4)]
			float GetPosLeft();

			// Token: 0x0600031E RID: 798
			void SetPosWidth([MarshalAs(UnmanagedType.R4)] [In] float p);

			// Token: 0x0600031F RID: 799
			[return: MarshalAs(UnmanagedType.R4)]
			float GetPosWidth();

			// Token: 0x06000320 RID: 800
			void SetPosHeight([MarshalAs(UnmanagedType.R4)] [In] float p);

			// Token: 0x06000321 RID: 801
			[return: MarshalAs(UnmanagedType.R4)]
			float GetPosHeight();

			// Token: 0x06000322 RID: 802
			void SetCursor([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000323 RID: 803
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetCursor();

			// Token: 0x06000324 RID: 804
			void SetClip([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000325 RID: 805
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetClip();

			// Token: 0x06000326 RID: 806
			void SetFilter([MarshalAs(UnmanagedType.BStr)] [In] string p);

			// Token: 0x06000327 RID: 807
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFilter();

			// Token: 0x06000328 RID: 808
			void SetAttribute([MarshalAs(UnmanagedType.BStr)] [In] string strAttributeName, [MarshalAs(UnmanagedType.Struct)] [In] object AttributeValue, [MarshalAs(UnmanagedType.I4)] [In] int lFlags);

			// Token: 0x06000329 RID: 809
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetAttribute([MarshalAs(UnmanagedType.BStr)] [In] string strAttributeName, [MarshalAs(UnmanagedType.I4)] [In] int lFlags);

			// Token: 0x0600032A RID: 810
			[return: MarshalAs(UnmanagedType.Bool)]
			bool RemoveAttribute([MarshalAs(UnmanagedType.BStr)] [In] string strAttributeName, [MarshalAs(UnmanagedType.I4)] [In] int lFlags);
		}

		// Token: 0x02000042 RID: 66
		[Guid("3050F3DB-98B5-11CF-BB82-00AA00BDCE0B")]
		[ComVisible(true)]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComImport]
		public interface IHTMLCurrentStyle
		{
			// Token: 0x0600032B RID: 811
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPosition();

			// Token: 0x0600032C RID: 812
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetStyleFloat();

			// Token: 0x0600032D RID: 813
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetColor();

			// Token: 0x0600032E RID: 814
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBackgroundColor();

			// Token: 0x0600032F RID: 815
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontFamily();

			// Token: 0x06000330 RID: 816
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontStyle();

			// Token: 0x06000331 RID: 817
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontObject();

			// Token: 0x06000332 RID: 818
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetFontWeight();

			// Token: 0x06000333 RID: 819
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetFontSize();

			// Token: 0x06000334 RID: 820
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundImage();

			// Token: 0x06000335 RID: 821
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBackgroundPositionX();

			// Token: 0x06000336 RID: 822
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBackgroundPositionY();

			// Token: 0x06000337 RID: 823
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundRepeat();

			// Token: 0x06000338 RID: 824
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderLeftColor();

			// Token: 0x06000339 RID: 825
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderTopColor();

			// Token: 0x0600033A RID: 826
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderRightColor();

			// Token: 0x0600033B RID: 827
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderBottomColor();

			// Token: 0x0600033C RID: 828
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderTopStyle();

			// Token: 0x0600033D RID: 829
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderRightStyle();

			// Token: 0x0600033E RID: 830
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderBottomStyle();

			// Token: 0x0600033F RID: 831
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderLeftStyle();

			// Token: 0x06000340 RID: 832
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderTopWidth();

			// Token: 0x06000341 RID: 833
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderRightWidth();

			// Token: 0x06000342 RID: 834
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderBottomWidth();

			// Token: 0x06000343 RID: 835
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderLeftWidth();

			// Token: 0x06000344 RID: 836
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetLeft();

			// Token: 0x06000345 RID: 837
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetTop();

			// Token: 0x06000346 RID: 838
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetWidth();

			// Token: 0x06000347 RID: 839
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetHeight();

			// Token: 0x06000348 RID: 840
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetPaddingLeft();

			// Token: 0x06000349 RID: 841
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetPaddingTop();

			// Token: 0x0600034A RID: 842
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetPaddingRight();

			// Token: 0x0600034B RID: 843
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetPaddingBottom();

			// Token: 0x0600034C RID: 844
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTextAlign();

			// Token: 0x0600034D RID: 845
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTextDecoration();

			// Token: 0x0600034E RID: 846
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetDisplay();

			// Token: 0x0600034F RID: 847
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetVisibility();

			// Token: 0x06000350 RID: 848
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetZIndex();

			// Token: 0x06000351 RID: 849
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetLetterSpacing();

			// Token: 0x06000352 RID: 850
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetLineHeight();

			// Token: 0x06000353 RID: 851
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetTextIndent();

			// Token: 0x06000354 RID: 852
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetVerticalAlign();

			// Token: 0x06000355 RID: 853
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundAttachment();

			// Token: 0x06000356 RID: 854
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetMarginTop();

			// Token: 0x06000357 RID: 855
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetMarginRight();

			// Token: 0x06000358 RID: 856
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetMarginBottom();

			// Token: 0x06000359 RID: 857
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetMarginLeft();

			// Token: 0x0600035A RID: 858
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetClear();

			// Token: 0x0600035B RID: 859
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStyleType();

			// Token: 0x0600035C RID: 860
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStylePosition();

			// Token: 0x0600035D RID: 861
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStyleImage();

			// Token: 0x0600035E RID: 862
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetClipTop();

			// Token: 0x0600035F RID: 863
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetClipRight();

			// Token: 0x06000360 RID: 864
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetClipBottom();

			// Token: 0x06000361 RID: 865
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetClipLeft();

			// Token: 0x06000362 RID: 866
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetOverflow();

			// Token: 0x06000363 RID: 867
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPageBreakBefore();

			// Token: 0x06000364 RID: 868
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPageBreakAfter();

			// Token: 0x06000365 RID: 869
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetCursor();

			// Token: 0x06000366 RID: 870
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTableLayout();

			// Token: 0x06000367 RID: 871
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderCollapse();

			// Token: 0x06000368 RID: 872
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetDirection();

			// Token: 0x06000369 RID: 873
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBehavior();

			// Token: 0x0600036A RID: 874
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetAttribute([MarshalAs(UnmanagedType.BStr)] [In] string strAttributeName, [MarshalAs(UnmanagedType.I4)] [In] int lFlags);

			// Token: 0x0600036B RID: 875
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetUnicodeBidi();

			// Token: 0x0600036C RID: 876
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetRight();

			// Token: 0x0600036D RID: 877
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBottom();
		}

		// Token: 0x02000043 RID: 67
		[Guid("3050F21F-98B5-11CF-BB82-00AA00BDCE0B")]
		[ComVisible(true)]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComImport]
		public interface IHTMLElementCollection
		{
			// Token: 0x0600036E RID: 878
			[return: MarshalAs(UnmanagedType.BStr)]
			string toString();

			// Token: 0x0600036F RID: 879
			void SetLength([MarshalAs(UnmanagedType.I4)] [In] int p);

			// Token: 0x06000370 RID: 880
			[return: MarshalAs(UnmanagedType.I4)]
			int GetLength();

			// Token: 0x06000371 RID: 881
			[return: MarshalAs(UnmanagedType.Interface)]
			object Get_newEnum();

			// Token: 0x06000372 RID: 882
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElement Item([MarshalAs(UnmanagedType.Struct)] [In] object name, [MarshalAs(UnmanagedType.Struct)] [In] object index);

			// Token: 0x06000373 RID: 883
			[return: MarshalAs(UnmanagedType.Interface)]
			object Tags([MarshalAs(UnmanagedType.Struct)] [In] object tagName);
		}

		// Token: 0x02000044 RID: 68
		[ComVisible(true)]
		[Guid("3050F4A3-98B5-11CF-BB82-00AA00BDCE0B")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComImport]
		public interface IHTMLRect
		{
			// Token: 0x06000374 RID: 884
			void SetLeft([MarshalAs(UnmanagedType.I4)] [In] int p);

			// Token: 0x06000375 RID: 885
			[return: MarshalAs(UnmanagedType.I4)]
			int GetLeft();

			// Token: 0x06000376 RID: 886
			void SetTop([MarshalAs(UnmanagedType.I4)] [In] int p);

			// Token: 0x06000377 RID: 887
			[return: MarshalAs(UnmanagedType.I4)]
			int GetTop();

			// Token: 0x06000378 RID: 888
			void SetRight([MarshalAs(UnmanagedType.I4)] [In] int p);

			// Token: 0x06000379 RID: 889
			[return: MarshalAs(UnmanagedType.I4)]
			int GetRight();

			// Token: 0x0600037A RID: 890
			void SetBottom([MarshalAs(UnmanagedType.I4)] [In] int p);

			// Token: 0x0600037B RID: 891
			[return: MarshalAs(UnmanagedType.I4)]
			int GetBottom();
		}

		// Token: 0x02000045 RID: 69
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComVisible(true)]
		[Guid("3050F4A4-98B5-11CF-BB82-00AA00BDCE0B")]
		[ComImport]
		public interface IHTMLRectCollection
		{
			// Token: 0x0600037C RID: 892
			[return: MarshalAs(UnmanagedType.I4)]
			int GetLength();

			// Token: 0x0600037D RID: 893
			[return: MarshalAs(UnmanagedType.Interface)]
			object Get_newEnum();

			// Token: 0x0600037E RID: 894
			[return: MarshalAs(UnmanagedType.Struct)]
			object Item([In] ref object pvarIndex);
		}

		// Token: 0x02000046 RID: 70
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[Guid("3050F5DA-98B5-11CF-BB82-00AA00BDCE0B")]
		[ComVisible(true)]
		[ComImport]
		public interface IHTMLDOMNode
		{
			// Token: 0x0600037F RID: 895
			[return: MarshalAs(UnmanagedType.I4)]
			int GetNodeType();

			// Token: 0x06000380 RID: 896
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode GetParentNode();

			// Token: 0x06000381 RID: 897
			[return: MarshalAs(UnmanagedType.Bool)]
			bool HasChildNodes();

			// Token: 0x06000382 RID: 898
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetChildNodes();

			// Token: 0x06000383 RID: 899
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetAttributes();

			// Token: 0x06000384 RID: 900
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode InsertBefore([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IHTMLDOMNode newChild, [MarshalAs(UnmanagedType.Struct)] [In] object refChild);

			// Token: 0x06000385 RID: 901
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode RemoveChild([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IHTMLDOMNode oldChild);

			// Token: 0x06000386 RID: 902
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode ReplaceChild([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IHTMLDOMNode newChild, [MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IHTMLDOMNode oldChild);

			// Token: 0x06000387 RID: 903
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode CloneNode([MarshalAs(UnmanagedType.Bool)] [In] bool fDeep);

			// Token: 0x06000388 RID: 904
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode RemoveNode([MarshalAs(UnmanagedType.Bool)] [In] bool fDeep);

			// Token: 0x06000389 RID: 905
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode SwapNode([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IHTMLDOMNode otherNode);

			// Token: 0x0600038A RID: 906
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode ReplaceNode([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IHTMLDOMNode replacement);

			// Token: 0x0600038B RID: 907
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode AppendChild([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IHTMLDOMNode newChild);

			// Token: 0x0600038C RID: 908
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetNodeName();

			// Token: 0x0600038D RID: 909
			void SetNodeValue([MarshalAs(UnmanagedType.Struct)] [In] object p);

			// Token: 0x0600038E RID: 910
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetNodeValue();

			// Token: 0x0600038F RID: 911
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode GetFirstChild();

			// Token: 0x06000390 RID: 912
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode GetLastChild();

			// Token: 0x06000391 RID: 913
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode GetPreviousSibling();

			// Token: 0x06000392 RID: 914
			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode GetNextSibling();
		}

		// Token: 0x02000047 RID: 71
		[StructLayout(LayoutKind.Sequential)]
		public class HDHITTESTINFO
		{
			// Token: 0x0400096B RID: 2411
			public int pt_x;

			// Token: 0x0400096C RID: 2412
			public int pt_y;

			// Token: 0x0400096D RID: 2413
			public int flags;

			// Token: 0x0400096E RID: 2414
			public int iItem;
		}

		// Token: 0x02000048 RID: 72
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagOLEVERB
		{
			// Token: 0x0400096F RID: 2415
			[MarshalAs(UnmanagedType.I4)]
			public int lVerb;

			// Token: 0x04000970 RID: 2416
			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpszVerbName;

			// Token: 0x04000971 RID: 2417
			[MarshalAs(UnmanagedType.U4)]
			public int fuFlags;

			// Token: 0x04000972 RID: 2418
			[MarshalAs(UnmanagedType.U4)]
			public int grfAttribs;
		}

		// Token: 0x02000049 RID: 73
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
		public class TV_HITTESTINFO
		{
			// Token: 0x04000973 RID: 2419
			public int pt_x;

			// Token: 0x04000974 RID: 2420
			public int pt_y;

			// Token: 0x04000975 RID: 2421
			public int flags;

			// Token: 0x04000976 RID: 2422
			public int hItem;
		}

		// Token: 0x0200004A RID: 74
		// (Invoke) Token: 0x06000397 RID: 919
		public delegate int ListViewCompareCallback(IntPtr lParam1, IntPtr lParam2, IntPtr lParamSort);

		// Token: 0x0200004B RID: 75
		// (Invoke) Token: 0x0600039B RID: 923
		public delegate void TimerProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x0200004C RID: 76
		// (Invoke) Token: 0x0600039F RID: 927
		public delegate IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x0200004D RID: 77
		internal class Util
		{
			// Token: 0x060003A2 RID: 930 RVA: 0x00002A6C File Offset: 0x00001A6C
			public static int MAKELONG(int low, int high)
			{
				return (high << 16) | (low & 65535);
			}

			// Token: 0x060003A3 RID: 931 RVA: 0x00002A7A File Offset: 0x00001A7A
			public static int MAKELPARAM(int low, int high)
			{
				return (high << 16) | (low & 65535);
			}

			// Token: 0x060003A4 RID: 932 RVA: 0x00002A88 File Offset: 0x00001A88
			public static int HIWORD(int n)
			{
				return (n >> 16) & 65535;
			}

			// Token: 0x060003A5 RID: 933 RVA: 0x00002A94 File Offset: 0x00001A94
			public static int LOWORD(int n)
			{
				return n & 65535;
			}

			// Token: 0x060003A6 RID: 934 RVA: 0x00002AA0 File Offset: 0x00001AA0
			public static int SignedHIWORD(int n)
			{
				int num = (int)((short)((n >> 16) & 65535));
				num <<= 16;
				return num >> 16;
			}

			// Token: 0x060003A7 RID: 935 RVA: 0x00002AC4 File Offset: 0x00001AC4
			public static int SignedLOWORD(int n)
			{
				int num = (int)((short)(n & 65535));
				num <<= 16;
				return num >> 16;
			}

			// Token: 0x060003A8 RID: 936 RVA: 0x00002AE5 File Offset: 0x00001AE5
			public static int SignedHIWORD(IntPtr n)
			{
				return NativeMethods.Util.SignedHIWORD((int)(long)n);
			}

			// Token: 0x060003A9 RID: 937 RVA: 0x00002AF3 File Offset: 0x00001AF3
			public static int SignedLOWORD(IntPtr n)
			{
				return NativeMethods.Util.SignedLOWORD((int)(long)n);
			}

			// Token: 0x060003AA RID: 938
			[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
			private static extern int lstrlen(string s);

			// Token: 0x060003AB RID: 939
			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			internal static extern int RegisterWindowMessage(string msg);
		}

		// Token: 0x0200004E RID: 78
		public sealed class CommonHandles
		{
			// Token: 0x04000977 RID: 2423
			public static readonly int Accelerator = global::System.Internal.HandleCollector.RegisterType("Accelerator", 80, 50);

			// Token: 0x04000978 RID: 2424
			public static readonly int Cursor = global::System.Internal.HandleCollector.RegisterType("Cursor", 20, 500);

			// Token: 0x04000979 RID: 2425
			public static readonly int EMF = global::System.Internal.HandleCollector.RegisterType("EnhancedMetaFile", 20, 500);

			// Token: 0x0400097A RID: 2426
			public static readonly int Find = global::System.Internal.HandleCollector.RegisterType("Find", 0, 1000);

			// Token: 0x0400097B RID: 2427
			public static readonly int GDI = global::System.Internal.HandleCollector.RegisterType("GDI", 90, 50);

			// Token: 0x0400097C RID: 2428
			public static readonly int HDC = global::System.Internal.HandleCollector.RegisterType("HDC", 100, 2);

			// Token: 0x0400097D RID: 2429
			public static readonly int Icon = global::System.Internal.HandleCollector.RegisterType("Icon", 20, 500);

			// Token: 0x0400097E RID: 2430
			public static readonly int Kernel = global::System.Internal.HandleCollector.RegisterType("Kernel", 0, 1000);

			// Token: 0x0400097F RID: 2431
			public static readonly int Menu = global::System.Internal.HandleCollector.RegisterType("Menu", 30, 1000);

			// Token: 0x04000980 RID: 2432
			public static readonly int Window = global::System.Internal.HandleCollector.RegisterType("Window", 5, 1000);
		}

		// Token: 0x0200004F RID: 79
		internal class ActiveX
		{
			// Token: 0x04000981 RID: 2433
			public const int OCM__BASE = 8192;

			// Token: 0x04000982 RID: 2434
			public const int DISPID_VALUE = 0;

			// Token: 0x04000983 RID: 2435
			public const int DISPID_UNKNOWN = -1;

			// Token: 0x04000984 RID: 2436
			public const int DISPID_AUTOSIZE = -500;

			// Token: 0x04000985 RID: 2437
			public const int DISPID_BACKCOLOR = -501;

			// Token: 0x04000986 RID: 2438
			public const int DISPID_BACKSTYLE = -502;

			// Token: 0x04000987 RID: 2439
			public const int DISPID_BORDERCOLOR = -503;

			// Token: 0x04000988 RID: 2440
			public const int DISPID_BORDERSTYLE = -504;

			// Token: 0x04000989 RID: 2441
			public const int DISPID_BORDERWIDTH = -505;

			// Token: 0x0400098A RID: 2442
			public const int DISPID_DRAWMODE = -507;

			// Token: 0x0400098B RID: 2443
			public const int DISPID_DRAWSTYLE = -508;

			// Token: 0x0400098C RID: 2444
			public const int DISPID_DRAWWIDTH = -509;

			// Token: 0x0400098D RID: 2445
			public const int DISPID_FILLCOLOR = -510;

			// Token: 0x0400098E RID: 2446
			public const int DISPID_FILLSTYLE = -511;

			// Token: 0x0400098F RID: 2447
			public const int DISPID_FONT = -512;

			// Token: 0x04000990 RID: 2448
			public const int DISPID_FORECOLOR = -513;

			// Token: 0x04000991 RID: 2449
			public const int DISPID_ENABLED = -514;

			// Token: 0x04000992 RID: 2450
			public const int DISPID_HWND = -515;

			// Token: 0x04000993 RID: 2451
			public const int DISPID_TABSTOP = -516;

			// Token: 0x04000994 RID: 2452
			public const int DISPID_TEXT = -517;

			// Token: 0x04000995 RID: 2453
			public const int DISPID_CAPTION = -518;

			// Token: 0x04000996 RID: 2454
			public const int DISPID_BORDERVISIBLE = -519;

			// Token: 0x04000997 RID: 2455
			public const int DISPID_APPEARANCE = -520;

			// Token: 0x04000998 RID: 2456
			public const int DISPID_MOUSEPOINTER = -521;

			// Token: 0x04000999 RID: 2457
			public const int DISPID_MOUSEICON = -522;

			// Token: 0x0400099A RID: 2458
			public const int DISPID_PICTURE = -523;

			// Token: 0x0400099B RID: 2459
			public const int DISPID_VALID = -524;

			// Token: 0x0400099C RID: 2460
			public const int DISPID_READYSTATE = -525;

			// Token: 0x0400099D RID: 2461
			public const int DISPID_REFRESH = -550;

			// Token: 0x0400099E RID: 2462
			public const int DISPID_DOCLICK = -551;

			// Token: 0x0400099F RID: 2463
			public const int DISPID_ABOUTBOX = -552;

			// Token: 0x040009A0 RID: 2464
			public const int DISPID_CLICK = -600;

			// Token: 0x040009A1 RID: 2465
			public const int DISPID_DBLCLICK = -601;

			// Token: 0x040009A2 RID: 2466
			public const int DISPID_KEYDOWN = -602;

			// Token: 0x040009A3 RID: 2467
			public const int DISPID_KEYPRESS = -603;

			// Token: 0x040009A4 RID: 2468
			public const int DISPID_KEYUP = -604;

			// Token: 0x040009A5 RID: 2469
			public const int DISPID_MOUSEDOWN = -605;

			// Token: 0x040009A6 RID: 2470
			public const int DISPID_MOUSEMOVE = -606;

			// Token: 0x040009A7 RID: 2471
			public const int DISPID_MOUSEUP = -607;

			// Token: 0x040009A8 RID: 2472
			public const int DISPID_ERROREVENT = -608;

			// Token: 0x040009A9 RID: 2473
			public const int DISPID_RIGHTTOLEFT = -611;

			// Token: 0x040009AA RID: 2474
			public const int DISPID_READYSTATECHANGE = -609;

			// Token: 0x040009AB RID: 2475
			public const int DISPID_AMBIENT_BACKCOLOR = -701;

			// Token: 0x040009AC RID: 2476
			public const int DISPID_AMBIENT_DISPLAYNAME = -702;

			// Token: 0x040009AD RID: 2477
			public const int DISPID_AMBIENT_FONT = -703;

			// Token: 0x040009AE RID: 2478
			public const int DISPID_AMBIENT_FORECOLOR = -704;

			// Token: 0x040009AF RID: 2479
			public const int DISPID_AMBIENT_LOCALEID = -705;

			// Token: 0x040009B0 RID: 2480
			public const int DISPID_AMBIENT_MESSAGEREFLECT = -706;

			// Token: 0x040009B1 RID: 2481
			public const int DISPID_AMBIENT_SCALEUNITS = -707;

			// Token: 0x040009B2 RID: 2482
			public const int DISPID_AMBIENT_TEXTALIGN = -708;

			// Token: 0x040009B3 RID: 2483
			public const int DISPID_AMBIENT_USERMODE = -709;

			// Token: 0x040009B4 RID: 2484
			public const int DISPID_AMBIENT_UIDEAD = -710;

			// Token: 0x040009B5 RID: 2485
			public const int DISPID_AMBIENT_SHOWGRABHANDLES = -711;

			// Token: 0x040009B6 RID: 2486
			public const int DISPID_AMBIENT_SHOWHATCHING = -712;

			// Token: 0x040009B7 RID: 2487
			public const int DISPID_AMBIENT_DISPLAYASDEFAULT = -713;

			// Token: 0x040009B8 RID: 2488
			public const int DISPID_AMBIENT_SUPPORTSMNEMONICS = -714;

			// Token: 0x040009B9 RID: 2489
			public const int DISPID_AMBIENT_AUTOCLIP = -715;

			// Token: 0x040009BA RID: 2490
			public const int DISPID_AMBIENT_APPEARANCE = -716;

			// Token: 0x040009BB RID: 2491
			public const int DISPID_AMBIENT_PALETTE = -726;

			// Token: 0x040009BC RID: 2492
			public const int DISPID_AMBIENT_TRANSFERPRIORITY = -728;

			// Token: 0x040009BD RID: 2493
			public const int DISPID_Name = -800;

			// Token: 0x040009BE RID: 2494
			public const int DISPID_Delete = -801;

			// Token: 0x040009BF RID: 2495
			public const int DISPID_Object = -802;

			// Token: 0x040009C0 RID: 2496
			public const int DISPID_Parent = -803;

			// Token: 0x040009C1 RID: 2497
			public const int DVASPECT_CONTENT = 1;

			// Token: 0x040009C2 RID: 2498
			public const int DVASPECT_THUMBNAIL = 2;

			// Token: 0x040009C3 RID: 2499
			public const int DVASPECT_ICON = 4;

			// Token: 0x040009C4 RID: 2500
			public const int DVASPECT_DOCPRINT = 8;

			// Token: 0x040009C5 RID: 2501
			public const int OLEMISC_RECOMPOSEONRESIZE = 1;

			// Token: 0x040009C6 RID: 2502
			public const int OLEMISC_ONLYICONIC = 2;

			// Token: 0x040009C7 RID: 2503
			public const int OLEMISC_INSERTNOTREPLACE = 4;

			// Token: 0x040009C8 RID: 2504
			public const int OLEMISC_STATIC = 8;

			// Token: 0x040009C9 RID: 2505
			public const int OLEMISC_CANTLINKINSIDE = 16;

			// Token: 0x040009CA RID: 2506
			public const int OLEMISC_CANLINKBYOLE1 = 32;

			// Token: 0x040009CB RID: 2507
			public const int OLEMISC_ISLINKOBJECT = 64;

			// Token: 0x040009CC RID: 2508
			public const int OLEMISC_INSIDEOUT = 128;

			// Token: 0x040009CD RID: 2509
			public const int OLEMISC_ACTIVATEWHENVISIBLE = 256;

			// Token: 0x040009CE RID: 2510
			public const int OLEMISC_RENDERINGISDEVICEINDEPENDENT = 512;

			// Token: 0x040009CF RID: 2511
			public const int OLEMISC_INVISIBLEATRUNTIME = 1024;

			// Token: 0x040009D0 RID: 2512
			public const int OLEMISC_ALWAYSRUN = 2048;

			// Token: 0x040009D1 RID: 2513
			public const int OLEMISC_ACTSLIKEBUTTON = 4096;

			// Token: 0x040009D2 RID: 2514
			public const int OLEMISC_ACTSLIKELABEL = 8192;

			// Token: 0x040009D3 RID: 2515
			public const int OLEMISC_NOUIACTIVATE = 16384;

			// Token: 0x040009D4 RID: 2516
			public const int OLEMISC_ALIGNABLE = 32768;

			// Token: 0x040009D5 RID: 2517
			public const int OLEMISC_SIMPLEFRAME = 65536;

			// Token: 0x040009D6 RID: 2518
			public const int OLEMISC_SETCLIENTSITEFIRST = 131072;

			// Token: 0x040009D7 RID: 2519
			public const int OLEMISC_IMEMODE = 262144;

			// Token: 0x040009D8 RID: 2520
			public const int OLEMISC_IGNOREACTIVATEWHENVISIBLE = 524288;

			// Token: 0x040009D9 RID: 2521
			public const int OLEMISC_WANTSTOMENUMERGE = 1048576;

			// Token: 0x040009DA RID: 2522
			public const int OLEMISC_SUPPORTSMULTILEVELUNDO = 2097152;

			// Token: 0x040009DB RID: 2523
			public const int QACONTAINER_SHOWHATCHING = 1;

			// Token: 0x040009DC RID: 2524
			public const int QACONTAINER_SHOWGRABHANDLES = 2;

			// Token: 0x040009DD RID: 2525
			public const int QACONTAINER_USERMODE = 4;

			// Token: 0x040009DE RID: 2526
			public const int QACONTAINER_DISPLAYASDEFAULT = 8;

			// Token: 0x040009DF RID: 2527
			public const int QACONTAINER_UIDEAD = 16;

			// Token: 0x040009E0 RID: 2528
			public const int QACONTAINER_AUTOCLIP = 32;

			// Token: 0x040009E1 RID: 2529
			public const int QACONTAINER_MESSAGEREFLECT = 64;

			// Token: 0x040009E2 RID: 2530
			public const int QACONTAINER_SUPPORTSMNEMONICS = 128;

			// Token: 0x040009E3 RID: 2531
			public const int XFORMCOORDS_POSITION = 1;

			// Token: 0x040009E4 RID: 2532
			public const int XFORMCOORDS_SIZE = 2;

			// Token: 0x040009E5 RID: 2533
			public const int XFORMCOORDS_HIMETRICTOCONTAINER = 4;

			// Token: 0x040009E6 RID: 2534
			public const int XFORMCOORDS_CONTAINERTOHIMETRIC = 8;

			// Token: 0x040009E7 RID: 2535
			public const int PROPCAT_Nil = -1;

			// Token: 0x040009E8 RID: 2536
			public const int PROPCAT_Misc = -2;

			// Token: 0x040009E9 RID: 2537
			public const int PROPCAT_Font = -3;

			// Token: 0x040009EA RID: 2538
			public const int PROPCAT_Position = -4;

			// Token: 0x040009EB RID: 2539
			public const int PROPCAT_Appearance = -5;

			// Token: 0x040009EC RID: 2540
			public const int PROPCAT_Behavior = -6;

			// Token: 0x040009ED RID: 2541
			public const int PROPCAT_Data = -7;

			// Token: 0x040009EE RID: 2542
			public const int PROPCAT_List = -8;

			// Token: 0x040009EF RID: 2543
			public const int PROPCAT_Text = -9;

			// Token: 0x040009F0 RID: 2544
			public const int PROPCAT_Scale = -10;

			// Token: 0x040009F1 RID: 2545
			public const int PROPCAT_DDE = -11;

			// Token: 0x040009F2 RID: 2546
			public const int GC_WCH_SIBLING = 1;

			// Token: 0x040009F3 RID: 2547
			public const int GC_WCH_CONTAINER = 2;

			// Token: 0x040009F4 RID: 2548
			public const int GC_WCH_CONTAINED = 3;

			// Token: 0x040009F5 RID: 2549
			public const int GC_WCH_ALL = 4;

			// Token: 0x040009F6 RID: 2550
			public const int GC_WCH_FREVERSEDIR = 134217728;

			// Token: 0x040009F7 RID: 2551
			public const int GC_WCH_FONLYNEXT = 268435456;

			// Token: 0x040009F8 RID: 2552
			public const int GC_WCH_FONLYPREV = 536870912;

			// Token: 0x040009F9 RID: 2553
			public const int GC_WCH_FSELECTED = 1073741824;

			// Token: 0x040009FA RID: 2554
			public const int OLECONTF_EMBEDDINGS = 1;

			// Token: 0x040009FB RID: 2555
			public const int OLECONTF_LINKS = 2;

			// Token: 0x040009FC RID: 2556
			public const int OLECONTF_OTHERS = 4;

			// Token: 0x040009FD RID: 2557
			public const int OLECONTF_ONLYUSER = 8;

			// Token: 0x040009FE RID: 2558
			public const int OLECONTF_ONLYIFRUNNING = 16;

			// Token: 0x040009FF RID: 2559
			public const int ALIGN_MIN = 0;

			// Token: 0x04000A00 RID: 2560
			public const int ALIGN_NO_CHANGE = 0;

			// Token: 0x04000A01 RID: 2561
			public const int ALIGN_TOP = 1;

			// Token: 0x04000A02 RID: 2562
			public const int ALIGN_BOTTOM = 2;

			// Token: 0x04000A03 RID: 2563
			public const int ALIGN_LEFT = 3;

			// Token: 0x04000A04 RID: 2564
			public const int ALIGN_RIGHT = 4;

			// Token: 0x04000A05 RID: 2565
			public const int ALIGN_MAX = 4;

			// Token: 0x04000A06 RID: 2566
			public const int OLEVERBATTRIB_NEVERDIRTIES = 1;

			// Token: 0x04000A07 RID: 2567
			public const int OLEVERBATTRIB_ONCONTAINERMENU = 2;

			// Token: 0x04000A08 RID: 2568
			public static Guid IID_IUnknown = new Guid("{00000000-0000-0000-C000-000000000046}");
		}

		// Token: 0x02000050 RID: 80
		[Guid("00000104-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IEnumOLEVERB
		{
			// Token: 0x060003B1 RID: 945
			[PreserveSig]
			int Next([MarshalAs(UnmanagedType.U4)] int celt, [In] [Out] NativeMethods.tagOLEVERB rgelt, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pceltFetched);

			// Token: 0x060003B2 RID: 946
			[PreserveSig]
			int Skip([MarshalAs(UnmanagedType.U4)] [In] int celt);

			// Token: 0x060003B3 RID: 947
			void Reset();

			// Token: 0x060003B4 RID: 948
			void Clone(out NativeMethods.IEnumOLEVERB ppenum);
		}

		// Token: 0x02000051 RID: 81
		[Guid("00000105-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IEnumSTATDATA
		{
			// Token: 0x060003B5 RID: 949
			void Next([MarshalAs(UnmanagedType.U4)] [In] int celt, [Out] NativeMethods.STATDATA rgelt, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pceltFetched);

			// Token: 0x060003B6 RID: 950
			void Skip([MarshalAs(UnmanagedType.U4)] [In] int celt);

			// Token: 0x060003B7 RID: 951
			void Reset();

			// Token: 0x060003B8 RID: 952
			void Clone([MarshalAs(UnmanagedType.LPArray)] [Out] NativeMethods.IEnumSTATDATA[] ppenum);
		}

		// Token: 0x02000052 RID: 82
		[StructLayout(LayoutKind.Sequential)]
		public sealed class STATDATA
		{
			// Token: 0x04000A09 RID: 2569
			[MarshalAs(UnmanagedType.U4)]
			public int advf;

			// Token: 0x04000A0A RID: 2570
			[MarshalAs(UnmanagedType.U4)]
			public int dwConnection;
		}

		// Token: 0x02000053 RID: 83
		[StructLayout(LayoutKind.Sequential)]
		public class CHARRANGE
		{
			// Token: 0x04000A0B RID: 2571
			public int cpMin;

			// Token: 0x04000A0C RID: 2572
			public int cpMax;
		}

		// Token: 0x02000054 RID: 84
		[StructLayout(LayoutKind.Sequential)]
		public class STATSTG
		{
			// Token: 0x04000A0D RID: 2573
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pwcsName;

			// Token: 0x04000A0E RID: 2574
			public int type;

			// Token: 0x04000A0F RID: 2575
			[MarshalAs(UnmanagedType.I8)]
			public long cbSize;

			// Token: 0x04000A10 RID: 2576
			[MarshalAs(UnmanagedType.I8)]
			public long mtime;

			// Token: 0x04000A11 RID: 2577
			[MarshalAs(UnmanagedType.I8)]
			public long ctime;

			// Token: 0x04000A12 RID: 2578
			[MarshalAs(UnmanagedType.I8)]
			public long atime;

			// Token: 0x04000A13 RID: 2579
			[MarshalAs(UnmanagedType.I4)]
			public int grfMode;

			// Token: 0x04000A14 RID: 2580
			[MarshalAs(UnmanagedType.I4)]
			public int grfLocksSupported;

			// Token: 0x04000A15 RID: 2581
			public int clsid_data1;

			// Token: 0x04000A16 RID: 2582
			[MarshalAs(UnmanagedType.I2)]
			public short clsid_data2;

			// Token: 0x04000A17 RID: 2583
			[MarshalAs(UnmanagedType.I2)]
			public short clsid_data3;

			// Token: 0x04000A18 RID: 2584
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b0;

			// Token: 0x04000A19 RID: 2585
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b1;

			// Token: 0x04000A1A RID: 2586
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b2;

			// Token: 0x04000A1B RID: 2587
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b3;

			// Token: 0x04000A1C RID: 2588
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b4;

			// Token: 0x04000A1D RID: 2589
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b5;

			// Token: 0x04000A1E RID: 2590
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b6;

			// Token: 0x04000A1F RID: 2591
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b7;

			// Token: 0x04000A20 RID: 2592
			[MarshalAs(UnmanagedType.I4)]
			public int grfStateBits;

			// Token: 0x04000A21 RID: 2593
			[MarshalAs(UnmanagedType.I4)]
			public int reserved;
		}

		// Token: 0x02000055 RID: 85
		[StructLayout(LayoutKind.Sequential)]
		public class FILETIME
		{
			// Token: 0x04000A22 RID: 2594
			public int dwLowDateTime;

			// Token: 0x04000A23 RID: 2595
			public int dwHighDateTime;
		}

		// Token: 0x02000056 RID: 86
		[Guid("00000103-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IEnumFORMATETC
		{
			// Token: 0x060003BD RID: 957
			[PreserveSig]
			int Next([MarshalAs(UnmanagedType.U4)] [In] int celt, [Out] NativeMethods.FORMATETC rgelt, [MarshalAs(UnmanagedType.LPArray)] [In] [Out] int[] pceltFetched);

			// Token: 0x060003BE RID: 958
			[PreserveSig]
			int Skip([MarshalAs(UnmanagedType.U4)] [In] int celt);

			// Token: 0x060003BF RID: 959
			[PreserveSig]
			int Reset();

			// Token: 0x060003C0 RID: 960
			[PreserveSig]
			int Clone([MarshalAs(UnmanagedType.LPArray)] [Out] NativeMethods.IEnumFORMATETC[] ppenum);
		}

		// Token: 0x02000057 RID: 87
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class LOGFONT
		{
			// Token: 0x060003C1 RID: 961 RVA: 0x00002C29 File Offset: 0x00001C29
			public LOGFONT()
			{
			}

			// Token: 0x060003C2 RID: 962 RVA: 0x00002C34 File Offset: 0x00001C34
			public LOGFONT(NativeMethods.LOGFONT lf)
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

			// Token: 0x060003C3 RID: 963 RVA: 0x00002CF0 File Offset: 0x00001CF0
			public override string ToString()
			{
				return string.Concat(new object[]
				{
					"lfHeight=", this.lfHeight, ", lfWidth=", this.lfWidth, ", lfEscapement=", this.lfEscapement, ", lfOrientation=", this.lfOrientation, ", lfWeight=", this.lfWeight,
					", lfItalic=", this.lfItalic, ", lfUnderline=", this.lfUnderline, ", lfStrikeOut=", this.lfStrikeOut, ", lfCharSet=", this.lfCharSet, ", lfOutPrecision=", this.lfOutPrecision,
					", lfClipPrecision=", this.lfClipPrecision, ", lfQuality=", this.lfQuality, ", lfPitchAndFamily=", this.lfPitchAndFamily, ", lfFaceName=", this.lfFaceName
				});
			}

			// Token: 0x04000A24 RID: 2596
			public int lfHeight;

			// Token: 0x04000A25 RID: 2597
			public int lfWidth;

			// Token: 0x04000A26 RID: 2598
			public int lfEscapement;

			// Token: 0x04000A27 RID: 2599
			public int lfOrientation;

			// Token: 0x04000A28 RID: 2600
			public int lfWeight;

			// Token: 0x04000A29 RID: 2601
			public byte lfItalic;

			// Token: 0x04000A2A RID: 2602
			public byte lfUnderline;

			// Token: 0x04000A2B RID: 2603
			public byte lfStrikeOut;

			// Token: 0x04000A2C RID: 2604
			public byte lfCharSet;

			// Token: 0x04000A2D RID: 2605
			public byte lfOutPrecision;

			// Token: 0x04000A2E RID: 2606
			public byte lfClipPrecision;

			// Token: 0x04000A2F RID: 2607
			public byte lfQuality;

			// Token: 0x04000A30 RID: 2608
			public byte lfPitchAndFamily;

			// Token: 0x04000A31 RID: 2609
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string lfFaceName;
		}

		// Token: 0x02000058 RID: 88
		[StructLayout(LayoutKind.Sequential)]
		public class NONCLIENTMETRICS
		{
			// Token: 0x04000A32 RID: 2610
			public int cbSize = Marshal.SizeOf(typeof(NativeMethods.NONCLIENTMETRICS));

			// Token: 0x04000A33 RID: 2611
			public int iBorderWidth;

			// Token: 0x04000A34 RID: 2612
			public int iScrollWidth;

			// Token: 0x04000A35 RID: 2613
			public int iScrollHeight;

			// Token: 0x04000A36 RID: 2614
			public int iCaptionWidth;

			// Token: 0x04000A37 RID: 2615
			public int iCaptionHeight;

			// Token: 0x04000A38 RID: 2616
			[MarshalAs(UnmanagedType.Struct)]
			public NativeMethods.LOGFONT lfCaptionFont;

			// Token: 0x04000A39 RID: 2617
			public int iSmCaptionWidth;

			// Token: 0x04000A3A RID: 2618
			public int iSmCaptionHeight;

			// Token: 0x04000A3B RID: 2619
			[MarshalAs(UnmanagedType.Struct)]
			public NativeMethods.LOGFONT lfSmCaptionFont;

			// Token: 0x04000A3C RID: 2620
			public int iMenuWidth;

			// Token: 0x04000A3D RID: 2621
			public int iMenuHeight;

			// Token: 0x04000A3E RID: 2622
			[MarshalAs(UnmanagedType.Struct)]
			public NativeMethods.LOGFONT lfMenuFont;

			// Token: 0x04000A3F RID: 2623
			[MarshalAs(UnmanagedType.Struct)]
			public NativeMethods.LOGFONT lfStatusFont;

			// Token: 0x04000A40 RID: 2624
			[MarshalAs(UnmanagedType.Struct)]
			public NativeMethods.LOGFONT lfMessageFont;
		}
	}
}
