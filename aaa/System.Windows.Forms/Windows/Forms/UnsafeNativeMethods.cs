using System;
using System.Drawing;
using System.Internal;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x0200014F RID: 335
	[SuppressUnmanagedCodeSecurity]
	internal static class UnsafeNativeMethods
	{
		// Token: 0x0600054F RID: 1359
		[DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		internal static extern uint SHLoadIndirectString(string pszSource, StringBuilder pszOutBuf, uint cchOutBuf, IntPtr ppvReserved);

		// Token: 0x06000550 RID: 1360
		[DllImport("ole32.dll")]
		public static extern int ReadClassStg(HandleRef pStg, [In] [Out] ref Guid pclsid);

		// Token: 0x06000551 RID: 1361
		[DllImport("user32.dll")]
		public static extern int GetClassName(HandleRef hwnd, StringBuilder lpClassName, int nMaxCount);

		// Token: 0x06000552 RID: 1362 RVA: 0x0000E4BA File Offset: 0x0000D4BA
		public static IntPtr SetClassLong(HandleRef hWnd, int nIndex, IntPtr dwNewLong)
		{
			if (IntPtr.Size == 4)
			{
				return UnsafeNativeMethods.SetClassLongPtr32(hWnd, nIndex, dwNewLong);
			}
			return UnsafeNativeMethods.SetClassLongPtr64(hWnd, nIndex, dwNewLong);
		}

		// Token: 0x06000553 RID: 1363
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetClassLong")]
		public static extern IntPtr SetClassLongPtr32(HandleRef hwnd, int nIndex, IntPtr dwNewLong);

		// Token: 0x06000554 RID: 1364
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetClassLongPtr")]
		public static extern IntPtr SetClassLongPtr64(HandleRef hwnd, int nIndex, IntPtr dwNewLong);

		// Token: 0x06000555 RID: 1365
		[DllImport("ole32.dll", ExactSpelling = true, PreserveSig = false)]
		public static extern UnsafeNativeMethods.IClassFactory2 CoGetClassObject([In] ref Guid clsid, int dwContext, int serverInfo, [In] ref Guid refiid);

		// Token: 0x06000556 RID: 1366
		[DllImport("ole32.dll", ExactSpelling = true, PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.Interface)]
		public static extern object CoCreateInstance([In] ref Guid clsid, [MarshalAs(UnmanagedType.Interface)] object punkOuter, int context, [In] ref Guid iid);

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x0000E4D8 File Offset: 0x0000D4D8
		internal static bool IsVista
		{
			get
			{
				OperatingSystem osversion = Environment.OSVersion;
				return osversion != null && osversion.Platform == PlatformID.Win32NT && osversion.Version.CompareTo(UnsafeNativeMethods.VistaOSVersion) >= 0;
			}
		}

		// Token: 0x06000558 RID: 1368
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern int GetLocaleInfo(int Locale, int LCType, StringBuilder lpLCData, int cchData);

		// Token: 0x06000559 RID: 1369
		[DllImport("ole32.dll")]
		public static extern int WriteClassStm(UnsafeNativeMethods.IStream pStream, ref Guid clsid);

		// Token: 0x0600055A RID: 1370
		[DllImport("ole32.dll")]
		public static extern int ReadClassStg(UnsafeNativeMethods.IStorage pStorage, out Guid clsid);

		// Token: 0x0600055B RID: 1371
		[DllImport("ole32.dll")]
		public static extern int ReadClassStm(UnsafeNativeMethods.IStream pStream, out Guid clsid);

		// Token: 0x0600055C RID: 1372
		[DllImport("ole32.dll")]
		public static extern int OleLoadFromStream(UnsafeNativeMethods.IStream pStorage, ref Guid iid, out UnsafeNativeMethods.IOleObject pObject);

		// Token: 0x0600055D RID: 1373
		[DllImport("ole32.dll")]
		public static extern int OleSaveToStream(UnsafeNativeMethods.IPersistStream pPersistStream, UnsafeNativeMethods.IStream pStream);

		// Token: 0x0600055E RID: 1374
		[DllImport("ole32.dll")]
		public static extern int CoGetMalloc(int dwReserved, out UnsafeNativeMethods.IMalloc pMalloc);

		// Token: 0x0600055F RID: 1375
		[DllImport("comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool PageSetupDlg([In] [Out] NativeMethods.PAGESETUPDLG lppsd);

		// Token: 0x06000560 RID: 1376
		[DllImport("comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool PrintDlg([In] [Out] NativeMethods.PRINTDLG lppd);

		// Token: 0x06000561 RID: 1377
		[DllImport("comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int PrintDlgEx([In] [Out] NativeMethods.PRINTDLGEX lppdex);

		// Token: 0x06000562 RID: 1378
		[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int OleGetClipboard(ref IDataObject data);

		// Token: 0x06000563 RID: 1379
		[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int OleSetClipboard(IDataObject pDataObj);

		// Token: 0x06000564 RID: 1380
		[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int OleFlushClipboard();

		// Token: 0x06000565 RID: 1381
		[DllImport("oleaut32.dll", ExactSpelling = true)]
		public static extern void OleCreatePropertyFrameIndirect(NativeMethods.OCPFIPARAMS p);

		// Token: 0x06000566 RID: 1382
		[DllImport("oleaut32.dll", EntryPoint = "OleCreateFontIndirect", ExactSpelling = true, PreserveSig = false)]
		public static extern UnsafeNativeMethods.IFont OleCreateIFontIndirect(NativeMethods.FONTDESC fd, ref Guid iid);

		// Token: 0x06000567 RID: 1383
		[DllImport("oleaut32.dll", EntryPoint = "OleCreatePictureIndirect", ExactSpelling = true, PreserveSig = false)]
		public static extern UnsafeNativeMethods.IPicture OleCreateIPictureIndirect([MarshalAs(UnmanagedType.AsAny)] object pictdesc, ref Guid iid, bool fOwn);

		// Token: 0x06000568 RID: 1384
		[DllImport("oleaut32.dll", EntryPoint = "OleCreatePictureIndirect", ExactSpelling = true, PreserveSig = false)]
		public static extern UnsafeNativeMethods.IPictureDisp OleCreateIPictureDispIndirect([MarshalAs(UnmanagedType.AsAny)] object pictdesc, ref Guid iid, bool fOwn);

		// Token: 0x06000569 RID: 1385
		[DllImport("oleaut32.dll", PreserveSig = false)]
		public static extern UnsafeNativeMethods.IPicture OleCreatePictureIndirect(NativeMethods.PICTDESC pictdesc, [In] ref Guid refiid, bool fOwn);

		// Token: 0x0600056A RID: 1386
		[DllImport("oleaut32.dll", PreserveSig = false)]
		public static extern UnsafeNativeMethods.IFont OleCreateFontIndirect(NativeMethods.tagFONTDESC fontdesc, [In] ref Guid refiid);

		// Token: 0x0600056B RID: 1387
		[DllImport("oleaut32.dll", ExactSpelling = true)]
		public static extern int VarFormat(ref object pvarIn, HandleRef pstrFormat, int iFirstDay, int iFirstWeek, uint dwFlags, [In] [Out] ref IntPtr pbstr);

		// Token: 0x0600056C RID: 1388
		[DllImport("shell32.dll", CharSet = CharSet.Auto)]
		public static extern int DragQueryFile(HandleRef hDrop, int iFile, StringBuilder lpszFile, int cch);

		// Token: 0x0600056D RID: 1389
		[DllImport("user32.dll", ExactSpelling = true)]
		public static extern bool EnumChildWindows(HandleRef hwndParent, NativeMethods.EnumChildrenCallback lpEnumFunc, HandleRef lParam);

		// Token: 0x0600056E RID: 1390
		[DllImport("shell32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr ShellExecute(HandleRef hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

		// Token: 0x0600056F RID: 1391
		[DllImport("shell32.dll", BestFitMapping = false, CharSet = CharSet.Auto, EntryPoint = "ShellExecute")]
		public static extern IntPtr ShellExecute_NoBFM(HandleRef hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

		// Token: 0x06000570 RID: 1392
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int SetScrollPos(HandleRef hWnd, int nBar, int nPos, bool bRedraw);

		// Token: 0x06000571 RID: 1393
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool EnableScrollBar(HandleRef hWnd, int nBar, int value);

		// Token: 0x06000572 RID: 1394
		[DllImport("shell32.dll", CharSet = CharSet.Auto)]
		public static extern int Shell_NotifyIcon(int message, NativeMethods.NOTIFYICONDATA pnid);

		// Token: 0x06000573 RID: 1395
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool InsertMenuItem(HandleRef hMenu, int uItem, bool fByPosition, NativeMethods.MENUITEMINFO_T lpmii);

		// Token: 0x06000574 RID: 1396
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetMenu(HandleRef hWnd);

		// Token: 0x06000575 RID: 1397
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetMenuItemInfo(HandleRef hMenu, int uItem, bool fByPosition, [In] [Out] NativeMethods.MENUITEMINFO_T lpmii);

		// Token: 0x06000576 RID: 1398
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetMenuItemInfo(HandleRef hMenu, int uItem, bool fByPosition, [In] [Out] NativeMethods.MENUITEMINFO_T_RW lpmii);

		// Token: 0x06000577 RID: 1399
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SetMenuItemInfo(HandleRef hMenu, int uItem, bool fByPosition, NativeMethods.MENUITEMINFO_T lpmii);

		// Token: 0x06000578 RID: 1400
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateMenu", ExactSpelling = true)]
		private static extern IntPtr IntCreateMenu();

		// Token: 0x06000579 RID: 1401 RVA: 0x0000E511 File Offset: 0x0000D511
		public static IntPtr CreateMenu()
		{
			return global::System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntCreateMenu(), NativeMethods.CommonHandles.Menu);
		}

		// Token: 0x0600057A RID: 1402
		[DllImport("comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool GetOpenFileName([In] [Out] NativeMethods.OPENFILENAME_I ofn);

		// Token: 0x0600057B RID: 1403
		[DllImport("user32.dll", ExactSpelling = true)]
		public static extern bool EndDialog(HandleRef hWnd, IntPtr result);

		// Token: 0x0600057C RID: 1404
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		public static extern int MultiByteToWideChar(int CodePage, int dwFlags, byte[] lpMultiByteStr, int cchMultiByte, char[] lpWideCharStr, int cchWideChar);

		// Token: 0x0600057D RID: 1405
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern int WideCharToMultiByte(int codePage, int flags, [MarshalAs(UnmanagedType.LPWStr)] string wideStr, int chars, [In] [Out] byte[] pOutBytes, int bufferBytes, IntPtr defaultChar, IntPtr pDefaultUsed);

		// Token: 0x0600057E RID: 1406
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "RtlMoveMemory", ExactSpelling = true, SetLastError = true)]
		public static extern void CopyMemory(HandleRef destData, HandleRef srcData, int size);

		// Token: 0x0600057F RID: 1407
		[DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", ExactSpelling = true)]
		public static extern void CopyMemory(IntPtr pdst, byte[] psrc, int cb);

		// Token: 0x06000580 RID: 1408
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "RtlMoveMemory", ExactSpelling = true)]
		public static extern void CopyMemoryW(IntPtr pdst, string psrc, int cb);

		// Token: 0x06000581 RID: 1409
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "RtlMoveMemory", ExactSpelling = true)]
		public static extern void CopyMemoryW(IntPtr pdst, char[] psrc, int cb);

		// Token: 0x06000582 RID: 1410
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "RtlMoveMemory", ExactSpelling = true)]
		public static extern void CopyMemoryA(IntPtr pdst, string psrc, int cb);

		// Token: 0x06000583 RID: 1411
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "RtlMoveMemory", ExactSpelling = true)]
		public static extern void CopyMemoryA(IntPtr pdst, char[] psrc, int cb);

		// Token: 0x06000584 RID: 1412
		[DllImport("kernel32.dll", EntryPoint = "DuplicateHandle", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntDuplicateHandle(HandleRef processSource, HandleRef handleSource, HandleRef processTarget, ref IntPtr handleTarget, int desiredAccess, bool inheritHandle, int options);

		// Token: 0x06000585 RID: 1413 RVA: 0x0000E524 File Offset: 0x0000D524
		public static IntPtr DuplicateHandle(HandleRef processSource, HandleRef handleSource, HandleRef processTarget, ref IntPtr handleTarget, int desiredAccess, bool inheritHandle, int options)
		{
			IntPtr intPtr = UnsafeNativeMethods.IntDuplicateHandle(processSource, handleSource, processTarget, ref handleTarget, desiredAccess, inheritHandle, options);
			global::System.Internal.HandleCollector.Add(handleTarget, NativeMethods.CommonHandles.Kernel);
			return intPtr;
		}

		// Token: 0x06000586 RID: 1414
		[DllImport("ole32.dll", PreserveSig = false)]
		public static extern UnsafeNativeMethods.IStorage StgOpenStorageOnILockBytes(UnsafeNativeMethods.ILockBytes iLockBytes, UnsafeNativeMethods.IStorage pStgPriority, int grfMode, int sndExcluded, int reserved);

		// Token: 0x06000587 RID: 1415
		[DllImport("ole32.dll", PreserveSig = false)]
		public static extern IntPtr GetHGlobalFromILockBytes(UnsafeNativeMethods.ILockBytes pLkbyt);

		// Token: 0x06000588 RID: 1416
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SetWindowsHookEx(int hookid, NativeMethods.HookProc pfnhook, HandleRef hinst, int threadid);

		// Token: 0x06000589 RID: 1417
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetKeyboardState(byte[] keystate);

		// Token: 0x0600058A RID: 1418
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "keybd_event", ExactSpelling = true)]
		public static extern void Keybd_event(byte vk, byte scan, int flags, int extrainfo);

		// Token: 0x0600058B RID: 1419
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int SetKeyboardState(byte[] keystate);

		// Token: 0x0600058C RID: 1420
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool UnhookWindowsHookEx(HandleRef hhook);

		// Token: 0x0600058D RID: 1421
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern short GetAsyncKeyState(int vkey);

		// Token: 0x0600058E RID: 1422
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr CallNextHookEx(HandleRef hhook, int code, IntPtr wparam, IntPtr lparam);

		// Token: 0x0600058F RID: 1423
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int ScreenToClient(HandleRef hWnd, [In] [Out] NativeMethods.POINT pt);

		// Token: 0x06000590 RID: 1424
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern int GetModuleFileName(HandleRef hModule, StringBuilder buffer, int length);

		// Token: 0x06000591 RID: 1425
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern bool IsDialogMessage(HandleRef hWndDlg, [In] [Out] ref NativeMethods.MSG msg);

		// Token: 0x06000592 RID: 1426
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool TranslateMessage([In] [Out] ref NativeMethods.MSG msg);

		// Token: 0x06000593 RID: 1427
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr DispatchMessage([In] ref NativeMethods.MSG msg);

		// Token: 0x06000594 RID: 1428
		[DllImport("user32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
		public static extern IntPtr DispatchMessageA([In] ref NativeMethods.MSG msg);

		// Token: 0x06000595 RID: 1429
		[DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern IntPtr DispatchMessageW([In] ref NativeMethods.MSG msg);

		// Token: 0x06000596 RID: 1430
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int PostThreadMessage(int id, int msg, IntPtr wparam, IntPtr lparam);

		// Token: 0x06000597 RID: 1431
		[DllImport("ole32.dll", ExactSpelling = true)]
		public static extern int CoRegisterMessageFilter(HandleRef newFilter, ref IntPtr oldMsgFilter);

		// Token: 0x06000598 RID: 1432
		[DllImport("ole32.dll", EntryPoint = "OleInitialize", ExactSpelling = true, SetLastError = true)]
		private static extern int IntOleInitialize(int val);

		// Token: 0x06000599 RID: 1433 RVA: 0x0000E553 File Offset: 0x0000D553
		public static int OleInitialize()
		{
			return UnsafeNativeMethods.IntOleInitialize(0);
		}

		// Token: 0x0600059A RID: 1434
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool EnumThreadWindows(int dwThreadId, NativeMethods.EnumThreadWindowsCallback lpfn, HandleRef lParam);

		// Token: 0x0600059B RID: 1435
		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetExitCodeThread(IntPtr hThread, out uint lpExitCode);

		// Token: 0x0600059C RID: 1436
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendDlgItemMessage(HandleRef hDlg, int nIDDlgItem, int Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x0600059D RID: 1437
		[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int OleUninitialize();

		// Token: 0x0600059E RID: 1438
		[DllImport("comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool GetSaveFileName([In] [Out] NativeMethods.OPENFILENAME_I ofn);

		// Token: 0x0600059F RID: 1439
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "ChildWindowFromPointEx", ExactSpelling = true)]
		private static extern IntPtr _ChildWindowFromPointEx(HandleRef hwndParent, UnsafeNativeMethods.POINTSTRUCT pt, int uFlags);

		// Token: 0x060005A0 RID: 1440 RVA: 0x0000E55C File Offset: 0x0000D55C
		public static IntPtr ChildWindowFromPointEx(HandleRef hwndParent, int x, int y, int uFlags)
		{
			UnsafeNativeMethods.POINTSTRUCT pointstruct = new UnsafeNativeMethods.POINTSTRUCT(x, y);
			return UnsafeNativeMethods._ChildWindowFromPointEx(hwndParent, pointstruct, uFlags);
		}

		// Token: 0x060005A1 RID: 1441
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "CloseHandle", ExactSpelling = true, SetLastError = true)]
		private static extern bool IntCloseHandle(HandleRef handle);

		// Token: 0x060005A2 RID: 1442 RVA: 0x0000E57A File Offset: 0x0000D57A
		public static bool CloseHandle(HandleRef handle)
		{
			global::System.Internal.HandleCollector.Remove((IntPtr)handle, NativeMethods.CommonHandles.Kernel);
			return UnsafeNativeMethods.IntCloseHandle(handle);
		}

		// Token: 0x060005A3 RID: 1443
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateCompatibleDC", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntCreateCompatibleDC(HandleRef hDC);

		// Token: 0x060005A4 RID: 1444 RVA: 0x0000E593 File Offset: 0x0000D593
		public static IntPtr CreateCompatibleDC(HandleRef hDC)
		{
			return global::System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntCreateCompatibleDC(hDC), NativeMethods.CommonHandles.CompatibleHDC);
		}

		// Token: 0x060005A5 RID: 1445
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BlockInput([MarshalAs(UnmanagedType.Bool)] [In] bool fBlockIt);

		// Token: 0x060005A6 RID: 1446
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern uint SendInput(uint nInputs, NativeMethods.INPUT[] pInputs, int cbSize);

		// Token: 0x060005A7 RID: 1447
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "MapViewOfFile", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr IntMapViewOfFile(HandleRef hFileMapping, int dwDesiredAccess, int dwFileOffsetHigh, int dwFileOffsetLow, int dwNumberOfBytesToMap);

		// Token: 0x060005A8 RID: 1448 RVA: 0x0000E5A5 File Offset: 0x0000D5A5
		public static IntPtr MapViewOfFile(HandleRef hFileMapping, int dwDesiredAccess, int dwFileOffsetHigh, int dwFileOffsetLow, int dwNumberOfBytesToMap)
		{
			return global::System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntMapViewOfFile(hFileMapping, dwDesiredAccess, dwFileOffsetHigh, dwFileOffsetLow, dwNumberOfBytesToMap), NativeMethods.CommonHandles.Kernel);
		}

		// Token: 0x060005A9 RID: 1449
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "UnmapViewOfFile", ExactSpelling = true, SetLastError = true)]
		private static extern bool IntUnmapViewOfFile(HandleRef pvBaseAddress);

		// Token: 0x060005AA RID: 1450 RVA: 0x0000E5BC File Offset: 0x0000D5BC
		public static bool UnmapViewOfFile(HandleRef pvBaseAddress)
		{
			global::System.Internal.HandleCollector.Remove((IntPtr)pvBaseAddress, NativeMethods.CommonHandles.Kernel);
			return UnsafeNativeMethods.IntUnmapViewOfFile(pvBaseAddress);
		}

		// Token: 0x060005AB RID: 1451
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetDCEx", ExactSpelling = true)]
		private static extern IntPtr IntGetDCEx(HandleRef hWnd, HandleRef hrgnClip, int flags);

		// Token: 0x060005AC RID: 1452 RVA: 0x0000E5D5 File Offset: 0x0000D5D5
		public static IntPtr GetDCEx(HandleRef hWnd, HandleRef hrgnClip, int flags)
		{
			return global::System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntGetDCEx(hWnd, hrgnClip, flags), NativeMethods.CommonHandles.HDC);
		}

		// Token: 0x060005AD RID: 1453
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetObject(HandleRef hObject, int nSize, [In] [Out] NativeMethods.BITMAP bm);

		// Token: 0x060005AE RID: 1454
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetObject(HandleRef hObject, int nSize, [In] [Out] NativeMethods.LOGPEN lp);

		// Token: 0x060005AF RID: 1455 RVA: 0x0000E5E9 File Offset: 0x0000D5E9
		public static int GetObject(HandleRef hObject, NativeMethods.LOGPEN lp)
		{
			return UnsafeNativeMethods.GetObject(hObject, Marshal.SizeOf(typeof(NativeMethods.LOGPEN)), lp);
		}

		// Token: 0x060005B0 RID: 1456
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetObject(HandleRef hObject, int nSize, [In] [Out] NativeMethods.LOGBRUSH lb);

		// Token: 0x060005B1 RID: 1457 RVA: 0x0000E601 File Offset: 0x0000D601
		public static int GetObject(HandleRef hObject, NativeMethods.LOGBRUSH lb)
		{
			return UnsafeNativeMethods.GetObject(hObject, Marshal.SizeOf(typeof(NativeMethods.LOGBRUSH)), lb);
		}

		// Token: 0x060005B2 RID: 1458
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetObject(HandleRef hObject, int nSize, [In] [Out] NativeMethods.LOGFONT lf);

		// Token: 0x060005B3 RID: 1459 RVA: 0x0000E619 File Offset: 0x0000D619
		public static int GetObject(HandleRef hObject, NativeMethods.LOGFONT lp)
		{
			return UnsafeNativeMethods.GetObject(hObject, Marshal.SizeOf(typeof(NativeMethods.LOGFONT)), lp);
		}

		// Token: 0x060005B4 RID: 1460
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetObject(HandleRef hObject, int nSize, ref int nEntries);

		// Token: 0x060005B5 RID: 1461
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetObject(HandleRef hObject, int nSize, int[] nEntries);

		// Token: 0x060005B6 RID: 1462
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetObjectType(HandleRef hObject);

		// Token: 0x060005B7 RID: 1463
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateAcceleratorTable")]
		private static extern IntPtr IntCreateAcceleratorTable(HandleRef pentries, int cCount);

		// Token: 0x060005B8 RID: 1464 RVA: 0x0000E631 File Offset: 0x0000D631
		public static IntPtr CreateAcceleratorTable(HandleRef pentries, int cCount)
		{
			return global::System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntCreateAcceleratorTable(pentries, cCount), NativeMethods.CommonHandles.Accelerator);
		}

		// Token: 0x060005B9 RID: 1465
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "DestroyAcceleratorTable", ExactSpelling = true)]
		private static extern bool IntDestroyAcceleratorTable(HandleRef hAccel);

		// Token: 0x060005BA RID: 1466 RVA: 0x0000E644 File Offset: 0x0000D644
		public static bool DestroyAcceleratorTable(HandleRef hAccel)
		{
			global::System.Internal.HandleCollector.Remove((IntPtr)hAccel, NativeMethods.CommonHandles.Accelerator);
			return UnsafeNativeMethods.IntDestroyAcceleratorTable(hAccel);
		}

		// Token: 0x060005BB RID: 1467
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern short VkKeyScan(char key);

		// Token: 0x060005BC RID: 1468
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetCapture();

		// Token: 0x060005BD RID: 1469
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr SetCapture(HandleRef hwnd);

		// Token: 0x060005BE RID: 1470
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetFocus();

		// Token: 0x060005BF RID: 1471
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetCursorPos([In] [Out] NativeMethods.POINT pt);

		// Token: 0x060005C0 RID: 1472
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern short GetKeyState(int keyCode);

		// Token: 0x060005C1 RID: 1473
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern uint GetShortPathName(string lpszLongPath, StringBuilder lpszShortPath, uint cchBuffer);

		// Token: 0x060005C2 RID: 1474
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowRgn", ExactSpelling = true)]
		private static extern int IntSetWindowRgn(HandleRef hwnd, HandleRef hrgn, bool fRedraw);

		// Token: 0x060005C3 RID: 1475 RVA: 0x0000E660 File Offset: 0x0000D660
		public static int SetWindowRgn(HandleRef hwnd, HandleRef hrgn, bool fRedraw)
		{
			int num = UnsafeNativeMethods.IntSetWindowRgn(hwnd, hrgn, fRedraw);
			if (num != 0)
			{
				global::System.Internal.HandleCollector.Remove((IntPtr)hrgn, NativeMethods.CommonHandles.GDI);
			}
			return num;
		}

		// Token: 0x060005C4 RID: 1476
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetWindowText(HandleRef hWnd, StringBuilder lpString, int nMaxCount);

		// Token: 0x060005C5 RID: 1477
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern void GetTempFileName(string tempDirName, string prefixName, int unique, StringBuilder sb);

		// Token: 0x060005C6 RID: 1478
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SetWindowText(HandleRef hWnd, string text);

		// Token: 0x060005C7 RID: 1479
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GlobalAlloc(int uFlags, int dwBytes);

		// Token: 0x060005C8 RID: 1480
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GlobalReAlloc(HandleRef handle, int bytes, int flags);

		// Token: 0x060005C9 RID: 1481
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GlobalLock(HandleRef handle);

		// Token: 0x060005CA RID: 1482
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GlobalUnlock(HandleRef handle);

		// Token: 0x060005CB RID: 1483
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GlobalFree(HandleRef handle);

		// Token: 0x060005CC RID: 1484
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GlobalSize(HandleRef handle);

		// Token: 0x060005CD RID: 1485
		[DllImport("imm32.dll", CharSet = CharSet.Auto)]
		public static extern bool ImmSetConversionStatus(HandleRef hIMC, int conversion, int sentence);

		// Token: 0x060005CE RID: 1486
		[DllImport("imm32.dll", CharSet = CharSet.Auto)]
		public static extern bool ImmGetConversionStatus(HandleRef hIMC, ref int conversion, ref int sentence);

		// Token: 0x060005CF RID: 1487
		[DllImport("imm32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr ImmGetContext(HandleRef hWnd);

		// Token: 0x060005D0 RID: 1488
		[DllImport("imm32.dll", CharSet = CharSet.Auto)]
		public static extern bool ImmReleaseContext(HandleRef hWnd, HandleRef hIMC);

		// Token: 0x060005D1 RID: 1489
		[DllImport("imm32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr ImmAssociateContext(HandleRef hWnd, HandleRef hIMC);

		// Token: 0x060005D2 RID: 1490
		[DllImport("imm32.dll", CharSet = CharSet.Auto)]
		public static extern bool ImmDestroyContext(HandleRef hIMC);

		// Token: 0x060005D3 RID: 1491
		[DllImport("imm32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr ImmCreateContext();

		// Token: 0x060005D4 RID: 1492
		[DllImport("imm32.dll", CharSet = CharSet.Auto)]
		public static extern bool ImmSetOpenStatus(HandleRef hIMC, bool open);

		// Token: 0x060005D5 RID: 1493
		[DllImport("imm32.dll", CharSet = CharSet.Auto)]
		public static extern bool ImmGetOpenStatus(HandleRef hIMC);

		// Token: 0x060005D6 RID: 1494
		[DllImport("imm32.dll", CharSet = CharSet.Auto)]
		public static extern bool ImmNotifyIME(HandleRef hIMC, int dwAction, int dwIndex, int dwValue);

		// Token: 0x060005D7 RID: 1495
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr SetFocus(HandleRef hWnd);

		// Token: 0x060005D8 RID: 1496
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetParent(HandleRef hWnd);

		// Token: 0x060005D9 RID: 1497
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetAncestor(HandleRef hWnd, int flags);

		// Token: 0x060005DA RID: 1498
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool IsChild(HandleRef hWndParent, HandleRef hwnd);

		// Token: 0x060005DB RID: 1499
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool IsZoomed(HandleRef hWnd);

		// Token: 0x060005DC RID: 1500
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr FindWindow(string className, string windowName);

		// Token: 0x060005DD RID: 1501
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int MapWindowPoints(HandleRef hWndFrom, HandleRef hWndTo, [In] [Out] ref NativeMethods.RECT rect, int cPoints);

		// Token: 0x060005DE RID: 1502
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int MapWindowPoints(HandleRef hWndFrom, HandleRef hWndTo, [In] [Out] NativeMethods.POINT pt, int cPoints);

		// Token: 0x060005DF RID: 1503
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, bool wParam, int lParam);

		// Token: 0x060005E0 RID: 1504
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, int[] lParam);

		// Token: 0x060005E1 RID: 1505
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int[] wParam, int[] lParam);

		// Token: 0x060005E2 RID: 1506
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, ref int wParam, ref int lParam);

		// Token: 0x060005E3 RID: 1507
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, string lParam);

		// Token: 0x060005E4 RID: 1508
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, IntPtr wParam, string lParam);

		// Token: 0x060005E5 RID: 1509
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, StringBuilder lParam);

		// Token: 0x060005E6 RID: 1510
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.TOOLINFO_T lParam);

		// Token: 0x060005E7 RID: 1511
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.TOOLINFO_TOOLTIP lParam);

		// Token: 0x060005E8 RID: 1512
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, ref NativeMethods.TBBUTTON lParam);

		// Token: 0x060005E9 RID: 1513
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, ref NativeMethods.TBBUTTONINFO lParam);

		// Token: 0x060005EA RID: 1514
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, ref NativeMethods.TV_ITEM lParam);

		// Token: 0x060005EB RID: 1515
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, ref NativeMethods.TV_INSERTSTRUCT lParam);

		// Token: 0x060005EC RID: 1516
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.TV_HITTESTINFO lParam);

		// Token: 0x060005ED RID: 1517
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.LVBKIMAGE lParam);

		// Token: 0x060005EE RID: 1518
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int SendMessage(HandleRef hWnd, int msg, int wParam, ref NativeMethods.LVHITTESTINFO lParam);

		// Token: 0x060005EF RID: 1519
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.TCITEM_T lParam);

		// Token: 0x060005F0 RID: 1520
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, ref NativeMethods.HDLAYOUT hdlayout);

		// Token: 0x060005F1 RID: 1521
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, HandleRef wParam, int lParam);

		// Token: 0x060005F2 RID: 1522
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, HandleRef lParam);

		// Token: 0x060005F3 RID: 1523
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPStruct)] [In] [Out] NativeMethods.PARAFORMAT lParam);

		// Token: 0x060005F4 RID: 1524
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPStruct)] [In] [Out] NativeMethods.CHARFORMATA lParam);

		// Token: 0x060005F5 RID: 1525
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPStruct)] [In] [Out] NativeMethods.CHARFORMAT2A lParam);

		// Token: 0x060005F6 RID: 1526
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPStruct)] [In] [Out] NativeMethods.CHARFORMATW lParam);

		// Token: 0x060005F7 RID: 1527
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int SendMessage(HandleRef hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.IUnknown)] out object editOle);

		// Token: 0x060005F8 RID: 1528
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.CHARRANGE lParam);

		// Token: 0x060005F9 RID: 1529
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.FINDTEXT lParam);

		// Token: 0x060005FA RID: 1530
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.TEXTRANGE lParam);

		// Token: 0x060005FB RID: 1531
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.POINT lParam);

		// Token: 0x060005FC RID: 1532
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, NativeMethods.POINT wParam, int lParam);

		// Token: 0x060005FD RID: 1533
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.REPASTESPECIAL lParam);

		// Token: 0x060005FE RID: 1534
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.EDITSTREAM lParam);

		// Token: 0x060005FF RID: 1535
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.EDITSTREAM64 lParam);

		// Token: 0x06000600 RID: 1536
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, NativeMethods.GETTEXTLENGTHEX wParam, int lParam);

		// Token: 0x06000601 RID: 1537
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, [In] [Out] NativeMethods.SIZE lParam);

		// Token: 0x06000602 RID: 1538
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, [In] [Out] ref NativeMethods.LVFINDINFO lParam);

		// Token: 0x06000603 RID: 1539
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.LVHITTESTINFO lParam);

		// Token: 0x06000604 RID: 1540
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.LVCOLUMN_T lParam);

		// Token: 0x06000605 RID: 1541
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, [In] [Out] ref NativeMethods.LVITEM lParam);

		// Token: 0x06000606 RID: 1542
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.LVCOLUMN lParam);

		// Token: 0x06000607 RID: 1543
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.LVGROUP lParam);

		// Token: 0x06000608 RID: 1544
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, NativeMethods.POINT wParam, [In] [Out] NativeMethods.LVINSERTMARK lParam);

		// Token: 0x06000609 RID: 1545
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.LVINSERTMARK lParam);

		// Token: 0x0600060A RID: 1546
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SendMessage(HandleRef hWnd, int msg, int wParam, [In] [Out] NativeMethods.LVTILEVIEWINFO lParam);

		// Token: 0x0600060B RID: 1547
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.MCHITTESTINFO lParam);

		// Token: 0x0600060C RID: 1548
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.SYSTEMTIME lParam);

		// Token: 0x0600060D RID: 1549
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.SYSTEMTIMEARRAY lParam);

		// Token: 0x0600060E RID: 1550
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, [In] [Out] NativeMethods.LOGFONT lParam);

		// Token: 0x0600060F RID: 1551
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, NativeMethods.MSG lParam);

		// Token: 0x06000610 RID: 1552
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, int lParam);

		// Token: 0x06000611 RID: 1553
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000612 RID: 1554
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int Msg, IntPtr wParam, [In] [Out] ref NativeMethods.RECT lParam);

		// Token: 0x06000613 RID: 1555
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int Msg, ref short wParam, ref short lParam);

		// Token: 0x06000614 RID: 1556
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int Msg, [MarshalAs(UnmanagedType.Bool)] [In] [Out] ref bool wParam, IntPtr lParam);

		// Token: 0x06000615 RID: 1557
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int Msg, int wParam, IntPtr lParam);

		// Token: 0x06000616 RID: 1558
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int Msg, int wParam, [In] [Out] ref NativeMethods.RECT lParam);

		// Token: 0x06000617 RID: 1559
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int Msg, int wParam, [In] [Out] ref Rectangle lParam);

		// Token: 0x06000618 RID: 1560
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int Msg, IntPtr wParam, NativeMethods.ListViewCompareCallback pfnCompare);

		// Token: 0x06000619 RID: 1561
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessageTimeout(HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam, int flags, int timeout, out IntPtr pdwResult);

		// Token: 0x0600061A RID: 1562
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr SetParent(HandleRef hWnd, HandleRef hWndParent);

		// Token: 0x0600061B RID: 1563
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetWindowRect(HandleRef hWnd, [In] [Out] ref NativeMethods.RECT rect);

		// Token: 0x0600061C RID: 1564
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetWindow(HandleRef hWnd, int uCmd);

		// Token: 0x0600061D RID: 1565
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetDlgItem(HandleRef hWnd, int nIDDlgItem);

		// Token: 0x0600061E RID: 1566
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetModuleHandle(string modName);

		// Token: 0x0600061F RID: 1567
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr DefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000620 RID: 1568
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr DefMDIChildProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000621 RID: 1569
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr CallWindowProc(IntPtr wndProc, IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000622 RID: 1570
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern short GlobalDeleteAtom(short atom);

		// Token: 0x06000623 RID: 1571
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
		public static extern IntPtr GetProcAddress(HandleRef hModule, string lpProcName);

		// Token: 0x06000624 RID: 1572
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetClassInfo(HandleRef hInst, string lpszClass, [In] [Out] NativeMethods.WNDCLASS_I wc);

		// Token: 0x06000625 RID: 1573
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetClassInfo(HandleRef hInst, string lpszClass, IntPtr h);

		// Token: 0x06000626 RID: 1574
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetSystemMetrics(int nIndex);

		// Token: 0x06000627 RID: 1575
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SystemParametersInfo(int nAction, int nParam, ref NativeMethods.RECT rc, int nUpdate);

		// Token: 0x06000628 RID: 1576
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SystemParametersInfo(int nAction, int nParam, ref int value, int ignore);

		// Token: 0x06000629 RID: 1577
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SystemParametersInfo(int nAction, int nParam, ref bool value, int ignore);

		// Token: 0x0600062A RID: 1578
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SystemParametersInfo(int nAction, int nParam, ref NativeMethods.HIGHCONTRAST_I rc, int nUpdate);

		// Token: 0x0600062B RID: 1579
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SystemParametersInfo(int nAction, int nParam, [In] [Out] NativeMethods.NONCLIENTMETRICS metrics, int nUpdate);

		// Token: 0x0600062C RID: 1580
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SystemParametersInfo(int nAction, int nParam, [In] [Out] NativeMethods.LOGFONT font, int nUpdate);

		// Token: 0x0600062D RID: 1581
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SystemParametersInfo(int nAction, int nParam, bool[] flag, bool nUpdate);

		// Token: 0x0600062E RID: 1582
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetComputerName(StringBuilder lpBuffer, int[] nSize);

		// Token: 0x0600062F RID: 1583
		[DllImport("advapi32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetUserName(StringBuilder lpBuffer, int[] nSize);

		// Token: 0x06000630 RID: 1584
		[DllImport("user32.dll", ExactSpelling = true)]
		public static extern IntPtr GetProcessWindowStation();

		// Token: 0x06000631 RID: 1585
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool GetUserObjectInformation(HandleRef hObj, int nIndex, [MarshalAs(UnmanagedType.LPStruct)] NativeMethods.USEROBJECTFLAGS pvBuffer, int nLength, ref int lpnLengthNeeded);

		// Token: 0x06000632 RID: 1586
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int ClientToScreen(HandleRef hWnd, [In] [Out] NativeMethods.POINT pt);

		// Token: 0x06000633 RID: 1587
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetForegroundWindow();

		// Token: 0x06000634 RID: 1588
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int MsgWaitForMultipleObjectsEx(int nCount, IntPtr pHandles, int dwMilliseconds, int dwWakeMask, int dwFlags);

		// Token: 0x06000635 RID: 1589
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetDesktopWindow();

		// Token: 0x06000636 RID: 1590
		[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int RegisterDragDrop(HandleRef hwnd, UnsafeNativeMethods.IOleDropTarget target);

		// Token: 0x06000637 RID: 1591
		[DllImport("ole32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int RevokeDragDrop(HandleRef hwnd);

		// Token: 0x06000638 RID: 1592
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool PeekMessage([In] [Out] ref NativeMethods.MSG msg, HandleRef hwnd, int msgMin, int msgMax, int remove);

		// Token: 0x06000639 RID: 1593
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern bool PeekMessageW([In] [Out] ref NativeMethods.MSG msg, HandleRef hwnd, int msgMin, int msgMax, int remove);

		// Token: 0x0600063A RID: 1594
		[DllImport("user32.dll", CharSet = CharSet.Ansi)]
		public static extern bool PeekMessageA([In] [Out] ref NativeMethods.MSG msg, HandleRef hwnd, int msgMin, int msgMax, int remove);

		// Token: 0x0600063B RID: 1595
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool PostMessage(HandleRef hwnd, int msg, IntPtr wparam, IntPtr lparam);

		// Token: 0x0600063C RID: 1596
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern short GlobalAddAtom(string atomName);

		// Token: 0x0600063D RID: 1597
		[DllImport("oleacc.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr LresultFromObject(ref Guid refiid, IntPtr wParam, HandleRef pAcc);

		// Token: 0x0600063E RID: 1598
		[DllImport("oleacc.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int CreateStdAccessibleObject(HandleRef hWnd, int objID, ref Guid refiid, [MarshalAs(UnmanagedType.Interface)] [In] [Out] ref object pAcc);

		// Token: 0x0600063F RID: 1599
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern void NotifyWinEvent(int winEvent, HandleRef hwnd, int objType, int objID);

		// Token: 0x06000640 RID: 1600
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetMenuItemID(HandleRef hMenu, int nPos);

		// Token: 0x06000641 RID: 1601
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetSubMenu(HandleRef hwnd, int index);

		// Token: 0x06000642 RID: 1602
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetMenuItemCount(HandleRef hMenu);

		// Token: 0x06000643 RID: 1603
		[DllImport("oleaut32.dll", PreserveSig = false)]
		public static extern void GetErrorInfo(int reserved, [In] [Out] ref UnsafeNativeMethods.IErrorInfo errorInfo);

		// Token: 0x06000644 RID: 1604
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "BeginPaint", ExactSpelling = true)]
		private static extern IntPtr IntBeginPaint(HandleRef hWnd, [In] [Out] ref NativeMethods.PAINTSTRUCT lpPaint);

		// Token: 0x06000645 RID: 1605 RVA: 0x0000E68B File Offset: 0x0000D68B
		public static IntPtr BeginPaint(HandleRef hWnd, [MarshalAs(UnmanagedType.LPStruct)] [In] [Out] ref NativeMethods.PAINTSTRUCT lpPaint)
		{
			return global::System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntBeginPaint(hWnd, ref lpPaint), NativeMethods.CommonHandles.HDC);
		}

		// Token: 0x06000646 RID: 1606
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "EndPaint", ExactSpelling = true)]
		private static extern bool IntEndPaint(HandleRef hWnd, ref NativeMethods.PAINTSTRUCT lpPaint);

		// Token: 0x06000647 RID: 1607 RVA: 0x0000E69E File Offset: 0x0000D69E
		public static bool EndPaint(HandleRef hWnd, [MarshalAs(UnmanagedType.LPStruct)] [In] ref NativeMethods.PAINTSTRUCT lpPaint)
		{
			global::System.Internal.HandleCollector.Remove(lpPaint.hdc, NativeMethods.CommonHandles.HDC);
			return UnsafeNativeMethods.IntEndPaint(hWnd, ref lpPaint);
		}

		// Token: 0x06000648 RID: 1608
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetDC", ExactSpelling = true)]
		private static extern IntPtr IntGetDC(HandleRef hWnd);

		// Token: 0x06000649 RID: 1609 RVA: 0x0000E6B8 File Offset: 0x0000D6B8
		public static IntPtr GetDC(HandleRef hWnd)
		{
			return global::System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntGetDC(hWnd), NativeMethods.CommonHandles.HDC);
		}

		// Token: 0x0600064A RID: 1610
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowDC", ExactSpelling = true)]
		private static extern IntPtr IntGetWindowDC(HandleRef hWnd);

		// Token: 0x0600064B RID: 1611 RVA: 0x0000E6CA File Offset: 0x0000D6CA
		public static IntPtr GetWindowDC(HandleRef hWnd)
		{
			return global::System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntGetWindowDC(hWnd), NativeMethods.CommonHandles.HDC);
		}

		// Token: 0x0600064C RID: 1612
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "ReleaseDC", ExactSpelling = true)]
		private static extern int IntReleaseDC(HandleRef hWnd, HandleRef hDC);

		// Token: 0x0600064D RID: 1613 RVA: 0x0000E6DC File Offset: 0x0000D6DC
		public static int ReleaseDC(HandleRef hWnd, HandleRef hDC)
		{
			global::System.Internal.HandleCollector.Remove((IntPtr)hDC, NativeMethods.CommonHandles.HDC);
			return UnsafeNativeMethods.IntReleaseDC(hWnd, hDC);
		}

		// Token: 0x0600064E RID: 1614
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateDC", SetLastError = true)]
		private static extern IntPtr IntCreateDC(string lpszDriver, string lpszDeviceName, string lpszOutput, HandleRef devMode);

		// Token: 0x0600064F RID: 1615 RVA: 0x0000E6F6 File Offset: 0x0000D6F6
		public static IntPtr CreateDC(string lpszDriver)
		{
			return global::System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntCreateDC(lpszDriver, null, null, NativeMethods.NullHandleRef), NativeMethods.CommonHandles.HDC);
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0000E70F File Offset: 0x0000D70F
		public static IntPtr CreateDC(string lpszDriverName, string lpszDeviceName, string lpszOutput, HandleRef lpInitData)
		{
			return global::System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntCreateDC(lpszDriverName, lpszDeviceName, lpszOutput, lpInitData), NativeMethods.CommonHandles.HDC);
		}

		// Token: 0x06000651 RID: 1617
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SystemParametersInfo(int nAction, int nParam, [In] [Out] IntPtr[] rc, int nUpdate);

		// Token: 0x06000652 RID: 1618
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessage")]
		public static extern IntPtr SendCallbackMessage(HandleRef hWnd, int Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000653 RID: 1619
		[DllImport("shell32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
		public static extern void DragAcceptFiles(HandleRef hWnd, bool fAccept);

		// Token: 0x06000654 RID: 1620
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetDeviceCaps(HandleRef hDC, int nIndex);

		// Token: 0x06000655 RID: 1621
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetScrollInfo(HandleRef hWnd, int fnBar, NativeMethods.SCROLLINFO si);

		// Token: 0x06000656 RID: 1622
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int SetScrollInfo(HandleRef hWnd, int fnBar, NativeMethods.SCROLLINFO si, bool redraw);

		// Token: 0x06000657 RID: 1623
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetActiveWindow();

		// Token: 0x06000658 RID: 1624
		[DllImport("mscoree.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern int LoadLibraryShim(string dllName, string version, IntPtr reserved, out IntPtr dllModule);

		// Token: 0x06000659 RID: 1625
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr LoadLibrary(string libname);

		// Token: 0x0600065A RID: 1626
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern bool FreeLibrary(HandleRef hModule);

		// Token: 0x0600065B RID: 1627 RVA: 0x0000E724 File Offset: 0x0000D724
		public static IntPtr GetWindowLong(HandleRef hWnd, int nIndex)
		{
			if (IntPtr.Size == 4)
			{
				return UnsafeNativeMethods.GetWindowLong32(hWnd, nIndex);
			}
			return UnsafeNativeMethods.GetWindowLongPtr64(hWnd, nIndex);
		}

		// Token: 0x0600065C RID: 1628
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLong")]
		public static extern IntPtr GetWindowLong32(HandleRef hWnd, int nIndex);

		// Token: 0x0600065D RID: 1629
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLongPtr")]
		public static extern IntPtr GetWindowLongPtr64(HandleRef hWnd, int nIndex);

		// Token: 0x0600065E RID: 1630 RVA: 0x0000E73D File Offset: 0x0000D73D
		public static IntPtr SetWindowLong(HandleRef hWnd, int nIndex, HandleRef dwNewLong)
		{
			if (IntPtr.Size == 4)
			{
				return UnsafeNativeMethods.SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
			}
			return UnsafeNativeMethods.SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
		}

		// Token: 0x0600065F RID: 1631
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLong")]
		public static extern IntPtr SetWindowLongPtr32(HandleRef hWnd, int nIndex, HandleRef dwNewLong);

		// Token: 0x06000660 RID: 1632
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtr")]
		public static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, HandleRef dwNewLong);

		// Token: 0x06000661 RID: 1633 RVA: 0x0000E758 File Offset: 0x0000D758
		public static IntPtr SetWindowLong(HandleRef hWnd, int nIndex, NativeMethods.WndProc wndproc)
		{
			if (IntPtr.Size == 4)
			{
				return UnsafeNativeMethods.SetWindowLongPtr32(hWnd, nIndex, wndproc);
			}
			return UnsafeNativeMethods.SetWindowLongPtr64(hWnd, nIndex, wndproc);
		}

		// Token: 0x06000662 RID: 1634
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLong")]
		public static extern IntPtr SetWindowLongPtr32(HandleRef hWnd, int nIndex, NativeMethods.WndProc wndproc);

		// Token: 0x06000663 RID: 1635
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtr")]
		public static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, NativeMethods.WndProc wndproc);

		// Token: 0x06000664 RID: 1636
		[DllImport("ole32.dll", PreserveSig = false)]
		public static extern UnsafeNativeMethods.ILockBytes CreateILockBytesOnHGlobal(HandleRef hGlobal, bool fDeleteOnRelease);

		// Token: 0x06000665 RID: 1637
		[DllImport("ole32.dll", PreserveSig = false)]
		public static extern UnsafeNativeMethods.IStorage StgCreateDocfileOnILockBytes(UnsafeNativeMethods.ILockBytes iLockBytes, int grfMode, int reserved);

		// Token: 0x06000666 RID: 1638
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "CreatePopupMenu", ExactSpelling = true)]
		private static extern IntPtr IntCreatePopupMenu();

		// Token: 0x06000667 RID: 1639 RVA: 0x0000E773 File Offset: 0x0000D773
		public static IntPtr CreatePopupMenu()
		{
			return global::System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntCreatePopupMenu(), NativeMethods.CommonHandles.Menu);
		}

		// Token: 0x06000668 RID: 1640
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool RemoveMenu(HandleRef hMenu, int uPosition, int uFlags);

		// Token: 0x06000669 RID: 1641
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "DestroyMenu", ExactSpelling = true)]
		private static extern bool IntDestroyMenu(HandleRef hMenu);

		// Token: 0x0600066A RID: 1642 RVA: 0x0000E784 File Offset: 0x0000D784
		public static bool DestroyMenu(HandleRef hMenu)
		{
			global::System.Internal.HandleCollector.Remove((IntPtr)hMenu, NativeMethods.CommonHandles.Menu);
			return UnsafeNativeMethods.IntDestroyMenu(hMenu);
		}

		// Token: 0x0600066B RID: 1643
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool SetForegroundWindow(HandleRef hWnd);

		// Token: 0x0600066C RID: 1644
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetSystemMenu(HandleRef hWnd, bool bRevert);

		// Token: 0x0600066D RID: 1645
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr DefFrameProc(IntPtr hWnd, IntPtr hWndClient, int msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x0600066E RID: 1646
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool TranslateMDISysAccel(IntPtr hWndClient, [In] [Out] ref NativeMethods.MSG msg);

		// Token: 0x0600066F RID: 1647
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool SetLayeredWindowAttributes(HandleRef hwnd, int crKey, byte bAlpha, int dwFlags);

		// Token: 0x06000670 RID: 1648
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool SetMenu(HandleRef hWnd, HandleRef hMenu);

		// Token: 0x06000671 RID: 1649
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int GetWindowPlacement(HandleRef hWnd, ref NativeMethods.WINDOWPLACEMENT placement);

		// Token: 0x06000672 RID: 1650
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern void GetStartupInfo([In] [Out] NativeMethods.STARTUPINFO_I startupinfo_i);

		// Token: 0x06000673 RID: 1651
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool SetMenuDefaultItem(HandleRef hwnd, int nIndex, bool pos);

		// Token: 0x06000674 RID: 1652
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool EnableMenuItem(HandleRef hMenu, int UIDEnabledItem, int uEnable);

		// Token: 0x06000675 RID: 1653
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr SetActiveWindow(HandleRef hWnd);

		// Token: 0x06000676 RID: 1654
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateIC", SetLastError = true)]
		private static extern IntPtr IntCreateIC(string lpszDriverName, string lpszDeviceName, string lpszOutput, HandleRef lpInitData);

		// Token: 0x06000677 RID: 1655 RVA: 0x0000E79D File Offset: 0x0000D79D
		public static IntPtr CreateIC(string lpszDriverName, string lpszDeviceName, string lpszOutput, HandleRef lpInitData)
		{
			return global::System.Internal.HandleCollector.Add(UnsafeNativeMethods.IntCreateIC(lpszDriverName, lpszDeviceName, lpszOutput, lpInitData), NativeMethods.CommonHandles.HDC);
		}

		// Token: 0x06000678 RID: 1656
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool ClipCursor(ref NativeMethods.RECT rcClip);

		// Token: 0x06000679 RID: 1657
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool ClipCursor(NativeMethods.COMRECT rcClip);

		// Token: 0x0600067A RID: 1658
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr SetCursor(HandleRef hcursor);

		// Token: 0x0600067B RID: 1659
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool SetCursorPos(int x, int y);

		// Token: 0x0600067C RID: 1660
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int ShowCursor(bool bShow);

		// Token: 0x0600067D RID: 1661
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "DestroyCursor", ExactSpelling = true)]
		private static extern bool IntDestroyCursor(HandleRef hCurs);

		// Token: 0x0600067E RID: 1662 RVA: 0x0000E7B2 File Offset: 0x0000D7B2
		public static bool DestroyCursor(HandleRef hCurs)
		{
			global::System.Internal.HandleCollector.Remove((IntPtr)hCurs, NativeMethods.CommonHandles.Cursor);
			return UnsafeNativeMethods.IntDestroyCursor(hCurs);
		}

		// Token: 0x0600067F RID: 1663
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool IsWindow(HandleRef hWnd);

		// Token: 0x06000680 RID: 1664
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, EntryPoint = "DeleteDC", ExactSpelling = true, SetLastError = true)]
		private static extern bool IntDeleteDC(HandleRef hDC);

		// Token: 0x06000681 RID: 1665 RVA: 0x0000E7CB File Offset: 0x0000D7CB
		public static bool DeleteDC(HandleRef hDC)
		{
			global::System.Internal.HandleCollector.Remove((IntPtr)hDC, NativeMethods.CommonHandles.HDC);
			return UnsafeNativeMethods.IntDeleteDC(hDC);
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0000E7E4 File Offset: 0x0000D7E4
		public static bool DeleteCompatibleDC(HandleRef hDC)
		{
			global::System.Internal.HandleCollector.Remove((IntPtr)hDC, NativeMethods.CommonHandles.CompatibleHDC);
			return UnsafeNativeMethods.IntDeleteDC(hDC);
		}

		// Token: 0x06000683 RID: 1667
		[DllImport("user32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
		public static extern bool GetMessageA([In] [Out] ref NativeMethods.MSG msg, HandleRef hWnd, int uMsgFilterMin, int uMsgFilterMax);

		// Token: 0x06000684 RID: 1668
		[DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern bool GetMessageW([In] [Out] ref NativeMethods.MSG msg, HandleRef hWnd, int uMsgFilterMin, int uMsgFilterMax);

		// Token: 0x06000685 RID: 1669
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr PostMessage(HandleRef hwnd, int msg, int wparam, int lparam);

		// Token: 0x06000686 RID: 1670
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr PostMessage(HandleRef hwnd, int msg, int wparam, IntPtr lparam);

		// Token: 0x06000687 RID: 1671
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetClientRect(HandleRef hWnd, [In] [Out] ref NativeMethods.RECT rect);

		// Token: 0x06000688 RID: 1672
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetClientRect(HandleRef hWnd, IntPtr rect);

		// Token: 0x06000689 RID: 1673
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "WindowFromPoint", ExactSpelling = true)]
		private static extern IntPtr _WindowFromPoint(UnsafeNativeMethods.POINTSTRUCT pt);

		// Token: 0x0600068A RID: 1674 RVA: 0x0000E800 File Offset: 0x0000D800
		public static IntPtr WindowFromPoint(int x, int y)
		{
			UnsafeNativeMethods.POINTSTRUCT pointstruct = new UnsafeNativeMethods.POINTSTRUCT(x, y);
			return UnsafeNativeMethods._WindowFromPoint(pointstruct);
		}

		// Token: 0x0600068B RID: 1675
		[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr WindowFromDC(HandleRef hDC);

		// Token: 0x0600068C RID: 1676
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "CreateWindowEx", SetLastError = true)]
		public static extern IntPtr IntCreateWindowEx(int dwExStyle, string lpszClassName, string lpszWindowName, int style, int x, int y, int width, int height, HandleRef hWndParent, HandleRef hMenu, HandleRef hInst, [MarshalAs(UnmanagedType.AsAny)] object pvParam);

		// Token: 0x0600068D RID: 1677 RVA: 0x0000E81C File Offset: 0x0000D81C
		public static IntPtr CreateWindowEx(int dwExStyle, string lpszClassName, string lpszWindowName, int style, int x, int y, int width, int height, HandleRef hWndParent, HandleRef hMenu, HandleRef hInst, [MarshalAs(UnmanagedType.AsAny)] object pvParam)
		{
			return UnsafeNativeMethods.IntCreateWindowEx(dwExStyle, lpszClassName, lpszWindowName, style, x, y, width, height, hWndParent, hMenu, hInst, pvParam);
		}

		// Token: 0x0600068E RID: 1678
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "DestroyWindow", ExactSpelling = true)]
		public static extern bool IntDestroyWindow(HandleRef hWnd);

		// Token: 0x0600068F RID: 1679 RVA: 0x0000E842 File Offset: 0x0000D842
		public static bool DestroyWindow(HandleRef hWnd)
		{
			return UnsafeNativeMethods.IntDestroyWindow(hWnd);
		}

		// Token: 0x06000690 RID: 1680
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool UnregisterClass(string className, HandleRef hInstance);

		// Token: 0x06000691 RID: 1681
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GetStockObject(int nIndex);

		// Token: 0x06000692 RID: 1682
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern short RegisterClass(NativeMethods.WNDCLASS_D wc);

		// Token: 0x06000693 RID: 1683
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern void PostQuitMessage(int nExitCode);

		// Token: 0x06000694 RID: 1684
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern void WaitMessage();

		// Token: 0x06000695 RID: 1685
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool SetWindowPlacement(HandleRef hWnd, [In] ref NativeMethods.WINDOWPLACEMENT placement);

		// Token: 0x06000696 RID: 1686
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool GetSystemPowerStatus([In] [Out] ref NativeMethods.SYSTEM_POWER_STATUS systemPowerStatus);

		// Token: 0x06000697 RID: 1687
		[DllImport("Powrprof.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);

		// Token: 0x06000698 RID: 1688
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern int GetRegionData(HandleRef hRgn, int size, IntPtr lpRgnData);

		// Token: 0x06000699 RID: 1689 RVA: 0x0000E84C File Offset: 0x0000D84C
		public unsafe static NativeMethods.RECT[] GetRectsFromRegion(IntPtr hRgn)
		{
			NativeMethods.RECT[] array = null;
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				int regionData = UnsafeNativeMethods.GetRegionData(new HandleRef(null, hRgn), 0, IntPtr.Zero);
				if (regionData != 0)
				{
					intPtr = Marshal.AllocCoTaskMem(regionData);
					int regionData2 = UnsafeNativeMethods.GetRegionData(new HandleRef(null, hRgn), regionData, intPtr);
					if (regionData2 == regionData)
					{
						NativeMethods.RGNDATAHEADER* ptr = (NativeMethods.RGNDATAHEADER*)(void*)intPtr;
						if (ptr->iType == 1)
						{
							array = new NativeMethods.RECT[ptr->nCount];
							int cbSizeOfStruct = ptr->cbSizeOfStruct;
							for (int i = 0; i < ptr->nCount; i++)
							{
								array[i] = *(NativeMethods.RECT*)((byte*)((byte*)(void*)intPtr + cbSizeOfStruct) + (IntPtr)Marshal.SizeOf(typeof(NativeMethods.RECT)) * (IntPtr)i);
							}
						}
					}
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(intPtr);
				}
			}
			return array;
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0000E928 File Offset: 0x0000D928
		internal static bool IsComObject(object o)
		{
			return Marshal.IsComObject(o);
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0000E930 File Offset: 0x0000D930
		internal static int ReleaseComObject(object objToRelease)
		{
			return Marshal.ReleaseComObject(objToRelease);
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0000E938 File Offset: 0x0000D938
		[ReflectionPermission(SecurityAction.Assert, Unrestricted = true)]
		public static object PtrToStructure(IntPtr lparam, Type cls)
		{
			return Marshal.PtrToStructure(lparam, cls);
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0000E941 File Offset: 0x0000D941
		[ReflectionPermission(SecurityAction.Assert, Unrestricted = true)]
		public static void PtrToStructure(IntPtr lparam, object data)
		{
			Marshal.PtrToStructure(lparam, data);
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0000E94A File Offset: 0x0000D94A
		internal static int SizeOf(Type t)
		{
			return Marshal.SizeOf(t);
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0000E952 File Offset: 0x0000D952
		internal static void ThrowExceptionForHR(int errorCode)
		{
			Marshal.ThrowExceptionForHR(errorCode);
		}

		// Token: 0x060006A0 RID: 1696
		[DllImport("mscorwks.dll", BestFitMapping = false, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
		internal static extern void CorLaunchApplication(uint hostType, string applicationFullName, int manifestPathsCount, string[] manifestPaths, int activationDataCount, string[] activationData, UnsafeNativeMethods.PROCESS_INFORMATION processInformation);

		// Token: 0x04000F5F RID: 3935
		public const int MB_PRECOMPOSED = 1;

		// Token: 0x04000F60 RID: 3936
		public const int SMTO_ABORTIFHUNG = 2;

		// Token: 0x04000F61 RID: 3937
		public const int LAYOUT_RTL = 1;

		// Token: 0x04000F62 RID: 3938
		public const int LAYOUT_BITMAPORIENTATIONPRESERVED = 8;

		// Token: 0x04000F63 RID: 3939
		private static readonly Version VistaOSVersion = new Version(6, 0);

		// Token: 0x02000150 RID: 336
		private struct POINTSTRUCT
		{
			// Token: 0x060006A2 RID: 1698 RVA: 0x0000E968 File Offset: 0x0000D968
			public POINTSTRUCT(int x, int y)
			{
				this.x = x;
				this.y = y;
			}

			// Token: 0x04000F64 RID: 3940
			public int x;

			// Token: 0x04000F65 RID: 3941
			public int y;
		}

		// Token: 0x02000151 RID: 337
		[Guid("00000122-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleDropTarget
		{
			// Token: 0x060006A3 RID: 1699
			[PreserveSig]
			int OleDragEnter([MarshalAs(UnmanagedType.Interface)] [In] object pDataObj, [MarshalAs(UnmanagedType.U4)] [In] int grfKeyState, [MarshalAs(UnmanagedType.U8)] [In] long pt, [In] [Out] ref int pdwEffect);

			// Token: 0x060006A4 RID: 1700
			[PreserveSig]
			int OleDragOver([MarshalAs(UnmanagedType.U4)] [In] int grfKeyState, [MarshalAs(UnmanagedType.U8)] [In] long pt, [In] [Out] ref int pdwEffect);

			// Token: 0x060006A5 RID: 1701
			[PreserveSig]
			int OleDragLeave();

			// Token: 0x060006A6 RID: 1702
			[PreserveSig]
			int OleDrop([MarshalAs(UnmanagedType.Interface)] [In] object pDataObj, [MarshalAs(UnmanagedType.U4)] [In] int grfKeyState, [MarshalAs(UnmanagedType.U8)] [In] long pt, [In] [Out] ref int pdwEffect);
		}

		// Token: 0x02000152 RID: 338
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("00000121-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IOleDropSource
		{
			// Token: 0x060006A7 RID: 1703
			[PreserveSig]
			int OleQueryContinueDrag(int fEscapePressed, [MarshalAs(UnmanagedType.U4)] [In] int grfKeyState);

			// Token: 0x060006A8 RID: 1704
			[PreserveSig]
			int OleGiveFeedback([MarshalAs(UnmanagedType.U4)] [In] int dwEffect);
		}

		// Token: 0x02000153 RID: 339
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("00000016-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IOleMessageFilter
		{
			// Token: 0x060006A9 RID: 1705
			[PreserveSig]
			int HandleInComingCall(int dwCallType, IntPtr hTaskCaller, int dwTickCount, IntPtr lpInterfaceInfo);

			// Token: 0x060006AA RID: 1706
			[PreserveSig]
			int RetryRejectedCall(IntPtr hTaskCallee, int dwTickCount, int dwRejectType);

			// Token: 0x060006AB RID: 1707
			[PreserveSig]
			int MessagePending(IntPtr hTaskCallee, int dwTickCount, int dwPendingType);
		}

		// Token: 0x02000154 RID: 340
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("B196B289-BAB4-101A-B69C-00AA00341D07")]
		[ComImport]
		public interface IOleControlSite
		{
			// Token: 0x060006AC RID: 1708
			[PreserveSig]
			int OnControlInfoChanged();

			// Token: 0x060006AD RID: 1709
			[PreserveSig]
			int LockInPlaceActive(int fLock);

			// Token: 0x060006AE RID: 1710
			[PreserveSig]
			int GetExtendedControl([MarshalAs(UnmanagedType.IDispatch)] out object ppDisp);

			// Token: 0x060006AF RID: 1711
			[PreserveSig]
			int TransformCoords([In] [Out] NativeMethods._POINTL pPtlHimetric, [In] [Out] NativeMethods.tagPOINTF pPtfContainer, [MarshalAs(UnmanagedType.U4)] [In] int dwFlags);

			// Token: 0x060006B0 RID: 1712
			[PreserveSig]
			int TranslateAccelerator([In] ref NativeMethods.MSG pMsg, [MarshalAs(UnmanagedType.U4)] [In] int grfModifiers);

			// Token: 0x060006B1 RID: 1713
			[PreserveSig]
			int OnFocus(int fGotFocus);

			// Token: 0x060006B2 RID: 1714
			[PreserveSig]
			int ShowPropertyFrame();
		}

		// Token: 0x02000155 RID: 341
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("00000118-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IOleClientSite
		{
			// Token: 0x060006B3 RID: 1715
			[PreserveSig]
			int SaveObject();

			// Token: 0x060006B4 RID: 1716
			[PreserveSig]
			int GetMoniker([MarshalAs(UnmanagedType.U4)] [In] int dwAssign, [MarshalAs(UnmanagedType.U4)] [In] int dwWhichMoniker, [MarshalAs(UnmanagedType.Interface)] out object moniker);

			// Token: 0x060006B5 RID: 1717
			[PreserveSig]
			int GetContainer(out UnsafeNativeMethods.IOleContainer container);

			// Token: 0x060006B6 RID: 1718
			[PreserveSig]
			int ShowObject();

			// Token: 0x060006B7 RID: 1719
			[PreserveSig]
			int OnShowWindow(int fShow);

			// Token: 0x060006B8 RID: 1720
			[PreserveSig]
			int RequestNewObjectLayout();
		}

		// Token: 0x02000156 RID: 342
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("00000119-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IOleInPlaceSite
		{
			// Token: 0x060006B9 RID: 1721
			IntPtr GetWindow();

			// Token: 0x060006BA RID: 1722
			[PreserveSig]
			int ContextSensitiveHelp(int fEnterMode);

			// Token: 0x060006BB RID: 1723
			[PreserveSig]
			int CanInPlaceActivate();

			// Token: 0x060006BC RID: 1724
			[PreserveSig]
			int OnInPlaceActivate();

			// Token: 0x060006BD RID: 1725
			[PreserveSig]
			int OnUIActivate();

			// Token: 0x060006BE RID: 1726
			[PreserveSig]
			int GetWindowContext([MarshalAs(UnmanagedType.Interface)] out UnsafeNativeMethods.IOleInPlaceFrame ppFrame, [MarshalAs(UnmanagedType.Interface)] out UnsafeNativeMethods.IOleInPlaceUIWindow ppDoc, [Out] NativeMethods.COMRECT lprcPosRect, [Out] NativeMethods.COMRECT lprcClipRect, [In] [Out] NativeMethods.tagOIFI lpFrameInfo);

			// Token: 0x060006BF RID: 1727
			[PreserveSig]
			int Scroll(NativeMethods.tagSIZE scrollExtant);

			// Token: 0x060006C0 RID: 1728
			[PreserveSig]
			int OnUIDeactivate(int fUndoable);

			// Token: 0x060006C1 RID: 1729
			[PreserveSig]
			int OnInPlaceDeactivate();

			// Token: 0x060006C2 RID: 1730
			[PreserveSig]
			int DiscardUndoState();

			// Token: 0x060006C3 RID: 1731
			[PreserveSig]
			int DeactivateAndUndo();

			// Token: 0x060006C4 RID: 1732
			[PreserveSig]
			int OnPosRectChange([In] NativeMethods.COMRECT lprcPosRect);
		}

		// Token: 0x02000157 RID: 343
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("742B0E01-14E6-101B-914E-00AA00300CAB")]
		[ComImport]
		public interface ISimpleFrameSite
		{
			// Token: 0x060006C5 RID: 1733
			[PreserveSig]
			int PreMessageFilter(IntPtr hwnd, [MarshalAs(UnmanagedType.U4)] [In] int msg, IntPtr wp, IntPtr lp, [In] [Out] ref IntPtr plResult, [MarshalAs(UnmanagedType.U4)] [In] [Out] ref int pdwCookie);

			// Token: 0x060006C6 RID: 1734
			[PreserveSig]
			int PostMessageFilter(IntPtr hwnd, [MarshalAs(UnmanagedType.U4)] [In] int msg, IntPtr wp, IntPtr lp, [In] [Out] ref IntPtr plResult, [MarshalAs(UnmanagedType.U4)] [In] int dwCookie);
		}

		// Token: 0x02000158 RID: 344
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("40A050A0-3C31-101B-A82E-08002B2B2337")]
		[ComImport]
		public interface IVBGetControl
		{
			// Token: 0x060006C7 RID: 1735
			[PreserveSig]
			int EnumControls(int dwOleContF, int dwWhich, out UnsafeNativeMethods.IEnumUnknown ppenum);
		}

		// Token: 0x02000159 RID: 345
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("91733A60-3F4C-101B-A3F6-00AA0034E4E9")]
		[ComImport]
		public interface IGetVBAObject
		{
			// Token: 0x060006C8 RID: 1736
			[PreserveSig]
			int GetObject([In] ref Guid riid, [MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.IVBFormat[] rval, int dwReserved);
		}

		// Token: 0x0200015A RID: 346
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("9BFBBC02-EFF1-101A-84ED-00AA00341D07")]
		[ComImport]
		public interface IPropertyNotifySink
		{
			// Token: 0x060006C9 RID: 1737
			void OnChanged(int dispID);

			// Token: 0x060006CA RID: 1738
			[PreserveSig]
			int OnRequestEdit(int dispID);
		}

		// Token: 0x0200015B RID: 347
		[Guid("9849FD60-3768-101B-8D72-AE6164FFE3CF")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IVBFormat
		{
			// Token: 0x060006CB RID: 1739
			[PreserveSig]
			int Format([In] ref object var, IntPtr pszFormat, IntPtr lpBuffer, short cpBuffer, int lcid, short firstD, short firstW, [MarshalAs(UnmanagedType.LPArray)] [Out] short[] result);
		}

		// Token: 0x0200015C RID: 348
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("00000100-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IEnumUnknown
		{
			// Token: 0x060006CC RID: 1740
			[PreserveSig]
			int Next([MarshalAs(UnmanagedType.U4)] [In] int celt, [Out] IntPtr rgelt, IntPtr pceltFetched);

			// Token: 0x060006CD RID: 1741
			[PreserveSig]
			int Skip([MarshalAs(UnmanagedType.U4)] [In] int celt);

			// Token: 0x060006CE RID: 1742
			void Reset();

			// Token: 0x060006CF RID: 1743
			void Clone(out UnsafeNativeMethods.IEnumUnknown ppenum);
		}

		// Token: 0x0200015D RID: 349
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("0000011B-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IOleContainer
		{
			// Token: 0x060006D0 RID: 1744
			[PreserveSig]
			int ParseDisplayName([MarshalAs(UnmanagedType.Interface)] [In] object pbc, [MarshalAs(UnmanagedType.BStr)] [In] string pszDisplayName, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pchEaten, [MarshalAs(UnmanagedType.LPArray)] [Out] object[] ppmkOut);

			// Token: 0x060006D1 RID: 1745
			[PreserveSig]
			int EnumObjects([MarshalAs(UnmanagedType.U4)] [In] int grfFlags, out UnsafeNativeMethods.IEnumUnknown ppenum);

			// Token: 0x060006D2 RID: 1746
			[PreserveSig]
			int LockContainer(bool fLock);
		}

		// Token: 0x0200015E RID: 350
		[Guid("00000116-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleInPlaceFrame
		{
			// Token: 0x060006D3 RID: 1747
			IntPtr GetWindow();

			// Token: 0x060006D4 RID: 1748
			[PreserveSig]
			int ContextSensitiveHelp(int fEnterMode);

			// Token: 0x060006D5 RID: 1749
			[PreserveSig]
			int GetBorder([Out] NativeMethods.COMRECT lprectBorder);

			// Token: 0x060006D6 RID: 1750
			[PreserveSig]
			int RequestBorderSpace([In] NativeMethods.COMRECT pborderwidths);

			// Token: 0x060006D7 RID: 1751
			[PreserveSig]
			int SetBorderSpace([In] NativeMethods.COMRECT pborderwidths);

			// Token: 0x060006D8 RID: 1752
			[PreserveSig]
			int SetActiveObject([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IOleInPlaceActiveObject pActiveObject, [MarshalAs(UnmanagedType.LPWStr)] [In] string pszObjName);

			// Token: 0x060006D9 RID: 1753
			[PreserveSig]
			int InsertMenus([In] IntPtr hmenuShared, [In] [Out] NativeMethods.tagOleMenuGroupWidths lpMenuWidths);

			// Token: 0x060006DA RID: 1754
			[PreserveSig]
			int SetMenu([In] IntPtr hmenuShared, [In] IntPtr holemenu, [In] IntPtr hwndActiveObject);

			// Token: 0x060006DB RID: 1755
			[PreserveSig]
			int RemoveMenus([In] IntPtr hmenuShared);

			// Token: 0x060006DC RID: 1756
			[PreserveSig]
			int SetStatusText([MarshalAs(UnmanagedType.LPWStr)] [In] string pszStatusText);

			// Token: 0x060006DD RID: 1757
			[PreserveSig]
			int EnableModeless(bool fEnable);

			// Token: 0x060006DE RID: 1758
			[PreserveSig]
			int TranslateAccelerator([In] ref NativeMethods.MSG lpmsg, [MarshalAs(UnmanagedType.U2)] [In] short wID);
		}

		// Token: 0x0200015F RID: 351
		[Guid("BD3F23C0-D43E-11CF-893B-00AA00BDCE1A")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComVisible(true)]
		[ComImport]
		public interface IDocHostUIHandler
		{
			// Token: 0x060006DF RID: 1759
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int ShowContextMenu([MarshalAs(UnmanagedType.U4)] [In] int dwID, [In] NativeMethods.POINT pt, [MarshalAs(UnmanagedType.Interface)] [In] object pcmdtReserved, [MarshalAs(UnmanagedType.Interface)] [In] object pdispReserved);

			// Token: 0x060006E0 RID: 1760
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int GetHostInfo([In] [Out] NativeMethods.DOCHOSTUIINFO info);

			// Token: 0x060006E1 RID: 1761
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int ShowUI([MarshalAs(UnmanagedType.I4)] [In] int dwID, [In] UnsafeNativeMethods.IOleInPlaceActiveObject activeObject, [In] NativeMethods.IOleCommandTarget commandTarget, [In] UnsafeNativeMethods.IOleInPlaceFrame frame, [In] UnsafeNativeMethods.IOleInPlaceUIWindow doc);

			// Token: 0x060006E2 RID: 1762
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int HideUI();

			// Token: 0x060006E3 RID: 1763
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int UpdateUI();

			// Token: 0x060006E4 RID: 1764
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int EnableModeless([MarshalAs(UnmanagedType.Bool)] [In] bool fEnable);

			// Token: 0x060006E5 RID: 1765
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int OnDocWindowActivate([MarshalAs(UnmanagedType.Bool)] [In] bool fActivate);

			// Token: 0x060006E6 RID: 1766
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int OnFrameWindowActivate([MarshalAs(UnmanagedType.Bool)] [In] bool fActivate);

			// Token: 0x060006E7 RID: 1767
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int ResizeBorder([In] NativeMethods.COMRECT rect, [In] UnsafeNativeMethods.IOleInPlaceUIWindow doc, bool fFrameWindow);

			// Token: 0x060006E8 RID: 1768
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int TranslateAccelerator([In] ref NativeMethods.MSG msg, [In] ref Guid group, [MarshalAs(UnmanagedType.I4)] [In] int nCmdID);

			// Token: 0x060006E9 RID: 1769
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int GetOptionKeyPath([MarshalAs(UnmanagedType.LPArray)] [Out] string[] pbstrKey, [MarshalAs(UnmanagedType.U4)] [In] int dw);

			// Token: 0x060006EA RID: 1770
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int GetDropTarget([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IOleDropTarget pDropTarget, [MarshalAs(UnmanagedType.Interface)] out UnsafeNativeMethods.IOleDropTarget ppDropTarget);

			// Token: 0x060006EB RID: 1771
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int GetExternal([MarshalAs(UnmanagedType.Interface)] out object ppDispatch);

			// Token: 0x060006EC RID: 1772
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int TranslateUrl([MarshalAs(UnmanagedType.U4)] [In] int dwTranslate, [MarshalAs(UnmanagedType.LPWStr)] [In] string strURLIn, [MarshalAs(UnmanagedType.LPWStr)] out string pstrURLOut);

			// Token: 0x060006ED RID: 1773
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int FilterDataObject(IDataObject pDO, out IDataObject ppDORet);
		}

		// Token: 0x02000160 RID: 352
		[Guid("D30C1661-CDAF-11d0-8A3E-00C04FC9E26E")]
		[TypeLibType(TypeLibTypeFlags.FHidden | TypeLibTypeFlags.FDual | TypeLibTypeFlags.FOleAutomation)]
		[SuppressUnmanagedCodeSecurity]
		[ComImport]
		public interface IWebBrowser2
		{
			// Token: 0x060006EE RID: 1774
			[DispId(100)]
			void GoBack();

			// Token: 0x060006EF RID: 1775
			[DispId(101)]
			void GoForward();

			// Token: 0x060006F0 RID: 1776
			[DispId(102)]
			void GoHome();

			// Token: 0x060006F1 RID: 1777
			[DispId(103)]
			void GoSearch();

			// Token: 0x060006F2 RID: 1778
			[DispId(104)]
			void Navigate([In] string Url, [In] ref object flags, [In] ref object targetFrameName, [In] ref object postData, [In] ref object headers);

			// Token: 0x060006F3 RID: 1779
			[DispId(-550)]
			void Refresh();

			// Token: 0x060006F4 RID: 1780
			[DispId(105)]
			void Refresh2([In] ref object level);

			// Token: 0x060006F5 RID: 1781
			[DispId(106)]
			void Stop();

			// Token: 0x17000180 RID: 384
			// (get) Token: 0x060006F6 RID: 1782
			[DispId(200)]
			object Application
			{
				[return: MarshalAs(UnmanagedType.IDispatch)]
				get;
			}

			// Token: 0x17000181 RID: 385
			// (get) Token: 0x060006F7 RID: 1783
			[DispId(201)]
			object Parent
			{
				[return: MarshalAs(UnmanagedType.IDispatch)]
				get;
			}

			// Token: 0x17000182 RID: 386
			// (get) Token: 0x060006F8 RID: 1784
			[DispId(202)]
			object Container
			{
				[return: MarshalAs(UnmanagedType.IDispatch)]
				get;
			}

			// Token: 0x17000183 RID: 387
			// (get) Token: 0x060006F9 RID: 1785
			[DispId(203)]
			object Document
			{
				[return: MarshalAs(UnmanagedType.IDispatch)]
				get;
			}

			// Token: 0x17000184 RID: 388
			// (get) Token: 0x060006FA RID: 1786
			[DispId(204)]
			bool TopLevelContainer { get; }

			// Token: 0x17000185 RID: 389
			// (get) Token: 0x060006FB RID: 1787
			[DispId(205)]
			string Type { get; }

			// Token: 0x17000186 RID: 390
			// (get) Token: 0x060006FC RID: 1788
			// (set) Token: 0x060006FD RID: 1789
			[DispId(206)]
			int Left { get; set; }

			// Token: 0x17000187 RID: 391
			// (get) Token: 0x060006FE RID: 1790
			// (set) Token: 0x060006FF RID: 1791
			[DispId(207)]
			int Top { get; set; }

			// Token: 0x17000188 RID: 392
			// (get) Token: 0x06000700 RID: 1792
			// (set) Token: 0x06000701 RID: 1793
			[DispId(208)]
			int Width { get; set; }

			// Token: 0x17000189 RID: 393
			// (get) Token: 0x06000702 RID: 1794
			// (set) Token: 0x06000703 RID: 1795
			[DispId(209)]
			int Height { get; set; }

			// Token: 0x1700018A RID: 394
			// (get) Token: 0x06000704 RID: 1796
			[DispId(210)]
			string LocationName { get; }

			// Token: 0x1700018B RID: 395
			// (get) Token: 0x06000705 RID: 1797
			[DispId(211)]
			string LocationURL { get; }

			// Token: 0x1700018C RID: 396
			// (get) Token: 0x06000706 RID: 1798
			[DispId(212)]
			bool Busy { get; }

			// Token: 0x06000707 RID: 1799
			[DispId(300)]
			void Quit();

			// Token: 0x06000708 RID: 1800
			[DispId(301)]
			void ClientToWindow(out int pcx, out int pcy);

			// Token: 0x06000709 RID: 1801
			[DispId(302)]
			void PutProperty([In] string property, [In] object vtValue);

			// Token: 0x0600070A RID: 1802
			[DispId(303)]
			object GetProperty([In] string property);

			// Token: 0x1700018D RID: 397
			// (get) Token: 0x0600070B RID: 1803
			[DispId(0)]
			string Name { get; }

			// Token: 0x1700018E RID: 398
			// (get) Token: 0x0600070C RID: 1804
			[DispId(-515)]
			int HWND { get; }

			// Token: 0x1700018F RID: 399
			// (get) Token: 0x0600070D RID: 1805
			[DispId(400)]
			string FullName { get; }

			// Token: 0x17000190 RID: 400
			// (get) Token: 0x0600070E RID: 1806
			[DispId(401)]
			string Path { get; }

			// Token: 0x17000191 RID: 401
			// (get) Token: 0x0600070F RID: 1807
			// (set) Token: 0x06000710 RID: 1808
			[DispId(402)]
			bool Visible { get; set; }

			// Token: 0x17000192 RID: 402
			// (get) Token: 0x06000711 RID: 1809
			// (set) Token: 0x06000712 RID: 1810
			[DispId(403)]
			bool StatusBar { get; set; }

			// Token: 0x17000193 RID: 403
			// (get) Token: 0x06000713 RID: 1811
			// (set) Token: 0x06000714 RID: 1812
			[DispId(404)]
			string StatusText { get; set; }

			// Token: 0x17000194 RID: 404
			// (get) Token: 0x06000715 RID: 1813
			// (set) Token: 0x06000716 RID: 1814
			[DispId(405)]
			int ToolBar { get; set; }

			// Token: 0x17000195 RID: 405
			// (get) Token: 0x06000717 RID: 1815
			// (set) Token: 0x06000718 RID: 1816
			[DispId(406)]
			bool MenuBar { get; set; }

			// Token: 0x17000196 RID: 406
			// (get) Token: 0x06000719 RID: 1817
			// (set) Token: 0x0600071A RID: 1818
			[DispId(407)]
			bool FullScreen { get; set; }

			// Token: 0x0600071B RID: 1819
			[DispId(500)]
			void Navigate2([In] ref object URL, [In] ref object flags, [In] ref object targetFrameName, [In] ref object postData, [In] ref object headers);

			// Token: 0x0600071C RID: 1820
			[DispId(501)]
			NativeMethods.OLECMDF QueryStatusWB([In] NativeMethods.OLECMDID cmdID);

			// Token: 0x0600071D RID: 1821
			[DispId(502)]
			void ExecWB([In] NativeMethods.OLECMDID cmdID, [In] NativeMethods.OLECMDEXECOPT cmdexecopt, ref object pvaIn, IntPtr pvaOut);

			// Token: 0x0600071E RID: 1822
			[DispId(503)]
			void ShowBrowserBar([In] ref object pvaClsid, [In] ref object pvarShow, [In] ref object pvarSize);

			// Token: 0x17000197 RID: 407
			// (get) Token: 0x0600071F RID: 1823
			[DispId(-525)]
			WebBrowserReadyState ReadyState { get; }

			// Token: 0x17000198 RID: 408
			// (get) Token: 0x06000720 RID: 1824
			// (set) Token: 0x06000721 RID: 1825
			[DispId(550)]
			bool Offline { get; set; }

			// Token: 0x17000199 RID: 409
			// (get) Token: 0x06000722 RID: 1826
			// (set) Token: 0x06000723 RID: 1827
			[DispId(551)]
			bool Silent { get; set; }

			// Token: 0x1700019A RID: 410
			// (get) Token: 0x06000724 RID: 1828
			// (set) Token: 0x06000725 RID: 1829
			[DispId(552)]
			bool RegisterAsBrowser { get; set; }

			// Token: 0x1700019B RID: 411
			// (get) Token: 0x06000726 RID: 1830
			// (set) Token: 0x06000727 RID: 1831
			[DispId(553)]
			bool RegisterAsDropTarget { get; set; }

			// Token: 0x1700019C RID: 412
			// (get) Token: 0x06000728 RID: 1832
			// (set) Token: 0x06000729 RID: 1833
			[DispId(554)]
			bool TheaterMode { get; set; }

			// Token: 0x1700019D RID: 413
			// (get) Token: 0x0600072A RID: 1834
			// (set) Token: 0x0600072B RID: 1835
			[DispId(555)]
			bool AddressBar { get; set; }

			// Token: 0x1700019E RID: 414
			// (get) Token: 0x0600072C RID: 1836
			// (set) Token: 0x0600072D RID: 1837
			[DispId(556)]
			bool Resizable { get; set; }
		}

		// Token: 0x02000161 RID: 353
		[Guid("34A715A0-6587-11D0-924A-0020AFC7AC4D")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DWebBrowserEvents2
		{
			// Token: 0x0600072E RID: 1838
			[DispId(102)]
			void StatusTextChange([In] string text);

			// Token: 0x0600072F RID: 1839
			[DispId(108)]
			void ProgressChange([In] int progress, [In] int progressMax);

			// Token: 0x06000730 RID: 1840
			[DispId(105)]
			void CommandStateChange([In] long command, [In] bool enable);

			// Token: 0x06000731 RID: 1841
			[DispId(106)]
			void DownloadBegin();

			// Token: 0x06000732 RID: 1842
			[DispId(104)]
			void DownloadComplete();

			// Token: 0x06000733 RID: 1843
			[DispId(113)]
			void TitleChange([In] string text);

			// Token: 0x06000734 RID: 1844
			[DispId(112)]
			void PropertyChange([In] string szProperty);

			// Token: 0x06000735 RID: 1845
			[DispId(250)]
			void BeforeNavigate2([MarshalAs(UnmanagedType.IDispatch)] [In] object pDisp, [In] ref object URL, [In] ref object flags, [In] ref object targetFrameName, [In] ref object postData, [In] ref object headers, [In] [Out] ref bool cancel);

			// Token: 0x06000736 RID: 1846
			[DispId(251)]
			void NewWindow2([MarshalAs(UnmanagedType.IDispatch)] [In] [Out] ref object pDisp, [In] [Out] ref bool cancel);

			// Token: 0x06000737 RID: 1847
			[DispId(252)]
			void NavigateComplete2([MarshalAs(UnmanagedType.IDispatch)] [In] object pDisp, [In] ref object URL);

			// Token: 0x06000738 RID: 1848
			[DispId(259)]
			void DocumentComplete([MarshalAs(UnmanagedType.IDispatch)] [In] object pDisp, [In] ref object URL);

			// Token: 0x06000739 RID: 1849
			[DispId(253)]
			void OnQuit();

			// Token: 0x0600073A RID: 1850
			[DispId(254)]
			void OnVisible([In] bool visible);

			// Token: 0x0600073B RID: 1851
			[DispId(255)]
			void OnToolBar([In] bool toolBar);

			// Token: 0x0600073C RID: 1852
			[DispId(256)]
			void OnMenuBar([In] bool menuBar);

			// Token: 0x0600073D RID: 1853
			[DispId(257)]
			void OnStatusBar([In] bool statusBar);

			// Token: 0x0600073E RID: 1854
			[DispId(258)]
			void OnFullScreen([In] bool fullScreen);

			// Token: 0x0600073F RID: 1855
			[DispId(260)]
			void OnTheaterMode([In] bool theaterMode);

			// Token: 0x06000740 RID: 1856
			[DispId(262)]
			void WindowSetResizable([In] bool resizable);

			// Token: 0x06000741 RID: 1857
			[DispId(264)]
			void WindowSetLeft([In] int left);

			// Token: 0x06000742 RID: 1858
			[DispId(265)]
			void WindowSetTop([In] int top);

			// Token: 0x06000743 RID: 1859
			[DispId(266)]
			void WindowSetWidth([In] int width);

			// Token: 0x06000744 RID: 1860
			[DispId(267)]
			void WindowSetHeight([In] int height);

			// Token: 0x06000745 RID: 1861
			[DispId(263)]
			void WindowClosing([In] bool isChildWindow, [In] [Out] ref bool cancel);

			// Token: 0x06000746 RID: 1862
			[DispId(268)]
			void ClientToHostWindow([In] [Out] ref long cx, [In] [Out] ref long cy);

			// Token: 0x06000747 RID: 1863
			[DispId(269)]
			void SetSecureLockIcon([In] int secureLockIcon);

			// Token: 0x06000748 RID: 1864
			[DispId(270)]
			void FileDownload([In] [Out] ref bool cancel);

			// Token: 0x06000749 RID: 1865
			[DispId(271)]
			void NavigateError([MarshalAs(UnmanagedType.IDispatch)] [In] object pDisp, [In] ref object URL, [In] ref object frame, [In] ref object statusCode, [In] [Out] ref bool cancel);

			// Token: 0x0600074A RID: 1866
			[DispId(225)]
			void PrintTemplateInstantiation([MarshalAs(UnmanagedType.IDispatch)] [In] object pDisp);

			// Token: 0x0600074B RID: 1867
			[DispId(226)]
			void PrintTemplateTeardown([MarshalAs(UnmanagedType.IDispatch)] [In] object pDisp);

			// Token: 0x0600074C RID: 1868
			[DispId(227)]
			void UpdatePageStatus([MarshalAs(UnmanagedType.IDispatch)] [In] object pDisp, [In] ref object nPage, [In] ref object fDone);

			// Token: 0x0600074D RID: 1869
			[DispId(272)]
			void PrivacyImpactedStateChange([In] bool bImpacted);
		}

		// Token: 0x02000162 RID: 354
		[SuppressUnmanagedCodeSecurity]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComVisible(true)]
		[Guid("626FC520-A41E-11cf-A731-00A0C9082637")]
		internal interface IHTMLDocument
		{
			// Token: 0x0600074E RID: 1870
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object GetScript();
		}

		// Token: 0x02000163 RID: 355
		[Guid("332C4425-26CB-11D0-B483-00C04FD90119")]
		[ComVisible(true)]
		[SuppressUnmanagedCodeSecurity]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		internal interface IHTMLDocument2
		{
			// Token: 0x0600074F RID: 1871
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object GetScript();

			// Token: 0x06000750 RID: 1872
			UnsafeNativeMethods.IHTMLElementCollection GetAll();

			// Token: 0x06000751 RID: 1873
			UnsafeNativeMethods.IHTMLElement GetBody();

			// Token: 0x06000752 RID: 1874
			UnsafeNativeMethods.IHTMLElement GetActiveElement();

			// Token: 0x06000753 RID: 1875
			UnsafeNativeMethods.IHTMLElementCollection GetImages();

			// Token: 0x06000754 RID: 1876
			UnsafeNativeMethods.IHTMLElementCollection GetApplets();

			// Token: 0x06000755 RID: 1877
			UnsafeNativeMethods.IHTMLElementCollection GetLinks();

			// Token: 0x06000756 RID: 1878
			UnsafeNativeMethods.IHTMLElementCollection GetForms();

			// Token: 0x06000757 RID: 1879
			UnsafeNativeMethods.IHTMLElementCollection GetAnchors();

			// Token: 0x06000758 RID: 1880
			void SetTitle(string p);

			// Token: 0x06000759 RID: 1881
			string GetTitle();

			// Token: 0x0600075A RID: 1882
			UnsafeNativeMethods.IHTMLElementCollection GetScripts();

			// Token: 0x0600075B RID: 1883
			void SetDesignMode(string p);

			// Token: 0x0600075C RID: 1884
			string GetDesignMode();

			// Token: 0x0600075D RID: 1885
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetSelection();

			// Token: 0x0600075E RID: 1886
			string GetReadyState();

			// Token: 0x0600075F RID: 1887
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetFrames();

			// Token: 0x06000760 RID: 1888
			UnsafeNativeMethods.IHTMLElementCollection GetEmbeds();

			// Token: 0x06000761 RID: 1889
			UnsafeNativeMethods.IHTMLElementCollection GetPlugins();

			// Token: 0x06000762 RID: 1890
			void SetAlinkColor(object c);

			// Token: 0x06000763 RID: 1891
			object GetAlinkColor();

			// Token: 0x06000764 RID: 1892
			void SetBgColor(object c);

			// Token: 0x06000765 RID: 1893
			object GetBgColor();

			// Token: 0x06000766 RID: 1894
			void SetFgColor(object c);

			// Token: 0x06000767 RID: 1895
			object GetFgColor();

			// Token: 0x06000768 RID: 1896
			void SetLinkColor(object c);

			// Token: 0x06000769 RID: 1897
			object GetLinkColor();

			// Token: 0x0600076A RID: 1898
			void SetVlinkColor(object c);

			// Token: 0x0600076B RID: 1899
			object GetVlinkColor();

			// Token: 0x0600076C RID: 1900
			string GetReferrer();

			// Token: 0x0600076D RID: 1901
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLLocation GetLocation();

			// Token: 0x0600076E RID: 1902
			string GetLastModified();

			// Token: 0x0600076F RID: 1903
			void SetUrl(string p);

			// Token: 0x06000770 RID: 1904
			string GetUrl();

			// Token: 0x06000771 RID: 1905
			void SetDomain(string p);

			// Token: 0x06000772 RID: 1906
			string GetDomain();

			// Token: 0x06000773 RID: 1907
			void SetCookie(string p);

			// Token: 0x06000774 RID: 1908
			string GetCookie();

			// Token: 0x06000775 RID: 1909
			void SetExpando(bool p);

			// Token: 0x06000776 RID: 1910
			bool GetExpando();

			// Token: 0x06000777 RID: 1911
			void SetCharset(string p);

			// Token: 0x06000778 RID: 1912
			string GetCharset();

			// Token: 0x06000779 RID: 1913
			void SetDefaultCharset(string p);

			// Token: 0x0600077A RID: 1914
			string GetDefaultCharset();

			// Token: 0x0600077B RID: 1915
			string GetMimeType();

			// Token: 0x0600077C RID: 1916
			string GetFileSize();

			// Token: 0x0600077D RID: 1917
			string GetFileCreatedDate();

			// Token: 0x0600077E RID: 1918
			string GetFileModifiedDate();

			// Token: 0x0600077F RID: 1919
			string GetFileUpdatedDate();

			// Token: 0x06000780 RID: 1920
			string GetSecurity();

			// Token: 0x06000781 RID: 1921
			string GetProtocol();

			// Token: 0x06000782 RID: 1922
			string GetNameProp();

			// Token: 0x06000783 RID: 1923
			int Write([MarshalAs(UnmanagedType.SafeArray)] [In] object[] psarray);

			// Token: 0x06000784 RID: 1924
			int WriteLine([MarshalAs(UnmanagedType.SafeArray)] [In] object[] psarray);

			// Token: 0x06000785 RID: 1925
			[return: MarshalAs(UnmanagedType.Interface)]
			object Open(string mimeExtension, object name, object features, object replace);

			// Token: 0x06000786 RID: 1926
			void Close();

			// Token: 0x06000787 RID: 1927
			void Clear();

			// Token: 0x06000788 RID: 1928
			bool QueryCommandSupported(string cmdID);

			// Token: 0x06000789 RID: 1929
			bool QueryCommandEnabled(string cmdID);

			// Token: 0x0600078A RID: 1930
			bool QueryCommandState(string cmdID);

			// Token: 0x0600078B RID: 1931
			bool QueryCommandIndeterm(string cmdID);

			// Token: 0x0600078C RID: 1932
			string QueryCommandText(string cmdID);

			// Token: 0x0600078D RID: 1933
			object QueryCommandValue(string cmdID);

			// Token: 0x0600078E RID: 1934
			bool ExecCommand(string cmdID, bool showUI, object value);

			// Token: 0x0600078F RID: 1935
			bool ExecCommandShowHelp(string cmdID);

			// Token: 0x06000790 RID: 1936
			UnsafeNativeMethods.IHTMLElement CreateElement(string eTag);

			// Token: 0x06000791 RID: 1937
			void SetOnhelp(object p);

			// Token: 0x06000792 RID: 1938
			object GetOnhelp();

			// Token: 0x06000793 RID: 1939
			void SetOnclick(object p);

			// Token: 0x06000794 RID: 1940
			object GetOnclick();

			// Token: 0x06000795 RID: 1941
			void SetOndblclick(object p);

			// Token: 0x06000796 RID: 1942
			object GetOndblclick();

			// Token: 0x06000797 RID: 1943
			void SetOnkeyup(object p);

			// Token: 0x06000798 RID: 1944
			object GetOnkeyup();

			// Token: 0x06000799 RID: 1945
			void SetOnkeydown(object p);

			// Token: 0x0600079A RID: 1946
			object GetOnkeydown();

			// Token: 0x0600079B RID: 1947
			void SetOnkeypress(object p);

			// Token: 0x0600079C RID: 1948
			object GetOnkeypress();

			// Token: 0x0600079D RID: 1949
			void SetOnmouseup(object p);

			// Token: 0x0600079E RID: 1950
			object GetOnmouseup();

			// Token: 0x0600079F RID: 1951
			void SetOnmousedown(object p);

			// Token: 0x060007A0 RID: 1952
			object GetOnmousedown();

			// Token: 0x060007A1 RID: 1953
			void SetOnmousemove(object p);

			// Token: 0x060007A2 RID: 1954
			object GetOnmousemove();

			// Token: 0x060007A3 RID: 1955
			void SetOnmouseout(object p);

			// Token: 0x060007A4 RID: 1956
			object GetOnmouseout();

			// Token: 0x060007A5 RID: 1957
			void SetOnmouseover(object p);

			// Token: 0x060007A6 RID: 1958
			object GetOnmouseover();

			// Token: 0x060007A7 RID: 1959
			void SetOnreadystatechange(object p);

			// Token: 0x060007A8 RID: 1960
			object GetOnreadystatechange();

			// Token: 0x060007A9 RID: 1961
			void SetOnafterupdate(object p);

			// Token: 0x060007AA RID: 1962
			object GetOnafterupdate();

			// Token: 0x060007AB RID: 1963
			void SetOnrowexit(object p);

			// Token: 0x060007AC RID: 1964
			object GetOnrowexit();

			// Token: 0x060007AD RID: 1965
			void SetOnrowenter(object p);

			// Token: 0x060007AE RID: 1966
			object GetOnrowenter();

			// Token: 0x060007AF RID: 1967
			void SetOndragstart(object p);

			// Token: 0x060007B0 RID: 1968
			object GetOndragstart();

			// Token: 0x060007B1 RID: 1969
			void SetOnselectstart(object p);

			// Token: 0x060007B2 RID: 1970
			object GetOnselectstart();

			// Token: 0x060007B3 RID: 1971
			UnsafeNativeMethods.IHTMLElement ElementFromPoint(int x, int y);

			// Token: 0x060007B4 RID: 1972
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLWindow2 GetParentWindow();

			// Token: 0x060007B5 RID: 1973
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetStyleSheets();

			// Token: 0x060007B6 RID: 1974
			void SetOnbeforeupdate(object p);

			// Token: 0x060007B7 RID: 1975
			object GetOnbeforeupdate();

			// Token: 0x060007B8 RID: 1976
			void SetOnerrorupdate(object p);

			// Token: 0x060007B9 RID: 1977
			object GetOnerrorupdate();

			// Token: 0x060007BA RID: 1978
			string toString();

			// Token: 0x060007BB RID: 1979
			[return: MarshalAs(UnmanagedType.Interface)]
			object CreateStyleSheet(string bstrHref, int lIndex);
		}

		// Token: 0x02000164 RID: 356
		[Guid("3050F485-98B5-11CF-BB82-00AA00BDCE0B")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		internal interface IHTMLDocument3
		{
			// Token: 0x060007BC RID: 1980
			void ReleaseCapture();

			// Token: 0x060007BD RID: 1981
			void Recalc([In] bool fForce);

			// Token: 0x060007BE RID: 1982
			object CreateTextNode([In] string text);

			// Token: 0x060007BF RID: 1983
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLElement GetDocumentElement();

			// Token: 0x060007C0 RID: 1984
			string GetUniqueID();

			// Token: 0x060007C1 RID: 1985
			bool AttachEvent([In] string ev, [MarshalAs(UnmanagedType.IDispatch)] [In] object pdisp);

			// Token: 0x060007C2 RID: 1986
			void DetachEvent([In] string ev, [MarshalAs(UnmanagedType.IDispatch)] [In] object pdisp);

			// Token: 0x060007C3 RID: 1987
			void SetOnrowsdelete([In] object p);

			// Token: 0x060007C4 RID: 1988
			object GetOnrowsdelete();

			// Token: 0x060007C5 RID: 1989
			void SetOnrowsinserted([In] object p);

			// Token: 0x060007C6 RID: 1990
			object GetOnrowsinserted();

			// Token: 0x060007C7 RID: 1991
			void SetOncellchange([In] object p);

			// Token: 0x060007C8 RID: 1992
			object GetOncellchange();

			// Token: 0x060007C9 RID: 1993
			void SetOndatasetchanged([In] object p);

			// Token: 0x060007CA RID: 1994
			object GetOndatasetchanged();

			// Token: 0x060007CB RID: 1995
			void SetOndataavailable([In] object p);

			// Token: 0x060007CC RID: 1996
			object GetOndataavailable();

			// Token: 0x060007CD RID: 1997
			void SetOndatasetcomplete([In] object p);

			// Token: 0x060007CE RID: 1998
			object GetOndatasetcomplete();

			// Token: 0x060007CF RID: 1999
			void SetOnpropertychange([In] object p);

			// Token: 0x060007D0 RID: 2000
			object GetOnpropertychange();

			// Token: 0x060007D1 RID: 2001
			void SetDir([In] string p);

			// Token: 0x060007D2 RID: 2002
			string GetDir();

			// Token: 0x060007D3 RID: 2003
			void SetOncontextmenu([In] object p);

			// Token: 0x060007D4 RID: 2004
			object GetOncontextmenu();

			// Token: 0x060007D5 RID: 2005
			void SetOnstop([In] object p);

			// Token: 0x060007D6 RID: 2006
			object GetOnstop();

			// Token: 0x060007D7 RID: 2007
			object CreateDocumentFragment();

			// Token: 0x060007D8 RID: 2008
			object GetParentDocument();

			// Token: 0x060007D9 RID: 2009
			void SetEnableDownload([In] bool p);

			// Token: 0x060007DA RID: 2010
			bool GetEnableDownload();

			// Token: 0x060007DB RID: 2011
			void SetBaseUrl([In] string p);

			// Token: 0x060007DC RID: 2012
			string GetBaseUrl();

			// Token: 0x060007DD RID: 2013
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object GetChildNodes();

			// Token: 0x060007DE RID: 2014
			void SetInheritStyleSheets([In] bool p);

			// Token: 0x060007DF RID: 2015
			bool GetInheritStyleSheets();

			// Token: 0x060007E0 RID: 2016
			void SetOnbeforeeditfocus([In] object p);

			// Token: 0x060007E1 RID: 2017
			object GetOnbeforeeditfocus();

			// Token: 0x060007E2 RID: 2018
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLElementCollection GetElementsByName([In] string v);

			// Token: 0x060007E3 RID: 2019
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLElement GetElementById([In] string v);

			// Token: 0x060007E4 RID: 2020
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLElementCollection GetElementsByTagName([In] string v);
		}

		// Token: 0x02000165 RID: 357
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[Guid("3050F69A-98B5-11CF-BB82-00AA00BDCE0B")]
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		internal interface IHTMLDocument4
		{
			// Token: 0x060007E5 RID: 2021
			void Focus();

			// Token: 0x060007E6 RID: 2022
			bool HasFocus();

			// Token: 0x060007E7 RID: 2023
			void SetOnselectionchange(object p);

			// Token: 0x060007E8 RID: 2024
			object GetOnselectionchange();

			// Token: 0x060007E9 RID: 2025
			object GetNamespaces();

			// Token: 0x060007EA RID: 2026
			object createDocumentFromUrl(string bstrUrl, string bstrOptions);

			// Token: 0x060007EB RID: 2027
			void SetMedia(string bstrMedia);

			// Token: 0x060007EC RID: 2028
			string GetMedia();

			// Token: 0x060007ED RID: 2029
			object CreateEventObject([In] [Optional] ref object eventObject);

			// Token: 0x060007EE RID: 2030
			bool FireEvent(string eventName);

			// Token: 0x060007EF RID: 2031
			object CreateRenderStyle(string bstr);

			// Token: 0x060007F0 RID: 2032
			void SetOncontrolselect(object p);

			// Token: 0x060007F1 RID: 2033
			object GetOncontrolselect();

			// Token: 0x060007F2 RID: 2034
			string GetURLUnencoded();
		}

		// Token: 0x02000166 RID: 358
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[Guid("3050f613-98b5-11cf-bb82-00aa00bdce0b")]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLDocumentEvents2
		{
			// Token: 0x060007F3 RID: 2035
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060007F4 RID: 2036
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060007F5 RID: 2037
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060007F6 RID: 2038
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060007F7 RID: 2039
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060007F8 RID: 2040
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060007F9 RID: 2041
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060007FA RID: 2042
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060007FB RID: 2043
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060007FC RID: 2044
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060007FD RID: 2045
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060007FE RID: 2046
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x060007FF RID: 2047
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000800 RID: 2048
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000801 RID: 2049
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000802 RID: 2050
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000803 RID: 2051
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000804 RID: 2052
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000805 RID: 2053
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000806 RID: 2054
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000807 RID: 2055
			[DispId(1026)]
			bool onstop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000808 RID: 2056
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000809 RID: 2057
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600080A RID: 2058
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600080B RID: 2059
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600080C RID: 2060
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600080D RID: 2061
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600080E RID: 2062
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600080F RID: 2063
			[DispId(1027)]
			void onbeforeeditfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000810 RID: 2064
			[DispId(1037)]
			void onselectionchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000811 RID: 2065
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000812 RID: 2066
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000813 RID: 2067
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000814 RID: 2068
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000815 RID: 2069
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000816 RID: 2070
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000817 RID: 2071
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000818 RID: 2072
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x02000167 RID: 359
		[Guid("332C4426-26CB-11D0-B483-00C04FD90119")]
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		internal interface IHTMLFramesCollection2
		{
			// Token: 0x06000819 RID: 2073
			object Item(ref object idOrName);

			// Token: 0x0600081A RID: 2074
			int GetLength();
		}

		// Token: 0x02000168 RID: 360
		[ComVisible(true)]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[SuppressUnmanagedCodeSecurity]
		[Guid("332C4427-26CB-11D0-B483-00C04FD90119")]
		public interface IHTMLWindow2
		{
			// Token: 0x0600081B RID: 2075
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object Item([In] ref object pvarIndex);

			// Token: 0x0600081C RID: 2076
			int GetLength();

			// Token: 0x0600081D RID: 2077
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLFramesCollection2 GetFrames();

			// Token: 0x0600081E RID: 2078
			void SetDefaultStatus([In] string p);

			// Token: 0x0600081F RID: 2079
			string GetDefaultStatus();

			// Token: 0x06000820 RID: 2080
			void SetStatus([In] string p);

			// Token: 0x06000821 RID: 2081
			string GetStatus();

			// Token: 0x06000822 RID: 2082
			int SetTimeout([In] string expression, [In] int msec, [In] ref object language);

			// Token: 0x06000823 RID: 2083
			void ClearTimeout([In] int timerID);

			// Token: 0x06000824 RID: 2084
			void Alert([In] string message);

			// Token: 0x06000825 RID: 2085
			bool Confirm([In] string message);

			// Token: 0x06000826 RID: 2086
			[return: MarshalAs(UnmanagedType.Struct)]
			object Prompt([In] string message, [In] string defstr);

			// Token: 0x06000827 RID: 2087
			object GetImage();

			// Token: 0x06000828 RID: 2088
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLLocation GetLocation();

			// Token: 0x06000829 RID: 2089
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IOmHistory GetHistory();

			// Token: 0x0600082A RID: 2090
			void Close();

			// Token: 0x0600082B RID: 2091
			void SetOpener([In] object p);

			// Token: 0x0600082C RID: 2092
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object GetOpener();

			// Token: 0x0600082D RID: 2093
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IOmNavigator GetNavigator();

			// Token: 0x0600082E RID: 2094
			void SetName([In] string p);

			// Token: 0x0600082F RID: 2095
			string GetName();

			// Token: 0x06000830 RID: 2096
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLWindow2 GetParent();

			// Token: 0x06000831 RID: 2097
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLWindow2 Open([In] string URL, [In] string name, [In] string features, [In] bool replace);

			// Token: 0x06000832 RID: 2098
			object GetSelf();

			// Token: 0x06000833 RID: 2099
			object GetTop();

			// Token: 0x06000834 RID: 2100
			object GetWindow();

			// Token: 0x06000835 RID: 2101
			void Navigate([In] string URL);

			// Token: 0x06000836 RID: 2102
			void SetOnfocus([In] object p);

			// Token: 0x06000837 RID: 2103
			object GetOnfocus();

			// Token: 0x06000838 RID: 2104
			void SetOnblur([In] object p);

			// Token: 0x06000839 RID: 2105
			object GetOnblur();

			// Token: 0x0600083A RID: 2106
			void SetOnload([In] object p);

			// Token: 0x0600083B RID: 2107
			object GetOnload();

			// Token: 0x0600083C RID: 2108
			void SetOnbeforeunload(object p);

			// Token: 0x0600083D RID: 2109
			object GetOnbeforeunload();

			// Token: 0x0600083E RID: 2110
			void SetOnunload([In] object p);

			// Token: 0x0600083F RID: 2111
			object GetOnunload();

			// Token: 0x06000840 RID: 2112
			void SetOnhelp(object p);

			// Token: 0x06000841 RID: 2113
			object GetOnhelp();

			// Token: 0x06000842 RID: 2114
			void SetOnerror([In] object p);

			// Token: 0x06000843 RID: 2115
			object GetOnerror();

			// Token: 0x06000844 RID: 2116
			void SetOnresize([In] object p);

			// Token: 0x06000845 RID: 2117
			object GetOnresize();

			// Token: 0x06000846 RID: 2118
			void SetOnscroll([In] object p);

			// Token: 0x06000847 RID: 2119
			object GetOnscroll();

			// Token: 0x06000848 RID: 2120
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLDocument2 GetDocument();

			// Token: 0x06000849 RID: 2121
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLEventObj GetEvent();

			// Token: 0x0600084A RID: 2122
			object Get_newEnum();

			// Token: 0x0600084B RID: 2123
			object ShowModalDialog([In] string dialog, [In] ref object varArgIn, [In] ref object varOptions);

			// Token: 0x0600084C RID: 2124
			void ShowHelp([In] string helpURL, [In] object helpArg, [In] string features);

			// Token: 0x0600084D RID: 2125
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLScreen GetScreen();

			// Token: 0x0600084E RID: 2126
			object GetOption();

			// Token: 0x0600084F RID: 2127
			void Focus();

			// Token: 0x06000850 RID: 2128
			bool GetClosed();

			// Token: 0x06000851 RID: 2129
			void Blur();

			// Token: 0x06000852 RID: 2130
			void Scroll([In] int x, [In] int y);

			// Token: 0x06000853 RID: 2131
			object GetClientInformation();

			// Token: 0x06000854 RID: 2132
			int SetInterval([In] string expression, [In] int msec, [In] ref object language);

			// Token: 0x06000855 RID: 2133
			void ClearInterval([In] int timerID);

			// Token: 0x06000856 RID: 2134
			void SetOffscreenBuffering([In] object p);

			// Token: 0x06000857 RID: 2135
			object GetOffscreenBuffering();

			// Token: 0x06000858 RID: 2136
			[return: MarshalAs(UnmanagedType.Struct)]
			object ExecScript([In] string code, [In] string language);

			// Token: 0x06000859 RID: 2137
			string toString();

			// Token: 0x0600085A RID: 2138
			void ScrollBy([In] int x, [In] int y);

			// Token: 0x0600085B RID: 2139
			void ScrollTo([In] int x, [In] int y);

			// Token: 0x0600085C RID: 2140
			void MoveTo([In] int x, [In] int y);

			// Token: 0x0600085D RID: 2141
			void MoveBy([In] int x, [In] int y);

			// Token: 0x0600085E RID: 2142
			void ResizeTo([In] int x, [In] int y);

			// Token: 0x0600085F RID: 2143
			void ResizeBy([In] int x, [In] int y);

			// Token: 0x06000860 RID: 2144
			object GetExternal();
		}

		// Token: 0x02000169 RID: 361
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[Guid("3050f4ae-98b5-11cf-bb82-00aa00bdce0b")]
		[ComVisible(true)]
		[SuppressUnmanagedCodeSecurity]
		public interface IHTMLWindow3
		{
			// Token: 0x06000861 RID: 2145
			int GetScreenLeft();

			// Token: 0x06000862 RID: 2146
			int GetScreenTop();

			// Token: 0x06000863 RID: 2147
			bool AttachEvent(string ev, [MarshalAs(UnmanagedType.IDispatch)] [In] object pdisp);

			// Token: 0x06000864 RID: 2148
			void DetachEvent(string ev, [MarshalAs(UnmanagedType.IDispatch)] [In] object pdisp);

			// Token: 0x06000865 RID: 2149
			int SetTimeout([In] ref object expression, int msec, [In] ref object language);

			// Token: 0x06000866 RID: 2150
			int SetInterval([In] ref object expression, int msec, [In] ref object language);

			// Token: 0x06000867 RID: 2151
			void Print();

			// Token: 0x06000868 RID: 2152
			void SetBeforePrint(object o);

			// Token: 0x06000869 RID: 2153
			object GetBeforePrint();

			// Token: 0x0600086A RID: 2154
			void SetAfterPrint(object o);

			// Token: 0x0600086B RID: 2155
			object GetAfterPrint();

			// Token: 0x0600086C RID: 2156
			object GetClipboardData();

			// Token: 0x0600086D RID: 2157
			object ShowModelessDialog(string url, object varArgIn, object options);
		}

		// Token: 0x0200016A RID: 362
		[SuppressUnmanagedCodeSecurity]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComVisible(true)]
		[Guid("3050f6cf-98b5-11cf-bb82-00aa00bdce0b")]
		public interface IHTMLWindow4
		{
			// Token: 0x0600086E RID: 2158
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object CreatePopup([In] ref object reserved);

			// Token: 0x0600086F RID: 2159
			[return: MarshalAs(UnmanagedType.Interface)]
			object frameElement();
		}

		// Token: 0x0200016B RID: 363
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[Guid("3050f625-98b5-11cf-bb82-00aa00bdce0b")]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLWindowEvents2
		{
			// Token: 0x06000870 RID: 2160
			[DispId(1003)]
			void onload(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000871 RID: 2161
			[DispId(1008)]
			void onunload(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000872 RID: 2162
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000873 RID: 2163
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000874 RID: 2164
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000875 RID: 2165
			[DispId(1002)]
			bool onerror(string description, string url, int line);

			// Token: 0x06000876 RID: 2166
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000877 RID: 2167
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000878 RID: 2168
			[DispId(1017)]
			void onbeforeunload(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000879 RID: 2169
			[DispId(1024)]
			void onbeforeprint(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x0600087A RID: 2170
			[DispId(1025)]
			void onafterprint(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x0200016C RID: 364
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComVisible(true)]
		[Guid("3050f666-98b5-11cf-bb82-00aa00bdce0b")]
		[SuppressUnmanagedCodeSecurity]
		public interface IHTMLPopup
		{
			// Token: 0x0600087B RID: 2171
			void show(int x, int y, int w, int h, ref object element);

			// Token: 0x0600087C RID: 2172
			void hide();

			// Token: 0x0600087D RID: 2173
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLDocument GetDocument();

			// Token: 0x0600087E RID: 2174
			bool IsOpen();
		}

		// Token: 0x0200016D RID: 365
		[ComVisible(true)]
		[Guid("3050f35c-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[SuppressUnmanagedCodeSecurity]
		public interface IHTMLScreen
		{
			// Token: 0x0600087F RID: 2175
			int GetColorDepth();

			// Token: 0x06000880 RID: 2176
			void SetBufferDepth(int d);

			// Token: 0x06000881 RID: 2177
			int GetBufferDepth();

			// Token: 0x06000882 RID: 2178
			int GetWidth();

			// Token: 0x06000883 RID: 2179
			int GetHeight();

			// Token: 0x06000884 RID: 2180
			void SetUpdateInterval(int i);

			// Token: 0x06000885 RID: 2181
			int GetUpdateInterval();

			// Token: 0x06000886 RID: 2182
			int GetAvailHeight();

			// Token: 0x06000887 RID: 2183
			int GetAvailWidth();

			// Token: 0x06000888 RID: 2184
			bool GetFontSmoothingEnabled();
		}

		// Token: 0x0200016E RID: 366
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComVisible(true)]
		[SuppressUnmanagedCodeSecurity]
		[Guid("163BB1E0-6E00-11CF-837A-48DC04C10000")]
		internal interface IHTMLLocation
		{
			// Token: 0x06000889 RID: 2185
			void SetHref([In] string p);

			// Token: 0x0600088A RID: 2186
			string GetHref();

			// Token: 0x0600088B RID: 2187
			void SetProtocol([In] string p);

			// Token: 0x0600088C RID: 2188
			string GetProtocol();

			// Token: 0x0600088D RID: 2189
			void SetHost([In] string p);

			// Token: 0x0600088E RID: 2190
			string GetHost();

			// Token: 0x0600088F RID: 2191
			void SetHostname([In] string p);

			// Token: 0x06000890 RID: 2192
			string GetHostname();

			// Token: 0x06000891 RID: 2193
			void SetPort([In] string p);

			// Token: 0x06000892 RID: 2194
			string GetPort();

			// Token: 0x06000893 RID: 2195
			void SetPathname([In] string p);

			// Token: 0x06000894 RID: 2196
			string GetPathname();

			// Token: 0x06000895 RID: 2197
			void SetSearch([In] string p);

			// Token: 0x06000896 RID: 2198
			string GetSearch();

			// Token: 0x06000897 RID: 2199
			void SetHash([In] string p);

			// Token: 0x06000898 RID: 2200
			string GetHash();

			// Token: 0x06000899 RID: 2201
			void Reload([In] bool flag);

			// Token: 0x0600089A RID: 2202
			void Replace([In] string bstr);

			// Token: 0x0600089B RID: 2203
			void Assign([In] string bstr);
		}

		// Token: 0x0200016F RID: 367
		[ComVisible(true)]
		[Guid("FECEAAA2-8405-11CF-8BA1-00AA00476DA6")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[SuppressUnmanagedCodeSecurity]
		internal interface IOmHistory
		{
			// Token: 0x0600089C RID: 2204
			short GetLength();

			// Token: 0x0600089D RID: 2205
			void Back();

			// Token: 0x0600089E RID: 2206
			void Forward();

			// Token: 0x0600089F RID: 2207
			void Go([In] ref object pvargdistance);
		}

		// Token: 0x02000170 RID: 368
		[ComVisible(true)]
		[Guid("FECEAAA5-8405-11CF-8BA1-00AA00476DA6")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[SuppressUnmanagedCodeSecurity]
		internal interface IOmNavigator
		{
			// Token: 0x060008A0 RID: 2208
			string GetAppCodeName();

			// Token: 0x060008A1 RID: 2209
			string GetAppName();

			// Token: 0x060008A2 RID: 2210
			string GetAppVersion();

			// Token: 0x060008A3 RID: 2211
			string GetUserAgent();

			// Token: 0x060008A4 RID: 2212
			bool JavaEnabled();

			// Token: 0x060008A5 RID: 2213
			bool TaintEnabled();

			// Token: 0x060008A6 RID: 2214
			object GetMimeTypes();

			// Token: 0x060008A7 RID: 2215
			object GetPlugins();

			// Token: 0x060008A8 RID: 2216
			bool GetCookieEnabled();

			// Token: 0x060008A9 RID: 2217
			object GetOpsProfile();

			// Token: 0x060008AA RID: 2218
			string GetCpuClass();

			// Token: 0x060008AB RID: 2219
			string GetSystemLanguage();

			// Token: 0x060008AC RID: 2220
			string GetBrowserLanguage();

			// Token: 0x060008AD RID: 2221
			string GetUserLanguage();

			// Token: 0x060008AE RID: 2222
			string GetPlatform();

			// Token: 0x060008AF RID: 2223
			string GetAppMinorVersion();

			// Token: 0x060008B0 RID: 2224
			int GetConnectionSpeed();

			// Token: 0x060008B1 RID: 2225
			bool GetOnLine();

			// Token: 0x060008B2 RID: 2226
			object GetUserProfile();
		}

		// Token: 0x02000171 RID: 369
		[SuppressUnmanagedCodeSecurity]
		[Guid("3050F32D-98B5-11CF-BB82-00AA00BDCE0B")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComVisible(true)]
		internal interface IHTMLEventObj
		{
			// Token: 0x060008B3 RID: 2227
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLElement GetSrcElement();

			// Token: 0x060008B4 RID: 2228
			bool GetAltKey();

			// Token: 0x060008B5 RID: 2229
			bool GetCtrlKey();

			// Token: 0x060008B6 RID: 2230
			bool GetShiftKey();

			// Token: 0x060008B7 RID: 2231
			void SetReturnValue(object p);

			// Token: 0x060008B8 RID: 2232
			object GetReturnValue();

			// Token: 0x060008B9 RID: 2233
			void SetCancelBubble(bool p);

			// Token: 0x060008BA RID: 2234
			bool GetCancelBubble();

			// Token: 0x060008BB RID: 2235
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLElement GetFromElement();

			// Token: 0x060008BC RID: 2236
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLElement GetToElement();

			// Token: 0x060008BD RID: 2237
			void SetKeyCode([In] int p);

			// Token: 0x060008BE RID: 2238
			int GetKeyCode();

			// Token: 0x060008BF RID: 2239
			int GetButton();

			// Token: 0x060008C0 RID: 2240
			string GetEventType();

			// Token: 0x060008C1 RID: 2241
			string GetQualifier();

			// Token: 0x060008C2 RID: 2242
			int GetReason();

			// Token: 0x060008C3 RID: 2243
			int GetX();

			// Token: 0x060008C4 RID: 2244
			int GetY();

			// Token: 0x060008C5 RID: 2245
			int GetClientX();

			// Token: 0x060008C6 RID: 2246
			int GetClientY();

			// Token: 0x060008C7 RID: 2247
			int GetOffsetX();

			// Token: 0x060008C8 RID: 2248
			int GetOffsetY();

			// Token: 0x060008C9 RID: 2249
			int GetScreenX();

			// Token: 0x060008CA RID: 2250
			int GetScreenY();

			// Token: 0x060008CB RID: 2251
			object GetSrcFilter();
		}

		// Token: 0x02000172 RID: 370
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[Guid("3050f48B-98b5-11cf-bb82-00aa00bdce0b")]
		internal interface IHTMLEventObj2
		{
			// Token: 0x060008CC RID: 2252
			void SetAttribute(string attributeName, object attributeValue, int lFlags);

			// Token: 0x060008CD RID: 2253
			object GetAttribute(string attributeName, int lFlags);

			// Token: 0x060008CE RID: 2254
			bool RemoveAttribute(string attributeName, int lFlags);

			// Token: 0x060008CF RID: 2255
			void SetPropertyName(string name);

			// Token: 0x060008D0 RID: 2256
			string GetPropertyName();

			// Token: 0x060008D1 RID: 2257
			void SetBookmarks(ref object bm);

			// Token: 0x060008D2 RID: 2258
			object GetBookmarks();

			// Token: 0x060008D3 RID: 2259
			void SetRecordset(ref object rs);

			// Token: 0x060008D4 RID: 2260
			object GetRecordset();

			// Token: 0x060008D5 RID: 2261
			void SetDataFld(string df);

			// Token: 0x060008D6 RID: 2262
			string GetDataFld();

			// Token: 0x060008D7 RID: 2263
			void SetBoundElements(ref object be);

			// Token: 0x060008D8 RID: 2264
			object GetBoundElements();

			// Token: 0x060008D9 RID: 2265
			void SetRepeat(bool r);

			// Token: 0x060008DA RID: 2266
			bool GetRepeat();

			// Token: 0x060008DB RID: 2267
			void SetSrcUrn(string urn);

			// Token: 0x060008DC RID: 2268
			string GetSrcUrn();

			// Token: 0x060008DD RID: 2269
			void SetSrcElement(ref object se);

			// Token: 0x060008DE RID: 2270
			object GetSrcElement();

			// Token: 0x060008DF RID: 2271
			void SetAltKey(bool alt);

			// Token: 0x060008E0 RID: 2272
			bool GetAltKey();

			// Token: 0x060008E1 RID: 2273
			void SetCtrlKey(bool ctrl);

			// Token: 0x060008E2 RID: 2274
			bool GetCtrlKey();

			// Token: 0x060008E3 RID: 2275
			void SetShiftKey(bool shift);

			// Token: 0x060008E4 RID: 2276
			bool GetShiftKey();

			// Token: 0x060008E5 RID: 2277
			void SetFromElement(ref object element);

			// Token: 0x060008E6 RID: 2278
			object GetFromElement();

			// Token: 0x060008E7 RID: 2279
			void SetToElement(ref object element);

			// Token: 0x060008E8 RID: 2280
			object GetToElement();

			// Token: 0x060008E9 RID: 2281
			void SetButton(int b);

			// Token: 0x060008EA RID: 2282
			int GetButton();

			// Token: 0x060008EB RID: 2283
			void SetType(string type);

			// Token: 0x060008EC RID: 2284
			string GetType();

			// Token: 0x060008ED RID: 2285
			void SetQualifier(string q);

			// Token: 0x060008EE RID: 2286
			string GetQualifier();

			// Token: 0x060008EF RID: 2287
			void SetReason(int r);

			// Token: 0x060008F0 RID: 2288
			int GetReason();

			// Token: 0x060008F1 RID: 2289
			void SetX(int x);

			// Token: 0x060008F2 RID: 2290
			int GetX();

			// Token: 0x060008F3 RID: 2291
			void SetY(int y);

			// Token: 0x060008F4 RID: 2292
			int GetY();

			// Token: 0x060008F5 RID: 2293
			void SetClientX(int x);

			// Token: 0x060008F6 RID: 2294
			int GetClientX();

			// Token: 0x060008F7 RID: 2295
			void SetClientY(int y);

			// Token: 0x060008F8 RID: 2296
			int GetClientY();

			// Token: 0x060008F9 RID: 2297
			void SetOffsetX(int x);

			// Token: 0x060008FA RID: 2298
			int GetOffsetX();

			// Token: 0x060008FB RID: 2299
			void SetOffsetY(int y);

			// Token: 0x060008FC RID: 2300
			int GetOffsetY();

			// Token: 0x060008FD RID: 2301
			void SetScreenX(int x);

			// Token: 0x060008FE RID: 2302
			int GetScreenX();

			// Token: 0x060008FF RID: 2303
			void SetScreenY(int y);

			// Token: 0x06000900 RID: 2304
			int GetScreenY();

			// Token: 0x06000901 RID: 2305
			void SetSrcFilter(ref object filter);

			// Token: 0x06000902 RID: 2306
			object GetSrcFilter();

			// Token: 0x06000903 RID: 2307
			object GetDataTransfer();
		}

		// Token: 0x02000173 RID: 371
		[SuppressUnmanagedCodeSecurity]
		[Guid("3050f814-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComVisible(true)]
		internal interface IHTMLEventObj4
		{
			// Token: 0x06000904 RID: 2308
			int GetWheelDelta();
		}

		// Token: 0x02000174 RID: 372
		[SuppressUnmanagedCodeSecurity]
		[Guid("3050F21F-98B5-11CF-BB82-00AA00BDCE0B")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComVisible(true)]
		internal interface IHTMLElementCollection
		{
			// Token: 0x06000905 RID: 2309
			string toString();

			// Token: 0x06000906 RID: 2310
			void SetLength(int p);

			// Token: 0x06000907 RID: 2311
			int GetLength();

			// Token: 0x06000908 RID: 2312
			[return: MarshalAs(UnmanagedType.Interface)]
			object Get_newEnum();

			// Token: 0x06000909 RID: 2313
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object Item(object idOrName, object index);

			// Token: 0x0600090A RID: 2314
			[return: MarshalAs(UnmanagedType.Interface)]
			object Tags(object tagName);
		}

		// Token: 0x02000175 RID: 373
		[ComVisible(true)]
		[Guid("3050F1FF-98B5-11CF-BB82-00AA00BDCE0B")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[SuppressUnmanagedCodeSecurity]
		internal interface IHTMLElement
		{
			// Token: 0x0600090B RID: 2315
			void SetAttribute(string attributeName, object attributeValue, int lFlags);

			// Token: 0x0600090C RID: 2316
			object GetAttribute(string attributeName, int lFlags);

			// Token: 0x0600090D RID: 2317
			bool RemoveAttribute(string strAttributeName, int lFlags);

			// Token: 0x0600090E RID: 2318
			void SetClassName(string p);

			// Token: 0x0600090F RID: 2319
			string GetClassName();

			// Token: 0x06000910 RID: 2320
			void SetId(string p);

			// Token: 0x06000911 RID: 2321
			string GetId();

			// Token: 0x06000912 RID: 2322
			string GetTagName();

			// Token: 0x06000913 RID: 2323
			UnsafeNativeMethods.IHTMLElement GetParentElement();

			// Token: 0x06000914 RID: 2324
			UnsafeNativeMethods.IHTMLStyle GetStyle();

			// Token: 0x06000915 RID: 2325
			void SetOnhelp(object p);

			// Token: 0x06000916 RID: 2326
			object GetOnhelp();

			// Token: 0x06000917 RID: 2327
			void SetOnclick(object p);

			// Token: 0x06000918 RID: 2328
			object GetOnclick();

			// Token: 0x06000919 RID: 2329
			void SetOndblclick(object p);

			// Token: 0x0600091A RID: 2330
			object GetOndblclick();

			// Token: 0x0600091B RID: 2331
			void SetOnkeydown(object p);

			// Token: 0x0600091C RID: 2332
			object GetOnkeydown();

			// Token: 0x0600091D RID: 2333
			void SetOnkeyup(object p);

			// Token: 0x0600091E RID: 2334
			object GetOnkeyup();

			// Token: 0x0600091F RID: 2335
			void SetOnkeypress(object p);

			// Token: 0x06000920 RID: 2336
			object GetOnkeypress();

			// Token: 0x06000921 RID: 2337
			void SetOnmouseout(object p);

			// Token: 0x06000922 RID: 2338
			object GetOnmouseout();

			// Token: 0x06000923 RID: 2339
			void SetOnmouseover(object p);

			// Token: 0x06000924 RID: 2340
			object GetOnmouseover();

			// Token: 0x06000925 RID: 2341
			void SetOnmousemove(object p);

			// Token: 0x06000926 RID: 2342
			object GetOnmousemove();

			// Token: 0x06000927 RID: 2343
			void SetOnmousedown(object p);

			// Token: 0x06000928 RID: 2344
			object GetOnmousedown();

			// Token: 0x06000929 RID: 2345
			void SetOnmouseup(object p);

			// Token: 0x0600092A RID: 2346
			object GetOnmouseup();

			// Token: 0x0600092B RID: 2347
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLDocument2 GetDocument();

			// Token: 0x0600092C RID: 2348
			void SetTitle(string p);

			// Token: 0x0600092D RID: 2349
			string GetTitle();

			// Token: 0x0600092E RID: 2350
			void SetLanguage(string p);

			// Token: 0x0600092F RID: 2351
			string GetLanguage();

			// Token: 0x06000930 RID: 2352
			void SetOnselectstart(object p);

			// Token: 0x06000931 RID: 2353
			object GetOnselectstart();

			// Token: 0x06000932 RID: 2354
			void ScrollIntoView(object varargStart);

			// Token: 0x06000933 RID: 2355
			bool Contains(UnsafeNativeMethods.IHTMLElement pChild);

			// Token: 0x06000934 RID: 2356
			int GetSourceIndex();

			// Token: 0x06000935 RID: 2357
			object GetRecordNumber();

			// Token: 0x06000936 RID: 2358
			void SetLang(string p);

			// Token: 0x06000937 RID: 2359
			string GetLang();

			// Token: 0x06000938 RID: 2360
			int GetOffsetLeft();

			// Token: 0x06000939 RID: 2361
			int GetOffsetTop();

			// Token: 0x0600093A RID: 2362
			int GetOffsetWidth();

			// Token: 0x0600093B RID: 2363
			int GetOffsetHeight();

			// Token: 0x0600093C RID: 2364
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLElement GetOffsetParent();

			// Token: 0x0600093D RID: 2365
			void SetInnerHTML(string p);

			// Token: 0x0600093E RID: 2366
			string GetInnerHTML();

			// Token: 0x0600093F RID: 2367
			void SetInnerText(string p);

			// Token: 0x06000940 RID: 2368
			string GetInnerText();

			// Token: 0x06000941 RID: 2369
			void SetOuterHTML(string p);

			// Token: 0x06000942 RID: 2370
			string GetOuterHTML();

			// Token: 0x06000943 RID: 2371
			void SetOuterText(string p);

			// Token: 0x06000944 RID: 2372
			string GetOuterText();

			// Token: 0x06000945 RID: 2373
			void InsertAdjacentHTML(string where, string html);

			// Token: 0x06000946 RID: 2374
			void InsertAdjacentText(string where, string text);

			// Token: 0x06000947 RID: 2375
			UnsafeNativeMethods.IHTMLElement GetParentTextEdit();

			// Token: 0x06000948 RID: 2376
			bool GetIsTextEdit();

			// Token: 0x06000949 RID: 2377
			void Click();

			// Token: 0x0600094A RID: 2378
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetFilters();

			// Token: 0x0600094B RID: 2379
			void SetOndragstart(object p);

			// Token: 0x0600094C RID: 2380
			object GetOndragstart();

			// Token: 0x0600094D RID: 2381
			string toString();

			// Token: 0x0600094E RID: 2382
			void SetOnbeforeupdate(object p);

			// Token: 0x0600094F RID: 2383
			object GetOnbeforeupdate();

			// Token: 0x06000950 RID: 2384
			void SetOnafterupdate(object p);

			// Token: 0x06000951 RID: 2385
			object GetOnafterupdate();

			// Token: 0x06000952 RID: 2386
			void SetOnerrorupdate(object p);

			// Token: 0x06000953 RID: 2387
			object GetOnerrorupdate();

			// Token: 0x06000954 RID: 2388
			void SetOnrowexit(object p);

			// Token: 0x06000955 RID: 2389
			object GetOnrowexit();

			// Token: 0x06000956 RID: 2390
			void SetOnrowenter(object p);

			// Token: 0x06000957 RID: 2391
			object GetOnrowenter();

			// Token: 0x06000958 RID: 2392
			void SetOndatasetchanged(object p);

			// Token: 0x06000959 RID: 2393
			object GetOndatasetchanged();

			// Token: 0x0600095A RID: 2394
			void SetOndataavailable(object p);

			// Token: 0x0600095B RID: 2395
			object GetOndataavailable();

			// Token: 0x0600095C RID: 2396
			void SetOndatasetcomplete(object p);

			// Token: 0x0600095D RID: 2397
			object GetOndatasetcomplete();

			// Token: 0x0600095E RID: 2398
			void SetOnfilterchange(object p);

			// Token: 0x0600095F RID: 2399
			object GetOnfilterchange();

			// Token: 0x06000960 RID: 2400
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object GetChildren();

			// Token: 0x06000961 RID: 2401
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object GetAll();
		}

		// Token: 0x02000176 RID: 374
		[Guid("3050f434-98b5-11cf-bb82-00aa00bdce0b")]
		[ComVisible(true)]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[SuppressUnmanagedCodeSecurity]
		internal interface IHTMLElement2
		{
			// Token: 0x06000962 RID: 2402
			string ScopeName();

			// Token: 0x06000963 RID: 2403
			void SetCapture(bool containerCapture);

			// Token: 0x06000964 RID: 2404
			void ReleaseCapture();

			// Token: 0x06000965 RID: 2405
			void SetOnLoseCapture(object v);

			// Token: 0x06000966 RID: 2406
			object GetOnLoseCapture();

			// Token: 0x06000967 RID: 2407
			string GetComponentFromPoint(int x, int y);

			// Token: 0x06000968 RID: 2408
			void DoScroll(object component);

			// Token: 0x06000969 RID: 2409
			void SetOnScroll(object v);

			// Token: 0x0600096A RID: 2410
			object GetOnScroll();

			// Token: 0x0600096B RID: 2411
			void SetOnDrag(object v);

			// Token: 0x0600096C RID: 2412
			object GetOnDrag();

			// Token: 0x0600096D RID: 2413
			void SetOnDragEnd(object v);

			// Token: 0x0600096E RID: 2414
			object GetOnDragEnd();

			// Token: 0x0600096F RID: 2415
			void SetOnDragEnter(object v);

			// Token: 0x06000970 RID: 2416
			object GetOnDragEnter();

			// Token: 0x06000971 RID: 2417
			void SetOnDragOver(object v);

			// Token: 0x06000972 RID: 2418
			object GetOnDragOver();

			// Token: 0x06000973 RID: 2419
			void SetOnDragleave(object v);

			// Token: 0x06000974 RID: 2420
			object GetOnDragLeave();

			// Token: 0x06000975 RID: 2421
			void SetOnDrop(object v);

			// Token: 0x06000976 RID: 2422
			object GetOnDrop();

			// Token: 0x06000977 RID: 2423
			void SetOnBeforeCut(object v);

			// Token: 0x06000978 RID: 2424
			object GetOnBeforeCut();

			// Token: 0x06000979 RID: 2425
			void SetOnCut(object v);

			// Token: 0x0600097A RID: 2426
			object GetOnCut();

			// Token: 0x0600097B RID: 2427
			void SetOnBeforeCopy(object v);

			// Token: 0x0600097C RID: 2428
			object GetOnBeforeCopy();

			// Token: 0x0600097D RID: 2429
			void SetOnCopy(object v);

			// Token: 0x0600097E RID: 2430
			object GetOnCopy(object p);

			// Token: 0x0600097F RID: 2431
			void SetOnBeforePaste(object v);

			// Token: 0x06000980 RID: 2432
			object GetOnBeforePaste(object p);

			// Token: 0x06000981 RID: 2433
			void SetOnPaste(object v);

			// Token: 0x06000982 RID: 2434
			object GetOnPaste(object p);

			// Token: 0x06000983 RID: 2435
			object GetCurrentStyle();

			// Token: 0x06000984 RID: 2436
			void SetOnPropertyChange(object v);

			// Token: 0x06000985 RID: 2437
			object GetOnPropertyChange(object p);

			// Token: 0x06000986 RID: 2438
			object GetClientRects();

			// Token: 0x06000987 RID: 2439
			object GetBoundingClientRect();

			// Token: 0x06000988 RID: 2440
			void SetExpression(string propName, string expression, string language);

			// Token: 0x06000989 RID: 2441
			object GetExpression(string propName);

			// Token: 0x0600098A RID: 2442
			bool RemoveExpression(string propName);

			// Token: 0x0600098B RID: 2443
			void SetTabIndex(int v);

			// Token: 0x0600098C RID: 2444
			short GetTabIndex();

			// Token: 0x0600098D RID: 2445
			void Focus();

			// Token: 0x0600098E RID: 2446
			void SetAccessKey(string v);

			// Token: 0x0600098F RID: 2447
			string GetAccessKey();

			// Token: 0x06000990 RID: 2448
			void SetOnBlur(object v);

			// Token: 0x06000991 RID: 2449
			object GetOnBlur();

			// Token: 0x06000992 RID: 2450
			void SetOnFocus(object v);

			// Token: 0x06000993 RID: 2451
			object GetOnFocus();

			// Token: 0x06000994 RID: 2452
			void SetOnResize(object v);

			// Token: 0x06000995 RID: 2453
			object GetOnResize();

			// Token: 0x06000996 RID: 2454
			void Blur();

			// Token: 0x06000997 RID: 2455
			void AddFilter(object pUnk);

			// Token: 0x06000998 RID: 2456
			void RemoveFilter(object pUnk);

			// Token: 0x06000999 RID: 2457
			int ClientHeight();

			// Token: 0x0600099A RID: 2458
			int ClientWidth();

			// Token: 0x0600099B RID: 2459
			int ClientTop();

			// Token: 0x0600099C RID: 2460
			int ClientLeft();

			// Token: 0x0600099D RID: 2461
			bool AttachEvent(string ev, [MarshalAs(UnmanagedType.IDispatch)] [In] object pdisp);

			// Token: 0x0600099E RID: 2462
			void DetachEvent(string ev, [MarshalAs(UnmanagedType.IDispatch)] [In] object pdisp);

			// Token: 0x0600099F RID: 2463
			object ReadyState();

			// Token: 0x060009A0 RID: 2464
			void SetOnReadyStateChange(object v);

			// Token: 0x060009A1 RID: 2465
			object GetOnReadyStateChange();

			// Token: 0x060009A2 RID: 2466
			void SetOnRowsDelete(object v);

			// Token: 0x060009A3 RID: 2467
			object GetOnRowsDelete();

			// Token: 0x060009A4 RID: 2468
			void SetOnRowsInserted(object v);

			// Token: 0x060009A5 RID: 2469
			object GetOnRowsInserted();

			// Token: 0x060009A6 RID: 2470
			void SetOnCellChange(object v);

			// Token: 0x060009A7 RID: 2471
			object GetOnCellChange();

			// Token: 0x060009A8 RID: 2472
			void SetDir(string v);

			// Token: 0x060009A9 RID: 2473
			string GetDir();

			// Token: 0x060009AA RID: 2474
			object CreateControlRange();

			// Token: 0x060009AB RID: 2475
			int GetScrollHeight();

			// Token: 0x060009AC RID: 2476
			int GetScrollWidth();

			// Token: 0x060009AD RID: 2477
			void SetScrollTop(int v);

			// Token: 0x060009AE RID: 2478
			int GetScrollTop();

			// Token: 0x060009AF RID: 2479
			void SetScrollLeft(int v);

			// Token: 0x060009B0 RID: 2480
			int GetScrollLeft();

			// Token: 0x060009B1 RID: 2481
			void ClearAttributes();

			// Token: 0x060009B2 RID: 2482
			void MergeAttributes(object mergeThis);

			// Token: 0x060009B3 RID: 2483
			void SetOnContextMenu(object v);

			// Token: 0x060009B4 RID: 2484
			object GetOnContextMenu();

			// Token: 0x060009B5 RID: 2485
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLElement InsertAdjacentElement(string where, [MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IHTMLElement insertedElement);

			// Token: 0x060009B6 RID: 2486
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLElement applyElement([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IHTMLElement apply, string where);

			// Token: 0x060009B7 RID: 2487
			string GetAdjacentText(string where);

			// Token: 0x060009B8 RID: 2488
			string ReplaceAdjacentText(string where, string newText);

			// Token: 0x060009B9 RID: 2489
			bool CanHaveChildren();

			// Token: 0x060009BA RID: 2490
			int AddBehavior(string url, ref object oFactory);

			// Token: 0x060009BB RID: 2491
			bool RemoveBehavior(int cookie);

			// Token: 0x060009BC RID: 2492
			object GetRuntimeStyle();

			// Token: 0x060009BD RID: 2493
			object GetBehaviorUrns();

			// Token: 0x060009BE RID: 2494
			void SetTagUrn(string v);

			// Token: 0x060009BF RID: 2495
			string GetTagUrn();

			// Token: 0x060009C0 RID: 2496
			void SetOnBeforeEditFocus(object v);

			// Token: 0x060009C1 RID: 2497
			object GetOnBeforeEditFocus();

			// Token: 0x060009C2 RID: 2498
			int GetReadyStateValue();

			// Token: 0x060009C3 RID: 2499
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IHTMLElementCollection GetElementsByTagName(string v);
		}

		// Token: 0x02000177 RID: 375
		[ComVisible(true)]
		[Guid("3050f673-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[SuppressUnmanagedCodeSecurity]
		internal interface IHTMLElement3
		{
			// Token: 0x060009C4 RID: 2500
			void MergeAttributes(object mergeThis, object pvarFlags);

			// Token: 0x060009C5 RID: 2501
			bool IsMultiLine();

			// Token: 0x060009C6 RID: 2502
			bool CanHaveHTML();

			// Token: 0x060009C7 RID: 2503
			void SetOnLayoutComplete(object v);

			// Token: 0x060009C8 RID: 2504
			object GetOnLayoutComplete();

			// Token: 0x060009C9 RID: 2505
			void SetOnPage(object v);

			// Token: 0x060009CA RID: 2506
			object GetOnPage();

			// Token: 0x060009CB RID: 2507
			void SetInflateBlock(bool v);

			// Token: 0x060009CC RID: 2508
			bool GetInflateBlock();

			// Token: 0x060009CD RID: 2509
			void SetOnBeforeDeactivate(object v);

			// Token: 0x060009CE RID: 2510
			object GetOnBeforeDeactivate();

			// Token: 0x060009CF RID: 2511
			void SetActive();

			// Token: 0x060009D0 RID: 2512
			void SetContentEditable(string v);

			// Token: 0x060009D1 RID: 2513
			string GetContentEditable();

			// Token: 0x060009D2 RID: 2514
			bool IsContentEditable();

			// Token: 0x060009D3 RID: 2515
			void SetHideFocus(bool v);

			// Token: 0x060009D4 RID: 2516
			bool GetHideFocus();

			// Token: 0x060009D5 RID: 2517
			void SetDisabled(bool v);

			// Token: 0x060009D6 RID: 2518
			bool GetDisabled();

			// Token: 0x060009D7 RID: 2519
			bool IsDisabled();

			// Token: 0x060009D8 RID: 2520
			void SetOnMove(object v);

			// Token: 0x060009D9 RID: 2521
			object GetOnMove();

			// Token: 0x060009DA RID: 2522
			void SetOnControlSelect(object v);

			// Token: 0x060009DB RID: 2523
			object GetOnControlSelect();

			// Token: 0x060009DC RID: 2524
			bool FireEvent(string bstrEventName, object pvarEventObject);

			// Token: 0x060009DD RID: 2525
			void SetOnResizeStart(object v);

			// Token: 0x060009DE RID: 2526
			object GetOnResizeStart();

			// Token: 0x060009DF RID: 2527
			void SetOnResizeEnd(object v);

			// Token: 0x060009E0 RID: 2528
			object GetOnResizeEnd();

			// Token: 0x060009E1 RID: 2529
			void SetOnMoveStart(object v);

			// Token: 0x060009E2 RID: 2530
			object GetOnMoveStart();

			// Token: 0x060009E3 RID: 2531
			void SetOnMoveEnd(object v);

			// Token: 0x060009E4 RID: 2532
			object GetOnMoveEnd();

			// Token: 0x060009E5 RID: 2533
			void SetOnMouseEnter(object v);

			// Token: 0x060009E6 RID: 2534
			object GetOnMouseEnter();

			// Token: 0x060009E7 RID: 2535
			void SetOnMouseLeave(object v);

			// Token: 0x060009E8 RID: 2536
			object GetOnMouseLeave();

			// Token: 0x060009E9 RID: 2537
			void SetOnActivate(object v);

			// Token: 0x060009EA RID: 2538
			object GetOnActivate();

			// Token: 0x060009EB RID: 2539
			void SetOnDeactivate(object v);

			// Token: 0x060009EC RID: 2540
			object GetOnDeactivate();

			// Token: 0x060009ED RID: 2541
			bool DragDrop();

			// Token: 0x060009EE RID: 2542
			int GlyphMode();
		}

		// Token: 0x02000178 RID: 376
		[SuppressUnmanagedCodeSecurity]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[Guid("3050f5da-98b5-11cf-bb82-00aa00bdce0b")]
		[ComVisible(true)]
		public interface IHTMLDOMNode
		{
			// Token: 0x060009EF RID: 2543
			long GetNodeType();

			// Token: 0x060009F0 RID: 2544
			UnsafeNativeMethods.IHTMLDOMNode GetParentNode();

			// Token: 0x060009F1 RID: 2545
			bool HasChildNodes();

			// Token: 0x060009F2 RID: 2546
			object GetChildNodes();

			// Token: 0x060009F3 RID: 2547
			object GetAttributes();

			// Token: 0x060009F4 RID: 2548
			UnsafeNativeMethods.IHTMLDOMNode InsertBefore(UnsafeNativeMethods.IHTMLDOMNode newChild, object refChild);

			// Token: 0x060009F5 RID: 2549
			UnsafeNativeMethods.IHTMLDOMNode RemoveChild(UnsafeNativeMethods.IHTMLDOMNode oldChild);

			// Token: 0x060009F6 RID: 2550
			UnsafeNativeMethods.IHTMLDOMNode ReplaceChild(UnsafeNativeMethods.IHTMLDOMNode newChild, UnsafeNativeMethods.IHTMLDOMNode oldChild);

			// Token: 0x060009F7 RID: 2551
			UnsafeNativeMethods.IHTMLDOMNode CloneNode(bool fDeep);

			// Token: 0x060009F8 RID: 2552
			UnsafeNativeMethods.IHTMLDOMNode RemoveNode(bool fDeep);

			// Token: 0x060009F9 RID: 2553
			UnsafeNativeMethods.IHTMLDOMNode SwapNode(UnsafeNativeMethods.IHTMLDOMNode otherNode);

			// Token: 0x060009FA RID: 2554
			UnsafeNativeMethods.IHTMLDOMNode ReplaceNode(UnsafeNativeMethods.IHTMLDOMNode replacement);

			// Token: 0x060009FB RID: 2555
			UnsafeNativeMethods.IHTMLDOMNode AppendChild(UnsafeNativeMethods.IHTMLDOMNode newChild);

			// Token: 0x060009FC RID: 2556
			string NodeName();

			// Token: 0x060009FD RID: 2557
			void SetNodeValue(object v);

			// Token: 0x060009FE RID: 2558
			object GetNodeValue();

			// Token: 0x060009FF RID: 2559
			UnsafeNativeMethods.IHTMLDOMNode FirstChild();

			// Token: 0x06000A00 RID: 2560
			UnsafeNativeMethods.IHTMLDOMNode LastChild();

			// Token: 0x06000A01 RID: 2561
			UnsafeNativeMethods.IHTMLDOMNode PreviousSibling();

			// Token: 0x06000A02 RID: 2562
			UnsafeNativeMethods.IHTMLDOMNode NextSibling();
		}

		// Token: 0x02000179 RID: 377
		[Guid("3050f60f-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLElementEvents2
		{
			// Token: 0x06000A03 RID: 2563
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A04 RID: 2564
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A05 RID: 2565
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A06 RID: 2566
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A07 RID: 2567
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A08 RID: 2568
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A09 RID: 2569
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A0A RID: 2570
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A0B RID: 2571
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A0C RID: 2572
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A0D RID: 2573
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A0E RID: 2574
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A0F RID: 2575
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A10 RID: 2576
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A11 RID: 2577
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A12 RID: 2578
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A13 RID: 2579
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A14 RID: 2580
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A15 RID: 2581
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A16 RID: 2582
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A17 RID: 2583
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A18 RID: 2584
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A19 RID: 2585
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A1A RID: 2586
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A1B RID: 2587
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A1C RID: 2588
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A1D RID: 2589
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A1E RID: 2590
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A1F RID: 2591
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A20 RID: 2592
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A21 RID: 2593
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A22 RID: 2594
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A23 RID: 2595
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A24 RID: 2596
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A25 RID: 2597
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A26 RID: 2598
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A27 RID: 2599
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A28 RID: 2600
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A29 RID: 2601
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A2A RID: 2602
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A2B RID: 2603
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A2C RID: 2604
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A2D RID: 2605
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A2E RID: 2606
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A2F RID: 2607
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A30 RID: 2608
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A31 RID: 2609
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A32 RID: 2610
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A33 RID: 2611
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A34 RID: 2612
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A35 RID: 2613
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A36 RID: 2614
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A37 RID: 2615
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A38 RID: 2616
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A39 RID: 2617
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A3A RID: 2618
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A3B RID: 2619
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A3C RID: 2620
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A3D RID: 2621
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A3E RID: 2622
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A3F RID: 2623
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A40 RID: 2624
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x0200017A RID: 378
		[Guid("3050f610-98b5-11cf-bb82-00aa00bdce0b")]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[ComImport]
		public interface DHTMLAnchorEvents2
		{
			// Token: 0x06000A41 RID: 2625
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A42 RID: 2626
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A43 RID: 2627
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A44 RID: 2628
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A45 RID: 2629
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A46 RID: 2630
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A47 RID: 2631
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A48 RID: 2632
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A49 RID: 2633
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A4A RID: 2634
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A4B RID: 2635
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A4C RID: 2636
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A4D RID: 2637
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A4E RID: 2638
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A4F RID: 2639
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A50 RID: 2640
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A51 RID: 2641
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A52 RID: 2642
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A53 RID: 2643
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A54 RID: 2644
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A55 RID: 2645
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A56 RID: 2646
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A57 RID: 2647
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A58 RID: 2648
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A59 RID: 2649
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A5A RID: 2650
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A5B RID: 2651
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A5C RID: 2652
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A5D RID: 2653
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A5E RID: 2654
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A5F RID: 2655
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A60 RID: 2656
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A61 RID: 2657
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A62 RID: 2658
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A63 RID: 2659
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A64 RID: 2660
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A65 RID: 2661
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A66 RID: 2662
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A67 RID: 2663
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A68 RID: 2664
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A69 RID: 2665
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A6A RID: 2666
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A6B RID: 2667
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A6C RID: 2668
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A6D RID: 2669
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A6E RID: 2670
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A6F RID: 2671
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A70 RID: 2672
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A71 RID: 2673
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A72 RID: 2674
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A73 RID: 2675
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A74 RID: 2676
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A75 RID: 2677
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A76 RID: 2678
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A77 RID: 2679
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A78 RID: 2680
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A79 RID: 2681
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A7A RID: 2682
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A7B RID: 2683
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A7C RID: 2684
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A7D RID: 2685
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A7E RID: 2686
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x0200017B RID: 379
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[Guid("3050f611-98b5-11cf-bb82-00aa00bdce0b")]
		[ComImport]
		public interface DHTMLAreaEvents2
		{
			// Token: 0x06000A7F RID: 2687
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A80 RID: 2688
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A81 RID: 2689
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A82 RID: 2690
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A83 RID: 2691
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A84 RID: 2692
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A85 RID: 2693
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A86 RID: 2694
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A87 RID: 2695
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A88 RID: 2696
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A89 RID: 2697
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A8A RID: 2698
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A8B RID: 2699
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A8C RID: 2700
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A8D RID: 2701
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A8E RID: 2702
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A8F RID: 2703
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A90 RID: 2704
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A91 RID: 2705
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A92 RID: 2706
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A93 RID: 2707
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A94 RID: 2708
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A95 RID: 2709
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A96 RID: 2710
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A97 RID: 2711
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A98 RID: 2712
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A99 RID: 2713
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A9A RID: 2714
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A9B RID: 2715
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A9C RID: 2716
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A9D RID: 2717
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A9E RID: 2718
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000A9F RID: 2719
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AA0 RID: 2720
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AA1 RID: 2721
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AA2 RID: 2722
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AA3 RID: 2723
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AA4 RID: 2724
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AA5 RID: 2725
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AA6 RID: 2726
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AA7 RID: 2727
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AA8 RID: 2728
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AA9 RID: 2729
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AAA RID: 2730
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AAB RID: 2731
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AAC RID: 2732
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AAD RID: 2733
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AAE RID: 2734
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AAF RID: 2735
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AB0 RID: 2736
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AB1 RID: 2737
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AB2 RID: 2738
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AB3 RID: 2739
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AB4 RID: 2740
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AB5 RID: 2741
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AB6 RID: 2742
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AB7 RID: 2743
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AB8 RID: 2744
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AB9 RID: 2745
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ABA RID: 2746
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ABB RID: 2747
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ABC RID: 2748
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x0200017C RID: 380
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[Guid("3050f617-98b5-11cf-bb82-00aa00bdce0b")]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLButtonElementEvents2
		{
			// Token: 0x06000ABD RID: 2749
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ABE RID: 2750
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ABF RID: 2751
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AC0 RID: 2752
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AC1 RID: 2753
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AC2 RID: 2754
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AC3 RID: 2755
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AC4 RID: 2756
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AC5 RID: 2757
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AC6 RID: 2758
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AC7 RID: 2759
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AC8 RID: 2760
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AC9 RID: 2761
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ACA RID: 2762
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ACB RID: 2763
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ACC RID: 2764
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ACD RID: 2765
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ACE RID: 2766
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ACF RID: 2767
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AD0 RID: 2768
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AD1 RID: 2769
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AD2 RID: 2770
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AD3 RID: 2771
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AD4 RID: 2772
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AD5 RID: 2773
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AD6 RID: 2774
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AD7 RID: 2775
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AD8 RID: 2776
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AD9 RID: 2777
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ADA RID: 2778
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ADB RID: 2779
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ADC RID: 2780
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ADD RID: 2781
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ADE RID: 2782
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ADF RID: 2783
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AE0 RID: 2784
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AE1 RID: 2785
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AE2 RID: 2786
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AE3 RID: 2787
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AE4 RID: 2788
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AE5 RID: 2789
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AE6 RID: 2790
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AE7 RID: 2791
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AE8 RID: 2792
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AE9 RID: 2793
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AEA RID: 2794
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AEB RID: 2795
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AEC RID: 2796
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AED RID: 2797
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AEE RID: 2798
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AEF RID: 2799
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AF0 RID: 2800
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AF1 RID: 2801
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AF2 RID: 2802
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AF3 RID: 2803
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AF4 RID: 2804
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AF5 RID: 2805
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AF6 RID: 2806
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AF7 RID: 2807
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AF8 RID: 2808
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AF9 RID: 2809
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AFA RID: 2810
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x0200017D RID: 381
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[Guid("3050f612-98b5-11cf-bb82-00aa00bdce0b")]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLControlElementEvents2
		{
			// Token: 0x06000AFB RID: 2811
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AFC RID: 2812
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AFD RID: 2813
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AFE RID: 2814
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000AFF RID: 2815
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B00 RID: 2816
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B01 RID: 2817
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B02 RID: 2818
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B03 RID: 2819
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B04 RID: 2820
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B05 RID: 2821
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B06 RID: 2822
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B07 RID: 2823
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B08 RID: 2824
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B09 RID: 2825
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B0A RID: 2826
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B0B RID: 2827
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B0C RID: 2828
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B0D RID: 2829
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B0E RID: 2830
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B0F RID: 2831
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B10 RID: 2832
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B11 RID: 2833
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B12 RID: 2834
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B13 RID: 2835
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B14 RID: 2836
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B15 RID: 2837
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B16 RID: 2838
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B17 RID: 2839
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B18 RID: 2840
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B19 RID: 2841
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B1A RID: 2842
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B1B RID: 2843
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B1C RID: 2844
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B1D RID: 2845
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B1E RID: 2846
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B1F RID: 2847
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B20 RID: 2848
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B21 RID: 2849
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B22 RID: 2850
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B23 RID: 2851
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B24 RID: 2852
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B25 RID: 2853
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B26 RID: 2854
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B27 RID: 2855
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B28 RID: 2856
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B29 RID: 2857
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B2A RID: 2858
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B2B RID: 2859
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B2C RID: 2860
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B2D RID: 2861
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B2E RID: 2862
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B2F RID: 2863
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B30 RID: 2864
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B31 RID: 2865
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B32 RID: 2866
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B33 RID: 2867
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B34 RID: 2868
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B35 RID: 2869
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B36 RID: 2870
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B37 RID: 2871
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B38 RID: 2872
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x0200017E RID: 382
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[Guid("3050f614-98b5-11cf-bb82-00aa00bdce0b")]
		[ComImport]
		public interface DHTMLFormElementEvents2
		{
			// Token: 0x06000B39 RID: 2873
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B3A RID: 2874
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B3B RID: 2875
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B3C RID: 2876
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B3D RID: 2877
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B3E RID: 2878
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B3F RID: 2879
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B40 RID: 2880
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B41 RID: 2881
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B42 RID: 2882
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B43 RID: 2883
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B44 RID: 2884
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B45 RID: 2885
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B46 RID: 2886
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B47 RID: 2887
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B48 RID: 2888
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B49 RID: 2889
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B4A RID: 2890
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B4B RID: 2891
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B4C RID: 2892
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B4D RID: 2893
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B4E RID: 2894
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B4F RID: 2895
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B50 RID: 2896
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B51 RID: 2897
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B52 RID: 2898
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B53 RID: 2899
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B54 RID: 2900
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B55 RID: 2901
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B56 RID: 2902
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B57 RID: 2903
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B58 RID: 2904
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B59 RID: 2905
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B5A RID: 2906
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B5B RID: 2907
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B5C RID: 2908
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B5D RID: 2909
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B5E RID: 2910
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B5F RID: 2911
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B60 RID: 2912
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B61 RID: 2913
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B62 RID: 2914
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B63 RID: 2915
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B64 RID: 2916
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B65 RID: 2917
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B66 RID: 2918
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B67 RID: 2919
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B68 RID: 2920
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B69 RID: 2921
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B6A RID: 2922
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B6B RID: 2923
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B6C RID: 2924
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B6D RID: 2925
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B6E RID: 2926
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B6F RID: 2927
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B70 RID: 2928
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B71 RID: 2929
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B72 RID: 2930
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B73 RID: 2931
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B74 RID: 2932
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B75 RID: 2933
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B76 RID: 2934
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B77 RID: 2935
			[DispId(1007)]
			bool onsubmit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B78 RID: 2936
			[DispId(1015)]
			bool onreset(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x0200017F RID: 383
		[Guid("3050f7ff-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLFrameSiteEvents2
		{
			// Token: 0x06000B79 RID: 2937
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B7A RID: 2938
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B7B RID: 2939
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B7C RID: 2940
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B7D RID: 2941
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B7E RID: 2942
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B7F RID: 2943
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B80 RID: 2944
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B81 RID: 2945
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B82 RID: 2946
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B83 RID: 2947
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B84 RID: 2948
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B85 RID: 2949
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B86 RID: 2950
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B87 RID: 2951
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B88 RID: 2952
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B89 RID: 2953
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B8A RID: 2954
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B8B RID: 2955
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B8C RID: 2956
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B8D RID: 2957
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B8E RID: 2958
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B8F RID: 2959
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B90 RID: 2960
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B91 RID: 2961
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B92 RID: 2962
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B93 RID: 2963
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B94 RID: 2964
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B95 RID: 2965
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B96 RID: 2966
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B97 RID: 2967
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B98 RID: 2968
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B99 RID: 2969
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B9A RID: 2970
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B9B RID: 2971
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B9C RID: 2972
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B9D RID: 2973
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B9E RID: 2974
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000B9F RID: 2975
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BA0 RID: 2976
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BA1 RID: 2977
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BA2 RID: 2978
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BA3 RID: 2979
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BA4 RID: 2980
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BA5 RID: 2981
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BA6 RID: 2982
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BA7 RID: 2983
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BA8 RID: 2984
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BA9 RID: 2985
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BAA RID: 2986
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BAB RID: 2987
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BAC RID: 2988
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BAD RID: 2989
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BAE RID: 2990
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BAF RID: 2991
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BB0 RID: 2992
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BB1 RID: 2993
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BB2 RID: 2994
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BB3 RID: 2995
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BB4 RID: 2996
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BB5 RID: 2997
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BB6 RID: 2998
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BB7 RID: 2999
			[DispId(1003)]
			void onload(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x02000180 RID: 384
		[Guid("3050f616-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLImgEvents2
		{
			// Token: 0x06000BB8 RID: 3000
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BB9 RID: 3001
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BBA RID: 3002
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BBB RID: 3003
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BBC RID: 3004
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BBD RID: 3005
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BBE RID: 3006
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BBF RID: 3007
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BC0 RID: 3008
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BC1 RID: 3009
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BC2 RID: 3010
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BC3 RID: 3011
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BC4 RID: 3012
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BC5 RID: 3013
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BC6 RID: 3014
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BC7 RID: 3015
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BC8 RID: 3016
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BC9 RID: 3017
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BCA RID: 3018
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BCB RID: 3019
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BCC RID: 3020
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BCD RID: 3021
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BCE RID: 3022
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BCF RID: 3023
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BD0 RID: 3024
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BD1 RID: 3025
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BD2 RID: 3026
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BD3 RID: 3027
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BD4 RID: 3028
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BD5 RID: 3029
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BD6 RID: 3030
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BD7 RID: 3031
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BD8 RID: 3032
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BD9 RID: 3033
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BDA RID: 3034
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BDB RID: 3035
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BDC RID: 3036
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BDD RID: 3037
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BDE RID: 3038
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BDF RID: 3039
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BE0 RID: 3040
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BE1 RID: 3041
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BE2 RID: 3042
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BE3 RID: 3043
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BE4 RID: 3044
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BE5 RID: 3045
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BE6 RID: 3046
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BE7 RID: 3047
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BE8 RID: 3048
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BE9 RID: 3049
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BEA RID: 3050
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BEB RID: 3051
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BEC RID: 3052
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BED RID: 3053
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BEE RID: 3054
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BEF RID: 3055
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BF0 RID: 3056
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BF1 RID: 3057
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BF2 RID: 3058
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BF3 RID: 3059
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BF4 RID: 3060
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BF5 RID: 3061
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BF6 RID: 3062
			[DispId(1003)]
			void onload(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BF7 RID: 3063
			[DispId(1002)]
			void onerror(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BF8 RID: 3064
			[DispId(1000)]
			void onabort(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x02000181 RID: 385
		[Guid("3050f61a-98b5-11cf-bb82-00aa00bdce0b")]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[ComImport]
		public interface DHTMLInputFileElementEvents2
		{
			// Token: 0x06000BF9 RID: 3065
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BFA RID: 3066
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BFB RID: 3067
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BFC RID: 3068
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BFD RID: 3069
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BFE RID: 3070
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000BFF RID: 3071
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C00 RID: 3072
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C01 RID: 3073
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C02 RID: 3074
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C03 RID: 3075
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C04 RID: 3076
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C05 RID: 3077
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C06 RID: 3078
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C07 RID: 3079
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C08 RID: 3080
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C09 RID: 3081
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C0A RID: 3082
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C0B RID: 3083
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C0C RID: 3084
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C0D RID: 3085
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C0E RID: 3086
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C0F RID: 3087
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C10 RID: 3088
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C11 RID: 3089
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C12 RID: 3090
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C13 RID: 3091
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C14 RID: 3092
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C15 RID: 3093
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C16 RID: 3094
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C17 RID: 3095
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C18 RID: 3096
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C19 RID: 3097
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C1A RID: 3098
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C1B RID: 3099
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C1C RID: 3100
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C1D RID: 3101
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C1E RID: 3102
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C1F RID: 3103
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C20 RID: 3104
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C21 RID: 3105
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C22 RID: 3106
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C23 RID: 3107
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C24 RID: 3108
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C25 RID: 3109
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C26 RID: 3110
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C27 RID: 3111
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C28 RID: 3112
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C29 RID: 3113
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C2A RID: 3114
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C2B RID: 3115
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C2C RID: 3116
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C2D RID: 3117
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C2E RID: 3118
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C2F RID: 3119
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C30 RID: 3120
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C31 RID: 3121
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C32 RID: 3122
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C33 RID: 3123
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C34 RID: 3124
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C35 RID: 3125
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C36 RID: 3126
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C37 RID: 3127
			[DispId(-2147412082)]
			bool onchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C38 RID: 3128
			[DispId(-2147412102)]
			void onselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C39 RID: 3129
			[DispId(1003)]
			void onload(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C3A RID: 3130
			[DispId(1002)]
			void onerror(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C3B RID: 3131
			[DispId(1000)]
			void onabort(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x02000182 RID: 386
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[Guid("3050f61b-98b5-11cf-bb82-00aa00bdce0b")]
		[ComImport]
		public interface DHTMLInputImageEvents2
		{
			// Token: 0x06000C3C RID: 3132
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C3D RID: 3133
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C3E RID: 3134
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C3F RID: 3135
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C40 RID: 3136
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C41 RID: 3137
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C42 RID: 3138
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C43 RID: 3139
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C44 RID: 3140
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C45 RID: 3141
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C46 RID: 3142
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C47 RID: 3143
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C48 RID: 3144
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C49 RID: 3145
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C4A RID: 3146
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C4B RID: 3147
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C4C RID: 3148
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C4D RID: 3149
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C4E RID: 3150
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C4F RID: 3151
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C50 RID: 3152
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C51 RID: 3153
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C52 RID: 3154
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C53 RID: 3155
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C54 RID: 3156
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C55 RID: 3157
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C56 RID: 3158
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C57 RID: 3159
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C58 RID: 3160
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C59 RID: 3161
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C5A RID: 3162
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C5B RID: 3163
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C5C RID: 3164
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C5D RID: 3165
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C5E RID: 3166
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C5F RID: 3167
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C60 RID: 3168
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C61 RID: 3169
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C62 RID: 3170
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C63 RID: 3171
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C64 RID: 3172
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C65 RID: 3173
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C66 RID: 3174
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C67 RID: 3175
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C68 RID: 3176
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C69 RID: 3177
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C6A RID: 3178
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C6B RID: 3179
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C6C RID: 3180
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C6D RID: 3181
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C6E RID: 3182
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C6F RID: 3183
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C70 RID: 3184
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C71 RID: 3185
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C72 RID: 3186
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C73 RID: 3187
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C74 RID: 3188
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C75 RID: 3189
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C76 RID: 3190
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C77 RID: 3191
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C78 RID: 3192
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C79 RID: 3193
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C7A RID: 3194
			[DispId(-2147412080)]
			void onload(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C7B RID: 3195
			[DispId(-2147412083)]
			void onerror(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C7C RID: 3196
			[DispId(-2147412084)]
			void onabort(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x02000183 RID: 387
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[Guid("3050f618-98b5-11cf-bb82-00aa00bdce0b")]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLInputTextElementEvents2
		{
			// Token: 0x06000C7D RID: 3197
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C7E RID: 3198
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C7F RID: 3199
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C80 RID: 3200
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C81 RID: 3201
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C82 RID: 3202
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C83 RID: 3203
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C84 RID: 3204
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C85 RID: 3205
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C86 RID: 3206
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C87 RID: 3207
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C88 RID: 3208
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C89 RID: 3209
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C8A RID: 3210
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C8B RID: 3211
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C8C RID: 3212
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C8D RID: 3213
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C8E RID: 3214
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C8F RID: 3215
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C90 RID: 3216
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C91 RID: 3217
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C92 RID: 3218
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C93 RID: 3219
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C94 RID: 3220
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C95 RID: 3221
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C96 RID: 3222
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C97 RID: 3223
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C98 RID: 3224
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C99 RID: 3225
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C9A RID: 3226
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C9B RID: 3227
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C9C RID: 3228
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C9D RID: 3229
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C9E RID: 3230
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000C9F RID: 3231
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CA0 RID: 3232
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CA1 RID: 3233
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CA2 RID: 3234
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CA3 RID: 3235
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CA4 RID: 3236
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CA5 RID: 3237
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CA6 RID: 3238
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CA7 RID: 3239
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CA8 RID: 3240
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CA9 RID: 3241
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CAA RID: 3242
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CAB RID: 3243
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CAC RID: 3244
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CAD RID: 3245
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CAE RID: 3246
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CAF RID: 3247
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CB0 RID: 3248
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CB1 RID: 3249
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CB2 RID: 3250
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CB3 RID: 3251
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CB4 RID: 3252
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CB5 RID: 3253
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CB6 RID: 3254
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CB7 RID: 3255
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CB8 RID: 3256
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CB9 RID: 3257
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CBA RID: 3258
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CBB RID: 3259
			[DispId(1001)]
			bool onchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CBC RID: 3260
			[DispId(1006)]
			void onselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CBD RID: 3261
			[DispId(1003)]
			void onload(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CBE RID: 3262
			[DispId(1002)]
			void onerror(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CBF RID: 3263
			[DispId(1001)]
			void onabort(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x02000184 RID: 388
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[Guid("3050f61c-98b5-11cf-bb82-00aa00bdce0b")]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLLabelEvents2
		{
			// Token: 0x06000CC0 RID: 3264
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CC1 RID: 3265
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CC2 RID: 3266
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CC3 RID: 3267
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CC4 RID: 3268
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CC5 RID: 3269
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CC6 RID: 3270
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CC7 RID: 3271
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CC8 RID: 3272
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CC9 RID: 3273
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CCA RID: 3274
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CCB RID: 3275
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CCC RID: 3276
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CCD RID: 3277
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CCE RID: 3278
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CCF RID: 3279
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CD0 RID: 3280
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CD1 RID: 3281
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CD2 RID: 3282
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CD3 RID: 3283
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CD4 RID: 3284
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CD5 RID: 3285
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CD6 RID: 3286
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CD7 RID: 3287
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CD8 RID: 3288
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CD9 RID: 3289
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CDA RID: 3290
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CDB RID: 3291
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CDC RID: 3292
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CDD RID: 3293
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CDE RID: 3294
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CDF RID: 3295
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CE0 RID: 3296
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CE1 RID: 3297
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CE2 RID: 3298
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CE3 RID: 3299
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CE4 RID: 3300
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CE5 RID: 3301
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CE6 RID: 3302
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CE7 RID: 3303
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CE8 RID: 3304
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CE9 RID: 3305
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CEA RID: 3306
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CEB RID: 3307
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CEC RID: 3308
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CED RID: 3309
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CEE RID: 3310
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CEF RID: 3311
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CF0 RID: 3312
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CF1 RID: 3313
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CF2 RID: 3314
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CF3 RID: 3315
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CF4 RID: 3316
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CF5 RID: 3317
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CF6 RID: 3318
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CF7 RID: 3319
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CF8 RID: 3320
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CF9 RID: 3321
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CFA RID: 3322
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CFB RID: 3323
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CFC RID: 3324
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CFD RID: 3325
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x02000185 RID: 389
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[Guid("3050f61d-98b5-11cf-bb82-00aa00bdce0b")]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLLinkElementEvents2
		{
			// Token: 0x06000CFE RID: 3326
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000CFF RID: 3327
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D00 RID: 3328
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D01 RID: 3329
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D02 RID: 3330
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D03 RID: 3331
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D04 RID: 3332
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D05 RID: 3333
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D06 RID: 3334
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D07 RID: 3335
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D08 RID: 3336
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D09 RID: 3337
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D0A RID: 3338
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D0B RID: 3339
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D0C RID: 3340
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D0D RID: 3341
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D0E RID: 3342
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D0F RID: 3343
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D10 RID: 3344
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D11 RID: 3345
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D12 RID: 3346
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D13 RID: 3347
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D14 RID: 3348
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D15 RID: 3349
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D16 RID: 3350
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D17 RID: 3351
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D18 RID: 3352
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D19 RID: 3353
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D1A RID: 3354
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D1B RID: 3355
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D1C RID: 3356
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D1D RID: 3357
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D1E RID: 3358
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D1F RID: 3359
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D20 RID: 3360
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D21 RID: 3361
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D22 RID: 3362
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D23 RID: 3363
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D24 RID: 3364
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D25 RID: 3365
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D26 RID: 3366
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D27 RID: 3367
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D28 RID: 3368
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D29 RID: 3369
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D2A RID: 3370
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D2B RID: 3371
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D2C RID: 3372
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D2D RID: 3373
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D2E RID: 3374
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D2F RID: 3375
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D30 RID: 3376
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D31 RID: 3377
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D32 RID: 3378
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D33 RID: 3379
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D34 RID: 3380
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D35 RID: 3381
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D36 RID: 3382
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D37 RID: 3383
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D38 RID: 3384
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D39 RID: 3385
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D3A RID: 3386
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D3B RID: 3387
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D3C RID: 3388
			[DispId(-2147412080)]
			void onload(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D3D RID: 3389
			[DispId(-2147412083)]
			void onerror(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x02000186 RID: 390
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[Guid("3050f61e-98b5-11cf-bb82-00aa00bdce0b")]
		[ComImport]
		public interface DHTMLMapEvents2
		{
			// Token: 0x06000D3E RID: 3390
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D3F RID: 3391
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D40 RID: 3392
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D41 RID: 3393
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D42 RID: 3394
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D43 RID: 3395
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D44 RID: 3396
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D45 RID: 3397
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D46 RID: 3398
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D47 RID: 3399
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D48 RID: 3400
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D49 RID: 3401
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D4A RID: 3402
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D4B RID: 3403
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D4C RID: 3404
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D4D RID: 3405
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D4E RID: 3406
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D4F RID: 3407
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D50 RID: 3408
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D51 RID: 3409
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D52 RID: 3410
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D53 RID: 3411
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D54 RID: 3412
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D55 RID: 3413
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D56 RID: 3414
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D57 RID: 3415
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D58 RID: 3416
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D59 RID: 3417
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D5A RID: 3418
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D5B RID: 3419
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D5C RID: 3420
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D5D RID: 3421
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D5E RID: 3422
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D5F RID: 3423
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D60 RID: 3424
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D61 RID: 3425
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D62 RID: 3426
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D63 RID: 3427
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D64 RID: 3428
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D65 RID: 3429
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D66 RID: 3430
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D67 RID: 3431
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D68 RID: 3432
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D69 RID: 3433
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D6A RID: 3434
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D6B RID: 3435
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D6C RID: 3436
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D6D RID: 3437
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D6E RID: 3438
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D6F RID: 3439
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D70 RID: 3440
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D71 RID: 3441
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D72 RID: 3442
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D73 RID: 3443
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D74 RID: 3444
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D75 RID: 3445
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D76 RID: 3446
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D77 RID: 3447
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D78 RID: 3448
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D79 RID: 3449
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D7A RID: 3450
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D7B RID: 3451
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x02000187 RID: 391
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[Guid("3050f61f-98b5-11cf-bb82-00aa00bdce0b")]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLMarqueeElementEvents2
		{
			// Token: 0x06000D7C RID: 3452
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D7D RID: 3453
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D7E RID: 3454
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D7F RID: 3455
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D80 RID: 3456
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D81 RID: 3457
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D82 RID: 3458
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D83 RID: 3459
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D84 RID: 3460
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D85 RID: 3461
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D86 RID: 3462
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D87 RID: 3463
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D88 RID: 3464
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D89 RID: 3465
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D8A RID: 3466
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D8B RID: 3467
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D8C RID: 3468
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D8D RID: 3469
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D8E RID: 3470
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D8F RID: 3471
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D90 RID: 3472
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D91 RID: 3473
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D92 RID: 3474
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D93 RID: 3475
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D94 RID: 3476
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D95 RID: 3477
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D96 RID: 3478
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D97 RID: 3479
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D98 RID: 3480
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D99 RID: 3481
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D9A RID: 3482
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D9B RID: 3483
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D9C RID: 3484
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D9D RID: 3485
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D9E RID: 3486
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000D9F RID: 3487
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DA0 RID: 3488
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DA1 RID: 3489
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DA2 RID: 3490
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DA3 RID: 3491
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DA4 RID: 3492
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DA5 RID: 3493
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DA6 RID: 3494
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DA7 RID: 3495
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DA8 RID: 3496
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DA9 RID: 3497
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DAA RID: 3498
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DAB RID: 3499
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DAC RID: 3500
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DAD RID: 3501
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DAE RID: 3502
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DAF RID: 3503
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DB0 RID: 3504
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DB1 RID: 3505
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DB2 RID: 3506
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DB3 RID: 3507
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DB4 RID: 3508
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DB5 RID: 3509
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DB6 RID: 3510
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DB7 RID: 3511
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DB8 RID: 3512
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DB9 RID: 3513
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DBA RID: 3514
			[DispId(-2147412092)]
			void onbounce(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DBB RID: 3515
			[DispId(-2147412086)]
			void onfinish(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DBC RID: 3516
			[DispId(-2147412085)]
			void onstart(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x02000188 RID: 392
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[Guid("3050f619-98b5-11cf-bb82-00aa00bdce0b")]
		[ComImport]
		public interface DHTMLOptionButtonElementEvents2
		{
			// Token: 0x06000DBD RID: 3517
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DBE RID: 3518
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DBF RID: 3519
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DC0 RID: 3520
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DC1 RID: 3521
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DC2 RID: 3522
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DC3 RID: 3523
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DC4 RID: 3524
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DC5 RID: 3525
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DC6 RID: 3526
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DC7 RID: 3527
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DC8 RID: 3528
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DC9 RID: 3529
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DCA RID: 3530
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DCB RID: 3531
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DCC RID: 3532
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DCD RID: 3533
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DCE RID: 3534
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DCF RID: 3535
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DD0 RID: 3536
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DD1 RID: 3537
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DD2 RID: 3538
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DD3 RID: 3539
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DD4 RID: 3540
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DD5 RID: 3541
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DD6 RID: 3542
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DD7 RID: 3543
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DD8 RID: 3544
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DD9 RID: 3545
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DDA RID: 3546
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DDB RID: 3547
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DDC RID: 3548
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DDD RID: 3549
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DDE RID: 3550
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DDF RID: 3551
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DE0 RID: 3552
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DE1 RID: 3553
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DE2 RID: 3554
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DE3 RID: 3555
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DE4 RID: 3556
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DE5 RID: 3557
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DE6 RID: 3558
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DE7 RID: 3559
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DE8 RID: 3560
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DE9 RID: 3561
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DEA RID: 3562
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DEB RID: 3563
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DEC RID: 3564
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DED RID: 3565
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DEE RID: 3566
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DEF RID: 3567
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DF0 RID: 3568
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DF1 RID: 3569
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DF2 RID: 3570
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DF3 RID: 3571
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DF4 RID: 3572
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DF5 RID: 3573
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DF6 RID: 3574
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DF7 RID: 3575
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DF8 RID: 3576
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DF9 RID: 3577
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DFA RID: 3578
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DFB RID: 3579
			[DispId(-2147412082)]
			bool onchange(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x02000189 RID: 393
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[Guid("3050f622-98b5-11cf-bb82-00aa00bdce0b")]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLSelectElementEvents2
		{
			// Token: 0x06000DFC RID: 3580
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DFD RID: 3581
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DFE RID: 3582
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000DFF RID: 3583
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E00 RID: 3584
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E01 RID: 3585
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E02 RID: 3586
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E03 RID: 3587
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E04 RID: 3588
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E05 RID: 3589
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E06 RID: 3590
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E07 RID: 3591
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E08 RID: 3592
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E09 RID: 3593
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E0A RID: 3594
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E0B RID: 3595
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E0C RID: 3596
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E0D RID: 3597
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E0E RID: 3598
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E0F RID: 3599
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E10 RID: 3600
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E11 RID: 3601
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E12 RID: 3602
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E13 RID: 3603
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E14 RID: 3604
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E15 RID: 3605
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E16 RID: 3606
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E17 RID: 3607
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E18 RID: 3608
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E19 RID: 3609
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E1A RID: 3610
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E1B RID: 3611
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E1C RID: 3612
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E1D RID: 3613
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E1E RID: 3614
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E1F RID: 3615
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E20 RID: 3616
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E21 RID: 3617
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E22 RID: 3618
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E23 RID: 3619
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E24 RID: 3620
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E25 RID: 3621
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E26 RID: 3622
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E27 RID: 3623
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E28 RID: 3624
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E29 RID: 3625
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E2A RID: 3626
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E2B RID: 3627
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E2C RID: 3628
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E2D RID: 3629
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E2E RID: 3630
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E2F RID: 3631
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E30 RID: 3632
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E31 RID: 3633
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E32 RID: 3634
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E33 RID: 3635
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E34 RID: 3636
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E35 RID: 3637
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E36 RID: 3638
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E37 RID: 3639
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E38 RID: 3640
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E39 RID: 3641
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E3A RID: 3642
			[DispId(-2147412082)]
			void onchange_void(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x0200018A RID: 394
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[Guid("3050f615-98b5-11cf-bb82-00aa00bdce0b")]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLStyleElementEvents2
		{
			// Token: 0x06000E3B RID: 3643
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E3C RID: 3644
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E3D RID: 3645
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E3E RID: 3646
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E3F RID: 3647
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E40 RID: 3648
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E41 RID: 3649
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E42 RID: 3650
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E43 RID: 3651
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E44 RID: 3652
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E45 RID: 3653
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E46 RID: 3654
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E47 RID: 3655
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E48 RID: 3656
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E49 RID: 3657
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E4A RID: 3658
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E4B RID: 3659
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E4C RID: 3660
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E4D RID: 3661
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E4E RID: 3662
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E4F RID: 3663
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E50 RID: 3664
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E51 RID: 3665
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E52 RID: 3666
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E53 RID: 3667
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E54 RID: 3668
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E55 RID: 3669
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E56 RID: 3670
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E57 RID: 3671
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E58 RID: 3672
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E59 RID: 3673
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E5A RID: 3674
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E5B RID: 3675
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E5C RID: 3676
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E5D RID: 3677
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E5E RID: 3678
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E5F RID: 3679
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E60 RID: 3680
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E61 RID: 3681
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E62 RID: 3682
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E63 RID: 3683
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E64 RID: 3684
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E65 RID: 3685
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E66 RID: 3686
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E67 RID: 3687
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E68 RID: 3688
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E69 RID: 3689
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E6A RID: 3690
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E6B RID: 3691
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E6C RID: 3692
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E6D RID: 3693
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E6E RID: 3694
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E6F RID: 3695
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E70 RID: 3696
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E71 RID: 3697
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E72 RID: 3698
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E73 RID: 3699
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E74 RID: 3700
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E75 RID: 3701
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E76 RID: 3702
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E77 RID: 3703
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E78 RID: 3704
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E79 RID: 3705
			[DispId(1003)]
			void onload(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E7A RID: 3706
			[DispId(1002)]
			void onerror(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x0200018B RID: 395
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[Guid("3050f623-98b5-11cf-bb82-00aa00bdce0b")]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLTableEvents2
		{
			// Token: 0x06000E7B RID: 3707
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E7C RID: 3708
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E7D RID: 3709
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E7E RID: 3710
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E7F RID: 3711
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E80 RID: 3712
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E81 RID: 3713
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E82 RID: 3714
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E83 RID: 3715
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E84 RID: 3716
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E85 RID: 3717
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E86 RID: 3718
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E87 RID: 3719
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E88 RID: 3720
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E89 RID: 3721
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E8A RID: 3722
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E8B RID: 3723
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E8C RID: 3724
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E8D RID: 3725
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E8E RID: 3726
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E8F RID: 3727
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E90 RID: 3728
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E91 RID: 3729
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E92 RID: 3730
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E93 RID: 3731
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E94 RID: 3732
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E95 RID: 3733
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E96 RID: 3734
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E97 RID: 3735
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E98 RID: 3736
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E99 RID: 3737
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E9A RID: 3738
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E9B RID: 3739
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E9C RID: 3740
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E9D RID: 3741
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E9E RID: 3742
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000E9F RID: 3743
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EA0 RID: 3744
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EA1 RID: 3745
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EA2 RID: 3746
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EA3 RID: 3747
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EA4 RID: 3748
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EA5 RID: 3749
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EA6 RID: 3750
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EA7 RID: 3751
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EA8 RID: 3752
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EA9 RID: 3753
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EAA RID: 3754
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EAB RID: 3755
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EAC RID: 3756
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EAD RID: 3757
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EAE RID: 3758
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EAF RID: 3759
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EB0 RID: 3760
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EB1 RID: 3761
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EB2 RID: 3762
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EB3 RID: 3763
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EB4 RID: 3764
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EB5 RID: 3765
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EB6 RID: 3766
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EB7 RID: 3767
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EB8 RID: 3768
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x0200018C RID: 396
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[Guid("3050f624-98b5-11cf-bb82-00aa00bdce0b")]
		[ComImport]
		public interface DHTMLTextContainerEvents2
		{
			// Token: 0x06000EB9 RID: 3769
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EBA RID: 3770
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EBB RID: 3771
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EBC RID: 3772
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EBD RID: 3773
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EBE RID: 3774
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EBF RID: 3775
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EC0 RID: 3776
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EC1 RID: 3777
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EC2 RID: 3778
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EC3 RID: 3779
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EC4 RID: 3780
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EC5 RID: 3781
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EC6 RID: 3782
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EC7 RID: 3783
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EC8 RID: 3784
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EC9 RID: 3785
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ECA RID: 3786
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ECB RID: 3787
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ECC RID: 3788
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ECD RID: 3789
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ECE RID: 3790
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ECF RID: 3791
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ED0 RID: 3792
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ED1 RID: 3793
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ED2 RID: 3794
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ED3 RID: 3795
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ED4 RID: 3796
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ED5 RID: 3797
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ED6 RID: 3798
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ED7 RID: 3799
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ED8 RID: 3800
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000ED9 RID: 3801
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EDA RID: 3802
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EDB RID: 3803
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EDC RID: 3804
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EDD RID: 3805
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EDE RID: 3806
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EDF RID: 3807
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EE0 RID: 3808
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EE1 RID: 3809
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EE2 RID: 3810
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EE3 RID: 3811
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EE4 RID: 3812
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EE5 RID: 3813
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EE6 RID: 3814
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EE7 RID: 3815
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EE8 RID: 3816
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EE9 RID: 3817
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EEA RID: 3818
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EEB RID: 3819
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EEC RID: 3820
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EED RID: 3821
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EEE RID: 3822
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EEF RID: 3823
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EF0 RID: 3824
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EF1 RID: 3825
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EF2 RID: 3826
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EF3 RID: 3827
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EF4 RID: 3828
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EF5 RID: 3829
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EF6 RID: 3830
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EF7 RID: 3831
			[DispId(1001)]
			void onchange_void(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EF8 RID: 3832
			[DispId(1006)]
			void onselect(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x0200018D RID: 397
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[Guid("3050f621-98b5-11cf-bb82-00aa00bdce0b")]
		[TypeLibType(TypeLibTypeFlags.FHidden)]
		[ComImport]
		public interface DHTMLScriptEvents2
		{
			// Token: 0x06000EF9 RID: 3833
			[DispId(-2147418102)]
			bool onhelp(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EFA RID: 3834
			[DispId(-600)]
			bool onclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EFB RID: 3835
			[DispId(-601)]
			bool ondblclick(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EFC RID: 3836
			[DispId(-603)]
			bool onkeypress(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EFD RID: 3837
			[DispId(-602)]
			void onkeydown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EFE RID: 3838
			[DispId(-604)]
			void onkeyup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000EFF RID: 3839
			[DispId(-2147418103)]
			void onmouseout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F00 RID: 3840
			[DispId(-2147418104)]
			void onmouseover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F01 RID: 3841
			[DispId(-606)]
			void onmousemove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F02 RID: 3842
			[DispId(-605)]
			void onmousedown(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F03 RID: 3843
			[DispId(-607)]
			void onmouseup(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F04 RID: 3844
			[DispId(-2147418100)]
			bool onselectstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F05 RID: 3845
			[DispId(-2147418095)]
			void onfilterchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F06 RID: 3846
			[DispId(-2147418101)]
			bool ondragstart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F07 RID: 3847
			[DispId(-2147418108)]
			bool onbeforeupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F08 RID: 3848
			[DispId(-2147418107)]
			void onafterupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F09 RID: 3849
			[DispId(-2147418099)]
			bool onerrorupdate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F0A RID: 3850
			[DispId(-2147418106)]
			bool onrowexit(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F0B RID: 3851
			[DispId(-2147418105)]
			void onrowenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F0C RID: 3852
			[DispId(-2147418098)]
			void ondatasetchanged(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F0D RID: 3853
			[DispId(-2147418097)]
			void ondataavailable(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F0E RID: 3854
			[DispId(-2147418096)]
			void ondatasetcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F0F RID: 3855
			[DispId(-2147418094)]
			void onlosecapture(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F10 RID: 3856
			[DispId(-2147418093)]
			void onpropertychange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F11 RID: 3857
			[DispId(1014)]
			void onscroll(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F12 RID: 3858
			[DispId(-2147418111)]
			void onfocus(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F13 RID: 3859
			[DispId(-2147418112)]
			void onblur(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F14 RID: 3860
			[DispId(1016)]
			void onresize(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F15 RID: 3861
			[DispId(-2147418092)]
			bool ondrag(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F16 RID: 3862
			[DispId(-2147418091)]
			void ondragend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F17 RID: 3863
			[DispId(-2147418090)]
			bool ondragenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F18 RID: 3864
			[DispId(-2147418089)]
			bool ondragover(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F19 RID: 3865
			[DispId(-2147418088)]
			void ondragleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F1A RID: 3866
			[DispId(-2147418087)]
			bool ondrop(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F1B RID: 3867
			[DispId(-2147418083)]
			bool onbeforecut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F1C RID: 3868
			[DispId(-2147418086)]
			bool oncut(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F1D RID: 3869
			[DispId(-2147418082)]
			bool onbeforecopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F1E RID: 3870
			[DispId(-2147418085)]
			bool oncopy(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F1F RID: 3871
			[DispId(-2147418081)]
			bool onbeforepaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F20 RID: 3872
			[DispId(-2147418084)]
			bool onpaste(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F21 RID: 3873
			[DispId(1023)]
			bool oncontextmenu(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F22 RID: 3874
			[DispId(-2147418080)]
			void onrowsdelete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F23 RID: 3875
			[DispId(-2147418079)]
			void onrowsinserted(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F24 RID: 3876
			[DispId(-2147418078)]
			void oncellchange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F25 RID: 3877
			[DispId(-609)]
			void onreadystatechange(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F26 RID: 3878
			[DispId(1030)]
			void onlayoutcomplete(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F27 RID: 3879
			[DispId(1031)]
			void onpage(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F28 RID: 3880
			[DispId(1042)]
			void onmouseenter(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F29 RID: 3881
			[DispId(1043)]
			void onmouseleave(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F2A RID: 3882
			[DispId(1044)]
			void onactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F2B RID: 3883
			[DispId(1045)]
			void ondeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F2C RID: 3884
			[DispId(1034)]
			bool onbeforedeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F2D RID: 3885
			[DispId(1047)]
			bool onbeforeactivate(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F2E RID: 3886
			[DispId(1048)]
			void onfocusin(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F2F RID: 3887
			[DispId(1049)]
			void onfocusout(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F30 RID: 3888
			[DispId(1035)]
			void onmove(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F31 RID: 3889
			[DispId(1036)]
			bool oncontrolselect(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F32 RID: 3890
			[DispId(1038)]
			bool onmovestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F33 RID: 3891
			[DispId(1039)]
			void onmoveend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F34 RID: 3892
			[DispId(1040)]
			bool onresizestart(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F35 RID: 3893
			[DispId(1041)]
			void onresizeend(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F36 RID: 3894
			[DispId(1033)]
			bool onmousewheel(UnsafeNativeMethods.IHTMLEventObj evtObj);

			// Token: 0x06000F37 RID: 3895
			[DispId(1002)]
			void onerror(UnsafeNativeMethods.IHTMLEventObj evtObj);
		}

		// Token: 0x0200018E RID: 398
		[ComVisible(true)]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[SuppressUnmanagedCodeSecurity]
		[Guid("3050F25E-98B5-11CF-BB82-00AA00BDCE0B")]
		internal interface IHTMLStyle
		{
			// Token: 0x06000F38 RID: 3896
			void SetFontFamily(string p);

			// Token: 0x06000F39 RID: 3897
			string GetFontFamily();

			// Token: 0x06000F3A RID: 3898
			void SetFontStyle(string p);

			// Token: 0x06000F3B RID: 3899
			string GetFontStyle();

			// Token: 0x06000F3C RID: 3900
			void SetFontObject(string p);

			// Token: 0x06000F3D RID: 3901
			string GetFontObject();

			// Token: 0x06000F3E RID: 3902
			void SetFontWeight(string p);

			// Token: 0x06000F3F RID: 3903
			string GetFontWeight();

			// Token: 0x06000F40 RID: 3904
			void SetFontSize(object p);

			// Token: 0x06000F41 RID: 3905
			object GetFontSize();

			// Token: 0x06000F42 RID: 3906
			void SetFont(string p);

			// Token: 0x06000F43 RID: 3907
			string GetFont();

			// Token: 0x06000F44 RID: 3908
			void SetColor(object p);

			// Token: 0x06000F45 RID: 3909
			object GetColor();

			// Token: 0x06000F46 RID: 3910
			void SetBackground(string p);

			// Token: 0x06000F47 RID: 3911
			string GetBackground();

			// Token: 0x06000F48 RID: 3912
			void SetBackgroundColor(object p);

			// Token: 0x06000F49 RID: 3913
			object GetBackgroundColor();

			// Token: 0x06000F4A RID: 3914
			void SetBackgroundImage(string p);

			// Token: 0x06000F4B RID: 3915
			string GetBackgroundImage();

			// Token: 0x06000F4C RID: 3916
			void SetBackgroundRepeat(string p);

			// Token: 0x06000F4D RID: 3917
			string GetBackgroundRepeat();

			// Token: 0x06000F4E RID: 3918
			void SetBackgroundAttachment(string p);

			// Token: 0x06000F4F RID: 3919
			string GetBackgroundAttachment();

			// Token: 0x06000F50 RID: 3920
			void SetBackgroundPosition(string p);

			// Token: 0x06000F51 RID: 3921
			string GetBackgroundPosition();

			// Token: 0x06000F52 RID: 3922
			void SetBackgroundPositionX(object p);

			// Token: 0x06000F53 RID: 3923
			object GetBackgroundPositionX();

			// Token: 0x06000F54 RID: 3924
			void SetBackgroundPositionY(object p);

			// Token: 0x06000F55 RID: 3925
			object GetBackgroundPositionY();

			// Token: 0x06000F56 RID: 3926
			void SetWordSpacing(object p);

			// Token: 0x06000F57 RID: 3927
			object GetWordSpacing();

			// Token: 0x06000F58 RID: 3928
			void SetLetterSpacing(object p);

			// Token: 0x06000F59 RID: 3929
			object GetLetterSpacing();

			// Token: 0x06000F5A RID: 3930
			void SetTextDecoration(string p);

			// Token: 0x06000F5B RID: 3931
			string GetTextDecoration();

			// Token: 0x06000F5C RID: 3932
			void SetTextDecorationNone(bool p);

			// Token: 0x06000F5D RID: 3933
			bool GetTextDecorationNone();

			// Token: 0x06000F5E RID: 3934
			void SetTextDecorationUnderline(bool p);

			// Token: 0x06000F5F RID: 3935
			bool GetTextDecorationUnderline();

			// Token: 0x06000F60 RID: 3936
			void SetTextDecorationOverline(bool p);

			// Token: 0x06000F61 RID: 3937
			bool GetTextDecorationOverline();

			// Token: 0x06000F62 RID: 3938
			void SetTextDecorationLineThrough(bool p);

			// Token: 0x06000F63 RID: 3939
			bool GetTextDecorationLineThrough();

			// Token: 0x06000F64 RID: 3940
			void SetTextDecorationBlink(bool p);

			// Token: 0x06000F65 RID: 3941
			bool GetTextDecorationBlink();

			// Token: 0x06000F66 RID: 3942
			void SetVerticalAlign(object p);

			// Token: 0x06000F67 RID: 3943
			object GetVerticalAlign();

			// Token: 0x06000F68 RID: 3944
			void SetTextTransform(string p);

			// Token: 0x06000F69 RID: 3945
			string GetTextTransform();

			// Token: 0x06000F6A RID: 3946
			void SetTextAlign(string p);

			// Token: 0x06000F6B RID: 3947
			string GetTextAlign();

			// Token: 0x06000F6C RID: 3948
			void SetTextIndent(object p);

			// Token: 0x06000F6D RID: 3949
			object GetTextIndent();

			// Token: 0x06000F6E RID: 3950
			void SetLineHeight(object p);

			// Token: 0x06000F6F RID: 3951
			object GetLineHeight();

			// Token: 0x06000F70 RID: 3952
			void SetMarginTop(object p);

			// Token: 0x06000F71 RID: 3953
			object GetMarginTop();

			// Token: 0x06000F72 RID: 3954
			void SetMarginRight(object p);

			// Token: 0x06000F73 RID: 3955
			object GetMarginRight();

			// Token: 0x06000F74 RID: 3956
			void SetMarginBottom(object p);

			// Token: 0x06000F75 RID: 3957
			object GetMarginBottom();

			// Token: 0x06000F76 RID: 3958
			void SetMarginLeft(object p);

			// Token: 0x06000F77 RID: 3959
			object GetMarginLeft();

			// Token: 0x06000F78 RID: 3960
			void SetMargin(string p);

			// Token: 0x06000F79 RID: 3961
			string GetMargin();

			// Token: 0x06000F7A RID: 3962
			void SetPaddingTop(object p);

			// Token: 0x06000F7B RID: 3963
			object GetPaddingTop();

			// Token: 0x06000F7C RID: 3964
			void SetPaddingRight(object p);

			// Token: 0x06000F7D RID: 3965
			object GetPaddingRight();

			// Token: 0x06000F7E RID: 3966
			void SetPaddingBottom(object p);

			// Token: 0x06000F7F RID: 3967
			object GetPaddingBottom();

			// Token: 0x06000F80 RID: 3968
			void SetPaddingLeft(object p);

			// Token: 0x06000F81 RID: 3969
			object GetPaddingLeft();

			// Token: 0x06000F82 RID: 3970
			void SetPadding(string p);

			// Token: 0x06000F83 RID: 3971
			string GetPadding();

			// Token: 0x06000F84 RID: 3972
			void SetBorder(string p);

			// Token: 0x06000F85 RID: 3973
			string GetBorder();

			// Token: 0x06000F86 RID: 3974
			void SetBorderTop(string p);

			// Token: 0x06000F87 RID: 3975
			string GetBorderTop();

			// Token: 0x06000F88 RID: 3976
			void SetBorderRight(string p);

			// Token: 0x06000F89 RID: 3977
			string GetBorderRight();

			// Token: 0x06000F8A RID: 3978
			void SetBorderBottom(string p);

			// Token: 0x06000F8B RID: 3979
			string GetBorderBottom();

			// Token: 0x06000F8C RID: 3980
			void SetBorderLeft(string p);

			// Token: 0x06000F8D RID: 3981
			string GetBorderLeft();

			// Token: 0x06000F8E RID: 3982
			void SetBorderColor(string p);

			// Token: 0x06000F8F RID: 3983
			string GetBorderColor();

			// Token: 0x06000F90 RID: 3984
			void SetBorderTopColor(object p);

			// Token: 0x06000F91 RID: 3985
			object GetBorderTopColor();

			// Token: 0x06000F92 RID: 3986
			void SetBorderRightColor(object p);

			// Token: 0x06000F93 RID: 3987
			object GetBorderRightColor();

			// Token: 0x06000F94 RID: 3988
			void SetBorderBottomColor(object p);

			// Token: 0x06000F95 RID: 3989
			object GetBorderBottomColor();

			// Token: 0x06000F96 RID: 3990
			void SetBorderLeftColor(object p);

			// Token: 0x06000F97 RID: 3991
			object GetBorderLeftColor();

			// Token: 0x06000F98 RID: 3992
			void SetBorderWidth(string p);

			// Token: 0x06000F99 RID: 3993
			string GetBorderWidth();

			// Token: 0x06000F9A RID: 3994
			void SetBorderTopWidth(object p);

			// Token: 0x06000F9B RID: 3995
			object GetBorderTopWidth();

			// Token: 0x06000F9C RID: 3996
			void SetBorderRightWidth(object p);

			// Token: 0x06000F9D RID: 3997
			object GetBorderRightWidth();

			// Token: 0x06000F9E RID: 3998
			void SetBorderBottomWidth(object p);

			// Token: 0x06000F9F RID: 3999
			object GetBorderBottomWidth();

			// Token: 0x06000FA0 RID: 4000
			void SetBorderLeftWidth(object p);

			// Token: 0x06000FA1 RID: 4001
			object GetBorderLeftWidth();

			// Token: 0x06000FA2 RID: 4002
			void SetBorderStyle(string p);

			// Token: 0x06000FA3 RID: 4003
			string GetBorderStyle();

			// Token: 0x06000FA4 RID: 4004
			void SetBorderTopStyle(string p);

			// Token: 0x06000FA5 RID: 4005
			string GetBorderTopStyle();

			// Token: 0x06000FA6 RID: 4006
			void SetBorderRightStyle(string p);

			// Token: 0x06000FA7 RID: 4007
			string GetBorderRightStyle();

			// Token: 0x06000FA8 RID: 4008
			void SetBorderBottomStyle(string p);

			// Token: 0x06000FA9 RID: 4009
			string GetBorderBottomStyle();

			// Token: 0x06000FAA RID: 4010
			void SetBorderLeftStyle(string p);

			// Token: 0x06000FAB RID: 4011
			string GetBorderLeftStyle();

			// Token: 0x06000FAC RID: 4012
			void SetWidth(object p);

			// Token: 0x06000FAD RID: 4013
			object GetWidth();

			// Token: 0x06000FAE RID: 4014
			void SetHeight(object p);

			// Token: 0x06000FAF RID: 4015
			object GetHeight();

			// Token: 0x06000FB0 RID: 4016
			void SetStyleFloat(string p);

			// Token: 0x06000FB1 RID: 4017
			string GetStyleFloat();

			// Token: 0x06000FB2 RID: 4018
			void SetClear(string p);

			// Token: 0x06000FB3 RID: 4019
			string GetClear();

			// Token: 0x06000FB4 RID: 4020
			void SetDisplay(string p);

			// Token: 0x06000FB5 RID: 4021
			string GetDisplay();

			// Token: 0x06000FB6 RID: 4022
			void SetVisibility(string p);

			// Token: 0x06000FB7 RID: 4023
			string GetVisibility();

			// Token: 0x06000FB8 RID: 4024
			void SetListStyleType(string p);

			// Token: 0x06000FB9 RID: 4025
			string GetListStyleType();

			// Token: 0x06000FBA RID: 4026
			void SetListStylePosition(string p);

			// Token: 0x06000FBB RID: 4027
			string GetListStylePosition();

			// Token: 0x06000FBC RID: 4028
			void SetListStyleImage(string p);

			// Token: 0x06000FBD RID: 4029
			string GetListStyleImage();

			// Token: 0x06000FBE RID: 4030
			void SetListStyle(string p);

			// Token: 0x06000FBF RID: 4031
			string GetListStyle();

			// Token: 0x06000FC0 RID: 4032
			void SetWhiteSpace(string p);

			// Token: 0x06000FC1 RID: 4033
			string GetWhiteSpace();

			// Token: 0x06000FC2 RID: 4034
			void SetTop(object p);

			// Token: 0x06000FC3 RID: 4035
			object GetTop();

			// Token: 0x06000FC4 RID: 4036
			void SetLeft(object p);

			// Token: 0x06000FC5 RID: 4037
			object GetLeft();

			// Token: 0x06000FC6 RID: 4038
			string GetPosition();

			// Token: 0x06000FC7 RID: 4039
			void SetZIndex(object p);

			// Token: 0x06000FC8 RID: 4040
			object GetZIndex();

			// Token: 0x06000FC9 RID: 4041
			void SetOverflow(string p);

			// Token: 0x06000FCA RID: 4042
			string GetOverflow();

			// Token: 0x06000FCB RID: 4043
			void SetPageBreakBefore(string p);

			// Token: 0x06000FCC RID: 4044
			string GetPageBreakBefore();

			// Token: 0x06000FCD RID: 4045
			void SetPageBreakAfter(string p);

			// Token: 0x06000FCE RID: 4046
			string GetPageBreakAfter();

			// Token: 0x06000FCF RID: 4047
			void SetCssText(string p);

			// Token: 0x06000FD0 RID: 4048
			string GetCssText();

			// Token: 0x06000FD1 RID: 4049
			void SetPixelTop(int p);

			// Token: 0x06000FD2 RID: 4050
			int GetPixelTop();

			// Token: 0x06000FD3 RID: 4051
			void SetPixelLeft(int p);

			// Token: 0x06000FD4 RID: 4052
			int GetPixelLeft();

			// Token: 0x06000FD5 RID: 4053
			void SetPixelWidth(int p);

			// Token: 0x06000FD6 RID: 4054
			int GetPixelWidth();

			// Token: 0x06000FD7 RID: 4055
			void SetPixelHeight(int p);

			// Token: 0x06000FD8 RID: 4056
			int GetPixelHeight();

			// Token: 0x06000FD9 RID: 4057
			void SetPosTop(float p);

			// Token: 0x06000FDA RID: 4058
			float GetPosTop();

			// Token: 0x06000FDB RID: 4059
			void SetPosLeft(float p);

			// Token: 0x06000FDC RID: 4060
			float GetPosLeft();

			// Token: 0x06000FDD RID: 4061
			void SetPosWidth(float p);

			// Token: 0x06000FDE RID: 4062
			float GetPosWidth();

			// Token: 0x06000FDF RID: 4063
			void SetPosHeight(float p);

			// Token: 0x06000FE0 RID: 4064
			float GetPosHeight();

			// Token: 0x06000FE1 RID: 4065
			void SetCursor(string p);

			// Token: 0x06000FE2 RID: 4066
			string GetCursor();

			// Token: 0x06000FE3 RID: 4067
			void SetClip(string p);

			// Token: 0x06000FE4 RID: 4068
			string GetClip();

			// Token: 0x06000FE5 RID: 4069
			void SetFilter(string p);

			// Token: 0x06000FE6 RID: 4070
			string GetFilter();

			// Token: 0x06000FE7 RID: 4071
			void SetAttribute(string strAttributeName, object AttributeValue, int lFlags);

			// Token: 0x06000FE8 RID: 4072
			object GetAttribute(string strAttributeName, int lFlags);

			// Token: 0x06000FE9 RID: 4073
			bool RemoveAttribute(string strAttributeName, int lFlags);
		}

		// Token: 0x0200018F RID: 399
		[Guid("39088D7E-B71E-11D1-8F39-00C04FD946D0")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IExtender
		{
			// Token: 0x1700019F RID: 415
			// (get) Token: 0x06000FEA RID: 4074
			// (set) Token: 0x06000FEB RID: 4075
			int Align { get; set; }

			// Token: 0x170001A0 RID: 416
			// (get) Token: 0x06000FEC RID: 4076
			// (set) Token: 0x06000FED RID: 4077
			bool Enabled { get; set; }

			// Token: 0x170001A1 RID: 417
			// (get) Token: 0x06000FEE RID: 4078
			// (set) Token: 0x06000FEF RID: 4079
			int Height { get; set; }

			// Token: 0x170001A2 RID: 418
			// (get) Token: 0x06000FF0 RID: 4080
			// (set) Token: 0x06000FF1 RID: 4081
			int Left { get; set; }

			// Token: 0x170001A3 RID: 419
			// (get) Token: 0x06000FF2 RID: 4082
			// (set) Token: 0x06000FF3 RID: 4083
			bool TabStop { get; set; }

			// Token: 0x170001A4 RID: 420
			// (get) Token: 0x06000FF4 RID: 4084
			// (set) Token: 0x06000FF5 RID: 4085
			int Top { get; set; }

			// Token: 0x170001A5 RID: 421
			// (get) Token: 0x06000FF6 RID: 4086
			// (set) Token: 0x06000FF7 RID: 4087
			bool Visible { get; set; }

			// Token: 0x170001A6 RID: 422
			// (get) Token: 0x06000FF8 RID: 4088
			// (set) Token: 0x06000FF9 RID: 4089
			int Width { get; set; }

			// Token: 0x170001A7 RID: 423
			// (get) Token: 0x06000FFA RID: 4090
			string Name
			{
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
			}

			// Token: 0x170001A8 RID: 424
			// (get) Token: 0x06000FFB RID: 4091
			object Parent
			{
				[return: MarshalAs(UnmanagedType.Interface)]
				get;
			}

			// Token: 0x170001A9 RID: 425
			// (get) Token: 0x06000FFC RID: 4092
			IntPtr Hwnd { get; }

			// Token: 0x170001AA RID: 426
			// (get) Token: 0x06000FFD RID: 4093
			object Container
			{
				[return: MarshalAs(UnmanagedType.Interface)]
				get;
			}

			// Token: 0x06000FFE RID: 4094
			void Move([MarshalAs(UnmanagedType.Interface)] [In] object left, [MarshalAs(UnmanagedType.Interface)] [In] object top, [MarshalAs(UnmanagedType.Interface)] [In] object width, [MarshalAs(UnmanagedType.Interface)] [In] object height);
		}

		// Token: 0x02000190 RID: 400
		[Guid("8A701DA0-4FEB-101B-A82E-08002B2B2337")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IGetOleObject
		{
			// Token: 0x06000FFF RID: 4095
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetOleObject(ref Guid riid);
		}

		// Token: 0x02000191 RID: 401
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("CB2F6722-AB3A-11d2-9C40-00C04FA30A3E")]
		[ComImport]
		internal interface ICorRuntimeHost
		{
			// Token: 0x06001000 RID: 4096
			[PreserveSig]
			int CreateLogicalThreadState();

			// Token: 0x06001001 RID: 4097
			[PreserveSig]
			int DeleteLogicalThreadState();

			// Token: 0x06001002 RID: 4098
			[PreserveSig]
			int SwitchInLogicalThreadState([In] ref uint pFiberCookie);

			// Token: 0x06001003 RID: 4099
			[PreserveSig]
			int SwitchOutLogicalThreadState(out uint FiberCookie);

			// Token: 0x06001004 RID: 4100
			[PreserveSig]
			int LocksHeldByLogicalThread(out uint pCount);

			// Token: 0x06001005 RID: 4101
			[PreserveSig]
			int MapFile(IntPtr hFile, out IntPtr hMapAddress);

			// Token: 0x06001006 RID: 4102
			[PreserveSig]
			int GetConfiguration([MarshalAs(UnmanagedType.IUnknown)] out object pConfiguration);

			// Token: 0x06001007 RID: 4103
			[PreserveSig]
			int Start();

			// Token: 0x06001008 RID: 4104
			[PreserveSig]
			int Stop();

			// Token: 0x06001009 RID: 4105
			[PreserveSig]
			int CreateDomain(string pwzFriendlyName, [MarshalAs(UnmanagedType.IUnknown)] object pIdentityArray, [MarshalAs(UnmanagedType.IUnknown)] out object pAppDomain);

			// Token: 0x0600100A RID: 4106
			[PreserveSig]
			int GetDefaultDomain([MarshalAs(UnmanagedType.IUnknown)] out object pAppDomain);

			// Token: 0x0600100B RID: 4107
			[PreserveSig]
			int EnumDomains(out IntPtr hEnum);

			// Token: 0x0600100C RID: 4108
			[PreserveSig]
			int NextDomain(IntPtr hEnum, [MarshalAs(UnmanagedType.IUnknown)] out object pAppDomain);

			// Token: 0x0600100D RID: 4109
			[PreserveSig]
			int CloseEnum(IntPtr hEnum);

			// Token: 0x0600100E RID: 4110
			[PreserveSig]
			int CreateDomainEx(string pwzFriendlyName, [MarshalAs(UnmanagedType.IUnknown)] object pSetup, [MarshalAs(UnmanagedType.IUnknown)] object pEvidence, [MarshalAs(UnmanagedType.IUnknown)] out object pAppDomain);

			// Token: 0x0600100F RID: 4111
			[PreserveSig]
			int CreateDomainSetup([MarshalAs(UnmanagedType.IUnknown)] out object pAppDomainSetup);

			// Token: 0x06001010 RID: 4112
			[PreserveSig]
			int CreateEvidence([MarshalAs(UnmanagedType.IUnknown)] out object pEvidence);

			// Token: 0x06001011 RID: 4113
			[PreserveSig]
			int UnloadDomain([MarshalAs(UnmanagedType.IUnknown)] object pAppDomain);

			// Token: 0x06001012 RID: 4114
			[PreserveSig]
			int CurrentDomain([MarshalAs(UnmanagedType.IUnknown)] out object pAppDomain);
		}

		// Token: 0x02000192 RID: 402
		[Guid("CB2F6723-AB3A-11d2-9C40-00C04FA30A3E")]
		[ComImport]
		internal class CorRuntimeHost
		{
			// Token: 0x06001013 RID: 4115
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			public extern CorRuntimeHost();
		}

		// Token: 0x02000193 RID: 403
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("000C0601-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IMsoComponentManager
		{
			// Token: 0x06001014 RID: 4116
			[PreserveSig]
			int QueryService(ref Guid guidService, ref Guid iid, [MarshalAs(UnmanagedType.Interface)] out object ppvObj);

			// Token: 0x06001015 RID: 4117
			[PreserveSig]
			bool FDebugMessage(IntPtr hInst, int msg, IntPtr wParam, IntPtr lParam);

			// Token: 0x06001016 RID: 4118
			[PreserveSig]
			bool FRegisterComponent(UnsafeNativeMethods.IMsoComponent component, NativeMethods.MSOCRINFOSTRUCT pcrinfo, out int dwComponentID);

			// Token: 0x06001017 RID: 4119
			[PreserveSig]
			bool FRevokeComponent(int dwComponentID);

			// Token: 0x06001018 RID: 4120
			[PreserveSig]
			bool FUpdateComponentRegistration(int dwComponentID, NativeMethods.MSOCRINFOSTRUCT pcrinfo);

			// Token: 0x06001019 RID: 4121
			[PreserveSig]
			bool FOnComponentActivate(int dwComponentID);

			// Token: 0x0600101A RID: 4122
			[PreserveSig]
			bool FSetTrackingComponent(int dwComponentID, [MarshalAs(UnmanagedType.Bool)] [In] bool fTrack);

			// Token: 0x0600101B RID: 4123
			[PreserveSig]
			void OnComponentEnterState(int dwComponentID, int uStateID, int uContext, int cpicmExclude, int rgpicmExclude, int dwReserved);

			// Token: 0x0600101C RID: 4124
			[PreserveSig]
			bool FOnComponentExitState(int dwComponentID, int uStateID, int uContext, int cpicmExclude, int rgpicmExclude);

			// Token: 0x0600101D RID: 4125
			[PreserveSig]
			bool FInState(int uStateID, IntPtr pvoid);

			// Token: 0x0600101E RID: 4126
			[PreserveSig]
			bool FContinueIdle();

			// Token: 0x0600101F RID: 4127
			[PreserveSig]
			bool FPushMessageLoop(int dwComponentID, int uReason, int pvLoopData);

			// Token: 0x06001020 RID: 4128
			[PreserveSig]
			bool FCreateSubComponentManager([MarshalAs(UnmanagedType.Interface)] object punkOuter, [MarshalAs(UnmanagedType.Interface)] object punkServProv, ref Guid riid, out IntPtr ppvObj);

			// Token: 0x06001021 RID: 4129
			[PreserveSig]
			bool FGetParentComponentManager(out UnsafeNativeMethods.IMsoComponentManager ppicm);

			// Token: 0x06001022 RID: 4130
			[PreserveSig]
			bool FGetActiveComponent(int dwgac, [MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.IMsoComponent[] ppic, NativeMethods.MSOCRINFOSTRUCT pcrinfo, int dwReserved);
		}

		// Token: 0x02000194 RID: 404
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("000C0600-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IMsoComponent
		{
			// Token: 0x06001023 RID: 4131
			[PreserveSig]
			bool FDebugMessage(IntPtr hInst, int msg, IntPtr wParam, IntPtr lParam);

			// Token: 0x06001024 RID: 4132
			[PreserveSig]
			bool FPreTranslateMessage(ref NativeMethods.MSG msg);

			// Token: 0x06001025 RID: 4133
			[PreserveSig]
			void OnEnterState(int uStateID, bool fEnter);

			// Token: 0x06001026 RID: 4134
			[PreserveSig]
			void OnAppActivate(bool fActive, int dwOtherThreadID);

			// Token: 0x06001027 RID: 4135
			[PreserveSig]
			void OnLoseActivation();

			// Token: 0x06001028 RID: 4136
			[PreserveSig]
			void OnActivationChange(UnsafeNativeMethods.IMsoComponent component, bool fSameComponent, int pcrinfo, bool fHostIsActivating, int pchostinfo, int dwReserved);

			// Token: 0x06001029 RID: 4137
			[PreserveSig]
			bool FDoIdle(int grfidlef);

			// Token: 0x0600102A RID: 4138
			[PreserveSig]
			bool FContinueMessageLoop(int uReason, int pvLoopData, [MarshalAs(UnmanagedType.LPArray)] NativeMethods.MSG[] pMsgPeeked);

			// Token: 0x0600102B RID: 4139
			[PreserveSig]
			bool FQueryTerminate(bool fPromptUser);

			// Token: 0x0600102C RID: 4140
			[PreserveSig]
			void Terminate();

			// Token: 0x0600102D RID: 4141
			[PreserveSig]
			IntPtr HwndGetWindow(int dwWhich, int dwReserved);
		}

		// Token: 0x02000195 RID: 405
		[SuppressUnmanagedCodeSecurity]
		[ComVisible(true)]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[Guid("8CC497C0-A1DF-11ce-8098-00AA0047BE5D")]
		public interface ITextDocument
		{
			// Token: 0x0600102E RID: 4142
			string GetName();

			// Token: 0x0600102F RID: 4143
			object GetSelection();

			// Token: 0x06001030 RID: 4144
			int GetStoryCount();

			// Token: 0x06001031 RID: 4145
			object GetStoryRanges();

			// Token: 0x06001032 RID: 4146
			int GetSaved();

			// Token: 0x06001033 RID: 4147
			void SetSaved(int value);

			// Token: 0x06001034 RID: 4148
			object GetDefaultTabStop();

			// Token: 0x06001035 RID: 4149
			void SetDefaultTabStop(object value);

			// Token: 0x06001036 RID: 4150
			void New();

			// Token: 0x06001037 RID: 4151
			void Open(object pVar, int flags, int codePage);

			// Token: 0x06001038 RID: 4152
			void Save(object pVar, int flags, int codePage);

			// Token: 0x06001039 RID: 4153
			int Freeze();

			// Token: 0x0600103A RID: 4154
			int Unfreeze();

			// Token: 0x0600103B RID: 4155
			void BeginEditCollection();

			// Token: 0x0600103C RID: 4156
			void EndEditCollection();

			// Token: 0x0600103D RID: 4157
			int Undo(int count);

			// Token: 0x0600103E RID: 4158
			int Redo(int count);

			// Token: 0x0600103F RID: 4159
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.ITextRange Range(int cp1, int cp2);

			// Token: 0x06001040 RID: 4160
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.ITextRange RangeFromPoint(int x, int y);
		}

		// Token: 0x02000196 RID: 406
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[SuppressUnmanagedCodeSecurity]
		[Guid("8CC497C2-A1DF-11ce-8098-00AA0047BE5D")]
		[ComVisible(true)]
		public interface ITextRange
		{
			// Token: 0x06001041 RID: 4161
			string GetText();

			// Token: 0x06001042 RID: 4162
			void SetText(string text);

			// Token: 0x06001043 RID: 4163
			object GetChar();

			// Token: 0x06001044 RID: 4164
			void SetChar(object ch);

			// Token: 0x06001045 RID: 4165
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.ITextRange GetDuplicate();

			// Token: 0x06001046 RID: 4166
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.ITextRange GetFormattedText();

			// Token: 0x06001047 RID: 4167
			void SetFormattedText([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.ITextRange range);

			// Token: 0x06001048 RID: 4168
			int GetStart();

			// Token: 0x06001049 RID: 4169
			void SetStart(int cpFirst);

			// Token: 0x0600104A RID: 4170
			int GetEnd();

			// Token: 0x0600104B RID: 4171
			void SetEnd(int cpLim);

			// Token: 0x0600104C RID: 4172
			object GetFont();

			// Token: 0x0600104D RID: 4173
			void SetFont(object font);

			// Token: 0x0600104E RID: 4174
			object GetPara();

			// Token: 0x0600104F RID: 4175
			void SetPara(object para);

			// Token: 0x06001050 RID: 4176
			int GetStoryLength();

			// Token: 0x06001051 RID: 4177
			int GetStoryType();

			// Token: 0x06001052 RID: 4178
			void Collapse(int start);

			// Token: 0x06001053 RID: 4179
			int Expand(int unit);

			// Token: 0x06001054 RID: 4180
			int GetIndex(int unit);

			// Token: 0x06001055 RID: 4181
			void SetIndex(int unit, int index, int extend);

			// Token: 0x06001056 RID: 4182
			void SetRange(int cpActive, int cpOther);

			// Token: 0x06001057 RID: 4183
			int InRange([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.ITextRange range);

			// Token: 0x06001058 RID: 4184
			int InStory([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.ITextRange range);

			// Token: 0x06001059 RID: 4185
			int IsEqual([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.ITextRange range);

			// Token: 0x0600105A RID: 4186
			void Select();

			// Token: 0x0600105B RID: 4187
			int StartOf(int unit, int extend);

			// Token: 0x0600105C RID: 4188
			int EndOf(int unit, int extend);

			// Token: 0x0600105D RID: 4189
			int Move(int unit, int count);

			// Token: 0x0600105E RID: 4190
			int MoveStart(int unit, int count);

			// Token: 0x0600105F RID: 4191
			int MoveEnd(int unit, int count);

			// Token: 0x06001060 RID: 4192
			int MoveWhile(object cset, int count);

			// Token: 0x06001061 RID: 4193
			int MoveStartWhile(object cset, int count);

			// Token: 0x06001062 RID: 4194
			int MoveEndWhile(object cset, int count);

			// Token: 0x06001063 RID: 4195
			int MoveUntil(object cset, int count);

			// Token: 0x06001064 RID: 4196
			int MoveStartUntil(object cset, int count);

			// Token: 0x06001065 RID: 4197
			int MoveEndUntil(object cset, int count);

			// Token: 0x06001066 RID: 4198
			int FindText(string text, int cch, int flags);

			// Token: 0x06001067 RID: 4199
			int FindTextStart(string text, int cch, int flags);

			// Token: 0x06001068 RID: 4200
			int FindTextEnd(string text, int cch, int flags);

			// Token: 0x06001069 RID: 4201
			int Delete(int unit, int count);

			// Token: 0x0600106A RID: 4202
			void Cut(out object pVar);

			// Token: 0x0600106B RID: 4203
			void Copy(out object pVar);

			// Token: 0x0600106C RID: 4204
			void Paste(object pVar, int format);

			// Token: 0x0600106D RID: 4205
			int CanPaste(object pVar, int format);

			// Token: 0x0600106E RID: 4206
			int CanEdit();

			// Token: 0x0600106F RID: 4207
			void ChangeCase(int type);

			// Token: 0x06001070 RID: 4208
			void GetPoint(int type, out int x, out int y);

			// Token: 0x06001071 RID: 4209
			void SetPoint(int x, int y, int type, int extend);

			// Token: 0x06001072 RID: 4210
			void ScrollIntoView(int value);

			// Token: 0x06001073 RID: 4211
			object GetEmbeddedObject();
		}

		// Token: 0x02000197 RID: 407
		[Guid("00020D03-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IRichEditOleCallback
		{
			// Token: 0x06001074 RID: 4212
			[PreserveSig]
			int GetNewStorage(out UnsafeNativeMethods.IStorage ret);

			// Token: 0x06001075 RID: 4213
			[PreserveSig]
			int GetInPlaceContext(IntPtr lplpFrame, IntPtr lplpDoc, IntPtr lpFrameInfo);

			// Token: 0x06001076 RID: 4214
			[PreserveSig]
			int ShowContainerUI(int fShow);

			// Token: 0x06001077 RID: 4215
			[PreserveSig]
			int QueryInsertObject(ref Guid lpclsid, IntPtr lpstg, int cp);

			// Token: 0x06001078 RID: 4216
			[PreserveSig]
			int DeleteObject(IntPtr lpoleobj);

			// Token: 0x06001079 RID: 4217
			[PreserveSig]
			int QueryAcceptData(IDataObject lpdataobj, IntPtr lpcfFormat, int reco, int fReally, IntPtr hMetaPict);

			// Token: 0x0600107A RID: 4218
			[PreserveSig]
			int ContextSensitiveHelp(int fEnterMode);

			// Token: 0x0600107B RID: 4219
			[PreserveSig]
			int GetClipboardData(NativeMethods.CHARRANGE lpchrg, int reco, IntPtr lplpdataobj);

			// Token: 0x0600107C RID: 4220
			[PreserveSig]
			int GetDragDropEffect(bool fDrag, int grfKeyState, ref int pdwEffect);

			// Token: 0x0600107D RID: 4221
			[PreserveSig]
			int GetContextMenu(short seltype, IntPtr lpoleobj, NativeMethods.CHARRANGE lpchrg, out IntPtr hmenu);
		}

		// Token: 0x02000198 RID: 408
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("00000115-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IOleInPlaceUIWindow
		{
			// Token: 0x0600107E RID: 4222
			IntPtr GetWindow();

			// Token: 0x0600107F RID: 4223
			[PreserveSig]
			int ContextSensitiveHelp(int fEnterMode);

			// Token: 0x06001080 RID: 4224
			[PreserveSig]
			int GetBorder([Out] NativeMethods.COMRECT lprectBorder);

			// Token: 0x06001081 RID: 4225
			[PreserveSig]
			int RequestBorderSpace([In] NativeMethods.COMRECT pborderwidths);

			// Token: 0x06001082 RID: 4226
			[PreserveSig]
			int SetBorderSpace([In] NativeMethods.COMRECT pborderwidths);

			// Token: 0x06001083 RID: 4227
			void SetActiveObject([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IOleInPlaceActiveObject pActiveObject, [MarshalAs(UnmanagedType.LPWStr)] [In] string pszObjName);
		}

		// Token: 0x02000199 RID: 409
		[Guid("00000117-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[SuppressUnmanagedCodeSecurity]
		[ComImport]
		public interface IOleInPlaceActiveObject
		{
			// Token: 0x06001084 RID: 4228
			[PreserveSig]
			int GetWindow(out IntPtr hwnd);

			// Token: 0x06001085 RID: 4229
			void ContextSensitiveHelp(int fEnterMode);

			// Token: 0x06001086 RID: 4230
			[PreserveSig]
			int TranslateAccelerator([In] ref NativeMethods.MSG lpmsg);

			// Token: 0x06001087 RID: 4231
			void OnFrameWindowActivate(bool fActivate);

			// Token: 0x06001088 RID: 4232
			void OnDocWindowActivate(int fActivate);

			// Token: 0x06001089 RID: 4233
			void ResizeBorder([In] NativeMethods.COMRECT prcBorder, [In] UnsafeNativeMethods.IOleInPlaceUIWindow pUIWindow, bool fFrameWindow);

			// Token: 0x0600108A RID: 4234
			void EnableModeless(int fEnable);
		}

		// Token: 0x0200019A RID: 410
		[Guid("00000114-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleWindow
		{
			// Token: 0x0600108B RID: 4235
			[PreserveSig]
			int GetWindow(out IntPtr hwnd);

			// Token: 0x0600108C RID: 4236
			void ContextSensitiveHelp(int fEnterMode);
		}

		// Token: 0x0200019B RID: 411
		[Guid("00000113-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[SuppressUnmanagedCodeSecurity]
		[ComImport]
		public interface IOleInPlaceObject
		{
			// Token: 0x0600108D RID: 4237
			[PreserveSig]
			int GetWindow(out IntPtr hwnd);

			// Token: 0x0600108E RID: 4238
			void ContextSensitiveHelp(int fEnterMode);

			// Token: 0x0600108F RID: 4239
			void InPlaceDeactivate();

			// Token: 0x06001090 RID: 4240
			[PreserveSig]
			int UIDeactivate();

			// Token: 0x06001091 RID: 4241
			void SetObjectRects([In] NativeMethods.COMRECT lprcPosRect, [In] NativeMethods.COMRECT lprcClipRect);

			// Token: 0x06001092 RID: 4242
			void ReactivateAndUndo();
		}

		// Token: 0x0200019C RID: 412
		[Guid("00000112-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[SuppressUnmanagedCodeSecurity]
		[ComImport]
		public interface IOleObject
		{
			// Token: 0x06001093 RID: 4243
			[PreserveSig]
			int SetClientSite([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IOleClientSite pClientSite);

			// Token: 0x06001094 RID: 4244
			UnsafeNativeMethods.IOleClientSite GetClientSite();

			// Token: 0x06001095 RID: 4245
			[PreserveSig]
			int SetHostNames([MarshalAs(UnmanagedType.LPWStr)] [In] string szContainerApp, [MarshalAs(UnmanagedType.LPWStr)] [In] string szContainerObj);

			// Token: 0x06001096 RID: 4246
			[PreserveSig]
			int Close(int dwSaveOption);

			// Token: 0x06001097 RID: 4247
			[PreserveSig]
			int SetMoniker([MarshalAs(UnmanagedType.U4)] [In] int dwWhichMoniker, [MarshalAs(UnmanagedType.Interface)] [In] object pmk);

			// Token: 0x06001098 RID: 4248
			[PreserveSig]
			int GetMoniker([MarshalAs(UnmanagedType.U4)] [In] int dwAssign, [MarshalAs(UnmanagedType.U4)] [In] int dwWhichMoniker, [MarshalAs(UnmanagedType.Interface)] out object moniker);

			// Token: 0x06001099 RID: 4249
			[PreserveSig]
			int InitFromData([MarshalAs(UnmanagedType.Interface)] [In] IDataObject pDataObject, int fCreation, [MarshalAs(UnmanagedType.U4)] [In] int dwReserved);

			// Token: 0x0600109A RID: 4250
			[PreserveSig]
			int GetClipboardData([MarshalAs(UnmanagedType.U4)] [In] int dwReserved, out IDataObject data);

			// Token: 0x0600109B RID: 4251
			[PreserveSig]
			int DoVerb(int iVerb, [In] IntPtr lpmsg, [MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IOleClientSite pActiveSite, int lindex, IntPtr hwndParent, [In] NativeMethods.COMRECT lprcPosRect);

			// Token: 0x0600109C RID: 4252
			[PreserveSig]
			int EnumVerbs(out UnsafeNativeMethods.IEnumOLEVERB e);

			// Token: 0x0600109D RID: 4253
			[PreserveSig]
			int OleUpdate();

			// Token: 0x0600109E RID: 4254
			[PreserveSig]
			int IsUpToDate();

			// Token: 0x0600109F RID: 4255
			[PreserveSig]
			int GetUserClassID([In] [Out] ref Guid pClsid);

			// Token: 0x060010A0 RID: 4256
			[PreserveSig]
			int GetUserType([MarshalAs(UnmanagedType.U4)] [In] int dwFormOfType, [MarshalAs(UnmanagedType.LPWStr)] out string userType);

			// Token: 0x060010A1 RID: 4257
			[PreserveSig]
			int SetExtent([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, [In] NativeMethods.tagSIZEL pSizel);

			// Token: 0x060010A2 RID: 4258
			[PreserveSig]
			int GetExtent([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, [Out] NativeMethods.tagSIZEL pSizel);

			// Token: 0x060010A3 RID: 4259
			[PreserveSig]
			int Advise(IAdviseSink pAdvSink, out int cookie);

			// Token: 0x060010A4 RID: 4260
			[PreserveSig]
			int Unadvise([MarshalAs(UnmanagedType.U4)] [In] int dwConnection);

			// Token: 0x060010A5 RID: 4261
			[PreserveSig]
			int EnumAdvise(out IEnumSTATDATA e);

			// Token: 0x060010A6 RID: 4262
			[PreserveSig]
			int GetMiscStatus([MarshalAs(UnmanagedType.U4)] [In] int dwAspect, out int misc);

			// Token: 0x060010A7 RID: 4263
			[PreserveSig]
			int SetColorScheme([In] NativeMethods.tagLOGPALETTE pLogpal);
		}

		// Token: 0x0200019D RID: 413
		[Guid("1C2056CC-5EF4-101B-8BC8-00AA003E3B29")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleInPlaceObjectWindowless
		{
			// Token: 0x060010A8 RID: 4264
			[PreserveSig]
			int SetClientSite([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IOleClientSite pClientSite);

			// Token: 0x060010A9 RID: 4265
			[PreserveSig]
			int GetClientSite(out UnsafeNativeMethods.IOleClientSite site);

			// Token: 0x060010AA RID: 4266
			[PreserveSig]
			int SetHostNames([MarshalAs(UnmanagedType.LPWStr)] [In] string szContainerApp, [MarshalAs(UnmanagedType.LPWStr)] [In] string szContainerObj);

			// Token: 0x060010AB RID: 4267
			[PreserveSig]
			int Close(int dwSaveOption);

			// Token: 0x060010AC RID: 4268
			[PreserveSig]
			int SetMoniker([MarshalAs(UnmanagedType.U4)] [In] int dwWhichMoniker, [MarshalAs(UnmanagedType.Interface)] [In] object pmk);

			// Token: 0x060010AD RID: 4269
			[PreserveSig]
			int GetMoniker([MarshalAs(UnmanagedType.U4)] [In] int dwAssign, [MarshalAs(UnmanagedType.U4)] [In] int dwWhichMoniker, [MarshalAs(UnmanagedType.Interface)] out object moniker);

			// Token: 0x060010AE RID: 4270
			[PreserveSig]
			int InitFromData([MarshalAs(UnmanagedType.Interface)] [In] IDataObject pDataObject, int fCreation, [MarshalAs(UnmanagedType.U4)] [In] int dwReserved);

			// Token: 0x060010AF RID: 4271
			[PreserveSig]
			int GetClipboardData([MarshalAs(UnmanagedType.U4)] [In] int dwReserved, out IDataObject data);

			// Token: 0x060010B0 RID: 4272
			[PreserveSig]
			int DoVerb(int iVerb, [In] IntPtr lpmsg, [MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IOleClientSite pActiveSite, int lindex, IntPtr hwndParent, [In] NativeMethods.COMRECT lprcPosRect);

			// Token: 0x060010B1 RID: 4273
			[PreserveSig]
			int EnumVerbs(out UnsafeNativeMethods.IEnumOLEVERB e);

			// Token: 0x060010B2 RID: 4274
			[PreserveSig]
			int OleUpdate();

			// Token: 0x060010B3 RID: 4275
			[PreserveSig]
			int IsUpToDate();

			// Token: 0x060010B4 RID: 4276
			[PreserveSig]
			int GetUserClassID([In] [Out] ref Guid pClsid);

			// Token: 0x060010B5 RID: 4277
			[PreserveSig]
			int GetUserType([MarshalAs(UnmanagedType.U4)] [In] int dwFormOfType, [MarshalAs(UnmanagedType.LPWStr)] out string userType);

			// Token: 0x060010B6 RID: 4278
			[PreserveSig]
			int SetExtent([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, [In] NativeMethods.tagSIZEL pSizel);

			// Token: 0x060010B7 RID: 4279
			[PreserveSig]
			int GetExtent([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, [Out] NativeMethods.tagSIZEL pSizel);

			// Token: 0x060010B8 RID: 4280
			[PreserveSig]
			int Advise([MarshalAs(UnmanagedType.Interface)] [In] IAdviseSink pAdvSink, out int cookie);

			// Token: 0x060010B9 RID: 4281
			[PreserveSig]
			int Unadvise([MarshalAs(UnmanagedType.U4)] [In] int dwConnection);

			// Token: 0x060010BA RID: 4282
			[PreserveSig]
			int EnumAdvise(out IEnumSTATDATA e);

			// Token: 0x060010BB RID: 4283
			[PreserveSig]
			int GetMiscStatus([MarshalAs(UnmanagedType.U4)] [In] int dwAspect, out int misc);

			// Token: 0x060010BC RID: 4284
			[PreserveSig]
			int SetColorScheme([In] NativeMethods.tagLOGPALETTE pLogpal);

			// Token: 0x060010BD RID: 4285
			[PreserveSig]
			int OnWindowMessage([MarshalAs(UnmanagedType.U4)] [In] int msg, [MarshalAs(UnmanagedType.U4)] [In] int wParam, [MarshalAs(UnmanagedType.U4)] [In] int lParam, [MarshalAs(UnmanagedType.U4)] [Out] int plResult);

			// Token: 0x060010BE RID: 4286
			[PreserveSig]
			int GetDropTarget([MarshalAs(UnmanagedType.Interface)] [Out] object ppDropTarget);
		}

		// Token: 0x0200019E RID: 414
		[SuppressUnmanagedCodeSecurity]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("B196B288-BAB4-101A-B69C-00AA00341D07")]
		[ComImport]
		public interface IOleControl
		{
			// Token: 0x060010BF RID: 4287
			[PreserveSig]
			int GetControlInfo([Out] NativeMethods.tagCONTROLINFO pCI);

			// Token: 0x060010C0 RID: 4288
			[PreserveSig]
			int OnMnemonic([In] ref NativeMethods.MSG pMsg);

			// Token: 0x060010C1 RID: 4289
			[PreserveSig]
			int OnAmbientPropertyChange(int dispID);

			// Token: 0x060010C2 RID: 4290
			[PreserveSig]
			int FreezeEvents(int bFreeze);
		}

		// Token: 0x0200019F RID: 415
		[Guid("6D5140C1-7436-11CE-8034-00AA006009FA")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleServiceProvider
		{
			// Token: 0x060010C3 RID: 4291
			[PreserveSig]
			int QueryService([In] ref Guid guidService, [In] ref Guid riid, out IntPtr ppvObject);
		}

		// Token: 0x020001A0 RID: 416
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("0000010d-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IViewObject
		{
			// Token: 0x060010C4 RID: 4292
			[PreserveSig]
			int Draw([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, int lindex, IntPtr pvAspect, [In] NativeMethods.tagDVTARGETDEVICE ptd, IntPtr hdcTargetDev, IntPtr hdcDraw, [In] NativeMethods.COMRECT lprcBounds, [In] NativeMethods.COMRECT lprcWBounds, IntPtr pfnContinue, [In] int dwContinue);

			// Token: 0x060010C5 RID: 4293
			[PreserveSig]
			int GetColorSet([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, int lindex, IntPtr pvAspect, [In] NativeMethods.tagDVTARGETDEVICE ptd, IntPtr hicTargetDev, [Out] NativeMethods.tagLOGPALETTE ppColorSet);

			// Token: 0x060010C6 RID: 4294
			[PreserveSig]
			int Freeze([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, int lindex, IntPtr pvAspect, [Out] IntPtr pdwFreeze);

			// Token: 0x060010C7 RID: 4295
			[PreserveSig]
			int Unfreeze([MarshalAs(UnmanagedType.U4)] [In] int dwFreeze);

			// Token: 0x060010C8 RID: 4296
			void SetAdvise([MarshalAs(UnmanagedType.U4)] [In] int aspects, [MarshalAs(UnmanagedType.U4)] [In] int advf, [MarshalAs(UnmanagedType.Interface)] [In] IAdviseSink pAdvSink);

			// Token: 0x060010C9 RID: 4297
			void GetAdvise([MarshalAs(UnmanagedType.LPArray)] [In] [Out] int[] paspects, [MarshalAs(UnmanagedType.LPArray)] [In] [Out] int[] advf, [MarshalAs(UnmanagedType.LPArray)] [In] [Out] IAdviseSink[] pAdvSink);
		}

		// Token: 0x020001A1 RID: 417
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("00000127-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IViewObject2
		{
			// Token: 0x060010CA RID: 4298
			void Draw([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, int lindex, IntPtr pvAspect, [In] NativeMethods.tagDVTARGETDEVICE ptd, IntPtr hdcTargetDev, IntPtr hdcDraw, [In] NativeMethods.COMRECT lprcBounds, [In] NativeMethods.COMRECT lprcWBounds, IntPtr pfnContinue, [In] int dwContinue);

			// Token: 0x060010CB RID: 4299
			[PreserveSig]
			int GetColorSet([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, int lindex, IntPtr pvAspect, [In] NativeMethods.tagDVTARGETDEVICE ptd, IntPtr hicTargetDev, [Out] NativeMethods.tagLOGPALETTE ppColorSet);

			// Token: 0x060010CC RID: 4300
			[PreserveSig]
			int Freeze([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, int lindex, IntPtr pvAspect, [Out] IntPtr pdwFreeze);

			// Token: 0x060010CD RID: 4301
			[PreserveSig]
			int Unfreeze([MarshalAs(UnmanagedType.U4)] [In] int dwFreeze);

			// Token: 0x060010CE RID: 4302
			void SetAdvise([MarshalAs(UnmanagedType.U4)] [In] int aspects, [MarshalAs(UnmanagedType.U4)] [In] int advf, [MarshalAs(UnmanagedType.Interface)] [In] IAdviseSink pAdvSink);

			// Token: 0x060010CF RID: 4303
			void GetAdvise([MarshalAs(UnmanagedType.LPArray)] [In] [Out] int[] paspects, [MarshalAs(UnmanagedType.LPArray)] [In] [Out] int[] advf, [MarshalAs(UnmanagedType.LPArray)] [In] [Out] IAdviseSink[] pAdvSink);

			// Token: 0x060010D0 RID: 4304
			void GetExtent([MarshalAs(UnmanagedType.U4)] [In] int dwDrawAspect, int lindex, [In] NativeMethods.tagDVTARGETDEVICE ptd, [Out] NativeMethods.tagSIZEL lpsizel);
		}

		// Token: 0x020001A2 RID: 418
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("0000010C-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IPersist
		{
			// Token: 0x060010D1 RID: 4305
			[SuppressUnmanagedCodeSecurity]
			void GetClassID(out Guid pClassID);
		}

		// Token: 0x020001A3 RID: 419
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("37D84F60-42CB-11CE-8135-00AA004BB851")]
		[ComImport]
		public interface IPersistPropertyBag
		{
			// Token: 0x060010D2 RID: 4306
			void GetClassID(out Guid pClassID);

			// Token: 0x060010D3 RID: 4307
			void InitNew();

			// Token: 0x060010D4 RID: 4308
			void Load([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IPropertyBag pPropBag, [MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IErrorLog pErrorLog);

			// Token: 0x060010D5 RID: 4309
			void Save([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IPropertyBag pPropBag, [MarshalAs(UnmanagedType.Bool)] [In] bool fClearDirty, [MarshalAs(UnmanagedType.Bool)] [In] bool fSaveAllProperties);
		}

		// Token: 0x020001A4 RID: 420
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("CF51ED10-62FE-11CF-BF86-00A0C9034836")]
		[ComImport]
		public interface IQuickActivate
		{
			// Token: 0x060010D6 RID: 4310
			void QuickActivate([In] UnsafeNativeMethods.tagQACONTAINER pQaContainer, [Out] UnsafeNativeMethods.tagQACONTROL pQaControl);

			// Token: 0x060010D7 RID: 4311
			void SetContentExtent([In] NativeMethods.tagSIZEL pSizel);

			// Token: 0x060010D8 RID: 4312
			void GetContentExtent([Out] NativeMethods.tagSIZEL pSizel);
		}

		// Token: 0x020001A5 RID: 421
		[Guid("000C060B-0000-0000-C000-000000000046")]
		[ComImport]
		public class SMsoComponentManager
		{
			// Token: 0x060010D9 RID: 4313
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			public extern SMsoComponentManager();
		}

		// Token: 0x020001A6 RID: 422
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("55272A00-42CB-11CE-8135-00AA004BB851")]
		[ComImport]
		public interface IPropertyBag
		{
			// Token: 0x060010DA RID: 4314
			[PreserveSig]
			int Read([MarshalAs(UnmanagedType.LPWStr)] [In] string pszPropName, [In] [Out] ref object pVar, [In] UnsafeNativeMethods.IErrorLog pErrorLog);

			// Token: 0x060010DB RID: 4315
			[PreserveSig]
			int Write([MarshalAs(UnmanagedType.LPWStr)] [In] string pszPropName, [In] ref object pVar);
		}

		// Token: 0x020001A7 RID: 423
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("3127CA40-446E-11CE-8135-00AA004BB851")]
		[ComImport]
		public interface IErrorLog
		{
			// Token: 0x060010DC RID: 4316
			void AddError([MarshalAs(UnmanagedType.LPWStr)] [In] string pszPropName_p0, [MarshalAs(UnmanagedType.Struct)] [In] NativeMethods.tagEXCEPINFO pExcepInfo_p1);
		}

		// Token: 0x020001A8 RID: 424
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("00000109-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IPersistStream
		{
			// Token: 0x060010DD RID: 4317
			void GetClassID(out Guid pClassId);

			// Token: 0x060010DE RID: 4318
			[PreserveSig]
			int IsDirty();

			// Token: 0x060010DF RID: 4319
			void Load([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStream pstm);

			// Token: 0x060010E0 RID: 4320
			void Save([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStream pstm, [MarshalAs(UnmanagedType.Bool)] [In] bool fClearDirty);

			// Token: 0x060010E1 RID: 4321
			long GetSizeMax();
		}

		// Token: 0x020001A9 RID: 425
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[SuppressUnmanagedCodeSecurity]
		[Guid("7FD52380-4E07-101B-AE2D-08002B2EC713")]
		[ComImport]
		public interface IPersistStreamInit
		{
			// Token: 0x060010E2 RID: 4322
			void GetClassID(out Guid pClassID);

			// Token: 0x060010E3 RID: 4323
			[PreserveSig]
			int IsDirty();

			// Token: 0x060010E4 RID: 4324
			void Load([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStream pstm);

			// Token: 0x060010E5 RID: 4325
			void Save([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStream pstm, [MarshalAs(UnmanagedType.Bool)] [In] bool fClearDirty);

			// Token: 0x060010E6 RID: 4326
			void GetSizeMax([MarshalAs(UnmanagedType.LPArray)] [Out] long pcbSize);

			// Token: 0x060010E7 RID: 4327
			void InitNew();
		}

		// Token: 0x020001AA RID: 426
		[SuppressUnmanagedCodeSecurity]
		[Guid("B196B286-BAB4-101A-B69C-00AA00341D07")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IConnectionPoint
		{
			// Token: 0x060010E8 RID: 4328
			[PreserveSig]
			int GetConnectionInterface(out Guid iid);

			// Token: 0x060010E9 RID: 4329
			[PreserveSig]
			int GetConnectionPointContainer([MarshalAs(UnmanagedType.Interface)] ref UnsafeNativeMethods.IConnectionPointContainer pContainer);

			// Token: 0x060010EA RID: 4330
			[PreserveSig]
			int Advise([MarshalAs(UnmanagedType.Interface)] [In] object pUnkSink, ref int cookie);

			// Token: 0x060010EB RID: 4331
			[PreserveSig]
			int Unadvise(int cookie);

			// Token: 0x060010EC RID: 4332
			[PreserveSig]
			int EnumConnections(out object pEnum);
		}

		// Token: 0x020001AB RID: 427
		[Guid("0000010A-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IPersistStorage
		{
			// Token: 0x060010ED RID: 4333
			void GetClassID(out Guid pClassID);

			// Token: 0x060010EE RID: 4334
			[PreserveSig]
			int IsDirty();

			// Token: 0x060010EF RID: 4335
			void InitNew(UnsafeNativeMethods.IStorage pstg);

			// Token: 0x060010F0 RID: 4336
			[PreserveSig]
			int Load(UnsafeNativeMethods.IStorage pstg);

			// Token: 0x060010F1 RID: 4337
			void Save(UnsafeNativeMethods.IStorage pStgSave, bool fSameAsLoad);

			// Token: 0x060010F2 RID: 4338
			void SaveCompleted(UnsafeNativeMethods.IStorage pStgNew);

			// Token: 0x060010F3 RID: 4339
			void HandsOffStorage();
		}

		// Token: 0x020001AC RID: 428
		[Guid("00020404-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IEnumVariant
		{
			// Token: 0x060010F4 RID: 4340
			[PreserveSig]
			int Next([MarshalAs(UnmanagedType.U4)] [In] int celt, [In] [Out] IntPtr rgvar, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pceltFetched);

			// Token: 0x060010F5 RID: 4341
			void Skip([MarshalAs(UnmanagedType.U4)] [In] int celt);

			// Token: 0x060010F6 RID: 4342
			void Reset();

			// Token: 0x060010F7 RID: 4343
			void Clone([MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.IEnumVariant[] ppenum);
		}

		// Token: 0x020001AD RID: 429
		[Guid("00000104-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IEnumOLEVERB
		{
			// Token: 0x060010F8 RID: 4344
			[PreserveSig]
			int Next([MarshalAs(UnmanagedType.U4)] int celt, [Out] NativeMethods.tagOLEVERB rgelt, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pceltFetched);

			// Token: 0x060010F9 RID: 4345
			[PreserveSig]
			int Skip([MarshalAs(UnmanagedType.U4)] [In] int celt);

			// Token: 0x060010FA RID: 4346
			void Reset();

			// Token: 0x060010FB RID: 4347
			void Clone(out UnsafeNativeMethods.IEnumOLEVERB ppenum);
		}

		// Token: 0x020001AE RID: 430
		[Guid("00bb2762-6a77-11d0-a535-00c04fd7d062")]
		[SuppressUnmanagedCodeSecurity]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IAutoComplete
		{
			// Token: 0x060010FC RID: 4348
			int Init([In] HandleRef hwndEdit, [In] IEnumString punkACL, [In] string pwszRegKeyPath, [In] string pwszQuickComplete);

			// Token: 0x060010FD RID: 4349
			void Enable([In] bool fEnable);
		}

		// Token: 0x020001AF RID: 431
		[Guid("EAC04BC0-3791-11d2-BB95-0060977B464C")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[SuppressUnmanagedCodeSecurity]
		[ComImport]
		public interface IAutoComplete2
		{
			// Token: 0x060010FE RID: 4350
			int Init([In] HandleRef hwndEdit, [In] IEnumString punkACL, [In] string pwszRegKeyPath, [In] string pwszQuickComplete);

			// Token: 0x060010FF RID: 4351
			void Enable([In] bool fEnable);

			// Token: 0x06001100 RID: 4352
			int SetOptions([In] int dwFlag);

			// Token: 0x06001101 RID: 4353
			void GetOptions([Out] IntPtr pdwFlag);
		}

		// Token: 0x020001B0 RID: 432
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("0000000C-0000-0000-C000-000000000046")]
		[SuppressUnmanagedCodeSecurity]
		[ComImport]
		public interface IStream
		{
			// Token: 0x06001102 RID: 4354
			int Read(IntPtr buf, int len);

			// Token: 0x06001103 RID: 4355
			int Write(IntPtr buf, int len);

			// Token: 0x06001104 RID: 4356
			[return: MarshalAs(UnmanagedType.I8)]
			long Seek([MarshalAs(UnmanagedType.I8)] [In] long dlibMove, int dwOrigin);

			// Token: 0x06001105 RID: 4357
			void SetSize([MarshalAs(UnmanagedType.I8)] [In] long libNewSize);

			// Token: 0x06001106 RID: 4358
			[return: MarshalAs(UnmanagedType.I8)]
			long CopyTo([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStream pstm, [MarshalAs(UnmanagedType.I8)] [In] long cb, [MarshalAs(UnmanagedType.LPArray)] [Out] long[] pcbRead);

			// Token: 0x06001107 RID: 4359
			void Commit(int grfCommitFlags);

			// Token: 0x06001108 RID: 4360
			void Revert();

			// Token: 0x06001109 RID: 4361
			void LockRegion([MarshalAs(UnmanagedType.I8)] [In] long libOffset, [MarshalAs(UnmanagedType.I8)] [In] long cb, int dwLockType);

			// Token: 0x0600110A RID: 4362
			void UnlockRegion([MarshalAs(UnmanagedType.I8)] [In] long libOffset, [MarshalAs(UnmanagedType.I8)] [In] long cb, int dwLockType);

			// Token: 0x0600110B RID: 4363
			void Stat([Out] NativeMethods.STATSTG pStatstg, int grfStatFlag);

			// Token: 0x0600110C RID: 4364
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IStream Clone();
		}

		// Token: 0x020001B1 RID: 433
		public abstract class CharBuffer
		{
			// Token: 0x0600110D RID: 4365 RVA: 0x0000E978 File Offset: 0x0000D978
			public static UnsafeNativeMethods.CharBuffer CreateBuffer(int size)
			{
				if (Marshal.SystemDefaultCharSize == 1)
				{
					return new UnsafeNativeMethods.AnsiCharBuffer(size);
				}
				return new UnsafeNativeMethods.UnicodeCharBuffer(size);
			}

			// Token: 0x0600110E RID: 4366
			public abstract IntPtr AllocCoTaskMem();

			// Token: 0x0600110F RID: 4367
			public abstract string GetString();

			// Token: 0x06001110 RID: 4368
			public abstract void PutCoTaskMem(IntPtr ptr);

			// Token: 0x06001111 RID: 4369
			public abstract void PutString(string s);
		}

		// Token: 0x020001B2 RID: 434
		public class AnsiCharBuffer : UnsafeNativeMethods.CharBuffer
		{
			// Token: 0x06001113 RID: 4371 RVA: 0x0000E997 File Offset: 0x0000D997
			public AnsiCharBuffer(int size)
			{
				this.buffer = new byte[size];
			}

			// Token: 0x06001114 RID: 4372 RVA: 0x0000E9AC File Offset: 0x0000D9AC
			public override IntPtr AllocCoTaskMem()
			{
				IntPtr intPtr = Marshal.AllocCoTaskMem(this.buffer.Length);
				Marshal.Copy(this.buffer, 0, intPtr, this.buffer.Length);
				return intPtr;
			}

			// Token: 0x06001115 RID: 4373 RVA: 0x0000E9E0 File Offset: 0x0000D9E0
			public override string GetString()
			{
				int num = this.offset;
				while (num < this.buffer.Length && this.buffer[num] != 0)
				{
					num++;
				}
				string @string = Encoding.Default.GetString(this.buffer, this.offset, num - this.offset);
				if (num < this.buffer.Length)
				{
					num++;
				}
				this.offset = num;
				return @string;
			}

			// Token: 0x06001116 RID: 4374 RVA: 0x0000EA45 File Offset: 0x0000DA45
			public override void PutCoTaskMem(IntPtr ptr)
			{
				Marshal.Copy(ptr, this.buffer, 0, this.buffer.Length);
				this.offset = 0;
			}

			// Token: 0x06001117 RID: 4375 RVA: 0x0000EA64 File Offset: 0x0000DA64
			public override void PutString(string s)
			{
				byte[] bytes = Encoding.Default.GetBytes(s);
				int num = Math.Min(bytes.Length, this.buffer.Length - this.offset);
				Array.Copy(bytes, 0, this.buffer, this.offset, num);
				this.offset += num;
				if (this.offset < this.buffer.Length)
				{
					this.buffer[this.offset++] = 0;
				}
			}

			// Token: 0x04000F66 RID: 3942
			internal byte[] buffer;

			// Token: 0x04000F67 RID: 3943
			internal int offset;
		}

		// Token: 0x020001B3 RID: 435
		public class UnicodeCharBuffer : UnsafeNativeMethods.CharBuffer
		{
			// Token: 0x06001118 RID: 4376 RVA: 0x0000EAE0 File Offset: 0x0000DAE0
			public UnicodeCharBuffer(int size)
			{
				this.buffer = new char[size];
			}

			// Token: 0x06001119 RID: 4377 RVA: 0x0000EAF4 File Offset: 0x0000DAF4
			public override IntPtr AllocCoTaskMem()
			{
				IntPtr intPtr = Marshal.AllocCoTaskMem(this.buffer.Length * 2);
				Marshal.Copy(this.buffer, 0, intPtr, this.buffer.Length);
				return intPtr;
			}

			// Token: 0x0600111A RID: 4378 RVA: 0x0000EB28 File Offset: 0x0000DB28
			public override string GetString()
			{
				int num = this.offset;
				while (num < this.buffer.Length && this.buffer[num] != '\0')
				{
					num++;
				}
				string text = new string(this.buffer, this.offset, num - this.offset);
				if (num < this.buffer.Length)
				{
					num++;
				}
				this.offset = num;
				return text;
			}

			// Token: 0x0600111B RID: 4379 RVA: 0x0000EB88 File Offset: 0x0000DB88
			public override void PutCoTaskMem(IntPtr ptr)
			{
				Marshal.Copy(ptr, this.buffer, 0, this.buffer.Length);
				this.offset = 0;
			}

			// Token: 0x0600111C RID: 4380 RVA: 0x0000EBA8 File Offset: 0x0000DBA8
			public override void PutString(string s)
			{
				int num = Math.Min(s.Length, this.buffer.Length - this.offset);
				s.CopyTo(0, this.buffer, this.offset, num);
				this.offset += num;
				if (this.offset < this.buffer.Length)
				{
					this.buffer[this.offset++] = '\0';
				}
			}

			// Token: 0x04000F68 RID: 3944
			internal char[] buffer;

			// Token: 0x04000F69 RID: 3945
			internal int offset;
		}

		// Token: 0x020001B4 RID: 436
		public class ComStreamFromDataStream : UnsafeNativeMethods.IStream
		{
			// Token: 0x0600111D RID: 4381 RVA: 0x0000EC1B File Offset: 0x0000DC1B
			public ComStreamFromDataStream(Stream dataStream)
			{
				if (dataStream == null)
				{
					throw new ArgumentNullException("dataStream");
				}
				this.dataStream = dataStream;
			}

			// Token: 0x0600111E RID: 4382 RVA: 0x0000EC40 File Offset: 0x0000DC40
			private void ActualizeVirtualPosition()
			{
				if (this.virtualPosition == -1L)
				{
					return;
				}
				if (this.virtualPosition > this.dataStream.Length)
				{
					this.dataStream.SetLength(this.virtualPosition);
				}
				this.dataStream.Position = this.virtualPosition;
				this.virtualPosition = -1L;
			}

			// Token: 0x0600111F RID: 4383 RVA: 0x0000EC95 File Offset: 0x0000DC95
			public UnsafeNativeMethods.IStream Clone()
			{
				UnsafeNativeMethods.ComStreamFromDataStream.NotImplemented();
				return null;
			}

			// Token: 0x06001120 RID: 4384 RVA: 0x0000EC9D File Offset: 0x0000DC9D
			public void Commit(int grfCommitFlags)
			{
				this.dataStream.Flush();
				this.ActualizeVirtualPosition();
			}

			// Token: 0x06001121 RID: 4385 RVA: 0x0000ECB0 File Offset: 0x0000DCB0
			public long CopyTo(UnsafeNativeMethods.IStream pstm, long cb, long[] pcbRead)
			{
				int num = 4096;
				IntPtr intPtr = Marshal.AllocHGlobal(num);
				if (intPtr == IntPtr.Zero)
				{
					throw new OutOfMemoryException();
				}
				long num2 = 0L;
				try
				{
					while (num2 < cb)
					{
						int num3 = num;
						if (num2 + (long)num3 > cb)
						{
							num3 = (int)(cb - num2);
						}
						int num4 = this.Read(intPtr, num3);
						if (num4 == 0)
						{
							break;
						}
						if (pstm.Write(intPtr, num4) != num4)
						{
							throw UnsafeNativeMethods.ComStreamFromDataStream.EFail("Wrote an incorrect number of bytes");
						}
						num2 += (long)num4;
					}
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
				}
				if (pcbRead != null && pcbRead.Length > 0)
				{
					pcbRead[0] = num2;
				}
				return num2;
			}

			// Token: 0x06001122 RID: 4386 RVA: 0x0000ED48 File Offset: 0x0000DD48
			public Stream GetDataStream()
			{
				return this.dataStream;
			}

			// Token: 0x06001123 RID: 4387 RVA: 0x0000ED50 File Offset: 0x0000DD50
			public void LockRegion(long libOffset, long cb, int dwLockType)
			{
			}

			// Token: 0x06001124 RID: 4388 RVA: 0x0000ED54 File Offset: 0x0000DD54
			protected static ExternalException EFail(string msg)
			{
				ExternalException ex = new ExternalException(msg, -2147467259);
				throw ex;
			}

			// Token: 0x06001125 RID: 4389 RVA: 0x0000ED70 File Offset: 0x0000DD70
			protected static void NotImplemented()
			{
				ExternalException ex = new ExternalException(SR.GetString("UnsafeNativeMethodsNotImplemented"), -2147467263);
				throw ex;
			}

			// Token: 0x06001126 RID: 4390 RVA: 0x0000ED94 File Offset: 0x0000DD94
			public int Read(IntPtr buf, int length)
			{
				byte[] array = new byte[length];
				int num = this.Read(array, length);
				Marshal.Copy(array, 0, buf, num);
				return num;
			}

			// Token: 0x06001127 RID: 4391 RVA: 0x0000EDBB File Offset: 0x0000DDBB
			public int Read(byte[] buffer, int length)
			{
				this.ActualizeVirtualPosition();
				return this.dataStream.Read(buffer, 0, length);
			}

			// Token: 0x06001128 RID: 4392 RVA: 0x0000EDD1 File Offset: 0x0000DDD1
			public void Revert()
			{
				UnsafeNativeMethods.ComStreamFromDataStream.NotImplemented();
			}

			// Token: 0x06001129 RID: 4393 RVA: 0x0000EDD8 File Offset: 0x0000DDD8
			public long Seek(long offset, int origin)
			{
				long position = this.virtualPosition;
				if (this.virtualPosition == -1L)
				{
					position = this.dataStream.Position;
				}
				long length = this.dataStream.Length;
				switch (origin)
				{
				case 0:
					if (offset <= length)
					{
						this.dataStream.Position = offset;
						this.virtualPosition = -1L;
					}
					else
					{
						this.virtualPosition = offset;
					}
					break;
				case 1:
					if (offset + position <= length)
					{
						this.dataStream.Position = position + offset;
						this.virtualPosition = -1L;
					}
					else
					{
						this.virtualPosition = offset + position;
					}
					break;
				case 2:
					if (offset <= 0L)
					{
						this.dataStream.Position = length + offset;
						this.virtualPosition = -1L;
					}
					else
					{
						this.virtualPosition = length + offset;
					}
					break;
				}
				if (this.virtualPosition != -1L)
				{
					return this.virtualPosition;
				}
				return this.dataStream.Position;
			}

			// Token: 0x0600112A RID: 4394 RVA: 0x0000EEB2 File Offset: 0x0000DEB2
			public void SetSize(long value)
			{
				this.dataStream.SetLength(value);
			}

			// Token: 0x0600112B RID: 4395 RVA: 0x0000EEC0 File Offset: 0x0000DEC0
			public void Stat(NativeMethods.STATSTG pstatstg, int grfStatFlag)
			{
				pstatstg.type = 2;
				pstatstg.cbSize = this.dataStream.Length;
				pstatstg.grfLocksSupported = 2;
			}

			// Token: 0x0600112C RID: 4396 RVA: 0x0000EEE1 File Offset: 0x0000DEE1
			public void UnlockRegion(long libOffset, long cb, int dwLockType)
			{
			}

			// Token: 0x0600112D RID: 4397 RVA: 0x0000EEE4 File Offset: 0x0000DEE4
			public int Write(IntPtr buf, int length)
			{
				byte[] array = new byte[length];
				Marshal.Copy(buf, array, 0, length);
				return this.Write(array, length);
			}

			// Token: 0x0600112E RID: 4398 RVA: 0x0000EF09 File Offset: 0x0000DF09
			public int Write(byte[] buffer, int length)
			{
				this.ActualizeVirtualPosition();
				this.dataStream.Write(buffer, 0, length);
				return length;
			}

			// Token: 0x04000F6A RID: 3946
			protected Stream dataStream;

			// Token: 0x04000F6B RID: 3947
			private long virtualPosition = -1L;
		}

		// Token: 0x020001B5 RID: 437
		[Guid("0000000B-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IStorage
		{
			// Token: 0x0600112F RID: 4399
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IStream CreateStream([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName, [MarshalAs(UnmanagedType.U4)] [In] int grfMode, [MarshalAs(UnmanagedType.U4)] [In] int reserved1, [MarshalAs(UnmanagedType.U4)] [In] int reserved2);

			// Token: 0x06001130 RID: 4400
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IStream OpenStream([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName, IntPtr reserved1, [MarshalAs(UnmanagedType.U4)] [In] int grfMode, [MarshalAs(UnmanagedType.U4)] [In] int reserved2);

			// Token: 0x06001131 RID: 4401
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IStorage CreateStorage([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName, [MarshalAs(UnmanagedType.U4)] [In] int grfMode, [MarshalAs(UnmanagedType.U4)] [In] int reserved1, [MarshalAs(UnmanagedType.U4)] [In] int reserved2);

			// Token: 0x06001132 RID: 4402
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IStorage OpenStorage([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName, IntPtr pstgPriority, [MarshalAs(UnmanagedType.U4)] [In] int grfMode, IntPtr snbExclude, [MarshalAs(UnmanagedType.U4)] [In] int reserved);

			// Token: 0x06001133 RID: 4403
			void CopyTo(int ciidExclude, [MarshalAs(UnmanagedType.LPArray)] [In] Guid[] pIIDExclude, IntPtr snbExclude, [MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStorage stgDest);

			// Token: 0x06001134 RID: 4404
			void MoveElementTo([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName, [MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStorage stgDest, [MarshalAs(UnmanagedType.BStr)] [In] string pwcsNewName, [MarshalAs(UnmanagedType.U4)] [In] int grfFlags);

			// Token: 0x06001135 RID: 4405
			void Commit(int grfCommitFlags);

			// Token: 0x06001136 RID: 4406
			void Revert();

			// Token: 0x06001137 RID: 4407
			void EnumElements([MarshalAs(UnmanagedType.U4)] [In] int reserved1, IntPtr reserved2, [MarshalAs(UnmanagedType.U4)] [In] int reserved3, [MarshalAs(UnmanagedType.Interface)] out object ppVal);

			// Token: 0x06001138 RID: 4408
			void DestroyElement([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName);

			// Token: 0x06001139 RID: 4409
			void RenameElement([MarshalAs(UnmanagedType.BStr)] [In] string pwcsOldName, [MarshalAs(UnmanagedType.BStr)] [In] string pwcsNewName);

			// Token: 0x0600113A RID: 4410
			void SetElementTimes([MarshalAs(UnmanagedType.BStr)] [In] string pwcsName, [In] NativeMethods.FILETIME pctime, [In] NativeMethods.FILETIME patime, [In] NativeMethods.FILETIME pmtime);

			// Token: 0x0600113B RID: 4411
			void SetClass([In] ref Guid clsid);

			// Token: 0x0600113C RID: 4412
			void SetStateBits(int grfStateBits, int grfMask);

			// Token: 0x0600113D RID: 4413
			void Stat([Out] NativeMethods.STATSTG pStatStg, int grfStatFlag);
		}

		// Token: 0x020001B6 RID: 438
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("B196B28F-BAB4-101A-B69C-00AA00341D07")]
		[ComImport]
		public interface IClassFactory2
		{
			// Token: 0x0600113E RID: 4414
			void CreateInstance([MarshalAs(UnmanagedType.Interface)] [In] object unused, [In] ref Guid refiid, [MarshalAs(UnmanagedType.LPArray)] [Out] object[] ppunk);

			// Token: 0x0600113F RID: 4415
			void LockServer(int fLock);

			// Token: 0x06001140 RID: 4416
			void GetLicInfo([Out] NativeMethods.tagLICINFO licInfo);

			// Token: 0x06001141 RID: 4417
			void RequestLicKey([MarshalAs(UnmanagedType.U4)] [In] int dwReserved, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] pBstrKey);

			// Token: 0x06001142 RID: 4418
			void CreateInstanceLic([MarshalAs(UnmanagedType.Interface)] [In] object pUnkOuter, [MarshalAs(UnmanagedType.Interface)] [In] object pUnkReserved, [In] ref Guid riid, [MarshalAs(UnmanagedType.BStr)] [In] string bstrKey, [MarshalAs(UnmanagedType.Interface)] out object ppVal);
		}

		// Token: 0x020001B7 RID: 439
		[Guid("B196B284-BAB4-101A-B69C-00AA00341D07")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[SuppressUnmanagedCodeSecurity]
		[ComImport]
		public interface IConnectionPointContainer
		{
			// Token: 0x06001143 RID: 4419
			[return: MarshalAs(UnmanagedType.Interface)]
			object EnumConnectionPoints();

			// Token: 0x06001144 RID: 4420
			[PreserveSig]
			int FindConnectionPoint([In] ref Guid guid, [MarshalAs(UnmanagedType.Interface)] out UnsafeNativeMethods.IConnectionPoint ppCP);
		}

		// Token: 0x020001B8 RID: 440
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("B196B285-BAB4-101A-B69C-00AA00341D07")]
		[ComImport]
		public interface IEnumConnectionPoints
		{
			// Token: 0x06001145 RID: 4421
			[PreserveSig]
			int Next(int cConnections, out UnsafeNativeMethods.IConnectionPoint pCp, out int pcFetched);

			// Token: 0x06001146 RID: 4422
			[PreserveSig]
			int Skip(int cSkip);

			// Token: 0x06001147 RID: 4423
			void Reset();

			// Token: 0x06001148 RID: 4424
			UnsafeNativeMethods.IEnumConnectionPoints Clone();
		}

		// Token: 0x020001B9 RID: 441
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("00020400-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IDispatch
		{
			// Token: 0x06001149 RID: 4425
			int GetTypeInfoCount();

			// Token: 0x0600114A RID: 4426
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.ITypeInfo GetTypeInfo([MarshalAs(UnmanagedType.U4)] [In] int iTInfo, [MarshalAs(UnmanagedType.U4)] [In] int lcid);

			// Token: 0x0600114B RID: 4427
			[PreserveSig]
			int GetIDsOfNames([In] ref Guid riid, [MarshalAs(UnmanagedType.LPArray)] [In] string[] rgszNames, [MarshalAs(UnmanagedType.U4)] [In] int cNames, [MarshalAs(UnmanagedType.U4)] [In] int lcid, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] rgDispId);

			// Token: 0x0600114C RID: 4428
			[PreserveSig]
			int Invoke(int dispIdMember, [In] ref Guid riid, [MarshalAs(UnmanagedType.U4)] [In] int lcid, [MarshalAs(UnmanagedType.U4)] [In] int dwFlags, [In] [Out] NativeMethods.tagDISPPARAMS pDispParams, [MarshalAs(UnmanagedType.LPArray)] [Out] object[] pVarResult, [In] [Out] NativeMethods.tagEXCEPINFO pExcepInfo, [MarshalAs(UnmanagedType.LPArray)] [Out] IntPtr[] pArgErr);
		}

		// Token: 0x020001BA RID: 442
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("00020401-0000-0000-C000-000000000046")]
		[ComImport]
		public interface ITypeInfo
		{
			// Token: 0x0600114D RID: 4429
			[PreserveSig]
			int GetTypeAttr(ref IntPtr pTypeAttr);

			// Token: 0x0600114E RID: 4430
			[PreserveSig]
			int GetTypeComp([MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.ITypeComp[] ppTComp);

			// Token: 0x0600114F RID: 4431
			[PreserveSig]
			int GetFuncDesc([MarshalAs(UnmanagedType.U4)] [In] int index, ref IntPtr pFuncDesc);

			// Token: 0x06001150 RID: 4432
			[PreserveSig]
			int GetVarDesc([MarshalAs(UnmanagedType.U4)] [In] int index, ref IntPtr pVarDesc);

			// Token: 0x06001151 RID: 4433
			[PreserveSig]
			int GetNames(int memid, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] rgBstrNames, [MarshalAs(UnmanagedType.U4)] [In] int cMaxNames, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pcNames);

			// Token: 0x06001152 RID: 4434
			[PreserveSig]
			int GetRefTypeOfImplType([MarshalAs(UnmanagedType.U4)] [In] int index, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pRefType);

			// Token: 0x06001153 RID: 4435
			[PreserveSig]
			int GetImplTypeFlags([MarshalAs(UnmanagedType.U4)] [In] int index, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pImplTypeFlags);

			// Token: 0x06001154 RID: 4436
			[PreserveSig]
			int GetIDsOfNames(IntPtr rgszNames, int cNames, IntPtr pMemId);

			// Token: 0x06001155 RID: 4437
			[PreserveSig]
			int Invoke();

			// Token: 0x06001156 RID: 4438
			[PreserveSig]
			int GetDocumentation(int memid, ref string pBstrName, ref string pBstrDocString, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pdwHelpContext, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] pBstrHelpFile);

			// Token: 0x06001157 RID: 4439
			[PreserveSig]
			int GetDllEntry(int memid, NativeMethods.tagINVOKEKIND invkind, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] pBstrDllName, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] pBstrName, [MarshalAs(UnmanagedType.LPArray)] [Out] short[] pwOrdinal);

			// Token: 0x06001158 RID: 4440
			[PreserveSig]
			int GetRefTypeInfo(IntPtr hreftype, ref UnsafeNativeMethods.ITypeInfo pTypeInfo);

			// Token: 0x06001159 RID: 4441
			[PreserveSig]
			int AddressOfMember();

			// Token: 0x0600115A RID: 4442
			[PreserveSig]
			int CreateInstance([In] ref Guid riid, [MarshalAs(UnmanagedType.LPArray)] [Out] object[] ppvObj);

			// Token: 0x0600115B RID: 4443
			[PreserveSig]
			int GetMops(int memid, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] pBstrMops);

			// Token: 0x0600115C RID: 4444
			[PreserveSig]
			int GetContainingTypeLib([MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.ITypeLib[] ppTLib, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pIndex);

			// Token: 0x0600115D RID: 4445
			[PreserveSig]
			void ReleaseTypeAttr(IntPtr typeAttr);

			// Token: 0x0600115E RID: 4446
			[PreserveSig]
			void ReleaseFuncDesc(IntPtr funcDesc);

			// Token: 0x0600115F RID: 4447
			[PreserveSig]
			void ReleaseVarDesc(IntPtr varDesc);
		}

		// Token: 0x020001BB RID: 443
		[Guid("00020403-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface ITypeComp
		{
			// Token: 0x06001160 RID: 4448
			void RemoteBind([MarshalAs(UnmanagedType.LPWStr)] [In] string szName, [MarshalAs(UnmanagedType.U4)] [In] int lHashVal, [MarshalAs(UnmanagedType.U2)] [In] short wFlags, [MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.ITypeInfo[] ppTInfo, [MarshalAs(UnmanagedType.LPArray)] [Out] NativeMethods.tagDESCKIND[] pDescKind, [MarshalAs(UnmanagedType.LPArray)] [Out] NativeMethods.tagFUNCDESC[] ppFuncDesc, [MarshalAs(UnmanagedType.LPArray)] [Out] NativeMethods.tagVARDESC[] ppVarDesc, [MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.ITypeComp[] ppTypeComp, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pDummy);

			// Token: 0x06001161 RID: 4449
			void RemoteBindType([MarshalAs(UnmanagedType.LPWStr)] [In] string szName, [MarshalAs(UnmanagedType.U4)] [In] int lHashVal, [MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.ITypeInfo[] ppTInfo);
		}

		// Token: 0x020001BC RID: 444
		[Guid("00020402-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface ITypeLib
		{
			// Token: 0x06001162 RID: 4450
			void RemoteGetTypeInfoCount([MarshalAs(UnmanagedType.LPArray)] [Out] int[] pcTInfo);

			// Token: 0x06001163 RID: 4451
			void GetTypeInfo([MarshalAs(UnmanagedType.U4)] [In] int index, [MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.ITypeInfo[] ppTInfo);

			// Token: 0x06001164 RID: 4452
			void GetTypeInfoType([MarshalAs(UnmanagedType.U4)] [In] int index, [MarshalAs(UnmanagedType.LPArray)] [Out] NativeMethods.tagTYPEKIND[] pTKind);

			// Token: 0x06001165 RID: 4453
			void GetTypeInfoOfGuid([In] ref Guid guid, [MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.ITypeInfo[] ppTInfo);

			// Token: 0x06001166 RID: 4454
			void RemoteGetLibAttr(IntPtr ppTLibAttr, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pDummy);

			// Token: 0x06001167 RID: 4455
			void GetTypeComp([MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.ITypeComp[] ppTComp);

			// Token: 0x06001168 RID: 4456
			void RemoteGetDocumentation(int index, [MarshalAs(UnmanagedType.U4)] [In] int refPtrFlags, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] pBstrName, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] pBstrDocString, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pdwHelpContext, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] pBstrHelpFile);

			// Token: 0x06001169 RID: 4457
			void RemoteIsName([MarshalAs(UnmanagedType.LPWStr)] [In] string szNameBuf, [MarshalAs(UnmanagedType.U4)] [In] int lHashVal, [MarshalAs(UnmanagedType.LPArray)] [Out] IntPtr[] pfName, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] pBstrLibName);

			// Token: 0x0600116A RID: 4458
			void RemoteFindName([MarshalAs(UnmanagedType.LPWStr)] [In] string szNameBuf, [MarshalAs(UnmanagedType.U4)] [In] int lHashVal, [MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.ITypeInfo[] ppTInfo, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] rgMemId, [MarshalAs(UnmanagedType.LPArray)] [In] [Out] short[] pcFound, [MarshalAs(UnmanagedType.LPArray)] [Out] string[] pBstrLibName);

			// Token: 0x0600116B RID: 4459
			void LocalReleaseTLibAttr();
		}

		// Token: 0x020001BD RID: 445
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("DF0B3D60-548F-101B-8E65-08002B2BD119")]
		[ComImport]
		public interface ISupportErrorInfo
		{
			// Token: 0x0600116C RID: 4460
			int InterfaceSupportsErrorInfo([In] ref Guid riid);
		}

		// Token: 0x020001BE RID: 446
		[Guid("1CF2B120-547D-101B-8E65-08002B2BD119")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IErrorInfo
		{
			// Token: 0x0600116D RID: 4461
			[SuppressUnmanagedCodeSecurity]
			[PreserveSig]
			int GetGUID(out Guid pguid);

			// Token: 0x0600116E RID: 4462
			[SuppressUnmanagedCodeSecurity]
			[PreserveSig]
			int GetSource([MarshalAs(UnmanagedType.BStr)] [In] [Out] ref string pBstrSource);

			// Token: 0x0600116F RID: 4463
			[SuppressUnmanagedCodeSecurity]
			[PreserveSig]
			int GetDescription([MarshalAs(UnmanagedType.BStr)] [In] [Out] ref string pBstrDescription);

			// Token: 0x06001170 RID: 4464
			[SuppressUnmanagedCodeSecurity]
			[PreserveSig]
			int GetHelpFile([MarshalAs(UnmanagedType.BStr)] [In] [Out] ref string pBstrHelpFile);

			// Token: 0x06001171 RID: 4465
			[SuppressUnmanagedCodeSecurity]
			[PreserveSig]
			int GetHelpContext([MarshalAs(UnmanagedType.U4)] [In] [Out] ref int pdwHelpContext);
		}

		// Token: 0x020001BF RID: 447
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagQACONTAINER
		{
			// Token: 0x04000F6C RID: 3948
			[MarshalAs(UnmanagedType.U4)]
			public int cbSize = Marshal.SizeOf(typeof(UnsafeNativeMethods.tagQACONTAINER));

			// Token: 0x04000F6D RID: 3949
			public UnsafeNativeMethods.IOleClientSite pClientSite;

			// Token: 0x04000F6E RID: 3950
			[MarshalAs(UnmanagedType.Interface)]
			public object pAdviseSink;

			// Token: 0x04000F6F RID: 3951
			public UnsafeNativeMethods.IPropertyNotifySink pPropertyNotifySink;

			// Token: 0x04000F70 RID: 3952
			[MarshalAs(UnmanagedType.Interface)]
			public object pUnkEventSink;

			// Token: 0x04000F71 RID: 3953
			[MarshalAs(UnmanagedType.U4)]
			public int dwAmbientFlags;

			// Token: 0x04000F72 RID: 3954
			[MarshalAs(UnmanagedType.U4)]
			public uint colorFore;

			// Token: 0x04000F73 RID: 3955
			[MarshalAs(UnmanagedType.U4)]
			public uint colorBack;

			// Token: 0x04000F74 RID: 3956
			[MarshalAs(UnmanagedType.Interface)]
			public object pFont;

			// Token: 0x04000F75 RID: 3957
			[MarshalAs(UnmanagedType.Interface)]
			public object pUndoMgr;

			// Token: 0x04000F76 RID: 3958
			[MarshalAs(UnmanagedType.U4)]
			public int dwAppearance;

			// Token: 0x04000F77 RID: 3959
			public int lcid;

			// Token: 0x04000F78 RID: 3960
			public IntPtr hpal = IntPtr.Zero;

			// Token: 0x04000F79 RID: 3961
			[MarshalAs(UnmanagedType.Interface)]
			public object pBindHost;
		}

		// Token: 0x020001C0 RID: 448
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagQACONTROL
		{
			// Token: 0x04000F7A RID: 3962
			[MarshalAs(UnmanagedType.U4)]
			public int cbSize = Marshal.SizeOf(typeof(UnsafeNativeMethods.tagQACONTROL));

			// Token: 0x04000F7B RID: 3963
			[MarshalAs(UnmanagedType.U4)]
			public int dwMiscStatus;

			// Token: 0x04000F7C RID: 3964
			[MarshalAs(UnmanagedType.U4)]
			public int dwViewStatus;

			// Token: 0x04000F7D RID: 3965
			[MarshalAs(UnmanagedType.U4)]
			public int dwEventCookie;

			// Token: 0x04000F7E RID: 3966
			[MarshalAs(UnmanagedType.U4)]
			public int dwPropNotifyCookie;

			// Token: 0x04000F7F RID: 3967
			[MarshalAs(UnmanagedType.U4)]
			public int dwPointerActivationPolicy;
		}

		// Token: 0x020001C1 RID: 449
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("0000000A-0000-0000-C000-000000000046")]
		[ComImport]
		public interface ILockBytes
		{
			// Token: 0x06001174 RID: 4468
			void ReadAt([MarshalAs(UnmanagedType.U8)] [In] long ulOffset, [Out] IntPtr pv, [MarshalAs(UnmanagedType.U4)] [In] int cb, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pcbRead);

			// Token: 0x06001175 RID: 4469
			void WriteAt([MarshalAs(UnmanagedType.U8)] [In] long ulOffset, IntPtr pv, [MarshalAs(UnmanagedType.U4)] [In] int cb, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] pcbWritten);

			// Token: 0x06001176 RID: 4470
			void Flush();

			// Token: 0x06001177 RID: 4471
			void SetSize([MarshalAs(UnmanagedType.U8)] [In] long cb);

			// Token: 0x06001178 RID: 4472
			void LockRegion([MarshalAs(UnmanagedType.U8)] [In] long libOffset, [MarshalAs(UnmanagedType.U8)] [In] long cb, [MarshalAs(UnmanagedType.U4)] [In] int dwLockType);

			// Token: 0x06001179 RID: 4473
			void UnlockRegion([MarshalAs(UnmanagedType.U8)] [In] long libOffset, [MarshalAs(UnmanagedType.U8)] [In] long cb, [MarshalAs(UnmanagedType.U4)] [In] int dwLockType);

			// Token: 0x0600117A RID: 4474
			void Stat([Out] NativeMethods.STATSTG pstatstg, [MarshalAs(UnmanagedType.U4)] [In] int grfStatFlag);
		}

		// Token: 0x020001C2 RID: 450
		[SuppressUnmanagedCodeSecurity]
		[StructLayout(LayoutKind.Sequential)]
		public class OFNOTIFY
		{
			// Token: 0x04000F80 RID: 3968
			public IntPtr hdr_hwndFrom = IntPtr.Zero;

			// Token: 0x04000F81 RID: 3969
			public IntPtr hdr_idFrom = IntPtr.Zero;

			// Token: 0x04000F82 RID: 3970
			public int hdr_code;

			// Token: 0x04000F83 RID: 3971
			public IntPtr lpOFN = IntPtr.Zero;

			// Token: 0x04000F84 RID: 3972
			public IntPtr pszFile = IntPtr.Zero;
		}

		// Token: 0x020001C3 RID: 451
		// (Invoke) Token: 0x0600117D RID: 4477
		public delegate int BrowseCallbackProc(IntPtr hwnd, int msg, IntPtr lParam, IntPtr lpData);

		// Token: 0x020001C4 RID: 452
		[Flags]
		public enum BrowseInfos
		{
			// Token: 0x04000F86 RID: 3974
			NewDialogStyle = 64,
			// Token: 0x04000F87 RID: 3975
			HideNewFolderButton = 512
		}

		// Token: 0x020001C5 RID: 453
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class BROWSEINFO
		{
			// Token: 0x04000F88 RID: 3976
			public IntPtr hwndOwner;

			// Token: 0x04000F89 RID: 3977
			public IntPtr pidlRoot;

			// Token: 0x04000F8A RID: 3978
			public IntPtr pszDisplayName;

			// Token: 0x04000F8B RID: 3979
			public string lpszTitle;

			// Token: 0x04000F8C RID: 3980
			public int ulFlags;

			// Token: 0x04000F8D RID: 3981
			public UnsafeNativeMethods.BrowseCallbackProc lpfn;

			// Token: 0x04000F8E RID: 3982
			public IntPtr lParam;

			// Token: 0x04000F8F RID: 3983
			public int iImage;
		}

		// Token: 0x020001C6 RID: 454
		[SuppressUnmanagedCodeSecurity]
		internal class Shell32
		{
			// Token: 0x06001181 RID: 4481
			[DllImport("shell32.dll")]
			public static extern int SHGetSpecialFolderLocation(IntPtr hwnd, int csidl, ref IntPtr ppidl);

			// Token: 0x06001182 RID: 4482
			[DllImport("shell32.dll", CharSet = CharSet.Auto)]
			public static extern bool SHGetPathFromIDList(IntPtr pidl, IntPtr pszPath);

			// Token: 0x06001183 RID: 4483
			[DllImport("shell32.dll", CharSet = CharSet.Auto)]
			public static extern IntPtr SHBrowseForFolder([In] UnsafeNativeMethods.BROWSEINFO lpbi);

			// Token: 0x06001184 RID: 4484
			[DllImport("shell32.dll")]
			public static extern int SHGetMalloc([MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.IMalloc[] ppMalloc);

			// Token: 0x06001185 RID: 4485
			[DllImport("shell32.dll", EntryPoint = "SHGetFolderPathEx")]
			private static extern int SHGetFolderPathExPrivate(ref Guid rfid, uint dwFlags, IntPtr hToken, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszPath, uint cchPath);

			// Token: 0x06001186 RID: 4486 RVA: 0x0000EFA1 File Offset: 0x0000DFA1
			public static int SHGetFolderPathEx(ref Guid rfid, uint dwFlags, IntPtr hToken, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszPath, uint cchPath)
			{
				if (UnsafeNativeMethods.IsVista)
				{
					return UnsafeNativeMethods.Shell32.SHGetFolderPathExPrivate(ref rfid, dwFlags, hToken, pszPath, cchPath);
				}
				throw new NotSupportedException();
			}

			// Token: 0x06001187 RID: 4487
			[DllImport("shell32.dll")]
			public static extern int SHCreateShellItem(IntPtr pidlParent, IntPtr psfParent, IntPtr pidl, out FileDialogNative.IShellItem ppsi);

			// Token: 0x06001188 RID: 4488
			[DllImport("shell32.dll")]
			public static extern int SHILCreateFromPath([MarshalAs(UnmanagedType.LPWStr)] string pszPath, out IntPtr ppIdl, ref uint rgflnOut);
		}

		// Token: 0x020001C7 RID: 455
		[Guid("00000002-0000-0000-c000-000000000046")]
		[SuppressUnmanagedCodeSecurity]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IMalloc
		{
			// Token: 0x0600118A RID: 4490
			[PreserveSig]
			IntPtr Alloc(int cb);

			// Token: 0x0600118B RID: 4491
			[PreserveSig]
			IntPtr Realloc(IntPtr pv, int cb);

			// Token: 0x0600118C RID: 4492
			[PreserveSig]
			void Free(IntPtr pv);

			// Token: 0x0600118D RID: 4493
			[PreserveSig]
			int GetSize(IntPtr pv);

			// Token: 0x0600118E RID: 4494
			[PreserveSig]
			int DidAlloc(IntPtr pv);

			// Token: 0x0600118F RID: 4495
			[PreserveSig]
			void HeapMinimize();
		}

		// Token: 0x020001C8 RID: 456
		[Guid("00000126-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IRunnableObject
		{
			// Token: 0x06001190 RID: 4496
			void GetRunningClass(out Guid guid);

			// Token: 0x06001191 RID: 4497
			[PreserveSig]
			int Run(IntPtr lpBindContext);

			// Token: 0x06001192 RID: 4498
			bool IsRunning();

			// Token: 0x06001193 RID: 4499
			void LockRunning(bool fLock, bool fLastUnlockCloses);

			// Token: 0x06001194 RID: 4500
			void SetContainedObject(bool fContained);
		}

		// Token: 0x020001C9 RID: 457
		[ComVisible(true)]
		[Guid("B722BCC7-4E68-101B-A2BC-00AA00404770")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IOleDocumentSite
		{
			// Token: 0x06001195 RID: 4501
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int ActivateMe([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IOleDocumentView pViewToActivate);
		}

		// Token: 0x020001CA RID: 458
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComVisible(true)]
		[Guid("B722BCC6-4E68-101B-A2BC-00AA00404770")]
		public interface IOleDocumentView
		{
			// Token: 0x06001196 RID: 4502
			void SetInPlaceSite([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IOleInPlaceSite pIPSite);

			// Token: 0x06001197 RID: 4503
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IOleInPlaceSite GetInPlaceSite();

			// Token: 0x06001198 RID: 4504
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetDocument();

			// Token: 0x06001199 RID: 4505
			void SetRect([In] ref NativeMethods.RECT prcView);

			// Token: 0x0600119A RID: 4506
			void GetRect([In] [Out] ref NativeMethods.RECT prcView);

			// Token: 0x0600119B RID: 4507
			void SetRectComplex([In] NativeMethods.RECT prcView, [In] NativeMethods.RECT prcHScroll, [In] NativeMethods.RECT prcVScroll, [In] NativeMethods.RECT prcSizeBox);

			// Token: 0x0600119C RID: 4508
			void Show(bool fShow);

			// Token: 0x0600119D RID: 4509
			[PreserveSig]
			int UIActivate(bool fUIActivate);

			// Token: 0x0600119E RID: 4510
			void Open();

			// Token: 0x0600119F RID: 4511
			[PreserveSig]
			int Close([MarshalAs(UnmanagedType.U4)] [In] int dwReserved);

			// Token: 0x060011A0 RID: 4512
			void SaveViewState([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStream pstm);

			// Token: 0x060011A1 RID: 4513
			void ApplyViewState([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStream pstm);

			// Token: 0x060011A2 RID: 4514
			void Clone([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IOleInPlaceSite pIPSiteNew, [MarshalAs(UnmanagedType.LPArray)] [Out] UnsafeNativeMethods.IOleDocumentView[] ppViewNew);
		}

		// Token: 0x020001CB RID: 459
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("b722bcc5-4e68-101b-a2bc-00aa00404770")]
		[ComImport]
		public interface IOleDocument
		{
			// Token: 0x060011A3 RID: 4515
			[PreserveSig]
			int CreateView(UnsafeNativeMethods.IOleInPlaceSite pIPSite, UnsafeNativeMethods.IStream pstm, int dwReserved, out UnsafeNativeMethods.IOleDocumentView ppView);

			// Token: 0x060011A4 RID: 4516
			[PreserveSig]
			int GetDocMiscStatus(out int pdwStatus);

			// Token: 0x060011A5 RID: 4517
			int EnumViews(out object ppEnum, out UnsafeNativeMethods.IOleDocumentView ppView);
		}

		// Token: 0x020001CC RID: 460
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("0000011e-0000-0000-C000-000000000046")]
		[ComImport]
		public interface IOleCache
		{
			// Token: 0x060011A6 RID: 4518
			int Cache(ref FORMATETC pformatetc, int advf);

			// Token: 0x060011A7 RID: 4519
			void Uncache(int dwConnection);

			// Token: 0x060011A8 RID: 4520
			object EnumCache();

			// Token: 0x060011A9 RID: 4521
			void InitCache(IDataObject pDataObject);

			// Token: 0x060011AA RID: 4522
			void SetData(ref FORMATETC pformatetc, ref STGMEDIUM pmedium, bool fRelease);
		}

		// Token: 0x020001CD RID: 461
		[Guid("618736E0-3C3D-11CF-810C-00AA00389B71")]
		[TypeLibType(4176)]
		[ComImport]
		public interface IAccessibleInternal
		{
			// Token: 0x060011AB RID: 4523
			[DispId(-5000)]
			[TypeLibFunc(64)]
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object get_accParent();

			// Token: 0x060011AC RID: 4524
			[DispId(-5001)]
			[TypeLibFunc(64)]
			int get_accChildCount();

			// Token: 0x060011AD RID: 4525
			[TypeLibFunc(64)]
			[DispId(-5002)]
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object get_accChild([MarshalAs(UnmanagedType.Struct)] [In] object varChild);

			// Token: 0x060011AE RID: 4526
			[TypeLibFunc(64)]
			[DispId(-5003)]
			[return: MarshalAs(UnmanagedType.BStr)]
			string get_accName([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

			// Token: 0x060011AF RID: 4527
			[DispId(-5004)]
			[TypeLibFunc(64)]
			[return: MarshalAs(UnmanagedType.BStr)]
			string get_accValue([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

			// Token: 0x060011B0 RID: 4528
			[TypeLibFunc(64)]
			[DispId(-5005)]
			[return: MarshalAs(UnmanagedType.BStr)]
			string get_accDescription([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

			// Token: 0x060011B1 RID: 4529
			[DispId(-5006)]
			[TypeLibFunc(64)]
			[return: MarshalAs(UnmanagedType.Struct)]
			object get_accRole([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

			// Token: 0x060011B2 RID: 4530
			[DispId(-5007)]
			[TypeLibFunc(64)]
			[return: MarshalAs(UnmanagedType.Struct)]
			object get_accState([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

			// Token: 0x060011B3 RID: 4531
			[DispId(-5008)]
			[TypeLibFunc(64)]
			[return: MarshalAs(UnmanagedType.BStr)]
			string get_accHelp([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

			// Token: 0x060011B4 RID: 4532
			[TypeLibFunc(64)]
			[DispId(-5009)]
			int get_accHelpTopic([MarshalAs(UnmanagedType.BStr)] out string pszHelpFile, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

			// Token: 0x060011B5 RID: 4533
			[TypeLibFunc(64)]
			[DispId(-5010)]
			[return: MarshalAs(UnmanagedType.BStr)]
			string get_accKeyboardShortcut([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

			// Token: 0x060011B6 RID: 4534
			[TypeLibFunc(64)]
			[DispId(-5011)]
			[return: MarshalAs(UnmanagedType.Struct)]
			object get_accFocus();

			// Token: 0x060011B7 RID: 4535
			[TypeLibFunc(64)]
			[DispId(-5012)]
			[return: MarshalAs(UnmanagedType.Struct)]
			object get_accSelection();

			// Token: 0x060011B8 RID: 4536
			[DispId(-5013)]
			[TypeLibFunc(64)]
			[return: MarshalAs(UnmanagedType.BStr)]
			string get_accDefaultAction([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

			// Token: 0x060011B9 RID: 4537
			[TypeLibFunc(64)]
			[DispId(-5014)]
			void accSelect([In] int flagsSelect, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

			// Token: 0x060011BA RID: 4538
			[TypeLibFunc(64)]
			[DispId(-5015)]
			void accLocation(out int pxLeft, out int pyTop, out int pcxWidth, out int pcyHeight, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

			// Token: 0x060011BB RID: 4539
			[TypeLibFunc(64)]
			[DispId(-5016)]
			[return: MarshalAs(UnmanagedType.Struct)]
			object accNavigate([In] int navDir, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varStart);

			// Token: 0x060011BC RID: 4540
			[DispId(-5017)]
			[TypeLibFunc(64)]
			[return: MarshalAs(UnmanagedType.Struct)]
			object accHitTest([In] int xLeft, [In] int yTop);

			// Token: 0x060011BD RID: 4541
			[TypeLibFunc(64)]
			[DispId(-5018)]
			void accDoDefaultAction([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

			// Token: 0x060011BE RID: 4542
			[DispId(-5003)]
			[TypeLibFunc(64)]
			void set_accName([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild, [MarshalAs(UnmanagedType.BStr)] [In] string pszName);

			// Token: 0x060011BF RID: 4543
			[DispId(-5004)]
			[TypeLibFunc(64)]
			void set_accValue([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild, [MarshalAs(UnmanagedType.BStr)] [In] string pszValue);
		}

		// Token: 0x020001CE RID: 462
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("BEF6E002-A874-101A-8BBA-00AA00300CAB")]
		[ComImport]
		public interface IFont
		{
			// Token: 0x060011C0 RID: 4544
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetName();

			// Token: 0x060011C1 RID: 4545
			void SetName([MarshalAs(UnmanagedType.BStr)] [In] string pname);

			// Token: 0x060011C2 RID: 4546
			[return: MarshalAs(UnmanagedType.U8)]
			long GetSize();

			// Token: 0x060011C3 RID: 4547
			void SetSize([MarshalAs(UnmanagedType.U8)] [In] long psize);

			// Token: 0x060011C4 RID: 4548
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetBold();

			// Token: 0x060011C5 RID: 4549
			void SetBold([MarshalAs(UnmanagedType.Bool)] [In] bool pbold);

			// Token: 0x060011C6 RID: 4550
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetItalic();

			// Token: 0x060011C7 RID: 4551
			void SetItalic([MarshalAs(UnmanagedType.Bool)] [In] bool pitalic);

			// Token: 0x060011C8 RID: 4552
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetUnderline();

			// Token: 0x060011C9 RID: 4553
			void SetUnderline([MarshalAs(UnmanagedType.Bool)] [In] bool punderline);

			// Token: 0x060011CA RID: 4554
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetStrikethrough();

			// Token: 0x060011CB RID: 4555
			void SetStrikethrough([MarshalAs(UnmanagedType.Bool)] [In] bool pstrikethrough);

			// Token: 0x060011CC RID: 4556
			[return: MarshalAs(UnmanagedType.I2)]
			short GetWeight();

			// Token: 0x060011CD RID: 4557
			void SetWeight([MarshalAs(UnmanagedType.I2)] [In] short pweight);

			// Token: 0x060011CE RID: 4558
			[return: MarshalAs(UnmanagedType.I2)]
			short GetCharset();

			// Token: 0x060011CF RID: 4559
			void SetCharset([MarshalAs(UnmanagedType.I2)] [In] short pcharset);

			// Token: 0x060011D0 RID: 4560
			IntPtr GetHFont();

			// Token: 0x060011D1 RID: 4561
			void Clone(out UnsafeNativeMethods.IFont ppfont);

			// Token: 0x060011D2 RID: 4562
			[PreserveSig]
			int IsEqual([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IFont pfontOther);

			// Token: 0x060011D3 RID: 4563
			void SetRatio(int cyLogical, int cyHimetric);

			// Token: 0x060011D4 RID: 4564
			void QueryTextMetrics(out IntPtr ptm);

			// Token: 0x060011D5 RID: 4565
			void AddRefHfont(IntPtr hFont);

			// Token: 0x060011D6 RID: 4566
			void ReleaseHfont(IntPtr hFont);

			// Token: 0x060011D7 RID: 4567
			void SetHdc(IntPtr hdc);
		}

		// Token: 0x020001CF RID: 463
		[Guid("7BF80980-BF32-101A-8BBB-00AA00300CAB")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IPicture
		{
			// Token: 0x060011D8 RID: 4568
			IntPtr GetHandle();

			// Token: 0x060011D9 RID: 4569
			IntPtr GetHPal();

			// Token: 0x060011DA RID: 4570
			[return: MarshalAs(UnmanagedType.I2)]
			short GetPictureType();

			// Token: 0x060011DB RID: 4571
			int GetWidth();

			// Token: 0x060011DC RID: 4572
			int GetHeight();

			// Token: 0x060011DD RID: 4573
			void Render(IntPtr hDC, int x, int y, int cx, int cy, int xSrc, int ySrc, int cxSrc, int cySrc, IntPtr rcBounds);

			// Token: 0x060011DE RID: 4574
			void SetHPal(IntPtr phpal);

			// Token: 0x060011DF RID: 4575
			IntPtr GetCurDC();

			// Token: 0x060011E0 RID: 4576
			void SelectPicture(IntPtr hdcIn, [MarshalAs(UnmanagedType.LPArray)] [Out] IntPtr[] phdcOut, [MarshalAs(UnmanagedType.LPArray)] [Out] IntPtr[] phbmpOut);

			// Token: 0x060011E1 RID: 4577
			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetKeepOriginalFormat();

			// Token: 0x060011E2 RID: 4578
			void SetKeepOriginalFormat([MarshalAs(UnmanagedType.Bool)] [In] bool pfkeep);

			// Token: 0x060011E3 RID: 4579
			void PictureChanged();

			// Token: 0x060011E4 RID: 4580
			[PreserveSig]
			int SaveAsFile([MarshalAs(UnmanagedType.Interface)] [In] UnsafeNativeMethods.IStream pstm, int fSaveMemCopy, out int pcbSize);

			// Token: 0x060011E5 RID: 4581
			int GetAttributes();
		}

		// Token: 0x020001D0 RID: 464
		[Guid("7BF80981-BF32-101A-8BBB-00AA00300CAB")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		[ComImport]
		public interface IPictureDisp
		{
			// Token: 0x170001AB RID: 427
			// (get) Token: 0x060011E6 RID: 4582
			IntPtr Handle { get; }

			// Token: 0x170001AC RID: 428
			// (get) Token: 0x060011E7 RID: 4583
			IntPtr HPal { get; }

			// Token: 0x170001AD RID: 429
			// (get) Token: 0x060011E8 RID: 4584
			short PictureType { get; }

			// Token: 0x170001AE RID: 430
			// (get) Token: 0x060011E9 RID: 4585
			int Width { get; }

			// Token: 0x170001AF RID: 431
			// (get) Token: 0x060011EA RID: 4586
			int Height { get; }

			// Token: 0x060011EB RID: 4587
			void Render(IntPtr hdc, int x, int y, int cx, int cy, int xSrc, int ySrc, int cxSrc, int cySrc);
		}

		// Token: 0x020001D1 RID: 465
		[SuppressUnmanagedCodeSecurity]
		internal class ThemingScope
		{
			// Token: 0x060011EC RID: 4588 RVA: 0x0000EFC4 File Offset: 0x0000DFC4
			private static bool IsContextActive()
			{
				IntPtr zero = IntPtr.Zero;
				return UnsafeNativeMethods.ThemingScope.contextCreationSucceeded && UnsafeNativeMethods.ThemingScope.GetCurrentActCtx(out zero) && zero == UnsafeNativeMethods.ThemingScope.hActCtx;
			}

			// Token: 0x060011ED RID: 4589 RVA: 0x0000EFF4 File Offset: 0x0000DFF4
			public static IntPtr Activate()
			{
				IntPtr intPtr = IntPtr.Zero;
				if (Application.UseVisualStyles && UnsafeNativeMethods.ThemingScope.contextCreationSucceeded && OSFeature.Feature.IsPresent(OSFeature.Themes) && !UnsafeNativeMethods.ThemingScope.IsContextActive() && !UnsafeNativeMethods.ThemingScope.ActivateActCtx(UnsafeNativeMethods.ThemingScope.hActCtx, out intPtr))
				{
					intPtr = IntPtr.Zero;
				}
				return intPtr;
			}

			// Token: 0x060011EE RID: 4590 RVA: 0x0000F042 File Offset: 0x0000E042
			public static IntPtr Deactivate(IntPtr userCookie)
			{
				if (userCookie != IntPtr.Zero && OSFeature.Feature.IsPresent(OSFeature.Themes) && UnsafeNativeMethods.ThemingScope.DeactivateActCtx(0, userCookie))
				{
					userCookie = IntPtr.Zero;
				}
				return userCookie;
			}

			// Token: 0x060011EF RID: 4591 RVA: 0x0000F074 File Offset: 0x0000E074
			public static bool CreateActivationContext(string dllPath, int nativeResourceManifestID)
			{
				bool flag;
				lock (typeof(UnsafeNativeMethods.ThemingScope))
				{
					if (!UnsafeNativeMethods.ThemingScope.contextCreationSucceeded && OSFeature.Feature.IsPresent(OSFeature.Themes))
					{
						UnsafeNativeMethods.ThemingScope.enableThemingActivationContext = default(UnsafeNativeMethods.ThemingScope.ACTCTX);
						UnsafeNativeMethods.ThemingScope.enableThemingActivationContext.cbSize = Marshal.SizeOf(typeof(UnsafeNativeMethods.ThemingScope.ACTCTX));
						UnsafeNativeMethods.ThemingScope.enableThemingActivationContext.lpSource = dllPath;
						UnsafeNativeMethods.ThemingScope.enableThemingActivationContext.lpResourceName = (IntPtr)nativeResourceManifestID;
						UnsafeNativeMethods.ThemingScope.enableThemingActivationContext.dwFlags = 8U;
						UnsafeNativeMethods.ThemingScope.hActCtx = UnsafeNativeMethods.ThemingScope.CreateActCtx(ref UnsafeNativeMethods.ThemingScope.enableThemingActivationContext);
						UnsafeNativeMethods.ThemingScope.contextCreationSucceeded = UnsafeNativeMethods.ThemingScope.hActCtx != new IntPtr(-1);
					}
					flag = UnsafeNativeMethods.ThemingScope.contextCreationSucceeded;
				}
				return flag;
			}

			// Token: 0x060011F0 RID: 4592
			[DllImport("kernel32.dll")]
			private static extern IntPtr CreateActCtx(ref UnsafeNativeMethods.ThemingScope.ACTCTX actctx);

			// Token: 0x060011F1 RID: 4593
			[DllImport("kernel32.dll")]
			private static extern bool ActivateActCtx(IntPtr hActCtx, out IntPtr lpCookie);

			// Token: 0x060011F2 RID: 4594
			[DllImport("kernel32.dll")]
			private static extern bool DeactivateActCtx(int dwFlags, IntPtr lpCookie);

			// Token: 0x060011F3 RID: 4595
			[DllImport("kernel32.dll")]
			private static extern bool GetCurrentActCtx(out IntPtr handle);

			// Token: 0x04000F90 RID: 3984
			private const int ACTCTX_FLAG_ASSEMBLY_DIRECTORY_VALID = 4;

			// Token: 0x04000F91 RID: 3985
			private const int ACTCTX_FLAG_RESOURCE_NAME_VALID = 8;

			// Token: 0x04000F92 RID: 3986
			private static UnsafeNativeMethods.ThemingScope.ACTCTX enableThemingActivationContext;

			// Token: 0x04000F93 RID: 3987
			private static IntPtr hActCtx;

			// Token: 0x04000F94 RID: 3988
			private static bool contextCreationSucceeded;

			// Token: 0x020001D2 RID: 466
			private struct ACTCTX
			{
				// Token: 0x04000F95 RID: 3989
				public int cbSize;

				// Token: 0x04000F96 RID: 3990
				public uint dwFlags;

				// Token: 0x04000F97 RID: 3991
				public string lpSource;

				// Token: 0x04000F98 RID: 3992
				public ushort wProcessorArchitecture;

				// Token: 0x04000F99 RID: 3993
				public ushort wLangId;

				// Token: 0x04000F9A RID: 3994
				public string lpAssemblyDirectory;

				// Token: 0x04000F9B RID: 3995
				public IntPtr lpResourceName;

				// Token: 0x04000F9C RID: 3996
				public string lpApplicationName;
			}
		}

		// Token: 0x020001D3 RID: 467
		[SuppressUnmanagedCodeSecurity]
		[StructLayout(LayoutKind.Sequential)]
		internal class PROCESS_INFORMATION
		{
			// Token: 0x060011F5 RID: 4597 RVA: 0x0000F140 File Offset: 0x0000E140
			~PROCESS_INFORMATION()
			{
				this.Close();
			}

			// Token: 0x060011F6 RID: 4598 RVA: 0x0000F16C File Offset: 0x0000E16C
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			internal void Close()
			{
				if (this.hProcess != (IntPtr)0 && this.hProcess != UnsafeNativeMethods.PROCESS_INFORMATION.INVALID_HANDLE_VALUE)
				{
					UnsafeNativeMethods.PROCESS_INFORMATION.CloseHandle(new HandleRef(this, this.hProcess));
					this.hProcess = UnsafeNativeMethods.PROCESS_INFORMATION.INVALID_HANDLE_VALUE;
				}
				if (this.hThread != (IntPtr)0 && this.hThread != UnsafeNativeMethods.PROCESS_INFORMATION.INVALID_HANDLE_VALUE)
				{
					UnsafeNativeMethods.PROCESS_INFORMATION.CloseHandle(new HandleRef(this, this.hThread));
					this.hThread = UnsafeNativeMethods.PROCESS_INFORMATION.INVALID_HANDLE_VALUE;
				}
			}

			// Token: 0x060011F7 RID: 4599
			[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
			private static extern bool CloseHandle(HandleRef handle);

			// Token: 0x04000F9D RID: 3997
			public IntPtr hProcess = IntPtr.Zero;

			// Token: 0x04000F9E RID: 3998
			public IntPtr hThread = IntPtr.Zero;

			// Token: 0x04000F9F RID: 3999
			public int dwProcessId;

			// Token: 0x04000FA0 RID: 4000
			public int dwThreadId;

			// Token: 0x04000FA1 RID: 4001
			private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
		}
	}
}
