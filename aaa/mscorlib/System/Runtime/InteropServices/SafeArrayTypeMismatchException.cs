using System;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200051D RID: 1309
	[ComVisible(true)]
	[Serializable]
	public class SafeArrayTypeMismatchException : SystemException
	{
		// Token: 0x060032CB RID: 13003 RVA: 0x000ACF44 File Offset: 0x000ABF44
		public SafeArrayTypeMismatchException()
			: base(Environment.GetResourceString("Arg_SafeArrayTypeMismatchException"))
		{
			base.SetErrorCode(-2146233037);
		}

		// Token: 0x060032CC RID: 13004 RVA: 0x000ACF61 File Offset: 0x000ABF61
		public SafeArrayTypeMismatchException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233037);
		}

		// Token: 0x060032CD RID: 13005 RVA: 0x000ACF75 File Offset: 0x000ABF75
		public SafeArrayTypeMismatchException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233037);
		}

		// Token: 0x060032CE RID: 13006 RVA: 0x000ACF8A File Offset: 0x000ABF8A
		protected SafeArrayTypeMismatchException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
