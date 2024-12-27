using System;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Permissions;
using System.Threading;

namespace System.Net
{
	// Token: 0x020004CF RID: 1231
	internal class ConnectStream : Stream, ICloseEx
	{
		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x0600260E RID: 9742 RVA: 0x00099AC8 File Offset: 0x00098AC8
		public override bool CanTimeout
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x0600260F RID: 9743 RVA: 0x00099ACB File Offset: 0x00098ACB
		// (set) Token: 0x06002610 RID: 9744 RVA: 0x00099AD3 File Offset: 0x00098AD3
		public override int ReadTimeout
		{
			get
			{
				return this.m_ReadTimeout;
			}
			set
			{
				if (value <= 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException(SR.GetString("net_io_timeout_use_gt_zero"));
				}
				this.m_ReadTimeout = value;
			}
		}

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x06002611 RID: 9745 RVA: 0x00099AF4 File Offset: 0x00098AF4
		// (set) Token: 0x06002612 RID: 9746 RVA: 0x00099AFC File Offset: 0x00098AFC
		public override int WriteTimeout
		{
			get
			{
				return this.m_WriteTimeout;
			}
			set
			{
				if (value <= 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException(SR.GetString("net_io_timeout_use_gt_zero"));
				}
				this.m_WriteTimeout = value;
			}
		}

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x06002613 RID: 9747 RVA: 0x00099B1D File Offset: 0x00098B1D
		internal bool IgnoreSocketErrors
		{
			get
			{
				return this.m_IgnoreSocketErrors;
			}
		}

		// Token: 0x06002614 RID: 9748 RVA: 0x00099B25 File Offset: 0x00098B25
		internal void ErrorResponseNotify(bool isKeepAlive)
		{
			this.m_ErrorResponseStatus = true;
			this.m_IgnoreSocketErrors |= !isKeepAlive;
		}

		// Token: 0x06002615 RID: 9749 RVA: 0x00099B40 File Offset: 0x00098B40
		internal void FatalResponseNotify()
		{
			if (this.m_ErrorException == null)
			{
				Interlocked.CompareExchange<Exception>(ref this.m_ErrorException, new IOException(SR.GetString("net_io_readfailure", new object[] { SR.GetString("net_io_connectionclosed") })), null);
			}
			this.m_ErrorResponseStatus = false;
		}

		// Token: 0x06002616 RID: 9750 RVA: 0x00099B90 File Offset: 0x00098B90
		public ConnectStream(Connection connection, HttpWebRequest request)
		{
			this.m_Connection = connection;
			this.m_ReadTimeout = (this.m_WriteTimeout = -1);
			this.m_Request = request;
			this.m_HttpWriteMode = request.HttpWriteMode;
			this.m_BytesLeftToWrite = ((this.m_HttpWriteMode == HttpWriteMode.ContentLength) ? request.ContentLength : (-1L));
			if (request.HttpWriteMode == HttpWriteMode.Buffer)
			{
				this.m_BufferOnly = true;
				this.EnableWriteBuffering();
			}
		}

		// Token: 0x06002617 RID: 9751 RVA: 0x00099BFC File Offset: 0x00098BFC
		public ConnectStream(Connection connection, byte[] buffer, int offset, int bufferCount, long readCount, bool chunked, HttpWebRequest request)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "ConnectStream", SR.GetString("net_log_buffered_n_bytes", new object[] { readCount }));
			}
			this.m_ReadBuffer = buffer;
			this.m_ReadOffset = offset;
			this.m_ReadBufferSize = bufferCount;
			this.m_ReadBytes = readCount;
			this.m_ReadTimeout = (this.m_WriteTimeout = -1);
			this.m_Chunked = chunked;
			this.m_Connection = connection;
			this.m_TempBuffer = new byte[2];
			this.m_Request = request;
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x00099C93 File Offset: 0x00098C93
		internal void SwitchToContentLength()
		{
			this.m_HttpWriteMode = HttpWriteMode.ContentLength;
		}

		// Token: 0x170007E3 RID: 2019
		// (set) Token: 0x06002619 RID: 9753 RVA: 0x00099C9C File Offset: 0x00098C9C
		internal bool SuppressWrite
		{
			set
			{
				this.m_SuppressWrite = value;
			}
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x0600261A RID: 9754 RVA: 0x00099CA5 File Offset: 0x00098CA5
		internal Connection Connection
		{
			get
			{
				return this.m_Connection;
			}
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x0600261B RID: 9755 RVA: 0x00099CAD File Offset: 0x00098CAD
		internal bool BufferOnly
		{
			get
			{
				return this.m_BufferOnly;
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x0600261C RID: 9756 RVA: 0x00099CB5 File Offset: 0x00098CB5
		// (set) Token: 0x0600261D RID: 9757 RVA: 0x00099CBD File Offset: 0x00098CBD
		internal ScatterGatherBuffers BufferedData
		{
			get
			{
				return this.m_BufferedData;
			}
			set
			{
				this.m_BufferedData = value;
			}
		}

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x0600261E RID: 9758 RVA: 0x00099CC6 File Offset: 0x00098CC6
		private bool WriteChunked
		{
			get
			{
				return this.m_HttpWriteMode == HttpWriteMode.Chunked;
			}
		}

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x0600261F RID: 9759 RVA: 0x00099CD1 File Offset: 0x00098CD1
		internal long BytesLeftToWrite
		{
			get
			{
				return this.m_BytesLeftToWrite;
			}
		}

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06002620 RID: 9760 RVA: 0x00099CD9 File Offset: 0x00098CD9
		private bool WriteStream
		{
			get
			{
				return this.m_HttpWriteMode != HttpWriteMode.Unknown;
			}
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06002621 RID: 9761 RVA: 0x00099CE7 File Offset: 0x00098CE7
		internal bool IsPostStream
		{
			get
			{
				return this.m_HttpWriteMode != HttpWriteMode.None;
			}
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06002622 RID: 9762 RVA: 0x00099CF5 File Offset: 0x00098CF5
		internal bool ErrorInStream
		{
			get
			{
				return this.m_ErrorException != null;
			}
		}

		// Token: 0x06002623 RID: 9763 RVA: 0x00099D03 File Offset: 0x00098D03
		internal void CallDone()
		{
			this.CallDone(null);
		}

		// Token: 0x06002624 RID: 9764 RVA: 0x00099D0C File Offset: 0x00098D0C
		private void CallDone(ConnectionReturnResult returnResult)
		{
			if (Interlocked.Increment(ref this.m_DoneCalled) == 1)
			{
				if (!this.WriteStream)
				{
					if (returnResult == null)
					{
						this.m_Connection.ReadStartNextRequest(this.m_Request, ref returnResult);
						return;
					}
					ConnectionReturnResult.SetResponses(returnResult);
					return;
				}
				else
				{
					this.m_Request.WriteCallDone(this, returnResult);
				}
			}
		}

		// Token: 0x06002625 RID: 9765 RVA: 0x00099D5C File Offset: 0x00098D5C
		internal void ProcessWriteCallDone(ConnectionReturnResult returnResult)
		{
			try
			{
				if (returnResult == null)
				{
					this.m_Connection.WriteStartNextRequest(this.m_Request, ref returnResult);
					if (!this.m_Request.Async && this.m_Request.ConnectionReaderAsyncResult.InternalWaitForCompletion() == null && !this.m_Request.SawInitialResponse)
					{
						this.m_Connection.SyncRead(this.m_Request, true, false);
					}
					this.m_Request.SawInitialResponse = false;
				}
				ConnectionReturnResult.SetResponses(returnResult);
			}
			finally
			{
				if (this.IsPostStream || this.m_Request.Async)
				{
					this.m_Request.CheckWriteSideResponseProcessing();
				}
			}
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06002626 RID: 9766 RVA: 0x00099E08 File Offset: 0x00098E08
		internal bool IsClosed
		{
			get
			{
				return this.m_ShutDown != 0;
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06002627 RID: 9767 RVA: 0x00099E16 File Offset: 0x00098E16
		public override bool CanRead
		{
			get
			{
				return !this.WriteStream && !this.IsClosed;
			}
		}

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06002628 RID: 9768 RVA: 0x00099E2B File Offset: 0x00098E2B
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06002629 RID: 9769 RVA: 0x00099E2E File Offset: 0x00098E2E
		public override bool CanWrite
		{
			get
			{
				return this.WriteStream && !this.IsClosed;
			}
		}

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x0600262A RID: 9770 RVA: 0x00099E43 File Offset: 0x00098E43
		public override long Length
		{
			get
			{
				throw new NotSupportedException(SR.GetString("net_noseek"));
			}
		}

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x0600262B RID: 9771 RVA: 0x00099E54 File Offset: 0x00098E54
		// (set) Token: 0x0600262C RID: 9772 RVA: 0x00099E65 File Offset: 0x00098E65
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

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x0600262D RID: 9773 RVA: 0x00099E78 File Offset: 0x00098E78
		private bool Eof
		{
			get
			{
				if (this.ErrorInStream)
				{
					return true;
				}
				if (this.m_Chunked)
				{
					return this.m_ChunkEofRecvd;
				}
				return this.m_ReadBytes == 0L || (this.m_ReadBytes == -1L && this.m_DoneCalled > 0 && this.m_ReadBufferSize <= 0);
			}
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x00099ED0 File Offset: 0x00098ED0
		internal void ResubmitWrite(ConnectStream oldStream, bool suppressWrite)
		{
			try
			{
				Interlocked.CompareExchange(ref this.m_CallNesting, 4, 0);
				ScatterGatherBuffers bufferedData = oldStream.BufferedData;
				this.SafeSetSocketTimeout(SocketShutdown.Send);
				if (!this.WriteChunked)
				{
					if (!suppressWrite)
					{
						this.m_Connection.Write(bufferedData);
					}
				}
				else
				{
					this.m_HttpWriteMode = HttpWriteMode.ContentLength;
					if (bufferedData.Length == 0)
					{
						this.m_Connection.Write(NclConstants.ChunkTerminator, 0, NclConstants.ChunkTerminator.Length);
					}
					else
					{
						int num = 0;
						byte[] chunkHeader = ConnectStream.GetChunkHeader(bufferedData.Length, out num);
						BufferOffsetSize[] buffers = bufferedData.GetBuffers();
						BufferOffsetSize[] array = new BufferOffsetSize[buffers.Length + 3];
						array[0] = new BufferOffsetSize(chunkHeader, num, chunkHeader.Length - num, false);
						int num2 = 0;
						foreach (BufferOffsetSize bufferOffsetSize in buffers)
						{
							array[++num2] = bufferOffsetSize;
						}
						array[++num2] = new BufferOffsetSize(NclConstants.CRLF, 0, NclConstants.CRLF.Length, false);
						array[num2 + 1] = new BufferOffsetSize(NclConstants.ChunkTerminator, 0, NclConstants.ChunkTerminator.Length, false);
						SplitWritesState splitWritesState = new SplitWritesState(array);
						for (BufferOffsetSize[] array3 = splitWritesState.GetNextBuffers(); array3 != null; array3 = splitWritesState.GetNextBuffers())
						{
							this.m_Connection.MultipleWrite(array3);
						}
					}
				}
				if (Logging.On && bufferedData.GetBuffers() != null)
				{
					foreach (BufferOffsetSize bufferOffsetSize2 in bufferedData.GetBuffers())
					{
						if (bufferOffsetSize2 == null)
						{
							Logging.Dump(Logging.Web, this, "ResubmitWrite", null, 0, 0);
						}
						else
						{
							Logging.Dump(Logging.Web, this, "ResubmitWrite", bufferOffsetSize2.Buffer, bufferOffsetSize2.Offset, bufferOffsetSize2.Size);
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (NclUtilities.IsFatal(ex))
				{
					throw;
				}
				WebException ex2 = new WebException(NetRes.GetWebStatusString("net_connclosed", WebExceptionStatus.SendFailure), WebExceptionStatus.SendFailure, WebExceptionInternalStatus.RequestFatal, ex);
				this.IOError(ex2, false);
			}
			finally
			{
				Interlocked.CompareExchange(ref this.m_CallNesting, 0, 4);
			}
			this.m_BytesLeftToWrite = 0L;
			this.CallDone();
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x0009A100 File Offset: 0x00099100
		internal void EnableWriteBuffering()
		{
			if (this.BufferedData == null)
			{
				if (this.WriteChunked)
				{
					this.BufferedData = new ScatterGatherBuffers();
					return;
				}
				this.BufferedData = new ScatterGatherBuffers(this.BytesLeftToWrite);
			}
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x0009A130 File Offset: 0x00099130
		private int FillFromBufferedData(byte[] buffer, ref int offset, ref int size)
		{
			if (this.m_ReadBufferSize == 0)
			{
				return 0;
			}
			int num = Math.Min(size, this.m_ReadBufferSize);
			Buffer.BlockCopy(this.m_ReadBuffer, this.m_ReadOffset, buffer, offset, num);
			this.m_ReadOffset += num;
			this.m_ReadBufferSize -= num;
			if (this.m_ReadBufferSize == 0)
			{
				this.m_ReadBuffer = null;
			}
			size -= num;
			offset += num;
			return num;
		}

		// Token: 0x06002631 RID: 9777 RVA: 0x0009A1A4 File Offset: 0x000991A4
		public override void Write(byte[] buffer, int offset, int size)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "Write", "");
			}
			if (!this.WriteStream)
			{
				throw new NotSupportedException(SR.GetString("net_readonlystream"));
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
			this.InternalWrite(false, buffer, offset, size, null, null);
			if (Logging.On)
			{
				Logging.Dump(Logging.Web, this, "Write", buffer, offset, size);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "Write", "");
			}
		}

		// Token: 0x06002632 RID: 9778 RVA: 0x0009A264 File Offset: 0x00099264
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "BeginWrite", "");
			}
			if (!this.WriteStream)
			{
				throw new NotSupportedException(SR.GetString("net_readonlystream"));
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
			IAsyncResult asyncResult = this.InternalWrite(true, buffer, offset, size, callback, state);
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "BeginWrite", asyncResult);
			}
			return asyncResult;
		}

		// Token: 0x06002633 RID: 9779 RVA: 0x0009A308 File Offset: 0x00099308
		private IAsyncResult InternalWrite(bool async, byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			if (this.ErrorInStream)
			{
				throw this.m_ErrorException;
			}
			if (this.IsClosed && !this.IgnoreSocketErrors)
			{
				throw new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.ConnectionClosed), WebExceptionStatus.ConnectionClosed);
			}
			if (this.m_Request.Aborted && !this.IgnoreSocketErrors)
			{
				throw new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
			}
			int num = Interlocked.CompareExchange(ref this.m_CallNesting, 1, 0);
			if (num != 0 && num != 2)
			{
				throw new NotSupportedException(SR.GetString("net_no_concurrent_io_allowed"));
			}
			if (this.BufferedData != null && size != 0 && (this.m_Request.ContentLength != 0L || !this.IsPostStream || !this.m_Request.NtlmKeepAlive))
			{
				this.BufferedData.Write(buffer, offset, size);
			}
			LazyAsyncResult lazyAsyncResult = null;
			bool flag = false;
			IAsyncResult asyncResult;
			try
			{
				if (size == 0 || this.BufferOnly || this.m_SuppressWrite || this.IgnoreSocketErrors)
				{
					if (this.m_SuppressWrite && this.m_BytesLeftToWrite > 0L && size > 0)
					{
						this.m_BytesLeftToWrite -= (long)size;
					}
					if (async)
					{
						lazyAsyncResult = new LazyAsyncResult(this, state, callback);
						flag = true;
					}
					asyncResult = lazyAsyncResult;
				}
				else if (this.WriteChunked)
				{
					int num2 = 0;
					byte[] chunkHeader = ConnectStream.GetChunkHeader(size, out num2);
					BufferOffsetSize[] array;
					if (this.m_ErrorResponseStatus)
					{
						this.m_IgnoreSocketErrors = true;
						array = new BufferOffsetSize[]
						{
							new BufferOffsetSize(NclConstants.ChunkTerminator, 0, NclConstants.ChunkTerminator.Length, false)
						};
					}
					else
					{
						array = new BufferOffsetSize[]
						{
							new BufferOffsetSize(chunkHeader, num2, chunkHeader.Length - num2, false),
							new BufferOffsetSize(buffer, offset, size, false),
							new BufferOffsetSize(NclConstants.CRLF, 0, NclConstants.CRLF.Length, false)
						};
					}
					lazyAsyncResult = (async ? new NestedMultipleAsyncResult(this, state, callback, array) : null);
					try
					{
						if (async)
						{
							this.m_Connection.BeginMultipleWrite(array, ConnectStream.m_WriteCallbackDelegate, lazyAsyncResult);
						}
						else
						{
							this.SafeSetSocketTimeout(SocketShutdown.Send);
							this.m_Connection.MultipleWrite(array);
						}
					}
					catch (Exception ex)
					{
						if (this.IgnoreSocketErrors && !NclUtilities.IsFatal(ex))
						{
							if (async)
							{
								flag = true;
							}
							return lazyAsyncResult;
						}
						if (this.m_Request.Aborted && (ex is IOException || ex is ObjectDisposedException))
						{
							throw new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
						}
						num = 3;
						if (NclUtilities.IsFatal(ex))
						{
							this.m_ErrorResponseStatus = false;
							this.IOError(ex);
							throw;
						}
						if (!this.m_ErrorResponseStatus)
						{
							this.IOError(ex);
							throw;
						}
						this.m_IgnoreSocketErrors = true;
						if (async)
						{
							flag = true;
						}
					}
					asyncResult = lazyAsyncResult;
				}
				else
				{
					lazyAsyncResult = (async ? new NestedSingleAsyncResult(this, state, callback, buffer, offset, size) : null);
					if (this.BytesLeftToWrite != -1L)
					{
						if (this.BytesLeftToWrite < (long)size)
						{
							throw new ProtocolViolationException(SR.GetString("net_entitytoobig"));
						}
						if (!async)
						{
							this.m_BytesLeftToWrite -= (long)size;
						}
					}
					try
					{
						if (async)
						{
							if (this.m_Request.ContentLength == 0L && this.IsPostStream)
							{
								this.m_BytesLeftToWrite -= (long)size;
								flag = true;
							}
							else
							{
								this.m_BytesAlreadyTransferred = size;
								this.m_Connection.BeginWrite(buffer, offset, size, ConnectStream.m_WriteCallbackDelegate, lazyAsyncResult);
							}
						}
						else
						{
							this.SafeSetSocketTimeout(SocketShutdown.Send);
							if (this.m_Request.ContentLength != 0L || !this.IsPostStream || !this.m_Request.NtlmKeepAlive)
							{
								this.m_Connection.Write(buffer, offset, size);
							}
						}
					}
					catch (Exception ex2)
					{
						if (this.IgnoreSocketErrors && !NclUtilities.IsFatal(ex2))
						{
							if (async)
							{
								flag = true;
							}
							return lazyAsyncResult;
						}
						if (this.m_Request.Aborted && (ex2 is IOException || ex2 is ObjectDisposedException))
						{
							throw new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
						}
						num = 3;
						if (NclUtilities.IsFatal(ex2))
						{
							this.m_ErrorResponseStatus = false;
							this.IOError(ex2);
							throw;
						}
						if (!this.m_ErrorResponseStatus)
						{
							this.IOError(ex2);
							throw;
						}
						this.m_IgnoreSocketErrors = true;
						if (async)
						{
							flag = true;
						}
					}
					asyncResult = lazyAsyncResult;
				}
			}
			finally
			{
				if (!async || num == 3 || flag)
				{
					num = Interlocked.CompareExchange(ref this.m_CallNesting, (num == 3) ? 3 : 0, 1);
					if (num == 2)
					{
						this.ResumeInternalClose(lazyAsyncResult);
					}
					else if (flag && lazyAsyncResult != null)
					{
						lazyAsyncResult.InvokeCallback();
					}
				}
			}
			return asyncResult;
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x0009A78C File Offset: 0x0009978C
		private static void WriteCallback(IAsyncResult asyncResult)
		{
			LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)asyncResult.AsyncState;
			((ConnectStream)lazyAsyncResult.AsyncObject).ProcessWriteCallback(asyncResult, lazyAsyncResult);
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x0009A7B8 File Offset: 0x000997B8
		private void ProcessWriteCallback(IAsyncResult asyncResult, LazyAsyncResult userResult)
		{
			Exception ex = null;
			try
			{
				NestedSingleAsyncResult nestedSingleAsyncResult = userResult as NestedSingleAsyncResult;
				if (nestedSingleAsyncResult != null)
				{
					try
					{
						this.m_Connection.EndWrite(asyncResult);
						if (this.BytesLeftToWrite != -1L)
						{
							this.m_BytesLeftToWrite -= (long)this.m_BytesAlreadyTransferred;
							this.m_BytesAlreadyTransferred = 0;
						}
						if (Logging.On)
						{
							Logging.Dump(Logging.Web, this, "WriteCallback", nestedSingleAsyncResult.Buffer, nestedSingleAsyncResult.Offset, nestedSingleAsyncResult.Size);
						}
						goto IL_0134;
					}
					catch (Exception ex2)
					{
						ex = ex2;
						if (NclUtilities.IsFatal(ex2))
						{
							this.m_ErrorResponseStatus = false;
							this.IOError(ex2);
							throw;
						}
						if (this.m_ErrorResponseStatus)
						{
							this.m_IgnoreSocketErrors = true;
							ex = null;
						}
						goto IL_0134;
					}
				}
				NestedMultipleAsyncResult nestedMultipleAsyncResult = (NestedMultipleAsyncResult)userResult;
				try
				{
					this.m_Connection.EndMultipleWrite(asyncResult);
					if (Logging.On)
					{
						foreach (BufferOffsetSize bufferOffsetSize in nestedMultipleAsyncResult.Buffers)
						{
							Logging.Dump(Logging.Web, nestedMultipleAsyncResult, "WriteCallback", bufferOffsetSize.Buffer, bufferOffsetSize.Offset, bufferOffsetSize.Size);
						}
					}
				}
				catch (Exception ex3)
				{
					ex = ex3;
					if (NclUtilities.IsFatal(ex3))
					{
						this.m_ErrorResponseStatus = false;
						this.IOError(ex3);
						throw;
					}
					if (this.m_ErrorResponseStatus)
					{
						this.m_IgnoreSocketErrors = true;
						ex = null;
					}
				}
				IL_0134:;
			}
			finally
			{
				if (2 == this.ExchangeCallNesting((ex == null) ? 0 : 3, 1))
				{
					if (ex != null && this.m_ErrorException == null)
					{
						Interlocked.CompareExchange<Exception>(ref this.m_ErrorException, ex, null);
					}
					this.ResumeInternalClose(userResult);
				}
				else
				{
					userResult.InvokeCallback(ex);
				}
			}
		}

		// Token: 0x06002636 RID: 9782 RVA: 0x0009A984 File Offset: 0x00099984
		private int ExchangeCallNesting(int value, int comparand)
		{
			return Interlocked.CompareExchange(ref this.m_CallNesting, value, comparand);
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x0009A9A0 File Offset: 0x000999A0
		public override void EndWrite(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "EndWrite", "");
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
			if (lazyAsyncResult == null || lazyAsyncResult.AsyncObject != this)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
			}
			if (lazyAsyncResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndWrite" }));
			}
			lazyAsyncResult.EndCalled = true;
			object obj = lazyAsyncResult.InternalWaitForCompletion();
			if (this.ErrorInStream)
			{
				throw this.m_ErrorException;
			}
			Exception ex = obj as Exception;
			if (ex == null)
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "EndWrite", "");
				}
				return;
			}
			if (ex is IOException && this.m_Request.Aborted)
			{
				throw new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
			}
			this.IOError(ex);
			throw ex;
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x0009AA9C File Offset: 0x00099A9C
		public override int Read([In] [Out] byte[] buffer, int offset, int size)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "Read", "");
			}
			if (this.WriteStream)
			{
				throw new NotSupportedException(SR.GetString("net_writeonlystream"));
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
			if (this.ErrorInStream)
			{
				throw this.m_ErrorException;
			}
			if (this.IsClosed)
			{
				throw new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.ConnectionClosed), WebExceptionStatus.ConnectionClosed);
			}
			if (this.m_Request.Aborted)
			{
				throw new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
			}
			int num = Interlocked.CompareExchange(ref this.m_CallNesting, 1, 0);
			if (num != 0)
			{
				throw new NotSupportedException(SR.GetString("net_no_concurrent_io_allowed"));
			}
			int num2 = -1;
			try
			{
				this.SafeSetSocketTimeout(SocketShutdown.Receive);
			}
			catch (Exception ex)
			{
				this.IOError(ex);
				throw;
			}
			try
			{
				num2 = this.ReadWithoutValidation(buffer, offset, size);
			}
			catch (Exception ex2)
			{
				Win32Exception ex3 = ex2.InnerException as Win32Exception;
				if (ex3 != null && ex3.NativeErrorCode == 10060)
				{
					ex2 = new WebException(SR.GetString("net_timeout"), WebExceptionStatus.Timeout);
				}
				throw ex2;
			}
			Interlocked.CompareExchange(ref this.m_CallNesting, 0, 1);
			if (Logging.On && num2 > 0)
			{
				Logging.Dump(Logging.Web, this, "Read", buffer, offset, num2);
			}
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "Read", num2);
			}
			return num2;
		}

		// Token: 0x06002639 RID: 9785 RVA: 0x0009AC3C File Offset: 0x00099C3C
		private int ReadWithoutValidation(byte[] buffer, int offset, int size)
		{
			return this.ReadWithoutValidation(buffer, offset, size, true);
		}

		// Token: 0x0600263A RID: 9786 RVA: 0x0009AC48 File Offset: 0x00099C48
		private int ReadWithoutValidation([In] [Out] byte[] buffer, int offset, int size, bool abortOnError)
		{
			int num = 0;
			if (this.m_Chunked)
			{
				if (!this.m_ChunkEofRecvd)
				{
					if (this.m_ChunkSize == 0)
					{
						try
						{
							num = this.ReadChunkedSync(buffer, offset, size);
							this.m_ChunkSize -= num;
						}
						catch (Exception ex)
						{
							if (abortOnError)
							{
								this.IOError(ex);
							}
							throw;
						}
						return num;
					}
					num = Math.Min(size, this.m_ChunkSize);
				}
			}
			else if (this.m_ReadBytes != -1L)
			{
				num = (int)Math.Min(this.m_ReadBytes, (long)size);
			}
			else
			{
				num = size;
			}
			if (num == 0 || this.Eof)
			{
				return 0;
			}
			try
			{
				num = this.InternalRead(buffer, offset, num);
			}
			catch (Exception ex2)
			{
				if (abortOnError)
				{
					this.IOError(ex2);
				}
				throw;
			}
			int num2 = num;
			if (this.m_Chunked && this.m_ChunkSize > 0)
			{
				if (num2 == 0)
				{
					WebException ex3 = new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.ConnectionClosed), WebExceptionStatus.ConnectionClosed);
					this.IOError(ex3, true);
					throw ex3;
				}
				this.m_ChunkSize -= num2;
			}
			else
			{
				bool flag = false;
				if (num2 <= 0)
				{
					num2 = 0;
					if (this.m_ReadBytes != -1L)
					{
						if (!abortOnError)
						{
							throw this.m_ErrorException;
						}
						this.IOError(null, false);
					}
					else
					{
						flag = true;
					}
				}
				if (this.m_ReadBytes != -1L)
				{
					this.m_ReadBytes -= (long)num2;
					if (this.m_ReadBytes < 0L)
					{
						throw new InternalException();
					}
				}
				if (this.m_ReadBytes == 0L || flag)
				{
					this.m_ReadBytes = 0L;
					this.CallDone();
				}
			}
			return num2;
		}

		// Token: 0x0600263B RID: 9787 RVA: 0x0009ADC0 File Offset: 0x00099DC0
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "BeginRead", "");
			}
			if (this.WriteStream)
			{
				throw new NotSupportedException(SR.GetString("net_writeonlystream"));
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
			if (this.ErrorInStream)
			{
				throw this.m_ErrorException;
			}
			if (this.IsClosed)
			{
				throw new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.ConnectionClosed), WebExceptionStatus.ConnectionClosed);
			}
			if (this.m_Request.Aborted)
			{
				throw new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
			}
			int num = Interlocked.CompareExchange(ref this.m_CallNesting, 1, 0);
			if (num != 0)
			{
				throw new NotSupportedException(SR.GetString("net_no_concurrent_io_allowed"));
			}
			IAsyncResult asyncResult = this.BeginReadWithoutValidation(buffer, offset, size, callback, state);
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "BeginRead", asyncResult);
			}
			return asyncResult;
		}

		// Token: 0x0600263C RID: 9788 RVA: 0x0009AECC File Offset: 0x00099ECC
		private IAsyncResult BeginReadWithoutValidation(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			int num = 0;
			if (this.m_Chunked)
			{
				if (!this.m_ChunkEofRecvd)
				{
					if (this.m_ChunkSize == 0)
					{
						NestedSingleAsyncResult nestedSingleAsyncResult = new NestedSingleAsyncResult(this, state, callback, buffer, offset, size);
						ThreadPool.QueueUserWorkItem(ConnectStream.m_ReadChunkedCallbackDelegate, nestedSingleAsyncResult);
						return nestedSingleAsyncResult;
					}
					num = Math.Min(size, this.m_ChunkSize);
				}
			}
			else if (this.m_ReadBytes != -1L)
			{
				num = (int)Math.Min(this.m_ReadBytes, (long)size);
			}
			else
			{
				num = size;
			}
			if (num == 0 || this.Eof)
			{
				return new NestedSingleAsyncResult(this, state, callback, ConnectStream.ZeroLengthRead);
			}
			IAsyncResult asyncResult2;
			try
			{
				int num2 = 0;
				if (this.m_ReadBufferSize > 0)
				{
					num2 = this.FillFromBufferedData(buffer, ref offset, ref num);
					if (num == 0)
					{
						return new NestedSingleAsyncResult(this, state, callback, num2);
					}
				}
				if (this.ErrorInStream)
				{
					throw this.m_ErrorException;
				}
				this.m_BytesAlreadyTransferred = num2;
				IAsyncResult asyncResult = this.m_Connection.BeginRead(buffer, offset, num, callback, state);
				if (asyncResult == null)
				{
					this.m_BytesAlreadyTransferred = 0;
					this.m_ErrorException = new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
					throw this.m_ErrorException;
				}
				asyncResult2 = asyncResult;
			}
			catch (Exception ex)
			{
				this.IOError(ex);
				throw;
			}
			return asyncResult2;
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x0009B004 File Offset: 0x0009A004
		private int InternalRead(byte[] buffer, int offset, int size)
		{
			int num = this.FillFromBufferedData(buffer, ref offset, ref size);
			if (num > 0)
			{
				return num;
			}
			if (this.ErrorInStream)
			{
				throw this.m_ErrorException;
			}
			return this.m_Connection.Read(buffer, offset, size);
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x0009B044 File Offset: 0x0009A044
		private static void ReadCallback(IAsyncResult asyncResult)
		{
			NestedSingleAsyncResult nestedSingleAsyncResult = (NestedSingleAsyncResult)asyncResult.AsyncState;
			ConnectStream connectStream = (ConnectStream)nestedSingleAsyncResult.AsyncObject;
			try
			{
				int num = connectStream.m_Connection.EndRead(asyncResult);
				if (Logging.On)
				{
					Logging.Dump(Logging.Web, connectStream, "ReadCallback", nestedSingleAsyncResult.Buffer, nestedSingleAsyncResult.Offset, Math.Min(nestedSingleAsyncResult.Size, num));
				}
				nestedSingleAsyncResult.InvokeCallback(num);
			}
			catch (Exception ex)
			{
				if (NclUtilities.IsFatal(ex))
				{
					throw;
				}
				nestedSingleAsyncResult.InvokeCallback(ex);
			}
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x0009B0D8 File Offset: 0x0009A0D8
		public override int EndRead(IAsyncResult asyncResult)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "EndRead", "");
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			bool flag = false;
			int num;
			if (asyncResult.GetType() == typeof(NestedSingleAsyncResult))
			{
				NestedSingleAsyncResult nestedSingleAsyncResult = (NestedSingleAsyncResult)asyncResult;
				if (nestedSingleAsyncResult.AsyncObject != this)
				{
					throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"), "asyncResult");
				}
				if (nestedSingleAsyncResult.EndCalled)
				{
					throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndRead" }));
				}
				nestedSingleAsyncResult.EndCalled = true;
				if (this.ErrorInStream)
				{
					throw this.m_ErrorException;
				}
				object obj = nestedSingleAsyncResult.InternalWaitForCompletion();
				Exception ex = obj as Exception;
				if (ex != null)
				{
					this.IOError(ex, false);
					num = -1;
					goto IL_0113;
				}
				if (obj == null)
				{
					num = 0;
					goto IL_0113;
				}
				if (obj == ConnectStream.ZeroLengthRead)
				{
					num = 0;
					flag = true;
					goto IL_0113;
				}
				try
				{
					num = (int)obj;
					goto IL_0113;
				}
				catch (InvalidCastException)
				{
					num = -1;
					goto IL_0113;
				}
			}
			try
			{
				num = this.m_Connection.EndRead(asyncResult);
			}
			catch (Exception ex2)
			{
				if (NclUtilities.IsFatal(ex2))
				{
					throw;
				}
				this.IOError(ex2, false);
				num = -1;
			}
			IL_0113:
			num = this.EndReadWithoutValidation(num, flag);
			Interlocked.CompareExchange(ref this.m_CallNesting, 0, 1);
			if (Logging.On)
			{
				Logging.Exit(Logging.Web, this, "EndRead", num);
			}
			if (this.m_ErrorException != null)
			{
				throw this.m_ErrorException;
			}
			return num;
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x0009B258 File Offset: 0x0009A258
		private int EndReadWithoutValidation(int bytesTransferred, bool zeroLengthRead)
		{
			int bytesAlreadyTransferred = this.m_BytesAlreadyTransferred;
			this.m_BytesAlreadyTransferred = 0;
			if (this.m_Chunked)
			{
				if (bytesTransferred < 0)
				{
					this.IOError(null, false);
					bytesTransferred = 0;
				}
				if (bytesTransferred == 0 && this.m_ChunkSize > 0)
				{
					WebException ex = new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.ConnectionClosed), WebExceptionStatus.ConnectionClosed);
					this.IOError(ex, true);
					throw ex;
				}
				bytesTransferred += bytesAlreadyTransferred;
				this.m_ChunkSize -= bytesTransferred;
			}
			else
			{
				bool flag = false;
				if (bytesTransferred <= 0)
				{
					if (this.m_ReadBytes != -1L && (bytesTransferred < 0 || !zeroLengthRead))
					{
						this.IOError(null, false);
					}
					else
					{
						flag = true;
						bytesTransferred = 0;
					}
				}
				bytesTransferred += bytesAlreadyTransferred;
				if (this.m_ReadBytes != -1L)
				{
					this.m_ReadBytes -= (long)bytesTransferred;
				}
				if (this.m_ReadBytes == 0L || flag)
				{
					this.m_ReadBytes = 0L;
					this.CallDone();
				}
			}
			return bytesTransferred;
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x0009B328 File Offset: 0x0009A328
		internal int ReadSingleByte()
		{
			if (this.ErrorInStream)
			{
				return -1;
			}
			if (this.m_ReadBufferSize != 0)
			{
				this.m_ReadBufferSize--;
				return (int)this.m_ReadBuffer[this.m_ReadOffset++];
			}
			int num = this.m_Connection.Read(this.m_TempBuffer, 0, 1);
			if (num <= 0)
			{
				return -1;
			}
			return (int)this.m_TempBuffer[0];
		}

		// Token: 0x06002642 RID: 9794 RVA: 0x0009B390 File Offset: 0x0009A390
		private int ReadCRLF(byte[] buffer)
		{
			int num = 0;
			int num2 = NclConstants.CRLF.Length;
			int num3 = this.FillFromBufferedData(buffer, ref num, ref num2);
			if (num3 >= 0 && num3 != NclConstants.CRLF.Length)
			{
				for (;;)
				{
					int num4 = this.m_Connection.Read(buffer, num, num2);
					if (num4 <= 0)
					{
						break;
					}
					num2 -= num4;
					num += num4;
					if (num2 <= 0)
					{
						return num3;
					}
				}
				throw new IOException(SR.GetString("net_io_readfailure", new object[] { SR.GetString("net_io_connectionclosed") }));
			}
			return num3;
		}

		// Token: 0x06002643 RID: 9795 RVA: 0x0009B40C File Offset: 0x0009A40C
		private static void ReadChunkedCallback(object state)
		{
			NestedSingleAsyncResult nestedSingleAsyncResult = state as NestedSingleAsyncResult;
			ConnectStream connectStream = nestedSingleAsyncResult.AsyncObject as ConnectStream;
			try
			{
				if (!connectStream.m_Draining && connectStream.IsClosed)
				{
					Exception ex = new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.ConnectionClosed), WebExceptionStatus.ConnectionClosed);
					nestedSingleAsyncResult.InvokeCallback(ex);
				}
				else if (connectStream.m_ErrorException != null)
				{
					nestedSingleAsyncResult.InvokeCallback(connectStream.m_ErrorException);
				}
				else
				{
					if (connectStream.m_ChunkedNeedCRLFRead)
					{
						connectStream.ReadCRLF(connectStream.m_TempBuffer);
						connectStream.m_ChunkedNeedCRLFRead = false;
					}
					StreamChunkBytes streamChunkBytes = new StreamChunkBytes(connectStream);
					connectStream.m_ChunkSize = connectStream.ProcessReadChunkedSize(streamChunkBytes);
					if (connectStream.m_ChunkSize != 0)
					{
						connectStream.m_ChunkedNeedCRLFRead = true;
						int num = Math.Min(nestedSingleAsyncResult.Size, connectStream.m_ChunkSize);
						int num2 = 0;
						if (connectStream.m_ReadBufferSize > 0)
						{
							num2 = connectStream.FillFromBufferedData(nestedSingleAsyncResult.Buffer, ref nestedSingleAsyncResult.Offset, ref num);
							if (num == 0)
							{
								nestedSingleAsyncResult.InvokeCallback(num2);
								return;
							}
						}
						if (connectStream.ErrorInStream)
						{
							throw connectStream.m_ErrorException;
						}
						connectStream.m_BytesAlreadyTransferred = num2;
						if (connectStream.m_Connection.BeginRead(nestedSingleAsyncResult.Buffer, nestedSingleAsyncResult.Offset, num, ConnectStream.m_ReadCallbackDelegate, nestedSingleAsyncResult) == null)
						{
							connectStream.m_BytesAlreadyTransferred = 0;
							connectStream.m_ErrorException = new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
							throw connectStream.m_ErrorException;
						}
					}
					else
					{
						connectStream.ReadCRLF(connectStream.m_TempBuffer);
						connectStream.RemoveTrailers(streamChunkBytes);
						connectStream.m_ReadBytes = 0L;
						connectStream.m_ChunkEofRecvd = true;
						connectStream.CallDone();
						nestedSingleAsyncResult.InvokeCallback(0);
					}
				}
			}
			catch (Exception ex2)
			{
				if (NclUtilities.IsFatal(ex2))
				{
					throw;
				}
				nestedSingleAsyncResult.InvokeCallback(ex2);
			}
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x0009B5CC File Offset: 0x0009A5CC
		private int ReadChunkedSync(byte[] buffer, int offset, int size)
		{
			if (!this.m_Draining && this.IsClosed)
			{
				Exception ex = new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.ConnectionClosed), WebExceptionStatus.ConnectionClosed);
				throw ex;
			}
			if (this.m_ErrorException != null)
			{
				throw this.m_ErrorException;
			}
			if (this.m_ChunkedNeedCRLFRead)
			{
				this.ReadCRLF(this.m_TempBuffer);
				this.m_ChunkedNeedCRLFRead = false;
			}
			StreamChunkBytes streamChunkBytes = new StreamChunkBytes(this);
			this.m_ChunkSize = this.ProcessReadChunkedSize(streamChunkBytes);
			if (this.m_ChunkSize != 0)
			{
				this.m_ChunkedNeedCRLFRead = true;
				return this.InternalRead(buffer, offset, Math.Min(size, this.m_ChunkSize));
			}
			this.ReadCRLF(this.m_TempBuffer);
			this.RemoveTrailers(streamChunkBytes);
			this.m_ReadBytes = 0L;
			this.m_ChunkEofRecvd = true;
			this.CallDone();
			return 0;
		}

		// Token: 0x06002645 RID: 9797 RVA: 0x0009B68C File Offset: 0x0009A68C
		private int ProcessReadChunkedSize(StreamChunkBytes ReadByteBuffer)
		{
			int num2;
			int num = ChunkParse.GetChunkSize(ReadByteBuffer, out num2);
			if (num <= 0)
			{
				throw new IOException(SR.GetString("net_io_readfailure", new object[] { SR.GetString("net_io_connectionclosed") }));
			}
			num = ChunkParse.SkipPastCRLF(ReadByteBuffer);
			if (num <= 0)
			{
				throw new IOException(SR.GetString("net_io_readfailure", new object[] { SR.GetString("net_io_connectionclosed") }));
			}
			return num2;
		}

		// Token: 0x06002646 RID: 9798 RVA: 0x0009B6FC File Offset: 0x0009A6FC
		private void RemoveTrailers(StreamChunkBytes ReadByteBuffer)
		{
			while (this.m_TempBuffer[0] != 13 && this.m_TempBuffer[1] != 10)
			{
				int num = ChunkParse.SkipPastCRLF(ReadByteBuffer);
				if (num <= 0)
				{
					throw new IOException(SR.GetString("net_io_readfailure", new object[] { SR.GetString("net_io_connectionclosed") }));
				}
				this.ReadCRLF(this.m_TempBuffer);
			}
		}

		// Token: 0x06002647 RID: 9799 RVA: 0x0009B760 File Offset: 0x0009A760
		private static void WriteHeadersCallback(IAsyncResult ar)
		{
			if (ar.CompletedSynchronously)
			{
				return;
			}
			WriteHeadersCallbackState writeHeadersCallbackState = (WriteHeadersCallbackState)ar.AsyncState;
			ConnectStream stream = writeHeadersCallbackState.stream;
			HttpWebRequest request = writeHeadersCallbackState.request;
			WebExceptionStatus webExceptionStatus = WebExceptionStatus.SendFailure;
			byte[] writeBuffer = request.WriteBuffer;
			try
			{
				stream.m_Connection.EndWrite(ar);
				stream.m_Connection.CheckStartReceive(request);
				if (stream.m_Connection.m_InnerException != null)
				{
					throw stream.m_Connection.m_InnerException;
				}
				webExceptionStatus = WebExceptionStatus.Success;
			}
			catch (Exception ex)
			{
				if (NclUtilities.IsFatal(ex))
				{
					throw;
				}
				if (ex is IOException || ex is ObjectDisposedException)
				{
					if (!stream.m_Connection.AtLeastOneResponseReceived && !request.BodyStarted)
					{
						ex = new WebException(NetRes.GetWebStatusString("net_connclosed", webExceptionStatus), webExceptionStatus, WebExceptionInternalStatus.Recoverable, ex);
					}
					else
					{
						ex = new WebException(NetRes.GetWebStatusString("net_connclosed", webExceptionStatus), webExceptionStatus, stream.m_Connection.AtLeastOneResponseReceived ? WebExceptionInternalStatus.Isolated : WebExceptionInternalStatus.RequestFatal, ex);
					}
				}
				stream.IOError(ex, false);
			}
			stream.ExchangeCallNesting(0, 4);
			request.WriteHeadersCallback(webExceptionStatus, stream, true);
		}

		// Token: 0x06002648 RID: 9800 RVA: 0x0009B870 File Offset: 0x0009A870
		internal void WriteHeaders(bool async)
		{
			WebExceptionStatus webExceptionStatus = WebExceptionStatus.SendFailure;
			if (!this.ErrorInStream)
			{
				byte[] writeBuffer = this.m_Request.WriteBuffer;
				try
				{
					Interlocked.CompareExchange(ref this.m_CallNesting, 4, 0);
					if (async)
					{
						WriteHeadersCallbackState writeHeadersCallbackState = new WriteHeadersCallbackState(this.m_Request, this);
						IAsyncResult asyncResult = this.m_Connection.UnsafeBeginWrite(writeBuffer, 0, writeBuffer.Length, ConnectStream.m_WriteHeadersCallback, writeHeadersCallbackState);
						if (asyncResult.CompletedSynchronously)
						{
							this.m_Connection.EndWrite(asyncResult);
							this.m_Connection.CheckStartReceive(this.m_Request);
							webExceptionStatus = WebExceptionStatus.Success;
						}
						else
						{
							webExceptionStatus = WebExceptionStatus.Pending;
						}
					}
					else
					{
						this.SafeSetSocketTimeout(SocketShutdown.Send);
						this.m_Connection.Write(writeBuffer, 0, writeBuffer.Length);
						this.m_Connection.CheckStartReceive(this.m_Request);
						webExceptionStatus = WebExceptionStatus.Success;
					}
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_sending_headers", new object[] { this.m_Request.Headers.ToString(true) }));
					}
				}
				catch (Exception ex)
				{
					if (NclUtilities.IsFatal(ex))
					{
						throw;
					}
					if (ex is IOException || ex is ObjectDisposedException)
					{
						if (!this.m_Connection.AtLeastOneResponseReceived && !this.m_Request.BodyStarted)
						{
							ex = new WebException(NetRes.GetWebStatusString("net_connclosed", webExceptionStatus), webExceptionStatus, WebExceptionInternalStatus.Recoverable, ex);
						}
						else
						{
							ex = new WebException(NetRes.GetWebStatusString("net_connclosed", webExceptionStatus), webExceptionStatus, this.m_Connection.AtLeastOneResponseReceived ? WebExceptionInternalStatus.Isolated : WebExceptionInternalStatus.RequestFatal, ex);
						}
					}
					this.IOError(ex, false);
				}
				finally
				{
					if (webExceptionStatus != WebExceptionStatus.Pending)
					{
						Interlocked.CompareExchange(ref this.m_CallNesting, 0, 4);
					}
				}
			}
			if (webExceptionStatus != WebExceptionStatus.Pending)
			{
				this.m_Request.WriteHeadersCallback(webExceptionStatus, this, async);
			}
		}

		// Token: 0x06002649 RID: 9801 RVA: 0x0009BA48 File Offset: 0x0009AA48
		internal ChannelBinding GetChannelBinding(ChannelBindingKind kind)
		{
			ChannelBinding channelBinding = null;
			TlsStream tlsStream = this.m_Connection.NetworkStream as TlsStream;
			if (tlsStream != null)
			{
				channelBinding = tlsStream.GetChannelBinding(kind);
			}
			return channelBinding;
		}

		// Token: 0x0600264A RID: 9802 RVA: 0x0009BA74 File Offset: 0x0009AA74
		internal void PollAndRead(bool userRetrievedStream)
		{
			this.m_Connection.PollAndRead(this.m_Request, userRetrievedStream);
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x0009BA88 File Offset: 0x0009AA88
		private void SafeSetSocketTimeout(SocketShutdown mode)
		{
			if (this.Eof)
			{
				return;
			}
			int num;
			if (mode == SocketShutdown.Receive)
			{
				num = this.ReadTimeout;
			}
			else
			{
				num = this.WriteTimeout;
			}
			Connection connection = this.m_Connection;
			if (connection != null)
			{
				NetworkStream networkStream = connection.NetworkStream;
				if (networkStream != null)
				{
					networkStream.SetSocketTimeoutOption(mode, num, false);
				}
			}
		}

		// Token: 0x0600264C RID: 9804 RVA: 0x0009BAD0 File Offset: 0x0009AAD0
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (Logging.On)
				{
					Logging.Enter(Logging.Web, this, "Close", "");
				}
				((ICloseEx)this).CloseEx(CloseExState.Normal);
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "Close", "");
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x0600264D RID: 9805 RVA: 0x0009BB38 File Offset: 0x0009AB38
		internal void CloseInternal(bool internalCall)
		{
			((ICloseEx)this).CloseEx(internalCall ? CloseExState.Silent : CloseExState.Normal);
		}

		// Token: 0x0600264E RID: 9806 RVA: 0x0009BB47 File Offset: 0x0009AB47
		void ICloseEx.CloseEx(CloseExState closeState)
		{
			this.CloseInternal((closeState & CloseExState.Silent) != CloseExState.Normal, (closeState & CloseExState.Abort) != CloseExState.Normal);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x0009BB68 File Offset: 0x0009AB68
		private void ResumeInternalClose(LazyAsyncResult userResult)
		{
			if (this.WriteChunked && !this.ErrorInStream && !this.m_IgnoreSocketErrors)
			{
				this.m_IgnoreSocketErrors = true;
				try
				{
					if (userResult != null)
					{
						this.m_Connection.BeginWrite(NclConstants.ChunkTerminator, 0, NclConstants.ChunkTerminator.Length, new AsyncCallback(this.ResumeClose_Part2_Wrapper), userResult);
						return;
					}
					this.SafeSetSocketTimeout(SocketShutdown.Send);
					this.m_Connection.Write(NclConstants.ChunkTerminator, 0, NclConstants.ChunkTerminator.Length);
				}
				catch (Exception)
				{
				}
			}
			this.ResumeClose_Part2(userResult);
		}

		// Token: 0x06002650 RID: 9808 RVA: 0x0009BBFC File Offset: 0x0009ABFC
		private void ResumeClose_Part2_Wrapper(IAsyncResult ar)
		{
			try
			{
				this.m_Connection.EndWrite(ar);
			}
			catch (Exception)
			{
			}
			this.ResumeClose_Part2((LazyAsyncResult)ar.AsyncState);
		}

		// Token: 0x06002651 RID: 9809 RVA: 0x0009BC3C File Offset: 0x0009AC3C
		private void ResumeClose_Part2(LazyAsyncResult userResult)
		{
			try
			{
				try
				{
					if (this.ErrorInStream)
					{
						this.m_Connection.AbortSocket(true);
					}
				}
				finally
				{
					this.CallDone();
				}
			}
			catch
			{
			}
			finally
			{
				if (userResult != null)
				{
					userResult.InvokeCallback();
				}
			}
		}

		// Token: 0x06002652 RID: 9810 RVA: 0x0009BCA0 File Offset: 0x0009ACA0
		private void CloseInternal(bool internalCall, bool aborting)
		{
			bool flag = !aborting;
			Exception ex = null;
			if (aborting)
			{
				if (Interlocked.Exchange(ref this.m_ShutDown, 777777) >= 777777)
				{
					return;
				}
			}
			else if (Interlocked.Increment(ref this.m_ShutDown) > 1)
			{
				return;
			}
			int num = ((this.IsPostStream && internalCall && !this.IgnoreSocketErrors && !this.BufferOnly && flag && !NclUtilities.HasShutdownStarted) ? 2 : 3);
			if (Interlocked.Exchange(ref this.m_CallNesting, num) == 1)
			{
				if (num == 2)
				{
					return;
				}
				flag &= !NclUtilities.HasShutdownStarted;
			}
			if (this.IgnoreSocketErrors && this.IsPostStream && !internalCall)
			{
				this.m_BytesLeftToWrite = 0L;
			}
			if (!this.IgnoreSocketErrors && flag)
			{
				if (!this.WriteStream)
				{
					flag = this.DrainSocket();
				}
				else
				{
					try
					{
						if (!this.ErrorInStream)
						{
							if (this.WriteChunked)
							{
								try
								{
									if (!this.m_IgnoreSocketErrors)
									{
										this.m_IgnoreSocketErrors = true;
										this.SafeSetSocketTimeout(SocketShutdown.Send);
										this.m_Connection.Write(NclConstants.ChunkTerminator, 0, NclConstants.ChunkTerminator.Length);
									}
								}
								catch
								{
								}
								this.m_BytesLeftToWrite = 0L;
							}
							else
							{
								if (this.BytesLeftToWrite > 0L)
								{
									throw new IOException(SR.GetString("net_io_notenoughbyteswritten"));
								}
								if (this.BufferOnly)
								{
									this.m_BytesLeftToWrite = (long)this.BufferedData.Length;
									this.m_Request.SwitchToContentLength();
									this.SafeSetSocketTimeout(SocketShutdown.Send);
									this.m_Request.NeedEndSubmitRequest();
									return;
								}
							}
						}
						else
						{
							flag = false;
						}
					}
					catch (Exception ex2)
					{
						flag = false;
						if (NclUtilities.IsFatal(ex2))
						{
							this.m_ErrorException = ex2;
							throw;
						}
						ex = new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), ex2, WebExceptionStatus.RequestCanceled, null);
					}
				}
			}
			if (!flag && this.m_DoneCalled == 0)
			{
				if (!aborting && Interlocked.Exchange(ref this.m_ShutDown, 777777) >= 777777)
				{
					return;
				}
				this.m_ErrorException = new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
				this.m_Connection.AbortSocket(true);
				if (this.WriteStream)
				{
					HttpWebRequest request = this.m_Request;
					if (request != null)
					{
						request.Abort();
					}
				}
				if (ex != null)
				{
					this.CallDone();
					if (!internalCall)
					{
						throw ex;
					}
				}
			}
			this.CallDone();
		}

		// Token: 0x06002653 RID: 9811 RVA: 0x0009BED0 File Offset: 0x0009AED0
		public override void Flush()
		{
		}

		// Token: 0x06002654 RID: 9812 RVA: 0x0009BED2 File Offset: 0x0009AED2
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06002655 RID: 9813 RVA: 0x0009BEE3 File Offset: 0x0009AEE3
		public override void SetLength(long value)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06002656 RID: 9814 RVA: 0x0009BEF4 File Offset: 0x0009AEF4
		private bool DrainSocket()
		{
			if (this.IgnoreSocketErrors)
			{
				return true;
			}
			long readBytes = this.m_ReadBytes;
			if (!this.m_Chunked)
			{
				if (this.m_ReadBufferSize != 0)
				{
					this.m_ReadOffset += this.m_ReadBufferSize;
					if (this.m_ReadBytes != -1L)
					{
						this.m_ReadBytes -= (long)this.m_ReadBufferSize;
						if (this.m_ReadBytes < 0L)
						{
							this.m_ReadBytes = 0L;
							return false;
						}
					}
					this.m_ReadBufferSize = 0;
					this.m_ReadBuffer = null;
				}
				if (readBytes == -1L)
				{
					return true;
				}
			}
			if (this.Eof)
			{
				return true;
			}
			if (this.m_ReadBytes > 65536L)
			{
				this.m_Connection.AbortSocket(false);
				return true;
			}
			this.m_Draining = true;
			int num;
			try
			{
				do
				{
					num = this.ReadWithoutValidation(ConnectStream.s_DrainingBuffer, 0, ConnectStream.s_DrainingBuffer.Length, false);
				}
				while (num > 0);
			}
			catch (Exception ex)
			{
				if (NclUtilities.IsFatal(ex))
				{
					throw;
				}
				num = -1;
			}
			return num > 0;
		}

		// Token: 0x06002657 RID: 9815 RVA: 0x0009BFEC File Offset: 0x0009AFEC
		private void IOError(Exception exception)
		{
			this.IOError(exception, true);
		}

		// Token: 0x06002658 RID: 9816 RVA: 0x0009BFF8 File Offset: 0x0009AFF8
		private void IOError(Exception exception, bool willThrow)
		{
			if (this.m_ErrorException == null)
			{
				if (exception == null)
				{
					string text;
					if (!this.WriteStream)
					{
						text = SR.GetString("net_io_readfailure", new object[] { SR.GetString("net_io_connectionclosed") });
					}
					else
					{
						text = SR.GetString("net_io_writefailure", new object[] { SR.GetString("net_io_connectionclosed") });
					}
					Interlocked.CompareExchange<Exception>(ref this.m_ErrorException, new IOException(text), null);
				}
				else
				{
					willThrow &= Interlocked.CompareExchange<Exception>(ref this.m_ErrorException, exception, null) != null;
				}
			}
			this.m_ChunkEofRecvd = true;
			ConnectionReturnResult connectionReturnResult = null;
			if (this.WriteStream)
			{
				this.m_Connection.HandleConnectStreamException(true, false, WebExceptionStatus.SendFailure, ref connectionReturnResult, this.m_ErrorException);
			}
			else
			{
				this.m_Connection.HandleConnectStreamException(false, true, WebExceptionStatus.ReceiveFailure, ref connectionReturnResult, this.m_ErrorException);
			}
			this.CallDone(connectionReturnResult);
			if (willThrow)
			{
				throw this.m_ErrorException;
			}
		}

		// Token: 0x06002659 RID: 9817 RVA: 0x0009C0D8 File Offset: 0x0009B0D8
		internal static byte[] GetChunkHeader(int size, out int offset)
		{
			uint num = 4026531840U;
			byte[] array = new byte[10];
			offset = -1;
			int i = 0;
			while (i < 8)
			{
				if (offset != -1 || ((long)size & (long)((ulong)num)) != 0L)
				{
					uint num2 = (uint)size >> 28;
					if (num2 < 10U)
					{
						array[i] = (byte)(num2 + 48U);
					}
					else
					{
						array[i] = (byte)(num2 - 10U + 65U);
					}
					if (offset == -1)
					{
						offset = i;
					}
				}
				i++;
				size <<= 4;
			}
			array[8] = 13;
			array[9] = 10;
			return array;
		}

		// Token: 0x040025C0 RID: 9664
		private const long c_MaxDrainBytes = 65536L;

		// Token: 0x040025C1 RID: 9665
		private const int AlreadyAborted = 777777;

		// Token: 0x040025C2 RID: 9666
		private int m_CallNesting;

		// Token: 0x040025C3 RID: 9667
		private ScatterGatherBuffers m_BufferedData;

		// Token: 0x040025C4 RID: 9668
		private bool m_SuppressWrite;

		// Token: 0x040025C5 RID: 9669
		private bool m_BufferOnly;

		// Token: 0x040025C6 RID: 9670
		private long m_BytesLeftToWrite;

		// Token: 0x040025C7 RID: 9671
		private int m_BytesAlreadyTransferred;

		// Token: 0x040025C8 RID: 9672
		private Connection m_Connection;

		// Token: 0x040025C9 RID: 9673
		private byte[] m_ReadBuffer;

		// Token: 0x040025CA RID: 9674
		private int m_ReadOffset;

		// Token: 0x040025CB RID: 9675
		private int m_ReadBufferSize;

		// Token: 0x040025CC RID: 9676
		private long m_ReadBytes;

		// Token: 0x040025CD RID: 9677
		private bool m_Chunked;

		// Token: 0x040025CE RID: 9678
		private int m_DoneCalled;

		// Token: 0x040025CF RID: 9679
		private int m_ShutDown;

		// Token: 0x040025D0 RID: 9680
		private Exception m_ErrorException;

		// Token: 0x040025D1 RID: 9681
		private bool m_ChunkEofRecvd;

		// Token: 0x040025D2 RID: 9682
		private int m_ChunkSize;

		// Token: 0x040025D3 RID: 9683
		private byte[] m_TempBuffer;

		// Token: 0x040025D4 RID: 9684
		private bool m_ChunkedNeedCRLFRead;

		// Token: 0x040025D5 RID: 9685
		private bool m_Draining;

		// Token: 0x040025D6 RID: 9686
		private HttpWriteMode m_HttpWriteMode;

		// Token: 0x040025D7 RID: 9687
		private int m_ReadTimeout;

		// Token: 0x040025D8 RID: 9688
		private int m_WriteTimeout;

		// Token: 0x040025D9 RID: 9689
		private static readonly WaitCallback m_ReadChunkedCallbackDelegate = new WaitCallback(ConnectStream.ReadChunkedCallback);

		// Token: 0x040025DA RID: 9690
		private static readonly AsyncCallback m_ReadCallbackDelegate = new AsyncCallback(ConnectStream.ReadCallback);

		// Token: 0x040025DB RID: 9691
		private static readonly AsyncCallback m_WriteCallbackDelegate = new AsyncCallback(ConnectStream.WriteCallback);

		// Token: 0x040025DC RID: 9692
		private static readonly AsyncCallback m_WriteHeadersCallback = new AsyncCallback(ConnectStream.WriteHeadersCallback);

		// Token: 0x040025DD RID: 9693
		private static readonly object ZeroLengthRead = new object();

		// Token: 0x040025DE RID: 9694
		private HttpWebRequest m_Request;

		// Token: 0x040025DF RID: 9695
		private bool m_IgnoreSocketErrors;

		// Token: 0x040025E0 RID: 9696
		private bool m_ErrorResponseStatus;

		// Token: 0x040025E1 RID: 9697
		internal static byte[] s_DrainingBuffer = new byte[4096];

		// Token: 0x020004D0 RID: 1232
		private static class Nesting
		{
			// Token: 0x040025E2 RID: 9698
			public const int Idle = 0;

			// Token: 0x040025E3 RID: 9699
			public const int IoInProgress = 1;

			// Token: 0x040025E4 RID: 9700
			public const int Closed = 2;

			// Token: 0x040025E5 RID: 9701
			public const int InError = 3;

			// Token: 0x040025E6 RID: 9702
			public const int InternalIO = 4;
		}
	}
}
