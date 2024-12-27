using System;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Web.Configuration;
using System.Web.DataAccess;

namespace System.Web.Caching
{
	// Token: 0x02000113 RID: 275
	internal static class SqlCacheDependencyManager
	{
		// Token: 0x06000CA7 RID: 3239 RVA: 0x000324C0 File Offset: 0x000314C0
		internal static string GetMoniterKey(string database, string table)
		{
			if (database.IndexOf(':') != -1)
			{
				database = database.Replace(":", "\\:");
			}
			if (table.IndexOf(':') != -1)
			{
				table = table.Replace(":", "\\:");
			}
			return "b" + database + ":" + table;
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x00032518 File Offset: 0x00031518
		internal static void Dispose(int waitTimeoutMs)
		{
			try
			{
				DateTime dateTime = DateTime.UtcNow.AddMilliseconds((double)waitTimeoutMs);
				SqlCacheDependencyManager.s_shutdown = true;
				if (SqlCacheDependencyManager.s_DatabaseNotifStates != null && SqlCacheDependencyManager.s_DatabaseNotifStates.Count > 0)
				{
					lock (SqlCacheDependencyManager.s_DatabaseNotifStates)
					{
						foreach (object obj in SqlCacheDependencyManager.s_DatabaseNotifStates)
						{
							object value = ((DictionaryEntry)obj).Value;
							if (value != null)
							{
								((DatabaseNotifState)value).Dispose();
							}
						}
					}
					while (SqlCacheDependencyManager.s_activePolling != 0)
					{
						Thread.Sleep(250);
						if (!Debugger.IsAttached && DateTime.UtcNow > dateTime)
						{
							break;
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x00032610 File Offset: 0x00031610
		internal static SqlCacheDependencyDatabase GetDatabaseConfig(string database)
		{
			SqlCacheDependencySection sqlCacheDependency = RuntimeConfig.GetAppConfig().SqlCacheDependency;
			object obj = sqlCacheDependency.Databases[database];
			if (obj == null)
			{
				throw new HttpException(SR.GetString("Database_not_found", new object[] { database }));
			}
			return (SqlCacheDependencyDatabase)obj;
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x0003265C File Offset: 0x0003165C
		internal static void InitPolling(string database)
		{
			SqlCacheDependencySection sqlCacheDependency = RuntimeConfig.GetAppConfig().SqlCacheDependency;
			if (!sqlCacheDependency.Enabled)
			{
				throw new ConfigurationErrorsException(SR.GetString("Polling_not_enabled_for_sql_cache"), sqlCacheDependency.ElementInformation.Properties["enabled"].Source, sqlCacheDependency.ElementInformation.Properties["enabled"].LineNumber);
			}
			SqlCacheDependencyDatabase databaseConfig = SqlCacheDependencyManager.GetDatabaseConfig(database);
			if (databaseConfig.PollTime == 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("Polltime_zero_for_database_sql_cache", new object[] { database }), databaseConfig.ElementInformation.Properties["pollTime"].Source, databaseConfig.ElementInformation.Properties["pollTime"].LineNumber);
			}
			if (SqlCacheDependencyManager.s_DatabaseNotifStates.ContainsKey(database))
			{
				return;
			}
			string connectionString = SqlConnectionHelper.GetConnectionString(databaseConfig.ConnectionStringName, true, true);
			if (connectionString == null || connectionString.Length < 1)
			{
				throw new ConfigurationErrorsException(SR.GetString("Connection_string_not_found", new object[] { databaseConfig.ConnectionStringName }), databaseConfig.ElementInformation.Properties["connectionStringName"].Source, databaseConfig.ElementInformation.Properties["connectionStringName"].LineNumber);
			}
			lock (SqlCacheDependencyManager.s_DatabaseNotifStates)
			{
				if (!SqlCacheDependencyManager.s_DatabaseNotifStates.ContainsKey(database))
				{
					DatabaseNotifState databaseNotifState = new DatabaseNotifState(database, connectionString, databaseConfig.PollTime);
					databaseNotifState._timer = new Timer(SqlCacheDependencyManager.s_timerCallback, databaseNotifState, 0, databaseConfig.PollTime);
					SqlCacheDependencyManager.s_DatabaseNotifStates.Add(database, databaseNotifState);
				}
			}
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x0003280C File Offset: 0x0003180C
		private static void PollCallback(object state)
		{
			using (new ApplicationImpersonationContext())
			{
				SqlCacheDependencyManager.PollDatabaseForChanges((DatabaseNotifState)state, true);
			}
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x00032848 File Offset: 0x00031848
		internal static void PollDatabaseForChanges(DatabaseNotifState dbState, bool fromTimer)
		{
			SqlDataReader sqlDataReader = null;
			SqlConnection sqlConnection = null;
			SqlCommand sqlCommand = null;
			CacheInternal cacheInternal = HttpRuntime.CacheInternal;
			bool flag = false;
			Exception ex = null;
			if (SqlCacheDependencyManager.s_shutdown)
			{
				return;
			}
			if (dbState._refCount == 0 && fromTimer && dbState._init)
			{
				return;
			}
			if (Interlocked.CompareExchange(ref dbState._rqInCallback, 1, 0) != 0)
			{
				if (fromTimer)
				{
					return;
				}
				HttpContext httpContext = HttpContext.Current;
				int num;
				if (httpContext == null)
				{
					num = 30;
				}
				else
				{
					num = Math.Max(httpContext.Timeout.Seconds / 3, 30);
				}
				DateTime dateTime = DateTime.UtcNow.Add(new TimeSpan(0, 0, num));
				while (Interlocked.CompareExchange(ref dbState._rqInCallback, 1, 0) != 0)
				{
					Thread.Sleep(250);
					if (SqlCacheDependencyManager.s_shutdown)
					{
						return;
					}
					if (!Debugger.IsAttached && DateTime.UtcNow > dateTime)
					{
						throw new HttpException(SR.GetString("Cant_connect_sql_cache_dep_database_polling", new object[] { dbState._database }));
					}
				}
			}
			try
			{
				try
				{
					Interlocked.Increment(ref SqlCacheDependencyManager.s_activePolling);
					dbState.GetConnection(out sqlConnection, out sqlCommand);
					sqlDataReader = sqlCommand.ExecuteReader();
					if (!SqlCacheDependencyManager.s_shutdown)
					{
						flag = true;
						Hashtable hashtable = (Hashtable)dbState._tables.Clone();
						while (sqlDataReader.Read())
						{
							string @string = sqlDataReader.GetString(0);
							int @int = sqlDataReader.GetInt32(1);
							string moniterKey = SqlCacheDependencyManager.GetMoniterKey(dbState._database, @string);
							object obj = cacheInternal[moniterKey];
							if (obj == null)
							{
								cacheInternal.UtcAdd(moniterKey, @int, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
								dbState._tables.Add(@string, null);
							}
							else if (@int != (int)obj)
							{
								cacheInternal.UtcInsert(moniterKey, @int, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
							}
							hashtable.Remove(@string);
						}
						foreach (object obj2 in hashtable.Keys)
						{
							dbState._tables.Remove((string)obj2);
							cacheInternal.Remove(SqlCacheDependencyManager.GetMoniterKey(dbState._database, (string)obj2));
						}
						if (dbState._pollSqlError != 0)
						{
							dbState._pollSqlError = 0;
						}
					}
				}
				catch (Exception ex2)
				{
					ex = ex2;
					SqlException ex3 = ex2 as SqlException;
					if (ex3 != null)
					{
						dbState._pollSqlError = ex3.Number;
					}
					else
					{
						dbState._pollSqlError = 0;
					}
				}
				finally
				{
					try
					{
						if (sqlDataReader != null)
						{
							sqlDataReader.Close();
						}
						dbState.ReleaseConnection(ref sqlConnection, ref sqlCommand, ex != null);
					}
					catch
					{
					}
					lock (dbState)
					{
						dbState._pollExpt = ex;
						if (dbState._notifEnabled && !flag && ex != null && dbState._pollSqlError == 2812)
						{
							foreach (object obj3 in dbState._tables.Keys)
							{
								try
								{
									cacheInternal.Remove(SqlCacheDependencyManager.GetMoniterKey(dbState._database, (string)obj3));
								}
								catch
								{
								}
							}
							dbState._tables.Clear();
						}
						dbState._notifEnabled = flag;
						dbState._utcTablesUpdated = DateTime.UtcNow;
					}
					if (!dbState._init)
					{
						dbState._init = true;
					}
					Interlocked.Decrement(ref SqlCacheDependencyManager.s_activePolling);
					Interlocked.Exchange(ref dbState._rqInCallback, 0);
				}
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x00032C80 File Offset: 0x00031C80
		internal static void EnsureTableIsRegisteredAndPolled(string database, string table)
		{
			bool flag = false;
			if (HttpRuntime.CacheInternal[SqlCacheDependencyManager.GetMoniterKey(database, table)] != null)
			{
				return;
			}
			SqlCacheDependencyManager.InitPolling(database);
			DatabaseNotifState databaseNotifState = (DatabaseNotifState)SqlCacheDependencyManager.s_DatabaseNotifStates[database];
			if (!databaseNotifState._init)
			{
				HttpContext httpContext = HttpContext.Current;
				int num;
				if (httpContext == null)
				{
					num = 30;
				}
				else
				{
					num = Math.Max(httpContext.Timeout.Seconds / 3, 30);
				}
				DateTime dateTime = DateTime.UtcNow.Add(new TimeSpan(0, 0, num));
				while (!databaseNotifState._init)
				{
					Thread.Sleep(250);
					if (!Debugger.IsAttached && DateTime.UtcNow > dateTime)
					{
						throw new HttpException(SR.GetString("Cant_connect_sql_cache_dep_database_polling", new object[] { database }));
					}
				}
			}
			int num2;
			Exception ex;
			bool notifEnabled;
			for (;;)
			{
				num2 = 0;
				DateTime utcTablesUpdated;
				lock (databaseNotifState)
				{
					ex = databaseNotifState._pollExpt;
					if (ex != null)
					{
						num2 = databaseNotifState._pollSqlError;
					}
					utcTablesUpdated = databaseNotifState._utcTablesUpdated;
					notifEnabled = databaseNotifState._notifEnabled;
				}
				if (ex == null && notifEnabled && databaseNotifState._tables.ContainsKey(table))
				{
					break;
				}
				if (flag || !(DateTime.UtcNow - utcTablesUpdated >= SqlCacheDependencyManager.OneSec))
				{
					goto IL_0133;
				}
				SqlCacheDependencyManager.UpdateDatabaseNotifState(database);
				flag = true;
			}
			return;
			IL_0133:
			if (num2 == 2812)
			{
				ex = null;
			}
			if (ex != null)
			{
				string text;
				if (num2 == 229 || num2 == 262)
				{
					text = "Permission_denied_database_polling";
				}
				else
				{
					text = "Cant_connect_sql_cache_dep_database_polling";
				}
				HttpException ex2 = new HttpException(SR.GetString(text, new object[] { database }), ex);
				ex2.SetFormatter(new UseLastUnhandledErrorFormatter(ex2));
				throw ex2;
			}
			if (!notifEnabled)
			{
				throw new DatabaseNotEnabledForNotificationException(SR.GetString("Database_not_enabled_for_notification", new object[] { database }));
			}
			throw new TableNotEnabledForNotificationException(SR.GetString("Table_not_enabled_for_notification", new object[] { table, database }));
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x00032E78 File Offset: 0x00031E78
		internal static void UpdateDatabaseNotifState(string database)
		{
			using (new ApplicationImpersonationContext())
			{
				SqlCacheDependencyManager.InitPolling(database);
				SqlCacheDependencyManager.PollDatabaseForChanges((DatabaseNotifState)SqlCacheDependencyManager.s_DatabaseNotifStates[database], false);
			}
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x00032EC4 File Offset: 0x00031EC4
		internal static void UpdateAllDatabaseNotifState()
		{
			lock (SqlCacheDependencyManager.s_DatabaseNotifStates)
			{
				foreach (object obj in SqlCacheDependencyManager.s_DatabaseNotifStates)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					DatabaseNotifState databaseNotifState = (DatabaseNotifState)dictionaryEntry.Value;
					if (databaseNotifState._init)
					{
						SqlCacheDependencyManager.UpdateDatabaseNotifState((string)dictionaryEntry.Key);
					}
				}
			}
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x00032F60 File Offset: 0x00031F60
		internal static DatabaseNotifState AddRef(string database)
		{
			DatabaseNotifState databaseNotifState = (DatabaseNotifState)SqlCacheDependencyManager.s_DatabaseNotifStates[database];
			Interlocked.Increment(ref databaseNotifState._refCount);
			return databaseNotifState;
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x00032F8B File Offset: 0x00031F8B
		internal static void Release(DatabaseNotifState dbState)
		{
			Interlocked.Decrement(ref dbState._refCount);
		}

		// Token: 0x04001464 RID: 5220
		internal const bool ENABLED_DEFAULT = true;

		// Token: 0x04001465 RID: 5221
		internal const int POLLTIME_DEFAULT = 60000;

		// Token: 0x04001466 RID: 5222
		internal const int TABLE_NAME_LENGTH = 128;

		// Token: 0x04001467 RID: 5223
		internal const int SQL_EXCEPTION_SP_NOT_FOUND = 2812;

		// Token: 0x04001468 RID: 5224
		internal const int SQL_EXCEPTION_PERMISSION_DENIED_ON_OBJECT = 229;

		// Token: 0x04001469 RID: 5225
		internal const int SQL_EXCEPTION_PERMISSION_DENIED_ON_DATABASE = 262;

		// Token: 0x0400146A RID: 5226
		internal const int SQL_EXCEPTION_PERMISSION_DENIED_ON_USER = 2760;

		// Token: 0x0400146B RID: 5227
		internal const int SQL_EXCEPTION_NO_GRANT_PERMISSION = 4613;

		// Token: 0x0400146C RID: 5228
		internal const int SQL_EXCEPTION_ADHOC = 50000;

		// Token: 0x0400146D RID: 5229
		private const char CacheKeySeparatorChar = ':';

		// Token: 0x0400146E RID: 5230
		private const string CacheKeySeparator = ":";

		// Token: 0x0400146F RID: 5231
		private const string CacheKeySeparatorEscaped = "\\:";

		// Token: 0x04001470 RID: 5232
		internal const string SQL_CUSTOM_ERROR_TABLE_NOT_FOUND = "00000001";

		// Token: 0x04001471 RID: 5233
		internal const string SQL_NOTIF_TABLE = "AspNet_SqlCacheTablesForChangeNotification";

		// Token: 0x04001472 RID: 5234
		internal const string SQL_POLLING_SP = "AspNet_SqlCachePollingStoredProcedure";

		// Token: 0x04001473 RID: 5235
		internal const string SQL_POLLING_SP_DBO = "dbo.AspNet_SqlCachePollingStoredProcedure";

		// Token: 0x04001474 RID: 5236
		internal static TimeSpan OneSec = new TimeSpan(0, 0, 1);

		// Token: 0x04001475 RID: 5237
		internal static Hashtable s_DatabaseNotifStates = new Hashtable();

		// Token: 0x04001476 RID: 5238
		private static TimerCallback s_timerCallback = new TimerCallback(SqlCacheDependencyManager.PollCallback);

		// Token: 0x04001477 RID: 5239
		private static int s_activePolling = 0;

		// Token: 0x04001478 RID: 5240
		private static bool s_shutdown = false;
	}
}
