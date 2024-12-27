using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x02000311 RID: 785
	internal sealed class ToolStripPanelSelectionGlyph : ControlBodyGlyph
	{
		// Token: 0x06001DE0 RID: 7648 RVA: 0x000AAAE8 File Offset: 0x000A9AE8
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

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06001DE1 RID: 7649 RVA: 0x000AAB62 File Offset: 0x000A9B62
		// (set) Token: 0x06001DE2 RID: 7650 RVA: 0x000AAB6A File Offset: 0x000A9B6A
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

		// Token: 0x06001DE3 RID: 7651 RVA: 0x000AAB84 File Offset: 0x000A9B84
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

		// Token: 0x06001DE4 RID: 7652 RVA: 0x000AABF0 File Offset: 0x000A9BF0
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

		// Token: 0x06001DE5 RID: 7653 RVA: 0x000AAD84 File Offset: 0x000A9D84
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

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06001DE6 RID: 7654 RVA: 0x000AAF15 File Offset: 0x000A9F15
		public override Rectangle Bounds
		{
			get
			{
				return this.glyphBounds;
			}
		}

		// Token: 0x06001DE7 RID: 7655 RVA: 0x000AAF20 File Offset: 0x000A9F20
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

		// Token: 0x06001DE8 RID: 7656 RVA: 0x000AAF88 File Offset: 0x000A9F88
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

		// Token: 0x04001716 RID: 5910
		private const int imageWidth = 50;

		// Token: 0x04001717 RID: 5911
		private const int imageHeight = 6;

		// Token: 0x04001718 RID: 5912
		private ToolStripPanel relatedPanel;

		// Token: 0x04001719 RID: 5913
		private Rectangle glyphBounds;

		// Token: 0x0400171A RID: 5914
		private IServiceProvider provider;

		// Token: 0x0400171B RID: 5915
		private ToolStripPanelSelectionBehavior relatedBehavior;

		// Token: 0x0400171C RID: 5916
		private Image image;

		// Token: 0x0400171D RID: 5917
		private Control baseParent;

		// Token: 0x0400171E RID: 5918
		private BehaviorService behaviorService;

		// Token: 0x0400171F RID: 5919
		private bool isExpanded;
	}
}
