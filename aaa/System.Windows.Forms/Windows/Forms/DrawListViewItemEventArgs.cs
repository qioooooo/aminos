using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020003CE RID: 974
	public class DrawListViewItemEventArgs : EventArgs
	{
		// Token: 0x06003AC4 RID: 15044 RVA: 0x000D568F File Offset: 0x000D468F
		public DrawListViewItemEventArgs(Graphics graphics, ListViewItem item, Rectangle bounds, int itemIndex, ListViewItemStates state)
		{
			this.graphics = graphics;
			this.item = item;
			this.bounds = bounds;
			this.itemIndex = itemIndex;
			this.state = state;
			this.drawDefault = false;
		}

		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x06003AC5 RID: 15045 RVA: 0x000D56C3 File Offset: 0x000D46C3
		// (set) Token: 0x06003AC6 RID: 15046 RVA: 0x000D56CB File Offset: 0x000D46CB
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

		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x06003AC7 RID: 15047 RVA: 0x000D56D4 File Offset: 0x000D46D4
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x06003AC8 RID: 15048 RVA: 0x000D56DC File Offset: 0x000D46DC
		public ListViewItem Item
		{
			get
			{
				return this.item;
			}
		}

		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x06003AC9 RID: 15049 RVA: 0x000D56E4 File Offset: 0x000D46E4
		public Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x06003ACA RID: 15050 RVA: 0x000D56EC File Offset: 0x000D46EC
		public int ItemIndex
		{
			get
			{
				return this.itemIndex;
			}
		}

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x06003ACB RID: 15051 RVA: 0x000D56F4 File Offset: 0x000D46F4
		public ListViewItemStates State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x06003ACC RID: 15052 RVA: 0x000D56FC File Offset: 0x000D46FC
		public void DrawBackground()
		{
			Brush brush = new SolidBrush(this.item.BackColor);
			this.Graphics.FillRectangle(brush, this.bounds);
			brush.Dispose();
		}

		// Token: 0x06003ACD RID: 15053 RVA: 0x000D5734 File Offset: 0x000D4734
		public void DrawFocusRectangle()
		{
			if ((this.state & ListViewItemStates.Focused) == ListViewItemStates.Focused)
			{
				Rectangle rectangle = this.bounds;
				ControlPaint.DrawFocusRectangle(this.graphics, this.UpdateBounds(rectangle, false), this.item.ForeColor, this.item.BackColor);
			}
		}

		// Token: 0x06003ACE RID: 15054 RVA: 0x000D577E File Offset: 0x000D477E
		public void DrawText()
		{
			this.DrawText(TextFormatFlags.Default);
		}

		// Token: 0x06003ACF RID: 15055 RVA: 0x000D5787 File Offset: 0x000D4787
		public void DrawText(TextFormatFlags flags)
		{
			TextRenderer.DrawText(this.graphics, this.item.Text, this.item.Font, this.UpdateBounds(this.bounds, true), this.item.ForeColor, flags);
		}

		// Token: 0x06003AD0 RID: 15056 RVA: 0x000D57C4 File Offset: 0x000D47C4
		private Rectangle UpdateBounds(Rectangle originalBounds, bool drawText)
		{
			Rectangle rectangle = originalBounds;
			if (this.item.ListView.View == View.Details)
			{
				if (!this.item.ListView.FullRowSelect && this.item.SubItems.Count > 0)
				{
					ListViewItem.ListViewSubItem listViewSubItem = this.item.SubItems[0];
					Size size = TextRenderer.MeasureText(listViewSubItem.Text, listViewSubItem.Font);
					rectangle = new Rectangle(originalBounds.X, originalBounds.Y, size.Width, size.Height);
					rectangle.X += 4;
					rectangle.Width++;
				}
				else
				{
					rectangle.X += 4;
					rectangle.Width -= 4;
				}
				if (drawText)
				{
					rectangle.X--;
				}
			}
			return rectangle;
		}

		// Token: 0x04001D58 RID: 7512
		private readonly Graphics graphics;

		// Token: 0x04001D59 RID: 7513
		private readonly ListViewItem item;

		// Token: 0x04001D5A RID: 7514
		private readonly Rectangle bounds;

		// Token: 0x04001D5B RID: 7515
		private readonly int itemIndex;

		// Token: 0x04001D5C RID: 7516
		private readonly ListViewItemStates state;

		// Token: 0x04001D5D RID: 7517
		private bool drawDefault;
	}
}
