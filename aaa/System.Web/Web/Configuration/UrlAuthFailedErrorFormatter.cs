using System;
using System.Collections.Specialized;

namespace System.Web.Configuration
{
	// Token: 0x0200025D RID: 605
	internal class UrlAuthFailedErrorFormatter : ErrorFormatter
	{
		// Token: 0x06001FF0 RID: 8176 RVA: 0x0008C3F4 File Offset: 0x0008B3F4
		internal UrlAuthFailedErrorFormatter()
		{
		}

		// Token: 0x06001FF1 RID: 8177 RVA: 0x0008C407 File Offset: 0x0008B407
		internal static string GetErrorText()
		{
			return UrlAuthFailedErrorFormatter.GetErrorText(HttpContext.Current);
		}

		// Token: 0x06001FF2 RID: 8178 RVA: 0x0008C414 File Offset: 0x0008B414
		internal static string GetErrorText(HttpContext context)
		{
			bool isCustomErrorEnabled = context.IsCustomErrorEnabled;
			return new UrlAuthFailedErrorFormatter().GetErrorMessage(context, isCustomErrorEnabled);
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06001FF3 RID: 8179 RVA: 0x0008C434 File Offset: 0x0008B434
		protected override string ErrorTitle
		{
			get
			{
				return SR.GetString("Assess_Denied_Title");
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06001FF4 RID: 8180 RVA: 0x0008C440 File Offset: 0x0008B440
		protected override string Description
		{
			get
			{
				return SR.GetString("Assess_Denied_Description2");
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06001FF5 RID: 8181 RVA: 0x0008C44C File Offset: 0x0008B44C
		protected override string MiscSectionTitle
		{
			get
			{
				return SR.GetString("Assess_Denied_Section_Title2");
			}
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06001FF6 RID: 8182 RVA: 0x0008C458 File Offset: 0x0008B458
		protected override string MiscSectionContent
		{
			get
			{
				string text = HttpUtility.FormatPlainTextAsHtml(SR.GetString("Assess_Denied_Misc_Content2"));
				this.AdaptiveMiscContent.Add(text);
				return text;
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06001FF7 RID: 8183 RVA: 0x0008C483 File Offset: 0x0008B483
		protected override string ColoredSquareTitle
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06001FF8 RID: 8184 RVA: 0x0008C486 File Offset: 0x0008B486
		protected override string ColoredSquareContent
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06001FF9 RID: 8185 RVA: 0x0008C489 File Offset: 0x0008B489
		protected override StringCollection AdaptiveMiscContent
		{
			get
			{
				return this._adaptiveMiscContent;
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06001FFA RID: 8186 RVA: 0x0008C491 File Offset: 0x0008B491
		protected override bool ShowSourceFileInfo
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04001A78 RID: 6776
		private StringCollection _adaptiveMiscContent = new StringCollection();
	}
}
