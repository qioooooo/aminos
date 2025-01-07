using System;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class MultiViewDesigner : ContainerControlDesigner
	{
		public MultiViewDesigner()
		{
			base.FrameStyleInternal.Width = Unit.Percentage(100.0);
		}
	}
}
