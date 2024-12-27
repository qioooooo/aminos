using System;
using System.Runtime.Serialization;

namespace System.Threading
{
	// Token: 0x02000168 RID: 360
	[Serializable]
	public sealed class ThreadStartException : SystemException
	{
		// Token: 0x060013B0 RID: 5040 RVA: 0x00035B50 File Offset: 0x00034B50
		private ThreadStartException()
			: base(Environment.GetResourceString("Arg_ThreadStartException"))
		{
			base.SetErrorCode(-2146233051);
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x00035B6D File Offset: 0x00034B6D
		private ThreadStartException(Exception reason)
			: base(Environment.GetResourceString("Arg_ThreadStartException"), reason)
		{
			base.SetErrorCode(-2146233051);
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x00035B8B File Offset: 0x00034B8B
		internal ThreadStartException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
