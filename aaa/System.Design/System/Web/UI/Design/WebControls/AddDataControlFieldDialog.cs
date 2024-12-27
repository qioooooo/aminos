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
	// Token: 0x020003DB RID: 987
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed partial class AddDataControlFieldDialog : DesignerForm
	{
		// Token: 0x06002476 RID: 9334 RVA: 0x000C2F2C File Offset: 0x000C1F2C
		public AddDataControlFieldDialog(DataBoundControlDesigner controlDesigner)
			: base(controlDesigner.Component.Site)
		{
			this._controlDesigner = controlDesigner;
			this.IgnoreRefreshSchemaEvents();
			this.InitForm();
		}

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06002477 RID: 9335 RVA: 0x000C2F5A File Offset: 0x000C1F5A
		private DataBoundControl Control
		{
			get
			{
				return this._controlDesigner.Component as DataBoundControl;
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06002478 RID: 9336 RVA: 0x000C2F6C File Offset: 0x000C1F6C
		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.DataControlField.AddDataControlFieldDialog";
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06002479 RID: 9337 RVA: 0x000C2F73 File Offset: 0x000C1F73
		// (set) Token: 0x0600247A RID: 9338 RVA: 0x000C2FB2 File Offset: 0x000C1FB2
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

		// Token: 0x0600247B RID: 9339 RVA: 0x000C2FF0 File Offset: 0x000C1FF0
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

		// Token: 0x0600247C RID: 9340 RVA: 0x000C32F0 File Offset: 0x000C22F0
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

		// Token: 0x0600247D RID: 9341 RVA: 0x000C335C File Offset: 0x000C235C
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

		// Token: 0x0600247E RID: 9342 RVA: 0x000C33F8 File Offset: 0x000C23F8
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

		// Token: 0x0600247F RID: 9343 RVA: 0x000C35C0 File Offset: 0x000C25C0
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

		// Token: 0x06002480 RID: 9344 RVA: 0x000C366C File Offset: 0x000C266C
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

		// Token: 0x06002481 RID: 9345 RVA: 0x000C36A4 File Offset: 0x000C26A4
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

		// Token: 0x06002482 RID: 9346 RVA: 0x000C3780 File Offset: 0x000C2780
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

		// Token: 0x06002483 RID: 9347 RVA: 0x000C37E4 File Offset: 0x000C27E4
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

		// Token: 0x06002484 RID: 9348 RVA: 0x000C38C4 File Offset: 0x000C28C4
		protected override void OnClosed(EventArgs e)
		{
			IDataSourceDesigner dataSourceDesigner = this._controlDesigner.DataSourceDesigner;
			if (dataSourceDesigner != null)
			{
				dataSourceDesigner.ResumeDataSourceEvents();
			}
			this.IgnoreRefreshSchema = this._initialIgnoreRefreshSchemaValue;
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x000C38F2 File Offset: 0x000C28F2
		private void OnSelectedFieldTypeChanged(object sender, EventArgs e)
		{
			this.SetSelectedFieldControlVisible();
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x000C38FC File Offset: 0x000C28FC
		private void SetSelectedFieldControlVisible()
		{
			foreach (AddDataControlFieldDialog.DataControlFieldControl dataControlFieldControl in this.GetDataControlFieldControls())
			{
				dataControlFieldControl.Visible = false;
			}
			this.GetDataControlFieldControls()[this._fieldList.SelectedIndex].Visible = true;
			this.Refresh();
		}

		// Token: 0x040018FB RID: 6395
		private const int buttonWidth = 75;

		// Token: 0x040018FC RID: 6396
		private const int buttonHeight = 23;

		// Token: 0x040018FD RID: 6397
		private const int formHeight = 510;

		// Token: 0x040018FE RID: 6398
		private const int formWidth = 330;

		// Token: 0x040018FF RID: 6399
		private const int labelLeft = 12;

		// Token: 0x04001900 RID: 6400
		private const int labelHeight = 17;

		// Token: 0x04001901 RID: 6401
		private const int labelPadding = 2;

		// Token: 0x04001902 RID: 6402
		private const int labelWidth = 270;

		// Token: 0x04001903 RID: 6403
		private const int controlLeft = 12;

		// Token: 0x04001904 RID: 6404
		private const int controlHeight = 20;

		// Token: 0x04001905 RID: 6405
		private const int fieldChooserWidth = 150;

		// Token: 0x04001906 RID: 6406
		private const int textBoxWidth = 270;

		// Token: 0x04001907 RID: 6407
		private const int vertPadding = 4;

		// Token: 0x04001908 RID: 6408
		private const int horizPadding = 6;

		// Token: 0x04001909 RID: 6409
		private const int topPadding = 12;

		// Token: 0x0400190A RID: 6410
		private const int bottomPadding = 12;

		// Token: 0x0400190B RID: 6411
		private const int rightPadding = 12;

		// Token: 0x0400190C RID: 6412
		private const int linkWidth = 100;

		// Token: 0x0400190D RID: 6413
		private const int checkBoxWidth = 125;

		// Token: 0x0400190E RID: 6414
		private bool _dynamicDataEnabled;

		// Token: 0x0400190F RID: 6415
		private DataBoundControlDesigner _controlDesigner;

		// Token: 0x04001910 RID: 6416
		private AddDataControlFieldDialog.DataControlFieldControl[] _dataControlFieldControls;

		// Token: 0x04001911 RID: 6417
		private IDataSourceFieldSchema[] _fieldSchemas;

		// Token: 0x04001912 RID: 6418
		private bool _initialIgnoreRefreshSchemaValue;

		// Token: 0x04001913 RID: 6419
		private global::System.Windows.Forms.Button _okButton;

		// Token: 0x04001914 RID: 6420
		private global::System.Windows.Forms.Button _cancelButton;

		// Token: 0x04001915 RID: 6421
		private global::System.Windows.Forms.Label _fieldLabel;

		// Token: 0x04001916 RID: 6422
		private ComboBox _fieldList;

		// Token: 0x04001917 RID: 6423
		private LinkLabel _refreshSchemaLink;

		// Token: 0x04001918 RID: 6424
		private global::System.Windows.Forms.Panel _controlsPanel;

		// Token: 0x04001919 RID: 6425
		private int fieldControlTop = 51;

		// Token: 0x0400191A RID: 6426
		private IDictionary<Type, DataControlFieldDesigner> _customFieldDesigners;

		// Token: 0x020003DC RID: 988
		private abstract class DataControlFieldControl : Control
		{
			// Token: 0x06002487 RID: 9351 RVA: 0x000C3947 File Offset: 0x000C2947
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

			// Token: 0x170006D5 RID: 1749
			// (get) Token: 0x06002488 RID: 9352
			public abstract string FieldName { get; }

			// Token: 0x06002489 RID: 9353 RVA: 0x000C3974 File Offset: 0x000C2974
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

			// Token: 0x0600248A RID: 9354 RVA: 0x000C39D0 File Offset: 0x000C29D0
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

			// Token: 0x0600248B RID: 9355 RVA: 0x000C3A74 File Offset: 0x000C2A74
			public IDictionary PreserveFields()
			{
				Hashtable hashtable = new Hashtable();
				hashtable["HeaderText"] = this._headerTextBox.Text;
				this.PreserveFields(hashtable);
				return hashtable;
			}

			// Token: 0x0600248C RID: 9356
			protected abstract void PreserveFields(IDictionary table);

			// Token: 0x0600248D RID: 9357 RVA: 0x000C3AA5 File Offset: 0x000C2AA5
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

			// Token: 0x0600248E RID: 9358 RVA: 0x000C3ACB File Offset: 0x000C2ACB
			protected virtual void RefreshSchemaFields()
			{
			}

			// Token: 0x0600248F RID: 9359 RVA: 0x000C3ACD File Offset: 0x000C2ACD
			public void RestoreFields(IDictionary table)
			{
				this._headerTextBox.Text = table["HeaderText"].ToString();
				this.RestoreFieldsInternal(table);
			}

			// Token: 0x06002490 RID: 9360
			protected abstract void RestoreFieldsInternal(IDictionary table);

			// Token: 0x06002491 RID: 9361
			protected abstract DataControlField SaveValues(string headerText);

			// Token: 0x06002492 RID: 9362 RVA: 0x000C3AF4 File Offset: 0x000C2AF4
			public DataControlField SaveValues()
			{
				string text = ((this._headerTextBox == null) ? string.Empty : this._headerTextBox.Text);
				return this.SaveValues(text);
			}

			// Token: 0x06002493 RID: 9363 RVA: 0x000C3B23 File Offset: 0x000C2B23
			protected string StripAccelerators(string text)
			{
				return text.Replace("&", string.Empty);
			}

			// Token: 0x0400191B RID: 6427
			protected string[] _fieldSchemaNames;

			// Token: 0x0400191C RID: 6428
			protected Type _controlType;

			// Token: 0x0400191D RID: 6429
			protected bool _haveSchema;

			// Token: 0x0400191E RID: 6430
			protected IDataSourceFieldSchema[] _fieldSchemas;

			// Token: 0x0400191F RID: 6431
			private global::System.Windows.Forms.Label _headerTextLabel;

			// Token: 0x04001920 RID: 6432
			private global::System.Windows.Forms.TextBox _headerTextBox;
		}

		// Token: 0x020003DD RID: 989
		private class BoundFieldControl : AddDataControlFieldDialog.DataControlFieldControl
		{
			// Token: 0x06002494 RID: 9364 RVA: 0x000C3B35 File Offset: 0x000C2B35
			public BoundFieldControl(IDataSourceFieldSchema[] fieldSchemas, Type controlType)
				: base(fieldSchemas, controlType)
			{
			}

			// Token: 0x170006D6 RID: 1750
			// (get) Token: 0x06002495 RID: 9365 RVA: 0x000C3B3F File Offset: 0x000C2B3F
			public override string FieldName
			{
				get
				{
					return "BoundField";
				}
			}

			// Token: 0x06002496 RID: 9366 RVA: 0x000C3B48 File Offset: 0x000C2B48
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

			// Token: 0x06002497 RID: 9367 RVA: 0x000C3C9C File Offset: 0x000C2C9C
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

			// Token: 0x06002498 RID: 9368 RVA: 0x000C3CF4 File Offset: 0x000C2CF4
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

			// Token: 0x06002499 RID: 9369 RVA: 0x000C3D54 File Offset: 0x000C2D54
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

			// Token: 0x0600249A RID: 9370 RVA: 0x000C3DCC File Offset: 0x000C2DCC
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

			// Token: 0x0600249B RID: 9371 RVA: 0x000C3EB4 File Offset: 0x000C2EB4
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

			// Token: 0x04001921 RID: 6433
			private global::System.Windows.Forms.Label _dataFieldLabel;

			// Token: 0x04001922 RID: 6434
			protected ComboBox _dataFieldList;

			// Token: 0x04001923 RID: 6435
			protected global::System.Windows.Forms.TextBox _dataFieldBox;

			// Token: 0x04001924 RID: 6436
			protected global::System.Windows.Forms.CheckBox _readOnlyCheckBox;
		}

		// Token: 0x020003DE RID: 990
		private class DynamicDataFieldControl : AddDataControlFieldDialog.BoundFieldControl
		{
			// Token: 0x0600249C RID: 9372 RVA: 0x000C3F18 File Offset: 0x000C2F18
			public DynamicDataFieldControl(DataControlFieldDesigner dynamicFieldDesigner, IDataSourceFieldSchema[] fieldSchemas, Type controlType)
				: base(fieldSchemas, controlType)
			{
				this._designer = dynamicFieldDesigner;
			}

			// Token: 0x170006D7 RID: 1751
			// (get) Token: 0x0600249D RID: 9373 RVA: 0x000C3F29 File Offset: 0x000C2F29
			public override string FieldName
			{
				get
				{
					return "DynamicField";
				}
			}

			// Token: 0x0600249E RID: 9374 RVA: 0x000C3F30 File Offset: 0x000C2F30
			protected override void InitializeComponent()
			{
				base.InitializeComponent();
				this._readOnlyCheckBox.Visible = false;
			}

			// Token: 0x0600249F RID: 9375 RVA: 0x000C3F44 File Offset: 0x000C2F44
			protected override DataControlField SaveValues(string headerText)
			{
				DataControlField dataControlField = this._designer.CreateField();
				dataControlField.HeaderText = headerText;
				string text = (this._haveSchema ? this._dataFieldList.Text : this._dataFieldBox.Text);
				AddDataControlFieldDialog.DynamicDataFieldControl.SetProperty(dataControlField, "DataField", text);
				return dataControlField;
			}

			// Token: 0x060024A0 RID: 9376 RVA: 0x000C3F92 File Offset: 0x000C2F92
			private static void SetProperty(DataControlField target, string propertyName, object value)
			{
				target.GetType().GetProperty(propertyName).SetValue(target, value, null);
			}

			// Token: 0x04001925 RID: 6437
			private DataControlFieldDesigner _designer;
		}

		// Token: 0x020003DF RID: 991
		private class CheckBoxFieldControl : AddDataControlFieldDialog.DataControlFieldControl
		{
			// Token: 0x060024A1 RID: 9377 RVA: 0x000C3FA8 File Offset: 0x000C2FA8
			public CheckBoxFieldControl(IDataSourceFieldSchema[] fieldSchemas, Type controlType)
				: base(fieldSchemas, controlType)
			{
			}

			// Token: 0x170006D8 RID: 1752
			// (get) Token: 0x060024A2 RID: 9378 RVA: 0x000C3FB2 File Offset: 0x000C2FB2
			public override string FieldName
			{
				get
				{
					return "CheckBoxField";
				}
			}

			// Token: 0x060024A3 RID: 9379 RVA: 0x000C3FBC File Offset: 0x000C2FBC
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

			// Token: 0x060024A4 RID: 9380 RVA: 0x000C40F8 File Offset: 0x000C30F8
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

			// Token: 0x060024A5 RID: 9381 RVA: 0x000C4158 File Offset: 0x000C3158
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

			// Token: 0x060024A6 RID: 9382 RVA: 0x000C41D0 File Offset: 0x000C31D0
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

			// Token: 0x060024A7 RID: 9383 RVA: 0x000C42B8 File Offset: 0x000C32B8
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

			// Token: 0x04001926 RID: 6438
			private global::System.Windows.Forms.Label _dataFieldLabel;

			// Token: 0x04001927 RID: 6439
			private ComboBox _dataFieldList;

			// Token: 0x04001928 RID: 6440
			private global::System.Windows.Forms.TextBox _dataFieldBox;

			// Token: 0x04001929 RID: 6441
			private global::System.Windows.Forms.CheckBox _readOnlyCheckBox;
		}

		// Token: 0x020003E0 RID: 992
		private class ButtonFieldControl : AddDataControlFieldDialog.DataControlFieldControl
		{
			// Token: 0x060024A8 RID: 9384 RVA: 0x000C431C File Offset: 0x000C331C
			public ButtonFieldControl(IDataSourceFieldSchema[] fieldSchemas, Type controlType)
				: base(fieldSchemas, controlType)
			{
			}

			// Token: 0x170006D9 RID: 1753
			// (get) Token: 0x060024A9 RID: 9385 RVA: 0x000C4326 File Offset: 0x000C3326
			public override string FieldName
			{
				get
				{
					return "ButtonField";
				}
			}

			// Token: 0x060024AA RID: 9386 RVA: 0x000C4330 File Offset: 0x000C3330
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

			// Token: 0x060024AB RID: 9387 RVA: 0x000C4624 File Offset: 0x000C3624
			protected override void PreserveFields(IDictionary table)
			{
				table["ButtonType"] = this._buttonTypeList.SelectedIndex;
				table["CommandName"] = this._commandNameList.SelectedIndex;
				table["Text"] = this._textBox.Text;
			}

			// Token: 0x060024AC RID: 9388 RVA: 0x000C4680 File Offset: 0x000C3680
			protected override void RestoreFieldsInternal(IDictionary table)
			{
				this._buttonTypeList.SelectedIndex = (int)table["ButtonType"];
				this._commandNameList.SelectedIndex = (int)table["CommandName"];
				this._textBox.Text = table["Text"].ToString();
			}

			// Token: 0x060024AD RID: 9389 RVA: 0x000C46E0 File Offset: 0x000C36E0
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

			// Token: 0x0400192A RID: 6442
			private global::System.Windows.Forms.Label _buttonTypeLabel;

			// Token: 0x0400192B RID: 6443
			private global::System.Windows.Forms.Label _commandNameLabel;

			// Token: 0x0400192C RID: 6444
			private global::System.Windows.Forms.Label _textLabel;

			// Token: 0x0400192D RID: 6445
			private ComboBox _buttonTypeList;

			// Token: 0x0400192E RID: 6446
			private ComboBox _commandNameList;

			// Token: 0x0400192F RID: 6447
			private global::System.Windows.Forms.TextBox _textBox;
		}

		// Token: 0x020003E1 RID: 993
		private class HyperLinkFieldControl : AddDataControlFieldDialog.DataControlFieldControl
		{
			// Token: 0x060024AE RID: 9390 RVA: 0x000C474D File Offset: 0x000C374D
			public HyperLinkFieldControl(IDataSourceFieldSchema[] fieldSchemas, Type controlType)
				: base(fieldSchemas, controlType)
			{
			}

			// Token: 0x170006DA RID: 1754
			// (get) Token: 0x060024AF RID: 9391 RVA: 0x000C4757 File Offset: 0x000C3757
			public override string FieldName
			{
				get
				{
					return "HyperLinkField";
				}
			}

			// Token: 0x060024B0 RID: 9392 RVA: 0x000C4760 File Offset: 0x000C3760
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

			// Token: 0x060024B1 RID: 9393 RVA: 0x000C4EA8 File Offset: 0x000C3EA8
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

			// Token: 0x060024B2 RID: 9394 RVA: 0x000C4F3C File Offset: 0x000C3F3C
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

			// Token: 0x060024B3 RID: 9395 RVA: 0x000C4FD0 File Offset: 0x000C3FD0
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

			// Token: 0x060024B4 RID: 9396 RVA: 0x000C50D0 File Offset: 0x000C40D0
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

			// Token: 0x060024B5 RID: 9397 RVA: 0x000C51DC File Offset: 0x000C41DC
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

			// Token: 0x060024B6 RID: 9398 RVA: 0x000C5404 File Offset: 0x000C4404
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

			// Token: 0x04001930 RID: 6448
			private global::System.Windows.Forms.TextBox _dataTextFieldBox;

			// Token: 0x04001931 RID: 6449
			private global::System.Windows.Forms.TextBox _dataNavFieldBox;

			// Token: 0x04001932 RID: 6450
			private global::System.Windows.Forms.TextBox _dataNavFSBox;

			// Token: 0x04001933 RID: 6451
			private global::System.Windows.Forms.TextBox _textBox;

			// Token: 0x04001934 RID: 6452
			private global::System.Windows.Forms.TextBox _textFSBox;

			// Token: 0x04001935 RID: 6453
			private global::System.Windows.Forms.TextBox _linkBox;

			// Token: 0x04001936 RID: 6454
			private ComboBox _dataTextFieldList;

			// Token: 0x04001937 RID: 6455
			private ComboBox _dataNavFieldList;

			// Token: 0x04001938 RID: 6456
			private global::System.Windows.Forms.RadioButton _staticTextRadio;

			// Token: 0x04001939 RID: 6457
			private global::System.Windows.Forms.RadioButton _bindTextRadio;

			// Token: 0x0400193A RID: 6458
			private global::System.Windows.Forms.RadioButton _staticUrlRadio;

			// Token: 0x0400193B RID: 6459
			private global::System.Windows.Forms.RadioButton _bindUrlRadio;

			// Token: 0x0400193C RID: 6460
			private global::System.Windows.Forms.Label _linkTextFormatStringLabel;

			// Token: 0x0400193D RID: 6461
			private global::System.Windows.Forms.Label _linkUrlFormatStringLabel;

			// Token: 0x0400193E RID: 6462
			private global::System.Windows.Forms.Label _linkTextFormatStringExampleLabel;

			// Token: 0x0400193F RID: 6463
			private global::System.Windows.Forms.Label _linkUrlFormatStringExampleLabel;

			// Token: 0x04001940 RID: 6464
			private GroupBox _textGroupBox;

			// Token: 0x04001941 RID: 6465
			private GroupBox _linkGroupBox;

			// Token: 0x04001942 RID: 6466
			private global::System.Windows.Forms.Panel _staticTextPanel;

			// Token: 0x04001943 RID: 6467
			private global::System.Windows.Forms.Panel _bindTextPanel;

			// Token: 0x04001944 RID: 6468
			private global::System.Windows.Forms.Panel _staticUrlPanel;

			// Token: 0x04001945 RID: 6469
			private global::System.Windows.Forms.Panel _bindUrlPanel;
		}

		// Token: 0x020003E2 RID: 994
		private class CommandFieldControl : AddDataControlFieldDialog.DataControlFieldControl
		{
			// Token: 0x060024B7 RID: 9399 RVA: 0x000C54EF File Offset: 0x000C44EF
			public CommandFieldControl(IDataSourceFieldSchema[] fieldSchemas, Type controlType)
				: base(fieldSchemas, controlType)
			{
			}

			// Token: 0x170006DB RID: 1755
			// (get) Token: 0x060024B8 RID: 9400 RVA: 0x000C54F9 File Offset: 0x000C44F9
			public override string FieldName
			{
				get
				{
					return "CommandField";
				}
			}

			// Token: 0x060024B9 RID: 9401 RVA: 0x000C5500 File Offset: 0x000C4500
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

			// Token: 0x060024BA RID: 9402 RVA: 0x000C5919 File Offset: 0x000C4919
			private void OnCheckedChanged(object sender, EventArgs e)
			{
				this._cancelBox.Enabled = this._updateBox.Checked || this._insertBox.Checked;
			}

			// Token: 0x060024BB RID: 9403 RVA: 0x000C5944 File Offset: 0x000C4944
			protected override void PreserveFields(IDictionary table)
			{
				table["ButtonType"] = this._buttonTypeList.SelectedIndex;
				table["ShowDeleteButton"] = this._deleteBox.Checked;
				table["ShowSelectButton"] = this._selectBox.Checked;
				table["ShowCancelButton"] = this._cancelBox.Checked;
				table["ShowEditButton"] = this._updateBox.Checked;
				table["ShowInsertButton"] = this._insertBox.Checked;
			}

			// Token: 0x060024BC RID: 9404 RVA: 0x000C59F4 File Offset: 0x000C49F4
			protected override void RestoreFieldsInternal(IDictionary table)
			{
				this._buttonTypeList.SelectedIndex = (int)table["ButtonType"];
				this._deleteBox.Checked = (bool)table["ShowDeleteButton"];
				this._selectBox.Checked = (bool)table["ShowSelectButton"];
				this._cancelBox.Checked = (bool)table["ShowCancelButton"];
				this._updateBox.Checked = (bool)table["ShowEditButton"];
				this._insertBox.Checked = (bool)table["ShowInsertButton"];
			}

			// Token: 0x060024BD RID: 9405 RVA: 0x000C5AA4 File Offset: 0x000C4AA4
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

			// Token: 0x04001946 RID: 6470
			private const int checkBoxLeft = 8;

			// Token: 0x04001947 RID: 6471
			private global::System.Windows.Forms.Label _buttonTypeLabel;

			// Token: 0x04001948 RID: 6472
			private global::System.Windows.Forms.Label _commandButtonsLabel;

			// Token: 0x04001949 RID: 6473
			private ComboBox _buttonTypeList;

			// Token: 0x0400194A RID: 6474
			private global::System.Windows.Forms.CheckBox _deleteBox;

			// Token: 0x0400194B RID: 6475
			private global::System.Windows.Forms.CheckBox _selectBox;

			// Token: 0x0400194C RID: 6476
			private global::System.Windows.Forms.CheckBox _cancelBox;

			// Token: 0x0400194D RID: 6477
			private global::System.Windows.Forms.CheckBox _updateBox;

			// Token: 0x0400194E RID: 6478
			private global::System.Windows.Forms.CheckBox _insertBox;
		}

		// Token: 0x020003E3 RID: 995
		private class TemplateFieldControl : AddDataControlFieldDialog.DataControlFieldControl
		{
			// Token: 0x060024BE RID: 9406 RVA: 0x000C5B51 File Offset: 0x000C4B51
			public TemplateFieldControl(IDataSourceFieldSchema[] fieldSchemas, Type controlType)
				: base(fieldSchemas, controlType)
			{
			}

			// Token: 0x170006DC RID: 1756
			// (get) Token: 0x060024BF RID: 9407 RVA: 0x000C5B5B File Offset: 0x000C4B5B
			public override string FieldName
			{
				get
				{
					return "TemplateField";
				}
			}

			// Token: 0x060024C0 RID: 9408 RVA: 0x000C5B62 File Offset: 0x000C4B62
			protected override void InitializeComponent()
			{
				base.InitializeComponent();
			}

			// Token: 0x060024C1 RID: 9409 RVA: 0x000C5B6A File Offset: 0x000C4B6A
			protected override void PreserveFields(IDictionary table)
			{
			}

			// Token: 0x060024C2 RID: 9410 RVA: 0x000C5B6C File Offset: 0x000C4B6C
			protected override void RestoreFieldsInternal(IDictionary table)
			{
			}

			// Token: 0x060024C3 RID: 9411 RVA: 0x000C5B70 File Offset: 0x000C4B70
			protected override DataControlField SaveValues(string headerText)
			{
				return new TemplateField
				{
					HeaderText = headerText
				};
			}
		}

		// Token: 0x020003E4 RID: 996
		private class ImageFieldControl : AddDataControlFieldDialog.DataControlFieldControl
		{
			// Token: 0x060024C4 RID: 9412 RVA: 0x000C5B8B File Offset: 0x000C4B8B
			public ImageFieldControl(IDataSourceFieldSchema[] fieldSchemas, Type controlType)
				: base(fieldSchemas, controlType)
			{
			}

			// Token: 0x170006DD RID: 1757
			// (get) Token: 0x060024C5 RID: 9413 RVA: 0x000C5B95 File Offset: 0x000C4B95
			public override string FieldName
			{
				get
				{
					return "ImageField";
				}
			}

			// Token: 0x060024C6 RID: 9414 RVA: 0x000C5B9C File Offset: 0x000C4B9C
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

			// Token: 0x060024C7 RID: 9415 RVA: 0x000C5E18 File Offset: 0x000C4E18
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

			// Token: 0x060024C8 RID: 9416 RVA: 0x000C5E8C File Offset: 0x000C4E8C
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

			// Token: 0x060024C9 RID: 9417 RVA: 0x000C5F04 File Offset: 0x000C4F04
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

			// Token: 0x060024CA RID: 9418 RVA: 0x000C6008 File Offset: 0x000C5008
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

			// Token: 0x0400194F RID: 6479
			private global::System.Windows.Forms.Label _imageUrlFieldLabel;

			// Token: 0x04001950 RID: 6480
			private ComboBox _imageUrlFieldList;

			// Token: 0x04001951 RID: 6481
			private global::System.Windows.Forms.TextBox _imageUrlFieldBox;

			// Token: 0x04001952 RID: 6482
			private global::System.Windows.Forms.CheckBox _readOnlyCheckBox;

			// Token: 0x04001953 RID: 6483
			private global::System.Windows.Forms.TextBox _urlFormatBox;

			// Token: 0x04001954 RID: 6484
			private global::System.Windows.Forms.Label _urlFormatBoxLabel;

			// Token: 0x04001955 RID: 6485
			private global::System.Windows.Forms.Label _urlFormatExampleLabel;
		}

		// Token: 0x020003E5 RID: 997
		private class DataControlFieldDesignerControl : AddDataControlFieldDialog.DataControlFieldControl
		{
			// Token: 0x060024CB RID: 9419 RVA: 0x000C6071 File Offset: 0x000C5071
			public DataControlFieldDesignerControl(DataBoundControlDesigner controlDesigner, IServiceProvider serviceProvider, DataControlFieldDesigner designer, IDataSourceFieldSchema[] fieldSchemas, Type controlType)
				: base(fieldSchemas, controlType)
			{
				this._controlDesigner = controlDesigner;
				this._serviceProvider = serviceProvider;
				this._designer = designer;
				this.Initialize();
			}

			// Token: 0x170006DE RID: 1758
			// (get) Token: 0x060024CC RID: 9420 RVA: 0x000C6098 File Offset: 0x000C5098
			public override string FieldName
			{
				get
				{
					return this._designer.DefaultNodeText;
				}
			}

			// Token: 0x170006DF RID: 1759
			// (get) Token: 0x060024CD RID: 9421 RVA: 0x000C60A5 File Offset: 0x000C50A5
			public DataControlFieldDesigner Designer
			{
				get
				{
					return this._designer;
				}
			}

			// Token: 0x060024CE RID: 9422 RVA: 0x000C60AD File Offset: 0x000C50AD
			protected override void InitializeComponent()
			{
			}

			// Token: 0x060024CF RID: 9423 RVA: 0x000C60B0 File Offset: 0x000C50B0
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

			// Token: 0x060024D0 RID: 9424 RVA: 0x000C619A File Offset: 0x000C519A
			protected override void PreserveFields(IDictionary table)
			{
			}

			// Token: 0x060024D1 RID: 9425 RVA: 0x000C619C File Offset: 0x000C519C
			protected override void RefreshSchemaFields()
			{
			}

			// Token: 0x060024D2 RID: 9426 RVA: 0x000C619E File Offset: 0x000C519E
			protected override void RestoreFieldsInternal(IDictionary table)
			{
			}

			// Token: 0x060024D3 RID: 9427 RVA: 0x000C61A0 File Offset: 0x000C51A0
			protected override DataControlField SaveValues(string headerText)
			{
				this._fieldProps.Refresh();
				return this._field;
			}

			// Token: 0x04001956 RID: 6486
			private DataBoundControlDesigner _controlDesigner;

			// Token: 0x04001957 RID: 6487
			private DataControlFieldDesigner _designer;

			// Token: 0x04001958 RID: 6488
			private DataControlField _field;

			// Token: 0x04001959 RID: 6489
			private PropertyGrid _fieldProps;

			// Token: 0x0400195A RID: 6490
			private IServiceProvider _serviceProvider;
		}
	}
}
