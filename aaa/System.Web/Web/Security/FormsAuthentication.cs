using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Web.Configuration;
using System.Web.Management;
using System.Web.Util;

namespace System.Web.Security
{
	// Token: 0x02000334 RID: 820
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class FormsAuthentication
	{
		// Token: 0x06002819 RID: 10265 RVA: 0x000AFED8 File Offset: 0x000AEED8
		public static string HashPasswordForStoringInConfigFile(string password, string passwordFormat)
		{
			if (password == null)
			{
				throw new ArgumentNullException("password");
			}
			if (passwordFormat == null)
			{
				throw new ArgumentNullException("passwordFormat");
			}
			HashAlgorithm hashAlgorithm;
			if (StringUtil.EqualsIgnoreCase(passwordFormat, "sha1"))
			{
				hashAlgorithm = SHA1.Create();
			}
			else
			{
				if (!StringUtil.EqualsIgnoreCase(passwordFormat, "md5"))
				{
					throw new ArgumentException(SR.GetString("InvalidArgumentValue", new object[] { "passwordFormat" }));
				}
				hashAlgorithm = MD5.Create();
			}
			return MachineKeySection.ByteArrayToHexString(hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(password)), 0);
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x000AFF64 File Offset: 0x000AEF64
		public static void Initialize()
		{
			if (FormsAuthentication._Initialized)
			{
				return;
			}
			lock (FormsAuthentication._lockObject)
			{
				if (!FormsAuthentication._Initialized)
				{
					AuthenticationSection authentication = RuntimeConfig.GetAppConfig().Authentication;
					authentication.ValidateAuthenticationMode();
					FormsAuthentication._FormsName = authentication.Forms.Name;
					FormsAuthentication._RequireSSL = authentication.Forms.RequireSSL;
					FormsAuthentication._SlidingExpiration = authentication.Forms.SlidingExpiration;
					if (FormsAuthentication._FormsName == null)
					{
						FormsAuthentication._FormsName = ".ASPXAUTH";
					}
					FormsAuthentication._Protection = authentication.Forms.Protection;
					FormsAuthentication._Timeout = (int)authentication.Forms.Timeout.TotalMinutes;
					FormsAuthentication._FormsCookiePath = authentication.Forms.Path;
					FormsAuthentication._LoginUrl = authentication.Forms.LoginUrl;
					if (FormsAuthentication._LoginUrl == null)
					{
						FormsAuthentication._LoginUrl = "login.aspx";
					}
					FormsAuthentication._DefaultUrl = authentication.Forms.DefaultUrl;
					if (FormsAuthentication._DefaultUrl == null)
					{
						FormsAuthentication._DefaultUrl = "default.aspx";
					}
					FormsAuthentication._CookieMode = authentication.Forms.Cookieless;
					FormsAuthentication._CookieDomain = authentication.Forms.Domain;
					FormsAuthentication._EnableCrossAppRedirects = authentication.Forms.EnableCrossAppRedirects;
					FormsAuthentication._Initialized = true;
				}
			}
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x000B00BC File Offset: 0x000AF0BC
		public static FormsAuthenticationTicket Decrypt(string encryptedTicket)
		{
			if (string.IsNullOrEmpty(encryptedTicket) || encryptedTicket.Length > 4096)
			{
				throw new ArgumentException(SR.GetString("InvalidArgumentValue", new object[] { "encryptedTicket" }));
			}
			FormsAuthentication.Initialize();
			byte[] array = null;
			if (encryptedTicket.Length % 2 == 0)
			{
				try
				{
					array = MachineKeySection.HexStringToByteArray(encryptedTicket);
				}
				catch
				{
				}
			}
			if (array == null)
			{
				array = HttpServerUtility.UrlTokenDecode(encryptedTicket);
			}
			if (array == null || array.Length < 1)
			{
				throw new ArgumentException(SR.GetString("InvalidArgumentValue", new object[] { "encryptedTicket" }));
			}
			if (FormsAuthentication._Protection == FormsProtectionEnum.All || FormsAuthentication._Protection == FormsProtectionEnum.Encryption)
			{
				array = MachineKeySection.EncryptOrDecryptData(false, array, null, 0, array.Length, IVType.Random);
				if (array == null)
				{
					return null;
				}
			}
			int num = array.Length;
			if (FormsAuthentication._Protection == FormsProtectionEnum.All || FormsAuthentication._Protection == FormsProtectionEnum.Validation)
			{
				if (array.Length <= 20)
				{
					return null;
				}
				num -= 20;
				byte[] array2 = MachineKeySection.HashData(array, null, 0, num);
				if (array2 == null)
				{
					return null;
				}
				if (array2.Length != 20)
				{
					return null;
				}
				for (int i = 0; i < 20; i++)
				{
					if (array2[i] != array[num + i])
					{
						return null;
					}
				}
			}
			if (!AppSettings.UseLegacyFormsAuthenticationTicketCompatibility)
			{
				return FormsAuthenticationTicketSerializer.Deserialize(array, num);
			}
			int num2 = ((num > 4096) ? 4096 : num);
			StringBuilder stringBuilder = new StringBuilder(num2);
			StringBuilder stringBuilder2 = new StringBuilder(num2);
			StringBuilder stringBuilder3 = new StringBuilder(num2);
			byte[] array3 = new byte[2];
			long[] array4 = new long[2];
			int num3 = UnsafeNativeMethods.CookieAuthParseTicket(array, num, stringBuilder, num2, stringBuilder2, num2, stringBuilder3, num2, array3, array4);
			if (num3 != 0)
			{
				return null;
			}
			DateTime dateTime = DateTime.FromFileTime(array4[0]);
			DateTime dateTime2 = DateTime.FromFileTime(array4[1]);
			return new FormsAuthenticationTicket((int)array3[0], stringBuilder.ToString(), dateTime, dateTime2, array3[1] != 0, stringBuilder2.ToString(), stringBuilder3.ToString());
		}

		// Token: 0x0600281C RID: 10268 RVA: 0x000B0288 File Offset: 0x000AF288
		public static string Encrypt(FormsAuthenticationTicket ticket)
		{
			return FormsAuthentication.Encrypt(ticket, true);
		}

		// Token: 0x0600281D RID: 10269 RVA: 0x000B0294 File Offset: 0x000AF294
		private static string Encrypt(FormsAuthenticationTicket ticket, bool hexEncodedTicket)
		{
			if (ticket == null)
			{
				throw new ArgumentNullException("ticket");
			}
			FormsAuthentication.Initialize();
			byte[] array = FormsAuthentication.MakeTicketIntoBinaryBlob(ticket);
			if (array == null)
			{
				return null;
			}
			if (FormsAuthentication._Protection == FormsProtectionEnum.All || FormsAuthentication._Protection == FormsProtectionEnum.Validation)
			{
				byte[] array2 = MachineKeySection.HashData(array, null, 0, array.Length);
				if (array2 == null)
				{
					return null;
				}
				byte[] array3 = new byte[array2.Length + array.Length];
				Buffer.BlockCopy(array, 0, array3, 0, array.Length);
				Buffer.BlockCopy(array2, 0, array3, array.Length, array2.Length);
				array = array3;
			}
			if (FormsAuthentication._Protection == FormsProtectionEnum.All || FormsAuthentication._Protection == FormsProtectionEnum.Encryption)
			{
				array = MachineKeySection.EncryptOrDecryptData(true, array, null, 0, array.Length, IVType.Random);
			}
			if (!hexEncodedTicket)
			{
				return HttpServerUtility.UrlTokenEncode(array);
			}
			return MachineKeySection.ByteArrayToHexString(array, 0);
		}

		// Token: 0x0600281E RID: 10270 RVA: 0x000B0338 File Offset: 0x000AF338
		public static bool Authenticate(string name, string password)
		{
			bool flag = FormsAuthentication.InternalAuthenticate(name, password);
			if (flag)
			{
				PerfCounters.IncrementCounter(AppPerfCounter.FORMS_AUTH_SUCCESS);
				WebBaseEvent.RaiseSystemEvent(null, 4001, name);
			}
			else
			{
				PerfCounters.IncrementCounter(AppPerfCounter.FORMS_AUTH_FAIL);
				WebBaseEvent.RaiseSystemEvent(null, 4005, name);
			}
			return flag;
		}

		// Token: 0x0600281F RID: 10271 RVA: 0x000B037C File Offset: 0x000AF37C
		private static bool InternalAuthenticate(string name, string password)
		{
			if (name == null || password == null)
			{
				return false;
			}
			FormsAuthentication.Initialize();
			AuthenticationSection authentication = RuntimeConfig.GetAppConfig().Authentication;
			authentication.ValidateAuthenticationMode();
			FormsAuthenticationUserCollection users = authentication.Forms.Credentials.Users;
			if (users == null)
			{
				return false;
			}
			FormsAuthenticationUser formsAuthenticationUser = users[name.ToLower(CultureInfo.InvariantCulture)];
			if (formsAuthenticationUser == null)
			{
				return false;
			}
			string password2 = formsAuthenticationUser.Password;
			if (password2 == null)
			{
				return false;
			}
			string text;
			switch (authentication.Forms.Credentials.PasswordFormat)
			{
			case FormsAuthPasswordFormat.Clear:
				text = password;
				break;
			case FormsAuthPasswordFormat.SHA1:
				text = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");
				break;
			case FormsAuthPasswordFormat.MD5:
				text = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "md5");
				break;
			default:
				return false;
			}
			return string.Compare(text, password2, (authentication.Forms.Credentials.PasswordFormat != FormsAuthPasswordFormat.Clear) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) == 0;
		}

		// Token: 0x06002820 RID: 10272 RVA: 0x000B0450 File Offset: 0x000AF450
		public static void SignOut()
		{
			FormsAuthentication.Initialize();
			HttpContext httpContext = HttpContext.Current;
			bool flag = httpContext.CookielessHelper.DoesCookieValueExistInOriginal('F');
			httpContext.CookielessHelper.SetCookieValue('F', null);
			if (!CookielessHelperClass.UseCookieless(httpContext, false, FormsAuthentication.CookieMode) || httpContext.Request.Browser.Cookies)
			{
				string text = string.Empty;
				if (httpContext.Request.Browser["supportsEmptyStringInCookieValue"] == "false")
				{
					text = "NoCookie";
				}
				HttpCookie httpCookie = new HttpCookie(FormsAuthentication.FormsCookieName, text);
				httpCookie.HttpOnly = true;
				httpCookie.Path = FormsAuthentication._FormsCookiePath;
				httpCookie.Expires = new DateTime(1999, 10, 12);
				httpCookie.Secure = FormsAuthentication._RequireSSL;
				if (FormsAuthentication._CookieDomain != null)
				{
					httpCookie.Domain = FormsAuthentication._CookieDomain;
				}
				httpContext.Response.Cookies.RemoveCookie(FormsAuthentication.FormsCookieName);
				httpContext.Response.Cookies.Add(httpCookie);
			}
			if (flag)
			{
				httpContext.Response.Redirect(FormsAuthentication.GetLoginPage(null), false);
			}
		}

		// Token: 0x06002821 RID: 10273 RVA: 0x000B055E File Offset: 0x000AF55E
		public static void SetAuthCookie(string userName, bool createPersistentCookie)
		{
			FormsAuthentication.Initialize();
			FormsAuthentication.SetAuthCookie(userName, createPersistentCookie, FormsAuthentication.FormsCookiePath);
		}

		// Token: 0x06002822 RID: 10274 RVA: 0x000B0574 File Offset: 0x000AF574
		public static void SetAuthCookie(string userName, bool createPersistentCookie, string strCookiePath)
		{
			FormsAuthentication.Initialize();
			HttpContext httpContext = HttpContext.Current;
			if (!httpContext.Request.IsSecureConnection && FormsAuthentication.RequireSSL)
			{
				throw new HttpException(SR.GetString("Connection_not_secure_creating_secure_cookie"));
			}
			bool flag = CookielessHelperClass.UseCookieless(httpContext, false, FormsAuthentication.CookieMode);
			HttpCookie authCookie = FormsAuthentication.GetAuthCookie(userName, createPersistentCookie, flag ? "/" : strCookiePath, !flag);
			if (!flag)
			{
				HttpContext.Current.Response.Cookies.Add(authCookie);
				httpContext.CookielessHelper.SetCookieValue('F', null);
				return;
			}
			httpContext.CookielessHelper.SetCookieValue('F', authCookie.Value);
		}

		// Token: 0x06002823 RID: 10275 RVA: 0x000B060E File Offset: 0x000AF60E
		public static HttpCookie GetAuthCookie(string userName, bool createPersistentCookie)
		{
			FormsAuthentication.Initialize();
			return FormsAuthentication.GetAuthCookie(userName, createPersistentCookie, FormsAuthentication.FormsCookiePath);
		}

		// Token: 0x06002824 RID: 10276 RVA: 0x000B0621 File Offset: 0x000AF621
		public static HttpCookie GetAuthCookie(string userName, bool createPersistentCookie, string strCookiePath)
		{
			return FormsAuthentication.GetAuthCookie(userName, createPersistentCookie, strCookiePath, true);
		}

		// Token: 0x06002825 RID: 10277 RVA: 0x000B062C File Offset: 0x000AF62C
		private static HttpCookie GetAuthCookie(string userName, bool createPersistentCookie, string strCookiePath, bool hexEncodedTicket)
		{
			FormsAuthentication.Initialize();
			if (userName == null)
			{
				userName = string.Empty;
			}
			if (strCookiePath == null || strCookiePath.Length < 1)
			{
				strCookiePath = FormsAuthentication.FormsCookiePath;
			}
			DateTime utcNow = DateTime.UtcNow;
			DateTime dateTime = utcNow.AddMinutes((double)FormsAuthentication._Timeout);
			FormsAuthenticationTicket formsAuthenticationTicket = FormsAuthenticationTicket.FromUtc(2, userName, utcNow, dateTime, createPersistentCookie, string.Empty, strCookiePath);
			string text = FormsAuthentication.Encrypt(formsAuthenticationTicket, hexEncodedTicket);
			if (text == null || text.Length < 1)
			{
				throw new HttpException(SR.GetString("Unable_to_encrypt_cookie_ticket"));
			}
			HttpCookie httpCookie = new HttpCookie(FormsAuthentication.FormsCookieName, text);
			httpCookie.HttpOnly = true;
			httpCookie.Path = strCookiePath;
			httpCookie.Secure = FormsAuthentication._RequireSSL;
			if (FormsAuthentication._CookieDomain != null)
			{
				httpCookie.Domain = FormsAuthentication._CookieDomain;
			}
			if (formsAuthenticationTicket.IsPersistent)
			{
				httpCookie.Expires = formsAuthenticationTicket.Expiration;
			}
			return httpCookie;
		}

		// Token: 0x06002826 RID: 10278 RVA: 0x000B06F8 File Offset: 0x000AF6F8
		internal static string GetReturnUrl(bool useDefaultIfAbsent)
		{
			FormsAuthentication.Initialize();
			HttpContext httpContext = HttpContext.Current;
			string text = httpContext.Request.QueryString[FormsAuthentication.ReturnUrlVar];
			if (text == null)
			{
				text = httpContext.Request.Form[FormsAuthentication.ReturnUrlVar];
				if (!string.IsNullOrEmpty(text) && !text.Contains("/") && text.Contains("%"))
				{
					text = HttpUtility.UrlDecode(text);
				}
			}
			if (!string.IsNullOrEmpty(text) && !FormsAuthentication.EnableCrossAppRedirects && !UrlPath.IsPathOnSameServer(text, httpContext.Request.Url))
			{
				text = null;
			}
			if (!string.IsNullOrEmpty(text) && CrossSiteScriptingValidation.IsDangerousUrl(text))
			{
				throw new HttpException(SR.GetString("Invalid_redirect_return_url"));
			}
			if (text != null || !useDefaultIfAbsent)
			{
				return text;
			}
			return FormsAuthentication.DefaultUrl;
		}

		// Token: 0x06002827 RID: 10279 RVA: 0x000B07B9 File Offset: 0x000AF7B9
		public static string GetRedirectUrl(string userName, bool createPersistentCookie)
		{
			if (userName == null)
			{
				return null;
			}
			return FormsAuthentication.GetReturnUrl(true);
		}

		// Token: 0x06002828 RID: 10280 RVA: 0x000B07C6 File Offset: 0x000AF7C6
		public static void RedirectFromLoginPage(string userName, bool createPersistentCookie)
		{
			FormsAuthentication.Initialize();
			FormsAuthentication.RedirectFromLoginPage(userName, createPersistentCookie, FormsAuthentication.FormsCookiePath);
		}

		// Token: 0x06002829 RID: 10281 RVA: 0x000B07DC File Offset: 0x000AF7DC
		public static void RedirectFromLoginPage(string userName, bool createPersistentCookie, string strCookiePath)
		{
			FormsAuthentication.Initialize();
			if (userName == null)
			{
				return;
			}
			HttpContext httpContext = HttpContext.Current;
			string text = FormsAuthentication.GetReturnUrl(true);
			if (FormsAuthentication.CookiesSupported || FormsAuthentication.IsPathWithinAppRoot(httpContext, text))
			{
				FormsAuthentication.SetAuthCookie(userName, createPersistentCookie, strCookiePath);
				text = FormsAuthentication.RemoveQueryStringVariableFromUrl(text, FormsAuthentication.FormsCookieName);
				if (!FormsAuthentication.CookiesSupported)
				{
					int num = text.IndexOf("://", StringComparison.Ordinal);
					if (num > 0)
					{
						num = text.IndexOf('/', num + 3);
						if (num > 0)
						{
							text = text.Substring(num);
						}
					}
				}
			}
			else
			{
				if (!FormsAuthentication.EnableCrossAppRedirects)
				{
					throw new HttpException(SR.GetString("Can_not_issue_cookie_or_redirect"));
				}
				HttpCookie authCookie = FormsAuthentication.GetAuthCookie(userName, createPersistentCookie, strCookiePath);
				text = FormsAuthentication.RemoveQueryStringVariableFromUrl(text, authCookie.Name);
				if (text.IndexOf('?') > 0)
				{
					string text2 = text;
					text = string.Concat(new string[] { text2, "&", authCookie.Name, "=", authCookie.Value });
				}
				else
				{
					string text3 = text;
					text = string.Concat(new string[] { text3, "?", authCookie.Name, "=", authCookie.Value });
				}
			}
			httpContext.Response.Redirect(text, false);
		}

		// Token: 0x0600282A RID: 10282 RVA: 0x000B092C File Offset: 0x000AF92C
		public static FormsAuthenticationTicket RenewTicketIfOld(FormsAuthenticationTicket tOld)
		{
			if (tOld == null)
			{
				return null;
			}
			DateTime utcNow = DateTime.UtcNow;
			TimeSpan timeSpan = utcNow - tOld.IssueDateUtc;
			TimeSpan timeSpan2 = tOld.ExpirationUtc - utcNow;
			if (timeSpan2 > timeSpan)
			{
				return tOld;
			}
			TimeSpan timeSpan3 = tOld.ExpirationUtc - tOld.IssueDateUtc;
			DateTime dateTime = utcNow + timeSpan3;
			return FormsAuthenticationTicket.FromUtc(tOld.Version, tOld.Name, utcNow, dateTime, tOld.IsPersistent, tOld.UserData, tOld.CookiePath);
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x0600282B RID: 10283 RVA: 0x000B09AE File Offset: 0x000AF9AE
		public static string FormsCookieName
		{
			get
			{
				FormsAuthentication.Initialize();
				return FormsAuthentication._FormsName;
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x0600282C RID: 10284 RVA: 0x000B09BA File Offset: 0x000AF9BA
		public static string FormsCookiePath
		{
			get
			{
				FormsAuthentication.Initialize();
				return FormsAuthentication._FormsCookiePath;
			}
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x0600282D RID: 10285 RVA: 0x000B09C6 File Offset: 0x000AF9C6
		public static bool RequireSSL
		{
			get
			{
				FormsAuthentication.Initialize();
				return FormsAuthentication._RequireSSL;
			}
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x0600282E RID: 10286 RVA: 0x000B09D2 File Offset: 0x000AF9D2
		public static bool SlidingExpiration
		{
			get
			{
				FormsAuthentication.Initialize();
				return FormsAuthentication._SlidingExpiration;
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x0600282F RID: 10287 RVA: 0x000B09DE File Offset: 0x000AF9DE
		public static HttpCookieMode CookieMode
		{
			get
			{
				FormsAuthentication.Initialize();
				return FormsAuthentication._CookieMode;
			}
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06002830 RID: 10288 RVA: 0x000B09EA File Offset: 0x000AF9EA
		public static string CookieDomain
		{
			get
			{
				FormsAuthentication.Initialize();
				return FormsAuthentication._CookieDomain;
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06002831 RID: 10289 RVA: 0x000B09F6 File Offset: 0x000AF9F6
		public static bool EnableCrossAppRedirects
		{
			get
			{
				FormsAuthentication.Initialize();
				return FormsAuthentication._EnableCrossAppRedirects;
			}
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x06002832 RID: 10290 RVA: 0x000B0A04 File Offset: 0x000AFA04
		public static bool CookiesSupported
		{
			get
			{
				HttpContext httpContext = HttpContext.Current;
				return httpContext == null || !CookielessHelperClass.UseCookieless(httpContext, false, FormsAuthentication.CookieMode);
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06002833 RID: 10291 RVA: 0x000B0A2C File Offset: 0x000AFA2C
		public static string LoginUrl
		{
			get
			{
				FormsAuthentication.Initialize();
				HttpContext httpContext = HttpContext.Current;
				if (httpContext != null)
				{
					return AuthenticationConfig.GetCompleteLoginUrl(httpContext, FormsAuthentication._LoginUrl);
				}
				if (FormsAuthentication._LoginUrl.Length == 0 || (FormsAuthentication._LoginUrl[0] != '/' && FormsAuthentication._LoginUrl.IndexOf("//", StringComparison.Ordinal) < 0))
				{
					return "/" + FormsAuthentication._LoginUrl;
				}
				return FormsAuthentication._LoginUrl;
			}
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06002834 RID: 10292 RVA: 0x000B0A98 File Offset: 0x000AFA98
		public static string DefaultUrl
		{
			get
			{
				FormsAuthentication.Initialize();
				HttpContext httpContext = HttpContext.Current;
				if (httpContext != null)
				{
					return AuthenticationConfig.GetCompleteLoginUrl(httpContext, FormsAuthentication._DefaultUrl);
				}
				if (FormsAuthentication._DefaultUrl.Length == 0 || (FormsAuthentication._DefaultUrl[0] != '/' && FormsAuthentication._DefaultUrl.IndexOf("//", StringComparison.Ordinal) < 0))
				{
					return "/" + FormsAuthentication._DefaultUrl;
				}
				return FormsAuthentication._DefaultUrl;
			}
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06002835 RID: 10293 RVA: 0x000B0B02 File Offset: 0x000AFB02
		internal static string ReturnUrlVar
		{
			get
			{
				if (!string.IsNullOrEmpty(AppSettings.FormsAuthReturnUrlVar))
				{
					return AppSettings.FormsAuthReturnUrlVar;
				}
				return "ReturnUrl";
			}
		}

		// Token: 0x06002836 RID: 10294 RVA: 0x000B0B1B File Offset: 0x000AFB1B
		internal static string GetLoginPage(string extraQueryString)
		{
			return FormsAuthentication.GetLoginPage(extraQueryString, false);
		}

		// Token: 0x06002837 RID: 10295 RVA: 0x000B0B24 File Offset: 0x000AFB24
		internal static string GetLoginPage(string extraQueryString, bool reuseReturnUrl)
		{
			HttpContext httpContext = HttpContext.Current;
			string text = FormsAuthentication.LoginUrl;
			if (text.IndexOf('?') >= 0)
			{
				text = FormsAuthentication.RemoveQueryStringVariableFromUrl(text, FormsAuthentication.ReturnUrlVar);
			}
			int num = text.IndexOf('?');
			if (num < 0)
			{
				text += "?";
			}
			else if (num < text.Length - 1)
			{
				text += "&";
			}
			string text2 = null;
			if (reuseReturnUrl)
			{
				text2 = HttpUtility.UrlEncode(FormsAuthentication.GetReturnUrl(false), httpContext.Request.QueryStringEncoding);
			}
			if (text2 == null)
			{
				text2 = HttpUtility.UrlEncode(httpContext.Request.PathWithQueryString, httpContext.Request.ContentEncoding);
			}
			text = text + FormsAuthentication.ReturnUrlVar + "=" + text2;
			if (!string.IsNullOrEmpty(extraQueryString))
			{
				text = text + "&" + extraQueryString;
			}
			return text;
		}

		// Token: 0x06002838 RID: 10296 RVA: 0x000B0BE9 File Offset: 0x000AFBE9
		public static void RedirectToLoginPage()
		{
			FormsAuthentication.RedirectToLoginPage(null);
		}

		// Token: 0x06002839 RID: 10297 RVA: 0x000B0BF4 File Offset: 0x000AFBF4
		public static void RedirectToLoginPage(string extraQueryString)
		{
			HttpContext httpContext = HttpContext.Current;
			string loginPage = FormsAuthentication.GetLoginPage(extraQueryString);
			httpContext.Response.Redirect(loginPage, false);
		}

		// Token: 0x0600283A RID: 10298 RVA: 0x000B0C1C File Offset: 0x000AFC1C
		private static byte[] MakeTicketIntoBinaryBlob(FormsAuthenticationTicket ticket)
		{
			if (ticket.Name == null || ticket.UserData == null || ticket.CookiePath == null)
			{
				return null;
			}
			if (!AppSettings.UseLegacyFormsAuthenticationTicketCompatibility)
			{
				return FormsAuthenticationTicketSerializer.Serialize(ticket);
			}
			byte[] array = new byte[4096];
			byte[] array2 = new byte[2];
			long[] array3 = new long[2];
			bool flag = FormsAuthentication._Protection == FormsProtectionEnum.All || FormsAuthentication._Protection == FormsProtectionEnum.Encryption;
			bool flag2 = !flag || MachineKeySection.CompatMode == MachineKeyCompatibilityMode.Framework20SP1;
			if (flag2)
			{
				byte[] array4 = new byte[8];
				RNGCryptoServiceProvider rngcryptoServiceProvider = new RNGCryptoServiceProvider();
				rngcryptoServiceProvider.GetBytes(array4);
				Buffer.BlockCopy(array4, 0, array, 0, 8);
			}
			array2[0] = (byte)ticket.Version;
			array2[1] = (ticket.IsPersistent ? 1 : 0);
			array3[0] = ticket.IssueDate.ToFileTime();
			array3[1] = ticket.Expiration.ToFileTime();
			int num = UnsafeNativeMethods.CookieAuthConstructTicket(array, array.Length, ticket.Name, ticket.UserData, ticket.CookiePath, array2, array3);
			if (num < 0)
			{
				return null;
			}
			byte[] array5 = new byte[num];
			Buffer.BlockCopy(array, 0, array5, 0, num);
			return array5;
		}

		// Token: 0x0600283B RID: 10299 RVA: 0x000B0D30 File Offset: 0x000AFD30
		internal static string RemoveQueryStringVariableFromUrl(string strUrl, string QSVar)
		{
			int num = strUrl.IndexOf('?');
			if (num < 0)
			{
				return strUrl;
			}
			string text = "&";
			string text2 = "?";
			string text3 = text + QSVar + "=";
			FormsAuthentication.RemoveQSVar(ref strUrl, num, text3, text, text.Length);
			text3 = text2 + QSVar + "=";
			FormsAuthentication.RemoveQSVar(ref strUrl, num, text3, text, text2.Length);
			text = HttpUtility.UrlEncode("&");
			text2 = HttpUtility.UrlEncode("?");
			text3 = text + HttpUtility.UrlEncode(QSVar + "=");
			FormsAuthentication.RemoveQSVar(ref strUrl, num, text3, text, text.Length);
			text3 = text2 + HttpUtility.UrlEncode(QSVar + "=");
			FormsAuthentication.RemoveQSVar(ref strUrl, num, text3, text, text2.Length);
			return strUrl;
		}

		// Token: 0x0600283C RID: 10300 RVA: 0x000B0DF8 File Offset: 0x000AFDF8
		private static void RemoveQSVar(ref string strUrl, int posQ, string token, string sep, int lenAtStartToLeave)
		{
			for (int i = strUrl.LastIndexOf(token, StringComparison.Ordinal); i >= posQ; i = strUrl.LastIndexOf(token, StringComparison.Ordinal))
			{
				int num = strUrl.IndexOf(sep, i + token.Length, StringComparison.Ordinal) + sep.Length;
				if (num < sep.Length || num >= strUrl.Length)
				{
					strUrl = strUrl.Substring(0, i);
				}
				else
				{
					strUrl = strUrl.Substring(0, i + lenAtStartToLeave) + strUrl.Substring(num);
				}
			}
		}

		// Token: 0x0600283D RID: 10301 RVA: 0x000B0E74 File Offset: 0x000AFE74
		private static bool IsPathWithinAppRoot(HttpContext context, string path)
		{
			Uri uri;
			if (!Uri.TryCreate(path, UriKind.Absolute, out uri))
			{
				return HttpRuntime.IsPathWithinAppRoot(path);
			}
			return (uri.IsLoopback || string.Equals(context.Request.Url.Host, uri.Host, StringComparison.OrdinalIgnoreCase)) && HttpRuntime.IsPathWithinAppRoot(uri.AbsolutePath);
		}

		// Token: 0x04001E8B RID: 7819
		private const int MAC_LENGTH = 20;

		// Token: 0x04001E8C RID: 7820
		private const int MAX_TICKET_LENGTH = 4096;

		// Token: 0x04001E8D RID: 7821
		private const string CONFIG_DEFAULT_COOKIE = ".ASPXAUTH";

		// Token: 0x04001E8E RID: 7822
		private static object _lockObject = new object();

		// Token: 0x04001E8F RID: 7823
		private static bool _Initialized;

		// Token: 0x04001E90 RID: 7824
		private static string _FormsName;

		// Token: 0x04001E91 RID: 7825
		private static FormsProtectionEnum _Protection;

		// Token: 0x04001E92 RID: 7826
		private static int _Timeout;

		// Token: 0x04001E93 RID: 7827
		private static string _FormsCookiePath;

		// Token: 0x04001E94 RID: 7828
		private static bool _RequireSSL;

		// Token: 0x04001E95 RID: 7829
		private static bool _SlidingExpiration;

		// Token: 0x04001E96 RID: 7830
		private static string _LoginUrl;

		// Token: 0x04001E97 RID: 7831
		private static string _DefaultUrl;

		// Token: 0x04001E98 RID: 7832
		private static HttpCookieMode _CookieMode;

		// Token: 0x04001E99 RID: 7833
		private static string _CookieDomain = null;

		// Token: 0x04001E9A RID: 7834
		private static bool _EnableCrossAppRedirects;
	}
}
