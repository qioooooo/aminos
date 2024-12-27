using System;
using System.Security;
using System.Security.Permissions;

namespace System.Data.Common
{
	// Token: 0x02000142 RID: 322
	public abstract class DbProviderFactory
	{
		// Token: 0x170002EB RID: 747
		// (get) Token: 0x060014F5 RID: 5365 RVA: 0x00228CDC File Offset: 0x002280DC
		public virtual bool CanCreateDataSourceEnumerator
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x00228CEC File Offset: 0x002280EC
		public virtual DbCommand CreateCommand()
		{
			return null;
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x00228CFC File Offset: 0x002280FC
		public virtual DbCommandBuilder CreateCommandBuilder()
		{
			return null;
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x00228D0C File Offset: 0x0022810C
		public virtual DbConnection CreateConnection()
		{
			return null;
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x00228D1C File Offset: 0x0022811C
		public virtual DbConnectionStringBuilder CreateConnectionStringBuilder()
		{
			return null;
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x00228D2C File Offset: 0x0022812C
		public virtual DbDataAdapter CreateDataAdapter()
		{
			return null;
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x00228D3C File Offset: 0x0022813C
		public virtual DbParameter CreateParameter()
		{
			return null;
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x00228D4C File Offset: 0x0022814C
		public virtual CodeAccessPermission CreatePermission(PermissionState state)
		{
			return null;
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x00228D5C File Offset: 0x0022815C
		public virtual DbDataSourceEnumerator CreateDataSourceEnumerator()
		{
			return null;
		}
	}
}
