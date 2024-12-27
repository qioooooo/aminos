using System;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004FC RID: 1276
	[ComVisible(true)]
	[Serializable]
	public class InvalidOleVariantTypeException : SystemException
	{
		// Token: 0x06003194 RID: 12692 RVA: 0x000A9E77 File Offset: 0x000A8E77
		public InvalidOleVariantTypeException()
			: base(Environment.GetResourceString("Arg_InvalidOleVariantTypeException"))
		{
			base.SetErrorCode(-2146233039);
		}

		// Token: 0x06003195 RID: 12693 RVA: 0x000A9E94 File Offset: 0x000A8E94
		public InvalidOleVariantTypeException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233039);
		}

		// Token: 0x06003196 RID: 12694 RVA: 0x000A9EA8 File Offset: 0x000A8EA8
		public InvalidOleVariantTypeException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233039);
		}

		// Token: 0x06003197 RID: 12695 RVA: 0x000A9EBD File Offset: 0x000A8EBD
		protected InvalidOleVariantTypeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
