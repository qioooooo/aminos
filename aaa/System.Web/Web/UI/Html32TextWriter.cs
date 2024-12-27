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
	// Token: 0x020003A7 RID: 935
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class Html32TextWriter : HtmlTextWriter
	{
		// Token: 0x06002DB8 RID: 11704 RVA: 0x000CC623 File Offset: 0x000CB623
		public Html32TextWriter(TextWriter writer)
			: this(writer, "\t")
		{
		}

		// Token: 0x06002DB9 RID: 11705 RVA: 0x000CC634 File Offset: 0x000CB634
		public Html32TextWriter(TextWriter writer, string tabString)
			: base(writer, tabString)
		{
			this._beforeTag = new StringBuilder(256);
			this._beforeContent = new StringBuilder(256);
			this._afterContent = new StringBuilder(128);
			this._afterTag = new StringBuilder(128);
		}

		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x06002DBA RID: 11706 RVA: 0x000CC697 File Offset: 0x000CB697
		protected Stack FontStack
		{
			get
			{
				if (this._fontStack == null)
				{
					this._fontStack = new Stack(3);
				}
				return this._fontStack;
			}
		}

		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x06002DBB RID: 11707 RVA: 0x000CC6B3 File Offset: 0x000CB6B3
		internal override bool RenderDivAroundHiddenInputs
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x06002DBC RID: 11708 RVA: 0x000CC6B6 File Offset: 0x000CB6B6
		// (set) Token: 0x06002DBD RID: 11709 RVA: 0x000CC6BE File Offset: 0x000CB6BE
		public bool ShouldPerformDivTableSubstitution
		{
			get
			{
				return this._shouldPerformDivTableSubstitution;
			}
			set
			{
				this._shouldPerformDivTableSubstitution = value;
			}
		}

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x06002DBE RID: 11710 RVA: 0x000CC6C7 File Offset: 0x000CB6C7
		// (set) Token: 0x06002DBF RID: 11711 RVA: 0x000CC6CF File Offset: 0x000CB6CF
		public bool SupportsBold
		{
			get
			{
				return this._supportsBold;
			}
			set
			{
				this._supportsBold = value;
			}
		}

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x06002DC0 RID: 11712 RVA: 0x000CC6D8 File Offset: 0x000CB6D8
		// (set) Token: 0x06002DC1 RID: 11713 RVA: 0x000CC6E0 File Offset: 0x000CB6E0
		public bool SupportsItalic
		{
			get
			{
				return this._supportsItalic;
			}
			set
			{
				this._supportsItalic = value;
			}
		}

		// Token: 0x06002DC2 RID: 11714 RVA: 0x000CC6E9 File Offset: 0x000CB6E9
		private void AppendFontTag(StringBuilder sbBegin, StringBuilder sbEnd)
		{
			this.AppendFontTag(this._fontFace, this._fontColor, this._fontSize, sbBegin, sbEnd);
		}

		// Token: 0x06002DC3 RID: 11715 RVA: 0x000CC708 File Offset: 0x000CB708
		private void AppendFontTag(string fontFace, string fontColor, string fontSize, StringBuilder sbBegin, StringBuilder sbEnd)
		{
			sbBegin.Append('<');
			sbBegin.Append("font");
			if (fontFace != null)
			{
				sbBegin.Append(" face");
				sbBegin.Append("=\"");
				sbBegin.Append(fontFace);
				sbBegin.Append('"');
			}
			if (fontColor != null)
			{
				sbBegin.Append(" color=");
				sbBegin.Append('"');
				sbBegin.Append(fontColor);
				sbBegin.Append('"');
			}
			if (fontSize != null)
			{
				sbBegin.Append(" size=");
				sbBegin.Append('"');
				sbBegin.Append(fontSize);
				sbBegin.Append('"');
			}
			sbBegin.Append('>');
			sbEnd.Insert(0, "</font" + '>');
		}

		// Token: 0x06002DC4 RID: 11716 RVA: 0x000CC7DA File Offset: 0x000CB7DA
		private void AppendOtherTag(string tag)
		{
			if (this.Supports(1))
			{
				this.AppendOtherTag(tag, this._beforeContent, this._afterContent);
				return;
			}
			this.AppendOtherTag(tag, this._beforeTag, this._afterTag);
		}

		// Token: 0x06002DC5 RID: 11717 RVA: 0x000CC80C File Offset: 0x000CB80C
		private void AppendOtherTag(string tag, StringBuilder sbBegin, StringBuilder sbEnd)
		{
			sbBegin.Append('<');
			sbBegin.Append(tag);
			sbBegin.Append('>');
			sbEnd.Insert(0, "</" + tag + '>');
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x000CC844 File Offset: 0x000CB844
		private void AppendOtherTag(string tag, object[] attribs, StringBuilder sbBegin, StringBuilder sbEnd)
		{
			sbBegin.Append('<');
			sbBegin.Append(tag);
			for (int i = 0; i < attribs.Length; i++)
			{
				sbBegin.Append(' ');
				sbBegin.Append(((string[])attribs[i])[0]);
				sbBegin.Append("=\"");
				sbBegin.Append(((string[])attribs[i])[1]);
				sbBegin.Append('"');
			}
			sbBegin.Append('>');
			sbEnd.Insert(0, "</" + tag + '>');
		}

		// Token: 0x06002DC7 RID: 11719 RVA: 0x000CC8D4 File Offset: 0x000CB8D4
		private void ConsumeFont(StringBuilder sbBegin, StringBuilder sbEnd)
		{
			if (this.FontStack.Count > 0)
			{
				string text = null;
				string text2 = null;
				string text3 = null;
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				foreach (object obj in this.FontStack)
				{
					Html32TextWriter.FontStackItem fontStackItem = (Html32TextWriter.FontStackItem)obj;
					if (text == null)
					{
						text = fontStackItem.name;
					}
					if (text2 == null)
					{
						text2 = fontStackItem.color;
					}
					if (text3 == null)
					{
						text3 = fontStackItem.size;
					}
					if (!flag)
					{
						flag = fontStackItem.underline;
					}
					if (!flag2)
					{
						flag2 = fontStackItem.italic;
					}
					if (!flag3)
					{
						flag3 = fontStackItem.bold;
					}
					if (!flag4)
					{
						flag4 = fontStackItem.strikeout;
					}
				}
				if (text != null || text2 != null || text3 != null)
				{
					this.AppendFontTag(text, text2, text3, sbBegin, sbEnd);
				}
				if (flag)
				{
					this.AppendOtherTag("u", sbBegin, sbEnd);
				}
				if (flag2 && this.SupportsItalic)
				{
					this.AppendOtherTag("i", sbBegin, sbEnd);
				}
				if (flag3 && this.SupportsBold)
				{
					this.AppendOtherTag("b", sbBegin, sbEnd);
				}
				if (flag4)
				{
					this.AppendOtherTag("strike", sbBegin, sbEnd);
				}
			}
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x000CC9E4 File Offset: 0x000CB9E4
		private string ConvertToHtmlFontSize(string value)
		{
			FontUnit fontUnit = new FontUnit(value, CultureInfo.InvariantCulture);
			if (fontUnit.Type > FontSize.Larger)
			{
				return (fontUnit.Type - FontSize.Larger).ToString(CultureInfo.InvariantCulture);
			}
			if (fontUnit.Type != FontSize.AsUnit || fontUnit.Unit.Type != UnitType.Point)
			{
				return null;
			}
			if (fontUnit.Unit.Value <= 8.0)
			{
				return "1";
			}
			if (fontUnit.Unit.Value <= 10.0)
			{
				return "2";
			}
			if (fontUnit.Unit.Value <= 12.0)
			{
				return "3";
			}
			if (fontUnit.Unit.Value <= 14.0)
			{
				return "4";
			}
			if (fontUnit.Unit.Value <= 18.0)
			{
				return "5";
			}
			if (fontUnit.Unit.Value <= 24.0)
			{
				return "6";
			}
			return "7";
		}

		// Token: 0x06002DC9 RID: 11721 RVA: 0x000CCB10 File Offset: 0x000CBB10
		private string ConvertToHtmlSize(string value)
		{
			Unit unit = new Unit(value, CultureInfo.InvariantCulture);
			if (unit.Type == UnitType.Pixel)
			{
				return unit.Value.ToString(CultureInfo.InvariantCulture);
			}
			if (unit.Type == UnitType.Percentage)
			{
				return value;
			}
			return null;
		}

		// Token: 0x06002DCA RID: 11722 RVA: 0x000CCB58 File Offset: 0x000CBB58
		protected override bool OnStyleAttributeRender(string name, string value, HtmlTextWriterStyle key)
		{
			if (this.Supports(1))
			{
				switch (key)
				{
				case HtmlTextWriterStyle.Color:
					this._fontColor = value;
					this._renderFontTag = true;
					break;
				case HtmlTextWriterStyle.FontFamily:
					this._fontFace = value;
					this._renderFontTag = true;
					break;
				case HtmlTextWriterStyle.FontSize:
					this._fontSize = this.ConvertToHtmlFontSize(value);
					if (this._fontSize != null)
					{
						this._renderFontTag = true;
					}
					break;
				case HtmlTextWriterStyle.FontStyle:
					if (!StringUtil.EqualsIgnoreCase(value, "normal") && this.SupportsItalic)
					{
						this.AppendOtherTag("i");
					}
					break;
				case HtmlTextWriterStyle.FontWeight:
					if (StringUtil.EqualsIgnoreCase(value, "bold") && this.SupportsBold)
					{
						this.AppendOtherTag("b");
					}
					break;
				case HtmlTextWriterStyle.TextDecoration:
				{
					string text = value.ToLower(CultureInfo.InvariantCulture);
					if (text.IndexOf("underline", StringComparison.Ordinal) != -1)
					{
						this.AppendOtherTag("u");
					}
					if (text.IndexOf("line-through", StringComparison.Ordinal) != -1)
					{
						this.AppendOtherTag("strike");
					}
					break;
				}
				}
			}
			else if (this.Supports(16))
			{
				Html32TextWriter.FontStackItem fontStackItem = (Html32TextWriter.FontStackItem)this.FontStack.Peek();
				switch (key)
				{
				case HtmlTextWriterStyle.Color:
					fontStackItem.color = value;
					break;
				case HtmlTextWriterStyle.FontFamily:
					fontStackItem.name = value;
					break;
				case HtmlTextWriterStyle.FontSize:
					fontStackItem.size = this.ConvertToHtmlFontSize(value);
					break;
				case HtmlTextWriterStyle.FontStyle:
					if (!StringUtil.EqualsIgnoreCase(value, "normal"))
					{
						fontStackItem.italic = true;
					}
					break;
				case HtmlTextWriterStyle.FontWeight:
					if (StringUtil.EqualsIgnoreCase(value, "bold"))
					{
						fontStackItem.bold = true;
					}
					break;
				case HtmlTextWriterStyle.TextDecoration:
				{
					string text = value.ToLower(CultureInfo.InvariantCulture);
					if (text.IndexOf("underline", StringComparison.Ordinal) != -1)
					{
						fontStackItem.underline = true;
					}
					if (text.IndexOf("line-through", StringComparison.Ordinal) != -1)
					{
						fontStackItem.strikeout = true;
					}
					break;
				}
				}
			}
			if (this.Supports(128) && key == HtmlTextWriterStyle.BorderWidth)
			{
				string text = this.ConvertToHtmlSize(value);
				if (text != null)
				{
					this.AddAttribute(HtmlTextWriterAttribute.Border, text);
				}
			}
			if (this.Supports(256) && key == HtmlTextWriterStyle.WhiteSpace)
			{
				this.AddAttribute(HtmlTextWriterAttribute.Nowrap, value);
			}
			if (this.Supports(64))
			{
				switch (key)
				{
				case HtmlTextWriterStyle.Height:
				{
					string text = this.ConvertToHtmlSize(value);
					if (text != null)
					{
						this.AddAttribute(HtmlTextWriterAttribute.Height, text);
					}
					break;
				}
				case HtmlTextWriterStyle.Width:
				{
					string text = this.ConvertToHtmlSize(value);
					if (text != null)
					{
						this.AddAttribute(HtmlTextWriterAttribute.Width, text);
					}
					break;
				}
				}
			}
			if (this.Supports(4) || this.Supports(8))
			{
				switch (key)
				{
				case HtmlTextWriterStyle.BackgroundColor:
				{
					HtmlTextWriterTag tagKey = base.TagKey;
					if (tagKey <= HtmlTextWriterTag.Div)
					{
						if (tagKey != HtmlTextWriterTag.Body)
						{
							if (tagKey != HtmlTextWriterTag.Div)
							{
								break;
							}
							if (this.ShouldPerformDivTableSubstitution)
							{
								this.AddAttribute(HtmlTextWriterAttribute.Bgcolor, value);
								break;
							}
							break;
						}
					}
					else
					{
						switch (tagKey)
						{
						case HtmlTextWriterTag.Table:
						case HtmlTextWriterTag.Td:
							break;
						case HtmlTextWriterTag.Tbody:
							goto IL_03CF;
						default:
							if (tagKey != HtmlTextWriterTag.Th && tagKey != HtmlTextWriterTag.Tr)
							{
								goto IL_03CF;
							}
							break;
						}
					}
					this.AddAttribute(HtmlTextWriterAttribute.Bgcolor, value);
					break;
				}
				case HtmlTextWriterStyle.BackgroundImage:
				{
					HtmlTextWriterTag tagKey2 = base.TagKey;
					if (tagKey2 <= HtmlTextWriterTag.Div)
					{
						if (tagKey2 != HtmlTextWriterTag.Body)
						{
							if (tagKey2 != HtmlTextWriterTag.Div)
							{
								break;
							}
							if (this.ShouldPerformDivTableSubstitution)
							{
								if (StringUtil.StringStartsWith(value, "url("))
								{
									value = value.Substring(4, value.Length - 5);
								}
								this.AddAttribute(HtmlTextWriterAttribute.Background, value);
								break;
							}
							break;
						}
					}
					else
					{
						switch (tagKey2)
						{
						case HtmlTextWriterTag.Table:
						case HtmlTextWriterTag.Td:
							break;
						case HtmlTextWriterTag.Tbody:
							goto IL_03CF;
						default:
							if (tagKey2 != HtmlTextWriterTag.Th)
							{
								goto IL_03CF;
							}
							break;
						}
					}
					if (StringUtil.StringStartsWith(value, "url("))
					{
						value = value.Substring(4, value.Length - 5);
					}
					this.AddAttribute(HtmlTextWriterAttribute.Background, value);
					break;
				}
				case HtmlTextWriterStyle.BorderColor:
				{
					HtmlTextWriterTag tagKey3 = base.TagKey;
					if (tagKey3 == HtmlTextWriterTag.Div && this.ShouldPerformDivTableSubstitution)
					{
						this.AddAttribute(HtmlTextWriterAttribute.Bordercolor, value);
					}
					break;
				}
				}
			}
			IL_03CF:
			if (key != HtmlTextWriterStyle.ListStyleType)
			{
				if (key == HtmlTextWriterStyle.Display)
				{
					return true;
				}
				switch (key)
				{
				case HtmlTextWriterStyle.TextAlign:
					this.AddAttribute(HtmlTextWriterAttribute.Align, value);
					break;
				case HtmlTextWriterStyle.VerticalAlign:
					this.AddAttribute(HtmlTextWriterAttribute.Valign, value);
					break;
				}
			}
			else
			{
				string text2;
				switch (text2 = value)
				{
				case "decimal":
					this.AddAttribute(HtmlTextWriterAttribute.Type, "1");
					return false;
				case "lower-alpha":
					this.AddAttribute(HtmlTextWriterAttribute.Type, "a");
					return false;
				case "upper-alpha":
					this.AddAttribute(HtmlTextWriterAttribute.Type, "A");
					return false;
				case "lower-roman":
					this.AddAttribute(HtmlTextWriterAttribute.Type, "i");
					return false;
				case "upper-roman":
					this.AddAttribute(HtmlTextWriterAttribute.Type, "I");
					return false;
				case "disc":
				case "circle":
				case "square":
					this.AddAttribute(HtmlTextWriterAttribute.Type, value);
					return false;
				}
				this.AddAttribute(HtmlTextWriterAttribute.Type, "disc");
			}
			return false;
		}

		// Token: 0x06002DCB RID: 11723 RVA: 0x000CD094 File Offset: 0x000CC094
		protected override bool OnTagRender(string name, HtmlTextWriterTag key)
		{
			this.SetTagSupports();
			if (this.Supports(16))
			{
				this.FontStack.Push(new Html32TextWriter.FontStackItem());
			}
			if (key == HtmlTextWriterTag.Div && this.ShouldPerformDivTableSubstitution)
			{
				base.TagKey = HtmlTextWriterTag.Table;
			}
			return base.OnTagRender(name, key);
		}

		// Token: 0x06002DCC RID: 11724 RVA: 0x000CD0D3 File Offset: 0x000CC0D3
		protected override string GetTagName(HtmlTextWriterTag tagKey)
		{
			if (tagKey == HtmlTextWriterTag.Div && this.ShouldPerformDivTableSubstitution)
			{
				return "table";
			}
			return base.GetTagName(tagKey);
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x000CD0F0 File Offset: 0x000CC0F0
		public override void RenderBeginTag(HtmlTextWriterTag tagKey)
		{
			this._beforeTag.Length = 0;
			this._beforeContent.Length = 0;
			this._afterContent.Length = 0;
			this._afterTag.Length = 0;
			this._renderFontTag = false;
			this._fontFace = null;
			this._fontColor = null;
			this._fontSize = null;
			if (this.ShouldPerformDivTableSubstitution && tagKey == HtmlTextWriterTag.Div)
			{
				this.AppendOtherTag("tr", this._beforeContent, this._afterContent);
				string text;
				if (base.IsAttributeDefined(HtmlTextWriterAttribute.Align, out text))
				{
					string[] array = new string[]
					{
						base.GetAttributeName(HtmlTextWriterAttribute.Align),
						text
					};
					this.AppendOtherTag("td", new object[] { array }, this._beforeContent, this._afterContent);
				}
				else
				{
					this.AppendOtherTag("td", this._beforeContent, this._afterContent);
				}
				if (!base.IsAttributeDefined(HtmlTextWriterAttribute.Cellpadding))
				{
					this.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
				}
				if (!base.IsAttributeDefined(HtmlTextWriterAttribute.Cellspacing))
				{
					this.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
				}
				if (!base.IsStyleAttributeDefined(HtmlTextWriterStyle.BorderWidth))
				{
					this.AddAttribute(HtmlTextWriterAttribute.Border, "0");
				}
				if (!base.IsStyleAttributeDefined(HtmlTextWriterStyle.Width))
				{
					this.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
				}
			}
			base.RenderBeginTag(tagKey);
		}

		// Token: 0x06002DCE RID: 11726 RVA: 0x000CD230 File Offset: 0x000CC230
		protected override string RenderBeforeTag()
		{
			if (this._renderFontTag && this.Supports(2))
			{
				this.AppendFontTag(this._beforeTag, this._afterTag);
			}
			if (this._beforeTag.Length > 0)
			{
				return this._beforeTag.ToString();
			}
			return base.RenderBeforeTag();
		}

		// Token: 0x06002DCF RID: 11727 RVA: 0x000CD280 File Offset: 0x000CC280
		protected override string RenderBeforeContent()
		{
			if (this.Supports(32))
			{
				this.ConsumeFont(this._beforeContent, this._afterContent);
			}
			else if (this._renderFontTag && this.Supports(1))
			{
				this.AppendFontTag(this._beforeContent, this._afterContent);
			}
			if (this._beforeContent.Length > 0)
			{
				return this._beforeContent.ToString();
			}
			return base.RenderBeforeContent();
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x000CD2EE File Offset: 0x000CC2EE
		protected override string RenderAfterContent()
		{
			if (this._afterContent.Length > 0)
			{
				return this._afterContent.ToString();
			}
			return base.RenderAfterContent();
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x000CD310 File Offset: 0x000CC310
		protected override string RenderAfterTag()
		{
			if (this._afterTag.Length > 0)
			{
				return this._afterTag.ToString();
			}
			return base.RenderAfterTag();
		}

		// Token: 0x06002DD2 RID: 11730 RVA: 0x000CD332 File Offset: 0x000CC332
		public override void RenderEndTag()
		{
			base.RenderEndTag();
			this.SetTagSupports();
			if (this.Supports(16))
			{
				this.FontStack.Pop();
			}
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x000CD358 File Offset: 0x000CC358
		private void SetTagSupports()
		{
			this._tagSupports = 0;
			HtmlTextWriterTag tagKey = base.TagKey;
			if (tagKey > HtmlTextWriterTag.P)
			{
				if (tagKey <= HtmlTextWriterTag.Td)
				{
					if (tagKey == HtmlTextWriterTag.Span)
					{
						goto IL_0092;
					}
					switch (tagKey)
					{
					case HtmlTextWriterTag.Table:
						goto IL_00C4;
					case HtmlTextWriterTag.Tbody:
						goto IL_00F8;
					case HtmlTextWriterTag.Td:
						break;
					default:
						goto IL_00F8;
					}
				}
				else if (tagKey != HtmlTextWriterTag.Th)
				{
					if (tagKey != HtmlTextWriterTag.Tr && tagKey != HtmlTextWriterTag.Ul)
					{
						goto IL_00F8;
					}
					goto IL_00C4;
				}
				this._tagSupports |= 48;
				goto IL_00F8;
			}
			if (tagKey <= HtmlTextWriterTag.Div)
			{
				if (tagKey != HtmlTextWriterTag.A)
				{
					if (tagKey != HtmlTextWriterTag.Div)
					{
						goto IL_00F8;
					}
					this._tagSupports |= 17;
					goto IL_00F8;
				}
			}
			else
			{
				if (tagKey == HtmlTextWriterTag.Input)
				{
					this._tagSupports |= 128;
					goto IL_00F8;
				}
				switch (tagKey)
				{
				case HtmlTextWriterTag.Label:
					break;
				case HtmlTextWriterTag.Legend:
					goto IL_00F8;
				case HtmlTextWriterTag.Li:
					this._tagSupports |= 33;
					goto IL_00F8;
				default:
					switch (tagKey)
					{
					case HtmlTextWriterTag.Ol:
						goto IL_00C4;
					case HtmlTextWriterTag.Option:
						goto IL_00F8;
					case HtmlTextWriterTag.P:
						break;
					default:
						goto IL_00F8;
					}
					break;
				}
			}
			IL_0092:
			this._tagSupports |= 1;
			goto IL_00F8;
			IL_00C4:
			this._tagSupports |= 16;
			IL_00F8:
			HtmlTextWriterTag tagKey2 = base.TagKey;
			if (tagKey2 <= HtmlTextWriterTag.Img)
			{
				if (tagKey2 != HtmlTextWriterTag.Div)
				{
					if (tagKey2 == HtmlTextWriterTag.Img)
					{
						this._tagSupports |= 192;
					}
				}
				else
				{
					if (this.ShouldPerformDivTableSubstitution)
					{
						this._tagSupports |= 192;
					}
					this._tagSupports |= 256;
				}
			}
			else
			{
				switch (tagKey2)
				{
				case HtmlTextWriterTag.Table:
					this._tagSupports |= 64;
					goto IL_0194;
				case HtmlTextWriterTag.Tbody:
					goto IL_0194;
				case HtmlTextWriterTag.Td:
					break;
				default:
					if (tagKey2 != HtmlTextWriterTag.Th)
					{
						goto IL_0194;
					}
					break;
				}
				this._tagSupports |= 320;
			}
			IL_0194:
			HtmlTextWriterTag tagKey3 = base.TagKey;
			if (tagKey3 <= HtmlTextWriterTag.Td)
			{
				if (tagKey3 != HtmlTextWriterTag.Body)
				{
					switch (tagKey3)
					{
					case HtmlTextWriterTag.Table:
					case HtmlTextWriterTag.Td:
						break;
					case HtmlTextWriterTag.Tbody:
						goto IL_01D4;
					default:
						goto IL_01D4;
					}
				}
			}
			else if (tagKey3 != HtmlTextWriterTag.Th && tagKey3 != HtmlTextWriterTag.Tr)
			{
				goto IL_01D4;
			}
			this._tagSupports |= 4;
			IL_01D4:
			HtmlTextWriterTag tagKey4 = base.TagKey;
			if (tagKey4 != HtmlTextWriterTag.Div)
			{
				return;
			}
			if (this.ShouldPerformDivTableSubstitution)
			{
				this._tagSupports |= 8;
			}
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x000CD55C File Offset: 0x000CC55C
		private bool Supports(int flag)
		{
			return (this._tagSupports & flag) == flag;
		}

		// Token: 0x04002123 RID: 8483
		private const int NOTHING = 0;

		// Token: 0x04002124 RID: 8484
		private const int FONT_AROUND_CONTENT = 1;

		// Token: 0x04002125 RID: 8485
		private const int FONT_AROUND_TAG = 2;

		// Token: 0x04002126 RID: 8486
		private const int TABLE_ATTRIBUTES = 4;

		// Token: 0x04002127 RID: 8487
		private const int TABLE_AROUND_CONTENT = 8;

		// Token: 0x04002128 RID: 8488
		private const int FONT_PROPAGATE = 16;

		// Token: 0x04002129 RID: 8489
		private const int FONT_CONSUME = 32;

		// Token: 0x0400212A RID: 8490
		private const int SUPPORTS_HEIGHT_WIDTH = 64;

		// Token: 0x0400212B RID: 8491
		private const int SUPPORTS_BORDER = 128;

		// Token: 0x0400212C RID: 8492
		private const int SUPPORTS_NOWRAP = 256;

		// Token: 0x0400212D RID: 8493
		private StringBuilder _afterContent;

		// Token: 0x0400212E RID: 8494
		private StringBuilder _afterTag;

		// Token: 0x0400212F RID: 8495
		private StringBuilder _beforeContent;

		// Token: 0x04002130 RID: 8496
		private StringBuilder _beforeTag;

		// Token: 0x04002131 RID: 8497
		private string _fontColor;

		// Token: 0x04002132 RID: 8498
		private string _fontFace;

		// Token: 0x04002133 RID: 8499
		private string _fontSize;

		// Token: 0x04002134 RID: 8500
		private Stack _fontStack;

		// Token: 0x04002135 RID: 8501
		private bool _shouldPerformDivTableSubstitution;

		// Token: 0x04002136 RID: 8502
		private bool _renderFontTag;

		// Token: 0x04002137 RID: 8503
		private bool _supportsBold = true;

		// Token: 0x04002138 RID: 8504
		private bool _supportsItalic = true;

		// Token: 0x04002139 RID: 8505
		private int _tagSupports;

		// Token: 0x020003A8 RID: 936
		private sealed class FontStackItem
		{
			// Token: 0x0400213A RID: 8506
			public string name;

			// Token: 0x0400213B RID: 8507
			public string color;

			// Token: 0x0400213C RID: 8508
			public string size;

			// Token: 0x0400213D RID: 8509
			public bool bold;

			// Token: 0x0400213E RID: 8510
			public bool italic;

			// Token: 0x0400213F RID: 8511
			public bool underline;

			// Token: 0x04002140 RID: 8512
			public bool strikeout;
		}
	}
}
