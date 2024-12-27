using System;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.IO
{
	// Token: 0x0200058D RID: 1421
	internal sealed class __ConsoleStream : Stream
	{
		// Token: 0x06003471 RID: 13425 RVA: 0x000AE148 File Offset: 0x000AD148
		internal __ConsoleStream(SafeFileHandle handle, FileAccess access)
		{
			this._handle = handle;
			this._canRead = access == FileAccess.Read;
			this._canWrite = access == FileAccess.Write;
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06003472 RID: 13426 RVA: 0x000AE16B File Offset: 0x000AD16B
		public override bool CanRead
		{
			get
			{
				return this._canRead;
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06003473 RID: 13427 RVA: 0x000AE173 File Offset: 0x000AD173
		public override bool CanWrite
		{
			get
			{
				return this._canWrite;
			}
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06003474 RID: 13428 RVA: 0x000AE17B File Offset: 0x000AD17B
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x06003475 RID: 13429 RVA: 0x000AE17E File Offset: 0x000AD17E
		public override long Length
		{
			get
			{
				__Error.SeekNotSupported();
				return 0L;
			}
		}

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x06003476 RID: 13430 RVA: 0x000AE187 File Offset: 0x000AD187
		// (set) Token: 0x06003477 RID: 13431 RVA: 0x000AE190 File Offset: 0x000AD190
		public override long Position
		{
			get
			{
				__Error.SeekNotSupported();
				return 0L;
			}
			set
			{
				__Error.SeekNotSupported();
			}
		}

		// Token: 0x06003478 RID: 13432 RVA: 0x000AE197 File Offset: 0x000AD197
		protected override void Dispose(bool disposing)
		{
			if (this._handle != null)
			{
				this._handle = null;
			}
			this._canRead = false;
			this._canWrite = false;
			base.Dispose(disposing);
		}

		// Token: 0x06003479 RID: 13433 RVA: 0x000AE1BD File Offset: 0x000AD1BD
		public override void Flush()
		{
			if (this._handle == null)
			{
				__Error.FileNotOpen();
			}
			if (!this.CanWrite)
			{
				__Error.WriteNotSupported();
			}
		}

		// Token: 0x0600347A RID: 13434 RVA: 0x000AE1D9 File Offset: 0x000AD1D9
		public override void SetLength(long value)
		{
			__Error.SeekNotSupported();
		}

		// Token: 0x0600347B RID: 13435 RVA: 0x000AE1E0 File Offset: 0x000AD1E0
		public override int Read([In] [Out] byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((offset < 0) ? "offset" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (!this._canRead)
			{
				__Error.ReadNotSupported();
			}
			int num = 0;
			int num2 = __ConsoleStream.ReadFileNative(this._handle, buffer, offset, count, 0, out num);
			if (num2 == -1)
			{
				__Error.WinIOError(num, string.Empty);
			}
			return num2;
		}

		// Token: 0x0600347C RID: 13436 RVA: 0x000AE26C File Offset: 0x000AD26C
		public override long Seek(long offset, SeekOrigin origin)
		{
			__Error.SeekNotSupported();
			return 0L;
		}

		// Token: 0x0600347D RID: 13437 RVA: 0x000AE278 File Offset: 0x000AD278
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((offset < 0) ? "offset" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (!this._canWrite)
			{
				__Error.WriteNotSupported();
			}
			int num = 0;
			int num2 = __ConsoleStream.WriteFileNative(this._handle, buffer, offset, count, 0, out num);
			if (num2 == -1)
			{
				__Error.WinIOError(num, string.Empty);
			}
		}

		// Token: 0x0600347E RID: 13438 RVA: 0x000AE304 File Offset: 0x000AD304
		private unsafe static int ReadFileNative(SafeFileHandle hFile, byte[] bytes, int offset, int count, int mustBeZero, out int errorCode)
		{
			if (bytes.Length - offset < count)
			{
				throw new IndexOutOfRangeException(Environment.GetResourceString("IndexOutOfRange_IORaceCondition"));
			}
			if (bytes.Length == 0)
			{
				errorCode = 0;
				return 0;
			}
			int num2;
			int num;
			fixed (byte* ptr = bytes)
			{
				num = __ConsoleStream.ReadFile(hFile, ptr + offset, count, out num2, Win32Native.NULL);
			}
			if (num != 0)
			{
				errorCode = 0;
				return num2;
			}
			errorCode = Marshal.GetLastWin32Error();
			if (errorCode == 109)
			{
				return 0;
			}
			return -1;
		}

		// Token: 0x0600347F RID: 13439 RVA: 0x000AE37C File Offset: 0x000AD37C
		private unsafe static int WriteFileNative(SafeFileHandle hFile, byte[] bytes, int offset, int count, int mustBeZero, out int errorCode)
		{
			if (bytes.Length == 0)
			{
				errorCode = 0;
				return 0;
			}
			int num = 0;
			int num2;
			fixed (byte* ptr = bytes)
			{
				num2 = __ConsoleStream.WriteFile(hFile, ptr + offset, count, out num, Win32Native.NULL);
			}
			if (num2 != 0)
			{
				errorCode = 0;
				return num;
			}
			errorCode = Marshal.GetLastWin32Error();
			if (errorCode == 232 || errorCode == 109)
			{
				return 0;
			}
			return -1;
		}

		// Token: 0x06003480 RID: 13440
		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", SetLastError = true)]
		private unsafe static extern int ReadFile(SafeFileHandle handle, byte* bytes, int numBytesToRead, out int numBytesRead, IntPtr mustBeZero);

		// Token: 0x06003481 RID: 13441
		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", SetLastError = true)]
		internal unsafe static extern int WriteFile(SafeFileHandle handle, byte* bytes, int numBytesToWrite, out int numBytesWritten, IntPtr mustBeZero);

		// Token: 0x04001B95 RID: 7061
		internal const int DefaultBufferSize = 128;

		// Token: 0x04001B96 RID: 7062
		private const int ERROR_BROKEN_PIPE = 109;

		// Token: 0x04001B97 RID: 7063
		private const int ERROR_NO_DATA = 232;

		// Token: 0x04001B98 RID: 7064
		private SafeFileHandle _handle;

		// Token: 0x04001B99 RID: 7065
		private bool _canRead;

		// Token: 0x04001B9A RID: 7066
		private bool _canWrite;
	}
}
