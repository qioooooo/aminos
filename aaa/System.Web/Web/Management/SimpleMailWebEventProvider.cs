using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Net.Mail;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Web.Util;

namespace System.Web.Management
{
	// Token: 0x020002CC RID: 716
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class SimpleMailWebEventProvider : MailWebEventProvider, IInternalWebEventProvider
	{
		// Token: 0x060024B5 RID: 9397 RVA: 0x0009CE45 File Offset: 0x0009BE45
		internal SimpleMailWebEventProvider()
		{
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x0009CE64 File Offset: 0x0009BE64
		public override void Initialize(string name, NameValueCollection config)
		{
			string text = null;
			ProviderUtil.GetAndRemoveStringAttribute(config, "bodyHeader", name, ref this._bodyHeader);
			if (this._bodyHeader != null)
			{
				this._bodyHeader += "\n";
			}
			ProviderUtil.GetAndRemoveStringAttribute(config, "bodyFooter", name, ref this._bodyFooter);
			if (this._bodyFooter != null)
			{
				this._bodyFooter += "\n";
			}
			ProviderUtil.GetAndRemoveStringAttribute(config, "separator", name, ref text);
			if (text != null)
			{
				this._separator = text + "\n";
			}
			ProviderUtil.GetAndRemovePositiveOrInfiniteAttribute(config, "maxEventLength", name, ref this._maxEventLength);
			base.Initialize(name, config);
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x0009CF10 File Offset: 0x0009BF10
		private void GenerateWarnings(StringBuilder sb, DateTime lastFlush, int discardedSinceLastFlush, int seq, int eventsToDrop)
		{
			if (!base.UseBuffering)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			if (discardedSinceLastFlush != 0 && seq == 1)
			{
				sb.Append(SimpleMailWebEventProvider.s_header_warnings);
				sb.Append("\n");
				sb.Append(this._separator);
				flag = true;
				sb.Append(SR.GetString("MailWebEventProvider_discard_warning", new object[]
				{
					100.ToString(CultureInfo.InstalledUICulture),
					discardedSinceLastFlush.ToString(CultureInfo.InstalledUICulture),
					lastFlush.ToString("r", CultureInfo.InstalledUICulture)
				}));
				sb.Append("\n\n");
				flag2 = true;
			}
			if (eventsToDrop > 0)
			{
				if (!flag)
				{
					sb.Append(SimpleMailWebEventProvider.s_header_warnings);
					sb.Append("\n");
					sb.Append(this._separator);
				}
				sb.Append(SR.GetString("MailWebEventProvider_events_drop_warning", new object[]
				{
					101.ToString(CultureInfo.InstalledUICulture),
					eventsToDrop.ToString(CultureInfo.InstalledUICulture)
				}));
				sb.Append("\n\n");
				flag2 = true;
			}
			if (flag2)
			{
				sb.Append("\n");
			}
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x0009D048 File Offset: 0x0009C048
		private void GenerateApplicationInformation(StringBuilder sb)
		{
			sb.Append(SimpleMailWebEventProvider.s_header_app_info);
			sb.Append("\n");
			sb.Append(this._separator);
			sb.Append(WebBaseEvent.ApplicationInformation.ToString());
			sb.Append("\n\n");
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x0009D098 File Offset: 0x0009C098
		private void GenerateSummary(StringBuilder sb, int firstEvent, int lastEvent, int eventsInNotif, int eventsInBuffer)
		{
			if (!base.UseBuffering)
			{
				return;
			}
			sb.Append(SimpleMailWebEventProvider.s_header_summary);
			sb.Append("\n");
			sb.Append(this._separator);
			firstEvent++;
			lastEvent++;
			sb.Append(SR.GetString("MailWebEventProvider_summary_body", new object[]
			{
				firstEvent.ToString(CultureInfo.InstalledUICulture),
				lastEvent.ToString(CultureInfo.InstalledUICulture),
				eventsInNotif.ToString(CultureInfo.InstalledUICulture),
				eventsInBuffer.ToString(CultureInfo.InstalledUICulture)
			}));
			sb.Append("\n\n");
			sb.Append("\n");
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x0009D14C File Offset: 0x0009C14C
		private string GenerateBody(WebBaseEventCollection events, int begin, DateTime lastFlush, int discardedSinceLastFlush, int eventsInBuffer, int messageSequence, int eventsInNotification, int eventsLostDueToMessageLimit)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int count = events.Count;
			if (this._bodyHeader != null)
			{
				stringBuilder.Append(this._bodyHeader);
			}
			this.GenerateWarnings(stringBuilder, lastFlush, discardedSinceLastFlush, messageSequence, eventsLostDueToMessageLimit);
			this.GenerateSummary(stringBuilder, begin, begin + count - 1, eventsInNotification, eventsInBuffer);
			this.GenerateApplicationInformation(stringBuilder);
			for (int i = 0; i < count; i++)
			{
				WebBaseEvent webBaseEvent = events[i];
				string text = webBaseEvent.ToString(false, true);
				if (this._maxEventLength != 2147483647 && text.Length > this._maxEventLength)
				{
					text = text.Substring(0, this._maxEventLength);
				}
				if (i == 0)
				{
					stringBuilder.Append(SimpleMailWebEventProvider.s_header_events);
					stringBuilder.Append("\n");
					stringBuilder.Append(this._separator);
				}
				stringBuilder.Append(text);
				stringBuilder.Append("\n");
				stringBuilder.Append(this._separator);
			}
			if (this._bodyFooter != null)
			{
				stringBuilder.Append(this._bodyFooter);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x0009D258 File Offset: 0x0009C258
		internal override void SendMessage(WebBaseEvent eventRaised)
		{
			WebBaseEventCollection webBaseEventCollection = new WebBaseEventCollection(eventRaised);
			this.SendMessageInternal(webBaseEventCollection, Interlocked.Increment(ref this._nonBufferNotificationSequence), 0, DateTime.MinValue, 0, 0, 1, 1, 1, 0);
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x0009D28C File Offset: 0x0009C28C
		internal override void SendMessage(WebBaseEventCollection events, WebEventBufferFlushInfo flushInfo, int eventsInNotification, int eventsRemaining, int messagesInNotification, int eventsLostDueToMessageLimit, int messageSequence, int eventsSent, out bool fatalError)
		{
			this.SendMessageInternal(events, flushInfo.NotificationSequence, eventsSent, flushInfo.LastNotificationUtc, flushInfo.EventsDiscardedSinceLastNotification, flushInfo.EventsInBuffer, messageSequence, messagesInNotification, eventsInNotification, eventsLostDueToMessageLimit);
			fatalError = false;
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x0009D2C8 File Offset: 0x0009C2C8
		private void SendMessageInternal(WebBaseEventCollection events, int notificationSequence, int begin, DateTime lastFlush, int discardedSinceLastFlush, int eventsInBuffer, int messageSequence, int messagesInNotification, int eventsInNotification, int eventsLostDueToMessageLimit)
		{
			using (MailMessage message = base.GetMessage())
			{
				if (messageSequence != messagesInNotification)
				{
					eventsLostDueToMessageLimit = 0;
				}
				message.Body = this.GenerateBody(events, begin, lastFlush, discardedSinceLastFlush, eventsInBuffer, messageSequence, eventsInNotification, eventsLostDueToMessageLimit);
				message.Subject = base.GenerateSubject(notificationSequence, messageSequence, events, events.Count);
				base.SendMail(message);
			}
		}

		// Token: 0x04001C73 RID: 7283
		private const int DefaultMaxEventLength = 8192;

		// Token: 0x04001C74 RID: 7284
		private const int MessageIdDiscard = 100;

		// Token: 0x04001C75 RID: 7285
		private const int MessageIdEventsToDrop = 101;

		// Token: 0x04001C76 RID: 7286
		private static string s_header_warnings = SR.GetString("MailWebEventProvider_Warnings");

		// Token: 0x04001C77 RID: 7287
		private static string s_header_summary = SR.GetString("MailWebEventProvider_Summary");

		// Token: 0x04001C78 RID: 7288
		private static string s_header_app_info = SR.GetString("MailWebEventProvider_Application_Info");

		// Token: 0x04001C79 RID: 7289
		private static string s_header_events = SR.GetString("MailWebEventProvider_Events");

		// Token: 0x04001C7A RID: 7290
		private string _separator = "---------------\n";

		// Token: 0x04001C7B RID: 7291
		private string _bodyHeader;

		// Token: 0x04001C7C RID: 7292
		private string _bodyFooter;

		// Token: 0x04001C7D RID: 7293
		private int _maxEventLength = 8192;

		// Token: 0x04001C7E RID: 7294
		private int _nonBufferNotificationSequence;
	}
}
