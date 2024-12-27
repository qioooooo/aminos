using System;
using System.Collections;
using System.Data.SqlClient;
using System.Threading;
using System.Web.DataAccess;

namespace System.Web.Caching
{
	// Token: 0x02000112 RID: 274
	internal class DatabaseNotifState : IDisposable
	{
		// Token: 0x06000CA3 RID: 3235 RVA: 0x0003237A File Offset: 0x0003137A
		public void Dispose()
		{
			if (this._sqlConn != null)
			{
				this._sqlConn.Close();
				this._sqlConn = null;
			}
			if (this._timer != null)
			{
				this._timer.Dispose();
				this._timer = null;
			}
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x000323B0 File Offset: 0x000313B0
		internal DatabaseNotifState(string database, string connection, int polltime)
		{
			this._database = database;
			this._connectionString = connection;
			this._timer = null;
			this._tables = new Hashtable();
			this._pollExpt = null;
			this._utcTablesUpdated = DateTime.MinValue;
			if (polltime <= 5000)
			{
				this._poolConn = true;
			}
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x00032404 File Offset: 0x00031404
		internal void GetConnection(out SqlConnection sqlConn, out SqlCommand sqlCmd)
		{
			sqlConn = null;
			sqlCmd = null;
			if (this._sqlConn != null)
			{
				sqlConn = this._sqlConn;
				sqlCmd = this._sqlCmd;
				this._sqlConn = null;
				this._sqlCmd = null;
				return;
			}
			SqlConnectionHolder sqlConnectionHolder = null;
			try
			{
				sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._connectionString, true);
				sqlCmd = new SqlCommand("dbo.AspNet_SqlCachePollingStoredProcedure", sqlConnectionHolder.Connection);
				sqlConn = sqlConnectionHolder.Connection;
			}
			catch
			{
				if (sqlConnectionHolder != null)
				{
					sqlConnectionHolder.Close();
					sqlConnectionHolder = null;
				}
				sqlCmd = null;
				throw;
			}
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x0003248C File Offset: 0x0003148C
		internal void ReleaseConnection(ref SqlConnection sqlConn, ref SqlCommand sqlCmd, bool error)
		{
			if (sqlConn == null)
			{
				return;
			}
			if (this._poolConn && !error)
			{
				this._sqlConn = sqlConn;
				this._sqlCmd = sqlCmd;
			}
			else
			{
				sqlConn.Close();
			}
			sqlConn = null;
			sqlCmd = null;
		}

		// Token: 0x04001456 RID: 5206
		internal string _database;

		// Token: 0x04001457 RID: 5207
		internal string _connectionString;

		// Token: 0x04001458 RID: 5208
		internal int _rqInCallback;

		// Token: 0x04001459 RID: 5209
		internal bool _notifEnabled;

		// Token: 0x0400145A RID: 5210
		internal bool _init;

		// Token: 0x0400145B RID: 5211
		internal Timer _timer;

		// Token: 0x0400145C RID: 5212
		internal Hashtable _tables;

		// Token: 0x0400145D RID: 5213
		internal Exception _pollExpt;

		// Token: 0x0400145E RID: 5214
		internal int _pollSqlError;

		// Token: 0x0400145F RID: 5215
		internal SqlConnection _sqlConn;

		// Token: 0x04001460 RID: 5216
		internal SqlCommand _sqlCmd;

		// Token: 0x04001461 RID: 5217
		internal bool _poolConn;

		// Token: 0x04001462 RID: 5218
		internal DateTime _utcTablesUpdated;

		// Token: 0x04001463 RID: 5219
		internal int _refCount;
	}
}
