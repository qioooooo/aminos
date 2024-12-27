using System;

namespace System.Text
{
	// Token: 0x020003F2 RID: 1010
	[Serializable]
	public sealed class EncoderExceptionFallback : EncoderFallback
	{
		// Token: 0x06002A0E RID: 10766 RVA: 0x0008405B File Offset: 0x0008305B
		public override EncoderFallbackBuffer CreateFallbackBuffer()
		{
			return new EncoderExceptionFallbackBuffer();
		}

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06002A0F RID: 10767 RVA: 0x00084062 File Offset: 0x00083062
		public override int MaxCharCount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06002A10 RID: 10768 RVA: 0x00084068 File Offset: 0x00083068
		public override bool Equals(object value)
		{
			return value is EncoderExceptionFallback;
		}

		// Token: 0x06002A11 RID: 10769 RVA: 0x00084082 File Offset: 0x00083082
		public override int GetHashCode()
		{
			return 654;
		}
	}
}
