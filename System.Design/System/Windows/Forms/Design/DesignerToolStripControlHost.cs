using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms.Design
{
	internal class DesignerToolStripControlHost : ToolStripControlHost, IComponent, IDisposable
	{
		public DesignerToolStripControlHost(Control c)
			: base(c)
		{
			base.Margin = Padding.Empty;
		}

		protected override Size DefaultSize
		{
			get
			{
				return new Size(92, 22);
			}
		}

		internal GlyphCollection GetGlyphs(ToolStrip parent, GlyphCollection glyphs, Behavior standardBehavior)
		{
			if (this.b == null)
			{
				this.b = (BehaviorService)parent.Site.GetService(typeof(BehaviorService));
			}
			Point point = this.b.ControlToAdornerWindow(base.Parent);
			Rectangle bounds = this.Bounds;
			bounds.Offset(point);
			bounds.Inflate(-2, -2);
			glyphs.Add(new MiniLockedBorderGlyph(bounds, SelectionBorderGlyphType.Top, standardBehavior, true));
			glyphs.Add(new MiniLockedBorderGlyph(bounds, SelectionBorderGlyphType.Bottom, standardBehavior, true));
			glyphs.Add(new MiniLockedBorderGlyph(bounds, SelectionBorderGlyphType.Left, standardBehavior, true));
			glyphs.Add(new MiniLockedBorderGlyph(bounds, SelectionBorderGlyphType.Right, standardBehavior, true));
			return glyphs;
		}

		internal void RefreshSelectionGlyph()
		{
			ToolStrip toolStrip = base.Control as ToolStrip;
			if (toolStrip != null)
			{
				ToolStripTemplateNode.MiniToolStripRenderer miniToolStripRenderer = toolStrip.Renderer as ToolStripTemplateNode.MiniToolStripRenderer;
				if (miniToolStripRenderer != null)
				{
					miniToolStripRenderer.State = 0;
					toolStrip.Invalidate();
				}
			}
		}

		internal void SelectControl()
		{
			ToolStrip toolStrip = base.Control as ToolStrip;
			if (toolStrip != null)
			{
				ToolStripTemplateNode.MiniToolStripRenderer miniToolStripRenderer = toolStrip.Renderer as ToolStripTemplateNode.MiniToolStripRenderer;
				if (miniToolStripRenderer != null)
				{
					miniToolStripRenderer.State = 1;
					toolStrip.Invalidate();
				}
			}
		}

		private BehaviorService b;

		internal ToolStrip parent;
	}
}
