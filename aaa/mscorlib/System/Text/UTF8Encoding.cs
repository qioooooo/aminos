using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Text
{
	// Token: 0x0200040D RID: 1037
	[ComVisible(true)]
	[Serializable]
	public class UTF8Encoding : Encoding
	{
		// Token: 0x06002AEA RID: 10986 RVA: 0x0008C6B0 File Offset: 0x0008B6B0
		public UTF8Encoding()
			: this(false)
		{
		}

		// Token: 0x06002AEB RID: 10987 RVA: 0x0008C6B9 File Offset: 0x0008B6B9
		public UTF8Encoding(bool encoderShouldEmitUTF8Identifier)
			: this(encoderShouldEmitUTF8Identifier, false)
		{
		}

		// Token: 0x06002AEC RID: 10988 RVA: 0x0008C6C3 File Offset: 0x0008B6C3
		public UTF8Encoding(bool encoderShouldEmitUTF8Identifier, bool throwOnInvalidBytes)
			: base(65001)
		{
			this.emitUTF8Identifier = encoderShouldEmitUTF8Identifier;
			this.isThrowException = throwOnInvalidBytes;
			if (this.isThrowException)
			{
				this.SetDefaultFallbacks();
			}
		}

		// Token: 0x06002AED RID: 10989 RVA: 0x0008C6EC File Offset: 0x0008B6EC
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

		// Token: 0x06002AEE RID: 10990 RVA: 0x0008C738 File Offset: 0x0008B738
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

		// Token: 0x06002AEF RID: 10991 RVA: 0x0008C7D4 File Offset: 0x0008B7D4
		public unsafe override int GetByteCount(string chars)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("s");
			}
			IntPtr intPtr2;
			IntPtr intPtr = (intPtr2 = chars);
			if (intPtr != 0)
			{
				intPtr2 = (IntPtr)((int)intPtr + RuntimeHelpers.OffsetToStringData);
			}
			char* ptr = intPtr2;
			return this.GetByteCount(ptr, chars.Length, null);
		}

		// Token: 0x06002AF0 RID: 10992 RVA: 0x0008C80F File Offset: 0x0008B80F
		[CLSCompliant(false)]
		[ComVisible(false)]
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

		// Token: 0x06002AF1 RID: 10993 RVA: 0x0008C850 File Offset: 0x0008B850
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

		// Token: 0x06002AF2 RID: 10994 RVA: 0x0008C948 File Offset: 0x0008B948
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

		// Token: 0x06002AF3 RID: 10995 RVA: 0x0008CA50 File Offset: 0x0008BA50
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

		// Token: 0x06002AF4 RID: 10996 RVA: 0x0008CAC0 File Offset: 0x0008BAC0
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

		// Token: 0x06002AF5 RID: 10997 RVA: 0x0008CB58 File Offset: 0x0008BB58
		[CLSCompliant(false)]
		[ComVisible(false)]
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

		// Token: 0x06002AF6 RID: 10998 RVA: 0x0008CB98 File Offset: 0x0008BB98
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

		// Token: 0x06002AF7 RID: 10999 RVA: 0x0008CCA0 File Offset: 0x0008BCA0
		[CLSCompliant(false)]
		[ComVisible(false)]
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

		// Token: 0x06002AF8 RID: 11000 RVA: 0x0008CD10 File Offset: 0x0008BD10
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

		// Token: 0x06002AF9 RID: 11001 RVA: 0x0008CDAC File Offset: 0x0008BDAC
		internal unsafe override int GetByteCount(char* chars, int count, EncoderNLS baseEncoder)
		{
			EncoderFallbackBuffer encoderFallbackBuffer = null;
			char* ptr = chars;
			char* ptr2 = ptr + count;
			int num = count;
			int num2 = 0;
			if (baseEncoder != null)
			{
				UTF8Encoding.UTF8Encoder utf8Encoder = (UTF8Encoding.UTF8Encoder)baseEncoder;
				num2 = utf8Encoder.surrogateChar;
				if (utf8Encoder.InternalHasFallbackBuffer)
				{
					if ((encoderFallbackBuffer = utf8Encoder.FallbackBuffer).Remaining > 0)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_EncoderFallbackNotEmpty", new object[]
						{
							this.EncodingName,
							utf8Encoder.Fallback.GetType()
						}));
					}
					encoderFallbackBuffer.InternalInitialize(chars, ptr2, utf8Encoder, false);
				}
			}
			for (;;)
			{
				if (ptr >= ptr2)
				{
					if (num2 == 0)
					{
						num2 = (int)((encoderFallbackBuffer != null) ? encoderFallbackBuffer.InternalGetNextChar() : '\0');
						if (num2 > 0)
						{
							num++;
							goto IL_0155;
						}
					}
					else if (encoderFallbackBuffer != null && encoderFallbackBuffer.bFallingBack)
					{
						num2 = (int)encoderFallbackBuffer.InternalGetNextChar();
						num++;
						if (UTF8Encoding.InRange(num2, 56320, 57343))
						{
							num2 = 65533;
							num++;
							goto IL_0171;
						}
						if (num2 <= 0)
						{
							break;
						}
						goto IL_0155;
					}
					if (num2 > 0 && (baseEncoder == null || baseEncoder.MustFlush))
					{
						num++;
						goto IL_0171;
					}
					return num;
				}
				else if (num2 > 0)
				{
					int num3 = (int)(*ptr);
					num++;
					if (UTF8Encoding.InRange(num3, 56320, 57343))
					{
						num2 = 65533;
						ptr++;
						goto IL_0171;
					}
					goto IL_0171;
				}
				else
				{
					if (encoderFallbackBuffer != null)
					{
						num2 = (int)encoderFallbackBuffer.InternalGetNextChar();
						if (num2 > 0)
						{
							num++;
							goto IL_0155;
						}
					}
					num2 = (int)(*ptr);
					ptr++;
				}
				IL_0155:
				if (UTF8Encoding.InRange(num2, 55296, 56319))
				{
					num--;
					continue;
				}
				IL_0171:
				if (UTF8Encoding.InRange(num2, 55296, 57343))
				{
					if (encoderFallbackBuffer == null)
					{
						if (baseEncoder == null)
						{
							encoderFallbackBuffer = this.encoderFallback.CreateFallbackBuffer();
						}
						else
						{
							encoderFallbackBuffer = baseEncoder.FallbackBuffer;
						}
						encoderFallbackBuffer.InternalInitialize(chars, chars + count, baseEncoder, false);
					}
					encoderFallbackBuffer.InternalFallback((char)num2, ref ptr);
					num--;
					num2 = 0;
				}
				else
				{
					if (num2 > 127)
					{
						if (num2 > 2047)
						{
							num++;
						}
						num++;
					}
					if (encoderFallbackBuffer != null && (num2 = (int)encoderFallbackBuffer.InternalGetNextChar()) != 0)
					{
						num++;
						goto IL_0155;
					}
					int num4 = UTF8Encoding.PtrDiff(ptr2, ptr);
					if (num4 <= 13)
					{
						char* ptr3 = ptr2;
						while (ptr < ptr3)
						{
							num2 = (int)(*ptr);
							ptr++;
							if (num2 > 127)
							{
								goto IL_0155;
							}
						}
						return num;
					}
					char* ptr4 = ptr + num4 - 7;
					IL_03DC:
					while (ptr < ptr4)
					{
						num2 = (int)(*ptr);
						ptr++;
						if (num2 > 127)
						{
							if (num2 > 2047)
							{
								if ((num2 & 63488) == 55296)
								{
									goto IL_038C;
								}
								num++;
							}
							num++;
						}
						if ((ptr & 2) != 0)
						{
							num2 = (int)(*ptr);
							ptr++;
							if (num2 > 127)
							{
								if (num2 > 2047)
								{
									if ((num2 & 63488) == 55296)
									{
										goto IL_038C;
									}
									num++;
								}
								num++;
							}
						}
						while (ptr < ptr4)
						{
							num2 = *(int*)ptr;
							int num5 = *(int*)(ptr + 2);
							if (((num2 | num5) & -8323200) != 0)
							{
								if (((num2 | num5) & -134154240) != 0)
								{
									goto IL_037C;
								}
								if ((num2 & -8388608) != 0)
								{
									num++;
								}
								if ((num2 & 65408) != 0)
								{
									num++;
								}
								if ((num5 & -8388608) != 0)
								{
									num++;
								}
								if ((num5 & 65408) != 0)
								{
									num++;
								}
							}
							ptr += 4;
							num2 = *(int*)ptr;
							num5 = *(int*)(ptr + 2);
							if (((num2 | num5) & -8323200) != 0)
							{
								if (((num2 | num5) & -134154240) != 0)
								{
									goto IL_037C;
								}
								if ((num2 & -8388608) != 0)
								{
									num++;
								}
								if ((num2 & 65408) != 0)
								{
									num++;
								}
								if ((num5 & -8388608) != 0)
								{
									num++;
								}
								if ((num5 & 65408) != 0)
								{
									num++;
								}
							}
							ptr += 4;
							continue;
							IL_037C:
							num2 = (int)((ushort)num2);
							ptr++;
							if (num2 > 127)
							{
								goto IL_038C;
							}
							goto IL_03DC;
						}
						break;
						IL_038C:
						if (num2 > 2047)
						{
							if (UTF8Encoding.InRange(num2, 55296, 57343))
							{
								int num6 = (int)(*ptr);
								if (num2 > 56319 || !UTF8Encoding.InRange(num6, 56320, 57343))
								{
									ptr--;
									break;
								}
								ptr++;
							}
							num++;
						}
						num++;
					}
					num2 = 0;
				}
			}
			num--;
			return num;
		}

		// Token: 0x06002AFA RID: 11002 RVA: 0x0008D1A6 File Offset: 0x0008C1A6
		private unsafe static int PtrDiff(char* a, char* b)
		{
			return (int)((uint)((long)((a - b) / 1 * 2)) >> 1);
		}

		// Token: 0x06002AFB RID: 11003 RVA: 0x0008D1B1 File Offset: 0x0008C1B1
		private unsafe static int PtrDiff(byte* a, byte* b)
		{
			return (int)((long)(a - b));
		}

		// Token: 0x06002AFC RID: 11004 RVA: 0x0008D1BA File Offset: 0x0008C1BA
		private static bool InRange(int ch, int start, int end)
		{
			return ch - start <= end - start;
		}

		// Token: 0x06002AFD RID: 11005 RVA: 0x0008D1C8 File Offset: 0x0008C1C8
		internal unsafe override int GetBytes(char* chars, int charCount, byte* bytes, int byteCount, EncoderNLS baseEncoder)
		{
			UTF8Encoding.UTF8Encoder utf8Encoder = null;
			EncoderFallbackBuffer encoderFallbackBuffer = null;
			char* ptr = chars;
			byte* ptr2 = bytes;
			char* ptr3 = ptr + charCount;
			byte* ptr4 = ptr2 + byteCount;
			int num = 0;
			if (baseEncoder != null)
			{
				utf8Encoder = (UTF8Encoding.UTF8Encoder)baseEncoder;
				num = utf8Encoder.surrogateChar;
				if (utf8Encoder.InternalHasFallbackBuffer)
				{
					encoderFallbackBuffer = utf8Encoder.FallbackBuffer;
					if (encoderFallbackBuffer.Remaining > 0 && utf8Encoder.m_throwOnOverflow)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_EncoderFallbackNotEmpty", new object[]
						{
							this.EncodingName,
							utf8Encoder.Fallback.GetType()
						}));
					}
					encoderFallbackBuffer.InternalInitialize(chars, ptr3, utf8Encoder, true);
				}
			}
			for (;;)
			{
				if (ptr >= ptr3)
				{
					if (num == 0)
					{
						num = (int)((encoderFallbackBuffer != null) ? encoderFallbackBuffer.InternalGetNextChar() : '\0');
						if (num > 0)
						{
							goto IL_0159;
						}
					}
					else if (encoderFallbackBuffer != null && encoderFallbackBuffer.bFallingBack)
					{
						int num2 = num;
						num = (int)encoderFallbackBuffer.InternalGetNextChar();
						if (UTF8Encoding.InRange(num, 56320, 57343))
						{
							num = num + (num2 << 10) + -56613888;
							goto IL_016F;
						}
						if (num > 0)
						{
							goto IL_0159;
						}
						goto IL_04D9;
					}
					if (num <= 0)
					{
						goto IL_04D9;
					}
					if (utf8Encoder == null)
					{
						goto IL_016F;
					}
					if (utf8Encoder.MustFlush)
					{
						goto IL_016F;
					}
					goto IL_04D9;
				}
				else if (num > 0)
				{
					int num3 = (int)(*ptr);
					if (UTF8Encoding.InRange(num3, 56320, 57343))
					{
						num = num3 + (num << 10) + -56613888;
						ptr++;
						goto IL_016F;
					}
					goto IL_016F;
				}
				else
				{
					if (encoderFallbackBuffer != null)
					{
						num = (int)encoderFallbackBuffer.InternalGetNextChar();
						if (num > 0)
						{
							goto IL_0159;
						}
					}
					num = (int)(*ptr);
					ptr++;
				}
				IL_0159:
				if (UTF8Encoding.InRange(num, 55296, 56319))
				{
					continue;
				}
				IL_016F:
				if (UTF8Encoding.InRange(num, 55296, 57343))
				{
					if (encoderFallbackBuffer == null)
					{
						if (baseEncoder == null)
						{
							encoderFallbackBuffer = this.encoderFallback.CreateFallbackBuffer();
						}
						else
						{
							encoderFallbackBuffer = baseEncoder.FallbackBuffer;
						}
						encoderFallbackBuffer.InternalInitialize(chars, ptr3, baseEncoder, true);
					}
					encoderFallbackBuffer.InternalFallback((char)num, ref ptr);
					num = 0;
				}
				else
				{
					int num4 = 1;
					if (num > 127)
					{
						if (num > 2047)
						{
							if (num > 65535)
							{
								num4++;
							}
							num4++;
						}
						num4++;
					}
					if (ptr2 != ptr4 - num4)
					{
						break;
					}
					if (num <= 127)
					{
						*ptr2 = (byte)num;
					}
					else
					{
						int num5;
						if (num <= 2047)
						{
							num5 = (int)((byte)(-64 | (num >> 6)));
						}
						else
						{
							if (num <= 65535)
							{
								num5 = (int)((byte)(-32 | (num >> 12)));
							}
							else
							{
								*ptr2 = (byte)(-16 | (num >> 18));
								ptr2++;
								num5 = -128 | ((num >> 12) & 63);
							}
							*ptr2 = (byte)num5;
							ptr2++;
							num5 = -128 | ((num >> 6) & 63);
						}
						*ptr2 = (byte)num5;
						ptr2++;
						*ptr2 = (byte)(-128 | (num & 63));
					}
					ptr2++;
					if (encoderFallbackBuffer != null && (num = (int)encoderFallbackBuffer.InternalGetNextChar()) != 0)
					{
						goto IL_0159;
					}
					int num6 = UTF8Encoding.PtrDiff(ptr3, ptr);
					int num7 = UTF8Encoding.PtrDiff(ptr4, ptr2);
					if (num6 <= 13)
					{
						if (num7 >= num6)
						{
							char* ptr5 = ptr3;
							while (ptr < ptr5)
							{
								num = (int)(*ptr);
								ptr++;
								if (num > 127)
								{
									goto IL_0159;
								}
								*ptr2 = (byte)num;
								ptr2++;
							}
							goto Block_37;
						}
						num = 0;
					}
					else
					{
						if (num7 < num6)
						{
							num6 = num7;
						}
						char* ptr6 = ptr + num6 - 5;
						while (ptr < ptr6)
						{
							num = (int)(*ptr);
							ptr++;
							if (num <= 127)
							{
								*ptr2 = (byte)num;
								ptr2++;
								if ((ptr & 2) != 0)
								{
									num = (int)(*ptr);
									ptr++;
									if (num > 127)
									{
										goto IL_03F2;
									}
									*ptr2 = (byte)num;
									ptr2++;
								}
								while (ptr < ptr6)
								{
									num = *(int*)ptr;
									int num8 = *(int*)(ptr + 2);
									if (((num | num8) & -8323200) == 0)
									{
										*ptr2 = (byte)num;
										ptr2[1] = (byte)(num >> 16);
										ptr += 4;
										ptr2[2] = (byte)num8;
										ptr2[3] = (byte)(num8 >> 16);
										ptr2 += 4;
									}
									else
									{
										num = (int)((ushort)num);
										ptr++;
										if (num <= 127)
										{
											*ptr2 = (byte)num;
											ptr2++;
											break;
										}
										goto IL_03F2;
									}
								}
								continue;
							}
							IL_03F2:
							int num9;
							if (num <= 2047)
							{
								num9 = -64 | (num >> 6);
							}
							else
							{
								if (!UTF8Encoding.InRange(num, 55296, 57343))
								{
									num9 = -32 | (num >> 12);
								}
								else
								{
									if (num > 56319)
									{
										ptr--;
										break;
									}
									num9 = (int)(*ptr);
									ptr++;
									if (!UTF8Encoding.InRange(num9, 56320, 57343))
									{
										ptr -= 2;
										break;
									}
									num = num9 + (num << 10) + -56613888;
									*ptr2 = (byte)(-16 | (num >> 18));
									ptr2++;
									num9 = -128 | ((num >> 12) & 63);
								}
								*ptr2 = (byte)num9;
								ptr6--;
								ptr2++;
								num9 = -128 | ((num >> 6) & 63);
							}
							*ptr2 = (byte)num9;
							ptr6--;
							ptr2++;
							*ptr2 = (byte)(-128 | (num & 63));
							ptr2++;
						}
						num = 0;
					}
				}
			}
			if (encoderFallbackBuffer != null && encoderFallbackBuffer.bFallingBack)
			{
				encoderFallbackBuffer.MovePrevious();
				if (num > 65535)
				{
					encoderFallbackBuffer.MovePrevious();
				}
			}
			else
			{
				ptr--;
				if (num > 65535)
				{
					ptr--;
				}
			}
			base.ThrowBytesOverflow(utf8Encoder, ptr2 == bytes);
			num = 0;
			goto IL_04D9;
			Block_37:
			num = 0;
			IL_04D9:
			if (utf8Encoder != null)
			{
				utf8Encoder.surrogateChar = num;
				utf8Encoder.m_charsUsed = (int)((long)(ptr - chars));
			}
			return (int)((long)(ptr2 - bytes));
		}

		// Token: 0x06002AFE RID: 11006 RVA: 0x0008D6D0 File Offset: 0x0008C6D0
		internal unsafe override int GetCharCount(byte* bytes, int count, DecoderNLS baseDecoder)
		{
			byte* ptr = bytes;
			byte* ptr2 = ptr + count;
			int num = count;
			int num2 = 0;
			DecoderFallbackBuffer decoderFallbackBuffer = null;
			if (baseDecoder != null)
			{
				UTF8Encoding.UTF8Decoder utf8Decoder = (UTF8Encoding.UTF8Decoder)baseDecoder;
				num2 = utf8Decoder.bits;
				num -= num2 >> 30;
			}
			IL_0027:
			while (ptr < ptr2)
			{
				if (num2 == 0)
				{
					num2 = (int)(*ptr);
					ptr++;
					goto IL_010D;
				}
				int num3 = (int)(*ptr);
				ptr++;
				if ((num3 & -64) != 128)
				{
					ptr--;
					num += num2 >> 30;
				}
				else
				{
					num2 = (num2 << 6) | (num3 & 63);
					if ((num2 & 536870912) == 0)
					{
						if ((num2 & 268435456) != 0)
						{
							if ((num2 & 8388608) != 0 || UTF8Encoding.InRange(num2 & 496, 16, 256))
							{
								continue;
							}
						}
						else if ((num2 & 992) != 0)
						{
							if ((num2 & 992) != 864)
							{
								continue;
							}
						}
					}
					else
					{
						if ((num2 & 270467072) == 268435456)
						{
							num--;
							goto IL_0183;
						}
						goto IL_0183;
					}
				}
				IL_00C9:
				if (decoderFallbackBuffer == null)
				{
					if (baseDecoder == null)
					{
						decoderFallbackBuffer = this.decoderFallback.CreateFallbackBuffer();
					}
					else
					{
						decoderFallbackBuffer = baseDecoder.FallbackBuffer;
					}
					decoderFallbackBuffer.InternalInitialize(bytes, null);
				}
				num += this.FallbackInvalidByteSequence(ptr, num2, decoderFallbackBuffer);
				num2 = 0;
				continue;
				IL_0183:
				int num4 = UTF8Encoding.PtrDiff(ptr2, ptr);
				if (num4 <= 13)
				{
					byte* ptr3 = ptr2;
					while (ptr < ptr3)
					{
						num2 = (int)(*ptr);
						ptr++;
						if (num2 > 127)
						{
							goto IL_010D;
						}
					}
					num2 = 0;
					break;
				}
				byte* ptr4 = ptr + num4 - 7;
				while (ptr < ptr4)
				{
					num2 = (int)(*ptr);
					ptr++;
					if (num2 <= 127)
					{
						if ((ptr & 1) != 0)
						{
							num2 = (int)(*ptr);
							ptr++;
							if (num2 > 127)
							{
								goto IL_025A;
							}
						}
						if ((ptr & 2) != 0)
						{
							num2 = (int)(*(ushort*)ptr);
							if ((num2 & 32896) != 0)
							{
								goto IL_0245;
							}
							ptr += 2;
						}
						while (ptr < ptr4)
						{
							num2 = *(int*)ptr;
							int num5 = *(int*)(ptr + 4);
							if (((num2 | num5) & -2139062144) != 0)
							{
								goto IL_0245;
							}
							ptr += 8;
							if (ptr >= ptr4)
							{
								break;
							}
							num2 = *(int*)ptr;
							num5 = *(int*)(ptr + 4);
							if (((num2 | num5) & -2139062144) != 0)
							{
								goto IL_0245;
							}
							ptr += 8;
						}
						break;
						IL_0245:
						num2 &= 255;
						ptr++;
						if (num2 <= 127)
						{
							continue;
						}
					}
					IL_025A:
					int num6 = (int)(*ptr);
					ptr++;
					if ((num2 & 64) != 0 && (num6 & -64) == 128)
					{
						num6 &= 63;
						if ((num2 & 32) != 0)
						{
							num6 |= (num2 & 15) << 6;
							if ((num2 & 16) != 0)
							{
								num2 = (int)(*ptr);
								if (!UTF8Encoding.InRange(num6 >> 4, 1, 16) || (num2 & -64) != 128)
								{
									goto IL_032A;
								}
								num6 = (num6 << 6) | (num2 & 63);
								num2 = (int)ptr[1];
								if ((num2 & -64) != 128)
								{
									goto IL_032A;
								}
								ptr += 2;
								num--;
							}
							else
							{
								num2 = (int)(*ptr);
								if ((num6 & 992) == 0 || (num6 & 992) == 864 || (num2 & -64) != 128)
								{
									goto IL_032A;
								}
								ptr++;
								num--;
							}
						}
						else if ((num2 & 30) == 0)
						{
							goto IL_032A;
						}
						num--;
						continue;
					}
					IL_032A:
					ptr -= 2;
					num2 = 0;
					goto IL_0027;
				}
				num2 = 0;
				continue;
				IL_010D:
				if (num2 <= 127)
				{
					goto IL_0183;
				}
				num--;
				if ((num2 & 64) == 0)
				{
					goto IL_00C9;
				}
				if ((num2 & 32) != 0)
				{
					if ((num2 & 16) != 0)
					{
						num2 &= 15;
						if (num2 > 4)
						{
							num2 |= 240;
							goto IL_00C9;
						}
						num2 |= 1347226624;
						num--;
					}
					else
					{
						num2 = (num2 & 15) | 1210220544;
						num--;
					}
				}
				else
				{
					num2 &= 31;
					if (num2 <= 1)
					{
						num2 |= 192;
						goto IL_00C9;
					}
					num2 |= 8388608;
				}
			}
			if (num2 != 0)
			{
				num += num2 >> 30;
				if (baseDecoder == null || baseDecoder.MustFlush)
				{
					if (decoderFallbackBuffer == null)
					{
						if (baseDecoder == null)
						{
							decoderFallbackBuffer = this.decoderFallback.CreateFallbackBuffer();
						}
						else
						{
							decoderFallbackBuffer = baseDecoder.FallbackBuffer;
						}
						decoderFallbackBuffer.InternalInitialize(bytes, null);
					}
					num += this.FallbackInvalidByteSequence(ptr, num2, decoderFallbackBuffer);
				}
			}
			return num;
		}

		// Token: 0x06002AFF RID: 11007 RVA: 0x0008DA60 File Offset: 0x0008CA60
		internal unsafe override int GetChars(byte* bytes, int byteCount, char* chars, int charCount, DecoderNLS baseDecoder)
		{
			byte* ptr = bytes;
			char* ptr2 = chars;
			byte* ptr3 = ptr + byteCount;
			char* ptr4 = ptr2 + charCount;
			int num = 0;
			DecoderFallbackBuffer decoderFallbackBuffer = null;
			if (baseDecoder != null)
			{
				UTF8Encoding.UTF8Decoder utf8Decoder = (UTF8Encoding.UTF8Decoder)baseDecoder;
				num = utf8Decoder.bits;
			}
			IL_002C:
			while (ptr < ptr3)
			{
				if (num == 0)
				{
					num = (int)(*ptr);
					ptr++;
					goto IL_0165;
				}
				int num2 = (int)(*ptr);
				ptr++;
				if ((num2 & -64) != 128)
				{
					ptr--;
				}
				else
				{
					num = (num << 6) | (num2 & 63);
					if ((num & 536870912) == 0)
					{
						if ((num & 268435456) != 0)
						{
							if ((num & 8388608) != 0 || UTF8Encoding.InRange(num & 496, 16, 256))
							{
								continue;
							}
						}
						else if ((num & 992) != 0)
						{
							if ((num & 992) != 864)
							{
								continue;
							}
						}
					}
					else
					{
						if ((num & 270467072) > 268435456 && ptr2 < ptr4)
						{
							*ptr2 = (char)(((num >> 10) & 2047) + -10304);
							ptr2++;
							num = (num & 1023) + 56320;
							goto IL_01E6;
						}
						goto IL_01E6;
					}
				}
				IL_0100:
				if (decoderFallbackBuffer == null)
				{
					if (baseDecoder == null)
					{
						decoderFallbackBuffer = this.decoderFallback.CreateFallbackBuffer();
					}
					else
					{
						decoderFallbackBuffer = baseDecoder.FallbackBuffer;
					}
					decoderFallbackBuffer.InternalInitialize(bytes, ptr4);
				}
				if (!this.FallbackInvalidByteSequence(ref ptr, num, decoderFallbackBuffer, ref ptr2))
				{
					decoderFallbackBuffer.InternalReset();
					base.ThrowCharsOverflow(baseDecoder, ptr2 == chars);
					num = 0;
					break;
				}
				num = 0;
				continue;
				IL_01E6:
				if (ptr2 >= ptr4)
				{
					num &= 2097151;
					if (num > 127)
					{
						if (num > 2047)
						{
							if (num >= 56320 && num <= 57343)
							{
								ptr--;
								ptr2--;
							}
							else if (num > 65535)
							{
								ptr--;
							}
							ptr--;
						}
						ptr--;
					}
					ptr--;
					base.ThrowCharsOverflow(baseDecoder, ptr2 == chars);
					num = 0;
					break;
				}
				*ptr2 = (char)num;
				ptr2++;
				int num3 = UTF8Encoding.PtrDiff(ptr4, ptr2);
				int num4 = UTF8Encoding.PtrDiff(ptr3, ptr);
				if (num4 > 13)
				{
					if (num3 < num4)
					{
						num4 = num3;
					}
					char* ptr5 = ptr2 + num4 - 7;
					while (ptr2 < ptr5)
					{
						num = (int)(*ptr);
						ptr++;
						if (num <= 127)
						{
							*ptr2 = (char)num;
							ptr2++;
							if ((ptr & 1) != 0)
							{
								num = (int)(*ptr);
								ptr++;
								if (num > 127)
								{
									goto IL_0407;
								}
								*ptr2 = (char)num;
								ptr2++;
							}
							if ((ptr & 2) != 0)
							{
								num = (int)(*(ushort*)ptr);
								if ((num & 32896) != 0)
								{
									goto IL_03E3;
								}
								*ptr2 = (char)(num & 127);
								ptr += 2;
								ptr2[1] = (char)((num >> 8) & 127);
								ptr2 += 2;
							}
							while (ptr2 < ptr5)
							{
								num = *(int*)ptr;
								int num5 = *(int*)(ptr + 4);
								if (((num | num5) & -2139062144) != 0)
								{
									goto IL_03E3;
								}
								*ptr2 = (char)(num & 127);
								ptr2[1] = (char)((num >> 8) & 127);
								ptr2[2] = (char)((num >> 16) & 127);
								ptr2[3] = (char)((num >> 24) & 127);
								ptr += 8;
								ptr2[4] = (char)(num5 & 127);
								ptr2[5] = (char)((num5 >> 8) & 127);
								ptr2[6] = (char)((num5 >> 16) & 127);
								ptr2[7] = (char)((num5 >> 24) & 127);
								ptr2 += 8;
							}
							break;
							IL_03E3:
							num &= 255;
							ptr++;
							if (num <= 127)
							{
								*ptr2 = (char)num;
								ptr2++;
								continue;
							}
						}
						IL_0407:
						int num6 = (int)(*ptr);
						ptr++;
						if ((num & 64) != 0 && (num6 & -64) == 128)
						{
							num6 &= 63;
							if ((num & 32) != 0)
							{
								num6 |= (num & 15) << 6;
								if ((num & 16) != 0)
								{
									num = (int)(*ptr);
									if (!UTF8Encoding.InRange(num6 >> 4, 1, 16) || (num & -64) != 128)
									{
										goto IL_0552;
									}
									num6 = (num6 << 6) | (num & 63);
									num = (int)ptr[1];
									if ((num & -64) != 128)
									{
										goto IL_0552;
									}
									ptr += 2;
									num = (num6 << 6) | (num & 63);
									*ptr2 = (char)(((num >> 10) & 2047) + -10304);
									ptr2++;
									num = (num & 1023) + -9216;
									ptr5--;
								}
								else
								{
									num = (int)(*ptr);
									if ((num6 & 992) == 0 || (num6 & 992) == 864 || (num & -64) != 128)
									{
										goto IL_0552;
									}
									ptr++;
									num = (num6 << 6) | (num & 63);
									ptr5--;
								}
							}
							else
							{
								num &= 31;
								if (num <= 1)
								{
									goto IL_0552;
								}
								num = (num << 6) | num6;
							}
							*ptr2 = (char)num;
							ptr2++;
							ptr5--;
							continue;
						}
						IL_0552:
						ptr -= 2;
						num = 0;
						goto IL_002C;
					}
					num = 0;
					continue;
				}
				if (num3 < num4)
				{
					num = 0;
					continue;
				}
				byte* ptr6 = ptr3;
				while (ptr < ptr6)
				{
					num = (int)(*ptr);
					ptr++;
					if (num > 127)
					{
						goto IL_0165;
					}
					*ptr2 = (char)num;
					ptr2++;
				}
				num = 0;
				break;
				IL_0165:
				if (num <= 127)
				{
					goto IL_01E6;
				}
				if ((num & 64) == 0)
				{
					goto IL_0100;
				}
				if ((num & 32) != 0)
				{
					if ((num & 16) != 0)
					{
						num &= 15;
						if (num > 4)
						{
							num |= 240;
							goto IL_0100;
						}
						num |= 1347226624;
					}
					else
					{
						num = (num & 15) | 1210220544;
					}
				}
				else
				{
					num &= 31;
					if (num <= 1)
					{
						num |= 192;
						goto IL_0100;
					}
					num |= 8388608;
				}
			}
			if (num != 0 && (baseDecoder == null || baseDecoder.MustFlush))
			{
				if (decoderFallbackBuffer == null)
				{
					if (baseDecoder == null)
					{
						decoderFallbackBuffer = this.decoderFallback.CreateFallbackBuffer();
					}
					else
					{
						decoderFallbackBuffer = baseDecoder.FallbackBuffer;
					}
					decoderFallbackBuffer.InternalInitialize(bytes, ptr4);
				}
				if (!this.FallbackInvalidByteSequence(ref ptr, num, decoderFallbackBuffer, ref ptr2))
				{
					decoderFallbackBuffer.InternalReset();
					base.ThrowCharsOverflow(baseDecoder, ptr2 == chars);
				}
				num = 0;
			}
			if (baseDecoder != null)
			{
				UTF8Encoding.UTF8Decoder utf8Decoder2 = (UTF8Encoding.UTF8Decoder)baseDecoder;
				utf8Decoder2.bits = num;
				baseDecoder.m_bytesUsed = (int)((long)(ptr - bytes));
			}
			return UTF8Encoding.PtrDiff(ptr2, chars);
		}

		// Token: 0x06002B00 RID: 11008 RVA: 0x0008E058 File Offset: 0x0008D058
		private unsafe bool FallbackInvalidByteSequence(ref byte* pSrc, int ch, DecoderFallbackBuffer fallback, ref char* pTarget)
		{
			byte* ptr = pSrc;
			byte[] bytesUnknown = this.GetBytesUnknown(ref ptr, ch);
			if (!fallback.InternalFallback(bytesUnknown, pSrc, ref pTarget))
			{
				pSrc = ptr;
				return false;
			}
			return true;
		}

		// Token: 0x06002B01 RID: 11009 RVA: 0x0008E088 File Offset: 0x0008D088
		private unsafe int FallbackInvalidByteSequence(byte* pSrc, int ch, DecoderFallbackBuffer fallback)
		{
			byte[] bytesUnknown = this.GetBytesUnknown(ref pSrc, ch);
			return fallback.InternalFallback(bytesUnknown, pSrc);
		}

		// Token: 0x06002B02 RID: 11010 RVA: 0x0008E0AC File Offset: 0x0008D0AC
		private unsafe byte[] GetBytesUnknown(ref byte* pSrc, int ch)
		{
			byte[] array;
			if (ch < 256 && ch >= 0)
			{
				pSrc -= (IntPtr)1;
				array = new byte[] { (byte)ch };
			}
			else if ((ch & 402653184) == 0)
			{
				pSrc -= (IntPtr)1;
				array = new byte[] { (byte)((ch & 31) | 192) };
			}
			else if ((ch & 268435456) != 0)
			{
				if ((ch & 8388608) != 0)
				{
					pSrc -= (IntPtr)3;
					array = new byte[]
					{
						(byte)(((ch >> 12) & 7) | 240),
						(byte)(((ch >> 6) & 63) | 128),
						(byte)((ch & 63) | 128)
					};
				}
				else if ((ch & 131072) != 0)
				{
					pSrc -= (IntPtr)2;
					array = new byte[]
					{
						(byte)(((ch >> 6) & 7) | 240),
						(byte)((ch & 63) | 128)
					};
				}
				else
				{
					pSrc -= (IntPtr)1;
					array = new byte[] { (byte)((ch & 7) | 240) };
				}
			}
			else if ((ch & 8388608) != 0)
			{
				pSrc -= (IntPtr)2;
				array = new byte[]
				{
					(byte)(((ch >> 6) & 15) | 224),
					(byte)((ch & 63) | 128)
				};
			}
			else
			{
				pSrc -= (IntPtr)1;
				array = new byte[] { (byte)((ch & 15) | 224) };
			}
			return array;
		}

		// Token: 0x06002B03 RID: 11011 RVA: 0x0008E225 File Offset: 0x0008D225
		public override Decoder GetDecoder()
		{
			return new UTF8Encoding.UTF8Decoder(this);
		}

		// Token: 0x06002B04 RID: 11012 RVA: 0x0008E22D File Offset: 0x0008D22D
		public override Encoder GetEncoder()
		{
			return new UTF8Encoding.UTF8Encoder(this);
		}

		// Token: 0x06002B05 RID: 11013 RVA: 0x0008E238 File Offset: 0x0008D238
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
			num *= 3L;
			if (num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("charCount", Environment.GetResourceString("ArgumentOutOfRange_GetByteCountOverflow"));
			}
			return (int)num;
		}

		// Token: 0x06002B06 RID: 11014 RVA: 0x0008E2A8 File Offset: 0x0008D2A8
		public override int GetMaxCharCount(int byteCount)
		{
			if (byteCount < 0)
			{
				throw new ArgumentOutOfRangeException("byteCount", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			long num = (long)byteCount + 1L;
			if (base.DecoderFallback.MaxCharCount > 1)
			{
				num *= (long)base.DecoderFallback.MaxCharCount;
			}
			if (num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("byteCount", Environment.GetResourceString("ArgumentOutOfRange_GetCharCountOverflow"));
			}
			return (int)num;
		}

		// Token: 0x06002B07 RID: 11015 RVA: 0x0008E31B File Offset: 0x0008D31B
		public override byte[] GetPreamble()
		{
			if (this.emitUTF8Identifier)
			{
				return new byte[] { 239, 187, 191 };
			}
			return Encoding.emptyByteArray;
		}

		// Token: 0x06002B08 RID: 11016 RVA: 0x0008E33C File Offset: 0x0008D33C
		public override bool Equals(object value)
		{
			UTF8Encoding utf8Encoding = value as UTF8Encoding;
			return utf8Encoding != null && (this.emitUTF8Identifier == utf8Encoding.emitUTF8Identifier && base.EncoderFallback.Equals(utf8Encoding.EncoderFallback)) && base.DecoderFallback.Equals(utf8Encoding.DecoderFallback);
		}

		// Token: 0x06002B09 RID: 11017 RVA: 0x0008E389 File Offset: 0x0008D389
		public override int GetHashCode()
		{
			return base.EncoderFallback.GetHashCode() + base.DecoderFallback.GetHashCode() + 65001 + (this.emitUTF8Identifier ? 1 : 0);
		}

		// Token: 0x040014D8 RID: 5336
		private const int UTF8_CODEPAGE = 65001;

		// Token: 0x040014D9 RID: 5337
		private const int FinalByte = 536870912;

		// Token: 0x040014DA RID: 5338
		private const int SupplimentarySeq = 268435456;

		// Token: 0x040014DB RID: 5339
		private const int ThreeByteSeq = 134217728;

		// Token: 0x040014DC RID: 5340
		private bool emitUTF8Identifier;

		// Token: 0x040014DD RID: 5341
		private bool isThrowException;

		// Token: 0x0200040E RID: 1038
		[Serializable]
		internal class UTF8Encoder : EncoderNLS, ISerializable
		{
			// Token: 0x06002B0A RID: 11018 RVA: 0x0008E3B5 File Offset: 0x0008D3B5
			public UTF8Encoder(UTF8Encoding encoding)
				: base(encoding)
			{
			}

			// Token: 0x06002B0B RID: 11019 RVA: 0x0008E3C0 File Offset: 0x0008D3C0
			internal UTF8Encoder(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				this.m_encoding = (Encoding)info.GetValue("encoding", typeof(Encoding));
				this.surrogateChar = (int)info.GetValue("surrogateChar", typeof(int));
				try
				{
					this.m_fallback = (EncoderFallback)info.GetValue("m_fallback", typeof(EncoderFallback));
				}
				catch (SerializationException)
				{
					this.m_fallback = null;
				}
			}

			// Token: 0x06002B0C RID: 11020 RVA: 0x0008E460 File Offset: 0x0008D460
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				info.AddValue("encoding", this.m_encoding);
				info.AddValue("surrogateChar", this.surrogateChar);
				info.AddValue("m_fallback", this.m_fallback);
				info.AddValue("storedSurrogate", this.surrogateChar > 0);
				info.AddValue("mustFlush", false);
			}

			// Token: 0x06002B0D RID: 11021 RVA: 0x0008E4D2 File Offset: 0x0008D4D2
			public override void Reset()
			{
				this.surrogateChar = 0;
				if (this.m_fallbackBuffer != null)
				{
					this.m_fallbackBuffer.Reset();
				}
			}

			// Token: 0x17000814 RID: 2068
			// (get) Token: 0x06002B0E RID: 11022 RVA: 0x0008E4EE File Offset: 0x0008D4EE
			internal override bool HasState
			{
				get
				{
					return this.surrogateChar != 0;
				}
			}

			// Token: 0x040014DE RID: 5342
			internal int surrogateChar;
		}

		// Token: 0x0200040F RID: 1039
		[Serializable]
		internal class UTF8Decoder : DecoderNLS, ISerializable
		{
			// Token: 0x06002B0F RID: 11023 RVA: 0x0008E4FC File Offset: 0x0008D4FC
			public UTF8Decoder(UTF8Encoding encoding)
				: base(encoding)
			{
			}

			// Token: 0x06002B10 RID: 11024 RVA: 0x0008E508 File Offset: 0x0008D508
			internal UTF8Decoder(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				this.m_encoding = (Encoding)info.GetValue("encoding", typeof(Encoding));
				try
				{
					this.bits = (int)info.GetValue("wbits", typeof(int));
					this.m_fallback = (DecoderFallback)info.GetValue("m_fallback", typeof(DecoderFallback));
				}
				catch (SerializationException)
				{
					this.bits = 0;
					this.m_fallback = null;
				}
			}

			// Token: 0x06002B11 RID: 11025 RVA: 0x0008E5AC File Offset: 0x0008D5AC
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
			void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				info.AddValue("encoding", this.m_encoding);
				info.AddValue("wbits", this.bits);
				info.AddValue("m_fallback", this.m_fallback);
				info.AddValue("bits", 0);
				info.AddValue("trailCount", 0);
				info.AddValue("isSurrogate", false);
				info.AddValue("byteSequence", 0);
			}

			// Token: 0x06002B12 RID: 11026 RVA: 0x0008E62A File Offset: 0x0008D62A
			public override void Reset()
			{
				this.bits = 0;
				if (this.m_fallbackBuffer != null)
				{
					this.m_fallbackBuffer.Reset();
				}
			}

			// Token: 0x17000815 RID: 2069
			// (get) Token: 0x06002B13 RID: 11027 RVA: 0x0008E646 File Offset: 0x0008D646
			internal override bool HasState
			{
				get
				{
					return this.bits != 0;
				}
			}

			// Token: 0x040014DF RID: 5343
			internal int bits;
		}
	}
}
