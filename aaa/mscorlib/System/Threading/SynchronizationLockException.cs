using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Threading
{
	// Token: 0x02000152 RID: 338
	[ComVisible(true)]
	[Serializable]
	public class SynchronizationLockException : SystemException
	{
		// Token: 0x060012AE RID: 4782 RVA: 0x0003439F File Offset: 0x0003339F
		public SynchronizationLockException()
			: base(Environment.GetResourceString("Arg_SynchronizationLockException"))
		{
			base.SetErrorCode(-2146233064);
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x000343BC File Offset: 0x000333BC
		public SynchronizationLockException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233064);
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x000343D0 File Offset: 0x000333D0
		public SynchronizationLockException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146233064);
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x000343E5 File Offset: 0x000333E5
		protected SynchronizationLockException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
