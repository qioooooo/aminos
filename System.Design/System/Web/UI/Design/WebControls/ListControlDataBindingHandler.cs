using System;
using System.ComponentModel.Design;
using System.Design;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ListControlDataBindingHandler : DataBindingHandler
	{
		public override void DataBindControl(IDesignerHost designerHost, Control control)
		{
			DataBinding dataBinding = ((IDataBindingsAccessor)control).DataBindings["DataSource"];
			if (dataBinding != null)
			{
				ListControl listControl = (ListControl)control;
				listControl.Items.Clear();
				listControl.Items.Add(SR.GetString("Sample_Databound_Text"));
			}
		}
	}
}
