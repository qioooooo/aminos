using System;
using System.Runtime.Serialization;

namespace System.Deployment.Application
{
	// Token: 0x02000052 RID: 82
	[Serializable]
	public class TrustNotGrantedException : DeploymentException
	{
		// Token: 0x0600027E RID: 638 RVA: 0x0000F97A File Offset: 0x0000E97A
		public TrustNotGrantedException()
			: this(Resources.GetString("Ex_TrustNotGrantedException"))
		{
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000F98C File Offset: 0x0000E98C
		public TrustNotGrantedException(string message)
			: base(message)
		{
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000F995 File Offset: 0x0000E995
		public TrustNotGrantedException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000F99F File Offset: 0x0000E99F
		internal TrustNotGrantedException(ExceptionTypes exceptionType, string message)
			: base(exceptionType, message)
		{
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000F9A9 File Offset: 0x0000E9A9
		internal TrustNotGrantedException(ExceptionTypes exceptionType, string message, Exception innerException)
			: base(exceptionType, message, innerException)
		{
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000F9B4 File Offset: 0x0000E9B4
		protected TrustNotGrantedException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}
	}
}
