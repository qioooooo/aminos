using System;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;
using System.Text;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class TemplateEditingFrame : ITemplateEditingFrame, IDisposable
	{
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

		public Style ControlStyle
		{
			get
			{
				return this.controlStyle;
			}
		}

		public string Name
		{
			get
			{
				return this.frameName;
			}
		}

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

		public string[] TemplateNames
		{
			get
			{
				return this.templateNames;
			}
		}

		public Style[] TemplateStyles
		{
			get
			{
				return this.templateStyles;
			}
		}

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

		public void Close(bool saveChanges)
		{
			if (saveChanges)
			{
				this.Save();
			}
			this.ShowInternal(false);
		}

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

		private void ReleaseParentElement()
		{
			this.htmlElemParent = null;
			this.htmlElemFrame = null;
			this.htmlElemContent = null;
			this.htmlElemControlName = null;
			this.templateElements = null;
			this.fVisible = false;
		}

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

		public void Show()
		{
			this.ShowInternal(true);
		}

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

		public void UpdateControlName(string newName)
		{
			if (this.htmlElemControlName != null)
			{
				this.htmlElemControlName.SetInnerText(newName);
			}
		}

		private const string TemplateFrameHeaderContent = "<table cellspacing=0 cellpadding=0 border=0 style=\"{4}\">\r\n              <tr>\r\n                <td>\r\n                  <table cellspacing=0 cellpadding=2 border=0 width=100% height=100%>\r\n                    <tr style=\"background-color:buttonshadow\">\r\n                      <td>\r\n                        <table cellspacing=0 cellpadding=0 border=0 width=100% height=100%>\r\n                          <tr>\r\n                            <td valign=middle style=\"font:messagebox;font-weight:bold;color:buttonhighlight\">&nbsp;<span id=\"idControlName\">{0}</span> - <span id=\"idFrameName\">{1}</span>&nbsp;&nbsp;&nbsp;</td>\r\n                            <td align=right valign=middle>&nbsp;<img src=\"{2}\" height=13 width=14 title=\"{3}\">&nbsp;</td>\r\n                          </tr>\r\n                        </table>\r\n                      </td>\r\n                    </tr>\r\n                  </table>\r\n                </td>\r\n              </tr>";

		private const string TemplateFrameFooterContent = "</table>";

		private const string TemplateFrameSeparatorContent = "<tr style=\"height:1px\"><td style=\"font-size:0pt\"></td></tr>";

		private const string TemplateFrameTemplateContent = "<tr>\r\n                <td>\r\n                  <table cellspacing=0 cellpadding=2 border=0 width=100% height=100% style=\"border:solid 1px buttonface\">\r\n                    <tr style=\"font:messagebox;background-color:buttonface;color:buttonshadow\">\r\n                      <td style=\"border-bottom:solid 1px buttonshadow\">\r\n                        &nbsp;{0}&nbsp;&nbsp;&nbsp;\r\n                      </td>\r\n                    </tr>\r\n                    <tr style=\"{1}\" height=100%>\r\n                      <td style=\"{2}\">\r\n                        <div style=\"width:100%;height:100%\" id=\"{0}\"></div>\r\n                      </td>\r\n                    </tr>\r\n                  </table>\r\n                </td>\r\n              </tr>";

		private static readonly string TemplateInfoToolTip = SR.GetString("TemplateEdit_Tip");

		private static readonly string TemplateInfoIcon = "res://" + typeof(TemplateEditingFrame).Module.FullyQualifiedName + "//TEMPLATE_TIP";

		private string frameName;

		private string frameContent;

		private string[] templateNames;

		private Style controlStyle;

		private Style[] templateStyles;

		private TemplateEditingVerb verb;

		private int initialWidth;

		private int initialHeight;

		private NativeMethods.IHTMLElement htmlElemFrame;

		private NativeMethods.IHTMLElement htmlElemContent;

		private NativeMethods.IHTMLElement htmlElemParent;

		private NativeMethods.IHTMLElement htmlElemControlName;

		private object[] templateElements;

		private bool fVisible;

		private TemplatedControlDesigner owner;
	}
}
