using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading;

namespace System.Text
{
	// Token: 0x02000404 RID: 1028
	[Serializable]
	internal class SBCSCodePageEncoding : BaseCodePageEncoding, ISerializable
	{
		// Token: 0x06002A88 RID: 10888 RVA: 0x00088D58 File Offset: 0x00087D58
		public SBCSCodePageEncoding(int codePage)
			: this(codePage, codePage)
		{
		}

		// Token: 0x06002A89 RID: 10889 RVA: 0x00088D62 File Offset: 0x00087D62
		internal SBCSCodePageEncoding(int codePage, int dataCodePage)
		{
			this.mapBytesToUnicode = null;
			this.mapUnicodeToBytes = null;
			this.mapCodePageCached = null;
			base..ctor(codePage, dataCodePage);
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x00088D84 File Offset: 0x00087D84
		internal SBCSCodePageEncoding(SerializationInfo info, StreamingContext context)
		{
			this.mapBytesToUnicode = null;
			this.mapUnicodeToBytes = null;
			this.mapCodePageCached = null;
			base..ctor(0);
			throw new ArgumentNullException("this");
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x00088DB0 File Offset: 0x00087DB0
		protected unsafe override void LoadManagedCodePage()
		{
			if (this.pCodePage->ByteCount != 1)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_NoCodepageData", new object[] { this.CodePage }));
			}
			this.byteUnknown = (byte)this.pCodePage->ByteReplace;
			this.charUnknown = this.pCodePage->UnicodeReplace;
			byte* sharedMemory = base.GetSharedMemory(66052 + this.iExtraBytes);
			this.mapBytesToUnicode = (char*)sharedMemory;
			this.mapUnicodeToBytes = sharedMemory + 512;
			this.mapCodePageCached = (int*)(sharedMemory + 512 + 65536 + this.iExtraBytes);
			if (*this.mapCodePageCached == 0)
			{
				char* ptr = (char*)(&this.pCodePage->FirstDataWord);
				for (int i = 0; i < 256; i++)
				{
					if (ptr[i] != '\0' || i == 0)
					{
						this.mapBytesToUnicode[i] = ptr[i];
						if (ptr[i] != '\ufffd')
						{
							this.mapUnicodeToBytes[ptr[i]] = (byte)i;
						}
					}
					else
					{
						this.mapBytesToUnicode[i] = '\ufffd';
					}
				}
				*this.mapCodePageCached = this.dataTableCodePage;
				return;
			}
			if (*this.mapCodePageCached != this.dataTableCodePage)
			{
				throw new OutOfMemoryException(Environment.GetResourceString("Arg_OutOfMemoryException"));
			}
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06002A8C RID: 10892 RVA: 0x00088EFC File Offset: 0x00087EFC
		private static object InternalSyncObject
		{
			get
			{
				if (SBCSCodePageEncoding.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref SBCSCodePageEncoding.s_InternalSyncObject, obj, null);
				}
				return SBCSCodePageEncoding.s_InternalSyncObject;
			}
		}

		// Token: 0x06002A8D RID: 10893 RVA: 0x00088F28 File Offset: 0x00087F28
		protected unsafe override void ReadBestFitTable()
		{
			lock (SBCSCodePageEncoding.InternalSyncObject)
			{
				if (this.arrayUnicodeBestFit == null)
				{
					byte* ptr = (byte*)(&this.pCodePage->FirstDataWord);
					ptr += 512;
					char[] array = new char[256];
					for (int i = 0; i < 256; i++)
					{
						array[i] = this.mapBytesToUnicode[i];
					}
					ushort num;
					while ((num = *(ushort*)ptr) != 0)
					{
						ptr += 2;
						array[(int)num] = (char)(*(ushort*)ptr);
						ptr += 2;
					}
					this.arrayByteBestFit = array;
					ptr += 2;
					byte* ptr2 = ptr;
					int num2 = 0;
					int j = (int)(*(ushort*)ptr);
					ptr += 2;
					while (j < 65536)
					{
						byte b = *ptr;
						ptr++;
						if (b == 1)
						{
							j = (int)(*(ushort*)ptr);
							ptr += 2;
						}
						else if (b < 32 && b > 0 && b != 30)
						{
							j += (int)b;
						}
						else
						{
							if (b > 0)
							{
								num2++;
							}
							j++;
						}
					}
					array = new char[num2 * 2];
					ptr = ptr2;
					j = (int)(*(ushort*)ptr);
					ptr += 2;
					num2 = 0;
					while (j < 65536)
					{
						byte b2 = *ptr;
						ptr++;
						if (b2 == 1)
						{
							j = (int)(*(ushort*)ptr);
							ptr += 2;
						}
						else if (b2 < 32 && b2 > 0 && b2 != 30)
						{
							j += (int)b2;
						}
						else
						{
							if (b2 == 30)
							{
								b2 = *ptr;
								ptr++;
							}
							if (b2 > 0)
							{
								array[num2++] = (char)j;
								array[num2++] = this.mapBytesToUnicode[b2];
							}
							j++;
						}
					}
					this.arrayUnicodeBestFit = array;
				}
			}
		}

		// Token: 0x06002A8E RID: 10894 RVA: 0x000890D0 File Offset: 0x000880D0
		internal unsafe override int GetByteCount(char* chars, int count, EncoderNLS encoder)
		{
			base.CheckMemorySection();
			char c = '\0';
			EncoderReplacementFallback encoderReplacementFallback;
			if (encoder != null)
			{
				c = encoder.charLeftOver;
				encoderReplacementFallback = encoder.Fallback as EncoderReplacementFallback;
			}
			else
			{
				encoderReplacementFallback = base.EncoderFallback as EncoderReplacementFallback;
			}
			if (encoderReplacementFallback != null && encoderReplacementFallback.MaxCharCount == 1)
			{
				if (c > '\0')
				{
					count++;
				}
				return count;
			}
			EncoderFallbackBuffer encoderFallbackBuffer = null;
			int num = 0;
			char* ptr = chars + count;
			if (c > '\0')
			{
				encoderFallbackBuffer = encoder.FallbackBuffer;
				encoderFallbackBuffer.InternalInitialize(chars, ptr, encoder, false);
				encoderFallbackBuffer.InternalFallback(c, ref chars);
			}
			char c2;
			while ((c2 = ((encoderFallbackBuffer == null) ? '\0' : encoderFallbackBuffer.InternalGetNextChar())) != '\0' || chars < ptr)
			{
				if (c2 == '\0')
				{
					c2 = *chars;
					chars++;
				}
				if (this.mapUnicodeToBytes[c2] == 0 && c2 != '\0')
				{
					if (encoderFallbackBuffer == null)
					{
						if (encoder == null)
						{
							encoderFallbackBuffer = this.encoderFallback.CreateFallbackBuffer();
						}
						else
						{
							encoderFallbackBuffer = encoder.FallbackBuffer;
						}
						encoderFallbackBuffer.InternalInitialize(ptr - count, ptr, encoder, false);
					}
					encoderFallbackBuffer.InternalFallback(c2, ref chars);
				}
				else
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002A8F RID: 10895 RVA: 0x000891C8 File Offset: 0x000881C8
		internal unsafe override int GetBytes(char* chars, int charCount, byte* bytes, int byteCount, EncoderNLS encoder)
		{
			base.CheckMemorySection();
			char c = '\0';
			EncoderReplacementFallback encoderReplacementFallback;
			if (encoder != null)
			{
				c = encoder.charLeftOver;
				encoderReplacementFallback = encoder.Fallback as EncoderReplacementFallback;
			}
			else
			{
				encoderReplacementFallback = base.EncoderFallback as EncoderReplacementFallback;
			}
			char* ptr = chars + charCount;
			byte* ptr2 = bytes;
			char* ptr3 = chars;
			if (encoderReplacementFallback != null && encoderReplacementFallback.MaxCharCount == 1)
			{
				byte b = this.mapUnicodeToBytes[encoderReplacementFallback.DefaultString[0]];
				if (b != 0)
				{
					if (c > '\0')
					{
						if (byteCount == 0)
						{
							base.ThrowBytesOverflow(encoder, true);
						}
						*(bytes++) = b;
						byteCount--;
					}
					if (byteCount < charCount)
					{
						base.ThrowBytesOverflow(encoder, byteCount < 1);
						ptr = chars + byteCount;
					}
					while (chars < ptr)
					{
						char c2 = *chars;
						chars++;
						byte b2 = this.mapUnicodeToBytes[c2];
						if (b2 == 0 && c2 != '\0')
						{
							*bytes = b;
						}
						else
						{
							*bytes = b2;
						}
						bytes++;
					}
					if (encoder != null)
					{
						encoder.charLeftOver = '\0';
						encoder.m_charsUsed = (int)((long)(chars - ptr3));
					}
					return (int)((long)(bytes - ptr2));
				}
			}
			EncoderFallbackBuffer encoderFallbackBuffer = null;
			byte* ptr4 = bytes + byteCount;
			if (c > '\0')
			{
				encoderFallbackBuffer = encoder.FallbackBuffer;
				encoderFallbackBuffer.InternalInitialize(chars, ptr, encoder, true);
				encoderFallbackBuffer.InternalFallback(c, ref chars);
				if ((long)encoderFallbackBuffer.Remaining > (long)(ptr4 - bytes))
				{
					base.ThrowBytesOverflow(encoder, true);
				}
			}
			char c3;
			while ((c3 = ((encoderFallbackBuffer == null) ? '\0' : encoderFallbackBuffer.InternalGetNextChar())) != '\0' || chars < ptr)
			{
				if (c3 == '\0')
				{
					c3 = *chars;
					chars++;
				}
				byte b3 = this.mapUnicodeToBytes[c3];
				if (b3 == 0 && c3 != '\0')
				{
					if (encoderFallbackBuffer == null)
					{
						if (encoder == null)
						{
							encoderFallbackBuffer = this.encoderFallback.CreateFallbackBuffer();
						}
						else
						{
							encoderFallbackBuffer = encoder.FallbackBuffer;
						}
						encoderFallbackBuffer.InternalInitialize(ptr - charCount, ptr, encoder, true);
					}
					encoderFallbackBuffer.InternalFallback(c3, ref chars);
					if ((long)encoderFallbackBuffer.Remaining > (long)(ptr4 - bytes))
					{
						chars--;
						encoderFallbackBuffer.InternalReset();
						base.ThrowBytesOverflow(encoder, chars == ptr3);
						break;
					}
				}
				else
				{
					if (bytes >= ptr4)
					{
						if (encoderFallbackBuffer == null || !encoderFallbackBuffer.bFallingBack)
						{
							chars--;
						}
						base.ThrowBytesOverflow(encoder, chars == ptr3);
						break;
					}
					*bytes = b3;
					bytes++;
				}
			}
			if (encoder != null)
			{
				if (encoderFallbackBuffer != null && !encoderFallbackBuffer.bUsedEncoder)
				{
					encoder.charLeftOver = '\0';
				}
				encoder.m_charsUsed = (int)((long)(chars - ptr3));
			}
			return (int)((long)(bytes - ptr2));
		}

		// Token: 0x06002A90 RID: 10896 RVA: 0x00089430 File Offset: 0x00088430
		internal unsafe override int GetCharCount(byte* bytes, int count, DecoderNLS decoder)
		{
			base.CheckMemorySection();
			DecoderReplacementFallback decoderReplacementFallback;
			bool flag;
			if (decoder == null)
			{
				decoderReplacementFallback = base.DecoderFallback as DecoderReplacementFallback;
				flag = base.DecoderFallback.IsMicrosoftBestFitFallback;
			}
			else
			{
				decoderReplacementFallback = decoder.Fallback as DecoderReplacementFallback;
				flag = decoder.Fallback.IsMicrosoftBestFitFallback;
			}
			if (flag || (decoderReplacementFallback != null && decoderReplacementFallback.MaxCharCount == 1))
			{
				return count;
			}
			DecoderFallbackBuffer decoderFallbackBuffer = null;
			int num = count;
			byte[] array = new byte[1];
			byte* ptr = bytes + count;
			while (bytes < ptr)
			{
				char c = this.mapBytesToUnicode[*bytes];
				bytes++;
				if (c == '\ufffd')
				{
					if (decoderFallbackBuffer == null)
					{
						if (decoder == null)
						{
							decoderFallbackBuffer = base.DecoderFallback.CreateFallbackBuffer();
						}
						else
						{
							decoderFallbackBuffer = decoder.FallbackBuffer;
						}
						decoderFallbackBuffer.InternalInitialize(ptr - count, null);
					}
					array[0] = *(bytes - 1);
					num--;
					num += decoderFallbackBuffer.InternalFallback(array, bytes);
				}
			}
			return num;
		}

		// Token: 0x06002A91 RID: 10897 RVA: 0x00089504 File Offset: 0x00088504
		internal unsafe override int GetChars(byte* bytes, int byteCount, char* chars, int charCount, DecoderNLS decoder)
		{
			base.CheckMemorySection();
			byte* ptr = bytes + byteCount;
			byte* ptr2 = bytes;
			char* ptr3 = chars;
			DecoderReplacementFallback decoderReplacementFallback;
			bool flag;
			if (decoder == null)
			{
				decoderReplacementFallback = base.DecoderFallback as DecoderReplacementFallback;
				flag = base.DecoderFallback.IsMicrosoftBestFitFallback;
			}
			else
			{
				decoderReplacementFallback = decoder.Fallback as DecoderReplacementFallback;
				flag = decoder.Fallback.IsMicrosoftBestFitFallback;
			}
			if (flag || (decoderReplacementFallback != null && decoderReplacementFallback.MaxCharCount == 1))
			{
				char c;
				if (decoderReplacementFallback == null)
				{
					c = '?';
				}
				else
				{
					c = decoderReplacementFallback.DefaultString[0];
				}
				if (charCount < byteCount)
				{
					base.ThrowCharsOverflow(decoder, charCount < 1);
					ptr = bytes + charCount;
				}
				while (bytes < ptr)
				{
					char c2;
					if (flag)
					{
						if (this.arrayByteBestFit == null)
						{
							this.ReadBestFitTable();
						}
						c2 = this.arrayByteBestFit[(int)(*bytes)];
					}
					else
					{
						c2 = this.mapBytesToUnicode[*bytes];
					}
					bytes++;
					if (c2 == '\ufffd')
					{
						*chars = c;
					}
					else
					{
						*chars = c2;
					}
					chars++;
				}
				if (decoder != null)
				{
					decoder.m_bytesUsed = (int)((long)(bytes - ptr2));
				}
				return (int)((long)(chars - ptr3));
			}
			DecoderFallbackBuffer decoderFallbackBuffer = null;
			byte[] array = new byte[1];
			char* ptr4 = chars + charCount;
			while (bytes < ptr)
			{
				char c3 = this.mapBytesToUnicode[*bytes];
				bytes++;
				if (c3 == '\ufffd')
				{
					if (decoderFallbackBuffer == null)
					{
						if (decoder == null)
						{
							decoderFallbackBuffer = base.DecoderFallback.CreateFallbackBuffer();
						}
						else
						{
							decoderFallbackBuffer = decoder.FallbackBuffer;
						}
						decoderFallbackBuffer.InternalInitialize(ptr - byteCount, ptr4);
					}
					array[0] = *(bytes - 1);
					if (!decoderFallbackBuffer.InternalFallback(array, bytes, ref chars))
					{
						bytes--;
						decoderFallbackBuffer.InternalReset();
						base.ThrowCharsOverflow(decoder, bytes == ptr2);
						break;
					}
				}
				else
				{
					if (chars >= ptr4)
					{
						bytes--;
						base.ThrowCharsOverflow(decoder, bytes == ptr2);
						break;
					}
					*chars = c3;
					chars++;
				}
			}
			if (decoder != null)
			{
				decoder.m_bytesUsed = (int)((long)(bytes - ptr2));
			}
			return (int)((long)(chars - ptr3));
		}

		// Token: 0x06002A92 RID: 10898 RVA: 0x000896EC File Offset: 0x000886EC
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
			if (num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("charCount", Environment.GetResourceString("ArgumentOutOfRange_GetByteCountOverflow"));
			}
			return (int)num;
		}

		// Token: 0x06002A93 RID: 10899 RVA: 0x00089758 File Offset: 0x00088758
		public override int GetMaxCharCount(int byteCount)
		{
			if (byteCount < 0)
			{
				throw new ArgumentOutOfRangeException("byteCount", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			long num = (long)byteCount;
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

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06002A94 RID: 10900 RVA: 0x000897BE File Offset: 0x000887BE
		public override bool IsSingleByte
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x000897C4 File Offset: 0x000887C4
		[ComVisible(false)]
		public override bool IsAlwaysNormalized(NormalizationForm form)
		{
			if (form == NormalizationForm.FormC)
			{
				int codePage = this.CodePage;
				if (codePage <= 10007)
				{
					if (codePage <= 775)
					{
						if (codePage <= 500)
						{
							if (codePage != 37 && codePage != 437 && codePage != 500)
							{
								return false;
							}
						}
						else if (codePage != 720 && codePage != 737 && codePage != 775)
						{
							return false;
						}
					}
					else if (codePage <= 1047)
					{
						switch (codePage)
						{
						case 850:
						case 852:
						case 855:
						case 858:
						case 860:
						case 861:
						case 862:
						case 863:
						case 865:
						case 866:
						case 869:
						case 870:
							break;
						case 851:
						case 853:
						case 854:
						case 856:
						case 857:
						case 859:
						case 864:
						case 867:
						case 868:
							return false;
						default:
							if (codePage != 1026 && codePage != 1047)
							{
								return false;
							}
							break;
						}
					}
					else
					{
						switch (codePage)
						{
						case 1140:
						case 1141:
						case 1142:
						case 1143:
						case 1144:
						case 1145:
						case 1146:
						case 1147:
						case 1148:
						case 1149:
							break;
						default:
							switch (codePage)
							{
							case 1250:
							case 1251:
							case 1252:
							case 1254:
							case 1256:
								break;
							case 1253:
							case 1255:
								return false;
							default:
								if (codePage != 10007)
								{
									return false;
								}
								break;
							}
							break;
						}
					}
				}
				else if (codePage <= 20866)
				{
					if (codePage <= 20273)
					{
						if (codePage != 10017 && codePage != 10029 && codePage != 20273)
						{
							return false;
						}
					}
					else
					{
						switch (codePage)
						{
						case 20277:
						case 20278:
						case 20280:
						case 20284:
						case 20285:
							break;
						case 20279:
						case 20281:
						case 20282:
						case 20283:
							return false;
						default:
							if (codePage != 20297 && codePage != 20866)
							{
								return false;
							}
							break;
						}
					}
				}
				else if (codePage <= 20924)
				{
					if (codePage != 20871 && codePage != 20880 && codePage != 20924)
					{
						return false;
					}
				}
				else if (codePage <= 21866)
				{
					if (codePage != 21025 && codePage != 21866)
					{
						return false;
					}
				}
				else
				{
					switch (codePage)
					{
					case 28591:
					case 28592:
					case 28594:
					case 28595:
					case 28599:
						break;
					case 28593:
					case 28596:
					case 28597:
					case 28598:
						return false;
					default:
						switch (codePage)
						{
						case 28603:
						case 28605:
							break;
						case 28604:
							return false;
						default:
							return false;
						}
						break;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x040014B9 RID: 5305
		private const char UNKNOWN_CHAR = '\ufffd';

		// Token: 0x040014BA RID: 5306
		[NonSerialized]
		private unsafe char* mapBytesToUnicode;

		// Token: 0x040014BB RID: 5307
		[NonSerialized]
		private unsafe byte* mapUnicodeToBytes;

		// Token: 0x040014BC RID: 5308
		[NonSerialized]
		private unsafe int* mapCodePageCached;

		// Token: 0x040014BD RID: 5309
		[NonSerialized]
		private char[] arrayByteBestFit;

		// Token: 0x040014BE RID: 5310
		[NonSerialized]
		private byte byteUnknown;

		// Token: 0x040014BF RID: 5311
		[NonSerialized]
		private char charUnknown;

		// Token: 0x040014C0 RID: 5312
		private static object s_InternalSyncObject;
	}
}
