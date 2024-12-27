using System;
using System.Data.Common;
using System.Security;
using System.Security.Permissions;

namespace System.Data.Odbc
{
	// Token: 0x020001FA RID: 506
	[Serializable]
	public sealed class OdbcPermission : DBDataPermission
	{
		// Token: 0x06001C5C RID: 7260 RVA: 0x0024D06C File Offset: 0x0024C46C
		[Obsolete("OdbcPermission() has been deprecated.  Use the OdbcPermission(PermissionState.None) constructor.  http://go.microsoft.com/fwlink/?linkid=14202", true)]
		public OdbcPermission()
			: this(PermissionState.None)
		{
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x0024D080 File Offset: 0x0024C480
		public OdbcPermission(PermissionState state)
			: base(state)
		{
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x0024D094 File Offset: 0x0024C494
		[Obsolete("OdbcPermission(PermissionState state, Boolean allowBlankPassword) has been deprecated.  Use the OdbcPermission(PermissionState.None) constructor.  http://go.microsoft.com/fwlink/?linkid=14202", true)]
		public OdbcPermission(PermissionState state, bool allowBlankPassword)
			: this(state)
		{
			base.AllowBlankPassword = allowBlankPassword;
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x0024D0B0 File Offset: 0x0024C4B0
		private OdbcPermission(OdbcPermission permission)
			: base(permission)
		{
		}

		// Token: 0x06001C60 RID: 7264 RVA: 0x0024D0C4 File Offset: 0x0024C4C4
		internal OdbcPermission(OdbcPermissionAttribute permissionAttribute)
			: base(permissionAttribute)
		{
		}

		// Token: 0x06001C61 RID: 7265 RVA: 0x0024D0D8 File Offset: 0x0024C4D8
		internal OdbcPermission(OdbcConnectionString constr)
			: base(constr)
		{
			if (constr == null || constr.IsEmpty)
			{
				base.Add(ADP.StrEmpty, ADP.StrEmpty, KeyRestrictionBehavior.AllowOnly);
			}
		}

		// Token: 0x06001C62 RID: 7266 RVA: 0x0024D108 File Offset: 0x0024C508
		public override void Add(string connectionString, string restrictions, KeyRestrictionBehavior behavior)
		{
			DBConnectionString dbconnectionString = new DBConnectionString(connectionString, restrictions, behavior, null, true);
			base.AddPermissionEntry(dbconnectionString);
		}

		// Token: 0x06001C63 RID: 7267 RVA: 0x0024D128 File Offset: 0x0024C528
		public override IPermission Copy()
		{
			return new OdbcPermission(this);
		}
	}
}
