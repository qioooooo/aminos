using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x0200039C RID: 924
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class DataGridViewTextBoxEditingControl : TextBox, IDataGridViewEditingControl
	{
		// Token: 0x0600386A RID: 14442 RVA: 0x000CDEF2 File Offset: 0x000CCEF2
		public DataGridViewTextBoxEditingControl()
		{
			base.TabStop = false;
		}

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x0600386B RID: 14443 RVA: 0x000CDF01 File Offset: 0x000CCF01
		// (set) Token: 0x0600386C RID: 14444 RVA: 0x000CDF09 File Offset: 0x000CCF09
		public virtual DataGridView EditingControlDataGridView
		{
			get
			{
				return this.dataGridView;
			}
			set
			{
				this.dataGridView = value;
			}
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x0600386D RID: 14445 RVA: 0x000CDF12 File Offset: 0x000CCF12
		// (set) Token: 0x0600386E RID: 14446 RVA: 0x000CDF1B File Offset: 0x000CCF1B
		public virtual object EditingControlFormattedValue
		{
			get
			{
				return this.GetEditingControlFormattedValue(DataGridViewDataErrorContexts.Formatting);
			}
			set
			{
				this.Text = (string)value;
			}
		}

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x0600386F RID: 14447 RVA: 0x000CDF29 File Offset: 0x000CCF29
		// (set) Token: 0x06003870 RID: 14448 RVA: 0x000CDF31 File Offset: 0x000CCF31
		public virtual int EditingControlRowIndex
		{
			get
			{
				return this.rowIndex;
			}
			set
			{
				this.rowIndex = value;
			}
		}

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x06003871 RID: 14449 RVA: 0x000CDF3A File Offset: 0x000CCF3A
		// (set) Token: 0x06003872 RID: 14450 RVA: 0x000CDF42 File Offset: 0x000CCF42
		public virtual bool EditingControlValueChanged
		{
			get
			{
				return this.valueChanged;
			}
			set
			{
				this.valueChanged = value;
			}
		}

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06003873 RID: 14451 RVA: 0x000CDF4B File Offset: 0x000CCF4B
		public virtual Cursor EditingPanelCursor
		{
			get
			{
				return Cursors.Default;
			}
		}

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06003874 RID: 14452 RVA: 0x000CDF52 File Offset: 0x000CCF52
		public virtual bool RepositionEditingControlOnValueChange
		{
			get
			{
				return this.repositionOnValueChange;
			}
		}

		// Token: 0x06003875 RID: 14453 RVA: 0x000CDF5C File Offset: 0x000CCF5C
		public virtual void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
		{
			this.Font = dataGridViewCellStyle.Font;
			if (dataGridViewCellStyle.BackColor.A < 255)
			{
				Color color = Color.FromArgb(255, dataGridViewCellStyle.BackColor);
				this.BackColor = color;
				this.dataGridView.EditingPanel.BackColor = color;
			}
			else
			{
				this.BackColor = dataGridViewCellStyle.BackColor;
			}
			this.ForeColor = dataGridViewCellStyle.ForeColor;
			if (dataGridViewCellStyle.WrapMode == DataGridViewTriState.True)
			{
				base.WordWrap = true;
			}
			base.TextAlign = DataGridViewTextBoxEditingControl.TranslateAlignment(dataGridViewCellStyle.Alignment);
			this.repositionOnValueChange = dataGridViewCellStyle.WrapMode == DataGridViewTriState.True && (dataGridViewCellStyle.Alignment & DataGridViewTextBoxEditingControl.anyTop) == DataGridViewContentAlignment.NotSet;
		}

		// Token: 0x06003876 RID: 14454 RVA: 0x000CE010 File Offset: 0x000CD010
		public virtual bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
		{
			Keys keys = keyData & Keys.KeyCode;
			if (keys != Keys.Return)
			{
				switch (keys)
				{
				case Keys.Prior:
				case Keys.Next:
					if (this.valueChanged)
					{
						return true;
					}
					break;
				case Keys.End:
				case Keys.Home:
					if (this.SelectionLength != this.Text.Length)
					{
						return true;
					}
					break;
				case Keys.Left:
					if ((this.RightToLeft == RightToLeft.No && (this.SelectionLength != 0 || base.SelectionStart != 0)) || (this.RightToLeft == RightToLeft.Yes && (this.SelectionLength != 0 || base.SelectionStart != this.Text.Length)))
					{
						return true;
					}
					break;
				case Keys.Up:
					if (this.Text.IndexOf("\r\n") >= 0 && base.SelectionStart + this.SelectionLength >= this.Text.IndexOf("\r\n"))
					{
						return true;
					}
					break;
				case Keys.Right:
					if ((this.RightToLeft == RightToLeft.No && (this.SelectionLength != 0 || base.SelectionStart != this.Text.Length)) || (this.RightToLeft == RightToLeft.Yes && (this.SelectionLength != 0 || base.SelectionStart != 0)))
					{
						return true;
					}
					break;
				case Keys.Down:
				{
					int num = base.SelectionStart + this.SelectionLength;
					if (this.Text.IndexOf("\r\n", num) != -1)
					{
						return true;
					}
					break;
				}
				case Keys.Delete:
					if (this.SelectionLength > 0 || base.SelectionStart < this.Text.Length)
					{
						return true;
					}
					break;
				}
			}
			else if ((keyData & (Keys.Shift | Keys.Control | Keys.Alt)) == Keys.Shift && this.Multiline && base.AcceptsReturn)
			{
				return true;
			}
			return !dataGridViewWantsInputKey;
		}

		// Token: 0x06003877 RID: 14455 RVA: 0x000CE1B7 File Offset: 0x000CD1B7
		public virtual object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
		{
			return this.Text;
		}

		// Token: 0x06003878 RID: 14456 RVA: 0x000CE1BF File Offset: 0x000CD1BF
		public virtual void PrepareEditingControlForEdit(bool selectAll)
		{
			if (selectAll)
			{
				base.SelectAll();
				return;
			}
			base.SelectionStart = this.Text.Length;
		}

		// Token: 0x06003879 RID: 14457 RVA: 0x000CE1DC File Offset: 0x000CD1DC
		private void NotifyDataGridViewOfValueChange()
		{
			this.valueChanged = true;
			this.dataGridView.NotifyCurrentCellDirty(true);
		}

		// Token: 0x0600387A RID: 14458 RVA: 0x000CE1F1 File Offset: 0x000CD1F1
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			this.dataGridView.OnMouseWheelInternal(e);
		}

		// Token: 0x0600387B RID: 14459 RVA: 0x000CE1FF File Offset: 0x000CD1FF
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			this.NotifyDataGridViewOfValueChange();
		}

		// Token: 0x0600387C RID: 14460 RVA: 0x000CE210 File Offset: 0x000CD210
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override bool ProcessKeyEventArgs(ref Message m)
		{
			Keys keys = (Keys)(int)m.WParam;
			if (keys != Keys.LineFeed)
			{
				if (keys != Keys.Return)
				{
					if (keys == Keys.A)
					{
						if (m.Msg == 256 && Control.ModifierKeys == Keys.Control)
						{
							base.SelectAll();
							return true;
						}
					}
				}
				else if (m.Msg == 258 && (Control.ModifierKeys != Keys.Shift || !this.Multiline || !base.AcceptsReturn))
				{
					return true;
				}
			}
			else if (m.Msg == 258 && Control.ModifierKeys == Keys.Control && this.Multiline && base.AcceptsReturn)
			{
				return true;
			}
			return base.ProcessKeyEventArgs(ref m);
		}

		// Token: 0x0600387D RID: 14461 RVA: 0x000CE2B8 File Offset: 0x000CD2B8
		private static HorizontalAlignment TranslateAlignment(DataGridViewContentAlignment align)
		{
			if ((align & DataGridViewTextBoxEditingControl.anyRight) != DataGridViewContentAlignment.NotSet)
			{
				return HorizontalAlignment.Right;
			}
			if ((align & DataGridViewTextBoxEditingControl.anyCenter) != DataGridViewContentAlignment.NotSet)
			{
				return HorizontalAlignment.Center;
			}
			return HorizontalAlignment.Left;
		}

		// Token: 0x04001C73 RID: 7283
		private static readonly DataGridViewContentAlignment anyTop = (DataGridViewContentAlignment)7;

		// Token: 0x04001C74 RID: 7284
		private static readonly DataGridViewContentAlignment anyRight = (DataGridViewContentAlignment)1092;

		// Token: 0x04001C75 RID: 7285
		private static readonly DataGridViewContentAlignment anyCenter = (DataGridViewContentAlignment)546;

		// Token: 0x04001C76 RID: 7286
		private DataGridView dataGridView;

		// Token: 0x04001C77 RID: 7287
		private bool valueChanged;

		// Token: 0x04001C78 RID: 7288
		private bool repositionOnValueChange;

		// Token: 0x04001C79 RID: 7289
		private int rowIndex;
	}
}
