using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Security.Permissions;
using System.Text;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls.ListControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class DataGridColumnsPage : BaseDataListPage
	{
		protected override string HelpKeyword
		{
			get
			{
				return "net.Asp.DataGridProperties.Columns";
			}
		}

		private void InitForm()
		{
			this.autoColumnCheck = new global::System.Windows.Forms.CheckBox();
			GroupLabel groupLabel = new GroupLabel();
			global::System.Windows.Forms.Label label = new global::System.Windows.Forms.Label();
			this.availableColumnsTree = new global::System.Windows.Forms.TreeView();
			this.addColumnButton = new global::System.Windows.Forms.Button();
			global::System.Windows.Forms.Label label2 = new global::System.Windows.Forms.Label();
			this.selColumnsList = new ListView();
			this.moveColumnUpButton = new global::System.Windows.Forms.Button();
			this.moveColumnDownButton = new global::System.Windows.Forms.Button();
			this.deleteColumnButton = new global::System.Windows.Forms.Button();
			this.columnPropsGroup = new GroupLabel();
			global::System.Windows.Forms.Label label3 = new global::System.Windows.Forms.Label();
			this.columnHeaderTextEdit = new global::System.Windows.Forms.TextBox();
			global::System.Windows.Forms.Label label4 = new global::System.Windows.Forms.Label();
			this.columnHeaderImageEdit = new global::System.Windows.Forms.TextBox();
			this.columnHeaderImagePickerButton = new global::System.Windows.Forms.Button();
			global::System.Windows.Forms.Label label5 = new global::System.Windows.Forms.Label();
			this.columnFooterTextEdit = new global::System.Windows.Forms.TextBox();
			global::System.Windows.Forms.Label label6 = new global::System.Windows.Forms.Label();
			this.columnSortExprCombo = new ComboBox();
			this.columnVisibleCheck = new global::System.Windows.Forms.CheckBox();
			this.boundColumnEditor = new DataGridColumnsPage.BoundColumnEditor();
			this.buttonColumnEditor = new DataGridColumnsPage.ButtonColumnEditor();
			this.hyperLinkColumnEditor = new DataGridColumnsPage.HyperLinkColumnEditor();
			this.editCommandColumnEditor = new DataGridColumnsPage.EditCommandColumnEditor();
			this.templatizeLink = new LinkLabel();
			global::System.Drawing.Image image = new Bitmap(base.GetType(), "ColumnNodes.bmp");
			ImageList imageList = new ImageList();
			imageList.TransparentColor = Color.Magenta;
			imageList.Images.AddStrip(image);
			this.autoColumnCheck.SetBounds(4, 4, 400, 16);
			this.autoColumnCheck.Text = SR.GetString("DGCol_AutoGen");
			this.autoColumnCheck.TabIndex = 0;
			this.autoColumnCheck.TextAlign = ContentAlignment.MiddleLeft;
			this.autoColumnCheck.FlatStyle = FlatStyle.System;
			this.autoColumnCheck.CheckedChanged += this.OnCheckChangedAutoColumn;
			this.autoColumnCheck.Name = "AutoColumnCheckBox";
			groupLabel.SetBounds(4, 24, 431, 14);
			groupLabel.Text = SR.GetString("DGCol_ColListGroup");
			groupLabel.TabStop = false;
			groupLabel.TabIndex = 1;
			groupLabel.Name = "ColumnListGroup";
			label.SetBounds(12, 40, 184, 16);
			label.Text = SR.GetString("DGCol_AvailableCols");
			label.TabStop = false;
			label.TabIndex = 2;
			label.Name = "AvailableColumnsLabel";
			this.availableColumnsTree.SetBounds(12, 58, 170, 88);
			this.availableColumnsTree.ImageList = imageList;
			this.availableColumnsTree.Indent = 5;
			this.availableColumnsTree.HideSelection = false;
			this.availableColumnsTree.TabIndex = 3;
			this.availableColumnsTree.AfterSelect += this.OnSelChangedAvailableColumns;
			this.availableColumnsTree.Name = "AvailableColumnsTree";
			this.addColumnButton.SetBounds(187, 82, 31, 24);
			this.addColumnButton.Text = ">";
			this.addColumnButton.TabIndex = 4;
			this.addColumnButton.FlatStyle = FlatStyle.System;
			this.addColumnButton.Click += this.OnClickAddColumn;
			this.addColumnButton.Name = "AddColumnButton";
			this.addColumnButton.AccessibleName = SR.GetString("DGCol_AddColButtonDesc");
			label2.SetBounds(226, 40, 200, 14);
			label2.Text = SR.GetString("DGCol_SelectedCols");
			label2.TabStop = false;
			label2.TabIndex = 5;
			label2.Name = "SelectedColumnsLabel";
			ColumnHeader columnHeader = new ColumnHeader();
			columnHeader.Width = 176;
			this.selColumnsList.SetBounds(222, 58, 180, 88);
			this.selColumnsList.Columns.Add(columnHeader);
			this.selColumnsList.SmallImageList = imageList;
			this.selColumnsList.View = global::System.Windows.Forms.View.Details;
			this.selColumnsList.HeaderStyle = ColumnHeaderStyle.None;
			this.selColumnsList.LabelWrap = false;
			this.selColumnsList.HideSelection = false;
			this.selColumnsList.MultiSelect = false;
			this.selColumnsList.TabIndex = 6;
			this.selColumnsList.SelectedIndexChanged += this.OnSelIndexChangedSelColumnsList;
			this.selColumnsList.KeyDown += this.OnSelColumnsListKeyDown;
			this.selColumnsList.Name = "SelectedColumnsList";
			this.moveColumnUpButton.SetBounds(406, 58, 28, 27);
			this.moveColumnUpButton.TabIndex = 7;
			Bitmap bitmap = new Icon(base.GetType(), "SortUp.ico").ToBitmap();
			bitmap.MakeTransparent();
			this.moveColumnUpButton.Image = bitmap;
			this.moveColumnUpButton.Click += this.OnClickMoveColumnUp;
			this.moveColumnUpButton.Name = "MoveColumnUpButton";
			this.moveColumnUpButton.AccessibleName = SR.GetString("DGCol_MoveColumnUpButtonDesc");
			this.moveColumnDownButton.SetBounds(406, 88, 28, 27);
			this.moveColumnDownButton.TabIndex = 8;
			Bitmap bitmap2 = new Icon(base.GetType(), "SortDown.ico").ToBitmap();
			bitmap2.MakeTransparent();
			this.moveColumnDownButton.Image = bitmap2;
			this.moveColumnDownButton.Click += this.OnClickMoveColumnDown;
			this.moveColumnDownButton.Name = "MoveColumnDownButton";
			this.moveColumnDownButton.AccessibleName = SR.GetString("DGCol_MoveColumnDownButtonDesc");
			this.deleteColumnButton.SetBounds(406, 118, 28, 27);
			this.deleteColumnButton.TabIndex = 9;
			Bitmap bitmap3 = new Icon(base.GetType(), "Delete.ico").ToBitmap();
			bitmap3.MakeTransparent();
			this.deleteColumnButton.Image = bitmap3;
			this.deleteColumnButton.Click += this.OnClickDeleteColumn;
			this.deleteColumnButton.Name = "DeleteColumnButton";
			this.deleteColumnButton.AccessibleName = SR.GetString("DGCol_DeleteColumnButtonDesc");
			this.columnPropsGroup.SetBounds(8, 150, 431, 14);
			this.columnPropsGroup.Text = SR.GetString("DGCol_ColumnPropsGroup1");
			this.columnPropsGroup.TabStop = false;
			this.columnPropsGroup.TabIndex = 10;
			label3.SetBounds(20, 166, 180, 14);
			label3.Text = SR.GetString("DGCol_HeaderText");
			label3.TabStop = false;
			label3.TabIndex = 11;
			label3.Name = "ColumnHeaderTextLabel";
			this.columnHeaderTextEdit.SetBounds(20, 182, 182, 24);
			this.columnHeaderTextEdit.TabIndex = 12;
			this.columnHeaderTextEdit.TextChanged += this.OnTextChangedColHeaderText;
			this.columnHeaderTextEdit.LostFocus += this.OnLostFocusColHeaderText;
			this.columnHeaderTextEdit.Name = "ColumnHeaderTextEdit";
			label4.SetBounds(20, 208, 180, 14);
			label4.Text = SR.GetString("DGCol_HeaderImage");
			label4.TabStop = false;
			label4.TabIndex = 13;
			label4.Name = "ColumnHeaderImageLabel";
			this.columnHeaderImageEdit.SetBounds(20, 224, 156, 24);
			this.columnHeaderImageEdit.TabIndex = 14;
			this.columnHeaderImageEdit.TextChanged += this.OnChangedColumnProperties;
			this.columnHeaderImageEdit.Name = "ColumnHeaderImageEdit";
			this.columnHeaderImagePickerButton.SetBounds(180, 223, 24, 23);
			this.columnHeaderImagePickerButton.Text = "...";
			this.columnHeaderImagePickerButton.TabIndex = 15;
			this.columnHeaderImagePickerButton.FlatStyle = FlatStyle.System;
			this.columnHeaderImagePickerButton.Click += this.OnClickColHeaderImagePicker;
			this.columnHeaderImagePickerButton.Name = "ColumnHeaderImagePickerButton";
			this.columnHeaderImagePickerButton.AccessibleName = SR.GetString("DGCol_HeaderImagePickerDesc");
			label5.SetBounds(220, 166, 180, 14);
			label5.Text = SR.GetString("DGCol_FooterText");
			label5.TabStop = false;
			label5.TabIndex = 16;
			label5.Name = "ColumnFooterTextLabel";
			this.columnFooterTextEdit.SetBounds(220, 182, 182, 24);
			this.columnFooterTextEdit.TabIndex = 17;
			this.columnFooterTextEdit.TextChanged += this.OnChangedColumnProperties;
			this.columnFooterTextEdit.Name = "ColumnFooterTextEdit";
			label6.SetBounds(220, 208, 144, 16);
			label6.Text = SR.GetString("DGCol_SortExpr");
			label6.TabStop = false;
			label6.TabIndex = 18;
			label6.Name = "ColumnSortExprLabel";
			this.columnSortExprCombo.SetBounds(220, 224, 140, 21);
			this.columnSortExprCombo.TabIndex = 19;
			this.columnSortExprCombo.TextChanged += this.OnChangedColumnProperties;
			this.columnSortExprCombo.SelectedIndexChanged += this.OnChangedColumnProperties;
			this.columnSortExprCombo.Name = "ColumnSortExprCombo";
			this.columnVisibleCheck.SetBounds(368, 222, 100, 40);
			this.columnVisibleCheck.Text = SR.GetString("DGCol_Visible");
			this.columnVisibleCheck.TabIndex = 20;
			this.columnVisibleCheck.FlatStyle = FlatStyle.System;
			this.columnVisibleCheck.CheckAlign = ContentAlignment.TopLeft;
			this.columnVisibleCheck.TextAlign = ContentAlignment.TopLeft;
			this.columnVisibleCheck.CheckedChanged += this.OnChangedColumnProperties;
			this.columnVisibleCheck.Name = "ColumnVisibleCheckBox";
			this.boundColumnEditor.SetBounds(20, 250, 416, 164);
			this.boundColumnEditor.TabIndex = 21;
			this.boundColumnEditor.Visible = false;
			this.boundColumnEditor.Changed += this.OnChangedColumnProperties;
			this.buttonColumnEditor.SetBounds(20, 250, 416, 164);
			this.buttonColumnEditor.TabIndex = 22;
			this.buttonColumnEditor.Visible = false;
			this.buttonColumnEditor.Changed += this.OnChangedColumnProperties;
			this.hyperLinkColumnEditor.SetBounds(20, 250, 416, 164);
			this.hyperLinkColumnEditor.TabIndex = 23;
			this.hyperLinkColumnEditor.Visible = false;
			this.hyperLinkColumnEditor.Changed += this.OnChangedColumnProperties;
			this.editCommandColumnEditor.SetBounds(20, 250, 416, 164);
			this.editCommandColumnEditor.TabIndex = 24;
			this.editCommandColumnEditor.Visible = false;
			this.editCommandColumnEditor.Changed += this.OnChangedColumnProperties;
			this.templatizeLink.SetBounds(18, 414, 400, 16);
			this.templatizeLink.TabIndex = 25;
			this.templatizeLink.Text = SR.GetString("DGCol_Templatize");
			this.templatizeLink.Visible = false;
			this.templatizeLink.LinkClicked += this.OnClickTemplatize;
			this.templatizeLink.Name = "TemplatizeLink";
			this.Text = SR.GetString("DGCol_Text");
			base.AccessibleDescription = SR.GetString("DGCol_Desc");
			base.Size = new Size(464, 432);
			base.CommitOnDeactivate = true;
			base.Icon = new Icon(base.GetType(), "DataGridColumnsPage.ico");
			base.Controls.Clear();
			base.Controls.AddRange(new Control[]
			{
				this.templatizeLink, this.editCommandColumnEditor, this.hyperLinkColumnEditor, this.buttonColumnEditor, this.boundColumnEditor, this.columnVisibleCheck, this.columnSortExprCombo, label6, this.columnFooterTextEdit, label5,
				this.columnHeaderImagePickerButton, this.columnHeaderImageEdit, label4, this.columnHeaderTextEdit, label3, this.columnPropsGroup, this.deleteColumnButton, this.moveColumnDownButton, this.moveColumnUpButton, this.selColumnsList,
				label2, this.addColumnButton, this.availableColumnsTree, label, groupLabel, this.autoColumnCheck
			});
		}

		private void InitPage()
		{
			this.currentDataSource = null;
			this.autoColumnCheck.Checked = false;
			this.selectedDataSourceNode = null;
			this.availableColumnsTree.Nodes.Clear();
			this.selColumnsList.Items.Clear();
			this.currentColumnItem = null;
			this.columnSortExprCombo.Items.Clear();
			this.currentColumnEditor = null;
			this.boundColumnEditor.ClearDataFields();
			this.buttonColumnEditor.ClearDataFields();
			this.hyperLinkColumnEditor.ClearDataFields();
			this.editCommandColumnEditor.ClearDataFields();
			this.propChangesPending = false;
			this.headerTextChanged = false;
		}

		private void LoadColumnProperties()
		{
			string text = SR.GetString("DGCol_ColumnPropsGroup1");
			if (this.currentColumnItem != null)
			{
				base.EnterLoadingMode();
				this.columnHeaderTextEdit.Text = this.currentColumnItem.HeaderText;
				this.columnHeaderImageEdit.Text = this.currentColumnItem.HeaderImageUrl;
				this.columnFooterTextEdit.Text = this.currentColumnItem.FooterText;
				this.columnSortExprCombo.Text = this.currentColumnItem.SortExpression;
				this.columnVisibleCheck.Checked = this.currentColumnItem.Visible;
				this.currentColumnEditor = null;
				if (this.currentColumnItem is DataGridColumnsPage.BoundColumnItem)
				{
					this.currentColumnEditor = this.boundColumnEditor;
					text = SR.GetString("DGCol_ColumnPropsGroup2", new object[] { "BoundColumn" });
				}
				else if (this.currentColumnItem is DataGridColumnsPage.ButtonColumnItem)
				{
					this.currentColumnEditor = this.buttonColumnEditor;
					text = SR.GetString("DGCol_ColumnPropsGroup2", new object[] { "ButtonColumn" });
				}
				else if (this.currentColumnItem is DataGridColumnsPage.HyperLinkColumnItem)
				{
					this.currentColumnEditor = this.hyperLinkColumnEditor;
					text = SR.GetString("DGCol_ColumnPropsGroup2", new object[] { "HyperLinkColumn" });
				}
				else if (this.currentColumnItem is DataGridColumnsPage.EditCommandColumnItem)
				{
					this.currentColumnEditor = this.editCommandColumnEditor;
					text = SR.GetString("DGCol_ColumnPropsGroup2", new object[] { "EditCommandColumn" });
				}
				else if (this.currentColumnItem is DataGridColumnsPage.TemplateColumnItem)
				{
					text = SR.GetString("DGCol_ColumnPropsGroup2", new object[] { "TemplateColumn" });
				}
				if (this.currentColumnEditor != null)
				{
					this.currentColumnEditor.LoadColumn(this.currentColumnItem);
				}
				base.ExitLoadingMode();
			}
			this.columnPropsGroup.Text = text;
		}

		private void LoadColumns()
		{
			global::System.Web.UI.WebControls.DataGrid dataGrid = (global::System.Web.UI.WebControls.DataGrid)base.GetBaseControl();
			DataGridColumnCollection columns = dataGrid.Columns;
			if (columns != null)
			{
				int count = columns.Count;
				for (int i = 0; i < count; i++)
				{
					DataGridColumn dataGridColumn = columns[i];
					DataGridColumnsPage.ColumnItem columnItem;
					if (dataGridColumn is BoundColumn)
					{
						columnItem = new DataGridColumnsPage.BoundColumnItem((BoundColumn)dataGridColumn);
					}
					else if (dataGridColumn is ButtonColumn)
					{
						columnItem = new DataGridColumnsPage.ButtonColumnItem((ButtonColumn)dataGridColumn);
					}
					else if (dataGridColumn is HyperLinkColumn)
					{
						columnItem = new DataGridColumnsPage.HyperLinkColumnItem((HyperLinkColumn)dataGridColumn);
					}
					else if (dataGridColumn is TemplateColumn)
					{
						columnItem = new DataGridColumnsPage.TemplateColumnItem((TemplateColumn)dataGridColumn);
					}
					else if (dataGridColumn is EditCommandColumn)
					{
						columnItem = new DataGridColumnsPage.EditCommandColumnItem((EditCommandColumn)dataGridColumn);
					}
					else
					{
						columnItem = new DataGridColumnsPage.CustomColumnItem(dataGridColumn);
					}
					columnItem.LoadColumnInfo();
					this.selColumnsList.Items.Add(columnItem);
				}
				if (this.selColumnsList.Items.Count != 0)
				{
					this.currentColumnItem = (DataGridColumnsPage.ColumnItem)this.selColumnsList.Items[0];
					this.currentColumnItem.Selected = true;
				}
			}
		}

		protected override void LoadComponent()
		{
			this.InitPage();
			global::System.Web.UI.WebControls.DataGrid dataGrid = (global::System.Web.UI.WebControls.DataGrid)base.GetBaseControl();
			this.LoadDataSourceItem();
			this.LoadAvailableColumnsTree();
			this.LoadDataSourceFields();
			this.autoColumnCheck.Checked = dataGrid.AutoGenerateColumns;
			this.LoadColumns();
			this.UpdateEnabledVisibleState();
		}

		private void LoadDataSourceItem()
		{
			global::System.Web.UI.WebControls.DataGrid dataGrid = (global::System.Web.UI.WebControls.DataGrid)base.GetBaseControl();
			DataGridDesigner dataGridDesigner = (DataGridDesigner)base.GetBaseDesigner();
			string dataSource = dataGridDesigner.DataSource;
			if (dataSource != null)
			{
				ISite site = dataGrid.Site;
				IContainer container = (IContainer)site.GetService(typeof(IContainer));
				if (container != null)
				{
					IComponent component = container.Components[dataSource];
					if (component != null)
					{
						if (component is IListSource)
						{
							this.currentDataSource = new BaseDataListPage.ListSourceDataSourceItem(dataSource, (IListSource)component)
							{
								CurrentDataMember = dataGridDesigner.DataMember
							};
							return;
						}
						if (component is IEnumerable)
						{
							this.currentDataSource = new BaseDataListPage.DataSourceItem(dataSource, (IEnumerable)component);
						}
					}
				}
			}
		}

		private void LoadDataSourceFields()
		{
			base.EnterLoadingMode();
			if (this.currentDataSource != null)
			{
				PropertyDescriptorCollection fields = this.currentDataSource.Fields;
				if (fields != null)
				{
					int count = fields.Count;
					if (count > 0)
					{
						DataGridColumnsPage.DataFieldNode dataFieldNode = new DataGridColumnsPage.DataFieldNode();
						this.selectedDataSourceNode.Nodes.Add(dataFieldNode);
						foreach (object obj in fields)
						{
							PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
							if (BaseDataList.IsBindableType(propertyDescriptor.PropertyType))
							{
								string name = propertyDescriptor.Name;
								DataGridColumnsPage.DataFieldNode dataFieldNode2 = new DataGridColumnsPage.DataFieldNode(name);
								this.selectedDataSourceNode.Nodes.Add(dataFieldNode2);
								this.boundColumnEditor.AddDataField(name);
								this.buttonColumnEditor.AddDataField(name);
								this.hyperLinkColumnEditor.AddDataField(name);
								this.editCommandColumnEditor.AddDataField(name);
								this.columnSortExprCombo.Items.Add(name);
							}
						}
						this.availableColumnsTree.SelectedNode = dataFieldNode;
						dataFieldNode.EnsureVisible();
					}
				}
			}
			else
			{
				DataGridColumnsPage.DataFieldNode dataFieldNode3 = new DataGridColumnsPage.DataFieldNode(null);
				this.availableColumnsTree.Nodes.Insert(0, dataFieldNode3);
				this.availableColumnsTree.SelectedNode = dataFieldNode3;
				dataFieldNode3.EnsureVisible();
			}
			base.ExitLoadingMode();
		}

		private void LoadAvailableColumnsTree()
		{
			if (this.currentDataSource != null)
			{
				this.selectedDataSourceNode = new DataGridColumnsPage.DataSourceNode();
				this.availableColumnsTree.Nodes.Add(this.selectedDataSourceNode);
			}
			DataGridColumnsPage.ButtonNode buttonNode = new DataGridColumnsPage.ButtonNode();
			this.availableColumnsTree.Nodes.Add(buttonNode);
			DataGridColumnsPage.ButtonNode buttonNode2 = new DataGridColumnsPage.ButtonNode("Select", SR.GetString("DGCol_SelectButton"), SR.GetString("DGCol_Node_Select"));
			buttonNode.Nodes.Add(buttonNode2);
			DataGridColumnsPage.EditCommandNode editCommandNode = new DataGridColumnsPage.EditCommandNode();
			buttonNode.Nodes.Add(editCommandNode);
			DataGridColumnsPage.ButtonNode buttonNode3 = new DataGridColumnsPage.ButtonNode("Delete", SR.GetString("DGCol_DeleteButton"), SR.GetString("DGCol_Node_Delete"));
			buttonNode.Nodes.Add(buttonNode3);
			DataGridColumnsPage.HyperLinkNode hyperLinkNode = new DataGridColumnsPage.HyperLinkNode();
			this.availableColumnsTree.Nodes.Add(hyperLinkNode);
			DataGridColumnsPage.TemplateNode templateNode = new DataGridColumnsPage.TemplateNode();
			this.availableColumnsTree.Nodes.Add(templateNode);
		}

		private void OnChangedColumnProperties(object source, EventArgs e)
		{
			if (base.IsLoading())
			{
				return;
			}
			this.propChangesPending = true;
			this.SetDirty();
		}

		private void OnCheckChangedAutoColumn(object source, EventArgs e)
		{
			if (base.IsLoading())
			{
				return;
			}
			this.SetDirty();
			this.UpdateEnabledVisibleState();
		}

		private void OnClickAddColumn(object source, EventArgs e)
		{
			DataGridColumnsPage.AvailableColumnNode availableColumnNode = (DataGridColumnsPage.AvailableColumnNode)this.availableColumnsTree.SelectedNode;
			if (this.propChangesPending)
			{
				this.SaveColumnProperties();
			}
			if (!availableColumnNode.CreatesMultipleColumns)
			{
				DataGridColumnsPage.ColumnItem columnItem = availableColumnNode.CreateColumn();
				this.selColumnsList.Items.Add(columnItem);
				this.currentColumnItem = columnItem;
				this.currentColumnItem.Selected = true;
				this.currentColumnItem.EnsureVisible();
			}
			else
			{
				DataGridColumnsPage.ColumnItem[] array = availableColumnNode.CreateColumns(this.currentDataSource.Fields);
				int num = array.Length;
				for (int i = 0; i < num; i++)
				{
					this.selColumnsList.Items.Add(array[i]);
				}
				this.currentColumnItem = array[num - 1];
				this.currentColumnItem.Selected = true;
				this.currentColumnItem.EnsureVisible();
			}
			this.selColumnsList.Focus();
			this.SetDirty();
			this.UpdateEnabledVisibleState();
		}

		private void OnClickColHeaderImagePicker(object source, EventArgs e)
		{
			string text = this.columnHeaderImageEdit.Text.Trim();
			string @string = SR.GetString("DGCol_URLPCaption");
			string string2 = SR.GetString("DGCol_URLPFilter");
			text = UrlBuilder.BuildUrl(base.GetBaseControl(), this, text, @string, string2);
			if (text != null)
			{
				this.columnHeaderImageEdit.Text = text;
				this.OnChangedColumnProperties(this.columnHeaderImageEdit, EventArgs.Empty);
			}
		}

		private void OnClickDeleteColumn(object source, EventArgs e)
		{
			int index = this.currentColumnItem.Index;
			int num = -1;
			int count = this.selColumnsList.Items.Count;
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
			this.propChangesPending = false;
			this.currentColumnItem.Remove();
			this.currentColumnItem = null;
			if (num != -1)
			{
				this.currentColumnItem = (DataGridColumnsPage.ColumnItem)this.selColumnsList.Items[num];
				this.currentColumnItem.Selected = true;
				this.currentColumnItem.EnsureVisible();
			}
			this.SetDirty();
			this.UpdateEnabledVisibleState();
		}

		private void OnClickMoveColumnDown(object source, EventArgs e)
		{
			if (this.propChangesPending)
			{
				this.SaveColumnProperties();
			}
			int index = this.currentColumnItem.Index;
			ListViewItem listViewItem = this.selColumnsList.Items[index];
			this.selColumnsList.Items.RemoveAt(index);
			this.selColumnsList.Items.Insert(index + 1, listViewItem);
			this.currentColumnItem = (DataGridColumnsPage.ColumnItem)this.selColumnsList.Items[index + 1];
			this.currentColumnItem.Selected = true;
			this.currentColumnItem.EnsureVisible();
			this.SetDirty();
			this.UpdateEnabledVisibleState();
		}

		private void OnClickMoveColumnUp(object source, EventArgs e)
		{
			if (this.propChangesPending)
			{
				this.SaveColumnProperties();
			}
			int index = this.currentColumnItem.Index;
			ListViewItem listViewItem = this.selColumnsList.Items[index];
			this.selColumnsList.Items.RemoveAt(index);
			this.selColumnsList.Items.Insert(index - 1, listViewItem);
			this.currentColumnItem = (DataGridColumnsPage.ColumnItem)this.selColumnsList.Items[index - 1];
			this.currentColumnItem.Selected = true;
			this.currentColumnItem.EnsureVisible();
			this.SetDirty();
			this.UpdateEnabledVisibleState();
		}

		private void OnClickTemplatize(object source, LinkLabelLinkClickedEventArgs e)
		{
			if (this.currentColumnItem == null)
			{
				return;
			}
			if (this.propChangesPending)
			{
				this.SaveColumnProperties();
			}
			this.currentColumnItem.SaveColumnInfo();
			TemplateColumn templateColumn = this.currentColumnItem.GetTemplateColumn((global::System.Web.UI.WebControls.DataGrid)base.GetBaseControl());
			DataGridColumnsPage.TemplateColumnItem templateColumnItem = new DataGridColumnsPage.TemplateColumnItem(templateColumn);
			templateColumnItem.LoadColumnInfo();
			this.selColumnsList.Items[this.currentColumnItem.Index] = templateColumnItem;
			this.currentColumnItem = templateColumnItem;
			this.currentColumnItem.Selected = true;
			this.SetDirty();
			this.UpdateEnabledVisibleState();
		}

		private void OnLostFocusColHeaderText(object source, EventArgs e)
		{
			if (this.headerTextChanged)
			{
				this.headerTextChanged = false;
				if (this.currentColumnItem != null)
				{
					this.currentColumnItem.HeaderText = this.columnHeaderTextEdit.Text;
				}
			}
		}

		private void OnSelChangedAvailableColumns(object source, TreeViewEventArgs e)
		{
			this.UpdateEnabledVisibleState();
		}

		private void OnSelColumnsListKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Delete && this.currentColumnItem != null)
			{
				this.OnClickDeleteColumn(sender, e);
			}
		}

		private void OnSelIndexChangedSelColumnsList(object source, EventArgs e)
		{
			if (this.propChangesPending)
			{
				this.SaveColumnProperties();
			}
			if (this.selColumnsList.SelectedItems.Count == 0)
			{
				this.currentColumnItem = null;
			}
			else
			{
				this.currentColumnItem = (DataGridColumnsPage.ColumnItem)this.selColumnsList.SelectedItems[0];
			}
			this.LoadColumnProperties();
			this.UpdateEnabledVisibleState();
		}

		private void OnTextChangedColHeaderText(object source, EventArgs e)
		{
			if (base.IsLoading())
			{
				return;
			}
			this.headerTextChanged = true;
			this.propChangesPending = true;
			this.SetDirty();
		}

		private void SaveColumnProperties()
		{
			if (this.currentColumnItem != null)
			{
				this.currentColumnItem.HeaderText = this.columnHeaderTextEdit.Text;
				this.currentColumnItem.HeaderImageUrl = this.columnHeaderImageEdit.Text.Trim();
				this.currentColumnItem.FooterText = this.columnFooterTextEdit.Text;
				this.currentColumnItem.SortExpression = this.columnSortExprCombo.Text.Trim();
				this.currentColumnItem.Visible = this.columnVisibleCheck.Checked;
				if (this.currentColumnEditor != null)
				{
					this.currentColumnEditor.SaveColumn();
				}
			}
			this.propChangesPending = false;
		}

		protected override void SaveComponent()
		{
			if (this.propChangesPending)
			{
				this.SaveColumnProperties();
			}
			global::System.Web.UI.WebControls.DataGrid dataGrid = (global::System.Web.UI.WebControls.DataGrid)base.GetBaseControl();
			DataGridDesigner dataGridDesigner = (DataGridDesigner)base.GetBaseDesigner();
			dataGrid.AutoGenerateColumns = this.autoColumnCheck.Checked;
			DataGridColumnCollection columns = dataGrid.Columns;
			columns.Clear();
			int count = this.selColumnsList.Items.Count;
			for (int i = 0; i < count; i++)
			{
				DataGridColumnsPage.ColumnItem columnItem = (DataGridColumnsPage.ColumnItem)this.selColumnsList.Items[i];
				columnItem.SaveColumnInfo();
				columns.Add(columnItem.RuntimeColumn);
			}
			dataGridDesigner.OnColumnsChanged();
		}

		public override void SetComponent(IComponent component)
		{
			base.SetComponent(component);
			this.InitForm();
		}

		private void UpdateEnabledVisibleState()
		{
			DataGridColumnsPage.AvailableColumnNode availableColumnNode = (DataGridColumnsPage.AvailableColumnNode)this.availableColumnsTree.SelectedNode;
			int count = this.selColumnsList.Items.Count;
			int count2 = this.selColumnsList.SelectedItems.Count;
			DataGridColumnsPage.ColumnItem columnItem = null;
			int num = -1;
			if (count2 != 0)
			{
				columnItem = (DataGridColumnsPage.ColumnItem)this.selColumnsList.SelectedItems[0];
			}
			if (columnItem != null)
			{
				num = columnItem.Index;
			}
			bool flag = num != -1;
			this.addColumnButton.Enabled = availableColumnNode != null && availableColumnNode.IsColumnCreator;
			this.moveColumnUpButton.Enabled = num > 0;
			this.moveColumnDownButton.Enabled = num >= 0 && num < count - 1;
			this.deleteColumnButton.Enabled = flag;
			this.columnHeaderTextEdit.Enabled = flag;
			this.columnHeaderImageEdit.Enabled = flag;
			this.columnHeaderImagePickerButton.Enabled = flag;
			this.columnFooterTextEdit.Enabled = flag;
			this.columnSortExprCombo.Enabled = flag;
			this.columnVisibleCheck.Enabled = flag;
			this.boundColumnEditor.Visible = this.currentColumnEditor == this.boundColumnEditor && flag;
			this.buttonColumnEditor.Visible = this.currentColumnEditor == this.buttonColumnEditor && flag;
			this.hyperLinkColumnEditor.Visible = this.currentColumnEditor == this.hyperLinkColumnEditor && flag;
			this.editCommandColumnEditor.Visible = this.currentColumnEditor == this.editCommandColumnEditor && flag;
			this.templatizeLink.Visible = count != 0 && (this.boundColumnEditor.Visible || this.buttonColumnEditor.Visible || this.hyperLinkColumnEditor.Visible || this.editCommandColumnEditor.Visible);
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

		private global::System.Windows.Forms.CheckBox autoColumnCheck;

		private global::System.Windows.Forms.TreeView availableColumnsTree;

		private global::System.Windows.Forms.Button addColumnButton;

		private ListView selColumnsList;

		private global::System.Windows.Forms.Button moveColumnUpButton;

		private global::System.Windows.Forms.Button moveColumnDownButton;

		private global::System.Windows.Forms.Button deleteColumnButton;

		private GroupLabel columnPropsGroup;

		private global::System.Windows.Forms.TextBox columnHeaderTextEdit;

		private global::System.Windows.Forms.TextBox columnHeaderImageEdit;

		private global::System.Windows.Forms.TextBox columnFooterTextEdit;

		private ComboBox columnSortExprCombo;

		private global::System.Windows.Forms.CheckBox columnVisibleCheck;

		private global::System.Windows.Forms.Button columnHeaderImagePickerButton;

		private LinkLabel templatizeLink;

		private DataGridColumnsPage.BoundColumnEditor boundColumnEditor;

		private DataGridColumnsPage.ButtonColumnEditor buttonColumnEditor;

		private DataGridColumnsPage.HyperLinkColumnEditor hyperLinkColumnEditor;

		private DataGridColumnsPage.EditCommandColumnEditor editCommandColumnEditor;

		private BaseDataListPage.DataSourceItem currentDataSource;

		private DataGridColumnsPage.DataSourceNode selectedDataSourceNode;

		private DataGridColumnsPage.ColumnItem currentColumnItem;

		private DataGridColumnsPage.ColumnItemEditor currentColumnEditor;

		private bool propChangesPending;

		private bool headerTextChanged;

		private abstract class AvailableColumnNode : global::System.Windows.Forms.TreeNode
		{
			public AvailableColumnNode(string text, int icon)
				: base(text, icon, icon)
			{
			}

			public virtual bool CreatesMultipleColumns
			{
				get
				{
					return false;
				}
			}

			public virtual bool IsColumnCreator
			{
				get
				{
					return true;
				}
			}

			public virtual DataGridColumnsPage.ColumnItem CreateColumn()
			{
				return null;
			}

			public virtual DataGridColumnsPage.ColumnItem[] CreateColumns(PropertyDescriptorCollection fields)
			{
				return null;
			}
		}

		private class DataSourceNode : DataGridColumnsPage.AvailableColumnNode
		{
			public DataSourceNode()
				: base(SR.GetString("DGCol_Node_DataFields"), 0)
			{
			}

			public override bool IsColumnCreator
			{
				get
				{
					return false;
				}
			}
		}

		private class DataFieldNode : DataGridColumnsPage.AvailableColumnNode
		{
			public DataFieldNode()
				: base(SR.GetString("DGCol_Node_AllFields"), 2)
			{
				this.fieldName = null;
				this.allFields = true;
			}

			public DataFieldNode(string fieldName)
				: base(fieldName, 1)
			{
				this.fieldName = fieldName;
				if (fieldName == null)
				{
					this.genericBoundColumn = true;
					base.Text = SR.GetString("DGCol_Node_Bound");
				}
			}

			public override bool CreatesMultipleColumns
			{
				get
				{
					return this.allFields;
				}
			}

			public override DataGridColumnsPage.ColumnItem CreateColumn()
			{
				BoundColumn boundColumn = new BoundColumn();
				if (!this.genericBoundColumn)
				{
					boundColumn.HeaderText = this.fieldName;
					boundColumn.DataField = this.fieldName;
					boundColumn.SortExpression = this.fieldName;
				}
				DataGridColumnsPage.ColumnItem columnItem = new DataGridColumnsPage.BoundColumnItem(boundColumn);
				columnItem.LoadColumnInfo();
				return columnItem;
			}

			public override DataGridColumnsPage.ColumnItem[] CreateColumns(PropertyDescriptorCollection fields)
			{
				ArrayList arrayList = new ArrayList();
				foreach (object obj in fields)
				{
					PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
					if (BaseDataList.IsBindableType(propertyDescriptor.PropertyType))
					{
						DataGridColumnsPage.ColumnItem columnItem = new DataGridColumnsPage.BoundColumnItem(new BoundColumn
						{
							HeaderText = propertyDescriptor.Name,
							DataField = propertyDescriptor.Name
						});
						columnItem.LoadColumnInfo();
						arrayList.Add(columnItem);
					}
				}
				return (DataGridColumnsPage.ColumnItem[])arrayList.ToArray(typeof(DataGridColumnsPage.ColumnItem));
			}

			protected string fieldName;

			private bool genericBoundColumn;

			private bool allFields;
		}

		private class ButtonNode : DataGridColumnsPage.AvailableColumnNode
		{
			public ButtonNode()
				: this(string.Empty, SR.GetString("DGCol_Button"), SR.GetString("DGCol_Node_Button"))
			{
			}

			public ButtonNode(string command, string buttonText, string text)
				: base(text, 4)
			{
				this.command = command;
				this.buttonText = buttonText;
			}

			public override DataGridColumnsPage.ColumnItem CreateColumn()
			{
				DataGridColumnsPage.ColumnItem columnItem = new DataGridColumnsPage.ButtonColumnItem(new ButtonColumn
				{
					Text = this.buttonText,
					CommandName = this.command
				});
				columnItem.LoadColumnInfo();
				return columnItem;
			}

			private string command;

			private string buttonText;
		}

		private class EditCommandNode : DataGridColumnsPage.AvailableColumnNode
		{
			public EditCommandNode()
				: base(SR.GetString("DGCol_Node_Edit"), 4)
			{
			}

			public override DataGridColumnsPage.ColumnItem CreateColumn()
			{
				DataGridColumnsPage.ColumnItem columnItem = new DataGridColumnsPage.EditCommandColumnItem(new EditCommandColumn
				{
					EditText = SR.GetString("DGCol_EditButton"),
					UpdateText = SR.GetString("DGCol_UpdateButton"),
					CancelText = SR.GetString("DGCol_CancelButton")
				});
				columnItem.LoadColumnInfo();
				return columnItem;
			}
		}

		private class HyperLinkNode : DataGridColumnsPage.AvailableColumnNode
		{
			public HyperLinkNode()
				: this(SR.GetString("DGCol_HyperLink"))
			{
			}

			public HyperLinkNode(string hyperLinkText)
				: base(SR.GetString("DGCol_Node_HyperLink"), 8)
			{
				this.hyperLinkText = hyperLinkText;
			}

			public override DataGridColumnsPage.ColumnItem CreateColumn()
			{
				HyperLinkColumn hyperLinkColumn = new HyperLinkColumn();
				DataGridColumnsPage.ColumnItem columnItem = new DataGridColumnsPage.HyperLinkColumnItem(hyperLinkColumn);
				columnItem.Text = this.hyperLinkText;
				columnItem.LoadColumnInfo();
				return columnItem;
			}

			private string hyperLinkText;
		}

		private class TemplateNode : DataGridColumnsPage.AvailableColumnNode
		{
			public TemplateNode()
				: base(SR.GetString("DGCol_Node_Template"), 9)
			{
			}

			public override DataGridColumnsPage.ColumnItem CreateColumn()
			{
				TemplateColumn templateColumn = new TemplateColumn();
				DataGridColumnsPage.ColumnItem columnItem = new DataGridColumnsPage.TemplateColumnItem(templateColumn);
				columnItem.LoadColumnInfo();
				return columnItem;
			}
		}

		private abstract class ColumnItem : ListViewItem
		{
			public ColumnItem(DataGridColumn runtimeColumn, int image)
				: base(string.Empty, image)
			{
				this.runtimeColumn = runtimeColumn;
				this.headerText = this.GetDefaultHeaderText();
				base.Text = this.GetNodeText(null);
			}

			public virtual DataGridColumnsPage.ColumnItemEditor ColumnEditor
			{
				get
				{
					return null;
				}
			}

			public string HeaderText
			{
				get
				{
					return this.headerText;
				}
				set
				{
					this.headerText = value;
					this.UpdateDisplayText();
				}
			}

			public string HeaderImageUrl
			{
				get
				{
					return this.headerImageUrl;
				}
				set
				{
					this.headerImageUrl = value;
				}
			}

			public string FooterText
			{
				get
				{
					return this.footerText;
				}
				set
				{
					this.footerText = value;
				}
			}

			public DataGridColumn RuntimeColumn
			{
				get
				{
					return this.runtimeColumn;
				}
			}

			public string SortExpression
			{
				get
				{
					return this.sortExpression;
				}
				set
				{
					this.sortExpression = value;
				}
			}

			public bool Visible
			{
				get
				{
					return this.visible;
				}
				set
				{
					this.visible = value;
				}
			}

			protected virtual string GetDefaultHeaderText()
			{
				return SR.GetString("DGCol_Node");
			}

			public virtual string GetNodeText(string headerText)
			{
				if (headerText == null || headerText.Length == 0)
				{
					return this.GetDefaultHeaderText();
				}
				return headerText;
			}

			protected ITemplate GetTemplate(global::System.Web.UI.WebControls.DataGrid dataGrid, string templateContent)
			{
				ITemplate template;
				try
				{
					ISite site = dataGrid.Site;
					IDesignerHost designerHost = (IDesignerHost)site.GetService(typeof(IDesignerHost));
					template = ControlParser.ParseTemplate(designerHost, templateContent, null);
				}
				catch (Exception)
				{
					template = null;
				}
				return template;
			}

			public virtual TemplateColumn GetTemplateColumn(global::System.Web.UI.WebControls.DataGrid dataGrid)
			{
				return new TemplateColumn
				{
					HeaderText = this.headerText,
					HeaderImageUrl = this.headerImageUrl
				};
			}

			public virtual void LoadColumnInfo()
			{
				this.headerText = this.runtimeColumn.HeaderText;
				this.headerImageUrl = this.runtimeColumn.HeaderImageUrl;
				this.footerText = this.runtimeColumn.FooterText;
				this.visible = this.runtimeColumn.Visible;
				this.sortExpression = this.runtimeColumn.SortExpression;
				this.UpdateDisplayText();
			}

			public virtual void SaveColumnInfo()
			{
				this.runtimeColumn.HeaderText = this.headerText;
				this.runtimeColumn.HeaderImageUrl = this.headerImageUrl;
				this.runtimeColumn.FooterText = this.footerText;
				this.runtimeColumn.Visible = this.visible;
				this.runtimeColumn.SortExpression = this.sortExpression;
			}

			protected void UpdateDisplayText()
			{
				base.Text = this.GetNodeText(this.headerText);
			}

			protected DataGridColumn runtimeColumn;

			protected string headerText;

			protected string headerImageUrl;

			protected string footerText;

			protected bool visible;

			protected string sortExpression;
		}

		private class BoundColumnItem : DataGridColumnsPage.ColumnItem
		{
			public BoundColumnItem(BoundColumn runtimeColumn)
				: base(runtimeColumn, 1)
			{
			}

			public string DataField
			{
				get
				{
					return this.dataField;
				}
				set
				{
					this.dataField = value;
					base.UpdateDisplayText();
				}
			}

			public string DataFormatString
			{
				get
				{
					return this.dataFormatString;
				}
				set
				{
					this.dataFormatString = value;
				}
			}

			public bool ReadOnly
			{
				get
				{
					return this.readOnly;
				}
				set
				{
					this.readOnly = value;
				}
			}

			protected override string GetDefaultHeaderText()
			{
				if (this.dataField != null && this.dataField.Length != 0)
				{
					return this.dataField;
				}
				return SR.GetString("DGCol_Node_Bound");
			}

			public override TemplateColumn GetTemplateColumn(global::System.Web.UI.WebControls.DataGrid dataGrid)
			{
				TemplateColumn templateColumn = base.GetTemplateColumn(dataGrid);
				templateColumn.ItemTemplate = base.GetTemplate(dataGrid, this.GetTemplateContent(false));
				if (!this.readOnly)
				{
					templateColumn.EditItemTemplate = base.GetTemplate(dataGrid, this.GetTemplateContent(true));
				}
				return templateColumn;
			}

			private string GetTemplateContent(bool editMode)
			{
				StringBuilder stringBuilder = new StringBuilder();
				string text = (editMode ? "TextBox" : "Label");
				stringBuilder.Append("<asp:");
				stringBuilder.Append(text);
				stringBuilder.Append(" runat=\"server\"");
				string text2 = ((BoundColumn)base.RuntimeColumn).DataField;
				if (text2.Length != 0)
				{
					stringBuilder.Append(" Text='<%# DataBinder.Eval(Container, \"DataItem.");
					stringBuilder.Append(text2);
					stringBuilder.Append("\"");
					if (this.dataFormatString.Length != 0)
					{
						stringBuilder.Append(", \"");
						stringBuilder.Append(this.dataFormatString);
						stringBuilder.Append("\"");
					}
					stringBuilder.Append(") %>'");
				}
				stringBuilder.Append("></asp:");
				stringBuilder.Append(text);
				stringBuilder.Append(">");
				return stringBuilder.ToString();
			}

			public override void LoadColumnInfo()
			{
				base.LoadColumnInfo();
				BoundColumn boundColumn = (BoundColumn)base.RuntimeColumn;
				this.dataField = boundColumn.DataField;
				this.dataFormatString = boundColumn.DataFormatString;
				this.readOnly = boundColumn.ReadOnly;
				base.UpdateDisplayText();
			}

			public override void SaveColumnInfo()
			{
				base.SaveColumnInfo();
				BoundColumn boundColumn = (BoundColumn)base.RuntimeColumn;
				boundColumn.DataField = this.dataField;
				boundColumn.DataFormatString = this.dataFormatString;
				boundColumn.ReadOnly = this.readOnly;
			}

			protected string dataField;

			protected string dataFormatString;

			protected bool readOnly;
		}

		private class ButtonColumnItem : DataGridColumnsPage.ColumnItem
		{
			public ButtonColumnItem(ButtonColumn runtimeColumn)
				: base(runtimeColumn, 4)
			{
			}

			public string Command
			{
				get
				{
					return this.command;
				}
				set
				{
					this.command = value;
				}
			}

			public string ButtonText
			{
				get
				{
					return this.buttonText;
				}
				set
				{
					this.buttonText = value;
					base.UpdateDisplayText();
				}
			}

			public ButtonColumnType ButtonType
			{
				get
				{
					return this.buttonType;
				}
				set
				{
					this.buttonType = value;
				}
			}

			public string ButtonDataTextField
			{
				get
				{
					return this.buttonDataTextField;
				}
				set
				{
					this.buttonDataTextField = value;
				}
			}

			public string ButtonDataTextFormatString
			{
				get
				{
					return this.buttonDataTextFormatString;
				}
				set
				{
					this.buttonDataTextFormatString = value;
				}
			}

			protected override string GetDefaultHeaderText()
			{
				if (this.buttonText != null && this.buttonText.Length != 0)
				{
					return this.buttonText;
				}
				return SR.GetString("DGCol_Node_Button");
			}

			public override TemplateColumn GetTemplateColumn(global::System.Web.UI.WebControls.DataGrid dataGrid)
			{
				TemplateColumn templateColumn = base.GetTemplateColumn(dataGrid);
				StringBuilder stringBuilder = new StringBuilder();
				string text = ((this.buttonType == ButtonColumnType.LinkButton) ? "LinkButton" : "Button");
				stringBuilder.Append("<asp:");
				stringBuilder.Append(text);
				stringBuilder.Append(" runat=\"server\"");
				if (this.buttonDataTextField.Length != 0)
				{
					stringBuilder.Append(" Text='<%# DataBinder.Eval(Container, \"DataItem.");
					stringBuilder.Append(this.buttonDataTextField);
					stringBuilder.Append("\"");
					if (this.buttonDataTextFormatString.Length != 0)
					{
						stringBuilder.Append(", \"");
						stringBuilder.Append(this.buttonDataTextFormatString);
						stringBuilder.Append("\"");
					}
					stringBuilder.Append(") %>'");
				}
				else
				{
					stringBuilder.Append(" Text=\"");
					stringBuilder.Append(this.buttonText);
					stringBuilder.Append("\"");
				}
				stringBuilder.Append(" CommandName=\"");
				stringBuilder.Append(this.command);
				stringBuilder.Append("\"");
				stringBuilder.Append(" CausesValidation=\"false\"></asp:");
				stringBuilder.Append(text);
				stringBuilder.Append(">");
				templateColumn.ItemTemplate = base.GetTemplate(dataGrid, stringBuilder.ToString());
				return templateColumn;
			}

			public override void LoadColumnInfo()
			{
				base.LoadColumnInfo();
				ButtonColumn buttonColumn = (ButtonColumn)base.RuntimeColumn;
				this.command = buttonColumn.CommandName;
				this.buttonText = buttonColumn.Text;
				this.buttonDataTextField = buttonColumn.DataTextField;
				this.buttonDataTextFormatString = buttonColumn.DataTextFormatString;
				this.buttonType = buttonColumn.ButtonType;
				base.UpdateDisplayText();
			}

			public override void SaveColumnInfo()
			{
				base.SaveColumnInfo();
				ButtonColumn buttonColumn = (ButtonColumn)base.RuntimeColumn;
				buttonColumn.CommandName = this.command;
				buttonColumn.Text = this.buttonText;
				buttonColumn.DataTextField = this.buttonDataTextField;
				buttonColumn.DataTextFormatString = this.buttonDataTextFormatString;
				buttonColumn.ButtonType = this.buttonType;
			}

			protected string command;

			protected string buttonText;

			protected string buttonDataTextField;

			protected string buttonDataTextFormatString;

			protected ButtonColumnType buttonType;
		}

		private class HyperLinkColumnItem : DataGridColumnsPage.ColumnItem
		{
			public HyperLinkColumnItem(HyperLinkColumn runtimeColumn)
				: base(runtimeColumn, 8)
			{
			}

			public string AnchorText
			{
				get
				{
					return this.anchorText;
				}
				set
				{
					this.anchorText = value;
					base.UpdateDisplayText();
				}
			}

			public string AnchorDataTextField
			{
				get
				{
					return this.anchorDataTextField;
				}
				set
				{
					this.anchorDataTextField = value;
				}
			}

			public string AnchorDataTextFormatString
			{
				get
				{
					return this.anchorDataTextFormatString;
				}
				set
				{
					this.anchorDataTextFormatString = value;
				}
			}

			public string Url
			{
				get
				{
					return this.url;
				}
				set
				{
					this.url = value;
				}
			}

			public string DataUrlField
			{
				get
				{
					return this.dataUrlField;
				}
				set
				{
					this.dataUrlField = value;
				}
			}

			public string DataUrlFormatString
			{
				get
				{
					return this.dataUrlFormatString;
				}
				set
				{
					this.dataUrlFormatString = value;
				}
			}

			public string Target
			{
				get
				{
					return this.target;
				}
				set
				{
					this.target = value;
				}
			}

			protected override string GetDefaultHeaderText()
			{
				if (this.anchorText != null && this.anchorText.Length != 0)
				{
					return this.anchorText;
				}
				return SR.GetString("DGCol_Node_HyperLink");
			}

			public override TemplateColumn GetTemplateColumn(global::System.Web.UI.WebControls.DataGrid dataGrid)
			{
				TemplateColumn templateColumn = base.GetTemplateColumn(dataGrid);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("<asp:HyperLink");
				stringBuilder.Append(" runat=\"server\"");
				if (this.anchorDataTextField.Length != 0)
				{
					stringBuilder.Append(" Text='<%# DataBinder.Eval(Container, \"DataItem.");
					stringBuilder.Append(this.anchorDataTextField);
					stringBuilder.Append("\"");
					if (this.anchorDataTextFormatString.Length != 0)
					{
						stringBuilder.Append(", \"");
						stringBuilder.Append(this.anchorDataTextFormatString);
						stringBuilder.Append("\"");
					}
					stringBuilder.Append(") %>'");
				}
				else
				{
					stringBuilder.Append(" Text=\"");
					stringBuilder.Append(this.anchorText);
					stringBuilder.Append("\"");
				}
				if (this.dataUrlField.Length != 0)
				{
					stringBuilder.Append(" NavigateUrl='<%# DataBinder.Eval(Container, \"DataItem.");
					stringBuilder.Append(this.dataUrlField);
					stringBuilder.Append("\"");
					if (this.dataUrlFormatString.Length != 0)
					{
						stringBuilder.Append(", \"");
						stringBuilder.Append(this.dataUrlFormatString);
						stringBuilder.Append("\"");
					}
					stringBuilder.Append(") %>'");
				}
				else
				{
					stringBuilder.Append(" NavigateUrl=\"");
					stringBuilder.Append(this.url);
					stringBuilder.Append("\"");
				}
				if (this.target.Length != 0)
				{
					stringBuilder.Append(" Target=\"");
					stringBuilder.Append(this.target);
					stringBuilder.Append("\"");
				}
				stringBuilder.Append("></asp:HyperLink>");
				templateColumn.ItemTemplate = base.GetTemplate(dataGrid, stringBuilder.ToString());
				return templateColumn;
			}

			public override void LoadColumnInfo()
			{
				base.LoadColumnInfo();
				HyperLinkColumn hyperLinkColumn = (HyperLinkColumn)base.RuntimeColumn;
				this.anchorText = hyperLinkColumn.Text;
				this.anchorDataTextField = hyperLinkColumn.DataTextField;
				this.anchorDataTextFormatString = hyperLinkColumn.DataTextFormatString;
				this.url = hyperLinkColumn.NavigateUrl;
				this.dataUrlField = hyperLinkColumn.DataNavigateUrlField;
				this.dataUrlFormatString = hyperLinkColumn.DataNavigateUrlFormatString;
				this.target = hyperLinkColumn.Target;
				base.UpdateDisplayText();
			}

			public override void SaveColumnInfo()
			{
				base.SaveColumnInfo();
				HyperLinkColumn hyperLinkColumn = (HyperLinkColumn)base.RuntimeColumn;
				hyperLinkColumn.Text = this.anchorText;
				hyperLinkColumn.DataTextField = this.anchorDataTextField;
				hyperLinkColumn.DataTextFormatString = this.anchorDataTextFormatString;
				hyperLinkColumn.NavigateUrl = this.url;
				hyperLinkColumn.DataNavigateUrlField = this.dataUrlField;
				hyperLinkColumn.DataNavigateUrlFormatString = this.dataUrlFormatString;
				hyperLinkColumn.Target = this.target;
			}

			protected string anchorText;

			protected string anchorDataTextField;

			protected string anchorDataTextFormatString;

			protected string url;

			protected string dataUrlField;

			protected string dataUrlFormatString;

			protected string target;
		}

		private class TemplateColumnItem : DataGridColumnsPage.ColumnItem
		{
			public TemplateColumnItem(TemplateColumn runtimeColumn)
				: base(runtimeColumn, 9)
			{
			}

			protected override string GetDefaultHeaderText()
			{
				return SR.GetString("DGCol_Node_Template");
			}
		}

		private class EditCommandColumnItem : DataGridColumnsPage.ColumnItem
		{
			public EditCommandColumnItem(EditCommandColumn runtimeColumn)
				: base(runtimeColumn, 4)
			{
			}

			public ButtonColumnType ButtonType
			{
				get
				{
					return this.buttonType;
				}
				set
				{
					this.buttonType = value;
				}
			}

			public string CancelText
			{
				get
				{
					return this.cancelText;
				}
				set
				{
					this.cancelText = value;
				}
			}

			public string EditText
			{
				get
				{
					return this.editText;
				}
				set
				{
					this.editText = value;
				}
			}

			public string UpdateText
			{
				get
				{
					return this.updateText;
				}
				set
				{
					this.updateText = value;
				}
			}

			protected override string GetDefaultHeaderText()
			{
				return SR.GetString("DGCol_Node_Edit");
			}

			public override TemplateColumn GetTemplateColumn(global::System.Web.UI.WebControls.DataGrid dataGrid)
			{
				TemplateColumn templateColumn = base.GetTemplateColumn(dataGrid);
				templateColumn.ItemTemplate = base.GetTemplate(dataGrid, this.GetTemplateContent(false));
				templateColumn.EditItemTemplate = base.GetTemplate(dataGrid, this.GetTemplateContent(true));
				return templateColumn;
			}

			private string GetTemplateContent(bool editMode)
			{
				StringBuilder stringBuilder = new StringBuilder();
				string text = ((this.buttonType == ButtonColumnType.LinkButton) ? "LinkButton" : "Button");
				stringBuilder.Append("<asp:");
				stringBuilder.Append(text);
				stringBuilder.Append(" runat=\"server\"");
				stringBuilder.Append(" Text=\"");
				if (!editMode)
				{
					stringBuilder.Append(this.editText);
				}
				else
				{
					stringBuilder.Append(this.updateText);
				}
				stringBuilder.Append("\"");
				stringBuilder.Append(" CommandName=\"");
				if (!editMode)
				{
					stringBuilder.Append("Edit\"");
					stringBuilder.Append(" CausesValidation=\"false\"");
				}
				else
				{
					stringBuilder.Append("Update\"");
				}
				stringBuilder.Append("></asp:");
				stringBuilder.Append(text);
				stringBuilder.Append(">");
				if (editMode)
				{
					stringBuilder.Append("&nbsp;");
					stringBuilder.Append("<asp:");
					stringBuilder.Append(text);
					stringBuilder.Append(" runat=\"server\"");
					stringBuilder.Append(" Text=\"");
					stringBuilder.Append(this.cancelText);
					stringBuilder.Append("\"");
					stringBuilder.Append(" CommandName=\"");
					stringBuilder.Append("Cancel\"");
					stringBuilder.Append(" CausesValidation=\"false\"></asp:");
					stringBuilder.Append(text);
					stringBuilder.Append(">");
				}
				return stringBuilder.ToString();
			}

			public override void LoadColumnInfo()
			{
				base.LoadColumnInfo();
				EditCommandColumn editCommandColumn = (EditCommandColumn)base.RuntimeColumn;
				this.editText = editCommandColumn.EditText;
				this.updateText = editCommandColumn.UpdateText;
				this.cancelText = editCommandColumn.CancelText;
				this.buttonType = editCommandColumn.ButtonType;
			}

			public override void SaveColumnInfo()
			{
				base.SaveColumnInfo();
				EditCommandColumn editCommandColumn = (EditCommandColumn)base.RuntimeColumn;
				editCommandColumn.EditText = this.editText;
				editCommandColumn.UpdateText = this.updateText;
				editCommandColumn.CancelText = this.cancelText;
				editCommandColumn.ButtonType = this.buttonType;
			}

			private string editText;

			private string updateText;

			private string cancelText;

			private ButtonColumnType buttonType;
		}

		private class CustomColumnItem : DataGridColumnsPage.ColumnItem
		{
			public CustomColumnItem(DataGridColumn runtimeColumn)
				: base(runtimeColumn, 3)
			{
			}
		}

		private abstract class ColumnItemEditor : global::System.Windows.Forms.Panel
		{
			public ColumnItemEditor()
			{
				this.InitPanel();
			}

			public virtual void AddDataField(string fieldName)
			{
				this.dataFieldsAvailable = true;
			}

			public event EventHandler Changed
			{
				add
				{
					this.onChangedHandler = (EventHandler)Delegate.Combine(this.onChangedHandler, value);
				}
				remove
				{
					this.onChangedHandler = (EventHandler)Delegate.Remove(this.onChangedHandler, value);
				}
			}

			public virtual void ClearDataFields()
			{
				this.dataFieldsAvailable = false;
			}

			protected virtual void InitPanel()
			{
			}

			public virtual void LoadColumn(DataGridColumnsPage.ColumnItem columnItem)
			{
				this.columnItem = columnItem;
			}

			protected virtual void OnChanged(EventArgs e)
			{
				if (this.onChangedHandler != null)
				{
					this.onChangedHandler(this, e);
				}
			}

			public virtual void SaveColumn()
			{
			}

			protected DataGridColumnsPage.ColumnItem columnItem;

			protected EventHandler onChangedHandler;

			protected bool dataFieldsAvailable;
		}

		private class BoundColumnEditor : DataGridColumnsPage.ColumnItemEditor
		{
			protected override void InitPanel()
			{
				global::System.Windows.Forms.Label label = new global::System.Windows.Forms.Label();
				this.dataFieldEdit = new global::System.Windows.Forms.TextBox();
				global::System.Windows.Forms.Label label2 = new global::System.Windows.Forms.Label();
				this.dataFormatStringEdit = new global::System.Windows.Forms.TextBox();
				this.readOnlyCheck = new global::System.Windows.Forms.CheckBox();
				label.SetBounds(0, 0, 160, 14);
				label.Text = SR.GetString("DGCol_DFC_DataField");
				label.TabStop = false;
				label.TabIndex = 1;
				label.Name = "BoundColumnDataFieldLabel";
				this.dataFieldEdit.SetBounds(0, 16, 182, 20);
				this.dataFieldEdit.TabIndex = 2;
				this.dataFieldEdit.ReadOnly = true;
				this.dataFieldEdit.TextChanged += this.OnColumnChanged;
				this.dataFieldEdit.Name = "BoundColumnDataFieldEdit";
				label2.SetBounds(0, 40, 182, 14);
				label2.Text = SR.GetString("DGCol_DFC_DataFormat");
				label2.TabStop = false;
				label2.TabIndex = 3;
				label2.Name = "BoundColumnDataFormatStringLabel";
				this.dataFormatStringEdit.SetBounds(0, 56, 182, 20);
				this.dataFormatStringEdit.TabIndex = 4;
				this.dataFormatStringEdit.TextChanged += this.OnColumnChanged;
				this.dataFormatStringEdit.Name = "BoundColumnDataFormatStringEdit";
				this.readOnlyCheck.SetBounds(0, 80, 160, 16);
				this.readOnlyCheck.Text = SR.GetString("DGCol_DFC_ReadOnly");
				this.readOnlyCheck.TabIndex = 5;
				this.readOnlyCheck.TextAlign = ContentAlignment.MiddleLeft;
				this.readOnlyCheck.FlatStyle = FlatStyle.System;
				this.readOnlyCheck.CheckedChanged += this.OnColumnChanged;
				this.readOnlyCheck.Name = "BoundColumnReadOnlyCheck";
				base.Controls.Clear();
				base.Controls.AddRange(new Control[] { this.readOnlyCheck, this.dataFormatStringEdit, label2, this.dataFieldEdit, label });
			}

			public override void LoadColumn(DataGridColumnsPage.ColumnItem columnItem)
			{
				base.LoadColumn(columnItem);
				DataGridColumnsPage.BoundColumnItem boundColumnItem = (DataGridColumnsPage.BoundColumnItem)columnItem;
				this.dataFieldEdit.Text = boundColumnItem.DataField;
				this.dataFormatStringEdit.Text = boundColumnItem.DataFormatString;
				this.readOnlyCheck.Checked = boundColumnItem.ReadOnly;
				this.dataFieldEdit.ReadOnly = this.dataFieldsAvailable;
			}

			private void OnColumnChanged(object source, EventArgs e)
			{
				this.OnChanged(EventArgs.Empty);
			}

			public override void SaveColumn()
			{
				base.SaveColumn();
				DataGridColumnsPage.BoundColumnItem boundColumnItem = (DataGridColumnsPage.BoundColumnItem)this.columnItem;
				boundColumnItem.DataFormatString = this.dataFormatStringEdit.Text;
				boundColumnItem.ReadOnly = this.readOnlyCheck.Checked;
				if (!this.dataFieldsAvailable)
				{
					boundColumnItem.DataField = this.dataFieldEdit.Text.Trim();
				}
			}

			private global::System.Windows.Forms.TextBox dataFieldEdit;

			private global::System.Windows.Forms.TextBox dataFormatStringEdit;

			private global::System.Windows.Forms.CheckBox readOnlyCheck;
		}

		private class ButtonColumnEditor : DataGridColumnsPage.ColumnItemEditor
		{
			public override void AddDataField(string fieldName)
			{
				this.dataTextFieldCombo.AddItem(fieldName);
				base.AddDataField(fieldName);
			}

			public override void ClearDataFields()
			{
				this.dataTextFieldCombo.Items.Clear();
				this.dataTextFieldCombo.EnsureNotSetItem();
				base.ClearDataFields();
			}

			protected override void InitPanel()
			{
				global::System.Windows.Forms.Label label = new global::System.Windows.Forms.Label();
				this.textEdit = new global::System.Windows.Forms.TextBox();
				global::System.Windows.Forms.Label label2 = new global::System.Windows.Forms.Label();
				this.dataTextFieldCombo = new UnsettableComboBox();
				this.dataTextFieldEdit = new global::System.Windows.Forms.TextBox();
				global::System.Windows.Forms.Label label3 = new global::System.Windows.Forms.Label();
				this.dataTextFormatStringEdit = new global::System.Windows.Forms.TextBox();
				global::System.Windows.Forms.Label label4 = new global::System.Windows.Forms.Label();
				this.commandEdit = new global::System.Windows.Forms.TextBox();
				global::System.Windows.Forms.Label label5 = new global::System.Windows.Forms.Label();
				this.buttonTypeCombo = new ComboBox();
				label.SetBounds(0, 0, 160, 14);
				label.Text = SR.GetString("DGCol_BC_Text");
				label.TabStop = false;
				label.TabIndex = 1;
				label.Name = "ButtonColumnTextLabel";
				this.textEdit.SetBounds(0, 16, 182, 24);
				this.textEdit.TabIndex = 2;
				this.textEdit.TextChanged += this.OnColumnChanged;
				this.textEdit.Name = "ButtonColumnTextEdit";
				label2.SetBounds(0, 40, 160, 14);
				label2.Text = SR.GetString("DGCol_BC_DataTextField");
				label2.TabStop = false;
				label2.TabIndex = 3;
				label2.Name = "ButtonColumnDataTextFieldLabel";
				this.dataTextFieldCombo.SetBounds(0, 56, 182, 21);
				this.dataTextFieldCombo.TabIndex = 4;
				this.dataTextFieldCombo.DropDownStyle = ComboBoxStyle.DropDownList;
				this.dataTextFieldCombo.SelectedIndexChanged += this.OnColumnChanged;
				this.dataTextFieldCombo.Name = "ButtonColumnDataTextFieldCombo";
				this.dataTextFieldEdit.SetBounds(0, 56, 182, 14);
				this.dataTextFieldEdit.TabIndex = 4;
				this.dataTextFieldEdit.TextChanged += this.OnColumnChanged;
				this.dataTextFieldEdit.Name = "ButtonColumnDataTextFieldEdit";
				label3.SetBounds(0, 82, 182, 14);
				label3.Text = SR.GetString("DGCol_BC_DataTextFormat");
				label3.TabIndex = 5;
				label3.TabStop = false;
				label3.Name = "ButtonColumnDataTextFormatStringLabel";
				this.dataTextFormatStringEdit.SetBounds(0, 98, 182, 14);
				this.dataTextFormatStringEdit.TabIndex = 6;
				this.dataTextFormatStringEdit.TextChanged += this.OnColumnChanged;
				this.dataTextFormatStringEdit.Name = "ButtonColumDataTextFormatStringEdit";
				label4.SetBounds(200, 0, 160, 14);
				label4.Text = SR.GetString("DGCol_BC_Command");
				label4.TabStop = false;
				label4.TabIndex = 8;
				label4.Name = "ButtonColumnCommandLabel";
				this.commandEdit.SetBounds(200, 16, 182, 24);
				this.commandEdit.TabIndex = 9;
				this.commandEdit.TextChanged += this.OnColumnChanged;
				this.commandEdit.Name = "ButtonColumnCommandEdit";
				label5.SetBounds(200, 40, 160, 14);
				label5.Text = SR.GetString("DGCol_BC_ButtonType");
				label5.TabStop = false;
				label5.TabIndex = 10;
				label5.Name = "ButtonColumnButtonTypeLabel";
				this.buttonTypeCombo.SetBounds(200, 56, 182, 21);
				this.buttonTypeCombo.DropDownStyle = ComboBoxStyle.DropDownList;
				this.buttonTypeCombo.Items.AddRange(new object[]
				{
					SR.GetString("DGCol_BC_BT_Link"),
					SR.GetString("DGCol_BC_BT_Push")
				});
				this.buttonTypeCombo.TabIndex = 11;
				this.buttonTypeCombo.SelectedIndexChanged += this.OnColumnChanged;
				this.buttonTypeCombo.Name = "ButtonColumnButtonTypeCombo";
				base.Controls.Clear();
				base.Controls.AddRange(new Control[]
				{
					this.buttonTypeCombo, label5, this.commandEdit, label4, this.dataTextFormatStringEdit, label3, this.dataTextFieldEdit, this.dataTextFieldCombo, label2, this.textEdit,
					label
				});
			}

			public override void LoadColumn(DataGridColumnsPage.ColumnItem columnItem)
			{
				base.LoadColumn(columnItem);
				DataGridColumnsPage.ButtonColumnItem buttonColumnItem = (DataGridColumnsPage.ButtonColumnItem)this.columnItem;
				this.commandEdit.Text = buttonColumnItem.Command;
				this.textEdit.Text = buttonColumnItem.ButtonText;
				if (this.dataFieldsAvailable)
				{
					if (buttonColumnItem.ButtonDataTextField != null)
					{
						int num = this.dataTextFieldCombo.FindStringExact(buttonColumnItem.ButtonDataTextField);
						this.dataTextFieldCombo.SelectedIndex = num;
					}
					this.dataTextFieldCombo.Visible = true;
					this.dataTextFieldEdit.Visible = false;
				}
				else
				{
					this.dataTextFieldEdit.Text = buttonColumnItem.ButtonDataTextField;
					this.dataTextFieldEdit.Visible = true;
					this.dataTextFieldCombo.Visible = false;
				}
				this.dataTextFormatStringEdit.Text = buttonColumnItem.ButtonDataTextFormatString;
				switch (buttonColumnItem.ButtonType)
				{
				case ButtonColumnType.LinkButton:
					this.buttonTypeCombo.SelectedIndex = 0;
					break;
				case ButtonColumnType.PushButton:
					this.buttonTypeCombo.SelectedIndex = 1;
					break;
				}
				this.UpdateEnabledState();
			}

			private void OnColumnChanged(object source, EventArgs e)
			{
				this.OnChanged(EventArgs.Empty);
				if (source == this.dataTextFieldCombo || source == this.dataTextFieldEdit)
				{
					this.UpdateEnabledState();
				}
			}

			public override void SaveColumn()
			{
				base.SaveColumn();
				DataGridColumnsPage.ButtonColumnItem buttonColumnItem = (DataGridColumnsPage.ButtonColumnItem)this.columnItem;
				buttonColumnItem.Command = this.commandEdit.Text.Trim();
				buttonColumnItem.ButtonText = this.textEdit.Text;
				if (this.dataFieldsAvailable)
				{
					if (this.dataTextFieldCombo.IsSet())
					{
						buttonColumnItem.ButtonDataTextField = this.dataTextFieldCombo.Text;
					}
					else
					{
						buttonColumnItem.ButtonDataTextField = string.Empty;
					}
				}
				else
				{
					buttonColumnItem.ButtonDataTextField = this.dataTextFieldEdit.Text.Trim();
				}
				buttonColumnItem.ButtonDataTextFormatString = this.dataTextFormatStringEdit.Text;
				switch (this.buttonTypeCombo.SelectedIndex)
				{
				case 0:
					buttonColumnItem.ButtonType = ButtonColumnType.LinkButton;
					return;
				case 1:
					buttonColumnItem.ButtonType = ButtonColumnType.PushButton;
					return;
				default:
					return;
				}
			}

			private void UpdateEnabledState()
			{
				if (this.dataFieldsAvailable)
				{
					this.dataTextFormatStringEdit.Enabled = this.dataTextFieldCombo.IsSet();
					return;
				}
				this.dataTextFormatStringEdit.Enabled = this.dataTextFieldEdit.Text.Trim().Length != 0;
			}

			private const int IDX_TYPE_LINKBUTTON = 0;

			private const int IDX_TYPE_PUSHBUTTON = 1;

			private global::System.Windows.Forms.TextBox commandEdit;

			private global::System.Windows.Forms.TextBox textEdit;

			private UnsettableComboBox dataTextFieldCombo;

			private global::System.Windows.Forms.TextBox dataTextFieldEdit;

			private global::System.Windows.Forms.TextBox dataTextFormatStringEdit;

			private ComboBox buttonTypeCombo;
		}

		private class HyperLinkColumnEditor : DataGridColumnsPage.ColumnItemEditor
		{
			public override void AddDataField(string fieldName)
			{
				this.dataTextFieldCombo.AddItem(fieldName);
				this.dataUrlFieldCombo.AddItem(fieldName);
				base.AddDataField(fieldName);
			}

			public override void ClearDataFields()
			{
				this.dataTextFieldCombo.Items.Clear();
				this.dataUrlFieldCombo.Items.Clear();
				this.dataTextFieldCombo.EnsureNotSetItem();
				this.dataUrlFieldCombo.EnsureNotSetItem();
				base.ClearDataFields();
			}

			protected override void InitPanel()
			{
				global::System.Windows.Forms.Label label = new global::System.Windows.Forms.Label();
				this.textEdit = new global::System.Windows.Forms.TextBox();
				global::System.Windows.Forms.Label label2 = new global::System.Windows.Forms.Label();
				this.dataTextFieldCombo = new UnsettableComboBox();
				this.dataTextFieldEdit = new global::System.Windows.Forms.TextBox();
				global::System.Windows.Forms.Label label3 = new global::System.Windows.Forms.Label();
				this.dataTextFormatStringEdit = new global::System.Windows.Forms.TextBox();
				global::System.Windows.Forms.Label label4 = new global::System.Windows.Forms.Label();
				this.targetCombo = new ComboBox();
				global::System.Windows.Forms.Label label5 = new global::System.Windows.Forms.Label();
				this.urlEdit = new global::System.Windows.Forms.TextBox();
				global::System.Windows.Forms.Label label6 = new global::System.Windows.Forms.Label();
				this.dataUrlFieldCombo = new UnsettableComboBox();
				this.dataUrlFieldEdit = new global::System.Windows.Forms.TextBox();
				global::System.Windows.Forms.Label label7 = new global::System.Windows.Forms.Label();
				this.dataUrlFormatStringEdit = new global::System.Windows.Forms.TextBox();
				label.SetBounds(0, 0, 160, 14);
				label.Text = SR.GetString("DGCol_HC_Text");
				label.TabStop = false;
				label.TabIndex = 1;
				label.Name = "HyperlinkColumnTextLabel";
				this.textEdit.SetBounds(0, 16, 182, 24);
				this.textEdit.TabIndex = 2;
				this.textEdit.TextChanged += this.OnColumnChanged;
				this.textEdit.Name = "HyperlinkColumnTextEdit";
				label2.SetBounds(0, 40, 160, 14);
				label2.Text = SR.GetString("DGCol_HC_DataTextField");
				label2.TabStop = false;
				label2.TabIndex = 3;
				label2.Name = "HyperlinkColumnDataTextFieldLabel";
				this.dataTextFieldCombo.SetBounds(0, 56, 182, 21);
				this.dataTextFieldCombo.DropDownStyle = ComboBoxStyle.DropDownList;
				this.dataTextFieldCombo.TabIndex = 4;
				this.dataTextFieldCombo.SelectedIndexChanged += this.OnColumnChanged;
				this.dataTextFieldCombo.Name = "HyperlinkColumnDataTextFieldCombo";
				this.dataTextFieldEdit.SetBounds(0, 56, 182, 14);
				this.dataTextFieldEdit.TabIndex = 4;
				this.dataTextFieldEdit.TextChanged += this.OnColumnChanged;
				this.dataTextFieldEdit.Name = "HyperlinkColumnDataTextFieldEdit";
				label3.SetBounds(0, 82, 160, 14);
				label3.Text = SR.GetString("DGCol_HC_DataTextFormat");
				label3.TabStop = false;
				label3.TabIndex = 5;
				label3.Name = "HyperlinkColumnDataTextFormatStringLabel";
				this.dataTextFormatStringEdit.SetBounds(0, 98, 182, 21);
				this.dataTextFormatStringEdit.TabIndex = 6;
				this.dataTextFormatStringEdit.TextChanged += this.OnColumnChanged;
				this.dataTextFormatStringEdit.Name = "HyperlinkColumnDataTextFormatStringEdit";
				label4.SetBounds(0, 123, 160, 14);
				label4.Text = SR.GetString("DGCol_HC_Target");
				label4.TabStop = false;
				label4.TabIndex = 7;
				label4.Name = "HyperlinkColumnTargetLabel";
				this.targetCombo.SetBounds(0, 139, 182, 21);
				this.targetCombo.TabIndex = 8;
				this.targetCombo.Items.AddRange(new object[] { "_blank", "_parent", "_search", "_self", "_top" });
				this.targetCombo.SelectedIndexChanged += this.OnColumnChanged;
				this.targetCombo.TextChanged += this.OnColumnChanged;
				this.targetCombo.Name = "HyperlinkColumnTargetCombo";
				label5.SetBounds(200, 0, 160, 14);
				label5.Text = SR.GetString("DGCol_HC_URL");
				label5.TabStop = false;
				label5.TabIndex = 10;
				label5.Name = "HyperlinkColumnUrlLabel";
				this.urlEdit.SetBounds(200, 16, 182, 24);
				this.urlEdit.TabIndex = 11;
				this.urlEdit.TextChanged += this.OnColumnChanged;
				this.urlEdit.Name = "HyperlinkColumnUrlEdit";
				label6.SetBounds(200, 40, 160, 14);
				label6.Text = SR.GetString("DGCol_HC_DataURLField");
				label6.TabStop = false;
				label6.TabIndex = 12;
				label6.Name = "HyperlinkColumnDataUrlFieldLabel";
				this.dataUrlFieldCombo.SetBounds(200, 56, 182, 21);
				this.dataUrlFieldCombo.DropDownStyle = ComboBoxStyle.DropDownList;
				this.dataUrlFieldCombo.TabIndex = 13;
				this.dataUrlFieldCombo.SelectedIndexChanged += this.OnColumnChanged;
				this.dataUrlFieldCombo.Name = "HyperlinkColumnDataUrlFieldCombo";
				this.dataUrlFieldEdit.SetBounds(200, 56, 182, 14);
				this.dataUrlFieldEdit.TabIndex = 13;
				this.dataUrlFieldEdit.TextChanged += this.OnColumnChanged;
				this.dataUrlFieldEdit.Name = "HyperlinkColumnDataUrlFieldEdit";
				label7.SetBounds(200, 82, 160, 14);
				label7.Text = SR.GetString("DGCol_HC_DataURLFormat");
				label7.TabStop = false;
				label7.TabIndex = 14;
				label7.Name = "HyperlinkColumnDataUrlFormatStringLabel";
				this.dataUrlFormatStringEdit.SetBounds(200, 98, 182, 21);
				this.dataUrlFormatStringEdit.TabIndex = 15;
				this.dataUrlFormatStringEdit.TextChanged += this.OnColumnChanged;
				this.dataUrlFormatStringEdit.Name = "HyperlinkColumnDataUrlFormatStringEdit";
				base.Controls.Clear();
				base.Controls.AddRange(new Control[]
				{
					this.dataUrlFormatStringEdit, label7, this.dataUrlFieldEdit, this.dataUrlFieldCombo, label6, this.urlEdit, label5, this.targetCombo, label4, this.dataTextFormatStringEdit,
					label3, this.dataTextFieldEdit, this.dataTextFieldCombo, label2, this.textEdit, label
				});
			}

			public override void LoadColumn(DataGridColumnsPage.ColumnItem columnItem)
			{
				base.LoadColumn(columnItem);
				DataGridColumnsPage.HyperLinkColumnItem hyperLinkColumnItem = (DataGridColumnsPage.HyperLinkColumnItem)this.columnItem;
				this.textEdit.Text = hyperLinkColumnItem.AnchorText;
				if (this.dataFieldsAvailable)
				{
					if (hyperLinkColumnItem.AnchorDataTextField != null)
					{
						int num = this.dataTextFieldCombo.FindStringExact(hyperLinkColumnItem.AnchorDataTextField);
						this.dataTextFieldCombo.SelectedIndex = num;
					}
					this.dataTextFieldCombo.Visible = true;
					this.dataTextFieldEdit.Visible = false;
				}
				else
				{
					this.dataTextFieldEdit.Text = hyperLinkColumnItem.AnchorDataTextField;
					this.dataTextFieldEdit.Visible = true;
					this.dataTextFieldCombo.Visible = false;
				}
				this.dataTextFormatStringEdit.Text = hyperLinkColumnItem.AnchorDataTextFormatString;
				this.urlEdit.Text = hyperLinkColumnItem.Url;
				if (this.dataFieldsAvailable)
				{
					if (hyperLinkColumnItem.DataUrlField != null)
					{
						int num2 = this.dataTextFieldCombo.FindStringExact(hyperLinkColumnItem.DataUrlField);
						this.dataUrlFieldCombo.SelectedIndex = num2;
					}
					this.dataUrlFieldCombo.Visible = true;
					this.dataUrlFieldEdit.Visible = false;
				}
				else
				{
					this.dataUrlFieldEdit.Text = hyperLinkColumnItem.DataUrlField;
					this.dataUrlFieldEdit.Visible = true;
					this.dataUrlFieldCombo.Visible = false;
				}
				this.dataUrlFormatStringEdit.Text = hyperLinkColumnItem.DataUrlFormatString;
				this.targetCombo.Text = hyperLinkColumnItem.Target;
				this.UpdateEnabledState();
			}

			protected void OnColumnChanged(object source, EventArgs e)
			{
				this.OnChanged(EventArgs.Empty);
				if (source == this.dataTextFieldCombo || source == this.dataUrlFieldCombo || source == this.dataTextFieldEdit || source == this.dataUrlFieldEdit)
				{
					this.UpdateEnabledState();
				}
			}

			public override void SaveColumn()
			{
				base.SaveColumn();
				DataGridColumnsPage.HyperLinkColumnItem hyperLinkColumnItem = (DataGridColumnsPage.HyperLinkColumnItem)this.columnItem;
				hyperLinkColumnItem.AnchorText = this.textEdit.Text;
				if (this.dataFieldsAvailable)
				{
					if (this.dataTextFieldCombo.IsSet())
					{
						hyperLinkColumnItem.AnchorDataTextField = this.dataTextFieldCombo.Text;
					}
					else
					{
						hyperLinkColumnItem.AnchorDataTextField = string.Empty;
					}
				}
				else
				{
					hyperLinkColumnItem.AnchorDataTextField = this.dataTextFieldEdit.Text.Trim();
				}
				hyperLinkColumnItem.AnchorDataTextFormatString = this.dataTextFormatStringEdit.Text;
				hyperLinkColumnItem.Url = this.urlEdit.Text.Trim();
				if (this.dataFieldsAvailable)
				{
					if (this.dataUrlFieldCombo.IsSet())
					{
						hyperLinkColumnItem.DataUrlField = this.dataUrlFieldCombo.Text;
					}
					else
					{
						hyperLinkColumnItem.DataUrlField = string.Empty;
					}
				}
				else
				{
					hyperLinkColumnItem.DataUrlField = this.dataUrlFieldEdit.Text.Trim();
				}
				hyperLinkColumnItem.DataUrlFormatString = this.dataUrlFormatStringEdit.Text;
				hyperLinkColumnItem.Target = this.targetCombo.Text.Trim();
			}

			private void UpdateEnabledState()
			{
				if (this.dataFieldsAvailable)
				{
					this.dataTextFormatStringEdit.Enabled = this.dataTextFieldCombo.IsSet();
					this.dataUrlFormatStringEdit.Enabled = this.dataUrlFieldCombo.IsSet();
					return;
				}
				this.dataTextFormatStringEdit.Enabled = this.dataTextFieldEdit.Text.Trim().Length != 0;
				this.dataUrlFormatStringEdit.Enabled = this.dataUrlFieldEdit.Text.Trim().Length != 0;
			}

			private global::System.Windows.Forms.TextBox textEdit;

			private UnsettableComboBox dataTextFieldCombo;

			private global::System.Windows.Forms.TextBox dataTextFieldEdit;

			private global::System.Windows.Forms.TextBox dataTextFormatStringEdit;

			private global::System.Windows.Forms.TextBox urlEdit;

			private UnsettableComboBox dataUrlFieldCombo;

			private global::System.Windows.Forms.TextBox dataUrlFieldEdit;

			private global::System.Windows.Forms.TextBox dataUrlFormatStringEdit;

			private ComboBox targetCombo;
		}

		private class EditCommandColumnEditor : DataGridColumnsPage.ColumnItemEditor
		{
			protected override void InitPanel()
			{
				global::System.Windows.Forms.Label label = new global::System.Windows.Forms.Label();
				this.editTextEdit = new global::System.Windows.Forms.TextBox();
				global::System.Windows.Forms.Label label2 = new global::System.Windows.Forms.Label();
				this.updateTextEdit = new global::System.Windows.Forms.TextBox();
				global::System.Windows.Forms.Label label3 = new global::System.Windows.Forms.Label();
				this.cancelTextEdit = new global::System.Windows.Forms.TextBox();
				global::System.Windows.Forms.Label label4 = new global::System.Windows.Forms.Label();
				this.buttonTypeCombo = new ComboBox();
				label.SetBounds(0, 0, 160, 14);
				label.Text = SR.GetString("DGCol_EC_Edit");
				label.TabStop = false;
				label.TabIndex = 1;
				label.Name = "EditColumnEditTextLabel";
				this.editTextEdit.SetBounds(0, 16, 182, 24);
				this.editTextEdit.TabIndex = 2;
				this.editTextEdit.TextChanged += this.OnColumnChanged;
				this.editTextEdit.Name = "EditColumnEditTextEdit";
				label2.SetBounds(0, 40, 160, 14);
				label2.Text = SR.GetString("DGCol_EC_Update");
				label2.TabStop = false;
				label2.TabIndex = 3;
				label2.Name = "EditColumnUpdateTextLabel";
				this.updateTextEdit.SetBounds(0, 56, 182, 24);
				this.updateTextEdit.TabIndex = 4;
				this.updateTextEdit.TextChanged += this.OnColumnChanged;
				this.updateTextEdit.Name = "EditColumnUpdateTextEdit";
				label3.SetBounds(200, 0, 160, 14);
				label3.Text = SR.GetString("DGCol_EC_Cancel");
				label3.TabStop = false;
				label3.TabIndex = 5;
				label3.Name = "EditColumnCancelTextLabel";
				this.cancelTextEdit.SetBounds(200, 16, 182, 24);
				this.cancelTextEdit.TabIndex = 6;
				this.cancelTextEdit.TextChanged += this.OnColumnChanged;
				this.cancelTextEdit.Name = "EditColumnCancelTextEdit";
				label4.SetBounds(200, 40, 160, 14);
				label4.Text = SR.GetString("DGCol_EC_ButtonType");
				label4.TabStop = false;
				label4.TabIndex = 7;
				label4.Name = "EditColumnButtonTypeLabel";
				this.buttonTypeCombo.SetBounds(200, 56, 182, 21);
				this.buttonTypeCombo.DropDownStyle = ComboBoxStyle.DropDownList;
				this.buttonTypeCombo.Items.AddRange(new object[]
				{
					SR.GetString("DGCol_EC_BT_Link"),
					SR.GetString("DGCol_EC_BT_Push")
				});
				this.buttonTypeCombo.TabIndex = 8;
				this.buttonTypeCombo.SelectedIndexChanged += this.OnColumnChanged;
				this.buttonTypeCombo.Name = "EditColumnButtonTypeCombo";
				base.Controls.Clear();
				base.Controls.AddRange(new Control[] { this.buttonTypeCombo, label4, this.cancelTextEdit, label3, this.updateTextEdit, label2, this.editTextEdit, label });
			}

			public override void LoadColumn(DataGridColumnsPage.ColumnItem columnItem)
			{
				base.LoadColumn(columnItem);
				DataGridColumnsPage.EditCommandColumnItem editCommandColumnItem = (DataGridColumnsPage.EditCommandColumnItem)this.columnItem;
				this.editTextEdit.Text = editCommandColumnItem.EditText;
				this.updateTextEdit.Text = editCommandColumnItem.UpdateText;
				this.cancelTextEdit.Text = editCommandColumnItem.CancelText;
				switch (editCommandColumnItem.ButtonType)
				{
				case ButtonColumnType.LinkButton:
					this.buttonTypeCombo.SelectedIndex = 0;
					return;
				case ButtonColumnType.PushButton:
					this.buttonTypeCombo.SelectedIndex = 1;
					return;
				default:
					return;
				}
			}

			private void OnColumnChanged(object source, EventArgs e)
			{
				this.OnChanged(EventArgs.Empty);
			}

			public override void SaveColumn()
			{
				base.SaveColumn();
				DataGridColumnsPage.EditCommandColumnItem editCommandColumnItem = (DataGridColumnsPage.EditCommandColumnItem)this.columnItem;
				editCommandColumnItem.EditText = this.editTextEdit.Text;
				editCommandColumnItem.UpdateText = this.updateTextEdit.Text;
				editCommandColumnItem.CancelText = this.cancelTextEdit.Text;
				switch (this.buttonTypeCombo.SelectedIndex)
				{
				case 0:
					editCommandColumnItem.ButtonType = ButtonColumnType.LinkButton;
					return;
				case 1:
					editCommandColumnItem.ButtonType = ButtonColumnType.PushButton;
					return;
				default:
					return;
				}
			}

			private const int IDX_TYPE_LINKBUTTON = 0;

			private const int IDX_TYPE_PUSHBUTTON = 1;

			private global::System.Windows.Forms.TextBox editTextEdit;

			private global::System.Windows.Forms.TextBox updateTextEdit;

			private global::System.Windows.Forms.TextBox cancelTextEdit;

			private ComboBox buttonTypeCombo;
		}
	}
}
