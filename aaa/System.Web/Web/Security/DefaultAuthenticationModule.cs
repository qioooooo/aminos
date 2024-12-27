using System;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace System.Web.Security
{
	// Token: 0x0200032B RID: 811
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class DefaultAuthenticationModule : IHttpModule
	{
		// Token: 0x060027DC RID: 10204 RVA: 0x000AEC1D File Offset: 0x000ADC1D
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		public DefaultAuthenticationModule()
		{
		}

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x060027DD RID: 10205 RVA: 0x000AEC28 File Offset: 0x000ADC28
		// (remove) Token: 0x060027DE RID: 10206 RVA: 0x000AEC73 File Offset: 0x000ADC73
		public event DefaultAuthenticationEventHandler Authenticate
		{
			add
			{
				if (HttpRuntime.UseIntegratedPipeline)
				{
					throw new PlatformNotSupportedException(SR.GetString("Method_Not_Supported_By_Iis_Integrated_Mode", new object[] { "DefaultAuthentication.Authenticate" }));
				}
				this._eventHandler = (DefaultAuthenticationEventHandler)Delegate.Combine(this._eventHandler, value);
			}
			remove
			{
				this._eventHandler = (DefaultAuthenticationEventHandler)Delegate.Remove(this._eventHandler, value);
			}
		}

		// Token: 0x060027DF RID: 10207 RVA: 0x000AEC8C File Offset: 0x000ADC8C
		public void Dispose()
		{
		}

		// Token: 0x060027E0 RID: 10208 RVA: 0x000AEC8E File Offset: 0x000ADC8E
		public void Init(HttpApplication app)
		{
			if (HttpRuntime.UseIntegratedPipeline)
			{
				app.PostAuthenticateRequest += this.OnEnter;
				return;
			}
			app.DefaultAuthentication += this.OnEnter;
		}

		// Token: 0x060027E1 RID: 10209 RVA: 0x000AECBC File Offset: 0x000ADCBC
		private void OnAuthenticate(DefaultAuthenticationEventArgs e)
		{
			if (this._eventHandler != null)
			{
				this._eventHandler(this, e);
			}
		}

		// Token: 0x060027E2 RID: 10210 RVA: 0x000AECD4 File Offset: 0x000ADCD4
		private void OnEnter(object source, EventArgs eventArgs)
		{
			HttpApplication httpApplication = (HttpApplication)source;
			HttpContext context = httpApplication.Context;
			if (context.Response.StatusCode > 200)
			{
				if (context.Response.StatusCode == 401)
				{
					this.WriteErrorMessage(context);
				}
				httpApplication.CompleteRequest();
				return;
			}
			if (context.User == null)
			{
				this.OnAuthenticate(new DefaultAuthenticationEventArgs(context));
				if (context.Response.StatusCode > 200)
				{
					if (context.Response.StatusCode == 401)
					{
						this.WriteErrorMessage(context);
					}
					httpApplication.CompleteRequest();
					return;
				}
			}
			if (context.User == null)
			{
				context.SetPrincipalNoDemand(new GenericPrincipal(new GenericIdentity(string.Empty, string.Empty), new string[0]), false);
			}
			Thread.CurrentPrincipal = context.User;
		}

		// Token: 0x060027E3 RID: 10211 RVA: 0x000AED9B File Offset: 0x000ADD9B
		private void WriteErrorMessage(HttpContext context)
		{
			context.Response.Write(AuthFailedErrorFormatter.GetErrorText());
			context.Response.GenerateResponseHeadersForHandler();
		}

		// Token: 0x04001E70 RID: 7792
		private DefaultAuthenticationEventHandler _eventHandler;
	}
}
