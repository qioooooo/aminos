using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Security;
using System.Web.Util;

namespace System.Web.SessionState
{
	// Token: 0x02000368 RID: 872
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class SessionIDManager : ISessionIDManager
	{
		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06002A2E RID: 10798 RVA: 0x000BC0E2 File Offset: 0x000BB0E2
		public static int SessionIDMaxLength
		{
			get
			{
				return 80;
			}
		}

		// Token: 0x06002A2F RID: 10799 RVA: 0x000BC0E8 File Offset: 0x000BB0E8
		private void OneTimeInit()
		{
			SessionStateSection sessionState = RuntimeConfig.GetAppConfig().SessionState;
			SessionIDManager.s_appPath = HostingEnvironment.ApplicationVirtualPathObject.VirtualPathString;
			SessionIDManager.s_iSessionId = SessionIDManager.s_appPath.Length;
			SessionIDManager.s_config = sessionState;
		}

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06002A30 RID: 10800 RVA: 0x000BC124 File Offset: 0x000BB124
		private static SessionStateSection Config
		{
			get
			{
				if (SessionIDManager.s_config == null)
				{
					throw new HttpException(SR.GetString("SessionIDManager_uninit"));
				}
				return SessionIDManager.s_config;
			}
		}

		// Token: 0x06002A31 RID: 10801 RVA: 0x000BC144 File Offset: 0x000BB144
		public void Initialize()
		{
			if (SessionIDManager.s_config == null)
			{
				SessionIDManager.s_lock.AcquireWriterLock();
				try
				{
					if (SessionIDManager.s_config == null)
					{
						this.OneTimeInit();
					}
				}
				finally
				{
					SessionIDManager.s_lock.ReleaseWriterLock();
				}
			}
			this._isInherited = base.GetType() != typeof(SessionIDManager);
		}

		// Token: 0x06002A32 RID: 10802 RVA: 0x000BC1A8 File Offset: 0x000BB1A8
		internal void GetCookielessSessionID(HttpContext context, bool allowRedirect, out bool cookieless)
		{
			HttpRequest request = context.Request;
			cookieless = CookielessHelperClass.UseCookieless(context, allowRedirect, SessionIDManager.Config.Cookieless);
			context.Items["AspCookielessBoolSession"] = cookieless;
			if (cookieless)
			{
				string text = context.CookielessHelper.GetCookieValue('S');
				if (text == null)
				{
					text = string.Empty;
				}
				text = this.Decode(text);
				if (!this.ValidateInternal(text, false))
				{
					return;
				}
				context.Items.Add("AspCookielessSession", text);
			}
		}

		// Token: 0x06002A33 RID: 10803 RVA: 0x000BC228 File Offset: 0x000BB228
		private static HttpCookie CreateSessionCookie(string id)
		{
			return new HttpCookie(SessionIDManager.Config.CookieName, id)
			{
				Path = "/",
				HttpOnly = true
			};
		}

		// Token: 0x06002A34 RID: 10804 RVA: 0x000BC25C File Offset: 0x000BB25C
		internal static bool CheckIdLength(string id, bool throwOnFail)
		{
			bool flag = true;
			if (id.Length > 80)
			{
				if (throwOnFail)
				{
					throw new HttpException(SR.GetString("Session_id_too_long", new object[]
					{
						80.ToString(CultureInfo.InvariantCulture),
						id
					}));
				}
				flag = false;
			}
			return flag;
		}

		// Token: 0x06002A35 RID: 10805 RVA: 0x000BC2A9 File Offset: 0x000BB2A9
		private bool ValidateInternal(string id, bool throwOnIdCheck)
		{
			return SessionIDManager.CheckIdLength(id, throwOnIdCheck) && this.Validate(id);
		}

		// Token: 0x06002A36 RID: 10806 RVA: 0x000BC2BD File Offset: 0x000BB2BD
		public virtual bool Validate(string id)
		{
			return SessionId.IsLegit(id);
		}

		// Token: 0x06002A37 RID: 10807 RVA: 0x000BC2C5 File Offset: 0x000BB2C5
		public virtual string Encode(string id)
		{
			if (this._isInherited)
			{
				return HttpUtility.UrlEncode(id);
			}
			return id;
		}

		// Token: 0x06002A38 RID: 10808 RVA: 0x000BC2D7 File Offset: 0x000BB2D7
		public virtual string Decode(string id)
		{
			if (this._isInherited)
			{
				return HttpUtility.UrlDecode(id);
			}
			return id.ToLower(CultureInfo.InvariantCulture);
		}

		// Token: 0x06002A39 RID: 10809 RVA: 0x000BC2F4 File Offset: 0x000BB2F4
		internal bool UseCookieless(HttpContext context)
		{
			if (SessionIDManager.Config.Cookieless == HttpCookieMode.UseCookies)
			{
				return false;
			}
			object obj = context.Items["AspCookielessBoolSession"];
			return (bool)obj;
		}

		// Token: 0x06002A3A RID: 10810 RVA: 0x000BC327 File Offset: 0x000BB327
		private void CheckInitializeRequestCalled(HttpContext context)
		{
			if (context.Items["AspSessionIDManagerInitializeRequestCalled"] == null)
			{
				throw new HttpException(SR.GetString("SessionIDManager_InitializeRequest_not_called"));
			}
		}

		// Token: 0x06002A3B RID: 10811 RVA: 0x000BC34C File Offset: 0x000BB34C
		public bool InitializeRequest(HttpContext context, bool suppressAutoDetectRedirect, out bool supportSessionIDReissue)
		{
			if (context.Items["AspSessionIDManagerInitializeRequestCalled"] != null)
			{
				supportSessionIDReissue = this.UseCookieless(context);
				return false;
			}
			context.Items["AspSessionIDManagerInitializeRequestCalled"] = true;
			if (SessionIDManager.Config.Cookieless == HttpCookieMode.UseCookies)
			{
				supportSessionIDReissue = false;
				return false;
			}
			bool flag;
			this.GetCookielessSessionID(context, !suppressAutoDetectRedirect, out flag);
			supportSessionIDReissue = flag;
			return context.Response.IsRequestBeingRedirected;
		}

		// Token: 0x06002A3C RID: 10812 RVA: 0x000BC3BC File Offset: 0x000BB3BC
		public string GetSessionID(HttpContext context)
		{
			string text = null;
			this.CheckInitializeRequestCalled(context);
			if (this.UseCookieless(context))
			{
				text = (string)context.Items["AspCookielessSession"];
			}
			else
			{
				HttpCookie httpCookie = context.Request.Cookies[SessionIDManager.Config.CookieName];
				if (httpCookie != null && httpCookie.Value != null)
				{
					text = this.Decode(httpCookie.Value);
					if (text != null && !this.ValidateInternal(text, false))
					{
						text = null;
					}
				}
			}
			return text;
		}

		// Token: 0x06002A3D RID: 10813 RVA: 0x000BC436 File Offset: 0x000BB436
		public virtual string CreateSessionID(HttpContext context)
		{
			return SessionId.Create(ref this._randgen);
		}

		// Token: 0x06002A3E RID: 10814 RVA: 0x000BC444 File Offset: 0x000BB444
		public void SaveSessionID(HttpContext context, string id, out bool redirected, out bool cookieAdded)
		{
			redirected = false;
			cookieAdded = false;
			this.CheckInitializeRequestCalled(context);
			if (!context.Response.IsBuffered())
			{
				throw new HttpException(SR.GetString("Cant_save_session_id_because_response_was_flushed"));
			}
			if (!this.ValidateInternal(id, true))
			{
				throw new HttpException(SR.GetString("Cant_save_session_id_because_id_is_invalid", new object[] { id }));
			}
			string text = this.Encode(id);
			if (!this.UseCookieless(context))
			{
				HttpCookie httpCookie = SessionIDManager.CreateSessionCookie(text);
				context.Response.Cookies.Add(httpCookie);
				cookieAdded = true;
				return;
			}
			context.CookielessHelper.SetCookieValue('S', text);
			HttpRequest request = context.Request;
			string text2 = request.Path;
			string queryStringText = request.QueryStringText;
			if (!string.IsNullOrEmpty(queryStringText))
			{
				text2 = text2 + "?" + queryStringText;
			}
			context.Response.Redirect(text2, false);
			context.ApplicationInstance.CompleteRequest();
			redirected = true;
		}

		// Token: 0x06002A3F RID: 10815 RVA: 0x000BC529 File Offset: 0x000BB529
		public void RemoveSessionID(HttpContext context)
		{
			context.Response.Cookies.RemoveCookie(SessionIDManager.Config.CookieName);
		}

		// Token: 0x04001F4F RID: 8015
		private const int COOKIELESS_SESSION_LENGTH = 26;

		// Token: 0x04001F50 RID: 8016
		internal const string COOKIELESS_SESSION_KEY = "AspCookielessSession";

		// Token: 0x04001F51 RID: 8017
		internal const string COOKIELESS_BOOL_SESSION_KEY = "AspCookielessBoolSession";

		// Token: 0x04001F52 RID: 8018
		internal const string ASP_SESSIONID_MANAGER_INITIALIZEREQUEST_CALLED_KEY = "AspSessionIDManagerInitializeRequestCalled";

		// Token: 0x04001F53 RID: 8019
		internal const HttpCookieMode COOKIEMODE_DEFAULT = HttpCookieMode.UseCookies;

		// Token: 0x04001F54 RID: 8020
		internal const string SESSION_COOKIE_DEFAULT = "ASP.NET_SessionId";

		// Token: 0x04001F55 RID: 8021
		internal const int SESSION_ID_LENGTH_LIMIT = 80;

		// Token: 0x04001F56 RID: 8022
		private static string s_appPath;

		// Token: 0x04001F57 RID: 8023
		private static int s_iSessionId;

		// Token: 0x04001F58 RID: 8024
		private static ReadWriteSpinLock s_lock;

		// Token: 0x04001F59 RID: 8025
		private static SessionStateSection s_config;

		// Token: 0x04001F5A RID: 8026
		private bool _isInherited;

		// Token: 0x04001F5B RID: 8027
		private RandomNumberGenerator _randgen;
	}
}
