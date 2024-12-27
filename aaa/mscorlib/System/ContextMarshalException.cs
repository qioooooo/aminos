using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000096 RID: 150
	[Obsolete("ContextMarshalException is obsolete.")]
	[ComVisible(true)]
	[Serializable]
	public class ContextMarshalException : SystemException
	{
		// Token: 0x06000805 RID: 2053 RVA: 0x0001A386 File Offset: 0x00019386
		public ContextMarshalException()
			: base(Environment.GetResourceString("Arg_ContextMarshalException"))
		{
			base.SetErrorCode(-2146233084);
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0001A3A3 File Offset: 0x000193A3
		public ContextMarshalException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233084);
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0001A3B7 File Offset: 0x000193B7
		public ContextMarshalException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233084);
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0001A3CC File Offset: 0x000193CC
		protected ContextMarshalException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
