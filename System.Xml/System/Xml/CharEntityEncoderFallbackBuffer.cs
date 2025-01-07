using System;
using System.Globalization;
using System.Text;

namespace System.Xml
{
	internal class CharEntityEncoderFallbackBuffer : EncoderFallbackBuffer
	{
		internal CharEntityEncoderFallbackBuffer(CharEntityEncoderFallback parent)
		{
			this.parent = parent;
		}

		public override bool Fallback(char charUnknown, int index)
		{
			if (this.charEntityIndex >= 0)
			{
				new EncoderExceptionFallbackBuffer().Fallback(charUnknown, index);
			}
			if (this.parent.CanReplaceAt(index))
			{
				this.charEntity = string.Format(CultureInfo.InvariantCulture, "&#x{0:X};", new object[] { (int)charUnknown });
				this.charEntityIndex = 0;
				return true;
			}
			EncoderFallbackBuffer encoderFallbackBuffer = new EncoderExceptionFallback().CreateFallbackBuffer();
			encoderFallbackBuffer.Fallback(charUnknown, index);
			return false;
		}

		public override bool Fallback(char charUnknownHigh, char charUnknownLow, int index)
		{
			if (!char.IsSurrogatePair(charUnknownHigh, charUnknownLow))
			{
				throw XmlConvert.CreateInvalidSurrogatePairException(charUnknownHigh, charUnknownLow);
			}
			if (this.charEntityIndex >= 0)
			{
				new EncoderExceptionFallbackBuffer().Fallback(charUnknownHigh, charUnknownLow, index);
			}
			if (this.parent.CanReplaceAt(index))
			{
				this.charEntity = string.Format(CultureInfo.InvariantCulture, "&#x{0:X};", new object[] { char.ConvertToUtf32(charUnknownHigh, charUnknownLow) });
				this.charEntityIndex = 0;
				return true;
			}
			EncoderFallbackBuffer encoderFallbackBuffer = new EncoderExceptionFallback().CreateFallbackBuffer();
			encoderFallbackBuffer.Fallback(charUnknownHigh, charUnknownLow, index);
			return false;
		}

		public override char GetNextChar()
		{
			if (this.charEntityIndex == -1)
			{
				return '\0';
			}
			char c = this.charEntity[this.charEntityIndex++];
			if (this.charEntityIndex == this.charEntity.Length)
			{
				this.charEntityIndex = -1;
			}
			return c;
		}

		public override bool MovePrevious()
		{
			if (this.charEntityIndex == -1)
			{
				return false;
			}
			if (this.charEntityIndex > 0)
			{
				this.charEntityIndex--;
				return true;
			}
			return false;
		}

		public override int Remaining
		{
			get
			{
				if (this.charEntityIndex == -1)
				{
					return 0;
				}
				return this.charEntity.Length - this.charEntityIndex;
			}
		}

		public override void Reset()
		{
			this.charEntityIndex = -1;
		}

		private CharEntityEncoderFallback parent;

		private string charEntity = string.Empty;

		private int charEntityIndex = -1;
	}
}
