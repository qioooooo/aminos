using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000E5 RID: 229
	[ComVisible(true)]
	[Serializable]
	public class OverflowException : ArithmeticException
	{
		// Token: 0x06000C78 RID: 3192 RVA: 0x000257DA File Offset: 0x000247DA
		public OverflowException()
			: base(Environment.GetResourceString("Arg_OverflowException"))
		{
			base.SetErrorCode(-2146233066);
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x000257F7 File Offset: 0x000247F7
		public OverflowException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233066);
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x0002580B File Offset: 0x0002480B
		public OverflowException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146233066);
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x00025820 File Offset: 0x00024820
		protected OverflowException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
