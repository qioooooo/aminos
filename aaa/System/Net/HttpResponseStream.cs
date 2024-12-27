using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x020004E8 RID: 1256
	internal class HttpResponseStream : Stream
	{
		// Token: 0x0600270E RID: 9998 RVA: 0x000A1618 File Offset: 0x000A0618
		internal HttpResponseStream(HttpListenerContext httpContext)
		{
			this.m_HttpContext = httpContext;
		}

		// Token: 0x0600270F RID: 9999 RVA: 0x000A1638 File Offset: 0x000A0638
		internal UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS ComputeLeftToWrite()
		{
			UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS http_FLAGS = UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.NONE;
			if (!this.m_HttpContext.Response.ComputedHeaders)
			{
				http_FLAGS = this.m_HttpContext.Response.ComputeHeaders();
			}
			if (this.m_LeftToWrite == -9223372036854775808L)
			{
				UnsafeNclNativeMethods.HttpApi.HTTP_VERB knownMethod = this.m_HttpContext.GetKnownMethod();
				this.m_LeftToWrite = ((knownMethod != UnsafeNclNativeMethods.HttpApi.HTTP_VERB.HttpVerbHEAD) ? this.m_HttpContext.Response.ContentLength64 : 0L);
				if (this.m_LeftToWrite == 0L)
				{
					this.Close();
				}
				else if (knownMethod == UnsafeNclNativeMethods.HttpApi.HTTP_VERB.HttpVerbOPTIONS && this.m_LeftToWrite > 0L)
				{
					throw new ProtocolViolationException(SR.GetString("net_nouploadonget"));
				}
			}
			return http_FLAGS;
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x06002710 RID: 10000 RVA: 0x000A16D6 File Offset: 0x000A06D6
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x06002711 RID: 10001 RVA: 0x000A16D9 File Offset: 0x000A06D9
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x06002712 RID: 10002 RVA: 0x000A16DC File Offset: 0x000A06DC
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x000A16DF File Offset: 0x000A06DF
		public override void Flush()
		{
		}

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x06002714 RID: 10004 RVA: 0x000A16E1 File Offset: 0x000A06E1
		public override long Length
		{
			get
			{
				throw new NotSupportedException(SR.GetString("net_noseek"));
			}
		}

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06002715 RID: 10005 RVA: 0x000A16F2 File Offset: 0x000A06F2
		// (set) Token: 0x06002716 RID: 10006 RVA: 0x000A1703 File Offset: 0x000A0703
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

		// Token: 0x06002717 RID: 10007 RVA: 0x000A1714 File Offset: 0x000A0714
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x000A1725 File Offset: 0x000A0725
		public override void SetLength(long value)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06002719 RID: 10009 RVA: 0x000A1736 File Offset: 0x000A0736
		public override int Read([In] [Out] byte[] buffer, int offset, int size)
		{
			throw new InvalidOperationException(SR.GetString("net_writeonlystream"));
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x000A1747 File Offset: 0x000A0747
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			throw new InvalidOperationException(SR.GetString("net_writeonlystream"));
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x000A1758 File Offset: 0x000A0758
		public override int EndRead(IAsyncResult asyncResult)
		{
			throw new InvalidOperationException(SR.GetString("net_writeonlystream"));
		}

		// Token: 0x0600271C RID: 10012 RVA: 0x000A176C File Offset: 0x000A076C
		public unsafe override void Write(byte[] buffer, int offset, int size)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Write", "");
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
			UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS http_FLAGS = this.ComputeLeftToWrite();
			if (size == 0 || this.m_Closed)
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "Write", "");
				}
				return;
			}
			if (this.m_LeftToWrite > 0L && (long)size > this.m_LeftToWrite)
			{
				throw new ProtocolViolationException(SR.GetString("net_entitytoobig"));
			}
			uint num = (uint)size;
			SafeLocalFree safeLocalFree = null;
			IntPtr intPtr = IntPtr.Zero;
			bool sentHeaders = this.m_HttpContext.Response.SentHeaders;
			uint num2;
			try
			{
				try
				{
					fixed (byte* ptr = buffer)
					{
						byte* ptr2 = ptr;
						if (this.m_HttpContext.Response.BoundaryType == BoundaryType.Chunked)
						{
							string text = size.ToString("x", CultureInfo.InvariantCulture);
							num += (uint)(text.Length + 4);
							safeLocalFree = SafeLocalFree.LocalAlloc((int)num);
							intPtr = safeLocalFree.DangerousGetHandle();
							for (int i = 0; i < text.Length; i++)
							{
								Marshal.WriteByte(intPtr, i, (byte)text[i]);
							}
							Marshal.WriteInt16(intPtr, text.Length, 2573);
							Marshal.Copy(buffer, offset, IntPtrHelper.Add(intPtr, text.Length + 2), size);
							Marshal.WriteInt16(intPtr, (int)(num - 2U), 2573);
							ptr2 = (byte*)(void*)intPtr;
							offset = 0;
						}
						UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK http_DATA_CHUNK = default(UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK);
						http_DATA_CHUNK.DataChunkType = UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK_TYPE.HttpDataChunkFromMemory;
						http_DATA_CHUNK.pBuffer = ptr2 + offset;
						http_DATA_CHUNK.BufferLength = num;
						http_FLAGS |= ((this.m_LeftToWrite == (long)size) ? UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.NONE : UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.HTTP_SEND_RESPONSE_FLAG_MORE_DATA);
						if (!sentHeaders)
						{
							num2 = this.m_HttpContext.Response.SendHeaders(&http_DATA_CHUNK, null, http_FLAGS);
						}
						else
						{
							num2 = UnsafeNclNativeMethods.HttpApi.HttpSendResponseEntityBody(this.m_HttpContext.RequestQueueHandle, this.m_HttpContext.RequestId, (uint)http_FLAGS, 1, &http_DATA_CHUNK, null, SafeLocalFree.Zero, 0U, null, null);
							if (this.m_HttpContext.Listener.IgnoreWriteExceptions)
							{
								num2 = 0U;
							}
						}
					}
				}
				finally
				{
					byte* ptr = null;
				}
			}
			finally
			{
				if (safeLocalFree != null)
				{
					safeLocalFree.Close();
				}
			}
			if (num2 != 0U && num2 != 38U)
			{
				Exception ex = new HttpListenerException((int)num2);
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "Write", ex);
				}
				this.m_HttpContext.Abort();
				throw ex;
			}
			this.UpdateAfterWrite(num);
			if (Logging.On)
			{
				Logging.Dump(Logging.HttpListener, this, "Write", buffer, offset, (int)num);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "Write", "");
			}
		}

		// Token: 0x0600271D RID: 10013 RVA: 0x000A1A60 File Offset: 0x000A0A60
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
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
			UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS http_FLAGS = this.ComputeLeftToWrite();
			if (size == 0 || this.m_Closed)
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.HttpListener, this, "BeginWrite", "");
				}
				HttpResponseStreamAsyncResult httpResponseStreamAsyncResult = new HttpResponseStreamAsyncResult(this, state, callback);
				httpResponseStreamAsyncResult.InvokeCallback(0U);
				return httpResponseStreamAsyncResult;
			}
			if (this.m_LeftToWrite > 0L && (long)size > this.m_LeftToWrite)
			{
				throw new ProtocolViolationException(SR.GetString("net_entitytoobig"));
			}
			http_FLAGS |= ((this.m_LeftToWrite == (long)size) ? UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.NONE : UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.HTTP_SEND_RESPONSE_FLAG_MORE_DATA);
			bool sentHeaders = this.m_HttpContext.Response.SentHeaders;
			HttpResponseStreamAsyncResult httpResponseStreamAsyncResult2 = new HttpResponseStreamAsyncResult(this, state, callback, buffer, offset, size, this.m_HttpContext.Response.BoundaryType == BoundaryType.Chunked, sentHeaders);
			this.UpdateAfterWrite((uint)((this.m_HttpContext.Response.BoundaryType == BoundaryType.Chunked) ? 0 : size));
			uint num;
			try
			{
				if (!sentHeaders)
				{
					num = this.m_HttpContext.Response.SendHeaders(null, httpResponseStreamAsyncResult2, http_FLAGS);
				}
				else
				{
					this.m_HttpContext.EnsureBoundHandle();
					num = UnsafeNclNativeMethods.HttpApi.HttpSendResponseEntityBody(this.m_HttpContext.RequestQueueHandle, this.m_HttpContext.RequestId, (uint)http_FLAGS, httpResponseStreamAsyncResult2.dataChunkCount, httpResponseStreamAsyncResult2.pDataChunks, null, SafeLocalFree.Zero, 0U, httpResponseStreamAsyncResult2.m_pOverlapped, null);
				}
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "BeginWrite", ex);
				}
				httpResponseStreamAsyncResult2.InternalCleanup();
				this.m_HttpContext.Abort();
				throw;
			}
			if (num != 0U && num != 997U)
			{
				httpResponseStreamAsyncResult2.InternalCleanup();
				if (!this.m_HttpContext.Listener.IgnoreWriteExceptions || !sentHeaders)
				{
					Exception ex2 = new HttpListenerException((int)num);
					if (Logging.On)
					{
						Logging.Exception(Logging.HttpListener, this, "BeginWrite", ex2);
					}
					this.m_HttpContext.Abort();
					throw ex2;
				}
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "BeginWrite", "");
			}
			return httpResponseStreamAsyncResult2;
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x000A1C8C File Offset: 0x000A0C8C
		public override void EndWrite(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "EndWrite", "");
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			HttpResponseStreamAsyncResult httpResponseStreamAsyncResult = asyncResult as HttpResponseStreamAsyncResult;
			if (httpResponseStreamAsyncResult == null || httpResponseStreamAsyncResult.AsyncObject != this)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			if (httpResponseStreamAsyncResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndWrite" }));
			}
			httpResponseStreamAsyncResult.EndCalled = true;
			object obj = httpResponseStreamAsyncResult.InternalWaitForCompletion();
			Exception ex = obj as Exception;
			if (ex != null)
			{
				if (Logging.On)
				{
					Logging.Exception(Logging.HttpListener, this, "EndWrite", ex);
				}
				this.m_HttpContext.Abort();
				throw ex;
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.HttpListener, this, "EndWrite", "");
			}
		}

		// Token: 0x0600271F RID: 10015 RVA: 0x000A1D6C File Offset: 0x000A0D6C
		private void UpdateAfterWrite(uint dataWritten)
		{
			if (this.m_LeftToWrite > 0L)
			{
				this.m_LeftToWrite -= (long)((ulong)dataWritten);
			}
			if (this.m_LeftToWrite == 0L)
			{
				this.m_Closed = true;
			}
		}

		// Token: 0x06002720 RID: 10016 RVA: 0x000A1D98 File Offset: 0x000A0D98
		protected unsafe override void Dispose(bool disposing)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.HttpListener, this, "Close", "");
			}
			try
			{
				if (disposing)
				{
					if (this.m_Closed)
					{
						if (Logging.On)
						{
							Logging.Exit(Logging.HttpListener, this, "Close", "");
						}
						return;
					}
					this.m_Closed = true;
					UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS http_FLAGS = this.ComputeLeftToWrite();
					if (this.m_LeftToWrite > 0L)
					{
						throw new InvalidOperationException(SR.GetString("net_io_notenoughbyteswritten"));
					}
					bool sentHeaders = this.m_HttpContext.Response.SentHeaders;
					if (sentHeaders && this.m_LeftToWrite == 0L)
					{
						if (Logging.On)
						{
							Logging.Exit(Logging.HttpListener, this, "Close", "");
						}
						return;
					}
					uint num = 0U;
					if ((this.m_HttpContext.Response.BoundaryType == BoundaryType.Chunked || this.m_HttpContext.Response.BoundaryType == BoundaryType.None) && string.Compare(this.m_HttpContext.Request.HttpMethod, "HEAD", StringComparison.OrdinalIgnoreCase) != 0)
					{
						if (this.m_HttpContext.Response.BoundaryType == BoundaryType.None)
						{
							http_FLAGS |= UnsafeNclNativeMethods.HttpApi.HTTP_FLAGS.HTTP_RECEIVE_REQUEST_FLAG_COPY_BODY;
						}
						try
						{
							fixed (void* ptr = NclConstants.ChunkTerminator)
							{
								UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK* ptr2 = null;
								if (this.m_HttpContext.Response.BoundaryType == BoundaryType.Chunked)
								{
									UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK http_DATA_CHUNK = default(UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK);
									http_DATA_CHUNK.DataChunkType = UnsafeNclNativeMethods.HttpApi.HTTP_DATA_CHUNK_TYPE.HttpDataChunkFromMemory;
									http_DATA_CHUNK.pBuffer = (byte*)ptr;
									http_DATA_CHUNK.BufferLength = (uint)NclConstants.ChunkTerminator.Length;
									ptr2 = &http_DATA_CHUNK;
								}
								if (!sentHeaders)
								{
									num = this.m_HttpContext.Response.SendHeaders(ptr2, null, http_FLAGS);
								}
								else
								{
									num = UnsafeNclNativeMethods.HttpApi.HttpSendResponseEntityBody(this.m_HttpContext.RequestQueueHandle, this.m_HttpContext.RequestId, (uint)http_FLAGS, (ptr2 != null) ? 1 : 0, ptr2, null, SafeLocalFree.Zero, 0U, null, null);
									if (this.m_HttpContext.Listener.IgnoreWriteExceptions)
									{
										num = 0U;
									}
								}
								goto IL_01F6;
							}
						}
						finally
						{
							void* ptr = null;
						}
					}
					if (!sentHeaders)
					{
						num = this.m_HttpContext.Response.SendHeaders(null, null, http_FLAGS);
					}
					IL_01F6:
					if (num != 0U && num != 38U)
					{
						Exception ex = new HttpListenerException((int)num);
						if (Logging.On)
						{
							Logging.Exception(Logging.HttpListener, this, "Close", ex);
						}
						this.m_HttpContext.Abort();
						throw ex;
					}
					this.m_LeftToWrite = 0L;
				}
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

		// Token: 0x0400269D RID: 9885
		private HttpListenerContext m_HttpContext;

		// Token: 0x0400269E RID: 9886
		private long m_LeftToWrite = long.MinValue;

		// Token: 0x0400269F RID: 9887
		private bool m_Closed;
	}
}
