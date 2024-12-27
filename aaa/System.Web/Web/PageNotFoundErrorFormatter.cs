using System;
using System.Collections.Specialized;

namespace System.Web
{
	// Token: 0x0200001C RID: 28
	internal class PageNotFoundErrorFormatter : ErrorFormatter
	{
		// Token: 0x06000098 RID: 152 RVA: 0x000047CC File Offset: 0x000037CC
		internal PageNotFoundErrorFormatter(string url)
		{
			this._htmlEncodedUrl = HttpUtility.HtmlEncode(url);
			this._adaptiveMiscContent.Add(this._htmlEncodedUrl);
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000099 RID: 153 RVA: 0x000047FD File Offset: 0x000037FD
		protected override string ErrorTitle
		{
			get
			{
				return SR.GetString("NotFound_Resource_Not_Found");
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00004809 File Offset: 0x00003809
		protected override string Description
		{
			get
			{
				return HttpUtility.FormatPlainTextAsHtml(SR.GetString("NotFound_Http_404"));
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600009B RID: 155 RVA: 0x0000481A File Offset: 0x0000381A
		protected override string MiscSectionTitle
		{
			get
			{
				return SR.GetString("NotFound_Requested_Url");
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00004826 File Offset: 0x00003826
		protected override string MiscSectionContent
		{
			get
			{
				return this._htmlEncodedUrl;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600009D RID: 157 RVA: 0x0000482E File Offset: 0x0000382E
		protected override StringCollection AdaptiveMiscContent
		{
			get
			{
				return this._adaptiveMiscContent;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00004836 File Offset: 0x00003836
		protected override bool ShowSourceFileInfo
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00004839 File Offset: 0x00003839
		internal override bool CanBeShownToAllUsers
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04000D23 RID: 3363
		protected string _htmlEncodedUrl;

		// Token: 0x04000D24 RID: 3364
		private StringCollection _adaptiveMiscContent = new StringCollection();
	}
}
