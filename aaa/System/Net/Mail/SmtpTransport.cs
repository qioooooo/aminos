using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

namespace System.Net.Mail
{
	// Token: 0x020006DC RID: 1756
	internal class SmtpTransport
	{
		// Token: 0x06003617 RID: 13847 RVA: 0x000E6E47 File Offset: 0x000E5E47
		internal SmtpTransport(SmtpClient client)
			: this(client, SmtpAuthenticationManager.GetModules())
		{
		}

		// Token: 0x06003618 RID: 13848 RVA: 0x000E6E55 File Offset: 0x000E5E55
		internal SmtpTransport(SmtpClient client, ISmtpAuthenticationModule[] authenticationModules)
		{
			this.client = client;
			if (authenticationModules == null)
			{
				throw new ArgumentNullException("authenticationModules");
			}
			this.authenticationModules = authenticationModules;
		}

		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x06003619 RID: 13849 RVA: 0x000E6E8F File Offset: 0x000E5E8F
		// (set) Token: 0x0600361A RID: 13850 RVA: 0x000E6E97 File Offset: 0x000E5E97
		internal ICredentialsByHost Credentials
		{
			get
			{
				return this.credentials;
			}
			set
			{
				this.credentials = value;
			}
		}

		// Token: 0x17000C84 RID: 3204
		// (get) Token: 0x0600361B RID: 13851 RVA: 0x000E6EA0 File Offset: 0x000E5EA0
		// (set) Token: 0x0600361C RID: 13852 RVA: 0x000E6EA8 File Offset: 0x000E5EA8
		internal bool IdentityRequired
		{
			get
			{
				return this.m_IdentityRequired;
			}
			set
			{
				this.m_IdentityRequired = value;
			}
		}

		// Token: 0x17000C85 RID: 3205
		// (get) Token: 0x0600361D RID: 13853 RVA: 0x000E6EB1 File Offset: 0x000E5EB1
		internal bool IsConnected
		{
			get
			{
				return this.connection != null && this.connection.IsConnected;
			}
		}

		// Token: 0x17000C86 RID: 3206
		// (get) Token: 0x0600361E RID: 13854 RVA: 0x000E6EC8 File Offset: 0x000E5EC8
		// (set) Token: 0x0600361F RID: 13855 RVA: 0x000E6ED0 File Offset: 0x000E5ED0
		internal int Timeout
		{
			get
			{
				return this.timeout;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.timeout = value;
			}
		}

		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x06003620 RID: 13856 RVA: 0x000E6EE8 File Offset: 0x000E5EE8
		// (set) Token: 0x06003621 RID: 13857 RVA: 0x000E6EF0 File Offset: 0x000E5EF0
		internal bool EnableSsl
		{
			get
			{
				return this.enableSsl;
			}
			set
			{
				this.enableSsl = value;
			}
		}

		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x06003622 RID: 13858 RVA: 0x000E6EF9 File Offset: 0x000E5EF9
		internal X509CertificateCollection ClientCertificates
		{
			get
			{
				if (this.clientCertificates == null)
				{
					this.clientCertificates = new X509CertificateCollection();
				}
				return this.clientCertificates;
			}
		}

		// Token: 0x06003623 RID: 13859 RVA: 0x000E6F14 File Offset: 0x000E5F14
		internal void GetConnection(string host, int port)
		{
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			if (port < 0 || port > 65535)
			{
				throw new ArgumentOutOfRangeException("port");
			}
			this.connection = new SmtpConnection(this, this.client, this.credentials, this.authenticationModules);
			this.connection.Timeout = this.timeout;
			if (Logging.On)
			{
				Logging.Associate(Logging.Web, this, this.connection);
			}
			if (this.EnableSsl)
			{
				this.connection.EnableSsl = true;
				this.connection.ClientCertificates = this.ClientCertificates;
			}
			this.connection.GetConnection(host, port);
		}

		// Token: 0x06003624 RID: 13860 RVA: 0x000E6FC0 File Offset: 0x000E5FC0
		internal IAsyncResult BeginGetConnection(string host, int port, ContextAwareResult outerResult, AsyncCallback callback, object state)
		{
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			if (port < 0 || port > 65535)
			{
				throw new ArgumentOutOfRangeException("port");
			}
			IAsyncResult asyncResult = null;
			try
			{
				this.connection = new SmtpConnection(this, this.client, this.credentials, this.authenticationModules);
				this.connection.Timeout = this.timeout;
				if (Logging.On)
				{
					Logging.Associate(Logging.Web, this, this.connection);
				}
				if (this.EnableSsl)
				{
					this.connection.EnableSsl = true;
					this.connection.ClientCertificates = this.ClientCertificates;
				}
				asyncResult = this.connection.BeginGetConnection(host, port, outerResult, callback, state);
			}
			catch (Exception ex)
			{
				throw new SmtpException(SR.GetString("MailHostNotFound"), ex);
			}
			catch
			{
				throw new SmtpException(SR.GetString("MailHostNotFound"), new Exception(SR.GetString("net_nonClsCompliantException")));
			}
			return asyncResult;
		}

		// Token: 0x06003625 RID: 13861 RVA: 0x000E70C4 File Offset: 0x000E60C4
		internal void EndGetConnection(IAsyncResult result)
		{
			this.connection.EndGetConnection(result);
		}

		// Token: 0x06003626 RID: 13862 RVA: 0x000E70D4 File Offset: 0x000E60D4
		internal IAsyncResult BeginSendMail(MailAddress sender, MailAddressCollection recipients, string deliveryNotify, AsyncCallback callback, object state)
		{
			if (sender == null)
			{
				throw new ArgumentNullException("sender");
			}
			if (recipients == null)
			{
				throw new ArgumentNullException("recipients");
			}
			SendMailAsyncResult sendMailAsyncResult = new SendMailAsyncResult(this.connection, sender.SmtpAddress, recipients, this.connection.DSNEnabled ? deliveryNotify : null, callback, state);
			sendMailAsyncResult.Send();
			return sendMailAsyncResult;
		}

		// Token: 0x06003627 RID: 13863 RVA: 0x000E712C File Offset: 0x000E612C
		internal void ReleaseConnection()
		{
			if (this.connection != null)
			{
				this.connection.ReleaseConnection();
			}
		}

		// Token: 0x06003628 RID: 13864 RVA: 0x000E7141 File Offset: 0x000E6141
		internal void Abort()
		{
			if (this.connection != null)
			{
				this.connection.Abort();
			}
		}

		// Token: 0x06003629 RID: 13865 RVA: 0x000E7158 File Offset: 0x000E6158
		internal MailWriter EndSendMail(IAsyncResult result)
		{
			return SendMailAsyncResult.End(result);
		}

		// Token: 0x0600362A RID: 13866 RVA: 0x000E7170 File Offset: 0x000E6170
		internal MailWriter SendMail(MailAddress sender, MailAddressCollection recipients, string deliveryNotify, out SmtpFailedRecipientException exception)
		{
			if (sender == null)
			{
				throw new ArgumentNullException("sender");
			}
			if (recipients == null)
			{
				throw new ArgumentNullException("recipients");
			}
			MailCommand.Send(this.connection, SmtpCommands.Mail, sender.SmtpAddress);
			this.failedRecipientExceptions.Clear();
			exception = null;
			foreach (MailAddress mailAddress in recipients)
			{
				string text;
				if (!RecipientCommand.Send(this.connection, this.connection.DSNEnabled ? (mailAddress.SmtpAddress + deliveryNotify) : mailAddress.SmtpAddress, out text))
				{
					this.failedRecipientExceptions.Add(new SmtpFailedRecipientException(this.connection.Reader.StatusCode, mailAddress.SmtpAddress, text));
				}
			}
			if (this.failedRecipientExceptions.Count > 0)
			{
				if (this.failedRecipientExceptions.Count == 1)
				{
					exception = (SmtpFailedRecipientException)this.failedRecipientExceptions[0];
				}
				else
				{
					exception = new SmtpFailedRecipientsException(this.failedRecipientExceptions, this.failedRecipientExceptions.Count == recipients.Count);
				}
				if (this.failedRecipientExceptions.Count == recipients.Count)
				{
					exception.fatal = true;
					throw exception;
				}
			}
			DataCommand.Send(this.connection);
			return new MailWriter(this.connection.GetClosableStream());
		}

		// Token: 0x04003153 RID: 12627
		internal const int DefaultPort = 25;

		// Token: 0x04003154 RID: 12628
		private ISmtpAuthenticationModule[] authenticationModules;

		// Token: 0x04003155 RID: 12629
		private SmtpConnection connection;

		// Token: 0x04003156 RID: 12630
		private SmtpClient client;

		// Token: 0x04003157 RID: 12631
		private ICredentialsByHost credentials;

		// Token: 0x04003158 RID: 12632
		private int timeout = 100000;

		// Token: 0x04003159 RID: 12633
		private ArrayList failedRecipientExceptions = new ArrayList();

		// Token: 0x0400315A RID: 12634
		private bool m_IdentityRequired;

		// Token: 0x0400315B RID: 12635
		private bool enableSsl;

		// Token: 0x0400315C RID: 12636
		private X509CertificateCollection clientCertificates;
	}
}
