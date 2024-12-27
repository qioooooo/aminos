using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x020003FA RID: 1018
	public class FileDialogCustomPlacesCollection : Collection<FileDialogCustomPlace>
	{
		// Token: 0x06003C3D RID: 15421 RVA: 0x000D92D0 File Offset: 0x000D82D0
		internal void Apply(FileDialogNative.IFileDialog dialog)
		{
			new FileIOPermission(PermissionState.Unrestricted).Assert();
			for (int i = base.Items.Count - 1; i >= 0; i--)
			{
				FileDialogCustomPlace fileDialogCustomPlace = base.Items[i];
				try
				{
					FileDialogNative.IShellItem nativePath = fileDialogCustomPlace.GetNativePath();
					if (nativePath != null)
					{
						dialog.AddPlace(nativePath, 0);
					}
				}
				catch (FileNotFoundException)
				{
				}
			}
		}

		// Token: 0x06003C3E RID: 15422 RVA: 0x000D9334 File Offset: 0x000D8334
		public void Add(string path)
		{
			base.Add(new FileDialogCustomPlace(path));
		}

		// Token: 0x06003C3F RID: 15423 RVA: 0x000D9342 File Offset: 0x000D8342
		public void Add(Guid knownFolderGuid)
		{
			base.Add(new FileDialogCustomPlace(knownFolderGuid));
		}
	}
}
