using System;
using System.Data.Common;
using System.Security;
using System.Security.Permissions;

namespace System.Data.SqlClient
{
	// Token: 0x020002BF RID: 703
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class SqlClientPermissionAttribute : DBDataPermissionAttribute
	{
		// Token: 0x06002381 RID: 9089 RVA: 0x0027222C File Offset: 0x0027162C
		public SqlClientPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x06002382 RID: 9090 RVA: 0x00272240 File Offset: 0x00271640
		public override IPermission CreatePermission()
		{
			return new SqlClientPermission(this);
		}
	}
}
