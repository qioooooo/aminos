using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000CA RID: 202
	[ComVisible(true)]
	[Serializable]
	public sealed class InvalidProgramException : SystemException
	{
		// Token: 0x06000B89 RID: 2953 RVA: 0x0002313D File Offset: 0x0002213D
		public InvalidProgramException()
			: base(Environment.GetResourceString("InvalidProgram_Default"))
		{
			base.SetErrorCode(-2146233030);
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0002315A File Offset: 0x0002215A
		public InvalidProgramException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233030);
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x0002316E File Offset: 0x0002216E
		public InvalidProgramException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233030);
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x00023183 File Offset: 0x00022183
		internal InvalidProgramException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
