using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002AC RID: 684
	internal class ToolStripActionList : DesignerActionList
	{
		// Token: 0x06001993 RID: 6547 RVA: 0x00089CA8 File Offset: 0x00088CA8
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

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06001994 RID: 6548 RVA: 0x00089D14 File Offset: 0x00088D14
		private bool CanAddItems
		{
			get
			{
				InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(this._toolStrip)[typeof(InheritanceAttribute)];
				return inheritanceAttribute == null || inheritanceAttribute.InheritanceLevel == InheritanceLevel.NotInherited;
			}
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06001995 RID: 6549 RVA: 0x00089D50 File Offset: 0x00088D50
		private bool IsReadOnly
		{
			get
			{
				InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(this._toolStrip)[typeof(InheritanceAttribute)];
				return inheritanceAttribute == null || inheritanceAttribute.InheritanceLevel == InheritanceLevel.InheritedReadOnly;
			}
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x00089D8C File Offset: 0x00088D8C
		private object GetProperty(string propertyName)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this._toolStrip)[propertyName];
			if (propertyDescriptor != null)
			{
				return propertyDescriptor.GetValue(this._toolStrip);
			}
			return null;
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x00089DBC File Offset: 0x00088DBC
		private void ChangeProperty(string propertyName, object value)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this._toolStrip)[propertyName];
			if (propertyDescriptor != null)
			{
				propertyDescriptor.SetValue(this._toolStrip, value);
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06001998 RID: 6552 RVA: 0x00089DEB File Offset: 0x00088DEB
		// (set) Token: 0x06001999 RID: 6553 RVA: 0x00089DF3 File Offset: 0x00088DF3
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

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x0600199A RID: 6554 RVA: 0x00089E05 File Offset: 0x00088E05
		// (set) Token: 0x0600199B RID: 6555 RVA: 0x00089E17 File Offset: 0x00088E17
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

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x0600199C RID: 6556 RVA: 0x00089E33 File Offset: 0x00088E33
		// (set) Token: 0x0600199D RID: 6557 RVA: 0x00089E45 File Offset: 0x00088E45
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

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x0600199E RID: 6558 RVA: 0x00089E61 File Offset: 0x00088E61
		// (set) Token: 0x0600199F RID: 6559 RVA: 0x00089E73 File Offset: 0x00088E73
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

		// Token: 0x060019A0 RID: 6560 RVA: 0x00089E90 File Offset: 0x00088E90
		private void InvokeEmbedVerb()
		{
			DesignerActionUIService designerActionUIService = (DesignerActionUIService)this._toolStrip.Site.GetService(typeof(DesignerActionUIService));
			if (designerActionUIService != null)
			{
				designerActionUIService.HideUI(this._toolStrip);
			}
			this.changeParentVerb.ChangeParent();
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x00089ED7 File Offset: 0x00088ED7
		private void InvokeInsertStandardItemsVerb()
		{
			this.standardItemsVerb.InsertItems();
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x00089EE4 File Offset: 0x00088EE4
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

		// Token: 0x040014A9 RID: 5289
		private ToolStrip _toolStrip;

		// Token: 0x040014AA RID: 5290
		private bool _autoShow;

		// Token: 0x040014AB RID: 5291
		private ToolStripDesigner designer;

		// Token: 0x040014AC RID: 5292
		private ChangeToolStripParentVerb changeParentVerb;

		// Token: 0x040014AD RID: 5293
		private StandardMenuStripVerb standardItemsVerb;
	}
}
