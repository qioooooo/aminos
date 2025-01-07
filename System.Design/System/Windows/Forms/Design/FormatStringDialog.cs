using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	internal partial class FormatStringDialog : Form
	{
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

		public DataGridViewCellStyle DataGridViewCellStyle
		{
			set
			{
				this.dgvCellStyle = value;
				this.listControl = null;
			}
		}

		public bool Dirty
		{
			get
			{
				return this.dirty || this.formatControl1.Dirty;
			}
		}

		public ListControl ListControl
		{
			set
			{
				this.listControl = value;
				this.dgvCellStyle = null;
			}
		}

		private void FormatStringDialog_HelpButtonClicked(object sender, CancelEventArgs e)
		{
			this.FormatStringDialog_HelpRequestHandled();
			e.Cancel = true;
		}

		private void FormatStringDialog_HelpRequested(object sender, HelpEventArgs e)
		{
			this.FormatStringDialog_HelpRequestHandled();
			e.Handled = true;
		}

		private void FormatStringDialog_HelpRequestHandled()
		{
			IHelpService helpService = this.context.GetService(typeof(IHelpService)) as IHelpService;
			if (helpService != null)
			{
				helpService.ShowHelpFromKeyword("vs.FormatStringDialog");
			}
		}

		internal void FormatControlFinishedLoading()
		{
			this.okButton.Top = this.formatControl1.Bottom + 5;
			this.cancelButton.Top = this.formatControl1.Bottom + 5;
			int rightSideOffset = FormatStringDialog.GetRightSideOffset(this.formatControl1);
			int rightSideOffset2 = FormatStringDialog.GetRightSideOffset(this.cancelButton);
			this.okButton.Left += rightSideOffset - rightSideOffset2;
			this.cancelButton.Left += rightSideOffset - rightSideOffset2;
		}

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

		public void End()
		{
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.dirty = false;
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			this.PushChanges();
		}

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

		private ITypeDescriptorContext context;

		private bool dirty;

		private DataGridViewCellStyle dgvCellStyle;

		private ListControl listControl;
	}
}
