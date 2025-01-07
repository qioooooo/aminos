using System;
using System.Design;
using System.Drawing;
using System.Windows.Forms;

namespace System.Web.UI.Design.Util
{
	internal abstract partial class TaskForm : TaskFormBase
	{
		public TaskForm(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.InitializeComponent();
			this.InitializeUI();
		}

		protected Button OKButton
		{
			get
			{
				return this._okButton;
			}
		}

		private void InitializeUI()
		{
			this._cancelButton.Text = SR.GetString("Wizard_CancelButton");
			this._okButton.Text = SR.GetString("OKCaption");
		}

		protected virtual void OnCancelButtonClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		protected virtual void OnOKButtonClick(object sender, EventArgs e)
		{
		}
	}
}
