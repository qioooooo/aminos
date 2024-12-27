using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200040C RID: 1036
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class CompositeControlDesigner : ControlDesigner
	{
		// Token: 0x060025DC RID: 9692 RVA: 0x000CBF0C File Offset: 0x000CAF0C
		protected virtual void CreateChildControls()
		{
			ICompositeControlDesignerAccessor compositeControlDesignerAccessor = (ICompositeControlDesignerAccessor)base.ViewControl;
			compositeControlDesignerAccessor.RecreateChildControls();
		}

		// Token: 0x060025DD RID: 9693 RVA: 0x000CBF2B File Offset: 0x000CAF2B
		public override string GetDesignTimeHtml()
		{
			this.CreateChildControls();
			return base.GetDesignTimeHtml();
		}

		// Token: 0x060025DE RID: 9694 RVA: 0x000CBF39 File Offset: 0x000CAF39
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(INamingContainer));
			base.Initialize(component);
		}
	}
}
