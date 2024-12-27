using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x0200032C RID: 812
	[ComVisible(true)]
	[Serializable]
	public sealed class TargetParameterCountException : ApplicationException
	{
		// Token: 0x06001F82 RID: 8066 RVA: 0x0004FB8B File Offset: 0x0004EB8B
		public TargetParameterCountException()
			: base(Environment.GetResourceString("Arg_TargetParameterCountException"))
		{
			base.SetErrorCode(-2147352562);
		}

		// Token: 0x06001F83 RID: 8067 RVA: 0x0004FBA8 File Offset: 0x0004EBA8
		public TargetParameterCountException(string message)
			: base(message)
		{
			base.SetErrorCode(-2147352562);
		}

		// Token: 0x06001F84 RID: 8068 RVA: 0x0004FBBC File Offset: 0x0004EBBC
		public TargetParameterCountException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2147352562);
		}

		// Token: 0x06001F85 RID: 8069 RVA: 0x0004FBD1 File Offset: 0x0004EBD1
		internal TargetParameterCountException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
