using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Security.Permissions;
using System.Web.Configuration;
using System.Web.Util;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006DD RID: 1757
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public static class PersonalizationAdministration
	{
		// Token: 0x17001641 RID: 5697
		// (get) Token: 0x06005624 RID: 22052 RVA: 0x0015C133 File Offset: 0x0015B133
		// (set) Token: 0x06005625 RID: 22053 RVA: 0x0015C13F File Offset: 0x0015B13F
		public static string ApplicationName
		{
			get
			{
				return PersonalizationAdministration.Provider.ApplicationName;
			}
			set
			{
				PersonalizationAdministration.Provider.ApplicationName = value;
			}
		}

		// Token: 0x17001642 RID: 5698
		// (get) Token: 0x06005626 RID: 22054 RVA: 0x0015C14C File Offset: 0x0015B14C
		public static PersonalizationProvider Provider
		{
			get
			{
				PersonalizationAdministration.Initialize();
				return PersonalizationAdministration._provider;
			}
		}

		// Token: 0x17001643 RID: 5699
		// (get) Token: 0x06005627 RID: 22055 RVA: 0x0015C158 File Offset: 0x0015B158
		public static PersonalizationProviderCollection Providers
		{
			get
			{
				PersonalizationAdministration.Initialize();
				return PersonalizationAdministration._providers;
			}
		}

		// Token: 0x06005628 RID: 22056 RVA: 0x0015C164 File Offset: 0x0015B164
		private static void Initialize()
		{
			HttpRuntime.CheckAspNetHostingPermission(AspNetHostingPermissionLevel.Low, "Feature_not_supported_at_this_level");
			if (PersonalizationAdministration._initialized)
			{
				return;
			}
			lock (PersonalizationAdministration._initializationLock)
			{
				if (!PersonalizationAdministration._initialized)
				{
					WebPartsSection webParts = RuntimeConfig.GetAppConfig().WebParts;
					WebPartsPersonalization personalization = webParts.Personalization;
					PersonalizationAdministration._providers = new PersonalizationProviderCollection();
					ProvidersHelper.InstantiateProviders(personalization.Providers, PersonalizationAdministration._providers, typeof(PersonalizationProvider));
					PersonalizationAdministration._providers.SetReadOnly();
					PersonalizationAdministration._provider = PersonalizationAdministration._providers[personalization.DefaultProvider];
					if (PersonalizationAdministration._provider == null)
					{
						throw new ConfigurationErrorsException(SR.GetString("Config_provider_must_exist", new object[] { personalization.DefaultProvider }), personalization.ElementInformation.Properties["defaultProvider"].Source, personalization.ElementInformation.Properties["defaultProvider"].LineNumber);
					}
					PersonalizationAdministration._initialized = true;
				}
			}
		}

		// Token: 0x06005629 RID: 22057 RVA: 0x0015C270 File Offset: 0x0015B270
		public static int ResetAllState(PersonalizationScope scope)
		{
			PersonalizationProviderHelper.CheckPersonalizationScope(scope);
			return PersonalizationAdministration.ResetStatePrivate(scope, null, null);
		}

		// Token: 0x0600562A RID: 22058 RVA: 0x0015C280 File Offset: 0x0015B280
		public static int ResetState(PersonalizationStateInfoCollection data)
		{
			int num = 0;
			PersonalizationProviderHelper.CheckNullEntries(data, "data");
			StringCollection stringCollection = null;
			foreach (object obj in data)
			{
				PersonalizationStateInfo personalizationStateInfo = (PersonalizationStateInfo)obj;
				UserPersonalizationStateInfo userPersonalizationStateInfo = personalizationStateInfo as UserPersonalizationStateInfo;
				if (userPersonalizationStateInfo != null)
				{
					if (PersonalizationAdministration.ResetUserState(userPersonalizationStateInfo.Path, userPersonalizationStateInfo.Username))
					{
						num++;
					}
				}
				else
				{
					if (stringCollection == null)
					{
						stringCollection = new StringCollection();
					}
					stringCollection.Add(personalizationStateInfo.Path);
				}
			}
			if (stringCollection != null)
			{
				string[] array = new string[stringCollection.Count];
				stringCollection.CopyTo(array, 0);
				num += PersonalizationAdministration.ResetStatePrivate(PersonalizationScope.Shared, array, null);
			}
			return num;
		}

		// Token: 0x0600562B RID: 22059 RVA: 0x0015C344 File Offset: 0x0015B344
		public static bool ResetSharedState(string path)
		{
			path = StringUtil.CheckAndTrimString(path, "path");
			string[] array = new string[] { path };
			int num = PersonalizationAdministration.ResetStatePrivate(PersonalizationScope.Shared, array, null);
			if (num > 1)
			{
				throw new HttpException(SR.GetString("PersonalizationAdmin_UnexpectedResetSharedStateReturnValue", new object[] { num.ToString(CultureInfo.CurrentCulture) }));
			}
			return num == 1;
		}

		// Token: 0x0600562C RID: 22060 RVA: 0x0015C3A3 File Offset: 0x0015B3A3
		public static int ResetSharedState(string[] paths)
		{
			paths = PersonalizationProviderHelper.CheckAndTrimNonEmptyStringEntries(paths, "paths", true, false, -1);
			return PersonalizationAdministration.ResetStatePrivate(PersonalizationScope.Shared, paths, null);
		}

		// Token: 0x0600562D RID: 22061 RVA: 0x0015C3C0 File Offset: 0x0015B3C0
		public static int ResetUserState(string path)
		{
			path = StringUtil.CheckAndTrimString(path, "path");
			string[] array = new string[] { path };
			return PersonalizationAdministration.ResetStatePrivate(PersonalizationScope.User, array, null);
		}

		// Token: 0x0600562E RID: 22062 RVA: 0x0015C3EF File Offset: 0x0015B3EF
		public static int ResetUserState(string[] usernames)
		{
			usernames = PersonalizationProviderHelper.CheckAndTrimNonEmptyStringEntries(usernames, "usernames", true, true, -1);
			return PersonalizationAdministration.ResetStatePrivate(PersonalizationScope.User, null, usernames);
		}

		// Token: 0x0600562F RID: 22063 RVA: 0x0015C40C File Offset: 0x0015B40C
		public static bool ResetUserState(string path, string username)
		{
			path = StringUtil.CheckAndTrimString(path, "path");
			username = PersonalizationProviderHelper.CheckAndTrimStringWithoutCommas(username, "username");
			string[] array = new string[] { path };
			string[] array2 = new string[] { username };
			int num = PersonalizationAdministration.ResetStatePrivate(PersonalizationScope.User, array, array2);
			if (num > 1)
			{
				throw new HttpException(SR.GetString("PersonalizationAdmin_UnexpectedResetUserStateReturnValue", new object[] { num.ToString(CultureInfo.CurrentCulture) }));
			}
			return num == 1;
		}

		// Token: 0x06005630 RID: 22064 RVA: 0x0015C48C File Offset: 0x0015B48C
		public static int ResetUserState(string path, string[] usernames)
		{
			path = StringUtil.CheckAndTrimString(path, "path");
			usernames = PersonalizationProviderHelper.CheckAndTrimNonEmptyStringEntries(usernames, "usernames", true, true, -1);
			string[] array = new string[] { path };
			return PersonalizationAdministration.ResetStatePrivate(PersonalizationScope.User, array, usernames);
		}

		// Token: 0x06005631 RID: 22065 RVA: 0x0015C4CC File Offset: 0x0015B4CC
		private static int ResetStatePrivate(PersonalizationScope scope, string[] paths, string[] usernames)
		{
			PersonalizationAdministration.Initialize();
			int num = PersonalizationAdministration._provider.ResetState(scope, paths, usernames);
			PersonalizationProviderHelper.CheckNegativeReturnedInteger(num, "ResetState");
			return num;
		}

		// Token: 0x06005632 RID: 22066 RVA: 0x0015C4F8 File Offset: 0x0015B4F8
		public static int ResetInactiveUserState(DateTime userInactiveSinceDate)
		{
			return PersonalizationAdministration.ResetInactiveUserStatePrivate(null, userInactiveSinceDate);
		}

		// Token: 0x06005633 RID: 22067 RVA: 0x0015C501 File Offset: 0x0015B501
		public static int ResetInactiveUserState(string path, DateTime userInactiveSinceDate)
		{
			path = StringUtil.CheckAndTrimString(path, "path");
			return PersonalizationAdministration.ResetInactiveUserStatePrivate(path, userInactiveSinceDate);
		}

		// Token: 0x06005634 RID: 22068 RVA: 0x0015C518 File Offset: 0x0015B518
		private static int ResetInactiveUserStatePrivate(string path, DateTime userInactiveSinceDate)
		{
			PersonalizationAdministration.Initialize();
			int num = PersonalizationAdministration._provider.ResetUserState(path, userInactiveSinceDate);
			PersonalizationProviderHelper.CheckNegativeReturnedInteger(num, "ResetUserState");
			return num;
		}

		// Token: 0x06005635 RID: 22069 RVA: 0x0015C543 File Offset: 0x0015B543
		public static int GetCountOfState(PersonalizationScope scope)
		{
			return PersonalizationAdministration.GetCountOfState(scope, null);
		}

		// Token: 0x06005636 RID: 22070 RVA: 0x0015C54C File Offset: 0x0015B54C
		public static int GetCountOfState(PersonalizationScope scope, string pathToMatch)
		{
			PersonalizationProviderHelper.CheckPersonalizationScope(scope);
			pathToMatch = StringUtil.CheckAndTrimString(pathToMatch, "pathToMatch", false);
			return PersonalizationAdministration.GetCountOfStatePrivate(scope, new PersonalizationStateQuery
			{
				PathToMatch = pathToMatch
			});
		}

		// Token: 0x06005637 RID: 22071 RVA: 0x0015C584 File Offset: 0x0015B584
		private static int GetCountOfStatePrivate(PersonalizationScope scope, PersonalizationStateQuery stateQuery)
		{
			PersonalizationAdministration.Initialize();
			int countOfState = PersonalizationAdministration._provider.GetCountOfState(scope, stateQuery);
			PersonalizationProviderHelper.CheckNegativeReturnedInteger(countOfState, "GetCountOfState");
			return countOfState;
		}

		// Token: 0x06005638 RID: 22072 RVA: 0x0015C5B0 File Offset: 0x0015B5B0
		public static int GetCountOfUserState(string usernameToMatch)
		{
			usernameToMatch = StringUtil.CheckAndTrimString(usernameToMatch, "usernameToMatch", false);
			return PersonalizationAdministration.GetCountOfStatePrivate(PersonalizationScope.User, new PersonalizationStateQuery
			{
				UsernameToMatch = usernameToMatch
			});
		}

		// Token: 0x06005639 RID: 22073 RVA: 0x0015C5DF File Offset: 0x0015B5DF
		public static int GetCountOfInactiveUserState(DateTime userInactiveSinceDate)
		{
			return PersonalizationAdministration.GetCountOfInactiveUserState(null, userInactiveSinceDate);
		}

		// Token: 0x0600563A RID: 22074 RVA: 0x0015C5E8 File Offset: 0x0015B5E8
		public static int GetCountOfInactiveUserState(string pathToMatch, DateTime userInactiveSinceDate)
		{
			pathToMatch = StringUtil.CheckAndTrimString(pathToMatch, "pathToMatch", false);
			return PersonalizationAdministration.GetCountOfStatePrivate(PersonalizationScope.User, new PersonalizationStateQuery
			{
				PathToMatch = pathToMatch,
				UserInactiveSinceDate = userInactiveSinceDate
			});
		}

		// Token: 0x0600563B RID: 22075 RVA: 0x0015C61E File Offset: 0x0015B61E
		private static PersonalizationStateInfoCollection FindStatePrivate(PersonalizationScope scope, PersonalizationStateQuery stateQuery, int pageIndex, int pageSize, out int totalRecords)
		{
			PersonalizationAdministration.Initialize();
			return PersonalizationAdministration._provider.FindState(scope, stateQuery, pageIndex, pageSize, out totalRecords);
		}

		// Token: 0x0600563C RID: 22076 RVA: 0x0015C638 File Offset: 0x0015B638
		public static PersonalizationStateInfoCollection GetAllState(PersonalizationScope scope)
		{
			int num;
			return PersonalizationAdministration.GetAllState(scope, 0, int.MaxValue, out num);
		}

		// Token: 0x0600563D RID: 22077 RVA: 0x0015C653 File Offset: 0x0015B653
		public static PersonalizationStateInfoCollection GetAllState(PersonalizationScope scope, int pageIndex, int pageSize, out int totalRecords)
		{
			PersonalizationProviderHelper.CheckPersonalizationScope(scope);
			PersonalizationProviderHelper.CheckPageIndexAndSize(pageIndex, pageSize);
			return PersonalizationAdministration.FindStatePrivate(scope, null, pageIndex, pageSize, out totalRecords);
		}

		// Token: 0x0600563E RID: 22078 RVA: 0x0015C66C File Offset: 0x0015B66C
		public static PersonalizationStateInfoCollection GetAllInactiveUserState(DateTime userInactiveSinceDate)
		{
			int num;
			return PersonalizationAdministration.GetAllInactiveUserState(userInactiveSinceDate, 0, int.MaxValue, out num);
		}

		// Token: 0x0600563F RID: 22079 RVA: 0x0015C688 File Offset: 0x0015B688
		public static PersonalizationStateInfoCollection GetAllInactiveUserState(DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
		{
			PersonalizationProviderHelper.CheckPageIndexAndSize(pageIndex, pageSize);
			return PersonalizationAdministration.FindStatePrivate(PersonalizationScope.User, new PersonalizationStateQuery
			{
				UserInactiveSinceDate = userInactiveSinceDate
			}, pageIndex, pageSize, out totalRecords);
		}

		// Token: 0x06005640 RID: 22080 RVA: 0x0015C6B4 File Offset: 0x0015B6B4
		public static PersonalizationStateInfoCollection FindSharedState(string pathToMatch)
		{
			int num;
			return PersonalizationAdministration.FindSharedState(pathToMatch, 0, int.MaxValue, out num);
		}

		// Token: 0x06005641 RID: 22081 RVA: 0x0015C6D0 File Offset: 0x0015B6D0
		public static PersonalizationStateInfoCollection FindSharedState(string pathToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			pathToMatch = StringUtil.CheckAndTrimString(pathToMatch, "pathToMatch", false);
			PersonalizationProviderHelper.CheckPageIndexAndSize(pageIndex, pageSize);
			return PersonalizationAdministration.FindStatePrivate(PersonalizationScope.Shared, new PersonalizationStateQuery
			{
				PathToMatch = pathToMatch
			}, pageIndex, pageSize, out totalRecords);
		}

		// Token: 0x06005642 RID: 22082 RVA: 0x0015C70C File Offset: 0x0015B70C
		public static PersonalizationStateInfoCollection FindUserState(string pathToMatch, string usernameToMatch)
		{
			int num;
			return PersonalizationAdministration.FindUserState(pathToMatch, usernameToMatch, 0, int.MaxValue, out num);
		}

		// Token: 0x06005643 RID: 22083 RVA: 0x0015C728 File Offset: 0x0015B728
		public static PersonalizationStateInfoCollection FindUserState(string pathToMatch, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			pathToMatch = StringUtil.CheckAndTrimString(pathToMatch, "pathToMatch", false);
			usernameToMatch = StringUtil.CheckAndTrimString(usernameToMatch, "usernameToMatch", false);
			PersonalizationProviderHelper.CheckPageIndexAndSize(pageIndex, pageSize);
			return PersonalizationAdministration.FindStatePrivate(PersonalizationScope.User, new PersonalizationStateQuery
			{
				PathToMatch = pathToMatch,
				UsernameToMatch = usernameToMatch
			}, pageIndex, pageSize, out totalRecords);
		}

		// Token: 0x06005644 RID: 22084 RVA: 0x0015C778 File Offset: 0x0015B778
		public static PersonalizationStateInfoCollection FindInactiveUserState(string pathToMatch, string usernameToMatch, DateTime userInactiveSinceDate)
		{
			int num;
			return PersonalizationAdministration.FindInactiveUserState(pathToMatch, usernameToMatch, userInactiveSinceDate, 0, int.MaxValue, out num);
		}

		// Token: 0x06005645 RID: 22085 RVA: 0x0015C798 File Offset: 0x0015B798
		public static PersonalizationStateInfoCollection FindInactiveUserState(string pathToMatch, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
		{
			pathToMatch = StringUtil.CheckAndTrimString(pathToMatch, "pathToMatch", false);
			usernameToMatch = StringUtil.CheckAndTrimString(usernameToMatch, "usernameToMatch", false);
			PersonalizationProviderHelper.CheckPageIndexAndSize(pageIndex, pageSize);
			return PersonalizationAdministration.FindStatePrivate(PersonalizationScope.User, new PersonalizationStateQuery
			{
				PathToMatch = pathToMatch,
				UsernameToMatch = usernameToMatch,
				UserInactiveSinceDate = userInactiveSinceDate
			}, pageIndex, pageSize, out totalRecords);
		}

		// Token: 0x04002F4F RID: 12111
		private const int _defaultPageIndex = 0;

		// Token: 0x04002F50 RID: 12112
		private const int _defaultPageSize = 2147483647;

		// Token: 0x04002F51 RID: 12113
		private static readonly object _initializationLock = new object();

		// Token: 0x04002F52 RID: 12114
		private static bool _initialized;

		// Token: 0x04002F53 RID: 12115
		private static PersonalizationProvider _provider;

		// Token: 0x04002F54 RID: 12116
		private static PersonalizationProviderCollection _providers;

		// Token: 0x04002F55 RID: 12117
		internal static readonly DateTime DefaultInactiveSinceDate = DateTime.MaxValue;
	}
}
