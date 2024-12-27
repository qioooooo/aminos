using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Security;
using System.Text;
using System.Web.Hosting;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x02000019 RID: 25
	internal abstract class ErrorFormatter
	{
		// Token: 0x0600005B RID: 91 RVA: 0x00003138 File Offset: 0x00002138
		internal static bool RequiresAdaptiveErrorReporting(HttpContext context)
		{
			if (HttpRuntime.HostingInitFailed)
			{
				return false;
			}
			HttpRequest httpRequest = ((context != null) ? context.Request : null);
			if (context != null && context.WorkerRequest is StateHttpWorkerRequest)
			{
				return false;
			}
			HttpBrowserCapabilities httpBrowserCapabilities = null;
			try
			{
				httpBrowserCapabilities = ((httpRequest != null) ? httpRequest.Browser : null);
			}
			catch
			{
				return false;
			}
			return httpBrowserCapabilities != null && httpBrowserCapabilities["requiresAdaptiveErrorReporting"] == "true";
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000031B4 File Offset: 0x000021B4
		private Literal CreateBreakLiteral()
		{
			return new Literal
			{
				Text = "<br/>"
			};
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000031D4 File Offset: 0x000021D4
		private Label CreateLabelFromText(string text)
		{
			return new Label
			{
				Text = text
			};
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000031F0 File Offset: 0x000021F0
		internal virtual string GetAdaptiveErrorMessage(HttpContext context, bool dontShowSensitiveInfo)
		{
			this.GetHtmlErrorMessage(dontShowSensitiveInfo);
			context.Response.UseAdaptiveError = true;
			string text4;
			try
			{
				Page page = new ErrorFormatterPage();
				page.EnableViewState = false;
				HtmlForm htmlForm = new HtmlForm();
				page.Controls.Add(htmlForm);
				IParserAccessor parserAccessor = htmlForm;
				Label label = this.CreateLabelFromText(SR.GetString("Error_Formatter_ASPNET_Error", new object[] { HttpRuntime.AppDomainAppVirtualPath }));
				label.ForeColor = Color.Red;
				label.Font.Bold = true;
				label.Font.Size = FontUnit.Large;
				parserAccessor.AddParsedSubObject(label);
				parserAccessor.AddParsedSubObject(this.CreateBreakLiteral());
				label = this.CreateLabelFromText(this.ErrorTitle);
				label.ForeColor = Color.Maroon;
				label.Font.Bold = true;
				label.Font.Italic = true;
				parserAccessor.AddParsedSubObject(label);
				parserAccessor.AddParsedSubObject(this.CreateBreakLiteral());
				parserAccessor.AddParsedSubObject(this.CreateLabelFromText(SR.GetString("Error_Formatter_Description") + " " + this.Description));
				parserAccessor.AddParsedSubObject(this.CreateBreakLiteral());
				string miscSectionTitle = this.MiscSectionTitle;
				if (!string.IsNullOrEmpty(miscSectionTitle))
				{
					parserAccessor.AddParsedSubObject(this.CreateLabelFromText(miscSectionTitle));
					parserAccessor.AddParsedSubObject(this.CreateBreakLiteral());
				}
				StringCollection adaptiveMiscContent = this.AdaptiveMiscContent;
				if (adaptiveMiscContent != null && adaptiveMiscContent.Count > 0)
				{
					foreach (string text in adaptiveMiscContent)
					{
						parserAccessor.AddParsedSubObject(this.CreateLabelFromText(text));
						parserAccessor.AddParsedSubObject(this.CreateBreakLiteral());
					}
				}
				string displayPath = this.GetDisplayPath();
				if (!string.IsNullOrEmpty(displayPath))
				{
					string text2 = SR.GetString("Error_Formatter_Source_File") + " " + displayPath;
					parserAccessor.AddParsedSubObject(this.CreateLabelFromText(text2));
					parserAccessor.AddParsedSubObject(this.CreateBreakLiteral());
					text2 = SR.GetString("Error_Formatter_Line") + " " + this.SourceFileLineNumber;
					parserAccessor.AddParsedSubObject(this.CreateLabelFromText(text2));
					parserAccessor.AddParsedSubObject(this.CreateBreakLiteral());
				}
				StringCollection adaptiveStackTrace = this.AdaptiveStackTrace;
				if (adaptiveStackTrace != null && adaptiveStackTrace.Count > 0)
				{
					foreach (string text3 in adaptiveStackTrace)
					{
						parserAccessor.AddParsedSubObject(this.CreateLabelFromText(text3));
						parserAccessor.AddParsedSubObject(this.CreateBreakLiteral());
					}
				}
				StringWriter stringWriter = new StringWriter(CultureInfo.CurrentCulture);
				TextWriter textWriter = context.Response.SwitchWriter(stringWriter);
				page.ProcessRequest(context);
				context.Response.SwitchWriter(textWriter);
				text4 = stringWriter.ToString();
			}
			catch
			{
				text4 = this.GetStaticErrorMessage(context);
			}
			return text4;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000350C File Offset: 0x0000250C
		private string GetPreferredRenderingType(HttpContext context)
		{
			HttpRequest httpRequest = ((context != null) ? context.Request : null);
			HttpBrowserCapabilities httpBrowserCapabilities = null;
			try
			{
				httpBrowserCapabilities = ((httpRequest != null) ? httpRequest.Browser : null);
			}
			catch
			{
				return string.Empty;
			}
			if (httpBrowserCapabilities == null)
			{
				return string.Empty;
			}
			return httpBrowserCapabilities["preferredRenderingType"];
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003568 File Offset: 0x00002568
		private string GetStaticErrorMessage(HttpContext context)
		{
			string preferredRenderingType = this.GetPreferredRenderingType(context);
			string text;
			if (StringUtil.StringStartsWithIgnoreCase(preferredRenderingType, "xhtml"))
			{
				text = this.FormatStaticErrorMessage("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!DOCTYPE html PUBLIC \"-//WAPFORUM//DTD XHTML Mobile 1.0//EN\" \"http://www.wapforum.org/DTD/xhtml-mobile10.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n<title></title>\r\n</head>\r\n<body>\r\n<form>\r\n<div>\r\n<span style=\"color:Red;font-size:Large;font-weight:bold;\">{0}</span><br/>\r\n<span style=\"color:Maroon;font-weight:bold;font-style:italic;\">{1}</span><br/>\r\n", "</div>\r\n</form>\r\n</body>\r\n</html>");
			}
			else if (StringUtil.StringStartsWithIgnoreCase(preferredRenderingType, "wml"))
			{
				text = this.FormatStaticErrorMessage("<?xml version='1.0'?>\r\n<!DOCTYPE wml PUBLIC '-//WAPFORUM//DTD WML 1.1//EN' 'http://www.wapforum.org/DTD/wml_1.1.xml'><wml><head>\r\n<meta http-equiv=\"Cache-Control\" content=\"max-age=0\" forua=\"true\"/>\r\n</head>\r\n<card>\r\n<p>\r\n<b><big>{0}</big></b><br/>\r\n<b><i>{1}</i></b><br/>\r\n", "</p>\r\n</card>\r\n</wml>\r\n");
				if (string.Compare(context.Response.ContentType, 0, "text/vnd.wap.wml", 0, "text/vnd.wap.wml".Length, StringComparison.OrdinalIgnoreCase) != 0)
				{
					context.Response.ContentType = "text/vnd.wap.wml";
				}
			}
			else
			{
				text = this.FormatStaticErrorMessage("<html>\r\n<body>\r\n<form>\r\n<font color=\"Red\" size=\"5\">{0}</font><br/>\r\n<font color=\"Maroon\">{1}</font><br/>\r\n", "</form>\r\n</body>\r\n</html>");
			}
			return text;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003604 File Offset: 0x00002604
		private string FormatStaticErrorMessage(string errorBeginTemplate, string errorEndTemplate)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string @string = SR.GetString("Error_Formatter_ASPNET_Error", new object[] { HttpRuntime.AppDomainAppVirtualPath });
			stringBuilder.Append(string.Format(CultureInfo.CurrentCulture, errorBeginTemplate, new object[] { @string, this.ErrorTitle }));
			stringBuilder.Append(SR.GetString("Error_Formatter_Description") + " " + this.Description);
			stringBuilder.Append("<br/>\r\n");
			string miscSectionTitle = this.MiscSectionTitle;
			if (miscSectionTitle != null && miscSectionTitle.Length > 0)
			{
				stringBuilder.Append(miscSectionTitle);
				stringBuilder.Append("<br/>\r\n");
			}
			StringCollection adaptiveMiscContent = this.AdaptiveMiscContent;
			if (adaptiveMiscContent != null && adaptiveMiscContent.Count > 0)
			{
				foreach (string text in adaptiveMiscContent)
				{
					stringBuilder.Append(text);
					stringBuilder.Append("<br/>\r\n");
				}
			}
			string displayPath = this.GetDisplayPath();
			if (!string.IsNullOrEmpty(displayPath))
			{
				string text2 = SR.GetString("Error_Formatter_Source_File") + " " + displayPath;
				stringBuilder.Append(text2);
				stringBuilder.Append("<br/>\r\n");
				text2 = SR.GetString("Error_Formatter_Line") + " " + this.SourceFileLineNumber;
				stringBuilder.Append(text2);
				stringBuilder.Append("<br/>\r\n");
			}
			StringCollection adaptiveStackTrace = this.AdaptiveStackTrace;
			if (adaptiveStackTrace != null && adaptiveStackTrace.Count > 0)
			{
				foreach (string text3 in adaptiveStackTrace)
				{
					stringBuilder.Append(text3);
					stringBuilder.Append("<br/>\r\n");
				}
			}
			stringBuilder.Append(errorEndTemplate);
			return stringBuilder.ToString();
		}

		// Token: 0x06000062 RID: 98 RVA: 0x0000380C File Offset: 0x0000280C
		internal string GetErrorMessage()
		{
			return this.GetErrorMessage(HttpContext.Current, true);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x0000381A File Offset: 0x0000281A
		internal virtual string GetErrorMessage(HttpContext context, bool dontShowSensitiveInfo)
		{
			if (ErrorFormatter.RequiresAdaptiveErrorReporting(context))
			{
				return this.GetAdaptiveErrorMessage(context, dontShowSensitiveInfo);
			}
			return this.GetHtmlErrorMessage(dontShowSensitiveInfo);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003834 File Offset: 0x00002834
		internal string GetHtmlErrorMessage()
		{
			return this.GetHtmlErrorMessage(true);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003840 File Offset: 0x00002840
		internal string GetHtmlErrorMessage(bool dontShowSensitiveInfo)
		{
			this.PrepareFormatter();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<html");
			if (ErrorFormatter.IsTextRightToLeft)
			{
				stringBuilder.Append(" dir=\"rtl\"");
			}
			stringBuilder.Append(">\r\n");
			stringBuilder.Append("    <head>\r\n");
			stringBuilder.Append("        <title>" + this.ErrorTitle + "</title>\r\n");
			stringBuilder.Append("        <style>\r\n");
			stringBuilder.Append("         body {font-family:\"Verdana\";font-weight:normal;font-size: .7em;color:black;} \r\n");
			stringBuilder.Append("         p {font-family:\"Verdana\";font-weight:normal;color:black;margin-top: -5px}\r\n");
			stringBuilder.Append("         b {font-family:\"Verdana\";font-weight:bold;color:black;margin-top: -5px}\r\n");
			stringBuilder.Append("         H1 { font-family:\"Verdana\";font-weight:normal;font-size:18pt;color:red }\r\n");
			stringBuilder.Append("         H2 { font-family:\"Verdana\";font-weight:normal;font-size:14pt;color:maroon }\r\n");
			stringBuilder.Append("         pre {font-family:\"Lucida Console\";font-size: .9em}\r\n");
			stringBuilder.Append("         .marker {font-weight: bold; color: black;text-decoration: none;}\r\n");
			stringBuilder.Append("         .version {color: gray;}\r\n");
			stringBuilder.Append("         .error {margin-bottom: 10px;}\r\n");
			stringBuilder.Append("         .expandable { text-decoration:underline; font-weight:bold; color:navy; cursor:hand; }\r\n");
			stringBuilder.Append("        </style>\r\n");
			stringBuilder.Append("    </head>\r\n\r\n");
			stringBuilder.Append("    <body bgcolor=\"white\">\r\n\r\n");
			stringBuilder.Append("            <span><H1>" + SR.GetString("Error_Formatter_ASPNET_Error", new object[] { HttpRuntime.AppDomainAppVirtualPath }) + "<hr width=100% size=1 color=silver></H1>\r\n\r\n");
			stringBuilder.Append("            <h2> <i>" + this.ErrorTitle + "</i> </h2></span>\r\n\r\n");
			stringBuilder.Append("            <font face=\"Arial, Helvetica, Geneva, SunSans-Regular, sans-serif \">\r\n\r\n");
			stringBuilder.Append(string.Concat(new string[]
			{
				"            <b> ",
				SR.GetString("Error_Formatter_Description"),
				" </b>",
				this.Description,
				"\r\n"
			}));
			stringBuilder.Append("            <br><br>\r\n\r\n");
			if (this.MiscSectionTitle != null)
			{
				stringBuilder.Append(string.Concat(new string[] { "            <b> ", this.MiscSectionTitle, ": </b>", this.MiscSectionContent, "<br><br>\r\n\r\n" }));
			}
			this.WriteColoredSquare(stringBuilder, this.ColoredSquareTitle, this.ColoredSquareDescription, this.ColoredSquareContent, this.WrapColoredSquareContentLines);
			if (this.ShowSourceFileInfo)
			{
				string text = this.GetDisplayPath();
				if (text == null)
				{
					text = SR.GetString("Error_Formatter_No_Source_File");
				}
				stringBuilder.Append(string.Concat(new object[]
				{
					"            <b> ",
					SR.GetString("Error_Formatter_Source_File"),
					" </b> ",
					text,
					"<b> &nbsp;&nbsp; ",
					SR.GetString("Error_Formatter_Line"),
					" </b> ",
					this.SourceFileLineNumber,
					"\r\n"
				}));
				stringBuilder.Append("            <br><br>\r\n\r\n");
			}
			ConfigurationErrorsException ex = this.Exception as ConfigurationErrorsException;
			if (ex != null && ex.Errors.Count > 1)
			{
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<br><div class=\"expandable\" onclick=\"OnToggleTOCLevel1('{0}')\">{1}:</div>\r\n<div id=\"{0}\" style=\"display: none;\">\r\n            <br><table width=100% bgcolor=\"#ffffcc\">\r\n               <tr>\r\n                  <td>\r\n                      <code><pre>\r\n\r\n", new object[]
				{
					"additionalConfigurationErrors",
					SR.GetString("TmplConfigurationAdditionalError")
				}));
				bool flag = false;
				try
				{
					PermissionSet namedPermissionSet = HttpRuntime.NamedPermissionSet;
					if (namedPermissionSet != null)
					{
						namedPermissionSet.PermitOnly();
						flag = true;
					}
					int num = 0;
					foreach (object obj in ex.Errors)
					{
						ConfigurationException ex2 = (ConfigurationException)obj;
						if (num > 0)
						{
							stringBuilder.Append(ex2.Message);
							stringBuilder.Append("<BR/>\r\n");
						}
						num++;
					}
				}
				finally
				{
					if (flag)
					{
						CodeAccessPermission.RevertPermitOnly();
					}
				}
				stringBuilder.Append("                      </pre></code>\r\n\r\n                  </td>\r\n               </tr>\r\n            </table>\r\n\r\n            \r\n\r\n</div>\r\n");
				stringBuilder.Append("\r\n        <script type=\"text/javascript\">\r\n        function OnToggleTOCLevel1(level2ID)\r\n        {\r\n        var elemLevel2 = document.getElementById(level2ID);\r\n        if (elemLevel2.style.display == 'none')\r\n        {\r\n            elemLevel2.style.display = '';\r\n        }\r\n        else {\r\n            elemLevel2.style.display = 'none';\r\n        }\r\n        }\r\n        </script>\r\n                            ");
			}
			if (!dontShowSensitiveInfo && this.Exception != null && HttpRuntime.HasAspNetHostingPermission(AspNetHostingPermissionLevel.Medium))
			{
				for (Exception ex3 = this.Exception; ex3 != null; ex3 = ex3.InnerException)
				{
					string text2 = null;
					string text3 = null;
					FileNotFoundException ex4 = ex3 as FileNotFoundException;
					if (ex4 != null)
					{
						text2 = ex4.FusionLog;
						text3 = ex4.FileName;
					}
					FileLoadException ex5 = ex3 as FileLoadException;
					if (ex5 != null)
					{
						text2 = ex5.FusionLog;
						text3 = ex5.FileName;
					}
					BadImageFormatException ex6 = ex3 as BadImageFormatException;
					if (ex6 != null)
					{
						text2 = ex6.FusionLog;
						text3 = ex6.FileName;
					}
					if (!string.IsNullOrEmpty(text2))
					{
						this.WriteColoredSquare(stringBuilder, SR.GetString("Error_Formatter_FusionLog"), SR.GetString("Error_Formatter_FusionLogDesc", new object[] { text3 }), HttpUtility.HtmlEncode(text2), false);
						break;
					}
				}
			}
			this.WriteColoredSquare(stringBuilder, this.ColoredSquare2Title, this.ColoredSquare2Description, this.ColoredSquare2Content, false);
			if (!dontShowSensitiveInfo && !this._dontShowVersion)
			{
				stringBuilder.Append("            <hr width=100% size=1 color=silver>\r\n\r\n");
				stringBuilder.Append(string.Concat(new string[]
				{
					"            <b>",
					SR.GetString("Error_Formatter_Version"),
					"</b>&nbsp;",
					SR.GetString("Error_Formatter_CLR_Build"),
					VersionInfo.ClrVersion,
					SR.GetString("Error_Formatter_ASPNET_Build"),
					VersionInfo.EngineVersion,
					"\r\n\r\n"
				}));
				stringBuilder.Append("            </font>\r\n\r\n");
			}
			stringBuilder.Append("    </body>\r\n");
			stringBuilder.Append("</html>\r\n");
			stringBuilder.Append(this.PostMessage);
			return stringBuilder.ToString();
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003DE0 File Offset: 0x00002DE0
		private void WriteColoredSquare(StringBuilder sb, string title, string description, string content, bool wrapContentLines)
		{
			if (title != null)
			{
				sb.Append(string.Concat(new string[] { "            <b>", title, ":</b> ", description, "<br><br>\r\n\r\n" }));
				sb.Append("            <table width=100% bgcolor=\"#ffffcc\">\r\n");
				sb.Append("               <tr>\r\n");
				sb.Append("                  <td>\r\n");
				sb.Append("                      <code>");
				if (!wrapContentLines)
				{
					sb.Append("<pre>");
				}
				sb.Append("\r\n\r\n");
				sb.Append(content);
				if (!wrapContentLines)
				{
					sb.Append("</pre>");
				}
				sb.Append("</code>\r\n\r\n");
				sb.Append("                  </td>\r\n");
				sb.Append("               </tr>\r\n");
				sb.Append("            </table>\r\n\r\n");
				sb.Append("            <br>\r\n\r\n");
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003EC8 File Offset: 0x00002EC8
		internal virtual void PrepareFormatter()
		{
			if (this._adaptiveMiscContent != null)
			{
				this._adaptiveMiscContent.Clear();
			}
			if (this._adaptiveStackTrace != null)
			{
				this._adaptiveStackTrace.Clear();
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00003EF0 File Offset: 0x00002EF0
		protected virtual Exception Exception
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000069 RID: 105
		protected abstract string ErrorTitle { get; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600006A RID: 106
		protected abstract string Description { get; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600006B RID: 107
		protected abstract string MiscSectionTitle { get; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600006C RID: 108
		protected abstract string MiscSectionContent { get; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00003EF3 File Offset: 0x00002EF3
		protected virtual string ColoredSquareTitle
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00003EF6 File Offset: 0x00002EF6
		protected virtual string ColoredSquareDescription
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00003EF9 File Offset: 0x00002EF9
		protected virtual string ColoredSquareContent
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00003EFC File Offset: 0x00002EFC
		protected virtual bool WrapColoredSquareContentLines
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00003EFF File Offset: 0x00002EFF
		protected virtual string ColoredSquare2Title
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00003F02 File Offset: 0x00002F02
		protected virtual string ColoredSquare2Description
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00003F05 File Offset: 0x00002F05
		protected virtual string ColoredSquare2Content
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00003F08 File Offset: 0x00002F08
		protected virtual StringCollection AdaptiveMiscContent
		{
			get
			{
				if (this._adaptiveMiscContent == null)
				{
					this._adaptiveMiscContent = new StringCollection();
				}
				return this._adaptiveMiscContent;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00003F23 File Offset: 0x00002F23
		protected virtual StringCollection AdaptiveStackTrace
		{
			get
			{
				if (this._adaptiveStackTrace == null)
				{
					this._adaptiveStackTrace = new StringCollection();
				}
				return this._adaptiveStackTrace;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000076 RID: 118
		protected abstract bool ShowSourceFileInfo { get; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00003F3E File Offset: 0x00002F3E
		protected virtual string PhysicalPath
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00003F41 File Offset: 0x00002F41
		protected virtual string VirtualPath
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00003F44 File Offset: 0x00002F44
		protected virtual int SourceFileLineNumber
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00003F47 File Offset: 0x00002F47
		protected virtual string PostMessage
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00003F4A File Offset: 0x00002F4A
		internal virtual bool CanBeShownToAllUsers
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00003F4D File Offset: 0x00002F4D
		protected static bool IsTextRightToLeft
		{
			get
			{
				return CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft;
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003F5E File Offset: 0x00002F5E
		protected string WrapWithLeftToRightTextFormatIfNeeded(string content)
		{
			if (ErrorFormatter.IsTextRightToLeft)
			{
				content = "<div dir=\"ltr\">" + content + "</div>";
			}
			return content;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003F7C File Offset: 0x00002F7C
		internal static string MakeHttpLinePragma(string virtualPath)
		{
			string text = "http://server";
			if (virtualPath != null && !virtualPath.StartsWith("/", StringComparison.Ordinal))
			{
				text += "/";
			}
			return new Uri(text + virtualPath).ToString();
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003FC0 File Offset: 0x00002FC0
		internal static string GetSafePath(string linePragma)
		{
			string virtualPathFromHttpLinePragma = ErrorFormatter.GetVirtualPathFromHttpLinePragma(linePragma);
			if (virtualPathFromHttpLinePragma != null)
			{
				return virtualPathFromHttpLinePragma;
			}
			return HttpRuntime.GetSafePath(linePragma);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003FE0 File Offset: 0x00002FE0
		internal static string GetVirtualPathFromHttpLinePragma(string linePragma)
		{
			if (string.IsNullOrEmpty(linePragma))
			{
				return null;
			}
			try
			{
				Uri uri = new Uri(linePragma);
				if (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
				{
					return uri.LocalPath;
				}
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004044 File Offset: 0x00003044
		internal static string ResolveHttpFileName(string linePragma)
		{
			string virtualPathFromHttpLinePragma = ErrorFormatter.GetVirtualPathFromHttpLinePragma(linePragma);
			if (virtualPathFromHttpLinePragma == null)
			{
				return linePragma;
			}
			return HostingEnvironment.MapPathInternal(virtualPathFromHttpLinePragma);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004063 File Offset: 0x00003063
		private string GetDisplayPath()
		{
			if (this.VirtualPath != null)
			{
				return this.VirtualPath;
			}
			if (this.PhysicalPath != null)
			{
				return HttpRuntime.GetSafePath(this.PhysicalPath);
			}
			return null;
		}

		// Token: 0x04000D12 RID: 3346
		private const string startExpandableBlock = "<br><div class=\"expandable\" onclick=\"OnToggleTOCLevel1('{0}')\">{1}:</div>\r\n<div id=\"{0}\" style=\"display: none;\">\r\n            <br><table width=100% bgcolor=\"#ffffcc\">\r\n               <tr>\r\n                  <td>\r\n                      <code><pre>\r\n\r\n";

		// Token: 0x04000D13 RID: 3347
		private const string endExpandableBlock = "                      </pre></code>\r\n\r\n                  </td>\r\n               </tr>\r\n            </table>\r\n\r\n            \r\n\r\n</div>\r\n";

		// Token: 0x04000D14 RID: 3348
		private const string toggleScript = "\r\n        <script type=\"text/javascript\">\r\n        function OnToggleTOCLevel1(level2ID)\r\n        {\r\n        var elemLevel2 = document.getElementById(level2ID);\r\n        if (elemLevel2.style.display == 'none')\r\n        {\r\n            elemLevel2.style.display = '';\r\n        }\r\n        else {\r\n            elemLevel2.style.display = 'none';\r\n        }\r\n        }\r\n        </script>\r\n                            ";

		// Token: 0x04000D15 RID: 3349
		protected const string BeginLeftToRightTag = "<div dir=\"ltr\">";

		// Token: 0x04000D16 RID: 3350
		protected const string EndLeftToRightTag = "</div>";

		// Token: 0x04000D17 RID: 3351
		private StringCollection _adaptiveMiscContent;

		// Token: 0x04000D18 RID: 3352
		private StringCollection _adaptiveStackTrace;

		// Token: 0x04000D19 RID: 3353
		protected bool _dontShowVersion;
	}
}
