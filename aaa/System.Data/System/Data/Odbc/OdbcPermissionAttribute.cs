using System;
using System.Data.Common;
using System.Security;
using System.Security.Permissions;

namespace System.Data.Odbc
{
	// Token: 0x020001FB RID: 507
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class OdbcPermissionAttribute : DBDataPermissionAttribute
	{
		// Token: 0x06001C64 RID: 7268 RVA: 0x0024D13C File Offset: 0x0024C53C
		public OdbcPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x0024D150 File Offset: 0x0024C550
		public override IPermission CreatePermission()
		{
			return new OdbcPermission(this);
		}
	}
}
