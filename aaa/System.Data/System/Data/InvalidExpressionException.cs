using System;
using System.Runtime.Serialization;

namespace System.Data
{
	// Token: 0x020001AE RID: 430
	[Serializable]
	public class InvalidExpressionException : DataException
	{
		// Token: 0x060018C6 RID: 6342 RVA: 0x0023BED0 File Offset: 0x0023B2D0
		protected InvalidExpressionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060018C7 RID: 6343 RVA: 0x0023BEE8 File Offset: 0x0023B2E8
		public InvalidExpressionException()
		{
		}

		// Token: 0x060018C8 RID: 6344 RVA: 0x0023BEFC File Offset: 0x0023B2FC
		public InvalidExpressionException(string s)
			: base(s)
		{
		}

		// Token: 0x060018C9 RID: 6345 RVA: 0x0023BF10 File Offset: 0x0023B310
		public InvalidExpressionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
