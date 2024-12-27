using System;
using System.Security.Permissions;

namespace System.Web.Management
{
	// Token: 0x020002DA RID: 730
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class WebEventBufferFlushInfo
	{
		// Token: 0x06002518 RID: 9496 RVA: 0x0009F1FF File Offset: 0x0009E1FF
		internal WebEventBufferFlushInfo(WebBaseEventCollection events, EventNotificationType notificationType, int notificationSequence, DateTime lastNotification, int eventsDiscardedSinceLastNotification, int eventsInBuffer)
		{
			this._events = events;
			this._notificationType = notificationType;
			this._notificationSequence = notificationSequence;
			this._lastNotification = lastNotification;
			this._eventsDiscardedSinceLastNotification = eventsDiscardedSinceLastNotification;
			this._eventsInBuffer = eventsInBuffer;
		}

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06002519 RID: 9497 RVA: 0x0009F234 File Offset: 0x0009E234
		public WebBaseEventCollection Events
		{
			get
			{
				return this._events;
			}
		}

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x0600251A RID: 9498 RVA: 0x0009F23C File Offset: 0x0009E23C
		public DateTime LastNotificationUtc
		{
			get
			{
				return this._lastNotification;
			}
		}

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x0600251B RID: 9499 RVA: 0x0009F244 File Offset: 0x0009E244
		public int EventsDiscardedSinceLastNotification
		{
			get
			{
				return this._eventsDiscardedSinceLastNotification;
			}
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x0600251C RID: 9500 RVA: 0x0009F24C File Offset: 0x0009E24C
		public int EventsInBuffer
		{
			get
			{
				return this._eventsInBuffer;
			}
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x0600251D RID: 9501 RVA: 0x0009F254 File Offset: 0x0009E254
		public int NotificationSequence
		{
			get
			{
				return this._notificationSequence;
			}
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x0600251E RID: 9502 RVA: 0x0009F25C File Offset: 0x0009E25C
		public EventNotificationType NotificationType
		{
			get
			{
				return this._notificationType;
			}
		}

		// Token: 0x04001CC6 RID: 7366
		private WebBaseEventCollection _events;

		// Token: 0x04001CC7 RID: 7367
		private DateTime _lastNotification;

		// Token: 0x04001CC8 RID: 7368
		private int _eventsDiscardedSinceLastNotification;

		// Token: 0x04001CC9 RID: 7369
		private int _eventsInBuffer;

		// Token: 0x04001CCA RID: 7370
		private int _notificationSequence;

		// Token: 0x04001CCB RID: 7371
		private EventNotificationType _notificationType;
	}
}
