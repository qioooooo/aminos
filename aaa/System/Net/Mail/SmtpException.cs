using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net.Mail
{
	// Token: 0x020006CC RID: 1740
	[Serializable]
	public class SmtpException : Exception, ISerializable
	{
		// Token: 0x060035BA RID: 13754 RVA: 0x000E5578 File Offset: 0x000E4578
		private static string GetMessageForStatus(SmtpStatusCode statusCode, string serverResponse)
		{
			return SmtpException.GetMessageForStatus(statusCode) + " " + SR.GetString("MailServerResponse", new object[] { serverResponse });
		}

		// Token: 0x060035BB RID: 13755 RVA: 0x000E55AC File Offset: 0x000E45AC
		private static string GetMessageForStatus(SmtpStatusCode statusCode)
		{
			if (statusCode <= SmtpStatusCode.StartMailInput)
			{
				if (statusCode <= SmtpStatusCode.HelpMessage)
				{
					if (statusCode == SmtpStatusCode.SystemStatus)
					{
						return SR.GetString("SmtpSystemStatus");
					}
					if (statusCode == SmtpStatusCode.HelpMessage)
					{
						return SR.GetString("SmtpHelpMessage");
					}
				}
				else
				{
					switch (statusCode)
					{
					case SmtpStatusCode.ServiceReady:
						return SR.GetString("SmtpServiceReady");
					case SmtpStatusCode.ServiceClosingTransmissionChannel:
						return SR.GetString("SmtpServiceClosingTransmissionChannel");
					default:
						switch (statusCode)
						{
						case SmtpStatusCode.Ok:
							return SR.GetString("SmtpOK");
						case SmtpStatusCode.UserNotLocalWillForward:
							return SR.GetString("SmtpUserNotLocalWillForward");
						default:
							if (statusCode == SmtpStatusCode.StartMailInput)
							{
								return SR.GetString("SmtpStartMailInput");
							}
							break;
						}
						break;
					}
				}
			}
			else if (statusCode <= SmtpStatusCode.ClientNotPermitted)
			{
				if (statusCode == SmtpStatusCode.ServiceNotAvailable)
				{
					return SR.GetString("SmtpServiceNotAvailable");
				}
				switch (statusCode)
				{
				case SmtpStatusCode.MailboxBusy:
					return SR.GetString("SmtpMailboxBusy");
				case SmtpStatusCode.LocalErrorInProcessing:
					return SR.GetString("SmtpLocalErrorInProcessing");
				case SmtpStatusCode.InsufficientStorage:
					return SR.GetString("SmtpInsufficientStorage");
				case SmtpStatusCode.ClientNotPermitted:
					return SR.GetString("SmtpClientNotPermitted");
				}
			}
			else
			{
				switch (statusCode)
				{
				case SmtpStatusCode.CommandUnrecognized:
					break;
				case SmtpStatusCode.SyntaxError:
					return SR.GetString("SmtpSyntaxError");
				case SmtpStatusCode.CommandNotImplemented:
					return SR.GetString("SmtpCommandNotImplemented");
				case SmtpStatusCode.BadCommandSequence:
					return SR.GetString("SmtpBadCommandSequence");
				case SmtpStatusCode.CommandParameterNotImplemented:
					return SR.GetString("SmtpCommandParameterNotImplemented");
				default:
					if (statusCode == SmtpStatusCode.MustIssueStartTlsFirst)
					{
						return SR.GetString("SmtpMustIssueStartTlsFirst");
					}
					switch (statusCode)
					{
					case SmtpStatusCode.MailboxUnavailable:
						return SR.GetString("SmtpMailboxUnavailable");
					case SmtpStatusCode.UserNotLocalTryAlternatePath:
						return SR.GetString("SmtpUserNotLocalTryAlternatePath");
					case SmtpStatusCode.ExceededStorageAllocation:
						return SR.GetString("SmtpExceededStorageAllocation");
					case SmtpStatusCode.MailboxNameNotAllowed:
						return SR.GetString("SmtpMailboxNameNotAllowed");
					case SmtpStatusCode.TransactionFailed:
						return SR.GetString("SmtpTransactionFailed");
					}
					break;
				}
			}
			return SR.GetString("SmtpCommandUnrecognized");
		}

		// Token: 0x060035BC RID: 13756 RVA: 0x000E579A File Offset: 0x000E479A
		public SmtpException(SmtpStatusCode statusCode)
			: base(SmtpException.GetMessageForStatus(statusCode))
		{
			this.statusCode = statusCode;
		}

		// Token: 0x060035BD RID: 13757 RVA: 0x000E57B6 File Offset: 0x000E47B6
		public SmtpException(SmtpStatusCode statusCode, string message)
			: base(message)
		{
			this.statusCode = statusCode;
		}

		// Token: 0x060035BE RID: 13758 RVA: 0x000E57CD File Offset: 0x000E47CD
		public SmtpException()
			: this(SmtpStatusCode.GeneralFailure)
		{
		}

		// Token: 0x060035BF RID: 13759 RVA: 0x000E57D6 File Offset: 0x000E47D6
		public SmtpException(string message)
			: base(message)
		{
		}

		// Token: 0x060035C0 RID: 13760 RVA: 0x000E57E6 File Offset: 0x000E47E6
		public SmtpException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060035C1 RID: 13761 RVA: 0x000E57F7 File Offset: 0x000E47F7
		protected SmtpException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
			this.statusCode = (SmtpStatusCode)serializationInfo.GetInt32("Status");
		}

		// Token: 0x060035C2 RID: 13762 RVA: 0x000E5819 File Offset: 0x000E4819
		internal SmtpException(SmtpStatusCode statusCode, string serverMessage, bool serverResponse)
			: base(SmtpException.GetMessageForStatus(statusCode, serverMessage))
		{
			this.statusCode = statusCode;
		}

		// Token: 0x060035C3 RID: 13763 RVA: 0x000E5838 File Offset: 0x000E4838
		internal SmtpException(string message, string serverResponse)
			: base(message + " " + SR.GetString("MailServerResponse", new object[] { serverResponse }))
		{
		}

		// Token: 0x060035C4 RID: 13764 RVA: 0x000E5873 File Offset: 0x000E4873
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x060035C5 RID: 13765 RVA: 0x000E587D File Offset: 0x000E487D
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
			serializationInfo.AddValue("Status", (int)this.statusCode, typeof(int));
		}

		// Token: 0x17000C79 RID: 3193
		// (get) Token: 0x060035C6 RID: 13766 RVA: 0x000E58A7 File Offset: 0x000E48A7
		// (set) Token: 0x060035C7 RID: 13767 RVA: 0x000E58AF File Offset: 0x000E48AF
		public SmtpStatusCode StatusCode
		{
			get
			{
				return this.statusCode;
			}
			set
			{
				this.statusCode = value;
			}
		}

		// Token: 0x04003108 RID: 12552
		private SmtpStatusCode statusCode = SmtpStatusCode.GeneralFailure;
	}
}
