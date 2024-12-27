using System;
using System.Collections.Specialized;

namespace System.Web
{
	// Token: 0x02000021 RID: 33
	internal class ParseErrorFormatter : FormatterWithFileInfo
	{
		// Token: 0x060000C8 RID: 200 RVA: 0x0000545C File Offset: 0x0000445C
		internal ParseErrorFormatter(HttpParseException e, string virtualPath, string sourceCode, int line, string message)
			: base(virtualPath, null, sourceCode, line)
		{
			this._excep = e;
			this._message = HttpUtility.FormatPlainTextAsHtml(message);
			this._adaptiveMiscContent.Add(this._message);
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x0000549A File Offset: 0x0000449A
		protected override Exception Exception
		{
			get
			{
				return this._excep;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000CA RID: 202 RVA: 0x000054A2 File Offset: 0x000044A2
		protected override string ErrorTitle
		{
			get
			{
				return SR.GetString("Parser_Error");
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000CB RID: 203 RVA: 0x000054AE File Offset: 0x000044AE
		protected override string Description
		{
			get
			{
				return SR.GetString("Parser_Desc");
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000CC RID: 204 RVA: 0x000054BA File Offset: 0x000044BA
		protected override string MiscSectionTitle
		{
			get
			{
				return SR.GetString("Parser_Error_Message");
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000CD RID: 205 RVA: 0x000054C6 File Offset: 0x000044C6
		protected override string MiscSectionContent
		{
			get
			{
				return this._message;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000CE RID: 206 RVA: 0x000054CE File Offset: 0x000044CE
		protected override string ColoredSquareTitle
		{
			get
			{
				return SR.GetString("Parser_Source_Error");
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060000CF RID: 207 RVA: 0x000054DA File Offset: 0x000044DA
		protected override StringCollection AdaptiveMiscContent
		{
			get
			{
				return this._adaptiveMiscContent;
			}
		}

		// Token: 0x04000D35 RID: 3381
		protected string _message;

		// Token: 0x04000D36 RID: 3382
		private HttpParseException _excep;

		// Token: 0x04000D37 RID: 3383
		private StringCollection _adaptiveMiscContent = new StringCollection();
	}
}
