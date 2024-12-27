using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Security.Principal;
using System.Web.Configuration;
using System.Web.Management;

namespace System.Web.Security
{
	// Token: 0x02000357 RID: 855
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class UrlAuthorizationModule : IHttpModule
	{
		// Token: 0x060029B0 RID: 10672 RVA: 0x000BA4F4 File Offset: 0x000B94F4
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		public UrlAuthorizationModule()
		{
		}

		// Token: 0x060029B1 RID: 10673 RVA: 0x000BA4FC File Offset: 0x000B94FC
		public void Init(HttpApplication app)
		{
			app.AuthorizeRequest += this.OnEnter;
		}

		// Token: 0x060029B2 RID: 10674 RVA: 0x000BA510 File Offset: 0x000B9510
		public void Dispose()
		{
		}

		// Token: 0x060029B3 RID: 10675 RVA: 0x000BA514 File Offset: 0x000B9514
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		public static bool CheckUrlAccessForPrincipal(string virtualPath, IPrincipal user, string verb)
		{
			if (virtualPath == null)
			{
				throw new ArgumentNullException("virtualPath");
			}
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			if (verb == null)
			{
				throw new ArgumentNullException("verb");
			}
			verb = verb.Trim();
			VirtualPath virtualPath2 = VirtualPath.Create(virtualPath);
			if (!virtualPath2.IsWithinAppRoot)
			{
				throw new ArgumentException(SR.GetString("Virtual_path_outside_application_not_supported"), "virtualPath");
			}
			if (!UrlAuthorizationModule.s_EnabledDetermined)
			{
				if (!HttpRuntime.UseIntegratedPipeline)
				{
					HttpModulesSection httpModules = RuntimeConfig.GetConfig().HttpModules;
					int count = httpModules.Modules.Count;
					for (int i = 0; i < count; i++)
					{
						HttpModuleAction httpModuleAction = httpModules.Modules[i];
						if (Type.GetType(httpModuleAction.Type, false) == typeof(UrlAuthorizationModule))
						{
							UrlAuthorizationModule.s_Enabled = true;
							break;
						}
					}
				}
				else
				{
					List<ModuleConfigurationInfo> integratedModuleList = HttpApplication.IntegratedModuleList;
					foreach (ModuleConfigurationInfo moduleConfigurationInfo in integratedModuleList)
					{
						if (Type.GetType(moduleConfigurationInfo.Type, false) == typeof(UrlAuthorizationModule))
						{
							UrlAuthorizationModule.s_Enabled = true;
							break;
						}
					}
				}
				UrlAuthorizationModule.s_EnabledDetermined = true;
			}
			if (!UrlAuthorizationModule.s_Enabled)
			{
				return true;
			}
			AuthorizationSection authorization = RuntimeConfig.GetConfig(virtualPath2).Authorization;
			return authorization.EveryoneAllowed || authorization.IsUserAllowed(user, verb);
		}

		// Token: 0x060029B4 RID: 10676 RVA: 0x000BA674 File Offset: 0x000B9674
		private void OnEnter(object source, EventArgs eventArgs)
		{
			HttpApplication httpApplication = (HttpApplication)source;
			HttpContext context = httpApplication.Context;
			if (context.SkipAuthorization)
			{
				if (context.User == null || !context.User.Identity.IsAuthenticated)
				{
					PerfCounters.IncrementCounter(AppPerfCounter.ANONYMOUS_REQUESTS);
				}
				return;
			}
			AuthorizationSection authorization = RuntimeConfig.GetConfig(context).Authorization;
			if (!authorization.EveryoneAllowed && !authorization.IsUserAllowed(context.User, context.Request.RequestType))
			{
				context.Response.StatusCode = 401;
				this.WriteErrorMessage(context);
				if (context.User != null && context.User.Identity.IsAuthenticated)
				{
					WebBaseEvent.RaiseSystemEvent(this, 4007);
				}
				httpApplication.CompleteRequest();
				return;
			}
			if (context.User == null || !context.User.Identity.IsAuthenticated)
			{
				PerfCounters.IncrementCounter(AppPerfCounter.ANONYMOUS_REQUESTS);
			}
			WebBaseEvent.RaiseSystemEvent(this, 4003);
		}

		// Token: 0x060029B5 RID: 10677 RVA: 0x000BA755 File Offset: 0x000B9755
		private void WriteErrorMessage(HttpContext context)
		{
			context.Response.Write(UrlAuthFailedErrorFormatter.GetErrorText());
			context.Response.GenerateResponseHeadersForHandler();
		}

		// Token: 0x060029B6 RID: 10678 RVA: 0x000BA774 File Offset: 0x000B9774
		internal static bool RequestRequiresAuthorization(HttpContext context)
		{
			if (context.SkipAuthorization)
			{
				return false;
			}
			AuthorizationSection authorization = RuntimeConfig.GetConfig(context).Authorization;
			if (UrlAuthorizationModule._AnonUser == null)
			{
				UrlAuthorizationModule._AnonUser = new GenericPrincipal(new GenericIdentity(string.Empty, string.Empty), new string[0]);
			}
			return !authorization.IsUserAllowed(UrlAuthorizationModule._AnonUser, context.Request.RequestType);
		}

		// Token: 0x060029B7 RID: 10679 RVA: 0x000BA7D8 File Offset: 0x000B97D8
		internal static bool IsUserAllowedToPath(HttpContext context, VirtualPath virtualPath)
		{
			AuthorizationSection authorization = RuntimeConfig.GetConfig(context, virtualPath).Authorization;
			return authorization.EveryoneAllowed || authorization.IsUserAllowed(context.User, context.Request.RequestType);
		}

		// Token: 0x04001F17 RID: 7959
		private static bool s_EnabledDetermined;

		// Token: 0x04001F18 RID: 7960
		private static bool s_Enabled;

		// Token: 0x04001F19 RID: 7961
		private static GenericPrincipal _AnonUser;
	}
}
