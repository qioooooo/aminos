using System;
using System.ComponentModel;
using System.IO;
using System.Net.Configuration;
using System.Net.NetworkInformation;
using System.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Threading;

namespace System.Net.Mail
{
	// Token: 0x020006BB RID: 1723
	public class SmtpClient
	{
		// Token: 0x1400004E RID: 78
		// (add) Token: 0x06003520 RID: 13600 RVA: 0x000E1C5C File Offset: 0x000E0C5C
		// (remove) Token: 0x06003521 RID: 13601 RVA: 0x000E1C75 File Offset: 0x000E0C75
		public event SendCompletedEventHandler SendCompleted;

		// Token: 0x06003522 RID: 13602 RVA: 0x000E1C90 File Offset: 0x000E0C90
		public SmtpClient()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, "SmtpClient", ".ctor", "");
			}
			try
			{
				this.Initialize();
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, "SmtpClient", ".ctor", this);
				}
			}
		}

		// Token: 0x06003523 RID: 13603 RVA: 0x000E1CF8 File Offset: 0x000E0CF8
		public SmtpClient(string host)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, "SmtpClient", ".ctor", "host=" + host);
			}
			try
			{
				this.host = host;
				this.Initialize();
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, "SmtpClient", ".ctor", this);
				}
			}
		}

		// Token: 0x06003524 RID: 13604 RVA: 0x000E1D70 File Offset: 0x000E0D70
		public SmtpClient(string host, int port)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, "SmtpClient", ".ctor", string.Concat(new object[] { "host=", host, ", port=", port }));
			}
			try
			{
				if (port < 0)
				{
					throw new ArgumentOutOfRangeException("port");
				}
				this.host = host;
				this.port = port;
				this.Initialize();
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, "SmtpClient", ".ctor", this);
				}
			}
		}

		// Token: 0x06003525 RID: 13605 RVA: 0x000E1E1C File Offset: 0x000E0E1C
		private void Initialize()
		{
			if (this.port == SmtpClient.defaultPort || this.port == 0)
			{
				new SmtpPermission(SmtpAccess.Connect).Demand();
			}
			else
			{
				new SmtpPermission(SmtpAccess.ConnectToUnrestrictedPort).Demand();
			}
			this.transport = new SmtpTransport(this);
			if (Logging.On)
			{
				Logging.Associate(Logging.Web, this, this.transport);
			}
			this.onSendCompletedDelegate = new SendOrPostCallback(this.SendCompletedWaitCallback);
			if (SmtpClient.MailConfiguration.Smtp != null)
			{
				if (SmtpClient.MailConfiguration.Smtp.Network != null)
				{
					if (this.host == null || this.host.Length == 0)
					{
						this.host = SmtpClient.MailConfiguration.Smtp.Network.Host;
					}
					if (this.port == 0)
					{
						this.port = SmtpClient.MailConfiguration.Smtp.Network.Port;
					}
					this.transport.Credentials = SmtpClient.MailConfiguration.Smtp.Network.Credential;
					this.clientDomain = SmtpClient.MailConfiguration.Smtp.Network.ClientDomain;
					if (SmtpClient.MailConfiguration.Smtp.Network.TargetName != null)
					{
						this.targetName = SmtpClient.MailConfiguration.Smtp.Network.TargetName;
					}
				}
				this.deliveryMethod = SmtpClient.MailConfiguration.Smtp.DeliveryMethod;
				if (SmtpClient.MailConfiguration.Smtp.SpecifiedPickupDirectory != null)
				{
					this.pickupDirectoryLocation = SmtpClient.MailConfiguration.Smtp.SpecifiedPickupDirectory.PickupDirectoryLocation;
				}
			}
			if (this.host != null && this.host.Length != 0)
			{
				this.host = this.host.Trim();
			}
			if (this.port == 0)
			{
				this.port = SmtpClient.defaultPort;
			}
			if (this.clientDomain == null)
			{
				this.clientDomain = IPGlobalProperties.InternalGetIPGlobalProperties().HostName;
			}
			if (this.targetName == null)
			{
				this.targetName = "SMTPSVC/" + this.host;
			}
		}

		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x06003526 RID: 13606 RVA: 0x000E2018 File Offset: 0x000E1018
		// (set) Token: 0x06003527 RID: 13607 RVA: 0x000E2020 File Offset: 0x000E1020
		public string Host
		{
			get
			{
				return this.host;
			}
			set
			{
				if (this.InCall)
				{
					throw new InvalidOperationException(SR.GetString("SmtpInvalidOperationDuringSend"));
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value == string.Empty)
				{
					throw new ArgumentException(SR.GetString("net_emptystringset"), "value");
				}
				this.host = value.Trim();
			}
		}

		// Token: 0x17000C62 RID: 3170
		// (get) Token: 0x06003528 RID: 13608 RVA: 0x000E2081 File Offset: 0x000E1081
		// (set) Token: 0x06003529 RID: 13609 RVA: 0x000E208C File Offset: 0x000E108C
		public int Port
		{
			get
			{
				return this.port;
			}
			set
			{
				if (this.InCall)
				{
					throw new InvalidOperationException(SR.GetString("SmtpInvalidOperationDuringSend"));
				}
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (value != SmtpClient.defaultPort)
				{
					new SmtpPermission(SmtpAccess.ConnectToUnrestrictedPort).Demand();
				}
				this.port = value;
			}
		}

		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x0600352A RID: 13610 RVA: 0x000E20DA File Offset: 0x000E10DA
		// (set) Token: 0x0600352B RID: 13611 RVA: 0x000E20F1 File Offset: 0x000E10F1
		public bool UseDefaultCredentials
		{
			get
			{
				return this.transport.Credentials is SystemNetworkCredential;
			}
			set
			{
				if (this.InCall)
				{
					throw new InvalidOperationException(SR.GetString("SmtpInvalidOperationDuringSend"));
				}
				this.transport.Credentials = (value ? CredentialCache.DefaultNetworkCredentials : null);
			}
		}

		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x0600352C RID: 13612 RVA: 0x000E2121 File Offset: 0x000E1121
		// (set) Token: 0x0600352D RID: 13613 RVA: 0x000E212E File Offset: 0x000E112E
		public ICredentialsByHost Credentials
		{
			get
			{
				return this.transport.Credentials;
			}
			set
			{
				if (this.InCall)
				{
					throw new InvalidOperationException(SR.GetString("SmtpInvalidOperationDuringSend"));
				}
				this.transport.Credentials = value;
			}
		}

		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x0600352E RID: 13614 RVA: 0x000E2154 File Offset: 0x000E1154
		// (set) Token: 0x0600352F RID: 13615 RVA: 0x000E2161 File Offset: 0x000E1161
		public int Timeout
		{
			get
			{
				return this.transport.Timeout;
			}
			set
			{
				if (this.InCall)
				{
					throw new InvalidOperationException(SR.GetString("SmtpInvalidOperationDuringSend"));
				}
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.transport.Timeout = value;
			}
		}

		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x06003530 RID: 13616 RVA: 0x000E2196 File Offset: 0x000E1196
		public ServicePoint ServicePoint
		{
			get
			{
				this.CheckHostAndPort();
				return ServicePointManager.FindServicePoint(this.host, this.port);
			}
		}

		// Token: 0x17000C67 RID: 3175
		// (get) Token: 0x06003531 RID: 13617 RVA: 0x000E21AF File Offset: 0x000E11AF
		// (set) Token: 0x06003532 RID: 13618 RVA: 0x000E21B7 File Offset: 0x000E11B7
		public SmtpDeliveryMethod DeliveryMethod
		{
			get
			{
				return this.deliveryMethod;
			}
			set
			{
				this.deliveryMethod = value;
			}
		}

		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x06003533 RID: 13619 RVA: 0x000E21C0 File Offset: 0x000E11C0
		// (set) Token: 0x06003534 RID: 13620 RVA: 0x000E21C8 File Offset: 0x000E11C8
		public string PickupDirectoryLocation
		{
			get
			{
				return this.pickupDirectoryLocation;
			}
			set
			{
				this.pickupDirectoryLocation = value;
			}
		}

		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x06003535 RID: 13621 RVA: 0x000E21D1 File Offset: 0x000E11D1
		// (set) Token: 0x06003536 RID: 13622 RVA: 0x000E21DE File Offset: 0x000E11DE
		public bool EnableSsl
		{
			get
			{
				return this.transport.EnableSsl;
			}
			set
			{
				this.transport.EnableSsl = value;
			}
		}

		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x06003537 RID: 13623 RVA: 0x000E21EC File Offset: 0x000E11EC
		public X509CertificateCollection ClientCertificates
		{
			get
			{
				return this.transport.ClientCertificates;
			}
		}

		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x06003539 RID: 13625 RVA: 0x000E2202 File Offset: 0x000E1202
		// (set) Token: 0x06003538 RID: 13624 RVA: 0x000E21F9 File Offset: 0x000E11F9
		public string TargetName
		{
			get
			{
				return this.targetName;
			}
			set
			{
				this.targetName = value;
			}
		}

		// Token: 0x0600353A RID: 13626 RVA: 0x000E220C File Offset: 0x000E120C
		internal MailWriter GetFileMailWriter(string pickupDirectory)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, "SmtpClient.Send() pickupDirectory=" + pickupDirectory);
			}
			if (!Path.IsPathRooted(pickupDirectory))
			{
				throw new SmtpException(SR.GetString("SmtpNeedAbsolutePickupDirectory"));
			}
			string text2;
			do
			{
				string text = Guid.NewGuid().ToString() + ".eml";
				text2 = Path.Combine(pickupDirectory, text);
			}
			while (File.Exists(text2));
			FileStream fileStream = new FileStream(text2, FileMode.CreateNew);
			return new MailWriter(fileStream);
		}

		// Token: 0x0600353B RID: 13627 RVA: 0x000E2289 File Offset: 0x000E1289
		protected void OnSendCompleted(AsyncCompletedEventArgs e)
		{
			if (this.SendCompleted != null)
			{
				this.SendCompleted(this, e);
			}
		}

		// Token: 0x0600353C RID: 13628 RVA: 0x000E22A0 File Offset: 0x000E12A0
		private void SendCompletedWaitCallback(object operationState)
		{
			this.OnSendCompleted((AsyncCompletedEventArgs)operationState);
		}

		// Token: 0x0600353D RID: 13629 RVA: 0x000E22B0 File Offset: 0x000E12B0
		public void Send(string from, string recipients, string subject, string body)
		{
			MailMessage mailMessage = new MailMessage(from, recipients, subject, body);
			this.Send(mailMessage);
		}

		// Token: 0x0600353E RID: 13630 RVA: 0x000E22D0 File Offset: 0x000E12D0
		public void Send(MailMessage message)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "Send", message);
			}
			try
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.Web, this, "Send", "DeliveryMethod=" + this.DeliveryMethod.ToString());
				}
				if (Logging.On)
				{
					Logging.Associate(Logging.Web, this, message);
				}
				SmtpFailedRecipientException ex = null;
				if (this.InCall)
				{
					throw new InvalidOperationException(SR.GetString("net_inasync"));
				}
				if (message == null)
				{
					throw new ArgumentNullException("message");
				}
				if (this.DeliveryMethod == SmtpDeliveryMethod.Network)
				{
					this.CheckHostAndPort();
				}
				MailAddressCollection mailAddressCollection = new MailAddressCollection();
				if (message.From == null)
				{
					throw new InvalidOperationException(SR.GetString("SmtpFromRequired"));
				}
				if (message.To != null)
				{
					foreach (MailAddress mailAddress in message.To)
					{
						mailAddressCollection.Add(mailAddress);
					}
				}
				if (message.Bcc != null)
				{
					foreach (MailAddress mailAddress2 in message.Bcc)
					{
						mailAddressCollection.Add(mailAddress2);
					}
				}
				if (message.CC != null)
				{
					foreach (MailAddress mailAddress3 in message.CC)
					{
						mailAddressCollection.Add(mailAddress3);
					}
				}
				if (mailAddressCollection.Count == 0)
				{
					throw new InvalidOperationException(SR.GetString("SmtpRecipientRequired"));
				}
				this.transport.IdentityRequired = false;
				try
				{
					this.InCall = true;
					this.timedOut = false;
					this.timer = new Timer(new TimerCallback(this.TimeOutCallback), null, this.Timeout, this.Timeout);
					MailWriter mailWriter;
					switch (this.DeliveryMethod)
					{
					case SmtpDeliveryMethod.SpecifiedPickupDirectory:
						if (this.EnableSsl)
						{
							throw new SmtpException(SR.GetString("SmtpPickupDirectoryDoesnotSupportSsl"));
						}
						mailWriter = this.GetFileMailWriter(this.PickupDirectoryLocation);
						goto IL_025D;
					case SmtpDeliveryMethod.PickupDirectoryFromIis:
						if (this.EnableSsl)
						{
							throw new SmtpException(SR.GetString("SmtpPickupDirectoryDoesnotSupportSsl"));
						}
						mailWriter = this.GetFileMailWriter(IisPickupDirectory.GetPickupDirectory());
						goto IL_025D;
					}
					this.GetConnection();
					mailWriter = this.transport.SendMail((message.Sender != null) ? message.Sender : message.From, mailAddressCollection, message.BuildDeliveryStatusNotificationString(), out ex);
					IL_025D:
					this.message = message;
					message.Send(mailWriter, this.DeliveryMethod != SmtpDeliveryMethod.Network);
					mailWriter.Close();
					this.transport.ReleaseConnection();
					if (this.DeliveryMethod == SmtpDeliveryMethod.Network && ex != null)
					{
						throw ex;
					}
				}
				catch (Exception ex2)
				{
					if (Logging.On)
					{
						Logging.Exception(Logging.Web, this, "Send", ex2);
					}
					if (ex2 is SmtpFailedRecipientException && !((SmtpFailedRecipientException)ex2).fatal)
					{
						throw;
					}
					this.Abort();
					if (this.timedOut)
					{
						throw new SmtpException(SR.GetString("net_timeout"));
					}
					if (ex2 is SecurityException || ex2 is AuthenticationException || ex2 is SmtpException)
					{
						throw;
					}
					throw new SmtpException(SR.GetString("SmtpSendMailFailure"), ex2);
				}
				finally
				{
					this.InCall = false;
					if (this.timer != null)
					{
						this.timer.Dispose();
					}
				}
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "Send", null);
				}
			}
		}

		// Token: 0x0600353F RID: 13631 RVA: 0x000E26C8 File Offset: 0x000E16C8
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void SendAsync(string from, string recipients, string subject, string body, object userToken)
		{
			this.SendAsync(new MailMessage(from, recipients, subject, body), userToken);
		}

		// Token: 0x06003540 RID: 13632 RVA: 0x000E26DC File Offset: 0x000E16DC
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void SendAsync(MailMessage message, object userToken)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "SendAsync", "DeliveryMethod=" + this.DeliveryMethod.ToString());
			}
			try
			{
				if (this.InCall)
				{
					throw new InvalidOperationException(SR.GetString("net_inasync"));
				}
				if (message == null)
				{
					throw new ArgumentNullException("message");
				}
				if (this.DeliveryMethod == SmtpDeliveryMethod.Network)
				{
					this.CheckHostAndPort();
				}
				this.recipients = new MailAddressCollection();
				if (message.From == null)
				{
					throw new InvalidOperationException(SR.GetString("SmtpFromRequired"));
				}
				if (message.To != null)
				{
					foreach (MailAddress mailAddress in message.To)
					{
						this.recipients.Add(mailAddress);
					}
				}
				if (message.Bcc != null)
				{
					foreach (MailAddress mailAddress2 in message.Bcc)
					{
						this.recipients.Add(mailAddress2);
					}
				}
				if (message.CC != null)
				{
					foreach (MailAddress mailAddress3 in message.CC)
					{
						this.recipients.Add(mailAddress3);
					}
				}
				if (this.recipients.Count == 0)
				{
					throw new InvalidOperationException(SR.GetString("SmtpRecipientRequired"));
				}
				try
				{
					this.InCall = true;
					this.cancelled = false;
					this.message = message;
					CredentialCache credentialCache;
					this.transport.IdentityRequired = this.Credentials != null && ComNetOS.IsWinNt && (this.Credentials is SystemNetworkCredential || (credentialCache = this.Credentials as CredentialCache) == null || credentialCache.IsDefaultInCache);
					this.asyncOp = AsyncOperationManager.CreateOperation(userToken);
					switch (this.DeliveryMethod)
					{
					case SmtpDeliveryMethod.SpecifiedPickupDirectory:
					{
						if (this.EnableSsl)
						{
							throw new SmtpException(SR.GetString("SmtpPickupDirectoryDoesnotSupportSsl"));
						}
						this.writer = this.GetFileMailWriter(this.PickupDirectoryLocation);
						message.Send(this.writer, this.DeliveryMethod != SmtpDeliveryMethod.Network);
						if (this.writer != null)
						{
							this.writer.Close();
						}
						this.transport.ReleaseConnection();
						AsyncCompletedEventArgs asyncCompletedEventArgs = new AsyncCompletedEventArgs(null, false, this.asyncOp.UserSuppliedState);
						this.InCall = false;
						this.asyncOp.PostOperationCompleted(this.onSendCompletedDelegate, asyncCompletedEventArgs);
						goto IL_0387;
					}
					case SmtpDeliveryMethod.PickupDirectoryFromIis:
					{
						if (this.EnableSsl)
						{
							throw new SmtpException(SR.GetString("SmtpPickupDirectoryDoesnotSupportSsl"));
						}
						this.writer = this.GetFileMailWriter(IisPickupDirectory.GetPickupDirectory());
						message.Send(this.writer, this.DeliveryMethod != SmtpDeliveryMethod.Network);
						if (this.writer != null)
						{
							this.writer.Close();
						}
						this.transport.ReleaseConnection();
						AsyncCompletedEventArgs asyncCompletedEventArgs2 = new AsyncCompletedEventArgs(null, false, this.asyncOp.UserSuppliedState);
						this.InCall = false;
						this.asyncOp.PostOperationCompleted(this.onSendCompletedDelegate, asyncCompletedEventArgs2);
						goto IL_0387;
					}
					}
					this.operationCompletedResult = new ContextAwareResult(this.transport.IdentityRequired, true, null, this, SmtpClient._ContextSafeCompleteCallback);
					lock (this.operationCompletedResult.StartPostingAsyncOp())
					{
						this.transport.BeginGetConnection(this.host, this.port, this.operationCompletedResult, new AsyncCallback(this.ConnectCallback), this.operationCompletedResult);
						this.operationCompletedResult.FinishPostingAsyncOp();
					}
					IL_0387:;
				}
				catch (Exception ex)
				{
					this.InCall = false;
					if (Logging.On)
					{
						Logging.Exception(Logging.Web, this, "Send", ex);
					}
					if (ex is SmtpFailedRecipientException && !((SmtpFailedRecipientException)ex).fatal)
					{
						throw;
					}
					this.Abort();
					if (this.timedOut)
					{
						throw new SmtpException(SR.GetString("net_timeout"));
					}
					if (ex is SecurityException || ex is AuthenticationException || ex is SmtpException)
					{
						throw;
					}
					throw new SmtpException(SR.GetString("SmtpSendMailFailure"), ex);
				}
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "SendAsync", null);
				}
			}
		}

		// Token: 0x06003541 RID: 13633 RVA: 0x000E2BAC File Offset: 0x000E1BAC
		public void SendAsyncCancel()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "SendAsyncCancel", null);
			}
			try
			{
				if (this.InCall && !this.cancelled)
				{
					this.cancelled = true;
					this.Abort();
				}
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "SendAsyncCancel", null);
				}
			}
		}

		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x06003542 RID: 13634 RVA: 0x000E2C1C File Offset: 0x000E1C1C
		// (set) Token: 0x06003543 RID: 13635 RVA: 0x000E2C24 File Offset: 0x000E1C24
		internal bool InCall
		{
			get
			{
				return this.inCall;
			}
			set
			{
				this.inCall = value;
			}
		}

		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x06003544 RID: 13636 RVA: 0x000E2C2D File Offset: 0x000E1C2D
		internal static MailSettingsSectionGroupInternal MailConfiguration
		{
			get
			{
				if (SmtpClient.mailConfiguration == null)
				{
					SmtpClient.mailConfiguration = MailSettingsSectionGroupInternal.GetSection();
				}
				return SmtpClient.mailConfiguration;
			}
		}

		// Token: 0x06003545 RID: 13637 RVA: 0x000E2C45 File Offset: 0x000E1C45
		private void CheckHostAndPort()
		{
			if (this.host == null || this.host.Length == 0)
			{
				throw new InvalidOperationException(SR.GetString("UnspecifiedHost"));
			}
			if (this.port == 0)
			{
				throw new InvalidOperationException(SR.GetString("InvalidPort"));
			}
		}

		// Token: 0x06003546 RID: 13638 RVA: 0x000E2C84 File Offset: 0x000E1C84
		private void TimeOutCallback(object state)
		{
			if (!this.timedOut)
			{
				this.timedOut = true;
				this.Abort();
			}
		}

		// Token: 0x06003547 RID: 13639 RVA: 0x000E2C9C File Offset: 0x000E1C9C
		private void Complete(Exception exception, IAsyncResult result)
		{
			ContextAwareResult contextAwareResult = (ContextAwareResult)result.AsyncState;
			try
			{
				if (this.cancelled)
				{
					exception = null;
					this.Abort();
				}
				else if (exception != null && (!(exception is SmtpFailedRecipientException) || ((SmtpFailedRecipientException)exception).fatal))
				{
					this.Abort();
					if (!(exception is SmtpException))
					{
						exception = new SmtpException(SR.GetString("SmtpSendMailFailure"), exception);
					}
				}
				else
				{
					if (this.writer != null)
					{
						this.writer.Close();
					}
					this.transport.ReleaseConnection();
				}
			}
			finally
			{
				contextAwareResult.InvokeCallback(exception);
			}
		}

		// Token: 0x06003548 RID: 13640 RVA: 0x000E2D3C File Offset: 0x000E1D3C
		private static void ContextSafeCompleteCallback(IAsyncResult ar)
		{
			ContextAwareResult contextAwareResult = (ContextAwareResult)ar;
			SmtpClient smtpClient = (SmtpClient)ar.AsyncState;
			Exception ex = contextAwareResult.Result as Exception;
			AsyncOperation asyncOperation = smtpClient.asyncOp;
			AsyncCompletedEventArgs asyncCompletedEventArgs = new AsyncCompletedEventArgs(ex, smtpClient.cancelled, asyncOperation.UserSuppliedState);
			smtpClient.InCall = false;
			asyncOperation.PostOperationCompleted(smtpClient.onSendCompletedDelegate, asyncCompletedEventArgs);
		}

		// Token: 0x06003549 RID: 13641 RVA: 0x000E2D98 File Offset: 0x000E1D98
		private void SendMessageCallback(IAsyncResult result)
		{
			try
			{
				this.message.EndSend(result);
				this.Complete(null, result);
			}
			catch (Exception ex)
			{
				this.Complete(ex, result);
			}
		}

		// Token: 0x0600354A RID: 13642 RVA: 0x000E2DD8 File Offset: 0x000E1DD8
		private void SendMailCallback(IAsyncResult result)
		{
			try
			{
				this.writer = this.transport.EndSendMail(result);
			}
			catch (Exception ex)
			{
				if (!(ex is SmtpFailedRecipientException) || ((SmtpFailedRecipientException)ex).fatal)
				{
					this.Complete(ex, result);
					return;
				}
			}
			catch
			{
				this.Complete(new Exception(SR.GetString("net_nonClsCompliantException")), result);
				return;
			}
			try
			{
				if (this.cancelled)
				{
					this.Complete(null, result);
				}
				else
				{
					this.message.BeginSend(this.writer, this.DeliveryMethod != SmtpDeliveryMethod.Network, new AsyncCallback(this.SendMessageCallback), result.AsyncState);
				}
			}
			catch (Exception ex2)
			{
				this.Complete(ex2, result);
			}
		}

		// Token: 0x0600354B RID: 13643 RVA: 0x000E2EAC File Offset: 0x000E1EAC
		private void ConnectCallback(IAsyncResult result)
		{
			try
			{
				this.transport.EndGetConnection(result);
				if (this.cancelled)
				{
					this.Complete(null, result);
				}
				else
				{
					this.transport.BeginSendMail((this.message.Sender != null) ? this.message.Sender : this.message.From, this.recipients, this.message.BuildDeliveryStatusNotificationString(), new AsyncCallback(this.SendMailCallback), result.AsyncState);
				}
			}
			catch (Exception ex)
			{
				this.Complete(ex, result);
			}
		}

		// Token: 0x0600354C RID: 13644 RVA: 0x000E2F48 File Offset: 0x000E1F48
		private void GetConnection()
		{
			if (!this.transport.IsConnected)
			{
				this.transport.GetConnection(this.host, this.port);
			}
		}

		// Token: 0x0600354D RID: 13645 RVA: 0x000E2F70 File Offset: 0x000E1F70
		private void Abort()
		{
			try
			{
				this.transport.Abort();
			}
			catch
			{
			}
		}

		// Token: 0x040030B2 RID: 12466
		private string host;

		// Token: 0x040030B3 RID: 12467
		private int port;

		// Token: 0x040030B4 RID: 12468
		private bool inCall;

		// Token: 0x040030B5 RID: 12469
		private bool cancelled;

		// Token: 0x040030B6 RID: 12470
		private bool timedOut;

		// Token: 0x040030B7 RID: 12471
		private string targetName;

		// Token: 0x040030B8 RID: 12472
		private SmtpDeliveryMethod deliveryMethod;

		// Token: 0x040030B9 RID: 12473
		private string pickupDirectoryLocation;

		// Token: 0x040030BA RID: 12474
		private SmtpTransport transport;

		// Token: 0x040030BB RID: 12475
		private MailMessage message;

		// Token: 0x040030BC RID: 12476
		private MailWriter writer;

		// Token: 0x040030BD RID: 12477
		private MailAddressCollection recipients;

		// Token: 0x040030BE RID: 12478
		private SendOrPostCallback onSendCompletedDelegate;

		// Token: 0x040030BF RID: 12479
		private Timer timer;

		// Token: 0x040030C0 RID: 12480
		private static MailSettingsSectionGroupInternal mailConfiguration;

		// Token: 0x040030C1 RID: 12481
		private ContextAwareResult operationCompletedResult;

		// Token: 0x040030C2 RID: 12482
		private AsyncOperation asyncOp;

		// Token: 0x040030C3 RID: 12483
		private static AsyncCallback _ContextSafeCompleteCallback = new AsyncCallback(SmtpClient.ContextSafeCompleteCallback);

		// Token: 0x040030C4 RID: 12484
		private static int defaultPort = 25;

		// Token: 0x040030C5 RID: 12485
		internal string clientDomain;
	}
}
