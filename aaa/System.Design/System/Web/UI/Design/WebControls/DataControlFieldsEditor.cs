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
	// Token: 0x0200041F RID: 1055
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed partial class DataControlFieldsEditor : DesignerForm
	{
		// Token: 0x060026A5 RID: 9893 RVA: 0x000D1760 File Offset: 0x000D0760
		public DataControlFieldsEditor(DataBoundControlDesigner controlDesigner)
			: base(controlDesigner.Component.Site)
		{
			this._controlDesigner = controlDesigner;
			this.InitializeComponent();
			this.InitForm();
			this._initialActivate = true;
			this.IgnoreRefreshSchemaEvents();
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x060026A6 RID: 9894 RVA: 0x000D1793 File Offset: 0x000D0793
		// (set) Token: 0x060026A7 RID: 9895 RVA: 0x000D17D2 File Offset: 0x000D07D2
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

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x060026A8 RID: 9896 RVA: 0x000D1811 File Offset: 0x000D0811
		private DataBoundControl Control
		{
			get
			{
				return this._controlDesigner.Component as DataBoundControl;
			}
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x060026A9 RID: 9897 RVA: 0x000D1824 File Offset: 0x000D0824
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

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x060026AA RID: 9898 RVA: 0x000D18F6 File Offset: 0x000D08F6
		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.DataControlField.DataControlFieldEditor";
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x060026AB RID: 9899 RVA: 0x000D18FD File Offset: 0x000D08FD
		// (set) Token: 0x060026AC RID: 9900 RVA: 0x000D193C File Offset: 0x000D093C
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

		// Token: 0x060026AD RID: 9901 RVA: 0x000D197A File Offset: 0x000D097A
		private void EnterLoadingMode()
		{
			this._isLoading = true;
		}

		// Token: 0x060026AE RID: 9902 RVA: 0x000D1983 File Offset: 0x000D0983
		private void ExitLoadingMode()
		{
			this._isLoading = false;
		}

		// Token: 0x060026AF RID: 9903 RVA: 0x000D198C File Offset: 0x000D098C
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

		// Token: 0x060026B0 RID: 9904 RVA: 0x000D19B0 File Offset: 0x000D09B0
		private string GetNewDataSourceName(Type controlType, int editMode)
		{
			int num = 1;
			return this.GetNewDataSourceName(controlType, editMode, ref num);
		}

		// Token: 0x060026B1 RID: 9905 RVA: 0x000D19CC File Offset: 0x000D09CC
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

		// Token: 0x060026B2 RID: 9906 RVA: 0x000D1B40 File Offset: 0x000D0B40
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

		// Token: 0x060026B3 RID: 9907 RVA: 0x000D1BDC File Offset: 0x000D0BDC
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

		// Token: 0x060026B4 RID: 9908 RVA: 0x000D1C10 File Offset: 0x000D0C10
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

		// Token: 0x060026B6 RID: 9910 RVA: 0x000D25B4 File Offset: 0x000D15B4
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

		// Token: 0x060026B7 RID: 9911 RVA: 0x000D2868 File Offset: 0x000D1868
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

		// Token: 0x060026B8 RID: 9912 RVA: 0x000D28C0 File Offset: 0x000D18C0
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

		// Token: 0x060026B9 RID: 9913 RVA: 0x000D2A8E File Offset: 0x000D1A8E
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

		// Token: 0x060026BA RID: 9914 RVA: 0x000D2AC8 File Offset: 0x000D1AC8
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

		// Token: 0x060026BB RID: 9915 RVA: 0x000D2BD4 File Offset: 0x000D1BD4
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

		// Token: 0x060026BC RID: 9916 RVA: 0x000D2D30 File Offset: 0x000D1D30
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

		// Token: 0x060026BD RID: 9917 RVA: 0x000D2EB9 File Offset: 0x000D1EB9
		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			if (this._initialActivate)
			{
				this.LoadComponent();
				this._initialActivate = false;
			}
		}

		// Token: 0x060026BE RID: 9918 RVA: 0x000D2ED7 File Offset: 0x000D1ED7
		private void OnAvailableFieldsDoubleClick(object source, TreeNodeMouseClickEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this.OnClickAddField(source, e);
			}
		}

		// Token: 0x060026BF RID: 9919 RVA: 0x000D2EEE File Offset: 0x000D1EEE
		private void OnAvailableFieldsGotFocus(object source, EventArgs e)
		{
			this._currentFieldProps.SelectedObject = null;
		}

		// Token: 0x060026C0 RID: 9920 RVA: 0x000D2EFC File Offset: 0x000D1EFC
		private void OnAvailableFieldsKeyPress(object source, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				this.OnClickAddField(source, e);
				e.Handled = true;
			}
		}

		// Token: 0x060026C1 RID: 9921 RVA: 0x000D2F18 File Offset: 0x000D1F18
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

		// Token: 0x060026C2 RID: 9922 RVA: 0x000D2FB9 File Offset: 0x000D1FB9
		private void OnCheckChangedAutoField(object source, EventArgs e)
		{
			if (this._isLoading)
			{
				return;
			}
			this.UpdateEnabledVisibleState();
		}

		// Token: 0x060026C3 RID: 9923 RVA: 0x000D2FCC File Offset: 0x000D1FCC
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

		// Token: 0x060026C4 RID: 9924 RVA: 0x000D30EC File Offset: 0x000D20EC
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

		// Token: 0x060026C5 RID: 9925 RVA: 0x000D318C File Offset: 0x000D218C
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

		// Token: 0x060026C6 RID: 9926 RVA: 0x000D324C File Offset: 0x000D224C
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

		// Token: 0x060026C7 RID: 9927 RVA: 0x000D32E6 File Offset: 0x000D22E6
		private void OnClickOK(object source, EventArgs e)
		{
			this.SaveComponent();
			this.PersistClonedFieldsToControl();
		}

		// Token: 0x060026C8 RID: 9928 RVA: 0x000D32F4 File Offset: 0x000D22F4
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

		// Token: 0x060026C9 RID: 9929 RVA: 0x000D33B4 File Offset: 0x000D23B4
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

		// Token: 0x060026CA RID: 9930 RVA: 0x000D3424 File Offset: 0x000D2424
		protected override void OnClosed(EventArgs e)
		{
			IDataSourceDesigner dataSourceDesigner = this._controlDesigner.DataSourceDesigner;
			if (dataSourceDesigner != null)
			{
				dataSourceDesigner.ResumeDataSourceEvents();
			}
			this.IgnoreRefreshSchema = this._initialIgnoreRefreshSchemaValue;
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x000D3452 File Offset: 0x000D2452
		private void OnSelChangedAvailableFields(object source, TreeViewEventArgs e)
		{
			this.UpdateEnabledVisibleState();
		}

		// Token: 0x060026CC RID: 9932 RVA: 0x000D345A File Offset: 0x000D245A
		private void OnSelFieldsListGotFocus(object source, EventArgs e)
		{
			this.UpdateEnabledVisibleState();
		}

		// Token: 0x060026CD RID: 9933 RVA: 0x000D3462 File Offset: 0x000D2462
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

		// Token: 0x060026CE RID: 9934 RVA: 0x000D3488 File Offset: 0x000D2488
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

		// Token: 0x060026CF RID: 9935 RVA: 0x000D34F0 File Offset: 0x000D24F0
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

		// Token: 0x060026D0 RID: 9936 RVA: 0x000D3594 File Offset: 0x000D2594
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

		// Token: 0x060026D1 RID: 9937 RVA: 0x000D35E4 File Offset: 0x000D25E4
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

		// Token: 0x060026D2 RID: 9938 RVA: 0x000D3660 File Offset: 0x000D2660
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

		// Token: 0x060026D3 RID: 9939 RVA: 0x000D380C File Offset: 0x000D280C
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

		// Token: 0x060026D4 RID: 9940 RVA: 0x000D396C File Offset: 0x000D296C
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

		// Token: 0x04001A8D RID: 6797
		private const int ILI_DATASOURCE = 0;

		// Token: 0x04001A8E RID: 6798
		private const int ILI_BOUND = 1;

		// Token: 0x04001A8F RID: 6799
		private const int ILI_ALL = 2;

		// Token: 0x04001A90 RID: 6800
		private const int ILI_CUSTOM = 3;

		// Token: 0x04001A91 RID: 6801
		private const int ILI_BUTTON = 4;

		// Token: 0x04001A92 RID: 6802
		private const int ILI_SELECTBUTTON = 5;

		// Token: 0x04001A93 RID: 6803
		private const int ILI_EDITBUTTON = 6;

		// Token: 0x04001A94 RID: 6804
		private const int ILI_DELETEBUTTON = 7;

		// Token: 0x04001A95 RID: 6805
		private const int ILI_HYPERLINK = 8;

		// Token: 0x04001A96 RID: 6806
		private const int ILI_TEMPLATE = 9;

		// Token: 0x04001A97 RID: 6807
		private const int ILI_CHECKBOX = 10;

		// Token: 0x04001A98 RID: 6808
		private const int ILI_INSERTBUTTON = 11;

		// Token: 0x04001A99 RID: 6809
		private const int ILI_COMMAND = 12;

		// Token: 0x04001A9A RID: 6810
		private const int ILI_BOOLDATASOURCE = 13;

		// Token: 0x04001A9B RID: 6811
		private const int ILI_IMAGE = 14;

		// Token: 0x04001A9C RID: 6812
		private const int ILI_FIELDDESIGNER = 15;

		// Token: 0x04001A9D RID: 6813
		private const int CF_EDIT = 0;

		// Token: 0x04001A9E RID: 6814
		private const int CF_INSERT = 1;

		// Token: 0x04001A9F RID: 6815
		private const int CF_SELECT = 2;

		// Token: 0x04001AA0 RID: 6816
		private const int CF_DELETE = 3;

		// Token: 0x04001AA1 RID: 6817
		private const int MODE_READONLY = 0;

		// Token: 0x04001AA2 RID: 6818
		private const int MODE_EDIT = 1;

		// Token: 0x04001AA3 RID: 6819
		private const int MODE_INSERT = 2;

		// Token: 0x04001AB3 RID: 6835
		private DataControlFieldsEditor.DataSourceNode _selectedDataSourceNode;

		// Token: 0x04001AB4 RID: 6836
		private DataControlFieldsEditor.BoolDataSourceNode _selectedCheckBoxDataSourceNode;

		// Token: 0x04001AB5 RID: 6837
		private DataControlFieldsEditor.FieldItem _currentFieldItem;

		// Token: 0x04001AB6 RID: 6838
		private bool _propChangesPending;

		// Token: 0x04001AB7 RID: 6839
		private bool _fieldMovePending;

		// Token: 0x04001AB8 RID: 6840
		private DataControlFieldCollection _clonedFieldCollection;

		// Token: 0x04001AB9 RID: 6841
		private DataBoundControlDesigner _controlDesigner;

		// Token: 0x04001ABA RID: 6842
		private bool _isLoading;

		// Token: 0x04001ABB RID: 6843
		private IDataSourceFieldSchema[] _fieldSchemas;

		// Token: 0x04001ABC RID: 6844
		private IDataSourceViewSchema _viewSchema;

		// Token: 0x04001ABD RID: 6845
		private bool _initialActivate;

		// Token: 0x04001ABE RID: 6846
		private bool _initialIgnoreRefreshSchemaValue;

		// Token: 0x04001ABF RID: 6847
		private IDictionary<Type, DataControlFieldDesigner> _customFieldDesigners;

		// Token: 0x02000420 RID: 1056
		private abstract class AvailableFieldNode : global::System.Windows.Forms.TreeNode
		{
			// Token: 0x060026D5 RID: 9941 RVA: 0x000D39E9 File Offset: 0x000D29E9
			public AvailableFieldNode(string text, int icon)
				: base(text, icon, icon)
			{
			}

			// Token: 0x1700072E RID: 1838
			// (get) Token: 0x060026D6 RID: 9942 RVA: 0x000D39F4 File Offset: 0x000D29F4
			public virtual bool CreatesMultipleFields
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700072F RID: 1839
			// (get) Token: 0x060026D7 RID: 9943 RVA: 0x000D39F7 File Offset: 0x000D29F7
			public virtual bool IsFieldCreator
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060026D8 RID: 9944 RVA: 0x000D39FA File Offset: 0x000D29FA
			public virtual DataControlFieldsEditor.FieldItem CreateField()
			{
				return null;
			}

			// Token: 0x060026D9 RID: 9945 RVA: 0x000D39FD File Offset: 0x000D29FD
			public virtual DataControlFieldsEditor.FieldItem[] CreateFields(DataBoundControl control, IDataSourceFieldSchema[] fieldSchemas)
			{
				return null;
			}
		}

		// Token: 0x02000421 RID: 1057
		private class DataSourceNode : DataControlFieldsEditor.AvailableFieldNode
		{
			// Token: 0x060026DA RID: 9946 RVA: 0x000D3A00 File Offset: 0x000D2A00
			public DataSourceNode()
				: base(SR.GetString("DCFEditor_Node_Bound"), 0)
			{
			}

			// Token: 0x060026DB RID: 9947 RVA: 0x000D3A13 File Offset: 0x000D2A13
			public DataSourceNode(string nodeText)
				: base(nodeText, 0)
			{
			}

			// Token: 0x17000730 RID: 1840
			// (get) Token: 0x060026DC RID: 9948 RVA: 0x000D3A1D File Offset: 0x000D2A1D
			public override bool IsFieldCreator
			{
				get
				{
					return false;
				}
			}
		}

		// Token: 0x02000422 RID: 1058
		private class BoolDataSourceNode : DataControlFieldsEditor.AvailableFieldNode
		{
			// Token: 0x060026DD RID: 9949 RVA: 0x000D3A20 File Offset: 0x000D2A20
			public BoolDataSourceNode()
				: base(SR.GetString("DCFEditor_Node_CheckBox"), 13)
			{
			}

			// Token: 0x17000731 RID: 1841
			// (get) Token: 0x060026DE RID: 9950 RVA: 0x000D3A34 File Offset: 0x000D2A34
			public override bool IsFieldCreator
			{
				get
				{
					return false;
				}
			}
		}

		// Token: 0x02000423 RID: 1059
		private class DataFieldNode : DataControlFieldsEditor.AvailableFieldNode
		{
			// Token: 0x060026DF RID: 9951 RVA: 0x000D3A37 File Offset: 0x000D2A37
			public DataFieldNode(DataControlFieldsEditor fieldsEditor)
				: base(SR.GetString("DCFEditor_Node_AllFields"), 2)
			{
				this._fieldsEditor = fieldsEditor;
			}

			// Token: 0x17000732 RID: 1842
			// (get) Token: 0x060026E0 RID: 9952 RVA: 0x000D3A51 File Offset: 0x000D2A51
			public override bool CreatesMultipleFields
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060026E1 RID: 9953 RVA: 0x000D3A54 File Offset: 0x000D2A54
			public override DataControlFieldsEditor.FieldItem CreateField()
			{
				throw new NotSupportedException();
			}

			// Token: 0x060026E2 RID: 9954 RVA: 0x000D3A5C File Offset: 0x000D2A5C
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

			// Token: 0x04001AC0 RID: 6848
			private DataControlFieldsEditor _fieldsEditor;
		}

		// Token: 0x02000424 RID: 1060
		private class BoundNode : DataControlFieldsEditor.AvailableFieldNode
		{
			// Token: 0x060026E3 RID: 9955 RVA: 0x000D3BA0 File Offset: 0x000D2BA0
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

			// Token: 0x060026E4 RID: 9956 RVA: 0x000D3BEC File Offset: 0x000D2BEC
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

			// Token: 0x04001AC1 RID: 6849
			protected IDataSourceFieldSchema _fieldSchema;

			// Token: 0x04001AC2 RID: 6850
			private bool _genericBoundField;

			// Token: 0x04001AC3 RID: 6851
			private DataControlFieldsEditor _fieldsEditor;
		}

		// Token: 0x02000425 RID: 1061
		private class ButtonNode : DataControlFieldsEditor.AvailableFieldNode
		{
			// Token: 0x060026E5 RID: 9957 RVA: 0x000D3C7A File Offset: 0x000D2C7A
			public ButtonNode(DataControlFieldsEditor fieldsEditor)
				: this(fieldsEditor, string.Empty, SR.GetString("DCFEditor_Button"), SR.GetString("DCFEditor_Node_Button"))
			{
			}

			// Token: 0x060026E6 RID: 9958 RVA: 0x000D3C9C File Offset: 0x000D2C9C
			public ButtonNode(DataControlFieldsEditor fieldsEditor, string command, string buttonText, string text)
				: base(text, 4)
			{
				this._fieldsEditor = fieldsEditor;
				this.command = command;
				this.buttonText = buttonText;
			}

			// Token: 0x060026E7 RID: 9959 RVA: 0x000D3CBC File Offset: 0x000D2CBC
			public override DataControlFieldsEditor.FieldItem CreateField()
			{
				ButtonField buttonField = new ButtonField();
				buttonField.Text = this.buttonText;
				buttonField.CommandName = this.command;
				DataControlFieldsEditor.FieldItem fieldItem = new DataControlFieldsEditor.ButtonFieldItem(this._fieldsEditor, buttonField);
				fieldItem.LoadFieldInfo();
				return fieldItem;
			}

			// Token: 0x04001AC4 RID: 6852
			private string command;

			// Token: 0x04001AC5 RID: 6853
			private string buttonText;

			// Token: 0x04001AC6 RID: 6854
			private DataControlFieldsEditor _fieldsEditor;
		}

		// Token: 0x02000426 RID: 1062
		private class CheckBoxNode : DataControlFieldsEditor.AvailableFieldNode
		{
			// Token: 0x060026E8 RID: 9960 RVA: 0x000D3CFC File Offset: 0x000D2CFC
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

			// Token: 0x060026E9 RID: 9961 RVA: 0x000D3D4C File Offset: 0x000D2D4C
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

			// Token: 0x04001AC7 RID: 6855
			protected IDataSourceFieldSchema _fieldSchema;

			// Token: 0x04001AC8 RID: 6856
			private bool _genericCheckBoxField;

			// Token: 0x04001AC9 RID: 6857
			private DataControlFieldsEditor _fieldsEditor;
		}

		// Token: 0x02000427 RID: 1063
		private class ImageNode : DataControlFieldsEditor.AvailableFieldNode
		{
			// Token: 0x060026EA RID: 9962 RVA: 0x000D3DDA File Offset: 0x000D2DDA
			public ImageNode(DataControlFieldsEditor fieldsEditor)
				: base(string.Empty, 14)
			{
				this._fieldsEditor = fieldsEditor;
				base.Text = SR.GetString("DCFEditor_Node_Image");
			}

			// Token: 0x060026EB RID: 9963 RVA: 0x000D3E00 File Offset: 0x000D2E00
			public override DataControlFieldsEditor.FieldItem CreateField()
			{
				ImageField imageField = new ImageField();
				DataControlFieldsEditor.FieldItem fieldItem = new DataControlFieldsEditor.ImageFieldItem(this._fieldsEditor, imageField);
				fieldItem.LoadFieldInfo();
				return fieldItem;
			}

			// Token: 0x04001ACA RID: 6858
			private DataControlFieldsEditor _fieldsEditor;
		}

		// Token: 0x02000428 RID: 1064
		private class CommandNode : DataControlFieldsEditor.AvailableFieldNode
		{
			// Token: 0x060026EC RID: 9964 RVA: 0x000D3E27 File Offset: 0x000D2E27
			public CommandNode(DataControlFieldsEditor fieldsEditor)
				: this(fieldsEditor, -1, SR.GetString("DCFEditor_Node_Command"), 12)
			{
			}

			// Token: 0x060026ED RID: 9965 RVA: 0x000D3E3D File Offset: 0x000D2E3D
			public CommandNode(DataControlFieldsEditor fieldsEditor, int commandType, string text, int icon)
				: base(text, icon)
			{
				this.commandType = commandType;
				this._fieldsEditor = fieldsEditor;
			}

			// Token: 0x060026EE RID: 9966 RVA: 0x000D3E58 File Offset: 0x000D2E58
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

			// Token: 0x04001ACB RID: 6859
			private int commandType;

			// Token: 0x04001ACC RID: 6860
			private DataControlFieldsEditor _fieldsEditor;
		}

		// Token: 0x02000429 RID: 1065
		private class HyperLinkNode : DataControlFieldsEditor.AvailableFieldNode
		{
			// Token: 0x060026EF RID: 9967 RVA: 0x000D3EC0 File Offset: 0x000D2EC0
			public HyperLinkNode(DataControlFieldsEditor fieldsEditor)
				: this(fieldsEditor, SR.GetString("DCFEditor_HyperLink"))
			{
			}

			// Token: 0x060026F0 RID: 9968 RVA: 0x000D3ED3 File Offset: 0x000D2ED3
			public HyperLinkNode(DataControlFieldsEditor fieldsEditor, string hyperLinkText)
				: base(SR.GetString("DCFEditor_Node_HyperLink"), 8)
			{
				this._fieldsEditor = fieldsEditor;
				this.hyperLinkText = hyperLinkText;
			}

			// Token: 0x060026F1 RID: 9969 RVA: 0x000D3EF4 File Offset: 0x000D2EF4
			public override DataControlFieldsEditor.FieldItem CreateField()
			{
				HyperLinkField hyperLinkField = new HyperLinkField();
				DataControlFieldsEditor.FieldItem fieldItem = new DataControlFieldsEditor.HyperLinkFieldItem(this._fieldsEditor, hyperLinkField);
				fieldItem.Text = this.hyperLinkText;
				fieldItem.LoadFieldInfo();
				return fieldItem;
			}

			// Token: 0x04001ACD RID: 6861
			private string hyperLinkText;

			// Token: 0x04001ACE RID: 6862
			private DataControlFieldsEditor _fieldsEditor;
		}

		// Token: 0x0200042A RID: 1066
		private class TemplateNode : DataControlFieldsEditor.AvailableFieldNode
		{
			// Token: 0x060026F2 RID: 9970 RVA: 0x000D3F27 File Offset: 0x000D2F27
			public TemplateNode(DataControlFieldsEditor fieldsEditor)
				: base(SR.GetString("DCFEditor_Node_Template"), 9)
			{
				this._fieldsEditor = fieldsEditor;
			}

			// Token: 0x060026F3 RID: 9971 RVA: 0x000D3F44 File Offset: 0x000D2F44
			public override DataControlFieldsEditor.FieldItem CreateField()
			{
				TemplateField templateField = new TemplateField();
				DataControlFieldsEditor.FieldItem fieldItem = new DataControlFieldsEditor.TemplateFieldItem(this._fieldsEditor, templateField);
				fieldItem.LoadFieldInfo();
				return fieldItem;
			}

			// Token: 0x04001ACF RID: 6863
			private DataControlFieldsEditor _fieldsEditor;
		}

		// Token: 0x0200042B RID: 1067
		private abstract class FieldItem : ListViewItem
		{
			// Token: 0x060026F4 RID: 9972 RVA: 0x000D3F6B File Offset: 0x000D2F6B
			public FieldItem(DataControlFieldsEditor fieldsEditor, DataControlField runtimeField, int image)
				: base(string.Empty, image)
			{
				this.fieldsEditor = fieldsEditor;
				this.runtimeField = runtimeField;
				base.Text = this.GetNodeText(null);
			}

			// Token: 0x17000733 RID: 1843
			// (get) Token: 0x060026F5 RID: 9973 RVA: 0x000D3F94 File Offset: 0x000D2F94
			// (set) Token: 0x060026F6 RID: 9974 RVA: 0x000D3FA1 File Offset: 0x000D2FA1
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

			// Token: 0x17000734 RID: 1844
			// (get) Token: 0x060026F7 RID: 9975 RVA: 0x000D3FB5 File Offset: 0x000D2FB5
			public DataControlField RuntimeField
			{
				get
				{
					return this.runtimeField;
				}
			}

			// Token: 0x060026F8 RID: 9976 RVA: 0x000D3FBD File Offset: 0x000D2FBD
			protected virtual string GetDefaultNodeText()
			{
				return this.runtimeField.GetType().Name;
			}

			// Token: 0x060026F9 RID: 9977 RVA: 0x000D3FCF File Offset: 0x000D2FCF
			public virtual string GetNodeText(string headerText)
			{
				if (headerText == null || headerText.Length == 0)
				{
					return this.GetDefaultNodeText();
				}
				return headerText;
			}

			// Token: 0x060026FA RID: 9978 RVA: 0x000D3FE4 File Offset: 0x000D2FE4
			protected ITemplate GetTemplate(DataBoundControl control, string templateContent)
			{
				return DataControlFieldHelper.GetTemplate(control, templateContent);
			}

			// Token: 0x060026FB RID: 9979 RVA: 0x000D3FED File Offset: 0x000D2FED
			public virtual TemplateField GetTemplateField(DataBoundControl dataBoundControl)
			{
				return DataControlFieldHelper.GetTemplateField(this.runtimeField, dataBoundControl);
			}

			// Token: 0x060026FC RID: 9980 RVA: 0x000D3FFB File Offset: 0x000D2FFB
			public virtual void LoadFieldInfo()
			{
				this.UpdateDisplayText();
			}

			// Token: 0x060026FD RID: 9981 RVA: 0x000D4003 File Offset: 0x000D3003
			protected string PrepareFormatString(string formatString)
			{
				return formatString.Replace("'", "&#039;");
			}

			// Token: 0x060026FE RID: 9982 RVA: 0x000D4015 File Offset: 0x000D3015
			protected void UpdateDisplayText()
			{
				base.Text = this.GetNodeText(this.HeaderText);
			}

			// Token: 0x04001AD0 RID: 6864
			protected DataControlField runtimeField;

			// Token: 0x04001AD1 RID: 6865
			protected DataControlFieldsEditor fieldsEditor;
		}

		// Token: 0x0200042C RID: 1068
		private class BoundFieldItem : DataControlFieldsEditor.FieldItem
		{
			// Token: 0x060026FF RID: 9983 RVA: 0x000D4029 File Offset: 0x000D3029
			public BoundFieldItem(DataControlFieldsEditor fieldsEditor, BoundField runtimeField)
				: base(fieldsEditor, runtimeField, 1)
			{
			}

			// Token: 0x06002700 RID: 9984 RVA: 0x000D4034 File Offset: 0x000D3034
			protected override string GetDefaultNodeText()
			{
				string dataField = ((BoundField)base.RuntimeField).DataField;
				if (dataField != null && dataField.Length != 0)
				{
					return dataField;
				}
				return SR.GetString("DCFEditor_Node_Bound");
			}

			// Token: 0x06002701 RID: 9985 RVA: 0x000D406C File Offset: 0x000D306C
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

			// Token: 0x06002702 RID: 9986 RVA: 0x000D4114 File Offset: 0x000D3114
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

		// Token: 0x0200042D RID: 1069
		private class ButtonFieldItem : DataControlFieldsEditor.FieldItem
		{
			// Token: 0x06002703 RID: 9987 RVA: 0x000D4268 File Offset: 0x000D3268
			public ButtonFieldItem(DataControlFieldsEditor fieldsEditor, ButtonField runtimeField)
				: base(fieldsEditor, runtimeField, 4)
			{
			}

			// Token: 0x06002704 RID: 9988 RVA: 0x000D4274 File Offset: 0x000D3274
			protected override string GetDefaultNodeText()
			{
				string text = ((ButtonField)this.runtimeField).Text;
				if (text != null && text.Length != 0)
				{
					return text;
				}
				return SR.GetString("DCFEditor_Node_Button");
			}

			// Token: 0x06002705 RID: 9989 RVA: 0x000D42AC File Offset: 0x000D32AC
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

		// Token: 0x0200042E RID: 1070
		private class CheckBoxFieldItem : DataControlFieldsEditor.FieldItem
		{
			// Token: 0x06002706 RID: 9990 RVA: 0x000D4451 File Offset: 0x000D3451
			public CheckBoxFieldItem(DataControlFieldsEditor fieldsEditor, CheckBoxField runtimeField)
				: base(fieldsEditor, runtimeField, 10)
			{
			}

			// Token: 0x06002707 RID: 9991 RVA: 0x000D4460 File Offset: 0x000D3460
			protected override string GetDefaultNodeText()
			{
				string dataField = ((CheckBoxField)base.RuntimeField).DataField;
				if (dataField != null && dataField.Length != 0)
				{
					return dataField;
				}
				return SR.GetString("DCFEditor_Node_CheckBox");
			}

			// Token: 0x06002708 RID: 9992 RVA: 0x000D4498 File Offset: 0x000D3498
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

			// Token: 0x06002709 RID: 9993 RVA: 0x000D4524 File Offset: 0x000D3524
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

		// Token: 0x0200042F RID: 1071
		private class ImageFieldItem : DataControlFieldsEditor.FieldItem
		{
			// Token: 0x0600270A RID: 9994 RVA: 0x000D4620 File Offset: 0x000D3620
			public ImageFieldItem(DataControlFieldsEditor fieldsEditor, ImageField runtimeField)
				: base(fieldsEditor, runtimeField, 14)
			{
			}

			// Token: 0x0600270B RID: 9995 RVA: 0x000D462C File Offset: 0x000D362C
			protected override string GetDefaultNodeText()
			{
				string dataImageUrlField = ((ImageField)base.RuntimeField).DataImageUrlField;
				if (dataImageUrlField != null && dataImageUrlField.Length != 0)
				{
					return dataImageUrlField;
				}
				return SR.GetString("DCFEditor_Node_Image");
			}

			// Token: 0x0600270C RID: 9996 RVA: 0x000D4664 File Offset: 0x000D3664
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

			// Token: 0x0600270D RID: 9997 RVA: 0x000D46EC File Offset: 0x000D36EC
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

		// Token: 0x02000430 RID: 1072
		private class HyperLinkFieldItem : DataControlFieldsEditor.FieldItem
		{
			// Token: 0x0600270E RID: 9998 RVA: 0x000D48B1 File Offset: 0x000D38B1
			public HyperLinkFieldItem(DataControlFieldsEditor fieldsEditor, HyperLinkField runtimeField)
				: base(fieldsEditor, runtimeField, 8)
			{
			}

			// Token: 0x0600270F RID: 9999 RVA: 0x000D48BC File Offset: 0x000D38BC
			protected override string GetDefaultNodeText()
			{
				string text = ((HyperLinkField)base.RuntimeField).Text;
				if (text != null && text.Length != 0)
				{
					return text;
				}
				return SR.GetString("DCFEditor_Node_HyperLink");
			}

			// Token: 0x06002710 RID: 10000 RVA: 0x000D48F4 File Offset: 0x000D38F4
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

		// Token: 0x02000431 RID: 1073
		private class TemplateFieldItem : DataControlFieldsEditor.FieldItem
		{
			// Token: 0x06002711 RID: 10001 RVA: 0x000D4AB9 File Offset: 0x000D3AB9
			public TemplateFieldItem(DataControlFieldsEditor fieldsEditor, TemplateField runtimeField)
				: base(fieldsEditor, runtimeField, 9)
			{
			}

			// Token: 0x06002712 RID: 10002 RVA: 0x000D4AC5 File Offset: 0x000D3AC5
			protected override string GetDefaultNodeText()
			{
				return SR.GetString("DCFEditor_Node_Template");
			}
		}

		// Token: 0x02000432 RID: 1074
		private class CommandFieldItem : DataControlFieldsEditor.FieldItem
		{
			// Token: 0x06002713 RID: 10003 RVA: 0x000D4AD1 File Offset: 0x000D3AD1
			public CommandFieldItem(DataControlFieldsEditor fieldsEditor, CommandField runtimeField)
				: base(fieldsEditor, runtimeField, 12)
			{
				this.UpdateImageIndex();
			}

			// Token: 0x06002714 RID: 10004 RVA: 0x000D4AE4 File Offset: 0x000D3AE4
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

			// Token: 0x06002715 RID: 10005 RVA: 0x000D4BDC File Offset: 0x000D3BDC
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

			// Token: 0x06002716 RID: 10006 RVA: 0x000D4CAC File Offset: 0x000D3CAC
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

			// Token: 0x06002717 RID: 10007 RVA: 0x000D4D08 File Offset: 0x000D3D08
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

			// Token: 0x06002718 RID: 10008 RVA: 0x000D4FDC File Offset: 0x000D3FDC
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

		// Token: 0x02000433 RID: 1075
		private class TreeViewWithEnter : global::System.Windows.Forms.TreeView
		{
			// Token: 0x06002719 RID: 10009 RVA: 0x000D509E File Offset: 0x000D409E
			protected override bool IsInputKey(Keys keyCode)
			{
				return keyCode == Keys.Return || base.IsInputKey(keyCode);
			}
		}

		// Token: 0x02000434 RID: 1076
		private class ListViewWithEnter : ListView
		{
			// Token: 0x0600271B RID: 10011 RVA: 0x000D50B6 File Offset: 0x000D40B6
			protected override bool IsInputKey(Keys keyCode)
			{
				return keyCode == Keys.Return || base.IsInputKey(keyCode);
			}
		}

		// Token: 0x02000435 RID: 1077
		private class CustomFieldItem : DataControlFieldsEditor.FieldItem
		{
			// Token: 0x0600271D RID: 10013 RVA: 0x000D50CE File Offset: 0x000D40CE
			public CustomFieldItem(DataControlFieldsEditor fieldsEditor, DataControlField runtimeField)
				: base(fieldsEditor, runtimeField, 3)
			{
			}
		}

		// Token: 0x02000436 RID: 1078
		private sealed class DataControlFieldDesignerItem : DataControlFieldsEditor.FieldItem
		{
			// Token: 0x0600271E RID: 10014 RVA: 0x000D50D9 File Offset: 0x000D40D9
			public DataControlFieldDesignerItem(DataControlFieldDesigner fieldDesigner, DataControlField runtimeField)
				: base(null, runtimeField, 15)
			{
				this._fieldDesigner = fieldDesigner;
				base.Text = this.GetDefaultNodeText();
			}

			// Token: 0x0600271F RID: 10015 RVA: 0x000D50F8 File Offset: 0x000D40F8
			protected override string GetDefaultNodeText()
			{
				if (this._fieldDesigner != null)
				{
					return this._fieldDesigner.GetNodeText(base.RuntimeField);
				}
				return base.GetDefaultNodeText();
			}

			// Token: 0x06002720 RID: 10016 RVA: 0x000D511A File Offset: 0x000D411A
			public override TemplateField GetTemplateField(DataBoundControl dataBoundControl)
			{
				if (this._fieldDesigner != null)
				{
					return this._fieldDesigner.CreateTemplateField(base.RuntimeField, dataBoundControl);
				}
				return base.GetTemplateField(dataBoundControl);
			}

			// Token: 0x04001AD2 RID: 6866
			private DataControlFieldDesigner _fieldDesigner;
		}

		// Token: 0x02000437 RID: 1079
		private sealed class DataControlFieldDesignerNode : DataControlFieldsEditor.AvailableFieldNode
		{
			// Token: 0x06002721 RID: 10017 RVA: 0x000D513E File Offset: 0x000D413E
			public DataControlFieldDesignerNode(DataControlFieldDesigner fieldDesigner)
				: base(fieldDesigner.DefaultNodeText, 15)
			{
				this._fieldDesigner = fieldDesigner;
			}

			// Token: 0x06002722 RID: 10018 RVA: 0x000D5155 File Offset: 0x000D4155
			public DataControlFieldDesignerNode(DataControlFieldDesigner fieldDesigner, IDataSourceFieldSchema fieldSchema)
				: base((fieldSchema == null) ? fieldDesigner.DefaultNodeText : fieldSchema.Name, 15)
			{
				this._fieldSchema = fieldSchema;
				this._fieldDesigner = fieldDesigner;
			}

			// Token: 0x06002723 RID: 10019 RVA: 0x000D5180 File Offset: 0x000D4180
			public override DataControlFieldsEditor.FieldItem CreateField()
			{
				DataControlField dataControlField = ((this._fieldSchema == null) ? this._fieldDesigner.CreateField() : this._fieldDesigner.CreateField(this._fieldSchema));
				DataControlFieldsEditor.FieldItem fieldItem = new DataControlFieldsEditor.DataControlFieldDesignerItem(this._fieldDesigner, dataControlField);
				fieldItem.LoadFieldInfo();
				return fieldItem;
			}

			// Token: 0x04001AD3 RID: 6867
			private IDataSourceFieldSchema _fieldSchema;

			// Token: 0x04001AD4 RID: 6868
			private DataControlFieldDesigner _fieldDesigner;
		}
	}
}
