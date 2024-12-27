using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.IO
{
	// Token: 0x02000595 RID: 1429
	[ComVisible(true)]
	[FileIOPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
	[Serializable]
	public abstract class FileSystemInfo : MarshalByRefObject, ISerializable
	{
		// Token: 0x0600350E RID: 13582 RVA: 0x000B18C4 File Offset: 0x000B08C4
		protected FileSystemInfo()
		{
		}

		// Token: 0x0600350F RID: 13583 RVA: 0x000B18D4 File Offset: 0x000B08D4
		protected FileSystemInfo(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.FullPath = Path.GetFullPathInternal(info.GetString("FullPath"));
			this.OriginalPath = info.GetString("OriginalPath");
			this._dataInitialised = -1;
		}

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06003510 RID: 13584 RVA: 0x000B192C File Offset: 0x000B092C
		public virtual string FullName
		{
			get
			{
				string text;
				if (this is DirectoryInfo)
				{
					text = Directory.GetDemandDir(this.FullPath, true);
				}
				else
				{
					text = this.FullPath;
				}
				new FileIOPermission(FileIOPermissionAccess.PathDiscovery, text).Demand();
				return this.FullPath;
			}
		}

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x06003511 RID: 13585 RVA: 0x000B196C File Offset: 0x000B096C
		public string Extension
		{
			get
			{
				int length = this.FullPath.Length;
				int num = length;
				while (--num >= 0)
				{
					char c = this.FullPath[num];
					if (c == '.')
					{
						return this.FullPath.Substring(num, length - num);
					}
					if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar || c == Path.VolumeSeparatorChar)
					{
						break;
					}
				}
				return string.Empty;
			}
		}

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x06003512 RID: 13586
		public abstract string Name { get; }

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x06003513 RID: 13587
		public abstract bool Exists { get; }

		// Token: 0x06003514 RID: 13588
		public abstract void Delete();

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x06003515 RID: 13589 RVA: 0x000B19D0 File Offset: 0x000B09D0
		// (set) Token: 0x06003516 RID: 13590 RVA: 0x000B19EB File Offset: 0x000B09EB
		public DateTime CreationTime
		{
			get
			{
				return this.CreationTimeUtc.ToLocalTime();
			}
			set
			{
				this.CreationTimeUtc = value.ToUniversalTime();
			}
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06003517 RID: 13591 RVA: 0x000B19FC File Offset: 0x000B09FC
		// (set) Token: 0x06003518 RID: 13592 RVA: 0x000B1A60 File Offset: 0x000B0A60
		[ComVisible(false)]
		public DateTime CreationTimeUtc
		{
			get
			{
				if (this._dataInitialised == -1)
				{
					this._data = default(Win32Native.WIN32_FILE_ATTRIBUTE_DATA);
					this.Refresh();
				}
				if (this._dataInitialised != 0)
				{
					__Error.WinIOError(this._dataInitialised, this.OriginalPath);
				}
				long num = (long)(((ulong)this._data.ftCreationTimeHigh << 32) | (ulong)this._data.ftCreationTimeLow);
				return DateTime.FromFileTimeUtc(num);
			}
			set
			{
				if (this is DirectoryInfo)
				{
					Directory.SetCreationTimeUtc(this.FullPath, value);
				}
				else
				{
					File.SetCreationTimeUtc(this.FullPath, value);
				}
				this._dataInitialised = -1;
			}
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06003519 RID: 13593 RVA: 0x000B1A8C File Offset: 0x000B0A8C
		// (set) Token: 0x0600351A RID: 13594 RVA: 0x000B1AA7 File Offset: 0x000B0AA7
		public DateTime LastAccessTime
		{
			get
			{
				return this.LastAccessTimeUtc.ToLocalTime();
			}
			set
			{
				this.LastAccessTimeUtc = value.ToUniversalTime();
			}
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x0600351B RID: 13595 RVA: 0x000B1AB8 File Offset: 0x000B0AB8
		// (set) Token: 0x0600351C RID: 13596 RVA: 0x000B1B1C File Offset: 0x000B0B1C
		[ComVisible(false)]
		public DateTime LastAccessTimeUtc
		{
			get
			{
				if (this._dataInitialised == -1)
				{
					this._data = default(Win32Native.WIN32_FILE_ATTRIBUTE_DATA);
					this.Refresh();
				}
				if (this._dataInitialised != 0)
				{
					__Error.WinIOError(this._dataInitialised, this.OriginalPath);
				}
				long num = (long)(((ulong)this._data.ftLastAccessTimeHigh << 32) | (ulong)this._data.ftLastAccessTimeLow);
				return DateTime.FromFileTimeUtc(num);
			}
			set
			{
				if (this is DirectoryInfo)
				{
					Directory.SetLastAccessTimeUtc(this.FullPath, value);
				}
				else
				{
					File.SetLastAccessTimeUtc(this.FullPath, value);
				}
				this._dataInitialised = -1;
			}
		}

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x0600351D RID: 13597 RVA: 0x000B1B48 File Offset: 0x000B0B48
		// (set) Token: 0x0600351E RID: 13598 RVA: 0x000B1B63 File Offset: 0x000B0B63
		public DateTime LastWriteTime
		{
			get
			{
				return this.LastWriteTimeUtc.ToLocalTime();
			}
			set
			{
				this.LastWriteTimeUtc = value.ToUniversalTime();
			}
		}

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x0600351F RID: 13599 RVA: 0x000B1B74 File Offset: 0x000B0B74
		// (set) Token: 0x06003520 RID: 13600 RVA: 0x000B1BD8 File Offset: 0x000B0BD8
		[ComVisible(false)]
		public DateTime LastWriteTimeUtc
		{
			get
			{
				if (this._dataInitialised == -1)
				{
					this._data = default(Win32Native.WIN32_FILE_ATTRIBUTE_DATA);
					this.Refresh();
				}
				if (this._dataInitialised != 0)
				{
					__Error.WinIOError(this._dataInitialised, this.OriginalPath);
				}
				long num = (long)(((ulong)this._data.ftLastWriteTimeHigh << 32) | (ulong)this._data.ftLastWriteTimeLow);
				return DateTime.FromFileTimeUtc(num);
			}
			set
			{
				if (this is DirectoryInfo)
				{
					Directory.SetLastWriteTimeUtc(this.FullPath, value);
				}
				else
				{
					File.SetLastWriteTimeUtc(this.FullPath, value);
				}
				this._dataInitialised = -1;
			}
		}

		// Token: 0x06003521 RID: 13601 RVA: 0x000B1C03 File Offset: 0x000B0C03
		public void Refresh()
		{
			this._dataInitialised = File.FillAttributeInfo(this.FullPath, ref this._data, false, false);
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x06003522 RID: 13602 RVA: 0x000B1C20 File Offset: 0x000B0C20
		// (set) Token: 0x06003523 RID: 13603 RVA: 0x000B1C6C File Offset: 0x000B0C6C
		public FileAttributes Attributes
		{
			get
			{
				if (this._dataInitialised == -1)
				{
					this._data = default(Win32Native.WIN32_FILE_ATTRIBUTE_DATA);
					this.Refresh();
				}
				if (this._dataInitialised != 0)
				{
					__Error.WinIOError(this._dataInitialised, this.OriginalPath);
				}
				return (FileAttributes)this._data.fileAttributes;
			}
			set
			{
				new FileIOPermission(FileIOPermissionAccess.Write, this.FullPath).Demand();
				if (!Win32Native.SetFileAttributes(this.FullPath, (int)value))
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					if (lastWin32Error == 87)
					{
						throw new ArgumentException(Environment.GetResourceString("Arg_InvalidFileAttrs"));
					}
					if (lastWin32Error == 5)
					{
						throw new ArgumentException(Environment.GetResourceString("UnauthorizedAccess_IODenied_NoPathName"));
					}
					__Error.WinIOError(lastWin32Error, this.OriginalPath);
				}
				this._dataInitialised = -1;
			}
		}

		// Token: 0x06003524 RID: 13604 RVA: 0x000B1CDC File Offset: 0x000B0CDC
		[ComVisible(false)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			new FileIOPermission(FileIOPermissionAccess.PathDiscovery, this.FullPath).Demand();
			info.AddValue("OriginalPath", this.OriginalPath, typeof(string));
			info.AddValue("FullPath", this.FullPath, typeof(string));
		}

		// Token: 0x04001BC9 RID: 7113
		private const int ERROR_INVALID_PARAMETER = 87;

		// Token: 0x04001BCA RID: 7114
		internal const int ERROR_ACCESS_DENIED = 5;

		// Token: 0x04001BCB RID: 7115
		internal Win32Native.WIN32_FILE_ATTRIBUTE_DATA _data;

		// Token: 0x04001BCC RID: 7116
		internal int _dataInitialised = -1;

		// Token: 0x04001BCD RID: 7117
		protected string FullPath;

		// Token: 0x04001BCE RID: 7118
		protected string OriginalPath;
	}
}
