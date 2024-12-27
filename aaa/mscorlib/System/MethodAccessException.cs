using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000D3 RID: 211
	[ComVisible(true)]
	[Serializable]
	public class MethodAccessException : MemberAccessException
	{
		// Token: 0x06000BFA RID: 3066 RVA: 0x00023C95 File Offset: 0x00022C95
		public MethodAccessException()
			: base(Environment.GetResourceString("Arg_MethodAccessException"))
		{
			base.SetErrorCode(-2146233072);
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x00023CB2 File Offset: 0x00022CB2
		public MethodAccessException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233072);
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x00023CC6 File Offset: 0x00022CC6
		public MethodAccessException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233072);
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x00023CDB File Offset: 0x00022CDB
		protected MethodAccessException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
