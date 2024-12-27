using System;
using System.Data.SqlClient;
using System.Security;
using System.Security.Permissions;

namespace System.Web.Util
{
	// Token: 0x02000779 RID: 1913
	internal static class Permission
	{
		// Token: 0x06005C70 RID: 23664 RVA: 0x00172B6C File Offset: 0x00171B6C
		internal static bool HasSqlClientPermission()
		{
			NamedPermissionSet namedPermissionSet = HttpRuntime.NamedPermissionSet;
			if (namedPermissionSet == null)
			{
				return true;
			}
			IPermission permission = namedPermissionSet.GetPermission(typeof(SqlClientPermission));
			if (permission == null)
			{
				return false;
			}
			IPermission permission2 = null;
			try
			{
				permission2 = new SqlClientPermission(PermissionState.Unrestricted);
			}
			catch
			{
				return false;
			}
			return permission2.IsSubsetOf(permission);
		}
	}
}
