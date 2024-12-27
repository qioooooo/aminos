using System;
using System.ComponentModel;
using System.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000254 RID: 596
	internal class InheritanceUI
	{
		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x060016B6 RID: 5814 RVA: 0x000757A7 File Offset: 0x000747A7
		public Bitmap InheritanceGlyph
		{
			get
			{
				if (InheritanceUI.inheritanceGlyph == null)
				{
					InheritanceUI.inheritanceGlyph = new Bitmap(typeof(InheritanceUI), "InheritedGlyph.bmp");
					InheritanceUI.inheritanceGlyph.MakeTransparent();
				}
				return InheritanceUI.inheritanceGlyph;
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x060016B7 RID: 5815 RVA: 0x000757D8 File Offset: 0x000747D8
		public Rectangle InheritanceGlyphRectangle
		{
			get
			{
				if (InheritanceUI.inheritanceGlyphRect == Rectangle.Empty)
				{
					Size size = this.InheritanceGlyph.Size;
					InheritanceUI.inheritanceGlyphRect = new Rectangle(0, 0, size.Width, size.Height);
				}
				return InheritanceUI.inheritanceGlyphRect;
			}
		}

		// Token: 0x060016B8 RID: 5816 RVA: 0x00075824 File Offset: 0x00074824
		public void AddInheritedControl(Control c, InheritanceLevel level)
		{
			if (this.tooltip == null)
			{
				this.tooltip = new ToolTip();
				this.tooltip.ShowAlways = true;
			}
			string text;
			if (level == InheritanceLevel.InheritedReadOnly)
			{
				text = SR.GetString("DesignerInheritedReadOnly");
			}
			else
			{
				text = SR.GetString("DesignerInherited");
			}
			this.tooltip.SetToolTip(c, text);
			foreach (object obj in c.Controls)
			{
				Control control = (Control)obj;
				if (control.Site == null)
				{
					this.tooltip.SetToolTip(control, text);
				}
			}
		}

		// Token: 0x060016B9 RID: 5817 RVA: 0x000758D4 File Offset: 0x000748D4
		public void Dispose()
		{
			if (this.tooltip != null)
			{
				this.tooltip.Dispose();
			}
		}

		// Token: 0x060016BA RID: 5818 RVA: 0x000758EC File Offset: 0x000748EC
		public void RemoveInheritedControl(Control c)
		{
			if (this.tooltip != null && this.tooltip.GetToolTip(c).Length > 0)
			{
				this.tooltip.SetToolTip(c, null);
				foreach (object obj in c.Controls)
				{
					Control control = (Control)obj;
					if (control.Site == null)
					{
						this.tooltip.SetToolTip(control, null);
					}
				}
			}
		}

		// Token: 0x040012F8 RID: 4856
		private static Bitmap inheritanceGlyph;

		// Token: 0x040012F9 RID: 4857
		private static Rectangle inheritanceGlyphRect;

		// Token: 0x040012FA RID: 4858
		private ToolTip tooltip;
	}
}
