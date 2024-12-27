using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000474 RID: 1140
	internal partial class MenuBindingsEditorForm : DesignerForm
	{
		// Token: 0x06002950 RID: 10576 RVA: 0x000E2254 File Offset: 0x000E1254
		public MenuBindingsEditorForm(IServiceProvider serviceProvider, global::System.Web.UI.WebControls.Menu menu, MenuDesigner menuDesigner)
			: base(serviceProvider)
		{
			this._menu = menu;
			this.InitializeComponent();
			this.InitializeUI();
			foreach (object obj in this._menu.DataBindings)
			{
				MenuItemBinding menuItemBinding = (MenuItemBinding)obj;
				MenuItemBinding menuItemBinding2 = (MenuItemBinding)((ICloneable)menuItemBinding).Clone();
				menuDesigner.RegisterClone(menuItemBinding, menuItemBinding2);
				this._bindingsListView.Items.Add(menuItemBinding2);
			}
		}

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x06002951 RID: 10577 RVA: 0x000E22EC File Offset: 0x000E12EC
		private IDataSourceSchema Schema
		{
			get
			{
				if (this._schema == null)
				{
					IDesignerHost designerHost = (IDesignerHost)base.ServiceProvider.GetService(typeof(IDesignerHost));
					if (designerHost != null)
					{
						HierarchicalDataBoundControlDesigner hierarchicalDataBoundControlDesigner = designerHost.GetDesigner(this._menu) as HierarchicalDataBoundControlDesigner;
						if (hierarchicalDataBoundControlDesigner != null)
						{
							DesignerHierarchicalDataSourceView designerView = hierarchicalDataBoundControlDesigner.DesignerView;
							if (designerView != null)
							{
								try
								{
									this._schema = designerView.Schema;
								}
								catch (Exception ex)
								{
									IComponentDesignerDebugService componentDesignerDebugService = (IComponentDesignerDebugService)base.ServiceProvider.GetService(typeof(IComponentDesignerDebugService));
									if (componentDesignerDebugService != null)
									{
										componentDesignerDebugService.Fail(SR.GetString("DataSource_DebugService_FailedCall", new object[] { "DesignerHierarchicalDataSourceView.Schema", ex.Message }));
									}
								}
							}
						}
					}
				}
				return this._schema;
			}
		}

		// Token: 0x06002952 RID: 10578 RVA: 0x000E23B8 File Offset: 0x000E13B8
		private void AddBinding()
		{
			global::System.Windows.Forms.TreeNode selectedNode = this._schemaTreeView.SelectedNode;
			if (selectedNode != null)
			{
				MenuItemBinding menuItemBinding = new MenuItemBinding();
				if (selectedNode.Text != this._schemaTreeView.Nodes[0].Text)
				{
					menuItemBinding.DataMember = selectedNode.Text;
					if (((MenuBindingsEditorForm.SchemaTreeNode)selectedNode).Duplicate)
					{
						menuItemBinding.Depth = selectedNode.FullPath.Split(new char[] { this._schemaTreeView.PathSeparator[0] }).Length - 1;
					}
					((IDataSourceViewSchemaAccessor)menuItemBinding).DataSourceViewSchema = ((MenuBindingsEditorForm.SchemaTreeNode)selectedNode).Schema;
					int num = this._bindingsListView.Items.IndexOf(menuItemBinding);
					if (num == -1)
					{
						this._bindingsListView.Items.Add(menuItemBinding);
						this._bindingsListView.SetSelected(this._bindingsListView.Items.Count - 1, true);
					}
					else
					{
						menuItemBinding = (MenuItemBinding)this._bindingsListView.Items[num];
						this._bindingsListView.SetSelected(num, true);
					}
				}
				else
				{
					this._bindingsListView.Items.Add(menuItemBinding);
					this._bindingsListView.SetSelected(this._bindingsListView.Items.Count - 1, true);
				}
				this._propertyGrid.SelectedObject = menuItemBinding;
				this._propertyGrid.Refresh();
				this.UpdateEnabledStates();
			}
			this._bindingsListView.Focus();
		}

		// Token: 0x06002953 RID: 10579 RVA: 0x000E2525 File Offset: 0x000E1525
		private void ApplyBindings()
		{
			ControlDesigner.InvokeTransactedChange(this._menu, new TransactedChangeCallback(this.ApplyBindingsChangeCallback), null, SR.GetString("MenuDesigner_EditBindingsTransactionDescription"));
		}

		// Token: 0x06002954 RID: 10580 RVA: 0x000E254C File Offset: 0x000E154C
		private bool ApplyBindingsChangeCallback(object context)
		{
			this._menu.DataBindings.Clear();
			foreach (object obj in this._bindingsListView.Items)
			{
				MenuItemBinding menuItemBinding = (MenuItemBinding)obj;
				this._menu.DataBindings.Add(menuItemBinding);
			}
			return true;
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x000E25E7 File Offset: 0x000E15E7
		private IDataSourceViewSchema FindViewSchema(string viewName, int level)
		{
			return TreeViewBindingsEditorForm.FindViewSchemaRecursive(this.Schema, 0, viewName, level, null);
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x06002957 RID: 10583 RVA: 0x000E25F8 File Offset: 0x000E15F8
		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.Menu.BindingsEditorForm";
			}
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x000E2E2C File Offset: 0x000E1E2C
		private void InitializeUI()
		{
			this._bindingsLabel.Text = SR.GetString("MenuBindingsEditor_Bindings");
			this._schemaLabel.Text = SR.GetString("MenuBindingsEditor_Schema");
			this._okButton.Text = SR.GetString("MenuBindingsEditor_OK");
			this._applyButton.Text = SR.GetString("MenuBindingsEditor_Apply");
			this._cancelButton.Text = SR.GetString("MenuBindingsEditor_Cancel");
			this._propertiesLabel.Text = SR.GetString("MenuBindingsEditor_BindingProperties");
			this._addBindingButton.Text = SR.GetString("MenuBindingsEditor_AddBinding");
			this.Text = SR.GetString("MenuBindingsEditor_Title");
			Bitmap bitmap = new Icon(typeof(MenuBindingsEditorForm), "SortUp.ico").ToBitmap();
			bitmap.MakeTransparent();
			this._moveBindingUpButton.Image = bitmap;
			this._moveBindingUpButton.AccessibleName = SR.GetString("MenuBindingsEditor_MoveBindingUpName");
			this._moveBindingUpButton.AccessibleDescription = SR.GetString("MenuBindingsEditor_MoveBindingUpDescription");
			Bitmap bitmap2 = new Icon(typeof(MenuBindingsEditorForm), "SortDown.ico").ToBitmap();
			bitmap2.MakeTransparent();
			this._moveBindingDownButton.Image = bitmap2;
			this._moveBindingDownButton.AccessibleName = SR.GetString("MenuBindingsEditor_MoveBindingDownName");
			this._moveBindingDownButton.AccessibleDescription = SR.GetString("MenuBindingsEditor_MoveBindingDownDescription");
			Bitmap bitmap3 = new Icon(typeof(MenuBindingsEditorForm), "Delete.ico").ToBitmap();
			bitmap3.MakeTransparent();
			this._deleteBindingButton.Image = bitmap3;
			this._deleteBindingButton.AccessibleName = SR.GetString("MenuBindingsEditor_DeleteBindingName");
			this._deleteBindingButton.AccessibleDescription = SR.GetString("MenuBindingsEditor_DeleteBindingDescription");
			base.Icon = null;
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x000E2FE5 File Offset: 0x000E1FE5
		private void OnApplyButtonClick(object sender, EventArgs e)
		{
			this.ApplyBindings();
			this._applyButton.Enabled = false;
		}

		// Token: 0x0600295B RID: 10587 RVA: 0x000E2FF9 File Offset: 0x000E1FF9
		private void OnAddBindingButtonClick(object sender, EventArgs e)
		{
			this._applyButton.Enabled = true;
			this.AddBinding();
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x000E300D File Offset: 0x000E200D
		private void OnBindingsListViewGotFocus(object sender, EventArgs e)
		{
			this.UpdateSelectedBinding();
		}

		// Token: 0x0600295D RID: 10589 RVA: 0x000E3015 File Offset: 0x000E2015
		private void OnBindingsListViewSelectedIndexChanged(object sender, EventArgs e)
		{
			this.UpdateSelectedBinding();
		}

		// Token: 0x0600295E RID: 10590 RVA: 0x000E301D File Offset: 0x000E201D
		private void OnCancelButtonClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x0600295F RID: 10591 RVA: 0x000E302C File Offset: 0x000E202C
		private void OnDeleteBindingButtonClick(object sender, EventArgs e)
		{
			if (this._bindingsListView.SelectedIndices.Count > 0)
			{
				this._applyButton.Enabled = true;
				int num = this._bindingsListView.SelectedIndices[0];
				this._bindingsListView.Items.RemoveAt(num);
				if (num >= this._bindingsListView.Items.Count)
				{
					num--;
				}
				if (num >= 0 && this._bindingsListView.Items.Count > 0)
				{
					this._bindingsListView.SetSelected(num, true);
				}
			}
		}

		// Token: 0x06002960 RID: 10592 RVA: 0x000E30B8 File Offset: 0x000E20B8
		protected override void OnInitialActivated(EventArgs e)
		{
			base.OnInitialActivated(e);
			global::System.Windows.Forms.TreeNode treeNode = this._schemaTreeView.Nodes.Add(SR.GetString("MenuBindingsEditor_EmptyBindingText"));
			if (this.Schema != null)
			{
				this.PopulateSchema(this.Schema);
				this._schemaTreeView.ExpandAll();
			}
			this._schemaTreeView.SelectedNode = treeNode;
			this.UpdateEnabledStates();
		}

		// Token: 0x06002961 RID: 10593 RVA: 0x000E3118 File Offset: 0x000E2118
		private void OnMoveBindingUpButtonClick(object sender, EventArgs e)
		{
			if (this._bindingsListView.SelectedIndices.Count > 0)
			{
				this._applyButton.Enabled = true;
				int num = this._bindingsListView.SelectedIndices[0];
				if (num > 0)
				{
					MenuItemBinding menuItemBinding = (MenuItemBinding)this._bindingsListView.Items[num];
					this._bindingsListView.Items.RemoveAt(num);
					this._bindingsListView.Items.Insert(num - 1, menuItemBinding);
					this._bindingsListView.SetSelected(num - 1, true);
				}
			}
		}

		// Token: 0x06002962 RID: 10594 RVA: 0x000E31A8 File Offset: 0x000E21A8
		private void OnMoveBindingDownButtonClick(object sender, EventArgs e)
		{
			if (this._bindingsListView.SelectedIndices.Count > 0)
			{
				this._applyButton.Enabled = true;
				int num = this._bindingsListView.SelectedIndices[0];
				if (num + 1 < this._bindingsListView.Items.Count)
				{
					MenuItemBinding menuItemBinding = (MenuItemBinding)this._bindingsListView.Items[num];
					this._bindingsListView.Items.RemoveAt(num);
					this._bindingsListView.Items.Insert(num + 1, menuItemBinding);
					this._bindingsListView.SetSelected(num + 1, true);
				}
			}
		}

		// Token: 0x06002963 RID: 10595 RVA: 0x000E3248 File Offset: 0x000E2248
		private void OnOKButtonClick(object sender, EventArgs e)
		{
			try
			{
				this.ApplyBindings();
			}
			finally
			{
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
		}

		// Token: 0x06002964 RID: 10596 RVA: 0x000E327C File Offset: 0x000E227C
		private void OnPropertyGridPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			this._applyButton.Enabled = true;
			if (e.ChangedItem.PropertyDescriptor.Name == "DataMember")
			{
				string text = (string)e.ChangedItem.Value;
				MenuItemBinding menuItemBinding = (MenuItemBinding)this._bindingsListView.Items[this._bindingsListView.SelectedIndex];
				this._bindingsListView.Items[this._bindingsListView.SelectedIndex] = menuItemBinding;
				this._bindingsListView.Refresh();
				IDataSourceViewSchema dataSourceViewSchema = this.FindViewSchema(text, menuItemBinding.Depth);
				if (dataSourceViewSchema != null)
				{
					((IDataSourceViewSchemaAccessor)menuItemBinding).DataSourceViewSchema = dataSourceViewSchema;
				}
				this._propertyGrid.SelectedObject = menuItemBinding;
				this._propertyGrid.Refresh();
				return;
			}
			if (e.ChangedItem.PropertyDescriptor.Name == "Depth")
			{
				int num = (int)e.ChangedItem.Value;
				MenuItemBinding menuItemBinding2 = (MenuItemBinding)this._bindingsListView.Items[this._bindingsListView.SelectedIndex];
				IDataSourceViewSchema dataSourceViewSchema2 = this.FindViewSchema(menuItemBinding2.DataMember, num);
				if (dataSourceViewSchema2 != null)
				{
					((IDataSourceViewSchemaAccessor)menuItemBinding2).DataSourceViewSchema = dataSourceViewSchema2;
				}
				this._propertyGrid.SelectedObject = menuItemBinding2;
				this._propertyGrid.Refresh();
			}
		}

		// Token: 0x06002965 RID: 10597 RVA: 0x000E33C1 File Offset: 0x000E23C1
		private void OnSchemaTreeViewAfterSelect(object sender, TreeViewEventArgs e)
		{
			this.UpdateEnabledStates();
		}

		// Token: 0x06002966 RID: 10598 RVA: 0x000E33C9 File Offset: 0x000E23C9
		private void OnSchemaTreeViewGotFocus(object sender, EventArgs e)
		{
			this._propertyGrid.SelectedObject = null;
		}

		// Token: 0x06002967 RID: 10599 RVA: 0x000E33D8 File Offset: 0x000E23D8
		private void PopulateSchema(IDataSourceSchema schema)
		{
			if (schema == null)
			{
				return;
			}
			IDictionary dictionary = new Hashtable();
			IDataSourceViewSchema[] views = schema.GetViews();
			if (views != null)
			{
				for (int i = 0; i < views.Length; i++)
				{
					this.PopulateSchemaRecursive(this._schemaTreeView.Nodes, views[i], 0, dictionary);
				}
			}
		}

		// Token: 0x06002968 RID: 10600 RVA: 0x000E3420 File Offset: 0x000E2420
		private void PopulateSchemaRecursive(global::System.Windows.Forms.TreeNodeCollection nodes, IDataSourceViewSchema viewSchema, int depth, IDictionary duplicates)
		{
			if (viewSchema == null)
			{
				return;
			}
			MenuBindingsEditorForm.SchemaTreeNode schemaTreeNode = new MenuBindingsEditorForm.SchemaTreeNode(viewSchema);
			nodes.Add(schemaTreeNode);
			MenuBindingsEditorForm.SchemaTreeNode schemaTreeNode2 = (MenuBindingsEditorForm.SchemaTreeNode)duplicates[viewSchema.Name];
			if (schemaTreeNode2 != null)
			{
				schemaTreeNode2.Duplicate = true;
				schemaTreeNode.Duplicate = true;
			}
			foreach (object obj in this._bindingsListView.Items)
			{
				MenuItemBinding menuItemBinding = (MenuItemBinding)obj;
				if (string.Compare(menuItemBinding.DataMember, viewSchema.Name, StringComparison.OrdinalIgnoreCase) == 0)
				{
					IDataSourceViewSchemaAccessor dataSourceViewSchemaAccessor = menuItemBinding;
					if (depth == menuItemBinding.Depth || dataSourceViewSchemaAccessor.DataSourceViewSchema == null)
					{
						dataSourceViewSchemaAccessor.DataSourceViewSchema = viewSchema;
					}
				}
			}
			IDataSourceViewSchema[] children = viewSchema.GetChildren();
			if (children != null)
			{
				for (int i = 0; i < children.Length; i++)
				{
					this.PopulateSchemaRecursive(schemaTreeNode.Nodes, children[i], depth + 1, duplicates);
				}
			}
		}

		// Token: 0x06002969 RID: 10601 RVA: 0x000E3518 File Offset: 0x000E2518
		private void UpdateEnabledStates()
		{
			if (this._bindingsListView.SelectedIndices.Count > 0)
			{
				int num = this._bindingsListView.SelectedIndices[0];
				this._moveBindingDownButton.Enabled = num + 1 < this._bindingsListView.Items.Count;
				this._moveBindingUpButton.Enabled = num > 0;
				this._deleteBindingButton.Enabled = true;
			}
			else
			{
				this._moveBindingDownButton.Enabled = false;
				this._moveBindingUpButton.Enabled = false;
				this._deleteBindingButton.Enabled = false;
			}
			this._addBindingButton.Enabled = this._schemaTreeView.SelectedNode != null;
		}

		// Token: 0x0600296A RID: 10602 RVA: 0x000E35C8 File Offset: 0x000E25C8
		private void UpdateSelectedBinding()
		{
			MenuItemBinding menuItemBinding = null;
			if (this._bindingsListView.SelectedItems.Count > 0)
			{
				MenuItemBinding menuItemBinding2 = (MenuItemBinding)this._bindingsListView.SelectedItems[0];
				menuItemBinding = menuItemBinding2;
			}
			this._propertyGrid.SelectedObject = menuItemBinding;
			this._propertyGrid.Refresh();
			this.UpdateEnabledStates();
		}

		// Token: 0x04001C79 RID: 7289
		private IDataSourceSchema _schema;

		// Token: 0x02000475 RID: 1141
		private class SchemaTreeNode : global::System.Windows.Forms.TreeNode
		{
			// Token: 0x0600296B RID: 10603 RVA: 0x000E3620 File Offset: 0x000E2620
			public SchemaTreeNode(IDataSourceViewSchema schema)
				: base(schema.Name)
			{
				this._schema = schema;
			}

			// Token: 0x170007B1 RID: 1969
			// (get) Token: 0x0600296C RID: 10604 RVA: 0x000E3635 File Offset: 0x000E2635
			// (set) Token: 0x0600296D RID: 10605 RVA: 0x000E363D File Offset: 0x000E263D
			public bool Duplicate
			{
				get
				{
					return this._duplicate;
				}
				set
				{
					this._duplicate = value;
				}
			}

			// Token: 0x170007B2 RID: 1970
			// (get) Token: 0x0600296E RID: 10606 RVA: 0x000E3646 File Offset: 0x000E2646
			public object Schema
			{
				get
				{
					return this._schema;
				}
			}

			// Token: 0x04001C7A RID: 7290
			private IDataSourceViewSchema _schema;

			// Token: 0x04001C7B RID: 7291
			private bool _duplicate;
		}
	}
}
