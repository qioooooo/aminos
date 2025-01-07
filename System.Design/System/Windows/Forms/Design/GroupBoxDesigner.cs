using System;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	internal class GroupBoxDesigner : ParentControlDesigner
	{
		protected override Point DefaultControlLocation
		{
			get
			{
				GroupBox groupBox = (GroupBox)this.Control;
				return new Point(groupBox.DisplayRectangle.X, groupBox.DisplayRectangle.Y);
			}
		}

		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			if (this.DrawGrid)
			{
				Control control = this.Control;
				Rectangle displayRectangle = this.Control.DisplayRectangle;
				displayRectangle.Width++;
				displayRectangle.Height++;
				ControlPaint.DrawGrid(pe.Graphics, displayRectangle, base.GridSize, control.BackColor);
			}
			if (base.Inherited)
			{
				if (this.inheritanceUI == null)
				{
					this.inheritanceUI = (InheritanceUI)this.GetService(typeof(InheritanceUI));
				}
				if (this.inheritanceUI != null)
				{
					pe.Graphics.DrawImage(this.inheritanceUI.InheritanceGlyph, 0, 0);
				}
			}
		}

		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 132)
			{
				base.WndProc(ref m);
				if ((int)m.Result == -1)
				{
					m.Result = (IntPtr)1;
					return;
				}
			}
			else
			{
				base.WndProc(ref m);
			}
		}

		private InheritanceUI inheritanceUI;
	}
}
