using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x0200001F RID: 31
	internal abstract class FormatterWithFileInfo : ErrorFormatter
	{
		// Token: 0x060000B6 RID: 182 RVA: 0x00004A88 File Offset: 0x00003A88
		internal static string GetSourceFileLines(string fileName, Encoding encoding, string sourceCode, int lineNumber)
		{
			if (fileName != null && !HttpRuntime.HasFilePermission(fileName))
			{
				return SR.GetString("WithFile_No_Relevant_Line");
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (lineNumber <= 0)
			{
				return SR.GetString("WithFile_No_Relevant_Line");
			}
			TextReader textReader = null;
			string virtualPathFromHttpLinePragma = ErrorFormatter.GetVirtualPathFromHttpLinePragma(fileName);
			if (virtualPathFromHttpLinePragma != null)
			{
				Stream stream = VirtualPathProvider.OpenFile(virtualPathFromHttpLinePragma);
				if (stream != null)
				{
					textReader = Util.ReaderFromStream(stream, global::System.Web.VirtualPath.Create(virtualPathFromHttpLinePragma));
				}
			}
			try
			{
				if (textReader == null && fileName != null)
				{
					textReader = new StreamReader(fileName, encoding, true, 4096);
				}
			}
			catch
			{
			}
			if (textReader == null)
			{
				if (sourceCode == null)
				{
					return SR.GetString("WithFile_No_Relevant_Line");
				}
				textReader = new StringReader(sourceCode);
			}
			try
			{
				bool flag = false;
				if (ErrorFormatter.IsTextRightToLeft)
				{
					stringBuilder.Append("<div dir=\"ltr\">");
				}
				int num = 1;
				for (;;)
				{
					string text = textReader.ReadLine();
					if (text == null)
					{
						break;
					}
					if (num == lineNumber)
					{
						stringBuilder.Append("<font color=red>");
					}
					if (num >= lineNumber - 2 && num <= lineNumber + 2)
					{
						flag = true;
						string text2 = num.ToString("G", CultureInfo.CurrentCulture);
						stringBuilder.Append(SR.GetString("WithFile_Line_Num", new object[] { text2 }));
						if (text2.Length < 3)
						{
							stringBuilder.Append(' ', 3 - text2.Length);
						}
						stringBuilder.Append(HttpUtility.HtmlEncode(text));
						if (num != lineNumber + 2)
						{
							stringBuilder.Append("\r\n");
						}
					}
					if (num == lineNumber)
					{
						stringBuilder.Append("</font>");
					}
					if (num > lineNumber + 2)
					{
						break;
					}
					num++;
				}
				if (ErrorFormatter.IsTextRightToLeft)
				{
					stringBuilder.Append("</div>");
				}
				if (!flag)
				{
					return SR.GetString("WithFile_No_Relevant_Line");
				}
			}
			finally
			{
				textReader.Close();
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004C60 File Offset: 0x00003C60
		private string GetSourceFileLines()
		{
			return FormatterWithFileInfo.GetSourceFileLines(this._physicalPath, this.SourceFileEncoding, this._sourceCode, this._line);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004C80 File Offset: 0x00003C80
		internal FormatterWithFileInfo(string virtualPath, string physicalPath, string sourceCode, int line)
		{
			this._virtualPath = virtualPath;
			this._physicalPath = physicalPath;
			if (sourceCode == null && this._physicalPath == null && this._virtualPath != null)
			{
				if (UrlPath.IsValidVirtualPathWithoutProtocol(this._virtualPath))
				{
					this._physicalPath = HostingEnvironment.MapPath(this._virtualPath);
				}
				else
				{
					this._physicalPath = this._virtualPath;
				}
			}
			this._sourceCode = sourceCode;
			this._line = line;
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00004CEF File Offset: 0x00003CEF
		protected virtual Encoding SourceFileEncoding
		{
			get
			{
				return Encoding.Default;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00004CF6 File Offset: 0x00003CF6
		protected override string ColoredSquareContent
		{
			get
			{
				return this.GetSourceFileLines();
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00004CFE File Offset: 0x00003CFE
		protected override bool ShowSourceFileInfo
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00004D01 File Offset: 0x00003D01
		protected override string PhysicalPath
		{
			get
			{
				return this._physicalPath;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00004D09 File Offset: 0x00003D09
		protected override string VirtualPath
		{
			get
			{
				return this._virtualPath;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00004D11 File Offset: 0x00003D11
		protected override int SourceFileLineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x04000D29 RID: 3369
		private const int errorRange = 2;

		// Token: 0x04000D2A RID: 3370
		protected string _virtualPath;

		// Token: 0x04000D2B RID: 3371
		protected string _physicalPath;

		// Token: 0x04000D2C RID: 3372
		protected string _sourceCode;

		// Token: 0x04000D2D RID: 3373
		protected int _line;
	}
}
