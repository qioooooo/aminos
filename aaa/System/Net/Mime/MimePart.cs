using System;
using System.IO;
using System.Net.Mail;

namespace System.Net.Mime
{
	// Token: 0x020006AE RID: 1710
	internal class MimePart : MimeBasePart, IDisposable
	{
		// Token: 0x060034C2 RID: 13506 RVA: 0x000E00E4 File Offset: 0x000DF0E4
		internal MimePart()
		{
		}

		// Token: 0x060034C3 RID: 13507 RVA: 0x000E00EC File Offset: 0x000DF0EC
		public void Dispose()
		{
			if (this.stream != null)
			{
				this.stream.Close();
			}
		}

		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x060034C4 RID: 13508 RVA: 0x000E0101 File Offset: 0x000DF101
		internal Stream Stream
		{
			get
			{
				return this.stream;
			}
		}

		// Token: 0x17000C57 RID: 3159
		// (get) Token: 0x060034C5 RID: 13509 RVA: 0x000E0109 File Offset: 0x000DF109
		// (set) Token: 0x060034C6 RID: 13510 RVA: 0x000E0111 File Offset: 0x000DF111
		internal ContentDisposition ContentDisposition
		{
			get
			{
				return this.contentDisposition;
			}
			set
			{
				this.contentDisposition = value;
				if (value == null)
				{
					((HeaderCollection)base.Headers).InternalRemove(MailHeaderInfo.GetString(MailHeaderID.ContentDisposition));
					return;
				}
				this.contentDisposition.PersistIfNeeded((HeaderCollection)base.Headers, true);
			}
		}

		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x060034C7 RID: 13511 RVA: 0x000E014C File Offset: 0x000DF14C
		// (set) Token: 0x060034C8 RID: 13512 RVA: 0x000E01BC File Offset: 0x000DF1BC
		internal TransferEncoding TransferEncoding
		{
			get
			{
				if (base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)].Equals("base64", StringComparison.OrdinalIgnoreCase))
				{
					return TransferEncoding.Base64;
				}
				if (base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)].Equals("quoted-printable", StringComparison.OrdinalIgnoreCase))
				{
					return TransferEncoding.QuotedPrintable;
				}
				if (base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)].Equals("7bit", StringComparison.OrdinalIgnoreCase))
				{
					return TransferEncoding.SevenBit;
				}
				return TransferEncoding.Unknown;
			}
			set
			{
				if (value == TransferEncoding.Base64)
				{
					base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)] = "base64";
					return;
				}
				if (value == TransferEncoding.QuotedPrintable)
				{
					base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)] = "quoted-printable";
					return;
				}
				if (value == TransferEncoding.SevenBit)
				{
					base.Headers[MailHeaderInfo.GetString(MailHeaderID.ContentTransferEncoding)] = "7bit";
					return;
				}
				throw new NotSupportedException(SR.GetString("MimeTransferEncodingNotSupported", new object[] { value }));
			}
		}

		// Token: 0x060034C9 RID: 13513 RVA: 0x000E023C File Offset: 0x000DF23C
		internal void SetContent(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (this.streamSet)
			{
				this.stream.Close();
				this.stream = null;
				this.streamSet = false;
			}
			this.stream = stream;
			this.streamSet = true;
			this.streamUsedOnce = false;
			this.TransferEncoding = TransferEncoding.Base64;
		}

		// Token: 0x060034CA RID: 13514 RVA: 0x000E0294 File Offset: 0x000DF294
		internal void SetContent(Stream stream, string name, string mimeType)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (mimeType != null && mimeType != string.Empty)
			{
				this.contentType = new ContentType(mimeType);
			}
			if (name != null && name != string.Empty)
			{
				base.ContentType.Name = name;
			}
			this.SetContent(stream);
		}

		// Token: 0x060034CB RID: 13515 RVA: 0x000E02EE File Offset: 0x000DF2EE
		internal void SetContent(Stream stream, ContentType contentType)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this.contentType = contentType;
			this.SetContent(stream);
		}

		// Token: 0x060034CC RID: 13516 RVA: 0x000E030C File Offset: 0x000DF30C
		internal void Complete(IAsyncResult result, Exception e)
		{
			MimePart.MimePartContext mimePartContext = (MimePart.MimePartContext)result.AsyncState;
			if (mimePartContext.completed)
			{
				throw e;
			}
			try
			{
				if (mimePartContext.outputStream != null)
				{
					mimePartContext.outputStream.Close();
				}
			}
			catch (Exception ex)
			{
				if (e == null)
				{
					e = ex;
				}
			}
			catch
			{
				if (e == null)
				{
					e = new Exception(SR.GetString("net_nonClsCompliantException"));
				}
			}
			mimePartContext.completed = true;
			mimePartContext.result.InvokeCallback(e);
		}

		// Token: 0x060034CD RID: 13517 RVA: 0x000E0394 File Offset: 0x000DF394
		internal void ReadCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			((MimePart.MimePartContext)result.AsyncState).completedSynchronously = false;
			try
			{
				this.ReadCallbackHandler(result);
			}
			catch (Exception ex)
			{
				this.Complete(result, ex);
			}
			catch
			{
				this.Complete(result, new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x060034CE RID: 13518 RVA: 0x000E0404 File Offset: 0x000DF404
		internal void ReadCallbackHandler(IAsyncResult result)
		{
			MimePart.MimePartContext mimePartContext = (MimePart.MimePartContext)result.AsyncState;
			mimePartContext.bytesLeft = this.Stream.EndRead(result);
			if (mimePartContext.bytesLeft > 0)
			{
				IAsyncResult asyncResult = mimePartContext.outputStream.BeginWrite(mimePartContext.buffer, 0, mimePartContext.bytesLeft, this.writeCallback, mimePartContext);
				if (asyncResult.CompletedSynchronously)
				{
					this.WriteCallbackHandler(asyncResult);
					return;
				}
			}
			else
			{
				this.Complete(result, null);
			}
		}

		// Token: 0x060034CF RID: 13519 RVA: 0x000E0470 File Offset: 0x000DF470
		internal void WriteCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			((MimePart.MimePartContext)result.AsyncState).completedSynchronously = false;
			try
			{
				this.WriteCallbackHandler(result);
			}
			catch (Exception ex)
			{
				this.Complete(result, ex);
			}
			catch
			{
				this.Complete(result, new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x060034D0 RID: 13520 RVA: 0x000E04E0 File Offset: 0x000DF4E0
		internal void WriteCallbackHandler(IAsyncResult result)
		{
			MimePart.MimePartContext mimePartContext = (MimePart.MimePartContext)result.AsyncState;
			mimePartContext.outputStream.EndWrite(result);
			IAsyncResult asyncResult = this.Stream.BeginRead(mimePartContext.buffer, 0, mimePartContext.buffer.Length, this.readCallback, mimePartContext);
			if (asyncResult.CompletedSynchronously)
			{
				this.ReadCallbackHandler(asyncResult);
			}
		}

		// Token: 0x060034D1 RID: 13521 RVA: 0x000E0538 File Offset: 0x000DF538
		internal Stream GetEncodedStream(Stream stream)
		{
			Stream stream2 = stream;
			if (this.TransferEncoding == TransferEncoding.Base64)
			{
				stream2 = new Base64Stream(stream2);
			}
			else if (this.TransferEncoding == TransferEncoding.QuotedPrintable)
			{
				stream2 = new QuotedPrintableStream(stream2, true);
			}
			else if (this.TransferEncoding == TransferEncoding.SevenBit)
			{
				stream2 = new SevenBitStream(stream2);
			}
			return stream2;
		}

		// Token: 0x060034D2 RID: 13522 RVA: 0x000E057C File Offset: 0x000DF57C
		internal void ContentStreamCallbackHandler(IAsyncResult result)
		{
			MimePart.MimePartContext mimePartContext = (MimePart.MimePartContext)result.AsyncState;
			Stream stream = mimePartContext.writer.EndGetContentStream(result);
			mimePartContext.outputStream = this.GetEncodedStream(stream);
			this.readCallback = new AsyncCallback(this.ReadCallback);
			this.writeCallback = new AsyncCallback(this.WriteCallback);
			IAsyncResult asyncResult = this.Stream.BeginRead(mimePartContext.buffer, 0, mimePartContext.buffer.Length, this.readCallback, mimePartContext);
			if (asyncResult.CompletedSynchronously)
			{
				this.ReadCallbackHandler(asyncResult);
			}
		}

		// Token: 0x060034D3 RID: 13523 RVA: 0x000E0604 File Offset: 0x000DF604
		internal void ContentStreamCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			((MimePart.MimePartContext)result.AsyncState).completedSynchronously = false;
			try
			{
				this.ContentStreamCallbackHandler(result);
			}
			catch (Exception ex)
			{
				this.Complete(result, ex);
			}
			catch
			{
				this.Complete(result, new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x060034D4 RID: 13524 RVA: 0x000E0674 File Offset: 0x000DF674
		internal override IAsyncResult BeginSend(BaseWriter writer, AsyncCallback callback, object state)
		{
			writer.WriteHeaders(base.Headers);
			MimeBasePart.MimePartAsyncResult mimePartAsyncResult = new MimeBasePart.MimePartAsyncResult(this, state, callback);
			MimePart.MimePartContext mimePartContext = new MimePart.MimePartContext(writer, mimePartAsyncResult);
			this.ResetStream();
			this.streamUsedOnce = true;
			IAsyncResult asyncResult = writer.BeginGetContentStream(new AsyncCallback(this.ContentStreamCallback), mimePartContext);
			if (asyncResult.CompletedSynchronously)
			{
				this.ContentStreamCallbackHandler(asyncResult);
			}
			return mimePartAsyncResult;
		}

		// Token: 0x060034D5 RID: 13525 RVA: 0x000E06D0 File Offset: 0x000DF6D0
		internal override void Send(BaseWriter writer)
		{
			if (this.Stream != null)
			{
				byte[] array = new byte[17408];
				writer.WriteHeaders(base.Headers);
				Stream stream = writer.GetContentStream();
				stream = this.GetEncodedStream(stream);
				this.ResetStream();
				this.streamUsedOnce = true;
				int num;
				while ((num = this.Stream.Read(array, 0, 17408)) > 0)
				{
					stream.Write(array, 0, num);
				}
				stream.Close();
			}
		}

		// Token: 0x060034D6 RID: 13526 RVA: 0x000E0740 File Offset: 0x000DF740
		internal void ResetStream()
		{
			if (!this.streamUsedOnce)
			{
				return;
			}
			if (this.Stream.CanSeek)
			{
				this.Stream.Seek(0L, SeekOrigin.Begin);
				this.streamUsedOnce = false;
				return;
			}
			throw new InvalidOperationException(SR.GetString("MimePartCantResetStream"));
		}

		// Token: 0x04003079 RID: 12409
		private const int maxBufferSize = 17408;

		// Token: 0x0400307A RID: 12410
		private Stream stream;

		// Token: 0x0400307B RID: 12411
		private bool streamSet;

		// Token: 0x0400307C RID: 12412
		private bool streamUsedOnce;

		// Token: 0x0400307D RID: 12413
		private AsyncCallback readCallback;

		// Token: 0x0400307E RID: 12414
		private AsyncCallback writeCallback;

		// Token: 0x020006AF RID: 1711
		internal class MimePartContext
		{
			// Token: 0x060034D7 RID: 13527 RVA: 0x000E077E File Offset: 0x000DF77E
			internal MimePartContext(BaseWriter writer, LazyAsyncResult result)
			{
				this.writer = writer;
				this.result = result;
				this.buffer = new byte[17408];
			}

			// Token: 0x0400307F RID: 12415
			internal Stream outputStream;

			// Token: 0x04003080 RID: 12416
			internal LazyAsyncResult result;

			// Token: 0x04003081 RID: 12417
			internal int bytesLeft;

			// Token: 0x04003082 RID: 12418
			internal BaseWriter writer;

			// Token: 0x04003083 RID: 12419
			internal byte[] buffer;

			// Token: 0x04003084 RID: 12420
			internal bool completed;

			// Token: 0x04003085 RID: 12421
			internal bool completedSynchronously = true;
		}
	}
}
