using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting
{
	// Token: 0x02000750 RID: 1872
	[ComVisible(true)]
	[Serializable]
	public class RemotingTimeoutException : RemotingException
	{
		// Token: 0x060042F9 RID: 17145 RVA: 0x000E58BA File Offset: 0x000E48BA
		public RemotingTimeoutException()
			: base(RemotingTimeoutException._nullMessage)
		{
		}

		// Token: 0x060042FA RID: 17146 RVA: 0x000E58C7 File Offset: 0x000E48C7
		public RemotingTimeoutException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233077);
		}

		// Token: 0x060042FB RID: 17147 RVA: 0x000E58DB File Offset: 0x000E48DB
		public RemotingTimeoutException(string message, Exception InnerException)
			: base(message, InnerException)
		{
			base.SetErrorCode(-2146233077);
		}

		// Token: 0x060042FC RID: 17148 RVA: 0x000E58F0 File Offset: 0x000E48F0
		internal RemotingTimeoutException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x04002186 RID: 8582
		private static string _nullMessage = Environment.GetResourceString("Remoting_Default");
	}
}
