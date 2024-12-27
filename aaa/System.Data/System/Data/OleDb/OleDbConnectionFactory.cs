using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Common;
using System.Data.ProviderBase;
using System.IO;
using System.Reflection;

namespace System.Data.OleDb
{
	// Token: 0x02000215 RID: 533
	internal sealed class OleDbConnectionFactory : DbConnectionFactory
	{
		// Token: 0x06001E7C RID: 7804 RVA: 0x00257648 File Offset: 0x00256A48
		private OleDbConnectionFactory()
		{
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06001E7D RID: 7805 RVA: 0x0025765C File Offset: 0x00256A5C
		public override DbProviderFactory ProviderFactory
		{
			get
			{
				return OleDbFactory.Instance;
			}
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x00257670 File Offset: 0x00256A70
		protected override DbConnectionInternal CreateConnection(DbConnectionOptions options, object poolGroupProviderInfo, DbConnectionPool pool, DbConnection owningObject)
		{
			return new OleDbConnectionInternal((OleDbConnectionString)options, (OleDbConnection)owningObject);
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x00257694 File Offset: 0x00256A94
		protected override DbConnectionOptions CreateConnectionOptions(string connectionString, DbConnectionOptions previous)
		{
			return new OleDbConnectionString(connectionString, null != previous);
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x002576B0 File Offset: 0x00256AB0
		protected override DbMetaDataFactory CreateMetaDataFactory(DbConnectionInternal internalConnection, out bool cacheMetaDataFactory)
		{
			cacheMetaDataFactory = false;
			OleDbConnectionInternal oleDbConnectionInternal = (OleDbConnectionInternal)internalConnection;
			OleDbConnection connection = oleDbConnectionInternal.Connection;
			NameValueCollection nameValueCollection = (NameValueCollection)PrivilegedConfigurationManager.GetSection("system.data.oledb");
			Stream stream = null;
			string text = connection.GetDataSourcePropertyValue(OleDbPropertySetGuid.DataSourceInfo, 96) as string;
			if (nameValueCollection != null)
			{
				string[] array = null;
				string text2 = null;
				if (text != null)
				{
					text2 = text + ":MetaDataXml";
					array = nameValueCollection.GetValues(text2);
				}
				if (array == null)
				{
					text2 = "defaultMetaDataXml";
					array = nameValueCollection.GetValues(text2);
				}
				if (array != null)
				{
					stream = ADP.GetXmlStreamFromValues(array, text2);
				}
			}
			if (stream == null)
			{
				stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("System.Data.OleDb.OleDbMetaData.xml");
				cacheMetaDataFactory = true;
			}
			return new OleDbMetaDataFactory(stream, oleDbConnectionInternal.ServerVersion, oleDbConnectionInternal.ServerVersion, oleDbConnectionInternal.GetSchemaRowsetInformation());
		}

		// Token: 0x06001E81 RID: 7809 RVA: 0x00257764 File Offset: 0x00256B64
		protected override DbConnectionPoolGroupOptions CreateConnectionPoolGroupOptions(DbConnectionOptions connectionOptions)
		{
			return null;
		}

		// Token: 0x06001E82 RID: 7810 RVA: 0x00257774 File Offset: 0x00256B74
		internal override DbConnectionPoolGroupProviderInfo CreateConnectionPoolGroupProviderInfo(DbConnectionOptions connectionOptions)
		{
			return new OleDbConnectionPoolGroupProviderInfo();
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x00257788 File Offset: 0x00256B88
		internal override DbConnectionPoolGroup GetConnectionPoolGroup(DbConnection connection)
		{
			OleDbConnection oleDbConnection = connection as OleDbConnection;
			if (oleDbConnection != null)
			{
				return oleDbConnection.PoolGroup;
			}
			return null;
		}

		// Token: 0x06001E84 RID: 7812 RVA: 0x002577A8 File Offset: 0x00256BA8
		internal override DbConnectionInternal GetInnerConnection(DbConnection connection)
		{
			OleDbConnection oleDbConnection = connection as OleDbConnection;
			if (oleDbConnection != null)
			{
				return oleDbConnection.InnerConnection;
			}
			return null;
		}

		// Token: 0x06001E85 RID: 7813 RVA: 0x002577C8 File Offset: 0x00256BC8
		protected override int GetObjectId(DbConnection connection)
		{
			OleDbConnection oleDbConnection = connection as OleDbConnection;
			if (oleDbConnection != null)
			{
				return oleDbConnection.ObjectID;
			}
			return 0;
		}

		// Token: 0x06001E86 RID: 7814 RVA: 0x002577E8 File Offset: 0x00256BE8
		internal override void PermissionDemand(DbConnection outerConnection)
		{
			OleDbConnection oleDbConnection = outerConnection as OleDbConnection;
			if (oleDbConnection != null)
			{
				oleDbConnection.PermissionDemand();
			}
		}

		// Token: 0x06001E87 RID: 7815 RVA: 0x00257808 File Offset: 0x00256C08
		internal override void SetConnectionPoolGroup(DbConnection outerConnection, DbConnectionPoolGroup poolGroup)
		{
			OleDbConnection oleDbConnection = outerConnection as OleDbConnection;
			if (oleDbConnection != null)
			{
				oleDbConnection.PoolGroup = poolGroup;
			}
		}

		// Token: 0x06001E88 RID: 7816 RVA: 0x00257828 File Offset: 0x00256C28
		internal override void SetInnerConnectionEvent(DbConnection owningObject, DbConnectionInternal to)
		{
			OleDbConnection oleDbConnection = owningObject as OleDbConnection;
			if (oleDbConnection != null)
			{
				oleDbConnection.SetInnerConnectionEvent(to);
			}
		}

		// Token: 0x06001E89 RID: 7817 RVA: 0x00257848 File Offset: 0x00256C48
		internal override bool SetInnerConnectionFrom(DbConnection owningObject, DbConnectionInternal to, DbConnectionInternal from)
		{
			OleDbConnection oleDbConnection = owningObject as OleDbConnection;
			return oleDbConnection != null && oleDbConnection.SetInnerConnectionFrom(to, from);
		}

		// Token: 0x06001E8A RID: 7818 RVA: 0x0025786C File Offset: 0x00256C6C
		internal override void SetInnerConnectionTo(DbConnection owningObject, DbConnectionInternal to)
		{
			OleDbConnection oleDbConnection = owningObject as OleDbConnection;
			if (oleDbConnection != null)
			{
				oleDbConnection.SetInnerConnectionTo(to);
			}
		}

		// Token: 0x04001265 RID: 4709
		private const string _metaDataXml = ":MetaDataXml";

		// Token: 0x04001266 RID: 4710
		private const string _defaultMetaDataXml = "defaultMetaDataXml";

		// Token: 0x04001267 RID: 4711
		public static readonly OleDbConnectionFactory SingletonInstance = new OleDbConnectionFactory();
	}
}
