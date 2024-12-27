using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Collections.Generic
{
	// Token: 0x0200028C RID: 652
	[ComVisible(true)]
	[Serializable]
	public class KeyNotFoundException : SystemException, ISerializable
	{
		// Token: 0x060019E9 RID: 6633 RVA: 0x00044104 File Offset: 0x00043104
		public KeyNotFoundException()
			: base(Environment.GetResourceString("Arg_KeyNotFound"))
		{
			base.SetErrorCode(-2146232969);
		}

		// Token: 0x060019EA RID: 6634 RVA: 0x00044121 File Offset: 0x00043121
		public KeyNotFoundException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146232969);
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x00044135 File Offset: 0x00043135
		public KeyNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146232969);
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x0004414A File Offset: 0x0004314A
		protected KeyNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
