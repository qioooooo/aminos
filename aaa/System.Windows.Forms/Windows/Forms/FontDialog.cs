using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x02000406 RID: 1030
	[SRDescription("DescriptionFontDialog")]
	[DefaultEvent("Apply")]
	[DefaultProperty("Font")]
	public class FontDialog : CommonDialog
	{
		// Token: 0x06003C90 RID: 15504 RVA: 0x000D9E26 File Offset: 0x000D8E26
		public FontDialog()
		{
			this.Reset();
		}

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x06003C91 RID: 15505 RVA: 0x000D9E34 File Offset: 0x000D8E34
		// (set) Token: 0x06003C92 RID: 15506 RVA: 0x000D9E44 File Offset: 0x000D8E44
		[SRCategory("CatBehavior")]
		[SRDescription("FnDallowSimulationsDescr")]
		[DefaultValue(true)]
		public bool AllowSimulations
		{
			get
			{
				return !this.GetOption(4096);
			}
			set
			{
				this.SetOption(4096, !value);
			}
		}

		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x06003C93 RID: 15507 RVA: 0x000D9E55 File Offset: 0x000D8E55
		// (set) Token: 0x06003C94 RID: 15508 RVA: 0x000D9E65 File Offset: 0x000D8E65
		[DefaultValue(true)]
		[SRDescription("FnDallowVectorFontsDescr")]
		[SRCategory("CatBehavior")]
		public bool AllowVectorFonts
		{
			get
			{
				return !this.GetOption(2048);
			}
			set
			{
				this.SetOption(2048, !value);
			}
		}

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x06003C95 RID: 15509 RVA: 0x000D9E76 File Offset: 0x000D8E76
		// (set) Token: 0x06003C96 RID: 15510 RVA: 0x000D9E86 File Offset: 0x000D8E86
		[SRDescription("FnDallowVerticalFontsDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		public bool AllowVerticalFonts
		{
			get
			{
				return !this.GetOption(16777216);
			}
			set
			{
				this.SetOption(16777216, !value);
			}
		}

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x06003C97 RID: 15511 RVA: 0x000D9E97 File Offset: 0x000D8E97
		// (set) Token: 0x06003C98 RID: 15512 RVA: 0x000D9EA7 File Offset: 0x000D8EA7
		[DefaultValue(true)]
		[SRCategory("CatBehavior")]
		[SRDescription("FnDallowScriptChangeDescr")]
		public bool AllowScriptChange
		{
			get
			{
				return !this.GetOption(4194304);
			}
			set
			{
				this.SetOption(4194304, !value);
			}
		}

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x06003C99 RID: 15513 RVA: 0x000D9EB8 File Offset: 0x000D8EB8
		// (set) Token: 0x06003C9A RID: 15514 RVA: 0x000D9EC0 File Offset: 0x000D8EC0
		[DefaultValue(typeof(Color), "Black")]
		[SRCategory("CatData")]
		[SRDescription("FnDcolorDescr")]
		public Color Color
		{
			get
			{
				return this.color;
			}
			set
			{
				if (!value.IsEmpty)
				{
					this.color = value;
					return;
				}
				this.color = Color.Black;
			}
		}

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x06003C9B RID: 15515 RVA: 0x000D9EDE File Offset: 0x000D8EDE
		// (set) Token: 0x06003C9C RID: 15516 RVA: 0x000D9EEB File Offset: 0x000D8EEB
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("FnDfixedPitchOnlyDescr")]
		public bool FixedPitchOnly
		{
			get
			{
				return this.GetOption(16384);
			}
			set
			{
				this.SetOption(16384, value);
			}
		}

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x06003C9D RID: 15517 RVA: 0x000D9EFC File Offset: 0x000D8EFC
		// (set) Token: 0x06003C9E RID: 15518 RVA: 0x000D9F79 File Offset: 0x000D8F79
		[SRCategory("CatData")]
		[SRDescription("FnDfontDescr")]
		public Font Font
		{
			get
			{
				Font font = this.font;
				if (font == null)
				{
					font = Control.DefaultFont;
				}
				float sizeInPoints = font.SizeInPoints;
				if (this.minSize != 0 && sizeInPoints < (float)this.MinSize)
				{
					font = new Font(font.FontFamily, (float)this.MinSize, font.Style, GraphicsUnit.Point);
				}
				if (this.maxSize != 0 && sizeInPoints > (float)this.MaxSize)
				{
					font = new Font(font.FontFamily, (float)this.MaxSize, font.Style, GraphicsUnit.Point);
				}
				return font;
			}
			set
			{
				this.font = value;
			}
		}

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x06003C9F RID: 15519 RVA: 0x000D9F82 File Offset: 0x000D8F82
		// (set) Token: 0x06003CA0 RID: 15520 RVA: 0x000D9F8F File Offset: 0x000D8F8F
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("FnDfontMustExistDescr")]
		public bool FontMustExist
		{
			get
			{
				return this.GetOption(65536);
			}
			set
			{
				this.SetOption(65536, value);
			}
		}

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x06003CA1 RID: 15521 RVA: 0x000D9F9D File Offset: 0x000D8F9D
		// (set) Token: 0x06003CA2 RID: 15522 RVA: 0x000D9FA5 File Offset: 0x000D8FA5
		[SRCategory("CatData")]
		[SRDescription("FnDmaxSizeDescr")]
		[DefaultValue(0)]
		public int MaxSize
		{
			get
			{
				return this.maxSize;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				this.maxSize = value;
				if (this.maxSize > 0 && this.maxSize < this.minSize)
				{
					this.minSize = this.maxSize;
				}
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x06003CA3 RID: 15523 RVA: 0x000D9FD8 File Offset: 0x000D8FD8
		// (set) Token: 0x06003CA4 RID: 15524 RVA: 0x000D9FE0 File Offset: 0x000D8FE0
		[SRCategory("CatData")]
		[DefaultValue(0)]
		[SRDescription("FnDminSizeDescr")]
		public int MinSize
		{
			get
			{
				return this.minSize;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				this.minSize = value;
				if (this.maxSize > 0 && this.maxSize < this.minSize)
				{
					this.maxSize = this.minSize;
				}
			}
		}

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x06003CA5 RID: 15525 RVA: 0x000DA013 File Offset: 0x000D9013
		protected int Options
		{
			get
			{
				return this.options;
			}
		}

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x06003CA6 RID: 15526 RVA: 0x000DA01B File Offset: 0x000D901B
		// (set) Token: 0x06003CA7 RID: 15527 RVA: 0x000DA028 File Offset: 0x000D9028
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("FnDscriptsOnlyDescr")]
		public bool ScriptsOnly
		{
			get
			{
				return this.GetOption(1024);
			}
			set
			{
				this.SetOption(1024, value);
			}
		}

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x06003CA8 RID: 15528 RVA: 0x000DA036 File Offset: 0x000D9036
		// (set) Token: 0x06003CA9 RID: 15529 RVA: 0x000DA043 File Offset: 0x000D9043
		[SRDescription("FnDshowApplyDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		public bool ShowApply
		{
			get
			{
				return this.GetOption(512);
			}
			set
			{
				this.SetOption(512, value);
			}
		}

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x06003CAA RID: 15530 RVA: 0x000DA051 File Offset: 0x000D9051
		// (set) Token: 0x06003CAB RID: 15531 RVA: 0x000DA059 File Offset: 0x000D9059
		[SRDescription("FnDshowColorDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		public bool ShowColor
		{
			get
			{
				return this.showColor;
			}
			set
			{
				this.showColor = value;
			}
		}

		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x06003CAC RID: 15532 RVA: 0x000DA062 File Offset: 0x000D9062
		// (set) Token: 0x06003CAD RID: 15533 RVA: 0x000DA06F File Offset: 0x000D906F
		[SRDescription("FnDshowEffectsDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		public bool ShowEffects
		{
			get
			{
				return this.GetOption(256);
			}
			set
			{
				this.SetOption(256, value);
			}
		}

		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x06003CAE RID: 15534 RVA: 0x000DA07D File Offset: 0x000D907D
		// (set) Token: 0x06003CAF RID: 15535 RVA: 0x000DA086 File Offset: 0x000D9086
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("FnDshowHelpDescr")]
		public bool ShowHelp
		{
			get
			{
				return this.GetOption(4);
			}
			set
			{
				this.SetOption(4, value);
			}
		}

		// Token: 0x140001F7 RID: 503
		// (add) Token: 0x06003CB0 RID: 15536 RVA: 0x000DA090 File Offset: 0x000D9090
		// (remove) Token: 0x06003CB1 RID: 15537 RVA: 0x000DA0A3 File Offset: 0x000D90A3
		[SRDescription("FnDapplyDescr")]
		public event EventHandler Apply
		{
			add
			{
				base.Events.AddHandler(FontDialog.EventApply, value);
			}
			remove
			{
				base.Events.RemoveHandler(FontDialog.EventApply, value);
			}
		}

		// Token: 0x06003CB2 RID: 15538 RVA: 0x000DA0B6 File Offset: 0x000D90B6
		internal bool GetOption(int option)
		{
			return (this.options & option) != 0;
		}

		// Token: 0x06003CB3 RID: 15539 RVA: 0x000DA0C8 File Offset: 0x000D90C8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override IntPtr HookProc(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
		{
			switch (msg)
			{
			case 272:
				break;
			case 273:
			{
				if ((int)wparam != 1026)
				{
					goto IL_0116;
				}
				NativeMethods.LOGFONT logfont = new NativeMethods.LOGFONT();
				UnsafeNativeMethods.SendMessage(new HandleRef(null, hWnd), 1025, 0, logfont);
				this.UpdateFont(logfont);
				int num = (int)UnsafeNativeMethods.SendDlgItemMessage(new HandleRef(null, hWnd), 1139, 327, IntPtr.Zero, IntPtr.Zero);
				if (num != -1)
				{
					this.UpdateColor((int)UnsafeNativeMethods.SendDlgItemMessage(new HandleRef(null, hWnd), 1139, 336, (IntPtr)num, IntPtr.Zero));
				}
				if (NativeWindow.WndProcShouldBeDebuggable)
				{
					this.OnApply(EventArgs.Empty);
					goto IL_0116;
				}
				try
				{
					this.OnApply(EventArgs.Empty);
					goto IL_0116;
				}
				catch (Exception ex)
				{
					Application.OnThreadException(ex);
					goto IL_0116;
				}
				break;
			}
			default:
				goto IL_0116;
			}
			if (!this.showColor)
			{
				IntPtr intPtr = UnsafeNativeMethods.GetDlgItem(new HandleRef(null, hWnd), 1139);
				SafeNativeMethods.ShowWindow(new HandleRef(null, intPtr), 0);
				intPtr = UnsafeNativeMethods.GetDlgItem(new HandleRef(null, hWnd), 1091);
				SafeNativeMethods.ShowWindow(new HandleRef(null, intPtr), 0);
			}
			IL_0116:
			return base.HookProc(hWnd, msg, wparam, lparam);
		}

		// Token: 0x06003CB4 RID: 15540 RVA: 0x000DA208 File Offset: 0x000D9208
		protected virtual void OnApply(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[FontDialog.EventApply];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06003CB5 RID: 15541 RVA: 0x000DA236 File Offset: 0x000D9236
		public override void Reset()
		{
			this.options = 257;
			this.font = null;
			this.color = Color.Black;
			this.showColor = false;
			this.minSize = 0;
			this.maxSize = 0;
			this.SetOption(262144, true);
		}

		// Token: 0x06003CB6 RID: 15542 RVA: 0x000DA276 File Offset: 0x000D9276
		private void ResetFont()
		{
			this.font = null;
		}

		// Token: 0x06003CB7 RID: 15543 RVA: 0x000DA280 File Offset: 0x000D9280
		protected override bool RunDialog(IntPtr hWndOwner)
		{
			NativeMethods.WndProc wndProc = new NativeMethods.WndProc(this.HookProc);
			NativeMethods.CHOOSEFONT choosefont = new NativeMethods.CHOOSEFONT();
			IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
			NativeMethods.LOGFONT logfont = new NativeMethods.LOGFONT();
			Graphics graphics = Graphics.FromHdcInternal(dc);
			IntSecurity.ObjectFromWin32Handle.Assert();
			try
			{
				this.Font.ToLogFont(logfont, graphics);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
				graphics.Dispose();
			}
			UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
			IntPtr intPtr = IntPtr.Zero;
			bool flag;
			try
			{
				intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(NativeMethods.LOGFONT)));
				Marshal.StructureToPtr(logfont, intPtr, false);
				choosefont.lStructSize = Marshal.SizeOf(typeof(NativeMethods.CHOOSEFONT));
				choosefont.hwndOwner = hWndOwner;
				choosefont.hDC = IntPtr.Zero;
				choosefont.lpLogFont = intPtr;
				choosefont.Flags = this.Options | 64 | 8;
				if (this.minSize > 0 || this.maxSize > 0)
				{
					choosefont.Flags |= 8192;
				}
				if (this.ShowColor || this.ShowEffects)
				{
					choosefont.rgbColors = ColorTranslator.ToWin32(this.color);
				}
				else
				{
					choosefont.rgbColors = ColorTranslator.ToWin32(Color.Black);
				}
				choosefont.lpfnHook = wndProc;
				choosefont.hInstance = UnsafeNativeMethods.GetModuleHandle(null);
				choosefont.nSizeMin = this.minSize;
				if (this.maxSize == 0)
				{
					choosefont.nSizeMax = int.MaxValue;
				}
				else
				{
					choosefont.nSizeMax = this.maxSize;
				}
				if (!SafeNativeMethods.ChooseFont(choosefont))
				{
					flag = false;
				}
				else
				{
					NativeMethods.LOGFONT logfont2 = (NativeMethods.LOGFONT)UnsafeNativeMethods.PtrToStructure(intPtr, typeof(NativeMethods.LOGFONT));
					if (logfont2.lfFaceName != null && logfont2.lfFaceName.Length > 0)
					{
						logfont = logfont2;
						this.UpdateFont(logfont);
						this.UpdateColor(choosefont.rgbColors);
					}
					flag = true;
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(intPtr);
				}
			}
			return flag;
		}

		// Token: 0x06003CB8 RID: 15544 RVA: 0x000DA498 File Offset: 0x000D9498
		internal void SetOption(int option, bool value)
		{
			if (value)
			{
				this.options |= option;
				return;
			}
			this.options &= ~option;
		}

		// Token: 0x06003CB9 RID: 15545 RVA: 0x000DA4BB File Offset: 0x000D94BB
		private bool ShouldSerializeFont()
		{
			return !this.Font.Equals(Control.DefaultFont);
		}

		// Token: 0x06003CBA RID: 15546 RVA: 0x000DA4D0 File Offset: 0x000D94D0
		public override string ToString()
		{
			string text = base.ToString();
			return text + ",  Font: " + this.Font.ToString();
		}

		// Token: 0x06003CBB RID: 15547 RVA: 0x000DA4FA File Offset: 0x000D94FA
		private void UpdateColor(int rgb)
		{
			if (ColorTranslator.ToWin32(this.color) != rgb)
			{
				this.color = ColorTranslator.FromOle(rgb);
			}
		}

		// Token: 0x06003CBC RID: 15548 RVA: 0x000DA518 File Offset: 0x000D9518
		private void UpdateFont(NativeMethods.LOGFONT lf)
		{
			IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
			try
			{
				Font font = null;
				try
				{
					IntSecurity.UnmanagedCode.Assert();
					try
					{
						font = Font.FromLogFont(lf, dc);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					this.font = ControlPaint.FontInPoints(font);
				}
				finally
				{
					if (font != null)
					{
						font.Dispose();
					}
				}
			}
			finally
			{
				UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
			}
		}

		// Token: 0x04001E24 RID: 7716
		private const int defaultMinSize = 0;

		// Token: 0x04001E25 RID: 7717
		private const int defaultMaxSize = 0;

		// Token: 0x04001E26 RID: 7718
		protected static readonly object EventApply = new object();

		// Token: 0x04001E27 RID: 7719
		private int options;

		// Token: 0x04001E28 RID: 7720
		private Font font;

		// Token: 0x04001E29 RID: 7721
		private Color color;

		// Token: 0x04001E2A RID: 7722
		private int minSize;

		// Token: 0x04001E2B RID: 7723
		private int maxSize;

		// Token: 0x04001E2C RID: 7724
		private bool showColor;
	}
}
