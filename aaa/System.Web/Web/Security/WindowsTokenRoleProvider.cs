using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Globalization;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web.Security
{
	// Token: 0x0200035B RID: 859
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WindowsTokenRoleProvider : RoleProvider
	{
		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x060029CA RID: 10698 RVA: 0x000BAA5E File Offset: 0x000B9A5E
		// (set) Token: 0x060029CB RID: 10699 RVA: 0x000BAA66 File Offset: 0x000B9A66
		public override string ApplicationName
		{
			get
			{
				return this._AppName;
			}
			set
			{
				this._AppName = value;
				if (this._AppName.Length > 256)
				{
					throw new ProviderException(SR.GetString("Provider_application_name_too_long"));
				}
			}
		}

		// Token: 0x060029CC RID: 10700 RVA: 0x000BAA94 File Offset: 0x000B9A94
		public override void Initialize(string name, NameValueCollection config)
		{
			if (string.IsNullOrEmpty(name))
			{
				name = "WindowsTokenProvider";
			}
			if (string.IsNullOrEmpty(config["description"]))
			{
				config.Remove("description");
				config.Add("description", SR.GetString("RoleWindowsTokenProvider_description"));
			}
			base.Initialize(name, config);
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}
			this._AppName = config["applicationName"];
			if (string.IsNullOrEmpty(this._AppName))
			{
				this._AppName = SecUtility.GetDefaultAppName();
			}
			if (this._AppName.Length > 256)
			{
				throw new ProviderException(SR.GetString("Provider_application_name_too_long"));
			}
			config.Remove("applicationName");
			if (config.Count > 0)
			{
				string key = config.GetKey(0);
				if (!string.IsNullOrEmpty(key))
				{
					throw new ProviderException(SR.GetString("Provider_unrecognized_attribute", new object[] { key }));
				}
			}
		}

		// Token: 0x060029CD RID: 10701 RVA: 0x000BAB84 File Offset: 0x000B9B84
		public bool IsUserInRole(string username, WindowsBuiltInRole role)
		{
			if (username == null)
			{
				throw new ArgumentNullException("username");
			}
			username = username.Trim();
			WindowsIdentity currentWindowsIdentityAndCheckName = this.GetCurrentWindowsIdentityAndCheckName(username);
			if (username.Length < 1)
			{
				return false;
			}
			WindowsPrincipal windowsPrincipal = new WindowsPrincipal(currentWindowsIdentityAndCheckName);
			return windowsPrincipal.IsInRole(role);
		}

		// Token: 0x060029CE RID: 10702 RVA: 0x000BABC8 File Offset: 0x000B9BC8
		public override bool IsUserInRole(string username, string roleName)
		{
			if (username == null)
			{
				throw new ArgumentNullException("username");
			}
			username = username.Trim();
			if (roleName == null)
			{
				throw new ArgumentNullException("roleName");
			}
			roleName = roleName.Trim();
			if (username.Length < 1)
			{
				return false;
			}
			StringBuilder stringBuilder = new StringBuilder(1024);
			IntPtr currentTokenAndCheckName = this.GetCurrentTokenAndCheckName(username);
			switch (UnsafeNativeMethods.IsUserInRole(currentTokenAndCheckName, roleName, stringBuilder, 1024))
			{
			case 0:
				return false;
			case 1:
				return true;
			default:
				throw new ProviderException(SR.GetString("API_failed_due_to_error", new object[] { stringBuilder.ToString() }));
			}
		}

		// Token: 0x060029CF RID: 10703 RVA: 0x000BAC64 File Offset: 0x000B9C64
		public override string[] GetRolesForUser(string username)
		{
			HttpRuntime.CheckAspNetHostingPermission(AspNetHostingPermissionLevel.Low, "API_not_supported_at_this_level");
			if (username == null)
			{
				throw new ArgumentNullException("username");
			}
			username = username.Trim();
			IntPtr currentTokenAndCheckName = this.GetCurrentTokenAndCheckName(username);
			if (username.Length < 1)
			{
				return new string[0];
			}
			StringBuilder stringBuilder = new StringBuilder(1024);
			StringBuilder stringBuilder2 = new StringBuilder(1024);
			int num = UnsafeNativeMethods.GetGroupsForUser(currentTokenAndCheckName, stringBuilder, 1024, stringBuilder2, 1024);
			if (num < 0)
			{
				stringBuilder = new StringBuilder(-num);
				num = UnsafeNativeMethods.GetGroupsForUser(currentTokenAndCheckName, stringBuilder, -num, stringBuilder2, 1024);
			}
			if (num <= 0)
			{
				throw new ProviderException(SR.GetString("API_failed_due_to_error", new object[] { stringBuilder2.ToString() }));
			}
			string[] array = stringBuilder.ToString().Split(new char[] { '\t' });
			return WindowsTokenRoleProvider.AddLocalGroupsWithoutDomainNames(array);
		}

		// Token: 0x060029D0 RID: 10704 RVA: 0x000BAD40 File Offset: 0x000B9D40
		private static string[] AddLocalGroupsWithoutDomainNames(string[] roles)
		{
			string machineName = WindowsTokenRoleProvider.GetMachineName();
			int length = machineName.Length;
			for (int i = 0; i < roles.Length; i++)
			{
				roles[i] = roles[i].Trim();
				if (roles[i].ToLower(CultureInfo.InvariantCulture).StartsWith(machineName, StringComparison.Ordinal))
				{
					roles[i] = roles[i].Substring(length);
				}
			}
			return roles;
		}

		// Token: 0x060029D1 RID: 10705 RVA: 0x000BAD96 File Offset: 0x000B9D96
		public override void CreateRole(string roleName)
		{
			throw new ProviderException(SR.GetString("Windows_Token_API_not_supported"));
		}

		// Token: 0x060029D2 RID: 10706 RVA: 0x000BADA7 File Offset: 0x000B9DA7
		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			throw new ProviderException(SR.GetString("Windows_Token_API_not_supported"));
		}

		// Token: 0x060029D3 RID: 10707 RVA: 0x000BADB8 File Offset: 0x000B9DB8
		public override bool RoleExists(string roleName)
		{
			throw new ProviderException(SR.GetString("Windows_Token_API_not_supported"));
		}

		// Token: 0x060029D4 RID: 10708 RVA: 0x000BADC9 File Offset: 0x000B9DC9
		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			throw new ProviderException(SR.GetString("Windows_Token_API_not_supported"));
		}

		// Token: 0x060029D5 RID: 10709 RVA: 0x000BADDA File Offset: 0x000B9DDA
		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			throw new ProviderException(SR.GetString("Windows_Token_API_not_supported"));
		}

		// Token: 0x060029D6 RID: 10710 RVA: 0x000BADEB File Offset: 0x000B9DEB
		public override string[] GetUsersInRole(string roleName)
		{
			throw new ProviderException(SR.GetString("Windows_Token_API_not_supported"));
		}

		// Token: 0x060029D7 RID: 10711 RVA: 0x000BADFC File Offset: 0x000B9DFC
		public override string[] GetAllRoles()
		{
			throw new ProviderException(SR.GetString("Windows_Token_API_not_supported"));
		}

		// Token: 0x060029D8 RID: 10712 RVA: 0x000BAE0D File Offset: 0x000B9E0D
		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			throw new ProviderException(SR.GetString("Windows_Token_API_not_supported"));
		}

		// Token: 0x060029D9 RID: 10713 RVA: 0x000BAE1E File Offset: 0x000B9E1E
		[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
		private IntPtr GetCurrentTokenAndCheckName(string userName)
		{
			return this.GetCurrentWindowsIdentityAndCheckName(userName).Token;
		}

		// Token: 0x060029DA RID: 10714 RVA: 0x000BAE2C File Offset: 0x000B9E2C
		[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
		private static string GetMachineName()
		{
			if (WindowsTokenRoleProvider._MachineName == null)
			{
				WindowsTokenRoleProvider._MachineName = (Environment.MachineName + "\\").ToLower(CultureInfo.InvariantCulture);
			}
			return WindowsTokenRoleProvider._MachineName;
		}

		// Token: 0x060029DB RID: 10715 RVA: 0x000BAE58 File Offset: 0x000B9E58
		private WindowsIdentity GetCurrentWindowsIdentityAndCheckName(string userName)
		{
			if (HostingEnvironment.IsHosted)
			{
				HttpContext httpContext = HttpContext.Current;
				if (httpContext == null || httpContext.User == null)
				{
					throw new ProviderException(SR.GetString("API_supported_for_current_user_only"));
				}
				if (!(httpContext.User.Identity is WindowsIdentity))
				{
					throw new ProviderException(SR.GetString("API_supported_for_current_user_only"));
				}
				if (!StringUtil.EqualsIgnoreCase(userName, httpContext.User.Identity.Name))
				{
					throw new ProviderException(SR.GetString("API_supported_for_current_user_only"));
				}
				return (WindowsIdentity)httpContext.User.Identity;
			}
			else
			{
				IPrincipal currentPrincipal = Thread.CurrentPrincipal;
				if (currentPrincipal == null || currentPrincipal.Identity == null || !(currentPrincipal.Identity is WindowsIdentity))
				{
					throw new ProviderException(SR.GetString("API_supported_for_current_user_only"));
				}
				if (!StringUtil.EqualsIgnoreCase(userName, currentPrincipal.Identity.Name))
				{
					throw new ProviderException(SR.GetString("API_supported_for_current_user_only"));
				}
				return (WindowsIdentity)currentPrincipal.Identity;
			}
		}

		// Token: 0x04001F22 RID: 7970
		private static string _MachineName;

		// Token: 0x04001F23 RID: 7971
		private string _AppName;
	}
}
