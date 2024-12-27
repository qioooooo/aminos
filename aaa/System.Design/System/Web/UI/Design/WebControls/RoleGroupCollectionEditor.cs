using System;
using System.ComponentModel.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020004A8 RID: 1192
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class RoleGroupCollectionEditor : CollectionEditor
	{
		// Token: 0x06002B37 RID: 11063 RVA: 0x000EF480 File Offset: 0x000EE480
		public RoleGroupCollectionEditor(Type type)
			: base(type)
		{
		}

		// Token: 0x06002B38 RID: 11064 RVA: 0x000EF489 File Offset: 0x000EE489
		protected override bool CanSelectMultipleInstances()
		{
			return false;
		}
	}
}
