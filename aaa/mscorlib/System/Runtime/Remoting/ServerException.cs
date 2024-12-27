using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting
{
	// Token: 0x0200074F RID: 1871
	[ComVisible(true)]
	[Serializable]
	public class ServerException : SystemException
	{
		// Token: 0x060042F4 RID: 17140 RVA: 0x000E585E File Offset: 0x000E485E
		public ServerException()
			: base(ServerException._nullMessage)
		{
			base.SetErrorCode(-2146233074);
		}

		// Token: 0x060042F5 RID: 17141 RVA: 0x000E5876 File Offset: 0x000E4876
		public ServerException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233074);
		}

		// Token: 0x060042F6 RID: 17142 RVA: 0x000E588A File Offset: 0x000E488A
		public ServerException(string message, Exception InnerException)
			: base(message, InnerException)
		{
			base.SetErrorCode(-2146233074);
		}

		// Token: 0x060042F7 RID: 17143 RVA: 0x000E589F File Offset: 0x000E489F
		internal ServerException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x04002185 RID: 8581
		private static string _nullMessage = Environment.GetResourceString("Remoting_Default");
	}
}
