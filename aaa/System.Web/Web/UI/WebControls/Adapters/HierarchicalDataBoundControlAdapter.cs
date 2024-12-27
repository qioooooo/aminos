using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.Adapters
{
	// Token: 0x0200069A RID: 1690
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HierarchicalDataBoundControlAdapter : WebControlAdapter
	{
		// Token: 0x17001509 RID: 5385
		// (get) Token: 0x060052AE RID: 21166 RVA: 0x0014D5EE File Offset: 0x0014C5EE
		protected new HierarchicalDataBoundControl Control
		{
			get
			{
				return (HierarchicalDataBoundControl)base.Control;
			}
		}

		// Token: 0x060052AF RID: 21167 RVA: 0x0014D5FB File Offset: 0x0014C5FB
		protected internal virtual void PerformDataBinding()
		{
			this.Control.PerformDataBinding();
		}
	}
}
