using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x02000308 RID: 776
	[TypeConverter(typeof(DataGridViewCellConverter))]
	public abstract class DataGridViewCell : DataGridViewElement, ICloneable, IDisposable
	{
		// Token: 0x06003193 RID: 12691 RVA: 0x000AAC0E File Offset: 0x000A9C0E
		protected DataGridViewCell()
		{
			this.propertyStore = new PropertyStore();
			base.StateInternal = DataGridViewElementStates.None;
		}

		// Token: 0x06003194 RID: 12692 RVA: 0x000AAC28 File Offset: 0x000A9C28
		~DataGridViewCell()
		{
			this.Dispose(false);
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x06003195 RID: 12693 RVA: 0x000AAC58 File Offset: 0x000A9C58
		[Browsable(false)]
		public AccessibleObject AccessibilityObject
		{
			get
			{
				AccessibleObject accessibleObject = (AccessibleObject)this.Properties.GetObject(DataGridViewCell.PropCellAccessibilityObject);
				if (accessibleObject == null)
				{
					accessibleObject = this.CreateAccessibilityInstance();
					this.Properties.SetObject(DataGridViewCell.PropCellAccessibilityObject, accessibleObject);
				}
				return accessibleObject;
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06003196 RID: 12694 RVA: 0x000AAC97 File Offset: 0x000A9C97
		public int ColumnIndex
		{
			get
			{
				if (this.owningColumn == null)
				{
					return -1;
				}
				return this.owningColumn.Index;
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06003197 RID: 12695 RVA: 0x000AACAE File Offset: 0x000A9CAE
		[Browsable(false)]
		public Rectangle ContentBounds
		{
			get
			{
				return this.GetContentBounds(this.RowIndex);
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06003198 RID: 12696 RVA: 0x000AACBC File Offset: 0x000A9CBC
		// (set) Token: 0x06003199 RID: 12697 RVA: 0x000AACCA File Offset: 0x000A9CCA
		[DefaultValue(null)]
		public virtual ContextMenuStrip ContextMenuStrip
		{
			get
			{
				return this.GetContextMenuStrip(this.RowIndex);
			}
			set
			{
				this.ContextMenuStripInternal = value;
			}
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x0600319A RID: 12698 RVA: 0x000AACD3 File Offset: 0x000A9CD3
		// (set) Token: 0x0600319B RID: 12699 RVA: 0x000AACEC File Offset: 0x000A9CEC
		private ContextMenuStrip ContextMenuStripInternal
		{
			get
			{
				return (ContextMenuStrip)this.Properties.GetObject(DataGridViewCell.PropCellContextMenuStrip);
			}
			set
			{
				ContextMenuStrip contextMenuStrip = (ContextMenuStrip)this.Properties.GetObject(DataGridViewCell.PropCellContextMenuStrip);
				if (contextMenuStrip != value)
				{
					EventHandler eventHandler = new EventHandler(this.DetachContextMenuStrip);
					if (contextMenuStrip != null)
					{
						contextMenuStrip.Disposed -= eventHandler;
					}
					this.Properties.SetObject(DataGridViewCell.PropCellContextMenuStrip, value);
					if (value != null)
					{
						value.Disposed += eventHandler;
					}
					if (base.DataGridView != null)
					{
						base.DataGridView.OnCellContextMenuStripChanged(this);
					}
				}
			}
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x0600319C RID: 12700 RVA: 0x000AAD59 File Offset: 0x000A9D59
		// (set) Token: 0x0600319D RID: 12701 RVA: 0x000AAD64 File Offset: 0x000A9D64
		private byte CurrentMouseLocation
		{
			get
			{
				return this.flags & 3;
			}
			set
			{
				this.flags = (byte)((int)this.flags & -4);
				this.flags |= value;
			}
		}

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x0600319E RID: 12702 RVA: 0x000AAD85 File Offset: 0x000A9D85
		[Browsable(false)]
		public virtual object DefaultNewRowValue
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x0600319F RID: 12703 RVA: 0x000AAD88 File Offset: 0x000A9D88
		[Browsable(false)]
		public virtual bool Displayed
		{
			get
			{
				return base.DataGridView != null && (base.DataGridView != null && this.RowIndex >= 0 && this.ColumnIndex >= 0) && this.owningColumn.Displayed && this.owningRow.Displayed;
			}
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x060031A0 RID: 12704 RVA: 0x000AADD8 File Offset: 0x000A9DD8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public object EditedFormattedValue
		{
			get
			{
				if (base.DataGridView == null)
				{
					return null;
				}
				DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, this.RowIndex, false);
				return this.GetEditedFormattedValue(this.GetValue(this.RowIndex), this.RowIndex, ref inheritedStyle, DataGridViewDataErrorContexts.Formatting);
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x060031A1 RID: 12705 RVA: 0x000AAE19 File Offset: 0x000A9E19
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public virtual Type EditType
		{
			get
			{
				return typeof(DataGridViewTextBoxEditingControl);
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x060031A2 RID: 12706 RVA: 0x000AAE25 File Offset: 0x000A9E25
		private static Bitmap ErrorBitmap
		{
			get
			{
				if (DataGridViewCell.errorBmp == null)
				{
					DataGridViewCell.errorBmp = DataGridViewCell.GetBitmap("DataGridViewRow.error.bmp");
				}
				return DataGridViewCell.errorBmp;
			}
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x060031A3 RID: 12707 RVA: 0x000AAE42 File Offset: 0x000A9E42
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public Rectangle ErrorIconBounds
		{
			get
			{
				return this.GetErrorIconBounds(this.RowIndex);
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x060031A4 RID: 12708 RVA: 0x000AAE50 File Offset: 0x000A9E50
		// (set) Token: 0x060031A5 RID: 12709 RVA: 0x000AAE5E File Offset: 0x000A9E5E
		[Browsable(false)]
		public string ErrorText
		{
			get
			{
				return this.GetErrorText(this.RowIndex);
			}
			set
			{
				this.ErrorTextInternal = value;
			}
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x060031A6 RID: 12710 RVA: 0x000AAE68 File Offset: 0x000A9E68
		// (set) Token: 0x060031A7 RID: 12711 RVA: 0x000AAE98 File Offset: 0x000A9E98
		private string ErrorTextInternal
		{
			get
			{
				object @object = this.Properties.GetObject(DataGridViewCell.PropCellErrorText);
				if (@object != null)
				{
					return (string)@object;
				}
				return string.Empty;
			}
			set
			{
				string errorTextInternal = this.ErrorTextInternal;
				if (!string.IsNullOrEmpty(value) || this.Properties.ContainsObject(DataGridViewCell.PropCellErrorText))
				{
					this.Properties.SetObject(DataGridViewCell.PropCellErrorText, value);
				}
				if (base.DataGridView != null && !errorTextInternal.Equals(this.ErrorTextInternal))
				{
					base.DataGridView.OnCellErrorTextChanged(this);
				}
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x060031A8 RID: 12712 RVA: 0x000AAEFC File Offset: 0x000A9EFC
		[Browsable(false)]
		public object FormattedValue
		{
			get
			{
				if (base.DataGridView == null)
				{
					return null;
				}
				DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, this.RowIndex, false);
				return this.GetFormattedValue(this.RowIndex, ref inheritedStyle, DataGridViewDataErrorContexts.Formatting);
			}
		}

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x060031A9 RID: 12713 RVA: 0x000AAF31 File Offset: 0x000A9F31
		[Browsable(false)]
		public virtual Type FormattedValueType
		{
			get
			{
				return this.ValueType;
			}
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x060031AA RID: 12714 RVA: 0x000AAF3C File Offset: 0x000A9F3C
		private TypeConverter FormattedValueTypeConverter
		{
			get
			{
				TypeConverter typeConverter = null;
				if (this.FormattedValueType != null)
				{
					if (base.DataGridView != null)
					{
						typeConverter = base.DataGridView.GetCachedTypeConverter(this.FormattedValueType);
					}
					else
					{
						typeConverter = TypeDescriptor.GetConverter(this.FormattedValueType);
					}
				}
				return typeConverter;
			}
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x060031AB RID: 12715 RVA: 0x000AAF7C File Offset: 0x000A9F7C
		[Browsable(false)]
		public virtual bool Frozen
		{
			get
			{
				if (base.DataGridView != null && this.RowIndex >= 0 && this.ColumnIndex >= 0)
				{
					return this.owningColumn.Frozen && this.owningRow.Frozen;
				}
				return this.owningRow != null && (this.owningRow.DataGridView == null || this.RowIndex >= 0) && this.owningRow.Frozen;
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x060031AC RID: 12716 RVA: 0x000AAFE9 File Offset: 0x000A9FE9
		internal bool HasErrorText
		{
			get
			{
				return this.Properties.ContainsObject(DataGridViewCell.PropCellErrorText) && this.Properties.GetObject(DataGridViewCell.PropCellErrorText) != null;
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x060031AD RID: 12717 RVA: 0x000AB015 File Offset: 0x000AA015
		[Browsable(false)]
		public bool HasStyle
		{
			get
			{
				return this.Properties.ContainsObject(DataGridViewCell.PropCellStyle) && this.Properties.GetObject(DataGridViewCell.PropCellStyle) != null;
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x060031AE RID: 12718 RVA: 0x000AB041 File Offset: 0x000AA041
		internal bool HasToolTipText
		{
			get
			{
				return this.Properties.ContainsObject(DataGridViewCell.PropCellToolTipText) && this.Properties.GetObject(DataGridViewCell.PropCellToolTipText) != null;
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x060031AF RID: 12719 RVA: 0x000AB06D File Offset: 0x000AA06D
		internal bool HasValue
		{
			get
			{
				return this.Properties.ContainsObject(DataGridViewCell.PropCellValue) && this.Properties.GetObject(DataGridViewCell.PropCellValue) != null;
			}
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x060031B0 RID: 12720 RVA: 0x000AB099 File Offset: 0x000AA099
		internal virtual bool HasValueType
		{
			get
			{
				return this.Properties.ContainsObject(DataGridViewCell.PropCellValueType) && this.Properties.GetObject(DataGridViewCell.PropCellValueType) != null;
			}
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x060031B1 RID: 12721 RVA: 0x000AB0C5 File Offset: 0x000AA0C5
		[Browsable(false)]
		public DataGridViewElementStates InheritedState
		{
			get
			{
				return this.GetInheritedState(this.RowIndex);
			}
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x060031B2 RID: 12722 RVA: 0x000AB0D3 File Offset: 0x000AA0D3
		[Browsable(false)]
		public DataGridViewCellStyle InheritedStyle
		{
			get
			{
				return this.GetInheritedStyleInternal(this.RowIndex);
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x060031B3 RID: 12723 RVA: 0x000AB0E4 File Offset: 0x000AA0E4
		[Browsable(false)]
		public bool IsInEditMode
		{
			get
			{
				if (base.DataGridView == null)
				{
					return false;
				}
				if (this.RowIndex == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidOperationOnSharedCell"));
				}
				Point currentCellAddress = base.DataGridView.CurrentCellAddress;
				return currentCellAddress.X != -1 && currentCellAddress.X == this.ColumnIndex && currentCellAddress.Y == this.RowIndex && base.DataGridView.IsCurrentCellInEditMode;
			}
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x060031B4 RID: 12724 RVA: 0x000AB155 File Offset: 0x000AA155
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public DataGridViewColumn OwningColumn
		{
			get
			{
				return this.owningColumn;
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (set) Token: 0x060031B5 RID: 12725 RVA: 0x000AB15D File Offset: 0x000AA15D
		internal DataGridViewColumn OwningColumnInternal
		{
			set
			{
				this.owningColumn = value;
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x060031B6 RID: 12726 RVA: 0x000AB166 File Offset: 0x000AA166
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public DataGridViewRow OwningRow
		{
			get
			{
				return this.owningRow;
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (set) Token: 0x060031B7 RID: 12727 RVA: 0x000AB16E File Offset: 0x000AA16E
		internal DataGridViewRow OwningRowInternal
		{
			set
			{
				this.owningRow = value;
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x060031B8 RID: 12728 RVA: 0x000AB177 File Offset: 0x000AA177
		[Browsable(false)]
		public Size PreferredSize
		{
			get
			{
				return this.GetPreferredSize(this.RowIndex);
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x060031B9 RID: 12729 RVA: 0x000AB185 File Offset: 0x000AA185
		internal PropertyStore Properties
		{
			get
			{
				return this.propertyStore;
			}
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x060031BA RID: 12730 RVA: 0x000AB190 File Offset: 0x000AA190
		// (set) Token: 0x060031BB RID: 12731 RVA: 0x000AB200 File Offset: 0x000AA200
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public virtual bool ReadOnly
		{
			get
			{
				return (this.State & DataGridViewElementStates.ReadOnly) != DataGridViewElementStates.None || (this.owningRow != null && (this.owningRow.DataGridView == null || this.RowIndex >= 0) && this.owningRow.ReadOnly) || (base.DataGridView != null && this.RowIndex >= 0 && this.ColumnIndex >= 0 && this.owningColumn.ReadOnly);
			}
			set
			{
				if (base.DataGridView != null)
				{
					if (this.RowIndex == -1)
					{
						throw new InvalidOperationException(SR.GetString("DataGridView_InvalidOperationOnSharedCell"));
					}
					if (value != this.ReadOnly && !base.DataGridView.ReadOnly)
					{
						base.DataGridView.OnDataGridViewElementStateChanging(this, -1, DataGridViewElementStates.ReadOnly);
						base.DataGridView.SetReadOnlyCellCore(this.ColumnIndex, this.RowIndex, value);
						return;
					}
				}
				else if (this.owningRow == null)
				{
					if (value != this.ReadOnly)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewCell_CannotSetReadOnlyState"));
					}
				}
				else
				{
					this.owningRow.SetReadOnlyCellCore(this, value);
				}
			}
		}

		// Token: 0x170008AB RID: 2219
		// (set) Token: 0x060031BC RID: 12732 RVA: 0x000AB299 File Offset: 0x000AA299
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
				if (base.DataGridView != null)
				{
					base.DataGridView.OnDataGridViewElementStateChanged(this, -1, DataGridViewElementStates.ReadOnly);
				}
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x060031BD RID: 12733 RVA: 0x000AB2D4 File Offset: 0x000AA2D4
		[Browsable(false)]
		public virtual bool Resizable
		{
			get
			{
				return (this.owningRow != null && (this.owningRow.DataGridView == null || this.RowIndex >= 0) && this.owningRow.Resizable == DataGridViewTriState.True) || (base.DataGridView != null && this.RowIndex >= 0 && this.ColumnIndex >= 0 && this.owningColumn.Resizable == DataGridViewTriState.True);
			}
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x060031BE RID: 12734 RVA: 0x000AB339 File Offset: 0x000AA339
		[Browsable(false)]
		public int RowIndex
		{
			get
			{
				if (this.owningRow == null)
				{
					return -1;
				}
				return this.owningRow.Index;
			}
		}

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x060031BF RID: 12735 RVA: 0x000AB350 File Offset: 0x000AA350
		// (set) Token: 0x060031C0 RID: 12736 RVA: 0x000AB3C0 File Offset: 0x000AA3C0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool Selected
		{
			get
			{
				return (this.State & DataGridViewElementStates.Selected) != DataGridViewElementStates.None || (this.owningRow != null && (this.owningRow.DataGridView == null || this.RowIndex >= 0) && this.owningRow.Selected) || (base.DataGridView != null && this.RowIndex >= 0 && this.ColumnIndex >= 0 && this.owningColumn.Selected);
			}
			set
			{
				if (base.DataGridView != null)
				{
					if (this.RowIndex == -1)
					{
						throw new InvalidOperationException(SR.GetString("DataGridView_InvalidOperationOnSharedCell"));
					}
					base.DataGridView.SetSelectedCellCoreInternal(this.ColumnIndex, this.RowIndex, value);
					return;
				}
				else
				{
					if (value)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewCell_CannotSetSelectedState"));
					}
					return;
				}
			}
		}

		// Token: 0x170008AF RID: 2223
		// (set) Token: 0x060031C1 RID: 12737 RVA: 0x000AB41A File Offset: 0x000AA41A
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
					base.DataGridView.OnDataGridViewElementStateChanged(this, -1, DataGridViewElementStates.Selected);
				}
			}
		}

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x060031C2 RID: 12738 RVA: 0x000AB456 File Offset: 0x000AA456
		[Browsable(false)]
		public Size Size
		{
			get
			{
				return this.GetSize(this.RowIndex);
			}
		}

		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x060031C3 RID: 12739 RVA: 0x000AB464 File Offset: 0x000AA464
		internal Rectangle StdBorderWidths
		{
			get
			{
				if (base.DataGridView != null)
				{
					DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle = new DataGridViewAdvancedBorderStyle();
					DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle2 = this.AdjustCellBorderStyle(base.DataGridView.AdvancedCellBorderStyle, dataGridViewAdvancedBorderStyle, false, false, false, false);
					return this.BorderWidths(dataGridViewAdvancedBorderStyle2);
				}
				return Rectangle.Empty;
			}
		}

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x060031C4 RID: 12740 RVA: 0x000AB4A4 File Offset: 0x000AA4A4
		// (set) Token: 0x060031C5 RID: 12741 RVA: 0x000AB4F0 File Offset: 0x000AA4F0
		[Browsable(true)]
		public DataGridViewCellStyle Style
		{
			get
			{
				DataGridViewCellStyle dataGridViewCellStyle = (DataGridViewCellStyle)this.Properties.GetObject(DataGridViewCell.PropCellStyle);
				if (dataGridViewCellStyle == null)
				{
					dataGridViewCellStyle = new DataGridViewCellStyle();
					dataGridViewCellStyle.AddScope(base.DataGridView, DataGridViewCellStyleScopes.Cell);
					this.Properties.SetObject(DataGridViewCell.PropCellStyle, dataGridViewCellStyle);
				}
				return dataGridViewCellStyle;
			}
			set
			{
				DataGridViewCellStyle dataGridViewCellStyle = null;
				if (this.HasStyle)
				{
					dataGridViewCellStyle = this.Style;
					dataGridViewCellStyle.RemoveScope(DataGridViewCellStyleScopes.Cell);
				}
				if (value != null || this.Properties.ContainsObject(DataGridViewCell.PropCellStyle))
				{
					if (value != null)
					{
						value.AddScope(base.DataGridView, DataGridViewCellStyleScopes.Cell);
					}
					this.Properties.SetObject(DataGridViewCell.PropCellStyle, value);
				}
				if (((dataGridViewCellStyle != null && value == null) || (dataGridViewCellStyle == null && value != null) || (dataGridViewCellStyle != null && value != null && !dataGridViewCellStyle.Equals(this.Style))) && base.DataGridView != null)
				{
					base.DataGridView.OnCellStyleChanged(this);
				}
			}
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x060031C6 RID: 12742 RVA: 0x000AB57F File Offset: 0x000AA57F
		// (set) Token: 0x060031C7 RID: 12743 RVA: 0x000AB591 File Offset: 0x000AA591
		[Localizable(false)]
		[TypeConverter(typeof(StringConverter))]
		[SRCategory("CatData")]
		[Bindable(true)]
		[SRDescription("ControlTagDescr")]
		[DefaultValue(null)]
		public object Tag
		{
			get
			{
				return this.Properties.GetObject(DataGridViewCell.PropCellTag);
			}
			set
			{
				if (value != null || this.Properties.ContainsObject(DataGridViewCell.PropCellTag))
				{
					this.Properties.SetObject(DataGridViewCell.PropCellTag, value);
				}
			}
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x060031C8 RID: 12744 RVA: 0x000AB5B9 File Offset: 0x000AA5B9
		// (set) Token: 0x060031C9 RID: 12745 RVA: 0x000AB5C7 File Offset: 0x000AA5C7
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public string ToolTipText
		{
			get
			{
				return this.GetToolTipText(this.RowIndex);
			}
			set
			{
				this.ToolTipTextInternal = value;
			}
		}

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x060031CA RID: 12746 RVA: 0x000AB5D0 File Offset: 0x000AA5D0
		// (set) Token: 0x060031CB RID: 12747 RVA: 0x000AB600 File Offset: 0x000AA600
		private string ToolTipTextInternal
		{
			get
			{
				object @object = this.Properties.GetObject(DataGridViewCell.PropCellToolTipText);
				if (@object != null)
				{
					return (string)@object;
				}
				return string.Empty;
			}
			set
			{
				string toolTipTextInternal = this.ToolTipTextInternal;
				if (!string.IsNullOrEmpty(value) || this.Properties.ContainsObject(DataGridViewCell.PropCellToolTipText))
				{
					this.Properties.SetObject(DataGridViewCell.PropCellToolTipText, value);
				}
				if (base.DataGridView != null && !toolTipTextInternal.Equals(this.ToolTipTextInternal))
				{
					base.DataGridView.OnCellToolTipTextChanged(this);
				}
			}
		}

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x060031CC RID: 12748 RVA: 0x000AB661 File Offset: 0x000AA661
		// (set) Token: 0x060031CD RID: 12749 RVA: 0x000AB66F File Offset: 0x000AA66F
		[Browsable(false)]
		public object Value
		{
			get
			{
				return this.GetValue(this.RowIndex);
			}
			set
			{
				this.SetValue(this.RowIndex, value);
			}
		}

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x060031CE RID: 12750 RVA: 0x000AB680 File Offset: 0x000AA680
		// (set) Token: 0x060031CF RID: 12751 RVA: 0x000AB6BB File Offset: 0x000AA6BB
		[Browsable(false)]
		public virtual Type ValueType
		{
			get
			{
				Type type = (Type)this.Properties.GetObject(DataGridViewCell.PropCellValueType);
				if (type == null && this.OwningColumn != null)
				{
					type = this.OwningColumn.ValueType;
				}
				return type;
			}
			set
			{
				if (value != null || this.Properties.ContainsObject(DataGridViewCell.PropCellValueType))
				{
					this.Properties.SetObject(DataGridViewCell.PropCellValueType, value);
				}
			}
		}

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x060031D0 RID: 12752 RVA: 0x000AB6E4 File Offset: 0x000AA6E4
		private TypeConverter ValueTypeConverter
		{
			get
			{
				TypeConverter typeConverter = null;
				if (this.OwningColumn != null)
				{
					typeConverter = this.OwningColumn.BoundColumnConverter;
				}
				if (typeConverter == null && this.ValueType != null)
				{
					if (base.DataGridView != null)
					{
						typeConverter = base.DataGridView.GetCachedTypeConverter(this.ValueType);
					}
					else
					{
						typeConverter = TypeDescriptor.GetConverter(this.ValueType);
					}
				}
				return typeConverter;
			}
		}

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x060031D1 RID: 12753 RVA: 0x000AB73C File Offset: 0x000AA73C
		[Browsable(false)]
		public virtual bool Visible
		{
			get
			{
				if (base.DataGridView != null && this.RowIndex >= 0 && this.ColumnIndex >= 0)
				{
					return this.owningColumn.Visible && this.owningRow.Visible;
				}
				return this.owningRow != null && (this.owningRow.DataGridView == null || this.RowIndex >= 0) && this.owningRow.Visible;
			}
		}

		// Token: 0x060031D2 RID: 12754 RVA: 0x000AB7AC File Offset: 0x000AA7AC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual DataGridViewAdvancedBorderStyle AdjustCellBorderStyle(DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyleInput, DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStylePlaceholder, bool singleVerticalBorderAdded, bool singleHorizontalBorderAdded, bool isFirstDisplayedColumn, bool isFirstDisplayedRow)
		{
			DataGridViewAdvancedCellBorderStyle all = dataGridViewAdvancedBorderStyleInput.All;
			switch (all)
			{
			case DataGridViewAdvancedCellBorderStyle.NotSet:
				if (base.DataGridView != null && base.DataGridView.AdvancedCellBorderStyle == dataGridViewAdvancedBorderStyleInput)
				{
					DataGridViewCellBorderStyle cellBorderStyle = base.DataGridView.CellBorderStyle;
					if (cellBorderStyle == DataGridViewCellBorderStyle.SingleVertical)
					{
						if (base.DataGridView.RightToLeftInternal)
						{
							dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = DataGridViewAdvancedCellBorderStyle.Single;
							dataGridViewAdvancedBorderStylePlaceholder.RightInternal = ((isFirstDisplayedColumn && singleVerticalBorderAdded) ? DataGridViewAdvancedCellBorderStyle.Single : DataGridViewAdvancedCellBorderStyle.None);
						}
						else
						{
							dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = ((isFirstDisplayedColumn && singleVerticalBorderAdded) ? DataGridViewAdvancedCellBorderStyle.Single : DataGridViewAdvancedCellBorderStyle.None);
							dataGridViewAdvancedBorderStylePlaceholder.RightInternal = DataGridViewAdvancedCellBorderStyle.Single;
						}
						dataGridViewAdvancedBorderStylePlaceholder.TopInternal = DataGridViewAdvancedCellBorderStyle.None;
						dataGridViewAdvancedBorderStylePlaceholder.BottomInternal = DataGridViewAdvancedCellBorderStyle.None;
						return dataGridViewAdvancedBorderStylePlaceholder;
					}
					if (cellBorderStyle == DataGridViewCellBorderStyle.SingleHorizontal)
					{
						dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = DataGridViewAdvancedCellBorderStyle.None;
						dataGridViewAdvancedBorderStylePlaceholder.RightInternal = DataGridViewAdvancedCellBorderStyle.None;
						dataGridViewAdvancedBorderStylePlaceholder.TopInternal = ((isFirstDisplayedRow && singleHorizontalBorderAdded) ? DataGridViewAdvancedCellBorderStyle.Single : DataGridViewAdvancedCellBorderStyle.None);
						dataGridViewAdvancedBorderStylePlaceholder.BottomInternal = DataGridViewAdvancedCellBorderStyle.Single;
						return dataGridViewAdvancedBorderStylePlaceholder;
					}
				}
				break;
			case DataGridViewAdvancedCellBorderStyle.None:
				break;
			case DataGridViewAdvancedCellBorderStyle.Single:
				if (base.DataGridView != null && base.DataGridView.RightToLeftInternal)
				{
					dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = DataGridViewAdvancedCellBorderStyle.Single;
					dataGridViewAdvancedBorderStylePlaceholder.RightInternal = ((isFirstDisplayedColumn && singleVerticalBorderAdded) ? DataGridViewAdvancedCellBorderStyle.Single : DataGridViewAdvancedCellBorderStyle.None);
				}
				else
				{
					dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = ((isFirstDisplayedColumn && singleVerticalBorderAdded) ? DataGridViewAdvancedCellBorderStyle.Single : DataGridViewAdvancedCellBorderStyle.None);
					dataGridViewAdvancedBorderStylePlaceholder.RightInternal = DataGridViewAdvancedCellBorderStyle.Single;
				}
				dataGridViewAdvancedBorderStylePlaceholder.TopInternal = ((isFirstDisplayedRow && singleHorizontalBorderAdded) ? DataGridViewAdvancedCellBorderStyle.Single : DataGridViewAdvancedCellBorderStyle.None);
				dataGridViewAdvancedBorderStylePlaceholder.BottomInternal = DataGridViewAdvancedCellBorderStyle.Single;
				return dataGridViewAdvancedBorderStylePlaceholder;
			default:
				if (all != DataGridViewAdvancedCellBorderStyle.OutsetPartial)
				{
				}
				break;
			}
			return dataGridViewAdvancedBorderStyleInput;
		}

		// Token: 0x060031D3 RID: 12755 RVA: 0x000AB8EC File Offset: 0x000AA8EC
		protected virtual Rectangle BorderWidths(DataGridViewAdvancedBorderStyle advancedBorderStyle)
		{
			Rectangle rectangle = default(Rectangle);
			rectangle.X = ((advancedBorderStyle.Left == DataGridViewAdvancedCellBorderStyle.None) ? 0 : 1);
			if (advancedBorderStyle.Left == DataGridViewAdvancedCellBorderStyle.OutsetDouble || advancedBorderStyle.Left == DataGridViewAdvancedCellBorderStyle.InsetDouble)
			{
				rectangle.X++;
			}
			rectangle.Y = ((advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.None) ? 0 : 1);
			if (advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.OutsetDouble || advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.InsetDouble)
			{
				rectangle.Y++;
			}
			rectangle.Width = ((advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.None) ? 0 : 1);
			if (advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.OutsetDouble || advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.InsetDouble)
			{
				rectangle.Width++;
			}
			rectangle.Height = ((advancedBorderStyle.Bottom == DataGridViewAdvancedCellBorderStyle.None) ? 0 : 1);
			if (advancedBorderStyle.Bottom == DataGridViewAdvancedCellBorderStyle.OutsetDouble || advancedBorderStyle.Bottom == DataGridViewAdvancedCellBorderStyle.InsetDouble)
			{
				rectangle.Height++;
			}
			if (this.owningColumn != null)
			{
				if (base.DataGridView != null && base.DataGridView.RightToLeftInternal)
				{
					rectangle.X += this.owningColumn.DividerWidth;
				}
				else
				{
					rectangle.Width += this.owningColumn.DividerWidth;
				}
			}
			if (this.owningRow != null)
			{
				rectangle.Height += this.owningRow.DividerHeight;
			}
			return rectangle;
		}

		// Token: 0x060031D4 RID: 12756 RVA: 0x000ABA48 File Offset: 0x000AAA48
		internal virtual void CacheEditingControl()
		{
		}

		// Token: 0x060031D5 RID: 12757 RVA: 0x000ABA4C File Offset: 0x000AAA4C
		internal DataGridViewElementStates CellStateFromColumnRowStates(DataGridViewElementStates rowState)
		{
			DataGridViewElementStates dataGridViewElementStates = DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected;
			DataGridViewElementStates dataGridViewElementStates2 = DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible;
			DataGridViewElementStates dataGridViewElementStates3 = this.owningColumn.State & dataGridViewElementStates;
			dataGridViewElementStates3 |= rowState & dataGridViewElementStates;
			return dataGridViewElementStates3 | (this.owningColumn.State & dataGridViewElementStates2 & (rowState & dataGridViewElementStates2));
		}

		// Token: 0x060031D6 RID: 12758 RVA: 0x000ABA88 File Offset: 0x000AAA88
		protected virtual bool ClickUnsharesRow(DataGridViewCellEventArgs e)
		{
			return false;
		}

		// Token: 0x060031D7 RID: 12759 RVA: 0x000ABA8B File Offset: 0x000AAA8B
		internal bool ClickUnsharesRowInternal(DataGridViewCellEventArgs e)
		{
			return this.ClickUnsharesRow(e);
		}

		// Token: 0x060031D8 RID: 12760 RVA: 0x000ABA94 File Offset: 0x000AAA94
		internal void CloneInternal(DataGridViewCell dataGridViewCell)
		{
			if (this.HasValueType)
			{
				dataGridViewCell.ValueType = this.ValueType;
			}
			if (this.HasStyle)
			{
				dataGridViewCell.Style = new DataGridViewCellStyle(this.Style);
			}
			if (this.HasErrorText)
			{
				dataGridViewCell.ErrorText = this.ErrorTextInternal;
			}
			if (this.HasToolTipText)
			{
				dataGridViewCell.ToolTipText = this.ToolTipTextInternal;
			}
			if (this.ContextMenuStripInternal != null)
			{
				dataGridViewCell.ContextMenuStrip = this.ContextMenuStripInternal.Clone();
			}
			dataGridViewCell.StateInternal = this.State & ~DataGridViewElementStates.Selected;
			dataGridViewCell.Tag = this.Tag;
		}

		// Token: 0x060031D9 RID: 12761 RVA: 0x000ABB2C File Offset: 0x000AAB2C
		public virtual object Clone()
		{
			DataGridViewCell dataGridViewCell = (DataGridViewCell)Activator.CreateInstance(base.GetType());
			this.CloneInternal(dataGridViewCell);
			return dataGridViewCell;
		}

		// Token: 0x060031DA RID: 12762 RVA: 0x000ABB54 File Offset: 0x000AAB54
		internal static int ColorDistance(Color color1, Color color2)
		{
			int num = (int)(color1.R - color2.R);
			int num2 = (int)(color1.G - color2.G);
			int num3 = (int)(color1.B - color2.B);
			return num * num + num2 * num2 + num3 * num3;
		}

		// Token: 0x060031DB RID: 12763 RVA: 0x000ABB9C File Offset: 0x000AAB9C
		internal void ComputeBorderStyleCellStateAndCellBounds(int rowIndex, out DataGridViewAdvancedBorderStyle dgvabsEffective, out DataGridViewElementStates cellState, out Rectangle cellBounds)
		{
			bool flag = !base.DataGridView.RowHeadersVisible && base.DataGridView.AdvancedCellBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Single;
			bool flag2 = !base.DataGridView.ColumnHeadersVisible && base.DataGridView.AdvancedCellBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Single;
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle = new DataGridViewAdvancedBorderStyle();
			if (rowIndex > -1 && this.OwningColumn != null)
			{
				dgvabsEffective = this.AdjustCellBorderStyle(base.DataGridView.AdvancedCellBorderStyle, dataGridViewAdvancedBorderStyle, flag, flag2, rowIndex == base.DataGridView.FirstDisplayedRowIndex, this.ColumnIndex == base.DataGridView.FirstDisplayedColumnIndex);
				DataGridViewElementStates rowState = base.DataGridView.Rows.GetRowState(rowIndex);
				cellState = this.CellStateFromColumnRowStates(rowState);
				cellState |= this.State;
			}
			else if (this.OwningColumn != null)
			{
				DataGridViewColumn lastColumn = base.DataGridView.Columns.GetLastColumn(DataGridViewElementStates.Visible, DataGridViewElementStates.None);
				bool flag3 = lastColumn != null && lastColumn.Index == this.ColumnIndex;
				dgvabsEffective = base.DataGridView.AdjustColumnHeaderBorderStyle(base.DataGridView.AdvancedColumnHeadersBorderStyle, dataGridViewAdvancedBorderStyle, this.ColumnIndex == base.DataGridView.FirstDisplayedColumnIndex, flag3);
				cellState = this.OwningColumn.State | this.State;
			}
			else if (this.OwningRow != null)
			{
				dgvabsEffective = this.OwningRow.AdjustRowHeaderBorderStyle(base.DataGridView.AdvancedRowHeadersBorderStyle, dataGridViewAdvancedBorderStyle, flag, flag2, rowIndex == base.DataGridView.FirstDisplayedRowIndex, rowIndex == base.DataGridView.Rows.GetLastRow(DataGridViewElementStates.Visible));
				cellState = this.OwningRow.GetState(rowIndex) | this.State;
			}
			else
			{
				dgvabsEffective = base.DataGridView.AdjustedTopLeftHeaderBorderStyle;
				cellState = this.State;
			}
			cellBounds = new Rectangle(new Point(0, 0), this.GetSize(rowIndex));
		}

		// Token: 0x060031DC RID: 12764 RVA: 0x000ABD6C File Offset: 0x000AAD6C
		internal Rectangle ComputeErrorIconBounds(Rectangle cellValueBounds)
		{
			if (cellValueBounds.Width >= 20 && cellValueBounds.Height >= 19)
			{
				Rectangle rectangle = new Rectangle(base.DataGridView.RightToLeftInternal ? (cellValueBounds.Left + 4) : (cellValueBounds.Right - 4 - 12), cellValueBounds.Y + (cellValueBounds.Height - 11) / 2, 12, 11);
				return rectangle;
			}
			return Rectangle.Empty;
		}

		// Token: 0x060031DD RID: 12765 RVA: 0x000ABDD9 File Offset: 0x000AADD9
		protected virtual bool ContentClickUnsharesRow(DataGridViewCellEventArgs e)
		{
			return false;
		}

		// Token: 0x060031DE RID: 12766 RVA: 0x000ABDDC File Offset: 0x000AADDC
		internal bool ContentClickUnsharesRowInternal(DataGridViewCellEventArgs e)
		{
			return this.ContentClickUnsharesRow(e);
		}

		// Token: 0x060031DF RID: 12767 RVA: 0x000ABDE5 File Offset: 0x000AADE5
		protected virtual bool ContentDoubleClickUnsharesRow(DataGridViewCellEventArgs e)
		{
			return false;
		}

		// Token: 0x060031E0 RID: 12768 RVA: 0x000ABDE8 File Offset: 0x000AADE8
		internal bool ContentDoubleClickUnsharesRowInternal(DataGridViewCellEventArgs e)
		{
			return this.ContentDoubleClickUnsharesRow(e);
		}

		// Token: 0x060031E1 RID: 12769 RVA: 0x000ABDF1 File Offset: 0x000AADF1
		protected virtual AccessibleObject CreateAccessibilityInstance()
		{
			return new DataGridViewCell.DataGridViewCellAccessibleObject(this);
		}

		// Token: 0x060031E2 RID: 12770 RVA: 0x000ABDF9 File Offset: 0x000AADF9
		private void DetachContextMenuStrip(object sender, EventArgs e)
		{
			this.ContextMenuStripInternal = null;
		}

		// Token: 0x060031E3 RID: 12771 RVA: 0x000ABE04 File Offset: 0x000AAE04
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual void DetachEditingControl()
		{
			DataGridView dataGridView = base.DataGridView;
			if (dataGridView == null || dataGridView.EditingControl == null)
			{
				throw new InvalidOperationException();
			}
			if (dataGridView.EditingControl.ParentInternal != null)
			{
				if (dataGridView.EditingControl.ContainsFocus)
				{
					ContainerControl containerControl = dataGridView.GetContainerControlInternal() as ContainerControl;
					if (containerControl != null && (dataGridView.EditingControl == containerControl.ActiveControl || dataGridView.EditingControl.Contains(containerControl.ActiveControl)))
					{
						dataGridView.FocusInternal();
					}
					else
					{
						UnsafeNativeMethods.SetFocus(new HandleRef(null, IntPtr.Zero));
					}
				}
				dataGridView.EditingPanel.Controls.Remove(dataGridView.EditingControl);
			}
			if (dataGridView.EditingPanel.ParentInternal != null)
			{
				((DataGridView.DataGridViewControlCollection)dataGridView.Controls).RemoveInternal(dataGridView.EditingPanel);
			}
			this.CurrentMouseLocation = 0;
		}

		// Token: 0x060031E4 RID: 12772 RVA: 0x000ABECD File Offset: 0x000AAECD
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060031E5 RID: 12773 RVA: 0x000ABEDC File Offset: 0x000AAEDC
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

		// Token: 0x060031E6 RID: 12774 RVA: 0x000ABF08 File Offset: 0x000AAF08
		protected virtual bool DoubleClickUnsharesRow(DataGridViewCellEventArgs e)
		{
			return false;
		}

		// Token: 0x060031E7 RID: 12775 RVA: 0x000ABF0B File Offset: 0x000AAF0B
		internal bool DoubleClickUnsharesRowInternal(DataGridViewCellEventArgs e)
		{
			return this.DoubleClickUnsharesRow(e);
		}

		// Token: 0x060031E8 RID: 12776 RVA: 0x000ABF14 File Offset: 0x000AAF14
		protected virtual bool EnterUnsharesRow(int rowIndex, bool throughMouseClick)
		{
			return false;
		}

		// Token: 0x060031E9 RID: 12777 RVA: 0x000ABF17 File Offset: 0x000AAF17
		internal bool EnterUnsharesRowInternal(int rowIndex, bool throughMouseClick)
		{
			return this.EnterUnsharesRow(rowIndex, throughMouseClick);
		}

		// Token: 0x060031EA RID: 12778 RVA: 0x000ABF24 File Offset: 0x000AAF24
		internal static void FormatPlainText(string s, bool csv, TextWriter output, ref bool escapeApplied)
		{
			if (s == null)
			{
				return;
			}
			int length = s.Length;
			for (int i = 0; i < length; i++)
			{
				char c = s[i];
				char c2 = c;
				if (c2 != '\t')
				{
					if (c2 != '"')
					{
						if (c2 != ',')
						{
							output.Write(c);
						}
						else
						{
							if (csv)
							{
								escapeApplied = true;
							}
							output.Write(',');
						}
					}
					else if (csv)
					{
						output.Write("\"\"");
						escapeApplied = true;
					}
					else
					{
						output.Write('"');
					}
				}
				else if (!csv)
				{
					output.Write(' ');
				}
				else
				{
					output.Write('\t');
				}
			}
			if (escapeApplied)
			{
				output.Write('"');
			}
		}

		// Token: 0x060031EB RID: 12779 RVA: 0x000ABFBC File Offset: 0x000AAFBC
		internal static void FormatPlainTextAsHtml(string s, TextWriter output)
		{
			if (s == null)
			{
				return;
			}
			int length = s.Length;
			char c = '\0';
			int i = 0;
			while (i < length)
			{
				char c2 = s[i];
				char c3 = c2;
				if (c3 <= '\r')
				{
					if (c3 != '\n')
					{
						if (c3 != '\r')
						{
							goto IL_00D2;
						}
					}
					else
					{
						output.Write("<br>");
					}
				}
				else
				{
					switch (c3)
					{
					case ' ':
						if (c == ' ')
						{
							output.Write("&nbsp;");
						}
						else
						{
							output.Write(c2);
						}
						break;
					case '!':
						goto IL_00D2;
					case '"':
						output.Write("&quot;");
						break;
					default:
						if (c3 != '&')
						{
							switch (c3)
							{
							case '<':
								output.Write("&lt;");
								break;
							case '=':
								goto IL_00D2;
							case '>':
								output.Write("&gt;");
								break;
							default:
								goto IL_00D2;
							}
						}
						else
						{
							output.Write("&amp;");
						}
						break;
					}
				}
				IL_0113:
				c = c2;
				i++;
				continue;
				IL_00D2:
				if (c2 >= '\u00a0' && c2 < 'Ā')
				{
					output.Write("&#");
					int num = (int)c2;
					output.Write(num.ToString(NumberFormatInfo.InvariantInfo));
					output.Write(';');
					goto IL_0113;
				}
				output.Write(c2);
				goto IL_0113;
			}
		}

		// Token: 0x060031EC RID: 12780 RVA: 0x000AC0EC File Offset: 0x000AB0EC
		private static Bitmap GetBitmap(string bitmapName)
		{
			Bitmap bitmap = new Bitmap(typeof(DataGridViewCell), bitmapName);
			bitmap.MakeTransparent();
			return bitmap;
		}

		// Token: 0x060031ED RID: 12781 RVA: 0x000AC114 File Offset: 0x000AB114
		protected virtual object GetClipboardContent(int rowIndex, bool firstCell, bool lastCell, bool inFirstRow, bool inLastRow, string format)
		{
			if (base.DataGridView == null)
			{
				return null;
			}
			if (rowIndex < 0 || rowIndex >= base.DataGridView.Rows.Count)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, rowIndex, false);
			object obj = null;
			if (base.DataGridView.IsSharedCellSelected(this, rowIndex))
			{
				obj = this.GetEditedFormattedValue(this.GetValue(rowIndex), rowIndex, ref inheritedStyle, DataGridViewDataErrorContexts.Formatting | DataGridViewDataErrorContexts.ClipboardContent);
			}
			StringBuilder stringBuilder = new StringBuilder(64);
			if (string.Equals(format, DataFormats.Html, StringComparison.OrdinalIgnoreCase))
			{
				if (firstCell)
				{
					if (inFirstRow)
					{
						stringBuilder.Append("<TABLE>");
					}
					stringBuilder.Append("<TR>");
				}
				stringBuilder.Append("<TD>");
				if (obj != null)
				{
					DataGridViewCell.FormatPlainTextAsHtml(obj.ToString(), new StringWriter(stringBuilder, CultureInfo.CurrentCulture));
				}
				else
				{
					stringBuilder.Append("&nbsp;");
				}
				stringBuilder.Append("</TD>");
				if (lastCell)
				{
					stringBuilder.Append("</TR>");
					if (inLastRow)
					{
						stringBuilder.Append("</TABLE>");
					}
				}
				return stringBuilder.ToString();
			}
			bool flag = string.Equals(format, DataFormats.CommaSeparatedValue, StringComparison.OrdinalIgnoreCase);
			if (flag || string.Equals(format, DataFormats.Text, StringComparison.OrdinalIgnoreCase) || string.Equals(format, DataFormats.UnicodeText, StringComparison.OrdinalIgnoreCase))
			{
				if (obj != null)
				{
					if (firstCell && lastCell && inFirstRow && inLastRow)
					{
						stringBuilder.Append(obj.ToString());
					}
					else
					{
						bool flag2 = false;
						int length = stringBuilder.Length;
						DataGridViewCell.FormatPlainText(obj.ToString(), flag, new StringWriter(stringBuilder, CultureInfo.CurrentCulture), ref flag2);
						if (flag2)
						{
							stringBuilder.Insert(length, '"');
						}
					}
				}
				if (lastCell)
				{
					if (!inLastRow)
					{
						stringBuilder.Append('\r');
						stringBuilder.Append('\n');
					}
				}
				else
				{
					stringBuilder.Append(flag ? ',' : '\t');
				}
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x060031EE RID: 12782 RVA: 0x000AC2D4 File Offset: 0x000AB2D4
		internal object GetClipboardContentInternal(int rowIndex, bool firstCell, bool lastCell, bool inFirstRow, bool inLastRow, string format)
		{
			return this.GetClipboardContent(rowIndex, firstCell, lastCell, inFirstRow, inLastRow, format);
		}

		// Token: 0x060031EF RID: 12783 RVA: 0x000AC2E8 File Offset: 0x000AB2E8
		internal ContextMenuStrip GetContextMenuStrip(int rowIndex)
		{
			ContextMenuStrip contextMenuStrip = this.ContextMenuStripInternal;
			if (base.DataGridView != null && (base.DataGridView.VirtualMode || base.DataGridView.DataSource != null))
			{
				contextMenuStrip = base.DataGridView.OnCellContextMenuStripNeeded(this.ColumnIndex, rowIndex, contextMenuStrip);
			}
			return contextMenuStrip;
		}

		// Token: 0x060031F0 RID: 12784 RVA: 0x000AC334 File Offset: 0x000AB334
		internal void GetContrastedPens(Color baseline, ref Pen darkPen, ref Pen lightPen)
		{
			int num = DataGridViewCell.ColorDistance(baseline, SystemColors.ControlDark);
			int num2 = DataGridViewCell.ColorDistance(baseline, SystemColors.ControlLightLight);
			if (SystemInformation.HighContrast)
			{
				if (num < 2000)
				{
					darkPen = base.DataGridView.GetCachedPen(ControlPaint.DarkDark(baseline));
				}
				else
				{
					darkPen = base.DataGridView.GetCachedPen(SystemColors.ControlDark);
				}
				if (num2 < 2000)
				{
					lightPen = base.DataGridView.GetCachedPen(ControlPaint.LightLight(baseline));
					return;
				}
				lightPen = base.DataGridView.GetCachedPen(SystemColors.ControlLightLight);
				return;
			}
			else
			{
				if (num < 1000)
				{
					darkPen = base.DataGridView.GetCachedPen(ControlPaint.Dark(baseline));
				}
				else
				{
					darkPen = base.DataGridView.GetCachedPen(SystemColors.ControlDark);
				}
				if (num2 < 1000)
				{
					lightPen = base.DataGridView.GetCachedPen(ControlPaint.Light(baseline));
					return;
				}
				lightPen = base.DataGridView.GetCachedPen(SystemColors.ControlLightLight);
				return;
			}
		}

		// Token: 0x060031F1 RID: 12785 RVA: 0x000AC41C File Offset: 0x000AB41C
		public Rectangle GetContentBounds(int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return Rectangle.Empty;
			}
			DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, rowIndex, false);
			Rectangle contentBounds;
			using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
			{
				contentBounds = this.GetContentBounds(graphics, inheritedStyle, rowIndex);
			}
			return contentBounds;
		}

		// Token: 0x060031F2 RID: 12786 RVA: 0x000AC470 File Offset: 0x000AB470
		protected virtual Rectangle GetContentBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
		{
			return Rectangle.Empty;
		}

		// Token: 0x060031F3 RID: 12787 RVA: 0x000AC478 File Offset: 0x000AB478
		internal object GetEditedFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle dataGridViewCellStyle, DataGridViewDataErrorContexts context)
		{
			Point currentCellAddress = base.DataGridView.CurrentCellAddress;
			if (this.ColumnIndex != currentCellAddress.X || rowIndex != currentCellAddress.Y)
			{
				return this.GetFormattedValue(value, rowIndex, ref dataGridViewCellStyle, null, null, context);
			}
			IDataGridViewEditingControl dataGridViewEditingControl = (IDataGridViewEditingControl)base.DataGridView.EditingControl;
			if (dataGridViewEditingControl != null)
			{
				return dataGridViewEditingControl.GetEditingControlFormattedValue(context);
			}
			IDataGridViewEditingCell dataGridViewEditingCell = this as IDataGridViewEditingCell;
			if (dataGridViewEditingCell != null && base.DataGridView.IsCurrentCellInEditMode)
			{
				return dataGridViewEditingCell.GetEditingCellFormattedValue(context);
			}
			return this.GetFormattedValue(value, rowIndex, ref dataGridViewCellStyle, null, null, context);
		}

		// Token: 0x060031F4 RID: 12788 RVA: 0x000AC504 File Offset: 0x000AB504
		public object GetEditedFormattedValue(int rowIndex, DataGridViewDataErrorContexts context)
		{
			if (base.DataGridView == null)
			{
				return null;
			}
			DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, rowIndex, false);
			return this.GetEditedFormattedValue(this.GetValue(rowIndex), rowIndex, ref inheritedStyle, context);
		}

		// Token: 0x060031F5 RID: 12789 RVA: 0x000AC538 File Offset: 0x000AB538
		internal Rectangle GetErrorIconBounds(int rowIndex)
		{
			DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, rowIndex, false);
			Rectangle errorIconBounds;
			using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
			{
				errorIconBounds = this.GetErrorIconBounds(graphics, inheritedStyle, rowIndex);
			}
			return errorIconBounds;
		}

		// Token: 0x060031F6 RID: 12790 RVA: 0x000AC57C File Offset: 0x000AB57C
		protected virtual Rectangle GetErrorIconBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
		{
			return Rectangle.Empty;
		}

		// Token: 0x060031F7 RID: 12791 RVA: 0x000AC584 File Offset: 0x000AB584
		protected internal virtual string GetErrorText(int rowIndex)
		{
			string text = string.Empty;
			object @object = this.Properties.GetObject(DataGridViewCell.PropCellErrorText);
			if (@object != null)
			{
				text = (string)@object;
			}
			else if (base.DataGridView != null && rowIndex != -1 && rowIndex != base.DataGridView.NewRowIndex && this.OwningColumn != null && this.OwningColumn.IsDataBound && base.DataGridView.DataConnection != null)
			{
				text = base.DataGridView.DataConnection.GetError(this.OwningColumn.BoundColumnIndex, this.ColumnIndex, rowIndex);
			}
			if (base.DataGridView != null && (base.DataGridView.VirtualMode || base.DataGridView.DataSource != null) && this.ColumnIndex >= 0 && rowIndex >= 0)
			{
				text = base.DataGridView.OnCellErrorTextNeeded(this.ColumnIndex, rowIndex, text);
			}
			return text;
		}

		// Token: 0x060031F8 RID: 12792 RVA: 0x000AC657 File Offset: 0x000AB657
		internal object GetFormattedValue(int rowIndex, ref DataGridViewCellStyle cellStyle, DataGridViewDataErrorContexts context)
		{
			if (base.DataGridView == null)
			{
				return null;
			}
			return this.GetFormattedValue(this.GetValue(rowIndex), rowIndex, ref cellStyle, null, null, context);
		}

		// Token: 0x060031F9 RID: 12793 RVA: 0x000AC678 File Offset: 0x000AB678
		protected virtual object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
		{
			if (base.DataGridView == null)
			{
				return null;
			}
			DataGridViewCellFormattingEventArgs dataGridViewCellFormattingEventArgs = base.DataGridView.OnCellFormatting(this.ColumnIndex, rowIndex, value, this.FormattedValueType, cellStyle);
			cellStyle = dataGridViewCellFormattingEventArgs.CellStyle;
			bool formattingApplied = dataGridViewCellFormattingEventArgs.FormattingApplied;
			object obj = dataGridViewCellFormattingEventArgs.Value;
			bool flag = true;
			if (!formattingApplied && this.FormattedValueType != null)
			{
				if (obj != null)
				{
					if (this.FormattedValueType.IsAssignableFrom(obj.GetType()))
					{
						goto IL_00EB;
					}
				}
				try
				{
					obj = Formatter.FormatObject(obj, this.FormattedValueType, (valueTypeConverter == null) ? this.ValueTypeConverter : valueTypeConverter, (formattedValueTypeConverter == null) ? this.FormattedValueTypeConverter : formattedValueTypeConverter, cellStyle.Format, cellStyle.FormatProvider, cellStyle.NullValue, cellStyle.DataSourceNullValue);
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsCriticalException(ex))
					{
						throw;
					}
					DataGridViewDataErrorEventArgs dataGridViewDataErrorEventArgs = new DataGridViewDataErrorEventArgs(ex, this.ColumnIndex, rowIndex, context);
					base.RaiseDataError(dataGridViewDataErrorEventArgs);
					if (dataGridViewDataErrorEventArgs.ThrowException)
					{
						throw dataGridViewDataErrorEventArgs.Exception;
					}
					flag = false;
				}
			}
			IL_00EB:
			if (flag && (obj == null || this.FormattedValueType == null || !this.FormattedValueType.IsAssignableFrom(obj.GetType())))
			{
				if (obj == null && cellStyle.NullValue == null && this.FormattedValueType != null && !typeof(ValueType).IsAssignableFrom(this.FormattedValueType))
				{
					return null;
				}
				Exception ex2;
				if (this.FormattedValueType == null)
				{
					ex2 = new FormatException(SR.GetString("DataGridViewCell_FormattedValueTypeNull"));
				}
				else
				{
					ex2 = new FormatException(SR.GetString("DataGridViewCell_FormattedValueHasWrongType"));
				}
				DataGridViewDataErrorEventArgs dataGridViewDataErrorEventArgs2 = new DataGridViewDataErrorEventArgs(ex2, this.ColumnIndex, rowIndex, context);
				base.RaiseDataError(dataGridViewDataErrorEventArgs2);
				if (dataGridViewDataErrorEventArgs2.ThrowException)
				{
					throw dataGridViewDataErrorEventArgs2.Exception;
				}
			}
			return obj;
		}

		// Token: 0x060031FA RID: 12794 RVA: 0x000AC830 File Offset: 0x000AB830
		internal static DataGridViewFreeDimension GetFreeDimensionFromConstraint(Size constraintSize)
		{
			if (constraintSize.Width < 0 || constraintSize.Height < 0)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"constraintSize",
					constraintSize.ToString()
				}));
			}
			if (constraintSize.Width == 0)
			{
				if (constraintSize.Height == 0)
				{
					return DataGridViewFreeDimension.Both;
				}
				return DataGridViewFreeDimension.Width;
			}
			else
			{
				if (constraintSize.Height == 0)
				{
					return DataGridViewFreeDimension.Height;
				}
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"constraintSize",
					constraintSize.ToString()
				}));
			}
		}

		// Token: 0x060031FB RID: 12795 RVA: 0x000AC8D1 File Offset: 0x000AB8D1
		internal int GetHeight(int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return -1;
			}
			return this.owningRow.GetHeight(rowIndex);
		}

		// Token: 0x060031FC RID: 12796 RVA: 0x000AC8EC File Offset: 0x000AB8EC
		public virtual ContextMenuStrip GetInheritedContextMenuStrip(int rowIndex)
		{
			if (base.DataGridView != null)
			{
				if (rowIndex < 0 || rowIndex >= base.DataGridView.Rows.Count)
				{
					throw new ArgumentOutOfRangeException("rowIndex");
				}
				if (this.ColumnIndex < 0)
				{
					throw new InvalidOperationException();
				}
			}
			ContextMenuStrip contextMenuStrip = this.GetContextMenuStrip(rowIndex);
			if (contextMenuStrip != null)
			{
				return contextMenuStrip;
			}
			if (this.owningRow != null)
			{
				contextMenuStrip = this.owningRow.GetContextMenuStrip(rowIndex);
				if (contextMenuStrip != null)
				{
					return contextMenuStrip;
				}
			}
			if (this.owningColumn != null)
			{
				contextMenuStrip = this.owningColumn.ContextMenuStrip;
				if (contextMenuStrip != null)
				{
					return contextMenuStrip;
				}
			}
			if (base.DataGridView != null)
			{
				return base.DataGridView.ContextMenuStrip;
			}
			return null;
		}

		// Token: 0x060031FD RID: 12797 RVA: 0x000AC988 File Offset: 0x000AB988
		public virtual DataGridViewElementStates GetInheritedState(int rowIndex)
		{
			DataGridViewElementStates dataGridViewElementStates = this.State | DataGridViewElementStates.ResizableSet;
			if (base.DataGridView == null)
			{
				if (rowIndex != -1)
				{
					throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
					{
						"rowIndex",
						rowIndex.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.owningRow != null)
				{
					dataGridViewElementStates |= this.owningRow.GetState(-1) & (DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible);
					if (this.owningRow.GetResizable(rowIndex) == DataGridViewTriState.True)
					{
						dataGridViewElementStates |= DataGridViewElementStates.Resizable;
					}
				}
				return dataGridViewElementStates;
			}
			else
			{
				if (rowIndex < 0 || rowIndex >= base.DataGridView.Rows.Count)
				{
					throw new ArgumentOutOfRangeException("rowIndex");
				}
				if (base.DataGridView.Rows.SharedRow(rowIndex) != this.owningRow)
				{
					throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
					{
						"rowIndex",
						rowIndex.ToString(CultureInfo.CurrentCulture)
					}));
				}
				DataGridViewElementStates rowState = base.DataGridView.Rows.GetRowState(rowIndex);
				dataGridViewElementStates |= rowState & (DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Selected);
				dataGridViewElementStates |= this.owningColumn.State & (DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Selected);
				if (this.owningRow.GetResizable(rowIndex) == DataGridViewTriState.True || this.owningColumn.Resizable == DataGridViewTriState.True)
				{
					dataGridViewElementStates |= DataGridViewElementStates.Resizable;
				}
				if (this.owningColumn.Visible && this.owningRow.GetVisible(rowIndex))
				{
					dataGridViewElementStates |= DataGridViewElementStates.Visible;
					if (this.owningColumn.Displayed && this.owningRow.GetDisplayed(rowIndex))
					{
						dataGridViewElementStates |= DataGridViewElementStates.Displayed;
					}
				}
				if (this.owningColumn.Frozen && this.owningRow.GetFrozen(rowIndex))
				{
					dataGridViewElementStates |= DataGridViewElementStates.Frozen;
				}
				return dataGridViewElementStates;
			}
		}

		// Token: 0x060031FE RID: 12798 RVA: 0x000ACB20 File Offset: 0x000ABB20
		public virtual DataGridViewCellStyle GetInheritedStyle(DataGridViewCellStyle inheritedCellStyle, int rowIndex, bool includeColors)
		{
			if (base.DataGridView == null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_CellNeedsDataGridViewForInheritedStyle"));
			}
			if (rowIndex < 0 || rowIndex >= base.DataGridView.Rows.Count)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			if (this.ColumnIndex < 0)
			{
				throw new InvalidOperationException();
			}
			DataGridViewCellStyle dataGridViewCellStyle;
			if (inheritedCellStyle == null)
			{
				dataGridViewCellStyle = base.DataGridView.PlaceholderCellStyle;
				if (!includeColors)
				{
					dataGridViewCellStyle.BackColor = Color.Empty;
					dataGridViewCellStyle.ForeColor = Color.Empty;
					dataGridViewCellStyle.SelectionBackColor = Color.Empty;
					dataGridViewCellStyle.SelectionForeColor = Color.Empty;
				}
			}
			else
			{
				dataGridViewCellStyle = inheritedCellStyle;
			}
			DataGridViewCellStyle dataGridViewCellStyle2 = null;
			if (this.HasStyle)
			{
				dataGridViewCellStyle2 = this.Style;
			}
			DataGridViewCellStyle dataGridViewCellStyle3 = null;
			if (base.DataGridView.Rows.SharedRow(rowIndex).HasDefaultCellStyle)
			{
				dataGridViewCellStyle3 = base.DataGridView.Rows.SharedRow(rowIndex).DefaultCellStyle;
			}
			DataGridViewCellStyle dataGridViewCellStyle4 = null;
			if (this.owningColumn.HasDefaultCellStyle)
			{
				dataGridViewCellStyle4 = this.owningColumn.DefaultCellStyle;
			}
			DataGridViewCellStyle defaultCellStyle = base.DataGridView.DefaultCellStyle;
			if (includeColors)
			{
				if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.BackColor.IsEmpty)
				{
					dataGridViewCellStyle.BackColor = dataGridViewCellStyle2.BackColor;
				}
				else if (dataGridViewCellStyle3 != null && !dataGridViewCellStyle3.BackColor.IsEmpty)
				{
					dataGridViewCellStyle.BackColor = dataGridViewCellStyle3.BackColor;
				}
				else if (!base.DataGridView.RowsDefaultCellStyle.BackColor.IsEmpty && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.BackColor.IsEmpty))
				{
					dataGridViewCellStyle.BackColor = base.DataGridView.RowsDefaultCellStyle.BackColor;
				}
				else if (rowIndex % 2 == 1 && !base.DataGridView.AlternatingRowsDefaultCellStyle.BackColor.IsEmpty)
				{
					dataGridViewCellStyle.BackColor = base.DataGridView.AlternatingRowsDefaultCellStyle.BackColor;
				}
				else if (dataGridViewCellStyle4 != null && !dataGridViewCellStyle4.BackColor.IsEmpty)
				{
					dataGridViewCellStyle.BackColor = dataGridViewCellStyle4.BackColor;
				}
				else
				{
					dataGridViewCellStyle.BackColor = defaultCellStyle.BackColor;
				}
				if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.ForeColor.IsEmpty)
				{
					dataGridViewCellStyle.ForeColor = dataGridViewCellStyle2.ForeColor;
				}
				else if (dataGridViewCellStyle3 != null && !dataGridViewCellStyle3.ForeColor.IsEmpty)
				{
					dataGridViewCellStyle.ForeColor = dataGridViewCellStyle3.ForeColor;
				}
				else if (!base.DataGridView.RowsDefaultCellStyle.ForeColor.IsEmpty && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.ForeColor.IsEmpty))
				{
					dataGridViewCellStyle.ForeColor = base.DataGridView.RowsDefaultCellStyle.ForeColor;
				}
				else if (rowIndex % 2 == 1 && !base.DataGridView.AlternatingRowsDefaultCellStyle.ForeColor.IsEmpty)
				{
					dataGridViewCellStyle.ForeColor = base.DataGridView.AlternatingRowsDefaultCellStyle.ForeColor;
				}
				else if (dataGridViewCellStyle4 != null && !dataGridViewCellStyle4.ForeColor.IsEmpty)
				{
					dataGridViewCellStyle.ForeColor = dataGridViewCellStyle4.ForeColor;
				}
				else
				{
					dataGridViewCellStyle.ForeColor = defaultCellStyle.ForeColor;
				}
				if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.SelectionBackColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionBackColor = dataGridViewCellStyle2.SelectionBackColor;
				}
				else if (dataGridViewCellStyle3 != null && !dataGridViewCellStyle3.SelectionBackColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionBackColor = dataGridViewCellStyle3.SelectionBackColor;
				}
				else if (!base.DataGridView.RowsDefaultCellStyle.SelectionBackColor.IsEmpty && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor.IsEmpty))
				{
					dataGridViewCellStyle.SelectionBackColor = base.DataGridView.RowsDefaultCellStyle.SelectionBackColor;
				}
				else if (rowIndex % 2 == 1 && !base.DataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionBackColor = base.DataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor;
				}
				else if (dataGridViewCellStyle4 != null && !dataGridViewCellStyle4.SelectionBackColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionBackColor = dataGridViewCellStyle4.SelectionBackColor;
				}
				else
				{
					dataGridViewCellStyle.SelectionBackColor = defaultCellStyle.SelectionBackColor;
				}
				if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.SelectionForeColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionForeColor = dataGridViewCellStyle2.SelectionForeColor;
				}
				else if (dataGridViewCellStyle3 != null && !dataGridViewCellStyle3.SelectionForeColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionForeColor = dataGridViewCellStyle3.SelectionForeColor;
				}
				else if (!base.DataGridView.RowsDefaultCellStyle.SelectionForeColor.IsEmpty && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor.IsEmpty))
				{
					dataGridViewCellStyle.SelectionForeColor = base.DataGridView.RowsDefaultCellStyle.SelectionForeColor;
				}
				else if (rowIndex % 2 == 1 && !base.DataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionForeColor = base.DataGridView.AlternatingRowsDefaultCellStyle.SelectionForeColor;
				}
				else if (dataGridViewCellStyle4 != null && !dataGridViewCellStyle4.SelectionForeColor.IsEmpty)
				{
					dataGridViewCellStyle.SelectionForeColor = dataGridViewCellStyle4.SelectionForeColor;
				}
				else
				{
					dataGridViewCellStyle.SelectionForeColor = defaultCellStyle.SelectionForeColor;
				}
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Font != null)
			{
				dataGridViewCellStyle.Font = dataGridViewCellStyle2.Font;
			}
			else if (dataGridViewCellStyle3 != null && dataGridViewCellStyle3.Font != null)
			{
				dataGridViewCellStyle.Font = dataGridViewCellStyle3.Font;
			}
			else if (base.DataGridView.RowsDefaultCellStyle.Font != null && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.Font == null))
			{
				dataGridViewCellStyle.Font = base.DataGridView.RowsDefaultCellStyle.Font;
			}
			else if (rowIndex % 2 == 1 && base.DataGridView.AlternatingRowsDefaultCellStyle.Font != null)
			{
				dataGridViewCellStyle.Font = base.DataGridView.AlternatingRowsDefaultCellStyle.Font;
			}
			else if (dataGridViewCellStyle4 != null && dataGridViewCellStyle4.Font != null)
			{
				dataGridViewCellStyle.Font = dataGridViewCellStyle4.Font;
			}
			else
			{
				dataGridViewCellStyle.Font = defaultCellStyle.Font;
			}
			if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.IsNullValueDefault)
			{
				dataGridViewCellStyle.NullValue = dataGridViewCellStyle2.NullValue;
			}
			else if (dataGridViewCellStyle3 != null && !dataGridViewCellStyle3.IsNullValueDefault)
			{
				dataGridViewCellStyle.NullValue = dataGridViewCellStyle3.NullValue;
			}
			else if (!base.DataGridView.RowsDefaultCellStyle.IsNullValueDefault && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.IsNullValueDefault))
			{
				dataGridViewCellStyle.NullValue = base.DataGridView.RowsDefaultCellStyle.NullValue;
			}
			else if (rowIndex % 2 == 1 && !base.DataGridView.AlternatingRowsDefaultCellStyle.IsNullValueDefault)
			{
				dataGridViewCellStyle.NullValue = base.DataGridView.AlternatingRowsDefaultCellStyle.NullValue;
			}
			else if (dataGridViewCellStyle4 != null && !dataGridViewCellStyle4.IsNullValueDefault)
			{
				dataGridViewCellStyle.NullValue = dataGridViewCellStyle4.NullValue;
			}
			else
			{
				dataGridViewCellStyle.NullValue = defaultCellStyle.NullValue;
			}
			if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.IsDataSourceNullValueDefault)
			{
				dataGridViewCellStyle.DataSourceNullValue = dataGridViewCellStyle2.DataSourceNullValue;
			}
			else if (dataGridViewCellStyle3 != null && !dataGridViewCellStyle3.IsDataSourceNullValueDefault)
			{
				dataGridViewCellStyle.DataSourceNullValue = dataGridViewCellStyle3.DataSourceNullValue;
			}
			else if (!base.DataGridView.RowsDefaultCellStyle.IsDataSourceNullValueDefault && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.IsDataSourceNullValueDefault))
			{
				dataGridViewCellStyle.DataSourceNullValue = base.DataGridView.RowsDefaultCellStyle.DataSourceNullValue;
			}
			else if (rowIndex % 2 == 1 && !base.DataGridView.AlternatingRowsDefaultCellStyle.IsDataSourceNullValueDefault)
			{
				dataGridViewCellStyle.DataSourceNullValue = base.DataGridView.AlternatingRowsDefaultCellStyle.DataSourceNullValue;
			}
			else if (dataGridViewCellStyle4 != null && !dataGridViewCellStyle4.IsDataSourceNullValueDefault)
			{
				dataGridViewCellStyle.DataSourceNullValue = dataGridViewCellStyle4.DataSourceNullValue;
			}
			else
			{
				dataGridViewCellStyle.DataSourceNullValue = defaultCellStyle.DataSourceNullValue;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Format.Length != 0)
			{
				dataGridViewCellStyle.Format = dataGridViewCellStyle2.Format;
			}
			else if (dataGridViewCellStyle3 != null && dataGridViewCellStyle3.Format.Length != 0)
			{
				dataGridViewCellStyle.Format = dataGridViewCellStyle3.Format;
			}
			else if (base.DataGridView.RowsDefaultCellStyle.Format.Length != 0 && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.Format.Length == 0))
			{
				dataGridViewCellStyle.Format = base.DataGridView.RowsDefaultCellStyle.Format;
			}
			else if (rowIndex % 2 == 1 && base.DataGridView.AlternatingRowsDefaultCellStyle.Format.Length != 0)
			{
				dataGridViewCellStyle.Format = base.DataGridView.AlternatingRowsDefaultCellStyle.Format;
			}
			else if (dataGridViewCellStyle4 != null && dataGridViewCellStyle4.Format.Length != 0)
			{
				dataGridViewCellStyle.Format = dataGridViewCellStyle4.Format;
			}
			else
			{
				dataGridViewCellStyle.Format = defaultCellStyle.Format;
			}
			if (dataGridViewCellStyle2 != null && !dataGridViewCellStyle2.IsFormatProviderDefault)
			{
				dataGridViewCellStyle.FormatProvider = dataGridViewCellStyle2.FormatProvider;
			}
			else if (dataGridViewCellStyle3 != null && !dataGridViewCellStyle3.IsFormatProviderDefault)
			{
				dataGridViewCellStyle.FormatProvider = dataGridViewCellStyle3.FormatProvider;
			}
			else if (!base.DataGridView.RowsDefaultCellStyle.IsFormatProviderDefault && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.IsFormatProviderDefault))
			{
				dataGridViewCellStyle.FormatProvider = base.DataGridView.RowsDefaultCellStyle.FormatProvider;
			}
			else if (rowIndex % 2 == 1 && !base.DataGridView.AlternatingRowsDefaultCellStyle.IsFormatProviderDefault)
			{
				dataGridViewCellStyle.FormatProvider = base.DataGridView.AlternatingRowsDefaultCellStyle.FormatProvider;
			}
			else if (dataGridViewCellStyle4 != null && !dataGridViewCellStyle4.IsFormatProviderDefault)
			{
				dataGridViewCellStyle.FormatProvider = dataGridViewCellStyle4.FormatProvider;
			}
			else
			{
				dataGridViewCellStyle.FormatProvider = defaultCellStyle.FormatProvider;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Alignment != DataGridViewContentAlignment.NotSet)
			{
				dataGridViewCellStyle.AlignmentInternal = dataGridViewCellStyle2.Alignment;
			}
			else if (dataGridViewCellStyle3 != null && dataGridViewCellStyle3.Alignment != DataGridViewContentAlignment.NotSet)
			{
				dataGridViewCellStyle.AlignmentInternal = dataGridViewCellStyle3.Alignment;
			}
			else if (base.DataGridView.RowsDefaultCellStyle.Alignment != DataGridViewContentAlignment.NotSet && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.Alignment == DataGridViewContentAlignment.NotSet))
			{
				dataGridViewCellStyle.AlignmentInternal = base.DataGridView.RowsDefaultCellStyle.Alignment;
			}
			else if (rowIndex % 2 == 1 && base.DataGridView.AlternatingRowsDefaultCellStyle.Alignment != DataGridViewContentAlignment.NotSet)
			{
				dataGridViewCellStyle.AlignmentInternal = base.DataGridView.AlternatingRowsDefaultCellStyle.Alignment;
			}
			else if (dataGridViewCellStyle4 != null && dataGridViewCellStyle4.Alignment != DataGridViewContentAlignment.NotSet)
			{
				dataGridViewCellStyle.AlignmentInternal = dataGridViewCellStyle4.Alignment;
			}
			else
			{
				dataGridViewCellStyle.AlignmentInternal = defaultCellStyle.Alignment;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.WrapMode != DataGridViewTriState.NotSet)
			{
				dataGridViewCellStyle.WrapModeInternal = dataGridViewCellStyle2.WrapMode;
			}
			else if (dataGridViewCellStyle3 != null && dataGridViewCellStyle3.WrapMode != DataGridViewTriState.NotSet)
			{
				dataGridViewCellStyle.WrapModeInternal = dataGridViewCellStyle3.WrapMode;
			}
			else if (base.DataGridView.RowsDefaultCellStyle.WrapMode != DataGridViewTriState.NotSet && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.WrapMode == DataGridViewTriState.NotSet))
			{
				dataGridViewCellStyle.WrapModeInternal = base.DataGridView.RowsDefaultCellStyle.WrapMode;
			}
			else if (rowIndex % 2 == 1 && base.DataGridView.AlternatingRowsDefaultCellStyle.WrapMode != DataGridViewTriState.NotSet)
			{
				dataGridViewCellStyle.WrapModeInternal = base.DataGridView.AlternatingRowsDefaultCellStyle.WrapMode;
			}
			else if (dataGridViewCellStyle4 != null && dataGridViewCellStyle4.WrapMode != DataGridViewTriState.NotSet)
			{
				dataGridViewCellStyle.WrapModeInternal = dataGridViewCellStyle4.WrapMode;
			}
			else
			{
				dataGridViewCellStyle.WrapModeInternal = defaultCellStyle.WrapMode;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Tag != null)
			{
				dataGridViewCellStyle.Tag = dataGridViewCellStyle2.Tag;
			}
			else if (dataGridViewCellStyle3 != null && dataGridViewCellStyle3.Tag != null)
			{
				dataGridViewCellStyle.Tag = dataGridViewCellStyle3.Tag;
			}
			else if (base.DataGridView.RowsDefaultCellStyle.Tag != null && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.Tag == null))
			{
				dataGridViewCellStyle.Tag = base.DataGridView.RowsDefaultCellStyle.Tag;
			}
			else if (rowIndex % 2 == 1 && base.DataGridView.AlternatingRowsDefaultCellStyle.Tag != null)
			{
				dataGridViewCellStyle.Tag = base.DataGridView.AlternatingRowsDefaultCellStyle.Tag;
			}
			else if (dataGridViewCellStyle4 != null && dataGridViewCellStyle4.Tag != null)
			{
				dataGridViewCellStyle.Tag = dataGridViewCellStyle4.Tag;
			}
			else
			{
				dataGridViewCellStyle.Tag = defaultCellStyle.Tag;
			}
			if (dataGridViewCellStyle2 != null && dataGridViewCellStyle2.Padding != Padding.Empty)
			{
				dataGridViewCellStyle.PaddingInternal = dataGridViewCellStyle2.Padding;
			}
			else if (dataGridViewCellStyle3 != null && dataGridViewCellStyle3.Padding != Padding.Empty)
			{
				dataGridViewCellStyle.PaddingInternal = dataGridViewCellStyle3.Padding;
			}
			else if (base.DataGridView.RowsDefaultCellStyle.Padding != Padding.Empty && (rowIndex % 2 == 0 || base.DataGridView.AlternatingRowsDefaultCellStyle.Padding == Padding.Empty))
			{
				dataGridViewCellStyle.PaddingInternal = base.DataGridView.RowsDefaultCellStyle.Padding;
			}
			else if (rowIndex % 2 == 1 && base.DataGridView.AlternatingRowsDefaultCellStyle.Padding != Padding.Empty)
			{
				dataGridViewCellStyle.PaddingInternal = base.DataGridView.AlternatingRowsDefaultCellStyle.Padding;
			}
			else if (dataGridViewCellStyle4 != null && dataGridViewCellStyle4.Padding != Padding.Empty)
			{
				dataGridViewCellStyle.PaddingInternal = dataGridViewCellStyle4.Padding;
			}
			else
			{
				dataGridViewCellStyle.PaddingInternal = defaultCellStyle.Padding;
			}
			return dataGridViewCellStyle;
		}

		// Token: 0x060031FF RID: 12799 RVA: 0x000AD7DE File Offset: 0x000AC7DE
		internal DataGridViewCellStyle GetInheritedStyleInternal(int rowIndex)
		{
			return this.GetInheritedStyle(null, rowIndex, true);
		}

		// Token: 0x06003200 RID: 12800 RVA: 0x000AD7EC File Offset: 0x000AC7EC
		internal int GetPreferredHeight(int rowIndex, int width)
		{
			if (base.DataGridView == null)
			{
				return -1;
			}
			DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, rowIndex, false);
			int height;
			using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
			{
				height = this.GetPreferredSize(graphics, inheritedStyle, rowIndex, new Size(width, 0)).Height;
			}
			return height;
		}

		// Token: 0x06003201 RID: 12801 RVA: 0x000AD84C File Offset: 0x000AC84C
		internal Size GetPreferredSize(int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return new Size(-1, -1);
			}
			DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, rowIndex, false);
			Size preferredSize;
			using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
			{
				preferredSize = this.GetPreferredSize(graphics, inheritedStyle, rowIndex, Size.Empty);
			}
			return preferredSize;
		}

		// Token: 0x06003202 RID: 12802 RVA: 0x000AD8A8 File Offset: 0x000AC8A8
		protected virtual Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
		{
			return new Size(-1, -1);
		}

		// Token: 0x06003203 RID: 12803 RVA: 0x000AD8B4 File Offset: 0x000AC8B4
		internal static int GetPreferredTextHeight(Graphics g, bool rightToLeft, string text, DataGridViewCellStyle cellStyle, int maxWidth, out bool widthTruncated)
		{
			TextFormatFlags textFormatFlags = DataGridViewUtilities.ComputeTextFormatFlagsForCellStyleAlignment(rightToLeft, cellStyle.Alignment, cellStyle.WrapMode);
			if (cellStyle.WrapMode == DataGridViewTriState.True)
			{
				return DataGridViewCell.MeasureTextHeight(g, text, cellStyle.Font, maxWidth, textFormatFlags, out widthTruncated);
			}
			Size size = DataGridViewCell.MeasureTextSize(g, text, cellStyle.Font, textFormatFlags);
			widthTruncated = size.Width > maxWidth;
			return size.Height;
		}

		// Token: 0x06003204 RID: 12804 RVA: 0x000AD914 File Offset: 0x000AC914
		internal int GetPreferredWidth(int rowIndex, int height)
		{
			if (base.DataGridView == null)
			{
				return -1;
			}
			DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, rowIndex, false);
			int width;
			using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
			{
				width = this.GetPreferredSize(graphics, inheritedStyle, rowIndex, new Size(0, height)).Width;
			}
			return width;
		}

		// Token: 0x06003205 RID: 12805 RVA: 0x000AD974 File Offset: 0x000AC974
		protected virtual Size GetSize(int rowIndex)
		{
			if (base.DataGridView == null)
			{
				return new Size(-1, -1);
			}
			if (rowIndex == -1)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_InvalidPropertyGetOnSharedCell", new object[] { "Size" }));
			}
			return new Size(this.owningColumn.Thickness, this.owningRow.GetHeight(rowIndex));
		}

		// Token: 0x06003206 RID: 12806 RVA: 0x000AD9D4 File Offset: 0x000AC9D4
		private string GetToolTipText(int rowIndex)
		{
			string text = this.ToolTipTextInternal;
			if (base.DataGridView != null && (base.DataGridView.VirtualMode || base.DataGridView.DataSource != null))
			{
				text = base.DataGridView.OnCellToolTipTextNeeded(this.ColumnIndex, rowIndex, text);
			}
			return text;
		}

		// Token: 0x06003207 RID: 12807 RVA: 0x000ADA20 File Offset: 0x000ACA20
		protected virtual object GetValue(int rowIndex)
		{
			DataGridView dataGridView = base.DataGridView;
			if (dataGridView != null)
			{
				if (rowIndex < 0 || rowIndex >= dataGridView.Rows.Count)
				{
					throw new ArgumentOutOfRangeException("rowIndex");
				}
				if (this.ColumnIndex < 0)
				{
					throw new InvalidOperationException();
				}
			}
			if (dataGridView == null || (dataGridView.AllowUserToAddRowsInternal && rowIndex > -1 && rowIndex == dataGridView.NewRowIndex && rowIndex != dataGridView.CurrentCellAddress.Y) || (!dataGridView.VirtualMode && this.OwningColumn != null && !this.OwningColumn.IsDataBound) || rowIndex == -1 || this.ColumnIndex == -1)
			{
				return this.Properties.GetObject(DataGridViewCell.PropCellValue);
			}
			if (this.OwningColumn == null || !this.OwningColumn.IsDataBound)
			{
				return dataGridView.OnCellValueNeeded(this.ColumnIndex, rowIndex);
			}
			DataGridView.DataGridViewDataConnection dataConnection = dataGridView.DataConnection;
			if (dataConnection == null)
			{
				return null;
			}
			if (dataConnection.CurrencyManager.Count <= rowIndex)
			{
				return this.Properties.GetObject(DataGridViewCell.PropCellValue);
			}
			return dataConnection.GetValue(this.OwningColumn.BoundColumnIndex, this.ColumnIndex, rowIndex);
		}

		// Token: 0x06003208 RID: 12808 RVA: 0x000ADB2D File Offset: 0x000ACB2D
		internal object GetValueInternal(int rowIndex)
		{
			return this.GetValue(rowIndex);
		}

		// Token: 0x06003209 RID: 12809 RVA: 0x000ADB38 File Offset: 0x000ACB38
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
		{
			DataGridView dataGridView = base.DataGridView;
			if (dataGridView == null || dataGridView.EditingControl == null)
			{
				throw new InvalidOperationException();
			}
			if (dataGridView.EditingControl.ParentInternal == null)
			{
				dataGridView.EditingControl.CausesValidation = dataGridView.CausesValidation;
				dataGridView.EditingPanel.CausesValidation = dataGridView.CausesValidation;
				dataGridView.EditingControl.Visible = true;
				dataGridView.EditingPanel.Visible = false;
				dataGridView.Controls.Add(dataGridView.EditingPanel);
				dataGridView.EditingPanel.Controls.Add(dataGridView.EditingControl);
			}
		}

		// Token: 0x0600320A RID: 12810 RVA: 0x000ADBCB File Offset: 0x000ACBCB
		protected virtual bool KeyDownUnsharesRow(KeyEventArgs e, int rowIndex)
		{
			return false;
		}

		// Token: 0x0600320B RID: 12811 RVA: 0x000ADBCE File Offset: 0x000ACBCE
		internal bool KeyDownUnsharesRowInternal(KeyEventArgs e, int rowIndex)
		{
			return this.KeyDownUnsharesRow(e, rowIndex);
		}

		// Token: 0x0600320C RID: 12812 RVA: 0x000ADBD8 File Offset: 0x000ACBD8
		public virtual bool KeyEntersEditMode(KeyEventArgs e)
		{
			return false;
		}

		// Token: 0x0600320D RID: 12813 RVA: 0x000ADBDB File Offset: 0x000ACBDB
		protected virtual bool KeyPressUnsharesRow(KeyPressEventArgs e, int rowIndex)
		{
			return false;
		}

		// Token: 0x0600320E RID: 12814 RVA: 0x000ADBDE File Offset: 0x000ACBDE
		internal bool KeyPressUnsharesRowInternal(KeyPressEventArgs e, int rowIndex)
		{
			return this.KeyPressUnsharesRow(e, rowIndex);
		}

		// Token: 0x0600320F RID: 12815 RVA: 0x000ADBE8 File Offset: 0x000ACBE8
		protected virtual bool KeyUpUnsharesRow(KeyEventArgs e, int rowIndex)
		{
			return false;
		}

		// Token: 0x06003210 RID: 12816 RVA: 0x000ADBEB File Offset: 0x000ACBEB
		internal bool KeyUpUnsharesRowInternal(KeyEventArgs e, int rowIndex)
		{
			return this.KeyUpUnsharesRow(e, rowIndex);
		}

		// Token: 0x06003211 RID: 12817 RVA: 0x000ADBF5 File Offset: 0x000ACBF5
		protected virtual bool LeaveUnsharesRow(int rowIndex, bool throughMouseClick)
		{
			return false;
		}

		// Token: 0x06003212 RID: 12818 RVA: 0x000ADBF8 File Offset: 0x000ACBF8
		internal bool LeaveUnsharesRowInternal(int rowIndex, bool throughMouseClick)
		{
			return this.LeaveUnsharesRow(rowIndex, throughMouseClick);
		}

		// Token: 0x06003213 RID: 12819 RVA: 0x000ADC04 File Offset: 0x000ACC04
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static int MeasureTextHeight(Graphics graphics, string text, Font font, int maxWidth, TextFormatFlags flags)
		{
			bool flag;
			return DataGridViewCell.MeasureTextHeight(graphics, text, font, maxWidth, flags, out flag);
		}

		// Token: 0x06003214 RID: 12820 RVA: 0x000ADC20 File Offset: 0x000ACC20
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static int MeasureTextHeight(Graphics graphics, string text, Font font, int maxWidth, TextFormatFlags flags, out bool widthTruncated)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			if (font == null)
			{
				throw new ArgumentNullException("font");
			}
			if (maxWidth <= 0)
			{
				throw new ArgumentOutOfRangeException("maxWidth", SR.GetString("InvalidLowBoundArgument", new object[]
				{
					"maxWidth",
					maxWidth.ToString(CultureInfo.CurrentCulture),
					0.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (!DataGridViewUtilities.ValidTextFormatFlags(flags))
			{
				throw new InvalidEnumArgumentException("flags", (int)flags, typeof(TextFormatFlags));
			}
			flags &= TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.WordBreak;
			Size size = TextRenderer.MeasureText(text, font, new Size(maxWidth, int.MaxValue), flags);
			widthTruncated = size.Width > maxWidth;
			return size.Height;
		}

		// Token: 0x06003215 RID: 12821 RVA: 0x000ADCE8 File Offset: 0x000ACCE8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Size MeasureTextPreferredSize(Graphics graphics, string text, Font font, float maxRatio, TextFormatFlags flags)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			if (font == null)
			{
				throw new ArgumentNullException("font");
			}
			if (maxRatio <= 0f)
			{
				throw new ArgumentOutOfRangeException("maxRatio", SR.GetString("InvalidLowBoundArgument", new object[]
				{
					"maxRatio",
					maxRatio.ToString(CultureInfo.CurrentCulture),
					"0.0"
				}));
			}
			if (!DataGridViewUtilities.ValidTextFormatFlags(flags))
			{
				throw new InvalidEnumArgumentException("flags", (int)flags, typeof(TextFormatFlags));
			}
			if (string.IsNullOrEmpty(text))
			{
				return new Size(0, 0);
			}
			Size size = DataGridViewCell.MeasureTextSize(graphics, text, font, flags);
			if ((float)(size.Width / size.Height) <= maxRatio)
			{
				return size;
			}
			flags &= TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.WordBreak;
			float num = (float)(size.Width * size.Width) / (float)size.Height / maxRatio * 1.1f;
			Size size2;
			for (;;)
			{
				size2 = TextRenderer.MeasureText(text, font, new Size((int)num, int.MaxValue), flags);
				if ((float)(size2.Width / size2.Height) <= maxRatio || size2.Width > (int)num)
				{
					break;
				}
				num = (float)size2.Width * 0.9f;
				if (num <= 1f)
				{
					return size2;
				}
			}
			return size2;
		}

		// Token: 0x06003216 RID: 12822 RVA: 0x000ADE20 File Offset: 0x000ACE20
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Size MeasureTextSize(Graphics graphics, string text, Font font, TextFormatFlags flags)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			if (font == null)
			{
				throw new ArgumentNullException("font");
			}
			if (!DataGridViewUtilities.ValidTextFormatFlags(flags))
			{
				throw new InvalidEnumArgumentException("flags", (int)flags, typeof(TextFormatFlags));
			}
			flags &= TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.WordBreak;
			return TextRenderer.MeasureText(text, font, new Size(int.MaxValue, int.MaxValue), flags);
		}

		// Token: 0x06003217 RID: 12823 RVA: 0x000ADE88 File Offset: 0x000ACE88
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static int MeasureTextWidth(Graphics graphics, string text, Font font, int maxHeight, TextFormatFlags flags)
		{
			if (maxHeight <= 0)
			{
				throw new ArgumentOutOfRangeException("maxHeight", SR.GetString("InvalidLowBoundArgument", new object[]
				{
					"maxHeight",
					maxHeight.ToString(CultureInfo.CurrentCulture),
					0.ToString(CultureInfo.CurrentCulture)
				}));
			}
			Size size = DataGridViewCell.MeasureTextSize(graphics, text, font, flags);
			if (size.Height >= maxHeight || (flags & TextFormatFlags.SingleLine) != TextFormatFlags.Default)
			{
				return size.Width;
			}
			flags &= TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.WordBreak;
			int num = size.Width;
			float num2 = (float)num * 0.9f;
			for (;;)
			{
				Size size2 = TextRenderer.MeasureText(text, font, new Size((int)num2, maxHeight), flags);
				if (size2.Height > maxHeight || size2.Width > (int)num2)
				{
					break;
				}
				num = (int)num2;
				num2 = (float)size2.Width * 0.9f;
				if (num2 <= 1f)
				{
					return num;
				}
			}
			return num;
		}

		// Token: 0x06003218 RID: 12824 RVA: 0x000ADF67 File Offset: 0x000ACF67
		protected virtual bool MouseClickUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return false;
		}

		// Token: 0x06003219 RID: 12825 RVA: 0x000ADF6A File Offset: 0x000ACF6A
		internal bool MouseClickUnsharesRowInternal(DataGridViewCellMouseEventArgs e)
		{
			return this.MouseClickUnsharesRow(e);
		}

		// Token: 0x0600321A RID: 12826 RVA: 0x000ADF73 File Offset: 0x000ACF73
		protected virtual bool MouseDoubleClickUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return false;
		}

		// Token: 0x0600321B RID: 12827 RVA: 0x000ADF76 File Offset: 0x000ACF76
		internal bool MouseDoubleClickUnsharesRowInternal(DataGridViewCellMouseEventArgs e)
		{
			return this.MouseDoubleClickUnsharesRow(e);
		}

		// Token: 0x0600321C RID: 12828 RVA: 0x000ADF7F File Offset: 0x000ACF7F
		protected virtual bool MouseDownUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return false;
		}

		// Token: 0x0600321D RID: 12829 RVA: 0x000ADF82 File Offset: 0x000ACF82
		internal bool MouseDownUnsharesRowInternal(DataGridViewCellMouseEventArgs e)
		{
			return this.MouseDownUnsharesRow(e);
		}

		// Token: 0x0600321E RID: 12830 RVA: 0x000ADF8B File Offset: 0x000ACF8B
		protected virtual bool MouseEnterUnsharesRow(int rowIndex)
		{
			return false;
		}

		// Token: 0x0600321F RID: 12831 RVA: 0x000ADF8E File Offset: 0x000ACF8E
		internal bool MouseEnterUnsharesRowInternal(int rowIndex)
		{
			return this.MouseEnterUnsharesRow(rowIndex);
		}

		// Token: 0x06003220 RID: 12832 RVA: 0x000ADF97 File Offset: 0x000ACF97
		protected virtual bool MouseLeaveUnsharesRow(int rowIndex)
		{
			return false;
		}

		// Token: 0x06003221 RID: 12833 RVA: 0x000ADF9A File Offset: 0x000ACF9A
		internal bool MouseLeaveUnsharesRowInternal(int rowIndex)
		{
			return this.MouseLeaveUnsharesRow(rowIndex);
		}

		// Token: 0x06003222 RID: 12834 RVA: 0x000ADFA3 File Offset: 0x000ACFA3
		protected virtual bool MouseMoveUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return false;
		}

		// Token: 0x06003223 RID: 12835 RVA: 0x000ADFA6 File Offset: 0x000ACFA6
		internal bool MouseMoveUnsharesRowInternal(DataGridViewCellMouseEventArgs e)
		{
			return this.MouseMoveUnsharesRow(e);
		}

		// Token: 0x06003224 RID: 12836 RVA: 0x000ADFAF File Offset: 0x000ACFAF
		protected virtual bool MouseUpUnsharesRow(DataGridViewCellMouseEventArgs e)
		{
			return false;
		}

		// Token: 0x06003225 RID: 12837 RVA: 0x000ADFB2 File Offset: 0x000ACFB2
		internal bool MouseUpUnsharesRowInternal(DataGridViewCellMouseEventArgs e)
		{
			return this.MouseUpUnsharesRow(e);
		}

		// Token: 0x06003226 RID: 12838 RVA: 0x000ADFBC File Offset: 0x000ACFBC
		private void OnCellDataAreaMouseEnterInternal(int rowIndex)
		{
			if (!base.DataGridView.ShowCellToolTips)
			{
				return;
			}
			Point currentCellAddress = base.DataGridView.CurrentCellAddress;
			if (currentCellAddress.X != -1 && currentCellAddress.X == this.ColumnIndex && currentCellAddress.Y == rowIndex && base.DataGridView.EditingControl != null)
			{
				return;
			}
			string text = this.GetToolTipText(rowIndex);
			if (string.IsNullOrEmpty(text))
			{
				if (this.FormattedValueType != DataGridViewCell.stringType)
				{
					goto IL_01DF;
				}
				if (rowIndex != -1 && this.OwningColumn != null)
				{
					int preferredWidth = this.GetPreferredWidth(rowIndex, this.OwningRow.Height);
					int preferredHeight = this.GetPreferredHeight(rowIndex, this.OwningColumn.Width);
					if (this.OwningColumn.Width >= preferredWidth && this.OwningRow.Height >= preferredHeight)
					{
						goto IL_01DF;
					}
					DataGridViewCellStyle inheritedStyle = this.GetInheritedStyle(null, rowIndex, false);
					string text2 = this.GetEditedFormattedValue(this.GetValue(rowIndex), rowIndex, ref inheritedStyle, DataGridViewDataErrorContexts.Display) as string;
					if (!string.IsNullOrEmpty(text2))
					{
						text = DataGridViewCell.TruncateToolTipText(text2);
						goto IL_01DF;
					}
					goto IL_01DF;
				}
				else
				{
					if ((rowIndex == -1 || this.OwningRow == null || !base.DataGridView.RowHeadersVisible || base.DataGridView.RowHeadersWidth <= 0 || this.OwningColumn != null) && rowIndex != -1)
					{
						goto IL_01DF;
					}
					string text3 = this.GetValue(rowIndex) as string;
					if (string.IsNullOrEmpty(text3))
					{
						goto IL_01DF;
					}
					DataGridViewCellStyle inheritedStyle2 = this.GetInheritedStyle(null, rowIndex, false);
					using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
					{
						Rectangle contentBounds = this.GetContentBounds(graphics, inheritedStyle2, rowIndex);
						bool flag = false;
						int num = 0;
						if (contentBounds.Width > 0)
						{
							num = DataGridViewCell.GetPreferredTextHeight(graphics, base.DataGridView.RightToLeftInternal, text3, inheritedStyle2, contentBounds.Width, out flag);
						}
						else
						{
							flag = true;
						}
						if (num > contentBounds.Height || flag)
						{
							text = DataGridViewCell.TruncateToolTipText(text3);
						}
						goto IL_01DF;
					}
				}
			}
			if (base.DataGridView.IsRestricted)
			{
				text = DataGridViewCell.TruncateToolTipText(text);
			}
			IL_01DF:
			if (!string.IsNullOrEmpty(text))
			{
				base.DataGridView.ActivateToolTip(true, text, this.ColumnIndex, rowIndex);
			}
		}

		// Token: 0x06003227 RID: 12839 RVA: 0x000AE1D4 File Offset: 0x000AD1D4
		private void OnCellDataAreaMouseLeaveInternal()
		{
			if (base.DataGridView.IsDisposed)
			{
				return;
			}
			base.DataGridView.ActivateToolTip(false, string.Empty, -1, -1);
		}

		// Token: 0x06003228 RID: 12840 RVA: 0x000AE1F8 File Offset: 0x000AD1F8
		private void OnCellErrorAreaMouseEnterInternal(int rowIndex)
		{
			string errorText = this.GetErrorText(rowIndex);
			base.DataGridView.ActivateToolTip(true, errorText, this.ColumnIndex, rowIndex);
		}

		// Token: 0x06003229 RID: 12841 RVA: 0x000AE221 File Offset: 0x000AD221
		private void OnCellErrorAreaMouseLeaveInternal()
		{
			base.DataGridView.ActivateToolTip(false, string.Empty, -1, -1);
		}

		// Token: 0x0600322A RID: 12842 RVA: 0x000AE236 File Offset: 0x000AD236
		protected virtual void OnClick(DataGridViewCellEventArgs e)
		{
		}

		// Token: 0x0600322B RID: 12843 RVA: 0x000AE238 File Offset: 0x000AD238
		internal void OnClickInternal(DataGridViewCellEventArgs e)
		{
			this.OnClick(e);
		}

		// Token: 0x0600322C RID: 12844 RVA: 0x000AE244 File Offset: 0x000AD244
		internal void OnCommonChange()
		{
			if (base.DataGridView != null && !base.DataGridView.IsDisposed && !base.DataGridView.Disposing)
			{
				if (this.RowIndex == -1)
				{
					base.DataGridView.OnColumnCommonChange(this.ColumnIndex);
					return;
				}
				base.DataGridView.OnCellCommonChange(this.ColumnIndex, this.RowIndex);
			}
		}

		// Token: 0x0600322D RID: 12845 RVA: 0x000AE2A5 File Offset: 0x000AD2A5
		protected virtual void OnContentClick(DataGridViewCellEventArgs e)
		{
		}

		// Token: 0x0600322E RID: 12846 RVA: 0x000AE2A7 File Offset: 0x000AD2A7
		internal void OnContentClickInternal(DataGridViewCellEventArgs e)
		{
			this.OnContentClick(e);
		}

		// Token: 0x0600322F RID: 12847 RVA: 0x000AE2B0 File Offset: 0x000AD2B0
		protected virtual void OnContentDoubleClick(DataGridViewCellEventArgs e)
		{
		}

		// Token: 0x06003230 RID: 12848 RVA: 0x000AE2B2 File Offset: 0x000AD2B2
		internal void OnContentDoubleClickInternal(DataGridViewCellEventArgs e)
		{
			this.OnContentDoubleClick(e);
		}

		// Token: 0x06003231 RID: 12849 RVA: 0x000AE2BB File Offset: 0x000AD2BB
		protected virtual void OnDoubleClick(DataGridViewCellEventArgs e)
		{
		}

		// Token: 0x06003232 RID: 12850 RVA: 0x000AE2BD File Offset: 0x000AD2BD
		internal void OnDoubleClickInternal(DataGridViewCellEventArgs e)
		{
			this.OnDoubleClick(e);
		}

		// Token: 0x06003233 RID: 12851 RVA: 0x000AE2C6 File Offset: 0x000AD2C6
		protected virtual void OnEnter(int rowIndex, bool throughMouseClick)
		{
		}

		// Token: 0x06003234 RID: 12852 RVA: 0x000AE2C8 File Offset: 0x000AD2C8
		internal void OnEnterInternal(int rowIndex, bool throughMouseClick)
		{
			this.OnEnter(rowIndex, throughMouseClick);
		}

		// Token: 0x06003235 RID: 12853 RVA: 0x000AE2D2 File Offset: 0x000AD2D2
		internal void OnKeyDownInternal(KeyEventArgs e, int rowIndex)
		{
			this.OnKeyDown(e, rowIndex);
		}

		// Token: 0x06003236 RID: 12854 RVA: 0x000AE2DC File Offset: 0x000AD2DC
		protected virtual void OnKeyDown(KeyEventArgs e, int rowIndex)
		{
		}

		// Token: 0x06003237 RID: 12855 RVA: 0x000AE2DE File Offset: 0x000AD2DE
		internal void OnKeyPressInternal(KeyPressEventArgs e, int rowIndex)
		{
			this.OnKeyPress(e, rowIndex);
		}

		// Token: 0x06003238 RID: 12856 RVA: 0x000AE2E8 File Offset: 0x000AD2E8
		protected virtual void OnKeyPress(KeyPressEventArgs e, int rowIndex)
		{
		}

		// Token: 0x06003239 RID: 12857 RVA: 0x000AE2EA File Offset: 0x000AD2EA
		protected virtual void OnKeyUp(KeyEventArgs e, int rowIndex)
		{
		}

		// Token: 0x0600323A RID: 12858 RVA: 0x000AE2EC File Offset: 0x000AD2EC
		internal void OnKeyUpInternal(KeyEventArgs e, int rowIndex)
		{
			this.OnKeyUp(e, rowIndex);
		}

		// Token: 0x0600323B RID: 12859 RVA: 0x000AE2F6 File Offset: 0x000AD2F6
		protected virtual void OnLeave(int rowIndex, bool throughMouseClick)
		{
		}

		// Token: 0x0600323C RID: 12860 RVA: 0x000AE2F8 File Offset: 0x000AD2F8
		internal void OnLeaveInternal(int rowIndex, bool throughMouseClick)
		{
			this.OnLeave(rowIndex, throughMouseClick);
		}

		// Token: 0x0600323D RID: 12861 RVA: 0x000AE302 File Offset: 0x000AD302
		protected virtual void OnMouseClick(DataGridViewCellMouseEventArgs e)
		{
		}

		// Token: 0x0600323E RID: 12862 RVA: 0x000AE304 File Offset: 0x000AD304
		internal void OnMouseClickInternal(DataGridViewCellMouseEventArgs e)
		{
			this.OnMouseClick(e);
		}

		// Token: 0x0600323F RID: 12863 RVA: 0x000AE30D File Offset: 0x000AD30D
		protected virtual void OnMouseDoubleClick(DataGridViewCellMouseEventArgs e)
		{
		}

		// Token: 0x06003240 RID: 12864 RVA: 0x000AE30F File Offset: 0x000AD30F
		internal void OnMouseDoubleClickInternal(DataGridViewCellMouseEventArgs e)
		{
			this.OnMouseDoubleClick(e);
		}

		// Token: 0x06003241 RID: 12865 RVA: 0x000AE318 File Offset: 0x000AD318
		protected virtual void OnMouseDown(DataGridViewCellMouseEventArgs e)
		{
		}

		// Token: 0x06003242 RID: 12866 RVA: 0x000AE31C File Offset: 0x000AD31C
		internal void OnMouseDownInternal(DataGridViewCellMouseEventArgs e)
		{
			base.DataGridView.CellMouseDownInContentBounds = this.GetContentBounds(e.RowIndex).Contains(e.X, e.Y);
			if (((this.ColumnIndex < 0 || e.RowIndex < 0) && base.DataGridView.ApplyVisualStylesToHeaderCells) || (this.ColumnIndex >= 0 && e.RowIndex >= 0 && base.DataGridView.ApplyVisualStylesToInnerCells))
			{
				base.DataGridView.InvalidateCell(this.ColumnIndex, e.RowIndex);
			}
			this.OnMouseDown(e);
		}

		// Token: 0x06003243 RID: 12867 RVA: 0x000AE3B0 File Offset: 0x000AD3B0
		protected virtual void OnMouseEnter(int rowIndex)
		{
		}

		// Token: 0x06003244 RID: 12868 RVA: 0x000AE3B2 File Offset: 0x000AD3B2
		internal void OnMouseEnterInternal(int rowIndex)
		{
			this.OnMouseEnter(rowIndex);
		}

		// Token: 0x06003245 RID: 12869 RVA: 0x000AE3BB File Offset: 0x000AD3BB
		protected virtual void OnMouseLeave(int rowIndex)
		{
		}

		// Token: 0x06003246 RID: 12870 RVA: 0x000AE3C0 File Offset: 0x000AD3C0
		internal void OnMouseLeaveInternal(int rowIndex)
		{
			switch (this.CurrentMouseLocation)
			{
			case 1:
				this.OnCellDataAreaMouseLeaveInternal();
				break;
			case 2:
				this.OnCellErrorAreaMouseLeaveInternal();
				break;
			}
			this.CurrentMouseLocation = 0;
			this.OnMouseLeave(rowIndex);
		}

		// Token: 0x06003247 RID: 12871 RVA: 0x000AE404 File Offset: 0x000AD404
		protected virtual void OnMouseMove(DataGridViewCellMouseEventArgs e)
		{
		}

		// Token: 0x06003248 RID: 12872 RVA: 0x000AE408 File Offset: 0x000AD408
		internal void OnMouseMoveInternal(DataGridViewCellMouseEventArgs e)
		{
			byte currentMouseLocation = this.CurrentMouseLocation;
			this.UpdateCurrentMouseLocation(e);
			switch (currentMouseLocation)
			{
			case 0:
				if (this.CurrentMouseLocation == 1)
				{
					this.OnCellDataAreaMouseEnterInternal(e.RowIndex);
				}
				else
				{
					this.OnCellErrorAreaMouseEnterInternal(e.RowIndex);
				}
				break;
			case 1:
				if (this.CurrentMouseLocation == 2)
				{
					this.OnCellDataAreaMouseLeaveInternal();
					this.OnCellErrorAreaMouseEnterInternal(e.RowIndex);
				}
				break;
			case 2:
				if (this.CurrentMouseLocation == 1)
				{
					this.OnCellErrorAreaMouseLeaveInternal();
					this.OnCellDataAreaMouseEnterInternal(e.RowIndex);
				}
				break;
			}
			this.OnMouseMove(e);
		}

		// Token: 0x06003249 RID: 12873 RVA: 0x000AE49D File Offset: 0x000AD49D
		protected virtual void OnMouseUp(DataGridViewCellMouseEventArgs e)
		{
		}

		// Token: 0x0600324A RID: 12874 RVA: 0x000AE4A0 File Offset: 0x000AD4A0
		internal void OnMouseUpInternal(DataGridViewCellMouseEventArgs e)
		{
			int x = e.X;
			int y = e.Y;
			if (((this.ColumnIndex < 0 || e.RowIndex < 0) && base.DataGridView.ApplyVisualStylesToHeaderCells) || (this.ColumnIndex >= 0 && e.RowIndex >= 0 && base.DataGridView.ApplyVisualStylesToInnerCells))
			{
				base.DataGridView.InvalidateCell(this.ColumnIndex, e.RowIndex);
			}
			if (e.Button == MouseButtons.Left && this.GetContentBounds(e.RowIndex).Contains(x, y))
			{
				base.DataGridView.OnCommonCellContentClick(e.ColumnIndex, e.RowIndex, e.Clicks > 1);
			}
			if (base.DataGridView != null && e.ColumnIndex < base.DataGridView.Columns.Count && e.RowIndex < base.DataGridView.Rows.Count)
			{
				this.OnMouseUp(e);
			}
		}

		// Token: 0x0600324B RID: 12875 RVA: 0x000AE594 File Offset: 0x000AD594
		protected override void OnDataGridViewChanged()
		{
			if (this.HasStyle)
			{
				if (base.DataGridView == null)
				{
					this.Style.RemoveScope(DataGridViewCellStyleScopes.Cell);
				}
				else
				{
					this.Style.AddScope(base.DataGridView, DataGridViewCellStyleScopes.Cell);
				}
			}
			base.OnDataGridViewChanged();
		}

		// Token: 0x0600324C RID: 12876 RVA: 0x000AE5CC File Offset: 0x000AD5CC
		protected virtual void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
		}

		// Token: 0x0600324D RID: 12877 RVA: 0x000AE5D0 File Offset: 0x000AD5D0
		internal void PaintInternal(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			this.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
		}

		// Token: 0x0600324E RID: 12878 RVA: 0x000AE5F6 File Offset: 0x000AD5F6
		internal static bool PaintBackground(DataGridViewPaintParts paintParts)
		{
			return (paintParts & DataGridViewPaintParts.Background) != DataGridViewPaintParts.None;
		}

		// Token: 0x0600324F RID: 12879 RVA: 0x000AE601 File Offset: 0x000AD601
		internal static bool PaintBorder(DataGridViewPaintParts paintParts)
		{
			return (paintParts & DataGridViewPaintParts.Border) != DataGridViewPaintParts.None;
		}

		// Token: 0x06003250 RID: 12880 RVA: 0x000AE60C File Offset: 0x000AD60C
		protected virtual void PaintBorder(Graphics graphics, Rectangle clipBounds, Rectangle bounds, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			if (base.DataGridView == null)
			{
				return;
			}
			Pen pen = null;
			Pen pen2 = null;
			Pen cachedPen = base.DataGridView.GetCachedPen(cellStyle.BackColor);
			Pen gridPen = base.DataGridView.GridPen;
			this.GetContrastedPens(cellStyle.BackColor, ref pen, ref pen2);
			int num = ((this.owningColumn == null) ? 0 : this.owningColumn.DividerWidth);
			if (num != 0)
			{
				if (num > bounds.Width)
				{
					num = bounds.Width;
				}
				Color color;
				switch (advancedBorderStyle.Right)
				{
				case DataGridViewAdvancedCellBorderStyle.Single:
					color = base.DataGridView.GridPen.Color;
					break;
				case DataGridViewAdvancedCellBorderStyle.Inset:
					color = SystemColors.ControlLightLight;
					break;
				default:
					color = SystemColors.ControlDark;
					break;
				}
				graphics.FillRectangle(base.DataGridView.GetCachedBrush(color), base.DataGridView.RightToLeftInternal ? bounds.X : (bounds.Right - num), bounds.Y, num, bounds.Height);
				if (base.DataGridView.RightToLeftInternal)
				{
					bounds.X += num;
				}
				bounds.Width -= num;
				if (bounds.Width <= 0)
				{
					return;
				}
			}
			num = ((this.owningRow == null) ? 0 : this.owningRow.DividerHeight);
			if (num != 0)
			{
				if (num > bounds.Height)
				{
					num = bounds.Height;
				}
				Color color2;
				switch (advancedBorderStyle.Bottom)
				{
				case DataGridViewAdvancedCellBorderStyle.Single:
					color2 = base.DataGridView.GridPen.Color;
					break;
				case DataGridViewAdvancedCellBorderStyle.Inset:
					color2 = SystemColors.ControlLightLight;
					break;
				default:
					color2 = SystemColors.ControlDark;
					break;
				}
				graphics.FillRectangle(base.DataGridView.GetCachedBrush(color2), bounds.X, bounds.Bottom - num, bounds.Width, num);
				bounds.Height -= num;
				if (bounds.Height <= 0)
				{
					return;
				}
			}
			if (advancedBorderStyle.All == DataGridViewAdvancedCellBorderStyle.None)
			{
				return;
			}
			switch (advancedBorderStyle.Left)
			{
			case DataGridViewAdvancedCellBorderStyle.Single:
				graphics.DrawLine(gridPen, bounds.X, bounds.Y, bounds.X, bounds.Bottom - 1);
				break;
			case DataGridViewAdvancedCellBorderStyle.Inset:
				graphics.DrawLine(pen, bounds.X, bounds.Y, bounds.X, bounds.Bottom - 1);
				break;
			case DataGridViewAdvancedCellBorderStyle.InsetDouble:
			{
				int num2 = bounds.Y + 1;
				int num3 = bounds.Bottom - 1;
				if (advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.OutsetPartial || advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.None)
				{
					num2--;
				}
				if (advancedBorderStyle.Bottom == DataGridViewAdvancedCellBorderStyle.OutsetPartial)
				{
					num3++;
				}
				graphics.DrawLine(pen2, bounds.X, bounds.Y, bounds.X, bounds.Bottom - 1);
				graphics.DrawLine(pen, bounds.X + 1, num2, bounds.X + 1, num3);
				break;
			}
			case DataGridViewAdvancedCellBorderStyle.Outset:
				graphics.DrawLine(pen2, bounds.X, bounds.Y, bounds.X, bounds.Bottom - 1);
				break;
			case DataGridViewAdvancedCellBorderStyle.OutsetDouble:
			{
				int num2 = bounds.Y + 1;
				int num3 = bounds.Bottom - 1;
				if (advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.OutsetPartial || advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.None)
				{
					num2--;
				}
				if (advancedBorderStyle.Bottom == DataGridViewAdvancedCellBorderStyle.OutsetPartial)
				{
					num3++;
				}
				graphics.DrawLine(pen, bounds.X, bounds.Y, bounds.X, bounds.Bottom - 1);
				graphics.DrawLine(pen2, bounds.X + 1, num2, bounds.X + 1, num3);
				break;
			}
			case DataGridViewAdvancedCellBorderStyle.OutsetPartial:
			{
				int num2 = bounds.Y + 2;
				int num3 = bounds.Bottom - 3;
				if (advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.OutsetDouble || advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.InsetDouble)
				{
					num2++;
				}
				else if (advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.None)
				{
					num2--;
				}
				graphics.DrawLine(cachedPen, bounds.X, bounds.Y, bounds.X, bounds.Bottom - 1);
				graphics.DrawLine(pen2, bounds.X, num2, bounds.X, num3);
				break;
			}
			}
			switch (advancedBorderStyle.Right)
			{
			case DataGridViewAdvancedCellBorderStyle.Single:
				graphics.DrawLine(gridPen, bounds.Right - 1, bounds.Y, bounds.Right - 1, bounds.Bottom - 1);
				break;
			case DataGridViewAdvancedCellBorderStyle.Inset:
				graphics.DrawLine(pen2, bounds.Right - 1, bounds.Y, bounds.Right - 1, bounds.Bottom - 1);
				break;
			case DataGridViewAdvancedCellBorderStyle.InsetDouble:
			{
				int num2 = bounds.Y + 1;
				int num3 = bounds.Bottom - 1;
				if (advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.OutsetPartial || advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.None)
				{
					num2--;
				}
				if (advancedBorderStyle.Bottom == DataGridViewAdvancedCellBorderStyle.OutsetPartial || advancedBorderStyle.Bottom == DataGridViewAdvancedCellBorderStyle.Inset)
				{
					num3++;
				}
				graphics.DrawLine(pen2, bounds.Right - 2, bounds.Y, bounds.Right - 2, bounds.Bottom - 1);
				graphics.DrawLine(pen, bounds.Right - 1, num2, bounds.Right - 1, num3);
				break;
			}
			case DataGridViewAdvancedCellBorderStyle.Outset:
				graphics.DrawLine(pen, bounds.Right - 1, bounds.Y, bounds.Right - 1, bounds.Bottom - 1);
				break;
			case DataGridViewAdvancedCellBorderStyle.OutsetDouble:
			{
				int num2 = bounds.Y + 1;
				int num3 = bounds.Bottom - 1;
				if (advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.OutsetPartial || advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.None)
				{
					num2--;
				}
				if (advancedBorderStyle.Bottom == DataGridViewAdvancedCellBorderStyle.OutsetPartial)
				{
					num3++;
				}
				graphics.DrawLine(pen, bounds.Right - 2, bounds.Y, bounds.Right - 2, bounds.Bottom - 1);
				graphics.DrawLine(pen2, bounds.Right - 1, num2, bounds.Right - 1, num3);
				break;
			}
			case DataGridViewAdvancedCellBorderStyle.OutsetPartial:
			{
				int num2 = bounds.Y + 2;
				int num3 = bounds.Bottom - 3;
				if (advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.OutsetDouble || advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.InsetDouble)
				{
					num2++;
				}
				else if (advancedBorderStyle.Top == DataGridViewAdvancedCellBorderStyle.None)
				{
					num2--;
				}
				graphics.DrawLine(cachedPen, bounds.Right - 1, bounds.Y, bounds.Right - 1, bounds.Bottom - 1);
				graphics.DrawLine(pen, bounds.Right - 1, num2, bounds.Right - 1, num3);
				break;
			}
			}
			switch (advancedBorderStyle.Top)
			{
			case DataGridViewAdvancedCellBorderStyle.Single:
				graphics.DrawLine(gridPen, bounds.X, bounds.Y, bounds.Right - 1, bounds.Y);
				break;
			case DataGridViewAdvancedCellBorderStyle.Inset:
			{
				int num4 = bounds.X;
				int num5 = bounds.Right - 1;
				if (advancedBorderStyle.Left == DataGridViewAdvancedCellBorderStyle.OutsetDouble || advancedBorderStyle.Left == DataGridViewAdvancedCellBorderStyle.InsetDouble)
				{
					num4++;
				}
				if (advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.Inset || advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.Outset)
				{
					num5--;
				}
				graphics.DrawLine(pen, num4, bounds.Y, num5, bounds.Y);
				break;
			}
			case DataGridViewAdvancedCellBorderStyle.InsetDouble:
			{
				int num4 = bounds.X;
				if (advancedBorderStyle.Left != DataGridViewAdvancedCellBorderStyle.OutsetPartial && advancedBorderStyle.Left != DataGridViewAdvancedCellBorderStyle.None)
				{
					num4++;
				}
				int num5 = bounds.Right - 2;
				if (advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.OutsetPartial || advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.None)
				{
					num5++;
				}
				graphics.DrawLine(pen2, bounds.X, bounds.Y, bounds.Right - 1, bounds.Y);
				graphics.DrawLine(pen, num4, bounds.Y + 1, num5, bounds.Y + 1);
				break;
			}
			case DataGridViewAdvancedCellBorderStyle.Outset:
			{
				int num4 = bounds.X;
				int num5 = bounds.Right - 1;
				if (advancedBorderStyle.Left == DataGridViewAdvancedCellBorderStyle.OutsetDouble || advancedBorderStyle.Left == DataGridViewAdvancedCellBorderStyle.InsetDouble)
				{
					num4++;
				}
				if (advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.Inset || advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.Outset)
				{
					num5--;
				}
				graphics.DrawLine(pen2, num4, bounds.Y, num5, bounds.Y);
				break;
			}
			case DataGridViewAdvancedCellBorderStyle.OutsetDouble:
			{
				int num4 = bounds.X;
				if (advancedBorderStyle.Left != DataGridViewAdvancedCellBorderStyle.OutsetPartial && advancedBorderStyle.Left != DataGridViewAdvancedCellBorderStyle.None)
				{
					num4++;
				}
				int num5 = bounds.Right - 2;
				if (advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.OutsetPartial || advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.None)
				{
					num5++;
				}
				graphics.DrawLine(pen, bounds.X, bounds.Y, bounds.Right - 1, bounds.Y);
				graphics.DrawLine(pen2, num4, bounds.Y + 1, num5, bounds.Y + 1);
				break;
			}
			case DataGridViewAdvancedCellBorderStyle.OutsetPartial:
			{
				int num4 = bounds.X;
				int num5 = bounds.Right - 1;
				if (advancedBorderStyle.Left != DataGridViewAdvancedCellBorderStyle.None)
				{
					num4++;
					if (advancedBorderStyle.Left == DataGridViewAdvancedCellBorderStyle.OutsetDouble || advancedBorderStyle.Left == DataGridViewAdvancedCellBorderStyle.InsetDouble)
					{
						num4++;
					}
				}
				if (advancedBorderStyle.Right != DataGridViewAdvancedCellBorderStyle.None)
				{
					num5--;
					if (advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.OutsetDouble || advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.InsetDouble)
					{
						num5--;
					}
				}
				graphics.DrawLine(cachedPen, num4, bounds.Y, num5, bounds.Y);
				graphics.DrawLine(pen2, num4 + 1, bounds.Y, num5 - 1, bounds.Y);
				break;
			}
			}
			switch (advancedBorderStyle.Bottom)
			{
			case DataGridViewAdvancedCellBorderStyle.Single:
				graphics.DrawLine(gridPen, bounds.X, bounds.Bottom - 1, bounds.Right - 1, bounds.Bottom - 1);
				return;
			case DataGridViewAdvancedCellBorderStyle.Inset:
			{
				int num5 = bounds.Right - 1;
				if (advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.InsetDouble)
				{
					num5--;
				}
				graphics.DrawLine(pen2, bounds.X, bounds.Bottom - 1, num5, bounds.Bottom - 1);
				return;
			}
			case DataGridViewAdvancedCellBorderStyle.InsetDouble:
			case DataGridViewAdvancedCellBorderStyle.OutsetDouble:
				break;
			case DataGridViewAdvancedCellBorderStyle.Outset:
			{
				int num4 = bounds.X;
				int num5 = bounds.Right - 1;
				if (advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.InsetDouble || advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.OutsetDouble)
				{
					num5--;
				}
				graphics.DrawLine(pen, num4, bounds.Bottom - 1, num5, bounds.Bottom - 1);
				return;
			}
			case DataGridViewAdvancedCellBorderStyle.OutsetPartial:
			{
				int num4 = bounds.X;
				int num5 = bounds.Right - 1;
				if (advancedBorderStyle.Left != DataGridViewAdvancedCellBorderStyle.None)
				{
					num4++;
					if (advancedBorderStyle.Left == DataGridViewAdvancedCellBorderStyle.OutsetDouble || advancedBorderStyle.Left == DataGridViewAdvancedCellBorderStyle.InsetDouble)
					{
						num4++;
					}
				}
				if (advancedBorderStyle.Right != DataGridViewAdvancedCellBorderStyle.None)
				{
					num5--;
					if (advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.OutsetDouble || advancedBorderStyle.Right == DataGridViewAdvancedCellBorderStyle.InsetDouble)
					{
						num5--;
					}
				}
				graphics.DrawLine(cachedPen, num4, bounds.Bottom - 1, num5, bounds.Bottom - 1);
				graphics.DrawLine(pen, num4 + 1, bounds.Bottom - 1, num5 - 1, bounds.Bottom - 1);
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x06003251 RID: 12881 RVA: 0x000AF100 File Offset: 0x000AE100
		internal static bool PaintContentBackground(DataGridViewPaintParts paintParts)
		{
			return (paintParts & DataGridViewPaintParts.ContentBackground) != DataGridViewPaintParts.None;
		}

		// Token: 0x06003252 RID: 12882 RVA: 0x000AF10B File Offset: 0x000AE10B
		internal static bool PaintContentForeground(DataGridViewPaintParts paintParts)
		{
			return (paintParts & DataGridViewPaintParts.ContentForeground) != DataGridViewPaintParts.None;
		}

		// Token: 0x06003253 RID: 12883 RVA: 0x000AF116 File Offset: 0x000AE116
		protected virtual void PaintErrorIcon(Graphics graphics, Rectangle clipBounds, Rectangle cellValueBounds, string errorText)
		{
			if (!string.IsNullOrEmpty(errorText) && cellValueBounds.Width >= 20 && cellValueBounds.Height >= 19)
			{
				DataGridViewCell.PaintErrorIcon(graphics, this.ComputeErrorIconBounds(cellValueBounds));
			}
		}

		// Token: 0x06003254 RID: 12884 RVA: 0x000AF144 File Offset: 0x000AE144
		private static void PaintErrorIcon(Graphics graphics, Rectangle iconBounds)
		{
			Bitmap errorBitmap = DataGridViewCell.ErrorBitmap;
			if (errorBitmap != null)
			{
				lock (errorBitmap)
				{
					graphics.DrawImage(errorBitmap, iconBounds, 0, 0, 12, 11, GraphicsUnit.Pixel);
				}
			}
		}

		// Token: 0x06003255 RID: 12885 RVA: 0x000AF18C File Offset: 0x000AE18C
		internal void PaintErrorIcon(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Rectangle cellBounds, Rectangle cellValueBounds, string errorText)
		{
			if (!string.IsNullOrEmpty(errorText) && cellValueBounds.Width >= 20 && cellValueBounds.Height >= 19)
			{
				Rectangle errorIconBounds = this.GetErrorIconBounds(graphics, cellStyle, rowIndex);
				if (errorIconBounds.Width >= 4 && errorIconBounds.Height >= 11)
				{
					errorIconBounds.X += cellBounds.X;
					errorIconBounds.Y += cellBounds.Y;
					DataGridViewCell.PaintErrorIcon(graphics, errorIconBounds);
				}
			}
		}

		// Token: 0x06003256 RID: 12886 RVA: 0x000AF208 File Offset: 0x000AE208
		internal static bool PaintErrorIcon(DataGridViewPaintParts paintParts)
		{
			return (paintParts & DataGridViewPaintParts.ErrorIcon) != DataGridViewPaintParts.None;
		}

		// Token: 0x06003257 RID: 12887 RVA: 0x000AF214 File Offset: 0x000AE214
		internal static bool PaintFocus(DataGridViewPaintParts paintParts)
		{
			return (paintParts & DataGridViewPaintParts.Focus) != DataGridViewPaintParts.None;
		}

		// Token: 0x06003258 RID: 12888 RVA: 0x000AF220 File Offset: 0x000AE220
		internal static void PaintPadding(Graphics graphics, Rectangle bounds, DataGridViewCellStyle cellStyle, Brush br, bool rightToLeft)
		{
			Rectangle rectangle;
			if (rightToLeft)
			{
				rectangle = new Rectangle(bounds.X, bounds.Y, cellStyle.Padding.Right, bounds.Height);
				graphics.FillRectangle(br, rectangle);
				rectangle.X = bounds.Right - cellStyle.Padding.Left;
				rectangle.Width = cellStyle.Padding.Left;
				graphics.FillRectangle(br, rectangle);
				rectangle.X = bounds.Left + cellStyle.Padding.Right;
			}
			else
			{
				rectangle = new Rectangle(bounds.X, bounds.Y, cellStyle.Padding.Left, bounds.Height);
				graphics.FillRectangle(br, rectangle);
				rectangle.X = bounds.Right - cellStyle.Padding.Right;
				rectangle.Width = cellStyle.Padding.Right;
				graphics.FillRectangle(br, rectangle);
				rectangle.X = bounds.Left + cellStyle.Padding.Left;
			}
			rectangle.Y = bounds.Y;
			rectangle.Width = bounds.Width - cellStyle.Padding.Horizontal;
			rectangle.Height = cellStyle.Padding.Top;
			graphics.FillRectangle(br, rectangle);
			rectangle.Y = bounds.Bottom - cellStyle.Padding.Bottom;
			rectangle.Height = cellStyle.Padding.Bottom;
			graphics.FillRectangle(br, rectangle);
		}

		// Token: 0x06003259 RID: 12889 RVA: 0x000AF3D6 File Offset: 0x000AE3D6
		internal static bool PaintSelectionBackground(DataGridViewPaintParts paintParts)
		{
			return (paintParts & DataGridViewPaintParts.SelectionBackground) != DataGridViewPaintParts.None;
		}

		// Token: 0x0600325A RID: 12890 RVA: 0x000AF3E4 File Offset: 0x000AE3E4
		internal void PaintWork(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			DataGridView dataGridView = base.DataGridView;
			int columnIndex = this.ColumnIndex;
			object value = this.GetValue(rowIndex);
			string errorText = this.GetErrorText(rowIndex);
			object obj;
			if (columnIndex > -1 && rowIndex > -1)
			{
				obj = this.GetEditedFormattedValue(value, rowIndex, ref cellStyle, DataGridViewDataErrorContexts.Formatting | DataGridViewDataErrorContexts.Display);
			}
			else
			{
				obj = value;
			}
			DataGridViewCellPaintingEventArgs cellPaintingEventArgs = dataGridView.CellPaintingEventArgs;
			cellPaintingEventArgs.SetProperties(graphics, clipBounds, cellBounds, rowIndex, columnIndex, cellState, value, obj, errorText, cellStyle, advancedBorderStyle, paintParts);
			dataGridView.OnCellPainting(cellPaintingEventArgs);
			if (cellPaintingEventArgs.Handled)
			{
				return;
			}
			this.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, obj, errorText, cellStyle, advancedBorderStyle, paintParts);
		}

		// Token: 0x0600325B RID: 12891 RVA: 0x000AF476 File Offset: 0x000AE476
		public virtual object ParseFormattedValue(object formattedValue, DataGridViewCellStyle cellStyle, TypeConverter formattedValueTypeConverter, TypeConverter valueTypeConverter)
		{
			return this.ParseFormattedValueInternal(this.ValueType, formattedValue, cellStyle, formattedValueTypeConverter, valueTypeConverter);
		}

		// Token: 0x0600325C RID: 12892 RVA: 0x000AF48C File Offset: 0x000AE48C
		internal object ParseFormattedValueInternal(Type valueType, object formattedValue, DataGridViewCellStyle cellStyle, TypeConverter formattedValueTypeConverter, TypeConverter valueTypeConverter)
		{
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			if (this.FormattedValueType == null)
			{
				throw new FormatException(SR.GetString("DataGridViewCell_FormattedValueTypeNull"));
			}
			if (valueType == null)
			{
				throw new FormatException(SR.GetString("DataGridViewCell_ValueTypeNull"));
			}
			if (formattedValue == null || !this.FormattedValueType.IsAssignableFrom(formattedValue.GetType()))
			{
				throw new ArgumentException(SR.GetString("DataGridViewCell_FormattedValueHasWrongType"), "formattedValue");
			}
			return Formatter.ParseObject(formattedValue, valueType, this.FormattedValueType, (valueTypeConverter == null) ? this.ValueTypeConverter : valueTypeConverter, (formattedValueTypeConverter == null) ? this.FormattedValueTypeConverter : formattedValueTypeConverter, cellStyle.FormatProvider, cellStyle.NullValue, cellStyle.IsDataSourceNullValueDefault ? Formatter.GetDefaultDataSourceNullValue(valueType) : cellStyle.DataSourceNullValue);
		}

		// Token: 0x0600325D RID: 12893 RVA: 0x000AF548 File Offset: 0x000AE548
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual void PositionEditingControl(bool setLocation, bool setSize, Rectangle cellBounds, Rectangle cellClip, DataGridViewCellStyle cellStyle, bool singleVerticalBorderAdded, bool singleHorizontalBorderAdded, bool isFirstDisplayedColumn, bool isFirstDisplayedRow)
		{
			Rectangle rectangle = this.PositionEditingPanel(cellBounds, cellClip, cellStyle, singleVerticalBorderAdded, singleHorizontalBorderAdded, isFirstDisplayedColumn, isFirstDisplayedRow);
			if (setLocation)
			{
				base.DataGridView.EditingControl.Location = new Point(rectangle.X, rectangle.Y);
			}
			if (setSize)
			{
				base.DataGridView.EditingControl.Size = new Size(rectangle.Width, rectangle.Height);
			}
		}

		// Token: 0x0600325E RID: 12894 RVA: 0x000AF5B8 File Offset: 0x000AE5B8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual Rectangle PositionEditingPanel(Rectangle cellBounds, Rectangle cellClip, DataGridViewCellStyle cellStyle, bool singleVerticalBorderAdded, bool singleHorizontalBorderAdded, bool isFirstDisplayedColumn, bool isFirstDisplayedRow)
		{
			if (base.DataGridView == null)
			{
				throw new InvalidOperationException();
			}
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle = new DataGridViewAdvancedBorderStyle();
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle2 = this.AdjustCellBorderStyle(base.DataGridView.AdvancedCellBorderStyle, dataGridViewAdvancedBorderStyle, singleVerticalBorderAdded, singleHorizontalBorderAdded, isFirstDisplayedColumn, isFirstDisplayedRow);
			Rectangle rectangle = this.BorderWidths(dataGridViewAdvancedBorderStyle2);
			rectangle.X += cellStyle.Padding.Left;
			rectangle.Y += cellStyle.Padding.Top;
			rectangle.Width += cellStyle.Padding.Right;
			rectangle.Height += cellStyle.Padding.Bottom;
			int num = cellBounds.Width;
			int num2 = cellBounds.Height;
			int num3;
			if (cellClip.X - cellBounds.X >= rectangle.X)
			{
				num3 = cellClip.X;
				num -= cellClip.X - cellBounds.X;
			}
			else
			{
				num3 = cellBounds.X + rectangle.X;
				num -= rectangle.X;
			}
			if (cellClip.Right <= cellBounds.Right - rectangle.Width)
			{
				num -= cellBounds.Right - cellClip.Right;
			}
			else
			{
				num -= rectangle.Width;
			}
			int num4 = cellBounds.X - cellClip.X;
			int num5 = cellBounds.Width - rectangle.X - rectangle.Width;
			int num6;
			if (cellClip.Y - cellBounds.Y >= rectangle.Y)
			{
				num6 = cellClip.Y;
				num2 -= cellClip.Y - cellBounds.Y;
			}
			else
			{
				num6 = cellBounds.Y + rectangle.Y;
				num2 -= rectangle.Y;
			}
			if (cellClip.Bottom <= cellBounds.Bottom - rectangle.Height)
			{
				num2 -= cellBounds.Bottom - cellClip.Bottom;
			}
			else
			{
				num2 -= rectangle.Height;
			}
			int num7 = cellBounds.Y - cellClip.Y;
			int num8 = cellBounds.Height - rectangle.Y - rectangle.Height;
			base.DataGridView.EditingPanel.Location = new Point(num3, num6);
			base.DataGridView.EditingPanel.Size = new Size(num, num2);
			return new Rectangle(num4, num7, num5, num8);
		}

		// Token: 0x0600325F RID: 12895 RVA: 0x000AF830 File Offset: 0x000AE830
		protected virtual bool SetValue(int rowIndex, object value)
		{
			object obj = null;
			DataGridView dataGridView = base.DataGridView;
			if (dataGridView != null && !dataGridView.InSortOperation)
			{
				obj = this.GetValue(rowIndex);
			}
			if (dataGridView != null && this.OwningColumn != null && this.OwningColumn.IsDataBound)
			{
				DataGridView.DataGridViewDataConnection dataConnection = dataGridView.DataConnection;
				if (dataConnection == null)
				{
					return false;
				}
				if (dataConnection.CurrencyManager.Count <= rowIndex)
				{
					if (value != null || this.Properties.ContainsObject(DataGridViewCell.PropCellValue))
					{
						this.Properties.SetObject(DataGridViewCell.PropCellValue, value);
					}
				}
				else
				{
					if (!dataConnection.PushValue(this.OwningColumn.BoundColumnIndex, this.ColumnIndex, rowIndex, value))
					{
						return false;
					}
					if (base.DataGridView == null || this.OwningRow == null || this.OwningRow.DataGridView == null)
					{
						return true;
					}
					if (this.OwningRow.Index == base.DataGridView.CurrentCellAddress.Y)
					{
						base.DataGridView.IsCurrentRowDirtyInternal = true;
					}
				}
			}
			else if (dataGridView == null || !dataGridView.VirtualMode || rowIndex == -1 || this.ColumnIndex == -1)
			{
				if (value != null || this.Properties.ContainsObject(DataGridViewCell.PropCellValue))
				{
					this.Properties.SetObject(DataGridViewCell.PropCellValue, value);
				}
			}
			else
			{
				dataGridView.OnCellValuePushed(this.ColumnIndex, rowIndex, value);
			}
			if (dataGridView != null && !dataGridView.InSortOperation && ((obj == null && value != null) || (obj != null && value == null) || (obj != null && !value.Equals(obj))))
			{
				base.RaiseCellValueChanged(new DataGridViewCellEventArgs(this.ColumnIndex, rowIndex));
			}
			return true;
		}

		// Token: 0x06003260 RID: 12896 RVA: 0x000AF9B0 File Offset: 0x000AE9B0
		internal bool SetValueInternal(int rowIndex, object value)
		{
			return this.SetValue(rowIndex, value);
		}

		// Token: 0x06003261 RID: 12897 RVA: 0x000AF9BC File Offset: 0x000AE9BC
		internal static bool TextFitsInBounds(Graphics graphics, string text, Font font, Size maxBounds, TextFormatFlags flags)
		{
			bool flag;
			int num = DataGridViewCell.MeasureTextHeight(graphics, text, font, maxBounds.Width, flags, out flag);
			return num <= maxBounds.Height && !flag;
		}

		// Token: 0x06003262 RID: 12898 RVA: 0x000AF9F0 File Offset: 0x000AE9F0
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"DataGridViewCell { ColumnIndex=",
				this.ColumnIndex.ToString(CultureInfo.CurrentCulture),
				", RowIndex=",
				this.RowIndex.ToString(CultureInfo.CurrentCulture),
				" }"
			});
		}

		// Token: 0x06003263 RID: 12899 RVA: 0x000AFA50 File Offset: 0x000AEA50
		private static string TruncateToolTipText(string toolTipText)
		{
			if (toolTipText.Length > 288)
			{
				StringBuilder stringBuilder = new StringBuilder(toolTipText.Substring(0, 256), 259);
				stringBuilder.Append("...");
				return stringBuilder.ToString();
			}
			return toolTipText;
		}

		// Token: 0x06003264 RID: 12900 RVA: 0x000AFA98 File Offset: 0x000AEA98
		private void UpdateCurrentMouseLocation(DataGridViewCellMouseEventArgs e)
		{
			if (this.GetErrorIconBounds(e.RowIndex).Contains(e.X, e.Y))
			{
				this.CurrentMouseLocation = 2;
				return;
			}
			this.CurrentMouseLocation = 1;
		}

		// Token: 0x04001A52 RID: 6738
		private const TextFormatFlags textFormatSupportedFlags = TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.WordBreak;

		// Token: 0x04001A53 RID: 6739
		private const int DATAGRIDVIEWCELL_constrastThreshold = 1000;

		// Token: 0x04001A54 RID: 6740
		private const int DATAGRIDVIEWCELL_highConstrastThreshold = 2000;

		// Token: 0x04001A55 RID: 6741
		private const int DATAGRIDVIEWCELL_maxToolTipLength = 288;

		// Token: 0x04001A56 RID: 6742
		private const int DATAGRIDVIEWCELL_maxToolTipCutOff = 256;

		// Token: 0x04001A57 RID: 6743
		private const int DATAGRIDVIEWCELL_toolTipEllipsisLength = 3;

		// Token: 0x04001A58 RID: 6744
		private const string DATAGRIDVIEWCELL_toolTipEllipsis = "...";

		// Token: 0x04001A59 RID: 6745
		private const byte DATAGRIDVIEWCELL_flagAreaNotSet = 0;

		// Token: 0x04001A5A RID: 6746
		private const byte DATAGRIDVIEWCELL_flagDataArea = 1;

		// Token: 0x04001A5B RID: 6747
		private const byte DATAGRIDVIEWCELL_flagErrorArea = 2;

		// Token: 0x04001A5C RID: 6748
		internal const byte DATAGRIDVIEWCELL_iconMarginWidth = 4;

		// Token: 0x04001A5D RID: 6749
		internal const byte DATAGRIDVIEWCELL_iconMarginHeight = 4;

		// Token: 0x04001A5E RID: 6750
		internal const byte DATAGRIDVIEWCELL_iconsWidth = 12;

		// Token: 0x04001A5F RID: 6751
		internal const byte DATAGRIDVIEWCELL_iconsHeight = 11;

		// Token: 0x04001A60 RID: 6752
		internal static readonly int PropCellValue = PropertyStore.CreateKey();

		// Token: 0x04001A61 RID: 6753
		private static readonly int PropCellContextMenuStrip = PropertyStore.CreateKey();

		// Token: 0x04001A62 RID: 6754
		private static readonly int PropCellErrorText = PropertyStore.CreateKey();

		// Token: 0x04001A63 RID: 6755
		private static readonly int PropCellStyle = PropertyStore.CreateKey();

		// Token: 0x04001A64 RID: 6756
		private static readonly int PropCellValueType = PropertyStore.CreateKey();

		// Token: 0x04001A65 RID: 6757
		private static readonly int PropCellTag = PropertyStore.CreateKey();

		// Token: 0x04001A66 RID: 6758
		private static readonly int PropCellToolTipText = PropertyStore.CreateKey();

		// Token: 0x04001A67 RID: 6759
		private static readonly int PropCellAccessibilityObject = PropertyStore.CreateKey();

		// Token: 0x04001A68 RID: 6760
		private static Bitmap errorBmp = null;

		// Token: 0x04001A69 RID: 6761
		private PropertyStore propertyStore;

		// Token: 0x04001A6A RID: 6762
		private DataGridViewRow owningRow;

		// Token: 0x04001A6B RID: 6763
		private DataGridViewColumn owningColumn;

		// Token: 0x04001A6C RID: 6764
		private static Type stringType = typeof(string);

		// Token: 0x04001A6D RID: 6765
		private byte flags;

		// Token: 0x02000309 RID: 777
		[ComVisible(true)]
		protected class DataGridViewCellAccessibleObject : AccessibleObject
		{
			// Token: 0x06003266 RID: 12902 RVA: 0x000AFB4A File Offset: 0x000AEB4A
			public DataGridViewCellAccessibleObject()
			{
			}

			// Token: 0x06003267 RID: 12903 RVA: 0x000AFB52 File Offset: 0x000AEB52
			public DataGridViewCellAccessibleObject(DataGridViewCell owner)
			{
				this.owner = owner;
			}

			// Token: 0x170008BA RID: 2234
			// (get) Token: 0x06003268 RID: 12904 RVA: 0x000AFB61 File Offset: 0x000AEB61
			public override Rectangle Bounds
			{
				get
				{
					return this.GetAccessibleObjectBounds(this.GetAccessibleObjectParent());
				}
			}

			// Token: 0x170008BB RID: 2235
			// (get) Token: 0x06003269 RID: 12905 RVA: 0x000AFB6F File Offset: 0x000AEB6F
			public override string DefaultAction
			{
				get
				{
					if (this.Owner == null)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
					}
					if (!this.Owner.ReadOnly)
					{
						return SR.GetString("DataGridView_AccCellDefaultAction");
					}
					return string.Empty;
				}
			}

			// Token: 0x170008BC RID: 2236
			// (get) Token: 0x0600326A RID: 12906 RVA: 0x000AFBA6 File Offset: 0x000AEBA6
			public override string Help
			{
				get
				{
					return this.owner.GetType().Name + "(" + this.owner.GetType().BaseType.Name + ")";
				}
			}

			// Token: 0x170008BD RID: 2237
			// (get) Token: 0x0600326B RID: 12907 RVA: 0x000AFBDC File Offset: 0x000AEBDC
			public override string Name
			{
				get
				{
					if (this.owner == null)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
					}
					if (this.owner.OwningColumn != null)
					{
						return SR.GetString("DataGridView_AccDataGridViewCellName", new object[]
						{
							this.owner.OwningColumn.HeaderText,
							this.owner.OwningRow.Index
						});
					}
					return string.Empty;
				}
			}

			// Token: 0x170008BE RID: 2238
			// (get) Token: 0x0600326C RID: 12908 RVA: 0x000AFC51 File Offset: 0x000AEC51
			// (set) Token: 0x0600326D RID: 12909 RVA: 0x000AFC59 File Offset: 0x000AEC59
			public DataGridViewCell Owner
			{
				get
				{
					return this.owner;
				}
				set
				{
					if (this.owner != null)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerAlreadySet"));
					}
					this.owner = value;
				}
			}

			// Token: 0x170008BF RID: 2239
			// (get) Token: 0x0600326E RID: 12910 RVA: 0x000AFC7A File Offset: 0x000AEC7A
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.ParentPrivate;
				}
			}

			// Token: 0x170008C0 RID: 2240
			// (get) Token: 0x0600326F RID: 12911 RVA: 0x000AFC82 File Offset: 0x000AEC82
			private AccessibleObject ParentPrivate
			{
				get
				{
					if (this.owner == null)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
					}
					if (this.owner.OwningRow == null)
					{
						return null;
					}
					return this.owner.OwningRow.AccessibilityObject;
				}
			}

			// Token: 0x170008C1 RID: 2241
			// (get) Token: 0x06003270 RID: 12912 RVA: 0x000AFCBB File Offset: 0x000AECBB
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.Cell;
				}
			}

			// Token: 0x170008C2 RID: 2242
			// (get) Token: 0x06003271 RID: 12913 RVA: 0x000AFCC0 File Offset: 0x000AECC0
			public override AccessibleStates State
			{
				get
				{
					if (this.owner == null)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
					}
					AccessibleStates accessibleStates = AccessibleStates.Focusable | AccessibleStates.Selectable;
					if (this.owner == this.owner.DataGridView.CurrentCell)
					{
						accessibleStates |= AccessibleStates.Focused;
					}
					if (this.owner.Selected)
					{
						accessibleStates |= AccessibleStates.Selected;
					}
					Rectangle rectangle;
					if (this.owner.OwningColumn != null && this.owner.OwningRow != null)
					{
						rectangle = this.owner.DataGridView.GetCellDisplayRectangle(this.owner.OwningColumn.Index, this.owner.OwningRow.Index, false);
					}
					else if (this.owner.OwningRow != null)
					{
						rectangle = this.owner.DataGridView.GetCellDisplayRectangle(-1, this.owner.OwningRow.Index, false);
					}
					else if (this.owner.OwningColumn != null)
					{
						rectangle = this.owner.DataGridView.GetCellDisplayRectangle(this.owner.OwningColumn.Index, -1, false);
					}
					else
					{
						rectangle = this.owner.DataGridView.GetCellDisplayRectangle(-1, -1, false);
					}
					if (!rectangle.IntersectsWith(this.owner.DataGridView.ClientRectangle))
					{
						accessibleStates |= AccessibleStates.Offscreen;
					}
					return accessibleStates;
				}
			}

			// Token: 0x170008C3 RID: 2243
			// (get) Token: 0x06003272 RID: 12914 RVA: 0x000AFE00 File Offset: 0x000AEE00
			// (set) Token: 0x06003273 RID: 12915 RVA: 0x000AFE98 File Offset: 0x000AEE98
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					if (this.owner == null)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
					}
					object formattedValue = this.owner.FormattedValue;
					string text = formattedValue as string;
					if (formattedValue == null || (text != null && string.IsNullOrEmpty(text)))
					{
						return SR.GetString("DataGridView_AccNullValue");
					}
					if (text != null)
					{
						return text;
					}
					if (this.owner.OwningColumn == null)
					{
						return string.Empty;
					}
					TypeConverter formattedValueTypeConverter = this.owner.FormattedValueTypeConverter;
					if (formattedValueTypeConverter != null && formattedValueTypeConverter.CanConvertTo(typeof(string)))
					{
						return formattedValueTypeConverter.ConvertToString(formattedValue);
					}
					return formattedValue.ToString();
				}
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				set
				{
					if (this.owner is DataGridViewHeaderCell)
					{
						return;
					}
					if (this.owner.ReadOnly)
					{
						return;
					}
					if (this.owner.OwningRow == null)
					{
						return;
					}
					if (this.owner.DataGridView.IsCurrentCellInEditMode)
					{
						this.owner.DataGridView.EndEdit();
					}
					DataGridViewCellStyle inheritedStyle = this.owner.InheritedStyle;
					object formattedValue = this.owner.GetFormattedValue(value, this.owner.OwningRow.Index, ref inheritedStyle, null, null, DataGridViewDataErrorContexts.Formatting);
					this.owner.Value = this.owner.ParseFormattedValue(formattedValue, inheritedStyle, null, null);
				}
			}

			// Token: 0x06003274 RID: 12916 RVA: 0x000AFF3C File Offset: 0x000AEF3C
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
				}
				DataGridViewCell dataGridViewCell = this.Owner;
				DataGridView dataGridView = dataGridViewCell.DataGridView;
				if (dataGridViewCell is DataGridViewHeaderCell)
				{
					return;
				}
				if (dataGridView != null && dataGridViewCell.RowIndex == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidOperationOnSharedCell"));
				}
				this.Select(AccessibleSelection.TakeFocus | AccessibleSelection.TakeSelection);
				if (dataGridViewCell.ReadOnly)
				{
					return;
				}
				if (dataGridViewCell.EditType != null)
				{
					if (dataGridView.InBeginEdit || dataGridView.InEndEdit)
					{
						return;
					}
					if (dataGridView.IsCurrentCellInEditMode)
					{
						dataGridView.EndEdit();
						return;
					}
					if (dataGridView.EditMode != DataGridViewEditMode.EditProgrammatically)
					{
						dataGridView.BeginEdit(true);
					}
				}
			}

			// Token: 0x06003275 RID: 12917 RVA: 0x000AFFE0 File Offset: 0x000AEFE0
			internal Rectangle GetAccessibleObjectBounds(AccessibleObject parentAccObject)
			{
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
				}
				if (this.owner.OwningColumn == null)
				{
					return Rectangle.Empty;
				}
				Rectangle bounds = parentAccObject.Bounds;
				int num = this.owner.DataGridView.Columns.ColumnIndexToActualDisplayIndex(this.owner.DataGridView.FirstDisplayedScrollingColumnIndex, DataGridViewElementStates.Visible);
				int num2 = this.owner.DataGridView.Columns.ColumnIndexToActualDisplayIndex(this.owner.ColumnIndex, DataGridViewElementStates.Visible);
				bool rowHeadersVisible = this.owner.DataGridView.RowHeadersVisible;
				Rectangle rectangle;
				if (num2 < num)
				{
					rectangle = parentAccObject.GetChild(num2 + 1 + (rowHeadersVisible ? 1 : 0)).Bounds;
					if (this.Owner.DataGridView.RightToLeft == RightToLeft.No)
					{
						rectangle.X -= this.owner.OwningColumn.Width;
					}
					else
					{
						rectangle.X = rectangle.Right;
					}
					rectangle.Width = this.owner.OwningColumn.Width;
				}
				else if (num2 == num)
				{
					rectangle = this.owner.DataGridView.GetColumnDisplayRectangle(this.owner.ColumnIndex, false);
					int firstDisplayedScrollingColumnHiddenWidth = this.owner.DataGridView.FirstDisplayedScrollingColumnHiddenWidth;
					if (firstDisplayedScrollingColumnHiddenWidth != 0)
					{
						if (this.owner.DataGridView.RightToLeft == RightToLeft.No)
						{
							rectangle.X -= firstDisplayedScrollingColumnHiddenWidth;
						}
						rectangle.Width += firstDisplayedScrollingColumnHiddenWidth;
					}
					rectangle = this.owner.DataGridView.RectangleToScreen(rectangle);
				}
				else
				{
					rectangle = parentAccObject.GetChild(num2 - 1 + (rowHeadersVisible ? 1 : 0)).Bounds;
					if (this.owner.DataGridView.RightToLeft == RightToLeft.No)
					{
						rectangle.X = rectangle.Right;
					}
					else
					{
						rectangle.X -= this.owner.OwningColumn.Width;
					}
					rectangle.Width = this.owner.OwningColumn.Width;
				}
				bounds.X = rectangle.X;
				bounds.Width = rectangle.Width;
				return bounds;
			}

			// Token: 0x06003276 RID: 12918 RVA: 0x000B0200 File Offset: 0x000AF200
			private AccessibleObject GetAccessibleObjectParent()
			{
				if (this.owner is DataGridViewButtonCell || this.owner is DataGridViewCheckBoxCell || this.owner is DataGridViewComboBoxCell || this.owner is DataGridViewImageCell || this.owner is DataGridViewLinkCell || this.owner is DataGridViewTextBoxCell)
				{
					return this.ParentPrivate;
				}
				return this.Parent;
			}

			// Token: 0x06003277 RID: 12919 RVA: 0x000B0268 File Offset: 0x000AF268
			public override AccessibleObject GetChild(int index)
			{
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
				}
				if (this.owner.DataGridView.EditingControl != null && this.owner.DataGridView.IsCurrentCellInEditMode && this.owner.DataGridView.CurrentCell == this.owner && index == 0)
				{
					return this.owner.DataGridView.EditingControl.AccessibilityObject;
				}
				return null;
			}

			// Token: 0x06003278 RID: 12920 RVA: 0x000B02E4 File Offset: 0x000AF2E4
			public override int GetChildCount()
			{
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
				}
				if (this.owner.DataGridView.EditingControl != null && this.owner.DataGridView.IsCurrentCellInEditMode && this.owner.DataGridView.CurrentCell == this.owner)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x06003279 RID: 12921 RVA: 0x000B0348 File Offset: 0x000AF348
			public override AccessibleObject GetFocused()
			{
				return null;
			}

			// Token: 0x0600327A RID: 12922 RVA: 0x000B034B File Offset: 0x000AF34B
			public override AccessibleObject GetSelected()
			{
				return null;
			}

			// Token: 0x0600327B RID: 12923 RVA: 0x000B0350 File Offset: 0x000AF350
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navigationDirection)
			{
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
				}
				if (this.owner.OwningColumn == null || this.owner.OwningRow == null)
				{
					return null;
				}
				switch (navigationDirection)
				{
				case AccessibleNavigation.Up:
					if (this.owner.OwningRow.Index != this.owner.DataGridView.Rows.GetFirstRow(DataGridViewElementStates.Visible))
					{
						int previousRow = this.owner.DataGridView.Rows.GetPreviousRow(this.owner.OwningRow.Index, DataGridViewElementStates.Visible);
						return this.owner.DataGridView.Rows[previousRow].Cells[this.owner.OwningColumn.Index].AccessibilityObject;
					}
					if (this.owner.DataGridView.ColumnHeadersVisible)
					{
						return this.owner.OwningColumn.HeaderCell.AccessibilityObject;
					}
					return null;
				case AccessibleNavigation.Down:
				{
					if (this.owner.OwningRow.Index == this.owner.DataGridView.Rows.GetLastRow(DataGridViewElementStates.Visible))
					{
						return null;
					}
					int nextRow = this.owner.DataGridView.Rows.GetNextRow(this.owner.OwningRow.Index, DataGridViewElementStates.Visible);
					return this.owner.DataGridView.Rows[nextRow].Cells[this.owner.OwningColumn.Index].AccessibilityObject;
				}
				case AccessibleNavigation.Left:
					if (this.owner.DataGridView.RightToLeft == RightToLeft.No)
					{
						return this.NavigateBackward(true);
					}
					return this.NavigateForward(true);
				case AccessibleNavigation.Right:
					if (this.owner.DataGridView.RightToLeft == RightToLeft.No)
					{
						return this.NavigateForward(true);
					}
					return this.NavigateBackward(true);
				case AccessibleNavigation.Next:
					return this.NavigateForward(false);
				case AccessibleNavigation.Previous:
					return this.NavigateBackward(false);
				default:
					return null;
				}
			}

			// Token: 0x0600327C RID: 12924 RVA: 0x000B0548 File Offset: 0x000AF548
			private AccessibleObject NavigateBackward(bool wrapAround)
			{
				if (this.owner.OwningColumn != this.owner.DataGridView.Columns.GetFirstColumn(DataGridViewElementStates.Visible))
				{
					int index = this.owner.DataGridView.Columns.GetPreviousColumn(this.owner.OwningColumn, DataGridViewElementStates.Visible, DataGridViewElementStates.None).Index;
					return this.owner.OwningRow.Cells[index].AccessibilityObject;
				}
				if (wrapAround)
				{
					AccessibleObject accessibleObject = this.Owner.OwningRow.AccessibilityObject.Navigate(AccessibleNavigation.Previous);
					if (accessibleObject != null && accessibleObject.GetChildCount() > 0)
					{
						return accessibleObject.GetChild(accessibleObject.GetChildCount() - 1);
					}
					return null;
				}
				else
				{
					if (this.owner.DataGridView.RowHeadersVisible)
					{
						return this.owner.OwningRow.AccessibilityObject.GetChild(0);
					}
					return null;
				}
			}

			// Token: 0x0600327D RID: 12925 RVA: 0x000B0620 File Offset: 0x000AF620
			private AccessibleObject NavigateForward(bool wrapAround)
			{
				if (this.owner.OwningColumn != this.owner.DataGridView.Columns.GetLastColumn(DataGridViewElementStates.Visible, DataGridViewElementStates.None))
				{
					int index = this.owner.DataGridView.Columns.GetNextColumn(this.owner.OwningColumn, DataGridViewElementStates.Visible, DataGridViewElementStates.None).Index;
					return this.owner.OwningRow.Cells[index].AccessibilityObject;
				}
				if (!wrapAround)
				{
					return null;
				}
				AccessibleObject accessibleObject = this.Owner.OwningRow.AccessibilityObject.Navigate(AccessibleNavigation.Next);
				if (accessibleObject == null || accessibleObject.GetChildCount() <= 0)
				{
					return null;
				}
				if (this.Owner.DataGridView.RowHeadersVisible)
				{
					return accessibleObject.GetChild(1);
				}
				return accessibleObject.GetChild(0);
			}

			// Token: 0x0600327E RID: 12926 RVA: 0x000B06E4 File Offset: 0x000AF6E4
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void Select(AccessibleSelection flags)
			{
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewCellAccessibleObject_OwnerNotSet"));
				}
				if ((flags & AccessibleSelection.TakeFocus) == AccessibleSelection.TakeFocus)
				{
					this.owner.DataGridView.FocusInternal();
				}
				if ((flags & AccessibleSelection.TakeSelection) == AccessibleSelection.TakeSelection)
				{
					this.owner.Selected = true;
					this.owner.DataGridView.CurrentCell = this.owner;
				}
				if ((flags & AccessibleSelection.AddSelection) == AccessibleSelection.AddSelection)
				{
					this.owner.Selected = true;
				}
				if ((flags & AccessibleSelection.RemoveSelection) == AccessibleSelection.RemoveSelection && (flags & (AccessibleSelection.TakeSelection | AccessibleSelection.AddSelection)) == AccessibleSelection.None)
				{
					this.owner.Selected = false;
				}
			}

			// Token: 0x04001A6E RID: 6766
			private DataGridViewCell owner;
		}
	}
}
