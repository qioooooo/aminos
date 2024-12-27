using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000245 RID: 581
	internal partial class FormatStringDialog : Form
	{
		// Token: 0x0600160F RID: 5647 RVA: 0x00072E78 File Offset: 0x00071E78
		public FormatStringDialog(ITypeDescriptorContext context)
		{
			this.context = context;
			this.InitializeComponent();
			string @string = SR.GetString("RTL");
			if (@string.Equals("RTL_False"))
			{
				this.RightToLeft = RightToLeft.No;
				this.RightToLeftLayout = false;
				return;
			}
			this.RightToLeft = RightToLeft.Yes;
			this.RightToLeftLayout = true;
		}

		// Token: 0x170003B9 RID: 953
		// (set) Token: 0x06001610 RID: 5648 RVA: 0x00072ECD File Offset: 0x00071ECD
		public DataGridViewCellStyle DataGridViewCellStyle
		{
			set
			{
				this.dgvCellStyle = value;
				this.listControl = null;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06001611 RID: 5649 RVA: 0x00072EDD File Offset: 0x00071EDD
		public bool Dirty
		{
			get
			{
				return this.dirty || this.formatControl1.Dirty;
			}
		}

		// Token: 0x170003BB RID: 955
		// (set) Token: 0x06001612 RID: 5650 RVA: 0x00072EF4 File Offset: 0x00071EF4
		public ListControl ListControl
		{
			set
			{
				this.listControl = value;
				this.dgvCellStyle = null;
			}
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x00072F04 File Offset: 0x00071F04
		private void FormatStringDialog_HelpButtonClicked(object sender, CancelEventArgs e)
		{
			this.FormatStringDialog_HelpRequestHandled();
			e.Cancel = true;
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x00072F13 File Offset: 0x00071F13
		private void FormatStringDialog_HelpRequested(object sender, HelpEventArgs e)
		{
			this.FormatStringDialog_HelpRequestHandled();
			e.Handled = true;
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x00072F24 File Offset: 0x00071F24
		private void FormatStringDialog_HelpRequestHandled()
		{
			IHelpService helpService = this.context.GetService(typeof(IHelpService)) as IHelpService;
			if (helpService != null)
			{
				helpService.ShowHelpFromKeyword("vs.FormatStringDialog");
			}
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x00072F5C File Offset: 0x00071F5C
		internal void FormatControlFinishedLoading()
		{
			this.okButton.Top = this.formatControl1.Bottom + 5;
			this.cancelButton.Top = this.formatControl1.Bottom + 5;
			int rightSideOffset = FormatStringDialog.GetRightSideOffset(this.formatControl1);
			int rightSideOffset2 = FormatStringDialog.GetRightSideOffset(this.cancelButton);
			this.okButton.Left += rightSideOffset - rightSideOffset2;
			this.cancelButton.Left += rightSideOffset - rightSideOffset2;
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x00072FDC File Offset: 0x00071FDC
		private static int GetRightSideOffset(Control ctl)
		{
			int num = ctl.Width;
			while (ctl != null)
			{
				num += ctl.Left;
				ctl = ctl.Parent;
			}
			return num;
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x00073008 File Offset: 0x00072008
		private void FormatStringDialog_Load(object sender, EventArgs e)
		{
			string text = ((this.dgvCellStyle != null) ? this.dgvCellStyle.Format : this.listControl.FormatString);
			object obj = ((this.dgvCellStyle != null) ? this.dgvCellStyle.NullValue : null);
			string text2 = string.Empty;
			if (!string.IsNullOrEmpty(text))
			{
				text2 = FormatControl.FormatTypeStringFromFormatString(text);
			}
			if (this.dgvCellStyle != null)
			{
				this.formatControl1.NullValueTextBoxEnabled = true;
			}
			else
			{
				this.formatControl1.NullValueTextBoxEnabled = false;
			}
			this.formatControl1.FormatType = text2;
			FormatControl.FormatTypeClass formatTypeItem = this.formatControl1.FormatTypeItem;
			if (formatTypeItem != null)
			{
				formatTypeItem.PushFormatStringIntoFormatType(text);
			}
			else
			{
				this.formatControl1.FormatType = SR.GetString("BindingFormattingDialogFormatTypeNoFormatting");
			}
			this.formatControl1.NullValue = ((obj != null) ? obj.ToString() : "");
		}

		// Token: 0x06001619 RID: 5657 RVA: 0x000730D8 File Offset: 0x000720D8
		public void End()
		{
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x0007336F File Offset: 0x0007236F
		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.dirty = false;
		}

		// Token: 0x0600161C RID: 5660 RVA: 0x00073378 File Offset: 0x00072378
		private void okButton_Click(object sender, EventArgs e)
		{
			this.PushChanges();
		}

		// Token: 0x0600161D RID: 5661 RVA: 0x00073380 File Offset: 0x00072380
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if ((keyData & Keys.Modifiers) != Keys.None)
			{
				return base.ProcessDialogKey(keyData);
			}
			Keys keys = keyData & Keys.KeyCode;
			if (keys == Keys.Return)
			{
				base.DialogResult = DialogResult.OK;
				this.PushChanges();
				base.Close();
				return true;
			}
			if (keys != Keys.Escape)
			{
				return base.ProcessDialogKey(keyData);
			}
			this.dirty = false;
			base.DialogResult = DialogResult.Cancel;
			base.Close();
			return true;
		}

		// Token: 0x0600161E RID: 5662 RVA: 0x000733E4 File Offset: 0x000723E4
		private void PushChanges()
		{
			FormatControl.FormatTypeClass formatTypeItem = this.formatControl1.FormatTypeItem;
			if (formatTypeItem != null)
			{
				if (this.dgvCellStyle != null)
				{
					this.dgvCellStyle.Format = formatTypeItem.FormatString;
					this.dgvCellStyle.NullValue = this.formatControl1.NullValue;
				}
				else
				{
					this.listControl.FormatString = formatTypeItem.FormatString;
				}
				this.dirty = true;
			}
		}

		// Token: 0x040012D7 RID: 4823
		private ITypeDescriptorContext context;

		// Token: 0x040012DB RID: 4827
		private bool dirty;

		// Token: 0x040012DC RID: 4828
		private DataGridViewCellStyle dgvCellStyle;

		// Token: 0x040012DD RID: 4829
		private ListControl listControl;
	}
}
