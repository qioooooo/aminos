using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x020002ED RID: 749
	internal sealed class DesignerActionBehavior : Behavior
	{
		// Token: 0x06001D03 RID: 7427 RVA: 0x000A1E7F File Offset: 0x000A0E7F
		internal DesignerActionBehavior(IServiceProvider serviceProvider, IComponent relatedComponent, DesignerActionListCollection actionLists, DesignerActionUI parentUI)
		{
			this.actionLists = actionLists;
			this.serviceProvider = serviceProvider;
			this.relatedComponent = relatedComponent;
			this.parentUI = parentUI;
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001D04 RID: 7428 RVA: 0x000A1EA4 File Offset: 0x000A0EA4
		// (set) Token: 0x06001D05 RID: 7429 RVA: 0x000A1EAC File Offset: 0x000A0EAC
		internal DesignerActionListCollection ActionLists
		{
			get
			{
				return this.actionLists;
			}
			set
			{
				this.actionLists = value;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06001D06 RID: 7430 RVA: 0x000A1EB5 File Offset: 0x000A0EB5
		internal DesignerActionUI ParentUI
		{
			get
			{
				return this.parentUI;
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06001D07 RID: 7431 RVA: 0x000A1EBD File Offset: 0x000A0EBD
		internal IComponent RelatedComponent
		{
			get
			{
				return this.relatedComponent;
			}
		}

		// Token: 0x06001D08 RID: 7432 RVA: 0x000A1EC5 File Offset: 0x000A0EC5
		internal void HideUI()
		{
			this.ParentUI.HideDesignerActionPanel();
		}

		// Token: 0x06001D09 RID: 7433 RVA: 0x000A1ED4 File Offset: 0x000A0ED4
		internal DesignerActionPanel CreateDesignerActionPanel(IComponent relatedComponent)
		{
			DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
			designerActionListCollection.AddRange(this.ActionLists);
			DesignerActionPanel designerActionPanel = new DesignerActionPanel(this.serviceProvider);
			designerActionPanel.UpdateTasks(designerActionListCollection, new DesignerActionListCollection(), SR.GetString("DesignerActionPanel_DefaultPanelTitle", new object[] { relatedComponent.GetType().Name }), null);
			return designerActionPanel;
		}

		// Token: 0x06001D0A RID: 7434 RVA: 0x000A1F30 File Offset: 0x000A0F30
		internal void ShowUI(Glyph g)
		{
			DesignerActionGlyph designerActionGlyph = g as DesignerActionGlyph;
			if (designerActionGlyph == null)
			{
				return;
			}
			DesignerActionPanel designerActionPanel = this.CreateDesignerActionPanel(this.RelatedComponent);
			this.ParentUI.ShowDesignerActionPanel(this.RelatedComponent, designerActionPanel, designerActionGlyph);
		}

		// Token: 0x1700050D RID: 1293
		// (set) Token: 0x06001D0B RID: 7435 RVA: 0x000A1F68 File Offset: 0x000A0F68
		internal bool IgnoreNextMouseUp
		{
			set
			{
				this.ignoreNextMouseUp = value;
			}
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x000A1F71 File Offset: 0x000A0F71
		public override bool OnMouseDoubleClick(Glyph g, MouseButtons button, Point mouseLoc)
		{
			this.ignoreNextMouseUp = true;
			return true;
		}

		// Token: 0x06001D0D RID: 7437 RVA: 0x000A1F7B File Offset: 0x000A0F7B
		public override bool OnMouseDown(Glyph g, MouseButtons button, Point mouseLoc)
		{
			return !this.ParentUI.IsDesignerActionPanelVisible;
		}

		// Token: 0x06001D0E RID: 7438 RVA: 0x000A1F8C File Offset: 0x000A0F8C
		public override bool OnMouseUp(Glyph g, MouseButtons button)
		{
			if (button != MouseButtons.Left || this.ParentUI == null)
			{
				return true;
			}
			bool flag = true;
			if (this.ParentUI.IsDesignerActionPanelVisible)
			{
				this.HideUI();
			}
			else if (!this.ignoreNextMouseUp)
			{
				if (this.serviceProvider != null)
				{
					ISelectionService selectionService = (ISelectionService)this.serviceProvider.GetService(typeof(ISelectionService));
					if (selectionService != null && selectionService.PrimarySelection != this.RelatedComponent)
					{
						selectionService.SetSelectedComponents(new List<IComponent> { this.RelatedComponent }, SelectionTypes.Click);
					}
				}
				this.ShowUI(g);
			}
			else
			{
				flag = false;
			}
			this.ignoreNextMouseUp = false;
			return flag;
		}

		// Token: 0x0400161A RID: 5658
		private IComponent relatedComponent;

		// Token: 0x0400161B RID: 5659
		private DesignerActionUI parentUI;

		// Token: 0x0400161C RID: 5660
		private DesignerActionListCollection actionLists;

		// Token: 0x0400161D RID: 5661
		private IServiceProvider serviceProvider;

		// Token: 0x0400161E RID: 5662
		private bool ignoreNextMouseUp;
	}
}
