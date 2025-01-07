using System;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class HtmlIntrinsicControlDesigner : HtmlControlDesigner
	{
	}
}
