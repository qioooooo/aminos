using System;
using System.Drawing;
using System.Globalization;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005BC RID: 1468
	internal sealed class HyperLinkStyle : Style
	{
		// Token: 0x060047CC RID: 18380 RVA: 0x00125683 File Offset: 0x00124683
		public HyperLinkStyle(Style owner)
		{
			this._owner = owner;
		}

		// Token: 0x170011B3 RID: 4531
		// (get) Token: 0x060047CD RID: 18381 RVA: 0x00125692 File Offset: 0x00124692
		// (set) Token: 0x060047CE RID: 18382 RVA: 0x0012569A File Offset: 0x0012469A
		public bool DoNotRenderDefaults
		{
			get
			{
				return this._doNotRenderDefaults;
			}
			set
			{
				this._doNotRenderDefaults = value;
			}
		}

		// Token: 0x170011B4 RID: 4532
		// (get) Token: 0x060047CF RID: 18383 RVA: 0x001256A4 File Offset: 0x001246A4
		public sealed override bool IsEmpty
		{
			get
			{
				return base.RegisteredCssClass.Length == 0 && (!this._owner.IsSet(2) && !this._owner.IsSet(4) && !this._owner.IsSet(512) && !this._owner.IsSet(1024) && !this._owner.IsSet(2048) && !this._owner.IsSet(4096) && !this._owner.IsSet(8192) && !this._owner.IsSet(16384)) && !this._owner.IsSet(32768);
			}
		}

		// Token: 0x060047D0 RID: 18384 RVA: 0x00125768 File Offset: 0x00124768
		public sealed override void AddAttributesToRender(HtmlTextWriter writer, WebControl owner)
		{
			string text = string.Empty;
			bool flag = true;
			if (this._owner.IsSet(2))
			{
				text = this._owner.CssClass;
			}
			if (base.RegisteredCssClass.Length != 0)
			{
				flag = false;
				if (text.Length != 0)
				{
					text = text + " " + base.RegisteredCssClass;
				}
				else
				{
					text = base.RegisteredCssClass;
				}
			}
			if (text.Length > 0)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, text);
			}
			if (flag)
			{
				CssStyleCollection styleAttributes = base.GetStyleAttributes(owner);
				styleAttributes.Render(writer);
			}
		}

		// Token: 0x060047D1 RID: 18385 RVA: 0x001257F0 File Offset: 0x001247F0
		protected sealed override void FillStyleAttributes(CssStyleCollection attributes, IUrlResolutionService urlResolver)
		{
			StateBag viewState = base.ViewState;
			if (this._owner.IsSet(4))
			{
				Color foreColor = this._owner.ForeColor;
				if (!foreColor.IsEmpty)
				{
					attributes.Add(HtmlTextWriterStyle.Color, ColorTranslator.ToHtml(foreColor));
				}
			}
			FontInfo font = this._owner.Font;
			string[] names = font.Names;
			if (names.Length > 0)
			{
				attributes.Add(HtmlTextWriterStyle.FontFamily, string.Join(",", names));
			}
			FontUnit size = font.Size;
			if (!size.IsEmpty)
			{
				attributes.Add(HtmlTextWriterStyle.FontSize, size.ToString(CultureInfo.InvariantCulture));
			}
			if (this._owner.IsSet(2048))
			{
				if (font.Bold)
				{
					attributes.Add(HtmlTextWriterStyle.FontWeight, "bold");
				}
				else
				{
					attributes.Add(HtmlTextWriterStyle.FontWeight, "normal");
				}
			}
			if (this._owner.IsSet(4096))
			{
				if (font.Italic)
				{
					attributes.Add(HtmlTextWriterStyle.FontStyle, "italic");
				}
				else
				{
					attributes.Add(HtmlTextWriterStyle.FontStyle, "normal");
				}
			}
			string text = string.Empty;
			if (font.Underline)
			{
				text = "underline";
			}
			if (font.Overline)
			{
				text += " overline";
			}
			if (font.Strikeout)
			{
				text += " line-through";
			}
			if (text.Length > 0)
			{
				attributes.Add(HtmlTextWriterStyle.TextDecoration, text);
			}
			else if (!this.DoNotRenderDefaults)
			{
				attributes.Add(HtmlTextWriterStyle.TextDecoration, "none");
			}
			if (this._owner.IsSet(2))
			{
				attributes.Add(HtmlTextWriterStyle.BorderStyle, "none");
			}
		}

		// Token: 0x04002AAE RID: 10926
		private bool _doNotRenderDefaults;

		// Token: 0x04002AAF RID: 10927
		private Style _owner;
	}
}
