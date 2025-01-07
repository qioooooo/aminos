using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	internal sealed class DesignerActionGlyph : Glyph
	{
		public DesignerActionGlyph(DesignerActionBehavior behavior, Adorner adorner)
			: this(behavior, adorner, Rectangle.Empty, null)
		{
		}

		public DesignerActionGlyph(DesignerActionBehavior behavior, Rectangle alternativeBounds, Control alternativeParent)
			: this(behavior, null, alternativeBounds, alternativeParent)
		{
		}

		private DesignerActionGlyph(DesignerActionBehavior behavior, Adorner adorner, Rectangle alternativeBounds, Control alternativeParent)
			: base(behavior)
		{
			this.adorner = adorner;
			this.alternativeBounds = alternativeBounds;
			this.alternativeParent = alternativeParent;
			this.Invalidate();
		}

		public override Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

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

		public bool IsInComponentTray
		{
			get
			{
				return this.adorner == null;
			}
		}

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

		internal void InvalidateOwnerLocation()
		{
			if (this.alternativeParent != null)
			{
				this.alternativeParent.Invalidate(this.bounds);
				return;
			}
			this.adorner.Invalidate(this.bounds);
		}

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

		internal void UpdateAlternativeBounds(Rectangle newBounds)
		{
			this.alternativeBounds = newBounds;
			this.Invalidate();
		}

		internal const int CONTROLOVERLAP_X = 5;

		internal const int CONTROLOVERLAP_Y = 2;

		private Rectangle bounds;

		private Adorner adorner;

		private bool mouseOver;

		private Rectangle alternativeBounds = Rectangle.Empty;

		private Control alternativeParent;

		private DockStyle dockStyle;

		private Bitmap glyphImageClosed;

		private Bitmap glyphImageOpened;
	}
}
