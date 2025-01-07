using System;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	internal class TableLayoutPanelResizeGlyph : Glyph
	{
		internal TableLayoutPanelResizeGlyph(Rectangle controlBounds, TableLayoutStyle style, Cursor hitTestCursor, Behavior behavior)
			: base(behavior)
		{
			this.bounds = controlBounds;
			this.hitTestCursor = hitTestCursor;
			this.style = style;
			if (style is ColumnStyle)
			{
				this.type = TableLayoutPanelResizeGlyph.TableLayoutResizeType.Column;
				return;
			}
			this.type = TableLayoutPanelResizeGlyph.TableLayoutResizeType.Row;
		}

		public override Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		public TableLayoutStyle Style
		{
			get
			{
				return this.style;
			}
		}

		public TableLayoutPanelResizeGlyph.TableLayoutResizeType Type
		{
			get
			{
				return this.type;
			}
		}

		public override Cursor GetHitTest(Point p)
		{
			if (this.bounds.Contains(p))
			{
				return this.hitTestCursor;
			}
			return null;
		}

		public override void Paint(PaintEventArgs pe)
		{
		}

		private Rectangle bounds;

		private Cursor hitTestCursor;

		private TableLayoutStyle style;

		private TableLayoutPanelResizeGlyph.TableLayoutResizeType type;

		public enum TableLayoutResizeType
		{
			Column,
			Row
		}
	}
}
