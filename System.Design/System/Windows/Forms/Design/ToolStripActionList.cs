using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	internal class ToolStripActionList : DesignerActionList
	{
		public ToolStripActionList(ToolStripDesigner designer)
			: base(designer.Component)
		{
			this._toolStrip = (ToolStrip)designer.Component;
			this.designer = designer;
			this.changeParentVerb = new ChangeToolStripParentVerb(SR.GetString("ToolStripDesignerEmbedVerb"), designer);
			if (!(this._toolStrip is StatusStrip))
			{
				this.standardItemsVerb = new StandardMenuStripVerb(SR.GetString("ToolStripDesignerStandardItemsVerb"), designer);
			}
		}

		private bool CanAddItems
		{
			get
			{
				InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(this._toolStrip)[typeof(InheritanceAttribute)];
				return inheritanceAttribute == null || inheritanceAttribute.InheritanceLevel == InheritanceLevel.NotInherited;
			}
		}

		private bool IsReadOnly
		{
			get
			{
				InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(this._toolStrip)[typeof(InheritanceAttribute)];
				return inheritanceAttribute == null || inheritanceAttribute.InheritanceLevel == InheritanceLevel.InheritedReadOnly;
			}
		}

		private object GetProperty(string propertyName)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this._toolStrip)[propertyName];
			if (propertyDescriptor != null)
			{
				return propertyDescriptor.GetValue(this._toolStrip);
			}
			return null;
		}

		private void ChangeProperty(string propertyName, object value)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this._toolStrip)[propertyName];
			if (propertyDescriptor != null)
			{
				propertyDescriptor.SetValue(this._toolStrip, value);
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

		public DockStyle Dock
		{
			get
			{
				return (DockStyle)this.GetProperty("Dock");
			}
			set
			{
				if (value != this.Dock)
				{
					this.ChangeProperty("Dock", value);
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

		public ToolStripGripStyle GripStyle
		{
			get
			{
				return (ToolStripGripStyle)this.GetProperty("GripStyle");
			}
			set
			{
				if (value != this.GripStyle)
				{
					this.ChangeProperty("GripStyle", value);
				}
			}
		}

		private void InvokeEmbedVerb()
		{
			DesignerActionUIService designerActionUIService = (DesignerActionUIService)this._toolStrip.Site.GetService(typeof(DesignerActionUIService));
			if (designerActionUIService != null)
			{
				designerActionUIService.HideUI(this._toolStrip);
			}
			this.changeParentVerb.ChangeParent();
		}

		private void InvokeInsertStandardItemsVerb()
		{
			this.standardItemsVerb.InsertItems();
		}

		public override DesignerActionItemCollection GetSortedActionItems()
		{
			DesignerActionItemCollection designerActionItemCollection = new DesignerActionItemCollection();
			if (!this.IsReadOnly)
			{
				designerActionItemCollection.Add(new DesignerActionMethodItem(this, "InvokeEmbedVerb", SR.GetString("ToolStripDesignerEmbedVerb"), "", SR.GetString("ToolStripDesignerEmbedVerbDesc"), true));
			}
			if (this.CanAddItems)
			{
				if (!(this._toolStrip is StatusStrip))
				{
					designerActionItemCollection.Add(new DesignerActionMethodItem(this, "InvokeInsertStandardItemsVerb", SR.GetString("ToolStripDesignerStandardItemsVerb"), "", SR.GetString("ToolStripDesignerStandardItemsVerbDesc"), true));
				}
				designerActionItemCollection.Add(new DesignerActionPropertyItem("RenderMode", SR.GetString("ToolStripActionList_RenderMode"), SR.GetString("ToolStripActionList_Layout"), SR.GetString("ToolStripActionList_RenderModeDesc")));
			}
			if (!(this._toolStrip.Parent is ToolStripPanel))
			{
				designerActionItemCollection.Add(new DesignerActionPropertyItem("Dock", SR.GetString("ToolStripActionList_Dock"), SR.GetString("ToolStripActionList_Layout"), SR.GetString("ToolStripActionList_DockDesc")));
			}
			if (!(this._toolStrip is StatusStrip))
			{
				designerActionItemCollection.Add(new DesignerActionPropertyItem("GripStyle", SR.GetString("ToolStripActionList_GripStyle"), SR.GetString("ToolStripActionList_Layout"), SR.GetString("ToolStripActionList_GripStyleDesc")));
			}
			return designerActionItemCollection;
		}

		private ToolStrip _toolStrip;

		private bool _autoShow;

		private ToolStripDesigner designer;

		private ChangeToolStripParentVerb changeParentVerb;

		private StandardMenuStripVerb standardItemsVerb;
	}
}
