using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Threading;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.IO
{
	// Token: 0x020005A7 RID: 1447
	[ComVisible(true)]
	public class FileStream : Stream
	{
		// Token: 0x060035DC RID: 13788 RVA: 0x000B45C4 File Offset: 0x000B35C4
		internal FileStream()
		{
			this._fileName = null;
			this._handle = null;
		}

		// Token: 0x060035DD RID: 13789 RVA: 0x000B45DC File Offset: 0x000B35DC
		public FileStream(string path, FileMode mode)
			: this(path, mode, (mode == FileMode.Append) ? FileAccess.Write : FileAccess.ReadWrite, FileShare.Read, 4096, FileOptions.None, Path.GetFileName(path), false)
		{
		}

		// Token: 0x060035DE RID: 13790 RVA: 0x000B4608 File Offset: 0x000B3608
		public FileStream(string path, FileMode mode, FileAccess access)
			: this(path, mode, access, FileShare.Read, 4096, FileOptions.None, Path.GetFileName(path), false)
		{
		}

		// Token: 0x060035DF RID: 13791 RVA: 0x000B462C File Offset: 0x000B362C
		public FileStream(string path, FileMode mode, FileAccess access, FileShare share)
			: this(path, mode, access, share, 4096, FileOptions.None, Path.GetFileName(path), false)
		{
		}

		// Token: 0x060035E0 RID: 13792 RVA: 0x000B4654 File Offset: 0x000B3654
		public FileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize)
			: this(path, mode, access, share, bufferSize, FileOptions.None, Path.GetFileName(path), false)
		{
		}

		// Token: 0x060035E1 RID: 13793 RVA: 0x000B4678 File Offset: 0x000B3678
		public FileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options)
			: this(path, mode, access, share, bufferSize, options, Path.GetFileName(path), false)
		{
		}

		// Token: 0x060035E2 RID: 13794 RVA: 0x000B469C File Offset: 0x000B369C
		public FileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync)
			: this(path, mode, access, share, bufferSize, useAsync ? FileOptions.Asynchronous : FileOptions.None, Path.GetFileName(path), false)
		{
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x000B46CC File Offset: 0x000B36CC
		public FileStream(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options, FileSecurity fileSecurity)
		{
			object obj;
			Win32Native.SECURITY_ATTRIBUTES secAttrs = FileStream.GetSecAttrs(share, fileSecurity, out obj);
			try
			{
				this.Init(path, mode, (FileAccess)0, (int)rights, true, share, bufferSize, options, secAttrs, Path.GetFileName(path), false);
			}
			finally
			{
				if (obj != null)
				{
					((GCHandle)obj).Free();
				}
			}
		}

		// Token: 0x060035E4 RID: 13796 RVA: 0x000B4728 File Offset: 0x000B3728
		public FileStream(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options)
		{
			Win32Native.SECURITY_ATTRIBUTES secAttrs = FileStream.GetSecAttrs(share);
			this.Init(path, mode, (FileAccess)0, (int)rights, true, share, bufferSize, options, secAttrs, Path.GetFileName(path), false);
		}

		// Token: 0x060035E5 RID: 13797 RVA: 0x000B475C File Offset: 0x000B375C
		internal FileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options, string msgPath, bool bFromProxy)
		{
			Win32Native.SECURITY_ATTRIBUTES secAttrs = FileStream.GetSecAttrs(share);
			this.Init(path, mode, access, 0, false, share, bufferSize, options, secAttrs, msgPath, bFromProxy);
		}

		// Token: 0x060035E6 RID: 13798 RVA: 0x000B4790 File Offset: 0x000B3790
		internal void Init(string path, FileMode mode, FileAccess access, int rights, bool useRights, FileShare share, int bufferSize, FileOptions options, Win32Native.SECURITY_ATTRIBUTES secAttrs, string msgPath, bool bFromProxy)
		{
			this._fileName = msgPath;
			this._exposedHandle = false;
			if (path == null)
			{
				throw new ArgumentNullException("path", Environment.GetResourceString("ArgumentNull_Path"));
			}
			if (path.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyPath"));
			}
			if (Environment.IsWin9X())
			{
				if ((share & FileShare.Delete) != FileShare.None)
				{
					throw new PlatformNotSupportedException(Environment.GetResourceString("NotSupported_FileShareDeleteOnWin9x"));
				}
				if (useRights)
				{
					throw new PlatformNotSupportedException(Environment.GetResourceString("NotSupported_FileSystemRightsOnWin9x"));
				}
			}
			FileShare fileShare = share & ~FileShare.Inheritable;
			string text = null;
			if (mode < FileMode.CreateNew || mode > FileMode.Append)
			{
				text = "mode";
			}
			else if (!useRights && (access < FileAccess.Read || access > FileAccess.ReadWrite))
			{
				text = "access";
			}
			else if (useRights && (rights < 1 || rights > 2032127))
			{
				text = "rights";
			}
			else if ((fileShare < FileShare.None) || fileShare > (FileShare.Read | FileShare.Write | FileShare.Delete))
			{
				text = "share";
			}
			if (text != null)
			{
				throw new ArgumentOutOfRangeException(text, Environment.GetResourceString("ArgumentOutOfRange_Enum"));
			}
			if (options != FileOptions.None && (options & (FileOptions)67092479) != FileOptions.None)
			{
				throw new ArgumentOutOfRangeException("options", Environment.GetResourceString("ArgumentOutOfRange_Enum"));
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			if (((!useRights && (access & FileAccess.Write) == (FileAccess)0) || (useRights && (rights & 278) == 0)) && (mode == FileMode.Truncate || mode == FileMode.CreateNew || mode == FileMode.Create || mode == FileMode.Append))
			{
				if (!useRights)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidFileMode&AccessCombo"), new object[] { mode, access }));
				}
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidFileMode&RightsCombo"), new object[]
				{
					mode,
					(FileSystemRights)rights
				}));
			}
			else
			{
				if (useRights && mode == FileMode.Truncate)
				{
					if (rights != 278)
					{
						throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidFileModeTruncate&RightsCombo"), new object[]
						{
							mode,
							(FileSystemRights)rights
						}));
					}
					useRights = false;
					access = FileAccess.Write;
				}
				int num;
				if (!useRights)
				{
					num = ((access == FileAccess.Read) ? int.MinValue : ((access == FileAccess.Write) ? 1073741824 : (-1073741824)));
				}
				else
				{
					num = rights;
				}
				string fullPathInternal = Path.GetFullPathInternal(path);
				this._fileName = fullPathInternal;
				if (fullPathInternal.StartsWith("\\\\.\\", StringComparison.Ordinal))
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_DevicesNotSupported"));
				}
				FileIOPermissionAccess fileIOPermissionAccess = FileIOPermissionAccess.NoAccess;
				if ((!useRights && (access & FileAccess.Read) != (FileAccess)0) || (useRights && (rights & 131241) != 0))
				{
					if (mode == FileMode.Append)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_InvalidAppendMode"));
					}
					fileIOPermissionAccess |= FileIOPermissionAccess.Read;
				}
				if ((!useRights && (access & FileAccess.Write) != (FileAccess)0) || (useRights && (rights & 852310) != 0))
				{
					if (mode == FileMode.Append)
					{
						fileIOPermissionAccess |= FileIOPermissionAccess.Append;
					}
					else
					{
						fileIOPermissionAccess |= FileIOPermissionAccess.Write;
					}
				}
				AccessControlActions accessControlActions = ((secAttrs != null && secAttrs.pSecurityDescriptor != null) ? AccessControlActions.Change : AccessControlActions.None);
				new FileIOPermission(fileIOPermissionAccess, accessControlActions, new string[] { fullPathInternal }, false, false).Demand();
				share &= ~FileShare.Inheritable;
				bool flag = mode == FileMode.Append;
				if (mode == FileMode.Append)
				{
					mode = FileMode.OpenOrCreate;
				}
				if (FileStream._canUseAsync && (options & FileOptions.Asynchronous) != FileOptions.None)
				{
					this._isAsync = true;
				}
				else
				{
					options &= ~FileOptions.Asynchronous;
				}
				int num2 = (int)options;
				num2 |= 1048576;
				int num3 = Win32Native.SetErrorMode(1);
				try
				{
					this._handle = Win32Native.SafeCreateFile(fullPathInternal, num, share, secAttrs, mode, num2, Win32Native.NULL);
					if (this._handle.IsInvalid)
					{
						int num4 = Marshal.GetLastWin32Error();
						if (num4 == 3 && fullPathInternal.Equals(Directory.InternalGetDirectoryRoot(fullPathInternal)))
						{
							num4 = 5;
						}
						bool flag2 = false;
						if (!bFromProxy)
						{
							try
							{
								new FileIOPermission(FileIOPermissionAccess.PathDiscovery, new string[] { this._fileName }, false, false).Demand();
								flag2 = true;
							}
							catch (SecurityException)
							{
							}
						}
						if (flag2)
						{
							__Error.WinIOError(num4, this._fileName);
						}
						else
						{
							__Error.WinIOError(num4, msgPath);
						}
					}
				}
				finally
				{
					Win32Native.SetErrorMode(num3);
				}
				int fileType = Win32Native.GetFileType(this._handle);
				if (fileType != 1)
				{
					this._handle.Close();
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_FileStreamOnNonFiles"));
				}
				if (this._isAsync)
				{
					bool flag3 = false;
					new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
					try
					{
						flag3 = ThreadPool.BindHandle(this._handle);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
						if (!flag3)
						{
							this._handle.Close();
						}
					}
					if (!flag3)
					{
						throw new IOException(Environment.GetResourceString("IO.IO_BindHandleFailed"));
					}
				}
				if (!useRights)
				{
					this._canRead = (access & FileAccess.Read) != (FileAccess)0;
					this._canWrite = (access & FileAccess.Write) != (FileAccess)0;
				}
				else
				{
					this._canRead = (rights & 1) != 0;
					this._canWrite = (rights & 2) != 0 || (rights & 4) != 0;
				}
				this._canSeek = true;
				this._isPipe = false;
				this._pos = 0L;
				this._bufferSize = bufferSize;
				this._readPos = 0;
				this._readLen = 0;
				this._writePos = 0;
				if (flag)
				{
					this._appendStart = this.SeekCore(0L, SeekOrigin.End);
					return;
				}
				this._appendStart = -1L;
				return;
			}
		}

		// Token: 0x060035E7 RID: 13799 RVA: 0x000B4CBC File Offset: 0x000B3CBC
		[Obsolete("This constructor has been deprecated.  Please use new FileStream(SafeFileHandle handle, FileAccess access) instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public FileStream(IntPtr handle, FileAccess access)
			: this(handle, access, true, 4096, false)
		{
		}

		// Token: 0x060035E8 RID: 13800 RVA: 0x000B4CCD File Offset: 0x000B3CCD
		[Obsolete("This constructor has been deprecated.  Please use new FileStream(SafeFileHandle handle, FileAccess access) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public FileStream(IntPtr handle, FileAccess access, bool ownsHandle)
			: this(handle, access, ownsHandle, 4096, false)
		{
		}

		// Token: 0x060035E9 RID: 13801 RVA: 0x000B4CDE File Offset: 0x000B3CDE
		[Obsolete("This constructor has been deprecated.  Please use new FileStream(SafeFileHandle handle, FileAccess access, int bufferSize) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public FileStream(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize)
			: this(handle, access, ownsHandle, bufferSize, false)
		{
		}

		// Token: 0x060035EA RID: 13802 RVA: 0x000B4CEC File Offset: 0x000B3CEC
		[Obsolete("This constructor has been deprecated.  Please use new FileStream(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public FileStream(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize, bool isAsync)
			: this(new SafeFileHandle(handle, ownsHandle), access, bufferSize, isAsync)
		{
		}

		// Token: 0x060035EB RID: 13803 RVA: 0x000B4D00 File Offset: 0x000B3D00
		public FileStream(SafeFileHandle handle, FileAccess access)
			: this(handle, access, 4096, false)
		{
		}

		// Token: 0x060035EC RID: 13804 RVA: 0x000B4D10 File Offset: 0x000B3D10
		public FileStream(SafeFileHandle handle, FileAccess access, int bufferSize)
			: this(handle, access, bufferSize, false)
		{
		}

		// Token: 0x060035ED RID: 13805 RVA: 0x000B4D1C File Offset: 0x000B3D1C
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public FileStream(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync)
		{
			if (handle.IsInvalid)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidHandle"), "handle");
			}
			this._handle = handle;
			this._exposedHandle = true;
			if (access < FileAccess.Read || access > FileAccess.ReadWrite)
			{
				throw new ArgumentOutOfRangeException("access", Environment.GetResourceString("ArgumentOutOfRange_Enum"));
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			int fileType = Win32Native.GetFileType(this._handle);
			this._isAsync = isAsync && FileStream._canUseAsync;
			this._canRead = (FileAccess)0 != (access & FileAccess.Read);
			this._canWrite = (FileAccess)0 != (access & FileAccess.Write);
			this._canSeek = fileType == 1;
			this._bufferSize = bufferSize;
			this._readPos = 0;
			this._readLen = 0;
			this._writePos = 0;
			this._fileName = null;
			this._isPipe = fileType == 3;
			if (this._isAsync)
			{
				bool flag = false;
				try
				{
					flag = ThreadPool.BindHandle(this._handle);
				}
				catch (ApplicationException)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_HandleNotAsync"));
				}
				if (!flag)
				{
					throw new IOException(Environment.GetResourceString("IO.IO_BindHandleFailed"));
				}
			}
			else if (fileType != 3)
			{
				this.VerifyHandleIsSync();
			}
			if (this._canSeek)
			{
				this.SeekCore(0L, SeekOrigin.Current);
				return;
			}
			this._pos = 0L;
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x000B4E74 File Offset: 0x000B3E74
		private static Win32Native.SECURITY_ATTRIBUTES GetSecAttrs(FileShare share)
		{
			Win32Native.SECURITY_ATTRIBUTES security_ATTRIBUTES = null;
			if ((share & FileShare.Inheritable) != FileShare.None)
			{
				security_ATTRIBUTES = new Win32Native.SECURITY_ATTRIBUTES();
				security_ATTRIBUTES.nLength = Marshal.SizeOf(security_ATTRIBUTES);
				security_ATTRIBUTES.bInheritHandle = 1;
			}
			return security_ATTRIBUTES;
		}

		// Token: 0x060035EF RID: 13807 RVA: 0x000B4EA4 File Offset: 0x000B3EA4
		private unsafe static Win32Native.SECURITY_ATTRIBUTES GetSecAttrs(FileShare share, FileSecurity fileSecurity, out object pinningHandle)
		{
			pinningHandle = null;
			Win32Native.SECURITY_ATTRIBUTES security_ATTRIBUTES = null;
			if ((share & FileShare.Inheritable) != FileShare.None || fileSecurity != null)
			{
				security_ATTRIBUTES = new Win32Native.SECURITY_ATTRIBUTES();
				security_ATTRIBUTES.nLength = Marshal.SizeOf(security_ATTRIBUTES);
				if ((share & FileShare.Inheritable) != FileShare.None)
				{
					security_ATTRIBUTES.bInheritHandle = 1;
				}
				if (fileSecurity != null)
				{
					byte[] securityDescriptorBinaryForm = fileSecurity.GetSecurityDescriptorBinaryForm();
					pinningHandle = GCHandle.Alloc(securityDescriptorBinaryForm, GCHandleType.Pinned);
					fixed (byte* ptr = securityDescriptorBinaryForm)
					{
						security_ATTRIBUTES.pSecurityDescriptor = ptr;
					}
				}
			}
			return security_ATTRIBUTES;
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x000B4F1C File Offset: 0x000B3F1C
		private void VerifyHandleIsSync()
		{
			byte[] array = new byte[1];
			int num = 0;
			if (this.CanRead)
			{
				this.ReadFileNative(this._handle, array, 0, 0, null, out num);
			}
			else if (this.CanWrite)
			{
				this.WriteFileNative(this._handle, array, 0, 0, null, out num);
			}
			if (num == 87)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_HandleNotSync"));
			}
			if (num == 6)
			{
				__Error.WinIOError(num, "<OS handle>");
			}
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x060035F1 RID: 13809 RVA: 0x000B4F90 File Offset: 0x000B3F90
		public override bool CanRead
		{
			get
			{
				return this._canRead;
			}
		}

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x060035F2 RID: 13810 RVA: 0x000B4F98 File Offset: 0x000B3F98
		public override bool CanWrite
		{
			get
			{
				return this._canWrite;
			}
		}

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x060035F3 RID: 13811 RVA: 0x000B4FA0 File Offset: 0x000B3FA0
		public override bool CanSeek
		{
			get
			{
				return this._canSeek;
			}
		}

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x060035F4 RID: 13812 RVA: 0x000B4FA8 File Offset: 0x000B3FA8
		public virtual bool IsAsync
		{
			get
			{
				return this._isAsync;
			}
		}

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x060035F5 RID: 13813 RVA: 0x000B4FB0 File Offset: 0x000B3FB0
		public override long Length
		{
			get
			{
				if (this._handle.IsClosed)
				{
					__Error.FileNotOpen();
				}
				if (!this.CanSeek)
				{
					__Error.SeekNotSupported();
				}
				int num = 0;
				int fileSize = Win32Native.GetFileSize(this._handle, out num);
				if (fileSize == -1)
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					if (lastWin32Error != 0)
					{
						__Error.WinIOError(lastWin32Error, string.Empty);
					}
				}
				long num2 = ((long)num << 32) | (long)((ulong)fileSize);
				if (this._writePos > 0 && this._pos + (long)this._writePos > num2)
				{
					num2 = (long)this._writePos + this._pos;
				}
				return num2;
			}
		}

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x060035F6 RID: 13814 RVA: 0x000B503C File Offset: 0x000B403C
		public string Name
		{
			get
			{
				if (this._fileName == null)
				{
					return Environment.GetResourceString("IO_UnknownFileName");
				}
				new FileIOPermission(FileIOPermissionAccess.PathDiscovery, new string[] { this._fileName }, false, false).Demand();
				return this._fileName;
			}
		}

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x060035F7 RID: 13815 RVA: 0x000B5080 File Offset: 0x000B4080
		internal string NameInternal
		{
			get
			{
				if (this._fileName == null)
				{
					return "<UnknownFileName>";
				}
				return this._fileName;
			}
		}

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x060035F8 RID: 13816 RVA: 0x000B5098 File Offset: 0x000B4098
		// (set) Token: 0x060035F9 RID: 13817 RVA: 0x000B50F0 File Offset: 0x000B40F0
		public override long Position
		{
			get
			{
				if (this._handle.IsClosed)
				{
					__Error.FileNotOpen();
				}
				if (!this.CanSeek)
				{
					__Error.SeekNotSupported();
				}
				if (this._exposedHandle)
				{
					this.VerifyOSHandlePosition();
				}
				return this._pos + (long)(this._readPos - this._readLen + this._writePos);
			}
			set
			{
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._writePos > 0)
				{
					this.FlushWrite(false);
				}
				this._readPos = 0;
				this._readLen = 0;
				this.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x060035FA RID: 13818 RVA: 0x000B513E File Offset: 0x000B413E
		public FileSecurity GetAccessControl()
		{
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			return new FileSecurity(this._handle, this._fileName, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
		}

		// Token: 0x060035FB RID: 13819 RVA: 0x000B5165 File Offset: 0x000B4165
		public void SetAccessControl(FileSecurity fileSecurity)
		{
			if (fileSecurity == null)
			{
				throw new ArgumentNullException("fileSecurity");
			}
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			fileSecurity.Persist(this._handle, this._fileName);
		}

		// Token: 0x060035FC RID: 13820 RVA: 0x000B519C File Offset: 0x000B419C
		private unsafe static void AsyncFSCallback(uint errorCode, uint numBytes, NativeOverlapped* pOverlapped)
		{
			Overlapped overlapped = Overlapped.Unpack(pOverlapped);
			FileStreamAsyncResult fileStreamAsyncResult = (FileStreamAsyncResult)overlapped.AsyncResult;
			fileStreamAsyncResult._numBytes = (int)numBytes;
			if (errorCode == 109U || errorCode == 232U)
			{
				errorCode = 0U;
			}
			fileStreamAsyncResult._errorCode = (int)errorCode;
			fileStreamAsyncResult._completedSynchronously = false;
			fileStreamAsyncResult._isComplete = true;
			ManualResetEvent waitHandle = fileStreamAsyncResult._waitHandle;
			if (waitHandle != null && !waitHandle.Set())
			{
				__Error.WinIOError();
			}
			AsyncCallback userCallback = fileStreamAsyncResult._userCallback;
			if (userCallback != null)
			{
				userCallback(fileStreamAsyncResult);
			}
		}

		// Token: 0x060035FD RID: 13821 RVA: 0x000B5218 File Offset: 0x000B4218
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (this._handle != null && !this._handle.IsClosed && this._writePos > 0)
				{
					this.FlushWrite(!disposing);
				}
			}
			finally
			{
				if (this._handle != null && !this._handle.IsClosed)
				{
					this._handle.Dispose();
				}
				this._canRead = false;
				this._canWrite = false;
				this._canSeek = false;
				base.Dispose(disposing);
			}
		}

		// Token: 0x060035FE RID: 13822 RVA: 0x000B529C File Offset: 0x000B429C
		~FileStream()
		{
			if (this._handle != null)
			{
				this.Dispose(false);
			}
		}

		// Token: 0x060035FF RID: 13823 RVA: 0x000B52D4 File Offset: 0x000B42D4
		public override void Flush()
		{
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			if (this._writePos > 0)
			{
				this.FlushWrite(false);
			}
			else if (this._readPos < this._readLen && this.CanSeek)
			{
				this.FlushRead();
			}
			this._readPos = 0;
			this._readLen = 0;
		}

		// Token: 0x06003600 RID: 13824 RVA: 0x000B532F File Offset: 0x000B432F
		private void FlushRead()
		{
			if (this._readPos - this._readLen != 0)
			{
				this.SeekCore((long)(this._readPos - this._readLen), SeekOrigin.Current);
			}
			this._readPos = 0;
			this._readLen = 0;
		}

		// Token: 0x06003601 RID: 13825 RVA: 0x000B5364 File Offset: 0x000B4364
		private void FlushWrite(bool calledFromFinalizer)
		{
			if (this._isAsync)
			{
				IAsyncResult asyncResult = this.BeginWriteCore(this._buffer, 0, this._writePos, null, null);
				if (!calledFromFinalizer)
				{
					this.EndWrite(asyncResult);
				}
			}
			else
			{
				this.WriteCore(this._buffer, 0, this._writePos);
			}
			this._writePos = 0;
		}

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06003602 RID: 13826 RVA: 0x000B53B5 File Offset: 0x000B43B5
		[Obsolete("This property has been deprecated.  Please use FileStream's SafeFileHandle property instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public virtual IntPtr Handle
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				this.Flush();
				this._readPos = 0;
				this._readLen = 0;
				this._writePos = 0;
				this._exposedHandle = true;
				return this._handle.DangerousGetHandle();
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x06003603 RID: 13827 RVA: 0x000B53E4 File Offset: 0x000B43E4
		public virtual SafeFileHandle SafeFileHandle
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				this.Flush();
				this._readPos = 0;
				this._readLen = 0;
				this._writePos = 0;
				this._exposedHandle = true;
				return this._handle;
			}
		}

		// Token: 0x06003604 RID: 13828 RVA: 0x000B5410 File Offset: 0x000B4410
		public override void SetLength(long value)
		{
			if (value < 0L)
			{
				throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			if (!this.CanSeek)
			{
				__Error.SeekNotSupported();
			}
			if (!this.CanWrite)
			{
				__Error.WriteNotSupported();
			}
			if (this._writePos > 0)
			{
				this.FlushWrite(false);
			}
			else if (this._readPos < this._readLen)
			{
				this.FlushRead();
			}
			this._readPos = 0;
			this._readLen = 0;
			if (this._appendStart != -1L && value < this._appendStart)
			{
				throw new IOException(Environment.GetResourceString("IO.IO_SetLengthAppendTruncate"));
			}
			this.SetLengthCore(value);
		}

		// Token: 0x06003605 RID: 13829 RVA: 0x000B54C4 File Offset: 0x000B44C4
		private void SetLengthCore(long value)
		{
			long pos = this._pos;
			if (this._exposedHandle)
			{
				this.VerifyOSHandlePosition();
			}
			if (this._pos != value)
			{
				this.SeekCore(value, SeekOrigin.Begin);
			}
			if (!Win32Native.SetEndOfFile(this._handle))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error == 87)
				{
					throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_FileLengthTooBig"));
				}
				__Error.WinIOError(lastWin32Error, string.Empty);
			}
			if (pos != value)
			{
				if (pos < value)
				{
					this.SeekCore(pos, SeekOrigin.Begin);
					return;
				}
				this.SeekCore(0L, SeekOrigin.End);
			}
		}

		// Token: 0x06003606 RID: 13830 RVA: 0x000B554C File Offset: 0x000B454C
		public override int Read([In] [Out] byte[] array, int offset, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array", Environment.GetResourceString("ArgumentNull_Buffer"));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (array.Length - offset < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			bool flag = false;
			int num = this._readLen - this._readPos;
			if (num == 0)
			{
				if (!this.CanRead)
				{
					__Error.ReadNotSupported();
				}
				if (this._writePos > 0)
				{
					this.FlushWrite(false);
				}
				if (!this.CanSeek || count >= this._bufferSize)
				{
					num = this.ReadCore(array, offset, count);
					this._readPos = 0;
					this._readLen = 0;
					return num;
				}
				if (this._buffer == null)
				{
					this._buffer = new byte[this._bufferSize];
				}
				num = this.ReadCore(this._buffer, 0, this._bufferSize);
				if (num == 0)
				{
					return 0;
				}
				flag = num < this._bufferSize;
				this._readPos = 0;
				this._readLen = num;
			}
			if (num > count)
			{
				num = count;
			}
			Buffer.InternalBlockCopy(this._buffer, this._readPos, array, offset, num);
			this._readPos += num;
			if (!this._isPipe && num < count && !flag)
			{
				int num2 = this.ReadCore(array, offset + num, count - num);
				num += num2;
				this._readPos = 0;
				this._readLen = 0;
			}
			return num;
		}

		// Token: 0x06003607 RID: 13831 RVA: 0x000B56D0 File Offset: 0x000B46D0
		private int ReadCore(byte[] buffer, int offset, int count)
		{
			if (this._isAsync)
			{
				IAsyncResult asyncResult = this.BeginReadCore(buffer, offset, count, null, null, 0);
				return this.EndRead(asyncResult);
			}
			if (this._exposedHandle)
			{
				this.VerifyOSHandlePosition();
			}
			int num = 0;
			int num2 = this.ReadFileNative(this._handle, buffer, offset, count, null, out num);
			if (num2 == -1)
			{
				if (num == 109)
				{
					num2 = 0;
				}
				else
				{
					if (num == 87)
					{
						throw new ArgumentException(Environment.GetResourceString("Arg_HandleNotSync"));
					}
					__Error.WinIOError(num, string.Empty);
				}
			}
			this._pos += (long)num2;
			return num2;
		}

		// Token: 0x06003608 RID: 13832 RVA: 0x000B575C File Offset: 0x000B475C
		public override long Seek(long offset, SeekOrigin origin)
		{
			if (origin < SeekOrigin.Begin || origin > SeekOrigin.End)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidSeekOrigin"));
			}
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			if (!this.CanSeek)
			{
				__Error.SeekNotSupported();
			}
			if (this._writePos > 0)
			{
				this.FlushWrite(false);
			}
			else if (origin == SeekOrigin.Current)
			{
				offset -= (long)(this._readLen - this._readPos);
			}
			if (this._exposedHandle)
			{
				this.VerifyOSHandlePosition();
			}
			long num = this._pos + (long)(this._readPos - this._readLen);
			long num2 = this.SeekCore(offset, origin);
			if (this._appendStart != -1L && num2 < this._appendStart)
			{
				this.SeekCore(num, SeekOrigin.Begin);
				throw new IOException(Environment.GetResourceString("IO.IO_SeekAppendOverwrite"));
			}
			if (this._readLen > 0)
			{
				if (num == num2)
				{
					if (this._readPos > 0)
					{
						Buffer.InternalBlockCopy(this._buffer, this._readPos, this._buffer, 0, this._readLen - this._readPos);
						this._readLen -= this._readPos;
						this._readPos = 0;
					}
					if (this._readLen > 0)
					{
						this.SeekCore((long)this._readLen, SeekOrigin.Current);
					}
				}
				else if (num - (long)this._readPos < num2 && num2 < num + (long)this._readLen - (long)this._readPos)
				{
					int num3 = (int)(num2 - num);
					Buffer.InternalBlockCopy(this._buffer, this._readPos + num3, this._buffer, 0, this._readLen - (this._readPos + num3));
					this._readLen -= this._readPos + num3;
					this._readPos = 0;
					if (this._readLen > 0)
					{
						this.SeekCore((long)this._readLen, SeekOrigin.Current);
					}
				}
				else
				{
					this._readPos = 0;
					this._readLen = 0;
				}
			}
			return num2;
		}

		// Token: 0x06003609 RID: 13833 RVA: 0x000B592C File Offset: 0x000B492C
		private long SeekCore(long offset, SeekOrigin origin)
		{
			int num = 0;
			long num2 = Win32Native.SetFilePointer(this._handle, offset, origin, out num);
			if (num2 == -1L)
			{
				if (num == 6 && !this._handle.IsInvalid)
				{
					this._handle.Dispose();
				}
				__Error.WinIOError(num, string.Empty);
			}
			this._pos = num2;
			return num2;
		}

		// Token: 0x0600360A RID: 13834 RVA: 0x000B5984 File Offset: 0x000B4984
		private void VerifyOSHandlePosition()
		{
			if (!this.CanSeek)
			{
				return;
			}
			long pos = this._pos;
			long num = this.SeekCore(0L, SeekOrigin.Current);
			if (num != pos)
			{
				this._readPos = 0;
				this._readLen = 0;
				if (this._writePos > 0)
				{
					this._writePos = 0;
					throw new IOException(Environment.GetResourceString("IO.IO_FileStreamHandlePosition"));
				}
			}
		}

		// Token: 0x0600360B RID: 13835 RVA: 0x000B59E0 File Offset: 0x000B49E0
		public override void Write(byte[] array, int offset, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array", Environment.GetResourceString("ArgumentNull_Buffer"));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (array.Length - offset < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			if (this._writePos == 0)
			{
				if (!this.CanWrite)
				{
					__Error.WriteNotSupported();
				}
				if (this._readPos < this._readLen)
				{
					this.FlushRead();
				}
				this._readPos = 0;
				this._readLen = 0;
			}
			if (this._writePos > 0)
			{
				int num = this._bufferSize - this._writePos;
				if (num > 0)
				{
					if (num > count)
					{
						num = count;
					}
					Buffer.InternalBlockCopy(array, offset, this._buffer, this._writePos, num);
					this._writePos += num;
					if (count == num)
					{
						return;
					}
					offset += num;
					count -= num;
				}
				if (this._isAsync)
				{
					IAsyncResult asyncResult = this.BeginWriteCore(this._buffer, 0, this._writePos, null, null);
					this.EndWrite(asyncResult);
				}
				else
				{
					this.WriteCore(this._buffer, 0, this._writePos);
				}
				this._writePos = 0;
			}
			if (count >= this._bufferSize)
			{
				this.WriteCore(array, offset, count);
				return;
			}
			if (count == 0)
			{
				return;
			}
			if (this._buffer == null)
			{
				this._buffer = new byte[this._bufferSize];
			}
			Buffer.InternalBlockCopy(array, offset, this._buffer, this._writePos, count);
			this._writePos = count;
		}

		// Token: 0x0600360C RID: 13836 RVA: 0x000B5B7C File Offset: 0x000B4B7C
		private void WriteCore(byte[] buffer, int offset, int count)
		{
			if (this._isAsync)
			{
				IAsyncResult asyncResult = this.BeginWriteCore(buffer, offset, count, null, null);
				this.EndWrite(asyncResult);
				return;
			}
			if (this._exposedHandle)
			{
				this.VerifyOSHandlePosition();
			}
			int num = 0;
			int num2 = this.WriteFileNative(this._handle, buffer, offset, count, null, out num);
			if (num2 == -1)
			{
				if (num == 232)
				{
					num2 = 0;
				}
				else
				{
					if (num == 87)
					{
						throw new IOException(Environment.GetResourceString("IO.IO_FileTooLongOrHandleNotSync"));
					}
					__Error.WinIOError(num, string.Empty);
				}
			}
			this._pos += (long)num2;
		}

		// Token: 0x0600360D RID: 13837 RVA: 0x000B5C08 File Offset: 0x000B4C08
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginRead(byte[] array, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (numBytes < 0)
			{
				throw new ArgumentOutOfRangeException("numBytes", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (array.Length - offset < numBytes)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			if (!this._isAsync)
			{
				return base.BeginRead(array, offset, numBytes, userCallback, stateObject);
			}
			if (!this.CanRead)
			{
				__Error.ReadNotSupported();
			}
			if (this._isPipe)
			{
				if (this._readPos < this._readLen)
				{
					int num = this._readLen - this._readPos;
					if (num > numBytes)
					{
						num = numBytes;
					}
					Buffer.InternalBlockCopy(this._buffer, this._readPos, array, offset, num);
					this._readPos += num;
					FileStreamAsyncResult fileStreamAsyncResult = FileStreamAsyncResult.CreateBufferedReadResult(num, userCallback, stateObject);
					fileStreamAsyncResult.CallUserCallback();
					return fileStreamAsyncResult;
				}
				return this.BeginReadCore(array, offset, numBytes, userCallback, stateObject, 0);
			}
			else
			{
				if (this._writePos > 0)
				{
					this.FlushWrite(false);
				}
				if (this._readPos == this._readLen)
				{
					if (numBytes < this._bufferSize)
					{
						if (this._buffer == null)
						{
							this._buffer = new byte[this._bufferSize];
						}
						IAsyncResult asyncResult = this.BeginReadCore(this._buffer, 0, this._bufferSize, null, null, 0);
						this._readLen = this.EndRead(asyncResult);
						int num2 = this._readLen;
						if (num2 > numBytes)
						{
							num2 = numBytes;
						}
						Buffer.InternalBlockCopy(this._buffer, 0, array, offset, num2);
						this._readPos = num2;
						FileStreamAsyncResult fileStreamAsyncResult = FileStreamAsyncResult.CreateBufferedReadResult(num2, userCallback, stateObject);
						fileStreamAsyncResult.CallUserCallback();
						return fileStreamAsyncResult;
					}
					this._readPos = 0;
					this._readLen = 0;
					return this.BeginReadCore(array, offset, numBytes, userCallback, stateObject, 0);
				}
				else
				{
					int num3 = this._readLen - this._readPos;
					if (num3 > numBytes)
					{
						num3 = numBytes;
					}
					Buffer.InternalBlockCopy(this._buffer, this._readPos, array, offset, num3);
					this._readPos += num3;
					if (num3 >= numBytes || this._isPipe)
					{
						FileStreamAsyncResult fileStreamAsyncResult = FileStreamAsyncResult.CreateBufferedReadResult(num3, userCallback, stateObject);
						fileStreamAsyncResult.CallUserCallback();
						return fileStreamAsyncResult;
					}
					this._readPos = 0;
					this._readLen = 0;
					return this.BeginReadCore(array, offset + num3, numBytes - num3, userCallback, stateObject, num3);
				}
			}
		}

		// Token: 0x0600360E RID: 13838 RVA: 0x000B5E50 File Offset: 0x000B4E50
		private unsafe FileStreamAsyncResult BeginReadCore(byte[] bytes, int offset, int numBytes, AsyncCallback userCallback, object stateObject, int numBufferedBytesRead)
		{
			FileStreamAsyncResult fileStreamAsyncResult = new FileStreamAsyncResult();
			fileStreamAsyncResult._handle = this._handle;
			fileStreamAsyncResult._userCallback = userCallback;
			fileStreamAsyncResult._userStateObject = stateObject;
			fileStreamAsyncResult._isWrite = false;
			fileStreamAsyncResult._numBufferedBytes = numBufferedBytesRead;
			ManualResetEvent manualResetEvent = new ManualResetEvent(false);
			fileStreamAsyncResult._waitHandle = manualResetEvent;
			Overlapped overlapped = new Overlapped(0, 0, IntPtr.Zero, fileStreamAsyncResult);
			NativeOverlapped* ptr;
			if (userCallback != null)
			{
				ptr = overlapped.Pack(FileStream.IOCallback, bytes);
			}
			else
			{
				ptr = overlapped.UnsafePack(null, bytes);
			}
			fileStreamAsyncResult._overlapped = ptr;
			if (this.CanSeek)
			{
				long length = this.Length;
				if (this._exposedHandle)
				{
					this.VerifyOSHandlePosition();
				}
				if (this._pos + (long)numBytes > length)
				{
					if (this._pos <= length)
					{
						numBytes = (int)(length - this._pos);
					}
					else
					{
						numBytes = 0;
					}
				}
				ptr->OffsetLow = (int)this._pos;
				ptr->OffsetHigh = (int)(this._pos >> 32);
				this.SeekCore((long)numBytes, SeekOrigin.Current);
			}
			int num = 0;
			int num2 = this.ReadFileNative(this._handle, bytes, offset, numBytes, ptr, out num);
			if (num2 == -1 && numBytes != -1)
			{
				if (num == 109)
				{
					ptr->InternalLow = IntPtr.Zero;
					fileStreamAsyncResult.CallUserCallback();
				}
				else if (num != 997)
				{
					if (!this._handle.IsClosed && this.CanSeek)
					{
						this.SeekCore(0L, SeekOrigin.Current);
					}
					if (num == 38)
					{
						__Error.EndOfFile();
					}
					else
					{
						__Error.WinIOError(num, string.Empty);
					}
				}
			}
			return fileStreamAsyncResult;
		}

		// Token: 0x0600360F RID: 13839 RVA: 0x000B5FB8 File Offset: 0x000B4FB8
		public unsafe override int EndRead(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (!this._isAsync)
			{
				return base.EndRead(asyncResult);
			}
			FileStreamAsyncResult fileStreamAsyncResult = asyncResult as FileStreamAsyncResult;
			if (fileStreamAsyncResult == null || fileStreamAsyncResult._isWrite)
			{
				__Error.WrongAsyncResult();
			}
			if (1 == Interlocked.CompareExchange(ref fileStreamAsyncResult._EndXxxCalled, 1, 0))
			{
				__Error.EndReadCalledTwice();
			}
			WaitHandle waitHandle = fileStreamAsyncResult._waitHandle;
			if (waitHandle != null)
			{
				try
				{
					waitHandle.WaitOne();
				}
				finally
				{
					waitHandle.Close();
				}
			}
			NativeOverlapped* overlapped = fileStreamAsyncResult._overlapped;
			if (overlapped != null)
			{
				Overlapped.Free(overlapped);
			}
			if (fileStreamAsyncResult._errorCode != 0)
			{
				__Error.WinIOError(fileStreamAsyncResult._errorCode, Path.GetFileName(this._fileName));
			}
			return fileStreamAsyncResult._numBytes + fileStreamAsyncResult._numBufferedBytes;
		}

		// Token: 0x06003610 RID: 13840 RVA: 0x000B6078 File Offset: 0x000B5078
		public override int ReadByte()
		{
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			if (this._readLen == 0 && !this.CanRead)
			{
				__Error.ReadNotSupported();
			}
			if (this._readPos == this._readLen)
			{
				if (this._writePos > 0)
				{
					this.FlushWrite(false);
				}
				if (this._buffer == null)
				{
					this._buffer = new byte[this._bufferSize];
				}
				this._readLen = this.ReadCore(this._buffer, 0, this._bufferSize);
				this._readPos = 0;
			}
			if (this._readPos == this._readLen)
			{
				return -1;
			}
			int num = (int)this._buffer[this._readPos];
			this._readPos++;
			return num;
		}

		// Token: 0x06003611 RID: 13841 RVA: 0x000B6130 File Offset: 0x000B5130
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginWrite(byte[] array, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (numBytes < 0)
			{
				throw new ArgumentOutOfRangeException("numBytes", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (array.Length - offset < numBytes)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			if (!this._isAsync)
			{
				return base.BeginWrite(array, offset, numBytes, userCallback, stateObject);
			}
			if (!this.CanWrite)
			{
				__Error.WriteNotSupported();
			}
			if (this._isPipe)
			{
				if (this._writePos > 0)
				{
					this.FlushWrite(false);
				}
				return this.BeginWriteCore(array, offset, numBytes, userCallback, stateObject);
			}
			if (this._writePos == 0)
			{
				if (this._readPos < this._readLen)
				{
					this.FlushRead();
				}
				this._readPos = 0;
				this._readLen = 0;
			}
			int num = this._bufferSize - this._writePos;
			if (numBytes <= num)
			{
				if (this._writePos == 0)
				{
					this._buffer = new byte[this._bufferSize];
				}
				Buffer.InternalBlockCopy(array, offset, this._buffer, this._writePos, numBytes);
				this._writePos += numBytes;
				FileStreamAsyncResult fileStreamAsyncResult = new FileStreamAsyncResult();
				fileStreamAsyncResult._userCallback = userCallback;
				fileStreamAsyncResult._userStateObject = stateObject;
				fileStreamAsyncResult._waitHandle = null;
				fileStreamAsyncResult._isWrite = true;
				fileStreamAsyncResult._numBufferedBytes = numBytes;
				fileStreamAsyncResult.CallUserCallback();
				return fileStreamAsyncResult;
			}
			if (this._writePos > 0)
			{
				this.FlushWrite(false);
			}
			return this.BeginWriteCore(array, offset, numBytes, userCallback, stateObject);
		}

		// Token: 0x06003612 RID: 13842 RVA: 0x000B62B8 File Offset: 0x000B52B8
		private unsafe FileStreamAsyncResult BeginWriteCore(byte[] bytes, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
		{
			FileStreamAsyncResult fileStreamAsyncResult = new FileStreamAsyncResult();
			fileStreamAsyncResult._handle = this._handle;
			fileStreamAsyncResult._userCallback = userCallback;
			fileStreamAsyncResult._userStateObject = stateObject;
			fileStreamAsyncResult._isWrite = true;
			ManualResetEvent manualResetEvent = new ManualResetEvent(false);
			fileStreamAsyncResult._waitHandle = manualResetEvent;
			Overlapped overlapped = new Overlapped(0, 0, IntPtr.Zero, fileStreamAsyncResult);
			NativeOverlapped* ptr;
			if (userCallback != null)
			{
				ptr = overlapped.Pack(FileStream.IOCallback, bytes);
			}
			else
			{
				ptr = overlapped.UnsafePack(null, bytes);
			}
			fileStreamAsyncResult._overlapped = ptr;
			if (this.CanSeek)
			{
				long length = this.Length;
				if (this._exposedHandle)
				{
					this.VerifyOSHandlePosition();
				}
				if (this._pos + (long)numBytes > length)
				{
					this.SetLengthCore(this._pos + (long)numBytes);
				}
				ptr->OffsetLow = (int)this._pos;
				ptr->OffsetHigh = (int)(this._pos >> 32);
				this.SeekCore((long)numBytes, SeekOrigin.Current);
			}
			int num = 0;
			int num2 = this.WriteFileNative(this._handle, bytes, offset, numBytes, ptr, out num);
			if (num2 == -1 && numBytes != -1)
			{
				if (num == 232)
				{
					fileStreamAsyncResult.CallUserCallback();
				}
				else if (num != 997)
				{
					if (!this._handle.IsClosed && this.CanSeek)
					{
						this.SeekCore(0L, SeekOrigin.Current);
					}
					if (num == 38)
					{
						__Error.EndOfFile();
					}
					else
					{
						__Error.WinIOError(num, string.Empty);
					}
				}
			}
			return fileStreamAsyncResult;
		}

		// Token: 0x06003613 RID: 13843 RVA: 0x000B6404 File Offset: 0x000B5404
		public unsafe override void EndWrite(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (!this._isAsync)
			{
				base.EndWrite(asyncResult);
				return;
			}
			FileStreamAsyncResult fileStreamAsyncResult = asyncResult as FileStreamAsyncResult;
			if (fileStreamAsyncResult == null || !fileStreamAsyncResult._isWrite)
			{
				__Error.WrongAsyncResult();
			}
			if (1 == Interlocked.CompareExchange(ref fileStreamAsyncResult._EndXxxCalled, 1, 0))
			{
				__Error.EndWriteCalledTwice();
			}
			WaitHandle waitHandle = fileStreamAsyncResult._waitHandle;
			if (waitHandle != null)
			{
				try
				{
					waitHandle.WaitOne();
				}
				finally
				{
					waitHandle.Close();
				}
			}
			NativeOverlapped* overlapped = fileStreamAsyncResult._overlapped;
			if (overlapped != null)
			{
				Overlapped.Free(overlapped);
			}
			if (fileStreamAsyncResult._errorCode != 0)
			{
				__Error.WinIOError(fileStreamAsyncResult._errorCode, Path.GetFileName(this._fileName));
			}
		}

		// Token: 0x06003614 RID: 13844 RVA: 0x000B64B8 File Offset: 0x000B54B8
		public override void WriteByte(byte value)
		{
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			if (this._writePos == 0)
			{
				if (!this.CanWrite)
				{
					__Error.WriteNotSupported();
				}
				if (this._readPos < this._readLen)
				{
					this.FlushRead();
				}
				this._readPos = 0;
				this._readLen = 0;
				if (this._buffer == null)
				{
					this._buffer = new byte[this._bufferSize];
				}
			}
			if (this._writePos == this._bufferSize)
			{
				this.FlushWrite(false);
			}
			this._buffer[this._writePos] = value;
			this._writePos++;
		}

		// Token: 0x06003615 RID: 13845 RVA: 0x000B6558 File Offset: 0x000B5558
		public virtual void Lock(long position, long length)
		{
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			if (position < 0L || length < 0L)
			{
				throw new ArgumentOutOfRangeException((position < 0L) ? "position" : "length", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			int num = (int)position;
			int num2 = (int)(position >> 32);
			int num3 = (int)length;
			int num4 = (int)(length >> 32);
			if (!Win32Native.LockFile(this._handle, num, num2, num3, num4))
			{
				__Error.WinIOError();
			}
		}

		// Token: 0x06003616 RID: 13846 RVA: 0x000B65CC File Offset: 0x000B55CC
		public virtual void Unlock(long position, long length)
		{
			if (this._handle.IsClosed)
			{
				__Error.FileNotOpen();
			}
			if (position < 0L || length < 0L)
			{
				throw new ArgumentOutOfRangeException((position < 0L) ? "position" : "length", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			int num = (int)position;
			int num2 = (int)(position >> 32);
			int num3 = (int)length;
			int num4 = (int)(length >> 32);
			if (!Win32Native.UnlockFile(this._handle, num, num2, num3, num4))
			{
				__Error.WinIOError();
			}
		}

		// Token: 0x06003617 RID: 13847 RVA: 0x000B6640 File Offset: 0x000B5640
		private unsafe int ReadFileNative(SafeFileHandle handle, byte[] bytes, int offset, int count, NativeOverlapped* overlapped, out int hr)
		{
			if (bytes.Length - offset < count)
			{
				throw new IndexOutOfRangeException(Environment.GetResourceString("IndexOutOfRange_IORaceCondition"));
			}
			if (bytes.Length == 0)
			{
				hr = 0;
				return 0;
			}
			int num = 0;
			int num2;
			fixed (byte* ptr = bytes)
			{
				if (this._isAsync)
				{
					num2 = Win32Native.ReadFile(handle, ptr + offset, count, IntPtr.Zero, overlapped);
				}
				else
				{
					num2 = Win32Native.ReadFile(handle, ptr + offset, count, out num, IntPtr.Zero);
				}
			}
			if (num2 != 0)
			{
				hr = 0;
				return num;
			}
			hr = Marshal.GetLastWin32Error();
			if (hr == 109 || hr == 233)
			{
				return -1;
			}
			if (hr == 6 && !this._handle.IsInvalid)
			{
				this._handle.Dispose();
			}
			return -1;
		}

		// Token: 0x06003618 RID: 13848 RVA: 0x000B6704 File Offset: 0x000B5704
		private unsafe int WriteFileNative(SafeFileHandle handle, byte[] bytes, int offset, int count, NativeOverlapped* overlapped, out int hr)
		{
			if (bytes.Length - offset < count)
			{
				throw new IndexOutOfRangeException(Environment.GetResourceString("IndexOutOfRange_IORaceCondition"));
			}
			if (bytes.Length == 0)
			{
				hr = 0;
				return 0;
			}
			int num = 0;
			int num2;
			fixed (byte* ptr = bytes)
			{
				if (this._isAsync)
				{
					num2 = Win32Native.WriteFile(handle, ptr + offset, count, IntPtr.Zero, overlapped);
				}
				else
				{
					num2 = Win32Native.WriteFile(handle, ptr + offset, count, out num, IntPtr.Zero);
				}
			}
			if (num2 != 0)
			{
				hr = 0;
				return num;
			}
			hr = Marshal.GetLastWin32Error();
			if (hr == 232)
			{
				return -1;
			}
			if (hr == 6 && !this._handle.IsInvalid)
			{
				this._handle.Dispose();
			}
			return -1;
		}

		// Token: 0x04001C0C RID: 7180
		internal const int DefaultBufferSize = 4096;

		// Token: 0x04001C0D RID: 7181
		private const int FILE_ATTRIBUTE_NORMAL = 128;

		// Token: 0x04001C0E RID: 7182
		private const int FILE_ATTRIBUTE_ENCRYPTED = 16384;

		// Token: 0x04001C0F RID: 7183
		private const int FILE_FLAG_OVERLAPPED = 1073741824;

		// Token: 0x04001C10 RID: 7184
		internal const int GENERIC_READ = -2147483648;

		// Token: 0x04001C11 RID: 7185
		private const int GENERIC_WRITE = 1073741824;

		// Token: 0x04001C12 RID: 7186
		private const int FILE_BEGIN = 0;

		// Token: 0x04001C13 RID: 7187
		private const int FILE_CURRENT = 1;

		// Token: 0x04001C14 RID: 7188
		private const int FILE_END = 2;

		// Token: 0x04001C15 RID: 7189
		private const int ERROR_BROKEN_PIPE = 109;

		// Token: 0x04001C16 RID: 7190
		private const int ERROR_NO_DATA = 232;

		// Token: 0x04001C17 RID: 7191
		private const int ERROR_HANDLE_EOF = 38;

		// Token: 0x04001C18 RID: 7192
		private const int ERROR_INVALID_PARAMETER = 87;

		// Token: 0x04001C19 RID: 7193
		private const int ERROR_IO_PENDING = 997;

		// Token: 0x04001C1A RID: 7194
		private static readonly bool _canUseAsync = Environment.RunningOnWinNT;

		// Token: 0x04001C1B RID: 7195
		private static readonly IOCompletionCallback IOCallback = new IOCompletionCallback(FileStream.AsyncFSCallback);

		// Token: 0x04001C1C RID: 7196
		private byte[] _buffer;

		// Token: 0x04001C1D RID: 7197
		private string _fileName;

		// Token: 0x04001C1E RID: 7198
		private bool _isAsync;

		// Token: 0x04001C1F RID: 7199
		private bool _canRead;

		// Token: 0x04001C20 RID: 7200
		private bool _canWrite;

		// Token: 0x04001C21 RID: 7201
		private bool _canSeek;

		// Token: 0x04001C22 RID: 7202
		private bool _exposedHandle;

		// Token: 0x04001C23 RID: 7203
		private bool _isPipe;

		// Token: 0x04001C24 RID: 7204
		private int _readPos;

		// Token: 0x04001C25 RID: 7205
		private int _readLen;

		// Token: 0x04001C26 RID: 7206
		private int _writePos;

		// Token: 0x04001C27 RID: 7207
		private int _bufferSize;

		// Token: 0x04001C28 RID: 7208
		private SafeFileHandle _handle;

		// Token: 0x04001C29 RID: 7209
		private long _pos;

		// Token: 0x04001C2A RID: 7210
		private long _appendStart;
	}
}
