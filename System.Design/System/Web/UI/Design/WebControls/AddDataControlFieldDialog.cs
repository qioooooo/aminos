using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Security.Permissions;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed partial class AddDataControlFieldDialog : DesignerForm
	{
		public AddDataControlFieldDialog(DataBoundControlDesigner controlDesigner)
			: base(controlDesigner.Component.Site)
		{
			this._controlDesigner = controlDesigner;
			this.IgnoreRefreshSchemaEvents();
			this.InitForm();
		}

		private DataBoundControl Control
		{
			get
			{
				return this._controlDesigner.Component as DataBoundControl;
			}
		}

		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.DataControlField.AddDataControlFieldDialog";
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

		private void AddControls()
		{
			this._okButton.SetBounds(162, 475, 75, 23);
			this._okButton.Click += this.OnClickOKButton;
			this._okButton.Text = SR.GetString("OKCaption");
			this._okButton.TabIndex = 201;
			this._okButton.FlatStyle = FlatStyle.System;
			this._okButton.DialogResult = DialogResult.OK;
			this._cancelButton.SetBounds(243, 475, 75, 23);
			this._cancelButton.DialogResult = DialogResult.Cancel;
			this._cancelButton.Text = SR.GetString("CancelCaption");
			this._cancelButton.FlatStyle = FlatStyle.System;
			this._cancelButton.TabIndex = 202;
			this._cancelButton.DialogResult = DialogResult.Cancel;
			this._fieldLabel.Text = SR.GetString("DCFAdd_ChooseField");
			this._fieldLabel.TabStop = false;
			this._fieldLabel.TextAlign = ContentAlignment.BottomLeft;
			this._fieldLabel.SetBounds(12, 12, 306, 17);
			this._fieldLabel.TabIndex = 0;
			this._fieldList.DropDownStyle = ComboBoxStyle.DropDownList;
			this._fieldList.TabIndex = 1;
			this._controlsPanel.SetBounds(12, this.fieldControlTop, 330, 510 - this.fieldControlTop - 12 - 23 - 4);
			this._controlsPanel.TabIndex = 100;
			for (int i = 0; i < this.GetDataControlFieldControls().Length; i++)
			{
				AddDataControlFieldDialog.DataControlFieldControl dataControlFieldControl = this.GetDataControlFieldControls()[i];
				this._fieldList.Items.Add(dataControlFieldControl.FieldName);
				dataControlFieldControl.Visible = false;
				dataControlFieldControl.TabStop = false;
				dataControlFieldControl.SetBounds(0, 0, 330, 510 - this.fieldControlTop - 12 - 23 - 4);
				this._controlsPanel.Controls.Add(dataControlFieldControl);
			}
			this._fieldList.SelectedIndex = 0;
			this._fieldList.SelectedIndexChanged += this.OnSelectedFieldTypeChanged;
			this.SetSelectedFieldControlVisible();
			this._fieldList.SetBounds(12, 31, 150, 20);
			this._refreshSchemaLink.SetBounds(12, 475, 100, 42);
			this._refreshSchemaLink.TabIndex = 200;
			this._refreshSchemaLink.Visible = false;
			this._refreshSchemaLink.Text = SR.GetString("DataSourceDesigner_RefreshSchemaNoHotkey");
			this._refreshSchemaLink.UseMnemonic = true;
			this._refreshSchemaLink.LinkClicked += this.OnClickRefreshSchema;
			base.AcceptButton = this._okButton;
			base.CancelButton = this._cancelButton;
			base.Controls.AddRange(new Control[] { this._cancelButton, this._okButton, this._fieldLabel, this._fieldList, this._controlsPanel, this._refreshSchemaLink });
		}

		private IDataSourceFieldSchema[] GetBooleanFieldSchemas()
		{
			IDataSourceFieldSchema[] fieldSchemas = this.GetFieldSchemas();
			ArrayList arrayList = new ArrayList();
			IDataSourceFieldSchema[] array = null;
			if (fieldSchemas != null)
			{
				foreach (IDataSourceFieldSchema dataSourceFieldSchema in fieldSchemas)
				{
					if (dataSourceFieldSchema.DataType == typeof(bool))
					{
						arrayList.Add(dataSourceFieldSchema);
					}
				}
				array = new IDataSourceFieldSchema[arrayList.Count];
				arrayList.CopyTo(array);
			}
			return array;
		}

		private List<AddDataControlFieldDialog.DataControlFieldControl> GetDesignerDataControlFieldControls()
		{
			if (this._customFieldDesigners == null)
			{
				this._customFieldDesigners = DataControlFieldHelper.GetCustomFieldDesigners(this, this.Control);
			}
			Type type = this.Control.GetType();
			List<AddDataControlFieldDialog.DataControlFieldControl> list = new List<AddDataControlFieldDialog.DataControlFieldControl>();
			foreach (KeyValuePair<Type, DataControlFieldDesigner> keyValuePair in this._customFieldDesigners)
			{
				DataControlFieldDesigner value = keyValuePair.Value;
				list.Add(new AddDataControlFieldDialog.DataControlFieldDesignerControl(this._controlDesigner, base.ServiceProvider, value, null, type));
			}
			return list;
		}

		private AddDataControlFieldDialog.DataControlFieldControl[] GetDataControlFieldControls()
		{
			Type type = this.Control.GetType();
			if (this._dataControlFieldControls == null)
			{
				List<AddDataControlFieldDialog.DataControlFieldControl> designerDataControlFieldControls = this.GetDesignerDataControlFieldControls();
				DataControlFieldDesigner dataControlFieldDesigner = null;
				AddDataControlFieldDialog.DataControlFieldControl dataControlFieldControl = null;
				foreach (AddDataControlFieldDialog.DataControlFieldControl dataControlFieldControl2 in designerDataControlFieldControls)
				{
					AddDataControlFieldDialog.DataControlFieldDesignerControl dataControlFieldDesignerControl = dataControlFieldControl2 as AddDataControlFieldDialog.DataControlFieldDesignerControl;
					if (dataControlFieldDesignerControl != null && dataControlFieldDesignerControl.Designer.GetType().FullName == "System.Web.DynamicData.Design.DynamicFieldDesigner")
					{
						dataControlFieldControl = dataControlFieldDesignerControl;
						dataControlFieldDesigner = dataControlFieldDesignerControl.Designer;
						this._dynamicDataEnabled = true;
						break;
					}
				}
				if (this._dynamicDataEnabled)
				{
					designerDataControlFieldControls.Remove(dataControlFieldControl);
				}
				int num = (this._dynamicDataEnabled ? 8 : 7);
				this._dataControlFieldControls = new AddDataControlFieldDialog.DataControlFieldControl[num + designerDataControlFieldControls.Count];
				this._dataControlFieldControls[0] = new AddDataControlFieldDialog.BoundFieldControl(this.GetFieldSchemas(), type);
				this._dataControlFieldControls[1] = new AddDataControlFieldDialog.CheckBoxFieldControl(this.GetBooleanFieldSchemas(), type);
				this._dataControlFieldControls[2] = new AddDataControlFieldDialog.HyperLinkFieldControl(this.GetFieldSchemas(), type);
				this._dataControlFieldControls[3] = new AddDataControlFieldDialog.ButtonFieldControl(null, type);
				this._dataControlFieldControls[4] = new AddDataControlFieldDialog.CommandFieldControl(null, type);
				this._dataControlFieldControls[5] = new AddDataControlFieldDialog.ImageFieldControl(this.GetFieldSchemas(), type);
				this._dataControlFieldControls[6] = new AddDataControlFieldDialog.TemplateFieldControl(null, type);
				if (this._dynamicDataEnabled)
				{
					this._dataControlFieldControls[7] = new AddDataControlFieldDialog.DynamicDataFieldControl(dataControlFieldDesigner, this.GetFieldSchemas(), type);
				}
				int num2 = num;
				foreach (AddDataControlFieldDialog.DataControlFieldControl dataControlFieldControl3 in designerDataControlFieldControls)
				{
					this._dataControlFieldControls[num2++] = dataControlFieldControl3;
				}
			}
			return this._dataControlFieldControls;
		}

		private IDataSourceFieldSchema[] GetFieldSchemas()
		{
			if (this._fieldSchemas == null)
			{
				IDataSourceViewSchema dataSourceViewSchema = null;
				if (this._controlDesigner != null)
				{
					DesignerDataSourceView designerView = this._controlDesigner.DesignerView;
					if (designerView != null)
					{
						try
						{
							dataSourceViewSchema = designerView.Schema;
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
				if (dataSourceViewSchema != null)
				{
					this._fieldSchemas = dataSourceViewSchema.GetFields();
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
			base.SuspendLayout();
			this._okButton = new global::System.Windows.Forms.Button();
			this._cancelButton = new global::System.Windows.Forms.Button();
			this._fieldLabel = new global::System.Windows.Forms.Label();
			this._fieldList = new ComboBox();
			this._refreshSchemaLink = new LinkLabel();
			this._controlsPanel = new global::System.Windows.Forms.Panel();
			this.AddControls();
			IDataSourceDesigner dataSourceDesigner = this._controlDesigner.DataSourceDesigner;
			if (dataSourceDesigner != null && dataSourceDesigner.CanRefreshSchema)
			{
				this._refreshSchemaLink.Visible = true;
			}
			this.Text = SR.GetString("DCFAdd_Title");
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.ClientSize = new Size(330, 510);
			base.AcceptButton = this._okButton;
			base.CancelButton = this._cancelButton;
			base.Icon = null;
			base.InitializeForm();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private void OnClickOKButton(object sender, EventArgs e)
		{
			AddDataControlFieldDialog.DataControlFieldControl dataControlFieldControl = this.GetDataControlFieldControls()[this._fieldList.SelectedIndex];
			DataBoundControl control = this.Control;
			if (control is GridView)
			{
				((GridView)control).Columns.Add(dataControlFieldControl.SaveValues());
				return;
			}
			if (control is DetailsView)
			{
				((DetailsView)control).Fields.Add(dataControlFieldControl.SaveValues());
			}
		}

		private void OnClickRefreshSchema(object source, LinkLabelLinkClickedEventArgs e)
		{
			if (this._controlDesigner != null)
			{
				IDataSourceDesigner dataSourceDesigner = this._controlDesigner.DataSourceDesigner;
				if (dataSourceDesigner != null && dataSourceDesigner.CanRefreshSchema)
				{
					IDictionary dictionary = this.GetDataControlFieldControls()[this._fieldList.SelectedIndex].PreserveFields();
					dataSourceDesigner.RefreshSchema(false);
					this._fieldSchemas = this.GetFieldSchemas();
					this.GetDataControlFieldControls()[0].RefreshSchema(this._fieldSchemas);
					this.GetDataControlFieldControls()[1].RefreshSchema(this.GetBooleanFieldSchemas());
					this.GetDataControlFieldControls()[2].RefreshSchema(this._fieldSchemas);
					this.GetDataControlFieldControls()[5].RefreshSchema(this._fieldSchemas);
					if (this._dynamicDataEnabled)
					{
						this._dataControlFieldControls[7].RefreshSchema(this._fieldSchemas);
					}
					this.GetDataControlFieldControls()[this._fieldList.SelectedIndex].RestoreFields(dictionary);
				}
			}
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

		private void OnSelectedFieldTypeChanged(object sender, EventArgs e)
		{
			this.SetSelectedFieldControlVisible();
		}

		private void SetSelectedFieldControlVisible()
		{
			foreach (AddDataControlFieldDialog.DataControlFieldControl dataControlFieldControl in this.GetDataControlFieldControls())
			{
				dataControlFieldControl.Visible = false;
			}
			this.GetDataControlFieldControls()[this._fieldList.SelectedIndex].Visible = true;
			this.Refresh();
		}

		private const int buttonWidth = 75;

		private const int buttonHeight = 23;

		private const int formHeight = 510;

		private const int formWidth = 330;

		private const int labelLeft = 12;

		private const int labelHeight = 17;

		private const int labelPadding = 2;

		private const int labelWidth = 270;

		private const int controlLeft = 12;

		private const int controlHeight = 20;

		private const int fieldChooserWidth = 150;

		private const int textBoxWidth = 270;

		private const int vertPadding = 4;

		private const int horizPadding = 6;

		private const int topPadding = 12;

		private const int bottomPadding = 12;

		private const int rightPadding = 12;

		private const int linkWidth = 100;

		private const int checkBoxWidth = 125;

		private bool _dynamicDataEnabled;

		private DataBoundControlDesigner _controlDesigner;

		private AddDataControlFieldDialog.DataControlFieldControl[] _dataControlFieldControls;

		private IDataSourceFieldSchema[] _fieldSchemas;

		private bool _initialIgnoreRefreshSchemaValue;

		private global::System.Windows.Forms.Button _okButton;

		private global::System.Windows.Forms.Button _cancelButton;

		private global::System.Windows.Forms.Label _fieldLabel;

		private ComboBox _fieldList;

		private LinkLabel _refreshSchemaLink;

		private global::System.Windows.Forms.Panel _controlsPanel;

		private int fieldControlTop = 51;

		private IDictionary<Type, DataControlFieldDesigner> _customFieldDesigners;

		private abstract class DataControlFieldControl : Control
		{
			public DataControlFieldControl(IDataSourceFieldSchema[] fieldSchemas, Type controlType)
			{
				this._fieldSchemas = fieldSchemas;
				if (fieldSchemas != null && fieldSchemas.Length > 0)
				{
					this._haveSchema = true;
				}
				this._controlType = controlType;
				this.InitializeComponent();
			}

			public abstract string FieldName { get; }

			protected string[] GetFieldSchemaNames()
			{
				if (this._fieldSchemaNames == null && this._fieldSchemas != null)
				{
					int num = this._fieldSchemas.Length;
					this._fieldSchemaNames = new string[num];
					for (int i = 0; i < num; i++)
					{
						this._fieldSchemaNames[i] = this._fieldSchemas[i].Name;
					}
				}
				return this._fieldSchemaNames;
			}

			protected virtual void InitializeComponent()
			{
				this._headerTextLabel = new global::System.Windows.Forms.Label();
				this._headerTextBox = new global::System.Windows.Forms.TextBox();
				this._headerTextLabel.Text = SR.GetString("DCFAdd_HeaderText");
				this._headerTextLabel.TextAlign = ContentAlignment.BottomLeft;
				this._headerTextLabel.SetBounds(0, 0, 270, 17);
				this._headerTextBox.TabIndex = 0;
				this._headerTextBox.SetBounds(0, 19, 270, 20);
				base.Controls.AddRange(new Control[] { this._headerTextLabel, this._headerTextBox });
			}

			public IDictionary PreserveFields()
			{
				Hashtable hashtable = new Hashtable();
				hashtable["HeaderText"] = this._headerTextBox.Text;
				this.PreserveFields(hashtable);
				return hashtable;
			}

			protected abstract void PreserveFields(IDictionary table);

			public void RefreshSchema(IDataSourceFieldSchema[] fieldSchemas)
			{
				this._fieldSchemas = fieldSchemas;
				this._fieldSchemaNames = null;
				if (fieldSchemas != null && fieldSchemas.Length > 0)
				{
					this._haveSchema = true;
				}
				this.RefreshSchemaFields();
			}

			protected virtual void RefreshSchemaFields()
			{
			}

			public void RestoreFields(IDictionary table)
			{
				this._headerTextBox.Text = table["HeaderText"].ToString();
				this.RestoreFieldsInternal(table);
			}

			protected abstract void RestoreFieldsInternal(IDictionary table);

			protected abstract DataControlField SaveValues(string headerText);

			public DataControlField SaveValues()
			{
				string text = ((this._headerTextBox == null) ? string.Empty : this._headerTextBox.Text);
				return this.SaveValues(text);
			}

			protected string StripAccelerators(string text)
			{
				return text.Replace("&", string.Empty);
			}

			protected string[] _fieldSchemaNames;

			protected Type _controlType;

			protected bool _haveSchema;

			protected IDataSourceFieldSchema[] _fieldSchemas;

			private global::System.Windows.Forms.Label _headerTextLabel;

			private global::System.Windows.Forms.TextBox _headerTextBox;
		}

		private class BoundFieldControl : AddDataControlFieldDialog.DataControlFieldControl
		{
			public BoundFieldControl(IDataSourceFieldSchema[] fieldSchemas, Type controlType)
				: base(fieldSchemas, controlType)
			{
			}

			public override string FieldName
			{
				get
				{
					return "BoundField";
				}
			}

			protected override void InitializeComponent()
			{
				base.InitializeComponent();
				this._dataFieldList = new ComboBox();
				this._dataFieldBox = new global::System.Windows.Forms.TextBox();
				this._dataFieldLabel = new global::System.Windows.Forms.Label();
				this._readOnlyCheckBox = new global::System.Windows.Forms.CheckBox();
				this._dataFieldLabel.Text = SR.GetString("DCFAdd_DataField");
				this._dataFieldLabel.TextAlign = ContentAlignment.BottomLeft;
				this._dataFieldLabel.SetBounds(0, 43, 270, 17);
				this._dataFieldList.DropDownStyle = ComboBoxStyle.DropDownList;
				this._dataFieldList.TabIndex = 1;
				this._dataFieldList.SetBounds(0, 62, 270, 20);
				this._dataFieldList.SelectedIndexChanged += this.OnSelectedDataFieldChanged;
				this._dataFieldBox.TabIndex = 1;
				this._dataFieldBox.SetBounds(0, 62, 270, 20);
				this._readOnlyCheckBox.TabIndex = 2;
				this._readOnlyCheckBox.Text = SR.GetString("DCFAdd_ReadOnly");
				this._readOnlyCheckBox.SetBounds(0, 86, 270, 20);
				this.RefreshSchemaFields();
				base.Controls.AddRange(new Control[] { this._dataFieldLabel, this._dataFieldBox, this._dataFieldList, this._readOnlyCheckBox });
			}

			private void OnSelectedDataFieldChanged(object sender, EventArgs e)
			{
				if (this._haveSchema)
				{
					int num = Array.IndexOf<string>(base.GetFieldSchemaNames(), this._dataFieldList.Text);
					if (num >= 0 && this._fieldSchemas[num].PrimaryKey)
					{
						this._readOnlyCheckBox.Checked = true;
						return;
					}
				}
				this._readOnlyCheckBox.Checked = false;
			}

			protected override void PreserveFields(IDictionary table)
			{
				if (this._haveSchema)
				{
					table["DataField"] = this._dataFieldList.Text;
				}
				else
				{
					table["DataField"] = this._dataFieldBox.Text;
				}
				table["ReadOnly"] = this._readOnlyCheckBox.Checked;
			}

			protected override void RefreshSchemaFields()
			{
				if (this._haveSchema)
				{
					this._dataFieldList.Items.Clear();
					this._dataFieldList.Items.AddRange(base.GetFieldSchemaNames());
					this._dataFieldList.SelectedIndex = 0;
					this._dataFieldList.Visible = true;
					this._dataFieldBox.Visible = false;
					return;
				}
				this._dataFieldList.Visible = false;
				this._dataFieldBox.Visible = true;
			}

			protected override void RestoreFieldsInternal(IDictionary table)
			{
				string text = table["DataField"].ToString();
				if (this._haveSchema)
				{
					if (text.Length > 0)
					{
						bool flag = false;
						foreach (object obj in this._dataFieldList.Items)
						{
							if (string.Compare(text, obj.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
							{
								this._dataFieldList.SelectedItem = obj;
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							this._dataFieldList.Items.Insert(0, text);
							this._dataFieldList.SelectedIndex = 0;
						}
					}
				}
				else
				{
					this._dataFieldBox.Text = text;
				}
				this._readOnlyCheckBox.Checked = (bool)table["ReadOnly"];
			}

			protected override DataControlField SaveValues(string headerText)
			{
				BoundField boundField = new BoundField();
				boundField.HeaderText = headerText;
				if (this._haveSchema)
				{
					boundField.DataField = this._dataFieldList.Text;
				}
				else
				{
					boundField.DataField = this._dataFieldBox.Text;
				}
				boundField.ReadOnly = this._readOnlyCheckBox.Checked;
				boundField.SortExpression = boundField.DataField;
				return boundField;
			}

			private global::System.Windows.Forms.Label _dataFieldLabel;

			protected ComboBox _dataFieldList;

			protected global::System.Windows.Forms.TextBox _dataFieldBox;

			protected global::System.Windows.Forms.CheckBox _readOnlyCheckBox;
		}

		private class DynamicDataFieldControl : AddDataControlFieldDialog.BoundFieldControl
		{
			public DynamicDataFieldControl(DataControlFieldDesigner dynamicFieldDesigner, IDataSourceFieldSchema[] fieldSchemas, Type controlType)
				: base(fieldSchemas, controlType)
			{
				this._designer = dynamicFieldDesigner;
			}

			public override string FieldName
			{
				get
				{
					return "DynamicField";
				}
			}

			protected override void InitializeComponent()
			{
				base.InitializeComponent();
				this._readOnlyCheckBox.Visible = false;
			}

			protected override DataControlField SaveValues(string headerText)
			{
				DataControlField dataControlField = this._designer.CreateField();
				dataControlField.HeaderText = headerText;
				string text = (this._haveSchema ? this._dataFieldList.Text : this._dataFieldBox.Text);
				AddDataControlFieldDialog.DynamicDataFieldControl.SetProperty(dataControlField, "DataField", text);
				return dataControlField;
			}

			private static void SetProperty(DataControlField target, string propertyName, object value)
			{
				target.GetType().GetProperty(propertyName).SetValue(target, value, null);
			}

			private DataControlFieldDesigner _designer;
		}

		private class CheckBoxFieldControl : AddDataControlFieldDialog.DataControlFieldControl
		{
			public CheckBoxFieldControl(IDataSourceFieldSchema[] fieldSchemas, Type controlType)
				: base(fieldSchemas, controlType)
			{
			}

			public override string FieldName
			{
				get
				{
					return "CheckBoxField";
				}
			}

			protected override void InitializeComponent()
			{
				base.InitializeComponent();
				this._dataFieldList = new ComboBox();
				this._dataFieldBox = new global::System.Windows.Forms.TextBox();
				this._dataFieldLabel = new global::System.Windows.Forms.Label();
				this._readOnlyCheckBox = new global::System.Windows.Forms.CheckBox();
				this._dataFieldLabel.Text = SR.GetString("DCFAdd_DataField");
				this._dataFieldLabel.TextAlign = ContentAlignment.BottomLeft;
				this._dataFieldLabel.SetBounds(0, 43, 270, 17);
				this._dataFieldList.DropDownStyle = ComboBoxStyle.DropDownList;
				this._dataFieldList.TabIndex = 1;
				this._dataFieldList.SetBounds(0, 62, 270, 20);
				this._dataFieldBox.TabIndex = 1;
				this._dataFieldBox.SetBounds(0, 62, 270, 20);
				this._readOnlyCheckBox.TabIndex = 2;
				this._readOnlyCheckBox.Text = SR.GetString("DCFAdd_ReadOnly");
				this._readOnlyCheckBox.SetBounds(0, 86, 270, 20);
				this.RefreshSchemaFields();
				base.Controls.AddRange(new Control[] { this._dataFieldLabel, this._dataFieldBox, this._dataFieldList, this._readOnlyCheckBox });
			}

			protected override void PreserveFields(IDictionary table)
			{
				if (this._haveSchema)
				{
					table["DataField"] = this._dataFieldList.Text;
				}
				else
				{
					table["DataField"] = this._dataFieldBox.Text;
				}
				table["ReadOnly"] = this._readOnlyCheckBox.Checked;
			}

			protected override void RefreshSchemaFields()
			{
				if (this._haveSchema)
				{
					this._dataFieldList.Items.Clear();
					this._dataFieldList.Items.AddRange(base.GetFieldSchemaNames());
					this._dataFieldList.SelectedIndex = 0;
					this._dataFieldList.Visible = true;
					this._dataFieldBox.Visible = false;
					return;
				}
				this._dataFieldList.Visible = false;
				this._dataFieldBox.Visible = true;
			}

			protected override void RestoreFieldsInternal(IDictionary table)
			{
				string text = table["DataField"].ToString();
				if (this._haveSchema)
				{
					if (text.Length > 0)
					{
						bool flag = false;
						foreach (object obj in this._dataFieldList.Items)
						{
							if (string.Compare(text, obj.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
							{
								this._dataFieldList.SelectedItem = obj;
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							this._dataFieldList.Items.Insert(0, text);
							this._dataFieldList.SelectedIndex = 0;
						}
					}
				}
				else
				{
					this._dataFieldBox.Text = text;
				}
				this._readOnlyCheckBox.Checked = (bool)table["ReadOnly"];
			}

			protected override DataControlField SaveValues(string headerText)
			{
				CheckBoxField checkBoxField = new CheckBoxField();
				checkBoxField.HeaderText = headerText;
				if (this._haveSchema)
				{
					checkBoxField.DataField = this._dataFieldList.Text;
				}
				else
				{
					checkBoxField.DataField = this._dataFieldBox.Text;
				}
				checkBoxField.ReadOnly = this._readOnlyCheckBox.Checked;
				checkBoxField.SortExpression = checkBoxField.DataField;
				return checkBoxField;
			}

			private global::System.Windows.Forms.Label _dataFieldLabel;

			private ComboBox _dataFieldList;

			private global::System.Windows.Forms.TextBox _dataFieldBox;

			private global::System.Windows.Forms.CheckBox _readOnlyCheckBox;
		}

		private class ButtonFieldControl : AddDataControlFieldDialog.DataControlFieldControl
		{
			public ButtonFieldControl(IDataSourceFieldSchema[] fieldSchemas, Type controlType)
				: base(fieldSchemas, controlType)
			{
			}

			public override string FieldName
			{
				get
				{
					return "ButtonField";
				}
			}

			protected override void InitializeComponent()
			{
				base.InitializeComponent();
				this._buttonTypeLabel = new global::System.Windows.Forms.Label();
				this._commandNameLabel = new global::System.Windows.Forms.Label();
				this._textLabel = new global::System.Windows.Forms.Label();
				this._buttonTypeList = new ComboBox();
				this._commandNameList = new ComboBox();
				this._textBox = new global::System.Windows.Forms.TextBox();
				this._buttonTypeLabel.Text = SR.GetString("DCFAdd_ButtonType");
				this._buttonTypeLabel.TextAlign = ContentAlignment.BottomLeft;
				this._buttonTypeLabel.SetBounds(0, 43, 270, 17);
				this._buttonTypeList.Items.Add(ButtonType.Link.ToString());
				this._buttonTypeList.Items.Add(ButtonType.Button.ToString());
				this._buttonTypeList.SelectedIndex = 0;
				this._buttonTypeList.DropDownStyle = ComboBoxStyle.DropDownList;
				this._buttonTypeList.TabIndex = 1;
				this._buttonTypeList.SetBounds(0, 62, 270, 20);
				this._commandNameLabel.Text = SR.GetString("DCFAdd_CommandName");
				this._commandNameLabel.TextAlign = ContentAlignment.BottomLeft;
				this._commandNameLabel.SetBounds(0, 86, 270, 17);
				this._commandNameList.Items.Add("Cancel");
				this._commandNameList.Items.Add("Delete");
				this._commandNameList.Items.Add("Edit");
				this._commandNameList.Items.Add("Update");
				if (this._controlType == typeof(DetailsView))
				{
					this._commandNameList.Items.Insert(3, "Insert");
					this._commandNameList.Items.Insert(4, "New");
				}
				else if (this._controlType == typeof(GridView))
				{
					this._commandNameList.Items.Insert(3, "Select");
				}
				this._commandNameList.SelectedIndex = 0;
				this._commandNameList.DropDownStyle = ComboBoxStyle.DropDownList;
				this._commandNameList.TabIndex = 2;
				this._commandNameList.SetBounds(0, 105, 270, 20);
				this._textLabel.Text = SR.GetString("DCFAdd_Text");
				this._textLabel.TextAlign = ContentAlignment.BottomLeft;
				this._textLabel.SetBounds(0, 129, 270, 17);
				this._textBox.TabIndex = 3;
				this._textBox.Text = SR.GetString("DCFEditor_Button");
				this._textBox.SetBounds(0, 148, 270, 20);
				base.Controls.AddRange(new Control[] { this._buttonTypeLabel, this._commandNameLabel, this._textLabel, this._buttonTypeList, this._commandNameList, this._textBox });
			}

			protected override void PreserveFields(IDictionary table)
			{
				table["ButtonType"] = this._buttonTypeList.SelectedIndex;
				table["CommandName"] = this._commandNameList.SelectedIndex;
				table["Text"] = this._textBox.Text;
			}

			protected override void RestoreFieldsInternal(IDictionary table)
			{
				this._buttonTypeList.SelectedIndex = (int)table["ButtonType"];
				this._commandNameList.SelectedIndex = (int)table["CommandName"];
				this._textBox.Text = table["Text"].ToString();
			}

			protected override DataControlField SaveValues(string headerText)
			{
				ButtonField buttonField = new ButtonField();
				if (headerText != null && headerText.Length > 0)
				{
					buttonField.HeaderText = headerText;
					buttonField.ShowHeader = true;
				}
				buttonField.CommandName = this._commandNameList.Text;
				buttonField.Text = this._textBox.Text;
				if (this._buttonTypeList.SelectedIndex == 0)
				{
					buttonField.ButtonType = ButtonType.Link;
				}
				else
				{
					buttonField.ButtonType = ButtonType.Button;
				}
				return buttonField;
			}

			private global::System.Windows.Forms.Label _buttonTypeLabel;

			private global::System.Windows.Forms.Label _commandNameLabel;

			private global::System.Windows.Forms.Label _textLabel;

			private ComboBox _buttonTypeList;

			private ComboBox _commandNameList;

			private global::System.Windows.Forms.TextBox _textBox;
		}

		private class HyperLinkFieldControl : AddDataControlFieldDialog.DataControlFieldControl
		{
			public HyperLinkFieldControl(IDataSourceFieldSchema[] fieldSchemas, Type controlType)
				: base(fieldSchemas, controlType)
			{
			}

			public override string FieldName
			{
				get
				{
					return "HyperLinkField";
				}
			}

			protected override void InitializeComponent()
			{
				base.InitializeComponent();
				this._dataTextFieldBox = new global::System.Windows.Forms.TextBox();
				this._dataNavFieldBox = new global::System.Windows.Forms.TextBox();
				this._dataNavFSBox = new global::System.Windows.Forms.TextBox();
				this._linkBox = new global::System.Windows.Forms.TextBox();
				this._textBox = new global::System.Windows.Forms.TextBox();
				this._textFSBox = new global::System.Windows.Forms.TextBox();
				this._dataTextFieldList = new ComboBox();
				this._dataNavFieldList = new ComboBox();
				this._staticTextRadio = new global::System.Windows.Forms.RadioButton();
				this._bindTextRadio = new global::System.Windows.Forms.RadioButton();
				this._staticUrlRadio = new global::System.Windows.Forms.RadioButton();
				this._bindUrlRadio = new global::System.Windows.Forms.RadioButton();
				this._linkTextFormatStringLabel = new global::System.Windows.Forms.Label();
				this._linkUrlFormatStringLabel = new global::System.Windows.Forms.Label();
				this._linkTextFormatStringExampleLabel = new global::System.Windows.Forms.Label();
				this._linkUrlFormatStringExampleLabel = new global::System.Windows.Forms.Label();
				this._textGroupBox = new GroupBox();
				this._linkGroupBox = new GroupBox();
				this._staticTextPanel = new global::System.Windows.Forms.Panel();
				this._bindTextPanel = new global::System.Windows.Forms.Panel();
				this._staticUrlPanel = new global::System.Windows.Forms.Panel();
				this._bindUrlPanel = new global::System.Windows.Forms.Panel();
				this._textGroupBox.SetBounds(0, 47, 290, 169);
				this._textGroupBox.Text = SR.GetString("DCFAdd_HyperlinkText");
				this._textGroupBox.TabIndex = 1;
				this._staticTextRadio.TabIndex = 0;
				this._staticTextRadio.Text = SR.GetString("DCFAdd_SpecifyText");
				this._staticTextRadio.CheckedChanged += this.OnTextRadioChanged;
				this._staticTextRadio.Checked = true;
				this._staticTextRadio.SetBounds(9, 19, 261, 20);
				this._textBox.TabIndex = 0;
				this._textBox.SetBounds(0, 0, 246, 20);
				this._textBox.AccessibleName = base.StripAccelerators(SR.GetString("DCFAdd_SpecifyText"));
				this._staticTextPanel.TabIndex = 1;
				this._staticTextPanel.SetBounds(24, 39, 246, 24);
				this._staticTextPanel.Controls.Add(this._textBox);
				this._bindTextRadio.TabIndex = 2;
				this._bindTextRadio.Text = SR.GetString("DCFAdd_BindText");
				this._bindTextRadio.SetBounds(9, 63, 261, 20);
				this._dataTextFieldList.TabIndex = 0;
				this._dataTextFieldList.SetBounds(0, 0, 246, 20);
				this._dataTextFieldList.AccessibleName = base.StripAccelerators(SR.GetString("DCFAdd_BindText"));
				this._dataTextFieldBox.TabIndex = 1;
				this._dataTextFieldBox.SetBounds(0, 0, 246, 20);
				this._dataTextFieldBox.AccessibleName = base.StripAccelerators(SR.GetString("DCFAdd_BindText"));
				this._linkTextFormatStringLabel.Text = SR.GetString("DCFAdd_TextFormatString");
				this._linkTextFormatStringLabel.TabIndex = 2;
				this._linkTextFormatStringLabel.TextAlign = ContentAlignment.BottomLeft;
				this._linkTextFormatStringLabel.SetBounds(0, 20, 246, 17);
				this._textFSBox.TabIndex = 3;
				this._textFSBox.SetBounds(0, 39, 246, 20);
				this._linkTextFormatStringExampleLabel.Text = SR.GetString("DCFAdd_TextFormatStringExample");
				this._linkTextFormatStringExampleLabel.Enabled = false;
				this._linkTextFormatStringExampleLabel.TextAlign = ContentAlignment.BottomLeft;
				this._linkTextFormatStringExampleLabel.SetBounds(0, 59, 246, 17);
				this._bindTextPanel.TabIndex = 3;
				this._bindTextPanel.SetBounds(24, 83, 246, 78);
				this._bindTextPanel.Controls.AddRange(new Control[] { this._bindTextRadio, this._dataTextFieldList, this._dataTextFieldBox, this._linkTextFormatStringLabel, this._textFSBox, this._linkTextFormatStringExampleLabel });
				this._textGroupBox.Controls.AddRange(new Control[] { this._staticTextRadio, this._staticTextPanel, this._bindTextRadio, this._bindTextPanel });
				this._linkGroupBox.SetBounds(0, 220, 290, 173);
				this._linkGroupBox.Text = SR.GetString("DCFAdd_HyperlinkURL");
				this._linkGroupBox.TabIndex = 2;
				this._staticUrlRadio.TabIndex = 0;
				this._staticUrlRadio.Text = SR.GetString("DCFAdd_SpecifyURL");
				this._staticUrlRadio.CheckedChanged += this.OnUrlRadioChanged;
				this._staticUrlRadio.Checked = true;
				this._staticUrlRadio.SetBounds(9, 19, 261, 20);
				this._linkBox.TabIndex = 0;
				this._linkBox.SetBounds(0, 0, 246, 20);
				this._linkBox.AccessibleName = base.StripAccelerators(SR.GetString("DCFAdd_SpecifyURL"));
				this._staticUrlPanel.TabIndex = 1;
				this._staticUrlPanel.SetBounds(24, 39, 246, 24);
				this._staticUrlPanel.Controls.Add(this._linkBox);
				this._bindUrlRadio.TabIndex = 2;
				this._bindUrlRadio.Text = SR.GetString("DCFAdd_BindURL");
				this._bindUrlRadio.SetBounds(9, 63, 261, 20);
				this._dataNavFieldList.TabIndex = 0;
				this._dataNavFieldList.SetBounds(0, 0, 246, 20);
				this._dataNavFieldList.AccessibleName = base.StripAccelerators(SR.GetString("DCFAdd_BindURL"));
				this._dataNavFieldBox.TabIndex = 1;
				this._dataNavFieldBox.SetBounds(0, 0, 246, 20);
				this._dataNavFieldBox.AccessibleName = base.StripAccelerators(SR.GetString("DCFAdd_BindURL"));
				this._linkUrlFormatStringLabel.Text = SR.GetString("DCFAdd_URLFormatString");
				this._linkUrlFormatStringLabel.TabIndex = 2;
				this._linkUrlFormatStringLabel.TextAlign = ContentAlignment.BottomLeft;
				this._linkUrlFormatStringLabel.SetBounds(0, 20, 246, 17);
				this._dataNavFSBox.TabIndex = 3;
				this._dataNavFSBox.SetBounds(0, 39, 246, 20);
				this._linkUrlFormatStringExampleLabel.Text = SR.GetString("DCFAdd_URLFormatStringExample");
				this._linkUrlFormatStringExampleLabel.Enabled = false;
				this._linkUrlFormatStringExampleLabel.TextAlign = ContentAlignment.BottomLeft;
				this._linkUrlFormatStringExampleLabel.SetBounds(0, 59, 246, 17);
				this._bindUrlPanel.TabIndex = 3;
				this._bindUrlPanel.SetBounds(24, 83, 246, 78);
				this._bindUrlPanel.Controls.AddRange(new Control[] { this._dataNavFieldList, this._dataNavFieldBox, this._linkUrlFormatStringLabel, this._dataNavFSBox, this._linkUrlFormatStringExampleLabel });
				this._linkGroupBox.Controls.AddRange(new Control[] { this._staticUrlRadio, this._staticUrlPanel, this._bindUrlRadio, this._bindUrlPanel });
				this.RefreshSchemaFields();
				base.Controls.AddRange(new Control[] { this._textGroupBox, this._linkGroupBox });
			}

			private void OnTextRadioChanged(object sender, EventArgs e)
			{
				if (this._staticTextRadio.Checked)
				{
					this._textBox.Enabled = true;
					this._dataTextFieldList.Enabled = false;
					this._dataTextFieldBox.Enabled = false;
					this._textFSBox.Enabled = false;
					this._linkTextFormatStringLabel.Enabled = false;
					return;
				}
				this._textBox.Enabled = false;
				this._dataTextFieldList.Enabled = true;
				this._dataTextFieldBox.Enabled = true;
				this._textFSBox.Enabled = true;
				this._linkTextFormatStringLabel.Enabled = true;
			}

			private void OnUrlRadioChanged(object sender, EventArgs e)
			{
				if (this._staticUrlRadio.Checked)
				{
					this._linkBox.Enabled = true;
					this._dataNavFieldList.Enabled = false;
					this._dataNavFieldBox.Enabled = false;
					this._dataNavFSBox.Enabled = false;
					this._linkUrlFormatStringLabel.Enabled = false;
					return;
				}
				this._linkBox.Enabled = false;
				this._dataNavFieldList.Enabled = true;
				this._dataNavFieldBox.Enabled = true;
				this._dataNavFSBox.Enabled = true;
				this._linkUrlFormatStringLabel.Enabled = true;
			}

			protected override void PreserveFields(IDictionary table)
			{
				if (this._haveSchema)
				{
					table["DataTextField"] = this._dataTextFieldList.Text;
					table["DataNavigateUrlField"] = this._dataNavFieldList.Text;
				}
				else
				{
					table["DataTextField"] = this._dataTextFieldBox.Text;
					table["DataNavigateUrlField"] = this._dataNavFieldBox.Text;
				}
				table["DataNavigateUrlFormatString"] = this._dataNavFSBox.Text;
				table["DataTextFormatString"] = this._textFSBox.Text;
				table["NavigateUrl"] = this._linkBox.Text;
				table["linkMode"] = this._staticUrlRadio.Checked;
				table["textMode"] = this._staticTextRadio.Checked;
				table["Text"] = this._textBox.Text;
			}

			protected override void RefreshSchemaFields()
			{
				if (this._haveSchema)
				{
					this._dataTextFieldList.Items.Clear();
					this._dataTextFieldList.Items.AddRange(base.GetFieldSchemaNames());
					this._dataTextFieldList.Items.Insert(0, string.Empty);
					this._dataTextFieldList.SelectedIndex = 0;
					this._dataTextFieldList.Visible = true;
					this._dataTextFieldBox.Visible = false;
					this._dataNavFieldList.Items.Clear();
					this._dataNavFieldList.Items.AddRange(base.GetFieldSchemaNames());
					this._dataNavFieldList.Items.Insert(0, string.Empty);
					this._dataNavFieldList.SelectedIndex = 0;
					this._dataNavFieldList.Visible = true;
					this._dataNavFieldBox.Visible = false;
					return;
				}
				this._dataTextFieldList.Visible = false;
				this._dataTextFieldBox.Visible = true;
				this._dataNavFieldList.Visible = false;
				this._dataNavFieldBox.Visible = true;
			}

			protected override void RestoreFieldsInternal(IDictionary table)
			{
				string text = table["DataTextField"].ToString();
				string text2 = table["DataNavigateUrlField"].ToString();
				if (this._haveSchema)
				{
					bool flag = false;
					if (text.Length > 0)
					{
						foreach (object obj in this._dataTextFieldList.Items)
						{
							if (string.Compare(text, obj.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
							{
								this._dataTextFieldList.SelectedItem = obj;
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							this._dataTextFieldList.Items.Insert(0, text);
							this._dataTextFieldList.SelectedIndex = 0;
						}
					}
					if (text2.Length > 0)
					{
						flag = false;
						foreach (object obj2 in this._dataNavFieldList.Items)
						{
							if (string.Compare(text2, obj2.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
							{
								this._dataNavFieldList.SelectedItem = obj2;
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							this._dataNavFieldList.Items.Insert(0, text2);
							this._dataNavFieldList.SelectedIndex = 0;
						}
					}
				}
				else
				{
					this._dataTextFieldBox.Text = text;
					this._dataNavFieldBox.Text = text2;
				}
				this._dataNavFSBox.Text = table["DataNavigateUrlFormatString"].ToString();
				this._textFSBox.Text = table["DataTextFormatString"].ToString();
				this._linkBox.Text = table["NavigateUrl"].ToString();
				this._textBox.Text = table["Text"].ToString();
				this._staticUrlRadio.Checked = (bool)table["linkMode"];
				this._staticTextRadio.Checked = (bool)table["textMode"];
			}

			protected override DataControlField SaveValues(string headerText)
			{
				HyperLinkField hyperLinkField = new HyperLinkField();
				hyperLinkField.HeaderText = headerText;
				if (this._staticTextRadio.Checked)
				{
					hyperLinkField.Text = this._textBox.Text;
				}
				else
				{
					hyperLinkField.DataTextFormatString = this._textFSBox.Text;
					if (this._haveSchema)
					{
						hyperLinkField.DataTextField = this._dataTextFieldList.Text;
					}
					else
					{
						hyperLinkField.DataTextField = this._dataTextFieldBox.Text;
					}
				}
				if (this._staticUrlRadio.Checked)
				{
					hyperLinkField.NavigateUrl = this._linkBox.Text;
				}
				else
				{
					hyperLinkField.DataNavigateUrlFormatString = this._dataNavFSBox.Text;
					if (this._haveSchema)
					{
						hyperLinkField.DataNavigateUrlFields = new string[] { this._dataNavFieldList.Text };
					}
					else
					{
						hyperLinkField.DataNavigateUrlFields = new string[] { this._dataNavFieldBox.Text };
					}
				}
				return hyperLinkField;
			}

			private global::System.Windows.Forms.TextBox _dataTextFieldBox;

			private global::System.Windows.Forms.TextBox _dataNavFieldBox;

			private global::System.Windows.Forms.TextBox _dataNavFSBox;

			private global::System.Windows.Forms.TextBox _textBox;

			private global::System.Windows.Forms.TextBox _textFSBox;

			private global::System.Windows.Forms.TextBox _linkBox;

			private ComboBox _dataTextFieldList;

			private ComboBox _dataNavFieldList;

			private global::System.Windows.Forms.RadioButton _staticTextRadio;

			private global::System.Windows.Forms.RadioButton _bindTextRadio;

			private global::System.Windows.Forms.RadioButton _staticUrlRadio;

			private global::System.Windows.Forms.RadioButton _bindUrlRadio;

			private global::System.Windows.Forms.Label _linkTextFormatStringLabel;

			private global::System.Windows.Forms.Label _linkUrlFormatStringLabel;

			private global::System.Windows.Forms.Label _linkTextFormatStringExampleLabel;

			private global::System.Windows.Forms.Label _linkUrlFormatStringExampleLabel;

			private GroupBox _textGroupBox;

			private GroupBox _linkGroupBox;

			private global::System.Windows.Forms.Panel _staticTextPanel;

			private global::System.Windows.Forms.Panel _bindTextPanel;

			private global::System.Windows.Forms.Panel _staticUrlPanel;

			private global::System.Windows.Forms.Panel _bindUrlPanel;
		}

		private class CommandFieldControl : AddDataControlFieldDialog.DataControlFieldControl
		{
			public CommandFieldControl(IDataSourceFieldSchema[] fieldSchemas, Type controlType)
				: base(fieldSchemas, controlType)
			{
			}

			public override string FieldName
			{
				get
				{
					return "CommandField";
				}
			}

			protected override void InitializeComponent()
			{
				base.InitializeComponent();
				this._buttonTypeLabel = new global::System.Windows.Forms.Label();
				this._buttonTypeList = new ComboBox();
				this._commandButtonsLabel = new global::System.Windows.Forms.Label();
				this._deleteBox = new global::System.Windows.Forms.CheckBox();
				this._selectBox = new global::System.Windows.Forms.CheckBox();
				this._cancelBox = new global::System.Windows.Forms.CheckBox();
				this._updateBox = new global::System.Windows.Forms.CheckBox();
				this._insertBox = new global::System.Windows.Forms.CheckBox();
				this._buttonTypeLabel.Text = SR.GetString("DCFAdd_ButtonType");
				this._buttonTypeLabel.TextAlign = ContentAlignment.BottomLeft;
				this._buttonTypeLabel.SetBounds(0, 43, 270, 17);
				this._buttonTypeList.Items.Add(ButtonType.Link.ToString());
				this._buttonTypeList.Items.Add(ButtonType.Button.ToString());
				this._buttonTypeList.SelectedIndex = 0;
				this._buttonTypeList.DropDownStyle = ComboBoxStyle.DropDownList;
				this._buttonTypeList.TabIndex = 1;
				this._buttonTypeList.SetBounds(0, 62, 270, 20);
				this._commandButtonsLabel.Text = SR.GetString("DCFAdd_CommandButtons");
				this._commandButtonsLabel.TextAlign = ContentAlignment.BottomLeft;
				this._commandButtonsLabel.SetBounds(0, 86, 270, 17);
				this._deleteBox.Text = SR.GetString("DCFAdd_Delete");
				this._deleteBox.AccessibleDescription = SR.GetString("DCFAdd_DeleteDesc");
				this._deleteBox.TextAlign = ContentAlignment.TopLeft;
				this._deleteBox.CheckAlign = ContentAlignment.TopLeft;
				this._deleteBox.TabIndex = 2;
				this._deleteBox.SetBounds(8, 105, 125, 20);
				this._selectBox.Text = SR.GetString("DCFAdd_Select");
				this._selectBox.AccessibleDescription = SR.GetString("DCFAdd_SelectDesc");
				this._selectBox.TextAlign = ContentAlignment.TopLeft;
				this._selectBox.CheckAlign = ContentAlignment.TopLeft;
				this._selectBox.TabIndex = 4;
				this._selectBox.SetBounds(8, 125, 125, 20);
				this._cancelBox.Text = SR.GetString("DCFAdd_ShowCancel");
				this._cancelBox.AccessibleDescription = SR.GetString("DCFAdd_ShowCancelDesc");
				this._cancelBox.TextAlign = ContentAlignment.TopLeft;
				this._cancelBox.CheckAlign = ContentAlignment.TopLeft;
				this._cancelBox.Enabled = false;
				this._cancelBox.Checked = true;
				this._cancelBox.TabIndex = 6;
				this._cancelBox.SetBounds(8, 145, 270, 44);
				this._updateBox.Text = SR.GetString("DCFAdd_EditUpdate");
				this._updateBox.AccessibleDescription = SR.GetString("DCFAdd_EditUpdateDesc");
				this._updateBox.TextAlign = ContentAlignment.TopLeft;
				this._updateBox.CheckAlign = ContentAlignment.TopLeft;
				this._updateBox.TabIndex = 3;
				this._updateBox.CheckedChanged += this.OnCheckedChanged;
				this._updateBox.SetBounds(139, 105, 125, 20);
				this._insertBox.Text = SR.GetString("DCFAdd_NewInsert");
				this._insertBox.AccessibleDescription = SR.GetString("DCFAdd_NewInsertDesc");
				this._insertBox.TextAlign = ContentAlignment.TopLeft;
				this._insertBox.CheckAlign = ContentAlignment.TopLeft;
				this._insertBox.TabIndex = 5;
				this._insertBox.CheckedChanged += this.OnCheckedChanged;
				this._insertBox.SetBounds(8, 125, 125, 20);
				if (this._controlType == typeof(GridView))
				{
					this._insertBox.Visible = false;
				}
				else if (this._controlType == typeof(DetailsView))
				{
					this._selectBox.Visible = false;
				}
				base.Controls.AddRange(new Control[] { this._buttonTypeLabel, this._buttonTypeList, this._commandButtonsLabel, this._deleteBox, this._selectBox, this._cancelBox, this._updateBox, this._insertBox });
			}

			private void OnCheckedChanged(object sender, EventArgs e)
			{
				this._cancelBox.Enabled = this._updateBox.Checked || this._insertBox.Checked;
			}

			protected override void PreserveFields(IDictionary table)
			{
				table["ButtonType"] = this._buttonTypeList.SelectedIndex;
				table["ShowDeleteButton"] = this._deleteBox.Checked;
				table["ShowSelectButton"] = this._selectBox.Checked;
				table["ShowCancelButton"] = this._cancelBox.Checked;
				table["ShowEditButton"] = this._updateBox.Checked;
				table["ShowInsertButton"] = this._insertBox.Checked;
			}

			protected override void RestoreFieldsInternal(IDictionary table)
			{
				this._buttonTypeList.SelectedIndex = (int)table["ButtonType"];
				this._deleteBox.Checked = (bool)table["ShowDeleteButton"];
				this._selectBox.Checked = (bool)table["ShowSelectButton"];
				this._cancelBox.Checked = (bool)table["ShowCancelButton"];
				this._updateBox.Checked = (bool)table["ShowEditButton"];
				this._insertBox.Checked = (bool)table["ShowInsertButton"];
			}

			protected override DataControlField SaveValues(string headerText)
			{
				CommandField commandField = new CommandField();
				if (headerText != null && headerText.Length > 0)
				{
					commandField.HeaderText = headerText;
					commandField.ShowHeader = true;
				}
				if (this._buttonTypeList.SelectedIndex == 0)
				{
					commandField.ButtonType = ButtonType.Link;
				}
				else
				{
					commandField.ButtonType = ButtonType.Button;
				}
				commandField.ShowDeleteButton = this._deleteBox.Checked;
				commandField.ShowSelectButton = this._selectBox.Checked;
				if (this._cancelBox.Enabled)
				{
					commandField.ShowCancelButton = this._cancelBox.Checked;
				}
				commandField.ShowEditButton = this._updateBox.Checked;
				commandField.ShowInsertButton = this._insertBox.Checked;
				return commandField;
			}

			private const int checkBoxLeft = 8;

			private global::System.Windows.Forms.Label _buttonTypeLabel;

			private global::System.Windows.Forms.Label _commandButtonsLabel;

			private ComboBox _buttonTypeList;

			private global::System.Windows.Forms.CheckBox _deleteBox;

			private global::System.Windows.Forms.CheckBox _selectBox;

			private global::System.Windows.Forms.CheckBox _cancelBox;

			private global::System.Windows.Forms.CheckBox _updateBox;

			private global::System.Windows.Forms.CheckBox _insertBox;
		}

		private class TemplateFieldControl : AddDataControlFieldDialog.DataControlFieldControl
		{
			public TemplateFieldControl(IDataSourceFieldSchema[] fieldSchemas, Type controlType)
				: base(fieldSchemas, controlType)
			{
			}

			public override string FieldName
			{
				get
				{
					return "TemplateField";
				}
			}

			protected override void InitializeComponent()
			{
				base.InitializeComponent();
			}

			protected override void PreserveFields(IDictionary table)
			{
			}

			protected override void RestoreFieldsInternal(IDictionary table)
			{
			}

			protected override DataControlField SaveValues(string headerText)
			{
				return new TemplateField
				{
					HeaderText = headerText
				};
			}
		}

		private class ImageFieldControl : AddDataControlFieldDialog.DataControlFieldControl
		{
			public ImageFieldControl(IDataSourceFieldSchema[] fieldSchemas, Type controlType)
				: base(fieldSchemas, controlType)
			{
			}

			public override string FieldName
			{
				get
				{
					return "ImageField";
				}
			}

			protected override void InitializeComponent()
			{
				base.InitializeComponent();
				this._imageUrlFieldList = new ComboBox();
				this._imageUrlFieldBox = new global::System.Windows.Forms.TextBox();
				this._imageUrlFieldLabel = new global::System.Windows.Forms.Label();
				this._readOnlyCheckBox = new global::System.Windows.Forms.CheckBox();
				this._urlFormatBox = new global::System.Windows.Forms.TextBox();
				this._urlFormatBoxLabel = new global::System.Windows.Forms.Label();
				this._urlFormatExampleLabel = new global::System.Windows.Forms.Label();
				this._imageUrlFieldLabel.Text = SR.GetString("DCFAdd_DataField");
				this._imageUrlFieldLabel.TextAlign = ContentAlignment.BottomLeft;
				this._imageUrlFieldLabel.SetBounds(0, 43, 270, 17);
				this._imageUrlFieldList.DropDownStyle = ComboBoxStyle.DropDownList;
				this._imageUrlFieldList.TabIndex = 1;
				this._imageUrlFieldList.SetBounds(0, 62, 270, 20);
				this._imageUrlFieldBox.TabIndex = 2;
				this._imageUrlFieldBox.SetBounds(0, 62, 270, 20);
				this._urlFormatBoxLabel.TabIndex = 3;
				this._urlFormatBoxLabel.Text = SR.GetString("DCFAdd_LinkFormatString");
				this._urlFormatBoxLabel.TextAlign = ContentAlignment.BottomLeft;
				this._urlFormatBoxLabel.SetBounds(0, 86, 270, 17);
				this._urlFormatBox.TabIndex = 4;
				this._urlFormatBox.SetBounds(0, 105, 270, 20);
				this._urlFormatExampleLabel.Enabled = false;
				this._urlFormatExampleLabel.Text = SR.GetString("DCFAdd_ExampleFormatString");
				this._urlFormatExampleLabel.TextAlign = ContentAlignment.BottomLeft;
				this._urlFormatExampleLabel.SetBounds(0, 125, 270, 17);
				this._readOnlyCheckBox.TabIndex = 5;
				this._readOnlyCheckBox.Text = SR.GetString("DCFAdd_ReadOnly");
				this._readOnlyCheckBox.SetBounds(0, 144, 270, 20);
				if (this._haveSchema)
				{
					this._imageUrlFieldList.Items.AddRange(base.GetFieldSchemaNames());
					this._imageUrlFieldList.SelectedIndex = 0;
					this._imageUrlFieldList.Visible = true;
					this._imageUrlFieldBox.Visible = false;
				}
				else
				{
					this._imageUrlFieldList.Visible = false;
					this._imageUrlFieldBox.Visible = true;
				}
				base.Controls.AddRange(new Control[] { this._imageUrlFieldLabel, this._imageUrlFieldBox, this._imageUrlFieldList, this._readOnlyCheckBox, this._urlFormatBoxLabel, this._urlFormatBox, this._urlFormatExampleLabel });
			}

			protected override void PreserveFields(IDictionary table)
			{
				if (this._haveSchema)
				{
					table["ImageUrlField"] = this._imageUrlFieldList.Text;
				}
				else
				{
					table["ImageUrlField"] = this._imageUrlFieldBox.Text;
				}
				table["ReadOnly"] = this._readOnlyCheckBox.Checked;
				table["FormatString"] = this._urlFormatBox.Text;
			}

			protected override void RefreshSchemaFields()
			{
				if (this._haveSchema)
				{
					this._imageUrlFieldList.Items.Clear();
					this._imageUrlFieldList.Items.AddRange(base.GetFieldSchemaNames());
					this._imageUrlFieldList.SelectedIndex = 0;
					this._imageUrlFieldList.Visible = true;
					this._imageUrlFieldBox.Visible = false;
					return;
				}
				this._imageUrlFieldList.Visible = false;
				this._imageUrlFieldBox.Visible = true;
			}

			protected override void RestoreFieldsInternal(IDictionary table)
			{
				string text = table["ImageUrlField"].ToString();
				if (this._haveSchema)
				{
					if (text.Length > 0)
					{
						bool flag = false;
						foreach (object obj in this._imageUrlFieldList.Items)
						{
							if (string.Compare(text, obj.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
							{
								this._imageUrlFieldList.SelectedItem = obj;
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							this._imageUrlFieldList.Items.Insert(0, text);
							this._imageUrlFieldList.SelectedIndex = 0;
						}
					}
				}
				else
				{
					this._imageUrlFieldBox.Text = text;
				}
				this._readOnlyCheckBox.Checked = (bool)table["ReadOnly"];
				this._urlFormatBox.Text = (string)table["FormatString"];
			}

			protected override DataControlField SaveValues(string headerText)
			{
				ImageField imageField = new ImageField();
				imageField.HeaderText = headerText;
				if (this._haveSchema)
				{
					imageField.DataImageUrlField = this._imageUrlFieldList.Text;
				}
				else
				{
					imageField.DataImageUrlField = this._imageUrlFieldBox.Text;
				}
				imageField.ReadOnly = this._readOnlyCheckBox.Checked;
				imageField.DataImageUrlFormatString = this._urlFormatBox.Text;
				return imageField;
			}

			private global::System.Windows.Forms.Label _imageUrlFieldLabel;

			private ComboBox _imageUrlFieldList;

			private global::System.Windows.Forms.TextBox _imageUrlFieldBox;

			private global::System.Windows.Forms.CheckBox _readOnlyCheckBox;

			private global::System.Windows.Forms.TextBox _urlFormatBox;

			private global::System.Windows.Forms.Label _urlFormatBoxLabel;

			private global::System.Windows.Forms.Label _urlFormatExampleLabel;
		}

		private class DataControlFieldDesignerControl : AddDataControlFieldDialog.DataControlFieldControl
		{
			public DataControlFieldDesignerControl(DataBoundControlDesigner controlDesigner, IServiceProvider serviceProvider, DataControlFieldDesigner designer, IDataSourceFieldSchema[] fieldSchemas, Type controlType)
				: base(fieldSchemas, controlType)
			{
				this._controlDesigner = controlDesigner;
				this._serviceProvider = serviceProvider;
				this._designer = designer;
				this.Initialize();
			}

			public override string FieldName
			{
				get
				{
					return this._designer.DefaultNodeText;
				}
			}

			public DataControlFieldDesigner Designer
			{
				get
				{
					return this._designer;
				}
			}

			protected override void InitializeComponent()
			{
			}

			private void Initialize()
			{
				this._field = this._designer.CreateField();
				this._fieldProps = new VsPropertyGrid(this._serviceProvider);
				this._fieldProps.SelectedObject = this._field;
				this._fieldProps.CommandsVisibleIfAvailable = true;
				this._fieldProps.LargeButtons = false;
				this._fieldProps.LineColor = SystemColors.ScrollBar;
				this._fieldProps.Name = "_fieldProps";
				this._fieldProps.Size = new Size(248, 281);
				this._fieldProps.ToolbarVisible = true;
				this._fieldProps.ViewBackColor = SystemColors.Window;
				this._fieldProps.ViewForeColor = SystemColors.WindowText;
				this._fieldProps.Site = this._controlDesigner.Component.Site;
				base.Controls.Add(this._fieldProps);
			}

			protected override void PreserveFields(IDictionary table)
			{
			}

			protected override void RefreshSchemaFields()
			{
			}

			protected override void RestoreFieldsInternal(IDictionary table)
			{
			}

			protected override DataControlField SaveValues(string headerText)
			{
				this._fieldProps.Refresh();
				return this._field;
			}

			private DataBoundControlDesigner _controlDesigner;

			private DataControlFieldDesigner _designer;

			private DataControlField _field;

			private PropertyGrid _fieldProps;

			private IServiceProvider _serviceProvider;
		}
	}
}
