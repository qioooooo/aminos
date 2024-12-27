using System;
using System.Threading;

namespace System.IO.Compression
{
	// Token: 0x02000203 RID: 515
	internal class DeflateStreamAsyncResult : IAsyncResult
	{
		// Token: 0x06001186 RID: 4486 RVA: 0x000396C0 File Offset: 0x000386C0
		public DeflateStreamAsyncResult(object asyncObject, object asyncState, AsyncCallback asyncCallback, byte[] buffer, int offset, int count)
		{
			this.buffer = buffer;
			this.offset = offset;
			this.count = count;
			this.m_CompletedSynchronously = true;
			this.m_AsyncObject = asyncObject;
			this.m_AsyncState = asyncState;
			this.m_AsyncCallback = asyncCallback;
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06001187 RID: 4487 RVA: 0x000396FC File Offset: 0x000386FC
		public object AsyncState
		{
			get
			{
				return this.m_AsyncState;
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06001188 RID: 4488 RVA: 0x00039704 File Offset: 0x00038704
		public WaitHandle AsyncWaitHandle
		{
			get
			{
				int completed = this.m_Completed;
				if (this.m_Event == null)
				{
					Interlocked.CompareExchange(ref this.m_Event, new ManualResetEvent(completed != 0), null);
				}
				ManualResetEvent manualResetEvent = (ManualResetEvent)this.m_Event;
				if (completed == 0 && this.m_Completed != 0)
				{
					manualResetEvent.Set();
				}
				return manualResetEvent;
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06001189 RID: 4489 RVA: 0x00039758 File Offset: 0x00038758
		public bool CompletedSynchronously
		{
			get
			{
				return this.m_CompletedSynchronously;
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x0600118A RID: 4490 RVA: 0x00039760 File Offset: 0x00038760
		public bool IsCompleted
		{
			get
			{
				return this.m_Completed != 0;
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x0600118B RID: 4491 RVA: 0x0003976E File Offset: 0x0003876E
		internal object Result
		{
			get
			{
				return this.m_Result;
			}
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x00039776 File Offset: 0x00038776
		internal void Close()
		{
			if (this.m_Event != null)
			{
				((ManualResetEvent)this.m_Event).Close();
			}
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x00039790 File Offset: 0x00038790
		internal void InvokeCallback(bool completedSynchronously, object result)
		{
			this.Complete(completedSynchronously, result);
		}

		// Token: 0x0600118E RID: 4494 RVA: 0x0003979A File Offset: 0x0003879A
		internal void InvokeCallback(object result)
		{
			this.Complete(result);
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x000397A3 File Offset: 0x000387A3
		private void Complete(bool completedSynchronously, object result)
		{
			this.m_CompletedSynchronously = completedSynchronously;
			this.Complete(result);
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x000397B4 File Offset: 0x000387B4
		private void Complete(object result)
		{
			this.m_Result = result;
			Interlocked.Increment(ref this.m_Completed);
			if (this.m_Event != null)
			{
				((ManualResetEvent)this.m_Event).Set();
			}
			if (Interlocked.Increment(ref this.m_InvokedCallback) == 1 && this.m_AsyncCallback != null)
			{
				this.m_AsyncCallback(this);
			}
		}

		// Token: 0x04000FCA RID: 4042
		public byte[] buffer;

		// Token: 0x04000FCB RID: 4043
		public int offset;

		// Token: 0x04000FCC RID: 4044
		public int count;

		// Token: 0x04000FCD RID: 4045
		public bool isWrite;

		// Token: 0x04000FCE RID: 4046
		private object m_AsyncObject;

		// Token: 0x04000FCF RID: 4047
		private object m_AsyncState;

		// Token: 0x04000FD0 RID: 4048
		private AsyncCallback m_AsyncCallback;

		// Token: 0x04000FD1 RID: 4049
		private object m_Result;

		// Token: 0x04000FD2 RID: 4050
		internal bool m_CompletedSynchronously;

		// Token: 0x04000FD3 RID: 4051
		private int m_InvokedCallback;

		// Token: 0x04000FD4 RID: 4052
		private int m_Completed;

		// Token: 0x04000FD5 RID: 4053
		private object m_Event;
	}
}
