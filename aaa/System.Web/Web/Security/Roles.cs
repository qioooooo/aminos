using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Globalization;
using System.Security.Principal;
using System.Threading;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web.Security
{
	// Token: 0x02000353 RID: 851
	public static class Roles
	{
		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06002945 RID: 10565 RVA: 0x000B4F80 File Offset: 0x000B3F80
		public static RoleProvider Provider
		{
			get
			{
				Roles.EnsureEnabled();
				return Roles.s_Provider;
			}
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06002946 RID: 10566 RVA: 0x000B4F8C File Offset: 0x000B3F8C
		public static RoleProviderCollection Providers
		{
			get
			{
				Roles.EnsureEnabled();
				return Roles.s_Providers;
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06002947 RID: 10567 RVA: 0x000B4F98 File Offset: 0x000B3F98
		public static string CookieName
		{
			get
			{
				Roles.Initialize();
				return Roles.s_CookieName;
			}
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06002948 RID: 10568 RVA: 0x000B4FA4 File Offset: 0x000B3FA4
		public static bool CacheRolesInCookie
		{
			get
			{
				Roles.Initialize();
				return Roles.s_CacheRolesInCookie;
			}
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06002949 RID: 10569 RVA: 0x000B4FB0 File Offset: 0x000B3FB0
		public static int CookieTimeout
		{
			get
			{
				Roles.Initialize();
				return Roles.s_CookieTimeout;
			}
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x0600294A RID: 10570 RVA: 0x000B4FBC File Offset: 0x000B3FBC
		public static string CookiePath
		{
			get
			{
				Roles.Initialize();
				return Roles.s_CookiePath;
			}
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x0600294B RID: 10571 RVA: 0x000B4FC8 File Offset: 0x000B3FC8
		public static bool CookieRequireSSL
		{
			get
			{
				Roles.Initialize();
				return Roles.s_CookieRequireSSL;
			}
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x0600294C RID: 10572 RVA: 0x000B4FD4 File Offset: 0x000B3FD4
		public static bool CookieSlidingExpiration
		{
			get
			{
				Roles.Initialize();
				return Roles.s_CookieSlidingExpiration;
			}
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x0600294D RID: 10573 RVA: 0x000B4FE0 File Offset: 0x000B3FE0
		public static CookieProtection CookieProtectionValue
		{
			get
			{
				Roles.Initialize();
				return Roles.s_CookieProtection;
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x0600294E RID: 10574 RVA: 0x000B4FEC File Offset: 0x000B3FEC
		public static bool CreatePersistentCookie
		{
			get
			{
				Roles.Initialize();
				return Roles.s_CreatePersistentCookie;
			}
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x0600294F RID: 10575 RVA: 0x000B4FF8 File Offset: 0x000B3FF8
		public static string Domain
		{
			get
			{
				Roles.Initialize();
				return Roles.s_Domain;
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06002950 RID: 10576 RVA: 0x000B5004 File Offset: 0x000B4004
		public static int MaxCachedResults
		{
			get
			{
				Roles.Initialize();
				return Roles.s_MaxCachedResults;
			}
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06002951 RID: 10577 RVA: 0x000B5010 File Offset: 0x000B4010
		public static bool Enabled
		{
			get
			{
				if (HostingEnvironment.IsHosted && !HttpRuntime.HasAspNetHostingPermission(AspNetHostingPermissionLevel.Low))
				{
					return false;
				}
				if (!Roles.s_Initialized && !Roles.s_EnabledSet)
				{
					RoleManagerSection roleManager = RuntimeConfig.GetAppConfig().RoleManager;
					Roles.s_Enabled = roleManager.Enabled;
					Roles.s_EnabledSet = true;
				}
				return Roles.s_Enabled;
			}
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06002952 RID: 10578 RVA: 0x000B5061 File Offset: 0x000B4061
		// (set) Token: 0x06002953 RID: 10579 RVA: 0x000B506D File Offset: 0x000B406D
		public static string ApplicationName
		{
			get
			{
				return Roles.Provider.ApplicationName;
			}
			set
			{
				Roles.Provider.ApplicationName = value;
			}
		}

		// Token: 0x06002954 RID: 10580 RVA: 0x000B507C File Offset: 0x000B407C
		public static bool IsUserInRole(string username, string roleName)
		{
			if (HostingEnvironment.IsHosted && EtwTrace.IsTraceEnabled(4, 8))
			{
				EtwTrace.Trace(EtwTraceType.ETW_TYPE_ROLE_BEGIN, HttpContext.Current.WorkerRequest);
			}
			Roles.EnsureEnabled();
			bool flag = false;
			bool flag2 = false;
			bool flag3;
			try
			{
				SecUtility.CheckParameter(ref roleName, true, true, true, 0, "roleName");
				SecUtility.CheckParameter(ref username, true, false, true, 0, "username");
				if (username.Length < 1)
				{
					flag3 = false;
				}
				else
				{
					IPrincipal currentUser = Roles.GetCurrentUser();
					if (currentUser != null && currentUser is RolePrincipal && ((RolePrincipal)currentUser).ProviderName == Roles.Provider.Name && StringUtil.EqualsIgnoreCase(username, currentUser.Identity.Name))
					{
						flag = currentUser.IsInRole(roleName);
					}
					else
					{
						flag = Roles.Provider.IsUserInRole(username, roleName);
					}
					flag3 = flag;
				}
			}
			finally
			{
				if (HostingEnvironment.IsHosted && EtwTrace.IsTraceEnabled(4, 8))
				{
					if (EtwTrace.IsTraceEnabled(5, 8))
					{
						string @string = SR.Resources.GetString(flag ? "Etw_Success" : "Etw_Failure", CultureInfo.InstalledUICulture);
						EtwTrace.Trace(EtwTraceType.ETW_TYPE_ROLE_IS_USER_IN_ROLE, HttpContext.Current.WorkerRequest, flag2 ? "RolePrincipal" : Roles.Provider.GetType().FullName, username, roleName, @string);
					}
					EtwTrace.Trace(EtwTraceType.ETW_TYPE_ROLE_END, HttpContext.Current.WorkerRequest, flag2 ? "RolePrincipal" : Roles.Provider.GetType().FullName, username);
				}
			}
			return flag3;
		}

		// Token: 0x06002955 RID: 10581 RVA: 0x000B51EC File Offset: 0x000B41EC
		public static bool IsUserInRole(string roleName)
		{
			return Roles.IsUserInRole(Roles.GetCurrentUserName(), roleName);
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x000B51FC File Offset: 0x000B41FC
		public static string[] GetRolesForUser(string username)
		{
			if (HostingEnvironment.IsHosted && EtwTrace.IsTraceEnabled(4, 8))
			{
				EtwTrace.Trace(EtwTraceType.ETW_TYPE_ROLE_BEGIN, HttpContext.Current.WorkerRequest);
			}
			Roles.EnsureEnabled();
			string[] array = null;
			bool flag = false;
			string[] array2;
			try
			{
				SecUtility.CheckParameter(ref username, true, false, true, 0, "username");
				if (username.Length < 1)
				{
					array = new string[0];
					array2 = array;
				}
				else
				{
					IPrincipal currentUser = Roles.GetCurrentUser();
					if (currentUser != null && currentUser is RolePrincipal && ((RolePrincipal)currentUser).ProviderName == Roles.Provider.Name && StringUtil.EqualsIgnoreCase(username, currentUser.Identity.Name))
					{
						array = ((RolePrincipal)currentUser).GetRoles();
						flag = true;
					}
					else
					{
						array = Roles.Provider.GetRolesForUser(username);
					}
					array2 = array;
				}
			}
			finally
			{
				if (HostingEnvironment.IsHosted && EtwTrace.IsTraceEnabled(4, 8))
				{
					if (EtwTrace.IsTraceEnabled(5, 8))
					{
						string text = null;
						if (array != null && array.Length > 0)
						{
							text = array[0];
						}
						for (int i = 1; i < array.Length; i++)
						{
							text = text + "," + array[i];
						}
						EtwTrace.Trace(EtwTraceType.ETW_TYPE_ROLE_GET_USER_ROLES, HttpContext.Current.WorkerRequest, flag ? "RolePrincipal" : Roles.Provider.GetType().FullName, username, text, null);
					}
					EtwTrace.Trace(EtwTraceType.ETW_TYPE_ROLE_END, HttpContext.Current.WorkerRequest, flag ? "RolePrincipal" : Roles.Provider.GetType().FullName, username);
				}
			}
			return array2;
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x000B537C File Offset: 0x000B437C
		public static string[] GetRolesForUser()
		{
			return Roles.GetRolesForUser(Roles.GetCurrentUserName());
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x000B5388 File Offset: 0x000B4388
		public static string[] GetUsersInRole(string roleName)
		{
			Roles.EnsureEnabled();
			SecUtility.CheckParameter(ref roleName, true, true, true, 0, "roleName");
			return Roles.Provider.GetUsersInRole(roleName);
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x000B53AA File Offset: 0x000B43AA
		public static void CreateRole(string roleName)
		{
			Roles.EnsureEnabled();
			SecUtility.CheckParameter(ref roleName, true, true, true, 0, "roleName");
			Roles.Provider.CreateRole(roleName);
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x000B53CC File Offset: 0x000B43CC
		public static bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			Roles.EnsureEnabled();
			SecUtility.CheckParameter(ref roleName, true, true, true, 0, "roleName");
			bool flag = Roles.Provider.DeleteRole(roleName, throwOnPopulatedRole);
			try
			{
				RolePrincipal rolePrincipal = Roles.GetCurrentUser() as RolePrincipal;
				if (rolePrincipal != null && rolePrincipal.ProviderName == Roles.Provider.Name && rolePrincipal.IsRoleListCached && rolePrincipal.IsInRole(roleName))
				{
					rolePrincipal.SetDirty();
				}
			}
			catch
			{
			}
			return flag;
		}

		// Token: 0x0600295B RID: 10587 RVA: 0x000B5450 File Offset: 0x000B4450
		public static bool DeleteRole(string roleName)
		{
			return Roles.DeleteRole(roleName, true);
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x000B5459 File Offset: 0x000B4459
		public static bool RoleExists(string roleName)
		{
			Roles.EnsureEnabled();
			SecUtility.CheckParameter(ref roleName, true, true, true, 0, "roleName");
			return Roles.Provider.RoleExists(roleName);
		}

		// Token: 0x0600295D RID: 10589 RVA: 0x000B547C File Offset: 0x000B447C
		public static void AddUserToRole(string username, string roleName)
		{
			Roles.EnsureEnabled();
			SecUtility.CheckParameter(ref roleName, true, true, true, 0, "roleName");
			SecUtility.CheckParameter(ref username, true, true, true, 0, "username");
			Roles.Provider.AddUsersToRoles(new string[] { username }, new string[] { roleName });
			try
			{
				RolePrincipal rolePrincipal = Roles.GetCurrentUser() as RolePrincipal;
				if (rolePrincipal != null && rolePrincipal.ProviderName == Roles.Provider.Name && rolePrincipal.IsRoleListCached && StringUtil.EqualsIgnoreCase(rolePrincipal.Identity.Name, username))
				{
					rolePrincipal.SetDirty();
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600295E RID: 10590 RVA: 0x000B552C File Offset: 0x000B452C
		public static void AddUserToRoles(string username, string[] roleNames)
		{
			Roles.EnsureEnabled();
			SecUtility.CheckParameter(ref username, true, true, true, 0, "username");
			SecUtility.CheckArrayParameter(ref roleNames, true, true, true, 0, "roleNames");
			Roles.Provider.AddUsersToRoles(new string[] { username }, roleNames);
			try
			{
				RolePrincipal rolePrincipal = Roles.GetCurrentUser() as RolePrincipal;
				if (rolePrincipal != null && rolePrincipal.ProviderName == Roles.Provider.Name && rolePrincipal.IsRoleListCached && StringUtil.EqualsIgnoreCase(rolePrincipal.Identity.Name, username))
				{
					rolePrincipal.SetDirty();
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600295F RID: 10591 RVA: 0x000B55D0 File Offset: 0x000B45D0
		public static void AddUsersToRole(string[] usernames, string roleName)
		{
			Roles.EnsureEnabled();
			SecUtility.CheckParameter(ref roleName, true, true, true, 0, "roleName");
			SecUtility.CheckArrayParameter(ref usernames, true, true, true, 0, "usernames");
			Roles.Provider.AddUsersToRoles(usernames, new string[] { roleName });
			try
			{
				RolePrincipal rolePrincipal = Roles.GetCurrentUser() as RolePrincipal;
				if (rolePrincipal != null && rolePrincipal.ProviderName == Roles.Provider.Name && rolePrincipal.IsRoleListCached)
				{
					foreach (string text in usernames)
					{
						if (StringUtil.EqualsIgnoreCase(rolePrincipal.Identity.Name, text))
						{
							rolePrincipal.SetDirty();
							break;
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06002960 RID: 10592 RVA: 0x000B5690 File Offset: 0x000B4690
		public static void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			Roles.EnsureEnabled();
			SecUtility.CheckArrayParameter(ref roleNames, true, true, true, 0, "roleNames");
			SecUtility.CheckArrayParameter(ref usernames, true, true, true, 0, "usernames");
			Roles.Provider.AddUsersToRoles(usernames, roleNames);
			try
			{
				RolePrincipal rolePrincipal = Roles.GetCurrentUser() as RolePrincipal;
				if (rolePrincipal != null && rolePrincipal.ProviderName == Roles.Provider.Name && rolePrincipal.IsRoleListCached)
				{
					foreach (string text in usernames)
					{
						if (StringUtil.EqualsIgnoreCase(rolePrincipal.Identity.Name, text))
						{
							rolePrincipal.SetDirty();
							break;
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06002961 RID: 10593 RVA: 0x000B5740 File Offset: 0x000B4740
		public static void RemoveUserFromRole(string username, string roleName)
		{
			Roles.EnsureEnabled();
			SecUtility.CheckParameter(ref roleName, true, true, true, 0, "roleName");
			SecUtility.CheckParameter(ref username, true, true, true, 0, "username");
			Roles.Provider.RemoveUsersFromRoles(new string[] { username }, new string[] { roleName });
			try
			{
				RolePrincipal rolePrincipal = Roles.GetCurrentUser() as RolePrincipal;
				if (rolePrincipal != null && rolePrincipal.ProviderName == Roles.Provider.Name && rolePrincipal.IsRoleListCached && StringUtil.EqualsIgnoreCase(rolePrincipal.Identity.Name, username))
				{
					rolePrincipal.SetDirty();
				}
			}
			catch
			{
			}
		}

		// Token: 0x06002962 RID: 10594 RVA: 0x000B57F0 File Offset: 0x000B47F0
		public static void RemoveUserFromRoles(string username, string[] roleNames)
		{
			Roles.EnsureEnabled();
			SecUtility.CheckParameter(ref username, true, true, true, 0, "username");
			SecUtility.CheckArrayParameter(ref roleNames, true, true, true, 0, "roleNames");
			Roles.Provider.RemoveUsersFromRoles(new string[] { username }, roleNames);
			try
			{
				RolePrincipal rolePrincipal = Roles.GetCurrentUser() as RolePrincipal;
				if (rolePrincipal != null && rolePrincipal.ProviderName == Roles.Provider.Name && rolePrincipal.IsRoleListCached && StringUtil.EqualsIgnoreCase(rolePrincipal.Identity.Name, username))
				{
					rolePrincipal.SetDirty();
				}
			}
			catch
			{
			}
		}

		// Token: 0x06002963 RID: 10595 RVA: 0x000B5894 File Offset: 0x000B4894
		public static void RemoveUsersFromRole(string[] usernames, string roleName)
		{
			Roles.EnsureEnabled();
			SecUtility.CheckParameter(ref roleName, true, true, true, 0, "roleName");
			SecUtility.CheckArrayParameter(ref usernames, true, true, true, 0, "usernames");
			Roles.Provider.RemoveUsersFromRoles(usernames, new string[] { roleName });
			try
			{
				RolePrincipal rolePrincipal = Roles.GetCurrentUser() as RolePrincipal;
				if (rolePrincipal != null && rolePrincipal.ProviderName == Roles.Provider.Name && rolePrincipal.IsRoleListCached)
				{
					foreach (string text in usernames)
					{
						if (StringUtil.EqualsIgnoreCase(rolePrincipal.Identity.Name, text))
						{
							rolePrincipal.SetDirty();
							break;
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06002964 RID: 10596 RVA: 0x000B5954 File Offset: 0x000B4954
		public static void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			Roles.EnsureEnabled();
			SecUtility.CheckArrayParameter(ref roleNames, true, true, true, 0, "roleNames");
			SecUtility.CheckArrayParameter(ref usernames, true, true, true, 0, "usernames");
			Roles.Provider.RemoveUsersFromRoles(usernames, roleNames);
			try
			{
				RolePrincipal rolePrincipal = Roles.GetCurrentUser() as RolePrincipal;
				if (rolePrincipal != null && rolePrincipal.ProviderName == Roles.Provider.Name && rolePrincipal.IsRoleListCached)
				{
					foreach (string text in usernames)
					{
						if (StringUtil.EqualsIgnoreCase(rolePrincipal.Identity.Name, text))
						{
							rolePrincipal.SetDirty();
							break;
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06002965 RID: 10597 RVA: 0x000B5A04 File Offset: 0x000B4A04
		public static string[] GetAllRoles()
		{
			Roles.EnsureEnabled();
			return Roles.Provider.GetAllRoles();
		}

		// Token: 0x06002966 RID: 10598 RVA: 0x000B5A18 File Offset: 0x000B4A18
		public static void DeleteCookie()
		{
			Roles.EnsureEnabled();
			if (Roles.CookieName == null || Roles.CookieName.Length < 1)
			{
				return;
			}
			HttpContext httpContext = HttpContext.Current;
			if (httpContext == null || !httpContext.Request.Browser.Cookies)
			{
				return;
			}
			string text = string.Empty;
			if (httpContext.Request.Browser["supportsEmptyStringInCookieValue"] == "false")
			{
				text = "NoCookie";
			}
			HttpCookie httpCookie = new HttpCookie(Roles.CookieName, text);
			httpCookie.HttpOnly = true;
			httpCookie.Path = Roles.CookiePath;
			httpCookie.Domain = Roles.Domain;
			httpCookie.Expires = new DateTime(1999, 10, 12);
			httpCookie.Secure = Roles.CookieRequireSSL;
			httpContext.Response.Cookies.RemoveCookie(Roles.CookieName);
			httpContext.Response.Cookies.Add(httpCookie);
		}

		// Token: 0x06002967 RID: 10599 RVA: 0x000B5AF6 File Offset: 0x000B4AF6
		public static string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			Roles.EnsureEnabled();
			SecUtility.CheckParameter(ref roleName, true, true, true, 0, "roleName");
			SecUtility.CheckParameter(ref usernameToMatch, true, true, false, 0, "usernameToMatch");
			return Roles.Provider.FindUsersInRole(roleName, usernameToMatch);
		}

		// Token: 0x06002968 RID: 10600 RVA: 0x000B5B29 File Offset: 0x000B4B29
		private static void EnsureEnabled()
		{
			Roles.Initialize();
			if (!Roles.s_Enabled)
			{
				throw new ProviderException(SR.GetString("Roles_feature_not_enabled"));
			}
		}

		// Token: 0x06002969 RID: 10601 RVA: 0x000B5B48 File Offset: 0x000B4B48
		private static void Initialize()
		{
			if (!Roles.s_Initialized)
			{
				lock (Roles.s_lock)
				{
					if (Roles.s_Initialized)
					{
						if (Roles.s_InitializeException != null)
						{
							throw Roles.s_InitializeException;
						}
						return;
					}
					else
					{
						try
						{
							if (HostingEnvironment.IsHosted)
							{
								HttpRuntime.CheckAspNetHostingPermission(AspNetHostingPermissionLevel.Low, "Feature_not_supported_at_this_level");
							}
							RoleManagerSection roleManager = RuntimeConfig.GetAppConfig().RoleManager;
							Roles.s_Enabled = roleManager.Enabled;
							Roles.s_CookieName = roleManager.CookieName;
							Roles.s_CacheRolesInCookie = roleManager.CacheRolesInCookie;
							Roles.s_CookieTimeout = (int)roleManager.CookieTimeout.TotalMinutes;
							Roles.s_CookiePath = roleManager.CookiePath;
							Roles.s_CookieRequireSSL = roleManager.CookieRequireSSL;
							Roles.s_CookieSlidingExpiration = roleManager.CookieSlidingExpiration;
							Roles.s_CookieProtection = roleManager.CookieProtection;
							Roles.s_Domain = roleManager.Domain;
							Roles.s_CreatePersistentCookie = roleManager.CreatePersistentCookie;
							Roles.s_MaxCachedResults = roleManager.MaxCachedResults;
							if (Roles.s_Enabled)
							{
								if (Roles.s_MaxCachedResults < 0)
								{
									throw new ProviderException(SR.GetString("Value_must_be_non_negative_integer", new object[] { "maxCachedResults" }));
								}
								Roles.s_Providers = new RoleProviderCollection();
								if (HostingEnvironment.IsHosted)
								{
									ProvidersHelper.InstantiateProviders(roleManager.Providers, Roles.s_Providers, typeof(RoleProvider));
								}
								else
								{
									foreach (object obj2 in roleManager.Providers)
									{
										ProviderSettings providerSettings = (ProviderSettings)obj2;
										Type type = Type.GetType(providerSettings.Type, true, true);
										if (!typeof(RoleProvider).IsAssignableFrom(type))
										{
											throw new ArgumentException(SR.GetString("Provider_must_implement_type", new object[] { typeof(RoleProvider).ToString() }));
										}
										RoleProvider roleProvider = (RoleProvider)Activator.CreateInstance(type);
										NameValueCollection parameters = providerSettings.Parameters;
										NameValueCollection nameValueCollection = new NameValueCollection(parameters.Count, StringComparer.Ordinal);
										foreach (object obj3 in parameters)
										{
											string text = (string)obj3;
											nameValueCollection[text] = parameters[text];
										}
										roleProvider.Initialize(providerSettings.Name, nameValueCollection);
										Roles.s_Providers.Add(roleProvider);
									}
								}
								Roles.s_Providers.SetReadOnly();
								if (roleManager.DefaultProvider == null)
								{
									Roles.s_InitializeException = new ProviderException(SR.GetString("Def_role_provider_not_specified"));
								}
								else
								{
									try
									{
										Roles.s_Provider = Roles.s_Providers[roleManager.DefaultProvider];
									}
									catch
									{
									}
								}
								if (Roles.s_Provider == null)
								{
									Roles.s_InitializeException = new ConfigurationErrorsException(SR.GetString("Def_role_provider_not_found"), roleManager.ElementInformation.Properties["defaultProvider"].Source, roleManager.ElementInformation.Properties["defaultProvider"].LineNumber);
								}
							}
						}
						catch (Exception ex)
						{
							Roles.s_InitializeException = ex;
						}
						Roles.s_Initialized = true;
					}
				}
				if (Roles.s_InitializeException != null)
				{
					throw Roles.s_InitializeException;
				}
				return;
			}
			if (Roles.s_InitializeException != null)
			{
				throw Roles.s_InitializeException;
			}
		}

		// Token: 0x0600296A RID: 10602 RVA: 0x000B5EEC File Offset: 0x000B4EEC
		private static string GetCurrentUserName()
		{
			IPrincipal currentUser = Roles.GetCurrentUser();
			if (currentUser == null || currentUser.Identity == null)
			{
				return string.Empty;
			}
			return currentUser.Identity.Name;
		}

		// Token: 0x0600296B RID: 10603 RVA: 0x000B5F1C File Offset: 0x000B4F1C
		private static IPrincipal GetCurrentUser()
		{
			if (HostingEnvironment.IsHosted)
			{
				HttpContext httpContext = HttpContext.Current;
				if (httpContext != null)
				{
					return httpContext.User;
				}
			}
			return Thread.CurrentPrincipal;
		}

		// Token: 0x04001EF3 RID: 7923
		private static RoleProvider s_Provider;

		// Token: 0x04001EF4 RID: 7924
		private static bool s_Enabled;

		// Token: 0x04001EF5 RID: 7925
		private static string s_CookieName;

		// Token: 0x04001EF6 RID: 7926
		private static bool s_CacheRolesInCookie;

		// Token: 0x04001EF7 RID: 7927
		private static int s_CookieTimeout;

		// Token: 0x04001EF8 RID: 7928
		private static string s_CookiePath;

		// Token: 0x04001EF9 RID: 7929
		private static bool s_CookieRequireSSL;

		// Token: 0x04001EFA RID: 7930
		private static bool s_CookieSlidingExpiration;

		// Token: 0x04001EFB RID: 7931
		private static CookieProtection s_CookieProtection;

		// Token: 0x04001EFC RID: 7932
		private static string s_Domain;

		// Token: 0x04001EFD RID: 7933
		private static bool s_Initialized;

		// Token: 0x04001EFE RID: 7934
		private static bool s_EnabledSet;

		// Token: 0x04001EFF RID: 7935
		private static RoleProviderCollection s_Providers;

		// Token: 0x04001F00 RID: 7936
		private static Exception s_InitializeException = null;

		// Token: 0x04001F01 RID: 7937
		private static bool s_CreatePersistentCookie;

		// Token: 0x04001F02 RID: 7938
		private static object s_lock = new object();

		// Token: 0x04001F03 RID: 7939
		private static int s_MaxCachedResults = 25;
	}
}
