using System;
using System.Runtime.Serialization;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000064 RID: 100
	[Serializable]
	public class DsmlInvalidDocumentException : DirectoryException
	{
		// Token: 0x060001F4 RID: 500 RVA: 0x000086F2 File Offset: 0x000076F2
		public DsmlInvalidDocumentException()
			: base(Res.GetString("InvalidDocument"))
		{
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00008704 File Offset: 0x00007704
		public DsmlInvalidDocumentException(string message)
			: base(message)
		{
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000870D File Offset: 0x0000770D
		public DsmlInvalidDocumentException(string message, Exception inner)
			: base(message, inner)
		{
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00008717 File Offset: 0x00007717
		protected DsmlInvalidDocumentException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
