using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x0200020C RID: 524
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Designer("System.Windows.Forms.Design.ScrollableControlDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ComVisible(true)]
	public class ScrollableControl : Control, IArrangedElement, IComponent, IDisposable
	{
		// Token: 0x060017DC RID: 6108 RVA: 0x00028230 File Offset: 0x00027230
		public ScrollableControl()
		{
			base.SetStyle(ControlStyles.ContainerControl, true);
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, false);
			this.SetScrollState(1, false);
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x060017DD RID: 6109 RVA: 0x00028296 File Offset: 0x00027296
		// (set) Token: 0x060017DE RID: 6110 RVA: 0x0002829F File Offset: 0x0002729F
		[SRCategory("CatLayout")]
		[SRDescription("FormAutoScrollDescr")]
		[DefaultValue(false)]
		[Localizable(true)]
		public virtual bool AutoScroll
		{
			get
			{
				return this.GetScrollState(1);
			}
			set
			{
				if (value)
				{
					this.UpdateFullDrag();
				}
				this.SetScrollState(1, value);
				LayoutTransaction.DoLayout(this, this, PropertyNames.AutoScroll);
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x060017DF RID: 6111 RVA: 0x000282BE File Offset: 0x000272BE
		// (set) Token: 0x060017E0 RID: 6112 RVA: 0x000282C8 File Offset: 0x000272C8
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[SRDescription("FormAutoScrollMarginDescr")]
		public Size AutoScrollMargin
		{
			get
			{
				return this.requestedScrollMargin;
			}
			set
			{
				if (value.Width < 0 || value.Height < 0)
				{
					throw new ArgumentOutOfRangeException("AutoScrollMargin", SR.GetString("InvalidArgument", new object[]
					{
						"AutoScrollMargin",
						value.ToString()
					}));
				}
				this.SetAutoScrollMargin(value.Width, value.Height);
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x060017E1 RID: 6113 RVA: 0x00028334 File Offset: 0x00027334
		// (set) Token: 0x060017E2 RID: 6114 RVA: 0x0002835B File Offset: 0x0002735B
		[SRDescription("FormAutoScrollPositionDescr")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatLayout")]
		public Point AutoScrollPosition
		{
			get
			{
				Rectangle displayRectInternal = this.GetDisplayRectInternal();
				return new Point(displayRectInternal.X, displayRectInternal.Y);
			}
			set
			{
				if (base.Created)
				{
					this.SetDisplayRectLocation(-value.X, -value.Y);
					this.SyncScrollbars(true);
				}
				this.scrollPosition = value;
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x060017E3 RID: 6115 RVA: 0x00028389 File Offset: 0x00027389
		// (set) Token: 0x060017E4 RID: 6116 RVA: 0x00028391 File Offset: 0x00027391
		[SRDescription("FormAutoScrollMinSizeDescr")]
		[Localizable(true)]
		[SRCategory("CatLayout")]
		public Size AutoScrollMinSize
		{
			get
			{
				return this.userAutoScrollMinSize;
			}
			set
			{
				if (value != this.userAutoScrollMinSize)
				{
					this.userAutoScrollMinSize = value;
					this.AutoScroll = true;
					base.PerformLayout();
				}
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x060017E5 RID: 6117 RVA: 0x000283B8 File Offset: 0x000273B8
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				if (this.HScroll || this.HorizontalScroll.Visible)
				{
					createParams.Style |= 1048576;
				}
				else
				{
					createParams.Style &= -1048577;
				}
				if (this.VScroll || this.VerticalScroll.Visible)
				{
					createParams.Style |= 2097152;
				}
				else
				{
					createParams.Style &= -2097153;
				}
				return createParams;
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x060017E6 RID: 6118 RVA: 0x00028444 File Offset: 0x00027444
		public override Rectangle DisplayRectangle
		{
			get
			{
				Rectangle clientRectangle = base.ClientRectangle;
				if (!this.displayRect.IsEmpty)
				{
					clientRectangle.X = this.displayRect.X;
					clientRectangle.Y = this.displayRect.Y;
					if (this.HScroll)
					{
						clientRectangle.Width = this.displayRect.Width;
					}
					if (this.VScroll)
					{
						clientRectangle.Height = this.displayRect.Height;
					}
				}
				return LayoutUtils.DeflateRect(clientRectangle, base.Padding);
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x060017E7 RID: 6119 RVA: 0x000284CC File Offset: 0x000274CC
		Rectangle IArrangedElement.DisplayRectangle
		{
			get
			{
				Rectangle displayRectangle = this.DisplayRectangle;
				if (this.AutoScrollMinSize.Width != 0 && this.AutoScrollMinSize.Height != 0)
				{
					displayRectangle.Width = Math.Max(displayRectangle.Width, this.AutoScrollMinSize.Width);
					displayRectangle.Height = Math.Max(displayRectangle.Height, this.AutoScrollMinSize.Height);
				}
				return displayRectangle;
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x060017E8 RID: 6120 RVA: 0x00028544 File Offset: 0x00027544
		// (set) Token: 0x060017E9 RID: 6121 RVA: 0x0002854D File Offset: 0x0002754D
		protected bool HScroll
		{
			get
			{
				return this.GetScrollState(2);
			}
			set
			{
				this.SetScrollState(2, value);
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x060017EA RID: 6122 RVA: 0x00028557 File Offset: 0x00027557
		[SRCategory("CatLayout")]
		[SRDescription("ScrollableControlHorizontalScrollDescr")]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(false)]
		public HScrollProperties HorizontalScroll
		{
			get
			{
				if (this.horizontalScroll == null)
				{
					this.horizontalScroll = new HScrollProperties(this);
				}
				return this.horizontalScroll;
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x060017EB RID: 6123 RVA: 0x00028573 File Offset: 0x00027573
		// (set) Token: 0x060017EC RID: 6124 RVA: 0x0002857C File Offset: 0x0002757C
		protected bool VScroll
		{
			get
			{
				return this.GetScrollState(4);
			}
			set
			{
				this.SetScrollState(4, value);
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x060017ED RID: 6125 RVA: 0x00028586 File Offset: 0x00027586
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(false)]
		[SRCategory("CatLayout")]
		[SRDescription("ScrollableControlVerticalScrollDescr")]
		public VScrollProperties VerticalScroll
		{
			get
			{
				if (this.verticalScroll == null)
				{
					this.verticalScroll = new VScrollProperties(this);
				}
				return this.verticalScroll;
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x060017EE RID: 6126 RVA: 0x000285A2 File Offset: 0x000275A2
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ScrollableControl.DockPaddingEdges DockPadding
		{
			get
			{
				if (this.dockPadding == null)
				{
					this.dockPadding = new ScrollableControl.DockPaddingEdges(this);
				}
				return this.dockPadding;
			}
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x000285C0 File Offset: 0x000275C0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void AdjustFormScrollbars(bool displayScrollbars)
		{
			bool flag = false;
			Rectangle displayRectInternal = this.GetDisplayRectInternal();
			if (!displayScrollbars && (this.HScroll || this.VScroll))
			{
				flag = this.SetVisibleScrollbars(false, false);
			}
			if (!displayScrollbars)
			{
				Rectangle clientRectangle = base.ClientRectangle;
				displayRectInternal.Width = clientRectangle.Width;
				displayRectInternal.Height = clientRectangle.Height;
			}
			else
			{
				flag |= this.ApplyScrollbarChanges(displayRectInternal);
			}
			if (flag)
			{
				LayoutTransaction.DoLayout(this, this, PropertyNames.DisplayRectangle);
			}
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x00028634 File Offset: 0x00027634
		private bool ApplyScrollbarChanges(Rectangle display)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			Rectangle clientRectangle = base.ClientRectangle;
			Rectangle rectangle = clientRectangle;
			Rectangle rectangle2 = rectangle;
			if (this.HScroll)
			{
				rectangle.Height += SystemInformation.HorizontalScrollBarHeight;
			}
			else
			{
				rectangle2.Height -= SystemInformation.HorizontalScrollBarHeight;
			}
			if (this.VScroll)
			{
				rectangle.Width += SystemInformation.VerticalScrollBarWidth;
			}
			else
			{
				rectangle2.Width -= SystemInformation.VerticalScrollBarWidth;
			}
			int num = rectangle2.Width;
			int num2 = rectangle2.Height;
			if (base.Controls.Count != 0)
			{
				this.scrollMargin = this.requestedScrollMargin;
				if (this.dockPadding != null)
				{
					this.scrollMargin.Height = this.scrollMargin.Height + base.Padding.Bottom;
					this.scrollMargin.Width = this.scrollMargin.Width + base.Padding.Right;
				}
				for (int i = 0; i < base.Controls.Count; i++)
				{
					Control control = base.Controls[i];
					if (control != null && control.GetState(2))
					{
						switch (control.Dock)
						{
						case DockStyle.Bottom:
							this.scrollMargin.Height = this.scrollMargin.Height + control.Size.Height;
							break;
						case DockStyle.Right:
							this.scrollMargin.Width = this.scrollMargin.Width + control.Size.Width;
							break;
						}
					}
				}
			}
			if (!this.userAutoScrollMinSize.IsEmpty)
			{
				num = this.userAutoScrollMinSize.Width + this.scrollMargin.Width;
				num2 = this.userAutoScrollMinSize.Height + this.scrollMargin.Height;
				flag2 = true;
				flag3 = true;
			}
			bool flag4 = this.LayoutEngine == DefaultLayout.Instance;
			if (!flag4 && CommonProperties.HasLayoutBounds(this))
			{
				Size layoutBounds = CommonProperties.GetLayoutBounds(this);
				if (layoutBounds.Width > num)
				{
					flag2 = true;
					num = layoutBounds.Width;
				}
				if (layoutBounds.Height > num2)
				{
					flag3 = true;
					num2 = layoutBounds.Height;
				}
			}
			else if (base.Controls.Count != 0)
			{
				for (int j = 0; j < base.Controls.Count; j++)
				{
					bool flag5 = true;
					bool flag6 = true;
					Control control2 = base.Controls[j];
					if (control2 != null && control2.GetState(2))
					{
						if (flag4)
						{
							Control control3 = control2;
							switch (control3.Dock)
							{
							case DockStyle.Top:
								flag5 = false;
								break;
							case DockStyle.Bottom:
							case DockStyle.Right:
							case DockStyle.Fill:
								flag5 = false;
								flag6 = false;
								break;
							case DockStyle.Left:
								flag6 = false;
								break;
							default:
							{
								AnchorStyles anchor = control3.Anchor;
								if ((anchor & AnchorStyles.Right) == AnchorStyles.Right)
								{
									flag5 = false;
								}
								if ((anchor & AnchorStyles.Left) != AnchorStyles.Left)
								{
									flag5 = false;
								}
								if ((anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom)
								{
									flag6 = false;
								}
								if ((anchor & AnchorStyles.Top) != AnchorStyles.Top)
								{
									flag6 = false;
								}
								break;
							}
							}
						}
						if (flag5 || flag6)
						{
							Rectangle bounds = control2.Bounds;
							int num3 = -display.X + bounds.X + bounds.Width + this.scrollMargin.Width;
							int num4 = -display.Y + bounds.Y + bounds.Height + this.scrollMargin.Height;
							if (!flag4)
							{
								num3 += control2.Margin.Right;
								num4 += control2.Margin.Bottom;
							}
							if (num3 > num && flag5)
							{
								flag2 = true;
								num = num3;
							}
							if (num4 > num2 && flag6)
							{
								flag3 = true;
								num2 = num4;
							}
						}
					}
				}
			}
			if (num <= rectangle.Width)
			{
				flag2 = false;
			}
			if (num2 <= rectangle.Height)
			{
				flag3 = false;
			}
			Rectangle rectangle3 = rectangle;
			if (flag2)
			{
				rectangle3.Height -= SystemInformation.HorizontalScrollBarHeight;
			}
			if (flag3)
			{
				rectangle3.Width -= SystemInformation.VerticalScrollBarWidth;
			}
			if (flag2 && num2 > rectangle3.Height)
			{
				flag3 = true;
			}
			if (flag3 && num > rectangle3.Width)
			{
				flag2 = true;
			}
			if (!flag2)
			{
				num = rectangle3.Width;
			}
			if (!flag3)
			{
				num2 = rectangle3.Height;
			}
			flag = this.SetVisibleScrollbars(flag2, flag3) || flag;
			if (this.HScroll || this.VScroll)
			{
				flag = this.SetDisplayRectangleSize(num, num2) || flag;
			}
			else
			{
				this.SetDisplayRectangleSize(num, num2);
			}
			this.SyncScrollbars(true);
			return flag;
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x00028AAC File Offset: 0x00027AAC
		private Rectangle GetDisplayRectInternal()
		{
			if (this.displayRect.IsEmpty)
			{
				this.displayRect = base.ClientRectangle;
			}
			if (!this.AutoScroll && this.HorizontalScroll.visible)
			{
				this.displayRect = new Rectangle(this.displayRect.X, this.displayRect.Y, this.HorizontalScroll.Maximum, this.displayRect.Height);
			}
			if (!this.AutoScroll && this.VerticalScroll.visible)
			{
				this.displayRect = new Rectangle(this.displayRect.X, this.displayRect.Y, this.displayRect.Width, this.VerticalScroll.Maximum);
			}
			return this.displayRect;
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x00028B70 File Offset: 0x00027B70
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected bool GetScrollState(int bit)
		{
			return (bit & this.scrollState) == bit;
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x00028B7D File Offset: 0x00027B7D
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnLayout(LayoutEventArgs levent)
		{
			if (levent.AffectedControl != null && this.AutoScroll)
			{
				base.OnLayout(levent);
			}
			this.AdjustFormScrollbars(this.AutoScroll);
			base.OnLayout(levent);
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x00028BAC File Offset: 0x00027BAC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if (this.VScroll)
			{
				Rectangle clientRectangle = base.ClientRectangle;
				int num = -this.displayRect.Y;
				int num2 = -(clientRectangle.Height - this.displayRect.Height);
				num = Math.Max(num - e.Delta, 0);
				num = Math.Min(num, num2);
				this.SetDisplayRectLocation(this.displayRect.X, -num);
				this.SyncScrollbars(this.AutoScroll);
				if (e is HandledMouseEventArgs)
				{
					((HandledMouseEventArgs)e).Handled = true;
				}
			}
			else if (this.HScroll)
			{
				Rectangle clientRectangle2 = base.ClientRectangle;
				int num3 = -this.displayRect.X;
				int num4 = -(clientRectangle2.Width - this.displayRect.Width);
				num3 = Math.Max(num3 - e.Delta, 0);
				num3 = Math.Min(num3, num4);
				this.SetDisplayRectLocation(-num3, this.displayRect.Y);
				this.SyncScrollbars(this.AutoScroll);
				if (e is HandledMouseEventArgs)
				{
					((HandledMouseEventArgs)e).Handled = true;
				}
			}
			base.OnMouseWheel(e);
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x00028CC6 File Offset: 0x00027CC6
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnRightToLeftChanged(EventArgs e)
		{
			base.OnRightToLeftChanged(e);
			this.resetRTLHScrollValue = true;
			LayoutTransaction.DoLayout(this, this, PropertyNames.RightToLeft);
		}

		// Token: 0x060017F6 RID: 6134 RVA: 0x00028CE4 File Offset: 0x00027CE4
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			if ((this.HScroll || this.VScroll) && this.BackgroundImage != null && (this.BackgroundImageLayout == ImageLayout.Zoom || this.BackgroundImageLayout == ImageLayout.Stretch || this.BackgroundImageLayout == ImageLayout.Center))
			{
				if (ControlPaint.IsImageTransparent(this.BackgroundImage))
				{
					base.PaintTransparentBackground(e, this.displayRect);
				}
				ControlPaint.DrawBackgroundImage(e.Graphics, this.BackgroundImage, this.BackColor, this.BackgroundImageLayout, this.displayRect, this.displayRect, this.displayRect.Location);
				return;
			}
			base.OnPaintBackground(e);
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x00028D7C File Offset: 0x00027D7C
		protected override void OnPaddingChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventPaddingChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060017F8 RID: 6136 RVA: 0x00028DAA File Offset: 0x00027DAA
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnVisibleChanged(EventArgs e)
		{
			if (base.Visible)
			{
				LayoutTransaction.DoLayout(this, this, PropertyNames.Visible);
			}
			base.OnVisibleChanged(e);
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x00028DC7 File Offset: 0x00027DC7
		internal void ScaleDockPadding(float dx, float dy)
		{
			if (this.dockPadding != null)
			{
				this.dockPadding.Scale(dx, dy);
			}
		}

		// Token: 0x060017FA RID: 6138 RVA: 0x00028DDE File Offset: 0x00027DDE
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void ScaleCore(float dx, float dy)
		{
			this.ScaleDockPadding(dx, dy);
			base.ScaleCore(dx, dy);
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x00028DF0 File Offset: 0x00027DF0
		protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			this.ScaleDockPadding(factor.Width, factor.Height);
			base.ScaleControl(factor, specified);
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x00028E10 File Offset: 0x00027E10
		internal void SetDisplayFromScrollProps(int x, int y)
		{
			Rectangle displayRectInternal = this.GetDisplayRectInternal();
			this.ApplyScrollbarChanges(displayRectInternal);
			this.SetDisplayRectLocation(x, y);
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x00028E34 File Offset: 0x00027E34
		protected void SetDisplayRectLocation(int x, int y)
		{
			int num = 0;
			int num2 = 0;
			Rectangle clientRectangle = base.ClientRectangle;
			Rectangle rectangle = this.displayRect;
			int num3 = Math.Min(clientRectangle.Width - rectangle.Width, 0);
			int num4 = Math.Min(clientRectangle.Height - rectangle.Height, 0);
			if (x > 0)
			{
				x = 0;
			}
			if (y > 0)
			{
				y = 0;
			}
			if (x < num3)
			{
				x = num3;
			}
			if (y < num4)
			{
				y = num4;
			}
			if (rectangle.X != x)
			{
				num = x - rectangle.X;
			}
			if (rectangle.Y != y)
			{
				num2 = y - rectangle.Y;
			}
			this.displayRect.X = x;
			this.displayRect.Y = y;
			if (num != 0 || (num2 != 0 && base.IsHandleCreated))
			{
				Rectangle clientRectangle2 = base.ClientRectangle;
				NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(clientRectangle2.X, clientRectangle2.Y, clientRectangle2.Width, clientRectangle2.Height);
				NativeMethods.RECT rect2 = NativeMethods.RECT.FromXYWH(clientRectangle2.X, clientRectangle2.Y, clientRectangle2.Width, clientRectangle2.Height);
				SafeNativeMethods.ScrollWindowEx(new HandleRef(this, base.Handle), num, num2, null, ref rect, NativeMethods.NullHandleRef, ref rect2, 7);
			}
			for (int i = 0; i < base.Controls.Count; i++)
			{
				Control control = base.Controls[i];
				if (control != null && control.IsHandleCreated)
				{
					control.UpdateBounds();
				}
			}
		}

		// Token: 0x060017FE RID: 6142 RVA: 0x00028F9C File Offset: 0x00027F9C
		public void ScrollControlIntoView(Control activeControl)
		{
			Rectangle clientRectangle = base.ClientRectangle;
			if (base.IsDescendant(activeControl) && this.AutoScroll && (this.HScroll || this.VScroll) && activeControl != null && clientRectangle.Width > 0 && clientRectangle.Height > 0)
			{
				Point point = this.ScrollToControl(activeControl);
				this.SetScrollState(8, false);
				this.SetDisplayRectLocation(point.X, point.Y);
				this.SyncScrollbars(true);
			}
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x00029014 File Offset: 0x00028014
		protected virtual Point ScrollToControl(Control activeControl)
		{
			Rectangle clientRectangle = base.ClientRectangle;
			int num = this.displayRect.X;
			int num2 = this.displayRect.Y;
			int width = this.scrollMargin.Width;
			int height = this.scrollMargin.Height;
			Rectangle rectangle = activeControl.Bounds;
			if (activeControl.ParentInternal != this)
			{
				rectangle = base.RectangleToClient(activeControl.ParentInternal.RectangleToScreen(rectangle));
			}
			if (rectangle.X < width)
			{
				num = this.displayRect.X + width - rectangle.X;
			}
			else if (rectangle.X + rectangle.Width + width > clientRectangle.Width)
			{
				num = clientRectangle.Width - (rectangle.X + rectangle.Width + width - this.displayRect.X);
				if (rectangle.X + num - this.displayRect.X < width)
				{
					num = this.displayRect.X + width - rectangle.X;
				}
			}
			if (rectangle.Y < height)
			{
				num2 = this.displayRect.Y + height - rectangle.Y;
			}
			else if (rectangle.Y + rectangle.Height + height > clientRectangle.Height)
			{
				num2 = clientRectangle.Height - (rectangle.Y + rectangle.Height + height - this.displayRect.Y);
				if (rectangle.Y + num2 - this.displayRect.Y < height)
				{
					num2 = this.displayRect.Y + height - rectangle.Y;
				}
			}
			num += activeControl.AutoScrollOffset.X;
			num2 += activeControl.AutoScrollOffset.Y;
			return new Point(num, num2);
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x000291D0 File Offset: 0x000281D0
		private int ScrollThumbPosition(int fnBar)
		{
			NativeMethods.SCROLLINFO scrollinfo = new NativeMethods.SCROLLINFO();
			scrollinfo.fMask = 16;
			SafeNativeMethods.GetScrollInfo(new HandleRef(this, base.Handle), fnBar, scrollinfo);
			return scrollinfo.nTrackPos;
		}

		// Token: 0x1400004E RID: 78
		// (add) Token: 0x06001801 RID: 6145 RVA: 0x00029205 File Offset: 0x00028205
		// (remove) Token: 0x06001802 RID: 6146 RVA: 0x00029218 File Offset: 0x00028218
		[SRDescription("ScrollBarOnScrollDescr")]
		[SRCategory("CatAction")]
		public event ScrollEventHandler Scroll
		{
			add
			{
				base.Events.AddHandler(ScrollableControl.EVENT_SCROLL, value);
			}
			remove
			{
				base.Events.RemoveHandler(ScrollableControl.EVENT_SCROLL, value);
			}
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x0002922C File Offset: 0x0002822C
		protected virtual void OnScroll(ScrollEventArgs se)
		{
			ScrollEventHandler scrollEventHandler = (ScrollEventHandler)base.Events[ScrollableControl.EVENT_SCROLL];
			if (scrollEventHandler != null)
			{
				scrollEventHandler(this, se);
			}
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x0002925A File Offset: 0x0002825A
		private void ResetAutoScrollMargin()
		{
			this.AutoScrollMargin = Size.Empty;
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x00029267 File Offset: 0x00028267
		private void ResetAutoScrollMinSize()
		{
			this.AutoScrollMinSize = Size.Empty;
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x00029274 File Offset: 0x00028274
		private void ResetScrollProperties(ScrollProperties scrollProperties)
		{
			scrollProperties.visible = false;
			scrollProperties.value = 0;
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x00029284 File Offset: 0x00028284
		public void SetAutoScrollMargin(int x, int y)
		{
			if (x < 0)
			{
				x = 0;
			}
			if (y < 0)
			{
				y = 0;
			}
			if (x != this.requestedScrollMargin.Width || y != this.requestedScrollMargin.Height)
			{
				this.requestedScrollMargin = new Size(x, y);
				if (this.AutoScroll)
				{
					base.PerformLayout();
				}
			}
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x000292D8 File Offset: 0x000282D8
		private bool SetVisibleScrollbars(bool horiz, bool vert)
		{
			bool flag = false;
			if ((!horiz && this.HScroll) || (horiz && !this.HScroll) || (!vert && this.VScroll) || (vert && !this.VScroll))
			{
				flag = true;
			}
			if (horiz && !this.HScroll && this.RightToLeft == RightToLeft.Yes)
			{
				this.resetRTLHScrollValue = true;
			}
			if (flag)
			{
				int num = this.displayRect.X;
				int num2 = this.displayRect.Y;
				if (!horiz)
				{
					num = 0;
				}
				if (!vert)
				{
					num2 = 0;
				}
				this.SetDisplayRectLocation(num, num2);
				this.SetScrollState(8, false);
				this.HScroll = horiz;
				this.VScroll = vert;
				if (horiz)
				{
					this.HorizontalScroll.visible = true;
				}
				else
				{
					this.ResetScrollProperties(this.HorizontalScroll);
				}
				if (vert)
				{
					this.VerticalScroll.visible = true;
				}
				else
				{
					this.ResetScrollProperties(this.VerticalScroll);
				}
				base.UpdateStyles();
			}
			return flag;
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x000293B8 File Offset: 0x000283B8
		private bool SetDisplayRectangleSize(int width, int height)
		{
			bool flag = false;
			if (this.displayRect.Width != width || this.displayRect.Height != height)
			{
				this.displayRect.Width = width;
				this.displayRect.Height = height;
				flag = true;
			}
			int num = base.ClientRectangle.Width - width;
			int num2 = base.ClientRectangle.Height - height;
			if (num > 0)
			{
				num = 0;
			}
			if (num2 > 0)
			{
				num2 = 0;
			}
			int num3 = this.displayRect.X;
			int num4 = this.displayRect.Y;
			if (!this.HScroll)
			{
				num3 = 0;
			}
			if (!this.VScroll)
			{
				num4 = 0;
			}
			if (num3 < num)
			{
				num3 = num;
			}
			if (num4 < num2)
			{
				num4 = num2;
			}
			this.SetDisplayRectLocation(num3, num4);
			return flag;
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x00029473 File Offset: 0x00028473
		protected void SetScrollState(int bit, bool value)
		{
			if (value)
			{
				this.scrollState |= bit;
				return;
			}
			this.scrollState &= ~bit;
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x00029498 File Offset: 0x00028498
		private bool ShouldSerializeAutoScrollPosition()
		{
			if (this.AutoScroll)
			{
				Point autoScrollPosition = this.AutoScrollPosition;
				if (autoScrollPosition.X != 0 || autoScrollPosition.Y != 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x000294CC File Offset: 0x000284CC
		private bool ShouldSerializeAutoScrollMargin()
		{
			return !this.AutoScrollMargin.Equals(new Size(0, 0));
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x000294FC File Offset: 0x000284FC
		private bool ShouldSerializeAutoScrollMinSize()
		{
			return !this.AutoScrollMinSize.Equals(new Size(0, 0));
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x0002952C File Offset: 0x0002852C
		private void SyncScrollbars(bool autoScroll)
		{
			Rectangle rectangle = this.displayRect;
			if (autoScroll)
			{
				if (!base.IsHandleCreated)
				{
					return;
				}
				if (this.HScroll)
				{
					if (!this.HorizontalScroll.maximumSetExternally)
					{
						this.HorizontalScroll.maximum = rectangle.Width - 1;
					}
					if (!this.HorizontalScroll.largeChangeSetExternally)
					{
						this.HorizontalScroll.largeChange = base.ClientRectangle.Width;
					}
					if (!this.HorizontalScroll.smallChangeSetExternally)
					{
						this.HorizontalScroll.smallChange = 5;
					}
					if (this.resetRTLHScrollValue && !base.IsMirrored)
					{
						this.resetRTLHScrollValue = false;
						base.BeginInvoke(new EventHandler(this.OnSetScrollPosition));
					}
					else if (-rectangle.X >= this.HorizontalScroll.minimum && -rectangle.X < this.HorizontalScroll.maximum)
					{
						this.HorizontalScroll.value = -rectangle.X;
					}
					this.HorizontalScroll.UpdateScrollInfo();
				}
				if (this.VScroll)
				{
					if (!this.VerticalScroll.maximumSetExternally)
					{
						this.VerticalScroll.maximum = rectangle.Height - 1;
					}
					if (!this.VerticalScroll.largeChangeSetExternally)
					{
						this.VerticalScroll.largeChange = base.ClientRectangle.Height;
					}
					if (!this.VerticalScroll.smallChangeSetExternally)
					{
						this.VerticalScroll.smallChange = 5;
					}
					if (-rectangle.Y >= this.VerticalScroll.minimum && -rectangle.Y < this.VerticalScroll.maximum)
					{
						this.VerticalScroll.value = -rectangle.Y;
					}
					this.VerticalScroll.UpdateScrollInfo();
					return;
				}
			}
			else
			{
				if (this.HorizontalScroll.Visible)
				{
					this.HorizontalScroll.Value = -rectangle.X;
				}
				else
				{
					this.ResetScrollProperties(this.HorizontalScroll);
				}
				if (this.VerticalScroll.Visible)
				{
					this.VerticalScroll.Value = -rectangle.Y;
					return;
				}
				this.ResetScrollProperties(this.VerticalScroll);
			}
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x0002973D File Offset: 0x0002873D
		private void OnSetScrollPosition(object sender, EventArgs e)
		{
			if (!base.IsMirrored)
			{
				base.SendMessage(276, NativeMethods.Util.MAKELPARAM((this.RightToLeft == RightToLeft.Yes) ? 7 : 6, 0), 0);
			}
		}

		// Token: 0x06001810 RID: 6160 RVA: 0x00029767 File Offset: 0x00028767
		private void UpdateFullDrag()
		{
			this.SetScrollState(16, SystemInformation.DragFullWindows);
		}

		// Token: 0x06001811 RID: 6161 RVA: 0x00029778 File Offset: 0x00028778
		private void WmVScroll(ref Message m)
		{
			if (m.LParam != IntPtr.Zero)
			{
				base.WndProc(ref m);
				return;
			}
			Rectangle clientRectangle = base.ClientRectangle;
			bool flag = NativeMethods.Util.LOWORD(m.WParam) != 5;
			int num = -this.displayRect.Y;
			int num2 = num;
			int num3 = -(clientRectangle.Height - this.displayRect.Height);
			if (!this.AutoScroll)
			{
				num3 = this.VerticalScroll.Maximum;
			}
			switch (NativeMethods.Util.LOWORD(m.WParam))
			{
			case 0:
				if (num > 0)
				{
					num -= this.VerticalScroll.SmallChange;
				}
				else
				{
					num = 0;
				}
				break;
			case 1:
				if (num < num3 - this.VerticalScroll.SmallChange)
				{
					num += this.VerticalScroll.SmallChange;
				}
				else
				{
					num = num3;
				}
				break;
			case 2:
				if (num > this.VerticalScroll.LargeChange)
				{
					num -= this.VerticalScroll.LargeChange;
				}
				else
				{
					num = 0;
				}
				break;
			case 3:
				if (num < num3 - this.VerticalScroll.LargeChange)
				{
					num += this.VerticalScroll.LargeChange;
				}
				else
				{
					num = num3;
				}
				break;
			case 4:
			case 5:
				num = this.ScrollThumbPosition(1);
				break;
			case 6:
				num = 0;
				break;
			case 7:
				num = num3;
				break;
			}
			if (this.GetScrollState(16) || flag)
			{
				this.SetScrollState(8, true);
				this.SetDisplayRectLocation(this.displayRect.X, -num);
				this.SyncScrollbars(this.AutoScroll);
			}
			this.WmOnScroll(ref m, num2, num, ScrollOrientation.VerticalScroll);
		}

		// Token: 0x06001812 RID: 6162 RVA: 0x00029904 File Offset: 0x00028904
		private void WmHScroll(ref Message m)
		{
			if (m.LParam != IntPtr.Zero)
			{
				base.WndProc(ref m);
				return;
			}
			Rectangle clientRectangle = base.ClientRectangle;
			int num = -this.displayRect.X;
			int num2 = num;
			int num3 = -(clientRectangle.Width - this.displayRect.Width);
			if (!this.AutoScroll)
			{
				num3 = this.HorizontalScroll.Maximum;
			}
			switch (NativeMethods.Util.LOWORD(m.WParam))
			{
			case 0:
				if (num > this.HorizontalScroll.SmallChange)
				{
					num -= this.HorizontalScroll.SmallChange;
				}
				else
				{
					num = 0;
				}
				break;
			case 1:
				if (num < num3 - this.HorizontalScroll.SmallChange)
				{
					num += this.HorizontalScroll.SmallChange;
				}
				else
				{
					num = num3;
				}
				break;
			case 2:
				if (num > this.HorizontalScroll.LargeChange)
				{
					num -= this.HorizontalScroll.LargeChange;
				}
				else
				{
					num = 0;
				}
				break;
			case 3:
				if (num < num3 - this.HorizontalScroll.LargeChange)
				{
					num += this.HorizontalScroll.LargeChange;
				}
				else
				{
					num = num3;
				}
				break;
			case 4:
			case 5:
				num = this.ScrollThumbPosition(0);
				break;
			case 6:
				num = 0;
				break;
			case 7:
				num = num3;
				break;
			}
			if (this.GetScrollState(16) || NativeMethods.Util.LOWORD(m.WParam) != 5)
			{
				this.SetScrollState(8, true);
				this.SetDisplayRectLocation(-num, this.displayRect.Y);
				this.SyncScrollbars(this.AutoScroll);
			}
			this.WmOnScroll(ref m, num2, num, ScrollOrientation.HorizontalScroll);
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x00029A8C File Offset: 0x00028A8C
		private void WmOnScroll(ref Message m, int oldValue, int value, ScrollOrientation scrollOrientation)
		{
			ScrollEventType scrollEventType = (ScrollEventType)NativeMethods.Util.LOWORD(m.WParam);
			if (scrollEventType != ScrollEventType.EndScroll)
			{
				ScrollEventArgs scrollEventArgs = new ScrollEventArgs(scrollEventType, oldValue, value, scrollOrientation);
				this.OnScroll(scrollEventArgs);
			}
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x00029ABB File Offset: 0x00028ABB
		private void WmSettingChange(ref Message m)
		{
			base.WndProc(ref m);
			this.UpdateFullDrag();
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x00029ACC File Offset: 0x00028ACC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 26)
			{
				this.WmSettingChange(ref m);
				return;
			}
			switch (msg)
			{
			case 276:
				this.WmHScroll(ref m);
				return;
			case 277:
				this.WmVScroll(ref m);
				return;
			default:
				base.WndProc(ref m);
				return;
			}
		}

		// Token: 0x040011D1 RID: 4561
		protected const int ScrollStateAutoScrolling = 1;

		// Token: 0x040011D2 RID: 4562
		protected const int ScrollStateHScrollVisible = 2;

		// Token: 0x040011D3 RID: 4563
		protected const int ScrollStateVScrollVisible = 4;

		// Token: 0x040011D4 RID: 4564
		protected const int ScrollStateUserHasScrolled = 8;

		// Token: 0x040011D5 RID: 4565
		protected const int ScrollStateFullDrag = 16;

		// Token: 0x040011D6 RID: 4566
		internal static readonly TraceSwitch AutoScrolling;

		// Token: 0x040011D7 RID: 4567
		private Size userAutoScrollMinSize = Size.Empty;

		// Token: 0x040011D8 RID: 4568
		private Rectangle displayRect = Rectangle.Empty;

		// Token: 0x040011D9 RID: 4569
		private Size scrollMargin = Size.Empty;

		// Token: 0x040011DA RID: 4570
		private Size requestedScrollMargin = Size.Empty;

		// Token: 0x040011DB RID: 4571
		internal Point scrollPosition = Point.Empty;

		// Token: 0x040011DC RID: 4572
		private ScrollableControl.DockPaddingEdges dockPadding;

		// Token: 0x040011DD RID: 4573
		private int scrollState;

		// Token: 0x040011DE RID: 4574
		private VScrollProperties verticalScroll;

		// Token: 0x040011DF RID: 4575
		private HScrollProperties horizontalScroll;

		// Token: 0x040011E0 RID: 4576
		private static readonly object EVENT_SCROLL = new object();

		// Token: 0x040011E1 RID: 4577
		private bool resetRTLHScrollValue;

		// Token: 0x0200020D RID: 525
		[TypeConverter(typeof(ScrollableControl.DockPaddingEdgesConverter))]
		public class DockPaddingEdges : ICloneable
		{
			// Token: 0x06001817 RID: 6167 RVA: 0x00029B26 File Offset: 0x00028B26
			internal DockPaddingEdges(ScrollableControl owner)
			{
				this.owner = owner;
			}

			// Token: 0x06001818 RID: 6168 RVA: 0x00029B35 File Offset: 0x00028B35
			internal DockPaddingEdges(int left, int right, int top, int bottom)
			{
				this.left = left;
				this.right = right;
				this.top = top;
				this.bottom = bottom;
			}

			// Token: 0x170002DE RID: 734
			// (get) Token: 0x06001819 RID: 6169 RVA: 0x00029B5C File Offset: 0x00028B5C
			// (set) Token: 0x0600181A RID: 6170 RVA: 0x00029C29 File Offset: 0x00028C29
			[RefreshProperties(RefreshProperties.All)]
			[SRDescription("PaddingAllDescr")]
			public int All
			{
				get
				{
					if (this.owner == null)
					{
						if (this.left == this.right && this.top == this.bottom && this.left == this.top)
						{
							return this.left;
						}
						return 0;
					}
					else
					{
						if (this.owner.Padding.All == -1 && (this.owner.Padding.Left != -1 || this.owner.Padding.Top != -1 || this.owner.Padding.Right != -1 || this.owner.Padding.Bottom != -1))
						{
							return 0;
						}
						return this.owner.Padding.All;
					}
				}
				set
				{
					if (this.owner == null)
					{
						this.left = value;
						this.top = value;
						this.right = value;
						this.bottom = value;
						return;
					}
					this.owner.Padding = new Padding(value);
				}
			}

			// Token: 0x170002DF RID: 735
			// (get) Token: 0x0600181B RID: 6171 RVA: 0x00029C64 File Offset: 0x00028C64
			// (set) Token: 0x0600181C RID: 6172 RVA: 0x00029C94 File Offset: 0x00028C94
			[SRDescription("PaddingBottomDescr")]
			[RefreshProperties(RefreshProperties.All)]
			public int Bottom
			{
				get
				{
					if (this.owner == null)
					{
						return this.bottom;
					}
					return this.owner.Padding.Bottom;
				}
				set
				{
					if (this.owner == null)
					{
						this.bottom = value;
						return;
					}
					Padding padding = this.owner.Padding;
					padding.Bottom = value;
					this.owner.Padding = padding;
				}
			}

			// Token: 0x170002E0 RID: 736
			// (get) Token: 0x0600181D RID: 6173 RVA: 0x00029CD4 File Offset: 0x00028CD4
			// (set) Token: 0x0600181E RID: 6174 RVA: 0x00029D04 File Offset: 0x00028D04
			[RefreshProperties(RefreshProperties.All)]
			[SRDescription("PaddingLeftDescr")]
			public int Left
			{
				get
				{
					if (this.owner == null)
					{
						return this.left;
					}
					return this.owner.Padding.Left;
				}
				set
				{
					if (this.owner == null)
					{
						this.left = value;
						return;
					}
					Padding padding = this.owner.Padding;
					padding.Left = value;
					this.owner.Padding = padding;
				}
			}

			// Token: 0x170002E1 RID: 737
			// (get) Token: 0x0600181F RID: 6175 RVA: 0x00029D44 File Offset: 0x00028D44
			// (set) Token: 0x06001820 RID: 6176 RVA: 0x00029D74 File Offset: 0x00028D74
			[SRDescription("PaddingRightDescr")]
			[RefreshProperties(RefreshProperties.All)]
			public int Right
			{
				get
				{
					if (this.owner == null)
					{
						return this.right;
					}
					return this.owner.Padding.Right;
				}
				set
				{
					if (this.owner == null)
					{
						this.right = value;
						return;
					}
					Padding padding = this.owner.Padding;
					padding.Right = value;
					this.owner.Padding = padding;
				}
			}

			// Token: 0x170002E2 RID: 738
			// (get) Token: 0x06001821 RID: 6177 RVA: 0x00029DB4 File Offset: 0x00028DB4
			// (set) Token: 0x06001822 RID: 6178 RVA: 0x00029DE4 File Offset: 0x00028DE4
			[SRDescription("PaddingTopDescr")]
			[RefreshProperties(RefreshProperties.All)]
			public int Top
			{
				get
				{
					if (this.owner == null)
					{
						return this.bottom;
					}
					return this.owner.Padding.Top;
				}
				set
				{
					if (this.owner == null)
					{
						this.top = value;
						return;
					}
					Padding padding = this.owner.Padding;
					padding.Top = value;
					this.owner.Padding = padding;
				}
			}

			// Token: 0x06001823 RID: 6179 RVA: 0x00029E24 File Offset: 0x00028E24
			public override bool Equals(object other)
			{
				ScrollableControl.DockPaddingEdges dockPaddingEdges = other as ScrollableControl.DockPaddingEdges;
				return dockPaddingEdges != null && this.owner.Padding.Equals(dockPaddingEdges.owner.Padding);
			}

			// Token: 0x06001824 RID: 6180 RVA: 0x00029E66 File Offset: 0x00028E66
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x06001825 RID: 6181 RVA: 0x00029E6E File Offset: 0x00028E6E
			private void ResetAll()
			{
				this.All = 0;
			}

			// Token: 0x06001826 RID: 6182 RVA: 0x00029E77 File Offset: 0x00028E77
			private void ResetBottom()
			{
				this.Bottom = 0;
			}

			// Token: 0x06001827 RID: 6183 RVA: 0x00029E80 File Offset: 0x00028E80
			private void ResetLeft()
			{
				this.Left = 0;
			}

			// Token: 0x06001828 RID: 6184 RVA: 0x00029E89 File Offset: 0x00028E89
			private void ResetRight()
			{
				this.Right = 0;
			}

			// Token: 0x06001829 RID: 6185 RVA: 0x00029E92 File Offset: 0x00028E92
			private void ResetTop()
			{
				this.Top = 0;
			}

			// Token: 0x0600182A RID: 6186 RVA: 0x00029E9C File Offset: 0x00028E9C
			internal void Scale(float dx, float dy)
			{
				this.owner.Padding.Scale(dx, dy);
			}

			// Token: 0x0600182B RID: 6187 RVA: 0x00029EBE File Offset: 0x00028EBE
			public override string ToString()
			{
				return "";
			}

			// Token: 0x0600182C RID: 6188 RVA: 0x00029EC8 File Offset: 0x00028EC8
			object ICloneable.Clone()
			{
				return new ScrollableControl.DockPaddingEdges(this.Left, this.Right, this.Top, this.Bottom);
			}

			// Token: 0x040011E2 RID: 4578
			private ScrollableControl owner;

			// Token: 0x040011E3 RID: 4579
			private int left;

			// Token: 0x040011E4 RID: 4580
			private int right;

			// Token: 0x040011E5 RID: 4581
			private int top;

			// Token: 0x040011E6 RID: 4582
			private int bottom;
		}

		// Token: 0x0200020E RID: 526
		public class DockPaddingEdgesConverter : TypeConverter
		{
			// Token: 0x0600182D RID: 6189 RVA: 0x00029EF4 File Offset: 0x00028EF4
			public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(ScrollableControl.DockPaddingEdges), attributes);
				return properties.Sort(new string[] { "All", "Left", "Top", "Right", "Bottom" });
			}

			// Token: 0x0600182E RID: 6190 RVA: 0x00029F48 File Offset: 0x00028F48
			public override bool GetPropertiesSupported(ITypeDescriptorContext context)
			{
				return true;
			}
		}
	}
}
