using System;
using System.Runtime.Serialization;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000626 RID: 1574
	[Serializable]
	public class PingException : InvalidOperationException
	{
		// Token: 0x0600306A RID: 12394 RVA: 0x000D16C0 File Offset: 0x000D06C0
		internal PingException()
		{
		}

		// Token: 0x0600306B RID: 12395 RVA: 0x000D16C8 File Offset: 0x000D06C8
		protected PingException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x0600306C RID: 12396 RVA: 0x000D16D2 File Offset: 0x000D06D2
		public PingException(string message)
			: base(message)
		{
		}

		// Token: 0x0600306D RID: 12397 RVA: 0x000D16DB File Offset: 0x000D06DB
		public PingException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
