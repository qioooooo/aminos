using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Security.Principal;
using System.Threading;
using System.Web.Configuration;
using System.Web.Util;

namespace System.Web.SessionState
{
	// Token: 0x02000378 RID: 888
	internal class SqlSessionStateStore : SessionStateStoreProviderBase
	{
		// Token: 0x06002B17 RID: 11031 RVA: 0x000BEF80 File Offset: 0x000BDF80
		internal SqlSessionStateStore()
		{
		}

		// Token: 0x06002B18 RID: 11032 RVA: 0x000BEF88 File Offset: 0x000BDF88
		internal override void Initialize(string name, NameValueCollection config, IPartitionResolver partitionResolver)
		{
			this._partitionResolver = partitionResolver;
			this.Initialize(name, config);
		}

		// Token: 0x06002B19 RID: 11033 RVA: 0x000BEF9C File Offset: 0x000BDF9C
		public override void Initialize(string name, NameValueCollection config)
		{
			if (string.IsNullOrEmpty(name))
			{
				name = "SQL Server Session State Provider";
			}
			base.Initialize(name, config);
			if (!SqlSessionStateStore.s_oneTimeInited)
			{
				SqlSessionStateStore.s_lock.AcquireWriterLock();
				try
				{
					if (!SqlSessionStateStore.s_oneTimeInited)
					{
						this.OneTimeInit();
					}
				}
				finally
				{
					SqlSessionStateStore.s_lock.ReleaseWriterLock();
				}
			}
			if (!SqlSessionStateStore.s_usePartition)
			{
				this._partitionInfo = SqlSessionStateStore.s_singlePartitionInfo;
			}
		}

		// Token: 0x06002B1A RID: 11034 RVA: 0x000BF010 File Offset: 0x000BE010
		private void OneTimeInit()
		{
			SessionStateSection sessionState = RuntimeConfig.GetAppConfig().SessionState;
			SqlSessionStateStore.s_configPartitionResolverType = sessionState.PartitionResolverType;
			SqlSessionStateStore.s_configSqlConnectionFileName = sessionState.ElementInformation.Properties["sqlConnectionString"].Source;
			SqlSessionStateStore.s_configSqlConnectionLineNumber = sessionState.ElementInformation.Properties["sqlConnectionString"].LineNumber;
			SqlSessionStateStore.s_configAllowCustomSqlDatabase = sessionState.AllowCustomSqlDatabase;
			if (this._partitionResolver == null)
			{
				string sqlConnectionString = sessionState.SqlConnectionString;
				SessionStateModule.ReadConnectionString(sessionState, ref sqlConnectionString, "sqlConnectionString");
				SqlSessionStateStore.s_singlePartitionInfo = (SqlSessionStateStore.SqlPartitionInfo)this.CreatePartitionInfo(sqlConnectionString);
			}
			else
			{
				SqlSessionStateStore.s_usePartition = true;
				SqlSessionStateStore.s_partitionManager = new PartitionManager(new CreatePartitionInfo(this.CreatePartitionInfo));
			}
			SqlSessionStateStore.s_commandTimeout = (int)sessionState.SqlCommandTimeout.TotalSeconds;
			SqlSessionStateStore.s_isClearPoolInProgress = 0;
			SqlSessionStateStore.s_onAppDomainUnload = new EventHandler(this.OnAppDomainUnload);
			Thread.GetDomain().DomainUnload += SqlSessionStateStore.s_onAppDomainUnload;
			SqlSessionStateStore.s_oneTimeInited = true;
		}

		// Token: 0x06002B1B RID: 11035 RVA: 0x000BF107 File Offset: 0x000BE107
		private void OnAppDomainUnload(object unusedObject, EventArgs unusedEventArgs)
		{
			Thread.GetDomain().DomainUnload -= SqlSessionStateStore.s_onAppDomainUnload;
			if (this._partitionResolver == null)
			{
				if (SqlSessionStateStore.s_singlePartitionInfo != null)
				{
					SqlSessionStateStore.s_singlePartitionInfo.Dispose();
					return;
				}
			}
			else if (SqlSessionStateStore.s_partitionManager != null)
			{
				SqlSessionStateStore.s_partitionManager.Dispose();
			}
		}

		// Token: 0x06002B1C RID: 11036 RVA: 0x000BF144 File Offset: 0x000BE144
		internal IPartitionInfo CreatePartitionInfo(string sqlConnectionString)
		{
			string text = null;
			SqlConnection sqlConnection;
			try
			{
				sqlConnection = new SqlConnection(sqlConnectionString);
			}
			catch (Exception ex)
			{
				if (SqlSessionStateStore.s_usePartition)
				{
					HttpException ex2 = new HttpException(SR.GetString("Error_parsing_sql_partition_resolver_string", new object[]
					{
						SqlSessionStateStore.s_configPartitionResolverType,
						ex.Message
					}), ex);
					ex2.SetFormatter(new UseLastUnhandledErrorFormatter(ex2));
					throw ex2;
				}
				throw new ConfigurationErrorsException(SR.GetString("Error_parsing_session_sqlConnectionString", new object[] { ex.Message }), ex, SqlSessionStateStore.s_configSqlConnectionFileName, SqlSessionStateStore.s_configSqlConnectionLineNumber);
			}
			string text2 = sqlConnection.Database;
			SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(sqlConnectionString);
			if (string.IsNullOrEmpty(text2))
			{
				text2 = sqlConnectionStringBuilder.AttachDBFilename;
				text = text2;
			}
			if (!string.IsNullOrEmpty(text2))
			{
				if (!SqlSessionStateStore.s_configAllowCustomSqlDatabase)
				{
					if (SqlSessionStateStore.s_usePartition)
					{
						throw new HttpException(SR.GetString("No_database_allowed_in_sql_partition_resolver_string", new object[]
						{
							SqlSessionStateStore.s_configPartitionResolverType,
							sqlConnection.DataSource,
							text2
						}));
					}
					throw new ConfigurationErrorsException(SR.GetString("No_database_allowed_in_sqlConnectionString"), SqlSessionStateStore.s_configSqlConnectionFileName, SqlSessionStateStore.s_configSqlConnectionLineNumber);
				}
				else if (text != null)
				{
					HttpRuntime.CheckFilePermission(text, true);
				}
			}
			else
			{
				sqlConnectionString += ";Initial Catalog=ASPState";
			}
			return new SqlSessionStateStore.SqlPartitionInfo(new ResourcePool(new TimeSpan(0, 0, 5), int.MaxValue), sqlConnectionStringBuilder.IntegratedSecurity, sqlConnectionString);
		}

		// Token: 0x06002B1D RID: 11037 RVA: 0x000BF2A0 File Offset: 0x000BE2A0
		public override bool SetItemExpireCallback(SessionStateItemExpireCallback expireCallback)
		{
			return false;
		}

		// Token: 0x06002B1E RID: 11038 RVA: 0x000BF2A3 File Offset: 0x000BE2A3
		public override void Dispose()
		{
		}

		// Token: 0x06002B1F RID: 11039 RVA: 0x000BF2A5 File Offset: 0x000BE2A5
		public override void InitializeRequest(HttpContext context)
		{
			this._rqContext = context;
			this._rqOrigStreamLen = 0;
			if (SqlSessionStateStore.s_usePartition)
			{
				this._partitionInfo = null;
			}
		}

		// Token: 0x06002B20 RID: 11040 RVA: 0x000BF2C3 File Offset: 0x000BE2C3
		public override void EndRequest(HttpContext context)
		{
			this._rqContext = null;
		}

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x06002B21 RID: 11041 RVA: 0x000BF2CC File Offset: 0x000BE2CC
		public bool KnowForSureNotUsingIntegratedSecurity
		{
			get
			{
				return this._partitionInfo != null && !this._partitionInfo.UseIntegratedSecurity;
			}
		}

		// Token: 0x06002B22 RID: 11042 RVA: 0x000BF2E8 File Offset: 0x000BE2E8
		private bool CanUsePooling()
		{
			bool flag;
			if (this.KnowForSureNotUsingIntegratedSecurity)
			{
				flag = true;
			}
			else if (this._rqContext == null)
			{
				flag = false;
			}
			else if (!this._rqContext.IsClientImpersonationConfigured)
			{
				flag = true;
			}
			else if (HttpRuntime.IsOnUNCShareInternal)
			{
				flag = false;
			}
			else
			{
				string serverVariable = this._rqContext.WorkerRequest.GetServerVariable("LOGON_USER");
				flag = string.IsNullOrEmpty(serverVariable);
			}
			return flag;
		}

		// Token: 0x06002B23 RID: 11043 RVA: 0x000BF350 File Offset: 0x000BE350
		private SqlSessionStateStore.SqlStateConnection GetConnection(string id, ref bool usePooling)
		{
			SqlSessionStateStore.SqlStateConnection sqlStateConnection = null;
			if (this._partitionInfo == null)
			{
				this._partitionInfo = (SqlSessionStateStore.SqlPartitionInfo)SqlSessionStateStore.s_partitionManager.GetPartition(this._partitionResolver, id);
			}
			usePooling = this.CanUsePooling();
			if (usePooling)
			{
				sqlStateConnection = (SqlSessionStateStore.SqlStateConnection)this._partitionInfo.RetrieveResource();
				if (sqlStateConnection != null && (sqlStateConnection.Connection.State & ConnectionState.Open) == ConnectionState.Closed)
				{
					sqlStateConnection.Dispose();
					sqlStateConnection = null;
				}
			}
			if (sqlStateConnection == null)
			{
				if (SqlSessionStateStore.IsRetryEnabled)
				{
					sqlStateConnection = new SqlSessionStateStore.SqlStateConnection(this._partitionInfo, AppSettings.SqlSessionRetryInterval);
				}
				else
				{
					sqlStateConnection = new SqlSessionStateStore.SqlStateConnection(this._partitionInfo);
				}
			}
			return sqlStateConnection;
		}

		// Token: 0x06002B24 RID: 11044 RVA: 0x000BF3E4 File Offset: 0x000BE3E4
		private void DisposeOrReuseConnection(ref SqlSessionStateStore.SqlStateConnection conn, bool usePooling)
		{
			try
			{
				if (conn != null)
				{
					if (usePooling)
					{
						this._partitionInfo.StoreResource(conn);
						conn = null;
					}
				}
			}
			finally
			{
				if (conn != null)
				{
					conn.Dispose();
				}
			}
		}

		// Token: 0x06002B25 RID: 11045 RVA: 0x000BF428 File Offset: 0x000BE428
		internal static void ThrowSqlConnectionException(SqlConnection conn, Exception e)
		{
			if (SqlSessionStateStore.s_usePartition)
			{
				throw new HttpException(SR.GetString("Cant_connect_sql_session_database_partition_resolver", new object[]
				{
					SqlSessionStateStore.s_configPartitionResolverType,
					conn.DataSource,
					conn.Database
				}));
			}
			throw new HttpException(SR.GetString("Cant_connect_sql_session_database"), e);
		}

		// Token: 0x06002B26 RID: 11046 RVA: 0x000BF480 File Offset: 0x000BE480
		private SessionStateStoreData DoGet(HttpContext context, string id, bool getExclusive, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actionFlags)
		{
			MemoryStream memoryStream = null;
			bool flag = false;
			SqlSessionStateStore.SqlStateConnection sqlStateConnection = null;
			SqlCommand sqlCommand = null;
			bool flag2 = true;
			locked = false;
			lockId = null;
			lockAge = TimeSpan.Zero;
			actionFlags = SessionStateActions.None;
			byte[] array = null;
			SqlDataReader sqlDataReader = null;
			sqlStateConnection = this.GetConnection(id, ref flag2);
			if ((this._partitionInfo.SupportFlags & SqlSessionStateStore.SupportFlags.GetLockAge) != SqlSessionStateStore.SupportFlags.None)
			{
				flag = true;
			}
			SessionStateStoreData sessionStateStoreData;
			try
			{
				if (getExclusive)
				{
					sqlCommand = sqlStateConnection.TempGetExclusive;
				}
				else
				{
					sqlCommand = sqlStateConnection.TempGet;
				}
				sqlCommand.Parameters[0].Value = id + this._partitionInfo.AppSuffix;
				sqlCommand.Parameters[1].Value = Convert.DBNull;
				sqlCommand.Parameters[2].Value = Convert.DBNull;
				sqlCommand.Parameters[3].Value = Convert.DBNull;
				sqlCommand.Parameters[4].Value = Convert.DBNull;
				sqlCommand.Parameters[5].Value = Convert.DBNull;
				try
				{
					if (SqlSessionStateStore.IsRetryEnabled)
					{
						sqlDataReader = SqlSessionStateStore.SqlExecuteReaderWithRetry(sqlCommand, CommandBehavior.Default);
					}
					else
					{
						sqlDataReader = sqlCommand.ExecuteReader();
					}
					if (sqlDataReader != null)
					{
						try
						{
							if (sqlDataReader.Read())
							{
								array = (byte[])sqlDataReader[0];
							}
						}
						finally
						{
							sqlDataReader.Close();
						}
					}
				}
				catch (Exception ex)
				{
					SqlSessionStateStore.ThrowSqlConnectionException(sqlCommand.Connection, ex);
				}
				if (Convert.IsDBNull(sqlCommand.Parameters[2].Value))
				{
					sessionStateStoreData = null;
				}
				else
				{
					locked = (bool)sqlCommand.Parameters[2].Value;
					lockId = (int)sqlCommand.Parameters[4].Value;
					if (locked)
					{
						if (flag)
						{
							lockAge = new TimeSpan(0, 0, (int)sqlCommand.Parameters[3].Value);
						}
						else
						{
							DateTime dateTime = (DateTime)sqlCommand.Parameters[3].Value;
							lockAge = DateTime.Now - dateTime;
						}
						if (lockAge > new TimeSpan(0, 0, 31536000))
						{
							lockAge = TimeSpan.Zero;
						}
						sessionStateStoreData = null;
					}
					else
					{
						actionFlags = (SessionStateActions)sqlCommand.Parameters[5].Value;
						if (array == null)
						{
							array = (byte[])sqlCommand.Parameters[1].Value;
						}
						this.DisposeOrReuseConnection(ref sqlStateConnection, flag2);
						SessionStateStoreData sessionStateStoreData2;
						try
						{
							memoryStream = new MemoryStream(array);
							sessionStateStoreData2 = SessionStateUtility.Deserialize(context, memoryStream);
							this._rqOrigStreamLen = (int)memoryStream.Position;
						}
						finally
						{
							if (memoryStream != null)
							{
								memoryStream.Close();
							}
						}
						sessionStateStoreData = sessionStateStoreData2;
					}
				}
			}
			finally
			{
				this.DisposeOrReuseConnection(ref sqlStateConnection, flag2);
			}
			return sessionStateStoreData;
		}

		// Token: 0x06002B27 RID: 11047 RVA: 0x000BF794 File Offset: 0x000BE794
		public override SessionStateStoreData GetItem(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actionFlags)
		{
			SessionIDManager.CheckIdLength(id, true);
			return this.DoGet(context, id, false, out locked, out lockAge, out lockId, out actionFlags);
		}

		// Token: 0x06002B28 RID: 11048 RVA: 0x000BF7AE File Offset: 0x000BE7AE
		public override SessionStateStoreData GetItemExclusive(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actionFlags)
		{
			SessionIDManager.CheckIdLength(id, true);
			return this.DoGet(context, id, true, out locked, out lockAge, out lockId, out actionFlags);
		}

		// Token: 0x06002B29 RID: 11049 RVA: 0x000BF7C8 File Offset: 0x000BE7C8
		public override void ReleaseItemExclusive(HttpContext context, string id, object lockId)
		{
			bool flag = true;
			SqlSessionStateStore.SqlStateConnection sqlStateConnection = null;
			int num = (int)lockId;
			try
			{
				SessionIDManager.CheckIdLength(id, true);
				sqlStateConnection = this.GetConnection(id, ref flag);
				try
				{
					SqlCommand tempReleaseExclusive = sqlStateConnection.TempReleaseExclusive;
					tempReleaseExclusive.Parameters[0].Value = id + this._partitionInfo.AppSuffix;
					tempReleaseExclusive.Parameters[1].Value = num;
					if (SqlSessionStateStore.IsRetryEnabled)
					{
						SqlSessionStateStore.SqlExecuteNonQueryWithRetry(tempReleaseExclusive, false, null);
					}
					else
					{
						tempReleaseExclusive.ExecuteNonQuery();
					}
				}
				catch (Exception ex)
				{
					SqlSessionStateStore.ThrowSqlConnectionException(sqlStateConnection.Connection, ex);
				}
			}
			finally
			{
				this.DisposeOrReuseConnection(ref sqlStateConnection, flag);
			}
		}

		// Token: 0x06002B2A RID: 11050 RVA: 0x000BF888 File Offset: 0x000BE888
		public override void SetAndReleaseItemExclusive(HttpContext context, string id, SessionStateStoreData item, object lockId, bool newItem)
		{
			bool flag = true;
			SqlSessionStateStore.SqlStateConnection sqlStateConnection = null;
			try
			{
				SessionIDManager.CheckIdLength(id, true);
				byte[] array;
				int num;
				try
				{
					SessionStateUtility.SerializeStoreData(item, 7000, out array, out num);
				}
				catch
				{
					if (!newItem)
					{
						this.ReleaseItemExclusive(context, id, lockId);
					}
					throw;
				}
				int num2;
				if (lockId == null)
				{
					num2 = 0;
				}
				else
				{
					num2 = (int)lockId;
				}
				sqlStateConnection = this.GetConnection(id, ref flag);
				SqlCommand sqlCommand;
				if (!newItem)
				{
					if (num <= 7000)
					{
						if (this._rqOrigStreamLen <= 7000)
						{
							sqlCommand = sqlStateConnection.TempUpdateShort;
						}
						else
						{
							sqlCommand = sqlStateConnection.TempUpdateShortNullLong;
						}
					}
					else if (this._rqOrigStreamLen <= 7000)
					{
						sqlCommand = sqlStateConnection.TempUpdateLongNullShort;
					}
					else
					{
						sqlCommand = sqlStateConnection.TempUpdateLong;
					}
				}
				else if (num <= 7000)
				{
					sqlCommand = sqlStateConnection.TempInsertShort;
				}
				else
				{
					sqlCommand = sqlStateConnection.TempInsertLong;
				}
				sqlCommand.Parameters[0].Value = id + this._partitionInfo.AppSuffix;
				sqlCommand.Parameters[1].Size = num;
				sqlCommand.Parameters[1].Value = array;
				sqlCommand.Parameters[2].Value = item.Timeout;
				if (!newItem)
				{
					sqlCommand.Parameters[3].Value = num2;
				}
				try
				{
					if (SqlSessionStateStore.IsRetryEnabled)
					{
						SqlSessionStateStore.SqlExecuteNonQueryWithRetry(sqlCommand, newItem, id);
					}
					else
					{
						sqlCommand.ExecuteNonQuery();
					}
				}
				catch (Exception ex)
				{
					this.HandleInsertException(sqlStateConnection.Connection, ex, newItem, id);
				}
			}
			finally
			{
				this.DisposeOrReuseConnection(ref sqlStateConnection, flag);
			}
		}

		// Token: 0x06002B2B RID: 11051 RVA: 0x000BFA54 File Offset: 0x000BEA54
		public override void RemoveItem(HttpContext context, string id, object lockId, SessionStateStoreData item)
		{
			bool flag = true;
			SqlSessionStateStore.SqlStateConnection sqlStateConnection = null;
			int num = (int)lockId;
			try
			{
				SessionIDManager.CheckIdLength(id, true);
				sqlStateConnection = this.GetConnection(id, ref flag);
				try
				{
					SqlCommand tempRemove = sqlStateConnection.TempRemove;
					tempRemove.Parameters[0].Value = id + this._partitionInfo.AppSuffix;
					tempRemove.Parameters[1].Value = num;
					if (SqlSessionStateStore.IsRetryEnabled)
					{
						SqlSessionStateStore.SqlExecuteNonQueryWithRetry(tempRemove, false, null);
					}
					else
					{
						tempRemove.ExecuteNonQuery();
					}
				}
				catch (Exception ex)
				{
					SqlSessionStateStore.ThrowSqlConnectionException(sqlStateConnection.Connection, ex);
				}
			}
			finally
			{
				this.DisposeOrReuseConnection(ref sqlStateConnection, flag);
			}
		}

		// Token: 0x06002B2C RID: 11052 RVA: 0x000BFB14 File Offset: 0x000BEB14
		public override void ResetItemTimeout(HttpContext context, string id)
		{
			bool flag = true;
			SqlSessionStateStore.SqlStateConnection sqlStateConnection = null;
			try
			{
				SessionIDManager.CheckIdLength(id, true);
				sqlStateConnection = this.GetConnection(id, ref flag);
				try
				{
					SqlCommand tempResetTimeout = sqlStateConnection.TempResetTimeout;
					tempResetTimeout.Parameters[0].Value = id + this._partitionInfo.AppSuffix;
					if (SqlSessionStateStore.IsRetryEnabled)
					{
						SqlSessionStateStore.SqlExecuteNonQueryWithRetry(tempResetTimeout, false, null);
					}
					else
					{
						tempResetTimeout.ExecuteNonQuery();
					}
				}
				catch (Exception ex)
				{
					SqlSessionStateStore.ThrowSqlConnectionException(sqlStateConnection.Connection, ex);
				}
			}
			finally
			{
				this.DisposeOrReuseConnection(ref sqlStateConnection, flag);
			}
		}

		// Token: 0x06002B2D RID: 11053 RVA: 0x000BFBB4 File Offset: 0x000BEBB4
		public override SessionStateStoreData CreateNewStoreData(HttpContext context, int timeout)
		{
			return SessionStateUtility.CreateLegitStoreData(context, null, null, timeout);
		}

		// Token: 0x06002B2E RID: 11054 RVA: 0x000BFBC0 File Offset: 0x000BEBC0
		public override void CreateUninitializedItem(HttpContext context, string id, int timeout)
		{
			bool flag = true;
			SqlSessionStateStore.SqlStateConnection sqlStateConnection = null;
			try
			{
				SessionIDManager.CheckIdLength(id, true);
				byte[] array;
				int num;
				SessionStateUtility.SerializeStoreData(this.CreateNewStoreData(context, timeout), 7000, out array, out num);
				sqlStateConnection = this.GetConnection(id, ref flag);
				try
				{
					SqlCommand tempInsertUninitializedItem = sqlStateConnection.TempInsertUninitializedItem;
					tempInsertUninitializedItem.Parameters[0].Value = id + this._partitionInfo.AppSuffix;
					tempInsertUninitializedItem.Parameters[1].Size = num;
					tempInsertUninitializedItem.Parameters[1].Value = array;
					tempInsertUninitializedItem.Parameters[2].Value = timeout;
					if (SqlSessionStateStore.IsRetryEnabled)
					{
						SqlSessionStateStore.SqlExecuteNonQueryWithRetry(tempInsertUninitializedItem, true, id);
					}
					else
					{
						tempInsertUninitializedItem.ExecuteNonQuery();
					}
				}
				catch (Exception ex)
				{
					this.HandleInsertException(sqlStateConnection.Connection, ex, true, id);
				}
			}
			finally
			{
				this.DisposeOrReuseConnection(ref sqlStateConnection, flag);
			}
		}

		// Token: 0x06002B2F RID: 11055 RVA: 0x000BFCBC File Offset: 0x000BECBC
		private void HandleInsertException(SqlConnection conn, Exception e, bool newItem, string id)
		{
			SqlException ex = e as SqlException;
			if (ex != null && ex.Number == 2627 && newItem)
			{
				return;
			}
			SqlSessionStateStore.ThrowSqlConnectionException(conn, e);
		}

		// Token: 0x06002B30 RID: 11056 RVA: 0x000BFCEB File Offset: 0x000BECEB
		private static bool IsInsertPKException(SqlException ex, bool ignoreInsertPKException, string id)
		{
			return ex != null && ex.Number == 2627 && ignoreInsertPKException;
		}

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x06002B31 RID: 11057 RVA: 0x000BFD04 File Offset: 0x000BED04
		private static bool IsRetryEnabled
		{
			get
			{
				return AppSettings.SqlSessionRetryInterval.TotalSeconds >= 1.0;
			}
		}

		// Token: 0x06002B32 RID: 11058 RVA: 0x000BFD2C File Offset: 0x000BED2C
		private static bool CanRetryOnError(SqlException ex)
		{
			return ex != null && (ex.Class >= 20 || ex.Number == 4060 || ex.Number == -2 || ex.Number == 6005);
		}

		// Token: 0x06002B33 RID: 11059 RVA: 0x000BFD62 File Offset: 0x000BED62
		private static void ClearFlagForClearPoolInProgress()
		{
			Interlocked.CompareExchange(ref SqlSessionStateStore.s_isClearPoolInProgress, 0, 1);
		}

		// Token: 0x06002B34 RID: 11060 RVA: 0x000BFD74 File Offset: 0x000BED74
		private static bool CanRetry(SqlException ex, SqlConnection conn, ref bool isFirstAttempt, ref DateTime endRetryTime)
		{
			if (!SqlSessionStateStore.IsRetryEnabled)
			{
				return false;
			}
			if (!SqlSessionStateStore.CanRetryOnError(ex))
			{
				if (!isFirstAttempt)
				{
					SqlSessionStateStore.ClearFlagForClearPoolInProgress();
				}
				return false;
			}
			if (isFirstAttempt)
			{
				if (Interlocked.CompareExchange(ref SqlSessionStateStore.s_isClearPoolInProgress, 1, 0) == 0)
				{
					SqlConnection.ClearPool(conn);
				}
				Thread.Sleep(5000);
				endRetryTime = DateTime.UtcNow.Add(AppSettings.SqlSessionRetryInterval);
				isFirstAttempt = false;
				return true;
			}
			if (DateTime.UtcNow > endRetryTime)
			{
				if (!isFirstAttempt)
				{
					SqlSessionStateStore.ClearFlagForClearPoolInProgress();
				}
				return false;
			}
			Thread.Sleep(1000);
			return true;
		}

		// Token: 0x06002B35 RID: 11061 RVA: 0x000BFE04 File Offset: 0x000BEE04
		private static int SqlExecuteNonQueryWithRetry(SqlCommand cmd, bool ignoreInsertPKException, string id)
		{
			bool flag = true;
			DateTime utcNow = DateTime.UtcNow;
			int num2;
			try
			{
				IL_0008:
				if (cmd.Connection.State != ConnectionState.Open)
				{
					cmd.Connection.Open();
				}
				int num = cmd.ExecuteNonQuery();
				if (!flag)
				{
					SqlSessionStateStore.ClearFlagForClearPoolInProgress();
				}
				num2 = num;
			}
			catch (SqlException ex)
			{
				if (!SqlSessionStateStore.IsInsertPKException(ex, ignoreInsertPKException, id))
				{
					if (!SqlSessionStateStore.CanRetry(ex, cmd.Connection, ref flag, ref utcNow))
					{
						SqlSessionStateStore.ThrowSqlConnectionException(cmd.Connection, ex);
					}
					goto IL_0008;
				}
				num2 = -1;
			}
			catch (Exception ex2)
			{
				SqlSessionStateStore.ThrowSqlConnectionException(cmd.Connection, ex2);
				goto IL_0008;
			}
			return num2;
		}

		// Token: 0x06002B36 RID: 11062 RVA: 0x000BFEA8 File Offset: 0x000BEEA8
		private static SqlDataReader SqlExecuteReaderWithRetry(SqlCommand cmd, CommandBehavior cmdBehavior)
		{
			bool flag = true;
			DateTime utcNow = DateTime.UtcNow;
			SqlDataReader sqlDataReader2;
			try
			{
				IL_0008:
				if (cmd.Connection.State != ConnectionState.Open)
				{
					cmd.Connection.Open();
				}
				SqlDataReader sqlDataReader = cmd.ExecuteReader(cmdBehavior);
				if (!flag)
				{
					SqlSessionStateStore.ClearFlagForClearPoolInProgress();
				}
				sqlDataReader2 = sqlDataReader;
			}
			catch (SqlException ex)
			{
				if (!SqlSessionStateStore.CanRetry(ex, cmd.Connection, ref flag, ref utcNow))
				{
					SqlSessionStateStore.ThrowSqlConnectionException(cmd.Connection, ex);
				}
				goto IL_0008;
			}
			catch (Exception ex2)
			{
				SqlSessionStateStore.ThrowSqlConnectionException(cmd.Connection, ex2);
				goto IL_0008;
			}
			return sqlDataReader2;
		}

		// Token: 0x04001FC0 RID: 8128
		private const int ITEM_SHORT_LENGTH = 7000;

		// Token: 0x04001FC1 RID: 8129
		private const int SQL_ERROR_PRIMARY_KEY_VIOLATION = 2627;

		// Token: 0x04001FC2 RID: 8130
		private const int SQL_LOGIN_FAILED = 18456;

		// Token: 0x04001FC3 RID: 8131
		private const int SQL_LOGIN_FAILED_2 = 18452;

		// Token: 0x04001FC4 RID: 8132
		private const int SQL_LOGIN_FAILED_3 = 18450;

		// Token: 0x04001FC5 RID: 8133
		private const int APP_SUFFIX_LENGTH = 8;

		// Token: 0x04001FC6 RID: 8134
		private const int SQL_CANNOT_OPEN_DATABASE_FOR_LOGIN = 4060;

		// Token: 0x04001FC7 RID: 8135
		private const int SQL_SHUTDOWN_IN_PROGRESS = 6005;

		// Token: 0x04001FC8 RID: 8136
		private const int SQL_TIMEOUT_EXPIRED = -2;

		// Token: 0x04001FC9 RID: 8137
		private const int FIRST_RETRY_SLEEP_TIME = 5000;

		// Token: 0x04001FCA RID: 8138
		private const int RETRY_SLEEP_TIME = 1000;

		// Token: 0x04001FCB RID: 8139
		internal const int SQL_COMMAND_TIMEOUT_DEFAULT = 30;

		// Token: 0x04001FCC RID: 8140
		private static ReadWriteSpinLock s_lock;

		// Token: 0x04001FCD RID: 8141
		private static int s_commandTimeout;

		// Token: 0x04001FCE RID: 8142
		private static SqlSessionStateStore.SqlPartitionInfo s_singlePartitionInfo;

		// Token: 0x04001FCF RID: 8143
		private static PartitionManager s_partitionManager;

		// Token: 0x04001FD0 RID: 8144
		private static bool s_oneTimeInited;

		// Token: 0x04001FD1 RID: 8145
		private static bool s_usePartition;

		// Token: 0x04001FD2 RID: 8146
		private static EventHandler s_onAppDomainUnload;

		// Token: 0x04001FD3 RID: 8147
		private static int s_isClearPoolInProgress;

		// Token: 0x04001FD4 RID: 8148
		private static string s_configPartitionResolverType;

		// Token: 0x04001FD5 RID: 8149
		private static string s_configSqlConnectionFileName;

		// Token: 0x04001FD6 RID: 8150
		private static int s_configSqlConnectionLineNumber;

		// Token: 0x04001FD7 RID: 8151
		private static bool s_configAllowCustomSqlDatabase;

		// Token: 0x04001FD8 RID: 8152
		private HttpContext _rqContext;

		// Token: 0x04001FD9 RID: 8153
		private int _rqOrigStreamLen;

		// Token: 0x04001FDA RID: 8154
		private IPartitionResolver _partitionResolver;

		// Token: 0x04001FDB RID: 8155
		private SqlSessionStateStore.SqlPartitionInfo _partitionInfo;

		// Token: 0x04001FDC RID: 8156
		private static int ID_LENGTH = SessionIDManager.SessionIDMaxLength + 8;

		// Token: 0x02000379 RID: 889
		internal enum SupportFlags : uint
		{
			// Token: 0x04001FDE RID: 8158
			None,
			// Token: 0x04001FDF RID: 8159
			GetLockAge,
			// Token: 0x04001FE0 RID: 8160
			Uninitialized = 4294967295U
		}

		// Token: 0x0200037A RID: 890
		internal class SqlPartitionInfo : PartitionInfo
		{
			// Token: 0x06002B38 RID: 11064 RVA: 0x000BFF4A File Offset: 0x000BEF4A
			internal SqlPartitionInfo(ResourcePool rpool, bool useIntegratedSecurity, string sqlConnectionString)
				: base(rpool)
			{
				this._useIntegratedSecurity = useIntegratedSecurity;
				this._sqlConnectionString = sqlConnectionString;
			}

			// Token: 0x17000937 RID: 2359
			// (get) Token: 0x06002B39 RID: 11065 RVA: 0x000BFF73 File Offset: 0x000BEF73
			internal bool UseIntegratedSecurity
			{
				get
				{
					return this._useIntegratedSecurity;
				}
			}

			// Token: 0x17000938 RID: 2360
			// (get) Token: 0x06002B3A RID: 11066 RVA: 0x000BFF7B File Offset: 0x000BEF7B
			internal string SqlConnectionString
			{
				get
				{
					return this._sqlConnectionString;
				}
			}

			// Token: 0x17000939 RID: 2361
			// (get) Token: 0x06002B3B RID: 11067 RVA: 0x000BFF83 File Offset: 0x000BEF83
			// (set) Token: 0x06002B3C RID: 11068 RVA: 0x000BFF8B File Offset: 0x000BEF8B
			internal SqlSessionStateStore.SupportFlags SupportFlags
			{
				get
				{
					return this._support;
				}
				set
				{
					this._support = value;
				}
			}

			// Token: 0x1700093A RID: 2362
			// (get) Token: 0x06002B3D RID: 11069 RVA: 0x000BFF94 File Offset: 0x000BEF94
			protected override string TracingPartitionString
			{
				get
				{
					if (this._tracingPartitionString == null)
					{
						this._tracingPartitionString = new SqlConnectionStringBuilder(this._sqlConnectionString)
						{
							Password = string.Empty,
							UserID = string.Empty
						}.ConnectionString;
					}
					return this._tracingPartitionString;
				}
			}

			// Token: 0x1700093B RID: 2363
			// (get) Token: 0x06002B3E RID: 11070 RVA: 0x000BFFDD File Offset: 0x000BEFDD
			internal string AppSuffix
			{
				get
				{
					return this._appSuffix;
				}
			}

			// Token: 0x06002B3F RID: 11071 RVA: 0x000BFFE8 File Offset: 0x000BEFE8
			private void GetServerSupportOptions(SqlConnection sqlConnection)
			{
				SqlDataReader sqlDataReader = null;
				SqlSessionStateStore.SupportFlags supportFlags = SqlSessionStateStore.SupportFlags.None;
				bool flag = false;
				SqlCommand sqlCommand = new SqlCommand("Select name from sysobjects where type = 'P' and name = 'TempGetVersion'", sqlConnection);
				sqlCommand.CommandType = CommandType.Text;
				try
				{
					if (SqlSessionStateStore.IsRetryEnabled)
					{
						sqlDataReader = SqlSessionStateStore.SqlExecuteReaderWithRetry(sqlCommand, CommandBehavior.SingleRow);
					}
					else
					{
						sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SingleRow);
					}
					if (sqlDataReader.Read())
					{
						flag = true;
					}
				}
				catch (Exception ex)
				{
					SqlSessionStateStore.ThrowSqlConnectionException(sqlConnection, ex);
				}
				finally
				{
					if (sqlDataReader != null)
					{
						sqlDataReader.Close();
						sqlDataReader = null;
					}
				}
				if (flag)
				{
					sqlCommand = new SqlCommand("dbo.GetMajorVersion", sqlConnection);
					sqlCommand.CommandType = CommandType.StoredProcedure;
					SqlParameter sqlParameter = sqlCommand.Parameters.Add(new SqlParameter("@@ver", SqlDbType.Int));
					sqlParameter.Direction = ParameterDirection.Output;
					try
					{
						sqlCommand.ExecuteNonQuery();
						if ((int)sqlParameter.Value >= 8)
						{
							supportFlags |= SqlSessionStateStore.SupportFlags.GetLockAge;
						}
						this.SupportFlags = supportFlags;
					}
					catch (Exception ex2)
					{
						SqlSessionStateStore.ThrowSqlConnectionException(sqlConnection, ex2);
					}
					return;
				}
				if (SqlSessionStateStore.s_usePartition)
				{
					throw new HttpException(SR.GetString("Need_v2_SQL_Server_partition_resolver", new object[]
					{
						SqlSessionStateStore.s_configPartitionResolverType,
						sqlConnection.DataSource,
						sqlConnection.Database
					}));
				}
				throw new HttpException(SR.GetString("Need_v2_SQL_Server"));
			}

			// Token: 0x06002B40 RID: 11072 RVA: 0x000C0128 File Offset: 0x000BF128
			internal void InitSqlInfo(SqlConnection sqlConnection)
			{
				if (this._sqlInfoInited)
				{
					return;
				}
				lock (this._lock)
				{
					if (!this._sqlInfoInited)
					{
						this.GetServerSupportOptions(sqlConnection);
						SqlCommand sqlCommand = new SqlCommand("dbo.TempGetAppID", sqlConnection);
						sqlCommand.CommandType = CommandType.StoredProcedure;
						sqlCommand.CommandTimeout = SqlSessionStateStore.s_commandTimeout;
						SqlParameter sqlParameter = sqlCommand.Parameters.Add(new SqlParameter("@appName", SqlDbType.VarChar, 280));
						sqlParameter.Value = HttpRuntime.AppDomainAppIdInternal;
						sqlParameter = sqlCommand.Parameters.Add(new SqlParameter("@appId", SqlDbType.Int));
						sqlParameter.Direction = ParameterDirection.Output;
						sqlParameter.Value = Convert.DBNull;
						sqlCommand.ExecuteNonQuery();
						this._appSuffix = ((int)sqlParameter.Value).ToString("x8", CultureInfo.InvariantCulture);
						this._sqlInfoInited = true;
					}
				}
			}

			// Token: 0x04001FE1 RID: 8161
			private const string APP_SUFFIX_FORMAT = "x8";

			// Token: 0x04001FE2 RID: 8162
			private const int APPID_MAX = 280;

			// Token: 0x04001FE3 RID: 8163
			private const int SQL_2000_MAJ_VER = 8;

			// Token: 0x04001FE4 RID: 8164
			private bool _useIntegratedSecurity;

			// Token: 0x04001FE5 RID: 8165
			private string _sqlConnectionString;

			// Token: 0x04001FE6 RID: 8166
			private string _tracingPartitionString;

			// Token: 0x04001FE7 RID: 8167
			private SqlSessionStateStore.SupportFlags _support = (SqlSessionStateStore.SupportFlags)4294967295U;

			// Token: 0x04001FE8 RID: 8168
			private string _appSuffix;

			// Token: 0x04001FE9 RID: 8169
			private object _lock = new object();

			// Token: 0x04001FEA RID: 8170
			private bool _sqlInfoInited;
		}

		// Token: 0x0200037B RID: 891
		private class SqlStateConnection : IDisposable
		{
			// Token: 0x06002B41 RID: 11073 RVA: 0x000C0218 File Offset: 0x000BF218
			internal SqlStateConnection(SqlSessionStateStore.SqlPartitionInfo sqlPartitionInfo)
			{
				this._partitionInfo = sqlPartitionInfo;
				this._sqlConnection = new SqlConnection(sqlPartitionInfo.SqlConnectionString);
				try
				{
					this._sqlConnection.Open();
				}
				catch (Exception ex)
				{
					SqlConnection sqlConnection = this._sqlConnection;
					SqlException ex2 = ex as SqlException;
					this._sqlConnection = null;
					if (ex2 != null && (ex2.Number == 18456 || ex2.Number == 18452 || ex2.Number == 18450))
					{
						SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(sqlPartitionInfo.SqlConnectionString);
						string text;
						if (sqlConnectionStringBuilder.IntegratedSecurity)
						{
							text = WindowsIdentity.GetCurrent().Name;
						}
						else
						{
							text = sqlConnectionStringBuilder.UserID;
						}
						HttpException ex3 = new HttpException(SR.GetString("Login_failed_sql_session_database", new object[] { text }), ex);
						ex3.SetFormatter(new UseLastUnhandledErrorFormatter(ex3));
						ex = ex3;
					}
					SqlSessionStateStore.ThrowSqlConnectionException(sqlConnection, ex);
				}
				try
				{
					this._partitionInfo.InitSqlInfo(this._sqlConnection);
					PerfCounters.IncrementCounter(AppPerfCounter.SESSION_SQL_SERVER_CONNECTIONS);
				}
				catch
				{
					this.Dispose();
					throw;
				}
			}

			// Token: 0x06002B42 RID: 11074 RVA: 0x000C033C File Offset: 0x000BF33C
			internal SqlStateConnection(SqlSessionStateStore.SqlPartitionInfo sqlPartitionInfo, TimeSpan retryInterval)
			{
				this._partitionInfo = sqlPartitionInfo;
				this._sqlConnection = new SqlConnection(sqlPartitionInfo.SqlConnectionString);
				bool flag = true;
				DateTime utcNow = DateTime.UtcNow;
				try
				{
					IL_0026:
					this._sqlConnection.Open();
					if (!flag)
					{
						SqlSessionStateStore.ClearFlagForClearPoolInProgress();
					}
				}
				catch (SqlException ex)
				{
					if (ex != null && (ex.Number == 18456 || ex.Number == 18452 || ex.Number == 18450))
					{
						SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(sqlPartitionInfo.SqlConnectionString);
						string text;
						if (sqlConnectionStringBuilder.IntegratedSecurity)
						{
							text = WindowsIdentity.GetCurrent().Name;
						}
						else
						{
							text = sqlConnectionStringBuilder.UserID;
						}
						HttpException ex2 = new HttpException(SR.GetString("Login_failed_sql_session_database", new object[] { text }), ex);
						ex2.SetFormatter(new UseLastUnhandledErrorFormatter(ex2));
						this.ClearConnectionAndThrow(ex2);
					}
					if (!SqlSessionStateStore.CanRetry(ex, this._sqlConnection, ref flag, ref utcNow))
					{
						this.ClearConnectionAndThrow(ex);
					}
					goto IL_0026;
				}
				catch (Exception ex3)
				{
					this.ClearConnectionAndThrow(ex3);
					goto IL_0026;
				}
				try
				{
					this._partitionInfo.InitSqlInfo(this._sqlConnection);
					PerfCounters.IncrementCounter(AppPerfCounter.SESSION_SQL_SERVER_CONNECTIONS);
				}
				catch
				{
					this.Dispose();
					throw;
				}
			}

			// Token: 0x06002B43 RID: 11075 RVA: 0x000C0490 File Offset: 0x000BF490
			private void ClearConnectionAndThrow(Exception e)
			{
				SqlConnection sqlConnection = this._sqlConnection;
				this._sqlConnection = null;
				SqlSessionStateStore.ThrowSqlConnectionException(sqlConnection, e);
			}

			// Token: 0x1700093C RID: 2364
			// (get) Token: 0x06002B44 RID: 11076 RVA: 0x000C04B4 File Offset: 0x000BF4B4
			internal SqlCommand TempGet
			{
				get
				{
					if (this._cmdTempGet == null)
					{
						this._cmdTempGet = new SqlCommand("dbo.TempGetStateItem3", this._sqlConnection);
						this._cmdTempGet.CommandType = CommandType.StoredProcedure;
						this._cmdTempGet.CommandTimeout = SqlSessionStateStore.s_commandTimeout;
						if ((this._partitionInfo.SupportFlags & SqlSessionStateStore.SupportFlags.GetLockAge) != SqlSessionStateStore.SupportFlags.None)
						{
							this._cmdTempGet.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar, SqlSessionStateStore.ID_LENGTH));
							SqlParameter sqlParameter = this._cmdTempGet.Parameters.Add(new SqlParameter("@itemShort", SqlDbType.VarBinary, 7000));
							sqlParameter.Direction = ParameterDirection.Output;
							sqlParameter = this._cmdTempGet.Parameters.Add(new SqlParameter("@locked", SqlDbType.Bit));
							sqlParameter.Direction = ParameterDirection.Output;
							sqlParameter = this._cmdTempGet.Parameters.Add(new SqlParameter("@lockAge", SqlDbType.Int));
							sqlParameter.Direction = ParameterDirection.Output;
							sqlParameter = this._cmdTempGet.Parameters.Add(new SqlParameter("@lockCookie", SqlDbType.Int));
							sqlParameter.Direction = ParameterDirection.Output;
							sqlParameter = this._cmdTempGet.Parameters.Add(new SqlParameter("@actionFlags", SqlDbType.Int));
							sqlParameter.Direction = ParameterDirection.Output;
						}
						else
						{
							this._cmdTempGet.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar, SqlSessionStateStore.ID_LENGTH));
							SqlParameter sqlParameter = this._cmdTempGet.Parameters.Add(new SqlParameter("@itemShort", SqlDbType.VarBinary, 7000));
							sqlParameter.Direction = ParameterDirection.Output;
							sqlParameter = this._cmdTempGet.Parameters.Add(new SqlParameter("@locked", SqlDbType.Bit));
							sqlParameter.Direction = ParameterDirection.Output;
							sqlParameter = this._cmdTempGet.Parameters.Add(new SqlParameter("@lockDate", SqlDbType.DateTime));
							sqlParameter.Direction = ParameterDirection.Output;
							sqlParameter = this._cmdTempGet.Parameters.Add(new SqlParameter("@lockCookie", SqlDbType.Int));
							sqlParameter.Direction = ParameterDirection.Output;
							sqlParameter = this._cmdTempGet.Parameters.Add(new SqlParameter("@actionFlags", SqlDbType.Int));
							sqlParameter.Direction = ParameterDirection.Output;
						}
					}
					return this._cmdTempGet;
				}
			}

			// Token: 0x1700093D RID: 2365
			// (get) Token: 0x06002B45 RID: 11077 RVA: 0x000C06CC File Offset: 0x000BF6CC
			internal SqlCommand TempGetExclusive
			{
				get
				{
					if (this._cmdTempGetExclusive == null)
					{
						this._cmdTempGetExclusive = new SqlCommand("dbo.TempGetStateItemExclusive3", this._sqlConnection);
						this._cmdTempGetExclusive.CommandType = CommandType.StoredProcedure;
						this._cmdTempGetExclusive.CommandTimeout = SqlSessionStateStore.s_commandTimeout;
						if ((this._partitionInfo.SupportFlags & SqlSessionStateStore.SupportFlags.GetLockAge) != SqlSessionStateStore.SupportFlags.None)
						{
							this._cmdTempGetExclusive.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar, SqlSessionStateStore.ID_LENGTH));
							SqlParameter sqlParameter = this._cmdTempGetExclusive.Parameters.Add(new SqlParameter("@itemShort", SqlDbType.VarBinary, 7000));
							sqlParameter.Direction = ParameterDirection.Output;
							sqlParameter = this._cmdTempGetExclusive.Parameters.Add(new SqlParameter("@locked", SqlDbType.Bit));
							sqlParameter.Direction = ParameterDirection.Output;
							sqlParameter = this._cmdTempGetExclusive.Parameters.Add(new SqlParameter("@lockAge", SqlDbType.Int));
							sqlParameter.Direction = ParameterDirection.Output;
							sqlParameter = this._cmdTempGetExclusive.Parameters.Add(new SqlParameter("@lockCookie", SqlDbType.Int));
							sqlParameter.Direction = ParameterDirection.Output;
							sqlParameter = this._cmdTempGetExclusive.Parameters.Add(new SqlParameter("@actionFlags", SqlDbType.Int));
							sqlParameter.Direction = ParameterDirection.Output;
						}
						else
						{
							this._cmdTempGetExclusive.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar, SqlSessionStateStore.ID_LENGTH));
							SqlParameter sqlParameter = this._cmdTempGetExclusive.Parameters.Add(new SqlParameter("@itemShort", SqlDbType.VarBinary, 7000));
							sqlParameter.Direction = ParameterDirection.Output;
							sqlParameter = this._cmdTempGetExclusive.Parameters.Add(new SqlParameter("@locked", SqlDbType.Bit));
							sqlParameter.Direction = ParameterDirection.Output;
							sqlParameter = this._cmdTempGetExclusive.Parameters.Add(new SqlParameter("@lockDate", SqlDbType.DateTime));
							sqlParameter.Direction = ParameterDirection.Output;
							sqlParameter = this._cmdTempGetExclusive.Parameters.Add(new SqlParameter("@lockCookie", SqlDbType.Int));
							sqlParameter.Direction = ParameterDirection.Output;
							sqlParameter = this._cmdTempGetExclusive.Parameters.Add(new SqlParameter("@actionFlags", SqlDbType.Int));
							sqlParameter.Direction = ParameterDirection.Output;
						}
					}
					return this._cmdTempGetExclusive;
				}
			}

			// Token: 0x1700093E RID: 2366
			// (get) Token: 0x06002B46 RID: 11078 RVA: 0x000C08E4 File Offset: 0x000BF8E4
			internal SqlCommand TempReleaseExclusive
			{
				get
				{
					if (this._cmdTempReleaseExclusive == null)
					{
						this._cmdTempReleaseExclusive = new SqlCommand("dbo.TempReleaseStateItemExclusive", this._sqlConnection);
						this._cmdTempReleaseExclusive.CommandType = CommandType.StoredProcedure;
						this._cmdTempReleaseExclusive.CommandTimeout = SqlSessionStateStore.s_commandTimeout;
						this._cmdTempReleaseExclusive.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar, SqlSessionStateStore.ID_LENGTH));
						this._cmdTempReleaseExclusive.Parameters.Add(new SqlParameter("@lockCookie", SqlDbType.Int));
					}
					return this._cmdTempReleaseExclusive;
				}
			}

			// Token: 0x1700093F RID: 2367
			// (get) Token: 0x06002B47 RID: 11079 RVA: 0x000C0970 File Offset: 0x000BF970
			internal SqlCommand TempInsertLong
			{
				get
				{
					if (this._cmdTempInsertLong == null)
					{
						this._cmdTempInsertLong = new SqlCommand("dbo.TempInsertStateItemLong", this._sqlConnection);
						this._cmdTempInsertLong.CommandType = CommandType.StoredProcedure;
						this._cmdTempInsertLong.CommandTimeout = SqlSessionStateStore.s_commandTimeout;
						this._cmdTempInsertLong.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar, SqlSessionStateStore.ID_LENGTH));
						this._cmdTempInsertLong.Parameters.Add(new SqlParameter("@itemLong", SqlDbType.Image, 8000));
						this._cmdTempInsertLong.Parameters.Add(new SqlParameter("@timeout", SqlDbType.Int));
					}
					return this._cmdTempInsertLong;
				}
			}

			// Token: 0x17000940 RID: 2368
			// (get) Token: 0x06002B48 RID: 11080 RVA: 0x000C0A20 File Offset: 0x000BFA20
			internal SqlCommand TempInsertShort
			{
				get
				{
					if (this._cmdTempInsertShort == null)
					{
						this._cmdTempInsertShort = new SqlCommand("dbo.TempInsertStateItemShort", this._sqlConnection);
						this._cmdTempInsertShort.CommandType = CommandType.StoredProcedure;
						this._cmdTempInsertShort.CommandTimeout = SqlSessionStateStore.s_commandTimeout;
						this._cmdTempInsertShort.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar, SqlSessionStateStore.ID_LENGTH));
						this._cmdTempInsertShort.Parameters.Add(new SqlParameter("@itemShort", SqlDbType.VarBinary, 7000));
						this._cmdTempInsertShort.Parameters.Add(new SqlParameter("@timeout", SqlDbType.Int));
					}
					return this._cmdTempInsertShort;
				}
			}

			// Token: 0x17000941 RID: 2369
			// (get) Token: 0x06002B49 RID: 11081 RVA: 0x000C0AD0 File Offset: 0x000BFAD0
			internal SqlCommand TempUpdateLong
			{
				get
				{
					if (this._cmdTempUpdateLong == null)
					{
						this._cmdTempUpdateLong = new SqlCommand("dbo.TempUpdateStateItemLong", this._sqlConnection);
						this._cmdTempUpdateLong.CommandType = CommandType.StoredProcedure;
						this._cmdTempUpdateLong.CommandTimeout = SqlSessionStateStore.s_commandTimeout;
						this._cmdTempUpdateLong.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar, SqlSessionStateStore.ID_LENGTH));
						this._cmdTempUpdateLong.Parameters.Add(new SqlParameter("@itemLong", SqlDbType.Image, 8000));
						this._cmdTempUpdateLong.Parameters.Add(new SqlParameter("@timeout", SqlDbType.Int));
						this._cmdTempUpdateLong.Parameters.Add(new SqlParameter("@lockCookie", SqlDbType.Int));
					}
					return this._cmdTempUpdateLong;
				}
			}

			// Token: 0x17000942 RID: 2370
			// (get) Token: 0x06002B4A RID: 11082 RVA: 0x000C0B9C File Offset: 0x000BFB9C
			internal SqlCommand TempUpdateShort
			{
				get
				{
					if (this._cmdTempUpdateShort == null)
					{
						this._cmdTempUpdateShort = new SqlCommand("dbo.TempUpdateStateItemShort", this._sqlConnection);
						this._cmdTempUpdateShort.CommandType = CommandType.StoredProcedure;
						this._cmdTempUpdateShort.CommandTimeout = SqlSessionStateStore.s_commandTimeout;
						this._cmdTempUpdateShort.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar, SqlSessionStateStore.ID_LENGTH));
						this._cmdTempUpdateShort.Parameters.Add(new SqlParameter("@itemShort", SqlDbType.VarBinary, 7000));
						this._cmdTempUpdateShort.Parameters.Add(new SqlParameter("@timeout", SqlDbType.Int));
						this._cmdTempUpdateShort.Parameters.Add(new SqlParameter("@lockCookie", SqlDbType.Int));
					}
					return this._cmdTempUpdateShort;
				}
			}

			// Token: 0x17000943 RID: 2371
			// (get) Token: 0x06002B4B RID: 11083 RVA: 0x000C0C68 File Offset: 0x000BFC68
			internal SqlCommand TempUpdateShortNullLong
			{
				get
				{
					if (this._cmdTempUpdateShortNullLong == null)
					{
						this._cmdTempUpdateShortNullLong = new SqlCommand("dbo.TempUpdateStateItemShortNullLong", this._sqlConnection);
						this._cmdTempUpdateShortNullLong.CommandType = CommandType.StoredProcedure;
						this._cmdTempUpdateShortNullLong.CommandTimeout = SqlSessionStateStore.s_commandTimeout;
						this._cmdTempUpdateShortNullLong.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar, SqlSessionStateStore.ID_LENGTH));
						this._cmdTempUpdateShortNullLong.Parameters.Add(new SqlParameter("@itemShort", SqlDbType.VarBinary, 7000));
						this._cmdTempUpdateShortNullLong.Parameters.Add(new SqlParameter("@timeout", SqlDbType.Int));
						this._cmdTempUpdateShortNullLong.Parameters.Add(new SqlParameter("@lockCookie", SqlDbType.Int));
					}
					return this._cmdTempUpdateShortNullLong;
				}
			}

			// Token: 0x17000944 RID: 2372
			// (get) Token: 0x06002B4C RID: 11084 RVA: 0x000C0D34 File Offset: 0x000BFD34
			internal SqlCommand TempUpdateLongNullShort
			{
				get
				{
					if (this._cmdTempUpdateLongNullShort == null)
					{
						this._cmdTempUpdateLongNullShort = new SqlCommand("dbo.TempUpdateStateItemLongNullShort", this._sqlConnection);
						this._cmdTempUpdateLongNullShort.CommandType = CommandType.StoredProcedure;
						this._cmdTempUpdateLongNullShort.CommandTimeout = SqlSessionStateStore.s_commandTimeout;
						this._cmdTempUpdateLongNullShort.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar, SqlSessionStateStore.ID_LENGTH));
						this._cmdTempUpdateLongNullShort.Parameters.Add(new SqlParameter("@itemLong", SqlDbType.Image, 8000));
						this._cmdTempUpdateLongNullShort.Parameters.Add(new SqlParameter("@timeout", SqlDbType.Int));
						this._cmdTempUpdateLongNullShort.Parameters.Add(new SqlParameter("@lockCookie", SqlDbType.Int));
					}
					return this._cmdTempUpdateLongNullShort;
				}
			}

			// Token: 0x17000945 RID: 2373
			// (get) Token: 0x06002B4D RID: 11085 RVA: 0x000C0E00 File Offset: 0x000BFE00
			internal SqlCommand TempRemove
			{
				get
				{
					if (this._cmdTempRemove == null)
					{
						this._cmdTempRemove = new SqlCommand("dbo.TempRemoveStateItem", this._sqlConnection);
						this._cmdTempRemove.CommandType = CommandType.StoredProcedure;
						this._cmdTempRemove.CommandTimeout = SqlSessionStateStore.s_commandTimeout;
						this._cmdTempRemove.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar, SqlSessionStateStore.ID_LENGTH));
						this._cmdTempRemove.Parameters.Add(new SqlParameter("@lockCookie", SqlDbType.Int));
					}
					return this._cmdTempRemove;
				}
			}

			// Token: 0x17000946 RID: 2374
			// (get) Token: 0x06002B4E RID: 11086 RVA: 0x000C0E8C File Offset: 0x000BFE8C
			internal SqlCommand TempInsertUninitializedItem
			{
				get
				{
					if (this._cmdTempInsertUninitializedItem == null)
					{
						this._cmdTempInsertUninitializedItem = new SqlCommand("dbo.TempInsertUninitializedItem", this._sqlConnection);
						this._cmdTempInsertUninitializedItem.CommandType = CommandType.StoredProcedure;
						this._cmdTempInsertUninitializedItem.CommandTimeout = SqlSessionStateStore.s_commandTimeout;
						this._cmdTempInsertUninitializedItem.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar, SqlSessionStateStore.ID_LENGTH));
						this._cmdTempInsertUninitializedItem.Parameters.Add(new SqlParameter("@itemShort", SqlDbType.VarBinary, 7000));
						this._cmdTempInsertUninitializedItem.Parameters.Add(new SqlParameter("@timeout", SqlDbType.Int));
					}
					return this._cmdTempInsertUninitializedItem;
				}
			}

			// Token: 0x17000947 RID: 2375
			// (get) Token: 0x06002B4F RID: 11087 RVA: 0x000C0F3C File Offset: 0x000BFF3C
			internal SqlCommand TempResetTimeout
			{
				get
				{
					if (this._cmdTempResetTimeout == null)
					{
						this._cmdTempResetTimeout = new SqlCommand("dbo.TempResetTimeout", this._sqlConnection);
						this._cmdTempResetTimeout.CommandType = CommandType.StoredProcedure;
						this._cmdTempResetTimeout.CommandTimeout = SqlSessionStateStore.s_commandTimeout;
						this._cmdTempResetTimeout.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar, SqlSessionStateStore.ID_LENGTH));
					}
					return this._cmdTempResetTimeout;
				}
			}

			// Token: 0x06002B50 RID: 11088 RVA: 0x000C0FAB File Offset: 0x000BFFAB
			public void Dispose()
			{
				if (this._sqlConnection != null)
				{
					this._sqlConnection.Close();
					this._sqlConnection = null;
					PerfCounters.DecrementCounter(AppPerfCounter.SESSION_SQL_SERVER_CONNECTIONS);
				}
			}

			// Token: 0x17000948 RID: 2376
			// (get) Token: 0x06002B51 RID: 11089 RVA: 0x000C0FCE File Offset: 0x000BFFCE
			internal SqlConnection Connection
			{
				get
				{
					return this._sqlConnection;
				}
			}

			// Token: 0x04001FEB RID: 8171
			private SqlConnection _sqlConnection;

			// Token: 0x04001FEC RID: 8172
			private SqlCommand _cmdTempGet;

			// Token: 0x04001FED RID: 8173
			private SqlCommand _cmdTempGetExclusive;

			// Token: 0x04001FEE RID: 8174
			private SqlCommand _cmdTempReleaseExclusive;

			// Token: 0x04001FEF RID: 8175
			private SqlCommand _cmdTempInsertShort;

			// Token: 0x04001FF0 RID: 8176
			private SqlCommand _cmdTempInsertLong;

			// Token: 0x04001FF1 RID: 8177
			private SqlCommand _cmdTempUpdateShort;

			// Token: 0x04001FF2 RID: 8178
			private SqlCommand _cmdTempUpdateShortNullLong;

			// Token: 0x04001FF3 RID: 8179
			private SqlCommand _cmdTempUpdateLong;

			// Token: 0x04001FF4 RID: 8180
			private SqlCommand _cmdTempUpdateLongNullShort;

			// Token: 0x04001FF5 RID: 8181
			private SqlCommand _cmdTempRemove;

			// Token: 0x04001FF6 RID: 8182
			private SqlCommand _cmdTempResetTimeout;

			// Token: 0x04001FF7 RID: 8183
			private SqlCommand _cmdTempInsertUninitializedItem;

			// Token: 0x04001FF8 RID: 8184
			private SqlSessionStateStore.SqlPartitionInfo _partitionInfo;
		}
	}
}
