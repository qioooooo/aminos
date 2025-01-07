using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class CompositeControlDesigner : ControlDesigner
	{
		protected virtual void CreateChildControls()
		{
			ICompositeControlDesignerAccessor compositeControlDesignerAccessor = (ICompositeControlDesignerAccessor)base.ViewControl;
			compositeControlDesignerAccessor.RecreateChildControls();
		}

		public override string GetDesignTimeHtml()
		{
			this.CreateChildControls();
			return base.GetDesignTimeHtml();
		}

		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(INamingContainer));
			base.Initialize(component);
		}
	}
}
