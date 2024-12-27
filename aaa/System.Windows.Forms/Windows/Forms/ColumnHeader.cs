using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x0200027E RID: 638
	[ToolboxItem(false)]
	[DefaultProperty("Text")]
	[DesignTimeVisible(false)]
	[TypeConverter(typeof(ColumnHeaderConverter))]
	public class ColumnHeader : Component, ICloneable
	{
		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06002247 RID: 8775 RVA: 0x0004AB9E File Offset: 0x00049B9E
		// (set) Token: 0x06002248 RID: 8776 RVA: 0x0004ABA8 File Offset: 0x00049BA8
		internal ListView OwnerListview
		{
			get
			{
				return this.listview;
			}
			set
			{
				int num = this.Width;
				this.listview = value;
				this.Width = num;
			}
		}

		// Token: 0x06002249 RID: 8777 RVA: 0x0004ABCA File Offset: 0x00049BCA
		public ColumnHeader()
		{
			this.imageIndexer = new ColumnHeader.ColumnHeaderImageListIndexer(this);
		}

		// Token: 0x0600224A RID: 8778 RVA: 0x0004ABF4 File Offset: 0x00049BF4
		public ColumnHeader(int imageIndex)
			: this()
		{
			this.ImageIndex = imageIndex;
		}

		// Token: 0x0600224B RID: 8779 RVA: 0x0004AC03 File Offset: 0x00049C03
		public ColumnHeader(string imageKey)
			: this()
		{
			this.ImageKey = imageKey;
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x0600224C RID: 8780 RVA: 0x0004AC14 File Offset: 0x00049C14
		internal int ActualImageIndex_Internal
		{
			get
			{
				int actualIndex = this.imageIndexer.ActualIndex;
				if (this.ImageList == null || this.ImageList.Images == null || actualIndex >= this.ImageList.Images.Count)
				{
					return -1;
				}
				return actualIndex;
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x0600224D RID: 8781 RVA: 0x0004AC58 File Offset: 0x00049C58
		// (set) Token: 0x0600224E RID: 8782 RVA: 0x0004AC60 File Offset: 0x00049C60
		[SRDescription("ColumnHeaderDisplayIndexDescr")]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRCategory("CatBehavior")]
		public int DisplayIndex
		{
			get
			{
				return this.DisplayIndexInternal;
			}
			set
			{
				if (this.listview == null)
				{
					this.DisplayIndexInternal = value;
					return;
				}
				if (value < 0 || value > this.listview.Columns.Count - 1)
				{
					throw new ArgumentOutOfRangeException("DisplayIndex", SR.GetString("ColumnHeaderBadDisplayIndex"));
				}
				int num = Math.Min(this.DisplayIndexInternal, value);
				int num2 = Math.Max(this.DisplayIndexInternal, value);
				int[] array = new int[this.listview.Columns.Count];
				bool flag = value > this.DisplayIndexInternal;
				ColumnHeader columnHeader = null;
				for (int i = 0; i < this.listview.Columns.Count; i++)
				{
					ColumnHeader columnHeader2 = this.listview.Columns[i];
					if (columnHeader2.DisplayIndex == this.DisplayIndexInternal)
					{
						columnHeader = columnHeader2;
					}
					else if (columnHeader2.DisplayIndex >= num && columnHeader2.DisplayIndex <= num2)
					{
						columnHeader2.DisplayIndexInternal -= (flag ? 1 : (-1));
					}
					if (i != this.Index)
					{
						array[columnHeader2.DisplayIndexInternal] = i;
					}
				}
				columnHeader.DisplayIndexInternal = value;
				array[columnHeader.DisplayIndexInternal] = columnHeader.Index;
				this.SetDisplayIndices(array);
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x0600224F RID: 8783 RVA: 0x0004AD90 File Offset: 0x00049D90
		// (set) Token: 0x06002250 RID: 8784 RVA: 0x0004AD98 File Offset: 0x00049D98
		internal int DisplayIndexInternal
		{
			get
			{
				return this.displayIndexInternal;
			}
			set
			{
				this.displayIndexInternal = value;
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06002251 RID: 8785 RVA: 0x0004ADA1 File Offset: 0x00049DA1
		[Browsable(false)]
		public int Index
		{
			get
			{
				if (this.listview != null)
				{
					return this.listview.GetColumnIndex(this);
				}
				return -1;
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06002252 RID: 8786 RVA: 0x0004ADBC File Offset: 0x00049DBC
		// (set) Token: 0x06002253 RID: 8787 RVA: 0x0004AE1C File Offset: 0x00049E1C
		[TypeConverter(typeof(ImageIndexConverter))]
		[DefaultValue(-1)]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int ImageIndex
		{
			get
			{
				if (this.imageIndexer.Index != -1 && this.ImageList != null && this.imageIndexer.Index >= this.ImageList.Images.Count)
				{
					return this.ImageList.Images.Count - 1;
				}
				return this.imageIndexer.Index;
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("ImageIndex", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"ImageIndex",
						value.ToString(CultureInfo.CurrentCulture),
						(-1).ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.imageIndexer.Index != value)
				{
					this.imageIndexer.Index = value;
					if (this.ListView != null && this.ListView.IsHandleCreated)
					{
						this.ListView.SetColumnInfo(16, this);
					}
				}
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06002254 RID: 8788 RVA: 0x0004AEAF File Offset: 0x00049EAF
		[Browsable(false)]
		public ImageList ImageList
		{
			get
			{
				return this.imageIndexer.ImageList;
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06002255 RID: 8789 RVA: 0x0004AEBC File Offset: 0x00049EBC
		// (set) Token: 0x06002256 RID: 8790 RVA: 0x0004AECC File Offset: 0x00049ECC
		[TypeConverter(typeof(ImageKeyConverter))]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue("")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public string ImageKey
		{
			get
			{
				return this.imageIndexer.Key;
			}
			set
			{
				if (value != this.imageIndexer.Key)
				{
					this.imageIndexer.Key = value;
					if (this.ListView != null && this.ListView.IsHandleCreated)
					{
						this.ListView.SetColumnInfo(16, this);
					}
				}
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06002257 RID: 8791 RVA: 0x0004AF1B File Offset: 0x00049F1B
		[Browsable(false)]
		public ListView ListView
		{
			get
			{
				return this.listview;
			}
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06002258 RID: 8792 RVA: 0x0004AF23 File Offset: 0x00049F23
		// (set) Token: 0x06002259 RID: 8793 RVA: 0x0004AF31 File Offset: 0x00049F31
		[SRDescription("ColumnHeaderNameDescr")]
		[Browsable(false)]
		public string Name
		{
			get
			{
				return WindowsFormsUtils.GetComponentName(this, this.name);
			}
			set
			{
				if (value == null)
				{
					this.name = "";
				}
				else
				{
					this.name = value;
				}
				if (this.Site != null)
				{
					this.Site.Name = value;
				}
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x0600225A RID: 8794 RVA: 0x0004AF5E File Offset: 0x00049F5E
		// (set) Token: 0x0600225B RID: 8795 RVA: 0x0004AF74 File Offset: 0x00049F74
		[SRDescription("ColumnCaption")]
		[Localizable(true)]
		public string Text
		{
			get
			{
				if (this.text == null)
				{
					return "ColumnHeader";
				}
				return this.text;
			}
			set
			{
				if (value == null)
				{
					this.text = "";
				}
				else
				{
					this.text = value;
				}
				if (this.listview != null)
				{
					this.listview.SetColumnInfo(4, this);
				}
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x0600225C RID: 8796 RVA: 0x0004AFA4 File Offset: 0x00049FA4
		// (set) Token: 0x0600225D RID: 8797 RVA: 0x0004AFF8 File Offset: 0x00049FF8
		[SRDescription("ColumnAlignment")]
		[DefaultValue(HorizontalAlignment.Left)]
		[Localizable(true)]
		public HorizontalAlignment TextAlign
		{
			get
			{
				if (!this.textAlignInitialized && this.listview != null)
				{
					this.textAlignInitialized = true;
					if (this.Index != 0 && this.listview.RightToLeft == RightToLeft.Yes && !this.listview.IsMirrored)
					{
						this.textAlign = HorizontalAlignment.Right;
					}
				}
				return this.textAlign;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(HorizontalAlignment));
				}
				this.textAlign = value;
				if (this.Index == 0 && this.textAlign != HorizontalAlignment.Left)
				{
					this.textAlign = HorizontalAlignment.Left;
				}
				if (this.listview != null)
				{
					this.listview.SetColumnInfo(1, this);
					this.listview.Invalidate();
				}
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x0600225E RID: 8798 RVA: 0x0004B069 File Offset: 0x0004A069
		// (set) Token: 0x0600225F RID: 8799 RVA: 0x0004B071 File Offset: 0x0004A071
		[SRCategory("CatData")]
		[Localizable(false)]
		[TypeConverter(typeof(StringConverter))]
		[Bindable(true)]
		[SRDescription("ControlTagDescr")]
		[DefaultValue(null)]
		public object Tag
		{
			get
			{
				return this.userData;
			}
			set
			{
				this.userData = value;
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06002260 RID: 8800 RVA: 0x0004B07A File Offset: 0x0004A07A
		internal int WidthInternal
		{
			get
			{
				return this.width;
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06002261 RID: 8801 RVA: 0x0004B084 File Offset: 0x0004A084
		// (set) Token: 0x06002262 RID: 8802 RVA: 0x0004B15C File Offset: 0x0004A15C
		[SRDescription("ColumnWidth")]
		[DefaultValue(60)]
		[Localizable(true)]
		public int Width
		{
			get
			{
				if (this.listview != null && this.listview.IsHandleCreated && !this.listview.Disposing && this.listview.View == View.Details)
				{
					IntPtr intPtr = UnsafeNativeMethods.SendMessage(new HandleRef(this.listview, this.listview.Handle), 4127, 0, 0);
					if (intPtr != IntPtr.Zero)
					{
						int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this.listview, intPtr), 4608, 0, 0);
						if (this.Index < num)
						{
							this.width = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this.listview, this.listview.Handle), 4125, this.Index, 0);
						}
					}
				}
				return this.width;
			}
			set
			{
				this.width = value;
				if (this.listview != null)
				{
					this.listview.SetColumnWidth(this.Index, ColumnHeaderAutoResizeStyle.None);
				}
			}
		}

		// Token: 0x06002263 RID: 8803 RVA: 0x0004B17F File Offset: 0x0004A17F
		public void AutoResize(ColumnHeaderAutoResizeStyle headerAutoResize)
		{
			if (headerAutoResize < ColumnHeaderAutoResizeStyle.None || headerAutoResize > ColumnHeaderAutoResizeStyle.ColumnContent)
			{
				throw new InvalidEnumArgumentException("headerAutoResize", (int)headerAutoResize, typeof(ColumnHeaderAutoResizeStyle));
			}
			if (this.listview != null)
			{
				this.listview.AutoResizeColumn(this.Index, headerAutoResize);
			}
		}

		// Token: 0x06002264 RID: 8804 RVA: 0x0004B1BC File Offset: 0x0004A1BC
		public object Clone()
		{
			Type type = base.GetType();
			ColumnHeader columnHeader;
			if (type == typeof(ColumnHeader))
			{
				columnHeader = new ColumnHeader();
			}
			else
			{
				columnHeader = (ColumnHeader)Activator.CreateInstance(type);
			}
			columnHeader.text = this.text;
			columnHeader.Width = this.width;
			columnHeader.textAlign = this.TextAlign;
			return columnHeader;
		}

		// Token: 0x06002265 RID: 8805 RVA: 0x0004B218 File Offset: 0x0004A218
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.listview != null)
			{
				int num = this.Index;
				if (num != -1)
				{
					this.listview.Columns.RemoveAt(num);
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06002266 RID: 8806 RVA: 0x0004B253 File Offset: 0x0004A253
		private void ResetText()
		{
			this.Text = null;
		}

		// Token: 0x06002267 RID: 8807 RVA: 0x0004B25C File Offset: 0x0004A25C
		private void SetDisplayIndices(int[] cols)
		{
			if (this.listview.IsHandleCreated && !this.listview.Disposing)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this.listview, this.listview.Handle), 4154, cols.Length, cols);
			}
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x0004B2A8 File Offset: 0x0004A2A8
		private bool ShouldSerializeName()
		{
			return !string.IsNullOrEmpty(this.name);
		}

		// Token: 0x06002269 RID: 8809 RVA: 0x0004B2B8 File Offset: 0x0004A2B8
		private bool ShouldSerializeDisplayIndex()
		{
			return this.DisplayIndex != this.Index;
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x0004B2CB File Offset: 0x0004A2CB
		internal bool ShouldSerializeText()
		{
			return this.text != null;
		}

		// Token: 0x0600226B RID: 8811 RVA: 0x0004B2D9 File Offset: 0x0004A2D9
		public override string ToString()
		{
			return "ColumnHeader: Text: " + this.Text;
		}

		// Token: 0x04001504 RID: 5380
		internal int index = -1;

		// Token: 0x04001505 RID: 5381
		internal string text;

		// Token: 0x04001506 RID: 5382
		internal string name;

		// Token: 0x04001507 RID: 5383
		internal int width = 60;

		// Token: 0x04001508 RID: 5384
		private HorizontalAlignment textAlign;

		// Token: 0x04001509 RID: 5385
		private bool textAlignInitialized;

		// Token: 0x0400150A RID: 5386
		private int displayIndexInternal = -1;

		// Token: 0x0400150B RID: 5387
		private ColumnHeader.ColumnHeaderImageListIndexer imageIndexer;

		// Token: 0x0400150C RID: 5388
		private object userData;

		// Token: 0x0400150D RID: 5389
		private ListView listview;

		// Token: 0x02000286 RID: 646
		internal class ColumnHeaderImageListIndexer : ImageList.Indexer
		{
			// Token: 0x060022D0 RID: 8912 RVA: 0x0004CD93 File Offset: 0x0004BD93
			public ColumnHeaderImageListIndexer(ColumnHeader ch)
			{
				this.owner = ch;
			}

			// Token: 0x17000546 RID: 1350
			// (get) Token: 0x060022D1 RID: 8913 RVA: 0x0004CDA2 File Offset: 0x0004BDA2
			// (set) Token: 0x060022D2 RID: 8914 RVA: 0x0004CDCB File Offset: 0x0004BDCB
			public override ImageList ImageList
			{
				get
				{
					if (this.owner != null && this.owner.ListView != null)
					{
						return this.owner.ListView.SmallImageList;
					}
					return null;
				}
				set
				{
				}
			}

			// Token: 0x0400152E RID: 5422
			private ColumnHeader owner;
		}
	}
}
