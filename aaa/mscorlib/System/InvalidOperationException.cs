using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000C9 RID: 201
	[ComVisible(true)]
	[Serializable]
	public class InvalidOperationException : SystemException
	{
		// Token: 0x06000B85 RID: 2949 RVA: 0x000230ED File Offset: 0x000220ED
		public InvalidOperationException()
			: base(Environment.GetResourceString("Arg_InvalidOperationException"))
		{
			base.SetErrorCode(-2146233079);
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x0002310A File Offset: 0x0002210A
		public InvalidOperationException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233079);
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x0002311E File Offset: 0x0002211E
		public InvalidOperationException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146233079);
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x00023133 File Offset: 0x00022133
		protected InvalidOperationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
