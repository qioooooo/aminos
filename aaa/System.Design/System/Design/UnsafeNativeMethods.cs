using System;
using System.Internal;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Text;

namespace System.Design
{
	// Token: 0x0200005A RID: 90
	[SuppressUnmanagedCodeSecurity]
	internal class UnsafeNativeMethods
	{
		// Token: 0x060003DB RID: 987
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int ClientToScreen(HandleRef hWnd, [In] [Out] NativeMethods.POINT pt);

		// Token: 0x060003DC RID: 988
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

		// Token: 0x060003DD RID: 989
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hwnd, int msg, bool wparam, int lparam);

		// Token: 0x060003DE RID: 990
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetActiveWindow();

		// Token: 0x060003DF RID: 991
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetMessageTime();

		// Token: 0x060003E0 RID: 992
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr SetActiveWindow(HandleRef hWnd);

		// Token: 0x060003E1 RID: 993
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern void NotifyWinEvent(int winEvent, HandleRef hwnd, int objType, int objID);

		// Token: 0x060003E2 RID: 994
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr SetFocus(HandleRef hWnd);

		// Token: 0x060003E3 RID: 995
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetFocus();

		// Token: 0x060003E4 RID: 996
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool IsChild(HandleRef hWndParent, HandleRef hwnd);

		// Token: 0x060003E5 RID: 997
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetWindowText(HandleRef hWnd, StringBuilder lpString, int nMaxCount);

		// Token: 0x060003E6 RID: 998
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int MsgWaitForMultipleObjectsEx(int nCount, IntPtr pHandles, int dwMilliseconds, int dwWakeMask, int dwFlags);

		// Token: 0x060003E7 RID: 999
		[DllImport("ole32.dll")]
		public static extern int ReadClassStg(HandleRef pStg, [In] [Out] ref Guid pclsid);

		// Token: 0x060003E8 RID: 1000
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GetStockObject(int nIndex);

		// Token: 0x060003E9 RID: 1001
		[DllImport("oleacc.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr LresultFromObject(ref Guid refiid, IntPtr wParam, IntPtr pAcc);

		// Token: 0x060003EA RID: 1002
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr BeginPaint(IntPtr hWnd, [In] [Out] ref UnsafeNativeMethods.PAINTSTRUCT lpPaint);

		// Token: 0x060003EB RID: 1003
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool EndPaint(IntPtr hWnd, ref UnsafeNativeMethods.PAINTSTRUCT lpPaint);

		// Token: 0x060003EC RID: 1004
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetDC", ExactSpelling = true)]
		private static extern IntPtr IntGetDC(HandleRef hWnd);

		// Token: 0x060003ED RID: 1005 RVA: 0x00002E86 File Offset: 0x00001E86
		public static IntPtr GetDC(HandleRef hWnd)
		{
			return global::System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntGetDC(hWnd), NativeMethods.CommonHandles.HDC);
		}

		// Token: 0x060003EE RID: 1006
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "ReleaseDC", ExactSpelling = true)]
		private static extern int IntReleaseDC(HandleRef hWnd, HandleRef hDC);

		// Token: 0x060003EF RID: 1007 RVA: 0x00002E98 File Offset: 0x00001E98
		public static int ReleaseDC(HandleRef hWnd, HandleRef hDC)
		{
			global::System.Internal.HandleCollector.Remove((IntPtr)hDC, NativeMethods.CommonHandles.HDC);
			return UnsafeNativeMethods.IntReleaseDC(hWnd, hDC);
		}

		// Token: 0x060003F0 RID: 1008
		[DllImport("shell32.dll")]
		public static extern IntPtr ExtractIcon(HandleRef hMod, string exeName, int index);

		// Token: 0x060003F1 RID: 1009
		[DllImport("user32.dll")]
		public static extern bool DestroyIcon(HandleRef hIcon);

		// Token: 0x060003F2 RID: 1010
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SetWindowsHookEx(int hookid, UnsafeNativeMethods.HookProc pfnhook, HandleRef hinst, int threadid);

		// Token: 0x060003F3 RID: 1011 RVA: 0x00002EB2 File Offset: 0x00001EB2
		public static IntPtr GetWindowLong(HandleRef hWnd, int nIndex)
		{
			if (IntPtr.Size == 4)
			{
				return UnsafeNativeMethods.GetWindowLong32(hWnd, nIndex);
			}
			return UnsafeNativeMethods.GetWindowLongPtr64(hWnd, nIndex);
		}

		// Token: 0x060003F4 RID: 1012
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLong")]
		public static extern IntPtr GetWindowLong32(HandleRef hWnd, int nIndex);

		// Token: 0x060003F5 RID: 1013
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLongPtr")]
		public static extern IntPtr GetWindowLongPtr64(HandleRef hWnd, int nIndex);

		// Token: 0x060003F6 RID: 1014 RVA: 0x00002ECB File Offset: 0x00001ECB
		public static IntPtr SetWindowLong(HandleRef hWnd, int nIndex, HandleRef dwNewLong)
		{
			if (IntPtr.Size == 4)
			{
				return UnsafeNativeMethods.SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
			}
			return UnsafeNativeMethods.SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
		}

		// Token: 0x060003F7 RID: 1015
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLong")]
		public static extern IntPtr SetWindowLongPtr32(HandleRef hWnd, int nIndex, HandleRef dwNewLong);

		// Token: 0x060003F8 RID: 1016
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtr")]
		public static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, HandleRef dwNewLong);

		// Token: 0x060003F9 RID: 1017
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool UnhookWindowsHookEx(HandleRef hhook);

		// Token: 0x060003FA RID: 1018
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetWindowThreadProcessId(HandleRef hWnd, out int lpdwProcessId);

		// Token: 0x060003FB RID: 1019
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr CallNextHookEx(HandleRef hhook, int code, IntPtr wparam, IntPtr lparam);

		// Token: 0x060003FC RID: 1020
		[DllImport("ole32.dll", PreserveSig = false)]
		public static extern UnsafeNativeMethods.ILockBytes CreateILockBytesOnHGlobal(HandleRef hGlobal, bool fDeleteOnRelease);

		// Token: 0x060003FD RID: 1021
		[DllImport("ole32.dll", PreserveSig = false)]
		public static extern UnsafeNativeMethods.IStorage StgCreateDocfileOnILockBytes(UnsafeNativeMethods.ILockBytes iLockBytes, int grfMode, int reserved);

		// Token: 0x0200005B RID: 91
		[Flags]
		public enum BrowseInfos
		{
			// Token: 0x04000A42 RID: 2626
			ReturnOnlyFSDirs = 1,
			// Token: 0x04000A43 RID: 2627
			DontGoBelowDomain = 2,
			// Token: 0x04000A44 RID: 2628
			StatusText = 4,
			// Token: 0x04000A45 RID: 2629
			ReturnFSAncestors = 8,
			// Token: 0x04000A46 RID: 2630
			EditBox = 16,
			// Token: 0x04000A47 RID: 2631
			Validate = 32,
			// Token: 0x04000A48 RID: 2632
			NewDialogStyle = 64,
			// Token: 0x04000A49 RID: 2633
			UseNewUI = 80,
			// Token: 0x04000A4A RID: 2634
			AllowUrls = 128,
			// Token: 0x04000A4B RID: 2635
			BrowseForComputer = 4096,
			// Token: 0x04000A4C RID: 2636
			BrowseForPrinter = 8192,
			// Token: 0x04000A4D RID: 2637
			BrowseForEverything = 16384,
			// Token: 0x04000A4E RID: 2638
			ShowShares = 32768
		}

		// Token: 0x0200005C RID: 92
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class BROWSEINFO
		{
			// Token: 0x04000A4F RID: 2639
			public IntPtr hwndOwner;

			// Token: 0x04000A50 RID: 2640
			public IntPtr pidlRoot;

			// Token: 0x04000A51 RID: 2641
			public IntPtr pszDisplayName;

			// Token: 0x04000A52 RID: 2642
			public string lpszTitle;

			// Token: 0x04000A53 RID: 2643
			public int ulFlags;

			// Token: 0x04000A54 RID: 2644
			public IntPtr lpfn;

			// Token: 0x04000A55 RID: 2645
			public IntPtr lParam;

			// Token: 0x04000A56 RID: 2646
			public int iImage;
		}

		// Token: 0x0200005D RID: 93
		public class Shell32
		{
			// Token: 0x06000400 RID: 1024
			[DllImport("shell32.dll")]
			public static extern int SHGetSpecialFolderLocation(IntPtr hwnd, int csidl, ref IntPtr ppidl);

			// Token: 0x06000401 RID: 1025
			[DllImport("shell32.dll", CharSet = CharSet.Auto)]
			public static extern bool SHGetPathFromIDList(IntPtr pidl, IntPtr pszPath);

			// Token: 0x06000402 RID: 1026
			[DllImport("shell32.dll", CharSet = CharSet.Auto)]
			public static extern IntPtr SHBrowseForFolder([In] UnsafeNativeMethods.BROWSEINFO lpbi);

			// Token: 0x06000403 RID: 1027
			[DllImport("shell32.dll")]
			public static extern int SHGetMalloc([MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.IMalloc[] ppMalloc);
		}

		// Token: 0x0200005E RID: 94
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("00000002-0000-0000-c000-000000000046")]
		[ComImport]
		public interface IMalloc
		{
			// Token: 0x06000405 RID: 1029
			[PreserveSig]
			IntPtr Alloc(int cb);

			// Token: 0x06000406 RID: 1030
			[PreserveSig]
			IntPtr Realloc(IntPtr pv, int cb);

			// Token: 0x06000407 RID: 1031
			[PreserveSig]
			void Free(IntPtr pv);

			// Token: 0x06000408 RID: 1032
			[PreserveSig]
			int GetSize(IntPtr pv);

			// Token: 0x06000409 RID: 1033
			[PreserveSig]
			int DidAlloc(IntPtr pv);

			// Token: 0x0600040A RID: 1034
			[PreserveSig]
			void HeapMinimize();
		}

		// Token: 0x0200005F RID: 95
		public struct PAINTSTRUCT
		{
			// Token: 0x04000A57 RID: 2647
			public IntPtr hdc;

			// Token: 0x04000A58 RID: 2648
			public bool fErase;

			// Token: 0x04000A59 RID: 2649
			public int rcPaint_left;

			// Token: 0x04000A5A RID: 2650
			public int rcPaint_top;

			// Token: 0x04000A5B RID: 2651
			public int rcPaint_right;

			// Token: 0x04000A5C RID: 2652
			public int rcPaint_bottom;

			// Token: 0x04000A5D RID: 2653
			public bool fRestore;

			// Token: 0x04000A5E RID: 2654
			public bool fIncUpdate;

			// Token: 0x04000A5F RID: 2655
			public int reserved1;

			// Token: 0x04000A60 RID: 2656
			public int reserved2;

			// Token: 0x04000A61 RID: 2657
			public int reserved3;

			// Token: 0x04000A62 RID: 2658
			public int reserved4;

			// Token: 0x04000A63 RID: 2659
			public int reserved5;

			// Token: 0x04000A64 RID: 2660
			public int reserved6;

			// Token: 0x04000A65 RID: 2661
			public int reserved7;

			// Token: 0x04000A66 RID: 2662
			public int reserved8;
		}

		// Token: 0x02000060 RID: 96
		// (Invoke) Token: 0x0600040C RID: 1036
		public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

		// Token: 0x02000061 RID: 97
		[Guid("00020D03-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IRichEditOleCallback
		{
		}

		// Token: 0x02000062 RID: 98
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("00020D03-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IRichTextBoxOleCallback
		{
			// Token: 0x0600040F RID: 1039
			[PreserveSig]
			int GetNewStorage(out UnsafeNativeMethods.IStorage ret);

			// Token: 0x06000410 RID: 1040
			[PreserveSig]
			int GetInPlaceContext(IntPtr lplpFrame, IntPtr lplpDoc, IntPtr lpFrameInfo);

			// Token: 0x06000411 RID: 1041
			[PreserveSig]
			int ShowContainerUI(int fShow);

			// Token: 0x06000412 RID: 1042
			[PreserveSig]
			int QueryInsertObject(ref Guid lpclsid, IntPtr lpstg, int cp);

			// Token: 0x06000413 RID: 1043
			[PreserveSig]
			int DeleteObject(IntPtr lpoleobj);

			// Token: 0x06000414 RID: 1044
			[PreserveSig]
			int QueryAcceptData(IDataObject lpdataobj, IntPtr lpcfFormat, int reco, int fReally, IntPtr hMetaPict);

			// Token: 0x06000415 RID: 1045
			[PreserveSig]
			int ContextSensitiveHelp(int fEnterMode);

			// Token: 0x06000416 RID: 1046
			[PreserveSig]
			int GetClipboardData(NativeMethods.CHARRANGE lpchrg, int reco, IntPtr lplpdataobj);

			// Token: 0x06000417 RID: 1047
			[PreserveSig]
			int GetDragDropEffect(bool fDrag, int grfKeyState, ref int pdwEffect);

			// Token: 0x06000418 RID: 1048
			[PreserveSig]
			int GetContextMenu(short seltype, IntPtr lpoleobj, NativeMethods.CHARRANGE lpchrg, out IntPtr hmenu);
		}

		// Token: 0x02000063 RID: 99
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("0000000B-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IStorage
		{
			// Token: 0x06000419 RID: 1049
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IStream CreateStream([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName, [MarshalAs(UnmanagedType.U4)] [In] int grfMode, [MarshalAs(UnmanagedType.U4)] [In] int reserved1, [MarshalAs(UnmanagedType.U4)] [In] int reserved2);

			// Token: 0x0600041A RID: 1050
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IStream OpenStream([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName, IntPtr reserved1, [MarshalAs(UnmanagedType.U4)] [In] int grfMode, [MarshalAs(UnmanagedType.U4)] [In] int reserved2);

			// Token: 0x0600041B RID: 1051
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IStorage CreateStorage([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName, [MarshalAs(UnmanagedType.U4)] [In] int grfMode, [MarshalAs(UnmanagedType.U4)] [In] int reserved1, [MarshalAs(UnmanagedType.U4)] [In] int reserved2);

			// Token: 0x0600041C RID: 1052
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IStorage OpenStorage([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName, IntPtr pstgPriority, [MarshalAs(UnmanagedType.U4)] [In] int grfMode, IntPtr snbExclude, [MarshalAs(UnmanagedType.U4)] [In] int reserved);

			// Token: 0x0600041D RID: 1053
			void CopyTo(int ciidExclude, [MarshalAs(UnmanagedType.LPArray)] [In] Guid[] pIIDExclude, IntPtr snbExclude, [MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStorage stgDest);

			// Token: 0x0600041E RID: 1054
			void MoveElementTo([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName, [MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStorage stgDest, [MarshalAs(UnmanagedType.BStr)] [In] string pwcsNewName, [MarshalAs(UnmanagedType.U4)] [In] int grfFlags);

			// Token: 0x0600041F RID: 1055
			void Commit(int grfCommitFlags);

			// Token: 0x06000420 RID: 1056
			void Revert();

			// Token: 0x06000421 RID: 1057
			void EnumElements([MarshalAs(UnmanagedType.U4)] [In] int reserved1, IntPtr reserved2, [MarshalAs(UnmanagedType.U4)] [In] int reserved3, [MarshalAs(UnmanagedType.Interface)] out object ppVal);

			// Token: 0x06000422 RID: 1058
			void DestroyElement([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName);

			// Token: 0x06000423 RID: 1059
			void RenameElement([MarshalAs(UnmanagedType.BStr)] [In] string pwcsOldName, [MarshalAs(UnmanagedType.BStr)] [In] string pwcsNewName);

			// Token: 0x06000424 RID: 1060
			void SetElementTimes([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName, [In] NativeMethods.FILETIME pctime, [In] NativeMethods.FILETIME patime, [In] NativeMethods.FILETIME pmtime);

			// Token: 0x06000425 RID: 1061
			void SetClass([In] ref Guid clsid);

			// Token: 0x06000426 RID: 1062
			void SetStateBits(int grfStateBits, int grfMask);

			// Token: 0x06000427 RID: 1063
			void Stat([Out] NativeMethods.STATSTG pStatStg, int grfStatFlag);
		}

		// Token: 0x02000064 RID: 100
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("0000000C-0000-0000-C000-000000000046")]
		[SuppressUnmanagedCodeSecurity]
		[ComImport]
		public interface IStream
		{
			// Token: 0x06000428 RID: 1064
			int Read(IntPtr buf, int len);

			// Token: 0x06000429 RID: 1065
			int Write(IntPtr buf, int len);

			// Token: 0x0600042A RID: 1066
			[return: MarshalAs(UnmanagedType.I8)]
			long Seek([MarshalAs(UnmanagedType.I8)] [In] long dlibMove, int dwOrigin);

			// Token: 0x0600042B RID: 1067
			void SetSize([MarshalAs(UnmanagedType.I8)] [In] long libNewSize);

			// Token: 0x0600042C RID: 1068
			[return: MarshalAs(UnmanagedType.I8)]
			long CopyTo([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStream pstm, [MarshalAs(UnmanagedType.I8)] [In] long cb, [MarshalAs(UnmanagedType.LPArray)] [Out] long[] pcbRead);

			// Token: 0x0600042D RID: 1069
			void Commit(int grfCommitFlags);

			// Token: 0x0600042E RID: 1070
			void Revert();

			// Token: 0x0600042F RID: 1071
			void LockRegion([MarshalAs(UnmanagedType.I8)] [In] long libOffset, [MarshalAs(UnmanagedType.I8)] [In] long cb, int dwLockType);

			// Token: 0x06000430 RID: 1072
			void UnlockRegion([MarshalAs(UnmanagedType.I8)] [In] long libOffset, [MarshalAs(UnmanagedType.I8)] [In] long cb, int dwLockType);

			// Token: 0x06000431 RID: 1073
			void Stat([Out] NativeMethods.STATSTG pStatstg, int grfStatFlag);

			// Token: 0x06000432 RID: 1074
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IStream Clone();
		}

		// Token: 0x02000065 RID: 101
		[Guid("0000000A-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface ILockBytes
		{
			// Token: 0x06000433 RID: 1075
			void ReadAt([MarshalAs(UnmanagedType.U8)] [In] long ulOffset, [Out] IntPtr pv, [MarshalAs(UnmanagedType.U4)] [In] int cb, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pcbRead);

			// Token: 0x06000434 RID: 1076
			void WriteAt([MarshalAs(UnmanagedType.U8)] [In] long ulOffset, IntPtr pv, [MarshalAs(UnmanagedType.U4)] [In] int cb, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pcbWritten);

			// Token: 0x06000435 RID: 1077
			void Flush();

			// Token: 0x06000436 RID: 1078
			void SetSize([MarshalAs(UnmanagedType.U8)] [In] long cb);

			// Token: 0x06000437 RID: 1079
			void LockRegion([MarshalAs(UnmanagedType.U8)] [In] long libOffset, [MarshalAs(UnmanagedType.U8)] [In] long cb, [MarshalAs(UnmanagedType.U4)] [In] int dwLockType);

			// Token: 0x06000438 RID: 1080
			void UnlockRegion([MarshalAs(UnmanagedType.U8)] [In] long libOffset, [MarshalAs(UnmanagedType.U8)] [In] long cb, [MarshalAs(UnmanagedType.U4)] [In] int dwLockType);

			// Token: 0x06000439 RID: 1081
			void Stat([Out] NativeMethods.STATSTG pstatstg, [MarshalAs(UnmanagedType.U4)] [In] int grfStatFlag);
		}
	}
}
