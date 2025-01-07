using System;
using System.ComponentModel;
using System.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	internal class InheritanceUI
	{
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

		public void Dispose()
		{
			if (this.tooltip != null)
			{
				this.tooltip.Dispose();
			}
		}

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

		private static Bitmap inheritanceGlyph;

		private static Rectangle inheritanceGlyphRect;

		private ToolTip tooltip;
	}
}
