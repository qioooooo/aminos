using System;
using System.ComponentModel.Design;
using System.Reflection;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class TableRowsCollectionEditor : CollectionEditor
	{
		public TableRowsCollectionEditor(Type type)
			: base(type)
		{
		}

		protected override bool CanSelectMultipleInstances()
		{
			return false;
		}

		protected override object CreateInstance(Type itemType)
		{
			return Activator.CreateInstance(itemType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, null, null);
		}
	}
}
