using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Net
{
	// Token: 0x020004E6 RID: 1254
	internal class HttpRequestStream : Stream
	{
		// Token: 0x060026F6 RID: 9974 RVA: 0x000A0ECF File Offset: 0x0009FECF
		internal HttpRequestStream(HttpListenerContext httpContext)
		{
			this.m_HttpContext = httpContext;
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x060026F7 RID: 9975 RVA: 0x000A0EDE File Offset: 0x0009FEDE
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x060026F8 RID: 9976 RVA: 0x000A0EE1 File Offset: 0x0009FEE1
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x060026F9 RID: 9977 RVA: 0x000A0EE4 File Offset: 0x0009FEE4
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060026FA RID: 9978 RVA: 0x000A0EE7 File Offset: 0x0009FEE7
		public override void Flush()
		{
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x060026FB RID: 9979 RVA: 0x000A0EE9 File Offset: 0x0009FEE9
		public override long Length
		{
			get
			{
				throw new NotSupportedException(SR.GetString("net_noseek"));
			}
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x060026FC RID: 9980 RVA: 0x000A0EFA File Offset: 0x0009FEFA
		// (set) Token: 0x060026FD RID: 9981 RVA: 0x000A0F0B File Offset: 0x0009FF0B
		public override long Position
		{
			get
			{
				throw new NotSupportedException(SR.GetString("net_noseek"));
			}
			set
			{
				throw new NotSupportedException(SR.GetString("net_noseek"));
			}
		}

		// Token: 0x060026FE RID: 9982 RVA: 0x000A0F1C File Offset: 0x0009FF1C
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x060026FF RID: 9983 RVA: 0x000A0F2D File Offset: 0x0009FF2D
		public override void SetLength(long value)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06002700 RID: 9984 RVA: 0x000A0F40 File Offset: 0x0009FF40
		public unsafe override int Read([In] [Out] byte[] buffer, int offset, int size)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Read", "");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || size > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			if (size == 0 || this.m_Closed)
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "Read", "dataRead:0");
				}
				return 0;
			}
			uint num = 0U;
			if (this.m_DataChunkIndex != -1)
			{
				num = UnsafeNclNativeMethods.HttpApi.GetChunks(this.m_HttpContext.Request.RequestBuffer, this.m_HttpContext.Request.OriginalBlobAddress, ref this.m_DataChunkIndex, ref this.m_DataChunkOffset, buffer, offset, size);
			}
			if (this.m_DataChunkIndex == -1 && (ulong)num < (ulong)((long)size))
			{
				uint num2 = 0U;
				offset += (int)num;
				size -= (int)num;
				if (size > 131072)
				{
					size = 131072;
				}
				uint num3;
				fixed (byte* ptr = buffer)
				{
					num3 = UnsafeNclNativeMethods.HttpApi.HttpReceiveRequestEntityBody(this.m_HttpContext.RequestQueueHandle, this.m_HttpContext.RequestId, 1U, (void*)((byte*)ptr + offset), (uint)size, &num2, null);
					num += num2;
				}
				if (num3 != 0U && num3 != 38U)
				{
					Exception ex = new HttpListenerException((int)num3);
					if (Logging.On)
					{
						Logging.Exception(Logging.HttpListener, this, "Read", ex);
					}
					throw ex;
				}
				this.UpdateAfterRead(num3, num);
			}
			if (Logging.On)
			{
				Logging.Dump(Logging.HttpListener, this, "Read", buffer, offset, (int)num);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "Read", "dataRead:" + num);
			}
			return (int)num;
		}

		// Token: 0x06002701 RID: 9985 RVA: 0x000A10FC File Offset: 0x000A00FC
		private void UpdateAfterRead(uint statusCode, uint dataRead)
		{
			if (statusCode == 38U || dataRead == 0U)
			{
				this.Close();
			}
		}

		// Token: 0x06002702 RID: 9986 RVA: 0x000A110C File Offset: 0x000A010C
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public unsafe override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "BeginRead", "");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || size > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			if (size == 0 || this.m_Closed)
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "BeginRead", "");
				}
				HttpRequestStream.HttpRequestStreamAsyncResult httpRequestStreamAsyncResult = new HttpRequestStream.HttpRequestStreamAsyncResult(this, state, callback);
				httpRequestStreamAsyncResult.InvokeCallback(0U);
				return httpRequestStreamAsyncResult;
			}
			HttpRequestStream.HttpRequestStreamAsyncResult httpRequestStreamAsyncResult2 = null;
			uint num = 0U;
			if (this.m_DataChunkIndex != -1)
			{
				num = UnsafeNclNativeMethods.HttpApi.GetChunks(this.m_HttpContext.Request.RequestBuffer, this.m_HttpContext.Request.OriginalBlobAddress, ref this.m_DataChunkIndex, ref this.m_DataChunkOffset, buffer, offset, size);
				if (this.m_DataChunkIndex != -1 && (ulong)num == (ulong)((long)size))
				{
					httpRequestStreamAsyncResult2 = new HttpRequestStream.HttpRequestStreamAsyncResult(this, state, callback, buffer, offset, (uint)size, 0U);
					httpRequestStreamAsyncResult2.InvokeCallback(num);
				}
			}
			if (this.m_DataChunkIndex == -1 && (ulong)num < (ulong)((long)size))
			{
				uint num2 = 0U;
				offset += (int)num;
				size -= (int)num;
				if (size > 131072)
				{
					size = 131072;
				}
				httpRequestStreamAsyncResult2 = new HttpRequestStream.HttpRequestStreamAsyncResult(this, state, callback, buffer, offset, (uint)size, num);
				try
				{
					if (buffer != null)
					{
						int num3 = buffer.Length;
					}
					this.m_HttpContext.EnsureBoundHandle();
					num2 = UnsafeNclNativeMethods.HttpApi.HttpReceiveRequestEntityBody(this.m_HttpContext.RequestQueueHandle, this.m_HttpContext.RequestId, 1U, httpRequestStreamAsyncResult2.m_pPinnedBuffer, (uint)size, null, httpRequestStreamAsyncResult2.m_pOverlapped);
				}
				catch (Exception ex)
				{
					if (Logging.On)
					{
						Logging.Exception(Logging.HttpListener, this, "BeginRead", ex);
					}
					httpRequestStreamAsyncResult2.InternalCleanup();
					throw;
				}
				if (num2 != 0U && num2 != 997U)
				{
					if (num2 == 38U)
					{
						httpRequestStreamAsyncResult2.m_pOverlapped->InternalLow = IntPtr.Zero;
					}
					httpRequestStreamAsyncResult2.InternalCleanup();
					if (num2 != 38U)
					{
						Exception ex2 = new HttpListenerException((int)num2);
						if (Logging.On)
						{
							Logging.Exception(Logging.HttpListener, this, "BeginRead", ex2);
						}
						httpRequestStreamAsyncResult2.InternalCleanup();
						throw ex2;
					}
					httpRequestStreamAsyncResult2 = new HttpRequestStream.HttpRequestStreamAsyncResult(this, state, callback, num);
					httpRequestStreamAsyncResult2.InvokeCallback(0U);
				}
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "BeginRead", "");
			}
			return httpRequestStreamAsyncResult2;
		}

		// Token: 0x06002703 RID: 9987 RVA: 0x000A1360 File Offset: 0x000A0360
		public override int EndRead(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "EndRead", "");
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			HttpRequestStream.HttpRequestStreamAsyncResult httpRequestStreamAsyncResult = asyncResult as HttpRequestStream.HttpRequestStreamAsyncResult;
			if (httpRequestStreamAsyncResult == null || httpRequestStreamAsyncResult.AsyncObject != this)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			if (httpRequestStreamAsyncResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndRead" }));
			}
			httpRequestStreamAsyncResult.EndCalled = true;
			object obj = httpRequestStreamAsyncResult.InternalWaitForCompletion();
			Exception ex = obj as Exception;
			if (ex != null)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "EndRead", ex);
				}
				throw ex;
			}
			uint num = (uint)obj;
			this.UpdateAfterRead((uint)httpRequestStreamAsyncResult.ErrorCode, num);
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "EndRead", "");
			}
			return (int)(num + httpRequestStreamAsyncResult.m_dataAlreadyRead);
		}

		// Token: 0x06002704 RID: 9988 RVA: 0x000A1454 File Offset: 0x000A0454
		public override void Write(byte[] buffer, int offset, int size)
		{
			throw new InvalidOperationException(SR.GetString("net_readonlystream"));
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x000A1465 File Offset: 0x000A0465
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			throw new InvalidOperationException(SR.GetString("net_readonlystream"));
		}

		// Token: 0x06002706 RID: 9990 RVA: 0x000A1476 File Offset: 0x000A0476
		public override void EndWrite(IAsyncResult asyncResult)
		{
			throw new InvalidOperationException(SR.GetString("net_readonlystream"));
		}

		// Token: 0x06002707 RID: 9991 RVA: 0x000A1488 File Offset: 0x000A0488
		protected override void Dispose(bool disposing)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Dispose", "");
			}
			try
			{
				this.m_Closed = true;
			}
			finally
			{
				base.Dispose(disposing);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "Dispose", "");
			}
		}

		// Token: 0x04002694 RID: 9876
		private const int MaxReadSize = 131072;

		// Token: 0x04002695 RID: 9877
		private HttpListenerContext m_HttpContext;

		// Token: 0x04002696 RID: 9878
		private uint m_DataChunkOffset;

		// Token: 0x04002697 RID: 9879
		private int m_DataChunkIndex;

		// Token: 0x04002698 RID: 9880
		private bool m_Closed;

		// Token: 0x020004E7 RID: 1255
		private class HttpRequestStreamAsyncResult : LazyAsyncResult
		{
			// Token: 0x06002708 RID: 9992 RVA: 0x000A14F0 File Offset: 0x000A04F0
			internal HttpRequestStreamAsyncResult(object asyncObject, object userState, AsyncCallback callback)
				: base(asyncObject, userState, callback)
			{
			}

			// Token: 0x06002709 RID: 9993 RVA: 0x000A14FB File Offset: 0x000A04FB
			internal HttpRequestStreamAsyncResult(object asyncObject, object userState, AsyncCallback callback, uint dataAlreadyRead)
				: base(asyncObject, userState, callback)
			{
				this.m_dataAlreadyRead = dataAlreadyRead;
			}

			// Token: 0x0600270A RID: 9994 RVA: 0x000A1510 File Offset: 0x000A0510
			internal unsafe HttpRequestStreamAsyncResult(object asyncObject, object userState, AsyncCallback callback, byte[] buffer, int offset, uint size, uint dataAlreadyRead)
				: base(asyncObject, userState, callback)
			{
				this.m_dataAlreadyRead = dataAlreadyRead;
				this.m_pOverlapped = new Overlapped
				{
					AsyncResult = this
				}.Pack(HttpRequestStream.HttpRequestStreamAsyncResult.s_IOCallback, buffer);
				this.m_pPinnedBuffer = (void*)Marshal.UnsafeAddrOfPinnedArrayElement(buffer, offset);
			}

			// Token: 0x0600270B RID: 9995 RVA: 0x000A1564 File Offset: 0x000A0564
			private unsafe static void Callback(uint errorCode, uint numBytes, NativeOverlapped* nativeOverlapped)
			{
				Overlapped overlapped = Overlapped.Unpack(nativeOverlapped);
				HttpRequestStream.HttpRequestStreamAsyncResult httpRequestStreamAsyncResult = overlapped.AsyncResult as HttpRequestStream.HttpRequestStreamAsyncResult;
				object obj = null;
				try
				{
					if (errorCode != 0U && errorCode != 38U)
					{
						httpRequestStreamAsyncResult.ErrorCode = (int)errorCode;
						obj = new HttpListenerException((int)errorCode);
					}
					else
					{
						obj = numBytes;
						if (Logging.On)
						{
							Logging.Dump(Logging.HttpListener, httpRequestStreamAsyncResult, "Callback", (IntPtr)httpRequestStreamAsyncResult.m_pPinnedBuffer, (int)numBytes);
						}
					}
				}
				catch (Exception ex)
				{
					obj = ex;
				}
				httpRequestStreamAsyncResult.InvokeCallback(obj);
			}

			// Token: 0x0600270C RID: 9996 RVA: 0x000A15E8 File Offset: 0x000A05E8
			protected override void Cleanup()
			{
				base.Cleanup();
				if (this.m_pOverlapped != null)
				{
					Overlapped.Free(this.m_pOverlapped);
				}
			}

			// Token: 0x04002699 RID: 9881
			internal unsafe NativeOverlapped* m_pOverlapped;

			// Token: 0x0400269A RID: 9882
			internal unsafe void* m_pPinnedBuffer;

			// Token: 0x0400269B RID: 9883
			internal uint m_dataAlreadyRead;

			// Token: 0x0400269C RID: 9884
			private static readonly IOCompletionCallback s_IOCallback = new IOCompletionCallback(HttpRequestStream.HttpRequestStreamAsyncResult.Callback);
		}
	}
}
