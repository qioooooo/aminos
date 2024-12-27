using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Globalization;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002D8 RID: 728
	internal class TreeNodeCollectionEditor : CollectionEditor
	{
		// Token: 0x06001C14 RID: 7188 RVA: 0x0009DDF1 File Offset: 0x0009CDF1
		public TreeNodeCollectionEditor()
			: base(typeof(TreeNodeCollection))
		{
		}

		// Token: 0x06001C15 RID: 7189 RVA: 0x0009DE03 File Offset: 0x0009CE03
		protected override CollectionEditor.CollectionForm CreateCollectionForm()
		{
			return new TreeNodeCollectionEditor.TreeNodeCollectionForm(this);
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06001C16 RID: 7190 RVA: 0x0009DE0B File Offset: 0x0009CE0B
		protected override string HelpTopic
		{
			get
			{
				return "net.ComponentModel.TreeNodeCollectionEditor";
			}
		}

		// Token: 0x020002D9 RID: 729
		private class TreeNodeCollectionForm : CollectionEditor.CollectionForm
		{
			// Token: 0x06001C17 RID: 7191 RVA: 0x0009DE12 File Offset: 0x0009CE12
			public TreeNodeCollectionForm(CollectionEditor editor)
				: base(editor)
			{
				this.editor = (TreeNodeCollectionEditor)editor;
				this.InitializeComponent();
				this.HookEvents();
				this.intialNextNode = this.NextNode;
				this.SetButtonsState();
			}

			// Token: 0x170004DA RID: 1242
			// (get) Token: 0x06001C18 RID: 7192 RVA: 0x0009DE48 File Offset: 0x0009CE48
			private TreeNode LastNode
			{
				get
				{
					TreeNode treeNode = this.treeView1.Nodes[this.treeView1.Nodes.Count - 1];
					while (treeNode.Nodes.Count > 0)
					{
						treeNode = treeNode.Nodes[treeNode.Nodes.Count - 1];
					}
					return treeNode;
				}
			}

			// Token: 0x170004DB RID: 1243
			// (get) Token: 0x06001C19 RID: 7193 RVA: 0x0009DEA2 File Offset: 0x0009CEA2
			private TreeView TreeView
			{
				get
				{
					if (base.Context != null && base.Context.Instance is TreeView)
					{
						return (TreeView)base.Context.Instance;
					}
					return null;
				}
			}

			// Token: 0x170004DC RID: 1244
			// (get) Token: 0x06001C1A RID: 7194 RVA: 0x0009DED0 File Offset: 0x0009CED0
			// (set) Token: 0x06001C1B RID: 7195 RVA: 0x0009DF50 File Offset: 0x0009CF50
			private int NextNode
			{
				get
				{
					if (this.TreeView != null && this.TreeView.Site != null)
					{
						IDictionaryService dictionaryService = (IDictionaryService)this.TreeView.Site.GetService(typeof(IDictionaryService));
						if (dictionaryService != null)
						{
							object value = dictionaryService.GetValue(TreeNodeCollectionEditor.TreeNodeCollectionForm.NextNodeKey);
							if (value != null)
							{
								this.nextNode = (int)value;
							}
							else
							{
								this.nextNode = 0;
								dictionaryService.SetValue(TreeNodeCollectionEditor.TreeNodeCollectionForm.NextNodeKey, 0);
							}
						}
					}
					return this.nextNode;
				}
				set
				{
					this.nextNode = value;
					if (this.TreeView != null && this.TreeView.Site != null)
					{
						IDictionaryService dictionaryService = (IDictionaryService)this.TreeView.Site.GetService(typeof(IDictionaryService));
						if (dictionaryService != null)
						{
							dictionaryService.SetValue(TreeNodeCollectionEditor.TreeNodeCollectionForm.NextNodeKey, this.nextNode);
						}
					}
				}
			}

			// Token: 0x06001C1C RID: 7196 RVA: 0x0009DFB4 File Offset: 0x0009CFB4
			private void Add(TreeNode parent)
			{
				string @string = SR.GetString("BaseNodeName");
				TreeNode treeNode;
				if (parent == null)
				{
					treeNode = this.treeView1.Nodes.Add(@string + this.NextNode++.ToString(CultureInfo.InvariantCulture));
					treeNode.Name = treeNode.Text;
				}
				else
				{
					treeNode = parent.Nodes.Add(@string + this.NextNode++.ToString(CultureInfo.InvariantCulture));
					treeNode.Name = treeNode.Text;
					parent.Expand();
				}
				if (parent != null)
				{
					this.treeView1.SelectedNode = parent;
					return;
				}
				this.treeView1.SelectedNode = treeNode;
				this.SetNodeProps(treeNode);
			}

			// Token: 0x06001C1D RID: 7197 RVA: 0x0009E07C File Offset: 0x0009D07C
			private void HookEvents()
			{
				this.okButton.Click += this.BtnOK_click;
				this.btnCancel.Click += this.BtnCancel_click;
				this.btnAddChild.Click += this.BtnAddChild_click;
				this.btnAddRoot.Click += this.BtnAddRoot_click;
				this.btnDelete.Click += this.BtnDelete_click;
				this.propertyGrid1.PropertyValueChanged += this.PropertyGrid_propertyValueChanged;
				this.treeView1.AfterSelect += this.treeView1_afterSelect;
				this.treeView1.DragEnter += this.treeView1_DragEnter;
				this.treeView1.ItemDrag += this.treeView1_ItemDrag;
				this.treeView1.DragDrop += this.treeView1_DragDrop;
				this.treeView1.DragOver += this.treeView1_DragOver;
				base.HelpButtonClicked += this.TreeNodeCollectionEditor_HelpButtonClicked;
				this.moveDownButton.Click += this.moveDownButton_Click;
				this.moveUpButton.Click += this.moveUpButton_Click;
			}

			// Token: 0x06001C1E RID: 7198 RVA: 0x0009E1C8 File Offset: 0x0009D1C8
			private void InitializeComponent()
			{
				ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(TreeNodeCollectionEditor));
				this.okCancelPanel = new TableLayoutPanel();
				this.okButton = new Button();
				this.btnCancel = new Button();
				this.nodeControlPanel = new TableLayoutPanel();
				this.btnAddRoot = new Button();
				this.btnAddChild = new Button();
				this.btnDelete = new Button();
				this.moveDownButton = new Button();
				this.moveUpButton = new Button();
				this.propertyGrid1 = new VsPropertyGrid(base.Context);
				this.label2 = new Label();
				this.treeView1 = new TreeView();
				this.label1 = new Label();
				this.overarchingTableLayoutPanel = new TableLayoutPanel();
				this.navigationButtonsTableLayoutPanel = new TableLayoutPanel();
				this.okCancelPanel.SuspendLayout();
				this.nodeControlPanel.SuspendLayout();
				this.overarchingTableLayoutPanel.SuspendLayout();
				this.navigationButtonsTableLayoutPanel.SuspendLayout();
				base.SuspendLayout();
				componentResourceManager.ApplyResources(this.okCancelPanel, "okCancelPanel");
				this.okCancelPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
				this.okCancelPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
				this.okCancelPanel.Controls.Add(this.okButton, 0, 0);
				this.okCancelPanel.Controls.Add(this.btnCancel, 1, 0);
				this.okCancelPanel.Margin = new Padding(3, 3, 0, 0);
				this.okCancelPanel.Name = "okCancelPanel";
				this.okCancelPanel.RowStyles.Add(new RowStyle());
				componentResourceManager.ApplyResources(this.okButton, "okButton");
				this.okButton.DialogResult = DialogResult.OK;
				this.okButton.Margin = new Padding(0, 0, 3, 0);
				this.okButton.Name = "okButton";
				componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
				this.btnCancel.DialogResult = DialogResult.Cancel;
				this.btnCancel.Margin = new Padding(3, 0, 0, 0);
				this.btnCancel.Name = "btnCancel";
				componentResourceManager.ApplyResources(this.nodeControlPanel, "nodeControlPanel");
				this.nodeControlPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
				this.nodeControlPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
				this.nodeControlPanel.Controls.Add(this.btnAddRoot, 0, 0);
				this.nodeControlPanel.Controls.Add(this.btnAddChild, 1, 0);
				this.nodeControlPanel.Margin = new Padding(0, 3, 3, 3);
				this.nodeControlPanel.Name = "nodeControlPanel";
				this.nodeControlPanel.RowStyles.Add(new RowStyle());
				componentResourceManager.ApplyResources(this.btnAddRoot, "btnAddRoot");
				this.btnAddRoot.Margin = new Padding(0, 0, 3, 0);
				this.btnAddRoot.Name = "btnAddRoot";
				componentResourceManager.ApplyResources(this.btnAddChild, "btnAddChild");
				this.btnAddChild.Margin = new Padding(3, 0, 0, 0);
				this.btnAddChild.Name = "btnAddChild";
				componentResourceManager.ApplyResources(this.btnDelete, "btnDelete");
				this.btnDelete.Margin = new Padding(0, 3, 0, 0);
				this.btnDelete.Name = "btnDelete";
				componentResourceManager.ApplyResources(this.moveDownButton, "moveDownButton");
				this.moveDownButton.Margin = new Padding(0, 1, 0, 3);
				this.moveDownButton.Name = "moveDownButton";
				componentResourceManager.ApplyResources(this.moveUpButton, "moveUpButton");
				this.moveUpButton.Margin = new Padding(0, 0, 0, 1);
				this.moveUpButton.Name = "moveUpButton";
				componentResourceManager.ApplyResources(this.propertyGrid1, "propertyGrid1");
				this.propertyGrid1.LineColor = SystemColors.ScrollBar;
				this.propertyGrid1.Margin = new Padding(3, 3, 0, 3);
				this.propertyGrid1.Name = "propertyGrid1";
				this.overarchingTableLayoutPanel.SetRowSpan(this.propertyGrid1, 2);
				componentResourceManager.ApplyResources(this.label2, "label2");
				this.label2.Margin = new Padding(3, 1, 0, 0);
				this.label2.Name = "label2";
				this.treeView1.AllowDrop = true;
				componentResourceManager.ApplyResources(this.treeView1, "treeView1");
				this.treeView1.HideSelection = false;
				this.treeView1.Margin = new Padding(0, 3, 3, 3);
				this.treeView1.Name = "treeView1";
				componentResourceManager.ApplyResources(this.label1, "label1");
				this.label1.Margin = new Padding(0, 1, 3, 0);
				this.label1.Name = "label1";
				componentResourceManager.ApplyResources(this.overarchingTableLayoutPanel, "overarchingTableLayoutPanel");
				this.overarchingTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
				this.overarchingTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
				this.overarchingTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
				this.overarchingTableLayoutPanel.Controls.Add(this.navigationButtonsTableLayoutPanel, 1, 1);
				this.overarchingTableLayoutPanel.Controls.Add(this.label2, 2, 0);
				this.overarchingTableLayoutPanel.Controls.Add(this.propertyGrid1, 2, 1);
				this.overarchingTableLayoutPanel.Controls.Add(this.treeView1, 0, 1);
				this.overarchingTableLayoutPanel.Controls.Add(this.label1, 0, 0);
				this.overarchingTableLayoutPanel.Controls.Add(this.nodeControlPanel, 0, 2);
				this.overarchingTableLayoutPanel.Controls.Add(this.okCancelPanel, 2, 3);
				this.overarchingTableLayoutPanel.Name = "overarchingTableLayoutPanel";
				this.overarchingTableLayoutPanel.RowStyles.Add(new RowStyle());
				this.overarchingTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
				this.overarchingTableLayoutPanel.RowStyles.Add(new RowStyle());
				this.overarchingTableLayoutPanel.RowStyles.Add(new RowStyle());
				componentResourceManager.ApplyResources(this.navigationButtonsTableLayoutPanel, "navigationButtonsTableLayoutPanel");
				this.navigationButtonsTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
				this.navigationButtonsTableLayoutPanel.Controls.Add(this.moveUpButton, 0, 0);
				this.navigationButtonsTableLayoutPanel.Controls.Add(this.btnDelete, 0, 2);
				this.navigationButtonsTableLayoutPanel.Controls.Add(this.moveDownButton, 0, 1);
				this.navigationButtonsTableLayoutPanel.Margin = new Padding(3, 3, 18, 3);
				this.navigationButtonsTableLayoutPanel.Name = "navigationButtonsTableLayoutPanel";
				this.navigationButtonsTableLayoutPanel.RowStyles.Add(new RowStyle());
				this.navigationButtonsTableLayoutPanel.RowStyles.Add(new RowStyle());
				this.navigationButtonsTableLayoutPanel.RowStyles.Add(new RowStyle());
				base.AcceptButton = this.okButton;
				componentResourceManager.ApplyResources(this, "$this");
				base.AutoScaleMode = AutoScaleMode.Font;
				base.CancelButton = this.btnCancel;
				base.Controls.Add(this.overarchingTableLayoutPanel);
				base.HelpButton = true;
				base.MaximizeBox = false;
				base.MinimizeBox = false;
				base.Name = "TreeNodeCollectionEditor";
				base.ShowIcon = false;
				base.ShowInTaskbar = false;
				base.SizeGripStyle = SizeGripStyle.Show;
				this.okCancelPanel.ResumeLayout(false);
				this.okCancelPanel.PerformLayout();
				this.nodeControlPanel.ResumeLayout(false);
				this.nodeControlPanel.PerformLayout();
				this.overarchingTableLayoutPanel.ResumeLayout(false);
				this.overarchingTableLayoutPanel.PerformLayout();
				this.navigationButtonsTableLayoutPanel.ResumeLayout(false);
				base.ResumeLayout(false);
			}

			// Token: 0x06001C1F RID: 7199 RVA: 0x0009E9D0 File Offset: 0x0009D9D0
			protected override void OnEditValueChanged()
			{
				if (base.EditValue != null)
				{
					object[] items = base.Items;
					this.propertyGrid1.Site = new CollectionEditor.PropertyGridSite(base.Context, this.propertyGrid1);
					TreeNode[] array = new TreeNode[items.Length];
					for (int i = 0; i < items.Length; i++)
					{
						array[i] = (TreeNode)((TreeNode)items[i]).Clone();
					}
					this.treeView1.Nodes.Clear();
					this.treeView1.Nodes.AddRange(array);
					this.curNode = null;
					this.btnAddChild.Enabled = false;
					this.btnDelete.Enabled = false;
					TreeView treeView = this.TreeView;
					if (treeView != null)
					{
						this.SetImageProps(treeView);
					}
					if (items.Length > 0 && array[0] != null)
					{
						this.treeView1.SelectedNode = array[0];
					}
				}
			}

			// Token: 0x06001C20 RID: 7200 RVA: 0x0009EAA4 File Offset: 0x0009DAA4
			private void PropertyGrid_propertyValueChanged(object sender, PropertyValueChangedEventArgs e)
			{
				this.label2.Text = SR.GetString("CollectionEditorProperties", new object[] { this.treeView1.SelectedNode.Text });
			}

			// Token: 0x06001C21 RID: 7201 RVA: 0x0009EAE4 File Offset: 0x0009DAE4
			private void SetImageProps(TreeView actualTreeView)
			{
				if (actualTreeView.ImageList != null)
				{
					this.treeView1.ImageList = actualTreeView.ImageList;
					this.treeView1.ImageIndex = actualTreeView.ImageIndex;
					this.treeView1.SelectedImageIndex = actualTreeView.SelectedImageIndex;
				}
				else
				{
					this.treeView1.ImageList = null;
					this.treeView1.ImageIndex = -1;
					this.treeView1.SelectedImageIndex = -1;
				}
				if (actualTreeView.StateImageList != null)
				{
					this.treeView1.StateImageList = actualTreeView.StateImageList;
				}
				else
				{
					this.treeView1.StateImageList = null;
				}
				this.treeView1.CheckBoxes = actualTreeView.CheckBoxes;
			}

			// Token: 0x06001C22 RID: 7202 RVA: 0x0009EB8C File Offset: 0x0009DB8C
			private void SetNodeProps(TreeNode node)
			{
				if (node != null)
				{
					this.label2.Text = SR.GetString("CollectionEditorProperties", new object[] { node.Name.ToString() });
				}
				else
				{
					this.label2.Text = SR.GetString("CollectionEditorPropertiesNone");
				}
				this.propertyGrid1.SelectedObject = node;
			}

			// Token: 0x06001C23 RID: 7203 RVA: 0x0009EBEA File Offset: 0x0009DBEA
			private void treeView1_afterSelect(object sender, TreeViewEventArgs e)
			{
				this.curNode = e.Node;
				this.SetNodeProps(this.curNode);
				this.SetButtonsState();
			}

			// Token: 0x06001C24 RID: 7204 RVA: 0x0009EC0C File Offset: 0x0009DC0C
			private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
			{
				TreeNode treeNode = (TreeNode)e.Item;
				base.DoDragDrop(treeNode, DragDropEffects.Move);
			}

			// Token: 0x06001C25 RID: 7205 RVA: 0x0009EC2E File Offset: 0x0009DC2E
			private void treeView1_DragEnter(object sender, DragEventArgs e)
			{
				if (e.Data.GetDataPresent(typeof(TreeNode)))
				{
					e.Effect = DragDropEffects.Move;
					return;
				}
				e.Effect = DragDropEffects.None;
			}

			// Token: 0x06001C26 RID: 7206 RVA: 0x0009EC58 File Offset: 0x0009DC58
			private void treeView1_DragDrop(object sender, DragEventArgs e)
			{
				TreeNode treeNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
				Point point = new Point(0, 0);
				point.X = e.X;
				point.Y = e.Y;
				point = this.treeView1.PointToClient(point);
				TreeNode nodeAt = this.treeView1.GetNodeAt(point);
				if (treeNode != nodeAt)
				{
					this.treeView1.Nodes.Remove(treeNode);
					if (nodeAt != null && !this.CheckParent(nodeAt, treeNode))
					{
						nodeAt.Nodes.Add(treeNode);
						return;
					}
					this.treeView1.Nodes.Add(treeNode);
				}
			}

			// Token: 0x06001C27 RID: 7207 RVA: 0x0009ECFF File Offset: 0x0009DCFF
			private bool CheckParent(TreeNode child, TreeNode parent)
			{
				while (child != null)
				{
					if (parent == child.Parent)
					{
						return true;
					}
					child = child.Parent;
				}
				return false;
			}

			// Token: 0x06001C28 RID: 7208 RVA: 0x0009ED1C File Offset: 0x0009DD1C
			private void treeView1_DragOver(object sender, DragEventArgs e)
			{
				Point point = new Point(0, 0);
				point.X = e.X;
				point.Y = e.Y;
				point = this.treeView1.PointToClient(point);
				TreeNode nodeAt = this.treeView1.GetNodeAt(point);
				this.treeView1.SelectedNode = nodeAt;
			}

			// Token: 0x06001C29 RID: 7209 RVA: 0x0009ED72 File Offset: 0x0009DD72
			private void BtnAddChild_click(object sender, EventArgs e)
			{
				this.Add(this.curNode);
				this.SetButtonsState();
			}

			// Token: 0x06001C2A RID: 7210 RVA: 0x0009ED86 File Offset: 0x0009DD86
			private void BtnAddRoot_click(object sender, EventArgs e)
			{
				this.Add(null);
				this.SetButtonsState();
			}

			// Token: 0x06001C2B RID: 7211 RVA: 0x0009ED95 File Offset: 0x0009DD95
			private void BtnDelete_click(object sender, EventArgs e)
			{
				this.curNode.Remove();
				if (this.treeView1.Nodes.Count == 0)
				{
					this.curNode = null;
					this.SetNodeProps(null);
				}
				this.SetButtonsState();
			}

			// Token: 0x06001C2C RID: 7212 RVA: 0x0009EDC8 File Offset: 0x0009DDC8
			private void BtnOK_click(object sender, EventArgs e)
			{
				object[] array = new object[this.treeView1.Nodes.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.treeView1.Nodes[i].Clone();
				}
				base.Items = array;
				this.treeView1.Dispose();
				this.treeView1 = null;
			}

			// Token: 0x06001C2D RID: 7213 RVA: 0x0009EE2C File Offset: 0x0009DE2C
			private void moveDownButton_Click(object sender, EventArgs e)
			{
				TreeNode treeNode = this.curNode;
				TreeNode parent = this.curNode.Parent;
				if (parent == null)
				{
					this.treeView1.Nodes.RemoveAt(treeNode.Index);
					this.treeView1.Nodes[treeNode.Index].Nodes.Insert(0, treeNode);
				}
				else
				{
					parent.Nodes.RemoveAt(treeNode.Index);
					if (treeNode.Index < parent.Nodes.Count)
					{
						parent.Nodes[treeNode.Index].Nodes.Insert(0, treeNode);
					}
					else if (parent.Parent == null)
					{
						this.treeView1.Nodes.Insert(parent.Index + 1, treeNode);
					}
					else
					{
						parent.Parent.Nodes.Insert(parent.Index + 1, treeNode);
					}
				}
				this.treeView1.SelectedNode = treeNode;
				this.curNode = treeNode;
			}

			// Token: 0x06001C2E RID: 7214 RVA: 0x0009EF1C File Offset: 0x0009DF1C
			private void moveUpButton_Click(object sender, EventArgs e)
			{
				TreeNode treeNode = this.curNode;
				TreeNode parent = this.curNode.Parent;
				if (parent == null)
				{
					this.treeView1.Nodes.RemoveAt(treeNode.Index);
					this.treeView1.Nodes[treeNode.Index - 1].Nodes.Add(treeNode);
				}
				else
				{
					parent.Nodes.RemoveAt(treeNode.Index);
					if (treeNode.Index == 0)
					{
						if (parent.Parent == null)
						{
							this.treeView1.Nodes.Insert(parent.Index, treeNode);
						}
						else
						{
							parent.Parent.Nodes.Insert(parent.Index, treeNode);
						}
					}
					else
					{
						parent.Nodes[treeNode.Index - 1].Nodes.Add(treeNode);
					}
				}
				this.treeView1.SelectedNode = treeNode;
				this.curNode = treeNode;
			}

			// Token: 0x06001C2F RID: 7215 RVA: 0x0009F000 File Offset: 0x0009E000
			private void SetButtonsState()
			{
				bool flag = this.treeView1.Nodes.Count > 0;
				this.btnAddChild.Enabled = flag;
				this.btnDelete.Enabled = flag;
				this.moveDownButton.Enabled = flag && (this.curNode != this.LastNode || this.curNode.Level > 0) && this.curNode != this.treeView1.Nodes[this.treeView1.Nodes.Count - 1];
				this.moveUpButton.Enabled = flag && this.curNode != this.treeView1.Nodes[0];
			}

			// Token: 0x06001C30 RID: 7216 RVA: 0x0009F0C0 File Offset: 0x0009E0C0
			private void TreeNodeCollectionEditor_HelpButtonClicked(object sender, CancelEventArgs e)
			{
				e.Cancel = true;
				this.editor.ShowHelp();
			}

			// Token: 0x06001C31 RID: 7217 RVA: 0x0009F0D4 File Offset: 0x0009E0D4
			private void BtnCancel_click(object sender, EventArgs e)
			{
				if (this.NextNode != this.intialNextNode)
				{
					this.NextNode = this.intialNextNode;
				}
			}

			// Token: 0x040015C4 RID: 5572
			private int nextNode;

			// Token: 0x040015C5 RID: 5573
			private TreeNode curNode;

			// Token: 0x040015C6 RID: 5574
			private TreeNodeCollectionEditor editor;

			// Token: 0x040015C7 RID: 5575
			private Button okButton;

			// Token: 0x040015C8 RID: 5576
			private Button btnCancel;

			// Token: 0x040015C9 RID: 5577
			private Button btnAddChild;

			// Token: 0x040015CA RID: 5578
			private Button btnAddRoot;

			// Token: 0x040015CB RID: 5579
			private Button btnDelete;

			// Token: 0x040015CC RID: 5580
			private Button moveDownButton;

			// Token: 0x040015CD RID: 5581
			private Button moveUpButton;

			// Token: 0x040015CE RID: 5582
			private Label label1;

			// Token: 0x040015CF RID: 5583
			private TreeView treeView1;

			// Token: 0x040015D0 RID: 5584
			private Label label2;

			// Token: 0x040015D1 RID: 5585
			private VsPropertyGrid propertyGrid1;

			// Token: 0x040015D2 RID: 5586
			private TableLayoutPanel okCancelPanel;

			// Token: 0x040015D3 RID: 5587
			private TableLayoutPanel nodeControlPanel;

			// Token: 0x040015D4 RID: 5588
			private TableLayoutPanel overarchingTableLayoutPanel;

			// Token: 0x040015D5 RID: 5589
			private TableLayoutPanel navigationButtonsTableLayoutPanel;

			// Token: 0x040015D6 RID: 5590
			private static object NextNodeKey = new object();

			// Token: 0x040015D7 RID: 5591
			private int intialNextNode;
		}
	}
}
