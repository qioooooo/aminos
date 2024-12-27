using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000D8 RID: 216
	[ComVisible(true)]
	[Serializable]
	public sealed class MulticastNotSupportedException : SystemException
	{
		// Token: 0x06000C15 RID: 3093 RVA: 0x000240A6 File Offset: 0x000230A6
		public MulticastNotSupportedException()
			: base(Environment.GetResourceString("Arg_MulticastNotSupportedException"))
		{
			base.SetErrorCode(-2146233068);
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x000240C3 File Offset: 0x000230C3
		public MulticastNotSupportedException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233068);
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x000240D7 File Offset: 0x000230D7
		public MulticastNotSupportedException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233068);
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x000240EC File Offset: 0x000230EC
		internal MulticastNotSupportedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
