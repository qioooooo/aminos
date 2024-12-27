using System;
using System.Runtime.InteropServices;
using System.Text;

namespace System.IO
{
	// Token: 0x020005B2 RID: 1458
	[ComVisible(true)]
	[Serializable]
	public class StreamReader : TextReader
	{
		// Token: 0x06003691 RID: 13969 RVA: 0x000B9493 File Offset: 0x000B8493
		internal StreamReader()
		{
		}

		// Token: 0x06003692 RID: 13970 RVA: 0x000B949B File Offset: 0x000B849B
		public StreamReader(Stream stream)
			: this(stream, true)
		{
		}

		// Token: 0x06003693 RID: 13971 RVA: 0x000B94A5 File Offset: 0x000B84A5
		public StreamReader(Stream stream, bool detectEncodingFromByteOrderMarks)
			: this(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks, 1024)
		{
		}

		// Token: 0x06003694 RID: 13972 RVA: 0x000B94B9 File Offset: 0x000B84B9
		public StreamReader(Stream stream, Encoding encoding)
			: this(stream, encoding, true, 1024)
		{
		}

		// Token: 0x06003695 RID: 13973 RVA: 0x000B94C9 File Offset: 0x000B84C9
		public StreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks)
			: this(stream, encoding, detectEncodingFromByteOrderMarks, 1024)
		{
		}

		// Token: 0x06003696 RID: 13974 RVA: 0x000B94DC File Offset: 0x000B84DC
		public StreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize)
		{
			if (stream == null || encoding == null)
			{
				throw new ArgumentNullException((stream == null) ? "stream" : "encoding");
			}
			if (!stream.CanRead)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_StreamNotReadable"));
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			this.Init(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize);
		}

		// Token: 0x06003697 RID: 13975 RVA: 0x000B9547 File Offset: 0x000B8547
		internal StreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize, bool closable)
			: this(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize)
		{
			this._closable = closable;
		}

		// Token: 0x06003698 RID: 13976 RVA: 0x000B955C File Offset: 0x000B855C
		public StreamReader(string path)
			: this(path, true)
		{
		}

		// Token: 0x06003699 RID: 13977 RVA: 0x000B9566 File Offset: 0x000B8566
		public StreamReader(string path, bool detectEncodingFromByteOrderMarks)
			: this(path, Encoding.UTF8, detectEncodingFromByteOrderMarks, 1024)
		{
		}

		// Token: 0x0600369A RID: 13978 RVA: 0x000B957A File Offset: 0x000B857A
		public StreamReader(string path, Encoding encoding)
			: this(path, encoding, true, 1024)
		{
		}

		// Token: 0x0600369B RID: 13979 RVA: 0x000B958A File Offset: 0x000B858A
		public StreamReader(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks)
			: this(path, encoding, detectEncodingFromByteOrderMarks, 1024)
		{
		}

		// Token: 0x0600369C RID: 13980 RVA: 0x000B959C File Offset: 0x000B859C
		public StreamReader(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize)
		{
			if (path == null || encoding == null)
			{
				throw new ArgumentNullException((path == null) ? "path" : "encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyPath"));
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan);
			this.Init(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize);
		}

		// Token: 0x0600369D RID: 13981 RVA: 0x000B961C File Offset: 0x000B861C
		private void Init(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize)
		{
			this.stream = stream;
			this.encoding = encoding;
			this.decoder = encoding.GetDecoder();
			if (bufferSize < 128)
			{
				bufferSize = 128;
			}
			this.byteBuffer = new byte[bufferSize];
			this._maxCharsPerBuffer = encoding.GetMaxCharCount(bufferSize);
			this.charBuffer = new char[this._maxCharsPerBuffer];
			this.byteLen = 0;
			this.bytePos = 0;
			this._detectEncoding = detectEncodingFromByteOrderMarks;
			this._preamble = encoding.GetPreamble();
			this._checkPreamble = this._preamble.Length > 0;
			this._isBlocked = false;
			this._closable = true;
		}

		// Token: 0x0600369E RID: 13982 RVA: 0x000B96BF File Offset: 0x000B86BF
		public override void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x0600369F RID: 13983 RVA: 0x000B96C8 File Offset: 0x000B86C8
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (this.Closable && disposing && this.stream != null)
				{
					this.stream.Close();
				}
			}
			finally
			{
				if (this.Closable && this.stream != null)
				{
					this.stream = null;
					this.encoding = null;
					this.decoder = null;
					this.byteBuffer = null;
					this.charBuffer = null;
					this.charPos = 0;
					this.charLen = 0;
					base.Dispose(disposing);
				}
			}
		}

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x060036A0 RID: 13984 RVA: 0x000B9750 File Offset: 0x000B8750
		public virtual Encoding CurrentEncoding
		{
			get
			{
				return this.encoding;
			}
		}

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x060036A1 RID: 13985 RVA: 0x000B9758 File Offset: 0x000B8758
		public virtual Stream BaseStream
		{
			get
			{
				return this.stream;
			}
		}

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x060036A2 RID: 13986 RVA: 0x000B9760 File Offset: 0x000B8760
		internal bool Closable
		{
			get
			{
				return this._closable;
			}
		}

		// Token: 0x060036A3 RID: 13987 RVA: 0x000B9768 File Offset: 0x000B8768
		public void DiscardBufferedData()
		{
			this.byteLen = 0;
			this.charLen = 0;
			this.charPos = 0;
			this.decoder = this.encoding.GetDecoder();
			this._isBlocked = false;
		}

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x060036A4 RID: 13988 RVA: 0x000B9798 File Offset: 0x000B8798
		public bool EndOfStream
		{
			get
			{
				if (this.stream == null)
				{
					__Error.ReaderClosed();
				}
				if (this.charPos < this.charLen)
				{
					return false;
				}
				int num = this.ReadBuffer();
				return num == 0;
			}
		}

		// Token: 0x060036A5 RID: 13989 RVA: 0x000B97CD File Offset: 0x000B87CD
		public override int Peek()
		{
			if (this.stream == null)
			{
				__Error.ReaderClosed();
			}
			if (this.charPos == this.charLen && (this._isBlocked || this.ReadBuffer() == 0))
			{
				return -1;
			}
			return (int)this.charBuffer[this.charPos];
		}

		// Token: 0x060036A6 RID: 13990 RVA: 0x000B980C File Offset: 0x000B880C
		public override int Read()
		{
			if (this.stream == null)
			{
				__Error.ReaderClosed();
			}
			if (this.charPos == this.charLen && this.ReadBuffer() == 0)
			{
				return -1;
			}
			int num = (int)this.charBuffer[this.charPos];
			this.charPos++;
			return num;
		}

		// Token: 0x060036A7 RID: 13991 RVA: 0x000B985C File Offset: 0x000B885C
		public override int Read([In] [Out] char[] buffer, int index, int count)
		{
			if (this.stream == null)
			{
				__Error.ReaderClosed();
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", Environment.GetResourceString("ArgumentNull_Buffer"));
			}
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			int num = 0;
			bool flag = false;
			while (count > 0)
			{
				int num2 = this.charLen - this.charPos;
				if (num2 == 0)
				{
					num2 = this.ReadBuffer(buffer, index + num, count, out flag);
				}
				if (num2 == 0)
				{
					break;
				}
				if (num2 > count)
				{
					num2 = count;
				}
				if (!flag)
				{
					Buffer.InternalBlockCopy(this.charBuffer, this.charPos * 2, buffer, (index + num) * 2, num2 * 2);
					this.charPos += num2;
				}
				num += num2;
				count -= num2;
				if (this._isBlocked)
				{
					break;
				}
			}
			return num;
		}

		// Token: 0x060036A8 RID: 13992 RVA: 0x000B9940 File Offset: 0x000B8940
		public override string ReadToEnd()
		{
			if (this.stream == null)
			{
				__Error.ReaderClosed();
			}
			StringBuilder stringBuilder = new StringBuilder(this.charLen - this.charPos);
			do
			{
				stringBuilder.Append(this.charBuffer, this.charPos, this.charLen - this.charPos);
				this.charPos = this.charLen;
				this.ReadBuffer();
			}
			while (this.charLen > 0);
			return stringBuilder.ToString();
		}

		// Token: 0x060036A9 RID: 13993 RVA: 0x000B99AF File Offset: 0x000B89AF
		private void CompressBuffer(int n)
		{
			Buffer.InternalBlockCopy(this.byteBuffer, n, this.byteBuffer, 0, this.byteLen - n);
			this.byteLen -= n;
		}

		// Token: 0x060036AA RID: 13994 RVA: 0x000B99DC File Offset: 0x000B89DC
		private void DetectEncoding()
		{
			if (this.byteLen < 2)
			{
				return;
			}
			this._detectEncoding = false;
			bool flag = false;
			if (this.byteBuffer[0] == 254 && this.byteBuffer[1] == 255)
			{
				this.encoding = new UnicodeEncoding(true, true);
				this.CompressBuffer(2);
				flag = true;
			}
			else if (this.byteBuffer[0] == 255 && this.byteBuffer[1] == 254)
			{
				if (this.byteLen >= 4 && this.byteBuffer[2] == 0 && this.byteBuffer[3] == 0)
				{
					this.encoding = new UTF32Encoding(false, true);
					this.CompressBuffer(4);
				}
				else
				{
					this.encoding = new UnicodeEncoding(false, true);
					this.CompressBuffer(2);
				}
				flag = true;
			}
			else if (this.byteLen >= 3 && this.byteBuffer[0] == 239 && this.byteBuffer[1] == 187 && this.byteBuffer[2] == 191)
			{
				this.encoding = Encoding.UTF8;
				this.CompressBuffer(3);
				flag = true;
			}
			else if (this.byteLen >= 4 && this.byteBuffer[0] == 0 && this.byteBuffer[1] == 0 && this.byteBuffer[2] == 254 && this.byteBuffer[3] == 255)
			{
				this.encoding = new UTF32Encoding(true, true);
				flag = true;
			}
			else if (this.byteLen == 2)
			{
				this._detectEncoding = true;
			}
			if (flag)
			{
				this.decoder = this.encoding.GetDecoder();
				this._maxCharsPerBuffer = this.encoding.GetMaxCharCount(this.byteBuffer.Length);
				this.charBuffer = new char[this._maxCharsPerBuffer];
			}
		}

		// Token: 0x060036AB RID: 13995 RVA: 0x000B9B88 File Offset: 0x000B8B88
		private bool IsPreamble()
		{
			if (!this._checkPreamble)
			{
				return this._checkPreamble;
			}
			int num = ((this.byteLen >= this._preamble.Length) ? (this._preamble.Length - this.bytePos) : (this.byteLen - this.bytePos));
			int i = 0;
			while (i < num)
			{
				if (this.byteBuffer[this.bytePos] != this._preamble[this.bytePos])
				{
					this.bytePos = 0;
					this._checkPreamble = false;
					break;
				}
				i++;
				this.bytePos++;
			}
			if (this._checkPreamble && this.bytePos == this._preamble.Length)
			{
				this.CompressBuffer(this._preamble.Length);
				this.bytePos = 0;
				this._checkPreamble = false;
				this._detectEncoding = false;
			}
			return this._checkPreamble;
		}

		// Token: 0x060036AC RID: 13996 RVA: 0x000B9C5C File Offset: 0x000B8C5C
		private int ReadBuffer()
		{
			this.charLen = 0;
			this.charPos = 0;
			if (!this._checkPreamble)
			{
				this.byteLen = 0;
			}
			for (;;)
			{
				if (this._checkPreamble)
				{
					int num = this.stream.Read(this.byteBuffer, this.bytePos, this.byteBuffer.Length - this.bytePos);
					if (num == 0)
					{
						break;
					}
					this.byteLen += num;
				}
				else
				{
					this.byteLen = this.stream.Read(this.byteBuffer, 0, this.byteBuffer.Length);
					if (this.byteLen == 0)
					{
						goto Block_5;
					}
				}
				this._isBlocked = this.byteLen < this.byteBuffer.Length;
				if (!this.IsPreamble())
				{
					if (this._detectEncoding && this.byteLen >= 2)
					{
						this.DetectEncoding();
					}
					this.charLen += this.decoder.GetChars(this.byteBuffer, 0, this.byteLen, this.charBuffer, this.charLen);
				}
				if (this.charLen != 0)
				{
					goto Block_9;
				}
			}
			if (this.byteLen > 0)
			{
				this.charLen += this.decoder.GetChars(this.byteBuffer, 0, this.byteLen, this.charBuffer, this.charLen);
			}
			return this.charLen;
			Block_5:
			return this.charLen;
			Block_9:
			return this.charLen;
		}

		// Token: 0x060036AD RID: 13997 RVA: 0x000B9DB0 File Offset: 0x000B8DB0
		private int ReadBuffer(char[] userBuffer, int userOffset, int desiredChars, out bool readToUserBuffer)
		{
			this.charLen = 0;
			this.charPos = 0;
			if (!this._checkPreamble)
			{
				this.byteLen = 0;
			}
			int num = 0;
			readToUserBuffer = desiredChars >= this._maxCharsPerBuffer;
			for (;;)
			{
				if (this._checkPreamble)
				{
					int num2 = this.stream.Read(this.byteBuffer, this.bytePos, this.byteBuffer.Length - this.bytePos);
					if (num2 == 0)
					{
						break;
					}
					this.byteLen += num2;
				}
				else
				{
					this.byteLen = this.stream.Read(this.byteBuffer, 0, this.byteBuffer.Length);
					if (this.byteLen == 0)
					{
						return num;
					}
				}
				this._isBlocked = this.byteLen < this.byteBuffer.Length;
				if (!this.IsPreamble())
				{
					if (this._detectEncoding && this.byteLen >= 2)
					{
						this.DetectEncoding();
						readToUserBuffer = desiredChars >= this._maxCharsPerBuffer;
					}
					this.charPos = 0;
					if (readToUserBuffer)
					{
						num += this.decoder.GetChars(this.byteBuffer, 0, this.byteLen, userBuffer, userOffset + num);
						this.charLen = 0;
					}
					else
					{
						num = this.decoder.GetChars(this.byteBuffer, 0, this.byteLen, this.charBuffer, num);
						this.charLen += num;
					}
				}
				if (num != 0)
				{
					goto Block_11;
				}
			}
			if (this.byteLen > 0)
			{
				if (readToUserBuffer)
				{
					num += this.decoder.GetChars(this.byteBuffer, 0, this.byteLen, userBuffer, userOffset + num);
					this.charLen = 0;
				}
				else
				{
					num = this.decoder.GetChars(this.byteBuffer, 0, this.byteLen, this.charBuffer, num);
					this.charLen += num;
				}
			}
			return num;
			Block_11:
			this._isBlocked &= num < desiredChars;
			return num;
		}

		// Token: 0x060036AE RID: 13998 RVA: 0x000B9F84 File Offset: 0x000B8F84
		public override string ReadLine()
		{
			if (this.stream == null)
			{
				__Error.ReaderClosed();
			}
			if (this.charPos == this.charLen && this.ReadBuffer() == 0)
			{
				return null;
			}
			StringBuilder stringBuilder = null;
			int num;
			char c;
			for (;;)
			{
				num = this.charPos;
				do
				{
					c = this.charBuffer[num];
					if (c == '\r' || c == '\n')
					{
						goto IL_0044;
					}
					num++;
				}
				while (num < this.charLen);
				num = this.charLen - this.charPos;
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder(num + 80);
				}
				stringBuilder.Append(this.charBuffer, this.charPos, num);
				if (this.ReadBuffer() <= 0)
				{
					goto Block_11;
				}
			}
			IL_0044:
			string text;
			if (stringBuilder != null)
			{
				stringBuilder.Append(this.charBuffer, this.charPos, num - this.charPos);
				text = stringBuilder.ToString();
			}
			else
			{
				text = new string(this.charBuffer, this.charPos, num - this.charPos);
			}
			this.charPos = num + 1;
			if (c == '\r' && (this.charPos < this.charLen || this.ReadBuffer() > 0) && this.charBuffer[this.charPos] == '\n')
			{
				this.charPos++;
			}
			return text;
			Block_11:
			return stringBuilder.ToString();
		}

		// Token: 0x04001C5E RID: 7262
		internal const int DefaultBufferSize = 1024;

		// Token: 0x04001C5F RID: 7263
		private const int DefaultFileStreamBufferSize = 4096;

		// Token: 0x04001C60 RID: 7264
		private const int MinBufferSize = 128;

		// Token: 0x04001C61 RID: 7265
		public new static readonly StreamReader Null = new StreamReader.NullStreamReader();

		// Token: 0x04001C62 RID: 7266
		private bool _closable;

		// Token: 0x04001C63 RID: 7267
		private Stream stream;

		// Token: 0x04001C64 RID: 7268
		private Encoding encoding;

		// Token: 0x04001C65 RID: 7269
		private Decoder decoder;

		// Token: 0x04001C66 RID: 7270
		private byte[] byteBuffer;

		// Token: 0x04001C67 RID: 7271
		private char[] charBuffer;

		// Token: 0x04001C68 RID: 7272
		private byte[] _preamble;

		// Token: 0x04001C69 RID: 7273
		private int charPos;

		// Token: 0x04001C6A RID: 7274
		private int charLen;

		// Token: 0x04001C6B RID: 7275
		private int byteLen;

		// Token: 0x04001C6C RID: 7276
		private int bytePos;

		// Token: 0x04001C6D RID: 7277
		private int _maxCharsPerBuffer;

		// Token: 0x04001C6E RID: 7278
		private bool _detectEncoding;

		// Token: 0x04001C6F RID: 7279
		private bool _checkPreamble;

		// Token: 0x04001C70 RID: 7280
		private bool _isBlocked;

		// Token: 0x020005B3 RID: 1459
		private class NullStreamReader : StreamReader
		{
			// Token: 0x060036B0 RID: 14000 RVA: 0x000BA0B9 File Offset: 0x000B90B9
			internal NullStreamReader()
				: base(Stream.Null, Encoding.Unicode, false, 1)
			{
			}

			// Token: 0x1700094C RID: 2380
			// (get) Token: 0x060036B1 RID: 14001 RVA: 0x000BA0CD File Offset: 0x000B90CD
			public override Stream BaseStream
			{
				get
				{
					return Stream.Null;
				}
			}

			// Token: 0x1700094D RID: 2381
			// (get) Token: 0x060036B2 RID: 14002 RVA: 0x000BA0D4 File Offset: 0x000B90D4
			public override Encoding CurrentEncoding
			{
				get
				{
					return Encoding.Unicode;
				}
			}

			// Token: 0x060036B3 RID: 14003 RVA: 0x000BA0DB File Offset: 0x000B90DB
			protected override void Dispose(bool disposing)
			{
			}

			// Token: 0x060036B4 RID: 14004 RVA: 0x000BA0DD File Offset: 0x000B90DD
			public override int Peek()
			{
				return -1;
			}

			// Token: 0x060036B5 RID: 14005 RVA: 0x000BA0E0 File Offset: 0x000B90E0
			public override int Read()
			{
				return -1;
			}

			// Token: 0x060036B6 RID: 14006 RVA: 0x000BA0E3 File Offset: 0x000B90E3
			public override int Read(char[] buffer, int index, int count)
			{
				return 0;
			}

			// Token: 0x060036B7 RID: 14007 RVA: 0x000BA0E6 File Offset: 0x000B90E6
			public override string ReadLine()
			{
				return null;
			}

			// Token: 0x060036B8 RID: 14008 RVA: 0x000BA0E9 File Offset: 0x000B90E9
			public override string ReadToEnd()
			{
				return string.Empty;
			}
		}
	}
}
