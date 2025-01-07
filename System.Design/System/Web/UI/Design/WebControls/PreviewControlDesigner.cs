using System;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	[SupportsPreviewControl(true)]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class PreviewControlDesigner : ControlDesigner
	{
		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}
	}
}
