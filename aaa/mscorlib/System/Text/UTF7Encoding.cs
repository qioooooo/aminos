using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Text
{
	// Token: 0x02000408 RID: 1032
	[ComVisible(true)]
	[Serializable]
	public class UTF7Encoding : Encoding
	{
		// Token: 0x06002AB9 RID: 10937 RVA: 0x0008B726 File Offset: 0x0008A726
		public UTF7Encoding()
			: this(false)
		{
		}

		// Token: 0x06002ABA RID: 10938 RVA: 0x0008B72F File Offset: 0x0008A72F
		public UTF7Encoding(bool allowOptionals)
			: base(65000)
		{
			this.m_allowOptionals = allowOptionals;
			this.MakeTables();
		}

		// Token: 0x06002ABB RID: 10939 RVA: 0x0008B74C File Offset: 0x0008A74C
		private void MakeTables()
		{
			this.base64Bytes = new byte[64];
			for (int i = 0; i < 64; i++)
			{
				this.base64Bytes[i] = (byte)"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"[i];
			}
			this.base64Values = new sbyte[128];
			for (int j = 0; j < 128; j++)
			{
				this.base64Values[j] = -1;
			}
			for (int k = 0; k < 64; k++)
			{
				this.base64Values[(int)this.base64Bytes[k]] = (sbyte)k;
			}
			this.directEncode = new bool[128];
			int num = "\t\n\r '(),-./0123456789:?ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".Length;
			for (int l = 0; l < num; l++)
			{
				this.directEncode[(int)"\t\n\r '(),-./0123456789:?ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"[l]] = true;
			}
			if (this.m_allowOptionals)
			{
				num = "!\"#$%&*;<=>@[]^_`{|}".Length;
				for (int m = 0; m < num; m++)
				{
					this.directEncode[(int)"!\"#$%&*;<=>@[]^_`{|}"[m]] = true;
				}
			}
		}

		// Token: 0x06002ABC RID: 10940 RVA: 0x0008B844 File Offset: 0x0008A844
		internal override void SetDefaultFallbacks()
		{
			this.encoderFallback = new EncoderReplacementFallback(string.Empty);
			this.decoderFallback = new UTF7Encoding.DecoderUTF7Fallback();
		}

		// Token: 0x06002ABD RID: 10941 RVA: 0x0008B861 File Offset: 0x0008A861
		[OnDeserializing]
		private void OnDeserializing(StreamingContext ctx)
		{
			base.OnDeserializing();
		}

		// Token: 0x06002ABE RID: 10942 RVA: 0x0008B869 File Offset: 0x0008A869
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			base.OnDeserialized();
			if (this.m_deserializedFromEverett)
			{
				this.m_allowOptionals = this.directEncode[(int)"!\"#$%&*;<=>@[]^_`{|}"[0]];
			}
			this.MakeTables();
		}

		// Token: 0x06002ABF RID: 10943 RVA: 0x0008B898 File Offset: 0x0008A898
		[ComVisible(false)]
		public override bool Equals(object value)
		{
			UTF7Encoding utf7Encoding = value as UTF7Encoding;
			return utf7Encoding != null && (this.m_allowOptionals == utf7Encoding.m_allowOptionals && base.EncoderFallback.Equals(utf7Encoding.EncoderFallback)) && base.DecoderFallback.Equals(utf7Encoding.DecoderFallback);
		}

		// Token: 0x06002AC0 RID: 10944 RVA: 0x0008B8E5 File Offset: 0x0008A8E5
		[ComVisible(false)]
		public override int GetHashCode()
		{
			return this.CodePage + base.EncoderFallback.GetHashCode() + base.DecoderFallback.GetHashCode();
		}

		// Token: 0x06002AC1 RID: 10945 RVA: 0x0008B908 File Offset: 0x0008A908
		public unsafe override int GetByteCount(char[] chars, int index, int count)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (chars.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("chars", Environment.GetResourceString("ArgumentOutOfRange_IndexCountBuffer"));
			}
			if (chars.Length == 0)
			{
				return 0;
			}
			fixed (char* ptr = chars)
			{
				return this.GetByteCount(ptr + index, count, null);
			}
		}

		// Token: 0x06002AC2 RID: 10946 RVA: 0x0008B9A4 File Offset: 0x0008A9A4
		[ComVisible(false)]
		public unsafe override int GetByteCount(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			IntPtr intPtr2;
			IntPtr intPtr = (intPtr2 = s);
			if (intPtr != 0)
			{
				intPtr2 = (IntPtr)((int)intPtr + RuntimeHelpers.OffsetToStringData);
			}
			char* ptr = intPtr2;
			return this.GetByteCount(ptr, s.Length, null);
		}

		// Token: 0x06002AC3 RID: 10947 RVA: 0x0008B9DF File Offset: 0x0008A9DF
		[ComVisible(false)]
		[CLSCompliant(false)]
		public unsafe override int GetByteCount(char* chars, int count)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			return this.GetByteCount(chars, count, null);
		}

		// Token: 0x06002AC4 RID: 10948 RVA: 0x0008BA20 File Offset: 0x0008AA20
		[ComVisible(false)]
		public unsafe override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			if (s == null || bytes == null)
			{
				throw new ArgumentNullException((s == null) ? "s" : "bytes", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (charIndex < 0 || charCount < 0)
			{
				throw new ArgumentOutOfRangeException((charIndex < 0) ? "charIndex" : "charCount", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (s.Length - charIndex < charCount)
			{
				throw new ArgumentOutOfRangeException("s", Environment.GetResourceString("ArgumentOutOfRange_IndexCount"));
			}
			if (byteIndex < 0 || byteIndex > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("byteIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			int num = bytes.Length - byteIndex;
			if (bytes.Length == 0)
			{
				bytes = new byte[1];
			}
			IntPtr intPtr2;
			IntPtr intPtr = (intPtr2 = s);
			if (intPtr != 0)
			{
				intPtr2 = (IntPtr)((int)intPtr + RuntimeHelpers.OffsetToStringData);
			}
			char* ptr = intPtr2;
			fixed (byte* ptr2 = bytes)
			{
				return this.GetBytes(ptr + charIndex, charCount, ptr2 + byteIndex, num, null);
			}
		}

		// Token: 0x06002AC5 RID: 10949 RVA: 0x0008BB18 File Offset: 0x0008AB18
		public unsafe override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			if (chars == null || bytes == null)
			{
				throw new ArgumentNullException((chars == null) ? "chars" : "bytes", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (charIndex < 0 || charCount < 0)
			{
				throw new ArgumentOutOfRangeException((charIndex < 0) ? "charIndex" : "charCount", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (chars.Length - charIndex < charCount)
			{
				throw new ArgumentOutOfRangeException("chars", Environment.GetResourceString("ArgumentOutOfRange_IndexCountBuffer"));
			}
			if (byteIndex < 0 || byteIndex > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("byteIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (chars.Length == 0)
			{
				return 0;
			}
			int num = bytes.Length - byteIndex;
			if (bytes.Length == 0)
			{
				bytes = new byte[1];
			}
			fixed (char* ptr = chars)
			{
				fixed (byte* ptr2 = bytes)
				{
					return this.GetBytes(ptr + charIndex, charCount, ptr2 + byteIndex, num, null);
				}
			}
		}

		// Token: 0x06002AC6 RID: 10950 RVA: 0x0008BC20 File Offset: 0x0008AC20
		[ComVisible(false)]
		[CLSCompliant(false)]
		public unsafe override int GetBytes(char* chars, int charCount, byte* bytes, int byteCount)
		{
			if (bytes == null || chars == null)
			{
				throw new ArgumentNullException((bytes == null) ? "bytes" : "chars", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (charCount < 0 || byteCount < 0)
			{
				throw new ArgumentOutOfRangeException((charCount < 0) ? "charCount" : "byteCount", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			return this.GetBytes(chars, charCount, bytes, byteCount, null);
		}

		// Token: 0x06002AC7 RID: 10951 RVA: 0x0008BC90 File Offset: 0x0008AC90
		public unsafe override int GetCharCount(byte[] bytes, int index, int count)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (bytes.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("bytes", Environment.GetResourceString("ArgumentOutOfRange_IndexCountBuffer"));
			}
			if (bytes.Length == 0)
			{
				return 0;
			}
			fixed (byte* ptr = bytes)
			{
				return this.GetCharCount(ptr + index, count, null);
			}
		}

		// Token: 0x06002AC8 RID: 10952 RVA: 0x0008BD28 File Offset: 0x0008AD28
		[ComVisible(false)]
		[CLSCompliant(false)]
		public unsafe override int GetCharCount(byte* bytes, int count)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			return this.GetCharCount(bytes, count, null);
		}

		// Token: 0x06002AC9 RID: 10953 RVA: 0x0008BD68 File Offset: 0x0008AD68
		public unsafe override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			if (bytes == null || chars == null)
			{
				throw new ArgumentNullException((bytes == null) ? "bytes" : "chars", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (byteIndex < 0 || byteCount < 0)
			{
				throw new ArgumentOutOfRangeException((byteIndex < 0) ? "byteIndex" : "byteCount", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (bytes.Length - byteIndex < byteCount)
			{
				throw new ArgumentOutOfRangeException("bytes", Environment.GetResourceString("ArgumentOutOfRange_IndexCountBuffer"));
			}
			if (charIndex < 0 || charIndex > chars.Length)
			{
				throw new ArgumentOutOfRangeException("charIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (bytes.Length == 0)
			{
				return 0;
			}
			int num = chars.Length - charIndex;
			if (chars.Length == 0)
			{
				chars = new char[1];
			}
			fixed (byte* ptr = bytes)
			{
				fixed (char* ptr2 = chars)
				{
					return this.GetChars(ptr + byteIndex, byteCount, ptr2 + charIndex, num, null);
				}
			}
		}

		// Token: 0x06002ACA RID: 10954 RVA: 0x0008BE70 File Offset: 0x0008AE70
		[ComVisible(false)]
		[CLSCompliant(false)]
		public unsafe override int GetChars(byte* bytes, int byteCount, char* chars, int charCount)
		{
			if (bytes == null || chars == null)
			{
				throw new ArgumentNullException((bytes == null) ? "bytes" : "chars", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (charCount < 0 || byteCount < 0)
			{
				throw new ArgumentOutOfRangeException((charCount < 0) ? "charCount" : "byteCount", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			return this.GetChars(bytes, byteCount, chars, charCount, null);
		}

		// Token: 0x06002ACB RID: 10955 RVA: 0x0008BEE0 File Offset: 0x0008AEE0
		[ComVisible(false)]
		public unsafe override string GetString(byte[] bytes, int index, int count)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (bytes.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("bytes", Environment.GetResourceString("ArgumentOutOfRange_IndexCountBuffer"));
			}
			if (bytes.Length == 0)
			{
				return string.Empty;
			}
			fixed (byte* ptr = bytes)
			{
				return string.CreateStringFromEncoding(ptr + index, count, this);
			}
		}

		// Token: 0x06002ACC RID: 10956 RVA: 0x0008BF7B File Offset: 0x0008AF7B
		internal unsafe override int GetByteCount(char* chars, int count, EncoderNLS baseEncoder)
		{
			return this.GetBytes(chars, count, null, 0, baseEncoder);
		}

		// Token: 0x06002ACD RID: 10957 RVA: 0x0008BF8C File Offset: 0x0008AF8C
		internal unsafe override int GetBytes(char* chars, int charCount, byte* bytes, int byteCount, EncoderNLS baseEncoder)
		{
			UTF7Encoding.Encoder encoder = (UTF7Encoding.Encoder)baseEncoder;
			int num = 0;
			int i = -1;
			Encoding.EncodingByteBuffer encodingByteBuffer = new Encoding.EncodingByteBuffer(this, encoder, bytes, byteCount, chars, charCount);
			if (encoder != null)
			{
				num = encoder.bits;
				i = encoder.bitCount;
				while (i >= 6)
				{
					i -= 6;
					if (!encodingByteBuffer.AddByte(this.base64Bytes[(num >> i) & 63]))
					{
						base.ThrowBytesOverflow(encoder, encodingByteBuffer.Count == 0);
					}
				}
			}
			while (encodingByteBuffer.MoreData)
			{
				char c = encodingByteBuffer.GetNextChar();
				if (c < '\u0080' && this.directEncode[(int)c])
				{
					if (i >= 0)
					{
						if (i > 0)
						{
							if (!encodingByteBuffer.AddByte(this.base64Bytes[(num << 6 - i) & 63]))
							{
								break;
							}
							i = 0;
						}
						if (!encodingByteBuffer.AddByte(45))
						{
							break;
						}
						i = -1;
					}
					if (!encodingByteBuffer.AddByte((byte)c))
					{
						break;
					}
				}
				else if (i < 0 && c == '+')
				{
					if (!encodingByteBuffer.AddByte(43, 45))
					{
						break;
					}
				}
				else
				{
					if (i < 0)
					{
						if (!encodingByteBuffer.AddByte(43))
						{
							break;
						}
						i = 0;
					}
					num = (num << 16) | (int)c;
					i += 16;
					while (i >= 6)
					{
						i -= 6;
						if (!encodingByteBuffer.AddByte(this.base64Bytes[(num >> i) & 63]))
						{
							i += 6;
							c = encodingByteBuffer.GetNextChar();
							break;
						}
					}
					if (i >= 6)
					{
						break;
					}
				}
			}
			if (i >= 0 && (encoder == null || encoder.MustFlush))
			{
				if (i > 0 && encodingByteBuffer.AddByte(this.base64Bytes[(num << 6 - i) & 63]))
				{
					i = 0;
				}
				if (encodingByteBuffer.AddByte(45))
				{
					num = 0;
					i = -1;
				}
				else
				{
					encodingByteBuffer.GetNextChar();
				}
			}
			if (bytes != null && encoder != null)
			{
				encoder.bits = num;
				encoder.bitCount = i;
				encoder.m_charsUsed = encodingByteBuffer.CharsUsed;
			}
			return encodingByteBuffer.Count;
		}

		// Token: 0x06002ACE RID: 10958 RVA: 0x0008C13E File Offset: 0x0008B13E
		internal unsafe override int GetCharCount(byte* bytes, int count, DecoderNLS baseDecoder)
		{
			return this.GetChars(bytes, count, null, 0, baseDecoder);
		}

		// Token: 0x06002ACF RID: 10959 RVA: 0x0008C14C File Offset: 0x0008B14C
		internal unsafe override int GetChars(byte* bytes, int byteCount, char* chars, int charCount, DecoderNLS baseDecoder)
		{
			UTF7Encoding.Decoder decoder = (UTF7Encoding.Decoder)baseDecoder;
			Encoding.EncodingCharBuffer encodingCharBuffer = new Encoding.EncodingCharBuffer(this, decoder, chars, charCount, bytes, byteCount);
			int num = 0;
			int num2 = -1;
			bool flag = false;
			if (decoder != null)
			{
				num = decoder.bits;
				num2 = decoder.bitCount;
				flag = decoder.firstByte;
			}
			if (num2 >= 16)
			{
				if (!encodingCharBuffer.AddChar((char)((num >> num2 - 16) & 65535)))
				{
					base.ThrowCharsOverflow(decoder, true);
				}
				num2 -= 16;
			}
			while (encodingCharBuffer.MoreData)
			{
				byte nextByte = encodingCharBuffer.GetNextByte();
				int num3;
				if (num2 >= 0)
				{
					sbyte b;
					if (nextByte < 128 && (b = this.base64Values[(int)nextByte]) >= 0)
					{
						flag = false;
						num = (num << 6) | (int)((byte)b);
						num2 += 6;
						if (num2 < 16)
						{
							continue;
						}
						num3 = (num >> num2 - 16) & 65535;
						num2 -= 16;
					}
					else
					{
						num2 = -1;
						if (nextByte != 45)
						{
							if (!encodingCharBuffer.Fallback(nextByte))
							{
								break;
							}
							continue;
						}
						else
						{
							if (!flag)
							{
								continue;
							}
							num3 = 43;
						}
					}
				}
				else
				{
					if (nextByte == 43)
					{
						num2 = 0;
						flag = true;
						continue;
					}
					if (nextByte >= 128)
					{
						if (!encodingCharBuffer.Fallback(nextByte))
						{
							break;
						}
						continue;
					}
					else
					{
						num3 = (int)nextByte;
					}
				}
				if (num3 >= 0 && !encodingCharBuffer.AddChar((char)num3))
				{
					if (num2 >= 0)
					{
						encodingCharBuffer.AdjustBytes(1);
						num2 += 16;
						break;
					}
					break;
				}
			}
			if (chars != null && decoder != null)
			{
				if (decoder.MustFlush)
				{
					decoder.bits = 0;
					decoder.bitCount = -1;
					decoder.firstByte = false;
				}
				else
				{
					decoder.bits = num;
					decoder.bitCount = num2;
					decoder.firstByte = flag;
				}
				decoder.m_bytesUsed = encodingCharBuffer.BytesUsed;
			}
			return encodingCharBuffer.Count;
		}

		// Token: 0x06002AD0 RID: 10960 RVA: 0x0008C2D0 File Offset: 0x0008B2D0
		public override global::System.Text.Decoder GetDecoder()
		{
			return new UTF7Encoding.Decoder(this);
		}

		// Token: 0x06002AD1 RID: 10961 RVA: 0x0008C2D8 File Offset: 0x0008B2D8
		public override global::System.Text.Encoder GetEncoder()
		{
			return new UTF7Encoding.Encoder(this);
		}

		// Token: 0x06002AD2 RID: 10962 RVA: 0x0008C2E0 File Offset: 0x0008B2E0
		public override int GetMaxByteCount(int charCount)
		{
			if (charCount < 0)
			{
				throw new ArgumentOutOfRangeException("charCount", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			long num = (long)charCount * 3L + 2L;
			if (num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("charCount", Environment.GetResourceString("ArgumentOutOfRange_GetByteCountOverflow"));
			}
			return (int)num;
		}

		// Token: 0x06002AD3 RID: 10963 RVA: 0x0008C330 File Offset: 0x0008B330
		public override int GetMaxCharCount(int byteCount)
		{
			if (byteCount < 0)
			{
				throw new ArgumentOutOfRangeException("byteCount", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			int num = byteCount;
			if (num == 0)
			{
				num = 1;
			}
			return num;
		}

		// Token: 0x040014C8 RID: 5320
		private const string base64Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

		// Token: 0x040014C9 RID: 5321
		private const string directChars = "\t\n\r '(),-./0123456789:?ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

		// Token: 0x040014CA RID: 5322
		private const string optionalChars = "!\"#$%&*;<=>@[]^_`{|}";

		// Token: 0x040014CB RID: 5323
		private const int UTF7_CODEPAGE = 65000;

		// Token: 0x040014CC RID: 5324
		private byte[] base64Bytes;

		// Token: 0x040014CD RID: 5325
		private sbyte[] base64Values;

		// Token: 0x040014CE RID: 5326
		private bool[] directEncode;

		// Token: 0x040014CF RID: 5327
		[OptionalField(VersionAdded = 2)]
		private bool m_allowOptionals;

		// Token: 0x02000409 RID: 1033
		[Serializable]
		private class Decoder : DecoderNLS, ISerializable
		{
			// Token: 0x06002AD4 RID: 10964 RVA: 0x0008C35E File Offset: 0x0008B35E
			public Decoder(UTF7Encoding encoding)
				: base(encoding)
			{
			}

			// Token: 0x06002AD5 RID: 10965 RVA: 0x0008C368 File Offset: 0x0008B368
			internal Decoder(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				this.bits = (int)info.GetValue("bits", typeof(int));
				this.bitCount = (int)info.GetValue("bitCount", typeof(int));
				this.firstByte = (bool)info.GetValue("firstByte", typeof(bool));
				this.m_encoding = (Encoding)info.GetValue("encoding", typeof(Encoding));
			}

			// Token: 0x06002AD6 RID: 10966 RVA: 0x0008C40C File Offset: 0x0008B40C
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				info.AddValue("encoding", this.m_encoding);
				info.AddValue("bits", this.bits);
				info.AddValue("bitCount", this.bitCount);
				info.AddValue("firstByte", this.firstByte);
			}

			// Token: 0x06002AD7 RID: 10967 RVA: 0x0008C46B File Offset: 0x0008B46B
			public override void Reset()
			{
				this.bits = 0;
				this.bitCount = -1;
				this.firstByte = false;
				if (this.m_fallbackBuffer != null)
				{
					this.m_fallbackBuffer.Reset();
				}
			}

			// Token: 0x17000810 RID: 2064
			// (get) Token: 0x06002AD8 RID: 10968 RVA: 0x0008C495 File Offset: 0x0008B495
			internal override bool HasState
			{
				get
				{
					return this.bitCount != -1;
				}
			}

			// Token: 0x040014D0 RID: 5328
			internal int bits;

			// Token: 0x040014D1 RID: 5329
			internal int bitCount;

			// Token: 0x040014D2 RID: 5330
			internal bool firstByte;
		}

		// Token: 0x0200040A RID: 1034
		[Serializable]
		private class Encoder : EncoderNLS, ISerializable
		{
			// Token: 0x06002AD9 RID: 10969 RVA: 0x0008C4A3 File Offset: 0x0008B4A3
			public Encoder(UTF7Encoding encoding)
				: base(encoding)
			{
			}

			// Token: 0x06002ADA RID: 10970 RVA: 0x0008C4AC File Offset: 0x0008B4AC
			internal Encoder(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				this.bits = (int)info.GetValue("bits", typeof(int));
				this.bitCount = (int)info.GetValue("bitCount", typeof(int));
				this.m_encoding = (Encoding)info.GetValue("encoding", typeof(Encoding));
			}

			// Token: 0x06002ADB RID: 10971 RVA: 0x0008C530 File Offset: 0x0008B530
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				info.AddValue("encoding", this.m_encoding);
				info.AddValue("bits", this.bits);
				info.AddValue("bitCount", this.bitCount);
			}

			// Token: 0x06002ADC RID: 10972 RVA: 0x0008C57E File Offset: 0x0008B57E
			public override void Reset()
			{
				this.bitCount = -1;
				this.bits = 0;
				if (this.m_fallbackBuffer != null)
				{
					this.m_fallbackBuffer.Reset();
				}
			}

			// Token: 0x17000811 RID: 2065
			// (get) Token: 0x06002ADD RID: 10973 RVA: 0x0008C5A1 File Offset: 0x0008B5A1
			internal override bool HasState
			{
				get
				{
					return this.bits != 0 || this.bitCount != -1;
				}
			}

			// Token: 0x040014D3 RID: 5331
			internal int bits;

			// Token: 0x040014D4 RID: 5332
			internal int bitCount;
		}

		// Token: 0x0200040B RID: 1035
		[Serializable]
		internal sealed class DecoderUTF7Fallback : DecoderFallback
		{
			// Token: 0x06002ADF RID: 10975 RVA: 0x0008C5C1 File Offset: 0x0008B5C1
			public override DecoderFallbackBuffer CreateFallbackBuffer()
			{
				return new UTF7Encoding.DecoderUTF7FallbackBuffer(this);
			}

			// Token: 0x17000812 RID: 2066
			// (get) Token: 0x06002AE0 RID: 10976 RVA: 0x0008C5C9 File Offset: 0x0008B5C9
			public override int MaxCharCount
			{
				get
				{
					return 1;
				}
			}

			// Token: 0x06002AE1 RID: 10977 RVA: 0x0008C5CC File Offset: 0x0008B5CC
			public override bool Equals(object value)
			{
				return value is UTF7Encoding.DecoderUTF7Fallback;
			}

			// Token: 0x06002AE2 RID: 10978 RVA: 0x0008C5E6 File Offset: 0x0008B5E6
			public override int GetHashCode()
			{
				return 984;
			}
		}

		// Token: 0x0200040C RID: 1036
		internal sealed class DecoderUTF7FallbackBuffer : DecoderFallbackBuffer
		{
			// Token: 0x06002AE3 RID: 10979 RVA: 0x0008C5ED File Offset: 0x0008B5ED
			public DecoderUTF7FallbackBuffer(UTF7Encoding.DecoderUTF7Fallback fallback)
			{
			}

			// Token: 0x06002AE4 RID: 10980 RVA: 0x0008C5FC File Offset: 0x0008B5FC
			public override bool Fallback(byte[] bytesUnknown, int index)
			{
				this.cFallback = (char)bytesUnknown[0];
				this.iCount = (this.iSize = 1);
				return true;
			}

			// Token: 0x06002AE5 RID: 10981 RVA: 0x0008C624 File Offset: 0x0008B624
			public override char GetNextChar()
			{
				if (this.iCount-- > 0)
				{
					return this.cFallback;
				}
				return '\0';
			}

			// Token: 0x06002AE6 RID: 10982 RVA: 0x0008C64D File Offset: 0x0008B64D
			public override bool MovePrevious()
			{
				if (this.iCount >= 0)
				{
					this.iCount++;
				}
				return this.iCount >= 0 && this.iCount <= this.iSize;
			}

			// Token: 0x17000813 RID: 2067
			// (get) Token: 0x06002AE7 RID: 10983 RVA: 0x0008C682 File Offset: 0x0008B682
			public override int Remaining
			{
				get
				{
					if (this.iCount <= 0)
					{
						return 0;
					}
					return this.iCount;
				}
			}

			// Token: 0x06002AE8 RID: 10984 RVA: 0x0008C695 File Offset: 0x0008B695
			public override void Reset()
			{
				this.iCount = -1;
				this.byteStart = null;
			}

			// Token: 0x06002AE9 RID: 10985 RVA: 0x0008C6A6 File Offset: 0x0008B6A6
			internal unsafe override int InternalFallback(byte[] bytes, byte* pBytes)
			{
				if (bytes[0] != 0)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x040014D5 RID: 5333
			private char cFallback;

			// Token: 0x040014D6 RID: 5334
			private int iCount = -1;

			// Token: 0x040014D7 RID: 5335
			private int iSize;
		}
	}
}
