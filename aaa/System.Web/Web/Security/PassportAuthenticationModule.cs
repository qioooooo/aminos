using System;
using System.Security.Permissions;
using System.Web.Configuration;
using System.Web.Handlers;

namespace System.Web.Security
{
	// Token: 0x0200034B RID: 843
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class PassportAuthenticationModule : IHttpModule
	{
		// Token: 0x060028D0 RID: 10448 RVA: 0x000B2BFB File Offset: 0x000B1BFB
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		public PassportAuthenticationModule()
		{
		}

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x060028D1 RID: 10449 RVA: 0x000B2C03 File Offset: 0x000B1C03
		// (remove) Token: 0x060028D2 RID: 10450 RVA: 0x000B2C1C File Offset: 0x000B1C1C
		public event PassportAuthenticationEventHandler Authenticate
		{
			add
			{
				this._eventHandler = (PassportAuthenticationEventHandler)Delegate.Combine(this._eventHandler, value);
			}
			remove
			{
				this._eventHandler = (PassportAuthenticationEventHandler)Delegate.Remove(this._eventHandler, value);
			}
		}

		// Token: 0x060028D3 RID: 10451 RVA: 0x000B2C35 File Offset: 0x000B1C35
		public void Dispose()
		{
		}

		// Token: 0x060028D4 RID: 10452 RVA: 0x000B2C37 File Offset: 0x000B1C37
		public void Init(HttpApplication app)
		{
			app.AuthenticateRequest += this.OnEnter;
			app.EndRequest += this.OnLeave;
		}

		// Token: 0x060028D5 RID: 10453 RVA: 0x000B2C60 File Offset: 0x000B1C60
		private void OnAuthenticate(PassportAuthenticationEventArgs e)
		{
			if (this._eventHandler != null)
			{
				this._eventHandler(this, e);
				if (e.Context.User == null && e.User != null)
				{
					InternalSecurityPermissions.ControlPrincipal.Demand();
					e.Context.User = e.User;
				}
			}
			if (e.Context.User == null)
			{
				InternalSecurityPermissions.ControlPrincipal.Demand();
				e.Context.User = new PassportPrincipal(e.Identity, new string[0]);
			}
		}

		// Token: 0x060028D6 RID: 10454 RVA: 0x000B2CE8 File Offset: 0x000B1CE8
		private void OnEnter(object source, EventArgs eventArgs)
		{
			if (PassportAuthenticationModule._fAuthChecked && !PassportAuthenticationModule._fAuthRequired)
			{
				return;
			}
			HttpApplication httpApplication = (HttpApplication)source;
			HttpContext context = httpApplication.Context;
			if (!PassportAuthenticationModule._fAuthChecked)
			{
				AuthenticationSection authentication = RuntimeConfig.GetAppConfig().Authentication;
				authentication.ValidateAuthenticationMode();
				PassportAuthenticationModule._fAuthRequired = authentication.Mode == AuthenticationMode.Passport;
				PassportAuthenticationModule._LoginUrl = authentication.Passport.RedirectUrl;
				PassportAuthenticationModule._fAuthChecked = true;
			}
			if (!PassportAuthenticationModule._fAuthRequired)
			{
				return;
			}
			PassportIdentity passportIdentity = new PassportIdentity();
			this.OnAuthenticate(new PassportAuthenticationEventArgs(passportIdentity, context));
			context.SetSkipAuthorizationNoDemand(AuthenticationConfig.AccessingLoginPage(context, PassportAuthenticationModule._LoginUrl), false);
			if (!context.SkipAuthorization)
			{
				context.SkipAuthorization = AssemblyResourceLoader.IsValidWebResourceRequest(context);
			}
		}

		// Token: 0x060028D7 RID: 10455 RVA: 0x000B2D90 File Offset: 0x000B1D90
		private void OnLeave(object source, EventArgs eventArgs)
		{
			HttpApplication httpApplication = (HttpApplication)source;
			HttpContext context = httpApplication.Context;
			if (!PassportAuthenticationModule._fAuthChecked || !PassportAuthenticationModule._fAuthRequired || context.User == null || context.User.Identity == null || !(context.User.Identity is PassportIdentity))
			{
				return;
			}
			PassportIdentity passportIdentity = (PassportIdentity)context.User.Identity;
			if (context.Response.StatusCode != 401 || passportIdentity.WWWAuthHeaderSet)
			{
				return;
			}
			if (PassportAuthenticationModule._LoginUrl == null || PassportAuthenticationModule._LoginUrl.Length < 1 || string.Compare(PassportAuthenticationModule._LoginUrl, "internal", StringComparison.Ordinal) == 0)
			{
				context.Response.Clear();
				context.Response.StatusCode = 200;
				if (!ErrorFormatter.RequiresAdaptiveErrorReporting(context))
				{
					string text = context.Request.Url.ToString();
					int num = text.IndexOf('?');
					if (num >= 0)
					{
						text = text.Substring(0, num);
					}
					string text2 = passportIdentity.LogoTag2(HttpUtility.UrlEncode(text, context.Request.ContentEncoding));
					string @string = SR.GetString("PassportAuthFailed", new object[] { text2 });
					context.Response.Write(@string);
					return;
				}
				ErrorFormatter errorFormatter = new PassportAuthFailedErrorFormatter();
				context.Response.Write(errorFormatter.GetAdaptiveErrorMessage(context, true));
				return;
			}
			else
			{
				string completeLoginUrl = AuthenticationConfig.GetCompleteLoginUrl(context, PassportAuthenticationModule._LoginUrl);
				if (completeLoginUrl == null || completeLoginUrl.Length <= 0)
				{
					throw new HttpException(SR.GetString("Invalid_Passport_Redirect_URL"));
				}
				string text3 = context.Request.Url.ToString();
				string text4;
				if (completeLoginUrl.IndexOf('?') >= 0)
				{
					text4 = "&";
				}
				else
				{
					text4 = "?";
				}
				string text5 = completeLoginUrl + text4 + "ReturnUrl=" + HttpUtility.UrlEncode(text3, context.Request.ContentEncoding);
				int num2 = text3.IndexOf('?');
				if (num2 >= 0 && num2 < text3.Length - 1)
				{
					text5 = text5 + "&" + text3.Substring(num2 + 1);
				}
				context.Response.Redirect(text5, false);
				return;
			}
		}

		// Token: 0x04001EDC RID: 7900
		private PassportAuthenticationEventHandler _eventHandler;

		// Token: 0x04001EDD RID: 7901
		private static bool _fAuthChecked;

		// Token: 0x04001EDE RID: 7902
		private static bool _fAuthRequired;

		// Token: 0x04001EDF RID: 7903
		private static string _LoginUrl;
	}
}
