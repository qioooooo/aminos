using System;
using System.Runtime.Serialization;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000079 RID: 121
	[Serializable]
	public class TlsOperationException : DirectoryOperationException
	{
		// Token: 0x0600028E RID: 654 RVA: 0x0000D672 File Offset: 0x0000C672
		protected TlsOperationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000D67C File Offset: 0x0000C67C
		public TlsOperationException()
		{
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000D684 File Offset: 0x0000C684
		public TlsOperationException(string message)
			: base(message)
		{
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000D68D File Offset: 0x0000C68D
		public TlsOperationException(string message, Exception inner)
			: base(message, inner)
		{
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000D697 File Offset: 0x0000C697
		public TlsOperationException(DirectoryResponse response)
			: base(response)
		{
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000D6A0 File Offset: 0x0000C6A0
		public TlsOperationException(DirectoryResponse response, string message)
			: base(response, message)
		{
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000D6AA File Offset: 0x0000C6AA
		public TlsOperationException(DirectoryResponse response, string message, Exception inner)
			: base(response, message, inner)
		{
		}
	}
}
