using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	internal class ContextMenuStripActionList : DesignerActionList
	{
		public ContextMenuStripActionList(ToolStripDropDownDesigner designer)
			: base(designer.Component)
		{
			this._toolStripDropDown = (ToolStripDropDown)designer.Component;
		}

		private object GetProperty(string propertyName)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this._toolStripDropDown)[propertyName];
			if (propertyDescriptor != null)
			{
				return propertyDescriptor.GetValue(this._toolStripDropDown);
			}
			return null;
		}

		private void ChangeProperty(string propertyName, object value)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this._toolStripDropDown)[propertyName];
			if (propertyDescriptor != null)
			{
				propertyDescriptor.SetValue(this._toolStripDropDown, value);
			}
		}

		public override bool AutoShow
		{
			get
			{
				return this._autoShow;
			}
			set
			{
				if (this._autoShow != value)
				{
					this._autoShow = value;
				}
			}
		}

		public bool ShowImageMargin
		{
			get
			{
				return (bool)this.GetProperty("ShowImageMargin");
			}
			set
			{
				if (value != this.ShowImageMargin)
				{
					this.ChangeProperty("ShowImageMargin", value);
				}
			}
		}

		public bool ShowCheckMargin
		{
			get
			{
				return (bool)this.GetProperty("ShowCheckMargin");
			}
			set
			{
				if (value != this.ShowCheckMargin)
				{
					this.ChangeProperty("ShowCheckMargin", value);
				}
			}
		}

		public ToolStripRenderMode RenderMode
		{
			get
			{
				return (ToolStripRenderMode)this.GetProperty("RenderMode");
			}
			set
			{
				if (value != this.RenderMode)
				{
					this.ChangeProperty("RenderMode", value);
				}
			}
		}

		public override DesignerActionItemCollection GetSortedActionItems()
		{
			DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
			designerActionItemCollection.Add(new DesignerActionPropertyItem(SR.GetString("ToolStripActionList_RenderMode"), SR.GetString("ToolStripActionList_RenderMode"), SR.GetString("ToolStripActionList_Layout"), SR.GetString("ToolStripActionList_RenderModeDesc")));
			if (this._toolStripDropDown is ToolStripDropDownMenu)
			{
				designerActionItemCollection.Add(new DesignerActionPropertyItem(SR.GetString("ContextMenuStripActionList_ShowImageMargin"), SR.GetString("ContextMenuStripActionList_ShowImageMargin"), SR.GetString("ToolStripActionList_Layout"), SR.GetString("ContextMenuStripActionList_ShowImageMarginDesc")));
				designerActionItemCollection.Add(new DesignerActionPropertyItem(SR.GetString("ContextMenuStripActionList_ShowCheckMargin"), SR.GetString("ContextMenuStripActionList_ShowCheckMargin"), SR.GetString("ToolStripActionList_Layout"), SR.GetString("ContextMenuStripActionList_ShowCheckMarginDesc")));
			}
			return designerActionItemCollection;
		}

		private ToolStripDropDown _toolStripDropDown;

		private bool _autoShow;
	}
}
