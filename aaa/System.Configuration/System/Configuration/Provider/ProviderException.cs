using System;
using System.Runtime.Serialization;

namespace System.Configuration.Provider
{
	// Token: 0x0200008C RID: 140
	[Serializable]
	public class ProviderException : Exception
	{
		// Token: 0x0600051D RID: 1309 RVA: 0x00019A4E File Offset: 0x00018A4E
		public ProviderException()
		{
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00019A56 File Offset: 0x00018A56
		public ProviderException(string message)
			: base(message)
		{
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00019A5F File Offset: 0x00018A5F
		public ProviderException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00019A69 File Offset: 0x00018A69
		protected ProviderException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
