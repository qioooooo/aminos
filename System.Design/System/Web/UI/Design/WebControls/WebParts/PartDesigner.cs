using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Security.Permissions;
using System.Web.UI.WebControls.WebParts;

namespace System.Web.UI.Design.WebControls.WebParts
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public abstract class PartDesigner : CompositeControlDesigner
	{
		internal PartDesigner()
		{
		}

		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

		internal static Control GetViewControl(Control control)
		{
			ControlDesigner designer = PartDesigner.GetDesigner(control);
			if (designer != null)
			{
				return designer.ViewControl;
			}
			return control;
		}

		private static ControlDesigner GetDesigner(Control control)
		{
			ControlDesigner controlDesigner = null;
			ISite site = control.Site;
			if (site != null)
			{
				IDesignerHost designerHost = (IDesignerHost)site.GetService(typeof(IDesignerHost));
				controlDesigner = designerHost.GetDesigner(control) as ControlDesigner;
			}
			return controlDesigner;
		}

		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(Part));
			base.Initialize(component);
		}
	}
}
