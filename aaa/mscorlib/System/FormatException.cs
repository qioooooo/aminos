using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000B9 RID: 185
	[ComVisible(true)]
	[Serializable]
	public class FormatException : SystemException
	{
		// Token: 0x06000AB2 RID: 2738 RVA: 0x00020DE4 File Offset: 0x0001FDE4
		public FormatException()
			: base(Environment.GetResourceString("Arg_FormatException"))
		{
			base.SetErrorCode(-2146233033);
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x00020E01 File Offset: 0x0001FE01
		public FormatException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233033);
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x00020E15 File Offset: 0x0001FE15
		public FormatException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146233033);
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x00020E2A File Offset: 0x0001FE2A
		protected FormatException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
