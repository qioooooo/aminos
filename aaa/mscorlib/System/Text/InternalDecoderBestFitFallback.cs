using System;

namespace System.Text
{
	// Token: 0x020003E5 RID: 997
	[Serializable]
	internal sealed class InternalDecoderBestFitFallback : DecoderFallback
	{
		// Token: 0x060029A5 RID: 10661 RVA: 0x00082CE0 File Offset: 0x00081CE0
		internal InternalDecoderBestFitFallback(Encoding encoding)
		{
			this.encoding = encoding;
			this.bIsMicrosoftBestFitFallback = true;
		}

		// Token: 0x060029A6 RID: 10662 RVA: 0x00082CFE File Offset: 0x00081CFE
		public override DecoderFallbackBuffer CreateFallbackBuffer()
		{
			return new InternalDecoderBestFitFallbackBuffer(this);
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x060029A7 RID: 10663 RVA: 0x00082D06 File Offset: 0x00081D06
		public override int MaxCharCount
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060029A8 RID: 10664 RVA: 0x00082D0C File Offset: 0x00081D0C
		public override bool Equals(object value)
		{
			InternalDecoderBestFitFallback internalDecoderBestFitFallback = value as InternalDecoderBestFitFallback;
			return internalDecoderBestFitFallback != null && this.encoding.CodePage == internalDecoderBestFitFallback.encoding.CodePage;
		}

		// Token: 0x060029A9 RID: 10665 RVA: 0x00082D3D File Offset: 0x00081D3D
		public override int GetHashCode()
		{
			return this.encoding.CodePage;
		}

		// Token: 0x0400142E RID: 5166
		internal Encoding encoding;

		// Token: 0x0400142F RID: 5167
		internal char[] arrayBestFit;

		// Token: 0x04001430 RID: 5168
		internal char cReplacement = '?';
	}
}
