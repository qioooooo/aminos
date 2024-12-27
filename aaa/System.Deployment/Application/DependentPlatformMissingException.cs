using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Deployment.Application
{
	// Token: 0x02000053 RID: 83
	[Serializable]
	public class DependentPlatformMissingException : DeploymentException
	{
		// Token: 0x06000284 RID: 644 RVA: 0x0000F9BE File Offset: 0x0000E9BE
		public DependentPlatformMissingException()
			: this(Resources.GetString("Ex_DependentPlatformMissingException"))
		{
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000F9D0 File Offset: 0x0000E9D0
		public DependentPlatformMissingException(string message)
			: base(message)
		{
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000F9D9 File Offset: 0x0000E9D9
		public DependentPlatformMissingException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000F9E3 File Offset: 0x0000E9E3
		protected DependentPlatformMissingException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
			this._supportUrl = (Uri)serializationInfo.GetValue("_supportUrl", typeof(Uri));
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000FA0D File Offset: 0x0000EA0D
		public DependentPlatformMissingException(string message, Uri supportUrl)
			: base(message)
		{
			this._supportUrl = supportUrl;
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000FA1D File Offset: 0x0000EA1D
		internal DependentPlatformMissingException(ExceptionTypes exceptionType, string message)
			: base(exceptionType, message)
		{
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000FA27 File Offset: 0x0000EA27
		internal DependentPlatformMissingException(ExceptionTypes exceptionType, string message, Exception innerException)
			: base(exceptionType, message, innerException)
		{
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000FA32 File Offset: 0x0000EA32
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("_supportUrl", this._supportUrl);
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600028C RID: 652 RVA: 0x0000FA4D File Offset: 0x0000EA4D
		internal Uri SupportUrl
		{
			get
			{
				return this._supportUrl;
			}
		}

		// Token: 0x04000206 RID: 518
		private Uri _supportUrl;
	}
}
