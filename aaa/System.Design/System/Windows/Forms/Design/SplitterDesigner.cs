using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200028F RID: 655
	internal class SplitterDesigner : ControlDesigner
	{
		// Token: 0x0600185C RID: 6236 RVA: 0x0008005B File Offset: 0x0007F05B
		public SplitterDesigner()
		{
			base.AutoResizeHandles = true;
		}

		// Token: 0x0600185D RID: 6237 RVA: 0x0008006C File Offset: 0x0007F06C
		private void DrawBorder(Graphics graphics)
		{
			Control control = this.Control;
			Rectangle clientRectangle = control.ClientRectangle;
			Color color;
			if ((double)control.BackColor.GetBrightness() < 0.5)
			{
				color = Color.White;
			}
			else
			{
				color = Color.Black;
			}
			using (Pen pen = new Pen(color))
			{
				pen.DashStyle = DashStyle.Dash;
				clientRectangle.Width--;
				clientRectangle.Height--;
				graphics.DrawRectangle(pen, clientRectangle);
			}
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x00080100 File Offset: 0x0007F100
		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			Splitter splitter = (Splitter)base.Component;
			base.OnPaintAdornments(pe);
			if (splitter.BorderStyle == BorderStyle.None)
			{
				this.DrawBorder(pe.Graphics);
			}
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x00080134 File Offset: 0x0007F134
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 71)
			{
				Control control = this.Control;
				control.Invalidate();
			}
			base.WndProc(ref m);
		}
	}
}
