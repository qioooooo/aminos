using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net.Mail
{
	// Token: 0x020006CE RID: 1742
	[Serializable]
	public class SmtpFailedRecipientsException : SmtpFailedRecipientException, ISerializable
	{
		// Token: 0x060035D2 RID: 13778 RVA: 0x000E5958 File Offset: 0x000E4958
		public SmtpFailedRecipientsException()
		{
			this.innerExceptions = new SmtpFailedRecipientException[0];
		}

		// Token: 0x060035D3 RID: 13779 RVA: 0x000E596C File Offset: 0x000E496C
		public SmtpFailedRecipientsException(string message)
			: base(message)
		{
			this.innerExceptions = new SmtpFailedRecipientException[0];
		}

		// Token: 0x060035D4 RID: 13780 RVA: 0x000E5984 File Offset: 0x000E4984
		public SmtpFailedRecipientsException(string message, Exception innerException)
			: base(message, innerException)
		{
			SmtpFailedRecipientException ex = innerException as SmtpFailedRecipientException;
			this.innerExceptions = ((ex == null) ? new SmtpFailedRecipientException[0] : new SmtpFailedRecipientException[] { ex });
		}

		// Token: 0x060035D5 RID: 13781 RVA: 0x000E59BD File Offset: 0x000E49BD
		protected SmtpFailedRecipientsException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.innerExceptions = (SmtpFailedRecipientException[])info.GetValue("innerExceptions", typeof(SmtpFailedRecipientException[]));
		}

		// Token: 0x060035D6 RID: 13782 RVA: 0x000E59E8 File Offset: 0x000E49E8
		public SmtpFailedRecipientsException(string message, SmtpFailedRecipientException[] innerExceptions)
			: base(message, (innerExceptions != null && innerExceptions.Length > 0) ? innerExceptions[0].FailedRecipient : null, (innerExceptions != null && innerExceptions.Length > 0) ? innerExceptions[0] : null)
		{
			if (innerExceptions == null)
			{
				throw new ArgumentNullException("innerExceptions");
			}
			this.innerExceptions = ((innerExceptions == null) ? new SmtpFailedRecipientException[0] : innerExceptions);
		}

		// Token: 0x060035D7 RID: 13783 RVA: 0x000E5A40 File Offset: 0x000E4A40
		internal SmtpFailedRecipientsException(ArrayList innerExceptions, bool allFailed)
			: base(allFailed ? SR.GetString("SmtpAllRecipientsFailed") : SR.GetString("SmtpRecipientFailed"), (innerExceptions != null && innerExceptions.Count > 0) ? ((SmtpFailedRecipientException)innerExceptions[0]).FailedRecipient : null, (innerExceptions != null && innerExceptions.Count > 0) ? ((SmtpFailedRecipientException)innerExceptions[0]) : null)
		{
			if (innerExceptions == null)
			{
				throw new ArgumentNullException("innerExceptions");
			}
			this.innerExceptions = new SmtpFailedRecipientException[innerExceptions.Count];
			int num = 0;
			foreach (object obj in innerExceptions)
			{
				SmtpFailedRecipientException ex = (SmtpFailedRecipientException)obj;
				this.innerExceptions[num++] = ex;
			}
		}

		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x060035D8 RID: 13784 RVA: 0x000E5B18 File Offset: 0x000E4B18
		public SmtpFailedRecipientException[] InnerExceptions
		{
			get
			{
				return this.innerExceptions;
			}
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x000E5B20 File Offset: 0x000E4B20
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x000E5B2A File Offset: 0x000E4B2A
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
			serializationInfo.AddValue("innerExceptions", this.innerExceptions, typeof(SmtpFailedRecipientException[]));
		}

		// Token: 0x0400310B RID: 12555
		private SmtpFailedRecipientException[] innerExceptions;
	}
}
