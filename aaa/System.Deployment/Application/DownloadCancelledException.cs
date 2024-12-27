using System;
using System.Runtime.Serialization;

namespace System.Deployment.Application
{
	// Token: 0x02000054 RID: 84
	[Serializable]
	internal class DownloadCancelledException : DeploymentDownloadException
	{
		// Token: 0x0600028D RID: 653 RVA: 0x0000FA55 File Offset: 0x0000EA55
		public DownloadCancelledException()
			: this(Resources.GetString("Ex_DownloadCancelledException"))
		{
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000FA67 File Offset: 0x0000EA67
		public DownloadCancelledException(string message)
			: base(message)
		{
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000FA70 File Offset: 0x0000EA70
		public DownloadCancelledException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000FA7A File Offset: 0x0000EA7A
		protected DownloadCancelledException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}
	}
}
