using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Drawing.Design
{
	// Token: 0x02000018 RID: 24
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class FontNameEditor : UITypeEditor
	{
		// Token: 0x0600009D RID: 157 RVA: 0x000056DB File Offset: 0x000046DB
		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000056E0 File Offset: 0x000046E0
		public override void PaintValue(PaintValueEventArgs e)
		{
			string text = e.Value as string;
			if (text != null)
			{
				if (text == "")
				{
					return;
				}
				e.Graphics.FillRectangle(SystemBrushes.ActiveCaption, e.Bounds);
				FontFamily fontFamily = null;
				try
				{
					fontFamily = new FontFamily(text);
				}
				catch
				{
				}
				if (fontFamily != null)
				{
					try
					{
						FontNameEditor.DrawFontSample(e, fontFamily, FontStyle.Regular);
					}
					catch
					{
						try
						{
							FontNameEditor.DrawFontSample(e, fontFamily, FontStyle.Italic);
						}
						catch
						{
							try
							{
								FontNameEditor.DrawFontSample(e, fontFamily, FontStyle.Bold);
							}
							catch
							{
								try
								{
									FontNameEditor.DrawFontSample(e, fontFamily, FontStyle.Bold | FontStyle.Italic);
								}
								catch
								{
								}
							}
						}
					}
				}
				e.Graphics.DrawLine(SystemPens.WindowFrame, e.Bounds.Right, e.Bounds.Y, e.Bounds.Right, e.Bounds.Bottom);
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000057F4 File Offset: 0x000047F4
		private static void DrawFontSample(PaintValueEventArgs e, FontFamily fontFamily, FontStyle fontStyle)
		{
			float num = (float)((double)e.Bounds.Height / 1.2);
			Font font = new Font(fontFamily, num, fontStyle, GraphicsUnit.Pixel);
			if (font == null)
			{
				return;
			}
			try
			{
				e.Graphics.DrawString("abcd", font, SystemBrushes.ActiveCaptionText, e.Bounds);
			}
			finally
			{
				font.Dispose();
			}
		}
	}
}
