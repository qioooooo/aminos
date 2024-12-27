using System;
using System.Globalization;
using System.IO;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004B9 RID: 1209
	internal class AdPostCacheSubstitution
	{
		// Token: 0x0600393E RID: 14654 RVA: 0x000F2AA8 File Offset: 0x000F1AA8
		private AdPostCacheSubstitution()
		{
		}

		// Token: 0x0600393F RID: 14655 RVA: 0x000F2AB0 File Offset: 0x000F1AB0
		internal AdPostCacheSubstitution(AdRotator adRotator)
		{
			this._adRotatorHelper = new AdRotator();
			this._adRotatorHelper.CopyFrom(adRotator);
			this._adRotatorHelper.IsPostCacheAdHelper = true;
			this._adRotatorHelper.Page = new Page();
		}

		// Token: 0x06003940 RID: 14656 RVA: 0x000F2AEC File Offset: 0x000F1AEC
		internal void RegisterPostCacheCallBack(HttpContext context, Page page, HtmlTextWriter writer)
		{
			HttpResponseSubstitutionCallback httpResponseSubstitutionCallback = new HttpResponseSubstitutionCallback(this.Render);
			context.Response.WriteSubstitution(httpResponseSubstitutionCallback);
		}

		// Token: 0x06003941 RID: 14657 RVA: 0x000F2B14 File Offset: 0x000F1B14
		internal string Render(HttpContext context)
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.CurrentCulture);
			HtmlTextWriter htmlTextWriter = this._adRotatorHelper.Page.CreateHtmlTextWriter(stringWriter);
			this._adRotatorHelper.RenderControl(htmlTextWriter);
			return stringWriter.ToString();
		}

		// Token: 0x0400261B RID: 9755
		private AdRotator _adRotatorHelper;
	}
}
