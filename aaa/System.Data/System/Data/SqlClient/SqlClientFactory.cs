using System;
using System.Data.Common;
using System.Data.Sql;
using System.Security;
using System.Security.Permissions;

namespace System.Data.SqlClient
{
	// Token: 0x020002BC RID: 700
	public sealed class SqlClientFactory : DbProviderFactory, IServiceProvider
	{
		// Token: 0x0600236C RID: 9068 RVA: 0x00271FCC File Offset: 0x002713CC
		private SqlClientFactory()
		{
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x0600236D RID: 9069 RVA: 0x00271FE0 File Offset: 0x002713E0
		public override bool CanCreateDataSourceEnumerator
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600236E RID: 9070 RVA: 0x00271FF0 File Offset: 0x002713F0
		public override DbCommand CreateCommand()
		{
			return new SqlCommand();
		}

		// Token: 0x0600236F RID: 9071 RVA: 0x00272004 File Offset: 0x00271404
		public override DbCommandBuilder CreateCommandBuilder()
		{
			return new SqlCommandBuilder();
		}

		// Token: 0x06002370 RID: 9072 RVA: 0x00272018 File Offset: 0x00271418
		public override DbConnection CreateConnection()
		{
			return new SqlConnection();
		}

		// Token: 0x06002371 RID: 9073 RVA: 0x0027202C File Offset: 0x0027142C
		public override DbConnectionStringBuilder CreateConnectionStringBuilder()
		{
			return new SqlConnectionStringBuilder();
		}

		// Token: 0x06002372 RID: 9074 RVA: 0x00272040 File Offset: 0x00271440
		public override DbDataAdapter CreateDataAdapter()
		{
			return new SqlDataAdapter();
		}

		// Token: 0x06002373 RID: 9075 RVA: 0x00272054 File Offset: 0x00271454
		public override DbParameter CreateParameter()
		{
			return new SqlParameter();
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x00272068 File Offset: 0x00271468
		public override CodeAccessPermission CreatePermission(PermissionState state)
		{
			return new SqlClientPermission(state);
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x0027207C File Offset: 0x0027147C
		public override DbDataSourceEnumerator CreateDataSourceEnumerator()
		{
			return SqlDataSourceEnumerator.Instance;
		}

		// Token: 0x06002376 RID: 9078 RVA: 0x00272090 File Offset: 0x00271490
		object IServiceProvider.GetService(Type serviceType)
		{
			object obj = null;
			if (serviceType == GreenMethods.SystemDataCommonDbProviderServices_Type)
			{
				obj = GreenMethods.SystemDataSqlClientSqlProviderServices_Instance();
			}
			return obj;
		}

		// Token: 0x040016FD RID: 5885
		public static readonly SqlClientFactory Instance = new SqlClientFactory();
	}
}
