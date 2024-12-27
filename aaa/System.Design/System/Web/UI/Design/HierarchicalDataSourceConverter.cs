using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x0200036D RID: 877
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class HierarchicalDataSourceConverter : DataSourceConverter
	{
		// Token: 0x060020DC RID: 8412 RVA: 0x000B8B10 File Offset: 0x000B7B10
		protected override bool IsValidDataSource(IComponent component)
		{
			Control control = component as Control;
			return control != null && !string.IsNullOrEmpty(control.ID) && component is IHierarchicalEnumerable;
		}
	}
}
