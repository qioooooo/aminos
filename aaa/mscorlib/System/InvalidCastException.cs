using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000C8 RID: 200
	[ComVisible(true)]
	[Serializable]
	public class InvalidCastException : SystemException
	{
		// Token: 0x06000B80 RID: 2944 RVA: 0x0002308D File Offset: 0x0002208D
		public InvalidCastException()
			: base(Environment.GetResourceString("Arg_InvalidCastException"))
		{
			base.SetErrorCode(-2147467262);
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x000230AA File Offset: 0x000220AA
		public InvalidCastException(string message)
			: base(message)
		{
			base.SetErrorCode(-2147467262);
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x000230BE File Offset: 0x000220BE
		public InvalidCastException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2147467262);
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x000230D3 File Offset: 0x000220D3
		protected InvalidCastException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x000230DD File Offset: 0x000220DD
		public InvalidCastException(string message, int errorCode)
			: base(message)
		{
			base.SetErrorCode(errorCode);
		}
	}
}
