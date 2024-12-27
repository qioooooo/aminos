using System;
using System.Data.Common;
using System.Security;
using System.Security.Permissions;

namespace System.Data.Odbc
{
	// Token: 0x020001E0 RID: 480
	internal sealed class OdbcConnectionString : DbConnectionOptions
	{
		// Token: 0x06001AE1 RID: 6881 RVA: 0x002444A0 File Offset: 0x002438A0
		internal OdbcConnectionString(string connectionString, bool validate)
			: base(connectionString, null, true)
		{
			if (!validate)
			{
				string text = null;
				int num = 0;
				this._expandedConnectionString = base.ExpandDataDirectories(ref text, ref num);
			}
			if ((validate || this._expandedConnectionString == null) && connectionString != null && 1024 < connectionString.Length)
			{
				throw ODBC.ConnectionStringTooLong();
			}
		}

		// Token: 0x06001AE2 RID: 6882 RVA: 0x002444F0 File Offset: 0x002438F0
		protected internal override PermissionSet CreatePermissionSet()
		{
			PermissionSet permissionSet;
			if (base.ContainsKey("savefile"))
			{
				permissionSet = new NamedPermissionSet("FullTrust");
			}
			else
			{
				permissionSet = new PermissionSet(PermissionState.None);
				permissionSet.AddPermission(new OdbcPermission(this));
			}
			return permissionSet;
		}

		// Token: 0x06001AE3 RID: 6883 RVA: 0x0024452C File Offset: 0x0024392C
		protected internal override string Expand()
		{
			if (this._expandedConnectionString != null)
			{
				return this._expandedConnectionString;
			}
			return base.Expand();
		}

		// Token: 0x04000FC7 RID: 4039
		private readonly string _expandedConnectionString;
	}
}
