using System;
using System.Security.Permissions;
using System.Security.Principal;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.Management;

namespace System.Web.Security
{
	// Token: 0x02000331 RID: 817
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class FileAuthorizationModule : IHttpModule
	{
		// Token: 0x060027FD RID: 10237 RVA: 0x000AF761 File Offset: 0x000AE761
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		public FileAuthorizationModule()
		{
		}

		// Token: 0x060027FE RID: 10238 RVA: 0x000AF76C File Offset: 0x000AE76C
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		public static bool CheckFileAccessForUser(string virtualPath, IntPtr token, string verb)
		{
			if (virtualPath == null)
			{
				throw new ArgumentNullException("virtualPath");
			}
			if (token == IntPtr.Zero)
			{
				throw new ArgumentNullException("token");
			}
			if (verb == null)
			{
				throw new ArgumentNullException("verb");
			}
			VirtualPath virtualPath2 = VirtualPath.Create(virtualPath);
			if (!virtualPath2.IsWithinAppRoot)
			{
				throw new ArgumentException(SR.GetString("Virtual_path_outside_application_not_supported"), "virtualPath");
			}
			if (!FileAuthorizationModule.s_EnabledDetermined)
			{
				if (HttpRuntime.UseIntegratedPipeline)
				{
					FileAuthorizationModule.s_Enabled = true;
				}
				else
				{
					HttpModulesSection httpModules = RuntimeConfig.GetConfig().HttpModules;
					int count = httpModules.Modules.Count;
					for (int i = 0; i < count; i++)
					{
						HttpModuleAction httpModuleAction = httpModules.Modules[i];
						if (Type.GetType(httpModuleAction.Type, false) == typeof(FileAuthorizationModule))
						{
							FileAuthorizationModule.s_Enabled = true;
							break;
						}
					}
				}
				FileAuthorizationModule.s_EnabledDetermined = true;
			}
			if (!FileAuthorizationModule.s_Enabled)
			{
				return true;
			}
			bool flag;
			FileSecurityDescriptorWrapper fileSecurityDescriptorWrapper = FileAuthorizationModule.GetFileSecurityDescriptorWrapper(virtualPath2.MapPath(), out flag);
			int num = 3;
			if (verb == "GET" || verb == "POST" || verb == "HEAD" || verb == "OPTIONS")
			{
				num = 1;
			}
			bool flag2 = fileSecurityDescriptorWrapper.IsAccessAllowed(token, num);
			if (flag)
			{
				fileSecurityDescriptorWrapper.FreeSecurityDescriptor();
			}
			return flag2;
		}

		// Token: 0x060027FF RID: 10239 RVA: 0x000AF8AA File Offset: 0x000AE8AA
		public void Init(HttpApplication app)
		{
			app.AuthorizeRequest += this.OnEnter;
		}

		// Token: 0x06002800 RID: 10240 RVA: 0x000AF8BE File Offset: 0x000AE8BE
		public void Dispose()
		{
		}

		// Token: 0x06002801 RID: 10241 RVA: 0x000AF8C0 File Offset: 0x000AE8C0
		private void OnEnter(object source, EventArgs eventArgs)
		{
			if (HttpRuntime.IsOnUNCShareInternal)
			{
				return;
			}
			HttpApplication httpApplication = (HttpApplication)source;
			HttpContext context = httpApplication.Context;
			if (!FileAuthorizationModule.IsUserAllowedToFile(context, null))
			{
				context.Response.StatusCode = 401;
				this.WriteErrorMessage(context);
				httpApplication.CompleteRequest();
			}
		}

		// Token: 0x06002802 RID: 10242 RVA: 0x000AF909 File Offset: 0x000AE909
		internal static bool IsWindowsIdentity(HttpContext context)
		{
			return context.User != null && context.User.Identity != null && context.User.Identity is WindowsIdentity;
		}

		// Token: 0x06002803 RID: 10243 RVA: 0x000AF938 File Offset: 0x000AE938
		internal static bool IsUserAllowedToFile(HttpContext context, string fileName)
		{
			if (!FileAuthorizationModule.IsWindowsIdentity(context))
			{
				return true;
			}
			if (fileName == null)
			{
				fileName = context.Request.PhysicalPathInternal;
			}
			int num = 3;
			if (context.Request.HttpVerb == HttpVerb.HEAD || context.Request.HttpVerb == HttpVerb.GET || context.Request.HttpVerb == HttpVerb.POST || context.Request.HttpMethod == "OPTIONS")
			{
				num = 1;
			}
			bool flag;
			FileSecurityDescriptorWrapper fileSecurityDescriptorWrapper = FileAuthorizationModule.GetFileSecurityDescriptorWrapper(fileName, out flag);
			bool flag2;
			if (fileSecurityDescriptorWrapper._AnonymousAccessChecked && (context.User == null || !context.User.Identity.IsAuthenticated))
			{
				flag2 = fileSecurityDescriptorWrapper._AnonymousAccess;
			}
			else
			{
				flag2 = fileSecurityDescriptorWrapper.IsAccessAllowed(context.WorkerRequest.GetUserToken(), num);
			}
			if (!fileSecurityDescriptorWrapper._AnonymousAccessChecked && (context.User == null || !context.User.Identity.IsAuthenticated))
			{
				fileSecurityDescriptorWrapper._AnonymousAccess = flag2;
				fileSecurityDescriptorWrapper._AnonymousAccessChecked = true;
			}
			if (flag)
			{
				fileSecurityDescriptorWrapper.FreeSecurityDescriptor();
			}
			if (flag2)
			{
				WebBaseEvent.RaiseSystemEvent(null, 4004);
			}
			else if (context.User != null && context.User.Identity.IsAuthenticated)
			{
				WebBaseEvent.RaiseSystemEvent(null, 4008);
			}
			return flag2;
		}

		// Token: 0x06002804 RID: 10244 RVA: 0x000AFA5C File Offset: 0x000AEA5C
		internal static FileSecurityDescriptorWrapper GetFileSecurityDescriptorWrapper(string fileName, out bool freeDescriptor)
		{
			freeDescriptor = false;
			string text = "h" + fileName;
			FileSecurityDescriptorWrapper fileSecurityDescriptorWrapper = HttpRuntime.CacheInternal.Get(text) as FileSecurityDescriptorWrapper;
			if (fileSecurityDescriptorWrapper == null)
			{
				fileSecurityDescriptorWrapper = new FileSecurityDescriptorWrapper(fileName);
				if (fileSecurityDescriptorWrapper.IsSecurityDescriptorValid())
				{
					try
					{
						CacheDependency cacheDependency = new CacheDependency(0, fileName);
						HttpRuntime.CacheInternal.UtcInsert(text, fileSecurityDescriptorWrapper, cacheDependency, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, new CacheItemRemovedCallback(fileSecurityDescriptorWrapper.OnCacheItemRemoved));
					}
					catch
					{
						freeDescriptor = true;
					}
				}
			}
			return fileSecurityDescriptorWrapper;
		}

		// Token: 0x06002805 RID: 10245 RVA: 0x000AFAE0 File Offset: 0x000AEAE0
		private void WriteErrorMessage(HttpContext context)
		{
			if (!context.IsCustomErrorEnabled)
			{
				context.Response.Write(new FileAccessFailedErrorFormatter(context.Request.PhysicalPathInternal).GetErrorMessage(context, false));
			}
			else
			{
				context.Response.Write(new FileAccessFailedErrorFormatter(null).GetErrorMessage(context, true));
			}
			context.Response.GenerateResponseHeadersForHandler();
		}

		// Token: 0x06002806 RID: 10246 RVA: 0x000AFB3C File Offset: 0x000AEB3C
		internal static bool RequestRequiresAuthorization(HttpContext context)
		{
			if (!FileAuthorizationModule.IsWindowsIdentity(context))
			{
				return false;
			}
			string text = "h" + context.Request.PhysicalPathInternal;
			object obj = HttpRuntime.CacheInternal.Get(text);
			if (obj == null || !(obj is FileSecurityDescriptorWrapper))
			{
				return true;
			}
			FileSecurityDescriptorWrapper fileSecurityDescriptorWrapper = (FileSecurityDescriptorWrapper)obj;
			return !fileSecurityDescriptorWrapper._AnonymousAccessChecked || !fileSecurityDescriptorWrapper._AnonymousAccess;
		}

		// Token: 0x06002807 RID: 10247 RVA: 0x000AFB9C File Offset: 0x000AEB9C
		internal static bool IsUserAllowedToPath(HttpContext context, VirtualPath virtualPath)
		{
			return FileAuthorizationModule.IsUserAllowedToFile(context, virtualPath.MapPath());
		}

		// Token: 0x04001E82 RID: 7810
		private static bool s_EnabledDetermined;

		// Token: 0x04001E83 RID: 7811
		private static bool s_Enabled;
	}
}
