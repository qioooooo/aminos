using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x020002C5 RID: 709
	[ComVisible(true)]
	[Serializable]
	public sealed class AmbiguousMatchException : SystemException
	{
		// Token: 0x06001BB8 RID: 7096 RVA: 0x00048445 File Offset: 0x00047445
		public AmbiguousMatchException()
			: base(Environment.GetResourceString("Arg_AmbiguousMatchException"))
		{
			base.SetErrorCode(-2147475171);
		}

		// Token: 0x06001BB9 RID: 7097 RVA: 0x00048462 File Offset: 0x00047462
		public AmbiguousMatchException(string message)
			: base(message)
		{
			base.SetErrorCode(-2147475171);
		}

		// Token: 0x06001BBA RID: 7098 RVA: 0x00048476 File Offset: 0x00047476
		public AmbiguousMatchException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2147475171);
		}

		// Token: 0x06001BBB RID: 7099 RVA: 0x0004848B File Offset: 0x0004748B
		internal AmbiguousMatchException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
