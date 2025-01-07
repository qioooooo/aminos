using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	internal sealed class DesignerActionBehavior : Behavior
	{
		internal DesignerActionBehavior(IServiceProvider serviceProvider, IComponent relatedComponent, DesignerActionListCollection actionLists, DesignerActionUI parentUI)
		{
			this.actionLists = actionLists;
			this.serviceProvider = serviceProvider;
			this.relatedComponent = relatedComponent;
			this.parentUI = parentUI;
		}

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

		internal DesignerActionUI ParentUI
		{
			get
			{
				return this.parentUI;
			}
		}

		internal IComponent RelatedComponent
		{
			get
			{
				return this.relatedComponent;
			}
		}

		internal void HideUI()
		{
			this.ParentUI.HideDesignerActionPanel();
		}

		internal DesignerActionPanel CreateDesignerActionPanel(IComponent relatedComponent)
		{
			DesignerActionListCollection designerActionListCollection = new DesignerActionListCollection();
			designerActionListCollection.AddRange(this.ActionLists);
			DesignerActionPanel designerActionPanel = new DesignerActionPanel(this.serviceProvider);
			designerActionPanel.UpdateTasks(designerActionListCollection, new DesignerActionListCollection(), SR.GetString("DesignerActionPanel_DefaultPanelTitle", new object[] { relatedComponent.GetType().Name }), null);
			return designerActionPanel;
		}

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

		internal bool IgnoreNextMouseUp
		{
			set
			{
				this.ignoreNextMouseUp = value;
			}
		}

		public override bool OnMouseDoubleClick(Glyph g, MouseButtons button, Point mouseLoc)
		{
			this.ignoreNextMouseUp = true;
			return true;
		}

		public override bool OnMouseDown(Glyph g, MouseButtons button, Point mouseLoc)
		{
			return !this.ParentUI.IsDesignerActionPanelVisible;
		}

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

		private IComponent relatedComponent;

		private DesignerActionUI parentUI;

		private DesignerActionListCollection actionLists;

		private IServiceProvider serviceProvider;

		private bool ignoreNextMouseUp;
	}
}
