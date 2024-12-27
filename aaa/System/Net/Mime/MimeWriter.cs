using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace System.Net.Mime
{
	// Token: 0x020006B0 RID: 1712
	internal class MimeWriter : BaseWriter
	{
		// Token: 0x060034D8 RID: 13528 RVA: 0x000E07AB File Offset: 0x000DF7AB
		internal MimeWriter(Stream stream, string boundary)
			: this(stream, boundary, null, MimeWriter.DefaultLineLength)
		{
		}

		// Token: 0x060034D9 RID: 13529 RVA: 0x000E07BC File Offset: 0x000DF7BC
		internal MimeWriter(Stream stream, string boundary, string preface, int lineLength)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (boundary == null)
			{
				throw new ArgumentNullException("boundary");
			}
			if (lineLength < 40)
			{
				throw new ArgumentOutOfRangeException("lineLength", lineLength, SR.GetString("MailWriterLineLengthTooSmall"));
			}
			this.stream = stream;
			this.lineLength = lineLength;
			this.onCloseHandler = new EventHandler(this.OnClose);
			this.boundaryBytes = Encoding.ASCII.GetBytes(boundary);
			this.preface = preface;
		}

		// Token: 0x060034DA RID: 13530 RVA: 0x000E0858 File Offset: 0x000DF858
		internal IAsyncResult BeginClose(AsyncCallback callback, object state)
		{
			MultiAsyncResult multiAsyncResult = new MultiAsyncResult(this, callback, state);
			this.Close(multiAsyncResult);
			multiAsyncResult.CompleteSequence();
			return multiAsyncResult;
		}

		// Token: 0x060034DB RID: 13531 RVA: 0x000E087C File Offset: 0x000DF87C
		internal void EndClose(IAsyncResult result)
		{
			MultiAsyncResult.End(result);
			this.stream.Close();
		}

		// Token: 0x060034DC RID: 13532 RVA: 0x000E0890 File Offset: 0x000DF890
		internal override void Close()
		{
			this.Close(null);
			this.stream.Close();
		}

		// Token: 0x060034DD RID: 13533 RVA: 0x000E08A4 File Offset: 0x000DF8A4
		private void Close(MultiAsyncResult multiResult)
		{
			this.bufferBuilder.Append(MimeWriter.CRLF);
			this.bufferBuilder.Append(MimeWriter.DASHDASH);
			this.bufferBuilder.Append(this.boundaryBytes);
			this.bufferBuilder.Append(MimeWriter.DASHDASH);
			this.bufferBuilder.Append(MimeWriter.CRLF);
			this.Flush(multiResult);
		}

		// Token: 0x060034DE RID: 13534 RVA: 0x000E090C File Offset: 0x000DF90C
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

		// Token: 0x060034DF RID: 13535 RVA: 0x000E0946 File Offset: 0x000DF946
		internal override IAsyncResult BeginGetContentStream(AsyncCallback callback, object state)
		{
			return this.BeginGetContentStream(ContentTransferEncoding.SevenBit, callback, state);
		}

		// Token: 0x060034E0 RID: 13536 RVA: 0x000E0954 File Offset: 0x000DF954
		internal override Stream EndGetContentStream(IAsyncResult result)
		{
			object obj = MultiAsyncResult.End(result);
			if (obj is Exception)
			{
				throw (Exception)obj;
			}
			return (Stream)obj;
		}

		// Token: 0x060034E1 RID: 13537 RVA: 0x000E097D File Offset: 0x000DF97D
		internal Stream GetContentStream(ContentTransferEncoding contentTransferEncoding)
		{
			if (this.isInContent)
			{
				throw new InvalidOperationException(SR.GetString("MailWriterIsInContent"));
			}
			this.isInContent = true;
			return this.GetContentStream(contentTransferEncoding, null);
		}

		// Token: 0x060034E2 RID: 13538 RVA: 0x000E09A6 File Offset: 0x000DF9A6
		internal override Stream GetContentStream()
		{
			return this.GetContentStream(ContentTransferEncoding.SevenBit);
		}

		// Token: 0x060034E3 RID: 13539 RVA: 0x000E09B0 File Offset: 0x000DF9B0
		private Stream GetContentStream(ContentTransferEncoding contentTransferEncoding, MultiAsyncResult multiResult)
		{
			this.CheckBoundary();
			this.bufferBuilder.Append(MimeWriter.CRLF);
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

		// Token: 0x060034E4 RID: 13540 RVA: 0x000E0A28 File Offset: 0x000DFA28
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
			this.CheckBoundary();
			this.bufferBuilder.Append(name);
			this.bufferBuilder.Append(": ");
			this.WriteAndFold(value, name.Length + 2);
			this.bufferBuilder.Append(MimeWriter.CRLF);
		}

		// Token: 0x060034E5 RID: 13541 RVA: 0x000E0AAC File Offset: 0x000DFAAC
		internal override void WriteHeaders(NameValueCollection headers)
		{
			if (headers == null)
			{
				throw new ArgumentNullException("headers");
			}
			foreach (object obj in headers)
			{
				string text = (string)obj;
				this.WriteHeader(text, headers[text]);
			}
		}

		// Token: 0x060034E6 RID: 13542 RVA: 0x000E0B18 File Offset: 0x000DFB18
		private void OnClose(object sender, EventArgs args)
		{
			if (this.contentStream != sender)
			{
				return;
			}
			this.contentStream.Flush();
			this.contentStream = null;
			this.writeBoundary = true;
			this.isInContent = false;
		}

		// Token: 0x060034E7 RID: 13543 RVA: 0x000E0B44 File Offset: 0x000DFB44
		private void CheckBoundary()
		{
			if (this.preface != null)
			{
				this.bufferBuilder.Append(this.preface);
				this.preface = null;
			}
			if (this.writeBoundary)
			{
				this.bufferBuilder.Append(MimeWriter.CRLF);
				this.bufferBuilder.Append(MimeWriter.DASHDASH);
				this.bufferBuilder.Append(this.boundaryBytes);
				this.bufferBuilder.Append(MimeWriter.CRLF);
				this.writeBoundary = false;
			}
		}

		// Token: 0x060034E8 RID: 13544 RVA: 0x000E0BC4 File Offset: 0x000DFBC4
		private static void OnWrite(IAsyncResult result)
		{
			if (!result.CompletedSynchronously)
			{
				MultiAsyncResult multiAsyncResult = (MultiAsyncResult)result.AsyncState;
				MimeWriter mimeWriter = (MimeWriter)multiAsyncResult.Context;
				try
				{
					mimeWriter.stream.EndWrite(result);
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

		// Token: 0x060034E9 RID: 13545 RVA: 0x000E0C44 File Offset: 0x000DFC44
		private void Flush(MultiAsyncResult multiResult)
		{
			if (this.bufferBuilder.Length > 0)
			{
				if (multiResult != null)
				{
					multiResult.Enter();
					IAsyncResult asyncResult = this.stream.BeginWrite(this.bufferBuilder.GetBuffer(), 0, this.bufferBuilder.Length, MimeWriter.onWrite, multiResult);
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

		// Token: 0x060034EA RID: 13546 RVA: 0x000E0CDC File Offset: 0x000DFCDC
		private void WriteAndFold(string value, int startLength)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			while (num != value.Length)
			{
				if (value[num] == ' ' || value[num] == '\t')
				{
					if (num - num3 >= this.lineLength - startLength)
					{
						startLength = 0;
						if (num2 == num3)
						{
							num2 = num;
						}
						this.bufferBuilder.Append(value, num3, num2 - num3);
						this.bufferBuilder.Append(MimeWriter.CRLF);
						num3 = num2;
					}
					num2 = num;
				}
				num++;
			}
			if (num - num3 > 0)
			{
				this.bufferBuilder.Append(value, num3, num - num3);
				return;
			}
		}

		// Token: 0x04003086 RID: 12422
		private static int DefaultLineLength = 78;

		// Token: 0x04003087 RID: 12423
		private static byte[] DASHDASH = new byte[] { 45, 45 };

		// Token: 0x04003088 RID: 12424
		private static byte[] CRLF = new byte[] { 13, 10 };

		// Token: 0x04003089 RID: 12425
		private byte[] boundaryBytes;

		// Token: 0x0400308A RID: 12426
		private BufferBuilder bufferBuilder = new BufferBuilder();

		// Token: 0x0400308B RID: 12427
		private Stream contentStream;

		// Token: 0x0400308C RID: 12428
		private bool isInContent;

		// Token: 0x0400308D RID: 12429
		private int lineLength;

		// Token: 0x0400308E RID: 12430
		private EventHandler onCloseHandler;

		// Token: 0x0400308F RID: 12431
		private Stream stream;

		// Token: 0x04003090 RID: 12432
		private bool writeBoundary = true;

		// Token: 0x04003091 RID: 12433
		private string preface;

		// Token: 0x04003092 RID: 12434
		private static AsyncCallback onWrite = new AsyncCallback(MimeWriter.OnWrite);
	}
}
