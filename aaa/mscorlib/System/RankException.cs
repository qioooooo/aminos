using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000EB RID: 235
	[ComVisible(true)]
	[Serializable]
	public class RankException : SystemException
	{
		// Token: 0x06000C93 RID: 3219 RVA: 0x00025BAD File Offset: 0x00024BAD
		public RankException()
			: base(Environment.GetResourceString("Arg_RankException"))
		{
			base.SetErrorCode(-2146233065);
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x00025BCA File Offset: 0x00024BCA
		public RankException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233065);
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x00025BDE File Offset: 0x00024BDE
		public RankException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146233065);
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x00025BF3 File Offset: 0x00024BF3
		protected RankException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
