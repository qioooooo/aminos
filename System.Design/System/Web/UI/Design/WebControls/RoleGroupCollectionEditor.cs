using System;
using System.ComponentModel.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class RoleGroupCollectionEditor : CollectionEditor
	{
		public RoleGroupCollectionEditor(Type type)
			: base(type)
		{
		}

		protected override bool CanSelectMultipleInstances()
		{
			return false;
		}
	}
}
