using System;
using System.ComponentModel.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x02000342 RID: 834
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public abstract class DataBindingHandler
	{
		// Token: 0x06001F79 RID: 8057
		public abstract void DataBindControl(IDesignerHost designerHost, Control control);
	}
}
