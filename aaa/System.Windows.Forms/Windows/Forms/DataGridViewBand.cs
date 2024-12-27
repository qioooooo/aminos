using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x02000306 RID: 774
	public class DataGridViewBand : DataGridViewElement, ICloneable, IDisposable
	{
		// Token: 0x06003156 RID: 12630 RVA: 0x000A9974 File Offset: 0x000A8974
		internal DataGridViewBand()
		{
			this.propertyStore = new PropertyStore();
			this.bandIndex = -1;
		}

		// Token: 0x06003157 RID: 12631 RVA: 0x000A9990 File Offset: 0x000A8990
		~DataGridViewBand()
		{
			this.Dispose(false);
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06003158 RID: 12632 RVA: 0x000A99C0 File Offset: 0x000A89C0
		// (set) Token: 0x06003159 RID: 12633 RVA: 0x000A99C8 File Offset: 0x000A89C8
		internal int CachedThickness
		{
			get
			{
				return this.cachedThickness;
			}
			set
			{
				this.cachedThickness = value;
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x0600315A RID: 12634 RVA: 0x000A99D1 File Offset: 0x000A89D1
		// (set) Token: 0x0600315B RID: 12635 RVA: 0x000A99F3 File Offset: 0x000A89F3
		[DefaultValue(null)]
		public virtual ContextMenuStrip ContextMenuStrip
		{
			get
			{
				if (this.bandIsRow)
				{
					return ((DataGridViewRow)this).GetContextMenuStrip(this.Index);
				}
				return this.ContextMenuStripInternal;
			}
			set
			{
				this.ContextMenuStripInternal = value;
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x0600315C RID: 12636 RVA: 0x000A99FC File Offset: 0x000A89FC
		// (set) Token: 0x0600315D RID: 12637 RVA: 0x000A9A14 File Offset: 0x000A8A14
		internal ContextMenuStrip ContextMenuStripInternal
		{
			get
			{
				return (ContextMenuStrip)this.Properties.GetObject(DataGridViewBand.PropContextMenuStrip);
			}
			set
			{
				ContextMenuStrip contextMenuStrip = (ContextMenuStrip)this.Properties.GetObject(DataGridViewBand.PropContextMenuStrip);
				if (contextMenuStrip != value)
				{
					EventHandler eventHandler = new EventHandler(this.DetachContextMenuStrip);
					if (contextMenuStrip != null)
					{
						contextMenuStrip.Disposed -= eventHandler;
					}
					this.Properties.SetObject(DataGridViewBand.PropContextMenuStrip, value);
					if (value != null)
					{
						value.Disposed += eventHandler;
					}
					if (base.DataGridView != null)
					{
						base.DataGridView.OnBandContextMenuStripChanged(this);
					}
				}
			}
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x0600315E RID: 12638 RVA: 0x000A9A84 File Offset: 0x000A8A84
		// (set) Token: 0x0600315F RID: 12639 RVA: 0x000A9ADC File Offset: 0x000A8ADC
		[Browsable(false)]
		public virtual DataGridViewCellStyle DefaultCellStyle
		{
			get
			{
				DataGridViewCellStyle dataGridViewCellStyle = (DataGridViewCellStyle)this.Properties.GetObject(DataGridViewBand.PropDefaultCellStyle);
				if (dataGridViewCellStyle == null)
				{
					dataGridViewCellStyle = new DataGridViewCellStyle();
					dataGridViewCellStyle.AddScope(base.DataGridView, this.bandIsRow ? DataGridViewCellStyleScopes.Row : DataGridViewCellStyleScopes.Column);
					this.Properties.SetObject(DataGridViewBand.PropDefaultCellStyle, dataGridViewCellStyle);
				}
				return dataGridViewCellStyle;
			}
			set
			{
				DataGridViewCellStyle dataGridViewCellStyle = null;
				if (this.HasDefaultCellStyle)
				{
					dataGridViewCellStyle = this.DefaultCellStyle;
					dataGridViewCellStyle.RemoveScope(this.bandIsRow ? DataGridViewCellStyleScopes.Row : DataGridViewCellStyleScopes.Column);
				}
				if (value != null || this.Properties.ContainsObject(DataGridViewBand.PropDefaultCellStyle))
				{
					if (value != null)
					{
						value.AddScope(base.DataGridView, this.bandIsRow ? DataGridViewCellStyleScopes.Row : DataGridViewCellStyleScopes.Column);
					}
					this.Properties.SetObject(DataGridViewBand.PropDefaultCellStyle, value);
				}
				if (((dataGridViewCellStyle != null && value == null) || (dataGridViewCellStyle == null && value != null) || (dataGridViewCellStyle != null && value != null && !dataGridViewCellStyle.Equals(this.DefaultCellStyle))) && base.DataGridView != null)
				{
					base.DataGridView.OnBandDefaultCellStyleChanged(this);
				}
			}
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06003160 RID: 12640 RVA: 0x000A9B84 File Offset: 0x000A8B84
		// (set) Token: 0x06003161 RID: 12641 RVA: 0x000A9BCC File Offset: 0x000A8BCC
		[Browsable(false)]
		public Type DefaultHeaderCellType
		{
			get
			{
				Type type = (Type)this.Properties.GetObject(DataGridViewBand.PropDefaultHeaderCellType);
				if (type == null)
				{
					if (this.bandIsRow)
					{
						type = typeof(DataGridViewRowHeaderCell);
					}
					else
					{
						type = typeof(DataGridViewColumnHeaderCell);
					}
				}
				return type;
			}
			set
			{
				if (value == null && !this.Properties.ContainsObject(DataGridViewBand.PropDefaultHeaderCellType))
				{
					return;
				}
				if (Type.GetType("System.Windows.Forms.DataGridViewHeaderCell").IsAssignableFrom(value))
				{
					this.Properties.SetObject(DataGridViewBand.PropDefaultHeaderCellType, value);
					return;
				}
				throw new ArgumentException(SR.GetString("DataGridView_WrongType", new object[] { "DefaultHeaderCellType", "System.Windows.Forms.DataGridViewHeaderCell" }));
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06003162 RID: 12642 RVA: 0x000A9C3C File Offset: 0x000A8C3C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public virtual bool Displayed
		{
			get
			{
				return (this.State & DataGridViewElementStates.Displayed) != DataGridViewElementStates.None;
			}
		}

		// Token: 0x17000873 RID: 2163
		// (set) Token: 0x06003163 RID: 12643 RVA: 0x000A9C59 File Offset: 0x000A8C59
		internal bool DisplayedInternal
		{
			set
			{
				if (value)
				{
					base.StateInternal = this.State | DataGridViewElementStates.Displayed;
				}
				else
				{
					base.StateInternal = this.State & ~DataGridViewElementStates.Displayed;
				}
				if (base.DataGridView != null)
				{
					this.OnStateChanged(DataGridViewElementStates.Displayed);
				}
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06003164 RID: 12644 RVA: 0x000A9C8C File Offset: 0x000A8C8C
		// (set) Token: 0x06003165 RID: 12645 RVA: 0x000A9CB4 File Offset: 0x000A8CB4
		internal int DividerThickness
		{
			get
			{
				bool flag;
				int integer = this.Properties.GetInteger(DataGridViewBand.PropDividerThickness, out flag);
				if (!flag)
				{
					return 0;
				}
				return integer;
			}
			set
			{
				if (value < 0)
				{
					if (this.bandIsRow)
					{
						throw new ArgumentOutOfRangeException("DividerHeight", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"DividerHeight",
							value.ToString(CultureInfo.CurrentCulture),
							0.ToString(CultureInfo.CurrentCulture)
						}));
					}
					throw new ArgumentOutOfRangeException("DividerWidth", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"DividerWidth",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				else
				{
					if (value <= 65536)
					{
						if (value != this.DividerThickness)
						{
							this.Properties.SetInteger(DataGridViewBand.PropDividerThickness, value);
							if (base.DataGridView != null)
							{
								base.DataGridView.OnBandDividerThicknessChanged(this);
							}
						}
						return;
					}
					if (this.bandIsRow)
					{
						throw new ArgumentOutOfRangeException("DividerHeight", SR.GetString("InvalidHighBoundArgumentEx", new object[]
						{
							"DividerHeight",
							value.ToString(CultureInfo.CurrentCulture),
							65536.ToString(CultureInfo.CurrentCulture)
						}));
					}
					throw new ArgumentOutOfRangeException("DividerWidth", SR.GetString("InvalidHighBoundArgumentEx", new object[]
					{
						"DividerWidth",
						value.ToString(CultureInfo.CurrentCulture),
						65536.ToString(CultureInfo.CurrentCulture)
					}));
				}
			}
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x06003166 RID: 12646 RVA: 0x000A9E39 File Offset: 0x000A8E39
		// (set) Token: 0x06003167 RID: 12647 RVA: 0x000A9E4C File Offset: 0x000A8E4C
		[DefaultValue(false)]
		public virtual bool Frozen
		{
			get
			{
				return (this.State & DataGridViewElementStates.Frozen) != DataGridViewElementStates.None;
			}
			set
			{
				if ((this.State & DataGridViewElementStates.Frozen) != DataGridViewElementStates.None != value)
				{
					this.OnStateChanging(DataGridViewElementStates.Frozen);
					if (value)
					{
						base.StateInternal = this.State | DataGridViewElementStates.Frozen;
					}
					else
					{
						base.StateInternal = this.State & ~DataGridViewElementStates.Frozen;
					}
					this.OnStateChanged(DataGridViewElementStates.Frozen);
				}
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06003168 RID: 12648 RVA: 0x000A9E9A File Offset: 0x000A8E9A
		[Browsable(false)]
		public bool HasDefaultCellStyle
		{
			get
			{
				return this.Properties.ContainsObject(DataGridViewBand.PropDefaultCellStyle) && this.Properties.GetObject(DataGridViewBand.PropDefaultCellStyle) != null;
			}
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06003169 RID: 12649 RVA: 0x000A9EC6 File Offset: 0x000A8EC6
		internal bool HasDefaultHeaderCellType
		{
			get
			{
				return this.Properties.ContainsObject(DataGridViewBand.PropDefaultHeaderCellType) && this.Properties.GetObject(DataGridViewBand.PropDefaultHeaderCellType) != null;
			}
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x0600316A RID: 12650 RVA: 0x000A9EF2 File Offset: 0x000A8EF2
		internal bool HasHeaderCell
		{
			get
			{
				return this.Properties.ContainsObject(DataGridViewBand.PropHeaderCell) && this.Properties.GetObject(DataGridViewBand.PropHeaderCell) != null;
			}
		}

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x0600316B RID: 12651 RVA: 0x000A9F20 File Offset: 0x000A8F20
		// (set) Token: 0x0600316C RID: 12652 RVA: 0x000A9FE0 File Offset: 0x000A8FE0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected DataGridViewHeaderCell HeaderCellCore
		{
			get
			{
				DataGridViewHeaderCell dataGridViewHeaderCell = (DataGridViewHeaderCell)this.Properties.GetObject(DataGridViewBand.PropHeaderCell);
				if (dataGridViewHeaderCell == null)
				{
					Type defaultHeaderCellType = this.DefaultHeaderCellType;
					dataGridViewHeaderCell = (DataGridViewHeaderCell)SecurityUtils.SecureCreateInstance(defaultHeaderCellType);
					dataGridViewHeaderCell.DataGridViewInternal = base.DataGridView;
					if (this.bandIsRow)
					{
						dataGridViewHeaderCell.OwningRowInternal = (DataGridViewRow)this;
						this.Properties.SetObject(DataGridViewBand.PropHeaderCell, dataGridViewHeaderCell);
					}
					else
					{
						DataGridViewColumn dataGridViewColumn = this as DataGridViewColumn;
						dataGridViewHeaderCell.OwningColumnInternal = dataGridViewColumn;
						this.Properties.SetObject(DataGridViewBand.PropHeaderCell, dataGridViewHeaderCell);
						if (base.DataGridView != null && base.DataGridView.SortedColumn == dataGridViewColumn)
						{
							DataGridViewColumnHeaderCell dataGridViewColumnHeaderCell = dataGridViewHeaderCell as DataGridViewColumnHeaderCell;
							dataGridViewColumnHeaderCell.SortGlyphDirection = base.DataGridView.SortOrder;
						}
					}
				}
				return dataGridViewHeaderCell;
			}
			set
			{
				DataGridViewHeaderCell dataGridViewHeaderCell = (DataGridViewHeaderCell)this.Properties.GetObject(DataGridViewBand.PropHeaderCell);
				if (value != null || this.Properties.ContainsObject(DataGridViewBand.PropHeaderCell))
				{
					if (dataGridViewHeaderCell != null)
					{
						dataGridViewHeaderCell.DataGridViewInternal = null;
						if (this.bandIsRow)
						{
							dataGridViewHeaderCell.OwningRowInternal = null;
						}
						else
						{
							dataGridViewHeaderCell.OwningColumnInternal = null;
							((DataGridViewColumnHeaderCell)dataGridViewHeaderCell).SortGlyphDirectionInternal = SortOrder.None;
						}
					}
					if (value != null)
					{
						if (this.bandIsRow)
						{
							if (!(value is DataGridViewRowHeaderCell))
							{
								throw new ArgumentException(SR.GetString("DataGridView_WrongType", new object[] { "HeaderCell", "System.Windows.Forms.DataGridViewRowHeaderCell" }));
							}
							if (value.OwningRow != null)
							{
								value.OwningRow.HeaderCell = null;
							}
							value.OwningRowInternal = (DataGridViewRow)this;
						}
						else
						{
							if (!(value is DataGridViewColumnHeaderCell))
							{
								throw new ArgumentException(SR.GetString("DataGridView_WrongType", new object[] { "HeaderCell", "System.Windows.Forms.DataGridViewColumnHeaderCell" }));
							}
							if (value.OwningColumn != null)
							{
								value.OwningColumn.HeaderCell = null;
							}
							value.OwningColumnInternal = (DataGridViewColumn)this;
						}
						value.DataGridViewInternal = base.DataGridView;
					}
					this.Properties.SetObject(DataGridViewBand.PropHeaderCell, value);
				}
				if (((value == null && dataGridViewHeaderCell != null) || (value != null && dataGridViewHeaderCell == null) || (value != null && dataGridViewHeaderCell != null && !dataGridViewHeaderCell.Equals(value))) && base.DataGridView != null)
				{
					base.DataGridView.OnBandHeaderCellChanged(this);
				}
			}
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x0600316D RID: 12653 RVA: 0x000AA147 File Offset: 0x000A9147
		[Browsable(false)]
		public int Index
		{
			get
			{
				return this.bandIndex;
			}
		}

		// Token: 0x1700087B RID: 2171
		// (set) Token: 0x0600316E RID: 12654 RVA: 0x000AA14F File Offset: 0x000A914F
		internal int IndexInternal
		{
			set
			{
				this.bandIndex = value;
			}
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x0600316F RID: 12655 RVA: 0x000AA158 File Offset: 0x000A9158
		[Browsable(false)]
		public virtual DataGridViewCellStyle InheritedStyle
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06003170 RID: 12656 RVA: 0x000AA15B File Offset: 0x000A915B
		protected bool IsRow
		{
			get
			{
				return this.bandIsRow;
			}
		}

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x06003171 RID: 12657 RVA: 0x000AA164 File Offset: 0x000A9164
		// (set) Token: 0x06003172 RID: 12658 RVA: 0x000AA19C File Offset: 0x000A919C
		internal int MinimumThickness
		{
			get
			{
				if (this.bandIsRow && this.bandIndex > -1)
				{
					int num;
					int num2;
					this.GetHeightInfo(this.bandIndex, out num, out num2);
					return num2;
				}
				return this.minimumThickness;
			}
			set
			{
				if (this.minimumThickness != value)
				{
					if (value < 2)
					{
						if (this.bandIsRow)
						{
							throw new ArgumentOutOfRangeException("MinimumHeight", value, SR.GetString("DataGridViewBand_MinimumHeightSmallerThanOne", new object[] { 2.ToString(CultureInfo.CurrentCulture) }));
						}
						throw new ArgumentOutOfRangeException("MinimumWidth", value, SR.GetString("DataGridViewBand_MinimumWidthSmallerThanOne", new object[] { 2.ToString(CultureInfo.CurrentCulture) }));
					}
					else
					{
						if (this.Thickness < value)
						{
							if (base.DataGridView != null && !this.bandIsRow)
							{
								base.DataGridView.OnColumnMinimumWidthChanging((DataGridViewColumn)this, value);
							}
							this.Thickness = value;
						}
						this.minimumThickness = value;
						if (base.DataGridView != null)
						{
							base.DataGridView.OnBandMinimumThicknessChanged(this);
						}
					}
				}
			}
		}

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x06003173 RID: 12659 RVA: 0x000AA276 File Offset: 0x000A9276
		internal PropertyStore Properties
		{
			get
			{
				return this.propertyStore;
			}
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x06003174 RID: 12660 RVA: 0x000AA27E File Offset: 0x000A927E
		// (set) Token: 0x06003175 RID: 12661 RVA: 0x000AA2A4 File Offset: 0x000A92A4
		[DefaultValue(false)]
		public virtual bool ReadOnly
		{
			get
			{
				return (this.State & DataGridViewElementStates.ReadOnly) != DataGridViewElementStates.None || (base.DataGridView != null && base.DataGridView.ReadOnly);
			}
			set
			{
				if (base.DataGridView == null)
				{
					if ((this.State & DataGridViewElementStates.ReadOnly) != DataGridViewElementStates.None != value)
					{
						if (value)
						{
							if (this.bandIsRow)
							{
								foreach (object obj in ((DataGridViewRow)this).Cells)
								{
									DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
									if (dataGridViewCell.ReadOnly)
									{
										dataGridViewCell.ReadOnlyInternal = false;
									}
								}
							}
							base.StateInternal = this.State | DataGridViewElementStates.ReadOnly;
							return;
						}
						base.StateInternal = this.State & ~DataGridViewElementStates.ReadOnly;
					}
					return;
				}
				if (base.DataGridView.ReadOnly)
				{
					return;
				}
				if (!this.bandIsRow)
				{
					this.OnStateChanging(DataGridViewElementStates.ReadOnly);
					base.DataGridView.SetReadOnlyColumnCore(this.bandIndex, value);
					return;
				}
				if (this.bandIndex == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidPropertySetOnSharedRow", new object[] { "ReadOnly" }));
				}
				this.OnStateChanging(DataGridViewElementStates.ReadOnly);
				base.DataGridView.SetReadOnlyRowCore(this.bandIndex, value);
			}
		}

		// Token: 0x17000881 RID: 2177
		// (set) Token: 0x06003176 RID: 12662 RVA: 0x000AA3C0 File Offset: 0x000A93C0
		internal bool ReadOnlyInternal
		{
			set
			{
				if (value)
				{
					base.StateInternal = this.State | DataGridViewElementStates.ReadOnly;
				}
				else
				{
					base.StateInternal = this.State & ~DataGridViewElementStates.ReadOnly;
				}
				this.OnStateChanged(DataGridViewElementStates.ReadOnly);
			}
		}

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06003177 RID: 12663 RVA: 0x000AA3EB File Offset: 0x000A93EB
		// (set) Token: 0x06003178 RID: 12664 RVA: 0x000AA420 File Offset: 0x000A9420
		[Browsable(true)]
		public virtual DataGridViewTriState Resizable
		{
			get
			{
				if ((this.State & DataGridViewElementStates.ResizableSet) != DataGridViewElementStates.None)
				{
					if ((this.State & DataGridViewElementStates.Resizable) == DataGridViewElementStates.None)
					{
						return DataGridViewTriState.False;
					}
					return DataGridViewTriState.True;
				}
				else
				{
					if (base.DataGridView == null)
					{
						return DataGridViewTriState.NotSet;
					}
					if (!base.DataGridView.AllowUserToResizeColumns)
					{
						return DataGridViewTriState.False;
					}
					return DataGridViewTriState.True;
				}
			}
			set
			{
				DataGridViewTriState resizable = this.Resizable;
				if (value == DataGridViewTriState.NotSet)
				{
					base.StateInternal = this.State & ~DataGridViewElementStates.ResizableSet;
				}
				else
				{
					base.StateInternal = this.State | DataGridViewElementStates.ResizableSet;
					if ((this.State & DataGridViewElementStates.Resizable) != DataGridViewElementStates.None != (value == DataGridViewTriState.True))
					{
						if (value == DataGridViewTriState.True)
						{
							base.StateInternal = this.State | DataGridViewElementStates.Resizable;
						}
						else
						{
							base.StateInternal = this.State & ~DataGridViewElementStates.Resizable;
						}
					}
				}
				if (resizable != this.Resizable)
				{
					this.OnStateChanged(DataGridViewElementStates.Resizable);
				}
			}
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x06003179 RID: 12665 RVA: 0x000AA49E File Offset: 0x000A949E
		// (set) Token: 0x0600317A RID: 12666 RVA: 0x000AA4B0 File Offset: 0x000A94B0
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public virtual bool Selected
		{
			get
			{
				return (this.State & DataGridViewElementStates.Selected) != DataGridViewElementStates.None;
			}
			set
			{
				if (base.DataGridView != null)
				{
					if (this.bandIsRow)
					{
						if (this.bandIndex == -1)
						{
							throw new InvalidOperationException(SR.GetString("DataGridView_InvalidPropertySetOnSharedRow", new object[] { "Selected" }));
						}
						if (base.DataGridView.SelectionMode == DataGridViewSelectionMode.FullRowSelect || base.DataGridView.SelectionMode == DataGridViewSelectionMode.RowHeaderSelect)
						{
							base.DataGridView.SetSelectedRowCoreInternal(this.bandIndex, value);
							return;
						}
					}
					else if (base.DataGridView.SelectionMode == DataGridViewSelectionMode.FullColumnSelect || base.DataGridView.SelectionMode == DataGridViewSelectionMode.ColumnHeaderSelect)
					{
						base.DataGridView.SetSelectedColumnCoreInternal(this.bandIndex, value);
						return;
					}
				}
				else if (value)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewBand_CannotSelect"));
				}
			}
		}

		// Token: 0x17000884 RID: 2180
		// (set) Token: 0x0600317B RID: 12667 RVA: 0x000AA56A File Offset: 0x000A956A
		internal bool SelectedInternal
		{
			set
			{
				if (value)
				{
					base.StateInternal = this.State | DataGridViewElementStates.Selected;
				}
				else
				{
					base.StateInternal = this.State & ~DataGridViewElementStates.Selected;
				}
				if (base.DataGridView != null)
				{
					this.OnStateChanged(DataGridViewElementStates.Selected);
				}
			}
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x0600317C RID: 12668 RVA: 0x000AA59F File Offset: 0x000A959F
		// (set) Token: 0x0600317D RID: 12669 RVA: 0x000AA5B1 File Offset: 0x000A95B1
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object Tag
		{
			get
			{
				return this.Properties.GetObject(DataGridViewBand.PropUserData);
			}
			set
			{
				if (value != null || this.Properties.ContainsObject(DataGridViewBand.PropUserData))
				{
					this.Properties.SetObject(DataGridViewBand.PropUserData, value);
				}
			}
		}

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x0600317E RID: 12670 RVA: 0x000AA5DC File Offset: 0x000A95DC
		// (set) Token: 0x0600317F RID: 12671 RVA: 0x000AA614 File Offset: 0x000A9614
		internal int Thickness
		{
			get
			{
				if (this.bandIsRow && this.bandIndex > -1)
				{
					int num;
					int num2;
					this.GetHeightInfo(this.bandIndex, out num, out num2);
					return num;
				}
				return this.thickness;
			}
			set
			{
				int num = this.MinimumThickness;
				if (value < num)
				{
					value = num;
				}
				if (value <= 65536)
				{
					bool flag = true;
					if (this.bandIsRow)
					{
						if (base.DataGridView != null && base.DataGridView.AutoSizeRowsMode != DataGridViewAutoSizeRowsMode.None)
						{
							this.cachedThickness = value;
							flag = false;
						}
					}
					else
					{
						DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)this;
						DataGridViewAutoSizeColumnMode inheritedAutoSizeMode = dataGridViewColumn.InheritedAutoSizeMode;
						if (inheritedAutoSizeMode != DataGridViewAutoSizeColumnMode.Fill && inheritedAutoSizeMode != DataGridViewAutoSizeColumnMode.None && inheritedAutoSizeMode != DataGridViewAutoSizeColumnMode.NotSet)
						{
							this.cachedThickness = value;
							flag = false;
						}
						else if (inheritedAutoSizeMode == DataGridViewAutoSizeColumnMode.Fill && base.DataGridView != null && dataGridViewColumn.Visible)
						{
							IntPtr handle = base.DataGridView.Handle;
							base.DataGridView.AdjustFillingColumn(dataGridViewColumn, value);
							flag = false;
						}
					}
					if (flag && this.thickness != value)
					{
						if (base.DataGridView != null)
						{
							base.DataGridView.OnBandThicknessChanging();
						}
						this.ThicknessInternal = value;
					}
					return;
				}
				if (this.bandIsRow)
				{
					throw new ArgumentOutOfRangeException("Height", SR.GetString("InvalidHighBoundArgumentEx", new object[]
					{
						"Height",
						value.ToString(CultureInfo.CurrentCulture),
						65536.ToString(CultureInfo.CurrentCulture)
					}));
				}
				throw new ArgumentOutOfRangeException("Width", SR.GetString("InvalidHighBoundArgumentEx", new object[]
				{
					"Width",
					value.ToString(CultureInfo.CurrentCulture),
					65536.ToString(CultureInfo.CurrentCulture)
				}));
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x06003180 RID: 12672 RVA: 0x000AA785 File Offset: 0x000A9785
		// (set) Token: 0x06003181 RID: 12673 RVA: 0x000AA78D File Offset: 0x000A978D
		internal int ThicknessInternal
		{
			get
			{
				return this.thickness;
			}
			set
			{
				this.thickness = value;
				if (base.DataGridView != null)
				{
					base.DataGridView.OnBandThicknessChanged(this);
				}
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06003182 RID: 12674 RVA: 0x000AA7AA File Offset: 0x000A97AA
		// (set) Token: 0x06003183 RID: 12675 RVA: 0x000AA7BC File Offset: 0x000A97BC
		[DefaultValue(true)]
		public virtual bool Visible
		{
			get
			{
				return (this.State & DataGridViewElementStates.Visible) != DataGridViewElementStates.None;
			}
			set
			{
				if ((this.State & DataGridViewElementStates.Visible) != DataGridViewElementStates.None != value)
				{
					if (base.DataGridView != null && this.bandIsRow && base.DataGridView.NewRowIndex != -1 && base.DataGridView.NewRowIndex == this.bandIndex && !value)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewBand_NewRowCannotBeInvisible"));
					}
					this.OnStateChanging(DataGridViewElementStates.Visible);
					if (value)
					{
						base.StateInternal = this.State | DataGridViewElementStates.Visible;
					}
					else
					{
						base.StateInternal = this.State & ~DataGridViewElementStates.Visible;
					}
					this.OnStateChanged(DataGridViewElementStates.Visible);
				}
			}
		}

		// Token: 0x06003184 RID: 12676 RVA: 0x000AA854 File Offset: 0x000A9854
		public virtual object Clone()
		{
			DataGridViewBand dataGridViewBand = (DataGridViewBand)Activator.CreateInstance(base.GetType());
			if (dataGridViewBand != null)
			{
				this.CloneInternal(dataGridViewBand);
			}
			return dataGridViewBand;
		}

		// Token: 0x06003185 RID: 12677 RVA: 0x000AA880 File Offset: 0x000A9880
		internal void CloneInternal(DataGridViewBand dataGridViewBand)
		{
			dataGridViewBand.propertyStore = new PropertyStore();
			dataGridViewBand.bandIndex = -1;
			dataGridViewBand.bandIsRow = this.bandIsRow;
			if (!this.bandIsRow || this.bandIndex >= 0 || base.DataGridView == null)
			{
				dataGridViewBand.StateInternal = this.State & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Selected);
			}
			dataGridViewBand.thickness = this.Thickness;
			dataGridViewBand.MinimumThickness = this.MinimumThickness;
			dataGridViewBand.cachedThickness = this.CachedThickness;
			dataGridViewBand.DividerThickness = this.DividerThickness;
			dataGridViewBand.Tag = this.Tag;
			if (this.HasDefaultCellStyle)
			{
				dataGridViewBand.DefaultCellStyle = new DataGridViewCellStyle(this.DefaultCellStyle);
			}
			if (this.HasDefaultHeaderCellType)
			{
				dataGridViewBand.DefaultHeaderCellType = this.DefaultHeaderCellType;
			}
			if (this.ContextMenuStripInternal != null)
			{
				dataGridViewBand.ContextMenuStrip = this.ContextMenuStripInternal.Clone();
			}
		}

		// Token: 0x06003186 RID: 12678 RVA: 0x000AA955 File Offset: 0x000A9955
		private void DetachContextMenuStrip(object sender, EventArgs e)
		{
			this.ContextMenuStripInternal = null;
		}

		// Token: 0x06003187 RID: 12679 RVA: 0x000AA95E File Offset: 0x000A995E
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003188 RID: 12680 RVA: 0x000AA970 File Offset: 0x000A9970
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				ContextMenuStrip contextMenuStripInternal = this.ContextMenuStripInternal;
				if (contextMenuStripInternal != null)
				{
					contextMenuStripInternal.Disposed -= this.DetachContextMenuStrip;
				}
			}
		}

		// Token: 0x06003189 RID: 12681 RVA: 0x000AA99C File Offset: 0x000A999C
		internal void GetHeightInfo(int rowIndex, out int height, out int minimumHeight)
		{
			if (base.DataGridView != null && (base.DataGridView.VirtualMode || base.DataGridView.DataSource != null) && base.DataGridView.AutoSizeRowsMode == DataGridViewAutoSizeRowsMode.None)
			{
				DataGridViewRowHeightInfoNeededEventArgs dataGridViewRowHeightInfoNeededEventArgs = base.DataGridView.OnRowHeightInfoNeeded(rowIndex, this.thickness, this.minimumThickness);
				height = dataGridViewRowHeightInfoNeededEventArgs.Height;
				minimumHeight = dataGridViewRowHeightInfoNeededEventArgs.MinimumHeight;
				return;
			}
			height = this.thickness;
			minimumHeight = this.minimumThickness;
		}

		// Token: 0x0600318A RID: 12682 RVA: 0x000AAA14 File Offset: 0x000A9A14
		internal void OnStateChanged(DataGridViewElementStates elementState)
		{
			if (base.DataGridView != null)
			{
				if (this.bandIsRow)
				{
					base.DataGridView.Rows.InvalidateCachedRowCount(elementState);
					base.DataGridView.Rows.InvalidateCachedRowsHeight(elementState);
					if (this.bandIndex != -1)
					{
						base.DataGridView.OnDataGridViewElementStateChanged(this, -1, elementState);
						return;
					}
				}
				else
				{
					base.DataGridView.Columns.InvalidateCachedColumnCount(elementState);
					base.DataGridView.Columns.InvalidateCachedColumnsWidth(elementState);
					base.DataGridView.OnDataGridViewElementStateChanged(this, -1, elementState);
				}
			}
		}

		// Token: 0x0600318B RID: 12683 RVA: 0x000AAA9B File Offset: 0x000A9A9B
		private void OnStateChanging(DataGridViewElementStates elementState)
		{
			if (base.DataGridView != null)
			{
				if (this.bandIsRow)
				{
					if (this.bandIndex != -1)
					{
						base.DataGridView.OnDataGridViewElementStateChanging(this, -1, elementState);
						return;
					}
				}
				else
				{
					base.DataGridView.OnDataGridViewElementStateChanging(this, -1, elementState);
				}
			}
		}

		// Token: 0x0600318C RID: 12684 RVA: 0x000AAAD4 File Offset: 0x000A9AD4
		protected override void OnDataGridViewChanged()
		{
			if (this.HasDefaultCellStyle)
			{
				if (base.DataGridView == null)
				{
					this.DefaultCellStyle.RemoveScope(this.bandIsRow ? DataGridViewCellStyleScopes.Row : DataGridViewCellStyleScopes.Column);
				}
				else
				{
					this.DefaultCellStyle.AddScope(base.DataGridView, this.bandIsRow ? DataGridViewCellStyleScopes.Row : DataGridViewCellStyleScopes.Column);
				}
			}
			base.OnDataGridViewChanged();
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x000AAB30 File Offset: 0x000A9B30
		private bool ShouldSerializeDefaultHeaderCellType()
		{
			Type type = (Type)this.Properties.GetObject(DataGridViewBand.PropDefaultHeaderCellType);
			return type != null;
		}

		// Token: 0x0600318E RID: 12686 RVA: 0x000AAB5A File Offset: 0x000A9B5A
		internal bool ShouldSerializeResizable()
		{
			return (this.State & DataGridViewElementStates.ResizableSet) != DataGridViewElementStates.None;
		}

		// Token: 0x0600318F RID: 12687 RVA: 0x000AAB6C File Offset: 0x000A9B6C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(36);
			stringBuilder.Append("DataGridViewBand { Index=");
			stringBuilder.Append(this.Index.ToString(CultureInfo.CurrentCulture));
			stringBuilder.Append(" }");
			return stringBuilder.ToString();
		}

		// Token: 0x04001A43 RID: 6723
		internal const int minBandThickness = 2;

		// Token: 0x04001A44 RID: 6724
		internal const int maxBandThickness = 65536;

		// Token: 0x04001A45 RID: 6725
		private static readonly int PropContextMenuStrip = PropertyStore.CreateKey();

		// Token: 0x04001A46 RID: 6726
		private static readonly int PropDefaultCellStyle = PropertyStore.CreateKey();

		// Token: 0x04001A47 RID: 6727
		private static readonly int PropDefaultHeaderCellType = PropertyStore.CreateKey();

		// Token: 0x04001A48 RID: 6728
		private static readonly int PropDividerThickness = PropertyStore.CreateKey();

		// Token: 0x04001A49 RID: 6729
		private static readonly int PropHeaderCell = PropertyStore.CreateKey();

		// Token: 0x04001A4A RID: 6730
		private static readonly int PropUserData = PropertyStore.CreateKey();

		// Token: 0x04001A4B RID: 6731
		private PropertyStore propertyStore;

		// Token: 0x04001A4C RID: 6732
		private int thickness;

		// Token: 0x04001A4D RID: 6733
		private int cachedThickness;

		// Token: 0x04001A4E RID: 6734
		private int minimumThickness;

		// Token: 0x04001A4F RID: 6735
		private int bandIndex;

		// Token: 0x04001A50 RID: 6736
		internal bool bandIsRow;
	}
}
