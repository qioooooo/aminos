using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.EnterpriseServices.Admin
{
	// Token: 0x0200005B RID: 91
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[Guid("790C6E0B-9194-4cc9-9426-A48A63185696")]
	[SuppressUnmanagedCodeSecurity]
	[ComImport]
	internal interface ICatalog2
	{
		// Token: 0x060001C6 RID: 454
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetCollection([MarshalAs(UnmanagedType.BStr)] [In] string bstrCollName);

		// Token: 0x060001C7 RID: 455
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.Interface)]
		object Connect([MarshalAs(UnmanagedType.BStr)] [In] string connectStr);

		// Token: 0x060001C8 RID: 456
		[DispId(3)]
		int MajorVersion();

		// Token: 0x060001C9 RID: 457
		[DispId(4)]
		int MinorVersion();

		// Token: 0x060001CA RID: 458
		[DispId(5)]
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetCollectionByQuery([MarshalAs(UnmanagedType.BStr)] [In] string collName, [MarshalAs(UnmanagedType.SafeArray)] [In] ref object[] aQuery);

		// Token: 0x060001CB RID: 459
		[DispId(6)]
		void ImportComponent([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplIdOrName, [MarshalAs(UnmanagedType.BStr)] [In] string bstrCLSIDOrProgId);

		// Token: 0x060001CC RID: 460
		[DispId(7)]
		void InstallComponent([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplIdOrName, [MarshalAs(UnmanagedType.BStr)] [In] string bstrDLL, [MarshalAs(UnmanagedType.BStr)] [In] string bstrTLB, [MarshalAs(UnmanagedType.BStr)] [In] string bstrPSDLL);

		// Token: 0x060001CD RID: 461
		[DispId(8)]
		void ShutdownApplication([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplIdOrName);

		// Token: 0x060001CE RID: 462
		[DispId(9)]
		void ExportApplication([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplIdOrName, [MarshalAs(UnmanagedType.BStr)] [In] string bstrApplicationFile, [In] int lOptions);

		// Token: 0x060001CF RID: 463
		[DispId(10)]
		void InstallApplication([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplicationFile, [MarshalAs(UnmanagedType.BStr)] [In] string bstrDestinationDirectory, [In] int lOptions, [MarshalAs(UnmanagedType.BStr)] [In] string bstrUserId, [MarshalAs(UnmanagedType.BStr)] [In] string bstrPassword, [MarshalAs(UnmanagedType.BStr)] [In] string bstrRSN);

		// Token: 0x060001D0 RID: 464
		[DispId(11)]
		void StopRouter();

		// Token: 0x060001D1 RID: 465
		[DispId(12)]
		void RefreshRouter();

		// Token: 0x060001D2 RID: 466
		[DispId(13)]
		void StartRouter();

		// Token: 0x060001D3 RID: 467
		[DispId(14)]
		void Reserved1();

		// Token: 0x060001D4 RID: 468
		[DispId(15)]
		void Reserved2();

		// Token: 0x060001D5 RID: 469
		[DispId(16)]
		void InstallMultipleComponents([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplIdOrName, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_VARIANT)] [In] ref object[] fileNames, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_VARIANT)] [In] ref object[] CLSIDS);

		// Token: 0x060001D6 RID: 470
		[DispId(17)]
		void GetMultipleComponentsInfo([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplIdOrName, [In] object varFileNames, [MarshalAs(UnmanagedType.SafeArray)] out object[] varCLSIDS, [MarshalAs(UnmanagedType.SafeArray)] out object[] varClassNames, [MarshalAs(UnmanagedType.SafeArray)] out object[] varFileFlags, [MarshalAs(UnmanagedType.SafeArray)] out object[] varComponentFlags);

		// Token: 0x060001D7 RID: 471
		[DispId(18)]
		void RefreshComponents();

		// Token: 0x060001D8 RID: 472
		[DispId(19)]
		void BackupREGDB([MarshalAs(UnmanagedType.BStr)] [In] string bstrBackupFilePath);

		// Token: 0x060001D9 RID: 473
		[DispId(20)]
		void RestoreREGDB([MarshalAs(UnmanagedType.BStr)] [In] string bstrBackupFilePath);

		// Token: 0x060001DA RID: 474
		[DispId(21)]
		void QueryApplicationFile([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplicationFile, [MarshalAs(UnmanagedType.BStr)] out string bstrApplicationName, [MarshalAs(UnmanagedType.BStr)] out string bstrApplicationDescription, [MarshalAs(UnmanagedType.VariantBool)] out bool bHasUsers, [MarshalAs(UnmanagedType.VariantBool)] out bool bIsProxy, [MarshalAs(UnmanagedType.SafeArray)] out object[] varFileNames);

		// Token: 0x060001DB RID: 475
		[DispId(22)]
		void StartApplication([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplIdOrName);

		// Token: 0x060001DC RID: 476
		[DispId(23)]
		int ServiceCheck([In] int lService);

		// Token: 0x060001DD RID: 477
		[DispId(24)]
		void InstallMultipleEventClasses([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplIdOrName, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_VARIANT)] [In] ref object[] fileNames, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_VARIANT)] [In] ref object[] CLSIDS);

		// Token: 0x060001DE RID: 478
		[DispId(25)]
		void InstallEventClass([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplIdOrName, [MarshalAs(UnmanagedType.BStr)] [In] string bstrDLL, [MarshalAs(UnmanagedType.BStr)] [In] string bstrTLB, [MarshalAs(UnmanagedType.BStr)] [In] string bstrPSDLL);

		// Token: 0x060001DF RID: 479
		[DispId(26)]
		void GetEventClassesForIID([In] string bstrIID, [MarshalAs(UnmanagedType.SafeArray)] [In] [Out] ref object[] varCLSIDS, [MarshalAs(UnmanagedType.SafeArray)] [In] [Out] ref object[] varProgIDs, [MarshalAs(UnmanagedType.SafeArray)] [In] [Out] ref object[] varDescriptions);

		// Token: 0x060001E0 RID: 480
		[DispId(27)]
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetCollectionByQuery2([MarshalAs(UnmanagedType.BStr)] [In] string bstrCollectionName, [MarshalAs(UnmanagedType.LPStruct)] [In] object pVarQueryStrings);

		// Token: 0x060001E1 RID: 481
		[DispId(28)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetApplicationInstanceIDFromProcessID([MarshalAs(UnmanagedType.I4)] [In] int lProcessID);

		// Token: 0x060001E2 RID: 482
		[DispId(29)]
		void ShutdownApplicationInstances([MarshalAs(UnmanagedType.LPStruct)] [In] object pVarApplicationInstanceID);

		// Token: 0x060001E3 RID: 483
		[DispId(30)]
		void PauseApplicationInstances([MarshalAs(UnmanagedType.LPStruct)] [In] object pVarApplicationInstanceID);

		// Token: 0x060001E4 RID: 484
		[DispId(31)]
		void ResumeApplicationInstances([MarshalAs(UnmanagedType.LPStruct)] [In] object pVarApplicationInstanceID);

		// Token: 0x060001E5 RID: 485
		[DispId(32)]
		void RecycleApplicationInstances([MarshalAs(UnmanagedType.LPStruct)] [In] object pVarApplicationInstanceID, [MarshalAs(UnmanagedType.I4)] [In] int lReasonCode);

		// Token: 0x060001E6 RID: 486
		[DispId(33)]
		[return: MarshalAs(UnmanagedType.VariantBool)]
		bool AreApplicationInstancesPaused([MarshalAs(UnmanagedType.LPStruct)] [In] object pVarApplicationInstanceID);

		// Token: 0x060001E7 RID: 487
		[DispId(34)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string DumpApplicationInstance([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplicationInstanceID, [MarshalAs(UnmanagedType.BStr)] [In] string bstrDirectory, [MarshalAs(UnmanagedType.I4)] [In] int lMaxImages);

		// Token: 0x060001E8 RID: 488
		[DispId(35)]
		[return: MarshalAs(UnmanagedType.VariantBool)]
		bool IsApplicationInstanceDumpSupported();

		// Token: 0x060001E9 RID: 489
		[DispId(36)]
		void CreateServiceForApplication([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplicationIDOrName, [MarshalAs(UnmanagedType.BStr)] [In] string bstrServiceName, [MarshalAs(UnmanagedType.BStr)] [In] string bstrStartType, [MarshalAs(UnmanagedType.BStr)] [In] string bstrErrorControl, [MarshalAs(UnmanagedType.BStr)] [In] string bstrDependencies, [MarshalAs(UnmanagedType.BStr)] [In] string bstrRunAs, [MarshalAs(UnmanagedType.BStr)] [In] string bstrPassword, [MarshalAs(UnmanagedType.VariantBool)] [In] bool bDesktopOk);

		// Token: 0x060001EA RID: 490
		[DispId(37)]
		void DeleteServiceForApplication([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplicationIDOrName);

		// Token: 0x060001EB RID: 491
		[DispId(38)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetPartitionID([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplicationIDOrName);

		// Token: 0x060001EC RID: 492
		[DispId(39)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetPartitionName([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplicationIDOrName);

		// Token: 0x060001ED RID: 493
		[DispId(40)]
		void CurrentPartition([MarshalAs(UnmanagedType.BStr)] [In] string bstrPartitionIDOrName);

		// Token: 0x060001EE RID: 494
		[DispId(41)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string CurrentPartitionID();

		// Token: 0x060001EF RID: 495
		[DispId(42)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string CurrentPartitionName();

		// Token: 0x060001F0 RID: 496
		[DispId(43)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GlobalPartitionID();

		// Token: 0x060001F1 RID: 497
		[DispId(44)]
		void FlushPartitionCache();

		// Token: 0x060001F2 RID: 498
		[DispId(45)]
		void CopyApplications([MarshalAs(UnmanagedType.BStr)] [In] string bstrSourcePartitionIDOrName, [MarshalAs(UnmanagedType.LPStruct)] [In] object pVarApplicationID, [MarshalAs(UnmanagedType.BStr)] [In] string bstrDestinationPartitionIDOrName);

		// Token: 0x060001F3 RID: 499
		[DispId(46)]
		void CopyComponents([MarshalAs(UnmanagedType.BStr)] [In] string bstrSourceApplicationIDOrName, [MarshalAs(UnmanagedType.LPStruct)] [In] object pVarCLSIDOrProgID, [MarshalAs(UnmanagedType.BStr)] [In] string bstrDestinationApplicationIDOrName);

		// Token: 0x060001F4 RID: 500
		[DispId(47)]
		void MoveComponents([MarshalAs(UnmanagedType.BStr)] [In] string bstrSourceApplicationIDOrName, [MarshalAs(UnmanagedType.LPStruct)] [In] object pVarCLSIDOrProgID, [MarshalAs(UnmanagedType.BStr)] [In] string bstrDestinationApplicationIDOrName);

		// Token: 0x060001F5 RID: 501
		[DispId(48)]
		void AliasComponent([MarshalAs(UnmanagedType.BStr)] [In] string bstrSrcApplicationIDOrName, [MarshalAs(UnmanagedType.BStr)] [In] string bstrCLSIDOrProgID, [MarshalAs(UnmanagedType.BStr)] [In] string bstrDestApplicationIDOrName, [MarshalAs(UnmanagedType.BStr)] [In] string bstrNewProgId, [MarshalAs(UnmanagedType.BStr)] [In] string bstrNewClsid);

		// Token: 0x060001F6 RID: 502
		[DispId(49)]
		[return: MarshalAs(UnmanagedType.Interface)]
		object IsSafeToDelete([MarshalAs(UnmanagedType.BStr)] [In] string bstrDllName);

		// Token: 0x060001F7 RID: 503
		[DispId(50)]
		void ImportUnconfiguredComponents([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplicationIDOrName, [MarshalAs(UnmanagedType.LPStruct)] [In] object pVarCLSIDOrProgID, [MarshalAs(UnmanagedType.LPStruct)] [In] object pVarComponentType);

		// Token: 0x060001F8 RID: 504
		[DispId(51)]
		void PromoteUnconfiguredComponents([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplicationIDOrName, [MarshalAs(UnmanagedType.LPStruct)] [In] object pVarCLSIDOrProgID, [MarshalAs(UnmanagedType.LPStruct)] [In] object pVarComponentType);

		// Token: 0x060001F9 RID: 505
		[DispId(52)]
		void ImportComponents([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplicationIDOrName, [In] ref object pVarCLSIDOrProgID, [In] ref object pVarComponentType);

		// Token: 0x060001FA RID: 506
		[DispId(53)]
		[return: MarshalAs(UnmanagedType.VariantBool)]
		bool Is64BitCatalogServer();

		// Token: 0x060001FB RID: 507
		[DispId(54)]
		void ExportPartition([MarshalAs(UnmanagedType.BStr)] [In] string bstrPartitionIDOrName, [MarshalAs(UnmanagedType.BStr)] [In] string bstrPartitionFileName, [MarshalAs(UnmanagedType.I4)] [In] int lOptions);

		// Token: 0x060001FC RID: 508
		[DispId(55)]
		void InstallPartition([MarshalAs(UnmanagedType.BStr)] [In] string bstrFileName, [MarshalAs(UnmanagedType.BStr)] [In] string bstrDestDirectory, [MarshalAs(UnmanagedType.I4)] [In] int lOptions, [MarshalAs(UnmanagedType.BStr)] [In] string bstrUserID, [MarshalAs(UnmanagedType.BStr)] [In] string bstrPassword, [MarshalAs(UnmanagedType.BStr)] [In] string bstrRSN);

		// Token: 0x060001FD RID: 509
		[DispId(56)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		object QueryApplicationFile2([MarshalAs(UnmanagedType.BStr)] [In] string bstrApplicationFile);

		// Token: 0x060001FE RID: 510
		[DispId(57)]
		[return: MarshalAs(UnmanagedType.I4)]
		int GetComponentVersionCount([MarshalAs(UnmanagedType.BStr)] [In] string bstrCLSIDOrProgID);
	}
}
