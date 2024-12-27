using System;
using System.Design;
using System.Drawing;
using System.Windows.Forms;

namespace System.Web.UI.Design.Util
{
	// Token: 0x020003CD RID: 973
	internal abstract partial class TaskForm : TaskFormBase
	{
		// Token: 0x060023B7 RID: 9143 RVA: 0x000BF8B5 File Offset: 0x000BE8B5
		public TaskForm(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.InitializeComponent();
			this.InitializeUI();
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x060023B8 RID: 9144 RVA: 0x000BF8CA File Offset: 0x000BE8CA
		protected Button OKButton
		{
			get
			{
				return this._okButton;
			}
		}

		// Token: 0x060023BA RID: 9146 RVA: 0x000BFC13 File Offset: 0x000BEC13
		private void InitializeUI()
		{
			this._cancelButton.Text = SR.GetString("Wizard_CancelButton");
			this._okButton.Text = SR.GetString("OKCaption");
		}

		// Token: 0x060023BB RID: 9147 RVA: 0x000BFC3F File Offset: 0x000BEC3F
		protected virtual void OnCancelButtonClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x060023BC RID: 9148 RVA: 0x000BFC4E File Offset: 0x000BEC4E
		protected virtual void OnOKButtonClick(object sender, EventArgs e)
		{
		}
	}
}
