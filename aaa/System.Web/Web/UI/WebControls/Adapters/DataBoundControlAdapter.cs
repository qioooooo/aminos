using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.Adapters
{
	// Token: 0x02000698 RID: 1688
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class DataBoundControlAdapter : WebControlAdapter
	{
		// Token: 0x17001508 RID: 5384
		// (get) Token: 0x060052A9 RID: 21161 RVA: 0x0014D5A7 File Offset: 0x0014C5A7
		protected new DataBoundControl Control
		{
			get
			{
				return (DataBoundControl)base.Control;
			}
		}

		// Token: 0x060052AA RID: 21162 RVA: 0x0014D5B4 File Offset: 0x0014C5B4
		protected internal virtual void PerformDataBinding(IEnumerable data)
		{
			this.Control.PerformDataBinding(data);
		}
	}
}
