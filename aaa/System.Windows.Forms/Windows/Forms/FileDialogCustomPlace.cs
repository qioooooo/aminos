using System;
using System.Globalization;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x020003F9 RID: 1017
	public class FileDialogCustomPlace
	{
		// Token: 0x06003C34 RID: 15412 RVA: 0x000D9161 File Offset: 0x000D8161
		public FileDialogCustomPlace(string path)
		{
			this.Path = path;
		}

		// Token: 0x06003C35 RID: 15413 RVA: 0x000D9186 File Offset: 0x000D8186
		public FileDialogCustomPlace(Guid knownFolderGuid)
		{
			this.KnownFolderGuid = knownFolderGuid;
		}

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x06003C36 RID: 15414 RVA: 0x000D91AB File Offset: 0x000D81AB
		// (set) Token: 0x06003C37 RID: 15415 RVA: 0x000D91C6 File Offset: 0x000D81C6
		public string Path
		{
			get
			{
				if (string.IsNullOrEmpty(this._path))
				{
					return string.Empty;
				}
				return this._path;
			}
			set
			{
				this._path = value ?? "";
				this._knownFolderGuid = Guid.Empty;
			}
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x06003C38 RID: 15416 RVA: 0x000D91E3 File Offset: 0x000D81E3
		// (set) Token: 0x06003C39 RID: 15417 RVA: 0x000D91EB File Offset: 0x000D81EB
		public Guid KnownFolderGuid
		{
			get
			{
				return this._knownFolderGuid;
			}
			set
			{
				this._path = string.Empty;
				this._knownFolderGuid = value;
			}
		}

		// Token: 0x06003C3A RID: 15418 RVA: 0x000D9200 File Offset: 0x000D8200
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{0} Path: {1} KnownFolderGuid: {2}", new object[]
			{
				base.ToString(),
				this.Path,
				this.KnownFolderGuid
			});
		}

		// Token: 0x06003C3B RID: 15419 RVA: 0x000D9244 File Offset: 0x000D8244
		internal FileDialogNative.IShellItem GetNativePath()
		{
			string text;
			if (!string.IsNullOrEmpty(this._path))
			{
				text = this._path;
			}
			else
			{
				text = FileDialogCustomPlace.GetFolderLocation(this._knownFolderGuid);
			}
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			return FileDialog.GetShellItemForPath(text);
		}

		// Token: 0x06003C3C RID: 15420 RVA: 0x000D928C File Offset: 0x000D828C
		private static string GetFolderLocation(Guid folderGuid)
		{
			if (!UnsafeNativeMethods.IsVista)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder(260);
			if (UnsafeNativeMethods.Shell32.SHGetFolderPathEx(ref folderGuid, 0U, IntPtr.Zero, stringBuilder, (uint)stringBuilder.Capacity) == 0)
			{
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x04001E05 RID: 7685
		private string _path = "";

		// Token: 0x04001E06 RID: 7686
		private Guid _knownFolderGuid = Guid.Empty;
	}
}
