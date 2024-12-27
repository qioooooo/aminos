using System;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200051C RID: 1308
	[ComVisible(true)]
	[Serializable]
	public class SafeArrayRankMismatchException : SystemException
	{
		// Token: 0x060032C7 RID: 12999 RVA: 0x000ACEF4 File Offset: 0x000ABEF4
		public SafeArrayRankMismatchException()
			: base(Environment.GetResourceString("Arg_SafeArrayRankMismatchException"))
		{
			base.SetErrorCode(-2146233032);
		}

		// Token: 0x060032C8 RID: 13000 RVA: 0x000ACF11 File Offset: 0x000ABF11
		public SafeArrayRankMismatchException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233032);
		}

		// Token: 0x060032C9 RID: 13001 RVA: 0x000ACF25 File Offset: 0x000ABF25
		public SafeArrayRankMismatchException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233032);
		}

		// Token: 0x060032CA RID: 13002 RVA: 0x000ACF3A File Offset: 0x000ABF3A
		protected SafeArrayRankMismatchException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
