using System;
using System.Runtime.Serialization;

namespace Microsoft.JScript
{
	// Token: 0x0200010E RID: 270
	[Serializable]
	public sealed class ReturnOutOfFinally : ApplicationException
	{
		// Token: 0x06000B54 RID: 2900 RVA: 0x00056B97 File Offset: 0x00055B97
		public ReturnOutOfFinally()
		{
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x00056B9F File Offset: 0x00055B9F
		public ReturnOutOfFinally(string m)
			: base(m)
		{
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x00056BA8 File Offset: 0x00055BA8
		public ReturnOutOfFinally(string m, Exception e)
			: base(m, e)
		{
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x00056BB2 File Offset: 0x00055BB2
		private ReturnOutOfFinally(SerializationInfo s, StreamingContext c)
			: base(s, c)
		{
		}
	}
}
