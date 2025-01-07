using System;
using System.ComponentModel.Design;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class CalendarDataBindingHandler : DataBindingHandler
	{
		public override void DataBindControl(IDesignerHost designerHost, Control control)
		{
			Calendar calendar = (Calendar)control;
			DataBinding dataBinding = ((IDataBindingsAccessor)calendar).DataBindings["SelectedDate"];
			if (dataBinding != null)
			{
				calendar.SelectedDate = DateTime.Today;
			}
		}
	}
}
