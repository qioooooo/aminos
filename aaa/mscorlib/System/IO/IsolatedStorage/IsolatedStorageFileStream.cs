using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.IO.IsolatedStorage
{
	// Token: 0x0200079A RID: 1946
	[ComVisible(true)]
	public class IsolatedStorageFileStream : FileStream
	{
		// Token: 0x060045EF RID: 17903 RVA: 0x000EFB74 File Offset: 0x000EEB74
		private IsolatedStorageFileStream()
		{
		}

		// Token: 0x060045F0 RID: 17904 RVA: 0x000EFB7C File Offset: 0x000EEB7C
		public IsolatedStorageFileStream(string path, FileMode mode)
			: this(path, mode, (mode == FileMode.Append) ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None, null)
		{
		}

		// Token: 0x060045F1 RID: 17905 RVA: 0x000EFB90 File Offset: 0x000EEB90
		public IsolatedStorageFileStream(string path, FileMode mode, IsolatedStorageFile isf)
			: this(path, mode, FileAccess.ReadWrite, FileShare.None, isf)
		{
		}

		// Token: 0x060045F2 RID: 17906 RVA: 0x000EFB9D File Offset: 0x000EEB9D
		public IsolatedStorageFileStream(string path, FileMode mode, FileAccess access)
			: this(path, mode, access, (access == FileAccess.Read) ? FileShare.Read : FileShare.None, 4096, null)
		{
		}

		// Token: 0x060045F3 RID: 17907 RVA: 0x000EFBB6 File Offset: 0x000EEBB6
		public IsolatedStorageFileStream(string path, FileMode mode, FileAccess access, IsolatedStorageFile isf)
			: this(path, mode, access, (access == FileAccess.Read) ? FileShare.Read : FileShare.None, 4096, isf)
		{
		}

		// Token: 0x060045F4 RID: 17908 RVA: 0x000EFBD0 File Offset: 0x000EEBD0
		public IsolatedStorageFileStream(string path, FileMode mode, FileAccess access, FileShare share)
			: this(path, mode, access, share, 4096, null)
		{
		}

		// Token: 0x060045F5 RID: 17909 RVA: 0x000EFBE3 File Offset: 0x000EEBE3
		public IsolatedStorageFileStream(string path, FileMode mode, FileAccess access, FileShare share, IsolatedStorageFile isf)
			: this(path, mode, access, share, 4096, isf)
		{
		}

		// Token: 0x060045F6 RID: 17910 RVA: 0x000EFBF7 File Offset: 0x000EEBF7
		public IsolatedStorageFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize)
			: this(path, mode, access, share, bufferSize, null)
		{
		}

		// Token: 0x060045F7 RID: 17911 RVA: 0x000EFC08 File Offset: 0x000EEC08
		public IsolatedStorageFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, IsolatedStorageFile isf)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 0 || path.Equals("\\"))
			{
				throw new ArgumentException(Environment.GetResourceString("IsolatedStorage_Path"));
			}
			ulong num = 0UL;
			bool flag = false;
			bool flag2 = false;
			if (isf == null)
			{
				this.m_OwnedStore = true;
				isf = IsolatedStorageFile.GetUserStoreForDomain();
			}
			this.m_isf = isf;
			FileIOPermission fileIOPermission = new FileIOPermission(FileIOPermissionAccess.AllAccess, this.m_isf.RootDirectory);
			fileIOPermission.Assert();
			fileIOPermission.PermitOnly();
			this.m_GivenPath = path;
			this.m_FullPath = this.m_isf.GetFullPath(this.m_GivenPath);
			try
			{
				switch (mode)
				{
				case FileMode.CreateNew:
					flag = true;
					goto IL_0105;
				case FileMode.Create:
				case FileMode.OpenOrCreate:
				case FileMode.Truncate:
				case FileMode.Append:
					this.m_isf.Lock();
					flag2 = true;
					try
					{
						FileInfo fileInfo = new FileInfo(this.m_FullPath);
						num = IsolatedStorageFile.RoundToBlockSize((ulong)fileInfo.Length);
						goto IL_0105;
					}
					catch (FileNotFoundException)
					{
						flag = true;
						goto IL_0105;
					}
					catch
					{
						goto IL_0105;
					}
					break;
				case FileMode.Open:
					goto IL_0105;
				}
				throw new ArgumentException(Environment.GetResourceString("IsolatedStorage_FileOpenMode"));
				IL_0105:
				if (flag)
				{
					this.m_isf.ReserveOneBlock();
				}
				try
				{
					this.m_fs = new FileStream(this.m_FullPath, mode, access, share, bufferSize, FileOptions.None, this.m_GivenPath, true);
				}
				catch
				{
					if (flag)
					{
						this.m_isf.UnreserveOneBlock();
					}
					throw;
				}
				if (!flag && (mode == FileMode.Truncate || mode == FileMode.Create))
				{
					ulong num2 = IsolatedStorageFile.RoundToBlockSize((ulong)this.m_fs.Length);
					if (num > num2)
					{
						this.m_isf.Unreserve(num - num2);
					}
					else if (num2 > num)
					{
						this.m_isf.Reserve(num2 - num);
					}
				}
			}
			finally
			{
				if (flag2)
				{
					this.m_isf.Unlock();
				}
			}
			CodeAccessPermission.RevertAll();
		}

		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x060045F8 RID: 17912 RVA: 0x000EFDE8 File Offset: 0x000EEDE8
		public override bool CanRead
		{
			get
			{
				return this.m_fs.CanRead;
			}
		}

		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x060045F9 RID: 17913 RVA: 0x000EFDF5 File Offset: 0x000EEDF5
		public override bool CanWrite
		{
			get
			{
				return this.m_fs.CanWrite;
			}
		}

		// Token: 0x17000C57 RID: 3159
		// (get) Token: 0x060045FA RID: 17914 RVA: 0x000EFE02 File Offset: 0x000EEE02
		public override bool CanSeek
		{
			get
			{
				return this.m_fs.CanSeek;
			}
		}

		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x060045FB RID: 17915 RVA: 0x000EFE0F File Offset: 0x000EEE0F
		public override bool IsAsync
		{
			get
			{
				return this.m_fs.IsAsync;
			}
		}

		// Token: 0x17000C59 RID: 3161
		// (get) Token: 0x060045FC RID: 17916 RVA: 0x000EFE1C File Offset: 0x000EEE1C
		public override long Length
		{
			get
			{
				return this.m_fs.Length;
			}
		}

		// Token: 0x17000C5A RID: 3162
		// (get) Token: 0x060045FD RID: 17917 RVA: 0x000EFE29 File Offset: 0x000EEE29
		// (set) Token: 0x060045FE RID: 17918 RVA: 0x000EFE36 File Offset: 0x000EEE36
		public override long Position
		{
			get
			{
				return this.m_fs.Position;
			}
			set
			{
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				this.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x060045FF RID: 17919 RVA: 0x000EFE5B File Offset: 0x000EEE5B
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.m_fs != null)
				{
					this.m_fs.Close();
				}
				if (this.m_OwnedStore && this.m_isf != null)
				{
					this.m_isf.Close();
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06004600 RID: 17920 RVA: 0x000EFE95 File Offset: 0x000EEE95
		public override void Flush()
		{
			this.m_fs.Flush();
		}

		// Token: 0x17000C5B RID: 3163
		// (get) Token: 0x06004601 RID: 17921 RVA: 0x000EFEA2 File Offset: 0x000EEEA2
		[Obsolete("This property has been deprecated.  Please use IsolatedStorageFileStream's SafeFileHandle property instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public override IntPtr Handle
		{
			[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				this.NotPermittedError();
				return Win32Native.INVALID_HANDLE_VALUE;
			}
		}

		// Token: 0x17000C5C RID: 3164
		// (get) Token: 0x06004602 RID: 17922 RVA: 0x000EFEAF File Offset: 0x000EEEAF
		public override SafeFileHandle SafeFileHandle
		{
			[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				this.NotPermittedError();
				return null;
			}
		}

		// Token: 0x06004603 RID: 17923 RVA: 0x000EFEB8 File Offset: 0x000EEEB8
		public override void SetLength(long value)
		{
			if (value < 0L)
			{
				throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			this.m_isf.Lock();
			try
			{
				ulong length = (ulong)this.m_fs.Length;
				this.m_isf.Reserve(length, (ulong)value);
				try
				{
					this.ZeroInit(length, (ulong)value);
					this.m_fs.SetLength(value);
				}
				catch
				{
					this.m_isf.UndoReserveOperation(length, (ulong)value);
					throw;
				}
				if (length > (ulong)value)
				{
					this.m_isf.UndoReserveOperation((ulong)value, length);
				}
			}
			finally
			{
				this.m_isf.Unlock();
			}
		}

		// Token: 0x06004604 RID: 17924 RVA: 0x000EFF68 File Offset: 0x000EEF68
		private void ZeroInit(ulong oldLen, ulong newLen)
		{
			if (oldLen >= newLen)
			{
				return;
			}
			ulong num = newLen - oldLen;
			byte[] array = new byte[1024];
			long position = this.m_fs.Position;
			this.m_fs.Seek((long)oldLen, SeekOrigin.Begin);
			if (num <= 1024UL)
			{
				this.m_fs.Write(array, 0, (int)num);
				this.m_fs.Position = position;
				return;
			}
			int num2 = 1024 - (int)(oldLen & 1023UL);
			this.m_fs.Write(array, 0, num2);
			num -= (ulong)((long)num2);
			int num3 = (int)(num / 1024UL);
			for (int i = 0; i < num3; i++)
			{
				this.m_fs.Write(array, 0, 1024);
			}
			this.m_fs.Write(array, 0, (int)(num & 1023UL));
			this.m_fs.Position = position;
		}

		// Token: 0x06004605 RID: 17925 RVA: 0x000F003B File Offset: 0x000EF03B
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.m_fs.Read(buffer, offset, count);
		}

		// Token: 0x06004606 RID: 17926 RVA: 0x000F004B File Offset: 0x000EF04B
		public override int ReadByte()
		{
			return this.m_fs.ReadByte();
		}

		// Token: 0x06004607 RID: 17927 RVA: 0x000F0058 File Offset: 0x000EF058
		public override long Seek(long offset, SeekOrigin origin)
		{
			this.m_isf.Lock();
			long num2;
			try
			{
				ulong length = (ulong)this.m_fs.Length;
				ulong num;
				switch (origin)
				{
				case SeekOrigin.Begin:
					num = (ulong)((offset < 0L) ? 0L : offset);
					break;
				case SeekOrigin.Current:
					num = (ulong)((this.m_fs.Position + offset < 0L) ? 0L : (this.m_fs.Position + offset));
					break;
				case SeekOrigin.End:
					num = (ulong)((this.m_fs.Length + offset < 0L) ? 0L : (this.m_fs.Length + offset));
					break;
				default:
					throw new ArgumentException(Environment.GetResourceString("IsolatedStorage_SeekOrigin"));
				}
				this.m_isf.Reserve(length, num);
				try
				{
					this.ZeroInit(length, num);
					num2 = this.m_fs.Seek(offset, origin);
				}
				catch
				{
					this.m_isf.UndoReserveOperation(length, num);
					throw;
				}
			}
			finally
			{
				this.m_isf.Unlock();
			}
			return num2;
		}

		// Token: 0x06004608 RID: 17928 RVA: 0x000F015C File Offset: 0x000EF15C
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.m_isf.Lock();
			try
			{
				ulong length = (ulong)this.m_fs.Length;
				ulong num = (ulong)(this.m_fs.Position + (long)count);
				this.m_isf.Reserve(length, num);
				try
				{
					this.m_fs.Write(buffer, offset, count);
				}
				catch
				{
					this.m_isf.UndoReserveOperation(length, num);
					throw;
				}
			}
			finally
			{
				this.m_isf.Unlock();
			}
		}

		// Token: 0x06004609 RID: 17929 RVA: 0x000F01E8 File Offset: 0x000EF1E8
		public override void WriteByte(byte value)
		{
			this.m_isf.Lock();
			try
			{
				ulong length = (ulong)this.m_fs.Length;
				ulong num = (ulong)(this.m_fs.Position + 1L);
				this.m_isf.Reserve(length, num);
				try
				{
					this.m_fs.WriteByte(value);
				}
				catch
				{
					this.m_isf.UndoReserveOperation(length, num);
					throw;
				}
			}
			finally
			{
				this.m_isf.Unlock();
			}
		}

		// Token: 0x0600460A RID: 17930 RVA: 0x000F0270 File Offset: 0x000EF270
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
		{
			return this.m_fs.BeginRead(buffer, offset, numBytes, userCallback, stateObject);
		}

		// Token: 0x0600460B RID: 17931 RVA: 0x000F0284 File Offset: 0x000EF284
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this.m_fs.EndRead(asyncResult);
		}

		// Token: 0x0600460C RID: 17932 RVA: 0x000F0294 File Offset: 0x000EF294
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
		{
			this.m_isf.Lock();
			IAsyncResult asyncResult;
			try
			{
				ulong length = (ulong)this.m_fs.Length;
				ulong num = (ulong)(this.m_fs.Position + (long)numBytes);
				this.m_isf.Reserve(length, num);
				try
				{
					asyncResult = this.m_fs.BeginWrite(buffer, offset, numBytes, userCallback, stateObject);
				}
				catch
				{
					this.m_isf.UndoReserveOperation(length, num);
					throw;
				}
			}
			finally
			{
				this.m_isf.Unlock();
			}
			return asyncResult;
		}

		// Token: 0x0600460D RID: 17933 RVA: 0x000F0324 File Offset: 0x000EF324
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.m_fs.EndWrite(asyncResult);
		}

		// Token: 0x0600460E RID: 17934 RVA: 0x000F0332 File Offset: 0x000EF332
		internal void NotPermittedError(string str)
		{
			throw new IsolatedStorageException(str);
		}

		// Token: 0x0600460F RID: 17935 RVA: 0x000F033A File Offset: 0x000EF33A
		internal void NotPermittedError()
		{
			this.NotPermittedError(Environment.GetResourceString("IsolatedStorage_Operation"));
		}

		// Token: 0x040022AC RID: 8876
		private const int s_BlockSize = 1024;

		// Token: 0x040022AD RID: 8877
		private const string s_BackSlash = "\\";

		// Token: 0x040022AE RID: 8878
		private FileStream m_fs;

		// Token: 0x040022AF RID: 8879
		private IsolatedStorageFile m_isf;

		// Token: 0x040022B0 RID: 8880
		private string m_GivenPath;

		// Token: 0x040022B1 RID: 8881
		private string m_FullPath;

		// Token: 0x040022B2 RID: 8882
		private bool m_OwnedStore;
	}
}
