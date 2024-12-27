using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace System.IO
{
	// Token: 0x020005B7 RID: 1463
	[ComVisible(true)]
	[Serializable]
	public class StreamWriter : TextWriter
	{
		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x0600371A RID: 14106 RVA: 0x000BA8D4 File Offset: 0x000B98D4
		internal static Encoding UTF8NoBOM
		{
			get
			{
				if (StreamWriter._UTF8NoBOM == null)
				{
					UTF8Encoding utf8Encoding = new UTF8Encoding(false, true);
					Thread.MemoryBarrier();
					StreamWriter._UTF8NoBOM = utf8Encoding;
				}
				return StreamWriter._UTF8NoBOM;
			}
		}

		// Token: 0x0600371B RID: 14107 RVA: 0x000BA900 File Offset: 0x000B9900
		internal StreamWriter()
			: base(null)
		{
		}

		// Token: 0x0600371C RID: 14108 RVA: 0x000BA909 File Offset: 0x000B9909
		public StreamWriter(Stream stream)
			: this(stream, StreamWriter.UTF8NoBOM, 1024)
		{
		}

		// Token: 0x0600371D RID: 14109 RVA: 0x000BA91C File Offset: 0x000B991C
		public StreamWriter(Stream stream, Encoding encoding)
			: this(stream, encoding, 1024)
		{
		}

		// Token: 0x0600371E RID: 14110 RVA: 0x000BA92C File Offset: 0x000B992C
		public StreamWriter(Stream stream, Encoding encoding, int bufferSize)
			: base(null)
		{
			if (stream == null || encoding == null)
			{
				throw new ArgumentNullException((stream == null) ? "stream" : "encoding");
			}
			if (!stream.CanWrite)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_StreamNotWritable"));
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			this.Init(stream, encoding, bufferSize);
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x000BA995 File Offset: 0x000B9995
		internal StreamWriter(Stream stream, Encoding encoding, int bufferSize, bool closeable)
			: this(stream, encoding, bufferSize)
		{
			this.closable = closeable;
		}

		// Token: 0x06003720 RID: 14112 RVA: 0x000BA9A8 File Offset: 0x000B99A8
		public StreamWriter(string path)
			: this(path, false, StreamWriter.UTF8NoBOM, 1024)
		{
		}

		// Token: 0x06003721 RID: 14113 RVA: 0x000BA9BC File Offset: 0x000B99BC
		public StreamWriter(string path, bool append)
			: this(path, append, StreamWriter.UTF8NoBOM, 1024)
		{
		}

		// Token: 0x06003722 RID: 14114 RVA: 0x000BA9D0 File Offset: 0x000B99D0
		public StreamWriter(string path, bool append, Encoding encoding)
			: this(path, append, encoding, 1024)
		{
		}

		// Token: 0x06003723 RID: 14115 RVA: 0x000BA9E0 File Offset: 0x000B99E0
		public StreamWriter(string path, bool append, Encoding encoding, int bufferSize)
			: base(null)
		{
			if (path == null || encoding == null)
			{
				throw new ArgumentNullException((path == null) ? "path" : "encoding");
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			Stream stream = StreamWriter.CreateFile(path, append);
			this.Init(stream, encoding, bufferSize);
		}

		// Token: 0x06003724 RID: 14116 RVA: 0x000BAA3C File Offset: 0x000B9A3C
		private void Init(Stream stream, Encoding encoding, int bufferSize)
		{
			this.stream = stream;
			this.encoding = encoding;
			this.encoder = encoding.GetEncoder();
			if (bufferSize < 128)
			{
				bufferSize = 128;
			}
			this.charBuffer = new char[bufferSize];
			this.byteBuffer = new byte[encoding.GetMaxByteCount(bufferSize)];
			this.charLen = bufferSize;
			if (stream.CanSeek && stream.Position > 0L)
			{
				this.haveWrittenPreamble = true;
			}
			this.closable = true;
			if (Mda.StreamWriterBufferMDAEnabled)
			{
				string stackTrace = Environment.GetStackTrace(null, false);
				this.mdaHelper = new MdaHelper(this, stackTrace);
			}
		}

		// Token: 0x06003725 RID: 14117 RVA: 0x000BAAD4 File Offset: 0x000B9AD4
		private static Stream CreateFile(string path, bool append)
		{
			FileMode fileMode = (append ? FileMode.Append : FileMode.Create);
			return new FileStream(path, fileMode, FileAccess.Write, FileShare.Read, 4096, FileOptions.SequentialScan);
		}

		// Token: 0x06003726 RID: 14118 RVA: 0x000BAAFE File Offset: 0x000B9AFE
		public override void Close()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003727 RID: 14119 RVA: 0x000BAB10 File Offset: 0x000B9B10
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (this.stream != null && (disposing || (!this.Closable && this.stream is __ConsoleStream)))
				{
					this.Flush(true, true);
					if (this.mdaHelper != null)
					{
						GC.SuppressFinalize(this.mdaHelper);
					}
				}
			}
			finally
			{
				if (this.Closable && this.stream != null)
				{
					try
					{
						if (disposing)
						{
							this.stream.Close();
						}
					}
					finally
					{
						this.stream = null;
						this.byteBuffer = null;
						this.charBuffer = null;
						this.encoding = null;
						this.encoder = null;
						this.charLen = 0;
						base.Dispose(disposing);
					}
				}
			}
		}

		// Token: 0x06003728 RID: 14120 RVA: 0x000BABCC File Offset: 0x000B9BCC
		public override void Flush()
		{
			this.Flush(true, true);
		}

		// Token: 0x06003729 RID: 14121 RVA: 0x000BABD8 File Offset: 0x000B9BD8
		private void Flush(bool flushStream, bool flushEncoder)
		{
			if (this.stream == null)
			{
				__Error.WriterClosed();
			}
			if (this.charPos == 0 && !flushStream && !flushEncoder)
			{
				return;
			}
			if (!this.haveWrittenPreamble)
			{
				this.haveWrittenPreamble = true;
				byte[] preamble = this.encoding.GetPreamble();
				if (preamble.Length > 0)
				{
					this.stream.Write(preamble, 0, preamble.Length);
				}
			}
			int bytes = this.encoder.GetBytes(this.charBuffer, 0, this.charPos, this.byteBuffer, 0, flushEncoder);
			this.charPos = 0;
			if (bytes > 0)
			{
				this.stream.Write(this.byteBuffer, 0, bytes);
			}
			if (flushStream)
			{
				this.stream.Flush();
			}
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x0600372A RID: 14122 RVA: 0x000BAC7F File Offset: 0x000B9C7F
		// (set) Token: 0x0600372B RID: 14123 RVA: 0x000BAC87 File Offset: 0x000B9C87
		public virtual bool AutoFlush
		{
			get
			{
				return this.autoFlush;
			}
			set
			{
				this.autoFlush = value;
				if (value)
				{
					this.Flush(true, false);
				}
			}
		}

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x0600372C RID: 14124 RVA: 0x000BAC9B File Offset: 0x000B9C9B
		public virtual Stream BaseStream
		{
			get
			{
				return this.stream;
			}
		}

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x0600372D RID: 14125 RVA: 0x000BACA3 File Offset: 0x000B9CA3
		internal bool Closable
		{
			get
			{
				return this.closable;
			}
		}

		// Token: 0x17000959 RID: 2393
		// (set) Token: 0x0600372E RID: 14126 RVA: 0x000BACAB File Offset: 0x000B9CAB
		internal bool HaveWrittenPreamble
		{
			set
			{
				this.haveWrittenPreamble = value;
			}
		}

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x0600372F RID: 14127 RVA: 0x000BACB4 File Offset: 0x000B9CB4
		public override Encoding Encoding
		{
			get
			{
				return this.encoding;
			}
		}

		// Token: 0x06003730 RID: 14128 RVA: 0x000BACBC File Offset: 0x000B9CBC
		public override void Write(char value)
		{
			if (this.charPos == this.charLen)
			{
				this.Flush(false, false);
			}
			this.charBuffer[this.charPos] = value;
			this.charPos++;
			if (this.autoFlush)
			{
				this.Flush(true, false);
			}
		}

		// Token: 0x06003731 RID: 14129 RVA: 0x000BAD0C File Offset: 0x000B9D0C
		public override void Write(char[] buffer)
		{
			if (buffer == null)
			{
				return;
			}
			int num = 0;
			int num2;
			for (int i = buffer.Length; i > 0; i -= num2)
			{
				if (this.charPos == this.charLen)
				{
					this.Flush(false, false);
				}
				num2 = this.charLen - this.charPos;
				if (num2 > i)
				{
					num2 = i;
				}
				Buffer.InternalBlockCopy(buffer, num * 2, this.charBuffer, this.charPos * 2, num2 * 2);
				this.charPos += num2;
				num += num2;
			}
			if (this.autoFlush)
			{
				this.Flush(true, false);
			}
		}

		// Token: 0x06003732 RID: 14130 RVA: 0x000BAD94 File Offset: 0x000B9D94
		public override void Write(char[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", Environment.GetResourceString("ArgumentNull_Buffer"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			while (count > 0)
			{
				if (this.charPos == this.charLen)
				{
					this.Flush(false, false);
				}
				int num = this.charLen - this.charPos;
				if (num > count)
				{
					num = count;
				}
				Buffer.InternalBlockCopy(buffer, index * 2, this.charBuffer, this.charPos * 2, num * 2);
				this.charPos += num;
				index += num;
				count -= num;
			}
			if (this.autoFlush)
			{
				this.Flush(true, false);
			}
		}

		// Token: 0x06003733 RID: 14131 RVA: 0x000BAE74 File Offset: 0x000B9E74
		public override void Write(string value)
		{
			if (value != null)
			{
				int i = value.Length;
				int num = 0;
				while (i > 0)
				{
					if (this.charPos == this.charLen)
					{
						this.Flush(false, false);
					}
					int num2 = this.charLen - this.charPos;
					if (num2 > i)
					{
						num2 = i;
					}
					value.CopyTo(num, this.charBuffer, this.charPos, num2);
					this.charPos += num2;
					num += num2;
					i -= num2;
				}
				if (this.autoFlush)
				{
					this.Flush(true, false);
				}
			}
		}

		// Token: 0x04001C76 RID: 7286
		private const int DefaultBufferSize = 1024;

		// Token: 0x04001C77 RID: 7287
		private const int DefaultFileStreamBufferSize = 4096;

		// Token: 0x04001C78 RID: 7288
		private const int MinBufferSize = 128;

		// Token: 0x04001C79 RID: 7289
		public new static readonly StreamWriter Null = new StreamWriter(Stream.Null, new UTF8Encoding(false, true), 128, false);

		// Token: 0x04001C7A RID: 7290
		internal Stream stream;

		// Token: 0x04001C7B RID: 7291
		private Encoding encoding;

		// Token: 0x04001C7C RID: 7292
		private Encoder encoder;

		// Token: 0x04001C7D RID: 7293
		internal byte[] byteBuffer;

		// Token: 0x04001C7E RID: 7294
		internal char[] charBuffer;

		// Token: 0x04001C7F RID: 7295
		internal int charPos;

		// Token: 0x04001C80 RID: 7296
		internal int charLen;

		// Token: 0x04001C81 RID: 7297
		internal bool autoFlush;

		// Token: 0x04001C82 RID: 7298
		private bool haveWrittenPreamble;

		// Token: 0x04001C83 RID: 7299
		private bool closable;

		// Token: 0x04001C84 RID: 7300
		[NonSerialized]
		private MdaHelper mdaHelper;

		// Token: 0x04001C85 RID: 7301
		private static Encoding _UTF8NoBOM;
	}
}
