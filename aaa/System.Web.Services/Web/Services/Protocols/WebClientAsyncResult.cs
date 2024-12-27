using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Permissions;
using System.Threading;
using System.Web.Services.Diagnostics;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000027 RID: 39
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class WebClientAsyncResult : IAsyncResult
	{
		// Token: 0x060000C3 RID: 195 RVA: 0x00003B91 File Offset: 0x00002B91
		internal WebClientAsyncResult(WebClientProtocol clientProtocol, object internalAsyncState, WebRequest request, AsyncCallback userCallback, object userAsyncState)
		{
			this.ClientProtocol = clientProtocol;
			this.InternalAsyncState = internalAsyncState;
			this.userAsyncState = userAsyncState;
			this.userCallback = userCallback;
			this.Request = request;
			this.completedSynchronously = true;
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00003BC5 File Offset: 0x00002BC5
		public object AsyncState
		{
			get
			{
				return this.userAsyncState;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00003BD0 File Offset: 0x00002BD0
		public WaitHandle AsyncWaitHandle
		{
			get
			{
				bool flag = this.isCompleted;
				if (this.manualResetEvent == null)
				{
					lock (this)
					{
						if (this.manualResetEvent == null)
						{
							this.manualResetEvent = new ManualResetEvent(flag);
						}
					}
				}
				if (!flag && this.isCompleted)
				{
					this.manualResetEvent.Set();
				}
				return this.manualResetEvent;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00003C40 File Offset: 0x00002C40
		public bool CompletedSynchronously
		{
			get
			{
				return this.completedSynchronously;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00003C48 File Offset: 0x00002C48
		public bool IsCompleted
		{
			get
			{
				return this.isCompleted;
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00003C50 File Offset: 0x00002C50
		public void Abort()
		{
			WebRequest request = this.Request;
			if (request != null)
			{
				request.Abort();
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00003C70 File Offset: 0x00002C70
		internal void Complete()
		{
			try
			{
				if (this.ResponseStream != null)
				{
					this.ResponseStream.Close();
					this.ResponseStream = null;
				}
				if (this.ResponseBufferedStream != null)
				{
					this.ResponseBufferedStream.Position = 0L;
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (this.Exception == null)
				{
					this.Exception = ex;
				}
				if (Tracing.On)
				{
					Tracing.ExceptionCatch(TraceEventType.Error, this, "Complete", ex);
				}
			}
			catch
			{
				if (this.Exception == null)
				{
					this.Exception = new Exception(Res.GetString("NonClsCompliantException"));
				}
			}
			this.isCompleted = true;
			try
			{
				if (this.manualResetEvent != null)
				{
					this.manualResetEvent.Set();
				}
			}
			catch (Exception ex2)
			{
				if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
				{
					throw;
				}
				if (this.Exception == null)
				{
					this.Exception = ex2;
				}
				if (Tracing.On)
				{
					Tracing.ExceptionCatch(TraceEventType.Error, this, "Complete", ex2);
				}
			}
			catch
			{
				if (this.Exception == null)
				{
					this.Exception = new Exception(Res.GetString("NonClsCompliantException"));
				}
			}
			if (this.userCallback != null)
			{
				this.userCallback(this);
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00003DD8 File Offset: 0x00002DD8
		internal void Complete(Exception e)
		{
			this.Exception = e;
			this.Complete();
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00003DE7 File Offset: 0x00002DE7
		internal WebResponse WaitForResponse()
		{
			if (!this.isCompleted)
			{
				this.AsyncWaitHandle.WaitOne();
			}
			if (this.Exception != null)
			{
				throw this.Exception;
			}
			return this.Response;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00003E12 File Offset: 0x00002E12
		internal void CombineCompletedSynchronously(bool innerCompletedSynchronously)
		{
			this.completedSynchronously = this.completedSynchronously && innerCompletedSynchronously;
		}

		// Token: 0x0400024C RID: 588
		private object userAsyncState;

		// Token: 0x0400024D RID: 589
		private bool completedSynchronously;

		// Token: 0x0400024E RID: 590
		private bool isCompleted;

		// Token: 0x0400024F RID: 591
		private ManualResetEvent manualResetEvent;

		// Token: 0x04000250 RID: 592
		private AsyncCallback userCallback;

		// Token: 0x04000251 RID: 593
		internal WebClientProtocol ClientProtocol;

		// Token: 0x04000252 RID: 594
		internal object InternalAsyncState;

		// Token: 0x04000253 RID: 595
		internal Exception Exception;

		// Token: 0x04000254 RID: 596
		internal WebResponse Response;

		// Token: 0x04000255 RID: 597
		internal WebRequest Request;

		// Token: 0x04000256 RID: 598
		internal Stream ResponseStream;

		// Token: 0x04000257 RID: 599
		internal Stream ResponseBufferedStream;

		// Token: 0x04000258 RID: 600
		internal byte[] Buffer;

		// Token: 0x04000259 RID: 601
		internal bool EndSendCalled;
	}
}
