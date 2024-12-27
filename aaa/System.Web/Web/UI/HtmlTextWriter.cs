using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x020003A0 RID: 928
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HtmlTextWriter : TextWriter
	{
		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x06002D41 RID: 11585 RVA: 0x000CA8FB File Offset: 0x000C98FB
		internal virtual bool RenderDivAroundHiddenInputs
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x000CA8FE File Offset: 0x000C98FE
		public virtual void EnterStyle(Style style, HtmlTextWriterTag tag)
		{
			if (!style.IsEmpty || tag != HtmlTextWriterTag.Span)
			{
				style.AddAttributesToRender(this);
				this.RenderBeginTag(tag);
			}
		}

		// Token: 0x06002D43 RID: 11587 RVA: 0x000CA91B File Offset: 0x000C991B
		public virtual void ExitStyle(Style style, HtmlTextWriterTag tag)
		{
			if (!style.IsEmpty || tag != HtmlTextWriterTag.Span)
			{
				this.RenderEndTag();
			}
		}

		// Token: 0x06002D44 RID: 11588 RVA: 0x000CA930 File Offset: 0x000C9930
		internal virtual void OpenDiv()
		{
			this.OpenDiv(this._currentLayout, this._currentLayout != null && this._currentLayout.Align != HorizontalAlign.NotSet, this._currentLayout != null && !this._currentLayout.Wrap);
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x000CA980 File Offset: 0x000C9980
		private void OpenDiv(HtmlTextWriter.Layout layout, bool writeHorizontalAlign, bool writeWrapping)
		{
			this.WriteBeginTag("div");
			if (writeHorizontalAlign)
			{
				string text;
				switch (layout.Align)
				{
				case HorizontalAlign.Center:
					text = "text-align:center";
					break;
				case HorizontalAlign.Right:
					text = "text-align:right";
					break;
				default:
					text = "text-align:left";
					break;
				}
				this.WriteAttribute("style", text);
			}
			if (writeWrapping)
			{
				this.WriteAttribute("mode", layout.Wrap ? "wrap" : "nowrap");
			}
			this.Write('>');
			this._currentWrittenLayout = layout;
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x000CAA07 File Offset: 0x000C9A07
		public virtual bool IsValidFormAttribute(string attribute)
		{
			return true;
		}

		// Token: 0x06002D47 RID: 11591 RVA: 0x000CAA0C File Offset: 0x000C9A0C
		static HtmlTextWriter()
		{
			HtmlTextWriter.RegisterTag(string.Empty, HtmlTextWriterTag.Unknown, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("a", HtmlTextWriterTag.A, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("acronym", HtmlTextWriterTag.Acronym, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("address", HtmlTextWriterTag.Address, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("area", HtmlTextWriterTag.Area, HtmlTextWriter.TagType.NonClosing);
			HtmlTextWriter.RegisterTag("b", HtmlTextWriterTag.B, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("base", HtmlTextWriterTag.Base, HtmlTextWriter.TagType.NonClosing);
			HtmlTextWriter.RegisterTag("basefont", HtmlTextWriterTag.Basefont, HtmlTextWriter.TagType.NonClosing);
			HtmlTextWriter.RegisterTag("bdo", HtmlTextWriterTag.Bdo, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("bgsound", HtmlTextWriterTag.Bgsound, HtmlTextWriter.TagType.NonClosing);
			HtmlTextWriter.RegisterTag("big", HtmlTextWriterTag.Big, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("blockquote", HtmlTextWriterTag.Blockquote, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("body", HtmlTextWriterTag.Body, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("br", HtmlTextWriterTag.Br, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("button", HtmlTextWriterTag.Button, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("caption", HtmlTextWriterTag.Caption, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("center", HtmlTextWriterTag.Center, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("cite", HtmlTextWriterTag.Cite, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("code", HtmlTextWriterTag.Code, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("col", HtmlTextWriterTag.Col, HtmlTextWriter.TagType.NonClosing);
			HtmlTextWriter.RegisterTag("colgroup", HtmlTextWriterTag.Colgroup, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("del", HtmlTextWriterTag.Del, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("dd", HtmlTextWriterTag.Dd, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("dfn", HtmlTextWriterTag.Dfn, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("dir", HtmlTextWriterTag.Dir, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("div", HtmlTextWriterTag.Div, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("dl", HtmlTextWriterTag.Dl, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("dt", HtmlTextWriterTag.Dt, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("em", HtmlTextWriterTag.Em, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("embed", HtmlTextWriterTag.Embed, HtmlTextWriter.TagType.NonClosing);
			HtmlTextWriter.RegisterTag("fieldset", HtmlTextWriterTag.Fieldset, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("font", HtmlTextWriterTag.Font, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("form", HtmlTextWriterTag.Form, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("frame", HtmlTextWriterTag.Frame, HtmlTextWriter.TagType.NonClosing);
			HtmlTextWriter.RegisterTag("frameset", HtmlTextWriterTag.Frameset, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("h1", HtmlTextWriterTag.H1, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("h2", HtmlTextWriterTag.H2, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("h3", HtmlTextWriterTag.H3, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("h4", HtmlTextWriterTag.H4, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("h5", HtmlTextWriterTag.H5, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("h6", HtmlTextWriterTag.H6, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("head", HtmlTextWriterTag.Head, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("hr", HtmlTextWriterTag.Hr, HtmlTextWriter.TagType.NonClosing);
			HtmlTextWriter.RegisterTag("html", HtmlTextWriterTag.Html, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("i", HtmlTextWriterTag.I, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("iframe", HtmlTextWriterTag.Iframe, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("img", HtmlTextWriterTag.Img, HtmlTextWriter.TagType.NonClosing);
			HtmlTextWriter.RegisterTag("input", HtmlTextWriterTag.Input, HtmlTextWriter.TagType.NonClosing);
			HtmlTextWriter.RegisterTag("ins", HtmlTextWriterTag.Ins, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("isindex", HtmlTextWriterTag.Isindex, HtmlTextWriter.TagType.NonClosing);
			HtmlTextWriter.RegisterTag("kbd", HtmlTextWriterTag.Kbd, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("label", HtmlTextWriterTag.Label, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("legend", HtmlTextWriterTag.Legend, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("li", HtmlTextWriterTag.Li, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("link", HtmlTextWriterTag.Link, HtmlTextWriter.TagType.NonClosing);
			HtmlTextWriter.RegisterTag("map", HtmlTextWriterTag.Map, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("marquee", HtmlTextWriterTag.Marquee, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("menu", HtmlTextWriterTag.Menu, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("meta", HtmlTextWriterTag.Meta, HtmlTextWriter.TagType.NonClosing);
			HtmlTextWriter.RegisterTag("nobr", HtmlTextWriterTag.Nobr, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("noframes", HtmlTextWriterTag.Noframes, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("noscript", HtmlTextWriterTag.Noscript, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("object", HtmlTextWriterTag.Object, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("ol", HtmlTextWriterTag.Ol, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("option", HtmlTextWriterTag.Option, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("p", HtmlTextWriterTag.P, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("param", HtmlTextWriterTag.Param, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("pre", HtmlTextWriterTag.Pre, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("ruby", HtmlTextWriterTag.Ruby, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("rt", HtmlTextWriterTag.Rt, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("q", HtmlTextWriterTag.Q, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("s", HtmlTextWriterTag.S, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("samp", HtmlTextWriterTag.Samp, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("script", HtmlTextWriterTag.Script, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("select", HtmlTextWriterTag.Select, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("small", HtmlTextWriterTag.Small, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("span", HtmlTextWriterTag.Span, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("strike", HtmlTextWriterTag.Strike, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("strong", HtmlTextWriterTag.Strong, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("style", HtmlTextWriterTag.Style, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("sub", HtmlTextWriterTag.Sub, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("sup", HtmlTextWriterTag.Sup, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("table", HtmlTextWriterTag.Table, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("tbody", HtmlTextWriterTag.Tbody, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("td", HtmlTextWriterTag.Td, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("textarea", HtmlTextWriterTag.Textarea, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("tfoot", HtmlTextWriterTag.Tfoot, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("th", HtmlTextWriterTag.Th, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("thead", HtmlTextWriterTag.Thead, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("title", HtmlTextWriterTag.Title, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("tr", HtmlTextWriterTag.Tr, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("tt", HtmlTextWriterTag.Tt, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("u", HtmlTextWriterTag.U, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("ul", HtmlTextWriterTag.Ul, HtmlTextWriter.TagType.Other);
			HtmlTextWriter.RegisterTag("var", HtmlTextWriterTag.Var, HtmlTextWriter.TagType.Inline);
			HtmlTextWriter.RegisterTag("wbr", HtmlTextWriterTag.Wbr, HtmlTextWriter.TagType.NonClosing);
			HtmlTextWriter.RegisterTag("xml", HtmlTextWriterTag.Xml, HtmlTextWriter.TagType.Other);
			HtmlTextWriter._attrKeyLookupTable = new Hashtable(54);
			HtmlTextWriter._attrNameLookupArray = new HtmlTextWriter.AttributeInformation[54];
			HtmlTextWriter.RegisterAttribute("abbr", HtmlTextWriterAttribute.Abbr, true);
			HtmlTextWriter.RegisterAttribute("accesskey", HtmlTextWriterAttribute.Accesskey, true);
			HtmlTextWriter.RegisterAttribute("align", HtmlTextWriterAttribute.Align, false);
			HtmlTextWriter.RegisterAttribute("alt", HtmlTextWriterAttribute.Alt, true);
			HtmlTextWriter.RegisterAttribute("autocomplete", HtmlTextWriterAttribute.AutoComplete, false);
			HtmlTextWriter.RegisterAttribute("axis", HtmlTextWriterAttribute.Axis, true);
			HtmlTextWriter.RegisterAttribute("background", HtmlTextWriterAttribute.Background, true, true);
			HtmlTextWriter.RegisterAttribute("bgcolor", HtmlTextWriterAttribute.Bgcolor, false);
			HtmlTextWriter.RegisterAttribute("border", HtmlTextWriterAttribute.Border, false);
			HtmlTextWriter.RegisterAttribute("bordercolor", HtmlTextWriterAttribute.Bordercolor, false);
			HtmlTextWriter.RegisterAttribute("cellpadding", HtmlTextWriterAttribute.Cellpadding, false);
			HtmlTextWriter.RegisterAttribute("cellspacing", HtmlTextWriterAttribute.Cellspacing, false);
			HtmlTextWriter.RegisterAttribute("checked", HtmlTextWriterAttribute.Checked, false);
			HtmlTextWriter.RegisterAttribute("class", HtmlTextWriterAttribute.Class, true);
			HtmlTextWriter.RegisterAttribute("cols", HtmlTextWriterAttribute.Cols, false);
			HtmlTextWriter.RegisterAttribute("colspan", HtmlTextWriterAttribute.Colspan, false);
			HtmlTextWriter.RegisterAttribute("content", HtmlTextWriterAttribute.Content, true);
			HtmlTextWriter.RegisterAttribute("coords", HtmlTextWriterAttribute.Coords, false);
			HtmlTextWriter.RegisterAttribute("dir", HtmlTextWriterAttribute.Dir, false);
			HtmlTextWriter.RegisterAttribute("disabled", HtmlTextWriterAttribute.Disabled, false);
			HtmlTextWriter.RegisterAttribute("for", HtmlTextWriterAttribute.For, false);
			HtmlTextWriter.RegisterAttribute("headers", HtmlTextWriterAttribute.Headers, true);
			HtmlTextWriter.RegisterAttribute("height", HtmlTextWriterAttribute.Height, false);
			HtmlTextWriter.RegisterAttribute("href", HtmlTextWriterAttribute.Href, true, true);
			HtmlTextWriter.RegisterAttribute("id", HtmlTextWriterAttribute.Id, false);
			HtmlTextWriter.RegisterAttribute("longdesc", HtmlTextWriterAttribute.Longdesc, true, true);
			HtmlTextWriter.RegisterAttribute("maxlength", HtmlTextWriterAttribute.Maxlength, false);
			HtmlTextWriter.RegisterAttribute("multiple", HtmlTextWriterAttribute.Multiple, false);
			HtmlTextWriter.RegisterAttribute("name", HtmlTextWriterAttribute.Name, false);
			HtmlTextWriter.RegisterAttribute("nowrap", HtmlTextWriterAttribute.Nowrap, false);
			HtmlTextWriter.RegisterAttribute("onclick", HtmlTextWriterAttribute.Onclick, true);
			HtmlTextWriter.RegisterAttribute("onchange", HtmlTextWriterAttribute.Onchange, true);
			HtmlTextWriter.RegisterAttribute("readonly", HtmlTextWriterAttribute.ReadOnly, false);
			HtmlTextWriter.RegisterAttribute("rel", HtmlTextWriterAttribute.Rel, false);
			HtmlTextWriter.RegisterAttribute("rows", HtmlTextWriterAttribute.Rows, false);
			HtmlTextWriter.RegisterAttribute("rowspan", HtmlTextWriterAttribute.Rowspan, false);
			HtmlTextWriter.RegisterAttribute("rules", HtmlTextWriterAttribute.Rules, false);
			HtmlTextWriter.RegisterAttribute("scope", HtmlTextWriterAttribute.Scope, false);
			HtmlTextWriter.RegisterAttribute("selected", HtmlTextWriterAttribute.Selected, false);
			HtmlTextWriter.RegisterAttribute("shape", HtmlTextWriterAttribute.Shape, false);
			HtmlTextWriter.RegisterAttribute("size", HtmlTextWriterAttribute.Size, false);
			HtmlTextWriter.RegisterAttribute("src", HtmlTextWriterAttribute.Src, true, true);
			HtmlTextWriter.RegisterAttribute("style", HtmlTextWriterAttribute.Style, false);
			HtmlTextWriter.RegisterAttribute("tabindex", HtmlTextWriterAttribute.Tabindex, false);
			HtmlTextWriter.RegisterAttribute("target", HtmlTextWriterAttribute.Target, false);
			HtmlTextWriter.RegisterAttribute("title", HtmlTextWriterAttribute.Title, true);
			HtmlTextWriter.RegisterAttribute("type", HtmlTextWriterAttribute.Type, false);
			HtmlTextWriter.RegisterAttribute("usemap", HtmlTextWriterAttribute.Usemap, false);
			HtmlTextWriter.RegisterAttribute("valign", HtmlTextWriterAttribute.Valign, false);
			HtmlTextWriter.RegisterAttribute("value", HtmlTextWriterAttribute.Value, true);
			HtmlTextWriter.RegisterAttribute("vcard_name", HtmlTextWriterAttribute.VCardName, false);
			HtmlTextWriter.RegisterAttribute("width", HtmlTextWriterAttribute.Width, false);
			HtmlTextWriter.RegisterAttribute("wrap", HtmlTextWriterAttribute.Wrap, false);
			HtmlTextWriter.RegisterAttribute("_designerRegion", HtmlTextWriterAttribute.DesignerRegion, false);
		}

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x06002D48 RID: 11592 RVA: 0x000CB1E6 File Offset: 0x000CA1E6
		public override Encoding Encoding
		{
			get
			{
				return this.writer.Encoding;
			}
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x06002D49 RID: 11593 RVA: 0x000CB1F3 File Offset: 0x000CA1F3
		// (set) Token: 0x06002D4A RID: 11594 RVA: 0x000CB200 File Offset: 0x000CA200
		public override string NewLine
		{
			get
			{
				return this.writer.NewLine;
			}
			set
			{
				this.writer.NewLine = value;
			}
		}

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x06002D4B RID: 11595 RVA: 0x000CB20E File Offset: 0x000CA20E
		// (set) Token: 0x06002D4C RID: 11596 RVA: 0x000CB216 File Offset: 0x000CA216
		public int Indent
		{
			get
			{
				return this.indentLevel;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				this.indentLevel = value;
			}
		}

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x06002D4D RID: 11597 RVA: 0x000CB226 File Offset: 0x000CA226
		// (set) Token: 0x06002D4E RID: 11598 RVA: 0x000CB22E File Offset: 0x000CA22E
		public TextWriter InnerWriter
		{
			get
			{
				return this.writer;
			}
			set
			{
				this.writer = value;
				this._httpWriter = value as HttpWriter;
			}
		}

		// Token: 0x06002D4F RID: 11599 RVA: 0x000CB243 File Offset: 0x000CA243
		public virtual void BeginRender()
		{
		}

		// Token: 0x06002D50 RID: 11600 RVA: 0x000CB245 File Offset: 0x000CA245
		public override void Close()
		{
			this.writer.Close();
		}

		// Token: 0x06002D51 RID: 11601 RVA: 0x000CB252 File Offset: 0x000CA252
		public virtual void EndRender()
		{
		}

		// Token: 0x06002D52 RID: 11602 RVA: 0x000CB254 File Offset: 0x000CA254
		public virtual void EnterStyle(Style style)
		{
			this.EnterStyle(style, HtmlTextWriterTag.Span);
		}

		// Token: 0x06002D53 RID: 11603 RVA: 0x000CB25F File Offset: 0x000CA25F
		public virtual void ExitStyle(Style style)
		{
			this.ExitStyle(style, HtmlTextWriterTag.Span);
		}

		// Token: 0x06002D54 RID: 11604 RVA: 0x000CB26A File Offset: 0x000CA26A
		public override void Flush()
		{
			this.writer.Flush();
		}

		// Token: 0x06002D55 RID: 11605 RVA: 0x000CB278 File Offset: 0x000CA278
		protected virtual void OutputTabs()
		{
			if (this.tabsPending)
			{
				for (int i = 0; i < this.indentLevel; i++)
				{
					this.writer.Write(this.tabString);
				}
				this.tabsPending = false;
			}
		}

		// Token: 0x06002D56 RID: 11606 RVA: 0x000CB2B6 File Offset: 0x000CA2B6
		public override void Write(string s)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.Write(s);
		}

		// Token: 0x06002D57 RID: 11607 RVA: 0x000CB2D2 File Offset: 0x000CA2D2
		public override void Write(bool value)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.Write(value);
		}

		// Token: 0x06002D58 RID: 11608 RVA: 0x000CB2EE File Offset: 0x000CA2EE
		public override void Write(char value)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.Write(value);
		}

		// Token: 0x06002D59 RID: 11609 RVA: 0x000CB30A File Offset: 0x000CA30A
		public override void Write(char[] buffer)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.Write(buffer);
		}

		// Token: 0x06002D5A RID: 11610 RVA: 0x000CB326 File Offset: 0x000CA326
		public override void Write(char[] buffer, int index, int count)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.Write(buffer, index, count);
		}

		// Token: 0x06002D5B RID: 11611 RVA: 0x000CB344 File Offset: 0x000CA344
		public override void Write(double value)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.Write(value);
		}

		// Token: 0x06002D5C RID: 11612 RVA: 0x000CB360 File Offset: 0x000CA360
		public override void Write(float value)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.Write(value);
		}

		// Token: 0x06002D5D RID: 11613 RVA: 0x000CB37C File Offset: 0x000CA37C
		public override void Write(int value)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.Write(value);
		}

		// Token: 0x06002D5E RID: 11614 RVA: 0x000CB398 File Offset: 0x000CA398
		public override void Write(long value)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.Write(value);
		}

		// Token: 0x06002D5F RID: 11615 RVA: 0x000CB3B4 File Offset: 0x000CA3B4
		public override void Write(object value)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.Write(value);
		}

		// Token: 0x06002D60 RID: 11616 RVA: 0x000CB3D0 File Offset: 0x000CA3D0
		public override void Write(string format, object arg0)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.Write(format, arg0);
		}

		// Token: 0x06002D61 RID: 11617 RVA: 0x000CB3ED File Offset: 0x000CA3ED
		public override void Write(string format, object arg0, object arg1)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.Write(format, arg0, arg1);
		}

		// Token: 0x06002D62 RID: 11618 RVA: 0x000CB40B File Offset: 0x000CA40B
		public override void Write(string format, params object[] arg)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.Write(format, arg);
		}

		// Token: 0x06002D63 RID: 11619 RVA: 0x000CB428 File Offset: 0x000CA428
		public void WriteLineNoTabs(string s)
		{
			this.writer.WriteLine(s);
			this.tabsPending = true;
		}

		// Token: 0x06002D64 RID: 11620 RVA: 0x000CB43D File Offset: 0x000CA43D
		public override void WriteLine(string s)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.WriteLine(s);
			this.tabsPending = true;
		}

		// Token: 0x06002D65 RID: 11621 RVA: 0x000CB460 File Offset: 0x000CA460
		public override void WriteLine()
		{
			this.writer.WriteLine();
			this.tabsPending = true;
		}

		// Token: 0x06002D66 RID: 11622 RVA: 0x000CB474 File Offset: 0x000CA474
		public override void WriteLine(bool value)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.WriteLine(value);
			this.tabsPending = true;
		}

		// Token: 0x06002D67 RID: 11623 RVA: 0x000CB497 File Offset: 0x000CA497
		public override void WriteLine(char value)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.WriteLine(value);
			this.tabsPending = true;
		}

		// Token: 0x06002D68 RID: 11624 RVA: 0x000CB4BA File Offset: 0x000CA4BA
		public override void WriteLine(char[] buffer)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.WriteLine(buffer);
			this.tabsPending = true;
		}

		// Token: 0x06002D69 RID: 11625 RVA: 0x000CB4DD File Offset: 0x000CA4DD
		public override void WriteLine(char[] buffer, int index, int count)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.WriteLine(buffer, index, count);
			this.tabsPending = true;
		}

		// Token: 0x06002D6A RID: 11626 RVA: 0x000CB502 File Offset: 0x000CA502
		public override void WriteLine(double value)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.WriteLine(value);
			this.tabsPending = true;
		}

		// Token: 0x06002D6B RID: 11627 RVA: 0x000CB525 File Offset: 0x000CA525
		public override void WriteLine(float value)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.WriteLine(value);
			this.tabsPending = true;
		}

		// Token: 0x06002D6C RID: 11628 RVA: 0x000CB548 File Offset: 0x000CA548
		public override void WriteLine(int value)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.WriteLine(value);
			this.tabsPending = true;
		}

		// Token: 0x06002D6D RID: 11629 RVA: 0x000CB56B File Offset: 0x000CA56B
		public override void WriteLine(long value)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.WriteLine(value);
			this.tabsPending = true;
		}

		// Token: 0x06002D6E RID: 11630 RVA: 0x000CB58E File Offset: 0x000CA58E
		public override void WriteLine(object value)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.WriteLine(value);
			this.tabsPending = true;
		}

		// Token: 0x06002D6F RID: 11631 RVA: 0x000CB5B1 File Offset: 0x000CA5B1
		public override void WriteLine(string format, object arg0)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.WriteLine(format, arg0);
			this.tabsPending = true;
		}

		// Token: 0x06002D70 RID: 11632 RVA: 0x000CB5D5 File Offset: 0x000CA5D5
		public override void WriteLine(string format, object arg0, object arg1)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.WriteLine(format, arg0, arg1);
			this.tabsPending = true;
		}

		// Token: 0x06002D71 RID: 11633 RVA: 0x000CB5FA File Offset: 0x000CA5FA
		public override void WriteLine(string format, params object[] arg)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.WriteLine(format, arg);
			this.tabsPending = true;
		}

		// Token: 0x06002D72 RID: 11634 RVA: 0x000CB61E File Offset: 0x000CA61E
		[CLSCompliant(false)]
		public override void WriteLine(uint value)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.WriteLine(value);
			this.tabsPending = true;
		}

		// Token: 0x06002D73 RID: 11635 RVA: 0x000CB641 File Offset: 0x000CA641
		protected static void RegisterTag(string name, HtmlTextWriterTag key)
		{
			HtmlTextWriter.RegisterTag(name, key, HtmlTextWriter.TagType.Other);
		}

		// Token: 0x06002D74 RID: 11636 RVA: 0x000CB64C File Offset: 0x000CA64C
		private static void RegisterTag(string name, HtmlTextWriterTag key, HtmlTextWriter.TagType type)
		{
			string text = name.ToLower(CultureInfo.InvariantCulture);
			HtmlTextWriter._tagKeyLookupTable.Add(text, key);
			string text2 = null;
			if (type != HtmlTextWriter.TagType.NonClosing && key != HtmlTextWriterTag.Unknown)
			{
				text2 = "</" + text + '>'.ToString(CultureInfo.InvariantCulture);
			}
			if (key < (HtmlTextWriterTag)HtmlTextWriter._tagNameLookupArray.Length)
			{
				HtmlTextWriter._tagNameLookupArray[(int)key] = new HtmlTextWriter.TagInformation(name, type, text2);
			}
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x000CB6BC File Offset: 0x000CA6BC
		protected static void RegisterAttribute(string name, HtmlTextWriterAttribute key)
		{
			HtmlTextWriter.RegisterAttribute(name, key, false);
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x000CB6C6 File Offset: 0x000CA6C6
		private static void RegisterAttribute(string name, HtmlTextWriterAttribute key, bool encode)
		{
			HtmlTextWriter.RegisterAttribute(name, key, encode, false);
		}

		// Token: 0x06002D77 RID: 11639 RVA: 0x000CB6D4 File Offset: 0x000CA6D4
		private static void RegisterAttribute(string name, HtmlTextWriterAttribute key, bool encode, bool isUrl)
		{
			string text = name.ToLower(CultureInfo.InvariantCulture);
			HtmlTextWriter._attrKeyLookupTable.Add(text, key);
			if (key < (HtmlTextWriterAttribute)HtmlTextWriter._attrNameLookupArray.Length)
			{
				HtmlTextWriter._attrNameLookupArray[(int)key] = new HtmlTextWriter.AttributeInformation(name, encode, isUrl);
			}
		}

		// Token: 0x06002D78 RID: 11640 RVA: 0x000CB720 File Offset: 0x000CA720
		protected static void RegisterStyle(string name, HtmlTextWriterStyle key)
		{
			CssTextWriter.RegisterAttribute(name, key);
		}

		// Token: 0x06002D79 RID: 11641 RVA: 0x000CB729 File Offset: 0x000CA729
		public HtmlTextWriter(TextWriter writer)
			: this(writer, "\t")
		{
		}

		// Token: 0x06002D7A RID: 11642 RVA: 0x000CB738 File Offset: 0x000CA738
		public HtmlTextWriter(TextWriter writer, string tabString)
			: base(CultureInfo.InvariantCulture)
		{
			this.writer = writer;
			this.tabString = tabString;
			this.indentLevel = 0;
			this.tabsPending = false;
			this._httpWriter = writer as HttpWriter;
			this._isDescendant = base.GetType() != typeof(HtmlTextWriter);
			this._attrCount = 0;
			this._styleCount = 0;
			this._endTagCount = 0;
			this._inlineCount = 0;
		}

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x06002D7B RID: 11643 RVA: 0x000CB7BC File Offset: 0x000CA7BC
		// (set) Token: 0x06002D7C RID: 11644 RVA: 0x000CB7C4 File Offset: 0x000CA7C4
		protected HtmlTextWriterTag TagKey
		{
			get
			{
				return this._tagKey;
			}
			set
			{
				this._tagIndex = (int)value;
				if (this._tagIndex < 0 || this._tagIndex >= HtmlTextWriter._tagNameLookupArray.Length)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._tagKey = value;
				if (value != HtmlTextWriterTag.Unknown)
				{
					this._tagName = HtmlTextWriter._tagNameLookupArray[this._tagIndex].name;
				}
			}
		}

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x06002D7D RID: 11645 RVA: 0x000CB820 File Offset: 0x000CA820
		// (set) Token: 0x06002D7E RID: 11646 RVA: 0x000CB828 File Offset: 0x000CA828
		protected string TagName
		{
			get
			{
				return this._tagName;
			}
			set
			{
				this._tagName = value;
				this._tagKey = this.GetTagKey(this._tagName);
				this._tagIndex = (int)this._tagKey;
			}
		}

		// Token: 0x06002D7F RID: 11647 RVA: 0x000CB850 File Offset: 0x000CA850
		public virtual void AddAttribute(string name, string value)
		{
			HtmlTextWriterAttribute attributeKey = this.GetAttributeKey(name);
			value = this.EncodeAttributeValue(attributeKey, value);
			this.AddAttribute(name, value, attributeKey);
		}

		// Token: 0x06002D80 RID: 11648 RVA: 0x000CB878 File Offset: 0x000CA878
		public virtual void AddAttribute(string name, string value, bool fEndode)
		{
			value = this.EncodeAttributeValue(value, fEndode);
			this.AddAttribute(name, value, this.GetAttributeKey(name));
		}

		// Token: 0x06002D81 RID: 11649 RVA: 0x000CB894 File Offset: 0x000CA894
		public virtual void AddAttribute(HtmlTextWriterAttribute key, string value)
		{
			if (key >= HtmlTextWriterAttribute.Accesskey && key < (HtmlTextWriterAttribute)HtmlTextWriter._attrNameLookupArray.Length)
			{
				HtmlTextWriter.AttributeInformation attributeInformation = HtmlTextWriter._attrNameLookupArray[(int)key];
				this.AddAttribute(attributeInformation.name, value, key, attributeInformation.encode, attributeInformation.isUrl);
			}
		}

		// Token: 0x06002D82 RID: 11650 RVA: 0x000CB8E0 File Offset: 0x000CA8E0
		public virtual void AddAttribute(HtmlTextWriterAttribute key, string value, bool fEncode)
		{
			if (key >= HtmlTextWriterAttribute.Accesskey && key < (HtmlTextWriterAttribute)HtmlTextWriter._attrNameLookupArray.Length)
			{
				HtmlTextWriter.AttributeInformation attributeInformation = HtmlTextWriter._attrNameLookupArray[(int)key];
				this.AddAttribute(attributeInformation.name, value, key, fEncode, attributeInformation.isUrl);
			}
		}

		// Token: 0x06002D83 RID: 11651 RVA: 0x000CB925 File Offset: 0x000CA925
		protected virtual void AddAttribute(string name, string value, HtmlTextWriterAttribute key)
		{
			this.AddAttribute(name, value, key, false, false);
		}

		// Token: 0x06002D84 RID: 11652 RVA: 0x000CB934 File Offset: 0x000CA934
		private void AddAttribute(string name, string value, HtmlTextWriterAttribute key, bool encode, bool isUrl)
		{
			if (this._attrList == null)
			{
				this._attrList = new HtmlTextWriter.RenderAttribute[20];
			}
			else if (this._attrCount >= this._attrList.Length)
			{
				HtmlTextWriter.RenderAttribute[] array = new HtmlTextWriter.RenderAttribute[this._attrList.Length * 2];
				Array.Copy(this._attrList, array, this._attrList.Length);
				this._attrList = array;
			}
			HtmlTextWriter.RenderAttribute renderAttribute;
			renderAttribute.name = name;
			renderAttribute.value = value;
			renderAttribute.key = key;
			renderAttribute.encode = encode;
			renderAttribute.isUrl = isUrl;
			this._attrList[this._attrCount] = renderAttribute;
			this._attrCount++;
		}

		// Token: 0x06002D85 RID: 11653 RVA: 0x000CB9E2 File Offset: 0x000CA9E2
		public virtual void AddStyleAttribute(string name, string value)
		{
			this.AddStyleAttribute(name, value, CssTextWriter.GetStyleKey(name));
		}

		// Token: 0x06002D86 RID: 11654 RVA: 0x000CB9F2 File Offset: 0x000CA9F2
		public virtual void AddStyleAttribute(HtmlTextWriterStyle key, string value)
		{
			this.AddStyleAttribute(CssTextWriter.GetStyleName(key), value, key);
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x000CBA04 File Offset: 0x000CAA04
		protected virtual void AddStyleAttribute(string name, string value, HtmlTextWriterStyle key)
		{
			if (this._styleList == null)
			{
				this._styleList = new RenderStyle[20];
			}
			else if (this._styleCount > this._styleList.Length)
			{
				RenderStyle[] array = new RenderStyle[this._styleList.Length * 2];
				Array.Copy(this._styleList, array, this._styleList.Length);
				this._styleList = array;
			}
			RenderStyle renderStyle;
			renderStyle.name = name;
			renderStyle.key = key;
			string text = value;
			if (CssTextWriter.IsStyleEncoded(key))
			{
				text = HttpUtility.HtmlAttributeEncode(value);
			}
			renderStyle.value = text;
			this._styleList[this._styleCount] = renderStyle;
			this._styleCount++;
		}

		// Token: 0x06002D88 RID: 11656 RVA: 0x000CBAB1 File Offset: 0x000CAAB1
		protected string EncodeAttributeValue(string value, bool fEncode)
		{
			if (value == null)
			{
				return null;
			}
			if (!fEncode)
			{
				return value;
			}
			return HttpUtility.HtmlAttributeEncode(value);
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x000CBAC4 File Offset: 0x000CAAC4
		protected virtual string EncodeAttributeValue(HtmlTextWriterAttribute attrKey, string value)
		{
			bool flag = true;
			if (HtmlTextWriterAttribute.Accesskey <= attrKey && attrKey < (HtmlTextWriterAttribute)HtmlTextWriter._attrNameLookupArray.Length)
			{
				flag = HtmlTextWriter._attrNameLookupArray[(int)attrKey].encode;
			}
			return this.EncodeAttributeValue(value, flag);
		}

		// Token: 0x06002D8A RID: 11658 RVA: 0x000CBAFA File Offset: 0x000CAAFA
		protected string EncodeUrl(string url)
		{
			if (!UrlPath.IsUncSharePath(url))
			{
				return HttpUtility.UrlPathEncode(url);
			}
			return url;
		}

		// Token: 0x06002D8B RID: 11659 RVA: 0x000CBB0C File Offset: 0x000CAB0C
		protected HtmlTextWriterAttribute GetAttributeKey(string attrName)
		{
			if (!string.IsNullOrEmpty(attrName))
			{
				object obj = HtmlTextWriter._attrKeyLookupTable[attrName.ToLower(CultureInfo.InvariantCulture)];
				if (obj != null)
				{
					return (HtmlTextWriterAttribute)obj;
				}
			}
			return (HtmlTextWriterAttribute)(-1);
		}

		// Token: 0x06002D8C RID: 11660 RVA: 0x000CBB42 File Offset: 0x000CAB42
		protected string GetAttributeName(HtmlTextWriterAttribute attrKey)
		{
			if (attrKey >= HtmlTextWriterAttribute.Accesskey && attrKey < (HtmlTextWriterAttribute)HtmlTextWriter._attrNameLookupArray.Length)
			{
				return HtmlTextWriter._attrNameLookupArray[(int)attrKey].name;
			}
			return string.Empty;
		}

		// Token: 0x06002D8D RID: 11661 RVA: 0x000CBB68 File Offset: 0x000CAB68
		protected HtmlTextWriterStyle GetStyleKey(string styleName)
		{
			return CssTextWriter.GetStyleKey(styleName);
		}

		// Token: 0x06002D8E RID: 11662 RVA: 0x000CBB70 File Offset: 0x000CAB70
		protected string GetStyleName(HtmlTextWriterStyle styleKey)
		{
			return CssTextWriter.GetStyleName(styleKey);
		}

		// Token: 0x06002D8F RID: 11663 RVA: 0x000CBB78 File Offset: 0x000CAB78
		protected virtual HtmlTextWriterTag GetTagKey(string tagName)
		{
			if (!string.IsNullOrEmpty(tagName))
			{
				object obj = HtmlTextWriter._tagKeyLookupTable[tagName.ToLower(CultureInfo.InvariantCulture)];
				if (obj != null)
				{
					return (HtmlTextWriterTag)obj;
				}
			}
			return HtmlTextWriterTag.Unknown;
		}

		// Token: 0x06002D90 RID: 11664 RVA: 0x000CBBB0 File Offset: 0x000CABB0
		protected virtual string GetTagName(HtmlTextWriterTag tagKey)
		{
			if (tagKey >= HtmlTextWriterTag.Unknown && tagKey < (HtmlTextWriterTag)HtmlTextWriter._tagNameLookupArray.Length)
			{
				return HtmlTextWriter._tagNameLookupArray[(int)tagKey].name;
			}
			return string.Empty;
		}

		// Token: 0x06002D91 RID: 11665 RVA: 0x000CBBE4 File Offset: 0x000CABE4
		protected bool IsAttributeDefined(HtmlTextWriterAttribute key)
		{
			for (int i = 0; i < this._attrCount; i++)
			{
				if (this._attrList[i].key == key)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x000CBC1C File Offset: 0x000CAC1C
		protected bool IsAttributeDefined(HtmlTextWriterAttribute key, out string value)
		{
			value = null;
			for (int i = 0; i < this._attrCount; i++)
			{
				if (this._attrList[i].key == key)
				{
					value = this._attrList[i].value;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x000CBC68 File Offset: 0x000CAC68
		protected bool IsStyleAttributeDefined(HtmlTextWriterStyle key)
		{
			for (int i = 0; i < this._styleCount; i++)
			{
				if (this._styleList[i].key == key)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002D94 RID: 11668 RVA: 0x000CBCA0 File Offset: 0x000CACA0
		protected bool IsStyleAttributeDefined(HtmlTextWriterStyle key, out string value)
		{
			value = null;
			for (int i = 0; i < this._styleCount; i++)
			{
				if (this._styleList[i].key == key)
				{
					value = this._styleList[i].value;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002D95 RID: 11669 RVA: 0x000CBCEB File Offset: 0x000CACEB
		protected virtual bool OnAttributeRender(string name, string value, HtmlTextWriterAttribute key)
		{
			return true;
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x000CBCEE File Offset: 0x000CACEE
		protected virtual bool OnStyleAttributeRender(string name, string value, HtmlTextWriterStyle key)
		{
			return true;
		}

		// Token: 0x06002D97 RID: 11671 RVA: 0x000CBCF1 File Offset: 0x000CACF1
		protected virtual bool OnTagRender(string name, HtmlTextWriterTag key)
		{
			return true;
		}

		// Token: 0x06002D98 RID: 11672 RVA: 0x000CBCF4 File Offset: 0x000CACF4
		protected string PopEndTag()
		{
			if (this._endTagCount <= 0)
			{
				throw new InvalidOperationException(SR.GetString("HTMLTextWriterUnbalancedPop"));
			}
			this._endTagCount--;
			this.TagKey = this._endTags[this._endTagCount].tagKey;
			return this._endTags[this._endTagCount].endTagText;
		}

		// Token: 0x06002D99 RID: 11673 RVA: 0x000CBD5C File Offset: 0x000CAD5C
		protected void PushEndTag(string endTag)
		{
			if (this._endTags == null)
			{
				this._endTags = new HtmlTextWriter.TagStackEntry[16];
			}
			else if (this._endTagCount >= this._endTags.Length)
			{
				HtmlTextWriter.TagStackEntry[] array = new HtmlTextWriter.TagStackEntry[this._endTags.Length * 2];
				Array.Copy(this._endTags, array, this._endTags.Length);
				this._endTags = array;
			}
			this._endTags[this._endTagCount].tagKey = this._tagKey;
			this._endTags[this._endTagCount].endTagText = endTag;
			this._endTagCount++;
		}

		// Token: 0x06002D9A RID: 11674 RVA: 0x000CBDFC File Offset: 0x000CADFC
		protected virtual void FilterAttributes()
		{
			int num = 0;
			for (int i = 0; i < this._styleCount; i++)
			{
				RenderStyle renderStyle = this._styleList[i];
				if (this.OnStyleAttributeRender(renderStyle.name, renderStyle.value, renderStyle.key))
				{
					this._styleList[num] = renderStyle;
					num++;
				}
			}
			this._styleCount = num;
			int num2 = 0;
			for (int j = 0; j < this._attrCount; j++)
			{
				HtmlTextWriter.RenderAttribute renderAttribute = this._attrList[j];
				if (this.OnAttributeRender(renderAttribute.name, renderAttribute.value, renderAttribute.key))
				{
					this._attrList[num2] = renderAttribute;
					num2++;
				}
			}
			this._attrCount = num2;
		}

		// Token: 0x06002D9B RID: 11675 RVA: 0x000CBECE File Offset: 0x000CAECE
		public virtual void RenderBeginTag(string tagName)
		{
			this.TagName = tagName;
			this.RenderBeginTag(this._tagKey);
		}

		// Token: 0x06002D9C RID: 11676 RVA: 0x000CBEE4 File Offset: 0x000CAEE4
		public virtual void RenderBeginTag(HtmlTextWriterTag tagKey)
		{
			this.TagKey = tagKey;
			bool flag = true;
			if (this._isDescendant)
			{
				flag = this.OnTagRender(this._tagName, this._tagKey);
				this.FilterAttributes();
				string text = this.RenderBeforeTag();
				if (text != null)
				{
					if (this.tabsPending)
					{
						this.OutputTabs();
					}
					this.writer.Write(text);
				}
			}
			HtmlTextWriter.TagInformation tagInformation = HtmlTextWriter._tagNameLookupArray[this._tagIndex];
			HtmlTextWriter.TagType tagType = tagInformation.tagType;
			bool flag2 = flag && tagType != HtmlTextWriter.TagType.NonClosing;
			string text2 = (flag2 ? tagInformation.closingTag : null);
			if (flag)
			{
				if (this.tabsPending)
				{
					this.OutputTabs();
				}
				this.writer.Write('<');
				this.writer.Write(this._tagName);
				string text3 = null;
				for (int i = 0; i < this._attrCount; i++)
				{
					HtmlTextWriter.RenderAttribute renderAttribute = this._attrList[i];
					if (renderAttribute.key == HtmlTextWriterAttribute.Style)
					{
						text3 = renderAttribute.value;
					}
					else
					{
						this.writer.Write(' ');
						this.writer.Write(renderAttribute.name);
						if (renderAttribute.value != null)
						{
							this.writer.Write("=\"");
							string text4 = renderAttribute.value;
							if (renderAttribute.isUrl && (renderAttribute.key != HtmlTextWriterAttribute.Href || !text4.StartsWith("javascript:", StringComparison.Ordinal)))
							{
								text4 = this.EncodeUrl(text4);
							}
							if (renderAttribute.encode)
							{
								this.WriteHtmlAttributeEncode(text4);
							}
							else
							{
								this.writer.Write(text4);
							}
							this.writer.Write('"');
						}
					}
				}
				if (this._styleCount > 0 || text3 != null)
				{
					this.writer.Write(' ');
					this.writer.Write("style");
					this.writer.Write("=\"");
					CssTextWriter.WriteAttributes(this.writer, this._styleList, this._styleCount);
					if (text3 != null)
					{
						this.writer.Write(text3);
					}
					this.writer.Write('"');
				}
				if (tagType == HtmlTextWriter.TagType.NonClosing)
				{
					this.writer.Write(" />");
				}
				else
				{
					this.writer.Write('>');
				}
			}
			string text5 = this.RenderBeforeContent();
			if (text5 != null)
			{
				if (this.tabsPending)
				{
					this.OutputTabs();
				}
				this.writer.Write(text5);
			}
			if (flag2)
			{
				if (tagType == HtmlTextWriter.TagType.Inline)
				{
					this._inlineCount++;
				}
				else
				{
					this.WriteLine();
					this.Indent++;
				}
				if (text2 == null)
				{
					text2 = "</" + this._tagName + '>'.ToString(CultureInfo.InvariantCulture);
				}
			}
			if (this._isDescendant)
			{
				string text6 = this.RenderAfterTag();
				if (text6 != null)
				{
					text2 = ((text2 == null) ? text6 : (text6 + text2));
				}
				string text7 = this.RenderAfterContent();
				if (text7 != null)
				{
					text2 = ((text2 == null) ? text7 : (text7 + text2));
				}
			}
			this.PushEndTag(text2);
			this._attrCount = 0;
			this._styleCount = 0;
		}

		// Token: 0x06002D9D RID: 11677 RVA: 0x000CC1FC File Offset: 0x000CB1FC
		public virtual void RenderEndTag()
		{
			string text = this.PopEndTag();
			if (text != null)
			{
				if (HtmlTextWriter._tagNameLookupArray[this._tagIndex].tagType == HtmlTextWriter.TagType.Inline)
				{
					this._inlineCount--;
					this.Write(text);
					return;
				}
				this.WriteLine();
				this.Indent--;
				this.Write(text);
			}
		}

		// Token: 0x06002D9E RID: 11678 RVA: 0x000CC25B File Offset: 0x000CB25B
		protected virtual string RenderBeforeTag()
		{
			return null;
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x000CC25E File Offset: 0x000CB25E
		protected virtual string RenderBeforeContent()
		{
			return null;
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x000CC261 File Offset: 0x000CB261
		protected virtual string RenderAfterContent()
		{
			return null;
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x000CC264 File Offset: 0x000CB264
		protected virtual string RenderAfterTag()
		{
			return null;
		}

		// Token: 0x06002DA2 RID: 11682 RVA: 0x000CC267 File Offset: 0x000CB267
		public virtual void WriteAttribute(string name, string value)
		{
			this.WriteAttribute(name, value, false);
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x000CC274 File Offset: 0x000CB274
		public virtual void WriteAttribute(string name, string value, bool fEncode)
		{
			this.writer.Write(' ');
			this.writer.Write(name);
			if (value != null)
			{
				this.writer.Write("=\"");
				if (fEncode)
				{
					this.WriteHtmlAttributeEncode(value);
				}
				else
				{
					this.writer.Write(value);
				}
				this.writer.Write('"');
			}
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x000CC2D2 File Offset: 0x000CB2D2
		public virtual void WriteBeginTag(string tagName)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.Write('<');
			this.writer.Write(tagName);
		}

		// Token: 0x06002DA5 RID: 11685 RVA: 0x000CC2FB File Offset: 0x000CB2FB
		public virtual void WriteBreak()
		{
			this.Write("<br />");
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x000CC308 File Offset: 0x000CB308
		internal void WriteObsoleteBreak()
		{
			this.Write("<br>");
		}

		// Token: 0x06002DA7 RID: 11687 RVA: 0x000CC315 File Offset: 0x000CB315
		public virtual void WriteFullBeginTag(string tagName)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.Write('<');
			this.writer.Write(tagName);
			this.writer.Write('>');
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x000CC34C File Offset: 0x000CB34C
		public virtual void WriteEndTag(string tagName)
		{
			if (this.tabsPending)
			{
				this.OutputTabs();
			}
			this.writer.Write('<');
			this.writer.Write('/');
			this.writer.Write(tagName);
			this.writer.Write('>');
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x000CC39A File Offset: 0x000CB39A
		public virtual void WriteStyleAttribute(string name, string value)
		{
			this.WriteStyleAttribute(name, value, false);
		}

		// Token: 0x06002DAA RID: 11690 RVA: 0x000CC3A5 File Offset: 0x000CB3A5
		public virtual void WriteStyleAttribute(string name, string value, bool fEncode)
		{
			this.writer.Write(name);
			this.writer.Write(':');
			if (fEncode)
			{
				this.WriteHtmlAttributeEncode(value);
			}
			else
			{
				this.writer.Write(value);
			}
			this.writer.Write(';');
		}

		// Token: 0x06002DAB RID: 11691 RVA: 0x000CC3E5 File Offset: 0x000CB3E5
		internal void WriteUTF8ResourceString(IntPtr pv, int offset, int size, bool fAsciiOnly)
		{
			if (this._httpWriter != null)
			{
				if (this.tabsPending)
				{
					this.OutputTabs();
				}
				this._httpWriter.WriteUTF8ResourceString(pv, offset, size, fAsciiOnly);
				return;
			}
			this.Write(StringResourceManager.ResourceToString(pv, offset, size));
		}

		// Token: 0x06002DAC RID: 11692 RVA: 0x000CC41C File Offset: 0x000CB41C
		public virtual void WriteEncodedUrl(string url)
		{
			int num = url.IndexOf('?');
			if (num != -1)
			{
				this.WriteUrlEncodedString(url.Substring(0, num), false);
				this.Write(url.Substring(num));
				return;
			}
			this.WriteUrlEncodedString(url, false);
		}

		// Token: 0x06002DAD RID: 11693 RVA: 0x000CC45B File Offset: 0x000CB45B
		public virtual void WriteEncodedUrlParameter(string urlText)
		{
			this.WriteUrlEncodedString(urlText, true);
		}

		// Token: 0x06002DAE RID: 11694 RVA: 0x000CC468 File Offset: 0x000CB468
		public virtual void WriteEncodedText(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			int length = text.Length;
			int i = 0;
			while (i < length)
			{
				int num = text.IndexOf('\u00a0', i);
				if (num < 0)
				{
					HttpUtility.HtmlEncode((i == 0) ? text : text.Substring(i, length - i), this);
					i = length;
				}
				else
				{
					if (num > i)
					{
						HttpUtility.HtmlEncode(text.Substring(i, num - i), this);
					}
					this.Write("&nbsp;");
					i = num + 1;
				}
			}
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x000CC4E0 File Offset: 0x000CB4E0
		protected void WriteUrlEncodedString(string text, bool argument)
		{
			int length = text.Length;
			for (int i = 0; i < length; i++)
			{
				char c = text[i];
				if (HttpUtility.IsSafe(c))
				{
					this.Write(c);
				}
				else if (!argument && (c == '/' || c == ':' || c == '#' || c == ','))
				{
					this.Write(c);
				}
				else if (c == ' ' && argument)
				{
					this.Write('+');
				}
				else if ((c & 'ﾀ') == '\0')
				{
					this.Write('%');
					this.Write(HttpUtility.IntToHex((int)((c >> 4) & '\u000f')));
					this.Write(HttpUtility.IntToHex((int)(c & '\u000f')));
				}
				else
				{
					this.Write(HttpUtility.UrlEncodeNonAscii(char.ToString(c), Encoding.UTF8));
				}
			}
		}

		// Token: 0x06002DB0 RID: 11696 RVA: 0x000CC59A File Offset: 0x000CB59A
		internal void WriteHtmlAttributeEncode(string s)
		{
			if (this._httpWriter == null)
			{
				HttpUtility.HtmlAttributeEncode(s, this.writer);
				return;
			}
			HttpUtility.HtmlAttributeEncodeInternal(s, this._httpWriter);
		}

		// Token: 0x040020EB RID: 8427
		public const char TagLeftChar = '<';

		// Token: 0x040020EC RID: 8428
		public const char TagRightChar = '>';

		// Token: 0x040020ED RID: 8429
		public const string SelfClosingChars = " /";

		// Token: 0x040020EE RID: 8430
		public const string SelfClosingTagEnd = " />";

		// Token: 0x040020EF RID: 8431
		public const string EndTagLeftChars = "</";

		// Token: 0x040020F0 RID: 8432
		public const char DoubleQuoteChar = '"';

		// Token: 0x040020F1 RID: 8433
		public const char SingleQuoteChar = '\'';

		// Token: 0x040020F2 RID: 8434
		public const char SpaceChar = ' ';

		// Token: 0x040020F3 RID: 8435
		public const char EqualsChar = '=';

		// Token: 0x040020F4 RID: 8436
		public const char SlashChar = '/';

		// Token: 0x040020F5 RID: 8437
		public const string EqualsDoubleQuoteString = "=\"";

		// Token: 0x040020F6 RID: 8438
		public const char SemicolonChar = ';';

		// Token: 0x040020F7 RID: 8439
		public const char StyleEqualsChar = ':';

		// Token: 0x040020F8 RID: 8440
		public const string DefaultTabString = "\t";

		// Token: 0x040020F9 RID: 8441
		internal const string DesignerRegionAttributeName = "_designerRegion";

		// Token: 0x040020FA RID: 8442
		private HtmlTextWriter.Layout _currentLayout = new HtmlTextWriter.Layout(HorizontalAlign.NotSet, true);

		// Token: 0x040020FB RID: 8443
		private HtmlTextWriter.Layout _currentWrittenLayout;

		// Token: 0x040020FC RID: 8444
		private TextWriter writer;

		// Token: 0x040020FD RID: 8445
		private int indentLevel;

		// Token: 0x040020FE RID: 8446
		private bool tabsPending;

		// Token: 0x040020FF RID: 8447
		private string tabString;

		// Token: 0x04002100 RID: 8448
		private static Hashtable _tagKeyLookupTable = new Hashtable(97);

		// Token: 0x04002101 RID: 8449
		private static Hashtable _attrKeyLookupTable;

		// Token: 0x04002102 RID: 8450
		private static HtmlTextWriter.TagInformation[] _tagNameLookupArray = new HtmlTextWriter.TagInformation[97];

		// Token: 0x04002103 RID: 8451
		private static HtmlTextWriter.AttributeInformation[] _attrNameLookupArray;

		// Token: 0x04002104 RID: 8452
		private HtmlTextWriter.RenderAttribute[] _attrList;

		// Token: 0x04002105 RID: 8453
		private int _attrCount;

		// Token: 0x04002106 RID: 8454
		private int _endTagCount;

		// Token: 0x04002107 RID: 8455
		private HtmlTextWriter.TagStackEntry[] _endTags;

		// Token: 0x04002108 RID: 8456
		private HttpWriter _httpWriter;

		// Token: 0x04002109 RID: 8457
		private int _inlineCount;

		// Token: 0x0400210A RID: 8458
		private bool _isDescendant;

		// Token: 0x0400210B RID: 8459
		private RenderStyle[] _styleList;

		// Token: 0x0400210C RID: 8460
		private int _styleCount;

		// Token: 0x0400210D RID: 8461
		private int _tagIndex;

		// Token: 0x0400210E RID: 8462
		private HtmlTextWriterTag _tagKey;

		// Token: 0x0400210F RID: 8463
		private string _tagName;

		// Token: 0x020003A1 RID: 929
		internal class Layout
		{
			// Token: 0x06002DB1 RID: 11697 RVA: 0x000CC5BD File Offset: 0x000CB5BD
			public Layout(HorizontalAlign alignment, bool wrapping)
			{
				this.Align = alignment;
				this.Wrap = wrapping;
			}

			// Token: 0x170009ED RID: 2541
			// (get) Token: 0x06002DB2 RID: 11698 RVA: 0x000CC5D3 File Offset: 0x000CB5D3
			// (set) Token: 0x06002DB3 RID: 11699 RVA: 0x000CC5DB File Offset: 0x000CB5DB
			public bool Wrap
			{
				get
				{
					return this._wrap;
				}
				set
				{
					this._wrap = value;
				}
			}

			// Token: 0x170009EE RID: 2542
			// (get) Token: 0x06002DB4 RID: 11700 RVA: 0x000CC5E4 File Offset: 0x000CB5E4
			// (set) Token: 0x06002DB5 RID: 11701 RVA: 0x000CC5EC File Offset: 0x000CB5EC
			public HorizontalAlign Align
			{
				get
				{
					return this._align;
				}
				set
				{
					this._align = value;
				}
			}

			// Token: 0x04002110 RID: 8464
			private bool _wrap;

			// Token: 0x04002111 RID: 8465
			private HorizontalAlign _align;
		}

		// Token: 0x020003A2 RID: 930
		private struct TagStackEntry
		{
			// Token: 0x04002112 RID: 8466
			public HtmlTextWriterTag tagKey;

			// Token: 0x04002113 RID: 8467
			public string endTagText;
		}

		// Token: 0x020003A3 RID: 931
		private struct RenderAttribute
		{
			// Token: 0x04002114 RID: 8468
			public string name;

			// Token: 0x04002115 RID: 8469
			public string value;

			// Token: 0x04002116 RID: 8470
			public HtmlTextWriterAttribute key;

			// Token: 0x04002117 RID: 8471
			public bool encode;

			// Token: 0x04002118 RID: 8472
			public bool isUrl;
		}

		// Token: 0x020003A4 RID: 932
		private struct AttributeInformation
		{
			// Token: 0x06002DB6 RID: 11702 RVA: 0x000CC5F5 File Offset: 0x000CB5F5
			public AttributeInformation(string name, bool encode, bool isUrl)
			{
				this.name = name;
				this.encode = encode;
				this.isUrl = isUrl;
			}

			// Token: 0x04002119 RID: 8473
			public string name;

			// Token: 0x0400211A RID: 8474
			public bool isUrl;

			// Token: 0x0400211B RID: 8475
			public bool encode;
		}

		// Token: 0x020003A5 RID: 933
		private enum TagType
		{
			// Token: 0x0400211D RID: 8477
			Inline,
			// Token: 0x0400211E RID: 8478
			NonClosing,
			// Token: 0x0400211F RID: 8479
			Other
		}

		// Token: 0x020003A6 RID: 934
		private struct TagInformation
		{
			// Token: 0x06002DB7 RID: 11703 RVA: 0x000CC60C File Offset: 0x000CB60C
			public TagInformation(string name, HtmlTextWriter.TagType tagType, string closingTag)
			{
				this.name = name;
				this.tagType = tagType;
				this.closingTag = closingTag;
			}

			// Token: 0x04002120 RID: 8480
			public string name;

			// Token: 0x04002121 RID: 8481
			public HtmlTextWriter.TagType tagType;

			// Token: 0x04002122 RID: 8482
			public string closingTag;
		}
	}
}
