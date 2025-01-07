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
	internal partial class MenuBindingsEditorForm : DesignerForm
	{
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

		private void ApplyBindings()
		{
			ControlDesigner.InvokeTransactedChange(this._menu, new TransactedChangeCallback(this.ApplyBindingsChangeCallback), null, SR.GetString("MenuDesigner_EditBindingsTransactionDescription"));
		}

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

		private IDataSourceViewSchema FindViewSchema(string viewName, int level)
		{
			return TreeViewBindingsEditorForm.FindViewSchemaRecursive(this.Schema, 0, viewName, level, null);
		}

		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.Menu.BindingsEditorForm";
			}
		}

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

		private void OnApplyButtonClick(object sender, EventArgs e)
		{
			this.ApplyBindings();
			this._applyButton.Enabled = false;
		}

		private void OnAddBindingButtonClick(object sender, EventArgs e)
		{
			this._applyButton.Enabled = true;
			this.AddBinding();
		}

		private void OnBindingsListViewGotFocus(object sender, EventArgs e)
		{
			this.UpdateSelectedBinding();
		}

		private void OnBindingsListViewSelectedIndexChanged(object sender, EventArgs e)
		{
			this.UpdateSelectedBinding();
		}

		private void OnCancelButtonClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

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

		private void OnSchemaTreeViewAfterSelect(object sender, TreeViewEventArgs e)
		{
			this.UpdateEnabledStates();
		}

		private void OnSchemaTreeViewGotFocus(object sender, EventArgs e)
		{
			this._propertyGrid.SelectedObject = null;
		}

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

		private IDataSourceSchema _schema;

		private class SchemaTreeNode : global::System.Windows.Forms.TreeNode
		{
			public SchemaTreeNode(IDataSourceViewSchema schema)
				: base(schema.Name)
			{
				this._schema = schema;
			}

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

			public object Schema
			{
				get
				{
					return this._schema;
				}
			}

			private IDataSourceViewSchema _schema;

			private bool _duplicate;
		}
	}
}
