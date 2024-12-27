using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x02000375 RID: 885
	[ToolboxBitmap(typeof(DataGridViewImageColumn), "DataGridViewImageColumn.bmp")]
	public class DataGridViewImageColumn : DataGridViewColumn
	{
		// Token: 0x0600363C RID: 13884 RVA: 0x000C1CFB File Offset: 0x000C0CFB
		public DataGridViewImageColumn()
			: this(false)
		{
		}

		// Token: 0x0600363D RID: 13885 RVA: 0x000C1D04 File Offset: 0x000C0D04
		public DataGridViewImageColumn(bool valuesAreIcons)
			: base(new DataGridViewImageCell(valuesAreIcons))
		{
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			dataGridViewCellStyle.AlignmentInternal = DataGridViewContentAlignment.MiddleCenter;
			if (valuesAreIcons)
			{
				dataGridViewCellStyle.NullValue = DataGridViewImageCell.ErrorIcon;
			}
			else
			{
				dataGridViewCellStyle.NullValue = DataGridViewImageCell.ErrorBitmap;
			}
			this.DefaultCellStyle = dataGridViewCellStyle;
		}

		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x0600363E RID: 13886 RVA: 0x000C1D4D File Offset: 0x000C0D4D
		// (set) Token: 0x0600363F RID: 13887 RVA: 0x000C1D58 File Offset: 0x000C0D58
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
				if (value != null && !(value is DataGridViewImageCell))
				{
					throw new InvalidCastException(SR.GetString("DataGridViewTypeColumn_WrongCellTemplateType", new object[] { "System.Windows.Forms.DataGridViewImageCell" }));
				}
				base.CellTemplate = value;
			}
		}

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x06003640 RID: 13888 RVA: 0x000C1D97 File Offset: 0x000C0D97
		// (set) Token: 0x06003641 RID: 13889 RVA: 0x000C1D9F File Offset: 0x000C0D9F
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

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x06003642 RID: 13890 RVA: 0x000C1DA8 File Offset: 0x000C0DA8
		// (set) Token: 0x06003643 RID: 13891 RVA: 0x000C1DD0 File Offset: 0x000C0DD0
		[SRDescription("DataGridViewImageColumn_DescriptionDescr")]
		[Browsable(true)]
		[DefaultValue("")]
		[SRCategory("CatAppearance")]
		public string Description
		{
			get
			{
				if (this.CellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.ImageCellTemplate.Description;
			}
			set
			{
				if (this.CellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				this.ImageCellTemplate.Description = value;
				if (base.DataGridView != null)
				{
					DataGridViewRowCollection rows = base.DataGridView.Rows;
					int count = rows.Count;
					for (int i = 0; i < count; i++)
					{
						DataGridViewRow dataGridViewRow = rows.SharedRow(i);
						DataGridViewImageCell dataGridViewImageCell = dataGridViewRow.Cells[base.Index] as DataGridViewImageCell;
						if (dataGridViewImageCell != null)
						{
							dataGridViewImageCell.Description = value;
						}
					}
				}
			}
		}

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x06003644 RID: 13892 RVA: 0x000C1E54 File Offset: 0x000C0E54
		// (set) Token: 0x06003645 RID: 13893 RVA: 0x000C1E5C File Offset: 0x000C0E5C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Icon Icon
		{
			get
			{
				return this.icon;
			}
			set
			{
				this.icon = value;
				if (base.DataGridView != null)
				{
					base.DataGridView.OnColumnCommonChange(base.Index);
				}
			}
		}

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x06003646 RID: 13894 RVA: 0x000C1E7E File Offset: 0x000C0E7E
		// (set) Token: 0x06003647 RID: 13895 RVA: 0x000C1E86 File Offset: 0x000C0E86
		[SRDescription("DataGridViewImageColumn_ImageDescr")]
		[DefaultValue(null)]
		[SRCategory("CatAppearance")]
		public Image Image
		{
			get
			{
				return this.image;
			}
			set
			{
				this.image = value;
				if (base.DataGridView != null)
				{
					base.DataGridView.OnColumnCommonChange(base.Index);
				}
			}
		}

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x06003648 RID: 13896 RVA: 0x000C1EA8 File Offset: 0x000C0EA8
		private DataGridViewImageCell ImageCellTemplate
		{
			get
			{
				return (DataGridViewImageCell)this.CellTemplate;
			}
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x06003649 RID: 13897 RVA: 0x000C1EB8 File Offset: 0x000C0EB8
		// (set) Token: 0x0600364A RID: 13898 RVA: 0x000C1EF0 File Offset: 0x000C0EF0
		[DefaultValue(DataGridViewImageCellLayout.Normal)]
		[SRDescription("DataGridViewImageColumn_ImageLayoutDescr")]
		[SRCategory("CatAppearance")]
		public DataGridViewImageCellLayout ImageLayout
		{
			get
			{
				if (this.CellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				DataGridViewImageCellLayout dataGridViewImageCellLayout = this.ImageCellTemplate.ImageLayout;
				if (dataGridViewImageCellLayout == DataGridViewImageCellLayout.NotSet)
				{
					dataGridViewImageCellLayout = DataGridViewImageCellLayout.Normal;
				}
				return dataGridViewImageCellLayout;
			}
			set
			{
				if (this.ImageLayout != value)
				{
					this.ImageCellTemplate.ImageLayout = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewImageCell dataGridViewImageCell = dataGridViewRow.Cells[base.Index] as DataGridViewImageCell;
							if (dataGridViewImageCell != null)
							{
								dataGridViewImageCell.ImageLayoutInternal = value;
							}
						}
						base.DataGridView.OnColumnCommonChange(base.Index);
					}
				}
			}
		}

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x0600364B RID: 13899 RVA: 0x000C1F76 File Offset: 0x000C0F76
		// (set) Token: 0x0600364C RID: 13900 RVA: 0x000C1F9C File Offset: 0x000C0F9C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool ValuesAreIcons
		{
			get
			{
				if (this.ImageCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.ImageCellTemplate.ValueIsIcon;
			}
			set
			{
				if (this.ValuesAreIcons != value)
				{
					this.ImageCellTemplate.ValueIsIconInternal = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewImageCell dataGridViewImageCell = dataGridViewRow.Cells[base.Index] as DataGridViewImageCell;
							if (dataGridViewImageCell != null)
							{
								dataGridViewImageCell.ValueIsIconInternal = value;
							}
						}
						base.DataGridView.OnColumnCommonChange(base.Index);
					}
					if (value && this.DefaultCellStyle.NullValue is Bitmap && (Bitmap)this.DefaultCellStyle.NullValue == DataGridViewImageCell.ErrorBitmap)
					{
						this.DefaultCellStyle.NullValue = DataGridViewImageCell.ErrorIcon;
						return;
					}
					if (!value && this.DefaultCellStyle.NullValue is Icon && (Icon)this.DefaultCellStyle.NullValue == DataGridViewImageCell.ErrorIcon)
					{
						this.DefaultCellStyle.NullValue = DataGridViewImageCell.ErrorBitmap;
					}
				}
			}
		}

		// Token: 0x0600364D RID: 13901 RVA: 0x000C20A0 File Offset: 0x000C10A0
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewImageColumn dataGridViewImageColumn;
			if (type == DataGridViewImageColumn.columnType)
			{
				dataGridViewImageColumn = new DataGridViewImageColumn();
			}
			else
			{
				dataGridViewImageColumn = (DataGridViewImageColumn)Activator.CreateInstance(type);
			}
			if (dataGridViewImageColumn != null)
			{
				base.CloneInternal(dataGridViewImageColumn);
				dataGridViewImageColumn.Icon = this.icon;
				dataGridViewImageColumn.Image = this.image;
			}
			return dataGridViewImageColumn;
		}

		// Token: 0x0600364E RID: 13902 RVA: 0x000C20F4 File Offset: 0x000C10F4
		private bool ShouldSerializeDefaultCellStyle()
		{
			DataGridViewImageCell dataGridViewImageCell = this.CellTemplate as DataGridViewImageCell;
			if (dataGridViewImageCell == null)
			{
				return true;
			}
			if (!base.HasDefaultCellStyle)
			{
				return false;
			}
			object obj;
			if (dataGridViewImageCell.ValueIsIcon)
			{
				obj = DataGridViewImageCell.ErrorIcon;
			}
			else
			{
				obj = DataGridViewImageCell.ErrorBitmap;
			}
			DataGridViewCellStyle defaultCellStyle = this.DefaultCellStyle;
			return !defaultCellStyle.BackColor.IsEmpty || !defaultCellStyle.ForeColor.IsEmpty || !defaultCellStyle.SelectionBackColor.IsEmpty || !defaultCellStyle.SelectionForeColor.IsEmpty || defaultCellStyle.Font != null || !obj.Equals(defaultCellStyle.NullValue) || !defaultCellStyle.IsDataSourceNullValueDefault || !string.IsNullOrEmpty(defaultCellStyle.Format) || !defaultCellStyle.FormatProvider.Equals(CultureInfo.CurrentCulture) || defaultCellStyle.Alignment != DataGridViewContentAlignment.MiddleCenter || defaultCellStyle.WrapMode != DataGridViewTriState.NotSet || defaultCellStyle.Tag != null || !defaultCellStyle.Padding.Equals(Padding.Empty);
		}

		// Token: 0x0600364F RID: 13903 RVA: 0x000C2200 File Offset: 0x000C1200
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.Append("DataGridViewImageColumn { Name=");
			stringBuilder.Append(base.Name);
			stringBuilder.Append(", Index=");
			stringBuilder.Append(base.Index.ToString(CultureInfo.CurrentCulture));
			stringBuilder.Append(" }");
			return stringBuilder.ToString();
		}

		// Token: 0x04001BCD RID: 7117
		private static Type columnType = typeof(DataGridViewImageColumn);

		// Token: 0x04001BCE RID: 7118
		private Image image;

		// Token: 0x04001BCF RID: 7119
		private Icon icon;
	}
}
