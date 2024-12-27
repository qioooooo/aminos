using System;
using System.Runtime.Serialization;

namespace System.Configuration.Install
{
	// Token: 0x02000015 RID: 21
	[Serializable]
	public class InstallException : SystemException
	{
		// Token: 0x06000080 RID: 128 RVA: 0x00004245 File Offset: 0x00003245
		public InstallException()
		{
			base.HResult = -2146232057;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004258 File Offset: 0x00003258
		public InstallException(string message)
			: base(message)
		{
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004261 File Offset: 0x00003261
		public InstallException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0000426B File Offset: 0x0000326B
		protected InstallException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
