using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000C1 RID: 193
	[ComVisible(true)]
	[Serializable]
	public sealed class IndexOutOfRangeException : SystemException
	{
		// Token: 0x06000AFF RID: 2815 RVA: 0x0002255F File Offset: 0x0002155F
		public IndexOutOfRangeException()
			: base(Environment.GetResourceString("Arg_IndexOutOfRangeException"))
		{
			base.SetErrorCode(-2146233080);
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x0002257C File Offset: 0x0002157C
		public IndexOutOfRangeException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233080);
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x00022590 File Offset: 0x00021590
		public IndexOutOfRangeException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146233080);
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x000225A5 File Offset: 0x000215A5
		internal IndexOutOfRangeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
