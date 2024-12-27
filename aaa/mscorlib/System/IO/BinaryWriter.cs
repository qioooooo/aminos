using System;
using System.Runtime.InteropServices;
using System.Text;

namespace System.IO
{
	// Token: 0x02000591 RID: 1425
	[ComVisible(true)]
	[Serializable]
	public class BinaryWriter : IDisposable
	{
		// Token: 0x060034B1 RID: 13489 RVA: 0x000AF2FC File Offset: 0x000AE2FC
		protected BinaryWriter()
		{
			this.OutStream = Stream.Null;
			this._buffer = new byte[16];
			this._encoding = new UTF8Encoding(false, true);
			this._encoder = this._encoding.GetEncoder();
		}

		// Token: 0x060034B2 RID: 13490 RVA: 0x000AF351 File Offset: 0x000AE351
		public BinaryWriter(Stream output)
			: this(output, new UTF8Encoding(false, true))
		{
		}

		// Token: 0x060034B3 RID: 13491 RVA: 0x000AF364 File Offset: 0x000AE364
		public BinaryWriter(Stream output, Encoding encoding)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (!output.CanWrite)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_StreamNotWritable"));
			}
			this.OutStream = output;
			this._buffer = new byte[16];
			this._encoding = encoding;
			this._encoder = this._encoding.GetEncoder();
		}

		// Token: 0x060034B4 RID: 13492 RVA: 0x000AF3E3 File Offset: 0x000AE3E3
		public virtual void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x060034B5 RID: 13493 RVA: 0x000AF3EC File Offset: 0x000AE3EC
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.OutStream.Close();
			}
		}

		// Token: 0x060034B6 RID: 13494 RVA: 0x000AF3FC File Offset: 0x000AE3FC
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x060034B7 RID: 13495 RVA: 0x000AF405 File Offset: 0x000AE405
		public virtual Stream BaseStream
		{
			get
			{
				this.Flush();
				return this.OutStream;
			}
		}

		// Token: 0x060034B8 RID: 13496 RVA: 0x000AF413 File Offset: 0x000AE413
		public virtual void Flush()
		{
			this.OutStream.Flush();
		}

		// Token: 0x060034B9 RID: 13497 RVA: 0x000AF420 File Offset: 0x000AE420
		public virtual long Seek(int offset, SeekOrigin origin)
		{
			return this.OutStream.Seek((long)offset, origin);
		}

		// Token: 0x060034BA RID: 13498 RVA: 0x000AF430 File Offset: 0x000AE430
		public virtual void Write(bool value)
		{
			this._buffer[0] = (value ? 1 : 0);
			this.OutStream.Write(this._buffer, 0, 1);
		}

		// Token: 0x060034BB RID: 13499 RVA: 0x000AF455 File Offset: 0x000AE455
		public virtual void Write(byte value)
		{
			this.OutStream.WriteByte(value);
		}

		// Token: 0x060034BC RID: 13500 RVA: 0x000AF463 File Offset: 0x000AE463
		[CLSCompliant(false)]
		public virtual void Write(sbyte value)
		{
			this.OutStream.WriteByte((byte)value);
		}

		// Token: 0x060034BD RID: 13501 RVA: 0x000AF472 File Offset: 0x000AE472
		public virtual void Write(byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			this.OutStream.Write(buffer, 0, buffer.Length);
		}

		// Token: 0x060034BE RID: 13502 RVA: 0x000AF492 File Offset: 0x000AE492
		public virtual void Write(byte[] buffer, int index, int count)
		{
			this.OutStream.Write(buffer, index, count);
		}

		// Token: 0x060034BF RID: 13503 RVA: 0x000AF4A4 File Offset: 0x000AE4A4
		public unsafe virtual void Write(char ch)
		{
			if (char.IsSurrogate(ch))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_SurrogatesNotAllowedAsSingleChar"));
			}
			int bytes;
			fixed (byte* buffer = this._buffer)
			{
				bytes = this._encoder.GetBytes(&ch, 1, buffer, 16, true);
			}
			this.OutStream.Write(this._buffer, 0, bytes);
		}

		// Token: 0x060034C0 RID: 13504 RVA: 0x000AF514 File Offset: 0x000AE514
		public virtual void Write(char[] chars)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			byte[] bytes = this._encoding.GetBytes(chars, 0, chars.Length);
			this.OutStream.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x060034C1 RID: 13505 RVA: 0x000AF550 File Offset: 0x000AE550
		public virtual void Write(char[] chars, int index, int count)
		{
			byte[] bytes = this._encoding.GetBytes(chars, index, count);
			this.OutStream.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x060034C2 RID: 13506 RVA: 0x000AF57C File Offset: 0x000AE57C
		public unsafe virtual void Write(double value)
		{
			ulong num = (ulong)(*(long*)(&value));
			this._buffer[0] = (byte)num;
			this._buffer[1] = (byte)(num >> 8);
			this._buffer[2] = (byte)(num >> 16);
			this._buffer[3] = (byte)(num >> 24);
			this._buffer[4] = (byte)(num >> 32);
			this._buffer[5] = (byte)(num >> 40);
			this._buffer[6] = (byte)(num >> 48);
			this._buffer[7] = (byte)(num >> 56);
			this.OutStream.Write(this._buffer, 0, 8);
		}

		// Token: 0x060034C3 RID: 13507 RVA: 0x000AF605 File Offset: 0x000AE605
		public virtual void Write(decimal value)
		{
			decimal.GetBytes(value, this._buffer);
			this.OutStream.Write(this._buffer, 0, 16);
		}

		// Token: 0x060034C4 RID: 13508 RVA: 0x000AF627 File Offset: 0x000AE627
		public virtual void Write(short value)
		{
			this._buffer[0] = (byte)value;
			this._buffer[1] = (byte)(value >> 8);
			this.OutStream.Write(this._buffer, 0, 2);
		}

		// Token: 0x060034C5 RID: 13509 RVA: 0x000AF652 File Offset: 0x000AE652
		[CLSCompliant(false)]
		public virtual void Write(ushort value)
		{
			this._buffer[0] = (byte)value;
			this._buffer[1] = (byte)(value >> 8);
			this.OutStream.Write(this._buffer, 0, 2);
		}

		// Token: 0x060034C6 RID: 13510 RVA: 0x000AF680 File Offset: 0x000AE680
		public virtual void Write(int value)
		{
			this._buffer[0] = (byte)value;
			this._buffer[1] = (byte)(value >> 8);
			this._buffer[2] = (byte)(value >> 16);
			this._buffer[3] = (byte)(value >> 24);
			this.OutStream.Write(this._buffer, 0, 4);
		}

		// Token: 0x060034C7 RID: 13511 RVA: 0x000AF6D0 File Offset: 0x000AE6D0
		[CLSCompliant(false)]
		public virtual void Write(uint value)
		{
			this._buffer[0] = (byte)value;
			this._buffer[1] = (byte)(value >> 8);
			this._buffer[2] = (byte)(value >> 16);
			this._buffer[3] = (byte)(value >> 24);
			this.OutStream.Write(this._buffer, 0, 4);
		}

		// Token: 0x060034C8 RID: 13512 RVA: 0x000AF720 File Offset: 0x000AE720
		public virtual void Write(long value)
		{
			this._buffer[0] = (byte)value;
			this._buffer[1] = (byte)(value >> 8);
			this._buffer[2] = (byte)(value >> 16);
			this._buffer[3] = (byte)(value >> 24);
			this._buffer[4] = (byte)(value >> 32);
			this._buffer[5] = (byte)(value >> 40);
			this._buffer[6] = (byte)(value >> 48);
			this._buffer[7] = (byte)(value >> 56);
			this.OutStream.Write(this._buffer, 0, 8);
		}

		// Token: 0x060034C9 RID: 13513 RVA: 0x000AF7A4 File Offset: 0x000AE7A4
		[CLSCompliant(false)]
		public virtual void Write(ulong value)
		{
			this._buffer[0] = (byte)value;
			this._buffer[1] = (byte)(value >> 8);
			this._buffer[2] = (byte)(value >> 16);
			this._buffer[3] = (byte)(value >> 24);
			this._buffer[4] = (byte)(value >> 32);
			this._buffer[5] = (byte)(value >> 40);
			this._buffer[6] = (byte)(value >> 48);
			this._buffer[7] = (byte)(value >> 56);
			this.OutStream.Write(this._buffer, 0, 8);
		}

		// Token: 0x060034CA RID: 13514 RVA: 0x000AF828 File Offset: 0x000AE828
		public unsafe virtual void Write(float value)
		{
			uint num = *(uint*)(&value);
			this._buffer[0] = (byte)num;
			this._buffer[1] = (byte)(num >> 8);
			this._buffer[2] = (byte)(num >> 16);
			this._buffer[3] = (byte)(num >> 24);
			this.OutStream.Write(this._buffer, 0, 4);
		}

		// Token: 0x060034CB RID: 13515 RVA: 0x000AF880 File Offset: 0x000AE880
		public unsafe virtual void Write(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			int byteCount = this._encoding.GetByteCount(value);
			this.Write7BitEncodedInt(byteCount);
			if (this._largeByteBuffer == null)
			{
				this._largeByteBuffer = new byte[256];
				this._maxChars = 256 / this._encoding.GetMaxByteCount(1);
			}
			if (byteCount <= 256)
			{
				this._encoding.GetBytes(value, 0, value.Length, this._largeByteBuffer, 0);
				this.OutStream.Write(this._largeByteBuffer, 0, byteCount);
				return;
			}
			int num = 0;
			int num2;
			for (int i = value.Length; i > 0; i -= num2)
			{
				num2 = ((i > this._maxChars) ? this._maxChars : i);
				int bytes;
				fixed (char* ptr = value)
				{
					fixed (byte* largeByteBuffer = this._largeByteBuffer)
					{
						bytes = this._encoder.GetBytes(ptr + num, num2, largeByteBuffer, 256, num2 == i);
					}
				}
				this.OutStream.Write(this._largeByteBuffer, 0, bytes);
				num += num2;
			}
		}

		// Token: 0x060034CC RID: 13516 RVA: 0x000AF9B4 File Offset: 0x000AE9B4
		protected void Write7BitEncodedInt(int value)
		{
			uint num;
			for (num = (uint)value; num >= 128U; num >>= 7)
			{
				this.Write((byte)(num | 128U));
			}
			this.Write((byte)num);
		}

		// Token: 0x04001BB0 RID: 7088
		private const int LargeByteBufferSize = 256;

		// Token: 0x04001BB1 RID: 7089
		public static readonly BinaryWriter Null = new BinaryWriter();

		// Token: 0x04001BB2 RID: 7090
		protected Stream OutStream;

		// Token: 0x04001BB3 RID: 7091
		private byte[] _buffer;

		// Token: 0x04001BB4 RID: 7092
		private Encoding _encoding;

		// Token: 0x04001BB5 RID: 7093
		private Encoder _encoder;

		// Token: 0x04001BB6 RID: 7094
		private char[] _tmpOneCharBuffer = new char[1];

		// Token: 0x04001BB7 RID: 7095
		private byte[] _largeByteBuffer;

		// Token: 0x04001BB8 RID: 7096
		private int _maxChars;
	}
}
