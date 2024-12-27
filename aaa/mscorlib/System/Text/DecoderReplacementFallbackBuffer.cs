using System;

namespace System.Text
{
	// Token: 0x020003EC RID: 1004
	public sealed class DecoderReplacementFallbackBuffer : DecoderFallbackBuffer
	{
		// Token: 0x060029D7 RID: 10711 RVA: 0x000833FF File Offset: 0x000823FF
		public DecoderReplacementFallbackBuffer(DecoderReplacementFallback fallback)
		{
			this.strDefault = fallback.DefaultString;
		}

		// Token: 0x060029D8 RID: 10712 RVA: 0x00083421 File Offset: 0x00082421
		public override bool Fallback(byte[] bytesUnknown, int index)
		{
			if (this.fallbackCount >= 1)
			{
				base.ThrowLastBytesRecursive(bytesUnknown);
			}
			if (this.strDefault.Length == 0)
			{
				return false;
			}
			this.fallbackCount = this.strDefault.Length;
			this.fallbackIndex = -1;
			return true;
		}

		// Token: 0x060029D9 RID: 10713 RVA: 0x0008345C File Offset: 0x0008245C
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

		// Token: 0x060029DA RID: 10714 RVA: 0x000834B7 File Offset: 0x000824B7
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

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x060029DB RID: 10715 RVA: 0x000834EA File Offset: 0x000824EA
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

		// Token: 0x060029DC RID: 10716 RVA: 0x000834FD File Offset: 0x000824FD
		public override void Reset()
		{
			this.fallbackCount = -1;
			this.fallbackIndex = -1;
			this.byteStart = null;
		}

		// Token: 0x060029DD RID: 10717 RVA: 0x00083515 File Offset: 0x00082515
		internal unsafe override int InternalFallback(byte[] bytes, byte* pBytes)
		{
			return this.strDefault.Length;
		}

		// Token: 0x0400143B RID: 5179
		private string strDefault;

		// Token: 0x0400143C RID: 5180
		private int fallbackCount = -1;

		// Token: 0x0400143D RID: 5181
		private int fallbackIndex = -1;
	}
}
