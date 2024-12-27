using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace System.Web
{
	// Token: 0x02000020 RID: 32
	internal class DynamicCompileErrorFormatter : ErrorFormatter
	{
		// Token: 0x060000BF RID: 191 RVA: 0x00004D19 File Offset: 0x00003D19
		internal DynamicCompileErrorFormatter(HttpCompileException excep)
		{
			this._excep = excep;
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00004D28 File Offset: 0x00003D28
		protected override Exception Exception
		{
			get
			{
				return this._excep;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00004D30 File Offset: 0x00003D30
		protected override bool ShowSourceFileInfo
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00004D33 File Offset: 0x00003D33
		protected override string ErrorTitle
		{
			get
			{
				return SR.GetString("TmplCompilerErrorTitle");
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00004D3F File Offset: 0x00003D3F
		protected override string Description
		{
			get
			{
				return SR.GetString("TmplCompilerErrorDesc");
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00004D4B File Offset: 0x00003D4B
		protected override string MiscSectionTitle
		{
			get
			{
				return SR.GetString("TmplCompilerErrorSecTitle");
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00004D58 File Offset: 0x00003D58
		protected override string MiscSectionContent
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(128);
				CompilerResults results = this._excep.Results;
				if (results.Errors.Count == 0 && results.NativeCompilerReturnValue != 0)
				{
					string @string = SR.GetString("TmplCompilerFatalError", new object[] { results.NativeCompilerReturnValue.ToString("G", CultureInfo.CurrentCulture) });
					this.AdaptiveMiscContent.Add(@string);
					stringBuilder.Append(@string);
					stringBuilder.Append("<br><br>\r\n");
				}
				if (results.Errors.HasErrors)
				{
					CompilerError firstCompileError = this._excep.FirstCompileError;
					if (firstCompileError != null)
					{
						string text = HttpUtility.HtmlEncode(firstCompileError.ErrorNumber);
						string text2 = text;
						stringBuilder.Append(text);
						if (HttpRuntime.HasAspNetHostingPermission(AspNetHostingPermissionLevel.Medium))
						{
							text = HttpUtility.HtmlEncode(firstCompileError.ErrorText);
							stringBuilder.Append(": ");
							stringBuilder.Append(text);
							text2 = text2 + ": " + text;
						}
						this.AdaptiveMiscContent.Add(text2);
						stringBuilder.Append("<br><br>\r\n");
						stringBuilder.Append("<b>");
						stringBuilder.Append(SR.GetString("TmplCompilerSourceSecTitle"));
						stringBuilder.Append(":</b><br><br>\r\n");
						stringBuilder.Append("            <table width=100% bgcolor=\"#ffffcc\">\r\n");
						stringBuilder.Append("               <tr><td>\r\n");
						stringBuilder.Append("               ");
						stringBuilder.Append("               </td></tr>\r\n");
						stringBuilder.Append("               <tr>\r\n");
						stringBuilder.Append("                  <td>\r\n");
						stringBuilder.Append("                      <code><pre>\r\n\r\n");
						stringBuilder.Append(FormatterWithFileInfo.GetSourceFileLines(firstCompileError.FileName, Encoding.Default, this._excep.SourceCode, firstCompileError.Line));
						stringBuilder.Append("</pre></code>\r\n\r\n");
						stringBuilder.Append("                  </td>\r\n");
						stringBuilder.Append("               </tr>\r\n");
						stringBuilder.Append("            </table>\r\n\r\n");
						stringBuilder.Append("            <br>\r\n\r\n");
						stringBuilder.Append("            <b>");
						stringBuilder.Append(SR.GetString("TmplCompilerSourceFileTitle"));
						stringBuilder.Append(":</b> ");
						this._sourceFilePath = ErrorFormatter.GetSafePath(firstCompileError.FileName);
						stringBuilder.Append(HttpUtility.HtmlEncode(this._sourceFilePath));
						stringBuilder.Append("\r\n");
						TypeConverter typeConverter = new Int32Converter();
						stringBuilder.Append("            &nbsp;&nbsp; <b>");
						stringBuilder.Append(SR.GetString("TmplCompilerSourceFileLine"));
						stringBuilder.Append(":</b>  ");
						this._sourceFileLineNumber = firstCompileError.Line;
						stringBuilder.Append(HttpUtility.HtmlEncode(typeConverter.ConvertToString(this._sourceFileLineNumber)));
						stringBuilder.Append("\r\n");
						stringBuilder.Append("            <br><br>\r\n");
					}
				}
				if (results.Errors.HasWarnings)
				{
					stringBuilder.Append("<br><div class=\"expandable\" onclick=\"OnToggleTOCLevel1('warningDiv')\">");
					stringBuilder.Append(SR.GetString("TmplCompilerWarningBanner"));
					stringBuilder.Append(":</div>\r\n");
					stringBuilder.Append("<div id=\"warningDiv\" style=\"display: none;\">\r\n");
					foreach (object obj in results.Errors)
					{
						CompilerError compilerError = (CompilerError)obj;
						if (compilerError.IsWarning)
						{
							stringBuilder.Append("<b>");
							stringBuilder.Append(SR.GetString("TmplCompilerWarningSecTitle"));
							stringBuilder.Append(":</b> ");
							stringBuilder.Append(HttpUtility.HtmlEncode(compilerError.ErrorNumber));
							if (HttpRuntime.HasAspNetHostingPermission(AspNetHostingPermissionLevel.Medium))
							{
								stringBuilder.Append(": ");
								stringBuilder.Append(HttpUtility.HtmlEncode(compilerError.ErrorText));
							}
							stringBuilder.Append("<br>\r\n");
							stringBuilder.Append("<b>");
							stringBuilder.Append(SR.GetString("TmplCompilerSourceSecTitle"));
							stringBuilder.Append(":</b><br><br>\r\n");
							stringBuilder.Append("            <table width=100% bgcolor=\"#ffffcc\">\r\n");
							stringBuilder.Append("               <tr><td>\r\n");
							stringBuilder.Append("               <b>");
							stringBuilder.Append(HttpUtility.HtmlEncode(HttpRuntime.GetSafePath(compilerError.FileName)));
							stringBuilder.Append("</b>\r\n");
							stringBuilder.Append("               </td></tr>\r\n");
							stringBuilder.Append("               <tr>\r\n");
							stringBuilder.Append("                  <td>\r\n");
							stringBuilder.Append("                      <code><pre>\r\n\r\n");
							stringBuilder.Append(FormatterWithFileInfo.GetSourceFileLines(compilerError.FileName, Encoding.Default, this._excep.SourceCode, compilerError.Line));
							stringBuilder.Append("</pre></code>\r\n\r\n");
							stringBuilder.Append("                  </td>\r\n");
							stringBuilder.Append("               </tr>\r\n");
							stringBuilder.Append("            </table>\r\n\r\n");
							stringBuilder.Append("            <br>\r\n\r\n");
						}
					}
					stringBuilder.Append("</div>\r\n");
				}
				if (!this._hideDetailedCompilerOutput)
				{
					if (results.Output.Count > 0 && HttpRuntime.HasAspNetHostingPermission(AspNetHostingPermissionLevel.Medium))
					{
						stringBuilder.Append(string.Format(CultureInfo.CurrentCulture, "<br><div class=\"expandable\" onclick=\"OnToggleTOCLevel1('{0}')\">{1}:</div>\r\n<div id=\"{0}\" style=\"display: none;\">\r\n            <br><table width=100% bgcolor=\"#ffffcc\">\r\n               <tr>\r\n                  <td>\r\n                      <code><pre>\r\n\r\n", new object[]
						{
							"compilerOutputDiv",
							SR.GetString("TmplCompilerCompleteOutput")
						}));
						foreach (string text3 in results.Output)
						{
							stringBuilder.Append(HttpUtility.HtmlEncode(text3));
							stringBuilder.Append("\r\n");
						}
						stringBuilder.Append("</pre></code>\r\n\r\n                  </td>\r\n               </tr>\r\n            </table>\r\n\r\n            \r\n\r\n</div>\r\n");
					}
					if (this._excep.SourceCode != null && HttpRuntime.HasAspNetHostingPermission(AspNetHostingPermissionLevel.Medium))
					{
						stringBuilder.Append(string.Format(CultureInfo.CurrentCulture, "<br><div class=\"expandable\" onclick=\"OnToggleTOCLevel1('{0}')\">{1}:</div>\r\n<div id=\"{0}\" style=\"display: none;\">\r\n            <br><table width=100% bgcolor=\"#ffffcc\">\r\n               <tr>\r\n                  <td>\r\n                      <code><pre>\r\n\r\n", new object[]
						{
							"dynamicCodeDiv",
							SR.GetString("TmplCompilerGeneratedFile")
						}));
						string[] array = this._excep.SourceCode.Split(new char[] { '\n' });
						int num = 1;
						foreach (string text4 in array)
						{
							string text5 = num.ToString("G", CultureInfo.CurrentCulture);
							stringBuilder.Append(SR.GetString("TmplCompilerLineHeader", new object[] { text5 }));
							if (text5.Length < 5)
							{
								stringBuilder.Append(' ', 5 - text5.Length);
							}
							num++;
							stringBuilder.Append(HttpUtility.HtmlEncode(text4));
						}
						stringBuilder.Append("</pre></code>\r\n\r\n                  </td>\r\n               </tr>\r\n            </table>\r\n\r\n            \r\n\r\n</div>\r\n");
					}
					stringBuilder.Append("\r\n    <script type=\"text/javascript\">\r\n    function OnToggleTOCLevel1(level2ID)\r\n    {\r\n      var elemLevel2 = document.getElementById(level2ID);\r\n      if (elemLevel2.style.display == 'none')\r\n      {\r\n        elemLevel2.style.display = '';\r\n      }\r\n      else {\r\n        elemLevel2.style.display = 'none';\r\n      }\r\n    }\r\n    </script>\r\n                          ");
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x0000544C File Offset: 0x0000444C
		protected override string PhysicalPath
		{
			get
			{
				return this._sourceFilePath;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00005454 File Offset: 0x00004454
		protected override int SourceFileLineNumber
		{
			get
			{
				return this._sourceFileLineNumber;
			}
		}

		// Token: 0x04000D2E RID: 3374
		private const string startExpandableBlock = "<br><div class=\"expandable\" onclick=\"OnToggleTOCLevel1('{0}')\">{1}:</div>\r\n<div id=\"{0}\" style=\"display: none;\">\r\n            <br><table width=100% bgcolor=\"#ffffcc\">\r\n               <tr>\r\n                  <td>\r\n                      <code><pre>\r\n\r\n";

		// Token: 0x04000D2F RID: 3375
		private const string endExpandableBlock = "</pre></code>\r\n\r\n                  </td>\r\n               </tr>\r\n            </table>\r\n\r\n            \r\n\r\n</div>\r\n";

		// Token: 0x04000D30 RID: 3376
		private const int errorRange = 2;

		// Token: 0x04000D31 RID: 3377
		private HttpCompileException _excep;

		// Token: 0x04000D32 RID: 3378
		private string _sourceFilePath;

		// Token: 0x04000D33 RID: 3379
		private int _sourceFileLineNumber;

		// Token: 0x04000D34 RID: 3380
		protected bool _hideDetailedCompilerOutput;
	}
}
