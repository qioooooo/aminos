using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000B7 RID: 183
	[ComVisible(true)]
	[Serializable]
	public class FieldAccessException : MemberAccessException
	{
		// Token: 0x06000AAD RID: 2733 RVA: 0x00020D8C File Offset: 0x0001FD8C
		public FieldAccessException()
			: base(Environment.GetResourceString("Arg_FieldAccessException"))
		{
			base.SetErrorCode(-2146233081);
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x00020DA9 File Offset: 0x0001FDA9
		public FieldAccessException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233081);
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x00020DBD File Offset: 0x0001FDBD
		public FieldAccessException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233081);
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x00020DD2 File Offset: 0x0001FDD2
		protected FieldAccessException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
