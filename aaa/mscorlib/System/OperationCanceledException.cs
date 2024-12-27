using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000E4 RID: 228
	[ComVisible(true)]
	[Serializable]
	public class OperationCanceledException : SystemException
	{
		// Token: 0x06000C74 RID: 3188 RVA: 0x0002578A File Offset: 0x0002478A
		public OperationCanceledException()
			: base(Environment.GetResourceString("OperationCanceled"))
		{
			base.SetErrorCode(-2146233029);
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x000257A7 File Offset: 0x000247A7
		public OperationCanceledException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233029);
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x000257BB File Offset: 0x000247BB
		public OperationCanceledException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146233029);
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x000257D0 File Offset: 0x000247D0
		protected OperationCanceledException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
