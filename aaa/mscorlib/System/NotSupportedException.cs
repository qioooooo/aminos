using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000DC RID: 220
	[ComVisible(true)]
	[Serializable]
	public class NotSupportedException : SystemException
	{
		// Token: 0x06000C29 RID: 3113 RVA: 0x0002428A File Offset: 0x0002328A
		public NotSupportedException()
			: base(Environment.GetResourceString("Arg_NotSupportedException"))
		{
			base.SetErrorCode(-2146233067);
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x000242A7 File Offset: 0x000232A7
		public NotSupportedException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233067);
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x000242BB File Offset: 0x000232BB
		public NotSupportedException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146233067);
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x000242D0 File Offset: 0x000232D0
		protected NotSupportedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
