using System;
using System.Security.Permissions;
using System.Threading;

namespace System.IO.Compression
{
	// Token: 0x02000201 RID: 513
	public class DeflateStream : Stream
	{
		// Token: 0x06001168 RID: 4456 RVA: 0x00038D95 File Offset: 0x00037D95
		public DeflateStream(Stream stream, CompressionMode mode)
			: this(stream, mode, false, false)
		{
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x00038DA1 File Offset: 0x00037DA1
		public DeflateStream(Stream stream, CompressionMode mode, bool leaveOpen)
			: this(stream, mode, leaveOpen, false)
		{
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x00038DB0 File Offset: 0x00037DB0
		internal DeflateStream(Stream stream, CompressionMode mode, bool leaveOpen, bool usingGZip)
		{
			this._stream = stream;
			this._mode = mode;
			this._leaveOpen = leaveOpen;
			if (this._stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			switch (this._mode)
			{
			case CompressionMode.Decompress:
				if (!this._stream.CanRead)
				{
					throw new ArgumentException(SR.GetString("NotReadableStream"), "stream");
				}
				this.inflater = new Inflater(usingGZip);
				this.m_CallBack = new AsyncCallback(this.ReadCallback);
				break;
			case CompressionMode.Compress:
				if (!this._stream.CanWrite)
				{
					throw new ArgumentException(SR.GetString("NotWriteableStream"), "stream");
				}
				this.deflater = new Deflater(usingGZip);
				this.m_AsyncWriterDelegate = new DeflateStream.AsyncWriteDelegate(this.InternalWrite);
				this.m_CallBack = new AsyncCallback(this.WriteCallback);
				break;
			default:
				throw new ArgumentException(SR.GetString("ArgumentOutOfRange_Enum"), "mode");
			}
			this.buffer = new byte[4096];
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x0600116B RID: 4459 RVA: 0x00038EC2 File Offset: 0x00037EC2
		public override bool CanRead
		{
			get
			{
				return this._stream != null && this._mode == CompressionMode.Decompress && this._stream.CanRead;
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x0600116C RID: 4460 RVA: 0x00038EE3 File Offset: 0x00037EE3
		public override bool CanWrite
		{
			get
			{
				return this._stream != null && this._mode == CompressionMode.Compress && this._stream.CanWrite;
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x0600116D RID: 4461 RVA: 0x00038F05 File Offset: 0x00037F05
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x0600116E RID: 4462 RVA: 0x00038F08 File Offset: 0x00037F08
		public override long Length
		{
			get
			{
				throw new NotSupportedException(SR.GetString("NotSupported"));
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x0600116F RID: 4463 RVA: 0x00038F19 File Offset: 0x00037F19
		// (set) Token: 0x06001170 RID: 4464 RVA: 0x00038F2A File Offset: 0x00037F2A
		public override long Position
		{
			get
			{
				throw new NotSupportedException(SR.GetString("NotSupported"));
			}
			set
			{
				throw new NotSupportedException(SR.GetString("NotSupported"));
			}
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x00038F3B File Offset: 0x00037F3B
		public override void Flush()
		{
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, SR.GetString("ObjectDisposed_StreamClosed"));
			}
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x00038F56 File Offset: 0x00037F56
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(SR.GetString("NotSupported"));
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x00038F67 File Offset: 0x00037F67
		public override void SetLength(long value)
		{
			throw new NotSupportedException(SR.GetString("NotSupported"));
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x00038F78 File Offset: 0x00037F78
		public override int Read(byte[] array, int offset, int count)
		{
			this.EnsureDecompressionMode();
			this.ValidateParameters(array, offset, count);
			int num = offset;
			int num2 = count;
			for (;;)
			{
				int num3 = this.inflater.Inflate(array, num, num2);
				num += num3;
				num2 -= num3;
				if (num2 == 0 || this.inflater.Finished())
				{
					break;
				}
				int num4 = this._stream.Read(this.buffer, 0, this.buffer.Length);
				if (num4 == 0)
				{
					break;
				}
				this.inflater.SetInput(this.buffer, 0, num4);
			}
			return count - num2;
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x00038FF8 File Offset: 0x00037FF8
		private void ValidateParameters(byte[] array, int offset, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (array.Length - offset < count)
			{
				throw new ArgumentException(SR.GetString("InvalidArgumentOffsetCount"));
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, SR.GetString("ObjectDisposed_StreamClosed"));
			}
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x00039062 File Offset: 0x00038062
		private void EnsureDecompressionMode()
		{
			if (this._mode != CompressionMode.Decompress)
			{
				throw new InvalidOperationException(SR.GetString("CannotReadFromDeflateStream"));
			}
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x0003907C File Offset: 0x0003807C
		private void EnsureCompressionMode()
		{
			if (this._mode != CompressionMode.Compress)
			{
				throw new InvalidOperationException(SR.GetString("CannotWriteToDeflateStream"));
			}
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x00039098 File Offset: 0x00038098
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginRead(byte[] array, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			this.EnsureDecompressionMode();
			if (this.asyncOperations != 0)
			{
				throw new InvalidOperationException(SR.GetString("InvalidBeginCall"));
			}
			Interlocked.Increment(ref this.asyncOperations);
			IAsyncResult asyncResult;
			try
			{
				this.ValidateParameters(array, offset, count);
				DeflateStreamAsyncResult deflateStreamAsyncResult = new DeflateStreamAsyncResult(this, asyncState, asyncCallback, array, offset, count);
				deflateStreamAsyncResult.isWrite = false;
				int num = this.inflater.Inflate(array, offset, count);
				if (num != 0)
				{
					deflateStreamAsyncResult.InvokeCallback(true, num);
					asyncResult = deflateStreamAsyncResult;
				}
				else if (this.inflater.Finished())
				{
					deflateStreamAsyncResult.InvokeCallback(true, 0);
					asyncResult = deflateStreamAsyncResult;
				}
				else
				{
					this._stream.BeginRead(this.buffer, 0, this.buffer.Length, this.m_CallBack, deflateStreamAsyncResult);
					deflateStreamAsyncResult.m_CompletedSynchronously &= deflateStreamAsyncResult.IsCompleted;
					asyncResult = deflateStreamAsyncResult;
				}
			}
			catch
			{
				Interlocked.Decrement(ref this.asyncOperations);
				throw;
			}
			return asyncResult;
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x00039188 File Offset: 0x00038188
		private void ReadCallback(IAsyncResult baseStreamResult)
		{
			DeflateStreamAsyncResult deflateStreamAsyncResult = (DeflateStreamAsyncResult)baseStreamResult.AsyncState;
			deflateStreamAsyncResult.m_CompletedSynchronously &= baseStreamResult.CompletedSynchronously;
			int num = 0;
			try
			{
				num = this._stream.EndRead(baseStreamResult);
			}
			catch (Exception ex)
			{
				deflateStreamAsyncResult.InvokeCallback(ex);
				return;
			}
			if (num <= 0)
			{
				deflateStreamAsyncResult.InvokeCallback(0);
			}
			else
			{
				this.inflater.SetInput(this.buffer, 0, num);
				num = this.inflater.Inflate(deflateStreamAsyncResult.buffer, deflateStreamAsyncResult.offset, deflateStreamAsyncResult.count);
				if (num == 0 && !this.inflater.Finished())
				{
					this._stream.BeginRead(this.buffer, 0, this.buffer.Length, this.m_CallBack, deflateStreamAsyncResult);
					return;
				}
				deflateStreamAsyncResult.InvokeCallback(num);
				return;
			}
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x00039260 File Offset: 0x00038260
		public override int EndRead(IAsyncResult asyncResult)
		{
			this.EnsureDecompressionMode();
			if (this.asyncOperations != 1)
			{
				throw new InvalidOperationException(SR.GetString("InvalidEndCall"));
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (this._stream == null)
			{
				throw new InvalidOperationException(SR.GetString("ObjectDisposed_StreamClosed"));
			}
			DeflateStreamAsyncResult deflateStreamAsyncResult = asyncResult as DeflateStreamAsyncResult;
			if (deflateStreamAsyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			try
			{
				if (!deflateStreamAsyncResult.IsCompleted)
				{
					deflateStreamAsyncResult.AsyncWaitHandle.WaitOne();
				}
			}
			finally
			{
				Interlocked.Decrement(ref this.asyncOperations);
				deflateStreamAsyncResult.Close();
			}
			if (deflateStreamAsyncResult.Result is Exception)
			{
				throw (Exception)deflateStreamAsyncResult.Result;
			}
			return (int)deflateStreamAsyncResult.Result;
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x00039324 File Offset: 0x00038324
		public override void Write(byte[] array, int offset, int count)
		{
			this.EnsureCompressionMode();
			this.ValidateParameters(array, offset, count);
			this.InternalWrite(array, offset, count, false);
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x00039340 File Offset: 0x00038340
		internal void InternalWrite(byte[] array, int offset, int count, bool isAsync)
		{
			while (!this.deflater.NeedsInput())
			{
				int num = this.deflater.GetDeflateOutput(this.buffer);
				if (num != 0)
				{
					if (isAsync)
					{
						IAsyncResult asyncResult = this._stream.BeginWrite(this.buffer, 0, num, null, null);
						this._stream.EndWrite(asyncResult);
					}
					else
					{
						this._stream.Write(this.buffer, 0, num);
					}
				}
			}
			this.deflater.SetInput(array, offset, count);
			while (!this.deflater.NeedsInput())
			{
				int num = this.deflater.GetDeflateOutput(this.buffer);
				if (num != 0)
				{
					if (isAsync)
					{
						IAsyncResult asyncResult2 = this._stream.BeginWrite(this.buffer, 0, num, null, null);
						this._stream.EndWrite(asyncResult2);
					}
					else
					{
						this._stream.Write(this.buffer, 0, num);
					}
				}
			}
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x0003941C File Offset: 0x0003841C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && this._stream != null)
				{
					this.Flush();
					if (this._mode == CompressionMode.Compress && this._stream != null)
					{
						int num;
						while (!this.deflater.NeedsInput())
						{
							num = this.deflater.GetDeflateOutput(this.buffer);
							if (num != 0)
							{
								this._stream.Write(this.buffer, 0, num);
							}
						}
						num = this.deflater.Finish(this.buffer);
						if (num > 0)
						{
							this._stream.Write(this.buffer, 0, num);
						}
					}
				}
			}
			finally
			{
				try
				{
					if (disposing && !this._leaveOpen && this._stream != null)
					{
						this._stream.Close();
					}
				}
				finally
				{
					this._stream = null;
					base.Dispose(disposing);
				}
			}
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x000394FC File Offset: 0x000384FC
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginWrite(byte[] array, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			this.EnsureCompressionMode();
			if (this.asyncOperations != 0)
			{
				throw new InvalidOperationException(SR.GetString("InvalidBeginCall"));
			}
			Interlocked.Increment(ref this.asyncOperations);
			IAsyncResult asyncResult;
			try
			{
				this.ValidateParameters(array, offset, count);
				DeflateStreamAsyncResult deflateStreamAsyncResult = new DeflateStreamAsyncResult(this, asyncState, asyncCallback, array, offset, count);
				deflateStreamAsyncResult.isWrite = true;
				this.m_AsyncWriterDelegate.BeginInvoke(array, offset, count, true, this.m_CallBack, deflateStreamAsyncResult);
				deflateStreamAsyncResult.m_CompletedSynchronously &= deflateStreamAsyncResult.IsCompleted;
				asyncResult = deflateStreamAsyncResult;
			}
			catch
			{
				Interlocked.Decrement(ref this.asyncOperations);
				throw;
			}
			return asyncResult;
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x000395A0 File Offset: 0x000385A0
		private void WriteCallback(IAsyncResult asyncResult)
		{
			DeflateStreamAsyncResult deflateStreamAsyncResult = (DeflateStreamAsyncResult)asyncResult.AsyncState;
			deflateStreamAsyncResult.m_CompletedSynchronously &= asyncResult.CompletedSynchronously;
			try
			{
				this.m_AsyncWriterDelegate.EndInvoke(asyncResult);
			}
			catch (Exception ex)
			{
				deflateStreamAsyncResult.InvokeCallback(ex);
				return;
			}
			deflateStreamAsyncResult.InvokeCallback(null);
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x000395FC File Offset: 0x000385FC
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.EnsureCompressionMode();
			if (this.asyncOperations != 1)
			{
				throw new InvalidOperationException(SR.GetString("InvalidEndCall"));
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (this._stream == null)
			{
				throw new InvalidOperationException(SR.GetString("ObjectDisposed_StreamClosed"));
			}
			DeflateStreamAsyncResult deflateStreamAsyncResult = asyncResult as DeflateStreamAsyncResult;
			if (deflateStreamAsyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			try
			{
				if (!deflateStreamAsyncResult.IsCompleted)
				{
					deflateStreamAsyncResult.AsyncWaitHandle.WaitOne();
				}
			}
			finally
			{
				Interlocked.Decrement(ref this.asyncOperations);
				deflateStreamAsyncResult.Close();
			}
			if (deflateStreamAsyncResult.Result is Exception)
			{
				throw (Exception)deflateStreamAsyncResult.Result;
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06001181 RID: 4481 RVA: 0x000396B8 File Offset: 0x000386B8
		public Stream BaseStream
		{
			get
			{
				return this._stream;
			}
		}

		// Token: 0x04000FC0 RID: 4032
		private const int bufferSize = 4096;

		// Token: 0x04000FC1 RID: 4033
		private Stream _stream;

		// Token: 0x04000FC2 RID: 4034
		private CompressionMode _mode;

		// Token: 0x04000FC3 RID: 4035
		private bool _leaveOpen;

		// Token: 0x04000FC4 RID: 4036
		private Inflater inflater;

		// Token: 0x04000FC5 RID: 4037
		private Deflater deflater;

		// Token: 0x04000FC6 RID: 4038
		private byte[] buffer;

		// Token: 0x04000FC7 RID: 4039
		private int asyncOperations;

		// Token: 0x04000FC8 RID: 4040
		private readonly AsyncCallback m_CallBack;

		// Token: 0x04000FC9 RID: 4041
		private readonly DeflateStream.AsyncWriteDelegate m_AsyncWriterDelegate;

		// Token: 0x02000202 RID: 514
		// (Invoke) Token: 0x06001183 RID: 4483
		internal delegate void AsyncWriteDelegate(byte[] array, int offset, int count, bool isAsync);
	}
}
