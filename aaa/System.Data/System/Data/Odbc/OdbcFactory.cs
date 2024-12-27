using System;
using System.Data.Common;
using System.Security;
using System.Security.Permissions;

namespace System.Data.Odbc
{
	// Token: 0x020001EE RID: 494
	public sealed class OdbcFactory : DbProviderFactory
	{
		// Token: 0x06001B98 RID: 7064 RVA: 0x00248658 File Offset: 0x00247A58
		private OdbcFactory()
		{
		}

		// Token: 0x06001B99 RID: 7065 RVA: 0x0024866C File Offset: 0x00247A6C
		public override DbCommand CreateCommand()
		{
			return new OdbcCommand();
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x00248680 File Offset: 0x00247A80
		public override DbCommandBuilder CreateCommandBuilder()
		{
			return new OdbcCommandBuilder();
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x00248694 File Offset: 0x00247A94
		public override DbConnection CreateConnection()
		{
			return new OdbcConnection();
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x002486A8 File Offset: 0x00247AA8
		public override DbConnectionStringBuilder CreateConnectionStringBuilder()
		{
			return new OdbcConnectionStringBuilder();
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x002486BC File Offset: 0x00247ABC
		public override DbDataAdapter CreateDataAdapter()
		{
			return new OdbcDataAdapter();
		}

		// Token: 0x06001B9E RID: 7070 RVA: 0x002486D0 File Offset: 0x00247AD0
		public override DbParameter CreateParameter()
		{
			return new OdbcParameter();
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x002486E4 File Offset: 0x00247AE4
		public override CodeAccessPermission CreatePermission(PermissionState state)
		{
			return new OdbcPermission(state);
		}

		// Token: 0x0400100E RID: 4110
		public static readonly OdbcFactory Instance = new OdbcFactory();
	}
}
