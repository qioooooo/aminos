using System;
using System.Runtime.Serialization;

namespace System.ServiceProcess
{
	// Token: 0x02000037 RID: 55
	[Serializable]
	public class TimeoutException : SystemException
	{
		// Token: 0x0600010D RID: 269 RVA: 0x00006323 File Offset: 0x00005323
		public TimeoutException()
		{
			base.HResult = -2146232058;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00006336 File Offset: 0x00005336
		public TimeoutException(string message)
			: base(message)
		{
			base.HResult = -2146232058;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000634A File Offset: 0x0000534A
		public TimeoutException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2146232058;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000635F File Offset: 0x0000535F
		protected TimeoutException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
