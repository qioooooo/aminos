using System;
using System.ComponentModel.Design;
using System.Design;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000461 RID: 1121
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ListControlDataBindingHandler : DataBindingHandler
	{
		// Token: 0x060028D6 RID: 10454 RVA: 0x000E04C4 File Offset: 0x000DF4C4
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
