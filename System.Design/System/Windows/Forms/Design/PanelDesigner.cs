using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms.Design
{
	internal class PanelDesigner : ScrollableControlDesigner
	{
		public PanelDesigner()
		{
			base.AutoResizeHandles = true;
		}

		protected virtual void DrawBorder(Graphics graphics)
		{
			Panel panel = (Panel)base.Component;
			if (panel == null || !panel.Visible)
			{
				return;
			}
			Pen borderPen = this.BorderPen;
			Rectangle clientRectangle = this.Control.ClientRectangle;
			clientRectangle.Width--;
			clientRectangle.Height--;
			graphics.DrawRectangle(borderPen, clientRectangle);
			borderPen.Dispose();
		}

		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			Panel panel = (Panel)base.Component;
			if (panel.BorderStyle == BorderStyle.None)
			{
				this.DrawBorder(pe.Graphics);
			}
			base.OnPaintAdornments(pe);
		}

		protected Pen BorderPen
		{
			get
			{
				Color color = (((double)this.Control.BackColor.GetBrightness() < 0.5) ? ControlPaint.Light(this.Control.BackColor) : ControlPaint.Dark(this.Control.BackColor));
				return new Pen(color)
				{
					DashStyle = DashStyle.Dash
				};
			}
		}
	}
}
