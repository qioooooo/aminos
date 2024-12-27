using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting
{
	// Token: 0x0200074E RID: 1870
	[ComVisible(true)]
	[Serializable]
	public class RemotingException : SystemException
	{
		// Token: 0x060042EF RID: 17135 RVA: 0x000E5802 File Offset: 0x000E4802
		public RemotingException()
			: base(RemotingException._nullMessage)
		{
			base.SetErrorCode(-2146233077);
		}

		// Token: 0x060042F0 RID: 17136 RVA: 0x000E581A File Offset: 0x000E481A
		public RemotingException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233077);
		}

		// Token: 0x060042F1 RID: 17137 RVA: 0x000E582E File Offset: 0x000E482E
		public RemotingException(string message, Exception InnerException)
			: base(message, InnerException)
		{
			base.SetErrorCode(-2146233077);
		}

		// Token: 0x060042F2 RID: 17138 RVA: 0x000E5843 File Offset: 0x000E4843
		protected RemotingException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x04002184 RID: 8580
		private static string _nullMessage = Environment.GetResourceString("Remoting_Default");
	}
}
