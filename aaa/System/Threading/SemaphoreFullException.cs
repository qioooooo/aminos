using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Threading
{
	// Token: 0x02000218 RID: 536
	[ComVisible(false)]
	[Serializable]
	public class SemaphoreFullException : SystemException
	{
		// Token: 0x0600121E RID: 4638 RVA: 0x0003D2D4 File Offset: 0x0003C2D4
		public SemaphoreFullException()
			: base(SR.GetString("Threading_SemaphoreFullException"))
		{
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x0003D2E6 File Offset: 0x0003C2E6
		public SemaphoreFullException(string message)
			: base(message)
		{
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x0003D2EF File Offset: 0x0003C2EF
		public SemaphoreFullException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x0003D2F9 File Offset: 0x0003C2F9
		protected SemaphoreFullException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
