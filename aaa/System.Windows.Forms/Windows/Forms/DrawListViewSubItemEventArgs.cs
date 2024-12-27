using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020003D0 RID: 976
	public class DrawListViewSubItemEventArgs : EventArgs
	{
		// Token: 0x06003AD5 RID: 15061 RVA: 0x000D58A8 File Offset: 0x000D48A8
		public DrawListViewSubItemEventArgs(Graphics graphics, Rectangle bounds, ListViewItem item, ListViewItem.ListViewSubItem subItem, int itemIndex, int columnIndex, ColumnHeader header, ListViewItemStates itemState)
		{
			this.graphics = graphics;
			this.bounds = bounds;
			this.item = item;
			this.subItem = subItem;
			this.itemIndex = itemIndex;
			this.columnIndex = columnIndex;
			this.header = header;
			this.itemState = itemState;
		}

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x06003AD6 RID: 15062 RVA: 0x000D58F8 File Offset: 0x000D48F8
		// (set) Token: 0x06003AD7 RID: 15063 RVA: 0x000D5900 File Offset: 0x000D4900
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

		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x06003AD8 RID: 15064 RVA: 0x000D5909 File Offset: 0x000D4909
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x06003AD9 RID: 15065 RVA: 0x000D5911 File Offset: 0x000D4911
		public Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x06003ADA RID: 15066 RVA: 0x000D5919 File Offset: 0x000D4919
		public ListViewItem Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x06003ADB RID: 15067 RVA: 0x000D5921 File Offset: 0x000D4921
		public ListViewItem.ListViewSubItem SubItem
		{
			get
			{
				return this.subItem;
			}
		}

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x06003ADC RID: 15068 RVA: 0x000D5929 File Offset: 0x000D4929
		public int ItemIndex
		{
			get
			{
				return this.itemIndex;
			}
		}

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x06003ADD RID: 15069 RVA: 0x000D5931 File Offset: 0x000D4931
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x06003ADE RID: 15070 RVA: 0x000D5939 File Offset: 0x000D4939
		public ColumnHeader Header
		{
			get
			{
				return this.header;
			}
		}

		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x06003ADF RID: 15071 RVA: 0x000D5941 File Offset: 0x000D4941
		public ListViewItemStates ItemState
		{
			get
			{
				return this.itemState;
			}
		}

		// Token: 0x06003AE0 RID: 15072 RVA: 0x000D594C File Offset: 0x000D494C
		public void DrawBackground()
		{
			Color color = ((this.itemIndex == -1) ? this.item.BackColor : this.subItem.BackColor);
			using (Brush brush = new SolidBrush(color))
			{
				this.Graphics.FillRectangle(brush, this.bounds);
			}
		}

		// Token: 0x06003AE1 RID: 15073 RVA: 0x000D59B0 File Offset: 0x000D49B0
		public void DrawFocusRectangle(Rectangle bounds)
		{
			if ((this.itemState & ListViewItemStates.Focused) == ListViewItemStates.Focused)
			{
				ControlPaint.DrawFocusRectangle(this.graphics, Rectangle.Inflate(bounds, -1, -1), this.item.ForeColor, this.item.BackColor);
			}
		}

		// Token: 0x06003AE2 RID: 15074 RVA: 0x000D59E8 File Offset: 0x000D49E8
		public void DrawText()
		{
			HorizontalAlignment textAlign = this.header.TextAlign;
			TextFormatFlags textFormatFlags = ((textAlign == HorizontalAlignment.Left) ? TextFormatFlags.Default : ((textAlign == HorizontalAlignment.Center) ? TextFormatFlags.HorizontalCenter : TextFormatFlags.Right));
			textFormatFlags |= TextFormatFlags.WordEllipsis;
			this.DrawText(textFormatFlags);
		}

		// Token: 0x06003AE3 RID: 15075 RVA: 0x000D5A20 File Offset: 0x000D4A20
		public void DrawText(TextFormatFlags flags)
		{
			string text = ((this.itemIndex == -1) ? this.item.Text : this.subItem.Text);
			Font font = ((this.itemIndex == -1) ? this.item.Font : this.subItem.Font);
			Color color = ((this.itemIndex == -1) ? this.item.ForeColor : this.subItem.ForeColor);
			int width = TextRenderer.MeasureText(" ", font).Width;
			Rectangle rectangle = Rectangle.Inflate(this.bounds, -width, 0);
			TextRenderer.DrawText(this.graphics, text, font, rectangle, color, flags);
		}

		// Token: 0x04001D5E RID: 7518
		private readonly Graphics graphics;

		// Token: 0x04001D5F RID: 7519
		private readonly Rectangle bounds;

		// Token: 0x04001D60 RID: 7520
		private readonly ListViewItem item;

		// Token: 0x04001D61 RID: 7521
		private readonly ListViewItem.ListViewSubItem subItem;

		// Token: 0x04001D62 RID: 7522
		private readonly int itemIndex;

		// Token: 0x04001D63 RID: 7523
		private readonly int columnIndex;

		// Token: 0x04001D64 RID: 7524
		private readonly ColumnHeader header;

		// Token: 0x04001D65 RID: 7525
		private readonly ListViewItemStates itemState;

		// Token: 0x04001D66 RID: 7526
		private bool drawDefault;
	}
}
