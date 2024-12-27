using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;
using System.Web.UI.HtmlControls;
using System.Web.Util;
using System.Xml;

namespace System.Web.UI
{
	// Token: 0x02000449 RID: 1097
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class PageTheme
	{
		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x0600343F RID: 13375
		protected abstract string[] LinkedStyleSheets { get; }

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x06003440 RID: 13376
		protected abstract IDictionary ControlSkins { get; }

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x06003441 RID: 13377
		protected abstract string AppRelativeTemplateSourceDirectory { get; }

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x06003442 RID: 13378 RVA: 0x000E3051 File Offset: 0x000E2051
		protected Page Page
		{
			get
			{
				return this._page;
			}
		}

		// Token: 0x06003443 RID: 13379 RVA: 0x000E3059 File Offset: 0x000E2059
		internal void Initialize(Page page, bool styleSheetTheme)
		{
			this._page = page;
			this._styleSheetTheme = styleSheetTheme;
		}

		// Token: 0x06003444 RID: 13380 RVA: 0x000E3069 File Offset: 0x000E2069
		protected object Eval(string expression)
		{
			return this.Page.Eval(expression);
		}

		// Token: 0x06003445 RID: 13381 RVA: 0x000E3077 File Offset: 0x000E2077
		protected string Eval(string expression, string format)
		{
			return this.Page.Eval(expression, format);
		}

		// Token: 0x06003446 RID: 13382 RVA: 0x000E3086 File Offset: 0x000E2086
		public static object CreateSkinKey(Type controlType, string skinID)
		{
			if (controlType == null)
			{
				throw new ArgumentNullException("controlType");
			}
			return new PageTheme.SkinKey(controlType.ToString(), skinID);
		}

		// Token: 0x06003447 RID: 13383 RVA: 0x000E30A4 File Offset: 0x000E20A4
		internal void ApplyControlSkin(Control control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			string skinID = control.SkinID;
			ControlSkin controlSkin = (ControlSkin)this.ControlSkins[PageTheme.CreateSkinKey(control.GetType(), skinID)];
			if (controlSkin != null)
			{
				controlSkin.ApplySkin(control);
			}
		}

		// Token: 0x06003448 RID: 13384 RVA: 0x000E30F0 File Offset: 0x000E20F0
		internal void SetStyleSheet()
		{
			if (this.LinkedStyleSheets != null && this.LinkedStyleSheets.Length > 0)
			{
				if (this.Page.Header == null)
				{
					throw new InvalidOperationException(SR.GetString("Page_theme_requires_page_header"));
				}
				int num = 0;
				foreach (string text in this.LinkedStyleSheets)
				{
					HtmlLink htmlLink = new HtmlLink();
					htmlLink.Href = text;
					htmlLink.Attributes["type"] = "text/css";
					htmlLink.Attributes["rel"] = "stylesheet";
					if (this._styleSheetTheme)
					{
						this.Page.Header.Controls.AddAt(num++, htmlLink);
					}
					else
					{
						this.Page.Header.Controls.Add(htmlLink);
					}
				}
			}
		}

		// Token: 0x06003449 RID: 13385 RVA: 0x000E31C8 File Offset: 0x000E21C8
		public bool TestDeviceFilter(string deviceFilterName)
		{
			return this.Page.TestDeviceFilter(deviceFilterName);
		}

		// Token: 0x0600344A RID: 13386 RVA: 0x000E31D6 File Offset: 0x000E21D6
		protected object XPath(string xPathExpression)
		{
			return this.Page.XPath(xPathExpression);
		}

		// Token: 0x0600344B RID: 13387 RVA: 0x000E31E4 File Offset: 0x000E21E4
		protected object XPath(string xPathExpression, IXmlNamespaceResolver resolver)
		{
			return this.Page.XPath(xPathExpression, resolver);
		}

		// Token: 0x0600344C RID: 13388 RVA: 0x000E31F3 File Offset: 0x000E21F3
		protected string XPath(string xPathExpression, string format)
		{
			return this.Page.XPath(xPathExpression, format);
		}

		// Token: 0x0600344D RID: 13389 RVA: 0x000E3202 File Offset: 0x000E2202
		protected string XPath(string xPathExpression, string format, IXmlNamespaceResolver resolver)
		{
			return this.Page.XPath(xPathExpression, format, resolver);
		}

		// Token: 0x0600344E RID: 13390 RVA: 0x000E3212 File Offset: 0x000E2212
		protected IEnumerable XPathSelect(string xPathExpression)
		{
			return this.Page.XPathSelect(xPathExpression);
		}

		// Token: 0x0600344F RID: 13391 RVA: 0x000E3220 File Offset: 0x000E2220
		protected IEnumerable XPathSelect(string xPathExpression, IXmlNamespaceResolver resolver)
		{
			return this.Page.XPathSelect(xPathExpression, resolver);
		}

		// Token: 0x040024AC RID: 9388
		private Page _page;

		// Token: 0x040024AD RID: 9389
		private bool _styleSheetTheme;

		// Token: 0x0200044A RID: 1098
		private class SkinKey
		{
			// Token: 0x06003451 RID: 13393 RVA: 0x000E3237 File Offset: 0x000E2237
			internal SkinKey(string typeName, string skinID)
			{
				this._typeName = typeName;
				if (string.IsNullOrEmpty(skinID))
				{
					this._skinID = null;
					return;
				}
				this._skinID = skinID.ToLower(CultureInfo.InvariantCulture);
			}

			// Token: 0x06003452 RID: 13394 RVA: 0x000E3267 File Offset: 0x000E2267
			public override int GetHashCode()
			{
				if (this._skinID == null)
				{
					return this._typeName.GetHashCode();
				}
				return HashCodeCombiner.CombineHashCodes(this._typeName.GetHashCode(), this._skinID.GetHashCode());
			}

			// Token: 0x06003453 RID: 13395 RVA: 0x000E3298 File Offset: 0x000E2298
			public override bool Equals(object o)
			{
				PageTheme.SkinKey skinKey = (PageTheme.SkinKey)o;
				return this._typeName == skinKey._typeName && this._skinID == skinKey._skinID;
			}

			// Token: 0x040024AE RID: 9390
			private string _skinID;

			// Token: 0x040024AF RID: 9391
			private string _typeName;
		}
	}
}
