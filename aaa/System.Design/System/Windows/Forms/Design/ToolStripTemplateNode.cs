using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002D1 RID: 721
	internal class ToolStripTemplateNode : IMenuStatusHandler
	{
		// Token: 0x06001BB1 RID: 7089 RVA: 0x0009B320 File Offset: 0x0009A320
		public ToolStripTemplateNode(IComponent component, string text, Image image)
		{
			this.component = component;
			this.activeItem = component as ToolStripItem;
			this._designerHost = (IDesignerHost)component.Site.GetService(typeof(IDesignerHost));
			this._designer = this._designerHost.GetDesigner(component);
			this._designSurface = (DesignSurface)component.Site.GetService(typeof(DesignSurface));
			this._designSurface.Flushed += this.OnLoaderFlushed;
			this.SetupNewEditNode(this, text, image, component);
			this.commands = new MenuCommand[]
			{
				new MenuCommand(new EventHandler(this.OnMenuCut), MenuCommands.KeyMoveUp),
				new MenuCommand(new EventHandler(this.OnMenuCut), MenuCommands.KeyMoveDown),
				new MenuCommand(new EventHandler(this.OnMenuCut), MenuCommands.KeyMoveLeft),
				new MenuCommand(new EventHandler(this.OnMenuCut), MenuCommands.KeyMoveRight),
				new MenuCommand(new EventHandler(this.OnMenuCut), StandardCommands.Delete),
				new MenuCommand(new EventHandler(this.OnMenuCut), StandardCommands.Cut),
				new MenuCommand(new EventHandler(this.OnMenuCut), StandardCommands.Copy),
				new MenuCommand(new EventHandler(this.OnMenuCut), MenuCommands.KeyNudgeUp),
				new MenuCommand(new EventHandler(this.OnMenuCut), MenuCommands.KeyNudgeDown),
				new MenuCommand(new EventHandler(this.OnMenuCut), MenuCommands.KeyNudgeLeft),
				new MenuCommand(new EventHandler(this.OnMenuCut), MenuCommands.KeyNudgeRight),
				new MenuCommand(new EventHandler(this.OnMenuCut), MenuCommands.KeySizeWidthIncrease),
				new MenuCommand(new EventHandler(this.OnMenuCut), MenuCommands.KeySizeHeightIncrease),
				new MenuCommand(new EventHandler(this.OnMenuCut), MenuCommands.KeySizeWidthDecrease),
				new MenuCommand(new EventHandler(this.OnMenuCut), MenuCommands.KeySizeHeightDecrease),
				new MenuCommand(new EventHandler(this.OnMenuCut), MenuCommands.KeyNudgeWidthIncrease),
				new MenuCommand(new EventHandler(this.OnMenuCut), MenuCommands.KeyNudgeHeightIncrease),
				new MenuCommand(new EventHandler(this.OnMenuCut), MenuCommands.KeyNudgeWidthDecrease),
				new MenuCommand(new EventHandler(this.OnMenuCut), MenuCommands.KeyNudgeHeightDecrease)
			};
			this.addCommands = new MenuCommand[]
			{
				new MenuCommand(new EventHandler(this.OnMenuCut), StandardCommands.Undo),
				new MenuCommand(new EventHandler(this.OnMenuCut), StandardCommands.Redo)
			};
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001BB2 RID: 7090 RVA: 0x0009B5ED File Offset: 0x0009A5ED
		// (set) Token: 0x06001BB3 RID: 7091 RVA: 0x0009B5F8 File Offset: 0x0009A5F8
		public bool Active
		{
			get
			{
				return this.active;
			}
			set
			{
				if (this.active != value)
				{
					this.active = value;
					if (this.KeyboardService != null)
					{
						this.KeyboardService.TemplateNodeActive = value;
					}
					if (this.active)
					{
						this.OnActivated(new EventArgs());
						if (this.KeyboardService != null)
						{
							this.KeyboardService.ActiveTemplateNode = this;
						}
						IMenuCommandService menuCommandService = (IMenuCommandService)this.component.Site.GetService(typeof(IMenuCommandService));
						if (menuCommandService != null)
						{
							this.oldUndoCommand = menuCommandService.FindCommand(StandardCommands.Undo);
							if (this.oldUndoCommand != null)
							{
								menuCommandService.RemoveCommand(this.oldUndoCommand);
							}
							this.oldRedoCommand = menuCommandService.FindCommand(StandardCommands.Redo);
							if (this.oldRedoCommand != null)
							{
								menuCommandService.RemoveCommand(this.oldRedoCommand);
							}
							for (int i = 0; i < this.addCommands.Length; i++)
							{
								this.addCommands[i].Enabled = false;
								menuCommandService.AddCommand(this.addCommands[i]);
							}
						}
						IEventHandlerService eventHandlerService = (IEventHandlerService)this.component.Site.GetService(typeof(IEventHandlerService));
						if (eventHandlerService != null)
						{
							eventHandlerService.PushHandler(this);
							return;
						}
					}
					else
					{
						this.OnDeactivated(new EventArgs());
						if (this.KeyboardService != null)
						{
							this.KeyboardService.ActiveTemplateNode = null;
						}
						IMenuCommandService menuCommandService2 = (IMenuCommandService)this.component.Site.GetService(typeof(IMenuCommandService));
						if (menuCommandService2 != null)
						{
							for (int j = 0; j < this.addCommands.Length; j++)
							{
								menuCommandService2.RemoveCommand(this.addCommands[j]);
							}
						}
						if (this.oldUndoCommand != null)
						{
							menuCommandService2.AddCommand(this.oldUndoCommand);
						}
						if (this.oldRedoCommand != null)
						{
							menuCommandService2.AddCommand(this.oldRedoCommand);
						}
						IEventHandlerService eventHandlerService2 = (IEventHandlerService)this.component.Site.GetService(typeof(IEventHandlerService));
						if (eventHandlerService2 != null)
						{
							eventHandlerService2.PopHandler(this);
						}
					}
				}
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06001BB4 RID: 7092 RVA: 0x0009B7DD File Offset: 0x0009A7DD
		// (set) Token: 0x06001BB5 RID: 7093 RVA: 0x0009B7E5 File Offset: 0x0009A7E5
		public ToolStripItem ActiveItem
		{
			get
			{
				return this.activeItem;
			}
			set
			{
				this.activeItem = value;
			}
		}

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06001BB6 RID: 7094 RVA: 0x0009B7EE File Offset: 0x0009A7EE
		// (remove) Token: 0x06001BB7 RID: 7095 RVA: 0x0009B807 File Offset: 0x0009A807
		public event EventHandler Activated
		{
			add
			{
				this.onActivated = (EventHandler)Delegate.Combine(this.onActivated, value);
			}
			remove
			{
				this.onActivated = (EventHandler)Delegate.Remove(this.onActivated, value);
			}
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06001BB8 RID: 7096 RVA: 0x0009B820 File Offset: 0x0009A820
		// (set) Token: 0x06001BB9 RID: 7097 RVA: 0x0009B828 File Offset: 0x0009A828
		public Rectangle Bounds
		{
			get
			{
				return this.boundingRect;
			}
			set
			{
				this.boundingRect = value;
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06001BBA RID: 7098 RVA: 0x0009B831 File Offset: 0x0009A831
		// (set) Token: 0x06001BBB RID: 7099 RVA: 0x0009B839 File Offset: 0x0009A839
		public DesignerToolStripControlHost ControlHost
		{
			get
			{
				return this.controlHost;
			}
			set
			{
				this.controlHost = value;
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06001BBC RID: 7100 RVA: 0x0009B844 File Offset: 0x0009A844
		private ContextMenuStrip DesignerContextMenu
		{
			get
			{
				BaseContextMenuStrip baseContextMenuStrip = new BaseContextMenuStrip(this.component.Site, this.controlHost);
				baseContextMenuStrip.Populated = false;
				baseContextMenuStrip.GroupOrdering.Clear();
				baseContextMenuStrip.GroupOrdering.AddRange(new string[] { "Code", "Custom", "Selection", "Edit" });
				baseContextMenuStrip.Text = "CustomContextMenu";
				TemplateNodeCustomMenuItemCollection templateNodeCustomMenuItemCollection = new TemplateNodeCustomMenuItemCollection(this.component.Site, this.controlHost);
				foreach (object obj in templateNodeCustomMenuItemCollection)
				{
					ToolStripItem toolStripItem = (ToolStripItem)obj;
					baseContextMenuStrip.Groups["Custom"].Items.Add(toolStripItem);
				}
				return baseContextMenuStrip;
			}
		}

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06001BBD RID: 7101 RVA: 0x0009B934 File Offset: 0x0009A934
		// (remove) Token: 0x06001BBE RID: 7102 RVA: 0x0009B94D File Offset: 0x0009A94D
		public event EventHandler Deactivated
		{
			add
			{
				this.onDeactivated = (EventHandler)Delegate.Combine(this.onDeactivated, value);
			}
			remove
			{
				this.onDeactivated = (EventHandler)Delegate.Remove(this.onDeactivated, value);
			}
		}

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06001BBF RID: 7103 RVA: 0x0009B966 File Offset: 0x0009A966
		// (remove) Token: 0x06001BC0 RID: 7104 RVA: 0x0009B97F File Offset: 0x0009A97F
		public event EventHandler Closed
		{
			add
			{
				this.onClosed = (EventHandler)Delegate.Combine(this.onClosed, value);
			}
			remove
			{
				this.onClosed = (EventHandler)Delegate.Remove(this.onClosed, value);
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06001BC1 RID: 7105 RVA: 0x0009B998 File Offset: 0x0009A998
		public ToolStrip EditorToolStrip
		{
			get
			{
				return this._miniToolStrip;
			}
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06001BC2 RID: 7106 RVA: 0x0009B9A0 File Offset: 0x0009A9A0
		internal TextBox EditBox
		{
			get
			{
				if (this.centerTextBox == null)
				{
					return null;
				}
				return (TextBox)this.centerTextBox.Control;
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06001BC3 RID: 7107 RVA: 0x0009B9BC File Offset: 0x0009A9BC
		// (set) Token: 0x06001BC4 RID: 7108 RVA: 0x0009B9C4 File Offset: 0x0009A9C4
		public Rectangle HotRegion
		{
			get
			{
				return this.hotRegion;
			}
			set
			{
				this.hotRegion = value;
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06001BC5 RID: 7109 RVA: 0x0009B9CD File Offset: 0x0009A9CD
		// (set) Token: 0x06001BC6 RID: 7110 RVA: 0x0009B9D5 File Offset: 0x0009A9D5
		public bool IMEModeSet
		{
			get
			{
				return this.imeModeSet;
			}
			set
			{
				this.imeModeSet = value;
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06001BC7 RID: 7111 RVA: 0x0009B9DE File Offset: 0x0009A9DE
		private ToolStripKeyboardHandlingService KeyboardService
		{
			get
			{
				if (this.toolStripKeyBoardService == null)
				{
					this.toolStripKeyBoardService = (ToolStripKeyboardHandlingService)this.component.Site.GetService(typeof(ToolStripKeyboardHandlingService));
				}
				return this.toolStripKeyBoardService;
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06001BC8 RID: 7112 RVA: 0x0009BA13 File Offset: 0x0009AA13
		private ISelectionService SelectionService
		{
			get
			{
				if (this.selectionService == null)
				{
					this.selectionService = (ISelectionService)this.component.Site.GetService(typeof(ISelectionService));
				}
				return this.selectionService;
			}
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06001BC9 RID: 7113 RVA: 0x0009BA48 File Offset: 0x0009AA48
		private BehaviorService BehaviorService
		{
			get
			{
				if (this.behaviorService == null)
				{
					this.behaviorService = (BehaviorService)this.component.Site.GetService(typeof(BehaviorService));
				}
				return this.behaviorService;
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06001BCA RID: 7114 RVA: 0x0009BA7D File Offset: 0x0009AA7D
		// (set) Token: 0x06001BCB RID: 7115 RVA: 0x0009BA85 File Offset: 0x0009AA85
		public Type ToolStripItemType
		{
			get
			{
				return this.itemType;
			}
			set
			{
				this.itemType = value;
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06001BCC RID: 7116 RVA: 0x0009BA8E File Offset: 0x0009AA8E
		// (set) Token: 0x06001BCD RID: 7117 RVA: 0x0009BA96 File Offset: 0x0009AA96
		internal bool IsSystemContextMenuDisplayed
		{
			get
			{
				return this.isSystemContextMenuDisplayed;
			}
			set
			{
				this.isSystemContextMenuDisplayed = value;
			}
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x0009BAA0 File Offset: 0x0009AAA0
		private void AddNewItemClick(object sender, EventArgs e)
		{
			if (this.addItemButton != null)
			{
				this.addItemButton.DropDown.Visible = false;
			}
			if (this.component is ToolStrip && this.SelectionService != null)
			{
				ToolStripDesigner toolStripDesigner = this._designerHost.GetDesigner(this.component) as ToolStripDesigner;
				try
				{
					if (toolStripDesigner != null)
					{
						toolStripDesigner.DontCloseOverflow = true;
					}
					this.SelectionService.SetSelectedComponents(new object[] { this.component });
				}
				finally
				{
					if (toolStripDesigner != null)
					{
						toolStripDesigner.DontCloseOverflow = false;
					}
				}
			}
			ItemTypeToolStripMenuItem itemTypeToolStripMenuItem = (ItemTypeToolStripMenuItem)sender;
			if (this.lastSelection != null)
			{
				this.lastSelection.Checked = false;
			}
			itemTypeToolStripMenuItem.Checked = true;
			this.lastSelection = itemTypeToolStripMenuItem;
			this.ToolStripItemType = itemTypeToolStripMenuItem.ItemType;
			ToolStrip currentParent = this.controlHost.GetCurrentParent();
			if (currentParent is MenuStrip)
			{
				this.CommitEditor(true, true, false);
			}
			else
			{
				this.CommitEditor(true, false, false);
			}
			if (this.KeyboardService != null)
			{
				this.KeyboardService.TemplateNodeActive = false;
			}
		}

		// Token: 0x06001BCF RID: 7119 RVA: 0x0009BBA8 File Offset: 0x0009ABA8
		private void CenterLabelClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				if (this.KeyboardService != null && this.KeyboardService.TemplateNodeActive)
				{
					return;
				}
				if (this.KeyboardService != null)
				{
					this.KeyboardService.SelectedDesignerControl = this.controlHost;
				}
				this.SelectionService.SetSelectedComponents(null, SelectionTypes.Replace);
				if (this.BehaviorService != null)
				{
					Point point = this.BehaviorService.ControlToAdornerWindow(this._miniToolStrip);
					point = this.BehaviorService.AdornerWindowPointToScreen(point);
					point.Offset(e.Location);
					this.DesignerContextMenu.Show(point);
					return;
				}
			}
			else
			{
				if (this.hotRegion.Contains(e.Location) && !this.KeyboardService.TemplateNodeActive)
				{
					if (this.KeyboardService != null)
					{
						this.KeyboardService.SelectedDesignerControl = this.controlHost;
					}
					this.SelectionService.SetSelectedComponents(null, SelectionTypes.Replace);
					ToolStripDropDown toolStripDropDown = this.contextMenu;
					if (toolStripDropDown != null)
					{
						toolStripDropDown.Closed -= this.OnContextMenuClosed;
						toolStripDropDown.Opened -= this.OnContextMenuOpened;
						toolStripDropDown.Dispose();
					}
					this.contextMenu = null;
					this.ShowDropDownMenu();
					return;
				}
				ToolStripDesigner.LastCursorPosition = Cursor.Position;
				if (this._designer is ToolStripDesigner)
				{
					if (this.KeyboardService.TemplateNodeActive)
					{
						this.KeyboardService.ActiveTemplateNode.Commit(false, false);
					}
					if (this.SelectionService.PrimarySelection == null)
					{
						this.SelectionService.SetSelectedComponents(new object[] { this.component }, SelectionTypes.Replace);
					}
					this.KeyboardService.SelectedDesignerControl = this.controlHost;
					this.SelectionService.SetSelectedComponents(null, SelectionTypes.Replace);
					((ToolStripDesigner)this._designer).ShowEditNode(true);
				}
				if (this._designer is ToolStripMenuItemDesigner)
				{
					IServiceProvider site = this.component.Site;
					if (this.KeyboardService.TemplateNodeActive)
					{
						ToolStripItem toolStripItem = this.component as ToolStripItem;
						if (toolStripItem != null)
						{
							if (toolStripItem.Visible)
							{
								this.KeyboardService.ActiveTemplateNode.Commit(false, false);
							}
							else
							{
								this.KeyboardService.ActiveTemplateNode.Commit(false, true);
							}
						}
						else
						{
							this.KeyboardService.ActiveTemplateNode.Commit(false, false);
						}
					}
					if (this._designer != null)
					{
						((ToolStripMenuItemDesigner)this._designer).EditTemplateNode(true);
						return;
					}
					ISelectionService selectionService = (ISelectionService)site.GetService(typeof(ISelectionService));
					ToolStripItem toolStripItem2 = selectionService.PrimarySelection as ToolStripItem;
					if (toolStripItem2 != null && this._designerHost != null)
					{
						ToolStripMenuItemDesigner toolStripMenuItemDesigner = this._designerHost.GetDesigner(toolStripItem2) as ToolStripMenuItemDesigner;
						if (toolStripMenuItemDesigner != null)
						{
							if (!toolStripItem2.IsOnDropDown)
							{
								Rectangle glyphBounds = toolStripMenuItemDesigner.GetGlyphBounds();
								ToolStripDesignerUtils.GetAdjustedBounds(toolStripItem2, ref glyphBounds);
								BehaviorService behaviorService = site.GetService(typeof(BehaviorService)) as BehaviorService;
								if (behaviorService != null)
								{
									behaviorService.Invalidate(glyphBounds);
								}
							}
							toolStripMenuItemDesigner.EditTemplateNode(true);
						}
					}
				}
			}
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x0009BE83 File Offset: 0x0009AE83
		private void CenterLabelMouseEnter(object sender, EventArgs e)
		{
			if (this.renderer != null && !this.KeyboardService.TemplateNodeActive && this.renderer.State != 6)
			{
				this.renderer.State = 4;
				this._miniToolStrip.Invalidate();
			}
		}

		// Token: 0x06001BD1 RID: 7121 RVA: 0x0009BEC0 File Offset: 0x0009AEC0
		private void CenterLabelMouseMove(object sender, MouseEventArgs e)
		{
			if (this.renderer != null && !this.KeyboardService.TemplateNodeActive && this.renderer.State != 6)
			{
				if (this.hotRegion.Contains(e.Location))
				{
					this.renderer.State = 5;
				}
				else
				{
					this.renderer.State = 4;
				}
				this._miniToolStrip.Invalidate();
			}
		}

		// Token: 0x06001BD2 RID: 7122 RVA: 0x0009BF28 File Offset: 0x0009AF28
		private void CenterLabelMouseLeave(object sender, EventArgs e)
		{
			if (this.renderer != null && !this.KeyboardService.TemplateNodeActive)
			{
				if (this.renderer.State != 6)
				{
					this.renderer.State = 0;
				}
				if (this.KeyboardService != null && this.KeyboardService.SelectedDesignerControl == this.controlHost)
				{
					this.renderer.State = 1;
				}
				this._miniToolStrip.Invalidate();
			}
		}

		// Token: 0x06001BD3 RID: 7123 RVA: 0x0009BF96 File Offset: 0x0009AF96
		private void CenterTextBoxMouseEnter(object sender, EventArgs e)
		{
			if (this.renderer != null)
			{
				this.renderer.State = 1;
				this._miniToolStrip.Invalidate();
			}
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x0009BFB7 File Offset: 0x0009AFB7
		private void CenterTextBoxMouseLeave(object sender, EventArgs e)
		{
			if (this.renderer != null && !this.Active)
			{
				this.renderer.State = 0;
				this._miniToolStrip.Invalidate();
			}
		}

		// Token: 0x06001BD5 RID: 7125 RVA: 0x0009BFE0 File Offset: 0x0009AFE0
		internal void CloseEditor()
		{
			if (this._miniToolStrip != null)
			{
				this.Active = false;
				if (this.lastSelection != null)
				{
					this.lastSelection.Dispose();
					this.lastSelection = null;
				}
				ToolStrip toolStrip = this.component as ToolStrip;
				if (toolStrip != null)
				{
					toolStrip.RightToLeftChanged -= this.OnRightToLeftChanged;
				}
				else
				{
					ToolStripDropDownItem toolStripDropDownItem = this.component as ToolStripDropDownItem;
					if (toolStripDropDownItem != null)
					{
						toolStripDropDownItem.RightToLeftChanged -= this.OnRightToLeftChanged;
					}
				}
				if (this.centerLabel != null)
				{
					this.centerLabel.MouseUp -= this.CenterLabelClick;
					this.centerLabel.MouseEnter -= this.CenterLabelMouseEnter;
					this.centerLabel.MouseMove -= this.CenterLabelMouseMove;
					this.centerLabel.MouseLeave -= this.CenterLabelMouseLeave;
					this.centerLabel.Dispose();
					this.centerLabel = null;
				}
				if (this.addItemButton != null)
				{
					this.addItemButton.MouseMove -= this.OnMouseMove;
					this.addItemButton.MouseUp -= this.OnMouseUp;
					this.addItemButton.MouseDown -= this.OnMouseDown;
					this.addItemButton.DropDownOpened -= this.OnAddItemButtonDropDownOpened;
					this.addItemButton.DropDown.Dispose();
					this.addItemButton.Dispose();
					this.addItemButton = null;
				}
				if (this.contextMenu != null)
				{
					this.contextMenu.Closed -= this.OnContextMenuClosed;
					this.contextMenu.Opened -= this.OnContextMenuOpened;
					this.contextMenu = null;
				}
				this._miniToolStrip.MouseLeave -= this.OnMouseLeave;
				this._miniToolStrip.Dispose();
				this._miniToolStrip = null;
				this._designSurface.Flushed -= this.OnLoaderFlushed;
				this._designSurface = null;
				this._designer = null;
				this.OnClosed(new EventArgs());
			}
		}

		// Token: 0x06001BD6 RID: 7126 RVA: 0x0009C1F0 File Offset: 0x0009B1F0
		internal void Commit(bool enterKeyPressed, bool tabKeyPressed)
		{
			if (this._miniToolStrip != null && this.inSituMode)
			{
				string text = ((TextBox)this.centerTextBox.Control).Text;
				if (string.IsNullOrEmpty(text))
				{
					this.RollBack();
					return;
				}
				this.CommitEditor(true, enterKeyPressed, tabKeyPressed);
			}
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x0009C23B File Offset: 0x0009B23B
		internal void CommitAndSelect()
		{
			this.Commit(false, false);
		}

		// Token: 0x06001BD8 RID: 7128 RVA: 0x0009C248 File Offset: 0x0009B248
		private void CommitEditor(bool commit, bool enterKeyPressed, bool tabKeyPressed)
		{
			ToolStripItem toolStripItem = this.SelectionService.PrimarySelection as ToolStripItem;
			string text = ((this.centerTextBox != null) ? ((TextBox)this.centerTextBox.Control).Text : string.Empty);
			this.ExitInSituEdit();
			this.FocusForm();
			if (commit && (this._designer is ToolStripDesigner || this._designer is ToolStripMenuItemDesigner))
			{
				if (text == "-" && this._designer is ToolStripMenuItemDesigner)
				{
					this.ToolStripItemType = typeof(ToolStripSeparator);
				}
				Type type;
				if (this.ToolStripItemType != null)
				{
					type = this.ToolStripItemType;
					this.ToolStripItemType = null;
				}
				else
				{
					Type[] standardItemTypes = ToolStripDesignerUtils.GetStandardItemTypes(this.component);
					type = standardItemTypes[0];
				}
				if (this._designer is ToolStripDesigner)
				{
					((ToolStripDesigner)this._designer).AddNewItem(type, text, enterKeyPressed, tabKeyPressed);
				}
				else
				{
					((ToolStripItemDesigner)this._designer).CommitEdit(type, text, commit, enterKeyPressed, tabKeyPressed);
				}
			}
			else if (this._designer is ToolStripItemDesigner)
			{
				((ToolStripItemDesigner)this._designer).CommitEdit(this._designer.Component.GetType(), text, commit, enterKeyPressed, tabKeyPressed);
			}
			if (toolStripItem != null && this._designerHost != null)
			{
				ToolStripItemDesigner toolStripItemDesigner = this._designerHost.GetDesigner(toolStripItem) as ToolStripItemDesigner;
				if (toolStripItemDesigner != null)
				{
					Rectangle glyphBounds = toolStripItemDesigner.GetGlyphBounds();
					ToolStripDesignerUtils.GetAdjustedBounds(toolStripItem, ref glyphBounds);
					glyphBounds.Inflate(1, 1);
					Region region = new Region(glyphBounds);
					glyphBounds.Inflate(-2, -2);
					region.Exclude(glyphBounds);
					if (this.BehaviorService != null)
					{
						this.BehaviorService.Invalidate(region);
					}
					region.Dispose();
				}
			}
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x0009C3F0 File Offset: 0x0009B3F0
		private void EnterInSituEdit()
		{
			if (!this.inSituMode)
			{
				if (this._miniToolStrip.Parent != null)
				{
					this._miniToolStrip.Parent.SuspendLayout();
				}
				try
				{
					this.Active = true;
					this.inSituMode = true;
					if (this.renderer != null)
					{
						this.renderer.State = 1;
					}
					TextBox textBox = new ToolStripTemplateNode.TemplateTextBox(this._miniToolStrip, this);
					textBox.BorderStyle = BorderStyle.FixedSingle;
					textBox.Text = this.centerLabel.Text;
					textBox.ForeColor = SystemColors.WindowText;
					int num = 90;
					this.centerTextBox = new ToolStripControlHost(textBox);
					this.centerTextBox.Dock = DockStyle.None;
					this.centerTextBox.AutoSize = false;
					this.centerTextBox.Width = num;
					ToolStripDropDownItem toolStripDropDownItem = this.activeItem as ToolStripDropDownItem;
					if (toolStripDropDownItem != null && !toolStripDropDownItem.IsOnDropDown)
					{
						this.centerTextBox.Margin = new Padding(1, 2, 1, 3);
					}
					else
					{
						this.centerTextBox.Margin = new Padding(1);
					}
					this.centerTextBox.Size = this._miniToolStrip.DisplayRectangle.Size - this.centerTextBox.Margin.Size;
					this.centerTextBox.Name = "centerTextBox";
					this.centerTextBox.MouseEnter += this.CenterTextBoxMouseEnter;
					this.centerTextBox.MouseLeave += this.CenterTextBoxMouseLeave;
					int num2 = this._miniToolStrip.Items.IndexOf(this.centerLabel);
					if (num2 != -1)
					{
						this._miniToolStrip.Items.Insert(num2, this.centerTextBox);
						this._miniToolStrip.Items.Remove(this.centerLabel);
					}
					textBox.KeyUp += this.OnKeyUp;
					textBox.KeyDown += this.OnKeyDown;
					textBox.SelectAll();
					if (this._designerHost != null)
					{
						Control control = (Control)this._designerHost.RootComponent;
						NativeMethods.SendMessage(control.Handle, 11, 0, 0);
						textBox.Focus();
						NativeMethods.SendMessage(control.Handle, 11, 1, 0);
					}
				}
				finally
				{
					if (this._miniToolStrip.Parent != null)
					{
						this._miniToolStrip.Parent.ResumeLayout();
					}
				}
			}
		}

		// Token: 0x06001BDA RID: 7130 RVA: 0x0009C654 File Offset: 0x0009B654
		private void ExitInSituEdit()
		{
			if (this.centerTextBox != null && this.inSituMode)
			{
				if (this._miniToolStrip.Parent != null)
				{
					this._miniToolStrip.Parent.SuspendLayout();
				}
				try
				{
					int num = this._miniToolStrip.Items.IndexOf(this.centerTextBox);
					if (num != -1)
					{
						this.centerLabel.Text = SR.GetString("ToolStripDesignerTemplateNodeEnterText");
						this._miniToolStrip.Items.Insert(num, this.centerLabel);
						this._miniToolStrip.Items.Remove(this.centerTextBox);
						((TextBox)this.centerTextBox.Control).KeyUp -= this.OnKeyUp;
						((TextBox)this.centerTextBox.Control).KeyDown -= this.OnKeyDown;
					}
					this.centerTextBox.MouseEnter -= this.CenterTextBoxMouseEnter;
					this.centerTextBox.MouseLeave -= this.CenterTextBoxMouseLeave;
					this.centerTextBox.Dispose();
					this.centerTextBox = null;
					this.inSituMode = false;
					this.SetWidth(null);
				}
				finally
				{
					if (this._miniToolStrip.Parent != null)
					{
						this._miniToolStrip.Parent.ResumeLayout();
					}
					this.Active = false;
				}
			}
		}

		// Token: 0x06001BDB RID: 7131 RVA: 0x0009C7BC File Offset: 0x0009B7BC
		internal void FocusEditor(ToolStripItem currentItem)
		{
			if (currentItem != null)
			{
				this.centerLabel.Text = currentItem.Text;
			}
			this.EnterInSituEdit();
		}

		// Token: 0x06001BDC RID: 7132 RVA: 0x0009C7D8 File Offset: 0x0009B7D8
		private void FocusForm()
		{
			DesignerFrame designerFrame = this.component.Site.GetService(typeof(ISplitWindowService)) as DesignerFrame;
			if (designerFrame != null && this._designerHost != null)
			{
				Control control = (Control)this._designerHost.RootComponent;
				NativeMethods.SendMessage(control.Handle, 11, 0, 0);
				designerFrame.Focus();
				NativeMethods.SendMessage(control.Handle, 11, 1, 0);
			}
		}

		// Token: 0x06001BDD RID: 7133 RVA: 0x0009C84A File Offset: 0x0009B84A
		protected void OnActivated(EventArgs e)
		{
			if (this.onActivated != null)
			{
				this.onActivated(this, e);
			}
		}

		// Token: 0x06001BDE RID: 7134 RVA: 0x0009C861 File Offset: 0x0009B861
		private void OnAddItemButtonDropDownOpened(object sender, EventArgs e)
		{
			this.addItemButton.DropDown.Focus();
		}

		// Token: 0x06001BDF RID: 7135 RVA: 0x0009C874 File Offset: 0x0009B874
		protected void OnClosed(EventArgs e)
		{
			if (this.onClosed != null)
			{
				this.onClosed(this, e);
			}
		}

		// Token: 0x06001BE0 RID: 7136 RVA: 0x0009C88B File Offset: 0x0009B88B
		private void OnContextMenuClosed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			if (this.renderer != null)
			{
				this.renderer.State = 1;
				this._miniToolStrip.Invalidate();
			}
		}

		// Token: 0x06001BE1 RID: 7137 RVA: 0x0009C8AC File Offset: 0x0009B8AC
		private void OnContextMenuOpened(object sender, EventArgs e)
		{
			if (this.KeyboardService != null)
			{
				this.KeyboardService.TemplateNodeContextMenuOpen = true;
			}
		}

		// Token: 0x06001BE2 RID: 7138 RVA: 0x0009C8C2 File Offset: 0x0009B8C2
		protected void OnDeactivated(EventArgs e)
		{
			if (this.onDeactivated != null)
			{
				this.onDeactivated(this, e);
			}
		}

		// Token: 0x06001BE3 RID: 7139 RVA: 0x0009C8D9 File Offset: 0x0009B8D9
		private void OnLoaderFlushed(object sender, EventArgs e)
		{
			this.Commit(false, false);
		}

		// Token: 0x06001BE4 RID: 7140 RVA: 0x0009C8E4 File Offset: 0x0009B8E4
		private void OnKeyUp(object sender, KeyEventArgs e)
		{
			if (this.IMEModeSet)
			{
				return;
			}
			Keys keyCode = e.KeyCode;
			if (keyCode != Keys.Return)
			{
				if (keyCode == Keys.Escape)
				{
					this.CommitEditor(false, false, false);
					return;
				}
				switch (keyCode)
				{
				case Keys.Up:
					this.Commit(false, true);
					if (this.KeyboardService != null)
					{
						this.KeyboardService.ProcessUpDown(false);
						return;
					}
					break;
				case Keys.Right:
					break;
				case Keys.Down:
					this.Commit(true, false);
					return;
				default:
					return;
				}
			}
			else
			{
				if (this.ignoreFirstKeyUp)
				{
					this.ignoreFirstKeyUp = false;
					return;
				}
				this.OnKeyDefaultAction(sender, e);
			}
		}

		// Token: 0x06001BE5 RID: 7141 RVA: 0x0009C96C File Offset: 0x0009B96C
		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (this.IMEModeSet)
			{
				return;
			}
			if (e.KeyCode == Keys.A && (e.KeyData & Keys.Control) != Keys.None)
			{
				TextBox textBox = sender as TextBox;
				if (textBox != null)
				{
					textBox.SelectAll();
				}
			}
		}

		// Token: 0x06001BE6 RID: 7142 RVA: 0x0009C9AC File Offset: 0x0009B9AC
		private void OnKeyDefaultAction(object sender, EventArgs e)
		{
			this.Active = false;
			if (this.centerTextBox.Control != null)
			{
				string text = ((TextBox)this.centerTextBox.Control).Text;
				if (string.IsNullOrEmpty(text))
				{
					this.CommitEditor(false, false, false);
					return;
				}
				this.CommitEditor(true, true, false);
			}
		}

		// Token: 0x06001BE7 RID: 7143 RVA: 0x0009C9FE File Offset: 0x0009B9FE
		private void OnMenuCut(object sender, EventArgs e)
		{
		}

		// Token: 0x06001BE8 RID: 7144 RVA: 0x0009CA00 File Offset: 0x0009BA00
		private void OnMouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && this.BehaviorService != null)
			{
				Point point = this.BehaviorService.ControlToAdornerWindow(this._miniToolStrip);
				point = this.BehaviorService.AdornerWindowPointToScreen(point);
				point.Offset(e.Location);
				this.DesignerContextMenu.Show(point);
			}
		}

		// Token: 0x06001BE9 RID: 7145 RVA: 0x0009CA5A File Offset: 0x0009BA5A
		private void OnMouseDown(object sender, MouseEventArgs e)
		{
			if (this.KeyboardService != null)
			{
				this.KeyboardService.SelectedDesignerControl = this.controlHost;
			}
			this.SelectionService.SetSelectedComponents(null, SelectionTypes.Replace);
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x0009CA84 File Offset: 0x0009BA84
		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			this.renderer.State = 0;
			if (this.renderer != null)
			{
				if (this.addItemButton != null)
				{
					if (this.addItemButton.ButtonBounds.Contains(e.Location))
					{
						this.renderer.State = 2;
					}
					else if (this.addItemButton.DropDownButtonBounds.Contains(e.Location))
					{
						this.renderer.State = 3;
					}
				}
				this._miniToolStrip.Invalidate();
			}
		}

		// Token: 0x06001BEB RID: 7147 RVA: 0x0009CB08 File Offset: 0x0009BB08
		private void OnMouseLeave(object sender, EventArgs e)
		{
			if (this.SelectionService != null)
			{
				ToolStripItem toolStripItem = this.SelectionService.PrimarySelection as ToolStripItem;
				if (toolStripItem != null && this.renderer != null && this.renderer.State != 6)
				{
					this.renderer.State = 0;
				}
				if (this.KeyboardService != null && this.KeyboardService.SelectedDesignerControl == this.controlHost)
				{
					this.renderer.State = 1;
				}
				this._miniToolStrip.Invalidate();
			}
		}

		// Token: 0x06001BEC RID: 7148 RVA: 0x0009CB88 File Offset: 0x0009BB88
		private void OnRightToLeftChanged(object sender, EventArgs e)
		{
			ToolStrip toolStrip = sender as ToolStrip;
			if (toolStrip != null)
			{
				this._miniToolStrip.RightToLeft = toolStrip.RightToLeft;
				return;
			}
			ToolStripDropDownItem toolStripDropDownItem = sender as ToolStripDropDownItem;
			this._miniToolStrip.RightToLeft = toolStripDropDownItem.RightToLeft;
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x0009CBCC File Offset: 0x0009BBCC
		public bool OverrideInvoke(MenuCommand cmd)
		{
			for (int i = 0; i < this.commands.Length; i++)
			{
				if (this.commands[i].CommandID.Equals(cmd.CommandID) && (cmd.CommandID == StandardCommands.Delete || cmd.CommandID == StandardCommands.Cut || cmd.CommandID == StandardCommands.Copy))
				{
					this.commands[i].Invoke();
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001BEE RID: 7150 RVA: 0x0009CC40 File Offset: 0x0009BC40
		public bool OverrideStatus(MenuCommand cmd)
		{
			for (int i = 0; i < this.commands.Length; i++)
			{
				if (this.commands[i].CommandID.Equals(cmd.CommandID))
				{
					cmd.Enabled = false;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x0009CC84 File Offset: 0x0009BC84
		internal void RollBack()
		{
			if (this._miniToolStrip != null && this.inSituMode)
			{
				this.CommitEditor(false, false, false);
			}
		}

		// Token: 0x06001BF0 RID: 7152 RVA: 0x0009CC9F File Offset: 0x0009BC9F
		internal void ShowContextMenu(Point pt)
		{
			this.DesignerContextMenu.Show(pt);
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x0009CCB0 File Offset: 0x0009BCB0
		internal void ShowDropDownMenu()
		{
			if (this.addItemButton != null)
			{
				this.addItemButton.ShowDropDown();
				return;
			}
			if (this.BehaviorService != null)
			{
				Point point = this.BehaviorService.ControlToAdornerWindow(this._miniToolStrip);
				point = this.BehaviorService.AdornerWindowPointToScreen(point);
				Rectangle rectangle = new Rectangle(point, this._miniToolStrip.Size);
				if (this.contextMenu == null)
				{
					this.contextMenu = ToolStripDesignerUtils.GetNewItemDropDown(this.component, null, new EventHandler(this.AddNewItemClick), false, this.component.Site);
					this.contextMenu.Closed += this.OnContextMenuClosed;
					this.contextMenu.Opened += this.OnContextMenuOpened;
					this.contextMenu.Text = "ItemSelectionMenu";
				}
				ToolStrip toolStrip = this.component as ToolStrip;
				if (toolStrip != null)
				{
					this.contextMenu.RightToLeft = toolStrip.RightToLeft;
				}
				else
				{
					ToolStripDropDownItem toolStripDropDownItem = this.component as ToolStripDropDownItem;
					if (toolStripDropDownItem != null)
					{
						this.contextMenu.RightToLeft = toolStripDropDownItem.RightToLeft;
					}
				}
				this.contextMenu.Show(rectangle.X, rectangle.Y + rectangle.Height);
				this.contextMenu.Focus();
				if (this.renderer != null)
				{
					this.renderer.State = 6;
					this._miniToolStrip.Invalidate();
				}
			}
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x0009CE0C File Offset: 0x0009BE0C
		private void SetUpMenuTemplateNode(ToolStripTemplateNode owner, string text, Image image, IComponent currentItem)
		{
			this.centerLabel = new ToolStripLabel();
			this.centerLabel.Text = text;
			this.centerLabel.AutoSize = false;
			this.centerLabel.IsLink = false;
			this.centerLabel.Margin = new Padding(1);
			if (currentItem is ToolStripDropDownItem)
			{
				this.centerLabel.Margin = new Padding(1, 2, 1, 3);
			}
			this.centerLabel.Padding = new Padding(0, 1, 0, 0);
			this.centerLabel.Name = "centerLabel";
			this.centerLabel.Size = this._miniToolStrip.DisplayRectangle.Size - this.centerLabel.Margin.Size;
			this.centerLabel.ToolTipText = SR.GetString("ToolStripDesignerTemplateNodeLabelToolTip");
			this.centerLabel.MouseUp += this.CenterLabelClick;
			this.centerLabel.MouseEnter += this.CenterLabelMouseEnter;
			this.centerLabel.MouseMove += this.CenterLabelMouseMove;
			this.centerLabel.MouseLeave += this.CenterLabelMouseLeave;
			this._miniToolStrip.Items.AddRange(new ToolStripItem[] { this.centerLabel });
		}

		// Token: 0x06001BF3 RID: 7155 RVA: 0x0009CF64 File Offset: 0x0009BF64
		private void SetUpToolTemplateNode(ToolStripTemplateNode owner, string text, Image image, IComponent component)
		{
			this.addItemButton = new ToolStripSplitButton();
			this.addItemButton.AutoSize = false;
			this.addItemButton.Margin = new Padding(1);
			this.addItemButton.Size = this._miniToolStrip.DisplayRectangle.Size - this.addItemButton.Margin.Size;
			this.addItemButton.DropDownButtonWidth = 11;
			this.addItemButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			if (component is StatusStrip)
			{
				this.addItemButton.ToolTipText = SR.GetString("ToolStripDesignerTemplateNodeSplitButtonStatusStripToolTip");
			}
			else
			{
				this.addItemButton.ToolTipText = SR.GetString("ToolStripDesignerTemplateNodeSplitButtonToolTip");
			}
			this.addItemButton.MouseDown += this.OnMouseDown;
			this.addItemButton.MouseMove += this.OnMouseMove;
			this.addItemButton.MouseUp += this.OnMouseUp;
			this.addItemButton.DropDownOpened += this.OnAddItemButtonDropDownOpened;
			this.contextMenu = ToolStripDesignerUtils.GetNewItemDropDown(component, null, new EventHandler(this.AddNewItemClick), false, component.Site);
			this.contextMenu.Text = "ItemSelectionMenu";
			this.contextMenu.Closed += this.OnContextMenuClosed;
			this.contextMenu.Opened += this.OnContextMenuOpened;
			this.addItemButton.DropDown = this.contextMenu;
			try
			{
				if (this.addItemButton.DropDownItems.Count > 0)
				{
					ItemTypeToolStripMenuItem itemTypeToolStripMenuItem = (ItemTypeToolStripMenuItem)this.addItemButton.DropDownItems[0];
					this.addItemButton.ImageTransparentColor = Color.Lime;
					this.addItemButton.Image = new Bitmap(typeof(ToolStripTemplateNode), "ToolStripTemplateNode.bmp");
					this.addItemButton.DefaultItem = itemTypeToolStripMenuItem;
				}
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsCriticalException(ex))
				{
					throw;
				}
			}
			this._miniToolStrip.Items.AddRange(new ToolStripItem[] { this.addItemButton });
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x0009D194 File Offset: 0x0009C194
		private void SetupNewEditNode(ToolStripTemplateNode owner, string text, Image image, IComponent currentItem)
		{
			this.renderer = new ToolStripTemplateNode.MiniToolStripRenderer(owner);
			this._miniToolStrip = new ToolStripTemplateNode.TransparentToolStrip(owner);
			ToolStrip toolStrip = currentItem as ToolStrip;
			if (toolStrip != null)
			{
				this._miniToolStrip.RightToLeft = toolStrip.RightToLeft;
				toolStrip.RightToLeftChanged += this.OnRightToLeftChanged;
			}
			ToolStripDropDownItem toolStripDropDownItem = currentItem as ToolStripDropDownItem;
			if (toolStripDropDownItem != null)
			{
				this._miniToolStrip.RightToLeft = toolStripDropDownItem.RightToLeft;
				toolStripDropDownItem.RightToLeftChanged += this.OnRightToLeftChanged;
			}
			this._miniToolStrip.SuspendLayout();
			this._miniToolStrip.CanOverflow = false;
			this._miniToolStrip.Cursor = Cursors.Default;
			this._miniToolStrip.Dock = DockStyle.None;
			this._miniToolStrip.GripStyle = ToolStripGripStyle.Hidden;
			this._miniToolStrip.Name = "miniToolStrip";
			this._miniToolStrip.TabIndex = 0;
			this._miniToolStrip.Text = "miniToolStrip";
			this._miniToolStrip.Visible = true;
			this._miniToolStrip.Renderer = this.renderer;
			if (currentItem is MenuStrip || currentItem is ToolStripDropDownItem)
			{
				this.SetUpMenuTemplateNode(owner, text, image, currentItem);
			}
			else
			{
				this.SetUpToolTemplateNode(owner, text, image, currentItem);
			}
			this._miniToolStrip.MouseLeave += this.OnMouseLeave;
			this._miniToolStrip.ResumeLayout();
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x0009D2E9 File Offset: 0x0009C2E9
		internal void SetWidth(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				this._miniToolStrip.Width = this.centerLabel.Width + 2;
				return;
			}
			this.centerLabel.Text = text;
		}

		// Token: 0x04001594 RID: 5524
		private const int GLYPHBORDER = 1;

		// Token: 0x04001595 RID: 5525
		private const int GLYPHINSET = 2;

		// Token: 0x04001596 RID: 5526
		private IComponent component;

		// Token: 0x04001597 RID: 5527
		private IDesigner _designer;

		// Token: 0x04001598 RID: 5528
		private IDesignerHost _designerHost;

		// Token: 0x04001599 RID: 5529
		private MenuCommand[] commands;

		// Token: 0x0400159A RID: 5530
		private MenuCommand[] addCommands;

		// Token: 0x0400159B RID: 5531
		private ToolStripTemplateNode.TransparentToolStrip _miniToolStrip;

		// Token: 0x0400159C RID: 5532
		private ToolStripLabel centerLabel;

		// Token: 0x0400159D RID: 5533
		private ToolStripSplitButton addItemButton;

		// Token: 0x0400159E RID: 5534
		private ToolStripControlHost centerTextBox;

		// Token: 0x0400159F RID: 5535
		internal bool ignoreFirstKeyUp;

		// Token: 0x040015A0 RID: 5536
		private Rectangle boundingRect;

		// Token: 0x040015A1 RID: 5537
		private bool inSituMode;

		// Token: 0x040015A2 RID: 5538
		private bool active;

		// Token: 0x040015A3 RID: 5539
		private ItemTypeToolStripMenuItem lastSelection;

		// Token: 0x040015A4 RID: 5540
		private ToolStripTemplateNode.MiniToolStripRenderer renderer;

		// Token: 0x040015A5 RID: 5541
		private Type itemType;

		// Token: 0x040015A6 RID: 5542
		private ToolStripKeyboardHandlingService toolStripKeyBoardService;

		// Token: 0x040015A7 RID: 5543
		private ISelectionService selectionService;

		// Token: 0x040015A8 RID: 5544
		private BehaviorService behaviorService;

		// Token: 0x040015A9 RID: 5545
		private DesignerToolStripControlHost controlHost;

		// Token: 0x040015AA RID: 5546
		private ToolStripItem activeItem;

		// Token: 0x040015AB RID: 5547
		private EventHandler onActivated;

		// Token: 0x040015AC RID: 5548
		private EventHandler onClosed;

		// Token: 0x040015AD RID: 5549
		private EventHandler onDeactivated;

		// Token: 0x040015AE RID: 5550
		private MenuCommand oldUndoCommand;

		// Token: 0x040015AF RID: 5551
		private MenuCommand oldRedoCommand;

		// Token: 0x040015B0 RID: 5552
		private ToolStripDropDown contextMenu;

		// Token: 0x040015B1 RID: 5553
		private Rectangle hotRegion;

		// Token: 0x040015B2 RID: 5554
		private bool imeModeSet;

		// Token: 0x040015B3 RID: 5555
		private DesignSurface _designSurface;

		// Token: 0x040015B4 RID: 5556
		private bool isSystemContextMenuDisplayed;

		// Token: 0x020002D2 RID: 722
		private class TemplateTextBox : TextBox
		{
			// Token: 0x06001BF6 RID: 7158 RVA: 0x0009D318 File Offset: 0x0009C318
			public TemplateTextBox(ToolStripTemplateNode.TransparentToolStrip parent, ToolStripTemplateNode owner)
			{
				this.parent = parent;
				this.owner = owner;
				this.AutoSize = false;
				this.Multiline = false;
			}

			// Token: 0x06001BF7 RID: 7159 RVA: 0x0009D33C File Offset: 0x0009C33C
			private bool IsParentWindow(IntPtr hWnd)
			{
				return hWnd == this.parent.Handle;
			}

			// Token: 0x06001BF8 RID: 7160 RVA: 0x0009D354 File Offset: 0x0009C354
			protected override bool IsInputKey(Keys keyData)
			{
				Keys keys = keyData & Keys.KeyCode;
				if (keys == Keys.Return)
				{
					this.owner.Commit(true, false);
					return true;
				}
				return base.IsInputKey(keyData);
			}

			// Token: 0x06001BF9 RID: 7161 RVA: 0x0009D384 File Offset: 0x0009C384
			protected override bool ProcessDialogKey(Keys keyData)
			{
				if (keyData == Keys.ProcessKey)
				{
					this.owner.IMEModeSet = true;
				}
				else
				{
					this.owner.IMEModeSet = false;
					this.owner.ignoreFirstKeyUp = false;
				}
				return base.ProcessDialogKey(keyData);
			}

			// Token: 0x06001BFA RID: 7162 RVA: 0x0009D3BC File Offset: 0x0009C3BC
			protected override void WndProc(ref Message m)
			{
				int msg = m.Msg;
				if (msg != 8)
				{
					if (msg == 123)
					{
						this.owner.IsSystemContextMenuDisplayed = true;
						base.WndProc(ref m);
						this.owner.IsSystemContextMenuDisplayed = false;
						return;
					}
					base.WndProc(ref m);
				}
				else
				{
					base.WndProc(ref m);
					IntPtr wparam = m.WParam;
					if (!this.IsParentWindow(wparam))
					{
						this.owner.Commit(false, false);
						return;
					}
				}
			}

			// Token: 0x040015B5 RID: 5557
			private const int IMEMODE = 229;

			// Token: 0x040015B6 RID: 5558
			private ToolStripTemplateNode.TransparentToolStrip parent;

			// Token: 0x040015B7 RID: 5559
			private ToolStripTemplateNode owner;
		}

		// Token: 0x020002D3 RID: 723
		public class TransparentToolStrip : ToolStrip
		{
			// Token: 0x06001BFB RID: 7163 RVA: 0x0009D427 File Offset: 0x0009C427
			public TransparentToolStrip(ToolStripTemplateNode owner)
			{
				this.owner = owner;
				this.currentItem = owner.component;
				base.TabStop = true;
				base.SetStyle(ControlStyles.Selectable, true);
				this.AutoSize = false;
			}

			// Token: 0x170004D5 RID: 1237
			// (get) Token: 0x06001BFC RID: 7164 RVA: 0x0009D45C File Offset: 0x0009C45C
			public ToolStripTemplateNode TemplateNode
			{
				get
				{
					return this.owner;
				}
			}

			// Token: 0x06001BFD RID: 7165 RVA: 0x0009D464 File Offset: 0x0009C464
			private void CommitAndSelectNext(bool forward)
			{
				this.owner.Commit(false, true);
				if (this.owner.KeyboardService != null)
				{
					this.owner.KeyboardService.ProcessKeySelect(!forward, null);
				}
			}

			// Token: 0x06001BFE RID: 7166 RVA: 0x0009D498 File Offset: 0x0009C498
			private ToolStripItem GetSelectedItem()
			{
				ToolStripItem toolStripItem = null;
				for (int i = 0; i < this.Items.Count; i++)
				{
					if (this.Items[i].Selected)
					{
						toolStripItem = this.Items[i];
					}
				}
				return toolStripItem;
			}

			// Token: 0x06001BFF RID: 7167 RVA: 0x0009D4DE File Offset: 0x0009C4DE
			[EditorBrowsable(EditorBrowsableState.Advanced)]
			public override Size GetPreferredSize(Size proposedSize)
			{
				if (this.currentItem is ToolStripDropDownItem)
				{
					return new Size(base.Width, 22);
				}
				return new Size(base.Width, 19);
			}

			// Token: 0x06001C00 RID: 7168 RVA: 0x0009D508 File Offset: 0x0009C508
			private bool ProcessTabKey(bool forward)
			{
				ToolStripItem selectedItem = this.GetSelectedItem();
				if (selectedItem is ToolStripControlHost)
				{
					this.CommitAndSelectNext(forward);
					return true;
				}
				return false;
			}

			// Token: 0x06001C01 RID: 7169 RVA: 0x0009D530 File Offset: 0x0009C530
			protected override bool ProcessDialogKey(Keys keyData)
			{
				bool flag = false;
				if (this.owner.Active)
				{
					if ((keyData & (Keys.Control | Keys.Alt)) == Keys.None)
					{
						Keys keys = keyData & Keys.KeyCode;
						Keys keys2 = keys;
						if (keys2 == Keys.Tab)
						{
							flag = this.ProcessTabKey((keyData & Keys.Shift) == Keys.None);
						}
					}
					if (flag)
					{
						return flag;
					}
				}
				return base.ProcessDialogKey(keyData);
			}

			// Token: 0x06001C02 RID: 7170 RVA: 0x0009D584 File Offset: 0x0009C584
			[EditorBrowsable(EditorBrowsableState.Advanced)]
			protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
			{
				if (this.currentItem is ToolStripDropDownItem)
				{
					base.SetBoundsCore(x, y, 92, 22, specified);
					return;
				}
				if (this.currentItem is MenuStrip)
				{
					base.SetBoundsCore(x, y, 92, 19, specified);
					return;
				}
				base.SetBoundsCore(x, y, 31, 19, specified);
			}

			// Token: 0x040015B8 RID: 5560
			private ToolStripTemplateNode owner;

			// Token: 0x040015B9 RID: 5561
			private IComponent currentItem;
		}

		// Token: 0x020002D4 RID: 724
		public class MiniToolStripRenderer : ToolStripSystemRenderer
		{
			// Token: 0x06001C03 RID: 7171 RVA: 0x0009D5D8 File Offset: 0x0009C5D8
			public MiniToolStripRenderer(ToolStripTemplateNode owner)
			{
				this.owner = owner;
				this.selectedBorderColor = Color.FromArgb(46, 106, 197);
				this.defaultBorderColor = Color.FromArgb(171, 171, 171);
				this.dropDownMouseOverColor = Color.FromArgb(193, 210, 238);
				this.dropDownMouseDownColor = Color.FromArgb(152, 181, 226);
				this.toolStripBorderColor = Color.White;
			}

			// Token: 0x170004D6 RID: 1238
			// (get) Token: 0x06001C04 RID: 7172 RVA: 0x0009D66A File Offset: 0x0009C66A
			// (set) Token: 0x06001C05 RID: 7173 RVA: 0x0009D672 File Offset: 0x0009C672
			public int State
			{
				get
				{
					return this.state;
				}
				set
				{
					this.state = value;
				}
			}

			// Token: 0x06001C06 RID: 7174 RVA: 0x0009D67B File Offset: 0x0009C67B
			private void DrawArrow(Graphics g, Rectangle bounds)
			{
				bounds.Width--;
				base.DrawArrow(new ToolStripArrowRenderEventArgs(g, null, bounds, SystemColors.ControlText, ArrowDirection.Down));
			}

			// Token: 0x06001C07 RID: 7175 RVA: 0x0009D6A4 File Offset: 0x0009C6A4
			private void DrawDropDown(Graphics g, Rectangle bounds, int state)
			{
				switch (state)
				{
				case 4:
				{
					using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(bounds, Color.White, this.defaultBorderColor, LinearGradientMode.Vertical))
					{
						g.FillRectangle(linearGradientBrush, bounds);
						goto IL_0089;
					}
					break;
				}
				case 5:
					break;
				case 6:
					goto IL_0064;
				default:
					goto IL_0089;
				}
				using (SolidBrush solidBrush = new SolidBrush(this.dropDownMouseOverColor))
				{
					g.FillRectangle(solidBrush, this.hotRegion);
					goto IL_0089;
				}
				IL_0064:
				using (SolidBrush solidBrush2 = new SolidBrush(this.dropDownMouseDownColor))
				{
					g.FillRectangle(solidBrush2, this.hotRegion);
				}
				IL_0089:
				this.DrawArrow(g, bounds);
			}

			// Token: 0x06001C08 RID: 7176 RVA: 0x0009D76C File Offset: 0x0009C76C
			protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
			{
				if (this.owner.component is MenuStrip || this.owner.component is ToolStripDropDownItem)
				{
					Graphics graphics = e.Graphics;
					graphics.Clear(this.toolStripBorderColor);
					return;
				}
				base.OnRenderToolStripBackground(e);
			}

			// Token: 0x06001C09 RID: 7177 RVA: 0x0009D7B8 File Offset: 0x0009C7B8
			protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
			{
				Graphics graphics = e.Graphics;
				Rectangle rectangle = new Rectangle(Point.Empty, e.ToolStrip.Size);
				Pen pen = new Pen(this.toolStripBorderColor);
				Rectangle rectangle2 = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
				graphics.DrawRectangle(pen, rectangle2);
				pen.Dispose();
			}

			// Token: 0x06001C0A RID: 7178 RVA: 0x0009D824 File Offset: 0x0009C824
			protected override void OnRenderLabelBackground(ToolStripItemRenderEventArgs e)
			{
				base.OnRenderLabelBackground(e);
				ToolStripItem item = e.Item;
				ToolStrip toolStrip = e.ToolStrip;
				Graphics graphics = e.Graphics;
				Rectangle rectangle = new Rectangle(Point.Empty, item.Size);
				Rectangle rectangle2 = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
				Pen pen = new Pen(this.defaultBorderColor);
				if (this.state == 1)
				{
					graphics.FillRectangle(new SolidBrush(this.toolStripBorderColor), rectangle2);
					if (this.owner.EditorToolStrip.RightToLeft == RightToLeft.Yes)
					{
						this.hotRegion = new Rectangle(rectangle.Left + 2, rectangle.Top + 2, 9, rectangle.Bottom - 4);
					}
					else
					{
						this.hotRegion = new Rectangle(rectangle.Right - 11, rectangle.Top + 2, 9, rectangle.Bottom - 4);
					}
					this.owner.HotRegion = this.hotRegion;
					pen.Color = Color.Black;
					item.ForeColor = this.defaultBorderColor;
					graphics.DrawRectangle(pen, rectangle2);
				}
				if (this.state == 4)
				{
					if (this.owner.EditorToolStrip.RightToLeft == RightToLeft.Yes)
					{
						this.hotRegion = new Rectangle(rectangle.Left + 2, rectangle.Top + 2, 9, rectangle.Bottom - 4);
					}
					else
					{
						this.hotRegion = new Rectangle(rectangle.Right - 11, rectangle.Top + 2, 9, rectangle.Bottom - 4);
					}
					this.owner.HotRegion = this.hotRegion;
					graphics.Clear(this.toolStripBorderColor);
					this.DrawDropDown(graphics, this.hotRegion, this.state);
					pen.Color = Color.Black;
					pen.DashStyle = DashStyle.Dot;
					graphics.DrawRectangle(pen, rectangle2);
				}
				if (this.state == 5)
				{
					graphics.Clear(this.toolStripBorderColor);
					this.DrawDropDown(graphics, this.hotRegion, this.state);
					pen.Color = Color.Black;
					pen.DashStyle = DashStyle.Dot;
					item.ForeColor = this.defaultBorderColor;
					graphics.DrawRectangle(pen, rectangle2);
				}
				if (this.state == 6)
				{
					graphics.Clear(this.toolStripBorderColor);
					this.DrawDropDown(graphics, this.hotRegion, this.state);
					pen.Color = Color.Black;
					item.ForeColor = this.defaultBorderColor;
					graphics.DrawRectangle(pen, rectangle2);
				}
				if (this.state == 0)
				{
					graphics.Clear(this.toolStripBorderColor);
					graphics.DrawRectangle(pen, rectangle2);
					item.ForeColor = this.defaultBorderColor;
				}
				pen.Dispose();
			}

			// Token: 0x06001C0B RID: 7179 RVA: 0x0009DAD4 File Offset: 0x0009CAD4
			protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
			{
				Graphics graphics = e.Graphics;
				ToolStripSplitButton toolStripSplitButton = e.Item as ToolStripSplitButton;
				if (toolStripSplitButton != null)
				{
					Rectangle dropDownButtonBounds = toolStripSplitButton.DropDownButtonBounds;
					using (Pen pen = new Pen(this.toolStripBorderColor))
					{
						graphics.DrawLine(pen, dropDownButtonBounds.Left, dropDownButtonBounds.Top + 1, dropDownButtonBounds.Left, dropDownButtonBounds.Bottom - 1);
					}
					Rectangle rectangle = new Rectangle(Point.Empty, toolStripSplitButton.Size);
					bool flag = false;
					if (toolStripSplitButton.DropDownButtonPressed)
					{
						this.state = 0;
						Rectangle rectangle2 = new Rectangle(dropDownButtonBounds.Left + 1, dropDownButtonBounds.Top, dropDownButtonBounds.Right, dropDownButtonBounds.Bottom);
						graphics.FillRectangle(new SolidBrush(this.dropDownMouseDownColor), rectangle2);
						flag = true;
					}
					else if (this.state == 2)
					{
						graphics.FillRectangle(new SolidBrush(this.dropDownMouseOverColor), toolStripSplitButton.ButtonBounds);
						flag = true;
					}
					else if (this.state == 3)
					{
						Rectangle rectangle3 = new Rectangle(dropDownButtonBounds.Left + 1, dropDownButtonBounds.Top, dropDownButtonBounds.Right, dropDownButtonBounds.Bottom);
						graphics.FillRectangle(new SolidBrush(this.dropDownMouseOverColor), rectangle3);
						flag = true;
					}
					else if (this.state == 1)
					{
						flag = true;
					}
					Pen pen2;
					if (flag)
					{
						pen2 = new Pen(this.selectedBorderColor);
					}
					else
					{
						pen2 = new Pen(this.defaultBorderColor);
					}
					Rectangle rectangle4 = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
					graphics.DrawRectangle(pen2, rectangle4);
					pen2.Dispose();
					base.DrawArrow(new ToolStripArrowRenderEventArgs(graphics, toolStripSplitButton, toolStripSplitButton.DropDownButtonBounds, SystemColors.ControlText, ArrowDirection.Down));
				}
			}

			// Token: 0x040015BA RID: 5562
			private int state;

			// Token: 0x040015BB RID: 5563
			private Color selectedBorderColor;

			// Token: 0x040015BC RID: 5564
			private Color defaultBorderColor;

			// Token: 0x040015BD RID: 5565
			private Color dropDownMouseOverColor;

			// Token: 0x040015BE RID: 5566
			private Color dropDownMouseDownColor;

			// Token: 0x040015BF RID: 5567
			private Color toolStripBorderColor;

			// Token: 0x040015C0 RID: 5568
			private ToolStripTemplateNode owner;

			// Token: 0x040015C1 RID: 5569
			private Rectangle hotRegion = Rectangle.Empty;
		}
	}
}
