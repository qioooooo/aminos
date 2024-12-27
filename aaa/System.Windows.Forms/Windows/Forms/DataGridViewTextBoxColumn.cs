using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x0200039B RID: 923
	[ToolboxBitmap(typeof(DataGridViewTextBoxColumn), "DataGridViewTextBoxColumn.bmp")]
	public class DataGridViewTextBoxColumn : DataGridViewColumn
	{
		// Token: 0x06003861 RID: 14433 RVA: 0x000CDD76 File Offset: 0x000CCD76
		public DataGridViewTextBoxColumn()
			: base(new DataGridViewTextBoxCell())
		{
			this.SortMode = DataGridViewColumnSortMode.Automatic;
		}

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06003862 RID: 14434 RVA: 0x000CDD8A File Offset: 0x000CCD8A
		// (set) Token: 0x06003863 RID: 14435 RVA: 0x000CDD94 File Offset: 0x000CCD94
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public override DataGridViewCell CellTemplate
		{
			get
			{
				return base.CellTemplate;
			}
			set
			{
				if (value != null && !(value is DataGridViewTextBoxCell))
				{
					throw new InvalidCastException(SR.GetString("DataGridViewTypeColumn_WrongCellTemplateType", new object[] { "System.Windows.Forms.DataGridViewTextBoxCell" }));
				}
				base.CellTemplate = value;
			}
		}

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x06003864 RID: 14436 RVA: 0x000CDDD3 File Offset: 0x000CCDD3
		// (set) Token: 0x06003865 RID: 14437 RVA: 0x000CDDF8 File Offset: 0x000CCDF8
		[SRCategory("CatBehavior")]
		[SRDescription("DataGridView_TextBoxColumnMaxInputLengthDescr")]
		[DefaultValue(32767)]
		public int MaxInputLength
		{
			get
			{
				if (this.TextBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.TextBoxCellTemplate.MaxInputLength;
			}
			set
			{
				if (this.MaxInputLength != value)
				{
					this.TextBoxCellTemplate.MaxInputLength = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewTextBoxCell dataGridViewTextBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewTextBoxCell;
							if (dataGridViewTextBoxCell != null)
							{
								dataGridViewTextBoxCell.MaxInputLength = value;
							}
						}
					}
				}
			}
		}

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x06003866 RID: 14438 RVA: 0x000CDE6D File Offset: 0x000CCE6D
		// (set) Token: 0x06003867 RID: 14439 RVA: 0x000CDE75 File Offset: 0x000CCE75
		[DefaultValue(DataGridViewColumnSortMode.Automatic)]
		public new DataGridViewColumnSortMode SortMode
		{
			get
			{
				return base.SortMode;
			}
			set
			{
				base.SortMode = value;
			}
		}

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x06003868 RID: 14440 RVA: 0x000CDE7E File Offset: 0x000CCE7E
		private DataGridViewTextBoxCell TextBoxCellTemplate
		{
			get
			{
				return (DataGridViewTextBoxCell)this.CellTemplate;
			}
		}

		// Token: 0x06003869 RID: 14441 RVA: 0x000CDE8C File Offset: 0x000CCE8C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.Append("DataGridViewTextBoxColumn { Name=");
			stringBuilder.Append(base.Name);
			stringBuilder.Append(", Index=");
			stringBuilder.Append(base.Index.ToString(CultureInfo.CurrentCulture));
			stringBuilder.Append(" }");
			return stringBuilder.ToString();
		}

		// Token: 0x04001C72 RID: 7282
		private const int DATAGRIDVIEWTEXTBOXCOLUMN_maxInputLength = 32767;
	}
}
