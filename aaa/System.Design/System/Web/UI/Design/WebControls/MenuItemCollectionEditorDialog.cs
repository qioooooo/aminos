using System;
using System.Design;
using System.Drawing;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200047E RID: 1150
	internal sealed partial class MenuItemCollectionEditorDialog : CollectionEditorDialog
	{
		// Token: 0x060029B5 RID: 10677 RVA: 0x000E4430 File Offset: 0x000E3430
		public MenuItemCollectionEditorDialog(global::System.Web.UI.WebControls.Menu menu, MenuDesigner menuDesigner)
			: base(menu.Site)
		{
			this._webMenu = menu;
			this._menuDesigner = menuDesigner;
			this._treeViewPanel = new global::System.Windows.Forms.Panel();
			this._treeView = new global::System.Windows.Forms.TreeView();
			this._treeViewToolBar = new ToolStrip();
			ToolStripRenderer toolStripRenderer = UIServiceHelper.GetToolStripRenderer(base.ServiceProvider);
			if (toolStripRenderer != null)
			{
				this._treeViewToolBar.Renderer = toolStripRenderer;
			}
			this._propertyGrid = new VsPropertyGrid(base.ServiceProvider);
			this._okButton = new global::System.Windows.Forms.Button();
			this._cancelButton = new global::System.Windows.Forms.Button();
			this._propertiesLabel = new global::System.Windows.Forms.Label();
			this._nodesLabel = new global::System.Windows.Forms.Label();
			this._addRootButton = base.CreatePushButton(SR.GetString("MenuItemCollectionEditor_AddRoot"), 3);
			this._addChildButton = base.CreatePushButton(SR.GetString("MenuItemCollectionEditor_AddChild"), 2);
			this._removeButton = base.CreatePushButton(SR.GetString("MenuItemCollectionEditor_Remove"), 4);
			this._moveUpButton = base.CreatePushButton(SR.GetString("MenuItemCollectionEditor_MoveUp"), 5);
			this._moveDownButton = base.CreatePushButton(SR.GetString("MenuItemCollectionEditor_MoveDown"), 6);
			this._indentButton = base.CreatePushButton(SR.GetString("MenuItemCollectionEditor_Indent"), 1);
			this._unindentButton = base.CreatePushButton(SR.GetString("MenuItemCollectionEditor_Unindent"), 0);
			this._toolBarSeparator = new ToolStripSeparator();
			this._treeViewPanel.SuspendLayout();
			base.SuspendLayout();
			this._treeViewPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this._treeViewPanel.BackColor = SystemColors.ControlDark;
			this._treeViewPanel.Controls.Add(this._treeView);
			this._treeViewPanel.DockPadding.Left = 1;
			this._treeViewPanel.DockPadding.Right = 1;
			this._treeViewPanel.DockPadding.Bottom = 1;
			this._treeViewPanel.DockPadding.Top = 1;
			this._treeViewPanel.Location = new Point(12, 54);
			this._treeViewPanel.Name = "_treeViewPanel";
			this._treeViewPanel.Size = new Size(227, 233);
			this._treeViewPanel.TabIndex = 1;
			this._treeView.BorderStyle = global::System.Windows.Forms.BorderStyle.None;
			this._treeView.Dock = DockStyle.Fill;
			this._treeView.ImageIndex = -1;
			this._treeView.HideSelection = false;
			this._treeView.Location = new Point(1, 1);
			this._treeView.Name = "_treeView";
			this._treeView.SelectedImageIndex = -1;
			this._treeView.TabIndex = 0;
			this._treeView.AfterSelect += this.OnTreeViewAfterSelect;
			this._treeView.KeyDown += this.OnTreeViewKeyDown;
			this._treeViewToolBar.Items.AddRange(new ToolStripItem[] { this._addRootButton, this._addChildButton, this._removeButton, this._toolBarSeparator, this._moveUpButton, this._moveDownButton, this._unindentButton, this._indentButton });
			this._treeViewToolBar.Location = new Point(12, 28);
			this._treeViewToolBar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this._treeViewToolBar.AutoSize = false;
			this._treeViewToolBar.Size = new Size(227, 26);
			this._treeViewToolBar.CanOverflow = false;
			Padding padding = this._treeViewToolBar.Padding;
			padding.Left = 2;
			this._treeViewToolBar.Padding = padding;
			this._treeViewToolBar.Name = "_treeViewToolBar";
			this._treeViewToolBar.ShowItemToolTips = true;
			this._treeViewToolBar.GripStyle = ToolStripGripStyle.Hidden;
			this._treeViewToolBar.TabIndex = 1;
			this._treeViewToolBar.TabStop = true;
			this._treeViewToolBar.ItemClicked += this.OnTreeViewToolBarButtonClick;
			this._propertyGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
			this._propertyGrid.CommandsVisibleIfAvailable = true;
			this._propertyGrid.LargeButtons = false;
			this._propertyGrid.LineColor = SystemColors.ScrollBar;
			this._propertyGrid.Location = new Point(260, 28);
			this._propertyGrid.Name = "_propertyGrid";
			this._propertyGrid.PropertySort = PropertySort.Alphabetical;
			this._propertyGrid.Size = new Size(204, 259);
			this._propertyGrid.TabIndex = 3;
			this._propertyGrid.Text = SR.GetString("MenuItemCollectionEditor_PropertyGrid");
			this._propertyGrid.ToolbarVisible = true;
			this._propertyGrid.ViewBackColor = SystemColors.Window;
			this._propertyGrid.ViewForeColor = SystemColors.WindowText;
			this._propertyGrid.PropertyValueChanged += this.OnPropertyGridPropertyValueChanged;
			this._propertyGrid.Site = this._webMenu.Site;
			this._okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this._okButton.FlatStyle = FlatStyle.System;
			this._okButton.Location = new Point(309, 296);
			this._okButton.Name = "_okButton";
			this._okButton.Size = new Size(75, 23);
			this._okButton.TabIndex = 9;
			this._okButton.Text = SR.GetString("MenuItemCollectionEditor_OK");
			this._okButton.Click += this.OnOkButtonClick;
			this._cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this._cancelButton.FlatStyle = FlatStyle.System;
			this._cancelButton.Location = new Point(389, 296);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new Size(75, 23);
			this._cancelButton.TabIndex = 10;
			this._cancelButton.Text = SR.GetString("MenuItemCollectionEditor_Cancel");
			this._cancelButton.Click += this.OnCancelButtonClick;
			this._propertiesLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this._propertiesLabel.Location = new Point(260, 12);
			this._propertiesLabel.Name = "_propertiesLabel";
			this._propertiesLabel.Size = new Size(204, 14);
			this._propertiesLabel.TabIndex = 2;
			this._propertiesLabel.Text = SR.GetString("MenuItemCollectionEditor_Properties");
			this._nodesLabel.Location = new Point(12, 12);
			this._nodesLabel.Name = "_nodesLabel";
			this._nodesLabel.Size = new Size(100, 14);
			this._nodesLabel.TabIndex = 0;
			this._nodesLabel.Text = SR.GetString("MenuItemCollectionEditor_Nodes");
			ImageList imageList = new ImageList();
			imageList.ImageSize = new Size(16, 16);
			imageList.TransparentColor = Color.Magenta;
			imageList.Images.AddStrip(new Bitmap(base.GetType(), "Commands.bmp"));
			this._treeViewToolBar.ImageList = imageList;
			base.ClientSize = new Size(478, 331);
			base.CancelButton = this._cancelButton;
			base.Controls.AddRange(new Control[] { this._nodesLabel, this._propertiesLabel, this._cancelButton, this._okButton, this._propertyGrid, this._treeViewPanel, this._treeViewToolBar });
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MinimumSize = new Size(484, 331);
			base.Name = "TreeNodeEditor";
			base.SizeGripStyle = SizeGripStyle.Hide;
			this.Text = SR.GetString("MenuItemCollectionEditor_Title");
			this._treeViewPanel.ResumeLayout(false);
			base.InitializeForm();
			base.ResumeLayout(false);
		}

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x060029B6 RID: 10678 RVA: 0x000E4C09 File Offset: 0x000E3C09
		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.Menu.CollectionEditor";
			}
		}

		// Token: 0x060029B7 RID: 10679 RVA: 0x000E4C10 File Offset: 0x000E3C10
		private void LoadNodes(global::System.Windows.Forms.TreeNodeCollection destNodes, MenuItemCollection sourceNodes)
		{
			foreach (object obj in sourceNodes)
			{
				global::System.Web.UI.WebControls.MenuItem menuItem = (global::System.Web.UI.WebControls.MenuItem)obj;
				MenuItemCollectionEditorDialog.MenuItemContainer menuItemContainer = new MenuItemCollectionEditorDialog.MenuItemContainer();
				destNodes.Add(menuItemContainer);
				menuItemContainer.Text = menuItem.Text;
				global::System.Web.UI.WebControls.MenuItem menuItem2 = (global::System.Web.UI.WebControls.MenuItem)((ICloneable)menuItem).Clone();
				this._menuDesigner.RegisterClone(menuItem, menuItem2);
				menuItemContainer.WebMenuItem = menuItem2;
				if (menuItem.ChildItems.Count > 0)
				{
					this.LoadNodes(menuItemContainer.Nodes, menuItem.ChildItems);
				}
			}
		}

		// Token: 0x060029B8 RID: 10680 RVA: 0x000E4CBC File Offset: 0x000E3CBC
		private void OnAddChildButtonClick()
		{
			this.ValidatePropertyGrid();
			global::System.Windows.Forms.TreeNode selectedNode = this._treeView.SelectedNode;
			if (selectedNode != null)
			{
				MenuItemCollectionEditorDialog.MenuItemContainer menuItemContainer = new MenuItemCollectionEditorDialog.MenuItemContainer();
				selectedNode.Nodes.Add(menuItemContainer);
				string @string = SR.GetString("MenuItemCollectionEditor_NewNodeText");
				menuItemContainer.Text = @string;
				menuItemContainer.WebMenuItem.Text = @string;
				selectedNode.Expand();
				this._treeView.SelectedNode = menuItemContainer;
			}
		}

		// Token: 0x060029B9 RID: 10681 RVA: 0x000E4D24 File Offset: 0x000E3D24
		private void OnAddRootButtonClick()
		{
			this.ValidatePropertyGrid();
			MenuItemCollectionEditorDialog.MenuItemContainer menuItemContainer = new MenuItemCollectionEditorDialog.MenuItemContainer();
			this._treeView.Nodes.Add(menuItemContainer);
			string @string = SR.GetString("MenuItemCollectionEditor_NewNodeText");
			menuItemContainer.Text = @string;
			menuItemContainer.WebMenuItem.Text = @string;
			this._treeView.SelectedNode = menuItemContainer;
		}

		// Token: 0x060029BA RID: 10682 RVA: 0x000E4D79 File Offset: 0x000E3D79
		private void OnCancelButtonClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x060029BB RID: 10683 RVA: 0x000E4D88 File Offset: 0x000E3D88
		private void OnIndentButtonClick()
		{
			this.ValidatePropertyGrid();
			global::System.Windows.Forms.TreeNode selectedNode = this._treeView.SelectedNode;
			if (selectedNode != null)
			{
				global::System.Windows.Forms.TreeNode prevNode = selectedNode.PrevNode;
				if (prevNode != null)
				{
					selectedNode.Remove();
					prevNode.Nodes.Add(selectedNode);
					this._treeView.SelectedNode = selectedNode;
				}
			}
		}

		// Token: 0x060029BC RID: 10684 RVA: 0x000E4DD4 File Offset: 0x000E3DD4
		protected override void OnInitialActivated(EventArgs e)
		{
			base.OnInitialActivated(e);
			this.LoadNodes(this._treeView.Nodes, this._webMenu.Items);
			if (this._treeView.Nodes.Count > 0)
			{
				this._treeView.SelectedNode = this._treeView.Nodes[0];
			}
			this.UpdateEnabledState();
		}

		// Token: 0x060029BD RID: 10685 RVA: 0x000E4E39 File Offset: 0x000E3E39
		private void OnTreeViewAfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node != null)
			{
				this._propertyGrid.SelectedObject = ((MenuItemCollectionEditorDialog.MenuItemContainer)e.Node).WebMenuItem;
			}
			else
			{
				this._propertyGrid.SelectedObject = null;
			}
			this.UpdateEnabledState();
		}

		// Token: 0x060029BE RID: 10686 RVA: 0x000E4E74 File Offset: 0x000E3E74
		private void OnTreeViewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Insert)
			{
				if ((Control.ModifierKeys & Keys.Alt) != Keys.None)
				{
					this.OnAddChildButtonClick();
				}
				else
				{
					this.OnAddRootButtonClick();
				}
				e.Handled = true;
				return;
			}
			if (e.KeyCode == Keys.Delete)
			{
				this.OnRemoveButtonClick();
				e.Handled = true;
				return;
			}
			if ((Control.ModifierKeys & Keys.Shift) != Keys.None)
			{
				if (e.KeyCode == Keys.Up)
				{
					this.OnMoveUpButtonClick();
				}
				else if (e.KeyCode == Keys.Down)
				{
					this.OnMoveDownButtonClick();
				}
				else if (e.KeyCode == Keys.Left)
				{
					this.OnUnindentButtonClick();
				}
				else if (e.KeyCode == Keys.Right)
				{
					this.OnIndentButtonClick();
				}
				e.Handled = true;
			}
		}

		// Token: 0x060029BF RID: 10687 RVA: 0x000E4F20 File Offset: 0x000E3F20
		private void OnTreeViewToolBarButtonClick(object sender, ToolStripItemClickedEventArgs e)
		{
			if (e.ClickedItem == this._addRootButton)
			{
				this.OnAddRootButtonClick();
				return;
			}
			if (e.ClickedItem == this._addChildButton)
			{
				this.OnAddChildButtonClick();
				return;
			}
			if (e.ClickedItem == this._removeButton)
			{
				this.OnRemoveButtonClick();
				return;
			}
			if (e.ClickedItem == this._moveUpButton)
			{
				this.OnMoveUpButtonClick();
				return;
			}
			if (e.ClickedItem == this._unindentButton)
			{
				this.OnUnindentButtonClick();
				return;
			}
			if (e.ClickedItem == this._indentButton)
			{
				this.OnIndentButtonClick();
				return;
			}
			if (e.ClickedItem == this._moveDownButton)
			{
				this.OnMoveDownButtonClick();
			}
		}

		// Token: 0x060029C0 RID: 10688 RVA: 0x000E4FC0 File Offset: 0x000E3FC0
		private void OnMoveDownButtonClick()
		{
			this.ValidatePropertyGrid();
			global::System.Windows.Forms.TreeNode selectedNode = this._treeView.SelectedNode;
			if (selectedNode != null)
			{
				global::System.Windows.Forms.TreeNode nextNode = selectedNode.NextNode;
				global::System.Windows.Forms.TreeNodeCollection treeNodeCollection = this._treeView.Nodes;
				if (selectedNode.Parent != null)
				{
					treeNodeCollection = selectedNode.Parent.Nodes;
				}
				if (nextNode != null)
				{
					selectedNode.Remove();
					treeNodeCollection.Insert(nextNode.Index + 1, selectedNode);
					this._treeView.SelectedNode = selectedNode;
				}
			}
		}

		// Token: 0x060029C1 RID: 10689 RVA: 0x000E5030 File Offset: 0x000E4030
		private void OnMoveUpButtonClick()
		{
			this.ValidatePropertyGrid();
			global::System.Windows.Forms.TreeNode selectedNode = this._treeView.SelectedNode;
			if (selectedNode != null)
			{
				global::System.Windows.Forms.TreeNode prevNode = selectedNode.PrevNode;
				global::System.Windows.Forms.TreeNodeCollection treeNodeCollection = this._treeView.Nodes;
				if (selectedNode.Parent != null)
				{
					treeNodeCollection = selectedNode.Parent.Nodes;
				}
				if (prevNode != null)
				{
					selectedNode.Remove();
					treeNodeCollection.Insert(prevNode.Index, selectedNode);
					this._treeView.SelectedNode = selectedNode;
				}
			}
		}

		// Token: 0x060029C2 RID: 10690 RVA: 0x000E509B File Offset: 0x000E409B
		private void OnOkButtonClick(object sender, EventArgs e)
		{
			this.ValidatePropertyGrid();
			this.SaveNodes(this._webMenu.Items, this._treeView.Nodes);
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x060029C3 RID: 10691 RVA: 0x000E50CC File Offset: 0x000E40CC
		private void OnPropertyGridPropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
		{
			this.ValidatePropertyGrid();
			MenuItemCollectionEditorDialog.MenuItemContainer menuItemContainer = (MenuItemCollectionEditorDialog.MenuItemContainer)this._treeView.SelectedNode;
			if (menuItemContainer != null)
			{
				menuItemContainer.Text = menuItemContainer.WebMenuItem.Text;
			}
			this._propertyGrid.Refresh();
		}

		// Token: 0x060029C4 RID: 10692 RVA: 0x000E5110 File Offset: 0x000E4110
		private void OnRemoveButtonClick()
		{
			this.ValidatePropertyGrid();
			global::System.Windows.Forms.TreeNode selectedNode = this._treeView.SelectedNode;
			if (selectedNode != null)
			{
				global::System.Windows.Forms.TreeNodeCollection treeNodeCollection;
				if (selectedNode.Parent != null)
				{
					treeNodeCollection = selectedNode.Parent.Nodes;
				}
				else
				{
					treeNodeCollection = this._treeView.Nodes;
				}
				if (treeNodeCollection.Count == 1)
				{
					this._treeView.SelectedNode = selectedNode.Parent;
				}
				else if (selectedNode.NextNode != null)
				{
					this._treeView.SelectedNode = selectedNode.NextNode;
				}
				else
				{
					this._treeView.SelectedNode = selectedNode.PrevNode;
				}
				selectedNode.Remove();
				if (this._treeView.SelectedNode == null)
				{
					this._propertyGrid.SelectedObject = null;
				}
				this.UpdateEnabledState();
			}
		}

		// Token: 0x060029C5 RID: 10693 RVA: 0x000E51C8 File Offset: 0x000E41C8
		private void OnUnindentButtonClick()
		{
			this.ValidatePropertyGrid();
			global::System.Windows.Forms.TreeNode selectedNode = this._treeView.SelectedNode;
			if (selectedNode != null)
			{
				global::System.Windows.Forms.TreeNode parent = selectedNode.Parent;
				if (parent != null)
				{
					global::System.Windows.Forms.TreeNodeCollection treeNodeCollection = this._treeView.Nodes;
					if (parent.Parent != null)
					{
						treeNodeCollection = parent.Parent.Nodes;
					}
					if (parent != null)
					{
						selectedNode.Remove();
						treeNodeCollection.Insert(parent.Index + 1, selectedNode);
						this._treeView.SelectedNode = selectedNode;
					}
				}
			}
		}

		// Token: 0x060029C6 RID: 10694 RVA: 0x000E5238 File Offset: 0x000E4238
		private void SaveNodes(MenuItemCollection destNodes, global::System.Windows.Forms.TreeNodeCollection sourceNodes)
		{
			this.ValidatePropertyGrid();
			destNodes.Clear();
			foreach (object obj in sourceNodes)
			{
				MenuItemCollectionEditorDialog.MenuItemContainer menuItemContainer = (MenuItemCollectionEditorDialog.MenuItemContainer)obj;
				global::System.Web.UI.WebControls.MenuItem webMenuItem = menuItemContainer.WebMenuItem;
				destNodes.Add(webMenuItem);
				if (menuItemContainer.Nodes.Count > 0)
				{
					this.SaveNodes(webMenuItem.ChildItems, menuItemContainer.Nodes);
				}
			}
		}

		// Token: 0x060029C7 RID: 10695 RVA: 0x000E52C0 File Offset: 0x000E42C0
		private void UpdateEnabledState()
		{
			global::System.Windows.Forms.TreeNode selectedNode = this._treeView.SelectedNode;
			if (selectedNode != null)
			{
				this._addChildButton.Enabled = true;
				this._removeButton.Enabled = true;
				this._moveUpButton.Enabled = selectedNode.PrevNode != null;
				this._moveDownButton.Enabled = selectedNode.NextNode != null;
				this._indentButton.Enabled = selectedNode.PrevNode != null;
				this._unindentButton.Enabled = selectedNode.Parent != null;
				return;
			}
			this._addChildButton.Enabled = false;
			this._removeButton.Enabled = false;
			this._moveUpButton.Enabled = false;
			this._moveDownButton.Enabled = false;
			this._indentButton.Enabled = false;
			this._unindentButton.Enabled = false;
		}

		// Token: 0x060029C8 RID: 10696 RVA: 0x000E539C File Offset: 0x000E439C
		private void ValidatePropertyGrid()
		{
			MenuItemCollectionEditorDialog.MenuItemContainer menuItemContainer = (MenuItemCollectionEditorDialog.MenuItemContainer)this._treeView.SelectedNode;
			if (menuItemContainer != null)
			{
				menuItemContainer.Text = menuItemContainer.WebMenuItem.Text;
				if (menuItemContainer.WebMenuItem.Selected && (!menuItemContainer.WebMenuItem.Selectable || !menuItemContainer.WebMenuItem.Enabled))
				{
					UIServiceHelper.ShowError(base.ServiceProvider, SR.GetString("MenuItemCollectionEditor_CantSelect"));
					menuItemContainer.WebMenuItem.Selected = false;
					this._propertyGrid.Refresh();
				}
			}
		}

		// Token: 0x04001C93 RID: 7315
		private global::System.Windows.Forms.Panel _treeViewPanel;

		// Token: 0x04001C94 RID: 7316
		private global::System.Windows.Forms.TreeView _treeView;

		// Token: 0x04001C95 RID: 7317
		private PropertyGrid _propertyGrid;

		// Token: 0x04001C96 RID: 7318
		private global::System.Windows.Forms.Button _okButton;

		// Token: 0x04001C97 RID: 7319
		private global::System.Windows.Forms.Button _cancelButton;

		// Token: 0x04001C98 RID: 7320
		private global::System.Windows.Forms.Label _propertiesLabel;

		// Token: 0x04001C99 RID: 7321
		private global::System.Windows.Forms.Label _nodesLabel;

		// Token: 0x04001C9A RID: 7322
		private ToolStripButton _addRootButton;

		// Token: 0x04001C9B RID: 7323
		private ToolStripButton _addChildButton;

		// Token: 0x04001C9C RID: 7324
		private ToolStripButton _removeButton;

		// Token: 0x04001C9D RID: 7325
		private ToolStripButton _moveUpButton;

		// Token: 0x04001C9E RID: 7326
		private ToolStripButton _moveDownButton;

		// Token: 0x04001C9F RID: 7327
		private ToolStripButton _indentButton;

		// Token: 0x04001CA0 RID: 7328
		private ToolStripButton _unindentButton;

		// Token: 0x04001CA1 RID: 7329
		private ToolStripSeparator _toolBarSeparator;

		// Token: 0x04001CA2 RID: 7330
		private ToolStrip _treeViewToolBar;

		// Token: 0x04001CA3 RID: 7331
		private global::System.Web.UI.WebControls.Menu _webMenu;

		// Token: 0x04001CA4 RID: 7332
		private MenuDesigner _menuDesigner;

		// Token: 0x0200047F RID: 1151
		private class MenuItemContainer : global::System.Windows.Forms.TreeNode
		{
			// Token: 0x170007C5 RID: 1989
			// (get) Token: 0x060029C9 RID: 10697 RVA: 0x000E5421 File Offset: 0x000E4421
			// (set) Token: 0x060029CA RID: 10698 RVA: 0x000E543C File Offset: 0x000E443C
			public global::System.Web.UI.WebControls.MenuItem WebMenuItem
			{
				get
				{
					if (this._webMenuNode == null)
					{
						this._webMenuNode = new global::System.Web.UI.WebControls.MenuItem();
					}
					return this._webMenuNode;
				}
				set
				{
					this._webMenuNode = value;
				}
			}

			// Token: 0x04001CA5 RID: 7333
			private global::System.Web.UI.WebControls.MenuItem _webMenuNode;
		}
	}
}
