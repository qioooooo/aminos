﻿using System;
using System.Runtime.CompilerServices;

namespace System.Text
{
	// Token: 0x02000410 RID: 1040
	[Serializable]
	public sealed class UTF32Encoding : Encoding
	{
		// Token: 0x06002B14 RID: 11028 RVA: 0x0008E654 File Offset: 0x0008D654
		public UTF32Encoding()
			: this(false, true, false)
		{
		}

		// Token: 0x06002B15 RID: 11029 RVA: 0x0008E65F File Offset: 0x0008D65F
		public UTF32Encoding(bool bigEndian, bool byteOrderMark)
			: this(bigEndian, byteOrderMark, false)
		{
		}

		// Token: 0x06002B16 RID: 11030 RVA: 0x0008E66A File Offset: 0x0008D66A
		public UTF32Encoding(bool bigEndian, bool byteOrderMark, bool throwOnInvalidCharacters)
			: base(bigEndian ? 12001 : 12000)
		{
			this.bigEndian = bigEndian;
			this.emitUTF32ByteOrderMark = byteOrderMark;
			this.isThrowException = throwOnInvalidCharacters;
			if (this.isThrowException)
			{
				this.SetDefaultFallbacks();
			}
		}

		// Token: 0x06002B17 RID: 11031 RVA: 0x0008E6A4 File Offset: 0x0008D6A4
		internal override void SetDefaultFallbacks()
		{
			if (this.isThrowException)
			{
				this.encoderFallback = EncoderFallback.ExceptionFallback;
				this.decoderFallback = DecoderFallback.ExceptionFallback;
				return;
			}
			this.encoderFallback = new EncoderReplacementFallback("\ufffd");
			this.decoderFallback = new DecoderReplacementFallback("\ufffd");
		}

		// Token: 0x06002B18 RID: 11032 RVA: 0x0008E6F0 File Offset: 0x0008D6F0
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

		// Token: 0x06002B19 RID: 11033 RVA: 0x0008E78C File Offset: 0x0008D78C
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

		// Token: 0x06002B1A RID: 11034 RVA: 0x0008E7C7 File Offset: 0x0008D7C7
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

		// Token: 0x06002B1B RID: 11035 RVA: 0x0008E808 File Offset: 0x0008D808
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

		// Token: 0x06002B1C RID: 11036 RVA: 0x0008E900 File Offset: 0x0008D900
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

		// Token: 0x06002B1D RID: 11037 RVA: 0x0008EA08 File Offset: 0x0008DA08
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

		// Token: 0x06002B1E RID: 11038 RVA: 0x0008EA78 File Offset: 0x0008DA78
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

		// Token: 0x06002B1F RID: 11039 RVA: 0x0008EB10 File Offset: 0x0008DB10
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

		// Token: 0x06002B20 RID: 11040 RVA: 0x0008EB50 File Offset: 0x0008DB50
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

		// Token: 0x06002B21 RID: 11041 RVA: 0x0008EC58 File Offset: 0x0008DC58
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

		// Token: 0x06002B22 RID: 11042 RVA: 0x0008ECC8 File Offset: 0x0008DCC8
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

		// Token: 0x06002B23 RID: 11043 RVA: 0x0008ED64 File Offset: 0x0008DD64
		internal unsafe override int GetByteCount(char* chars, int count, EncoderNLS encoder)
		{
			char* ptr = chars + count;
			char* ptr2 = chars;
			int num = 0;
			char c = '\0';
			EncoderFallbackBuffer encoderFallbackBuffer;
			if (encoder != null)
			{
				c = encoder.charLeftOver;
				encoderFallbackBuffer = encoder.FallbackBuffer;
				if (encoderFallbackBuffer.Remaining > 0)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_EncoderFallbackNotEmpty", new object[]
					{
						this.EncodingName,
						encoder.Fallback.GetType()
					}));
				}
			}
			else
			{
				encoderFallbackBuffer = this.encoderFallback.CreateFallbackBuffer();
			}
			encoderFallbackBuffer.InternalInitialize(ptr2, ptr, encoder, false);
			for (;;)
			{
				char c2;
				if ((c2 = encoderFallbackBuffer.InternalGetNextChar()) == '\0' && chars >= ptr)
				{
					if ((encoder != null && !encoder.MustFlush) || c <= '\0')
					{
						break;
					}
					encoderFallbackBuffer.InternalFallback(c, ref chars);
					c = '\0';
				}
				else
				{
					if (c2 == '\0')
					{
						c2 = *chars;
						chars++;
					}
					if (c != '\0')
					{
						if (char.IsLowSurrogate(c2))
						{
							c = '\0';
							num += 4;
						}
						else
						{
							chars--;
							encoderFallbackBuffer.InternalFallback(c, ref chars);
							c = '\0';
						}
					}
					else if (char.IsHighSurrogate(c2))
					{
						c = c2;
					}
					else if (char.IsLowSurrogate(c2))
					{
						encoderFallbackBuffer.InternalFallback(c2, ref chars);
					}
					else
					{
						num += 4;
					}
				}
			}
			if (num < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_GetByteCountOverflow"));
			}
			return num;
		}

		// Token: 0x06002B24 RID: 11044 RVA: 0x0008EE94 File Offset: 0x0008DE94
		internal unsafe override int GetBytes(char* chars, int charCount, byte* bytes, int byteCount, EncoderNLS encoder)
		{
			char* ptr = chars;
			char* ptr2 = chars + charCount;
			byte* ptr3 = bytes;
			byte* ptr4 = bytes + byteCount;
			char c = '\0';
			EncoderFallbackBuffer encoderFallbackBuffer;
			if (encoder != null)
			{
				c = encoder.charLeftOver;
				encoderFallbackBuffer = encoder.FallbackBuffer;
				if (encoder.m_throwOnOverflow && encoderFallbackBuffer.Remaining > 0)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_EncoderFallbackNotEmpty", new object[]
					{
						this.EncodingName,
						encoder.Fallback.GetType()
					}));
				}
			}
			else
			{
				encoderFallbackBuffer = this.encoderFallback.CreateFallbackBuffer();
			}
			encoderFallbackBuffer.InternalInitialize(ptr, ptr2, encoder, true);
			for (;;)
			{
				char c2;
				if ((c2 = encoderFallbackBuffer.InternalGetNextChar()) != '\0' || chars < ptr2)
				{
					if (c2 == '\0')
					{
						c2 = *chars;
						chars++;
					}
					if (c != '\0')
					{
						if (!char.IsLowSurrogate(c2))
						{
							chars--;
							encoderFallbackBuffer.InternalFallback(c, ref chars);
							c = '\0';
							continue;
						}
						uint surrogate = this.GetSurrogate(c, c2);
						c = '\0';
						if (bytes + 3 >= ptr4)
						{
							if (encoderFallbackBuffer.bFallingBack)
							{
								encoderFallbackBuffer.MovePrevious();
								encoderFallbackBuffer.MovePrevious();
							}
							else
							{
								chars -= 2;
							}
							base.ThrowBytesOverflow(encoder, bytes == ptr3);
							c = '\0';
						}
						else
						{
							if (this.bigEndian)
							{
								*(bytes++) = 0;
								*(bytes++) = (byte)(surrogate >> 16);
								*(bytes++) = (byte)(surrogate >> 8);
								*(bytes++) = (byte)surrogate;
								continue;
							}
							*(bytes++) = (byte)surrogate;
							*(bytes++) = (byte)(surrogate >> 8);
							*(bytes++) = (byte)(surrogate >> 16);
							*(bytes++) = 0;
							continue;
						}
					}
					else
					{
						if (char.IsHighSurrogate(c2))
						{
							c = c2;
							continue;
						}
						if (char.IsLowSurrogate(c2))
						{
							encoderFallbackBuffer.InternalFallback(c2, ref chars);
							continue;
						}
						if (bytes + 3 >= ptr4)
						{
							if (encoderFallbackBuffer.bFallingBack)
							{
								encoderFallbackBuffer.MovePrevious();
							}
							else
							{
								chars--;
							}
							base.ThrowBytesOverflow(encoder, bytes == ptr3);
						}
						else
						{
							if (this.bigEndian)
							{
								*(bytes++) = 0;
								*(bytes++) = 0;
								*(bytes++) = (byte)(c2 >> 8);
								*(bytes++) = (byte)c2;
								continue;
							}
							*(bytes++) = (byte)c2;
							*(bytes++) = (byte)(c2 >> 8);
							*(bytes++) = 0;
							*(bytes++) = 0;
							continue;
						}
					}
				}
				if ((encoder != null && !encoder.MustFlush) || c <= '\0')
				{
					break;
				}
				encoderFallbackBuffer.InternalFallback(c, ref chars);
				c = '\0';
			}
			if (encoder != null)
			{
				encoder.charLeftOver = c;
				encoder.m_charsUsed = (int)((long)(chars - ptr));
			}
			return (int)((long)(bytes - ptr3));
		}

		// Token: 0x06002B25 RID: 11045 RVA: 0x0008F13C File Offset: 0x0008E13C
		internal unsafe override int GetCharCount(byte* bytes, int count, DecoderNLS baseDecoder)
		{
			UTF32Encoding.UTF32Decoder utf32Decoder = (UTF32Encoding.UTF32Decoder)baseDecoder;
			int num = 0;
			byte* ptr = bytes + count;
			byte* ptr2 = bytes;
			int i = 0;
			uint num2 = 0U;
			DecoderFallbackBuffer decoderFallbackBuffer;
			if (utf32Decoder != null)
			{
				i = utf32Decoder.readByteCount;
				num2 = (uint)utf32Decoder.iChar;
				decoderFallbackBuffer = utf32Decoder.FallbackBuffer;
			}
			else
			{
				decoderFallbackBuffer = this.decoderFallback.CreateFallbackBuffer();
			}
			decoderFallbackBuffer.InternalInitialize(ptr2, null);
			while (bytes < ptr && num >= 0)
			{
				if (this.bigEndian)
				{
					num2 <<= 8;
					num2 += (uint)(*(bytes++));
				}
				else
				{
					num2 >>= 8;
					num2 += (uint)((uint)(*(bytes++)) << 24);
				}
				i++;
				if (i >= 4)
				{
					i = 0;
					if (num2 > 1114111U || (num2 >= 55296U && num2 <= 57343U))
					{
						byte[] array;
						if (this.bigEndian)
						{
							array = new byte[]
							{
								(byte)(num2 >> 24),
								(byte)(num2 >> 16),
								(byte)(num2 >> 8),
								(byte)num2
							};
						}
						else
						{
							array = new byte[]
							{
								(byte)num2,
								(byte)(num2 >> 8),
								(byte)(num2 >> 16),
								(byte)(num2 >> 24)
							};
						}
						num += decoderFallbackBuffer.InternalFallback(array, bytes);
						num2 = 0U;
					}
					else
					{
						if (num2 >= 65536U)
						{
							num++;
						}
						num++;
						num2 = 0U;
					}
				}
			}
			if (i > 0 && (utf32Decoder == null || utf32Decoder.MustFlush))
			{
				byte[] array2 = new byte[i];
				if (this.bigEndian)
				{
					while (i > 0)
					{
						array2[--i] = (byte)num2;
						num2 >>= 8;
					}
				}
				else
				{
					while (i > 0)
					{
						array2[--i] = (byte)(num2 >> 24);
						num2 <<= 8;
					}
				}
				num += decoderFallbackBuffer.InternalFallback(array2, bytes);
			}
			if (num < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_GetByteCountOverflow"));
			}
			return num;
		}

		// Token: 0x06002B26 RID: 11046 RVA: 0x0008F318 File Offset: 0x0008E318
		internal unsafe override int GetChars(byte* bytes, int byteCount, char* chars, int charCount, DecoderNLS baseDecoder)
		{
			UTF32Encoding.UTF32Decoder utf32Decoder = (UTF32Encoding.UTF32Decoder)baseDecoder;
			char* ptr = chars;
			char* ptr2 = chars + charCount;
			byte* ptr3 = bytes;
			byte* ptr4 = bytes + byteCount;
			int num = 0;
			uint num2 = 0U;
			DecoderFallbackBuffer decoderFallbackBuffer;
			if (utf32Decoder != null)
			{
				num = utf32Decoder.readByteCount;
				num2 = (uint)utf32Decoder.iChar;
				decoderFallbackBuffer = baseDecoder.FallbackBuffer;
			}
			else
			{
				decoderFallbackBuffer = this.decoderFallback.CreateFallbackBuffer();
			}
			decoderFallbackBuffer.InternalInitialize(bytes, chars + charCount);
			while (bytes < ptr4)
			{
				if (this.bigEndian)
				{
					num2 <<= 8;
					num2 += (uint)(*(bytes++));
				}
				else
				{
					num2 >>= 8;
					num2 += (uint)((uint)(*(bytes++)) << 24);
				}
				num++;
				if (num >= 4)
				{
					num = 0;
					if (num2 > 1114111U || (num2 >= 55296U && num2 <= 57343U))
					{
						byte[] array;
						if (this.bigEndian)
						{
							array = new byte[]
							{
								(byte)(num2 >> 24),
								(byte)(num2 >> 16),
								(byte)(num2 >> 8),
								(byte)num2
							};
						}
						else
						{
							array = new byte[]
							{
								(byte)num2,
								(byte)(num2 >> 8),
								(byte)(num2 >> 16),
								(byte)(num2 >> 24)
							};
						}
						if (!decoderFallbackBuffer.InternalFallback(array, bytes, ref chars))
						{
							bytes -= 4;
							num2 = 0U;
							decoderFallbackBuffer.InternalReset();
							base.ThrowCharsOverflow(utf32Decoder, chars == ptr);
							break;
						}
						num2 = 0U;
					}
					else
					{
						if (num2 >= 65536U)
						{
							if (chars >= ptr2 - 1)
							{
								bytes -= 4;
								num2 = 0U;
								base.ThrowCharsOverflow(utf32Decoder, chars == ptr);
								break;
							}
							*(chars++) = this.GetHighSurrogate(num2);
							num2 = (uint)this.GetLowSurrogate(num2);
						}
						else if (chars >= ptr2)
						{
							bytes -= 4;
							num2 = 0U;
							base.ThrowCharsOverflow(utf32Decoder, chars == ptr);
							break;
						}
						*(chars++) = (char)num2;
						num2 = 0U;
					}
				}
			}
			if (num > 0 && (utf32Decoder == null || utf32Decoder.MustFlush))
			{
				byte[] array2 = new byte[num];
				int i = num;
				if (this.bigEndian)
				{
					while (i > 0)
					{
						array2[--i] = (byte)num2;
						num2 >>= 8;
					}
				}
				else
				{
					while (i > 0)
					{
						array2[--i] = (byte)(num2 >> 24);
						num2 <<= 8;
					}
				}
				if (!decoderFallbackBuffer.InternalFallback(array2, bytes, ref chars))
				{
					decoderFallbackBuffer.InternalReset();
					base.ThrowCharsOverflow(utf32Decoder, chars == ptr);
				}
				else
				{
					num = 0;
					num2 = 0U;
				}
			}
			if (utf32Decoder != null)
			{
				utf32Decoder.iChar = (int)num2;
				utf32Decoder.readByteCount = num;
				utf32Decoder.m_bytesUsed = (int)((long)(bytes - ptr3));
			}
			return (int)((long)(chars - ptr));
		}

		// Token: 0x06002B27 RID: 11047 RVA: 0x0008F5A2 File Offset: 0x0008E5A2
		private uint GetSurrogate(char cHigh, char cLow)
		{
			return (uint)((cHigh - '\ud800') * 'Ѐ' + (cLow - '\udc00')) + 65536U;
		}

		// Token: 0x06002B28 RID: 11048 RVA: 0x0008F5BF File Offset: 0x0008E5BF
		private char GetHighSurrogate(uint iChar)
		{
			return (char)((iChar - 65536U) / 1024U + 55296U);
		}

		// Token: 0x06002B29 RID: 11049 RVA: 0x0008F5D5 File Offset: 0x0008E5D5
		private char GetLowSurrogate(uint iChar)
		{
			return (char)((iChar - 65536U) % 1024U + 56320U);
		}

		// Token: 0x06002B2A RID: 11050 RVA: 0x0008F5EB File Offset: 0x0008E5EB
		public override Decoder GetDecoder()
		{
			return new UTF32Encoding.UTF32Decoder(this);
		}

		// Token: 0x06002B2B RID: 11051 RVA: 0x0008F5F3 File Offset: 0x0008E5F3
		public override Encoder GetEncoder()
		{
			return new EncoderNLS(this);
		}

		// Token: 0x06002B2C RID: 11052 RVA: 0x0008F5FC File Offset: 0x0008E5FC
		public override int GetMaxByteCount(int charCount)
		{
			if (charCount < 0)
			{
				throw new ArgumentOutOfRangeException("charCount", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			long num = (long)charCount + 1L;
			if (base.EncoderFallback.MaxCharCount > 1)
			{
				num *= (long)base.EncoderFallback.MaxCharCount;
			}
			num *= 4L;
			if (num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("charCount", Environment.GetResourceString("ArgumentOutOfRange_GetByteCountOverflow"));
			}
			return (int)num;
		}

		// Token: 0x06002B2D RID: 11053 RVA: 0x0008F66C File Offset: 0x0008E66C
		public override int GetMaxCharCount(int byteCount)
		{
			if (byteCount < 0)
			{
				throw new ArgumentOutOfRangeException("byteCount", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			int num = byteCount / 2 + 2;
			if (base.DecoderFallback.MaxCharCount > 2)
			{
				num *= base.DecoderFallback.MaxCharCount;
				num /= 2;
			}
			if (num > 2147483647)
			{
				throw new ArgumentOutOfRangeException("byteCount", Environment.GetResourceString("ArgumentOutOfRange_GetCharCountOverflow"));
			}
			return num;
		}

		// Token: 0x06002B2E RID: 11054 RVA: 0x0008F6D8 File Offset: 0x0008E6D8
		public override byte[] GetPreamble()
		{
			if (!this.emitUTF32ByteOrderMark)
			{
				return Encoding.emptyByteArray;
			}
			if (this.bigEndian)
			{
				return new byte[] { 0, 0, 254, byte.MaxValue };
			}
			byte[] array = new byte[4];
			array[0] = byte.MaxValue;
			array[1] = 254;
			return array;
		}

		// Token: 0x06002B2F RID: 11055 RVA: 0x0008F72C File Offset: 0x0008E72C
		public override bool Equals(object value)
		{
			UTF32Encoding utf32Encoding = value as UTF32Encoding;
			return utf32Encoding != null && (this.emitUTF32ByteOrderMark == utf32Encoding.emitUTF32ByteOrderMark && this.bigEndian == utf32Encoding.bigEndian && base.EncoderFallback.Equals(utf32Encoding.EncoderFallback)) && base.DecoderFallback.Equals(utf32Encoding.DecoderFallback);
		}

		// Token: 0x06002B30 RID: 11056 RVA: 0x0008F787 File Offset: 0x0008E787
		public override int GetHashCode()
		{
			return base.EncoderFallback.GetHashCode() + base.DecoderFallback.GetHashCode() + this.CodePage + (this.emitUTF32ByteOrderMark ? 4 : 0) + (this.bigEndian ? 8 : 0);
		}

		// Token: 0x040014E0 RID: 5344
		private bool emitUTF32ByteOrderMark;

		// Token: 0x040014E1 RID: 5345
		private bool isThrowException;

		// Token: 0x040014E2 RID: 5346
		private bool bigEndian;

		// Token: 0x02000411 RID: 1041
		[Serializable]
		internal class UTF32Decoder : DecoderNLS
		{
			// Token: 0x06002B31 RID: 11057 RVA: 0x0008F7C1 File Offset: 0x0008E7C1
			public UTF32Decoder(UTF32Encoding encoding)
				: base(encoding)
			{
			}

			// Token: 0x06002B32 RID: 11058 RVA: 0x0008F7CA File Offset: 0x0008E7CA
			public override void Reset()
			{
				this.iChar = 0;
				this.readByteCount = 0;
				if (this.m_fallbackBuffer != null)
				{
					this.m_fallbackBuffer.Reset();
				}
			}

			// Token: 0x17000816 RID: 2070
			// (get) Token: 0x06002B33 RID: 11059 RVA: 0x0008F7ED File Offset: 0x0008E7ED
			internal override bool HasState
			{
				get
				{
					return this.readByteCount != 0;
				}
			}

			// Token: 0x040014E3 RID: 5347
			internal int iChar;

			// Token: 0x040014E4 RID: 5348
			internal int readByteCount;
		}
	}
}
