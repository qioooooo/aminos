using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms.Design
{
	internal class SplitterDesigner : ControlDesigner
	{
		public SplitterDesigner()
		{
			base.AutoResizeHandles = true;
		}

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

		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			Splitter splitter = (Splitter)base.Component;
			base.OnPaintAdornments(pe);
			if (splitter.BorderStyle == BorderStyle.None)
			{
				this.DrawBorder(pe.Graphics);
			}
		}

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
