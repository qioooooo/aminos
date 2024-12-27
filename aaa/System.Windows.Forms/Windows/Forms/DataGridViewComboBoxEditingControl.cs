using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000344 RID: 836
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	public class DataGridViewComboBoxEditingControl : ComboBox, IDataGridViewEditingControl
	{
		// Token: 0x06003572 RID: 13682 RVA: 0x000C096F File Offset: 0x000BF96F
		public DataGridViewComboBoxEditingControl()
		{
			base.TabStop = false;
		}

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x06003573 RID: 13683 RVA: 0x000C097E File Offset: 0x000BF97E
		// (set) Token: 0x06003574 RID: 13684 RVA: 0x000C0986 File Offset: 0x000BF986
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

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x06003575 RID: 13685 RVA: 0x000C098F File Offset: 0x000BF98F
		// (set) Token: 0x06003576 RID: 13686 RVA: 0x000C0998 File Offset: 0x000BF998
		public virtual object EditingControlFormattedValue
		{
			get
			{
				return this.GetEditingControlFormattedValue(DataGridViewDataErrorContexts.Formatting);
			}
			set
			{
				string text = value as string;
				if (text != null)
				{
					this.Text = text;
					if (string.Compare(text, this.Text, true, CultureInfo.CurrentCulture) != 0)
					{
						this.SelectedIndex = -1;
					}
				}
			}
		}

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x06003577 RID: 13687 RVA: 0x000C09D1 File Offset: 0x000BF9D1
		// (set) Token: 0x06003578 RID: 13688 RVA: 0x000C09D9 File Offset: 0x000BF9D9
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

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x06003579 RID: 13689 RVA: 0x000C09E2 File Offset: 0x000BF9E2
		// (set) Token: 0x0600357A RID: 13690 RVA: 0x000C09EA File Offset: 0x000BF9EA
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

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x0600357B RID: 13691 RVA: 0x000C09F3 File Offset: 0x000BF9F3
		public virtual Cursor EditingPanelCursor
		{
			get
			{
				return Cursors.Default;
			}
		}

		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x0600357C RID: 13692 RVA: 0x000C09FA File Offset: 0x000BF9FA
		public virtual bool RepositionEditingControlOnValueChange
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600357D RID: 13693 RVA: 0x000C0A00 File Offset: 0x000BFA00
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
		}

		// Token: 0x0600357E RID: 13694 RVA: 0x000C0A71 File Offset: 0x000BFA71
		public virtual bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
		{
			return (keyData & Keys.KeyCode) == Keys.Down || (keyData & Keys.KeyCode) == Keys.Up || (base.DroppedDown && (keyData & Keys.KeyCode) == Keys.Escape) || (keyData & Keys.KeyCode) == Keys.Return || !dataGridViewWantsInputKey;
		}

		// Token: 0x0600357F RID: 13695 RVA: 0x000C0AAD File Offset: 0x000BFAAD
		public virtual object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
		{
			return this.Text;
		}

		// Token: 0x06003580 RID: 13696 RVA: 0x000C0AB5 File Offset: 0x000BFAB5
		public virtual void PrepareEditingControlForEdit(bool selectAll)
		{
			if (selectAll)
			{
				base.SelectAll();
			}
		}

		// Token: 0x06003581 RID: 13697 RVA: 0x000C0AC0 File Offset: 0x000BFAC0
		private void NotifyDataGridViewOfValueChange()
		{
			this.valueChanged = true;
			this.dataGridView.NotifyCurrentCellDirty(true);
		}

		// Token: 0x06003582 RID: 13698 RVA: 0x000C0AD5 File Offset: 0x000BFAD5
		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			base.OnSelectedIndexChanged(e);
			if (this.SelectedIndex != -1)
			{
				this.NotifyDataGridViewOfValueChange();
			}
		}

		// Token: 0x04001B78 RID: 7032
		private DataGridView dataGridView;

		// Token: 0x04001B79 RID: 7033
		private bool valueChanged;

		// Token: 0x04001B7A RID: 7034
		private int rowIndex;
	}
}
