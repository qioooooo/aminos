using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x0200030D RID: 781
	[DesignTimeVisible(false)]
	[Designer("System.Windows.Forms.Design.DataGridViewColumnDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ToolboxItem(false)]
	[TypeConverter(typeof(DataGridViewColumnConverter))]
	public class DataGridViewColumn : DataGridViewBand, IComponent, IDisposable
	{
		// Token: 0x060032AB RID: 12971 RVA: 0x000B1EB9 File Offset: 0x000B0EB9
		public DataGridViewColumn()
			: this(null)
		{
		}

		// Token: 0x060032AC RID: 12972 RVA: 0x000B1EC4 File Offset: 0x000B0EC4
		public DataGridViewColumn(DataGridViewCell cellTemplate)
		{
			this.fillWeight = 100f;
			this.usedFillWeight = 100f;
			base.Thickness = 100;
			base.MinimumThickness = 5;
			this.name = string.Empty;
			this.bandIsRow = false;
			this.displayIndex = -1;
			this.cellTemplate = cellTemplate;
			this.autoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x060032AD RID: 12973 RVA: 0x000B1F35 File Offset: 0x000B0F35
		// (set) Token: 0x060032AE RID: 12974 RVA: 0x000B1F40 File Offset: 0x000B0F40
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("DataGridViewColumn_AutoSizeModeDescr")]
		[SRCategory("CatLayout")]
		[DefaultValue(DataGridViewAutoSizeColumnMode.NotSet)]
		public DataGridViewAutoSizeColumnMode AutoSizeMode
		{
			get
			{
				return this.autoSizeMode;
			}
			set
			{
				switch (value)
				{
				case DataGridViewAutoSizeColumnMode.NotSet:
				case DataGridViewAutoSizeColumnMode.None:
				case DataGridViewAutoSizeColumnMode.ColumnHeader:
				case DataGridViewAutoSizeColumnMode.AllCellsExceptHeader:
				case DataGridViewAutoSizeColumnMode.AllCells:
				case DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader:
				case DataGridViewAutoSizeColumnMode.DisplayedCells:
					goto IL_004F;
				case (DataGridViewAutoSizeColumnMode)3:
				case (DataGridViewAutoSizeColumnMode)5:
				case (DataGridViewAutoSizeColumnMode)7:
				case (DataGridViewAutoSizeColumnMode)9:
					break;
				default:
					if (value == DataGridViewAutoSizeColumnMode.Fill)
					{
						goto IL_004F;
					}
					break;
				}
				throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridViewAutoSizeColumnMode));
				IL_004F:
				if (this.autoSizeMode != value)
				{
					if (this.Visible && base.DataGridView != null)
					{
						if (!base.DataGridView.ColumnHeadersVisible && (value == DataGridViewAutoSizeColumnMode.ColumnHeader || (value == DataGridViewAutoSizeColumnMode.NotSet && base.DataGridView.AutoSizeColumnsMode == DataGridViewAutoSizeColumnsMode.ColumnHeader)))
						{
							throw new InvalidOperationException(SR.GetString("DataGridViewColumn_AutoSizeCriteriaCannotUseInvisibleHeaders"));
						}
						if (this.Frozen && (value == DataGridViewAutoSizeColumnMode.Fill || (value == DataGridViewAutoSizeColumnMode.NotSet && base.DataGridView.AutoSizeColumnsMode == DataGridViewAutoSizeColumnsMode.Fill)))
						{
							throw new InvalidOperationException(SR.GetString("DataGridViewColumn_FrozenColumnCannotAutoFill"));
						}
					}
					DataGridViewAutoSizeColumnMode inheritedAutoSizeMode = this.InheritedAutoSizeMode;
					bool flag = inheritedAutoSizeMode != DataGridViewAutoSizeColumnMode.Fill && inheritedAutoSizeMode != DataGridViewAutoSizeColumnMode.None && inheritedAutoSizeMode != DataGridViewAutoSizeColumnMode.NotSet;
					this.autoSizeMode = value;
					if (base.DataGridView == null)
					{
						if (this.InheritedAutoSizeMode != DataGridViewAutoSizeColumnMode.Fill && this.InheritedAutoSizeMode != DataGridViewAutoSizeColumnMode.None && this.InheritedAutoSizeMode != DataGridViewAutoSizeColumnMode.NotSet)
						{
							if (!flag)
							{
								base.CachedThickness = base.Thickness;
								return;
							}
						}
						else if (base.Thickness != base.CachedThickness && flag)
						{
							base.ThicknessInternal = base.CachedThickness;
							return;
						}
					}
					else
					{
						base.DataGridView.OnAutoSizeColumnModeChanged(this, inheritedAutoSizeMode);
					}
				}
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x060032AF RID: 12975 RVA: 0x000B2099 File Offset: 0x000B1099
		// (set) Token: 0x060032B0 RID: 12976 RVA: 0x000B20A1 File Offset: 0x000B10A1
		internal TypeConverter BoundColumnConverter
		{
			get
			{
				return this.boundColumnConverter;
			}
			set
			{
				this.boundColumnConverter = value;
			}
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x060032B1 RID: 12977 RVA: 0x000B20AA File Offset: 0x000B10AA
		// (set) Token: 0x060032B2 RID: 12978 RVA: 0x000B20B2 File Offset: 0x000B10B2
		internal int BoundColumnIndex
		{
			get
			{
				return this.boundColumnIndex;
			}
			set
			{
				this.boundColumnIndex = value;
			}
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x060032B3 RID: 12979 RVA: 0x000B20BB File Offset: 0x000B10BB
		// (set) Token: 0x060032B4 RID: 12980 RVA: 0x000B20C3 File Offset: 0x000B10C3
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual DataGridViewCell CellTemplate
		{
			get
			{
				return this.cellTemplate;
			}
			set
			{
				this.cellTemplate = value;
			}
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x060032B5 RID: 12981 RVA: 0x000B20CC File Offset: 0x000B10CC
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public Type CellType
		{
			get
			{
				if (this.cellTemplate != null)
				{
					return this.cellTemplate.GetType();
				}
				return null;
			}
		}

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x060032B6 RID: 12982 RVA: 0x000B20E3 File Offset: 0x000B10E3
		// (set) Token: 0x060032B7 RID: 12983 RVA: 0x000B20EB File Offset: 0x000B10EB
		[SRDescription("DataGridView_ColumnContextMenuStripDescr")]
		[DefaultValue(null)]
		[SRCategory("CatBehavior")]
		public override ContextMenuStrip ContextMenuStrip
		{
			get
			{
				return base.ContextMenuStrip;
			}
			set
			{
				base.ContextMenuStrip = value;
			}
		}

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x060032B8 RID: 12984 RVA: 0x000B20F4 File Offset: 0x000B10F4
		// (set) Token: 0x060032B9 RID: 12985 RVA: 0x000B20FC File Offset: 0x000B10FC
		[Browsable(true)]
		[SRCategory("CatData")]
		[DefaultValue("")]
		[TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[Editor("System.Windows.Forms.Design.DataGridViewColumnDataPropertyNameEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("DataGridView_ColumnDataPropertyNameDescr")]
		public string DataPropertyName
		{
			get
			{
				return this.dataPropertyName;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				if (value != this.dataPropertyName)
				{
					this.dataPropertyName = value;
					if (base.DataGridView != null)
					{
						base.DataGridView.OnColumnDataPropertyNameChanged(this);
					}
				}
			}
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x060032BA RID: 12986 RVA: 0x000B2131 File Offset: 0x000B1131
		// (set) Token: 0x060032BB RID: 12987 RVA: 0x000B2139 File Offset: 0x000B1139
		[Browsable(true)]
		[SRDescription("DataGridView_ColumnDefaultCellStyleDescr")]
		[SRCategory("CatAppearance")]
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

		// Token: 0x060032BC RID: 12988 RVA: 0x000B2144 File Offset: 0x000B1144
		private bool ShouldSerializeDefaultCellStyle()
		{
			if (!base.HasDefaultCellStyle)
			{
				return false;
			}
			DataGridViewCellStyle defaultCellStyle = this.DefaultCellStyle;
			return !defaultCellStyle.BackColor.IsEmpty || !defaultCellStyle.ForeColor.IsEmpty || !defaultCellStyle.SelectionBackColor.IsEmpty || !defaultCellStyle.SelectionForeColor.IsEmpty || defaultCellStyle.Font != null || !defaultCellStyle.IsNullValueDefault || !defaultCellStyle.IsDataSourceNullValueDefault || !string.IsNullOrEmpty(defaultCellStyle.Format) || !defaultCellStyle.FormatProvider.Equals(CultureInfo.CurrentCulture) || defaultCellStyle.Alignment != DataGridViewContentAlignment.NotSet || defaultCellStyle.WrapMode != DataGridViewTriState.NotSet || defaultCellStyle.Tag != null || !defaultCellStyle.Padding.Equals(Padding.Empty);
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x060032BD RID: 12989 RVA: 0x000B221F File Offset: 0x000B121F
		// (set) Token: 0x060032BE RID: 12990 RVA: 0x000B2227 File Offset: 0x000B1227
		internal int DesiredFillWidth
		{
			get
			{
				return this.desiredFillWidth;
			}
			set
			{
				this.desiredFillWidth = value;
			}
		}

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x060032BF RID: 12991 RVA: 0x000B2230 File Offset: 0x000B1230
		// (set) Token: 0x060032C0 RID: 12992 RVA: 0x000B2238 File Offset: 0x000B1238
		internal int DesiredMinimumWidth
		{
			get
			{
				return this.desiredMinimumWidth;
			}
			set
			{
				this.desiredMinimumWidth = value;
			}
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x060032C1 RID: 12993 RVA: 0x000B2241 File Offset: 0x000B1241
		// (set) Token: 0x060032C2 RID: 12994 RVA: 0x000B224C File Offset: 0x000B124C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int DisplayIndex
		{
			get
			{
				return this.displayIndex;
			}
			set
			{
				if (this.displayIndex != value)
				{
					if (value == 2147483647)
					{
						throw new ArgumentOutOfRangeException("DisplayIndex", value, SR.GetString("DataGridViewColumn_DisplayIndexTooLarge", new object[] { int.MaxValue.ToString(CultureInfo.CurrentCulture) }));
					}
					if (base.DataGridView != null)
					{
						if (value < 0)
						{
							throw new ArgumentOutOfRangeException("DisplayIndex", value, SR.GetString("DataGridViewColumn_DisplayIndexNegative"));
						}
						if (value >= base.DataGridView.Columns.Count)
						{
							throw new ArgumentOutOfRangeException("DisplayIndex", value, SR.GetString("DataGridViewColumn_DisplayIndexExceedsColumnCount"));
						}
						base.DataGridView.OnColumnDisplayIndexChanging(this, value);
						this.displayIndex = value;
						try
						{
							base.DataGridView.InDisplayIndexAdjustments = true;
							base.DataGridView.OnColumnDisplayIndexChanged_PreNotification();
							base.DataGridView.OnColumnDisplayIndexChanged(this);
							base.DataGridView.OnColumnDisplayIndexChanged_PostNotification();
							return;
						}
						finally
						{
							base.DataGridView.InDisplayIndexAdjustments = false;
						}
					}
					if (value < -1)
					{
						throw new ArgumentOutOfRangeException("DisplayIndex", value, SR.GetString("DataGridViewColumn_DisplayIndexTooNegative"));
					}
					this.displayIndex = value;
				}
			}
		}

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x060032C3 RID: 12995 RVA: 0x000B2384 File Offset: 0x000B1384
		// (set) Token: 0x060032C4 RID: 12996 RVA: 0x000B2395 File Offset: 0x000B1395
		internal bool DisplayIndexHasChanged
		{
			get
			{
				return (this.flags & 16) != 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 16;
					return;
				}
				this.flags = (byte)((int)this.flags & -17);
			}
		}

		// Token: 0x170008DA RID: 2266
		// (set) Token: 0x060032C5 RID: 12997 RVA: 0x000B23BB File Offset: 0x000B13BB
		internal int DisplayIndexInternal
		{
			set
			{
				this.displayIndex = value;
			}
		}

		// Token: 0x140001D1 RID: 465
		// (add) Token: 0x060032C6 RID: 12998 RVA: 0x000B23C4 File Offset: 0x000B13C4
		// (remove) Token: 0x060032C7 RID: 12999 RVA: 0x000B23DD File Offset: 0x000B13DD
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler Disposed
		{
			add
			{
				this.disposed = (EventHandler)Delegate.Combine(this.disposed, value);
			}
			remove
			{
				this.disposed = (EventHandler)Delegate.Remove(this.disposed, value);
			}
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x060032C8 RID: 13000 RVA: 0x000B23F6 File Offset: 0x000B13F6
		// (set) Token: 0x060032C9 RID: 13001 RVA: 0x000B23FE File Offset: 0x000B13FE
		[DefaultValue(0)]
		[SRCategory("CatLayout")]
		[SRDescription("DataGridView_ColumnDividerWidthDescr")]
		public int DividerWidth
		{
			get
			{
				return base.DividerThickness;
			}
			set
			{
				base.DividerThickness = value;
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x060032CA RID: 13002 RVA: 0x000B2407 File Offset: 0x000B1407
		// (set) Token: 0x060032CB RID: 13003 RVA: 0x000B2410 File Offset: 0x000B1410
		[SRCategory("CatLayout")]
		[DefaultValue(100f)]
		[SRDescription("DataGridViewColumn_FillWeightDescr")]
		public float FillWeight
		{
			get
			{
				return this.fillWeight;
			}
			set
			{
				if (value <= 0f)
				{
					throw new ArgumentOutOfRangeException("FillWeight", SR.GetString("InvalidLowBoundArgument", new object[]
					{
						"FillWeight",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (value > 65535f)
				{
					throw new ArgumentOutOfRangeException("FillWeight", SR.GetString("InvalidHighBoundArgumentEx", new object[]
					{
						"FillWeight",
						value.ToString(CultureInfo.CurrentCulture),
						ushort.MaxValue.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (base.DataGridView != null)
				{
					base.DataGridView.OnColumnFillWeightChanging(this, value);
					this.fillWeight = value;
					base.DataGridView.OnColumnFillWeightChanged(this);
					return;
				}
				this.fillWeight = value;
			}
		}

		// Token: 0x170008DD RID: 2269
		// (set) Token: 0x060032CC RID: 13004 RVA: 0x000B24EB File Offset: 0x000B14EB
		internal float FillWeightInternal
		{
			set
			{
				this.fillWeight = value;
			}
		}

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x060032CD RID: 13005 RVA: 0x000B24F4 File Offset: 0x000B14F4
		// (set) Token: 0x060032CE RID: 13006 RVA: 0x000B24FC File Offset: 0x000B14FC
		[SRDescription("DataGridView_ColumnFrozenDescr")]
		[DefaultValue(false)]
		[RefreshProperties(RefreshProperties.All)]
		[SRCategory("CatLayout")]
		public override bool Frozen
		{
			get
			{
				return base.Frozen;
			}
			set
			{
				base.Frozen = value;
			}
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x060032CF RID: 13007 RVA: 0x000B2505 File Offset: 0x000B1505
		// (set) Token: 0x060032D0 RID: 13008 RVA: 0x000B2512 File Offset: 0x000B1512
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public DataGridViewColumnHeaderCell HeaderCell
		{
			get
			{
				return (DataGridViewColumnHeaderCell)base.HeaderCellCore;
			}
			set
			{
				base.HeaderCellCore = value;
			}
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x060032D1 RID: 13009 RVA: 0x000B251C File Offset: 0x000B151C
		// (set) Token: 0x060032D2 RID: 13010 RVA: 0x000B2554 File Offset: 0x000B1554
		[Localizable(true)]
		[SRDescription("DataGridView_ColumnHeaderTextDescr")]
		[SRCategory("CatAppearance")]
		public string HeaderText
		{
			get
			{
				if (!base.HasHeaderCell)
				{
					return string.Empty;
				}
				string text = this.HeaderCell.Value as string;
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				if ((value != null || base.HasHeaderCell) && this.HeaderCell.ValueType != null && this.HeaderCell.ValueType.IsAssignableFrom(typeof(string)))
				{
					this.HeaderCell.Value = value;
				}
			}
		}

		// Token: 0x060032D3 RID: 13011 RVA: 0x000B25A1 File Offset: 0x000B15A1
		private bool ShouldSerializeHeaderText()
		{
			return base.HasHeaderCell && this.HeaderCell.ContainsLocalValue;
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x060032D4 RID: 13012 RVA: 0x000B25B8 File Offset: 0x000B15B8
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataGridViewAutoSizeColumnMode InheritedAutoSizeMode
		{
			get
			{
				return this.GetInheritedAutoSizeMode(base.DataGridView);
			}
		}

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x060032D5 RID: 13013 RVA: 0x000B25C8 File Offset: 0x000B15C8
		[Browsable(false)]
		public override DataGridViewCellStyle InheritedStyle
		{
			get
			{
				DataGridViewCellStyle dataGridViewCellStyle = null;
				if (base.HasDefaultCellStyle)
				{
					dataGridViewCellStyle = this.DefaultCellStyle;
				}
				if (base.DataGridView == null)
				{
					return dataGridViewCellStyle;
				}
				DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
				DataGridViewCellStyle defaultCellStyle = base.DataGridView.DefaultCellStyle;
				if (dataGridViewCellStyle != null && !dataGridViewCellStyle.BackColor.IsEmpty)
				{
					dataGridViewCellStyle2.BackColor = dataGridViewCellStyle.BackColor;
				}
				else
				{
					dataGridViewCellStyle2.BackColor = defaultCellStyle.BackColor;
				}
				if (dataGridViewCellStyle != null && !dataGridViewCellStyle.ForeColor.IsEmpty)
				{
					dataGridViewCellStyle2.ForeColor = dataGridViewCellStyle.ForeColor;
				}
				else
				{
					dataGridViewCellStyle2.ForeColor = defaultCellStyle.ForeColor;
				}
				if (dataGridViewCellStyle != null && !dataGridViewCellStyle.SelectionBackColor.IsEmpty)
				{
					dataGridViewCellStyle2.SelectionBackColor = dataGridViewCellStyle.SelectionBackColor;
				}
				else
				{
					dataGridViewCellStyle2.SelectionBackColor = defaultCellStyle.SelectionBackColor;
				}
				if (dataGridViewCellStyle != null && !dataGridViewCellStyle.SelectionForeColor.IsEmpty)
				{
					dataGridViewCellStyle2.SelectionForeColor = dataGridViewCellStyle.SelectionForeColor;
				}
				else
				{
					dataGridViewCellStyle2.SelectionForeColor = defaultCellStyle.SelectionForeColor;
				}
				if (dataGridViewCellStyle != null && dataGridViewCellStyle.Font != null)
				{
					dataGridViewCellStyle2.Font = dataGridViewCellStyle.Font;
				}
				else
				{
					dataGridViewCellStyle2.Font = defaultCellStyle.Font;
				}
				if (dataGridViewCellStyle != null && !dataGridViewCellStyle.IsNullValueDefault)
				{
					dataGridViewCellStyle2.NullValue = dataGridViewCellStyle.NullValue;
				}
				else
				{
					dataGridViewCellStyle2.NullValue = defaultCellStyle.NullValue;
				}
				if (dataGridViewCellStyle != null && !dataGridViewCellStyle.IsDataSourceNullValueDefault)
				{
					dataGridViewCellStyle2.DataSourceNullValue = dataGridViewCellStyle.DataSourceNullValue;
				}
				else
				{
					dataGridViewCellStyle2.DataSourceNullValue = defaultCellStyle.DataSourceNullValue;
				}
				if (dataGridViewCellStyle != null && dataGridViewCellStyle.Format.Length != 0)
				{
					dataGridViewCellStyle2.Format = dataGridViewCellStyle.Format;
				}
				else
				{
					dataGridViewCellStyle2.Format = defaultCellStyle.Format;
				}
				if (dataGridViewCellStyle != null && !dataGridViewCellStyle.IsFormatProviderDefault)
				{
					dataGridViewCellStyle2.FormatProvider = dataGridViewCellStyle.FormatProvider;
				}
				else
				{
					dataGridViewCellStyle2.FormatProvider = defaultCellStyle.FormatProvider;
				}
				if (dataGridViewCellStyle != null && dataGridViewCellStyle.Alignment != DataGridViewContentAlignment.NotSet)
				{
					dataGridViewCellStyle2.AlignmentInternal = dataGridViewCellStyle.Alignment;
				}
				else
				{
					dataGridViewCellStyle2.AlignmentInternal = defaultCellStyle.Alignment;
				}
				if (dataGridViewCellStyle != null && dataGridViewCellStyle.WrapMode != DataGridViewTriState.NotSet)
				{
					dataGridViewCellStyle2.WrapModeInternal = dataGridViewCellStyle.WrapMode;
				}
				else
				{
					dataGridViewCellStyle2.WrapModeInternal = defaultCellStyle.WrapMode;
				}
				if (dataGridViewCellStyle != null && dataGridViewCellStyle.Tag != null)
				{
					dataGridViewCellStyle2.Tag = dataGridViewCellStyle.Tag;
				}
				else
				{
					dataGridViewCellStyle2.Tag = defaultCellStyle.Tag;
				}
				if (dataGridViewCellStyle != null && dataGridViewCellStyle.Padding != Padding.Empty)
				{
					dataGridViewCellStyle2.PaddingInternal = dataGridViewCellStyle.Padding;
				}
				else
				{
					dataGridViewCellStyle2.PaddingInternal = defaultCellStyle.Padding;
				}
				return dataGridViewCellStyle2;
			}
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x060032D6 RID: 13014 RVA: 0x000B2816 File Offset: 0x000B1816
		// (set) Token: 0x060032D7 RID: 13015 RVA: 0x000B2826 File Offset: 0x000B1826
		internal bool IsBrowsableInternal
		{
			get
			{
				return (this.flags & 8) != 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 8;
					return;
				}
				this.flags = (byte)((int)this.flags & -9);
			}
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x060032D8 RID: 13016 RVA: 0x000B284B File Offset: 0x000B184B
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool IsDataBound
		{
			get
			{
				return this.IsDataBoundInternal;
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x060032D9 RID: 13017 RVA: 0x000B2853 File Offset: 0x000B1853
		// (set) Token: 0x060032DA RID: 13018 RVA: 0x000B2863 File Offset: 0x000B1863
		internal bool IsDataBoundInternal
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

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x060032DB RID: 13019 RVA: 0x000B2888 File Offset: 0x000B1888
		// (set) Token: 0x060032DC RID: 13020 RVA: 0x000B2890 File Offset: 0x000B1890
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("DataGridView_ColumnMinimumWidthDescr")]
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[DefaultValue(5)]
		public int MinimumWidth
		{
			get
			{
				return base.MinimumThickness;
			}
			set
			{
				base.MinimumThickness = value;
			}
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x060032DD RID: 13021 RVA: 0x000B2899 File Offset: 0x000B1899
		// (set) Token: 0x060032DE RID: 13022 RVA: 0x000B28CC File Offset: 0x000B18CC
		[Browsable(false)]
		public string Name
		{
			get
			{
				if (this.Site != null && !string.IsNullOrEmpty(this.Site.Name))
				{
					this.name = this.Site.Name;
				}
				return this.name;
			}
			set
			{
				string text = this.name;
				if (string.IsNullOrEmpty(value))
				{
					this.name = string.Empty;
				}
				else
				{
					this.name = value;
				}
				if (base.DataGridView != null && !string.Equals(this.name, text, StringComparison.Ordinal))
				{
					base.DataGridView.OnColumnNameChanged(this);
				}
			}
		}

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x060032DF RID: 13023 RVA: 0x000B291F File Offset: 0x000B191F
		// (set) Token: 0x060032E0 RID: 13024 RVA: 0x000B2928 File Offset: 0x000B1928
		[SRDescription("DataGridView_ColumnReadOnlyDescr")]
		[SRCategory("CatBehavior")]
		public override bool ReadOnly
		{
			get
			{
				return base.ReadOnly;
			}
			set
			{
				if (this.IsDataBound && base.DataGridView != null && base.DataGridView.DataConnection != null && this.boundColumnIndex != -1 && base.DataGridView.DataConnection.DataFieldIsReadOnly(this.boundColumnIndex) && !value)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_ColumnBoundToAReadOnlyFieldMustRemainReadOnly"));
				}
				base.ReadOnly = value;
			}
		}

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x060032E1 RID: 13025 RVA: 0x000B298D File Offset: 0x000B198D
		// (set) Token: 0x060032E2 RID: 13026 RVA: 0x000B2995 File Offset: 0x000B1995
		[SRDescription("DataGridView_ColumnResizableDescr")]
		[SRCategory("CatBehavior")]
		public override DataGridViewTriState Resizable
		{
			get
			{
				return base.Resizable;
			}
			set
			{
				base.Resizable = value;
			}
		}

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x060032E3 RID: 13027 RVA: 0x000B299E File Offset: 0x000B199E
		// (set) Token: 0x060032E4 RID: 13028 RVA: 0x000B29A6 File Offset: 0x000B19A6
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ISite Site
		{
			get
			{
				return this.site;
			}
			set
			{
				this.site = value;
			}
		}

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x060032E5 RID: 13029 RVA: 0x000B29AF File Offset: 0x000B19AF
		// (set) Token: 0x060032E6 RID: 13030 RVA: 0x000B29CC File Offset: 0x000B19CC
		[SRCategory("CatBehavior")]
		[DefaultValue(DataGridViewColumnSortMode.NotSortable)]
		[SRDescription("DataGridView_ColumnSortModeDescr")]
		public DataGridViewColumnSortMode SortMode
		{
			get
			{
				if ((this.flags & 1) != 0)
				{
					return DataGridViewColumnSortMode.Automatic;
				}
				if ((this.flags & 2) != 0)
				{
					return DataGridViewColumnSortMode.Programmatic;
				}
				return DataGridViewColumnSortMode.NotSortable;
			}
			set
			{
				if (value != this.SortMode)
				{
					if (value != DataGridViewColumnSortMode.NotSortable)
					{
						if (base.DataGridView != null && !base.DataGridView.InInitialization && value == DataGridViewColumnSortMode.Automatic && (base.DataGridView.SelectionMode == DataGridViewSelectionMode.FullColumnSelect || base.DataGridView.SelectionMode == DataGridViewSelectionMode.ColumnHeaderSelect))
						{
							throw new InvalidOperationException(SR.GetString("DataGridViewColumn_SortModeAndSelectionModeClash", new object[]
							{
								value.ToString(),
								base.DataGridView.SelectionMode.ToString()
							}));
						}
						if (value == DataGridViewColumnSortMode.Automatic)
						{
							this.flags = (byte)((int)this.flags & -3);
							this.flags |= 1;
						}
						else
						{
							this.flags = (byte)((int)this.flags & -2);
							this.flags |= 2;
						}
					}
					else
					{
						this.flags = (byte)((int)this.flags & -2);
						this.flags = (byte)((int)this.flags & -3);
					}
					if (base.DataGridView != null)
					{
						base.DataGridView.OnColumnSortModeChanged(this);
					}
				}
			}
		}

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x060032E7 RID: 13031 RVA: 0x000B2AD8 File Offset: 0x000B1AD8
		// (set) Token: 0x060032E8 RID: 13032 RVA: 0x000B2AE5 File Offset: 0x000B1AE5
		[SRCategory("CatAppearance")]
		[DefaultValue("")]
		[Localizable(true)]
		[SRDescription("DataGridView_ColumnToolTipTextDescr")]
		public string ToolTipText
		{
			get
			{
				return this.HeaderCell.ToolTipText;
			}
			set
			{
				if (string.Compare(this.ToolTipText, value, false, CultureInfo.InvariantCulture) != 0)
				{
					this.HeaderCell.ToolTipText = value;
					if (base.DataGridView != null)
					{
						base.DataGridView.OnColumnToolTipTextChanged(this);
					}
				}
			}
		}

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x060032E9 RID: 13033 RVA: 0x000B2B1B File Offset: 0x000B1B1B
		// (set) Token: 0x060032EA RID: 13034 RVA: 0x000B2B23 File Offset: 0x000B1B23
		internal float UsedFillWeight
		{
			get
			{
				return this.usedFillWeight;
			}
			set
			{
				this.usedFillWeight = value;
			}
		}

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x060032EB RID: 13035 RVA: 0x000B2B2C File Offset: 0x000B1B2C
		// (set) Token: 0x060032EC RID: 13036 RVA: 0x000B2B43 File Offset: 0x000B1B43
		[Browsable(false)]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Type ValueType
		{
			get
			{
				return (Type)base.Properties.GetObject(DataGridViewColumn.PropDataGridViewColumnValueType);
			}
			set
			{
				base.Properties.SetObject(DataGridViewColumn.PropDataGridViewColumnValueType, value);
			}
		}

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x060032ED RID: 13037 RVA: 0x000B2B56 File Offset: 0x000B1B56
		// (set) Token: 0x060032EE RID: 13038 RVA: 0x000B2B5E File Offset: 0x000B1B5E
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_ColumnVisibleDescr")]
		[DefaultValue(true)]
		public override bool Visible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				base.Visible = value;
			}
		}

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x060032EF RID: 13039 RVA: 0x000B2B67 File Offset: 0x000B1B67
		// (set) Token: 0x060032F0 RID: 13040 RVA: 0x000B2B6F File Offset: 0x000B1B6F
		[SRCategory("CatLayout")]
		[SRDescription("DataGridView_ColumnWidthDescr")]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public int Width
		{
			get
			{
				return base.Thickness;
			}
			set
			{
				base.Thickness = value;
			}
		}

		// Token: 0x060032F1 RID: 13041 RVA: 0x000B2B78 File Offset: 0x000B1B78
		public override object Clone()
		{
			DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)Activator.CreateInstance(base.GetType());
			if (dataGridViewColumn != null)
			{
				this.CloneInternal(dataGridViewColumn);
			}
			return dataGridViewColumn;
		}

		// Token: 0x060032F2 RID: 13042 RVA: 0x000B2BA4 File Offset: 0x000B1BA4
		internal void CloneInternal(DataGridViewColumn dataGridViewColumn)
		{
			base.CloneInternal(dataGridViewColumn);
			dataGridViewColumn.name = this.Name;
			dataGridViewColumn.displayIndex = -1;
			dataGridViewColumn.HeaderText = this.HeaderText;
			dataGridViewColumn.DataPropertyName = this.DataPropertyName;
			if (dataGridViewColumn.CellTemplate != null)
			{
				dataGridViewColumn.cellTemplate = (DataGridViewCell)this.CellTemplate.Clone();
			}
			else
			{
				dataGridViewColumn.cellTemplate = null;
			}
			if (base.HasHeaderCell)
			{
				dataGridViewColumn.HeaderCell = (DataGridViewColumnHeaderCell)this.HeaderCell.Clone();
			}
			dataGridViewColumn.AutoSizeMode = this.AutoSizeMode;
			dataGridViewColumn.SortMode = this.SortMode;
			dataGridViewColumn.FillWeightInternal = this.FillWeight;
		}

		// Token: 0x060032F3 RID: 13043 RVA: 0x000B2C4C File Offset: 0x000B1C4C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					lock (this)
					{
						if (this.site != null && this.site.Container != null)
						{
							this.site.Container.Remove(this);
						}
						if (this.disposed != null)
						{
							this.disposed(this, EventArgs.Empty);
						}
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x060032F4 RID: 13044 RVA: 0x000B2CD4 File Offset: 0x000B1CD4
		internal DataGridViewAutoSizeColumnMode GetInheritedAutoSizeMode(DataGridView dataGridView)
		{
			if (dataGridView != null && this.autoSizeMode == DataGridViewAutoSizeColumnMode.NotSet)
			{
				DataGridViewAutoSizeColumnsMode autoSizeColumnsMode = dataGridView.AutoSizeColumnsMode;
				switch (autoSizeColumnsMode)
				{
				case DataGridViewAutoSizeColumnsMode.ColumnHeader:
					return DataGridViewAutoSizeColumnMode.ColumnHeader;
				case (DataGridViewAutoSizeColumnsMode)3:
				case (DataGridViewAutoSizeColumnsMode)5:
				case (DataGridViewAutoSizeColumnsMode)7:
				case (DataGridViewAutoSizeColumnsMode)9:
					break;
				case DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader:
					return DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
				case DataGridViewAutoSizeColumnsMode.AllCells:
					return DataGridViewAutoSizeColumnMode.AllCells;
				case DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader:
					return DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
				case DataGridViewAutoSizeColumnsMode.DisplayedCells:
					return DataGridViewAutoSizeColumnMode.DisplayedCells;
				default:
					if (autoSizeColumnsMode == DataGridViewAutoSizeColumnsMode.Fill)
					{
						return DataGridViewAutoSizeColumnMode.Fill;
					}
					break;
				}
				return DataGridViewAutoSizeColumnMode.None;
			}
			return this.autoSizeMode;
		}

		// Token: 0x060032F5 RID: 13045 RVA: 0x000B2D3C File Offset: 0x000B1D3C
		public virtual int GetPreferredWidth(DataGridViewAutoSizeColumnMode autoSizeColumnMode, bool fixedHeight)
		{
			if (autoSizeColumnMode == DataGridViewAutoSizeColumnMode.NotSet || autoSizeColumnMode == DataGridViewAutoSizeColumnMode.None || autoSizeColumnMode == DataGridViewAutoSizeColumnMode.Fill)
			{
				throw new ArgumentException(SR.GetString("DataGridView_NeedColumnAutoSizingCriteria", new object[] { "autoSizeColumnMode" }));
			}
			switch (autoSizeColumnMode)
			{
			case DataGridViewAutoSizeColumnMode.NotSet:
			case DataGridViewAutoSizeColumnMode.None:
			case DataGridViewAutoSizeColumnMode.ColumnHeader:
			case DataGridViewAutoSizeColumnMode.AllCellsExceptHeader:
			case DataGridViewAutoSizeColumnMode.AllCells:
			case DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader:
			case DataGridViewAutoSizeColumnMode.DisplayedCells:
				goto IL_0081;
			case (DataGridViewAutoSizeColumnMode)3:
			case (DataGridViewAutoSizeColumnMode)5:
			case (DataGridViewAutoSizeColumnMode)7:
			case (DataGridViewAutoSizeColumnMode)9:
				break;
			default:
				if (autoSizeColumnMode == DataGridViewAutoSizeColumnMode.Fill)
				{
					goto IL_0081;
				}
				break;
			}
			throw new InvalidEnumArgumentException("value", (int)autoSizeColumnMode, typeof(DataGridViewAutoSizeColumnMode));
			IL_0081:
			DataGridView dataGridView = base.DataGridView;
			if (dataGridView == null)
			{
				return -1;
			}
			int num = 0;
			if (dataGridView.ColumnHeadersVisible && (autoSizeColumnMode & DataGridViewAutoSizeColumnMode.ColumnHeader) != DataGridViewAutoSizeColumnMode.NotSet)
			{
				int num2;
				if (fixedHeight)
				{
					num2 = this.HeaderCell.GetPreferredWidth(-1, dataGridView.ColumnHeadersHeight);
				}
				else
				{
					num2 = this.HeaderCell.GetPreferredSize(-1).Width;
				}
				if (num < num2)
				{
					num = num2;
				}
			}
			if ((autoSizeColumnMode & DataGridViewAutoSizeColumnMode.AllCellsExceptHeader) != DataGridViewAutoSizeColumnMode.NotSet)
			{
				for (int num3 = dataGridView.Rows.GetFirstRow(DataGridViewElementStates.Visible); num3 != -1; num3 = dataGridView.Rows.GetNextRow(num3, DataGridViewElementStates.Visible))
				{
					DataGridViewRow dataGridViewRow = dataGridView.Rows.SharedRow(num3);
					int num2;
					if (fixedHeight)
					{
						num2 = dataGridViewRow.Cells[base.Index].GetPreferredWidth(num3, dataGridViewRow.Thickness);
					}
					else
					{
						num2 = dataGridViewRow.Cells[base.Index].GetPreferredSize(num3).Width;
					}
					if (num < num2)
					{
						num = num2;
					}
				}
			}
			else if ((autoSizeColumnMode & DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader) != DataGridViewAutoSizeColumnMode.NotSet)
			{
				int height = dataGridView.LayoutInfo.Data.Height;
				int num4 = 0;
				int num3 = dataGridView.Rows.GetFirstRow(DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible);
				while (num3 != -1 && num4 < height)
				{
					DataGridViewRow dataGridViewRow = dataGridView.Rows.SharedRow(num3);
					int num2;
					if (fixedHeight)
					{
						num2 = dataGridViewRow.Cells[base.Index].GetPreferredWidth(num3, dataGridViewRow.Thickness);
					}
					else
					{
						num2 = dataGridViewRow.Cells[base.Index].GetPreferredSize(num3).Width;
					}
					if (num < num2)
					{
						num = num2;
					}
					num4 += dataGridViewRow.Thickness;
					num3 = dataGridView.Rows.GetNextRow(num3, DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible);
				}
				if (num4 < height)
				{
					num3 = dataGridView.DisplayedBandsInfo.FirstDisplayedScrollingRow;
					while (num3 != -1 && num4 < height)
					{
						DataGridViewRow dataGridViewRow = dataGridView.Rows.SharedRow(num3);
						int num2;
						if (fixedHeight)
						{
							num2 = dataGridViewRow.Cells[base.Index].GetPreferredWidth(num3, dataGridViewRow.Thickness);
						}
						else
						{
							num2 = dataGridViewRow.Cells[base.Index].GetPreferredSize(num3).Width;
						}
						if (num < num2)
						{
							num = num2;
						}
						num4 += dataGridViewRow.Thickness;
						num3 = dataGridView.Rows.GetNextRow(num3, DataGridViewElementStates.Visible);
					}
				}
			}
			return num;
		}

		// Token: 0x060032F6 RID: 13046 RVA: 0x000B3008 File Offset: 0x000B2008
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.Append("DataGridViewColumn { Name=");
			stringBuilder.Append(this.Name);
			stringBuilder.Append(", Index=");
			stringBuilder.Append(base.Index.ToString(CultureInfo.CurrentCulture));
			stringBuilder.Append(" }");
			return stringBuilder.ToString();
		}

		// Token: 0x04001A7D RID: 6781
		private const float DATAGRIDVIEWCOLUMN_defaultFillWeight = 100f;

		// Token: 0x04001A7E RID: 6782
		private const int DATAGRIDVIEWCOLUMN_defaultWidth = 100;

		// Token: 0x04001A7F RID: 6783
		private const int DATAGRIDVIEWCOLUMN_defaultMinColumnThickness = 5;

		// Token: 0x04001A80 RID: 6784
		private const byte DATAGRIDVIEWCOLUMN_automaticSort = 1;

		// Token: 0x04001A81 RID: 6785
		private const byte DATAGRIDVIEWCOLUMN_programmaticSort = 2;

		// Token: 0x04001A82 RID: 6786
		private const byte DATAGRIDVIEWCOLUMN_isDataBound = 4;

		// Token: 0x04001A83 RID: 6787
		private const byte DATAGRIDVIEWCOLUMN_isBrowsableInternal = 8;

		// Token: 0x04001A84 RID: 6788
		private const byte DATAGRIDVIEWCOLUMN_displayIndexHasChangedInternal = 16;

		// Token: 0x04001A85 RID: 6789
		private byte flags;

		// Token: 0x04001A86 RID: 6790
		private DataGridViewCell cellTemplate;

		// Token: 0x04001A87 RID: 6791
		private string name;

		// Token: 0x04001A88 RID: 6792
		private int displayIndex;

		// Token: 0x04001A89 RID: 6793
		private int desiredFillWidth;

		// Token: 0x04001A8A RID: 6794
		private int desiredMinimumWidth;

		// Token: 0x04001A8B RID: 6795
		private float fillWeight;

		// Token: 0x04001A8C RID: 6796
		private float usedFillWeight;

		// Token: 0x04001A8D RID: 6797
		private DataGridViewAutoSizeColumnMode autoSizeMode;

		// Token: 0x04001A8E RID: 6798
		private int boundColumnIndex = -1;

		// Token: 0x04001A8F RID: 6799
		private string dataPropertyName = string.Empty;

		// Token: 0x04001A90 RID: 6800
		private TypeConverter boundColumnConverter;

		// Token: 0x04001A91 RID: 6801
		private ISite site;

		// Token: 0x04001A92 RID: 6802
		private EventHandler disposed;

		// Token: 0x04001A93 RID: 6803
		private static readonly int PropDataGridViewColumnValueType = PropertyStore.CreateKey();
	}
}
