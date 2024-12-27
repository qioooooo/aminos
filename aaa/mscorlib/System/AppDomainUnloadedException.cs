using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000063 RID: 99
	[ComVisible(true)]
	[Serializable]
	public class AppDomainUnloadedException : SystemException
	{
		// Token: 0x060005FB RID: 1531 RVA: 0x00014D8D File Offset: 0x00013D8D
		public AppDomainUnloadedException()
			: base(Environment.GetResourceString("Arg_AppDomainUnloadedException"))
		{
			base.SetErrorCode(-2146234348);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00014DAA File Offset: 0x00013DAA
		public AppDomainUnloadedException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146234348);
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00014DBE File Offset: 0x00013DBE
		public AppDomainUnloadedException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146234348);
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00014DD3 File Offset: 0x00013DD3
		protected AppDomainUnloadedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
