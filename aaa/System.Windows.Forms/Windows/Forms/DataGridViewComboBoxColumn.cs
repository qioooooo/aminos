using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x02000341 RID: 833
	[Designer("System.Windows.Forms.Design.DataGridViewComboBoxColumnDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ToolboxBitmap(typeof(DataGridViewComboBoxColumn), "DataGridViewComboBoxColumn.bmp")]
	public class DataGridViewComboBoxColumn : DataGridViewColumn
	{
		// Token: 0x06003547 RID: 13639 RVA: 0x000C0086 File Offset: 0x000BF086
		public DataGridViewComboBoxColumn()
			: base(new DataGridViewComboBoxCell())
		{
			((DataGridViewComboBoxCell)base.CellTemplate).TemplateComboBoxColumn = this;
		}

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x06003548 RID: 13640 RVA: 0x000C00A4 File Offset: 0x000BF0A4
		// (set) Token: 0x06003549 RID: 13641 RVA: 0x000C00CC File Offset: 0x000BF0CC
		[SRDescription("DataGridView_ComboBoxColumnAutoCompleteDescr")]
		[DefaultValue(true)]
		[SRCategory("CatBehavior")]
		[Browsable(true)]
		public bool AutoComplete
		{
			get
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.ComboBoxCellTemplate.AutoComplete;
			}
			set
			{
				if (this.AutoComplete != value)
				{
					this.ComboBoxCellTemplate.AutoComplete = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewComboBoxCell dataGridViewComboBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewComboBoxCell;
							if (dataGridViewComboBoxCell != null)
							{
								dataGridViewComboBoxCell.AutoComplete = value;
							}
						}
					}
				}
			}
		}

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x0600354A RID: 13642 RVA: 0x000C0141 File Offset: 0x000BF141
		// (set) Token: 0x0600354B RID: 13643 RVA: 0x000C014C File Offset: 0x000BF14C
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
				DataGridViewComboBoxCell dataGridViewComboBoxCell = value as DataGridViewComboBoxCell;
				if (value != null && dataGridViewComboBoxCell == null)
				{
					throw new InvalidCastException(SR.GetString("DataGridViewTypeColumn_WrongCellTemplateType", new object[] { "System.Windows.Forms.DataGridViewComboBoxCell" }));
				}
				base.CellTemplate = value;
				if (value != null)
				{
					dataGridViewComboBoxCell.TemplateComboBoxColumn = this;
				}
			}
		}

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x0600354C RID: 13644 RVA: 0x000C0197 File Offset: 0x000BF197
		private DataGridViewComboBoxCell ComboBoxCellTemplate
		{
			get
			{
				return (DataGridViewComboBoxCell)this.CellTemplate;
			}
		}

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x0600354D RID: 13645 RVA: 0x000C01A4 File Offset: 0x000BF1A4
		// (set) Token: 0x0600354E RID: 13646 RVA: 0x000C01CC File Offset: 0x000BF1CC
		[DefaultValue(null)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[AttributeProvider(typeof(IListSource))]
		[SRCategory("CatData")]
		[SRDescription("DataGridView_ComboBoxColumnDataSourceDescr")]
		public object DataSource
		{
			get
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.ComboBoxCellTemplate.DataSource;
			}
			set
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				this.ComboBoxCellTemplate.DataSource = value;
				if (base.DataGridView != null)
				{
					DataGridViewRowCollection rows = base.DataGridView.Rows;
					int count = rows.Count;
					for (int i = 0; i < count; i++)
					{
						DataGridViewRow dataGridViewRow = rows.SharedRow(i);
						DataGridViewComboBoxCell dataGridViewComboBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewComboBoxCell;
						if (dataGridViewComboBoxCell != null)
						{
							dataGridViewComboBoxCell.DataSource = value;
						}
					}
					base.DataGridView.OnColumnCommonChange(base.Index);
				}
			}
		}

		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x0600354F RID: 13647 RVA: 0x000C0261 File Offset: 0x000BF261
		// (set) Token: 0x06003550 RID: 13648 RVA: 0x000C0288 File Offset: 0x000BF288
		[Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[SRCategory("CatData")]
		[SRDescription("DataGridView_ComboBoxColumnDisplayMemberDescr")]
		[DefaultValue("")]
		public string DisplayMember
		{
			get
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.ComboBoxCellTemplate.DisplayMember;
			}
			set
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				this.ComboBoxCellTemplate.DisplayMember = value;
				if (base.DataGridView != null)
				{
					DataGridViewRowCollection rows = base.DataGridView.Rows;
					int count = rows.Count;
					for (int i = 0; i < count; i++)
					{
						DataGridViewRow dataGridViewRow = rows.SharedRow(i);
						DataGridViewComboBoxCell dataGridViewComboBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewComboBoxCell;
						if (dataGridViewComboBoxCell != null)
						{
							dataGridViewComboBoxCell.DisplayMember = value;
						}
					}
					base.DataGridView.OnColumnCommonChange(base.Index);
				}
			}
		}

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x06003551 RID: 13649 RVA: 0x000C031D File Offset: 0x000BF31D
		// (set) Token: 0x06003552 RID: 13650 RVA: 0x000C0344 File Offset: 0x000BF344
		[DefaultValue(DataGridViewComboBoxDisplayStyle.DropDownButton)]
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_ComboBoxColumnDisplayStyleDescr")]
		public DataGridViewComboBoxDisplayStyle DisplayStyle
		{
			get
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.ComboBoxCellTemplate.DisplayStyle;
			}
			set
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				this.ComboBoxCellTemplate.DisplayStyle = value;
				if (base.DataGridView != null)
				{
					DataGridViewRowCollection rows = base.DataGridView.Rows;
					int count = rows.Count;
					for (int i = 0; i < count; i++)
					{
						DataGridViewRow dataGridViewRow = rows.SharedRow(i);
						DataGridViewComboBoxCell dataGridViewComboBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewComboBoxCell;
						if (dataGridViewComboBoxCell != null)
						{
							dataGridViewComboBoxCell.DisplayStyleInternal = value;
						}
					}
					base.DataGridView.InvalidateColumn(base.Index);
				}
			}
		}

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x06003553 RID: 13651 RVA: 0x000C03D9 File Offset: 0x000BF3D9
		// (set) Token: 0x06003554 RID: 13652 RVA: 0x000C0400 File Offset: 0x000BF400
		[SRDescription("DataGridView_ComboBoxColumnDisplayStyleForCurrentCellOnlyDescr")]
		[DefaultValue(false)]
		[SRCategory("CatAppearance")]
		public bool DisplayStyleForCurrentCellOnly
		{
			get
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.ComboBoxCellTemplate.DisplayStyleForCurrentCellOnly;
			}
			set
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				this.ComboBoxCellTemplate.DisplayStyleForCurrentCellOnly = value;
				if (base.DataGridView != null)
				{
					DataGridViewRowCollection rows = base.DataGridView.Rows;
					int count = rows.Count;
					for (int i = 0; i < count; i++)
					{
						DataGridViewRow dataGridViewRow = rows.SharedRow(i);
						DataGridViewComboBoxCell dataGridViewComboBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewComboBoxCell;
						if (dataGridViewComboBoxCell != null)
						{
							dataGridViewComboBoxCell.DisplayStyleForCurrentCellOnlyInternal = value;
						}
					}
					base.DataGridView.InvalidateColumn(base.Index);
				}
			}
		}

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x06003555 RID: 13653 RVA: 0x000C0495 File Offset: 0x000BF495
		// (set) Token: 0x06003556 RID: 13654 RVA: 0x000C04BC File Offset: 0x000BF4BC
		[DefaultValue(1)]
		[SRCategory("CatBehavior")]
		[SRDescription("DataGridView_ComboBoxColumnDropDownWidthDescr")]
		public int DropDownWidth
		{
			get
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.ComboBoxCellTemplate.DropDownWidth;
			}
			set
			{
				if (this.DropDownWidth != value)
				{
					this.ComboBoxCellTemplate.DropDownWidth = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewComboBoxCell dataGridViewComboBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewComboBoxCell;
							if (dataGridViewComboBoxCell != null)
							{
								dataGridViewComboBoxCell.DropDownWidth = value;
							}
						}
					}
				}
			}
		}

		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x06003557 RID: 13655 RVA: 0x000C0531 File Offset: 0x000BF531
		// (set) Token: 0x06003558 RID: 13656 RVA: 0x000C055C File Offset: 0x000BF55C
		[SRDescription("DataGridView_ComboBoxColumnFlatStyleDescr")]
		[DefaultValue(FlatStyle.Standard)]
		[SRCategory("CatAppearance")]
		public FlatStyle FlatStyle
		{
			get
			{
				if (this.CellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return ((DataGridViewComboBoxCell)this.CellTemplate).FlatStyle;
			}
			set
			{
				if (this.FlatStyle != value)
				{
					((DataGridViewComboBoxCell)this.CellTemplate).FlatStyle = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewComboBoxCell dataGridViewComboBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewComboBoxCell;
							if (dataGridViewComboBoxCell != null)
							{
								dataGridViewComboBoxCell.FlatStyleInternal = value;
							}
						}
						base.DataGridView.OnColumnCommonChange(base.Index);
					}
				}
			}
		}

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x06003559 RID: 13657 RVA: 0x000C05E7 File Offset: 0x000BF5E7
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRCategory("CatData")]
		[SRDescription("DataGridView_ComboBoxColumnItemsDescr")]
		public DataGridViewComboBoxCell.ObjectCollection Items
		{
			get
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.ComboBoxCellTemplate.GetItems(base.DataGridView);
			}
		}

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x0600355A RID: 13658 RVA: 0x000C0612 File Offset: 0x000BF612
		// (set) Token: 0x0600355B RID: 13659 RVA: 0x000C0638 File Offset: 0x000BF638
		[DefaultValue("")]
		[SRCategory("CatData")]
		[Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("DataGridView_ComboBoxColumnValueMemberDescr")]
		[TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string ValueMember
		{
			get
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.ComboBoxCellTemplate.ValueMember;
			}
			set
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				this.ComboBoxCellTemplate.ValueMember = value;
				if (base.DataGridView != null)
				{
					DataGridViewRowCollection rows = base.DataGridView.Rows;
					int count = rows.Count;
					for (int i = 0; i < count; i++)
					{
						DataGridViewRow dataGridViewRow = rows.SharedRow(i);
						DataGridViewComboBoxCell dataGridViewComboBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewComboBoxCell;
						if (dataGridViewComboBoxCell != null)
						{
							dataGridViewComboBoxCell.ValueMember = value;
						}
					}
					base.DataGridView.OnColumnCommonChange(base.Index);
				}
			}
		}

		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x0600355C RID: 13660 RVA: 0x000C06CD File Offset: 0x000BF6CD
		// (set) Token: 0x0600355D RID: 13661 RVA: 0x000C06F4 File Offset: 0x000BF6F4
		[SRCategory("CatBehavior")]
		[SRDescription("DataGridView_ComboBoxColumnMaxDropDownItemsDescr")]
		[DefaultValue(8)]
		public int MaxDropDownItems
		{
			get
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.ComboBoxCellTemplate.MaxDropDownItems;
			}
			set
			{
				if (this.MaxDropDownItems != value)
				{
					this.ComboBoxCellTemplate.MaxDropDownItems = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewComboBoxCell dataGridViewComboBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewComboBoxCell;
							if (dataGridViewComboBoxCell != null)
							{
								dataGridViewComboBoxCell.MaxDropDownItems = value;
							}
						}
					}
				}
			}
		}

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x0600355E RID: 13662 RVA: 0x000C0769 File Offset: 0x000BF769
		// (set) Token: 0x0600355F RID: 13663 RVA: 0x000C0790 File Offset: 0x000BF790
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("DataGridView_ComboBoxColumnSortedDescr")]
		public bool Sorted
		{
			get
			{
				if (this.ComboBoxCellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return this.ComboBoxCellTemplate.Sorted;
			}
			set
			{
				if (this.Sorted != value)
				{
					this.ComboBoxCellTemplate.Sorted = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewComboBoxCell dataGridViewComboBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewComboBoxCell;
							if (dataGridViewComboBoxCell != null)
							{
								dataGridViewComboBoxCell.Sorted = value;
							}
						}
					}
				}
			}
		}

		// Token: 0x06003560 RID: 13664 RVA: 0x000C0808 File Offset: 0x000BF808
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewComboBoxColumn dataGridViewComboBoxColumn;
			if (type == DataGridViewComboBoxColumn.columnType)
			{
				dataGridViewComboBoxColumn = new DataGridViewComboBoxColumn();
			}
			else
			{
				dataGridViewComboBoxColumn = (DataGridViewComboBoxColumn)Activator.CreateInstance(type);
			}
			if (dataGridViewComboBoxColumn != null)
			{
				base.CloneInternal(dataGridViewComboBoxColumn);
				((DataGridViewComboBoxCell)dataGridViewComboBoxColumn.CellTemplate).TemplateComboBoxColumn = dataGridViewComboBoxColumn;
			}
			return dataGridViewComboBoxColumn;
		}

		// Token: 0x06003561 RID: 13665 RVA: 0x000C0854 File Offset: 0x000BF854
		internal void OnItemsCollectionChanged()
		{
			if (base.DataGridView != null)
			{
				DataGridViewRowCollection rows = base.DataGridView.Rows;
				int count = rows.Count;
				object[] array = ((DataGridViewComboBoxCell)this.CellTemplate).Items.InnerArray.ToArray();
				for (int i = 0; i < count; i++)
				{
					DataGridViewRow dataGridViewRow = rows.SharedRow(i);
					DataGridViewComboBoxCell dataGridViewComboBoxCell = dataGridViewRow.Cells[base.Index] as DataGridViewComboBoxCell;
					if (dataGridViewComboBoxCell != null)
					{
						dataGridViewComboBoxCell.Items.ClearInternal();
						dataGridViewComboBoxCell.Items.AddRangeInternal(array);
					}
				}
				base.DataGridView.OnColumnCommonChange(base.Index);
			}
		}

		// Token: 0x06003562 RID: 13666 RVA: 0x000C08F8 File Offset: 0x000BF8F8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.Append("DataGridViewComboBoxColumn { Name=");
			stringBuilder.Append(base.Name);
			stringBuilder.Append(", Index=");
			stringBuilder.Append(base.Index.ToString(CultureInfo.CurrentCulture));
			stringBuilder.Append(" }");
			return stringBuilder.ToString();
		}

		// Token: 0x04001B73 RID: 7027
		private static Type columnType = typeof(DataGridViewComboBoxColumn);
	}
}
