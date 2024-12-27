using System;
using System.Drawing;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006BB RID: 1723
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class EditorPartChrome
	{
		// Token: 0x060054A9 RID: 21673 RVA: 0x001585CF File Offset: 0x001575CF
		public EditorPartChrome(EditorZoneBase zone)
		{
			if (zone == null)
			{
				throw new ArgumentNullException("zone");
			}
			this._zone = zone;
		}

		// Token: 0x170015AD RID: 5549
		// (get) Token: 0x060054AA RID: 21674 RVA: 0x001585EC File Offset: 0x001575EC
		protected EditorZoneBase Zone
		{
			get
			{
				return this._zone;
			}
		}

		// Token: 0x060054AB RID: 21675 RVA: 0x001585F4 File Offset: 0x001575F4
		protected virtual Style CreateEditorPartChromeStyle(EditorPart editorPart, PartChromeType chromeType)
		{
			if (editorPart == null)
			{
				throw new ArgumentNullException("editorPart");
			}
			if (chromeType < PartChromeType.Default || chromeType > PartChromeType.BorderOnly)
			{
				throw new ArgumentOutOfRangeException("chromeType");
			}
			if (chromeType == PartChromeType.BorderOnly || chromeType == PartChromeType.TitleAndBorder)
			{
				return this.Zone.PartChromeStyle;
			}
			if (this._chromeStyleNoBorder == null)
			{
				Style style = new Style();
				style.CopyFrom(this.Zone.PartChromeStyle);
				if (style.BorderStyle != BorderStyle.None)
				{
					style.BorderStyle = BorderStyle.None;
				}
				if (style.BorderWidth != Unit.Empty)
				{
					style.BorderWidth = Unit.Empty;
				}
				if (style.BorderColor != Color.Empty)
				{
					style.BorderColor = Color.Empty;
				}
				this._chromeStyleNoBorder = style;
			}
			return this._chromeStyleNoBorder;
		}

		// Token: 0x060054AC RID: 21676 RVA: 0x001586AC File Offset: 0x001576AC
		public virtual void PerformPreRender()
		{
		}

		// Token: 0x060054AD RID: 21677 RVA: 0x001586B0 File Offset: 0x001576B0
		public virtual void RenderEditorPart(HtmlTextWriter writer, EditorPart editorPart)
		{
			if (editorPart == null)
			{
				throw new ArgumentNullException("editorPart");
			}
			PartChromeType effectiveChromeType = this.Zone.GetEffectiveChromeType(editorPart);
			Style style = this.CreateEditorPartChromeStyle(editorPart, effectiveChromeType);
			if (!style.IsEmpty)
			{
				style.AddAttributesToRender(writer, this.Zone);
			}
			writer.RenderBeginTag(HtmlTextWriterTag.Fieldset);
			if (effectiveChromeType == PartChromeType.TitleAndBorder || effectiveChromeType == PartChromeType.TitleOnly)
			{
				this.RenderTitle(writer, editorPart);
			}
			if (editorPart.ChromeState != PartChromeState.Minimized)
			{
				Style partStyle = this.Zone.PartStyle;
				if (!partStyle.IsEmpty)
				{
					partStyle.AddAttributesToRender(writer, this.Zone);
				}
				writer.RenderBeginTag(HtmlTextWriterTag.Div);
				this.RenderPartContents(writer, editorPart);
				writer.RenderEndTag();
			}
			writer.RenderEndTag();
		}

		// Token: 0x060054AE RID: 21678 RVA: 0x00158754 File Offset: 0x00157754
		protected virtual void RenderPartContents(HtmlTextWriter writer, EditorPart editorPart)
		{
			string accessKey = editorPart.AccessKey;
			if (!string.IsNullOrEmpty(accessKey))
			{
				editorPart.AccessKey = string.Empty;
			}
			editorPart.RenderControl(writer);
			if (!string.IsNullOrEmpty(accessKey))
			{
				editorPart.AccessKey = accessKey;
			}
		}

		// Token: 0x060054AF RID: 21679 RVA: 0x00158794 File Offset: 0x00157794
		private void RenderTitle(HtmlTextWriter writer, EditorPart editorPart)
		{
			string displayTitle = editorPart.DisplayTitle;
			if (string.IsNullOrEmpty(displayTitle))
			{
				return;
			}
			TableItemStyle partTitleStyle = this.Zone.PartTitleStyle;
			if (this._titleTextStyle == null)
			{
				Style style = new Style();
				style.CopyFrom(partTitleStyle);
				this._titleTextStyle = style;
			}
			if (!this._titleTextStyle.IsEmpty)
			{
				this._titleTextStyle.AddAttributesToRender(writer, this.Zone);
			}
			string description = editorPart.Description;
			if (!string.IsNullOrEmpty(description))
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Title, description);
			}
			string accessKey = editorPart.AccessKey;
			if (!string.IsNullOrEmpty(accessKey))
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Accesskey, accessKey);
			}
			writer.RenderBeginTag(HtmlTextWriterTag.Legend);
			writer.Write(displayTitle);
			writer.RenderEndTag();
		}

		// Token: 0x04002EE5 RID: 12005
		private EditorZoneBase _zone;

		// Token: 0x04002EE6 RID: 12006
		private Style _chromeStyleNoBorder;

		// Token: 0x04002EE7 RID: 12007
		private Style _titleTextStyle;
	}
}
