using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020003D3 RID: 979
	public class DrawToolTipEventArgs : EventArgs
	{
		// Token: 0x06003AE8 RID: 15080 RVA: 0x000D5ACC File Offset: 0x000D4ACC
		public DrawToolTipEventArgs(Graphics graphics, IWin32Window associatedWindow, Control associatedControl, Rectangle bounds, string toolTipText, Color backColor, Color foreColor, Font font)
		{
			this.graphics = graphics;
			this.associatedWindow = associatedWindow;
			this.associatedControl = associatedControl;
			this.bounds = bounds;
			this.toolTipText = toolTipText;
			this.backColor = backColor;
			this.foreColor = foreColor;
			this.font = font;
		}

		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x06003AE9 RID: 15081 RVA: 0x000D5B1C File Offset: 0x000D4B1C
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x06003AEA RID: 15082 RVA: 0x000D5B24 File Offset: 0x000D4B24
		public IWin32Window AssociatedWindow
		{
			get
			{
				return this.associatedWindow;
			}
		}

		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x06003AEB RID: 15083 RVA: 0x000D5B2C File Offset: 0x000D4B2C
		public Control AssociatedControl
		{
			get
			{
				return this.associatedControl;
			}
		}

		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x06003AEC RID: 15084 RVA: 0x000D5B34 File Offset: 0x000D4B34
		public Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x06003AED RID: 15085 RVA: 0x000D5B3C File Offset: 0x000D4B3C
		public string ToolTipText
		{
			get
			{
				return this.toolTipText;
			}
		}

		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x06003AEE RID: 15086 RVA: 0x000D5B44 File Offset: 0x000D4B44
		public Font Font
		{
			get
			{
				return this.font;
			}
		}

		// Token: 0x06003AEF RID: 15087 RVA: 0x000D5B4C File Offset: 0x000D4B4C
		public void DrawBackground()
		{
			Brush brush = new SolidBrush(this.backColor);
			this.Graphics.FillRectangle(brush, this.bounds);
			brush.Dispose();
		}

		// Token: 0x06003AF0 RID: 15088 RVA: 0x000D5B7D File Offset: 0x000D4B7D
		public void DrawText()
		{
			this.DrawText(TextFormatFlags.HidePrefix | TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter);
		}

		// Token: 0x06003AF1 RID: 15089 RVA: 0x000D5B8A File Offset: 0x000D4B8A
		public void DrawText(TextFormatFlags flags)
		{
			TextRenderer.DrawText(this.graphics, this.toolTipText, this.font, this.bounds, this.foreColor, flags);
		}

		// Token: 0x06003AF2 RID: 15090 RVA: 0x000D5BB0 File Offset: 0x000D4BB0
		public void DrawBorder()
		{
			ControlPaint.DrawBorder(this.graphics, this.bounds, SystemColors.WindowFrame, ButtonBorderStyle.Solid);
		}

		// Token: 0x04001D6B RID: 7531
		private readonly Graphics graphics;

		// Token: 0x04001D6C RID: 7532
		private readonly IWin32Window associatedWindow;

		// Token: 0x04001D6D RID: 7533
		private readonly Control associatedControl;

		// Token: 0x04001D6E RID: 7534
		private readonly Rectangle bounds;

		// Token: 0x04001D6F RID: 7535
		private readonly string toolTipText;

		// Token: 0x04001D70 RID: 7536
		private readonly Color backColor;

		// Token: 0x04001D71 RID: 7537
		private readonly Color foreColor;

		// Token: 0x04001D72 RID: 7538
		private readonly Font font;
	}
}
