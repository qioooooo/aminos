using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000428 RID: 1064
	public abstract class ScrollProperties
	{
		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x06003F4A RID: 16202 RVA: 0x000E5C4B File Offset: 0x000E4C4B
		protected ScrollableControl ParentControl
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x06003F4B RID: 16203 RVA: 0x000E5C53 File Offset: 0x000E4C53
		protected ScrollProperties(ScrollableControl container)
		{
			this.parent = container;
		}

		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x06003F4C RID: 16204 RVA: 0x000E5C80 File Offset: 0x000E4C80
		// (set) Token: 0x06003F4D RID: 16205 RVA: 0x000E5C88 File Offset: 0x000E4C88
		[DefaultValue(true)]
		[SRDescription("ScrollBarEnableDescr")]
		[SRCategory("CatBehavior")]
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				if (this.parent.AutoScroll)
				{
					return;
				}
				if (value != this.enabled)
				{
					this.enabled = value;
					this.EnableScroll(value);
				}
			}
		}

		// Token: 0x17000C2B RID: 3115
		// (get) Token: 0x06003F4E RID: 16206 RVA: 0x000E5CAF File Offset: 0x000E4CAF
		// (set) Token: 0x06003F4F RID: 16207 RVA: 0x000E5CCC File Offset: 0x000E4CCC
		[DefaultValue(10)]
		[SRCategory("CatBehavior")]
		[SRDescription("ScrollBarLargeChangeDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public int LargeChange
		{
			get
			{
				return Math.Min(this.largeChange, this.maximum - this.minimum + 1);
			}
			set
			{
				if (this.largeChange != value)
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("LargeChange", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"LargeChange",
							value.ToString(CultureInfo.CurrentCulture),
							0.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.largeChange = value;
					this.largeChangeSetExternally = true;
					this.UpdateScrollInfo();
				}
			}
		}

		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x06003F50 RID: 16208 RVA: 0x000E5D3F File Offset: 0x000E4D3F
		// (set) Token: 0x06003F51 RID: 16209 RVA: 0x000E5D48 File Offset: 0x000E4D48
		[DefaultValue(100)]
		[SRDescription("ScrollBarMaximumDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRCategory("CatBehavior")]
		public int Maximum
		{
			get
			{
				return this.maximum;
			}
			set
			{
				if (this.parent.AutoScroll)
				{
					return;
				}
				if (this.maximum != value)
				{
					if (this.minimum > value)
					{
						this.minimum = value;
					}
					if (value < this.value)
					{
						this.Value = value;
					}
					this.maximum = value;
					this.maximumSetExternally = true;
					this.UpdateScrollInfo();
				}
			}
		}

		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x06003F52 RID: 16210 RVA: 0x000E5DA0 File Offset: 0x000E4DA0
		// (set) Token: 0x06003F53 RID: 16211 RVA: 0x000E5DA8 File Offset: 0x000E4DA8
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("ScrollBarMinimumDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(0)]
		public int Minimum
		{
			get
			{
				return this.minimum;
			}
			set
			{
				if (this.parent.AutoScroll)
				{
					return;
				}
				if (this.minimum != value)
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("Minimum", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"Minimum",
							value.ToString(CultureInfo.CurrentCulture),
							0.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (this.maximum < value)
					{
						this.maximum = value;
					}
					if (value > this.value)
					{
						this.value = value;
					}
					this.minimum = value;
					this.UpdateScrollInfo();
				}
			}
		}

		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x06003F54 RID: 16212
		internal abstract int PageSize { get; }

		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x06003F55 RID: 16213
		internal abstract int Orientation { get; }

		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x06003F56 RID: 16214
		internal abstract int HorizontalDisplayPosition { get; }

		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x06003F57 RID: 16215
		internal abstract int VerticalDisplayPosition { get; }

		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x06003F58 RID: 16216 RVA: 0x000E5E42 File Offset: 0x000E4E42
		// (set) Token: 0x06003F59 RID: 16217 RVA: 0x000E5E58 File Offset: 0x000E4E58
		[DefaultValue(1)]
		[SRDescription("ScrollBarSmallChangeDescr")]
		[SRCategory("CatBehavior")]
		public int SmallChange
		{
			get
			{
				return Math.Min(this.smallChange, this.LargeChange);
			}
			set
			{
				if (this.smallChange != value)
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("SmallChange", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"SmallChange",
							value.ToString(CultureInfo.CurrentCulture),
							0.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.smallChange = value;
					this.smallChangeSetExternally = true;
					this.UpdateScrollInfo();
				}
			}
		}

		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x06003F5A RID: 16218 RVA: 0x000E5ECB File Offset: 0x000E4ECB
		// (set) Token: 0x06003F5B RID: 16219 RVA: 0x000E5ED4 File Offset: 0x000E4ED4
		[SRCategory("CatBehavior")]
		[DefaultValue(0)]
		[Bindable(true)]
		[SRDescription("ScrollBarValueDescr")]
		public int Value
		{
			get
			{
				return this.value;
			}
			set
			{
				if (this.value != value)
				{
					if (value < this.minimum || value > this.maximum)
					{
						throw new ArgumentOutOfRangeException("Value", SR.GetString("InvalidBoundArgument", new object[]
						{
							"Value",
							value.ToString(CultureInfo.CurrentCulture),
							"'minimum'",
							"'maximum'"
						}));
					}
					this.value = value;
					this.UpdateScrollInfo();
					this.parent.SetDisplayFromScrollProps(this.HorizontalDisplayPosition, this.VerticalDisplayPosition);
				}
			}
		}

		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x06003F5C RID: 16220 RVA: 0x000E5F64 File Offset: 0x000E4F64
		// (set) Token: 0x06003F5D RID: 16221 RVA: 0x000E5F6C File Offset: 0x000E4F6C
		[SRDescription("ScrollBarVisibleDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		public bool Visible
		{
			get
			{
				return this.visible;
			}
			set
			{
				if (this.parent.AutoScroll)
				{
					return;
				}
				if (value != this.visible)
				{
					this.visible = value;
					this.parent.UpdateStylesCore();
					this.UpdateScrollInfo();
					this.parent.SetDisplayFromScrollProps(this.HorizontalDisplayPosition, this.VerticalDisplayPosition);
				}
			}
		}

		// Token: 0x06003F5E RID: 16222 RVA: 0x000E5FC0 File Offset: 0x000E4FC0
		internal void UpdateScrollInfo()
		{
			if (this.parent.IsHandleCreated && this.visible)
			{
				NativeMethods.SCROLLINFO scrollinfo = new NativeMethods.SCROLLINFO();
				scrollinfo.cbSize = Marshal.SizeOf(typeof(NativeMethods.SCROLLINFO));
				scrollinfo.fMask = 23;
				scrollinfo.nMin = this.minimum;
				scrollinfo.nMax = this.maximum;
				scrollinfo.nPage = (this.parent.AutoScroll ? this.PageSize : this.LargeChange);
				scrollinfo.nPos = this.value;
				scrollinfo.nTrackPos = 0;
				UnsafeNativeMethods.SetScrollInfo(new HandleRef(this.parent, this.parent.Handle), this.Orientation, scrollinfo, true);
			}
		}

		// Token: 0x06003F5F RID: 16223 RVA: 0x000E607C File Offset: 0x000E507C
		private void EnableScroll(bool enable)
		{
			if (enable)
			{
				UnsafeNativeMethods.EnableScrollBar(new HandleRef(this.parent, this.parent.Handle), this.Orientation, 0);
				return;
			}
			UnsafeNativeMethods.EnableScrollBar(new HandleRef(this.parent, this.parent.Handle), this.Orientation, 3);
		}

		// Token: 0x04001F19 RID: 7961
		private const int SCROLL_LINE = 5;

		// Token: 0x04001F1A RID: 7962
		internal int minimum;

		// Token: 0x04001F1B RID: 7963
		internal int maximum = 100;

		// Token: 0x04001F1C RID: 7964
		internal int smallChange = 1;

		// Token: 0x04001F1D RID: 7965
		internal int largeChange = 10;

		// Token: 0x04001F1E RID: 7966
		internal int value;

		// Token: 0x04001F1F RID: 7967
		internal bool maximumSetExternally;

		// Token: 0x04001F20 RID: 7968
		internal bool smallChangeSetExternally;

		// Token: 0x04001F21 RID: 7969
		internal bool largeChangeSetExternally;

		// Token: 0x04001F22 RID: 7970
		private ScrollableControl parent;

		// Token: 0x04001F23 RID: 7971
		internal bool visible;

		// Token: 0x04001F24 RID: 7972
		private bool enabled = true;
	}
}
