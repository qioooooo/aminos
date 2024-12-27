using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x020003CC RID: 972
	public class DrawListViewColumnHeaderEventArgs : EventArgs
	{
		// Token: 0x06003AB2 RID: 15026 RVA: 0x000D53E4 File Offset: 0x000D43E4
		public DrawListViewColumnHeaderEventArgs(Graphics graphics, Rectangle bounds, int columnIndex, ColumnHeader header, ListViewItemStates state, Color foreColor, Color backColor, Font font)
		{
			this.graphics = graphics;
			this.bounds = bounds;
			this.columnIndex = columnIndex;
			this.header = header;
			this.state = state;
			this.foreColor = foreColor;
			this.backColor = backColor;
			this.font = font;
		}

		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x06003AB3 RID: 15027 RVA: 0x000D5434 File Offset: 0x000D4434
		// (set) Token: 0x06003AB4 RID: 15028 RVA: 0x000D543C File Offset: 0x000D443C
		public bool DrawDefault
		{
			get
			{
				return this.drawDefault;
			}
			set
			{
				this.drawDefault = value;
			}
		}

		// Token: 0x17000B1B RID: 2843
		// (get) Token: 0x06003AB5 RID: 15029 RVA: 0x000D5445 File Offset: 0x000D4445
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x06003AB6 RID: 15030 RVA: 0x000D544D File Offset: 0x000D444D
		public Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x06003AB7 RID: 15031 RVA: 0x000D5455 File Offset: 0x000D4455
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x06003AB8 RID: 15032 RVA: 0x000D545D File Offset: 0x000D445D
		public ColumnHeader Header
		{
			get
			{
				return this.header;
			}
		}

		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x06003AB9 RID: 15033 RVA: 0x000D5465 File Offset: 0x000D4465
		public ListViewItemStates State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x06003ABA RID: 15034 RVA: 0x000D546D File Offset: 0x000D446D
		public Color ForeColor
		{
			get
			{
				return this.foreColor;
			}
		}

		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x06003ABB RID: 15035 RVA: 0x000D5475 File Offset: 0x000D4475
		public Color BackColor
		{
			get
			{
				return this.backColor;
			}
		}

		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x06003ABC RID: 15036 RVA: 0x000D547D File Offset: 0x000D447D
		public Font Font
		{
			get
			{
				return this.font;
			}
		}

		// Token: 0x06003ABD RID: 15037 RVA: 0x000D5488 File Offset: 0x000D4488
		public void DrawBackground()
		{
			if (Application.RenderWithVisualStyles)
			{
				VisualStyleRenderer visualStyleRenderer = new VisualStyleRenderer(VisualStyleElement.Header.Item.Normal);
				visualStyleRenderer.DrawBackground(this.graphics, this.bounds);
				return;
			}
			using (Brush brush = new SolidBrush(this.backColor))
			{
				this.graphics.FillRectangle(brush, this.bounds);
			}
			Rectangle rectangle = this.bounds;
			rectangle.Width--;
			rectangle.Height--;
			this.graphics.DrawRectangle(SystemPens.ControlDarkDark, rectangle);
			rectangle.Width--;
			rectangle.Height--;
			this.graphics.DrawLine(SystemPens.ControlLightLight, rectangle.X, rectangle.Y, rectangle.Right, rectangle.Y);
			this.graphics.DrawLine(SystemPens.ControlLightLight, rectangle.X, rectangle.Y, rectangle.X, rectangle.Bottom);
			this.graphics.DrawLine(SystemPens.ControlDark, rectangle.X + 1, rectangle.Bottom, rectangle.Right, rectangle.Bottom);
			this.graphics.DrawLine(SystemPens.ControlDark, rectangle.Right, rectangle.Y + 1, rectangle.Right, rectangle.Bottom);
		}

		// Token: 0x06003ABE RID: 15038 RVA: 0x000D55FC File Offset: 0x000D45FC
		public void DrawText()
		{
			HorizontalAlignment textAlign = this.header.TextAlign;
			TextFormatFlags textFormatFlags = ((textAlign == HorizontalAlignment.Left) ? TextFormatFlags.Default : ((textAlign == HorizontalAlignment.Center) ? TextFormatFlags.HorizontalCenter : TextFormatFlags.Right));
			textFormatFlags |= TextFormatFlags.WordEllipsis;
			this.DrawText(textFormatFlags);
		}

		// Token: 0x06003ABF RID: 15039 RVA: 0x000D5634 File Offset: 0x000D4634
		public void DrawText(TextFormatFlags flags)
		{
			string text = this.header.Text;
			int width = TextRenderer.MeasureText(" ", this.font).Width;
			Rectangle rectangle = Rectangle.Inflate(this.bounds, -width, 0);
			TextRenderer.DrawText(this.graphics, text, this.font, rectangle, this.foreColor, flags);
		}

		// Token: 0x04001D4F RID: 7503
		private readonly Graphics graphics;

		// Token: 0x04001D50 RID: 7504
		private readonly Rectangle bounds;

		// Token: 0x04001D51 RID: 7505
		private readonly int columnIndex;

		// Token: 0x04001D52 RID: 7506
		private readonly ColumnHeader header;

		// Token: 0x04001D53 RID: 7507
		private readonly ListViewItemStates state;

		// Token: 0x04001D54 RID: 7508
		private readonly Color foreColor;

		// Token: 0x04001D55 RID: 7509
		private readonly Color backColor;

		// Token: 0x04001D56 RID: 7510
		private readonly Font font;

		// Token: 0x04001D57 RID: 7511
		private bool drawDefault;
	}
}
