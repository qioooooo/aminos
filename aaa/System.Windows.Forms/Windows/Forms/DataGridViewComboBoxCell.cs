using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x0200033D RID: 829
	public class DataGridViewComboBoxCell : DataGridViewCell
	{
		// Token: 0x060034C9 RID: 13513 RVA: 0x000BC3F0 File Offset: 0x000BB3F0
		public DataGridViewComboBoxCell()
		{
			this.flags = 8;
		}

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x060034CA RID: 13514 RVA: 0x000BC3FF File Offset: 0x000BB3FF
		// (set) Token: 0x060034CB RID: 13515 RVA: 0x000BC410 File Offset: 0x000BB410
		[DefaultValue(true)]
		public virtual bool AutoComplete
		{
			get
			{
				return (this.flags & 8) != 0;
			}
			set
			{
				if (value != this.AutoComplete)
				{
					if (value)
					{
						this.flags |= 8;
					}
					else
					{
						this.flags = (byte)((int)this.flags & -9);
					}
					if (this.OwnsEditingComboBox(base.RowIndex))
					{
						if (value)
						{
							this.EditingComboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
							this.EditingComboBox.AutoCompleteMode = AutoCompleteMode.Append;
							return;
						}
						this.EditingComboBox.AutoCompleteMode = AutoCompleteMode.None;
						this.EditingComboBox.AutoCompleteSource = AutoCompleteSource.None;
					}
				}
			}
		}

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x060034CC RID: 13516 RVA: 0x000BC494 File Offset: 0x000BB494
		// (set) Token: 0x060034CD RID: 13517 RVA: 0x000BC4A2 File Offset: 0x000BB4A2
		private CurrencyManager DataManager
		{
			get
			{
				return this.GetDataManager(base.DataGridView);
			}
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewComboBoxCell.PropComboBoxCellDataManager))
				{
					base.Properties.SetObject(DataGridViewComboBoxCell.PropComboBoxCellDataManager, value);
				}
			}
		}

		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x060034CE RID: 13518 RVA: 0x000BC4CA File Offset: 0x000BB4CA
		// (set) Token: 0x060034CF RID: 13519 RVA: 0x000BC4DC File Offset: 0x000BB4DC
		public virtual object DataSource
		{
			get
			{
				return base.Properties.GetObject(DataGridViewComboBoxCell.PropComboBoxCellDataSource);
			}
			set
			{
				if (value != null && !(value is IList) && !(value is IListSource))
				{
					throw new ArgumentException(SR.GetString("BadDataSourceForComplexBinding"));
				}
				if (this.DataSource != value)
				{
					this.DataManager = null;
					this.UnwireDataSource();
					base.Properties.SetObject(DataGridViewComboBoxCell.PropComboBoxCellDataSource, value);
					this.WireDataSource(value);
					this.CreateItemsFromDataSource = true;
					DataGridViewComboBoxCell.cachedDropDownWidth = -1;
					try
					{
						this.InitializeDisplayMemberPropertyDescriptor(this.DisplayMember);
					}
					catch (Exception ex)
					{
						if (ClientUtils.IsCriticalException(ex))
						{
							throw;
						}
						this.DisplayMemberInternal = null;
					}
					try
					{
						this.InitializeValueMemberPropertyDescriptor(this.ValueMember);
					}
					catch (Exception ex2)
					{
						if (ClientUtils.IsCriticalException(ex2))
						{
							throw;
						}
						this.ValueMemberInternal = null;
					}
					if (value == null)
					{
						this.DisplayMemberInternal = null;
						this.ValueMemberInternal = null;
					}
					if (this.OwnsEditingComboBox(base.RowIndex))
					{
						this.EditingComboBox.DataSource = value;
						this.InitializeComboBoxText();
						return;
					}
					base.OnCommonChange();
				}
			}
		}

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x060034D0 RID: 13520 RVA: 0x000BC5E4 File Offset: 0x000BB5E4
		// (set) Token: 0x060034D1 RID: 13521 RVA: 0x000BC611 File Offset: 0x000BB611
		[DefaultValue("")]
		public virtual string DisplayMember
		{
			get
			{
				object @object = base.Properties.GetObject(DataGridViewComboBoxCell.PropComboBoxCellDisplayMember);
				if (@object == null)
				{
					return string.Empty;
				}
				return (string)@object;
			}
			set
			{
				this.DisplayMemberInternal = value;
				if (this.OwnsEditingComboBox(base.RowIndex))
				{
					this.EditingComboBox.DisplayMember = value;
					this.InitializeComboBoxText();
					return;
				}
				base.OnCommonChange();
			}
		}

		// Token: 0x17000993 RID: 2451
		// (set) Token: 0x060034D2 RID: 13522 RVA: 0x000BC641 File Offset: 0x000BB641
		private string DisplayMemberInternal
		{
			set
			{
				this.InitializeDisplayMemberPropertyDescriptor(value);
				if ((value != null && value.Length > 0) || base.Properties.ContainsObject(DataGridViewComboBoxCell.PropComboBoxCellDisplayMember))
				{
					base.Properties.SetObject(DataGridViewComboBoxCell.PropComboBoxCellDisplayMember, value);
				}
			}
		}

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x060034D3 RID: 13523 RVA: 0x000BC679 File Offset: 0x000BB679
		// (set) Token: 0x060034D4 RID: 13524 RVA: 0x000BC690 File Offset: 0x000BB690
		private PropertyDescriptor DisplayMemberProperty
		{
			get
			{
				return (PropertyDescriptor)base.Properties.GetObject(DataGridViewComboBoxCell.PropComboBoxCellDisplayMemberProp);
			}
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewComboBoxCell.PropComboBoxCellDisplayMemberProp))
				{
					base.Properties.SetObject(DataGridViewComboBoxCell.PropComboBoxCellDisplayMemberProp, value);
				}
			}
		}

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x060034D5 RID: 13525 RVA: 0x000BC6B8 File Offset: 0x000BB6B8
		// (set) Token: 0x060034D6 RID: 13526 RVA: 0x000BC6E0 File Offset: 0x000BB6E0
		[DefaultValue(DataGridViewComboBoxDisplayStyle.DropDownButton)]
		public DataGridViewComboBoxDisplayStyle DisplayStyle
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewComboBoxCell.PropComboBoxCellDisplayStyle, out flag);
				if (flag)
				{
					return (DataGridViewComboBoxDisplayStyle)integer;
				}
				return DataGridViewComboBoxDisplayStyle.DropDownButton;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridViewComboBoxDisplayStyle));
				}
				if (value != this.DisplayStyle)
				{
					base.Properties.SetInteger(DataGridViewComboBoxCell.PropComboBoxCellDisplayStyle, (int)value);
					if (base.DataGridView != null)
					{
						if (base.RowIndex != -1)
						{
							base.DataGridView.InvalidateCell(this);
							return;
						}
						base.DataGridView.InvalidateColumnInternal(base.ColumnIndex);
					}
				}
			}
		}

		// Token: 0x17000996 RID: 2454
		// (set) Token: 0x060034D7 RID: 13527 RVA: 0x000BC75C File Offset: 0x000BB75C
		internal DataGridViewComboBoxDisplayStyle DisplayStyleInternal
		{
			set
			{
				if (value != this.DisplayStyle)
				{
					base.Properties.SetInteger(DataGridViewComboBoxCell.PropComboBoxCellDisplayStyle, (int)value);
				}
			}
		}

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x060034D8 RID: 13528 RVA: 0x000BC778 File Offset: 0x000BB778
		// (set) Token: 0x060034D9 RID: 13529 RVA: 0x000BC7A4 File Offset: 0x000BB7A4
		[DefaultValue(false)]
		public bool DisplayStyleForCurrentCellOnly
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewComboBoxCell.PropComboBoxCellDisplayStyleForCurrentCellOnly, out flag);
				return flag && integer != 0;
			}
			set
			{
				if (value != this.DisplayStyleForCurrentCellOnly)
				{
					base.Properties.SetInteger(DataGridViewComboBoxCell.PropComboBoxCellDisplayStyleForCurrentCellOnly, value ? 1 : 0);
					if (base.DataGridView != null)
					{
						if (base.RowIndex != -1)
						{
							base.DataGridView.InvalidateCell(this);
							return;
						}
						base.DataGridView.InvalidateColumnInternal(base.ColumnIndex);
					}
				}
			}
		}

		// Token: 0x17000998 RID: 2456
		// (set) Token: 0x060034DA RID: 13530 RVA: 0x000BC800 File Offset: 0x000BB800
		internal bool DisplayStyleForCurrentCellOnlyInternal
		{
			set
			{
				if (value != this.DisplayStyleForCurrentCellOnly)
				{
					base.Properties.SetInteger(DataGridViewComboBoxCell.PropComboBoxCellDisplayStyleForCurrentCellOnly, value ? 1 : 0);
				}
			}
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x060034DB RID: 13531 RVA: 0x000BC822 File Offset: 0x000BB822
		private Type DisplayType
		{
			get
			{
				if (this.DisplayMemberProperty != null)
				{
					return this.DisplayMemberProperty.PropertyType;
				}
				if (this.ValueMemberProperty != null)
				{
					return this.ValueMemberProperty.PropertyType;
				}
				return DataGridViewComboBoxCell.defaultFormattedValueType;
			}
		}

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x060034DC RID: 13532 RVA: 0x000BC851 File Offset: 0x000BB851
		private TypeConverter DisplayTypeConverter
		{
			get
			{
				if (base.DataGridView != null)
				{
					return base.DataGridView.GetCachedTypeConverter(this.DisplayType);
				}
				return TypeDescriptor.GetConverter(this.DisplayType);
			}
		}

		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x060034DD RID: 13533 RVA: 0x000BC878 File Offset: 0x000BB878
		// (set) Token: 0x060034DE RID: 13534 RVA: 0x000BC8A0 File Offset: 0x000BB8A0
		[DefaultValue(1)]
		public virtual int DropDownWidth
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewComboBoxCell.PropComboBoxCellDropDownWidth, out flag);
				if (!flag)
				{
					return 1;
				}
				return integer;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("DropDownWidth", value, SR.GetString("DataGridViewComboBoxCell_DropDownWidthOutOfRange", new object[] { 1.ToString(CultureInfo.CurrentCulture) }));
				}
				base.Properties.SetInteger(DataGridViewComboBoxCell.PropComboBoxCellDropDownWidth, value);
				if (this.OwnsEditingComboBox(base.RowIndex))
				{
					this.EditingComboBox.DropDownWidth = value;
				}
			}
		}

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x060034DF RID: 13535 RVA: 0x000BC910 File Offset: 0x000BB910
		// (set) Token: 0x060034E0 RID: 13536 RVA: 0x000BC927 File Offset: 0x000BB927
		private DataGridViewComboBoxEditingControl EditingComboBox
		{
			get
			{
				return (DataGridViewComboBoxEditingControl)base.Properties.GetObject(DataGridViewComboBoxCell.PropComboBoxCellEditingComboBox);
			}
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewComboBoxCell.PropComboBoxCellEditingComboBox))
				{
					base.Properties.SetObject(DataGridViewComboBoxCell.PropComboBoxCellEditingComboBox, value);
				}
			}
		}

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x060034E1 RID: 13537 RVA: 0x000BC94F File Offset: 0x000BB94F
		public override Type EditType
		{
			get
			{
				return DataGridViewComboBoxCell.defaultEditType;
			}
		}

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x060034E2 RID: 13538 RVA: 0x000BC958 File Offset: 0x000BB958
		// (set) Token: 0x060034E3 RID: 13539 RVA: 0x000BC980 File Offset: 0x000BB980
		[DefaultValue(FlatStyle.Standard)]
		public FlatStyle FlatStyle
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewComboBoxCell.PropComboBoxCellFlatStyle, out flag);
				if (flag)
				{
					return (FlatStyle)integer;
				}
				return FlatStyle.Standard;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(FlatStyle));
				}
				if (value != this.FlatStyle)
				{
					base.Properties.SetInteger(DataGridViewComboBoxCell.PropComboBoxCellFlatStyle, (int)value);
					base.OnCommonChange();
				}
			}
		}

		// Token: 0x1700099F RID: 2463
		// (set) Token: 0x060034E4 RID: 13540 RVA: 0x000BC9D3 File Offset: 0x000BB9D3
		internal FlatStyle FlatStyleInternal
		{
			set
			{
				if (value != this.FlatStyle)
				{
					base.Properties.SetInteger(DataGridViewComboBoxCell.PropComboBoxCellFlatStyle, (int)value);
				}
			}
		}

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x060034E5 RID: 13541 RVA: 0x000BC9EF File Offset: 0x000BB9EF
		public override Type FormattedValueType
		{
			get
			{
				return DataGridViewComboBoxCell.defaultFormattedValueType;
			}
		}

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x060034E6 RID: 13542 RVA: 0x000BC9F6 File Offset: 0x000BB9F6
		internal bool HasItems
		{
			get
			{
				return base.Properties.ContainsObject(DataGridViewComboBoxCell.PropComboBoxCellItems) && base.Properties.GetObject(DataGridViewComboBoxCell.PropComboBoxCellItems) != null;
			}
		}

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x060034E7 RID: 13543 RVA: 0x000BCA22 File Offset: 0x000BBA22
		[Browsable(false)]
		public virtual DataGridViewComboBoxCell.ObjectCollection Items
		{
			get
			{
				return this.GetItems(base.DataGridView);
			}
		}

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x060034E8 RID: 13544 RVA: 0x000BCA30 File Offset: 0x000BBA30
		// (set) Token: 0x060034E9 RID: 13545 RVA: 0x000BCA58 File Offset: 0x000BBA58
		[DefaultValue(8)]
		public virtual int MaxDropDownItems
		{
			get
			{
				bool flag;
				int integer = base.Properties.GetInteger(DataGridViewComboBoxCell.PropComboBoxCellMaxDropDownItems, out flag);
				if (flag)
				{
					return integer;
				}
				return 8;
			}
			set
			{
				if (value < 1 || value > 100)
				{
					throw new ArgumentOutOfRangeException("MaxDropDownItems", value, SR.GetString("DataGridViewComboBoxCell_MaxDropDownItemsOutOfRange", new object[]
					{
						1.ToString(CultureInfo.CurrentCulture),
						100.ToString(CultureInfo.CurrentCulture)
					}));
				}
				base.Properties.SetInteger(DataGridViewComboBoxCell.PropComboBoxCellMaxDropDownItems, value);
				if (this.OwnsEditingComboBox(base.RowIndex))
				{
					this.EditingComboBox.MaxDropDownItems = value;
				}
			}
		}

		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x060034EA RID: 13546 RVA: 0x000BCAE0 File Offset: 0x000BBAE0
		private bool PaintXPThemes
		{
			get
			{
				return this.FlatStyle != FlatStyle.Flat && this.FlatStyle != FlatStyle.Popup && base.DataGridView.ApplyVisualStylesToInnerCells;
			}
		}

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x060034EB RID: 13547 RVA: 0x000BCB12 File Offset: 0x000BBB12
		private static bool PostXPThemesExist
		{
			get
			{
				return VisualStyleRenderer.IsElementDefined(VisualStyleElement.ComboBox.ReadOnlyButton.Normal);
			}
		}

		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x060034EC RID: 13548 RVA: 0x000BCB1E File Offset: 0x000BBB1E
		// (set) Token: 0x060034ED RID: 13549 RVA: 0x000BCB30 File Offset: 0x000BBB30
		[DefaultValue(false)]
		public virtual bool Sorted
		{
			get
			{
				return (this.flags & 2) != 0;
			}
			set
			{
				if (value != this.Sorted)
				{
					if (value)
					{
						if (this.DataSource != null)
						{
							throw new ArgumentException(SR.GetString("ComboBoxSortWithDataSource"));
						}
						this.Items.SortInternal();
						this.flags |= 2;
					}
					else
					{
						this.flags = (byte)((int)this.flags & -3);
					}
					if (this.OwnsEditingComboBox(base.RowIndex))
					{
						this.EditingComboBox.Sorted = value;
					}
				}
			}
		}

		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x060034EE RID: 13550 RVA: 0x000BCBA9 File Offset: 0x000BBBA9
		// (set) Token: 0x060034EF RID: 13551 RVA: 0x000BCBC0 File Offset: 0x000BBBC0
		internal DataGridViewComboBoxColumn TemplateComboBoxColumn
		{
			get
			{
				return (DataGridViewComboBoxColumn)base.Properties.GetObject(DataGridViewComboBoxCell.PropComboBoxCellColumnTemplate);
			}
			set
			{
				base.Properties.SetObject(DataGridViewComboBoxCell.PropComboBoxCellColumnTemplate, value);
			}
		}

		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x060034F0 RID: 13552 RVA: 0x000BCBD4 File Offset: 0x000BBBD4
		// (set) Token: 0x060034F1 RID: 13553 RVA: 0x000BCC01 File Offset: 0x000BBC01
		[DefaultValue("")]
		public virtual string ValueMember
		{
			get
			{
				object @object = base.Properties.GetObject(DataGridViewComboBoxCell.PropComboBoxCellValueMember);
				if (@object == null)
				{
					return string.Empty;
				}
				return (string)@object;
			}
			set
			{
				this.ValueMemberInternal = value;
				if (this.OwnsEditingComboBox(base.RowIndex))
				{
					this.EditingComboBox.ValueMember = value;
					this.InitializeComboBoxText();
					return;
				}
				base.OnCommonChange();
			}
		}

		// Token: 0x170009A9 RID: 2473
		// (set) Token: 0x060034F2 RID: 13554 RVA: 0x000BCC31 File Offset: 0x000BBC31
		private string ValueMemberInternal
		{
			set
			{
				this.InitializeValueMemberPropertyDescriptor(value);
				if ((value != null && value.Length > 0) || base.Properties.ContainsObject(DataGridViewComboBoxCell.PropComboBoxCellValueMember))
				{
					base.Properties.SetObject(DataGridViewComboBoxCell.PropComboBoxCellValueMember, value);
				}
			}
		}

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x060034F3 RID: 13555 RVA: 0x000BCC69 File Offset: 0x000BBC69
		// (set) Token: 0x060034F4 RID: 13556 RVA: 0x000BCC80 File Offset: 0x000BBC80
		private PropertyDescriptor ValueMemberProperty
		{
			get
			{
				return (PropertyDescriptor)base.Properties.GetObject(DataGridViewComboBoxCell.PropComboBoxCellValueMemberProp);
			}
			set
			{
				if (value != null || base.Properties.ContainsObject(DataGridViewComboBoxCell.PropComboBoxCellValueMemberProp))
				{
					base.Properties.SetObject(DataGridViewComboBoxCell.PropComboBoxCellValueMemberProp, value);
				}
			}
		}

		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x060034F5 RID: 13557 RVA: 0x000BCCA8 File Offset: 0x000BBCA8
		public override Type ValueType
		{
			get
			{
				if (this.ValueMemberProperty != null)
				{
					return this.ValueMemberProperty.PropertyType;
				}
				if (this.DisplayMemberProperty != null)
				{
					return this.DisplayMemberProperty.PropertyType;
				}
				Type valueType = base.ValueType;
				if (valueType != null)
				{
					return valueType;
				}
				return DataGridViewComboBoxCell.defaultValueType;
			}
		}

		// Token: 0x060034F6 RID: 13558 RVA: 0x000BCCEE File Offset: 0x000BBCEE
		internal override void CacheEditingControl()
		{
			this.EditingComboBox = base.DataGridView.EditingControl as DataGridViewComboBoxEditingControl;
		}

		// Token: 0x060034F7 RID: 13559 RVA: 0x000BCD08 File Offset: 0x000BBD08
		private void CheckDropDownList(int x, int y, int rowIndex)
		{
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle = new DataGridViewAdvancedBorderStyle();
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle2 = this.AdjustCellBorderStyle(base.DataGridView.AdvancedCellBorderStyle, dataGridViewAdvancedBorderStyle, false, false, false, false);
			DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, rowIndex, false);
			Rectangle rectangle = this.BorderWidths(dataGridViewAdvancedBorderStyle2);
			rectangle.X += inheritedStyle.Padding.Left;
			rectangle.Y += inheritedStyle.Padding.Top;
			rectangle.Width += inheritedStyle.Padding.Right;
			rectangle.Height += inheritedStyle.Padding.Bottom;
			Size size = this.GetSize(rowIndex);
			Size size2 = new Size(size.Width - rectangle.X - rectangle.Width, size.Height - rectangle.Y - rectangle.Height);
			int num;
			using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
			{
				num = Math.Min(this.GetDropDownButtonHeight(graphics, inheritedStyle), size2.Height - 2);
			}
			int num2 = Math.Min(SystemInformation.HorizontalScrollBarThumbWidth, size2.Width - 6 - 1);
			if (num > 0 && num2 > 0 && y >= rectangle.Y + 1 && y <= rectangle.Y + 1 + num)
			{
				if (base.DataGridView.RightToLeftInternal)
				{
					if (x >= rectangle.X + 1 && x <= rectangle.X + num2 + 1)
					{
						this.EditingComboBox.DroppedDown = true;
						return;
					}
				}
				else if (x >= size.Width - rectangle.Width - num2 - 1 && x <= size.Width - rectangle.Width - 1)
				{
					this.EditingComboBox.DroppedDown = true;
				}
			}
		}

		// Token: 0x060034F8 RID: 13560 RVA: 0x000BCEE4 File Offset: 0x000BBEE4
		private void CheckNoDataSource()
		{
			if (this.DataSource != null)
			{
				throw new ArgumentException(SR.GetString("DataSourceLocksItems"));
			}
		}

		// Token: 0x060034F9 RID: 13561 RVA: 0x000BCF00 File Offset: 0x000BBF00
		private void ComboBox_DropDown(object sender, EventArgs e)
		{
			ComboBox editingComboBox = this.EditingComboBox;
			DataGridViewComboBoxColumn dataGridViewComboBoxColumn = base.OwningColumn as DataGridViewComboBoxColumn;
			if (dataGridViewComboBoxColumn != null)
			{
				DataGridViewAutoSizeColumnMode inheritedAutoSizeMode = dataGridViewComboBoxColumn.GetInheritedAutoSizeMode(base.DataGridView);
				if (inheritedAutoSizeMode != DataGridViewAutoSizeColumnMode.ColumnHeader && inheritedAutoSizeMode != DataGridViewAutoSizeColumnMode.Fill && inheritedAutoSizeMode != DataGridViewAutoSizeColumnMode.None)
				{
					if (this.DropDownWidth == 1)
					{
						if (DataGridViewComboBoxCell.cachedDropDownWidth == -1)
						{
							int num = -1;
							if ((this.HasItems || this.CreateItemsFromDataSource) && this.Items.Count > 0)
							{
								foreach (object obj in this.Items)
								{
									Size size = TextRenderer.MeasureText(editingComboBox.GetItemText(obj), editingComboBox.Font);
									if (size.Width > num)
									{
										num = size.Width;
									}
								}
							}
							DataGridViewComboBoxCell.cachedDropDownWidth = num + 2 + SystemInformation.VerticalScrollBarWidth;
						}
						UnsafeNativeMethods.SendMessage(new HandleRef(editingComboBox, editingComboBox.Handle), 352, DataGridViewComboBoxCell.cachedDropDownWidth, 0);
						return;
					}
				}
				else
				{
					int num2 = (int)UnsafeNativeMethods.SendMessage(new HandleRef(editingComboBox, editingComboBox.Handle), 351, 0, 0);
					if (num2 != this.DropDownWidth)
					{
						UnsafeNativeMethods.SendMessage(new HandleRef(editingComboBox, editingComboBox.Handle), 352, this.DropDownWidth, 0);
					}
				}
			}
		}

		// Token: 0x060034FA RID: 13562 RVA: 0x000BD064 File Offset: 0x000BC064
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewComboBoxCell dataGridViewComboBoxCell;
			if (type == DataGridViewComboBoxCell.cellType)
			{
				dataGridViewComboBoxCell = new DataGridViewComboBoxCell();
			}
			else
			{
				dataGridViewComboBoxCell = (DataGridViewComboBoxCell)Activator.CreateInstance(type);
			}
			base.CloneInternal(dataGridViewComboBoxCell);
			dataGridViewComboBoxCell.DropDownWidth = this.DropDownWidth;
			dataGridViewComboBoxCell.MaxDropDownItems = this.MaxDropDownItems;
			dataGridViewComboBoxCell.CreateItemsFromDataSource = false;
			dataGridViewComboBoxCell.DataSource = this.DataSource;
			dataGridViewComboBoxCell.DisplayMember = this.DisplayMember;
			dataGridViewComboBoxCell.ValueMember = this.ValueMember;
			if (this.HasItems && this.DataSource == null && this.Items.Count > 0)
			{
				dataGridViewComboBoxCell.Items.AddRangeInternal(this.Items.InnerArray.ToArray());
			}
			dataGridViewComboBoxCell.AutoComplete = this.AutoComplete;
			dataGridViewComboBoxCell.Sorted = this.Sorted;
			dataGridViewComboBoxCell.FlatStyleInternal = this.FlatStyle;
			dataGridViewComboBoxCell.DisplayStyleInternal = this.DisplayStyle;
			dataGridViewComboBoxCell.DisplayStyleForCurrentCellOnlyInternal = this.DisplayStyleForCurrentCellOnly;
			return dataGridViewComboBoxCell;
		}

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x060034FB RID: 13563 RVA: 0x000BD154 File Offset: 0x000BC154
		// (set) Token: 0x060034FC RID: 13564 RVA: 0x000BD164 File Offset: 0x000BC164
		private bool CreateItemsFromDataSource
		{
			get
			{
				return (this.flags & 4) != 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 4;
					return;
				}
				this.flags = (byte)((int)this.flags & -5);
			}
		}

		// Token: 0x060034FD RID: 13565 RVA: 0x000BD189 File Offset: 0x000BC189
		private void DataSource_Disposed(object sender, EventArgs e)
		{
			this.DataSource = null;
		}

		// Token: 0x060034FE RID: 13566 RVA: 0x000BD194 File Offset: 0x000BC194
		private void DataSource_Initialized(object sender, EventArgs e)
		{
			ISupportInitializeNotification supportInitializeNotification = this.DataSource as ISupportInitializeNotification;
			if (supportInitializeNotification != null)
			{
				supportInitializeNotification.Initialized -= this.DataSource_Initialized;
			}
			this.flags = (byte)((int)this.flags & -17);
			this.InitializeDisplayMemberPropertyDescriptor(this.DisplayMember);
			this.InitializeValueMemberPropertyDescriptor(this.ValueMember);
		}

		// Token: 0x060034FF RID: 13567 RVA: 0x000BD1EC File Offset: 0x000BC1EC
		public override void DetachEditingControl()
		{
			DataGridView dataGridView = base.DataGridView;
			if (dataGridView == null || dataGridView.EditingControl == null)
			{
				throw new InvalidOperationException();
			}
			if (this.EditingComboBox != null && (this.flags & 32) != 0)
			{
				this.EditingComboBox.DropDown -= this.ComboBox_DropDown;
				this.flags = (byte)((int)this.flags & -33);
			}
			this.EditingComboBox = null;
			base.DetachEditingControl();
		}

		// Token: 0x06003500 RID: 13568 RVA: 0x000BD258 File Offset: 0x000BC258
		protected override Rectangle GetContentBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			if (base.DataGridView == null || rowIndex < 0 || base.OwningColumn == null)
			{
				return Rectangle.Empty;
			}
			object value = this.GetValue(rowIndex);
			object editedFormattedValue = base.GetEditedFormattedValue(value, rowIndex, ref cellStyle, DataGridViewDataErrorContexts.Formatting);
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle;
			DataGridViewElementStates dataGridViewElementStates;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out dataGridViewAdvancedBorderStyle, out dataGridViewElementStates, out rectangle);
			Rectangle rectangle2;
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, dataGridViewElementStates, editedFormattedValue, null, cellStyle, dataGridViewAdvancedBorderStyle, out rectangle2, DataGridViewPaintParts.ContentForeground, true, false, false, false);
		}

		// Token: 0x06003501 RID: 13569 RVA: 0x000BD2CC File Offset: 0x000BC2CC
		private CurrencyManager GetDataManager(DataGridView dataGridView)
		{
			CurrencyManager currencyManager = (CurrencyManager)base.Properties.GetObject(DataGridViewComboBoxCell.PropComboBoxCellDataManager);
			if (currencyManager == null && this.DataSource != null && dataGridView != null && dataGridView.BindingContext != null && this.DataSource != Convert.DBNull)
			{
				ISupportInitializeNotification supportInitializeNotification = this.DataSource as ISupportInitializeNotification;
				if (supportInitializeNotification != null && !supportInitializeNotification.IsInitialized)
				{
					if ((this.flags & 16) == 0)
					{
						supportInitializeNotification.Initialized += this.DataSource_Initialized;
						this.flags |= 16;
					}
				}
				else
				{
					currencyManager = (CurrencyManager)dataGridView.BindingContext[this.DataSource];
					this.DataManager = currencyManager;
				}
			}
			return currencyManager;
		}

		// Token: 0x06003502 RID: 13570 RVA: 0x000BD37C File Offset: 0x000BC37C
		private int GetDropDownButtonHeight(Graphics graphics, DataGridViewCellStyle cellStyle)
		{
			int num = 4;
			if (this.PaintXPThemes)
			{
				if (DataGridViewComboBoxCell.PostXPThemesExist)
				{
					num = 8;
				}
				else
				{
					num = 6;
				}
			}
			return DataGridViewCell.MeasureTextHeight(graphics, " ", cellStyle.Font, int.MaxValue, TextFormatFlags.Default) + num;
		}

		// Token: 0x06003503 RID: 13571 RVA: 0x000BD3BC File Offset: 0x000BC3BC
		protected override Rectangle GetErrorIconBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			if (base.DataGridView == null || rowIndex < 0 || base.OwningColumn == null || !base.DataGridView.ShowCellErrors || string.IsNullOrEmpty(this.GetErrorText(rowIndex)))
			{
				return Rectangle.Empty;
			}
			object value = this.GetValue(rowIndex);
			object editedFormattedValue = base.GetEditedFormattedValue(value, rowIndex, ref cellStyle, DataGridViewDataErrorContexts.Formatting);
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle;
			DataGridViewElementStates dataGridViewElementStates;
			Rectangle rectangle;
			base.ComputeBorderStyleCellStateAndCellBounds(rowIndex, out dataGridViewAdvancedBorderStyle, out dataGridViewElementStates, out rectangle);
			Rectangle rectangle2;
			return this.PaintPrivate(graphics, rectangle, rectangle, rowIndex, dataGridViewElementStates, editedFormattedValue, this.GetErrorText(rowIndex), cellStyle, dataGridViewAdvancedBorderStyle, out rectangle2, DataGridViewPaintParts.ContentForeground, false, true, false, false);
		}

		// Token: 0x06003504 RID: 13572 RVA: 0x000BD450 File Offset: 0x000BC450
		protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
		{
			if (valueTypeConverter == null)
			{
				if (this.ValueMemberProperty != null)
				{
					valueTypeConverter = this.ValueMemberProperty.Converter;
				}
				else if (this.DisplayMemberProperty != null)
				{
					valueTypeConverter = this.DisplayMemberProperty.Converter;
				}
			}
			if (value == null || (this.ValueType != null && !this.ValueType.IsAssignableFrom(value.GetType()) && value != DBNull.Value))
			{
				if (value == null)
				{
					return base.GetFormattedValue(null, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
				}
				if (base.DataGridView != null)
				{
					DataGridViewDataErrorEventArgs dataGridViewDataErrorEventArgs = new DataGridViewDataErrorEventArgs(new FormatException(SR.GetString("DataGridViewComboBoxCell_InvalidValue")), base.ColumnIndex, rowIndex, context);
					base.RaiseDataError(dataGridViewDataErrorEventArgs);
					if (dataGridViewDataErrorEventArgs.ThrowException)
					{
						throw dataGridViewDataErrorEventArgs.Exception;
					}
				}
				return base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
			}
			else
			{
				string text = value as string;
				if ((this.DataManager != null && (this.ValueMemberProperty != null || this.DisplayMemberProperty != null)) || !string.IsNullOrEmpty(this.ValueMember) || !string.IsNullOrEmpty(this.DisplayMember))
				{
					object obj;
					if (!this.LookupDisplayValue(rowIndex, value, out obj))
					{
						if (value == DBNull.Value)
						{
							obj = DBNull.Value;
						}
						else if (text != null && string.IsNullOrEmpty(text) && this.DisplayType == typeof(string))
						{
							obj = string.Empty;
						}
						else if (base.DataGridView != null)
						{
							DataGridViewDataErrorEventArgs dataGridViewDataErrorEventArgs2 = new DataGridViewDataErrorEventArgs(new ArgumentException(SR.GetString("DataGridViewComboBoxCell_InvalidValue")), base.ColumnIndex, rowIndex, context);
							base.RaiseDataError(dataGridViewDataErrorEventArgs2);
							if (dataGridViewDataErrorEventArgs2.ThrowException)
							{
								throw dataGridViewDataErrorEventArgs2.Exception;
							}
							if (this.OwnsEditingComboBox(rowIndex))
							{
								((IDataGridViewEditingControl)this.EditingComboBox).EditingControlValueChanged = true;
								base.DataGridView.NotifyCurrentCellDirty(true);
							}
						}
					}
					return base.GetFormattedValue(obj, rowIndex, ref cellStyle, this.DisplayTypeConverter, formattedValueTypeConverter, context);
				}
				if (!this.Items.Contains(value) && value != DBNull.Value && (!(value is string) || !string.IsNullOrEmpty(text)))
				{
					if (base.DataGridView != null)
					{
						DataGridViewDataErrorEventArgs dataGridViewDataErrorEventArgs3 = new DataGridViewDataErrorEventArgs(new ArgumentException(SR.GetString("DataGridViewComboBoxCell_InvalidValue")), base.ColumnIndex, rowIndex, context);
						base.RaiseDataError(dataGridViewDataErrorEventArgs3);
						if (dataGridViewDataErrorEventArgs3.ThrowException)
						{
							throw dataGridViewDataErrorEventArgs3.Exception;
						}
					}
					if (this.Items.Count > 0)
					{
						value = this.Items[0];
					}
					else
					{
						value = string.Empty;
					}
				}
				return base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
			}
		}

		// Token: 0x06003505 RID: 13573 RVA: 0x000BD6A0 File Offset: 0x000BC6A0
		internal string GetItemDisplayText(object item)
		{
			object itemDisplayValue = this.GetItemDisplayValue(item);
			if (itemDisplayValue == null)
			{
				return string.Empty;
			}
			return Convert.ToString(itemDisplayValue, CultureInfo.CurrentCulture);
		}

		// Token: 0x06003506 RID: 13574 RVA: 0x000BD6CC File Offset: 0x000BC6CC
		internal object GetItemDisplayValue(object item)
		{
			bool flag = false;
			object obj = null;
			if (this.DisplayMemberProperty != null)
			{
				obj = this.DisplayMemberProperty.GetValue(item);
				flag = true;
			}
			else if (this.ValueMemberProperty != null)
			{
				obj = this.ValueMemberProperty.GetValue(item);
				flag = true;
			}
			else if (!string.IsNullOrEmpty(this.DisplayMember))
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(item).Find(this.DisplayMember, true);
				if (propertyDescriptor != null)
				{
					obj = propertyDescriptor.GetValue(item);
					flag = true;
				}
			}
			else if (!string.IsNullOrEmpty(this.ValueMember))
			{
				PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(item).Find(this.ValueMember, true);
				if (propertyDescriptor2 != null)
				{
					obj = propertyDescriptor2.GetValue(item);
					flag = true;
				}
			}
			if (!flag)
			{
				obj = item;
			}
			return obj;
		}

		// Token: 0x06003507 RID: 13575 RVA: 0x000BD774 File Offset: 0x000BC774
		internal DataGridViewComboBoxCell.ObjectCollection GetItems(DataGridView dataGridView)
		{
			DataGridViewComboBoxCell.ObjectCollection objectCollection = (DataGridViewComboBoxCell.ObjectCollection)base.Properties.GetObject(DataGridViewComboBoxCell.PropComboBoxCellItems);
			if (objectCollection == null)
			{
				objectCollection = new DataGridViewComboBoxCell.ObjectCollection(this);
				base.Properties.SetObject(DataGridViewComboBoxCell.PropComboBoxCellItems, objectCollection);
			}
			if (this.CreateItemsFromDataSource)
			{
				objectCollection.ClearInternal();
				CurrencyManager dataManager = this.GetDataManager(dataGridView);
				if (dataManager != null && dataManager.Count != -1)
				{
					object[] array = new object[dataManager.Count];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = dataManager[i];
					}
					objectCollection.AddRangeInternal(array);
				}
				if (dataManager != null || (this.flags & 16) == 0)
				{
					this.CreateItemsFromDataSource = false;
				}
			}
			return objectCollection;
		}

		// Token: 0x06003508 RID: 13576 RVA: 0x000BD818 File Offset: 0x000BC818
		internal object GetItemValue(object item)
		{
			bool flag = false;
			object obj = null;
			if (this.ValueMemberProperty != null)
			{
				obj = this.ValueMemberProperty.GetValue(item);
				flag = true;
			}
			else if (this.DisplayMemberProperty != null)
			{
				obj = this.DisplayMemberProperty.GetValue(item);
				flag = true;
			}
			else if (!string.IsNullOrEmpty(this.ValueMember))
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(item).Find(this.ValueMember, true);
				if (propertyDescriptor != null)
				{
					obj = propertyDescriptor.GetValue(item);
					flag = true;
				}
			}
			if (!flag && !string.IsNullOrEmpty(this.DisplayMember))
			{
				PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(item).Find(this.DisplayMember, true);
				if (propertyDescriptor2 != null)
				{
					obj = propertyDescriptor2.GetValue(item);
					flag = true;
				}
			}
			if (!flag)
			{
				obj = item;
			}
			return obj;
		}

		// Token: 0x06003509 RID: 13577 RVA: 0x000BD8C0 File Offset: 0x000BC8C0
		protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
		{
			if (base.DataGridView == null)
			{
				return new Size(-1, -1);
			}
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			Size size = Size.Empty;
			DataGridViewFreeDimension freeDimensionFromConstraint = DataGridViewCell.GetFreeDimensionFromConstraint(constraintSize);
			Rectangle stdBorderWidths = base.StdBorderWidths;
			int num = stdBorderWidths.Left + stdBorderWidths.Width + cellStyle.Padding.Horizontal;
			int num2 = stdBorderWidths.Top + stdBorderWidths.Height + cellStyle.Padding.Vertical;
			TextFormatFlags textFormatFlags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
			string text = base.GetFormattedValue(rowIndex, ref cellStyle, DataGridViewDataErrorContexts.Formatting | DataGridViewDataErrorContexts.PreferredSize) as string;
			if (!string.IsNullOrEmpty(text))
			{
				size = DataGridViewCell.MeasureTextSize(graphics, text, cellStyle.Font, textFormatFlags);
			}
			else
			{
				size = DataGridViewCell.MeasureTextSize(graphics, " ", cellStyle.Font, textFormatFlags);
			}
			if (freeDimensionFromConstraint == DataGridViewFreeDimension.Height)
			{
				size.Width = 0;
			}
			else if (freeDimensionFromConstraint == DataGridViewFreeDimension.Width)
			{
				size.Height = 0;
			}
			if (freeDimensionFromConstraint != DataGridViewFreeDimension.Height)
			{
				size.Width += SystemInformation.HorizontalScrollBarThumbWidth + 1 + 6 + num;
				if (base.DataGridView.ShowCellErrors)
				{
					size.Width = Math.Max(size.Width, num + SystemInformation.HorizontalScrollBarThumbWidth + 1 + 8 + 12);
				}
			}
			if (freeDimensionFromConstraint != DataGridViewFreeDimension.Width)
			{
				if (this.FlatStyle == FlatStyle.Flat || this.FlatStyle == FlatStyle.Popup)
				{
					size.Height += 6;
				}
				else
				{
					size.Height += 8;
				}
				size.Height += num2;
				if (base.DataGridView.ShowCellErrors)
				{
					size.Height = Math.Max(size.Height, num2 + 8 + 11);
				}
			}
			return size;
		}

		// Token: 0x0600350A RID: 13578 RVA: 0x000BDA74 File Offset: 0x000BCA74
		private void InitializeComboBoxText()
		{
			((IDataGridViewEditingControl)this.EditingComboBox).EditingControlValueChanged = false;
			int editingControlRowIndex = ((IDataGridViewEditingControl)this.EditingComboBox).EditingControlRowIndex;
			DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, editingControlRowIndex, false);
			this.EditingComboBox.Text = (string)this.GetFormattedValue(this.GetValue(editingControlRowIndex), editingControlRowIndex, ref inheritedStyle, null, null, DataGridViewDataErrorContexts.Formatting);
		}

		// Token: 0x0600350B RID: 13579 RVA: 0x000BDAC8 File Offset: 0x000BCAC8
		public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
		{
			base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
			ComboBox comboBox = base.DataGridView.EditingControl as ComboBox;
			if (comboBox != null)
			{
				if ((this.GetInheritedState(rowIndex) & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
				{
					base.DataGridView.EditingPanel.BackColor = dataGridViewCellStyle.SelectionBackColor;
				}
				if (comboBox.ParentInternal != null)
				{
					IntPtr handle = comboBox.ParentInternal.Handle;
				}
				IntPtr handle2 = comboBox.Handle;
				comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
				comboBox.FormattingEnabled = true;
				comboBox.MaxDropDownItems = this.MaxDropDownItems;
				comboBox.DropDownWidth = this.DropDownWidth;
				comboBox.DataSource = null;
				comboBox.ValueMember = null;
				comboBox.Items.Clear();
				comboBox.DataSource = this.DataSource;
				comboBox.DisplayMember = this.DisplayMember;
				comboBox.ValueMember = this.ValueMember;
				if (this.HasItems && this.DataSource == null && this.Items.Count > 0)
				{
					comboBox.Items.AddRange(this.Items.InnerArray.ToArray());
				}
				comboBox.Sorted = this.Sorted;
				comboBox.FlatStyle = this.FlatStyle;
				if (this.AutoComplete)
				{
					comboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
					comboBox.AutoCompleteMode = AutoCompleteMode.Append;
				}
				else
				{
					comboBox.AutoCompleteMode = AutoCompleteMode.None;
					comboBox.AutoCompleteSource = AutoCompleteSource.None;
				}
				string text = initialFormattedValue as string;
				if (text == null)
				{
					text = string.Empty;
				}
				comboBox.Text = text;
				if ((this.flags & 32) == 0)
				{
					comboBox.DropDown += this.ComboBox_DropDown;
					this.flags |= 32;
				}
				DataGridViewComboBoxCell.cachedDropDownWidth = -1;
				this.EditingComboBox = base.DataGridView.EditingControl as DataGridViewComboBoxEditingControl;
				if (base.GetHeight(rowIndex) > 21)
				{
					Rectangle cellDisplayRectangle = base.DataGridView.GetCellDisplayRectangle(base.ColumnIndex, rowIndex, true);
					cellDisplayRectangle.Y += 21;
					cellDisplayRectangle.Height -= 21;
					base.DataGridView.Invalidate(cellDisplayRectangle);
				}
			}
		}

		// Token: 0x0600350C RID: 13580 RVA: 0x000BDCC4 File Offset: 0x000BCCC4
		private void InitializeDisplayMemberPropertyDescriptor(string displayMember)
		{
			if (this.DataManager != null)
			{
				if (string.IsNullOrEmpty(displayMember))
				{
					this.DisplayMemberProperty = null;
					return;
				}
				BindingMemberInfo bindingMemberInfo = new BindingMemberInfo(displayMember);
				this.DataManager = base.DataGridView.BindingContext[this.DataSource, bindingMemberInfo.BindingPath] as CurrencyManager;
				PropertyDescriptorCollection itemProperties = this.DataManager.GetItemProperties();
				PropertyDescriptor propertyDescriptor = itemProperties.Find(bindingMemberInfo.BindingField, true);
				if (propertyDescriptor == null)
				{
					throw new ArgumentException(SR.GetString("DataGridViewComboBoxCell_FieldNotFound", new object[] { displayMember }));
				}
				this.DisplayMemberProperty = propertyDescriptor;
			}
		}

		// Token: 0x0600350D RID: 13581 RVA: 0x000BDD60 File Offset: 0x000BCD60
		private void InitializeValueMemberPropertyDescriptor(string valueMember)
		{
			if (this.DataManager != null)
			{
				if (string.IsNullOrEmpty(valueMember))
				{
					this.ValueMemberProperty = null;
					return;
				}
				BindingMemberInfo bindingMemberInfo = new BindingMemberInfo(valueMember);
				this.DataManager = base.DataGridView.BindingContext[this.DataSource, bindingMemberInfo.BindingPath] as CurrencyManager;
				PropertyDescriptorCollection itemProperties = this.DataManager.GetItemProperties();
				PropertyDescriptor propertyDescriptor = itemProperties.Find(bindingMemberInfo.BindingField, true);
				if (propertyDescriptor == null)
				{
					throw new ArgumentException(SR.GetString("DataGridViewComboBoxCell_FieldNotFound", new object[] { valueMember }));
				}
				this.ValueMemberProperty = propertyDescriptor;
			}
		}

		// Token: 0x0600350E RID: 13582 RVA: 0x000BDDFC File Offset: 0x000BCDFC
		private object ItemFromComboBoxDataSource(PropertyDescriptor property, object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			object obj = null;
			if (this.DataManager.List is IBindingList && ((IBindingList)this.DataManager.List).SupportsSearching)
			{
				int num = ((IBindingList)this.DataManager.List).Find(property, key);
				if (num != -1)
				{
					obj = this.DataManager.List[num];
				}
			}
			else
			{
				for (int i = 0; i < this.DataManager.List.Count; i++)
				{
					object obj2 = this.DataManager.List[i];
					object value = property.GetValue(obj2);
					if (key.Equals(value))
					{
						obj = obj2;
						break;
					}
				}
			}
			return obj;
		}

		// Token: 0x0600350F RID: 13583 RVA: 0x000BDEB8 File Offset: 0x000BCEB8
		private object ItemFromComboBoxItems(int rowIndex, string field, object key)
		{
			object obj = null;
			if (this.OwnsEditingComboBox(rowIndex))
			{
				obj = this.EditingComboBox.SelectedItem;
				object obj2 = null;
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(obj).Find(field, true);
				if (propertyDescriptor != null)
				{
					obj2 = propertyDescriptor.GetValue(obj);
				}
				if (obj2 == null || !obj2.Equals(key))
				{
					obj = null;
				}
			}
			if (obj == null)
			{
				foreach (object obj3 in this.Items)
				{
					object obj4 = null;
					PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(obj3).Find(field, true);
					if (propertyDescriptor2 != null)
					{
						obj4 = propertyDescriptor2.GetValue(obj3);
					}
					if (obj4 != null && obj4.Equals(key))
					{
						obj = obj3;
						break;
					}
				}
			}
			if (obj == null)
			{
				if (this.OwnsEditingComboBox(rowIndex))
				{
					obj = this.EditingComboBox.SelectedItem;
					if (obj == null || !obj.Equals(key))
					{
						obj = null;
					}
				}
				if (obj == null && this.Items.Contains(key))
				{
					obj = key;
				}
			}
			return obj;
		}

		// Token: 0x06003510 RID: 13584 RVA: 0x000BDFBC File Offset: 0x000BCFBC
		public override bool KeyEntersEditMode(KeyEventArgs e)
		{
			return (((char.IsLetterOrDigit((char)e.KeyCode) && (e.KeyCode < Keys.F1 || e.KeyCode > Keys.F24)) || (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.Divide) || (e.KeyCode >= Keys.OemSemicolon && e.KeyCode <= Keys.OemBackslash) || (e.KeyCode == Keys.Space && !e.Shift) || e.KeyCode == Keys.F4 || ((e.KeyCode == Keys.Down || e.KeyCode == Keys.Up) && e.Alt)) && (!e.Alt || e.KeyCode == Keys.Down || e.KeyCode == Keys.Up) && !e.Control) || base.KeyEntersEditMode(e);
		}

		// Token: 0x06003511 RID: 13585 RVA: 0x000BE084 File Offset: 0x000BD084
		private bool LookupDisplayValue(int rowIndex, object value, out object displayValue)
		{
			object obj;
			if (this.DisplayMemberProperty != null || this.ValueMemberProperty != null)
			{
				obj = this.ItemFromComboBoxDataSource((this.ValueMemberProperty != null) ? this.ValueMemberProperty : this.DisplayMemberProperty, value);
			}
			else
			{
				obj = this.ItemFromComboBoxItems(rowIndex, string.IsNullOrEmpty(this.ValueMember) ? this.DisplayMember : this.ValueMember, value);
			}
			if (obj == null)
			{
				displayValue = null;
				return false;
			}
			displayValue = this.GetItemDisplayValue(obj);
			return true;
		}

		// Token: 0x06003512 RID: 13586 RVA: 0x000BE0FC File Offset: 0x000BD0FC
		private bool LookupValue(object formattedValue, out object value)
		{
			if (formattedValue == null)
			{
				value = null;
				return true;
			}
			object obj;
			if (this.DisplayMemberProperty != null || this.ValueMemberProperty != null)
			{
				obj = this.ItemFromComboBoxDataSource((this.DisplayMemberProperty != null) ? this.DisplayMemberProperty : this.ValueMemberProperty, formattedValue);
			}
			else
			{
				obj = this.ItemFromComboBoxItems(base.RowIndex, string.IsNullOrEmpty(this.DisplayMember) ? this.ValueMember : this.DisplayMember, formattedValue);
			}
			if (obj == null)
			{
				value = null;
				return false;
			}
			value = this.GetItemValue(obj);
			return true;
		}

		// Token: 0x06003513 RID: 13587 RVA: 0x000BE17E File Offset: 0x000BD17E
		protected override void OnDataGridViewChanged()
		{
			if (base.DataGridView != null)
			{
				this.InitializeDisplayMemberPropertyDescriptor(this.DisplayMember);
				this.InitializeValueMemberPropertyDescriptor(this.ValueMember);
			}
			base.OnDataGridViewChanged();
		}

		// Token: 0x06003514 RID: 13588 RVA: 0x000BE1A6 File Offset: 0x000BD1A6
		protected override void OnEnter(int rowIndex, bool throughMouseClick)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (throughMouseClick && base.DataGridView.EditMode != DataGridViewEditMode.EditOnEnter)
			{
				this.flags |= 1;
			}
		}

		// Token: 0x06003515 RID: 13589 RVA: 0x000BE1D0 File Offset: 0x000BD1D0
		private void OnItemsCollectionChanged()
		{
			if (this.TemplateComboBoxColumn != null)
			{
				this.TemplateComboBoxColumn.OnItemsCollectionChanged();
			}
			DataGridViewComboBoxCell.cachedDropDownWidth = -1;
			if (this.OwnsEditingComboBox(base.RowIndex))
			{
				this.InitializeComboBoxText();
				return;
			}
			base.OnCommonChange();
		}

		// Token: 0x06003516 RID: 13590 RVA: 0x000BE206 File Offset: 0x000BD206
		protected override void OnLeave(int rowIndex, bool throughMouseClick)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			this.flags = (byte)((int)this.flags & -2);
		}

		// Token: 0x06003517 RID: 13591 RVA: 0x000BE224 File Offset: 0x000BD224
		protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			Point currentCellAddress = base.DataGridView.CurrentCellAddress;
			if (currentCellAddress.X == e.ColumnIndex && currentCellAddress.Y == e.RowIndex)
			{
				if ((this.flags & 1) != 0)
				{
					this.flags = (byte)((int)this.flags & -2);
					return;
				}
				if ((this.EditingComboBox == null || !this.EditingComboBox.DroppedDown) && base.DataGridView.EditMode != DataGridViewEditMode.EditProgrammatically && base.DataGridView.BeginEdit(true) && this.EditingComboBox != null && this.DisplayStyle != DataGridViewComboBoxDisplayStyle.Nothing)
				{
					this.CheckDropDownList(e.X, e.Y, e.RowIndex);
				}
			}
		}

		// Token: 0x06003518 RID: 13592 RVA: 0x000BE2DC File Offset: 0x000BD2DC
		protected override void OnMouseEnter(int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (this.DisplayStyle == DataGridViewComboBoxDisplayStyle.ComboBox && this.FlatStyle == FlatStyle.Popup)
			{
				base.DataGridView.InvalidateCell(base.ColumnIndex, rowIndex);
			}
			base.OnMouseEnter(rowIndex);
		}

		// Token: 0x06003519 RID: 13593 RVA: 0x000BE314 File Offset: 0x000BD314
		protected override void OnMouseLeave(int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if (DataGridViewComboBoxCell.mouseInDropDownButtonBounds)
			{
				DataGridViewComboBoxCell.mouseInDropDownButtonBounds = false;
				if (base.ColumnIndex >= 0 && rowIndex >= 0 && (this.FlatStyle == FlatStyle.Standard || this.FlatStyle == FlatStyle.System) && base.DataGridView.ApplyVisualStylesToInnerCells)
				{
					base.DataGridView.InvalidateCell(base.ColumnIndex, rowIndex);
				}
			}
			if (this.DisplayStyle == DataGridViewComboBoxDisplayStyle.ComboBox && this.FlatStyle == FlatStyle.Popup)
			{
				base.DataGridView.InvalidateCell(base.ColumnIndex, rowIndex);
			}
			base.OnMouseEnter(rowIndex);
		}

		// Token: 0x0600351A RID: 13594 RVA: 0x000BE3A0 File Offset: 0x000BD3A0
		protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
		{
			if (base.DataGridView == null)
			{
				return;
			}
			if ((this.FlatStyle == FlatStyle.Standard || this.FlatStyle == FlatStyle.System) && base.DataGridView.ApplyVisualStylesToInnerCells)
			{
				int rowIndex = e.RowIndex;
				DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, rowIndex, false);
				bool flag = !base.DataGridView.RowHeadersVisible && base.DataGridView.AdvancedCellBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Single;
				bool flag2 = !base.DataGridView.ColumnHeadersVisible && base.DataGridView.AdvancedCellBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Single;
				bool flag3 = rowIndex == base.DataGridView.FirstDisplayedScrollingRowIndex;
				bool flag4 = base.OwningColumn.Index == base.DataGridView.FirstDisplayedColumnIndex;
				bool flag5 = base.OwningColumn.Index == base.DataGridView.FirstDisplayedScrollingColumnIndex;
				DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle = new DataGridViewAdvancedBorderStyle();
				DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle2 = this.AdjustCellBorderStyle(base.DataGridView.AdvancedCellBorderStyle, dataGridViewAdvancedBorderStyle, flag, flag2, flag3, flag4);
				Rectangle cellDisplayRectangle = base.DataGridView.GetCellDisplayRectangle(base.OwningColumn.Index, rowIndex, false);
				if (flag5)
				{
					cellDisplayRectangle.X -= base.DataGridView.FirstDisplayedScrollingColumnHiddenWidth;
					cellDisplayRectangle.Width += base.DataGridView.FirstDisplayedScrollingColumnHiddenWidth;
				}
				DataGridViewElementStates rowState = base.DataGridView.Rows.GetRowState(rowIndex);
				DataGridViewElementStates dataGridViewElementStates = base.CellStateFromColumnRowStates(rowState);
				dataGridViewElementStates |= this.State;
				Rectangle rectangle;
				using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
				{
					this.PaintPrivate(graphics, cellDisplayRectangle, cellDisplayRectangle, rowIndex, dataGridViewElementStates, null, null, inheritedStyle, dataGridViewAdvancedBorderStyle2, out rectangle, DataGridViewPaintParts.ContentForeground, false, false, true, false);
				}
				bool flag6 = rectangle.Contains(base.DataGridView.PointToClient(Control.MousePosition));
				if (flag6 != DataGridViewComboBoxCell.mouseInDropDownButtonBounds)
				{
					DataGridViewComboBoxCell.mouseInDropDownButtonBounds = flag6;
					base.DataGridView.InvalidateCell(e.ColumnIndex, rowIndex);
				}
			}
			base.OnMouseMove(e);
		}

		// Token: 0x0600351B RID: 13595 RVA: 0x000BE598 File Offset: 0x000BD598
		private bool OwnsEditingComboBox(int rowIndex)
		{
			return rowIndex != -1 && this.EditingComboBox != null && rowIndex == ((IDataGridViewEditingControl)this.EditingComboBox).EditingControlRowIndex;
		}

		// Token: 0x0600351C RID: 13596 RVA: 0x000BE5B8 File Offset: 0x000BD5B8
		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			Rectangle rectangle;
			this.PaintPrivate(graphics, clipBounds, cellBounds, rowIndex, elementState, formattedValue, errorText, cellStyle, advancedBorderStyle, out rectangle, paintParts, false, false, false, true);
		}

		// Token: 0x0600351D RID: 13597 RVA: 0x000BE5F4 File Offset: 0x000BD5F4
		private Rectangle PaintPrivate(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, out Rectangle dropDownButtonRect, DataGridViewPaintParts paintParts, bool computeContentBounds, bool computeErrorIconBounds, bool computeDropDownButtonRect, bool paint)
		{
			Rectangle rectangle = Rectangle.Empty;
			dropDownButtonRect = Rectangle.Empty;
			bool flag = this.FlatStyle == FlatStyle.Flat || this.FlatStyle == FlatStyle.Popup;
			bool flag2 = this.FlatStyle == FlatStyle.Popup && base.DataGridView.MouseEnteredCellAddress.Y == rowIndex && base.DataGridView.MouseEnteredCellAddress.X == base.ColumnIndex;
			bool flag3 = !flag && base.DataGridView.ApplyVisualStylesToInnerCells;
			bool flag4 = flag3 && DataGridViewComboBoxCell.PostXPThemesExist;
			ComboBoxState comboBoxState = ComboBoxState.Normal;
			if (base.DataGridView.MouseEnteredCellAddress.Y == rowIndex && base.DataGridView.MouseEnteredCellAddress.X == base.ColumnIndex && DataGridViewComboBoxCell.mouseInDropDownButtonBounds)
			{
				comboBoxState = ComboBoxState.Hot;
			}
			if (paint && DataGridViewCell.PaintBorder(paintParts))
			{
				this.PaintBorder(g, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
			}
			Rectangle rectangle2 = this.BorderWidths(advancedBorderStyle);
			Rectangle rectangle3 = cellBounds;
			rectangle3.Offset(rectangle2.X, rectangle2.Y);
			rectangle3.Width -= rectangle2.Right;
			rectangle3.Height -= rectangle2.Bottom;
			Point currentCellAddress = base.DataGridView.CurrentCellAddress;
			bool flag5 = currentCellAddress.X == base.ColumnIndex && currentCellAddress.Y == rowIndex;
			bool flag6 = flag5 && base.DataGridView.EditingControl != null;
			bool flag7 = (elementState & DataGridViewElementStates.Selected) != DataGridViewElementStates.None;
			bool flag8 = this.DisplayStyle == DataGridViewComboBoxDisplayStyle.ComboBox && ((this.DisplayStyleForCurrentCellOnly && flag5) || !this.DisplayStyleForCurrentCellOnly);
			bool flag9 = this.DisplayStyle != DataGridViewComboBoxDisplayStyle.Nothing && ((this.DisplayStyleForCurrentCellOnly && flag5) || !this.DisplayStyleForCurrentCellOnly);
			SolidBrush solidBrush;
			if (DataGridViewCell.PaintSelectionBackground(paintParts) && flag7 && !flag6)
			{
				solidBrush = base.DataGridView.GetCachedBrush(cellStyle.SelectionBackColor);
			}
			else
			{
				solidBrush = base.DataGridView.GetCachedBrush(cellStyle.BackColor);
			}
			if (paint && DataGridViewCell.PaintBackground(paintParts) && solidBrush.Color.A == 255 && rectangle3.Width > 0 && rectangle3.Height > 0)
			{
				DataGridViewCell.PaintPadding(g, rectangle3, cellStyle, solidBrush, base.DataGridView.RightToLeftInternal);
			}
			if (cellStyle.Padding != Padding.Empty)
			{
				if (base.DataGridView.RightToLeftInternal)
				{
					rectangle3.Offset(cellStyle.Padding.Right, cellStyle.Padding.Top);
				}
				else
				{
					rectangle3.Offset(cellStyle.Padding.Left, cellStyle.Padding.Top);
				}
				rectangle3.Width -= cellStyle.Padding.Horizontal;
				rectangle3.Height -= cellStyle.Padding.Vertical;
			}
			if (paint && rectangle3.Width > 0 && rectangle3.Height > 0)
			{
				if (flag3 && flag8)
				{
					if (flag4 && DataGridViewCell.PaintBackground(paintParts) && solidBrush.Color.A == 255)
					{
						g.FillRectangle(solidBrush, rectangle3.Left, rectangle3.Top, rectangle3.Width, rectangle3.Height);
					}
					if (DataGridViewCell.PaintContentBackground(paintParts))
					{
						if (flag4)
						{
							DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.DrawBorder(g, rectangle3);
						}
						else
						{
							DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.DrawTextBox(g, rectangle3, comboBoxState);
						}
					}
					if (!flag4 && DataGridViewCell.PaintBackground(paintParts) && solidBrush.Color.A == 255 && rectangle3.Width > 2 && rectangle3.Height > 2)
					{
						g.FillRectangle(solidBrush, rectangle3.Left + 1, rectangle3.Top + 1, rectangle3.Width - 2, rectangle3.Height - 2);
					}
				}
				else if (DataGridViewCell.PaintBackground(paintParts) && solidBrush.Color.A == 255)
				{
					if (flag4 && flag9 && !flag8)
					{
						g.DrawRectangle(SystemPens.ControlLightLight, new Rectangle(rectangle3.X, rectangle3.Y, rectangle3.Width - 1, rectangle3.Height - 1));
					}
					else
					{
						g.FillRectangle(solidBrush, rectangle3.Left, rectangle3.Top, rectangle3.Width, rectangle3.Height);
					}
				}
			}
			int num = Math.Min(SystemInformation.HorizontalScrollBarThumbWidth, rectangle3.Width - 6 - 1);
			if (!flag6)
			{
				int num2;
				if (flag3 || flag)
				{
					num2 = Math.Min(this.GetDropDownButtonHeight(g, cellStyle), flag4 ? rectangle3.Height : (rectangle3.Height - 2));
				}
				else
				{
					num2 = Math.Min(this.GetDropDownButtonHeight(g, cellStyle), rectangle3.Height - 4);
				}
				if (num > 0 && num2 > 0)
				{
					Rectangle rectangle4;
					if (flag3 || flag)
					{
						if (flag4)
						{
							rectangle4 = new Rectangle(base.DataGridView.RightToLeftInternal ? rectangle3.Left : (rectangle3.Right - num), rectangle3.Top, num, num2);
						}
						else
						{
							rectangle4 = new Rectangle(base.DataGridView.RightToLeftInternal ? (rectangle3.Left + 1) : (rectangle3.Right - num - 1), rectangle3.Top + 1, num, num2);
						}
					}
					else
					{
						rectangle4 = new Rectangle(base.DataGridView.RightToLeftInternal ? (rectangle3.Left + 2) : (rectangle3.Right - num - 2), rectangle3.Top + 2, num, num2);
					}
					if (flag4 && flag9 && !flag8)
					{
						dropDownButtonRect = rectangle3;
					}
					else
					{
						dropDownButtonRect = rectangle4;
					}
					if (paint && DataGridViewCell.PaintContentBackground(paintParts))
					{
						if (flag9)
						{
							if (flag)
							{
								g.FillRectangle(SystemBrushes.Control, rectangle4);
							}
							else if (flag3)
							{
								if (flag4)
								{
									if (flag8)
									{
										DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.DrawDropDownButton(g, rectangle4, comboBoxState, base.DataGridView.RightToLeftInternal);
									}
									else
									{
										DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.DrawReadOnlyButton(g, rectangle3, comboBoxState);
										DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.DrawDropDownButton(g, rectangle4, ComboBoxState.Normal);
									}
								}
								else
								{
									DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.DrawDropDownButton(g, rectangle4, comboBoxState);
								}
							}
							else
							{
								g.FillRectangle(SystemBrushes.Control, rectangle4);
							}
						}
						if (!flag && !flag3 && (flag8 || flag9))
						{
							Color control = SystemColors.Control;
							Color color = control;
							bool flag10 = control.ToKnownColor() == SystemColors.Control.ToKnownColor();
							bool highContrast = SystemInformation.HighContrast;
							Color color2;
							Color color3;
							Color color4;
							if (control == SystemColors.Control)
							{
								color2 = SystemColors.ControlDark;
								color3 = SystemColors.ControlDarkDark;
								color4 = SystemColors.ControlLightLight;
							}
							else
							{
								color2 = ControlPaint.Dark(control);
								color4 = ControlPaint.LightLight(control);
								if (highContrast)
								{
									color3 = ControlPaint.LightLight(control);
								}
								else
								{
									color3 = ControlPaint.DarkDark(control);
								}
							}
							color2 = g.GetNearestColor(color2);
							color3 = g.GetNearestColor(color3);
							color = g.GetNearestColor(color);
							color4 = g.GetNearestColor(color4);
							Pen pen;
							if (flag10)
							{
								if (SystemInformation.HighContrast)
								{
									pen = SystemPens.ControlLight;
								}
								else
								{
									pen = SystemPens.Control;
								}
							}
							else
							{
								pen = new Pen(color4);
							}
							if (flag9)
							{
								g.DrawLine(pen, rectangle4.X, rectangle4.Y, rectangle4.X + rectangle4.Width - 1, rectangle4.Y);
								g.DrawLine(pen, rectangle4.X, rectangle4.Y, rectangle4.X, rectangle4.Y + rectangle4.Height - 1);
							}
							if (flag8)
							{
								g.DrawLine(pen, rectangle3.X, rectangle3.Y + rectangle3.Height - 1, rectangle3.X + rectangle3.Width - 1, rectangle3.Y + rectangle3.Height - 1);
								g.DrawLine(pen, rectangle3.X + rectangle3.Width - 1, rectangle3.Y, rectangle3.X + rectangle3.Width - 1, rectangle3.Y + rectangle3.Height - 1);
							}
							if (flag10)
							{
								pen = SystemPens.ControlDarkDark;
							}
							else
							{
								pen.Color = color3;
							}
							if (flag9)
							{
								g.DrawLine(pen, rectangle4.X, rectangle4.Y + rectangle4.Height - 1, rectangle4.X + rectangle4.Width - 1, rectangle4.Y + rectangle4.Height - 1);
								g.DrawLine(pen, rectangle4.X + rectangle4.Width - 1, rectangle4.Y, rectangle4.X + rectangle4.Width - 1, rectangle4.Y + rectangle4.Height - 1);
							}
							if (flag8)
							{
								g.DrawLine(pen, rectangle3.X, rectangle3.Y, rectangle3.X + rectangle3.Width - 2, rectangle3.Y);
								g.DrawLine(pen, rectangle3.X, rectangle3.Y, rectangle3.X, rectangle3.Y + rectangle3.Height - 1);
							}
							if (flag10)
							{
								pen = SystemPens.ControlLightLight;
							}
							else
							{
								pen.Color = color;
							}
							if (flag9)
							{
								g.DrawLine(pen, rectangle4.X + 1, rectangle4.Y + 1, rectangle4.X + rectangle4.Width - 2, rectangle4.Y + 1);
								g.DrawLine(pen, rectangle4.X + 1, rectangle4.Y + 1, rectangle4.X + 1, rectangle4.Y + rectangle4.Height - 2);
							}
							if (flag10)
							{
								pen = SystemPens.ControlDark;
							}
							else
							{
								pen.Color = color2;
							}
							if (flag9)
							{
								g.DrawLine(pen, rectangle4.X + 1, rectangle4.Y + rectangle4.Height - 2, rectangle4.X + rectangle4.Width - 2, rectangle4.Y + rectangle4.Height - 2);
								g.DrawLine(pen, rectangle4.X + rectangle4.Width - 2, rectangle4.Y + 1, rectangle4.X + rectangle4.Width - 2, rectangle4.Y + rectangle4.Height - 2);
							}
							if (!flag10)
							{
								pen.Dispose();
							}
						}
						if (num >= 5 && num2 >= 3 && flag9)
						{
							if (flag)
							{
								Point point = new Point(rectangle4.Left + rectangle4.Width / 2, rectangle4.Top + rectangle4.Height / 2);
								point.X += rectangle4.Width % 2;
								point.Y += rectangle4.Height % 2;
								g.FillPolygon(SystemBrushes.ControlText, new Point[]
								{
									new Point(point.X - 2, point.Y - 1),
									new Point(point.X + 3, point.Y - 1),
									new Point(point.X, point.Y + 2)
								});
							}
							else if (!flag3)
							{
								rectangle4.X--;
								rectangle4.Width++;
								Point point2 = new Point(rectangle4.Left + (rectangle4.Width - 1) / 2, rectangle4.Top + (rectangle4.Height + 4) / 2);
								point2.X += (rectangle4.Width + 1) % 2;
								point2.Y += rectangle4.Height % 2;
								Point point3 = new Point(point2.X - 3, point2.Y - 4);
								Point point4 = new Point(point2.X + 3, point2.Y - 4);
								g.FillPolygon(SystemBrushes.ControlText, new Point[] { point3, point4, point2 });
								g.DrawLine(SystemPens.ControlText, point3.X, point3.Y, point4.X, point4.Y);
								rectangle4.X++;
								rectangle4.Width--;
							}
						}
						if (flag2 && flag8)
						{
							rectangle4.Y--;
							rectangle4.Height++;
							g.DrawRectangle(SystemPens.ControlDark, rectangle4);
						}
					}
				}
			}
			Rectangle rectangle5 = rectangle3;
			Rectangle rectangle6 = Rectangle.Inflate(rectangle3, -2, -2);
			if (flag4)
			{
				if (!base.DataGridView.RightToLeftInternal)
				{
					rectangle6.X--;
				}
				rectangle6.Width++;
			}
			if (flag9)
			{
				if (flag3 || flag)
				{
					rectangle5.Width -= num;
					rectangle6.Width -= num;
					if (base.DataGridView.RightToLeftInternal)
					{
						rectangle5.X += num;
						rectangle6.X += num;
					}
				}
				else
				{
					rectangle5.Width -= num + 1;
					rectangle6.Width -= num + 1;
					if (base.DataGridView.RightToLeftInternal)
					{
						rectangle5.X += num + 1;
						rectangle6.X += num + 1;
					}
				}
			}
			if (rectangle6.Width > 1 && rectangle6.Height > 1)
			{
				if (flag5 && !flag6 && DataGridViewCell.PaintFocus(paintParts) && base.DataGridView.ShowFocusCues && base.DataGridView.Focused && paint)
				{
					if (flag)
					{
						Rectangle rectangle7 = rectangle6;
						if (!base.DataGridView.RightToLeftInternal)
						{
							rectangle7.X--;
						}
						rectangle7.Width++;
						rectangle7.Y--;
						rectangle7.Height += 2;
						ControlPaint.DrawFocusRectangle(g, rectangle7, Color.Empty, solidBrush.Color);
					}
					else if (flag4)
					{
						Rectangle rectangle8 = rectangle6;
						rectangle8.X++;
						rectangle8.Width -= 2;
						rectangle8.Y++;
						rectangle8.Height -= 2;
						if (rectangle8.Width > 0 && rectangle8.Height > 0)
						{
							ControlPaint.DrawFocusRectangle(g, rectangle8, Color.Empty, solidBrush.Color);
						}
					}
					else
					{
						ControlPaint.DrawFocusRectangle(g, rectangle6, Color.Empty, solidBrush.Color);
					}
				}
				if (flag2)
				{
					rectangle3.Width--;
					rectangle3.Height--;
					if (!flag6 && paint && DataGridViewCell.PaintContentBackground(paintParts) && flag8)
					{
						g.DrawRectangle(SystemPens.ControlDark, rectangle3);
					}
				}
				string text = formattedValue as string;
				if (text != null)
				{
					int num3 = ((cellStyle.WrapMode == DataGridViewTriState.True) ? 0 : 1);
					if (base.DataGridView.RightToLeftInternal)
					{
						rectangle6.Offset(0, num3);
						rectangle6.Width += 2;
					}
					else
					{
						rectangle6.Offset(-1, num3);
						rectangle6.Width++;
					}
					rectangle6.Height -= num3;
					if (rectangle6.Width > 0 && rectangle6.Height > 0)
					{
						TextFormatFlags textFormatFlags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeftInternal, cellStyle.Alignment, cellStyle.WrapMode);
						if (!flag6 && paint)
						{
							if (DataGridViewCell.PaintContentForeground(paintParts))
							{
								if ((textFormatFlags & TextFormatFlags.SingleLine) != TextFormatFlags.Default)
								{
									textFormatFlags |= TextFormatFlags.EndEllipsis;
								}
								Color color5;
								if (flag4 && (flag9 || flag8))
								{
									color5 = DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.VisualStyleRenderer.GetColor(ColorProperty.TextColor);
								}
								else
								{
									color5 = (flag7 ? cellStyle.SelectionForeColor : cellStyle.ForeColor);
								}
								TextRenderer.DrawText(g, text, cellStyle.Font, rectangle6, color5, textFormatFlags);
							}
						}
						else if (computeContentBounds)
						{
							rectangle = DataGridViewUtilities.GetTextBounds(rectangle6, text, textFormatFlags, cellStyle);
						}
					}
				}
				if (base.DataGridView.ShowCellErrors && paint && DataGridViewCell.PaintErrorIcon(paintParts))
				{
					base.PaintErrorIcon(g, cellStyle, rowIndex, cellBounds, rectangle5, errorText);
					if (flag6)
					{
						return Rectangle.Empty;
					}
				}
			}
			if (computeErrorIconBounds)
			{
				if (!string.IsNullOrEmpty(errorText))
				{
					rectangle = base.ComputeErrorIconBounds(rectangle5);
				}
				else
				{
					rectangle = Rectangle.Empty;
				}
			}
			return rectangle;
		}

		// Token: 0x0600351E RID: 13598 RVA: 0x000BF6B0 File Offset: 0x000BE6B0
		public override object ParseFormattedValue(object formattedValue, DataGridViewCellStyle cellStyle, TypeConverter formattedValueTypeConverter, TypeConverter valueTypeConverter)
		{
			if (valueTypeConverter == null)
			{
				if (this.ValueMemberProperty != null)
				{
					valueTypeConverter = this.ValueMemberProperty.Converter;
				}
				else if (this.DisplayMemberProperty != null)
				{
					valueTypeConverter = this.DisplayMemberProperty.Converter;
				}
			}
			if ((this.DataManager != null && (this.DisplayMemberProperty != null || this.ValueMemberProperty != null)) || !string.IsNullOrEmpty(this.DisplayMember) || !string.IsNullOrEmpty(this.ValueMember))
			{
				object obj = base.ParseFormattedValueInternal(this.DisplayType, formattedValue, cellStyle, formattedValueTypeConverter, this.DisplayTypeConverter);
				object obj2 = obj;
				if (!this.LookupValue(obj2, out obj))
				{
					if (obj2 != DBNull.Value)
					{
						throw new FormatException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Formatter_CantConvert"), new object[] { obj, this.DisplayType }));
					}
					obj = DBNull.Value;
				}
				return obj;
			}
			return base.ParseFormattedValueInternal(this.ValueType, formattedValue, cellStyle, formattedValueTypeConverter, valueTypeConverter);
		}

		// Token: 0x0600351F RID: 13599 RVA: 0x000BF794 File Offset: 0x000BE794
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"DataGridViewComboBoxCell { ColumnIndex=",
				base.ColumnIndex.ToString(CultureInfo.CurrentCulture),
				", RowIndex=",
				base.RowIndex.ToString(CultureInfo.CurrentCulture),
				" }"
			});
		}

		// Token: 0x06003520 RID: 13600 RVA: 0x000BF7F4 File Offset: 0x000BE7F4
		private void UnwireDataSource()
		{
			IComponent component = this.DataSource as IComponent;
			if (component != null)
			{
				component.Disposed -= this.DataSource_Disposed;
			}
			ISupportInitializeNotification supportInitializeNotification = this.DataSource as ISupportInitializeNotification;
			if (supportInitializeNotification != null && (this.flags & 16) != 0)
			{
				supportInitializeNotification.Initialized -= this.DataSource_Initialized;
				this.flags = (byte)((int)this.flags & -17);
			}
		}

		// Token: 0x06003521 RID: 13601 RVA: 0x000BF860 File Offset: 0x000BE860
		private void WireDataSource(object dataSource)
		{
			IComponent component = dataSource as IComponent;
			if (component != null)
			{
				component.Disposed += this.DataSource_Disposed;
			}
		}

		// Token: 0x04001B48 RID: 6984
		private const byte DATAGRIDVIEWCOMBOBOXCELL_margin = 3;

		// Token: 0x04001B49 RID: 6985
		private const byte DATAGRIDVIEWCOMBOBOXCELL_nonXPTriangleHeight = 4;

		// Token: 0x04001B4A RID: 6986
		private const byte DATAGRIDVIEWCOMBOBOXCELL_nonXPTriangleWidth = 7;

		// Token: 0x04001B4B RID: 6987
		private const byte DATAGRIDVIEWCOMBOBOXCELL_horizontalTextMarginLeft = 0;

		// Token: 0x04001B4C RID: 6988
		private const byte DATAGRIDVIEWCOMBOBOXCELL_verticalTextMarginTopWithWrapping = 0;

		// Token: 0x04001B4D RID: 6989
		private const byte DATAGRIDVIEWCOMBOBOXCELL_verticalTextMarginTopWithoutWrapping = 1;

		// Token: 0x04001B4E RID: 6990
		private const byte DATAGRIDVIEWCOMBOBOXCELL_ignoreNextMouseClick = 1;

		// Token: 0x04001B4F RID: 6991
		private const byte DATAGRIDVIEWCOMBOBOXCELL_sorted = 2;

		// Token: 0x04001B50 RID: 6992
		private const byte DATAGRIDVIEWCOMBOBOXCELL_createItemsFromDataSource = 4;

		// Token: 0x04001B51 RID: 6993
		private const byte DATAGRIDVIEWCOMBOBOXCELL_autoComplete = 8;

		// Token: 0x04001B52 RID: 6994
		private const byte DATAGRIDVIEWCOMBOBOXCELL_dataSourceInitializedHookedUp = 16;

		// Token: 0x04001B53 RID: 6995
		private const byte DATAGRIDVIEWCOMBOBOXCELL_dropDownHookedUp = 32;

		// Token: 0x04001B54 RID: 6996
		internal const int DATAGRIDVIEWCOMBOBOXCELL_defaultMaxDropDownItems = 8;

		// Token: 0x04001B55 RID: 6997
		private static readonly int PropComboBoxCellDataSource = PropertyStore.CreateKey();

		// Token: 0x04001B56 RID: 6998
		private static readonly int PropComboBoxCellDisplayMember = PropertyStore.CreateKey();

		// Token: 0x04001B57 RID: 6999
		private static readonly int PropComboBoxCellValueMember = PropertyStore.CreateKey();

		// Token: 0x04001B58 RID: 7000
		private static readonly int PropComboBoxCellItems = PropertyStore.CreateKey();

		// Token: 0x04001B59 RID: 7001
		private static readonly int PropComboBoxCellDropDownWidth = PropertyStore.CreateKey();

		// Token: 0x04001B5A RID: 7002
		private static readonly int PropComboBoxCellMaxDropDownItems = PropertyStore.CreateKey();

		// Token: 0x04001B5B RID: 7003
		private static readonly int PropComboBoxCellEditingComboBox = PropertyStore.CreateKey();

		// Token: 0x04001B5C RID: 7004
		private static readonly int PropComboBoxCellValueMemberProp = PropertyStore.CreateKey();

		// Token: 0x04001B5D RID: 7005
		private static readonly int PropComboBoxCellDisplayMemberProp = PropertyStore.CreateKey();

		// Token: 0x04001B5E RID: 7006
		private static readonly int PropComboBoxCellDataManager = PropertyStore.CreateKey();

		// Token: 0x04001B5F RID: 7007
		private static readonly int PropComboBoxCellColumnTemplate = PropertyStore.CreateKey();

		// Token: 0x04001B60 RID: 7008
		private static readonly int PropComboBoxCellFlatStyle = PropertyStore.CreateKey();

		// Token: 0x04001B61 RID: 7009
		private static readonly int PropComboBoxCellDisplayStyle = PropertyStore.CreateKey();

		// Token: 0x04001B62 RID: 7010
		private static readonly int PropComboBoxCellDisplayStyleForCurrentCellOnly = PropertyStore.CreateKey();

		// Token: 0x04001B63 RID: 7011
		private static Type defaultFormattedValueType = typeof(string);

		// Token: 0x04001B64 RID: 7012
		private static Type defaultEditType = typeof(DataGridViewComboBoxEditingControl);

		// Token: 0x04001B65 RID: 7013
		private static Type defaultValueType = typeof(object);

		// Token: 0x04001B66 RID: 7014
		private static Type cellType = typeof(DataGridViewComboBoxCell);

		// Token: 0x04001B67 RID: 7015
		private byte flags;

		// Token: 0x04001B68 RID: 7016
		private static bool mouseInDropDownButtonBounds = false;

		// Token: 0x04001B69 RID: 7017
		private static int cachedDropDownWidth = -1;

		// Token: 0x0200033E RID: 830
		[ListBindable(false)]
		public class ObjectCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x06003523 RID: 13603 RVA: 0x000BF96D File Offset: 0x000BE96D
			public ObjectCollection(DataGridViewComboBoxCell owner)
			{
				this.owner = owner;
			}

			// Token: 0x170009AD RID: 2477
			// (get) Token: 0x06003524 RID: 13604 RVA: 0x000BF97C File Offset: 0x000BE97C
			private IComparer Comparer
			{
				get
				{
					if (this.comparer == null)
					{
						this.comparer = new DataGridViewComboBoxCell.ItemComparer(this.owner);
					}
					return this.comparer;
				}
			}

			// Token: 0x170009AE RID: 2478
			// (get) Token: 0x06003525 RID: 13605 RVA: 0x000BF99D File Offset: 0x000BE99D
			public int Count
			{
				get
				{
					return this.InnerArray.Count;
				}
			}

			// Token: 0x170009AF RID: 2479
			// (get) Token: 0x06003526 RID: 13606 RVA: 0x000BF9AA File Offset: 0x000BE9AA
			internal ArrayList InnerArray
			{
				get
				{
					if (this.items == null)
					{
						this.items = new ArrayList();
					}
					return this.items;
				}
			}

			// Token: 0x170009B0 RID: 2480
			// (get) Token: 0x06003527 RID: 13607 RVA: 0x000BF9C5 File Offset: 0x000BE9C5
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x170009B1 RID: 2481
			// (get) Token: 0x06003528 RID: 13608 RVA: 0x000BF9C8 File Offset: 0x000BE9C8
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170009B2 RID: 2482
			// (get) Token: 0x06003529 RID: 13609 RVA: 0x000BF9CB File Offset: 0x000BE9CB
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170009B3 RID: 2483
			// (get) Token: 0x0600352A RID: 13610 RVA: 0x000BF9CE File Offset: 0x000BE9CE
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x0600352B RID: 13611 RVA: 0x000BF9D4 File Offset: 0x000BE9D4
			public int Add(object item)
			{
				this.owner.CheckNoDataSource();
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				int num = this.InnerArray.Add(item);
				bool flag = false;
				if (this.owner.Sorted)
				{
					try
					{
						this.InnerArray.Sort(this.Comparer);
						num = this.InnerArray.IndexOf(item);
						flag = true;
					}
					finally
					{
						if (!flag)
						{
							this.InnerArray.Remove(item);
						}
					}
				}
				this.owner.OnItemsCollectionChanged();
				return num;
			}

			// Token: 0x0600352C RID: 13612 RVA: 0x000BFA64 File Offset: 0x000BEA64
			int IList.Add(object item)
			{
				return this.Add(item);
			}

			// Token: 0x0600352D RID: 13613 RVA: 0x000BFA6D File Offset: 0x000BEA6D
			public void AddRange(params object[] items)
			{
				this.owner.CheckNoDataSource();
				this.AddRangeInternal(items);
				this.owner.OnItemsCollectionChanged();
			}

			// Token: 0x0600352E RID: 13614 RVA: 0x000BFA8C File Offset: 0x000BEA8C
			public void AddRange(DataGridViewComboBoxCell.ObjectCollection value)
			{
				this.owner.CheckNoDataSource();
				this.AddRangeInternal(value);
				this.owner.OnItemsCollectionChanged();
			}

			// Token: 0x0600352F RID: 13615 RVA: 0x000BFAAC File Offset: 0x000BEAAC
			internal void AddRangeInternal(ICollection items)
			{
				if (items == null)
				{
					throw new ArgumentNullException("items");
				}
				using (IEnumerator enumerator = items.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current == null)
						{
							throw new InvalidOperationException(SR.GetString("InvalidNullItemInCollection"));
						}
					}
				}
				this.InnerArray.AddRange(items);
				if (this.owner.Sorted)
				{
					this.InnerArray.Sort(this.Comparer);
				}
			}

			// Token: 0x06003530 RID: 13616 RVA: 0x000BFB40 File Offset: 0x000BEB40
			internal void SortInternal()
			{
				this.InnerArray.Sort(this.Comparer);
			}

			// Token: 0x170009B4 RID: 2484
			public virtual object this[int index]
			{
				get
				{
					if (index < 0 || index >= this.InnerArray.Count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					return this.InnerArray[index];
				}
				set
				{
					this.owner.CheckNoDataSource();
					if (value == null)
					{
						throw new ArgumentNullException("value");
					}
					if (index < 0 || index >= this.InnerArray.Count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.InnerArray[index] = value;
					this.owner.OnItemsCollectionChanged();
				}
			}

			// Token: 0x06003533 RID: 13619 RVA: 0x000BFC38 File Offset: 0x000BEC38
			public void Clear()
			{
				if (this.InnerArray.Count > 0)
				{
					this.owner.CheckNoDataSource();
					this.InnerArray.Clear();
					this.owner.OnItemsCollectionChanged();
				}
			}

			// Token: 0x06003534 RID: 13620 RVA: 0x000BFC69 File Offset: 0x000BEC69
			internal void ClearInternal()
			{
				this.InnerArray.Clear();
			}

			// Token: 0x06003535 RID: 13621 RVA: 0x000BFC76 File Offset: 0x000BEC76
			public bool Contains(object value)
			{
				return this.IndexOf(value) != -1;
			}

			// Token: 0x06003536 RID: 13622 RVA: 0x000BFC88 File Offset: 0x000BEC88
			public void CopyTo(object[] destination, int arrayIndex)
			{
				int count = this.InnerArray.Count;
				for (int i = 0; i < count; i++)
				{
					destination[i + arrayIndex] = this.InnerArray[i];
				}
			}

			// Token: 0x06003537 RID: 13623 RVA: 0x000BFCC0 File Offset: 0x000BECC0
			void ICollection.CopyTo(Array destination, int index)
			{
				int count = this.InnerArray.Count;
				for (int i = 0; i < count; i++)
				{
					destination.SetValue(this.InnerArray[i], i + index);
				}
			}

			// Token: 0x06003538 RID: 13624 RVA: 0x000BFCFA File Offset: 0x000BECFA
			public IEnumerator GetEnumerator()
			{
				return this.InnerArray.GetEnumerator();
			}

			// Token: 0x06003539 RID: 13625 RVA: 0x000BFD07 File Offset: 0x000BED07
			public int IndexOf(object value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				return this.InnerArray.IndexOf(value);
			}

			// Token: 0x0600353A RID: 13626 RVA: 0x000BFD24 File Offset: 0x000BED24
			public void Insert(int index, object item)
			{
				this.owner.CheckNoDataSource();
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				if (index < 0 || index > this.InnerArray.Count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.owner.Sorted)
				{
					this.Add(item);
					return;
				}
				this.InnerArray.Insert(index, item);
				this.owner.OnItemsCollectionChanged();
			}

			// Token: 0x0600353B RID: 13627 RVA: 0x000BFDC0 File Offset: 0x000BEDC0
			public void Remove(object value)
			{
				int num = this.InnerArray.IndexOf(value);
				if (num != -1)
				{
					this.RemoveAt(num);
				}
			}

			// Token: 0x0600353C RID: 13628 RVA: 0x000BFDE8 File Offset: 0x000BEDE8
			public void RemoveAt(int index)
			{
				this.owner.CheckNoDataSource();
				if (index < 0 || index >= this.InnerArray.Count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.InnerArray.RemoveAt(index);
				this.owner.OnItemsCollectionChanged();
			}

			// Token: 0x04001B6A RID: 7018
			private DataGridViewComboBoxCell owner;

			// Token: 0x04001B6B RID: 7019
			private ArrayList items;

			// Token: 0x04001B6C RID: 7020
			private IComparer comparer;
		}

		// Token: 0x0200033F RID: 831
		private sealed class ItemComparer : IComparer
		{
			// Token: 0x0600353D RID: 13629 RVA: 0x000BFE5D File Offset: 0x000BEE5D
			public ItemComparer(DataGridViewComboBoxCell dataGridViewComboBoxCell)
			{
				this.dataGridViewComboBoxCell = dataGridViewComboBoxCell;
			}

			// Token: 0x0600353E RID: 13630 RVA: 0x000BFE6C File Offset: 0x000BEE6C
			public int Compare(object item1, object item2)
			{
				if (item1 == null)
				{
					if (item2 == null)
					{
						return 0;
					}
					return -1;
				}
				else
				{
					if (item2 == null)
					{
						return 1;
					}
					string itemDisplayText = this.dataGridViewComboBoxCell.GetItemDisplayText(item1);
					string itemDisplayText2 = this.dataGridViewComboBoxCell.GetItemDisplayText(item2);
					CompareInfo compareInfo = Application.CurrentCulture.CompareInfo;
					return compareInfo.Compare(itemDisplayText, itemDisplayText2, CompareOptions.StringSort);
				}
			}

			// Token: 0x04001B6D RID: 7021
			private DataGridViewComboBoxCell dataGridViewComboBoxCell;
		}

		// Token: 0x02000340 RID: 832
		private class DataGridViewComboBoxCellRenderer
		{
			// Token: 0x0600353F RID: 13631 RVA: 0x000BFEBA File Offset: 0x000BEEBA
			private DataGridViewComboBoxCellRenderer()
			{
			}

			// Token: 0x170009B5 RID: 2485
			// (get) Token: 0x06003540 RID: 13632 RVA: 0x000BFEC2 File Offset: 0x000BEEC2
			public static VisualStyleRenderer VisualStyleRenderer
			{
				get
				{
					if (DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer == null)
					{
						DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer = new VisualStyleRenderer(DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxReadOnlyButton);
					}
					return DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer;
				}
			}

			// Token: 0x06003541 RID: 13633 RVA: 0x000BFEDF File Offset: 0x000BEEDF
			public static void DrawTextBox(Graphics g, Rectangle bounds, ComboBoxState state)
			{
				ComboBoxRenderer.DrawTextBox(g, bounds, state);
			}

			// Token: 0x06003542 RID: 13634 RVA: 0x000BFEE9 File Offset: 0x000BEEE9
			public static void DrawDropDownButton(Graphics g, Rectangle bounds, ComboBoxState state)
			{
				ComboBoxRenderer.DrawDropDownButton(g, bounds, state);
			}

			// Token: 0x06003543 RID: 13635 RVA: 0x000BFEF4 File Offset: 0x000BEEF4
			public static void DrawBorder(Graphics g, Rectangle bounds)
			{
				if (DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer == null)
				{
					DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer = new VisualStyleRenderer(DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxBorder);
				}
				else
				{
					DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer.SetParameters(DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxBorder.ClassName, DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxBorder.Part, DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxBorder.State);
				}
				DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer.DrawBackground(g, bounds);
			}

			// Token: 0x06003544 RID: 13636 RVA: 0x000BFF50 File Offset: 0x000BEF50
			public static void DrawDropDownButton(Graphics g, Rectangle bounds, ComboBoxState state, bool rightToLeft)
			{
				if (rightToLeft)
				{
					if (DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer == null)
					{
						DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer = new VisualStyleRenderer(DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxDropDownButtonLeft.ClassName, DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxDropDownButtonLeft.Part, (int)state);
					}
					else
					{
						DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer.SetParameters(DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxDropDownButtonLeft.ClassName, DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxDropDownButtonLeft.Part, (int)state);
					}
				}
				else if (DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer == null)
				{
					DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer = new VisualStyleRenderer(DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxDropDownButtonRight.ClassName, DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxDropDownButtonRight.Part, (int)state);
				}
				else
				{
					DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer.SetParameters(DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxDropDownButtonRight.ClassName, DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxDropDownButtonRight.Part, (int)state);
				}
				DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer.DrawBackground(g, bounds);
			}

			// Token: 0x06003545 RID: 13637 RVA: 0x000BFFFC File Offset: 0x000BEFFC
			public static void DrawReadOnlyButton(Graphics g, Rectangle bounds, ComboBoxState state)
			{
				if (DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer == null)
				{
					DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer = new VisualStyleRenderer(DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxReadOnlyButton.ClassName, DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxReadOnlyButton.Part, (int)state);
				}
				else
				{
					DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer.SetParameters(DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxReadOnlyButton.ClassName, DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.ComboBoxReadOnlyButton.Part, (int)state);
				}
				DataGridViewComboBoxCell.DataGridViewComboBoxCellRenderer.visualStyleRenderer.DrawBackground(g, bounds);
			}

			// Token: 0x04001B6E RID: 7022
			[ThreadStatic]
			private static VisualStyleRenderer visualStyleRenderer;

			// Token: 0x04001B6F RID: 7023
			private static readonly VisualStyleElement ComboBoxBorder = VisualStyleElement.ComboBox.Border.Normal;

			// Token: 0x04001B70 RID: 7024
			private static readonly VisualStyleElement ComboBoxDropDownButtonRight = VisualStyleElement.ComboBox.DropDownButtonRight.Normal;

			// Token: 0x04001B71 RID: 7025
			private static readonly VisualStyleElement ComboBoxDropDownButtonLeft = VisualStyleElement.ComboBox.DropDownButtonLeft.Normal;

			// Token: 0x04001B72 RID: 7026
			private static readonly VisualStyleElement ComboBoxReadOnlyButton = VisualStyleElement.ComboBox.ReadOnlyButton.Normal;
		}
	}
}
