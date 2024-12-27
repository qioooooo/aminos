using System;

namespace System.Text
{
	// Token: 0x020003E8 RID: 1000
	[Serializable]
	public sealed class DecoderExceptionFallback : DecoderFallback
	{
		// Token: 0x060029BF RID: 10687 RVA: 0x000831BC File Offset: 0x000821BC
		public override DecoderFallbackBuffer CreateFallbackBuffer()
		{
			return new DecoderExceptionFallbackBuffer();
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x060029C0 RID: 10688 RVA: 0x000831C3 File Offset: 0x000821C3
		public override int MaxCharCount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060029C1 RID: 10689 RVA: 0x000831C8 File Offset: 0x000821C8
		public override bool Equals(object value)
		{
			return value is DecoderExceptionFallback;
		}

		// Token: 0x060029C2 RID: 10690 RVA: 0x000831E2 File Offset: 0x000821E2
		public override int GetHashCode()
		{
			return 879;
		}
	}
}
