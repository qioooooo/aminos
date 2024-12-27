using System;
using System.Design;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020004EE RID: 1262
	internal sealed partial class TreeNodeCollectionEditorDialog : CollectionEditorDialog
	{
		// Token: 0x06002D1B RID: 11547 RVA: 0x000FE9B0 File Offset: 0x000FD9B0
		public TreeNodeCollectionEditorDialog(global::System.Web.UI.WebControls.TreeView treeView, TreeViewDesigner treeViewDesigner)
		{
			/*
An exception occurred when decompiling this method (06002D1B)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Web.UI.Design.WebControls.TreeNodeCollectionEditorDialog::.ctor(System.Web.UI.WebControls.TreeView,System.Web.UI.Design.WebControls.TreeViewDesigner)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILExpression..ctor(ILCode code, Object operand) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstTypes.cs:line 626
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.ConvertToAst(List`1 body) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 1010
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.ConvertToAst(List`1 body, HashSet`1 ehs) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 959
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 280
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 117
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06002D1C RID: 11548 RVA: 0x000FF189 File Offset: 0x000FE189
		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.TreeView.CollectionEditor";
			}
		}

		// Token: 0x06002D1D RID: 11549 RVA: 0x000FF190 File Offset: 0x000FE190
		private void LoadNodes(global::System.Windows.Forms.TreeNodeCollection destNodes, global::System.Web.UI.WebControls.TreeNodeCollection sourceNodes)
		{
			foreach (object obj in sourceNodes)
			{
				global::System.Web.UI.WebControls.TreeNode treeNode = (global::System.Web.UI.WebControls.TreeNode)obj;
				TreeNodeCollectionEditorDialog.TreeNodeContainer treeNodeContainer = new TreeNodeCollectionEditorDialog.TreeNodeContainer();
				destNodes.Add(treeNodeContainer);
				treeNodeContainer.Text = treeNode.Text;
				global::System.Web.UI.WebControls.TreeNode treeNode2 = (global::System.Web.UI.WebControls.TreeNode)((ICloneable)treeNode).Clone();
				this._treeViewDesigner.RegisterClone(treeNode, treeNode2);
				treeNodeContainer.WebTreeNode = treeNode2;
				if (treeNode.ChildNodes.Count > 0)
				{
					this.LoadNodes(treeNodeContainer.Nodes, treeNode.ChildNodes);
				}
			}
		}

		// Token: 0x06002D1E RID: 11550 RVA: 0x000FF23C File Offset: 0x000FE23C
		private void OnAddRootButtonClick()
		{
			/*
An exception occurred when decompiling this method (06002D1E)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Web.UI.Design.WebControls.TreeNodeCollectionEditorDialog::OnAddRootButtonClick()

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.FindLoops(HashSet`1 scope, ControlFlowNode entryPoint, Boolean excludeEntryPoint) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 137
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.FindLoops(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 57
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 343
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x06002D1F RID: 11551 RVA: 0x000FF28C File Offset: 0x000FE28C
		private void OnAddChildButtonClick()
		{
			global::System.Windows.Forms.TreeNode selectedNode = this._treeView.SelectedNode;
			if (selectedNode != null)
			{
				TreeNodeCollectionEditorDialog.TreeNodeContainer treeNodeContainer = new TreeNodeCollectionEditorDialog.TreeNodeContainer();
				selectedNode.Nodes.Add(treeNodeContainer);
				string @string = SR.GetString("TreeNodeCollectionEditor_NewNodeText");
				treeNodeContainer.Text = @string;
				treeNodeContainer.WebTreeNode.Text = @string;
				if (!selectedNode.IsExpanded)
				{
					selectedNode.Expand();
				}
				this._treeView.SelectedNode = treeNodeContainer;
			}
		}

		// Token: 0x06002D20 RID: 11552 RVA: 0x000FF2F3 File Offset: 0x000FE2F3
		private void OnCancelButtonClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x06002D21 RID: 11553 RVA: 0x000FF304 File Offset: 0x000FE304
		private void OnIndentButtonClick()
		{
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

		// Token: 0x06002D22 RID: 11554 RVA: 0x000FF34C File Offset: 0x000FE34C
		private void OnMoveDownButtonClick()
		{
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

		// Token: 0x06002D23 RID: 11555 RVA: 0x000FF3B4 File Offset: 0x000FE3B4
		private void OnMoveUpButtonClick()
		{
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

		// Token: 0x06002D24 RID: 11556 RVA: 0x000FF419 File Offset: 0x000FE419
		private void OnOkButtonClick(object sender, EventArgs e)
		{
			this.SaveNodes(this._webTreeView.Nodes, this._treeView.Nodes);
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x06002D25 RID: 11557 RVA: 0x000FF444 File Offset: 0x000FE444
		private void OnPropertyGridPropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
		{
			TreeNodeCollectionEditorDialog.TreeNodeContainer treeNodeContainer = (TreeNodeCollectionEditorDialog.TreeNodeContainer)this._treeView.SelectedNode;
			if (treeNodeContainer != null)
			{
				treeNodeContainer.Text = treeNodeContainer.WebTreeNode.Text;
			}
			this._propertyGrid.Refresh();
		}

		// Token: 0x06002D26 RID: 11558 RVA: 0x000FF484 File Offset: 0x000FE484
		private void OnRemoveButtonClick()
		{
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

		// Token: 0x06002D27 RID: 11559 RVA: 0x000FF534 File Offset: 0x000FE534
		protected override void OnInitialActivated(EventArgs e)
		{
			base.OnInitialActivated(e);
			this.LoadNodes(this._treeView.Nodes, this._webTreeView.Nodes);
			if (this._treeView.Nodes.Count > 0)
			{
				this._treeView.SelectedNode = this._treeView.Nodes[0];
			}
			this.UpdateEnabledState();
		}

		// Token: 0x06002D28 RID: 11560 RVA: 0x000FF599 File Offset: 0x000FE599
		private void OnTreeViewAfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node != null)
			{
				this._propertyGrid.SelectedObject = ((TreeNodeCollectionEditorDialog.TreeNodeContainer)e.Node).WebTreeNode;
			}
			else
			{
				this._propertyGrid.SelectedObject = null;
			}
			this.UpdateEnabledState();
		}

		// Token: 0x06002D29 RID: 11561 RVA: 0x000FF5D4 File Offset: 0x000FE5D4
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

		// Token: 0x06002D2A RID: 11562 RVA: 0x000FF680 File Offset: 0x000FE680
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

		// Token: 0x06002D2B RID: 11563 RVA: 0x000FF720 File Offset: 0x000FE720
		private void OnUnindentButtonClick()
		{
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

		// Token: 0x06002D2C RID: 11564 RVA: 0x000FF78C File Offset: 0x000FE78C
		private void SaveNodes(global::System.Web.UI.WebControls.TreeNodeCollection destNodes, global::System.Windows.Forms.TreeNodeCollection sourceNodes)
		{
			destNodes.Clear();
			foreach (object obj in sourceNodes)
			{
				TreeNodeCollectionEditorDialog.TreeNodeContainer treeNodeContainer = (TreeNodeCollectionEditorDialog.TreeNodeContainer)obj;
				global::System.Web.UI.WebControls.TreeNode webTreeNode = treeNodeContainer.WebTreeNode;
				destNodes.Add(webTreeNode);
				if (treeNodeContainer.Nodes.Count > 0)
				{
					this.SaveNodes(webTreeNode.ChildNodes, treeNodeContainer.Nodes);
				}
			}
		}

		// Token: 0x06002D2D RID: 11565 RVA: 0x000FF810 File Offset: 0x000FE810
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

		// Token: 0x04001EB4 RID: 7860
		private global::System.Windows.Forms.Panel _treeViewPanel;

		// Token: 0x04001EB5 RID: 7861
		private global::System.Windows.Forms.TreeView _treeView;

		// Token: 0x04001EB6 RID: 7862
		private PropertyGrid _propertyGrid;

		// Token: 0x04001EB7 RID: 7863
		private global::System.Windows.Forms.Button _okButton;

		// Token: 0x04001EB8 RID: 7864
		private global::System.Windows.Forms.Button _cancelButton;

		// Token: 0x04001EB9 RID: 7865
		private global::System.Windows.Forms.Label _propertiesLabel;

		// Token: 0x04001EBA RID: 7866
		private global::System.Windows.Forms.Label _nodesLabel;

		// Token: 0x04001EBB RID: 7867
		private ToolStripButton _addRootButton;

		// Token: 0x04001EBC RID: 7868
		private ToolStripButton _addChildButton;

		// Token: 0x04001EBD RID: 7869
		private ToolStripButton _removeButton;

		// Token: 0x04001EBE RID: 7870
		private ToolStripButton _moveUpButton;

		// Token: 0x04001EBF RID: 7871
		private ToolStripButton _moveDownButton;

		// Token: 0x04001EC0 RID: 7872
		private ToolStripButton _indentButton;

		// Token: 0x04001EC1 RID: 7873
		private ToolStripButton _unindentButton;

		// Token: 0x04001EC2 RID: 7874
		private ToolStripSeparator _toolBarSeparator;

		// Token: 0x04001EC3 RID: 7875
		private ToolStrip _treeViewToolBar;

		// Token: 0x04001EC4 RID: 7876
		private global::System.Web.UI.WebControls.TreeView _webTreeView;

		// Token: 0x04001EC5 RID: 7877
		private TreeViewDesigner _treeViewDesigner;

		// Token: 0x020004EF RID: 1263
		private class TreeNodeContainer : global::System.Windows.Forms.TreeNode
		{
			// Token: 0x1700087D RID: 2173
			// (get) Token: 0x06002D2E RID: 11566 RVA: 0x000FF8E9 File Offset: 0x000FE8E9
			// (set) Token: 0x06002D2F RID: 11567 RVA: 0x000FF904 File Offset: 0x000FE904
			public global::System.Web.UI.WebControls.TreeNode WebTreeNode
			{
				get
				{
					if (this._webTreeNode == null)
					{
						this._webTreeNode = new global::System.Web.UI.WebControls.TreeNode();
					}
					return this._webTreeNode;
				}
				set
				{
					this._webTreeNode = value;
				}
			}

			// Token: 0x04001EC6 RID: 7878
			private global::System.Web.UI.WebControls.TreeNode _webTreeNode;
		}
	}
}
