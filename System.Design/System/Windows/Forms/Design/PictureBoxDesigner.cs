using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms.Design
{
	internal class PictureBoxDesigner : ControlDesigner
	{
		public PictureBoxDesigner()
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
				color = ControlPaint.Light(control.BackColor);
			}
			else
			{
				color = ControlPaint.Dark(control.BackColor);
			}
			Pen pen = new Pen(color);
			pen.DashStyle = DashStyle.Dash;
			clientRectangle.Width--;
			clientRectangle.Height--;
			graphics.DrawRectangle(pen, clientRectangle);
			pen.Dispose();
		}

		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			PictureBox pictureBox = (PictureBox)base.Component;
			if (pictureBox.BorderStyle == BorderStyle.None)
			{
				this.DrawBorder(pe.Graphics);
			}
			base.OnPaintAdornments(pe);
		}

		public override SelectionRules SelectionRules
		{
			get
			{
				SelectionRules selectionRules = base.SelectionRules;
				object component = base.Component;
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["SizeMode"];
				if (propertyDescriptor != null)
				{
					PictureBoxSizeMode pictureBoxSizeMode = (PictureBoxSizeMode)propertyDescriptor.GetValue(component);
					if (pictureBoxSizeMode == PictureBoxSizeMode.AutoSize)
					{
						selectionRules &= ~(SelectionRules.TopSizeable | SelectionRules.BottomSizeable | SelectionRules.LeftSizeable | SelectionRules.RightSizeable);
					}
				}
				return selectionRules;
			}
		}

		public override DesignerActionListCollection ActionLists
		{
			get
			{
				if (this._actionLists == null)
				{
					this._actionLists = new DesignerActionListCollection();
					this._actionLists.Add(new PictureBoxActionList(this));
				}
				return this._actionLists;
			}
		}

		private DesignerActionListCollection _actionLists;
	}
}
