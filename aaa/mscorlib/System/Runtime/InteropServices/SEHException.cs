using System;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000504 RID: 1284
	[ComVisible(true)]
	[Serializable]
	public class SEHException : ExternalException
	{
		// Token: 0x0600326B RID: 12907 RVA: 0x000AB158 File Offset: 0x000AA158
		public SEHException()
		{
			base.SetErrorCode(-2147467259);
		}

		// Token: 0x0600326C RID: 12908 RVA: 0x000AB16B File Offset: 0x000AA16B
		public SEHException(string message)
			: base(message)
		{
			base.SetErrorCode(-2147467259);
		}

		// Token: 0x0600326D RID: 12909 RVA: 0x000AB17F File Offset: 0x000AA17F
		public SEHException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2147467259);
		}

		// Token: 0x0600326E RID: 12910 RVA: 0x000AB194 File Offset: 0x000AA194
		protected SEHException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x0600326F RID: 12911 RVA: 0x000AB19E File Offset: 0x000AA19E
		public virtual bool CanResume()
		{
			return false;
		}
	}
}
