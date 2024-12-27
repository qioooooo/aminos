using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x020000D1 RID: 209
	[Guid("3BC15AF2-736C-477E-9E51-238AF8667DCC")]
	[InterfaceType(1)]
	[ComImport]
	internal interface IWbemPath
	{
		// Token: 0x06000630 RID: 1584
		[PreserveSig]
		int SetText_([In] uint uMode, [MarshalAs(UnmanagedType.LPWStr)] [In] string pszPath);

		// Token: 0x06000631 RID: 1585
		[PreserveSig]
		int GetText_([In] int lFlags, [In] [Out] ref uint puBuffLength, [MarshalAs(UnmanagedType.LPWStr)] [In] [Out] string pszText);

		// Token: 0x06000632 RID: 1586
		[PreserveSig]
		int GetInfo_([In] uint uRequestedInfo, out ulong puResponse);

		// Token: 0x06000633 RID: 1587
		[PreserveSig]
		int SetServer_([MarshalAs(UnmanagedType.LPWStr)] [In] string Name);

		// Token: 0x06000634 RID: 1588
		[PreserveSig]
		int GetServer_([In] [Out] ref uint puNameBufLength, [MarshalAs(UnmanagedType.LPWStr)] [In] [Out] string pName);

		// Token: 0x06000635 RID: 1589
		[PreserveSig]
		int GetNamespaceCount_(out uint puCount);

		// Token: 0x06000636 RID: 1590
		[PreserveSig]
		int SetNamespaceAt_([In] uint uIndex, [MarshalAs(UnmanagedType.LPWStr)] [In] string pszName);

		// Token: 0x06000637 RID: 1591
		[PreserveSig]
		int GetNamespaceAt_([In] uint uIndex, [In] [Out] ref uint puNameBufLength, [MarshalAs(UnmanagedType.LPWStr)] [In] [Out] string pName);

		// Token: 0x06000638 RID: 1592
		[PreserveSig]
		int RemoveNamespaceAt_([In] uint uIndex);

		// Token: 0x06000639 RID: 1593
		[PreserveSig]
		int RemoveAllNamespaces_();

		// Token: 0x0600063A RID: 1594
		[PreserveSig]
		int GetScopeCount_(out uint puCount);

		// Token: 0x0600063B RID: 1595
		[PreserveSig]
		int SetScope_([In] uint uIndex, [MarshalAs(UnmanagedType.LPWStr)] [In] string pszClass);

		// Token: 0x0600063C RID: 1596
		[PreserveSig]
		int SetScopeFromText_([In] uint uIndex, [MarshalAs(UnmanagedType.LPWStr)] [In] string pszText);

		// Token: 0x0600063D RID: 1597
		[PreserveSig]
		int GetScope_([In] uint uIndex, [In] [Out] ref uint puClassNameBufSize, [MarshalAs(UnmanagedType.LPWStr)] [In] [Out] string pszClass, [MarshalAs(UnmanagedType.Interface)] out IWbemPathKeyList pKeyList);

		// Token: 0x0600063E RID: 1598
		[PreserveSig]
		int GetScopeAsText_([In] uint uIndex, [In] [Out] ref uint puTextBufSize, [MarshalAs(UnmanagedType.LPWStr)] [In] [Out] string pszText);

		// Token: 0x0600063F RID: 1599
		[PreserveSig]
		int RemoveScope_([In] uint uIndex);

		// Token: 0x06000640 RID: 1600
		[PreserveSig]
		int RemoveAllScopes_();

		// Token: 0x06000641 RID: 1601
		[PreserveSig]
		int SetClassName_([MarshalAs(UnmanagedType.LPWStr)] [In] string Name);

		// Token: 0x06000642 RID: 1602
		[PreserveSig]
		int GetClassName_([In] [Out] ref uint puBuffLength, [MarshalAs(UnmanagedType.LPWStr)] [In] [Out] string pszName);

		// Token: 0x06000643 RID: 1603
		[PreserveSig]
		int GetKeyList_([MarshalAs(UnmanagedType.Interface)] out IWbemPathKeyList pOut);

		// Token: 0x06000644 RID: 1604
		[PreserveSig]
		int CreateClassPart_([In] int lFlags, [MarshalAs(UnmanagedType.LPWStr)] [In] string Name);

		// Token: 0x06000645 RID: 1605
		[PreserveSig]
		int DeleteClassPart_([In] int lFlags);

		// Token: 0x06000646 RID: 1606
		[PreserveSig]
		int IsRelative_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszMachine, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszNamespace);

		// Token: 0x06000647 RID: 1607
		[PreserveSig]
		int IsRelativeOrChild_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszMachine, [MarshalAs(UnmanagedType.LPWStr)] [In] string wszNamespace, [In] int lFlags);

		// Token: 0x06000648 RID: 1608
		[PreserveSig]
		int IsLocal_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszMachine);

		// Token: 0x06000649 RID: 1609
		[PreserveSig]
		int IsSameClassName_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszClass);
	}
}
