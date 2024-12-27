using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x020002DF RID: 735
	[ComVisible(true)]
	[Serializable]
	public class CustomAttributeFormatException : FormatException
	{
		// Token: 0x06001D01 RID: 7425 RVA: 0x0004A1C0 File Offset: 0x000491C0
		public CustomAttributeFormatException()
			: base(Environment.GetResourceString("Arg_CustomAttributeFormatException"))
		{
			base.SetErrorCode(-2146232827);
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x0004A1DD File Offset: 0x000491DD
		public CustomAttributeFormatException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146232827);
		}

		// Token: 0x06001D03 RID: 7427 RVA: 0x0004A1F1 File Offset: 0x000491F1
		public CustomAttributeFormatException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146232827);
		}

		// Token: 0x06001D04 RID: 7428 RVA: 0x0004A206 File Offset: 0x00049206
		protected CustomAttributeFormatException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
