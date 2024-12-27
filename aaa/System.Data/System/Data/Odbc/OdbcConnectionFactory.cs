using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Common;
using System.Data.ProviderBase;
using System.IO;
using System.Reflection;

namespace System.Data.Odbc
{
	// Token: 0x020001D8 RID: 472
	internal sealed class OdbcConnectionFactory : DbConnectionFactory
	{
		// Token: 0x06001A57 RID: 6743 RVA: 0x00242D20 File Offset: 0x00242120
		private OdbcConnectionFactory()
		{
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06001A58 RID: 6744 RVA: 0x00242D34 File Offset: 0x00242134
		public override DbProviderFactory ProviderFactory
		{
			get
			{
				return OdbcFactory.Instance;
			}
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x00242D48 File Offset: 0x00242148
		protected override DbConnectionInternal CreateConnection(DbConnectionOptions options, object poolGroupProviderInfo, DbConnectionPool pool, DbConnection owningObject)
		{
			return new OdbcConnectionOpen(owningObject as OdbcConnection, options as OdbcConnectionString);
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x00242D6C File Offset: 0x0024216C
		protected override DbConnectionOptions CreateConnectionOptions(string connectionString, DbConnectionOptions previous)
		{
			return new OdbcConnectionString(connectionString, null != previous);
		}

		// Token: 0x06001A5B RID: 6747 RVA: 0x00242D88 File Offset: 0x00242188
		protected override DbConnectionPoolGroupOptions CreateConnectionPoolGroupOptions(DbConnectionOptions connectionOptions)
		{
			return null;
		}

		// Token: 0x06001A5C RID: 6748 RVA: 0x00242D98 File Offset: 0x00242198
		internal override DbConnectionPoolGroupProviderInfo CreateConnectionPoolGroupProviderInfo(DbConnectionOptions connectionOptions)
		{
			return new OdbcConnectionPoolGroupProviderInfo();
		}

		// Token: 0x06001A5D RID: 6749 RVA: 0x00242DAC File Offset: 0x002421AC
		protected override DbMetaDataFactory CreateMetaDataFactory(DbConnectionInternal internalConnection, out bool cacheMetaDataFactory)
		{
			cacheMetaDataFactory = false;
			OdbcConnection outerConnection = ((OdbcConnectionOpen)internalConnection).OuterConnection;
			NameValueCollection nameValueCollection = (NameValueCollection)PrivilegedConfigurationManager.GetSection("system.data.odbc");
			Stream stream = null;
			object obj = null;
			string infoStringUnhandled = outerConnection.GetInfoStringUnhandled(ODBC32.SQL_INFO.DRIVER_NAME);
			if (infoStringUnhandled != null)
			{
				obj = infoStringUnhandled;
			}
			if (nameValueCollection != null)
			{
				string[] array = null;
				string text = null;
				if (obj != null)
				{
					text = (string)obj + ":MetaDataXml";
					array = nameValueCollection.GetValues(text);
				}
				if (array == null)
				{
					text = "defaultMetaDataXml";
					array = nameValueCollection.GetValues(text);
				}
				if (array != null)
				{
					stream = ADP.GetXmlStreamFromValues(array, text);
				}
			}
			if (stream == null)
			{
				stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("System.Data.Odbc.OdbcMetaData.xml");
				cacheMetaDataFactory = true;
			}
			string infoStringUnhandled2 = outerConnection.GetInfoStringUnhandled(ODBC32.SQL_INFO.DBMS_VER);
			return new OdbcMetaDataFactory(stream, infoStringUnhandled2, infoStringUnhandled2, outerConnection);
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x00242E60 File Offset: 0x00242260
		internal override DbConnectionPoolGroup GetConnectionPoolGroup(DbConnection connection)
		{
			OdbcConnection odbcConnection = connection as OdbcConnection;
			if (odbcConnection != null)
			{
				return odbcConnection.PoolGroup;
			}
			return null;
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x00242E80 File Offset: 0x00242280
		internal override DbConnectionInternal GetInnerConnection(DbConnection connection)
		{
			OdbcConnection odbcConnection = connection as OdbcConnection;
			if (odbcConnection != null)
			{
				return odbcConnection.InnerConnection;
			}
			return null;
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x00242EA0 File Offset: 0x002422A0
		protected override int GetObjectId(DbConnection connection)
		{
			OdbcConnection odbcConnection = connection as OdbcConnection;
			if (odbcConnection != null)
			{
				return odbcConnection.ObjectID;
			}
			return 0;
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x00242EC0 File Offset: 0x002422C0
		internal override void PermissionDemand(DbConnection outerConnection)
		{
			OdbcConnection odbcConnection = outerConnection as OdbcConnection;
			if (odbcConnection != null)
			{
				odbcConnection.PermissionDemand();
			}
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x00242EE0 File Offset: 0x002422E0
		internal override void SetConnectionPoolGroup(DbConnection outerConnection, DbConnectionPoolGroup poolGroup)
		{
			OdbcConnection odbcConnection = outerConnection as OdbcConnection;
			if (odbcConnection != null)
			{
				odbcConnection.PoolGroup = poolGroup;
			}
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x00242F00 File Offset: 0x00242300
		internal override void SetInnerConnectionEvent(DbConnection owningObject, DbConnectionInternal to)
		{
			OdbcConnection odbcConnection = owningObject as OdbcConnection;
			if (odbcConnection != null)
			{
				odbcConnection.SetInnerConnectionEvent(to);
			}
		}

		// Token: 0x06001A64 RID: 6756 RVA: 0x00242F20 File Offset: 0x00242320
		internal override bool SetInnerConnectionFrom(DbConnection owningObject, DbConnectionInternal to, DbConnectionInternal from)
		{
			OdbcConnection odbcConnection = owningObject as OdbcConnection;
			return odbcConnection != null && odbcConnection.SetInnerConnectionFrom(to, from);
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x00242F44 File Offset: 0x00242344
		internal override void SetInnerConnectionTo(DbConnection owningObject, DbConnectionInternal to)
		{
			OdbcConnection odbcConnection = owningObject as OdbcConnection;
			if (odbcConnection != null)
			{
				odbcConnection.SetInnerConnectionTo(to);
			}
		}

		// Token: 0x04000F98 RID: 3992
		private const string _MetaData = ":MetaDataXml";

		// Token: 0x04000F99 RID: 3993
		private const string _defaultMetaDataXml = "defaultMetaDataXml";

		// Token: 0x04000F9A RID: 3994
		public static readonly OdbcConnectionFactory SingletonInstance = new OdbcConnectionFactory();
	}
}
