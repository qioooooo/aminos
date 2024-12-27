using System;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000481 RID: 1153
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class MultiViewDesigner : ContainerControlDesigner
	{
		// Token: 0x060029D1 RID: 10705 RVA: 0x000E54C6 File Offset: 0x000E44C6
		public MultiViewDesigner()
		{
			base.FrameStyleInternal.Width = Unit.Percentage(100.0);
		}
	}
}
