using System;
using System.Drawing;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006A8 RID: 1704
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class CatalogPartChrome
	{
		// Token: 0x0600533B RID: 21307 RVA: 0x00151C2D File Offset: 0x00150C2D
		public CatalogPartChrome(CatalogZoneBase zone)
		{
			if (zone == null)
			{
				throw new ArgumentNullException("zone");
			}
			this._zone = zone;
			this._page = zone.Page;
		}

		// Token: 0x1700152B RID: 5419
		// (get) Token: 0x0600533C RID: 21308 RVA: 0x00151C56 File Offset: 0x00150C56
		protected CatalogZoneBase Zone
		{
			get
			{
				return this._zone;
			}
		}

		// Token: 0x0600533D RID: 21309 RVA: 0x00151C60 File Offset: 0x00150C60
		protected virtual Style CreateCatalogPartChromeStyle(CatalogPart catalogPart, PartChromeType chromeType)
		{
			if (catalogPart == null)
			{
				throw new ArgumentNullException("catalogPart");
			}
			if (chromeType < PartChromeType.Default || chromeType > PartChromeType.BorderOnly)
			{
				throw new ArgumentOutOfRangeException("chromeType");
			}
			if (chromeType == PartChromeType.BorderOnly || chromeType == PartChromeType.TitleAndBorder)
			{
				if (this._chromeStyleWithBorder == null)
				{
					Style style = new Style();
					style.CopyFrom(this.Zone.PartChromeStyle);
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
					this._chromeStyleWithBorder = style;
				}
				return this._chromeStyleWithBorder;
			}
			if (this._chromeStyleNoBorder == null)
			{
				Style style2 = new Style();
				style2.CopyFrom(this.Zone.PartChromeStyle);
				if (style2.BorderStyle != BorderStyle.NotSet)
				{
					style2.BorderStyle = BorderStyle.NotSet;
				}
				if (style2.BorderWidth != Unit.Empty)
				{
					style2.BorderWidth = Unit.Empty;
				}
				if (style2.BorderColor != Color.Empty)
				{
					style2.BorderColor = Color.Empty;
				}
				this._chromeStyleNoBorder = style2;
			}
			return this._chromeStyleNoBorder;
		}

		// Token: 0x0600533E RID: 21310 RVA: 0x00151D82 File Offset: 0x00150D82
		public virtual void PerformPreRender()
		{
		}

		// Token: 0x0600533F RID: 21311 RVA: 0x00151D84 File Offset: 0x00150D84
		public virtual void RenderCatalogPart(HtmlTextWriter writer, CatalogPart catalogPart)
		{
			if (catalogPart == null)
			{
				throw new ArgumentNullException("catalogPart");
			}
			PartChromeType effectiveChromeType = this.Zone.GetEffectiveChromeType(catalogPart);
			Style style = this.CreateCatalogPartChromeStyle(catalogPart, effectiveChromeType);
			if (!style.IsEmpty)
			{
				style.AddAttributesToRender(writer, this.Zone);
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "2");
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Table);
			if (effectiveChromeType == PartChromeType.TitleOnly || effectiveChromeType == PartChromeType.TitleAndBorder)
			{
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				Style partTitleStyle = this.Zone.PartTitleStyle;
				if (!partTitleStyle.IsEmpty)
				{
					partTitleStyle.AddAttributesToRender(writer, this.Zone);
				}
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				this.RenderTitle(writer, catalogPart);
				writer.RenderEndTag();
				writer.RenderEndTag();
			}
			if (catalogPart.ChromeState != PartChromeState.Minimized)
			{
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				Style partStyle = this.Zone.PartStyle;
				if (!partStyle.IsEmpty)
				{
					partStyle.AddAttributesToRender(writer, this.Zone);
				}
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				this.RenderPartContents(writer, catalogPart);
				this.RenderItems(writer, catalogPart);
				writer.RenderEndTag();
				writer.RenderEndTag();
			}
			writer.RenderEndTag();
		}

		// Token: 0x06005340 RID: 21312 RVA: 0x00151EAC File Offset: 0x00150EAC
		private void RenderItem(HtmlTextWriter writer, WebPartDescription webPartDescription)
		{
			string text = webPartDescription.Description;
			if (string.IsNullOrEmpty(text))
			{
				text = webPartDescription.Title;
			}
			this.RenderItemCheckBox(writer, webPartDescription.ID);
			writer.Write("&nbsp;");
			if (this.Zone.ShowCatalogIcons)
			{
				string catalogIconImageUrl = webPartDescription.CatalogIconImageUrl;
				if (!string.IsNullOrEmpty(catalogIconImageUrl))
				{
					this.RenderItemIcon(writer, catalogIconImageUrl, text);
					writer.Write("&nbsp;");
				}
			}
			this.RenderItemText(writer, webPartDescription.ID, webPartDescription.Title, text);
			writer.WriteBreak();
		}

		// Token: 0x06005341 RID: 21313 RVA: 0x00151F34 File Offset: 0x00150F34
		private void RenderItemCheckBox(HtmlTextWriter writer, string value)
		{
			this.Zone.EditUIStyle.AddAttributesToRender(writer, this.Zone);
			writer.AddAttribute(HtmlTextWriterAttribute.Type, "checkbox");
			writer.AddAttribute(HtmlTextWriterAttribute.Id, this.Zone.GetCheckBoxID(value));
			writer.AddAttribute(HtmlTextWriterAttribute.Name, this.Zone.CheckBoxName);
			writer.AddAttribute(HtmlTextWriterAttribute.Value, value);
			writer.RenderBeginTag(HtmlTextWriterTag.Input);
			writer.RenderEndTag();
			if (this._page != null)
			{
				this._page.ClientScript.RegisterForEventValidation(this.Zone.CheckBoxName);
			}
		}

		// Token: 0x06005342 RID: 21314 RVA: 0x00151FC8 File Offset: 0x00150FC8
		private void RenderItemIcon(HtmlTextWriter writer, string iconUrl, string description)
		{
			new Image
			{
				AlternateText = description,
				ImageUrl = iconUrl,
				BorderStyle = BorderStyle.None,
				Page = this._page
			}.RenderControl(writer);
		}

		// Token: 0x06005343 RID: 21315 RVA: 0x00152004 File Offset: 0x00151004
		private void RenderItemText(HtmlTextWriter writer, string value, string text, string description)
		{
			this.Zone.LabelStyle.AddAttributesToRender(writer, this.Zone);
			writer.AddAttribute(HtmlTextWriterAttribute.For, this.Zone.GetCheckBoxID(value));
			writer.AddAttribute(HtmlTextWriterAttribute.Title, description, true);
			writer.RenderBeginTag(HtmlTextWriterTag.Label);
			writer.WriteEncodedText(text);
			writer.RenderEndTag();
		}

		// Token: 0x06005344 RID: 21316 RVA: 0x0015205C File Offset: 0x0015105C
		private void RenderItems(HtmlTextWriter writer, CatalogPart catalogPart)
		{
			WebPartDescriptionCollection availableWebPartDescriptions = catalogPart.GetAvailableWebPartDescriptions();
			if (availableWebPartDescriptions != null)
			{
				foreach (object obj in availableWebPartDescriptions)
				{
					WebPartDescription webPartDescription = (WebPartDescription)obj;
					this.RenderItem(writer, webPartDescription);
				}
			}
		}

		// Token: 0x06005345 RID: 21317 RVA: 0x001520BC File Offset: 0x001510BC
		protected virtual void RenderPartContents(HtmlTextWriter writer, CatalogPart catalogPart)
		{
			if (catalogPart == null)
			{
				throw new ArgumentNullException("catalogPart");
			}
			catalogPart.RenderControl(writer);
		}

		// Token: 0x06005346 RID: 21318 RVA: 0x001520D4 File Offset: 0x001510D4
		private void RenderTitle(HtmlTextWriter writer, CatalogPart catalogPart)
		{
			new Label
			{
				Text = catalogPart.DisplayTitle,
				ToolTip = catalogPart.Description,
				Page = this._page
			}.RenderControl(writer);
		}

		// Token: 0x04002E57 RID: 11863
		private CatalogZoneBase _zone;

		// Token: 0x04002E58 RID: 11864
		private Page _page;

		// Token: 0x04002E59 RID: 11865
		private Style _chromeStyleWithBorder;

		// Token: 0x04002E5A RID: 11866
		private Style _chromeStyleNoBorder;
	}
}
