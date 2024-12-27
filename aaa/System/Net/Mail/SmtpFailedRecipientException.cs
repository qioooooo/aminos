using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net.Mail
{
	// Token: 0x020006CD RID: 1741
	[Serializable]
	public class SmtpFailedRecipientException : SmtpException, ISerializable
	{
		// Token: 0x060035C8 RID: 13768 RVA: 0x000E58B8 File Offset: 0x000E48B8
		public SmtpFailedRecipientException()
		{
		}

		// Token: 0x060035C9 RID: 13769 RVA: 0x000E58C0 File Offset: 0x000E48C0
		public SmtpFailedRecipientException(string message)
			: base(message)
		{
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x000E58C9 File Offset: 0x000E48C9
		public SmtpFailedRecipientException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x000E58D3 File Offset: 0x000E48D3
		protected SmtpFailedRecipientException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.failedRecipient = info.GetString("failedRecipient");
		}

		// Token: 0x060035CC RID: 13772 RVA: 0x000E58EE File Offset: 0x000E48EE
		public SmtpFailedRecipientException(SmtpStatusCode statusCode, string failedRecipient)
			: base(statusCode)
		{
			this.failedRecipient = failedRecipient;
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x000E58FE File Offset: 0x000E48FE
		public SmtpFailedRecipientException(SmtpStatusCode statusCode, string failedRecipient, string serverResponse)
			: base(statusCode, serverResponse, true)
		{
			this.failedRecipient = failedRecipient;
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x000E5910 File Offset: 0x000E4910
		public SmtpFailedRecipientException(string message, string failedRecipient, Exception innerException)
			: base(message, innerException)
		{
			this.failedRecipient = failedRecipient;
		}

		// Token: 0x17000C7A RID: 3194
		// (get) Token: 0x060035CF RID: 13775 RVA: 0x000E5921 File Offset: 0x000E4921
		public string FailedRecipient
		{
			get
			{
				return this.failedRecipient;
			}
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x000E5929 File Offset: 0x000E4929
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x060035D1 RID: 13777 RVA: 0x000E5933 File Offset: 0x000E4933
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
			serializationInfo.AddValue("failedRecipient", this.failedRecipient, typeof(string));
		}

		// Token: 0x04003109 RID: 12553
		private string failedRecipient;

		// Token: 0x0400310A RID: 12554
		internal bool fatal;
	}
}
