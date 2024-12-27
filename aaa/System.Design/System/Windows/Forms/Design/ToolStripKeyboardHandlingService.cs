using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002CB RID: 715
	internal class ToolStripKeyboardHandlingService
	{
		// Token: 0x06001B00 RID: 6912 RVA: 0x00093F14 File Offset: 0x00092F14
		public ToolStripKeyboardHandlingService(IServiceProvider serviceProvider)
		{
			this.provider = serviceProvider;
			this.selectionService = (ISelectionService)serviceProvider.GetService(typeof(ISelectionService));
			if (this.selectionService != null)
			{
				this.selectionService.SelectionChanging += this.OnSelectionChanging;
				this.selectionService.SelectionChanged += this.OnSelectionChanged;
			}
			this.designerHost = (IDesignerHost)this.provider.GetService(typeof(IDesignerHost));
			if (this.designerHost != null)
			{
				this.designerHost.AddService(typeof(ToolStripKeyboardHandlingService), this);
			}
			this.componentChangeSvc = (IComponentChangeService)this.designerHost.GetService(typeof(IComponentChangeService));
			if (this.componentChangeSvc != null)
			{
				this.componentChangeSvc.ComponentRemoved += this.OnComponentRemoved;
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06001B01 RID: 6913 RVA: 0x00093FFC File Offset: 0x00092FFC
		// (set) Token: 0x06001B02 RID: 6914 RVA: 0x00094004 File Offset: 0x00093004
		internal ToolStripTemplateNode ActiveTemplateNode
		{
			get
			{
				return this.activeTemplateNode;
			}
			set
			{
				this.activeTemplateNode = value;
				this.ResetActiveTemplateNodeSelectionState();
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06001B03 RID: 6915 RVA: 0x00094013 File Offset: 0x00093013
		// (set) Token: 0x06001B04 RID: 6916 RVA: 0x0009401B File Offset: 0x0009301B
		internal bool ContextMenuShownByKeyBoard
		{
			get
			{
				return this.contextMenuShownByKeyBoard;
			}
			set
			{
				this.contextMenuShownByKeyBoard = value;
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06001B05 RID: 6917 RVA: 0x00094024 File Offset: 0x00093024
		// (set) Token: 0x06001B06 RID: 6918 RVA: 0x0009402C File Offset: 0x0009302C
		internal bool CopyInProgress
		{
			get
			{
				return this.copyInProgress;
			}
			set
			{
				if (value != this.CopyInProgress)
				{
					this.copyInProgress = value;
				}
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06001B07 RID: 6919 RVA: 0x0009403E File Offset: 0x0009303E
		// (set) Token: 0x06001B08 RID: 6920 RVA: 0x00094046 File Offset: 0x00093046
		internal bool CutOrDeleteInProgress
		{
			get
			{
				return this.cutOrDeleteInProgress;
			}
			set
			{
				if (value != this.cutOrDeleteInProgress)
				{
					this.cutOrDeleteInProgress = value;
				}
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06001B09 RID: 6921 RVA: 0x00094058 File Offset: 0x00093058
		private IDesignerHost Host
		{
			get
			{
				return this.designerHost;
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06001B0A RID: 6922 RVA: 0x00094060 File Offset: 0x00093060
		private IMenuCommandService MenuService
		{
			get
			{
				if (this.menuCommandService == null && this.provider != null)
				{
					this.menuCommandService = (IMenuCommandService)this.provider.GetService(typeof(IMenuCommandService));
				}
				return this.menuCommandService;
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06001B0B RID: 6923 RVA: 0x00094098 File Offset: 0x00093098
		// (set) Token: 0x06001B0C RID: 6924 RVA: 0x000940A0 File Offset: 0x000930A0
		internal object SelectedDesignerControl
		{
			get
			{
				return this.currentSelection;
			}
			set
			{
				if (value != this.SelectedDesignerControl)
				{
					DesignerToolStripControlHost designerToolStripControlHost = this.SelectedDesignerControl as DesignerToolStripControlHost;
					if (designerToolStripControlHost != null)
					{
						designerToolStripControlHost.RefreshSelectionGlyph();
					}
					this.currentSelection = value;
					if (this.currentSelection != null)
					{
						DesignerToolStripControlHost designerToolStripControlHost2 = this.currentSelection as DesignerToolStripControlHost;
						if (designerToolStripControlHost2 != null)
						{
							designerToolStripControlHost2.SelectControl();
							ToolStripItem.ToolStripItemAccessibleObject toolStripItemAccessibleObject = designerToolStripControlHost2.AccessibilityObject as ToolStripItem.ToolStripItemAccessibleObject;
							if (toolStripItemAccessibleObject != null)
							{
								toolStripItemAccessibleObject.AddState(AccessibleStates.Selected | AccessibleStates.Focused);
								ToolStrip currentParent = designerToolStripControlHost2.GetCurrentParent();
								int num = 0;
								if (currentParent != null)
								{
									num = currentParent.Items.IndexOf(designerToolStripControlHost2);
								}
								UnsafeNativeMethods.NotifyWinEvent(32775, new HandleRef(currentParent, currentParent.Handle), -4, num + 1);
								UnsafeNativeMethods.NotifyWinEvent(32773, new HandleRef(currentParent, currentParent.Handle), -4, num + 1);
							}
						}
					}
				}
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06001B0D RID: 6925 RVA: 0x0009415B File Offset: 0x0009315B
		// (set) Token: 0x06001B0E RID: 6926 RVA: 0x00094163 File Offset: 0x00093163
		internal object OwnerItemAfterCut
		{
			get
			{
				return this.ownerItemAfterCut;
			}
			set
			{
				this.ownerItemAfterCut = value;
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06001B10 RID: 6928 RVA: 0x00094175 File Offset: 0x00093175
		// (set) Token: 0x06001B0F RID: 6927 RVA: 0x0009416C File Offset: 0x0009316C
		internal object ShiftPrimaryItem
		{
			get
			{
				return this.shiftPrimary;
			}
			set
			{
				this.shiftPrimary = value;
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06001B11 RID: 6929 RVA: 0x0009417D File Offset: 0x0009317D
		private ISelectionService SelectionService
		{
			get
			{
				return this.selectionService;
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06001B12 RID: 6930 RVA: 0x00094185 File Offset: 0x00093185
		// (set) Token: 0x06001B13 RID: 6931 RVA: 0x00094190 File Offset: 0x00093190
		internal bool TemplateNodeActive
		{
			get
			{
				return this.templateNodeActive;
			}
			set
			{
				this.templateNodeActive = value;
				if (this.newCommands != null)
				{
					foreach (object obj in this.newCommands)
					{
						MenuCommand menuCommand = (MenuCommand)obj;
						menuCommand.Enabled = !this.templateNodeActive;
					}
				}
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06001B14 RID: 6932 RVA: 0x00094200 File Offset: 0x00093200
		// (set) Token: 0x06001B15 RID: 6933 RVA: 0x00094208 File Offset: 0x00093208
		internal bool TemplateNodeContextMenuOpen
		{
			get
			{
				return this.templateNodeContextMenuOpen;
			}
			set
			{
				this.templateNodeContextMenuOpen = value;
				if (this.newCommands != null)
				{
					foreach (object obj in this.newCommands)
					{
						MenuCommand menuCommand = (MenuCommand)obj;
						menuCommand.Enabled = !this.templateNodeActive;
					}
				}
			}
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x00094278 File Offset: 0x00093278
		public void AddCommands()
		{
			IMenuCommandService menuService = this.MenuService;
			if ((menuService != null) & !this.commandsAdded)
			{
				if (this.oldCommands == null)
				{
					this.PopulateOldCommands();
				}
				foreach (object obj in this.oldCommands)
				{
					MenuCommand menuCommand = (MenuCommand)obj;
					if (menuCommand != null)
					{
						menuService.RemoveCommand(menuCommand);
					}
				}
				if (this.newCommands == null)
				{
					this.PopulateNewCommands();
				}
				foreach (object obj2 in this.newCommands)
				{
					MenuCommand menuCommand2 = (MenuCommand)obj2;
					if (menuCommand2 != null && menuService.FindCommand(menuCommand2.CommandID) == null)
					{
						menuService.AddCommand(menuCommand2);
					}
				}
				this.commandsAdded = true;
			}
		}

		// Token: 0x06001B17 RID: 6935 RVA: 0x00094378 File Offset: 0x00093378
		private ToolStripItem GetNextItem(ToolStrip parent, ToolStripItem startItem, ArrowDirection direction)
		{
			if (parent.RightToLeft == RightToLeft.Yes && (direction == ArrowDirection.Left || direction == ArrowDirection.Right))
			{
				if (direction == ArrowDirection.Right)
				{
					direction = ArrowDirection.Left;
				}
				else if (direction == ArrowDirection.Left)
				{
					direction = ArrowDirection.Right;
				}
			}
			return parent.GetNextItem(startItem, direction);
		}

		// Token: 0x06001B18 RID: 6936 RVA: 0x000943A4 File Offset: 0x000933A4
		private Control GetNextControlInTab(Control basectl, Control ctl, bool forward)
		{
			if (forward)
			{
				while (ctl != basectl)
				{
					int tabIndex = ctl.TabIndex;
					bool flag = false;
					Control control = null;
					Control parent = ctl.Parent;
					int num = 0;
					Control.ControlCollection controls = parent.Controls;
					if (controls != null)
					{
						num = controls.Count;
					}
					for (int i = 0; i < num; i++)
					{
						if (controls[i] != ctl)
						{
							if (controls[i].TabIndex >= tabIndex && (control == null || control.TabIndex > controls[i].TabIndex) && ((controls[i].Site != null && controls[i].TabIndex != tabIndex) || flag))
							{
								control = controls[i];
							}
						}
						else
						{
							flag = true;
						}
					}
					if (control != null)
					{
						return control;
					}
					ctl = ctl.Parent;
				}
			}
			else if (ctl != basectl)
			{
				int tabIndex2 = ctl.TabIndex;
				bool flag2 = false;
				Control control2 = null;
				Control parent2 = ctl.Parent;
				int num2 = 0;
				Control.ControlCollection controls2 = parent2.Controls;
				if (controls2 != null)
				{
					num2 = controls2.Count;
				}
				for (int j = num2 - 1; j >= 0; j--)
				{
					if (controls2[j] != ctl)
					{
						if (controls2[j].TabIndex <= tabIndex2 && (control2 == null || control2.TabIndex < controls2[j].TabIndex) && (controls2[j].TabIndex != tabIndex2 || flag2))
						{
							control2 = controls2[j];
						}
					}
					else
					{
						flag2 = true;
					}
				}
				if (control2 != null)
				{
					ctl = control2;
				}
				else
				{
					if (parent2 == basectl)
					{
						return null;
					}
					return parent2;
				}
			}
			if (ctl != basectl)
			{
				return ctl;
			}
			return null;
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x0009453C File Offset: 0x0009353C
		private void InvokeOldCommand(object sender)
		{
			MenuCommand menuCommand = sender as MenuCommand;
			foreach (object obj in this.oldCommands)
			{
				MenuCommand menuCommand2 = (MenuCommand)obj;
				if (menuCommand2 != null && menuCommand2.CommandID == menuCommand.CommandID)
				{
					menuCommand2.Invoke();
					break;
				}
			}
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x000945B0 File Offset: 0x000935B0
		private void OnComponentRemoved(object sender, ComponentEventArgs e)
		{
			bool flag = false;
			ComponentCollection components = this.designerHost.Container.Components;
			foreach (object obj in components)
			{
				IComponent component = (IComponent)obj;
				if (component is ToolStrip)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				ToolStripKeyboardHandlingService toolStripKeyboardHandlingService = (ToolStripKeyboardHandlingService)this.provider.GetService(typeof(ToolStripKeyboardHandlingService));
				if (toolStripKeyboardHandlingService != null)
				{
					toolStripKeyboardHandlingService.RestoreCommands();
					toolStripKeyboardHandlingService.RemoveCommands();
					this.designerHost.RemoveService(typeof(ToolStripKeyboardHandlingService));
				}
			}
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x00094668 File Offset: 0x00093668
		public bool OnContextMenu(int x, int y)
		{
			if (this.TemplateNodeActive)
			{
				return true;
			}
			if (this.commandsAdded && x == -1 && y == -1)
			{
				this.ContextMenuShownByKeyBoard = true;
				Point position = Cursor.Position;
				x = position.X;
				y = position.Y;
			}
			if (!(this.SelectionService.PrimarySelection is Component))
			{
				DesignerToolStripControlHost designerToolStripControlHost = this.SelectedDesignerControl as DesignerToolStripControlHost;
				if (designerToolStripControlHost != null)
				{
					ToolStripTemplateNode.TransparentToolStrip transparentToolStrip = designerToolStripControlHost.Control as ToolStripTemplateNode.TransparentToolStrip;
					if (transparentToolStrip != null)
					{
						ToolStripTemplateNode templateNode = transparentToolStrip.TemplateNode;
						if (templateNode != null)
						{
							templateNode.ShowContextMenu(new Point(x, y));
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x00094700 File Offset: 0x00093700
		private void OnCommandCopy(object sender, EventArgs e)
		{
			bool flag = false;
			try
			{
				MenuCommand menuCommand = sender as MenuCommand;
				if (menuCommand != null && menuCommand.CommandID == StandardCommands.Cut)
				{
					flag = true;
					this.CutOrDeleteInProgress = true;
				}
				this.InvokeOldCommand(sender);
				if (flag)
				{
					ToolStripDropDownItem toolStripDropDownItem = this.OwnerItemAfterCut as ToolStripDropDownItem;
					if (toolStripDropDownItem != null)
					{
						ToolStripDropDown dropDown = toolStripDropDownItem.DropDown;
						ToolStripDropDownDesigner toolStripDropDownDesigner = this.Host.GetDesigner(dropDown) as ToolStripDropDownDesigner;
						if (toolStripDropDownDesigner != null)
						{
							this.SelectionService.SetSelectedComponents(new object[] { toolStripDropDownDesigner.Component }, SelectionTypes.Replace);
						}
						else if (toolStripDropDownItem != null && !toolStripDropDownItem.DropDown.Visible)
						{
							ToolStripMenuItemDesigner toolStripMenuItemDesigner = this.Host.GetDesigner(toolStripDropDownItem) as ToolStripMenuItemDesigner;
							if (toolStripMenuItemDesigner != null)
							{
								toolStripMenuItemDesigner.SetSelection(true);
								DesignerToolStripControlHost designerToolStripControlHost = this.SelectedDesignerControl as DesignerToolStripControlHost;
								if (designerToolStripControlHost != null)
								{
									designerToolStripControlHost.SelectControl();
								}
							}
						}
					}
				}
				IMenuCommandService menuService = this.MenuService;
				if (menuService != null && this.newCommandPaste == null)
				{
					this.oldCommandPaste = menuService.FindCommand(StandardCommands.Paste);
					if (this.oldCommandPaste != null)
					{
						menuService.RemoveCommand(this.oldCommandPaste);
					}
					this.newCommandPaste = new MenuCommand(new EventHandler(this.OnCommandPaste), StandardCommands.Paste);
					if (this.newCommandPaste != null && menuService.FindCommand(this.newCommandPaste.CommandID) == null)
					{
						menuService.AddCommand(this.newCommandPaste);
					}
				}
			}
			finally
			{
				this.CutOrDeleteInProgress = false;
			}
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x00094880 File Offset: 0x00093880
		private void OnCommandDelete(object sender, EventArgs e)
		{
			try
			{
				this.CutOrDeleteInProgress = true;
				this.InvokeOldCommand(sender);
			}
			finally
			{
				this.CutOrDeleteInProgress = false;
			}
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x000948B8 File Offset: 0x000938B8
		private void OnCommandPaste(object sender, EventArgs e)
		{
			if (this.TemplateNodeActive)
			{
				return;
			}
			ISelectionService selectionService = this.SelectionService;
			IDesignerHost host = this.Host;
			if (selectionService != null && host != null)
			{
				IComponent component = selectionService.PrimarySelection as IComponent;
				if (component == null)
				{
					component = (IComponent)this.SelectedDesignerControl;
				}
				ToolStripItem toolStripItem = component as ToolStripItem;
				ToolStrip toolStrip = null;
				if (toolStripItem != null)
				{
					toolStrip = toolStripItem.GetCurrentParent();
				}
				if (toolStrip != null)
				{
					toolStrip.SuspendLayout();
				}
				if (this.oldCommandPaste != null)
				{
					this.oldCommandPaste.Invoke();
				}
				if (toolStrip != null)
				{
					toolStrip.ResumeLayout();
					BehaviorService behaviorService = (BehaviorService)this.provider.GetService(typeof(BehaviorService));
					if (behaviorService != null)
					{
						behaviorService.SyncSelection();
					}
					ToolStripItemDesigner toolStripItemDesigner = host.GetDesigner(toolStripItem) as ToolStripItemDesigner;
					if (toolStripItemDesigner != null)
					{
						ToolStripDropDown firstDropDown = toolStripItemDesigner.GetFirstDropDown(toolStripItem);
						if (firstDropDown != null && !firstDropDown.IsAutoGenerated)
						{
							ToolStripDropDownDesigner toolStripDropDownDesigner = host.GetDesigner(firstDropDown) as ToolStripDropDownDesigner;
							if (toolStripDropDownDesigner != null)
							{
								toolStripDropDownDesigner.AddSelectionGlyphs();
							}
						}
					}
					ToolStripDropDown toolStripDropDown = toolStrip as ToolStripDropDown;
					if (toolStripDropDown != null && toolStripDropDown.Visible)
					{
						ToolStripDropDownItem toolStripDropDownItem = toolStripDropDown.OwnerItem as ToolStripDropDownItem;
						if (toolStripDropDownItem != null)
						{
							ToolStripMenuItemDesigner toolStripMenuItemDesigner = host.GetDesigner(toolStripDropDownItem) as ToolStripMenuItemDesigner;
							if (toolStripMenuItemDesigner != null)
							{
								toolStripMenuItemDesigner.ResetGlyphs(toolStripDropDownItem);
							}
						}
					}
					ToolStripDropDownItem toolStripDropDownItem2 = selectionService.PrimarySelection as ToolStripDropDownItem;
					if (toolStripDropDownItem2 != null && toolStripDropDownItem2.DropDown.Visible)
					{
						toolStripDropDownItem2.HideDropDown();
						ToolStripMenuItemDesigner toolStripMenuItemDesigner2 = host.GetDesigner(toolStripDropDownItem2) as ToolStripMenuItemDesigner;
						if (toolStripMenuItemDesigner2 != null)
						{
							toolStripMenuItemDesigner2.InitializeDropDown();
							toolStripMenuItemDesigner2.InitializeBodyGlyphsForItems(false, toolStripDropDownItem2);
							toolStripMenuItemDesigner2.InitializeBodyGlyphsForItems(true, toolStripDropDownItem2);
						}
					}
				}
			}
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x00094A48 File Offset: 0x00093A48
		private void OnCommandHome(object sender, EventArgs e)
		{
			ISelectionService selectionService = this.SelectionService;
			if (selectionService != null)
			{
				ToolStripItem toolStripItem = selectionService.PrimarySelection as ToolStripItem;
				if (toolStripItem == null)
				{
					toolStripItem = this.SelectedDesignerControl as ToolStripItem;
				}
				if (toolStripItem != null)
				{
					ToolStrip currentParent = toolStripItem.GetCurrentParent();
					int count = currentParent.Items.Count;
					if (count >= 3)
					{
						bool flag = (Control.ModifierKeys & Keys.Shift) > Keys.None;
						if (flag)
						{
							int num = 0;
							int num2 = Math.Max(0, currentParent.Items.IndexOf(toolStripItem));
							int num3 = num2 - num + 1;
							object[] array = new object[num3];
							int num4 = 0;
							for (int i = num; i <= num2; i++)
							{
								array[num4++] = currentParent.Items[i];
							}
							selectionService.SetSelectedComponents(array, SelectionTypes.Replace);
							return;
						}
						this.SetSelection(currentParent.Items[0]);
					}
				}
			}
		}

		// Token: 0x06001B20 RID: 6944 RVA: 0x00094B24 File Offset: 0x00093B24
		private void OnCommandEnd(object sender, EventArgs e)
		{
			ISelectionService selectionService = this.SelectionService;
			if (selectionService != null)
			{
				ToolStripItem toolStripItem = selectionService.PrimarySelection as ToolStripItem;
				if (toolStripItem == null)
				{
					toolStripItem = this.SelectedDesignerControl as ToolStripItem;
				}
				if (toolStripItem != null)
				{
					ToolStrip currentParent = toolStripItem.GetCurrentParent();
					int count = currentParent.Items.Count;
					if (count >= 3)
					{
						bool flag = (Control.ModifierKeys & Keys.Shift) > Keys.None;
						if (flag)
						{
							int num = currentParent.Items.IndexOf(toolStripItem);
							int num2 = Math.Max(num, count - 2);
							int num3 = num2 - num + 1;
							object[] array = new object[num3];
							int num4 = 0;
							for (int i = num; i <= num2; i++)
							{
								array[num4++] = currentParent.Items[i];
							}
							selectionService.SetSelectedComponents(array, SelectionTypes.Replace);
							return;
						}
						this.SetSelection(currentParent.Items[count - 2]);
					}
				}
			}
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x00094C04 File Offset: 0x00093C04
		private void OnCommandSelectAll(object sender, EventArgs e)
		{
			ISelectionService selectionService = this.SelectionService;
			if (selectionService != null)
			{
				object primarySelection = selectionService.PrimarySelection;
				if (primarySelection is ToolStripItem)
				{
					ToolStripItem toolStripItem = primarySelection as ToolStripItem;
					ToolStrip toolStrip = toolStripItem.GetCurrentParent();
					if (toolStrip is ToolStripOverflow)
					{
						toolStrip = toolStripItem.Owner;
					}
					this.SelectItems(toolStrip);
					BehaviorService behaviorService = (BehaviorService)this.provider.GetService(typeof(BehaviorService));
					if (behaviorService != null)
					{
						behaviorService.Invalidate();
					}
					return;
				}
				if (primarySelection is ToolStrip)
				{
					ToolStrip toolStrip2 = primarySelection as ToolStrip;
					this.SelectItems(toolStrip2);
					return;
				}
				if (primarySelection is ToolStripPanel)
				{
					ToolStripPanel toolStripPanel = primarySelection as ToolStripPanel;
					selectionService.SetSelectedComponents(toolStripPanel.Controls, SelectionTypes.Replace);
				}
			}
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x00094CB0 File Offset: 0x00093CB0
		private void OnKeyShowDesignerActions(object sender, EventArgs e)
		{
			ISelectionService selectionService = this.SelectionService;
			if (selectionService != null && selectionService.PrimarySelection == null)
			{
				DesignerToolStripControlHost designerToolStripControlHost = this.SelectedDesignerControl as DesignerToolStripControlHost;
				if (designerToolStripControlHost != null)
				{
					ToolStripTemplateNode.TransparentToolStrip transparentToolStrip = designerToolStripControlHost.Control as ToolStripTemplateNode.TransparentToolStrip;
					if (transparentToolStrip != null)
					{
						ToolStripTemplateNode templateNode = transparentToolStrip.TemplateNode;
						if (templateNode != null)
						{
							templateNode.ShowDropDownMenu();
							return;
						}
					}
				}
			}
			this.InvokeOldCommand(sender);
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x00094D08 File Offset: 0x00093D08
		private void OnKeyDefault(object sender, EventArgs e)
		{
			if (this.templateNodeContextMenuOpen)
			{
				this.templateNodeContextMenuOpen = false;
				return;
			}
			ISelectionService selectionService = this.SelectionService;
			IDesignerHost host = this.Host;
			if (selectionService != null)
			{
				IComponent component = selectionService.PrimarySelection as IComponent;
				if (component == null)
				{
					DesignerToolStripControlHost designerToolStripControlHost = this.SelectedDesignerControl as DesignerToolStripControlHost;
					if (designerToolStripControlHost != null && host != null)
					{
						if (designerToolStripControlHost.IsOnDropDown && !designerToolStripControlHost.IsOnOverflow)
						{
							ToolStripDropDownItem toolStripDropDownItem = (ToolStripDropDownItem)((ToolStripDropDown)designerToolStripControlHost.Owner).OwnerItem;
							ToolStripMenuItemDesigner toolStripMenuItemDesigner = host.GetDesigner(toolStripDropDownItem) as ToolStripMenuItemDesigner;
							if (toolStripMenuItemDesigner != null && !toolStripMenuItemDesigner.IsEditorActive)
							{
								toolStripMenuItemDesigner.EditTemplateNode(true);
								if (this.ActiveTemplateNode != null)
								{
									this.ActiveTemplateNode.ignoreFirstKeyUp = true;
									return;
								}
							}
						}
						else
						{
							ToolStripDesigner toolStripDesigner = host.GetDesigner(designerToolStripControlHost.Owner) as ToolStripDesigner;
							if (toolStripDesigner != null)
							{
								toolStripDesigner.ShowEditNode(true);
								if (this.ActiveTemplateNode != null)
								{
									this.ActiveTemplateNode.ignoreFirstKeyUp = true;
									return;
								}
							}
						}
					}
				}
				else if (host != null)
				{
					IDesigner designer = host.GetDesigner(component);
					ToolStripMenuItemDesigner toolStripMenuItemDesigner2 = designer as ToolStripMenuItemDesigner;
					if (toolStripMenuItemDesigner2 != null)
					{
						if (toolStripMenuItemDesigner2.IsEditorActive)
						{
							return;
						}
						toolStripMenuItemDesigner2.ShowEditNode(false);
						if (this.ActiveTemplateNode != null)
						{
							this.ActiveTemplateNode.ignoreFirstKeyUp = true;
							return;
						}
					}
					else if (designer != null)
					{
						this.InvokeOldCommand(sender);
					}
				}
			}
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x00094E4C File Offset: 0x00093E4C
		private void OnKeyEdit(object sender, EventArgs e)
		{
			ISelectionService selectionService = this.SelectionService;
			IDesignerHost host = this.Host;
			if (selectionService != null)
			{
				IComponent component = selectionService.PrimarySelection as IComponent;
				if (component == null)
				{
					component = (IComponent)this.SelectedDesignerControl;
				}
				ToolStripItem toolStripItem = component as ToolStripItem;
				if (toolStripItem != null && host != null)
				{
					CommandID commandID = ((MenuCommand)sender).CommandID;
					if (commandID.Equals(MenuCommands.EditLabel))
					{
						if (component is ToolStripMenuItem)
						{
							ToolStripMenuItemDesigner toolStripMenuItemDesigner = host.GetDesigner(component) as ToolStripMenuItemDesigner;
							if (toolStripMenuItemDesigner != null && !toolStripMenuItemDesigner.IsEditorActive)
							{
								toolStripMenuItemDesigner.ShowEditNode(false);
							}
						}
						if (component is DesignerToolStripControlHost)
						{
							DesignerToolStripControlHost designerToolStripControlHost = component as DesignerToolStripControlHost;
							if (designerToolStripControlHost.IsOnDropDown)
							{
								ToolStripDropDownItem toolStripDropDownItem = (ToolStripDropDownItem)((ToolStripDropDown)designerToolStripControlHost.Owner).OwnerItem;
								ToolStripMenuItemDesigner toolStripMenuItemDesigner2 = host.GetDesigner(toolStripDropDownItem) as ToolStripMenuItemDesigner;
								if (toolStripMenuItemDesigner2 != null && !toolStripMenuItemDesigner2.IsEditorActive)
								{
									toolStripMenuItemDesigner2.EditTemplateNode(false);
									return;
								}
							}
							else
							{
								ToolStripDesigner toolStripDesigner = host.GetDesigner(designerToolStripControlHost.Owner) as ToolStripDesigner;
								if (toolStripDesigner != null)
								{
									toolStripDesigner.ShowEditNode(false);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x00094F5C File Offset: 0x00093F5C
		private void OnKeyMove(object sender, EventArgs e)
		{
			ISelectionService selectionService = this.SelectionService;
			if (selectionService != null)
			{
				MenuCommand menuCommand = (MenuCommand)sender;
				if (menuCommand.CommandID.Equals(MenuCommands.KeySizeWidthIncrease) || menuCommand.CommandID.Equals(MenuCommands.KeySizeWidthDecrease) || menuCommand.CommandID.Equals(MenuCommands.KeySizeHeightDecrease) || menuCommand.CommandID.Equals(MenuCommands.KeySizeHeightIncrease))
				{
					this.shiftPressed = true;
				}
				else
				{
					this.shiftPressed = false;
				}
				ContextMenuStrip contextMenuStrip = selectionService.PrimarySelection as ContextMenuStrip;
				if (contextMenuStrip != null)
				{
					if (menuCommand.CommandID.Equals(MenuCommands.KeyMoveDown))
					{
						this.ProcessUpDown(true);
					}
					return;
				}
				ToolStripItem toolStripItem = selectionService.PrimarySelection as ToolStripItem;
				if (toolStripItem == null)
				{
					toolStripItem = this.SelectedDesignerControl as ToolStripItem;
				}
				if (toolStripItem != null)
				{
					if ((menuCommand.CommandID.Equals(MenuCommands.KeyMoveRight) || menuCommand.CommandID.Equals(MenuCommands.KeyNudgeRight) || menuCommand.CommandID.Equals(MenuCommands.KeySizeWidthIncrease)) && !this.ProcessRightLeft(true))
					{
						this.RotateTab(false);
						return;
					}
					if ((menuCommand.CommandID.Equals(MenuCommands.KeyMoveLeft) || menuCommand.CommandID.Equals(MenuCommands.KeyNudgeLeft) || menuCommand.CommandID.Equals(MenuCommands.KeySizeWidthDecrease)) && !this.ProcessRightLeft(false))
					{
						this.RotateTab(true);
						return;
					}
					if (menuCommand.CommandID.Equals(MenuCommands.KeyMoveDown) || menuCommand.CommandID.Equals(MenuCommands.KeyNudgeDown) || menuCommand.CommandID.Equals(MenuCommands.KeySizeHeightIncrease))
					{
						this.ProcessUpDown(true);
						return;
					}
					if (menuCommand.CommandID.Equals(MenuCommands.KeyMoveUp) || menuCommand.CommandID.Equals(MenuCommands.KeyNudgeUp) || menuCommand.CommandID.Equals(MenuCommands.KeySizeHeightDecrease))
					{
						this.ProcessUpDown(false);
						return;
					}
				}
				else
				{
					this.InvokeOldCommand(sender);
				}
			}
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x00095130 File Offset: 0x00094130
		private void OnKeyCancel(object sender, EventArgs e)
		{
			ISelectionService selectionService = this.SelectionService;
			if (selectionService != null)
			{
				ToolStripItem toolStripItem = selectionService.PrimarySelection as ToolStripItem;
				if (toolStripItem == null)
				{
					toolStripItem = this.SelectedDesignerControl as ToolStripItem;
				}
				if (toolStripItem != null)
				{
					MenuCommand menuCommand = (MenuCommand)sender;
					bool flag = menuCommand.CommandID.Equals(MenuCommands.KeyReverseCancel);
					this.RotateParent(flag);
					return;
				}
				ToolStripDropDown toolStripDropDown = selectionService.PrimarySelection as ToolStripDropDown;
				if (toolStripDropDown != null && toolStripDropDown.Site != null)
				{
					selectionService.SetSelectedComponents(new object[] { this.Host.RootComponent }, SelectionTypes.Replace);
					return;
				}
				this.InvokeOldCommand(sender);
			}
		}

		// Token: 0x06001B27 RID: 6951 RVA: 0x000951CC File Offset: 0x000941CC
		private void OnKeySelect(object sender, EventArgs e)
		{
			MenuCommand menuCommand = (MenuCommand)sender;
			bool flag = menuCommand.CommandID.Equals(MenuCommands.KeySelectPrevious);
			this.ProcessKeySelect(flag, menuCommand);
		}

		// Token: 0x06001B28 RID: 6952 RVA: 0x000951FC File Offset: 0x000941FC
		private void OnSelectionChanging(object sender, EventArgs e)
		{
			Component component = this.SelectionService.PrimarySelection as Component;
			if (component == null)
			{
				component = this.SelectedDesignerControl as ToolStripItem;
			}
			ToolStrip toolStrip = component as ToolStrip;
			if (toolStrip != null)
			{
				InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(toolStrip)[typeof(InheritanceAttribute)];
				if (inheritanceAttribute != null && (inheritanceAttribute.InheritanceLevel == InheritanceLevel.Inherited || inheritanceAttribute.InheritanceLevel == InheritanceLevel.InheritedReadOnly))
				{
					return;
				}
			}
			if (toolStrip == null && !(component is ToolStripItem))
			{
				this.RestoreCommands();
				this.SelectedDesignerControl = null;
			}
		}

		// Token: 0x06001B29 RID: 6953 RVA: 0x0009527C File Offset: 0x0009427C
		private void OnSelectionChanged(object sender, EventArgs e)
		{
			Component component = this.SelectionService.PrimarySelection as Component;
			if (component == null)
			{
				component = this.SelectedDesignerControl as ToolStripItem;
			}
			ToolStrip toolStrip = component as ToolStrip;
			if (toolStrip != null)
			{
				InheritanceAttribute inheritanceAttribute = (InheritanceAttribute)TypeDescriptor.GetAttributes(toolStrip)[typeof(InheritanceAttribute)];
				if (inheritanceAttribute != null && (inheritanceAttribute.InheritanceLevel == InheritanceLevel.Inherited || inheritanceAttribute.InheritanceLevel == InheritanceLevel.InheritedReadOnly))
				{
					return;
				}
			}
			if (toolStrip != null || component is ToolStripItem)
			{
				BehaviorService behaviorService = (BehaviorService)this.provider.GetService(typeof(BehaviorService));
				if (behaviorService != null)
				{
					DesignerActionUI designerActionUI = behaviorService.DesignerActionUI;
					if (designerActionUI != null)
					{
						designerActionUI.HideDesignerActionPanel();
					}
				}
				this.AddCommands();
			}
		}

		// Token: 0x06001B2A RID: 6954 RVA: 0x00095328 File Offset: 0x00094328
		public void ProcessKeySelect(bool reverse, MenuCommand cmd)
		{
			ISelectionService selectionService = this.SelectionService;
			if (selectionService != null)
			{
				ToolStripItem toolStripItem = selectionService.PrimarySelection as ToolStripItem;
				if (toolStripItem == null)
				{
					toolStripItem = this.SelectedDesignerControl as ToolStripItem;
				}
				if (toolStripItem != null)
				{
					if (!this.ProcessRightLeft(!reverse))
					{
						this.RotateTab(reverse);
					}
					return;
				}
				if (toolStripItem == null && selectionService.PrimarySelection is ToolStrip)
				{
					this.RotateTab(reverse);
				}
			}
		}

		// Token: 0x06001B2B RID: 6955 RVA: 0x00095388 File Offset: 0x00094388
		private bool ProcessRightLeft(bool right)
		{
			object obj = null;
			ISelectionService selectionService = this.SelectionService;
			IDesignerHost host = this.Host;
			if (selectionService == null || host == null || !(host.RootComponent is Control))
			{
				return false;
			}
			object obj2 = selectionService.PrimarySelection;
			if (this.shiftPressed && this.ShiftPrimaryItem != null)
			{
				obj2 = this.ShiftPrimaryItem;
			}
			if (obj2 == null)
			{
				obj2 = this.SelectedDesignerControl;
			}
			Control control = obj2 as Control;
			if (obj == null && control == null)
			{
				ToolStripItem toolStripItem = selectionService.PrimarySelection as ToolStripItem;
				if (this.shiftPressed && this.ShiftPrimaryItem != null)
				{
					toolStripItem = this.ShiftPrimaryItem as ToolStripItem;
				}
				if (toolStripItem == null)
				{
					toolStripItem = this.SelectedDesignerControl as ToolStripItem;
				}
				ToolStripDropDown toolStripDropDown = toolStripItem.GetCurrentParent() as ToolStripDropDown;
				if (toolStripItem is DesignerToolStripControlHost && toolStripDropDown != null)
				{
					if (toolStripDropDown != null && !right)
					{
						if (toolStripDropDown is ToolStripOverflow)
						{
							obj = this.GetNextItem(toolStripDropDown, toolStripItem, ArrowDirection.Left);
						}
						else
						{
							obj = toolStripDropDown.OwnerItem;
						}
					}
					if (obj != null)
					{
						this.SetSelection(obj);
						return true;
					}
				}
				else
				{
					ToolStripItem toolStripItem2 = selectionService.PrimarySelection as ToolStripItem;
					if (this.shiftPressed && this.ShiftPrimaryItem != null)
					{
						toolStripItem2 = this.ShiftPrimaryItem as ToolStripDropDownItem;
					}
					if (toolStripItem2 == null)
					{
						toolStripItem2 = this.SelectedDesignerControl as ToolStripDropDownItem;
					}
					if (toolStripItem2 != null && toolStripItem2.IsOnDropDown)
					{
						bool rightAlignedMenus = SystemInformation.RightAlignedMenus;
						if ((rightAlignedMenus && right) || (!rightAlignedMenus && right))
						{
							ToolStripDropDownItem toolStripDropDownItem = toolStripItem2 as ToolStripDropDownItem;
							if (toolStripDropDownItem != null)
							{
								obj = this.GetNextItem(toolStripDropDownItem.DropDown, null, ArrowDirection.Right);
								if (obj != null)
								{
									this.SetSelection(obj);
									if (!toolStripDropDownItem.DropDown.Visible)
									{
										ToolStripMenuItemDesigner toolStripMenuItemDesigner = host.GetDesigner(toolStripDropDownItem) as ToolStripMenuItemDesigner;
										if (toolStripMenuItemDesigner != null)
										{
											toolStripMenuItemDesigner.InitializeDropDown();
										}
									}
									return true;
								}
							}
						}
						if (!right && !rightAlignedMenus)
						{
							ToolStripItem ownerItem = ((ToolStripDropDown)toolStripItem2.Owner).OwnerItem;
							if (!ownerItem.IsOnDropDown)
							{
								ToolStrip currentParent = ownerItem.GetCurrentParent();
								obj = this.GetNextItem(currentParent, ownerItem, ArrowDirection.Left);
							}
							else
							{
								obj = ownerItem;
							}
							if (obj != null)
							{
								this.SetSelection(obj);
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06001B2C RID: 6956 RVA: 0x00095584 File Offset: 0x00094584
		public void ProcessUpDown(bool down)
		{
			object obj = null;
			ISelectionService selectionService = this.SelectionService;
			IDesignerHost host = this.Host;
			if (selectionService == null || host == null || !(host.RootComponent is Control))
			{
				return;
			}
			object obj2 = selectionService.PrimarySelection;
			if (this.shiftPressed && this.ShiftPrimaryItem != null)
			{
				obj2 = this.ShiftPrimaryItem;
			}
			ContextMenuStrip contextMenuStrip = obj2 as ContextMenuStrip;
			if (contextMenuStrip != null)
			{
				if (down)
				{
					obj = this.GetNextItem(contextMenuStrip, null, ArrowDirection.Down);
					this.SetSelection(obj);
				}
				return;
			}
			if (obj2 == null)
			{
				obj2 = this.SelectedDesignerControl;
			}
			Control control = obj2 as Control;
			if (obj == null && control == null)
			{
				ToolStripItem toolStripItem = selectionService.PrimarySelection as ToolStripItem;
				if (this.shiftPressed && this.ShiftPrimaryItem != null)
				{
					toolStripItem = this.ShiftPrimaryItem as ToolStripItem;
				}
				if (toolStripItem == null)
				{
					toolStripItem = this.SelectedDesignerControl as ToolStripItem;
				}
				ToolStripDropDown toolStripDropDown = null;
				if (toolStripItem != null)
				{
					if (toolStripItem is DesignerToolStripControlHost)
					{
						if (down)
						{
							DesignerToolStripControlHost designerToolStripControlHost = this.SelectedDesignerControl as DesignerToolStripControlHost;
							if (designerToolStripControlHost != null)
							{
								ToolStripTemplateNode.TransparentToolStrip transparentToolStrip = designerToolStripControlHost.Control as ToolStripTemplateNode.TransparentToolStrip;
								if (transparentToolStrip != null)
								{
									ToolStripTemplateNode templateNode = transparentToolStrip.TemplateNode;
									if (templateNode != null)
									{
										if (!(toolStripItem.Owner is MenuStrip) && !(toolStripItem.Owner is ToolStripDropDown))
										{
											templateNode.ShowDropDownMenu();
											return;
										}
										toolStripDropDown = toolStripItem.GetCurrentParent() as ToolStripDropDown;
									}
								}
							}
						}
						else
						{
							toolStripDropDown = toolStripItem.GetCurrentParent() as ToolStripDropDown;
						}
					}
					else
					{
						ToolStripDropDownItem toolStripDropDownItem = toolStripItem as ToolStripDropDownItem;
						if (toolStripDropDownItem != null && !toolStripDropDownItem.IsOnDropDown)
						{
							toolStripDropDown = toolStripDropDownItem.DropDown;
							toolStripItem = null;
						}
						else if (toolStripDropDownItem != null)
						{
							toolStripDropDown = ((toolStripDropDownItem.Placement == ToolStripItemPlacement.Overflow) ? toolStripDropDownItem.Owner.OverflowButton.DropDown : toolStripDropDownItem.Owner) as ToolStripDropDown;
							toolStripItem = toolStripDropDownItem;
						}
						if (toolStripDropDownItem == null)
						{
							toolStripDropDown = toolStripItem.GetCurrentParent() as ToolStripDropDown;
						}
					}
					if (toolStripDropDown != null)
					{
						if (down)
						{
							obj = this.GetNextItem(toolStripDropDown, toolStripItem, ArrowDirection.Down);
							if (toolStripDropDown.OwnerItem != null && !toolStripDropDown.OwnerItem.IsOnDropDown && toolStripDropDown.OwnerItem.Owner != null && toolStripDropDown.OwnerItem.Owner.Site != null)
							{
								ToolStripItem toolStripItem2 = obj as ToolStripItem;
								if (toolStripItem2 != null && toolStripDropDown.Items.IndexOf(toolStripItem2) != -1 && toolStripDropDown.Items.IndexOf(toolStripItem2) <= toolStripDropDown.Items.IndexOf(toolStripItem))
								{
									obj = toolStripDropDown.OwnerItem;
								}
							}
							if (this.shiftPressed && this.SelectionService.GetComponentSelected(obj))
							{
								this.SelectionService.SetSelectedComponents(new object[] { this.ShiftPrimaryItem, obj }, SelectionTypes.Remove);
							}
						}
						else
						{
							if (toolStripDropDown is ToolStripOverflow)
							{
								ToolStripItem nextItem = this.GetNextItem(toolStripDropDown, null, ArrowDirection.Down);
								if (toolStripItem == nextItem)
								{
									ToolStrip owner = toolStripItem.Owner;
									if (owner != null)
									{
										obj = this.GetNextItem(owner, toolStripDropDown.OwnerItem, ArrowDirection.Left);
									}
								}
								else
								{
									obj = this.GetNextItem(toolStripDropDown, toolStripItem, ArrowDirection.Up);
								}
							}
							else
							{
								obj = this.GetNextItem(toolStripDropDown, toolStripItem, ArrowDirection.Up);
							}
							if (toolStripDropDown.OwnerItem != null && !toolStripDropDown.OwnerItem.IsOnDropDown && toolStripDropDown.OwnerItem.Owner != null && toolStripDropDown.OwnerItem.Owner.Site != null)
							{
								ToolStripItem toolStripItem3 = obj as ToolStripItem;
								if (toolStripItem3 != null && toolStripItem != null && toolStripDropDown.Items.IndexOf(toolStripItem3) != -1 && toolStripDropDown.Items.IndexOf(toolStripItem3) >= toolStripDropDown.Items.IndexOf(toolStripItem))
								{
									obj = toolStripDropDown.OwnerItem;
								}
							}
							if (this.shiftPressed && this.SelectionService.GetComponentSelected(obj))
							{
								this.SelectionService.SetSelectedComponents(new object[] { this.ShiftPrimaryItem, obj }, SelectionTypes.Remove);
							}
						}
						if (obj != null && obj != toolStripItem)
						{
							this.SetSelection(obj);
						}
					}
				}
			}
		}

		// Token: 0x06001B2D RID: 6957 RVA: 0x00095958 File Offset: 0x00094958
		private void PopulateOldCommands()
		{
			if (this.oldCommands == null)
			{
				this.oldCommands = new ArrayList();
			}
			IMenuCommandService menuService = this.MenuService;
			if (menuService != null)
			{
				this.oldCommands.Add(menuService.FindCommand(MenuCommands.KeySelectNext));
				this.oldCommands.Add(menuService.FindCommand(MenuCommands.KeySelectPrevious));
				this.oldCommands.Add(menuService.FindCommand(MenuCommands.KeyDefaultAction));
				this.oldCommands.Add(menuService.FindCommand(MenuCommands.KeyMoveUp));
				this.oldCommands.Add(menuService.FindCommand(MenuCommands.KeyMoveDown));
				this.oldCommands.Add(menuService.FindCommand(MenuCommands.KeyMoveLeft));
				this.oldCommands.Add(menuService.FindCommand(MenuCommands.KeyMoveRight));
				this.oldCommands.Add(menuService.FindCommand(MenuCommands.KeyNudgeUp));
				this.oldCommands.Add(menuService.FindCommand(MenuCommands.KeyNudgeDown));
				this.oldCommands.Add(menuService.FindCommand(MenuCommands.KeyNudgeLeft));
				this.oldCommands.Add(menuService.FindCommand(MenuCommands.KeyNudgeRight));
				this.oldCommands.Add(menuService.FindCommand(MenuCommands.KeySizeWidthIncrease));
				this.oldCommands.Add(menuService.FindCommand(MenuCommands.KeySizeHeightIncrease));
				this.oldCommands.Add(menuService.FindCommand(MenuCommands.KeySizeWidthDecrease));
				this.oldCommands.Add(menuService.FindCommand(MenuCommands.KeySizeHeightDecrease));
				this.oldCommands.Add(menuService.FindCommand(MenuCommands.KeyCancel));
				this.oldCommands.Add(menuService.FindCommand(MenuCommands.KeyReverseCancel));
				this.oldCommands.Add(menuService.FindCommand(StandardCommands.Copy));
				this.oldCommands.Add(menuService.FindCommand(StandardCommands.SelectAll));
				this.oldCommands.Add(menuService.FindCommand(MenuCommands.KeyInvokeSmartTag));
				this.oldCommands.Add(menuService.FindCommand(StandardCommands.Cut));
				this.oldCommands.Add(menuService.FindCommand(StandardCommands.Delete));
			}
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x00095B80 File Offset: 0x00094B80
		private void PopulateNewCommands()
		{
			if (this.newCommands == null)
			{
				this.newCommands = new ArrayList();
			}
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnKeySelect), MenuCommands.KeySelectNext));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnKeySelect), MenuCommands.KeySelectPrevious));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnKeyDefault), MenuCommands.KeyDefaultAction));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnKeyEdit), MenuCommands.EditLabel));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnKeyMove), MenuCommands.KeyMoveUp));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnKeyMove), MenuCommands.KeyMoveDown));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnKeyMove), MenuCommands.KeyMoveLeft));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnKeyMove), MenuCommands.KeyMoveRight));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnKeyMove), MenuCommands.KeyNudgeUp));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnKeyMove), MenuCommands.KeyNudgeDown));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnKeyMove), MenuCommands.KeyNudgeLeft));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnKeyMove), MenuCommands.KeyNudgeRight));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnKeyMove), MenuCommands.KeySizeWidthIncrease));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnKeyMove), MenuCommands.KeySizeHeightIncrease));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnKeyMove), MenuCommands.KeySizeWidthDecrease));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnKeyMove), MenuCommands.KeySizeHeightDecrease));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnKeyCancel), MenuCommands.KeyCancel));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnKeyCancel), MenuCommands.KeyReverseCancel));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnCommandCopy), StandardCommands.Copy));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnCommandSelectAll), StandardCommands.SelectAll));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnCommandHome), MenuCommands.KeyHome));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnCommandEnd), MenuCommands.KeyEnd));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnCommandHome), MenuCommands.KeyShiftHome));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnCommandEnd), MenuCommands.KeyShiftEnd));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnKeyShowDesignerActions), MenuCommands.KeyInvokeSmartTag));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnCommandCopy), StandardCommands.Cut));
			this.newCommands.Add(new MenuCommand(new EventHandler(this.OnCommandDelete), StandardCommands.Delete));
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x00095F38 File Offset: 0x00094F38
		public void RestoreCommands()
		{
			IMenuCommandService menuService = this.MenuService;
			if ((menuService != null) & this.commandsAdded)
			{
				if (this.newCommands != null)
				{
					foreach (object obj in this.newCommands)
					{
						MenuCommand menuCommand = (MenuCommand)obj;
						menuService.RemoveCommand(menuCommand);
					}
				}
				if (this.oldCommands != null)
				{
					foreach (object obj2 in this.oldCommands)
					{
						MenuCommand menuCommand2 = (MenuCommand)obj2;
						if (menuCommand2 != null && menuService.FindCommand(menuCommand2.CommandID) == null)
						{
							menuService.AddCommand(menuCommand2);
						}
					}
				}
				if (this.newCommandPaste != null)
				{
					menuService.RemoveCommand(this.newCommandPaste);
					this.newCommandPaste = null;
				}
				if (this.oldCommandPaste != null && menuService.FindCommand(this.oldCommandPaste.CommandID) == null)
				{
					menuService.AddCommand(this.oldCommandPaste);
					this.oldCommandPaste = null;
				}
				this.commandsAdded = false;
			}
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x00096070 File Offset: 0x00095070
		internal void ResetActiveTemplateNodeSelectionState()
		{
			if (this.SelectedDesignerControl != null)
			{
				DesignerToolStripControlHost designerToolStripControlHost = this.SelectedDesignerControl as DesignerToolStripControlHost;
				if (designerToolStripControlHost != null)
				{
					designerToolStripControlHost.RefreshSelectionGlyph();
				}
			}
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x0009609C File Offset: 0x0009509C
		public void RemoveCommands()
		{
			IMenuCommandService menuService = this.MenuService;
			if (menuService != null && this.commandsAdded && this.newCommands != null)
			{
				foreach (object obj in this.newCommands)
				{
					MenuCommand menuCommand = (MenuCommand)obj;
					menuService.RemoveCommand(menuCommand);
				}
			}
			if (this.newCommandPaste != null)
			{
				menuService.RemoveCommand(this.newCommandPaste);
				this.newCommandPaste = null;
			}
			if (this.oldCommandPaste != null)
			{
				this.oldCommandPaste = null;
			}
			if (this.newCommands != null)
			{
				this.newCommands.Clear();
				this.newCommands = null;
			}
			if (this.oldCommands != null)
			{
				this.oldCommands.Clear();
				this.oldCommands = null;
			}
			if (this.selectionService != null)
			{
				this.selectionService.SelectionChanging -= this.OnSelectionChanging;
				this.selectionService.SelectionChanged -= this.OnSelectionChanged;
				this.selectionService = null;
			}
			if (this.componentChangeSvc != null)
			{
				this.componentChangeSvc.ComponentRemoved -= this.OnComponentRemoved;
				this.componentChangeSvc = null;
			}
			this.currentSelection = null;
			this.shiftPrimary = null;
			this.provider = null;
			this.menuCommandService = null;
			this.activeTemplateNode = null;
		}

		// Token: 0x06001B32 RID: 6962 RVA: 0x000961F4 File Offset: 0x000951F4
		private void RotateParent(bool backwards)
		{
			Control control = null;
			object obj = null;
			ToolStripItem toolStripItem = null;
			ISelectionService selectionService = this.SelectionService;
			IDesignerHost host = this.Host;
			if (selectionService == null || host == null || !(host.RootComponent is Control))
			{
				return;
			}
			IContainer container = host.Container;
			Control control2 = selectionService.PrimarySelection as Control;
			if (control2 == null)
			{
				control2 = this.SelectedDesignerControl as Control;
			}
			if (control2 != null)
			{
				control = control2;
			}
			else
			{
				toolStripItem = selectionService.PrimarySelection as ToolStripItem;
				if (toolStripItem == null)
				{
					toolStripItem = this.SelectedDesignerControl as ToolStripItem;
				}
				if (toolStripItem == null)
				{
					control = (Control)host.RootComponent;
				}
			}
			if (backwards)
			{
				if (control != null)
				{
					if (control.Controls.Count > 0)
					{
						obj = control.Controls[0];
					}
					else
					{
						obj = control;
					}
				}
				else if (toolStripItem != null)
				{
					obj = toolStripItem.Owner.Controls[0];
				}
			}
			else if (control != null)
			{
				obj = control.Parent;
				Control control3 = obj as Control;
				if (control3 == null || control3.Site == null || control3.Site.Container != container)
				{
					obj = control;
				}
			}
			else if (toolStripItem != null)
			{
				if (toolStripItem.IsOnDropDown && toolStripItem.Placement != ToolStripItemPlacement.Overflow)
				{
					obj = ((ToolStripDropDown)toolStripItem.Owner).OwnerItem;
				}
				else if (toolStripItem.IsOnDropDown && toolStripItem.Placement == ToolStripItemPlacement.Overflow)
				{
					ToolStrip owner = toolStripItem.Owner;
					if (owner != null)
					{
						owner.OverflowButton.HideDropDown();
					}
					obj = toolStripItem.Owner;
				}
				else
				{
					obj = toolStripItem.Owner;
				}
			}
			if (obj is DesignerToolStripControlHost)
			{
				this.SelectedDesignerControl = obj;
				selectionService.SetSelectedComponents(null, SelectionTypes.Replace);
				return;
			}
			this.SelectedDesignerControl = null;
			selectionService.SetSelectedComponents(new object[] { obj }, SelectionTypes.Replace);
		}

		// Token: 0x06001B33 RID: 6963 RVA: 0x0009639C File Offset: 0x0009539C
		public void RotateTab(bool backwards)
		{
			object obj = null;
			ISelectionService selectionService = this.SelectionService;
			IDesignerHost host = this.Host;
			if (selectionService == null || host == null || !(host.RootComponent is Control))
			{
				return;
			}
			IContainer container = host.Container;
			Control control = (Control)host.RootComponent;
			object obj2 = selectionService.PrimarySelection;
			if (this.shiftPressed && this.ShiftPrimaryItem != null)
			{
				obj2 = this.ShiftPrimaryItem;
			}
			Control control2;
			if (obj2 == null)
			{
				obj2 = this.SelectedDesignerControl;
				if (obj2 != null)
				{
					DesignerToolStripControlHost designerToolStripControlHost = obj2 as DesignerToolStripControlHost;
					if (designerToolStripControlHost != null && (!designerToolStripControlHost.IsOnDropDown || (designerToolStripControlHost.IsOnDropDown && designerToolStripControlHost.IsOnOverflow)))
					{
						control2 = designerToolStripControlHost.Owner;
						if ((control2.RightToLeft != RightToLeft.Yes && !backwards) || (control2.RightToLeft == RightToLeft.Yes && backwards))
						{
							obj = this.GetNextControlInTab(control, control2, !backwards);
							if (obj == null)
							{
								ComponentTray componentTray = (ComponentTray)this.provider.GetService(typeof(ComponentTray));
								if (componentTray != null)
								{
									obj = componentTray.GetNextComponent((IComponent)obj2, !backwards);
									if (obj != null)
									{
										ControlDesigner controlDesigner = host.GetDesigner((IComponent)obj) as ControlDesigner;
										while (controlDesigner != null)
										{
											obj = componentTray.GetNextComponent((IComponent)obj, !backwards);
											if (obj != null)
											{
												controlDesigner = host.GetDesigner((IComponent)obj) as ControlDesigner;
											}
											else
											{
												controlDesigner = null;
											}
										}
									}
								}
								if (obj == null)
								{
									obj = control;
								}
							}
						}
					}
				}
			}
			control2 = obj2 as Control;
			ToolStrip toolStrip = control2 as ToolStrip;
			if (obj == null && toolStrip != null)
			{
				ToolStripItemCollection items = toolStrip.Items;
				if (items != null)
				{
					if (!backwards)
					{
						obj = items[0];
					}
					else
					{
						obj = items[toolStrip.Items.Count - 1];
					}
				}
			}
			if (obj == null && control2 == null)
			{
				ToolStripItem toolStripItem = selectionService.PrimarySelection as ToolStripItem;
				if (this.shiftPressed && this.ShiftPrimaryItem != null)
				{
					toolStripItem = this.ShiftPrimaryItem as ToolStripItem;
				}
				if (toolStripItem == null)
				{
					toolStripItem = this.SelectedDesignerControl as ToolStripItem;
				}
				if (toolStripItem != null && toolStripItem.IsOnDropDown && toolStripItem.Placement != ToolStripItemPlacement.Overflow)
				{
					DesignerToolStripControlHost designerToolStripControlHost2 = toolStripItem as DesignerToolStripControlHost;
					if (designerToolStripControlHost2 != null)
					{
						ToolStripItem ownerItem = ((ToolStripDropDown)designerToolStripControlHost2.Owner).OwnerItem;
						ToolStripMenuItemDesigner toolStripMenuItemDesigner = host.GetDesigner(ownerItem) as ToolStripMenuItemDesigner;
						ToolStripDropDown firstDropDown = toolStripMenuItemDesigner.GetFirstDropDown((ToolStripDropDownItem)ownerItem);
						if (firstDropDown != null)
						{
							toolStripItem = firstDropDown.OwnerItem;
						}
						else
						{
							toolStripItem = ownerItem;
						}
					}
				}
				if (toolStripItem != null && !(toolStripItem is DesignerToolStripControlHost))
				{
					ToolStrip currentParent = toolStripItem.GetCurrentParent();
					if (currentParent != null)
					{
						if (backwards)
						{
							if (currentParent is ToolStripOverflow)
							{
								ToolStripItem nextItem = this.GetNextItem(currentParent, null, ArrowDirection.Down);
								if (toolStripItem == nextItem)
								{
									ToolStrip owner = toolStripItem.Owner;
									if (owner != null)
									{
										obj = this.GetNextItem(owner, ((ToolStripDropDown)currentParent).OwnerItem, ArrowDirection.Left);
									}
								}
								else
								{
									obj = this.GetNextItem(currentParent, toolStripItem, ArrowDirection.Left);
								}
							}
							else if (toolStripItem == currentParent.Items[0] && currentParent.RightToLeft != RightToLeft.Yes)
							{
								if (this.shiftPressed)
								{
									return;
								}
								obj = this.GetNextControlInTab(control, currentParent, !backwards);
								if (obj == null)
								{
									ComponentTray componentTray2 = (ComponentTray)this.provider.GetService(typeof(ComponentTray));
									if (componentTray2 != null)
									{
										obj = componentTray2.GetNextComponent((IComponent)obj2, !backwards);
										if (obj != null)
										{
											ControlDesigner controlDesigner2 = host.GetDesigner((IComponent)obj) as ControlDesigner;
											while (controlDesigner2 != null)
											{
												obj = componentTray2.GetNextComponent((IComponent)obj, !backwards);
												if (obj != null)
												{
													controlDesigner2 = host.GetDesigner((IComponent)obj) as ControlDesigner;
												}
												else
												{
													controlDesigner2 = null;
												}
											}
										}
									}
									if (obj == null)
									{
										obj = control;
									}
								}
							}
							else
							{
								obj = this.GetNextItem(currentParent, toolStripItem, ArrowDirection.Left);
								if (this.shiftPressed && this.SelectionService.GetComponentSelected(obj))
								{
									this.SelectionService.SetSelectedComponents(new object[] { this.ShiftPrimaryItem, obj }, SelectionTypes.Remove);
								}
							}
						}
						else if (currentParent is ToolStripOverflow)
						{
							obj = this.GetNextItem(currentParent, toolStripItem, ArrowDirection.Down);
						}
						else if (toolStripItem == currentParent.Items[0] && currentParent.RightToLeft == RightToLeft.Yes)
						{
							if (this.shiftPressed)
							{
								return;
							}
							obj = this.GetNextControlInTab(control, currentParent, !backwards);
							if (obj == null)
							{
								obj = control;
							}
						}
						else
						{
							obj = this.GetNextItem(currentParent, toolStripItem, ArrowDirection.Right);
							if (this.shiftPressed && this.SelectionService.GetComponentSelected(obj))
							{
								this.SelectionService.SetSelectedComponents(new object[] { this.ShiftPrimaryItem, obj }, SelectionTypes.Remove);
							}
						}
					}
				}
				else if (toolStripItem != null)
				{
					ToolStrip currentParent2 = toolStripItem.GetCurrentParent();
					if (currentParent2 != null)
					{
						if (currentParent2.RightToLeft == RightToLeft.Yes)
						{
							backwards = !backwards;
						}
						if (backwards)
						{
							ToolStripItemCollection items2 = currentParent2.Items;
							if (items2.Count >= 2)
							{
								obj = items2[items2.Count - 2];
							}
							else
							{
								obj = this.GetNextControlInTab(control, currentParent2, !backwards);
							}
						}
						else
						{
							ToolStripItemCollection items3 = currentParent2.Items;
							obj = items3[0];
						}
					}
				}
			}
			if (obj == null && control2 != null)
			{
				if (!control.Contains(control2))
				{
					if (control != obj2)
					{
						goto IL_0554;
					}
				}
				while ((control2 = this.GetNextControlInTab(control, control2, !backwards)) != null && (control2.Site == null || control2.Site.Container != container || control2 is ToolStripPanel))
				{
				}
				obj = control2;
			}
			IL_0554:
			if (obj == null)
			{
				ComponentTray componentTray3 = (ComponentTray)this.provider.GetService(typeof(ComponentTray));
				if (componentTray3 != null)
				{
					obj = componentTray3.GetNextComponent((IComponent)obj2, !backwards);
				}
				if (obj == null || obj == obj2)
				{
					obj = control;
				}
			}
			if (obj is DesignerToolStripControlHost && obj2 is DesignerToolStripControlHost)
			{
				this.SelectedDesignerControl = obj;
				selectionService.SetSelectedComponents(new object[] { obj }, SelectionTypes.Replace);
				selectionService.SetSelectedComponents(null, SelectionTypes.Replace);
				return;
			}
			this.SetSelection(obj);
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x0009697C File Offset: 0x0009597C
		private void SelectItems(ToolStrip parent)
		{
			object[] array = new object[parent.Items.Count - 1];
			for (int i = 0; i < parent.Items.Count - 1; i++)
			{
				if (!(parent.Items[i] is DesignerToolStripControlHost))
				{
					array[i] = parent.Items[i];
				}
			}
			this.SelectionService.SetSelectedComponents(array, SelectionTypes.Replace);
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x000969E4 File Offset: 0x000959E4
		private void SetSelection(object targetSelection)
		{
			ISelectionService selectionService = this.SelectionService;
			if (selectionService != null)
			{
				ICollection selectedComponents = selectionService.GetSelectedComponents();
				ArrayList arrayList = new ArrayList(selectedComponents);
				if (arrayList.Count == 0 && this.SelectedDesignerControl != null)
				{
					arrayList.Add(this.SelectedDesignerControl);
				}
				if (targetSelection is DesignerToolStripControlHost)
				{
					if (!this.shiftPressed)
					{
						this.SelectedDesignerControl = targetSelection;
						selectionService.SetSelectedComponents(null, SelectionTypes.Replace);
					}
				}
				else
				{
					ToolStripOverflowButton toolStripOverflowButton = targetSelection as ToolStripOverflowButton;
					if (toolStripOverflowButton != null)
					{
						this.SelectedDesignerControl = null;
						if (toolStripOverflowButton != null)
						{
							toolStripOverflowButton.ShowDropDown();
						}
						object nextItem = this.GetNextItem(toolStripOverflowButton.DropDown, null, ArrowDirection.Down);
						if (!this.shiftPressed)
						{
							this.ShiftPrimaryItem = null;
							selectionService.SetSelectedComponents(new object[] { nextItem }, SelectionTypes.Replace);
						}
						else
						{
							selectionService.SetSelectedComponents(new object[] { nextItem });
							this.ShiftPrimaryItem = targetSelection;
						}
					}
					else
					{
						this.SelectedDesignerControl = null;
						if (!this.shiftPressed)
						{
							this.ShiftPrimaryItem = null;
							selectionService.SetSelectedComponents(new object[] { targetSelection }, SelectionTypes.Replace);
						}
						else
						{
							selectionService.SetSelectedComponents(new object[] { targetSelection });
							this.ShiftPrimaryItem = targetSelection;
						}
					}
				}
				ToolStripDesignerUtils.InvalidateSelection(arrayList, targetSelection as ToolStripItem, this.provider, this.shiftPressed);
			}
			this.shiftPressed = false;
		}

		// Token: 0x04001557 RID: 5463
		private const int GLYPHBORDER = 1;

		// Token: 0x04001558 RID: 5464
		private const int GLYPHINSET = 2;

		// Token: 0x04001559 RID: 5465
		private ISelectionService selectionService;

		// Token: 0x0400155A RID: 5466
		private IComponentChangeService componentChangeSvc;

		// Token: 0x0400155B RID: 5467
		private IServiceProvider provider;

		// Token: 0x0400155C RID: 5468
		private IMenuCommandService menuCommandService;

		// Token: 0x0400155D RID: 5469
		private IDesignerHost designerHost;

		// Token: 0x0400155E RID: 5470
		private object shiftPrimary;

		// Token: 0x0400155F RID: 5471
		private bool shiftPressed;

		// Token: 0x04001560 RID: 5472
		private object currentSelection;

		// Token: 0x04001561 RID: 5473
		private bool templateNodeActive;

		// Token: 0x04001562 RID: 5474
		private ToolStripTemplateNode activeTemplateNode;

		// Token: 0x04001563 RID: 5475
		private bool templateNodeContextMenuOpen;

		// Token: 0x04001564 RID: 5476
		private ArrayList oldCommands;

		// Token: 0x04001565 RID: 5477
		private ArrayList newCommands;

		// Token: 0x04001566 RID: 5478
		private MenuCommand oldCommandPaste;

		// Token: 0x04001567 RID: 5479
		private MenuCommand newCommandPaste;

		// Token: 0x04001568 RID: 5480
		private bool commandsAdded;

		// Token: 0x04001569 RID: 5481
		private bool copyInProgress;

		// Token: 0x0400156A RID: 5482
		private bool cutOrDeleteInProgress;

		// Token: 0x0400156B RID: 5483
		private bool contextMenuShownByKeyBoard;

		// Token: 0x0400156C RID: 5484
		private object ownerItemAfterCut;
	}
}
