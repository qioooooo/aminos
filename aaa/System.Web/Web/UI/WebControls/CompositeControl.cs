using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004E0 RID: 1248
	[Designer("System.Web.UI.Design.WebControls.CompositeControlDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class CompositeControl : WebControl, INamingContainer, ICompositeControlDesignerAccessor
	{
		// Token: 0x17000DE1 RID: 3553
		// (get) Token: 0x06003C22 RID: 15394 RVA: 0x000FD3E9 File Offset: 0x000FC3E9
		public override ControlCollection Controls
		{
			get
			{
				this.EnsureChildControls();
				return base.Controls;
			}
		}

		// Token: 0x06003C23 RID: 15395 RVA: 0x000FD3F7 File Offset: 0x000FC3F7
		public override void DataBind()
		{
			this.OnDataBinding(EventArgs.Empty);
			this.EnsureChildControls();
			this.DataBindChildren();
		}

		// Token: 0x06003C24 RID: 15396 RVA: 0x000FD410 File Offset: 0x000FC410
		protected virtual void RecreateChildControls()
		{
			base.ChildControlsCreated = false;
			this.EnsureChildControls();
		}

		// Token: 0x06003C25 RID: 15397 RVA: 0x000FD41F File Offset: 0x000FC41F
		protected internal override void Render(HtmlTextWriter writer)
		{
			if (base.DesignMode)
			{
				this.EnsureChildControls();
			}
			base.Render(writer);
		}

		// Token: 0x06003C26 RID: 15398 RVA: 0x000FD436 File Offset: 0x000FC436
		void ICompositeControlDesignerAccessor.RecreateChildControls()
		{
			this.RecreateChildControls();
		}
	}
}
