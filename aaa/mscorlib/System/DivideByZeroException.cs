using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000A8 RID: 168
	[ComVisible(true)]
	[Serializable]
	public class DivideByZeroException : ArithmeticException
	{
		// Token: 0x06000A21 RID: 2593 RVA: 0x0001F418 File Offset: 0x0001E418
		public DivideByZeroException()
			: base(Environment.GetResourceString("Arg_DivideByZero"))
		{
			base.SetErrorCode(-2147352558);
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x0001F435 File Offset: 0x0001E435
		public DivideByZeroException(string message)
			: base(message)
		{
			base.SetErrorCode(-2147352558);
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x0001F449 File Offset: 0x0001E449
		public DivideByZeroException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2147352558);
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x0001F45E File Offset: 0x0001E45E
		protected DivideByZeroException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
