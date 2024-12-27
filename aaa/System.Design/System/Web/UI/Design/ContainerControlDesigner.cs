using System;
using System.Collections;
using System.Collections.Specialized;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design
{
	// Token: 0x02000331 RID: 817
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ContainerControlDesigner : ControlDesigner
	{
		// Token: 0x06001EEA RID: 7914 RVA: 0x000AE624 File Offset: 0x000AD624
		public ContainerControlDesigner()
		{
			this._defaultFrameStyle = new Style();
			this._defaultFrameStyle.Font.Name = "Tahoma";
			this._defaultFrameStyle.ForeColor = SystemColors.ControlText;
			this._defaultFrameStyle.BackColor = SystemColors.Control;
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06001EEB RID: 7915 RVA: 0x000AE677 File Offset: 0x000AD677
		public override bool AllowResize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06001EEC RID: 7916 RVA: 0x000AE67A File Offset: 0x000AD67A
		internal virtual string DesignTimeHtml
		{
			get
			{
				if (!string.IsNullOrEmpty(this.FrameCaption))
				{
					return "<table height=\"{8}\" width=\"{9}\" style=\"{0}{2}{10}\" cellpadding=1 cellspacing=0>\r\n                <tr>\r\n                    <td nowrap align=center valign=middle style=\"{1}{2}{3}{4}\">{5}</td>\r\n                </tr>\r\n                <tr>\r\n                    <td nowrap style=\"vertical-align:top;{6}{10}\" {7}=0></td>\r\n                </tr>\r\n            </table>";
				}
				return "<div height=\"{8}\" width=\"{9}\" style=\"{0}{2}{3}{4}{6}{10}\" {7}=0></div>";
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06001EED RID: 7917 RVA: 0x000AE694 File Offset: 0x000AD694
		public virtual string FrameCaption
		{
			get
			{
				return this.ID;
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06001EEE RID: 7918 RVA: 0x000AE69C File Offset: 0x000AD69C
		public virtual Style FrameStyle
		{
			get
			{
				return this._defaultFrameStyle;
			}
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06001EEF RID: 7919 RVA: 0x000AE6A4 File Offset: 0x000AD6A4
		internal Style FrameStyleInternal
		{
			get
			{
				return this._defaultFrameStyle;
			}
		}

		// Token: 0x06001EF0 RID: 7920 RVA: 0x000AE6AC File Offset: 0x000AD6AC
		public virtual IDictionary GetDesignTimeCssAttributes()
		{
			IDictionary dictionary = new HybridDictionary();
			this.AddDesignTimeCssAttributes(dictionary);
			return dictionary;
		}

		// Token: 0x06001EF1 RID: 7921 RVA: 0x000AE6C8 File Offset: 0x000AD6C8
		protected virtual void AddDesignTimeCssAttributes(IDictionary styleAttributes)
		{
			if (!base.IsWebControl)
			{
				return;
			}
			WebControl webControl = base.ViewControl as WebControl;
			Unit width = webControl.Width;
			if (!width.IsEmpty && width.Value != 0.0)
			{
				styleAttributes["width"] = width.ToString(CultureInfo.InvariantCulture);
			}
			Unit height = webControl.Height;
			if (!height.IsEmpty && height.Value != 0.0)
			{
				styleAttributes["height"] = height.ToString(CultureInfo.InvariantCulture);
			}
			string text = ColorTranslator.ToHtml(webControl.BackColor);
			if (text.Length > 0)
			{
				styleAttributes["background-color"] = text;
			}
			text = ColorTranslator.ToHtml(webControl.ForeColor);
			if (text.Length > 0)
			{
				styleAttributes["color"] = text;
			}
			text = ColorTranslator.ToHtml(webControl.BorderColor);
			if (text.Length > 0)
			{
				styleAttributes["border-color"] = text;
			}
			Unit borderWidth = webControl.BorderWidth;
			if (!borderWidth.IsEmpty && borderWidth.Value != 0.0)
			{
				styleAttributes["border-width"] = borderWidth.ToString(CultureInfo.InvariantCulture);
			}
			BorderStyle borderStyle = webControl.BorderStyle;
			if (borderStyle != BorderStyle.NotSet)
			{
				styleAttributes["border-style"] = borderStyle;
			}
			else if (!borderWidth.IsEmpty && borderWidth.Value != 0.0)
			{
				styleAttributes["border-style"] = BorderStyle.Solid;
			}
			string name = webControl.Font.Name;
			if (name.Length != 0)
			{
				styleAttributes["font-family"] = HttpUtility.HtmlEncode(name);
			}
			FontUnit size = webControl.Font.Size;
			if (size != FontUnit.Empty)
			{
				styleAttributes["font-size"] = size.ToString(CultureInfo.InvariantCulture);
			}
			bool flag = webControl.Font.Bold;
			if (flag)
			{
				styleAttributes["font-weight"] = "bold";
			}
			flag = webControl.Font.Italic;
			if (flag)
			{
				styleAttributes["font-style"] = "italic";
			}
			string text2 = string.Empty;
			flag = webControl.Font.Underline;
			if (flag)
			{
				text2 += "underline ";
			}
			flag = webControl.Font.Strikeout;
			if (flag)
			{
				text2 += "line-through";
			}
			flag = webControl.Font.Overline;
			if (flag)
			{
				text2 += "overline";
			}
			if (text2.Length > 0)
			{
				styleAttributes["text-decoration"] = text2.Trim();
			}
		}

		// Token: 0x06001EF2 RID: 7922 RVA: 0x000AE961 File Offset: 0x000AD961
		public override string GetPersistenceContent()
		{
			if (base.LocalizedInnerContent != null)
			{
				return base.LocalizedInnerContent;
			}
			return null;
		}

		// Token: 0x06001EF3 RID: 7923 RVA: 0x000AE974 File Offset: 0x000AD974
		public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
		{
			string text = string.Empty;
			if (base.Tag != null)
			{
				try
				{
					text = base.Tag.GetContent();
					if (text == null)
					{
						text = string.Empty;
					}
				}
				catch (Exception)
				{
				}
			}
			return text;
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x000AE9BC File Offset: 0x000AD9BC
		public override void SetEditableDesignerRegionContent(EditableDesignerRegion region, string content)
		{
			if (base.Tag != null)
			{
				try
				{
					base.Tag.SetContent(content);
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06001EF5 RID: 7925 RVA: 0x000AE9F4 File Offset: 0x000AD9F4
		private IDictionary ConvertFontInfoToCss(FontInfo font)
		{
			IDictionary dictionary = new HybridDictionary();
			string name = font.Name;
			if (name.Length != 0)
			{
				dictionary["font-family"] = HttpUtility.HtmlEncode(name);
			}
			FontUnit size = font.Size;
			if (size != FontUnit.Empty)
			{
				dictionary["font-size"] = size.ToString(CultureInfo.CurrentCulture);
			}
			bool flag = font.Bold;
			if (flag)
			{
				dictionary["font-weight"] = "bold";
			}
			flag = font.Italic;
			if (flag)
			{
				dictionary["font-style"] = "italic";
			}
			string text = string.Empty;
			flag = font.Underline;
			if (flag)
			{
				text += "underline ";
			}
			flag = font.Strikeout;
			if (flag)
			{
				text += "line-through";
			}
			flag = font.Overline;
			if (flag)
			{
				text += "overline";
			}
			if (text.Length > 0)
			{
				dictionary["text-decoration"] = text.Trim();
			}
			return dictionary;
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x000AEAF4 File Offset: 0x000ADAF4
		private string CssDictionaryToString(IDictionary dictionary)
		{
			string text = string.Empty;
			if (dictionary != null)
			{
				foreach (object obj in dictionary)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					object obj2 = text;
					text = string.Concat(new object[] { obj2, dictionaryEntry.Key, ":", dictionaryEntry.Value, "; " });
				}
			}
			return text;
		}

		// Token: 0x06001EF7 RID: 7927 RVA: 0x000AEB90 File Offset: 0x000ADB90
		private string GenerateDesignTimeHtml()
		{
			string text = this.FrameCaption;
			Unit unit = this.FrameStyle.Height;
			Unit unit2 = this.FrameStyle.Width;
			string text2 = string.Empty;
			if (base.IsWebControl)
			{
				WebControl webControl = (WebControl)base.ViewControl;
				if (unit.IsEmpty)
				{
					unit = webControl.Height;
				}
				if (unit2.IsEmpty)
				{
					unit2 = webControl.Width;
				}
				text2 = webControl.CssClass;
			}
			if (text == null)
			{
				text = string.Empty;
			}
			else if (text.IndexOf('\0') > -1)
			{
				text = text.Replace("\0", string.Empty);
			}
			string text3 = string.Empty;
			WebControl webControl2 = base.ViewControl as WebControl;
			if (webControl2 != null)
			{
				text3 = webControl2.Style.Value + ";";
			}
			string text4 = string.Empty;
			string text5 = string.Empty;
			if ((!this.FrameStyle.BorderWidth.IsEmpty && this.FrameStyle.BorderWidth.Value != 0.0) || this.FrameStyle.BorderStyle != BorderStyle.NotSet || !this.FrameStyle.BorderColor.IsEmpty)
			{
				text4 = string.Format(CultureInfo.InvariantCulture, "border:{0} {1} {2}; ", new object[]
				{
					this.FrameStyle.BorderWidth,
					this.FrameStyle.BorderStyle,
					ColorTranslator.ToHtml(this.FrameStyle.BorderColor)
				});
				text5 = string.Format(CultureInfo.InvariantCulture, "border-bottom:{0} {1} {2}; ", new object[]
				{
					this.FrameStyle.BorderWidth,
					this.FrameStyle.BorderStyle,
					ColorTranslator.ToHtml(this.FrameStyle.BorderColor)
				});
			}
			string text6 = string.Empty;
			if (!this.FrameStyle.ForeColor.IsEmpty)
			{
				text6 = string.Format(CultureInfo.InvariantCulture, "color:{0}; ", new object[] { ColorTranslator.ToHtml(this.FrameStyle.ForeColor) });
			}
			string text7 = string.Empty;
			if (!this.FrameStyle.BackColor.IsEmpty)
			{
				text7 = string.Format(CultureInfo.InvariantCulture, "background-color:{0}; ", new object[] { ColorTranslator.ToHtml(this.FrameStyle.BackColor) });
			}
			return string.Format(CultureInfo.InvariantCulture, this.DesignTimeHtml, new object[]
			{
				text4,
				text5,
				text6,
				text7,
				this.CssDictionaryToString(this.ConvertFontInfoToCss(this.FrameStyle.Font)),
				text,
				this.CssDictionaryToString(this.GetDesignTimeCssAttributes()),
				DesignerRegion.DesignerRegionAttributeName,
				unit,
				unit2,
				text3,
				text2
			});
		}

		// Token: 0x06001EF8 RID: 7928 RVA: 0x000AEE94 File Offset: 0x000ADE94
		public override string GetDesignTimeHtml(DesignerRegionCollection regions)
		{
			Control control = (Control)base.Component;
			control.Controls.Clear();
			EditableDesignerRegion editableDesignerRegion = new EditableDesignerRegion(this, "Content");
			editableDesignerRegion.Description = SR.GetString("ContainerControlDesigner_RegionWatermark");
			editableDesignerRegion.Properties[typeof(Control)] = control;
			editableDesignerRegion.EnsureSize = true;
			regions.Add(editableDesignerRegion);
			return this.GenerateDesignTimeHtml();
		}

		// Token: 0x0400175A RID: 5978
		private const string ContainerControlWithCaptionDesignTimeHtml = "<table height=\"{8}\" width=\"{9}\" style=\"{0}{2}{10}\" cellpadding=1 cellspacing=0>\r\n                <tr>\r\n                    <td nowrap align=center valign=middle style=\"{1}{2}{3}{4}\">{5}</td>\r\n                </tr>\r\n                <tr>\r\n                    <td nowrap style=\"vertical-align:top;{6}{10}\" {7}=0></td>\r\n                </tr>\r\n            </table>";

		// Token: 0x0400175B RID: 5979
		private const string ContainerControlNoCaptionDesignTimeHtml = "<div height=\"{8}\" width=\"{9}\" style=\"{0}{2}{3}{4}{6}{10}\" {7}=0></div>";

		// Token: 0x0400175C RID: 5980
		private Style _defaultFrameStyle;
	}
}
