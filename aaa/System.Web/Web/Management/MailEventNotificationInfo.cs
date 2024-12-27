using System;
using System.Net.Mail;
using System.Security.Permissions;

namespace System.Web.Management
{
	// Token: 0x020002D5 RID: 725
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class MailEventNotificationInfo
	{
		// Token: 0x060024FD RID: 9469 RVA: 0x0009EFA4 File Offset: 0x0009DFA4
		internal MailEventNotificationInfo(MailMessage msg, WebBaseEventCollection events, DateTime lastNotificationUtc, int discardedSinceLastNotification, int eventsInBuffer, int notificationSequence, EventNotificationType notificationType, int eventsInNotification, int eventsRemaining, int messagesInNotification, int eventsLostDueToMessageLimit, int messageSequence)
		{
			this._events = events;
			this._lastNotificationUtc = lastNotificationUtc;
			this._discardedSinceLastNotification = discardedSinceLastNotification;
			this._eventsInBuffer = eventsInBuffer;
			this._notificationSequence = notificationSequence;
			this._notificationType = notificationType;
			this._eventsInNotification = eventsInNotification;
			this._eventsRemaining = eventsRemaining;
			this._messagesInNotification = messagesInNotification;
			this._eventsLostDueToMessageLimit = eventsLostDueToMessageLimit;
			this._messageSequence = messageSequence;
			this._msg = msg;
		}

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x060024FE RID: 9470 RVA: 0x0009F014 File Offset: 0x0009E014
		public WebBaseEventCollection Events
		{
			get
			{
				return this._events;
			}
		}

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x060024FF RID: 9471 RVA: 0x0009F01C File Offset: 0x0009E01C
		public EventNotificationType NotificationType
		{
			get
			{
				return this._notificationType;
			}
		}

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x06002500 RID: 9472 RVA: 0x0009F024 File Offset: 0x0009E024
		public int EventsInNotification
		{
			get
			{
				return this._eventsInNotification;
			}
		}

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06002501 RID: 9473 RVA: 0x0009F02C File Offset: 0x0009E02C
		public int EventsRemaining
		{
			get
			{
				return this._eventsRemaining;
			}
		}

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06002502 RID: 9474 RVA: 0x0009F034 File Offset: 0x0009E034
		public int MessagesInNotification
		{
			get
			{
				return this._messagesInNotification;
			}
		}

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06002503 RID: 9475 RVA: 0x0009F03C File Offset: 0x0009E03C
		public int EventsInBuffer
		{
			get
			{
				return this._eventsInBuffer;
			}
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06002504 RID: 9476 RVA: 0x0009F044 File Offset: 0x0009E044
		public int EventsDiscardedByBuffer
		{
			get
			{
				return this._discardedSinceLastNotification;
			}
		}

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06002505 RID: 9477 RVA: 0x0009F04C File Offset: 0x0009E04C
		public int EventsDiscardedDueToMessageLimit
		{
			get
			{
				return this._eventsLostDueToMessageLimit;
			}
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06002506 RID: 9478 RVA: 0x0009F054 File Offset: 0x0009E054
		public int NotificationSequence
		{
			get
			{
				return this._notificationSequence;
			}
		}

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06002507 RID: 9479 RVA: 0x0009F05C File Offset: 0x0009E05C
		public int MessageSequence
		{
			get
			{
				return this._messageSequence;
			}
		}

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x06002508 RID: 9480 RVA: 0x0009F064 File Offset: 0x0009E064
		public DateTime LastNotificationUtc
		{
			get
			{
				return this._lastNotificationUtc;
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06002509 RID: 9481 RVA: 0x0009F06C File Offset: 0x0009E06C
		public MailMessage Message
		{
			get
			{
				return this._msg;
			}
		}

		// Token: 0x04001CAD RID: 7341
		private WebBaseEventCollection _events;

		// Token: 0x04001CAE RID: 7342
		private DateTime _lastNotificationUtc;

		// Token: 0x04001CAF RID: 7343
		private int _discardedSinceLastNotification;

		// Token: 0x04001CB0 RID: 7344
		private int _eventsInBuffer;

		// Token: 0x04001CB1 RID: 7345
		private int _notificationSequence;

		// Token: 0x04001CB2 RID: 7346
		private EventNotificationType _notificationType;

		// Token: 0x04001CB3 RID: 7347
		private int _eventsInNotification;

		// Token: 0x04001CB4 RID: 7348
		private int _eventsRemaining;

		// Token: 0x04001CB5 RID: 7349
		private int _messagesInNotification;

		// Token: 0x04001CB6 RID: 7350
		private int _eventsLostDueToMessageLimit;

		// Token: 0x04001CB7 RID: 7351
		private int _messageSequence;

		// Token: 0x04001CB8 RID: 7352
		private MailMessage _msg;
	}
}
