using System;
using System.Globalization;
using System.Security.Permissions;
using System.Text;
using System.Web.Configuration;

namespace System.Web.Security
{
	// Token: 0x02000324 RID: 804
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class AnonymousIdentificationModule : IHttpModule
	{
		// Token: 0x06002795 RID: 10133 RVA: 0x000AD08D File Offset: 0x000AC08D
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		public AnonymousIdentificationModule()
		{
		}

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06002796 RID: 10134 RVA: 0x000AD095 File Offset: 0x000AC095
		// (remove) Token: 0x06002797 RID: 10135 RVA: 0x000AD0AE File Offset: 0x000AC0AE
		public event AnonymousIdentificationEventHandler Creating
		{
			add
			{
				this._CreateNewIdEventHandler = (AnonymousIdentificationEventHandler)Delegate.Combine(this._CreateNewIdEventHandler, value);
			}
			remove
			{
				this._CreateNewIdEventHandler = (AnonymousIdentificationEventHandler)Delegate.Remove(this._CreateNewIdEventHandler, value);
			}
		}

		// Token: 0x06002798 RID: 10136 RVA: 0x000AD0C8 File Offset: 0x000AC0C8
		public static void ClearAnonymousIdentifier()
		{
			if (!AnonymousIdentificationModule.s_Initialized)
			{
				AnonymousIdentificationModule.Initialize();
			}
			HttpContext httpContext = HttpContext.Current;
			if (httpContext == null)
			{
				return;
			}
			if (!AnonymousIdentificationModule.s_Enabled || !httpContext.Request.IsAuthenticated)
			{
				throw new NotSupportedException(SR.GetString("Anonymous_ClearAnonymousIdentifierNotSupported"));
			}
			bool flag = false;
			if (httpContext.CookielessHelper.GetCookieValue('A') != null)
			{
				httpContext.CookielessHelper.SetCookieValue('A', null);
				flag = true;
			}
			if (!CookielessHelperClass.UseCookieless(httpContext, false, AnonymousIdentificationModule.s_CookieMode) || httpContext.Request.Browser.Cookies)
			{
				string text = string.Empty;
				if (httpContext.Request.Browser["supportsEmptyStringInCookieValue"] == "false")
				{
					text = "NoCookie";
				}
				HttpCookie httpCookie = new HttpCookie(AnonymousIdentificationModule.s_CookieName, text);
				httpCookie.HttpOnly = true;
				httpCookie.Path = AnonymousIdentificationModule.s_CookiePath;
				httpCookie.Secure = AnonymousIdentificationModule.s_RequireSSL;
				if (AnonymousIdentificationModule.s_Domain != null)
				{
					httpCookie.Domain = AnonymousIdentificationModule.s_Domain;
				}
				httpCookie.Expires = new DateTime(1999, 10, 12);
				httpContext.Response.Cookies.RemoveCookie(AnonymousIdentificationModule.s_CookieName);
				httpContext.Response.Cookies.Add(httpCookie);
			}
			if (flag)
			{
				httpContext.Response.Redirect(httpContext.Request.PathWithQueryString, false);
			}
		}

		// Token: 0x06002799 RID: 10137 RVA: 0x000AD20F File Offset: 0x000AC20F
		public void Dispose()
		{
		}

		// Token: 0x0600279A RID: 10138 RVA: 0x000AD211 File Offset: 0x000AC211
		public void Init(HttpApplication app)
		{
			if (!AnonymousIdentificationModule.s_Initialized)
			{
				AnonymousIdentificationModule.Initialize();
			}
			if (AnonymousIdentificationModule.s_Enabled)
			{
				app.PostAuthenticateRequest += this.OnEnter;
			}
		}

		// Token: 0x0600279B RID: 10139 RVA: 0x000AD238 File Offset: 0x000AC238
		private void OnEnter(object source, EventArgs eventArgs)
		{
			if (!AnonymousIdentificationModule.s_Initialized)
			{
				AnonymousIdentificationModule.Initialize();
			}
			if (!AnonymousIdentificationModule.s_Enabled)
			{
				return;
			}
			bool flag = false;
			string text = null;
			HttpApplication httpApplication = (HttpApplication)source;
			HttpContext context = httpApplication.Context;
			bool isAuthenticated = context.Request.IsAuthenticated;
			bool flag2;
			if (isAuthenticated)
			{
				flag2 = CookielessHelperClass.UseCookieless(context, false, AnonymousIdentificationModule.s_CookieMode);
			}
			else
			{
				flag2 = CookielessHelperClass.UseCookieless(context, true, AnonymousIdentificationModule.s_CookieMode);
			}
			if (AnonymousIdentificationModule.s_RequireSSL && !context.Request.IsSecureConnection && !flag2)
			{
				HttpCookie httpCookie = context.Request.Cookies[AnonymousIdentificationModule.s_CookieName];
				if (httpCookie != null)
				{
					httpCookie = new HttpCookie(AnonymousIdentificationModule.s_CookieName, string.Empty);
					httpCookie.HttpOnly = true;
					httpCookie.Path = AnonymousIdentificationModule.s_CookiePath;
					httpCookie.Secure = AnonymousIdentificationModule.s_RequireSSL;
					if (AnonymousIdentificationModule.s_Domain != null)
					{
						httpCookie.Domain = AnonymousIdentificationModule.s_Domain;
					}
					httpCookie.Expires = new DateTime(1999, 10, 12);
					if (context.Request.Browser["supportsEmptyStringInCookieValue"] == "false")
					{
						httpCookie.Value = "NoCookie";
					}
					context.Response.Cookies.Add(httpCookie);
				}
				return;
			}
			if (!flag2)
			{
				HttpCookie httpCookie = context.Request.Cookies[AnonymousIdentificationModule.s_CookieName];
				if (httpCookie != null)
				{
					text = httpCookie.Value;
					httpCookie.Path = AnonymousIdentificationModule.s_CookiePath;
					if (AnonymousIdentificationModule.s_Domain != null)
					{
						httpCookie.Domain = AnonymousIdentificationModule.s_Domain;
					}
				}
			}
			else
			{
				text = context.CookielessHelper.GetCookieValue('A');
			}
			AnonymousIdData decodedValue = AnonymousIdentificationModule.GetDecodedValue(text);
			if (decodedValue != null && decodedValue.AnonymousId != null)
			{
				context.Request._AnonymousId = decodedValue.AnonymousId;
			}
			if (isAuthenticated)
			{
				return;
			}
			if (context.Request._AnonymousId == null)
			{
				if (this._CreateNewIdEventHandler != null)
				{
					AnonymousIdentificationEventArgs anonymousIdentificationEventArgs = new AnonymousIdentificationEventArgs(context);
					this._CreateNewIdEventHandler(this, anonymousIdentificationEventArgs);
					context.Request._AnonymousId = anonymousIdentificationEventArgs.AnonymousID;
				}
				if (string.IsNullOrEmpty(context.Request._AnonymousId))
				{
					context.Request._AnonymousId = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);
				}
				else if (context.Request._AnonymousId.Length > 128)
				{
					throw new HttpException(SR.GetString("Anonymous_id_too_long"));
				}
				if (AnonymousIdentificationModule.s_RequireSSL && !context.Request.IsSecureConnection && !flag2)
				{
					return;
				}
				flag = true;
			}
			DateTime utcNow = DateTime.UtcNow;
			if (!flag && AnonymousIdentificationModule.s_SlidingExpiration)
			{
				if (decodedValue == null || decodedValue.ExpireDate < utcNow)
				{
					flag = true;
				}
				else
				{
					double totalSeconds = (decodedValue.ExpireDate - utcNow).TotalSeconds;
					if (totalSeconds < (double)(AnonymousIdentificationModule.s_CookieTimeout * 60 / 2))
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				DateTime dateTime = utcNow.AddMinutes((double)AnonymousIdentificationModule.s_CookieTimeout);
				text = AnonymousIdentificationModule.GetEncodedValue(new AnonymousIdData(context.Request.AnonymousID, dateTime));
				if (text.Length > 512)
				{
					throw new HttpException(SR.GetString("Anonymous_id_too_long_2"));
				}
				if (!flag2)
				{
					HttpCookie httpCookie = new HttpCookie(AnonymousIdentificationModule.s_CookieName, text);
					httpCookie.HttpOnly = true;
					httpCookie.Expires = dateTime;
					httpCookie.Path = AnonymousIdentificationModule.s_CookiePath;
					httpCookie.Secure = AnonymousIdentificationModule.s_RequireSSL;
					if (AnonymousIdentificationModule.s_Domain != null)
					{
						httpCookie.Domain = AnonymousIdentificationModule.s_Domain;
					}
					context.Response.Cookies.Add(httpCookie);
					return;
				}
				context.CookielessHelper.SetCookieValue('A', text);
				context.Response.Redirect(context.Request.PathWithQueryString);
			}
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x0600279C RID: 10140 RVA: 0x000AD5C8 File Offset: 0x000AC5C8
		public static bool Enabled
		{
			get
			{
				if (!AnonymousIdentificationModule.s_Initialized)
				{
					AnonymousIdentificationModule.Initialize();
				}
				return AnonymousIdentificationModule.s_Enabled;
			}
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x000AD5DC File Offset: 0x000AC5DC
		private static void Initialize()
		{
			if (AnonymousIdentificationModule.s_Initialized)
			{
				return;
			}
			lock (AnonymousIdentificationModule.s_InitLock)
			{
				if (!AnonymousIdentificationModule.s_Initialized)
				{
					AnonymousIdentificationSection anonymousIdentification = RuntimeConfig.GetAppConfig().AnonymousIdentification;
					AnonymousIdentificationModule.s_Enabled = anonymousIdentification.Enabled;
					AnonymousIdentificationModule.s_CookieName = anonymousIdentification.CookieName;
					AnonymousIdentificationModule.s_CookiePath = anonymousIdentification.CookiePath;
					AnonymousIdentificationModule.s_CookieTimeout = (int)anonymousIdentification.CookieTimeout.TotalMinutes;
					AnonymousIdentificationModule.s_RequireSSL = anonymousIdentification.CookieRequireSSL;
					AnonymousIdentificationModule.s_SlidingExpiration = anonymousIdentification.CookieSlidingExpiration;
					AnonymousIdentificationModule.s_Protection = anonymousIdentification.CookieProtection;
					AnonymousIdentificationModule.s_CookieMode = anonymousIdentification.Cookieless;
					AnonymousIdentificationModule.s_Domain = anonymousIdentification.Domain;
					AnonymousIdentificationModule.s_Modifier = Encoding.UTF8.GetBytes("AnonymousIdentification");
					if (AnonymousIdentificationModule.s_CookieTimeout < 1)
					{
						AnonymousIdentificationModule.s_CookieTimeout = 1;
					}
					if (AnonymousIdentificationModule.s_CookieTimeout > 1051200)
					{
						AnonymousIdentificationModule.s_CookieTimeout = 1051200;
					}
					AnonymousIdentificationModule.s_Initialized = true;
				}
			}
		}

		// Token: 0x0600279E RID: 10142 RVA: 0x000AD6D8 File Offset: 0x000AC6D8
		private static string GetEncodedValue(AnonymousIdData data)
		{
			if (data == null)
			{
				return null;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(data.AnonymousId);
			byte[] bytes2 = BitConverter.GetBytes(bytes.Length);
			byte[] bytes3 = BitConverter.GetBytes(data.ExpireDate.ToFileTimeUtc());
			byte[] array = new byte[12 + bytes.Length];
			Buffer.BlockCopy(bytes3, 0, array, 0, 8);
			Buffer.BlockCopy(bytes2, 0, array, 8, 4);
			Buffer.BlockCopy(bytes, 0, array, 12, bytes.Length);
			return CookieProtectionHelper.Encode(AnonymousIdentificationModule.s_Protection, array, array.Length);
		}

		// Token: 0x0600279F RID: 10143 RVA: 0x000AD750 File Offset: 0x000AC750
		private static AnonymousIdData GetDecodedValue(string data)
		{
			if (data == null || data.Length < 1 || data.Length > 512)
			{
				return null;
			}
			try
			{
				byte[] array = CookieProtectionHelper.Decode(AnonymousIdentificationModule.s_Protection, data);
				if (array == null || array.Length < 13)
				{
					return null;
				}
				DateTime dateTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(array, 0));
				if (dateTime < DateTime.UtcNow)
				{
					return null;
				}
				int num = BitConverter.ToInt32(array, 8);
				if (num < 0 || num > array.Length - 12)
				{
					return null;
				}
				string @string = Encoding.UTF8.GetString(array, 12, num);
				if (@string.Length > 128)
				{
					return null;
				}
				return new AnonymousIdData(@string, dateTime);
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x04001E50 RID: 7760
		private const int MAX_ENCODED_COOKIE_STRING = 512;

		// Token: 0x04001E51 RID: 7761
		private const int MAX_ID_LENGTH = 128;

		// Token: 0x04001E52 RID: 7762
		private AnonymousIdentificationEventHandler _CreateNewIdEventHandler;

		// Token: 0x04001E53 RID: 7763
		private static bool s_Initialized = false;

		// Token: 0x04001E54 RID: 7764
		private static bool s_Enabled = false;

		// Token: 0x04001E55 RID: 7765
		private static string s_CookieName = ".ASPXANONYMOUS";

		// Token: 0x04001E56 RID: 7766
		private static string s_CookiePath = "/";

		// Token: 0x04001E57 RID: 7767
		private static int s_CookieTimeout = 100000;

		// Token: 0x04001E58 RID: 7768
		private static bool s_RequireSSL = false;

		// Token: 0x04001E59 RID: 7769
		private static string s_Domain = null;

		// Token: 0x04001E5A RID: 7770
		private static bool s_SlidingExpiration = true;

		// Token: 0x04001E5B RID: 7771
		private static byte[] s_Modifier = null;

		// Token: 0x04001E5C RID: 7772
		private static object s_InitLock = new object();

		// Token: 0x04001E5D RID: 7773
		private static HttpCookieMode s_CookieMode = HttpCookieMode.UseDeviceProfile;

		// Token: 0x04001E5E RID: 7774
		private static CookieProtection s_Protection = CookieProtection.None;
	}
}
