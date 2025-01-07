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
	internal class AccessDataSourceConnectionChooserPanel : SqlDataSourceConnectionPanel
	{
		public AccessDataSourceConnectionChooserPanel(AccessDataSourceDesigner accessDataSourceDesigner, AccessDataSource accessDataSource)
			: base(accessDataSourceDesigner)
		{
			this._accessDataSource = accessDataSource;
			this._accessDataSourceDesigner = accessDataSourceDesigner;
			this.InitializeComponent();
			this.InitializeUI();
			this.DataFile = this._accessDataSource.DataFile;
		}

		public override DesignerDataConnection DataConnection
		{
			get
			{
				AccessDataSource accessDataSource = new AccessDataSource();
				accessDataSource.DataFile = this.DataFile;
				return new DesignerDataConnection("AccessDataSource", accessDataSource.ProviderName, AccessDataSourceDesigner.GetConnectionString(base.ServiceProvider, accessDataSource));
			}
		}

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

		private void InitializeUI()
		{
			this._dataFileLabel.Text = SR.GetString("AccessDataSourceConnectionChooserPanel_DataFileLabel");
			this._selectFileButton.Text = SR.GetString("AccessDataSourceConnectionChooserPanel_BrowseButton");
			this._helpLabel.Text = SR.GetString("AccessDataSourceConnectionChooserPanel_HelpLabel");
			base.Caption = SR.GetString("AccessDataSourceConnectionChooserPanel_PanelCaption");
		}

		protected internal override void OnComplete()
		{
			if (this._accessDataSource.DataFile != this.DataFile)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this._accessDataSource)["DataFile"];
				propertyDescriptor.ResetValue(this._accessDataSource);
				propertyDescriptor.SetValue(this._accessDataSource, this.DataFile);
			}
		}

		private void OnDataFileTextBoxTextChanged(object sender, EventArgs e)
		{
			this.SetEnabledState();
		}

		private void OnSelectFileButtonClick(object sender, EventArgs e)
		{
			string text = UrlBuilder.BuildUrl(this._accessDataSource, this, this.DataFile, SR.GetString("MdbDataFileEditor_Caption"), SR.GetString("MdbDataFileEditor_Filter"));
			if (text != null)
			{
				this.DataFile = text;
			}
		}

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

		private void SetEnabledState()
		{
			if (base.ParentWizard != null)
			{
				base.ParentWizard.NextButton.Enabled = this._dataFileTextBox.Text.Length > 0;
			}
		}

		private global::System.Windows.Forms.Label _dataFileLabel;

		private global::System.Windows.Forms.TextBox _dataFileTextBox;

		private global::System.Windows.Forms.Button _selectFileButton;

		private global::System.Windows.Forms.Label _helpLabel;

		private AccessDataSource _accessDataSource;

		private AccessDataSourceDesigner _accessDataSourceDesigner;
	}
}
