using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace System.Web
{
	// Token: 0x0200001D RID: 29
	internal class PageForbiddenErrorFormatter : ErrorFormatter
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x0000483C File Offset: 0x0000383C
		internal PageForbiddenErrorFormatter(string url)
			: this(url, null)
		{
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004846 File Offset: 0x00003846
		internal PageForbiddenErrorFormatter(string url, string description)
		{
			this._htmlEncodedUrl = HttpUtility.HtmlEncode(url);
			this._adaptiveMiscContent.Add(this._htmlEncodedUrl);
			this._description = description;
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x0000487E File Offset: 0x0000387E
		protected override string ErrorTitle
		{
			get
			{
				return SR.GetString("Forbidden_Type_Not_Served");
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x0000488C File Offset: 0x0000388C
		protected override string Description
		{
			get
			{
				if (this._description != null)
				{
					return this._description;
				}
				Match match = Regex.Match(this._htmlEncodedUrl, "\\.\\w+$");
				string text = string.Empty;
				if (match.Success)
				{
					text = SR.GetString("Forbidden_Extension_Incorrect", new object[] { match.ToString() });
				}
				return HttpUtility.FormatPlainTextAsHtml(SR.GetString("Forbidden_Extension_Desc", new object[] { text }));
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x000048FE File Offset: 0x000038FE
		protected override string MiscSectionTitle
		{
			get
			{
				return SR.GetString("NotFound_Requested_Url");
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x0000490A File Offset: 0x0000390A
		protected override string MiscSectionContent
		{
			get
			{
				return this._htmlEncodedUrl;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00004912 File Offset: 0x00003912
		protected override StringCollection AdaptiveMiscContent
		{
			get
			{
				return this._adaptiveMiscContent;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x0000491A File Offset: 0x0000391A
		protected override bool ShowSourceFileInfo
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x0000491D File Offset: 0x0000391D
		internal override bool CanBeShownToAllUsers
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04000D25 RID: 3365
		protected string _htmlEncodedUrl;

		// Token: 0x04000D26 RID: 3366
		private StringCollection _adaptiveMiscContent = new StringCollection();

		// Token: 0x04000D27 RID: 3367
		private string _description;
	}
}
