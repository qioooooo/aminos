using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using System.Threading;
using System.Web.Util;

namespace System.Web.Management
{
	// Token: 0x020002D3 RID: 723
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class TemplatedMailWebEventProvider : MailWebEventProvider, IInternalWebEventProvider
	{
		// Token: 0x060024F3 RID: 9459 RVA: 0x0009EC0C File Offset: 0x0009DC0C
		internal TemplatedMailWebEventProvider()
		{
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x0009EC14 File Offset: 0x0009DC14
		public override void Initialize(string name, NameValueCollection config)
		{
			ProviderUtil.GetAndRemoveStringAttribute(config, "template", name, ref this._templateUrl);
			if (this._templateUrl == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Provider_missing_attribute", new object[] { "template", name }));
			}
			this._templateUrl = this._templateUrl.Trim();
			if (this._templateUrl.Length == 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_provider_attribute", new object[] { "template", name, this._templateUrl }));
			}
			if (!UrlPath.IsRelativeUrl(this._templateUrl))
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_mail_template_provider_attribute", new object[] { "template", name, this._templateUrl }));
			}
			this._templateUrl = UrlPath.Combine(HttpRuntime.AppDomainAppVirtualPathString, this._templateUrl);
			if (!HttpRuntime.IsPathWithinAppRoot(this._templateUrl))
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_mail_template_provider_attribute", new object[] { "template", name, this._templateUrl }));
			}
			ProviderUtil.GetAndRemoveBooleanAttribute(config, "detailedTemplateErrors", name, ref this._detailedTemplateErrors);
			base.Initialize(name, config);
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x0009ED50 File Offset: 0x0009DD50
		private void GenerateMessageBody(MailMessage msg, WebBaseEventCollection events, DateTime lastNotificationUtc, int discardedSinceLastNotification, int eventsInBuffer, int notificationSequence, EventNotificationType notificationType, int eventsInNotification, int eventsRemaining, int messagesInNotification, int eventsLostDueToMessageLimit, int messageSequence, out bool fatalError)
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InstalledUICulture);
			MailEventNotificationInfo mailEventNotificationInfo = new MailEventNotificationInfo(msg, events, lastNotificationUtc, discardedSinceLastNotification, eventsInBuffer, notificationSequence, notificationType, eventsInNotification, eventsRemaining, messagesInNotification, eventsLostDueToMessageLimit, messageSequence);
			CallContext.SetData("_TWCurEvt", mailEventNotificationInfo);
			try
			{
				TemplatedMailWebEventProvider.TemplatedMailErrorFormatterGenerator templatedMailErrorFormatterGenerator = new TemplatedMailWebEventProvider.TemplatedMailErrorFormatterGenerator(events.Count + eventsRemaining, this._detailedTemplateErrors);
				HttpServerUtility.ExecuteLocalRequestAndCaptureResponse(this._templateUrl, stringWriter, templatedMailErrorFormatterGenerator);
				fatalError = templatedMailErrorFormatterGenerator.ErrorFormatterCalled;
				if (fatalError)
				{
					msg.Subject = HttpUtility.HtmlEncode(SR.GetString("WebEvent_event_email_subject_template_error", new object[]
					{
						notificationSequence.ToString(CultureInfo.InstalledUICulture),
						messageSequence.ToString(CultureInfo.InstalledUICulture),
						base.SubjectPrefix
					}));
				}
				msg.Body = stringWriter.ToString();
				msg.IsBodyHtml = true;
			}
			finally
			{
				CallContext.FreeNamedDataSlot("_TWCurEvt");
			}
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x0009EE34 File Offset: 0x0009DE34
		internal override void SendMessage(WebBaseEvent eventRaised)
		{
			WebBaseEventCollection webBaseEventCollection = new WebBaseEventCollection(eventRaised);
			bool flag;
			this.SendMessageInternal(webBaseEventCollection, DateTime.MinValue, 0, 0, Interlocked.Increment(ref this._nonBufferNotificationSequence), EventNotificationType.Unbuffered, 1, 0, 1, 0, 1, out flag);
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x0009EE6C File Offset: 0x0009DE6C
		internal override void SendMessage(WebBaseEventCollection events, WebEventBufferFlushInfo flushInfo, int eventsInNotification, int eventsRemaining, int messagesInNotification, int eventsLostDueToMessageLimit, int messageSequence, int eventsSent, out bool fatalError)
		{
			this.SendMessageInternal(events, flushInfo.LastNotificationUtc, flushInfo.EventsDiscardedSinceLastNotification, flushInfo.EventsInBuffer, flushInfo.NotificationSequence, flushInfo.NotificationType, eventsInNotification, eventsRemaining, messagesInNotification, eventsLostDueToMessageLimit, messageSequence, out fatalError);
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x0009EEAC File Offset: 0x0009DEAC
		private void SendMessageInternal(WebBaseEventCollection events, DateTime lastNotificationUtc, int discardedSinceLastNotification, int eventsInBuffer, int notificationSequence, EventNotificationType notificationType, int eventsInNotification, int eventsRemaining, int messagesInNotification, int eventsLostDueToMessageLimit, int messageSequence, out bool fatalError)
		{
			using (MailMessage message = base.GetMessage())
			{
				message.Subject = base.GenerateSubject(notificationSequence, messageSequence, events, events.Count);
				this.GenerateMessageBody(message, events, lastNotificationUtc, discardedSinceLastNotification, eventsInBuffer, notificationSequence, notificationType, eventsInNotification, eventsRemaining, messagesInNotification, eventsLostDueToMessageLimit, messageSequence, out fatalError);
				base.SendMail(message);
			}
		}

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x060024F9 RID: 9465 RVA: 0x0009EF18 File Offset: 0x0009DF18
		public static MailEventNotificationInfo CurrentNotification
		{
			get
			{
				return (MailEventNotificationInfo)CallContext.GetData("_TWCurEvt");
			}
		}

		// Token: 0x04001CA6 RID: 7334
		internal const string CurrentEventsName = "_TWCurEvt";

		// Token: 0x04001CA7 RID: 7335
		private int _nonBufferNotificationSequence;

		// Token: 0x04001CA8 RID: 7336
		private string _templateUrl;

		// Token: 0x04001CA9 RID: 7337
		private bool _detailedTemplateErrors;

		// Token: 0x020002D4 RID: 724
		private class TemplatedMailErrorFormatterGenerator : ErrorFormatterGenerator
		{
			// Token: 0x060024FA RID: 9466 RVA: 0x0009EF29 File Offset: 0x0009DF29
			internal TemplatedMailErrorFormatterGenerator(int eventsRemaining, bool showDetails)
			{
				this._eventsRemaining = eventsRemaining;
				this._showDetails = showDetails;
			}

			// Token: 0x17000794 RID: 1940
			// (get) Token: 0x060024FB RID: 9467 RVA: 0x0009EF3F File Offset: 0x0009DF3F
			internal bool ErrorFormatterCalled
			{
				get
				{
					return this._errorFormatterCalled;
				}
			}

			// Token: 0x060024FC RID: 9468 RVA: 0x0009EF48 File Offset: 0x0009DF48
			internal override ErrorFormatter GetErrorFormatter(Exception e)
			{
				Exception ex = e.InnerException;
				this._errorFormatterCalled = true;
				while (ex != null)
				{
					if (ex is HttpCompileException)
					{
						return new TemplatedMailCompileErrorFormatter((HttpCompileException)ex, this._eventsRemaining, this._showDetails);
					}
					ex = ex.InnerException;
				}
				return new TemplatedMailRuntimeErrorFormatter(e, this._eventsRemaining, this._showDetails);
			}

			// Token: 0x04001CAA RID: 7338
			private int _eventsRemaining;

			// Token: 0x04001CAB RID: 7339
			private bool _showDetails;

			// Token: 0x04001CAC RID: 7340
			private bool _errorFormatterCalled;
		}
	}
}
