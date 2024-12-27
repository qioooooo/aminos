using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x0200032A RID: 810
	[ComVisible(true)]
	[Serializable]
	public class TargetException : ApplicationException
	{
		// Token: 0x06001F79 RID: 8057 RVA: 0x0004FAD7 File Offset: 0x0004EAD7
		public TargetException()
		{
			base.SetErrorCode(-2146232829);
		}

		// Token: 0x06001F7A RID: 8058 RVA: 0x0004FAEA File Offset: 0x0004EAEA
		public TargetException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146232829);
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x0004FAFE File Offset: 0x0004EAFE
		public TargetException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146232829);
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x0004FB13 File Offset: 0x0004EB13
		protected TargetException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
