using System;
using System.ComponentModel;
using System.IO;
using System.Security;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x020005A9 RID: 1449
	[SRDescription("DescriptionOpenFileDialog")]
	public sealed class OpenFileDialog : FileDialog
	{
		// Token: 0x17000ECF RID: 3791
		// (get) Token: 0x06004B17 RID: 19223 RVA: 0x00110370 File Offset: 0x0010F370
		// (set) Token: 0x06004B18 RID: 19224 RVA: 0x00110378 File Offset: 0x0010F378
		[SRDescription("OFDcheckFileExistsDescr")]
		[DefaultValue(true)]
		public override bool CheckFileExists
		{
			get
			{
				return base.CheckFileExists;
			}
			set
			{
				base.CheckFileExists = value;
			}
		}

		// Token: 0x17000ED0 RID: 3792
		// (get) Token: 0x06004B19 RID: 19225 RVA: 0x00110381 File Offset: 0x0010F381
		// (set) Token: 0x06004B1A RID: 19226 RVA: 0x0011038E File Offset: 0x0010F38E
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("OFDmultiSelectDescr")]
		public bool Multiselect
		{
			get
			{
				return base.GetOption(512);
			}
			set
			{
				base.SetOption(512, value);
			}
		}

		// Token: 0x17000ED1 RID: 3793
		// (get) Token: 0x06004B1B RID: 19227 RVA: 0x0011039C File Offset: 0x0010F39C
		// (set) Token: 0x06004B1C RID: 19228 RVA: 0x001103A5 File Offset: 0x0010F3A5
		[SRDescription("OFDreadOnlyCheckedDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		public bool ReadOnlyChecked
		{
			get
			{
				return base.GetOption(1);
			}
			set
			{
				base.SetOption(1, value);
			}
		}

		// Token: 0x17000ED2 RID: 3794
		// (get) Token: 0x06004B1D RID: 19229 RVA: 0x001103AF File Offset: 0x0010F3AF
		// (set) Token: 0x06004B1E RID: 19230 RVA: 0x001103BB File Offset: 0x0010F3BB
		[SRDescription("OFDshowReadOnlyDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		public bool ShowReadOnly
		{
			get
			{
				return !base.GetOption(4);
			}
			set
			{
				base.SetOption(4, !value);
			}
		}

		// Token: 0x06004B1F RID: 19231 RVA: 0x001103C8 File Offset: 0x0010F3C8
		public Stream OpenFile()
		{
			IntSecurity.FileDialogOpenFile.Demand();
			string text = base.FileNamesInternal[0];
			if (text == null || text.Length == 0)
			{
				throw new ArgumentNullException("FileName");
			}
			Stream stream = null;
			new FileIOPermission(FileIOPermissionAccess.Read, IntSecurity.UnsafeGetFullPath(text)).Assert();
			try
			{
				stream = new FileStream(text, FileMode.Open, FileAccess.Read, FileShare.Read);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return stream;
		}

		// Token: 0x06004B20 RID: 19232 RVA: 0x00110434 File Offset: 0x0010F434
		public override void Reset()
		{
			base.Reset();
			base.SetOption(4096, true);
		}

		// Token: 0x06004B21 RID: 19233 RVA: 0x00110448 File Offset: 0x0010F448
		internal override void EnsureFileDialogPermission()
		{
			IntSecurity.FileDialogOpenFile.Demand();
		}

		// Token: 0x06004B22 RID: 19234 RVA: 0x00110454 File Offset: 0x0010F454
		internal override bool RunFileDialog(NativeMethods.OPENFILENAME_I ofn)
		{
			IntSecurity.FileDialogOpenFile.Demand();
			bool openFileName = UnsafeNativeMethods.GetOpenFileName(ofn);
			if (!openFileName)
			{
				switch (SafeNativeMethods.CommDlgExtendedError())
				{
				case 12289:
					throw new InvalidOperationException(SR.GetString("FileDialogSubLassFailure"));
				case 12290:
					throw new InvalidOperationException(SR.GetString("FileDialogInvalidFileName", new object[] { base.FileName }));
				case 12291:
					throw new InvalidOperationException(SR.GetString("FileDialogBufferTooSmall"));
				}
			}
			return openFileName;
		}

		// Token: 0x06004B23 RID: 19235 RVA: 0x001104DC File Offset: 0x0010F4DC
		internal override string[] ProcessVistaFiles(FileDialogNative.IFileDialog dialog)
		{
			FileDialogNative.IFileOpenDialog fileOpenDialog = (FileDialogNative.IFileOpenDialog)dialog;
			if (this.Multiselect)
			{
				FileDialogNative.IShellItemArray shellItemArray;
				fileOpenDialog.GetResults(out shellItemArray);
				uint num;
				shellItemArray.GetCount(out num);
				string[] array = new string[num];
				for (uint num2 = 0U; num2 < num; num2 += 1U)
				{
					FileDialogNative.IShellItem shellItem;
					shellItemArray.GetItemAt(num2, out shellItem);
					array[(int)num2] = FileDialog.GetFilePathFromShellItem(shellItem);
				}
				return array;
			}
			FileDialogNative.IShellItem shellItem2;
			fileOpenDialog.GetResult(out shellItem2);
			return new string[] { FileDialog.GetFilePathFromShellItem(shellItem2) };
		}

		// Token: 0x06004B24 RID: 19236 RVA: 0x00110554 File Offset: 0x0010F554
		internal override FileDialogNative.IFileDialog CreateVistaDialog()
		{
			return (FileDialogNative.NativeFileOpenDialog)new FileDialogNative.FileOpenDialogRCW();
		}

		// Token: 0x17000ED3 RID: 3795
		// (get) Token: 0x06004B25 RID: 19237 RVA: 0x00110560 File Offset: 0x0010F560
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string SafeFileName
		{
			get
			{
				new FileIOPermission(PermissionState.Unrestricted).Assert();
				string fileName = base.FileName;
				CodeAccessPermission.RevertAssert();
				if (string.IsNullOrEmpty(fileName))
				{
					return "";
				}
				return OpenFileDialog.RemoveSensitivePathInformation(fileName);
			}
		}

		// Token: 0x06004B26 RID: 19238 RVA: 0x0011059A File Offset: 0x0010F59A
		private static string RemoveSensitivePathInformation(string fullPath)
		{
			return Path.GetFileName(fullPath);
		}

		// Token: 0x17000ED4 RID: 3796
		// (get) Token: 0x06004B27 RID: 19239 RVA: 0x001105A4 File Offset: 0x0010F5A4
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string[] SafeFileNames
		{
			get
			{
				new FileIOPermission(PermissionState.Unrestricted).Assert();
				string[] fileNames = base.FileNames;
				CodeAccessPermission.RevertAssert();
				if (fileNames == null || fileNames.Length == 0)
				{
					return new string[0];
				}
				string[] array = new string[fileNames.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = OpenFileDialog.RemoveSensitivePathInformation(fileNames[i]);
				}
				return array;
			}
		}

		// Token: 0x17000ED5 RID: 3797
		// (get) Token: 0x06004B28 RID: 19240 RVA: 0x001105FA File Offset: 0x0010F5FA
		internal override bool SettingsSupportVistaDialog
		{
			get
			{
				return base.SettingsSupportVistaDialog && !this.ShowReadOnly;
			}
		}
	}
}
