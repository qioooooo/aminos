using System;

namespace System.Data.SqlClient
{
	// Token: 0x020002EF RID: 751
	internal class SqlNotification : MarshalByRefObject
	{
		// Token: 0x06002703 RID: 9987 RVA: 0x00287BF4 File Offset: 0x00286FF4
		internal SqlNotification(SqlNotificationInfo info, SqlNotificationSource source, SqlNotificationType type, string key)
		{
			this._info = info;
			this._source = source;
			this._type = type;
			this._key = key;
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06002704 RID: 9988 RVA: 0x00287C24 File Offset: 0x00287024
		internal SqlNotificationInfo Info
		{
			get
			{
				return this._info;
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06002705 RID: 9989 RVA: 0x00287C38 File Offset: 0x00287038
		internal string Key
		{
			get
			{
				return this._key;
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06002706 RID: 9990 RVA: 0x00287C4C File Offset: 0x0028704C
		internal SqlNotificationSource Source
		{
			get
			{
				return this._source;
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06002707 RID: 9991 RVA: 0x00287C60 File Offset: 0x00287060
		internal SqlNotificationType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x04001897 RID: 6295
		private readonly SqlNotificationInfo _info;

		// Token: 0x04001898 RID: 6296
		private readonly SqlNotificationSource _source;

		// Token: 0x04001899 RID: 6297
		private readonly SqlNotificationType _type;

		// Token: 0x0400189A RID: 6298
		private readonly string _key;
	}
}
