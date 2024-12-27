using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x02000691 RID: 1681
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("70b51430-b6ca-11d0-b9b9-00a0c922e750")]
	[ComImport]
	internal interface IMSAdminBase
	{
		// Token: 0x060033DF RID: 13279
		[PreserveSig]
		int AddKey(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string Path);

		// Token: 0x060033E0 RID: 13280
		[PreserveSig]
		int DeleteKey(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string Path);

		// Token: 0x060033E1 RID: 13281
		void DeleteChildKeys(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string Path);

		// Token: 0x060033E2 RID: 13282
		[PreserveSig]
		int EnumKeys(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string Path, StringBuilder Buffer, int EnumKeyIndex);

		// Token: 0x060033E3 RID: 13283
		void CopyKey(IntPtr source, [MarshalAs(UnmanagedType.LPWStr)] string SourcePath, IntPtr dest, [MarshalAs(UnmanagedType.LPWStr)] string DestPath, bool OverwriteFlag, bool CopyFlag);

		// Token: 0x060033E4 RID: 13284
		void RenameKey(IntPtr key, [MarshalAs(UnmanagedType.LPWStr)] string path, [MarshalAs(UnmanagedType.LPWStr)] string newName);

		// Token: 0x060033E5 RID: 13285
		[PreserveSig]
		int SetData(IntPtr key, [MarshalAs(UnmanagedType.LPWStr)] string path, ref MetadataRecord data);

		// Token: 0x060033E6 RID: 13286
		[PreserveSig]
		int GetData(IntPtr key, [MarshalAs(UnmanagedType.LPWStr)] string path, ref MetadataRecord data, [In] [Out] ref uint RequiredDataLen);

		// Token: 0x060033E7 RID: 13287
		[PreserveSig]
		int DeleteData(IntPtr key, [MarshalAs(UnmanagedType.LPWStr)] string path, uint Identifier, uint DataType);

		// Token: 0x060033E8 RID: 13288
		[PreserveSig]
		int EnumData(IntPtr key, [MarshalAs(UnmanagedType.LPWStr)] string path, ref MetadataRecord data, int EnumDataIndex, [In] [Out] ref uint RequiredDataLen);

		// Token: 0x060033E9 RID: 13289
		[PreserveSig]
		int GetAllData(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string Path, uint Attributes, uint UserType, uint DataType, [In] [Out] ref uint NumDataEntries, [In] [Out] ref uint DataSetNumber, uint BufferSize, IntPtr buffer, [In] [Out] ref uint RequiredBufferSize);

		// Token: 0x060033EA RID: 13290
		void DeleteAllData(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string Path, uint UserType, uint DataType);

		// Token: 0x060033EB RID: 13291
		[PreserveSig]
		int CopyData(IntPtr sourcehandle, [MarshalAs(UnmanagedType.LPWStr)] string SourcePath, IntPtr desthandle, [MarshalAs(UnmanagedType.LPWStr)] string DestPath, int Attributes, int UserType, int DataType, [MarshalAs(UnmanagedType.Bool)] bool CopyFlag);

		// Token: 0x060033EC RID: 13292
		[PreserveSig]
		void GetDataPaths(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string Path, int Identifier, int DataType, int BufferSize, [MarshalAs(UnmanagedType.LPWStr)] out char[] Buffer, [MarshalAs(UnmanagedType.U4)] [In] [Out] ref int RequiredBufferSize);

		// Token: 0x060033ED RID: 13293
		[PreserveSig]
		int OpenKey(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string Path, [MarshalAs(UnmanagedType.U4)] MBKeyAccess AccessRequested, int TimeOut, [In] [Out] ref IntPtr NewHandle);

		// Token: 0x060033EE RID: 13294
		[PreserveSig]
		int CloseKey(IntPtr handle);

		// Token: 0x060033EF RID: 13295
		void ChangePermissions(IntPtr handle, int TimeOut, [MarshalAs(UnmanagedType.U4)] MBKeyAccess AccessRequested);

		// Token: 0x060033F0 RID: 13296
		void SaveData();

		// Token: 0x060033F1 RID: 13297
		[PreserveSig]
		void GetHandleInfo(IntPtr handle, [In] [Out] ref _METADATA_HANDLE_INFO Info);

		// Token: 0x060033F2 RID: 13298
		[PreserveSig]
		void GetSystemChangeNumber([MarshalAs(UnmanagedType.U4)] [In] [Out] ref uint SystemChangeNumber);

		// Token: 0x060033F3 RID: 13299
		[PreserveSig]
		void GetDataSetNumber(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string Path, [In] [Out] ref uint DataSetNumber);

		// Token: 0x060033F4 RID: 13300
		[PreserveSig]
		void SetLastChangeTime(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string Path, out global::System.Runtime.InteropServices.ComTypes.FILETIME LastChangeTime, bool LocalTime);

		// Token: 0x060033F5 RID: 13301
		[PreserveSig]
		int GetLastChangeTime(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string Path, [In] [Out] ref global::System.Runtime.InteropServices.ComTypes.FILETIME LastChangeTime, bool LocalTime);

		// Token: 0x060033F6 RID: 13302
		[PreserveSig]
		int KeyExchangePhase1();

		// Token: 0x060033F7 RID: 13303
		[PreserveSig]
		int KeyExchangePhase2();

		// Token: 0x060033F8 RID: 13304
		[PreserveSig]
		int Backup([MarshalAs(UnmanagedType.LPWStr)] string Location, int Version, int Flags);

		// Token: 0x060033F9 RID: 13305
		[PreserveSig]
		int Restore([MarshalAs(UnmanagedType.LPWStr)] string Location, int Version, int Flags);

		// Token: 0x060033FA RID: 13306
		[PreserveSig]
		void EnumBackups([MarshalAs(UnmanagedType.LPWStr)] out string Location, [MarshalAs(UnmanagedType.U4)] out uint Version, out global::System.Runtime.InteropServices.ComTypes.FILETIME BackupTime, uint EnumIndex);

		// Token: 0x060033FB RID: 13307
		[PreserveSig]
		void DeleteBackup([MarshalAs(UnmanagedType.LPWStr)] string Location, int Version);

		// Token: 0x060033FC RID: 13308
		[PreserveSig]
		int UnmarshalInterface([MarshalAs(UnmanagedType.Interface)] out IMSAdminBase interf);

		// Token: 0x060033FD RID: 13309
		[PreserveSig]
		int GetServerGuid();
	}
}
