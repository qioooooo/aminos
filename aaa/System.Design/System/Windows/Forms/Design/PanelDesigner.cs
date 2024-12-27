using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000233 RID: 563
	internal class PanelDesigner : ScrollableControlDesigner
	{
		// Token: 0x0600156F RID: 5487 RVA: 0x0006F5E6 File Offset: 0x0006E5E6
		public PanelDesigner()
		{
			base.AutoResizeHandles = true;
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x0006F5F8 File Offset: 0x0006E5F8
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

		// Token: 0x06001571 RID: 5489 RVA: 0x0006F65C File Offset: 0x0006E65C
		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			Panel panel = (Panel)base.Component;
			if (panel.BorderStyle == BorderStyle.None)
			{
				this.DrawBorder(pe.Graphics);
			}
			base.OnPaintAdornments(pe);
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06001572 RID: 5490 RVA: 0x0006F690 File Offset: 0x0006E690
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
