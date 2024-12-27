using System;

namespace System.Text
{
	// Token: 0x020003F6 RID: 1014
	public sealed class EncoderReplacementFallbackBuffer : EncoderFallbackBuffer
	{
		// Token: 0x06002A2A RID: 10794 RVA: 0x000843BB File Offset: 0x000833BB
		public EncoderReplacementFallbackBuffer(EncoderReplacementFallback fallback)
		{
			this.strDefault = fallback.DefaultString + fallback.DefaultString;
		}

		// Token: 0x06002A2B RID: 10795 RVA: 0x000843E8 File Offset: 0x000833E8
		public override bool Fallback(char charUnknown, int index)
		{
			if (this.fallbackCount >= 1)
			{
				if (char.IsHighSurrogate(charUnknown) && this.fallbackCount >= 0 && char.IsLowSurrogate(this.strDefault[this.fallbackIndex + 1]))
				{
					base.ThrowLastCharRecursive(char.ConvertToUtf32(charUnknown, this.strDefault[this.fallbackIndex + 1]));
				}
				base.ThrowLastCharRecursive((int)charUnknown);
			}
			this.fallbackCount = this.strDefault.Length / 2;
			this.fallbackIndex = -1;
			return this.fallbackCount != 0;
		}

		// Token: 0x06002A2C RID: 10796 RVA: 0x00084478 File Offset: 0x00083478
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
			if (this.fallbackCount >= 1)
			{
				base.ThrowLastCharRecursive(char.ConvertToUtf32(charUnknownHigh, charUnknownLow));
			}
			this.fallbackCount = this.strDefault.Length;
			this.fallbackIndex = -1;
			return this.fallbackCount != 0;
		}

		// Token: 0x06002A2D RID: 10797 RVA: 0x00084540 File Offset: 0x00083540
		public override char GetNextChar()
		{
			this.fallbackCount--;
			this.fallbackIndex++;
			if (this.fallbackCount < 0)
			{
				return '\0';
			}
			if (this.fallbackCount == 2147483647)
			{
				this.fallbackCount = -1;
				return '\0';
			}
			return this.strDefault[this.fallbackIndex];
		}

		// Token: 0x06002A2E RID: 10798 RVA: 0x0008459B File Offset: 0x0008359B
		public override bool MovePrevious()
		{
			if (this.fallbackCount >= -1 && this.fallbackIndex >= 0)
			{
				this.fallbackIndex--;
				this.fallbackCount++;
				return true;
			}
			return false;
		}

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06002A2F RID: 10799 RVA: 0x000845CE File Offset: 0x000835CE
		public override int Remaining
		{
			get
			{
				if (this.fallbackCount >= 0)
				{
					return this.fallbackCount;
				}
				return 0;
			}
		}

		// Token: 0x06002A30 RID: 10800 RVA: 0x000845E1 File Offset: 0x000835E1
		public override void Reset()
		{
			this.fallbackCount = -1;
			this.fallbackIndex = 0;
			this.charStart = null;
			this.bFallingBack = false;
		}

		// Token: 0x0400145B RID: 5211
		private string strDefault;

		// Token: 0x0400145C RID: 5212
		private int fallbackCount = -1;

		// Token: 0x0400145D RID: 5213
		private int fallbackIndex = -1;
	}
}
