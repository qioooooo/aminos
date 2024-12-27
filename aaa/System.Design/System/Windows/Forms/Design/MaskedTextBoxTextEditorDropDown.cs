using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000273 RID: 627
	internal class MaskedTextBoxTextEditorDropDown : UserControl
	{
		// Token: 0x0600179E RID: 6046 RVA: 0x0007AC4C File Offset: 0x00079C4C
		public MaskedTextBoxTextEditorDropDown(MaskedTextBox maskedTextBox)
		{
			this.cloneMtb = MaskedTextBoxDesigner.GetDesignMaskedTextBox(maskedTextBox);
			this.errorProvider = new ErrorProvider();
			((ISupportInitialize)this.errorProvider).BeginInit();
			base.SuspendLayout();
			this.cloneMtb.Dock = DockStyle.Fill;
			this.cloneMtb.TextMaskFormat = MaskFormat.IncludePromptAndLiterals;
			this.cloneMtb.ResetOnPrompt = true;
			this.cloneMtb.SkipLiterals = true;
			this.cloneMtb.ResetOnSpace = true;
			this.cloneMtb.Name = "MaskedTextBoxClone";
			this.cloneMtb.TabIndex = 0;
			this.cloneMtb.MaskInputRejected += this.maskedTextBox_MaskInputRejected;
			this.cloneMtb.KeyDown += this.maskedTextBox_KeyDown;
			this.errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProvider.ContainerControl = this;
			base.Controls.Add(this.cloneMtb);
			this.BackColor = SystemColors.Control;
			base.BorderStyle = BorderStyle.FixedSingle;
			base.Name = "MaskedTextBoxTextEditorDropDown";
			base.Padding = new Padding(16);
			base.Size = new Size(100, 52);
			((ISupportInitialize)this.errorProvider).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x0600179F RID: 6047 RVA: 0x0007AD87 File Offset: 0x00079D87
		public string Value
		{
			get
			{
				if (this.cancel)
				{
					return null;
				}
				return this.cloneMtb.Text;
			}
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x0007AD9E File Offset: 0x00079D9E
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (keyData == Keys.Escape)
			{
				this.cancel = true;
			}
			return base.ProcessDialogKey(keyData);
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x0007ADB3 File Offset: 0x00079DB3
		private void maskedTextBox_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
		{
			this.errorProvider.SetError(this.cloneMtb, MaskedTextBoxDesigner.GetMaskInputRejectedErrorMessage(e));
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x0007ADCC File Offset: 0x00079DCC
		private void maskedTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			this.errorProvider.Clear();
		}

		// Token: 0x0400134D RID: 4941
		private bool cancel;

		// Token: 0x0400134E RID: 4942
		private MaskedTextBox cloneMtb;

		// Token: 0x0400134F RID: 4943
		private ErrorProvider errorProvider;
	}
}
