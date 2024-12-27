using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Net.Mail;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.Management
{
	// Token: 0x020002C7 RID: 711
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class MailWebEventProvider : BufferedWebEventProvider
	{
		// Token: 0x06002484 RID: 9348 RVA: 0x0009BF86 File Offset: 0x0009AF86
		internal MailWebEventProvider()
		{
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x0009BFA0 File Offset: 0x0009AFA0
		public override void Initialize(string name, NameValueCollection config)
		{
			ProviderUtil.GetAndRemoveRequiredNonEmptyStringAttribute(config, "from", name, ref this._from);
			ProviderUtil.GetAndRemoveStringAttribute(config, "to", name, ref this._to);
			ProviderUtil.GetAndRemoveStringAttribute(config, "cc", name, ref this._cc);
			ProviderUtil.GetAndRemoveStringAttribute(config, "bcc", name, ref this._bcc);
			if (string.IsNullOrEmpty(this._to) && string.IsNullOrEmpty(this._cc) && string.IsNullOrEmpty(this._bcc))
			{
				throw new ConfigurationErrorsException(SR.GetString("MailWebEventProvider_no_recipient_error", new object[]
				{
					base.GetType().ToString(),
					name
				}));
			}
			ProviderUtil.GetAndRemoveStringAttribute(config, "subjectPrefix", name, ref this._subjectPrefix);
			ProviderUtil.GetAndRemoveNonZeroPositiveOrInfiniteAttribute(config, "maxMessagesPerNotification", name, ref this._maxMessagesPerNotification);
			ProviderUtil.GetAndRemoveNonZeroPositiveOrInfiniteAttribute(config, "maxEventsPerMessage", name, ref this._maxEventsPerMessage);
			this._smtpClient = new SmtpClient();
			base.Initialize(name, config);
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06002486 RID: 9350 RVA: 0x0009C08F File Offset: 0x0009B08F
		internal string SubjectPrefix
		{
			get
			{
				return this._subjectPrefix;
			}
		}

		// Token: 0x06002487 RID: 9351 RVA: 0x0009C098 File Offset: 0x0009B098
		internal string GenerateSubject(int notificationSequence, int messageSequence, WebBaseEventCollection events, int count)
		{
			WebBaseEvent webBaseEvent = events[0];
			if (count == 1)
			{
				return HttpUtility.HtmlEncode(SR.GetString("WebEvent_event_email_subject", new string[]
				{
					notificationSequence.ToString(CultureInfo.InstalledUICulture),
					messageSequence.ToString(CultureInfo.InstalledUICulture),
					this._subjectPrefix,
					webBaseEvent.GetType().ToString(),
					WebBaseEvent.ApplicationInformation.ApplicationVirtualPath
				}));
			}
			return HttpUtility.HtmlEncode(SR.GetString("WebEvent_event_group_email_subject", new string[]
			{
				notificationSequence.ToString(CultureInfo.InstalledUICulture),
				messageSequence.ToString(CultureInfo.InstalledUICulture),
				this._subjectPrefix,
				count.ToString(CultureInfo.InstalledUICulture),
				WebBaseEvent.ApplicationInformation.ApplicationVirtualPath
			}));
		}

		// Token: 0x06002488 RID: 9352 RVA: 0x0009C168 File Offset: 0x0009B168
		internal MailMessage GetMessage()
		{
			MailMessage mailMessage = new MailMessage(this._from, this._to);
			if (!string.IsNullOrEmpty(this._cc))
			{
				mailMessage.CC.Add(new MailAddress(this._cc));
			}
			if (!string.IsNullOrEmpty(this._bcc))
			{
				mailMessage.Bcc.Add(new MailAddress(this._bcc));
			}
			return mailMessage;
		}

		// Token: 0x06002489 RID: 9353 RVA: 0x0009C1D0 File Offset: 0x0009B1D0
		[SmtpPermission(SecurityAction.Assert, Access = "Connect")]
		internal void SendMail(MailMessage msg)
		{
			try
			{
				this._smtpClient.Send(msg);
			}
			catch (Exception ex)
			{
				throw new HttpException(SR.GetString("MailWebEventProvider_cannot_send_mail"), ex);
			}
		}

		// Token: 0x0600248A RID: 9354
		internal abstract void SendMessage(WebBaseEvent eventRaised);

		// Token: 0x0600248B RID: 9355 RVA: 0x0009C210 File Offset: 0x0009B210
		public override void ProcessEvent(WebBaseEvent eventRaised)
		{
			if (base.UseBuffering)
			{
				base.ProcessEvent(eventRaised);
				return;
			}
			this.SendMessage(eventRaised);
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x0009C229 File Offset: 0x0009B229
		public override void Shutdown()
		{
			this.Flush();
		}

		// Token: 0x0600248D RID: 9357 RVA: 0x0009C234 File Offset: 0x0009B234
		public override void ProcessEventFlush(WebEventBufferFlushInfo flushInfo)
		{
			int num = flushInfo.Events.Count;
			bool flag = false;
			int num2 = 1;
			int num3 = 0;
			bool flag2 = false;
			if (num == 0)
			{
				return;
			}
			WebBaseEvent[] array = null;
			int num4;
			if (num > this.MaxEventsPerMessage)
			{
				flag = true;
				num4 = num / this.MaxEventsPerMessage;
				if (num > num4 * this.MaxEventsPerMessage)
				{
					num4++;
				}
				if (num4 > this.MaxMessagesPerNotification)
				{
					num3 = num - this.MaxMessagesPerNotification * this.MaxEventsPerMessage;
					num4 = this.MaxMessagesPerNotification;
					num -= num3;
				}
			}
			else
			{
				num4 = 1;
			}
			int i = 0;
			while (i < num)
			{
				WebBaseEventCollection webBaseEventCollection;
				if (flag)
				{
					int num5 = Math.Min(this.MaxEventsPerMessage, num - i);
					if (array == null || array.Length != num5)
					{
						array = new WebBaseEvent[num5];
					}
					for (int j = 0; j < num5; j++)
					{
						array[j] = flushInfo.Events[j + i];
					}
					webBaseEventCollection = new WebBaseEventCollection(array);
				}
				else
				{
					webBaseEventCollection = flushInfo.Events;
				}
				this.SendMessage(webBaseEventCollection, flushInfo, num, num - (i + webBaseEventCollection.Count), num4, num3, num2, i, out flag2);
				if (flag2)
				{
					return;
				}
				i += webBaseEventCollection.Count;
				num2++;
			}
		}

		// Token: 0x0600248E RID: 9358
		internal abstract void SendMessage(WebBaseEventCollection events, WebEventBufferFlushInfo flushInfo, int eventsInNotification, int eventsRemaining, int messagesInNotification, int eventsLostDueToMessageLimit, int messageSequence, int eventsSent, out bool fatalError);

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x0600248F RID: 9359 RVA: 0x0009C350 File Offset: 0x0009B350
		internal int MaxMessagesPerNotification
		{
			get
			{
				return this._maxMessagesPerNotification;
			}
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x06002490 RID: 9360 RVA: 0x0009C358 File Offset: 0x0009B358
		internal int MaxEventsPerMessage
		{
			get
			{
				return this._maxEventsPerMessage;
			}
		}

		// Token: 0x04001C4B RID: 7243
		internal const int DefaultMaxMessagesPerNotification = 10;

		// Token: 0x04001C4C RID: 7244
		internal const int DefaultMaxEventsPerMessage = 50;

		// Token: 0x04001C4D RID: 7245
		internal const int MessageSequenceBase = 1;

		// Token: 0x04001C4E RID: 7246
		private string _from;

		// Token: 0x04001C4F RID: 7247
		private string _to;

		// Token: 0x04001C50 RID: 7248
		private string _cc;

		// Token: 0x04001C51 RID: 7249
		private string _bcc;

		// Token: 0x04001C52 RID: 7250
		private string _subjectPrefix;

		// Token: 0x04001C53 RID: 7251
		private SmtpClient _smtpClient;

		// Token: 0x04001C54 RID: 7252
		private int _maxMessagesPerNotification = 10;

		// Token: 0x04001C55 RID: 7253
		private int _maxEventsPerMessage = 50;
	}
}
