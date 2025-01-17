﻿using System;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;
using System.Text;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design
{
	// Token: 0x02000398 RID: 920
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class TemplateEditingFrame : ITemplateEditingFrame, IDisposable
	{
		// Token: 0x060021F5 RID: 8693 RVA: 0x000BAB00 File Offset: 0x000B9B00
		public TemplateEditingFrame(TemplatedControlDesigner owner, string frameName, string[] templateNames, Style controlStyle, Style[] templateStyles)
		{
			this.owner = owner;
			this.frameName = frameName;
			this.controlStyle = controlStyle;
			this.templateStyles = templateStyles;
			this.verb = null;
			this.templateNames = (string[])templateNames.Clone();
			if (owner.BehaviorInternal != null)
			{
				NativeMethods.IHTMLElement ihtmlelement = (NativeMethods.IHTMLElement)((IControlDesignerBehavior)owner.BehaviorInternal).DesignTimeElementView;
				this.htmlElemParent = ihtmlelement;
			}
			this.htmlElemControlName = null;
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x060021F6 RID: 8694 RVA: 0x000BAB75 File Offset: 0x000B9B75
		private string Content
		{
			get
			{
				if (this.frameContent == null)
				{
					this.frameContent = this.CreateFrameContent();
				}
				return this.frameContent;
			}
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x060021F7 RID: 8695 RVA: 0x000BAB91 File Offset: 0x000B9B91
		public Style ControlStyle
		{
			get
			{
				return this.controlStyle;
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x060021F8 RID: 8696 RVA: 0x000BAB99 File Offset: 0x000B9B99
		public string Name
		{
			get
			{
				return this.frameName;
			}
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x060021F9 RID: 8697 RVA: 0x000BABA1 File Offset: 0x000B9BA1
		// (set) Token: 0x060021FA RID: 8698 RVA: 0x000BABA9 File Offset: 0x000B9BA9
		public int InitialHeight
		{
			get
			{
				return this.initialHeight;
			}
			set
			{
				this.initialHeight = value;
			}
		}

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x060021FB RID: 8699 RVA: 0x000BABB2 File Offset: 0x000B9BB2
		// (set) Token: 0x060021FC RID: 8700 RVA: 0x000BABBA File Offset: 0x000B9BBA
		public int InitialWidth
		{
			get
			{
				return this.initialWidth;
			}
			set
			{
				this.initialWidth = value;
			}
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x060021FD RID: 8701 RVA: 0x000BABC3 File Offset: 0x000B9BC3
		public string[] TemplateNames
		{
			get
			{
				return this.templateNames;
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x060021FE RID: 8702 RVA: 0x000BABCB File Offset: 0x000B9BCB
		public Style[] TemplateStyles
		{
			get
			{
				return this.templateStyles;
			}
		}

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x060021FF RID: 8703 RVA: 0x000BABD3 File Offset: 0x000B9BD3
		// (set) Token: 0x06002200 RID: 8704 RVA: 0x000BABDB File Offset: 0x000B9BDB
		public TemplateEditingVerb Verb
		{
			get
			{
				return this.verb;
			}
			set
			{
				this.verb = value;
			}
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x000BABE4 File Offset: 0x000B9BE4
		public void Close(bool saveChanges)
		{
			if (saveChanges)
			{
				this.Save();
			}
			this.ShowInternal(false);
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x000BABF8 File Offset: 0x000B9BF8
		private string CreateFrameContent()
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			string text = string.Empty;
			if (this.initialWidth > 0)
			{
				text = "width:" + this.initialWidth + "px;";
			}
			if (this.initialHeight > 0)
			{
				object obj = text;
				text = string.Concat(new object[] { obj, "height:", this.initialHeight, "px;" });
			}
			stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<table cellspacing=0 cellpadding=0 border=0 style=\"{4}\">\r\n              <tr>\r\n                <td>\r\n                  <table cellspacing=0 cellpadding=2 border=0 width=100% height=100%>\r\n                    <tr style=\"background-color:buttonshadow\">\r\n                      <td>\r\n                        <table cellspacing=0 cellpadding=0 border=0 width=100% height=100%>\r\n                          <tr>\r\n                            <td valign=middle style=\"font:messagebox;font-weight:bold;color:buttonhighlight\">&nbsp;<span id=\"idControlName\">{0}</span> - <span id=\"idFrameName\">{1}</span>&nbsp;&nbsp;&nbsp;</td>\r\n                            <td align=right valign=middle>&nbsp;<img src=\"{2}\" height=13 width=14 title=\"{3}\">&nbsp;</td>\r\n                          </tr>\r\n                        </table>\r\n                      </td>\r\n                    </tr>\r\n                  </table>\r\n                </td>\r\n              </tr>", new object[]
			{
				this.owner.Component.GetType().Name,
				this.Name,
				TemplateEditingFrame.TemplateInfoIcon,
				TemplateEditingFrame.TemplateInfoToolTip,
				text
			}));
			string text2 = string.Empty;
			if (this.controlStyle != null)
			{
				text2 = this.StyleToCss(this.controlStyle);
			}
			string text3 = string.Empty;
			for (int i = 0; i < this.templateNames.Length; i++)
			{
				stringBuilder.Append("<tr style=\"height:1px\"><td style=\"font-size:0pt\"></td></tr>");
				if (this.templateStyles != null)
				{
					text3 = this.StyleToCss(this.templateStyles[i]);
				}
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<tr>\r\n                <td>\r\n                  <table cellspacing=0 cellpadding=2 border=0 width=100% height=100% style=\"border:solid 1px buttonface\">\r\n                    <tr style=\"font:messagebox;background-color:buttonface;color:buttonshadow\">\r\n                      <td style=\"border-bottom:solid 1px buttonshadow\">\r\n                        &nbsp;{0}&nbsp;&nbsp;&nbsp;\r\n                      </td>\r\n                    </tr>\r\n                    <tr style=\"{1}\" height=100%>\r\n                      <td style=\"{2}\">\r\n                        <div style=\"width:100%;height:100%\" id=\"{0}\"></div>\r\n                      </td>\r\n                    </tr>\r\n                  </table>\r\n                </td>\r\n              </tr>", new object[]
				{
					this.templateNames[i],
					text2,
					text3
				}));
			}
			stringBuilder.Append("</table>");
			return stringBuilder.ToString();
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x000BAD7C File Offset: 0x000B9D7C
		public void Dispose()
		{
			if (this.owner != null && this.owner.InTemplateMode)
			{
				this.owner.ExitTemplateMode(false, false, false);
			}
			this.ReleaseParentElement();
			if (this.verb != null)
			{
				this.verb.Dispose();
				this.verb = null;
			}
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x000BADCC File Offset: 0x000B9DCC
		private void Initialize()
		{
			if (this.htmlElemFrame != null)
			{
				return;
			}
			try
			{
				NativeMethods.IHTMLDocument2 document = this.htmlElemParent.GetDocument();
				this.htmlElemFrame = document.CreateElement("SPAN");
				this.htmlElemFrame.SetInnerHTML(this.Content);
				NativeMethods.IHTMLDOMNode ihtmldomnode = (NativeMethods.IHTMLDOMNode)this.htmlElemFrame;
				if (ihtmldomnode != null)
				{
					this.htmlElemContent = (NativeMethods.IHTMLElement)ihtmldomnode.GetFirstChild();
				}
				NativeMethods.IHTMLElement3 ihtmlelement = (NativeMethods.IHTMLElement3)this.htmlElemFrame;
				if (ihtmlelement != null)
				{
					ihtmlelement.SetContentEditable("false");
				}
				this.templateElements = new object[this.templateNames.Length];
				object obj = 0;
				NativeMethods.IHTMLElementCollection ihtmlelementCollection = (NativeMethods.IHTMLElementCollection)this.htmlElemFrame.GetAll();
				object obj2;
				for (int i = 0; i < this.templateNames.Length; i++)
				{
					try
					{
						obj2 = this.templateNames[i];
						NativeMethods.IHTMLElement ihtmlelement2 = ihtmlelementCollection.Item(obj2, obj);
						ihtmlelement2.SetAttribute("templatename", obj2, 0);
						string text = "<DIV contentEditable=\"true\" style=\"padding:1;height:100%;width:100%\"></DIV>";
						ihtmlelement2.SetInnerHTML(text);
						NativeMethods.IHTMLDOMNode ihtmldomnode2 = (NativeMethods.IHTMLDOMNode)ihtmlelement2;
						if (ihtmldomnode2 != null)
						{
							this.templateElements[i] = ihtmldomnode2.GetFirstChild();
						}
					}
					catch (Exception)
					{
						this.templateElements[i] = null;
					}
				}
				obj2 = "idControlName";
				this.htmlElemControlName = ihtmlelementCollection.Item(obj2, obj);
				obj2 = "idFrameName";
				object obj3 = ihtmlelementCollection.Item(obj2, obj);
				if (obj3 != null)
				{
					NativeMethods.IHTMLElement ihtmlelement3 = (NativeMethods.IHTMLElement)obj3;
					ihtmlelement3.SetInnerText(this.frameName);
				}
				NativeMethods.IHTMLDOMNode ihtmldomnode3 = (NativeMethods.IHTMLDOMNode)this.htmlElemParent;
				if (ihtmldomnode3 != null)
				{
					ihtmldomnode3.AppendChild(ihtmldomnode);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x000BAF88 File Offset: 0x000B9F88
		public void Open()
		{
			NativeMethods.IHTMLElement ihtmlelement = (NativeMethods.IHTMLElement)((IControlDesignerBehavior)this.owner.BehaviorInternal).DesignTimeElementView;
			if (this.htmlElemParent != ihtmlelement)
			{
				this.ReleaseParentElement();
				this.htmlElemParent = ihtmlelement;
			}
			this.Initialize();
			try
			{
				for (int i = 0; i < this.templateNames.Length; i++)
				{
					if (this.templateElements[i] != null)
					{
						bool flag = true;
						NativeMethods.IHTMLElement ihtmlelement2 = (NativeMethods.IHTMLElement)this.templateElements[i];
						string text = this.owner.GetTemplateContent(this, this.templateNames[i], out flag);
						ihtmlelement2.SetAttribute("contentEditable", flag, 0);
						if (text != null)
						{
							text = "<body contentEditable=true>" + text + "</body>";
							ihtmlelement2.SetInnerHTML(text);
						}
					}
				}
				if (this.htmlElemControlName != null)
				{
					this.htmlElemControlName.SetInnerText(this.owner.Component.Site.Name);
				}
			}
			catch (Exception)
			{
			}
			this.ShowInternal(true);
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x000BB088 File Offset: 0x000BA088
		private void ReleaseParentElement()
		{
			this.htmlElemParent = null;
			this.htmlElemFrame = null;
			this.htmlElemContent = null;
			this.htmlElemControlName = null;
			this.templateElements = null;
			this.fVisible = false;
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x000BB0B4 File Offset: 0x000BA0B4
		public void Resize(int width, int height)
		{
			if (this.htmlElemContent != null)
			{
				NativeMethods.IHTMLStyle style = this.htmlElemContent.GetStyle();
				if (style != null)
				{
					style.SetPixelWidth(width);
					style.SetPixelHeight(height);
				}
			}
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x000BB0E8 File Offset: 0x000BA0E8
		public void Save()
		{
			try
			{
				if (this.templateElements != null)
				{
					object[] array = new object[1];
					for (int i = 0; i < this.templateNames.Length; i++)
					{
						if (this.templateElements[i] != null)
						{
							NativeMethods.IHTMLElement ihtmlelement = (NativeMethods.IHTMLElement)this.templateElements[i];
							ihtmlelement.GetAttribute("contentEditable", 0, array);
							if (array[0] != null && array[0] is string && string.Compare((string)array[0], "true", StringComparison.OrdinalIgnoreCase) == 0)
							{
								string innerHTML = ihtmlelement.GetInnerHTML();
								this.owner.SetTemplateContent(this, this.templateNames[i], innerHTML);
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06002209 RID: 8713 RVA: 0x000BB194 File Offset: 0x000BA194
		public void Show()
		{
			this.ShowInternal(true);
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x000BB1A0 File Offset: 0x000BA1A0
		private void ShowInternal(bool fShow)
		{
			if (this.htmlElemFrame == null || this.fVisible == fShow)
			{
				return;
			}
			try
			{
				NativeMethods.IHTMLDOMNode ihtmldomnode = (NativeMethods.IHTMLDOMNode)this.htmlElemFrame;
				NativeMethods.IHTMLElement ihtmlelement = (NativeMethods.IHTMLElement)ihtmldomnode;
				NativeMethods.IHTMLStyle style = ihtmlelement.GetStyle();
				if (fShow)
				{
					style.SetDisplay(string.Empty);
				}
				else
				{
					if (this.templateElements != null)
					{
						for (int i = 0; i < this.templateElements.Length; i++)
						{
							if (this.templateElements[i] != null)
							{
								NativeMethods.IHTMLElement ihtmlelement2 = (NativeMethods.IHTMLElement)this.templateElements[i];
								ihtmlelement2.SetInnerHTML(string.Empty);
							}
						}
					}
					style.SetDisplay("none");
				}
			}
			catch (Exception)
			{
			}
			this.fVisible = fShow;
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x000BB250 File Offset: 0x000BA250
		private string StyleToCss(Style style)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Color color = style.ForeColor;
			if (!color.IsEmpty)
			{
				stringBuilder.Append("color:");
				stringBuilder.Append(ColorTranslator.ToHtml(color));
				stringBuilder.Append(";");
			}
			color = style.BackColor;
			if (!color.IsEmpty)
			{
				stringBuilder.Append("background-color:");
				stringBuilder.Append(ColorTranslator.ToHtml(color));
				stringBuilder.Append(";");
			}
			FontInfo font = style.Font;
			string text = font.Name;
			if (text.Length != 0)
			{
				stringBuilder.Append("font-family:'");
				stringBuilder.Append(text);
				stringBuilder.Append("';");
			}
			if (font.Bold)
			{
				stringBuilder.Append("font-weight:bold;");
			}
			if (font.Italic)
			{
				stringBuilder.Append("font-style:italic;");
			}
			text = string.Empty;
			if (font.Underline)
			{
				text += "underline";
			}
			if (font.Strikeout)
			{
				text += " line-through";
			}
			if (font.Overline)
			{
				text += " overline";
			}
			if (text.Length != 0)
			{
				stringBuilder.Append("text-decoration:");
				stringBuilder.Append(text);
				stringBuilder.Append(';');
			}
			FontUnit size = font.Size;
			if (!size.IsEmpty)
			{
				stringBuilder.Append("font-size:");
				stringBuilder.Append(size.ToString(CultureInfo.InvariantCulture));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x000BB3C8 File Offset: 0x000BA3C8
		public void UpdateControlName(string newName)
		{
			if (this.htmlElemControlName != null)
			{
				this.htmlElemControlName.SetInnerText(newName);
			}
		}

		// Token: 0x0400182E RID: 6190
		private const string TemplateFrameHeaderContent = "<table cellspacing=0 cellpadding=0 border=0 style=\"{4}\">\r\n              <tr>\r\n                <td>\r\n                  <table cellspacing=0 cellpadding=2 border=0 width=100% height=100%>\r\n                    <tr style=\"background-color:buttonshadow\">\r\n                      <td>\r\n                        <table cellspacing=0 cellpadding=0 border=0 width=100% height=100%>\r\n                          <tr>\r\n                            <td valign=middle style=\"font:messagebox;font-weight:bold;color:buttonhighlight\">&nbsp;<span id=\"idControlName\">{0}</span> - <span id=\"idFrameName\">{1}</span>&nbsp;&nbsp;&nbsp;</td>\r\n                            <td align=right valign=middle>&nbsp;<img src=\"{2}\" height=13 width=14 title=\"{3}\">&nbsp;</td>\r\n                          </tr>\r\n                        </table>\r\n                      </td>\r\n                    </tr>\r\n                  </table>\r\n                </td>\r\n              </tr>";

		// Token: 0x0400182F RID: 6191
		private const string TemplateFrameFooterContent = "</table>";

		// Token: 0x04001830 RID: 6192
		private const string TemplateFrameSeparatorContent = "<tr style=\"height:1px\"><td style=\"font-size:0pt\"></td></tr>";

		// Token: 0x04001831 RID: 6193
		private const string TemplateFrameTemplateContent = "<tr>\r\n                <td>\r\n                  <table cellspacing=0 cellpadding=2 border=0 width=100% height=100% style=\"border:solid 1px buttonface\">\r\n                    <tr style=\"font:messagebox;background-color:buttonface;color:buttonshadow\">\r\n                      <td style=\"border-bottom:solid 1px buttonshadow\">\r\n                        &nbsp;{0}&nbsp;&nbsp;&nbsp;\r\n                      </td>\r\n                    </tr>\r\n                    <tr style=\"{1}\" height=100%>\r\n                      <td style=\"{2}\">\r\n                        <div style=\"width:100%;height:100%\" id=\"{0}\"></div>\r\n                      </td>\r\n                    </tr>\r\n                  </table>\r\n                </td>\r\n              </tr>";

		// Token: 0x04001832 RID: 6194
		private static readonly string TemplateInfoToolTip = SR.GetString("TemplateEdit_Tip");

		// Token: 0x04001833 RID: 6195
		private static readonly string TemplateInfoIcon = "res://" + typeof(TemplateEditingFrame).Module.FullyQualifiedName + "//TEMPLATE_TIP";

		// Token: 0x04001834 RID: 6196
		private string frameName;

		// Token: 0x04001835 RID: 6197
		private string frameContent;

		// Token: 0x04001836 RID: 6198
		private string[] templateNames;

		// Token: 0x04001837 RID: 6199
		private Style controlStyle;

		// Token: 0x04001838 RID: 6200
		private Style[] templateStyles;

		// Token: 0x04001839 RID: 6201
		private TemplateEditingVerb verb;

		// Token: 0x0400183A RID: 6202
		private int initialWidth;

		// Token: 0x0400183B RID: 6203
		private int initialHeight;

		// Token: 0x0400183C RID: 6204
		private NativeMethods.IHTMLElement htmlElemFrame;

		// Token: 0x0400183D RID: 6205
		private NativeMethods.IHTMLElement htmlElemContent;

		// Token: 0x0400183E RID: 6206
		private NativeMethods.IHTMLElement htmlElemParent;

		// Token: 0x0400183F RID: 6207
		private NativeMethods.IHTMLElement htmlElemControlName;

		// Token: 0x04001840 RID: 6208
		private object[] templateElements;

		// Token: 0x04001841 RID: 6209
		private bool fVisible;

		// Token: 0x04001842 RID: 6210
		private TemplatedControlDesigner owner;
	}
}
