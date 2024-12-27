using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x0200070D RID: 1805
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebPartChrome
	{
		// Token: 0x060057CD RID: 22477 RVA: 0x001615EC File Offset: 0x001605EC
		public WebPartChrome(WebPartZoneBase zone, WebPartManager manager)
		{
			if (zone == null)
			{
				throw new ArgumentNullException("zone");
			}
			this._zone = zone;
			this._page = zone.Page;
			this._designMode = zone.DesignMode;
			this._manager = manager;
			if (this._designMode)
			{
				this._personalizationEnabled = true;
			}
			else
			{
				this._personalizationEnabled = manager != null && manager.Personalization.IsModifiable;
			}
			if (manager != null)
			{
				this._personalizationScope = manager.Personalization.Scope;
				return;
			}
			this._personalizationScope = PersonalizationScope.Shared;
		}

		// Token: 0x170016AB RID: 5803
		// (get) Token: 0x060057CE RID: 22478 RVA: 0x00161677 File Offset: 0x00160677
		private WebPartConnectionCollection Connections
		{
			get
			{
				if (this._connections == null)
				{
					this._connections = this._manager.Connections;
				}
				return this._connections;
			}
		}

		// Token: 0x170016AC RID: 5804
		// (get) Token: 0x060057CF RID: 22479 RVA: 0x00161698 File Offset: 0x00160698
		protected bool DragDropEnabled
		{
			get
			{
				return this.Zone.DragDropEnabled;
			}
		}

		// Token: 0x170016AD RID: 5805
		// (get) Token: 0x060057D0 RID: 22480 RVA: 0x001616A5 File Offset: 0x001606A5
		protected WebPartManager WebPartManager
		{
			get
			{
				return this._manager;
			}
		}

		// Token: 0x170016AE RID: 5806
		// (get) Token: 0x060057D1 RID: 22481 RVA: 0x001616AD File Offset: 0x001606AD
		protected WebPartZoneBase Zone
		{
			get
			{
				return this._zone;
			}
		}

		// Token: 0x060057D2 RID: 22482 RVA: 0x001616B8 File Offset: 0x001606B8
		private Style CreateChromeStyleNoBorder(Style partChromeStyle)
		{
			Style style = new Style();
			style.CopyFrom(this.Zone.PartChromeStyle);
			if (style.BorderStyle != BorderStyle.NotSet)
			{
				style.BorderStyle = BorderStyle.NotSet;
			}
			if (style.BorderWidth != Unit.Empty)
			{
				style.BorderWidth = Unit.Empty;
			}
			if (style.BorderColor != Color.Empty)
			{
				style.BorderColor = Color.Empty;
			}
			return style;
		}

		// Token: 0x060057D3 RID: 22483 RVA: 0x00161728 File Offset: 0x00160728
		private Style CreateChromeStyleWithBorder(Style partChromeStyle)
		{
			Style style = new Style();
			style.CopyFrom(partChromeStyle);
			if (style.BorderStyle == BorderStyle.NotSet)
			{
				style.BorderStyle = BorderStyle.Solid;
			}
			if (style.BorderWidth == Unit.Empty)
			{
				style.BorderWidth = Unit.Pixel(1);
			}
			if (style.BorderColor == Color.Empty)
			{
				style.BorderColor = Color.Black;
			}
			return style;
		}

		// Token: 0x060057D4 RID: 22484 RVA: 0x00161790 File Offset: 0x00160790
		private Style CreateTitleTextStyle(Style partTitleStyle)
		{
			Style style = new Style();
			if (partTitleStyle.ForeColor != Color.Empty)
			{
				style.ForeColor = partTitleStyle.ForeColor;
			}
			style.Font.CopyFrom(partTitleStyle.Font);
			return style;
		}

		// Token: 0x060057D5 RID: 22485 RVA: 0x001617D4 File Offset: 0x001607D4
		private Style CreateTitleStyleWithoutFontOrAlign(Style partTitleStyle)
		{
			Style style = new Style();
			style.CopyFrom(partTitleStyle);
			style.Font.Reset();
			if (style.ForeColor != Color.Empty)
			{
				style.ForeColor = Color.Empty;
			}
			return style;
		}

		// Token: 0x060057D6 RID: 22486 RVA: 0x00161818 File Offset: 0x00160818
		protected virtual Style CreateWebPartChromeStyle(WebPart webPart, PartChromeType chromeType)
		{
			if (webPart == null)
			{
				throw new ArgumentNullException("webPart");
			}
			if (chromeType < PartChromeType.Default || chromeType > PartChromeType.BorderOnly)
			{
				throw new ArgumentOutOfRangeException("chromeType");
			}
			Style style;
			if (chromeType == PartChromeType.BorderOnly || chromeType == PartChromeType.TitleAndBorder)
			{
				if (this._chromeStyleWithBorder == null)
				{
					this._chromeStyleWithBorder = this.CreateChromeStyleWithBorder(this.Zone.PartChromeStyle);
				}
				style = this._chromeStyleWithBorder;
			}
			else
			{
				if (this._chromeStyleNoBorder == null)
				{
					this._chromeStyleNoBorder = this.CreateChromeStyleNoBorder(this.Zone.PartChromeStyle);
				}
				style = this._chromeStyleNoBorder;
			}
			if (this.WebPartManager != null && webPart == this.WebPartManager.SelectedWebPart)
			{
				Style style2 = new Style();
				style2.CopyFrom(style);
				style2.CopyFrom(this.Zone.SelectedPartChromeStyle);
				return style2;
			}
			return style;
		}

		// Token: 0x060057D7 RID: 22487 RVA: 0x001618D4 File Offset: 0x001608D4
		private string GenerateDescriptionText(WebPart webPart)
		{
			string text = webPart.DisplayTitle;
			string description = webPart.Description;
			if (!string.IsNullOrEmpty(description))
			{
				text = text + " - " + description;
			}
			return text;
		}

		// Token: 0x060057D8 RID: 22488 RVA: 0x00161908 File Offset: 0x00160908
		private string GenerateTitleText(WebPart webPart)
		{
			string text = webPart.DisplayTitle;
			string subtitle = webPart.Subtitle;
			if (!string.IsNullOrEmpty(subtitle))
			{
				text = text + " - " + subtitle;
			}
			return text;
		}

		// Token: 0x060057D9 RID: 22489 RVA: 0x00161939 File Offset: 0x00160939
		protected string GetWebPartChromeClientID(WebPart webPart)
		{
			if (webPart == null)
			{
				throw new ArgumentNullException("webPart");
			}
			return webPart.WholePartID;
		}

		// Token: 0x060057DA RID: 22490 RVA: 0x0016194F File Offset: 0x0016094F
		protected string GetWebPartTitleClientID(WebPart webPart)
		{
			if (webPart == null)
			{
				throw new ArgumentNullException("webPart");
			}
			return webPart.TitleBarID;
		}

		// Token: 0x060057DB RID: 22491 RVA: 0x00161965 File Offset: 0x00160965
		protected virtual WebPartVerbCollection GetWebPartVerbs(WebPart webPart)
		{
			if (webPart == null)
			{
				throw new ArgumentNullException("webPart");
			}
			return this.Zone.VerbsForWebPart(webPart);
		}

		// Token: 0x060057DC RID: 22492 RVA: 0x00161984 File Offset: 0x00160984
		protected virtual WebPartVerbCollection FilterWebPartVerbs(WebPartVerbCollection verbs, WebPart webPart)
		{
			if (verbs == null)
			{
				throw new ArgumentNullException("verbs");
			}
			if (webPart == null)
			{
				throw new ArgumentNullException("webPart");
			}
			WebPartVerbCollection webPartVerbCollection = new WebPartVerbCollection();
			foreach (object obj in verbs)
			{
				WebPartVerb webPartVerb = (WebPartVerb)obj;
				if (this.ShouldRenderVerb(webPartVerb, webPart))
				{
					webPartVerbCollection.Add(webPartVerb);
				}
			}
			return webPartVerbCollection;
		}

		// Token: 0x060057DD RID: 22493 RVA: 0x00161A08 File Offset: 0x00160A08
		private void RegisterStyle(Style style)
		{
			if (!style.IsEmpty)
			{
				string text = this.Zone.ClientID + "_" + this._cssStyleIndex++.ToString(NumberFormatInfo.InvariantInfo);
				this._page.Header.StyleSheet.CreateStyleRule(style, this.Zone, "." + text);
				style.SetRegisteredCssClass(text);
			}
		}

		// Token: 0x060057DE RID: 22494 RVA: 0x00161A80 File Offset: 0x00160A80
		public virtual void PerformPreRender()
		{
			if (this._page != null && this._page.SupportsStyleSheets)
			{
				Style partChromeStyle = this.Zone.PartChromeStyle;
				Style partTitleStyle = this.Zone.PartTitleStyle;
				this._chromeStyleWithBorder = this.CreateChromeStyleWithBorder(partChromeStyle);
				this.RegisterStyle(this._chromeStyleWithBorder);
				this._chromeStyleNoBorder = this.CreateChromeStyleNoBorder(partChromeStyle);
				this.RegisterStyle(this._chromeStyleNoBorder);
				this._titleTextStyle = this.CreateTitleTextStyle(partTitleStyle);
				this.RegisterStyle(this._titleTextStyle);
				this._titleStyleWithoutFontOrAlign = this.CreateTitleStyleWithoutFontOrAlign(partTitleStyle);
				this.RegisterStyle(this._titleStyleWithoutFontOrAlign);
				if (this.Zone.RenderClientScript && this.Zone.WebPartVerbRenderMode == WebPartVerbRenderMode.Menu && this.Zone.Menu != null)
				{
					this.Zone.Menu.RegisterStyles();
				}
			}
		}

		// Token: 0x060057DF RID: 22495 RVA: 0x00161B5C File Offset: 0x00160B5C
		protected virtual void RenderPartContents(HtmlTextWriter writer, WebPart webPart)
		{
			if (!string.IsNullOrEmpty(webPart.ConnectErrorMessage))
			{
				if (!this.Zone.ErrorStyle.IsEmpty)
				{
					this.Zone.ErrorStyle.AddAttributesToRender(writer, this.Zone);
				}
				writer.RenderBeginTag(HtmlTextWriterTag.Div);
				writer.WriteEncodedText(webPart.ConnectErrorMessage);
				writer.RenderEndTag();
				return;
			}
			webPart.RenderControl(writer);
		}

		// Token: 0x060057E0 RID: 22496 RVA: 0x00161BC4 File Offset: 0x00160BC4
		private void RenderTitleBar(HtmlTextWriter writer, WebPart webPart)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Table);
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			int num = 1;
			bool showTitleIcons = this.Zone.ShowTitleIcons;
			string text = null;
			if (showTitleIcons)
			{
				text = webPart.TitleIconImageUrl;
				if (!string.IsNullOrEmpty(text))
				{
					num++;
					writer.RenderBeginTag(HtmlTextWriterTag.Td);
					this.RenderTitleIcon(writer, webPart);
					writer.RenderEndTag();
				}
			}
			writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
			TableItemStyle partTitleStyle = this.Zone.PartTitleStyle;
			if (!partTitleStyle.Wrap)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.WhiteSpace, "nowrap");
			}
			HorizontalAlign horizontalAlign = partTitleStyle.HorizontalAlign;
			if (horizontalAlign != HorizontalAlign.NotSet)
			{
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(HorizontalAlign));
				writer.AddAttribute(HtmlTextWriterAttribute.Align, converter.ConvertToString(horizontalAlign).ToLower(CultureInfo.InvariantCulture));
			}
			VerticalAlign verticalAlign = partTitleStyle.VerticalAlign;
			if (verticalAlign != VerticalAlign.NotSet)
			{
				TypeConverter converter2 = TypeDescriptor.GetConverter(typeof(VerticalAlign));
				writer.AddAttribute(HtmlTextWriterAttribute.Valign, converter2.ConvertToString(verticalAlign).ToLower(CultureInfo.InvariantCulture));
			}
			if (this.Zone.RenderClientScript)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Id, this.GetWebPartTitleClientID(webPart));
			}
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			if (showTitleIcons && !string.IsNullOrEmpty(text))
			{
				writer.Write("&nbsp;");
			}
			this.RenderTitleText(writer, webPart);
			writer.RenderEndTag();
			this.RenderVerbsInTitleBar(writer, webPart, num);
			writer.RenderEndTag();
			writer.RenderEndTag();
		}

		// Token: 0x060057E1 RID: 22497 RVA: 0x00161D52 File Offset: 0x00160D52
		private void RenderTitleIcon(HtmlTextWriter writer, WebPart webPart)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Src, this.Zone.ResolveClientUrl(webPart.TitleIconImageUrl));
			writer.AddAttribute(HtmlTextWriterAttribute.Alt, this.GenerateDescriptionText(webPart));
			writer.RenderBeginTag(HtmlTextWriterTag.Img);
			writer.RenderEndTag();
		}

		// Token: 0x060057E2 RID: 22498 RVA: 0x00161D8C File Offset: 0x00160D8C
		private void RenderTitleText(HtmlTextWriter writer, WebPart webPart)
		{
			if (this._titleTextStyle == null)
			{
				this._titleTextStyle = this.CreateTitleTextStyle(this.Zone.PartTitleStyle);
			}
			if (!this._titleTextStyle.IsEmpty)
			{
				this._titleTextStyle.AddAttributesToRender(writer, this.Zone);
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Title, this.GenerateDescriptionText(webPart), true);
			string titleUrl = webPart.TitleUrl;
			string text = this.GenerateTitleText(webPart);
			if (!string.IsNullOrEmpty(titleUrl) && !this.DragDropEnabled)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Href, this.Zone.ResolveClientUrl(titleUrl));
				writer.RenderBeginTag(HtmlTextWriterTag.A);
			}
			else
			{
				writer.RenderBeginTag(HtmlTextWriterTag.Span);
			}
			writer.WriteEncodedText(text);
			writer.RenderEndTag();
			writer.Write("&nbsp;");
		}

		// Token: 0x060057E3 RID: 22499 RVA: 0x00161E44 File Offset: 0x00160E44
		private void RenderVerb(HtmlTextWriter writer, WebPart webPart, WebPartVerb verb)
		{
			bool flag = this.Zone.IsEnabled && verb.Enabled;
			ButtonType titleBarVerbButtonType = this.Zone.TitleBarVerbButtonType;
			WebControl webControl;
			if (verb == this.Zone.HelpVerb)
			{
				string text = this.Zone.ResolveClientUrl(webPart.HelpUrl);
				if (titleBarVerbButtonType == ButtonType.Button)
				{
					ZoneButton zoneButton = new ZoneButton(this.Zone, null);
					if (flag)
					{
						if (this.Zone.RenderClientScript)
						{
							zoneButton.OnClientClick = string.Concat(new string[]
							{
								"__wpm.ShowHelp('",
								Util.QuoteJScriptString(text),
								"', ",
								((int)webPart.HelpMode).ToString(CultureInfo.InvariantCulture),
								");return;"
							});
						}
						else if (webPart.HelpMode != WebPartHelpMode.Navigate)
						{
							zoneButton.OnClientClick = "window.open('" + Util.QuoteJScriptString(text) + "', '_blank', 'scrollbars=yes,resizable=yes,status=no,toolbar=no,menubar=no,location=no');return;";
						}
						else
						{
							zoneButton.OnClientClick = "window.location.href='" + Util.QuoteJScriptString(text) + "';return;";
						}
					}
					zoneButton.Text = verb.Text;
					webControl = zoneButton;
				}
				else
				{
					HyperLink hyperLink = new HyperLink();
					switch (webPart.HelpMode)
					{
					case WebPartHelpMode.Modal:
						if (this.Zone.RenderClientScript)
						{
							hyperLink.NavigateUrl = "javascript:__wpm.ShowHelp('" + Util.QuoteJScriptString(text) + "', 0)";
							goto IL_0187;
						}
						break;
					case WebPartHelpMode.Modeless:
						break;
					case WebPartHelpMode.Navigate:
						hyperLink.NavigateUrl = text;
						goto IL_0187;
					default:
						goto IL_0187;
					}
					hyperLink.NavigateUrl = text;
					hyperLink.Target = "_blank";
					IL_0187:
					hyperLink.Text = verb.Text;
					if (titleBarVerbButtonType == ButtonType.Image)
					{
						hyperLink.ImageUrl = verb.ImageUrl;
					}
					webControl = hyperLink;
				}
			}
			else if (verb == this.Zone.ExportVerb)
			{
				string exportUrl = this._manager.GetExportUrl(webPart);
				if (titleBarVerbButtonType == ButtonType.Button)
				{
					ZoneButton zoneButton2 = new ZoneButton(this.Zone, string.Empty);
					zoneButton2.Text = verb.Text;
					if (flag)
					{
						if (webPart.ExportMode == WebPartExportMode.All && this._personalizationScope == PersonalizationScope.User)
						{
							if (this.Zone.RenderClientScript)
							{
								zoneButton2.OnClientClick = "__wpm.ExportWebPart('" + Util.QuoteJScriptString(exportUrl) + "', true, false);return false;";
							}
							else
							{
								zoneButton2.OnClientClick = "if(__wpmExportWarning.length == 0 || confirm(__wpmExportWarning)){window.location='" + Util.QuoteJScriptString(exportUrl) + "';}return false;";
							}
						}
						else
						{
							zoneButton2.OnClientClick = "window.location='" + Util.QuoteJScriptString(exportUrl) + "';return false;";
						}
					}
					webControl = zoneButton2;
				}
				else
				{
					HyperLink hyperLink2 = new HyperLink();
					hyperLink2.Text = verb.Text;
					if (titleBarVerbButtonType == ButtonType.Image)
					{
						hyperLink2.ImageUrl = verb.ImageUrl;
					}
					hyperLink2.NavigateUrl = exportUrl;
					if (webPart.ExportMode == WebPartExportMode.All)
					{
						if (this.Zone.RenderClientScript)
						{
							hyperLink2.Attributes.Add("onclick", "return __wpm.ExportWebPart('', true, true)");
						}
						else
						{
							string text2 = "return (__wpmExportWarning.length == 0 || confirm(__wpmExportWarning))";
							hyperLink2.Attributes.Add("onclick", text2);
						}
					}
					webControl = hyperLink2;
				}
			}
			else
			{
				string eventArgument = verb.GetEventArgument(webPart.ID);
				string clientClickHandler = verb.ClientClickHandler;
				if (titleBarVerbButtonType == ButtonType.Button)
				{
					ZoneButton zoneButton3 = new ZoneButton(this.Zone, eventArgument);
					zoneButton3.Text = verb.Text;
					if (!string.IsNullOrEmpty(clientClickHandler) && flag)
					{
						zoneButton3.OnClientClick = clientClickHandler;
					}
					webControl = zoneButton3;
				}
				else
				{
					ZoneLinkButton zoneLinkButton = new ZoneLinkButton(this.Zone, eventArgument);
					zoneLinkButton.Text = verb.Text;
					if (titleBarVerbButtonType == ButtonType.Image)
					{
						zoneLinkButton.ImageUrl = verb.ImageUrl;
					}
					if (!string.IsNullOrEmpty(clientClickHandler) && flag)
					{
						zoneLinkButton.OnClientClick = clientClickHandler;
					}
					webControl = zoneLinkButton;
				}
				if (this._manager != null && flag)
				{
					if (verb == this.Zone.CloseVerb)
					{
						ProviderConnectionPointCollection providerConnectionPoints = this._manager.GetProviderConnectionPoints(webPart);
						if (providerConnectionPoints != null && providerConnectionPoints.Count > 0 && this.Connections.ContainsProvider(webPart))
						{
							string text3 = "if (__wpmCloseProviderWarning.length >= 0 && !confirm(__wpmCloseProviderWarning)) { return false; }";
							webControl.Attributes.Add("onclick", text3);
						}
					}
					else if (verb == this.Zone.DeleteVerb)
					{
						string text4 = "if (__wpmDeleteWarning.length >= 0 && !confirm(__wpmDeleteWarning)) { return false; }";
						webControl.Attributes.Add("onclick", text4);
					}
				}
			}
			webControl.ApplyStyle(this.Zone.TitleBarVerbStyle);
			webControl.ToolTip = string.Format(CultureInfo.CurrentCulture, verb.Description, new object[] { webPart.DisplayTitle });
			webControl.Enabled = verb.Enabled;
			webControl.Page = this._page;
			webControl.RenderControl(writer);
		}

		// Token: 0x060057E4 RID: 22500 RVA: 0x001622B8 File Offset: 0x001612B8
		private void RenderVerbs(HtmlTextWriter writer, WebPart webPart, WebPartVerbCollection verbs)
		{
			if (verbs == null)
			{
				throw new ArgumentNullException("verbs");
			}
			WebPartVerb webPartVerb = null;
			foreach (object obj in verbs)
			{
				WebPartVerb webPartVerb2 = (WebPartVerb)obj;
				if (webPartVerb != null && (this.VerbRenderedAsLinkButton(webPartVerb2) || this.VerbRenderedAsLinkButton(webPartVerb)))
				{
					writer.Write("&nbsp;");
				}
				this.RenderVerb(writer, webPart, webPartVerb2);
				webPartVerb = webPartVerb2;
			}
		}

		// Token: 0x060057E5 RID: 22501 RVA: 0x00162340 File Offset: 0x00161340
		private void RenderVerbsInTitleBar(HtmlTextWriter writer, WebPart webPart, int colspan)
		{
			WebPartVerbCollection webPartVerbCollection = this.GetWebPartVerbs(webPart);
			webPartVerbCollection = this.FilterWebPartVerbs(webPartVerbCollection, webPart);
			if (webPartVerbCollection != null && webPartVerbCollection.Count > 0)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.WhiteSpace, "nowrap");
				colspan++;
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				if (this.Zone.RenderClientScript && this.Zone.WebPartVerbRenderMode == WebPartVerbRenderMode.Menu && this.Zone.Menu != null)
				{
					if (this._designMode)
					{
						this.Zone.Menu.Render(writer, webPart.WholePartID + "Verbs");
					}
					else
					{
						this.Zone.Menu.Render(writer, webPartVerbCollection, webPart.WholePartID + "Verbs", webPart, this.WebPartManager);
					}
				}
				else
				{
					this.RenderVerbs(writer, webPart, webPartVerbCollection);
				}
				writer.RenderEndTag();
			}
		}

		// Token: 0x060057E6 RID: 22502 RVA: 0x00162418 File Offset: 0x00161418
		public virtual void RenderWebPart(HtmlTextWriter writer, WebPart webPart)
		{
			if (webPart == null)
			{
				throw new ArgumentNullException("webPart");
			}
			bool flag = this.Zone.LayoutOrientation == Orientation.Vertical;
			PartChromeType effectiveChromeType = this.Zone.GetEffectiveChromeType(webPart);
			Style style = this.CreateWebPartChromeStyle(webPart, effectiveChromeType);
			if (!style.IsEmpty)
			{
				style.AddAttributesToRender(writer, this.Zone);
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "2");
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			if (flag)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
			}
			else if (webPart.ChromeState != PartChromeState.Minimized)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
			}
			if (this.Zone.RenderClientScript)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Id, this.GetWebPartChromeClientID(webPart));
			}
			if (!this._designMode && webPart.Hidden && this.WebPartManager != null && !this.WebPartManager.DisplayMode.ShowHiddenWebParts)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
			}
			writer.RenderBeginTag(HtmlTextWriterTag.Table);
			if (effectiveChromeType == PartChromeType.TitleOnly || effectiveChromeType == PartChromeType.TitleAndBorder)
			{
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				if (this._titleStyleWithoutFontOrAlign == null)
				{
					this._titleStyleWithoutFontOrAlign = this.CreateTitleStyleWithoutFontOrAlign(this.Zone.PartTitleStyle);
				}
				if (!this._titleStyleWithoutFontOrAlign.IsEmpty)
				{
					this._titleStyleWithoutFontOrAlign.AddAttributesToRender(writer, this.Zone);
				}
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				this.RenderTitleBar(writer, webPart);
				writer.RenderEndTag();
				writer.RenderEndTag();
			}
			if (webPart.ChromeState == PartChromeState.Minimized)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
			}
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			if (!flag)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
				writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
			}
			Style partStyle = this.Zone.PartStyle;
			if (!partStyle.IsEmpty)
			{
				partStyle.AddAttributesToRender(writer, this.Zone);
			}
			writer.AddStyleAttribute(HtmlTextWriterStyle.Padding, this.Zone.PartChromePadding.ToString());
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			this.RenderPartContents(writer, webPart);
			writer.RenderEndTag();
			writer.RenderEndTag();
			writer.RenderEndTag();
		}

		// Token: 0x060057E7 RID: 22503 RVA: 0x00162620 File Offset: 0x00161620
		private bool ShouldRenderVerb(WebPartVerb verb, WebPart webPart)
		{
			if (verb == null)
			{
				return false;
			}
			if (!verb.Visible)
			{
				return false;
			}
			if (verb == this.Zone.CloseVerb && (!this._personalizationEnabled || !webPart.AllowClose || !this.Zone.AllowLayoutChange))
			{
				return false;
			}
			if (verb == this.Zone.ConnectVerb && this.WebPartManager != null)
			{
				if (this.WebPartManager.DisplayMode != WebPartManager.ConnectDisplayMode || webPart == this.WebPartManager.SelectedWebPart || !webPart.AllowConnect)
				{
					return false;
				}
				ConsumerConnectionPointCollection enabledConsumerConnectionPoints = this.WebPartManager.GetEnabledConsumerConnectionPoints(webPart);
				ProviderConnectionPointCollection enabledProviderConnectionPoints = this.WebPartManager.GetEnabledProviderConnectionPoints(webPart);
				if ((enabledConsumerConnectionPoints == null || enabledConsumerConnectionPoints.Count == 0) && (enabledProviderConnectionPoints == null || enabledProviderConnectionPoints.Count == 0))
				{
					return false;
				}
			}
			return (verb != this.Zone.DeleteVerb || (this._personalizationEnabled && this.Zone.AllowLayoutChange && !webPart.IsStatic && (!webPart.IsShared || this._personalizationScope != PersonalizationScope.User) && (this.WebPartManager == null || this.WebPartManager.DisplayMode.AllowPageDesign))) && (verb != this.Zone.EditVerb || this.WebPartManager == null || (this.WebPartManager.DisplayMode == WebPartManager.EditDisplayMode && webPart != this.WebPartManager.SelectedWebPart)) && (verb != this.Zone.HelpVerb || !string.IsNullOrEmpty(webPart.HelpUrl)) && (verb != this.Zone.MinimizeVerb || (this._personalizationEnabled && webPart.ChromeState != PartChromeState.Minimized && webPart.AllowMinimize && this.Zone.AllowLayoutChange)) && (verb != this.Zone.RestoreVerb || (this._personalizationEnabled && webPart.ChromeState != PartChromeState.Normal && this.Zone.AllowLayoutChange)) && (verb != this.Zone.ExportVerb || (this._personalizationEnabled && webPart.ExportMode != WebPartExportMode.None));
		}

		// Token: 0x060057E8 RID: 22504 RVA: 0x0016280B File Offset: 0x0016180B
		private bool VerbRenderedAsLinkButton(WebPartVerb verb)
		{
			return this.Zone.TitleBarVerbButtonType == ButtonType.Link || string.IsNullOrEmpty(verb.ImageUrl);
		}

		// Token: 0x04002FB5 RID: 12213
		private const string titleSeparator = " - ";

		// Token: 0x04002FB6 RID: 12214
		private const string descriptionSeparator = " - ";

		// Token: 0x04002FB7 RID: 12215
		private WebPartManager _manager;

		// Token: 0x04002FB8 RID: 12216
		private WebPartConnectionCollection _connections;

		// Token: 0x04002FB9 RID: 12217
		private WebPartZoneBase _zone;

		// Token: 0x04002FBA RID: 12218
		private Page _page;

		// Token: 0x04002FBB RID: 12219
		private bool _designMode;

		// Token: 0x04002FBC RID: 12220
		private bool _personalizationEnabled;

		// Token: 0x04002FBD RID: 12221
		private PersonalizationScope _personalizationScope;

		// Token: 0x04002FBE RID: 12222
		private Style _chromeStyleWithBorder;

		// Token: 0x04002FBF RID: 12223
		private Style _chromeStyleNoBorder;

		// Token: 0x04002FC0 RID: 12224
		private Style _titleTextStyle;

		// Token: 0x04002FC1 RID: 12225
		private Style _titleStyleWithoutFontOrAlign;

		// Token: 0x04002FC2 RID: 12226
		private int _cssStyleIndex;
	}
}
