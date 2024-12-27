using System;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x0200019F RID: 415
	internal static class AuthenticationConfig
	{
		// Token: 0x06001168 RID: 4456 RVA: 0x0004DA52 File Offset: 0x0004CA52
		internal static string GetCompleteLoginUrl(HttpContext context, string loginUrl)
		{
			if (string.IsNullOrEmpty(loginUrl))
			{
				return string.Empty;
			}
			if (UrlPath.IsRelativeUrl(loginUrl))
			{
				loginUrl = UrlPath.Combine(HttpRuntime.AppDomainAppVirtualPathString, loginUrl);
			}
			return loginUrl;
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x0004DA78 File Offset: 0x0004CA78
		internal static bool AccessingLoginPage(HttpContext context, string loginUrl)
		{
			if (string.IsNullOrEmpty(loginUrl))
			{
				return false;
			}
			loginUrl = AuthenticationConfig.GetCompleteLoginUrl(context, loginUrl);
			if (string.IsNullOrEmpty(loginUrl))
			{
				return false;
			}
			int num = loginUrl.IndexOf('?');
			if (num >= 0)
			{
				loginUrl = loginUrl.Substring(0, num);
			}
			string path = context.Request.Path;
			if (StringUtil.EqualsIgnoreCase(path, loginUrl))
			{
				return true;
			}
			if (loginUrl.IndexOf('%') >= 0)
			{
				string text = HttpUtility.UrlDecode(loginUrl);
				if (StringUtil.EqualsIgnoreCase(path, text))
				{
					return true;
				}
				text = HttpUtility.UrlDecode(loginUrl, context.Request.ContentEncoding);
				if (StringUtil.EqualsIgnoreCase(path, text))
				{
					return true;
				}
			}
			return false;
		}
	}
}
