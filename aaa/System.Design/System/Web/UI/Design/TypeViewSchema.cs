using System;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x020003A4 RID: 932
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class TypeViewSchema : BaseTypeViewSchema
	{
		// Token: 0x06002263 RID: 8803 RVA: 0x000BC0F4 File Offset: 0x000BB0F4
		public TypeViewSchema(string viewName, Type type)
			: base(viewName, type)
		{
		}

		// Token: 0x06002264 RID: 8804 RVA: 0x000BC0FE File Offset: 0x000BB0FE
		protected override Type GetRowType(Type objectType)
		{
			return objectType;
		}
	}
}
