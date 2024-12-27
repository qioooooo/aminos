using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020003C9 RID: 969
	public class DrawItemEventArgs : EventArgs
	{
		// Token: 0x06003AA3 RID: 15011 RVA: 0x000D528C File Offset: 0x000D428C
		public DrawItemEventArgs(Graphics graphics, Font font, Rectangle rect, int index, DrawItemState state)
		{
			this.graphics = graphics;
			this.font = font;
			this.rect = rect;
			this.index = index;
			this.state = state;
			this.foreColor = SystemColors.WindowText;
			this.backColor = SystemColors.Window;
		}

		// Token: 0x06003AA4 RID: 15012 RVA: 0x000D52DA File Offset: 0x000D42DA
		public DrawItemEventArgs(Graphics graphics, Font font, Rectangle rect, int index, DrawItemState state, Color foreColor, Color backColor)
		{
			this.graphics = graphics;
			this.font = font;
			this.rect = rect;
			this.index = index;
			this.state = state;
			this.foreColor = foreColor;
			this.backColor = backColor;
		}

		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x06003AA5 RID: 15013 RVA: 0x000D5317 File Offset: 0x000D4317
		public Color BackColor
		{
			get
			{
				if ((this.state & DrawItemState.Selected) == DrawItemState.Selected)
				{
					return SystemColors.Highlight;
				}
				return this.backColor;
			}
		}

		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x06003AA6 RID: 15014 RVA: 0x000D5330 File Offset: 0x000D4330
		public Rectangle Bounds
		{
			get
			{
				return this.rect;
			}
		}

		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x06003AA7 RID: 15015 RVA: 0x000D5338 File Offset: 0x000D4338
		public Font Font
		{
			get
			{
				return this.font;
			}
		}

		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x06003AA8 RID: 15016 RVA: 0x000D5340 File Offset: 0x000D4340
		public Color ForeColor
		{
			get
			{
				if ((this.state & DrawItemState.Selected) == DrawItemState.Selected)
				{
					return SystemColors.HighlightText;
				}
				return this.foreColor;
			}
		}

		// Token: 0x17000B17 RID: 2839
		// (get) Token: 0x06003AA9 RID: 15017 RVA: 0x000D5359 File Offset: 0x000D4359
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x06003AAA RID: 15018 RVA: 0x000D5361 File Offset: 0x000D4361
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x06003AAB RID: 15019 RVA: 0x000D5369 File Offset: 0x000D4369
		public DrawItemState State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x06003AAC RID: 15020 RVA: 0x000D5374 File Offset: 0x000D4374
		public virtual void DrawBackground()
		{
			Brush brush = new SolidBrush(this.BackColor);
			this.Graphics.FillRectangle(brush, this.rect);
			brush.Dispose();
		}

		// Token: 0x06003AAD RID: 15021 RVA: 0x000D53A5 File Offset: 0x000D43A5
		public virtual void DrawFocusRectangle()
		{
			if ((this.state & DrawItemState.Focus) == DrawItemState.Focus && (this.state & DrawItemState.NoFocusRect) != DrawItemState.NoFocusRect)
			{
				ControlPaint.DrawFocusRectangle(this.Graphics, this.rect, this.ForeColor, this.BackColor);
			}
		}

		// Token: 0x04001D3B RID: 7483
		private Color backColor;

		// Token: 0x04001D3C RID: 7484
		private Color foreColor;

		// Token: 0x04001D3D RID: 7485
		private Font font;

		// Token: 0x04001D3E RID: 7486
		private readonly Graphics graphics;

		// Token: 0x04001D3F RID: 7487
		private readonly int index;

		// Token: 0x04001D40 RID: 7488
		private readonly Rectangle rect;

		// Token: 0x04001D41 RID: 7489
		private readonly DrawItemState state;
	}
}
