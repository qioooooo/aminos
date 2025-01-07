using System;
using System.ComponentModel;
using System.Design;
using System.Drawing.Design;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Design
{
	public class FolderNameEditor : UITypeEditor
	{
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (this.folderBrowser == null)
			{
				this.folderBrowser = new FolderNameEditor.FolderBrowser();
				this.InitializeDialog(this.folderBrowser);
			}
			if (this.folderBrowser.ShowDialog() != DialogResult.OK)
			{
				return value;
			}
			return this.folderBrowser.DirectoryPath;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		protected virtual void InitializeDialog(FolderNameEditor.FolderBrowser folderBrowser)
		{
		}

		private FolderNameEditor.FolderBrowser folderBrowser;

		protected sealed class FolderBrowser : Component
		{
			public FolderNameEditor.FolderBrowserStyles Style
			{
				get
				{
					return this.publicOptions;
				}
				set
				{
					this.publicOptions = value;
				}
			}

			public string DirectoryPath
			{
				get
				{
					return this.directoryPath;
				}
			}

			public FolderNameEditor.FolderBrowserFolder StartLocation
			{
				get
				{
					return this.startLocation;
				}
				set
				{
					this.startLocation = value;
				}
			}

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

			private static UnsafeNativeMethods.IMalloc GetSHMalloc()
			{
				UnsafeNativeMethods.IMalloc[] array = new UnsafeNativeMethods.IMalloc[1];
				UnsafeNativeMethods.Shell32.SHGetMalloc(array);
				return array[0];
			}

			public DialogResult ShowDialog()
			{
				return this.ShowDialog(null);
			}

			public DialogResult ShowDialog(IWin32Window owner)
			{
				IntPtr zero = IntPtr.Zero;
				IntPtr intPtr;
				if (owner != null)
				{
					intPtr = owner.Handle;
				}
				else
				{
					intPtr = UnsafeNativeMethods.GetActiveWindow();
				}
				UnsafeNativeMethods.Shell32.SHGetSpecialFolderLocation(intPtr, (int)this.startLocation, ref zero);
				if (zero == IntPtr.Zero)
				{
					return DialogResult.Cancel;
				}
				int num = (int)(this.publicOptions | (FolderNameEditor.FolderBrowserStyles)this.privateOptions);
				if ((num & 64) != 0)
				{
					Application.OleRequired();
				}
				IntPtr intPtr2 = IntPtr.Zero;
				try
				{
					UnsafeNativeMethods.BROWSEINFO browseinfo = new UnsafeNativeMethods.BROWSEINFO();
					IntPtr intPtr3 = Marshal.AllocHGlobal(FolderNameEditor.FolderBrowser.MAX_PATH);
					browseinfo.pidlRoot = zero;
					browseinfo.hwndOwner = intPtr;
					browseinfo.pszDisplayName = intPtr3;
					browseinfo.lpszTitle = this.descriptionText;
					browseinfo.ulFlags = num;
					browseinfo.lpfn = IntPtr.Zero;
					browseinfo.lParam = IntPtr.Zero;
					browseinfo.iImage = 0;
					intPtr2 = UnsafeNativeMethods.Shell32.SHBrowseForFolder(browseinfo);
					if (intPtr2 == IntPtr.Zero)
					{
						return DialogResult.Cancel;
					}
					UnsafeNativeMethods.Shell32.SHGetPathFromIDList(intPtr2, intPtr3);
					this.directoryPath = Marshal.PtrToStringAuto(intPtr3);
					Marshal.FreeHGlobal(intPtr3);
				}
				finally
				{
					UnsafeNativeMethods.IMalloc shmalloc = FolderNameEditor.FolderBrowser.GetSHMalloc();
					shmalloc.Free(zero);
					if (intPtr2 != IntPtr.Zero)
					{
						shmalloc.Free(intPtr2);
					}
				}
				return DialogResult.OK;
			}

			private static readonly int MAX_PATH = 260;

			private FolderNameEditor.FolderBrowserFolder startLocation;

			private FolderNameEditor.FolderBrowserStyles publicOptions = FolderNameEditor.FolderBrowserStyles.RestrictToFilesystem;

			private UnsafeNativeMethods.BrowseInfos privateOptions = UnsafeNativeMethods.BrowseInfos.NewDialogStyle;

			private string descriptionText = string.Empty;

			private string directoryPath = string.Empty;
		}

		protected enum FolderBrowserFolder
		{
			Desktop,
			Favorites = 6,
			MyComputer = 17,
			MyDocuments = 5,
			MyPictures = 39,
			NetAndDialUpConnections = 49,
			NetworkNeighborhood = 18,
			Printers = 4,
			Recent = 8,
			SendTo,
			StartMenu = 11,
			Templates = 21
		}

		[Flags]
		protected enum FolderBrowserStyles
		{
			BrowseForComputer = 4096,
			BrowseForEverything = 16384,
			BrowseForPrinter = 8192,
			RestrictToDomain = 2,
			RestrictToFilesystem = 1,
			RestrictToSubfolders = 8,
			ShowTextBox = 16
		}
	}
}
