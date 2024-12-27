using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x020000D0 RID: 208
	[InterfaceType(1)]
	[Guid("9AE62877-7544-4BB0-AA26-A13824659ED6")]
	[ComImport]
	internal interface IWbemPathKeyList
	{
		// Token: 0x06000626 RID: 1574
		[PreserveSig]
		int GetCount_(out uint puKeyCount);

		// Token: 0x06000627 RID: 1575
		[PreserveSig]
		int SetKey_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] uint uFlags, [In] uint uCimType, [In] IntPtr pKeyVal);

		// Token: 0x06000628 RID: 1576
		[PreserveSig]
		int SetKey2_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] uint uFlags, [In] uint uCimType, [In] ref object pKeyVal);

		// Token: 0x06000629 RID: 1577
		[PreserveSig]
		int GetKey_([In] uint uKeyIx, [In] uint uFlags, [In] [Out] ref uint puNameBufSize, [MarshalAs(UnmanagedType.LPWStr)] [In] [Out] string pszKeyName, [In] [Out] ref uint puKeyValBufSize, [In] [Out] IntPtr pKeyVal, out uint puApparentCimType);

		// Token: 0x0600062A RID: 1578
		[PreserveSig]
		int GetKey2_([In] uint uKeyIx, [In] uint uFlags, [In] [Out] ref uint puNameBufSize, [MarshalAs(UnmanagedType.LPWStr)] [In] [Out] string pszKeyName, [In] [Out] ref object pKeyValue, out uint puApparentCimType);

		// Token: 0x0600062B RID: 1579
		[PreserveSig]
		int RemoveKey_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] uint uFlags);

		// Token: 0x0600062C RID: 1580
		[PreserveSig]
		int RemoveAllKeys_([In] uint uFlags);

		// Token: 0x0600062D RID: 1581
		[PreserveSig]
		int MakeSingleton_([In] sbyte bSet);

		// Token: 0x0600062E RID: 1582
		[PreserveSig]
		int GetInfo_([In] uint uRequestedInfo, out ulong puResponse);

		// Token: 0x0600062F RID: 1583
		[PreserveSig]
		int GetText_([In] int lFlags, [In] [Out] ref uint puBuffLength, [MarshalAs(UnmanagedType.LPWStr)] [In] [Out] string pszText);
	}
}
