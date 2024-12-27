using System;

namespace System.Data.SqlClient
{
	// Token: 0x02000301 RID: 769
	public class SqlNotificationEventArgs : EventArgs
	{
		// Token: 0x06002819 RID: 10265 RVA: 0x0028D944 File Offset: 0x0028CD44
		public SqlNotificationEventArgs(SqlNotificationType type, SqlNotificationInfo info, SqlNotificationSource source)
		{
			this._info = info;
			this._source = source;
			this._type = type;
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x0600281A RID: 10266 RVA: 0x0028D96C File Offset: 0x0028CD6C
		public SqlNotificationType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x0600281B RID: 10267 RVA: 0x0028D980 File Offset: 0x0028CD80
		public SqlNotificationInfo Info
		{
			get
			{
				return this._info;
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x0600281C RID: 10268 RVA: 0x0028D994 File Offset: 0x0028CD94
		public SqlNotificationSource Source
		{
			get
			{
				return this._source;
			}
		}

		// Token: 0x04001928 RID: 6440
		private SqlNotificationType _type;

		// Token: 0x04001929 RID: 6441
		private SqlNotificationInfo _info;

		// Token: 0x0400192A RID: 6442
		private SqlNotificationSource _source;

		// Token: 0x0400192B RID: 6443
		internal static SqlNotificationEventArgs NotifyError = new SqlNotificationEventArgs(SqlNotificationType.Subscribe, SqlNotificationInfo.Error, SqlNotificationSource.Object);
	}
}
