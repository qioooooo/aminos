using System;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000501 RID: 1281
	[ComVisible(true)]
	[Serializable]
	public class MarshalDirectiveException : SystemException
	{
		// Token: 0x0600325D RID: 12893 RVA: 0x000AB068 File Offset: 0x000AA068
		public MarshalDirectiveException()
			: base(Environment.GetResourceString("Arg_MarshalDirectiveException"))
		{
			base.SetErrorCode(-2146233035);
		}

		// Token: 0x0600325E RID: 12894 RVA: 0x000AB085 File Offset: 0x000AA085
		public MarshalDirectiveException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233035);
		}

		// Token: 0x0600325F RID: 12895 RVA: 0x000AB099 File Offset: 0x000AA099
		public MarshalDirectiveException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233035);
		}

		// Token: 0x06003260 RID: 12896 RVA: 0x000AB0AE File Offset: 0x000AA0AE
		protected MarshalDirectiveException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
