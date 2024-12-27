using System;
using System.ComponentModel;
using System.ComponentModel.Design.Data;
using System.Design;
using System.Drawing;
using System.IO;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020003D6 RID: 982
	internal class AccessDataSourceConnectionChooserPanel : SqlDataSourceConnectionPanel
	{
		// Token: 0x0600241B RID: 9243 RVA: 0x000C1551 File Offset: 0x000C0551
		public AccessDataSourceConnectionChooserPanel(AccessDataSourceDesigner accessDataSourceDesigner, AccessDataSource accessDataSource)
			: base(accessDataSourceDesigner)
		{
			this._accessDataSource = accessDataSource;
			this._accessDataSourceDesigner = accessDataSourceDesigner;
			this.InitializeComponent();
			this.InitializeUI();
			this.DataFile = this._accessDataSource.DataFile;
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x0600241C RID: 9244 RVA: 0x000C1588 File Offset: 0x000C0588
		public override DesignerDataConnection DataConnection
		{
			get
			{
				AccessDataSource accessDataSource = new AccessDataSource();
				accessDataSource.DataFile = this.DataFile;
				return new DesignerDataConnection("AccessDataSource", accessDataSource.ProviderName, AccessDataSourceDesigner.GetConnectionString(base.ServiceProvider, accessDataSource));
			}
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x0600241D RID: 9245 RVA: 0x000C15C3 File Offset: 0x000C05C3
		// (set) Token: 0x0600241E RID: 9246 RVA: 0x000C15D0 File Offset: 0x000C05D0
		private string DataFile
		{
			get
			{
				return this._dataFileTextBox.Text;
			}
			set
			{
				this._dataFileTextBox.Text = value;
				this._dataFileTextBox.Select(0, 0);
			}
		}

		// Token: 0x0600241F RID: 9247 RVA: 0x000C15EC File Offset: 0x000C05EC
		private void InitializeComponent()
		{
			this._dataFileLabel = new global::System.Windows.Forms.Label();
			this._dataFileTextBox = new global::System.Windows.Forms.TextBox();
			this._selectFileButton = new global::System.Windows.Forms.Button();
			this._helpLabel = new global::System.Windows.Forms.Label();
			base.SuspendLayout();
			this._dataFileLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._dataFileLabel.Location = new Point(0, 0);
			this._dataFileLabel.Name = "_dataFileLabel";
			this._dataFileLabel.Size = new Size(463, 16);
			this._dataFileLabel.TabIndex = 10;
			this._dataFileTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._dataFileTextBox.Location = new Point(0, 18);
			this._dataFileTextBox.Name = "_dataFileTextBox";
			this._dataFileTextBox.Size = new Size(463, 20);
			this._dataFileTextBox.TabIndex = 20;
			this._dataFileTextBox.TextChanged += this.OnDataFileTextBoxTextChanged;
			this._selectFileButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this._selectFileButton.Location = new Point(469, 17);
			this._selectFileButton.Name = "_selectFileButton";
			this._selectFileButton.Size = new Size(75, 23);
			this._selectFileButton.TabIndex = 30;
			this._selectFileButton.Click += this.OnSelectFileButtonClick;
			this._helpLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._helpLabel.Location = new Point(0, 44);
			this._helpLabel.Name = "_helpLabel";
			this._helpLabel.Size = new Size(463, 32);
			this._helpLabel.TabIndex = 40;
			base.Controls.Add(this._helpLabel);
			base.Controls.Add(this._selectFileButton);
			base.Controls.Add(this._dataFileTextBox);
			base.Controls.Add(this._dataFileLabel);
			base.Name = "AccessDataSourceConnectionChooserPanel";
			base.Size = new Size(544, 274);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x06002420 RID: 9248 RVA: 0x000C181C File Offset: 0x000C081C
		private void InitializeUI()
		{
			this._dataFileLabel.Text = SR.GetString("AccessDataSourceConnectionChooserPanel_DataFileLabel");
			this._selectFileButton.Text = SR.GetString("AccessDataSourceConnectionChooserPanel_BrowseButton");
			this._helpLabel.Text = SR.GetString("AccessDataSourceConnectionChooserPanel_HelpLabel");
			base.Caption = SR.GetString("AccessDataSourceConnectionChooserPanel_PanelCaption");
		}

		// Token: 0x06002421 RID: 9249 RVA: 0x000C1878 File Offset: 0x000C0878
		protected internal override void OnComplete()
		{
			if (this._accessDataSource.DataFile != this.DataFile)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this._accessDataSource)["DataFile"];
				propertyDescriptor.ResetValue(this._accessDataSource);
				propertyDescriptor.SetValue(this._accessDataSource, this.DataFile);
			}
		}

		// Token: 0x06002422 RID: 9250 RVA: 0x000C18D1 File Offset: 0x000C08D1
		private void OnDataFileTextBoxTextChanged(object sender, EventArgs e)
		{
			this.SetEnabledState();
		}

		// Token: 0x06002423 RID: 9251 RVA: 0x000C18DC File Offset: 0x000C08DC
		private void OnSelectFileButtonClick(object sender, EventArgs e)
		{
			string text = UrlBuilder.BuildUrl(this._accessDataSource, this, this.DataFile, SR.GetString("MdbDataFileEditor_Caption"), SR.GetString("MdbDataFileEditor_Filter"));
			if (text != null)
			{
				this.DataFile = text;
			}
		}

		// Token: 0x06002424 RID: 9252 RVA: 0x000C191C File Offset: 0x000C091C
		public override bool OnNext()
		{
			string text = UrlPath.MapPath(base.ServiceProvider, this.DataFile);
			if (!File.Exists(text))
			{
				UIServiceHelper.ShowError(base.ServiceProvider, SR.GetString("AccessDataSourceConnectionChooserPanel_FileNotFound", new object[] { this.DataFile }));
				return false;
			}
			return base.OnNext();
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x000C1971 File Offset: 0x000C0971
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			base.ParentWizard.FinishButton.Enabled = false;
			if (base.Visible)
			{
				this.SetEnabledState();
				return;
			}
			base.ParentWizard.NextButton.Enabled = true;
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x000C19AB File Offset: 0x000C09AB
		private void SetEnabledState()
		{
			if (base.ParentWizard != null)
			{
				base.ParentWizard.NextButton.Enabled = this._dataFileTextBox.Text.Length > 0;
			}
		}

		// Token: 0x040018DF RID: 6367
		private global::System.Windows.Forms.Label _dataFileLabel;

		// Token: 0x040018E0 RID: 6368
		private global::System.Windows.Forms.TextBox _dataFileTextBox;

		// Token: 0x040018E1 RID: 6369
		private global::System.Windows.Forms.Button _selectFileButton;

		// Token: 0x040018E2 RID: 6370
		private global::System.Windows.Forms.Label _helpLabel;

		// Token: 0x040018E3 RID: 6371
		private AccessDataSource _accessDataSource;

		// Token: 0x040018E4 RID: 6372
		private AccessDataSourceDesigner _accessDataSourceDesigner;
	}
}
