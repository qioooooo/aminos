using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000114 RID: 276
	[ComVisible(true)]
	[Serializable]
	public class TimeoutException : SystemException
	{
		// Token: 0x06001026 RID: 4134 RVA: 0x0002DEDE File Offset: 0x0002CEDE
		public TimeoutException()
			: base(Environment.GetResourceString("Arg_TimeoutException"))
		{
			base.SetErrorCode(-2146233083);
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x0002DEFB File Offset: 0x0002CEFB
		public TimeoutException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233083);
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x0002DF0F File Offset: 0x0002CF0F
		public TimeoutException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146233083);
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x0002DF24 File Offset: 0x0002CF24
		protected TimeoutException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
