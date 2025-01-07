namespace System.Web.UI.Design.Util
{
	internal abstract partial class WizardForm : global::System.Web.UI.Design.Util.TaskFormBase
	{
		private void InitializeComponent()
		{
			this._wizardButtonsTableLayoutPanel = new global::System.Windows.Forms.TableLayoutPanel();
			this._previousButton = new global::System.Windows.Forms.Button();
			this._nextButton = new global::System.Windows.Forms.Button();
			this._dummyLabel2 = new global::System.Windows.Forms.Label();
			this._finishButton = new global::System.Windows.Forms.Button();
			this._dummyLabel3 = new global::System.Windows.Forms.Label();
			this._cancelButton = new global::System.Windows.Forms.Button();
			this._dummyLabel1 = new global::System.Windows.Forms.Label();
			this._wizardButtonsTableLayoutPanel.SuspendLayout();
			base.SuspendLayout();
			this._wizardButtonsTableLayoutPanel.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right;
			this._wizardButtonsTableLayoutPanel.AutoSize = true;
			this._wizardButtonsTableLayoutPanel.ColumnCount = 7;
			this._wizardButtonsTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 25f));
			this._wizardButtonsTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Absolute, 3f));
			this._wizardButtonsTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 25f));
			this._wizardButtonsTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Absolute, 7f));
			this._wizardButtonsTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 25f));
			this._wizardButtonsTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Absolute, 7f));
			this._wizardButtonsTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 25f));
			this._wizardButtonsTableLayoutPanel.Controls.Add(this._previousButton);
			this._wizardButtonsTableLayoutPanel.Controls.Add(this._dummyLabel1);
			this._wizardButtonsTableLayoutPanel.Controls.Add(this._nextButton);
			this._wizardButtonsTableLayoutPanel.Controls.Add(this._dummyLabel2);
			this._wizardButtonsTableLayoutPanel.Controls.Add(this._finishButton);
			this._wizardButtonsTableLayoutPanel.Controls.Add(this._dummyLabel3);
			this._wizardButtonsTableLayoutPanel.Controls.Add(this._cancelButton);
			this._wizardButtonsTableLayoutPanel.Location = new global::System.Drawing.Point(239, 384);
			this._wizardButtonsTableLayoutPanel.Name = "_wizardButtonsTableLayoutPanel";
			this._wizardButtonsTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			this._wizardButtonsTableLayoutPanel.Size = new global::System.Drawing.Size(317, 23);
			this._wizardButtonsTableLayoutPanel.TabIndex = 100;
			this._previousButton.Anchor = global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._previousButton.AutoSize = true;
			this._previousButton.Enabled = false;
			this._previousButton.Location = new global::System.Drawing.Point(0, 0);
			this._previousButton.Margin = new global::System.Windows.Forms.Padding(0);
			this._previousButton.MinimumSize = new global::System.Drawing.Size(75, 23);
			this._previousButton.Name = "_previousButton";
			this._previousButton.TabIndex = 10;
			this._previousButton.Click += new global::System.EventHandler(this.OnPreviousButtonClick);
			this._dummyLabel1.Location = new global::System.Drawing.Point(75, 0);
			this._dummyLabel1.Margin = new global::System.Windows.Forms.Padding(0);
			this._dummyLabel1.Name = "_dummyLabel1";
			this._dummyLabel1.Size = new global::System.Drawing.Size(3, 0);
			this._dummyLabel1.TabIndex = 20;
			this._nextButton.Anchor = global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._nextButton.AutoSize = true;
			this._nextButton.Location = new global::System.Drawing.Point(78, 0);
			this._nextButton.Margin = new global::System.Windows.Forms.Padding(0);
			this._nextButton.MinimumSize = new global::System.Drawing.Size(75, 23);
			this._nextButton.Name = "_nextButton";
			this._nextButton.TabIndex = 30;
			this._nextButton.Click += new global::System.EventHandler(this.OnNextButtonClick);
			this._dummyLabel2.Location = new global::System.Drawing.Point(153, 0);
			this._dummyLabel2.Margin = new global::System.Windows.Forms.Padding(0);
			this._dummyLabel2.Name = "_dummyLabel2";
			this._dummyLabel2.Size = new global::System.Drawing.Size(7, 0);
			this._dummyLabel2.TabIndex = 40;
			this._finishButton.Anchor = global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._finishButton.AutoSize = true;
			this._finishButton.Enabled = false;
			this._finishButton.Location = new global::System.Drawing.Point(160, 0);
			this._finishButton.Margin = new global::System.Windows.Forms.Padding(0);
			this._finishButton.MinimumSize = new global::System.Drawing.Size(75, 23);
			this._finishButton.Name = "_finishButton";
			this._finishButton.TabIndex = 50;
			this._finishButton.Click += new global::System.EventHandler(this.OnFinishButtonClick);
			this._dummyLabel3.Location = new global::System.Drawing.Point(235, 0);
			this._dummyLabel3.Margin = new global::System.Windows.Forms.Padding(0);
			this._dummyLabel3.Name = "_dummyLabel3";
			this._dummyLabel3.Size = new global::System.Drawing.Size(7, 0);
			this._dummyLabel3.TabIndex = 60;
			this._cancelButton.Anchor = global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
			this._cancelButton.AutoSize = true;
			this._cancelButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Location = new global::System.Drawing.Point(242, 0);
			this._cancelButton.Margin = new global::System.Windows.Forms.Padding(0);
			this._cancelButton.MinimumSize = new global::System.Drawing.Size(75, 23);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.TabIndex = 70;
			this._cancelButton.Click += new global::System.EventHandler(this.OnCancelButtonClick);
			base.AcceptButton = this._nextButton;
			base.CancelButton = this._cancelButton;
			base.Controls.Add(this._wizardButtonsTableLayoutPanel);
			this.MinimumSize = new global::System.Drawing.Size(580, 450);
			base.SizeGripStyle = global::System.Windows.Forms.SizeGripStyle.Show;
			this._wizardButtonsTableLayoutPanel.ResumeLayout(false);
			this._wizardButtonsTableLayoutPanel.PerformLayout();
			base.InitializeForm();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private global::System.Windows.Forms.Button _nextButton;

		private global::System.Windows.Forms.Button _previousButton;

		private global::System.Windows.Forms.Button _cancelButton;

		private global::System.Windows.Forms.Button _finishButton;

		private global::System.Windows.Forms.TableLayoutPanel _wizardButtonsTableLayoutPanel;

		private global::System.Windows.Forms.Label _dummyLabel1;

		private global::System.Windows.Forms.Label _dummyLabel2;

		private global::System.Windows.Forms.Label _dummyLabel3;
	}
}
