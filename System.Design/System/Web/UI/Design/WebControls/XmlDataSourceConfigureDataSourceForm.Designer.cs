namespace System.Web.UI.Design.WebControls
{
	internal sealed partial class XmlDataSourceConfigureDataSourceForm : global::System.Web.UI.Design.Util.DesignerForm
	{
		private void InitializeComponent()
		{
			this._cancelButton = new global::System.Windows.Forms.Button();
			this._okButton = new global::System.Windows.Forms.Button();
			this._dataFileLabel = new global::System.Windows.Forms.Label();
			this._dataFileTextBox = new global::System.Windows.Forms.TextBox();
			this._chooseDataFileButton = new global::System.Windows.Forms.Button();
			this._helpLabel = new global::System.Windows.Forms.Label();
			this._chooseTransformFileButton = new global::System.Windows.Forms.Button();
			this._transformFileTextBox = new global::System.Windows.Forms.TextBox();
			this._transformFileLabel = new global::System.Windows.Forms.Label();
			this._transformFileHelpLabel = new global::System.Windows.Forms.Label();
			this._xpathExpressionTextBox = new global::System.Windows.Forms.TextBox();
			this._xpathExpressionLabel = new global::System.Windows.Forms.Label();
			this._xpathExpressionHelpLabel = new global::System.Windows.Forms.Label();
			base.SuspendLayout();
			this._helpLabel.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._helpLabel.Location = new global::System.Drawing.Point(12, 12);
			this._helpLabel.Name = "_helpLabel";
			this._helpLabel.Size = new global::System.Drawing.Size(501, 40);
			this._helpLabel.TabIndex = 10;
			this._dataFileLabel.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._dataFileLabel.Location = new global::System.Drawing.Point(12, 60);
			this._dataFileLabel.Name = "_dataFileLabel";
			this._dataFileLabel.Size = new global::System.Drawing.Size(410, 16);
			this._dataFileLabel.TabIndex = 20;
			this._dataFileTextBox.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._dataFileTextBox.Location = new global::System.Drawing.Point(12, 78);
			this._dataFileTextBox.Name = "_dataFileTextBox";
			this._dataFileTextBox.Size = new global::System.Drawing.Size(425, 20);
			this._dataFileTextBox.TabIndex = 30;
			this._chooseDataFileButton.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right;
			this._chooseDataFileButton.Location = new global::System.Drawing.Point(444, 78);
			this._chooseDataFileButton.Name = "_chooseDataFileButton";
			this._chooseDataFileButton.Size = new global::System.Drawing.Size(75, 23);
			this._chooseDataFileButton.TabIndex = 40;
			this._chooseDataFileButton.Click += new global::System.EventHandler(this.OnChooseDataFileButtonClick);
			this._transformFileLabel.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._transformFileLabel.Location = new global::System.Drawing.Point(12, 110);
			this._transformFileLabel.Name = "_transformFileLabel";
			this._transformFileLabel.Size = new global::System.Drawing.Size(410, 16);
			this._transformFileLabel.TabIndex = 80;
			this._transformFileTextBox.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._transformFileTextBox.Location = new global::System.Drawing.Point(12, 128);
			this._transformFileTextBox.Name = "_transformFileTextBox";
			this._transformFileTextBox.Size = new global::System.Drawing.Size(425, 20);
			this._transformFileTextBox.TabIndex = 90;
			this._chooseTransformFileButton.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right;
			this._chooseTransformFileButton.Location = new global::System.Drawing.Point(444, 128);
			this._chooseTransformFileButton.Name = "_chooseTransformFileButton";
			this._chooseTransformFileButton.TabIndex = 100;
			this._chooseTransformFileButton.Size = new global::System.Drawing.Size(75, 23);
			this._chooseTransformFileButton.Click += new global::System.EventHandler(this.OnChooseTransformFileButtonClick);
			this._transformFileHelpLabel.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._transformFileHelpLabel.Location = new global::System.Drawing.Point(12, 152);
			this._transformFileHelpLabel.Name = "_transformFileHelpLabel";
			this._transformFileHelpLabel.Size = new global::System.Drawing.Size(504, 32);
			this._transformFileHelpLabel.TabIndex = 105;
			this._xpathExpressionLabel.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._xpathExpressionLabel.Location = new global::System.Drawing.Point(12, 194);
			this._xpathExpressionLabel.Name = "_xpathExpressionLabel";
			this._xpathExpressionLabel.Size = new global::System.Drawing.Size(410, 16);
			this._xpathExpressionLabel.TabIndex = 110;
			this._xpathExpressionTextBox.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._xpathExpressionTextBox.Location = new global::System.Drawing.Point(12, 212);
			this._xpathExpressionTextBox.Name = "_xpathExpressionTextBox";
			this._xpathExpressionTextBox.Size = new global::System.Drawing.Size(425, 20);
			this._xpathExpressionTextBox.TabIndex = 120;
			this._xpathExpressionHelpLabel.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._xpathExpressionHelpLabel.Location = new global::System.Drawing.Point(12, 238);
			this._xpathExpressionHelpLabel.Name = "_xpathExpressionHelpLabel";
			this._xpathExpressionHelpLabel.Size = new global::System.Drawing.Size(504, 30);
			this._xpathExpressionHelpLabel.TabIndex = 125;
			this._okButton.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right;
			this._okButton.Location = new global::System.Drawing.Point(366, 289);
			this._okButton.Name = "_okButton";
			this._okButton.TabIndex = 130;
			this._okButton.Click += new global::System.EventHandler(this.OnOkButtonClick);
			this._cancelButton.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right;
			this._cancelButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Location = new global::System.Drawing.Point(447, 289);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.TabIndex = 140;
			this._cancelButton.Click += new global::System.EventHandler(this.OnCancelButtonClick);
			base.AcceptButton = this._okButton;
			base.CancelButton = this._cancelButton;
			base.ClientSize = new global::System.Drawing.Size(534, 324);
			base.Controls.Add(this._xpathExpressionTextBox);
			base.Controls.Add(this._xpathExpressionLabel);
			base.Controls.Add(this._xpathExpressionHelpLabel);
			base.Controls.Add(this._chooseTransformFileButton);
			base.Controls.Add(this._transformFileTextBox);
			base.Controls.Add(this._transformFileLabel);
			base.Controls.Add(this._transformFileHelpLabel);
			base.Controls.Add(this._helpLabel);
			base.Controls.Add(this._chooseDataFileButton);
			base.Controls.Add(this._dataFileTextBox);
			base.Controls.Add(this._dataFileLabel);
			base.Controls.Add(this._okButton);
			base.Controls.Add(this._cancelButton);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "XmlDataSourceConfigureDataSourceForm";
			base.InitializeForm();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private global::System.Windows.Forms.Label _helpLabel;

		private global::System.Windows.Forms.Label _dataFileLabel;

		private global::System.Windows.Forms.TextBox _dataFileTextBox;

		private global::System.Windows.Forms.Button _chooseDataFileButton;

		private global::System.Windows.Forms.Label _transformFileLabel;

		private global::System.Windows.Forms.TextBox _transformFileTextBox;

		private global::System.Windows.Forms.Button _chooseTransformFileButton;

		private global::System.Windows.Forms.Label _transformFileHelpLabel;

		private global::System.Windows.Forms.Label _xpathExpressionLabel;

		private global::System.Windows.Forms.TextBox _xpathExpressionTextBox;

		private global::System.Windows.Forms.Label _xpathExpressionHelpLabel;

		private global::System.Windows.Forms.Button _okButton;

		private global::System.Windows.Forms.Button _cancelButton;
	}
}
