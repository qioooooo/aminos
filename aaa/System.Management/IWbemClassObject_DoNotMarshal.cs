using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x020000BC RID: 188
	[InterfaceType(1)]
	[Guid("DC12A681-737F-11CF-884D-00AA004B2E24")]
	[TypeLibType(512)]
	[ComImport]
	internal interface IWbemClassObject_DoNotMarshal
	{
		// Token: 0x060005A4 RID: 1444
		[PreserveSig]
		int GetQualifierSet_([MarshalAs(UnmanagedType.Interface)] out IWbemQualifierSet_DoNotMarshal ppQualSet);

		// Token: 0x060005A5 RID: 1445
		[PreserveSig]
		int Get_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, [In] [Out] ref object pVal, [In] [Out] ref int pType, [In] [Out] ref int plFlavor);

		// Token: 0x060005A6 RID: 1446
		[PreserveSig]
		int Put_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, [In] ref object pVal, [In] int Type);

		// Token: 0x060005A7 RID: 1447
		[PreserveSig]
		int Delete_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName);

		// Token: 0x060005A8 RID: 1448
		[PreserveSig]
		int GetNames_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszQualifierName, [In] int lFlags, [In] ref object pQualifierVal, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)] out string[] pNames);

		// Token: 0x060005A9 RID: 1449
		[PreserveSig]
		int BeginEnumeration_([In] int lEnumFlags);

		// Token: 0x060005AA RID: 1450
		[PreserveSig]
		int Next_([In] int lFlags, [MarshalAs(UnmanagedType.BStr)] [In] [Out] ref string strName, [In] [Out] ref object pVal, [In] [Out] ref int pType, [In] [Out] ref int plFlavor);

		// Token: 0x060005AB RID: 1451
		[PreserveSig]
		int EndEnumeration_();

		// Token: 0x060005AC RID: 1452
		[PreserveSig]
		int GetPropertyQualifierSet_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszProperty, [MarshalAs(UnmanagedType.Interface)] out IWbemQualifierSet_DoNotMarshal ppQualSet);

		// Token: 0x060005AD RID: 1453
		[PreserveSig]
		int Clone_([MarshalAs(UnmanagedType.Interface)] out IWbemClassObject_DoNotMarshal ppCopy);

		// Token: 0x060005AE RID: 1454
		[PreserveSig]
		int GetObjectText_([In] int lFlags, [MarshalAs(UnmanagedType.BStr)] out string pstrObjectText);

		// Token: 0x060005AF RID: 1455
		[PreserveSig]
		int SpawnDerivedClass_([In] int lFlags, [MarshalAs(UnmanagedType.Interface)] out IWbemClassObject_DoNotMarshal ppNewClass);

		// Token: 0x060005B0 RID: 1456
		[PreserveSig]
		int SpawnInstance_([In] int lFlags, [MarshalAs(UnmanagedType.Interface)] out IWbemClassObject_DoNotMarshal ppNewInstance);

		// Token: 0x060005B1 RID: 1457
		[PreserveSig]
		int CompareTo_([In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pCompareTo);

		// Token: 0x060005B2 RID: 1458
		[PreserveSig]
		int GetPropertyOrigin_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [MarshalAs(UnmanagedType.BStr)] out string pstrClassName);

		// Token: 0x060005B3 RID: 1459
		[PreserveSig]
		int InheritsFrom_([MarshalAs(UnmanagedType.LPWStr)] [In] string strAncestor);

		// Token: 0x060005B4 RID: 1460
		[PreserveSig]
		int GetMethod_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] out IWbemClassObject_DoNotMarshal ppInSignature, [MarshalAs(UnmanagedType.Interface)] out IWbemClassObject_DoNotMarshal ppOutSignature);

		// Token: 0x060005B5 RID: 1461
		[PreserveSig]
		int PutMethod_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pInSignature, [MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pOutSignature);

		// Token: 0x060005B6 RID: 1462
		[PreserveSig]
		int DeleteMethod_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName);

		// Token: 0x060005B7 RID: 1463
		[PreserveSig]
		int BeginMethodEnumeration_([In] int lEnumFlags);

		// Token: 0x060005B8 RID: 1464
		[PreserveSig]
		int NextMethod_([In] int lFlags, [MarshalAs(UnmanagedType.BStr)] [In] [Out] ref string pstrName, [MarshalAs(UnmanagedType.Interface)] [In] [Out] ref IWbemClassObject_DoNotMarshal ppInSignature, [MarshalAs(UnmanagedType.Interface)] [In] [Out] ref IWbemClassObject_DoNotMarshal ppOutSignature);

		// Token: 0x060005B9 RID: 1465
		[PreserveSig]
		int EndMethodEnumeration_();

		// Token: 0x060005BA RID: 1466
		[PreserveSig]
		int GetMethodQualifierSet_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszMethod, [MarshalAs(UnmanagedType.Interface)] out IWbemQualifierSet_DoNotMarshal ppQualSet);

		// Token: 0x060005BB RID: 1467
		[PreserveSig]
		int GetMethodOrigin_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszMethodName, [MarshalAs(UnmanagedType.BStr)] out string pstrClassName);
	}
}
