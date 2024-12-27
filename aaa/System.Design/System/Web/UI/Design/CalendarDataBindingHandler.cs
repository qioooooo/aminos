using System;
using System.ComponentModel.Design;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design
{
	// Token: 0x020003FF RID: 1023
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class CalendarDataBindingHandler : DataBindingHandler
	{
		// Token: 0x0600258F RID: 9615 RVA: 0x000CA884 File Offset: 0x000C9884
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
