using System;
using System.Configuration;
using System.Configuration.Provider;
using System.Security.Permissions;
using System.Web.Configuration;
using System.Web.Util;

namespace System.Web.Profile
{
	// Token: 0x0200030B RID: 779
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public static class ProfileManager
	{
		// Token: 0x0600266B RID: 9835 RVA: 0x000A4D40 File Offset: 0x000A3D40
		public static bool DeleteProfile(string username)
		{
			SecUtility.CheckParameter(ref username, true, true, true, 0, "username");
			return ProfileManager.Provider.DeleteProfiles(new string[] { username }) != 0;
		}

		// Token: 0x0600266C RID: 9836 RVA: 0x000A4D7C File Offset: 0x000A3D7C
		public static int DeleteProfiles(ProfileInfoCollection profiles)
		{
			if (profiles == null)
			{
				throw new ArgumentNullException("profiles");
			}
			if (profiles.Count < 1)
			{
				throw new ArgumentException(SR.GetString("Parameter_collection_empty", new object[] { "profiles" }), "profiles");
			}
			foreach (object obj in profiles)
			{
				ProfileInfo profileInfo = (ProfileInfo)obj;
				string userName = profileInfo.UserName;
				SecUtility.CheckParameter(ref userName, true, true, true, 0, "UserName");
			}
			return ProfileManager.Provider.DeleteProfiles(profiles);
		}

		// Token: 0x0600266D RID: 9837 RVA: 0x000A4E2C File Offset: 0x000A3E2C
		public static int DeleteProfiles(string[] usernames)
		{
			SecUtility.CheckArrayParameter(ref usernames, true, true, true, 0, "usernames");
			return ProfileManager.Provider.DeleteProfiles(usernames);
		}

		// Token: 0x0600266E RID: 9838 RVA: 0x000A4E49 File Offset: 0x000A3E49
		public static int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
		{
			return ProfileManager.Provider.DeleteInactiveProfiles(authenticationOption, userInactiveSinceDate);
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x000A4E58 File Offset: 0x000A3E58
		public static int GetNumberOfProfiles(ProfileAuthenticationOption authenticationOption)
		{
			return ProfileManager.Provider.GetNumberOfInactiveProfiles(authenticationOption, DateTime.Now.AddDays(1.0));
		}

		// Token: 0x06002670 RID: 9840 RVA: 0x000A4E86 File Offset: 0x000A3E86
		public static int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
		{
			return ProfileManager.Provider.GetNumberOfInactiveProfiles(authenticationOption, userInactiveSinceDate);
		}

		// Token: 0x06002671 RID: 9841 RVA: 0x000A4E94 File Offset: 0x000A3E94
		public static ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption)
		{
			int num;
			return ProfileManager.Provider.GetAllProfiles(authenticationOption, 0, int.MaxValue, out num);
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x000A4EB4 File Offset: 0x000A3EB4
		public static ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
		{
			return ProfileManager.Provider.GetAllProfiles(authenticationOption, pageIndex, pageSize, out totalRecords);
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x000A4EC4 File Offset: 0x000A3EC4
		public static ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
		{
			int num;
			return ProfileManager.Provider.GetAllInactiveProfiles(authenticationOption, userInactiveSinceDate, 0, int.MaxValue, out num);
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x000A4EE5 File Offset: 0x000A3EE5
		public static ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
		{
			return ProfileManager.Provider.GetAllInactiveProfiles(authenticationOption, userInactiveSinceDate, pageIndex, pageSize, out totalRecords);
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x000A4EF8 File Offset: 0x000A3EF8
		public static ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch)
		{
			SecUtility.CheckParameter(ref usernameToMatch, true, true, false, 0, "usernameToMatch");
			int num;
			return ProfileManager.Provider.FindProfilesByUserName(authenticationOption, usernameToMatch, 0, int.MaxValue, out num);
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x000A4F2C File Offset: 0x000A3F2C
		public static ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			if (pageIndex < 0)
			{
				throw new ArgumentException(SR.GetString("PageIndex_bad"), "pageIndex");
			}
			if (pageSize < 1)
			{
				throw new ArgumentException(SR.GetString("PageSize_bad"), "pageSize");
			}
			SecUtility.CheckParameter(ref usernameToMatch, true, true, false, 0, "usernameToMatch");
			return ProfileManager.Provider.FindProfilesByUserName(authenticationOption, usernameToMatch, pageIndex, pageSize, out totalRecords);
		}

		// Token: 0x06002677 RID: 9847 RVA: 0x000A4F8C File Offset: 0x000A3F8C
		public static ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate)
		{
			SecUtility.CheckParameter(ref usernameToMatch, true, true, false, 0, "usernameToMatch");
			int num;
			return ProfileManager.Provider.FindInactiveProfilesByUserName(authenticationOption, usernameToMatch, userInactiveSinceDate, 0, int.MaxValue, out num);
		}

		// Token: 0x06002678 RID: 9848 RVA: 0x000A4FC0 File Offset: 0x000A3FC0
		public static ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
		{
			if (pageIndex < 0)
			{
				throw new ArgumentException(SR.GetString("PageIndex_bad"), "pageIndex");
			}
			if (pageSize < 1)
			{
				throw new ArgumentException(SR.GetString("PageSize_bad"), "pageSize");
			}
			SecUtility.CheckParameter(ref usernameToMatch, true, true, false, 0, "usernameToMatch");
			return ProfileManager.Provider.FindInactiveProfilesByUserName(authenticationOption, usernameToMatch, userInactiveSinceDate, pageIndex, pageSize, out totalRecords);
		}

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06002679 RID: 9849 RVA: 0x000A5022 File Offset: 0x000A4022
		public static bool Enabled
		{
			get
			{
				if (!ProfileManager.s_Initialized && !ProfileManager.s_InitializedEnabled)
				{
					ProfileManager.InitializeEnabled(false);
				}
				return ProfileManager.s_Enabled;
			}
		}

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x0600267A RID: 9850 RVA: 0x000A503D File Offset: 0x000A403D
		// (set) Token: 0x0600267B RID: 9851 RVA: 0x000A5049 File Offset: 0x000A4049
		public static string ApplicationName
		{
			get
			{
				return ProfileManager.Provider.ApplicationName;
			}
			set
			{
				ProfileManager.Provider.ApplicationName = value;
			}
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x0600267C RID: 9852 RVA: 0x000A5056 File Offset: 0x000A4056
		public static bool AutomaticSaveEnabled
		{
			get
			{
				HttpRuntime.CheckAspNetHostingPermission(AspNetHostingPermissionLevel.Low, "Feature_not_supported_at_this_level");
				ProfileManager.InitializeEnabled(false);
				return ProfileManager.s_AutomaticSaveEnabled;
			}
		}

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x0600267D RID: 9853 RVA: 0x000A5072 File Offset: 0x000A4072
		public static ProfileProvider Provider
		{
			get
			{
				HttpRuntime.CheckAspNetHostingPermission(AspNetHostingPermissionLevel.Low, "Feature_not_supported_at_this_level");
				ProfileManager.Initialize(true);
				return ProfileManager.s_Provider;
			}
		}

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x0600267E RID: 9854 RVA: 0x000A508E File Offset: 0x000A408E
		public static ProfileProviderCollection Providers
		{
			get
			{
				HttpRuntime.CheckAspNetHostingPermission(AspNetHostingPermissionLevel.Low, "Feature_not_supported_at_this_level");
				ProfileManager.Initialize(true);
				return ProfileManager.s_Providers;
			}
		}

		// Token: 0x0600267F RID: 9855 RVA: 0x000A50AC File Offset: 0x000A40AC
		private static void InitializeEnabled(bool initProviders)
		{
			if (!ProfileManager.s_Initialized || !ProfileManager.s_InitializedProviders)
			{
				lock (ProfileManager.s_Lock)
				{
					if (ProfileManager.s_Initialized)
					{
						if (ProfileManager.s_InitializedProviders)
						{
							goto IL_00A0;
						}
					}
					try
					{
						ProfileSection profile = RuntimeConfig.GetAppConfig().Profile;
						if (!ProfileManager.s_InitializedEnabled)
						{
							ProfileManager.s_Enabled = profile.Enabled && HttpRuntime.HasAspNetHostingPermission(AspNetHostingPermissionLevel.Low);
							ProfileManager.s_AutomaticSaveEnabled = ProfileManager.s_Enabled && profile.AutomaticSaveEnabled;
							ProfileManager.s_InitializedEnabled = true;
						}
						if (initProviders && ProfileManager.s_Enabled && !ProfileManager.s_InitializedProviders)
						{
							ProfileManager.InitProviders(profile);
							ProfileManager.s_InitializedProviders = true;
						}
					}
					catch (Exception ex)
					{
						ProfileManager.s_InitException = ex;
					}
					ProfileManager.s_Initialized = true;
					IL_00A0:;
				}
			}
		}

		// Token: 0x06002680 RID: 9856 RVA: 0x000A5180 File Offset: 0x000A4180
		private static void Initialize(bool throwIfNotEnabled)
		{
			ProfileManager.InitializeEnabled(true);
			if (ProfileManager.s_InitException != null)
			{
				throw ProfileManager.s_InitException;
			}
			if (throwIfNotEnabled && !ProfileManager.s_Enabled)
			{
				throw new ProviderException(SR.GetString("Profile_not_enabled"));
			}
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x000A51B0 File Offset: 0x000A41B0
		private static void InitProviders(ProfileSection config)
		{
			ProfileManager.s_Providers = new ProfileProviderCollection();
			if (config.Providers != null)
			{
				ProvidersHelper.InstantiateProviders(config.Providers, ProfileManager.s_Providers, typeof(ProfileProvider));
			}
			ProfileManager.s_Providers.SetReadOnly();
			if (config.DefaultProvider == null)
			{
				throw new ProviderException(SR.GetString("Profile_default_provider_not_specified"));
			}
			ProfileManager.s_Provider = ProfileManager.s_Providers[config.DefaultProvider];
			if (ProfileManager.s_Provider == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Profile_default_provider_not_found"), config.ElementInformation.Properties["providers"].Source, config.ElementInformation.Properties["providers"].LineNumber);
			}
		}

		// Token: 0x04001DC2 RID: 7618
		private static ProfileProvider s_Provider;

		// Token: 0x04001DC3 RID: 7619
		private static ProfileProviderCollection s_Providers;

		// Token: 0x04001DC4 RID: 7620
		private static bool s_Enabled;

		// Token: 0x04001DC5 RID: 7621
		private static bool s_Initialized;

		// Token: 0x04001DC6 RID: 7622
		private static bool s_InitializedProviders;

		// Token: 0x04001DC7 RID: 7623
		private static object s_Lock = new object();

		// Token: 0x04001DC8 RID: 7624
		private static Exception s_InitException;

		// Token: 0x04001DC9 RID: 7625
		private static bool s_InitializedEnabled;

		// Token: 0x04001DCA RID: 7626
		private static bool s_AutomaticSaveEnabled;
	}
}
