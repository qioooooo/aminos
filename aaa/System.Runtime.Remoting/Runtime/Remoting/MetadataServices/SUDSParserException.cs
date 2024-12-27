using System;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting.MetadataServices
{
	// Token: 0x02000070 RID: 112
	[Serializable]
	public class SUDSParserException : Exception
	{
		// Token: 0x06000389 RID: 905 RVA: 0x00010D94 File Offset: 0x0000FD94
		internal SUDSParserException(string message)
			: base(message)
		{
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00010D9D File Offset: 0x0000FD9D
		protected SUDSParserException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
