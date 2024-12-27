using System;
using System.Data.Common;
using System.Security;
using System.Security.Permissions;

namespace System.Data.SqlClient
{
	// Token: 0x020002BE RID: 702
	[Serializable]
	public sealed class SqlClientPermission : DBDataPermission
	{
		// Token: 0x06002379 RID: 9081 RVA: 0x00272158 File Offset: 0x00271558
		[Obsolete("SqlClientPermission() has been deprecated.  Use the SqlClientPermission(PermissionState.None) constructor.  http://go.microsoft.com/fwlink/?linkid=14202", true)]
		public SqlClientPermission()
			: this(PermissionState.None)
		{
		}

		// Token: 0x0600237A RID: 9082 RVA: 0x0027216C File Offset: 0x0027156C
		public SqlClientPermission(PermissionState state)
			: base(state)
		{
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x00272180 File Offset: 0x00271580
		[Obsolete("SqlClientPermission(PermissionState state, Boolean allowBlankPassword) has been deprecated.  Use the SqlClientPermission(PermissionState.None) constructor.  http://go.microsoft.com/fwlink/?linkid=14202", true)]
		public SqlClientPermission(PermissionState state, bool allowBlankPassword)
			: this(state)
		{
			base.AllowBlankPassword = allowBlankPassword;
		}

		// Token: 0x0600237C RID: 9084 RVA: 0x0027219C File Offset: 0x0027159C
		private SqlClientPermission(SqlClientPermission permission)
			: base(permission)
		{
		}

		// Token: 0x0600237D RID: 9085 RVA: 0x002721B0 File Offset: 0x002715B0
		internal SqlClientPermission(SqlClientPermissionAttribute permissionAttribute)
			: base(permissionAttribute)
		{
		}

		// Token: 0x0600237E RID: 9086 RVA: 0x002721C4 File Offset: 0x002715C4
		internal SqlClientPermission(SqlConnectionString constr)
			: base(constr)
		{
			if (constr == null || constr.IsEmpty)
			{
				base.Add(ADP.StrEmpty, ADP.StrEmpty, KeyRestrictionBehavior.AllowOnly);
			}
		}

		// Token: 0x0600237F RID: 9087 RVA: 0x002721F4 File Offset: 0x002715F4
		public override void Add(string connectionString, string restrictions, KeyRestrictionBehavior behavior)
		{
			DBConnectionString dbconnectionString = new DBConnectionString(connectionString, restrictions, behavior, SqlConnectionString.GetParseSynonyms(), false);
			base.AddPermissionEntry(dbconnectionString);
		}

		// Token: 0x06002380 RID: 9088 RVA: 0x00272218 File Offset: 0x00271618
		public override IPermission Copy()
		{
			return new SqlClientPermission(this);
		}
	}
}
