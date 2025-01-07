using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	internal sealed class ToolStripPanelSelectionGlyph : ControlBodyGlyph
	{
		internal ToolStripPanelSelectionGlyph(Rectangle bounds, Cursor cursor, IComponent relatedComponent, IServiceProvider provider, ToolStripPanelSelectionBehavior behavior)
			: base(bounds, cursor, relatedComponent, behavior)
		{
			this.relatedBehavior = behavior;
			this.provider = provider;
			this.relatedPanel = relatedComponent as ToolStripPanel;
			this.behaviorService = (BehaviorService)provider.GetService(typeof(BehaviorService));
			if (this.behaviorService == null)
			{
				return;
			}
			if ((IDesignerHost)provider.GetService(typeof(IDesignerHost)) == null)
			{
				return;
			}
			this.UpdateGlyph();
		}

		public bool IsExpanded
		{
			get
			{
				return this.isExpanded;
			}
			set
			{
				if (value != this.isExpanded)
				{
					this.isExpanded = value;
					this.UpdateGlyph();
				}
			}
		}

		public void UpdateGlyph()
		{
			if (this.behaviorService != null)
			{
				Rectangle rectangle = this.behaviorService.ControlRectInAdornerWindow(this.relatedPanel);
				this.glyphBounds = Rectangle.Empty;
				ToolStripContainer toolStripContainer = this.relatedPanel.Parent as ToolStripContainer;
				if (toolStripContainer != null)
				{
					this.baseParent = toolStripContainer.Parent;
				}
				if (!this.isExpanded)
				{
					this.CollapseGlyph(rectangle);
					return;
				}
				this.ExpandGlyph(rectangle);
			}
		}

		private void CollapseGlyph(Rectangle bounds)
		{
			switch (this.relatedPanel.Dock)
			{
			case DockStyle.Top:
			{
				this.image = new Bitmap(typeof(ToolStripPanelSelectionGlyph), "topopen.bmp");
				int num = (bounds.Width - 50) / 2;
				if (num > 0)
				{
					this.glyphBounds = new Rectangle(bounds.X + num, bounds.Y + bounds.Height, 50, 6);
					return;
				}
				break;
			}
			case DockStyle.Bottom:
			{
				this.image = new Bitmap(typeof(ToolStripPanelSelectionGlyph), "bottomopen.bmp");
				int num = (bounds.Width - 50) / 2;
				if (num > 0)
				{
					this.glyphBounds = new Rectangle(bounds.X + num, bounds.Y - 6, 50, 6);
					return;
				}
				break;
			}
			case DockStyle.Left:
			{
				this.image = new Bitmap(typeof(ToolStripPanelSelectionGlyph), "leftopen.bmp");
				int num2 = (bounds.Height - 50) / 2;
				if (num2 > 0)
				{
					this.glyphBounds = new Rectangle(bounds.X + bounds.Width, bounds.Y + num2, 6, 50);
					return;
				}
				break;
			}
			case DockStyle.Right:
			{
				this.image = new Bitmap(typeof(ToolStripPanelSelectionGlyph), "rightopen.bmp");
				int num2 = (bounds.Height - 50) / 2;
				if (num2 > 0)
				{
					this.glyphBounds = new Rectangle(bounds.X - 6, bounds.Y + num2, 6, 50);
					return;
				}
				break;
			}
			default:
				throw new Exception(SR.GetString("ToolStripPanelGlyphUnsupportedDock"));
			}
		}

		private void ExpandGlyph(Rectangle bounds)
		{
			switch (this.relatedPanel.Dock)
			{
			case DockStyle.Top:
			{
				this.image = new Bitmap(typeof(ToolStripPanelSelectionGlyph), "topclose.bmp");
				int num = (bounds.Width - 50) / 2;
				if (num > 0)
				{
					this.glyphBounds = new Rectangle(bounds.X + num, bounds.Y + bounds.Height, 50, 6);
					return;
				}
				break;
			}
			case DockStyle.Bottom:
			{
				this.image = new Bitmap(typeof(ToolStripPanelSelectionGlyph), "bottomclose.bmp");
				int num = (bounds.Width - 50) / 2;
				if (num > 0)
				{
					this.glyphBounds = new Rectangle(bounds.X + num, bounds.Y - 6, 50, 6);
					return;
				}
				break;
			}
			case DockStyle.Left:
			{
				this.image = new Bitmap(typeof(ToolStripPanelSelectionGlyph), "leftclose.bmp");
				int num2 = (bounds.Height - 50) / 2;
				if (num2 > 0)
				{
					this.glyphBounds = new Rectangle(bounds.X + bounds.Width, bounds.Y + num2, 6, 50);
					return;
				}
				break;
			}
			case DockStyle.Right:
			{
				this.image = new Bitmap(typeof(ToolStripPanelSelectionGlyph), "rightclose.bmp");
				int num2 = (bounds.Height - 50) / 2;
				if (num2 > 0)
				{
					this.glyphBounds = new Rectangle(bounds.X - 6, bounds.Y + num2, 6, 50);
					return;
				}
				break;
			}
			default:
				throw new Exception(SR.GetString("ToolStripPanelGlyphUnsupportedDock"));
			}
		}

		public override Rectangle Bounds
		{
			get
			{
				return this.glyphBounds;
			}
		}

		public override Cursor GetHitTest(Point p)
		{
			if (this.behaviorService != null && this.baseParent != null)
			{
				Rectangle rectangle = this.behaviorService.ControlRectInAdornerWindow(this.baseParent);
				if (this.glyphBounds != Rectangle.Empty && rectangle.Contains(this.glyphBounds) && this.glyphBounds.Contains(p))
				{
					return Cursors.Hand;
				}
			}
			return null;
		}

		public override void Paint(PaintEventArgs pe)
		{
			if (this.behaviorService != null && this.baseParent != null)
			{
				Rectangle rectangle = this.behaviorService.ControlRectInAdornerWindow(this.baseParent);
				if (this.relatedPanel.Visible && this.image != null && this.glyphBounds != Rectangle.Empty && rectangle.Contains(this.glyphBounds))
				{
					pe.Graphics.DrawImage(this.image, this.glyphBounds.Left, this.glyphBounds.Top);
				}
			}
		}

		private const int imageWidth = 50;

		private const int imageHeight = 6;

		private ToolStripPanel relatedPanel;

		private Rectangle glyphBounds;

		private IServiceProvider provider;

		private ToolStripPanelSelectionBehavior relatedBehavior;

		private Image image;

		private Control baseParent;

		private BehaviorService behaviorService;

		private bool isExpanded;
	}
}
