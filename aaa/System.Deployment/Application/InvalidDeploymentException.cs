using System;
using System.Runtime.Serialization;

namespace System.Deployment.Application
{
	// Token: 0x02000050 RID: 80
	[Serializable]
	public class InvalidDeploymentException : DeploymentException
	{
		// Token: 0x06000272 RID: 626 RVA: 0x0000F8F2 File Offset: 0x0000E8F2
		public InvalidDeploymentException()
			: this(Resources.GetString("Ex_InvalidDeploymentException"))
		{
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000F904 File Offset: 0x0000E904
		public InvalidDeploymentException(string message)
			: base(message)
		{
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000F90D File Offset: 0x0000E90D
		public InvalidDeploymentException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000F917 File Offset: 0x0000E917
		internal InvalidDeploymentException(ExceptionTypes exceptionType, string message)
			: base(exceptionType, message)
		{
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000F921 File Offset: 0x0000E921
		internal InvalidDeploymentException(ExceptionTypes exceptionType, string message, Exception innerException)
			: base(exceptionType, message, innerException)
		{
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000F92C File Offset: 0x0000E92C
		protected InvalidDeploymentException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}
	}
}
