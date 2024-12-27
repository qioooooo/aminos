using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x020000C6 RID: 198
	[InterfaceType(1)]
	[Guid("49353C9A-516B-11D1-AEA6-00C04FB68820")]
	[TypeLibType(512)]
	[ComImport]
	internal interface IWbemObjectAccess
	{
		// Token: 0x060005F2 RID: 1522
		[PreserveSig]
		int GetQualifierSet_([MarshalAs(UnmanagedType.Interface)] out IWbemQualifierSet_DoNotMarshal ppQualSet);

		// Token: 0x060005F3 RID: 1523
		[PreserveSig]
		int Get_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, [In] [Out] ref object pVal, [In] [Out] ref int pType, [In] [Out] ref int plFlavor);

		// Token: 0x060005F4 RID: 1524
		[PreserveSig]
		int Put_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, [In] ref object pVal, [In] int Type);

		// Token: 0x060005F5 RID: 1525
		[PreserveSig]
		int Delete_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName);

		// Token: 0x060005F6 RID: 1526
		[PreserveSig]
		int GetNames_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszQualifierName, [In] int lFlags, [In] ref object pQualifierVal, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)] out string[] pNames);

		// Token: 0x060005F7 RID: 1527
		[PreserveSig]
		int BeginEnumeration_([In] int lEnumFlags);

		// Token: 0x060005F8 RID: 1528
		[PreserveSig]
		int Next_([In] int lFlags, [MarshalAs(UnmanagedType.BStr)] [In] [Out] ref string strName, [In] [Out] ref object pVal, [In] [Out] ref int pType, [In] [Out] ref int plFlavor);

		// Token: 0x060005F9 RID: 1529
		[PreserveSig]
		int EndEnumeration_();

		// Token: 0x060005FA RID: 1530
		[PreserveSig]
		int GetPropertyQualifierSet_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszProperty, [MarshalAs(UnmanagedType.Interface)] out IWbemQualifierSet_DoNotMarshal ppQualSet);

		// Token: 0x060005FB RID: 1531
		[PreserveSig]
		int Clone_([MarshalAs(UnmanagedType.Interface)] out IWbemClassObject_DoNotMarshal ppCopy);

		// Token: 0x060005FC RID: 1532
		[PreserveSig]
		int GetObjectText_([In] int lFlags, [MarshalAs(UnmanagedType.BStr)] out string pstrObjectText);

		// Token: 0x060005FD RID: 1533
		[PreserveSig]
		int SpawnDerivedClass_([In] int lFlags, [MarshalAs(UnmanagedType.Interface)] out IWbemClassObject_DoNotMarshal ppNewClass);

		// Token: 0x060005FE RID: 1534
		[PreserveSig]
		int SpawnInstance_([In] int lFlags, [MarshalAs(UnmanagedType.Interface)] out IWbemClassObject_DoNotMarshal ppNewInstance);

		// Token: 0x060005FF RID: 1535
		[PreserveSig]
		int CompareTo_([In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pCompareTo);

		// Token: 0x06000600 RID: 1536
		[PreserveSig]
		int GetPropertyOrigin_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [MarshalAs(UnmanagedType.BStr)] out string pstrClassName);

		// Token: 0x06000601 RID: 1537
		[PreserveSig]
		int InheritsFrom_([MarshalAs(UnmanagedType.LPWStr)] [In] string strAncestor);

		// Token: 0x06000602 RID: 1538
		[PreserveSig]
		int GetMethod_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] out IWbemClassObject_DoNotMarshal ppInSignature, [MarshalAs(UnmanagedType.Interface)] out IWbemClassObject_DoNotMarshal ppOutSignature);

		// Token: 0x06000603 RID: 1539
		[PreserveSig]
		int PutMethod_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pInSignature, [MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pOutSignature);

		// Token: 0x06000604 RID: 1540
		[PreserveSig]
		int DeleteMethod_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName);

		// Token: 0x06000605 RID: 1541
		[PreserveSig]
		int BeginMethodEnumeration_([In] int lEnumFlags);

		// Token: 0x06000606 RID: 1542
		[PreserveSig]
		int NextMethod_([In] int lFlags, [MarshalAs(UnmanagedType.BStr)] [In] [Out] ref string pstrName, [MarshalAs(UnmanagedType.Interface)] [In] [Out] ref IWbemClassObject_DoNotMarshal ppInSignature, [MarshalAs(UnmanagedType.Interface)] [In] [Out] ref IWbemClassObject_DoNotMarshal ppOutSignature);

		// Token: 0x06000607 RID: 1543
		[PreserveSig]
		int EndMethodEnumeration_();

		// Token: 0x06000608 RID: 1544
		[PreserveSig]
		int GetMethodQualifierSet_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszMethod, [MarshalAs(UnmanagedType.Interface)] out IWbemQualifierSet_DoNotMarshal ppQualSet);

		// Token: 0x06000609 RID: 1545
		[PreserveSig]
		int GetMethodOrigin_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszMethodName, [MarshalAs(UnmanagedType.BStr)] out string pstrClassName);

		// Token: 0x0600060A RID: 1546
		[PreserveSig]
		int GetPropertyHandle_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszPropertyName, out int pType, out int plHandle);

		// Token: 0x0600060B RID: 1547
		[PreserveSig]
		int WritePropertyValue_([In] int lHandle, [In] int lNumBytes, [In] ref byte aData);

		// Token: 0x0600060C RID: 1548
		[PreserveSig]
		int ReadPropertyValue_([In] int lHandle, [In] int lBufferSize, out int plNumBytes, out byte aData);

		// Token: 0x0600060D RID: 1549
		[PreserveSig]
		int ReadDWORD_([In] int lHandle, out uint pdw);

		// Token: 0x0600060E RID: 1550
		[PreserveSig]
		int WriteDWORD_([In] int lHandle, [In] uint dw);

		// Token: 0x0600060F RID: 1551
		[PreserveSig]
		int ReadQWORD_([In] int lHandle, out ulong pqw);

		// Token: 0x06000610 RID: 1552
		[PreserveSig]
		int WriteQWORD_([In] int lHandle, [In] ulong pw);

		// Token: 0x06000611 RID: 1553
		[PreserveSig]
		int GetPropertyInfoByHandle_([In] int lHandle, [MarshalAs(UnmanagedType.BStr)] out string pstrName, out int pType);

		// Token: 0x06000612 RID: 1554
		[PreserveSig]
		int Lock_([In] int lFlags);

		// Token: 0x06000613 RID: 1555
		[PreserveSig]
		int Unlock_([In] int lFlags);
	}
}
