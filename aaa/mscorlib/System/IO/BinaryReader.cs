using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.IO
{
	// Token: 0x02000590 RID: 1424
	[ComVisible(true)]
	public class BinaryReader : IDisposable
	{
		// Token: 0x06003493 RID: 13459 RVA: 0x000AE898 File Offset: 0x000AD898
		public BinaryReader(Stream input)
			: this(input, new UTF8Encoding())
		{
		}

		// Token: 0x06003494 RID: 13460 RVA: 0x000AE8A8 File Offset: 0x000AD8A8
		public BinaryReader(Stream input, Encoding encoding)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (!input.CanRead)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_StreamNotReadable"));
			}
			this.m_stream = input;
			this.m_decoder = encoding.GetDecoder();
			this.m_maxCharsSize = encoding.GetMaxCharCount(128);
			int num = encoding.GetMaxByteCount(1);
			if (num < 16)
			{
				num = 16;
			}
			this.m_buffer = new byte[num];
			this.m_charBuffer = null;
			this.m_charBytes = null;
			this.m_2BytesPerChar = encoding is UnicodeEncoding;
			this.m_isMemoryStream = this.m_stream.GetType() == typeof(MemoryStream);
		}

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x06003495 RID: 13461 RVA: 0x000AE969 File Offset: 0x000AD969
		public virtual Stream BaseStream
		{
			get
			{
				return this.m_stream;
			}
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x000AE971 File Offset: 0x000AD971
		public virtual void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x06003497 RID: 13463 RVA: 0x000AE97C File Offset: 0x000AD97C
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				Stream stream = this.m_stream;
				this.m_stream = null;
				if (stream != null)
				{
					stream.Close();
				}
			}
			this.m_stream = null;
			this.m_buffer = null;
			this.m_decoder = null;
			this.m_charBytes = null;
			this.m_singleChar = null;
			this.m_charBuffer = null;
		}

		// Token: 0x06003498 RID: 13464 RVA: 0x000AE9CD File Offset: 0x000AD9CD
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06003499 RID: 13465 RVA: 0x000AE9D8 File Offset: 0x000AD9D8
		public virtual int PeekChar()
		{
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			if (!this.m_stream.CanSeek)
			{
				return -1;
			}
			long position = this.m_stream.Position;
			int num = this.Read();
			this.m_stream.Position = position;
			return num;
		}

		// Token: 0x0600349A RID: 13466 RVA: 0x000AEA21 File Offset: 0x000ADA21
		public virtual int Read()
		{
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			return this.InternalReadOneChar();
		}

		// Token: 0x0600349B RID: 13467 RVA: 0x000AEA36 File Offset: 0x000ADA36
		public virtual bool ReadBoolean()
		{
			this.FillBuffer(1);
			return this.m_buffer[0] != 0;
		}

		// Token: 0x0600349C RID: 13468 RVA: 0x000AEA50 File Offset: 0x000ADA50
		public virtual byte ReadByte()
		{
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			int num = this.m_stream.ReadByte();
			if (num == -1)
			{
				__Error.EndOfFile();
			}
			return (byte)num;
		}

		// Token: 0x0600349D RID: 13469 RVA: 0x000AEA81 File Offset: 0x000ADA81
		[CLSCompliant(false)]
		public virtual sbyte ReadSByte()
		{
			this.FillBuffer(1);
			return (sbyte)this.m_buffer[0];
		}

		// Token: 0x0600349E RID: 13470 RVA: 0x000AEA94 File Offset: 0x000ADA94
		public virtual char ReadChar()
		{
			int num = this.Read();
			if (num == -1)
			{
				__Error.EndOfFile();
			}
			return (char)num;
		}

		// Token: 0x0600349F RID: 13471 RVA: 0x000AEAB3 File Offset: 0x000ADAB3
		public virtual short ReadInt16()
		{
			this.FillBuffer(2);
			return (short)((int)this.m_buffer[0] | ((int)this.m_buffer[1] << 8));
		}

		// Token: 0x060034A0 RID: 13472 RVA: 0x000AEAD0 File Offset: 0x000ADAD0
		[CLSCompliant(false)]
		public virtual ushort ReadUInt16()
		{
			this.FillBuffer(2);
			return (ushort)((int)this.m_buffer[0] | ((int)this.m_buffer[1] << 8));
		}

		// Token: 0x060034A1 RID: 13473 RVA: 0x000AEAF0 File Offset: 0x000ADAF0
		public virtual int ReadInt32()
		{
			if (this.m_isMemoryStream)
			{
				MemoryStream memoryStream = this.m_stream as MemoryStream;
				return memoryStream.InternalReadInt32();
			}
			this.FillBuffer(4);
			return (int)this.m_buffer[0] | ((int)this.m_buffer[1] << 8) | ((int)this.m_buffer[2] << 16) | ((int)this.m_buffer[3] << 24);
		}

		// Token: 0x060034A2 RID: 13474 RVA: 0x000AEB4A File Offset: 0x000ADB4A
		[CLSCompliant(false)]
		public virtual uint ReadUInt32()
		{
			this.FillBuffer(4);
			return (uint)((int)this.m_buffer[0] | ((int)this.m_buffer[1] << 8) | ((int)this.m_buffer[2] << 16) | ((int)this.m_buffer[3] << 24));
		}

		// Token: 0x060034A3 RID: 13475 RVA: 0x000AEB80 File Offset: 0x000ADB80
		public virtual long ReadInt64()
		{
			this.FillBuffer(8);
			uint num = (uint)((int)this.m_buffer[0] | ((int)this.m_buffer[1] << 8) | ((int)this.m_buffer[2] << 16) | ((int)this.m_buffer[3] << 24));
			uint num2 = (uint)((int)this.m_buffer[4] | ((int)this.m_buffer[5] << 8) | ((int)this.m_buffer[6] << 16) | ((int)this.m_buffer[7] << 24));
			return (long)(((ulong)num2 << 32) | (ulong)num);
		}

		// Token: 0x060034A4 RID: 13476 RVA: 0x000AEBF4 File Offset: 0x000ADBF4
		[CLSCompliant(false)]
		public virtual ulong ReadUInt64()
		{
			this.FillBuffer(8);
			uint num = (uint)((int)this.m_buffer[0] | ((int)this.m_buffer[1] << 8) | ((int)this.m_buffer[2] << 16) | ((int)this.m_buffer[3] << 24));
			uint num2 = (uint)((int)this.m_buffer[4] | ((int)this.m_buffer[5] << 8) | ((int)this.m_buffer[6] << 16) | ((int)this.m_buffer[7] << 24));
			return ((ulong)num2 << 32) | (ulong)num;
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x000AEC68 File Offset: 0x000ADC68
		public unsafe virtual float ReadSingle()
		{
			this.FillBuffer(4);
			uint num = (uint)((int)this.m_buffer[0] | ((int)this.m_buffer[1] << 8) | ((int)this.m_buffer[2] << 16) | ((int)this.m_buffer[3] << 24));
			return *(float*)(&num);
		}

		// Token: 0x060034A6 RID: 13478 RVA: 0x000AECAC File Offset: 0x000ADCAC
		public unsafe virtual double ReadDouble()
		{
			this.FillBuffer(8);
			uint num = (uint)((int)this.m_buffer[0] | ((int)this.m_buffer[1] << 8) | ((int)this.m_buffer[2] << 16) | ((int)this.m_buffer[3] << 24));
			uint num2 = (uint)((int)this.m_buffer[4] | ((int)this.m_buffer[5] << 8) | ((int)this.m_buffer[6] << 16) | ((int)this.m_buffer[7] << 24));
			ulong num3 = ((ulong)num2 << 32) | (ulong)num;
			return *(double*)(&num3);
		}

		// Token: 0x060034A7 RID: 13479 RVA: 0x000AED25 File Offset: 0x000ADD25
		public virtual decimal ReadDecimal()
		{
			this.FillBuffer(16);
			return decimal.ToDecimal(this.m_buffer);
		}

		// Token: 0x060034A8 RID: 13480 RVA: 0x000AED3C File Offset: 0x000ADD3C
		public virtual string ReadString()
		{
			int num = 0;
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			int num2 = this.Read7BitEncodedInt();
			if (num2 < 0)
			{
				throw new IOException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.IO_InvalidStringLen_Len"), new object[] { num2 }));
			}
			if (num2 == 0)
			{
				return string.Empty;
			}
			if (this.m_charBytes == null)
			{
				this.m_charBytes = new byte[128];
			}
			if (this.m_charBuffer == null)
			{
				this.m_charBuffer = new char[this.m_maxCharsSize];
			}
			StringBuilder stringBuilder = null;
			int chars;
			for (;;)
			{
				int num3 = ((num2 - num > 128) ? 128 : (num2 - num));
				int num4 = this.m_stream.Read(this.m_charBytes, 0, num3);
				if (num4 == 0)
				{
					__Error.EndOfFile();
				}
				chars = this.m_decoder.GetChars(this.m_charBytes, 0, num4, this.m_charBuffer, 0);
				if (num == 0 && num4 == num2)
				{
					break;
				}
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder(Math.Min(num2, 360));
				}
				stringBuilder.Append(this.m_charBuffer, 0, chars);
				num += num4;
				if (num >= num2)
				{
					goto Block_11;
				}
			}
			return new string(this.m_charBuffer, 0, chars);
			Block_11:
			return stringBuilder.ToString();
		}

		// Token: 0x060034A9 RID: 13481 RVA: 0x000AEE6C File Offset: 0x000ADE6C
		public virtual int Read(char[] buffer, int index, int count)
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
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			return this.InternalReadChars(buffer, index, count);
		}

		// Token: 0x060034AA RID: 13482 RVA: 0x000AEEF4 File Offset: 0x000ADEF4
		private int InternalReadChars(char[] buffer, int index, int count)
		{
			int i = count;
			if (this.m_charBytes == null)
			{
				this.m_charBytes = new byte[128];
			}
			while (i > 0)
			{
				int num = i;
				if (this.m_2BytesPerChar)
				{
					num <<= 1;
				}
				if (num > 128)
				{
					num = 128;
				}
				int num3;
				if (this.m_isMemoryStream)
				{
					MemoryStream memoryStream = this.m_stream as MemoryStream;
					int num2 = memoryStream.InternalGetPosition();
					num = memoryStream.InternalEmulateRead(num);
					if (num == 0)
					{
						return count - i;
					}
					num3 = this.m_decoder.GetChars(memoryStream.InternalGetBuffer(), num2, num, buffer, index);
				}
				else
				{
					num = this.m_stream.Read(this.m_charBytes, 0, num);
					if (num == 0)
					{
						return count - i;
					}
					num3 = this.m_decoder.GetChars(this.m_charBytes, 0, num, buffer, index);
				}
				i -= num3;
				index += num3;
			}
			return count;
		}

		// Token: 0x060034AB RID: 13483 RVA: 0x000AEFCC File Offset: 0x000ADFCC
		private int InternalReadOneChar()
		{
			int num = 0;
			long num2 = (num2 = 0L);
			if (this.m_stream.CanSeek)
			{
				num2 = this.m_stream.Position;
			}
			if (this.m_charBytes == null)
			{
				this.m_charBytes = new byte[128];
			}
			if (this.m_singleChar == null)
			{
				this.m_singleChar = new char[1];
			}
			while (num == 0)
			{
				int num3 = (this.m_2BytesPerChar ? 2 : 1);
				int num4 = this.m_stream.ReadByte();
				this.m_charBytes[0] = (byte)num4;
				if (num4 == -1)
				{
					num3 = 0;
				}
				if (num3 == 2)
				{
					num4 = this.m_stream.ReadByte();
					this.m_charBytes[1] = (byte)num4;
					if (num4 == -1)
					{
						num3 = 1;
					}
				}
				if (num3 == 0)
				{
					return -1;
				}
				try
				{
					num = this.m_decoder.GetChars(this.m_charBytes, 0, num3, this.m_singleChar, 0);
				}
				catch
				{
					if (this.m_stream.CanSeek)
					{
						this.m_stream.Seek(num2 - this.m_stream.Position, SeekOrigin.Current);
					}
					throw;
				}
			}
			if (num == 0)
			{
				return -1;
			}
			return (int)this.m_singleChar[0];
		}

		// Token: 0x060034AC RID: 13484 RVA: 0x000AF0E8 File Offset: 0x000AE0E8
		public virtual char[] ReadChars(int count)
		{
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			char[] array = new char[count];
			int num = this.InternalReadChars(array, 0, count);
			if (num != count)
			{
				char[] array2 = new char[num];
				Buffer.InternalBlockCopy(array, 0, array2, 0, 2 * num);
				array = array2;
			}
			return array;
		}

		// Token: 0x060034AD RID: 13485 RVA: 0x000AF148 File Offset: 0x000AE148
		public virtual int Read(byte[] buffer, int index, int count)
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
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			return this.m_stream.Read(buffer, index, count);
		}

		// Token: 0x060034AE RID: 13486 RVA: 0x000AF1D4 File Offset: 0x000AE1D4
		public virtual byte[] ReadBytes(int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			byte[] array = new byte[count];
			int num = 0;
			do
			{
				int num2 = this.m_stream.Read(array, num, count);
				if (num2 == 0)
				{
					break;
				}
				num += num2;
				count -= num2;
			}
			while (count > 0);
			if (num != array.Length)
			{
				byte[] array2 = new byte[num];
				Buffer.InternalBlockCopy(array, 0, array2, 0, num);
				array = array2;
			}
			return array;
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x000AF24C File Offset: 0x000AE24C
		protected virtual void FillBuffer(int numBytes)
		{
			int num = 0;
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			if (numBytes == 1)
			{
				int num2 = this.m_stream.ReadByte();
				if (num2 == -1)
				{
					__Error.EndOfFile();
				}
				this.m_buffer[0] = (byte)num2;
				return;
			}
			do
			{
				int num2 = this.m_stream.Read(this.m_buffer, num, numBytes - num);
				if (num2 == 0)
				{
					__Error.EndOfFile();
				}
				num += num2;
			}
			while (num < numBytes);
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x000AF2B4 File Offset: 0x000AE2B4
		protected internal int Read7BitEncodedInt()
		{
			int num = 0;
			int num2 = 0;
			while (num2 != 35)
			{
				byte b = this.ReadByte();
				num |= (int)(b & 127) << num2;
				num2 += 7;
				if ((b & 128) == 0)
				{
					return num;
				}
			}
			throw new FormatException(Environment.GetResourceString("Format_Bad7BitInt32"));
		}

		// Token: 0x04001BA5 RID: 7077
		private const int MaxCharBytesSize = 128;

		// Token: 0x04001BA6 RID: 7078
		private const int MaxStringBuilderCapacity = 360;

		// Token: 0x04001BA7 RID: 7079
		private Stream m_stream;

		// Token: 0x04001BA8 RID: 7080
		private byte[] m_buffer;

		// Token: 0x04001BA9 RID: 7081
		private Decoder m_decoder;

		// Token: 0x04001BAA RID: 7082
		private byte[] m_charBytes;

		// Token: 0x04001BAB RID: 7083
		private char[] m_singleChar;

		// Token: 0x04001BAC RID: 7084
		private char[] m_charBuffer;

		// Token: 0x04001BAD RID: 7085
		private int m_maxCharsSize;

		// Token: 0x04001BAE RID: 7086
		private bool m_2BytesPerChar;

		// Token: 0x04001BAF RID: 7087
		private bool m_isMemoryStream;
	}
}
