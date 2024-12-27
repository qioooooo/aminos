using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Windows.Forms
{
	// Token: 0x02000405 RID: 1029
	[Designer("System.Windows.Forms.Design.FolderBrowserDialogDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultEvent("HelpRequest")]
	[SRDescription("DescriptionFolderBrowserDialog")]
	[DefaultProperty("SelectedPath")]
	public sealed class FolderBrowserDialog : CommonDialog
	{
		// Token: 0x06003C81 RID: 15489 RVA: 0x000D9A87 File Offset: 0x000D8A87
		public FolderBrowserDialog()
		{
			this.Reset();
		}

		// Token: 0x140001F6 RID: 502
		// (add) Token: 0x06003C82 RID: 15490 RVA: 0x000D9A95 File Offset: 0x000D8A95
		// (remove) Token: 0x06003C83 RID: 15491 RVA: 0x000D9A9E File Offset: 0x000D8A9E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler HelpRequest
		{
			add
			{
				base.HelpRequest += value;
			}
			remove
			{
				base.HelpRequest -= value;
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x06003C84 RID: 15492 RVA: 0x000D9AA7 File Offset: 0x000D8AA7
		// (set) Token: 0x06003C85 RID: 15493 RVA: 0x000D9AAF File Offset: 0x000D8AAF
		[Localizable(false)]
		[SRDescription("FolderBrowserDialogShowNewFolderButton")]
		[SRCategory("CatFolderBrowsing")]
		[Browsable(true)]
		[DefaultValue(true)]
		public bool ShowNewFolderButton
		{
			get
			{
				return this.showNewFolderButton;
			}
			set
			{
				this.showNewFolderButton = value;
			}
		}

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x06003C86 RID: 15494 RVA: 0x000D9AB8 File Offset: 0x000D8AB8
		// (set) Token: 0x06003C87 RID: 15495 RVA: 0x000D9AF5 File Offset: 0x000D8AF5
		[Editor("System.Windows.Forms.Design.SelectedPathEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("FolderBrowserDialogSelectedPath")]
		[SRCategory("CatFolderBrowsing")]
		[DefaultValue("")]
		[Browsable(true)]
		[Localizable(true)]
		public string SelectedPath
		{
			get
			{
				if (this.selectedPath == null || this.selectedPath.Length == 0)
				{
					return this.selectedPath;
				}
				if (this.selectedPathNeedsCheck)
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, this.selectedPath).Demand();
				}
				return this.selectedPath;
			}
			set
			{
				this.selectedPath = ((value == null) ? string.Empty : value);
				this.selectedPathNeedsCheck = false;
			}
		}

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x06003C88 RID: 15496 RVA: 0x000D9B0F File Offset: 0x000D8B0F
		// (set) Token: 0x06003C89 RID: 15497 RVA: 0x000D9B17 File Offset: 0x000D8B17
		[Browsable(true)]
		[SRCategory("CatFolderBrowsing")]
		[SRDescription("FolderBrowserDialogRootFolder")]
		[TypeConverter(typeof(SpecialFolderEnumConverter))]
		[DefaultValue(Environment.SpecialFolder.Desktop)]
		[Localizable(false)]
		public Environment.SpecialFolder RootFolder
		{
			get
			{
				return this.rootFolder;
			}
			set
			{
				if (!Enum.IsDefined(typeof(Environment.SpecialFolder), value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(Environment.SpecialFolder));
				}
				this.rootFolder = value;
			}
		}

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x06003C8A RID: 15498 RVA: 0x000D9B4D File Offset: 0x000D8B4D
		// (set) Token: 0x06003C8B RID: 15499 RVA: 0x000D9B55 File Offset: 0x000D8B55
		[SRCategory("CatFolderBrowsing")]
		[SRDescription("FolderBrowserDialogDescription")]
		[Browsable(true)]
		[DefaultValue("")]
		[Localizable(true)]
		public string Description
		{
			get
			{
				return this.descriptionText;
			}
			set
			{
				this.descriptionText = ((value == null) ? string.Empty : value);
			}
		}

		// Token: 0x06003C8C RID: 15500 RVA: 0x000D9B68 File Offset: 0x000D8B68
		private static UnsafeNativeMethods.IMalloc GetSHMalloc()
		{
			UnsafeNativeMethods.IMalloc[] array = new UnsafeNativeMethods.IMalloc[1];
			UnsafeNativeMethods.Shell32.SHGetMalloc(array);
			return array[0];
		}

		// Token: 0x06003C8D RID: 15501 RVA: 0x000D9B86 File Offset: 0x000D8B86
		public override void Reset()
		{
			this.rootFolder = Environment.SpecialFolder.Desktop;
			this.descriptionText = string.Empty;
			this.selectedPath = string.Empty;
			this.selectedPathNeedsCheck = false;
			this.showNewFolderButton = true;
		}

		// Token: 0x06003C8E RID: 15502 RVA: 0x000D9BB4 File Offset: 0x000D8BB4
		protected override bool RunDialog(IntPtr hWndOwner)
		{
			IntPtr zero = IntPtr.Zero;
			bool flag = false;
			UnsafeNativeMethods.Shell32.SHGetSpecialFolderLocation(hWndOwner, (int)this.rootFolder, ref zero);
			if (zero == IntPtr.Zero)
			{
				UnsafeNativeMethods.Shell32.SHGetSpecialFolderLocation(hWndOwner, 0, ref zero);
				if (zero == IntPtr.Zero)
				{
					throw new InvalidOperationException(SR.GetString("FolderBrowserDialogNoRootFolder"));
				}
			}
			int num = 64;
			if (!this.showNewFolderButton)
			{
				num += 512;
			}
			if (Control.CheckForIllegalCrossThreadCalls && Application.OleRequired() != ApartmentState.STA)
			{
				throw new ThreadStateException(SR.GetString("DebuggingExceptionOnly", new object[] { SR.GetString("ThreadMustBeSTA") }));
			}
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			IntPtr intPtr3 = IntPtr.Zero;
			try
			{
				UnsafeNativeMethods.BROWSEINFO browseinfo = new UnsafeNativeMethods.BROWSEINFO();
				intPtr2 = Marshal.AllocHGlobal(260 * Marshal.SystemDefaultCharSize);
				intPtr3 = Marshal.AllocHGlobal(260 * Marshal.SystemDefaultCharSize);
				this.callback = new UnsafeNativeMethods.BrowseCallbackProc(this.FolderBrowserDialog_BrowseCallbackProc);
				browseinfo.pidlRoot = zero;
				browseinfo.hwndOwner = hWndOwner;
				browseinfo.pszDisplayName = intPtr2;
				browseinfo.lpszTitle = this.descriptionText;
				browseinfo.ulFlags = num;
				browseinfo.lpfn = this.callback;
				browseinfo.lParam = IntPtr.Zero;
				browseinfo.iImage = 0;
				intPtr = UnsafeNativeMethods.Shell32.SHBrowseForFolder(browseinfo);
				if (intPtr != IntPtr.Zero)
				{
					UnsafeNativeMethods.Shell32.SHGetPathFromIDList(intPtr, intPtr3);
					this.selectedPathNeedsCheck = true;
					this.selectedPath = Marshal.PtrToStringAuto(intPtr3);
					flag = true;
				}
			}
			finally
			{
				UnsafeNativeMethods.IMalloc shmalloc = FolderBrowserDialog.GetSHMalloc();
				shmalloc.Free(zero);
				if (intPtr != IntPtr.Zero)
				{
					shmalloc.Free(intPtr);
				}
				if (intPtr3 != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr3);
				}
				if (intPtr2 != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr2);
				}
				this.callback = null;
			}
			return flag;
		}

		// Token: 0x06003C8F RID: 15503 RVA: 0x000D9D94 File Offset: 0x000D8D94
		private int FolderBrowserDialog_BrowseCallbackProc(IntPtr hwnd, int msg, IntPtr lParam, IntPtr lpData)
		{
			switch (msg)
			{
			case 1:
				if (this.selectedPath.Length != 0)
				{
					UnsafeNativeMethods.SendMessage(new HandleRef(null, hwnd), NativeMethods.BFFM_SETSELECTION, 1, this.selectedPath);
				}
				break;
			case 2:
				if (lParam != IntPtr.Zero)
				{
					IntPtr intPtr = Marshal.AllocHGlobal(260 * Marshal.SystemDefaultCharSize);
					bool flag = UnsafeNativeMethods.Shell32.SHGetPathFromIDList(lParam, intPtr);
					Marshal.FreeHGlobal(intPtr);
					UnsafeNativeMethods.SendMessage(new HandleRef(null, hwnd), 1125, 0, flag ? 1 : 0);
				}
				break;
			}
			return 0;
		}

		// Token: 0x04001E1E RID: 7710
		private Environment.SpecialFolder rootFolder;

		// Token: 0x04001E1F RID: 7711
		private string descriptionText;

		// Token: 0x04001E20 RID: 7712
		private string selectedPath;

		// Token: 0x04001E21 RID: 7713
		private bool showNewFolderButton;

		// Token: 0x04001E22 RID: 7714
		private bool selectedPathNeedsCheck;

		// Token: 0x04001E23 RID: 7715
		private UnsafeNativeMethods.BrowseCallbackProc callback;
	}
}
