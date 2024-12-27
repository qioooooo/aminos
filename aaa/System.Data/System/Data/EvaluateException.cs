using System;
using System.Runtime.Serialization;

namespace System.Data
{
	// Token: 0x020001AF RID: 431
	[Serializable]
	public class EvaluateException : InvalidExpressionException
	{
		// Token: 0x060018CA RID: 6346 RVA: 0x0023BF28 File Offset: 0x0023B328
		protected EvaluateException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060018CB RID: 6347 RVA: 0x0023BF40 File Offset: 0x0023B340
		public EvaluateException()
		{
		}

		// Token: 0x060018CC RID: 6348 RVA: 0x0023BF54 File Offset: 0x0023B354
		public EvaluateException(string s)
			: base(s)
		{
		}

		// Token: 0x060018CD RID: 6349 RVA: 0x0023BF68 File Offset: 0x0023B368
		public EvaluateException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
