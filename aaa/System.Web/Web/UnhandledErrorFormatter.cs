using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x0200001A RID: 26
	internal class UnhandledErrorFormatter : ErrorFormatter
	{
		// Token: 0x06000084 RID: 132 RVA: 0x00004091 File Offset: 0x00003091
		internal UnhandledErrorFormatter(Exception e)
			: this(e, null, null)
		{
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000409C File Offset: 0x0000309C
		internal UnhandledErrorFormatter(Exception e, string message, string postMessage)
		{
			this._message = message;
			this._postMessage = postMessage;
			this._e = e;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000040C4 File Offset: 0x000030C4
		internal override void PrepareFormatter()
		{
			for (Exception ex = this._e; ex != null; ex = ex.InnerException)
			{
				this._exStack.Add(ex);
				this._initialException = ex;
			}
			this._coloredSquare2Content = this.ColoredSquare2Content;
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00004104 File Offset: 0x00003104
		protected override Exception Exception
		{
			get
			{
				return this._e;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000088 RID: 136 RVA: 0x0000410C File Offset: 0x0000310C
		protected override string ErrorTitle
		{
			get
			{
				string message = this._initialException.Message;
				if (!string.IsNullOrEmpty(message))
				{
					return HttpUtility.FormatPlainTextAsHtml(message);
				}
				return SR.GetString("Unhandled_Err_Error");
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000089 RID: 137 RVA: 0x0000413E File Offset: 0x0000313E
		protected override string Description
		{
			get
			{
				if (this._message != null)
				{
					return this._message;
				}
				return SR.GetString("Unhandled_Err_Desc");
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00004159 File Offset: 0x00003159
		protected override string MiscSectionTitle
		{
			get
			{
				return SR.GetString("Unhandled_Err_Exception_Details");
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00004168 File Offset: 0x00003168
		protected override string MiscSectionContent
		{
			get
			{
				string fullName = this._initialException.GetType().FullName;
				StringBuilder stringBuilder = new StringBuilder(fullName);
				string text = fullName;
				if (this._initialException.Message != null)
				{
					string text2 = HttpUtility.FormatPlainTextAsHtml(this._initialException.Message);
					stringBuilder.Append(": ");
					stringBuilder.Append(text2);
					text = text + ": " + text2;
				}
				this.AdaptiveMiscContent.Add(text);
				if (this._initialException is UnauthorizedAccessException)
				{
					stringBuilder.Append("\r\n<br><br>");
					string text3 = SR.GetString("Unauthorized_Err_Desc1");
					text3 = HttpUtility.HtmlEncode(text3);
					stringBuilder.Append(text3);
					this.AdaptiveMiscContent.Add(text3);
					stringBuilder.Append("\r\n<br><br>");
					text3 = SR.GetString("Unauthorized_Err_Desc2");
					text3 = HttpUtility.HtmlEncode(text3);
					stringBuilder.Append(text3);
					this.AdaptiveMiscContent.Add(text3);
				}
				else if (this._initialException is HostingEnvironmentException)
				{
					string details = ((HostingEnvironmentException)this._initialException).Details;
					if (!string.IsNullOrEmpty(details))
					{
						stringBuilder.Append("\r\n<br><br><b>");
						stringBuilder.Append(details);
						stringBuilder.Append("</b>");
						this.AdaptiveMiscContent.Add(details);
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600008C RID: 140 RVA: 0x000042B7 File Offset: 0x000032B7
		protected override string ColoredSquareTitle
		{
			get
			{
				return SR.GetString("TmplCompilerSourceSecTitle");
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600008D RID: 141 RVA: 0x000042C4 File Offset: 0x000032C4
		protected override string ColoredSquareContent
		{
			get
			{
				if (this._physicalPath == null)
				{
					bool flag = false;
					string text;
					if (!this._fGeneratedCodeOnStack || !HttpRuntime.HasAspNetHostingPermission(AspNetHostingPermissionLevel.Medium))
					{
						text = SR.GetString("Src_not_available_nodebug");
					}
					else
					{
						if (ErrorFormatter.IsTextRightToLeft)
						{
							flag = true;
						}
						text = SR.GetString("Src_not_available", new object[]
						{
							flag ? "BeginMarker" : string.Empty,
							flag ? "EndMarker" : string.Empty,
							flag ? "BeginMarker" : string.Empty,
							flag ? "EndMarker" : string.Empty
						});
					}
					text = HttpUtility.FormatPlainTextAsHtml(text);
					if (flag)
					{
						text = text.Replace("BeginMarker", "</code><div dir=\"ltr\"><code>");
						text = text.Replace("EndMarker", "</code></div><code>");
					}
					return text;
				}
				return FormatterWithFileInfo.GetSourceFileLines(this._physicalPath, Encoding.Default, null, this._line);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600008E RID: 142 RVA: 0x000043A8 File Offset: 0x000033A8
		protected override bool WrapColoredSquareContentLines
		{
			get
			{
				return this._physicalPath == null;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600008F RID: 143 RVA: 0x000043B3 File Offset: 0x000033B3
		protected override string ColoredSquare2Title
		{
			get
			{
				return SR.GetString("Unhandled_Err_Stack_Trace");
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000090 RID: 144 RVA: 0x000043C0 File Offset: 0x000033C0
		protected override string ColoredSquare2Content
		{
			get
			{
				if (this._coloredSquare2Content != null)
				{
					return this._coloredSquare2Content;
				}
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = true;
				int num = 0;
				for (int i = this._exStack.Count - 1; i >= 0; i--)
				{
					if (i < this._exStack.Count - 1)
					{
						stringBuilder.Append("\r\n");
					}
					Exception ex = (Exception)this._exStack[i];
					stringBuilder.Append("[" + this._exStack[i].GetType().Name);
					if (ex is ExternalException && ((ExternalException)ex).ErrorCode != 0)
					{
						stringBuilder.Append(" (0x" + ((ExternalException)ex).ErrorCode.ToString("x", CultureInfo.CurrentCulture) + ")");
					}
					if (ex.Message != null && ex.Message.Length > 0)
					{
						stringBuilder.Append(": " + ex.Message);
					}
					stringBuilder.Append("]\r\n");
					StackTrace stackTrace = new StackTrace(ex, true);
					for (int j = 0; j < stackTrace.FrameCount; j++)
					{
						if (flag)
						{
							num = stringBuilder.Length;
						}
						StackFrame frame = stackTrace.GetFrame(j);
						MethodBase method = frame.GetMethod();
						Type declaringType = method.DeclaringType;
						string text = string.Empty;
						if (declaringType != null)
						{
							string text2 = null;
							try
							{
								text2 = Util.GetAssemblyCodeBase(declaringType.Assembly);
							}
							catch
							{
							}
							if (text2 != null)
							{
								text2 = Path.GetDirectoryName(text2);
								if (string.Compare(text2, HttpRuntime.CodegenDirInternal, StringComparison.OrdinalIgnoreCase) == 0 && frame.GetNativeOffset() > 0)
								{
									this._fGeneratedCodeOnStack = true;
								}
							}
							text = declaringType.Namespace;
						}
						if (text != null)
						{
							text += ".";
						}
						if (declaringType == null)
						{
							stringBuilder.Append("   " + method.Name + "(");
						}
						else
						{
							stringBuilder.Append(string.Concat(new string[] { "   ", text, declaringType.Name, ".", method.Name, "(" }));
						}
						ParameterInfo[] parameters = method.GetParameters();
						for (int k = 0; k < parameters.Length; k++)
						{
							stringBuilder.Append(((k != 0) ? ", " : string.Empty) + parameters[k].ParameterType.Name + " " + parameters[k].Name);
						}
						stringBuilder.Append(")");
						string text3 = frame.GetFileName();
						if (text3 != null)
						{
							text3 = ErrorFormatter.ResolveHttpFileName(text3);
							if (text3 != null)
							{
								if (this._physicalPath == null && FileUtil.FileExists(text3))
								{
									this._physicalPath = text3;
									this._line = frame.GetFileLineNumber();
								}
								stringBuilder.Append(string.Concat(new object[]
								{
									" in ",
									HttpRuntime.GetSafePath(text3),
									":",
									frame.GetFileLineNumber()
								}));
							}
						}
						else
						{
							stringBuilder.Append(" +" + frame.GetNativeOffset());
						}
						if (flag)
						{
							string text4 = stringBuilder.ToString(num, stringBuilder.Length - num);
							this.AdaptiveStackTrace.Add(HttpUtility.HtmlEncode(text4));
						}
						stringBuilder.Append("\r\n");
					}
					flag = false;
				}
				this._coloredSquare2Content = HttpUtility.HtmlEncode(stringBuilder.ToString());
				this._coloredSquare2Content = base.WrapWithLeftToRightTextFormatIfNeeded(this._coloredSquare2Content);
				return this._coloredSquare2Content;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00004780 File Offset: 0x00003780
		protected override string PostMessage
		{
			get
			{
				return this._postMessage;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00004788 File Offset: 0x00003788
		protected override bool ShowSourceFileInfo
		{
			get
			{
				return this._physicalPath != null;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00004796 File Offset: 0x00003796
		protected override string PhysicalPath
		{
			get
			{
				return this._physicalPath;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000094 RID: 148 RVA: 0x0000479E File Offset: 0x0000379E
		protected override int SourceFileLineNumber
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x04000D1A RID: 3354
		protected Exception _e;

		// Token: 0x04000D1B RID: 3355
		protected Exception _initialException;

		// Token: 0x04000D1C RID: 3356
		protected ArrayList _exStack = new ArrayList();

		// Token: 0x04000D1D RID: 3357
		protected string _physicalPath;

		// Token: 0x04000D1E RID: 3358
		protected int _line;

		// Token: 0x04000D1F RID: 3359
		private string _coloredSquare2Content;

		// Token: 0x04000D20 RID: 3360
		private bool _fGeneratedCodeOnStack;

		// Token: 0x04000D21 RID: 3361
		protected string _message;

		// Token: 0x04000D22 RID: 3362
		protected string _postMessage;
	}
}
