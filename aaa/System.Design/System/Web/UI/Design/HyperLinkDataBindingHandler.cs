using System;
using System.ComponentModel.Design;
using System.Design;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design
{
	// Token: 0x0200045A RID: 1114
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class HyperLinkDataBindingHandler : DataBindingHandler
	{
		// Token: 0x060028AC RID: 10412 RVA: 0x000DF50C File Offset: 0x000DE50C
		public override void DataBindControl(IDesignerHost designerHost, Control control)
		{
			DataBindingCollection dataBindings = ((IDataBindingsAccessor)control).DataBindings;
			DataBinding dataBinding = dataBindings["Text"];
			DataBinding dataBinding2 = dataBindings["NavigateUrl"];
			if (dataBinding != null || dataBinding2 != null)
			{
				HyperLink hyperLink = (HyperLink)control;
				if (dataBinding != null)
				{
					hyperLink.Text = SR.GetString("Sample_Databound_Text");
				}
				if (dataBinding2 != null)
				{
					hyperLink.NavigateUrl = "url";
				}
			}
		}
	}
}
