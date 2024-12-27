using System;
using System.Threading;

namespace System.Runtime.Remoting.Channels.Ipc
{
	// Token: 0x0200005A RID: 90
	internal class PipeAsyncResult : IAsyncResult
	{
		// Token: 0x060002DE RID: 734 RVA: 0x0000E348 File Offset: 0x0000D348
		internal PipeAsyncResult(AsyncCallback callback)
		{
			this._userCallback = callback;
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060002DF RID: 735 RVA: 0x0000E357 File Offset: 0x0000D357
		public bool IsCompleted
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000E35E File Offset: 0x0000D35E
		public WaitHandle AsyncWaitHandle
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x0000E365 File Offset: 0x0000D365
		public object AsyncState
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000E36C File Offset: 0x0000D36C
		public bool CompletedSynchronously
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000E36F File Offset: 0x0000D36F
		internal void CallUserCallback()
		{
			ThreadPool.QueueUserWorkItem(new WaitCallback(this.CallUserCallbackWorker));
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000E383 File Offset: 0x0000D383
		private void CallUserCallbackWorker(object callbackState)
		{
			this._userCallback(this);
		}

		// Token: 0x04000209 RID: 521
		internal unsafe NativeOverlapped* _overlapped;

		// Token: 0x0400020A RID: 522
		internal AsyncCallback _userCallback;

		// Token: 0x0400020B RID: 523
		internal int _numBytes;

		// Token: 0x0400020C RID: 524
		internal int _errorCode;
	}
}
