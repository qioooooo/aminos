using System;
using System.Threading;

namespace System.Net
{
	// Token: 0x020004E5 RID: 1253
	internal class ListenerAsyncResult : LazyAsyncResult
	{
		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x060026F0 RID: 9968 RVA: 0x000A0CAB File Offset: 0x0009FCAB
		internal static IOCompletionCallback IOCallback
		{
			get
			{
				return ListenerAsyncResult.s_IOCallback;
			}
		}

		// Token: 0x060026F1 RID: 9969 RVA: 0x000A0CB2 File Offset: 0x0009FCB2
		internal ListenerAsyncResult(object asyncObject, object userState, AsyncCallback callback)
			: base(asyncObject, userState, callback)
		{
			this.m_RequestContext = new AsyncRequestContext(this);
		}

		// Token: 0x060026F2 RID: 9970 RVA: 0x000A0CCC File Offset: 0x0009FCCC
		private unsafe static void WaitCallback(uint errorCode, uint numBytes, NativeOverlapped* nativeOverlapped)
		{
			Overlapped overlapped = Overlapped.Unpack(nativeOverlapped);
			ListenerAsyncResult listenerAsyncResult = (ListenerAsyncResult)overlapped.AsyncResult;
			object obj = null;
			try
			{
				if (errorCode != 0U && errorCode != 234U)
				{
					listenerAsyncResult.ErrorCode = (int)errorCode;
					obj = new HttpListenerException((int)errorCode);
				}
				else
				{
					HttpListener httpListener = listenerAsyncResult.AsyncObject as HttpListener;
					if (errorCode == 0U)
					{
						bool flag = false;
						try
						{
							obj = httpListener.HandleAuthentication(listenerAsyncResult.m_RequestContext, out flag);
							goto IL_0099;
						}
						finally
						{
							if (flag)
							{
								listenerAsyncResult.m_RequestContext = ((obj == null) ? new AsyncRequestContext(listenerAsyncResult) : null);
							}
							else
							{
								listenerAsyncResult.m_RequestContext.Reset(0UL, 0U);
							}
						}
					}
					listenerAsyncResult.m_RequestContext.Reset(listenerAsyncResult.m_RequestContext.RequestBlob->RequestId, numBytes);
					IL_0099:
					if (obj == null)
					{
						uint num = listenerAsyncResult.QueueBeginGetContext();
						if (num != 0U && num != 997U)
						{
							obj = new HttpListenerException((int)num);
						}
					}
					if (obj == null)
					{
						return;
					}
				}
			}
			catch (Exception ex)
			{
				if (NclUtilities.IsFatal(ex))
				{
					throw;
				}
				obj = ex;
			}
			listenerAsyncResult.InvokeCallback(obj);
		}

		// Token: 0x060026F3 RID: 9971 RVA: 0x000A0DD0 File Offset: 0x0009FDD0
		internal unsafe uint QueueBeginGetContext()
		{
			uint num2;
			for (;;)
			{
				(base.AsyncObject as HttpListener).EnsureBoundHandle();
				uint num = 0U;
				num2 = UnsafeNclNativeMethods.HttpApi.HttpReceiveHttpRequest((base.AsyncObject as HttpListener).RequestQueueHandle, this.m_RequestContext.RequestBlob->RequestId, 1U, this.m_RequestContext.RequestBlob, this.m_RequestContext.Size, &num, this.m_RequestContext.NativeOverlapped);
				if (num2 == 87U && this.m_RequestContext.RequestBlob->RequestId != 0UL)
				{
					this.m_RequestContext.RequestBlob->RequestId = 0UL;
				}
				else
				{
					if (num2 != 234U)
					{
						break;
					}
					this.m_RequestContext.Reset(this.m_RequestContext.RequestBlob->RequestId, num);
				}
			}
			return num2;
		}

		// Token: 0x060026F4 RID: 9972 RVA: 0x000A0E96 File Offset: 0x0009FE96
		protected override void Cleanup()
		{
			if (this.m_RequestContext != null)
			{
				this.m_RequestContext.ReleasePins();
				this.m_RequestContext.Close();
			}
			base.Cleanup();
		}

		// Token: 0x04002692 RID: 9874
		private static readonly IOCompletionCallback s_IOCallback = new IOCompletionCallback(ListenerAsyncResult.WaitCallback);

		// Token: 0x04002693 RID: 9875
		private AsyncRequestContext m_RequestContext;
	}
}
