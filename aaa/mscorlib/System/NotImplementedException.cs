using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000DB RID: 219
	[ComVisible(true)]
	[Serializable]
	public class NotImplementedException : SystemException
	{
		// Token: 0x06000C25 RID: 3109 RVA: 0x0002423A File Offset: 0x0002323A
		public NotImplementedException()
			: base(Environment.GetResourceString("Arg_NotImplementedException"))
		{
			base.SetErrorCode(-2147467263);
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x00024257 File Offset: 0x00023257
		public NotImplementedException(string message)
			: base(message)
		{
			base.SetErrorCode(-2147467263);
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x0002426B File Offset: 0x0002326B
		public NotImplementedException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2147467263);
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x00024280 File Offset: 0x00023280
		protected NotImplementedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
