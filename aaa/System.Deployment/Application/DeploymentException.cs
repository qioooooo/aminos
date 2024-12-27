using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Deployment.Application
{
	// Token: 0x0200004F RID: 79
	[Serializable]
	public class DeploymentException : SystemException
	{
		// Token: 0x0600026A RID: 618 RVA: 0x0000F84C File Offset: 0x0000E84C
		public DeploymentException()
			: this(Resources.GetString("Ex_DeploymentException"))
		{
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000F85E File Offset: 0x0000E85E
		public DeploymentException(string message)
			: base(message)
		{
			this._type = ExceptionTypes.Unknown;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000F86E File Offset: 0x0000E86E
		public DeploymentException(string message, Exception innerException)
			: base(message, innerException)
		{
			this._type = ExceptionTypes.Unknown;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000F87F File Offset: 0x0000E87F
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("_type", this._type);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000F89F File Offset: 0x0000E89F
		internal DeploymentException(ExceptionTypes exceptionType, string message)
			: base(message)
		{
			this._type = exceptionType;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000F8AF File Offset: 0x0000E8AF
		internal DeploymentException(ExceptionTypes exceptionType, string message, Exception innerException)
			: base(message, innerException)
		{
			this._type = exceptionType;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000F8C0 File Offset: 0x0000E8C0
		protected DeploymentException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
			this._type = (ExceptionTypes)serializationInfo.GetValue("_type", typeof(ExceptionTypes));
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000F8EA File Offset: 0x0000E8EA
		internal ExceptionTypes SubType
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x04000205 RID: 517
		private ExceptionTypes _type;
	}
}
