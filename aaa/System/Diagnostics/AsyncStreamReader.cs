using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;

namespace System.Diagnostics
{
	// Token: 0x02000741 RID: 1857
	internal class AsyncStreamReader : IDisposable
	{
		// Token: 0x06003890 RID: 14480 RVA: 0x000EED38 File Offset: 0x000EDD38
		internal AsyncStreamReader(Process process, Stream stream, UserCallBack callback, Encoding encoding)
			: this(process, stream, callback, encoding, 1024)
		{
		}

		// Token: 0x06003891 RID: 14481 RVA: 0x000EED4A File Offset: 0x000EDD4A
		internal AsyncStreamReader(Process process, Stream stream, UserCallBack callback, Encoding encoding, int bufferSize)
		{
			this.Init(process, stream, callback, encoding, bufferSize);
			this.messageQueue = new Queue();
		}

		// Token: 0x06003892 RID: 14482 RVA: 0x000EED6C File Offset: 0x000EDD6C
		private void Init(Process process, Stream stream, UserCallBack callback, Encoding encoding, int bufferSize)
		{
			this.process = process;
			this.stream = stream;
			this.encoding = encoding;
			this.userCallBack = callback;
			this.decoder = encoding.GetDecoder();
			if (bufferSize < 128)
			{
				bufferSize = 128;
			}
			this.byteBuffer = new byte[bufferSize];
			this._maxCharsPerBuffer = encoding.GetMaxCharCount(bufferSize);
			this.charBuffer = new char[this._maxCharsPerBuffer];
			this.cancelOperation = false;
			this.eofEvent = new ManualResetEvent(false);
			this.sb = null;
			this.bLastCarriageReturn = false;
		}

		// Token: 0x06003893 RID: 14483 RVA: 0x000EEE01 File Offset: 0x000EDE01
		public virtual void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x06003894 RID: 14484 RVA: 0x000EEE0A File Offset: 0x000EDE0A
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06003895 RID: 14485 RVA: 0x000EEE14 File Offset: 0x000EDE14
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.stream != null)
			{
				this.stream.Close();
			}
			if (this.stream != null)
			{
				this.stream = null;
				this.encoding = null;
				this.decoder = null;
				this.byteBuffer = null;
				this.charBuffer = null;
			}
			if (this.eofEvent != null)
			{
				this.eofEvent.Close();
				this.eofEvent = null;
			}
		}

		// Token: 0x17000D19 RID: 3353
		// (get) Token: 0x06003896 RID: 14486 RVA: 0x000EEE7C File Offset: 0x000EDE7C
		public virtual Encoding CurrentEncoding
		{
			get
			{
				return this.encoding;
			}
		}

		// Token: 0x17000D1A RID: 3354
		// (get) Token: 0x06003897 RID: 14487 RVA: 0x000EEE84 File Offset: 0x000EDE84
		public virtual Stream BaseStream
		{
			get
			{
				return this.stream;
			}
		}

		// Token: 0x06003898 RID: 14488 RVA: 0x000EEE8C File Offset: 0x000EDE8C
		internal void BeginReadLine()
		{
			if (this.cancelOperation)
			{
				this.cancelOperation = false;
			}
			if (this.sb == null)
			{
				this.sb = new StringBuilder(1024);
				this.stream.BeginRead(this.byteBuffer, 0, this.byteBuffer.Length, new AsyncCallback(this.ReadBuffer), null);
				return;
			}
			this.FlushMessageQueue();
		}

		// Token: 0x06003899 RID: 14489 RVA: 0x000EEEEF File Offset: 0x000EDEEF
		internal void CancelOperation()
		{
			this.cancelOperation = true;
		}

		// Token: 0x0600389A RID: 14490 RVA: 0x000EEEF8 File Offset: 0x000EDEF8
		private void ReadBuffer(IAsyncResult ar)
		{
			int num;
			try
			{
				num = this.stream.EndRead(ar);
			}
			catch (IOException)
			{
				num = 0;
			}
			catch (OperationCanceledException)
			{
				num = 0;
			}
			if (num == 0)
			{
				lock (this.messageQueue)
				{
					if (this.sb.Length != 0)
					{
						this.messageQueue.Enqueue(this.sb.ToString());
						this.sb.Length = 0;
					}
					this.messageQueue.Enqueue(null);
				}
				try
				{
					this.FlushMessageQueue();
					return;
				}
				finally
				{
					this.eofEvent.Set();
				}
			}
			int chars = this.decoder.GetChars(this.byteBuffer, 0, num, this.charBuffer, 0);
			this.sb.Append(this.charBuffer, 0, chars);
			this.GetLinesFromStringBuilder();
			this.stream.BeginRead(this.byteBuffer, 0, this.byteBuffer.Length, new AsyncCallback(this.ReadBuffer), null);
		}

		// Token: 0x0600389B RID: 14491 RVA: 0x000EF018 File Offset: 0x000EE018
		private void GetLinesFromStringBuilder()
		{
			int i = 0;
			int num = 0;
			int length = this.sb.Length;
			if (this.bLastCarriageReturn && length > 0 && this.sb[0] == '\n')
			{
				i = 1;
				num = 1;
				this.bLastCarriageReturn = false;
			}
			while (i < length)
			{
				char c = this.sb[i];
				if (c == '\r' || c == '\n')
				{
					string text = this.sb.ToString(num, i - num);
					num = i + 1;
					if (c == '\r' && num < length && this.sb[num] == '\n')
					{
						num++;
						i++;
					}
					lock (this.messageQueue)
					{
						this.messageQueue.Enqueue(text);
					}
				}
				i++;
			}
			if (this.sb[length - 1] == '\r')
			{
				this.bLastCarriageReturn = true;
			}
			if (num < length)
			{
				this.sb.Remove(0, num);
			}
			else
			{
				this.sb.Length = 0;
			}
			this.FlushMessageQueue();
		}

		// Token: 0x0600389C RID: 14492 RVA: 0x000EF130 File Offset: 0x000EE130
		private void FlushMessageQueue()
		{
			while (this.messageQueue.Count > 0)
			{
				lock (this.messageQueue)
				{
					if (this.messageQueue.Count > 0)
					{
						string text = (string)this.messageQueue.Dequeue();
						if (!this.cancelOperation)
						{
							this.userCallBack(text);
						}
					}
					continue;
				}
				break;
			}
		}

		// Token: 0x0600389D RID: 14493 RVA: 0x000EF1A4 File Offset: 0x000EE1A4
		internal void WaitUtilEOF()
		{
			if (this.eofEvent != null)
			{
				this.eofEvent.WaitOne();
				this.eofEvent.Close();
				this.eofEvent = null;
			}
		}

		// Token: 0x0400324B RID: 12875
		internal const int DefaultBufferSize = 1024;

		// Token: 0x0400324C RID: 12876
		private const int MinBufferSize = 128;

		// Token: 0x0400324D RID: 12877
		private Stream stream;

		// Token: 0x0400324E RID: 12878
		private Encoding encoding;

		// Token: 0x0400324F RID: 12879
		private Decoder decoder;

		// Token: 0x04003250 RID: 12880
		private byte[] byteBuffer;

		// Token: 0x04003251 RID: 12881
		private char[] charBuffer;

		// Token: 0x04003252 RID: 12882
		private int _maxCharsPerBuffer;

		// Token: 0x04003253 RID: 12883
		private Process process;

		// Token: 0x04003254 RID: 12884
		private UserCallBack userCallBack;

		// Token: 0x04003255 RID: 12885
		private bool cancelOperation;

		// Token: 0x04003256 RID: 12886
		private ManualResetEvent eofEvent;

		// Token: 0x04003257 RID: 12887
		private Queue messageQueue;

		// Token: 0x04003258 RID: 12888
		private StringBuilder sb;

		// Token: 0x04003259 RID: 12889
		private bool bLastCarriageReturn;
	}
}
