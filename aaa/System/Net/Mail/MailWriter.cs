using System;
using System.Collections.Specialized;
using System.IO;
using System.Net.Mime;

namespace System.Net.Mail
{
	// Token: 0x020006A1 RID: 1697
	internal class MailWriter : BaseWriter
	{
		// Token: 0x06003474 RID: 13428 RVA: 0x000DE9B8 File Offset: 0x000DD9B8
		internal MailWriter(Stream stream)
			: this(stream, MailWriter.DefaultLineLength)
		{
		}

		// Token: 0x06003475 RID: 13429 RVA: 0x000DE9C8 File Offset: 0x000DD9C8
		internal MailWriter(Stream stream, int lineLength)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (lineLength < 0)
			{
				throw new ArgumentOutOfRangeException("lineLength");
			}
			this.stream = stream;
			this.lineLength = lineLength;
			this.onCloseHandler = new EventHandler(this.OnClose);
		}

		// Token: 0x06003476 RID: 13430 RVA: 0x000DEA23 File Offset: 0x000DDA23
		internal override void Close()
		{
			this.stream.Write(MailWriter.CRLF, 0, 2);
			this.stream.Close();
		}

		// Token: 0x06003477 RID: 13431 RVA: 0x000DEA44 File Offset: 0x000DDA44
		internal IAsyncResult BeginGetContentStream(ContentTransferEncoding contentTransferEncoding, AsyncCallback callback, object state)
		{
			MultiAsyncResult multiAsyncResult = new MultiAsyncResult(this, callback, state);
			Stream stream = this.GetContentStream(contentTransferEncoding, multiAsyncResult);
			if (!(multiAsyncResult.Result is Exception))
			{
				multiAsyncResult.Result = stream;
			}
			multiAsyncResult.CompleteSequence();
			return multiAsyncResult;
		}

		// Token: 0x06003478 RID: 13432 RVA: 0x000DEA7E File Offset: 0x000DDA7E
		internal override IAsyncResult BeginGetContentStream(AsyncCallback callback, object state)
		{
			return this.BeginGetContentStream(ContentTransferEncoding.SevenBit, callback, state);
		}

		// Token: 0x06003479 RID: 13433 RVA: 0x000DEA8C File Offset: 0x000DDA8C
		internal override Stream EndGetContentStream(IAsyncResult result)
		{
			object obj = MultiAsyncResult.End(result);
			if (obj is Exception)
			{
				throw (Exception)obj;
			}
			return (Stream)obj;
		}

		// Token: 0x0600347A RID: 13434 RVA: 0x000DEAB5 File Offset: 0x000DDAB5
		internal Stream GetContentStream(ContentTransferEncoding contentTransferEncoding)
		{
			return this.GetContentStream(contentTransferEncoding, null);
		}

		// Token: 0x0600347B RID: 13435 RVA: 0x000DEABF File Offset: 0x000DDABF
		internal override Stream GetContentStream()
		{
			return this.GetContentStream(ContentTransferEncoding.SevenBit);
		}

		// Token: 0x0600347C RID: 13436 RVA: 0x000DEAC8 File Offset: 0x000DDAC8
		private Stream GetContentStream(ContentTransferEncoding contentTransferEncoding, MultiAsyncResult multiResult)
		{
			if (this.isInContent)
			{
				throw new InvalidOperationException(SR.GetString("MailWriterIsInContent"));
			}
			this.isInContent = true;
			this.bufferBuilder.Append(MailWriter.CRLF);
			this.Flush(multiResult);
			Stream stream = this.stream;
			if (contentTransferEncoding == ContentTransferEncoding.SevenBit)
			{
				stream = new SevenBitStream(stream);
			}
			else if (contentTransferEncoding == ContentTransferEncoding.QuotedPrintable)
			{
				stream = new QuotedPrintableStream(stream, this.lineLength);
			}
			else if (contentTransferEncoding == ContentTransferEncoding.Base64)
			{
				stream = new Base64Stream(stream, this.lineLength);
			}
			ClosableStream closableStream = new ClosableStream(stream, this.onCloseHandler);
			this.contentStream = closableStream;
			return closableStream;
		}

		// Token: 0x0600347D RID: 13437 RVA: 0x000DEB58 File Offset: 0x000DDB58
		internal override void WriteHeader(string name, string value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.isInContent)
			{
				throw new InvalidOperationException(SR.GetString("MailWriterIsInContent"));
			}
			this.bufferBuilder.Append(name);
			this.bufferBuilder.Append(": ");
			this.WriteAndFold(value);
			this.bufferBuilder.Append(MailWriter.CRLF);
		}

		// Token: 0x0600347E RID: 13438 RVA: 0x000DEBCC File Offset: 0x000DDBCC
		internal override void WriteHeaders(NameValueCollection headers)
		{
			if (headers == null)
			{
				throw new ArgumentNullException("headers");
			}
			if (this.isInContent)
			{
				throw new InvalidOperationException(SR.GetString("MailWriterIsInContent"));
			}
			foreach (object obj in headers)
			{
				string text = (string)obj;
				string[] values = headers.GetValues(text);
				foreach (string text2 in values)
				{
					this.WriteHeader(text, text2);
				}
			}
		}

		// Token: 0x0600347F RID: 13439 RVA: 0x000DEC70 File Offset: 0x000DDC70
		private void OnClose(object sender, EventArgs args)
		{
			this.contentStream.Flush();
			this.contentStream = null;
		}

		// Token: 0x06003480 RID: 13440 RVA: 0x000DEC84 File Offset: 0x000DDC84
		private static void OnWrite(IAsyncResult result)
		{
			if (!result.CompletedSynchronously)
			{
				MultiAsyncResult multiAsyncResult = (MultiAsyncResult)result.AsyncState;
				MailWriter mailWriter = (MailWriter)multiAsyncResult.Context;
				try
				{
					mailWriter.stream.EndWrite(result);
					multiAsyncResult.Leave();
				}
				catch (Exception ex)
				{
					multiAsyncResult.Leave(ex);
				}
				catch
				{
					multiAsyncResult.Leave(new Exception(SR.GetString("net_nonClsCompliantException")));
				}
			}
		}

		// Token: 0x06003481 RID: 13441 RVA: 0x000DED04 File Offset: 0x000DDD04
		private void Flush(MultiAsyncResult multiResult)
		{
			if (this.bufferBuilder.Length > 0)
			{
				if (multiResult != null)
				{
					multiResult.Enter();
					IAsyncResult asyncResult = this.stream.BeginWrite(this.bufferBuilder.GetBuffer(), 0, this.bufferBuilder.Length, MailWriter.onWrite, multiResult);
					if (asyncResult.CompletedSynchronously)
					{
						this.stream.EndWrite(asyncResult);
						multiResult.Leave();
					}
				}
				else
				{
					this.stream.Write(this.bufferBuilder.GetBuffer(), 0, this.bufferBuilder.Length);
				}
				this.bufferBuilder.Reset();
			}
		}

		// Token: 0x06003482 RID: 13442 RVA: 0x000DED9C File Offset: 0x000DDD9C
		private void WriteAndFold(string value)
		{
			if (value.Length < MailWriter.DefaultLineLength)
			{
				this.bufferBuilder.Append(value);
				return;
			}
			int num = 0;
			int length = value.Length;
			while (length - num > MailWriter.DefaultLineLength)
			{
				int num2 = value.LastIndexOf(' ', num + MailWriter.DefaultLineLength - 1, MailWriter.DefaultLineLength - 1);
				if (num2 > -1)
				{
					this.bufferBuilder.Append(value, num, num2 - num);
					this.bufferBuilder.Append(MailWriter.CRLF);
					num = num2;
				}
				else
				{
					this.bufferBuilder.Append(value, num, MailWriter.DefaultLineLength);
					num += MailWriter.DefaultLineLength;
				}
			}
			if (num < length)
			{
				this.bufferBuilder.Append(value, num, length - num);
			}
		}

		// Token: 0x0400303F RID: 12351
		private static byte[] CRLF = new byte[] { 13, 10 };

		// Token: 0x04003040 RID: 12352
		private static int DefaultLineLength = 78;

		// Token: 0x04003041 RID: 12353
		private Stream contentStream;

		// Token: 0x04003042 RID: 12354
		private bool isInContent;

		// Token: 0x04003043 RID: 12355
		private int lineLength;

		// Token: 0x04003044 RID: 12356
		private EventHandler onCloseHandler;

		// Token: 0x04003045 RID: 12357
		private Stream stream;

		// Token: 0x04003046 RID: 12358
		private BufferBuilder bufferBuilder = new BufferBuilder();

		// Token: 0x04003047 RID: 12359
		private static AsyncCallback onWrite = new AsyncCallback(MailWriter.OnWrite);
	}
}
