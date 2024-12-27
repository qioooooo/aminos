using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x020002FB RID: 763
	[ComVisible(true)]
	[Serializable]
	public class InvalidFilterCriteriaException : ApplicationException
	{
		// Token: 0x06001E4E RID: 7758 RVA: 0x0004D1A2 File Offset: 0x0004C1A2
		public InvalidFilterCriteriaException()
			: base(Environment.GetResourceString("Arg_InvalidFilterCriteriaException"))
		{
			base.SetErrorCode(-2146232831);
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x0004D1BF File Offset: 0x0004C1BF
		public InvalidFilterCriteriaException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146232831);
		}

		// Token: 0x06001E50 RID: 7760 RVA: 0x0004D1D3 File Offset: 0x0004C1D3
		public InvalidFilterCriteriaException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146232831);
		}

		// Token: 0x06001E51 RID: 7761 RVA: 0x0004D1E8 File Offset: 0x0004C1E8
		protected InvalidFilterCriteriaException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
