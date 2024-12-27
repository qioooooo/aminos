using System;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200050E RID: 1294
	[ComVisible(true)]
	[Serializable]
	public class InvalidComObjectException : SystemException
	{
		// Token: 0x06003294 RID: 12948 RVA: 0x000AB7EB File Offset: 0x000AA7EB
		public InvalidComObjectException()
			: base(Environment.GetResourceString("Arg_InvalidComObjectException"))
		{
			base.SetErrorCode(-2146233049);
		}

		// Token: 0x06003295 RID: 12949 RVA: 0x000AB808 File Offset: 0x000AA808
		public InvalidComObjectException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233049);
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x000AB81C File Offset: 0x000AA81C
		public InvalidComObjectException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233049);
		}

		// Token: 0x06003297 RID: 12951 RVA: 0x000AB831 File Offset: 0x000AA831
		protected InvalidComObjectException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
