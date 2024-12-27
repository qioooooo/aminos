using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.Internal;
using System.Windows.Forms.Layout;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x0200041B RID: 1051
	[DefaultEvent("Enter")]
	[Designer("System.Windows.Forms.Design.GroupBoxDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("Text")]
	[ComVisible(true)]
	[SRDescription("DescriptionGroupBox")]
	public class GroupBox : Control
	{
		// Token: 0x06003E74 RID: 15988 RVA: 0x000E33F0 File Offset: 0x000E23F0
		public GroupBox()
		{
			base.SetState2(2048, true);
			base.SetStyle(ControlStyles.ContainerControl, true);
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, this.OwnerDraw);
			base.SetStyle(ControlStyles.Selectable, false);
			this.TabStop = false;
		}

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x06003E75 RID: 15989 RVA: 0x000E3449 File Offset: 0x000E2449
		// (set) Token: 0x06003E76 RID: 15990 RVA: 0x000E3451 File Offset: 0x000E2451
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public override bool AllowDrop
		{
			get
			{
				return base.AllowDrop;
			}
			set
			{
				base.AllowDrop = value;
			}
		}

		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x06003E77 RID: 15991 RVA: 0x000E345A File Offset: 0x000E245A
		// (set) Token: 0x06003E78 RID: 15992 RVA: 0x000E3462 File Offset: 0x000E2462
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
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

		// Token: 0x14000211 RID: 529
		// (add) Token: 0x06003E79 RID: 15993 RVA: 0x000E346B File Offset: 0x000E246B
		// (remove) Token: 0x06003E7A RID: 15994 RVA: 0x000E3474 File Offset: 0x000E2474
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

		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x06003E7B RID: 15995 RVA: 0x000E347D File Offset: 0x000E247D
		// (set) Token: 0x06003E7C RID: 15996 RVA: 0x000E3488 File Offset: 0x000E2488
		[SRCategory("CatLayout")]
		[SRDescription("ControlAutoSizeModeDescr")]
		[Browsable(true)]
		[DefaultValue(AutoSizeMode.GrowOnly)]
		[Localizable(true)]
		public AutoSizeMode AutoSizeMode
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

		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x06003E7D RID: 15997 RVA: 0x000E350C File Offset: 0x000E250C
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				if (!this.OwnerDraw)
				{
					createParams.ClassName = "BUTTON";
					createParams.Style |= 7;
				}
				else
				{
					createParams.ClassName = null;
					createParams.Style &= -8;
				}
				createParams.ExStyle |= 65536;
				return createParams;
			}
		}

		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x06003E7E RID: 15998 RVA: 0x000E356C File Offset: 0x000E256C
		protected override Padding DefaultPadding
		{
			get
			{
				return new Padding(3);
			}
		}

		// Token: 0x17000C02 RID: 3074
		// (get) Token: 0x06003E7F RID: 15999 RVA: 0x000E3574 File Offset: 0x000E2574
		protected override Size DefaultSize
		{
			get
			{
				return new Size(200, 100);
			}
		}

		// Token: 0x17000C03 RID: 3075
		// (get) Token: 0x06003E80 RID: 16000 RVA: 0x000E3584 File Offset: 0x000E2584
		public override Rectangle DisplayRectangle
		{
			get
			{
				Size clientSize = base.ClientSize;
				if (this.fontHeight == -1)
				{
					this.fontHeight = this.Font.Height;
					this.cachedFont = this.Font;
				}
				else if (!object.ReferenceEquals(this.cachedFont, this.Font))
				{
					this.fontHeight = this.Font.Height;
					this.cachedFont = this.Font;
				}
				Padding padding = base.Padding;
				return new Rectangle(padding.Left, this.fontHeight + padding.Top, Math.Max(clientSize.Width - padding.Horizontal, 0), Math.Max(clientSize.Height - this.fontHeight - padding.Vertical, 0));
			}
		}

		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x06003E81 RID: 16001 RVA: 0x000E3642 File Offset: 0x000E2642
		// (set) Token: 0x06003E82 RID: 16002 RVA: 0x000E364C File Offset: 0x000E264C
		[SRCategory("CatAppearance")]
		[DefaultValue(FlatStyle.Standard)]
		[SRDescription("ButtonFlatStyleDescr")]
		public FlatStyle FlatStyle
		{
			get
			{
				return this.flatStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(FlatStyle));
				}
				if (this.flatStyle != value)
				{
					bool ownerDraw = this.OwnerDraw;
					this.flatStyle = value;
					bool flag = this.OwnerDraw != ownerDraw;
					base.SetStyle(ControlStyles.ContainerControl, true);
					base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.UserMouse | ControlStyles.SupportsTransparentBackColor, this.OwnerDraw);
					if (flag)
					{
						base.RecreateHandle();
						return;
					}
					this.Refresh();
				}
			}
		}

		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x06003E83 RID: 16003 RVA: 0x000E36CC File Offset: 0x000E26CC
		private bool OwnerDraw
		{
			get
			{
				return this.FlatStyle != FlatStyle.System;
			}
		}

		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x06003E84 RID: 16004 RVA: 0x000E36DA File Offset: 0x000E26DA
		// (set) Token: 0x06003E85 RID: 16005 RVA: 0x000E36E2 File Offset: 0x000E26E2
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
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

		// Token: 0x14000212 RID: 530
		// (add) Token: 0x06003E86 RID: 16006 RVA: 0x000E36EB File Offset: 0x000E26EB
		// (remove) Token: 0x06003E87 RID: 16007 RVA: 0x000E36F4 File Offset: 0x000E26F4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public new event EventHandler TabStopChanged
		{
			add
			{
				base.TabStopChanged += value;
			}
			remove
			{
				base.TabStopChanged -= value;
			}
		}

		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x06003E88 RID: 16008 RVA: 0x000E36FD File Offset: 0x000E26FD
		// (set) Token: 0x06003E89 RID: 16009 RVA: 0x000E3708 File Offset: 0x000E2708
		[Localizable(true)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				bool visible = base.Visible;
				try
				{
					if (visible && base.IsHandleCreated)
					{
						base.SendMessage(11, 0, 0);
					}
					base.Text = value;
				}
				finally
				{
					if (visible && base.IsHandleCreated)
					{
						base.SendMessage(11, 1, 0);
					}
				}
				base.Invalidate(true);
			}
		}

		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x06003E8A RID: 16010 RVA: 0x000E376C File Offset: 0x000E276C
		// (set) Token: 0x06003E8B RID: 16011 RVA: 0x000E3774 File Offset: 0x000E2774
		[SRDescription("UseCompatibleTextRenderingDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		public bool UseCompatibleTextRendering
		{
			get
			{
				return base.UseCompatibleTextRenderingInt;
			}
			set
			{
				base.UseCompatibleTextRenderingInt = value;
			}
		}

		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x06003E8C RID: 16012 RVA: 0x000E377D File Offset: 0x000E277D
		internal override bool SupportsUseCompatibleTextRendering
		{
			get
			{
				return true;
			}
		}

		// Token: 0x14000213 RID: 531
		// (add) Token: 0x06003E8D RID: 16013 RVA: 0x000E3780 File Offset: 0x000E2780
		// (remove) Token: 0x06003E8E RID: 16014 RVA: 0x000E3789 File Offset: 0x000E2789
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
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

		// Token: 0x14000214 RID: 532
		// (add) Token: 0x06003E8F RID: 16015 RVA: 0x000E3792 File Offset: 0x000E2792
		// (remove) Token: 0x06003E90 RID: 16016 RVA: 0x000E379B File Offset: 0x000E279B
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
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

		// Token: 0x14000215 RID: 533
		// (add) Token: 0x06003E91 RID: 16017 RVA: 0x000E37A4 File Offset: 0x000E27A4
		// (remove) Token: 0x06003E92 RID: 16018 RVA: 0x000E37AD File Offset: 0x000E27AD
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public new event EventHandler DoubleClick
		{
			add
			{
				base.DoubleClick += value;
			}
			remove
			{
				base.DoubleClick -= value;
			}
		}

		// Token: 0x14000216 RID: 534
		// (add) Token: 0x06003E93 RID: 16019 RVA: 0x000E37B6 File Offset: 0x000E27B6
		// (remove) Token: 0x06003E94 RID: 16020 RVA: 0x000E37BF File Offset: 0x000E27BF
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public new event MouseEventHandler MouseDoubleClick
		{
			add
			{
				base.MouseDoubleClick += value;
			}
			remove
			{
				base.MouseDoubleClick -= value;
			}
		}

		// Token: 0x14000217 RID: 535
		// (add) Token: 0x06003E95 RID: 16021 RVA: 0x000E37C8 File Offset: 0x000E27C8
		// (remove) Token: 0x06003E96 RID: 16022 RVA: 0x000E37D1 File Offset: 0x000E27D1
		[EditorBrowsable(EditorBrowsableState.Advanced)]
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

		// Token: 0x14000218 RID: 536
		// (add) Token: 0x06003E97 RID: 16023 RVA: 0x000E37DA File Offset: 0x000E27DA
		// (remove) Token: 0x06003E98 RID: 16024 RVA: 0x000E37E3 File Offset: 0x000E27E3
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
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

		// Token: 0x14000219 RID: 537
		// (add) Token: 0x06003E99 RID: 16025 RVA: 0x000E37EC File Offset: 0x000E27EC
		// (remove) Token: 0x06003E9A RID: 16026 RVA: 0x000E37F5 File Offset: 0x000E27F5
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
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

		// Token: 0x1400021A RID: 538
		// (add) Token: 0x06003E9B RID: 16027 RVA: 0x000E37FE File Offset: 0x000E27FE
		// (remove) Token: 0x06003E9C RID: 16028 RVA: 0x000E3807 File Offset: 0x000E2807
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public new event MouseEventHandler MouseDown
		{
			add
			{
				base.MouseDown += value;
			}
			remove
			{
				base.MouseDown -= value;
			}
		}

		// Token: 0x1400021B RID: 539
		// (add) Token: 0x06003E9D RID: 16029 RVA: 0x000E3810 File Offset: 0x000E2810
		// (remove) Token: 0x06003E9E RID: 16030 RVA: 0x000E3819 File Offset: 0x000E2819
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public new event MouseEventHandler MouseUp
		{
			add
			{
				base.MouseUp += value;
			}
			remove
			{
				base.MouseUp -= value;
			}
		}

		// Token: 0x1400021C RID: 540
		// (add) Token: 0x06003E9F RID: 16031 RVA: 0x000E3822 File Offset: 0x000E2822
		// (remove) Token: 0x06003EA0 RID: 16032 RVA: 0x000E382B File Offset: 0x000E282B
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public new event MouseEventHandler MouseMove
		{
			add
			{
				base.MouseMove += value;
			}
			remove
			{
				base.MouseMove -= value;
			}
		}

		// Token: 0x1400021D RID: 541
		// (add) Token: 0x06003EA1 RID: 16033 RVA: 0x000E3834 File Offset: 0x000E2834
		// (remove) Token: 0x06003EA2 RID: 16034 RVA: 0x000E383D File Offset: 0x000E283D
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public new event EventHandler MouseEnter
		{
			add
			{
				base.MouseEnter += value;
			}
			remove
			{
				base.MouseEnter -= value;
			}
		}

		// Token: 0x1400021E RID: 542
		// (add) Token: 0x06003EA3 RID: 16035 RVA: 0x000E3846 File Offset: 0x000E2846
		// (remove) Token: 0x06003EA4 RID: 16036 RVA: 0x000E384F File Offset: 0x000E284F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public new event EventHandler MouseLeave
		{
			add
			{
				base.MouseLeave += value;
			}
			remove
			{
				base.MouseLeave -= value;
			}
		}

		// Token: 0x06003EA5 RID: 16037 RVA: 0x000E3858 File Offset: 0x000E2858
		protected override void OnPaint(PaintEventArgs e)
		{
			if (Application.RenderWithVisualStyles && base.Width >= 10 && base.Height >= 10)
			{
				GroupBoxState groupBoxState = (base.Enabled ? GroupBoxState.Normal : GroupBoxState.Disabled);
				TextFormatFlags textFormatFlags = TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak | TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform;
				if (!this.ShowKeyboardCues)
				{
					textFormatFlags |= TextFormatFlags.HidePrefix;
				}
				if (this.RightToLeft == RightToLeft.Yes)
				{
					textFormatFlags |= TextFormatFlags.Right | TextFormatFlags.RightToLeft;
				}
				if (this.ShouldSerializeForeColor() || !base.Enabled)
				{
					Color color = (base.Enabled ? this.ForeColor : TextRenderer.DisabledTextColor(this.BackColor));
					GroupBoxRenderer.DrawGroupBox(e.Graphics, new Rectangle(0, 0, base.Width, base.Height), this.Text, this.Font, color, textFormatFlags, groupBoxState);
				}
				else
				{
					GroupBoxRenderer.DrawGroupBox(e.Graphics, new Rectangle(0, 0, base.Width, base.Height), this.Text, this.Font, textFormatFlags, groupBoxState);
				}
			}
			else
			{
				this.DrawGroupBox(e);
			}
			base.OnPaint(e);
		}

		// Token: 0x06003EA6 RID: 16038 RVA: 0x000E3954 File Offset: 0x000E2954
		private void DrawGroupBox(PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;
			Rectangle clientRectangle = base.ClientRectangle;
			int num = 8;
			Color disabledColor = base.DisabledColor;
			Pen pen = new Pen(ControlPaint.Light(disabledColor, 1f));
			Pen pen2 = new Pen(ControlPaint.Dark(disabledColor, 0f));
			clientRectangle.X += num;
			clientRectangle.Width -= 2 * num;
			try
			{
				Size size;
				if (this.UseCompatibleTextRendering)
				{
					using (Brush brush = new SolidBrush(this.ForeColor))
					{
						using (StringFormat stringFormat = new StringFormat())
						{
							stringFormat.HotkeyPrefix = (this.ShowKeyboardCues ? HotkeyPrefix.Show : HotkeyPrefix.Hide);
							if (this.RightToLeft == RightToLeft.Yes)
							{
								stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
							}
							size = Size.Ceiling(graphics.MeasureString(this.Text, this.Font, clientRectangle.Width, stringFormat));
							if (base.Enabled)
							{
								graphics.DrawString(this.Text, this.Font, brush, clientRectangle, stringFormat);
							}
							else
							{
								ControlPaint.DrawStringDisabled(graphics, this.Text, this.Font, disabledColor, clientRectangle, stringFormat);
							}
						}
						goto IL_01E7;
					}
				}
				using (WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(graphics))
				{
					IntTextFormatFlags intTextFormatFlags = IntTextFormatFlags.TextBoxControl | IntTextFormatFlags.WordBreak;
					if (!this.ShowKeyboardCues)
					{
						intTextFormatFlags |= IntTextFormatFlags.HidePrefix;
					}
					if (this.RightToLeft == RightToLeft.Yes)
					{
						intTextFormatFlags |= IntTextFormatFlags.RightToLeft;
						intTextFormatFlags |= IntTextFormatFlags.Right;
					}
					using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(this.Font))
					{
						size = windowsGraphics.MeasureText(this.Text, windowsFont, new Size(clientRectangle.Width, int.MaxValue), intTextFormatFlags);
						if (base.Enabled)
						{
							windowsGraphics.DrawText(this.Text, windowsFont, clientRectangle, this.ForeColor, intTextFormatFlags);
						}
						else
						{
							ControlPaint.DrawStringDisabled(windowsGraphics, this.Text, this.Font, disabledColor, clientRectangle, (TextFormatFlags)intTextFormatFlags);
						}
					}
				}
				IL_01E7:
				int num2 = num;
				if (this.RightToLeft == RightToLeft.Yes)
				{
					num2 += clientRectangle.Width - size.Width;
				}
				int num3 = Math.Min(num2 + size.Width, base.Width - 6);
				int num4 = base.FontHeight / 2;
				graphics.DrawLine(pen, 1, num4, 1, base.Height - 1);
				graphics.DrawLine(pen2, 0, num4, 0, base.Height - 2);
				graphics.DrawLine(pen, 0, base.Height - 1, base.Width, base.Height - 1);
				graphics.DrawLine(pen2, 0, base.Height - 2, base.Width - 1, base.Height - 2);
				graphics.DrawLine(pen2, 0, num4 - 1, num2, num4 - 1);
				graphics.DrawLine(pen, 1, num4, num2, num4);
				graphics.DrawLine(pen2, num3, num4 - 1, base.Width - 2, num4 - 1);
				graphics.DrawLine(pen, num3, num4, base.Width - 1, num4);
				graphics.DrawLine(pen, base.Width - 1, num4 - 1, base.Width - 1, base.Height - 1);
				graphics.DrawLine(pen2, base.Width - 2, num4, base.Width - 2, base.Height - 2);
			}
			finally
			{
				pen.Dispose();
				pen2.Dispose();
			}
		}

		// Token: 0x06003EA7 RID: 16039 RVA: 0x000E3D18 File Offset: 0x000E2D18
		internal override Size GetPreferredSizeCore(Size proposedSize)
		{
			Size size = this.SizeFromClientSize(Size.Empty);
			Size size2 = size + new Size(0, this.fontHeight) + base.Padding.Size;
			Size preferredSize = this.LayoutEngine.GetPreferredSize(this, proposedSize - size2);
			return preferredSize + size2;
		}

		// Token: 0x06003EA8 RID: 16040 RVA: 0x000E3D72 File Offset: 0x000E2D72
		protected override void OnFontChanged(EventArgs e)
		{
			this.fontHeight = -1;
			this.cachedFont = null;
			base.Invalidate();
			base.OnFontChanged(e);
		}

		// Token: 0x06003EA9 RID: 16041 RVA: 0x000E3D90 File Offset: 0x000E2D90
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			if (Control.IsMnemonic(charCode, this.Text) && this.CanProcessMnemonic())
			{
				IntSecurity.ModifyFocus.Assert();
				try
				{
					base.SelectNextControl(null, true, true, true, false);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				return true;
			}
			return false;
		}

		// Token: 0x06003EAA RID: 16042 RVA: 0x000E3DE4 File Offset: 0x000E2DE4
		protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			if (factor.Width != 1f && factor.Height != 1f)
			{
				this.fontHeight = -1;
				this.cachedFont = null;
			}
			base.ScaleControl(factor, specified);
		}

		// Token: 0x06003EAB RID: 16043 RVA: 0x000E3E18 File Offset: 0x000E2E18
		public override string ToString()
		{
			string text = base.ToString();
			return text + ", Text: " + this.Text;
		}

		// Token: 0x06003EAC RID: 16044 RVA: 0x000E3E40 File Offset: 0x000E2E40
		private void WmEraseBkgnd(ref Message m)
		{
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			SafeNativeMethods.GetClientRect(new HandleRef(this, base.Handle), ref rect);
			using (Graphics graphics = Graphics.FromHdcInternal(m.WParam))
			{
				using (Brush brush = new SolidBrush(this.BackColor))
				{
					graphics.FillRectangle(brush, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
				}
			}
			m.Result = (IntPtr)1;
		}

		// Token: 0x06003EAD RID: 16045 RVA: 0x000E3EF4 File Offset: 0x000E2EF4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			if (this.OwnerDraw)
			{
				base.WndProc(ref m);
				return;
			}
			int msg = m.Msg;
			if (msg != 20)
			{
				if (msg != 61)
				{
					if (msg == 792)
					{
						goto IL_0029;
					}
					base.WndProc(ref m);
				}
				else
				{
					base.WndProc(ref m);
					if ((int)(long)m.LParam == -12)
					{
						m.Result = IntPtr.Zero;
						return;
					}
				}
				return;
			}
			IL_0029:
			this.WmEraseBkgnd(ref m);
		}

		// Token: 0x06003EAE RID: 16046 RVA: 0x000E3F5C File Offset: 0x000E2F5C
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new GroupBox.GroupBoxAccessibleObject(this);
		}

		// Token: 0x04001ECA RID: 7882
		private int fontHeight = -1;

		// Token: 0x04001ECB RID: 7883
		private Font cachedFont;

		// Token: 0x04001ECC RID: 7884
		private FlatStyle flatStyle = FlatStyle.Standard;

		// Token: 0x0200041C RID: 1052
		[ComVisible(true)]
		internal class GroupBoxAccessibleObject : Control.ControlAccessibleObject
		{
			// Token: 0x06003EAF RID: 16047 RVA: 0x000E3F64 File Offset: 0x000E2F64
			internal GroupBoxAccessibleObject(GroupBox owner)
				: base(owner)
			{
			}

			// Token: 0x17000C0A RID: 3082
			// (get) Token: 0x06003EB0 RID: 16048 RVA: 0x000E3F70 File Offset: 0x000E2F70
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.Grouping;
				}
			}
		}
	}
}
