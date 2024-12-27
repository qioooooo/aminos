using System;

namespace System.Text
{
	// Token: 0x020003F0 RID: 1008
	public abstract class EncoderFallbackBuffer
	{
		// Token: 0x060029F8 RID: 10744
		public abstract bool Fallback(char charUnknown, int index);

		// Token: 0x060029F9 RID: 10745
		public abstract bool Fallback(char charUnknownHigh, char charUnknownLow, int index);

		// Token: 0x060029FA RID: 10746
		public abstract char GetNextChar();

		// Token: 0x060029FB RID: 10747
		public abstract bool MovePrevious();

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x060029FC RID: 10748
		public abstract int Remaining { get; }

		// Token: 0x060029FD RID: 10749 RVA: 0x00083BDA File Offset: 0x00082BDA
		public virtual void Reset()
		{
			while (this.GetNextChar() != '\0')
			{
			}
		}

		// Token: 0x060029FE RID: 10750 RVA: 0x00083BE4 File Offset: 0x00082BE4
		internal void InternalReset()
		{
			this.charStart = null;
			this.bFallingBack = false;
			this.iRecursionCount = 0;
			this.Reset();
		}

		// Token: 0x060029FF RID: 10751 RVA: 0x00083C02 File Offset: 0x00082C02
		internal unsafe void InternalInitialize(char* charStart, char* charEnd, EncoderNLS encoder, bool setEncoder)
		{
			this.charStart = charStart;
			this.charEnd = charEnd;
			this.encoder = encoder;
			this.setEncoder = setEncoder;
			this.bUsedEncoder = false;
			this.bFallingBack = false;
			this.iRecursionCount = 0;
		}

		// Token: 0x06002A00 RID: 10752 RVA: 0x00083C38 File Offset: 0x00082C38
		internal char InternalGetNextChar()
		{
			char nextChar = this.GetNextChar();
			this.bFallingBack = nextChar != '\0';
			if (nextChar == '\0')
			{
				this.iRecursionCount = 0;
			}
			return nextChar;
		}

		// Token: 0x06002A01 RID: 10753 RVA: 0x00083C64 File Offset: 0x00082C64
		internal unsafe virtual bool InternalFallback(char ch, ref char* chars)
		{
			int num = (chars - this.charStart) / 2 - 1;
			if (char.IsHighSurrogate(ch))
			{
				if (chars >= this.charEnd)
				{
					if (this.encoder != null && !this.encoder.MustFlush)
					{
						if (this.setEncoder)
						{
							this.bUsedEncoder = true;
							this.encoder.charLeftOver = ch;
						}
						this.bFallingBack = false;
						return false;
					}
				}
				else
				{
					char c = (char)(*chars);
					if (char.IsLowSurrogate(c))
					{
						if (this.bFallingBack && this.iRecursionCount++ > 250)
						{
							this.ThrowLastCharRecursive(char.ConvertToUtf32(ch, c));
						}
						chars += (IntPtr)2;
						this.bFallingBack = this.Fallback(ch, c, num);
						return this.bFallingBack;
					}
				}
			}
			if (this.bFallingBack && this.iRecursionCount++ > 250)
			{
				this.ThrowLastCharRecursive((int)ch);
			}
			this.bFallingBack = this.Fallback(ch, num);
			return this.bFallingBack;
		}

		// Token: 0x06002A02 RID: 10754 RVA: 0x00083D64 File Offset: 0x00082D64
		internal void ThrowLastCharRecursive(int charRecursive)
		{
			throw new ArgumentException(Environment.GetResourceString("Argument_RecursiveFallback", new object[] { charRecursive }), "chars");
		}

		// Token: 0x04001449 RID: 5193
		private const int iMaxRecursion = 250;

		// Token: 0x0400144A RID: 5194
		internal unsafe char* charStart = null;

		// Token: 0x0400144B RID: 5195
		internal unsafe char* charEnd;

		// Token: 0x0400144C RID: 5196
		internal EncoderNLS encoder;

		// Token: 0x0400144D RID: 5197
		internal bool setEncoder;

		// Token: 0x0400144E RID: 5198
		internal bool bUsedEncoder;

		// Token: 0x0400144F RID: 5199
		internal bool bFallingBack;

		// Token: 0x04001450 RID: 5200
		internal int iRecursionCount;
	}
}
