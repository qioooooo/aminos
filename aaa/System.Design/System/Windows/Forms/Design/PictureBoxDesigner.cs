using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200027A RID: 634
	internal class PictureBoxDesigner : ControlDesigner
	{
		// Token: 0x060017B6 RID: 6070 RVA: 0x0007B847 File Offset: 0x0007A847
		public PictureBoxDesigner()
		{
			base.AutoResizeHandles = true;
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x0007B858 File Offset: 0x0007A858
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

		// Token: 0x060017B8 RID: 6072 RVA: 0x0007B8E4 File Offset: 0x0007A8E4
		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			PictureBox pictureBox = (PictureBox)base.Component;
			if (pictureBox.BorderStyle == BorderStyle.None)
			{
				this.DrawBorder(pe.Graphics);
			}
			base.OnPaintAdornments(pe);
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x060017B9 RID: 6073 RVA: 0x0007B918 File Offset: 0x0007A918
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

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x060017BA RID: 6074 RVA: 0x0007B963 File Offset: 0x0007A963
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

		// Token: 0x040013AB RID: 5035
		private DesignerActionListCollection _actionLists;
	}
}
