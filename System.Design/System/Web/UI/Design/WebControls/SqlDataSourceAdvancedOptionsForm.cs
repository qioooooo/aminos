using System;
using System.Design;
using System.Drawing;
using System.Web.UI.Design.Util;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	internal sealed partial class SqlDataSourceAdvancedOptionsForm : DesignerForm
	{
		public SqlDataSourceAdvancedOptionsForm(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.InitializeComponent();
			this.InitializeUI();
		}

		public bool GenerateStatements
		{
			get
			{
				return this._generateCheckBox.Checked;
			}
			set
			{
				this._generateCheckBox.Checked = value;
				this.UpdateEnabledState();
			}
		}

		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.SqlDataSource.AdvancedOptions";
			}
		}

		public bool OptimisticConcurrency
		{
			get
			{
				return this._optimisticCheckBox.Checked;
			}
			set
			{
				this._optimisticCheckBox.Checked = value;
				this.UpdateEnabledState();
			}
		}

		private void InitializeUI()
		{
			this._helpLabel.Text = SR.GetString("SqlDataSourceAdvancedOptionsForm_HelpLabel");
			this._generateCheckBox.Text = SR.GetString("SqlDataSourceAdvancedOptionsForm_GenerateCheckBox");
			this._generateHelpLabel.Text = SR.GetString("SqlDataSourceAdvancedOptionsForm_GenerateHelpLabel");
			this._optimisticCheckBox.Text = SR.GetString("SqlDataSourceAdvancedOptionsForm_OptimisticCheckBox");
			this._optimisticHelpLabel.Text = SR.GetString("SqlDataSourceAdvancedOptionsForm_OptimisticLabel");
			this.Text = SR.GetString("SqlDataSourceAdvancedOptionsForm_Caption");
			this._generateCheckBox.AccessibleDescription = this._generateHelpLabel.Text;
			this._optimisticCheckBox.AccessibleDescription = this._optimisticHelpLabel.Text;
			this._okButton.Text = SR.GetString("OK");
			this._cancelButton.Text = SR.GetString("Cancel");
			this.UpdateFonts();
		}

		private void OnCancelButtonClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.UpdateFonts();
		}

		private void OnGenerateCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			this.UpdateEnabledState();
		}

		private void OnOkButtonClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		public void SetAllowAutogenerate(bool allowAutogenerate)
		{
			if (!allowAutogenerate)
			{
				this._generateCheckBox.Checked = false;
				this._generateCheckBox.Enabled = false;
				this._generateHelpLabel.Enabled = false;
				this.UpdateEnabledState();
			}
		}

		private void UpdateEnabledState()
		{
			bool @checked = this._generateCheckBox.Checked;
			this._optimisticCheckBox.Enabled = @checked;
			this._optimisticHelpLabel.Enabled = @checked;
			if (!@checked)
			{
				this._optimisticCheckBox.Checked = false;
			}
		}

		private void UpdateFonts()
		{
			Font font = new Font(this.Font, FontStyle.Bold);
			this._generateCheckBox.Font = font;
			this._optimisticCheckBox.Font = font;
		}
	}
}
