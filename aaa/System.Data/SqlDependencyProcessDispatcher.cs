using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Threading;
using System.Xml;

// Token: 0x020002E8 RID: 744
internal class SqlDependencyProcessDispatcher : MarshalByRefObject
{
	// Token: 0x1700061D RID: 1565
	// (get) Token: 0x060026CC RID: 9932 RVA: 0x00284DB0 File Offset: 0x002841B0
	internal int ObjectID
	{
		get
		{
			return this._objectID;
		}
	}

	// Token: 0x060026CD RID: 9933 RVA: 0x00284DC4 File Offset: 0x002841C4
	private SqlDependencyProcessDispatcher(object dummyVariable)
	{
		IntPtr intPtr;
		Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependencyProcessDispatcher|DEP> %d#", this.ObjectID);
		try
		{
			this._connectionContainers = new Dictionary<SqlDependencyProcessDispatcher.SqlConnectionContainerHashHelper, SqlDependencyProcessDispatcher.SqlConnectionContainer>();
			this._sqlDependencyPerAppDomainDispatchers = new Dictionary<string, SqlDependencyPerAppDomainDispatcher>();
		}
		finally
		{
			Bid.ScopeLeave(ref intPtr);
		}
	}

	// Token: 0x060026CE RID: 9934 RVA: 0x00284E38 File Offset: 0x00284238
	public SqlDependencyProcessDispatcher()
	{
		IntPtr intPtr;
		Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependencyProcessDispatcher|DEP> %d#", this.ObjectID);
		try
		{
		}
		finally
		{
			Bid.ScopeLeave(ref intPtr);
		}
	}

	// Token: 0x1700061E RID: 1566
	// (get) Token: 0x060026CF RID: 9935 RVA: 0x00284E94 File Offset: 0x00284294
	internal SqlDependencyProcessDispatcher SingletonProcessDispatcher
	{
		get
		{
			return SqlDependencyProcessDispatcher._staticInstance;
		}
	}

	// Token: 0x060026D0 RID: 9936 RVA: 0x00284EA8 File Offset: 0x002842A8
	private static SqlDependencyProcessDispatcher.SqlConnectionContainerHashHelper GetHashHelper(string connectionString, out SqlConnectionStringBuilder connectionStringBuilder, out DbConnectionPoolIdentity identity, out string user, string queue)
	{
		IntPtr intPtr;
		Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependencyProcessDispatcher.GetHashString|DEP> %d#, queue: %ls", SqlDependencyProcessDispatcher._staticInstance.ObjectID, queue);
		SqlDependencyProcessDispatcher.SqlConnectionContainerHashHelper sqlConnectionContainerHashHelper;
		try
		{
			connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
			connectionStringBuilder.AsynchronousProcessing = true;
			connectionStringBuilder.Pooling = false;
			connectionStringBuilder.Enlist = false;
			if (queue != null)
			{
				connectionStringBuilder.ApplicationName = queue;
			}
			if (connectionStringBuilder.IntegratedSecurity)
			{
				identity = DbConnectionPoolIdentity.GetCurrent();
				user = null;
			}
			else
			{
				identity = null;
				user = connectionStringBuilder.UserID;
			}
			sqlConnectionContainerHashHelper = new SqlDependencyProcessDispatcher.SqlConnectionContainerHashHelper(identity, connectionStringBuilder.ConnectionString, queue, connectionStringBuilder);
		}
		finally
		{
			Bid.ScopeLeave(ref intPtr);
		}
		return sqlConnectionContainerHashHelper;
	}

	// Token: 0x060026D1 RID: 9937 RVA: 0x00284F58 File Offset: 0x00284358
	public override object InitializeLifetimeService()
	{
		return null;
	}

	// Token: 0x060026D2 RID: 9938 RVA: 0x00284F68 File Offset: 0x00284368
	private void Invalidate(string server, SqlNotification sqlNotification)
	{
		IntPtr intPtr;
		Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependencyProcessDispatcher.Invalidate|DEP> %d#, server: %ls", this.ObjectID, server);
		try
		{
			lock (this._sqlDependencyPerAppDomainDispatchers)
			{
				foreach (KeyValuePair<string, SqlDependencyPerAppDomainDispatcher> keyValuePair in this._sqlDependencyPerAppDomainDispatchers)
				{
					SqlDependencyPerAppDomainDispatcher value = keyValuePair.Value;
					try
					{
						value.InvalidateServer(server, sqlNotification);
					}
					catch (Exception ex)
					{
						if (!ADP.IsCatchableExceptionType(ex))
						{
							throw;
						}
						ADP.TraceExceptionWithoutRethrow(ex);
					}
				}
			}
		}
		finally
		{
			Bid.ScopeLeave(ref intPtr);
		}
	}

	// Token: 0x060026D3 RID: 9939 RVA: 0x00285064 File Offset: 0x00284464
	internal void QueueAppDomainUnloading(string appDomainKey)
	{
		ThreadPool.QueueUserWorkItem(new WaitCallback(this.AppDomainUnloading), appDomainKey);
	}

	// Token: 0x060026D4 RID: 9940 RVA: 0x00285084 File Offset: 0x00284484
	private void AppDomainUnloading(object state)
	{
		IntPtr intPtr;
		Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependencyProcessDispatcher.AppDomainUnloading|DEP> %d#", this.ObjectID);
		try
		{
			string text = (string)state;
			lock (this._connectionContainers)
			{
				List<SqlDependencyProcessDispatcher.SqlConnectionContainerHashHelper> list = new List<SqlDependencyProcessDispatcher.SqlConnectionContainerHashHelper>();
				foreach (KeyValuePair<SqlDependencyProcessDispatcher.SqlConnectionContainerHashHelper, SqlDependencyProcessDispatcher.SqlConnectionContainer> keyValuePair in this._connectionContainers)
				{
					SqlDependencyProcessDispatcher.SqlConnectionContainer value = keyValuePair.Value;
					if (value.AppDomainUnload(text))
					{
						list.Add(value.HashHelper);
					}
				}
				foreach (SqlDependencyProcessDispatcher.SqlConnectionContainerHashHelper sqlConnectionContainerHashHelper in list)
				{
					this._connectionContainers.Remove(sqlConnectionContainerHashHelper);
				}
			}
			lock (this._sqlDependencyPerAppDomainDispatchers)
			{
				this._sqlDependencyPerAppDomainDispatchers.Remove(text);
			}
		}
		finally
		{
			Bid.ScopeLeave(ref intPtr);
		}
	}

	// Token: 0x060026D5 RID: 9941 RVA: 0x002851FC File Offset: 0x002845FC
	internal bool StartWithDefault(string connectionString, out string server, out DbConnectionPoolIdentity identity, out string user, out string database, ref string service, string appDomainKey, SqlDependencyPerAppDomainDispatcher dispatcher, out bool errorOccurred, out bool appDomainStart)
	{
		return this.Start(connectionString, out server, out identity, out user, out database, ref service, appDomainKey, dispatcher, out errorOccurred, out appDomainStart, true);
	}

	// Token: 0x060026D6 RID: 9942 RVA: 0x00285224 File Offset: 0x00284624
	internal bool Start(string connectionString, string queue, string appDomainKey, SqlDependencyPerAppDomainDispatcher dispatcher)
	{
		string text = null;
		bool flag = false;
		DbConnectionPoolIdentity dbConnectionPoolIdentity = null;
		return this.Start(connectionString, out text, out dbConnectionPoolIdentity, out text, out text, ref queue, appDomainKey, dispatcher, out flag, out flag, false);
	}

	// Token: 0x060026D7 RID: 9943 RVA: 0x00285250 File Offset: 0x00284650
	private bool Start(string connectionString, out string server, out DbConnectionPoolIdentity identity, out string user, out string database, ref string queueService, string appDomainKey, SqlDependencyPerAppDomainDispatcher dispatcher, out bool errorOccurred, out bool appDomainStart, bool useDefaults)
	{
		IntPtr intPtr;
		Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependencyProcessDispatcher.Start|DEP> %d#, queue: '%ls', appDomainKey: '%ls', perAppDomainDispatcher ID: '%d'", this.ObjectID, queueService, appDomainKey, dispatcher.ObjectID);
		bool flag2;
		try
		{
			server = null;
			identity = null;
			user = null;
			database = null;
			errorOccurred = false;
			appDomainStart = false;
			if (!this._sqlDependencyPerAppDomainDispatchers.ContainsKey(appDomainKey))
			{
				lock (this._sqlDependencyPerAppDomainDispatchers)
				{
					this._sqlDependencyPerAppDomainDispatchers[appDomainKey] = dispatcher;
				}
			}
			SqlConnectionStringBuilder sqlConnectionStringBuilder = null;
			SqlDependencyProcessDispatcher.SqlConnectionContainerHashHelper hashHelper = SqlDependencyProcessDispatcher.GetHashHelper(connectionString, out sqlConnectionStringBuilder, out identity, out user, queueService);
			bool flag = false;
			SqlDependencyProcessDispatcher.SqlConnectionContainer sqlConnectionContainer = null;
			lock (this._connectionContainers)
			{
				if (!this._connectionContainers.ContainsKey(hashHelper))
				{
					Bid.NotificationsTrace("<sc.SqlDependencyProcessDispatcher.Start|DEP> %d#, hashtable miss, creating new container.\n", this.ObjectID);
					sqlConnectionContainer = new SqlDependencyProcessDispatcher.SqlConnectionContainer(hashHelper, appDomainKey, useDefaults);
					this._connectionContainers.Add(hashHelper, sqlConnectionContainer);
					flag = true;
					appDomainStart = true;
				}
				else
				{
					sqlConnectionContainer = this._connectionContainers[hashHelper];
					Bid.NotificationsTrace("<sc.SqlDependencyProcessDispatcher.Start|DEP> %d#, hashtable hit, container: %d\n", this.ObjectID, sqlConnectionContainer.ObjectID);
					if (sqlConnectionContainer.InErrorState)
					{
						Bid.NotificationsTrace("<sc.SqlDependencyProcessDispatcher.Start|DEP> %d#, container: %d is in error state!\n", this.ObjectID, sqlConnectionContainer.ObjectID);
						errorOccurred = true;
					}
					else
					{
						sqlConnectionContainer.IncrementStartCount(appDomainKey, out appDomainStart);
					}
				}
			}
			if (useDefaults && !errorOccurred)
			{
				server = sqlConnectionContainer.Server;
				database = sqlConnectionContainer.Database;
				queueService = sqlConnectionContainer.Queue;
				Bid.NotificationsTrace("<sc.SqlDependencyProcessDispatcher.Start|DEP> %d#, default service: '%ls', server: '%ls', database: '%ls'\n", this.ObjectID, queueService, server, database);
			}
			Bid.NotificationsTrace("<sc.SqlDependencyProcessDispatcher.Start|DEP> %d#, started: %d\n", this.ObjectID, flag);
			flag2 = flag;
		}
		finally
		{
			Bid.ScopeLeave(ref intPtr);
		}
		return flag2;
	}

	// Token: 0x060026D8 RID: 9944 RVA: 0x0028542C File Offset: 0x0028482C
	internal bool Stop(string connectionString, out string server, out DbConnectionPoolIdentity identity, out string user, out string database, ref string queueService, string appDomainKey, out bool appDomainStop)
	{
		IntPtr intPtr;
		Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependencyProcessDispatcher.Stop|DEP> %d#, queue: '%ls'", this.ObjectID, queueService);
		bool flag2;
		try
		{
			server = null;
			identity = null;
			user = null;
			database = null;
			appDomainStop = false;
			SqlConnectionStringBuilder sqlConnectionStringBuilder = null;
			SqlDependencyProcessDispatcher.SqlConnectionContainerHashHelper hashHelper = SqlDependencyProcessDispatcher.GetHashHelper(connectionString, out sqlConnectionStringBuilder, out identity, out user, queueService);
			bool flag = false;
			lock (this._connectionContainers)
			{
				if (this._connectionContainers.ContainsKey(hashHelper))
				{
					SqlDependencyProcessDispatcher.SqlConnectionContainer sqlConnectionContainer = this._connectionContainers[hashHelper];
					Bid.NotificationsTrace("<sc.SqlDependencyProcessDispatcher.Stop|DEP> %d#, hashtable hit, container: %d\n", this.ObjectID, sqlConnectionContainer.ObjectID);
					server = sqlConnectionContainer.Server;
					database = sqlConnectionContainer.Database;
					queueService = sqlConnectionContainer.Queue;
					if (sqlConnectionContainer.Stop(appDomainKey, out appDomainStop))
					{
						flag = true;
						this._connectionContainers.Remove(hashHelper);
					}
				}
				else
				{
					Bid.NotificationsTrace("<sc.SqlDependencyProcessDispatcher.Stop|DEP> %d#, hashtable miss.\n", this.ObjectID);
				}
			}
			Bid.NotificationsTrace("<sc.SqlDependencyProcessDispatcher.Stop|DEP> %d#, stopped: %d\n", this.ObjectID, flag);
			flag2 = flag;
		}
		finally
		{
			Bid.ScopeLeave(ref intPtr);
		}
		return flag2;
	}

	// Token: 0x0400185F RID: 6239
	private static SqlDependencyProcessDispatcher _staticInstance = new SqlDependencyProcessDispatcher(null);

	// Token: 0x04001860 RID: 6240
	private Dictionary<SqlDependencyProcessDispatcher.SqlConnectionContainerHashHelper, SqlDependencyProcessDispatcher.SqlConnectionContainer> _connectionContainers;

	// Token: 0x04001861 RID: 6241
	private Dictionary<string, SqlDependencyPerAppDomainDispatcher> _sqlDependencyPerAppDomainDispatchers;

	// Token: 0x04001862 RID: 6242
	private readonly int _objectID = Interlocked.Increment(ref SqlDependencyProcessDispatcher._objectTypeCount);

	// Token: 0x04001863 RID: 6243
	private static int _objectTypeCount;

	// Token: 0x020002E9 RID: 745
	private class SqlConnectionContainer
	{
		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x060026DA RID: 9946 RVA: 0x00285570 File Offset: 0x00284970
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x060026DB RID: 9947 RVA: 0x00285584 File Offset: 0x00284984
		internal SqlConnectionContainer(SqlDependencyProcessDispatcher.SqlConnectionContainerHashHelper hashHelper, string appDomainKey, bool useDefaults)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlConnectionContainer|DEP> %d#, queue: '%ls'", this.ObjectID, hashHelper.Queue);
			try
			{
				this._hashHelper = hashHelper;
				string text = null;
				if (useDefaults)
				{
					text = Guid.NewGuid().ToString();
					this._queue = "SqlQueryNotificationService-" + text;
					this._hashHelper.ConnectionStringBuilder.ApplicationName = this._queue;
				}
				else
				{
					this._queue = this._hashHelper.Queue;
				}
				this._con = new SqlConnection(this._hashHelper.ConnectionStringBuilder.ConnectionString);
				SqlConnectionString sqlConnectionString = (SqlConnectionString)this._con.ConnectionOptions;
				sqlConnectionString.CreatePermissionSet().Assert();
				if (sqlConnectionString.LocalDBInstance != null)
				{
					LocalDBAPI.AssertLocalDBPermissions();
				}
				this._con.Open();
				this._cachedServer = this._con.DataSource;
				if (!this._con.IsYukonOrNewer)
				{
					throw SQL.NotificationsRequireYukon();
				}
				if (hashHelper.Identity != null)
				{
					this._windowsIdentity = DbConnectionPoolIdentity.GetCurrentWindowsIdentity();
				}
				this._escapedQueueName = SqlConnection.FixupDatabaseTransactionName(this._queue);
				this._appDomainKeyHash = new Dictionary<string, int>();
				this._com = new SqlCommand();
				this._com.Connection = this._con;
				this._com.CommandText = "select is_broker_enabled from sys.databases where database_id=db_id()";
				if (!(bool)this._com.ExecuteScalar())
				{
					throw SQL.SqlDependencyDatabaseBrokerDisabled();
				}
				this._conversationGuidParam = new SqlParameter("@p1", SqlDbType.UniqueIdentifier);
				this._timeoutParam = new SqlParameter("@p2", SqlDbType.Int);
				this._timeoutParam.Value = 0;
				this._com.Parameters.Add(this._timeoutParam);
				try
				{
					this._receiveQuery = "WAITFOR(RECEIVE TOP (1) message_type_name, conversation_handle, cast(message_body AS XML) as message_body from " + this._escapedQueueName + "), TIMEOUT @p2;";
					if (useDefaults)
					{
						this._sprocName = SqlConnection.FixupDatabaseTransactionName("SqlQueryNotificationStoredProcedure-" + text);
						this.CreateQueueAndService(false);
					}
					else
					{
						this._com.CommandText = this._receiveQuery;
						this._endConversationQuery = "END CONVERSATION @p1; ";
						this._concatQuery = this._endConversationQuery + this._receiveQuery;
					}
					bool flag = false;
					this.IncrementStartCount(appDomainKey, out flag);
					this.SynchronouslyQueryServiceBrokerQueue();
					this._timeoutParam.Value = this._defaultWaitforTimeout;
					this.AsynchronouslyQueryServiceBrokerQueue();
				}
				catch (Exception ex)
				{
					if (!ADP.IsCatchableExceptionType(ex))
					{
						throw;
					}
					ADP.TraceExceptionWithoutRethrow(ex);
					this.TearDownAndDispose();
					throw;
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x060026DC RID: 9948 RVA: 0x00285850 File Offset: 0x00284C50
		internal string Database
		{
			get
			{
				if (this._cachedDatabase == null)
				{
					this._cachedDatabase = this._con.Database;
				}
				return this._cachedDatabase;
			}
		}

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x060026DD RID: 9949 RVA: 0x0028587C File Offset: 0x00284C7C
		internal SqlDependencyProcessDispatcher.SqlConnectionContainerHashHelper HashHelper
		{
			get
			{
				return this._hashHelper;
			}
		}

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x060026DE RID: 9950 RVA: 0x00285890 File Offset: 0x00284C90
		internal bool InErrorState
		{
			get
			{
				return this._errorState;
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x060026DF RID: 9951 RVA: 0x002858A8 File Offset: 0x00284CA8
		internal string Queue
		{
			get
			{
				return this._queue;
			}
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x060026E0 RID: 9952 RVA: 0x002858BC File Offset: 0x00284CBC
		internal string Server
		{
			get
			{
				return this._cachedServer;
			}
		}

		// Token: 0x060026E1 RID: 9953 RVA: 0x002858D0 File Offset: 0x00284CD0
		internal bool AppDomainUnload(string appDomainKey)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlConnectionContainer.AppDomainUnload|DEP> %d#, AppDomainKey: '%ls'", this.ObjectID, appDomainKey);
			bool stopped;
			try
			{
				lock (this._appDomainKeyHash)
				{
					if (this._appDomainKeyHash.ContainsKey(appDomainKey))
					{
						Bid.NotificationsTrace("<sc.SqlConnectionContainer.AppDomainUnload|DEP> _appDomainKeyHash contained AppDomainKey: '%ls'.\n", appDomainKey);
						int i = this._appDomainKeyHash[appDomainKey];
						Bid.NotificationsTrace("<sc.SqlConnectionContainer.AppDomainUnload|DEP> _appDomainKeyHash for AppDomainKey: '%ls' count: '%d'.\n", appDomainKey, i);
						bool flag = false;
						while (i > 0)
						{
							this.Stop(appDomainKey, out flag);
							i--;
						}
						if (this._appDomainKeyHash.ContainsKey(appDomainKey))
						{
							Bid.NotificationsTrace("<sc.SqlConnectionContainer.AppDomainUnload|DEP|ERR> ERROR - after the Stop() loop, _appDomainKeyHash for AppDomainKey: '%ls' entry not removed from hash.  Count: %d'\n", appDomainKey, this._appDomainKeyHash[appDomainKey]);
						}
					}
					else
					{
						Bid.NotificationsTrace("<sc.SqlConnectionContainer.AppDomainUnload|DEP> _appDomainKeyHash did not contain AppDomainKey: '%ls'.\n", appDomainKey);
					}
				}
				Bid.NotificationsTrace("<sc.SqlConnectionContainer.AppDomainUnload|DEP> Exiting, _stopped: '%d'.\n", this._stopped);
				stopped = this._stopped;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return stopped;
		}

		// Token: 0x060026E2 RID: 9954 RVA: 0x002859DC File Offset: 0x00284DDC
		private void AsynchronouslyQueryServiceBrokerQueue()
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlConnectionContainer.AsynchronouslyQueryServiceBrokerQueue|DEP> %d#", this.ObjectID);
			try
			{
				AsyncCallback asyncCallback = new AsyncCallback(this.AsyncResultCallback);
				this._com.BeginExecuteReader(asyncCallback, null);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060026E3 RID: 9955 RVA: 0x00285A3C File Offset: 0x00284E3C
		private void AsyncResultCallback(IAsyncResult asyncResult)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlConnectionContainer.AsyncResultCallback|DEP> %d#", this.ObjectID);
			try
			{
				using (SqlDataReader sqlDataReader = this._com.EndExecuteReader(asyncResult))
				{
					this.ProcessNotificationResults(sqlDataReader);
				}
				if (!this._stop)
				{
					this.AsynchronouslyQueryServiceBrokerQueue();
				}
				else
				{
					this.TearDownAndDispose();
				}
			}
			catch (Exception ex)
			{
				if (!ADP.IsCatchableExceptionType(ex))
				{
					throw;
				}
				Bid.NotificationsTrace("<sc.SqlConnectionContainer.AsyncResultCallback|DEP> Exception occurred.\n");
				if (!this._stop)
				{
					ADP.TraceExceptionWithoutRethrow(ex);
				}
				if (this._stop)
				{
					this.TearDownAndDispose();
				}
				else
				{
					this._errorState = true;
					this.Restart(null);
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060026E4 RID: 9956 RVA: 0x00285B34 File Offset: 0x00284F34
		private void CreateQueueAndService(bool restart)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlConnectionContainer.CreateQueueAndService|DEP> %d#", this.ObjectID);
			try
			{
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = this._con;
				SqlTransaction sqlTransaction = null;
				try
				{
					sqlTransaction = this._con.BeginTransaction();
					sqlCommand.Transaction = sqlTransaction;
					sqlCommand.CommandText = string.Concat(new string[]
					{
						"CREATE PROCEDURE ", this._sprocName, " AS BEGIN BEGIN TRANSACTION; RECEIVE TOP(0) conversation_handle FROM ", this._escapedQueueName, "; IF (SELECT COUNT(*) FROM ", this._escapedQueueName, " WHERE message_type_name = 'http://schemas.microsoft.com/SQL/ServiceBroker/DialogTimer') > 0 BEGIN DROP SERVICE ", this._escapedQueueName, "; DROP QUEUE ", this._escapedQueueName,
						"; DROP PROCEDURE ", this._sprocName, "; END COMMIT TRANSACTION; END"
					});
					if (!restart)
					{
						sqlCommand.ExecuteNonQuery();
					}
					else
					{
						try
						{
							sqlCommand.ExecuteNonQuery();
						}
						catch (Exception ex)
						{
							if (!ADP.IsCatchableExceptionType(ex))
							{
								throw;
							}
							ADP.TraceExceptionWithoutRethrow(ex);
							try
							{
								if (sqlTransaction != null)
								{
									sqlTransaction.Rollback();
									sqlTransaction = null;
								}
							}
							catch (Exception ex2)
							{
								if (!ADP.IsCatchableExceptionType(ex2))
								{
									throw;
								}
								ADP.TraceExceptionWithoutRethrow(ex2);
							}
						}
						if (sqlTransaction == null)
						{
							sqlTransaction = this._con.BeginTransaction();
							sqlCommand.Transaction = sqlTransaction;
						}
					}
					sqlCommand.CommandText = string.Concat(new string[]
					{
						"IF OBJECT_ID('", this._queue, "') IS NULL BEGIN CREATE QUEUE ", this._escapedQueueName, " WITH ACTIVATION (PROCEDURE_NAME=", this._sprocName, ", MAX_QUEUE_READERS=1, EXECUTE AS OWNER); END; IF (SELECT COUNT(*) FROM sys.services WHERE NAME='", this._queue, "') = 0 BEGIN CREATE SERVICE ", this._escapedQueueName,
						" ON QUEUE ", this._escapedQueueName, " ([http://schemas.microsoft.com/SQL/Notifications/PostQueryNotification]); IF (SELECT COUNT(*) FROM sys.database_principals WHERE name='sql_dependency_subscriber' AND type='R') <> 0 BEGIN GRANT SEND ON SERVICE::", this._escapedQueueName, " TO sql_dependency_subscriber; END;  END; BEGIN DIALOG @dialog_handle FROM SERVICE ", this._escapedQueueName, " TO SERVICE '", this._queue, "'"
					});
					SqlParameter sqlParameter = new SqlParameter();
					sqlParameter.ParameterName = "@dialog_handle";
					sqlParameter.DbType = DbType.Guid;
					sqlParameter.Direction = ParameterDirection.Output;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.ExecuteNonQuery();
					this._dialogHandle = ((Guid)sqlParameter.Value).ToString();
					this._beginConversationQuery = "BEGIN CONVERSATION TIMER ('" + this._dialogHandle + "') TIMEOUT = 120; " + this._receiveQuery;
					this._com.CommandText = this._beginConversationQuery;
					this._endConversationQuery = "END CONVERSATION @p1; ";
					this._concatQuery = this._endConversationQuery + this._com.CommandText;
					sqlTransaction.Commit();
					sqlTransaction = null;
					this._serviceQueueCreated = true;
				}
				finally
				{
					if (sqlTransaction != null)
					{
						try
						{
							sqlTransaction.Rollback();
							sqlTransaction = null;
						}
						catch (Exception ex3)
						{
							if (!ADP.IsCatchableExceptionType(ex3))
							{
								throw;
							}
							ADP.TraceExceptionWithoutRethrow(ex3);
						}
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060026E5 RID: 9957 RVA: 0x00285E94 File Offset: 0x00285294
		internal void IncrementStartCount(string appDomainKey, out bool appDomainStart)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlConnectionContainer.IncrementStartCount|DEP> %d#", this.ObjectID);
			try
			{
				appDomainStart = false;
				int num = Interlocked.Increment(ref this._startCount);
				Bid.NotificationsTrace("<sc.SqlConnectionContainer.IncrementStartCount|DEP> %d#, incremented _startCount: %d\n", SqlDependencyProcessDispatcher._staticInstance.ObjectID, num);
				lock (this._appDomainKeyHash)
				{
					if (this._appDomainKeyHash.ContainsKey(appDomainKey))
					{
						this._appDomainKeyHash[appDomainKey] = this._appDomainKeyHash[appDomainKey] + 1;
						Bid.NotificationsTrace("<sc.SqlConnectionContainer.IncrementStartCount|DEP> _appDomainKeyHash contained AppDomainKey: '%ls', incremented count: '%d'.\n", appDomainKey, this._appDomainKeyHash[appDomainKey]);
					}
					else
					{
						this._appDomainKeyHash[appDomainKey] = 1;
						appDomainStart = true;
						Bid.NotificationsTrace("<sc.SqlConnectionContainer.IncrementStartCount|DEP> _appDomainKeyHash did not contain AppDomainKey: '%ls', added to hashtable and value set to 1.\n", appDomainKey);
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060026E6 RID: 9958 RVA: 0x00285F88 File Offset: 0x00285388
		private void ProcessNotificationResults(SqlDataReader reader)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlConnectionContainer.ProcessNotificationResults|DEP> %d#", this.ObjectID);
			try
			{
				Guid guid = Guid.Empty;
				try
				{
					if (!this._stop)
					{
						while (reader.Read())
						{
							Bid.NotificationsTrace("<sc.SqlConnectionContainer.ProcessNotificationResults|DEP> Row read.\n");
							string @string = reader.GetString(0);
							Bid.NotificationsTrace("<sc.SqlConnectionContainer.ProcessNotificationResults|DEP> msgType: '%ls'\n", @string);
							guid = reader.GetGuid(1);
							if (string.Compare(@string, "http://schemas.microsoft.com/SQL/Notifications/QueryNotification", StringComparison.OrdinalIgnoreCase) == 0)
							{
								SqlXml sqlXml = reader.GetSqlXml(2);
								if (sqlXml != null)
								{
									SqlNotification sqlNotification = SqlDependencyProcessDispatcher.SqlNotificationParser.ProcessMessage(sqlXml);
									if (sqlNotification != null)
									{
										string key = sqlNotification.Key;
										Bid.NotificationsTrace("<sc.SqlConnectionContainer.ProcessNotificationResults|DEP> Key: '%ls'\n", key);
										int num = key.IndexOf(';');
										if (num >= 0)
										{
											string text = key.Substring(0, num);
											SqlDependencyPerAppDomainDispatcher sqlDependencyPerAppDomainDispatcher = SqlDependencyProcessDispatcher._staticInstance._sqlDependencyPerAppDomainDispatchers[text];
											if (sqlDependencyPerAppDomainDispatcher != null)
											{
												try
												{
													sqlDependencyPerAppDomainDispatcher.InvalidateCommandID(sqlNotification);
													continue;
												}
												catch (Exception ex)
												{
													if (!ADP.IsCatchableExceptionType(ex))
													{
														throw;
													}
													ADP.TraceExceptionWithoutRethrow(ex);
													continue;
												}
											}
											Bid.NotificationsTrace("<sc.SqlConnectionContainer.ProcessNotificationResults|DEP|ERR> Received notification but do not have an associated PerAppDomainDispatcher!\n");
										}
										else
										{
											Bid.NotificationsTrace("<sc.SqlConnectionContainer.ProcessNotificationResults|DEP|ERR> Unexpected ID format received!\n");
										}
									}
									else
									{
										Bid.NotificationsTrace("<sc.SqlConnectionContainer.ProcessNotificationResults|DEP|ERR> Null notification returned from ProcessMessage!\n");
									}
								}
								else
								{
									Bid.NotificationsTrace("<sc.SqlConnectionContainer.ProcessNotificationResults|DEP|ERR> Null payload for QN notification type!\n");
								}
							}
							else
							{
								guid = Guid.Empty;
								Bid.NotificationsTrace("<sc.SqlConnectionContainer.ProcessNotificationResults|DEP> Unexpected message format received!\n");
							}
						}
					}
				}
				finally
				{
					if (guid == Guid.Empty)
					{
						this._com.CommandText = ((this._beginConversationQuery != null) ? this._beginConversationQuery : this._receiveQuery);
						if (this._com.Parameters.Count > 1)
						{
							this._com.Parameters.Remove(this._conversationGuidParam);
						}
					}
					else
					{
						this._com.CommandText = this._concatQuery;
						this._conversationGuidParam.Value = guid;
						if (this._com.Parameters.Count == 1)
						{
							this._com.Parameters.Add(this._conversationGuidParam);
						}
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060026E7 RID: 9959 RVA: 0x002861C4 File Offset: 0x002855C4
		private void Restart(object unused)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlConnectionContainer.Restart|DEP> %d#", this.ObjectID);
			try
			{
				lock (this)
				{
					if (!this._stop)
					{
						try
						{
							this._con.Close();
						}
						catch (Exception ex)
						{
							if (!ADP.IsCatchableExceptionType(ex))
							{
								throw;
							}
							ADP.TraceExceptionWithoutRethrow(ex);
						}
					}
				}
				lock (this)
				{
					if (!this._stop)
					{
						if (this._hashHelper.Identity != null)
						{
							WindowsImpersonationContext windowsImpersonationContext = null;
							RuntimeHelpers.PrepareConstrainedRegions();
							try
							{
								windowsImpersonationContext = this._windowsIdentity.Impersonate();
								this._con.Open();
								goto IL_00A7;
							}
							finally
							{
								if (windowsImpersonationContext != null)
								{
									windowsImpersonationContext.Undo();
								}
							}
						}
						this._con.Open();
					}
					IL_00A7:;
				}
				lock (this)
				{
					if (!this._stop && this._serviceQueueCreated)
					{
						bool flag = false;
						try
						{
							this.CreateQueueAndService(true);
						}
						catch (Exception ex2)
						{
							if (!ADP.IsCatchableExceptionType(ex2))
							{
								throw;
							}
							ADP.TraceExceptionWithoutRethrow(ex2);
							flag = true;
						}
						if (flag)
						{
							SqlDependencyProcessDispatcher._staticInstance.Invalidate(this.Server, new SqlNotification(SqlNotificationInfo.Error, SqlNotificationSource.Client, SqlNotificationType.Change, null));
						}
					}
				}
				lock (this)
				{
					if (!this._stop)
					{
						this._timeoutParam.Value = 0;
						this.SynchronouslyQueryServiceBrokerQueue();
						this._timeoutParam.Value = this._defaultWaitforTimeout;
						this.AsynchronouslyQueryServiceBrokerQueue();
						this._errorState = false;
						this._retryTimer = null;
					}
				}
				if (this._stop)
				{
					this.TearDownAndDispose();
				}
			}
			catch (Exception ex3)
			{
				if (!ADP.IsCatchableExceptionType(ex3))
				{
					throw;
				}
				ADP.TraceExceptionWithoutRethrow(ex3);
				try
				{
					SqlDependencyProcessDispatcher._staticInstance.Invalidate(this.Server, new SqlNotification(SqlNotificationInfo.Error, SqlNotificationSource.Client, SqlNotificationType.Change, null));
				}
				catch (Exception ex4)
				{
					if (!ADP.IsCatchableExceptionType(ex4))
					{
						throw;
					}
					ADP.TraceExceptionWithoutRethrow(ex4);
				}
				try
				{
					this._con.Close();
				}
				catch (Exception ex5)
				{
					if (!ADP.IsCatchableExceptionType(ex5))
					{
						throw;
					}
					ADP.TraceExceptionWithoutRethrow(ex5);
				}
				if (!this._stop)
				{
					this._retryTimer = new Timer(new TimerCallback(this.Restart), null, this._defaultWaitforTimeout, -1);
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060026E8 RID: 9960 RVA: 0x00286504 File Offset: 0x00285904
		internal bool Stop(string appDomainKey, out bool appDomainStop)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlConnectionContainer.Stop|DEP> %d#", this.ObjectID);
			bool stopped;
			try
			{
				appDomainStop = false;
				if (appDomainKey != null)
				{
					lock (this._appDomainKeyHash)
					{
						if (this._appDomainKeyHash.ContainsKey(appDomainKey))
						{
							int num = this._appDomainKeyHash[appDomainKey];
							Bid.NotificationsTrace("<sc.SqlConnectionContainer.Stop|DEP> _appDomainKeyHash contained AppDomainKey: '%ls', pre-decrement Count: '%d'.\n", appDomainKey, num);
							if (num > 0)
							{
								this._appDomainKeyHash[appDomainKey] = num - 1;
							}
							else
							{
								Bid.NotificationsTrace("<sc.SqlConnectionContainer.Stop|DEP}ERR> ERROR pre-decremented count <= 0!\n");
							}
							if (1 == num)
							{
								this._appDomainKeyHash.Remove(appDomainKey);
								appDomainStop = true;
							}
						}
						else
						{
							Bid.NotificationsTrace("<sc.SqlConnectionContainer.Stop|DEP|ERR> ERROR appDomainKey not null and not found in hash!\n");
						}
					}
				}
				if (Interlocked.Decrement(ref this._startCount) == 0)
				{
					Bid.NotificationsTrace("<sc.SqlConnectionContainer.Stop|DEP> Reached 0 count, cancelling and waiting.\n");
					lock (this)
					{
						try
						{
							this._com.Cancel();
						}
						catch (Exception ex)
						{
							if (!ADP.IsCatchableExceptionType(ex))
							{
								throw;
							}
							ADP.TraceExceptionWithoutRethrow(ex);
						}
						this._stop = true;
					}
					for (;;)
					{
						lock (this)
						{
							if (this._stopped)
							{
								break;
							}
							if (this._errorState)
							{
								Timer retryTimer = this._retryTimer;
								this._retryTimer = null;
								if (retryTimer != null)
								{
									retryTimer.Dispose();
								}
								this.TearDownAndDispose();
							}
						}
						Thread.Sleep(0);
					}
				}
				else
				{
					Bid.NotificationsTrace("<sc.SqlConnectionContainer.Stop|DEP> _startCount not 0 after decrement.  _startCount: '%d'.\n", this._startCount);
				}
				stopped = this._stopped;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return stopped;
		}

		// Token: 0x060026E9 RID: 9961 RVA: 0x002866F4 File Offset: 0x00285AF4
		private void SynchronouslyQueryServiceBrokerQueue()
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlConnectionContainer.SynchronouslyQueryServiceBrokerQueue|DEP> %d#", this.ObjectID);
			try
			{
				using (SqlDataReader sqlDataReader = this._com.ExecuteReader())
				{
					this.ProcessNotificationResults(sqlDataReader);
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060026EA RID: 9962 RVA: 0x00286770 File Offset: 0x00285B70
		private void TearDownAndDispose()
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlConnectionContainer.TearDownAndDispose|DEP> %d#", this.ObjectID);
			try
			{
				lock (this)
				{
					try
					{
						if (this._con.State != ConnectionState.Closed && ConnectionState.Broken != this._con.State)
						{
							if (this._com.Parameters.Count > 1)
							{
								try
								{
									this._com.CommandText = this._endConversationQuery;
									this._com.Parameters.Remove(this._timeoutParam);
									this._com.ExecuteNonQuery();
								}
								catch (Exception ex)
								{
									if (!ADP.IsCatchableExceptionType(ex))
									{
										throw;
									}
									ADP.TraceExceptionWithoutRethrow(ex);
								}
							}
							if (this._serviceQueueCreated && !this._errorState)
							{
								this._com.CommandText = string.Concat(new string[] { "BEGIN TRANSACTION; DROP SERVICE ", this._escapedQueueName, "; DROP QUEUE ", this._escapedQueueName, "; DROP PROCEDURE ", this._sprocName, "; COMMIT TRANSACTION;" });
								try
								{
									this._com.ExecuteNonQuery();
								}
								catch (Exception ex2)
								{
									if (!ADP.IsCatchableExceptionType(ex2))
									{
										throw;
									}
									ADP.TraceExceptionWithoutRethrow(ex2);
								}
							}
						}
					}
					finally
					{
						this._stopped = true;
						this._con.Dispose();
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x04001864 RID: 6244
		private SqlConnection _con;

		// Token: 0x04001865 RID: 6245
		private SqlCommand _com;

		// Token: 0x04001866 RID: 6246
		private SqlParameter _conversationGuidParam;

		// Token: 0x04001867 RID: 6247
		private SqlParameter _timeoutParam;

		// Token: 0x04001868 RID: 6248
		private SqlDependencyProcessDispatcher.SqlConnectionContainerHashHelper _hashHelper;

		// Token: 0x04001869 RID: 6249
		private WindowsIdentity _windowsIdentity;

		// Token: 0x0400186A RID: 6250
		private string _queue;

		// Token: 0x0400186B RID: 6251
		private string _receiveQuery;

		// Token: 0x0400186C RID: 6252
		private string _beginConversationQuery;

		// Token: 0x0400186D RID: 6253
		private string _endConversationQuery;

		// Token: 0x0400186E RID: 6254
		private string _concatQuery;

		// Token: 0x0400186F RID: 6255
		private readonly int _defaultWaitforTimeout = 60000;

		// Token: 0x04001870 RID: 6256
		private string _escapedQueueName;

		// Token: 0x04001871 RID: 6257
		private string _sprocName;

		// Token: 0x04001872 RID: 6258
		private string _dialogHandle;

		// Token: 0x04001873 RID: 6259
		private string _cachedServer;

		// Token: 0x04001874 RID: 6260
		private string _cachedDatabase;

		// Token: 0x04001875 RID: 6261
		private volatile bool _errorState;

		// Token: 0x04001876 RID: 6262
		private volatile bool _stop;

		// Token: 0x04001877 RID: 6263
		private volatile bool _stopped;

		// Token: 0x04001878 RID: 6264
		private volatile bool _serviceQueueCreated;

		// Token: 0x04001879 RID: 6265
		private int _startCount;

		// Token: 0x0400187A RID: 6266
		private Timer _retryTimer;

		// Token: 0x0400187B RID: 6267
		private Dictionary<string, int> _appDomainKeyHash;

		// Token: 0x0400187C RID: 6268
		private readonly int _objectID = Interlocked.Increment(ref SqlDependencyProcessDispatcher.SqlConnectionContainer._objectTypeCount);

		// Token: 0x0400187D RID: 6269
		private static int _objectTypeCount;
	}

	// Token: 0x020002EA RID: 746
	private class SqlNotificationParser
	{
		// Token: 0x060026EB RID: 9963 RVA: 0x00286948 File Offset: 0x00285D48
		internal static SqlNotification ProcessMessage(SqlXml xmlMessage)
		{
			SqlNotification sqlNotification;
			using (XmlReader xmlReader = xmlMessage.CreateReader())
			{
				string empty = string.Empty;
				SqlDependencyProcessDispatcher.SqlNotificationParser.MessageAttributes messageAttributes = SqlDependencyProcessDispatcher.SqlNotificationParser.MessageAttributes.None;
				SqlNotificationType sqlNotificationType = SqlNotificationType.Unknown;
				SqlNotificationInfo sqlNotificationInfo = SqlNotificationInfo.Unknown;
				SqlNotificationSource sqlNotificationSource = SqlNotificationSource.Unknown;
				string text = string.Empty;
				xmlReader.Read();
				if (XmlNodeType.Element == xmlReader.NodeType && "QueryNotification" == xmlReader.LocalName && 3 <= xmlReader.AttributeCount)
				{
					while (SqlDependencyProcessDispatcher.SqlNotificationParser.MessageAttributes.All != messageAttributes && xmlReader.MoveToNextAttribute())
					{
						try
						{
							string localName;
							if ((localName = xmlReader.LocalName) != null)
							{
								if (!(localName == "type"))
								{
									if (!(localName == "source"))
									{
										if (localName == "info")
										{
											try
											{
												string value = xmlReader.Value;
												string text2;
												if ((text2 = value) != null)
												{
													if (text2 == "set options")
													{
														sqlNotificationInfo = SqlNotificationInfo.Options;
														goto IL_01D2;
													}
													if (text2 == "previous invalid")
													{
														sqlNotificationInfo = SqlNotificationInfo.PreviousFire;
														goto IL_01D2;
													}
													if (text2 == "query template limit")
													{
														sqlNotificationInfo = SqlNotificationInfo.TemplateLimit;
														goto IL_01D2;
													}
												}
												SqlNotificationInfo sqlNotificationInfo2 = (SqlNotificationInfo)Enum.Parse(typeof(SqlNotificationInfo), value, true);
												if (Enum.IsDefined(typeof(SqlNotificationInfo), sqlNotificationInfo2))
												{
													sqlNotificationInfo = sqlNotificationInfo2;
												}
												IL_01D2:;
											}
											catch (Exception ex)
											{
												if (!ADP.IsCatchableExceptionType(ex))
												{
													throw;
												}
												ADP.TraceExceptionWithoutRethrow(ex);
											}
											messageAttributes |= SqlDependencyProcessDispatcher.SqlNotificationParser.MessageAttributes.Info;
										}
									}
									else
									{
										try
										{
											SqlNotificationSource sqlNotificationSource2 = (SqlNotificationSource)Enum.Parse(typeof(SqlNotificationSource), xmlReader.Value, true);
											if (Enum.IsDefined(typeof(SqlNotificationSource), sqlNotificationSource2))
											{
												sqlNotificationSource = sqlNotificationSource2;
											}
										}
										catch (Exception ex2)
										{
											if (!ADP.IsCatchableExceptionType(ex2))
											{
												throw;
											}
											ADP.TraceExceptionWithoutRethrow(ex2);
										}
										messageAttributes |= SqlDependencyProcessDispatcher.SqlNotificationParser.MessageAttributes.Source;
									}
								}
								else
								{
									try
									{
										SqlNotificationType sqlNotificationType2 = (SqlNotificationType)Enum.Parse(typeof(SqlNotificationType), xmlReader.Value, true);
										if (Enum.IsDefined(typeof(SqlNotificationType), sqlNotificationType2))
										{
											sqlNotificationType = sqlNotificationType2;
										}
									}
									catch (Exception ex3)
									{
										if (!ADP.IsCatchableExceptionType(ex3))
										{
											throw;
										}
										ADP.TraceExceptionWithoutRethrow(ex3);
									}
									messageAttributes |= SqlDependencyProcessDispatcher.SqlNotificationParser.MessageAttributes.Type;
								}
							}
						}
						catch (ArgumentException ex4)
						{
							ADP.TraceExceptionWithoutRethrow(ex4);
							Bid.Trace("<sc.SqlDependencyProcessDispatcher.ProcessMessage|DEP|ERR> Exception thrown - Enum.Parse failed to parse the value '%ls' of the attribute '%ls'.\n", xmlReader.Value, xmlReader.LocalName);
							return null;
						}
					}
					if (SqlDependencyProcessDispatcher.SqlNotificationParser.MessageAttributes.All != messageAttributes)
					{
						Bid.Trace("<sc.SqlDependencyProcessDispatcher.ProcessMessage|DEP|ERR> Not all expected attributes in Message; messageAttributes = '%d'.\n", (int)messageAttributes);
						sqlNotification = null;
					}
					else if (!xmlReader.Read())
					{
						Bid.Trace("<sc.SqlDependencyProcessDispatcher.ProcessMessage|DEP|ERR> unexpected Read failure on xml or unexpected structure of xml.\n");
						sqlNotification = null;
					}
					else if (XmlNodeType.Element != xmlReader.NodeType || string.Compare(xmlReader.LocalName, "Message", StringComparison.OrdinalIgnoreCase) != 0)
					{
						Bid.Trace("<sc.SqlDependencyProcessDispatcher.ProcessMessage|DEP|ERR> unexpected Read failure on xml or unexpected structure of xml.\n");
						sqlNotification = null;
					}
					else if (!xmlReader.Read())
					{
						Bid.Trace("<sc.SqlDependencyProcessDispatcher.ProcessMessage|DEP|ERR> unexpected Read failure on xml or unexpected structure of xml.\n");
						sqlNotification = null;
					}
					else if (xmlReader.NodeType != XmlNodeType.Text)
					{
						Bid.Trace("<sc.SqlDependencyProcessDispatcher.ProcessMessage|DEP|ERR> unexpected Read failure on xml or unexpected structure of xml.\n");
						sqlNotification = null;
					}
					else
					{
						using (XmlTextReader xmlTextReader = new XmlTextReader(xmlReader.Value, XmlNodeType.Element, null))
						{
							if (!xmlTextReader.Read())
							{
								Bid.Trace("<sc.SqlDependencyProcessDispatcher.ProcessMessage|DEP|ERR> unexpected Read failure on xml or unexpected structure of xml.\n");
								return null;
							}
							if (xmlTextReader.NodeType != XmlNodeType.Text)
							{
								Bid.Trace("<sc.SqlDependencyProcessDispatcher.ProcessMessage|DEP|ERR> unexpected Read failure on xml or unexpected structure of xml.\n");
								return null;
							}
							text = xmlTextReader.Value;
							xmlTextReader.Close();
						}
						sqlNotification = new SqlNotification(sqlNotificationInfo, sqlNotificationSource, sqlNotificationType, text);
					}
				}
				else
				{
					Bid.Trace("<sc.SqlDependencyProcessDispatcher.ProcessMessage|DEP|ERR> unexpected Read failure on xml or unexpected structure of xml.\n");
					sqlNotification = null;
				}
			}
			return sqlNotification;
		}

		// Token: 0x0400187E RID: 6270
		private const string RootNode = "QueryNotification";

		// Token: 0x0400187F RID: 6271
		private const string MessageNode = "Message";

		// Token: 0x04001880 RID: 6272
		private const string InfoAttribute = "info";

		// Token: 0x04001881 RID: 6273
		private const string SourceAttribute = "source";

		// Token: 0x04001882 RID: 6274
		private const string TypeAttribute = "type";

		// Token: 0x020002EB RID: 747
		[Flags]
		private enum MessageAttributes
		{
			// Token: 0x04001884 RID: 6276
			None = 0,
			// Token: 0x04001885 RID: 6277
			Type = 1,
			// Token: 0x04001886 RID: 6278
			Source = 2,
			// Token: 0x04001887 RID: 6279
			Info = 4,
			// Token: 0x04001888 RID: 6280
			All = 7
		}
	}

	// Token: 0x020002EC RID: 748
	private class SqlConnectionContainerHashHelper
	{
		// Token: 0x060026ED RID: 9965 RVA: 0x00286D34 File Offset: 0x00286134
		internal SqlConnectionContainerHashHelper(DbConnectionPoolIdentity identity, string connectionString, string queue, SqlConnectionStringBuilder connectionStringBuilder)
		{
			this._identity = identity;
			this._connectionString = connectionString;
			this._queue = queue;
			this._connectionStringBuilder = connectionStringBuilder;
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x060026EE RID: 9966 RVA: 0x00286D64 File Offset: 0x00286164
		internal SqlConnectionStringBuilder ConnectionStringBuilder
		{
			get
			{
				return this._connectionStringBuilder;
			}
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x060026EF RID: 9967 RVA: 0x00286D78 File Offset: 0x00286178
		internal DbConnectionPoolIdentity Identity
		{
			get
			{
				return this._identity;
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x060026F0 RID: 9968 RVA: 0x00286D8C File Offset: 0x0028618C
		internal string Queue
		{
			get
			{
				return this._queue;
			}
		}

		// Token: 0x060026F1 RID: 9969 RVA: 0x00286DA0 File Offset: 0x002861A0
		public override bool Equals(object value)
		{
			SqlDependencyProcessDispatcher.SqlConnectionContainerHashHelper sqlConnectionContainerHashHelper = (SqlDependencyProcessDispatcher.SqlConnectionContainerHashHelper)value;
			bool flag;
			if (sqlConnectionContainerHashHelper == null)
			{
				flag = false;
			}
			else if (this == sqlConnectionContainerHashHelper)
			{
				flag = true;
			}
			else if ((this._identity != null && sqlConnectionContainerHashHelper._identity == null) || (this._identity == null && sqlConnectionContainerHashHelper._identity != null))
			{
				flag = false;
			}
			else if (this._identity == null && sqlConnectionContainerHashHelper._identity == null)
			{
				flag = sqlConnectionContainerHashHelper._connectionString == this._connectionString && string.Equals(sqlConnectionContainerHashHelper._queue, this._queue, StringComparison.OrdinalIgnoreCase);
			}
			else
			{
				flag = sqlConnectionContainerHashHelper._identity.Equals(this._identity) && sqlConnectionContainerHashHelper._connectionString == this._connectionString && string.Equals(sqlConnectionContainerHashHelper._queue, this._queue, StringComparison.OrdinalIgnoreCase);
			}
			return flag;
		}

		// Token: 0x060026F2 RID: 9970 RVA: 0x00286E74 File Offset: 0x00286274
		public override int GetHashCode()
		{
			int num = 0;
			if (this._identity != null)
			{
				num = this._identity.GetHashCode();
			}
			if (this._queue != null)
			{
				num = this._connectionString.GetHashCode() + this._queue.GetHashCode() + num;
			}
			else
			{
				num = this._connectionString.GetHashCode() + num;
			}
			return num;
		}

		// Token: 0x04001889 RID: 6281
		private DbConnectionPoolIdentity _identity;

		// Token: 0x0400188A RID: 6282
		private string _connectionString;

		// Token: 0x0400188B RID: 6283
		private string _queue;

		// Token: 0x0400188C RID: 6284
		private SqlConnectionStringBuilder _connectionStringBuilder;
	}
}
