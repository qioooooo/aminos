using System;
using System.Runtime.Serialization;

namespace System.Net
{
	// Token: 0x020003EB RID: 1003
	internal class InternalException : SystemException
	{
		// Token: 0x06002076 RID: 8310 RVA: 0x0007FD6F File Offset: 0x0007ED6F
		internal InternalException()
		{
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x0007FD77 File Offset: 0x0007ED77
		internal InternalException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}
	}
}
