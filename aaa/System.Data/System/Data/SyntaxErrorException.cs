using System;
using System.Runtime.Serialization;

namespace System.Data
{
	// Token: 0x020001B0 RID: 432
	[Serializable]
	public class SyntaxErrorException : InvalidExpressionException
	{
		// Token: 0x060018CE RID: 6350 RVA: 0x0023BF80 File Offset: 0x0023B380
		protected SyntaxErrorException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x0023BF98 File Offset: 0x0023B398
		public SyntaxErrorException()
		{
		}

		// Token: 0x060018D0 RID: 6352 RVA: 0x0023BFAC File Offset: 0x0023B3AC
		public SyntaxErrorException(string s)
			: base(s)
		{
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x0023BFC0 File Offset: 0x0023B3C0
		public SyntaxErrorException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
