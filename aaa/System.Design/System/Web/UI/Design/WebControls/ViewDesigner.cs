using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020004FD RID: 1277
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ViewDesigner : ContainerControlDesigner
	{
		// Token: 0x06002DAE RID: 11694 RVA: 0x001034F7 File Offset: 0x001024F7
		public ViewDesigner()
		{
			base.FrameStyleInternal.Width = Unit.Percentage(100.0);
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x00103518 File Offset: 0x00102518
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(View));
			base.Initialize(component);
		}

		// Token: 0x06002DB0 RID: 11696 RVA: 0x00103531 File Offset: 0x00102531
		public override string GetDesignTimeHtml(DesignerRegionCollection regions)
		{
			return this.GetDesignTimeHtmlHelper(true, regions);
		}

		// Token: 0x06002DB1 RID: 11697 RVA: 0x0010353B File Offset: 0x0010253B
		public override string GetDesignTimeHtml()
		{
			return this.GetDesignTimeHtmlHelper(false, null);
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x00103548 File Offset: 0x00102548
		private string GetDesignTimeHtmlHelper(bool useRegions, DesignerRegionCollection regions)
		{
			View view = (View)base.Component;
			if (!(view.Parent is MultiView))
			{
				return base.CreateInvalidParentDesignTimeHtml(typeof(View), typeof(MultiView));
			}
			if (useRegions)
			{
				return base.GetDesignTimeHtml(regions);
			}
			return base.GetDesignTimeHtml();
		}
	}
}
