using System;
using System.Runtime.Serialization;

namespace System.Deployment.Application
{
	// Token: 0x02000051 RID: 81
	[Serializable]
	public class DeploymentDownloadException : DeploymentException
	{
		// Token: 0x06000278 RID: 632 RVA: 0x0000F936 File Offset: 0x0000E936
		public DeploymentDownloadException()
			: this(Resources.GetString("Ex_DeploymentDownloadException"))
		{
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000F948 File Offset: 0x0000E948
		public DeploymentDownloadException(string message)
			: base(message)
		{
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000F951 File Offset: 0x0000E951
		public DeploymentDownloadException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000F95B File Offset: 0x0000E95B
		internal DeploymentDownloadException(ExceptionTypes exceptionType, string message)
			: base(exceptionType, message)
		{
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000F965 File Offset: 0x0000E965
		internal DeploymentDownloadException(ExceptionTypes exceptionType, string message, Exception innerException)
			: base(exceptionType, message, innerException)
		{
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000F970 File Offset: 0x0000E970
		protected DeploymentDownloadException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}
	}
}
