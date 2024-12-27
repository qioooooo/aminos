using System;
using System.Runtime.Serialization;

namespace System.Data
{
	// Token: 0x02000104 RID: 260
	[Serializable]
	public class StrongTypingException : DataException
	{
		// Token: 0x06000F50 RID: 3920 RVA: 0x00215AFC File Offset: 0x00214EFC
		protected StrongTypingException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x00215B14 File Offset: 0x00214F14
		public StrongTypingException()
		{
			base.HResult = -2146232021;
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x00215B34 File Offset: 0x00214F34
		public StrongTypingException(string message)
			: base(message)
		{
			base.HResult = -2146232021;
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x00215B54 File Offset: 0x00214F54
		public StrongTypingException(string s, Exception innerException)
			: base(s, innerException)
		{
			base.HResult = -2146232021;
		}
	}
}
