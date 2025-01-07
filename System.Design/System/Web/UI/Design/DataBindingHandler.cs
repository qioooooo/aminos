using System;
using System.ComponentModel.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public abstract class DataBindingHandler
	{
		public abstract void DataBindControl(IDesignerHost designerHost, Control control);
	}
}
