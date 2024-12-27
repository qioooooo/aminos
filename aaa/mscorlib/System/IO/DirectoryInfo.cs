using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.IO
{
	// Token: 0x02000596 RID: 1430
	[ComVisible(true)]
	[Serializable]
	public sealed class DirectoryInfo : FileSystemInfo
	{
		// Token: 0x06003525 RID: 13605 RVA: 0x000B1D30 File Offset: 0x000B0D30
		public DirectoryInfo(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 2 && path[1] == ':')
			{
				this.OriginalPath = ".";
			}
			else
			{
				this.OriginalPath = path;
			}
			string fullPathInternal = Path.GetFullPathInternal(path);
			this.demandDir = new string[] { Directory.GetDemandDir(fullPathInternal, true) };
			new FileIOPermission(FileIOPermissionAccess.Read, this.demandDir, false, false).Demand();
			this.FullPath = fullPathInternal;
		}

		// Token: 0x06003526 RID: 13606 RVA: 0x000B1DB4 File Offset: 0x000B0DB4
		internal DirectoryInfo(string fullPath, bool junk)
		{
			this.OriginalPath = Path.GetFileName(fullPath);
			this.FullPath = fullPath;
			this.demandDir = new string[] { Directory.GetDemandDir(fullPath, true) };
		}

		// Token: 0x06003527 RID: 13607 RVA: 0x000B1DF4 File Offset: 0x000B0DF4
		private DirectoryInfo(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.demandDir = new string[] { Directory.GetDemandDir(this.FullPath, true) };
			new FileIOPermission(FileIOPermissionAccess.Read, this.demandDir, false, false).Demand();
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x06003528 RID: 13608 RVA: 0x000B1E3C File Offset: 0x000B0E3C
		public override string Name
		{
			get
			{
				string text = this.FullPath;
				if (text.Length > 3)
				{
					if (text.EndsWith(Path.DirectorySeparatorChar))
					{
						text = this.FullPath.Substring(0, this.FullPath.Length - 1);
					}
					return Path.GetFileName(text);
				}
				return this.FullPath;
			}
		}

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06003529 RID: 13609 RVA: 0x000B1E90 File Offset: 0x000B0E90
		public DirectoryInfo Parent
		{
			get
			{
				string text = this.FullPath;
				if (text.Length > 3 && text.EndsWith(Path.DirectorySeparatorChar))
				{
					text = this.FullPath.Substring(0, this.FullPath.Length - 1);
				}
				string directoryName = Path.GetDirectoryName(text);
				if (directoryName == null)
				{
					return null;
				}
				DirectoryInfo directoryInfo = new DirectoryInfo(directoryName, false);
				new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, directoryInfo.demandDir, false, false).Demand();
				return directoryInfo;
			}
		}

		// Token: 0x0600352A RID: 13610 RVA: 0x000B1EFD File Offset: 0x000B0EFD
		public DirectoryInfo CreateSubdirectory(string path)
		{
			return this.CreateSubdirectory(path, null);
		}

		// Token: 0x0600352B RID: 13611 RVA: 0x000B1F08 File Offset: 0x000B0F08
		public DirectoryInfo CreateSubdirectory(string path, DirectorySecurity directorySecurity)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			string text = Path.InternalCombine(this.FullPath, path);
			string fullPathInternal = Path.GetFullPathInternal(text);
			if (string.Compare(this.FullPath, 0, fullPathInternal, 0, this.FullPath.Length, StringComparison.OrdinalIgnoreCase) != 0)
			{
				string displayablePath = __Error.GetDisplayablePath(this.OriginalPath, false);
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidSubPath"), new object[] { path, displayablePath }));
			}
			string text2 = Directory.GetDemandDir(fullPathInternal, true);
			new FileIOPermission(FileIOPermissionAccess.Write, new string[] { text2 }, false, false).Demand();
			Directory.InternalCreateDirectory(fullPathInternal, path, directorySecurity);
			return new DirectoryInfo(fullPathInternal);
		}

		// Token: 0x0600352C RID: 13612 RVA: 0x000B1FC0 File Offset: 0x000B0FC0
		public void Create()
		{
			Directory.InternalCreateDirectory(this.FullPath, this.OriginalPath, null);
		}

		// Token: 0x0600352D RID: 13613 RVA: 0x000B1FD4 File Offset: 0x000B0FD4
		public void Create(DirectorySecurity directorySecurity)
		{
			Directory.InternalCreateDirectory(this.FullPath, this.OriginalPath, directorySecurity);
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x0600352E RID: 13614 RVA: 0x000B1FE8 File Offset: 0x000B0FE8
		public override bool Exists
		{
			get
			{
				bool flag;
				try
				{
					if (this._dataInitialised == -1)
					{
						base.Refresh();
					}
					if (this._dataInitialised != 0)
					{
						flag = false;
					}
					else
					{
						flag = this._data.fileAttributes != -1 && (this._data.fileAttributes & 16) != 0;
					}
				}
				catch
				{
					flag = false;
				}
				return flag;
			}
		}

		// Token: 0x0600352F RID: 13615 RVA: 0x000B2050 File Offset: 0x000B1050
		public DirectorySecurity GetAccessControl()
		{
			return Directory.GetAccessControl(this.FullPath, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
		}

		// Token: 0x06003530 RID: 13616 RVA: 0x000B205F File Offset: 0x000B105F
		public DirectorySecurity GetAccessControl(AccessControlSections includeSections)
		{
			return Directory.GetAccessControl(this.FullPath, includeSections);
		}

		// Token: 0x06003531 RID: 13617 RVA: 0x000B206D File Offset: 0x000B106D
		public void SetAccessControl(DirectorySecurity directorySecurity)
		{
			Directory.SetAccessControl(this.FullPath, directorySecurity);
		}

		// Token: 0x06003532 RID: 13618 RVA: 0x000B207B File Offset: 0x000B107B
		public FileInfo[] GetFiles(string searchPattern)
		{
			return this.GetFiles(searchPattern, SearchOption.TopDirectoryOnly);
		}

		// Token: 0x06003533 RID: 13619 RVA: 0x000B2088 File Offset: 0x000B1088
		private string FixupFileDirFullPath(string fileDirUserPath)
		{
			string text;
			if (this.OriginalPath.Length == 0)
			{
				text = Path.InternalCombine(this.FullPath, fileDirUserPath);
			}
			else if (this.OriginalPath.EndsWith(Path.DirectorySeparatorChar) || this.OriginalPath.EndsWith(Path.AltDirectorySeparatorChar))
			{
				text = Path.InternalCombine(this.FullPath, fileDirUserPath.Substring(this.OriginalPath.Length));
			}
			else
			{
				text = Path.InternalCombine(this.FullPath, fileDirUserPath.Substring(this.OriginalPath.Length + 1));
			}
			return text;
		}

		// Token: 0x06003534 RID: 13620 RVA: 0x000B2114 File Offset: 0x000B1114
		public FileInfo[] GetFiles(string searchPattern, SearchOption searchOption)
		{
			if (searchPattern == null)
			{
				throw new ArgumentNullException("searchPattern");
			}
			string[] array = Directory.InternalGetFileDirectoryNames(this.FullPath, this.OriginalPath, searchPattern, true, false, searchOption);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.FixupFileDirFullPath(array[i]);
			}
			if (array.Length != 0)
			{
				new FileIOPermission(FileIOPermissionAccess.Read, array, false, false).Demand();
			}
			FileInfo[] array2 = new FileInfo[array.Length];
			for (int j = 0; j < array.Length; j++)
			{
				array2[j] = new FileInfo(array[j], false);
			}
			return array2;
		}

		// Token: 0x06003535 RID: 13621 RVA: 0x000B2196 File Offset: 0x000B1196
		public FileInfo[] GetFiles()
		{
			return this.GetFiles("*");
		}

		// Token: 0x06003536 RID: 13622 RVA: 0x000B21A3 File Offset: 0x000B11A3
		public DirectoryInfo[] GetDirectories()
		{
			return this.GetDirectories("*");
		}

		// Token: 0x06003537 RID: 13623 RVA: 0x000B21B0 File Offset: 0x000B11B0
		public FileSystemInfo[] GetFileSystemInfos(string searchPattern)
		{
			return this.GetFileSystemInfos(searchPattern, SearchOption.TopDirectoryOnly);
		}

		// Token: 0x06003538 RID: 13624 RVA: 0x000B21BC File Offset: 0x000B11BC
		private FileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption)
		{
			if (searchPattern == null)
			{
				throw new ArgumentNullException("searchPattern");
			}
			string[] array = Directory.InternalGetFileDirectoryNames(this.FullPath, this.OriginalPath, searchPattern, false, true, searchOption);
			string[] array2 = Directory.InternalGetFileDirectoryNames(this.FullPath, this.OriginalPath, searchPattern, true, false, searchOption);
			FileSystemInfo[] array3 = new FileSystemInfo[array.Length + array2.Length];
			string[] array4 = new string[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.FixupFileDirFullPath(array[i]);
				array4[i] = array[i] + "\\.";
			}
			if (array.Length != 0)
			{
				new FileIOPermission(FileIOPermissionAccess.Read, array4, false, false).Demand();
			}
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = this.FixupFileDirFullPath(array2[j]);
			}
			if (array2.Length != 0)
			{
				new FileIOPermission(FileIOPermissionAccess.Read, array2, false, false).Demand();
			}
			int num = 0;
			for (int k = 0; k < array.Length; k++)
			{
				array3[num++] = new DirectoryInfo(array[k], false);
			}
			for (int l = 0; l < array2.Length; l++)
			{
				array3[num++] = new FileInfo(array2[l], false);
			}
			return array3;
		}

		// Token: 0x06003539 RID: 13625 RVA: 0x000B22DF File Offset: 0x000B12DF
		public FileSystemInfo[] GetFileSystemInfos()
		{
			return this.GetFileSystemInfos("*");
		}

		// Token: 0x0600353A RID: 13626 RVA: 0x000B22EC File Offset: 0x000B12EC
		public DirectoryInfo[] GetDirectories(string searchPattern)
		{
			return this.GetDirectories(searchPattern, SearchOption.TopDirectoryOnly);
		}

		// Token: 0x0600353B RID: 13627 RVA: 0x000B22F8 File Offset: 0x000B12F8
		public DirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption)
		{
			if (searchPattern == null)
			{
				throw new ArgumentNullException("searchPattern");
			}
			string[] array = Directory.InternalGetFileDirectoryNames(this.FullPath, this.OriginalPath, searchPattern, false, true, searchOption);
			string[] array2 = new string[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.FixupFileDirFullPath(array[i]);
				array2[i] = array[i] + "\\.";
			}
			if (array.Length != 0)
			{
				new FileIOPermission(FileIOPermissionAccess.Read, array2, false, false).Demand();
			}
			DirectoryInfo[] array3 = new DirectoryInfo[array.Length];
			for (int j = 0; j < array.Length; j++)
			{
				array3[j] = new DirectoryInfo(array[j], false);
			}
			return array3;
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x0600353C RID: 13628 RVA: 0x000B239C File Offset: 0x000B139C
		public DirectoryInfo Root
		{
			get
			{
				int rootLength = Path.GetRootLength(this.FullPath);
				string text = this.FullPath.Substring(0, rootLength);
				string text2 = Directory.GetDemandDir(text, true);
				new FileIOPermission(FileIOPermissionAccess.PathDiscovery, new string[] { text2 }, false, false).Demand();
				return new DirectoryInfo(text);
			}
		}

		// Token: 0x0600353D RID: 13629 RVA: 0x000B23EC File Offset: 0x000B13EC
		public void MoveTo(string destDirName)
		{
			if (destDirName == null)
			{
				throw new ArgumentNullException("destDirName");
			}
			if (destDirName.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), "destDirName");
			}
			new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.Write, this.demandDir, false, false).Demand();
			string text = Path.GetFullPathInternal(destDirName);
			if (!text.EndsWith(Path.DirectorySeparatorChar))
			{
				text += Path.DirectorySeparatorChar;
			}
			string text2 = text + '.';
			new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.Write, text2).Demand();
			string text3;
			if (this.FullPath.EndsWith(Path.DirectorySeparatorChar))
			{
				text3 = this.FullPath;
			}
			else
			{
				text3 = this.FullPath + Path.DirectorySeparatorChar;
			}
			if (CultureInfo.InvariantCulture.CompareInfo.Compare(text3, text, CompareOptions.IgnoreCase) == 0)
			{
				throw new IOException(Environment.GetResourceString("IO.IO_SourceDestMustBeDifferent"));
			}
			string pathRoot = Path.GetPathRoot(text3);
			string pathRoot2 = Path.GetPathRoot(text);
			if (CultureInfo.InvariantCulture.CompareInfo.Compare(pathRoot, pathRoot2, CompareOptions.IgnoreCase) != 0)
			{
				throw new IOException(Environment.GetResourceString("IO.IO_SourceDestMustHaveSameRoot"));
			}
			if (Environment.IsWin9X() && !Directory.InternalExists(this.FullPath))
			{
				throw new DirectoryNotFoundException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.PathNotFound_Path"), new object[] { destDirName }));
			}
			if (!Win32Native.MoveFile(this.FullPath, destDirName))
			{
				int num = Marshal.GetLastWin32Error();
				if (num == 2)
				{
					num = 3;
					__Error.WinIOError(num, this.OriginalPath);
				}
				if (num == 5)
				{
					throw new IOException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("UnauthorizedAccess_IODenied_Path"), new object[] { this.OriginalPath }));
				}
				__Error.WinIOError(num, string.Empty);
			}
			this.FullPath = text;
			this.OriginalPath = destDirName;
			this.demandDir = new string[] { Directory.GetDemandDir(this.FullPath, true) };
			this._dataInitialised = -1;
		}

		// Token: 0x0600353E RID: 13630 RVA: 0x000B25DA File Offset: 0x000B15DA
		public override void Delete()
		{
			Directory.Delete(this.FullPath, this.OriginalPath, false);
		}

		// Token: 0x0600353F RID: 13631 RVA: 0x000B25EE File Offset: 0x000B15EE
		public void Delete(bool recursive)
		{
			Directory.Delete(this.FullPath, this.OriginalPath, recursive);
		}

		// Token: 0x06003540 RID: 13632 RVA: 0x000B2602 File Offset: 0x000B1602
		public override string ToString()
		{
			return this.OriginalPath;
		}

		// Token: 0x04001BCF RID: 7119
		private string[] demandDir;
	}
}
