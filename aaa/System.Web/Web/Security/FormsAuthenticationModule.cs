using System;
using System.Security.Permissions;
using System.Security.Principal;
using System.Web.Configuration;
using System.Web.Handlers;
using System.Web.Management;

namespace System.Web.Security
{
	// Token: 0x02000337 RID: 823
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class FormsAuthenticationModule : IHttpModule
	{
		// Token: 0x06002847 RID: 10311 RVA: 0x000B0F00 File Offset: 0x000AFF00
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		public FormsAuthenticationModule()
		{
		}

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x06002848 RID: 10312 RVA: 0x000B0F08 File Offset: 0x000AFF08
		// (remove) Token: 0x06002849 RID: 10313 RVA: 0x000B0F21 File Offset: 0x000AFF21
		public event FormsAuthenticationEventHandler Authenticate
		{
			add
			{
				this._eventHandler = (FormsAuthenticationEventHandler)Delegate.Combine(this._eventHandler, value);
			}
			remove
			{
				this._eventHandler = (FormsAuthenticationEventHandler)Delegate.Remove(this._eventHandler, value);
			}
		}

		// Token: 0x0600284A RID: 10314 RVA: 0x000B0F3C File Offset: 0x000AFF3C
		private static string GetReturnUrl(HttpContext context)
		{
			if (context.WorkerRequest == null || !context.WorkerRequest.IsRewriteModuleEnabled)
			{
				return context.Request.PathWithQueryString;
			}
			return context.Request.RawUrl;
		}

		// Token: 0x0600284B RID: 10315 RVA: 0x000B0F7A File Offset: 0x000AFF7A
		public void Dispose()
		{
		}

		// Token: 0x0600284C RID: 10316 RVA: 0x000B0F7C File Offset: 0x000AFF7C
		public void Init(HttpApplication app)
		{
			if (!FormsAuthenticationModule._fAuthChecked)
			{
				AuthenticationSection authentication = RuntimeConfig.GetAppConfig().Authentication;
				authentication.ValidateAuthenticationMode();
				FormsAuthenticationModule._fAuthRequired = authentication.Mode == AuthenticationMode.Forms;
				FormsAuthenticationModule._LoginUrl = authentication.Forms.LoginUrl;
				if (FormsAuthenticationModule._LoginUrl == null)
				{
					FormsAuthenticationModule._LoginUrl = "login.aspx";
				}
				FormsAuthenticationModule._fAuthChecked = true;
			}
			if (FormsAuthenticationModule._fAuthRequired)
			{
				FormsAuthentication.Initialize();
				app.AuthenticateRequest += this.OnEnter;
				app.EndRequest += this.OnLeave;
			}
		}

		// Token: 0x0600284D RID: 10317 RVA: 0x000B1008 File Offset: 0x000B0008
		private void OnAuthenticate(FormsAuthenticationEventArgs e)
		{
			HttpCookie httpCookie = null;
			if (this._eventHandler != null)
			{
				this._eventHandler(this, e);
			}
			if (e.Context.User != null)
			{
				return;
			}
			if (e.User != null)
			{
				e.Context.SetPrincipalNoDemand(e.User);
				return;
			}
			FormsAuthenticationTicket formsAuthenticationTicket = null;
			bool flag = false;
			try
			{
				formsAuthenticationTicket = FormsAuthenticationModule.ExtractTicketFromCookie(e.Context, FormsAuthentication.FormsCookieName, out flag);
			}
			catch
			{
				formsAuthenticationTicket = null;
			}
			if (formsAuthenticationTicket == null || formsAuthenticationTicket.Expired)
			{
				return;
			}
			FormsAuthenticationTicket formsAuthenticationTicket2 = formsAuthenticationTicket;
			if (FormsAuthentication.SlidingExpiration)
			{
				formsAuthenticationTicket2 = FormsAuthentication.RenewTicketIfOld(formsAuthenticationTicket);
			}
			e.Context.SetPrincipalNoDemand(new GenericPrincipal(new FormsIdentity(formsAuthenticationTicket2), new string[0]));
			if (!flag && !formsAuthenticationTicket2.CookiePath.Equals("/"))
			{
				httpCookie = e.Context.Request.Cookies[FormsAuthentication.FormsCookieName];
				if (httpCookie != null)
				{
					httpCookie.Path = formsAuthenticationTicket2.CookiePath;
				}
			}
			if (formsAuthenticationTicket2 != formsAuthenticationTicket)
			{
				if (flag && formsAuthenticationTicket2.CookiePath != "/" && formsAuthenticationTicket2.CookiePath.Length > 1)
				{
					formsAuthenticationTicket2 = FormsAuthenticationTicket.FromUtc(formsAuthenticationTicket2.Version, formsAuthenticationTicket2.Name, formsAuthenticationTicket2.IssueDateUtc, formsAuthenticationTicket2.ExpirationUtc, formsAuthenticationTicket2.IsPersistent, formsAuthenticationTicket2.UserData, "/");
				}
				string text = FormsAuthentication.Encrypt(formsAuthenticationTicket2);
				if (flag)
				{
					e.Context.CookielessHelper.SetCookieValue('F', text);
					e.Context.Response.Redirect(FormsAuthenticationModule.GetReturnUrl(e.Context));
					return;
				}
				if (httpCookie != null)
				{
					httpCookie = e.Context.Request.Cookies[FormsAuthentication.FormsCookieName];
				}
				if (httpCookie == null)
				{
					httpCookie = new HttpCookie(FormsAuthentication.FormsCookieName, text);
					httpCookie.Path = formsAuthenticationTicket2.CookiePath;
				}
				if (formsAuthenticationTicket2.IsPersistent)
				{
					httpCookie.Expires = formsAuthenticationTicket2.Expiration;
				}
				httpCookie.Value = text;
				httpCookie.Secure = FormsAuthentication.RequireSSL;
				httpCookie.HttpOnly = true;
				if (FormsAuthentication.CookieDomain != null)
				{
					httpCookie.Domain = FormsAuthentication.CookieDomain;
				}
				e.Context.Response.Cookies.Remove(httpCookie.Name);
				e.Context.Response.Cookies.Add(httpCookie);
			}
		}

		// Token: 0x0600284E RID: 10318 RVA: 0x000B1238 File Offset: 0x000B0238
		private void OnEnter(object source, EventArgs eventArgs)
		{
			this._fOnEnterCalled = true;
			HttpApplication httpApplication = (HttpApplication)source;
			HttpContext context = httpApplication.Context;
			FormsAuthenticationModule.Trace("*******************Request path: " + FormsAuthenticationModule.GetReturnUrl(context));
			this.OnAuthenticate(new FormsAuthenticationEventArgs(context));
			CookielessHelperClass cookielessHelper = context.CookielessHelper;
			if (AuthenticationConfig.AccessingLoginPage(context, FormsAuthenticationModule._LoginUrl))
			{
				context.SetSkipAuthorizationNoDemand(true, false);
				cookielessHelper.RedirectWithDetectionIfRequired(null, FormsAuthentication.CookieMode);
			}
			if (!context.SkipAuthorization)
			{
				context.SetSkipAuthorizationNoDemand(AssemblyResourceLoader.IsValidWebResourceRequest(context), false);
			}
		}

		// Token: 0x0600284F RID: 10319 RVA: 0x000B12B8 File Offset: 0x000B02B8
		private void OnLeave(object source, EventArgs eventArgs)
		{
			if (!this._fOnEnterCalled)
			{
				return;
			}
			this._fOnEnterCalled = false;
			HttpApplication httpApplication = (HttpApplication)source;
			HttpContext context = httpApplication.Context;
			if (!context.Request.IsSecureConnection && context.Response.Cookies.GetNoCreate(FormsAuthentication.FormsCookieName) != null)
			{
				context.Response.Cache.SetCacheability(HttpCacheability.NoCache, "Set-Cookie");
			}
			if (context.Response.StatusCode != 401)
			{
				return;
			}
			string returnUrl = FormsAuthenticationModule.GetReturnUrl(context);
			if (returnUrl.IndexOf("?" + FormsAuthentication.ReturnUrlVar + "=", StringComparison.Ordinal) != -1 || returnUrl.IndexOf("&" + FormsAuthentication.ReturnUrlVar + "=", StringComparison.Ordinal) != -1)
			{
				return;
			}
			string text = null;
			if (!string.IsNullOrEmpty(FormsAuthenticationModule._LoginUrl))
			{
				text = AuthenticationConfig.GetCompleteLoginUrl(context, FormsAuthenticationModule._LoginUrl);
			}
			if (text == null || text.Length <= 0)
			{
				throw new HttpException(SR.GetString("Auth_Invalid_Login_Url"));
			}
			CookielessHelperClass cookielessHelper = context.CookielessHelper;
			string text2;
			if (text.IndexOf('?') >= 0)
			{
				text = FormsAuthentication.RemoveQueryStringVariableFromUrl(text, FormsAuthentication.ReturnUrlVar);
				text2 = string.Concat(new string[]
				{
					text,
					"&",
					FormsAuthentication.ReturnUrlVar,
					"=",
					HttpUtility.UrlEncode(returnUrl, context.Request.ContentEncoding)
				});
			}
			else
			{
				text2 = string.Concat(new string[]
				{
					text,
					"?",
					FormsAuthentication.ReturnUrlVar,
					"=",
					HttpUtility.UrlEncode(returnUrl, context.Request.ContentEncoding)
				});
			}
			int num = returnUrl.IndexOf('?');
			if (num >= 0 && num < returnUrl.Length - 1)
			{
				text2 = text2 + "&" + returnUrl.Substring(num + 1);
			}
			cookielessHelper.SetCookieValue('F', null);
			cookielessHelper.RedirectWithDetectionIfRequired(text2, FormsAuthentication.CookieMode);
			context.Response.Redirect(text2, false);
		}

		// Token: 0x06002850 RID: 10320 RVA: 0x000B14B4 File Offset: 0x000B04B4
		private static FormsAuthenticationTicket ExtractTicketFromCookie(HttpContext context, string name, out bool cookielessTicket)
		{
			FormsAuthenticationTicket formsAuthenticationTicket = null;
			string text = null;
			bool flag = false;
			bool flag2 = false;
			FormsAuthenticationTicket formsAuthenticationTicket2;
			try
			{
				try
				{
					cookielessTicket = CookielessHelperClass.UseCookieless(context, false, FormsAuthentication.CookieMode);
					if (cookielessTicket)
					{
						text = context.CookielessHelper.GetCookieValue('F');
					}
					else
					{
						HttpCookie httpCookie = context.Request.Cookies[name];
						if (httpCookie != null)
						{
							text = httpCookie.Value;
						}
					}
					if (text != null && text.Length > 1)
					{
						try
						{
							formsAuthenticationTicket = FormsAuthentication.Decrypt(text);
						}
						catch
						{
							if (cookielessTicket)
							{
								context.CookielessHelper.SetCookieValue('F', null);
							}
							else
							{
								context.Request.Cookies.Remove(name);
							}
							flag2 = true;
						}
						if (formsAuthenticationTicket == null)
						{
							flag2 = true;
						}
						if (formsAuthenticationTicket != null && !formsAuthenticationTicket.Expired && (cookielessTicket || !FormsAuthentication.RequireSSL || context.Request.IsSecureConnection))
						{
							return formsAuthenticationTicket;
						}
						if (formsAuthenticationTicket != null && formsAuthenticationTicket.Expired)
						{
							flag = true;
						}
						formsAuthenticationTicket = null;
						if (cookielessTicket)
						{
							context.CookielessHelper.SetCookieValue('F', null);
						}
						else
						{
							context.Request.Cookies.Remove(name);
						}
					}
					if (FormsAuthentication.EnableCrossAppRedirects)
					{
						text = context.Request.QueryString[name];
						if (text != null && text.Length > 1)
						{
							if (!cookielessTicket && FormsAuthentication.CookieMode == HttpCookieMode.AutoDetect)
							{
								cookielessTicket = CookielessHelperClass.UseCookieless(context, true, FormsAuthentication.CookieMode);
							}
							try
							{
								formsAuthenticationTicket = FormsAuthentication.Decrypt(text);
							}
							catch
							{
								flag2 = true;
							}
							if (formsAuthenticationTicket == null)
							{
								flag2 = true;
							}
						}
						if (formsAuthenticationTicket == null || formsAuthenticationTicket.Expired)
						{
							text = context.Request.Form[name];
							if (text != null && text.Length > 1)
							{
								if (!cookielessTicket && FormsAuthentication.CookieMode == HttpCookieMode.AutoDetect)
								{
									cookielessTicket = CookielessHelperClass.UseCookieless(context, true, FormsAuthentication.CookieMode);
								}
								try
								{
									formsAuthenticationTicket = FormsAuthentication.Decrypt(text);
								}
								catch
								{
									flag2 = true;
								}
								if (formsAuthenticationTicket == null)
								{
									flag2 = true;
								}
							}
						}
					}
					if (formsAuthenticationTicket == null || formsAuthenticationTicket.Expired)
					{
						if (formsAuthenticationTicket != null && formsAuthenticationTicket.Expired)
						{
							flag = true;
						}
						formsAuthenticationTicket2 = null;
					}
					else
					{
						if (FormsAuthentication.RequireSSL && !context.Request.IsSecureConnection)
						{
							throw new HttpException(SR.GetString("Connection_not_secure_creating_secure_cookie"));
						}
						if (cookielessTicket)
						{
							if (formsAuthenticationTicket.CookiePath != "/")
							{
								formsAuthenticationTicket = FormsAuthenticationTicket.FromUtc(formsAuthenticationTicket.Version, formsAuthenticationTicket.Name, formsAuthenticationTicket.IssueDateUtc, formsAuthenticationTicket.ExpirationUtc, formsAuthenticationTicket.IsPersistent, formsAuthenticationTicket.UserData, "/");
								text = FormsAuthentication.Encrypt(formsAuthenticationTicket);
							}
							context.CookielessHelper.SetCookieValue('F', text);
							string text2 = FormsAuthentication.RemoveQueryStringVariableFromUrl(FormsAuthenticationModule.GetReturnUrl(context), name);
							context.Response.Redirect(text2);
						}
						else
						{
							HttpCookie httpCookie2 = new HttpCookie(name, text);
							httpCookie2.HttpOnly = true;
							httpCookie2.Path = formsAuthenticationTicket.CookiePath;
							if (formsAuthenticationTicket.IsPersistent)
							{
								httpCookie2.Expires = formsAuthenticationTicket.Expiration;
							}
							httpCookie2.Secure = FormsAuthentication.RequireSSL;
							if (FormsAuthentication.CookieDomain != null)
							{
								httpCookie2.Domain = FormsAuthentication.CookieDomain;
							}
							context.Response.Cookies.Remove(httpCookie2.Name);
							context.Response.Cookies.Add(httpCookie2);
						}
						formsAuthenticationTicket2 = formsAuthenticationTicket;
					}
				}
				finally
				{
					if (flag2)
					{
						WebBaseEvent.RaiseSystemEvent(null, 4005, 50201);
					}
					else if (flag)
					{
						WebBaseEvent.RaiseSystemEvent(null, 4005, 50202);
					}
				}
			}
			catch
			{
				throw;
			}
			return formsAuthenticationTicket2;
		}

		// Token: 0x06002851 RID: 10321 RVA: 0x000B1844 File Offset: 0x000B0844
		private static void Trace(string str)
		{
		}

		// Token: 0x04001E9D RID: 7837
		private const string CONFIG_DEFAULT_COOKIE = ".ASPXAUTH";

		// Token: 0x04001E9E RID: 7838
		private const string CONFIG_DEFAULT_LOGINURL = "login.aspx";

		// Token: 0x04001E9F RID: 7839
		private static string _LoginUrl;

		// Token: 0x04001EA0 RID: 7840
		private static bool _fAuthChecked;

		// Token: 0x04001EA1 RID: 7841
		private static bool _fAuthRequired;

		// Token: 0x04001EA2 RID: 7842
		private bool _fOnEnterCalled;

		// Token: 0x04001EA3 RID: 7843
		private FormsAuthenticationEventHandler _eventHandler;
	}
}
