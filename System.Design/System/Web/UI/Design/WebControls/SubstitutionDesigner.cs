using System;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	[SupportsPreviewControl(true)]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class SubstitutionDesigner : ControlDesigner
	{
		public override string GetDesignTimeHtml()
		{
			return this.GetEmptyDesignTimeHtml();
		}
	}
}
