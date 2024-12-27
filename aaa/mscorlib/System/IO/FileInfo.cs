using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;

namespace System.IO
{
	// Token: 0x020005A0 RID: 1440
	[ComVisible(true)]
	[Serializable]
	public sealed class FileInfo : FileSystemInfo
	{
		// Token: 0x06003598 RID: 13720 RVA: 0x000B3AA8 File Offset: 0x000B2AA8
		public FileInfo(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			this.OriginalPath = fileName;
			string fullPathInternal = Path.GetFullPathInternal(fileName);
			new FileIOPermission(FileIOPermissionAccess.Read, new string[] { fullPathInternal }, false, false).Demand();
			this._name = Path.GetFileName(fileName);
			this.FullPath = fullPathInternal;
		}

		// Token: 0x06003599 RID: 13721 RVA: 0x000B3B04 File Offset: 0x000B2B04
		private FileInfo(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			new FileIOPermission(FileIOPermissionAccess.Read, new string[] { this.FullPath }, false, false).Demand();
			this._name = Path.GetFileName(this.OriginalPath);
		}

		// Token: 0x0600359A RID: 13722 RVA: 0x000B3B48 File Offset: 0x000B2B48
		internal FileInfo(string fullPath, bool ignoreThis)
		{
			this._name = Path.GetFileName(fullPath);
			this.OriginalPath = this._name;
			this.FullPath = fullPath;
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x0600359B RID: 13723 RVA: 0x000B3B6F File Offset: 0x000B2B6F
		public override string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x0600359C RID: 13724 RVA: 0x000B3B78 File Offset: 0x000B2B78
		public long Length
		{
			get
			{
				if (this._dataInitialised == -1)
				{
					base.Refresh();
				}
				if (this._dataInitialised != 0)
				{
					__Error.WinIOError(this._dataInitialised, this.OriginalPath);
				}
				if ((this._data.fileAttributes & 16) != 0)
				{
					__Error.WinIOError(2, this.OriginalPath);
				}
				return ((long)this._data.fileSizeHigh << 32) | ((long)this._data.fileSizeLow & (long)((ulong)(-1)));
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x0600359D RID: 13725 RVA: 0x000B3BE8 File Offset: 0x000B2BE8
		public string DirectoryName
		{
			get
			{
				string directoryName = Path.GetDirectoryName(this.FullPath);
				if (directoryName != null)
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, new string[] { directoryName }, false, false).Demand();
				}
				return directoryName;
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x0600359E RID: 13726 RVA: 0x000B3C20 File Offset: 0x000B2C20
		public DirectoryInfo Directory
		{
			get
			{
				string directoryName = this.DirectoryName;
				if (directoryName == null)
				{
					return null;
				}
				return new DirectoryInfo(directoryName);
			}
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x0600359F RID: 13727 RVA: 0x000B3C3F File Offset: 0x000B2C3F
		// (set) Token: 0x060035A0 RID: 13728 RVA: 0x000B3C4F File Offset: 0x000B2C4F
		public bool IsReadOnly
		{
			get
			{
				return (base.Attributes & FileAttributes.ReadOnly) != (FileAttributes)0;
			}
			set
			{
				if (value)
				{
					base.Attributes |= FileAttributes.ReadOnly;
					return;
				}
				base.Attributes &= ~FileAttributes.ReadOnly;
			}
		}

		// Token: 0x060035A1 RID: 13729 RVA: 0x000B3C72 File Offset: 0x000B2C72
		public FileSecurity GetAccessControl()
		{
			return File.GetAccessControl(this.FullPath, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
		}

		// Token: 0x060035A2 RID: 13730 RVA: 0x000B3C81 File Offset: 0x000B2C81
		public FileSecurity GetAccessControl(AccessControlSections includeSections)
		{
			return File.GetAccessControl(this.FullPath, includeSections);
		}

		// Token: 0x060035A3 RID: 13731 RVA: 0x000B3C8F File Offset: 0x000B2C8F
		public void SetAccessControl(FileSecurity fileSecurity)
		{
			File.SetAccessControl(this.FullPath, fileSecurity);
		}

		// Token: 0x060035A4 RID: 13732 RVA: 0x000B3C9D File Offset: 0x000B2C9D
		public StreamReader OpenText()
		{
			return new StreamReader(this.FullPath, Encoding.UTF8, true, 1024);
		}

		// Token: 0x060035A5 RID: 13733 RVA: 0x000B3CB5 File Offset: 0x000B2CB5
		public StreamWriter CreateText()
		{
			return new StreamWriter(this.FullPath, false);
		}

		// Token: 0x060035A6 RID: 13734 RVA: 0x000B3CC3 File Offset: 0x000B2CC3
		public StreamWriter AppendText()
		{
			return new StreamWriter(this.FullPath, true);
		}

		// Token: 0x060035A7 RID: 13735 RVA: 0x000B3CD1 File Offset: 0x000B2CD1
		public FileInfo CopyTo(string destFileName)
		{
			return this.CopyTo(destFileName, false);
		}

		// Token: 0x060035A8 RID: 13736 RVA: 0x000B3CDB File Offset: 0x000B2CDB
		public FileInfo CopyTo(string destFileName, bool overwrite)
		{
			destFileName = File.InternalCopy(this.FullPath, destFileName, overwrite);
			return new FileInfo(destFileName, false);
		}

		// Token: 0x060035A9 RID: 13737 RVA: 0x000B3CF3 File Offset: 0x000B2CF3
		public FileStream Create()
		{
			return File.Create(this.FullPath);
		}

		// Token: 0x060035AA RID: 13738 RVA: 0x000B3D00 File Offset: 0x000B2D00
		public override void Delete()
		{
			new FileIOPermission(FileIOPermissionAccess.Write, new string[] { this.FullPath }, false, false).Demand();
			if (Environment.IsWin9X() && global::System.IO.Directory.InternalExists(this.FullPath))
			{
				throw new UnauthorizedAccessException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("UnauthorizedAccess_IODenied_Path"), new object[] { this.OriginalPath }));
			}
			if (!Win32Native.DeleteFile(this.FullPath))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error == 2)
				{
					return;
				}
				__Error.WinIOError(lastWin32Error, this.OriginalPath);
			}
		}

		// Token: 0x060035AB RID: 13739 RVA: 0x000B3D90 File Offset: 0x000B2D90
		[ComVisible(false)]
		public void Decrypt()
		{
			File.Decrypt(this.FullPath);
		}

		// Token: 0x060035AC RID: 13740 RVA: 0x000B3D9D File Offset: 0x000B2D9D
		[ComVisible(false)]
		public void Encrypt()
		{
			File.Encrypt(this.FullPath);
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x060035AD RID: 13741 RVA: 0x000B3DAC File Offset: 0x000B2DAC
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
						flag = (this._data.fileAttributes & 16) == 0;
					}
				}
				catch
				{
					flag = false;
				}
				return flag;
			}
		}

		// Token: 0x060035AE RID: 13742 RVA: 0x000B3E00 File Offset: 0x000B2E00
		public FileStream Open(FileMode mode)
		{
			return this.Open(mode, FileAccess.ReadWrite, FileShare.None);
		}

		// Token: 0x060035AF RID: 13743 RVA: 0x000B3E0B File Offset: 0x000B2E0B
		public FileStream Open(FileMode mode, FileAccess access)
		{
			return this.Open(mode, access, FileShare.None);
		}

		// Token: 0x060035B0 RID: 13744 RVA: 0x000B3E16 File Offset: 0x000B2E16
		public FileStream Open(FileMode mode, FileAccess access, FileShare share)
		{
			return new FileStream(this.FullPath, mode, access, share);
		}

		// Token: 0x060035B1 RID: 13745 RVA: 0x000B3E26 File Offset: 0x000B2E26
		public FileStream OpenRead()
		{
			return new FileStream(this.FullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		// Token: 0x060035B2 RID: 13746 RVA: 0x000B3E36 File Offset: 0x000B2E36
		public FileStream OpenWrite()
		{
			return new FileStream(this.FullPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
		}

		// Token: 0x060035B3 RID: 13747 RVA: 0x000B3E48 File Offset: 0x000B2E48
		public void MoveTo(string destFileName)
		{
			if (destFileName == null)
			{
				throw new ArgumentNullException("destFileName");
			}
			if (destFileName.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), "destFileName");
			}
			new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.Write, new string[] { this.FullPath }, false, false).Demand();
			string fullPathInternal = Path.GetFullPathInternal(destFileName);
			new FileIOPermission(FileIOPermissionAccess.Write, new string[] { fullPathInternal }, false, false).Demand();
			if (!Win32Native.MoveFile(this.FullPath, fullPathInternal))
			{
				__Error.WinIOError();
			}
			this.FullPath = fullPathInternal;
			this.OriginalPath = destFileName;
			this._name = Path.GetFileName(fullPathInternal);
			this._dataInitialised = -1;
		}

		// Token: 0x060035B4 RID: 13748 RVA: 0x000B3EF2 File Offset: 0x000B2EF2
		[ComVisible(false)]
		public FileInfo Replace(string destinationFileName, string destinationBackupFileName)
		{
			return this.Replace(destinationFileName, destinationBackupFileName, false);
		}

		// Token: 0x060035B5 RID: 13749 RVA: 0x000B3EFD File Offset: 0x000B2EFD
		[ComVisible(false)]
		public FileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
		{
			File.Replace(this.FullPath, destinationFileName, destinationBackupFileName, ignoreMetadataErrors);
			return new FileInfo(destinationFileName);
		}

		// Token: 0x060035B6 RID: 13750 RVA: 0x000B3F13 File Offset: 0x000B2F13
		public override string ToString()
		{
			return this.OriginalPath;
		}

		// Token: 0x04001BE5 RID: 7141
		private string _name;
	}
}
