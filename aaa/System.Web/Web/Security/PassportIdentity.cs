using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Web.Util;

namespace System.Web.Security
{
	// Token: 0x0200034D RID: 845
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class PassportIdentity : IIdentity, IDisposable
	{
		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x060028E0 RID: 10464 RVA: 0x000B2FD9 File Offset: 0x000B1FD9
		internal bool WWWAuthHeaderSet
		{
			get
			{
				return this._WWWAuthHeaderSet;
			}
		}

		// Token: 0x060028E1 RID: 10465 RVA: 0x000B2FE4 File Offset: 0x000B1FE4
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		public PassportIdentity()
		{
			HttpContext httpContext = HttpContext.Current;
			if (PassportIdentity._iPassportVer == 0)
			{
				PassportIdentity._iPassportVer = UnsafeNativeMethods.PassportVersion();
			}
			if (PassportIdentity._iPassportVer < 3)
			{
				string text = httpContext.Request.QueryString["t"];
				string text2 = httpContext.Request.QueryString["p"];
				HttpCookie httpCookie = httpContext.Request.Cookies["MSPAuth"];
				HttpCookie httpCookie2 = httpContext.Request.Cookies["MSPProf"];
				HttpCookie httpCookie3 = httpContext.Request.Cookies["MSPProfC"];
				string text3 = ((httpCookie != null && httpCookie.Value != null) ? httpCookie.Value : string.Empty);
				string text4 = ((httpCookie2 != null && httpCookie2.Value != null) ? httpCookie2.Value : string.Empty);
				string text5 = ((httpCookie3 != null && httpCookie3.Value != null) ? httpCookie3.Value : string.Empty);
				StringBuilder stringBuilder = new StringBuilder(1028);
				StringBuilder stringBuilder2 = new StringBuilder(1028);
				text3 = HttpUtility.UrlDecode(text3);
				text4 = HttpUtility.UrlDecode(text4);
				text5 = HttpUtility.UrlDecode(text5);
				int num = UnsafeNativeMethods.PassportCreate(text, text2, text3, text4, text5, stringBuilder, stringBuilder2, 1024, ref this._iPassport);
				if (this._iPassport == IntPtr.Zero)
				{
					throw new COMException(SR.GetString("Could_not_create_passport_identity"), num);
				}
				string text6 = PassportIdentity.UrlEncodeCookie(stringBuilder.ToString());
				string text7 = PassportIdentity.UrlEncodeCookie(stringBuilder2.ToString());
				if (text6.Length > 1)
				{
					httpContext.Response.AppendHeader("Set-Cookie", text6);
				}
				if (text7.Length > 1)
				{
					httpContext.Response.AppendHeader("Set-Cookie", text7);
				}
			}
			else
			{
				string text8 = string.Concat(new string[]
				{
					httpContext.Request.HttpMethod,
					" ",
					httpContext.Request.RawUrl,
					" ",
					httpContext.Request.ServerVariables["SERVER_PROTOCOL"],
					"\r\n"
				});
				StringBuilder stringBuilder3 = new StringBuilder(4092);
				int num2 = UnsafeNativeMethods.PassportCreateHttpRaw(text8, httpContext.Request.ServerVariables["ALL_RAW"], httpContext.Request.IsSecureConnection ? 1 : 0, stringBuilder3, 4090, ref this._iPassport);
				if (this._iPassport == IntPtr.Zero)
				{
					throw new COMException(SR.GetString("Could_not_create_passport_identity"), num2);
				}
				string text9 = stringBuilder3.ToString();
				this.SetHeaders(httpContext, text9);
			}
			this._Authenticated = this.GetIsAuthenticated(-1, -1, -1);
			if (!this._Authenticated)
			{
				this._Name = string.Empty;
			}
		}

		// Token: 0x060028E2 RID: 10466 RVA: 0x000B32B4 File Offset: 0x000B22B4
		private void SetHeaders(HttpContext context, string strResponseHeaders)
		{
			int num;
			for (int i = 0; i < strResponseHeaders.Length; i = num + 2)
			{
				num = strResponseHeaders.IndexOf('\r', i);
				if (num < 0)
				{
					num = strResponseHeaders.Length;
				}
				string text = strResponseHeaders.Substring(i, num - i);
				int num2 = text.IndexOf(':');
				if (num2 > 0)
				{
					string text2 = text.Substring(0, num2);
					string text3 = text.Substring(num2 + 1);
					context.Response.AppendHeader(text2, text3);
				}
			}
		}

		// Token: 0x060028E3 RID: 10467 RVA: 0x000B3324 File Offset: 0x000B2324
		~PassportIdentity()
		{
			UnsafeNativeMethods.PassportDestroy(this._iPassport);
			this._iPassport = IntPtr.Zero;
		}

		// Token: 0x060028E4 RID: 10468 RVA: 0x000B3360 File Offset: 0x000B2360
		private static string UrlEncodeCookie(string strIn)
		{
			if (strIn == null || strIn.Length < 1)
			{
				return string.Empty;
			}
			int num = strIn.IndexOf('=');
			if (num < 0)
			{
				return HttpUtility.AspCompatUrlEncode(strIn);
			}
			num++;
			int num2 = strIn.IndexOf(';', num);
			if (num2 < 0)
			{
				return HttpUtility.AspCompatUrlEncode(strIn);
			}
			string text = strIn.Substring(0, num);
			string text2 = strIn.Substring(num, num2 - num);
			string text3 = strIn.Substring(num2, strIn.Length - num2);
			return text + HttpUtility.AspCompatUrlEncode(text2) + text3;
		}

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x060028E5 RID: 10469 RVA: 0x000B33E0 File Offset: 0x000B23E0
		public string Name
		{
			get
			{
				if (this._Name == null)
				{
					if (PassportIdentity._iPassportVer >= 3)
					{
						this._Name = this.HexPUID;
					}
					else if (this.HasProfile("core"))
					{
						this._Name = int.Parse(this["MemberIDHigh"], CultureInfo.InvariantCulture).ToString("X8", CultureInfo.InvariantCulture) + int.Parse(this["MemberIDLow"], CultureInfo.InvariantCulture).ToString("X8", CultureInfo.InvariantCulture);
					}
					else
					{
						this._Name = string.Empty;
					}
				}
				return this._Name;
			}
		}

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x060028E6 RID: 10470 RVA: 0x000B3487 File Offset: 0x000B2487
		public string AuthenticationType
		{
			get
			{
				return "Passport";
			}
		}

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x060028E7 RID: 10471 RVA: 0x000B348E File Offset: 0x000B248E
		public bool IsAuthenticated
		{
			get
			{
				return this._Authenticated;
			}
		}

		// Token: 0x170008B1 RID: 2225
		public string this[string strProfileName]
		{
			get
			{
				object profileObject = this.GetProfileObject(strProfileName);
				if (profileObject == null)
				{
					return string.Empty;
				}
				if (profileObject is string)
				{
					return (string)profileObject;
				}
				return profileObject.ToString();
			}
		}

		// Token: 0x060028E9 RID: 10473 RVA: 0x000B34CB File Offset: 0x000B24CB
		public bool GetIsAuthenticated(int iTimeWindow, bool bForceLogin, bool bCheckSecure)
		{
			return this.GetIsAuthenticated(iTimeWindow, bForceLogin ? 1 : 0, bCheckSecure ? 10 : 0);
		}

		// Token: 0x060028EA RID: 10474 RVA: 0x000B34E4 File Offset: 0x000B24E4
		public bool GetIsAuthenticated(int iTimeWindow, int iForceLogin, int iCheckSecure)
		{
			int num = UnsafeNativeMethods.PassportIsAuthenticated(this._iPassport, iTimeWindow, iForceLogin, iCheckSecure);
			if (num < 0)
			{
				throw new COMException(SR.GetString("Passport_method_failed"), num);
			}
			return num == 0;
		}

		// Token: 0x060028EB RID: 10475 RVA: 0x000B351C File Offset: 0x000B251C
		public object GetProfileObject(string strProfileName)
		{
			object obj = new object();
			int num = UnsafeNativeMethods.PassportGetProfile(this._iPassport, strProfileName, out obj);
			if (num < 0)
			{
				throw new COMException(SR.GetString("Passport_method_failed"), num);
			}
			return obj;
		}

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x060028EC RID: 10476 RVA: 0x000B3554 File Offset: 0x000B2554
		public int Error
		{
			get
			{
				return UnsafeNativeMethods.PassportGetError(this._iPassport);
			}
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x060028ED RID: 10477 RVA: 0x000B3564 File Offset: 0x000B2564
		public bool GetFromNetworkServer
		{
			get
			{
				int num = UnsafeNativeMethods.PassportGetFromNetworkServer(this._iPassport);
				if (num < 0)
				{
					throw new COMException(SR.GetString("Passport_method_failed"), num);
				}
				return num == 0;
			}
		}

		// Token: 0x060028EE RID: 10478 RVA: 0x000B3598 File Offset: 0x000B2598
		public string GetDomainFromMemberName(string strMemberName)
		{
			StringBuilder stringBuilder = new StringBuilder(1028);
			int num = UnsafeNativeMethods.PassportDomainFromMemberName(this._iPassport, strMemberName, stringBuilder, 1024);
			if (num < 0)
			{
				throw new COMException(SR.GetString("Passport_method_failed"), num);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060028EF RID: 10479 RVA: 0x000B35E0 File Offset: 0x000B25E0
		public bool HasProfile(string strProfile)
		{
			int num = UnsafeNativeMethods.PassportHasProfile(this._iPassport, strProfile);
			if (num < 0)
			{
				throw new COMException(SR.GetString("Passport_method_failed"), num);
			}
			return num == 0;
		}

		// Token: 0x060028F0 RID: 10480 RVA: 0x000B3614 File Offset: 0x000B2614
		public bool HasFlag(int iFlagMask)
		{
			int num = UnsafeNativeMethods.PassportHasFlag(this._iPassport, iFlagMask);
			if (num < 0)
			{
				throw new COMException(SR.GetString("Passport_method_failed"), num);
			}
			return num == 0;
		}

		// Token: 0x060028F1 RID: 10481 RVA: 0x000B3648 File Offset: 0x000B2648
		public bool HaveConsent(bool bNeedFullConsent, bool bNeedBirthdate)
		{
			int num = UnsafeNativeMethods.PassportHasConsent(this._iPassport, bNeedFullConsent ? 1 : 0, bNeedBirthdate ? 1 : 0);
			if (num < 0)
			{
				throw new COMException(SR.GetString("Passport_method_failed"), num);
			}
			return num == 0;
		}

		// Token: 0x060028F2 RID: 10482 RVA: 0x000B3688 File Offset: 0x000B2688
		public object GetOption(string strOpt)
		{
			object obj = new object();
			int num = UnsafeNativeMethods.PassportGetOption(this._iPassport, strOpt, out obj);
			if (num < 0)
			{
				throw new COMException(SR.GetString("Passport_method_failed"), num);
			}
			return obj;
		}

		// Token: 0x060028F3 RID: 10483 RVA: 0x000B36C0 File Offset: 0x000B26C0
		public void SetOption(string strOpt, object vOpt)
		{
			int num = UnsafeNativeMethods.PassportSetOption(this._iPassport, strOpt, vOpt);
			if (num < 0)
			{
				throw new COMException(SR.GetString("Passport_method_failed"), num);
			}
		}

		// Token: 0x060028F4 RID: 10484 RVA: 0x000B36F0 File Offset: 0x000B26F0
		public string LogoutURL()
		{
			return this.LogoutURL(null, null, -1, null, -1);
		}

		// Token: 0x060028F5 RID: 10485 RVA: 0x000B3700 File Offset: 0x000B2700
		public string LogoutURL(string szReturnURL, string szCOBrandArgs, int iLangID, string strDomain, int iUseSecureAuth)
		{
			StringBuilder stringBuilder = new StringBuilder(4096);
			int num = UnsafeNativeMethods.PassportLogoutURL(this._iPassport, szReturnURL, szCOBrandArgs, iLangID, strDomain, iUseSecureAuth, stringBuilder, 4096);
			if (num < 0)
			{
				throw new COMException(SR.GetString("Passport_method_failed"), num);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x060028F6 RID: 10486 RVA: 0x000B374C File Offset: 0x000B274C
		public bool HasSavedPassword
		{
			get
			{
				int num = UnsafeNativeMethods.PassportGetHasSavedPassword(this._iPassport);
				if (num < 0)
				{
					throw new COMException(SR.GetString("Passport_method_failed"), num);
				}
				return num == 0;
			}
		}

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x060028F7 RID: 10487 RVA: 0x000B3780 File Offset: 0x000B2780
		public bool HasTicket
		{
			get
			{
				int num = UnsafeNativeMethods.PassportHasTicket(this._iPassport);
				if (num < 0)
				{
					throw new COMException(SR.GetString("Passport_method_failed"), num);
				}
				return num == 0;
			}
		}

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x060028F8 RID: 10488 RVA: 0x000B37B4 File Offset: 0x000B27B4
		public int TicketAge
		{
			get
			{
				int num = UnsafeNativeMethods.PassportGetTicketAge(this._iPassport);
				if (num < 0)
				{
					throw new COMException(SR.GetString("Passport_method_failed"), num);
				}
				return num;
			}
		}

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x060028F9 RID: 10489 RVA: 0x000B37E4 File Offset: 0x000B27E4
		public int TimeSinceSignIn
		{
			get
			{
				int num = UnsafeNativeMethods.PassportGetTimeSinceSignIn(this._iPassport);
				if (num < 0)
				{
					throw new COMException(SR.GetString("Passport_method_failed"), num);
				}
				return num;
			}
		}

		// Token: 0x060028FA RID: 10490 RVA: 0x000B3814 File Offset: 0x000B2814
		public string LogoTag()
		{
			return this.LogoTag(null, -1, -1, null, -1, -1, null, -1, -1);
		}

		// Token: 0x060028FB RID: 10491 RVA: 0x000B3830 File Offset: 0x000B2830
		public string LogoTag(string strReturnUrl)
		{
			return this.LogoTag(strReturnUrl, -1, -1, null, -1, -1, null, -1, -1);
		}

		// Token: 0x060028FC RID: 10492 RVA: 0x000B384C File Offset: 0x000B284C
		public string LogoTag2()
		{
			return this.LogoTag2(null, -1, -1, null, -1, -1, null, -1, -1);
		}

		// Token: 0x060028FD RID: 10493 RVA: 0x000B3868 File Offset: 0x000B2868
		public string LogoTag2(string strReturnUrl)
		{
			return this.LogoTag2(strReturnUrl, -1, -1, null, -1, -1, null, -1, -1);
		}

		// Token: 0x060028FE RID: 10494 RVA: 0x000B3884 File Offset: 0x000B2884
		public string LogoTag(string strReturnUrl, int iTimeWindow, bool fForceLogin, string strCoBrandedArgs, int iLangID, bool fSecure, string strNameSpace, int iKPP, bool bUseSecureAuth)
		{
			return this.LogoTag(strReturnUrl, iTimeWindow, fForceLogin ? 1 : 0, strCoBrandedArgs, iLangID, fSecure ? 1 : 0, strNameSpace, iKPP, bUseSecureAuth ? 10 : 0);
		}

		// Token: 0x060028FF RID: 10495 RVA: 0x000B38BC File Offset: 0x000B28BC
		public string LogoTag(string strReturnUrl, int iTimeWindow, int iForceLogin, string strCoBrandedArgs, int iLangID, int iSecure, string strNameSpace, int iKPP, int iUseSecureAuth)
		{
			StringBuilder stringBuilder = new StringBuilder(4092);
			int num = UnsafeNativeMethods.PassportLogoTag(this._iPassport, strReturnUrl, iTimeWindow, iForceLogin, strCoBrandedArgs, iLangID, iSecure, strNameSpace, iKPP, iUseSecureAuth, stringBuilder, 4090);
			if (num < 0)
			{
				throw new COMException(SR.GetString("Passport_method_failed"), num);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002900 RID: 10496 RVA: 0x000B3910 File Offset: 0x000B2910
		public string LogoTag2(string strReturnUrl, int iTimeWindow, bool fForceLogin, string strCoBrandedArgs, int iLangID, bool fSecure, string strNameSpace, int iKPP, bool bUseSecureAuth)
		{
			return this.LogoTag2(strReturnUrl, iTimeWindow, fForceLogin ? 1 : 0, strCoBrandedArgs, iLangID, fSecure ? 1 : 0, strNameSpace, iKPP, bUseSecureAuth ? 10 : 0);
		}

		// Token: 0x06002901 RID: 10497 RVA: 0x000B3948 File Offset: 0x000B2948
		public string LogoTag2(string strReturnUrl, int iTimeWindow, int iForceLogin, string strCoBrandedArgs, int iLangID, int iSecure, string strNameSpace, int iKPP, int iUseSecureAuth)
		{
			StringBuilder stringBuilder = new StringBuilder(4092);
			int num = UnsafeNativeMethods.PassportLogoTag2(this._iPassport, strReturnUrl, iTimeWindow, iForceLogin, strCoBrandedArgs, iLangID, iSecure, strNameSpace, iKPP, iUseSecureAuth, stringBuilder, 4090);
			if (num < 0)
			{
				throw new COMException(SR.GetString("Passport_method_failed"), num);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002902 RID: 10498 RVA: 0x000B399C File Offset: 0x000B299C
		public string AuthUrl()
		{
			return this.AuthUrl(null, -1, -1, null, -1, null, -1, -1);
		}

		// Token: 0x06002903 RID: 10499 RVA: 0x000B39B8 File Offset: 0x000B29B8
		public string AuthUrl(string strReturnUrl)
		{
			return this.AuthUrl(strReturnUrl, -1, -1, null, -1, null, -1, -1);
		}

		// Token: 0x06002904 RID: 10500 RVA: 0x000B39D4 File Offset: 0x000B29D4
		public string AuthUrl2()
		{
			return this.AuthUrl2(null, -1, -1, null, -1, null, -1, -1);
		}

		// Token: 0x06002905 RID: 10501 RVA: 0x000B39F0 File Offset: 0x000B29F0
		public string AuthUrl2(string strReturnUrl)
		{
			return this.AuthUrl2(strReturnUrl, -1, -1, null, -1, null, -1, -1);
		}

		// Token: 0x06002906 RID: 10502 RVA: 0x000B3A0C File Offset: 0x000B2A0C
		public string AuthUrl(string strReturnUrl, int iTimeWindow, bool fForceLogin, string strCoBrandedArgs, int iLangID, string strNameSpace, int iKPP, bool bUseSecureAuth)
		{
			StringBuilder stringBuilder = new StringBuilder(4092);
			int num = UnsafeNativeMethods.PassportAuthURL(this._iPassport, strReturnUrl, iTimeWindow, fForceLogin ? 1 : 0, strCoBrandedArgs, iLangID, strNameSpace, iKPP, bUseSecureAuth ? 10 : 0, stringBuilder, 4090);
			if (num < 0)
			{
				throw new COMException(SR.GetString("Passport_method_failed"), num);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002907 RID: 10503 RVA: 0x000B3A6C File Offset: 0x000B2A6C
		public string AuthUrl2(string strReturnUrl, int iTimeWindow, bool fForceLogin, string strCoBrandedArgs, int iLangID, string strNameSpace, int iKPP, bool bUseSecureAuth)
		{
			StringBuilder stringBuilder = new StringBuilder(4092);
			int num = UnsafeNativeMethods.PassportAuthURL2(this._iPassport, strReturnUrl, iTimeWindow, fForceLogin ? 1 : 0, strCoBrandedArgs, iLangID, strNameSpace, iKPP, bUseSecureAuth ? 10 : 0, stringBuilder, 4090);
			if (num < 0)
			{
				throw new COMException(SR.GetString("Passport_method_failed"), num);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002908 RID: 10504 RVA: 0x000B3ACC File Offset: 0x000B2ACC
		public string AuthUrl(string strReturnUrl, int iTimeWindow, int iForceLogin, string strCoBrandedArgs, int iLangID, string strNameSpace, int iKPP, int iUseSecureAuth)
		{
			StringBuilder stringBuilder = new StringBuilder(4092);
			int num = UnsafeNativeMethods.PassportAuthURL(this._iPassport, strReturnUrl, iTimeWindow, iForceLogin, strCoBrandedArgs, iLangID, strNameSpace, iKPP, iUseSecureAuth, stringBuilder, 4090);
			if (num < 0)
			{
				throw new COMException(SR.GetString("Passport_method_failed"), num);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002909 RID: 10505 RVA: 0x000B3B20 File Offset: 0x000B2B20
		public string AuthUrl2(string strReturnUrl, int iTimeWindow, int iForceLogin, string strCoBrandedArgs, int iLangID, string strNameSpace, int iKPP, int iUseSecureAuth)
		{
			StringBuilder stringBuilder = new StringBuilder(4092);
			int num = UnsafeNativeMethods.PassportAuthURL2(this._iPassport, strReturnUrl, iTimeWindow, iForceLogin, strCoBrandedArgs, iLangID, strNameSpace, iKPP, iUseSecureAuth, stringBuilder, 4090);
			if (num < 0)
			{
				throw new COMException(SR.GetString("Passport_method_failed"), num);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600290A RID: 10506 RVA: 0x000B3B74 File Offset: 0x000B2B74
		public int LoginUser(string szRetURL, int iTimeWindow, bool fForceLogin, string szCOBrandArgs, int iLangID, string strNameSpace, int iKPP, bool fUseSecureAuth, object oExtraParams)
		{
			return this.LoginUser(szRetURL, iTimeWindow, fForceLogin ? 1 : 0, szCOBrandArgs, iLangID, strNameSpace, iKPP, fUseSecureAuth ? 10 : 0, oExtraParams);
		}

		// Token: 0x0600290B RID: 10507 RVA: 0x000B3BA4 File Offset: 0x000B2BA4
		public int LoginUser(string szRetURL, int iTimeWindow, int fForceLogin, string szCOBrandArgs, int iLangID, string strNameSpace, int iKPP, int iUseSecureAuth, object oExtraParams)
		{
			string text = this.GetLoginChallenge(szRetURL, iTimeWindow, fForceLogin, szCOBrandArgs, iLangID, strNameSpace, iKPP, iUseSecureAuth, oExtraParams);
			if (text == null || text.Length < 1)
			{
				return -1;
			}
			HttpContext httpContext = HttpContext.Current;
			this.SetHeaders(httpContext, text);
			this._WWWAuthHeaderSet = true;
			text = httpContext.Request.Headers["Accept-Auth"];
			if (text != null && text.Length > 0 && text.IndexOf("Passport", StringComparison.Ordinal) >= 0)
			{
				httpContext.Response.StatusCode = 401;
				httpContext.Response.End();
				return 0;
			}
			text = this.AuthUrl(szRetURL, iTimeWindow, fForceLogin, szCOBrandArgs, iLangID, strNameSpace, iKPP, iUseSecureAuth);
			if (!string.IsNullOrEmpty(text))
			{
				httpContext.Response.Redirect(text, false);
				return 0;
			}
			return -1;
		}

		// Token: 0x0600290C RID: 10508 RVA: 0x000B3C64 File Offset: 0x000B2C64
		public int LoginUser()
		{
			return this.LoginUser(null, -1, -1, null, -1, null, -1, -1, null);
		}

		// Token: 0x0600290D RID: 10509 RVA: 0x000B3C80 File Offset: 0x000B2C80
		public int LoginUser(string strReturnUrl)
		{
			return this.LoginUser(strReturnUrl, -1, -1, null, -1, null, -1, -1, null);
		}

		// Token: 0x0600290E RID: 10510 RVA: 0x000B3C9C File Offset: 0x000B2C9C
		public string GetLoginChallenge()
		{
			return this.GetLoginChallenge(null, -1, -1, null, -1, null, -1, -1, null);
		}

		// Token: 0x0600290F RID: 10511 RVA: 0x000B3CB8 File Offset: 0x000B2CB8
		public string GetLoginChallenge(string strReturnUrl)
		{
			return this.GetLoginChallenge(strReturnUrl, -1, -1, null, -1, null, -1, -1, null);
		}

		// Token: 0x06002910 RID: 10512 RVA: 0x000B3CD4 File Offset: 0x000B2CD4
		public string GetLoginChallenge(string szRetURL, int iTimeWindow, int fForceLogin, string szCOBrandArgs, int iLangID, string strNameSpace, int iKPP, int iUseSecureAuth, object oExtraParams)
		{
			StringBuilder stringBuilder = new StringBuilder(4092);
			int num = UnsafeNativeMethods.PassportGetLoginChallenge(this._iPassport, szRetURL, iTimeWindow, fForceLogin, szCOBrandArgs, iLangID, strNameSpace, iKPP, iUseSecureAuth, oExtraParams, stringBuilder, 4090);
			if (num < 0)
			{
				throw new COMException(SR.GetString("Passport_method_failed"), num);
			}
			string text = stringBuilder.ToString();
			if (text != null && !StringUtil.StringStartsWith(text, "WWW-Authenticate"))
			{
				text = "WWW-Authenticate: " + text;
			}
			return text;
		}

		// Token: 0x06002911 RID: 10513 RVA: 0x000B3D48 File Offset: 0x000B2D48
		public string GetDomainAttribute(string strAttribute, int iLCID, string strDomain)
		{
			StringBuilder stringBuilder = new StringBuilder(1028);
			int num = UnsafeNativeMethods.PassportGetDomainAttribute(this._iPassport, strAttribute, iLCID, strDomain, stringBuilder, 1024);
			if (num >= 0)
			{
				return stringBuilder.ToString();
			}
			throw new COMException(SR.GetString("Passport_method_failed"), num);
		}

		// Token: 0x06002912 RID: 10514 RVA: 0x000B3D90 File Offset: 0x000B2D90
		public object Ticket(string strAttribute)
		{
			object obj = new object();
			int num = UnsafeNativeMethods.PassportTicket(this._iPassport, strAttribute, out obj);
			if (num < 0)
			{
				throw new COMException(SR.GetString("Passport_method_failed"), num);
			}
			return obj;
		}

		// Token: 0x06002913 RID: 10515 RVA: 0x000B3DC8 File Offset: 0x000B2DC8
		public object GetCurrentConfig(string strAttribute)
		{
			object obj = new object();
			int num = UnsafeNativeMethods.PassportGetCurrentConfig(this._iPassport, strAttribute, out obj);
			if (num < 0)
			{
				throw new COMException(SR.GetString("Passport_method_failed"), num);
			}
			return obj;
		}

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x06002914 RID: 10516 RVA: 0x000B3E00 File Offset: 0x000B2E00
		public string HexPUID
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(1024);
				int num = UnsafeNativeMethods.PassportHexPUID(this._iPassport, stringBuilder, 1024);
				if (num >= 0)
				{
					return stringBuilder.ToString();
				}
				throw new COMException(SR.GetString("Passport_method_failed"), num);
			}
		}

		// Token: 0x06002915 RID: 10517 RVA: 0x000B3E45 File Offset: 0x000B2E45
		void IDisposable.Dispose()
		{
			if (this._iPassport != IntPtr.Zero)
			{
				UnsafeNativeMethods.PassportDestroy(this._iPassport);
			}
			this._iPassport = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002916 RID: 10518 RVA: 0x000B3E78 File Offset: 0x000B2E78
		public static void SignOut(string strSignOutDotGifFileName)
		{
			HttpContext httpContext = HttpContext.Current;
			string[] array = new string[] { "MSPAuth", "MSPProf", "MSPConsent", "MSPSecAuth", "MSPProfC" };
			string[] array2 = new string[] { "TicketDomain", "TicketDomain", "ProfileDomain", "SecureDomain", "TicketDomain" };
			string[] array3 = new string[] { "TicketPath", "TicketPath", "ProfilePath", "SecurePath", "TicketPath" };
			string[] array4 = new string[5];
			string[] array5 = new string[5];
			httpContext.Response.ClearHeaders();
			try
			{
				PassportIdentity passportIdentity;
				if (httpContext.User.Identity is PassportIdentity)
				{
					passportIdentity = (PassportIdentity)httpContext.User.Identity;
				}
				else
				{
					passportIdentity = new PassportIdentity();
				}
				if (passportIdentity != null && PassportIdentity._iPassportVer >= 3)
				{
					for (int i = 0; i < 5; i++)
					{
						object currentConfig = passportIdentity.GetCurrentConfig(array2[i]);
						if (currentConfig != null && currentConfig is string)
						{
							array4[i] = (string)currentConfig;
						}
					}
					for (int i = 0; i < 5; i++)
					{
						object currentConfig2 = passportIdentity.GetCurrentConfig(array3[i]);
						if (currentConfig2 != null && currentConfig2 is string)
						{
							array5[i] = (string)currentConfig2;
						}
					}
				}
			}
			catch
			{
			}
			for (int i = 0; i < 5; i++)
			{
				HttpCookie httpCookie = new HttpCookie(array[i], string.Empty);
				httpCookie.Expires = new DateTime(1998, 1, 1);
				if (array4[i] != null && array4[i].Length > 0)
				{
					httpCookie.Domain = array4[i];
				}
				if (array5[i] != null && array5[i].Length > 0)
				{
					httpCookie.Path = array5[i];
				}
				else
				{
					httpCookie.Path = "/";
				}
				httpContext.Response.Cookies.Add(httpCookie);
			}
			httpContext.Response.Expires = -1;
			httpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
			httpContext.Response.AppendHeader("Pragma", "no-cache");
			httpContext.Response.ContentType = "image/gif";
			httpContext.Response.WriteFile(strSignOutDotGifFileName);
			string text = httpContext.Request.QueryString["ru"];
			if (text != null && text.Length > 1)
			{
				httpContext.Response.Redirect(text, false);
			}
		}

		// Token: 0x06002917 RID: 10519 RVA: 0x000B4128 File Offset: 0x000B3128
		public static string Encrypt(string strData)
		{
			return PassportIdentity.CallPassportCryptFunction(0, strData);
		}

		// Token: 0x06002918 RID: 10520 RVA: 0x000B4131 File Offset: 0x000B3131
		public static string Decrypt(string strData)
		{
			return PassportIdentity.CallPassportCryptFunction(1, strData);
		}

		// Token: 0x06002919 RID: 10521 RVA: 0x000B413A File Offset: 0x000B313A
		public static string Compress(string strData)
		{
			return PassportIdentity.CallPassportCryptFunction(2, strData);
		}

		// Token: 0x0600291A RID: 10522 RVA: 0x000B4143 File Offset: 0x000B3143
		public static string Decompress(string strData)
		{
			return PassportIdentity.CallPassportCryptFunction(3, strData);
		}

		// Token: 0x0600291B RID: 10523 RVA: 0x000B414C File Offset: 0x000B314C
		public static int CryptPutHost(string strHost)
		{
			int num = UnsafeNativeMethods.PassportCryptPut(0, strHost);
			if (num < 0)
			{
				throw new COMException(SR.GetString("Passport_method_failed"), num);
			}
			return num;
		}

		// Token: 0x0600291C RID: 10524 RVA: 0x000B4178 File Offset: 0x000B3178
		public static int CryptPutSite(string strSite)
		{
			int num = UnsafeNativeMethods.PassportCryptPut(1, strSite);
			if (num < 0)
			{
				throw new COMException(SR.GetString("Passport_method_failed"), num);
			}
			return num;
		}

		// Token: 0x0600291D RID: 10525 RVA: 0x000B41A4 File Offset: 0x000B31A4
		public static bool CryptIsValid()
		{
			int num = UnsafeNativeMethods.PassportCryptIsValid();
			if (num < 0)
			{
				throw new COMException(SR.GetString("Passport_method_failed"), num);
			}
			return num == 0;
		}

		// Token: 0x0600291E RID: 10526 RVA: 0x000B41D0 File Offset: 0x000B31D0
		private static string CallPassportCryptFunction(int iFunctionID, string strData)
		{
			int num = ((strData == null || strData.Length < 512) ? 512 : strData.Length);
			StringBuilder stringBuilder;
			int num2;
			for (;;)
			{
				num *= 2;
				stringBuilder = new StringBuilder(num);
				num2 = UnsafeNativeMethods.PassportCrypt(iFunctionID, strData, stringBuilder, num);
				if (num2 == 0)
				{
					break;
				}
				if (num2 != -2147024774 && num2 < 0)
				{
					goto Block_5;
				}
				if (num2 != -2147024774 || num >= 10485760)
				{
					goto IL_006C;
				}
			}
			return stringBuilder.ToString();
			Block_5:
			throw new COMException(SR.GetString("Passport_method_failed"), num2);
			IL_006C:
			return null;
		}

		// Token: 0x04001EE0 RID: 7904
		private string _Name;

		// Token: 0x04001EE1 RID: 7905
		private bool _Authenticated;

		// Token: 0x04001EE2 RID: 7906
		private IntPtr _iPassport;

		// Token: 0x04001EE3 RID: 7907
		private static int _iPassportVer;

		// Token: 0x04001EE4 RID: 7908
		private bool _WWWAuthHeaderSet;
	}
}
