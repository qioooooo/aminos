using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	internal class ToolStripTemplateNode : IMenuStatusHandler
	{
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

		public ToolStrip EditorToolStrip
		{
			get
			{
				return this._miniToolStrip;
			}
		}

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

		private void CenterLabelMouseEnter(object sender, EventArgs e)
		{
			if (this.renderer != null && !this.KeyboardService.TemplateNodeActive && this.renderer.State != 6)
			{
				this.renderer.State = 4;
				this._miniToolStrip.Invalidate();
			}
		}

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

		private void CenterTextBoxMouseEnter(object sender, EventArgs e)
		{
			if (this.renderer != null)
			{
				this.renderer.State = 1;
				this._miniToolStrip.Invalidate();
			}
		}

		private void CenterTextBoxMouseLeave(object sender, EventArgs e)
		{
			if (this.renderer != null && !this.Active)
			{
				this.renderer.State = 0;
				this._miniToolStrip.Invalidate();
			}
		}

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

		internal void CommitAndSelect()
		{
			this.Commit(false, false);
		}

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

		internal void FocusEditor(ToolStripItem currentItem)
		{
			if (currentItem != null)
			{
				this.centerLabel.Text = currentItem.Text;
			}
			this.EnterInSituEdit();
		}

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

		protected void OnActivated(EventArgs e)
		{
			if (this.onActivated != null)
			{
				this.onActivated(this, e);
			}
		}

		private void OnAddItemButtonDropDownOpened(object sender, EventArgs e)
		{
			this.addItemButton.DropDown.Focus();
		}

		protected void OnClosed(EventArgs e)
		{
			if (this.onClosed != null)
			{
				this.onClosed(this, e);
			}
		}

		private void OnContextMenuClosed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			if (this.renderer != null)
			{
				this.renderer.State = 1;
				this._miniToolStrip.Invalidate();
			}
		}

		private void OnContextMenuOpened(object sender, EventArgs e)
		{
			if (this.KeyboardService != null)
			{
				this.KeyboardService.TemplateNodeContextMenuOpen = true;
			}
		}

		protected void OnDeactivated(EventArgs e)
		{
			if (this.onDeactivated != null)
			{
				this.onDeactivated(this, e);
			}
		}

		private void OnLoaderFlushed(object sender, EventArgs e)
		{
			this.Commit(false, false);
		}

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

		private void OnMenuCut(object sender, EventArgs e)
		{
		}

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

		private void OnMouseDown(object sender, MouseEventArgs e)
		{
			if (this.KeyboardService != null)
			{
				this.KeyboardService.SelectedDesignerControl = this.controlHost;
			}
			this.SelectionService.SetSelectedComponents(null, SelectionTypes.Replace);
		}

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

		internal void RollBack()
		{
			if (this._miniToolStrip != null && this.inSituMode)
			{
				this.CommitEditor(false, false, false);
			}
		}

		internal void ShowContextMenu(Point pt)
		{
			this.DesignerContextMenu.Show(pt);
		}

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

		internal void SetWidth(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				this._miniToolStrip.Width = this.centerLabel.Width + 2;
				return;
			}
			this.centerLabel.Text = text;
		}

		private const int GLYPHBORDER = 1;

		private const int GLYPHINSET = 2;

		private IComponent component;

		private IDesigner _designer;

		private IDesignerHost _designerHost;

		private MenuCommand[] commands;

		private MenuCommand[] addCommands;

		private ToolStripTemplateNode.TransparentToolStrip _miniToolStrip;

		private ToolStripLabel centerLabel;

		private ToolStripSplitButton addItemButton;

		private ToolStripControlHost centerTextBox;

		internal bool ignoreFirstKeyUp;

		private Rectangle boundingRect;

		private bool inSituMode;

		private bool active;

		private ItemTypeToolStripMenuItem lastSelection;

		private ToolStripTemplateNode.MiniToolStripRenderer renderer;

		private Type itemType;

		private ToolStripKeyboardHandlingService toolStripKeyBoardService;

		private ISelectionService selectionService;

		private BehaviorService behaviorService;

		private DesignerToolStripControlHost controlHost;

		private ToolStripItem activeItem;

		private EventHandler onActivated;

		private EventHandler onClosed;

		private EventHandler onDeactivated;

		private MenuCommand oldUndoCommand;

		private MenuCommand oldRedoCommand;

		private ToolStripDropDown contextMenu;

		private Rectangle hotRegion;

		private bool imeModeSet;

		private DesignSurface _designSurface;

		private bool isSystemContextMenuDisplayed;

		private class TemplateTextBox : TextBox
		{
			public TemplateTextBox(ToolStripTemplateNode.TransparentToolStrip parent, ToolStripTemplateNode owner)
			{
				this.parent = parent;
				this.owner = owner;
				this.AutoSize = false;
				this.Multiline = false;
			}

			private bool IsParentWindow(IntPtr hWnd)
			{
				return hWnd == this.parent.Handle;
			}

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

			private const int IMEMODE = 229;

			private ToolStripTemplateNode.TransparentToolStrip parent;

			private ToolStripTemplateNode owner;
		}

		public class TransparentToolStrip : ToolStrip
		{
			public TransparentToolStrip(ToolStripTemplateNode owner)
			{
				this.owner = owner;
				this.currentItem = owner.component;
				base.TabStop = true;
				base.SetStyle(ControlStyles.Selectable, true);
				this.AutoSize = false;
			}

			public ToolStripTemplateNode TemplateNode
			{
				get
				{
					return this.owner;
				}
			}

			private void CommitAndSelectNext(bool forward)
			{
				this.owner.Commit(false, true);
				if (this.owner.KeyboardService != null)
				{
					this.owner.KeyboardService.ProcessKeySelect(!forward, null);
				}
			}

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

			[EditorBrowsable(EditorBrowsableState.Advanced)]
			public override Size GetPreferredSize(Size proposedSize)
			{
				if (this.currentItem is ToolStripDropDownItem)
				{
					return new Size(base.Width, 22);
				}
				return new Size(base.Width, 19);
			}

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

			private ToolStripTemplateNode owner;

			private IComponent currentItem;
		}

		public class MiniToolStripRenderer : ToolStripSystemRenderer
		{
			public MiniToolStripRenderer(ToolStripTemplateNode owner)
			{
				this.owner = owner;
				this.selectedBorderColor = Color.FromArgb(46, 106, 197);
				this.defaultBorderColor = Color.FromArgb(171, 171, 171);
				this.dropDownMouseOverColor = Color.FromArgb(193, 210, 238);
				this.dropDownMouseDownColor = Color.FromArgb(152, 181, 226);
				this.toolStripBorderColor = Color.White;
			}

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

			private void DrawArrow(Graphics g, Rectangle bounds)
			{
				bounds.Width--;
				base.DrawArrow(new ToolStripArrowRenderEventArgs(g, null, bounds, SystemColors.ControlText, ArrowDirection.Down));
			}

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

			protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
			{
				Graphics graphics = e.Graphics;
				Rectangle rectangle = new Rectangle(Point.Empty, e.ToolStrip.Size);
				Pen pen = new Pen(this.toolStripBorderColor);
				Rectangle rectangle2 = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
				graphics.DrawRectangle(pen, rectangle2);
				pen.Dispose();
			}

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

			private int state;

			private Color selectedBorderColor;

			private Color defaultBorderColor;

			private Color dropDownMouseOverColor;

			private Color dropDownMouseDownColor;

			private Color toolStripBorderColor;

			private ToolStripTemplateNode owner;

			private Rectangle hotRegion = Rectangle.Empty;
		}
	}
}
