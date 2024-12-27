using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000AE RID: 174
	[ComVisible(true)]
	[Serializable]
	public class DllNotFoundException : TypeLoadException
	{
		// Token: 0x06000A64 RID: 2660 RVA: 0x0001FBE1 File Offset: 0x0001EBE1
		public DllNotFoundException()
			: base(Environment.GetResourceString("Arg_DllNotFoundException"))
		{
			base.SetErrorCode(-2146233052);
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x0001FBFE File Offset: 0x0001EBFE
		public DllNotFoundException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233052);
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x0001FC12 File Offset: 0x0001EC12
		public DllNotFoundException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233052);
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x0001FC27 File Offset: 0x0001EC27
		protected DllNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
