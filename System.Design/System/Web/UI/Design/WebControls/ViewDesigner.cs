using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ViewDesigner : ContainerControlDesigner
	{
		public ViewDesigner()
		{
			base.FrameStyleInternal.Width = Unit.Percentage(100.0);
		}

		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(View));
			base.Initialize(component);
		}

		public override string GetDesignTimeHtml(DesignerRegionCollection regions)
		{
			return this.GetDesignTimeHtmlHelper(true, regions);
		}

		public override string GetDesignTimeHtml()
		{
			return this.GetDesignTimeHtmlHelper(false, null);
		}

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
