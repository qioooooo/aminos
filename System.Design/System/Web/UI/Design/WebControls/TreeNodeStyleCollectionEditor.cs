using System;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class TreeNodeStyleCollectionEditor : StyleCollectionEditor
	{
		public TreeNodeStyleCollectionEditor(Type type)
			: base(type)
		{
		}

		protected override Type CreateCollectionItemType()
		{
			return typeof(TreeNodeStyle);
		}
	}
}
