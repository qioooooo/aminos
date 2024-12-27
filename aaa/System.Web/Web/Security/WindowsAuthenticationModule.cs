using System;
using System.Security.Permissions;
using System.Security.Principal;
using System.Web.Configuration;
using System.Web.Util;

namespace System.Web.Security
{
	// Token: 0x0200035A RID: 858
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class WindowsAuthenticationModule : IHttpModule
	{
		// Token: 0x060029C1 RID: 10689 RVA: 0x000BA84A File Offset: 0x000B984A
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		public WindowsAuthenticationModule()
		{
		}

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x060029C2 RID: 10690 RVA: 0x000BA852 File Offset: 0x000B9852
		// (remove) Token: 0x060029C3 RID: 10691 RVA: 0x000BA86B File Offset: 0x000B986B
		public event WindowsAuthenticationEventHandler Authenticate
		{
			add
			{
				this._eventHandler = (WindowsAuthenticationEventHandler)Delegate.Combine(this._eventHandler, value);
			}
			remove
			{
				this._eventHandler = (WindowsAuthenticationEventHandler)Delegate.Remove(this._eventHandler, value);
			}
		}

		// Token: 0x060029C4 RID: 10692 RVA: 0x000BA884 File Offset: 0x000B9884
		public void Dispose()
		{
		}

		// Token: 0x060029C5 RID: 10693 RVA: 0x000BA886 File Offset: 0x000B9886
		public void Init(HttpApplication app)
		{
			app.AuthenticateRequest += this.OnEnter;
		}

		// Token: 0x060029C6 RID: 10694 RVA: 0x000BA89C File Offset: 0x000B989C
		private void OnAuthenticate(WindowsAuthenticationEventArgs e)
		{
			if (this._eventHandler != null)
			{
				this._eventHandler(this, e);
			}
			if (e.Context.User == null)
			{
				if (e.User != null)
				{
					e.Context.User = e.User;
					return;
				}
				if (e.Identity == WindowsAuthenticationModule._anonymousIdentity)
				{
					e.Context.SetPrincipalNoDemand(WindowsAuthenticationModule._anonymousPrincipal, false);
					return;
				}
				e.Context.SetPrincipalNoDemand(new WindowsPrincipal(e.Identity), false);
			}
		}

		// Token: 0x060029C7 RID: 10695 RVA: 0x000BA91C File Offset: 0x000B991C
		private void OnEnter(object source, EventArgs eventArgs)
		{
			if (!WindowsAuthenticationModule.IsEnabled)
			{
				return;
			}
			HttpApplication httpApplication = (HttpApplication)source;
			HttpContext context = httpApplication.Context;
			WindowsIdentity windowsIdentity = null;
			if (HttpRuntime.UseIntegratedPipeline)
			{
				WindowsPrincipal windowsPrincipal = context.User as WindowsPrincipal;
				if (windowsPrincipal != null)
				{
					windowsIdentity = windowsPrincipal.Identity as WindowsIdentity;
					context.SetPrincipalNoDemand(null, false);
				}
			}
			else
			{
				string text = context.WorkerRequest.GetServerVariable("LOGON_USER");
				string text2 = context.WorkerRequest.GetServerVariable("AUTH_TYPE");
				if (text == null)
				{
					text = string.Empty;
				}
				if (text2 == null)
				{
					text2 = string.Empty;
				}
				if (text.Length == 0 && (text2.Length == 0 || StringUtil.EqualsIgnoreCase(text2, "basic")))
				{
					windowsIdentity = WindowsAuthenticationModule._anonymousIdentity;
				}
				else
				{
					windowsIdentity = new WindowsIdentity(context.WorkerRequest.GetUserToken(), text2, WindowsAccountType.Normal, true);
				}
			}
			if (windowsIdentity != null)
			{
				this.OnAuthenticate(new WindowsAuthenticationEventArgs(windowsIdentity, context));
			}
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x060029C8 RID: 10696 RVA: 0x000BA9F7 File Offset: 0x000B99F7
		internal static IPrincipal AnonymousPrincipal
		{
			get
			{
				return WindowsAuthenticationModule._anonymousPrincipal;
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x060029C9 RID: 10697 RVA: 0x000BAA00 File Offset: 0x000B9A00
		internal static bool IsEnabled
		{
			get
			{
				if (!WindowsAuthenticationModule._fAuthChecked)
				{
					AuthenticationSection authentication = RuntimeConfig.GetAppConfig().Authentication;
					authentication.ValidateAuthenticationMode();
					WindowsAuthenticationModule._fAuthRequired = authentication.Mode == AuthenticationMode.Windows;
					if (WindowsAuthenticationModule._fAuthRequired)
					{
						WindowsAuthenticationModule._anonymousIdentity = WindowsIdentity.GetAnonymous();
						WindowsAuthenticationModule._anonymousPrincipal = new WindowsPrincipal(WindowsAuthenticationModule._anonymousIdentity);
					}
					WindowsAuthenticationModule._fAuthChecked = true;
				}
				return WindowsAuthenticationModule._fAuthRequired;
			}
		}

		// Token: 0x04001F1D RID: 7965
		private WindowsAuthenticationEventHandler _eventHandler;

		// Token: 0x04001F1E RID: 7966
		private static bool _fAuthChecked;

		// Token: 0x04001F1F RID: 7967
		private static bool _fAuthRequired;

		// Token: 0x04001F20 RID: 7968
		private static WindowsIdentity _anonymousIdentity;

		// Token: 0x04001F21 RID: 7969
		private static WindowsPrincipal _anonymousPrincipal;
	}
}
