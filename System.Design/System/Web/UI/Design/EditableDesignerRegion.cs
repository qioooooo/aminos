using System;

namespace System.Web.UI.Design
{
	public class EditableDesignerRegion : DesignerRegion
	{
		public EditableDesignerRegion(ControlDesigner owner, string name)
			: this(owner, name, false)
		{
		}

		public EditableDesignerRegion(ControlDesigner owner, string name, bool serverControlsOnly)
			: base(owner, name)
		{
			this._serverControlsOnly = serverControlsOnly;
		}

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

		public virtual ViewRendering GetChildViewRendering(Control control)
		{
			return ControlDesigner.GetViewRendering(control);
		}

		private bool _serverControlsOnly;

		private bool _supportsDataBinding;
	}
}
