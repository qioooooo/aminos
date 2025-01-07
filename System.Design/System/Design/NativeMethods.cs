using System;
using System.Drawing;
using System.Internal;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Design
{
	internal class NativeMethods
	{
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern int MultiByteToWideChar(int CodePage, int dwFlags, byte[] lpMultiByteStr, int cchMultiByte, char[] lpWideCharStr, int cchWideChar);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetClientRect(IntPtr hWnd, [In] [Out] NativeMethods.COMRECT rect);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool PeekMessage([In] [Out] ref NativeMethods.MSG msg, IntPtr hwnd, int msgMin, int msgMax, int remove);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetCursor();

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetCursorPos([In] [Out] NativeMethods.POINT pt);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr WindowFromPoint(int x, int y);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, [In] [Out] NativeMethods.HDHITTESTINFO lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, [In] [Out] NativeMethods.TV_HITTESTINFO lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr DefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetWindow(IntPtr hWnd, int uCmd);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern short GetKeyState(int keyCode);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [In] [Out] ref NativeMethods.RECT rect, int cPoints);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [In] [Out] NativeMethods.POINT pt, int cPoints);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool ValidateRect(IntPtr hwnd, IntPtr prect);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateRectRgn", ExactSpelling = true)]
		private static extern IntPtr IntCreateRectRgn(int x1, int y1, int x2, int y2);

		public static IntPtr CreateRectRgn(int x1, int y1, int x2, int y2)
		{
			return global::System.Internal.HandleCollector.Add(NativeMethods.IntCreateRectRgn(x1, y1, x2, y2), NativeMethods.CommonHandles.GDI);
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetUpdateRect(IntPtr hwnd, [In] [Out] ref NativeMethods.RECT rc, bool fErase);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetUpdateRgn(IntPtr hwnd, IntPtr hrgn, bool fErase);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "DeleteObject", ExactSpelling = true)]
		public static extern bool ExternalDeleteObject(HandleRef hObject);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "DeleteObject", ExactSpelling = true)]
		private static extern bool IntDeleteObject(IntPtr hObject);

		public static bool DeleteObject(IntPtr hObject)
		{
			global::System.Internal.HandleCollector.Remove(hObject, NativeMethods.CommonHandles.GDI);
			return NativeMethods.IntDeleteObject(hObject);
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr SetParent(IntPtr hWnd, IntPtr hWndParent);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool TranslateMessage([In] [Out] ref NativeMethods.MSG msg);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int DispatchMessage([In] ref NativeMethods.MSG msg);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetWindowRect(IntPtr hWnd, [In] [Out] ref NativeMethods.RECT rect);

		[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int RevokeDragDrop(IntPtr hwnd);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr ChildWindowFromPointEx(IntPtr hwndParent, int x, int y, int uFlags);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool IsWindowVisible(IntPtr hWnd);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetFocus();

		public static bool Succeeded(int hr)
		{
			return hr >= 0;
		}

		public static bool Failed(int hr)
		{
			return hr < 0;
		}

		[DllImport("oleaut32.dll", PreserveSig = false)]
		public static extern ITypeLib LoadRegTypeLib(ref Guid clsid, short majorVersion, short minorVersion, int lcid);

		[DllImport("oleaut32.dll", PreserveSig = false)]
		public static extern ITypeLib LoadTypeLib([MarshalAs(UnmanagedType.LPWStr)] [In] string typelib);

		[DllImport("oleaut32.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.BStr)]
		public static extern string QueryPathOfRegTypeLib(ref Guid guid, short majorVersion, short minorVersion, int lcid);

		public const int HOLLOW_BRUSH = 5;

		public const int WM_USER = 1024;

		public const int WM_CLOSE = 16;

		public const int WM_GETDLGCODE = 135;

		public const int WM_MOUSEMOVE = 512;

		public const int WM_NOTIFY = 78;

		public const int DLGC_WANTALLKEYS = 4;

		public const int NM_CLICK = -2;

		public const int WM_REFLECT = 8192;

		public const int BM_SETIMAGE = 247;

		public const int IMAGE_ICON = 1;

		public const int WM_DESTROY = 2;

		public const int BS_ICON = 64;

		public const int VK_PROCESSKEY = 229;

		public const int STGM_READ = 0;

		public const int STGM_WRITE = 1;

		public const int STGM_READWRITE = 2;

		public const int STGM_SHARE_EXCLUSIVE = 16;

		public const int STGM_CREATE = 4096;

		public const int STGM_TRANSACTED = 65536;

		public const int STGM_CONVERT = 131072;

		public const int STGM_DELETEONRELEASE = 67108864;

		public const int RECO_PASTE = 0;

		public const int RECO_DROP = 1;

		public const int TCM_HITTEST = 4877;

		public const int S_OK = 0;

		public const int S_FALSE = 1;

		public const int E_NOTIMPL = -2147467263;

		public const int E_NOINTERFACE = -2147467262;

		public const int E_INVALIDARG = -2147024809;

		public const int E_FAIL = -2147467259;

		public const int WS_EX_STATICEDGE = 131072;

		public const int OLEIVERB_PRIMARY = 0;

		public const int OLEIVERB_SHOW = -1;

		public const int OLEIVERB_OPEN = -2;

		public const int OLEIVERB_HIDE = -3;

		public const int OLEIVERB_UIACTIVATE = -4;

		public const int OLEIVERB_INPLACEACTIVATE = -5;

		public const int OLEIVERB_DISCARDUNDOSTATE = -6;

		public const int OLEIVERB_PROPERTIES = -7;

		public const int OLECLOSE_SAVEIFDIRTY = 0;

		public const int OLECLOSE_NOSAVE = 1;

		public const int OLECLOSE_PROMPTSAVE = 2;

		public const int PM_NOREMOVE = 0;

		public const int PM_REMOVE = 1;

		public const int WM_CHAR = 258;

		public const int DT_CALCRECT = 1024;

		public const int WM_CAPTURECHANGED = 533;

		public const int WM_PARENTNOTIFY = 528;

		public const int WM_CREATE = 1;

		public const int WM_SETREDRAW = 11;

		public const int WM_NCACTIVATE = 134;

		public const int WM_HSCROLL = 276;

		public const int WM_VSCROLL = 277;

		public const int WM_SHOWWINDOW = 24;

		public const int WM_WINDOWPOSCHANGING = 70;

		public const int WM_WINDOWPOSCHANGED = 71;

		public const int WS_DISABLED = 134217728;

		public const int WS_CLIPSIBLINGS = 67108864;

		public const int WS_CLIPCHILDREN = 33554432;

		public const int WS_EX_TOOLWINDOW = 128;

		public const int WS_POPUP = -2147483648;

		public const int WS_BORDER = 8388608;

		public const int CS_DROPSHADOW = 131072;

		public const int CS_DBLCLKS = 8;

		public const int NOTSRCCOPY = 3342344;

		public const int SRCCOPY = 13369376;

		public const int LVM_SETCOLUMNWIDTH = 4126;

		public const int LVM_GETHEADER = 4127;

		public const int LVM_CREATEDRAGIMAGE = 4129;

		public const int LVM_GETVIEWRECT = 4130;

		public const int LVM_GETTEXTCOLOR = 4131;

		public const int LVM_SETTEXTCOLOR = 4132;

		public const int LVM_GETTEXTBKCOLOR = 4133;

		public const int LVM_SETTEXTBKCOLOR = 4134;

		public const int LVM_GETTOPINDEX = 4135;

		public const int LVM_GETCOUNTPERPAGE = 4136;

		public const int LVM_GETORIGIN = 4137;

		public const int LVM_UPDATE = 4138;

		public const int LVM_SETITEMSTATE = 4139;

		public const int LVM_GETITEMSTATE = 4140;

		public const int LVM_GETITEMTEXTA = 4141;

		public const int LVM_GETITEMTEXTW = 4211;

		public const int LVM_SETITEMTEXTA = 4142;

		public const int LVM_SETITEMTEXTW = 4212;

		public const int LVSICF_NOINVALIDATEALL = 1;

		public const int LVSICF_NOSCROLL = 2;

		public const int LVM_SETITEMCOUNT = 4143;

		public const int LVM_SORTITEMS = 4144;

		public const int LVM_SETITEMPOSITION32 = 4145;

		public const int LVM_GETSELECTEDCOUNT = 4146;

		public const int LVM_GETITEMSPACING = 4147;

		public const int LVM_GETISEARCHSTRINGA = 4148;

		public const int LVM_GETISEARCHSTRINGW = 4213;

		public const int LVM_SETICONSPACING = 4149;

		public const int LVM_SETEXTENDEDLISTVIEWSTYLE = 4150;

		public const int LVM_GETEXTENDEDLISTVIEWSTYLE = 4151;

		public const int LVS_EX_GRIDLINES = 1;

		public const int HDM_HITTEST = 4614;

		public const int HDM_GETITEMRECT = 4615;

		public const int HDM_SETIMAGELIST = 4616;

		public const int HDM_GETIMAGELIST = 4617;

		public const int HDM_ORDERTOINDEX = 4623;

		public const int HDM_CREATEDRAGIMAGE = 4624;

		public const int HDM_GETORDERARRAY = 4625;

		public const int HDM_SETORDERARRAY = 4626;

		public const int HDM_SETHOTDIVIDER = 4627;

		public const int HDN_ITEMCHANGINGA = -300;

		public const int HDN_ITEMCHANGINGW = -320;

		public const int HDN_ITEMCHANGEDA = -301;

		public const int HDN_ITEMCHANGEDW = -321;

		public const int HDN_ITEMCLICKA = -302;

		public const int HDN_ITEMCLICKW = -322;

		public const int HDN_ITEMDBLCLICKA = -303;

		public const int HDN_ITEMDBLCLICKW = -323;

		public const int HDN_DIVIDERDBLCLICKA = -305;

		public const int HDN_DIVIDERDBLCLICKW = -325;

		public const int HDN_BEGINTRACKA = -306;

		public const int HDN_BEGINTRACKW = -326;

		public const int HDN_ENDTRACKA = -307;

		public const int HDN_ENDTRACKW = -327;

		public const int HDN_TRACKA = -308;

		public const int HDN_TRACKW = -328;

		public const int HDN_GETDISPINFOA = -309;

		public const int HDN_GETDISPINFOW = -329;

		public const int HDN_BEGINDRAG = -310;

		public const int HDN_ENDDRAG = -311;

		public const int HC_ACTION = 0;

		public const int HIST_BACK = 0;

		public const int HHT_ONHEADER = 2;

		public const int HHT_ONDIVIDER = 4;

		public const int HHT_ONDIVOPEN = 8;

		public const int HHT_ABOVE = 256;

		public const int HHT_BELOW = 512;

		public const int HHT_TORIGHT = 1024;

		public const int HHT_TOLEFT = 2048;

		public const int HWND_TOP = 0;

		public const int HWND_BOTTOM = 1;

		public const int HWND_TOPMOST = -1;

		public const int HWND_NOTOPMOST = -2;

		public const int CWP_SKIPINVISIBLE = 1;

		public const int RDW_FRAME = 1024;

		public const int WM_KILLFOCUS = 8;

		public const int WM_STYLECHANGED = 125;

		public const int TVM_GETITEMRECT = 4356;

		public const int TVM_GETCOUNT = 4357;

		public const int TVM_GETINDENT = 4358;

		public const int TVM_SETINDENT = 4359;

		public const int TVM_GETIMAGELIST = 4360;

		public const int TVSIL_NORMAL = 0;

		public const int TVSIL_STATE = 2;

		public const int TVM_SETIMAGELIST = 4361;

		public const int TVM_GETNEXTITEM = 4362;

		public const int TVGN_ROOT = 0;

		public const int TVHT_ONITEMICON = 2;

		public const int TVHT_ONITEMLABEL = 4;

		public const int TVHT_ONITEMINDENT = 8;

		public const int TVHT_ONITEMBUTTON = 16;

		public const int TVHT_ONITEMRIGHT = 32;

		public const int TVHT_ONITEMSTATEICON = 64;

		public const int TVHT_ABOVE = 256;

		public const int TVHT_BELOW = 512;

		public const int TVHT_TORIGHT = 1024;

		public const int TVHT_TOLEFT = 2048;

		public const int GW_HWNDFIRST = 0;

		public const int GW_HWNDLAST = 1;

		public const int GW_HWNDNEXT = 2;

		public const int GW_HWNDPREV = 3;

		public const int GW_OWNER = 4;

		public const int GW_CHILD = 5;

		public const int GW_MAX = 5;

		public const int GWL_HWNDPARENT = -8;

		public const int SB_HORZ = 0;

		public const int SB_VERT = 1;

		public const int SB_CTL = 2;

		public const int SB_BOTH = 3;

		public const int SB_LINEUP = 0;

		public const int SB_LINELEFT = 0;

		public const int SB_LINEDOWN = 1;

		public const int SB_LINERIGHT = 1;

		public const int SB_PAGEUP = 2;

		public const int SB_PAGELEFT = 2;

		public const int SB_PAGEDOWN = 3;

		public const int SB_PAGERIGHT = 3;

		public const int SB_THUMBPOSITION = 4;

		public const int SB_THUMBTRACK = 5;

		public const int SB_TOP = 6;

		public const int SB_LEFT = 6;

		public const int SB_BOTTOM = 7;

		public const int SB_RIGHT = 7;

		public const int SB_ENDSCROLL = 8;

		public const int MK_LBUTTON = 1;

		public const int TVM_HITTEST = 4369;

		public const int MK_RBUTTON = 2;

		public const int MK_SHIFT = 4;

		public const int MK_CONTROL = 8;

		public const int MK_MBUTTON = 16;

		public const int MK_XBUTTON1 = 32;

		public const int MK_XBUTTON2 = 64;

		public const int LB_ADDSTRING = 384;

		public const int LB_INSERTSTRING = 385;

		public const int LB_DELETESTRING = 386;

		public const int LB_SELITEMRANGEEX = 387;

		public const int LB_RESETCONTENT = 388;

		public const int LB_SETSEL = 389;

		public const int LB_SETCURSEL = 390;

		public const int LB_GETSEL = 391;

		public const int LB_GETCURSEL = 392;

		public const int LB_GETTEXT = 393;

		public const int LB_GETTEXTLEN = 394;

		public const int LB_GETCOUNT = 395;

		public const int LB_SELECTSTRING = 396;

		public const int LB_DIR = 397;

		public const int LB_GETTOPINDEX = 398;

		public const int LB_FINDSTRING = 399;

		public const int LB_GETSELCOUNT = 400;

		public const int LB_GETSELITEMS = 401;

		public const int LB_SETTABSTOPS = 402;

		public const int LB_GETHORIZONTALEXTENT = 403;

		public const int LB_SETHORIZONTALEXTENT = 404;

		public const int LB_SETCOLUMNWIDTH = 405;

		public const int LB_ADDFILE = 406;

		public const int LB_SETTOPINDEX = 407;

		public const int LB_GETITEMRECT = 408;

		public const int LB_GETITEMDATA = 409;

		public const int LB_SETITEMDATA = 410;

		public const int LB_SELITEMRANGE = 411;

		public const int LB_SETANCHORINDEX = 412;

		public const int LB_GETANCHORINDEX = 413;

		public const int LB_SETCARETINDEX = 414;

		public const int LB_GETCARETINDEX = 415;

		public const int LB_SETITEMHEIGHT = 416;

		public const int LB_GETITEMHEIGHT = 417;

		public const int LB_FINDSTRINGEXACT = 418;

		public const int LB_SETLOCALE = 421;

		public const int LB_GETLOCALE = 422;

		public const int LB_SETCOUNT = 423;

		public const int LB_INITSTORAGE = 424;

		public const int LB_ITEMFROMPOINT = 425;

		public const int LB_MSGMAX = 432;

		public const int HTHSCROLL = 6;

		public const int HTVSCROLL = 7;

		public const int HTERROR = -2;

		public const int HTTRANSPARENT = -1;

		public const int HTNOWHERE = 0;

		public const int HTCLIENT = 1;

		public const int HTCAPTION = 2;

		public const int HTSYSMENU = 3;

		public const int HTGROWBOX = 4;

		public const int HTSIZE = 4;

		public const int PRF_NONCLIENT = 2;

		public const int PRF_CLIENT = 4;

		public const int PRF_ERASEBKGND = 8;

		public const int PRF_CHILDREN = 16;

		public const int SWP_NOSIZE = 1;

		public const int SWP_NOMOVE = 2;

		public const int SWP_NOZORDER = 4;

		public const int SWP_NOREDRAW = 8;

		public const int SWP_NOACTIVATE = 16;

		public const int SWP_FRAMECHANGED = 32;

		public const int SWP_SHOWWINDOW = 64;

		public const int SWP_HIDEWINDOW = 128;

		public const int SWP_NOCOPYBITS = 256;

		public const int SWP_NOOWNERZORDER = 512;

		public const int SWP_NOSENDCHANGING = 1024;

		public const int SWP_DRAWFRAME = 32;

		public const int SWP_NOREPOSITION = 512;

		public const int SWP_DEFERERASE = 8192;

		public const int SWP_ASYNCWINDOWPOS = 16384;

		public const int WA_INACTIVE = 0;

		public const int WA_ACTIVE = 1;

		public const int WH_MOUSE = 7;

		public const int WM_IME_STARTCOMPOSITION = 269;

		public const int WM_IME_ENDCOMPOSITION = 270;

		public const int WM_IME_COMPOSITION = 271;

		public const int WM_ACTIVATE = 6;

		public const int WM_NCMOUSEMOVE = 160;

		public const int WM_NCLBUTTONDOWN = 161;

		public const int WM_NCLBUTTONUP = 162;

		public const int WM_NCLBUTTONDBLCLK = 163;

		public const int WM_NCRBUTTONDOWN = 164;

		public const int WM_NCRBUTTONUP = 165;

		public const int WM_NCRBUTTONDBLCLK = 166;

		public const int WM_NCMBUTTONDOWN = 167;

		public const int WM_NCMBUTTONUP = 168;

		public const int WM_NCMBUTTONDBLCLK = 169;

		public const int WM_NCXBUTTONDOWN = 171;

		public const int WM_NCXBUTTONUP = 172;

		public const int WM_NCXBUTTONDBLCLK = 173;

		public const int WM_MOUSEHOVER = 673;

		public const int WM_MOUSELEAVE = 675;

		public const int WM_MOUSEFIRST = 512;

		public const int WM_MOUSEACTIVATE = 33;

		public const int WM_LBUTTONDOWN = 513;

		public const int WM_LBUTTONUP = 514;

		public const int WM_LBUTTONDBLCLK = 515;

		public const int WM_RBUTTONDOWN = 516;

		public const int WM_RBUTTONUP = 517;

		public const int WM_RBUTTONDBLCLK = 518;

		public const int WM_MBUTTONDOWN = 519;

		public const int WM_MBUTTONUP = 520;

		public const int WM_MBUTTONDBLCLK = 521;

		public const int WM_NCMOUSEHOVER = 672;

		public const int WM_NCMOUSELEAVE = 674;

		public const int WM_MOUSEWHEEL = 522;

		public const int WM_MOUSELAST = 522;

		public const int WM_NCHITTEST = 132;

		public const int WM_SETCURSOR = 32;

		public const int WM_GETOBJECT = 61;

		public const int WM_CANCELMODE = 31;

		public const int WM_SETFOCUS = 7;

		public const int WM_KEYFIRST = 256;

		public const int WM_KEYDOWN = 256;

		public const int WM_KEYUP = 257;

		public const int WM_DEADCHAR = 259;

		public const int WM_SYSKEYDOWN = 260;

		public const int WM_SYSKEYUP = 261;

		public const int WM_SYSCHAR = 262;

		public const int WM_SYSDEADCHAR = 263;

		public const int WM_KEYLAST = 264;

		public const int WM_CONTEXTMENU = 123;

		public const int WM_PAINT = 15;

		public const int WM_PRINTCLIENT = 792;

		public const int WM_NCPAINT = 133;

		public const int WM_SIZE = 5;

		public const int WM_TIMER = 275;

		public const int WM_PRINT = 791;

		public const int CHILDID_SELF = 0;

		public const int OBJID_WINDOW = 0;

		public const int OBJID_CLIENT = -4;

		public const string uuid_IAccessible = "{618736E0-3C3D-11CF-810C-00AA00389B71}";

		public const string uuid_IEnumVariant = "{00020404-0000-0000-C000-000000000046}";

		public const int QS_KEY = 1;

		public const int QS_MOUSEMOVE = 2;

		public const int QS_MOUSEBUTTON = 4;

		public const int QS_POSTMESSAGE = 8;

		public const int QS_TIMER = 16;

		public const int QS_PAINT = 32;

		public const int QS_SENDMESSAGE = 64;

		public const int QS_HOTKEY = 128;

		public const int QS_ALLPOSTMESSAGE = 256;

		public const int QS_MOUSE = 6;

		public const int QS_INPUT = 7;

		public const int QS_ALLEVENTS = 191;

		public const int QS_ALLINPUT = 255;

		public const int MWMO_INPUTAVAILABLE = 4;

		public const int GWL_EXSTYLE = -20;

		public const int GWL_STYLE = -16;

		public const int WS_EX_LAYOUTRTL = 4194304;

		public const int SPI_GETNONCLIENTMETRICS = 41;

		public static HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);

		public static int PS_SOLID = 0;

		public static IntPtr InvalidIntPtr = (IntPtr)(-1);

		public static int TME_HOVER = 1;

		public static readonly int WM_MOUSEENTER = NativeMethods.Util.RegisterWindowMessage("WinFormsMouseEnter");

		public static readonly int HDN_ENDTRACK = ((Marshal.SystemDefaultCharSize == 1) ? (-307) : (-327));

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class TEXTMETRIC
		{
			public int tmHeight;

			public int tmAscent;

			public int tmDescent;

			public int tmInternalLeading;

			public int tmExternalLeading;

			public int tmAveCharWidth;

			public int tmMaxCharWidth;

			public int tmWeight;

			public int tmOverhang;

			public int tmDigitizedAspectX;

			public int tmDigitizedAspectY;

			public char tmFirstChar;

			public char tmLastChar;

			public char tmDefaultChar;

			public char tmBreakChar;

			public byte tmItalic;

			public byte tmUnderlined;

			public byte tmStruckOut;

			public byte tmPitchAndFamily;

			public byte tmCharSet;
		}

		public delegate bool EnumChildrenCallback(IntPtr hwnd, IntPtr lParam);

		[StructLayout(LayoutKind.Sequential)]
		public class NMHEADER
		{
			public int hwndFrom;

			public int idFrom;

			public int code;

			public int iItem;

			public int iButton;

			public int pItem;
		}

		[StructLayout(LayoutKind.Sequential)]
		public class POINT
		{
			public POINT()
			{
			}

			public POINT(int x, int y)
			{
				this.x = x;
				this.y = y;
			}

			public int x;

			public int y;
		}

		public struct WINDOWPOS
		{
			public IntPtr hwnd;

			public IntPtr hwndInsertAfter;

			public int x;

			public int y;

			public int cx;

			public int cy;

			public int flags;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
		public class TV_ITEM
		{
			public int mask;

			public int hItem;

			public int state;

			public int stateMask;

			public int pszText;

			public int cchTextMax;

			public int iImage;

			public int iSelectedImage;

			public int cChildren;

			public int lParam;
		}

		[StructLayout(LayoutKind.Sequential)]
		public class NMHDR
		{
			public int hwndFrom;

			public int idFrom;

			public int code;
		}

		[StructLayout(LayoutKind.Sequential)]
		public class NMTREEVIEW
		{
			public NativeMethods.NMHDR nmhdr;

			public int action;

			public NativeMethods.TV_ITEM itemOld;

			public NativeMethods.TV_ITEM itemNew;

			public NativeMethods.POINT ptDrag;
		}

		[StructLayout(LayoutKind.Sequential)]
		public class TCHITTESTINFO
		{
			public Point pt;

			public NativeMethods.TabControlHitTest flags;
		}

		[Flags]
		public enum TabControlHitTest
		{
			TCHT_NOWHERE = 1,
			TCHT_ONITEMICON = 2,
			TCHT_ONITEMLABEL = 4
		}

		[StructLayout(LayoutKind.Sequential)]
		public class TRACKMOUSEEVENT
		{
			public int cbSize = Marshal.SizeOf(typeof(NativeMethods.TRACKMOUSEEVENT));

			public int dwFlags;

			public IntPtr hwndTrack;

			public int dwHoverTime;
		}

		[ComVisible(false)]
		public enum StructFormat
		{
			Ansi = 1,
			Unicode,
			Auto
		}

		public struct MOUSEHOOKSTRUCT
		{
			public int pt_x;

			public int pt_y;

			public IntPtr hWnd;

			public int wHitTestCode;

			public int dwExtraInfo;
		}

		public struct MSG
		{
			public IntPtr hwnd;

			public int message;

			public IntPtr wParam;

			public IntPtr lParam;

			public int time;

			public int pt_x;

			public int pt_y;
		}

		[StructLayout(LayoutKind.Sequential)]
		public class COMRECT
		{
			public COMRECT()
			{
			}

			public COMRECT(int left, int top, int right, int bottom)
			{
				this.left = left;
				this.top = top;
				this.right = right;
				this.bottom = bottom;
			}

			public int left;

			public int top;

			public int right;

			public int bottom;
		}

		[StructLayout(LayoutKind.Sequential)]
		public sealed class FORMATETC
		{
			[MarshalAs(UnmanagedType.I4)]
			public int cfFormat;

			[MarshalAs(UnmanagedType.I4)]
			public IntPtr ptd = IntPtr.Zero;

			[MarshalAs(UnmanagedType.I4)]
			public int dwAspect;

			[MarshalAs(UnmanagedType.I4)]
			public int lindex;

			[MarshalAs(UnmanagedType.I4)]
			public int tymed;
		}

		[StructLayout(LayoutKind.Sequential)]
		public class STGMEDIUM
		{
			[MarshalAs(UnmanagedType.I4)]
			public int tymed;

			public IntPtr unionmember = IntPtr.Zero;

			public IntPtr pUnkForRelease = IntPtr.Zero;
		}

		public struct RECT
		{
			public RECT(int left, int top, int right, int bottom)
			{
				this.left = left;
				this.top = top;
				this.right = right;
				this.bottom = bottom;
			}

			public int left;

			public int top;

			public int right;

			public int bottom;
		}

		[StructLayout(LayoutKind.Sequential)]
		public sealed class OLECMD
		{
			[MarshalAs(UnmanagedType.U4)]
			public int cmdID;

			[MarshalAs(UnmanagedType.U4)]
			public int cmdf;
		}

		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagOIFI
		{
			[MarshalAs(UnmanagedType.U4)]
			public int cb;

			[MarshalAs(UnmanagedType.I4)]
			public int fMDIApp;

			public IntPtr hwndFrame;

			public IntPtr hAccel;

			[MarshalAs(UnmanagedType.U4)]
			public int cAccelEntries;
		}

		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagSIZE
		{
			[MarshalAs(UnmanagedType.I4)]
			public int cx;

			[MarshalAs(UnmanagedType.I4)]
			public int cy;
		}

		[ComVisible(true)]
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagSIZEL
		{
			[MarshalAs(UnmanagedType.I4)]
			public int cx;

			[MarshalAs(UnmanagedType.I4)]
			public int cy;
		}

		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagLOGPALETTE
		{
			[MarshalAs(UnmanagedType.U2)]
			public short palVersion;

			[MarshalAs(UnmanagedType.U2)]
			public short palNumEntries;
		}

		public class DOCHOSTUIDBLCLICK
		{
			public const int DEFAULT = 0;

			public const int SHOWPROPERTIES = 1;

			public const int SHOWCODE = 2;
		}

		public class DOCHOSTUIFLAG
		{
			public const int DIALOG = 1;

			public const int DISABLE_HELP_MENU = 2;

			public const int NO3DBORDER = 4;

			public const int SCROLL_NO = 8;

			public const int DISABLE_SCRIPT_INACTIVE = 16;

			public const int OPENNEWWIN = 32;

			public const int DISABLE_OFFSCREEN = 64;

			public const int FLAT_SCROLLBAR = 128;

			public const int DIV_BLOCKDEFAULT = 256;

			public const int ACTIVATE_CLIENTHIT_ONLY = 512;

			public const int DISABLE_COOKIE = 1024;
		}

		[ComVisible(true)]
		[StructLayout(LayoutKind.Sequential)]
		public class DOCHOSTUIINFO
		{
			[MarshalAs(UnmanagedType.U4)]
			public int cbSize;

			[MarshalAs(UnmanagedType.I4)]
			public int dwFlags;

			[MarshalAs(UnmanagedType.I4)]
			public int dwDoubleClick;

			[MarshalAs(UnmanagedType.I4)]
			public int dwReserved1;

			[MarshalAs(UnmanagedType.I4)]
			public int dwReserved2;
		}

		[ComVisible(true)]
		[Guid("BD3F23C0-D43E-11CF-893B-00AA00BDCE1A")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IDocHostUIHandler
		{
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int ShowContextMenu([MarshalAs(UnmanagedType.U4)] [In] int dwID, [In] NativeMethods.POINT pt, [MarshalAs(UnmanagedType.Interface)] [In] object pcmdtReserved, [MarshalAs(UnmanagedType.Interface)] [In] object pdispReserved);

			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int GetHostInfo([In] [Out] NativeMethods.DOCHOSTUIINFO info);

			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int ShowUI([MarshalAs(UnmanagedType.I4)] [In] int dwID, [In] NativeMethods.IOleInPlaceActiveObject activeObject, [In] NativeMethods.IOleCommandTarget commandTarget, [In] NativeMethods.IOleInPlaceFrame frame, [In] NativeMethods.IOleInPlaceUIWindow doc);

			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int HideUI();

			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int UpdateUI();

			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int EnableModeless([MarshalAs(UnmanagedType.Bool)] [In] bool fEnable);

			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int OnDocWindowActivate([MarshalAs(UnmanagedType.Bool)] [In] bool fActivate);

			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int OnFrameWindowActivate([MarshalAs(UnmanagedType.Bool)] [In] bool fActivate);

			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int ResizeBorder([In] NativeMethods.COMRECT rect, [In] NativeMethods.IOleInPlaceUIWindow doc, bool fFrameWindow);

			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int TranslateAccelerator([In] ref NativeMethods.MSG msg, [In] ref Guid group, [MarshalAs(UnmanagedType.I4)] [In] int nCmdID);

			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int GetOptionKeyPath([MarshalAs(UnmanagedType.LPArray)] [Out] string[] pbstrKey, [MarshalAs(UnmanagedType.U4)] [In] int dw);

			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int GetDropTarget([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IOleDropTarget pDropTarget, [MarshalAs(UnmanagedType.Interface)] out NativeMethods.IOleDropTarget ppDropTarget);

			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int GetExternal([MarshalAs(UnmanagedType.Interface)] out object ppDispatch);

			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int TranslateUrl([MarshalAs(UnmanagedType.U4)] [In] int dwTranslate, [MarshalAs(UnmanagedType.LPWStr)] [In] string strURLIn, [MarshalAs(UnmanagedType.LPWStr)] out string pstrURLOut);

			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int FilterDataObject(IDataObject pDO, out IDataObject ppDORet);
		}

		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComVisible(true)]
		[Guid("00000122-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IOleDropTarget
		{
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int OleDragEnter(IDataObject pDataObj, [MarshalAs(UnmanagedType.U4)] [In] int grfKeyState, [MarshalAs(UnmanagedType.U8)] [In] long pt, [MarshalAs(UnmanagedType.I4)] [In] [Out] ref int pdwEffect);

			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int OleDragOver([MarshalAs(UnmanagedType.U4)] [In] int grfKeyState, [MarshalAs(UnmanagedType.U8)] [In] long pt, [MarshalAs(UnmanagedType.I4)] [In] [Out] ref int pdwEffect);

			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int OleDragLeave();

			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int OleDrop(IDataObject pDataObj, [MarshalAs(UnmanagedType.U4)] [In] int grfKeyState, [MarshalAs(UnmanagedType.U8)] [In] long pt, [MarshalAs(UnmanagedType.I4)] [In] [Out] ref int pdwEffect);
		}

		[Guid("B722BCCB-4E68-101B-A2BC-00AA00404770")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComVisible(true)]
		[ComImport]
		public interface IOleCommandTarget
		{
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int QueryStatus(ref Guid pguidCmdGroup, int cCmds, [In] [Out] NativeMethods.OLECMD prgCmds, [In] [Out] string pCmdText);

			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int Exec(ref Guid pguidCmdGroup, int nCmdID, int nCmdexecopt, [MarshalAs(UnmanagedType.LPArray)] [In] object[] pvaIn, IntPtr pvaOut);
		}

		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComVisible(true)]
		[Guid("00000116-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IOleInPlaceFrame
		{
			IntPtr GetWindow();

			void ContextSensitiveHelp([MarshalAs(UnmanagedType.I4)] [In] int fEnterMode);

			void GetBorder([Out] NativeMethods.COMRECT lprectBorder);

			void RequestBorderSpace([In] NativeMethods.COMRECT pborderwidths);

			void SetBorderSpace([In] NativeMethods.COMRECT pborderwidths);

			void SetActiveObject([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IOleInPlaceActiveObject pActiveObject, [MarshalAs(UnmanagedType.LPWStr)] [In] string pszObjName);

			void InsertMenus([In] IntPtr hmenuShared, [In] [Out] object lpMenuWidths);

			void SetMenu([In] IntPtr hmenuShared, [In] IntPtr holemenu, [In] IntPtr hwndActiveObject);

			void RemoveMenus([In] IntPtr hmenuShared);

			void SetStatusText([MarshalAs(UnmanagedType.BStr)] [In] string pszStatusText);

			void EnableModeless([MarshalAs(UnmanagedType.I4)] [In] int fEnable);

			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int TranslateAccelerator([In] ref NativeMethods.MSG lpmsg, [MarshalAs(UnmanagedType.U2)] [In] short wID);
		}

		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComVisible(true)]
		[Guid("00000115-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IOleInPlaceUIWindow
		{
			IntPtr GetWindow();

			void ContextSensitiveHelp([MarshalAs(UnmanagedType.I4)] [In] int fEnterMode);

			void GetBorder([Out] NativeMethods.COMRECT lprectBorder);

			void RequestBorderSpace([In] NativeMethods.COMRECT pborderwidths);

			void SetBorderSpace([In] NativeMethods.COMRECT pborderwidths);

			void SetActiveObject([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IOleInPlaceActiveObject pActiveObject, [MarshalAs(UnmanagedType.LPWStr)] [In] string pszObjName);
		}

		[Guid("00000117-0000-0000-C000-000000000046")]
		[ComVisible(true)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleInPlaceActiveObject
		{
			int GetWindow(out IntPtr hwnd);

			void ContextSensitiveHelp([MarshalAs(UnmanagedType.I4)] [In] int fEnterMode);

			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int TranslateAccelerator([In] ref NativeMethods.MSG lpmsg);

			void OnFrameWindowActivate([MarshalAs(UnmanagedType.I4)] [In] int fActivate);

			void OnDocWindowActivate([MarshalAs(UnmanagedType.I4)] [In] int fActivate);

			void ResizeBorder([In] NativeMethods.COMRECT prcBorder, [In] NativeMethods.IOleInPlaceUIWindow pUIWindow, [MarshalAs(UnmanagedType.I4)] [In] int fFrameWindow);

			void EnableModeless([MarshalAs(UnmanagedType.I4)] [In] int fEnable);
		}

		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComVisible(true)]
		[Guid("0000011B-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IOleContainer
		{
			void ParseDisplayName([MarshalAs(UnmanagedType.Interface)] [In] object pbc, [MarshalAs(UnmanagedType.BStr)] [In] string pszDisplayName, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pchEaten, [MarshalAs(UnmanagedType.LPArray)] [Out] object[] ppmkOut);

			void EnumObjects([MarshalAs(UnmanagedType.U4)] [In] int grfFlags, [MarshalAs(UnmanagedType.Interface)] out object ppenum);

			void LockContainer([MarshalAs(UnmanagedType.I4)] [In] int fLock);
		}

		[Guid("00000118-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComVisible(true)]
		[ComImport]
		public interface IOleClientSite
		{
			void SaveObject();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetMoniker([MarshalAs(UnmanagedType.U4)] [In] int dwAssign, [MarshalAs(UnmanagedType.U4)] [In] int dwWhichMoniker);

			[PreserveSig]
			int GetContainer(out NativeMethods.IOleContainer ppContainer);

			void ShowObject();

			void OnShowWindow([MarshalAs(UnmanagedType.I4)] [In] int fShow);

			void RequestNewObjectLayout();
		}

		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComVisible(true)]
		[Guid("B722BCC7-4E68-101B-A2BC-00AA00404770")]
		[ComImport]
		public interface IOleDocumentSite
		{
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int ActivateMe([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IOleDocumentView pViewToActivate);
		}

		[Guid("B722BCC6-4E68-101B-A2BC-00AA00404770")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComVisible(true)]
		[ComImport]
		public interface IOleDocumentView
		{
			void SetInPlaceSite([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IOleInPlaceSite pIPSite);

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IOleInPlaceSite GetInPlaceSite();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetDocument();

			void SetRect([In] NativeMethods.COMRECT prcView);

			void GetRect([Out] NativeMethods.COMRECT prcView);

			void SetRectComplex([In] NativeMethods.COMRECT prcView, [In] NativeMethods.COMRECT prcHScroll, [In] NativeMethods.COMRECT prcVScroll, [In] NativeMethods.COMRECT prcSizeBox);

			void Show([MarshalAs(UnmanagedType.I4)] [In] int fShow);

			void UIActivate([MarshalAs(UnmanagedType.I4)] [In] int fUIActivate);

			void Open();

			void CloseView([MarshalAs(UnmanagedType.U4)] [In] int dwReserved);

			void SaveViewState([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IStream pstm);

			void ApplyViewState([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IStream pstm);

			void Clone([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IOleInPlaceSite pIPSiteNew, [MarshalAs(UnmanagedType.LPArray)] [Out] NativeMethods.IOleDocumentView[] ppViewNew);
		}

		[ComVisible(true)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("00000119-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IOleInPlaceSite
		{
			IntPtr GetWindow();

			void ContextSensitiveHelp([MarshalAs(UnmanagedType.I4)] [In] int fEnterMode);

			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int CanInPlaceActivate();

			void OnInPlaceActivate();

			void OnUIActivate();

			void GetWindowContext(out NativeMethods.IOleInPlaceFrame ppFrame, out NativeMethods.IOleInPlaceUIWindow ppDoc, [Out] NativeMethods.COMRECT lprcPosRect, [Out] NativeMethods.COMRECT lprcClipRect, [In] [Out] NativeMethods.tagOIFI lpFrameInfo);

			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int Scroll([MarshalAs(UnmanagedType.U4)] [In] NativeMethods.tagSIZE scrollExtant);

			void OnUIDeactivate([MarshalAs(UnmanagedType.I4)] [In] int fUndoable);

			void OnInPlaceDeactivate();

			void DiscardUndoState();

			void DeactivateAndUndo();

			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int OnPosRectChange([In] NativeMethods.COMRECT lprcPosRect);
		}

		[ComVisible(true)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("0000000C-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IStream
		{
			[return: MarshalAs(UnmanagedType.I4)]
			int Read([In] IntPtr buf, [MarshalAs(UnmanagedType.I4)] [In] int len);

			[return: MarshalAs(UnmanagedType.I4)]
			int Write([In] IntPtr buf, [MarshalAs(UnmanagedType.I4)] [In] int len);

			[return: MarshalAs(UnmanagedType.I8)]
			long Seek([MarshalAs(UnmanagedType.I8)] [In] long dlibMove, [MarshalAs(UnmanagedType.I4)] [In] int dwOrigin);

			void SetSize([MarshalAs(UnmanagedType.I8)] [In] long libNewSize);

			[return: MarshalAs(UnmanagedType.I8)]
			long CopyTo([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IStream pstm, [MarshalAs(UnmanagedType.I8)] [In] long cb, [MarshalAs(UnmanagedType.LPArray)] [Out] long[] pcbRead);

			void Commit([MarshalAs(UnmanagedType.I4)] [In] int grfCommitFlags);

			void Revert();

			void LockRegion([MarshalAs(UnmanagedType.I8)] [In] long libOffset, [MarshalAs(UnmanagedType.I8)] [In] long cb, [MarshalAs(UnmanagedType.I4)] [In] int dwLockType);

			void UnlockRegion([MarshalAs(UnmanagedType.I8)] [In] long libOffset, [MarshalAs(UnmanagedType.I8)] [In] long cb, [MarshalAs(UnmanagedType.I4)] [In] int dwLockType);

			void Stat([In] IntPtr pStatstg, [MarshalAs(UnmanagedType.I4)] [In] int grfStatFlag);

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IStream Clone();
		}

		[ComVisible(true)]
		[Guid("00000112-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleObject
		{
			[PreserveSig]
			int SetClientSite([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IOleClientSite pClientSite);

			[PreserveSig]
			int GetClientSite(out NativeMethods.IOleClientSite site);

			[PreserveSig]
			int SetHostNames([MarshalAs(UnmanagedType.LPWStr)] [In] string szContainerApp, [MarshalAs(UnmanagedType.LPWStr)] [In] string szContainerObj);

			[PreserveSig]
			int Close([MarshalAs(UnmanagedType.I4)] [In] int dwSaveOption);

			[PreserveSig]
			int SetMoniker([MarshalAs(UnmanagedType.U4)] [In] int dwWhichMoniker, [MarshalAs(UnmanagedType.Interface)] [In] object pmk);

			[PreserveSig]
			int GetMoniker([MarshalAs(UnmanagedType.U4)] [In] int dwAssign, [MarshalAs(UnmanagedType.U4)] [In] int dwWhichMoniker, out object moniker);

			[PreserveSig]
			int InitFromData(IDataObject pDataObject, [MarshalAs(UnmanagedType.I4)] [In] int fCreation, [MarshalAs(UnmanagedType.U4)] [In] int dwReserved);

			[PreserveSig]
			int GetClipboardData([MarshalAs(UnmanagedType.U4)] [In] int dwReserved, out IDataObject data);

			[PreserveSig]
			int DoVerb([MarshalAs(UnmanagedType.I4)] [In] int iVerb, [In] IntPtr lpmsg, [MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IOleClientSite pActiveSite, [MarshalAs(UnmanagedType.I4)] [In] int lindex, [In] IntPtr hwndParent, [In] NativeMethods.COMRECT lprcPosRect);

			[PreserveSig]
			int EnumVerbs(out NativeMethods.IEnumOLEVERB e);

			[PreserveSig]
			int OleUpdate();

			[PreserveSig]
			int IsUpToDate();

			[PreserveSig]
			int GetUserClassID([In] [Out] ref Guid pClsid);

			[PreserveSig]
			int GetUserType([MarshalAs(UnmanagedType.U4)] [In] int dwFormOfType, [MarshalAs(UnmanagedType.LPWStr)] out string userType);

			[PreserveSig]
			int SetExtent([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, [In] NativeMethods.tagSIZEL pSizel);

			[PreserveSig]
			int GetExtent([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, [Out] NativeMethods.tagSIZEL pSizel);

			[PreserveSig]
			int Advise([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IAdviseSink pAdvSink, out int cookie);

			[PreserveSig]
			int Unadvise([MarshalAs(UnmanagedType.U4)] [In] int dwConnection);

			[PreserveSig]
			int EnumAdvise(out object e);

			[PreserveSig]
			int GetMiscStatus([MarshalAs(UnmanagedType.U4)] [In] int dwAspect, out int misc);

			[PreserveSig]
			int SetColorScheme([In] NativeMethods.tagLOGPALETTE pLogpal);
		}

		[ComVisible(true)]
		[Guid("0000010F-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IAdviseSink
		{
			void OnDataChange([In] NativeMethods.FORMATETC pFormatetc, [In] NativeMethods.STGMEDIUM pStgmed);

			void OnViewChange([MarshalAs(UnmanagedType.U4)] [In] int dwAspect, [MarshalAs(UnmanagedType.I4)] [In] int lindex);

			void OnRename([MarshalAs(UnmanagedType.Interface)] [In] object pmk);

			void OnSave();

			void OnClose();
		}

		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComVisible(true)]
		[Guid("7FD52380-4E07-101B-AE2D-08002B2EC713")]
		[ComImport]
		public interface IPersistStreamInit
		{
			void GetClassID([In] [Out] ref Guid pClassID);

			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int IsDirty();

			void Load([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IStream pstm);

			void Save([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IStream pstm, [MarshalAs(UnmanagedType.Bool)] [In] bool fClearDirty);

			void GetSizeMax([MarshalAs(UnmanagedType.LPArray)] [Out] long pcbSize);

			void InitNew();
		}

		[ComVisible(true)]
		[Guid("25336920-03F9-11CF-8FD0-00AA00686F13")]
		[ComImport]
		public class HTMLDocument
		{
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			public extern HTMLDocument();
		}

		[Guid("626FC520-A41E-11CF-A731-00A0C9082637")]
		[ComVisible(true)]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComImport]
		public interface IHTMLDocument
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetScript();
		}

		[Guid("332C4425-26CB-11D0-B483-00C04FD90119")]
		[ComVisible(true)]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComImport]
		public interface IHTMLDocument2
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetScript();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElementCollection GetAll();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElement GetBody();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElement GetActiveElement();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElementCollection GetImages();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElementCollection GetApplets();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElementCollection GetLinks();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElementCollection GetForms();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElementCollection GetAnchors();

			void SetTitle([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTitle();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElementCollection GetScripts();

			void SetDesignMode([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetDesignMode();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetSelection();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetReadyState();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetFrames();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElementCollection GetEmbeds();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElementCollection GetPlugins();

			void SetAlinkColor([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetAlinkColor();

			void SetBgColor([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBgColor();

			void SetFgColor([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetFgColor();

			void SetLinkColor([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetLinkColor();

			void SetVlinkColor([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetVlinkColor();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetReferrer();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetLocation();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetLastModified();

			void SetURL([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetURL();

			void SetDomain([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetDomain();

			void SetCookie([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetCookie();

			void SetExpando([MarshalAs(UnmanagedType.Bool)] [In] bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetExpando();

			void SetCharset([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetCharset();

			void SetDefaultCharset([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetDefaultCharset();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetMimeType();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFileSize();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFileCreatedDate();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFileModifiedDate();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFileUpdatedDate();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetSecurity();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetProtocol();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetNameProp();

			void DummyWrite([MarshalAs(UnmanagedType.I4)] [In] int psarray);

			void DummyWriteln([MarshalAs(UnmanagedType.I4)] [In] int psarray);

			[return: MarshalAs(UnmanagedType.Interface)]
			object Open([MarshalAs(UnmanagedType.BStr)] [In] string URL, [MarshalAs(UnmanagedType.Struct)] [In] object name, [MarshalAs(UnmanagedType.Struct)] [In] object features, [MarshalAs(UnmanagedType.Struct)] [In] object replace);

			void Close();

			void Clear();

			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandSupported([MarshalAs(UnmanagedType.BStr)] [In] string cmdID);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandEnabled([MarshalAs(UnmanagedType.BStr)] [In] string cmdID);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandState([MarshalAs(UnmanagedType.BStr)] [In] string cmdID);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandIndeterm([MarshalAs(UnmanagedType.BStr)] [In] string cmdID);

			[return: MarshalAs(UnmanagedType.BStr)]
			string QueryCommandText([MarshalAs(UnmanagedType.BStr)] [In] string cmdID);

			[return: MarshalAs(UnmanagedType.Struct)]
			object QueryCommandValue([MarshalAs(UnmanagedType.BStr)] [In] string cmdID);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool ExecCommand([MarshalAs(UnmanagedType.BStr)] [In] string cmdID, [MarshalAs(UnmanagedType.Bool)] [In] bool showUI, [MarshalAs(UnmanagedType.Struct)] [In] object value);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool ExecCommandShowHelp([MarshalAs(UnmanagedType.BStr)] [In] string cmdID);

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElement CreateElement([MarshalAs(UnmanagedType.BStr)] [In] string eTag);

			void SetOnhelp([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnhelp();

			void SetOnclick([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnclick();

			void SetOndblclick([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndblclick();

			void SetOnkeyup([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnkeyup();

			void SetOnkeydown([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnkeydown();

			void SetOnkeypress([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnkeypress();

			void SetOnmouseup([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnmouseup();

			void SetOnmousedown([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnmousedown();

			void SetOnmousemove([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnmousemove();

			void SetOnmouseout([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnmouseout();

			void SetOnmouseover([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnmouseover();

			void SetOnreadystatechange([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnreadystatechange();

			void SetOnafterupdate([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnafterupdate();

			void SetOnrowexit([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnrowexit();

			void SetOnrowenter([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnrowenter();

			void SetOndragstart([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndragstart();

			void SetOnselectstart([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnselectstart();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElement ElementFromPoint([MarshalAs(UnmanagedType.I4)] [In] int x, [MarshalAs(UnmanagedType.I4)] [In] int y);

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetParentWindow();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetStyleSheets();

			void SetOnbeforeupdate([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnbeforeupdate();

			void SetOnerrorupdate([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnerrorupdate();

			[return: MarshalAs(UnmanagedType.BStr)]
			string toString();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLStyleSheet CreateStyleSheet([MarshalAs(UnmanagedType.BStr)] [In] string bstrHref, [MarshalAs(UnmanagedType.I4)] [In] int lIndex);
		}

		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[Guid("3050F1FF-98B5-11CF-BB82-00AA00BDCE0B")]
		[ComVisible(true)]
		[ComImport]
		public interface IHTMLElement
		{
			void SetAttribute([MarshalAs(UnmanagedType.BStr)] [In] string strAttributeName, [MarshalAs(UnmanagedType.Struct)] [In] object AttributeValue, [MarshalAs(UnmanagedType.I4)] [In] int lFlags);

			void GetAttribute([MarshalAs(UnmanagedType.BStr)] [In] string strAttributeName, [MarshalAs(UnmanagedType.I4)] [In] int lFlags, [MarshalAs(UnmanagedType.LPArray)] [Out] object[] pvars);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool RemoveAttribute([MarshalAs(UnmanagedType.BStr)] [In] string strAttributeName, [MarshalAs(UnmanagedType.I4)] [In] int lFlags);

			void SetClassName([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetClassName();

			void SetId([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetId();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTagName();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElement GetParentElement();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLStyle GetStyle();

			void SetOnhelp([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnhelp();

			void SetOnclick([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnclick();

			void SetOndblclick([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndblclick();

			void SetOnkeydown([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnkeydown();

			void SetOnkeyup([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnkeyup();

			void SetOnkeypress([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnkeypress();

			void SetOnmouseout([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnmouseout();

			void SetOnmouseover([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnmouseover();

			void SetOnmousemove([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnmousemove();

			void SetOnmousedown([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnmousedown();

			void SetOnmouseup([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnmouseup();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDocument2 GetDocument();

			void SetTitle([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTitle();

			void SetLanguage([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetLanguage();

			void SetOnselectstart([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnselectstart();

			void ScrollIntoView([MarshalAs(UnmanagedType.Struct)] [In] object varargStart);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool Contains([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IHTMLElement pChild);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetSourceIndex();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetRecordNumber();

			void SetLang([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetLang();

			[return: MarshalAs(UnmanagedType.I4)]
			int GetOffsetLeft();

			[return: MarshalAs(UnmanagedType.I4)]
			int GetOffsetTop();

			[return: MarshalAs(UnmanagedType.I4)]
			int GetOffsetWidth();

			[return: MarshalAs(UnmanagedType.I4)]
			int GetOffsetHeight();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElement GetOffsetParent();

			void SetInnerHTML([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetInnerHTML();

			void SetInnerText([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetInnerText();

			void SetOuterHTML([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetOuterHTML();

			void SetOuterText([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetOuterText();

			void InsertAdjacentHTML([MarshalAs(UnmanagedType.BStr)] [In] string where, [MarshalAs(UnmanagedType.BStr)] [In] string html);

			void InsertAdjacentText([MarshalAs(UnmanagedType.BStr)] [In] string where, [MarshalAs(UnmanagedType.BStr)] [In] string text);

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElement GetParentTextEdit();

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetIsTextEdit();

			void Click();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetFilters();

			void SetOndragstart([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndragstart();

			[return: MarshalAs(UnmanagedType.BStr)]
			string toString();

			void SetOnbeforeupdate([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnbeforeupdate();

			void SetOnafterupdate([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnafterupdate();

			void SetOnerrorupdate([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnerrorupdate();

			void SetOnrowexit([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnrowexit();

			void SetOnrowenter([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnrowenter();

			void SetOndatasetchanged([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndatasetchanged();

			void SetOndataavailable([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndataavailable();

			void SetOndatasetcomplete([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndatasetcomplete();

			void SetOnfilterchange([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnfilterchange();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetChildren();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetAll();
		}

		[Guid("3050F434-98B5-11CF-BB82-00AA00BDCE0B")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComImport]
		public interface IHTMLElement2
		{
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetScopeName();

			void SetCapture([MarshalAs(UnmanagedType.Bool)] [In] bool containerCapture);

			void ReleaseCapture();

			void SetOnlosecapture([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnlosecapture();

			[return: MarshalAs(UnmanagedType.BStr)]
			string ComponentFromPoint([MarshalAs(UnmanagedType.I4)] [In] int x, [MarshalAs(UnmanagedType.I4)] [In] int y);

			void DoScroll([MarshalAs(UnmanagedType.Struct)] [In] object component);

			void SetOnscroll([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnscroll();

			void SetOndrag([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndrag();

			void SetOndragend([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndragend();

			void SetOndragenter([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndragenter();

			void SetOndragover([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndragover();

			void SetOndragleave([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndragleave();

			void SetOndrop([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOndrop();

			void SetOnbeforecut([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnbeforecut();

			void SetOncut([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOncut();

			void SetOnbeforecopy([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnbeforecopy();

			void SetOncopy([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOncopy();

			void SetOnbeforepaste([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnbeforepaste();

			void SetOnpaste([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnpaste();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLCurrentStyle GetCurrentStyle();

			void SetOnpropertychange([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnpropertychange();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLRectCollection GetClientRects();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLRect GetBoundingClientRect();

			void SetExpression([MarshalAs(UnmanagedType.BStr)] [In] string propname, [MarshalAs(UnmanagedType.BStr)] [In] string expression, [MarshalAs(UnmanagedType.BStr)] [In] string language);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetExpression([MarshalAs(UnmanagedType.BStr)] [In] object propname);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool RemoveExpression([MarshalAs(UnmanagedType.BStr)] [In] string propname);

			void SetTabIndex([MarshalAs(UnmanagedType.I2)] [In] short p);

			[return: MarshalAs(UnmanagedType.I2)]
			short GetTabIndex();

			void Focus();

			void SetAccessKey([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetAccessKey();

			void SetOnblur([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnblur();

			void SetOnfocus([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnfocus();

			void SetOnresize([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnresize();

			void Blur();

			void AddFilter([MarshalAs(UnmanagedType.Interface)] [In] object pUnk);

			void RemoveFilter([MarshalAs(UnmanagedType.Interface)] [In] object pUnk);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetClientHeight();

			[return: MarshalAs(UnmanagedType.I4)]
			int GetClientWidth();

			[return: MarshalAs(UnmanagedType.I4)]
			int GetClientTop();

			[return: MarshalAs(UnmanagedType.I4)]
			int GetClientLeft();

			[return: MarshalAs(UnmanagedType.Bool)]
			bool AttachEvent([MarshalAs(UnmanagedType.BStr)] [In] string ev, [MarshalAs(UnmanagedType.Interface)] [In] object pdisp);

			void DetachEvent([MarshalAs(UnmanagedType.BStr)] [In] string ev, [MarshalAs(UnmanagedType.Interface)] [In] object pdisp);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetReadyState();

			void SetOnreadystatechange([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnreadystatechange();

			void SetOnrowsdelete([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnrowsdelete();

			void SetOnrowsinserted([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnrowsinserted();

			void SetOncellchange([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOncellchange();

			void SetDir([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetDir();

			[return: MarshalAs(UnmanagedType.Interface)]
			object CreateControlRange();

			[return: MarshalAs(UnmanagedType.I4)]
			int GetScrollHeight();

			[return: MarshalAs(UnmanagedType.I4)]
			int GetScrollWidth();

			void SetScrollTop([MarshalAs(UnmanagedType.I4)] [In] int p);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetScrollTop();

			void SetScrollLeft([MarshalAs(UnmanagedType.I4)] [In] int p);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetScrollLeft();

			void ClearAttributes();

			void MergeAttributes([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IHTMLElement mergeThis);

			void SetOncontextmenu([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOncontextmenu();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElement InsertAdjacentElement([MarshalAs(UnmanagedType.BStr)] [In] string where, [MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IHTMLElement insertedElement);

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElement ApplyElement([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IHTMLElement apply, [MarshalAs(UnmanagedType.BStr)] [In] string where);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetAdjacentText([MarshalAs(UnmanagedType.BStr)] [In] string where);

			[return: MarshalAs(UnmanagedType.BStr)]
			string ReplaceAdjacentText([MarshalAs(UnmanagedType.BStr)] [In] string where, [MarshalAs(UnmanagedType.BStr)] [In] string newText);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetCanHaveChildren();

			[return: MarshalAs(UnmanagedType.I4)]
			int AddBehavior([MarshalAs(UnmanagedType.BStr)] [In] string bstrUrl, [In] ref object pvarFactory);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool RemoveBehavior([MarshalAs(UnmanagedType.I4)] [In] int cookie);

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLStyle GetRuntimeStyle();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetBehaviorUrns();

			void SetTagUrn([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTagUrn();

			void SetOnbeforeeditfocus([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnbeforeeditfocus();

			[return: MarshalAs(UnmanagedType.I4)]
			int GetReadyStateValue();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElementCollection GetElementsByTagName([MarshalAs(UnmanagedType.BStr)] [In] string v);

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLStyle GetBaseStyle();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLCurrentStyle GetBaseCurrentStyle();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLStyle GetBaseRuntimeStyle();

			void SetOnmousehover([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnmousehover();

			void SetOnkeydownpreview([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnkeydownpreview();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetBehavior([MarshalAs(UnmanagedType.BStr)] [In] string bstrName, [MarshalAs(UnmanagedType.BStr)] [In] string bstrUrn);
		}

		[ComVisible(true)]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[Guid("3050F673-98B5-11CF-BB82-00AA00BDCE0B")]
		[ComImport]
		public interface IHTMLElement3
		{
			void MergeAttributes([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IHTMLElement mergeThis, [MarshalAs(UnmanagedType.Struct)] [In] object pvarFlags);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetIsMultiLine();

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetCanHaveHTML();

			void SetOnLayoutComplete([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnLayoutComplete();

			void SetOnPage([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnPage();

			void SetInflateBlock([MarshalAs(UnmanagedType.Bool)] [In] bool inflate);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetInflateBlock();

			void SetOnBeforeDeactivate([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnBeforeDeactivate();

			void SetActive();

			void SetContentEditable([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetContentEditable();

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetIsContentEditable();

			void SetHideFocus([MarshalAs(UnmanagedType.Bool)] [In] bool v);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetHideFocus();

			void SetDisabled([MarshalAs(UnmanagedType.Bool)] [In] bool v);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetDisabled();

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetIsDisabled();

			void SetOnMove([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnMove();

			void SetOnControlSelect([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnControlSelect();

			[return: MarshalAs(UnmanagedType.Bool)]
			bool FireEvent([MarshalAs(UnmanagedType.BStr)] [In] string eventName, [MarshalAs(UnmanagedType.Struct)] [In] object eventObject);

			void SetOnResizeStart([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnResizeStart();

			void SetOnResizeEnd([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnResizeEnd();

			void SetOnMoveStart([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnMoveStart();

			void SetOnMoveEnd([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnMoveEnd();

			void SetOnMouseEnter([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnMouseEnter();

			void SetOnMouseLeave([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnMouseLeave();

			void SetOnActivate([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnActivate();

			void SetOnDeactivate([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnDeactivate();

			[return: MarshalAs(UnmanagedType.Bool)]
			bool DragDrop();

			[return: MarshalAs(UnmanagedType.I4)]
			int GetGlyphMode();
		}

		[ComVisible(true)]
		[Guid("3050F1D8-98B5-11CF-BB82-00AA00BDCE0B")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComImport]
		public interface IHTMLBodyElement
		{
			void SetBackground([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackground();

			void SetBgProperties([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBgProperties();

			void SetLeftMargin([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetLeftMargin();

			void SetTopMargin([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetTopMargin();

			void SetRightMargin([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetRightMargin();

			void SetBottomMargin([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBottomMargin();

			void SetNoWrap([MarshalAs(UnmanagedType.Bool)] [In] bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetNoWrap();

			void SetBgColor([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBgColor();

			void SetText([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetText();

			void SetLink([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetLink();

			void SetVLink([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetVLink();

			void SetALink([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetALink();

			void SetOnload([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnload();

			void SetOnunload([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnunload();

			void SetScroll([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetScroll();

			void SetOnselect([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnselect();

			void SetOnbeforeunload([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnbeforeunload();

			[return: MarshalAs(UnmanagedType.Interface)]
			object CreateTextRange();
		}

		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComVisible(true)]
		[Guid("3050F2E3-98B5-11CF-BB82-00AA00BDCE0B")]
		[ComImport]
		public interface IHTMLStyleSheet
		{
			void SetTitle([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTitle();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLStyleSheet GetParentStyleSheet();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElement GetOwningElement();

			void SetDisabled([MarshalAs(UnmanagedType.Bool)] [In] bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetDisabled();

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetReadOnly();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetImports();

			void SetHref([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetHref();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetStyleSheetType();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetId();

			[return: MarshalAs(UnmanagedType.I4)]
			int AddImport([MarshalAs(UnmanagedType.BStr)] [In] string bstrURL, [MarshalAs(UnmanagedType.I4)] [In] int lIndex);

			[return: MarshalAs(UnmanagedType.I4)]
			int AddRule([MarshalAs(UnmanagedType.BStr)] [In] string bstrSelector, [MarshalAs(UnmanagedType.BStr)] [In] string bstrStyle, [MarshalAs(UnmanagedType.I4)] [In] int lIndex);

			void RemoveImport([MarshalAs(UnmanagedType.I4)] [In] int lIndex);

			void RemoveRule([MarshalAs(UnmanagedType.I4)] [In] int lIndex);

			void SetMedia([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetMedia();

			void SetCssText([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetCssText();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetRules();
		}

		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComVisible(true)]
		[Guid("3050F25E-98B5-11CF-BB82-00AA00BDCE0B")]
		[ComImport]
		public interface IHTMLStyle
		{
			void SetFontFamily([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontFamily();

			void SetFontStyle([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontStyle();

			void SetFontObject([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontObject();

			void SetFontWeight([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontWeight();

			void SetFontSize([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetFontSize();

			void SetFont([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFont();

			void SetColor([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetColor();

			void SetBackground([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackground();

			void SetBackgroundColor([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBackgroundColor();

			void SetBackgroundImage([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundImage();

			void SetBackgroundRepeat([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundRepeat();

			void SetBackgroundAttachment([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundAttachment();

			void SetBackgroundPosition([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundPosition();

			void SetBackgroundPositionX([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBackgroundPositionX();

			void SetBackgroundPositionY([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBackgroundPositionY();

			void SetWordSpacing([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetWordSpacing();

			void SetLetterSpacing([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetLetterSpacing();

			void SetTextDecoration([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTextDecoration();

			void SetTextDecorationNone([MarshalAs(UnmanagedType.Bool)] [In] bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTextDecorationNone();

			void SetTextDecorationUnderline([MarshalAs(UnmanagedType.Bool)] [In] bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTextDecorationUnderline();

			void SetTextDecorationOverline([MarshalAs(UnmanagedType.Bool)] [In] bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTextDecorationOverline();

			void SetTextDecorationLineThrough([MarshalAs(UnmanagedType.Bool)] [In] bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTextDecorationLineThrough();

			void SetTextDecorationBlink([MarshalAs(UnmanagedType.Bool)] [In] bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTextDecorationBlink();

			void SetVerticalAlign([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetVerticalAlign();

			void SetTextTransform([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTextTransform();

			void SetTextAlign([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTextAlign();

			void SetTextIndent([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetTextIndent();

			void SetLineHeight([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetLineHeight();

			void SetMarginTop([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetMarginTop();

			void SetMarginRight([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetMarginRight();

			void SetMarginBottom([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetMarginBottom();

			void SetMarginLeft([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetMarginLeft();

			void SetMargin([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetMargin();

			void SetPaddingTop([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetPaddingTop();

			void SetPaddingRight([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetPaddingRight();

			void SetPaddingBottom([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetPaddingBottom();

			void SetPaddingLeft([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetPaddingLeft();

			void SetPadding([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPadding();

			void SetBorder([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorder();

			void SetBorderTop([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderTop();

			void SetBorderRight([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderRight();

			void SetBorderBottom([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderBottom();

			void SetBorderLeft([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderLeft();

			void SetBorderColor([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderColor();

			void SetBorderTopColor([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderTopColor();

			void SetBorderRightColor([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderRightColor();

			void SetBorderBottomColor([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderBottomColor();

			void SetBorderLeftColor([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderLeftColor();

			void SetBorderWidth([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderWidth();

			void SetBorderTopWidth([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderTopWidth();

			void SetBorderRightWidth([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderRightWidth();

			void SetBorderBottomWidth([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderBottomWidth();

			void SetBorderLeftWidth([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderLeftWidth();

			void SetBorderStyle([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderStyle();

			void SetBorderTopStyle([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderTopStyle();

			void SetBorderRightStyle([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderRightStyle();

			void SetBorderBottomStyle([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderBottomStyle();

			void SetBorderLeftStyle([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderLeftStyle();

			void SetWidth([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetWidth();

			void SetHeight([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetHeight();

			void SetStyleFloat([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetStyleFloat();

			void SetClear([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetClear();

			void SetDisplay([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetDisplay();

			void SetVisibility([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetVisibility();

			void SetListStyleType([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStyleType();

			void SetListStylePosition([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStylePosition();

			void SetListStyleImage([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStyleImage();

			void SetListStyle([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStyle();

			void SetWhiteSpace([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetWhiteSpace();

			void SetTop([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetTop();

			void SetLeft([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetLeft();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPosition();

			void SetZIndex([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetZIndex();

			void SetOverflow([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetOverflow();

			void SetPageBreakBefore([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPageBreakBefore();

			void SetPageBreakAfter([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPageBreakAfter();

			void SetCssText([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetCssText();

			void SetPixelTop([MarshalAs(UnmanagedType.I4)] [In] int p);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetPixelTop();

			void SetPixelLeft([MarshalAs(UnmanagedType.I4)] [In] int p);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetPixelLeft();

			void SetPixelWidth([MarshalAs(UnmanagedType.I4)] [In] int p);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetPixelWidth();

			void SetPixelHeight([MarshalAs(UnmanagedType.I4)] [In] int p);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetPixelHeight();

			void SetPosTop([MarshalAs(UnmanagedType.R4)] [In] float p);

			[return: MarshalAs(UnmanagedType.R4)]
			float GetPosTop();

			void SetPosLeft([MarshalAs(UnmanagedType.R4)] [In] float p);

			[return: MarshalAs(UnmanagedType.R4)]
			float GetPosLeft();

			void SetPosWidth([MarshalAs(UnmanagedType.R4)] [In] float p);

			[return: MarshalAs(UnmanagedType.R4)]
			float GetPosWidth();

			void SetPosHeight([MarshalAs(UnmanagedType.R4)] [In] float p);

			[return: MarshalAs(UnmanagedType.R4)]
			float GetPosHeight();

			void SetCursor([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetCursor();

			void SetClip([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetClip();

			void SetFilter([MarshalAs(UnmanagedType.BStr)] [In] string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFilter();

			void SetAttribute([MarshalAs(UnmanagedType.BStr)] [In] string strAttributeName, [MarshalAs(UnmanagedType.Struct)] [In] object AttributeValue, [MarshalAs(UnmanagedType.I4)] [In] int lFlags);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetAttribute([MarshalAs(UnmanagedType.BStr)] [In] string strAttributeName, [MarshalAs(UnmanagedType.I4)] [In] int lFlags);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool RemoveAttribute([MarshalAs(UnmanagedType.BStr)] [In] string strAttributeName, [MarshalAs(UnmanagedType.I4)] [In] int lFlags);
		}

		[Guid("3050F3DB-98B5-11CF-BB82-00AA00BDCE0B")]
		[ComVisible(true)]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComImport]
		public interface IHTMLCurrentStyle
		{
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPosition();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetStyleFloat();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetColor();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBackgroundColor();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontFamily();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontStyle();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontObject();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetFontWeight();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetFontSize();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundImage();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBackgroundPositionX();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBackgroundPositionY();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundRepeat();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderLeftColor();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderTopColor();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderRightColor();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderBottomColor();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderTopStyle();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderRightStyle();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderBottomStyle();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderLeftStyle();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderTopWidth();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderRightWidth();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderBottomWidth();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBorderLeftWidth();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetLeft();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetTop();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetWidth();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetHeight();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetPaddingLeft();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetPaddingTop();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetPaddingRight();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetPaddingBottom();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTextAlign();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTextDecoration();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetDisplay();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetVisibility();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetZIndex();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetLetterSpacing();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetLineHeight();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetTextIndent();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetVerticalAlign();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundAttachment();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetMarginTop();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetMarginRight();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetMarginBottom();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetMarginLeft();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetClear();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStyleType();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStylePosition();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStyleImage();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetClipTop();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetClipRight();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetClipBottom();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetClipLeft();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetOverflow();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPageBreakBefore();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPageBreakAfter();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetCursor();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTableLayout();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderCollapse();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetDirection();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBehavior();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetAttribute([MarshalAs(UnmanagedType.BStr)] [In] string strAttributeName, [MarshalAs(UnmanagedType.I4)] [In] int lFlags);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetUnicodeBidi();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetRight();

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetBottom();
		}

		[Guid("3050F21F-98B5-11CF-BB82-00AA00BDCE0B")]
		[ComVisible(true)]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComImport]
		public interface IHTMLElementCollection
		{
			[return: MarshalAs(UnmanagedType.BStr)]
			string toString();

			void SetLength([MarshalAs(UnmanagedType.I4)] [In] int p);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetLength();

			[return: MarshalAs(UnmanagedType.Interface)]
			object Get_newEnum();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLElement Item([MarshalAs(UnmanagedType.Struct)] [In] object name, [MarshalAs(UnmanagedType.Struct)] [In] object index);

			[return: MarshalAs(UnmanagedType.Interface)]
			object Tags([MarshalAs(UnmanagedType.Struct)] [In] object tagName);
		}

		[ComVisible(true)]
		[Guid("3050F4A3-98B5-11CF-BB82-00AA00BDCE0B")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComImport]
		public interface IHTMLRect
		{
			void SetLeft([MarshalAs(UnmanagedType.I4)] [In] int p);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetLeft();

			void SetTop([MarshalAs(UnmanagedType.I4)] [In] int p);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetTop();

			void SetRight([MarshalAs(UnmanagedType.I4)] [In] int p);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetRight();

			void SetBottom([MarshalAs(UnmanagedType.I4)] [In] int p);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetBottom();
		}

		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComVisible(true)]
		[Guid("3050F4A4-98B5-11CF-BB82-00AA00BDCE0B")]
		[ComImport]
		public interface IHTMLRectCollection
		{
			[return: MarshalAs(UnmanagedType.I4)]
			int GetLength();

			[return: MarshalAs(UnmanagedType.Interface)]
			object Get_newEnum();

			[return: MarshalAs(UnmanagedType.Struct)]
			object Item([In] ref object pvarIndex);
		}

		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[Guid("3050F5DA-98B5-11CF-BB82-00AA00BDCE0B")]
		[ComVisible(true)]
		[ComImport]
		public interface IHTMLDOMNode
		{
			[return: MarshalAs(UnmanagedType.I4)]
			int GetNodeType();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode GetParentNode();

			[return: MarshalAs(UnmanagedType.Bool)]
			bool HasChildNodes();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetChildNodes();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetAttributes();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode InsertBefore([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IHTMLDOMNode newChild, [MarshalAs(UnmanagedType.Struct)] [In] object refChild);

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode RemoveChild([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IHTMLDOMNode oldChild);

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode ReplaceChild([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IHTMLDOMNode newChild, [MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IHTMLDOMNode oldChild);

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode CloneNode([MarshalAs(UnmanagedType.Bool)] [In] bool fDeep);

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode RemoveNode([MarshalAs(UnmanagedType.Bool)] [In] bool fDeep);

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode SwapNode([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IHTMLDOMNode otherNode);

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode ReplaceNode([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IHTMLDOMNode replacement);

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode AppendChild([MarshalAs(UnmanagedType.Interface)] [In] NativeMethods.IHTMLDOMNode newChild);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetNodeName();

			void SetNodeValue([MarshalAs(UnmanagedType.Struct)] [In] object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetNodeValue();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode GetFirstChild();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode GetLastChild();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode GetPreviousSibling();

			[return: MarshalAs(UnmanagedType.Interface)]
			NativeMethods.IHTMLDOMNode GetNextSibling();
		}

		[StructLayout(LayoutKind.Sequential)]
		public class HDHITTESTINFO
		{
			public int pt_x;

			public int pt_y;

			public int flags;

			public int iItem;
		}

		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagOLEVERB
		{
			[MarshalAs(UnmanagedType.I4)]
			public int lVerb;

			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpszVerbName;

			[MarshalAs(UnmanagedType.U4)]
			public int fuFlags;

			[MarshalAs(UnmanagedType.U4)]
			public int grfAttribs;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
		public class TV_HITTESTINFO
		{
			public int pt_x;

			public int pt_y;

			public int flags;

			public int hItem;
		}

		public delegate int ListViewCompareCallback(IntPtr lParam1, IntPtr lParam2, IntPtr lParamSort);

		public delegate void TimerProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		public delegate IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		internal class Util
		{
			public static int MAKELONG(int low, int high)
			{
				return (high << 16) | (low & 65535);
			}

			public static int MAKELPARAM(int low, int high)
			{
				return (high << 16) | (low & 65535);
			}

			public static int HIWORD(int n)
			{
				return (n >> 16) & 65535;
			}

			public static int LOWORD(int n)
			{
				return n & 65535;
			}

			public static int SignedHIWORD(int n)
			{
				int num = (int)((short)((n >> 16) & 65535));
				num <<= 16;
				return num >> 16;
			}

			public static int SignedLOWORD(int n)
			{
				int num = (int)((short)(n & 65535));
				num <<= 16;
				return num >> 16;
			}

			public static int SignedHIWORD(IntPtr n)
			{
				return NativeMethods.Util.SignedHIWORD((int)(long)n);
			}

			public static int SignedLOWORD(IntPtr n)
			{
				return NativeMethods.Util.SignedLOWORD((int)(long)n);
			}

			[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
			private static extern int lstrlen(string s);

			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			internal static extern int RegisterWindowMessage(string msg);
		}

		public sealed class CommonHandles
		{
			public static readonly int Accelerator = global::System.Internal.HandleCollector.RegisterType("Accelerator", 80, 50);

			public static readonly int Cursor = global::System.Internal.HandleCollector.RegisterType("Cursor", 20, 500);

			public static readonly int EMF = global::System.Internal.HandleCollector.RegisterType("EnhancedMetaFile", 20, 500);

			public static readonly int Find = global::System.Internal.HandleCollector.RegisterType("Find", 0, 1000);

			public static readonly int GDI = global::System.Internal.HandleCollector.RegisterType("GDI", 90, 50);

			public static readonly int HDC = global::System.Internal.HandleCollector.RegisterType("HDC", 100, 2);

			public static readonly int Icon = global::System.Internal.HandleCollector.RegisterType("Icon", 20, 500);

			public static readonly int Kernel = global::System.Internal.HandleCollector.RegisterType("Kernel", 0, 1000);

			public static readonly int Menu = global::System.Internal.HandleCollector.RegisterType("Menu", 30, 1000);

			public static readonly int Window = global::System.Internal.HandleCollector.RegisterType("Window", 5, 1000);
		}

		internal class ActiveX
		{
			public const int OCM__BASE = 8192;

			public const int DISPID_VALUE = 0;

			public const int DISPID_UNKNOWN = -1;

			public const int DISPID_AUTOSIZE = -500;

			public const int DISPID_BACKCOLOR = -501;

			public const int DISPID_BACKSTYLE = -502;

			public const int DISPID_BORDERCOLOR = -503;

			public const int DISPID_BORDERSTYLE = -504;

			public const int DISPID_BORDERWIDTH = -505;

			public const int DISPID_DRAWMODE = -507;

			public const int DISPID_DRAWSTYLE = -508;

			public const int DISPID_DRAWWIDTH = -509;

			public const int DISPID_FILLCOLOR = -510;

			public const int DISPID_FILLSTYLE = -511;

			public const int DISPID_FONT = -512;

			public const int DISPID_FORECOLOR = -513;

			public const int DISPID_ENABLED = -514;

			public const int DISPID_HWND = -515;

			public const int DISPID_TABSTOP = -516;

			public const int DISPID_TEXT = -517;

			public const int DISPID_CAPTION = -518;

			public const int DISPID_BORDERVISIBLE = -519;

			public const int DISPID_APPEARANCE = -520;

			public const int DISPID_MOUSEPOINTER = -521;

			public const int DISPID_MOUSEICON = -522;

			public const int DISPID_PICTURE = -523;

			public const int DISPID_VALID = -524;

			public const int DISPID_READYSTATE = -525;

			public const int DISPID_REFRESH = -550;

			public const int DISPID_DOCLICK = -551;

			public const int DISPID_ABOUTBOX = -552;

			public const int DISPID_CLICK = -600;

			public const int DISPID_DBLCLICK = -601;

			public const int DISPID_KEYDOWN = -602;

			public const int DISPID_KEYPRESS = -603;

			public const int DISPID_KEYUP = -604;

			public const int DISPID_MOUSEDOWN = -605;

			public const int DISPID_MOUSEMOVE = -606;

			public const int DISPID_MOUSEUP = -607;

			public const int DISPID_ERROREVENT = -608;

			public const int DISPID_RIGHTTOLEFT = -611;

			public const int DISPID_READYSTATECHANGE = -609;

			public const int DISPID_AMBIENT_BACKCOLOR = -701;

			public const int DISPID_AMBIENT_DISPLAYNAME = -702;

			public const int DISPID_AMBIENT_FONT = -703;

			public const int DISPID_AMBIENT_FORECOLOR = -704;

			public const int DISPID_AMBIENT_LOCALEID = -705;

			public const int DISPID_AMBIENT_MESSAGEREFLECT = -706;

			public const int DISPID_AMBIENT_SCALEUNITS = -707;

			public const int DISPID_AMBIENT_TEXTALIGN = -708;

			public const int DISPID_AMBIENT_USERMODE = -709;

			public const int DISPID_AMBIENT_UIDEAD = -710;

			public const int DISPID_AMBIENT_SHOWGRABHANDLES = -711;

			public const int DISPID_AMBIENT_SHOWHATCHING = -712;

			public const int DISPID_AMBIENT_DISPLAYASDEFAULT = -713;

			public const int DISPID_AMBIENT_SUPPORTSMNEMONICS = -714;

			public const int DISPID_AMBIENT_AUTOCLIP = -715;

			public const int DISPID_AMBIENT_APPEARANCE = -716;

			public const int DISPID_AMBIENT_PALETTE = -726;

			public const int DISPID_AMBIENT_TRANSFERPRIORITY = -728;

			public const int DISPID_Name = -800;

			public const int DISPID_Delete = -801;

			public const int DISPID_Object = -802;

			public const int DISPID_Parent = -803;

			public const int DVASPECT_CONTENT = 1;

			public const int DVASPECT_THUMBNAIL = 2;

			public const int DVASPECT_ICON = 4;

			public const int DVASPECT_DOCPRINT = 8;

			public const int OLEMISC_RECOMPOSEONRESIZE = 1;

			public const int OLEMISC_ONLYICONIC = 2;

			public const int OLEMISC_INSERTNOTREPLACE = 4;

			public const int OLEMISC_STATIC = 8;

			public const int OLEMISC_CANTLINKINSIDE = 16;

			public const int OLEMISC_CANLINKBYOLE1 = 32;

			public const int OLEMISC_ISLINKOBJECT = 64;

			public const int OLEMISC_INSIDEOUT = 128;

			public const int OLEMISC_ACTIVATEWHENVISIBLE = 256;

			public const int OLEMISC_RENDERINGISDEVICEINDEPENDENT = 512;

			public const int OLEMISC_INVISIBLEATRUNTIME = 1024;

			public const int OLEMISC_ALWAYSRUN = 2048;

			public const int OLEMISC_ACTSLIKEBUTTON = 4096;

			public const int OLEMISC_ACTSLIKELABEL = 8192;

			public const int OLEMISC_NOUIACTIVATE = 16384;

			public const int OLEMISC_ALIGNABLE = 32768;

			public const int OLEMISC_SIMPLEFRAME = 65536;

			public const int OLEMISC_SETCLIENTSITEFIRST = 131072;

			public const int OLEMISC_IMEMODE = 262144;

			public const int OLEMISC_IGNOREACTIVATEWHENVISIBLE = 524288;

			public const int OLEMISC_WANTSTOMENUMERGE = 1048576;

			public const int OLEMISC_SUPPORTSMULTILEVELUNDO = 2097152;

			public const int QACONTAINER_SHOWHATCHING = 1;

			public const int QACONTAINER_SHOWGRABHANDLES = 2;

			public const int QACONTAINER_USERMODE = 4;

			public const int QACONTAINER_DISPLAYASDEFAULT = 8;

			public const int QACONTAINER_UIDEAD = 16;

			public const int QACONTAINER_AUTOCLIP = 32;

			public const int QACONTAINER_MESSAGEREFLECT = 64;

			public const int QACONTAINER_SUPPORTSMNEMONICS = 128;

			public const int XFORMCOORDS_POSITION = 1;

			public const int XFORMCOORDS_SIZE = 2;

			public const int XFORMCOORDS_HIMETRICTOCONTAINER = 4;

			public const int XFORMCOORDS_CONTAINERTOHIMETRIC = 8;

			public const int PROPCAT_Nil = -1;

			public const int PROPCAT_Misc = -2;

			public const int PROPCAT_Font = -3;

			public const int PROPCAT_Position = -4;

			public const int PROPCAT_Appearance = -5;

			public const int PROPCAT_Behavior = -6;

			public const int PROPCAT_Data = -7;

			public const int PROPCAT_List = -8;

			public const int PROPCAT_Text = -9;

			public const int PROPCAT_Scale = -10;

			public const int PROPCAT_DDE = -11;

			public const int GC_WCH_SIBLING = 1;

			public const int GC_WCH_CONTAINER = 2;

			public const int GC_WCH_CONTAINED = 3;

			public const int GC_WCH_ALL = 4;

			public const int GC_WCH_FREVERSEDIR = 134217728;

			public const int GC_WCH_FONLYNEXT = 268435456;

			public const int GC_WCH_FONLYPREV = 536870912;

			public const int GC_WCH_FSELECTED = 1073741824;

			public const int OLECONTF_EMBEDDINGS = 1;

			public const int OLECONTF_LINKS = 2;

			public const int OLECONTF_OTHERS = 4;

			public const int OLECONTF_ONLYUSER = 8;

			public const int OLECONTF_ONLYIFRUNNING = 16;

			public const int ALIGN_MIN = 0;

			public const int ALIGN_NO_CHANGE = 0;

			public const int ALIGN_TOP = 1;

			public const int ALIGN_BOTTOM = 2;

			public const int ALIGN_LEFT = 3;

			public const int ALIGN_RIGHT = 4;

			public const int ALIGN_MAX = 4;

			public const int OLEVERBATTRIB_NEVERDIRTIES = 1;

			public const int OLEVERBATTRIB_ONCONTAINERMENU = 2;

			public static Guid IID_IUnknown = new Guid("{00000000-0000-0000-C000-000000000046}");
		}

		[Guid("00000104-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IEnumOLEVERB
		{
			[PreserveSig]
			int Next([MarshalAs(UnmanagedType.U4)] int celt, [In] [Out] NativeMethods.tagOLEVERB rgelt, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pceltFetched);

			[PreserveSig]
			int Skip([MarshalAs(UnmanagedType.U4)] [In] int celt);

			void Reset();

			void Clone(out NativeMethods.IEnumOLEVERB ppenum);
		}

		[Guid("00000105-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IEnumSTATDATA
		{
			void Next([MarshalAs(UnmanagedType.U4)] [In] int celt, [Out] NativeMethods.STATDATA rgelt, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pceltFetched);

			void Skip([MarshalAs(UnmanagedType.U4)] [In] int celt);

			void Reset();

			void Clone([MarshalAs(UnmanagedType.LPArray)] [Out] NativeMethods.IEnumSTATDATA[] ppenum);
		}

		[StructLayout(LayoutKind.Sequential)]
		public sealed class STATDATA
		{
			[MarshalAs(UnmanagedType.U4)]
			public int advf;

			[MarshalAs(UnmanagedType.U4)]
			public int dwConnection;
		}

		[StructLayout(LayoutKind.Sequential)]
		public class CHARRANGE
		{
			public int cpMin;

			public int cpMax;
		}

		[StructLayout(LayoutKind.Sequential)]
		public class STATSTG
		{
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pwcsName;

			public int type;

			[MarshalAs(UnmanagedType.I8)]
			public long cbSize;

			[MarshalAs(UnmanagedType.I8)]
			public long mtime;

			[MarshalAs(UnmanagedType.I8)]
			public long ctime;

			[MarshalAs(UnmanagedType.I8)]
			public long atime;

			[MarshalAs(UnmanagedType.I4)]
			public int grfMode;

			[MarshalAs(UnmanagedType.I4)]
			public int grfLocksSupported;

			public int clsid_data1;

			[MarshalAs(UnmanagedType.I2)]
			public short clsid_data2;

			[MarshalAs(UnmanagedType.I2)]
			public short clsid_data3;

			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b0;

			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b1;

			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b2;

			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b3;

			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b4;

			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b5;

			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b6;

			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b7;

			[MarshalAs(UnmanagedType.I4)]
			public int grfStateBits;

			[MarshalAs(UnmanagedType.I4)]
			public int reserved;
		}

		[StructLayout(LayoutKind.Sequential)]
		public class FILETIME
		{
			public int dwLowDateTime;

			public int dwHighDateTime;
		}

		[Guid("00000103-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IEnumFORMATETC
		{
			[PreserveSig]
			int Next([MarshalAs(UnmanagedType.U4)] [In] int celt, [Out] NativeMethods.FORMATETC rgelt, [MarshalAs(UnmanagedType.LPArray)] [In] [Out] int[] pceltFetched);

			[PreserveSig]
			int Skip([MarshalAs(UnmanagedType.U4)] [In] int celt);

			[PreserveSig]
			int Reset();

			[PreserveSig]
			int Clone([MarshalAs(UnmanagedType.LPArray)] [Out] NativeMethods.IEnumFORMATETC[] ppenum);
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class LOGFONT
		{
			public LOGFONT()
			{
			}

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

			public override string ToString()
			{
				return string.Concat(new object[]
				{
					"lfHeight=", this.lfHeight, ", lfWidth=", this.lfWidth, ", lfEscapement=", this.lfEscapement, ", lfOrientation=", this.lfOrientation, ", lfWeight=", this.lfWeight,
					", lfItalic=", this.lfItalic, ", lfUnderline=", this.lfUnderline, ", lfStrikeOut=", this.lfStrikeOut, ", lfCharSet=", this.lfCharSet, ", lfOutPrecision=", this.lfOutPrecision,
					", lfClipPrecision=", this.lfClipPrecision, ", lfQuality=", this.lfQuality, ", lfPitchAndFamily=", this.lfPitchAndFamily, ", lfFaceName=", this.lfFaceName
				});
			}

			public int lfHeight;

			public int lfWidth;

			public int lfEscapement;

			public int lfOrientation;

			public int lfWeight;

			public byte lfItalic;

			public byte lfUnderline;

			public byte lfStrikeOut;

			public byte lfCharSet;

			public byte lfOutPrecision;

			public byte lfClipPrecision;

			public byte lfQuality;

			public byte lfPitchAndFamily;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string lfFaceName;
		}

		[StructLayout(LayoutKind.Sequential)]
		public class NONCLIENTMETRICS
		{
			public int cbSize = Marshal.SizeOf(typeof(NativeMethods.NONCLIENTMETRICS));

			public int iBorderWidth;

			public int iScrollWidth;

			public int iScrollHeight;

			public int iCaptionWidth;

			public int iCaptionHeight;

			[MarshalAs(UnmanagedType.Struct)]
			public NativeMethods.LOGFONT lfCaptionFont;

			public int iSmCaptionWidth;

			public int iSmCaptionHeight;

			[MarshalAs(UnmanagedType.Struct)]
			public NativeMethods.LOGFONT lfSmCaptionFont;

			public int iMenuWidth;

			public int iMenuHeight;

			[MarshalAs(UnmanagedType.Struct)]
			public NativeMethods.LOGFONT lfMenuFont;

			[MarshalAs(UnmanagedType.Struct)]
			public NativeMethods.LOGFONT lfStatusFont;

			[MarshalAs(UnmanagedType.Struct)]
			public NativeMethods.LOGFONT lfMessageFont;
		}
	}
}
