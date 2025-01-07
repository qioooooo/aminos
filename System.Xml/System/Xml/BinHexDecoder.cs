using System;

namespace System.Xml
{
	internal class BinHexDecoder : IncrementalReadDecoder
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
					BinHexDecoder.Decode(ptr, ptr + len, ptr2, ptr2 + (this.endIndex - this.curIndex), ref this.hasHalfByteCached, ref this.cachedHalfByte, out num, out num2);
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
					BinHexDecoder.Decode(ptr + startPos, ptr + startPos + len, ptr2, ptr2 + (this.endIndex - this.curIndex), ref this.hasHalfByteCached, ref this.cachedHalfByte, out num, out num2);
				}
			}
			this.curIndex += num2;
			return num;
		}

		internal override void Reset()
		{
			this.hasHalfByteCached = false;
			this.cachedHalfByte = 0;
		}

		internal override void SetNextOutputBuffer(Array buffer, int index, int count)
		{
			this.buffer = (byte[])buffer;
			this.startIndex = index;
			this.curIndex = index;
			this.endIndex = index + count;
		}

		public unsafe static byte[] Decode(char[] chars, bool allowOddChars)
		{
			if (chars == null)
			{
				throw new ArgumentException("chars");
			}
			int num = chars.Length;
			if (num == 0)
			{
				return new byte[0];
			}
			byte[] array = new byte[(num + 1) / 2];
			bool flag = false;
			byte b = 0;
			int num3;
			fixed (char* ptr = &chars[0])
			{
				fixed (byte* ptr2 = &array[0])
				{
					int num2;
					BinHexDecoder.Decode(ptr, ptr + num, ptr2, ptr2 + array.Length, ref flag, ref b, out num2, out num3);
				}
			}
			if (flag && !allowOddChars)
			{
				throw new XmlException("Xml_InvalidBinHexValueOddCount", new string(chars));
			}
			if (num3 < array.Length)
			{
				byte[] array2 = new byte[num3];
				Buffer.BlockCopy(array, 0, array2, 0, num3);
				array = array2;
			}
			return array;
		}

		private unsafe static void Decode(char* pChars, char* pCharsEndPos, byte* pBytes, byte* pBytesEndPos, ref bool hasHalfByteCached, ref byte cachedHalfByte, out int charsDecoded, out int bytesDecoded)
		{
			char* ptr = pChars;
			byte* ptr2 = pBytes;
			XmlCharType instance = XmlCharType.Instance;
			while (ptr < pCharsEndPos && ptr2 < pBytesEndPos)
			{
				char c = *(ptr++);
				byte b;
				if (c >= 'a' && c <= 'f')
				{
					b = (byte)(c - 'a' + '\n');
				}
				else if (c >= 'A' && c <= 'F')
				{
					b = (byte)(c - 'A' + '\n');
				}
				else if (c >= '0' && c <= '9')
				{
					b = (byte)(c - '0');
				}
				else
				{
					if ((instance.charProperties[c] & 1) == 0)
					{
						throw new XmlException("Xml_InvalidBinHexValue", new string(pChars, 0, (int)((long)(pCharsEndPos - pChars))));
					}
					continue;
				}
				if (hasHalfByteCached)
				{
					*(ptr2++) = (byte)(((int)cachedHalfByte << 4) + (int)b);
					hasHalfByteCached = false;
				}
				else
				{
					cachedHalfByte = b;
					hasHalfByteCached = true;
				}
			}
			bytesDecoded = (int)((long)(ptr2 - pBytes));
			charsDecoded = (int)((long)(ptr - pChars));
		}

		private byte[] buffer;

		private int startIndex;

		private int curIndex;

		private int endIndex;

		private bool hasHalfByteCached;

		private byte cachedHalfByte;
	}
}
