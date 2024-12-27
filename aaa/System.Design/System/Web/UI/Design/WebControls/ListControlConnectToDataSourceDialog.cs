using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Security.Permissions;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200045F RID: 1119
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed partial class ListControlConnectToDataSourceDialog : TaskForm
	{
		// Token: 0x060028BC RID: 10428 RVA: 0x000DF884 File Offset: 0x000DE884
		public ListControlConnectToDataSourceDialog(ListControlDesigner controlDesigner)
			: base(controlDesigner.Component.Site)
		{
			this._controlDesigner = controlDesigner;
			this._originalDataSourceID = controlDesigner.DataSourceID;
			this.SuppressChangedEvents(this._controlDesigner.DataSourceDesigner);
			base.Glyph = new Bitmap(base.GetType(), "datasourcewizard.bmp");
			this.CreatePanel();
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x060028BD RID: 10429 RVA: 0x000DF8E2 File Offset: 0x000DE8E2
		private global::System.Web.UI.WebControls.ListControl Control
		{
			get
			{
				return this._controlDesigner.Component as global::System.Web.UI.WebControls.ListControl;
			}
		}

		// Token: 0x17000789 RID: 1929
		// (set) Token: 0x060028BE RID: 10430 RVA: 0x000DF8F4 File Offset: 0x000DE8F4
		private string DataSourceID
		{
			set
			{
				this._controlDesigner.DataSourceID = value;
			}
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x060028BF RID: 10431 RVA: 0x000DF902 File Offset: 0x000DE902
		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.ListControl.ConnectToDataSource";
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x060028C0 RID: 10432 RVA: 0x000DF909 File Offset: 0x000DE909
		private IList<IDataSourceDesigner> SuppressedDataSources
		{
			get
			{
				if (this._suppressedDataSources == null)
				{
					this._suppressedDataSources = new List<IDataSourceDesigner>();
				}
				return this._suppressedDataSources;
			}
		}

		// Token: 0x060028C1 RID: 10433 RVA: 0x000DF924 File Offset: 0x000DE924
		private void FillDataSourceList()
		{
			this._dataSourceBox.Items.Clear();
			IComponent component = this.GetComponent();
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(component)["DataSourceID"];
			TypeConverter converter = propertyDescriptor.Converter;
			ITypeDescriptorContext typeDescriptorContext = new ListControlConnectToDataSourceDialog.TypeDescriptorContext(component);
			ICollection standardValues = converter.GetStandardValues(typeDescriptorContext);
			foreach (object obj in standardValues)
			{
				string text = (string)obj;
				this._dataSourceBox.Items.Add(text);
			}
			string dataSourceID = this.Control.DataSourceID;
			if (dataSourceID.Length <= 0)
			{
				this._dataSourceBox.SelectedIndex = this._dataSourceBox.Items.IndexOf(SR.GetString("DataSourceIDChromeConverter_NoDataSource"));
				return;
			}
			int num = this._dataSourceBox.Items.IndexOf(dataSourceID);
			if (num > -1)
			{
				this._dataSourceBox.SelectedIndex = num;
				return;
			}
			this._dataSourceBox.SelectedIndex = this._dataSourceBox.Items.Add(dataSourceID);
		}

		// Token: 0x060028C2 RID: 10434 RVA: 0x000DFA50 File Offset: 0x000DEA50
		private void FillFieldLists(bool preserveSelection)
		{
			object selectedItem = this._dataTextFieldBox.SelectedItem;
			object selectedItem2 = this._dataValueFieldBox.SelectedItem;
			this._dataTextFieldBox.Items.Clear();
			this._dataTextFieldBox.Text = string.Empty;
			this._dataValueFieldBox.Items.Clear();
			this._dataValueFieldBox.Text = string.Empty;
			IDataSourceFieldSchema[] fieldSchemas = this.GetFieldSchemas();
			if (fieldSchemas != null && fieldSchemas.Length > 0)
			{
				foreach (IDataSourceFieldSchema dataSourceFieldSchema in fieldSchemas)
				{
					this._dataTextFieldBox.Items.Add(dataSourceFieldSchema.Name);
					this._dataValueFieldBox.Items.Add(dataSourceFieldSchema.Name);
				}
				this._dataTextFieldBox.SelectedIndex = 0;
				if (selectedItem != null && preserveSelection)
				{
					if (this._dataTextFieldBox.Items.Contains(selectedItem))
					{
						this._dataTextFieldBox.SelectedItem = selectedItem;
					}
					else
					{
						this._dataTextFieldBox.Items.Insert(0, selectedItem);
					}
				}
				this._dataValueFieldBox.SelectedIndex = 0;
				if (selectedItem2 != null && preserveSelection)
				{
					if (this._dataValueFieldBox.Items.Contains(selectedItem2))
					{
						this._dataValueFieldBox.SelectedItem = selectedItem2;
						return;
					}
					this._dataValueFieldBox.Items.Insert(0, selectedItem2);
				}
			}
		}

		// Token: 0x060028C3 RID: 10435 RVA: 0x000DFB9E File Offset: 0x000DEB9E
		private IComponent GetComponent()
		{
			if (this._controlDesigner != null)
			{
				return this._controlDesigner.Component;
			}
			return null;
		}

		// Token: 0x060028C4 RID: 10436 RVA: 0x000DFBB8 File Offset: 0x000DEBB8
		private IDataSourceFieldSchema[] GetFieldSchemas()
		{
			if (this._fieldSchemas == null)
			{
				IDataSourceViewSchema dataSourceViewSchema = null;
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
				if (dataSourceViewSchema != null)
				{
					this._fieldSchemas = dataSourceViewSchema.GetFields();
				}
			}
			return this._fieldSchemas;
		}

		// Token: 0x060028C5 RID: 10437 RVA: 0x000DFC5C File Offset: 0x000DEC5C
		private void CreatePanel()
		{
			base.SuspendLayout();
			this.CreatePanelControls();
			this.InitializePanelControls();
			base.InitializeForm();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x060028C6 RID: 10438 RVA: 0x000DFC84 File Offset: 0x000DEC84
		private void CreatePanelControls()
		{
			this._dataSourceBox = new ComboBox();
			this._dataTextFieldBox = new ComboBox();
			this._dataValueFieldBox = new ComboBox();
			this._refreshSchemaLink = new LinkLabel();
			this._dataSourceLabel = new global::System.Windows.Forms.Label();
			this._dataTextFieldLabel = new global::System.Windows.Forms.Label();
			this._dataValueFieldLabel = new global::System.Windows.Forms.Label();
			this._dataSourceLabel.Location = new Point(0, 0);
			this._dataSourceLabel.Name = "_dataSourceLabel";
			this._dataSourceLabel.Size = new Size(450, 16);
			this._dataSourceLabel.TabIndex = 0;
			this._dataSourceBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this._dataSourceBox.Location = new Point(0, 18);
			this._dataSourceBox.Name = "_dataSourceBox";
			this._dataSourceBox.Size = new Size(192, 21);
			this._dataSourceBox.TabIndex = 1;
			this._dataSourceBox.SelectedIndexChanged += this.OnSelectedDataSourceChanged;
			this._dataTextFieldLabel.Location = new Point(0, 47);
			this._dataTextFieldLabel.Name = "_dataTextFieldLabel";
			this._dataTextFieldLabel.Size = new Size(450, 16);
			this._dataTextFieldLabel.TabIndex = 2;
			this._dataTextFieldBox.DropDownStyle = ComboBoxStyle.DropDown;
			this._dataTextFieldBox.Location = new Point(0, 65);
			this._dataTextFieldBox.Name = "_dataTextFieldBox";
			this._dataTextFieldBox.Size = new Size(192, 21);
			this._dataTextFieldBox.TabIndex = 3;
			this._dataValueFieldLabel.Location = new Point(0, 94);
			this._dataValueFieldLabel.Name = "_dataValueFieldLabel";
			this._dataValueFieldLabel.Size = new Size(450, 16);
			this._dataValueFieldLabel.TabIndex = 4;
			this._dataValueFieldBox.DropDownStyle = ComboBoxStyle.DropDown;
			this._dataValueFieldBox.Location = new Point(0, 112);
			this._dataValueFieldBox.Name = "_dataValueFieldBox";
			this._dataValueFieldBox.Size = new Size(192, 21);
			this._dataValueFieldBox.TabIndex = 5;
			this._refreshSchemaLink.Links.Add(new LinkLabel.Link(0, 150));
			this._refreshSchemaLink.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			this._refreshSchemaLink.Location = new Point(0, 254);
			this._refreshSchemaLink.Name = "_refreshSchemaLink";
			this._refreshSchemaLink.Size = new Size(290, 16);
			this._refreshSchemaLink.TabIndex = 6;
			this._refreshSchemaLink.TabStop = true;
			this._refreshSchemaLink.Visible = false;
			this._refreshSchemaLink.LinkClicked += this.OnRefreshSchema;
			base.TaskPanel.Controls.Add(this._dataValueFieldLabel);
			base.TaskPanel.Controls.Add(this._dataTextFieldLabel);
			base.TaskPanel.Controls.Add(this._dataSourceLabel);
			base.TaskPanel.Controls.Add(this._refreshSchemaLink);
			base.TaskPanel.Controls.Add(this._dataValueFieldBox);
			base.TaskPanel.Controls.Add(this._dataTextFieldBox);
			base.TaskPanel.Controls.Add(this._dataSourceBox);
		}

		// Token: 0x060028C7 RID: 10439 RVA: 0x000DFFF8 File Offset: 0x000DEFF8
		private void InitializePanelControls()
		{
			string name = this.Control.GetType().Name;
			this._dataSourceLabel.Text = SR.GetString("ListControlCreateDataSource_SelectDataSource");
			this._dataTextFieldLabel.Text = SR.GetString("ListControlCreateDataSource_SelectDataTextField", new object[] { name });
			this._dataValueFieldLabel.Text = SR.GetString("ListControlCreateDataSource_SelectDataValueField", new object[] { name });
			this._refreshSchemaLink.Text = SR.GetString("DataSourceDesigner_RefreshSchemaNoHotkey");
			this.Text = SR.GetString("ListControlCreateDataSource_Title");
			base.AccessibleDescription = SR.GetString("ListControlCreateDataSource_Description", new object[] { name });
			base.CaptionLabel.Text = SR.GetString("ListControlCreateDataSource_Caption");
			this.FillDataSourceList();
		}

		// Token: 0x060028C8 RID: 10440 RVA: 0x000E00C9 File Offset: 0x000DF0C9
		protected override void OnCancelButtonClick(object sender, EventArgs e)
		{
			this.DataSourceID = this._originalDataSourceID;
		}

		// Token: 0x060028C9 RID: 10441 RVA: 0x000E00D7 File Offset: 0x000DF0D7
		protected override void OnClosed(EventArgs e)
		{
			this.ResumeChangedEvents();
		}

		// Token: 0x060028CA RID: 10442 RVA: 0x000E00DF File Offset: 0x000DF0DF
		protected override void OnOKButtonClick(object sender, EventArgs e)
		{
			this.Control.DataTextField = this._dataTextFieldBox.Text;
			this.Control.DataValueField = this._dataValueFieldBox.Text;
			TypeDescriptor.Refresh(this.GetComponent());
		}

		// Token: 0x060028CB RID: 10443 RVA: 0x000E0118 File Offset: 0x000DF118
		private void OnRefreshSchema(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this._fieldSchemas = null;
			IDataSourceDesigner dataSourceDesigner = this._controlDesigner.DataSourceDesigner;
			if (dataSourceDesigner != null && dataSourceDesigner.CanRefreshSchema)
			{
				dataSourceDesigner.RefreshSchema(false);
				this.FillFieldLists(true);
			}
		}

		// Token: 0x060028CC RID: 10444 RVA: 0x000E0154 File Offset: 0x000DF154
		private void OnSelectedDataSourceChanged(object sender, EventArgs e)
		{
			this._fieldSchemas = null;
			this.DataSourceID = this._dataSourceBox.Text;
			string dataSourceID = this._controlDesigner.DataSourceID;
			if (dataSourceID.Length > 0)
			{
				if (!this._dataSourceBox.Items.Contains(dataSourceID))
				{
					this.FillDataSourceList();
				}
				this._dataSourceBox.SelectedItem = dataSourceID;
				this._dataTextFieldBox.Enabled = true;
				this._dataValueFieldBox.Enabled = true;
				base.OKButton.Enabled = true;
				this._refreshSchemaLink.Visible = false;
				if (this._controlDesigner.DataSourceDesigner != null)
				{
					this.SuppressChangedEvents(this._controlDesigner.DataSourceDesigner);
					this._refreshSchemaLink.Visible = this._controlDesigner.DataSourceDesigner.CanRefreshSchema;
				}
				this.FillFieldLists(false);
				string dataTextField = this.Control.DataTextField;
				if (dataTextField.Length > 0)
				{
					int num = -1;
					for (int i = 0; i < this._dataTextFieldBox.Items.Count; i++)
					{
						if (string.Compare(this._dataTextFieldBox.Items[i].ToString(), dataTextField, StringComparison.OrdinalIgnoreCase) == 0)
						{
							num = i;
							break;
						}
					}
					if (this._dataTextFieldBox.Items.Count > 0)
					{
						if (num >= 0)
						{
							this._dataTextFieldBox.SelectedIndex = num;
						}
					}
					else
					{
						this._dataTextFieldBox.Items.Add(dataTextField);
						this._dataTextFieldBox.SelectedIndex = 0;
					}
				}
				string dataValueField = this.Control.DataValueField;
				if (dataValueField.Length > 0)
				{
					int num2 = -1;
					for (int j = 0; j < this._dataValueFieldBox.Items.Count; j++)
					{
						if (string.Compare(this._dataValueFieldBox.Items[j].ToString(), dataValueField, StringComparison.OrdinalIgnoreCase) == 0)
						{
							num2 = j;
							break;
						}
					}
					if (this._dataValueFieldBox.Items.Count <= 0)
					{
						this._dataValueFieldBox.Items.Add(dataValueField);
						this._dataValueFieldBox.SelectedIndex = 0;
						return;
					}
					if (num2 >= 0)
					{
						this._dataValueFieldBox.SelectedIndex = num2;
						return;
					}
				}
			}
			else
			{
				this._dataTextFieldBox.Items.Clear();
				this._dataValueFieldBox.Items.Clear();
				this._dataTextFieldBox.Text = string.Empty;
				this._dataValueFieldBox.Text = string.Empty;
				this._dataTextFieldBox.Enabled = false;
				this._dataValueFieldBox.Enabled = false;
				base.OKButton.Enabled = !string.Equals(dataSourceID, this._originalDataSourceID, StringComparison.Ordinal);
				this._refreshSchemaLink.Visible = false;
			}
		}

		// Token: 0x060028CD RID: 10445 RVA: 0x000E03EC File Offset: 0x000DF3EC
		private void ResumeChangedEvents()
		{
			foreach (IDataSourceDesigner dataSourceDesigner in this.SuppressedDataSources)
			{
				dataSourceDesigner.ResumeDataSourceEvents();
			}
		}

		// Token: 0x060028CE RID: 10446 RVA: 0x000E0438 File Offset: 0x000DF438
		private void SuppressChangedEvents(IDataSourceDesigner dsd)
		{
			if (dsd != null && !this.SuppressedDataSources.Contains(dsd))
			{
				this.SuppressedDataSources.Add(dsd);
				dsd.SuppressDataSourceEvents();
			}
		}

		// Token: 0x04001C29 RID: 7209
		private ListControlDesigner _controlDesigner;

		// Token: 0x04001C2A RID: 7210
		private string _originalDataSourceID;

		// Token: 0x04001C2B RID: 7211
		private IDataSourceFieldSchema[] _fieldSchemas;

		// Token: 0x04001C2C RID: 7212
		private ComboBox _dataSourceBox;

		// Token: 0x04001C2D RID: 7213
		private ComboBox _dataTextFieldBox;

		// Token: 0x04001C2E RID: 7214
		private global::System.Windows.Forms.Label _dataSourceLabel;

		// Token: 0x04001C2F RID: 7215
		private global::System.Windows.Forms.Label _dataTextFieldLabel;

		// Token: 0x04001C30 RID: 7216
		private global::System.Windows.Forms.Label _dataValueFieldLabel;

		// Token: 0x04001C31 RID: 7217
		private ComboBox _dataValueFieldBox;

		// Token: 0x04001C32 RID: 7218
		private LinkLabel _refreshSchemaLink;

		// Token: 0x04001C33 RID: 7219
		private IList<IDataSourceDesigner> _suppressedDataSources;

		// Token: 0x02000460 RID: 1120
		private sealed class TypeDescriptorContext : ITypeDescriptorContext, IServiceProvider
		{
			// Token: 0x060028CF RID: 10447 RVA: 0x000E045D File Offset: 0x000DF45D
			public TypeDescriptorContext(IComponent component)
			{
				this._component = component;
			}

			// Token: 0x1700078C RID: 1932
			// (get) Token: 0x060028D0 RID: 10448 RVA: 0x000E046C File Offset: 0x000DF46C
			public IContainer Container
			{
				get
				{
					ISite site = this._component.Site;
					if (site != null)
					{
						return site.Container;
					}
					return null;
				}
			}

			// Token: 0x1700078D RID: 1933
			// (get) Token: 0x060028D1 RID: 10449 RVA: 0x000E0490 File Offset: 0x000DF490
			public object Instance
			{
				get
				{
					return this._component;
				}
			}

			// Token: 0x1700078E RID: 1934
			// (get) Token: 0x060028D2 RID: 10450 RVA: 0x000E0498 File Offset: 0x000DF498
			public PropertyDescriptor PropertyDescriptor
			{
				get
				{
					return null;
				}
			}

			// Token: 0x060028D3 RID: 10451 RVA: 0x000E049B File Offset: 0x000DF49B
			public object GetService(Type serviceType)
			{
				if (this._component.Site == null)
				{
					return null;
				}
				return this._component.Site.GetService(serviceType);
			}

			// Token: 0x060028D4 RID: 10452 RVA: 0x000E04BD File Offset: 0x000DF4BD
			public bool OnComponentChanging()
			{
				return true;
			}

			// Token: 0x060028D5 RID: 10453 RVA: 0x000E04C0 File Offset: 0x000DF4C0
			public void OnComponentChanged()
			{
			}

			// Token: 0x04001C34 RID: 7220
			private IComponent _component;
		}
	}
}
