using System;

namespace System.Text
{
	// Token: 0x020003EF RID: 1007
	[Serializable]
	internal class InternalEncoderBestFitFallback : EncoderFallback
	{
		// Token: 0x060029F3 RID: 10739 RVA: 0x00083B78 File Offset: 0x00082B78
		internal InternalEncoderBestFitFallback(Encoding encoding)
		{
			this.encoding = encoding;
			this.bIsMicrosoftBestFitFallback = true;
		}

		// Token: 0x060029F4 RID: 10740 RVA: 0x00083B8E File Offset: 0x00082B8E
		public override EncoderFallbackBuffer CreateFallbackBuffer()
		{
			return new InternalEncoderBestFitFallbackBuffer(this);
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x060029F5 RID: 10741 RVA: 0x00083B96 File Offset: 0x00082B96
		public override int MaxCharCount
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060029F6 RID: 10742 RVA: 0x00083B9C File Offset: 0x00082B9C
		public override bool Equals(object value)
		{
			InternalEncoderBestFitFallback internalEncoderBestFitFallback = value as InternalEncoderBestFitFallback;
			return internalEncoderBestFitFallback != null && this.encoding.CodePage == internalEncoderBestFitFallback.encoding.CodePage;
		}

		// Token: 0x060029F7 RID: 10743 RVA: 0x00083BCD File Offset: 0x00082BCD
		public override int GetHashCode()
		{
			return this.encoding.CodePage;
		}

		// Token: 0x04001447 RID: 5191
		internal Encoding encoding;

		// Token: 0x04001448 RID: 5192
		internal char[] arrayBestFit;
	}
}
