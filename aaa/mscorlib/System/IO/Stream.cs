using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.IO
{
	// Token: 0x02000588 RID: 1416
	[ComVisible(true)]
	[Serializable]
	public abstract class Stream : MarshalByRefObject, IDisposable
	{
		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06003424 RID: 13348
		public abstract bool CanRead { get; }

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06003425 RID: 13349
		public abstract bool CanSeek { get; }

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06003426 RID: 13350 RVA: 0x000AD8FE File Offset: 0x000AC8FE
		[ComVisible(false)]
		public virtual bool CanTimeout
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06003427 RID: 13351
		public abstract bool CanWrite { get; }

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06003428 RID: 13352
		public abstract long Length { get; }

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06003429 RID: 13353
		// (set) Token: 0x0600342A RID: 13354
		public abstract long Position { get; set; }

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x0600342B RID: 13355 RVA: 0x000AD901 File Offset: 0x000AC901
		// (set) Token: 0x0600342C RID: 13356 RVA: 0x000AD912 File Offset: 0x000AC912
		[ComVisible(false)]
		public virtual int ReadTimeout
		{
			get
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_TimeoutsNotSupported"));
			}
			set
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_TimeoutsNotSupported"));
			}
		}

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x0600342D RID: 13357 RVA: 0x000AD923 File Offset: 0x000AC923
		// (set) Token: 0x0600342E RID: 13358 RVA: 0x000AD934 File Offset: 0x000AC934
		[ComVisible(false)]
		public virtual int WriteTimeout
		{
			get
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_TimeoutsNotSupported"));
			}
			set
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_TimeoutsNotSupported"));
			}
		}

		// Token: 0x0600342F RID: 13359 RVA: 0x000AD945 File Offset: 0x000AC945
		public virtual void Close()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003430 RID: 13360 RVA: 0x000AD954 File Offset: 0x000AC954
		public void Dispose()
		{
			this.Close();
		}

		// Token: 0x06003431 RID: 13361 RVA: 0x000AD95C File Offset: 0x000AC95C
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this._asyncActiveEvent != null)
			{
				this._CloseAsyncActiveEvent(Interlocked.Decrement(ref this._asyncActiveCount));
			}
		}

		// Token: 0x06003432 RID: 13362 RVA: 0x000AD97A File Offset: 0x000AC97A
		private void _CloseAsyncActiveEvent(int asyncActiveCount)
		{
			if (this._asyncActiveEvent != null && asyncActiveCount == 0)
			{
				this._asyncActiveEvent.Close();
				this._asyncActiveEvent = null;
			}
		}

		// Token: 0x06003433 RID: 13363
		public abstract void Flush();

		// Token: 0x06003434 RID: 13364 RVA: 0x000AD999 File Offset: 0x000AC999
		[Obsolete("CreateWaitHandle will be removed eventually.  Please use \"new ManualResetEvent(false)\" instead.")]
		protected virtual WaitHandle CreateWaitHandle()
		{
			return new ManualResetEvent(false);
		}

		// Token: 0x06003435 RID: 13365 RVA: 0x000AD9A4 File Offset: 0x000AC9A4
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (!this.CanRead)
			{
				__Error.ReadNotSupported();
			}
			Interlocked.Increment(ref this._asyncActiveCount);
			Stream.ReadDelegate readDelegate = new Stream.ReadDelegate(this.Read);
			if (this._asyncActiveEvent == null)
			{
				lock (this)
				{
					if (this._asyncActiveEvent == null)
					{
						this._asyncActiveEvent = new AutoResetEvent(true);
					}
				}
			}
			this._asyncActiveEvent.WaitOne();
			this._readDelegate = readDelegate;
			return readDelegate.BeginInvoke(buffer, offset, count, callback, state);
		}

		// Token: 0x06003436 RID: 13366 RVA: 0x000ADA38 File Offset: 0x000ACA38
		public virtual int EndRead(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (this._readDelegate == null)
			{
				throw new ArgumentException(Environment.GetResourceString("InvalidOperation_WrongAsyncResultOrEndReadCalledMultiple"));
			}
			int num = -1;
			try
			{
				num = this._readDelegate.EndInvoke(asyncResult);
			}
			finally
			{
				this._readDelegate = null;
				this._asyncActiveEvent.Set();
				this._CloseAsyncActiveEvent(Interlocked.Decrement(ref this._asyncActiveCount));
			}
			return num;
		}

		// Token: 0x06003437 RID: 13367 RVA: 0x000ADAB4 File Offset: 0x000ACAB4
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public virtual IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (!this.CanWrite)
			{
				__Error.WriteNotSupported();
			}
			Interlocked.Increment(ref this._asyncActiveCount);
			Stream.WriteDelegate writeDelegate = new Stream.WriteDelegate(this.Write);
			if (this._asyncActiveEvent == null)
			{
				lock (this)
				{
					if (this._asyncActiveEvent == null)
					{
						this._asyncActiveEvent = new AutoResetEvent(true);
					}
				}
			}
			this._asyncActiveEvent.WaitOne();
			this._writeDelegate = writeDelegate;
			return writeDelegate.BeginInvoke(buffer, offset, count, callback, state);
		}

		// Token: 0x06003438 RID: 13368 RVA: 0x000ADB48 File Offset: 0x000ACB48
		public virtual void EndWrite(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (this._writeDelegate == null)
			{
				throw new ArgumentException(Environment.GetResourceString("InvalidOperation_WrongAsyncResultOrEndWriteCalledMultiple"));
			}
			try
			{
				this._writeDelegate.EndInvoke(asyncResult);
			}
			finally
			{
				this._writeDelegate = null;
				this._asyncActiveEvent.Set();
				this._CloseAsyncActiveEvent(Interlocked.Decrement(ref this._asyncActiveCount));
			}
		}

		// Token: 0x06003439 RID: 13369
		public abstract long Seek(long offset, SeekOrigin origin);

		// Token: 0x0600343A RID: 13370
		public abstract void SetLength(long value);

		// Token: 0x0600343B RID: 13371
		public abstract int Read([In] [Out] byte[] buffer, int offset, int count);

		// Token: 0x0600343C RID: 13372 RVA: 0x000ADBC0 File Offset: 0x000ACBC0
		public virtual int ReadByte()
		{
			byte[] array = new byte[1];
			if (this.Read(array, 0, 1) == 0)
			{
				return -1;
			}
			return (int)array[0];
		}

		// Token: 0x0600343D RID: 13373
		public abstract void Write(byte[] buffer, int offset, int count);

		// Token: 0x0600343E RID: 13374 RVA: 0x000ADBE8 File Offset: 0x000ACBE8
		public virtual void WriteByte(byte value)
		{
			this.Write(new byte[] { value }, 0, 1);
		}

		// Token: 0x0600343F RID: 13375 RVA: 0x000ADC09 File Offset: 0x000ACC09
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public static Stream Synchronized(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (stream is Stream.SyncStream)
			{
				return stream;
			}
			return new Stream.SyncStream(stream);
		}

		// Token: 0x04001B8F RID: 7055
		public static readonly Stream Null = new Stream.NullStream();

		// Token: 0x04001B90 RID: 7056
		[NonSerialized]
		private Stream.ReadDelegate _readDelegate;

		// Token: 0x04001B91 RID: 7057
		[NonSerialized]
		private Stream.WriteDelegate _writeDelegate;

		// Token: 0x04001B92 RID: 7058
		[NonSerialized]
		private AutoResetEvent _asyncActiveEvent;

		// Token: 0x04001B93 RID: 7059
		[NonSerialized]
		private int _asyncActiveCount = 1;

		// Token: 0x02000589 RID: 1417
		// (Invoke) Token: 0x06003443 RID: 13379
		private delegate int ReadDelegate([In] [Out] byte[] bytes, int index, int offset);

		// Token: 0x0200058A RID: 1418
		// (Invoke) Token: 0x06003447 RID: 13383
		private delegate void WriteDelegate(byte[] bytes, int index, int offset);

		// Token: 0x0200058B RID: 1419
		[Serializable]
		private sealed class NullStream : Stream
		{
			// Token: 0x0600344A RID: 13386 RVA: 0x000ADC44 File Offset: 0x000ACC44
			internal NullStream()
			{
			}

			// Token: 0x170008EF RID: 2287
			// (get) Token: 0x0600344B RID: 13387 RVA: 0x000ADC4C File Offset: 0x000ACC4C
			public override bool CanRead
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170008F0 RID: 2288
			// (get) Token: 0x0600344C RID: 13388 RVA: 0x000ADC4F File Offset: 0x000ACC4F
			public override bool CanWrite
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170008F1 RID: 2289
			// (get) Token: 0x0600344D RID: 13389 RVA: 0x000ADC52 File Offset: 0x000ACC52
			public override bool CanSeek
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170008F2 RID: 2290
			// (get) Token: 0x0600344E RID: 13390 RVA: 0x000ADC55 File Offset: 0x000ACC55
			public override long Length
			{
				get
				{
					return 0L;
				}
			}

			// Token: 0x170008F3 RID: 2291
			// (get) Token: 0x0600344F RID: 13391 RVA: 0x000ADC59 File Offset: 0x000ACC59
			// (set) Token: 0x06003450 RID: 13392 RVA: 0x000ADC5D File Offset: 0x000ACC5D
			public override long Position
			{
				get
				{
					return 0L;
				}
				set
				{
				}
			}

			// Token: 0x06003451 RID: 13393 RVA: 0x000ADC5F File Offset: 0x000ACC5F
			public override void Flush()
			{
			}

			// Token: 0x06003452 RID: 13394 RVA: 0x000ADC61 File Offset: 0x000ACC61
			public override int Read([In] [Out] byte[] buffer, int offset, int count)
			{
				return 0;
			}

			// Token: 0x06003453 RID: 13395 RVA: 0x000ADC64 File Offset: 0x000ACC64
			public override int ReadByte()
			{
				return -1;
			}

			// Token: 0x06003454 RID: 13396 RVA: 0x000ADC67 File Offset: 0x000ACC67
			public override void Write(byte[] buffer, int offset, int count)
			{
			}

			// Token: 0x06003455 RID: 13397 RVA: 0x000ADC69 File Offset: 0x000ACC69
			public override void WriteByte(byte value)
			{
			}

			// Token: 0x06003456 RID: 13398 RVA: 0x000ADC6B File Offset: 0x000ACC6B
			public override long Seek(long offset, SeekOrigin origin)
			{
				return 0L;
			}

			// Token: 0x06003457 RID: 13399 RVA: 0x000ADC6F File Offset: 0x000ACC6F
			public override void SetLength(long length)
			{
			}
		}

		// Token: 0x0200058C RID: 1420
		[Serializable]
		internal sealed class SyncStream : Stream, IDisposable
		{
			// Token: 0x06003458 RID: 13400 RVA: 0x000ADC71 File Offset: 0x000ACC71
			internal SyncStream(Stream stream)
			{
				if (stream == null)
				{
					throw new ArgumentNullException("stream");
				}
				this._stream = stream;
			}

			// Token: 0x170008F4 RID: 2292
			// (get) Token: 0x06003459 RID: 13401 RVA: 0x000ADC8E File Offset: 0x000ACC8E
			public override bool CanRead
			{
				get
				{
					return this._stream.CanRead;
				}
			}

			// Token: 0x170008F5 RID: 2293
			// (get) Token: 0x0600345A RID: 13402 RVA: 0x000ADC9B File Offset: 0x000ACC9B
			public override bool CanWrite
			{
				get
				{
					return this._stream.CanWrite;
				}
			}

			// Token: 0x170008F6 RID: 2294
			// (get) Token: 0x0600345B RID: 13403 RVA: 0x000ADCA8 File Offset: 0x000ACCA8
			public override bool CanSeek
			{
				get
				{
					return this._stream.CanSeek;
				}
			}

			// Token: 0x170008F7 RID: 2295
			// (get) Token: 0x0600345C RID: 13404 RVA: 0x000ADCB5 File Offset: 0x000ACCB5
			[ComVisible(false)]
			public override bool CanTimeout
			{
				get
				{
					return this._stream.CanTimeout;
				}
			}

			// Token: 0x170008F8 RID: 2296
			// (get) Token: 0x0600345D RID: 13405 RVA: 0x000ADCC4 File Offset: 0x000ACCC4
			public override long Length
			{
				get
				{
					long length;
					lock (this._stream)
					{
						length = this._stream.Length;
					}
					return length;
				}
			}

			// Token: 0x170008F9 RID: 2297
			// (get) Token: 0x0600345E RID: 13406 RVA: 0x000ADD04 File Offset: 0x000ACD04
			// (set) Token: 0x0600345F RID: 13407 RVA: 0x000ADD44 File Offset: 0x000ACD44
			public override long Position
			{
				get
				{
					long position;
					lock (this._stream)
					{
						position = this._stream.Position;
					}
					return position;
				}
				set
				{
					lock (this._stream)
					{
						this._stream.Position = value;
					}
				}
			}

			// Token: 0x170008FA RID: 2298
			// (get) Token: 0x06003460 RID: 13408 RVA: 0x000ADD84 File Offset: 0x000ACD84
			// (set) Token: 0x06003461 RID: 13409 RVA: 0x000ADD91 File Offset: 0x000ACD91
			[ComVisible(false)]
			public override int ReadTimeout
			{
				get
				{
					return this._stream.ReadTimeout;
				}
				set
				{
					this._stream.ReadTimeout = value;
				}
			}

			// Token: 0x170008FB RID: 2299
			// (get) Token: 0x06003462 RID: 13410 RVA: 0x000ADD9F File Offset: 0x000ACD9F
			// (set) Token: 0x06003463 RID: 13411 RVA: 0x000ADDAC File Offset: 0x000ACDAC
			[ComVisible(false)]
			public override int WriteTimeout
			{
				get
				{
					return this._stream.WriteTimeout;
				}
				set
				{
					this._stream.WriteTimeout = value;
				}
			}

			// Token: 0x06003464 RID: 13412 RVA: 0x000ADDBC File Offset: 0x000ACDBC
			public override void Close()
			{
				lock (this._stream)
				{
					try
					{
						this._stream.Close();
					}
					finally
					{
						base.Dispose(true);
					}
				}
			}

			// Token: 0x06003465 RID: 13413 RVA: 0x000ADE10 File Offset: 0x000ACE10
			protected override void Dispose(bool disposing)
			{
				lock (this._stream)
				{
					try
					{
						if (disposing)
						{
							((IDisposable)this._stream).Dispose();
						}
					}
					finally
					{
						base.Dispose(disposing);
					}
				}
			}

			// Token: 0x06003466 RID: 13414 RVA: 0x000ADE68 File Offset: 0x000ACE68
			public override void Flush()
			{
				lock (this._stream)
				{
					this._stream.Flush();
				}
			}

			// Token: 0x06003467 RID: 13415 RVA: 0x000ADEA8 File Offset: 0x000ACEA8
			public override int Read([In] [Out] byte[] bytes, int offset, int count)
			{
				int num;
				lock (this._stream)
				{
					num = this._stream.Read(bytes, offset, count);
				}
				return num;
			}

			// Token: 0x06003468 RID: 13416 RVA: 0x000ADEEC File Offset: 0x000ACEEC
			public override int ReadByte()
			{
				int num;
				lock (this._stream)
				{
					num = this._stream.ReadByte();
				}
				return num;
			}

			// Token: 0x06003469 RID: 13417 RVA: 0x000ADF2C File Offset: 0x000ACF2C
			[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
			public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				IAsyncResult asyncResult;
				lock (this._stream)
				{
					asyncResult = this._stream.BeginRead(buffer, offset, count, callback, state);
				}
				return asyncResult;
			}

			// Token: 0x0600346A RID: 13418 RVA: 0x000ADF74 File Offset: 0x000ACF74
			public override int EndRead(IAsyncResult asyncResult)
			{
				int num;
				lock (this._stream)
				{
					num = this._stream.EndRead(asyncResult);
				}
				return num;
			}

			// Token: 0x0600346B RID: 13419 RVA: 0x000ADFB8 File Offset: 0x000ACFB8
			public override long Seek(long offset, SeekOrigin origin)
			{
				long num;
				lock (this._stream)
				{
					num = this._stream.Seek(offset, origin);
				}
				return num;
			}

			// Token: 0x0600346C RID: 13420 RVA: 0x000ADFFC File Offset: 0x000ACFFC
			public override void SetLength(long length)
			{
				lock (this._stream)
				{
					this._stream.SetLength(length);
				}
			}

			// Token: 0x0600346D RID: 13421 RVA: 0x000AE03C File Offset: 0x000AD03C
			public override void Write(byte[] bytes, int offset, int count)
			{
				lock (this._stream)
				{
					this._stream.Write(bytes, offset, count);
				}
			}

			// Token: 0x0600346E RID: 13422 RVA: 0x000AE080 File Offset: 0x000AD080
			public override void WriteByte(byte b)
			{
				lock (this._stream)
				{
					this._stream.WriteByte(b);
				}
			}

			// Token: 0x0600346F RID: 13423 RVA: 0x000AE0C0 File Offset: 0x000AD0C0
			[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
			public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				IAsyncResult asyncResult;
				lock (this._stream)
				{
					asyncResult = this._stream.BeginWrite(buffer, offset, count, callback, state);
				}
				return asyncResult;
			}

			// Token: 0x06003470 RID: 13424 RVA: 0x000AE108 File Offset: 0x000AD108
			public override void EndWrite(IAsyncResult asyncResult)
			{
				lock (this._stream)
				{
					this._stream.EndWrite(asyncResult);
				}
			}

			// Token: 0x04001B94 RID: 7060
			private Stream _stream;
		}
	}
}
