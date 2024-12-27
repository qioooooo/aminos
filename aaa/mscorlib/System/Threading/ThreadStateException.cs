using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Threading
{
	// Token: 0x02000166 RID: 358
	[ComVisible(true)]
	[Serializable]
	public class ThreadStateException : SystemException
	{
		// Token: 0x060013AB RID: 5035 RVA: 0x00035AF8 File Offset: 0x00034AF8
		public ThreadStateException()
			: base(Environment.GetResourceString("Arg_ThreadStateException"))
		{
			base.SetErrorCode(-2146233056);
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x00035B15 File Offset: 0x00034B15
		public ThreadStateException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233056);
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x00035B29 File Offset: 0x00034B29
		public ThreadStateException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146233056);
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x00035B3E File Offset: 0x00034B3E
		protected ThreadStateException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
