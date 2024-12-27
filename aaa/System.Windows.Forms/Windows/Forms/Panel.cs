using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x02000401 RID: 1025
	[Docking(DockingBehavior.Ask)]
	[ComVisible(true)]
	[Designer("System.Windows.Forms.Design.PanelDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionPanel")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("BorderStyle")]
	[DefaultEvent("Paint")]
	public class Panel : ScrollableControl
	{
		// Token: 0x06003C50 RID: 15440 RVA: 0x000D95B8 File Offset: 0x000D85B8
		public Panel()
		{
			base.SetState2(2048, true);
			this.TabStop = false;
			base.SetStyle(ControlStyles.Selectable | ControlStyles.AllPaintingInWmPaint, false);
			base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
		}

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x06003C51 RID: 15441 RVA: 0x000D95EB File Offset: 0x000D85EB
		// (set) Token: 0x06003C52 RID: 15442 RVA: 0x000D95F3 File Offset: 0x000D85F3
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
			}
		}

		// Token: 0x140001F1 RID: 497
		// (add) Token: 0x06003C53 RID: 15443 RVA: 0x000D95FC File Offset: 0x000D85FC
		// (remove) Token: 0x06003C54 RID: 15444 RVA: 0x000D9605 File Offset: 0x000D8605
		[EditorBrowsable(EditorBrowsableState.Always)]
		[SRCategory("CatPropertyChanged")]
		[SRDescription("ControlOnAutoSizeChangedDescr")]
		[Browsable(true)]
		public new event EventHandler AutoSizeChanged
		{
			add
			{
				base.AutoSizeChanged += value;
			}
			remove
			{
				base.AutoSizeChanged -= value;
			}
		}

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x06003C55 RID: 15445 RVA: 0x000D960E File Offset: 0x000D860E
		// (set) Token: 0x06003C56 RID: 15446 RVA: 0x000D9618 File Offset: 0x000D8618
		[Localizable(true)]
		[SRDescription("ControlAutoSizeModeDescr")]
		[SRCategory("CatLayout")]
		[Browsable(true)]
		[DefaultValue(AutoSizeMode.GrowOnly)]
		public virtual AutoSizeMode AutoSizeMode
		{
			get
			{
				return base.GetAutoSizeMode();
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(AutoSizeMode));
				}
				if (base.GetAutoSizeMode() != value)
				{
					base.SetAutoSizeMode(value);
					if (this.ParentInternal != null)
					{
						if (this.ParentInternal.LayoutEngine == DefaultLayout.Instance)
						{
							this.ParentInternal.LayoutEngine.InitLayout(this, BoundsSpecified.Size);
						}
						LayoutTransaction.DoLayout(this.ParentInternal, this, PropertyNames.AutoSize);
					}
				}
			}
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x06003C57 RID: 15447 RVA: 0x000D9699 File Offset: 0x000D8699
		// (set) Token: 0x06003C58 RID: 15448 RVA: 0x000D96A1 File Offset: 0x000D86A1
		[DefaultValue(BorderStyle.None)]
		[SRCategory("CatAppearance")]
		[DispId(-504)]
		[SRDescription("PanelBorderStyleDescr")]
		public BorderStyle BorderStyle
		{
			get
			{
				return this.borderStyle;
			}
			set
			{
				if (this.borderStyle != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(BorderStyle));
					}
					this.borderStyle = value;
					base.UpdateStyles();
				}
			}
		}

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x06003C59 RID: 15449 RVA: 0x000D96E0 File Offset: 0x000D86E0
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= 65536;
				createParams.ExStyle &= -513;
				createParams.Style &= -8388609;
				switch (this.borderStyle)
				{
				case BorderStyle.FixedSingle:
					createParams.Style |= 8388608;
					break;
				case BorderStyle.Fixed3D:
					createParams.ExStyle |= 512;
					break;
				}
				return createParams;
			}
		}

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x06003C5A RID: 15450 RVA: 0x000D976A File Offset: 0x000D876A
		protected override Size DefaultSize
		{
			get
			{
				return new Size(200, 100);
			}
		}

		// Token: 0x06003C5B RID: 15451 RVA: 0x000D9778 File Offset: 0x000D8778
		internal override Size GetPreferredSizeCore(Size proposedSize)
		{
			Size size = this.SizeFromClientSize(Size.Empty);
			Size size2 = size + base.Padding.Size;
			return this.LayoutEngine.GetPreferredSize(this, proposedSize - size2) + size2;
		}

		// Token: 0x140001F2 RID: 498
		// (add) Token: 0x06003C5C RID: 15452 RVA: 0x000D97BF File Offset: 0x000D87BF
		// (remove) Token: 0x06003C5D RID: 15453 RVA: 0x000D97C8 File Offset: 0x000D87C8
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event KeyEventHandler KeyUp
		{
			add
			{
				base.KeyUp += value;
			}
			remove
			{
				base.KeyUp -= value;
			}
		}

		// Token: 0x140001F3 RID: 499
		// (add) Token: 0x06003C5E RID: 15454 RVA: 0x000D97D1 File Offset: 0x000D87D1
		// (remove) Token: 0x06003C5F RID: 15455 RVA: 0x000D97DA File Offset: 0x000D87DA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event KeyEventHandler KeyDown
		{
			add
			{
				base.KeyDown += value;
			}
			remove
			{
				base.KeyDown -= value;
			}
		}

		// Token: 0x140001F4 RID: 500
		// (add) Token: 0x06003C60 RID: 15456 RVA: 0x000D97E3 File Offset: 0x000D87E3
		// (remove) Token: 0x06003C61 RID: 15457 RVA: 0x000D97EC File Offset: 0x000D87EC
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event KeyPressEventHandler KeyPress
		{
			add
			{
				base.KeyPress += value;
			}
			remove
			{
				base.KeyPress -= value;
			}
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x06003C62 RID: 15458 RVA: 0x000D97F5 File Offset: 0x000D87F5
		// (set) Token: 0x06003C63 RID: 15459 RVA: 0x000D97FD File Offset: 0x000D87FD
		[DefaultValue(false)]
		public new bool TabStop
		{
			get
			{
				return base.TabStop;
			}
			set
			{
				base.TabStop = value;
			}
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x06003C64 RID: 15460 RVA: 0x000D9806 File Offset: 0x000D8806
		// (set) Token: 0x06003C65 RID: 15461 RVA: 0x000D980E File Offset: 0x000D880E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Bindable(false)]
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

		// Token: 0x140001F5 RID: 501
		// (add) Token: 0x06003C66 RID: 15462 RVA: 0x000D9817 File Offset: 0x000D8817
		// (remove) Token: 0x06003C67 RID: 15463 RVA: 0x000D9820 File Offset: 0x000D8820
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x06003C68 RID: 15464 RVA: 0x000D9829 File Offset: 0x000D8829
		protected override void OnResize(EventArgs eventargs)
		{
			if (base.DesignMode && this.borderStyle == BorderStyle.None)
			{
				base.Invalidate();
			}
			base.OnResize(eventargs);
		}

		// Token: 0x06003C69 RID: 15465 RVA: 0x000D9848 File Offset: 0x000D8848
		internal override void PrintToMetaFileRecursive(HandleRef hDC, IntPtr lParam, Rectangle bounds)
		{
			base.PrintToMetaFileRecursive(hDC, lParam, bounds);
			using (new WindowsFormsUtils.DCMapping(hDC, bounds))
			{
				using (Graphics graphics = Graphics.FromHdcInternal(hDC.Handle))
				{
					ControlPaint.PrintBorder(graphics, new Rectangle(Point.Empty, base.Size), this.BorderStyle, Border3DStyle.Sunken);
				}
			}
		}

		// Token: 0x06003C6A RID: 15466 RVA: 0x000D98CC File Offset: 0x000D88CC
		private static string StringFromBorderStyle(BorderStyle value)
		{
			Type typeFromHandle = typeof(BorderStyle);
			if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
			{
				return "[Invalid BorderStyle]";
			}
			return typeFromHandle.ToString() + "." + value.ToString();
		}

		// Token: 0x06003C6B RID: 15467 RVA: 0x000D9918 File Offset: 0x000D8918
		public override string ToString()
		{
			string text = base.ToString();
			return text + ", BorderStyle: " + Panel.StringFromBorderStyle(this.borderStyle);
		}

		// Token: 0x04001E1B RID: 7707
		private BorderStyle borderStyle;
	}
}
