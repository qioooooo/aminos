using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.EnterpriseServices.Admin
{
	// Token: 0x0200005A RID: 90
	[SuppressUnmanagedCodeSecurity]
	[Guid("DD662187-DFC2-11D1-A2CF-00805FC79235")]
	[ComImport]
	internal interface ICatalog
	{
		// Token: 0x060001AC RID: 428
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetCollection([MarshalAs(UnmanagedType.BStr)] [In] string bstrCollName);

		// Token: 0x060001AD RID: 429
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.Interface)]
		object Connect([MarshalAs(UnmanagedType.BStr)] [In] string connectStr);

		// Token: 0x060001AE RID: 430
		[DispId(3)]
		int MajorVersion();

		// Token: 0x060001AF RID: 431
		[DispId(4)]
		int MinorVersion();

		// Token: 0x060001B0 RID: 432
		[DispId(5)]
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetCollectionByQuery([MarshalAs(UnmanagedType.BStr)] [In] string collName, [MarshalAs(UnmanagedType.SafeArray)] [In] ref object[] aQuery);

		// Token: 0x060001B1 RID: 433
		[DispId(6)]
		void ImportComponent([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplIdOrName, [MarshalAs(UnmanagedType.BStr)] [In] string bstrCLSIDOrProgId);

		// Token: 0x060001B2 RID: 434
		[DispId(7)]
		void InstallComponent([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplIdOrName, [MarshalAs(UnmanagedType.BStr)] [In] string bstrDLL, [MarshalAs(UnmanagedType.BStr)] [In] string bstrTLB, [MarshalAs(UnmanagedType.BStr)] [In] string bstrPSDLL);

		// Token: 0x060001B3 RID: 435
		[DispId(8)]
		void ShutdownApplication([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplIdOrName);

		// Token: 0x060001B4 RID: 436
		[DispId(9)]
		void ExportApplication([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplIdOrName, [MarshalAs(UnmanagedType.BStr)] [In] string bstrApplicationFile, [In] int lOptions);

		// Token: 0x060001B5 RID: 437
		[DispId(10)]
		void InstallApplication([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplicationFile, [MarshalAs(UnmanagedType.BStr)] [In] string bstrDestinationDirectory, [In] int lOptions, [MarshalAs(UnmanagedType.BStr)] [In] string bstrUserId, [MarshalAs(UnmanagedType.BStr)] [In] string bstrPassword, [MarshalAs(UnmanagedType.BStr)] [In] string bstrRSN);

		// Token: 0x060001B6 RID: 438
		[DispId(11)]
		void StopRouter();

		// Token: 0x060001B7 RID: 439
		[DispId(12)]
		void RefreshRouter();

		// Token: 0x060001B8 RID: 440
		[DispId(13)]
		void StartRouter();

		// Token: 0x060001B9 RID: 441
		[DispId(14)]
		void Reserved1();

		// Token: 0x060001BA RID: 442
		[DispId(15)]
		void Reserved2();

		// Token: 0x060001BB RID: 443
		[DispId(16)]
		void InstallMultipleComponents([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplIdOrName, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_VARIANT)] [In] ref object[] fileNames, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_VARIANT)] [In] ref object[] CLSIDS);

		// Token: 0x060001BC RID: 444
		[DispId(17)]
		void GetMultipleComponentsInfo([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplIdOrName, [In] object varFileNames, [MarshalAs(UnmanagedType.SafeArray)] out object[] varCLSIDS, [MarshalAs(UnmanagedType.SafeArray)] out object[] varClassNames, [MarshalAs(UnmanagedType.SafeArray)] out object[] varFileFlags, [MarshalAs(UnmanagedType.SafeArray)] out object[] varComponentFlags);

		// Token: 0x060001BD RID: 445
		[DispId(18)]
		void RefreshComponents();

		// Token: 0x060001BE RID: 446
		[DispId(19)]
		void BackupREGDB([MarshalAs(UnmanagedType.BStr)] [In] string bstrBackupFilePath);

		// Token: 0x060001BF RID: 447
		[DispId(20)]
		void RestoreREGDB([MarshalAs(UnmanagedType.BStr)] [In] string bstrBackupFilePath);

		// Token: 0x060001C0 RID: 448
		[DispId(21)]
		void QueryApplicationFile([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplicationFile, [MarshalAs(UnmanagedType.BStr)] out string bstrApplicationName, [MarshalAs(UnmanagedType.BStr)] out string bstrApplicationDescription, [MarshalAs(UnmanagedType.VariantBool)] out bool bHasUsers, [MarshalAs(UnmanagedType.VariantBool)] out bool bIsProxy, [MarshalAs(UnmanagedType.SafeArray)] out object[] varFileNames);

		// Token: 0x060001C1 RID: 449
		[DispId(22)]
		void StartApplication([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplIdOrName);

		// Token: 0x060001C2 RID: 450
		[DispId(23)]
		int ServiceCheck([In] int lService);

		// Token: 0x060001C3 RID: 451
		[DispId(24)]
		void InstallMultipleEventClasses([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplIdOrName, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_VARIANT)] [In] ref object[] fileNames, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_VARIANT)] [In] ref object[] CLSIDS);

		// Token: 0x060001C4 RID: 452
		[DispId(25)]
		void InstallEventClass([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplIdOrName, [MarshalAs(UnmanagedType.BStr)] [In] string bstrDLL, [MarshalAs(UnmanagedType.BStr)] [In] string bstrTLB, [MarshalAs(UnmanagedType.BStr)] [In] string bstrPSDLL);

		// Token: 0x060001C5 RID: 453
		[DispId(26)]
		void GetEventClassesForIID([In] string bstrIID, [MarshalAs(UnmanagedType.SafeArray)] [In] [Out] ref object[] varCLSIDS, [MarshalAs(UnmanagedType.SafeArray)] [In] [Out] ref object[] varProgIDs, [MarshalAs(UnmanagedType.SafeArray)] [In] [Out] ref object[] varDescriptions);
	}
}
