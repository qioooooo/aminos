using System;
using System.Security.Permissions;
using System.Threading;

namespace System.Web.Security
{
	// Token: 0x02000351 RID: 849
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class RoleManagerModule : IHttpModule
	{
		// Token: 0x06002928 RID: 10536 RVA: 0x000B427C File Offset: 0x000B327C
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		public RoleManagerModule()
		{
		}

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06002929 RID: 10537 RVA: 0x000B4284 File Offset: 0x000B3284
		// (remove) Token: 0x0600292A RID: 10538 RVA: 0x000B42AC File Offset: 0x000B32AC
		public event RoleManagerEventHandler GetRoles
		{
			add
			{
				HttpRuntime.CheckAspNetHostingPermission(AspNetHostingPermissionLevel.Low, "Feature_not_supported_at_this_level");
				this._eventHandler = (RoleManagerEventHandler)Delegate.Combine(this._eventHandler, value);
			}
			remove
			{
				this._eventHandler = (RoleManagerEventHandler)Delegate.Remove(this._eventHandler, value);
			}
		}

		// Token: 0x0600292B RID: 10539 RVA: 0x000B42C5 File Offset: 0x000B32C5
		public void Dispose()
		{
		}

		// Token: 0x0600292C RID: 10540 RVA: 0x000B42C7 File Offset: 0x000B32C7
		public void Init(HttpApplication app)
		{
			if (Roles.Enabled)
			{
				app.PostAuthenticateRequest += this.OnEnter;
				app.EndRequest += this.OnLeave;
			}
		}

		// Token: 0x0600292D RID: 10541 RVA: 0x000B42F4 File Offset: 0x000B32F4
		private void OnEnter(object source, EventArgs eventArgs)
		{
			if (!Roles.Enabled)
			{
				if (HttpRuntime.UseIntegratedPipeline)
				{
					((HttpApplication)source).Context.DisableNotifications(RequestNotification.EndRequest, (RequestNotification)0);
				}
				return;
			}
			HttpApplication httpApplication = (HttpApplication)source;
			HttpContext context = httpApplication.Context;
			if (this._eventHandler != null)
			{
				RoleManagerEventArgs roleManagerEventArgs = new RoleManagerEventArgs(context);
				this._eventHandler(this, roleManagerEventArgs);
				if (roleManagerEventArgs.RolesPopulated)
				{
					return;
				}
			}
			if (Roles.CacheRolesInCookie)
			{
				if (context.User.Identity.IsAuthenticated)
				{
					if (Roles.CookieRequireSSL)
					{
						if (!context.Request.IsSecureConnection)
						{
							goto IL_0118;
						}
					}
					try
					{
						HttpCookie httpCookie = context.Request.Cookies[Roles.CookieName];
						if (httpCookie != null)
						{
							string value = httpCookie.Value;
							if (value != null && value.Length > 4096)
							{
								Roles.DeleteCookie();
							}
							else
							{
								if (!string.IsNullOrEmpty(Roles.CookiePath) && Roles.CookiePath != "/")
								{
									httpCookie.Path = Roles.CookiePath;
								}
								httpCookie.Domain = Roles.Domain;
								context.User = new RolePrincipal(context.User.Identity, value);
							}
						}
						goto IL_0147;
					}
					catch
					{
						goto IL_0147;
					}
				}
				IL_0118:
				if (context.Request.Cookies[Roles.CookieName] != null)
				{
					Roles.DeleteCookie();
				}
				if (HttpRuntime.UseIntegratedPipeline)
				{
					context.DisableNotifications(RequestNotification.EndRequest, (RequestNotification)0);
				}
			}
			IL_0147:
			if (!(context.User is RolePrincipal))
			{
				context.User = new RolePrincipal(context.User.Identity);
			}
			Thread.CurrentPrincipal = context.User;
		}

		// Token: 0x0600292E RID: 10542 RVA: 0x000B4488 File Offset: 0x000B3488
		private void OnLeave(object source, EventArgs eventArgs)
		{
			HttpApplication httpApplication = (HttpApplication)source;
			HttpContext context = httpApplication.Context;
			if (!Roles.Enabled || !Roles.CacheRolesInCookie || context.Response.HeadersWritten)
			{
				return;
			}
			if (context.User == null || !(context.User is RolePrincipal) || !context.User.Identity.IsAuthenticated)
			{
				return;
			}
			if (Roles.CookieRequireSSL && !context.Request.IsSecureConnection)
			{
				if (context.Request.Cookies[Roles.CookieName] != null)
				{
					Roles.DeleteCookie();
				}
				return;
			}
			RolePrincipal rolePrincipal = (RolePrincipal)context.User;
			if (rolePrincipal.CachedListChanged && context.Request.Browser.Cookies)
			{
				string text = rolePrincipal.ToEncryptedTicket();
				if (string.IsNullOrEmpty(text) || text.Length > 4096)
				{
					Roles.DeleteCookie();
					return;
				}
				HttpCookie httpCookie = new HttpCookie(Roles.CookieName, text);
				httpCookie.HttpOnly = true;
				httpCookie.Path = Roles.CookiePath;
				httpCookie.Domain = Roles.Domain;
				if (Roles.CreatePersistentCookie)
				{
					httpCookie.Expires = rolePrincipal.ExpireDate;
				}
				httpCookie.Secure = Roles.CookieRequireSSL;
				context.Response.Cookies.Add(httpCookie);
			}
		}

		// Token: 0x04001EE7 RID: 7911
		private const int MAX_COOKIE_LENGTH = 4096;

		// Token: 0x04001EE8 RID: 7912
		private RoleManagerEventHandler _eventHandler;
	}
}
