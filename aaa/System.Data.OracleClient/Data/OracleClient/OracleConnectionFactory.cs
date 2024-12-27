using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Common;
using System.Data.ProviderBase;
using System.IO;
using System.Reflection;

namespace System.Data.OracleClient
{
	// Token: 0x02000056 RID: 86
	internal sealed class OracleConnectionFactory : DbConnectionFactory
	{
		// Token: 0x0600037E RID: 894 RVA: 0x00061E7C File Offset: 0x0006127C
		private OracleConnectionFactory()
			: base(OraclePerformanceCounters.SingletonInstance)
		{
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600037F RID: 895 RVA: 0x00061E94 File Offset: 0x00061294
		public override DbProviderFactory ProviderFactory
		{
			get
			{
				return OracleClientFactory.Instance;
			}
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00061EA8 File Offset: 0x000612A8
		protected override DbConnectionInternal CreateConnection(DbConnectionOptions options, object poolGroupProviderInfo, DbConnectionPool pool, DbConnection owningObject)
		{
			return new OracleInternalConnection(options as OracleConnectionString);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00061EC4 File Offset: 0x000612C4
		protected override DbConnectionOptions CreateConnectionOptions(string connectionOptions, DbConnectionOptions previous)
		{
			return new OracleConnectionString(connectionOptions);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00061EDC File Offset: 0x000612DC
		protected override DbConnectionPoolGroupOptions CreateConnectionPoolGroupOptions(DbConnectionOptions connectionOptions)
		{
			OracleConnectionString oracleConnectionString = (OracleConnectionString)connectionOptions;
			DbConnectionPoolGroupOptions dbConnectionPoolGroupOptions = null;
			if (oracleConnectionString.Pooling)
			{
				dbConnectionPoolGroupOptions = new DbConnectionPoolGroupOptions(oracleConnectionString.IntegratedSecurity, oracleConnectionString.MinPoolSize, oracleConnectionString.MaxPoolSize, 30000, oracleConnectionString.LoadBalanceTimeout, oracleConnectionString.Enlist, false);
			}
			return dbConnectionPoolGroupOptions;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00061F28 File Offset: 0x00061328
		protected override DbMetaDataFactory CreateMetaDataFactory(DbConnectionInternal internalConnection, out bool cacheMetaDataFactory)
		{
			cacheMetaDataFactory = false;
			NameValueCollection nameValueCollection = (NameValueCollection)PrivilegedConfigurationManager.GetSection("system.data.oracleclient");
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
				stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("System.Data.OracleClient.OracleMetaData.xml");
				cacheMetaDataFactory = true;
			}
			return new DbMetaDataFactory(stream, internalConnection.ServerVersion, internalConnection.ServerVersionNormalized);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00061F90 File Offset: 0x00061390
		internal override DbConnectionPoolGroup GetConnectionPoolGroup(DbConnection connection)
		{
			OracleConnection oracleConnection = connection as OracleConnection;
			if (oracleConnection != null)
			{
				return oracleConnection.PoolGroup;
			}
			return null;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00061FB0 File Offset: 0x000613B0
		internal override DbConnectionInternal GetInnerConnection(DbConnection connection)
		{
			OracleConnection oracleConnection = connection as OracleConnection;
			if (oracleConnection != null)
			{
				return oracleConnection.InnerConnection;
			}
			return null;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00061FD0 File Offset: 0x000613D0
		protected override int GetObjectId(DbConnection connection)
		{
			OracleConnection oracleConnection = connection as OracleConnection;
			if (oracleConnection != null)
			{
				return oracleConnection.ObjectID;
			}
			return 0;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00061FF0 File Offset: 0x000613F0
		internal override void PermissionDemand(DbConnection outerConnection)
		{
			OracleConnection oracleConnection = outerConnection as OracleConnection;
			if (oracleConnection != null)
			{
				oracleConnection.PermissionDemand();
			}
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00062010 File Offset: 0x00061410
		internal override void SetConnectionPoolGroup(DbConnection outerConnection, DbConnectionPoolGroup poolGroup)
		{
			OracleConnection oracleConnection = outerConnection as OracleConnection;
			if (oracleConnection != null)
			{
				oracleConnection.PoolGroup = poolGroup;
			}
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00062030 File Offset: 0x00061430
		internal override void SetInnerConnectionEvent(DbConnection owningObject, DbConnectionInternal to)
		{
			OracleConnection oracleConnection = owningObject as OracleConnection;
			if (oracleConnection != null)
			{
				oracleConnection.SetInnerConnectionEvent(to);
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00062050 File Offset: 0x00061450
		internal override bool SetInnerConnectionFrom(DbConnection owningObject, DbConnectionInternal to, DbConnectionInternal from)
		{
			OracleConnection oracleConnection = owningObject as OracleConnection;
			return oracleConnection != null && oracleConnection.SetInnerConnectionFrom(to, from);
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00062074 File Offset: 0x00061474
		internal override void SetInnerConnectionTo(DbConnection owningObject, DbConnectionInternal to)
		{
			OracleConnection oracleConnection = owningObject as OracleConnection;
			if (oracleConnection != null)
			{
				oracleConnection.SetInnerConnectionTo(to);
			}
		}

		// Token: 0x0400039C RID: 924
		public const string _metaDataXml = "MetaDataXml";

		// Token: 0x0400039D RID: 925
		public static readonly OracleConnectionFactory SingletonInstance = new OracleConnectionFactory();
	}
}
