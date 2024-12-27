using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms.Layout;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x02000266 RID: 614
	[SRDescription("DescriptionListBox")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Designer("System.Windows.Forms.Design.ListBoxDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultEvent("SelectedIndexChanged")]
	[DefaultProperty("Items")]
	[DefaultBindingProperty("SelectedValue")]
	[ComVisible(true)]
	public class ListBox : ListControl
	{
		// Token: 0x06002054 RID: 8276 RVA: 0x00044C50 File Offset: 0x00043C50
		public ListBox()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.StandardClick | ControlStyles.UseTextForAccessibility, false);
			base.SetState2(2048, true);
			base.SetBounds(0, 0, 120, 96);
			this.requestedHeight = base.Height;
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06002055 RID: 8277 RVA: 0x00044CC5 File Offset: 0x00043CC5
		// (set) Token: 0x06002056 RID: 8278 RVA: 0x00044CDB File Offset: 0x00043CDB
		public override Color BackColor
		{
			get
			{
				if (this.ShouldSerializeBackColor())
				{
					return base.BackColor;
				}
				return SystemColors.Window;
			}
			set
			{
				base.BackColor = value;
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06002057 RID: 8279 RVA: 0x00044CE4 File Offset: 0x00043CE4
		// (set) Token: 0x06002058 RID: 8280 RVA: 0x00044CEC File Offset: 0x00043CEC
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		// Token: 0x140000C9 RID: 201
		// (add) Token: 0x06002059 RID: 8281 RVA: 0x00044CF5 File Offset: 0x00043CF5
		// (remove) Token: 0x0600205A RID: 8282 RVA: 0x00044CFE File Offset: 0x00043CFE
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler BackgroundImageChanged
		{
			add
			{
				base.BackgroundImageChanged += value;
			}
			remove
			{
				base.BackgroundImageChanged -= value;
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x0600205B RID: 8283 RVA: 0x00044D07 File Offset: 0x00043D07
		// (set) Token: 0x0600205C RID: 8284 RVA: 0x00044D0F File Offset: 0x00043D0F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}

		// Token: 0x140000CA RID: 202
		// (add) Token: 0x0600205D RID: 8285 RVA: 0x00044D18 File Offset: 0x00043D18
		// (remove) Token: 0x0600205E RID: 8286 RVA: 0x00044D21 File Offset: 0x00043D21
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler BackgroundImageLayoutChanged
		{
			add
			{
				base.BackgroundImageLayoutChanged += value;
			}
			remove
			{
				base.BackgroundImageLayoutChanged -= value;
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x0600205F RID: 8287 RVA: 0x00044D2A File Offset: 0x00043D2A
		// (set) Token: 0x06002060 RID: 8288 RVA: 0x00044D34 File Offset: 0x00043D34
		[DefaultValue(BorderStyle.Fixed3D)]
		[DispId(-504)]
		[SRCategory("CatAppearance")]
		[SRDescription("ListBoxBorderDescr")]
		public BorderStyle BorderStyle
		{
			get
			{
				return this.borderStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(BorderStyle));
				}
				if (value != this.borderStyle)
				{
					this.borderStyle = value;
					base.RecreateHandle();
					this.integralHeightAdjust = true;
					try
					{
						base.Height = this.requestedHeight;
					}
					finally
					{
						this.integralHeightAdjust = false;
					}
				}
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06002061 RID: 8289 RVA: 0x00044DAC File Offset: 0x00043DAC
		// (set) Token: 0x06002062 RID: 8290 RVA: 0x00044DB4 File Offset: 0x00043DB4
		[SRCategory("CatBehavior")]
		[DefaultValue(0)]
		[Localizable(true)]
		[SRDescription("ListBoxColumnWidthDescr")]
		public int ColumnWidth
		{
			get
			{
				return this.columnWidth;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"value",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.columnWidth != value)
				{
					this.columnWidth = value;
					if (this.columnWidth == 0)
					{
						base.RecreateHandle();
						return;
					}
					if (base.IsHandleCreated)
					{
						base.SendMessage(405, this.columnWidth, 0);
					}
				}
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06002063 RID: 8291 RVA: 0x00044E40 File Offset: 0x00043E40
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "LISTBOX";
				createParams.Style |= 2097217;
				if (this.scrollAlwaysVisible)
				{
					createParams.Style |= 4096;
				}
				if (!this.integralHeight)
				{
					createParams.Style |= 256;
				}
				if (this.useTabStops)
				{
					createParams.Style |= 128;
				}
				switch (this.borderStyle)
				{
				case BorderStyle.FixedSingle:
					createParams.Style |= 8388608;
					break;
				case BorderStyle.Fixed3D:
					createParams.ExStyle |= 512;
					break;
				}
				if (this.multiColumn)
				{
					createParams.Style |= 1049088;
				}
				else if (this.horizontalScrollbar)
				{
					createParams.Style |= 1048576;
				}
				switch (this.selectionMode)
				{
				case SelectionMode.None:
					createParams.Style |= 16384;
					break;
				case SelectionMode.MultiSimple:
					createParams.Style |= 8;
					break;
				case SelectionMode.MultiExtended:
					createParams.Style |= 2048;
					break;
				}
				switch (this.drawMode)
				{
				case DrawMode.OwnerDrawFixed:
					createParams.Style |= 16;
					break;
				case DrawMode.OwnerDrawVariable:
					createParams.Style |= 32;
					break;
				}
				return createParams;
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06002064 RID: 8292 RVA: 0x00044FC5 File Offset: 0x00043FC5
		// (set) Token: 0x06002065 RID: 8293 RVA: 0x00044FCD File Offset: 0x00043FCD
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[Browsable(false)]
		public bool UseCustomTabOffsets
		{
			get
			{
				return this.useCustomTabOffsets;
			}
			set
			{
				if (this.useCustomTabOffsets != value)
				{
					this.useCustomTabOffsets = value;
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06002066 RID: 8294 RVA: 0x00044FE5 File Offset: 0x00043FE5
		protected override Size DefaultSize
		{
			get
			{
				return new Size(120, 96);
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06002067 RID: 8295 RVA: 0x00044FF0 File Offset: 0x00043FF0
		// (set) Token: 0x06002068 RID: 8296 RVA: 0x00044FF8 File Offset: 0x00043FF8
		[SRDescription("ListBoxDrawModeDescr")]
		[DefaultValue(DrawMode.Normal)]
		[SRCategory("CatBehavior")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public virtual DrawMode DrawMode
		{
			get
			{
				return this.drawMode;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DrawMode));
				}
				if (this.drawMode != value)
				{
					if (this.MultiColumn && value == DrawMode.OwnerDrawVariable)
					{
						throw new ArgumentException(SR.GetString("ListBoxVarHeightMultiCol"), "value");
					}
					this.drawMode = value;
					base.RecreateHandle();
					if (this.drawMode == DrawMode.OwnerDrawVariable)
					{
						LayoutTransaction.DoLayoutIf(this.AutoSize, this.ParentInternal, this, PropertyNames.DrawMode);
					}
				}
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06002069 RID: 8297 RVA: 0x00045082 File Offset: 0x00044082
		internal int FocusedIndex
		{
			get
			{
				if (base.IsHandleCreated)
				{
					return (int)base.SendMessage(415, 0, 0);
				}
				return -1;
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x0600206A RID: 8298 RVA: 0x000450A0 File Offset: 0x000440A0
		// (set) Token: 0x0600206B RID: 8299 RVA: 0x000450A8 File Offset: 0x000440A8
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
				if (!this.integralHeight)
				{
					this.RefreshItems();
				}
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x0600206C RID: 8300 RVA: 0x000450BF File Offset: 0x000440BF
		// (set) Token: 0x0600206D RID: 8301 RVA: 0x000450D5 File Offset: 0x000440D5
		public override Color ForeColor
		{
			get
			{
				if (this.ShouldSerializeForeColor())
				{
					return base.ForeColor;
				}
				return SystemColors.WindowText;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x0600206E RID: 8302 RVA: 0x000450DE File Offset: 0x000440DE
		// (set) Token: 0x0600206F RID: 8303 RVA: 0x000450E6 File Offset: 0x000440E6
		[SRCategory("CatBehavior")]
		[SRDescription("ListBoxHorizontalExtentDescr")]
		[DefaultValue(0)]
		[Localizable(true)]
		public int HorizontalExtent
		{
			get
			{
				return this.horizontalExtent;
			}
			set
			{
				if (value != this.horizontalExtent)
				{
					this.horizontalExtent = value;
					this.UpdateHorizontalExtent();
				}
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06002070 RID: 8304 RVA: 0x000450FE File Offset: 0x000440FE
		// (set) Token: 0x06002071 RID: 8305 RVA: 0x00045106 File Offset: 0x00044106
		[SRDescription("ListBoxHorizontalScrollbarDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[Localizable(true)]
		public bool HorizontalScrollbar
		{
			get
			{
				return this.horizontalScrollbar;
			}
			set
			{
				if (value != this.horizontalScrollbar)
				{
					this.horizontalScrollbar = value;
					this.RefreshItems();
					if (!this.MultiColumn)
					{
						base.RecreateHandle();
					}
				}
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06002072 RID: 8306 RVA: 0x0004512C File Offset: 0x0004412C
		// (set) Token: 0x06002073 RID: 8307 RVA: 0x00045134 File Offset: 0x00044134
		[SRDescription("ListBoxIntegralHeightDescr")]
		[Localizable(true)]
		[DefaultValue(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRCategory("CatBehavior")]
		public bool IntegralHeight
		{
			get
			{
				return this.integralHeight;
			}
			set
			{
				if (this.integralHeight != value)
				{
					this.integralHeight = value;
					base.RecreateHandle();
					this.integralHeightAdjust = true;
					try
					{
						base.Height = this.requestedHeight;
					}
					finally
					{
						this.integralHeightAdjust = false;
					}
				}
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06002074 RID: 8308 RVA: 0x00045184 File Offset: 0x00044184
		// (set) Token: 0x06002075 RID: 8309 RVA: 0x000451A8 File Offset: 0x000441A8
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRCategory("CatBehavior")]
		[DefaultValue(13)]
		[Localizable(true)]
		[SRDescription("ListBoxItemHeightDescr")]
		public virtual int ItemHeight
		{
			get
			{
				if (this.drawMode == DrawMode.OwnerDrawFixed || this.drawMode == DrawMode.OwnerDrawVariable)
				{
					return this.itemHeight;
				}
				return this.GetItemHeight(0);
			}
			set
			{
				if (value < 1 || value > 255)
				{
					throw new ArgumentOutOfRangeException("ItemHeight", SR.GetString("InvalidExBoundArgument", new object[]
					{
						"ItemHeight",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture),
						"256"
					}));
				}
				if (this.itemHeight != value)
				{
					this.itemHeight = value;
					if (this.drawMode == DrawMode.OwnerDrawFixed && base.IsHandleCreated)
					{
						this.BeginUpdate();
						base.SendMessage(416, 0, value);
						if (this.IntegralHeight)
						{
							Size size = base.Size;
							base.Size = new Size(size.Width + 1, size.Height);
							base.Size = size;
						}
						this.EndUpdate();
					}
				}
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06002076 RID: 8310 RVA: 0x0004527A File Offset: 0x0004427A
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		[SRDescription("ListBoxItemsDescr")]
		[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[MergableProperty(false)]
		[SRCategory("CatData")]
		public ListBox.ObjectCollection Items
		{
			get
			{
				if (this.itemsCollection == null)
				{
					this.itemsCollection = this.CreateItemCollection();
				}
				return this.itemsCollection;
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06002077 RID: 8311 RVA: 0x00045298 File Offset: 0x00044298
		internal virtual int MaxItemWidth
		{
			get
			{
				if (this.horizontalExtent > 0)
				{
					return this.horizontalExtent;
				}
				if (this.DrawMode != DrawMode.Normal)
				{
					return -1;
				}
				if (this.maxWidth > -1)
				{
					return this.maxWidth;
				}
				this.maxWidth = this.ComputeMaxItemWidth(this.maxWidth);
				return this.maxWidth;
			}
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06002078 RID: 8312 RVA: 0x000452E7 File Offset: 0x000442E7
		// (set) Token: 0x06002079 RID: 8313 RVA: 0x000452EF File Offset: 0x000442EF
		[SRDescription("ListBoxMultiColumnDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		public bool MultiColumn
		{
			get
			{
				return this.multiColumn;
			}
			set
			{
				if (this.multiColumn != value)
				{
					if (value && this.drawMode == DrawMode.OwnerDrawVariable)
					{
						throw new ArgumentException(SR.GetString("ListBoxVarHeightMultiCol"), "value");
					}
					this.multiColumn = value;
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x0600207A RID: 8314 RVA: 0x00045328 File Offset: 0x00044328
		[SRDescription("ListBoxPreferredHeightDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int PreferredHeight
		{
			get
			{
				int num = 0;
				if (this.drawMode == DrawMode.OwnerDrawVariable)
				{
					if (base.RecreatingHandle || base.GetState(262144))
					{
						num = base.Height;
					}
					else if (this.itemsCollection != null)
					{
						int count = this.itemsCollection.Count;
						for (int i = 0; i < count; i++)
						{
							num += this.GetItemHeight(i);
						}
					}
				}
				else
				{
					int num2 = ((this.itemsCollection == null || this.itemsCollection.Count == 0) ? 1 : this.itemsCollection.Count);
					num = this.GetItemHeight(0) * num2;
				}
				if (this.borderStyle != BorderStyle.None)
				{
					num += SystemInformation.BorderSize.Height * 4 + 3;
				}
				return num;
			}
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x000453D8 File Offset: 0x000443D8
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			int preferredHeight = this.PreferredHeight;
			if (base.IsHandleCreated)
			{
				int num = this.SizeFromClientSize(new Size(this.MaxItemWidth, preferredHeight)).Width;
				num += SystemInformation.VerticalScrollBarWidth + 4;
				return new Size(num, preferredHeight) + this.Padding.Size;
			}
			return this.DefaultSize;
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x0600207C RID: 8316 RVA: 0x0004543C File Offset: 0x0004443C
		// (set) Token: 0x0600207D RID: 8317 RVA: 0x0004544D File Offset: 0x0004444D
		public override RightToLeft RightToLeft
		{
			get
			{
				if (!ListBox.RunningOnWin2K)
				{
					return RightToLeft.No;
				}
				return base.RightToLeft;
			}
			set
			{
				base.RightToLeft = value;
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x0600207E RID: 8318 RVA: 0x00045456 File Offset: 0x00044456
		private static bool RunningOnWin2K
		{
			get
			{
				if (!ListBox.checkedOS && (Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version.Major < 5))
				{
					ListBox.runningOnWin2K = false;
					ListBox.checkedOS = true;
				}
				return ListBox.runningOnWin2K;
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x0600207F RID: 8319 RVA: 0x0004548F File Offset: 0x0004448F
		// (set) Token: 0x06002080 RID: 8320 RVA: 0x00045497 File Offset: 0x00044497
		[SRCategory("CatBehavior")]
		[SRDescription("ListBoxScrollIsVisibleDescr")]
		[DefaultValue(false)]
		[Localizable(true)]
		public bool ScrollAlwaysVisible
		{
			get
			{
				return this.scrollAlwaysVisible;
			}
			set
			{
				if (this.scrollAlwaysVisible != value)
				{
					this.scrollAlwaysVisible = value;
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06002081 RID: 8321 RVA: 0x000454AF File Offset: 0x000444AF
		protected override bool AllowSelection
		{
			get
			{
				return this.selectionMode != SelectionMode.None;
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06002082 RID: 8322 RVA: 0x000454C0 File Offset: 0x000444C0
		// (set) Token: 0x06002083 RID: 8323 RVA: 0x00045538 File Offset: 0x00044538
		[SRDescription("ListBoxSelectedIndexDescr")]
		[Browsable(false)]
		[Bindable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override int SelectedIndex
		{
			get
			{
				SelectionMode selectionMode = (this.selectionModeChanging ? this.cachedSelectionMode : this.selectionMode);
				if (selectionMode == SelectionMode.None)
				{
					return -1;
				}
				if (selectionMode == SelectionMode.One && base.IsHandleCreated)
				{
					return (int)base.SendMessage(392, 0, 0);
				}
				if (this.itemsCollection != null && this.SelectedItems.Count > 0)
				{
					return this.Items.IndexOfIdentifier(this.SelectedItems.GetObjectAt(0));
				}
				return -1;
			}
			set
			{
				int num = ((this.itemsCollection == null) ? 0 : this.itemsCollection.Count);
				if (value < -1 || value >= num)
				{
					throw new ArgumentOutOfRangeException("SelectedIndex", SR.GetString("InvalidArgument", new object[]
					{
						"SelectedIndex",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.selectionMode == SelectionMode.None)
				{
					throw new ArgumentException(SR.GetString("ListBoxInvalidSelectionMode"), "SelectedIndex");
				}
				if (this.selectionMode == SelectionMode.One && value != -1)
				{
					int selectedIndex = this.SelectedIndex;
					if (selectedIndex != value)
					{
						if (selectedIndex != -1)
						{
							this.SelectedItems.SetSelected(selectedIndex, false);
						}
						this.SelectedItems.SetSelected(value, true);
						if (base.IsHandleCreated)
						{
							this.NativeSetSelected(value, true);
						}
						this.OnSelectedIndexChanged(EventArgs.Empty);
						return;
					}
				}
				else if (value == -1)
				{
					if (this.SelectedIndex != -1)
					{
						this.ClearSelected();
						return;
					}
				}
				else if (!this.SelectedItems.GetSelected(value))
				{
					this.SelectedItems.SetSelected(value, true);
					if (base.IsHandleCreated)
					{
						this.NativeSetSelected(value, true);
					}
					this.OnSelectedIndexChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06002084 RID: 8324 RVA: 0x00045654 File Offset: 0x00044654
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ListBoxSelectedIndicesDescr")]
		[Browsable(false)]
		public ListBox.SelectedIndexCollection SelectedIndices
		{
			get
			{
				if (this.selectedIndices == null)
				{
					this.selectedIndices = new ListBox.SelectedIndexCollection(this);
				}
				return this.selectedIndices;
			}
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06002085 RID: 8325 RVA: 0x00045670 File Offset: 0x00044670
		// (set) Token: 0x06002086 RID: 8326 RVA: 0x00045690 File Offset: 0x00044690
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ListBoxSelectedItemDescr")]
		[Browsable(false)]
		[Bindable(true)]
		public object SelectedItem
		{
			get
			{
				if (this.SelectedItems.Count > 0)
				{
					return this.SelectedItems[0];
				}
				return null;
			}
			set
			{
				if (this.itemsCollection != null)
				{
					if (value != null)
					{
						int num = this.itemsCollection.IndexOf(value);
						if (num != -1)
						{
							this.SelectedIndex = num;
							return;
						}
					}
					else
					{
						this.SelectedIndex = -1;
					}
				}
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06002087 RID: 8327 RVA: 0x000456C8 File Offset: 0x000446C8
		[SRDescription("ListBoxSelectedItemsDescr")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ListBox.SelectedObjectCollection SelectedItems
		{
			get
			{
				if (this.selectedItems == null)
				{
					this.selectedItems = new ListBox.SelectedObjectCollection(this);
				}
				return this.selectedItems;
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06002088 RID: 8328 RVA: 0x000456E4 File Offset: 0x000446E4
		// (set) Token: 0x06002089 RID: 8329 RVA: 0x000456EC File Offset: 0x000446EC
		[DefaultValue(SelectionMode.One)]
		[SRCategory("CatBehavior")]
		[SRDescription("ListBoxSelectionModeDescr")]
		public virtual SelectionMode SelectionMode
		{
			get
			{
				return this.selectionMode;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(SelectionMode));
				}
				if (this.selectionMode != value)
				{
					this.SelectedItems.EnsureUpToDate();
					this.selectionMode = value;
					try
					{
						this.selectionModeChanging = true;
						base.RecreateHandle();
					}
					finally
					{
						this.selectionModeChanging = false;
						this.cachedSelectionMode = this.selectionMode;
						if (base.IsHandleCreated)
						{
							this.NativeUpdateSelection();
						}
					}
				}
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x0600208A RID: 8330 RVA: 0x0004577C File Offset: 0x0004477C
		// (set) Token: 0x0600208B RID: 8331 RVA: 0x00045784 File Offset: 0x00044784
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ListBoxSortedDescr")]
		public bool Sorted
		{
			get
			{
				return this.sorted;
			}
			set
			{
				if (this.sorted != value)
				{
					this.sorted = value;
					if (this.sorted && this.itemsCollection != null && this.itemsCollection.Count >= 1)
					{
						this.Sort();
					}
				}
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x0600208C RID: 8332 RVA: 0x000457BA File Offset: 0x000447BA
		// (set) Token: 0x0600208D RID: 8333 RVA: 0x000457FC File Offset: 0x000447FC
		[Bindable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public override string Text
		{
			get
			{
				if (this.SelectionMode == SelectionMode.None || this.SelectedItem == null)
				{
					return base.Text;
				}
				if (base.FormattingEnabled)
				{
					return base.GetItemText(this.SelectedItem);
				}
				return base.FilterItemOnProperty(this.SelectedItem).ToString();
			}
			set
			{
				base.Text = value;
				if (this.SelectionMode != SelectionMode.None && value != null && (this.SelectedItem == null || !value.Equals(base.GetItemText(this.SelectedItem))))
				{
					int count = this.Items.Count;
					for (int i = 0; i < count; i++)
					{
						if (string.Compare(value, base.GetItemText(this.Items[i]), true, CultureInfo.CurrentCulture) == 0)
						{
							this.SelectedIndex = i;
							return;
						}
					}
				}
			}
		}

		// Token: 0x140000CB RID: 203
		// (add) Token: 0x0600208E RID: 8334 RVA: 0x00045877 File Offset: 0x00044877
		// (remove) Token: 0x0600208F RID: 8335 RVA: 0x00045880 File Offset: 0x00044880
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public new event EventHandler TextChanged
		{
			add
			{
				base.TextChanged += value;
			}
			remove
			{
				base.TextChanged -= value;
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06002090 RID: 8336 RVA: 0x00045889 File Offset: 0x00044889
		// (set) Token: 0x06002091 RID: 8337 RVA: 0x000458AC File Offset: 0x000448AC
		[SRDescription("ListBoxTopIndexDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public int TopIndex
		{
			get
			{
				if (base.IsHandleCreated)
				{
					return (int)base.SendMessage(398, 0, 0);
				}
				return this.topIndex;
			}
			set
			{
				if (base.IsHandleCreated)
				{
					base.SendMessage(407, value, 0);
					return;
				}
				this.topIndex = value;
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06002092 RID: 8338 RVA: 0x000458CC File Offset: 0x000448CC
		// (set) Token: 0x06002093 RID: 8339 RVA: 0x000458D4 File Offset: 0x000448D4
		[DefaultValue(true)]
		[SRCategory("CatBehavior")]
		[SRDescription("ListBoxUseTabStopsDescr")]
		public bool UseTabStops
		{
			get
			{
				return this.useTabStops;
			}
			set
			{
				if (this.useTabStops != value)
				{
					this.useTabStops = value;
					base.RecreateHandle();
				}
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06002094 RID: 8340 RVA: 0x000458EC File Offset: 0x000448EC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Browsable(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("ListBoxCustomTabOffsetsDescr")]
		public ListBox.IntegerCollection CustomTabOffsets
		{
			get
			{
				if (this.customTabOffsets == null)
				{
					this.customTabOffsets = new ListBox.IntegerCollection(this);
				}
				return this.customTabOffsets;
			}
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x00045908 File Offset: 0x00044908
		[Obsolete("This method has been deprecated.  There is no replacement.  http://go.microsoft.com/fwlink/?linkid=14202")]
		protected virtual void AddItemsCore(object[] value)
		{
			if (value == null || value.Length == 0)
			{
				return;
			}
			this.Items.AddRangeInternal(value);
		}

		// Token: 0x140000CC RID: 204
		// (add) Token: 0x06002096 RID: 8342 RVA: 0x0004592F File Offset: 0x0004492F
		// (remove) Token: 0x06002097 RID: 8343 RVA: 0x00045938 File Offset: 0x00044938
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		public new event EventHandler Click
		{
			add
			{
				base.Click += value;
			}
			remove
			{
				base.Click -= value;
			}
		}

		// Token: 0x140000CD RID: 205
		// (add) Token: 0x06002098 RID: 8344 RVA: 0x00045941 File Offset: 0x00044941
		// (remove) Token: 0x06002099 RID: 8345 RVA: 0x0004594A File Offset: 0x0004494A
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public new event MouseEventHandler MouseClick
		{
			add
			{
				base.MouseClick += value;
			}
			remove
			{
				base.MouseClick -= value;
			}
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x0600209A RID: 8346 RVA: 0x00045953 File Offset: 0x00044953
		// (set) Token: 0x0600209B RID: 8347 RVA: 0x0004595B File Offset: 0x0004495B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Padding Padding
		{
			get
			{
				return base.Padding;
			}
			set
			{
				base.Padding = value;
			}
		}

		// Token: 0x140000CE RID: 206
		// (add) Token: 0x0600209C RID: 8348 RVA: 0x00045964 File Offset: 0x00044964
		// (remove) Token: 0x0600209D RID: 8349 RVA: 0x0004596D File Offset: 0x0004496D
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler PaddingChanged
		{
			add
			{
				base.PaddingChanged += value;
			}
			remove
			{
				base.PaddingChanged -= value;
			}
		}

		// Token: 0x140000CF RID: 207
		// (add) Token: 0x0600209E RID: 8350 RVA: 0x00045976 File Offset: 0x00044976
		// (remove) Token: 0x0600209F RID: 8351 RVA: 0x0004597F File Offset: 0x0004497F
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event PaintEventHandler Paint
		{
			add
			{
				base.Paint += value;
			}
			remove
			{
				base.Paint -= value;
			}
		}

		// Token: 0x140000D0 RID: 208
		// (add) Token: 0x060020A0 RID: 8352 RVA: 0x00045988 File Offset: 0x00044988
		// (remove) Token: 0x060020A1 RID: 8353 RVA: 0x0004599B File Offset: 0x0004499B
		[SRCategory("CatBehavior")]
		[SRDescription("drawItemEventDescr")]
		public event DrawItemEventHandler DrawItem
		{
			add
			{
				base.Events.AddHandler(ListBox.EVENT_DRAWITEM, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListBox.EVENT_DRAWITEM, value);
			}
		}

		// Token: 0x140000D1 RID: 209
		// (add) Token: 0x060020A2 RID: 8354 RVA: 0x000459AE File Offset: 0x000449AE
		// (remove) Token: 0x060020A3 RID: 8355 RVA: 0x000459C1 File Offset: 0x000449C1
		[SRDescription("measureItemEventDescr")]
		[SRCategory("CatBehavior")]
		public event MeasureItemEventHandler MeasureItem
		{
			add
			{
				base.Events.AddHandler(ListBox.EVENT_MEASUREITEM, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListBox.EVENT_MEASUREITEM, value);
			}
		}

		// Token: 0x140000D2 RID: 210
		// (add) Token: 0x060020A4 RID: 8356 RVA: 0x000459D4 File Offset: 0x000449D4
		// (remove) Token: 0x060020A5 RID: 8357 RVA: 0x000459E7 File Offset: 0x000449E7
		[SRDescription("selectedIndexChangedEventDescr")]
		[SRCategory("CatBehavior")]
		public event EventHandler SelectedIndexChanged
		{
			add
			{
				base.Events.AddHandler(ListBox.EVENT_SELECTEDINDEXCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListBox.EVENT_SELECTEDINDEXCHANGED, value);
			}
		}

		// Token: 0x060020A6 RID: 8358 RVA: 0x000459FA File Offset: 0x000449FA
		public void BeginUpdate()
		{
			base.BeginUpdateInternal();
			this.updateCount++;
		}

		// Token: 0x060020A7 RID: 8359 RVA: 0x00045A10 File Offset: 0x00044A10
		private void CheckIndex(int index)
		{
			if (index < 0 || index >= this.Items.Count)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("IndexOutOfRange", new object[] { index.ToString(CultureInfo.CurrentCulture) }));
			}
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x00045A5B File Offset: 0x00044A5B
		private void CheckNoDataSource()
		{
			if (base.DataSource != null)
			{
				throw new ArgumentException(SR.GetString("DataSourceLocksItems"));
			}
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x00045A75 File Offset: 0x00044A75
		protected virtual ListBox.ObjectCollection CreateItemCollection()
		{
			return new ListBox.ObjectCollection(this);
		}

		// Token: 0x060020AA RID: 8362 RVA: 0x00045A80 File Offset: 0x00044A80
		internal virtual int ComputeMaxItemWidth(int oldMax)
		{
			string[] array = new string[this.Items.Count];
			for (int i = 0; i < this.Items.Count; i++)
			{
				array[i] = base.GetItemText(this.Items[i]);
			}
			return Math.Max(oldMax, LayoutUtils.OldGetLargestStringSizeInCollection(this.Font, array).Width);
		}

		// Token: 0x060020AB RID: 8363 RVA: 0x00045AE4 File Offset: 0x00044AE4
		public void ClearSelected()
		{
			bool flag = false;
			int num = ((this.itemsCollection == null) ? 0 : this.itemsCollection.Count);
			for (int i = 0; i < num; i++)
			{
				if (this.SelectedItems.GetSelected(i))
				{
					flag = true;
					this.SelectedItems.SetSelected(i, false);
					if (base.IsHandleCreated)
					{
						this.NativeSetSelected(i, false);
					}
				}
			}
			if (flag)
			{
				this.OnSelectedIndexChanged(EventArgs.Empty);
			}
		}

		// Token: 0x060020AC RID: 8364 RVA: 0x00045B51 File Offset: 0x00044B51
		public void EndUpdate()
		{
			base.EndUpdateInternal();
			this.updateCount--;
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x00045B68 File Offset: 0x00044B68
		public int FindString(string s)
		{
			return this.FindString(s, -1);
		}

		// Token: 0x060020AE RID: 8366 RVA: 0x00045B74 File Offset: 0x00044B74
		public int FindString(string s, int startIndex)
		{
			if (s == null)
			{
				return -1;
			}
			int num = ((this.itemsCollection == null) ? 0 : this.itemsCollection.Count);
			if (num == 0)
			{
				return -1;
			}
			if (startIndex < -1 || startIndex >= num)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			return base.FindStringInternal(s, this.Items, startIndex, false);
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x00045BC4 File Offset: 0x00044BC4
		public int FindStringExact(string s)
		{
			return this.FindStringExact(s, -1);
		}

		// Token: 0x060020B0 RID: 8368 RVA: 0x00045BD0 File Offset: 0x00044BD0
		public int FindStringExact(string s, int startIndex)
		{
			if (s == null)
			{
				return -1;
			}
			int num = ((this.itemsCollection == null) ? 0 : this.itemsCollection.Count);
			if (num == 0)
			{
				return -1;
			}
			if (startIndex < -1 || startIndex >= num)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			return base.FindStringInternal(s, this.Items, startIndex, true);
		}

		// Token: 0x060020B1 RID: 8369 RVA: 0x00045C20 File Offset: 0x00044C20
		public int GetItemHeight(int index)
		{
			int num = ((this.itemsCollection == null) ? 0 : this.itemsCollection.Count);
			if (index < 0 || (index > 0 && index >= num))
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (this.drawMode != DrawMode.OwnerDrawVariable)
			{
				index = 0;
			}
			if (!base.IsHandleCreated)
			{
				return this.itemHeight;
			}
			int num2 = (int)base.SendMessage(417, index, 0);
			if (num2 == -1)
			{
				throw new Win32Exception();
			}
			return num2;
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x00045CC0 File Offset: 0x00044CC0
		public Rectangle GetItemRectangle(int index)
		{
			this.CheckIndex(index);
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			base.SendMessage(408, index, ref rect);
			return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
		}

		// Token: 0x060020B3 RID: 8371 RVA: 0x00045D0C File Offset: 0x00044D0C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override Rectangle GetScaledBounds(Rectangle bounds, SizeF factor, BoundsSpecified specified)
		{
			bounds.Height = this.requestedHeight;
			return base.GetScaledBounds(bounds, factor, specified);
		}

		// Token: 0x060020B4 RID: 8372 RVA: 0x00045D24 File Offset: 0x00044D24
		public bool GetSelected(int index)
		{
			this.CheckIndex(index);
			return this.GetSelectedInternal(index);
		}

		// Token: 0x060020B5 RID: 8373 RVA: 0x00045D34 File Offset: 0x00044D34
		private bool GetSelectedInternal(int index)
		{
			if (!base.IsHandleCreated)
			{
				return this.itemsCollection != null && this.SelectedItems.GetSelected(index);
			}
			int num = (int)base.SendMessage(391, index, 0);
			if (num == -1)
			{
				throw new Win32Exception();
			}
			return num > 0;
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x00045D84 File Offset: 0x00044D84
		public int IndexFromPoint(Point p)
		{
			return this.IndexFromPoint(p.X, p.Y);
		}

		// Token: 0x060020B7 RID: 8375 RVA: 0x00045D9C File Offset: 0x00044D9C
		public int IndexFromPoint(int x, int y)
		{
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			UnsafeNativeMethods.GetClientRect(new HandleRef(this, base.Handle), ref rect);
			if (rect.left <= x && x < rect.right && rect.top <= y && y < rect.bottom)
			{
				int num = (int)base.SendMessage(425, 0, (int)NativeMethods.Util.MAKELPARAM(x, y));
				if (NativeMethods.Util.HIWORD(num) == 0)
				{
					return NativeMethods.Util.LOWORD(num);
				}
			}
			return -1;
		}

		// Token: 0x060020B8 RID: 8376 RVA: 0x00045E1C File Offset: 0x00044E1C
		private int NativeAdd(object item)
		{
			int num = (int)base.SendMessage(384, 0, base.GetItemText(item));
			if (num == -2)
			{
				throw new OutOfMemoryException();
			}
			if (num == -1)
			{
				throw new OutOfMemoryException(SR.GetString("ListBoxItemOverflow"));
			}
			return num;
		}

		// Token: 0x060020B9 RID: 8377 RVA: 0x00045E62 File Offset: 0x00044E62
		private void NativeClear()
		{
			base.SendMessage(388, 0, 0);
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x00045E74 File Offset: 0x00044E74
		internal string NativeGetItemText(int index)
		{
			int num = (int)base.SendMessage(394, index, 0);
			StringBuilder stringBuilder = new StringBuilder(num + 1);
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 393, index, stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x00045EBC File Offset: 0x00044EBC
		private int NativeInsert(int index, object item)
		{
			int num = (int)base.SendMessage(385, index, base.GetItemText(item));
			if (num == -2)
			{
				throw new OutOfMemoryException();
			}
			if (num == -1)
			{
				throw new OutOfMemoryException(SR.GetString("ListBoxItemOverflow"));
			}
			return num;
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x00045F04 File Offset: 0x00044F04
		private void NativeRemoveAt(int index)
		{
			bool flag = (int)base.SendMessage(391, (IntPtr)index, IntPtr.Zero) > 0;
			base.SendMessage(386, index, 0);
			if (flag)
			{
				this.OnSelectedIndexChanged(EventArgs.Empty);
			}
		}

		// Token: 0x060020BD RID: 8381 RVA: 0x00045F4C File Offset: 0x00044F4C
		private void NativeSetSelected(int index, bool value)
		{
			if (this.selectionMode == SelectionMode.One)
			{
				base.SendMessage(390, value ? index : (-1), 0);
				return;
			}
			base.SendMessage(389, value ? (-1) : 0, index);
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x00045F80 File Offset: 0x00044F80
		private void NativeUpdateSelection()
		{
			int count = this.Items.Count;
			for (int i = 0; i < count; i++)
			{
				this.SelectedItems.SetSelected(i, false);
			}
			int[] array = null;
			switch (this.selectionMode)
			{
			case SelectionMode.One:
			{
				int num = (int)base.SendMessage(392, 0, 0);
				if (num >= 0)
				{
					array = new int[] { num };
				}
				break;
			}
			case SelectionMode.MultiSimple:
			case SelectionMode.MultiExtended:
			{
				int num2 = (int)base.SendMessage(400, 0, 0);
				if (num2 > 0)
				{
					array = new int[num2];
					UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 401, num2, array);
				}
				break;
			}
			}
			if (array != null)
			{
				foreach (int num3 in array)
				{
					this.SelectedItems.SetSelected(num3, true);
				}
			}
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x00046065 File Offset: 0x00045065
		protected override void OnChangeUICues(UICuesEventArgs e)
		{
			base.Invalidate();
			base.OnChangeUICues(e);
		}

		// Token: 0x060020C0 RID: 8384 RVA: 0x00046074 File Offset: 0x00045074
		protected virtual void OnDrawItem(DrawItemEventArgs e)
		{
			DrawItemEventHandler drawItemEventHandler = (DrawItemEventHandler)base.Events[ListBox.EVENT_DRAWITEM];
			if (drawItemEventHandler != null)
			{
				drawItemEventHandler(this, e);
			}
		}

		// Token: 0x060020C1 RID: 8385 RVA: 0x000460A4 File Offset: 0x000450A4
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			base.SendMessage(421, CultureInfo.CurrentCulture.LCID, 0);
			if (this.columnWidth != 0)
			{
				base.SendMessage(405, this.columnWidth, 0);
			}
			if (this.drawMode == DrawMode.OwnerDrawFixed)
			{
				base.SendMessage(416, 0, this.ItemHeight);
			}
			if (this.topIndex != 0)
			{
				base.SendMessage(407, this.topIndex, 0);
			}
			if (this.UseCustomTabOffsets && this.CustomTabOffsets != null)
			{
				int count = this.CustomTabOffsets.Count;
				int[] array = new int[count];
				this.CustomTabOffsets.CopyTo(array, 0);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 402, count, array);
			}
			if (this.itemsCollection != null)
			{
				int count2 = this.itemsCollection.Count;
				for (int i = 0; i < count2; i++)
				{
					this.NativeAdd(this.itemsCollection[i]);
					if (this.selectionMode != SelectionMode.None && this.selectedItems != null)
					{
						this.selectedItems.PushSelectionIntoNativeListBox(i);
					}
				}
			}
			if (this.selectedItems != null && this.selectedItems.Count > 0 && this.selectionMode == SelectionMode.One)
			{
				this.SelectedItems.Dirty();
				this.SelectedItems.EnsureUpToDate();
			}
			this.UpdateHorizontalExtent();
		}

		// Token: 0x060020C2 RID: 8386 RVA: 0x000461F4 File Offset: 0x000451F4
		protected override void OnHandleDestroyed(EventArgs e)
		{
			this.SelectedItems.EnsureUpToDate();
			if (base.Disposing)
			{
				this.itemsCollection = null;
			}
			base.OnHandleDestroyed(e);
		}

		// Token: 0x060020C3 RID: 8387 RVA: 0x00046218 File Offset: 0x00045218
		protected virtual void OnMeasureItem(MeasureItemEventArgs e)
		{
			MeasureItemEventHandler measureItemEventHandler = (MeasureItemEventHandler)base.Events[ListBox.EVENT_MEASUREITEM];
			if (measureItemEventHandler != null)
			{
				measureItemEventHandler(this, e);
			}
		}

		// Token: 0x060020C4 RID: 8388 RVA: 0x00046246 File Offset: 0x00045246
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.UpdateFontCache();
		}

		// Token: 0x060020C5 RID: 8389 RVA: 0x00046255 File Offset: 0x00045255
		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			if (this.ParentInternal != null)
			{
				base.RecreateHandle();
			}
		}

		// Token: 0x060020C6 RID: 8390 RVA: 0x0004626C File Offset: 0x0004526C
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (this.RightToLeft == RightToLeft.Yes || this.HorizontalScrollbar)
			{
				base.Invalidate();
			}
		}

		// Token: 0x060020C7 RID: 8391 RVA: 0x0004628C File Offset: 0x0004528C
		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			base.OnSelectedIndexChanged(e);
			if (base.DataManager != null && base.DataManager.Position != this.SelectedIndex && (!base.FormattingEnabled || this.SelectedIndex != -1))
			{
				base.DataManager.Position = this.SelectedIndex;
			}
			EventHandler eventHandler = (EventHandler)base.Events[ListBox.EVENT_SELECTEDINDEXCHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060020C8 RID: 8392 RVA: 0x000462FE File Offset: 0x000452FE
		protected override void OnSelectedValueChanged(EventArgs e)
		{
			base.OnSelectedValueChanged(e);
			this.selectedValueChangedFired = true;
		}

		// Token: 0x060020C9 RID: 8393 RVA: 0x0004630E File Offset: 0x0004530E
		protected override void OnDataSourceChanged(EventArgs e)
		{
			if (base.DataSource == null)
			{
				this.BeginUpdate();
				this.SelectedIndex = -1;
				this.Items.ClearInternal();
				this.EndUpdate();
			}
			base.OnDataSourceChanged(e);
			this.RefreshItems();
		}

		// Token: 0x060020CA RID: 8394 RVA: 0x00046343 File Offset: 0x00045343
		protected override void OnDisplayMemberChanged(EventArgs e)
		{
			base.OnDisplayMemberChanged(e);
			this.RefreshItems();
			if (this.SelectionMode != SelectionMode.None && base.DataManager != null)
			{
				this.SelectedIndex = base.DataManager.Position;
			}
		}

		// Token: 0x060020CB RID: 8395 RVA: 0x00046374 File Offset: 0x00045374
		public override void Refresh()
		{
			if (this.drawMode == DrawMode.OwnerDrawVariable)
			{
				int count = this.Items.Count;
				Graphics graphics = base.CreateGraphicsInternal();
				try
				{
					for (int i = 0; i < count; i++)
					{
						MeasureItemEventArgs measureItemEventArgs = new MeasureItemEventArgs(graphics, i, this.ItemHeight);
						this.OnMeasureItem(measureItemEventArgs);
					}
				}
				finally
				{
					graphics.Dispose();
				}
			}
			base.Refresh();
		}

		// Token: 0x060020CC RID: 8396 RVA: 0x000463E0 File Offset: 0x000453E0
		protected override void RefreshItems()
		{
			ListBox.ObjectCollection objectCollection = this.itemsCollection;
			this.itemsCollection = null;
			this.selectedIndices = null;
			if (base.IsHandleCreated)
			{
				this.NativeClear();
			}
			object[] array = null;
			if (base.DataManager != null && base.DataManager.Count != -1)
			{
				array = new object[base.DataManager.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = base.DataManager[i];
				}
			}
			else if (objectCollection != null)
			{
				array = new object[objectCollection.Count];
				objectCollection.CopyTo(array, 0);
			}
			if (array != null)
			{
				this.Items.AddRangeInternal(array);
			}
			if (this.SelectionMode != SelectionMode.None)
			{
				if (base.DataManager != null)
				{
					this.SelectedIndex = base.DataManager.Position;
					return;
				}
				if (objectCollection != null)
				{
					int count = objectCollection.Count;
					for (int j = 0; j < count; j++)
					{
						if (objectCollection.InnerArray.GetState(j, ListBox.SelectedObjectCollection.SelectedObjectMask))
						{
							this.SelectedItem = objectCollection[j];
						}
					}
				}
			}
		}

		// Token: 0x060020CD RID: 8397 RVA: 0x000464DC File Offset: 0x000454DC
		protected override void RefreshItem(int index)
		{
			this.Items.SetItemInternal(index, this.Items[index]);
		}

		// Token: 0x060020CE RID: 8398 RVA: 0x000464F6 File Offset: 0x000454F6
		public override void ResetBackColor()
		{
			base.ResetBackColor();
		}

		// Token: 0x060020CF RID: 8399 RVA: 0x000464FE File Offset: 0x000454FE
		public override void ResetForeColor()
		{
			base.ResetForeColor();
		}

		// Token: 0x060020D0 RID: 8400 RVA: 0x00046506 File Offset: 0x00045506
		private void ResetItemHeight()
		{
			this.itemHeight = 13;
		}

		// Token: 0x060020D1 RID: 8401 RVA: 0x00046510 File Offset: 0x00045510
		protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			if (factor.Width != 1f && factor.Height != 1f)
			{
				this.UpdateFontCache();
			}
			base.ScaleControl(factor, specified);
		}

		// Token: 0x060020D2 RID: 8402 RVA: 0x0004653C File Offset: 0x0004553C
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if (!this.integralHeightAdjust && height != base.Height)
			{
				this.requestedHeight = height;
			}
			base.SetBoundsCore(x, y, width, height, specified);
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x00046568 File Offset: 0x00045568
		protected override void SetItemsCore(IList value)
		{
			this.BeginUpdate();
			this.Items.ClearInternal();
			this.Items.AddRangeInternal(value);
			this.SelectedItems.Dirty();
			if (base.DataManager != null)
			{
				if (base.DataSource is ICurrencyManagerProvider)
				{
					this.selectedValueChangedFired = false;
				}
				if (base.IsHandleCreated)
				{
					base.SendMessage(390, base.DataManager.Position, 0);
				}
				if (!this.selectedValueChangedFired)
				{
					this.OnSelectedValueChanged(EventArgs.Empty);
					this.selectedValueChangedFired = false;
				}
			}
			this.EndUpdate();
		}

		// Token: 0x060020D4 RID: 8404 RVA: 0x000465F9 File Offset: 0x000455F9
		protected override void SetItemCore(int index, object value)
		{
			this.Items.SetItemInternal(index, value);
		}

		// Token: 0x060020D5 RID: 8405 RVA: 0x00046608 File Offset: 0x00045608
		public void SetSelected(int index, bool value)
		{
			int num = ((this.itemsCollection == null) ? 0 : this.itemsCollection.Count);
			if (index < 0 || index >= num)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (this.selectionMode == SelectionMode.None)
			{
				throw new InvalidOperationException(SR.GetString("ListBoxInvalidSelectionMode"));
			}
			this.SelectedItems.SetSelected(index, value);
			if (base.IsHandleCreated)
			{
				this.NativeSetSelected(index, value);
			}
			this.SelectedItems.Dirty();
			this.OnSelectedIndexChanged(EventArgs.Empty);
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x000466B4 File Offset: 0x000456B4
		protected virtual void Sort()
		{
			this.CheckNoDataSource();
			ListBox.SelectedObjectCollection selectedObjectCollection = this.SelectedItems;
			selectedObjectCollection.EnsureUpToDate();
			if (this.sorted && this.itemsCollection != null)
			{
				this.itemsCollection.InnerArray.Sort();
				if (base.IsHandleCreated)
				{
					this.NativeClear();
					int count = this.itemsCollection.Count;
					for (int i = 0; i < count; i++)
					{
						this.NativeAdd(this.itemsCollection[i]);
						if (selectedObjectCollection.GetSelected(i))
						{
							this.NativeSetSelected(i, true);
						}
					}
				}
			}
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x00046740 File Offset: 0x00045740
		public override string ToString()
		{
			string text = base.ToString();
			if (this.itemsCollection != null)
			{
				text = text + ", Items.Count: " + this.Items.Count.ToString(CultureInfo.CurrentCulture);
				if (this.Items.Count > 0)
				{
					string itemText = base.GetItemText(this.Items[0]);
					string text2 = ((itemText.Length > 40) ? itemText.Substring(0, 40) : itemText);
					text = text + ", Items[0]: " + text2;
				}
			}
			return text;
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x000467C8 File Offset: 0x000457C8
		private void UpdateFontCache()
		{
			this.fontIsChanged = true;
			this.integralHeightAdjust = true;
			try
			{
				base.Height = this.requestedHeight;
			}
			finally
			{
				this.integralHeightAdjust = false;
			}
			this.maxWidth = -1;
			this.UpdateHorizontalExtent();
			CommonProperties.xClearPreferredSizeCache(this);
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x0004681C File Offset: 0x0004581C
		private void UpdateHorizontalExtent()
		{
			if (!this.multiColumn && this.horizontalScrollbar && base.IsHandleCreated)
			{
				int maxItemWidth = this.horizontalExtent;
				if (maxItemWidth == 0)
				{
					maxItemWidth = this.MaxItemWidth;
				}
				base.SendMessage(404, maxItemWidth, 0);
			}
		}

		// Token: 0x060020DA RID: 8410 RVA: 0x00046860 File Offset: 0x00045860
		private void UpdateMaxItemWidth(object item, bool removing)
		{
			if (!this.horizontalScrollbar || this.horizontalExtent > 0)
			{
				this.maxWidth = -1;
				return;
			}
			if (this.maxWidth > -1)
			{
				int num;
				using (Graphics graphics = base.CreateGraphicsInternal())
				{
					num = (int)Math.Ceiling((double)graphics.MeasureString(base.GetItemText(item), this.Font).Width);
				}
				if (removing)
				{
					if (num >= this.maxWidth)
					{
						this.maxWidth = -1;
						return;
					}
				}
				else if (num > this.maxWidth)
				{
					this.maxWidth = num;
				}
			}
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x000468FC File Offset: 0x000458FC
		private void UpdateCustomTabOffsets()
		{
			if (base.IsHandleCreated && this.UseCustomTabOffsets && this.CustomTabOffsets != null)
			{
				int count = this.CustomTabOffsets.Count;
				int[] array = new int[count];
				this.CustomTabOffsets.CopyTo(array, 0);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 402, count, array);
				base.Invalidate();
			}
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x00046960 File Offset: 0x00045960
		private void WmPrint(ref Message m)
		{
			base.WndProc(ref m);
			if ((2 & (int)m.LParam) != 0 && Application.RenderWithVisualStyles && this.BorderStyle == BorderStyle.Fixed3D)
			{
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					using (Graphics graphics = Graphics.FromHdc(m.WParam))
					{
						Rectangle rectangle = new Rectangle(0, 0, base.Size.Width - 1, base.Size.Height - 1);
						graphics.DrawRectangle(new Pen(VisualStyleInformation.TextControlBorder), rectangle);
						rectangle.Inflate(-1, -1);
						graphics.DrawRectangle(SystemPens.Window, rectangle);
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x00046A30 File Offset: 0x00045A30
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected virtual void WmReflectCommand(ref Message m)
		{
			switch (NativeMethods.Util.HIWORD(m.WParam))
			{
			case 1:
				if (this.selectedItems != null)
				{
					this.selectedItems.Dirty();
				}
				this.OnSelectedIndexChanged(EventArgs.Empty);
				break;
			case 2:
				break;
			default:
				return;
			}
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x00046A78 File Offset: 0x00045A78
		private void WmReflectDrawItem(ref Message m)
		{
			NativeMethods.DRAWITEMSTRUCT drawitemstruct = (NativeMethods.DRAWITEMSTRUCT)m.GetLParam(typeof(NativeMethods.DRAWITEMSTRUCT));
			IntPtr hDC = drawitemstruct.hDC;
			IntPtr intPtr = Control.SetUpPalette(hDC, false, false);
			try
			{
				Graphics graphics = Graphics.FromHdcInternal(hDC);
				try
				{
					Rectangle rectangle = Rectangle.FromLTRB(drawitemstruct.rcItem.left, drawitemstruct.rcItem.top, drawitemstruct.rcItem.right, drawitemstruct.rcItem.bottom);
					if (this.HorizontalScrollbar)
					{
						if (this.MultiColumn)
						{
							rectangle.Width = Math.Max(this.ColumnWidth, rectangle.Width);
						}
						else
						{
							rectangle.Width = Math.Max(this.MaxItemWidth, rectangle.Width);
						}
					}
					this.OnDrawItem(new DrawItemEventArgs(graphics, this.Font, rectangle, drawitemstruct.itemID, (DrawItemState)drawitemstruct.itemState, this.ForeColor, this.BackColor));
				}
				finally
				{
					graphics.Dispose();
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					SafeNativeMethods.SelectPalette(new HandleRef(null, hDC), new HandleRef(null, intPtr), 0);
				}
			}
			m.Result = (IntPtr)1;
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x00046BAC File Offset: 0x00045BAC
		private void WmReflectMeasureItem(ref Message m)
		{
			NativeMethods.MEASUREITEMSTRUCT measureitemstruct = (NativeMethods.MEASUREITEMSTRUCT)m.GetLParam(typeof(NativeMethods.MEASUREITEMSTRUCT));
			if (this.drawMode == DrawMode.OwnerDrawVariable && measureitemstruct.itemID >= 0)
			{
				Graphics graphics = base.CreateGraphicsInternal();
				MeasureItemEventArgs measureItemEventArgs = new MeasureItemEventArgs(graphics, measureitemstruct.itemID, this.ItemHeight);
				try
				{
					this.OnMeasureItem(measureItemEventArgs);
					measureitemstruct.itemHeight = measureItemEventArgs.ItemHeight;
					goto IL_006A;
				}
				finally
				{
					graphics.Dispose();
				}
			}
			measureitemstruct.itemHeight = this.ItemHeight;
			IL_006A:
			Marshal.StructureToPtr(measureitemstruct, m.LParam, false);
			m.Result = (IntPtr)1;
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x00046C4C File Offset: 0x00045C4C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 517)
			{
				if (msg != 71)
				{
					switch (msg)
					{
					case 513:
						if (this.selectedItems != null)
						{
							this.selectedItems.Dirty();
						}
						base.WndProc(ref m);
						return;
					case 514:
					{
						int num = NativeMethods.Util.SignedLOWORD(m.LParam);
						int num2 = NativeMethods.Util.SignedHIWORD(m.LParam);
						Point point = new Point(num, num2);
						point = base.PointToScreen(point);
						bool capture = base.Capture;
						if (capture && UnsafeNativeMethods.WindowFromPoint(point.X, point.Y) == base.Handle)
						{
							if (!this.doubleClickFired && !base.ValidationCancelled)
							{
								this.OnClick(new MouseEventArgs(MouseButtons.Left, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
								this.OnMouseClick(new MouseEventArgs(MouseButtons.Left, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
							}
							else
							{
								this.doubleClickFired = false;
								if (!base.ValidationCancelled)
								{
									this.OnDoubleClick(new MouseEventArgs(MouseButtons.Left, 2, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
									this.OnMouseDoubleClick(new MouseEventArgs(MouseButtons.Left, 2, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
								}
							}
						}
						if (base.GetState(2048))
						{
							base.DefWndProc(ref m);
						}
						else
						{
							base.WndProc(ref m);
						}
						this.doubleClickFired = false;
						return;
					}
					case 515:
						this.doubleClickFired = true;
						base.WndProc(ref m);
						return;
					case 517:
					{
						int num3 = NativeMethods.Util.SignedLOWORD(m.LParam);
						int num4 = NativeMethods.Util.SignedHIWORD(m.LParam);
						Point point2 = new Point(num3, num4);
						point2 = base.PointToScreen(point2);
						bool capture2 = base.Capture;
						if (capture2 && UnsafeNativeMethods.WindowFromPoint(point2.X, point2.Y) == base.Handle && this.selectedItems != null)
						{
							this.selectedItems.Dirty();
						}
						base.WndProc(ref m);
						return;
					}
					}
				}
				else
				{
					base.WndProc(ref m);
					if (this.integralHeight && this.fontIsChanged)
					{
						base.Height = Math.Max(base.Height, this.ItemHeight);
						this.fontIsChanged = false;
						return;
					}
					return;
				}
			}
			else
			{
				if (msg == 791)
				{
					this.WmPrint(ref m);
					return;
				}
				switch (msg)
				{
				case 8235:
					this.WmReflectDrawItem(ref m);
					return;
				case 8236:
					this.WmReflectMeasureItem(ref m);
					return;
				default:
					if (msg == 8465)
					{
						this.WmReflectCommand(ref m);
						return;
					}
					break;
				}
			}
			base.WndProc(ref m);
		}

		// Token: 0x04001493 RID: 5267
		public const int NoMatches = -1;

		// Token: 0x04001494 RID: 5268
		public const int DefaultItemHeight = 13;

		// Token: 0x04001495 RID: 5269
		private const int maxWin9xHeight = 32767;

		// Token: 0x04001496 RID: 5270
		private static readonly object EVENT_SELECTEDINDEXCHANGED = new object();

		// Token: 0x04001497 RID: 5271
		private static readonly object EVENT_DRAWITEM = new object();

		// Token: 0x04001498 RID: 5272
		private static readonly object EVENT_MEASUREITEM = new object();

		// Token: 0x04001499 RID: 5273
		private static bool checkedOS = false;

		// Token: 0x0400149A RID: 5274
		private static bool runningOnWin2K = true;

		// Token: 0x0400149B RID: 5275
		private ListBox.SelectedObjectCollection selectedItems;

		// Token: 0x0400149C RID: 5276
		private ListBox.SelectedIndexCollection selectedIndices;

		// Token: 0x0400149D RID: 5277
		private ListBox.ObjectCollection itemsCollection;

		// Token: 0x0400149E RID: 5278
		private int itemHeight = 13;

		// Token: 0x0400149F RID: 5279
		private int columnWidth;

		// Token: 0x040014A0 RID: 5280
		private int requestedHeight;

		// Token: 0x040014A1 RID: 5281
		private int topIndex;

		// Token: 0x040014A2 RID: 5282
		private int horizontalExtent;

		// Token: 0x040014A3 RID: 5283
		private int maxWidth = -1;

		// Token: 0x040014A4 RID: 5284
		private int updateCount;

		// Token: 0x040014A5 RID: 5285
		private bool sorted;

		// Token: 0x040014A6 RID: 5286
		private bool scrollAlwaysVisible;

		// Token: 0x040014A7 RID: 5287
		private bool integralHeight = true;

		// Token: 0x040014A8 RID: 5288
		private bool integralHeightAdjust;

		// Token: 0x040014A9 RID: 5289
		private bool multiColumn;

		// Token: 0x040014AA RID: 5290
		private bool horizontalScrollbar;

		// Token: 0x040014AB RID: 5291
		private bool useTabStops = true;

		// Token: 0x040014AC RID: 5292
		private bool useCustomTabOffsets;

		// Token: 0x040014AD RID: 5293
		private bool fontIsChanged;

		// Token: 0x040014AE RID: 5294
		private bool doubleClickFired;

		// Token: 0x040014AF RID: 5295
		private bool selectedValueChangedFired;

		// Token: 0x040014B0 RID: 5296
		private DrawMode drawMode;

		// Token: 0x040014B1 RID: 5297
		private BorderStyle borderStyle = BorderStyle.Fixed3D;

		// Token: 0x040014B2 RID: 5298
		private SelectionMode selectionMode = SelectionMode.One;

		// Token: 0x040014B3 RID: 5299
		private SelectionMode cachedSelectionMode = SelectionMode.One;

		// Token: 0x040014B4 RID: 5300
		private bool selectionModeChanging;

		// Token: 0x040014B5 RID: 5301
		private ListBox.IntegerCollection customTabOffsets;

		// Token: 0x02000267 RID: 615
		internal class ItemArray : IComparer
		{
			// Token: 0x060020E2 RID: 8418 RVA: 0x00046F2B File Offset: 0x00045F2B
			public ItemArray(ListControl listControl)
			{
				this.listControl = listControl;
			}

			// Token: 0x170004CA RID: 1226
			// (get) Token: 0x060020E3 RID: 8419 RVA: 0x00046F3A File Offset: 0x00045F3A
			public int Version
			{
				get
				{
					return this.version;
				}
			}

			// Token: 0x060020E4 RID: 8420 RVA: 0x00046F44 File Offset: 0x00045F44
			public object Add(object item)
			{
				this.EnsureSpace(1);
				this.version++;
				this.entries[this.count] = new ListBox.ItemArray.Entry(item);
				return this.entries[this.count++];
			}

			// Token: 0x060020E5 RID: 8421 RVA: 0x00046F94 File Offset: 0x00045F94
			public void AddRange(ICollection items)
			{
				if (items == null)
				{
					throw new ArgumentNullException("items");
				}
				this.EnsureSpace(items.Count);
				foreach (object obj in items)
				{
					this.entries[this.count++] = new ListBox.ItemArray.Entry(obj);
				}
				this.version++;
			}

			// Token: 0x060020E6 RID: 8422 RVA: 0x00047024 File Offset: 0x00046024
			public void Clear()
			{
				this.count = 0;
				this.version++;
			}

			// Token: 0x060020E7 RID: 8423 RVA: 0x0004703C File Offset: 0x0004603C
			public static int CreateMask()
			{
				int num = ListBox.ItemArray.lastMask;
				ListBox.ItemArray.lastMask <<= 1;
				return num;
			}

			// Token: 0x060020E8 RID: 8424 RVA: 0x0004705C File Offset: 0x0004605C
			private void EnsureSpace(int elements)
			{
				if (this.entries == null)
				{
					this.entries = new ListBox.ItemArray.Entry[Math.Max(elements, 4)];
					return;
				}
				if (this.count + elements >= this.entries.Length)
				{
					int num = Math.Max(this.entries.Length * 2, this.entries.Length + elements);
					ListBox.ItemArray.Entry[] array = new ListBox.ItemArray.Entry[num];
					this.entries.CopyTo(array, 0);
					this.entries = array;
				}
			}

			// Token: 0x060020E9 RID: 8425 RVA: 0x000470CC File Offset: 0x000460CC
			public int GetActualIndex(int virtualIndex, int stateMask)
			{
				if (stateMask == 0)
				{
					return virtualIndex;
				}
				int num = -1;
				for (int i = 0; i < this.count; i++)
				{
					if ((this.entries[i].state & stateMask) != 0)
					{
						num++;
						if (num == virtualIndex)
						{
							return i;
						}
					}
				}
				return -1;
			}

			// Token: 0x060020EA RID: 8426 RVA: 0x00047110 File Offset: 0x00046110
			public int GetCount(int stateMask)
			{
				if (stateMask == 0)
				{
					return this.count;
				}
				int num = 0;
				for (int i = 0; i < this.count; i++)
				{
					if ((this.entries[i].state & stateMask) != 0)
					{
						num++;
					}
				}
				return num;
			}

			// Token: 0x060020EB RID: 8427 RVA: 0x00047150 File Offset: 0x00046150
			public IEnumerator GetEnumerator(int stateMask)
			{
				return this.GetEnumerator(stateMask, false);
			}

			// Token: 0x060020EC RID: 8428 RVA: 0x0004715A File Offset: 0x0004615A
			public IEnumerator GetEnumerator(int stateMask, bool anyBit)
			{
				return new ListBox.ItemArray.EntryEnumerator(this, stateMask, anyBit);
			}

			// Token: 0x060020ED RID: 8429 RVA: 0x00047164 File Offset: 0x00046164
			public object GetItem(int virtualIndex, int stateMask)
			{
				int actualIndex = this.GetActualIndex(virtualIndex, stateMask);
				if (actualIndex == -1)
				{
					throw new IndexOutOfRangeException();
				}
				return this.entries[actualIndex].item;
			}

			// Token: 0x060020EE RID: 8430 RVA: 0x00047194 File Offset: 0x00046194
			internal object GetEntryObject(int virtualIndex, int stateMask)
			{
				int actualIndex = this.GetActualIndex(virtualIndex, stateMask);
				if (actualIndex == -1)
				{
					throw new IndexOutOfRangeException();
				}
				return this.entries[actualIndex];
			}

			// Token: 0x060020EF RID: 8431 RVA: 0x000471BC File Offset: 0x000461BC
			public bool GetState(int index, int stateMask)
			{
				return (this.entries[index].state & stateMask) == stateMask;
			}

			// Token: 0x060020F0 RID: 8432 RVA: 0x000471D0 File Offset: 0x000461D0
			public int IndexOf(object item, int stateMask)
			{
				int num = -1;
				for (int i = 0; i < this.count; i++)
				{
					if (stateMask == 0 || (this.entries[i].state & stateMask) != 0)
					{
						num++;
						if (this.entries[i].item.Equals(item))
						{
							return num;
						}
					}
				}
				return -1;
			}

			// Token: 0x060020F1 RID: 8433 RVA: 0x00047220 File Offset: 0x00046220
			public int IndexOfIdentifier(object identifier, int stateMask)
			{
				int num = -1;
				for (int i = 0; i < this.count; i++)
				{
					if (stateMask == 0 || (this.entries[i].state & stateMask) != 0)
					{
						num++;
						if (this.entries[i] == identifier)
						{
							return num;
						}
					}
				}
				return -1;
			}

			// Token: 0x060020F2 RID: 8434 RVA: 0x00047268 File Offset: 0x00046268
			public void Insert(int index, object item)
			{
				this.EnsureSpace(1);
				if (index < this.count)
				{
					Array.Copy(this.entries, index, this.entries, index + 1, this.count - index);
				}
				this.entries[index] = new ListBox.ItemArray.Entry(item);
				this.count++;
				this.version++;
			}

			// Token: 0x060020F3 RID: 8435 RVA: 0x000472CC File Offset: 0x000462CC
			public void Remove(object item)
			{
				int num = this.IndexOf(item, 0);
				if (num != -1)
				{
					this.RemoveAt(num);
				}
			}

			// Token: 0x060020F4 RID: 8436 RVA: 0x000472F0 File Offset: 0x000462F0
			public void RemoveAt(int index)
			{
				this.count--;
				for (int i = index; i < this.count; i++)
				{
					this.entries[i] = this.entries[i + 1];
				}
				this.entries[this.count] = null;
				this.version++;
			}

			// Token: 0x060020F5 RID: 8437 RVA: 0x0004734A File Offset: 0x0004634A
			public void SetItem(int index, object item)
			{
				this.entries[index].item = item;
			}

			// Token: 0x060020F6 RID: 8438 RVA: 0x0004735A File Offset: 0x0004635A
			public void SetState(int index, int stateMask, bool value)
			{
				if (value)
				{
					this.entries[index].state |= stateMask;
				}
				else
				{
					this.entries[index].state &= ~stateMask;
				}
				this.version++;
			}

			// Token: 0x060020F7 RID: 8439 RVA: 0x0004739A File Offset: 0x0004639A
			public int BinarySearch(object element)
			{
				return Array.BinarySearch(this.entries, 0, this.count, element, this);
			}

			// Token: 0x060020F8 RID: 8440 RVA: 0x000473B0 File Offset: 0x000463B0
			public void Sort()
			{
				Array.Sort(this.entries, 0, this.count, this);
			}

			// Token: 0x060020F9 RID: 8441 RVA: 0x000473C5 File Offset: 0x000463C5
			public void Sort(Array externalArray)
			{
				Array.Sort(externalArray, this);
			}

			// Token: 0x060020FA RID: 8442 RVA: 0x000473D0 File Offset: 0x000463D0
			int IComparer.Compare(object item1, object item2)
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
					if (item1 is ListBox.ItemArray.Entry)
					{
						item1 = ((ListBox.ItemArray.Entry)item1).item;
					}
					if (item2 is ListBox.ItemArray.Entry)
					{
						item2 = ((ListBox.ItemArray.Entry)item2).item;
					}
					string itemText = this.listControl.GetItemText(item1);
					string itemText2 = this.listControl.GetItemText(item2);
					CompareInfo compareInfo = Application.CurrentCulture.CompareInfo;
					return compareInfo.Compare(itemText, itemText2, CompareOptions.StringSort);
				}
			}

			// Token: 0x040014B6 RID: 5302
			private static int lastMask = 1;

			// Token: 0x040014B7 RID: 5303
			private ListControl listControl;

			// Token: 0x040014B8 RID: 5304
			private ListBox.ItemArray.Entry[] entries;

			// Token: 0x040014B9 RID: 5305
			private int count;

			// Token: 0x040014BA RID: 5306
			private int version;

			// Token: 0x02000268 RID: 616
			private class Entry
			{
				// Token: 0x060020FC RID: 8444 RVA: 0x00047450 File Offset: 0x00046450
				public Entry(object item)
				{
					this.item = item;
					this.state = 0;
				}

				// Token: 0x040014BB RID: 5307
				public object item;

				// Token: 0x040014BC RID: 5308
				public int state;
			}

			// Token: 0x02000269 RID: 617
			private class EntryEnumerator : IEnumerator
			{
				// Token: 0x060020FD RID: 8445 RVA: 0x00047466 File Offset: 0x00046466
				public EntryEnumerator(ListBox.ItemArray items, int state, bool anyBit)
				{
					this.items = items;
					this.state = state;
					this.anyBit = anyBit;
					this.version = items.version;
					this.current = -1;
				}

				// Token: 0x060020FE RID: 8446 RVA: 0x00047498 File Offset: 0x00046498
				bool IEnumerator.MoveNext()
				{
					if (this.version != this.items.version)
					{
						throw new InvalidOperationException(SR.GetString("ListEnumVersionMismatch"));
					}
					while (this.current < this.items.count - 1)
					{
						this.current++;
						if (this.anyBit)
						{
							if ((this.items.entries[this.current].state & this.state) != 0)
							{
								return true;
							}
						}
						else if ((this.items.entries[this.current].state & this.state) == this.state)
						{
							return true;
						}
					}
					this.current = this.items.count;
					return false;
				}

				// Token: 0x060020FF RID: 8447 RVA: 0x0004754F File Offset: 0x0004654F
				void IEnumerator.Reset()
				{
					if (this.version != this.items.version)
					{
						throw new InvalidOperationException(SR.GetString("ListEnumVersionMismatch"));
					}
					this.current = -1;
				}

				// Token: 0x170004CB RID: 1227
				// (get) Token: 0x06002100 RID: 8448 RVA: 0x0004757C File Offset: 0x0004657C
				object IEnumerator.Current
				{
					get
					{
						if (this.current == -1 || this.current == this.items.count)
						{
							throw new InvalidOperationException(SR.GetString("ListEnumCurrentOutOfRange"));
						}
						return this.items.entries[this.current].item;
					}
				}

				// Token: 0x040014BD RID: 5309
				private ListBox.ItemArray items;

				// Token: 0x040014BE RID: 5310
				private bool anyBit;

				// Token: 0x040014BF RID: 5311
				private int state;

				// Token: 0x040014C0 RID: 5312
				private int current;

				// Token: 0x040014C1 RID: 5313
				private int version;
			}
		}

		// Token: 0x0200026A RID: 618
		[ListBindable(false)]
		public class ObjectCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x06002101 RID: 8449 RVA: 0x000475CC File Offset: 0x000465CC
			public ObjectCollection(ListBox owner)
			{
				this.owner = owner;
			}

			// Token: 0x06002102 RID: 8450 RVA: 0x000475DB File Offset: 0x000465DB
			public ObjectCollection(ListBox owner, ListBox.ObjectCollection value)
			{
				this.owner = owner;
				this.AddRange(value);
			}

			// Token: 0x06002103 RID: 8451 RVA: 0x000475F1 File Offset: 0x000465F1
			public ObjectCollection(ListBox owner, object[] value)
			{
				this.owner = owner;
				this.AddRange(value);
			}

			// Token: 0x170004CC RID: 1228
			// (get) Token: 0x06002104 RID: 8452 RVA: 0x00047607 File Offset: 0x00046607
			public int Count
			{
				get
				{
					return this.InnerArray.GetCount(0);
				}
			}

			// Token: 0x170004CD RID: 1229
			// (get) Token: 0x06002105 RID: 8453 RVA: 0x00047615 File Offset: 0x00046615
			internal ListBox.ItemArray InnerArray
			{
				get
				{
					if (this.items == null)
					{
						this.items = new ListBox.ItemArray(this.owner);
					}
					return this.items;
				}
			}

			// Token: 0x170004CE RID: 1230
			// (get) Token: 0x06002106 RID: 8454 RVA: 0x00047636 File Offset: 0x00046636
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x170004CF RID: 1231
			// (get) Token: 0x06002107 RID: 8455 RVA: 0x00047639 File Offset: 0x00046639
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170004D0 RID: 1232
			// (get) Token: 0x06002108 RID: 8456 RVA: 0x0004763C File Offset: 0x0004663C
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170004D1 RID: 1233
			// (get) Token: 0x06002109 RID: 8457 RVA: 0x0004763F File Offset: 0x0004663F
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x0600210A RID: 8458 RVA: 0x00047644 File Offset: 0x00046644
			public int Add(object item)
			{
				this.owner.CheckNoDataSource();
				int num = this.AddInternal(item);
				this.owner.UpdateHorizontalExtent();
				return num;
			}

			// Token: 0x0600210B RID: 8459 RVA: 0x00047670 File Offset: 0x00046670
			private int AddInternal(object item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				int num = -1;
				if (!this.owner.sorted)
				{
					this.InnerArray.Add(item);
				}
				else
				{
					if (this.Count > 0)
					{
						num = this.InnerArray.BinarySearch(item);
						if (num < 0)
						{
							num = ~num;
						}
					}
					else
					{
						num = 0;
					}
					this.InnerArray.Insert(num, item);
				}
				bool flag = false;
				try
				{
					if (this.owner.sorted)
					{
						if (this.owner.IsHandleCreated)
						{
							this.owner.NativeInsert(num, item);
							this.owner.UpdateMaxItemWidth(item, false);
							if (this.owner.selectedItems != null)
							{
								this.owner.selectedItems.Dirty();
							}
						}
					}
					else
					{
						num = this.Count - 1;
						if (this.owner.IsHandleCreated)
						{
							this.owner.NativeAdd(item);
							this.owner.UpdateMaxItemWidth(item, false);
						}
					}
					flag = true;
				}
				finally
				{
					if (!flag)
					{
						this.InnerArray.Remove(item);
					}
				}
				return num;
			}

			// Token: 0x0600210C RID: 8460 RVA: 0x00047784 File Offset: 0x00046784
			int IList.Add(object item)
			{
				return this.Add(item);
			}

			// Token: 0x0600210D RID: 8461 RVA: 0x0004778D File Offset: 0x0004678D
			public void AddRange(ListBox.ObjectCollection value)
			{
				this.owner.CheckNoDataSource();
				this.AddRangeInternal(value);
			}

			// Token: 0x0600210E RID: 8462 RVA: 0x000477A1 File Offset: 0x000467A1
			public void AddRange(object[] items)
			{
				this.owner.CheckNoDataSource();
				this.AddRangeInternal(items);
			}

			// Token: 0x0600210F RID: 8463 RVA: 0x000477B8 File Offset: 0x000467B8
			internal void AddRangeInternal(ICollection items)
			{
				/*
An exception occurred when decompiling this method (0600210F)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Windows.Forms.ListBox/ObjectCollection::AddRangeInternal(System.Collections.ICollection)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at System.Collections.Generic.HashSet`1.Resize(Int32 newSize, Boolean forceNewHashCodes)
   at System.Collections.Generic.HashSet`1.AddIfNotPresent(T value, Int32& location)
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.Enter(ILNode node, HashSet`1 visitedNodes) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 270
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.Exit(ILNode node, HashSet`1 visitedNodes) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 378
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.Enter(ILNode node, HashSet`1 visitedNodes) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 275
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.Exit(ILNode node, HashSet`1 visitedNodes) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 378
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.Exit(ILNode node, HashSet`1 visitedNodes) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 380
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.Exit(ILNode node, HashSet`1 visitedNodes) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 380
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.Enter(ILNode node, HashSet`1 visitedNodes) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 275
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.Exit(ILNode node, HashSet`1 visitedNodes) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 378
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.Exit(ILNode node, HashSet`1 visitedNodes) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 380
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.Exit(ILNode node, HashSet`1 visitedNodes) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 380
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.TrySimplifyGoto(ILExpression gotoExpr) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 238
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.RemoveGotosCore(ILBlock method) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 102
   at ICSharpCode.Decompiler.ILAst.GotoRemoval.RemoveGotos(DecompilerContext context, ILBlock method) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\GotoRemoval.cs:line 56
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 364
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
			}

			// Token: 0x170004D2 RID: 1234
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			[Browsable(false)]
			public virtual object this[int index]
			{
				get
				{
					if (index < 0 || index >= this.InnerArray.GetCount(0))
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					return this.InnerArray.GetItem(index, 0);
				}
				set
				{
					this.owner.CheckNoDataSource();
					this.SetItemInternal(index, value);
				}
			}

			// Token: 0x06002112 RID: 8466 RVA: 0x000478BE File Offset: 0x000468BE
			public virtual void Clear()
			{
				this.owner.CheckNoDataSource();
				this.ClearInternal();
			}

			// Token: 0x06002113 RID: 8467 RVA: 0x000478D4 File Offset: 0x000468D4
			internal void ClearInternal()
			{
				int count = this.owner.Items.Count;
				for (int i = 0; i < count; i++)
				{
					this.owner.UpdateMaxItemWidth(this.InnerArray.GetItem(i, 0), true);
				}
				if (this.owner.IsHandleCreated)
				{
					this.owner.NativeClear();
				}
				this.InnerArray.Clear();
				this.owner.maxWidth = -1;
				this.owner.UpdateHorizontalExtent();
			}

			// Token: 0x06002114 RID: 8468 RVA: 0x00047951 File Offset: 0x00046951
			public bool Contains(object value)
			{
				return this.IndexOf(value) != -1;
			}

			// Token: 0x06002115 RID: 8469 RVA: 0x00047960 File Offset: 0x00046960
			public void CopyTo(object[] destination, int arrayIndex)
			{
				int count = this.InnerArray.GetCount(0);
				for (int i = 0; i < count; i++)
				{
					destination[i + arrayIndex] = this.InnerArray.GetItem(i, 0);
				}
			}

			// Token: 0x06002116 RID: 8470 RVA: 0x00047998 File Offset: 0x00046998
			void ICollection.CopyTo(Array destination, int index)
			{
				int count = this.InnerArray.GetCount(0);
				for (int i = 0; i < count; i++)
				{
					destination.SetValue(this.InnerArray.GetItem(i, 0), i + index);
				}
			}

			// Token: 0x06002117 RID: 8471 RVA: 0x000479D4 File Offset: 0x000469D4
			public IEnumerator GetEnumerator()
			{
				return this.InnerArray.GetEnumerator(0);
			}

			// Token: 0x06002118 RID: 8472 RVA: 0x000479E2 File Offset: 0x000469E2
			public int IndexOf(object value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				return this.InnerArray.IndexOf(value, 0);
			}

			// Token: 0x06002119 RID: 8473 RVA: 0x000479FF File Offset: 0x000469FF
			internal int IndexOfIdentifier(object value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				return this.InnerArray.IndexOfIdentifier(value, 0);
			}

			// Token: 0x0600211A RID: 8474 RVA: 0x00047A1C File Offset: 0x00046A1C
			public void Insert(int index, object item)
			{
				this.owner.CheckNoDataSource();
				if (index < 0 || index > this.InnerArray.GetCount(0))
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.owner.sorted)
				{
					this.Add(item);
				}
				else
				{
					this.InnerArray.Insert(index, item);
					if (this.owner.IsHandleCreated)
					{
						bool flag = false;
						try
						{
							this.owner.NativeInsert(index, item);
							this.owner.UpdateMaxItemWidth(item, false);
							flag = true;
						}
						finally
						{
							if (!flag)
							{
								this.InnerArray.RemoveAt(index);
							}
						}
					}
				}
				this.owner.UpdateHorizontalExtent();
			}

			// Token: 0x0600211B RID: 8475 RVA: 0x00047AF8 File Offset: 0x00046AF8
			public void Remove(object value)
			{
				int num = this.InnerArray.IndexOf(value, 0);
				if (num != -1)
				{
					this.RemoveAt(num);
				}
			}

			// Token: 0x0600211C RID: 8476 RVA: 0x00047B20 File Offset: 0x00046B20
			public void RemoveAt(int index)
			{
				this.owner.CheckNoDataSource();
				if (index < 0 || index >= this.InnerArray.GetCount(0))
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.owner.UpdateMaxItemWidth(this.InnerArray.GetItem(index, 0), true);
				this.InnerArray.RemoveAt(index);
				if (this.owner.IsHandleCreated)
				{
					this.owner.NativeRemoveAt(index);
				}
				this.owner.UpdateHorizontalExtent();
			}

			// Token: 0x0600211D RID: 8477 RVA: 0x00047BC8 File Offset: 0x00046BC8
			internal void SetItemInternal(int index, object value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (index < 0 || index >= this.InnerArray.GetCount(0))
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.owner.UpdateMaxItemWidth(this.InnerArray.GetItem(index, 0), true);
				this.InnerArray.SetItem(index, value);
				if (this.owner.IsHandleCreated)
				{
					bool flag = this.owner.SelectedIndex == index;
					if (string.Compare(this.owner.GetItemText(value), this.owner.NativeGetItemText(index), true, CultureInfo.CurrentCulture) != 0)
					{
						this.owner.NativeRemoveAt(index);
						this.owner.SelectedItems.SetSelected(index, false);
						this.owner.NativeInsert(index, value);
						this.owner.UpdateMaxItemWidth(value, false);
						if (flag)
						{
							this.owner.SelectedIndex = index;
						}
					}
					else if (flag)
					{
						this.owner.OnSelectedIndexChanged(EventArgs.Empty);
					}
				}
				this.owner.UpdateHorizontalExtent();
			}

			// Token: 0x040014C2 RID: 5314
			private ListBox owner;

			// Token: 0x040014C3 RID: 5315
			private ListBox.ItemArray items;
		}

		// Token: 0x0200026B RID: 619
		public class IntegerCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x0600211E RID: 8478 RVA: 0x00047CFC File Offset: 0x00046CFC
			public IntegerCollection(ListBox owner)
			{
				this.owner = owner;
			}

			// Token: 0x170004D3 RID: 1235
			// (get) Token: 0x0600211F RID: 8479 RVA: 0x00047D0B File Offset: 0x00046D0B
			[Browsable(false)]
			public int Count
			{
				get
				{
					return this.count;
				}
			}

			// Token: 0x170004D4 RID: 1236
			// (get) Token: 0x06002120 RID: 8480 RVA: 0x00047D13 File Offset: 0x00046D13
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x170004D5 RID: 1237
			// (get) Token: 0x06002121 RID: 8481 RVA: 0x00047D16 File Offset: 0x00046D16
			bool ICollection.IsSynchronized
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170004D6 RID: 1238
			// (get) Token: 0x06002122 RID: 8482 RVA: 0x00047D19 File Offset: 0x00046D19
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170004D7 RID: 1239
			// (get) Token: 0x06002123 RID: 8483 RVA: 0x00047D1C File Offset: 0x00046D1C
			bool IList.IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06002124 RID: 8484 RVA: 0x00047D1F File Offset: 0x00046D1F
			public bool Contains(int item)
			{
				return this.IndexOf(item) != -1;
			}

			// Token: 0x06002125 RID: 8485 RVA: 0x00047D2E File Offset: 0x00046D2E
			bool IList.Contains(object item)
			{
				return item is int && this.Contains((int)item);
			}

			// Token: 0x06002126 RID: 8486 RVA: 0x00047D46 File Offset: 0x00046D46
			public void Clear()
			{
				this.count = 0;
				this.innerArray = null;
			}

			// Token: 0x06002127 RID: 8487 RVA: 0x00047D56 File Offset: 0x00046D56
			public int IndexOf(int item)
			{
				return Array.IndexOf<int>(this.innerArray, item);
			}

			// Token: 0x06002128 RID: 8488 RVA: 0x00047D64 File Offset: 0x00046D64
			int IList.IndexOf(object item)
			{
				if (item is int)
				{
					return this.IndexOf((int)item);
				}
				return -1;
			}

			// Token: 0x06002129 RID: 8489 RVA: 0x00047D7C File Offset: 0x00046D7C
			private int AddInternal(int item)
			{
				this.EnsureSpace(1);
				int num = this.IndexOf(item);
				if (num == -1)
				{
					this.innerArray[this.count++] = item;
					Array.Sort<int>(this.innerArray, 0, this.count);
					num = this.IndexOf(item);
				}
				return num;
			}

			// Token: 0x0600212A RID: 8490 RVA: 0x00047DD0 File Offset: 0x00046DD0
			public int Add(int item)
			{
				int num = this.AddInternal(item);
				this.owner.UpdateCustomTabOffsets();
				return num;
			}

			// Token: 0x0600212B RID: 8491 RVA: 0x00047DF1 File Offset: 0x00046DF1
			int IList.Add(object item)
			{
				if (!(item is int))
				{
					throw new ArgumentException("item");
				}
				return this.Add((int)item);
			}

			// Token: 0x0600212C RID: 8492 RVA: 0x00047E12 File Offset: 0x00046E12
			public void AddRange(int[] items)
			{
				this.AddRangeInternal(items);
			}

			// Token: 0x0600212D RID: 8493 RVA: 0x00047E1B File Offset: 0x00046E1B
			public void AddRange(ListBox.IntegerCollection value)
			{
				this.AddRangeInternal(value);
			}

			// Token: 0x0600212E RID: 8494 RVA: 0x00047E24 File Offset: 0x00046E24
			private void AddRangeInternal(ICollection items)
			{
				if (items == null)
				{
					throw new ArgumentNullException("items");
				}
				this.owner.BeginUpdate();
				try
				{
					this.EnsureSpace(items.Count);
					foreach (object obj in items)
					{
						if (!(obj is int))
						{
							throw new ArgumentException("item");
						}
						this.AddInternal((int)obj);
					}
					this.owner.UpdateCustomTabOffsets();
				}
				finally
				{
					this.owner.EndUpdate();
				}
			}

			// Token: 0x0600212F RID: 8495 RVA: 0x00047ED8 File Offset: 0x00046ED8
			private void EnsureSpace(int elements)
			{
				if (this.innerArray == null)
				{
					this.innerArray = new int[Math.Max(elements, 4)];
					return;
				}
				if (this.count + elements >= this.innerArray.Length)
				{
					int num = Math.Max(this.innerArray.Length * 2, this.innerArray.Length + elements);
					int[] array = new int[num];
					this.innerArray.CopyTo(array, 0);
					this.innerArray = array;
				}
			}

			// Token: 0x06002130 RID: 8496 RVA: 0x00047F47 File Offset: 0x00046F47
			void IList.Clear()
			{
				this.Clear();
			}

			// Token: 0x06002131 RID: 8497 RVA: 0x00047F4F File Offset: 0x00046F4F
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException(SR.GetString("ListBoxCantInsertIntoIntegerCollection"));
			}

			// Token: 0x06002132 RID: 8498 RVA: 0x00047F60 File Offset: 0x00046F60
			void IList.Remove(object value)
			{
				if (!(value is int))
				{
					throw new ArgumentException("value");
				}
				this.Remove((int)value);
			}

			// Token: 0x06002133 RID: 8499 RVA: 0x00047F81 File Offset: 0x00046F81
			void IList.RemoveAt(int index)
			{
				this.RemoveAt(index);
			}

			// Token: 0x06002134 RID: 8500 RVA: 0x00047F8C File Offset: 0x00046F8C
			public void Remove(int item)
			{
				int num = this.IndexOf(item);
				if (num != -1)
				{
					this.RemoveAt(num);
				}
			}

			// Token: 0x06002135 RID: 8501 RVA: 0x00047FAC File Offset: 0x00046FAC
			public void RemoveAt(int index)
			{
				this.count--;
				for (int i = index; i < this.count; i++)
				{
					this.innerArray[i] = this.innerArray[i + 1];
				}
			}

			// Token: 0x170004D8 RID: 1240
			public int this[int index]
			{
				get
				{
					return this.innerArray[index];
				}
				set
				{
					if (index < 0 || index >= this.count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.innerArray[index] = value;
					this.owner.UpdateCustomTabOffsets();
				}
			}

			// Token: 0x170004D9 RID: 1241
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					if (!(value is int))
					{
						throw new ArgumentException("value");
					}
					this[index] = (int)value;
				}
			}

			// Token: 0x0600213A RID: 8506 RVA: 0x00048088 File Offset: 0x00047088
			public void CopyTo(Array destination, int index)
			{
				int num = this.Count;
				for (int i = 0; i < num; i++)
				{
					destination.SetValue(this[i], i + index);
				}
			}

			// Token: 0x0600213B RID: 8507 RVA: 0x000480BD File Offset: 0x000470BD
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new ListBox.IntegerCollection.CustomTabOffsetsEnumerator(this);
			}

			// Token: 0x040014C4 RID: 5316
			private ListBox owner;

			// Token: 0x040014C5 RID: 5317
			private int[] innerArray;

			// Token: 0x040014C6 RID: 5318
			private int count;

			// Token: 0x0200026C RID: 620
			private class CustomTabOffsetsEnumerator : IEnumerator
			{
				// Token: 0x0600213C RID: 8508 RVA: 0x000480C5 File Offset: 0x000470C5
				public CustomTabOffsetsEnumerator(ListBox.IntegerCollection items)
				{
					this.items = items;
					this.current = -1;
				}

				// Token: 0x0600213D RID: 8509 RVA: 0x000480DB File Offset: 0x000470DB
				bool IEnumerator.MoveNext()
				{
					if (this.current < this.items.Count - 1)
					{
						this.current++;
						return true;
					}
					this.current = this.items.Count;
					return false;
				}

				// Token: 0x0600213E RID: 8510 RVA: 0x00048114 File Offset: 0x00047114
				void IEnumerator.Reset()
				{
					this.current = -1;
				}

				// Token: 0x170004DA RID: 1242
				// (get) Token: 0x0600213F RID: 8511 RVA: 0x00048120 File Offset: 0x00047120
				object IEnumerator.Current
				{
					get
					{
						if (this.current == -1 || this.current == this.items.Count)
						{
							throw new InvalidOperationException(SR.GetString("ListEnumCurrentOutOfRange"));
						}
						return this.items[this.current];
					}
				}

				// Token: 0x040014C7 RID: 5319
				private ListBox.IntegerCollection items;

				// Token: 0x040014C8 RID: 5320
				private int current;
			}
		}

		// Token: 0x0200026D RID: 621
		public class SelectedIndexCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x06002140 RID: 8512 RVA: 0x0004816F File Offset: 0x0004716F
			public SelectedIndexCollection(ListBox owner)
			{
				this.owner = owner;
			}

			// Token: 0x170004DB RID: 1243
			// (get) Token: 0x06002141 RID: 8513 RVA: 0x0004817E File Offset: 0x0004717E
			[Browsable(false)]
			public int Count
			{
				get
				{
					return this.owner.SelectedItems.Count;
				}
			}

			// Token: 0x170004DC RID: 1244
			// (get) Token: 0x06002142 RID: 8514 RVA: 0x00048190 File Offset: 0x00047190
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x170004DD RID: 1245
			// (get) Token: 0x06002143 RID: 8515 RVA: 0x00048193 File Offset: 0x00047193
			bool ICollection.IsSynchronized
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170004DE RID: 1246
			// (get) Token: 0x06002144 RID: 8516 RVA: 0x00048196 File Offset: 0x00047196
			bool IList.IsFixedSize
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170004DF RID: 1247
			// (get) Token: 0x06002145 RID: 8517 RVA: 0x00048199 File Offset: 0x00047199
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06002146 RID: 8518 RVA: 0x0004819C File Offset: 0x0004719C
			public bool Contains(int selectedIndex)
			{
				return this.IndexOf(selectedIndex) != -1;
			}

			// Token: 0x06002147 RID: 8519 RVA: 0x000481AB File Offset: 0x000471AB
			bool IList.Contains(object selectedIndex)
			{
				return selectedIndex is int && this.Contains((int)selectedIndex);
			}

			// Token: 0x06002148 RID: 8520 RVA: 0x000481C4 File Offset: 0x000471C4
			public int IndexOf(int selectedIndex)
			{
				if (selectedIndex >= 0 && selectedIndex < this.InnerArray.GetCount(0) && this.InnerArray.GetState(selectedIndex, ListBox.SelectedObjectCollection.SelectedObjectMask))
				{
					return this.InnerArray.IndexOf(this.InnerArray.GetItem(selectedIndex, 0), ListBox.SelectedObjectCollection.SelectedObjectMask);
				}
				return -1;
			}

			// Token: 0x06002149 RID: 8521 RVA: 0x00048216 File Offset: 0x00047216
			int IList.IndexOf(object selectedIndex)
			{
				/*
An exception occurred when decompiling this method (06002149)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Int32 System.Windows.Forms.ListBox/SelectedIndexCollection::System.Collections.IList.IndexOf(System.Object)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at System.Linq.Enumerable.JoinIterator[TOuter,TInner,TKey,TResult](IEnumerable`1 outer, IEnumerable`1 inner, Func`2 outerKeySelector, Func`2 innerKeySelector, Func`3 resultSelector, IEqualityComparer`1 comparer)
   at System.Linq.Enumerable.Join[TOuter,TInner,TKey,TResult](IEnumerable`1 outer, IEnumerable`1 inner, Func`2 outerKeySelector, Func`2 innerKeySelector, Func`3 resultSelector)
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 139
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
			}

			// Token: 0x0600214A RID: 8522 RVA: 0x0004822E File Offset: 0x0004722E
			int IList.Add(object value)
			{
				throw new NotSupportedException(SR.GetString("ListBoxSelectedIndexCollectionIsReadOnly"));
			}

			// Token: 0x0600214B RID: 8523 RVA: 0x0004823F File Offset: 0x0004723F
			void IList.Clear()
			{
				throw new NotSupportedException(SR.GetString("ListBoxSelectedIndexCollectionIsReadOnly"));
			}

			// Token: 0x0600214C RID: 8524 RVA: 0x00048250 File Offset: 0x00047250
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException(SR.GetString("ListBoxSelectedIndexCollectionIsReadOnly"));
			}

			// Token: 0x0600214D RID: 8525 RVA: 0x00048261 File Offset: 0x00047261
			void IList.Remove(object value)
			{
				throw new NotSupportedException(SR.GetString("ListBoxSelectedIndexCollectionIsReadOnly"));
			}

			// Token: 0x0600214E RID: 8526 RVA: 0x00048272 File Offset: 0x00047272
			void IList.RemoveAt(int index)
			{
				throw new NotSupportedException(SR.GetString("ListBoxSelectedIndexCollectionIsReadOnly"));
			}

			// Token: 0x170004E0 RID: 1248
			public int this[int index]
			{
				get
				{
					object entryObject = this.InnerArray.GetEntryObject(index, ListBox.SelectedObjectCollection.SelectedObjectMask);
					return this.InnerArray.IndexOfIdentifier(entryObject, 0);
				}
			}

			// Token: 0x170004E1 RID: 1249
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					throw new NotSupportedException(SR.GetString("ListBoxSelectedIndexCollectionIsReadOnly"));
				}
			}

			// Token: 0x170004E2 RID: 1250
			// (get) Token: 0x06002152 RID: 8530 RVA: 0x000482CF File Offset: 0x000472CF
			private ListBox.ItemArray InnerArray
			{
				get
				{
					this.owner.SelectedItems.EnsureUpToDate();
					return this.owner.Items.InnerArray;
				}
			}

			// Token: 0x06002153 RID: 8531 RVA: 0x000482F4 File Offset: 0x000472F4
			public void CopyTo(Array destination, int index)
			{
				int count = this.Count;
				for (int i = 0; i < count; i++)
				{
					destination.SetValue(this[i], i + index);
				}
			}

			// Token: 0x06002154 RID: 8532 RVA: 0x00048329 File Offset: 0x00047329
			public void Clear()
			{
				if (this.owner != null)
				{
					this.owner.ClearSelected();
				}
			}

			// Token: 0x06002155 RID: 8533 RVA: 0x00048340 File Offset: 0x00047340
			public void Add(int index)
			{
				if (this.owner != null)
				{
					ListBox.ObjectCollection items = this.owner.Items;
					if (items != null && index != -1 && !this.Contains(index))
					{
						this.owner.SetSelected(index, true);
					}
				}
			}

			// Token: 0x06002156 RID: 8534 RVA: 0x00048380 File Offset: 0x00047380
			public void Remove(int index)
			{
				if (this.owner != null)
				{
					ListBox.ObjectCollection items = this.owner.Items;
					if (items != null && index != -1 && this.Contains(index))
					{
						this.owner.SetSelected(index, false);
					}
				}
			}

			// Token: 0x06002157 RID: 8535 RVA: 0x000483BE File Offset: 0x000473BE
			public IEnumerator GetEnumerator()
			{
				return new ListBox.SelectedIndexCollection.SelectedIndexEnumerator(this);
			}

			// Token: 0x040014C9 RID: 5321
			private ListBox owner;

			// Token: 0x0200026E RID: 622
			private class SelectedIndexEnumerator : IEnumerator
			{
				// Token: 0x06002158 RID: 8536 RVA: 0x000483C6 File Offset: 0x000473C6
				public SelectedIndexEnumerator(ListBox.SelectedIndexCollection items)
				{
					this.items = items;
					this.current = -1;
				}

				// Token: 0x06002159 RID: 8537 RVA: 0x000483DC File Offset: 0x000473DC
				bool IEnumerator.MoveNext()
				{
					if (this.current < this.items.Count - 1)
					{
						this.current++;
						return true;
					}
					this.current = this.items.Count;
					return false;
				}

				// Token: 0x0600215A RID: 8538 RVA: 0x00048415 File Offset: 0x00047415
				void IEnumerator.Reset()
				{
					this.current = -1;
				}

				// Token: 0x170004E3 RID: 1251
				// (get) Token: 0x0600215B RID: 8539 RVA: 0x00048420 File Offset: 0x00047420
				object IEnumerator.Current
				{
					get
					{
						if (this.current == -1 || this.current == this.items.Count)
						{
							throw new InvalidOperationException(SR.GetString("ListEnumCurrentOutOfRange"));
						}
						return this.items[this.current];
					}
				}

				// Token: 0x040014CA RID: 5322
				private ListBox.SelectedIndexCollection items;

				// Token: 0x040014CB RID: 5323
				private int current;
			}
		}

		// Token: 0x0200026F RID: 623
		public class SelectedObjectCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x0600215C RID: 8540 RVA: 0x0004846F File Offset: 0x0004746F
			public SelectedObjectCollection(ListBox owner)
			{
				this.owner = owner;
				this.stateDirty = true;
				this.lastVersion = -1;
			}

			// Token: 0x170004E4 RID: 1252
			// (get) Token: 0x0600215D RID: 8541 RVA: 0x0004848C File Offset: 0x0004748C
			public int Count
			{
				get
				{
					if (!this.owner.IsHandleCreated)
					{
						if (this.lastVersion != this.InnerArray.Version)
						{
							this.lastVersion = this.InnerArray.Version;
							this.count = this.InnerArray.GetCount(ListBox.SelectedObjectCollection.SelectedObjectMask);
						}
						return this.count;
					}
					switch (this.owner.selectionModeChanging ? this.owner.cachedSelectionMode : this.owner.selectionMode)
					{
					case SelectionMode.None:
						return 0;
					case SelectionMode.One:
					{
						int selectedIndex = this.owner.SelectedIndex;
						if (selectedIndex >= 0)
						{
							return 1;
						}
						return 0;
					}
					case SelectionMode.MultiSimple:
					case SelectionMode.MultiExtended:
						return (int)this.owner.SendMessage(400, 0, 0);
					default:
						return 0;
					}
				}
			}

			// Token: 0x170004E5 RID: 1253
			// (get) Token: 0x0600215E RID: 8542 RVA: 0x00048556 File Offset: 0x00047556
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x170004E6 RID: 1254
			// (get) Token: 0x0600215F RID: 8543 RVA: 0x00048559 File Offset: 0x00047559
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170004E7 RID: 1255
			// (get) Token: 0x06002160 RID: 8544 RVA: 0x0004855C File Offset: 0x0004755C
			bool IList.IsFixedSize
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06002161 RID: 8545 RVA: 0x0004855F File Offset: 0x0004755F
			internal void Dirty()
			{
				this.stateDirty = true;
			}

			// Token: 0x170004E8 RID: 1256
			// (get) Token: 0x06002162 RID: 8546 RVA: 0x00048568 File Offset: 0x00047568
			private ListBox.ItemArray InnerArray
			{
				get
				{
					this.EnsureUpToDate();
					return this.owner.Items.InnerArray;
				}
			}

			// Token: 0x06002163 RID: 8547 RVA: 0x00048580 File Offset: 0x00047580
			internal void EnsureUpToDate()
			{
				if (this.stateDirty)
				{
					this.stateDirty = false;
					if (this.owner.IsHandleCreated)
					{
						this.owner.NativeUpdateSelection();
					}
				}
			}

			// Token: 0x170004E9 RID: 1257
			// (get) Token: 0x06002164 RID: 8548 RVA: 0x000485A9 File Offset: 0x000475A9
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06002165 RID: 8549 RVA: 0x000485AC File Offset: 0x000475AC
			public bool Contains(object selectedObject)
			{
				return this.IndexOf(selectedObject) != -1;
			}

			// Token: 0x06002166 RID: 8550 RVA: 0x000485BB File Offset: 0x000475BB
			public int IndexOf(object selectedObject)
			{
				return this.InnerArray.IndexOf(selectedObject, ListBox.SelectedObjectCollection.SelectedObjectMask);
			}

			// Token: 0x06002167 RID: 8551 RVA: 0x000485CE File Offset: 0x000475CE
			int IList.Add(object value)
			{
				throw new NotSupportedException(SR.GetString("ListBoxSelectedObjectCollectionIsReadOnly"));
			}

			// Token: 0x06002168 RID: 8552 RVA: 0x000485DF File Offset: 0x000475DF
			void IList.Clear()
			{
				throw new NotSupportedException(SR.GetString("ListBoxSelectedObjectCollectionIsReadOnly"));
			}

			// Token: 0x06002169 RID: 8553 RVA: 0x000485F0 File Offset: 0x000475F0
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException(SR.GetString("ListBoxSelectedObjectCollectionIsReadOnly"));
			}

			// Token: 0x0600216A RID: 8554 RVA: 0x00048601 File Offset: 0x00047601
			void IList.Remove(object value)
			{
				throw new NotSupportedException(SR.GetString("ListBoxSelectedObjectCollectionIsReadOnly"));
			}

			// Token: 0x0600216B RID: 8555 RVA: 0x00048612 File Offset: 0x00047612
			void IList.RemoveAt(int index)
			{
				throw new NotSupportedException(SR.GetString("ListBoxSelectedObjectCollectionIsReadOnly"));
			}

			// Token: 0x0600216C RID: 8556 RVA: 0x00048623 File Offset: 0x00047623
			internal object GetObjectAt(int index)
			{
				return this.InnerArray.GetEntryObject(index, ListBox.SelectedObjectCollection.SelectedObjectMask);
			}

			// Token: 0x170004EA RID: 1258
			[Browsable(false)]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			public object this[int index]
			{
				get
				{
					return this.InnerArray.GetItem(index, ListBox.SelectedObjectCollection.SelectedObjectMask);
				}
				set
				{
					throw new NotSupportedException(SR.GetString("ListBoxSelectedObjectCollectionIsReadOnly"));
				}
			}

			// Token: 0x0600216F RID: 8559 RVA: 0x0004865C File Offset: 0x0004765C
			public void CopyTo(Array destination, int index)
			{
				int num = this.InnerArray.GetCount(ListBox.SelectedObjectCollection.SelectedObjectMask);
				for (int i = 0; i < num; i++)
				{
					destination.SetValue(this.InnerArray.GetItem(i, ListBox.SelectedObjectCollection.SelectedObjectMask), i + index);
				}
			}

			// Token: 0x06002170 RID: 8560 RVA: 0x000486A0 File Offset: 0x000476A0
			public IEnumerator GetEnumerator()
			{
				return this.InnerArray.GetEnumerator(ListBox.SelectedObjectCollection.SelectedObjectMask);
			}

			// Token: 0x06002171 RID: 8561 RVA: 0x000486B2 File Offset: 0x000476B2
			internal bool GetSelected(int index)
			{
				return this.InnerArray.GetState(index, ListBox.SelectedObjectCollection.SelectedObjectMask);
			}

			// Token: 0x06002172 RID: 8562 RVA: 0x000486C8 File Offset: 0x000476C8
			internal void PushSelectionIntoNativeListBox(int index)
			{
				bool state = this.owner.Items.InnerArray.GetState(index, ListBox.SelectedObjectCollection.SelectedObjectMask);
				if (state)
				{
					this.owner.NativeSetSelected(index, true);
				}
			}

			// Token: 0x06002173 RID: 8563 RVA: 0x00048701 File Offset: 0x00047701
			internal void SetSelected(int index, bool value)
			{
				this.InnerArray.SetState(index, ListBox.SelectedObjectCollection.SelectedObjectMask, value);
			}

			// Token: 0x06002174 RID: 8564 RVA: 0x00048715 File Offset: 0x00047715
			public void Clear()
			{
				if (this.owner != null)
				{
					this.owner.ClearSelected();
				}
			}

			// Token: 0x06002175 RID: 8565 RVA: 0x0004872C File Offset: 0x0004772C
			public void Add(object value)
			{
				if (this.owner != null)
				{
					ListBox.ObjectCollection items = this.owner.Items;
					if (items != null && value != null)
					{
						int num = items.IndexOf(value);
						if (num != -1 && !this.GetSelected(num))
						{
							this.owner.SelectedIndex = num;
						}
					}
				}
			}

			// Token: 0x06002176 RID: 8566 RVA: 0x00048774 File Offset: 0x00047774
			public void Remove(object value)
			{
				if (this.owner != null)
				{
					ListBox.ObjectCollection items = this.owner.Items;
					if ((items != null) & (value != null))
					{
						int num = items.IndexOf(value);
						if (num != -1 && this.GetSelected(num))
						{
							this.owner.SetSelected(num, false);
						}
					}
				}
			}

			// Token: 0x040014CC RID: 5324
			internal static int SelectedObjectMask = ListBox.ItemArray.CreateMask();

			// Token: 0x040014CD RID: 5325
			private ListBox owner;

			// Token: 0x040014CE RID: 5326
			private bool stateDirty;

			// Token: 0x040014CF RID: 5327
			private int lastVersion;

			// Token: 0x040014D0 RID: 5328
			private int count;
		}
	}
}
