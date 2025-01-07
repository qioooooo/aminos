using System;

namespace System.Xml
{
	internal class Base64Decoder : IncrementalReadDecoder
	{
		internal override int DecodedCount
		{
			get
			{
				return this.curIndex - this.startIndex;
			}
		}

		internal override bool IsFull
		{
			get
			{
				return this.curIndex == this.endIndex;
			}
		}

		internal unsafe override int Decode(char[] chars, int startPos, int len)
		{
			if (len == 0)
			{
				return 0;
			}
			int num;
			int num2;
			fixed (char* ptr = &chars[startPos])
			{
				fixed (byte* ptr2 = &this.buffer[this.curIndex])
				{
					this.Decode(ptr, ptr + len, ptr2, ptr2 + (this.endIndex - this.curIndex), out num, out num2);
				}
			}
			this.curIndex += num2;
			return num;
		}

		internal unsafe override int Decode(string str, int startPos, int len)
		{
			if (len == 0)
			{
				return 0;
			}
			int num;
			int num2;
			fixed (char* ptr = str)
			{
				fixed (byte* ptr2 = &this.buffer[this.curIndex])
				{
					this.Decode(ptr + startPos, ptr + startPos + len, ptr2, ptr2 + (this.endIndex - this.curIndex), out num, out num2);
				}
			}
			this.curIndex += num2;
			return num;
		}

		internal override void Reset()
		{
			this.bitsFilled = 0;
			this.bits = 0;
		}

		internal override void SetNextOutputBuffer(Array buffer, int index, int count)
		{
			this.buffer = (byte[])buffer;
			this.startIndex = index;
			this.curIndex = index;
			this.endIndex = index + count;
		}

		private static byte[] ConstructMapBase64()
		{
			byte[] array = new byte[123];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = byte.MaxValue;
			}
			for (int j = 0; j < Base64Decoder.CharsBase64.Length; j++)
			{
				array[(int)Base64Decoder.CharsBase64[j]] = (byte)j;
			}
			return array;
		}

		private unsafe void Decode(char* pChars, char* pCharsEndPos, byte* pBytes, byte* pBytesEndPos, out int charsDecoded, out int bytesDecoded)
		{
			byte* ptr = pBytes;
			char* ptr2 = pChars;
			int num = this.bits;
			int num2 = this.bitsFilled;
			XmlCharType instance = XmlCharType.Instance;
			while (ptr2 < pCharsEndPos && ptr < pBytesEndPos)
			{
				char c = *ptr2;
				if (c == '=')
				{
					break;
				}
				ptr2++;
				if ((instance.charProperties[c] & 1) == 0)
				{
					int num3;
					if (c > 'z' || (num3 = (int)Base64Decoder.MapBase64[(int)c]) == 255)
					{
						throw new XmlException("Xml_InvalidBase64Value", new string(pChars, 0, (int)((long)(pCharsEndPos - pChars))));
					}
					num = (num << 6) | num3;
					num2 += 6;
					if (num2 >= 8)
					{
						*(ptr++) = (byte)((num >> num2 - 8) & 255);
						num2 -= 8;
						if (ptr == pBytesEndPos)
						{
							IL_00F4:
							this.bits = num;
							this.bitsFilled = num2;
							bytesDecoded = (int)((long)(ptr - pBytes));
							charsDecoded = (int)((long)(ptr2 - pChars));
							return;
						}
					}
				}
			}
			if (ptr2 >= pCharsEndPos || *ptr2 != '=')
			{
				goto IL_00F4;
			}
			num2 = 0;
			do
			{
				ptr2++;
			}
			while (ptr2 < pCharsEndPos && *ptr2 == '=');
			if (ptr2 < pCharsEndPos)
			{
				while ((instance.charProperties[*(ptr2++)] & 1) != 0)
				{
					if (ptr2 >= pCharsEndPos)
					{
						goto IL_00F4;
					}
				}
				throw new XmlException("Xml_InvalidBase64Value", new string(pChars, 0, (int)((long)(pCharsEndPos - pChars))));
			}
			goto IL_00F4;
		}

		private const int MaxValidChar = 122;

		private const byte Invalid = 255;

		private byte[] buffer;

		private int startIndex;

		private int curIndex;

		private int endIndex;

		private int bits;

		private int bitsFilled;

		private static readonly string CharsBase64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

		private static readonly byte[] MapBase64 = Base64Decoder.ConstructMapBase64();
	}
}
