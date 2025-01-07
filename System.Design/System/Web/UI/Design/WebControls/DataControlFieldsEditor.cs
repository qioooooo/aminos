using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;
using System.Text;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed partial class DataControlFieldsEditor : DesignerForm
	{
		public DataControlFieldsEditor(DataBoundControlDesigner controlDesigner)
			: base(controlDesigner.Component.Site)
		{
			this._controlDesigner = controlDesigner;
			this.InitializeComponent();
			this.InitForm();
			this._initialActivate = true;
			this.IgnoreRefreshSchemaEvents();
		}

		private bool AutoGenerateFields
		{
			get
			{
				if (this.Control is GridView)
				{
					return ((GridView)this.Control).AutoGenerateColumns;
				}
				return this.Control is DetailsView && ((DetailsView)this.Control).AutoGenerateRows;
			}
			set
			{
				if (this.Control is GridView)
				{
					((GridView)this.Control).AutoGenerateColumns = value;
					return;
				}
				if (this.Control is DetailsView)
				{
					((DetailsView)this.Control).AutoGenerateRows = value;
				}
			}
		}

		private DataBoundControl Control
		{
			get
			{
				return this._controlDesigner.Component as DataBoundControl;
			}
		}

		private DataControlFieldCollection FieldCollection
		{
			get
			{
				if (this._clonedFieldCollection == null)
				{
					if (this.Control is GridView)
					{
						DataControlFieldCollection columns = ((GridView)this.Control).Columns;
						this._clonedFieldCollection = columns.CloneFields();
						for (int i = 0; i < columns.Count; i++)
						{
							this._controlDesigner.RegisterClone(columns[i], this._clonedFieldCollection[i]);
						}
					}
					else if (this.Control is DetailsView)
					{
						DataControlFieldCollection fields = ((DetailsView)this.Control).Fields;
						this._clonedFieldCollection = fields.CloneFields();
						for (int j = 0; j < fields.Count; j++)
						{
							this._controlDesigner.RegisterClone(fields[j], this._clonedFieldCollection[j]);
						}
					}
				}
				return this._clonedFieldCollection;
			}
		}

		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.DataControlField.DataControlFieldEditor";
			}
		}

		private bool IgnoreRefreshSchema
		{
			get
			{
				if (this._controlDesigner is GridViewDesigner)
				{
					return ((GridViewDesigner)this._controlDesigner)._ignoreSchemaRefreshedEvent;
				}
				return this._controlDesigner is DetailsViewDesigner && ((DetailsViewDesigner)this._controlDesigner)._ignoreSchemaRefreshedEvent;
			}
			set
			{
				if (this._controlDesigner is GridViewDesigner)
				{
					((GridViewDesigner)this._controlDesigner)._ignoreSchemaRefreshedEvent = value;
				}
				if (this._controlDesigner is DetailsViewDesigner)
				{
					((DetailsViewDesigner)this._controlDesigner)._ignoreSchemaRefreshedEvent = value;
				}
			}
		}

		private void EnterLoadingMode()
		{
			this._isLoading = true;
		}

		private void ExitLoadingMode()
		{
			this._isLoading = false;
		}

		internal string GetNewDataSourceName(Type controlType, DataBoundControlMode mode)
		{
			if (mode == DataBoundControlMode.Edit)
			{
				return this.GetNewDataSourceName(controlType, 1);
			}
			if (mode == DataBoundControlMode.Insert)
			{
				return this.GetNewDataSourceName(controlType, 2);
			}
			return this.GetNewDataSourceName(controlType, 0);
		}

		private string GetNewDataSourceName(Type controlType, int editMode)
		{
			int num = 1;
			return this.GetNewDataSourceName(controlType, editMode, ref num);
		}

		private string GetNewDataSourceName(Type controlType, int editMode, ref int startIndex)
		{
			int num = startIndex;
			DataControlFieldCollection dataControlFieldCollection = new DataControlFieldCollection();
			int count = this._selFieldsList.Items.Count;
			for (int i = 0; i < count; i++)
			{
				DataControlFieldsEditor.FieldItem fieldItem = (DataControlFieldsEditor.FieldItem)this._selFieldsList.Items[i];
				dataControlFieldCollection.Add(fieldItem.RuntimeField);
			}
			if (dataControlFieldCollection != null && dataControlFieldCollection.Count > 0)
			{
				bool flag = false;
				while (!flag)
				{
					for (int j = 0; j < dataControlFieldCollection.Count; j++)
					{
						DataControlField dataControlField = dataControlFieldCollection[j];
						if (dataControlField is TemplateField)
						{
							ITemplate template = null;
							switch (editMode)
							{
							case 0:
								template = ((TemplateField)dataControlField).ItemTemplate;
								break;
							case 1:
								template = ((TemplateField)dataControlField).EditItemTemplate;
								break;
							case 2:
								template = ((TemplateField)dataControlField).InsertItemTemplate;
								break;
							}
							if (template != null)
							{
								IDesignerHost designerHost = (IDesignerHost)this.Control.Site.GetService(typeof(IDesignerHost));
								string text = ControlSerializer.SerializeTemplate(template, designerHost);
								if (text.Contains(controlType.Name + num.ToString(NumberFormatInfo.InvariantInfo)))
								{
									num++;
									break;
								}
							}
						}
						if (j == dataControlFieldCollection.Count - 1)
						{
							flag = true;
						}
					}
				}
			}
			startIndex = num;
			return controlType.Name + num.ToString(NumberFormatInfo.InvariantInfo);
		}

		private IDataSourceViewSchema GetViewSchema()
		{
			if (this._viewSchema == null && this._controlDesigner != null)
			{
				DesignerDataSourceView designerView = this._controlDesigner.DesignerView;
				if (designerView != null)
				{
					try
					{
						this._viewSchema = designerView.Schema;
					}
					catch (Exception ex)
					{
						IComponentDesignerDebugService componentDesignerDebugService = (IComponentDesignerDebugService)base.ServiceProvider.GetService(typeof(IComponentDesignerDebugService));
						if (componentDesignerDebugService != null)
						{
							componentDesignerDebugService.Fail(SR.GetString("DataSource_DebugService_FailedCall", new object[] { "DesignerDataSourceView.Schema", ex.Message }));
						}
					}
				}
			}
			return this._viewSchema;
		}

		private IDataSourceFieldSchema[] GetFieldSchemas()
		{
			if (this._fieldSchemas == null)
			{
				IDataSourceViewSchema viewSchema = this.GetViewSchema();
				if (viewSchema != null)
				{
					this._fieldSchemas = viewSchema.GetFields();
				}
			}
			return this._fieldSchemas;
		}

		private void IgnoreRefreshSchemaEvents()
		{
			this._initialIgnoreRefreshSchemaValue = this.IgnoreRefreshSchema;
			this.IgnoreRefreshSchema = true;
			IDataSourceDesigner dataSourceDesigner = this._controlDesigner.DataSourceDesigner;
			if (dataSourceDesigner != null)
			{
				dataSourceDesigner.SuppressDataSourceEvents();
			}
		}

		private void InitForm()
		{
			global::System.Drawing.Image image = new Bitmap(base.GetType(), "FieldNodes.bmp");
			ImageList imageList = new ImageList();
			imageList.TransparentColor = Color.Magenta;
			imageList.Images.AddStrip(image);
			this._autoFieldCheck.Text = SR.GetString("DCFEditor_AutoGen");
			this._availableFieldsTree.ImageList = imageList;
			this._addFieldButton.Text = SR.GetString("DCFEditor_Add");
			ColumnHeader columnHeader = new ColumnHeader();
			columnHeader.Width = this._selFieldsList.Width - 4;
			this._selFieldsList.Columns.Add(columnHeader);
			this._selFieldsList.SmallImageList = imageList;
			Icon icon = new Icon(base.GetType(), "SortUp.ico");
			Bitmap bitmap = icon.ToBitmap();
			bitmap.MakeTransparent();
			this._moveFieldUpButton.Image = bitmap;
			this._moveFieldUpButton.AccessibleDescription = SR.GetString("DCFEditor_MoveFieldUpDesc");
			this._moveFieldUpButton.AccessibleName = SR.GetString("DCFEditor_MoveFieldUpName");
			Icon icon2 = new Icon(base.GetType(), "SortDown.ico");
			Bitmap bitmap2 = icon2.ToBitmap();
			bitmap2.MakeTransparent();
			this._moveFieldDownButton.Image = bitmap2;
			this._moveFieldDownButton.AccessibleDescription = SR.GetString("DCFEditor_MoveFieldDownDesc");
			this._moveFieldDownButton.AccessibleName = SR.GetString("DCFEditor_MoveFieldDownName");
			Icon icon3 = new Icon(base.GetType(), "Delete.ico");
			Bitmap bitmap3 = icon3.ToBitmap();
			bitmap3.MakeTransparent();
			this._deleteFieldButton.Image = bitmap3;
			this._deleteFieldButton.AccessibleDescription = SR.GetString("DCFEditor_DeleteFieldDesc");
			this._deleteFieldButton.AccessibleName = SR.GetString("DCFEditor_DeleteFieldName");
			this._templatizeLink.Text = SR.GetString("DCFEditor_Templatize");
			this._refreshSchemaLink.Text = SR.GetString("DataSourceDesigner_RefreshSchemaNoHotkey");
			this._refreshSchemaLink.Visible = this._controlDesigner.DataSourceDesigner != null && this._controlDesigner.DataSourceDesigner.CanRefreshSchema;
			this._okButton.Text = SR.GetString("OKCaption");
			this._cancelButton.Text = SR.GetString("CancelCaption");
			this._selFieldLabel.Text = SR.GetString("DCFEditor_FieldProps");
			this._availableFieldsLabel.Text = SR.GetString("DCFEditor_AvailableFields");
			this._selFieldsLabel.Text = SR.GetString("DCFEditor_SelectedFields");
			this._currentFieldProps.Site = this._controlDesigner.Component.Site;
			this.Text = SR.GetString("DCFEditor_Text");
			base.Icon = new Icon(base.GetType(), "DataControlFieldsEditor.ico");
		}

		private void InitPage()
		{
			this._autoFieldCheck.Checked = false;
			this._selectedDataSourceNode = null;
			this._selectedCheckBoxDataSourceNode = null;
			this._availableFieldsTree.Nodes.Clear();
			this._selFieldsList.Items.Clear();
			this._currentFieldItem = null;
			this._propChangesPending = false;
		}

		private void LoadFields()
		{
			DataControlFieldCollection fieldCollection = this.FieldCollection;
			if (fieldCollection != null)
			{
				int count = fieldCollection.Count;
				IDataSourceViewSchema viewSchema = this.GetViewSchema();
				for (int i = 0; i < count; i++)
				{
					DataControlField dataControlField = fieldCollection[i];
					Type type = dataControlField.GetType();
					DataControlFieldsEditor.FieldItem fieldItem;
					if (type == typeof(CheckBoxField))
					{
						fieldItem = new DataControlFieldsEditor.CheckBoxFieldItem(this, (CheckBoxField)dataControlField);
					}
					else if (type == typeof(BoundField))
					{
						fieldItem = new DataControlFieldsEditor.BoundFieldItem(this, (BoundField)dataControlField);
					}
					else if (type == typeof(ButtonField))
					{
						fieldItem = new DataControlFieldsEditor.ButtonFieldItem(this, (ButtonField)dataControlField);
					}
					else if (type == typeof(HyperLinkField))
					{
						fieldItem = new DataControlFieldsEditor.HyperLinkFieldItem(this, (HyperLinkField)dataControlField);
					}
					else if (type == typeof(TemplateField))
					{
						fieldItem = new DataControlFieldsEditor.TemplateFieldItem(this, (TemplateField)dataControlField);
					}
					else if (type == typeof(CommandField))
					{
						fieldItem = new DataControlFieldsEditor.CommandFieldItem(this, (CommandField)dataControlField);
					}
					else if (type == typeof(ImageField))
					{
						fieldItem = new DataControlFieldsEditor.ImageFieldItem(this, (ImageField)dataControlField);
					}
					else if (this._customFieldDesigners.ContainsKey(type))
					{
						fieldItem = new DataControlFieldsEditor.DataControlFieldDesignerItem(this._customFieldDesigners[type], dataControlField);
					}
					else
					{
						fieldItem = new DataControlFieldsEditor.CustomFieldItem(this, dataControlField);
					}
					fieldItem.LoadFieldInfo();
					IDataSourceViewSchemaAccessor runtimeField = fieldItem.RuntimeField;
					if (runtimeField != null)
					{
						runtimeField.DataSourceViewSchema = viewSchema;
					}
					this._selFieldsList.Items.Add(fieldItem);
				}
				if (this._selFieldsList.Items.Count != 0)
				{
					this._currentFieldItem = (DataControlFieldsEditor.FieldItem)this._selFieldsList.Items[0];
					this._currentFieldItem.Selected = true;
				}
			}
		}

		private void LoadComponent()
		{
			this.InitPage();
			this.LoadAvailableFieldsTree();
			this.LoadDataSourceFields();
			this.LoadCustomFields();
			this._autoFieldCheck.Checked = this.AutoGenerateFields;
			this.LoadFields();
			this.UpdateEnabledVisibleState();
		}

		private void LoadCustomFields()
		{
			if (this._customFieldDesigners == null)
			{
				this._customFieldDesigners = DataControlFieldHelper.GetCustomFieldDesigners(this, this.Control);
			}
			IDataSourceFieldSchema[] fieldSchemas = this.GetFieldSchemas();
			bool flag = fieldSchemas != null && fieldSchemas.Length > 0;
			foreach (KeyValuePair<Type, DataControlFieldDesigner> keyValuePair in this._customFieldDesigners)
			{
				DataControlFieldDesigner value = keyValuePair.Value;
				if (value.UsesSchema && flag)
				{
					DataControlFieldsEditor.DataSourceNode dataSourceNode = new DataControlFieldsEditor.DataSourceNode(keyValuePair.Key.Name);
					this._availableFieldsTree.Nodes.Add(dataSourceNode);
					foreach (IDataSourceFieldSchema dataSourceFieldSchema in fieldSchemas)
					{
						dataSourceNode.Nodes.Add(new DataControlFieldsEditor.DataControlFieldDesignerNode(value, dataSourceFieldSchema));
					}
					dataSourceNode.Expand();
				}
				else
				{
					this._availableFieldsTree.Nodes.Add(new DataControlFieldsEditor.DataControlFieldDesignerNode(value));
				}
			}
		}

		private void LoadDataSourceFields()
		{
			this.EnterLoadingMode();
			IDataSourceFieldSchema[] fieldSchemas = this.GetFieldSchemas();
			if (fieldSchemas != null && fieldSchemas.Length > 0)
			{
				DataControlFieldsEditor.DataFieldNode dataFieldNode = new DataControlFieldsEditor.DataFieldNode(this);
				this._availableFieldsTree.Nodes.Insert(0, dataFieldNode);
				foreach (IDataSourceFieldSchema dataSourceFieldSchema in fieldSchemas)
				{
					DataControlFieldsEditor.BoundNode boundNode = new DataControlFieldsEditor.BoundNode(this, dataSourceFieldSchema);
					this._selectedDataSourceNode.Nodes.Add(boundNode);
				}
				this._selectedDataSourceNode.Expand();
				foreach (IDataSourceFieldSchema dataSourceFieldSchema2 in fieldSchemas)
				{
					if (dataSourceFieldSchema2.DataType == typeof(bool) || dataSourceFieldSchema2.DataType == typeof(bool?))
					{
						DataControlFieldsEditor.CheckBoxNode checkBoxNode = new DataControlFieldsEditor.CheckBoxNode(this, dataSourceFieldSchema2);
						this._selectedCheckBoxDataSourceNode.Nodes.Add(checkBoxNode);
					}
				}
				this._selectedCheckBoxDataSourceNode.Expand();
				this._availableFieldsTree.SelectedNode = dataFieldNode;
				dataFieldNode.EnsureVisible();
			}
			else
			{
				DataControlFieldsEditor.BoundNode boundNode2 = new DataControlFieldsEditor.BoundNode(this, null);
				this._availableFieldsTree.Nodes.Insert(0, boundNode2);
				boundNode2.EnsureVisible();
				DataControlFieldsEditor.CheckBoxNode checkBoxNode2 = new DataControlFieldsEditor.CheckBoxNode(this, null);
				this._availableFieldsTree.Nodes.Insert(1, checkBoxNode2);
				checkBoxNode2.EnsureVisible();
				this._availableFieldsTree.SelectedNode = boundNode2;
			}
			this.ExitLoadingMode();
		}

		private void LoadAvailableFieldsTree()
		{
			IDataSourceFieldSchema[] fieldSchemas = this.GetFieldSchemas();
			if (fieldSchemas != null && fieldSchemas.Length > 0)
			{
				this._selectedDataSourceNode = new DataControlFieldsEditor.DataSourceNode();
				this._availableFieldsTree.Nodes.Add(this._selectedDataSourceNode);
				this._selectedCheckBoxDataSourceNode = new DataControlFieldsEditor.BoolDataSourceNode();
				this._availableFieldsTree.Nodes.Add(this._selectedCheckBoxDataSourceNode);
			}
			DataControlFieldsEditor.HyperLinkNode hyperLinkNode = new DataControlFieldsEditor.HyperLinkNode(this);
			this._availableFieldsTree.Nodes.Add(hyperLinkNode);
			DataControlFieldsEditor.ImageNode imageNode = new DataControlFieldsEditor.ImageNode(this);
			this._availableFieldsTree.Nodes.Add(imageNode);
			DataControlFieldsEditor.ButtonNode buttonNode = new DataControlFieldsEditor.ButtonNode(this);
			this._availableFieldsTree.Nodes.Add(buttonNode);
			DataControlFieldsEditor.CommandNode commandNode = new DataControlFieldsEditor.CommandNode(this);
			this._availableFieldsTree.Nodes.Add(commandNode);
			DataControlFieldsEditor.CommandNode commandNode2 = new DataControlFieldsEditor.CommandNode(this, 0, SR.GetString("DCFEditor_Node_Edit"), 6);
			commandNode.Nodes.Add(commandNode2);
			if (this.Control is GridView)
			{
				DataControlFieldsEditor.CommandNode commandNode3 = new DataControlFieldsEditor.CommandNode(this, 2, SR.GetString("DCFEditor_Node_Select"), 5);
				commandNode.Nodes.Add(commandNode3);
			}
			DataControlFieldsEditor.CommandNode commandNode4 = new DataControlFieldsEditor.CommandNode(this, 3, SR.GetString("DCFEditor_Node_Delete"), 7);
			commandNode.Nodes.Add(commandNode4);
			if (this.Control is DetailsView)
			{
				DataControlFieldsEditor.CommandNode commandNode5 = new DataControlFieldsEditor.CommandNode(this, 1, SR.GetString("DCFEditor_Node_Insert"), 11);
				commandNode.Nodes.Add(commandNode5);
			}
			DataControlFieldsEditor.TemplateNode templateNode = new DataControlFieldsEditor.TemplateNode(this);
			this._availableFieldsTree.Nodes.Add(templateNode);
		}

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			if (this._initialActivate)
			{
				this.LoadComponent();
				this._initialActivate = false;
			}
		}

		private void OnAvailableFieldsDoubleClick(object source, TreeNodeMouseClickEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this.OnClickAddField(source, e);
			}
		}

		private void OnAvailableFieldsGotFocus(object source, EventArgs e)
		{
			this._currentFieldProps.SelectedObject = null;
		}

		private void OnAvailableFieldsKeyPress(object source, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				this.OnClickAddField(source, e);
				e.Handled = true;
			}
		}

		private void OnChangedPropertyValues(object source, PropertyValueChangedEventArgs e)
		{
			if (this._isLoading)
			{
				return;
			}
			if (e.ChangedItem.Label == "HeaderText" || e.ChangedItem.PropertyDescriptor.ComponentType == typeof(CommandField))
			{
				this._propChangesPending = true;
				this.SaveFieldProperties();
				if (this._selFieldsList.SelectedItems.Count == 0)
				{
					this._currentFieldItem = null;
					return;
				}
				this._currentFieldItem = (DataControlFieldsEditor.FieldItem)this._selFieldsList.SelectedItems[0];
				DataControlFieldsEditor.CommandFieldItem commandFieldItem = this._currentFieldItem as DataControlFieldsEditor.CommandFieldItem;
				if (commandFieldItem != null)
				{
					commandFieldItem.UpdateImageIndex();
				}
			}
		}

		private void OnCheckChangedAutoField(object source, EventArgs e)
		{
			if (this._isLoading)
			{
				return;
			}
			this.UpdateEnabledVisibleState();
		}

		private void OnClickAddField(object source, EventArgs e)
		{
			DataControlFieldsEditor.AvailableFieldNode availableFieldNode = (DataControlFieldsEditor.AvailableFieldNode)this._availableFieldsTree.SelectedNode;
			if (!this._addFieldButton.Enabled)
			{
				return;
			}
			if (this._propChangesPending)
			{
				this.SaveFieldProperties();
			}
			if (!availableFieldNode.CreatesMultipleFields)
			{
				DataControlFieldsEditor.FieldItem fieldItem = availableFieldNode.CreateField();
				this._selFieldsList.Items.Add(fieldItem);
				this._currentFieldItem = fieldItem;
				this._currentFieldItem.Selected = true;
				this._currentFieldItem.EnsureVisible();
			}
			else
			{
				IDataSourceFieldSchema[] fieldSchemas = this.GetFieldSchemas();
				DataControlFieldsEditor.FieldItem[] array = availableFieldNode.CreateFields(this.Control, fieldSchemas);
				int num = array.Length;
				for (int i = 0; i < num; i++)
				{
					this._selFieldsList.Items.Add(array[i]);
				}
				this._currentFieldItem = array[num - 1];
				this._currentFieldItem.Selected = true;
				this._currentFieldItem.EnsureVisible();
			}
			IDataSourceViewSchemaAccessor runtimeField = this._currentFieldItem.RuntimeField;
			if (runtimeField != null)
			{
				runtimeField.DataSourceViewSchema = this.GetViewSchema();
			}
			this._selFieldsList.Focus();
			this._selFieldsList.FocusedItem = this._currentFieldItem;
			this.UpdateEnabledVisibleState();
		}

		private void OnClickDeleteField(object source, EventArgs e)
		{
			int index = this._currentFieldItem.Index;
			int num = -1;
			int count = this._selFieldsList.Items.Count;
			if (count > 1)
			{
				if (index == count - 1)
				{
					num = index - 1;
				}
				else
				{
					num = index;
				}
			}
			this._propChangesPending = false;
			this._currentFieldItem.Remove();
			this._currentFieldItem = null;
			if (num != -1)
			{
				this._currentFieldItem = (DataControlFieldsEditor.FieldItem)this._selFieldsList.Items[num];
				this._currentFieldItem.Selected = true;
				this._currentFieldItem.EnsureVisible();
				this._deleteFieldButton.Focus();
			}
			this.UpdateEnabledVisibleState();
		}

		private void OnClickMoveFieldDown(object source, EventArgs e)
		{
			this._fieldMovePending = true;
			int index = this._currentFieldItem.Index;
			ListViewItem listViewItem = this._selFieldsList.Items[index];
			this._selFieldsList.Items.RemoveAt(index);
			this._selFieldsList.Items.Insert(index + 1, listViewItem);
			this._currentFieldItem = (DataControlFieldsEditor.FieldItem)this._selFieldsList.Items[index + 1];
			this._currentFieldItem.Selected = true;
			this._currentFieldItem.EnsureVisible();
			this.UpdateFieldPositionButtonsState();
			if (this._moveFieldUpButton.Enabled && !this._moveFieldDownButton.Enabled)
			{
				this._moveFieldUpButton.Focus();
			}
			this._fieldMovePending = false;
		}

		private void OnClickMoveFieldUp(object source, EventArgs e)
		{
			this._fieldMovePending = true;
			int index = this._currentFieldItem.Index;
			ListViewItem listViewItem = this._selFieldsList.Items[index];
			this._selFieldsList.Items.RemoveAt(index);
			this._selFieldsList.Items.Insert(index - 1, listViewItem);
			this._currentFieldItem = (DataControlFieldsEditor.FieldItem)this._selFieldsList.Items[index - 1];
			this._currentFieldItem.Selected = true;
			this._currentFieldItem.EnsureVisible();
			this.UpdateFieldPositionButtonsState();
			this._fieldMovePending = false;
		}

		private void OnClickOK(object source, EventArgs e)
		{
			this.SaveComponent();
			this.PersistClonedFieldsToControl();
		}

		private void OnClickRefreshSchema(object source, LinkLabelLinkClickedEventArgs e)
		{
			this._fieldSchemas = null;
			this._viewSchema = null;
			IDataSourceDesigner dataSourceDesigner = this._controlDesigner.DataSourceDesigner;
			if (dataSourceDesigner != null && dataSourceDesigner.CanRefreshSchema)
			{
				dataSourceDesigner.RefreshSchema(false);
			}
			IDataSourceViewSchema viewSchema = this.GetViewSchema();
			foreach (object obj in this._selFieldsList.Items)
			{
				DataControlFieldsEditor.FieldItem fieldItem = (DataControlFieldsEditor.FieldItem)obj;
				IDataSourceViewSchemaAccessor runtimeField = fieldItem.RuntimeField;
				if (runtimeField != null)
				{
					runtimeField.DataSourceViewSchema = viewSchema;
				}
			}
			this._availableFieldsTree.Nodes.Clear();
			this.LoadAvailableFieldsTree();
			this.LoadDataSourceFields();
		}

		private void OnClickTemplatize(object source, LinkLabelLinkClickedEventArgs e)
		{
			if (this._propChangesPending)
			{
				this.SaveFieldProperties();
			}
			TemplateField templateField = this._currentFieldItem.GetTemplateField(this.Control);
			DataControlFieldsEditor.TemplateFieldItem templateFieldItem = new DataControlFieldsEditor.TemplateFieldItem(this, templateField);
			templateFieldItem.LoadFieldInfo();
			this._selFieldsList.Items[this._currentFieldItem.Index] = templateFieldItem;
			this._currentFieldItem = templateFieldItem;
			this._currentFieldItem.Selected = true;
			this.UpdateEnabledVisibleState();
		}

		protected override void OnClosed(EventArgs e)
		{
			IDataSourceDesigner dataSourceDesigner = this._controlDesigner.DataSourceDesigner;
			if (dataSourceDesigner != null)
			{
				dataSourceDesigner.ResumeDataSourceEvents();
			}
			this.IgnoreRefreshSchema = this._initialIgnoreRefreshSchemaValue;
		}

		private void OnSelChangedAvailableFields(object source, TreeViewEventArgs e)
		{
			this.UpdateEnabledVisibleState();
		}

		private void OnSelFieldsListGotFocus(object source, EventArgs e)
		{
			this.UpdateEnabledVisibleState();
		}

		private void OnSelFieldsListKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Delete)
			{
				if (this._currentFieldItem != null)
				{
					this.OnClickDeleteField(sender, e);
				}
				e.Handled = true;
			}
		}

		private void OnSelIndexChangedSelFieldsList(object source, EventArgs e)
		{
			if (this._fieldMovePending)
			{
				return;
			}
			if (this._propChangesPending)
			{
				this.SaveFieldProperties();
			}
			if (this._selFieldsList.SelectedItems.Count == 0)
			{
				this._currentFieldItem = null;
			}
			else
			{
				this._currentFieldItem = (DataControlFieldsEditor.FieldItem)this._selFieldsList.SelectedItems[0];
			}
			this.SetFieldPropertyHeader();
			this.UpdateEnabledVisibleState();
		}

		private void PersistClonedFieldsToControl()
		{
			DataControlFieldCollection dataControlFieldCollection = null;
			if (this.Control is GridView)
			{
				dataControlFieldCollection = ((GridView)this.Control).Columns;
			}
			else if (this.Control is DetailsView)
			{
				dataControlFieldCollection = ((DetailsView)this.Control).Fields;
			}
			if (dataControlFieldCollection != null)
			{
				dataControlFieldCollection.Clear();
				foreach (object obj in this.FieldCollection)
				{
					DataControlField dataControlField = (DataControlField)obj;
					dataControlFieldCollection.Add(dataControlField);
				}
			}
		}

		private void SaveFieldProperties()
		{
			if (this._currentFieldItem != null)
			{
				this._currentFieldItem.HeaderText = this._currentFieldItem.RuntimeField.HeaderText;
				if (this._currentFieldProps.Visible)
				{
					this._currentFieldProps.Refresh();
				}
			}
			this._propChangesPending = false;
		}

		private void SaveComponent()
		{
			if (this._propChangesPending)
			{
				this.SaveFieldProperties();
			}
			this.AutoGenerateFields = this._autoFieldCheck.Checked;
			DataControlFieldCollection fieldCollection = this.FieldCollection;
			if (fieldCollection != null)
			{
				fieldCollection.Clear();
				int count = this._selFieldsList.Items.Count;
				for (int i = 0; i < count; i++)
				{
					DataControlFieldsEditor.FieldItem fieldItem = (DataControlFieldsEditor.FieldItem)this._selFieldsList.Items[i];
					fieldCollection.Add(fieldItem.RuntimeField);
				}
			}
		}

		private void SetFieldPropertyHeader()
		{
			string text = SR.GetString("DCFEditor_FieldProps");
			if (this._currentFieldItem != null)
			{
				this.EnterLoadingMode();
				Type type = this._currentFieldItem.GetType();
				if (type == typeof(DataControlFieldsEditor.CheckBoxFieldItem))
				{
					text = SR.GetString("DCFEditor_FieldPropsFormat", new object[] { SR.GetString("DCFEditor_Node_CheckBox") });
				}
				else if (type == typeof(DataControlFieldsEditor.BoundFieldItem))
				{
					text = SR.GetString("DCFEditor_FieldPropsFormat", new object[] { SR.GetString("DCFEditor_Node_Bound") });
				}
				else if (type == typeof(DataControlFieldsEditor.ButtonFieldItem))
				{
					text = SR.GetString("DCFEditor_FieldPropsFormat", new object[] { SR.GetString("DCFEditor_Node_Button") });
				}
				else if (type == typeof(DataControlFieldsEditor.HyperLinkFieldItem))
				{
					text = SR.GetString("DCFEditor_FieldPropsFormat", new object[] { SR.GetString("DCFEditor_Node_HyperLink") });
				}
				else if (type == typeof(DataControlFieldsEditor.CommandFieldItem))
				{
					text = SR.GetString("DCFEditor_FieldPropsFormat", new object[] { SR.GetString("DCFEditor_Node_Command") });
				}
				else if (type == typeof(DataControlFieldsEditor.TemplateFieldItem))
				{
					text = SR.GetString("DCFEditor_FieldPropsFormat", new object[] { SR.GetString("DCFEditor_Node_Template") });
				}
				else if (type == typeof(DataControlFieldsEditor.ImageFieldItem))
				{
					text = SR.GetString("DCFEditor_FieldPropsFormat", new object[] { SR.GetString("DCFEditor_Node_Image") });
				}
				this.ExitLoadingMode();
			}
			this._selFieldLabel.Text = text;
		}

		private void UpdateEnabledVisibleState()
		{
			DataControlFieldsEditor.AvailableFieldNode availableFieldNode = (DataControlFieldsEditor.AvailableFieldNode)this._availableFieldsTree.SelectedNode;
			int count = this._selFieldsList.Items.Count;
			int count2 = this._selFieldsList.SelectedItems.Count;
			DataControlFieldsEditor.FieldItem fieldItem = null;
			int num = -1;
			if (count2 != 0)
			{
				fieldItem = (DataControlFieldsEditor.FieldItem)this._selFieldsList.SelectedItems[0];
			}
			if (fieldItem != null)
			{
				num = fieldItem.Index;
			}
			bool flag = num != -1;
			this._addFieldButton.Enabled = availableFieldNode != null && availableFieldNode.IsFieldCreator;
			this._deleteFieldButton.Enabled = flag;
			this.UpdateFieldPositionButtonsState();
			this._currentFieldProps.Enabled = fieldItem != null;
			this._currentFieldProps.SelectedObject = ((fieldItem != null && this._selFieldsList.Focused) ? fieldItem.RuntimeField : null);
			Type type = ((fieldItem == null) ? null : fieldItem.RuntimeField.GetType());
			this._templatizeLink.Visible = count != 0 && fieldItem != null && (type == typeof(BoundField) || type == typeof(CheckBoxField) || type == typeof(ButtonField) || type == typeof(HyperLinkField) || type == typeof(CommandField) || type == typeof(ImageField) || this._customFieldDesigners.ContainsKey(type));
		}

		private void UpdateFieldPositionButtonsState()
		{
			int num = -1;
			int count = this._selFieldsList.SelectedItems.Count;
			DataControlFieldsEditor.FieldItem fieldItem = null;
			if (count > 0)
			{
				fieldItem = this._selFieldsList.SelectedItems[0] as DataControlFieldsEditor.FieldItem;
			}
			if (fieldItem != null)
			{
				num = fieldItem.Index;
			}
			this._moveFieldUpButton.Enabled = num > 0;
			this._moveFieldDownButton.Enabled = num >= 0 && num < this._selFieldsList.Items.Count - 1;
		}

		private const int ILI_DATASOURCE = 0;

		private const int ILI_BOUND = 1;

		private const int ILI_ALL = 2;

		private const int ILI_CUSTOM = 3;

		private const int ILI_BUTTON = 4;

		private const int ILI_SELECTBUTTON = 5;

		private const int ILI_EDITBUTTON = 6;

		private const int ILI_DELETEBUTTON = 7;

		private const int ILI_HYPERLINK = 8;

		private const int ILI_TEMPLATE = 9;

		private const int ILI_CHECKBOX = 10;

		private const int ILI_INSERTBUTTON = 11;

		private const int ILI_COMMAND = 12;

		private const int ILI_BOOLDATASOURCE = 13;

		private const int ILI_IMAGE = 14;

		private const int ILI_FIELDDESIGNER = 15;

		private const int CF_EDIT = 0;

		private const int CF_INSERT = 1;

		private const int CF_SELECT = 2;

		private const int CF_DELETE = 3;

		private const int MODE_READONLY = 0;

		private const int MODE_EDIT = 1;

		private const int MODE_INSERT = 2;

		private DataControlFieldsEditor.DataSourceNode _selectedDataSourceNode;

		private DataControlFieldsEditor.BoolDataSourceNode _selectedCheckBoxDataSourceNode;

		private DataControlFieldsEditor.FieldItem _currentFieldItem;

		private bool _propChangesPending;

		private bool _fieldMovePending;

		private DataControlFieldCollection _clonedFieldCollection;

		private DataBoundControlDesigner _controlDesigner;

		private bool _isLoading;

		private IDataSourceFieldSchema[] _fieldSchemas;

		private IDataSourceViewSchema _viewSchema;

		private bool _initialActivate;

		private bool _initialIgnoreRefreshSchemaValue;

		private IDictionary<Type, DataControlFieldDesigner> _customFieldDesigners;

		private abstract class AvailableFieldNode : global::System.Windows.Forms.TreeNode
		{
			public AvailableFieldNode(string text, int icon)
				: base(text, icon, icon)
			{
			}

			public virtual bool CreatesMultipleFields
			{
				get
				{
					return false;
				}
			}

			public virtual bool IsFieldCreator
			{
				get
				{
					return true;
				}
			}

			public virtual DataControlFieldsEditor.FieldItem CreateField()
			{
				return null;
			}

			public virtual DataControlFieldsEditor.FieldItem[] CreateFields(DataBoundControl control, IDataSourceFieldSchema[] fieldSchemas)
			{
				return null;
			}
		}

		private class DataSourceNode : DataControlFieldsEditor.AvailableFieldNode
		{
			public DataSourceNode()
				: base(SR.GetString("DCFEditor_Node_Bound"), 0)
			{
			}

			public DataSourceNode(string nodeText)
				: base(nodeText, 0)
			{
			}

			public override bool IsFieldCreator
			{
				get
				{
					return false;
				}
			}
		}

		private class BoolDataSourceNode : DataControlFieldsEditor.AvailableFieldNode
		{
			public BoolDataSourceNode()
				: base(SR.GetString("DCFEditor_Node_CheckBox"), 13)
			{
			}

			public override bool IsFieldCreator
			{
				get
				{
					return false;
				}
			}
		}

		private class DataFieldNode : DataControlFieldsEditor.AvailableFieldNode
		{
			public DataFieldNode(DataControlFieldsEditor fieldsEditor)
				: base(SR.GetString("DCFEditor_Node_AllFields"), 2)
			{
				this._fieldsEditor = fieldsEditor;
			}

			public override bool CreatesMultipleFields
			{
				get
				{
					return true;
				}
			}

			public override DataControlFieldsEditor.FieldItem CreateField()
			{
				throw new NotSupportedException();
			}

			public override DataControlFieldsEditor.FieldItem[] CreateFields(DataBoundControl control, IDataSourceFieldSchema[] fieldSchemas)
			{
				if (fieldSchemas == null)
				{
					return null;
				}
				ArrayList arrayList = new ArrayList();
				foreach (IDataSourceFieldSchema dataSourceFieldSchema in fieldSchemas)
				{
					if ((control is GridView && ((GridView)control).IsBindableType(dataSourceFieldSchema.DataType)) || (control is DetailsView && ((DetailsView)control).IsBindableType(dataSourceFieldSchema.DataType)))
					{
						string name = dataSourceFieldSchema.Name;
						BoundField boundField;
						DataControlFieldsEditor.FieldItem fieldItem;
						if (dataSourceFieldSchema.DataType == typeof(bool) || dataSourceFieldSchema.DataType == typeof(bool?))
						{
							boundField = new CheckBoxField();
							boundField.HeaderText = name;
							boundField.DataField = name;
							boundField.SortExpression = name;
							fieldItem = new DataControlFieldsEditor.CheckBoxFieldItem(this._fieldsEditor, (CheckBoxField)boundField);
						}
						else
						{
							boundField = new BoundField();
							boundField.HeaderText = name;
							boundField.DataField = name;
							boundField.SortExpression = name;
							fieldItem = new DataControlFieldsEditor.BoundFieldItem(this._fieldsEditor, boundField);
						}
						if (dataSourceFieldSchema.PrimaryKey)
						{
							boundField.ReadOnly = true;
						}
						if (dataSourceFieldSchema.Identity)
						{
							boundField.InsertVisible = false;
						}
						fieldItem.LoadFieldInfo();
						arrayList.Add(fieldItem);
					}
				}
				return (DataControlFieldsEditor.FieldItem[])arrayList.ToArray(typeof(DataControlFieldsEditor.FieldItem));
			}

			private DataControlFieldsEditor _fieldsEditor;
		}

		private class BoundNode : DataControlFieldsEditor.AvailableFieldNode
		{
			public BoundNode(DataControlFieldsEditor fieldsEditor, IDataSourceFieldSchema fieldSchema)
				: base((fieldSchema == null) ? string.Empty : fieldSchema.Name, 1)
			{
				this._fieldSchema = fieldSchema;
				this._fieldsEditor = fieldsEditor;
				if (fieldSchema == null)
				{
					this._genericBoundField = true;
					base.Text = SR.GetString("DCFEditor_Node_Bound");
				}
			}

			public override DataControlFieldsEditor.FieldItem CreateField()
			{
				BoundField boundField = new BoundField();
				string text = string.Empty;
				if (this._fieldSchema != null)
				{
					text = this._fieldSchema.Name;
				}
				if (!this._genericBoundField)
				{
					boundField.HeaderText = text;
					boundField.DataField = text;
					boundField.SortExpression = text;
				}
				if (this._fieldSchema != null)
				{
					if (this._fieldSchema.PrimaryKey)
					{
						boundField.ReadOnly = true;
					}
					if (this._fieldSchema.Identity)
					{
						boundField.InsertVisible = false;
					}
				}
				DataControlFieldsEditor.FieldItem fieldItem = new DataControlFieldsEditor.BoundFieldItem(this._fieldsEditor, boundField);
				fieldItem.LoadFieldInfo();
				return fieldItem;
			}

			protected IDataSourceFieldSchema _fieldSchema;

			private bool _genericBoundField;

			private DataControlFieldsEditor _fieldsEditor;
		}

		private class ButtonNode : DataControlFieldsEditor.AvailableFieldNode
		{
			public ButtonNode(DataControlFieldsEditor fieldsEditor)
				: this(fieldsEditor, string.Empty, SR.GetString("DCFEditor_Button"), SR.GetString("DCFEditor_Node_Button"))
			{
			}

			public ButtonNode(DataControlFieldsEditor fieldsEditor, string command, string buttonText, string text)
				: base(text, 4)
			{
				this._fieldsEditor = fieldsEditor;
				this.command = command;
				this.buttonText = buttonText;
			}

			public override DataControlFieldsEditor.FieldItem CreateField()
			{
				ButtonField buttonField = new ButtonField();
				buttonField.Text = this.buttonText;
				buttonField.CommandName = this.command;
				DataControlFieldsEditor.FieldItem fieldItem = new DataControlFieldsEditor.ButtonFieldItem(this._fieldsEditor, buttonField);
				fieldItem.LoadFieldInfo();
				return fieldItem;
			}

			private string command;

			private string buttonText;

			private DataControlFieldsEditor _fieldsEditor;
		}

		private class CheckBoxNode : DataControlFieldsEditor.AvailableFieldNode
		{
			public CheckBoxNode(DataControlFieldsEditor fieldsEditor, IDataSourceFieldSchema fieldSchema)
				: base((fieldSchema == null) ? string.Empty : fieldSchema.Name, 10)
			{
				this._fieldsEditor = fieldsEditor;
				this._fieldSchema = fieldSchema;
				if (fieldSchema == null)
				{
					this._genericCheckBoxField = true;
					base.Text = SR.GetString("DCFEditor_Node_CheckBox");
				}
			}

			public override DataControlFieldsEditor.FieldItem CreateField()
			{
				CheckBoxField checkBoxField = new CheckBoxField();
				string text = string.Empty;
				if (this._fieldSchema != null)
				{
					text = this._fieldSchema.Name;
				}
				if (!this._genericCheckBoxField)
				{
					checkBoxField.HeaderText = text;
					checkBoxField.DataField = text;
					checkBoxField.SortExpression = text;
				}
				if (this._fieldSchema != null)
				{
					if (this._fieldSchema.PrimaryKey)
					{
						checkBoxField.ReadOnly = true;
					}
					if (this._fieldSchema.Identity)
					{
						checkBoxField.InsertVisible = false;
					}
				}
				DataControlFieldsEditor.FieldItem fieldItem = new DataControlFieldsEditor.CheckBoxFieldItem(this._fieldsEditor, checkBoxField);
				fieldItem.LoadFieldInfo();
				return fieldItem;
			}

			protected IDataSourceFieldSchema _fieldSchema;

			private bool _genericCheckBoxField;

			private DataControlFieldsEditor _fieldsEditor;
		}

		private class ImageNode : DataControlFieldsEditor.AvailableFieldNode
		{
			public ImageNode(DataControlFieldsEditor fieldsEditor)
				: base(string.Empty, 14)
			{
				this._fieldsEditor = fieldsEditor;
				base.Text = SR.GetString("DCFEditor_Node_Image");
			}

			public override DataControlFieldsEditor.FieldItem CreateField()
			{
				ImageField imageField = new ImageField();
				DataControlFieldsEditor.FieldItem fieldItem = new DataControlFieldsEditor.ImageFieldItem(this._fieldsEditor, imageField);
				fieldItem.LoadFieldInfo();
				return fieldItem;
			}

			private DataControlFieldsEditor _fieldsEditor;
		}

		private class CommandNode : DataControlFieldsEditor.AvailableFieldNode
		{
			public CommandNode(DataControlFieldsEditor fieldsEditor)
				: this(fieldsEditor, -1, SR.GetString("DCFEditor_Node_Command"), 12)
			{
			}

			public CommandNode(DataControlFieldsEditor fieldsEditor, int commandType, string text, int icon)
				: base(text, icon)
			{
				this.commandType = commandType;
				this._fieldsEditor = fieldsEditor;
			}

			public override DataControlFieldsEditor.FieldItem CreateField()
			{
				CommandField commandField = new CommandField();
				switch (this.commandType)
				{
				case 0:
					commandField.ShowEditButton = true;
					break;
				case 1:
					commandField.ShowInsertButton = true;
					break;
				case 2:
					commandField.ShowSelectButton = true;
					break;
				case 3:
					commandField.ShowDeleteButton = true;
					break;
				}
				DataControlFieldsEditor.FieldItem fieldItem = new DataControlFieldsEditor.CommandFieldItem(this._fieldsEditor, commandField);
				fieldItem.LoadFieldInfo();
				return fieldItem;
			}

			private int commandType;

			private DataControlFieldsEditor _fieldsEditor;
		}

		private class HyperLinkNode : DataControlFieldsEditor.AvailableFieldNode
		{
			public HyperLinkNode(DataControlFieldsEditor fieldsEditor)
				: this(fieldsEditor, SR.GetString("DCFEditor_HyperLink"))
			{
			}

			public HyperLinkNode(DataControlFieldsEditor fieldsEditor, string hyperLinkText)
				: base(SR.GetString("DCFEditor_Node_HyperLink"), 8)
			{
				this._fieldsEditor = fieldsEditor;
				this.hyperLinkText = hyperLinkText;
			}

			public override DataControlFieldsEditor.FieldItem CreateField()
			{
				HyperLinkField hyperLinkField = new HyperLinkField();
				DataControlFieldsEditor.FieldItem fieldItem = new DataControlFieldsEditor.HyperLinkFieldItem(this._fieldsEditor, hyperLinkField);
				fieldItem.Text = this.hyperLinkText;
				fieldItem.LoadFieldInfo();
				return fieldItem;
			}

			private string hyperLinkText;

			private DataControlFieldsEditor _fieldsEditor;
		}

		private class TemplateNode : DataControlFieldsEditor.AvailableFieldNode
		{
			public TemplateNode(DataControlFieldsEditor fieldsEditor)
				: base(SR.GetString("DCFEditor_Node_Template"), 9)
			{
				this._fieldsEditor = fieldsEditor;
			}

			public override DataControlFieldsEditor.FieldItem CreateField()
			{
				TemplateField templateField = new TemplateField();
				DataControlFieldsEditor.FieldItem fieldItem = new DataControlFieldsEditor.TemplateFieldItem(this._fieldsEditor, templateField);
				fieldItem.LoadFieldInfo();
				return fieldItem;
			}

			private DataControlFieldsEditor _fieldsEditor;
		}

		private abstract class FieldItem : ListViewItem
		{
			public FieldItem(DataControlFieldsEditor fieldsEditor, DataControlField runtimeField, int image)
				: base(string.Empty, image)
			{
				this.fieldsEditor = fieldsEditor;
				this.runtimeField = runtimeField;
				base.Text = this.GetNodeText(null);
			}

			public string HeaderText
			{
				get
				{
					return this.runtimeField.HeaderText;
				}
				set
				{
					this.runtimeField.HeaderText = value;
					this.UpdateDisplayText();
				}
			}

			public DataControlField RuntimeField
			{
				get
				{
					return this.runtimeField;
				}
			}

			protected virtual string GetDefaultNodeText()
			{
				return this.runtimeField.GetType().Name;
			}

			public virtual string GetNodeText(string headerText)
			{
				if (headerText == null || headerText.Length == 0)
				{
					return this.GetDefaultNodeText();
				}
				return headerText;
			}

			protected ITemplate GetTemplate(DataBoundControl control, string templateContent)
			{
				return DataControlFieldHelper.GetTemplate(control, templateContent);
			}

			public virtual TemplateField GetTemplateField(DataBoundControl dataBoundControl)
			{
				return DataControlFieldHelper.GetTemplateField(this.runtimeField, dataBoundControl);
			}

			public virtual void LoadFieldInfo()
			{
				this.UpdateDisplayText();
			}

			protected string PrepareFormatString(string formatString)
			{
				return formatString.Replace("'", "&#039;");
			}

			protected void UpdateDisplayText()
			{
				base.Text = this.GetNodeText(this.HeaderText);
			}

			protected DataControlField runtimeField;

			protected DataControlFieldsEditor fieldsEditor;
		}

		private class BoundFieldItem : DataControlFieldsEditor.FieldItem
		{
			public BoundFieldItem(DataControlFieldsEditor fieldsEditor, BoundField runtimeField)
				: base(fieldsEditor, runtimeField, 1)
			{
			}

			protected override string GetDefaultNodeText()
			{
				string dataField = ((BoundField)base.RuntimeField).DataField;
				if (dataField != null && dataField.Length != 0)
				{
					return dataField;
				}
				return SR.GetString("DCFEditor_Node_Bound");
			}

			public override TemplateField GetTemplateField(DataBoundControl dataBoundControl)
			{
				TemplateField templateField = base.GetTemplateField(dataBoundControl);
				templateField.SortExpression = base.RuntimeField.SortExpression;
				templateField.ItemTemplate = base.GetTemplate(dataBoundControl, this.GetTemplateContent(0, false));
				templateField.ConvertEmptyStringToNull = ((BoundField)base.RuntimeField).ConvertEmptyStringToNull;
				templateField.EditItemTemplate = base.GetTemplate(dataBoundControl, this.GetTemplateContent(1, ((BoundField)base.RuntimeField).ReadOnly));
				if (dataBoundControl is DetailsView && ((BoundField)base.RuntimeField).InsertVisible)
				{
					templateField.InsertItemTemplate = base.GetTemplate(dataBoundControl, this.GetTemplateContent(2, false));
				}
				return templateField;
			}

			private string GetTemplateContent(int editMode, bool readOnly)
			{
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = editMode == 1 && readOnly;
				Type type = ((editMode == 0 || flag) ? typeof(global::System.Web.UI.WebControls.Label) : typeof(global::System.Web.UI.WebControls.TextBox));
				string dataFormatString = ((BoundField)base.RuntimeField).DataFormatString;
				string dataField = ((BoundField)base.RuntimeField).DataField;
				string text = string.Empty;
				if (editMode != 1 || ((BoundField)base.RuntimeField).ApplyFormatInEditMode || flag)
				{
					text = base.PrepareFormatString(dataFormatString);
				}
				string text2 = (flag ? DesignTimeDataBinding.CreateEvalExpression(dataField, text) : DesignTimeDataBinding.CreateBindExpression(dataField, text));
				if (editMode == 2 && !((BoundField)base.RuntimeField).InsertVisible)
				{
					return string.Empty;
				}
				stringBuilder.Append("<asp:");
				stringBuilder.Append(type.Name);
				stringBuilder.Append(" runat=\"server\"");
				if (dataField.Length != 0)
				{
					stringBuilder.Append(" Text='<%# ");
					stringBuilder.Append(text2);
					stringBuilder.Append(" %>'");
				}
				stringBuilder.Append(" id=\"");
				stringBuilder.Append(this.fieldsEditor.GetNewDataSourceName(type, editMode));
				stringBuilder.Append("\"></asp:");
				stringBuilder.Append(type.Name);
				stringBuilder.Append(">");
				return stringBuilder.ToString();
			}
		}

		private class ButtonFieldItem : DataControlFieldsEditor.FieldItem
		{
			public ButtonFieldItem(DataControlFieldsEditor fieldsEditor, ButtonField runtimeField)
				: base(fieldsEditor, runtimeField, 4)
			{
			}

			protected override string GetDefaultNodeText()
			{
				string text = ((ButtonField)this.runtimeField).Text;
				if (text != null && text.Length != 0)
				{
					return text;
				}
				return SR.GetString("DCFEditor_Node_Button");
			}

			public override TemplateField GetTemplateField(DataBoundControl dataBoundControl)
			{
				TemplateField templateField = base.GetTemplateField(dataBoundControl);
				ButtonField buttonField = (ButtonField)base.RuntimeField;
				StringBuilder stringBuilder = new StringBuilder();
				Type type = typeof(global::System.Web.UI.WebControls.Button);
				if (buttonField.ButtonType == ButtonType.Link)
				{
					type = typeof(LinkButton);
				}
				else if (buttonField.ButtonType == ButtonType.Image)
				{
					type = typeof(ImageButton);
				}
				stringBuilder.Append("<asp:");
				stringBuilder.Append(type.Name);
				stringBuilder.Append(" runat=\"server\"");
				if (buttonField.DataTextField.Length != 0)
				{
					stringBuilder.Append(" Text='<%# ");
					stringBuilder.Append(DesignTimeDataBinding.CreateEvalExpression(buttonField.DataTextField, base.PrepareFormatString(buttonField.DataTextFormatString)));
					stringBuilder.Append(" %>'");
				}
				else
				{
					stringBuilder.Append(" Text=\"");
					stringBuilder.Append(buttonField.Text);
					stringBuilder.Append("\"");
				}
				stringBuilder.Append(" CommandName=\"");
				stringBuilder.Append(buttonField.CommandName);
				stringBuilder.Append("\"");
				if (buttonField.ButtonType == ButtonType.Image && buttonField.ImageUrl.Length > 0)
				{
					stringBuilder.Append(" ImageUrl=\"");
					stringBuilder.Append(buttonField.ImageUrl);
					stringBuilder.Append("\"");
				}
				stringBuilder.Append(" CausesValidation=\"false\" id=\"");
				stringBuilder.Append(this.fieldsEditor.GetNewDataSourceName(type, 0));
				stringBuilder.Append("\"></asp:");
				stringBuilder.Append(type.Name);
				stringBuilder.Append(">");
				templateField.ItemTemplate = base.GetTemplate(dataBoundControl, stringBuilder.ToString());
				return templateField;
			}
		}

		private class CheckBoxFieldItem : DataControlFieldsEditor.FieldItem
		{
			public CheckBoxFieldItem(DataControlFieldsEditor fieldsEditor, CheckBoxField runtimeField)
				: base(fieldsEditor, runtimeField, 10)
			{
			}

			protected override string GetDefaultNodeText()
			{
				string dataField = ((CheckBoxField)base.RuntimeField).DataField;
				if (dataField != null && dataField.Length != 0)
				{
					return dataField;
				}
				return SR.GetString("DCFEditor_Node_CheckBox");
			}

			public override TemplateField GetTemplateField(DataBoundControl dataBoundControl)
			{
				TemplateField templateField = base.GetTemplateField(dataBoundControl);
				CheckBoxField checkBoxField = (CheckBoxField)base.RuntimeField;
				templateField.SortExpression = checkBoxField.SortExpression;
				templateField.ItemTemplate = base.GetTemplate(dataBoundControl, this.GetTemplateContent(0));
				if (!checkBoxField.ReadOnly)
				{
					templateField.EditItemTemplate = base.GetTemplate(dataBoundControl, this.GetTemplateContent(1));
				}
				if (dataBoundControl is DetailsView && ((CheckBoxField)base.RuntimeField).InsertVisible)
				{
					templateField.InsertItemTemplate = base.GetTemplate(dataBoundControl, this.GetTemplateContent(2));
				}
				return templateField;
			}

			private string GetTemplateContent(int editMode)
			{
				StringBuilder stringBuilder = new StringBuilder();
				Type typeFromHandle = typeof(global::System.Web.UI.WebControls.CheckBox);
				if (editMode == 2 && !((CheckBoxField)base.RuntimeField).InsertVisible)
				{
					return string.Empty;
				}
				stringBuilder.Append("<asp:");
				stringBuilder.Append(typeFromHandle.Name);
				stringBuilder.Append(" runat=\"server\"");
				string dataField = ((CheckBoxField)base.RuntimeField).DataField;
				if (dataField.Length != 0)
				{
					stringBuilder.Append(" Checked='<%# ");
					stringBuilder.Append(DesignTimeDataBinding.CreateBindExpression(dataField, string.Empty));
					stringBuilder.Append(" %>'");
					if (editMode == 0)
					{
						stringBuilder.Append(" Enabled=\"false\"");
					}
				}
				stringBuilder.Append(" id=\"");
				stringBuilder.Append(this.fieldsEditor.GetNewDataSourceName(typeFromHandle, editMode));
				stringBuilder.Append("\"></asp:");
				stringBuilder.Append(typeFromHandle.Name);
				stringBuilder.Append(">");
				return stringBuilder.ToString();
			}
		}

		private class ImageFieldItem : DataControlFieldsEditor.FieldItem
		{
			public ImageFieldItem(DataControlFieldsEditor fieldsEditor, ImageField runtimeField)
				: base(fieldsEditor, runtimeField, 14)
			{
			}

			protected override string GetDefaultNodeText()
			{
				string dataImageUrlField = ((ImageField)base.RuntimeField).DataImageUrlField;
				if (dataImageUrlField != null && dataImageUrlField.Length != 0)
				{
					return dataImageUrlField;
				}
				return SR.GetString("DCFEditor_Node_Image");
			}

			public override TemplateField GetTemplateField(DataBoundControl dataBoundControl)
			{
				TemplateField templateField = base.GetTemplateField(dataBoundControl);
				ImageField imageField = (ImageField)base.RuntimeField;
				templateField.SortExpression = imageField.SortExpression;
				templateField.ItemTemplate = base.GetTemplate(dataBoundControl, this.GetTemplateContent(0));
				templateField.ConvertEmptyStringToNull = imageField.ConvertEmptyStringToNull;
				if (!imageField.ReadOnly)
				{
					templateField.EditItemTemplate = base.GetTemplate(dataBoundControl, this.GetTemplateContent(1));
					if (dataBoundControl is DetailsView)
					{
						templateField.InsertItemTemplate = base.GetTemplate(dataBoundControl, this.GetTemplateContent(2));
					}
				}
				return templateField;
			}

			private string GetTemplateContent(int editMode)
			{
				StringBuilder stringBuilder = new StringBuilder();
				string dataImageUrlField = ((ImageField)base.RuntimeField).DataImageUrlField;
				string dataAlternateTextField = ((ImageField)this.runtimeField).DataAlternateTextField;
				string text;
				if (dataAlternateTextField.Length > 0)
				{
					string dataAlternateTextFormatString = ((ImageField)this.runtimeField).DataAlternateTextFormatString;
					text = "'<%# " + DesignTimeDataBinding.CreateEvalExpression(dataAlternateTextField, base.PrepareFormatString(dataAlternateTextFormatString)) + " %>'";
				}
				else
				{
					text = ((ImageField)this.runtimeField).AlternateText;
				}
				Type type;
				if (editMode == 0)
				{
					type = typeof(global::System.Web.UI.WebControls.Image);
				}
				else
				{
					type = typeof(global::System.Web.UI.WebControls.TextBox);
				}
				stringBuilder.Append("<asp:");
				stringBuilder.Append(type.Name);
				stringBuilder.Append(" runat=\"server\"");
				if (dataImageUrlField.Length > 0)
				{
					if (type == typeof(global::System.Web.UI.WebControls.Image))
					{
						stringBuilder.Append(" ImageUrl='<%# ");
						stringBuilder.Append(DesignTimeDataBinding.CreateEvalExpression(dataImageUrlField, base.PrepareFormatString(((ImageField)this.runtimeField).DataImageUrlFormatString)));
					}
					else if (type == typeof(global::System.Web.UI.WebControls.TextBox))
					{
						stringBuilder.Append(" Text='<%# ");
						stringBuilder.Append(DesignTimeDataBinding.CreateEvalExpression(dataImageUrlField, string.Empty));
					}
					stringBuilder.Append(" %>' ");
				}
				if (text.Length > 0)
				{
					if (type == typeof(global::System.Web.UI.WebControls.TextBox))
					{
						stringBuilder.Append(" Tooltip=");
					}
					else
					{
						stringBuilder.Append(" AlternateText=");
					}
					stringBuilder.Append(text);
				}
				stringBuilder.Append(" id=\"");
				stringBuilder.Append(this.fieldsEditor.GetNewDataSourceName(type, editMode));
				stringBuilder.Append("\"></asp:");
				stringBuilder.Append(type.Name);
				stringBuilder.Append(">");
				return stringBuilder.ToString();
			}
		}

		private class HyperLinkFieldItem : DataControlFieldsEditor.FieldItem
		{
			public HyperLinkFieldItem(DataControlFieldsEditor fieldsEditor, HyperLinkField runtimeField)
				: base(fieldsEditor, runtimeField, 8)
			{
			}

			protected override string GetDefaultNodeText()
			{
				string text = ((HyperLinkField)base.RuntimeField).Text;
				if (text != null && text.Length != 0)
				{
					return text;
				}
				return SR.GetString("DCFEditor_Node_HyperLink");
			}

			public override TemplateField GetTemplateField(DataBoundControl dataBoundControl)
			{
				TemplateField templateField = base.GetTemplateField(dataBoundControl);
				HyperLinkField hyperLinkField = (HyperLinkField)base.RuntimeField;
				Type typeFromHandle = typeof(HyperLink);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("<asp:");
				stringBuilder.Append(typeFromHandle.Name);
				stringBuilder.Append(" runat=\"server\"");
				if (hyperLinkField.DataTextField.Length != 0)
				{
					stringBuilder.Append(" Text='<%# ");
					stringBuilder.Append(DesignTimeDataBinding.CreateEvalExpression(hyperLinkField.DataTextField, base.PrepareFormatString(hyperLinkField.DataTextFormatString)));
					stringBuilder.Append(" %>'");
				}
				else
				{
					stringBuilder.Append(" Text=\"");
					stringBuilder.Append(hyperLinkField.Text);
					stringBuilder.Append("\"");
				}
				if (hyperLinkField.DataNavigateUrlFields.Length != 0 && hyperLinkField.DataNavigateUrlFields[0].Length > 0)
				{
					stringBuilder.Append(" NavigateUrl='<%# ");
					stringBuilder.Append(DesignTimeDataBinding.CreateEvalExpression(hyperLinkField.DataNavigateUrlFields[0], base.PrepareFormatString(hyperLinkField.DataNavigateUrlFormatString)));
					stringBuilder.Append(" %>'");
				}
				else
				{
					stringBuilder.Append(" NavigateUrl=\"");
					stringBuilder.Append(hyperLinkField.NavigateUrl);
					stringBuilder.Append("\"");
				}
				if (hyperLinkField.Target.Length != 0)
				{
					stringBuilder.Append(" Target=\"");
					stringBuilder.Append(hyperLinkField.Target);
					stringBuilder.Append("\"");
				}
				stringBuilder.Append(" id=\"");
				stringBuilder.Append(this.fieldsEditor.GetNewDataSourceName(typeFromHandle, 0));
				stringBuilder.Append("\"></asp:");
				stringBuilder.Append(typeFromHandle.Name);
				stringBuilder.Append(">");
				templateField.ItemTemplate = base.GetTemplate(dataBoundControl, stringBuilder.ToString());
				return templateField;
			}
		}

		private class TemplateFieldItem : DataControlFieldsEditor.FieldItem
		{
			public TemplateFieldItem(DataControlFieldsEditor fieldsEditor, TemplateField runtimeField)
				: base(fieldsEditor, runtimeField, 9)
			{
			}

			protected override string GetDefaultNodeText()
			{
				return SR.GetString("DCFEditor_Node_Template");
			}
		}

		private class CommandFieldItem : DataControlFieldsEditor.FieldItem
		{
			public CommandFieldItem(DataControlFieldsEditor fieldsEditor, CommandField runtimeField)
				: base(fieldsEditor, runtimeField, 12)
			{
				this.UpdateImageIndex();
			}

			private string BuildButtonString(Type controlType, string buttonText, string commandName, string imageUrl, bool causesValidation, int mode, ref int buttonNameStartIndex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("<asp:");
				stringBuilder.Append(controlType.Name);
				stringBuilder.Append(" runat=\"server\"");
				stringBuilder.Append(" Text=\"");
				stringBuilder.Append(buttonText);
				stringBuilder.Append("\"");
				stringBuilder.Append(" CommandName=\"");
				stringBuilder.Append(commandName);
				if (imageUrl != null && imageUrl.Length > 0)
				{
					stringBuilder.Append("\" ImageUrl=\"");
					stringBuilder.Append(imageUrl);
				}
				stringBuilder.Append("\" CausesValidation=\"");
				stringBuilder.Append(causesValidation.ToString());
				stringBuilder.Append("\" id=\"");
				stringBuilder.Append(this.fieldsEditor.GetNewDataSourceName(controlType, mode, ref buttonNameStartIndex));
				stringBuilder.Append("\"></asp:");
				stringBuilder.Append(controlType.Name);
				stringBuilder.Append(">");
				return stringBuilder.ToString();
			}

			protected override string GetDefaultNodeText()
			{
				CommandField commandField = (CommandField)base.RuntimeField;
				if (commandField.ShowEditButton && !commandField.ShowDeleteButton && !commandField.ShowSelectButton && !commandField.ShowInsertButton)
				{
					return SR.GetString("DCFEditor_Node_Edit");
				}
				if (commandField.ShowDeleteButton && !commandField.ShowEditButton && !commandField.ShowSelectButton && !commandField.ShowInsertButton)
				{
					return SR.GetString("DCFEditor_Node_Delete");
				}
				if (commandField.ShowSelectButton && !commandField.ShowDeleteButton && !commandField.ShowEditButton && !commandField.ShowInsertButton)
				{
					return SR.GetString("DCFEditor_Node_Select");
				}
				if (commandField.ShowInsertButton && !commandField.ShowDeleteButton && !commandField.ShowSelectButton && !commandField.ShowEditButton)
				{
					return SR.GetString("DCFEditor_Node_Insert");
				}
				return SR.GetString("DCFEditor_Node_Command");
			}

			public override TemplateField GetTemplateField(DataBoundControl dataBoundControl)
			{
				TemplateField templateField = base.GetTemplateField(dataBoundControl);
				templateField.ItemTemplate = base.GetTemplate(dataBoundControl, this.GetTemplateContent(0));
				templateField.EditItemTemplate = base.GetTemplate(dataBoundControl, this.GetTemplateContent(1));
				if (dataBoundControl is DetailsView)
				{
					templateField.InsertItemTemplate = base.GetTemplate(dataBoundControl, this.GetTemplateContent(2));
				}
				return templateField;
			}

			private string GetTemplateContent(int editMode)
			{
				StringBuilder stringBuilder = new StringBuilder();
				CommandField commandField = (CommandField)base.RuntimeField;
				Type type = typeof(global::System.Web.UI.WebControls.Button);
				int num = 1;
				if (commandField.ButtonType == ButtonType.Link)
				{
					type = typeof(LinkButton);
				}
				else if (commandField.ButtonType == ButtonType.Image)
				{
					type = typeof(ImageButton);
				}
				switch (editMode)
				{
				case 0:
				{
					bool flag = true;
					if (commandField.ShowEditButton)
					{
						string text = ((commandField.ButtonType == ButtonType.Image) ? commandField.EditImageUrl : null);
						stringBuilder.Append(this.BuildButtonString(type, commandField.EditText, "Edit", text, false, 0, ref num));
						num++;
						flag = false;
					}
					if (commandField.ShowInsertButton)
					{
						if (!flag)
						{
							stringBuilder.Append("&nbsp;");
						}
						string text2 = ((commandField.ButtonType == ButtonType.Image) ? commandField.NewImageUrl : null);
						stringBuilder.Append(this.BuildButtonString(type, commandField.NewText, "New", text2, false, 0, ref num));
						num++;
					}
					if (commandField.ShowSelectButton)
					{
						if (!flag)
						{
							stringBuilder.Append("&nbsp;");
						}
						string text3 = ((commandField.ButtonType == ButtonType.Image) ? commandField.SelectImageUrl : null);
						stringBuilder.Append(this.BuildButtonString(type, commandField.SelectText, "Select", text3, false, 0, ref num));
						num++;
					}
					if (commandField.ShowDeleteButton)
					{
						if (!flag)
						{
							stringBuilder.Append("&nbsp;");
						}
						string text4 = ((commandField.ButtonType == ButtonType.Image) ? commandField.DeleteImageUrl : null);
						stringBuilder.Append(this.BuildButtonString(type, commandField.DeleteText, "Delete", text4, false, 0, ref num));
						num++;
					}
					break;
				}
				case 1:
					if (commandField.ShowEditButton)
					{
						string text5 = ((commandField.ButtonType == ButtonType.Image) ? commandField.UpdateImageUrl : null);
						stringBuilder.Append(this.BuildButtonString(type, commandField.UpdateText, "Update", text5, true, 1, ref num));
						num++;
						if (commandField.ShowCancelButton)
						{
							stringBuilder.Append("&nbsp;");
							string text6 = ((commandField.ButtonType == ButtonType.Image) ? commandField.CancelImageUrl : null);
							stringBuilder.Append(this.BuildButtonString(type, commandField.CancelText, "Cancel", text6, false, 1, ref num));
							num++;
						}
					}
					break;
				case 2:
					if (commandField.ShowInsertButton)
					{
						string text7 = ((commandField.ButtonType == ButtonType.Image) ? commandField.InsertImageUrl : null);
						stringBuilder.Append(this.BuildButtonString(type, commandField.InsertText, "Insert", text7, true, 2, ref num));
						num++;
						if (commandField.ShowCancelButton)
						{
							stringBuilder.Append("&nbsp;");
							string text8 = ((commandField.ButtonType == ButtonType.Image) ? commandField.CancelImageUrl : null);
							stringBuilder.Append(this.BuildButtonString(type, commandField.CancelText, "Cancel", text8, false, 2, ref num));
							num++;
						}
					}
					break;
				}
				return stringBuilder.ToString();
			}

			public void UpdateImageIndex()
			{
				CommandField commandField = (CommandField)base.RuntimeField;
				if (commandField.ShowEditButton && !commandField.ShowDeleteButton && !commandField.ShowSelectButton && !commandField.ShowInsertButton)
				{
					base.ImageIndex = 6;
					return;
				}
				if (commandField.ShowDeleteButton && !commandField.ShowEditButton && !commandField.ShowSelectButton && !commandField.ShowInsertButton)
				{
					base.ImageIndex = 7;
					return;
				}
				if (commandField.ShowSelectButton && !commandField.ShowDeleteButton && !commandField.ShowEditButton && !commandField.ShowInsertButton)
				{
					base.ImageIndex = 5;
					return;
				}
				if (commandField.ShowInsertButton && !commandField.ShowDeleteButton && !commandField.ShowSelectButton && !commandField.ShowEditButton)
				{
					base.ImageIndex = 11;
					return;
				}
				base.ImageIndex = 12;
			}
		}

		private class TreeViewWithEnter : global::System.Windows.Forms.TreeView
		{
			protected override bool IsInputKey(Keys keyCode)
			{
				return keyCode == Keys.Return || base.IsInputKey(keyCode);
			}
		}

		private class ListViewWithEnter : ListView
		{
			protected override bool IsInputKey(Keys keyCode)
			{
				return keyCode == Keys.Return || base.IsInputKey(keyCode);
			}
		}

		private class CustomFieldItem : DataControlFieldsEditor.FieldItem
		{
			public CustomFieldItem(DataControlFieldsEditor fieldsEditor, DataControlField runtimeField)
				: base(fieldsEditor, runtimeField, 3)
			{
			}
		}

		private sealed class DataControlFieldDesignerItem : DataControlFieldsEditor.FieldItem
		{
			public DataControlFieldDesignerItem(DataControlFieldDesigner fieldDesigner, DataControlField runtimeField)
				: base(null, runtimeField, 15)
			{
				this._fieldDesigner = fieldDesigner;
				base.Text = this.GetDefaultNodeText();
			}

			protected override string GetDefaultNodeText()
			{
				if (this._fieldDesigner != null)
				{
					return this._fieldDesigner.GetNodeText(base.RuntimeField);
				}
				return base.GetDefaultNodeText();
			}

			public override TemplateField GetTemplateField(DataBoundControl dataBoundControl)
			{
				if (this._fieldDesigner != null)
				{
					return this._fieldDesigner.CreateTemplateField(base.RuntimeField, dataBoundControl);
				}
				return base.GetTemplateField(dataBoundControl);
			}

			private DataControlFieldDesigner _fieldDesigner;
		}

		private sealed class DataControlFieldDesignerNode : DataControlFieldsEditor.AvailableFieldNode
		{
			public DataControlFieldDesignerNode(DataControlFieldDesigner fieldDesigner)
				: base(fieldDesigner.DefaultNodeText, 15)
			{
				this._fieldDesigner = fieldDesigner;
			}

			public DataControlFieldDesignerNode(DataControlFieldDesigner fieldDesigner, IDataSourceFieldSchema fieldSchema)
				: base((fieldSchema == null) ? fieldDesigner.DefaultNodeText : fieldSchema.Name, 15)
			{
				this._fieldSchema = fieldSchema;
				this._fieldDesigner = fieldDesigner;
			}

			public override DataControlFieldsEditor.FieldItem CreateField()
			{
				DataControlField dataControlField = ((this._fieldSchema == null) ? this._fieldDesigner.CreateField() : this._fieldDesigner.CreateField(this._fieldSchema));
				DataControlFieldsEditor.FieldItem fieldItem = new DataControlFieldsEditor.DataControlFieldDesignerItem(this._fieldDesigner, dataControlField);
				fieldItem.LoadFieldInfo();
				return fieldItem;
			}

			private IDataSourceFieldSchema _fieldSchema;

			private DataControlFieldDesigner _fieldDesigner;
		}
	}
}
