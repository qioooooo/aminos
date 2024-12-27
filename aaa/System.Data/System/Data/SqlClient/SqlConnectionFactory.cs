using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Common;
using System.Data.ProviderBase;
using System.IO;
using System.Reflection;
using Microsoft.SqlServer.Server;

namespace System.Data.SqlClient
{
	// Token: 0x020002D0 RID: 720
	internal sealed class SqlConnectionFactory : DbConnectionFactory
	{
		// Token: 0x060024F6 RID: 9462 RVA: 0x00279F8C File Offset: 0x0027938C
		private SqlConnectionFactory()
			: base(SqlPerformanceCounters.SingletonInstance)
		{
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x060024F7 RID: 9463 RVA: 0x00279FA4 File Offset: 0x002793A4
		public override DbProviderFactory ProviderFactory
		{
			get
			{
				return SqlClientFactory.Instance;
			}
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x00279FB8 File Offset: 0x002793B8
		protected override DbConnectionInternal CreateConnection(DbConnectionOptions options, object poolGroupProviderInfo, DbConnectionPool pool, DbConnection owningConnection)
		{
			SqlConnectionString sqlConnectionString = (SqlConnectionString)options;
			SqlInternalConnection sqlInternalConnection;
			if (sqlConnectionString.ContextConnection)
			{
				sqlInternalConnection = this.GetContextConnection(sqlConnectionString, poolGroupProviderInfo, owningConnection);
			}
			else
			{
				bool flag = false;
				DbConnectionPoolIdentity dbConnectionPoolIdentity = null;
				if (sqlConnectionString.IntegratedSecurity)
				{
					if (pool != null)
					{
						dbConnectionPoolIdentity = pool.Identity;
					}
					else
					{
						dbConnectionPoolIdentity = DbConnectionPoolIdentity.GetCurrent();
					}
				}
				if (sqlConnectionString.UserInstance)
				{
					flag = true;
					string text;
					if (pool == null || (pool != null && pool.Count <= 0))
					{
						SqlInternalConnectionTds sqlInternalConnectionTds = null;
						try
						{
							sqlInternalConnectionTds = new SqlInternalConnectionTds(dbConnectionPoolIdentity, sqlConnectionString, null, "", null, false);
							text = sqlInternalConnectionTds.InstanceName;
							if (!text.StartsWith("\\\\.\\", StringComparison.Ordinal))
							{
								throw SQL.NonLocalSSEInstance();
							}
							if (pool != null)
							{
								SqlConnectionPoolProviderInfo sqlConnectionPoolProviderInfo = (SqlConnectionPoolProviderInfo)pool.ProviderInfo;
								sqlConnectionPoolProviderInfo.InstanceName = text;
							}
							goto IL_00C5;
						}
						finally
						{
							if (sqlInternalConnectionTds != null)
							{
								sqlInternalConnectionTds.Dispose();
							}
						}
					}
					SqlConnectionPoolProviderInfo sqlConnectionPoolProviderInfo2 = (SqlConnectionPoolProviderInfo)pool.ProviderInfo;
					text = sqlConnectionPoolProviderInfo2.InstanceName;
					IL_00C5:
					sqlConnectionString = new SqlConnectionString(sqlConnectionString, false, text);
					poolGroupProviderInfo = null;
				}
				sqlInternalConnection = new SqlInternalConnectionTds(dbConnectionPoolIdentity, sqlConnectionString, poolGroupProviderInfo, "", (SqlConnection)owningConnection, flag);
			}
			return sqlInternalConnection;
		}

		// Token: 0x060024F9 RID: 9465 RVA: 0x0027A0CC File Offset: 0x002794CC
		protected override DbConnectionOptions CreateConnectionOptions(string connectionString, DbConnectionOptions previous)
		{
			return new SqlConnectionString(connectionString);
		}

		// Token: 0x060024FA RID: 9466 RVA: 0x0027A0E4 File Offset: 0x002794E4
		internal override DbConnectionPoolProviderInfo CreateConnectionPoolProviderInfo(DbConnectionOptions connectionOptions)
		{
			DbConnectionPoolProviderInfo dbConnectionPoolProviderInfo = null;
			if (((SqlConnectionString)connectionOptions).UserInstance)
			{
				dbConnectionPoolProviderInfo = new SqlConnectionPoolProviderInfo();
			}
			return dbConnectionPoolProviderInfo;
		}

		// Token: 0x060024FB RID: 9467 RVA: 0x0027A108 File Offset: 0x00279508
		protected override DbConnectionPoolGroupOptions CreateConnectionPoolGroupOptions(DbConnectionOptions connectionOptions)
		{
			SqlConnectionString sqlConnectionString = (SqlConnectionString)connectionOptions;
			DbConnectionPoolGroupOptions dbConnectionPoolGroupOptions = null;
			if (!sqlConnectionString.ContextConnection && sqlConnectionString.Pooling)
			{
				int num = sqlConnectionString.ConnectTimeout;
				if (0 < num && num < 2147483)
				{
					num *= 1000;
				}
				else if (num >= 2147483)
				{
					num = int.MaxValue;
				}
				dbConnectionPoolGroupOptions = new DbConnectionPoolGroupOptions(sqlConnectionString.IntegratedSecurity, sqlConnectionString.MinPoolSize, sqlConnectionString.MaxPoolSize, num, sqlConnectionString.LoadBalanceTimeout, sqlConnectionString.Enlist, false);
			}
			return dbConnectionPoolGroupOptions;
		}

		// Token: 0x060024FC RID: 9468 RVA: 0x0027A180 File Offset: 0x00279580
		protected override DbMetaDataFactory CreateMetaDataFactory(DbConnectionInternal internalConnection, out bool cacheMetaDataFactory)
		{
			cacheMetaDataFactory = false;
			if (internalConnection is SqlInternalConnectionSmi)
			{
				throw SQL.NotAvailableOnContextConnection();
			}
			NameValueCollection nameValueCollection = (NameValueCollection)PrivilegedConfigurationManager.GetSection("system.data.sqlclient");
			Stream stream = null;
			if (nameValueCollection != null)
			{
				string[] values = nameValueCollection.GetValues("MetaDataXml");
				if (values != null)
				{
					stream = ADP.GetXmlStreamFromValues(values, "MetaDataXml");
				}
			}
			if (stream == null)
			{
				stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("System.Data.SqlClient.SqlMetaData.xml");
				cacheMetaDataFactory = true;
			}
			return new SqlMetaDataFactory(stream, internalConnection.ServerVersion, internalConnection.ServerVersion);
		}

		// Token: 0x060024FD RID: 9469 RVA: 0x0027A1F8 File Offset: 0x002795F8
		internal override DbConnectionPoolGroupProviderInfo CreateConnectionPoolGroupProviderInfo(DbConnectionOptions connectionOptions)
		{
			return new SqlConnectionPoolGroupProviderInfo((SqlConnectionString)connectionOptions);
		}

		// Token: 0x060024FE RID: 9470 RVA: 0x0027A210 File Offset: 0x00279610
		internal static SqlConnectionString FindSqlConnectionOptions(string connectionString)
		{
			SqlConnectionString sqlConnectionString = (SqlConnectionString)SqlConnectionFactory.SingletonInstance.FindConnectionOptions(connectionString);
			if (sqlConnectionString == null)
			{
				sqlConnectionString = new SqlConnectionString(connectionString);
			}
			if (sqlConnectionString.IsEmpty)
			{
				throw ADP.NoConnectionString();
			}
			return sqlConnectionString;
		}

		// Token: 0x060024FF RID: 9471 RVA: 0x0027A248 File Offset: 0x00279648
		private SqlInternalConnectionSmi GetContextConnection(SqlConnectionString options, object providerInfo, DbConnection owningConnection)
		{
			SmiContext currentContext = SmiContextFactory.Instance.GetCurrentContext();
			SqlInternalConnectionSmi sqlInternalConnectionSmi = (SqlInternalConnectionSmi)currentContext.GetContextValue(0);
			if (sqlInternalConnectionSmi == null || sqlInternalConnectionSmi.IsConnectionDoomed)
			{
				if (sqlInternalConnectionSmi != null)
				{
					sqlInternalConnectionSmi.Dispose();
				}
				sqlInternalConnectionSmi = new SqlInternalConnectionSmi(options, currentContext);
				currentContext.SetContextValue(0, sqlInternalConnectionSmi);
			}
			sqlInternalConnectionSmi.Activate();
			return sqlInternalConnectionSmi;
		}

		// Token: 0x06002500 RID: 9472 RVA: 0x0027A298 File Offset: 0x00279698
		internal override DbConnectionPoolGroup GetConnectionPoolGroup(DbConnection connection)
		{
			SqlConnection sqlConnection = connection as SqlConnection;
			if (sqlConnection != null)
			{
				return sqlConnection.PoolGroup;
			}
			return null;
		}

		// Token: 0x06002501 RID: 9473 RVA: 0x0027A2B8 File Offset: 0x002796B8
		internal override DbConnectionInternal GetInnerConnection(DbConnection connection)
		{
			SqlConnection sqlConnection = connection as SqlConnection;
			if (sqlConnection != null)
			{
				return sqlConnection.InnerConnection;
			}
			return null;
		}

		// Token: 0x06002502 RID: 9474 RVA: 0x0027A2D8 File Offset: 0x002796D8
		protected override int GetObjectId(DbConnection connection)
		{
			SqlConnection sqlConnection = connection as SqlConnection;
			if (sqlConnection != null)
			{
				return sqlConnection.ObjectID;
			}
			return 0;
		}

		// Token: 0x06002503 RID: 9475 RVA: 0x0027A2F8 File Offset: 0x002796F8
		internal override void PermissionDemand(DbConnection outerConnection)
		{
			SqlConnection sqlConnection = outerConnection as SqlConnection;
			if (sqlConnection != null)
			{
				sqlConnection.PermissionDemand();
			}
		}

		// Token: 0x06002504 RID: 9476 RVA: 0x0027A318 File Offset: 0x00279718
		internal override void SetConnectionPoolGroup(DbConnection outerConnection, DbConnectionPoolGroup poolGroup)
		{
			SqlConnection sqlConnection = outerConnection as SqlConnection;
			if (sqlConnection != null)
			{
				sqlConnection.PoolGroup = poolGroup;
			}
		}

		// Token: 0x06002505 RID: 9477 RVA: 0x0027A338 File Offset: 0x00279738
		internal override void SetInnerConnectionEvent(DbConnection owningObject, DbConnectionInternal to)
		{
			SqlConnection sqlConnection = owningObject as SqlConnection;
			if (sqlConnection != null)
			{
				sqlConnection.SetInnerConnectionEvent(to);
			}
		}

		// Token: 0x06002506 RID: 9478 RVA: 0x0027A358 File Offset: 0x00279758
		internal override bool SetInnerConnectionFrom(DbConnection owningObject, DbConnectionInternal to, DbConnectionInternal from)
		{
			SqlConnection sqlConnection = owningObject as SqlConnection;
			return sqlConnection != null && sqlConnection.SetInnerConnectionFrom(to, from);
		}

		// Token: 0x06002507 RID: 9479 RVA: 0x0027A37C File Offset: 0x0027977C
		internal override void SetInnerConnectionTo(DbConnection owningObject, DbConnectionInternal to)
		{
			SqlConnection sqlConnection = owningObject as SqlConnection;
			if (sqlConnection != null)
			{
				sqlConnection.SetInnerConnectionTo(to);
			}
		}

		// Token: 0x04001780 RID: 6016
		private const string _metaDataXml = "MetaDataXml";

		// Token: 0x04001781 RID: 6017
		public static readonly SqlConnectionFactory SingletonInstance = new SqlConnectionFactory();
	}
}
