using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x020002EE RID: 750
	internal sealed class DesignerActionGlyph : Glyph
	{
		// Token: 0x06001D0F RID: 7439 RVA: 0x000A202C File Offset: 0x000A102C
		public DesignerActionGlyph(DesignerActionBehavior behavior, Adorner adorner)
			: this(behavior, adorner, Rectangle.Empty, null)
		{
		}

		// Token: 0x06001D10 RID: 7440 RVA: 0x000A203C File Offset: 0x000A103C
		public DesignerActionGlyph(DesignerActionBehavior behavior, Rectangle alternativeBounds, Control alternativeParent)
			: this(behavior, null, alternativeBounds, alternativeParent)
		{
		}

		// Token: 0x06001D11 RID: 7441 RVA: 0x000A2048 File Offset: 0x000A1048
		private DesignerActionGlyph(DesignerActionBehavior behavior, Adorner adorner, Rectangle alternativeBounds, Control alternativeParent)
			: base(behavior)
		{
			this.adorner = adorner;
			this.alternativeBounds = alternativeBounds;
			this.alternativeParent = alternativeParent;
			this.Invalidate();
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06001D12 RID: 7442 RVA: 0x000A2078 File Offset: 0x000A1078
		public override Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001D13 RID: 7443 RVA: 0x000A2080 File Offset: 0x000A1080
		// (set) Token: 0x06001D14 RID: 7444 RVA: 0x000A2088 File Offset: 0x000A1088
		public DockStyle DockEdge
		{
			get
			{
				return this.dockStyle;
			}
			set
			{
				if (this.dockStyle != value)
				{
					this.dockStyle = value;
				}
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06001D15 RID: 7445 RVA: 0x000A209A File Offset: 0x000A109A
		public bool IsInComponentTray
		{
			get
			{
				return this.adorner == null;
			}
		}

		// Token: 0x06001D16 RID: 7446 RVA: 0x000A20A5 File Offset: 0x000A10A5
		public override Cursor GetHitTest(Point p)
		{
			if (this.bounds.Contains(p))
			{
				this.MouseOver = true;
				return Cursors.Default;
			}
			this.MouseOver = false;
			return null;
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06001D17 RID: 7447 RVA: 0x000A20CA File Offset: 0x000A10CA
		private Image GlyphImageClosed
		{
			get
			{
				if (this.glyphImageClosed == null)
				{
					this.glyphImageClosed = new Bitmap(typeof(DesignerActionGlyph), "Close_left.bmp");
					this.glyphImageClosed.MakeTransparent(Color.Magenta);
				}
				return this.glyphImageClosed;
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06001D18 RID: 7448 RVA: 0x000A2104 File Offset: 0x000A1104
		private Image GlyphImageOpened
		{
			get
			{
				if (this.glyphImageOpened == null)
				{
					this.glyphImageOpened = new Bitmap(typeof(DesignerActionGlyph), "Open_left.bmp");
					this.glyphImageOpened.MakeTransparent(Color.Magenta);
				}
				return this.glyphImageOpened;
			}
		}

		// Token: 0x06001D19 RID: 7449 RVA: 0x000A213E File Offset: 0x000A113E
		internal void InvalidateOwnerLocation()
		{
			if (this.alternativeParent != null)
			{
				this.alternativeParent.Invalidate(this.bounds);
				return;
			}
			this.adorner.Invalidate(this.bounds);
		}

		// Token: 0x06001D1A RID: 7450 RVA: 0x000A216C File Offset: 0x000A116C
		internal void Invalidate()
		{
			IComponent relatedComponent = ((DesignerActionBehavior)this.Behavior).RelatedComponent;
			Point point = Point.Empty;
			Control control = relatedComponent as Control;
			if (control != null && !(relatedComponent is ToolStripDropDown) && this.adorner != null)
			{
				point = this.adorner.BehaviorService.ControlToAdornerWindow(control);
				Control parent = control.Parent;
				point.X += control.Width;
			}
			else
			{
				ComponentTray componentTray = this.alternativeParent as ComponentTray;
				if (componentTray != null)
				{
					ComponentTray.TrayControl trayControlFromComponent = componentTray.GetTrayControlFromComponent(relatedComponent);
					if (trayControlFromComponent != null)
					{
						this.alternativeBounds = trayControlFromComponent.Bounds;
					}
				}
				Rectangle boundsForNoResizeSelectionType = DesignerUtils.GetBoundsForNoResizeSelectionType(this.alternativeBounds, SelectionBorderGlyphType.Top);
				point.X = boundsForNoResizeSelectionType.Right;
				point.Y = boundsForNoResizeSelectionType.Top;
			}
			point.X -= this.GlyphImageOpened.Width + 5;
			point.Y -= this.GlyphImageOpened.Height - 2;
			this.bounds = new Rectangle(point.X, point.Y, this.GlyphImageOpened.Width, this.GlyphImageOpened.Height);
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06001D1B RID: 7451 RVA: 0x000A2291 File Offset: 0x000A1291
		// (set) Token: 0x06001D1C RID: 7452 RVA: 0x000A2299 File Offset: 0x000A1299
		private bool MouseOver
		{
			get
			{
				return this.mouseOver;
			}
			set
			{
				if (this.mouseOver != value)
				{
					this.mouseOver = value;
					this.InvalidateOwnerLocation();
				}
			}
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x000A22B4 File Offset: 0x000A12B4
		public override void Paint(PaintEventArgs pe)
		{
			if (this.Behavior is DesignerActionBehavior)
			{
				IComponent lastPanelComponent = ((DesignerActionBehavior)this.Behavior).ParentUI.LastPanelComponent;
				IComponent relatedComponent = ((DesignerActionBehavior)this.Behavior).RelatedComponent;
				Image image;
				if (lastPanelComponent != null && lastPanelComponent == relatedComponent)
				{
					image = this.GlyphImageOpened;
				}
				else
				{
					image = this.GlyphImageClosed;
				}
				pe.Graphics.DrawImage(image, this.bounds.Left, this.bounds.Top);
				if (this.MouseOver || (lastPanelComponent != null && lastPanelComponent == relatedComponent))
				{
					pe.Graphics.FillRectangle(DesignerUtils.HoverBrush, Rectangle.Inflate(this.bounds, -1, -1));
				}
			}
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x000A235D File Offset: 0x000A135D
		internal void UpdateAlternativeBounds(Rectangle newBounds)
		{
			this.alternativeBounds = newBounds;
			this.Invalidate();
		}

		// Token: 0x0400161F RID: 5663
		internal const int CONTROLOVERLAP_X = 5;

		// Token: 0x04001620 RID: 5664
		internal const int CONTROLOVERLAP_Y = 2;

		// Token: 0x04001621 RID: 5665
		private Rectangle bounds;

		// Token: 0x04001622 RID: 5666
		private Adorner adorner;

		// Token: 0x04001623 RID: 5667
		private bool mouseOver;

		// Token: 0x04001624 RID: 5668
		private Rectangle alternativeBounds = Rectangle.Empty;

		// Token: 0x04001625 RID: 5669
		private Control alternativeParent;

		// Token: 0x04001626 RID: 5670
		private DockStyle dockStyle;

		// Token: 0x04001627 RID: 5671
		private Bitmap glyphImageClosed;

		// Token: 0x04001628 RID: 5672
		private Bitmap glyphImageOpened;
	}
}
