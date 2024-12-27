using System;
using System.Data.Common;
using System.Security;
using System.Security.Permissions;

namespace System.Data.OleDb
{
	// Token: 0x02000229 RID: 553
	public sealed class OleDbFactory : DbProviderFactory
	{
		// Token: 0x06001FA9 RID: 8105 RVA: 0x0025E918 File Offset: 0x0025DD18
		private OleDbFactory()
		{
		}

		// Token: 0x06001FAA RID: 8106 RVA: 0x0025E92C File Offset: 0x0025DD2C
		public override DbCommand CreateCommand()
		{
			return new OleDbCommand();
		}

		// Token: 0x06001FAB RID: 8107 RVA: 0x0025E940 File Offset: 0x0025DD40
		public override DbCommandBuilder CreateCommandBuilder()
		{
			return new OleDbCommandBuilder();
		}

		// Token: 0x06001FAC RID: 8108 RVA: 0x0025E954 File Offset: 0x0025DD54
		public override DbConnection CreateConnection()
		{
			return new OleDbConnection();
		}

		// Token: 0x06001FAD RID: 8109 RVA: 0x0025E968 File Offset: 0x0025DD68
		public override DbConnectionStringBuilder CreateConnectionStringBuilder()
		{
			return new OleDbConnectionStringBuilder();
		}

		// Token: 0x06001FAE RID: 8110 RVA: 0x0025E97C File Offset: 0x0025DD7C
		public override DbDataAdapter CreateDataAdapter()
		{
			return new OleDbDataAdapter();
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x0025E990 File Offset: 0x0025DD90
		public override DbParameter CreateParameter()
		{
			return new OleDbParameter();
		}

		// Token: 0x06001FB0 RID: 8112 RVA: 0x0025E9A4 File Offset: 0x0025DDA4
		public override CodeAccessPermission CreatePermission(PermissionState state)
		{
			return new OleDbPermission(state);
		}

		// Token: 0x040012E7 RID: 4839
		public static readonly OleDbFactory Instance = new OleDbFactory();
	}
}
