using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms.Layout;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x02000480 RID: 1152
	[DefaultProperty("Items")]
	[Designer("System.Windows.Forms.Design.ListViewDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultEvent("SelectedIndexChanged")]
	[SRDescription("DescriptionListView")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Docking(DockingBehavior.Ask)]
	public class ListView : Control
	{
		// Token: 0x06004376 RID: 17270 RVA: 0x000F20A0 File Offset: 0x000F10A0
		public ListView()
		{
			this.listViewState = new BitVector32(8392260);
			this.listViewState1 = new BitVector32(8);
			base.SetStyle(ControlStyles.UserPaint, false);
			base.SetStyle(ControlStyles.StandardClick, false);
			base.SetStyle(ControlStyles.UseTextForAccessibility, false);
			this.odCacheFont = this.Font;
			this.odCacheFontHandle = base.FontHandle;
			base.SetBounds(0, 0, 121, 97);
			this.listItemCollection = new ListView.ListViewItemCollection(new ListView.ListViewNativeItemCollection(this));
			this.columnHeaderCollection = new ListView.ColumnHeaderCollection(this);
		}

		// Token: 0x17000D38 RID: 3384
		// (get) Token: 0x06004377 RID: 17271 RVA: 0x000F21AB File Offset: 0x000F11AB
		// (set) Token: 0x06004378 RID: 17272 RVA: 0x000F21B4 File Offset: 0x000F11B4
		[DefaultValue(ItemActivation.Standard)]
		[SRDescription("ListViewActivationDescr")]
		[SRCategory("CatBehavior")]
		public ItemActivation Activation
		{
			get
			{
				return this.activation;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ItemActivation));
				}
				if (this.HotTracking && value != ItemActivation.OneClick)
				{
					throw new ArgumentException(SR.GetString("ListViewActivationMustBeOnWhenHotTrackingIsOn"), "value");
				}
				if (this.activation != value)
				{
					this.activation = value;
					this.UpdateExtendedStyles();
				}
			}
		}

		// Token: 0x17000D39 RID: 3385
		// (get) Token: 0x06004379 RID: 17273 RVA: 0x000F221E File Offset: 0x000F121E
		// (set) Token: 0x0600437A RID: 17274 RVA: 0x000F2228 File Offset: 0x000F1228
		[Localizable(true)]
		[SRDescription("ListViewAlignmentDescr")]
		[DefaultValue(ListViewAlignment.Top)]
		[SRCategory("CatBehavior")]
		public ListViewAlignment Alignment
		{
			get
			{
				return this.alignStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid_NotSequential(value, (int)value, new int[] { 0, 2, 1, 5 }))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ListViewAlignment));
				}
				if (this.alignStyle != value)
				{
					this.alignStyle = value;
					this.RecreateHandleInternal();
				}
			}
		}

		// Token: 0x17000D3A RID: 3386
		// (get) Token: 0x0600437B RID: 17275 RVA: 0x000F2283 File Offset: 0x000F1283
		// (set) Token: 0x0600437C RID: 17276 RVA: 0x000F2291 File Offset: 0x000F1291
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ListViewAllowColumnReorderDescr")]
		public bool AllowColumnReorder
		{
			get
			{
				return this.listViewState[2];
			}
			set
			{
				if (this.AllowColumnReorder != value)
				{
					this.listViewState[2] = value;
					this.UpdateExtendedStyles();
				}
			}
		}

		// Token: 0x17000D3B RID: 3387
		// (get) Token: 0x0600437D RID: 17277 RVA: 0x000F22AF File Offset: 0x000F12AF
		// (set) Token: 0x0600437E RID: 17278 RVA: 0x000F22BD File Offset: 0x000F12BD
		[DefaultValue(true)]
		[SRDescription("ListViewAutoArrangeDescr")]
		[SRCategory("CatBehavior")]
		public bool AutoArrange
		{
			get
			{
				return this.listViewState[4];
			}
			set
			{
				if (this.AutoArrange != value)
				{
					this.listViewState[4] = value;
					base.UpdateStyles();
				}
			}
		}

		// Token: 0x17000D3C RID: 3388
		// (get) Token: 0x0600437F RID: 17279 RVA: 0x000F22DB File Offset: 0x000F12DB
		// (set) Token: 0x06004380 RID: 17280 RVA: 0x000F22F1 File Offset: 0x000F12F1
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
				if (base.IsHandleCreated)
				{
					base.SendMessage(4097, 0, ColorTranslator.ToWin32(this.BackColor));
				}
			}
		}

		// Token: 0x17000D3D RID: 3389
		// (get) Token: 0x06004381 RID: 17281 RVA: 0x000F231A File Offset: 0x000F131A
		// (set) Token: 0x06004382 RID: 17282 RVA: 0x000F2322 File Offset: 0x000F1322
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

		// Token: 0x14000260 RID: 608
		// (add) Token: 0x06004383 RID: 17283 RVA: 0x000F232B File Offset: 0x000F132B
		// (remove) Token: 0x06004384 RID: 17284 RVA: 0x000F2334 File Offset: 0x000F1334
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x17000D3E RID: 3390
		// (get) Token: 0x06004385 RID: 17285 RVA: 0x000F233D File Offset: 0x000F133D
		// (set) Token: 0x06004386 RID: 17286 RVA: 0x000F2350 File Offset: 0x000F1350
		[SRDescription("ListViewBackgroundImageTiledDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		public bool BackgroundImageTiled
		{
			get
			{
				return this.listViewState[65536];
			}
			set
			{
				if (this.BackgroundImageTiled != value)
				{
					this.listViewState[65536] = value;
					if (base.IsHandleCreated && this.BackgroundImage != null)
					{
						NativeMethods.LVBKIMAGE lvbkimage = new NativeMethods.LVBKIMAGE();
						lvbkimage.xOffset = 0;
						lvbkimage.yOffset = 0;
						if (this.BackgroundImageTiled)
						{
							lvbkimage.ulFlags = 16;
						}
						else
						{
							lvbkimage.ulFlags = 0;
						}
						lvbkimage.ulFlags |= 2;
						lvbkimage.pszImage = this.backgroundImageFileName;
						lvbkimage.cchImageMax = this.backgroundImageFileName.Length + 1;
						UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.LVM_SETBKIMAGE, 0, lvbkimage);
					}
				}
			}
		}

		// Token: 0x17000D3F RID: 3391
		// (get) Token: 0x06004387 RID: 17287 RVA: 0x000F23FD File Offset: 0x000F13FD
		// (set) Token: 0x06004388 RID: 17288 RVA: 0x000F2405 File Offset: 0x000F1405
		[DispId(-504)]
		[SRDescription("borderStyleDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(BorderStyle.Fixed3D)]
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
				if (this.borderStyle != value)
				{
					this.borderStyle = value;
					base.UpdateStyles();
				}
			}
		}

		// Token: 0x17000D40 RID: 3392
		// (get) Token: 0x06004389 RID: 17289 RVA: 0x000F2443 File Offset: 0x000F1443
		// (set) Token: 0x0600438A RID: 17290 RVA: 0x000F2454 File Offset: 0x000F1454
		[DefaultValue(false)]
		[SRDescription("ListViewCheckBoxesDescr")]
		[SRCategory("CatAppearance")]
		public bool CheckBoxes
		{
			get
			{
				return this.listViewState[8];
			}
			set
			{
				if (this.UseCompatibleStateImageBehavior)
				{
					if (this.CheckBoxes != value)
					{
						if (value && this.View == View.Tile)
						{
							throw new NotSupportedException(SR.GetString("ListViewCheckBoxesNotSupportedInTileView"));
						}
						if (this.CheckBoxes)
						{
							this.savedCheckedItems = new List<ListViewItem>(this.CheckedItems.Count);
							ListViewItem[] array = new ListViewItem[this.CheckedItems.Count];
							this.CheckedItems.CopyTo(array, 0);
							for (int i = 0; i < array.Length; i++)
							{
								this.savedCheckedItems.Add(array[i]);
							}
						}
						this.listViewState[8] = value;
						this.UpdateExtendedStyles();
						if (this.CheckBoxes && this.savedCheckedItems != null)
						{
							if (this.savedCheckedItems.Count > 0)
							{
								foreach (ListViewItem listViewItem in this.savedCheckedItems)
								{
									listViewItem.Checked = true;
								}
							}
							this.savedCheckedItems = null;
						}
						if (this.AutoArrange)
						{
							this.ArrangeIcons(this.Alignment);
							return;
						}
					}
				}
				else if (this.CheckBoxes != value)
				{
					if (value && this.View == View.Tile)
					{
						throw new NotSupportedException(SR.GetString("ListViewCheckBoxesNotSupportedInTileView"));
					}
					if (this.CheckBoxes)
					{
						this.savedCheckedItems = new List<ListViewItem>(this.CheckedItems.Count);
						ListViewItem[] array2 = new ListViewItem[this.CheckedItems.Count];
						this.CheckedItems.CopyTo(array2, 0);
						for (int j = 0; j < array2.Length; j++)
						{
							this.savedCheckedItems.Add(array2[j]);
						}
					}
					this.listViewState[8] = value;
					if ((!value && this.StateImageList != null && base.IsHandleCreated) || (!value && this.Alignment == ListViewAlignment.Left && base.IsHandleCreated) || (value && this.View == View.List && base.IsHandleCreated) || (value && (this.View == View.SmallIcon || this.View == View.LargeIcon) && base.IsHandleCreated))
					{
						this.RecreateHandleInternal();
					}
					else
					{
						this.UpdateExtendedStyles();
					}
					if (this.CheckBoxes && this.savedCheckedItems != null)
					{
						if (this.savedCheckedItems.Count > 0)
						{
							foreach (ListViewItem listViewItem2 in this.savedCheckedItems)
							{
								listViewItem2.Checked = true;
							}
						}
						this.savedCheckedItems = null;
					}
					if (base.IsHandleCreated && this.imageListState != null)
					{
						if (this.CheckBoxes)
						{
							base.SendMessage(4099, 2, this.imageListState.Handle);
						}
						else
						{
							base.SendMessage(4099, 2, IntPtr.Zero);
						}
					}
					if (this.AutoArrange)
					{
						this.ArrangeIcons(this.Alignment);
					}
				}
			}
		}

		// Token: 0x17000D41 RID: 3393
		// (get) Token: 0x0600438B RID: 17291 RVA: 0x000F273C File Offset: 0x000F173C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public ListView.CheckedIndexCollection CheckedIndices
		{
			get
			{
				if (this.checkedIndexCollection == null)
				{
					this.checkedIndexCollection = new ListView.CheckedIndexCollection(this);
				}
				return this.checkedIndexCollection;
			}
		}

		// Token: 0x17000D42 RID: 3394
		// (get) Token: 0x0600438C RID: 17292 RVA: 0x000F2758 File Offset: 0x000F1758
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ListView.CheckedListViewItemCollection CheckedItems
		{
			get
			{
				if (this.checkedListViewItemCollection == null)
				{
					this.checkedListViewItemCollection = new ListView.CheckedListViewItemCollection(this);
				}
				return this.checkedListViewItemCollection;
			}
		}

		// Token: 0x17000D43 RID: 3395
		// (get) Token: 0x0600438D RID: 17293 RVA: 0x000F2774 File Offset: 0x000F1774
		[SRDescription("ListViewColumnsDescr")]
		[SRCategory("CatBehavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Localizable(true)]
		[MergableProperty(false)]
		[Editor("System.Windows.Forms.Design.ColumnHeaderCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public ListView.ColumnHeaderCollection Columns
		{
			get
			{
				return this.columnHeaderCollection;
			}
		}

		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x0600438E RID: 17294 RVA: 0x000F277C File Offset: 0x000F177C
		private bool ComctlSupportsVisualStyles
		{
			get
			{
				if (!this.listViewState[4194304])
				{
					this.listViewState[4194304] = true;
					this.listViewState[2097152] = Application.ComCtlSupportsVisualStyles;
				}
				return this.listViewState[2097152];
			}
		}

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x0600438F RID: 17295 RVA: 0x000F27D4 File Offset: 0x000F17D4
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "SysListView32";
				if (base.IsHandleCreated)
				{
					int num = (int)(long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, base.Handle), -16);
					createParams.Style |= num & 3145728;
				}
				createParams.Style |= 64;
				switch (this.alignStyle)
				{
				case ListViewAlignment.Left:
					createParams.Style |= 2048;
					break;
				case ListViewAlignment.Top:
				{
					CreateParams createParams2 = createParams;
					createParams2.Style = createParams2.Style;
					break;
				}
				}
				if (this.AutoArrange)
				{
					createParams.Style |= 256;
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
				switch (this.headerStyle)
				{
				case ColumnHeaderStyle.None:
					createParams.Style |= 16384;
					break;
				case ColumnHeaderStyle.Nonclickable:
					createParams.Style |= 32768;
					break;
				}
				if (this.LabelEdit)
				{
					createParams.Style |= 512;
				}
				if (!this.LabelWrap)
				{
					createParams.Style |= 128;
				}
				if (!this.HideSelection)
				{
					createParams.Style |= 8;
				}
				if (!this.MultiSelect)
				{
					createParams.Style |= 4;
				}
				if (this.listItemSorter == null)
				{
					switch (this.sorting)
					{
					case SortOrder.Ascending:
						createParams.Style |= 16;
						break;
					case SortOrder.Descending:
						createParams.Style |= 32;
						break;
					}
				}
				if (this.VirtualMode)
				{
					createParams.Style |= 4096;
				}
				if (this.viewStyle != View.Tile)
				{
					createParams.Style |= (int)this.viewStyle;
				}
				if (this.RightToLeft == RightToLeft.Yes && this.RightToLeftLayout)
				{
					createParams.ExStyle |= 4194304;
					createParams.ExStyle &= -28673;
				}
				return createParams;
			}
		}

		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x06004390 RID: 17296 RVA: 0x000F2A18 File Offset: 0x000F1A18
		internal ListViewGroup DefaultGroup
		{
			get
			{
				if (this.defaultGroup == null)
				{
					this.defaultGroup = new ListViewGroup(SR.GetString("ListViewGroupDefaultGroup", new object[] { "1" }));
				}
				return this.defaultGroup;
			}
		}

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x06004391 RID: 17297 RVA: 0x000F2A58 File Offset: 0x000F1A58
		protected override Size DefaultSize
		{
			get
			{
				return new Size(121, 97);
			}
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x06004392 RID: 17298 RVA: 0x000F2A63 File Offset: 0x000F1A63
		// (set) Token: 0x06004393 RID: 17299 RVA: 0x000F2A6B File Offset: 0x000F1A6B
		protected override bool DoubleBuffered
		{
			get
			{
				return base.DoubleBuffered;
			}
			set
			{
				if (this.DoubleBuffered != value)
				{
					base.DoubleBuffered = value;
					this.UpdateExtendedStyles();
				}
			}
		}

		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x06004394 RID: 17300 RVA: 0x000F2A83 File Offset: 0x000F1A83
		internal bool ExpectingMouseUp
		{
			get
			{
				return this.listViewState[1048576];
			}
		}

		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x06004395 RID: 17301 RVA: 0x000F2A98 File Offset: 0x000F1A98
		// (set) Token: 0x06004396 RID: 17302 RVA: 0x000F2AD2 File Offset: 0x000F1AD2
		[SRDescription("ListViewFocusedItemDescr")]
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ListViewItem FocusedItem
		{
			get
			{
				if (base.IsHandleCreated)
				{
					int num = (int)base.SendMessage(4108, -1, 1);
					if (num > -1)
					{
						return this.Items[num];
					}
				}
				return null;
			}
			set
			{
				if (base.IsHandleCreated && value != null)
				{
					value.Focused = true;
				}
			}
		}

		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x06004397 RID: 17303 RVA: 0x000F2AE6 File Offset: 0x000F1AE6
		// (set) Token: 0x06004398 RID: 17304 RVA: 0x000F2AFC File Offset: 0x000F1AFC
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
				if (base.IsHandleCreated)
				{
					base.SendMessage(4132, 0, ColorTranslator.ToWin32(this.ForeColor));
				}
			}
		}

		// Token: 0x17000D4C RID: 3404
		// (get) Token: 0x06004399 RID: 17305 RVA: 0x000F2B25 File Offset: 0x000F1B25
		// (set) Token: 0x0600439A RID: 17306 RVA: 0x000F2B37 File Offset: 0x000F1B37
		private bool FlipViewToLargeIconAndSmallIcon
		{
			get
			{
				return this.listViewState[268435456];
			}
			set
			{
				this.listViewState[268435456] = value;
			}
		}

		// Token: 0x17000D4D RID: 3405
		// (get) Token: 0x0600439B RID: 17307 RVA: 0x000F2B4A File Offset: 0x000F1B4A
		// (set) Token: 0x0600439C RID: 17308 RVA: 0x000F2B59 File Offset: 0x000F1B59
		[SRDescription("ListViewFullRowSelectDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		public bool FullRowSelect
		{
			get
			{
				return this.listViewState[16];
			}
			set
			{
				if (this.FullRowSelect != value)
				{
					this.listViewState[16] = value;
					this.UpdateExtendedStyles();
				}
			}
		}

		// Token: 0x17000D4E RID: 3406
		// (get) Token: 0x0600439D RID: 17309 RVA: 0x000F2B78 File Offset: 0x000F1B78
		// (set) Token: 0x0600439E RID: 17310 RVA: 0x000F2B87 File Offset: 0x000F1B87
		[SRDescription("ListViewGridLinesDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		public bool GridLines
		{
			get
			{
				return this.listViewState[32];
			}
			set
			{
				if (this.GridLines != value)
				{
					this.listViewState[32] = value;
					this.UpdateExtendedStyles();
				}
			}
		}

		// Token: 0x17000D4F RID: 3407
		// (get) Token: 0x0600439F RID: 17311 RVA: 0x000F2BA6 File Offset: 0x000F1BA6
		[MergableProperty(false)]
		[SRDescription("ListViewGroupsDescr")]
		[Localizable(true)]
		[Editor("System.Windows.Forms.Design.ListViewGroupCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRCategory("CatBehavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ListViewGroupCollection Groups
		{
			get
			{
				if (this.groups == null)
				{
					this.groups = new ListViewGroupCollection(this);
				}
				return this.groups;
			}
		}

		// Token: 0x17000D50 RID: 3408
		// (get) Token: 0x060043A0 RID: 17312 RVA: 0x000F2BC2 File Offset: 0x000F1BC2
		internal bool GroupsEnabled
		{
			get
			{
				return this.ShowGroups && this.groups != null && this.groups.Count > 0 && this.ComctlSupportsVisualStyles && !this.VirtualMode;
			}
		}

		// Token: 0x17000D51 RID: 3409
		// (get) Token: 0x060043A1 RID: 17313 RVA: 0x000F2BF5 File Offset: 0x000F1BF5
		// (set) Token: 0x060043A2 RID: 17314 RVA: 0x000F2C00 File Offset: 0x000F1C00
		[DefaultValue(ColumnHeaderStyle.Clickable)]
		[SRDescription("ListViewHeaderStyleDescr")]
		[SRCategory("CatBehavior")]
		public ColumnHeaderStyle HeaderStyle
		{
			get
			{
				return this.headerStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ColumnHeaderStyle));
				}
				if (this.headerStyle != value)
				{
					this.headerStyle = value;
					if ((this.listViewState[8192] && value == ColumnHeaderStyle.Clickable) || (!this.listViewState[8192] && value == ColumnHeaderStyle.Nonclickable))
					{
						this.listViewState[8192] = !this.listViewState[8192];
						this.RecreateHandleInternal();
						return;
					}
					base.UpdateStyles();
				}
			}
		}

		// Token: 0x17000D52 RID: 3410
		// (get) Token: 0x060043A3 RID: 17315 RVA: 0x000F2C9F File Offset: 0x000F1C9F
		// (set) Token: 0x060043A4 RID: 17316 RVA: 0x000F2CAE File Offset: 0x000F1CAE
		[SRDescription("ListViewHideSelectionDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		public bool HideSelection
		{
			get
			{
				return this.listViewState[64];
			}
			set
			{
				if (this.HideSelection != value)
				{
					this.listViewState[64] = value;
					base.UpdateStyles();
				}
			}
		}

		// Token: 0x17000D53 RID: 3411
		// (get) Token: 0x060043A5 RID: 17317 RVA: 0x000F2CCD File Offset: 0x000F1CCD
		// (set) Token: 0x060043A6 RID: 17318 RVA: 0x000F2CDF File Offset: 0x000F1CDF
		[SRDescription("ListViewHotTrackingDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		public bool HotTracking
		{
			get
			{
				return this.listViewState[128];
			}
			set
			{
				if (this.HotTracking != value)
				{
					this.listViewState[128] = value;
					if (value)
					{
						this.HoverSelection = true;
						this.Activation = ItemActivation.OneClick;
					}
					this.UpdateExtendedStyles();
				}
			}
		}

		// Token: 0x17000D54 RID: 3412
		// (get) Token: 0x060043A7 RID: 17319 RVA: 0x000F2D12 File Offset: 0x000F1D12
		// (set) Token: 0x060043A8 RID: 17320 RVA: 0x000F2D24 File Offset: 0x000F1D24
		[SRDescription("ListViewHoverSelectDescr")]
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		public bool HoverSelection
		{
			get
			{
				return this.listViewState[4096];
			}
			set
			{
				if (this.HoverSelection != value)
				{
					if (this.HotTracking && !value)
					{
						throw new ArgumentException(SR.GetString("ListViewHoverMustBeOnWhenHotTrackingIsOn"), "value");
					}
					this.listViewState[4096] = value;
					this.UpdateExtendedStyles();
				}
			}
		}

		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x060043A9 RID: 17321 RVA: 0x000F2D71 File Offset: 0x000F1D71
		internal bool InsertingItemsNatively
		{
			get
			{
				return this.listViewState1[1];
			}
		}

		// Token: 0x17000D56 RID: 3414
		// (get) Token: 0x060043AA RID: 17322 RVA: 0x000F2D7F File Offset: 0x000F1D7F
		[SRDescription("ListViewInsertionMarkDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public ListViewInsertionMark InsertionMark
		{
			get
			{
				if (this.insertionMark == null)
				{
					this.insertionMark = new ListViewInsertionMark(this);
				}
				return this.insertionMark;
			}
		}

		// Token: 0x17000D57 RID: 3415
		// (get) Token: 0x060043AB RID: 17323 RVA: 0x000F2D9B File Offset: 0x000F1D9B
		// (set) Token: 0x060043AC RID: 17324 RVA: 0x000F2DAD File Offset: 0x000F1DAD
		private bool ItemCollectionChangedInMouseDown
		{
			get
			{
				return this.listViewState[134217728];
			}
			set
			{
				this.listViewState[134217728] = value;
			}
		}

		// Token: 0x17000D58 RID: 3416
		// (get) Token: 0x060043AD RID: 17325 RVA: 0x000F2DC0 File Offset: 0x000F1DC0
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[SRDescription("ListViewItemsDescr")]
		[Localizable(true)]
		[SRCategory("CatBehavior")]
		[Editor("System.Windows.Forms.Design.ListViewItemCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[MergableProperty(false)]
		public ListView.ListViewItemCollection Items
		{
			get
			{
				return this.listItemCollection;
			}
		}

		// Token: 0x17000D59 RID: 3417
		// (get) Token: 0x060043AE RID: 17326 RVA: 0x000F2DC8 File Offset: 0x000F1DC8
		// (set) Token: 0x060043AF RID: 17327 RVA: 0x000F2DDA File Offset: 0x000F1DDA
		[SRDescription("ListViewLabelEditDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		public bool LabelEdit
		{
			get
			{
				return this.listViewState[256];
			}
			set
			{
				if (this.LabelEdit != value)
				{
					this.listViewState[256] = value;
					base.UpdateStyles();
				}
			}
		}

		// Token: 0x17000D5A RID: 3418
		// (get) Token: 0x060043B0 RID: 17328 RVA: 0x000F2DFC File Offset: 0x000F1DFC
		// (set) Token: 0x060043B1 RID: 17329 RVA: 0x000F2E0E File Offset: 0x000F1E0E
		[SRDescription("ListViewLabelWrapDescr")]
		[Localizable(true)]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		public bool LabelWrap
		{
			get
			{
				return this.listViewState[512];
			}
			set
			{
				if (this.LabelWrap != value)
				{
					this.listViewState[512] = value;
					base.UpdateStyles();
				}
			}
		}

		// Token: 0x17000D5B RID: 3419
		// (get) Token: 0x060043B2 RID: 17330 RVA: 0x000F2E30 File Offset: 0x000F1E30
		// (set) Token: 0x060043B3 RID: 17331 RVA: 0x000F2E38 File Offset: 0x000F1E38
		[DefaultValue(null)]
		[SRDescription("ListViewLargeImageListDescr")]
		[SRCategory("CatBehavior")]
		public ImageList LargeImageList
		{
			get
			{
				return this.imageListLarge;
			}
			set
			{
				if (value != this.imageListLarge)
				{
					EventHandler eventHandler = new EventHandler(this.LargeImageListRecreateHandle);
					EventHandler eventHandler2 = new EventHandler(this.DetachImageList);
					EventHandler eventHandler3 = new EventHandler(this.LargeImageListChangedHandle);
					if (this.imageListLarge != null)
					{
						this.imageListLarge.RecreateHandle -= eventHandler;
						this.imageListLarge.Disposed -= eventHandler2;
						this.imageListLarge.ChangeHandle -= eventHandler3;
					}
					this.imageListLarge = value;
					if (value != null)
					{
						value.RecreateHandle += eventHandler;
						value.Disposed += eventHandler2;
						value.ChangeHandle += eventHandler3;
					}
					if (base.IsHandleCreated)
					{
						base.SendMessage(4099, (IntPtr)0, (value == null) ? IntPtr.Zero : value.Handle);
						if (this.AutoArrange && !this.listViewState1[4])
						{
							this.UpdateListViewItemsLocations();
						}
					}
				}
			}
		}

		// Token: 0x17000D5C RID: 3420
		// (get) Token: 0x060043B4 RID: 17332 RVA: 0x000F2F09 File Offset: 0x000F1F09
		// (set) Token: 0x060043B5 RID: 17333 RVA: 0x000F2F1B File Offset: 0x000F1F1B
		internal bool ListViewHandleDestroyed
		{
			get
			{
				return this.listViewState[16777216];
			}
			set
			{
				this.listViewState[16777216] = value;
			}
		}

		// Token: 0x17000D5D RID: 3421
		// (get) Token: 0x060043B6 RID: 17334 RVA: 0x000F2F2E File Offset: 0x000F1F2E
		// (set) Token: 0x060043B7 RID: 17335 RVA: 0x000F2F36 File Offset: 0x000F1F36
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ListViewItemSorterDescr")]
		[SRCategory("CatBehavior")]
		public IComparer ListViewItemSorter
		{
			get
			{
				return this.listItemSorter;
			}
			set
			{
				if (this.listItemSorter != value)
				{
					this.listItemSorter = value;
					if (!this.VirtualMode)
					{
						this.Sort();
					}
				}
			}
		}

		// Token: 0x17000D5E RID: 3422
		// (get) Token: 0x060043B8 RID: 17336 RVA: 0x000F2F56 File Offset: 0x000F1F56
		// (set) Token: 0x060043B9 RID: 17337 RVA: 0x000F2F68 File Offset: 0x000F1F68
		[SRDescription("ListViewMultiSelectDescr")]
		[DefaultValue(true)]
		[SRCategory("CatBehavior")]
		public bool MultiSelect
		{
			get
			{
				return this.listViewState[1024];
			}
			set
			{
				if (this.MultiSelect != value)
				{
					this.listViewState[1024] = value;
					base.UpdateStyles();
				}
			}
		}

		// Token: 0x17000D5F RID: 3423
		// (get) Token: 0x060043BA RID: 17338 RVA: 0x000F2F8A File Offset: 0x000F1F8A
		// (set) Token: 0x060043BB RID: 17339 RVA: 0x000F2F98 File Offset: 0x000F1F98
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ListViewOwnerDrawDescr")]
		public bool OwnerDraw
		{
			get
			{
				return this.listViewState[1];
			}
			set
			{
				if (this.OwnerDraw != value)
				{
					this.listViewState[1] = value;
					base.Invalidate(true);
				}
			}
		}

		// Token: 0x17000D60 RID: 3424
		// (get) Token: 0x060043BC RID: 17340 RVA: 0x000F2FB7 File Offset: 0x000F1FB7
		// (set) Token: 0x060043BD RID: 17341 RVA: 0x000F2FC0 File Offset: 0x000F1FC0
		[SRDescription("ControlRightToLeftLayoutDescr")]
		[Localizable(true)]
		[DefaultValue(false)]
		[SRCategory("CatAppearance")]
		public virtual bool RightToLeftLayout
		{
			get
			{
				return this.rightToLeftLayout;
			}
			set
			{
				if (value != this.rightToLeftLayout)
				{
					this.rightToLeftLayout = value;
					using (new LayoutTransaction(this, this, PropertyNames.RightToLeftLayout))
					{
						this.OnRightToLeftLayoutChanged(EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x14000261 RID: 609
		// (add) Token: 0x060043BE RID: 17342 RVA: 0x000F3014 File Offset: 0x000F2014
		// (remove) Token: 0x060043BF RID: 17343 RVA: 0x000F3027 File Offset: 0x000F2027
		[SRDescription("ControlOnRightToLeftLayoutChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler RightToLeftLayoutChanged
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_RIGHTTOLEFTLAYOUTCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_RIGHTTOLEFTLAYOUTCHANGED, value);
			}
		}

		// Token: 0x17000D61 RID: 3425
		// (get) Token: 0x060043C0 RID: 17344 RVA: 0x000F303A File Offset: 0x000F203A
		// (set) Token: 0x060043C1 RID: 17345 RVA: 0x000F304C File Offset: 0x000F204C
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("ListViewScrollableDescr")]
		public bool Scrollable
		{
			get
			{
				return this.listViewState[2048];
			}
			set
			{
				if (this.Scrollable != value)
				{
					this.listViewState[2048] = value;
					this.RecreateHandleInternal();
				}
			}
		}

		// Token: 0x17000D62 RID: 3426
		// (get) Token: 0x060043C2 RID: 17346 RVA: 0x000F306E File Offset: 0x000F206E
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public ListView.SelectedIndexCollection SelectedIndices
		{
			get
			{
				if (this.selectedIndexCollection == null)
				{
					this.selectedIndexCollection = new ListView.SelectedIndexCollection(this);
				}
				return this.selectedIndexCollection;
			}
		}

		// Token: 0x17000D63 RID: 3427
		// (get) Token: 0x060043C3 RID: 17347 RVA: 0x000F308A File Offset: 0x000F208A
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatAppearance")]
		[SRDescription("ListViewSelectedItemsDescr")]
		public ListView.SelectedListViewItemCollection SelectedItems
		{
			get
			{
				if (this.selectedListViewItemCollection == null)
				{
					this.selectedListViewItemCollection = new ListView.SelectedListViewItemCollection(this);
				}
				return this.selectedListViewItemCollection;
			}
		}

		// Token: 0x17000D64 RID: 3428
		// (get) Token: 0x060043C4 RID: 17348 RVA: 0x000F30A6 File Offset: 0x000F20A6
		// (set) Token: 0x060043C5 RID: 17349 RVA: 0x000F30B8 File Offset: 0x000F20B8
		[SRDescription("ListViewShowGroupsDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		public bool ShowGroups
		{
			get
			{
				return this.listViewState[8388608];
			}
			set
			{
				if (value != this.ShowGroups)
				{
					this.listViewState[8388608] = value;
					if (base.IsHandleCreated)
					{
						this.UpdateGroupView();
					}
				}
			}
		}

		// Token: 0x17000D65 RID: 3429
		// (get) Token: 0x060043C6 RID: 17350 RVA: 0x000F30E2 File Offset: 0x000F20E2
		// (set) Token: 0x060043C7 RID: 17351 RVA: 0x000F30EC File Offset: 0x000F20EC
		[SRDescription("ListViewSmallImageListDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(null)]
		public ImageList SmallImageList
		{
			get
			{
				return this.imageListSmall;
			}
			set
			{
				if (this.imageListSmall != value)
				{
					EventHandler eventHandler = new EventHandler(this.SmallImageListRecreateHandle);
					EventHandler eventHandler2 = new EventHandler(this.DetachImageList);
					if (this.imageListSmall != null)
					{
						this.imageListSmall.RecreateHandle -= eventHandler;
						this.imageListSmall.Disposed -= eventHandler2;
					}
					this.imageListSmall = value;
					if (value != null)
					{
						value.RecreateHandle += eventHandler;
						value.Disposed += eventHandler2;
					}
					if (base.IsHandleCreated)
					{
						base.SendMessage(4099, (IntPtr)1, (value == null) ? IntPtr.Zero : value.Handle);
						if (this.View == View.SmallIcon)
						{
							this.View = View.LargeIcon;
							this.View = View.SmallIcon;
						}
						else if (!this.listViewState1[4])
						{
							this.UpdateListViewItemsLocations();
						}
						if (this.View == View.Details)
						{
							base.Invalidate(true);
						}
					}
				}
			}
		}

		// Token: 0x17000D66 RID: 3430
		// (get) Token: 0x060043C8 RID: 17352 RVA: 0x000F31BE File Offset: 0x000F21BE
		// (set) Token: 0x060043C9 RID: 17353 RVA: 0x000F31D0 File Offset: 0x000F21D0
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ListViewShowItemToolTipsDescr")]
		public bool ShowItemToolTips
		{
			get
			{
				return this.listViewState[32768];
			}
			set
			{
				if (this.ShowItemToolTips != value)
				{
					this.listViewState[32768] = value;
					this.RecreateHandleInternal();
				}
			}
		}

		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x060043CA RID: 17354 RVA: 0x000F31F2 File Offset: 0x000F21F2
		// (set) Token: 0x060043CB RID: 17355 RVA: 0x000F31FC File Offset: 0x000F21FC
		[SRCategory("CatBehavior")]
		[SRDescription("ListViewSortingDescr")]
		[DefaultValue(SortOrder.None)]
		public SortOrder Sorting
		{
			get
			{
				return this.sorting;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(SortOrder));
				}
				if (this.sorting != value)
				{
					this.sorting = value;
					if (this.View == View.LargeIcon || this.View == View.SmallIcon)
					{
						if (this.listItemSorter == null)
						{
							this.listItemSorter = new ListView.IconComparer(this.sorting);
						}
						else if (this.listItemSorter is ListView.IconComparer)
						{
							((ListView.IconComparer)this.listItemSorter).SortOrder = this.sorting;
						}
					}
					else if (value == SortOrder.None)
					{
						this.listItemSorter = null;
					}
					if (value == SortOrder.None)
					{
						base.UpdateStyles();
						return;
					}
					this.RecreateHandleInternal();
				}
			}
		}

		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x060043CC RID: 17356 RVA: 0x000F32AA File Offset: 0x000F22AA
		// (set) Token: 0x060043CD RID: 17357 RVA: 0x000F32B4 File Offset: 0x000F22B4
		[DefaultValue(null)]
		[SRDescription("ListViewStateImageListDescr")]
		[SRCategory("CatBehavior")]
		public ImageList StateImageList
		{
			get
			{
				return this.imageListState;
			}
			set
			{
				if (this.UseCompatibleStateImageBehavior)
				{
					if (this.imageListState != value)
					{
						EventHandler eventHandler = new EventHandler(this.StateImageListRecreateHandle);
						EventHandler eventHandler2 = new EventHandler(this.DetachImageList);
						if (this.imageListState != null)
						{
							this.imageListState.RecreateHandle -= eventHandler;
							this.imageListState.Disposed -= eventHandler2;
						}
						this.imageListState = value;
						if (value != null)
						{
							value.RecreateHandle += eventHandler;
							value.Disposed += eventHandler2;
						}
						if (base.IsHandleCreated)
						{
							base.SendMessage(4099, 2, (value == null) ? IntPtr.Zero : value.Handle);
							return;
						}
					}
				}
				else if (this.imageListState != value)
				{
					EventHandler eventHandler3 = new EventHandler(this.StateImageListRecreateHandle);
					EventHandler eventHandler4 = new EventHandler(this.DetachImageList);
					if (this.imageListState != null)
					{
						this.imageListState.RecreateHandle -= eventHandler3;
						this.imageListState.Disposed -= eventHandler4;
					}
					if (base.IsHandleCreated && this.imageListState != null && this.CheckBoxes)
					{
						base.SendMessage(4099, 2, IntPtr.Zero);
					}
					this.imageListState = value;
					if (value != null)
					{
						value.RecreateHandle += eventHandler3;
						value.Disposed += eventHandler4;
					}
					if (base.IsHandleCreated)
					{
						if (this.CheckBoxes)
						{
							this.RecreateHandleInternal();
						}
						else
						{
							base.SendMessage(4099, 2, (this.imageListState == null || this.imageListState.Images.Count == 0) ? IntPtr.Zero : this.imageListState.Handle);
						}
						if (!this.listViewState1[4])
						{
							this.UpdateListViewItemsLocations();
						}
					}
				}
			}
		}

		// Token: 0x17000D69 RID: 3433
		// (get) Token: 0x060043CE RID: 17358 RVA: 0x000F3440 File Offset: 0x000F2440
		// (set) Token: 0x060043CF RID: 17359 RVA: 0x000F3448 File Offset: 0x000F2448
		[Bindable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		// Token: 0x14000262 RID: 610
		// (add) Token: 0x060043D0 RID: 17360 RVA: 0x000F3451 File Offset: 0x000F2451
		// (remove) Token: 0x060043D1 RID: 17361 RVA: 0x000F345A File Offset: 0x000F245A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x060043D2 RID: 17362 RVA: 0x000F3464 File Offset: 0x000F2464
		// (set) Token: 0x060043D3 RID: 17363 RVA: 0x000F34D4 File Offset: 0x000F24D4
		[SRDescription("ListViewTileSizeDescr")]
		[Browsable(true)]
		[SRCategory("CatAppearance")]
		public Size TileSize
		{
			get
			{
				if (!this.tileSize.IsEmpty)
				{
					return this.tileSize;
				}
				if (base.IsHandleCreated)
				{
					NativeMethods.LVTILEVIEWINFO lvtileviewinfo = new NativeMethods.LVTILEVIEWINFO();
					lvtileviewinfo.dwMask = 1;
					UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4259, 0, lvtileviewinfo);
					return new Size(lvtileviewinfo.sizeTile.cx, lvtileviewinfo.sizeTile.cy);
				}
				return Size.Empty;
			}
			set
			{
				if (this.tileSize != value)
				{
					if (value.IsEmpty || value.Height <= 0 || value.Width <= 0)
					{
						throw new ArgumentOutOfRangeException("TileSize", SR.GetString("ListViewTileSizeMustBePositive"));
					}
					this.tileSize = value;
					if (base.IsHandleCreated)
					{
						NativeMethods.LVTILEVIEWINFO lvtileviewinfo = new NativeMethods.LVTILEVIEWINFO();
						lvtileviewinfo.dwMask = 1;
						lvtileviewinfo.dwFlags = 3;
						lvtileviewinfo.sizeTile = new NativeMethods.SIZE(this.tileSize.Width, this.tileSize.Height);
						UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4258, 0, lvtileviewinfo);
						if (this.AutoArrange)
						{
							this.UpdateListViewItemsLocations();
						}
					}
				}
			}
		}

		// Token: 0x060043D4 RID: 17364 RVA: 0x000F358F File Offset: 0x000F258F
		private bool ShouldSerializeTileSize()
		{
			return !this.tileSize.Equals(Size.Empty);
		}

		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x060043D5 RID: 17365 RVA: 0x000F35B0 File Offset: 0x000F25B0
		// (set) Token: 0x060043D6 RID: 17366 RVA: 0x000F3654 File Offset: 0x000F2654
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[SRCategory("CatAppearance")]
		[SRDescription("ListViewTopItemDescr")]
		public ListViewItem TopItem
		{
			get
			{
				if (this.viewStyle == View.LargeIcon || this.viewStyle == View.SmallIcon || this.viewStyle == View.Tile)
				{
					throw new InvalidOperationException(SR.GetString("ListViewGetTopItem"));
				}
				if (!base.IsHandleCreated)
				{
					if (this.Items.Count > 0)
					{
						return this.Items[0];
					}
					return null;
				}
				else
				{
					this.topIndex = (int)base.SendMessage(4135, 0, 0);
					if (this.topIndex >= 0 && this.topIndex < this.Items.Count)
					{
						return this.Items[this.topIndex];
					}
					return null;
				}
			}
			set
			{
				if (this.viewStyle == View.LargeIcon || this.viewStyle == View.SmallIcon || this.viewStyle == View.Tile)
				{
					throw new InvalidOperationException(SR.GetString("ListViewSetTopItem"));
				}
				if (value == null)
				{
					return;
				}
				if (value.ListView != this)
				{
					return;
				}
				if (!base.IsHandleCreated)
				{
					this.CreateHandle();
				}
				if (value == this.TopItem)
				{
					return;
				}
				this.EnsureVisible(value.Index);
				ListViewItem topItem = this.TopItem;
				if (topItem == null && this.topIndex == this.Items.Count)
				{
					if (this.Scrollable)
					{
						this.EnsureVisible(0);
						this.Scroll(0, value.Index);
					}
					return;
				}
				if (value.Index == topItem.Index)
				{
					return;
				}
				if (this.Scrollable)
				{
					this.Scroll(topItem.Index, value.Index);
				}
			}
		}

		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x060043D7 RID: 17367 RVA: 0x000F3722 File Offset: 0x000F2722
		// (set) Token: 0x060043D8 RID: 17368 RVA: 0x000F3730 File Offset: 0x000F2730
		[Browsable(false)]
		[DefaultValue(true)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool UseCompatibleStateImageBehavior
		{
			get
			{
				return this.listViewState1[8];
			}
			set
			{
				this.listViewState1[8] = value;
			}
		}

		// Token: 0x17000D6D RID: 3437
		// (get) Token: 0x060043D9 RID: 17369 RVA: 0x000F373F File Offset: 0x000F273F
		// (set) Token: 0x060043DA RID: 17370 RVA: 0x000F3748 File Offset: 0x000F2748
		[SRDescription("ListViewViewDescr")]
		[DefaultValue(View.LargeIcon)]
		[SRCategory("CatAppearance")]
		public View View
		{
			get
			{
				return this.viewStyle;
			}
			set
			{
				if (value == View.Tile && this.CheckBoxes)
				{
					throw new NotSupportedException(SR.GetString("ListViewTileViewDoesNotSupportCheckBoxes"));
				}
				this.FlipViewToLargeIconAndSmallIcon = false;
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 4))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(View));
				}
				if (value == View.Tile && this.VirtualMode)
				{
					throw new NotSupportedException(SR.GetString("ListViewCantSetViewToTileViewInVirtualMode"));
				}
				if (this.viewStyle != value)
				{
					this.viewStyle = value;
					if (base.IsHandleCreated && this.ComctlSupportsVisualStyles)
					{
						base.SendMessage(4238, (int)this.viewStyle, 0);
						this.UpdateGroupView();
						if (this.viewStyle == View.Tile)
						{
							this.UpdateTileView();
						}
					}
					else
					{
						base.UpdateStyles();
					}
					this.UpdateListViewItemsLocations();
				}
			}
		}

		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x060043DB RID: 17371 RVA: 0x000F3810 File Offset: 0x000F2810
		// (set) Token: 0x060043DC RID: 17372 RVA: 0x000F3818 File Offset: 0x000F2818
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("ListViewVirtualListSizeDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(0)]
		public int VirtualListSize
		{
			get
			{
				return this.virtualListSize;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(SR.GetString("ListViewVirtualListSizeInvalidArgument", new object[]
					{
						"value",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (value == this.virtualListSize)
				{
					return;
				}
				bool flag = base.IsHandleCreated && this.VirtualMode && this.View == View.Details && !base.DesignMode;
				int num = -1;
				if (flag)
				{
					num = (int)base.SendMessage(4135, 0, 0);
				}
				this.virtualListSize = value;
				if (base.IsHandleCreated && this.VirtualMode && !base.DesignMode)
				{
					base.SendMessage(4143, this.virtualListSize, 0);
				}
				if (flag)
				{
					num = Math.Min(num, this.VirtualListSize - 1);
					if (num > 0)
					{
						ListViewItem listViewItem = this.Items[num];
						this.TopItem = listViewItem;
					}
				}
			}
		}

		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x060043DD RID: 17373 RVA: 0x000F38FC File Offset: 0x000F28FC
		// (set) Token: 0x060043DE RID: 17374 RVA: 0x000F3910 File Offset: 0x000F2910
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRCategory("CatBehavior")]
		[SRDescription("ListViewVirtualModeDescr")]
		[DefaultValue(false)]
		public bool VirtualMode
		{
			get
			{
				return this.listViewState[33554432];
			}
			set
			{
				if (value == this.VirtualMode)
				{
					return;
				}
				if (value && this.Items.Count > 0)
				{
					throw new InvalidOperationException(SR.GetString("ListViewVirtualListViewRequiresNoItems"));
				}
				if (value && this.CheckedItems.Count > 0)
				{
					throw new InvalidOperationException(SR.GetString("ListViewVirtualListViewRequiresNoCheckedItems"));
				}
				if (value && this.SelectedItems.Count > 0)
				{
					throw new InvalidOperationException(SR.GetString("ListViewVirtualListViewRequiresNoSelectedItems"));
				}
				if (value && this.View == View.Tile)
				{
					throw new NotSupportedException(SR.GetString("ListViewCantSetVirtualModeWhenInTileView"));
				}
				this.listViewState[33554432] = value;
				this.RecreateHandleInternal();
			}
		}

		// Token: 0x14000263 RID: 611
		// (add) Token: 0x060043DF RID: 17375 RVA: 0x000F39BD File Offset: 0x000F29BD
		// (remove) Token: 0x060043E0 RID: 17376 RVA: 0x000F39D6 File Offset: 0x000F29D6
		[SRDescription("ListViewAfterLabelEditDescr")]
		[SRCategory("CatBehavior")]
		public event LabelEditEventHandler AfterLabelEdit
		{
			add
			{
				this.onAfterLabelEdit = (LabelEditEventHandler)Delegate.Combine(this.onAfterLabelEdit, value);
			}
			remove
			{
				this.onAfterLabelEdit = (LabelEditEventHandler)Delegate.Remove(this.onAfterLabelEdit, value);
			}
		}

		// Token: 0x14000264 RID: 612
		// (add) Token: 0x060043E1 RID: 17377 RVA: 0x000F39EF File Offset: 0x000F29EF
		// (remove) Token: 0x060043E2 RID: 17378 RVA: 0x000F3A08 File Offset: 0x000F2A08
		[SRDescription("ListViewBeforeLabelEditDescr")]
		[SRCategory("CatBehavior")]
		public event LabelEditEventHandler BeforeLabelEdit
		{
			add
			{
				this.onBeforeLabelEdit = (LabelEditEventHandler)Delegate.Combine(this.onBeforeLabelEdit, value);
			}
			remove
			{
				this.onBeforeLabelEdit = (LabelEditEventHandler)Delegate.Remove(this.onBeforeLabelEdit, value);
			}
		}

		// Token: 0x14000265 RID: 613
		// (add) Token: 0x060043E3 RID: 17379 RVA: 0x000F3A21 File Offset: 0x000F2A21
		// (remove) Token: 0x060043E4 RID: 17380 RVA: 0x000F3A34 File Offset: 0x000F2A34
		[SRDescription("ListViewCacheVirtualItemsEventDescr")]
		[SRCategory("CatAction")]
		public event CacheVirtualItemsEventHandler CacheVirtualItems
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_CACHEVIRTUALITEMS, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_CACHEVIRTUALITEMS, value);
			}
		}

		// Token: 0x14000266 RID: 614
		// (add) Token: 0x060043E5 RID: 17381 RVA: 0x000F3A47 File Offset: 0x000F2A47
		// (remove) Token: 0x060043E6 RID: 17382 RVA: 0x000F3A60 File Offset: 0x000F2A60
		[SRCategory("CatAction")]
		[SRDescription("ListViewColumnClickDescr")]
		public event ColumnClickEventHandler ColumnClick
		{
			add
			{
				this.onColumnClick = (ColumnClickEventHandler)Delegate.Combine(this.onColumnClick, value);
			}
			remove
			{
				this.onColumnClick = (ColumnClickEventHandler)Delegate.Remove(this.onColumnClick, value);
			}
		}

		// Token: 0x14000267 RID: 615
		// (add) Token: 0x060043E7 RID: 17383 RVA: 0x000F3A79 File Offset: 0x000F2A79
		// (remove) Token: 0x060043E8 RID: 17384 RVA: 0x000F3A8C File Offset: 0x000F2A8C
		[SRDescription("ListViewColumnReorderedDscr")]
		[SRCategory("CatPropertyChanged")]
		public event ColumnReorderedEventHandler ColumnReordered
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_COLUMNREORDERED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_COLUMNREORDERED, value);
			}
		}

		// Token: 0x14000268 RID: 616
		// (add) Token: 0x060043E9 RID: 17385 RVA: 0x000F3A9F File Offset: 0x000F2A9F
		// (remove) Token: 0x060043EA RID: 17386 RVA: 0x000F3AB2 File Offset: 0x000F2AB2
		[SRDescription("ListViewColumnWidthChangedDscr")]
		[SRCategory("CatPropertyChanged")]
		public event ColumnWidthChangedEventHandler ColumnWidthChanged
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_COLUMNWIDTHCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_COLUMNWIDTHCHANGED, value);
			}
		}

		// Token: 0x14000269 RID: 617
		// (add) Token: 0x060043EB RID: 17387 RVA: 0x000F3AC5 File Offset: 0x000F2AC5
		// (remove) Token: 0x060043EC RID: 17388 RVA: 0x000F3AD8 File Offset: 0x000F2AD8
		[SRDescription("ListViewColumnWidthChangingDscr")]
		[SRCategory("CatPropertyChanged")]
		public event ColumnWidthChangingEventHandler ColumnWidthChanging
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_COLUMNWIDTHCHANGING, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_COLUMNWIDTHCHANGING, value);
			}
		}

		// Token: 0x1400026A RID: 618
		// (add) Token: 0x060043ED RID: 17389 RVA: 0x000F3AEB File Offset: 0x000F2AEB
		// (remove) Token: 0x060043EE RID: 17390 RVA: 0x000F3AFE File Offset: 0x000F2AFE
		[SRDescription("ListViewDrawColumnHeaderEventDescr")]
		[SRCategory("CatBehavior")]
		public event DrawListViewColumnHeaderEventHandler DrawColumnHeader
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_DRAWCOLUMNHEADER, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_DRAWCOLUMNHEADER, value);
			}
		}

		// Token: 0x1400026B RID: 619
		// (add) Token: 0x060043EF RID: 17391 RVA: 0x000F3B11 File Offset: 0x000F2B11
		// (remove) Token: 0x060043F0 RID: 17392 RVA: 0x000F3B24 File Offset: 0x000F2B24
		[SRDescription("ListViewDrawItemEventDescr")]
		[SRCategory("CatBehavior")]
		public event DrawListViewItemEventHandler DrawItem
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_DRAWITEM, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_DRAWITEM, value);
			}
		}

		// Token: 0x1400026C RID: 620
		// (add) Token: 0x060043F1 RID: 17393 RVA: 0x000F3B37 File Offset: 0x000F2B37
		// (remove) Token: 0x060043F2 RID: 17394 RVA: 0x000F3B4A File Offset: 0x000F2B4A
		[SRDescription("ListViewDrawSubItemEventDescr")]
		[SRCategory("CatBehavior")]
		public event DrawListViewSubItemEventHandler DrawSubItem
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_DRAWSUBITEM, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_DRAWSUBITEM, value);
			}
		}

		// Token: 0x1400026D RID: 621
		// (add) Token: 0x060043F3 RID: 17395 RVA: 0x000F3B5D File Offset: 0x000F2B5D
		// (remove) Token: 0x060043F4 RID: 17396 RVA: 0x000F3B76 File Offset: 0x000F2B76
		[SRCategory("CatAction")]
		[SRDescription("ListViewItemClickDescr")]
		public event EventHandler ItemActivate
		{
			add
			{
				this.onItemActivate = (EventHandler)Delegate.Combine(this.onItemActivate, value);
			}
			remove
			{
				this.onItemActivate = (EventHandler)Delegate.Remove(this.onItemActivate, value);
			}
		}

		// Token: 0x1400026E RID: 622
		// (add) Token: 0x060043F5 RID: 17397 RVA: 0x000F3B8F File Offset: 0x000F2B8F
		// (remove) Token: 0x060043F6 RID: 17398 RVA: 0x000F3BA8 File Offset: 0x000F2BA8
		[SRDescription("CheckedListBoxItemCheckDescr")]
		[SRCategory("CatBehavior")]
		public event ItemCheckEventHandler ItemCheck
		{
			add
			{
				this.onItemCheck = (ItemCheckEventHandler)Delegate.Combine(this.onItemCheck, value);
			}
			remove
			{
				this.onItemCheck = (ItemCheckEventHandler)Delegate.Remove(this.onItemCheck, value);
			}
		}

		// Token: 0x1400026F RID: 623
		// (add) Token: 0x060043F7 RID: 17399 RVA: 0x000F3BC1 File Offset: 0x000F2BC1
		// (remove) Token: 0x060043F8 RID: 17400 RVA: 0x000F3BDA File Offset: 0x000F2BDA
		[SRDescription("ListViewItemCheckedDescr")]
		[SRCategory("CatBehavior")]
		public event ItemCheckedEventHandler ItemChecked
		{
			add
			{
				this.onItemChecked = (ItemCheckedEventHandler)Delegate.Combine(this.onItemChecked, value);
			}
			remove
			{
				this.onItemChecked = (ItemCheckedEventHandler)Delegate.Remove(this.onItemChecked, value);
			}
		}

		// Token: 0x14000270 RID: 624
		// (add) Token: 0x060043F9 RID: 17401 RVA: 0x000F3BF3 File Offset: 0x000F2BF3
		// (remove) Token: 0x060043FA RID: 17402 RVA: 0x000F3C0C File Offset: 0x000F2C0C
		[SRDescription("ListViewItemDragDescr")]
		[SRCategory("CatAction")]
		public event ItemDragEventHandler ItemDrag
		{
			add
			{
				this.onItemDrag = (ItemDragEventHandler)Delegate.Combine(this.onItemDrag, value);
			}
			remove
			{
				this.onItemDrag = (ItemDragEventHandler)Delegate.Remove(this.onItemDrag, value);
			}
		}

		// Token: 0x14000271 RID: 625
		// (add) Token: 0x060043FB RID: 17403 RVA: 0x000F3C25 File Offset: 0x000F2C25
		// (remove) Token: 0x060043FC RID: 17404 RVA: 0x000F3C3E File Offset: 0x000F2C3E
		[SRCategory("CatAction")]
		[SRDescription("ListViewItemMouseHoverDescr")]
		public event ListViewItemMouseHoverEventHandler ItemMouseHover
		{
			add
			{
				this.onItemMouseHover = (ListViewItemMouseHoverEventHandler)Delegate.Combine(this.onItemMouseHover, value);
			}
			remove
			{
				this.onItemMouseHover = (ListViewItemMouseHoverEventHandler)Delegate.Remove(this.onItemMouseHover, value);
			}
		}

		// Token: 0x14000272 RID: 626
		// (add) Token: 0x060043FD RID: 17405 RVA: 0x000F3C57 File Offset: 0x000F2C57
		// (remove) Token: 0x060043FE RID: 17406 RVA: 0x000F3C6A File Offset: 0x000F2C6A
		[SRCategory("CatBehavior")]
		[SRDescription("ListViewItemSelectionChangedDescr")]
		public event ListViewItemSelectionChangedEventHandler ItemSelectionChanged
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_ITEMSELECTIONCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_ITEMSELECTIONCHANGED, value);
			}
		}

		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x060043FF RID: 17407 RVA: 0x000F3C7D File Offset: 0x000F2C7D
		// (set) Token: 0x06004400 RID: 17408 RVA: 0x000F3C85 File Offset: 0x000F2C85
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x14000273 RID: 627
		// (add) Token: 0x06004401 RID: 17409 RVA: 0x000F3C8E File Offset: 0x000F2C8E
		// (remove) Token: 0x06004402 RID: 17410 RVA: 0x000F3C97 File Offset: 0x000F2C97
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x14000274 RID: 628
		// (add) Token: 0x06004403 RID: 17411 RVA: 0x000F3CA0 File Offset: 0x000F2CA0
		// (remove) Token: 0x06004404 RID: 17412 RVA: 0x000F3CA9 File Offset: 0x000F2CA9
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x14000275 RID: 629
		// (add) Token: 0x06004405 RID: 17413 RVA: 0x000F3CB2 File Offset: 0x000F2CB2
		// (remove) Token: 0x06004406 RID: 17414 RVA: 0x000F3CC5 File Offset: 0x000F2CC5
		[SRDescription("ListViewRetrieveVirtualItemEventDescr")]
		[SRCategory("CatAction")]
		public event RetrieveVirtualItemEventHandler RetrieveVirtualItem
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_RETRIEVEVIRTUALITEM, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_RETRIEVEVIRTUALITEM, value);
			}
		}

		// Token: 0x14000276 RID: 630
		// (add) Token: 0x06004407 RID: 17415 RVA: 0x000F3CD8 File Offset: 0x000F2CD8
		// (remove) Token: 0x06004408 RID: 17416 RVA: 0x000F3CEB File Offset: 0x000F2CEB
		[SRCategory("CatAction")]
		[SRDescription("ListViewSearchForVirtualItemDescr")]
		public event SearchForVirtualItemEventHandler SearchForVirtualItem
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_SEARCHFORVIRTUALITEM, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_SEARCHFORVIRTUALITEM, value);
			}
		}

		// Token: 0x14000277 RID: 631
		// (add) Token: 0x06004409 RID: 17417 RVA: 0x000F3CFE File Offset: 0x000F2CFE
		// (remove) Token: 0x0600440A RID: 17418 RVA: 0x000F3D11 File Offset: 0x000F2D11
		[SRDescription("ListViewSelectedIndexChangedDescr")]
		[SRCategory("CatBehavior")]
		public event EventHandler SelectedIndexChanged
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_SELECTEDINDEXCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_SELECTEDINDEXCHANGED, value);
			}
		}

		// Token: 0x14000278 RID: 632
		// (add) Token: 0x0600440B RID: 17419 RVA: 0x000F3D24 File Offset: 0x000F2D24
		// (remove) Token: 0x0600440C RID: 17420 RVA: 0x000F3D37 File Offset: 0x000F2D37
		[SRCategory("CatBehavior")]
		[SRDescription("ListViewVirtualItemsSelectionRangeChangedDescr")]
		public event ListViewVirtualItemsSelectionRangeChangedEventHandler VirtualItemsSelectionRangeChanged
		{
			add
			{
				base.Events.AddHandler(ListView.EVENT_VIRTUALITEMSSELECTIONRANGECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(ListView.EVENT_VIRTUALITEMSSELECTIONRANGECHANGED, value);
			}
		}

		// Token: 0x0600440D RID: 17421 RVA: 0x000F3D4C File Offset: 0x000F2D4C
		private void ApplyUpdateCachedItems()
		{
			ArrayList arrayList = (ArrayList)base.Properties.GetObject(ListView.PropDelayedUpdateItems);
			if (arrayList != null)
			{
				base.Properties.SetObject(ListView.PropDelayedUpdateItems, null);
				ListViewItem[] array = (ListViewItem[])arrayList.ToArray(typeof(ListViewItem));
				if (array.Length > 0)
				{
					this.InsertItems(this.itemCount, array, false);
				}
			}
		}

		// Token: 0x0600440E RID: 17422 RVA: 0x000F3DB0 File Offset: 0x000F2DB0
		public void ArrangeIcons(ListViewAlignment value)
		{
			if (this.viewStyle != View.SmallIcon)
			{
				return;
			}
			switch (value)
			{
			case ListViewAlignment.Default:
			case ListViewAlignment.Left:
			case ListViewAlignment.Top:
			case ListViewAlignment.SnapToGrid:
				if (base.IsHandleCreated)
				{
					UnsafeNativeMethods.PostMessage(new HandleRef(this, base.Handle), 4118, (int)value, 0);
				}
				if (!this.VirtualMode && this.sorting != SortOrder.None)
				{
					this.Sort();
				}
				return;
			}
			throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
			{
				"value",
				value.ToString()
			}));
		}

		// Token: 0x0600440F RID: 17423 RVA: 0x000F3E50 File Offset: 0x000F2E50
		public void ArrangeIcons()
		{
			this.ArrangeIcons(ListViewAlignment.Default);
		}

		// Token: 0x06004410 RID: 17424 RVA: 0x000F3E59 File Offset: 0x000F2E59
		public void AutoResizeColumns(ColumnHeaderAutoResizeStyle headerAutoResize)
		{
			if (!base.IsHandleCreated)
			{
				this.CreateHandle();
			}
			this.UpdateColumnWidths(headerAutoResize);
		}

		// Token: 0x06004411 RID: 17425 RVA: 0x000F3E70 File Offset: 0x000F2E70
		public void AutoResizeColumn(int columnIndex, ColumnHeaderAutoResizeStyle headerAutoResize)
		{
			if (!base.IsHandleCreated)
			{
				this.CreateHandle();
			}
			this.SetColumnWidth(columnIndex, headerAutoResize);
		}

		// Token: 0x06004412 RID: 17426 RVA: 0x000F3E88 File Offset: 0x000F2E88
		public void BeginUpdate()
		{
			base.BeginUpdateInternal();
			if (this.updateCounter++ == 0 && base.Properties.GetObject(ListView.PropDelayedUpdateItems) == null)
			{
				base.Properties.SetObject(ListView.PropDelayedUpdateItems, new ArrayList());
			}
		}

		// Token: 0x06004413 RID: 17427 RVA: 0x000F3ED8 File Offset: 0x000F2ED8
		internal void CacheSelectedStateForItem(ListViewItem lvi, bool selected)
		{
			if (selected)
			{
				if (this.savedSelectedItems == null)
				{
					this.savedSelectedItems = new List<ListViewItem>();
				}
				if (!this.savedSelectedItems.Contains(lvi))
				{
					this.savedSelectedItems.Add(lvi);
					return;
				}
			}
			else if (this.savedSelectedItems != null && this.savedSelectedItems.Contains(lvi))
			{
				this.savedSelectedItems.Remove(lvi);
			}
		}

		// Token: 0x06004414 RID: 17428 RVA: 0x000F3F3C File Offset: 0x000F2F3C
		private void CleanPreviousBackgroundImageFiles()
		{
			if (this.bkImgFileNames == null)
			{
				return;
			}
			FileIOPermission fileIOPermission = new FileIOPermission(PermissionState.Unrestricted);
			fileIOPermission.Assert();
			try
			{
				for (int i = 0; i <= this.bkImgFileNamesCount; i++)
				{
					FileInfo fileInfo = new FileInfo(this.bkImgFileNames[i]);
					if (fileInfo.Exists)
					{
						try
						{
							fileInfo.Delete();
						}
						catch (IOException)
						{
						}
					}
				}
			}
			finally
			{
				PermissionSet.RevertAssert();
			}
			this.bkImgFileNames = null;
			this.bkImgFileNamesCount = -1;
		}

		// Token: 0x06004415 RID: 17429 RVA: 0x000F3FC4 File Offset: 0x000F2FC4
		public void Clear()
		{
			this.Items.Clear();
			this.Columns.Clear();
		}

		// Token: 0x06004416 RID: 17430 RVA: 0x000F3FDC File Offset: 0x000F2FDC
		private int CompareFunc(IntPtr lparam1, IntPtr lparam2, IntPtr lparamSort)
		{
			if (this.listItemSorter != null)
			{
				return this.listItemSorter.Compare(this.listItemsTable[(int)lparam1], this.listItemsTable[(int)lparam2]);
			}
			return 0;
		}

		// Token: 0x06004417 RID: 17431 RVA: 0x000F402C File Offset: 0x000F302C
		private int CompensateColumnHeaderResize(Message m, bool columnResizeCancelled)
		{
			if (this.ComctlSupportsVisualStyles && this.View == View.Details && !columnResizeCancelled && this.Items.Count > 0)
			{
				NativeMethods.NMHEADER nmheader = (NativeMethods.NMHEADER)m.GetLParam(typeof(NativeMethods.NMHEADER));
				return this.CompensateColumnHeaderResize(nmheader.iItem, columnResizeCancelled);
			}
			return 0;
		}

		// Token: 0x06004418 RID: 17432 RVA: 0x000F4084 File Offset: 0x000F3084
		private int CompensateColumnHeaderResize(int columnIndex, bool columnResizeCancelled)
		{
			if (this.ComctlSupportsVisualStyles && this.View == View.Details && !columnResizeCancelled && this.Items.Count > 0 && columnIndex == 0)
			{
				ColumnHeader columnHeader = ((this.columnHeaders != null && this.columnHeaders.Length > 0) ? this.columnHeaders[0] : null);
				if (columnHeader != null)
				{
					if (this.SmallImageList == null)
					{
						return 2;
					}
					bool flag = true;
					for (int i = 0; i < this.Items.Count; i++)
					{
						if (this.Items[i].ImageIndexer.ActualIndex > -1)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return 18;
					}
				}
			}
			return 0;
		}

		// Token: 0x06004419 RID: 17433 RVA: 0x000F4124 File Offset: 0x000F3124
		protected override void CreateHandle()
		{
			/*
An exception occurred when decompiling this method (06004419)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Windows.Forms.ListView::CreateHandle()

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at dnlib.DotNet.Extensions.ToTypeSig(ITypeDefOrRef type, Boolean resolveToCheckValueType)
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.DoInferTypeForExpression(ILExpression expr, TypeSig expectedType, Boolean forceInferChildren) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 440
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.InferTypeForExpression(ILExpression expr, TypeSig expectedType, Boolean forceInferChildren) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 309
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.DoInferTypeForExpression(ILExpression expr, TypeSig expectedType, Boolean forceInferChildren) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 444
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.InferTypeForExpression(ILExpression expr, TypeSig expectedType, Boolean forceInferChildren) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 309
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.DoInferTypeForExpression(ILExpression expr, TypeSig expectedType, Boolean forceInferChildren) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 389
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.InferTypeForExpression(ILExpression expr, TypeSig expectedType, Boolean forceInferChildren) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 309
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.RunInference(ILExpression expr) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 284
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.RunInference() in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 220
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.Run(DecompilerContext context, ILBlock method) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 49
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 417
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x0600441A RID: 17434 RVA: 0x000F4184 File Offset: 0x000F3184
		private unsafe void CustomDraw(ref Message m)
		{
			bool flag = false;
			bool flag2 = false;
			try
			{
				NativeMethods.NMLVCUSTOMDRAW* ptr = (NativeMethods.NMLVCUSTOMDRAW*)(void*)m.LParam;
				int dwDrawStage = ptr->nmcd.dwDrawStage;
				if (dwDrawStage != 1)
				{
					int num;
					Rectangle rectangle;
					if (dwDrawStage != 65537)
					{
						if (dwDrawStage != 196609)
						{
							m.Result = (IntPtr)0;
							return;
						}
					}
					else
					{
						num = (int)ptr->nmcd.dwItemSpec;
						rectangle = this.GetItemRectOrEmpty(num);
						if (!base.ClientRectangle.IntersectsWith(rectangle))
						{
							return;
						}
						if (this.OwnerDraw)
						{
							Graphics graphics = Graphics.FromHdcInternal(ptr->nmcd.hdc);
							DrawListViewItemEventArgs drawListViewItemEventArgs = null;
							try
							{
								drawListViewItemEventArgs = new DrawListViewItemEventArgs(graphics, this.Items[(int)ptr->nmcd.dwItemSpec], rectangle, (int)ptr->nmcd.dwItemSpec, (ListViewItemStates)ptr->nmcd.uItemState);
								this.OnDrawItem(drawListViewItemEventArgs);
							}
							finally
							{
								graphics.Dispose();
							}
							flag2 = drawListViewItemEventArgs.DrawDefault;
							if (this.viewStyle == View.Details)
							{
								m.Result = (IntPtr)32;
							}
							else if (!drawListViewItemEventArgs.DrawDefault)
							{
								m.Result = (IntPtr)4;
							}
							if (!drawListViewItemEventArgs.DrawDefault)
							{
								return;
							}
						}
						if (this.viewStyle == View.Details)
						{
							m.Result = (IntPtr)34;
							flag = true;
						}
					}
					num = (int)ptr->nmcd.dwItemSpec;
					rectangle = this.GetItemRectOrEmpty(num);
					if (base.ClientRectangle.IntersectsWith(rectangle))
					{
						if (this.OwnerDraw && !flag2)
						{
							Graphics graphics2 = Graphics.FromHdcInternal(ptr->nmcd.hdc);
							bool flag3 = true;
							try
							{
								if (ptr->iSubItem < this.Items[num].SubItems.Count)
								{
									Rectangle subItemRect = this.GetSubItemRect(num, ptr->iSubItem);
									if (ptr->iSubItem == 0 && this.Items[num].SubItems.Count > 1)
									{
										subItemRect.Width = this.columnHeaders[0].Width;
									}
									if (base.ClientRectangle.IntersectsWith(subItemRect))
									{
										DrawListViewSubItemEventArgs drawListViewSubItemEventArgs = new DrawListViewSubItemEventArgs(graphics2, subItemRect, this.Items[num], this.Items[num].SubItems[ptr->iSubItem], num, ptr->iSubItem, this.columnHeaders[ptr->iSubItem], (ListViewItemStates)ptr->nmcd.uItemState);
										this.OnDrawSubItem(drawListViewSubItemEventArgs);
										flag3 = !drawListViewSubItemEventArgs.DrawDefault;
									}
								}
							}
							finally
							{
								graphics2.Dispose();
							}
							if (flag3)
							{
								m.Result = (IntPtr)4;
								return;
							}
						}
						ListViewItem listViewItem = this.Items[(int)ptr->nmcd.dwItemSpec];
						if (flag && listViewItem.UseItemStyleForSubItems)
						{
							m.Result = (IntPtr)2;
						}
						int num2 = ptr->nmcd.uItemState;
						if (!this.HideSelection)
						{
							int itemState = this.GetItemState((int)ptr->nmcd.dwItemSpec);
							if ((itemState & 2) == 0)
							{
								num2 &= -2;
							}
						}
						int num3 = (((ptr->nmcd.dwDrawStage & 131072) != 0) ? ptr->iSubItem : 0);
						Font font = null;
						Color color = Color.Empty;
						Color color2 = Color.Empty;
						bool flag4 = false;
						bool flag5 = false;
						if (listViewItem != null && num3 < listViewItem.SubItems.Count)
						{
							flag4 = true;
							if (num3 == 0 && (num2 & 64) != 0 && this.HotTracking)
							{
								flag5 = true;
								font = new Font(listViewItem.SubItems[0].Font, FontStyle.Underline);
							}
							else
							{
								font = listViewItem.SubItems[num3].Font;
							}
							if (num3 > 0 || (num2 & 71) == 0)
							{
								color = listViewItem.SubItems[num3].ForeColor;
								color2 = listViewItem.SubItems[num3].BackColor;
							}
						}
						Color color3 = Color.Empty;
						Color color4 = Color.Empty;
						if (flag4)
						{
							color3 = color;
							color4 = color2;
						}
						bool flag6 = true;
						if (!base.Enabled)
						{
							flag6 = false;
						}
						else if ((this.activation == ItemActivation.OneClick || this.activation == ItemActivation.TwoClick) && (num2 & 71) != 0)
						{
							flag6 = false;
						}
						if (flag6)
						{
							if (!flag4 || color3.IsEmpty)
							{
								ptr->clrText = ColorTranslator.ToWin32(this.odCacheForeColor);
							}
							else
							{
								ptr->clrText = ColorTranslator.ToWin32(color3);
							}
							if (ptr->clrText == ColorTranslator.ToWin32(SystemColors.HotTrack))
							{
								int num4 = 0;
								bool flag7 = false;
								int num5 = 16711680;
								do
								{
									int num6 = ptr->clrText & num5;
									if (num6 != 0 || num5 == 255)
									{
										int num7 = 16 - num4;
										if (num6 == num5)
										{
											num6 = (num6 >> num7) - 1 << num7;
										}
										else
										{
											num6 = (num6 >> num7) + 1 << num7;
										}
										ptr->clrText = (ptr->clrText & ~num5) | num6;
										flag7 = true;
									}
									else
									{
										num5 >>= 8;
										num4 += 8;
									}
								}
								while (!flag7);
							}
							if (!flag4 || color4.IsEmpty)
							{
								ptr->clrTextBk = ColorTranslator.ToWin32(this.odCacheBackColor);
							}
							else
							{
								ptr->clrTextBk = ColorTranslator.ToWin32(color4);
							}
						}
						if (!flag4 || font == null)
						{
							if (this.odCacheFont != null)
							{
								SafeNativeMethods.SelectObject(new HandleRef(ptr->nmcd, ptr->nmcd.hdc), new HandleRef(null, this.odCacheFontHandle));
							}
						}
						else
						{
							if (this.odCacheFontHandleWrapper != null)
							{
								this.odCacheFontHandleWrapper.Dispose();
							}
							this.odCacheFontHandleWrapper = new Control.FontHandleWrapper(font);
							SafeNativeMethods.SelectObject(new HandleRef(ptr->nmcd, ptr->nmcd.hdc), new HandleRef(this.odCacheFontHandleWrapper, this.odCacheFontHandleWrapper.Handle));
						}
						if (!flag)
						{
							m.Result = (IntPtr)2;
						}
						if (flag5)
						{
							font.Dispose();
						}
					}
				}
				else if (this.OwnerDraw)
				{
					m.Result = (IntPtr)32;
				}
				else
				{
					m.Result = (IntPtr)34;
					this.odCacheBackColor = this.BackColor;
					this.odCacheForeColor = this.ForeColor;
					this.odCacheFont = this.Font;
					this.odCacheFontHandle = base.FontHandle;
					if (ptr->dwItemType == 1)
					{
						if (this.odCacheFontHandleWrapper != null)
						{
							this.odCacheFontHandleWrapper.Dispose();
						}
						this.odCacheFont = new Font(this.odCacheFont, FontStyle.Bold);
						this.odCacheFontHandleWrapper = new Control.FontHandleWrapper(this.odCacheFont);
						this.odCacheFontHandle = this.odCacheFontHandleWrapper.Handle;
						SafeNativeMethods.SelectObject(new HandleRef(ptr->nmcd, ptr->nmcd.hdc), new HandleRef(this.odCacheFontHandleWrapper, this.odCacheFontHandleWrapper.Handle));
						m.Result = (IntPtr)2;
					}
				}
			}
			catch (Exception)
			{
				m.Result = (IntPtr)0;
			}
		}

		// Token: 0x0600441B RID: 17435 RVA: 0x000F48B4 File Offset: 0x000F38B4
		private void DeleteFileName(string fileName)
		{
			if (!string.IsNullOrEmpty(fileName))
			{
				FileIOPermission fileIOPermission = new FileIOPermission(PermissionState.Unrestricted);
				fileIOPermission.Assert();
				try
				{
					FileInfo fileInfo = new FileInfo(fileName);
					if (fileInfo.Exists)
					{
						try
						{
							fileInfo.Delete();
						}
						catch (IOException)
						{
						}
					}
				}
				finally
				{
					PermissionSet.RevertAssert();
				}
			}
		}

		// Token: 0x0600441C RID: 17436 RVA: 0x000F4914 File Offset: 0x000F3914
		private void DestroyLVGROUP(NativeMethods.LVGROUP lvgroup)
		{
			if (lvgroup.pszHeader != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(lvgroup.pszHeader);
			}
		}

		// Token: 0x0600441D RID: 17437 RVA: 0x000F4934 File Offset: 0x000F3934
		private void DetachImageList(object sender, EventArgs e)
		{
			this.listViewState1[4] = true;
			try
			{
				if (sender == this.imageListSmall)
				{
					this.SmallImageList = null;
				}
				if (sender == this.imageListLarge)
				{
					this.LargeImageList = null;
				}
				if (sender == this.imageListState)
				{
					this.StateImageList = null;
				}
			}
			finally
			{
				this.listViewState1[4] = false;
			}
			this.UpdateListViewItemsLocations();
		}

		// Token: 0x0600441E RID: 17438 RVA: 0x000F49A4 File Offset: 0x000F39A4
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.imageListSmall != null)
				{
					this.imageListSmall.Disposed -= this.DetachImageList;
					this.imageListSmall = null;
				}
				if (this.imageListLarge != null)
				{
					this.imageListLarge.Disposed -= this.DetachImageList;
					this.imageListLarge = null;
				}
				if (this.imageListState != null)
				{
					this.imageListState.Disposed -= this.DetachImageList;
					this.imageListState = null;
				}
				if (this.columnHeaders != null)
				{
					for (int i = this.columnHeaders.Length - 1; i >= 0; i--)
					{
						this.columnHeaders[i].OwnerListview = null;
						this.columnHeaders[i].Dispose();
					}
					this.columnHeaders = null;
				}
				this.Items.Clear();
				if (this.odCacheFontHandleWrapper != null)
				{
					this.odCacheFontHandleWrapper.Dispose();
					this.odCacheFontHandleWrapper = null;
				}
				if (!string.IsNullOrEmpty(this.backgroundImageFileName) || this.bkImgFileNames != null)
				{
					FileIOPermission fileIOPermission = new FileIOPermission(PermissionState.Unrestricted);
					fileIOPermission.Assert();
					try
					{
						if (!string.IsNullOrEmpty(this.backgroundImageFileName))
						{
							FileInfo fileInfo = new FileInfo(this.backgroundImageFileName);
							try
							{
								fileInfo.Delete();
							}
							catch (IOException)
							{
							}
							this.backgroundImageFileName = string.Empty;
						}
						for (int j = 0; j <= this.bkImgFileNamesCount; j++)
						{
							FileInfo fileInfo = new FileInfo(this.bkImgFileNames[j]);
							try
							{
								fileInfo.Delete();
							}
							catch (IOException)
							{
							}
						}
						this.bkImgFileNames = null;
						this.bkImgFileNamesCount = -1;
					}
					finally
					{
						PermissionSet.RevertAssert();
					}
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600441F RID: 17439 RVA: 0x000F4B50 File Offset: 0x000F3B50
		public void EndUpdate()
		{
			if (--this.updateCounter == 0 && base.Properties.GetObject(ListView.PropDelayedUpdateItems) != null)
			{
				this.ApplyUpdateCachedItems();
			}
			base.EndUpdateInternal();
		}

		// Token: 0x06004420 RID: 17440 RVA: 0x000F4B90 File Offset: 0x000F3B90
		private void EnsureDefaultGroup()
		{
			if (base.IsHandleCreated && this.ComctlSupportsVisualStyles && this.GroupsEnabled && base.SendMessage(4257, this.DefaultGroup.ID, 0) == IntPtr.Zero)
			{
				this.UpdateGroupView();
				this.InsertGroupNative(0, this.DefaultGroup);
			}
		}

		// Token: 0x06004421 RID: 17441 RVA: 0x000F4BEC File Offset: 0x000F3BEC
		public void EnsureVisible(int index)
		{
			if (index < 0 || index >= this.Items.Count)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (base.IsHandleCreated)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4115, index, 0);
			}
		}

		// Token: 0x06004422 RID: 17442 RVA: 0x000F4C60 File Offset: 0x000F3C60
		public ListViewItem FindItemWithText(string text)
		{
			if (this.Items.Count == 0)
			{
				return null;
			}
			return this.FindItemWithText(text, true, 0, true);
		}

		// Token: 0x06004423 RID: 17443 RVA: 0x000F4C7B File Offset: 0x000F3C7B
		public ListViewItem FindItemWithText(string text, bool includeSubItemsInSearch, int startIndex)
		{
			return this.FindItemWithText(text, includeSubItemsInSearch, startIndex, true);
		}

		// Token: 0x06004424 RID: 17444 RVA: 0x000F4C88 File Offset: 0x000F3C88
		public ListViewItem FindItemWithText(string text, bool includeSubItemsInSearch, int startIndex, bool isPrefixSearch)
		{
			if (startIndex < 0 || startIndex >= this.Items.Count)
			{
				throw new ArgumentOutOfRangeException("startIndex", SR.GetString("InvalidArgument", new object[]
				{
					"startIndex",
					startIndex.ToString(CultureInfo.CurrentCulture)
				}));
			}
			return this.FindItem(true, text, isPrefixSearch, new Point(0, 0), SearchDirectionHint.Down, startIndex, includeSubItemsInSearch);
		}

		// Token: 0x06004425 RID: 17445 RVA: 0x000F4CF0 File Offset: 0x000F3CF0
		public ListViewItem FindNearestItem(SearchDirectionHint dir, Point point)
		{
			return this.FindNearestItem(dir, point.X, point.Y);
		}

		// Token: 0x06004426 RID: 17446 RVA: 0x000F4D08 File Offset: 0x000F3D08
		public ListViewItem FindNearestItem(SearchDirectionHint searchDirection, int x, int y)
		{
			if (this.View != View.SmallIcon && this.View != View.LargeIcon)
			{
				throw new InvalidOperationException(SR.GetString("ListViewFindNearestItemWorksOnlyInIconView"));
			}
			if (searchDirection < SearchDirectionHint.Left || searchDirection > SearchDirectionHint.Down)
			{
				throw new ArgumentOutOfRangeException("searchDirection", SR.GetString("InvalidArgument", new object[]
				{
					"searchDirection",
					searchDirection.ToString()
				}));
			}
			ListViewItem itemAt = this.GetItemAt(x, y);
			if (itemAt != null)
			{
				Rectangle bounds = itemAt.Bounds;
				Rectangle itemRect = this.GetItemRect(itemAt.Index, ItemBoundsPortion.Icon);
				switch (searchDirection)
				{
				case SearchDirectionHint.Left:
					x = Math.Max(bounds.Left, itemRect.Left) - 1;
					break;
				case SearchDirectionHint.Up:
					y = Math.Max(bounds.Top, itemRect.Top) - 1;
					break;
				case SearchDirectionHint.Right:
					x = Math.Max(bounds.Left, itemRect.Left) + 1;
					break;
				case SearchDirectionHint.Down:
					y = Math.Max(bounds.Top, itemRect.Top) + 1;
					break;
				}
			}
			return this.FindItem(false, string.Empty, false, new Point(x, y), searchDirection, -1, false);
		}

		// Token: 0x06004427 RID: 17447 RVA: 0x000F4E30 File Offset: 0x000F3E30
		private ListViewItem FindItem(bool isTextSearch, string text, bool isPrefixSearch, Point pt, SearchDirectionHint dir, int startIndex, bool includeSubItemsInSearch)
		{
			if (this.Items.Count == 0)
			{
				return null;
			}
			if (!base.IsHandleCreated)
			{
				this.CreateHandle();
			}
			if (this.VirtualMode)
			{
				SearchForVirtualItemEventArgs searchForVirtualItemEventArgs = new SearchForVirtualItemEventArgs(isTextSearch, isPrefixSearch, includeSubItemsInSearch, text, pt, dir, startIndex);
				this.OnSearchForVirtualItem(searchForVirtualItemEventArgs);
				if (searchForVirtualItemEventArgs.Index != -1)
				{
					return this.Items[searchForVirtualItemEventArgs.Index];
				}
				return null;
			}
			else
			{
				NativeMethods.LVFINDINFO lvfindinfo = default(NativeMethods.LVFINDINFO);
				if (isTextSearch)
				{
					lvfindinfo.flags = 2;
					lvfindinfo.flags |= (isPrefixSearch ? 8 : 0);
					lvfindinfo.psz = text;
				}
				else
				{
					lvfindinfo.flags = 64;
					lvfindinfo.ptX = pt.X;
					lvfindinfo.ptY = pt.Y;
					lvfindinfo.vkDirection = (int)dir;
				}
				lvfindinfo.lParam = IntPtr.Zero;
				int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.LVM_FINDITEM, startIndex - 1, ref lvfindinfo);
				if (num >= 0)
				{
					return this.Items[num];
				}
				if (isTextSearch && includeSubItemsInSearch)
				{
					for (int i = startIndex; i < this.Items.Count; i++)
					{
						ListViewItem listViewItem = this.Items[i];
						for (int j = 0; j < listViewItem.SubItems.Count; j++)
						{
							ListViewItem.ListViewSubItem listViewSubItem = listViewItem.SubItems[j];
							if (string.Equals(text, listViewSubItem.Text, StringComparison.OrdinalIgnoreCase))
							{
								return listViewItem;
							}
							if (isPrefixSearch && CultureInfo.CurrentCulture.CompareInfo.IsPrefix(listViewSubItem.Text, text, CompareOptions.IgnoreCase))
							{
								return listViewItem;
							}
						}
					}
					return null;
				}
				return null;
			}
		}

		// Token: 0x06004428 RID: 17448 RVA: 0x000F4FCC File Offset: 0x000F3FCC
		private void ForceCheckBoxUpdate()
		{
			if (this.CheckBoxes && base.IsHandleCreated)
			{
				base.SendMessage(4150, 4, 0);
				base.SendMessage(4150, 4, 4);
				if (this.AutoArrange)
				{
					this.ArrangeIcons(this.Alignment);
				}
			}
		}

		// Token: 0x06004429 RID: 17449 RVA: 0x000F501C File Offset: 0x000F401C
		private string GenerateRandomName()
		{
			Bitmap bitmap = new Bitmap(this.BackgroundImage);
			int num = 0;
			try
			{
				num = (int)(long)bitmap.GetHicon();
			}
			catch
			{
				bitmap.Dispose();
			}
			Random random;
			if (num == 0)
			{
				random = new Random((int)DateTime.Now.Ticks);
			}
			else
			{
				random = new Random(num);
			}
			return random.Next().ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x0600442A RID: 17450 RVA: 0x000F5094 File Offset: 0x000F4094
		private int GenerateUniqueID()
		{
			int num = this.nextID++;
			if (num == -1)
			{
				num = 0;
				this.nextID = 1;
			}
			return num;
		}

		// Token: 0x0600442B RID: 17451 RVA: 0x000F50C4 File Offset: 0x000F40C4
		internal int GetDisplayIndex(ListViewItem item, int lastIndex)
		{
			this.ApplyUpdateCachedItems();
			if (base.IsHandleCreated && !this.ListViewHandleDestroyed)
			{
				NativeMethods.LVFINDINFO lvfindinfo = default(NativeMethods.LVFINDINFO);
				lvfindinfo.lParam = (IntPtr)item.ID;
				lvfindinfo.flags = 1;
				int num = -1;
				if (lastIndex != -1)
				{
					num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.LVM_FINDITEM, lastIndex - 1, ref lvfindinfo);
				}
				if (num == -1)
				{
					num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.LVM_FINDITEM, -1, ref lvfindinfo);
				}
				return num;
			}
			int num2 = 0;
			foreach (object obj in this.listItemsArray)
			{
				if (obj == item)
				{
					return num2;
				}
				num2++;
			}
			return -1;
		}

		// Token: 0x0600442C RID: 17452 RVA: 0x000F51B0 File Offset: 0x000F41B0
		internal int GetColumnIndex(ColumnHeader ch)
		{
			if (this.columnHeaders == null)
			{
				return -1;
			}
			for (int i = 0; i < this.columnHeaders.Length; i++)
			{
				if (this.columnHeaders[i] == ch)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600442D RID: 17453 RVA: 0x000F51E8 File Offset: 0x000F41E8
		public ListViewItem GetItemAt(int x, int y)
		{
			NativeMethods.LVHITTESTINFO lvhittestinfo = new NativeMethods.LVHITTESTINFO();
			lvhittestinfo.pt_x = x;
			lvhittestinfo.pt_y = y;
			int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4114, 0, lvhittestinfo);
			ListViewItem listViewItem = null;
			if (num >= 0 && (lvhittestinfo.flags & 14) != 0)
			{
				listViewItem = this.Items[num];
			}
			return listViewItem;
		}

		// Token: 0x0600442E RID: 17454 RVA: 0x000F5246 File Offset: 0x000F4246
		internal int GetNativeGroupId(ListViewItem item)
		{
			item.UpdateGroupFromName();
			if (item.Group != null && this.Groups.Contains(item.Group))
			{
				return item.Group.ID;
			}
			this.EnsureDefaultGroup();
			return this.DefaultGroup.ID;
		}

		// Token: 0x0600442F RID: 17455 RVA: 0x000F5288 File Offset: 0x000F4288
		internal void GetSubItemAt(int x, int y, out int iItem, out int iSubItem)
		{
			NativeMethods.LVHITTESTINFO lvhittestinfo = new NativeMethods.LVHITTESTINFO();
			lvhittestinfo.pt_x = x;
			lvhittestinfo.pt_y = y;
			int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4153, 0, lvhittestinfo);
			if (num > -1)
			{
				iItem = lvhittestinfo.iItem;
				iSubItem = lvhittestinfo.iSubItem;
				return;
			}
			iItem = -1;
			iSubItem = -1;
		}

		// Token: 0x06004430 RID: 17456 RVA: 0x000F52E4 File Offset: 0x000F42E4
		internal Point GetItemPosition(int index)
		{
			NativeMethods.POINT point = new NativeMethods.POINT();
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4112, index, point);
			return new Point(point.x, point.y);
		}

		// Token: 0x06004431 RID: 17457 RVA: 0x000F5321 File Offset: 0x000F4321
		internal int GetItemState(int index)
		{
			return this.GetItemState(index, 65295);
		}

		// Token: 0x06004432 RID: 17458 RVA: 0x000F5330 File Offset: 0x000F4330
		internal int GetItemState(int index, int mask)
		{
			if (index < 0 || (this.VirtualMode && index >= this.VirtualListSize) || (!this.VirtualMode && index >= this.itemCount))
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			return (int)base.SendMessage(4140, index, mask);
		}

		// Token: 0x06004433 RID: 17459 RVA: 0x000F53A9 File Offset: 0x000F43A9
		public Rectangle GetItemRect(int index)
		{
			return this.GetItemRect(index, ItemBoundsPortion.Entire);
		}

		// Token: 0x06004434 RID: 17460 RVA: 0x000F53B4 File Offset: 0x000F43B4
		public Rectangle GetItemRect(int index, ItemBoundsPortion portion)
		{
			if (index < 0 || index >= this.Items.Count)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (!ClientUtils.IsEnumValid(portion, (int)portion, 0, 3))
			{
				throw new InvalidEnumArgumentException("portion", (int)portion, typeof(ItemBoundsPortion));
			}
			if (this.View == View.Details && this.Columns.Count == 0)
			{
				return Rectangle.Empty;
			}
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			rect.left = (int)portion;
			if ((int)base.SendMessage(4110, index, ref rect) == 0)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
		}

		// Token: 0x06004435 RID: 17461 RVA: 0x000F54C0 File Offset: 0x000F44C0
		private Rectangle GetItemRectOrEmpty(int index)
		{
			if (index < 0 || index >= this.Items.Count)
			{
				return Rectangle.Empty;
			}
			if (this.View == View.Details && this.Columns.Count == 0)
			{
				return Rectangle.Empty;
			}
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			rect.left = 0;
			if ((int)base.SendMessage(4110, index, ref rect) == 0)
			{
				return Rectangle.Empty;
			}
			return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
		}

		// Token: 0x06004436 RID: 17462 RVA: 0x000F5550 File Offset: 0x000F4550
		private NativeMethods.LVGROUP GetLVGROUP(ListViewGroup group)
		{
			NativeMethods.LVGROUP lvgroup = new NativeMethods.LVGROUP();
			lvgroup.mask = 25U;
			string header = group.Header;
			lvgroup.pszHeader = Marshal.StringToHGlobalAuto(header);
			lvgroup.cchHeader = header.Length;
			lvgroup.iGroupId = group.ID;
			switch (group.HeaderAlignment)
			{
			case HorizontalAlignment.Left:
				lvgroup.uAlign = 1U;
				break;
			case HorizontalAlignment.Right:
				lvgroup.uAlign = 4U;
				break;
			case HorizontalAlignment.Center:
				lvgroup.uAlign = 2U;
				break;
			}
			return lvgroup;
		}

		// Token: 0x06004437 RID: 17463 RVA: 0x000F55CB File Offset: 0x000F45CB
		internal Rectangle GetSubItemRect(int itemIndex, int subItemIndex)
		{
			return this.GetSubItemRect(itemIndex, subItemIndex, ItemBoundsPortion.Entire);
		}

		// Token: 0x06004438 RID: 17464 RVA: 0x000F55D8 File Offset: 0x000F45D8
		internal Rectangle GetSubItemRect(int itemIndex, int subItemIndex, ItemBoundsPortion portion)
		{
			if (this.View != View.Details)
			{
				return Rectangle.Empty;
			}
			if (itemIndex < 0 || itemIndex >= this.Items.Count)
			{
				throw new ArgumentOutOfRangeException("itemIndex", SR.GetString("InvalidArgument", new object[]
				{
					"itemIndex",
					itemIndex.ToString(CultureInfo.CurrentCulture)
				}));
			}
			int count = this.Items[itemIndex].SubItems.Count;
			if (subItemIndex < 0 || subItemIndex >= count)
			{
				throw new ArgumentOutOfRangeException("subItemIndex", SR.GetString("InvalidArgument", new object[]
				{
					"subItemIndex",
					subItemIndex.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (!ClientUtils.IsEnumValid(portion, (int)portion, 0, 3))
			{
				throw new InvalidEnumArgumentException("portion", (int)portion, typeof(ItemBoundsPortion));
			}
			if (this.Columns.Count == 0)
			{
				return Rectangle.Empty;
			}
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			rect.left = (int)portion;
			rect.top = subItemIndex;
			if ((int)base.SendMessage(4152, itemIndex, ref rect) == 0)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"itemIndex",
					itemIndex.ToString(CultureInfo.CurrentCulture)
				}));
			}
			return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
		}

		// Token: 0x06004439 RID: 17465 RVA: 0x000F574D File Offset: 0x000F474D
		public ListViewHitTestInfo HitTest(Point point)
		{
			return this.HitTest(point.X, point.Y);
		}

		// Token: 0x0600443A RID: 17466 RVA: 0x000F5764 File Offset: 0x000F4764
		public ListViewHitTestInfo HitTest(int x, int y)
		{
			if (!base.ClientRectangle.Contains(x, y))
			{
				return new ListViewHitTestInfo(null, null, ListViewHitTestLocations.None);
			}
			NativeMethods.LVHITTESTINFO lvhittestinfo = new NativeMethods.LVHITTESTINFO();
			lvhittestinfo.pt_x = x;
			lvhittestinfo.pt_y = y;
			int num;
			if (this.View == View.Details)
			{
				num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4153, 0, lvhittestinfo);
			}
			else
			{
				num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4114, 0, lvhittestinfo);
			}
			ListViewItem listViewItem = ((num == -1) ? null : this.Items[num]);
			ListViewHitTestLocations listViewHitTestLocations;
			if (listViewItem == null && (8 & lvhittestinfo.flags) == 8)
			{
				listViewHitTestLocations = (ListViewHitTestLocations)((247 & lvhittestinfo.flags) | 256);
			}
			else if (listViewItem != null && (8 & lvhittestinfo.flags) == 8)
			{
				listViewHitTestLocations = (ListViewHitTestLocations)((247 & lvhittestinfo.flags) | 512);
			}
			else
			{
				listViewHitTestLocations = (ListViewHitTestLocations)lvhittestinfo.flags;
			}
			if (this.View != View.Details || listViewItem == null)
			{
				return new ListViewHitTestInfo(listViewItem, null, listViewHitTestLocations);
			}
			if (lvhittestinfo.iSubItem < listViewItem.SubItems.Count)
			{
				return new ListViewHitTestInfo(listViewItem, listViewItem.SubItems[lvhittestinfo.iSubItem], listViewHitTestLocations);
			}
			return new ListViewHitTestInfo(listViewItem, null, listViewHitTestLocations);
		}

		// Token: 0x0600443B RID: 17467 RVA: 0x000F5894 File Offset: 0x000F4894
		private void InvalidateColumnHeaders()
		{
			if (this.viewStyle == View.Details && base.IsHandleCreated)
			{
				IntPtr intPtr = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4127, 0, 0);
				if (intPtr != IntPtr.Zero)
				{
					SafeNativeMethods.InvalidateRect(new HandleRef(this, intPtr), null, true);
				}
			}
		}

		// Token: 0x0600443C RID: 17468 RVA: 0x000F58E7 File Offset: 0x000F48E7
		internal ColumnHeader InsertColumn(int index, ColumnHeader ch)
		{
			return this.InsertColumn(index, ch, true);
		}

		// Token: 0x0600443D RID: 17469 RVA: 0x000F58F4 File Offset: 0x000F48F4
		internal ColumnHeader InsertColumn(int index, ColumnHeader ch, bool refreshSubItems)
		{
			if (ch == null)
			{
				throw new ArgumentNullException("ch");
			}
			if (ch.OwnerListview != null)
			{
				throw new ArgumentException(SR.GetString("OnlyOneControl", new object[] { ch.Text }), "ch");
			}
			int num;
			if (base.IsHandleCreated && this.View != View.Tile)
			{
				num = this.InsertColumnNative(index, ch);
			}
			else
			{
				num = index;
			}
			if (-1 == num)
			{
				throw new InvalidOperationException(SR.GetString("ListViewAddColumnFailed"));
			}
			int num2 = ((this.columnHeaders == null) ? 0 : this.columnHeaders.Length);
			if (num2 > 0)
			{
				ColumnHeader[] array = new ColumnHeader[num2 + 1];
				if (num2 > 0)
				{
					Array.Copy(this.columnHeaders, 0, array, 0, num2);
				}
				this.columnHeaders = array;
			}
			else
			{
				this.columnHeaders = new ColumnHeader[1];
			}
			if (num < num2)
			{
				Array.Copy(this.columnHeaders, num, this.columnHeaders, num + 1, num2 - num);
			}
			this.columnHeaders[num] = ch;
			ch.OwnerListview = this;
			if (ch.ActualImageIndex_Internal != -1 && base.IsHandleCreated && this.View != View.Tile)
			{
				this.SetColumnInfo(16, ch);
			}
			int[] array2 = new int[this.Columns.Count];
			for (int i = 0; i < this.Columns.Count; i++)
			{
				ColumnHeader columnHeader = this.Columns[i];
				if (columnHeader == ch)
				{
					columnHeader.DisplayIndexInternal = index;
				}
				else if (columnHeader.DisplayIndex >= index)
				{
					columnHeader.DisplayIndexInternal++;
				}
				array2[i] = columnHeader.DisplayIndexInternal;
			}
			this.SetDisplayIndices(array2);
			if (base.IsHandleCreated && this.View == View.Tile)
			{
				this.RecreateHandleInternal();
			}
			else if (base.IsHandleCreated && refreshSubItems)
			{
				this.RealizeAllSubItems();
			}
			return ch;
		}

		// Token: 0x0600443E RID: 17470 RVA: 0x000F5AAC File Offset: 0x000F4AAC
		private int InsertColumnNative(int index, ColumnHeader ch)
		{
			NativeMethods.LVCOLUMN_T lvcolumn_T = new NativeMethods.LVCOLUMN_T();
			lvcolumn_T.mask = 7;
			if (ch.OwnerListview != null && ch.ActualImageIndex_Internal != -1)
			{
				lvcolumn_T.mask |= 16;
				lvcolumn_T.iImage = ch.ActualImageIndex_Internal;
			}
			lvcolumn_T.fmt = (int)ch.TextAlign;
			lvcolumn_T.cx = ch.Width;
			lvcolumn_T.pszText = ch.Text;
			return (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.LVM_INSERTCOLUMN, index, lvcolumn_T);
		}

		// Token: 0x0600443F RID: 17471 RVA: 0x000F5B34 File Offset: 0x000F4B34
		internal void InsertGroupInListView(int index, ListViewGroup group)
		{
			bool flag = this.groups.Count == 1 && this.GroupsEnabled;
			this.UpdateGroupView();
			this.EnsureDefaultGroup();
			this.InsertGroupNative(index, group);
			if (flag)
			{
				for (int i = 0; i < this.Items.Count; i++)
				{
					ListViewItem listViewItem = this.Items[i];
					if (listViewItem.Group == null)
					{
						listViewItem.UpdateStateToListView(listViewItem.Index);
					}
				}
			}
		}

		// Token: 0x06004440 RID: 17472 RVA: 0x000F5BA8 File Offset: 0x000F4BA8
		private void InsertGroupNative(int index, ListViewGroup group)
		{
			NativeMethods.LVGROUP lvgroup = new NativeMethods.LVGROUP();
			try
			{
				lvgroup = this.GetLVGROUP(group);
				(int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4241, index, lvgroup);
			}
			finally
			{
				this.DestroyLVGROUP(lvgroup);
			}
		}

		// Token: 0x06004441 RID: 17473 RVA: 0x000F5BFC File Offset: 0x000F4BFC
		private void InsertItems(int displayIndex, ListViewItem[] items, bool checkHosting)
		{
			if (items == null || items.Length == 0)
			{
				return;
			}
			if (base.IsHandleCreated && this.Items.Count == 0 && this.View == View.SmallIcon && this.ComctlSupportsVisualStyles)
			{
				this.FlipViewToLargeIconAndSmallIcon = true;
			}
			if (this.updateCounter > 0 && base.Properties.GetObject(ListView.PropDelayedUpdateItems) != null)
			{
				if (checkHosting)
				{
					for (int i = 0; i < items.Length; i++)
					{
						if (items[i].listView != null)
						{
							throw new ArgumentException(SR.GetString("OnlyOneControl", new object[] { items[i].Text }), "item");
						}
					}
				}
				ArrayList arrayList = (ArrayList)base.Properties.GetObject(ListView.PropDelayedUpdateItems);
				if (arrayList != null)
				{
					arrayList.AddRange(items);
				}
				for (int j = 0; j < items.Length; j++)
				{
					items[j].Host(this, this.GenerateUniqueID(), -1);
				}
				this.FlipViewToLargeIconAndSmallIcon = false;
				return;
			}
			for (int k = 0; k < items.Length; k++)
			{
				ListViewItem listViewItem = items[k];
				if (checkHosting && listViewItem.listView != null)
				{
					throw new ArgumentException(SR.GetString("OnlyOneControl", new object[] { listViewItem.Text }), "item");
				}
				int num = this.GenerateUniqueID();
				this.listItemsTable.Add(num, listViewItem);
				this.itemCount++;
				listViewItem.Host(this, num, -1);
				if (!base.IsHandleCreated)
				{
					this.listItemsArray.Insert(displayIndex + k, listViewItem);
				}
			}
			if (base.IsHandleCreated)
			{
				this.InsertItemsNative(displayIndex, items);
			}
			base.Invalidate();
			this.ArrangeIcons(this.alignStyle);
			if (!this.VirtualMode)
			{
				this.Sort();
			}
		}

		// Token: 0x06004442 RID: 17474 RVA: 0x000F5DBC File Offset: 0x000F4DBC
		private int InsertItemsNative(int index, ListViewItem[] items)
		{
			if (items == null || items.Length == 0)
			{
				return 0;
			}
			if (index == this.itemCount - 1)
			{
				index++;
			}
			NativeMethods.LVITEM lvitem = default(NativeMethods.LVITEM);
			int num = -1;
			IntPtr intPtr = IntPtr.Zero;
			int num2 = 0;
			this.listViewState1[1] = true;
			try
			{
				base.SendMessage(4143, this.itemCount, 0);
				for (int i = 0; i < items.Length; i++)
				{
					ListViewItem listViewItem = items[i];
					lvitem.Reset();
					lvitem.mask = 23;
					lvitem.iItem = index + i;
					lvitem.pszText = listViewItem.Text;
					lvitem.iImage = listViewItem.ImageIndexer.ActualIndex;
					lvitem.iIndent = listViewItem.IndentCount;
					lvitem.lParam = (IntPtr)listViewItem.ID;
					if (this.GroupsEnabled)
					{
						lvitem.mask |= 256;
						lvitem.iGroupId = this.GetNativeGroupId(listViewItem);
					}
					lvitem.mask |= 512;
					lvitem.cColumns = ((this.columnHeaders != null) ? Math.Min(20, this.columnHeaders.Length) : 0);
					if (lvitem.cColumns > num2 || intPtr == IntPtr.Zero)
					{
						if (intPtr != IntPtr.Zero)
						{
							Marshal.FreeHGlobal(intPtr);
						}
						intPtr = Marshal.AllocHGlobal(lvitem.cColumns * Marshal.SizeOf(typeof(int)));
						num2 = lvitem.cColumns;
					}
					lvitem.puColumns = intPtr;
					int[] array = new int[lvitem.cColumns];
					for (int j = 0; j < lvitem.cColumns; j++)
					{
						array[j] = j + 1;
					}
					Marshal.Copy(array, 0, lvitem.puColumns, lvitem.cColumns);
					ItemCheckEventHandler itemCheckEventHandler = this.onItemCheck;
					this.onItemCheck = null;
					int num3;
					try
					{
						listViewItem.UpdateStateToListView(lvitem.iItem, ref lvitem, false);
						num3 = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.LVM_INSERTITEM, 0, ref lvitem);
						if (num == -1)
						{
							num = num3;
							index = num;
						}
					}
					finally
					{
						this.onItemCheck = itemCheckEventHandler;
					}
					if (-1 == num3)
					{
						throw new InvalidOperationException(SR.GetString("ListViewAddItemFailed"));
					}
					for (int k = 1; k < listViewItem.SubItems.Count; k++)
					{
						this.SetItemText(num3, k, listViewItem.SubItems[k].Text, ref lvitem);
					}
					if (listViewItem.StateImageSet || listViewItem.StateSelected)
					{
						this.SetItemState(num3, lvitem.state, lvitem.stateMask);
					}
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
				this.listViewState1[1] = false;
			}
			if (this.listViewState1[16])
			{
				this.listViewState1[16] = false;
				this.OnSelectedIndexChanged(EventArgs.Empty);
			}
			if (this.FlipViewToLargeIconAndSmallIcon)
			{
				this.FlipViewToLargeIconAndSmallIcon = false;
				this.View = View.LargeIcon;
				this.View = View.SmallIcon;
			}
			return num;
		}

		// Token: 0x06004443 RID: 17475 RVA: 0x000F60F4 File Offset: 0x000F50F4
		protected override bool IsInputKey(Keys keyData)
		{
			if ((keyData & Keys.Alt) == Keys.Alt)
			{
				return false;
			}
			switch (keyData & Keys.KeyCode)
			{
			case Keys.Prior:
			case Keys.Next:
			case Keys.End:
			case Keys.Home:
				return true;
			default:
			{
				bool flag = base.IsInputKey(keyData);
				if (flag)
				{
					return true;
				}
				if (this.listViewState[16384])
				{
					Keys keys = keyData & Keys.KeyCode;
					if (keys == Keys.Return || keys == Keys.Escape)
					{
						return true;
					}
				}
				return false;
			}
			}
		}

		// Token: 0x06004444 RID: 17476 RVA: 0x000F616C File Offset: 0x000F516C
		private void LargeImageListRecreateHandle(object sender, EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				IntPtr intPtr = ((this.LargeImageList == null) ? IntPtr.Zero : this.LargeImageList.Handle);
				base.SendMessage(4099, (IntPtr)0, intPtr);
				this.ForceCheckBoxUpdate();
			}
		}

		// Token: 0x06004445 RID: 17477 RVA: 0x000F61B8 File Offset: 0x000F51B8
		private void LargeImageListChangedHandle(object sender, EventArgs e)
		{
			if (!this.VirtualMode && sender != null && sender == this.imageListLarge && base.IsHandleCreated)
			{
				foreach (object obj in this.Items)
				{
					ListViewItem listViewItem = (ListViewItem)obj;
					if (listViewItem.ImageIndexer.ActualIndex != -1 && listViewItem.ImageIndexer.ActualIndex >= this.imageListLarge.Images.Count)
					{
						this.SetItemImage(listViewItem.Index, this.imageListLarge.Images.Count - 1);
					}
					else
					{
						this.SetItemImage(listViewItem.Index, listViewItem.ImageIndexer.ActualIndex);
					}
				}
			}
		}

		// Token: 0x06004446 RID: 17478 RVA: 0x000F6294 File Offset: 0x000F5294
		internal void ListViewItemToolTipChanged(ListViewItem item)
		{
			if (base.IsHandleCreated)
			{
				this.SetItemText(item.Index, 0, item.Text);
			}
		}

		// Token: 0x06004447 RID: 17479 RVA: 0x000F62B4 File Offset: 0x000F52B4
		private void LvnBeginDrag(MouseButtons buttons, NativeMethods.NMLISTVIEW nmlv)
		{
			ListViewItem listViewItem = this.Items[nmlv.iItem];
			this.OnItemDrag(new ItemDragEventArgs(buttons, listViewItem));
		}

		// Token: 0x06004448 RID: 17480 RVA: 0x000F62E1 File Offset: 0x000F52E1
		protected virtual void OnAfterLabelEdit(LabelEditEventArgs e)
		{
			if (this.onAfterLabelEdit != null)
			{
				this.onAfterLabelEdit(this, e);
			}
		}

		// Token: 0x06004449 RID: 17481 RVA: 0x000F62F8 File Offset: 0x000F52F8
		protected override void OnBackgroundImageChanged(EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				this.SetBackgroundImage();
			}
			base.OnBackgroundImageChanged(e);
		}

		// Token: 0x0600444A RID: 17482 RVA: 0x000F630F File Offset: 0x000F530F
		protected override void OnMouseLeave(EventArgs e)
		{
			this.hoveredAlready = false;
			base.OnMouseLeave(e);
		}

		// Token: 0x0600444B RID: 17483 RVA: 0x000F6320 File Offset: 0x000F5320
		protected override void OnMouseHover(EventArgs e)
		{
			ListViewItem listViewItem = null;
			if (this.Items.Count > 0)
			{
				Point point = Cursor.Position;
				point = base.PointToClientInternal(point);
				listViewItem = this.GetItemAt(point.X, point.Y);
			}
			if (listViewItem != this.prevHoveredItem && listViewItem != null)
			{
				this.OnItemMouseHover(new ListViewItemMouseHoverEventArgs(listViewItem));
				this.prevHoveredItem = listViewItem;
			}
			if (!this.hoveredAlready)
			{
				base.OnMouseHover(e);
				this.hoveredAlready = true;
			}
			base.ResetMouseEventArgs();
		}

		// Token: 0x0600444C RID: 17484 RVA: 0x000F639B File Offset: 0x000F539B
		protected virtual void OnBeforeLabelEdit(LabelEditEventArgs e)
		{
			if (this.onBeforeLabelEdit != null)
			{
				this.onBeforeLabelEdit(this, e);
			}
		}

		// Token: 0x0600444D RID: 17485 RVA: 0x000F63B4 File Offset: 0x000F53B4
		protected virtual void OnCacheVirtualItems(CacheVirtualItemsEventArgs e)
		{
			CacheVirtualItemsEventHandler cacheVirtualItemsEventHandler = (CacheVirtualItemsEventHandler)base.Events[ListView.EVENT_CACHEVIRTUALITEMS];
			if (cacheVirtualItemsEventHandler != null)
			{
				cacheVirtualItemsEventHandler(this, e);
			}
		}

		// Token: 0x0600444E RID: 17486 RVA: 0x000F63E2 File Offset: 0x000F53E2
		protected virtual void OnColumnClick(ColumnClickEventArgs e)
		{
			if (this.onColumnClick != null)
			{
				this.onColumnClick(this, e);
			}
		}

		// Token: 0x0600444F RID: 17487 RVA: 0x000F63FC File Offset: 0x000F53FC
		protected virtual void OnColumnReordered(ColumnReorderedEventArgs e)
		{
			ColumnReorderedEventHandler columnReorderedEventHandler = (ColumnReorderedEventHandler)base.Events[ListView.EVENT_COLUMNREORDERED];
			if (columnReorderedEventHandler != null)
			{
				columnReorderedEventHandler(this, e);
			}
		}

		// Token: 0x06004450 RID: 17488 RVA: 0x000F642C File Offset: 0x000F542C
		protected virtual void OnColumnWidthChanged(ColumnWidthChangedEventArgs e)
		{
			ColumnWidthChangedEventHandler columnWidthChangedEventHandler = (ColumnWidthChangedEventHandler)base.Events[ListView.EVENT_COLUMNWIDTHCHANGED];
			if (columnWidthChangedEventHandler != null)
			{
				columnWidthChangedEventHandler(this, e);
			}
		}

		// Token: 0x06004451 RID: 17489 RVA: 0x000F645C File Offset: 0x000F545C
		protected virtual void OnColumnWidthChanging(ColumnWidthChangingEventArgs e)
		{
			ColumnWidthChangingEventHandler columnWidthChangingEventHandler = (ColumnWidthChangingEventHandler)base.Events[ListView.EVENT_COLUMNWIDTHCHANGING];
			if (columnWidthChangingEventHandler != null)
			{
				columnWidthChangingEventHandler(this, e);
			}
		}

		// Token: 0x06004452 RID: 17490 RVA: 0x000F648C File Offset: 0x000F548C
		protected virtual void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
		{
			DrawListViewColumnHeaderEventHandler drawListViewColumnHeaderEventHandler = (DrawListViewColumnHeaderEventHandler)base.Events[ListView.EVENT_DRAWCOLUMNHEADER];
			if (drawListViewColumnHeaderEventHandler != null)
			{
				drawListViewColumnHeaderEventHandler(this, e);
			}
		}

		// Token: 0x06004453 RID: 17491 RVA: 0x000F64BC File Offset: 0x000F54BC
		protected virtual void OnDrawItem(DrawListViewItemEventArgs e)
		{
			DrawListViewItemEventHandler drawListViewItemEventHandler = (DrawListViewItemEventHandler)base.Events[ListView.EVENT_DRAWITEM];
			if (drawListViewItemEventHandler != null)
			{
				drawListViewItemEventHandler(this, e);
			}
		}

		// Token: 0x06004454 RID: 17492 RVA: 0x000F64EC File Offset: 0x000F54EC
		protected virtual void OnDrawSubItem(DrawListViewSubItemEventArgs e)
		{
			DrawListViewSubItemEventHandler drawListViewSubItemEventHandler = (DrawListViewSubItemEventHandler)base.Events[ListView.EVENT_DRAWSUBITEM];
			if (drawListViewSubItemEventHandler != null)
			{
				drawListViewSubItemEventHandler(this, e);
			}
		}

		// Token: 0x06004455 RID: 17493 RVA: 0x000F651C File Offset: 0x000F551C
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			if (!this.VirtualMode && base.IsHandleCreated && this.AutoArrange)
			{
				this.BeginUpdate();
				try
				{
					base.SendMessage(4138, -1, 0);
				}
				finally
				{
					this.EndUpdate();
				}
			}
			this.InvalidateColumnHeaders();
		}

		// Token: 0x06004456 RID: 17494 RVA: 0x000F657C File Offset: 0x000F557C
		protected override void OnHandleCreated(EventArgs e)
		{
			this.listViewState[4194304] = false;
			this.FlipViewToLargeIconAndSmallIcon = false;
			base.OnHandleCreated(e);
			int num = (int)base.SendMessage(8200, 0, 0);
			if (num < 5)
			{
				base.SendMessage(8199, 5, 0);
			}
			this.UpdateExtendedStyles();
			this.RealizeProperties();
			int num2 = ColorTranslator.ToWin32(this.BackColor);
			base.SendMessage(4097, 0, num2);
			base.SendMessage(4132, 0, ColorTranslator.ToWin32(base.ForeColor));
			base.SendMessage(4134, 0, -1);
			if (!this.Scrollable)
			{
				int num3 = (int)(long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, base.Handle), -16);
				num3 |= 8192;
				UnsafeNativeMethods.SetWindowLong(new HandleRef(this, base.Handle), -16, new HandleRef(null, (IntPtr)num3));
			}
			if (this.VirtualMode)
			{
				int num4 = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4106, 0, 0);
				num4 |= 61440;
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4107, num4, 0);
			}
			if (this.ComctlSupportsVisualStyles)
			{
				base.SendMessage(4238, (int)this.viewStyle, 0);
				this.UpdateGroupView();
				if (this.groups != null)
				{
					for (int i = 0; i < this.groups.Count; i++)
					{
						this.InsertGroupNative(i, this.groups[i]);
					}
				}
				if (this.viewStyle == View.Tile)
				{
					this.UpdateTileView();
				}
			}
			this.ListViewHandleDestroyed = false;
			ListViewItem[] array = null;
			if (this.listItemsArray != null)
			{
				array = (ListViewItem[])this.listItemsArray.ToArray(typeof(ListViewItem));
				this.listItemsArray = null;
			}
			int num5 = ((this.columnHeaders == null) ? 0 : this.columnHeaders.Length);
			if (num5 > 0)
			{
				int[] array2 = new int[this.columnHeaders.Length];
				int num6 = 0;
				foreach (ColumnHeader columnHeader in this.columnHeaders)
				{
					array2[num6] = columnHeader.DisplayIndex;
					this.InsertColumnNative(num6++, columnHeader);
				}
				this.SetDisplayIndices(array2);
			}
			if (this.itemCount > 0 && array != null)
			{
				this.InsertItemsNative(0, array);
			}
			if (this.VirtualMode && this.VirtualListSize > -1 && !base.DesignMode)
			{
				base.SendMessage(4143, this.VirtualListSize, 0);
			}
			if (num5 > 0)
			{
				this.UpdateColumnWidths(ColumnHeaderAutoResizeStyle.None);
			}
			this.ArrangeIcons(this.alignStyle);
			this.UpdateListViewItemsLocations();
			if (!this.VirtualMode)
			{
				this.Sort();
			}
			if (this.ComctlSupportsVisualStyles && this.InsertionMark.Index > 0)
			{
				this.InsertionMark.UpdateListView();
			}
			this.savedCheckedItems = null;
			if (!this.CheckBoxes && !this.VirtualMode)
			{
				for (int k = 0; k < this.Items.Count; k++)
				{
					if (this.Items[k].Checked)
					{
						this.UpdateSavedCheckedItems(this.Items[k], true);
					}
				}
			}
		}

		// Token: 0x06004457 RID: 17495 RVA: 0x000F68A4 File Offset: 0x000F58A4
		protected override void OnHandleDestroyed(EventArgs e)
		{
			if (!base.Disposing && !this.VirtualMode)
			{
				int count = this.Items.Count;
				for (int i = 0; i < count; i++)
				{
					this.Items[i].UpdateStateFromListView(i, true);
				}
				if (this.SelectedItems != null && !this.VirtualMode)
				{
					ListViewItem[] array = new ListViewItem[this.SelectedItems.Count];
					this.SelectedItems.CopyTo(array, 0);
					this.savedSelectedItems = new List<ListViewItem>(array.Length);
					for (int j = 0; j < array.Length; j++)
					{
						this.savedSelectedItems.Add(array[j]);
					}
				}
				ListViewItem[] array2 = null;
				ListView.ListViewItemCollection items = this.Items;
				if (items != null)
				{
					array2 = new ListViewItem[items.Count];
					items.CopyTo(array2, 0);
				}
				if (array2 != null)
				{
					this.listItemsArray = new ArrayList(array2.Length);
					this.listItemsArray.AddRange(array2);
				}
				this.ListViewHandleDestroyed = true;
			}
			base.OnHandleDestroyed(e);
		}

		// Token: 0x06004458 RID: 17496 RVA: 0x000F699F File Offset: 0x000F599F
		protected virtual void OnItemActivate(EventArgs e)
		{
			if (this.onItemActivate != null)
			{
				this.onItemActivate(this, e);
			}
		}

		// Token: 0x06004459 RID: 17497 RVA: 0x000F69B6 File Offset: 0x000F59B6
		protected virtual void OnItemCheck(ItemCheckEventArgs ice)
		{
			if (this.onItemCheck != null)
			{
				this.onItemCheck(this, ice);
			}
		}

		// Token: 0x0600445A RID: 17498 RVA: 0x000F69CD File Offset: 0x000F59CD
		protected virtual void OnItemChecked(ItemCheckedEventArgs e)
		{
			if (this.onItemChecked != null)
			{
				this.onItemChecked(this, e);
			}
		}

		// Token: 0x0600445B RID: 17499 RVA: 0x000F69E4 File Offset: 0x000F59E4
		protected virtual void OnItemDrag(ItemDragEventArgs e)
		{
			if (this.onItemDrag != null)
			{
				this.onItemDrag(this, e);
			}
		}

		// Token: 0x0600445C RID: 17500 RVA: 0x000F69FB File Offset: 0x000F59FB
		protected virtual void OnItemMouseHover(ListViewItemMouseHoverEventArgs e)
		{
			if (this.onItemMouseHover != null)
			{
				this.onItemMouseHover(this, e);
			}
		}

		// Token: 0x0600445D RID: 17501 RVA: 0x000F6A14 File Offset: 0x000F5A14
		protected virtual void OnItemSelectionChanged(ListViewItemSelectionChangedEventArgs e)
		{
			ListViewItemSelectionChangedEventHandler listViewItemSelectionChangedEventHandler = (ListViewItemSelectionChangedEventHandler)base.Events[ListView.EVENT_ITEMSELECTIONCHANGED];
			if (listViewItemSelectionChangedEventHandler != null)
			{
				listViewItemSelectionChangedEventHandler(this, e);
			}
		}

		// Token: 0x0600445E RID: 17502 RVA: 0x000F6A42 File Offset: 0x000F5A42
		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			if (base.IsHandleCreated)
			{
				this.RecreateHandleInternal();
			}
		}

		// Token: 0x0600445F RID: 17503 RVA: 0x000F6A59 File Offset: 0x000F5A59
		protected override void OnResize(EventArgs e)
		{
			if (this.View == View.Details && !this.Scrollable && base.IsHandleCreated)
			{
				this.PositionHeader();
			}
			base.OnResize(e);
		}

		// Token: 0x06004460 RID: 17504 RVA: 0x000F6A84 File Offset: 0x000F5A84
		protected virtual void OnRetrieveVirtualItem(RetrieveVirtualItemEventArgs e)
		{
			RetrieveVirtualItemEventHandler retrieveVirtualItemEventHandler = (RetrieveVirtualItemEventHandler)base.Events[ListView.EVENT_RETRIEVEVIRTUALITEM];
			if (retrieveVirtualItemEventHandler != null)
			{
				retrieveVirtualItemEventHandler(this, e);
			}
		}

		// Token: 0x06004461 RID: 17505 RVA: 0x000F6AB4 File Offset: 0x000F5AB4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnRightToLeftLayoutChanged(EventArgs e)
		{
			if (base.GetAnyDisposingInHierarchy())
			{
				return;
			}
			if (this.RightToLeft == RightToLeft.Yes)
			{
				this.RecreateHandleInternal();
			}
			EventHandler eventHandler = base.Events[ListView.EVENT_RIGHTTOLEFTLAYOUTCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06004462 RID: 17506 RVA: 0x000F6AFC File Offset: 0x000F5AFC
		protected virtual void OnSearchForVirtualItem(SearchForVirtualItemEventArgs e)
		{
			SearchForVirtualItemEventHandler searchForVirtualItemEventHandler = (SearchForVirtualItemEventHandler)base.Events[ListView.EVENT_SEARCHFORVIRTUALITEM];
			if (searchForVirtualItemEventHandler != null)
			{
				searchForVirtualItemEventHandler(this, e);
			}
		}

		// Token: 0x06004463 RID: 17507 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnSelectedIndexChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ListView.EVENT_SELECTEDINDEXCHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06004464 RID: 17508 RVA: 0x000F6B5C File Offset: 0x000F5B5C
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			if (base.IsHandleCreated)
			{
				int num = ColorTranslator.ToWin32(this.BackColor);
				base.SendMessage(4097, 0, num);
				base.SendMessage(4134, 0, -1);
			}
		}

		// Token: 0x06004465 RID: 17509 RVA: 0x000F6BA0 File Offset: 0x000F5BA0
		protected virtual void OnVirtualItemsSelectionRangeChanged(ListViewVirtualItemsSelectionRangeChangedEventArgs e)
		{
			ListViewVirtualItemsSelectionRangeChangedEventHandler listViewVirtualItemsSelectionRangeChangedEventHandler = (ListViewVirtualItemsSelectionRangeChangedEventHandler)base.Events[ListView.EVENT_VIRTUALITEMSSELECTIONRANGECHANGED];
			if (listViewVirtualItemsSelectionRangeChangedEventHandler != null)
			{
				listViewVirtualItemsSelectionRangeChangedEventHandler(this, e);
			}
		}

		// Token: 0x06004466 RID: 17510 RVA: 0x000F6BD0 File Offset: 0x000F5BD0
		private void PositionHeader()
		{
			IntPtr window = UnsafeNativeMethods.GetWindow(new HandleRef(this, base.Handle), 5);
			if (window != IntPtr.Zero)
			{
				IntPtr intPtr = IntPtr.Zero;
				IntPtr intPtr2 = IntPtr.Zero;
				intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NativeMethods.RECT)));
				if (intPtr == IntPtr.Zero)
				{
					return;
				}
				try
				{
					intPtr2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NativeMethods.WINDOWPOS)));
					if (!(intPtr == IntPtr.Zero))
					{
						UnsafeNativeMethods.GetClientRect(new HandleRef(this, base.Handle), intPtr);
						NativeMethods.HDLAYOUT hdlayout = default(NativeMethods.HDLAYOUT);
						hdlayout.prc = intPtr;
						hdlayout.pwpos = intPtr2;
						UnsafeNativeMethods.SendMessage(new HandleRef(this, window), 4613, 0, ref hdlayout);
						NativeMethods.WINDOWPOS windowpos = (NativeMethods.WINDOWPOS)Marshal.PtrToStructure(intPtr2, typeof(NativeMethods.WINDOWPOS));
						SafeNativeMethods.SetWindowPos(new HandleRef(this, window), new HandleRef(this, windowpos.hwndInsertAfter), windowpos.x, windowpos.y, windowpos.cx, windowpos.cy, windowpos.flags | 64);
					}
				}
				finally
				{
					if (intPtr != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(intPtr);
					}
					if (intPtr2 != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(intPtr2);
					}
				}
			}
		}

		// Token: 0x06004467 RID: 17511 RVA: 0x000F6D28 File Offset: 0x000F5D28
		private void RealizeAllSubItems()
		{
			NativeMethods.LVITEM lvitem = default(NativeMethods.LVITEM);
			for (int i = 0; i < this.itemCount; i++)
			{
				int count = this.Items[i].SubItems.Count;
				for (int j = 0; j < count; j++)
				{
					this.SetItemText(i, j, this.Items[i].SubItems[j].Text, ref lvitem);
				}
			}
		}

		// Token: 0x06004468 RID: 17512 RVA: 0x000F6D98 File Offset: 0x000F5D98
		protected void RealizeProperties()
		{
			Color color = this.BackColor;
			if (color != SystemColors.Window)
			{
				base.SendMessage(4097, 0, ColorTranslator.ToWin32(color));
			}
			color = this.ForeColor;
			if (color != SystemColors.WindowText)
			{
				base.SendMessage(4132, 0, ColorTranslator.ToWin32(color));
			}
			if (this.imageListLarge != null)
			{
				base.SendMessage(4099, 0, this.imageListLarge.Handle);
			}
			if (this.imageListSmall != null)
			{
				base.SendMessage(4099, 1, this.imageListSmall.Handle);
			}
			if (this.imageListState != null)
			{
				base.SendMessage(4099, 2, this.imageListState.Handle);
			}
		}

		// Token: 0x06004469 RID: 17513 RVA: 0x000F6E54 File Offset: 0x000F5E54
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void RedrawItems(int startIndex, int endIndex, bool invalidateOnly)
		{
			if (this.VirtualMode)
			{
				if (startIndex < 0 || startIndex >= this.VirtualListSize)
				{
					throw new ArgumentOutOfRangeException("startIndex", SR.GetString("InvalidArgument", new object[]
					{
						"startIndex",
						startIndex.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (endIndex < 0 || endIndex >= this.VirtualListSize)
				{
					throw new ArgumentOutOfRangeException("endIndex", SR.GetString("InvalidArgument", new object[]
					{
						"endIndex",
						endIndex.ToString(CultureInfo.CurrentCulture)
					}));
				}
			}
			else
			{
				if (startIndex < 0 || startIndex >= this.Items.Count)
				{
					throw new ArgumentOutOfRangeException("startIndex", SR.GetString("InvalidArgument", new object[]
					{
						"startIndex",
						startIndex.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (endIndex < 0 || endIndex >= this.Items.Count)
				{
					throw new ArgumentOutOfRangeException("endIndex", SR.GetString("InvalidArgument", new object[]
					{
						"endIndex",
						endIndex.ToString(CultureInfo.CurrentCulture)
					}));
				}
			}
			if (startIndex > endIndex)
			{
				throw new ArgumentException(SR.GetString("ListViewStartIndexCannotBeLargerThanEndIndex"));
			}
			if (base.IsHandleCreated)
			{
				(int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4117, startIndex, endIndex);
				if (this.View == View.LargeIcon || this.View == View.SmallIcon)
				{
					Rectangle rectangle = this.Items[startIndex].Bounds;
					for (int i = startIndex + 1; i <= endIndex; i++)
					{
						rectangle = Rectangle.Union(rectangle, this.Items[i].Bounds);
					}
					if (startIndex > 0)
					{
						rectangle = Rectangle.Union(rectangle, this.Items[startIndex - 1].Bounds);
					}
					else
					{
						rectangle.Width += rectangle.X;
						rectangle.Height += rectangle.Y;
						rectangle.X = (rectangle.Y = 0);
					}
					if (endIndex < this.Items.Count - 1)
					{
						rectangle = Rectangle.Union(rectangle, this.Items[endIndex + 1].Bounds);
					}
					else
					{
						rectangle.Height += base.ClientRectangle.Bottom - rectangle.Bottom;
						rectangle.Width += base.ClientRectangle.Right - rectangle.Right;
					}
					if (this.View == View.LargeIcon)
					{
						rectangle.Inflate(1, this.Font.Height + 1);
					}
					base.Invalidate(rectangle);
				}
				if (!invalidateOnly)
				{
					base.Update();
				}
			}
		}

		// Token: 0x0600446A RID: 17514 RVA: 0x000F7110 File Offset: 0x000F6110
		internal void RemoveGroupFromListView(ListViewGroup group)
		{
			this.EnsureDefaultGroup();
			foreach (object obj in group.Items)
			{
				ListViewItem listViewItem = (ListViewItem)obj;
				if (listViewItem.ListView == this)
				{
					listViewItem.UpdateStateToListView(listViewItem.Index);
				}
			}
			this.RemoveGroupNative(group);
			this.UpdateGroupView();
		}

		// Token: 0x0600446B RID: 17515 RVA: 0x000F718C File Offset: 0x000F618C
		private void RemoveGroupNative(ListViewGroup group)
		{
			(int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4246, group.ID, IntPtr.Zero);
		}

		// Token: 0x0600446C RID: 17516 RVA: 0x000F71B8 File Offset: 0x000F61B8
		private void Scroll(int fromLVItem, int toLVItem)
		{
			int num = 0;
			int num2 = Math.Max(fromLVItem, toLVItem);
			int num3 = Math.Min(fromLVItem, toLVItem);
			for (int i = num3; i < num2; i++)
			{
				ListViewItem listViewItem = this.Items[i];
				int num4 = 0;
				if (listViewItem.ImageIndex != -1)
				{
					num4 = listViewItem.ImageList.Images[listViewItem.ImageIndex].Size.Height;
				}
				int num5 = 0;
				if (!string.IsNullOrEmpty(listViewItem.Text))
				{
					Graphics graphics = base.CreateGraphicsInternal();
					try
					{
						num5 = Size.Ceiling(graphics.MeasureString(listViewItem.Text, this.Font)).Height;
					}
					finally
					{
						graphics.Dispose();
					}
				}
				num += Math.Max(num5, num4);
			}
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4116, 0, (fromLVItem < toLVItem) ? num : (-num));
		}

		// Token: 0x0600446D RID: 17517 RVA: 0x000F72B0 File Offset: 0x000F62B0
		private void SetBackgroundImage()
		{
			Application.OleRequired();
			NativeMethods.LVBKIMAGE lvbkimage = new NativeMethods.LVBKIMAGE();
			lvbkimage.xOffset = 0;
			lvbkimage.yOffset = 0;
			string text = this.backgroundImageFileName;
			if (this.BackgroundImage != null)
			{
				EnvironmentPermission environmentPermission = new EnvironmentPermission(EnvironmentPermissionAccess.Read, "TEMP");
				FileIOPermission fileIOPermission = new FileIOPermission(PermissionState.Unrestricted);
				PermissionSet permissionSet = new PermissionSet(PermissionState.Unrestricted);
				permissionSet.AddPermission(environmentPermission);
				permissionSet.AddPermission(fileIOPermission);
				permissionSet.Assert();
				try
				{
					string tempPath = Path.GetTempPath();
					StringBuilder stringBuilder = new StringBuilder(1024);
					UnsafeNativeMethods.GetTempFileName(tempPath, this.GenerateRandomName(), 0, stringBuilder);
					this.backgroundImageFileName = stringBuilder.ToString();
					this.BackgroundImage.Save(this.backgroundImageFileName, ImageFormat.Bmp);
				}
				finally
				{
					PermissionSet.RevertAssert();
				}
				lvbkimage.pszImage = this.backgroundImageFileName;
				lvbkimage.cchImageMax = this.backgroundImageFileName.Length + 1;
				lvbkimage.ulFlags = 2;
				if (this.BackgroundImageTiled)
				{
					lvbkimage.ulFlags |= 16;
				}
				else
				{
					NativeMethods.LVBKIMAGE lvbkimage2 = lvbkimage;
					lvbkimage2.ulFlags = lvbkimage2.ulFlags;
				}
			}
			else
			{
				lvbkimage.ulFlags = 0;
				this.backgroundImageFileName = string.Empty;
			}
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.LVM_SETBKIMAGE, 0, lvbkimage);
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			if (this.bkImgFileNames == null)
			{
				this.bkImgFileNames = new string[8];
				this.bkImgFileNamesCount = -1;
			}
			if (this.bkImgFileNamesCount == 7)
			{
				this.DeleteFileName(this.bkImgFileNames[0]);
				this.bkImgFileNames[0] = this.bkImgFileNames[1];
				this.bkImgFileNames[1] = this.bkImgFileNames[2];
				this.bkImgFileNames[2] = this.bkImgFileNames[3];
				this.bkImgFileNames[3] = this.bkImgFileNames[4];
				this.bkImgFileNames[4] = this.bkImgFileNames[5];
				this.bkImgFileNames[5] = this.bkImgFileNames[6];
				this.bkImgFileNames[6] = this.bkImgFileNames[7];
				this.bkImgFileNames[7] = null;
				this.bkImgFileNamesCount--;
			}
			this.bkImgFileNamesCount++;
			this.bkImgFileNames[this.bkImgFileNamesCount] = text;
			this.Refresh();
		}

		// Token: 0x0600446E RID: 17518 RVA: 0x000F74DC File Offset: 0x000F64DC
		internal void SetColumnInfo(int mask, ColumnHeader ch)
		{
			if (base.IsHandleCreated)
			{
				NativeMethods.LVCOLUMN lvcolumn = new NativeMethods.LVCOLUMN();
				lvcolumn.mask = mask;
				if ((mask & 16) != 0 || (mask & 1) != 0)
				{
					lvcolumn.mask |= 1;
					if (ch.ActualImageIndex_Internal > -1)
					{
						lvcolumn.iImage = ch.ActualImageIndex_Internal;
						lvcolumn.fmt |= 2048;
					}
					lvcolumn.fmt |= (int)ch.TextAlign;
				}
				if ((mask & 4) != 0)
				{
					lvcolumn.pszText = Marshal.StringToHGlobalAuto(ch.Text);
				}
				int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.LVM_SETCOLUMN, ch.Index, lvcolumn);
				if ((mask & 4) != 0)
				{
					Marshal.FreeHGlobal(lvcolumn.pszText);
				}
				if (num == 0)
				{
					throw new InvalidOperationException(SR.GetString("ListViewColumnInfoSet"));
				}
				this.InvalidateColumnHeaders();
			}
		}

		// Token: 0x0600446F RID: 17519 RVA: 0x000F75B8 File Offset: 0x000F65B8
		internal void SetColumnWidth(int columnIndex, ColumnHeaderAutoResizeStyle headerAutoResize)
		{
			if (columnIndex < 0 || (columnIndex >= 0 && this.columnHeaders == null) || columnIndex >= this.columnHeaders.Length)
			{
				throw new ArgumentOutOfRangeException("columnIndex", SR.GetString("InvalidArgument", new object[]
				{
					"columnIndex",
					columnIndex.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (!ClientUtils.IsEnumValid(headerAutoResize, (int)headerAutoResize, 0, 2))
			{
				throw new InvalidEnumArgumentException("headerAutoResize", (int)headerAutoResize, typeof(ColumnHeaderAutoResizeStyle));
			}
			int num = 0;
			int num2 = 0;
			if (headerAutoResize == ColumnHeaderAutoResizeStyle.None)
			{
				num = this.columnHeaders[columnIndex].WidthInternal;
				if (num == -2)
				{
					headerAutoResize = ColumnHeaderAutoResizeStyle.HeaderSize;
				}
				else if (num == -1)
				{
					headerAutoResize = ColumnHeaderAutoResizeStyle.ColumnContent;
				}
			}
			if (headerAutoResize == ColumnHeaderAutoResizeStyle.HeaderSize)
			{
				num2 = this.CompensateColumnHeaderResize(columnIndex, false);
				num = -2;
			}
			else if (headerAutoResize == ColumnHeaderAutoResizeStyle.ColumnContent)
			{
				num2 = this.CompensateColumnHeaderResize(columnIndex, false);
				num = -1;
			}
			if (base.IsHandleCreated)
			{
				base.SendMessage(4126, columnIndex, NativeMethods.Util.MAKELPARAM(num, 0));
			}
			if (base.IsHandleCreated && (headerAutoResize == ColumnHeaderAutoResizeStyle.ColumnContent || headerAutoResize == ColumnHeaderAutoResizeStyle.HeaderSize) && num2 != 0)
			{
				int num3 = this.columnHeaders[columnIndex].Width + num2;
				base.SendMessage(4126, columnIndex, NativeMethods.Util.MAKELPARAM(num3, 0));
			}
		}

		// Token: 0x06004470 RID: 17520 RVA: 0x000F76D4 File Offset: 0x000F66D4
		private void SetColumnWidth(int index, int width)
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(4126, index, NativeMethods.Util.MAKELPARAM(width, 0));
			}
		}

		// Token: 0x06004471 RID: 17521 RVA: 0x000F76F4 File Offset: 0x000F66F4
		private void SetDisplayIndices(int[] indices)
		{
			int[] array = new int[indices.Length];
			for (int i = 0; i < indices.Length; i++)
			{
				this.Columns[i].DisplayIndexInternal = indices[i];
				array[indices[i]] = i;
			}
			if (base.IsHandleCreated && !base.Disposing)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4154, array.Length, array);
			}
		}

		// Token: 0x06004472 RID: 17522 RVA: 0x000F775D File Offset: 0x000F675D
		internal void UpdateSavedCheckedItems(ListViewItem item, bool addItem)
		{
			if (addItem && this.savedCheckedItems == null)
			{
				this.savedCheckedItems = new List<ListViewItem>();
			}
			if (addItem)
			{
				this.savedCheckedItems.Add(item);
				return;
			}
			if (this.savedCheckedItems != null)
			{
				this.savedCheckedItems.Remove(item);
			}
		}

		// Token: 0x06004473 RID: 17523 RVA: 0x000F779A File Offset: 0x000F679A
		internal void SetToolTip(ToolTip toolTip, string toolTipCaption)
		{
			this.toolTipCaption = toolTipCaption;
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4170, new HandleRef(toolTip, toolTip.Handle), 0);
		}

		// Token: 0x06004474 RID: 17524 RVA: 0x000F77C8 File Offset: 0x000F67C8
		internal void SetItemImage(int index, int image)
		{
			if (index < 0 || (this.VirtualMode && index >= this.VirtualListSize) || (!this.VirtualMode && index >= this.itemCount))
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (base.IsHandleCreated)
			{
				NativeMethods.LVITEM lvitem = default(NativeMethods.LVITEM);
				lvitem.mask = 2;
				lvitem.iItem = index;
				lvitem.iImage = image;
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.LVM_SETITEM, 0, ref lvitem);
			}
		}

		// Token: 0x06004475 RID: 17525 RVA: 0x000F7874 File Offset: 0x000F6874
		internal void SetItemIndentCount(int index, int indentCount)
		{
			if (index < 0 || (this.VirtualMode && index >= this.VirtualListSize) || (!this.VirtualMode && index >= this.itemCount))
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (base.IsHandleCreated)
			{
				NativeMethods.LVITEM lvitem = default(NativeMethods.LVITEM);
				lvitem.mask = 16;
				lvitem.iItem = index;
				lvitem.iIndent = indentCount;
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.LVM_SETITEM, 0, ref lvitem);
			}
		}

		// Token: 0x06004476 RID: 17526 RVA: 0x000F7920 File Offset: 0x000F6920
		internal void SetItemPosition(int index, int x, int y)
		{
			if (this.VirtualMode)
			{
				return;
			}
			if (index < 0 || index >= this.itemCount)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			NativeMethods.POINT point = new NativeMethods.POINT();
			point.x = x;
			point.y = y;
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4145, index, point);
		}

		// Token: 0x06004477 RID: 17527 RVA: 0x000F79A4 File Offset: 0x000F69A4
		internal void SetItemState(int index, int state, int mask)
		{
			if (index < -1 || (this.VirtualMode && index >= this.VirtualListSize) || (!this.VirtualMode && index >= this.itemCount))
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (base.IsHandleCreated)
			{
				NativeMethods.LVITEM lvitem = default(NativeMethods.LVITEM);
				lvitem.mask = 8;
				lvitem.state = state;
				lvitem.stateMask = mask;
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4139, index, ref lvitem);
			}
		}

		// Token: 0x06004478 RID: 17528 RVA: 0x000F7A50 File Offset: 0x000F6A50
		internal void SetItemText(int itemIndex, int subItemIndex, string text)
		{
			NativeMethods.LVITEM lvitem = default(NativeMethods.LVITEM);
			this.SetItemText(itemIndex, subItemIndex, text, ref lvitem);
		}

		// Token: 0x06004479 RID: 17529 RVA: 0x000F7A70 File Offset: 0x000F6A70
		private void SetItemText(int itemIndex, int subItemIndex, string text, ref NativeMethods.LVITEM lvItem)
		{
			if (this.View == View.List && subItemIndex == 0)
			{
				int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4125, 0, 0);
				Graphics graphics = base.CreateGraphicsInternal();
				int num2 = 0;
				try
				{
					num2 = Size.Ceiling(graphics.MeasureString(text, this.Font)).Width;
				}
				finally
				{
					graphics.Dispose();
				}
				if (num2 > num)
				{
					this.SetColumnWidth(0, num2);
				}
			}
			lvItem.mask = 1;
			lvItem.iItem = itemIndex;
			lvItem.iSubItem = subItemIndex;
			lvItem.pszText = text;
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), NativeMethods.LVM_SETITEMTEXT, itemIndex, ref lvItem);
		}

		// Token: 0x0600447A RID: 17530 RVA: 0x000F7B2C File Offset: 0x000F6B2C
		internal void SetSelectionMark(int itemIndex)
		{
			if (itemIndex < 0 || itemIndex >= this.Items.Count)
			{
				return;
			}
			base.SendMessage(4163, 0, itemIndex);
		}

		// Token: 0x0600447B RID: 17531 RVA: 0x000F7B50 File Offset: 0x000F6B50
		private void SmallImageListRecreateHandle(object sender, EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				IntPtr intPtr = ((this.SmallImageList == null) ? IntPtr.Zero : this.SmallImageList.Handle);
				base.SendMessage(4099, (IntPtr)1, intPtr);
				this.ForceCheckBoxUpdate();
			}
		}

		// Token: 0x0600447C RID: 17532 RVA: 0x000F7B9C File Offset: 0x000F6B9C
		public void Sort()
		{
			if (this.VirtualMode)
			{
				throw new InvalidOperationException(SR.GetString("ListViewSortNotAllowedInVirtualListView"));
			}
			this.ApplyUpdateCachedItems();
			if (base.IsHandleCreated && this.listItemSorter != null)
			{
				NativeMethods.ListViewCompareCallback listViewCompareCallback = new NativeMethods.ListViewCompareCallback(this.CompareFunc);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4144, IntPtr.Zero, listViewCompareCallback);
			}
		}

		// Token: 0x0600447D RID: 17533 RVA: 0x000F7C04 File Offset: 0x000F6C04
		private void StateImageListRecreateHandle(object sender, EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				IntPtr intPtr = IntPtr.Zero;
				if (this.StateImageList != null)
				{
					intPtr = this.imageListState.Handle;
				}
				base.SendMessage(4099, (IntPtr)2, intPtr);
			}
		}

		// Token: 0x0600447E RID: 17534 RVA: 0x000F7C48 File Offset: 0x000F6C48
		public override string ToString()
		{
			string text = base.ToString();
			if (this.listItemsArray != null)
			{
				text = text + ", Items.Count: " + this.listItemsArray.Count.ToString(CultureInfo.CurrentCulture);
				if (this.listItemsArray.Count > 0)
				{
					string text2 = this.listItemsArray[0].ToString();
					string text3 = ((text2.Length > 40) ? text2.Substring(0, 40) : text2);
					text = text + ", Items[0]: " + text3;
				}
			}
			else if (this.Items != null)
			{
				text = text + ", Items.Count: " + this.Items.Count.ToString(CultureInfo.CurrentCulture);
				if (this.Items.Count > 0 && !this.VirtualMode)
				{
					string text4 = ((this.Items[0] == null) ? "null" : this.Items[0].ToString());
					string text5 = ((text4.Length > 40) ? text4.Substring(0, 40) : text4);
					text = text + ", Items[0]: " + text5;
				}
			}
			return text;
		}

		// Token: 0x0600447F RID: 17535 RVA: 0x000F7D6C File Offset: 0x000F6D6C
		internal void UpdateListViewItemsLocations()
		{
			if (!this.VirtualMode && base.IsHandleCreated && this.AutoArrange)
			{
				if (this.View != View.LargeIcon)
				{
					if (this.View != View.SmallIcon)
					{
						return;
					}
				}
				try
				{
					this.BeginUpdate();
					base.SendMessage(4138, -1, 0);
				}
				finally
				{
					this.EndUpdate();
				}
			}
		}

		// Token: 0x06004480 RID: 17536 RVA: 0x000F7DD0 File Offset: 0x000F6DD0
		private void UpdateColumnWidths(ColumnHeaderAutoResizeStyle headerAutoResize)
		{
			if (this.columnHeaders != null)
			{
				for (int i = 0; i < this.columnHeaders.Length; i++)
				{
					this.SetColumnWidth(i, headerAutoResize);
				}
			}
		}

		// Token: 0x06004481 RID: 17537 RVA: 0x000F7E00 File Offset: 0x000F6E00
		protected void UpdateExtendedStyles()
		{
			if (base.IsHandleCreated)
			{
				int num = 0;
				int num2 = 68861;
				switch (this.activation)
				{
				case ItemActivation.OneClick:
					num |= 64;
					break;
				case ItemActivation.TwoClick:
					num |= 128;
					break;
				}
				if (this.AllowColumnReorder)
				{
					num |= 16;
				}
				if (this.CheckBoxes)
				{
					num |= 4;
				}
				if (this.DoubleBuffered)
				{
					num |= 65536;
				}
				if (this.FullRowSelect)
				{
					num |= 32;
				}
				if (this.GridLines)
				{
					num |= 1;
				}
				if (this.HoverSelection)
				{
					num |= 8;
				}
				if (this.HotTracking)
				{
					num |= 2048;
				}
				if (this.ShowItemToolTips)
				{
					num |= 1024;
				}
				base.SendMessage(4150, num2, num);
				base.Invalidate();
			}
		}

		// Token: 0x06004482 RID: 17538 RVA: 0x000F7ECC File Offset: 0x000F6ECC
		internal void UpdateGroupNative(ListViewGroup group)
		{
			NativeMethods.LVGROUP lvgroup = new NativeMethods.LVGROUP();
			try
			{
				lvgroup = this.GetLVGROUP(group);
				(int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4243, group.ID, lvgroup);
			}
			finally
			{
				this.DestroyLVGROUP(lvgroup);
			}
			base.Invalidate();
		}

		// Token: 0x06004483 RID: 17539 RVA: 0x000F7F2C File Offset: 0x000F6F2C
		internal void UpdateGroupView()
		{
			if (base.IsHandleCreated && this.ComctlSupportsVisualStyles && !this.VirtualMode)
			{
				(int)base.SendMessage(4253, this.GroupsEnabled ? 1 : 0, 0);
			}
		}

		// Token: 0x06004484 RID: 17540 RVA: 0x000F7F64 File Offset: 0x000F6F64
		private void UpdateTileView()
		{
			NativeMethods.LVTILEVIEWINFO lvtileviewinfo = new NativeMethods.LVTILEVIEWINFO();
			lvtileviewinfo.dwMask = 2;
			lvtileviewinfo.cLines = ((this.columnHeaders != null) ? this.columnHeaders.Length : 0);
			lvtileviewinfo.dwMask |= 1;
			lvtileviewinfo.dwFlags = 3;
			lvtileviewinfo.sizeTile = new NativeMethods.SIZE(this.TileSize.Width, this.TileSize.Height);
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4258, 0, lvtileviewinfo);
		}

		// Token: 0x06004485 RID: 17541 RVA: 0x000F7FEC File Offset: 0x000F6FEC
		private void WmNmClick(ref Message m)
		{
			if (this.CheckBoxes)
			{
				Point point = Cursor.Position;
				point = base.PointToClientInternal(point);
				NativeMethods.LVHITTESTINFO lvhittestinfo = new NativeMethods.LVHITTESTINFO();
				lvhittestinfo.pt_x = point.X;
				lvhittestinfo.pt_y = point.Y;
				int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4114, 0, lvhittestinfo);
				if (num != -1 && (lvhittestinfo.flags & 8) != 0)
				{
					ListViewItem listViewItem = this.Items[num];
					if (listViewItem.Selected)
					{
						bool flag = !listViewItem.Checked;
						if (!this.VirtualMode)
						{
							foreach (object obj in this.SelectedItems)
							{
								ListViewItem listViewItem2 = (ListViewItem)obj;
								if (listViewItem2 != listViewItem)
								{
									listViewItem2.Checked = flag;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06004486 RID: 17542 RVA: 0x000F80E4 File Offset: 0x000F70E4
		private void WmNmDblClick(ref Message m)
		{
			if (this.CheckBoxes)
			{
				Point point = Cursor.Position;
				point = base.PointToClientInternal(point);
				NativeMethods.LVHITTESTINFO lvhittestinfo = new NativeMethods.LVHITTESTINFO();
				lvhittestinfo.pt_x = point.X;
				lvhittestinfo.pt_y = point.Y;
				int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4114, 0, lvhittestinfo);
				if (num != -1 && (lvhittestinfo.flags & 14) != 0)
				{
					ListViewItem listViewItem = this.Items[num];
					listViewItem.Checked = !listViewItem.Checked;
				}
			}
		}

		// Token: 0x06004487 RID: 17543 RVA: 0x000F8170 File Offset: 0x000F7170
		private void WmMouseDown(ref Message m, MouseButtons button, int clicks)
		{
			this.listViewState[524288] = false;
			this.listViewState[1048576] = true;
			this.FocusInternal();
			int num = NativeMethods.Util.SignedLOWORD(m.LParam);
			int num2 = NativeMethods.Util.SignedHIWORD(m.LParam);
			this.OnMouseDown(new MouseEventArgs(button, clicks, num, num2, 0));
			if (!base.ValidationCancelled)
			{
				if (this.CheckBoxes && this.imageListState != null && this.imageListState.Images.Count < 2)
				{
					ListViewHitTestInfo listViewHitTestInfo = this.HitTest(num, num2);
					if (listViewHitTestInfo.Location != ListViewHitTestLocations.StateImage)
					{
						this.DefWndProc(ref m);
						return;
					}
				}
				else
				{
					this.DefWndProc(ref m);
				}
			}
		}

		// Token: 0x06004488 RID: 17544 RVA: 0x000F8220 File Offset: 0x000F7220
		private bool WmNotify(ref Message m)
		{
			/*
An exception occurred when decompiling this method (06004488)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Boolean System.Windows.Forms.ListView::WmNotify(System.Windows.Forms.Message&)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.ConvertToAst(List`1 body) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 1052
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.ConvertToAst(List`1 body, HashSet`1 ehs) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 888
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 280
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 117
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x06004489 RID: 17545 RVA: 0x000F8AF8 File Offset: 0x000F7AF8
		private Font GetListHeaderFont()
		{
			IntPtr intPtr = UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4127, 0, 0);
			IntPtr intPtr2 = UnsafeNativeMethods.SendMessage(new HandleRef(this, intPtr), 49, 0, 0);
			IntSecurity.ObjectFromWin32Handle.Assert();
			return Font.FromHfont(intPtr2);
		}

		// Token: 0x0600448A RID: 17546 RVA: 0x000F8B40 File Offset: 0x000F7B40
		private int GetIndexOfClickedItem(NativeMethods.LVHITTESTINFO lvhi)
		{
			Point point = Cursor.Position;
			point = base.PointToClientInternal(point);
			lvhi.pt_x = point.X;
			lvhi.pt_y = point.Y;
			return (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4114, 0, lvhi);
		}

		// Token: 0x0600448B RID: 17547 RVA: 0x000F8B92 File Offset: 0x000F7B92
		internal void RecreateHandleInternal()
		{
			if (base.IsHandleCreated && this.StateImageList != null)
			{
				base.SendMessage(4099, 2, IntPtr.Zero);
			}
			base.RecreateHandle();
		}

		// Token: 0x0600448C RID: 17548 RVA: 0x000F8BBC File Offset: 0x000F7BBC
		private unsafe void WmReflectNotify(ref Message m)
		{
			NativeMethods.NMHDR* ptr = (NativeMethods.NMHDR*)(void*)m.LParam;
			int code = ptr->code;
			if (code <= -155)
			{
				switch (code)
				{
				case -176:
					goto IL_014E;
				case -175:
					break;
				default:
				{
					if (code != -155)
					{
						goto IL_062D;
					}
					if (!this.CheckBoxes)
					{
						return;
					}
					NativeMethods.NMLVKEYDOWN nmlvkeydown = (NativeMethods.NMLVKEYDOWN)m.GetLParam(typeof(NativeMethods.NMLVKEYDOWN));
					if (nmlvkeydown.wVKey != 32)
					{
						return;
					}
					ListViewItem focusedItem = this.FocusedItem;
					if (focusedItem == null)
					{
						return;
					}
					bool flag = !focusedItem.Checked;
					if (!this.VirtualMode)
					{
						using (IEnumerator enumerator = this.SelectedItems.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object obj = enumerator.Current;
								ListViewItem listViewItem = (ListViewItem)obj;
								if (listViewItem != focusedItem)
								{
									listViewItem.Checked = flag;
								}
							}
							return;
						}
						goto IL_05FC;
					}
					return;
				}
				}
			}
			else
			{
				switch (code)
				{
				case -114:
					this.OnItemActivate(EventArgs.Empty);
					return;
				case -113:
					goto IL_05FC;
				case -112:
				case -110:
				case -107:
				case -104:
				case -103:
				case -102:
					goto IL_062D;
				case -111:
					if (!this.ItemCollectionChangedInMouseDown)
					{
						NativeMethods.NMLISTVIEW nmlistview = (NativeMethods.NMLISTVIEW)m.GetLParam(typeof(NativeMethods.NMLISTVIEW));
						this.LvnBeginDrag(MouseButtons.Right, nmlistview);
						return;
					}
					return;
				case -109:
					if (!this.ItemCollectionChangedInMouseDown)
					{
						NativeMethods.NMLISTVIEW nmlistview2 = (NativeMethods.NMLISTVIEW)m.GetLParam(typeof(NativeMethods.NMLISTVIEW));
						this.LvnBeginDrag(MouseButtons.Left, nmlistview2);
						return;
					}
					return;
				case -108:
				{
					NativeMethods.NMLISTVIEW nmlistview3 = (NativeMethods.NMLISTVIEW)m.GetLParam(typeof(NativeMethods.NMLISTVIEW));
					this.listViewState[131072] = true;
					this.columnIndex = nmlistview3.iSubItem;
					return;
				}
				case -106:
					goto IL_014E;
				case -105:
					break;
				case -101:
				{
					NativeMethods.NMLISTVIEW* ptr2 = (NativeMethods.NMLISTVIEW*)(void*)m.LParam;
					if ((ptr2->uChanged & 8) == 0)
					{
						return;
					}
					CheckState checkState = (((ptr2->uOldState & 61440) >> 12 == 1) ? CheckState.Unchecked : CheckState.Checked);
					CheckState checkState2 = (((ptr2->uNewState & 61440) >> 12 == 1) ? CheckState.Unchecked : CheckState.Checked);
					if (checkState2 != checkState)
					{
						ItemCheckedEventArgs itemCheckedEventArgs = new ItemCheckedEventArgs(this.Items[ptr2->iItem]);
						this.OnItemChecked(itemCheckedEventArgs);
					}
					int num = ptr2->uOldState & 2;
					int num2 = ptr2->uNewState & 2;
					if (num2 == num)
					{
						return;
					}
					if (this.VirtualMode && ptr2->iItem == -1)
					{
						if (this.VirtualListSize > 0)
						{
							ListViewVirtualItemsSelectionRangeChangedEventArgs listViewVirtualItemsSelectionRangeChangedEventArgs = new ListViewVirtualItemsSelectionRangeChangedEventArgs(0, this.VirtualListSize - 1, num2 != 0);
							this.OnVirtualItemsSelectionRangeChanged(listViewVirtualItemsSelectionRangeChangedEventArgs);
						}
					}
					else if (this.Items.Count > 0)
					{
						ListViewItemSelectionChangedEventArgs listViewItemSelectionChangedEventArgs = new ListViewItemSelectionChangedEventArgs(this.Items[ptr2->iItem], ptr2->iItem, num2 != 0);
						this.OnItemSelectionChanged(listViewItemSelectionChangedEventArgs);
					}
					if (this.Items.Count == 0 || this.Items[this.Items.Count - 1] != null)
					{
						this.listViewState1[16] = false;
						this.OnSelectedIndexChanged(EventArgs.Empty);
						return;
					}
					this.listViewState1[16] = true;
					return;
				}
				case -100:
				{
					NativeMethods.NMLISTVIEW* ptr3 = (NativeMethods.NMLISTVIEW*)(void*)m.LParam;
					if ((ptr3->uChanged & 8) == 0)
					{
						return;
					}
					CheckState checkState3 = (((ptr3->uOldState & 61440) >> 12 == 1) ? CheckState.Unchecked : CheckState.Checked);
					CheckState checkState4 = (((ptr3->uNewState & 61440) >> 12 == 1) ? CheckState.Unchecked : CheckState.Checked);
					if (checkState3 != checkState4)
					{
						ItemCheckEventArgs itemCheckEventArgs = new ItemCheckEventArgs(ptr3->iItem, checkState4, checkState3);
						this.OnItemCheck(itemCheckEventArgs);
						m.Result = (IntPtr)((((itemCheckEventArgs.NewValue == CheckState.Unchecked) ? CheckState.Unchecked : CheckState.Checked) == checkState3) ? 1 : 0);
						return;
					}
					return;
				}
				default:
					if (code != -12)
					{
						switch (code)
						{
						case -6:
							goto IL_0517;
						case -5:
							break;
						case -4:
							goto IL_062D;
						case -3:
							this.WmNmDblClick(ref m);
							goto IL_0517;
						case -2:
							this.WmNmClick(ref m);
							break;
						default:
							goto IL_062D;
						}
						NativeMethods.LVHITTESTINFO lvhittestinfo = new NativeMethods.LVHITTESTINFO();
						int indexOfClickedItem = this.GetIndexOfClickedItem(lvhittestinfo);
						MouseButtons mouseButtons = ((ptr->code == -2) ? MouseButtons.Left : MouseButtons.Right);
						Point point = Cursor.Position;
						point = base.PointToClientInternal(point);
						if (!base.ValidationCancelled && indexOfClickedItem != -1)
						{
							this.OnClick(EventArgs.Empty);
							this.OnMouseClick(new MouseEventArgs(mouseButtons, 1, point.X, point.Y, 0));
						}
						if (!this.listViewState[524288])
						{
							this.OnMouseUp(new MouseEventArgs(mouseButtons, 1, point.X, point.Y, 0));
							this.listViewState[524288] = true;
							return;
						}
						return;
						IL_0517:
						NativeMethods.LVHITTESTINFO lvhittestinfo2 = new NativeMethods.LVHITTESTINFO();
						int indexOfClickedItem2 = this.GetIndexOfClickedItem(lvhittestinfo2);
						if (indexOfClickedItem2 != -1)
						{
							this.listViewState[262144] = true;
						}
						this.listViewState[524288] = false;
						base.CaptureInternal = true;
						return;
					}
					this.CustomDraw(ref m);
					return;
				}
			}
			NativeMethods.NMLVDISPINFO_NOTEXT nmlvdispinfo_NOTEXT = (NativeMethods.NMLVDISPINFO_NOTEXT)m.GetLParam(typeof(NativeMethods.NMLVDISPINFO_NOTEXT));
			LabelEditEventArgs labelEditEventArgs = new LabelEditEventArgs(nmlvdispinfo_NOTEXT.item.iItem);
			this.OnBeforeLabelEdit(labelEditEventArgs);
			m.Result = (IntPtr)(labelEditEventArgs.CancelEdit ? 1 : 0);
			this.listViewState[16384] = !labelEditEventArgs.CancelEdit;
			return;
			IL_014E:
			this.listViewState[16384] = false;
			NativeMethods.NMLVDISPINFO nmlvdispinfo = (NativeMethods.NMLVDISPINFO)m.GetLParam(typeof(NativeMethods.NMLVDISPINFO));
			LabelEditEventArgs labelEditEventArgs2 = new LabelEditEventArgs(nmlvdispinfo.item.iItem, nmlvdispinfo.item.pszText);
			this.OnAfterLabelEdit(labelEditEventArgs2);
			m.Result = (IntPtr)(labelEditEventArgs2.CancelEdit ? 0 : 1);
			if (!labelEditEventArgs2.CancelEdit && nmlvdispinfo.item.pszText != null)
			{
				this.Items[nmlvdispinfo.item.iItem].Text = nmlvdispinfo.item.pszText;
				return;
			}
			return;
			IL_05FC:
			NativeMethods.NMLVCACHEHINT nmlvcachehint = (NativeMethods.NMLVCACHEHINT)m.GetLParam(typeof(NativeMethods.NMLVCACHEHINT));
			this.OnCacheVirtualItems(new CacheVirtualItemsEventArgs(nmlvcachehint.iFrom, nmlvcachehint.iTo));
			return;
			IL_062D:
			if (ptr->code == NativeMethods.LVN_GETDISPINFO)
			{
				if (this.VirtualMode && m.LParam != IntPtr.Zero)
				{
					NativeMethods.NMLVDISPINFO_NOTEXT nmlvdispinfo_NOTEXT2 = (NativeMethods.NMLVDISPINFO_NOTEXT)m.GetLParam(typeof(NativeMethods.NMLVDISPINFO_NOTEXT));
					RetrieveVirtualItemEventArgs retrieveVirtualItemEventArgs = new RetrieveVirtualItemEventArgs(nmlvdispinfo_NOTEXT2.item.iItem);
					this.OnRetrieveVirtualItem(retrieveVirtualItemEventArgs);
					ListViewItem item = retrieveVirtualItemEventArgs.Item;
					if (item == null)
					{
						throw new InvalidOperationException(SR.GetString("ListViewVirtualItemRequired"));
					}
					item.SetItemIndex(this, nmlvdispinfo_NOTEXT2.item.iItem);
					if ((nmlvdispinfo_NOTEXT2.item.mask & 1) != 0)
					{
						string text;
						if (nmlvdispinfo_NOTEXT2.item.iSubItem == 0)
						{
							text = item.Text;
						}
						else
						{
							if (item.SubItems.Count <= nmlvdispinfo_NOTEXT2.item.iSubItem)
							{
								throw new InvalidOperationException(SR.GetString("ListViewVirtualModeCantAccessSubItem"));
							}
							text = item.SubItems[nmlvdispinfo_NOTEXT2.item.iSubItem].Text;
						}
						if (nmlvdispinfo_NOTEXT2.item.cchTextMax <= text.Length)
						{
							text = text.Substring(0, nmlvdispinfo_NOTEXT2.item.cchTextMax - 1);
						}
						if (Marshal.SystemDefaultCharSize == 1)
						{
							byte[] bytes = Encoding.Default.GetBytes(text + "\0");
							Marshal.Copy(bytes, 0, nmlvdispinfo_NOTEXT2.item.pszText, text.Length + 1);
						}
						else
						{
							char[] array = (text + "\0").ToCharArray();
							Marshal.Copy(array, 0, nmlvdispinfo_NOTEXT2.item.pszText, text.Length + 1);
						}
					}
					if ((nmlvdispinfo_NOTEXT2.item.mask & 2) != 0 && item.ImageIndex != -1)
					{
						nmlvdispinfo_NOTEXT2.item.iImage = item.ImageIndex;
					}
					if ((nmlvdispinfo_NOTEXT2.item.mask & 16) != 0)
					{
						nmlvdispinfo_NOTEXT2.item.iIndent = item.IndentCount;
					}
					if ((nmlvdispinfo_NOTEXT2.item.stateMask & 61440) != 0)
					{
						NativeMethods.NMLVDISPINFO_NOTEXT nmlvdispinfo_NOTEXT3 = nmlvdispinfo_NOTEXT2;
						nmlvdispinfo_NOTEXT3.item.state = nmlvdispinfo_NOTEXT3.item.state | item.RawStateImageIndex;
					}
					Marshal.StructureToPtr(nmlvdispinfo_NOTEXT2, m.LParam, false);
					return;
				}
			}
			else if (ptr->code == -115)
			{
				if (this.VirtualMode && m.LParam != IntPtr.Zero)
				{
					NativeMethods.NMLVODSTATECHANGE nmlvodstatechange = (NativeMethods.NMLVODSTATECHANGE)m.GetLParam(typeof(NativeMethods.NMLVODSTATECHANGE));
					bool flag2 = (nmlvodstatechange.uNewState & 2) != (nmlvodstatechange.uOldState & 2);
					if (flag2)
					{
						ListViewVirtualItemsSelectionRangeChangedEventArgs listViewVirtualItemsSelectionRangeChangedEventArgs2 = new ListViewVirtualItemsSelectionRangeChangedEventArgs(nmlvodstatechange.iFrom, nmlvodstatechange.iTo - 1, (nmlvodstatechange.uNewState & 2) != 0);
						this.OnVirtualItemsSelectionRangeChanged(listViewVirtualItemsSelectionRangeChangedEventArgs2);
						return;
					}
				}
			}
			else if (ptr->code == NativeMethods.LVN_GETINFOTIP)
			{
				if (this.ShowItemToolTips && m.LParam != IntPtr.Zero)
				{
					NativeMethods.NMLVGETINFOTIP nmlvgetinfotip = (NativeMethods.NMLVGETINFOTIP)m.GetLParam(typeof(NativeMethods.NMLVGETINFOTIP));
					ListViewItem listViewItem2 = this.Items[nmlvgetinfotip.item];
					if (listViewItem2 != null && !string.IsNullOrEmpty(listViewItem2.ToolTipText))
					{
						UnsafeNativeMethods.SendMessage(new HandleRef(this, ptr->hwndFrom), 1048, 0, SystemInformation.MaxWindowTrackSize.Width);
						if (Marshal.SystemDefaultCharSize == 1)
						{
							byte[] bytes2 = Encoding.Default.GetBytes(listViewItem2.ToolTipText + "\0");
							Marshal.Copy(bytes2, 0, nmlvgetinfotip.lpszText, Math.Min(bytes2.Length, nmlvgetinfotip.cchTextMax));
						}
						else
						{
							char[] array2 = (listViewItem2.ToolTipText + "\0").ToCharArray();
							Marshal.Copy(array2, 0, nmlvgetinfotip.lpszText, Math.Min(array2.Length, nmlvgetinfotip.cchTextMax));
						}
						Marshal.StructureToPtr(nmlvgetinfotip, m.LParam, false);
						return;
					}
				}
			}
			else if (ptr->code == NativeMethods.LVN_ODFINDITEM && this.VirtualMode)
			{
				NativeMethods.NMLVFINDITEM nmlvfinditem = (NativeMethods.NMLVFINDITEM)m.GetLParam(typeof(NativeMethods.NMLVFINDITEM));
				if ((nmlvfinditem.lvfi.flags & 1) != 0)
				{
					m.Result = (IntPtr)(-1);
					return;
				}
				bool flag3 = (nmlvfinditem.lvfi.flags & 2) != 0 || (nmlvfinditem.lvfi.flags & 8) != 0;
				bool flag4 = (nmlvfinditem.lvfi.flags & 8) != 0;
				string text2 = string.Empty;
				if (flag3)
				{
					text2 = nmlvfinditem.lvfi.psz;
				}
				Point empty = Point.Empty;
				if ((nmlvfinditem.lvfi.flags & 64) != 0)
				{
					empty = new Point(nmlvfinditem.lvfi.ptX, nmlvfinditem.lvfi.ptY);
				}
				SearchDirectionHint searchDirectionHint = SearchDirectionHint.Down;
				if ((nmlvfinditem.lvfi.flags & 64) != 0)
				{
					searchDirectionHint = (SearchDirectionHint)nmlvfinditem.lvfi.vkDirection;
				}
				int iStart = nmlvfinditem.iStart;
				if (iStart >= this.VirtualListSize)
				{
				}
				SearchForVirtualItemEventArgs searchForVirtualItemEventArgs = new SearchForVirtualItemEventArgs(flag3, flag4, false, text2, empty, searchDirectionHint, nmlvfinditem.iStart);
				this.OnSearchForVirtualItem(searchForVirtualItemEventArgs);
				if (searchForVirtualItemEventArgs.Index != -1)
				{
					m.Result = (IntPtr)searchForVirtualItemEventArgs.Index;
					return;
				}
				m.Result = (IntPtr)(-1);
			}
		}

		// Token: 0x0600448D RID: 17549 RVA: 0x000F9754 File Offset: 0x000F8754
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

		// Token: 0x0600448E RID: 17550 RVA: 0x000F9824 File Offset: 0x000F8824
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 275)
			{
				if (msg <= 15)
				{
					if (msg != 7)
					{
						if (msg == 15)
						{
							base.WndProc(ref m);
							base.BeginInvoke(new MethodInvoker(this.CleanPreviousBackgroundImageFiles));
							return;
						}
					}
					else
					{
						base.WndProc(ref m);
						if (!base.RecreatingHandle && !this.ListViewHandleDestroyed && this.FocusedItem == null && this.Items.Count > 0)
						{
							this.Items[0].Focused = true;
							return;
						}
						return;
					}
				}
				else if (msg != 78)
				{
					if (msg == 275)
					{
						if ((int)m.WParam != 48 || !this.ComctlSupportsVisualStyles)
						{
							base.WndProc(ref m);
							return;
						}
						return;
					}
				}
				else if (this.WmNotify(ref m))
				{
					return;
				}
			}
			else if (msg <= 675)
			{
				switch (msg)
				{
				case 512:
					if (this.listViewState[1048576] && !this.listViewState[524288] && Control.MouseButtons == MouseButtons.None)
					{
						this.OnMouseUp(new MouseEventArgs(this.downButton, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
						this.listViewState[524288] = true;
					}
					base.CaptureInternal = false;
					base.WndProc(ref m);
					return;
				case 513:
					this.ItemCollectionChangedInMouseDown = false;
					this.WmMouseDown(ref m, MouseButtons.Left, 1);
					this.downButton = MouseButtons.Left;
					return;
				case 514:
				case 517:
				case 520:
				{
					NativeMethods.LVHITTESTINFO lvhittestinfo = new NativeMethods.LVHITTESTINFO();
					int indexOfClickedItem = this.GetIndexOfClickedItem(lvhittestinfo);
					if (!base.ValidationCancelled && this.listViewState[262144] && indexOfClickedItem != -1)
					{
						this.listViewState[262144] = false;
						this.OnDoubleClick(EventArgs.Empty);
						this.OnMouseDoubleClick(new MouseEventArgs(this.downButton, 2, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
					}
					if (!this.listViewState[524288])
					{
						this.OnMouseUp(new MouseEventArgs(this.downButton, 1, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam), 0));
						this.listViewState[1048576] = false;
					}
					this.ItemCollectionChangedInMouseDown = false;
					this.listViewState[524288] = true;
					base.CaptureInternal = false;
					return;
				}
				case 515:
					this.ItemCollectionChangedInMouseDown = false;
					base.CaptureInternal = true;
					this.WmMouseDown(ref m, MouseButtons.Left, 2);
					return;
				case 516:
					this.WmMouseDown(ref m, MouseButtons.Right, 1);
					this.downButton = MouseButtons.Right;
					return;
				case 518:
					this.WmMouseDown(ref m, MouseButtons.Right, 2);
					return;
				case 519:
					this.WmMouseDown(ref m, MouseButtons.Middle, 1);
					this.downButton = MouseButtons.Middle;
					return;
				case 521:
					this.WmMouseDown(ref m, MouseButtons.Middle, 2);
					return;
				default:
					switch (msg)
					{
					case 673:
						if (this.HoverSelection)
						{
							base.WndProc(ref m);
							return;
						}
						this.OnMouseHover(EventArgs.Empty);
						return;
					case 675:
						this.prevHoveredItem = null;
						base.WndProc(ref m);
						return;
					}
					break;
				}
			}
			else
			{
				if (msg == 791)
				{
					this.WmPrint(ref m);
					return;
				}
				if (msg == 8270)
				{
					this.WmReflectNotify(ref m);
					return;
				}
			}
			base.WndProc(ref m);
		}

		// Token: 0x040020DD RID: 8413
		private const int MASK_HITTESTFLAG = 247;

		// Token: 0x040020DE RID: 8414
		private const int LISTVIEWSTATE_ownerDraw = 1;

		// Token: 0x040020DF RID: 8415
		private const int LISTVIEWSTATE_allowColumnReorder = 2;

		// Token: 0x040020E0 RID: 8416
		private const int LISTVIEWSTATE_autoArrange = 4;

		// Token: 0x040020E1 RID: 8417
		private const int LISTVIEWSTATE_checkBoxes = 8;

		// Token: 0x040020E2 RID: 8418
		private const int LISTVIEWSTATE_fullRowSelect = 16;

		// Token: 0x040020E3 RID: 8419
		private const int LISTVIEWSTATE_gridLines = 32;

		// Token: 0x040020E4 RID: 8420
		private const int LISTVIEWSTATE_hideSelection = 64;

		// Token: 0x040020E5 RID: 8421
		private const int LISTVIEWSTATE_hotTracking = 128;

		// Token: 0x040020E6 RID: 8422
		private const int LISTVIEWSTATE_labelEdit = 256;

		// Token: 0x040020E7 RID: 8423
		private const int LISTVIEWSTATE_labelWrap = 512;

		// Token: 0x040020E8 RID: 8424
		private const int LISTVIEWSTATE_multiSelect = 1024;

		// Token: 0x040020E9 RID: 8425
		private const int LISTVIEWSTATE_scrollable = 2048;

		// Token: 0x040020EA RID: 8426
		private const int LISTVIEWSTATE_hoverSelection = 4096;

		// Token: 0x040020EB RID: 8427
		private const int LISTVIEWSTATE_nonclickHdr = 8192;

		// Token: 0x040020EC RID: 8428
		private const int LISTVIEWSTATE_inLabelEdit = 16384;

		// Token: 0x040020ED RID: 8429
		private const int LISTVIEWSTATE_showItemToolTips = 32768;

		// Token: 0x040020EE RID: 8430
		private const int LISTVIEWSTATE_backgroundImageTiled = 65536;

		// Token: 0x040020EF RID: 8431
		private const int LISTVIEWSTATE_columnClicked = 131072;

		// Token: 0x040020F0 RID: 8432
		private const int LISTVIEWSTATE_doubleclickFired = 262144;

		// Token: 0x040020F1 RID: 8433
		private const int LISTVIEWSTATE_mouseUpFired = 524288;

		// Token: 0x040020F2 RID: 8434
		private const int LISTVIEWSTATE_expectingMouseUp = 1048576;

		// Token: 0x040020F3 RID: 8435
		private const int LISTVIEWSTATE_comctlSupportsVisualStyles = 2097152;

		// Token: 0x040020F4 RID: 8436
		private const int LISTVIEWSTATE_comctlSupportsVisualStylesTested = 4194304;

		// Token: 0x040020F5 RID: 8437
		private const int LISTVIEWSTATE_showGroups = 8388608;

		// Token: 0x040020F6 RID: 8438
		private const int LISTVIEWSTATE_handleDestroyed = 16777216;

		// Token: 0x040020F7 RID: 8439
		private const int LISTVIEWSTATE_virtualMode = 33554432;

		// Token: 0x040020F8 RID: 8440
		private const int LISTVIEWSTATE_headerControlTracking = 67108864;

		// Token: 0x040020F9 RID: 8441
		private const int LISTVIEWSTATE_itemCollectionChangedInMouseDown = 134217728;

		// Token: 0x040020FA RID: 8442
		private const int LISTVIEWSTATE_flipViewToLargeIconAndSmallIcon = 268435456;

		// Token: 0x040020FB RID: 8443
		private const int LISTVIEWSTATE_headerDividerDblClick = 536870912;

		// Token: 0x040020FC RID: 8444
		private const int LISTVIEWSTATE_columnResizeCancelled = 1073741824;

		// Token: 0x040020FD RID: 8445
		private const int LISTVIEWSTATE1_insertingItemsNatively = 1;

		// Token: 0x040020FE RID: 8446
		private const int LISTVIEWSTATE1_cancelledColumnWidthChanging = 2;

		// Token: 0x040020FF RID: 8447
		private const int LISTVIEWSTATE1_disposingImageLists = 4;

		// Token: 0x04002100 RID: 8448
		private const int LISTVIEWSTATE1_useCompatibleStateImageBehavior = 8;

		// Token: 0x04002101 RID: 8449
		private const int LISTVIEWSTATE1_selectedIndexChangedSkipped = 16;

		// Token: 0x04002102 RID: 8450
		private const int LVTOOLTIPTRACKING = 48;

		// Token: 0x04002103 RID: 8451
		private const int MAXTILECOLUMNS = 20;

		// Token: 0x04002104 RID: 8452
		private const int BKIMGARRAYSIZE = 8;

		// Token: 0x04002105 RID: 8453
		private static readonly object EVENT_CACHEVIRTUALITEMS = new object();

		// Token: 0x04002106 RID: 8454
		private static readonly object EVENT_COLUMNREORDERED = new object();

		// Token: 0x04002107 RID: 8455
		private static readonly object EVENT_COLUMNWIDTHCHANGED = new object();

		// Token: 0x04002108 RID: 8456
		private static readonly object EVENT_COLUMNWIDTHCHANGING = new object();

		// Token: 0x04002109 RID: 8457
		private static readonly object EVENT_DRAWCOLUMNHEADER = new object();

		// Token: 0x0400210A RID: 8458
		private static readonly object EVENT_DRAWITEM = new object();

		// Token: 0x0400210B RID: 8459
		private static readonly object EVENT_DRAWSUBITEM = new object();

		// Token: 0x0400210C RID: 8460
		private static readonly object EVENT_ITEMSELECTIONCHANGED = new object();

		// Token: 0x0400210D RID: 8461
		private static readonly object EVENT_RETRIEVEVIRTUALITEM = new object();

		// Token: 0x0400210E RID: 8462
		private static readonly object EVENT_SEARCHFORVIRTUALITEM = new object();

		// Token: 0x0400210F RID: 8463
		private static readonly object EVENT_SELECTEDINDEXCHANGED = new object();

		// Token: 0x04002110 RID: 8464
		private static readonly object EVENT_VIRTUALITEMSSELECTIONRANGECHANGED = new object();

		// Token: 0x04002111 RID: 8465
		private static readonly object EVENT_RIGHTTOLEFTLAYOUTCHANGED = new object();

		// Token: 0x04002112 RID: 8466
		private ItemActivation activation;

		// Token: 0x04002113 RID: 8467
		private ListViewAlignment alignStyle = ListViewAlignment.Top;

		// Token: 0x04002114 RID: 8468
		private BorderStyle borderStyle = BorderStyle.Fixed3D;

		// Token: 0x04002115 RID: 8469
		private ColumnHeaderStyle headerStyle = ColumnHeaderStyle.Clickable;

		// Token: 0x04002116 RID: 8470
		private SortOrder sorting;

		// Token: 0x04002117 RID: 8471
		private View viewStyle;

		// Token: 0x04002118 RID: 8472
		private string toolTipCaption = string.Empty;

		// Token: 0x04002119 RID: 8473
		private BitVector32 listViewState;

		// Token: 0x0400211A RID: 8474
		private BitVector32 listViewState1;

		// Token: 0x0400211B RID: 8475
		private Color odCacheForeColor = SystemColors.WindowText;

		// Token: 0x0400211C RID: 8476
		private Color odCacheBackColor = SystemColors.Window;

		// Token: 0x0400211D RID: 8477
		private Font odCacheFont;

		// Token: 0x0400211E RID: 8478
		private IntPtr odCacheFontHandle = IntPtr.Zero;

		// Token: 0x0400211F RID: 8479
		private Control.FontHandleWrapper odCacheFontHandleWrapper;

		// Token: 0x04002120 RID: 8480
		private ImageList imageListLarge;

		// Token: 0x04002121 RID: 8481
		private ImageList imageListSmall;

		// Token: 0x04002122 RID: 8482
		private ImageList imageListState;

		// Token: 0x04002123 RID: 8483
		private MouseButtons downButton;

		// Token: 0x04002124 RID: 8484
		private int itemCount;

		// Token: 0x04002125 RID: 8485
		private int columnIndex;

		// Token: 0x04002126 RID: 8486
		private int topIndex;

		// Token: 0x04002127 RID: 8487
		private bool hoveredAlready;

		// Token: 0x04002128 RID: 8488
		private bool rightToLeftLayout;

		// Token: 0x04002129 RID: 8489
		private int virtualListSize;

		// Token: 0x0400212A RID: 8490
		private ListViewGroup defaultGroup;

		// Token: 0x0400212B RID: 8491
		private Hashtable listItemsTable = new Hashtable();

		// Token: 0x0400212C RID: 8492
		private ArrayList listItemsArray = new ArrayList();

		// Token: 0x0400212D RID: 8493
		private Size tileSize = Size.Empty;

		// Token: 0x0400212E RID: 8494
		private static readonly int PropDelayedUpdateItems = PropertyStore.CreateKey();

		// Token: 0x0400212F RID: 8495
		private int updateCounter;

		// Token: 0x04002130 RID: 8496
		private ColumnHeader[] columnHeaders;

		// Token: 0x04002131 RID: 8497
		private ListView.ListViewItemCollection listItemCollection;

		// Token: 0x04002132 RID: 8498
		private ListView.ColumnHeaderCollection columnHeaderCollection;

		// Token: 0x04002133 RID: 8499
		private ListView.CheckedIndexCollection checkedIndexCollection;

		// Token: 0x04002134 RID: 8500
		private ListView.CheckedListViewItemCollection checkedListViewItemCollection;

		// Token: 0x04002135 RID: 8501
		private ListView.SelectedListViewItemCollection selectedListViewItemCollection;

		// Token: 0x04002136 RID: 8502
		private ListView.SelectedIndexCollection selectedIndexCollection;

		// Token: 0x04002137 RID: 8503
		private ListViewGroupCollection groups;

		// Token: 0x04002138 RID: 8504
		private ListViewInsertionMark insertionMark;

		// Token: 0x04002139 RID: 8505
		private LabelEditEventHandler onAfterLabelEdit;

		// Token: 0x0400213A RID: 8506
		private LabelEditEventHandler onBeforeLabelEdit;

		// Token: 0x0400213B RID: 8507
		private ColumnClickEventHandler onColumnClick;

		// Token: 0x0400213C RID: 8508
		private EventHandler onItemActivate;

		// Token: 0x0400213D RID: 8509
		private ItemCheckedEventHandler onItemChecked;

		// Token: 0x0400213E RID: 8510
		private ItemDragEventHandler onItemDrag;

		// Token: 0x0400213F RID: 8511
		private ItemCheckEventHandler onItemCheck;

		// Token: 0x04002140 RID: 8512
		private ListViewItemMouseHoverEventHandler onItemMouseHover;

		// Token: 0x04002141 RID: 8513
		private int nextID;

		// Token: 0x04002142 RID: 8514
		private List<ListViewItem> savedSelectedItems;

		// Token: 0x04002143 RID: 8515
		private List<ListViewItem> savedCheckedItems;

		// Token: 0x04002144 RID: 8516
		private IComparer listItemSorter;

		// Token: 0x04002145 RID: 8517
		private ListViewItem prevHoveredItem;

		// Token: 0x04002146 RID: 8518
		private string backgroundImageFileName = string.Empty;

		// Token: 0x04002147 RID: 8519
		private int bkImgFileNamesCount = -1;

		// Token: 0x04002148 RID: 8520
		private string[] bkImgFileNames;

		// Token: 0x04002149 RID: 8521
		private ColumnHeader columnHeaderClicked;

		// Token: 0x0400214A RID: 8522
		private int columnHeaderClickedWidth;

		// Token: 0x0400214B RID: 8523
		private int newWidthForColumnWidthChangingCancelled = -1;

		// Token: 0x02000481 RID: 1153
		internal class IconComparer : IComparer
		{
			// Token: 0x06004490 RID: 17552 RVA: 0x000F9C25 File Offset: 0x000F8C25
			public IconComparer(SortOrder currentSortOrder)
			{
				this.sortOrder = currentSortOrder;
			}

			// Token: 0x17000D71 RID: 3441
			// (set) Token: 0x06004491 RID: 17553 RVA: 0x000F9C34 File Offset: 0x000F8C34
			public SortOrder SortOrder
			{
				set
				{
					this.sortOrder = value;
				}
			}

			// Token: 0x06004492 RID: 17554 RVA: 0x000F9C40 File Offset: 0x000F8C40
			public int Compare(object obj1, object obj2)
			{
				ListViewItem listViewItem = (ListViewItem)obj1;
				ListViewItem listViewItem2 = (ListViewItem)obj2;
				if (this.sortOrder == SortOrder.Ascending)
				{
					return string.Compare(listViewItem.Text, listViewItem2.Text, false, CultureInfo.CurrentCulture);
				}
				return string.Compare(listViewItem2.Text, listViewItem.Text, false, CultureInfo.CurrentCulture);
			}

			// Token: 0x0400214C RID: 8524
			private SortOrder sortOrder;
		}

		// Token: 0x02000482 RID: 1154
		[ListBindable(false)]
		public class CheckedIndexCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x06004493 RID: 17555 RVA: 0x000F9C93 File Offset: 0x000F8C93
			public CheckedIndexCollection(ListView owner)
			{
				this.owner = owner;
			}

			// Token: 0x17000D72 RID: 3442
			// (get) Token: 0x06004494 RID: 17556 RVA: 0x000F9CA4 File Offset: 0x000F8CA4
			[Browsable(false)]
			public int Count
			{
				get
				{
					if (!this.owner.CheckBoxes)
					{
						return 0;
					}
					int num = 0;
					foreach (object obj in this.owner.Items)
					{
						ListViewItem listViewItem = (ListViewItem)obj;
						if (listViewItem != null && listViewItem.Checked)
						{
							num++;
						}
					}
					return num;
				}
			}

			// Token: 0x17000D73 RID: 3443
			// (get) Token: 0x06004495 RID: 17557 RVA: 0x000F9D1C File Offset: 0x000F8D1C
			private int[] IndicesArray
			{
				get
				{
					int[] array = new int[this.Count];
					int num = 0;
					int num2 = 0;
					while (num2 < this.owner.Items.Count && num < array.Length)
					{
						if (this.owner.Items[num2].Checked)
						{
							array[num++] = num2;
						}
						num2++;
					}
					return array;
				}
			}

			// Token: 0x17000D74 RID: 3444
			public int this[int index]
			{
				get
				{
					if (index < 0)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					int count = this.owner.Items.Count;
					int num = 0;
					for (int i = 0; i < count; i++)
					{
						ListViewItem listViewItem = this.owner.Items[i];
						if (listViewItem.Checked)
						{
							if (num == index)
							{
								return i;
							}
							num++;
						}
					}
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
			}

			// Token: 0x17000D75 RID: 3445
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			// Token: 0x17000D76 RID: 3446
			// (get) Token: 0x06004499 RID: 17561 RVA: 0x000F9E54 File Offset: 0x000F8E54
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x17000D77 RID: 3447
			// (get) Token: 0x0600449A RID: 17562 RVA: 0x000F9E57 File Offset: 0x000F8E57
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000D78 RID: 3448
			// (get) Token: 0x0600449B RID: 17563 RVA: 0x000F9E5A File Offset: 0x000F8E5A
			bool IList.IsFixedSize
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000D79 RID: 3449
			// (get) Token: 0x0600449C RID: 17564 RVA: 0x000F9E5D File Offset: 0x000F8E5D
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x0600449D RID: 17565 RVA: 0x000F9E60 File Offset: 0x000F8E60
			public bool Contains(int checkedIndex)
			{
				return this.owner.Items[checkedIndex].Checked;
			}

			// Token: 0x0600449E RID: 17566 RVA: 0x000F9E7D File Offset: 0x000F8E7D
			bool IList.Contains(object checkedIndex)
			{
				return checkedIndex is int && this.Contains((int)checkedIndex);
			}

			// Token: 0x0600449F RID: 17567 RVA: 0x000F9E98 File Offset: 0x000F8E98
			public int IndexOf(int checkedIndex)
			{
				int[] indicesArray = this.IndicesArray;
				for (int i = 0; i < indicesArray.Length; i++)
				{
					if (indicesArray[i] == checkedIndex)
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x060044A0 RID: 17568 RVA: 0x000F9EC3 File Offset: 0x000F8EC3
			int IList.IndexOf(object checkedIndex)
			{
				if (checkedIndex is int)
				{
					return this.IndexOf((int)checkedIndex);
				}
				return -1;
			}

			// Token: 0x060044A1 RID: 17569 RVA: 0x000F9EDB File Offset: 0x000F8EDB
			int IList.Add(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060044A2 RID: 17570 RVA: 0x000F9EE2 File Offset: 0x000F8EE2
			void IList.Clear()
			{
				throw new NotSupportedException();
			}

			// Token: 0x060044A3 RID: 17571 RVA: 0x000F9EE9 File Offset: 0x000F8EE9
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060044A4 RID: 17572 RVA: 0x000F9EF0 File Offset: 0x000F8EF0
			void IList.Remove(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060044A5 RID: 17573 RVA: 0x000F9EF7 File Offset: 0x000F8EF7
			void IList.RemoveAt(int index)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060044A6 RID: 17574 RVA: 0x000F9EFE File Offset: 0x000F8EFE
			void ICollection.CopyTo(Array dest, int index)
			{
				if (this.Count > 0)
				{
					Array.Copy(this.IndicesArray, 0, dest, index, this.Count);
				}
			}

			// Token: 0x060044A7 RID: 17575 RVA: 0x000F9F20 File Offset: 0x000F8F20
			public IEnumerator GetEnumerator()
			{
				int[] indicesArray = this.IndicesArray;
				if (indicesArray != null)
				{
					return indicesArray.GetEnumerator();
				}
				return new int[0].GetEnumerator();
			}

			// Token: 0x0400214D RID: 8525
			private ListView owner;
		}

		// Token: 0x02000483 RID: 1155
		[ListBindable(false)]
		public class CheckedListViewItemCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x060044A8 RID: 17576 RVA: 0x000F9F49 File Offset: 0x000F8F49
			public CheckedListViewItemCollection(ListView owner)
			{
				this.owner = owner;
			}

			// Token: 0x17000D7A RID: 3450
			// (get) Token: 0x060044A9 RID: 17577 RVA: 0x000F9F5F File Offset: 0x000F8F5F
			[Browsable(false)]
			public int Count
			{
				get
				{
					if (this.owner.VirtualMode)
					{
						throw new InvalidOperationException(SR.GetString("ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode"));
					}
					return this.owner.CheckedIndices.Count;
				}
			}

			// Token: 0x17000D7B RID: 3451
			// (get) Token: 0x060044AA RID: 17578 RVA: 0x000F9F90 File Offset: 0x000F8F90
			private ListViewItem[] ItemArray
			{
				get
				{
					ListViewItem[] array = new ListViewItem[this.Count];
					int num = 0;
					int num2 = 0;
					while (num2 < this.owner.Items.Count && num < array.Length)
					{
						if (this.owner.Items[num2].Checked)
						{
							array[num++] = this.owner.Items[num2];
						}
						num2++;
					}
					return array;
				}
			}

			// Token: 0x17000D7C RID: 3452
			public ListViewItem this[int index]
			{
				get
				{
					if (this.owner.VirtualMode)
					{
						throw new InvalidOperationException(SR.GetString("ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode"));
					}
					int num = this.owner.CheckedIndices[index];
					return this.owner.Items[num];
				}
			}

			// Token: 0x17000D7D RID: 3453
			object IList.this[int index]
			{
				get
				{
					if (this.owner.VirtualMode)
					{
						throw new InvalidOperationException(SR.GetString("ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode"));
					}
					return this[index];
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			// Token: 0x17000D7E RID: 3454
			public virtual ListViewItem this[string key]
			{
				get
				{
					if (this.owner.VirtualMode)
					{
						throw new InvalidOperationException(SR.GetString("ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode"));
					}
					if (string.IsNullOrEmpty(key))
					{
						return null;
					}
					int num = this.IndexOfKey(key);
					if (this.IsValidIndex(num))
					{
						return this[num];
					}
					return null;
				}
			}

			// Token: 0x17000D7F RID: 3455
			// (get) Token: 0x060044AF RID: 17583 RVA: 0x000FA0CA File Offset: 0x000F90CA
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x17000D80 RID: 3456
			// (get) Token: 0x060044B0 RID: 17584 RVA: 0x000FA0CD File Offset: 0x000F90CD
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000D81 RID: 3457
			// (get) Token: 0x060044B1 RID: 17585 RVA: 0x000FA0D0 File Offset: 0x000F90D0
			bool IList.IsFixedSize
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000D82 RID: 3458
			// (get) Token: 0x060044B2 RID: 17586 RVA: 0x000FA0D3 File Offset: 0x000F90D3
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060044B3 RID: 17587 RVA: 0x000FA0D6 File Offset: 0x000F90D6
			public bool Contains(ListViewItem item)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode"));
				}
				return item != null && item.ListView == this.owner && item.Checked;
			}

			// Token: 0x060044B4 RID: 17588 RVA: 0x000FA111 File Offset: 0x000F9111
			bool IList.Contains(object item)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode"));
				}
				return item is ListViewItem && this.Contains((ListViewItem)item);
			}

			// Token: 0x060044B5 RID: 17589 RVA: 0x000FA146 File Offset: 0x000F9146
			public virtual bool ContainsKey(string key)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode"));
				}
				return this.IsValidIndex(this.IndexOfKey(key));
			}

			// Token: 0x060044B6 RID: 17590 RVA: 0x000FA174 File Offset: 0x000F9174
			public int IndexOf(ListViewItem item)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode"));
				}
				ListViewItem[] itemArray = this.ItemArray;
				for (int i = 0; i < itemArray.Length; i++)
				{
					if (itemArray[i] == item)
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x060044B7 RID: 17591 RVA: 0x000FA1BC File Offset: 0x000F91BC
			public virtual int IndexOfKey(string key)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode"));
				}
				if (string.IsNullOrEmpty(key))
				{
					return -1;
				}
				if (this.IsValidIndex(this.lastAccessedIndex) && WindowsFormsUtils.SafeCompareStrings(this[this.lastAccessedIndex].Name, key, true))
				{
					return this.lastAccessedIndex;
				}
				for (int i = 0; i < this.Count; i++)
				{
					if (WindowsFormsUtils.SafeCompareStrings(this[i].Name, key, true))
					{
						this.lastAccessedIndex = i;
						return i;
					}
				}
				this.lastAccessedIndex = -1;
				return -1;
			}

			// Token: 0x060044B8 RID: 17592 RVA: 0x000FA256 File Offset: 0x000F9256
			private bool IsValidIndex(int index)
			{
				return index >= 0 && index < this.Count;
			}

			// Token: 0x060044B9 RID: 17593 RVA: 0x000FA267 File Offset: 0x000F9267
			int IList.IndexOf(object item)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode"));
				}
				if (item is ListViewItem)
				{
					return this.IndexOf((ListViewItem)item);
				}
				return -1;
			}

			// Token: 0x060044BA RID: 17594 RVA: 0x000FA29C File Offset: 0x000F929C
			int IList.Add(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060044BB RID: 17595 RVA: 0x000FA2A3 File Offset: 0x000F92A3
			void IList.Clear()
			{
				throw new NotSupportedException();
			}

			// Token: 0x060044BC RID: 17596 RVA: 0x000FA2AA File Offset: 0x000F92AA
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060044BD RID: 17597 RVA: 0x000FA2B1 File Offset: 0x000F92B1
			void IList.Remove(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060044BE RID: 17598 RVA: 0x000FA2B8 File Offset: 0x000F92B8
			void IList.RemoveAt(int index)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060044BF RID: 17599 RVA: 0x000FA2BF File Offset: 0x000F92BF
			public void CopyTo(Array dest, int index)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode"));
				}
				if (this.Count > 0)
				{
					Array.Copy(this.ItemArray, 0, dest, index, this.Count);
				}
			}

			// Token: 0x060044C0 RID: 17600 RVA: 0x000FA2FC File Offset: 0x000F92FC
			public IEnumerator GetEnumerator()
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode"));
				}
				ListViewItem[] itemArray = this.ItemArray;
				if (itemArray != null)
				{
					return itemArray.GetEnumerator();
				}
				return new ListViewItem[0].GetEnumerator();
			}

			// Token: 0x0400214E RID: 8526
			private ListView owner;

			// Token: 0x0400214F RID: 8527
			private int lastAccessedIndex = -1;
		}

		// Token: 0x02000484 RID: 1156
		[ListBindable(false)]
		public class SelectedIndexCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x060044C1 RID: 17601 RVA: 0x000FA342 File Offset: 0x000F9342
			public SelectedIndexCollection(ListView owner)
			{
				this.owner = owner;
			}

			// Token: 0x17000D83 RID: 3459
			// (get) Token: 0x060044C2 RID: 17602 RVA: 0x000FA354 File Offset: 0x000F9354
			[Browsable(false)]
			public int Count
			{
				get
				{
					if (this.owner.IsHandleCreated)
					{
						return (int)this.owner.SendMessage(4146, 0, 0);
					}
					if (this.owner.savedSelectedItems != null)
					{
						return this.owner.savedSelectedItems.Count;
					}
					return 0;
				}
			}

			// Token: 0x17000D84 RID: 3460
			// (get) Token: 0x060044C3 RID: 17603 RVA: 0x000FA3A8 File Offset: 0x000F93A8
			private int[] IndicesArray
			{
				get
				{
					int count = this.Count;
					int[] array = new int[count];
					if (this.owner.IsHandleCreated)
					{
						int num = -1;
						for (int i = 0; i < count; i++)
						{
							int num2 = (int)this.owner.SendMessage(4108, num, 2);
							if (num2 <= -1)
							{
								throw new InvalidOperationException(SR.GetString("SelectedNotEqualActual"));
							}
							array[i] = num2;
							num = num2;
						}
					}
					else
					{
						for (int j = 0; j < count; j++)
						{
							array[j] = this.owner.savedSelectedItems[j].Index;
						}
					}
					return array;
				}
			}

			// Token: 0x17000D85 RID: 3461
			public int this[int index]
			{
				get
				{
					if (index < 0 || index >= this.Count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (this.owner.IsHandleCreated)
					{
						int num = -1;
						for (int i = 0; i <= index; i++)
						{
							num = (int)this.owner.SendMessage(4108, num, 2);
						}
						return num;
					}
					return this.owner.savedSelectedItems[index].Index;
				}
			}

			// Token: 0x17000D86 RID: 3462
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			// Token: 0x17000D87 RID: 3463
			// (get) Token: 0x060044C7 RID: 17607 RVA: 0x000FA4F2 File Offset: 0x000F94F2
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x17000D88 RID: 3464
			// (get) Token: 0x060044C8 RID: 17608 RVA: 0x000FA4F5 File Offset: 0x000F94F5
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000D89 RID: 3465
			// (get) Token: 0x060044C9 RID: 17609 RVA: 0x000FA4F8 File Offset: 0x000F94F8
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000D8A RID: 3466
			// (get) Token: 0x060044CA RID: 17610 RVA: 0x000FA4FB File Offset: 0x000F94FB
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060044CB RID: 17611 RVA: 0x000FA4FE File Offset: 0x000F94FE
			public bool Contains(int selectedIndex)
			{
				return this.owner.Items[selectedIndex].Selected;
			}

			// Token: 0x060044CC RID: 17612 RVA: 0x000FA516 File Offset: 0x000F9516
			bool IList.Contains(object selectedIndex)
			{
				return selectedIndex is int && this.Contains((int)selectedIndex);
			}

			// Token: 0x060044CD RID: 17613 RVA: 0x000FA530 File Offset: 0x000F9530
			public int IndexOf(int selectedIndex)
			{
				int[] indicesArray = this.IndicesArray;
				for (int i = 0; i < indicesArray.Length; i++)
				{
					if (indicesArray[i] == selectedIndex)
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x060044CE RID: 17614 RVA: 0x000FA55B File Offset: 0x000F955B
			int IList.IndexOf(object selectedIndex)
			{
				if (selectedIndex is int)
				{
					return this.IndexOf((int)selectedIndex);
				}
				return -1;
			}

			// Token: 0x060044CF RID: 17615 RVA: 0x000FA574 File Offset: 0x000F9574
			int IList.Add(object value)
			{
				if (value is int)
				{
					return this.Add((int)value);
				}
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"value",
					value.ToString()
				}));
			}

			// Token: 0x060044D0 RID: 17616 RVA: 0x000FA5BE File Offset: 0x000F95BE
			void IList.Clear()
			{
				this.Clear();
			}

			// Token: 0x060044D1 RID: 17617 RVA: 0x000FA5C6 File Offset: 0x000F95C6
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060044D2 RID: 17618 RVA: 0x000FA5D0 File Offset: 0x000F95D0
			void IList.Remove(object value)
			{
				if (value is int)
				{
					this.Remove((int)value);
					return;
				}
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"value",
					value.ToString()
				}));
			}

			// Token: 0x060044D3 RID: 17619 RVA: 0x000FA61A File Offset: 0x000F961A
			void IList.RemoveAt(int index)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060044D4 RID: 17620 RVA: 0x000FA624 File Offset: 0x000F9624
			public int Add(int itemIndex)
			{
				if (this.owner.VirtualMode)
				{
					if (itemIndex < 0 || itemIndex >= this.owner.VirtualListSize)
					{
						throw new ArgumentOutOfRangeException("itemIndex", SR.GetString("InvalidArgument", new object[]
						{
							"itemIndex",
							itemIndex.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (this.owner.IsHandleCreated)
					{
						this.owner.SetItemState(itemIndex, 2, 2);
						return this.Count;
					}
					return -1;
				}
				else
				{
					if (itemIndex < 0 || itemIndex >= this.owner.Items.Count)
					{
						throw new ArgumentOutOfRangeException("itemIndex", SR.GetString("InvalidArgument", new object[]
						{
							"itemIndex",
							itemIndex.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.owner.Items[itemIndex].Selected = true;
					return this.Count;
				}
			}

			// Token: 0x060044D5 RID: 17621 RVA: 0x000FA710 File Offset: 0x000F9710
			public void Clear()
			{
				if (!this.owner.VirtualMode)
				{
					this.owner.savedSelectedItems = null;
				}
				if (this.owner.IsHandleCreated)
				{
					this.owner.SetItemState(-1, 0, 2);
				}
			}

			// Token: 0x060044D6 RID: 17622 RVA: 0x000FA746 File Offset: 0x000F9746
			public void CopyTo(Array dest, int index)
			{
				if (this.Count > 0)
				{
					Array.Copy(this.IndicesArray, 0, dest, index, this.Count);
				}
			}

			// Token: 0x060044D7 RID: 17623 RVA: 0x000FA768 File Offset: 0x000F9768
			public IEnumerator GetEnumerator()
			{
				int[] indicesArray = this.IndicesArray;
				if (indicesArray != null)
				{
					return indicesArray.GetEnumerator();
				}
				return new int[0].GetEnumerator();
			}

			// Token: 0x060044D8 RID: 17624 RVA: 0x000FA794 File Offset: 0x000F9794
			public void Remove(int itemIndex)
			{
				if (this.owner.VirtualMode)
				{
					if (itemIndex < 0 || itemIndex >= this.owner.VirtualListSize)
					{
						throw new ArgumentOutOfRangeException("itemIndex", SR.GetString("InvalidArgument", new object[]
						{
							"itemIndex",
							itemIndex.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (this.owner.IsHandleCreated)
					{
						this.owner.SetItemState(itemIndex, 0, 2);
						return;
					}
				}
				else
				{
					if (itemIndex < 0 || itemIndex >= this.owner.Items.Count)
					{
						throw new ArgumentOutOfRangeException("itemIndex", SR.GetString("InvalidArgument", new object[]
						{
							"itemIndex",
							itemIndex.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.owner.Items[itemIndex].Selected = false;
				}
			}

			// Token: 0x04002150 RID: 8528
			private ListView owner;
		}

		// Token: 0x02000485 RID: 1157
		[ListBindable(false)]
		public class SelectedListViewItemCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x060044D9 RID: 17625 RVA: 0x000FA872 File Offset: 0x000F9872
			public SelectedListViewItemCollection(ListView owner)
			{
				this.owner = owner;
			}

			// Token: 0x17000D8B RID: 3467
			// (get) Token: 0x060044DA RID: 17626 RVA: 0x000FA888 File Offset: 0x000F9888
			private ListViewItem[] SelectedItemArray
			{
				get
				{
					if (this.owner.IsHandleCreated)
					{
						int num = (int)this.owner.SendMessage(4146, 0, 0);
						ListViewItem[] array = new ListViewItem[num];
						int num2 = -1;
						for (int i = 0; i < num; i++)
						{
							int num3 = (int)this.owner.SendMessage(4108, num2, 2);
							if (num3 <= -1)
							{
								throw new InvalidOperationException(SR.GetString("SelectedNotEqualActual"));
							}
							array[i] = this.owner.Items[num3];
							num2 = num3;
						}
						return array;
					}
					if (this.owner.savedSelectedItems != null)
					{
						ListViewItem[] array2 = new ListViewItem[this.owner.savedSelectedItems.Count];
						for (int j = 0; j < this.owner.savedSelectedItems.Count; j++)
						{
							array2[j] = this.owner.savedSelectedItems[j];
						}
						return array2;
					}
					return new ListViewItem[0];
				}
			}

			// Token: 0x17000D8C RID: 3468
			// (get) Token: 0x060044DB RID: 17627 RVA: 0x000FA97C File Offset: 0x000F997C
			[Browsable(false)]
			public int Count
			{
				get
				{
					if (this.owner.VirtualMode)
					{
						throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
					}
					if (this.owner.IsHandleCreated)
					{
						return (int)this.owner.SendMessage(4146, 0, 0);
					}
					if (this.owner.savedSelectedItems != null)
					{
						return this.owner.savedSelectedItems.Count;
					}
					return 0;
				}
			}

			// Token: 0x17000D8D RID: 3469
			public ListViewItem this[int index]
			{
				get
				{
					if (this.owner.VirtualMode)
					{
						throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
					}
					if (index < 0 || index >= this.Count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (this.owner.IsHandleCreated)
					{
						int num = -1;
						for (int i = 0; i <= index; i++)
						{
							num = (int)this.owner.SendMessage(4108, num, 2);
						}
						return this.owner.Items[num];
					}
					return this.owner.savedSelectedItems[index];
				}
			}

			// Token: 0x17000D8E RID: 3470
			public virtual ListViewItem this[string key]
			{
				get
				{
					if (this.owner.VirtualMode)
					{
						throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
					}
					if (string.IsNullOrEmpty(key))
					{
						return null;
					}
					int num = this.IndexOfKey(key);
					if (this.IsValidIndex(num))
					{
						return this[num];
					}
					return null;
				}
			}

			// Token: 0x17000D8F RID: 3471
			object IList.this[int index]
			{
				get
				{
					if (this.owner.VirtualMode)
					{
						throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
					}
					return this[index];
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			// Token: 0x17000D90 RID: 3472
			// (get) Token: 0x060044E0 RID: 17632 RVA: 0x000FAB2B File Offset: 0x000F9B2B
			bool IList.IsFixedSize
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000D91 RID: 3473
			// (get) Token: 0x060044E1 RID: 17633 RVA: 0x000FAB2E File Offset: 0x000F9B2E
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000D92 RID: 3474
			// (get) Token: 0x060044E2 RID: 17634 RVA: 0x000FAB31 File Offset: 0x000F9B31
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x17000D93 RID: 3475
			// (get) Token: 0x060044E3 RID: 17635 RVA: 0x000FAB34 File Offset: 0x000F9B34
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060044E4 RID: 17636 RVA: 0x000FAB37 File Offset: 0x000F9B37
			int IList.Add(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060044E5 RID: 17637 RVA: 0x000FAB3E File Offset: 0x000F9B3E
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060044E6 RID: 17638 RVA: 0x000FAB45 File Offset: 0x000F9B45
			private bool IsValidIndex(int index)
			{
				return index >= 0 && index < this.Count;
			}

			// Token: 0x060044E7 RID: 17639 RVA: 0x000FAB56 File Offset: 0x000F9B56
			void IList.Remove(object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060044E8 RID: 17640 RVA: 0x000FAB5D File Offset: 0x000F9B5D
			void IList.RemoveAt(int index)
			{
				throw new NotSupportedException();
			}

			// Token: 0x060044E9 RID: 17641 RVA: 0x000FAB64 File Offset: 0x000F9B64
			public void Clear()
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
				}
				ListViewItem[] selectedItemArray = this.SelectedItemArray;
				for (int i = 0; i < selectedItemArray.Length; i++)
				{
					selectedItemArray[i].Selected = false;
				}
			}

			// Token: 0x060044EA RID: 17642 RVA: 0x000FABAC File Offset: 0x000F9BAC
			public virtual bool ContainsKey(string key)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
				}
				return this.IsValidIndex(this.IndexOfKey(key));
			}

			// Token: 0x060044EB RID: 17643 RVA: 0x000FABD8 File Offset: 0x000F9BD8
			public bool Contains(ListViewItem item)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
				}
				return this.IndexOf(item) != -1;
			}

			// Token: 0x060044EC RID: 17644 RVA: 0x000FAC04 File Offset: 0x000F9C04
			bool IList.Contains(object item)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
				}
				return item is ListViewItem && this.Contains((ListViewItem)item);
			}

			// Token: 0x060044ED RID: 17645 RVA: 0x000FAC39 File Offset: 0x000F9C39
			public void CopyTo(Array dest, int index)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
				}
				if (this.Count > 0)
				{
					Array.Copy(this.SelectedItemArray, 0, dest, index, this.Count);
				}
			}

			// Token: 0x060044EE RID: 17646 RVA: 0x000FAC78 File Offset: 0x000F9C78
			public IEnumerator GetEnumerator()
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
				}
				ListViewItem[] selectedItemArray = this.SelectedItemArray;
				if (selectedItemArray != null)
				{
					return selectedItemArray.GetEnumerator();
				}
				return new ListViewItem[0].GetEnumerator();
			}

			// Token: 0x060044EF RID: 17647 RVA: 0x000FACC0 File Offset: 0x000F9CC0
			public int IndexOf(ListViewItem item)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
				}
				ListViewItem[] selectedItemArray = this.SelectedItemArray;
				for (int i = 0; i < selectedItemArray.Length; i++)
				{
					if (selectedItemArray[i] == item)
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x060044F0 RID: 17648 RVA: 0x000FAD08 File Offset: 0x000F9D08
			int IList.IndexOf(object item)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
				}
				if (item is ListViewItem)
				{
					return this.IndexOf((ListViewItem)item);
				}
				return -1;
			}

			// Token: 0x060044F1 RID: 17649 RVA: 0x000FAD40 File Offset: 0x000F9D40
			public virtual int IndexOfKey(string key)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode"));
				}
				if (string.IsNullOrEmpty(key))
				{
					return -1;
				}
				if (this.IsValidIndex(this.lastAccessedIndex) && WindowsFormsUtils.SafeCompareStrings(this[this.lastAccessedIndex].Name, key, true))
				{
					return this.lastAccessedIndex;
				}
				for (int i = 0; i < this.Count; i++)
				{
					if (WindowsFormsUtils.SafeCompareStrings(this[i].Name, key, true))
					{
						this.lastAccessedIndex = i;
						return i;
					}
				}
				this.lastAccessedIndex = -1;
				return -1;
			}

			// Token: 0x04002151 RID: 8529
			private ListView owner;

			// Token: 0x04002152 RID: 8530
			private int lastAccessedIndex = -1;
		}

		// Token: 0x02000486 RID: 1158
		[ListBindable(false)]
		public class ColumnHeaderCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x060044F2 RID: 17650 RVA: 0x000FADDA File Offset: 0x000F9DDA
			public ColumnHeaderCollection(ListView owner)
			{
				this.owner = owner;
			}

			// Token: 0x17000D94 RID: 3476
			public virtual ColumnHeader this[int index]
			{
				get
				{
					if (this.owner.columnHeaders == null || index < 0 || index >= this.owner.columnHeaders.Length)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					return this.owner.columnHeaders[index];
				}
			}

			// Token: 0x17000D95 RID: 3477
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			// Token: 0x17000D96 RID: 3478
			public virtual ColumnHeader this[string key]
			{
				get
				{
					if (string.IsNullOrEmpty(key))
					{
						return null;
					}
					int num = this.IndexOfKey(key);
					if (this.IsValidIndex(num))
					{
						return this[num];
					}
					return null;
				}
			}

			// Token: 0x17000D97 RID: 3479
			// (get) Token: 0x060044F7 RID: 17655 RVA: 0x000FAEA1 File Offset: 0x000F9EA1
			[Browsable(false)]
			public int Count
			{
				get
				{
					if (this.owner.columnHeaders != null)
					{
						return this.owner.columnHeaders.Length;
					}
					return 0;
				}
			}

			// Token: 0x17000D98 RID: 3480
			// (get) Token: 0x060044F8 RID: 17656 RVA: 0x000FAEBF File Offset: 0x000F9EBF
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x17000D99 RID: 3481
			// (get) Token: 0x060044F9 RID: 17657 RVA: 0x000FAEC2 File Offset: 0x000F9EC2
			bool ICollection.IsSynchronized
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000D9A RID: 3482
			// (get) Token: 0x060044FA RID: 17658 RVA: 0x000FAEC5 File Offset: 0x000F9EC5
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000D9B RID: 3483
			// (get) Token: 0x060044FB RID: 17659 RVA: 0x000FAEC8 File Offset: 0x000F9EC8
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060044FC RID: 17660 RVA: 0x000FAECC File Offset: 0x000F9ECC
			public virtual void RemoveByKey(string key)
			{
				int num = this.IndexOfKey(key);
				if (this.IsValidIndex(num))
				{
					this.RemoveAt(num);
				}
			}

			// Token: 0x060044FD RID: 17661 RVA: 0x000FAEF4 File Offset: 0x000F9EF4
			public virtual int IndexOfKey(string key)
			{
				if (string.IsNullOrEmpty(key))
				{
					return -1;
				}
				if (this.IsValidIndex(this.lastAccessedIndex) && WindowsFormsUtils.SafeCompareStrings(this[this.lastAccessedIndex].Name, key, true))
				{
					return this.lastAccessedIndex;
				}
				for (int i = 0; i < this.Count; i++)
				{
					if (WindowsFormsUtils.SafeCompareStrings(this[i].Name, key, true))
					{
						this.lastAccessedIndex = i;
						return i;
					}
				}
				this.lastAccessedIndex = -1;
				return -1;
			}

			// Token: 0x060044FE RID: 17662 RVA: 0x000FAF71 File Offset: 0x000F9F71
			private bool IsValidIndex(int index)
			{
				return index >= 0 && index < this.Count;
			}

			// Token: 0x060044FF RID: 17663 RVA: 0x000FAF84 File Offset: 0x000F9F84
			public virtual ColumnHeader Add(string text, int width, HorizontalAlignment textAlign)
			{
				ColumnHeader columnHeader = new ColumnHeader();
				columnHeader.Text = text;
				columnHeader.Width = width;
				columnHeader.TextAlign = textAlign;
				return this.owner.InsertColumn(this.Count, columnHeader);
			}

			// Token: 0x06004500 RID: 17664 RVA: 0x000FAFC0 File Offset: 0x000F9FC0
			public virtual int Add(ColumnHeader value)
			{
				int count = this.Count;
				this.owner.InsertColumn(count, value);
				return count;
			}

			// Token: 0x06004501 RID: 17665 RVA: 0x000FAFE4 File Offset: 0x000F9FE4
			public virtual ColumnHeader Add(string text)
			{
				ColumnHeader columnHeader = new ColumnHeader();
				columnHeader.Text = text;
				return this.owner.InsertColumn(this.Count, columnHeader);
			}

			// Token: 0x06004502 RID: 17666 RVA: 0x000FB010 File Offset: 0x000FA010
			public virtual ColumnHeader Add(string text, int width)
			{
				ColumnHeader columnHeader = new ColumnHeader();
				columnHeader.Text = text;
				columnHeader.Width = width;
				return this.owner.InsertColumn(this.Count, columnHeader);
			}

			// Token: 0x06004503 RID: 17667 RVA: 0x000FB044 File Offset: 0x000FA044
			public virtual ColumnHeader Add(string key, string text)
			{
				ColumnHeader columnHeader = new ColumnHeader();
				columnHeader.Name = key;
				columnHeader.Text = text;
				return this.owner.InsertColumn(this.Count, columnHeader);
			}

			// Token: 0x06004504 RID: 17668 RVA: 0x000FB078 File Offset: 0x000FA078
			public virtual ColumnHeader Add(string key, string text, int width)
			{
				ColumnHeader columnHeader = new ColumnHeader();
				columnHeader.Name = key;
				columnHeader.Text = text;
				columnHeader.Width = width;
				return this.owner.InsertColumn(this.Count, columnHeader);
			}

			// Token: 0x06004505 RID: 17669 RVA: 0x000FB0B4 File Offset: 0x000FA0B4
			public virtual ColumnHeader Add(string key, string text, int width, HorizontalAlignment textAlign, string imageKey)
			{
				ColumnHeader columnHeader = new ColumnHeader(imageKey);
				columnHeader.Name = key;
				columnHeader.Text = text;
				columnHeader.Width = width;
				columnHeader.TextAlign = textAlign;
				return this.owner.InsertColumn(this.Count, columnHeader);
			}

			// Token: 0x06004506 RID: 17670 RVA: 0x000FB0F8 File Offset: 0x000FA0F8
			public virtual ColumnHeader Add(string key, string text, int width, HorizontalAlignment textAlign, int imageIndex)
			{
				ColumnHeader columnHeader = new ColumnHeader(imageIndex);
				columnHeader.Name = key;
				columnHeader.Text = text;
				columnHeader.Width = width;
				columnHeader.TextAlign = textAlign;
				return this.owner.InsertColumn(this.Count, columnHeader);
			}

			// Token: 0x06004507 RID: 17671 RVA: 0x000FB13C File Offset: 0x000FA13C
			public virtual void AddRange(ColumnHeader[] values)
			{
				if (values == null)
				{
					throw new ArgumentNullException("values");
				}
				Hashtable hashtable = new Hashtable();
				int[] array = new int[values.Length];
				for (int i = 0; i < values.Length; i++)
				{
					if (values[i].DisplayIndex == -1)
					{
						values[i].DisplayIndexInternal = i;
					}
					if (!hashtable.ContainsKey(values[i].DisplayIndex) && values[i].DisplayIndex >= 0 && values[i].DisplayIndex < values.Length)
					{
						hashtable.Add(values[i].DisplayIndex, i);
					}
					array[i] = values[i].DisplayIndex;
					this.Add(values[i]);
				}
				if (hashtable.Count == values.Length)
				{
					this.owner.SetDisplayIndices(array);
				}
			}

			// Token: 0x06004508 RID: 17672 RVA: 0x000FB1FA File Offset: 0x000FA1FA
			int IList.Add(object value)
			{
				if (value is ColumnHeader)
				{
					return this.Add((ColumnHeader)value);
				}
				throw new ArgumentException(SR.GetString("ColumnHeaderCollectionInvalidArgument"));
			}

			// Token: 0x06004509 RID: 17673 RVA: 0x000FB220 File Offset: 0x000FA220
			public virtual void Clear()
			{
				if (this.owner.columnHeaders != null)
				{
					if (this.owner.View == View.Tile)
					{
						for (int i = this.owner.columnHeaders.Length - 1; i >= 0; i--)
						{
							int width = this.owner.columnHeaders[i].Width;
							this.owner.columnHeaders[i].OwnerListview = null;
						}
						this.owner.columnHeaders = null;
						if (this.owner.IsHandleCreated)
						{
							this.owner.RecreateHandleInternal();
							return;
						}
					}
					else
					{
						for (int j = this.owner.columnHeaders.Length - 1; j >= 0; j--)
						{
							int width2 = this.owner.columnHeaders[j].Width;
							if (this.owner.IsHandleCreated)
							{
								this.owner.SendMessage(4124, j, 0);
							}
							this.owner.columnHeaders[j].OwnerListview = null;
						}
						this.owner.columnHeaders = null;
					}
				}
			}

			// Token: 0x0600450A RID: 17674 RVA: 0x000FB31C File Offset: 0x000FA31C
			public bool Contains(ColumnHeader value)
			{
				return this.IndexOf(value) != -1;
			}

			// Token: 0x0600450B RID: 17675 RVA: 0x000FB32B File Offset: 0x000FA32B
			bool IList.Contains(object value)
			{
				return value is ColumnHeader && this.Contains((ColumnHeader)value);
			}

			// Token: 0x0600450C RID: 17676 RVA: 0x000FB343 File Offset: 0x000FA343
			public virtual bool ContainsKey(string key)
			{
				return this.IsValidIndex(this.IndexOfKey(key));
			}

			// Token: 0x0600450D RID: 17677 RVA: 0x000FB352 File Offset: 0x000FA352
			void ICollection.CopyTo(Array dest, int index)
			{
				if (this.Count > 0)
				{
					Array.Copy(this.owner.columnHeaders, 0, dest, index, this.Count);
				}
			}

			// Token: 0x0600450E RID: 17678 RVA: 0x000FB378 File Offset: 0x000FA378
			public int IndexOf(ColumnHeader value)
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this[i] == value)
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x0600450F RID: 17679 RVA: 0x000FB3A3 File Offset: 0x000FA3A3
			int IList.IndexOf(object value)
			{
				if (value is ColumnHeader)
				{
					return this.IndexOf((ColumnHeader)value);
				}
				return -1;
			}

			// Token: 0x06004510 RID: 17680 RVA: 0x000FB3BC File Offset: 0x000FA3BC
			public void Insert(int index, ColumnHeader value)
			{
				if (index < 0 || index > this.Count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.owner.InsertColumn(index, value);
			}

			// Token: 0x06004511 RID: 17681 RVA: 0x000FB418 File Offset: 0x000FA418
			void IList.Insert(int index, object value)
			{
				if (value is ColumnHeader)
				{
					this.Insert(index, (ColumnHeader)value);
				}
			}

			// Token: 0x06004512 RID: 17682 RVA: 0x000FB430 File Offset: 0x000FA430
			public void Insert(int index, string text, int width, HorizontalAlignment textAlign)
			{
				this.Insert(index, new ColumnHeader
				{
					Text = text,
					Width = width,
					TextAlign = textAlign
				});
			}

			// Token: 0x06004513 RID: 17683 RVA: 0x000FB464 File Offset: 0x000FA464
			public void Insert(int index, string text)
			{
				this.Insert(index, new ColumnHeader
				{
					Text = text
				});
			}

			// Token: 0x06004514 RID: 17684 RVA: 0x000FB488 File Offset: 0x000FA488
			public void Insert(int index, string text, int width)
			{
				this.Insert(index, new ColumnHeader
				{
					Text = text,
					Width = width
				});
			}

			// Token: 0x06004515 RID: 17685 RVA: 0x000FB4B4 File Offset: 0x000FA4B4
			public void Insert(int index, string key, string text)
			{
				this.Insert(index, new ColumnHeader
				{
					Name = key,
					Text = text
				});
			}

			// Token: 0x06004516 RID: 17686 RVA: 0x000FB4E0 File Offset: 0x000FA4E0
			public void Insert(int index, string key, string text, int width)
			{
				this.Insert(index, new ColumnHeader
				{
					Name = key,
					Text = text,
					Width = width
				});
			}

			// Token: 0x06004517 RID: 17687 RVA: 0x000FB514 File Offset: 0x000FA514
			public void Insert(int index, string key, string text, int width, HorizontalAlignment textAlign, string imageKey)
			{
				this.Insert(index, new ColumnHeader(imageKey)
				{
					Name = key,
					Text = text,
					Width = width,
					TextAlign = textAlign
				});
			}

			// Token: 0x06004518 RID: 17688 RVA: 0x000FB550 File Offset: 0x000FA550
			public void Insert(int index, string key, string text, int width, HorizontalAlignment textAlign, int imageIndex)
			{
				this.Insert(index, new ColumnHeader(imageIndex)
				{
					Name = key,
					Text = text,
					Width = width,
					TextAlign = textAlign
				});
			}

			// Token: 0x06004519 RID: 17689 RVA: 0x000FB58C File Offset: 0x000FA58C
			public virtual void RemoveAt(int index)
			{
				if (index < 0 || index >= this.owner.columnHeaders.Length)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				int width = this.owner.columnHeaders[index].Width;
				if (this.owner.IsHandleCreated && this.owner.View != View.Tile && (int)this.owner.SendMessage(4124, index, 0) == 0)
				{
					throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				int[] array = new int[this.Count - 1];
				ColumnHeader columnHeader = this[index];
				for (int i = 0; i < this.Count; i++)
				{
					ColumnHeader columnHeader2 = this[i];
					if (i != index)
					{
						if (columnHeader2.DisplayIndex >= columnHeader.DisplayIndex)
						{
							columnHeader2.DisplayIndexInternal--;
						}
						array[(i > index) ? (i - 1) : i] = columnHeader2.DisplayIndexInternal;
					}
				}
				columnHeader.DisplayIndexInternal = -1;
				this.owner.columnHeaders[index].OwnerListview = null;
				int num = this.owner.columnHeaders.Length;
				if (num == 1)
				{
					this.owner.columnHeaders = null;
				}
				else
				{
					ColumnHeader[] array2 = new ColumnHeader[--num];
					if (index > 0)
					{
						Array.Copy(this.owner.columnHeaders, 0, array2, 0, index);
					}
					if (index < num)
					{
						Array.Copy(this.owner.columnHeaders, index + 1, array2, index, num - index);
					}
					this.owner.columnHeaders = array2;
				}
				if (this.owner.IsHandleCreated && this.owner.View == View.Tile)
				{
					this.owner.RecreateHandleInternal();
				}
				this.owner.SetDisplayIndices(array);
			}

			// Token: 0x0600451A RID: 17690 RVA: 0x000FB788 File Offset: 0x000FA788
			public virtual void Remove(ColumnHeader column)
			{
				int num = this.IndexOf(column);
				if (num != -1)
				{
					this.RemoveAt(num);
				}
			}

			// Token: 0x0600451B RID: 17691 RVA: 0x000FB7A8 File Offset: 0x000FA7A8
			void IList.Remove(object value)
			{
				if (value is ColumnHeader)
				{
					this.Remove((ColumnHeader)value);
				}
			}

			// Token: 0x0600451C RID: 17692 RVA: 0x000FB7BE File Offset: 0x000FA7BE
			public IEnumerator GetEnumerator()
			{
				if (this.owner.columnHeaders != null)
				{
					return this.owner.columnHeaders.GetEnumerator();
				}
				return new ColumnHeader[0].GetEnumerator();
			}

			// Token: 0x04002153 RID: 8531
			private ListView owner;

			// Token: 0x04002154 RID: 8532
			private int lastAccessedIndex = -1;
		}

		// Token: 0x02000487 RID: 1159
		[ListBindable(false)]
		public class ListViewItemCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x0600451D RID: 17693 RVA: 0x000FB7E9 File Offset: 0x000FA7E9
			public ListViewItemCollection(ListView owner)
			{
				this.innerList = new ListView.ListViewNativeItemCollection(owner);
			}

			// Token: 0x0600451E RID: 17694 RVA: 0x000FB804 File Offset: 0x000FA804
			internal ListViewItemCollection(ListView.ListViewItemCollection.IInnerList innerList)
			{
				this.innerList = innerList;
			}

			// Token: 0x17000D9C RID: 3484
			// (get) Token: 0x0600451F RID: 17695 RVA: 0x000FB81A File Offset: 0x000FA81A
			private ListView.ListViewItemCollection.IInnerList InnerList
			{
				get
				{
					return this.innerList;
				}
			}

			// Token: 0x17000D9D RID: 3485
			// (get) Token: 0x06004520 RID: 17696 RVA: 0x000FB822 File Offset: 0x000FA822
			[Browsable(false)]
			public int Count
			{
				get
				{
					return this.InnerList.Count;
				}
			}

			// Token: 0x17000D9E RID: 3486
			// (get) Token: 0x06004521 RID: 17697 RVA: 0x000FB82F File Offset: 0x000FA82F
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x17000D9F RID: 3487
			// (get) Token: 0x06004522 RID: 17698 RVA: 0x000FB832 File Offset: 0x000FA832
			bool ICollection.IsSynchronized
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000DA0 RID: 3488
			// (get) Token: 0x06004523 RID: 17699 RVA: 0x000FB835 File Offset: 0x000FA835
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000DA1 RID: 3489
			// (get) Token: 0x06004524 RID: 17700 RVA: 0x000FB838 File Offset: 0x000FA838
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000DA2 RID: 3490
			public virtual ListViewItem this[int index]
			{
				get
				{
					if (index < 0 || index >= this.InnerList.Count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					return this.InnerList[index];
				}
				set
				{
					if (index < 0 || index >= this.InnerList.Count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.InnerList[index] = value;
				}
			}

			// Token: 0x17000DA3 RID: 3491
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					if (value is ListViewItem)
					{
						this[index] = (ListViewItem)value;
						return;
					}
					if (value != null)
					{
						this[index] = new ListViewItem(value.ToString(), -1);
					}
				}
			}

			// Token: 0x17000DA4 RID: 3492
			public virtual ListViewItem this[string key]
			{
				get
				{
					if (string.IsNullOrEmpty(key))
					{
						return null;
					}
					int num = this.IndexOfKey(key);
					if (this.IsValidIndex(num))
					{
						return this[num];
					}
					return null;
				}
			}

			// Token: 0x0600452A RID: 17706 RVA: 0x000FB965 File Offset: 0x000FA965
			public virtual ListViewItem Add(string text)
			{
				return this.Add(text, -1);
			}

			// Token: 0x0600452B RID: 17707 RVA: 0x000FB96F File Offset: 0x000FA96F
			int IList.Add(object item)
			{
				if (item is ListViewItem)
				{
					return this.IndexOf(this.Add((ListViewItem)item));
				}
				if (item != null)
				{
					return this.IndexOf(this.Add(item.ToString()));
				}
				return -1;
			}

			// Token: 0x0600452C RID: 17708 RVA: 0x000FB9A4 File Offset: 0x000FA9A4
			public virtual ListViewItem Add(string text, int imageIndex)
			{
				ListViewItem listViewItem = new ListViewItem(text, imageIndex);
				this.Add(listViewItem);
				return listViewItem;
			}

			// Token: 0x0600452D RID: 17709 RVA: 0x000FB9C2 File Offset: 0x000FA9C2
			public virtual ListViewItem Add(ListViewItem value)
			{
				this.InnerList.Add(value);
				return value;
			}

			// Token: 0x0600452E RID: 17710 RVA: 0x000FB9D4 File Offset: 0x000FA9D4
			public virtual ListViewItem Add(string text, string imageKey)
			{
				ListViewItem listViewItem = new ListViewItem(text, imageKey);
				this.Add(listViewItem);
				return listViewItem;
			}

			// Token: 0x0600452F RID: 17711 RVA: 0x000FB9F4 File Offset: 0x000FA9F4
			public virtual ListViewItem Add(string key, string text, string imageKey)
			{
				ListViewItem listViewItem = new ListViewItem(text, imageKey);
				listViewItem.Name = key;
				this.Add(listViewItem);
				return listViewItem;
			}

			// Token: 0x06004530 RID: 17712 RVA: 0x000FBA1C File Offset: 0x000FAA1C
			public virtual ListViewItem Add(string key, string text, int imageIndex)
			{
				ListViewItem listViewItem = new ListViewItem(text, imageIndex);
				listViewItem.Name = key;
				this.Add(listViewItem);
				return listViewItem;
			}

			// Token: 0x06004531 RID: 17713 RVA: 0x000FBA41 File Offset: 0x000FAA41
			public void AddRange(ListViewItem[] items)
			{
				if (items == null)
				{
					throw new ArgumentNullException("items");
				}
				this.InnerList.AddRange(items);
			}

			// Token: 0x06004532 RID: 17714 RVA: 0x000FBA60 File Offset: 0x000FAA60
			public void AddRange(ListView.ListViewItemCollection items)
			{
				if (items == null)
				{
					throw new ArgumentNullException("items");
				}
				ListViewItem[] array = new ListViewItem[items.Count];
				items.CopyTo(array, 0);
				this.InnerList.AddRange(array);
			}

			// Token: 0x06004533 RID: 17715 RVA: 0x000FBA9B File Offset: 0x000FAA9B
			public virtual void Clear()
			{
				this.InnerList.Clear();
			}

			// Token: 0x06004534 RID: 17716 RVA: 0x000FBAA8 File Offset: 0x000FAAA8
			public bool Contains(ListViewItem item)
			{
				return this.InnerList.Contains(item);
			}

			// Token: 0x06004535 RID: 17717 RVA: 0x000FBAB6 File Offset: 0x000FAAB6
			bool IList.Contains(object item)
			{
				return item is ListViewItem && this.Contains((ListViewItem)item);
			}

			// Token: 0x06004536 RID: 17718 RVA: 0x000FBACE File Offset: 0x000FAACE
			public virtual bool ContainsKey(string key)
			{
				return this.IsValidIndex(this.IndexOfKey(key));
			}

			// Token: 0x06004537 RID: 17719 RVA: 0x000FBADD File Offset: 0x000FAADD
			public void CopyTo(Array dest, int index)
			{
				this.InnerList.CopyTo(dest, index);
			}

			// Token: 0x06004538 RID: 17720 RVA: 0x000FBAEC File Offset: 0x000FAAEC
			public ListViewItem[] Find(string key, bool searchAllSubItems)
			{
				ArrayList arrayList = this.FindInternal(key, searchAllSubItems, this, new ArrayList());
				ListViewItem[] array = new ListViewItem[arrayList.Count];
				arrayList.CopyTo(array, 0);
				return array;
			}

			// Token: 0x06004539 RID: 17721 RVA: 0x000FBB20 File Offset: 0x000FAB20
			private ArrayList FindInternal(string key, bool searchAllSubItems, ListView.ListViewItemCollection listViewItems, ArrayList foundItems)
			{
				if (listViewItems == null || foundItems == null)
				{
					return null;
				}
				for (int i = 0; i < listViewItems.Count; i++)
				{
					if (WindowsFormsUtils.SafeCompareStrings(listViewItems[i].Name, key, true))
					{
						foundItems.Add(listViewItems[i]);
					}
					else if (searchAllSubItems)
					{
						for (int j = 1; j < listViewItems[i].SubItems.Count; j++)
						{
							if (WindowsFormsUtils.SafeCompareStrings(listViewItems[i].SubItems[j].Name, key, true))
							{
								foundItems.Add(listViewItems[i]);
								break;
							}
						}
					}
				}
				return foundItems;
			}

			// Token: 0x0600453A RID: 17722 RVA: 0x000FBBC2 File Offset: 0x000FABC2
			public IEnumerator GetEnumerator()
			{
				if (this.InnerList.OwnerIsVirtualListView && !this.InnerList.OwnerIsDesignMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantGetEnumeratorInVirtualMode"));
				}
				return this.InnerList.GetEnumerator();
			}

			// Token: 0x0600453B RID: 17723 RVA: 0x000FBBFC File Offset: 0x000FABFC
			public int IndexOf(ListViewItem item)
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this[i] == item)
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x0600453C RID: 17724 RVA: 0x000FBC27 File Offset: 0x000FAC27
			int IList.IndexOf(object item)
			{
				if (item is ListViewItem)
				{
					return this.IndexOf((ListViewItem)item);
				}
				return -1;
			}

			// Token: 0x0600453D RID: 17725 RVA: 0x000FBC40 File Offset: 0x000FAC40
			public virtual int IndexOfKey(string key)
			{
				if (string.IsNullOrEmpty(key))
				{
					return -1;
				}
				if (this.IsValidIndex(this.lastAccessedIndex) && WindowsFormsUtils.SafeCompareStrings(this[this.lastAccessedIndex].Name, key, true))
				{
					return this.lastAccessedIndex;
				}
				for (int i = 0; i < this.Count; i++)
				{
					if (WindowsFormsUtils.SafeCompareStrings(this[i].Name, key, true))
					{
						this.lastAccessedIndex = i;
						return i;
					}
				}
				this.lastAccessedIndex = -1;
				return -1;
			}

			// Token: 0x0600453E RID: 17726 RVA: 0x000FBCBD File Offset: 0x000FACBD
			private bool IsValidIndex(int index)
			{
				return index >= 0 && index < this.Count;
			}

			// Token: 0x0600453F RID: 17727 RVA: 0x000FBCD0 File Offset: 0x000FACD0
			public ListViewItem Insert(int index, ListViewItem item)
			{
				if (index < 0 || index > this.Count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.InnerList.Insert(index, item);
				return item;
			}

			// Token: 0x06004540 RID: 17728 RVA: 0x000FBD2D File Offset: 0x000FAD2D
			public ListViewItem Insert(int index, string text)
			{
				return this.Insert(index, new ListViewItem(text));
			}

			// Token: 0x06004541 RID: 17729 RVA: 0x000FBD3C File Offset: 0x000FAD3C
			public ListViewItem Insert(int index, string text, int imageIndex)
			{
				return this.Insert(index, new ListViewItem(text, imageIndex));
			}

			// Token: 0x06004542 RID: 17730 RVA: 0x000FBD4C File Offset: 0x000FAD4C
			void IList.Insert(int index, object item)
			{
				if (item is ListViewItem)
				{
					this.Insert(index, (ListViewItem)item);
					return;
				}
				if (item != null)
				{
					this.Insert(index, item.ToString());
				}
			}

			// Token: 0x06004543 RID: 17731 RVA: 0x000FBD76 File Offset: 0x000FAD76
			public ListViewItem Insert(int index, string text, string imageKey)
			{
				return this.Insert(index, new ListViewItem(text, imageKey));
			}

			// Token: 0x06004544 RID: 17732 RVA: 0x000FBD88 File Offset: 0x000FAD88
			public virtual ListViewItem Insert(int index, string key, string text, string imageKey)
			{
				return this.Insert(index, new ListViewItem(text, imageKey)
				{
					Name = key
				});
			}

			// Token: 0x06004545 RID: 17733 RVA: 0x000FBDB0 File Offset: 0x000FADB0
			public virtual ListViewItem Insert(int index, string key, string text, int imageIndex)
			{
				return this.Insert(index, new ListViewItem(text, imageIndex)
				{
					Name = key
				});
			}

			// Token: 0x06004546 RID: 17734 RVA: 0x000FBDD5 File Offset: 0x000FADD5
			public virtual void Remove(ListViewItem item)
			{
				this.InnerList.Remove(item);
			}

			// Token: 0x06004547 RID: 17735 RVA: 0x000FBDE4 File Offset: 0x000FADE4
			public virtual void RemoveAt(int index)
			{
				if (index < 0 || index >= this.Count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.InnerList.RemoveAt(index);
			}

			// Token: 0x06004548 RID: 17736 RVA: 0x000FBE40 File Offset: 0x000FAE40
			public virtual void RemoveByKey(string key)
			{
				int num = this.IndexOfKey(key);
				if (this.IsValidIndex(num))
				{
					this.RemoveAt(num);
				}
			}

			// Token: 0x06004549 RID: 17737 RVA: 0x000FBE65 File Offset: 0x000FAE65
			void IList.Remove(object item)
			{
				if (item == null || !(item is ListViewItem))
				{
					return;
				}
				this.Remove((ListViewItem)item);
			}

			// Token: 0x04002155 RID: 8533
			private int lastAccessedIndex = -1;

			// Token: 0x04002156 RID: 8534
			private ListView.ListViewItemCollection.IInnerList innerList;

			// Token: 0x02000488 RID: 1160
			internal interface IInnerList
			{
				// Token: 0x17000DA5 RID: 3493
				// (get) Token: 0x0600454A RID: 17738
				int Count { get; }

				// Token: 0x17000DA6 RID: 3494
				// (get) Token: 0x0600454B RID: 17739
				bool OwnerIsVirtualListView { get; }

				// Token: 0x17000DA7 RID: 3495
				// (get) Token: 0x0600454C RID: 17740
				bool OwnerIsDesignMode { get; }

				// Token: 0x17000DA8 RID: 3496
				ListViewItem this[int index] { get; set; }

				// Token: 0x0600454F RID: 17743
				ListViewItem Add(ListViewItem item);

				// Token: 0x06004550 RID: 17744
				void AddRange(ListViewItem[] items);

				// Token: 0x06004551 RID: 17745
				void Clear();

				// Token: 0x06004552 RID: 17746
				bool Contains(ListViewItem item);

				// Token: 0x06004553 RID: 17747
				void CopyTo(Array dest, int index);

				// Token: 0x06004554 RID: 17748
				IEnumerator GetEnumerator();

				// Token: 0x06004555 RID: 17749
				int IndexOf(ListViewItem item);

				// Token: 0x06004556 RID: 17750
				ListViewItem Insert(int index, ListViewItem item);

				// Token: 0x06004557 RID: 17751
				void Remove(ListViewItem item);

				// Token: 0x06004558 RID: 17752
				void RemoveAt(int index);
			}
		}

		// Token: 0x02000489 RID: 1161
		internal class ListViewNativeItemCollection : ListView.ListViewItemCollection.IInnerList
		{
			// Token: 0x06004559 RID: 17753 RVA: 0x000FBE7F File Offset: 0x000FAE7F
			public ListViewNativeItemCollection(ListView owner)
			{
				this.owner = owner;
			}

			// Token: 0x17000DA9 RID: 3497
			// (get) Token: 0x0600455A RID: 17754 RVA: 0x000FBE8E File Offset: 0x000FAE8E
			public int Count
			{
				get
				{
					this.owner.ApplyUpdateCachedItems();
					if (this.owner.VirtualMode)
					{
						return this.owner.VirtualListSize;
					}
					return this.owner.itemCount;
				}
			}

			// Token: 0x17000DAA RID: 3498
			// (get) Token: 0x0600455B RID: 17755 RVA: 0x000FBEBF File Offset: 0x000FAEBF
			public bool OwnerIsVirtualListView
			{
				get
				{
					return this.owner.VirtualMode;
				}
			}

			// Token: 0x17000DAB RID: 3499
			// (get) Token: 0x0600455C RID: 17756 RVA: 0x000FBECC File Offset: 0x000FAECC
			public bool OwnerIsDesignMode
			{
				get
				{
					/*
An exception occurred when decompiling this method (0600455C)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Boolean System.Windows.Forms.ListView/ListViewNativeItemCollection::get_OwnerIsDesignMode()

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.NRefactory.Utils.TreeTraversal.PostOrder[T](IEnumerable`1 input, Func`2 recursion)+MoveNext()
   at System.Linq.Enumerable.OfTypeIterator[TResult](IEnumerable source)+MoveNext()
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 404
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
				}
			}

			// Token: 0x17000DAC RID: 3500
			public ListViewItem this[int displayIndex]
			{
				get
				{
					this.owner.ApplyUpdateCachedItems();
					if (this.owner.VirtualMode)
					{
						RetrieveVirtualItemEventArgs retrieveVirtualItemEventArgs = new RetrieveVirtualItemEventArgs(displayIndex);
						this.owner.OnRetrieveVirtualItem(retrieveVirtualItemEventArgs);
						retrieveVirtualItemEventArgs.Item.SetItemIndex(this.owner, displayIndex);
						return retrieveVirtualItemEventArgs.Item;
					}
					if (displayIndex < 0 || displayIndex >= this.owner.itemCount)
					{
						throw new ArgumentOutOfRangeException("displayIndex", SR.GetString("InvalidArgument", new object[]
						{
							"displayIndex",
							displayIndex.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (this.owner.IsHandleCreated && !this.owner.ListViewHandleDestroyed)
					{
						return (ListViewItem)this.owner.listItemsTable[this.DisplayIndexToID(displayIndex)];
					}
					return (ListViewItem)this.owner.listItemsArray[displayIndex];
				}
				set
				{
					this.owner.ApplyUpdateCachedItems();
					if (this.owner.VirtualMode)
					{
						throw new InvalidOperationException(SR.GetString("ListViewCantModifyTheItemCollInAVirtualListView"));
					}
					if (displayIndex < 0 || displayIndex >= this.owner.itemCount)
					{
						throw new ArgumentOutOfRangeException("displayIndex", SR.GetString("InvalidArgument", new object[]
						{
							"displayIndex",
							displayIndex.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (this.owner.ExpectingMouseUp)
					{
						this.owner.ItemCollectionChangedInMouseDown = true;
					}
					this.RemoveAt(displayIndex);
					this.Insert(displayIndex, value);
				}
			}

			// Token: 0x0600455F RID: 17759 RVA: 0x000FC06C File Offset: 0x000FB06C
			public ListViewItem Add(ListViewItem value)
			{
				/*
An exception occurred when decompiling this method (0600455F)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Windows.Forms.ListViewItem System.Windows.Forms.ListView/ListViewNativeItemCollection::Add(System.Windows.Forms.ListViewItem)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at System.Collections.Generic.List`1.set_Capacity(Int32 value)
   at System.Collections.Generic.List`1.AddWithResize(T item)
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.BuildGraph(List`1 nodes, ILLabel entryLabel) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 123
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.FindLoops(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 53
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 343
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
			}

			// Token: 0x06004560 RID: 17760 RVA: 0x000FC104 File Offset: 0x000FB104
			public void AddRange(ListViewItem[] values)
			{
				if (values == null)
				{
					throw new ArgumentNullException("values");
				}
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAddItemsToAVirtualListView"));
				}
				IComparer listItemSorter = this.owner.listItemSorter;
				this.owner.listItemSorter = null;
				bool[] array = null;
				if (this.owner.IsHandleCreated && !this.owner.CheckBoxes)
				{
					array = new bool[values.Length];
					for (int i = 0; i < values.Length; i++)
					{
						array[i] = values[i].Checked;
					}
				}
				try
				{
					this.owner.BeginUpdate();
					this.owner.InsertItems(this.owner.itemCount, values, true);
					if (this.owner.IsHandleCreated && !this.owner.CheckBoxes)
					{
						for (int j = 0; j < values.Length; j++)
						{
							if (array[j])
							{
								this.owner.UpdateSavedCheckedItems(values[j], true);
							}
						}
					}
				}
				finally
				{
					this.owner.listItemSorter = listItemSorter;
					this.owner.EndUpdate();
				}
				if (this.owner.ExpectingMouseUp)
				{
					this.owner.ItemCollectionChangedInMouseDown = true;
				}
				if (listItemSorter != null || (this.owner.Sorting != SortOrder.None && !this.owner.VirtualMode))
				{
					this.owner.Sort();
				}
			}

			// Token: 0x06004561 RID: 17761 RVA: 0x000FC25C File Offset: 0x000FB25C
			private int DisplayIndexToID(int displayIndex)
			{
				if (this.owner.IsHandleCreated && !this.owner.ListViewHandleDestroyed)
				{
					NativeMethods.LVITEM lvitem = default(NativeMethods.LVITEM);
					lvitem.mask = 4;
					lvitem.iItem = displayIndex;
					UnsafeNativeMethods.SendMessage(new HandleRef(this.owner, this.owner.Handle), NativeMethods.LVM_GETITEM, 0, ref lvitem);
					return (int)lvitem.lParam;
				}
				return this[displayIndex].ID;
			}

			// Token: 0x06004562 RID: 17762 RVA: 0x000FC2D8 File Offset: 0x000FB2D8
			public void Clear()
			{
				if (this.owner.itemCount > 0)
				{
					this.owner.ApplyUpdateCachedItems();
					if (this.owner.IsHandleCreated && !this.owner.ListViewHandleDestroyed)
					{
						int count = this.owner.Items.Count;
						int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this.owner, this.owner.Handle), 4108, -1, 2);
						for (int i = 0; i < count; i++)
						{
							ListViewItem listViewItem = this.owner.Items[i];
							if (listViewItem != null)
							{
								if (i == num)
								{
									listViewItem.StateSelected = true;
									num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this.owner, this.owner.Handle), 4108, num, 2);
								}
								else
								{
									listViewItem.StateSelected = false;
								}
								listViewItem.UnHost(i, false);
							}
						}
						UnsafeNativeMethods.SendMessage(new HandleRef(this.owner, this.owner.Handle), 4105, 0, 0);
						if (this.owner.View == View.SmallIcon)
						{
							if (this.owner.ComctlSupportsVisualStyles)
							{
								this.owner.FlipViewToLargeIconAndSmallIcon = true;
							}
							else
							{
								this.owner.View = View.LargeIcon;
								this.owner.View = View.SmallIcon;
							}
						}
					}
					else
					{
						int count2 = this.owner.Items.Count;
						for (int j = 0; j < count2; j++)
						{
							ListViewItem listViewItem2 = this.owner.Items[j];
							if (listViewItem2 != null)
							{
								listViewItem2.UnHost(j, true);
							}
						}
						this.owner.listItemsArray.Clear();
					}
					this.owner.listItemsTable.Clear();
					if (this.owner.IsHandleCreated && !this.owner.CheckBoxes)
					{
						this.owner.savedCheckedItems = null;
					}
					this.owner.itemCount = 0;
					if (this.owner.ExpectingMouseUp)
					{
						this.owner.ItemCollectionChangedInMouseDown = true;
					}
				}
			}

			// Token: 0x06004563 RID: 17763 RVA: 0x000FC4D8 File Offset: 0x000FB4D8
			public bool Contains(ListViewItem item)
			{
				this.owner.ApplyUpdateCachedItems();
				if (this.owner.IsHandleCreated && !this.owner.ListViewHandleDestroyed)
				{
					return this.owner.listItemsTable[item.ID] == item;
				}
				return this.owner.listItemsArray.Contains(item);
			}

			// Token: 0x06004564 RID: 17764 RVA: 0x000FC53C File Offset: 0x000FB53C
			public ListViewItem Insert(int index, ListViewItem item)
			{
				int num;
				if (this.owner.VirtualMode)
				{
					num = this.Count;
				}
				else
				{
					num = this.owner.itemCount;
				}
				if (index < 0 || index > num)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantAddItemsToAVirtualListView"));
				}
				if (index < num)
				{
					this.owner.ApplyUpdateCachedItems();
				}
				this.owner.InsertItems(index, new ListViewItem[] { item }, true);
				if (this.owner.IsHandleCreated && !this.owner.CheckBoxes && item.Checked)
				{
					this.owner.UpdateSavedCheckedItems(item, true);
				}
				if (this.owner.ExpectingMouseUp)
				{
					this.owner.ItemCollectionChangedInMouseDown = true;
				}
				return item;
			}

			// Token: 0x06004565 RID: 17765 RVA: 0x000FC638 File Offset: 0x000FB638
			public int IndexOf(ListViewItem item)
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (item == this[i])
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x06004566 RID: 17766 RVA: 0x000FC664 File Offset: 0x000FB664
			public void Remove(ListViewItem item)
			{
				int num = (this.owner.VirtualMode ? (this.Count - 1) : this.IndexOf(item));
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantRemoveItemsFromAVirtualListView"));
				}
				if (num != -1)
				{
					this.RemoveAt(num);
				}
			}

			// Token: 0x06004567 RID: 17767 RVA: 0x000FC6B8 File Offset: 0x000FB6B8
			public void RemoveAt(int index)
			{
				if (this.owner.VirtualMode)
				{
					throw new InvalidOperationException(SR.GetString("ListViewCantRemoveItemsFromAVirtualListView"));
				}
				if (index < 0 || index >= this.owner.itemCount)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.owner.IsHandleCreated && !this.owner.CheckBoxes && this[index].Checked)
				{
					this.owner.UpdateSavedCheckedItems(this[index], false);
				}
				this.owner.ApplyUpdateCachedItems();
				int num = this.DisplayIndexToID(index);
				this[index].UnHost(true);
				if (this.owner.IsHandleCreated)
				{
					if ((int)this.owner.SendMessage(4104, index, 0) == 0)
					{
						throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
				}
				else
				{
					this.owner.listItemsArray.RemoveAt(index);
				}
				this.owner.itemCount--;
				this.owner.listItemsTable.Remove(num);
				if (this.owner.ExpectingMouseUp)
				{
					this.owner.ItemCollectionChangedInMouseDown = true;
				}
			}

			// Token: 0x06004568 RID: 17768 RVA: 0x000FC830 File Offset: 0x000FB830
			public void CopyTo(Array dest, int index)
			{
				if (this.owner.itemCount > 0)
				{
					for (int i = 0; i < this.Count; i++)
					{
						dest.SetValue(this[i], index++);
					}
				}
			}

			// Token: 0x06004569 RID: 17769 RVA: 0x000FC870 File Offset: 0x000FB870
			public IEnumerator GetEnumerator()
			{
				ListViewItem[] array = new ListViewItem[this.owner.itemCount];
				this.CopyTo(array, 0);
				return array.GetEnumerator();
			}

			// Token: 0x04002157 RID: 8535
			private ListView owner;
		}
	}
}
