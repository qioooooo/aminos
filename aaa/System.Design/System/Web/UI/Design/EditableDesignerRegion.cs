using System;

namespace System.Web.UI.Design
{
	// Token: 0x02000365 RID: 869
	public class EditableDesignerRegion : DesignerRegion
	{
		// Token: 0x060020AE RID: 8366 RVA: 0x000B76A7 File Offset: 0x000B66A7
		public EditableDesignerRegion(ControlDesigner owner, string name)
			: this(owner, name, false)
		{
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x000B76B2 File Offset: 0x000B66B2
		public EditableDesignerRegion(ControlDesigner owner, string name, bool serverControlsOnly)
			: base(owner, name)
		{
			this._serverControlsOnly = serverControlsOnly;
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x060020B0 RID: 8368 RVA: 0x000B76C3 File Offset: 0x000B66C3
		// (set) Token: 0x060020B1 RID: 8369 RVA: 0x000B76D1 File Offset: 0x000B66D1
		public virtual string Content
		{
			get
			{
				return base.Designer.GetEditableDesignerRegionContent(this);
			}
			set
			{
				base.Designer.SetEditableDesignerRegionContent(this, value);
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x060020B2 RID: 8370 RVA: 0x000B76E0 File Offset: 0x000B66E0
		// (set) Token: 0x060020B3 RID: 8371 RVA: 0x000B76E8 File Offset: 0x000B66E8
		public bool ServerControlsOnly
		{
			get
			{
				return this._serverControlsOnly;
			}
			set
			{
				this._serverControlsOnly = value;
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x060020B4 RID: 8372 RVA: 0x000B76F1 File Offset: 0x000B66F1
		// (set) Token: 0x060020B5 RID: 8373 RVA: 0x000B76F9 File Offset: 0x000B66F9
		public virtual bool SupportsDataBinding
		{
			get
			{
				return this._supportsDataBinding;
			}
			set
			{
				this._supportsDataBinding = value;
			}
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x000B7702 File Offset: 0x000B6702
		public virtual ViewRendering GetChildViewRendering(Control control)
		{
			return ControlDesigner.GetViewRendering(control);
		}

		// Token: 0x040017EA RID: 6122
		private bool _serverControlsOnly;

		// Token: 0x040017EB RID: 6123
		private bool _supportsDataBinding;
	}
}
