using System;
using System.Runtime.Serialization;

namespace System.Text
{
	// Token: 0x020003EA RID: 1002
	[Serializable]
	public sealed class DecoderFallbackException : ArgumentException
	{
		// Token: 0x060029C9 RID: 10697 RVA: 0x0008329F File Offset: 0x0008229F
		public DecoderFallbackException()
			: base(Environment.GetResourceString("Arg_ArgumentException"))
		{
			base.SetErrorCode(-2147024809);
		}

		// Token: 0x060029CA RID: 10698 RVA: 0x000832BC File Offset: 0x000822BC
		public DecoderFallbackException(string message)
			: base(message)
		{
			base.SetErrorCode(-2147024809);
		}

		// Token: 0x060029CB RID: 10699 RVA: 0x000832D0 File Offset: 0x000822D0
		public DecoderFallbackException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2147024809);
		}

		// Token: 0x060029CC RID: 10700 RVA: 0x000832E5 File Offset: 0x000822E5
		internal DecoderFallbackException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060029CD RID: 10701 RVA: 0x000832EF File Offset: 0x000822EF
		public DecoderFallbackException(string message, byte[] bytesUnknown, int index)
			: base(message)
		{
			this.bytesUnknown = bytesUnknown;
			this.index = index;
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x060029CE RID: 10702 RVA: 0x00083306 File Offset: 0x00082306
		public byte[] BytesUnknown
		{
			get
			{
				return this.bytesUnknown;
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x060029CF RID: 10703 RVA: 0x0008330E File Offset: 0x0008230E
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x04001438 RID: 5176
		private byte[] bytesUnknown;

		// Token: 0x04001439 RID: 5177
		private int index;
	}
}
