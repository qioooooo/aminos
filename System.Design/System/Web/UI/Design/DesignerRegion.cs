using System;
using System.Drawing;

namespace System.Web.UI.Design
{
	public class DesignerRegion : DesignerObject
	{
		public DesignerRegion(ControlDesigner designer, string name)
			: this(designer, name, false)
		{
		}

		public DesignerRegion(ControlDesigner designer, string name, bool selectable)
			: base(designer, name)
		{
			this._selectable = selectable;
		}

		public virtual string Description
		{
			get
			{
				if (this._description == null)
				{
					return string.Empty;
				}
				return this._description;
			}
			set
			{
				this._description = value;
			}
		}

		public virtual string DisplayName
		{
			get
			{
				if (this._displayName == null)
				{
					return string.Empty;
				}
				return this._displayName;
			}
			set
			{
				this._displayName = value;
			}
		}

		public bool EnsureSize
		{
			get
			{
				return this._ensureSize;
			}
			set
			{
				this._ensureSize = value;
			}
		}

		public virtual bool Highlight
		{
			get
			{
				return this._highlight;
			}
			set
			{
				this._highlight = value;
			}
		}

		public virtual bool Selectable
		{
			get
			{
				return this._selectable;
			}
			set
			{
				this._selectable = value;
			}
		}

		public virtual bool Selected
		{
			get
			{
				return this._selected;
			}
			set
			{
				this._selected = value;
			}
		}

		public object UserData
		{
			get
			{
				return this._userData;
			}
			set
			{
				this._userData = value;
			}
		}

		public Rectangle GetBounds()
		{
			return base.Designer.View.GetBounds(this);
		}

		public static readonly string DesignerRegionAttributeName = "_designerRegion";

		private string _displayName;

		private string _description;

		private object _userData;

		private bool _selectable;

		private bool _selected;

		private bool _highlight;

		private bool _ensureSize;
	}
}
