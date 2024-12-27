using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x02000034 RID: 52
	internal sealed class DirMonCompletion : IDisposable
	{
		// Token: 0x0600010F RID: 271 RVA: 0x00005C10 File Offset: 0x00004C10
		internal DirMonCompletion(DirectoryMonitor dirMon, string dir, bool watchSubtree, uint notifyFilter)
		{
			this._dirMon = dirMon;
			NativeFileChangeNotification nativeFileChangeNotification = new NativeFileChangeNotification(this.OnFileChange);
			this._rootCallback = GCHandle.Alloc(nativeFileChangeNotification);
			int num = UnsafeNativeMethods.DirMonOpen(dir, HttpRuntime.AppDomainAppIdInternal, watchSubtree, notifyFilter, nativeFileChangeNotification, out this._ndirMonCompletionPtr);
			if (num != 0)
			{
				this._rootCallback.Free();
				throw FileChangesMonitor.CreateFileMonitoringException(num, dir);
			}
			this._ndirMonCompletionHandle = new HandleRef(this, this._ndirMonCompletionPtr);
			Interlocked.Increment(ref DirMonCompletion._activeDirMonCompletions);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00005C8C File Offset: 0x00004C8C
		~DirMonCompletion()
		{
			this.Dispose(false);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00005CBC File Offset: 0x00004CBC
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00005CCC File Offset: 0x00004CCC
		private void Dispose(bool disposing)
		{
			HandleRef ndirMonCompletionHandle = this._ndirMonCompletionHandle;
			if (ndirMonCompletionHandle.Handle != IntPtr.Zero)
			{
				this._ndirMonCompletionHandle = new HandleRef(this, IntPtr.Zero);
				UnsafeNativeMethods.DirMonClose(ndirMonCompletionHandle);
				Interlocked.Decrement(ref DirMonCompletion._activeDirMonCompletions);
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00005D18 File Offset: 0x00004D18
		private void OnFileChange(FileAction action, string fileName, long ticks)
		{
			DateTime dateTime;
			if (ticks == 0L)
			{
				dateTime = DateTime.MinValue;
			}
			else
			{
				dateTime = DateTimeUtil.FromFileTimeToUtc(ticks);
			}
			if (action == FileAction.Dispose)
			{
				if (this._rootCallback.IsAllocated)
				{
					this._rootCallback.Free();
					return;
				}
			}
			else if (this._ndirMonCompletionHandle.Handle != IntPtr.Zero)
			{
				using (new ApplicationImpersonationContext())
				{
					this._dirMon.OnFileChange(action, fileName, dateTime);
				}
			}
		}

		// Token: 0x04000DBA RID: 3514
		private static int _activeDirMonCompletions;

		// Token: 0x04000DBB RID: 3515
		private DirectoryMonitor _dirMon;

		// Token: 0x04000DBC RID: 3516
		private IntPtr _ndirMonCompletionPtr;

		// Token: 0x04000DBD RID: 3517
		private HandleRef _ndirMonCompletionHandle;

		// Token: 0x04000DBE RID: 3518
		private GCHandle _rootCallback;
	}
}
