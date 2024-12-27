using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x0200030E RID: 782
	[ToolboxBitmap(typeof(DataGridViewButtonColumn), "DataGridViewButtonColumn.bmp")]
	public class DataGridViewButtonColumn : DataGridViewColumn
	{
		// Token: 0x060032F8 RID: 13048 RVA: 0x000B307C File Offset: 0x000B207C
		public DataGridViewButtonColumn()
			: base(new DataGridViewButtonCell())
		{
			this.DefaultCellStyle = new DataGridViewCellStyle
			{
				AlignmentInternal = DataGridViewContentAlignment.MiddleCenter
			};
		}

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x060032F9 RID: 13049 RVA: 0x000B30A9 File Offset: 0x000B20A9
		// (set) Token: 0x060032FA RID: 13050 RVA: 0x000B30B4 File Offset: 0x000B20B4
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override DataGridViewCell CellTemplate
		{
			get
			{
				return base.CellTemplate;
			}
			set
			{
				if (value != null && !(value is DataGridViewButtonCell))
				{
					throw new InvalidCastException(SR.GetString("DataGridViewTypeColumn_WrongCellTemplateType", new object[] { "System.Windows.Forms.DataGridViewButtonCell" }));
				}
				base.CellTemplate = value;
			}
		}

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x060032FB RID: 13051 RVA: 0x000B30F3 File Offset: 0x000B20F3
		// (set) Token: 0x060032FC RID: 13052 RVA: 0x000B30FB File Offset: 0x000B20FB
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_ColumnDefaultCellStyleDescr")]
		[Browsable(true)]
		public override DataGridViewCellStyle DefaultCellStyle
		{
			get
			{
				return base.DefaultCellStyle;
			}
			set
			{
				base.DefaultCellStyle = value;
			}
		}

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x060032FD RID: 13053 RVA: 0x000B3104 File Offset: 0x000B2104
		// (set) Token: 0x060032FE RID: 13054 RVA: 0x000B3130 File Offset: 0x000B2130
		[DefaultValue(FlatStyle.Standard)]
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_ButtonColumnFlatStyleDescr")]
		public FlatStyle FlatStyle
		{
			get
			{
				if (this.CellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return ((DataGridViewButtonCell)this.CellTemplate).FlatStyle;
			}
			set
			{
				if (this.FlatStyle != value)
				{
					((DataGridViewButtonCell)this.CellTemplate).FlatStyle = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewButtonCell dataGridViewButtonCell = dataGridViewRow.Cells[base.Index] as DataGridViewButtonCell;
							if (dataGridViewButtonCell != null)
							{
								dataGridViewButtonCell.FlatStyleInternal = value;
							}
						}
						base.DataGridView.OnColumnCommonChange(base.Index);
					}
				}
			}
		}

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x060032FF RID: 13055 RVA: 0x000B31BB File Offset: 0x000B21BB
		// (set) Token: 0x06003300 RID: 13056 RVA: 0x000B31C4 File Offset: 0x000B21C4
		[SRDescription("DataGridView_ButtonColumnTextDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(null)]
		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				if (!string.Equals(value, this.text, StringComparison.Ordinal))
				{
					this.text = value;
					if (base.DataGridView != null)
					{
						if (this.UseColumnTextForButtonValue)
						{
							base.DataGridView.OnColumnCommonChange(base.Index);
							return;
						}
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewButtonCell dataGridViewButtonCell = dataGridViewRow.Cells[base.Index] as DataGridViewButtonCell;
							if (dataGridViewButtonCell != null && dataGridViewButtonCell.UseColumnTextForButtonValue)
							{
								base.DataGridView.OnColumnCommonChange(base.Index);
								return;
							}
						}
						base.DataGridView.InvalidateColumn(base.Index);
					}
				}
			}
		}

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06003301 RID: 13057 RVA: 0x000B327E File Offset: 0x000B227E
		// (set) Token: 0x06003302 RID: 13058 RVA: 0x000B32A8 File Offset: 0x000B22A8
		[DefaultValue(false)]
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_ButtonColumnUseColumnTextForButtonValueDescr")]
		public bool UseColumnTextForButtonValue
		{
			get
			{
				if (this.CellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return ((DataGridViewButtonCell)this.CellTemplate).UseColumnTextForButtonValue;
			}
			set
			{
				if (this.UseColumnTextForButtonValue != value)
				{
					((DataGridViewButtonCell)this.CellTemplate).UseColumnTextForButtonValueInternal = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewButtonCell dataGridViewButtonCell = dataGridViewRow.Cells[base.Index] as DataGridViewButtonCell;
							if (dataGridViewButtonCell != null)
							{
								dataGridViewButtonCell.UseColumnTextForButtonValueInternal = value;
							}
						}
						base.DataGridView.OnColumnCommonChange(base.Index);
					}
				}
			}
		}

		// Token: 0x06003303 RID: 13059 RVA: 0x000B3334 File Offset: 0x000B2334
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewButtonColumn dataGridViewButtonColumn;
			if (type == DataGridViewButtonColumn.columnType)
			{
				dataGridViewButtonColumn = new DataGridViewButtonColumn();
			}
			else
			{
				dataGridViewButtonColumn = (DataGridViewButtonColumn)Activator.CreateInstance(type);
			}
			if (dataGridViewButtonColumn != null)
			{
				base.CloneInternal(dataGridViewButtonColumn);
				dataGridViewButtonColumn.Text = this.text;
			}
			return dataGridViewButtonColumn;
		}

		// Token: 0x06003304 RID: 13060 RVA: 0x000B337C File Offset: 0x000B237C
		private bool ShouldSerializeDefaultCellStyle()
		{
			if (!base.HasDefaultCellStyle)
			{
				return false;
			}
			DataGridViewCellStyle defaultCellStyle = this.DefaultCellStyle;
			return !defaultCellStyle.BackColor.IsEmpty || !defaultCellStyle.ForeColor.IsEmpty || !defaultCellStyle.SelectionBackColor.IsEmpty || !defaultCellStyle.SelectionForeColor.IsEmpty || defaultCellStyle.Font != null || !defaultCellStyle.IsNullValueDefault || !defaultCellStyle.IsDataSourceNullValueDefault || !string.IsNullOrEmpty(defaultCellStyle.Format) || !defaultCellStyle.FormatProvider.Equals(CultureInfo.CurrentCulture) || defaultCellStyle.Alignment != DataGridViewContentAlignment.MiddleCenter || defaultCellStyle.WrapMode != DataGridViewTriState.NotSet || defaultCellStyle.Tag != null || !defaultCellStyle.Padding.Equals(Padding.Empty);
		}

		// Token: 0x06003305 RID: 13061 RVA: 0x000B345C File Offset: 0x000B245C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.Append("DataGridViewButtonColumn { Name=");
			stringBuilder.Append(base.Name);
			stringBuilder.Append(", Index=");
			stringBuilder.Append(base.Index.ToString(CultureInfo.CurrentCulture));
			stringBuilder.Append(" }");
			return stringBuilder.ToString();
		}

		// Token: 0x04001A94 RID: 6804
		private static Type columnType = typeof(DataGridViewButtonColumn);

		// Token: 0x04001A95 RID: 6805
		private string text;
	}
}
