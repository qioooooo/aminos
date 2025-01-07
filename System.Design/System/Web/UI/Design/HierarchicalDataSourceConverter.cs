using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class HierarchicalDataSourceConverter : DataSourceConverter
	{
		protected override bool IsValidDataSource(IComponent component)
		{
			Control control = component as Control;
			return control != null && !string.IsNullOrEmpty(control.ID) && component is IHierarchicalEnumerable;
		}
	}
}
