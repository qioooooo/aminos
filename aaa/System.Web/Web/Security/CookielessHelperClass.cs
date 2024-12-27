using System;
using System.Web.Util;

namespace System.Web.Security
{
	// Token: 0x0200032D RID: 813
	internal sealed class CookielessHelperClass
	{
		// Token: 0x060027EE RID: 10222 RVA: 0x000AEE7B File Offset: 0x000ADE7B
		internal CookielessHelperClass(HttpContext context)
		{
			this._Context = context;
		}

		// Token: 0x060027EF RID: 10223 RVA: 0x000AEE8C File Offset: 0x000ADE8C
		private void Init()
		{
			if (this._Headers != null)
			{
				return;
			}
			if (this._Headers == null)
			{
				this.GetCookielessValuesFromHeader();
			}
			if (this._Headers == null)
			{
				this.RemoveCookielessValuesFromPath();
			}
			if (this._Headers == null)
			{
				this._Headers = string.Empty;
			}
			this._OriginalHeaders = this._Headers;
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x000AEEE0 File Offset: 0x000ADEE0
		private void GetCookielessValuesFromHeader()
		{
			this._Headers = this._Context.Request.Headers["AspFilterSessionId"];
			if (!string.IsNullOrEmpty(this._Headers))
			{
				if (this._Headers.Length == 24 && !this._Headers.Contains("("))
				{
					this._Headers = null;
					return;
				}
				this._Context.Response.SetAppPathModifier("(" + this._Headers + ")");
			}
		}

		// Token: 0x060027F1 RID: 10225 RVA: 0x000AEF68 File Offset: 0x000ADF68
		internal void RemoveCookielessValuesFromPath()
		{
			bool flag = this._Context.WorkerRequest != null && this._Context.WorkerRequest.IsRewriteModuleEnabled;
			string text = (flag ? this._Context.Request.ClientFilePath.VirtualPathString : this._Context.Request.FilePath);
			int num;
			int num2;
			if (AppSettings.RestoreAggressiveCookielessPathRemoval)
			{
				num = text.LastIndexOf(")/", StringComparison.Ordinal);
				num2 = ((num > 2) ? text.LastIndexOf("/(", num - 1, num, StringComparison.Ordinal) : (-1));
			}
			else
			{
				num2 = text.IndexOf("/(", StringComparison.Ordinal);
				num = ((num2 >= 0) ? text.IndexOf(")/", num2 + 2, StringComparison.Ordinal) : (-1));
			}
			if (num2 < 0)
			{
				return;
			}
			if (this._Headers == null)
			{
				this.GetCookielessValuesFromHeader();
			}
			if (!flag && this._Headers != null)
			{
				return;
			}
			if (CookielessHelperClass.IsValidHeader(text, num2 + 2, num))
			{
				if (!flag || this._Headers == null)
				{
					this._Headers = text.Substring(num2 + 2, num - num2 - 2);
				}
				text = text.Substring(0, num2) + text.Substring(num + 1);
				if (!flag)
				{
					this._Context.RewritePath(VirtualPath.CreateAbsolute(text), this._Context.Request.PathInfoObject, null, true);
				}
				else
				{
					this._Context.Request.ClientFilePath = VirtualPath.CreateAbsolute(text);
					string rawUrl = this._Context.WorkerRequest.GetRawUrl();
					int num3 = rawUrl.IndexOf('?');
					if (num3 > -1)
					{
						text += rawUrl.Substring(num3);
					}
					this._Context.WorkerRequest.SetRawUrl(text);
				}
				if (!string.IsNullOrEmpty(this._Headers))
				{
					this._Context.Response.SetAppPathModifier("(" + this._Headers + ")");
				}
			}
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x000AF12C File Offset: 0x000AE12C
		internal string GetCookieValue(char identifier)
		{
			int num = 0;
			int num2 = 0;
			this.Init();
			if (!CookielessHelperClass.GetValueStartAndEnd(this._Headers, identifier, out num, out num2))
			{
				return null;
			}
			return this._Headers.Substring(num, num2 - num);
		}

		// Token: 0x060027F3 RID: 10227 RVA: 0x000AF168 File Offset: 0x000AE168
		internal bool DoesCookieValueExistInOriginal(char identifier)
		{
			int num = 0;
			int num2 = 0;
			this.Init();
			return CookielessHelperClass.GetValueStartAndEnd(this._OriginalHeaders, identifier, out num, out num2);
		}

		// Token: 0x060027F4 RID: 10228 RVA: 0x000AF190 File Offset: 0x000AE190
		internal void SetCookieValue(char identifier, string cookieValue)
		{
			int num = 0;
			int num2 = 0;
			this.Init();
			while (CookielessHelperClass.GetValueStartAndEnd(this._Headers, identifier, out num, out num2))
			{
				this._Headers = this._Headers.Substring(0, num - 2) + this._Headers.Substring(num2 + 1);
			}
			if (!string.IsNullOrEmpty(cookieValue))
			{
				this._Headers = this._Headers + new string(new char[] { identifier, '(' }) + cookieValue + ")";
			}
			if (this._Headers.Length > 0)
			{
				this._Context.Response.SetAppPathModifier("(" + this._Headers + ")");
				return;
			}
			this._Context.Response.SetAppPathModifier(null);
		}

		// Token: 0x060027F5 RID: 10229 RVA: 0x000AF260 File Offset: 0x000AE260
		private static bool GetValueStartAndEnd(string headers, char identifier, out int startPos, out int endPos)
		{
			if (string.IsNullOrEmpty(headers))
			{
				startPos = (endPos = -1);
				return false;
			}
			string text = new string(new char[] { identifier, '(' });
			startPos = headers.IndexOf(text, StringComparison.Ordinal);
			if (startPos < 0)
			{
				startPos = (endPos = -1);
				return false;
			}
			startPos += 2;
			endPos = headers.IndexOf(')', startPos);
			if (endPos < 0)
			{
				startPos = (endPos = -1);
				return false;
			}
			return true;
		}

		// Token: 0x060027F6 RID: 10230 RVA: 0x000AF2D4 File Offset: 0x000AE2D4
		internal static bool UseCookieless(HttpContext context, bool doRedirect, HttpCookieMode cookieMode)
		{
			switch (cookieMode)
			{
			case HttpCookieMode.UseUri:
				return true;
			case HttpCookieMode.UseCookies:
				return false;
			case HttpCookieMode.AutoDetect:
			{
				if (context == null)
				{
					context = HttpContext.Current;
				}
				if (context == null)
				{
					return false;
				}
				if (!context.Request.Browser.Cookies || !context.Request.Browser.SupportsRedirectWithCookie)
				{
					return true;
				}
				string cookieValue = context.CookielessHelper.GetCookieValue('X');
				if (cookieValue != null && cookieValue == "1")
				{
					return true;
				}
				string text = context.Request.Headers["Cookie"];
				if (!string.IsNullOrEmpty(text))
				{
					return false;
				}
				string text2 = context.Request.QueryString["AspxAutoDetectCookieSupport"];
				if (text2 != null && text2 == "1")
				{
					context.CookielessHelper.SetCookieValue('X', "1");
					return true;
				}
				if (doRedirect)
				{
					context.CookielessHelper.RedirectWithDetection(null);
				}
				return false;
			}
			case HttpCookieMode.UseDeviceProfile:
				if (context == null)
				{
					context = HttpContext.Current;
				}
				return context != null && (!context.Request.Browser.Cookies || !context.Request.Browser.SupportsRedirectWithCookie);
			default:
				return false;
			}
		}

		// Token: 0x060027F7 RID: 10231 RVA: 0x000AF400 File Offset: 0x000AE400
		internal void RedirectWithDetection(string redirectPath)
		{
			this.Init();
			if (string.IsNullOrEmpty(redirectPath))
			{
				redirectPath = this._Context.Request.PathWithQueryString;
			}
			if (redirectPath.IndexOf("?", StringComparison.Ordinal) > 0)
			{
				redirectPath += "&AspxAutoDetectCookieSupport=1";
			}
			else
			{
				redirectPath += "?AspxAutoDetectCookieSupport=1";
			}
			this._Context.Response.Cookies.Add(new HttpCookie("AspxAutoDetectCookieSupport", "1"));
			this._Context.Response.Redirect(redirectPath, true);
		}

		// Token: 0x060027F8 RID: 10232 RVA: 0x000AF490 File Offset: 0x000AE490
		internal void RedirectWithDetectionIfRequired(string redirectPath, HttpCookieMode cookieMode)
		{
			this.Init();
			if (cookieMode != HttpCookieMode.AutoDetect)
			{
				return;
			}
			if (!this._Context.Request.Browser.Cookies || !this._Context.Request.Browser.SupportsRedirectWithCookie)
			{
				return;
			}
			string cookieValue = this.GetCookieValue('X');
			if (cookieValue != null && cookieValue == "1")
			{
				return;
			}
			string text = this._Context.Request.Headers["Cookie"];
			if (!string.IsNullOrEmpty(text))
			{
				return;
			}
			string text2 = this._Context.Request.QueryString["AspxAutoDetectCookieSupport"];
			if (text2 != null && text2 == "1")
			{
				this.SetCookieValue('X', "1");
				return;
			}
			this.RedirectWithDetection(redirectPath);
		}

		// Token: 0x060027F9 RID: 10233 RVA: 0x000AF558 File Offset: 0x000AE558
		private static bool IsValidHeader(string path, int startPos, int endPos)
		{
			if (endPos - startPos < 3)
			{
				return false;
			}
			while (startPos <= endPos - 3)
			{
				if (path[startPos] < 'A' || path[startPos] > 'Z')
				{
					return false;
				}
				if (path[startPos + 1] != '(')
				{
					return false;
				}
				startPos += 2;
				bool flag = false;
				while (startPos < endPos)
				{
					if (path[startPos] == ')')
					{
						startPos++;
						flag = true;
						break;
					}
					if (AppSettings.RestoreAggressiveCookielessPathRemoval)
					{
						if (path[startPos] == '/')
						{
							return false;
						}
					}
					else
					{
						char c = path[startPos];
						if ((c < 'A' || c > 'Z') && (c < 'a' || c > 'z') && (c < '0' || c > '9') && c != '-' && c != '_')
						{
							return false;
						}
					}
					startPos++;
				}
				if (!flag)
				{
					return false;
				}
			}
			return startPos >= endPos;
		}

		// Token: 0x04001E73 RID: 7795
		private const string COOKIELESS_SESSION_FILTER_HEADER = "AspFilterSessionId";

		// Token: 0x04001E74 RID: 7796
		private const string s_AutoDetectName = "AspxAutoDetectCookieSupport";

		// Token: 0x04001E75 RID: 7797
		private const string s_AutoDetectValue = "1";

		// Token: 0x04001E76 RID: 7798
		private HttpContext _Context;

		// Token: 0x04001E77 RID: 7799
		private string _Headers;

		// Token: 0x04001E78 RID: 7800
		private string _OriginalHeaders;
	}
}
