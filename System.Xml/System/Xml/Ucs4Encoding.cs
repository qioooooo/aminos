﻿using System;
using System.Text;

namespace System.Xml
{
	internal class Ucs4Encoding : Encoding
	{
		public override Decoder GetDecoder()
		{
			return this.ucs4Decoder;
		}

		public override int GetByteCount(char[] chars, int index, int count)
		{
			return checked(count * 4);
		}

		public override int GetByteCount(char[] chars)
		{
			return chars.Length * 4;
		}

		public override byte[] GetBytes(string s)
		{
			return null;
		}

		public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			return 0;
		}

		public override int GetMaxByteCount(int charCount)
		{
			return 0;
		}

		public override int GetCharCount(byte[] bytes, int index, int count)
		{
			return this.ucs4Decoder.GetCharCount(bytes, index, count);
		}

		public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			return this.ucs4Decoder.GetChars(bytes, byteIndex, byteCount, chars, charIndex);
		}

		public override int GetMaxCharCount(int byteCount)
		{
			return (byteCount + 3) / 4;
		}

		public override int CodePage
		{
			get
			{
				return 0;
			}
		}

		public override int GetCharCount(byte[] bytes)
		{
			return bytes.Length / 4;
		}

		public override Encoder GetEncoder()
		{
			return null;
		}

		internal static Encoding UCS4_Littleendian
		{
			get
			{
				return new Ucs4Encoding4321();
			}
		}

		internal static Encoding UCS4_Bigendian
		{
			get
			{
				return new Ucs4Encoding1234();
			}
		}

		internal static Encoding UCS4_2143
		{
			get
			{
				return new Ucs4Encoding2143();
			}
		}

		internal static Encoding UCS4_3412
		{
			get
			{
				return new Ucs4Encoding3412();
			}
		}

		internal Ucs4Decoder ucs4Decoder;
	}
}
