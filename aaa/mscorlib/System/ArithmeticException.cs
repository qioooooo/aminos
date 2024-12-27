using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000071 RID: 113
	[ComVisible(true)]
	[Serializable]
	public class ArithmeticException : SystemException
	{
		// Token: 0x0600066C RID: 1644 RVA: 0x00015CD2 File Offset: 0x00014CD2
		public ArithmeticException()
			: base(Environment.GetResourceString("Arg_ArithmeticException"))
		{
			base.SetErrorCode(-2147024362);
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00015CEF File Offset: 0x00014CEF
		public ArithmeticException(string message)
			: base(message)
		{
			base.SetErrorCode(-2147024362);
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00015D03 File Offset: 0x00014D03
		public ArithmeticException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2147024362);
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x00015D18 File Offset: 0x00014D18
		protected ArithmeticException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
