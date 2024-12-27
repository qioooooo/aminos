using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Threading
{
	// Token: 0x0200016E RID: 366
	[ComVisible(false)]
	[Serializable]
	public class WaitHandleCannotBeOpenedException : ApplicationException
	{
		// Token: 0x060013D0 RID: 5072 RVA: 0x0003615B File Offset: 0x0003515B
		public WaitHandleCannotBeOpenedException()
			: base(Environment.GetResourceString("Threading.WaitHandleCannotBeOpenedException"))
		{
			base.SetErrorCode(-2146233044);
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x00036178 File Offset: 0x00035178
		public WaitHandleCannotBeOpenedException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233044);
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x0003618C File Offset: 0x0003518C
		public WaitHandleCannotBeOpenedException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146233044);
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x000361A1 File Offset: 0x000351A1
		protected WaitHandleCannotBeOpenedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
