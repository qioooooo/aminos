using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Web.Handlers;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000739 RID: 1849
	internal sealed class WebPartMenu
	{
		// Token: 0x060059C4 RID: 22980 RVA: 0x0016A5E5 File Offset: 0x001695E5
		public WebPartMenu(IWebPartMenuUser menuUser)
		{
			this._menuUser = menuUser;
		}

		// Token: 0x17001731 RID: 5937
		// (get) Token: 0x060059C5 RID: 22981 RVA: 0x0016A5F4 File Offset: 0x001695F4
		private static string DefaultCheckImageUrl
		{
			get
			{
				if (WebPartMenu._defaultCheckImageUrl == null)
				{
					WebPartMenu._defaultCheckImageUrl = AssemblyResourceLoader.GetWebResourceUrl(typeof(WebPartMenu), "WebPartMenu_Check.gif");
				}
				return WebPartMenu._defaultCheckImageUrl;
			}
		}

		// Token: 0x060059C6 RID: 22982 RVA: 0x0016A61C File Offset: 0x0016961C
		private void RegisterStartupScript(string clientID)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			Style itemStyle = this._menuUser.ItemStyle;
			if (itemStyle != null)
			{
				text = itemStyle.GetStyleAttributes(this._menuUser.UrlResolver).Value;
			}
			Style itemHoverStyle = this._menuUser.ItemHoverStyle;
			if (itemHoverStyle != null)
			{
				text2 = itemHoverStyle.GetStyleAttributes(this._menuUser.UrlResolver).Value;
			}
			string text3 = string.Empty;
			string text4 = string.Empty;
			Style labelHoverStyle = this._menuUser.LabelHoverStyle;
			if (labelHoverStyle != null)
			{
				Color foreColor = labelHoverStyle.ForeColor;
				if (!foreColor.IsEmpty)
				{
					text3 = ColorTranslator.ToHtml(foreColor);
				}
				text4 = labelHoverStyle.RegisteredCssClass;
			}
			string text5 = string.Concat(new string[]
			{
				"\r\n<script type=\"text/javascript\">\r\nvar menu",
				clientID,
				" = new WebPartMenu(document.getElementById('",
				clientID,
				"'), document.getElementById('",
				clientID,
				"Popup'), document.getElementById('",
				clientID,
				"Menu'));\r\nmenu",
				clientID,
				".itemStyle = '",
				Util.QuoteJScriptString(text),
				"';\r\nmenu",
				clientID,
				".itemHoverStyle = '",
				Util.QuoteJScriptString(text2),
				"';\r\nmenu",
				clientID,
				".labelHoverColor = '",
				text3,
				"';\r\nmenu",
				clientID,
				".labelHoverClassName = '",
				text4,
				"';\r\n</script>\r\n"
			});
			if (this._menuUser.Page != null)
			{
				this._menuUser.Page.ClientScript.RegisterStartupScript((Control)this._menuUser, typeof(WebPartMenu), clientID, text5, false);
				IScriptManager scriptManager = this._menuUser.Page.ScriptManager;
				if (scriptManager != null && scriptManager.SupportsPartialRendering)
				{
					scriptManager.RegisterDispose((Control)this._menuUser, "document.getElementById('" + clientID + "').__menu.Dispose();");
				}
			}
		}

		// Token: 0x060059C7 RID: 22983 RVA: 0x0016A820 File Offset: 0x00169820
		private void RegisterStyle(Style style)
		{
			if (style != null && !style.IsEmpty)
			{
				string text = this._menuUser.ClientID + "__Menu_" + this._cssStyleIndex++.ToString(NumberFormatInfo.InvariantInfo);
				this._menuUser.Page.Header.StyleSheet.CreateStyleRule(style, this._menuUser.UrlResolver, "." + text);
				style.SetRegisteredCssClass(text);
			}
		}

		// Token: 0x060059C8 RID: 22984 RVA: 0x0016A8A4 File Offset: 0x001698A4
		public void RegisterStyles()
		{
			this.RegisterStyle(this._menuUser.LabelStyle);
			this.RegisterStyle(this._menuUser.LabelHoverStyle);
		}

		// Token: 0x060059C9 RID: 22985 RVA: 0x0016A8C8 File Offset: 0x001698C8
		public void Render(HtmlTextWriter writer, string clientID)
		{
			this.RenderLabel(writer, clientID, null);
		}

		// Token: 0x060059CA RID: 22986 RVA: 0x0016A8D3 File Offset: 0x001698D3
		public void Render(HtmlTextWriter writer, ICollection verbs, string clientID, WebPart associatedWebPart, WebPartManager webPartManager)
		{
			this.RegisterStartupScript(clientID);
			this.RenderLabel(writer, clientID, associatedWebPart);
			this.RenderMenuPopup(writer, verbs, clientID, associatedWebPart, webPartManager);
		}

		// Token: 0x060059CB RID: 22987 RVA: 0x0016A8F4 File Offset: 0x001698F4
		private void RenderLabel(HtmlTextWriter writer, string clientID, WebPart associatedWebPart)
		{
			this._menuUser.OnBeginRender(writer);
			if (associatedWebPart != null)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Id, clientID);
				Style labelStyle = this._menuUser.LabelStyle;
				if (labelStyle != null)
				{
					labelStyle.AddAttributesToRender(writer, this._menuUser as WebControl);
				}
			}
			writer.AddStyleAttribute(HtmlTextWriterStyle.Cursor, "hand");
			writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "inline-block");
			writer.AddStyleAttribute(HtmlTextWriterStyle.Padding, "1px");
			writer.AddStyleAttribute(HtmlTextWriterStyle.TextDecoration, "none");
			writer.RenderBeginTag(HtmlTextWriterTag.Span);
			string labelImageUrl = this._menuUser.LabelImageUrl;
			string labelText = this._menuUser.LabelText;
			if (!string.IsNullOrEmpty(labelImageUrl))
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Src, labelImageUrl);
				writer.AddAttribute(HtmlTextWriterAttribute.Alt, (!string.IsNullOrEmpty(labelText)) ? labelText : SR.GetString("WebPartMenu_DefaultDropDownAlternateText"), true);
				writer.AddStyleAttribute("vertical-align", "middle");
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "none");
				writer.RenderBeginTag(HtmlTextWriterTag.Img);
				writer.RenderEndTag();
				writer.Write("&nbsp;");
			}
			if (!string.IsNullOrEmpty(labelText))
			{
				writer.Write(labelText);
				writer.Write("&nbsp;");
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Id, clientID + "Popup");
			string popupImageUrl = this._menuUser.PopupImageUrl;
			if (!string.IsNullOrEmpty(popupImageUrl))
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Src, popupImageUrl);
				writer.AddAttribute(HtmlTextWriterAttribute.Alt, (!string.IsNullOrEmpty(labelText)) ? labelText : SR.GetString("WebPartMenu_DefaultDropDownAlternateText"), true);
				writer.AddStyleAttribute("vertical-align", "middle");
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "none");
				writer.RenderBeginTag(HtmlTextWriterTag.Img);
				writer.RenderEndTag();
			}
			else
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "Marlett");
				writer.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "8pt");
				writer.RenderBeginTag(HtmlTextWriterTag.Span);
				writer.Write("u");
				writer.RenderEndTag();
			}
			writer.RenderEndTag();
			this._menuUser.OnEndRender(writer);
		}

		// Token: 0x060059CC RID: 22988 RVA: 0x0016AAC8 File Offset: 0x00169AC8
		private void RenderMenuPopup(HtmlTextWriter writer, ICollection verbs, string clientID, WebPart associatedWebPart, WebPartManager webPartManager)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Id, clientID + "Menu");
			writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			bool flag = true;
			WebPartMenuStyle menuPopupStyle = this._menuUser.MenuPopupStyle;
			if (menuPopupStyle != null)
			{
				menuPopupStyle.AddAttributesToRender(writer, this._menuUser as WebControl);
				flag = menuPopupStyle.Width.IsEmpty;
			}
			else
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
				writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "1");
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderCollapse, "collapse");
			}
			if (flag)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
			}
			writer.RenderBeginTag(HtmlTextWriterTag.Table);
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			writer.AddStyleAttribute(HtmlTextWriterStyle.WhiteSpace, "nowrap");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			bool isEnabled = associatedWebPart.Zone.IsEnabled;
			foreach (object obj in verbs)
			{
				WebPartVerb webPartVerb = (WebPartVerb)obj;
				writer.RenderBeginTag(HtmlTextWriterTag.Div);
				string text;
				if (associatedWebPart != null)
				{
					text = string.Format(CultureInfo.CurrentCulture, webPartVerb.Description, new object[] { associatedWebPart.DisplayTitle });
				}
				else
				{
					text = webPartVerb.Description;
				}
				if (text.Length != 0)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Title, text);
				}
				bool flag2 = isEnabled && webPartVerb.Enabled;
				if (webPartVerb is WebPartHelpVerb)
				{
					string text2 = ((IUrlResolutionService)associatedWebPart).ResolveClientUrl(associatedWebPart.HelpUrl);
					writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
					if (flag2)
					{
						writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Concat(new string[]
						{
							"document.body.__wpm.ShowHelp('",
							Util.QuoteJScriptString(text2),
							"', ",
							((int)associatedWebPart.HelpMode).ToString(CultureInfo.InvariantCulture),
							")"
						}));
					}
				}
				else if (webPartVerb is WebPartExportVerb)
				{
					string exportUrl = webPartManager.GetExportUrl(associatedWebPart);
					writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
					if (flag2)
					{
						writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "document.body.__wpm.ExportWebPart('" + Util.QuoteJScriptString(exportUrl) + ((associatedWebPart.ExportMode == WebPartExportMode.All) ? "', true, false)" : "', false, false)"));
					}
				}
				else
				{
					string postBackTarget = this._menuUser.PostBackTarget;
					writer.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:void(0)");
					if (flag2)
					{
						string text3 = webPartVerb.EventArgument;
						if (associatedWebPart != null)
						{
							text3 = webPartVerb.GetEventArgument(associatedWebPart.ID);
						}
						string text4 = null;
						if (!string.IsNullOrEmpty(text3))
						{
							text4 = string.Concat(new string[]
							{
								"document.body.__wpm.SubmitPage('",
								Util.QuoteJScriptString(postBackTarget),
								"', '",
								Util.QuoteJScriptString(text3),
								"');"
							});
							this._menuUser.Page.ClientScript.RegisterForEventValidation(postBackTarget, text3);
						}
						string text5 = null;
						if (!string.IsNullOrEmpty(webPartVerb.ClientClickHandler))
						{
							text5 = "document.body.__wpm.Execute('" + Util.QuoteJScriptString(Util.EnsureEndWithSemiColon(webPartVerb.ClientClickHandler)) + "')";
						}
						string text6 = string.Empty;
						if (text4 != null && text5 != null)
						{
							text6 = string.Concat(new string[] { "if(", text5, "){", text4, "}" });
						}
						else if (text4 != null)
						{
							text6 = text4;
						}
						else if (text5 != null)
						{
							text6 = text5;
						}
						if (webPartVerb is WebPartCloseVerb)
						{
							ProviderConnectionPointCollection providerConnectionPoints = webPartManager.GetProviderConnectionPoints(associatedWebPart);
							if (providerConnectionPoints != null && providerConnectionPoints.Count > 0 && webPartManager.Connections.ContainsProvider(associatedWebPart))
							{
								text6 = "if(document.body.__wpmCloseProviderWarning.length == 0 || confirm(document.body.__wpmCloseProviderWarning)){" + text6 + "}";
							}
						}
						else if (webPartVerb is WebPartDeleteVerb)
						{
							text6 = "if(document.body.__wpmDeleteWarning.length == 0 || confirm(document.body.__wpmDeleteWarning)){" + text6 + "}";
						}
						writer.AddAttribute(HtmlTextWriterAttribute.Onclick, text6);
					}
				}
				writer.AddAttribute(HtmlTextWriterAttribute.Class, "menuItem");
				if (!webPartVerb.Enabled)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
				}
				writer.RenderBeginTag(HtmlTextWriterTag.A);
				string text7 = webPartVerb.ImageUrl;
				if (text7.Length != 0)
				{
					text7 = this._menuUser.UrlResolver.ResolveClientUrl(text7);
				}
				else if (webPartVerb.Checked)
				{
					text7 = this._menuUser.CheckImageUrl;
					if (text7.Length == 0)
					{
						text7 = WebPartMenu.DefaultCheckImageUrl;
					}
				}
				else
				{
					text7 = webPartManager.SpacerImageUrl;
				}
				writer.AddAttribute(HtmlTextWriterAttribute.Src, text7);
				writer.AddAttribute(HtmlTextWriterAttribute.Alt, text, true);
				writer.AddAttribute(HtmlTextWriterAttribute.Width, "16");
				writer.AddAttribute(HtmlTextWriterAttribute.Height, "16");
				writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "none");
				writer.AddStyleAttribute("vertical-align", "middle");
				if (webPartVerb.Checked)
				{
					Style checkImageStyle = this._menuUser.CheckImageStyle;
					if (checkImageStyle != null)
					{
						checkImageStyle.AddAttributesToRender(writer, this._menuUser as WebControl);
					}
				}
				writer.RenderBeginTag(HtmlTextWriterTag.Img);
				writer.RenderEndTag();
				writer.Write("&nbsp;");
				writer.Write(webPartVerb.Text);
				writer.Write("&nbsp;");
				writer.RenderEndTag();
				writer.RenderEndTag();
			}
			writer.RenderEndTag();
			writer.RenderEndTag();
			writer.RenderEndTag();
			writer.RenderEndTag();
		}

		// Token: 0x04003063 RID: 12387
		private static string _defaultCheckImageUrl;

		// Token: 0x04003064 RID: 12388
		private int _cssStyleIndex;

		// Token: 0x04003065 RID: 12389
		private IWebPartMenuUser _menuUser;
	}
}
