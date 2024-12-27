using System;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x0200030B RID: 779
	internal class TableLayoutPanelResizeGlyph : Glyph
	{
		// Token: 0x06001DC5 RID: 7621 RVA: 0x000A9A54 File Offset: 0x000A8A54
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

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001DC6 RID: 7622 RVA: 0x000A9A8A File Offset: 0x000A8A8A
		public override Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001DC7 RID: 7623 RVA: 0x000A9A92 File Offset: 0x000A8A92
		public TableLayoutStyle Style
		{
			get
			{
				return this.style;
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001DC8 RID: 7624 RVA: 0x000A9A9A File Offset: 0x000A8A9A
		public TableLayoutPanelResizeGlyph.TableLayoutResizeType Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x000A9AA2 File Offset: 0x000A8AA2
		public override Cursor GetHitTest(Point p)
		{
			if (this.bounds.Contains(p))
			{
				return this.hitTestCursor;
			}
			return null;
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x000A9ABA File Offset: 0x000A8ABA
		public override void Paint(PaintEventArgs pe)
		{
		}

		// Token: 0x040016F9 RID: 5881
		private Rectangle bounds;

		// Token: 0x040016FA RID: 5882
		private Cursor hitTestCursor;

		// Token: 0x040016FB RID: 5883
		private TableLayoutStyle style;

		// Token: 0x040016FC RID: 5884
		private TableLayoutPanelResizeGlyph.TableLayoutResizeType type;

		// Token: 0x0200030C RID: 780
		public enum TableLayoutResizeType
		{
			// Token: 0x040016FE RID: 5886
			Column,
			// Token: 0x040016FF RID: 5887
			Row
		}
	}
}
