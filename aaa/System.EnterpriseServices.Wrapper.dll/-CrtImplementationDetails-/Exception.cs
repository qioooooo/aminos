using System;
using System.Runtime.Serialization;

namespace <CrtImplementationDetails>
{
	// Token: 0x020000AB RID: 171
	[Serializable]
	internal class Exception : Exception
	{
		// Token: 0x06000110 RID: 272 RVA: 0x00006140 File Offset: 0x00005540
		protected Exception(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00006128 File Offset: 0x00005528
		public Exception(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00006114 File Offset: 0x00005514
		public Exception(string message)
			: base(message)
		{
		}
	}
}
