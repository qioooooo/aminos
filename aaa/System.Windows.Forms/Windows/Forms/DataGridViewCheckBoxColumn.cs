using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x0200032D RID: 813
	[ToolboxBitmap(typeof(DataGridViewCheckBoxColumn), "DataGridViewCheckBoxColumn.bmp")]
	public class DataGridViewCheckBoxColumn : DataGridViewColumn
	{
		// Token: 0x06003417 RID: 13335 RVA: 0x000B7CED File Offset: 0x000B6CED
		public DataGridViewCheckBoxColumn()
			: this(false)
		{
		}

		// Token: 0x06003418 RID: 13336 RVA: 0x000B7CF8 File Offset: 0x000B6CF8
		public DataGridViewCheckBoxColumn(bool threeState)
			: base(new DataGridViewCheckBoxCell(threeState))
		{
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			dataGridViewCellStyle.AlignmentInternal = DataGridViewContentAlignment.MiddleCenter;
			if (threeState)
			{
				dataGridViewCellStyle.NullValue = CheckState.Indeterminate;
			}
			else
			{
				dataGridViewCellStyle.NullValue = false;
			}
			this.DefaultCellStyle = dataGridViewCellStyle;
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06003419 RID: 13337 RVA: 0x000B7D43 File Offset: 0x000B6D43
		// (set) Token: 0x0600341A RID: 13338 RVA: 0x000B7D4C File Offset: 0x000B6D4C
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
				if (value != null && !(value is DataGridViewCheckBoxCell))
				{
					throw new InvalidCastException(SR.GetString("DataGridViewTypeColumn_WrongCellTemplateType", new object[] { "System.Windows.Forms.DataGridViewCheckBoxCell" }));
				}
				base.CellTemplate = value;
			}
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x0600341B RID: 13339 RVA: 0x000B7D8B File Offset: 0x000B6D8B
		private DataGridViewCheckBoxCell CheckBoxCellTemplate
		{
			get
			{
				return (DataGridViewCheckBoxCell)this.CellTemplate;
			}
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x0600341C RID: 13340 RVA: 0x000B7D98 File Offset: 0x000B6D98
		// (set) Token: 0x0600341D RID: 13341 RVA: 0x000B7DA0 File Offset: 0x000B6DA0
		[SRCategory("CatAppearance")]
		[Browsable(true)]
		[SRDescription("DataGridView_ColumnDefaultCellStyleDescr")]
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

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x0600341E RID: 13342 RVA: 0x000B7DA9 File Offset: 0x000B6DA9
		// (set) Token: 0x0600341F RID: 13343 RVA: 0x000B7DD0 File Offset: 0x000B6DD0
		[SRCategory("CatData")]
		[DefaultValue(null)]
		[SRDescription("DataGridView_CheckBoxColumnFalseValueDescr")]
		[TypeConverter(typeof(StringConverter))]
		public object FalseValue
		{
			get
			{
				if (this.CheckBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.CheckBoxCellTemplate.FalseValue;
			}
			set
			{
				if (this.FalseValue != value)
				{
					this.CheckBoxCellTemplate.FalseValueInternal = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewCheckBoxCell dataGridViewCheckBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewCheckBoxCell;
							if (dataGridViewCheckBoxCell != null)
							{
								dataGridViewCheckBoxCell.FalseValueInternal = value;
							}
						}
						base.DataGridView.InvalidateColumn(base.Index);
					}
				}
			}
		}

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x06003420 RID: 13344 RVA: 0x000B7E56 File Offset: 0x000B6E56
		// (set) Token: 0x06003421 RID: 13345 RVA: 0x000B7E7C File Offset: 0x000B6E7C
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_CheckBoxColumnFlatStyleDescr")]
		[DefaultValue(FlatStyle.Standard)]
		public FlatStyle FlatStyle
		{
			get
			{
				if (this.CheckBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.CheckBoxCellTemplate.FlatStyle;
			}
			set
			{
				if (this.FlatStyle != value)
				{
					this.CheckBoxCellTemplate.FlatStyle = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewCheckBoxCell dataGridViewCheckBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewCheckBoxCell;
							if (dataGridViewCheckBoxCell != null)
							{
								dataGridViewCheckBoxCell.FlatStyleInternal = value;
							}
						}
						base.DataGridView.OnColumnCommonChange(base.Index);
					}
				}
			}
		}

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x06003422 RID: 13346 RVA: 0x000B7F02 File Offset: 0x000B6F02
		// (set) Token: 0x06003423 RID: 13347 RVA: 0x000B7F28 File Offset: 0x000B6F28
		[DefaultValue(null)]
		[SRDescription("DataGridView_CheckBoxColumnIndeterminateValueDescr")]
		[TypeConverter(typeof(StringConverter))]
		[SRCategory("CatData")]
		public object IndeterminateValue
		{
			get
			{
				if (this.CheckBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.CheckBoxCellTemplate.IndeterminateValue;
			}
			set
			{
				if (this.IndeterminateValue != value)
				{
					this.CheckBoxCellTemplate.IndeterminateValueInternal = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewCheckBoxCell dataGridViewCheckBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewCheckBoxCell;
							if (dataGridViewCheckBoxCell != null)
							{
								dataGridViewCheckBoxCell.IndeterminateValueInternal = value;
							}
						}
						base.DataGridView.InvalidateColumn(base.Index);
					}
				}
			}
		}

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x06003424 RID: 13348 RVA: 0x000B7FAE File Offset: 0x000B6FAE
		// (set) Token: 0x06003425 RID: 13349 RVA: 0x000B7FD4 File Offset: 0x000B6FD4
		[DefaultValue(false)]
		[SRDescription("DataGridView_CheckBoxColumnThreeStateDescr")]
		[SRCategory("CatBehavior")]
		public bool ThreeState
		{
			get
			{
				if (this.CheckBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.CheckBoxCellTemplate.ThreeState;
			}
			set
			{
				if (this.ThreeState != value)
				{
					this.CheckBoxCellTemplate.ThreeStateInternal = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewCheckBoxCell dataGridViewCheckBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewCheckBoxCell;
							if (dataGridViewCheckBoxCell != null)
							{
								dataGridViewCheckBoxCell.ThreeStateInternal = value;
							}
						}
						base.DataGridView.InvalidateColumn(base.Index);
					}
					if (value && this.DefaultCellStyle.NullValue is bool && !(bool)this.DefaultCellStyle.NullValue)
					{
						this.DefaultCellStyle.NullValue = CheckState.Indeterminate;
						return;
					}
					if (!value && this.DefaultCellStyle.NullValue is CheckState && (CheckState)this.DefaultCellStyle.NullValue == CheckState.Indeterminate)
					{
						this.DefaultCellStyle.NullValue = false;
					}
				}
			}
		}

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06003426 RID: 13350 RVA: 0x000B80CF File Offset: 0x000B70CF
		// (set) Token: 0x06003427 RID: 13351 RVA: 0x000B80F4 File Offset: 0x000B70F4
		[DefaultValue(null)]
		[SRDescription("DataGridView_CheckBoxColumnTrueValueDescr")]
		[TypeConverter(typeof(StringConverter))]
		[SRCategory("CatData")]
		public object TrueValue
		{
			get
			{
				if (this.CheckBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.CheckBoxCellTemplate.TrueValue;
			}
			set
			{
				if (this.TrueValue != value)
				{
					this.CheckBoxCellTemplate.TrueValueInternal = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewCheckBoxCell dataGridViewCheckBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewCheckBoxCell;
							if (dataGridViewCheckBoxCell != null)
							{
								dataGridViewCheckBoxCell.TrueValueInternal = value;
							}
						}
						base.DataGridView.InvalidateColumn(base.Index);
					}
				}
			}
		}

		// Token: 0x06003428 RID: 13352 RVA: 0x000B817C File Offset: 0x000B717C
		private bool ShouldSerializeDefaultCellStyle()
		{
			DataGridViewCheckBoxCell dataGridViewCheckBoxCell = this.CellTemplate as DataGridViewCheckBoxCell;
			if (dataGridViewCheckBoxCell == null)
			{
				return true;
			}
			object obj;
			if (dataGridViewCheckBoxCell.ThreeState)
			{
				obj = CheckState.Indeterminate;
			}
			else
			{
				obj = false;
			}
			if (!base.HasDefaultCellStyle)
			{
				return false;
			}
			DataGridViewCellStyle defaultCellStyle = this.DefaultCellStyle;
			return !defaultCellStyle.BackColor.IsEmpty || !defaultCellStyle.ForeColor.IsEmpty || !defaultCellStyle.SelectionBackColor.IsEmpty || !defaultCellStyle.SelectionForeColor.IsEmpty || defaultCellStyle.Font != null || !defaultCellStyle.NullValue.Equals(obj) || !defaultCellStyle.IsDataSourceNullValueDefault || !string.IsNullOrEmpty(defaultCellStyle.Format) || !defaultCellStyle.FormatProvider.Equals(CultureInfo.CurrentCulture) || defaultCellStyle.Alignment != DataGridViewContentAlignment.MiddleCenter || defaultCellStyle.WrapMode != DataGridViewTriState.NotSet || defaultCellStyle.Tag != null || !defaultCellStyle.Padding.Equals(Padding.Empty);
		}

		// Token: 0x06003429 RID: 13353 RVA: 0x000B828C File Offset: 0x000B728C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.Append("DataGridViewCheckBoxColumn { Name=");
			stringBuilder.Append(base.Name);
			stringBuilder.Append(", Index=");
			stringBuilder.Append(base.Index.ToString(CultureInfo.CurrentCulture));
			stringBuilder.Append(" }");
			return stringBuilder.ToString();
		}
	}
}
