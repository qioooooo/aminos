using System;
using System.Internal;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Text;

namespace System.Design
{
	[SuppressUnmanagedCodeSecurity]
	internal class UnsafeNativeMethods
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int ClientToScreen(HandleRef hWnd, [In] [Out] NativeMethods.POINT pt);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hwnd, int msg, bool wparam, int lparam);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetActiveWindow();

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetMessageTime();

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr SetActiveWindow(HandleRef hWnd);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern void NotifyWinEvent(int winEvent, HandleRef hwnd, int objType, int objID);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr SetFocus(HandleRef hWnd);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetFocus();

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool IsChild(HandleRef hWndParent, HandleRef hwnd);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetWindowText(HandleRef hWnd, StringBuilder lpString, int nMaxCount);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int MsgWaitForMultipleObjectsEx(int nCount, IntPtr pHandles, int dwMilliseconds, int dwWakeMask, int dwFlags);

		[DllImport("ole32.dll")]
		public static extern int ReadClassStg(HandleRef pStg, [In] [Out] ref Guid pclsid);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GetStockObject(int nIndex);

		[DllImport("oleacc.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr LresultFromObject(ref Guid refiid, IntPtr wParam, IntPtr pAcc);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr BeginPaint(IntPtr hWnd, [In] [Out] ref UnsafeNativeMethods.PAINTSTRUCT lpPaint);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool EndPaint(IntPtr hWnd, ref UnsafeNativeMethods.PAINTSTRUCT lpPaint);

		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetDC", ExactSpelling = true)]
		private static extern IntPtr IntGetDC(HandleRef hWnd);

		public static IntPtr GetDC(HandleRef hWnd)
		{
			return global::System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntGetDC(hWnd), NativeMethods.CommonHandles.HDC);
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "ReleaseDC", ExactSpelling = true)]
		private static extern int IntReleaseDC(HandleRef hWnd, HandleRef hDC);

		public static int ReleaseDC(HandleRef hWnd, HandleRef hDC)
		{
			global::System.Internal.HandleCollector.Remove((IntPtr)hDC, NativeMethods.CommonHandles.HDC);
			return UnsafeNativeMethods.IntReleaseDC(hWnd, hDC);
		}

		[DllImport("shell32.dll")]
		public static extern IntPtr ExtractIcon(HandleRef hMod, string exeName, int index);

		[DllImport("user32.dll")]
		public static extern bool DestroyIcon(HandleRef hIcon);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SetWindowsHookEx(int hookid, UnsafeNativeMethods.HookProc pfnhook, HandleRef hinst, int threadid);

		public static IntPtr GetWindowLong(HandleRef hWnd, int nIndex)
		{
			if (IntPtr.Size == 4)
			{
				return UnsafeNativeMethods.GetWindowLong32(hWnd, nIndex);
			}
			return UnsafeNativeMethods.GetWindowLongPtr64(hWnd, nIndex);
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLong")]
		public static extern IntPtr GetWindowLong32(HandleRef hWnd, int nIndex);

		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLongPtr")]
		public static extern IntPtr GetWindowLongPtr64(HandleRef hWnd, int nIndex);

		public static IntPtr SetWindowLong(HandleRef hWnd, int nIndex, HandleRef dwNewLong)
		{
			if (IntPtr.Size == 4)
			{
				return UnsafeNativeMethods.SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
			}
			return UnsafeNativeMethods.SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLong")]
		public static extern IntPtr SetWindowLongPtr32(HandleRef hWnd, int nIndex, HandleRef dwNewLong);

		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtr")]
		public static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, HandleRef dwNewLong);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool UnhookWindowsHookEx(HandleRef hhook);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetWindowThreadProcessId(HandleRef hWnd, out int lpdwProcessId);

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr CallNextHookEx(HandleRef hhook, int code, IntPtr wparam, IntPtr lparam);

		[DllImport("ole32.dll", PreserveSig = false)]
		public static extern UnsafeNativeMethods.ILockBytes CreateILockBytesOnHGlobal(HandleRef hGlobal, bool fDeleteOnRelease);

		[DllImport("ole32.dll", PreserveSig = false)]
		public static extern UnsafeNativeMethods.IStorage StgCreateDocfileOnILockBytes(UnsafeNativeMethods.ILockBytes iLockBytes, int grfMode, int reserved);

		[Flags]
		public enum BrowseInfos
		{
			ReturnOnlyFSDirs = 1,
			DontGoBelowDomain = 2,
			StatusText = 4,
			ReturnFSAncestors = 8,
			EditBox = 16,
			Validate = 32,
			NewDialogStyle = 64,
			UseNewUI = 80,
			AllowUrls = 128,
			BrowseForComputer = 4096,
			BrowseForPrinter = 8192,
			BrowseForEverything = 16384,
			ShowShares = 32768
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class BROWSEINFO
		{
			public IntPtr hwndOwner;

			public IntPtr pidlRoot;

			public IntPtr pszDisplayName;

			public string lpszTitle;

			public int ulFlags;

			public IntPtr lpfn;

			public IntPtr lParam;

			public int iImage;
		}

		public class Shell32
		{
			[DllImport("shell32.dll")]
			public static extern int SHGetSpecialFolderLocation(IntPtr hwnd, int csidl, ref IntPtr ppidl);

			[DllImport("shell32.dll", CharSet = CharSet.Auto)]
			public static extern bool SHGetPathFromIDList(IntPtr pidl, IntPtr pszPath);

			[DllImport("shell32.dll", CharSet = CharSet.Auto)]
			public static extern IntPtr SHBrowseForFolder([In] UnsafeNativeMethods.BROWSEINFO lpbi);

			[DllImport("shell32.dll")]
			public static extern int SHGetMalloc([MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.IMalloc[] ppMalloc);
		}

		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("00000002-0000-0000-c000-000000000046")]
		[ComImport]
		public interface IMalloc
		{
			[PreserveSig]
			IntPtr Alloc(int cb);

			[PreserveSig]
			IntPtr Realloc(IntPtr pv, int cb);

			[PreserveSig]
			void Free(IntPtr pv);

			[PreserveSig]
			int GetSize(IntPtr pv);

			[PreserveSig]
			int DidAlloc(IntPtr pv);

			[PreserveSig]
			void HeapMinimize();
		}

		public struct PAINTSTRUCT
		{
			public IntPtr hdc;

			public bool fErase;

			public int rcPaint_left;

			public int rcPaint_top;

			public int rcPaint_right;

			public int rcPaint_bottom;

			public bool fRestore;

			public bool fIncUpdate;

			public int reserved1;

			public int reserved2;

			public int reserved3;

			public int reserved4;

			public int reserved5;

			public int reserved6;

			public int reserved7;

			public int reserved8;
		}

		public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

		[Guid("00020D03-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IRichEditOleCallback
		{
		}

		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("00020D03-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IRichTextBoxOleCallback
		{
			[PreserveSig]
			int GetNewStorage(out UnsafeNativeMethods.IStorage ret);

			[PreserveSig]
			int GetInPlaceContext(IntPtr lplpFrame, IntPtr lplpDoc, IntPtr lpFrameInfo);

			[PreserveSig]
			int ShowContainerUI(int fShow);

			[PreserveSig]
			int QueryInsertObject(ref Guid lpclsid, IntPtr lpstg, int cp);

			[PreserveSig]
			int DeleteObject(IntPtr lpoleobj);

			[PreserveSig]
			int QueryAcceptData(IDataObject lpdataobj, IntPtr lpcfFormat, int reco, int fReally, IntPtr hMetaPict);

			[PreserveSig]
			int ContextSensitiveHelp(int fEnterMode);

			[PreserveSig]
			int GetClipboardData(NativeMethods.CHARRANGE lpchrg, int reco, IntPtr lplpdataobj);

			[PreserveSig]
			int GetDragDropEffect(bool fDrag, int grfKeyState, ref int pdwEffect);

			[PreserveSig]
			int GetContextMenu(short seltype, IntPtr lpoleobj, NativeMethods.CHARRANGE lpchrg, out IntPtr hmenu);
		}

		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("0000000B-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IStorage
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IStream CreateStream([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName, [MarshalAs(UnmanagedType.U4)] [In] int grfMode, [MarshalAs(UnmanagedType.U4)] [In] int reserved1, [MarshalAs(UnmanagedType.U4)] [In] int reserved2);

			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IStream OpenStream([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName, IntPtr reserved1, [MarshalAs(UnmanagedType.U4)] [In] int grfMode, [MarshalAs(UnmanagedType.U4)] [In] int reserved2);

			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IStorage CreateStorage([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName, [MarshalAs(UnmanagedType.U4)] [In] int grfMode, [MarshalAs(UnmanagedType.U4)] [In] int reserved1, [MarshalAs(UnmanagedType.U4)] [In] int reserved2);

			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IStorage OpenStorage([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName, IntPtr pstgPriority, [MarshalAs(UnmanagedType.U4)] [In] int grfMode, IntPtr snbExclude, [MarshalAs(UnmanagedType.U4)] [In] int reserved);

			void CopyTo(int ciidExclude, [MarshalAs(UnmanagedType.LPArray)] [In] Guid[] pIIDExclude, IntPtr snbExclude, [MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStorage stgDest);

			void MoveElementTo([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName, [MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStorage stgDest, [MarshalAs(UnmanagedType.BStr)] [In] string pwcsNewName, [MarshalAs(UnmanagedType.U4)] [In] int grfFlags);

			void Commit(int grfCommitFlags);

			void Revert();

			void EnumElements([MarshalAs(UnmanagedType.U4)] [In] int reserved1, IntPtr reserved2, [MarshalAs(UnmanagedType.U4)] [In] int reserved3, [MarshalAs(UnmanagedType.Interface)] out object ppVal);

			void DestroyElement([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName);

			void RenameElement([MarshalAs(UnmanagedType.BStr)] [In] string pwcsOldName, [MarshalAs(UnmanagedType.BStr)] [In] string pwcsNewName);

			void SetElementTimes([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName, [In] NativeMethods.FILETIME pctime, [In] NativeMethods.FILETIME patime, [In] NativeMethods.FILETIME pmtime);

			void SetClass([In] ref Guid clsid);

			void SetStateBits(int grfStateBits, int grfMask);

			void Stat([Out] NativeMethods.STATSTG pStatStg, int grfStatFlag);
		}

		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("0000000C-0000-0000-C000-000000000046")]
		[SuppressUnmanagedCodeSecurity]
		[ComImport]
		public interface IStream
		{
			int Read(IntPtr buf, int len);

			int Write(IntPtr buf, int len);

			[return: MarshalAs(UnmanagedType.I8)]
			long Seek([MarshalAs(UnmanagedType.I8)] [In] long dlibMove, int dwOrigin);

			void SetSize([MarshalAs(UnmanagedType.I8)] [In] long libNewSize);

			[return: MarshalAs(UnmanagedType.I8)]
			long CopyTo([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStream pstm, [MarshalAs(UnmanagedType.I8)] [In] long cb, [MarshalAs(UnmanagedType.LPArray)] [Out] long[] pcbRead);

			void Commit(int grfCommitFlags);

			void Revert();

			void LockRegion([MarshalAs(UnmanagedType.I8)] [In] long libOffset, [MarshalAs(UnmanagedType.I8)] [In] long cb, int dwLockType);

			void UnlockRegion([MarshalAs(UnmanagedType.I8)] [In] long libOffset, [MarshalAs(UnmanagedType.I8)] [In] long cb, int dwLockType);

			void Stat([Out] NativeMethods.STATSTG pStatstg, int grfStatFlag);

			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IStream Clone();
		}

		[Guid("0000000A-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface ILockBytes
		{
			void ReadAt([MarshalAs(UnmanagedType.U8)] [In] long ulOffset, [Out] IntPtr pv, [MarshalAs(UnmanagedType.U4)] [In] int cb, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pcbRead);

			void WriteAt([MarshalAs(UnmanagedType.U8)] [In] long ulOffset, IntPtr pv, [MarshalAs(UnmanagedType.U4)] [In] int cb, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pcbWritten);

			void Flush();

			void SetSize([MarshalAs(UnmanagedType.U8)] [In] long cb);

			void LockRegion([MarshalAs(UnmanagedType.U8)] [In] long libOffset, [MarshalAs(UnmanagedType.U8)] [In] long cb, [MarshalAs(UnmanagedType.U4)] [In] int dwLockType);

			void UnlockRegion([MarshalAs(UnmanagedType.U8)] [In] long libOffset, [MarshalAs(UnmanagedType.U8)] [In] long cb, [MarshalAs(UnmanagedType.U4)] [In] int dwLockType);

			void Stat([Out] NativeMethods.STATSTG pstatstg, [MarshalAs(UnmanagedType.U4)] [In] int grfStatFlag);
		}
	}
}
