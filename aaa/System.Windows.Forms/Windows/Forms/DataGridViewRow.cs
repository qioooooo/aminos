using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x0200037D RID: 893
	[TypeConverter(typeof(DataGridViewRowConverter))]
	public class DataGridViewRow : DataGridViewBand
	{
		// Token: 0x060036B5 RID: 14005 RVA: 0x000C4153 File Offset: 0x000C3153
		public DataGridViewRow()
		{
			this.bandIsRow = true;
			base.MinimumThickness = 3;
			base.Thickness = Control.DefaultFont.Height + 9;
		}

		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x060036B6 RID: 14006 RVA: 0x000C417C File Offset: 0x000C317C
		[Browsable(false)]
		public AccessibleObject AccessibilityObject
		{
			get
			{
				AccessibleObject accessibleObject = (AccessibleObject)base.Properties.GetObject(DataGridViewRow.PropRowAccessibilityObject);
				if (accessibleObject == null)
				{
					accessibleObject = this.CreateAccessibilityInstance();
					base.Properties.SetObject(DataGridViewRow.PropRowAccessibilityObject, accessibleObject);
				}
				return accessibleObject;
			}
		}

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x060036B7 RID: 14007 RVA: 0x000C41BB File Offset: 0x000C31BB
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public DataGridViewCellCollection Cells
		{
			get
			{
				if (this.rowCells == null)
				{
					this.rowCells = this.CreateCellsInstance();
				}
				return this.rowCells;
			}
		}

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x060036B8 RID: 14008 RVA: 0x000C41D7 File Offset: 0x000C31D7
		// (set) Token: 0x060036B9 RID: 14009 RVA: 0x000C41DF File Offset: 0x000C31DF
		[SRCategory("CatBehavior")]
		[SRDescription("DataGridView_RowContextMenuStripDescr")]
		[DefaultValue(null)]
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

		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x060036BA RID: 14010 RVA: 0x000C41E8 File Offset: 0x000C31E8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public object DataBoundItem
		{
			get
			{
				if (base.DataGridView != null && base.DataGridView.DataConnection != null && base.Index > -1 && base.Index != base.DataGridView.NewRowIndex)
				{
					return base.DataGridView.DataConnection.CurrencyManager[base.Index];
				}
				return null;
			}
		}

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x060036BB RID: 14011 RVA: 0x000C4243 File Offset: 0x000C3243
		// (set) Token: 0x060036BC RID: 14012 RVA: 0x000C424C File Offset: 0x000C324C
		[Browsable(true)]
		[SRDescription("DataGridView_RowDefaultCellStyleDescr")]
		[NotifyParentProperty(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[SRCategory("CatAppearance")]
		public override DataGridViewCellStyle DefaultCellStyle
		{
			get
			{
				return base.DefaultCellStyle;
			}
			set
			{
				if (base.DataGridView != null && base.Index == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidPropertySetOnSharedRow", new object[] { "DefaultCellStyle" }));
				}
				base.DefaultCellStyle = value;
			}
		}

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x060036BD RID: 14013 RVA: 0x000C4294 File Offset: 0x000C3294
		[Browsable(false)]
		public override bool Displayed
		{
			get
			{
				if (base.DataGridView != null && base.Index == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidPropertyGetOnSharedRow", new object[] { "Displayed" }));
				}
				return this.GetDisplayed(base.Index);
			}
		}

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x060036BE RID: 14014 RVA: 0x000C42DE File Offset: 0x000C32DE
		// (set) Token: 0x060036BF RID: 14015 RVA: 0x000C42E8 File Offset: 0x000C32E8
		[DefaultValue(0)]
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_RowDividerHeightDescr")]
		[NotifyParentProperty(true)]
		public int DividerHeight
		{
			get
			{
				return base.DividerThickness;
			}
			set
			{
				if (base.DataGridView != null && base.Index == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidPropertySetOnSharedRow", new object[] { "DividerHeight" }));
				}
				base.DividerThickness = value;
			}
		}

		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x060036C0 RID: 14016 RVA: 0x000C432D File Offset: 0x000C332D
		// (set) Token: 0x060036C1 RID: 14017 RVA: 0x000C433B File Offset: 0x000C333B
		[SRDescription("DataGridView_RowErrorTextDescr")]
		[NotifyParentProperty(true)]
		[DefaultValue("")]
		[SRCategory("CatAppearance")]
		public string ErrorText
		{
			get
			{
				return this.GetErrorText(base.Index);
			}
			set
			{
				this.ErrorTextInternal = value;
			}
		}

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x060036C2 RID: 14018 RVA: 0x000C4344 File Offset: 0x000C3344
		// (set) Token: 0x060036C3 RID: 14019 RVA: 0x000C4374 File Offset: 0x000C3374
		private string ErrorTextInternal
		{
			get
			{
				object @object = base.Properties.GetObject(DataGridViewRow.PropRowErrorText);
				if (@object != null)
				{
					return (string)@object;
				}
				return string.Empty;
			}
			set
			{
				string errorTextInternal = this.ErrorTextInternal;
				if (!string.IsNullOrEmpty(value) || base.Properties.ContainsObject(DataGridViewRow.PropRowErrorText))
				{
					base.Properties.SetObject(DataGridViewRow.PropRowErrorText, value);
				}
				if (base.DataGridView != null && !errorTextInternal.Equals(this.ErrorTextInternal))
				{
					base.DataGridView.OnRowErrorTextChanged(this);
				}
			}
		}

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x060036C4 RID: 14020 RVA: 0x000C43D8 File Offset: 0x000C33D8
		// (set) Token: 0x060036C5 RID: 14021 RVA: 0x000C4424 File Offset: 0x000C3424
		[Browsable(false)]
		public override bool Frozen
		{
			get
			{
				if (base.DataGridView != null && base.Index == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidPropertyGetOnSharedRow", new object[] { "Frozen" }));
				}
				return this.GetFrozen(base.Index);
			}
			set
			{
				if (base.DataGridView != null && base.Index == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidPropertySetOnSharedRow", new object[] { "Frozen" }));
				}
				base.Frozen = value;
			}
		}

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x060036C6 RID: 14022 RVA: 0x000C4469 File Offset: 0x000C3469
		internal bool HasErrorText
		{
			get
			{
				return base.Properties.ContainsObject(DataGridViewRow.PropRowErrorText) && base.Properties.GetObject(DataGridViewRow.PropRowErrorText) != null;
			}
		}

		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x060036C7 RID: 14023 RVA: 0x000C4495 File Offset: 0x000C3495
		// (set) Token: 0x060036C8 RID: 14024 RVA: 0x000C44A2 File Offset: 0x000C34A2
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public DataGridViewRowHeaderCell HeaderCell
		{
			get
			{
				return (DataGridViewRowHeaderCell)base.HeaderCellCore;
			}
			set
			{
				base.HeaderCellCore = value;
			}
		}

		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x060036C9 RID: 14025 RVA: 0x000C44AB File Offset: 0x000C34AB
		// (set) Token: 0x060036CA RID: 14026 RVA: 0x000C44B4 File Offset: 0x000C34B4
		[SRCategory("CatAppearance")]
		[NotifyParentProperty(true)]
		[SRDescription("DataGridView_RowHeightDescr")]
		[DefaultValue(22)]
		public int Height
		{
			get
			{
				return base.Thickness;
			}
			set
			{
				if (base.DataGridView != null && base.Index == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidPropertySetOnSharedRow", new object[] { "Height" }));
				}
				base.Thickness = value;
			}
		}

		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x060036CB RID: 14027 RVA: 0x000C44FC File Offset: 0x000C34FC
		public override DataGridViewCellStyle InheritedStyle
		{
			get
			{
				if (base.Index == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidPropertyGetOnSharedRow", new object[] { "InheritedStyle" }));
				}
				DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
				this.BuildInheritedRowStyle(base.Index, dataGridViewCellStyle);
				return dataGridViewCellStyle;
			}
		}

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x060036CC RID: 14028 RVA: 0x000C4546 File Offset: 0x000C3546
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool IsNewRow
		{
			get
			{
				return base.DataGridView != null && base.DataGridView.NewRowIndex == base.Index;
			}
		}

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x060036CD RID: 14029 RVA: 0x000C4565 File Offset: 0x000C3565
		// (set) Token: 0x060036CE RID: 14030 RVA: 0x000C4570 File Offset: 0x000C3570
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int MinimumHeight
		{
			get
			{
				return base.MinimumThickness;
			}
			set
			{
				if (base.DataGridView != null && base.Index == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidPropertySetOnSharedRow", new object[] { "MinimumHeight" }));
				}
				base.MinimumThickness = value;
			}
		}

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x060036CF RID: 14031 RVA: 0x000C45B8 File Offset: 0x000C35B8
		// (set) Token: 0x060036D0 RID: 14032 RVA: 0x000C4602 File Offset: 0x000C3602
		[DefaultValue(false)]
		[Browsable(true)]
		[NotifyParentProperty(true)]
		[SRCategory("CatBehavior")]
		[SRDescription("DataGridView_RowReadOnlyDescr")]
		public override bool ReadOnly
		{
			get
			{
				if (base.DataGridView != null && base.Index == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidPropertyGetOnSharedRow", new object[] { "ReadOnly" }));
				}
				return this.GetReadOnly(base.Index);
			}
			set
			{
				base.ReadOnly = value;
			}
		}

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x060036D1 RID: 14033 RVA: 0x000C460C File Offset: 0x000C360C
		// (set) Token: 0x060036D2 RID: 14034 RVA: 0x000C4656 File Offset: 0x000C3656
		[NotifyParentProperty(true)]
		[SRDescription("DataGridView_RowResizableDescr")]
		[SRCategory("CatBehavior")]
		public override DataGridViewTriState Resizable
		{
			get
			{
				if (base.DataGridView != null && base.Index == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidPropertyGetOnSharedRow", new object[] { "Resizable" }));
				}
				return this.GetResizable(base.Index);
			}
			set
			{
				base.Resizable = value;
			}
		}

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x060036D3 RID: 14035 RVA: 0x000C4660 File Offset: 0x000C3660
		// (set) Token: 0x060036D4 RID: 14036 RVA: 0x000C46AA File Offset: 0x000C36AA
		public override bool Selected
		{
			get
			{
				if (base.DataGridView != null && base.Index == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidPropertyGetOnSharedRow", new object[] { "Selected" }));
				}
				return this.GetSelected(base.Index);
			}
			set
			{
				base.Selected = value;
			}
		}

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x060036D5 RID: 14037 RVA: 0x000C46B4 File Offset: 0x000C36B4
		public override DataGridViewElementStates State
		{
			get
			{
				if (base.DataGridView != null && base.Index == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidPropertyGetOnSharedRow", new object[] { "State" }));
				}
				return this.GetState(base.Index);
			}
		}

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x060036D6 RID: 14038 RVA: 0x000C4700 File Offset: 0x000C3700
		// (set) Token: 0x060036D7 RID: 14039 RVA: 0x000C474C File Offset: 0x000C374C
		[Browsable(false)]
		public override bool Visible
		{
			get
			{
				if (base.DataGridView != null && base.Index == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidPropertyGetOnSharedRow", new object[] { "Visible" }));
				}
				return this.GetVisible(base.Index);
			}
			set
			{
				if (base.DataGridView != null && base.Index == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidPropertySetOnSharedRow", new object[] { "Visible" }));
				}
				base.Visible = value;
			}
		}

		// Token: 0x060036D8 RID: 14040 RVA: 0x000C4794 File Offset: 0x000C3794
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual DataGridViewAdvancedBorderStyle AdjustRowHeaderBorderStyle(DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyleInput, DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStylePlaceholder, bool singleVerticalBorderAdded, bool singleHorizontalBorderAdded, bool isFirstDisplayedRow, bool isLastVisibleRow)
		{
			if (base.DataGridView != null && base.DataGridView.ApplyVisualStylesToHeaderCells)
			{
				switch (dataGridViewAdvancedBorderStyleInput.All)
				{
				case DataGridViewAdvancedCellBorderStyle.Single:
					if (isFirstDisplayedRow && !base.DataGridView.ColumnHeadersVisible)
					{
						dataGridViewAdvancedBorderStylePlaceholder.TopInternal = DataGridViewAdvancedCellBorderStyle.Single;
					}
					else
					{
						dataGridViewAdvancedBorderStylePlaceholder.TopInternal = DataGridViewAdvancedCellBorderStyle.None;
					}
					dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = DataGridViewAdvancedCellBorderStyle.Single;
					dataGridViewAdvancedBorderStylePlaceholder.RightInternal = DataGridViewAdvancedCellBorderStyle.Single;
					dataGridViewAdvancedBorderStylePlaceholder.BottomInternal = DataGridViewAdvancedCellBorderStyle.None;
					return dataGridViewAdvancedBorderStylePlaceholder;
				case DataGridViewAdvancedCellBorderStyle.Inset:
					if (isFirstDisplayedRow && !base.DataGridView.ColumnHeadersVisible)
					{
						dataGridViewAdvancedBorderStylePlaceholder.TopInternal = DataGridViewAdvancedCellBorderStyle.Inset;
					}
					else
					{
						dataGridViewAdvancedBorderStylePlaceholder.TopInternal = DataGridViewAdvancedCellBorderStyle.None;
					}
					dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = DataGridViewAdvancedCellBorderStyle.Inset;
					dataGridViewAdvancedBorderStylePlaceholder.RightInternal = DataGridViewAdvancedCellBorderStyle.Inset;
					dataGridViewAdvancedBorderStylePlaceholder.BottomInternal = DataGridViewAdvancedCellBorderStyle.None;
					return dataGridViewAdvancedBorderStylePlaceholder;
				case DataGridViewAdvancedCellBorderStyle.InsetDouble:
					if (isFirstDisplayedRow && !base.DataGridView.ColumnHeadersVisible)
					{
						dataGridViewAdvancedBorderStylePlaceholder.TopInternal = DataGridViewAdvancedCellBorderStyle.InsetDouble;
					}
					else
					{
						dataGridViewAdvancedBorderStylePlaceholder.TopInternal = DataGridViewAdvancedCellBorderStyle.None;
					}
					if (base.DataGridView != null && base.DataGridView.RightToLeftInternal)
					{
						dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = DataGridViewAdvancedCellBorderStyle.Inset;
					}
					else
					{
						dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = DataGridViewAdvancedCellBorderStyle.InsetDouble;
					}
					dataGridViewAdvancedBorderStylePlaceholder.RightInternal = DataGridViewAdvancedCellBorderStyle.Inset;
					dataGridViewAdvancedBorderStylePlaceholder.BottomInternal = DataGridViewAdvancedCellBorderStyle.None;
					return dataGridViewAdvancedBorderStylePlaceholder;
				case DataGridViewAdvancedCellBorderStyle.Outset:
					if (isFirstDisplayedRow && !base.DataGridView.ColumnHeadersVisible)
					{
						dataGridViewAdvancedBorderStylePlaceholder.TopInternal = DataGridViewAdvancedCellBorderStyle.Outset;
					}
					else
					{
						dataGridViewAdvancedBorderStylePlaceholder.TopInternal = DataGridViewAdvancedCellBorderStyle.None;
					}
					dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = DataGridViewAdvancedCellBorderStyle.Outset;
					dataGridViewAdvancedBorderStylePlaceholder.RightInternal = DataGridViewAdvancedCellBorderStyle.Outset;
					dataGridViewAdvancedBorderStylePlaceholder.BottomInternal = DataGridViewAdvancedCellBorderStyle.None;
					return dataGridViewAdvancedBorderStylePlaceholder;
				case DataGridViewAdvancedCellBorderStyle.OutsetDouble:
					if (isFirstDisplayedRow && !base.DataGridView.ColumnHeadersVisible)
					{
						dataGridViewAdvancedBorderStylePlaceholder.TopInternal = DataGridViewAdvancedCellBorderStyle.OutsetDouble;
					}
					else
					{
						dataGridViewAdvancedBorderStylePlaceholder.TopInternal = DataGridViewAdvancedCellBorderStyle.None;
					}
					if (base.DataGridView != null && base.DataGridView.RightToLeftInternal)
					{
						dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = DataGridViewAdvancedCellBorderStyle.Outset;
					}
					else
					{
						dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = DataGridViewAdvancedCellBorderStyle.OutsetDouble;
					}
					dataGridViewAdvancedBorderStylePlaceholder.RightInternal = DataGridViewAdvancedCellBorderStyle.Outset;
					dataGridViewAdvancedBorderStylePlaceholder.BottomInternal = DataGridViewAdvancedCellBorderStyle.None;
					return dataGridViewAdvancedBorderStylePlaceholder;
				case DataGridViewAdvancedCellBorderStyle.OutsetPartial:
					if (isFirstDisplayedRow && !base.DataGridView.ColumnHeadersVisible)
					{
						dataGridViewAdvancedBorderStylePlaceholder.TopInternal = DataGridViewAdvancedCellBorderStyle.OutsetDouble;
					}
					else
					{
						dataGridViewAdvancedBorderStylePlaceholder.TopInternal = DataGridViewAdvancedCellBorderStyle.None;
					}
					if (base.DataGridView != null && base.DataGridView.RightToLeftInternal)
					{
						dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = DataGridViewAdvancedCellBorderStyle.Outset;
					}
					else
					{
						dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = DataGridViewAdvancedCellBorderStyle.OutsetDouble;
					}
					dataGridViewAdvancedBorderStylePlaceholder.RightInternal = DataGridViewAdvancedCellBorderStyle.Outset;
					dataGridViewAdvancedBorderStylePlaceholder.BottomInternal = DataGridViewAdvancedCellBorderStyle.None;
					return dataGridViewAdvancedBorderStylePlaceholder;
				}
			}
			else
			{
				switch (dataGridViewAdvancedBorderStyleInput.All)
				{
				case DataGridViewAdvancedCellBorderStyle.Single:
					if (!isFirstDisplayedRow || base.DataGridView.ColumnHeadersVisible)
					{
						dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = DataGridViewAdvancedCellBorderStyle.Single;
						dataGridViewAdvancedBorderStylePlaceholder.TopInternal = DataGridViewAdvancedCellBorderStyle.None;
						dataGridViewAdvancedBorderStylePlaceholder.BottomInternal = DataGridViewAdvancedCellBorderStyle.Single;
						dataGridViewAdvancedBorderStylePlaceholder.RightInternal = DataGridViewAdvancedCellBorderStyle.Single;
						return dataGridViewAdvancedBorderStylePlaceholder;
					}
					break;
				case DataGridViewAdvancedCellBorderStyle.Inset:
					if (isFirstDisplayedRow && singleHorizontalBorderAdded)
					{
						dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = DataGridViewAdvancedCellBorderStyle.Inset;
						dataGridViewAdvancedBorderStylePlaceholder.TopInternal = DataGridViewAdvancedCellBorderStyle.InsetDouble;
						dataGridViewAdvancedBorderStylePlaceholder.BottomInternal = DataGridViewAdvancedCellBorderStyle.Inset;
						dataGridViewAdvancedBorderStylePlaceholder.RightInternal = DataGridViewAdvancedCellBorderStyle.Inset;
						return dataGridViewAdvancedBorderStylePlaceholder;
					}
					break;
				case DataGridViewAdvancedCellBorderStyle.InsetDouble:
					if (base.DataGridView != null && base.DataGridView.RightToLeftInternal)
					{
						dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = DataGridViewAdvancedCellBorderStyle.Inset;
						dataGridViewAdvancedBorderStylePlaceholder.RightInternal = DataGridViewAdvancedCellBorderStyle.InsetDouble;
					}
					else
					{
						dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = DataGridViewAdvancedCellBorderStyle.InsetDouble;
						dataGridViewAdvancedBorderStylePlaceholder.RightInternal = DataGridViewAdvancedCellBorderStyle.Inset;
					}
					if (isFirstDisplayedRow)
					{
						dataGridViewAdvancedBorderStylePlaceholder.TopInternal = (base.DataGridView.ColumnHeadersVisible ? DataGridViewAdvancedCellBorderStyle.Inset : DataGridViewAdvancedCellBorderStyle.InsetDouble);
					}
					else
					{
						dataGridViewAdvancedBorderStylePlaceholder.TopInternal = DataGridViewAdvancedCellBorderStyle.Inset;
					}
					dataGridViewAdvancedBorderStylePlaceholder.BottomInternal = DataGridViewAdvancedCellBorderStyle.Inset;
					return dataGridViewAdvancedBorderStylePlaceholder;
				case DataGridViewAdvancedCellBorderStyle.Outset:
					if (isFirstDisplayedRow && singleHorizontalBorderAdded)
					{
						dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = DataGridViewAdvancedCellBorderStyle.Outset;
						dataGridViewAdvancedBorderStylePlaceholder.TopInternal = DataGridViewAdvancedCellBorderStyle.OutsetDouble;
						dataGridViewAdvancedBorderStylePlaceholder.BottomInternal = DataGridViewAdvancedCellBorderStyle.Outset;
						dataGridViewAdvancedBorderStylePlaceholder.RightInternal = DataGridViewAdvancedCellBorderStyle.Outset;
						return dataGridViewAdvancedBorderStylePlaceholder;
					}
					break;
				case DataGridViewAdvancedCellBorderStyle.OutsetDouble:
					if (base.DataGridView != null && base.DataGridView.RightToLeftInternal)
					{
						dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = DataGridViewAdvancedCellBorderStyle.Outset;
						dataGridViewAdvancedBorderStylePlaceholder.RightInternal = DataGridViewAdvancedCellBorderStyle.OutsetDouble;
					}
					else
					{
						dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = DataGridViewAdvancedCellBorderStyle.OutsetDouble;
						dataGridViewAdvancedBorderStylePlaceholder.RightInternal = DataGridViewAdvancedCellBorderStyle.Outset;
					}
					if (isFirstDisplayedRow)
					{
						dataGridViewAdvancedBorderStylePlaceholder.TopInternal = (base.DataGridView.ColumnHeadersVisible ? DataGridViewAdvancedCellBorderStyle.Outset : DataGridViewAdvancedCellBorderStyle.OutsetDouble);
					}
					else
					{
						dataGridViewAdvancedBorderStylePlaceholder.TopInternal = DataGridViewAdvancedCellBorderStyle.Outset;
					}
					dataGridViewAdvancedBorderStylePlaceholder.BottomInternal = DataGridViewAdvancedCellBorderStyle.Outset;
					return dataGridViewAdvancedBorderStylePlaceholder;
				case DataGridViewAdvancedCellBorderStyle.OutsetPartial:
					if (base.DataGridView != null && base.DataGridView.RightToLeftInternal)
					{
						dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = DataGridViewAdvancedCellBorderStyle.Outset;
						dataGridViewAdvancedBorderStylePlaceholder.RightInternal = DataGridViewAdvancedCellBorderStyle.OutsetDouble;
					}
					else
					{
						dataGridViewAdvancedBorderStylePlaceholder.LeftInternal = DataGridViewAdvancedCellBorderStyle.OutsetDouble;
						dataGridViewAdvancedBorderStylePlaceholder.RightInternal = DataGridViewAdvancedCellBorderStyle.Outset;
					}
					if (isFirstDisplayedRow)
					{
						dataGridViewAdvancedBorderStylePlaceholder.TopInternal = (base.DataGridView.ColumnHeadersVisible ? DataGridViewAdvancedCellBorderStyle.Outset : DataGridViewAdvancedCellBorderStyle.OutsetDouble);
					}
					else
					{
						dataGridViewAdvancedBorderStylePlaceholder.TopInternal = DataGridViewAdvancedCellBorderStyle.OutsetPartial;
					}
					dataGridViewAdvancedBorderStylePlaceholder.BottomInternal = (isLastVisibleRow ? DataGridViewAdvancedCellBorderStyle.Outset : DataGridViewAdvancedCellBorderStyle.OutsetPartial);
					return dataGridViewAdvancedBorderStylePlaceholder;
				}
			}
			return dataGridViewAdvancedBorderStyleInput;
		}

		// Token: 0x060036D9 RID: 14041 RVA: 0x000C4B70 File Offset: 0x000C3B70
		private void BuildInheritedRowHeaderCellStyle(DataGridViewCellStyle inheritedCellStyle)
		{
			DataGridViewCellStyle dataGridViewCellStyle = null;
			if (this.HeaderCell.HasStyle)
			{
				dataGridViewCellStyle = this.HeaderCell.Style;
			}
			DataGridViewCellStyle rowHeadersDefaultCellStyle = base.DataGridView.RowHeadersDefaultCellStyle;
			DataGridViewCellStyle defaultCellStyle = base.DataGridView.DefaultCellStyle;
			if (dataGridViewCellStyle != null && !dataGridViewCellStyle.BackColor.IsEmpty)
			{
				inheritedCellStyle.BackColor = dataGridViewCellStyle.BackColor;
			}
			else if (!rowHeadersDefaultCellStyle.BackColor.IsEmpty)
			{
				inheritedCellStyle.BackColor = rowHeadersDefaultCellStyle.BackColor;
			}
			else
			{
				inheritedCellStyle.BackColor = defaultCellStyle.BackColor;
			}
			if (dataGridViewCellStyle != null && !dataGridViewCellStyle.ForeColor.IsEmpty)
			{
				inheritedCellStyle.ForeColor = dataGridViewCellStyle.ForeColor;
			}
			else if (!rowHeadersDefaultCellStyle.ForeColor.IsEmpty)
			{
				inheritedCellStyle.ForeColor = rowHeadersDefaultCellStyle.ForeColor;
			}
			else
			{
				inheritedCellStyle.ForeColor = defaultCellStyle.ForeColor;
			}
			if (dataGridViewCellStyle != null && !dataGridViewCellStyle.SelectionBackColor.IsEmpty)
			{
				inheritedCellStyle.SelectionBackColor = dataGridViewCellStyle.SelectionBackColor;
			}
			else if (!rowHeadersDefaultCellStyle.SelectionBackColor.IsEmpty)
			{
				inheritedCellStyle.SelectionBackColor = rowHeadersDefaultCellStyle.SelectionBackColor;
			}
			else
			{
				inheritedCellStyle.SelectionBackColor = defaultCellStyle.SelectionBackColor;
			}
			if (dataGridViewCellStyle != null && !dataGridViewCellStyle.SelectionForeColor.IsEmpty)
			{
				inheritedCellStyle.SelectionForeColor = dataGridViewCellStyle.SelectionForeColor;
			}
			else if (!rowHeadersDefaultCellStyle.SelectionForeColor.IsEmpty)
			{
				inheritedCellStyle.SelectionForeColor = rowHeadersDefaultCellStyle.SelectionForeColor;
			}
			else
			{
				inheritedCellStyle.SelectionForeColor = defaultCellStyle.SelectionForeColor;
			}
			if (dataGridViewCellStyle != null && dataGridViewCellStyle.Font != null)
			{
				inheritedCellStyle.Font = dataGridViewCellStyle.Font;
			}
			else if (rowHeadersDefaultCellStyle.Font != null)
			{
				inheritedCellStyle.Font = rowHeadersDefaultCellStyle.Font;
			}
			else
			{
				inheritedCellStyle.Font = defaultCellStyle.Font;
			}
			if (dataGridViewCellStyle != null && !dataGridViewCellStyle.IsNullValueDefault)
			{
				inheritedCellStyle.NullValue = dataGridViewCellStyle.NullValue;
			}
			else if (!rowHeadersDefaultCellStyle.IsNullValueDefault)
			{
				inheritedCellStyle.NullValue = rowHeadersDefaultCellStyle.NullValue;
			}
			else
			{
				inheritedCellStyle.NullValue = defaultCellStyle.NullValue;
			}
			if (dataGridViewCellStyle != null && !dataGridViewCellStyle.IsDataSourceNullValueDefault)
			{
				inheritedCellStyle.DataSourceNullValue = dataGridViewCellStyle.DataSourceNullValue;
			}
			else if (!rowHeadersDefaultCellStyle.IsDataSourceNullValueDefault)
			{
				inheritedCellStyle.DataSourceNullValue = rowHeadersDefaultCellStyle.DataSourceNullValue;
			}
			else
			{
				inheritedCellStyle.DataSourceNullValue = defaultCellStyle.DataSourceNullValue;
			}
			if (dataGridViewCellStyle != null && dataGridViewCellStyle.Format.Length != 0)
			{
				inheritedCellStyle.Format = dataGridViewCellStyle.Format;
			}
			else if (rowHeadersDefaultCellStyle.Format.Length != 0)
			{
				inheritedCellStyle.Format = rowHeadersDefaultCellStyle.Format;
			}
			else
			{
				inheritedCellStyle.Format = defaultCellStyle.Format;
			}
			if (dataGridViewCellStyle != null && !dataGridViewCellStyle.IsFormatProviderDefault)
			{
				inheritedCellStyle.FormatProvider = dataGridViewCellStyle.FormatProvider;
			}
			else if (!rowHeadersDefaultCellStyle.IsFormatProviderDefault)
			{
				inheritedCellStyle.FormatProvider = rowHeadersDefaultCellStyle.FormatProvider;
			}
			else
			{
				inheritedCellStyle.FormatProvider = defaultCellStyle.FormatProvider;
			}
			if (dataGridViewCellStyle != null && dataGridViewCellStyle.Alignment != DataGridViewContentAlignment.NotSet)
			{
				inheritedCellStyle.AlignmentInternal = dataGridViewCellStyle.Alignment;
			}
			else if (rowHeadersDefaultCellStyle != null && rowHeadersDefaultCellStyle.Alignment != DataGridViewContentAlignment.NotSet)
			{
				inheritedCellStyle.AlignmentInternal = rowHeadersDefaultCellStyle.Alignment;
			}
			else
			{
				inheritedCellStyle.AlignmentInternal = defaultCellStyle.Alignment;
			}
			if (dataGridViewCellStyle != null && dataGridViewCellStyle.WrapMode != DataGridViewTriState.NotSet)
			{
				inheritedCellStyle.WrapModeInternal = dataGridViewCellStyle.WrapMode;
			}
			else if (rowHeadersDefaultCellStyle != null && rowHeadersDefaultCellStyle.WrapMode != DataGridViewTriState.NotSet)
			{
				inheritedCellStyle.WrapModeInternal = rowHeadersDefaultCellStyle.WrapMode;
			}
			else
			{
				inheritedCellStyle.WrapModeInternal = defaultCellStyle.WrapMode;
			}
			if (dataGridViewCellStyle != null && dataGridViewCellStyle.Tag != null)
			{
				inheritedCellStyle.Tag = dataGridViewCellStyle.Tag;
			}
			else if (rowHeadersDefaultCellStyle.Tag != null)
			{
				inheritedCellStyle.Tag = rowHeadersDefaultCellStyle.Tag;
			}
			else
			{
				inheritedCellStyle.Tag = defaultCellStyle.Tag;
			}
			if (dataGridViewCellStyle != null && dataGridViewCellStyle.Padding != Padding.Empty)
			{
				inheritedCellStyle.PaddingInternal = dataGridViewCellStyle.Padding;
				return;
			}
			if (rowHeadersDefaultCellStyle.Padding != Padding.Empty)
			{
				inheritedCellStyle.PaddingInternal = rowHeadersDefaultCellStyle.Padding;
				return;
			}
			inheritedCellStyle.PaddingInternal = defaultCellStyle.Padding;
		}

		// Token: 0x060036DA RID: 14042 RVA: 0x000C4F18 File Offset: 0x000C3F18
		private void BuildInheritedRowStyle(int rowIndex, DataGridViewCellStyle inheritedRowStyle)
		{
			DataGridViewCellStyle dataGridViewCellStyle = null;
			if (base.HasDefaultCellStyle)
			{
				dataGridViewCellStyle = this.DefaultCellStyle;
			}
			DataGridViewCellStyle defaultCellStyle = base.DataGridView.DefaultCellStyle;
			DataGridViewCellStyle rowsDefaultCellStyle = base.DataGridView.RowsDefaultCellStyle;
			DataGridViewCellStyle alternatingRowsDefaultCellStyle = base.DataGridView.AlternatingRowsDefaultCellStyle;
			if (dataGridViewCellStyle != null && !dataGridViewCellStyle.BackColor.IsEmpty)
			{
				inheritedRowStyle.BackColor = dataGridViewCellStyle.BackColor;
			}
			else if (!rowsDefaultCellStyle.BackColor.IsEmpty && (rowIndex % 2 == 0 || alternatingRowsDefaultCellStyle.BackColor.IsEmpty))
			{
				inheritedRowStyle.BackColor = rowsDefaultCellStyle.BackColor;
			}
			else if (rowIndex % 2 == 1 && !alternatingRowsDefaultCellStyle.BackColor.IsEmpty)
			{
				inheritedRowStyle.BackColor = alternatingRowsDefaultCellStyle.BackColor;
			}
			else
			{
				inheritedRowStyle.BackColor = defaultCellStyle.BackColor;
			}
			if (dataGridViewCellStyle != null && !dataGridViewCellStyle.ForeColor.IsEmpty)
			{
				inheritedRowStyle.ForeColor = dataGridViewCellStyle.ForeColor;
			}
			else if (!rowsDefaultCellStyle.ForeColor.IsEmpty && (rowIndex % 2 == 0 || alternatingRowsDefaultCellStyle.ForeColor.IsEmpty))
			{
				inheritedRowStyle.ForeColor = rowsDefaultCellStyle.ForeColor;
			}
			else if (rowIndex % 2 == 1 && !alternatingRowsDefaultCellStyle.ForeColor.IsEmpty)
			{
				inheritedRowStyle.ForeColor = alternatingRowsDefaultCellStyle.ForeColor;
			}
			else
			{
				inheritedRowStyle.ForeColor = defaultCellStyle.ForeColor;
			}
			if (dataGridViewCellStyle != null && !dataGridViewCellStyle.SelectionBackColor.IsEmpty)
			{
				inheritedRowStyle.SelectionBackColor = dataGridViewCellStyle.SelectionBackColor;
			}
			else if (!rowsDefaultCellStyle.SelectionBackColor.IsEmpty && (rowIndex % 2 == 0 || alternatingRowsDefaultCellStyle.SelectionBackColor.IsEmpty))
			{
				inheritedRowStyle.SelectionBackColor = rowsDefaultCellStyle.SelectionBackColor;
			}
			else if (rowIndex % 2 == 1 && !alternatingRowsDefaultCellStyle.SelectionBackColor.IsEmpty)
			{
				inheritedRowStyle.SelectionBackColor = alternatingRowsDefaultCellStyle.SelectionBackColor;
			}
			else
			{
				inheritedRowStyle.SelectionBackColor = defaultCellStyle.SelectionBackColor;
			}
			if (dataGridViewCellStyle != null && !dataGridViewCellStyle.SelectionForeColor.IsEmpty)
			{
				inheritedRowStyle.SelectionForeColor = dataGridViewCellStyle.SelectionForeColor;
			}
			else if (!rowsDefaultCellStyle.SelectionForeColor.IsEmpty && (rowIndex % 2 == 0 || alternatingRowsDefaultCellStyle.SelectionForeColor.IsEmpty))
			{
				inheritedRowStyle.SelectionForeColor = rowsDefaultCellStyle.SelectionForeColor;
			}
			else if (rowIndex % 2 == 1 && !alternatingRowsDefaultCellStyle.SelectionForeColor.IsEmpty)
			{
				inheritedRowStyle.SelectionForeColor = alternatingRowsDefaultCellStyle.SelectionForeColor;
			}
			else
			{
				inheritedRowStyle.SelectionForeColor = defaultCellStyle.SelectionForeColor;
			}
			if (dataGridViewCellStyle != null && dataGridViewCellStyle.Font != null)
			{
				inheritedRowStyle.Font = dataGridViewCellStyle.Font;
			}
			else if (rowsDefaultCellStyle.Font != null && (rowIndex % 2 == 0 || alternatingRowsDefaultCellStyle.Font == null))
			{
				inheritedRowStyle.Font = rowsDefaultCellStyle.Font;
			}
			else if (rowIndex % 2 == 1 && alternatingRowsDefaultCellStyle.Font != null)
			{
				inheritedRowStyle.Font = alternatingRowsDefaultCellStyle.Font;
			}
			else
			{
				inheritedRowStyle.Font = defaultCellStyle.Font;
			}
			if (dataGridViewCellStyle != null && !dataGridViewCellStyle.IsNullValueDefault)
			{
				inheritedRowStyle.NullValue = dataGridViewCellStyle.NullValue;
			}
			else if (!rowsDefaultCellStyle.IsNullValueDefault && (rowIndex % 2 == 0 || alternatingRowsDefaultCellStyle.IsNullValueDefault))
			{
				inheritedRowStyle.NullValue = rowsDefaultCellStyle.NullValue;
			}
			else if (rowIndex % 2 == 1 && !alternatingRowsDefaultCellStyle.IsNullValueDefault)
			{
				inheritedRowStyle.NullValue = alternatingRowsDefaultCellStyle.NullValue;
			}
			else
			{
				inheritedRowStyle.NullValue = defaultCellStyle.NullValue;
			}
			if (dataGridViewCellStyle != null && !dataGridViewCellStyle.IsDataSourceNullValueDefault)
			{
				inheritedRowStyle.DataSourceNullValue = dataGridViewCellStyle.DataSourceNullValue;
			}
			else if (!rowsDefaultCellStyle.IsDataSourceNullValueDefault && (rowIndex % 2 == 0 || alternatingRowsDefaultCellStyle.IsDataSourceNullValueDefault))
			{
				inheritedRowStyle.DataSourceNullValue = rowsDefaultCellStyle.DataSourceNullValue;
			}
			else if (rowIndex % 2 == 1 && !alternatingRowsDefaultCellStyle.IsDataSourceNullValueDefault)
			{
				inheritedRowStyle.DataSourceNullValue = alternatingRowsDefaultCellStyle.DataSourceNullValue;
			}
			else
			{
				inheritedRowStyle.DataSourceNullValue = defaultCellStyle.DataSourceNullValue;
			}
			if (dataGridViewCellStyle != null && dataGridViewCellStyle.Format.Length != 0)
			{
				inheritedRowStyle.Format = dataGridViewCellStyle.Format;
			}
			else if (rowsDefaultCellStyle.Format.Length != 0 && (rowIndex % 2 == 0 || alternatingRowsDefaultCellStyle.Format.Length == 0))
			{
				inheritedRowStyle.Format = rowsDefaultCellStyle.Format;
			}
			else if (rowIndex % 2 == 1 && alternatingRowsDefaultCellStyle.Format.Length != 0)
			{
				inheritedRowStyle.Format = alternatingRowsDefaultCellStyle.Format;
			}
			else
			{
				inheritedRowStyle.Format = defaultCellStyle.Format;
			}
			if (dataGridViewCellStyle != null && !dataGridViewCellStyle.IsFormatProviderDefault)
			{
				inheritedRowStyle.FormatProvider = dataGridViewCellStyle.FormatProvider;
			}
			else if (!rowsDefaultCellStyle.IsFormatProviderDefault && (rowIndex % 2 == 0 || alternatingRowsDefaultCellStyle.IsFormatProviderDefault))
			{
				inheritedRowStyle.FormatProvider = rowsDefaultCellStyle.FormatProvider;
			}
			else if (rowIndex % 2 == 1 && !alternatingRowsDefaultCellStyle.IsFormatProviderDefault)
			{
				inheritedRowStyle.FormatProvider = alternatingRowsDefaultCellStyle.FormatProvider;
			}
			else
			{
				inheritedRowStyle.FormatProvider = defaultCellStyle.FormatProvider;
			}
			if (dataGridViewCellStyle != null && dataGridViewCellStyle.Alignment != DataGridViewContentAlignment.NotSet)
			{
				inheritedRowStyle.AlignmentInternal = dataGridViewCellStyle.Alignment;
			}
			else if (rowsDefaultCellStyle.Alignment != DataGridViewContentAlignment.NotSet && (rowIndex % 2 == 0 || alternatingRowsDefaultCellStyle.Alignment == DataGridViewContentAlignment.NotSet))
			{
				inheritedRowStyle.AlignmentInternal = rowsDefaultCellStyle.Alignment;
			}
			else if (rowIndex % 2 == 1 && alternatingRowsDefaultCellStyle.Alignment != DataGridViewContentAlignment.NotSet)
			{
				inheritedRowStyle.AlignmentInternal = alternatingRowsDefaultCellStyle.Alignment;
			}
			else
			{
				inheritedRowStyle.AlignmentInternal = defaultCellStyle.Alignment;
			}
			if (dataGridViewCellStyle != null && dataGridViewCellStyle.WrapMode != DataGridViewTriState.NotSet)
			{
				inheritedRowStyle.WrapModeInternal = dataGridViewCellStyle.WrapMode;
			}
			else if (rowsDefaultCellStyle.WrapMode != DataGridViewTriState.NotSet && (rowIndex % 2 == 0 || alternatingRowsDefaultCellStyle.WrapMode == DataGridViewTriState.NotSet))
			{
				inheritedRowStyle.WrapModeInternal = rowsDefaultCellStyle.WrapMode;
			}
			else if (rowIndex % 2 == 1 && alternatingRowsDefaultCellStyle.WrapMode != DataGridViewTriState.NotSet)
			{
				inheritedRowStyle.WrapModeInternal = alternatingRowsDefaultCellStyle.WrapMode;
			}
			else
			{
				inheritedRowStyle.WrapModeInternal = defaultCellStyle.WrapMode;
			}
			if (dataGridViewCellStyle != null && dataGridViewCellStyle.Tag != null)
			{
				inheritedRowStyle.Tag = dataGridViewCellStyle.Tag;
			}
			else if (rowsDefaultCellStyle.Tag != null && (rowIndex % 2 == 0 || alternatingRowsDefaultCellStyle.Tag == null))
			{
				inheritedRowStyle.Tag = rowsDefaultCellStyle.Tag;
			}
			else if (rowIndex % 2 == 1 && alternatingRowsDefaultCellStyle.Tag != null)
			{
				inheritedRowStyle.Tag = alternatingRowsDefaultCellStyle.Tag;
			}
			else
			{
				inheritedRowStyle.Tag = defaultCellStyle.Tag;
			}
			if (dataGridViewCellStyle != null && dataGridViewCellStyle.Padding != Padding.Empty)
			{
				inheritedRowStyle.PaddingInternal = dataGridViewCellStyle.Padding;
				return;
			}
			if (rowsDefaultCellStyle.Padding != Padding.Empty && (rowIndex % 2 == 0 || alternatingRowsDefaultCellStyle.Padding == Padding.Empty))
			{
				inheritedRowStyle.PaddingInternal = rowsDefaultCellStyle.Padding;
				return;
			}
			if (rowIndex % 2 == 1 && alternatingRowsDefaultCellStyle.Padding != Padding.Empty)
			{
				inheritedRowStyle.PaddingInternal = alternatingRowsDefaultCellStyle.Padding;
				return;
			}
			inheritedRowStyle.PaddingInternal = defaultCellStyle.Padding;
		}

		// Token: 0x060036DB RID: 14043 RVA: 0x000C5538 File Offset: 0x000C4538
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewRow dataGridViewRow;
			if (type == DataGridViewRow.rowType)
			{
				dataGridViewRow = new DataGridViewRow();
			}
			else
			{
				dataGridViewRow = (DataGridViewRow)Activator.CreateInstance(type);
			}
			if (dataGridViewRow != null)
			{
				base.CloneInternal(dataGridViewRow);
				if (this.HasErrorText)
				{
					dataGridViewRow.ErrorText = this.ErrorTextInternal;
				}
				if (base.HasHeaderCell)
				{
					dataGridViewRow.HeaderCell = (DataGridViewRowHeaderCell)this.HeaderCell.Clone();
				}
				dataGridViewRow.CloneCells(this);
			}
			return dataGridViewRow;
		}

		// Token: 0x060036DC RID: 14044 RVA: 0x000C55AC File Offset: 0x000C45AC
		private void CloneCells(DataGridViewRow rowTemplate)
		{
			int count = rowTemplate.Cells.Count;
			if (count > 0)
			{
				DataGridViewCell[] array = new DataGridViewCell[count];
				for (int i = 0; i < count; i++)
				{
					DataGridViewCell dataGridViewCell = rowTemplate.Cells[i];
					DataGridViewCell dataGridViewCell2 = (DataGridViewCell)dataGridViewCell.Clone();
					array[i] = dataGridViewCell2;
				}
				this.Cells.AddRange(array);
			}
		}

		// Token: 0x060036DD RID: 14045 RVA: 0x000C5607 File Offset: 0x000C4607
		protected virtual AccessibleObject CreateAccessibilityInstance()
		{
			return new DataGridViewRow.DataGridViewRowAccessibleObject(this);
		}

		// Token: 0x060036DE RID: 14046 RVA: 0x000C5610 File Offset: 0x000C4610
		public void CreateCells(DataGridView dataGridView)
		{
			if (dataGridView == null)
			{
				throw new ArgumentNullException("dataGridView");
			}
			if (base.DataGridView != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_RowAlreadyBelongsToDataGridView"));
			}
			DataGridViewCellCollection cells = this.Cells;
			cells.Clear();
			DataGridViewColumnCollection columns = dataGridView.Columns;
			foreach (object obj in columns)
			{
				DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)obj;
				if (dataGridViewColumn.CellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_AColumnHasNoCellTemplate"));
				}
				DataGridViewCell dataGridViewCell = (DataGridViewCell)dataGridViewColumn.CellTemplate.Clone();
				cells.Add(dataGridViewCell);
			}
		}

		// Token: 0x060036DF RID: 14047 RVA: 0x000C56D0 File Offset: 0x000C46D0
		public void CreateCells(DataGridView dataGridView, params object[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			this.CreateCells(dataGridView);
			this.SetValuesInternal(values);
		}

		// Token: 0x060036E0 RID: 14048 RVA: 0x000C56EF File Offset: 0x000C46EF
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual DataGridViewCellCollection CreateCellsInstance()
		{
			return new DataGridViewCellCollection(this);
		}

		// Token: 0x060036E1 RID: 14049 RVA: 0x000C56F8 File Offset: 0x000C46F8
		internal void DetachFromDataGridView()
		{
			if (base.DataGridView != null)
			{
				base.DataGridViewInternal = null;
				base.IndexInternal = -1;
				if (base.HasHeaderCell)
				{
					this.HeaderCell.DataGridViewInternal = null;
				}
				foreach (object obj in this.Cells)
				{
					DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
					dataGridViewCell.DataGridViewInternal = null;
					if (dataGridViewCell.Selected)
					{
						dataGridViewCell.SelectedInternal = false;
					}
				}
				if (this.Selected)
				{
					base.SelectedInternal = false;
				}
			}
		}

		// Token: 0x060036E2 RID: 14050 RVA: 0x000C579C File Offset: 0x000C479C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected internal virtual void DrawFocus(Graphics graphics, Rectangle clipBounds, Rectangle bounds, int rowIndex, DataGridViewElementStates rowState, DataGridViewCellStyle cellStyle, bool cellsPaintSelectionBackground)
		{
			if (base.DataGridView == null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_RowDoesNotYetBelongToDataGridView"));
			}
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			if (cellStyle == null)
			{
				throw new ArgumentNullException("cellStyle");
			}
			Color color;
			if (cellsPaintSelectionBackground && (rowState & DataGridViewElementStates.Selected) != DataGridViewElementStates.None)
			{
				color = cellStyle.SelectionBackColor;
			}
			else
			{
				color = cellStyle.BackColor;
			}
			ControlPaint.DrawFocusRectangle(graphics, bounds, Color.Empty, color);
		}

		// Token: 0x060036E3 RID: 14051 RVA: 0x000C5808 File Offset: 0x000C4808
		public ContextMenuStrip GetContextMenuStrip(int rowIndex)
		{
			ContextMenuStrip contextMenuStrip = base.ContextMenuStripInternal;
			if (base.DataGridView != null)
			{
				if (rowIndex == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidOperationOnSharedRow"));
				}
				if (rowIndex < 0 || rowIndex >= base.DataGridView.Rows.Count)
				{
					throw new ArgumentOutOfRangeException("rowIndex");
				}
				if (base.DataGridView.VirtualMode || base.DataGridView.DataSource != null)
				{
					contextMenuStrip = base.DataGridView.OnRowContextMenuStripNeeded(rowIndex, contextMenuStrip);
				}
			}
			return contextMenuStrip;
		}

		// Token: 0x060036E4 RID: 14052 RVA: 0x000C5883 File Offset: 0x000C4883
		internal bool GetDisplayed(int rowIndex)
		{
			return (this.GetState(rowIndex) & DataGridViewElementStates.Displayed) != DataGridViewElementStates.None;
		}

		// Token: 0x060036E5 RID: 14053 RVA: 0x000C5894 File Offset: 0x000C4894
		public string GetErrorText(int rowIndex)
		{
			string text = this.ErrorTextInternal;
			if (base.DataGridView != null)
			{
				if (rowIndex == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidOperationOnSharedRow"));
				}
				if (rowIndex < 0 || rowIndex >= base.DataGridView.Rows.Count)
				{
					throw new ArgumentOutOfRangeException("rowIndex");
				}
				if (string.IsNullOrEmpty(text) && base.DataGridView.DataSource != null && rowIndex != base.DataGridView.NewRowIndex)
				{
					text = base.DataGridView.DataConnection.GetError(rowIndex);
				}
				if (base.DataGridView.DataSource != null || base.DataGridView.VirtualMode)
				{
					text = base.DataGridView.OnRowErrorTextNeeded(rowIndex, text);
				}
			}
			return text;
		}

		// Token: 0x060036E6 RID: 14054 RVA: 0x000C5947 File Offset: 0x000C4947
		internal bool GetFrozen(int rowIndex)
		{
			return (this.GetState(rowIndex) & DataGridViewElementStates.Frozen) != DataGridViewElementStates.None;
		}

		// Token: 0x060036E7 RID: 14055 RVA: 0x000C5958 File Offset: 0x000C4958
		internal int GetHeight(int rowIndex)
		{
			int num;
			int num2;
			base.GetHeightInfo(rowIndex, out num, out num2);
			return num;
		}

		// Token: 0x060036E8 RID: 14056 RVA: 0x000C5974 File Offset: 0x000C4974
		internal int GetMinimumHeight(int rowIndex)
		{
			int num;
			int num2;
			base.GetHeightInfo(rowIndex, out num, out num2);
			return num2;
		}

		// Token: 0x060036E9 RID: 14057 RVA: 0x000C5990 File Offset: 0x000C4990
		public virtual int GetPreferredHeight(int rowIndex, DataGridViewAutoSizeRowMode autoSizeRowMode, bool fixedWidth)
		{
			if ((autoSizeRowMode & (DataGridViewAutoSizeRowMode)(-4)) != (DataGridViewAutoSizeRowMode)0)
			{
				throw new InvalidEnumArgumentException("autoSizeRowMode", (int)autoSizeRowMode, typeof(DataGridViewAutoSizeRowMode));
			}
			if (base.DataGridView != null && (rowIndex < 0 || rowIndex >= base.DataGridView.Rows.Count))
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			if (base.DataGridView == null)
			{
				return -1;
			}
			int num = 0;
			if (base.DataGridView.RowHeadersVisible && (autoSizeRowMode & DataGridViewAutoSizeRowMode.RowHeader) != (DataGridViewAutoSizeRowMode)0)
			{
				if (fixedWidth || base.DataGridView.RowHeadersWidthSizeMode == DataGridViewRowHeadersWidthSizeMode.EnableResizing || base.DataGridView.RowHeadersWidthSizeMode == DataGridViewRowHeadersWidthSizeMode.DisableResizing)
				{
					num = Math.Max(num, this.HeaderCell.GetPreferredHeight(rowIndex, base.DataGridView.RowHeadersWidth));
				}
				else
				{
					num = Math.Max(num, this.HeaderCell.GetPreferredSize(rowIndex).Height);
				}
			}
			if ((autoSizeRowMode & DataGridViewAutoSizeRowMode.AllCellsExceptHeader) != (DataGridViewAutoSizeRowMode)0)
			{
				foreach (object obj in this.Cells)
				{
					DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
					DataGridViewColumn dataGridViewColumn = base.DataGridView.Columns[dataGridViewCell.ColumnIndex];
					if (dataGridViewColumn.Visible)
					{
						int num2;
						if (fixedWidth || (dataGridViewColumn.InheritedAutoSizeMode & (DataGridViewAutoSizeColumnMode)12) == DataGridViewAutoSizeColumnMode.NotSet)
						{
							num2 = dataGridViewCell.GetPreferredHeight(rowIndex, dataGridViewColumn.Width);
						}
						else
						{
							num2 = dataGridViewCell.GetPreferredSize(rowIndex).Height;
						}
						if (num < num2)
						{
							num = num2;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060036EA RID: 14058 RVA: 0x000C5B04 File Offset: 0x000C4B04
		internal bool GetReadOnly(int rowIndex)
		{
			return (this.GetState(rowIndex) & DataGridViewElementStates.ReadOnly) != DataGridViewElementStates.None || (base.DataGridView != null && base.DataGridView.ReadOnly);
		}

		// Token: 0x060036EB RID: 14059 RVA: 0x000C5B28 File Offset: 0x000C4B28
		internal DataGridViewTriState GetResizable(int rowIndex)
		{
			if ((this.GetState(rowIndex) & DataGridViewElementStates.ResizableSet) != DataGridViewElementStates.None)
			{
				if ((this.GetState(rowIndex) & DataGridViewElementStates.Resizable) == DataGridViewElementStates.None)
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
				if (!base.DataGridView.AllowUserToResizeRows)
				{
					return DataGridViewTriState.False;
				}
				return DataGridViewTriState.True;
			}
		}

		// Token: 0x060036EC RID: 14060 RVA: 0x000C5B5F File Offset: 0x000C4B5F
		internal bool GetSelected(int rowIndex)
		{
			return (this.GetState(rowIndex) & DataGridViewElementStates.Selected) != DataGridViewElementStates.None;
		}

		// Token: 0x060036ED RID: 14061 RVA: 0x000C5B74 File Offset: 0x000C4B74
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual DataGridViewElementStates GetState(int rowIndex)
		{
			if (base.DataGridView != null && (rowIndex < 0 || rowIndex >= base.DataGridView.Rows.Count))
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			if (base.DataGridView != null && base.DataGridView.Rows.SharedRow(rowIndex).Index == -1)
			{
				return base.DataGridView.Rows.GetRowState(rowIndex);
			}
			if (rowIndex != base.Index)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"rowIndex",
					rowIndex.ToString(CultureInfo.CurrentCulture)
				}));
			}
			return base.State;
		}

		// Token: 0x060036EE RID: 14062 RVA: 0x000C5C1C File Offset: 0x000C4C1C
		internal bool GetVisible(int rowIndex)
		{
			return (this.GetState(rowIndex) & DataGridViewElementStates.Visible) != DataGridViewElementStates.None;
		}

		// Token: 0x060036EF RID: 14063 RVA: 0x000C5C2E File Offset: 0x000C4C2E
		internal void OnSharedStateChanged(int sharedRowIndex, DataGridViewElementStates elementState)
		{
			base.DataGridView.Rows.InvalidateCachedRowCount(elementState);
			base.DataGridView.Rows.InvalidateCachedRowsHeight(elementState);
			base.DataGridView.OnDataGridViewElementStateChanged(this, sharedRowIndex, elementState);
		}

		// Token: 0x060036F0 RID: 14064 RVA: 0x000C5C60 File Offset: 0x000C4C60
		internal void OnSharedStateChanging(int sharedRowIndex, DataGridViewElementStates elementState)
		{
			base.DataGridView.OnDataGridViewElementStateChanging(this, sharedRowIndex, elementState);
		}

		// Token: 0x060036F1 RID: 14065 RVA: 0x000C5C70 File Offset: 0x000C4C70
		protected internal virtual void Paint(Graphics graphics, Rectangle clipBounds, Rectangle rowBounds, int rowIndex, DataGridViewElementStates rowState, bool isFirstDisplayedRow, bool isLastVisibleRow)
		{
			if (base.DataGridView == null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_RowDoesNotYetBelongToDataGridView"));
			}
			DataGridView dataGridView = base.DataGridView;
			DataGridViewRow dataGridViewRow = dataGridView.Rows.SharedRow(rowIndex);
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			this.BuildInheritedRowStyle(rowIndex, dataGridViewCellStyle);
			DataGridViewRowPrePaintEventArgs rowPrePaintEventArgs = dataGridView.RowPrePaintEventArgs;
			rowPrePaintEventArgs.SetProperties(graphics, clipBounds, rowBounds, rowIndex, rowState, dataGridViewRow.GetErrorText(rowIndex), dataGridViewCellStyle, isFirstDisplayedRow, isLastVisibleRow);
			dataGridView.OnRowPrePaint(rowPrePaintEventArgs);
			if (rowPrePaintEventArgs.Handled)
			{
				return;
			}
			DataGridViewPaintParts paintParts = rowPrePaintEventArgs.PaintParts;
			Rectangle clipBounds2 = rowPrePaintEventArgs.ClipBounds;
			this.PaintHeader(graphics, clipBounds2, rowBounds, rowIndex, rowState, isFirstDisplayedRow, isLastVisibleRow, paintParts);
			this.PaintCells(graphics, clipBounds2, rowBounds, rowIndex, rowState, isFirstDisplayedRow, isLastVisibleRow, paintParts);
			dataGridViewRow = dataGridView.Rows.SharedRow(rowIndex);
			this.BuildInheritedRowStyle(rowIndex, dataGridViewCellStyle);
			DataGridViewRowPostPaintEventArgs rowPostPaintEventArgs = dataGridView.RowPostPaintEventArgs;
			rowPostPaintEventArgs.SetProperties(graphics, clipBounds2, rowBounds, rowIndex, rowState, dataGridViewRow.GetErrorText(rowIndex), dataGridViewCellStyle, isFirstDisplayedRow, isLastVisibleRow);
			dataGridView.OnRowPostPaint(rowPostPaintEventArgs);
		}

		// Token: 0x060036F2 RID: 14066 RVA: 0x000C5D6C File Offset: 0x000C4D6C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected internal virtual void PaintCells(Graphics graphics, Rectangle clipBounds, Rectangle rowBounds, int rowIndex, DataGridViewElementStates rowState, bool isFirstDisplayedRow, bool isLastVisibleRow, DataGridViewPaintParts paintParts)
		{
			if (base.DataGridView == null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_RowDoesNotYetBelongToDataGridView"));
			}
			if (paintParts < DataGridViewPaintParts.None || paintParts > DataGridViewPaintParts.All)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewPaintPartsCombination", new object[] { "paintParts" }));
			}
			DataGridView dataGridView = base.DataGridView;
			Rectangle rectangle = rowBounds;
			int num = (dataGridView.RowHeadersVisible ? dataGridView.RowHeadersWidth : 0);
			bool flag = true;
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle = new DataGridViewAdvancedBorderStyle();
			DataGridViewColumn dataGridViewColumn = dataGridView.Columns.GetFirstColumn(DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible);
			while (dataGridViewColumn != null)
			{
				DataGridViewCell dataGridViewCell = this.Cells[dataGridViewColumn.Index];
				rectangle.Width = dataGridViewColumn.Thickness;
				if (dataGridView.SingleVerticalBorderAdded && flag)
				{
					rectangle.Width++;
				}
				if (dataGridView.RightToLeftInternal)
				{
					rectangle.X = rowBounds.Right - num - rectangle.Width;
				}
				else
				{
					rectangle.X = rowBounds.X + num;
				}
				DataGridViewColumn dataGridViewColumn2 = dataGridView.Columns.GetNextColumn(dataGridViewColumn, DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible, DataGridViewElementStates.None);
				if (clipBounds.IntersectsWith(rectangle))
				{
					DataGridViewElementStates dataGridViewElementStates = dataGridViewCell.CellStateFromColumnRowStates(rowState);
					if (base.Index != -1)
					{
						dataGridViewElementStates |= dataGridViewCell.State;
					}
					dataGridViewCell.GetInheritedStyle(dataGridViewCellStyle, rowIndex, true);
					DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle2 = dataGridViewCell.AdjustCellBorderStyle(dataGridView.AdvancedCellBorderStyle, dataGridViewAdvancedBorderStyle, dataGridView.SingleVerticalBorderAdded, dataGridView.SingleHorizontalBorderAdded, flag, isFirstDisplayedRow);
					dataGridViewCell.PaintWork(graphics, clipBounds, rectangle, rowIndex, dataGridViewElementStates, dataGridViewCellStyle, dataGridViewAdvancedBorderStyle2, paintParts);
				}
				num += rectangle.Width;
				if (num >= rowBounds.Width)
				{
					break;
				}
				dataGridViewColumn = dataGridViewColumn2;
				flag = false;
			}
			Rectangle rectangle2 = rowBounds;
			if (num < rectangle2.Width && dataGridView.FirstDisplayedScrollingColumnIndex >= 0)
			{
				if (!dataGridView.RightToLeftInternal)
				{
					rectangle2.X -= dataGridView.FirstDisplayedScrollingColumnHiddenWidth;
				}
				rectangle2.Width += dataGridView.FirstDisplayedScrollingColumnHiddenWidth;
				Region region = null;
				if (dataGridView.FirstDisplayedScrollingColumnHiddenWidth > 0)
				{
					region = graphics.Clip;
					Rectangle rectangle3 = rowBounds;
					if (!dataGridView.RightToLeftInternal)
					{
						rectangle3.X += num;
					}
					rectangle3.Width -= num;
					graphics.SetClip(rectangle3);
				}
				dataGridViewColumn = dataGridView.Columns[dataGridView.FirstDisplayedScrollingColumnIndex];
				while (dataGridViewColumn != null)
				{
					DataGridViewCell dataGridViewCell = this.Cells[dataGridViewColumn.Index];
					rectangle.Width = dataGridViewColumn.Thickness;
					if (dataGridView.SingleVerticalBorderAdded && flag)
					{
						rectangle.Width++;
					}
					if (dataGridView.RightToLeftInternal)
					{
						rectangle.X = rectangle2.Right - num - rectangle.Width;
					}
					else
					{
						rectangle.X = rectangle2.X + num;
					}
					DataGridViewColumn dataGridViewColumn2 = dataGridView.Columns.GetNextColumn(dataGridViewColumn, DataGridViewElementStates.Visible, DataGridViewElementStates.None);
					if (clipBounds.IntersectsWith(rectangle))
					{
						DataGridViewElementStates dataGridViewElementStates = dataGridViewCell.CellStateFromColumnRowStates(rowState);
						if (base.Index != -1)
						{
							dataGridViewElementStates |= dataGridViewCell.State;
						}
						dataGridViewCell.GetInheritedStyle(dataGridViewCellStyle, rowIndex, true);
						DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle2 = dataGridViewCell.AdjustCellBorderStyle(dataGridView.AdvancedCellBorderStyle, dataGridViewAdvancedBorderStyle, dataGridView.SingleVerticalBorderAdded, dataGridView.SingleHorizontalBorderAdded, flag, isFirstDisplayedRow);
						dataGridViewCell.PaintWork(graphics, clipBounds, rectangle, rowIndex, dataGridViewElementStates, dataGridViewCellStyle, dataGridViewAdvancedBorderStyle2, paintParts);
					}
					num += rectangle.Width;
					if (num >= rectangle2.Width)
					{
						break;
					}
					dataGridViewColumn = dataGridViewColumn2;
					flag = false;
				}
				if (region != null)
				{
					graphics.Clip = region;
					region.Dispose();
				}
			}
		}

		// Token: 0x060036F3 RID: 14067 RVA: 0x000C60D8 File Offset: 0x000C50D8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected internal virtual void PaintHeader(Graphics graphics, Rectangle clipBounds, Rectangle rowBounds, int rowIndex, DataGridViewElementStates rowState, bool isFirstDisplayedRow, bool isLastVisibleRow, DataGridViewPaintParts paintParts)
		{
			if (base.DataGridView == null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_RowDoesNotYetBelongToDataGridView"));
			}
			if (paintParts < DataGridViewPaintParts.None || paintParts > DataGridViewPaintParts.All)
			{
				throw new InvalidEnumArgumentException("paintParts", (int)paintParts, typeof(DataGridViewPaintParts));
			}
			DataGridView dataGridView = base.DataGridView;
			if (dataGridView.RowHeadersVisible)
			{
				Rectangle rectangle = rowBounds;
				rectangle.Width = dataGridView.RowHeadersWidth;
				if (dataGridView.RightToLeftInternal)
				{
					rectangle.X = rowBounds.Right - rectangle.Width;
				}
				if (clipBounds.IntersectsWith(rectangle))
				{
					DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
					DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle = new DataGridViewAdvancedBorderStyle();
					this.BuildInheritedRowHeaderCellStyle(dataGridViewCellStyle);
					DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyle2 = this.AdjustRowHeaderBorderStyle(dataGridView.AdvancedRowHeadersBorderStyle, dataGridViewAdvancedBorderStyle, dataGridView.SingleVerticalBorderAdded, dataGridView.SingleHorizontalBorderAdded, isFirstDisplayedRow, isLastVisibleRow);
					this.HeaderCell.PaintWork(graphics, clipBounds, rectangle, rowIndex, rowState, dataGridViewCellStyle, dataGridViewAdvancedBorderStyle2, paintParts);
				}
			}
		}

		// Token: 0x060036F4 RID: 14068 RVA: 0x000C61B4 File Offset: 0x000C51B4
		internal void SetReadOnlyCellCore(DataGridViewCell dataGridViewCell, bool readOnly)
		{
			if (this.ReadOnly && !readOnly)
			{
				foreach (object obj in this.Cells)
				{
					DataGridViewCell dataGridViewCell2 = (DataGridViewCell)obj;
					dataGridViewCell2.ReadOnlyInternal = true;
				}
				dataGridViewCell.ReadOnlyInternal = false;
				this.ReadOnly = false;
				return;
			}
			if (!this.ReadOnly && readOnly)
			{
				dataGridViewCell.ReadOnlyInternal = true;
			}
		}

		// Token: 0x060036F5 RID: 14069 RVA: 0x000C623C File Offset: 0x000C523C
		public bool SetValues(params object[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			if (base.DataGridView != null)
			{
				if (base.DataGridView.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidOperationInVirtualMode"));
				}
				if (base.Index == -1)
				{
					throw new InvalidOperationException(SR.GetString("DataGridView_InvalidOperationOnSharedRow"));
				}
			}
			return this.SetValuesInternal(values);
		}

		// Token: 0x060036F6 RID: 14070 RVA: 0x000C629C File Offset: 0x000C529C
		internal bool SetValuesInternal(params object[] values)
		{
			bool flag = true;
			DataGridViewCellCollection cells = this.Cells;
			int count = cells.Count;
			int num = 0;
			while (num < cells.Count && num != values.Length)
			{
				if (!cells[num].SetValueInternal(base.Index, values[num]))
				{
					flag = false;
				}
				num++;
			}
			return flag && values.Length <= count;
		}

		// Token: 0x060036F7 RID: 14071 RVA: 0x000C62F8 File Offset: 0x000C52F8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(36);
			stringBuilder.Append("DataGridViewRow { Index=");
			stringBuilder.Append(base.Index.ToString(CultureInfo.CurrentCulture));
			stringBuilder.Append(" }");
			return stringBuilder.ToString();
		}

		// Token: 0x04001BF9 RID: 7161
		private const DataGridViewAutoSizeRowCriteriaInternal invalidDataGridViewAutoSizeRowCriteriaInternalMask = ~(DataGridViewAutoSizeRowCriteriaInternal.Header | DataGridViewAutoSizeRowCriteriaInternal.AllColumns);

		// Token: 0x04001BFA RID: 7162
		internal const int defaultMinRowThickness = 3;

		// Token: 0x04001BFB RID: 7163
		private static Type rowType = typeof(DataGridViewRow);

		// Token: 0x04001BFC RID: 7164
		private static readonly int PropRowErrorText = PropertyStore.CreateKey();

		// Token: 0x04001BFD RID: 7165
		private static readonly int PropRowAccessibilityObject = PropertyStore.CreateKey();

		// Token: 0x04001BFE RID: 7166
		private DataGridViewCellCollection rowCells;

		// Token: 0x0200037E RID: 894
		[ComVisible(true)]
		protected class DataGridViewRowAccessibleObject : AccessibleObject
		{
			// Token: 0x060036F9 RID: 14073 RVA: 0x000C636A File Offset: 0x000C536A
			public DataGridViewRowAccessibleObject()
			{
			}

			// Token: 0x060036FA RID: 14074 RVA: 0x000C6372 File Offset: 0x000C5372
			public DataGridViewRowAccessibleObject(DataGridViewRow owner)
			{
				this.owner = owner;
			}

			// Token: 0x17000A1F RID: 2591
			// (get) Token: 0x060036FB RID: 14075 RVA: 0x000C6384 File Offset: 0x000C5384
			public override Rectangle Bounds
			{
				get
				{
					if (this.owner == null)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewRowAccessibleObject_OwnerNotSet"));
					}
					Rectangle rectangle;
					if (this.owner.Index < this.owner.DataGridView.FirstDisplayedScrollingRowIndex)
					{
						int rowCount = this.owner.DataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible, 0, this.owner.Index);
						rectangle = this.ParentPrivate.GetChild(rowCount + 1 + 1).Bounds;
						rectangle.Y -= this.owner.Height;
						rectangle.Height = this.owner.Height;
					}
					else if (this.owner.Index >= this.owner.DataGridView.FirstDisplayedScrollingRowIndex && this.owner.Index < this.owner.DataGridView.FirstDisplayedScrollingRowIndex + this.owner.DataGridView.DisplayedRowCount(true))
					{
						rectangle = this.owner.DataGridView.GetRowDisplayRectangle(this.owner.Index, false);
						rectangle = this.owner.DataGridView.RectangleToScreen(rectangle);
					}
					else
					{
						int num = this.owner.DataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible, 0, this.owner.Index);
						if (!this.owner.DataGridView.Rows[0].Visible)
						{
							num--;
						}
						if (!this.owner.DataGridView.ColumnHeadersVisible)
						{
							num--;
						}
						rectangle = this.ParentPrivate.GetChild(num).Bounds;
						rectangle.Y += rectangle.Height;
						rectangle.Height = this.owner.Height;
					}
					return rectangle;
				}
			}

			// Token: 0x17000A20 RID: 2592
			// (get) Token: 0x060036FC RID: 14076 RVA: 0x000C6548 File Offset: 0x000C5548
			public override string Name
			{
				get
				{
					if (this.owner == null)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewRowAccessibleObject_OwnerNotSet"));
					}
					return SR.GetString("DataGridView_AccRowName", new object[] { this.owner.Index.ToString(CultureInfo.CurrentCulture) });
				}
			}

			// Token: 0x17000A21 RID: 2593
			// (get) Token: 0x060036FD RID: 14077 RVA: 0x000C659A File Offset: 0x000C559A
			// (set) Token: 0x060036FE RID: 14078 RVA: 0x000C65A2 File Offset: 0x000C55A2
			public DataGridViewRow Owner
			{
				get
				{
					return this.owner;
				}
				set
				{
					if (this.owner != null)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewRowAccessibleObject_OwnerAlreadySet"));
					}
					this.owner = value;
				}
			}

			// Token: 0x17000A22 RID: 2594
			// (get) Token: 0x060036FF RID: 14079 RVA: 0x000C65C3 File Offset: 0x000C55C3
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.ParentPrivate;
				}
			}

			// Token: 0x17000A23 RID: 2595
			// (get) Token: 0x06003700 RID: 14080 RVA: 0x000C65CB File Offset: 0x000C55CB
			private AccessibleObject ParentPrivate
			{
				get
				{
					if (this.owner == null)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewRowAccessibleObject_OwnerNotSet"));
					}
					return this.owner.DataGridView.AccessibilityObject;
				}
			}

			// Token: 0x17000A24 RID: 2596
			// (get) Token: 0x06003701 RID: 14081 RVA: 0x000C65F5 File Offset: 0x000C55F5
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.Row;
				}
			}

			// Token: 0x17000A25 RID: 2597
			// (get) Token: 0x06003702 RID: 14082 RVA: 0x000C65F9 File Offset: 0x000C55F9
			private AccessibleObject SelectedCellsAccessibilityObject
			{
				get
				{
					if (this.owner == null)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewRowAccessibleObject_OwnerNotSet"));
					}
					if (this.selectedCellsAccessibilityObject == null)
					{
						this.selectedCellsAccessibilityObject = new DataGridViewRow.DataGridViewSelectedRowCellsAccessibleObject(this.owner);
					}
					return this.selectedCellsAccessibilityObject;
				}
			}

			// Token: 0x17000A26 RID: 2598
			// (get) Token: 0x06003703 RID: 14083 RVA: 0x000C6634 File Offset: 0x000C5634
			public override AccessibleStates State
			{
				get
				{
					if (this.owner == null)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewRowAccessibleObject_OwnerNotSet"));
					}
					AccessibleStates accessibleStates = AccessibleStates.Selectable;
					bool flag = true;
					if (this.owner.Selected)
					{
						flag = true;
					}
					else
					{
						for (int i = 0; i < this.owner.Cells.Count; i++)
						{
							if (!this.owner.Cells[i].Selected)
							{
								flag = false;
								break;
							}
						}
					}
					if (flag)
					{
						accessibleStates |= AccessibleStates.Selected;
					}
					if (!this.owner.DataGridView.GetRowDisplayRectangle(this.owner.Index, true).IntersectsWith(this.owner.DataGridView.ClientRectangle))
					{
						accessibleStates |= AccessibleStates.Offscreen;
					}
					return accessibleStates;
				}
			}

			// Token: 0x17000A27 RID: 2599
			// (get) Token: 0x06003704 RID: 14084 RVA: 0x000C66F0 File Offset: 0x000C56F0
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					if (this.owner == null)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewRowAccessibleObject_OwnerNotSet"));
					}
					if (this.owner.DataGridView.AllowUserToAddRows && this.owner.Index == this.owner.DataGridView.NewRowIndex)
					{
						return SR.GetString("DataGridView_AccRowCreateNew");
					}
					StringBuilder stringBuilder = new StringBuilder(1024);
					int childCount = this.GetChildCount();
					int num = (this.owner.DataGridView.RowHeadersVisible ? 1 : 0);
					for (int i = num; i < childCount; i++)
					{
						AccessibleObject child = this.GetChild(i);
						if (child != null)
						{
							stringBuilder.Append(child.Value);
						}
						if (i != childCount - 1)
						{
							stringBuilder.Append(";");
						}
					}
					return stringBuilder.ToString();
				}
			}

			// Token: 0x06003705 RID: 14085 RVA: 0x000C67B8 File Offset: 0x000C57B8
			public override AccessibleObject GetChild(int index)
			{
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewRowAccessibleObject_OwnerNotSet"));
				}
				if (index == 0 && this.owner.DataGridView.RowHeadersVisible)
				{
					return this.owner.HeaderCell.AccessibilityObject;
				}
				if (this.owner.DataGridView.RowHeadersVisible)
				{
					index--;
				}
				int num = this.owner.DataGridView.Columns.ActualDisplayIndexToColumnIndex(index, DataGridViewElementStates.Visible);
				return this.owner.Cells[num].AccessibilityObject;
			}

			// Token: 0x06003706 RID: 14086 RVA: 0x000C6858 File Offset: 0x000C5858
			public override int GetChildCount()
			{
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewRowAccessibleObject_OwnerNotSet"));
				}
				int num = this.owner.DataGridView.Columns.GetColumnCount(DataGridViewElementStates.Visible);
				if (this.owner.DataGridView.RowHeadersVisible)
				{
					num++;
				}
				return num;
			}

			// Token: 0x06003707 RID: 14087 RVA: 0x000C68AC File Offset: 0x000C58AC
			public override AccessibleObject GetSelected()
			{
				return this.SelectedCellsAccessibilityObject;
			}

			// Token: 0x06003708 RID: 14088 RVA: 0x000C68B4 File Offset: 0x000C58B4
			public override AccessibleObject GetFocused()
			{
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewRowAccessibleObject_OwnerNotSet"));
				}
				if (this.owner.DataGridView.Focused && this.owner.DataGridView.CurrentCell != null && this.owner.DataGridView.CurrentCell.RowIndex == this.owner.Index)
				{
					return this.owner.DataGridView.CurrentCell.AccessibilityObject;
				}
				return null;
			}

			// Token: 0x06003709 RID: 14089 RVA: 0x000C6938 File Offset: 0x000C5938
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navigationDirection)
			{
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewRowAccessibleObject_OwnerNotSet"));
				}
				switch (navigationDirection)
				{
				case AccessibleNavigation.Up:
				case AccessibleNavigation.Previous:
					if (this.owner.Index != this.owner.DataGridView.Rows.GetFirstRow(DataGridViewElementStates.Visible))
					{
						int previousRow = this.owner.DataGridView.Rows.GetPreviousRow(this.owner.Index, DataGridViewElementStates.Visible);
						int rowCount = this.owner.DataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible, 0, previousRow);
						if (this.owner.DataGridView.ColumnHeadersVisible)
						{
							return this.owner.DataGridView.AccessibilityObject.GetChild(rowCount + 1);
						}
						return this.owner.DataGridView.AccessibilityObject.GetChild(rowCount);
					}
					else
					{
						if (this.owner.DataGridView.ColumnHeadersVisible)
						{
							return this.ParentPrivate.GetChild(0);
						}
						return null;
					}
					break;
				case AccessibleNavigation.Down:
				case AccessibleNavigation.Next:
				{
					if (this.owner.Index == this.owner.DataGridView.Rows.GetLastRow(DataGridViewElementStates.Visible))
					{
						return null;
					}
					int nextRow = this.owner.DataGridView.Rows.GetNextRow(this.owner.Index, DataGridViewElementStates.Visible);
					int rowCount2 = this.owner.DataGridView.Rows.GetRowCount(DataGridViewElementStates.Visible, 0, nextRow);
					if (this.owner.DataGridView.ColumnHeadersVisible)
					{
						return this.owner.DataGridView.AccessibilityObject.GetChild(rowCount2 + 1);
					}
					return this.owner.DataGridView.AccessibilityObject.GetChild(rowCount2);
				}
				case AccessibleNavigation.FirstChild:
					if (this.GetChildCount() == 0)
					{
						return null;
					}
					return this.GetChild(0);
				case AccessibleNavigation.LastChild:
				{
					int childCount = this.GetChildCount();
					if (childCount == 0)
					{
						return null;
					}
					return this.GetChild(childCount - 1);
				}
				}
				return null;
			}

			// Token: 0x0600370A RID: 14090 RVA: 0x000C6B24 File Offset: 0x000C5B24
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void Select(AccessibleSelection flags)
			{
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewRowAccessibleObject_OwnerNotSet"));
				}
				DataGridView dataGridView = this.owner.DataGridView;
				if (dataGridView == null)
				{
					return;
				}
				if ((flags & AccessibleSelection.TakeFocus) == AccessibleSelection.TakeFocus)
				{
					dataGridView.FocusInternal();
				}
				if ((flags & AccessibleSelection.TakeSelection) == AccessibleSelection.TakeSelection && this.owner.Cells.Count > 0)
				{
					if (dataGridView.CurrentCell != null && dataGridView.CurrentCell.OwningColumn != null)
					{
						dataGridView.CurrentCell = this.owner.Cells[dataGridView.CurrentCell.OwningColumn.Index];
					}
					else
					{
						int index = dataGridView.Columns.GetFirstColumn(DataGridViewElementStates.Visible).Index;
						if (index > -1)
						{
							dataGridView.CurrentCell = this.owner.Cells[index];
						}
					}
				}
				if ((flags & AccessibleSelection.AddSelection) == AccessibleSelection.AddSelection && (flags & AccessibleSelection.TakeSelection) == AccessibleSelection.None && (dataGridView.SelectionMode == DataGridViewSelectionMode.FullRowSelect || dataGridView.SelectionMode == DataGridViewSelectionMode.RowHeaderSelect))
				{
					this.owner.Selected = true;
				}
				if ((flags & AccessibleSelection.RemoveSelection) == AccessibleSelection.RemoveSelection && (flags & (AccessibleSelection.TakeSelection | AccessibleSelection.AddSelection)) == AccessibleSelection.None)
				{
					this.owner.Selected = false;
				}
			}

			// Token: 0x04001BFF RID: 7167
			private DataGridViewRow owner;

			// Token: 0x04001C00 RID: 7168
			private DataGridViewRow.DataGridViewSelectedRowCellsAccessibleObject selectedCellsAccessibilityObject;
		}

		// Token: 0x0200037F RID: 895
		private class DataGridViewSelectedRowCellsAccessibleObject : AccessibleObject
		{
			// Token: 0x0600370B RID: 14091 RVA: 0x000C6C2D File Offset: 0x000C5C2D
			internal DataGridViewSelectedRowCellsAccessibleObject(DataGridViewRow owner)
			{
				this.owner = owner;
			}

			// Token: 0x17000A28 RID: 2600
			// (get) Token: 0x0600370C RID: 14092 RVA: 0x000C6C3C File Offset: 0x000C5C3C
			public override string Name
			{
				get
				{
					return SR.GetString("DataGridView_AccSelectedRowCellsName");
				}
			}

			// Token: 0x17000A29 RID: 2601
			// (get) Token: 0x0600370D RID: 14093 RVA: 0x000C6C48 File Offset: 0x000C5C48
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.owner.AccessibilityObject;
				}
			}

			// Token: 0x17000A2A RID: 2602
			// (get) Token: 0x0600370E RID: 14094 RVA: 0x000C6C55 File Offset: 0x000C5C55
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.Grouping;
				}
			}

			// Token: 0x17000A2B RID: 2603
			// (get) Token: 0x0600370F RID: 14095 RVA: 0x000C6C59 File Offset: 0x000C5C59
			public override AccessibleStates State
			{
				get
				{
					return AccessibleStates.Selected | AccessibleStates.Selectable;
				}
			}

			// Token: 0x17000A2C RID: 2604
			// (get) Token: 0x06003710 RID: 14096 RVA: 0x000C6C60 File Offset: 0x000C5C60
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.Name;
				}
			}

			// Token: 0x06003711 RID: 14097 RVA: 0x000C6C68 File Offset: 0x000C5C68
			public override AccessibleObject GetChild(int index)
			{
				if (index < this.GetChildCount())
				{
					int num = -1;
					for (int i = 1; i < this.owner.AccessibilityObject.GetChildCount(); i++)
					{
						if ((this.owner.AccessibilityObject.GetChild(i).State & AccessibleStates.Selected) == AccessibleStates.Selected)
						{
							num++;
						}
						if (num == index)
						{
							return this.owner.AccessibilityObject.GetChild(i);
						}
					}
					return null;
				}
				return null;
			}

			// Token: 0x06003712 RID: 14098 RVA: 0x000C6CD4 File Offset: 0x000C5CD4
			public override int GetChildCount()
			{
				int num = 0;
				for (int i = 1; i < this.owner.AccessibilityObject.GetChildCount(); i++)
				{
					if ((this.owner.AccessibilityObject.GetChild(i).State & AccessibleStates.Selected) == AccessibleStates.Selected)
					{
						num++;
					}
				}
				return num;
			}

			// Token: 0x06003713 RID: 14099 RVA: 0x000C6D1E File Offset: 0x000C5D1E
			public override AccessibleObject GetSelected()
			{
				return this;
			}

			// Token: 0x06003714 RID: 14100 RVA: 0x000C6D24 File Offset: 0x000C5D24
			public override AccessibleObject GetFocused()
			{
				if (this.owner.DataGridView.CurrentCell != null && this.owner.DataGridView.CurrentCell.Selected)
				{
					return this.owner.DataGridView.CurrentCell.AccessibilityObject;
				}
				return null;
			}

			// Token: 0x06003715 RID: 14101 RVA: 0x000C6D74 File Offset: 0x000C5D74
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navigationDirection)
			{
				switch (navigationDirection)
				{
				case AccessibleNavigation.FirstChild:
					if (this.GetChildCount() > 0)
					{
						return this.GetChild(0);
					}
					return null;
				case AccessibleNavigation.LastChild:
					if (this.GetChildCount() > 0)
					{
						return this.GetChild(this.GetChildCount() - 1);
					}
					return null;
				default:
					return null;
				}
			}

			// Token: 0x04001C01 RID: 7169
			private DataGridViewRow owner;
		}
	}
}
