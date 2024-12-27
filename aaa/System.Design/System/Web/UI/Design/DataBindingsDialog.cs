using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Web.UI.Design.Util;
using System.Windows.Forms;

namespace System.Web.UI.Design
{
	// Token: 0x02000343 RID: 835
	internal sealed partial class DataBindingsDialog : DesignerForm
	{
		// Token: 0x06001F7B RID: 8059 RVA: 0x000B316B File Offset: 0x000B216B
		public DataBindingsDialog(IServiceProvider serviceProvider, Control control)
			: base(serviceProvider)
		{
			this._controlID = control.ID;
			this.InitializeComponent();
			this.InitializeUserInterface();
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001F7C RID: 8060 RVA: 0x000B318C File Offset: 0x000B218C
		private Control Control
		{
			get
			{
				IServiceProvider serviceProvider = base.ServiceProvider;
				if (serviceProvider != null)
				{
					ISite site = serviceProvider as ISite;
					IContainer container = null;
					if (site != null)
					{
						container = site.Container;
					}
					IContainer container2;
					if (container != null && container is NestedContainer)
					{
						container2 = container;
					}
					else
					{
						container2 = (IContainer)serviceProvider.GetService(typeof(IContainer));
					}
					if (container2 != null)
					{
						return container2.Components[this._controlID] as Control;
					}
				}
				return null;
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06001F7D RID: 8061 RVA: 0x000B31FC File Offset: 0x000B21FC
		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.DataBinding.BindingsDialog";
			}
		}

		// Token: 0x06001F7E RID: 8062 RVA: 0x000B3204 File Offset: 0x000B2204
		private bool ContainingTemplateIsBindable(ControlDesigner designer)
		{
			bool flag = false;
			IControlDesignerView view = designer.View;
			if (view != null)
			{
				TemplatedEditableDesignerRegion templatedEditableDesignerRegion = view.ContainingRegion as TemplatedEditableDesignerRegion;
				if (templatedEditableDesignerRegion != null)
				{
					TemplateDefinition templateDefinition = templatedEditableDesignerRegion.TemplateDefinition;
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(templateDefinition.TemplatedObject)[templateDefinition.TemplatePropertyName];
					if (propertyDescriptor != null)
					{
						TemplateContainerAttribute templateContainerAttribute = propertyDescriptor.Attributes[typeof(TemplateContainerAttribute)] as TemplateContainerAttribute;
						if (templateContainerAttribute != null && templateContainerAttribute.BindingDirection == BindingDirection.TwoWay)
						{
							flag = true;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x06001F7F RID: 8063 RVA: 0x000B3280 File Offset: 0x000B2280
		private void ExtractFields(IDataSourceViewSchema schema, ArrayList fields)
		{
			if (schema != null)
			{
				IDataSourceFieldSchema[] fields2 = schema.GetFields();
				if (fields2 != null)
				{
					for (int i = 0; i < fields2.Length; i++)
					{
						fields.Add(new DataBindingsDialog.FieldItem(fields2[i].Name, fields2[i].DataType));
					}
				}
			}
		}

		// Token: 0x06001F80 RID: 8064 RVA: 0x000B32C4 File Offset: 0x000B22C4
		private void ExtractFields(IDataSourceProvider dataSourceProvider, ArrayList fields)
		{
			IEnumerable resolvedSelectedDataSource = dataSourceProvider.GetResolvedSelectedDataSource();
			if (resolvedSelectedDataSource != null)
			{
				PropertyDescriptorCollection dataFields = DesignTimeData.GetDataFields(resolvedSelectedDataSource);
				if (dataFields != null && dataFields.Count != 0)
				{
					foreach (object obj in dataFields)
					{
						PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
						fields.Add(new DataBindingsDialog.FieldItem(propertyDescriptor.Name, propertyDescriptor.PropertyType));
					}
				}
			}
		}

		// Token: 0x06001F81 RID: 8065 RVA: 0x000B3348 File Offset: 0x000B2348
		private IDesigner GetNamingContainerDesigner(ControlDesigner designer)
		{
			IControlDesignerView view = designer.View;
			if (view == null)
			{
				return null;
			}
			return view.NamingContainerDesigner;
		}

		// Token: 0x06001F83 RID: 8067 RVA: 0x000B3EC0 File Offset: 0x000B2EC0
		private void InitializeUserInterface()
		{
			this.Text = SR.GetString("DBDlg_Text", new object[] { this.Control.Site.Name });
			this._instructionLabel.Text = SR.GetString("DBDlg_Inst");
			this._bindablePropsLabels.Text = SR.GetString("DBDlg_BindableProps");
			this._allPropsCheckBox.Text = SR.GetString("DBDlg_ShowAll");
			this._fieldBindingRadio.Text = SR.GetString("DBDlg_FieldBinding");
			this._fieldLabel.Text = SR.GetString("DBDlg_Field");
			this._formatLabel.Text = SR.GetString("DBDlg_Format");
			this._sampleLabel.Text = SR.GetString("DBDlg_Sample");
			this._exprBindingRadio.Text = SR.GetString("DBDlg_CustomBinding");
			this._okButton.Text = SR.GetString("DBDlg_OK");
			this._cancelButton.Text = SR.GetString("DBDlg_Cancel");
			this._refreshSchemaLink.Text = SR.GetString("DBDlg_RefreshSchema");
			this._exprLabel.Text = SR.GetString("DBDlg_Expr");
			this._twoWayBindingCheckBox.Text = SR.GetString("DBDlg_TwoWay");
			ImageList imageList = new ImageList();
			imageList.TransparentColor = Color.Magenta;
			imageList.ColorDepth = ColorDepth.Depth32Bit;
			imageList.Images.AddStrip(new Bitmap(typeof(DataBindingsDialog), "BindableProperties.bmp"));
			this._bindablePropsTree.ImageList = imageList;
			bool flag = false;
			IDesignerHost designerHost = (IDesignerHost)this.Control.Site.GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				ControlDesigner controlDesigner = designerHost.GetDesigner(this.Control) as ControlDesigner;
				if (controlDesigner != null)
				{
					flag = this.ContainingTemplateIsBindable(controlDesigner);
				}
			}
			this._twoWayBindingCheckBox.Visible = flag;
		}

		// Token: 0x06001F84 RID: 8068 RVA: 0x000B40A0 File Offset: 0x000B30A0
		private void LoadBindableProperties(bool showAll)
		{
			string text = string.Empty;
			if (this._bindablePropsTree.SelectedNode != null)
			{
				text = this._bindablePropsTree.SelectedNode.Text;
			}
			this._bindablePropsTree.Nodes.Clear();
			PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(this.Control.GetType(), DataBindingsDialog.BindablePropertiesFilter);
			if (showAll)
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this.Control.GetType(), DataBindingsDialog.BrowsablePropertiesFilter);
				if (properties != null && properties.Count > 0)
				{
					int count = propertyDescriptorCollection.Count;
					int count2 = properties.Count;
					PropertyDescriptor[] array = new PropertyDescriptor[count + count2];
					propertyDescriptorCollection.CopyTo(array, 0);
					int num = count;
					foreach (object obj in properties)
					{
						PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
						if (!propertyDescriptorCollection.Contains(propertyDescriptor) && !string.Equals(propertyDescriptor.Name, "id", StringComparison.OrdinalIgnoreCase))
						{
							array[num++] = propertyDescriptor;
						}
					}
					PropertyDescriptor[] array2 = new PropertyDescriptor[num];
					Array.Copy(array, array2, num);
					propertyDescriptorCollection = new PropertyDescriptorCollection(array2);
				}
			}
			string text2 = null;
			ControlValuePropertyAttribute controlValuePropertyAttribute = TypeDescriptor.GetAttributes(this.Control)[typeof(ControlValuePropertyAttribute)] as ControlValuePropertyAttribute;
			if (controlValuePropertyAttribute != null)
			{
				text2 = controlValuePropertyAttribute.Name;
			}
			else
			{
				PropertyDescriptor defaultProperty = TypeDescriptor.GetDefaultProperty(this.Control);
				if (defaultProperty != null)
				{
					text2 = defaultProperty.Name;
				}
			}
			TreeNodeCollection nodes = this._bindablePropsTree.Nodes;
			TreeNode treeNode = null;
			TreeNode treeNode2 = null;
			this._bindablePropsTree.BeginUpdate();
			foreach (object obj2 in propertyDescriptorCollection)
			{
				PropertyDescriptor propertyDescriptor2 = (PropertyDescriptor)obj2;
				bool flag = this._bindings[propertyDescriptor2.Name] != null;
				DataBindingsDialog.BindingMode bindingMode = DataBindingsDialog.BindingMode.NotSet;
				if (flag)
				{
					if (((DesignTimeDataBinding)this._bindings[propertyDescriptor2.Name]).IsTwoWayBound)
					{
						bindingMode = DataBindingsDialog.BindingMode.TwoWay;
					}
					else
					{
						bindingMode = DataBindingsDialog.BindingMode.OneWay;
					}
				}
				TreeNode treeNode3 = new DataBindingsDialog.BindablePropertyNode(propertyDescriptor2, bindingMode);
				if (propertyDescriptor2.Name.Equals(text2))
				{
					treeNode = treeNode3;
				}
				if (propertyDescriptor2.Name.Equals(text))
				{
					treeNode2 = treeNode3;
				}
				nodes.Add(treeNode3);
			}
			this._bindablePropsTree.EndUpdate();
			if (treeNode2 == null && treeNode == null && nodes.Count != 0)
			{
				int count3 = nodes.Count;
				for (int i = 0; i < count3; i++)
				{
					DataBindingsDialog.BindablePropertyNode bindablePropertyNode = (DataBindingsDialog.BindablePropertyNode)nodes[i];
					if (bindablePropertyNode.IsBound)
					{
						treeNode2 = bindablePropertyNode;
						break;
					}
				}
				if (treeNode2 == null)
				{
					treeNode2 = nodes[0];
				}
			}
			if (treeNode2 != null)
			{
				this._bindablePropsTree.SelectedNode = treeNode2;
			}
			else if (treeNode != null)
			{
				this._bindablePropsTree.SelectedNode = treeNode;
			}
			this.UpdateUIState();
		}

		// Token: 0x06001F85 RID: 8069 RVA: 0x000B43A0 File Offset: 0x000B33A0
		private void LoadCurrentDataBinding()
		{
			this._internalChange = true;
			try
			{
				this._fieldBindingRadio.Checked = this._fieldsAvailable;
				this._bindingLabel.Text = string.Empty;
				this._fieldCombo.SelectedIndex = -1;
				this._formatCombo.Text = string.Empty;
				this._sampleTextBox.Text = string.Empty;
				this._exprBindingRadio.Checked = !this._fieldsAvailable;
				this._exprTextBox.Text = string.Empty;
				this._twoWayBindingCheckBox.Checked = false;
				this._formatDirty = false;
				if (this._currentNode != null)
				{
					this._bindingLabel.Text = SR.GetString("DBDlg_BindingGroup", new object[] { this._currentNode.PropertyDescriptor.Name });
					this._twoWayBindingCheckBox.Checked = this._currentNode.TwoWayBoundByDefault && this._twoWayBindingCheckBox.Visible;
					if (this._currentDataBinding != null)
					{
						bool flag = true;
						if (this._fieldsAvailable && !this._currentDataBinding.IsCustom)
						{
							string text = this._currentDataBinding.Field;
							string format = this._currentDataBinding.Format;
							text = text.TrimStart(new char[] { '[' });
							text = text.TrimEnd(new char[] { ']' });
							int num = this._fieldCombo.FindStringExact(text, 1);
							if (num != -1)
							{
								flag = false;
								this._fieldCombo.SelectedIndex = num;
								this.UpdateFormatItems();
								bool flag2 = false;
								foreach (object obj in this._formatCombo.Items)
								{
									DataBindingsDialog.FormatItem formatItem = (DataBindingsDialog.FormatItem)obj;
									if (formatItem.Format.Equals(format))
									{
										flag2 = true;
										this._formatCombo.SelectedItem = formatItem;
									}
								}
								if (!flag2)
								{
									this._formatCombo.Text = format;
								}
								this.UpdateFormatSample();
								if (this._currentNode.BindingMode == DataBindingsDialog.BindingMode.TwoWay)
								{
									this._twoWayBindingCheckBox.Checked = true;
								}
								else if (this._currentNode.BindingMode == DataBindingsDialog.BindingMode.OneWay)
								{
									this._twoWayBindingCheckBox.Checked = false;
								}
							}
						}
						if (flag)
						{
							this._exprBindingRadio.Checked = true;
							this._exprTextBox.Text = this._currentDataBinding.Expression;
						}
						else
						{
							this.UpdateExpression();
						}
					}
				}
			}
			finally
			{
				this._internalChange = false;
				this.UpdateUIState();
			}
		}

		// Token: 0x06001F86 RID: 8070 RVA: 0x000B4654 File Offset: 0x000B3654
		private void LoadDataBindings()
		{
			this._bindings = new Hashtable();
			DataBindingCollection dataBindings = ((IDataBindingsAccessor)this.Control).DataBindings;
			foreach (object obj in dataBindings)
			{
				DataBinding dataBinding = (DataBinding)obj;
				this._bindings[dataBinding.PropertyName] = new DesignTimeDataBinding(dataBinding);
			}
		}

		// Token: 0x06001F87 RID: 8071 RVA: 0x000B46D0 File Offset: 0x000B36D0
		private void LoadFields()
		{
			this._fieldCombo.Items.Clear();
			ArrayList arrayList = new ArrayList();
			arrayList.Add(new DataBindingsDialog.FieldItem());
			IDesigner designer = null;
			IDesignerHost designerHost = (IDesignerHost)this.Control.Site.GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				ControlDesigner controlDesigner = designerHost.GetDesigner(this.Control) as ControlDesigner;
				if (controlDesigner != null)
				{
					designer = this.GetNamingContainerDesigner(controlDesigner);
				}
			}
			if (designer != null)
			{
				IDataBindingSchemaProvider dataBindingSchemaProvider = designer as IDataBindingSchemaProvider;
				if (dataBindingSchemaProvider != null)
				{
					if (dataBindingSchemaProvider.CanRefreshSchema)
					{
						this._refreshSchemaLink.Visible = true;
					}
					IDataSourceViewSchema dataSourceViewSchema = null;
					try
					{
						dataSourceViewSchema = dataBindingSchemaProvider.Schema;
					}
					catch (Exception ex)
					{
						IComponentDesignerDebugService componentDesignerDebugService = (IComponentDesignerDebugService)base.ServiceProvider.GetService(typeof(IComponentDesignerDebugService));
						if (componentDesignerDebugService != null)
						{
							componentDesignerDebugService.Fail(SR.GetString("DataSource_DebugService_FailedCall", new object[] { "DesignerDataSourceView.Schema", ex.Message }));
						}
					}
					this.ExtractFields(dataSourceViewSchema, arrayList);
				}
				else if (designer is IDataSourceProvider)
				{
					this.ExtractFields((IDataSourceProvider)designer, arrayList);
				}
			}
			this._fieldCombo.Items.AddRange(arrayList.ToArray());
			this._fieldsAvailable = arrayList.Count > 1;
		}

		// Token: 0x06001F88 RID: 8072 RVA: 0x000B4824 File Offset: 0x000B3824
		private void OnBindablePropsTreeAfterSelect(object sender, TreeViewEventArgs e)
		{
			if (this._currentDataBindingDirty)
			{
				this.SaveCurrentDataBinding();
			}
			this._currentDataBinding = null;
			this._currentNode = (DataBindingsDialog.BindablePropertyNode)this._bindablePropsTree.SelectedNode;
			if (this._currentNode != null)
			{
				this._currentDataBinding = (DesignTimeDataBinding)this._bindings[this._currentNode.PropertyDescriptor.Name];
			}
			this.LoadCurrentDataBinding();
		}

		// Token: 0x06001F89 RID: 8073 RVA: 0x000B4890 File Offset: 0x000B3890
		private void OnExprBindingRadioCheckedChanged(object sender, EventArgs e)
		{
			if (this._internalChange)
			{
				return;
			}
			this._currentDataBindingDirty = true;
			this.UpdateUIState();
		}

		// Token: 0x06001F8A RID: 8074 RVA: 0x000B48A8 File Offset: 0x000B38A8
		private void OnExprTextBoxTextChanged(object sender, EventArgs e)
		{
			if (this._internalChange)
			{
				return;
			}
			this._currentDataBindingDirty = true;
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x000B48BA File Offset: 0x000B38BA
		private void OnFieldBindingRadioCheckedChanged(object sender, EventArgs e)
		{
			if (this._internalChange)
			{
				return;
			}
			this._currentDataBindingDirty = true;
			if (this._fieldBindingRadio.Checked)
			{
				this.UpdateExpression();
			}
			this.UpdateUIState();
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x000B48E5 File Offset: 0x000B38E5
		private void OnFieldComboSelectedIndexChanged(object sender, EventArgs e)
		{
			if (this._internalChange)
			{
				return;
			}
			this._currentDataBindingDirty = true;
			this.UpdateFormatItems();
			this.UpdateExpression();
			this.UpdateUIState();
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x000B4909 File Offset: 0x000B3909
		private void OnFormatComboLostFocus(object sender, EventArgs e)
		{
			if (this._formatDirty)
			{
				this._formatDirty = false;
				this.UpdateFormatSample();
				this.UpdateExpression();
			}
		}

		// Token: 0x06001F8E RID: 8078 RVA: 0x000B4926 File Offset: 0x000B3926
		private void OnFormatComboTextChanged(object sender, EventArgs e)
		{
			if (this._internalChange)
			{
				return;
			}
			this._formatDirty = true;
		}

		// Token: 0x06001F8F RID: 8079 RVA: 0x000B4938 File Offset: 0x000B3938
		private void OnFormatComboSelectedIndexChanged(object sender, EventArgs e)
		{
			if (this._internalChange)
			{
				return;
			}
			this._formatDirty = true;
			this.UpdateFormatSample();
			this.UpdateExpression();
		}

		// Token: 0x06001F90 RID: 8080 RVA: 0x000B4956 File Offset: 0x000B3956
		protected override void OnInitialActivated(EventArgs e)
		{
			base.OnInitialActivated(e);
			this.LoadDataBindings();
			this.LoadFields();
			this.LoadBindableProperties(false);
		}

		// Token: 0x06001F91 RID: 8081 RVA: 0x000B4972 File Offset: 0x000B3972
		private void OnOKButtonClick(object sender, EventArgs e)
		{
			if (this._currentDataBindingDirty)
			{
				this.SaveCurrentDataBinding();
			}
			if (this._bindingsDirty)
			{
				this.SaveDataBindings();
			}
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		// Token: 0x06001F92 RID: 8082 RVA: 0x000B49A0 File Offset: 0x000B39A0
		private void OnRefreshSchemaLinkLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (this._currentDataBindingDirty)
			{
				this.SaveCurrentDataBinding();
			}
			IDesigner designer = null;
			IDesignerHost designerHost = (IDesignerHost)this.Control.Site.GetService(typeof(IDesignerHost));
			if (designerHost != null)
			{
				ControlDesigner controlDesigner = designerHost.GetDesigner(this.Control) as ControlDesigner;
				if (controlDesigner != null)
				{
					designer = this.GetNamingContainerDesigner(controlDesigner);
				}
			}
			if (designer != null)
			{
				IDataBindingSchemaProvider dataBindingSchemaProvider = designer as IDataBindingSchemaProvider;
				if (dataBindingSchemaProvider != null)
				{
					dataBindingSchemaProvider.RefreshSchema(false);
				}
			}
			this.LoadFields();
			if (this._currentNode != null)
			{
				this._currentDataBinding = (DesignTimeDataBinding)this._bindings[this._currentNode.PropertyDescriptor.Name];
			}
			this.LoadCurrentDataBinding();
		}

		// Token: 0x06001F93 RID: 8083 RVA: 0x000B4A4B File Offset: 0x000B3A4B
		private void OnShowAllCheckedChanged(object sender, EventArgs e)
		{
			this.LoadBindableProperties(this._allPropsCheckBox.Checked);
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x000B4A5E File Offset: 0x000B3A5E
		private void OnTwoWayBindingChecked(object sender, EventArgs e)
		{
			if (this._internalChange)
			{
				return;
			}
			this._currentDataBindingDirty = true;
			this.UpdateExpression();
			this.UpdateUIState();
		}

		// Token: 0x06001F95 RID: 8085 RVA: 0x000B4A7C File Offset: 0x000B3A7C
		private void SaveCurrentDataBinding()
		{
			DesignTimeDataBinding designTimeDataBinding = null;
			if (this._fieldBindingRadio.Checked)
			{
				if (this._fieldCombo.SelectedIndex > 0)
				{
					string text = this._fieldCombo.Text;
					string text2 = this.SaveFormat();
					designTimeDataBinding = new DesignTimeDataBinding(this._currentNode.PropertyDescriptor, text, text2, this._twoWayBindingCheckBox.Checked);
				}
			}
			else
			{
				string text3 = this._exprTextBox.Text.Trim();
				if (text3.Length != 0)
				{
					designTimeDataBinding = new DesignTimeDataBinding(this._currentNode.PropertyDescriptor, text3);
				}
			}
			if (designTimeDataBinding == null)
			{
				this._currentNode.BindingMode = DataBindingsDialog.BindingMode.NotSet;
				this._bindings.Remove(this._currentNode.PropertyDescriptor.Name);
			}
			else
			{
				if (this._fieldBindingRadio.Checked)
				{
					if (this._twoWayBindingCheckBox.Checked && this._twoWayBindingCheckBox.Visible)
					{
						this._currentNode.BindingMode = DataBindingsDialog.BindingMode.TwoWay;
					}
					else
					{
						this._currentNode.BindingMode = DataBindingsDialog.BindingMode.OneWay;
					}
				}
				else if (designTimeDataBinding.IsTwoWayBound)
				{
					this._currentNode.BindingMode = DataBindingsDialog.BindingMode.TwoWay;
				}
				else
				{
					this._currentNode.BindingMode = DataBindingsDialog.BindingMode.OneWay;
				}
				this._bindings[this._currentNode.PropertyDescriptor.Name] = designTimeDataBinding;
			}
			this._currentDataBindingDirty = false;
			this._bindingsDirty = true;
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x000B4BC4 File Offset: 0x000B3BC4
		private void SaveDataBindings()
		{
			DataBindingCollection dataBindings = ((IDataBindingsAccessor)this.Control).DataBindings;
			ExpressionBindingCollection expressions = ((IExpressionsAccessor)this.Control).Expressions;
			dataBindings.Clear();
			foreach (object obj in this._bindings.Values)
			{
				DesignTimeDataBinding designTimeDataBinding = (DesignTimeDataBinding)obj;
				dataBindings.Add(designTimeDataBinding.RuntimeDataBinding);
				expressions.Remove(designTimeDataBinding.RuntimeDataBinding.PropertyName);
			}
			this._bindingsDirty = false;
		}

		// Token: 0x06001F97 RID: 8087 RVA: 0x000B4C60 File Offset: 0x000B3C60
		private string SaveFormat()
		{
			string text = string.Empty;
			DataBindingsDialog.FormatItem formatItem = this._formatCombo.SelectedItem as DataBindingsDialog.FormatItem;
			if (formatItem != null)
			{
				text = formatItem.Format;
			}
			else
			{
				text = this._formatCombo.Text;
				string text2 = text.Trim();
				if (text2.Length == 0)
				{
					text = text2;
				}
			}
			return text;
		}

		// Token: 0x06001F98 RID: 8088 RVA: 0x000B4CB0 File Offset: 0x000B3CB0
		private void UpdateExpression()
		{
			string text = string.Empty;
			if (this._fieldCombo.SelectedIndex > 0)
			{
				string text2 = this._fieldCombo.Text;
				string text3 = this.SaveFormat();
				if (this._twoWayBindingCheckBox.Checked)
				{
					text = DesignTimeDataBinding.CreateBindExpression(text2, text3);
				}
				else
				{
					text = DesignTimeDataBinding.CreateEvalExpression(text2, text3);
				}
			}
			this._exprTextBox.Text = text;
		}

		// Token: 0x06001F99 RID: 8089 RVA: 0x000B4D10 File Offset: 0x000B3D10
		private void UpdateFormatItems()
		{
			DataBindingsDialog.FormatItem[] array = DataBindingsDialog.FormatItem.DefaultFormats;
			this._formatSampleObject = null;
			this._formatCombo.SelectedIndex = -1;
			this._formatCombo.Text = string.Empty;
			DataBindingsDialog.FieldItem fieldItem = (DataBindingsDialog.FieldItem)this._fieldCombo.SelectedItem;
			if (fieldItem != null && fieldItem.Type != null)
			{
				switch (Type.GetTypeCode(fieldItem.Type))
				{
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
					array = DataBindingsDialog.FormatItem.NumericFormats;
					this._formatSampleObject = 1;
					break;
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					array = DataBindingsDialog.FormatItem.DecimalFormats;
					this._formatSampleObject = 1;
					break;
				case TypeCode.DateTime:
					array = DataBindingsDialog.FormatItem.DateTimeFormats;
					this._formatSampleObject = DateTime.Today;
					break;
				case TypeCode.String:
					this._formatSampleObject = "abc";
					break;
				}
			}
			this._formatCombo.Items.Clear();
			this._formatCombo.Items.AddRange(array);
		}

		// Token: 0x06001F9A RID: 8090 RVA: 0x000B4E34 File Offset: 0x000B3E34
		private void UpdateFormatSample()
		{
			string text = string.Empty;
			if (this._formatSampleObject != null)
			{
				string text2 = this.SaveFormat();
				if (text2.Length != 0)
				{
					try
					{
						text = string.Format(CultureInfo.CurrentCulture, text2, new object[] { this._formatSampleObject });
					}
					catch
					{
						text = SR.GetString("DBDlg_InvalidFormat");
					}
				}
			}
			this._sampleTextBox.Text = text;
		}

		// Token: 0x06001F9B RID: 8091 RVA: 0x000B4EA8 File Offset: 0x000B3EA8
		private void UpdateUIState()
		{
			if (this._currentNode == null)
			{
				this._fieldBindingRadio.Enabled = false;
				this._fieldCombo.Enabled = false;
				this._formatCombo.Enabled = false;
				this._sampleTextBox.Enabled = false;
				this._fieldLabel.Enabled = false;
				this._formatLabel.Enabled = false;
				this._sampleLabel.Enabled = false;
				this._twoWayBindingCheckBox.Enabled = false;
				this._exprBindingRadio.Enabled = false;
				this._exprTextBox.Enabled = false;
				return;
			}
			this._fieldBindingRadio.Enabled = this._fieldsAvailable;
			this._exprBindingRadio.Enabled = true;
			bool @checked = this._fieldBindingRadio.Checked;
			bool flag = @checked && this._fieldCombo.SelectedIndex > 0;
			bool flag2 = flag && this._currentNode.PropertyDescriptor.PropertyType == typeof(string);
			this._fieldCombo.Enabled = @checked;
			this._fieldLabel.Enabled = @checked;
			this._formatCombo.Enabled = flag2;
			this._formatLabel.Enabled = flag2;
			this._sampleTextBox.Enabled = flag2;
			this._sampleLabel.Enabled = flag2;
			this._twoWayBindingCheckBox.Enabled = flag;
			this._exprTextBox.Enabled = !@checked;
		}

		// Token: 0x0400177A RID: 6010
		private const int UnboundImageIndex = 0;

		// Token: 0x0400177B RID: 6011
		private const int BoundImageIndex = 1;

		// Token: 0x0400177C RID: 6012
		private const int TwoWayBoundImageIndex = 2;

		// Token: 0x0400177D RID: 6013
		private const int UnboundItemIndex = 0;

		// Token: 0x0400177E RID: 6014
		private static readonly Attribute[] BrowsablePropertiesFilter = new Attribute[]
		{
			BrowsableAttribute.Yes,
			ReadOnlyAttribute.No
		};

		// Token: 0x0400177F RID: 6015
		private static readonly Attribute[] BindablePropertiesFilter = new Attribute[]
		{
			BindableAttribute.Yes,
			ReadOnlyAttribute.No
		};

		// Token: 0x04001796 RID: 6038
		private string _controlID;

		// Token: 0x04001797 RID: 6039
		private IDictionary _bindings;

		// Token: 0x04001798 RID: 6040
		private bool _bindingsDirty;

		// Token: 0x04001799 RID: 6041
		private bool _fieldsAvailable;

		// Token: 0x0400179A RID: 6042
		private DataBindingsDialog.BindablePropertyNode _currentNode;

		// Token: 0x0400179B RID: 6043
		private DesignTimeDataBinding _currentDataBinding;

		// Token: 0x0400179C RID: 6044
		private bool _currentDataBindingDirty;

		// Token: 0x0400179D RID: 6045
		private bool _internalChange;

		// Token: 0x0400179E RID: 6046
		private bool _formatDirty;

		// Token: 0x0400179F RID: 6047
		private object _formatSampleObject;

		// Token: 0x02000344 RID: 836
		private sealed class BindablePropertyNode : TreeNode
		{
			// Token: 0x06001F9D RID: 8093 RVA: 0x000B5044 File Offset: 0x000B4044
			public BindablePropertyNode(PropertyDescriptor propDesc, DataBindingsDialog.BindingMode bindingMode)
			{
				this._propDesc = propDesc;
				this._bindingMode = bindingMode;
				base.Text = propDesc.Name;
				int num = 0;
				if (bindingMode == DataBindingsDialog.BindingMode.OneWay)
				{
					num = 1;
				}
				else if (bindingMode == DataBindingsDialog.BindingMode.TwoWay)
				{
					num = 2;
				}
				base.ImageIndex = (base.SelectedImageIndex = num);
			}

			// Token: 0x1700057F RID: 1407
			// (get) Token: 0x06001F9E RID: 8094 RVA: 0x000B5091 File Offset: 0x000B4091
			// (set) Token: 0x06001F9F RID: 8095 RVA: 0x000B509C File Offset: 0x000B409C
			public DataBindingsDialog.BindingMode BindingMode
			{
				get
				{
					return this._bindingMode;
				}
				set
				{
					this._bindingMode = value;
					int num = 0;
					if (this._bindingMode == DataBindingsDialog.BindingMode.OneWay)
					{
						num = 1;
					}
					else if (this._bindingMode == DataBindingsDialog.BindingMode.TwoWay)
					{
						num = 2;
					}
					base.ImageIndex = (base.SelectedImageIndex = num);
				}
			}

			// Token: 0x17000580 RID: 1408
			// (get) Token: 0x06001FA0 RID: 8096 RVA: 0x000B50DA File Offset: 0x000B40DA
			public bool IsBound
			{
				get
				{
					return this._bindingMode == DataBindingsDialog.BindingMode.OneWay || this._bindingMode == DataBindingsDialog.BindingMode.TwoWay;
				}
			}

			// Token: 0x17000581 RID: 1409
			// (get) Token: 0x06001FA1 RID: 8097 RVA: 0x000B50F0 File Offset: 0x000B40F0
			public bool TwoWayBoundByDefault
			{
				get
				{
					if (!this._twoWayBoundByDefaultValid)
					{
						BindableAttribute bindableAttribute = this._propDesc.Attributes[typeof(BindableAttribute)] as BindableAttribute;
						if (bindableAttribute != null)
						{
							this._twoWayBoundByDefault = bindableAttribute.Direction == BindingDirection.TwoWay;
						}
						this._twoWayBoundByDefaultValid = true;
					}
					return this._twoWayBoundByDefault;
				}
			}

			// Token: 0x17000582 RID: 1410
			// (get) Token: 0x06001FA2 RID: 8098 RVA: 0x000B5144 File Offset: 0x000B4144
			public PropertyDescriptor PropertyDescriptor
			{
				get
				{
					return this._propDesc;
				}
			}

			// Token: 0x040017A0 RID: 6048
			private PropertyDescriptor _propDesc;

			// Token: 0x040017A1 RID: 6049
			private DataBindingsDialog.BindingMode _bindingMode;

			// Token: 0x040017A2 RID: 6050
			private bool _twoWayBoundByDefault;

			// Token: 0x040017A3 RID: 6051
			private bool _twoWayBoundByDefaultValid;
		}

		// Token: 0x02000345 RID: 837
		private enum BindingMode
		{
			// Token: 0x040017A5 RID: 6053
			NotSet,
			// Token: 0x040017A6 RID: 6054
			OneWay,
			// Token: 0x040017A7 RID: 6055
			TwoWay
		}

		// Token: 0x02000346 RID: 838
		private sealed class FieldItem
		{
			// Token: 0x06001FA3 RID: 8099 RVA: 0x000B514C File Offset: 0x000B414C
			public FieldItem()
				: this(SR.GetString("DBDlg_Unbound"), null)
			{
			}

			// Token: 0x06001FA4 RID: 8100 RVA: 0x000B515F File Offset: 0x000B415F
			public FieldItem(string name, Type type)
			{
				this._name = name;
				this._type = type;
			}

			// Token: 0x17000583 RID: 1411
			// (get) Token: 0x06001FA5 RID: 8101 RVA: 0x000B5175 File Offset: 0x000B4175
			public Type Type
			{
				get
				{
					return this._type;
				}
			}

			// Token: 0x06001FA6 RID: 8102 RVA: 0x000B517D File Offset: 0x000B417D
			public override string ToString()
			{
				return this._name;
			}

			// Token: 0x040017A8 RID: 6056
			private string _name;

			// Token: 0x040017A9 RID: 6057
			private Type _type;
		}

		// Token: 0x02000347 RID: 839
		private class FormatItem
		{
			// Token: 0x06001FA7 RID: 8103 RVA: 0x000B5188 File Offset: 0x000B4188
			private FormatItem(string displayText, string format)
			{
				this._displayText = string.Format(CultureInfo.CurrentCulture, displayText, new object[] { format });
				this._format = format;
			}

			// Token: 0x17000584 RID: 1412
			// (get) Token: 0x06001FA8 RID: 8104 RVA: 0x000B51BF File Offset: 0x000B41BF
			public string Format
			{
				get
				{
					return this._format;
				}
			}

			// Token: 0x06001FA9 RID: 8105 RVA: 0x000B51C7 File Offset: 0x000B41C7
			public override string ToString()
			{
				return this._displayText;
			}

			// Token: 0x040017AA RID: 6058
			private static readonly DataBindingsDialog.FormatItem nullFormat = new DataBindingsDialog.FormatItem(SR.GetString("DBDlg_Fmt_None"), string.Empty);

			// Token: 0x040017AB RID: 6059
			private static readonly DataBindingsDialog.FormatItem generalFormat = new DataBindingsDialog.FormatItem(SR.GetString("DBDlg_Fmt_General"), "{0}");

			// Token: 0x040017AC RID: 6060
			private static readonly DataBindingsDialog.FormatItem dtShortTime = new DataBindingsDialog.FormatItem(SR.GetString("DBDlg_Fmt_ShortTime"), "{0:t}");

			// Token: 0x040017AD RID: 6061
			private static readonly DataBindingsDialog.FormatItem dtLongTime = new DataBindingsDialog.FormatItem(SR.GetString("DBDlg_Fmt_LongTime"), "{0:T}");

			// Token: 0x040017AE RID: 6062
			private static readonly DataBindingsDialog.FormatItem dtShortDate = new DataBindingsDialog.FormatItem(SR.GetString("DBDlg_Fmt_ShortDate"), "{0:d}");

			// Token: 0x040017AF RID: 6063
			private static readonly DataBindingsDialog.FormatItem dtLongDate = new DataBindingsDialog.FormatItem(SR.GetString("DBDlg_Fmt_LongDate"), "{0:D}");

			// Token: 0x040017B0 RID: 6064
			private static readonly DataBindingsDialog.FormatItem dtDateTime = new DataBindingsDialog.FormatItem(SR.GetString("DBDlg_Fmt_DateTime"), "{0:g}");

			// Token: 0x040017B1 RID: 6065
			private static readonly DataBindingsDialog.FormatItem dtFullDate = new DataBindingsDialog.FormatItem(SR.GetString("DBDlg_Fmt_FullDate"), "{0:G}");

			// Token: 0x040017B2 RID: 6066
			private static readonly DataBindingsDialog.FormatItem numNumber = new DataBindingsDialog.FormatItem(SR.GetString("DBDlg_Fmt_Numeric"), "{0:N}");

			// Token: 0x040017B3 RID: 6067
			private static readonly DataBindingsDialog.FormatItem numDecimal = new DataBindingsDialog.FormatItem(SR.GetString("DBDlg_Fmt_Decimal"), "{0:D}");

			// Token: 0x040017B4 RID: 6068
			private static readonly DataBindingsDialog.FormatItem numFixed = new DataBindingsDialog.FormatItem(SR.GetString("DBDlg_Fmt_Fixed"), "{0:F}");

			// Token: 0x040017B5 RID: 6069
			private static readonly DataBindingsDialog.FormatItem numCurrency = new DataBindingsDialog.FormatItem(SR.GetString("DBDlg_Fmt_Currency"), "{0:C}");

			// Token: 0x040017B6 RID: 6070
			private static readonly DataBindingsDialog.FormatItem numScientific = new DataBindingsDialog.FormatItem(SR.GetString("DBDlg_Fmt_Scientific"), "{0:E}");

			// Token: 0x040017B7 RID: 6071
			private static readonly DataBindingsDialog.FormatItem numHex = new DataBindingsDialog.FormatItem(SR.GetString("DBDlg_Fmt_Hexadecimal"), "0x{0:X}");

			// Token: 0x040017B8 RID: 6072
			public static readonly DataBindingsDialog.FormatItem[] DefaultFormats = new DataBindingsDialog.FormatItem[]
			{
				DataBindingsDialog.FormatItem.nullFormat,
				DataBindingsDialog.FormatItem.generalFormat
			};

			// Token: 0x040017B9 RID: 6073
			public static readonly DataBindingsDialog.FormatItem[] DateTimeFormats = new DataBindingsDialog.FormatItem[]
			{
				DataBindingsDialog.FormatItem.nullFormat,
				DataBindingsDialog.FormatItem.generalFormat,
				DataBindingsDialog.FormatItem.dtShortTime,
				DataBindingsDialog.FormatItem.dtLongTime,
				DataBindingsDialog.FormatItem.dtShortDate,
				DataBindingsDialog.FormatItem.dtLongDate,
				DataBindingsDialog.FormatItem.dtDateTime,
				DataBindingsDialog.FormatItem.dtFullDate
			};

			// Token: 0x040017BA RID: 6074
			public static readonly DataBindingsDialog.FormatItem[] NumericFormats = new DataBindingsDialog.FormatItem[]
			{
				DataBindingsDialog.FormatItem.nullFormat,
				DataBindingsDialog.FormatItem.generalFormat,
				DataBindingsDialog.FormatItem.numNumber,
				DataBindingsDialog.FormatItem.numDecimal,
				DataBindingsDialog.FormatItem.numFixed,
				DataBindingsDialog.FormatItem.numCurrency,
				DataBindingsDialog.FormatItem.numScientific,
				DataBindingsDialog.FormatItem.numHex
			};

			// Token: 0x040017BB RID: 6075
			public static readonly DataBindingsDialog.FormatItem[] DecimalFormats = new DataBindingsDialog.FormatItem[]
			{
				DataBindingsDialog.FormatItem.nullFormat,
				DataBindingsDialog.FormatItem.generalFormat,
				DataBindingsDialog.FormatItem.numNumber,
				DataBindingsDialog.FormatItem.numDecimal,
				DataBindingsDialog.FormatItem.numFixed,
				DataBindingsDialog.FormatItem.numCurrency,
				DataBindingsDialog.FormatItem.numScientific
			};

			// Token: 0x040017BC RID: 6076
			private readonly string _displayText;

			// Token: 0x040017BD RID: 6077
			private readonly string _format;
		}
	}
}
