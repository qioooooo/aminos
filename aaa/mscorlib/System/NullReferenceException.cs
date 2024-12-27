using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000DD RID: 221
	[ComVisible(true)]
	[Serializable]
	public class NullReferenceException : SystemException
	{
		// Token: 0x06000C2D RID: 3117 RVA: 0x000242DA File Offset: 0x000232DA
		public NullReferenceException()
			: base(Environment.GetResourceString("Arg_NullReferenceException"))
		{
			base.SetErrorCode(-2147467261);
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x000242F7 File Offset: 0x000232F7
		public NullReferenceException(string message)
			: base(message)
		{
			base.SetErrorCode(-2147467261);
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x0002430B File Offset: 0x0002330B
		public NullReferenceException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2147467261);
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x00024320 File Offset: 0x00023320
		protected NullReferenceException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
