using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x020003E3 RID: 995
	internal static class FileDialogNative
	{
		// Token: 0x020003E4 RID: 996
		[Guid("42f85136-db7e-439c-85f1-e4075d135fc8")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface IFileDialog
		{
			// Token: 0x06003BC5 RID: 15301
			[PreserveSig]
			int Show([In] IntPtr parent);

			// Token: 0x06003BC6 RID: 15302
			void SetFileTypes([In] uint cFileTypes, [MarshalAs(UnmanagedType.LPArray)] [In] FileDialogNative.COMDLG_FILTERSPEC[] rgFilterSpec);

			// Token: 0x06003BC7 RID: 15303
			void SetFileTypeIndex([In] uint iFileType);

			// Token: 0x06003BC8 RID: 15304
			void GetFileTypeIndex(out uint piFileType);

			// Token: 0x06003BC9 RID: 15305
			void Advise([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IFileDialogEvents pfde, out uint pdwCookie);

			// Token: 0x06003BCA RID: 15306
			void Unadvise([In] uint dwCookie);

			// Token: 0x06003BCB RID: 15307
			void SetOptions([In] FileDialogNative.FOS fos);

			// Token: 0x06003BCC RID: 15308
			void GetOptions(out FileDialogNative.FOS pfos);

			// Token: 0x06003BCD RID: 15309
			void SetDefaultFolder([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi);

			// Token: 0x06003BCE RID: 15310
			void SetFolder([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi);

			// Token: 0x06003BCF RID: 15311
			void GetFolder([MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItem ppsi);

			// Token: 0x06003BD0 RID: 15312
			void GetCurrentSelection([MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItem ppsi);

			// Token: 0x06003BD1 RID: 15313
			void SetFileName([MarshalAs(UnmanagedType.LPWStr)] [In] string pszName);

			// Token: 0x06003BD2 RID: 15314
			void GetFileName([MarshalAs(UnmanagedType.LPWStr)] out string pszName);

			// Token: 0x06003BD3 RID: 15315
			void SetTitle([MarshalAs(UnmanagedType.LPWStr)] [In] string pszTitle);

			// Token: 0x06003BD4 RID: 15316
			void SetOkButtonLabel([MarshalAs(UnmanagedType.LPWStr)] [In] string pszText);

			// Token: 0x06003BD5 RID: 15317
			void SetFileNameLabel([MarshalAs(UnmanagedType.LPWStr)] [In] string pszLabel);

			// Token: 0x06003BD6 RID: 15318
			void GetResult([MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItem ppsi);

			// Token: 0x06003BD7 RID: 15319
			void AddPlace([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi, int alignment);

			// Token: 0x06003BD8 RID: 15320
			void SetDefaultExtension([MarshalAs(UnmanagedType.LPWStr)] [In] string pszDefaultExtension);

			// Token: 0x06003BD9 RID: 15321
			void Close([MarshalAs(UnmanagedType.Error)] int hr);

			// Token: 0x06003BDA RID: 15322
			void SetClientGuid([In] ref Guid guid);

			// Token: 0x06003BDB RID: 15323
			void ClearClientData();

			// Token: 0x06003BDC RID: 15324
			void SetFilter([MarshalAs(UnmanagedType.Interface)] IntPtr pFilter);
		}

		// Token: 0x020003E5 RID: 997
		[Guid("d57c7288-d4ad-4768-be02-9d969532d960")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface IFileOpenDialog : FileDialogNative.IFileDialog
		{
			// Token: 0x06003BDD RID: 15325
			[PreserveSig]
			int Show([In] IntPtr parent);

			// Token: 0x06003BDE RID: 15326
			void SetFileTypes([In] uint cFileTypes, [In] ref FileDialogNative.COMDLG_FILTERSPEC rgFilterSpec);

			// Token: 0x06003BDF RID: 15327
			void SetFileTypeIndex([In] uint iFileType);

			// Token: 0x06003BE0 RID: 15328
			void GetFileTypeIndex(out uint piFileType);

			// Token: 0x06003BE1 RID: 15329
			void Advise([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IFileDialogEvents pfde, out uint pdwCookie);

			// Token: 0x06003BE2 RID: 15330
			void Unadvise([In] uint dwCookie);

			// Token: 0x06003BE3 RID: 15331
			void SetOptions([In] FileDialogNative.FOS fos);

			// Token: 0x06003BE4 RID: 15332
			void GetOptions(out FileDialogNative.FOS pfos);

			// Token: 0x06003BE5 RID: 15333
			void SetDefaultFolder([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi);

			// Token: 0x06003BE6 RID: 15334
			void SetFolder([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi);

			// Token: 0x06003BE7 RID: 15335
			void GetFolder([MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItem ppsi);

			// Token: 0x06003BE8 RID: 15336
			void GetCurrentSelection([MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItem ppsi);

			// Token: 0x06003BE9 RID: 15337
			void SetFileName([MarshalAs(UnmanagedType.LPWStr)] [In] string pszName);

			// Token: 0x06003BEA RID: 15338
			void GetFileName([MarshalAs(UnmanagedType.LPWStr)] out string pszName);

			// Token: 0x06003BEB RID: 15339
			void SetTitle([MarshalAs(UnmanagedType.LPWStr)] [In] string pszTitle);

			// Token: 0x06003BEC RID: 15340
			void SetOkButtonLabel([MarshalAs(UnmanagedType.LPWStr)] [In] string pszText);

			// Token: 0x06003BED RID: 15341
			void SetFileNameLabel([MarshalAs(UnmanagedType.LPWStr)] [In] string pszLabel);

			// Token: 0x06003BEE RID: 15342
			void GetResult([MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItem ppsi);

			// Token: 0x06003BEF RID: 15343
			void AddPlace([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi, FileDialogCustomPlace fdcp);

			// Token: 0x06003BF0 RID: 15344
			void SetDefaultExtension([MarshalAs(UnmanagedType.LPWStr)] [In] string pszDefaultExtension);

			// Token: 0x06003BF1 RID: 15345
			void Close([MarshalAs(UnmanagedType.Error)] int hr);

			// Token: 0x06003BF2 RID: 15346
			void SetClientGuid([In] ref Guid guid);

			// Token: 0x06003BF3 RID: 15347
			void ClearClientData();

			// Token: 0x06003BF4 RID: 15348
			void SetFilter([MarshalAs(UnmanagedType.Interface)] IntPtr pFilter);

			// Token: 0x06003BF5 RID: 15349
			void GetResults([MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItemArray ppenum);

			// Token: 0x06003BF6 RID: 15350
			void GetSelectedItems([MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItemArray ppsai);
		}

		// Token: 0x020003E6 RID: 998
		[Guid("d57c7288-d4ad-4768-be02-9d969532d960")]
		[CoClass(typeof(FileDialogNative.FileOpenDialogRCW))]
		[ComImport]
		internal interface NativeFileOpenDialog : FileDialogNative.IFileOpenDialog, FileDialogNative.IFileDialog
		{
		}

		// Token: 0x020003E7 RID: 999
		[Guid("84bccd23-5fde-4cdb-aea4-af64b83d78ab")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface IFileSaveDialog : FileDialogNative.IFileDialog
		{
			// Token: 0x06003BF7 RID: 15351
			[PreserveSig]
			int Show([In] IntPtr parent);

			// Token: 0x06003BF8 RID: 15352
			void SetFileTypes([In] uint cFileTypes, [In] ref FileDialogNative.COMDLG_FILTERSPEC rgFilterSpec);

			// Token: 0x06003BF9 RID: 15353
			void SetFileTypeIndex([In] uint iFileType);

			// Token: 0x06003BFA RID: 15354
			void GetFileTypeIndex(out uint piFileType);

			// Token: 0x06003BFB RID: 15355
			void Advise([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IFileDialogEvents pfde, out uint pdwCookie);

			// Token: 0x06003BFC RID: 15356
			void Unadvise([In] uint dwCookie);

			// Token: 0x06003BFD RID: 15357
			void SetOptions([In] FileDialogNative.FOS fos);

			// Token: 0x06003BFE RID: 15358
			void GetOptions(out FileDialogNative.FOS pfos);

			// Token: 0x06003BFF RID: 15359
			void SetDefaultFolder([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi);

			// Token: 0x06003C00 RID: 15360
			void SetFolder([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi);

			// Token: 0x06003C01 RID: 15361
			void GetFolder([MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItem ppsi);

			// Token: 0x06003C02 RID: 15362
			void GetCurrentSelection([MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItem ppsi);

			// Token: 0x06003C03 RID: 15363
			void SetFileName([MarshalAs(UnmanagedType.LPWStr)] [In] string pszName);

			// Token: 0x06003C04 RID: 15364
			void GetFileName([MarshalAs(UnmanagedType.LPWStr)] out string pszName);

			// Token: 0x06003C05 RID: 15365
			void SetTitle([MarshalAs(UnmanagedType.LPWStr)] [In] string pszTitle);

			// Token: 0x06003C06 RID: 15366
			void SetOkButtonLabel([MarshalAs(UnmanagedType.LPWStr)] [In] string pszText);

			// Token: 0x06003C07 RID: 15367
			void SetFileNameLabel([MarshalAs(UnmanagedType.LPWStr)] [In] string pszLabel);

			// Token: 0x06003C08 RID: 15368
			void GetResult([MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItem ppsi);

			// Token: 0x06003C09 RID: 15369
			void AddPlace([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi, FileDialogCustomPlace fdcp);

			// Token: 0x06003C0A RID: 15370
			void SetDefaultExtension([MarshalAs(UnmanagedType.LPWStr)] [In] string pszDefaultExtension);

			// Token: 0x06003C0B RID: 15371
			void Close([MarshalAs(UnmanagedType.Error)] int hr);

			// Token: 0x06003C0C RID: 15372
			void SetClientGuid([In] ref Guid guid);

			// Token: 0x06003C0D RID: 15373
			void ClearClientData();

			// Token: 0x06003C0E RID: 15374
			void SetFilter([MarshalAs(UnmanagedType.Interface)] IntPtr pFilter);

			// Token: 0x06003C0F RID: 15375
			void SetSaveAsItem([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi);

			// Token: 0x06003C10 RID: 15376
			void SetProperties([MarshalAs(UnmanagedType.Interface)] [In] IntPtr pStore);

			// Token: 0x06003C11 RID: 15377
			void SetCollectedProperties([MarshalAs(UnmanagedType.Interface)] [In] IntPtr pList, [In] int fAppendDefault);

			// Token: 0x06003C12 RID: 15378
			void GetProperties([MarshalAs(UnmanagedType.Interface)] out IntPtr ppStore);

			// Token: 0x06003C13 RID: 15379
			void ApplyProperties([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi, [MarshalAs(UnmanagedType.Interface)] [In] IntPtr pStore, [ComAliasName("ShellObjects.wireHWND")] [In] ref IntPtr hwnd, [MarshalAs(UnmanagedType.Interface)] [In] IntPtr pSink);
		}

		// Token: 0x020003E8 RID: 1000
		[CoClass(typeof(FileDialogNative.FileSaveDialogRCW))]
		[Guid("84bccd23-5fde-4cdb-aea4-af64b83d78ab")]
		[ComImport]
		internal interface NativeFileSaveDialog : FileDialogNative.IFileSaveDialog, FileDialogNative.IFileDialog
		{
		}

		// Token: 0x020003E9 RID: 1001
		[TypeLibType(TypeLibTypeFlags.FCanCreate)]
		[ClassInterface(ClassInterfaceType.None)]
		[Guid("DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7")]
		[ComImport]
		internal class FileOpenDialogRCW
		{
			// Token: 0x06003C14 RID: 15380
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			public extern FileOpenDialogRCW();
		}

		// Token: 0x020003EA RID: 1002
		[TypeLibType(TypeLibTypeFlags.FCanCreate)]
		[ClassInterface(ClassInterfaceType.None)]
		[Guid("C0B4E2F3-BA21-4773-8DBA-335EC946EB8B")]
		[ComImport]
		internal class FileSaveDialogRCW
		{
			// Token: 0x06003C15 RID: 15381
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			public extern FileSaveDialogRCW();
		}

		// Token: 0x020003EB RID: 1003
		internal class IIDGuid
		{
			// Token: 0x06003C16 RID: 15382 RVA: 0x000D911C File Offset: 0x000D811C
			private IIDGuid()
			{
			}

			// Token: 0x04001DCC RID: 7628
			internal const string IModalWindow = "b4db1657-70d7-485e-8e3e-6fcb5a5c1802";

			// Token: 0x04001DCD RID: 7629
			internal const string IFileDialog = "42f85136-db7e-439c-85f1-e4075d135fc8";

			// Token: 0x04001DCE RID: 7630
			internal const string IFileOpenDialog = "d57c7288-d4ad-4768-be02-9d969532d960";

			// Token: 0x04001DCF RID: 7631
			internal const string IFileSaveDialog = "84bccd23-5fde-4cdb-aea4-af64b83d78ab";

			// Token: 0x04001DD0 RID: 7632
			internal const string IFileDialogEvents = "973510DB-7D7F-452B-8975-74A85828D354";

			// Token: 0x04001DD1 RID: 7633
			internal const string IShellItem = "43826D1E-E718-42EE-BC55-A1E261C37BFE";

			// Token: 0x04001DD2 RID: 7634
			internal const string IShellItemArray = "B63EA76D-1F85-456F-A19C-48159EFA858B";
		}

		// Token: 0x020003EC RID: 1004
		internal class CLSIDGuid
		{
			// Token: 0x06003C17 RID: 15383 RVA: 0x000D9124 File Offset: 0x000D8124
			private CLSIDGuid()
			{
			}

			// Token: 0x04001DD3 RID: 7635
			internal const string FileOpenDialog = "DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7";

			// Token: 0x04001DD4 RID: 7636
			internal const string FileSaveDialog = "C0B4E2F3-BA21-4773-8DBA-335EC946EB8B";
		}

		// Token: 0x020003ED RID: 1005
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("b4db1657-70d7-485e-8e3e-6fcb5a5c1802")]
		[ComImport]
		internal interface IModalWindow
		{
			// Token: 0x06003C18 RID: 15384
			[PreserveSig]
			int Show([In] IntPtr parent);
		}

		// Token: 0x020003EE RID: 1006
		internal enum SIATTRIBFLAGS
		{
			// Token: 0x04001DD6 RID: 7638
			SIATTRIBFLAGS_AND = 1,
			// Token: 0x04001DD7 RID: 7639
			SIATTRIBFLAGS_OR,
			// Token: 0x04001DD8 RID: 7640
			SIATTRIBFLAGS_APPCOMPAT
		}

		// Token: 0x020003EF RID: 1007
		[Guid("B63EA76D-1F85-456F-A19C-48159EFA858B")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface IShellItemArray
		{
			// Token: 0x06003C19 RID: 15385
			void BindToHandler([MarshalAs(UnmanagedType.Interface)] [In] IntPtr pbc, [In] ref Guid rbhid, [In] ref Guid riid, out IntPtr ppvOut);

			// Token: 0x06003C1A RID: 15386
			void GetPropertyStore([In] int Flags, [In] ref Guid riid, out IntPtr ppv);

			// Token: 0x06003C1B RID: 15387
			void GetPropertyDescriptionList([In] ref FileDialogNative.PROPERTYKEY keyType, [In] ref Guid riid, out IntPtr ppv);

			// Token: 0x06003C1C RID: 15388
			void GetAttributes([In] FileDialogNative.SIATTRIBFLAGS dwAttribFlags, [In] uint sfgaoMask, out uint psfgaoAttribs);

			// Token: 0x06003C1D RID: 15389
			void GetCount(out uint pdwNumItems);

			// Token: 0x06003C1E RID: 15390
			void GetItemAt([In] uint dwIndex, [MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItem ppsi);

			// Token: 0x06003C1F RID: 15391
			void EnumItems([MarshalAs(UnmanagedType.Interface)] out IntPtr ppenumShellItems);
		}

		// Token: 0x020003F0 RID: 1008
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		internal struct PROPERTYKEY
		{
			// Token: 0x04001DD9 RID: 7641
			internal Guid fmtid;

			// Token: 0x04001DDA RID: 7642
			internal uint pid;
		}

		// Token: 0x020003F1 RID: 1009
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("973510DB-7D7F-452B-8975-74A85828D354")]
		[ComImport]
		internal interface IFileDialogEvents
		{
			// Token: 0x06003C20 RID: 15392
			[PreserveSig]
			int OnFileOk([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IFileDialog pfd);

			// Token: 0x06003C21 RID: 15393
			[PreserveSig]
			int OnFolderChanging([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IFileDialog pfd, [MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psiFolder);

			// Token: 0x06003C22 RID: 15394
			void OnFolderChange([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IFileDialog pfd);

			// Token: 0x06003C23 RID: 15395
			void OnSelectionChange([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IFileDialog pfd);

			// Token: 0x06003C24 RID: 15396
			void OnShareViolation([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IFileDialog pfd, [MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi, out FileDialogNative.FDE_SHAREVIOLATION_RESPONSE pResponse);

			// Token: 0x06003C25 RID: 15397
			void OnTypeChange([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IFileDialog pfd);

			// Token: 0x06003C26 RID: 15398
			void OnOverwrite([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IFileDialog pfd, [MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi, out FileDialogNative.FDE_OVERWRITE_RESPONSE pResponse);
		}

		// Token: 0x020003F2 RID: 1010
		[Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface IShellItem
		{
			// Token: 0x06003C27 RID: 15399
			void BindToHandler([MarshalAs(UnmanagedType.Interface)] [In] IntPtr pbc, [In] ref Guid bhid, [In] ref Guid riid, out IntPtr ppv);

			// Token: 0x06003C28 RID: 15400
			void GetParent([MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItem ppsi);

			// Token: 0x06003C29 RID: 15401
			void GetDisplayName([In] FileDialogNative.SIGDN sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);

			// Token: 0x06003C2A RID: 15402
			void GetAttributes([In] uint sfgaoMask, out uint psfgaoAttribs);

			// Token: 0x06003C2B RID: 15403
			void Compare([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi, [In] uint hint, out int piOrder);
		}

		// Token: 0x020003F3 RID: 1011
		internal enum SIGDN : uint
		{
			// Token: 0x04001DDC RID: 7644
			SIGDN_NORMALDISPLAY,
			// Token: 0x04001DDD RID: 7645
			SIGDN_PARENTRELATIVEPARSING = 2147581953U,
			// Token: 0x04001DDE RID: 7646
			SIGDN_DESKTOPABSOLUTEPARSING = 2147647488U,
			// Token: 0x04001DDF RID: 7647
			SIGDN_PARENTRELATIVEEDITING = 2147684353U,
			// Token: 0x04001DE0 RID: 7648
			SIGDN_DESKTOPABSOLUTEEDITING = 2147794944U,
			// Token: 0x04001DE1 RID: 7649
			SIGDN_FILESYSPATH = 2147844096U,
			// Token: 0x04001DE2 RID: 7650
			SIGDN_URL = 2147909632U,
			// Token: 0x04001DE3 RID: 7651
			SIGDN_PARENTRELATIVEFORADDRESSBAR = 2147991553U,
			// Token: 0x04001DE4 RID: 7652
			SIGDN_PARENTRELATIVE = 2148007937U
		}

		// Token: 0x020003F4 RID: 1012
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
		internal struct COMDLG_FILTERSPEC
		{
			// Token: 0x04001DE5 RID: 7653
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string pszName;

			// Token: 0x04001DE6 RID: 7654
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string pszSpec;
		}

		// Token: 0x020003F5 RID: 1013
		[Flags]
		internal enum FOS : uint
		{
			// Token: 0x04001DE8 RID: 7656
			FOS_OVERWRITEPROMPT = 2U,
			// Token: 0x04001DE9 RID: 7657
			FOS_STRICTFILETYPES = 4U,
			// Token: 0x04001DEA RID: 7658
			FOS_NOCHANGEDIR = 8U,
			// Token: 0x04001DEB RID: 7659
			FOS_PICKFOLDERS = 32U,
			// Token: 0x04001DEC RID: 7660
			FOS_FORCEFILESYSTEM = 64U,
			// Token: 0x04001DED RID: 7661
			FOS_ALLNONSTORAGEITEMS = 128U,
			// Token: 0x04001DEE RID: 7662
			FOS_NOVALIDATE = 256U,
			// Token: 0x04001DEF RID: 7663
			FOS_ALLOWMULTISELECT = 512U,
			// Token: 0x04001DF0 RID: 7664
			FOS_PATHMUSTEXIST = 2048U,
			// Token: 0x04001DF1 RID: 7665
			FOS_FILEMUSTEXIST = 4096U,
			// Token: 0x04001DF2 RID: 7666
			FOS_CREATEPROMPT = 8192U,
			// Token: 0x04001DF3 RID: 7667
			FOS_SHAREAWARE = 16384U,
			// Token: 0x04001DF4 RID: 7668
			FOS_NOREADONLYRETURN = 32768U,
			// Token: 0x04001DF5 RID: 7669
			FOS_NOTESTFILECREATE = 65536U,
			// Token: 0x04001DF6 RID: 7670
			FOS_HIDEMRUPLACES = 131072U,
			// Token: 0x04001DF7 RID: 7671
			FOS_HIDEPINNEDPLACES = 262144U,
			// Token: 0x04001DF8 RID: 7672
			FOS_NODEREFERENCELINKS = 1048576U,
			// Token: 0x04001DF9 RID: 7673
			FOS_DONTADDTORECENT = 33554432U,
			// Token: 0x04001DFA RID: 7674
			FOS_FORCESHOWHIDDEN = 268435456U,
			// Token: 0x04001DFB RID: 7675
			FOS_DEFAULTNOMINIMODE = 536870912U
		}

		// Token: 0x020003F6 RID: 1014
		internal enum FDE_SHAREVIOLATION_RESPONSE
		{
			// Token: 0x04001DFD RID: 7677
			FDESVR_DEFAULT,
			// Token: 0x04001DFE RID: 7678
			FDESVR_ACCEPT,
			// Token: 0x04001DFF RID: 7679
			FDESVR_REFUSE
		}

		// Token: 0x020003F7 RID: 1015
		internal enum FDE_OVERWRITE_RESPONSE
		{
			// Token: 0x04001E01 RID: 7681
			FDEOR_DEFAULT,
			// Token: 0x04001E02 RID: 7682
			FDEOR_ACCEPT,
			// Token: 0x04001E03 RID: 7683
			FDEOR_REFUSE
		}
	}
}
