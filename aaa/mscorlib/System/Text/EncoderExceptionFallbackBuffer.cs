using System;

namespace System.Text
{
	// Token: 0x020003F3 RID: 1011
	public sealed class EncoderExceptionFallbackBuffer : EncoderFallbackBuffer
	{
		// Token: 0x06002A12 RID: 10770 RVA: 0x0008408C File Offset: 0x0008308C
		public override bool Fallback(char charUnknown, int index)
		{
			throw new EncoderFallbackException(Environment.GetResourceString("Argument_InvalidCodePageConversionIndex", new object[]
			{
				(int)charUnknown,
				index
			}), charUnknown, index);
		}

		// Token: 0x06002A13 RID: 10771 RVA: 0x000840C4 File Offset: 0x000830C4
		public override bool Fallback(char charUnknownHigh, char charUnknownLow, int index)
		{
			if (!char.IsHighSurrogate(charUnknownHigh))
			{
				throw new ArgumentOutOfRangeException("charUnknownHigh", Environment.GetResourceString("ArgumentOutOfRange_Range", new object[] { 55296, 56319 }));
			}
			if (!char.IsLowSurrogate(charUnknownLow))
			{
				throw new ArgumentOutOfRangeException("CharUnknownLow", Environment.GetResourceString("ArgumentOutOfRange_Range", new object[] { 56320, 57343 }));
			}
			int num = char.ConvertToUtf32(charUnknownHigh, charUnknownLow);
			throw new EncoderFallbackException(Environment.GetResourceString("Argument_InvalidCodePageConversionIndex", new object[] { num, index }), charUnknownHigh, charUnknownLow, index);
		}

		// Token: 0x06002A14 RID: 10772 RVA: 0x00084183 File Offset: 0x00083183
		public override char GetNextChar()
		{
			return '\0';
		}

		// Token: 0x06002A15 RID: 10773 RVA: 0x00084186 File Offset: 0x00083186
		public override bool MovePrevious()
		{
			return false;
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06002A16 RID: 10774 RVA: 0x00084189 File Offset: 0x00083189
		public override int Remaining
		{
			get
			{
				return 0;
			}
		}
	}
}
