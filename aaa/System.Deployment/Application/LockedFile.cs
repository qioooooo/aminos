using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Deployment.Application
{
	// Token: 0x02000063 RID: 99
	internal static class LockedFile
	{
		// Token: 0x06000309 RID: 777 RVA: 0x000116AC File Offset: 0x000106AC
		public static IDisposable AcquireLock(string path, TimeSpan timeout, bool writer)
		{
			LockedFile.LockedFileHandle lockedFileHandle = LockedFile.LockHeldByThread(path, writer);
			if (lockedFileHandle != null)
			{
				return lockedFileHandle;
			}
			DateTime dateTime = DateTime.UtcNow + timeout;
			FileAccess fileAccess;
			NativeMethods.GenericAccess genericAccess;
			NativeMethods.ShareMode shareMode;
			if (writer)
			{
				fileAccess = FileAccess.Write;
				genericAccess = NativeMethods.GenericAccess.GENERIC_WRITE;
				shareMode = NativeMethods.ShareMode.FILE_SHARE_NONE;
			}
			else
			{
				fileAccess = FileAccess.Read;
				genericAccess = (NativeMethods.GenericAccess)2147483648U;
				shareMode = (PlatformSpecific.OnWin9x ? NativeMethods.ShareMode.FILE_SHARE_READ : (NativeMethods.ShareMode.FILE_SHARE_READ | NativeMethods.ShareMode.FILE_SHARE_DELETE));
			}
			SafeFileHandle safeFileHandle;
			for (;;)
			{
				safeFileHandle = NativeMethods.CreateFile(path, (uint)genericAccess, (uint)shareMode, IntPtr.Zero, 4U, 67108864U, IntPtr.Zero);
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (!safeFileHandle.IsInvalid)
				{
					break;
				}
				if (lastWin32Error != 32 && lastWin32Error != 5)
				{
					Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
				}
				if (DateTime.UtcNow > dateTime)
				{
					goto Block_7;
				}
				Thread.Sleep(1);
			}
			return new LockedFile.LockedFileHandle(safeFileHandle, path, fileAccess);
			Block_7:
			throw new DeploymentException(ExceptionTypes.LockTimeout, Resources.GetString("Ex_LockTimeoutException"));
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00011768 File Offset: 0x00010768
		private static LockedFile.LockedFileHandle LockHeldByThread(string path, bool writer)
		{
			LockedFile.LockedFileHandle lockedFileHandle = (LockedFile.LockedFileHandle)LockedFile.ThreadWriterLocks[path];
			if (lockedFileHandle != null)
			{
				return new LockedFile.LockedFileHandle();
			}
			LockedFile.LockedFileHandle lockedFileHandle2 = (LockedFile.LockedFileHandle)LockedFile.ThreadReaderLocks[path];
			if (lockedFileHandle2 == null)
			{
				return null;
			}
			if (!writer)
			{
				return new LockedFile.LockedFileHandle();
			}
			throw new NotImplementedException();
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600030B RID: 779 RVA: 0x000117B3 File Offset: 0x000107B3
		private static Hashtable ThreadReaderLocks
		{
			get
			{
				if (LockedFile._threadReaderLocks == null)
				{
					LockedFile._threadReaderLocks = new Hashtable();
				}
				return LockedFile._threadReaderLocks;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600030C RID: 780 RVA: 0x000117CB File Offset: 0x000107CB
		private static Hashtable ThreadWriterLocks
		{
			get
			{
				if (LockedFile._threadWriterLocks == null)
				{
					LockedFile._threadWriterLocks = new Hashtable();
				}
				return LockedFile._threadWriterLocks;
			}
		}

		// Token: 0x04000253 RID: 595
		[ThreadStatic]
		private static Hashtable _threadReaderLocks;

		// Token: 0x04000254 RID: 596
		[ThreadStatic]
		private static Hashtable _threadWriterLocks;

		// Token: 0x02000064 RID: 100
		private class LockedFileHandle : IDisposable
		{
			// Token: 0x0600030D RID: 781 RVA: 0x000117E3 File Offset: 0x000107E3
			public LockedFileHandle()
			{
			}

			// Token: 0x0600030E RID: 782 RVA: 0x000117EC File Offset: 0x000107EC
			public LockedFileHandle(SafeFileHandle handle, string path, FileAccess access)
			{
				if (handle == null)
				{
					throw new ArgumentNullException("handle");
				}
				this._handle = handle;
				this._path = path;
				this._access = access;
				Hashtable hashtable = ((this._access == FileAccess.Read) ? LockedFile.ThreadReaderLocks : LockedFile.ThreadWriterLocks);
				hashtable.Add(this._path, this);
			}

			// Token: 0x0600030F RID: 783 RVA: 0x00011848 File Offset: 0x00010848
			public void Dispose()
			{
				if (!this._disposed)
				{
					if (this._handle != null)
					{
						Hashtable hashtable = ((this._access == FileAccess.Read) ? LockedFile.ThreadReaderLocks : LockedFile.ThreadWriterLocks);
						hashtable.Remove(this._path);
						this._handle.Dispose();
					}
					GC.SuppressFinalize(this);
					this._disposed = true;
				}
			}

			// Token: 0x04000255 RID: 597
			private SafeFileHandle _handle;

			// Token: 0x04000256 RID: 598
			private string _path;

			// Token: 0x04000257 RID: 599
			private FileAccess _access;

			// Token: 0x04000258 RID: 600
			private bool _disposed;
		}
	}
}
