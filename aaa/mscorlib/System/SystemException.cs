using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000037 RID: 55
	[ComVisible(true)]
	[Serializable]
	public class SystemException : Exception
	{
		// Token: 0x06000371 RID: 881 RVA: 0x0000E272 File Offset: 0x0000D272
		public SystemException()
			: base(Environment.GetResourceString("Arg_SystemException"))
		{
			base.SetErrorCode(-2146233087);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000E28F File Offset: 0x0000D28F
		public SystemException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233087);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000E2A3 File Offset: 0x0000D2A3
		public SystemException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146233087);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000E2B8 File Offset: 0x0000D2B8
		protected SystemException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
