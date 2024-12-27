using System;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Data.SqlTypes
{
	// Token: 0x0200034B RID: 843
	public sealed class SqlFileStream : Stream
	{
		// Token: 0x06002D2F RID: 11567 RVA: 0x002A98D0 File Offset: 0x002A8CD0
		public SqlFileStream(string path, byte[] transactionContext, FileAccess access)
			: this(path, transactionContext, access, FileOptions.None, 0L)
		{
		}

		// Token: 0x06002D30 RID: 11568 RVA: 0x002A98EC File Offset: 0x002A8CEC
		public SqlFileStream(string path, byte[] transactionContext, FileAccess access, FileOptions options, long allocationSize)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<sc.SqlFileStream.ctor|API> %d# access=%d options=%d path='%ls' ", this.ObjectID, (int)access, (int)options, path);
			try
			{
				if (transactionContext == null)
				{
					throw ADP.ArgumentNull("transactionContext");
				}
				if (path == null)
				{
					throw ADP.ArgumentNull("path");
				}
				this.m_disposed = false;
				this.m_fs = null;
				this.OpenSqlFileStream(path, transactionContext, access, options, allocationSize);
				this.Name = path;
				this.TransactionContext = transactionContext;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06002D31 RID: 11569 RVA: 0x002A9990 File Offset: 0x002A8D90
		~SqlFileStream()
		{
			this.Dispose(false);
		}

		// Token: 0x06002D32 RID: 11570 RVA: 0x002A99CC File Offset: 0x002A8DCC
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this.m_disposed)
				{
					try
					{
						if (disposing && this.m_fs != null)
						{
							this.m_fs.Close();
							this.m_fs = null;
						}
					}
					finally
					{
						this.m_disposed = true;
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x06002D33 RID: 11571 RVA: 0x002A9A48 File Offset: 0x002A8E48
		// (set) Token: 0x06002D34 RID: 11572 RVA: 0x002A9A5C File Offset: 0x002A8E5C
		public string Name
		{
			get
			{
				return this.m_path;
			}
			private set
			{
				this.m_path = SqlFileStream.GetFullPathInternal(value);
			}
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06002D35 RID: 11573 RVA: 0x002A9A78 File Offset: 0x002A8E78
		// (set) Token: 0x06002D36 RID: 11574 RVA: 0x002A9AA0 File Offset: 0x002A8EA0
		public byte[] TransactionContext
		{
			get
			{
				if (this.m_txn == null)
				{
					return null;
				}
				return (byte[])this.m_txn.Clone();
			}
			private set
			{
				this.m_txn = (byte[])value.Clone();
			}
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06002D37 RID: 11575 RVA: 0x002A9AC0 File Offset: 0x002A8EC0
		public override bool CanRead
		{
			get
			{
				if (this.m_disposed)
				{
					throw ADP.ObjectDisposed(this);
				}
				return this.m_fs.CanRead;
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06002D38 RID: 11576 RVA: 0x002A9AE8 File Offset: 0x002A8EE8
		public override bool CanSeek
		{
			get
			{
				if (this.m_disposed)
				{
					throw ADP.ObjectDisposed(this);
				}
				return this.m_fs.CanSeek;
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06002D39 RID: 11577 RVA: 0x002A9B10 File Offset: 0x002A8F10
		[ComVisible(false)]
		public override bool CanTimeout
		{
			get
			{
				if (this.m_disposed)
				{
					throw ADP.ObjectDisposed(this);
				}
				return this.m_fs.CanTimeout;
			}
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06002D3A RID: 11578 RVA: 0x002A9B38 File Offset: 0x002A8F38
		public override bool CanWrite
		{
			get
			{
				if (this.m_disposed)
				{
					throw ADP.ObjectDisposed(this);
				}
				return this.m_fs.CanWrite;
			}
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06002D3B RID: 11579 RVA: 0x002A9B60 File Offset: 0x002A8F60
		public override long Length
		{
			get
			{
				if (this.m_disposed)
				{
					throw ADP.ObjectDisposed(this);
				}
				return this.m_fs.Length;
			}
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06002D3C RID: 11580 RVA: 0x002A9B88 File Offset: 0x002A8F88
		// (set) Token: 0x06002D3D RID: 11581 RVA: 0x002A9BB0 File Offset: 0x002A8FB0
		public override long Position
		{
			get
			{
				if (this.m_disposed)
				{
					throw ADP.ObjectDisposed(this);
				}
				return this.m_fs.Position;
			}
			set
			{
				if (this.m_disposed)
				{
					throw ADP.ObjectDisposed(this);
				}
				this.m_fs.Position = value;
			}
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x06002D3E RID: 11582 RVA: 0x002A9BD8 File Offset: 0x002A8FD8
		// (set) Token: 0x06002D3F RID: 11583 RVA: 0x002A9C00 File Offset: 0x002A9000
		[ComVisible(false)]
		public override int ReadTimeout
		{
			get
			{
				if (this.m_disposed)
				{
					throw ADP.ObjectDisposed(this);
				}
				return this.m_fs.ReadTimeout;
			}
			set
			{
				if (this.m_disposed)
				{
					throw ADP.ObjectDisposed(this);
				}
				this.m_fs.ReadTimeout = value;
			}
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06002D40 RID: 11584 RVA: 0x002A9C28 File Offset: 0x002A9028
		// (set) Token: 0x06002D41 RID: 11585 RVA: 0x002A9C50 File Offset: 0x002A9050
		[ComVisible(false)]
		public override int WriteTimeout
		{
			get
			{
				if (this.m_disposed)
				{
					throw ADP.ObjectDisposed(this);
				}
				return this.m_fs.WriteTimeout;
			}
			set
			{
				if (this.m_disposed)
				{
					throw ADP.ObjectDisposed(this);
				}
				this.m_fs.WriteTimeout = value;
			}
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x002A9C78 File Offset: 0x002A9078
		public override void Flush()
		{
			if (this.m_disposed)
			{
				throw ADP.ObjectDisposed(this);
			}
			this.m_fs.Flush();
		}

		// Token: 0x06002D43 RID: 11587 RVA: 0x002A9CA0 File Offset: 0x002A90A0
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (this.m_disposed)
			{
				throw ADP.ObjectDisposed(this);
			}
			return this.m_fs.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x06002D44 RID: 11588 RVA: 0x002A9CD0 File Offset: 0x002A90D0
		public override int EndRead(IAsyncResult asyncResult)
		{
			if (this.m_disposed)
			{
				throw ADP.ObjectDisposed(this);
			}
			return this.m_fs.EndRead(asyncResult);
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x002A9CF8 File Offset: 0x002A90F8
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (this.m_disposed)
			{
				throw ADP.ObjectDisposed(this);
			}
			IAsyncResult asyncResult = this.m_fs.BeginWrite(buffer, offset, count, callback, state);
			if (count == 1)
			{
				this.m_fs.Flush();
			}
			return asyncResult;
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x002A9D38 File Offset: 0x002A9138
		public override void EndWrite(IAsyncResult asyncResult)
		{
			if (this.m_disposed)
			{
				throw ADP.ObjectDisposed(this);
			}
			this.m_fs.EndWrite(asyncResult);
		}

		// Token: 0x06002D47 RID: 11591 RVA: 0x002A9D60 File Offset: 0x002A9160
		public override long Seek(long offset, SeekOrigin origin)
		{
			if (this.m_disposed)
			{
				throw ADP.ObjectDisposed(this);
			}
			return this.m_fs.Seek(offset, origin);
		}

		// Token: 0x06002D48 RID: 11592 RVA: 0x002A9D8C File Offset: 0x002A918C
		public override void SetLength(long value)
		{
			if (this.m_disposed)
			{
				throw ADP.ObjectDisposed(this);
			}
			this.m_fs.SetLength(value);
		}

		// Token: 0x06002D49 RID: 11593 RVA: 0x002A9DB4 File Offset: 0x002A91B4
		public override int Read([In] [Out] byte[] buffer, int offset, int count)
		{
			if (this.m_disposed)
			{
				throw ADP.ObjectDisposed(this);
			}
			return this.m_fs.Read(buffer, offset, count);
		}

		// Token: 0x06002D4A RID: 11594 RVA: 0x002A9DE0 File Offset: 0x002A91E0
		public override int ReadByte()
		{
			if (this.m_disposed)
			{
				throw ADP.ObjectDisposed(this);
			}
			return this.m_fs.ReadByte();
		}

		// Token: 0x06002D4B RID: 11595 RVA: 0x002A9E08 File Offset: 0x002A9208
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.m_disposed)
			{
				throw ADP.ObjectDisposed(this);
			}
			this.m_fs.Write(buffer, offset, count);
			if (count == 1)
			{
				this.m_fs.Flush();
			}
		}

		// Token: 0x06002D4C RID: 11596 RVA: 0x002A9E44 File Offset: 0x002A9244
		public override void WriteByte(byte value)
		{
			if (this.m_disposed)
			{
				throw ADP.ObjectDisposed(this);
			}
			this.m_fs.WriteByte(value);
			this.m_fs.Flush();
		}

		// Token: 0x06002D4D RID: 11597 RVA: 0x002A9E78 File Offset: 0x002A9278
		[Conditional("DEBUG")]
		private static void AssertPathFormat(string path)
		{
		}

		// Token: 0x06002D4E RID: 11598 RVA: 0x002A9E88 File Offset: 0x002A9288
		private static string GetFullPathInternal(string path)
		{
			path = path.Trim();
			if (path.Length == 0)
			{
				throw ADP.Argument(Res.GetString("SqlFileStream_InvalidPath"), "path");
			}
			if (path.Length > SqlFileStream.MaxWin32PathLength)
			{
				throw ADP.Argument(Res.GetString("SqlFileStream_InvalidPath"), "path");
			}
			if (path.IndexOfAny(SqlFileStream.InvalidPathChars) >= 0)
			{
				throw ADP.Argument(Res.GetString("SqlFileStream_InvalidPath"), "path");
			}
			if (!path.StartsWith("\\\\", StringComparison.OrdinalIgnoreCase))
			{
				throw ADP.Argument(Res.GetString("SqlFileStream_InvalidPath"), "path");
			}
			path = UnsafeNativeMethods.SafeGetFullPathName(path);
			if (path.StartsWith("\\\\.\\", StringComparison.Ordinal))
			{
				throw ADP.Argument(Res.GetString("SqlFileStream_PathNotValidDiskResource"), "path");
			}
			return path;
		}

		// Token: 0x06002D4F RID: 11599 RVA: 0x002A9F50 File Offset: 0x002A9350
		private static void DemandAccessPermission(string path, FileAccess access)
		{
			FileIOPermissionAccess fileIOPermissionAccess;
			switch (access)
			{
			case FileAccess.Read:
				fileIOPermissionAccess = FileIOPermissionAccess.Read;
				goto IL_0024;
			case FileAccess.Write:
				fileIOPermissionAccess = FileIOPermissionAccess.Write;
				goto IL_0024;
			}
			fileIOPermissionAccess = FileIOPermissionAccess.Read | FileIOPermissionAccess.Write;
			IL_0024:
			bool flag = false;
			try
			{
				FileIOPermission fileIOPermission = new FileIOPermission(fileIOPermissionAccess, path);
				fileIOPermission.Demand();
			}
			catch (PathTooLongException ex)
			{
				flag = true;
				ADP.TraceExceptionWithoutRethrow(ex);
			}
			if (flag)
			{
				new FileIOPermission(PermissionState.Unrestricted)
				{
					AllFiles = fileIOPermissionAccess
				}.Demand();
			}
		}

		// Token: 0x06002D50 RID: 11600 RVA: 0x002A9FD4 File Offset: 0x002A93D4
		private void OpenSqlFileStream(string path, byte[] transactionContext, FileAccess access, FileOptions options, long allocationSize)
		{
			if (access != FileAccess.Read && access != FileAccess.Write && access != FileAccess.ReadWrite)
			{
				throw ADP.ArgumentOutOfRange("access");
			}
			if ((options & (FileOptions)671088639) != FileOptions.None)
			{
				throw ADP.ArgumentOutOfRange("options");
			}
			path = SqlFileStream.GetFullPathInternal(path);
			SqlFileStream.DemandAccessPermission(path, access);
			FileFullEaInformation fileFullEaInformation = null;
			SecurityQualityOfService securityQualityOfService = null;
			UnicodeString unicodeString = null;
			SafeFileHandle safeFileHandle = null;
			int num = 1048704;
			uint num2 = 0U;
			FileShare fileShare;
			uint num3;
			switch (access)
			{
			case FileAccess.Read:
				num |= 1;
				fileShare = FileShare.Read | FileShare.Write | FileShare.Delete;
				num3 = 1U;
				goto IL_008F;
			case FileAccess.Write:
				num |= 2;
				fileShare = FileShare.Read | FileShare.Delete;
				num3 = 4U;
				goto IL_008F;
			}
			num |= 3;
			fileShare = FileShare.Read | FileShare.Delete;
			num3 = 4U;
			IL_008F:
			if ((options & FileOptions.WriteThrough) != FileOptions.None)
			{
				num2 |= 2U;
			}
			if ((options & FileOptions.Asynchronous) == FileOptions.None)
			{
				num2 |= 32U;
			}
			if ((options & FileOptions.SequentialScan) != FileOptions.None)
			{
				num2 |= 4U;
			}
			if ((options & FileOptions.RandomAccess) != FileOptions.None)
			{
				num2 |= 2048U;
			}
			try
			{
				try
				{
					fileFullEaInformation = new FileFullEaInformation(transactionContext);
					securityQualityOfService = new SecurityQualityOfService(UnsafeNativeMethods.SecurityImpersonationLevel.SecurityAnonymous, false, false);
					string text = SqlFileStream.InitializeNtPath(path);
					unicodeString = new UnicodeString(text);
					UnsafeNativeMethods.OBJECT_ATTRIBUTES object_ATTRIBUTES;
					object_ATTRIBUTES.length = Marshal.SizeOf(typeof(UnsafeNativeMethods.OBJECT_ATTRIBUTES));
					object_ATTRIBUTES.rootDirectory = IntPtr.Zero;
					object_ATTRIBUTES.attributes = 64;
					object_ATTRIBUTES.securityDescriptor = IntPtr.Zero;
					object_ATTRIBUTES.securityQualityOfService = securityQualityOfService;
					object_ATTRIBUTES.objectName = unicodeString;
					uint num4 = UnsafeNativeMethods.SetErrorMode(1U);
					uint num5 = 0U;
					try
					{
						Bid.Trace("<sc.SqlFileStream.OpenSqlFileStream|ADV> %d#, desiredAccess=0x%08x, allocationSize=%I64d, fileAttributes=0x%08x, shareAccess=0x%08x, dwCreateDisposition=0x%08x, createOptions=0x%08x\n", this.ObjectID, num, allocationSize, 0U, (int)fileShare, num3, num2);
						UnsafeNativeMethods.IO_STATUS_BLOCK io_STATUS_BLOCK;
						num5 = UnsafeNativeMethods.NtCreateFile(out safeFileHandle, num, ref object_ATTRIBUTES, out io_STATUS_BLOCK, ref allocationSize, 0U, fileShare, num3, num2, fileFullEaInformation, (uint)fileFullEaInformation.Length);
					}
					finally
					{
						UnsafeNativeMethods.SetErrorMode(num4);
					}
					uint num6 = num5;
					if (num6 <= 3221225485U)
					{
						if (num6 != 0U)
						{
							if (num6 == 3221225485U)
							{
								throw ADP.Argument(Res.GetString("SqlFileStream_InvalidParameter"));
							}
						}
						else
						{
							if (safeFileHandle.IsInvalid)
							{
								Win32Exception ex = new Win32Exception(6);
								ADP.TraceExceptionAsReturnValue(ex);
								throw ex;
							}
							UnsafeNativeMethods.FileType fileType = UnsafeNativeMethods.GetFileType(safeFileHandle);
							if (fileType != UnsafeNativeMethods.FileType.Disk)
							{
								safeFileHandle.Dispose();
								throw ADP.Argument(Res.GetString("SqlFileStream_PathNotValidDiskResource"));
							}
							if (access == FileAccess.ReadWrite)
							{
								uint num7 = UnsafeNativeMethods.CTL_CODE(9, 2392, 0, 0);
								uint num8 = 0U;
								if (!UnsafeNativeMethods.DeviceIoControl(safeFileHandle, num7, IntPtr.Zero, 0U, IntPtr.Zero, 0U, out num8, IntPtr.Zero))
								{
									Win32Exception ex2 = new Win32Exception(Marshal.GetLastWin32Error());
									ADP.TraceExceptionAsReturnValue(ex2);
									throw ex2;
								}
							}
							bool flag = false;
							try
							{
								SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
								securityPermission.Assert();
								flag = true;
								this.m_fs = new FileStream(safeFileHandle, access, 1, (options & FileOptions.Asynchronous) != FileOptions.None);
							}
							finally
							{
								if (flag)
								{
									CodeAccessPermission.RevertAssert();
								}
							}
							goto IL_02EC;
						}
					}
					else
					{
						if (num6 == 3221225524U)
						{
							DirectoryNotFoundException ex3 = new DirectoryNotFoundException();
							ADP.TraceExceptionAsReturnValue(ex3);
							throw ex3;
						}
						if (num6 == 3221225539U)
						{
							throw ADP.InvalidOperation(Res.GetString("SqlFileStream_FileAlreadyInTransaction"));
						}
					}
					uint num9 = UnsafeNativeMethods.RtlNtStatusToDosError(num5);
					if (num9 == 317U)
					{
						num9 = num5;
					}
					Win32Exception ex4 = new Win32Exception((int)num9);
					ADP.TraceExceptionAsReturnValue(ex4);
					throw ex4;
				}
				catch
				{
					if (safeFileHandle != null && !safeFileHandle.IsInvalid)
					{
						safeFileHandle.Dispose();
					}
					throw;
				}
				IL_02EC:;
			}
			finally
			{
				if (fileFullEaInformation != null)
				{
					fileFullEaInformation.Dispose();
					fileFullEaInformation = null;
				}
				if (securityQualityOfService != null)
				{
					securityQualityOfService.Dispose();
					securityQualityOfService = null;
				}
				if (unicodeString != null)
				{
					unicodeString.Dispose();
					unicodeString = null;
				}
			}
		}

		// Token: 0x06002D51 RID: 11601 RVA: 0x002AA35C File Offset: 0x002A975C
		private static string InitializeNtPath(string path)
		{
			string text = "\\??\\UNC\\{0}\\{1}";
			string text2 = Guid.NewGuid().ToString("N");
			return string.Format(CultureInfo.InvariantCulture, text, new object[]
			{
				path.Trim(new char[] { '\\' }),
				text2
			});
		}

		// Token: 0x04001CDD RID: 7389
		internal const int DefaultBufferSize = 1;

		// Token: 0x04001CDE RID: 7390
		private const ushort IoControlCodeFunctionCode = 2392;

		// Token: 0x04001CDF RID: 7391
		private static int _objectTypeCount;

		// Token: 0x04001CE0 RID: 7392
		internal readonly int ObjectID = Interlocked.Increment(ref SqlFileStream._objectTypeCount);

		// Token: 0x04001CE1 RID: 7393
		private FileStream m_fs;

		// Token: 0x04001CE2 RID: 7394
		private string m_path;

		// Token: 0x04001CE3 RID: 7395
		private byte[] m_txn;

		// Token: 0x04001CE4 RID: 7396
		private bool m_disposed;

		// Token: 0x04001CE5 RID: 7397
		private static readonly char[] InvalidPathChars = Path.GetInvalidPathChars();

		// Token: 0x04001CE6 RID: 7398
		private static readonly int MaxWin32PathLength = 32766;
	}
}
