using System;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.IO
{
	// Token: 0x020005A6 RID: 1446
	internal sealed class FileStreamAsyncResult : IAsyncResult
	{
		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x060035D4 RID: 13780 RVA: 0x000B449C File Offset: 0x000B349C
		public object AsyncState
		{
			get
			{
				return this._userStateObject;
			}
		}

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x060035D5 RID: 13781 RVA: 0x000B44A4 File Offset: 0x000B34A4
		public bool IsCompleted
		{
			get
			{
				return this._isComplete;
			}
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x060035D6 RID: 13782 RVA: 0x000B44AC File Offset: 0x000B34AC
		public unsafe WaitHandle AsyncWaitHandle
		{
			get
			{
				if (this._waitHandle == null)
				{
					ManualResetEvent manualResetEvent = new ManualResetEvent(false);
					if (this._overlapped != null && this._overlapped->EventHandle != IntPtr.Zero)
					{
						manualResetEvent.SafeWaitHandle = new SafeWaitHandle(this._overlapped->EventHandle, true);
					}
					if (this._isComplete)
					{
						manualResetEvent.Set();
					}
					this._waitHandle = manualResetEvent;
				}
				return this._waitHandle;
			}
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x060035D7 RID: 13783 RVA: 0x000B451C File Offset: 0x000B351C
		public bool CompletedSynchronously
		{
			get
			{
				return this._completedSynchronously;
			}
		}

		// Token: 0x060035D8 RID: 13784 RVA: 0x000B4524 File Offset: 0x000B3524
		internal static FileStreamAsyncResult CreateBufferedReadResult(int numBufferedBytes, AsyncCallback userCallback, object userStateObject)
		{
			return new FileStreamAsyncResult
			{
				_userCallback = userCallback,
				_userStateObject = userStateObject,
				_isWrite = false,
				_numBufferedBytes = numBufferedBytes
			};
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x000B4554 File Offset: 0x000B3554
		private void CallUserCallbackWorker(object callbackState)
		{
			this._isComplete = true;
			if (this._waitHandle != null)
			{
				this._waitHandle.Set();
			}
			this._userCallback(this);
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x000B457D File Offset: 0x000B357D
		internal void CallUserCallback()
		{
			if (this._userCallback != null)
			{
				this._completedSynchronously = false;
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.CallUserCallbackWorker));
				return;
			}
			this._isComplete = true;
			if (this._waitHandle != null)
			{
				this._waitHandle.Set();
			}
		}

		// Token: 0x04001C00 RID: 7168
		internal AsyncCallback _userCallback;

		// Token: 0x04001C01 RID: 7169
		internal object _userStateObject;

		// Token: 0x04001C02 RID: 7170
		internal ManualResetEvent _waitHandle;

		// Token: 0x04001C03 RID: 7171
		internal SafeFileHandle _handle;

		// Token: 0x04001C04 RID: 7172
		internal unsafe NativeOverlapped* _overlapped;

		// Token: 0x04001C05 RID: 7173
		internal int _EndXxxCalled;

		// Token: 0x04001C06 RID: 7174
		internal int _numBytes;

		// Token: 0x04001C07 RID: 7175
		internal int _errorCode;

		// Token: 0x04001C08 RID: 7176
		internal int _numBufferedBytes;

		// Token: 0x04001C09 RID: 7177
		internal bool _isWrite;

		// Token: 0x04001C0A RID: 7178
		internal bool _isComplete;

		// Token: 0x04001C0B RID: 7179
		internal bool _completedSynchronously;
	}
}
