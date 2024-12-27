using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Threading;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web.Security
{
	// Token: 0x02000340 RID: 832
	public static class Membership
	{
		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x06002873 RID: 10355 RVA: 0x000B1D86 File Offset: 0x000B0D86
		public static bool EnablePasswordRetrieval
		{
			get
			{
				Membership.Initialize();
				return Membership.Provider.EnablePasswordRetrieval;
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06002874 RID: 10356 RVA: 0x000B1D97 File Offset: 0x000B0D97
		public static bool EnablePasswordReset
		{
			get
			{
				Membership.Initialize();
				return Membership.Provider.EnablePasswordReset;
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06002875 RID: 10357 RVA: 0x000B1DA8 File Offset: 0x000B0DA8
		public static bool RequiresQuestionAndAnswer
		{
			get
			{
				Membership.Initialize();
				return Membership.Provider.RequiresQuestionAndAnswer;
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06002876 RID: 10358 RVA: 0x000B1DB9 File Offset: 0x000B0DB9
		public static int UserIsOnlineTimeWindow
		{
			get
			{
				Membership.Initialize();
				return Membership.s_UserIsOnlineTimeWindow;
			}
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06002877 RID: 10359 RVA: 0x000B1DC5 File Offset: 0x000B0DC5
		public static MembershipProviderCollection Providers
		{
			get
			{
				Membership.Initialize();
				return Membership.s_Providers;
			}
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06002878 RID: 10360 RVA: 0x000B1DD1 File Offset: 0x000B0DD1
		public static MembershipProvider Provider
		{
			get
			{
				Membership.Initialize();
				return Membership.s_Provider;
			}
		}

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06002879 RID: 10361 RVA: 0x000B1DDD File Offset: 0x000B0DDD
		public static string HashAlgorithmType
		{
			get
			{
				Membership.Initialize();
				return Membership.s_HashAlgorithmType;
			}
		}

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x0600287A RID: 10362 RVA: 0x000B1DE9 File Offset: 0x000B0DE9
		internal static bool IsHashAlgorithmFromMembershipConfig
		{
			get
			{
				Membership.Initialize();
				return Membership.s_HashAlgorithmFromConfig;
			}
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x0600287B RID: 10363 RVA: 0x000B1DF5 File Offset: 0x000B0DF5
		public static int MaxInvalidPasswordAttempts
		{
			get
			{
				Membership.Initialize();
				return Membership.Provider.MaxInvalidPasswordAttempts;
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x0600287C RID: 10364 RVA: 0x000B1E06 File Offset: 0x000B0E06
		public static int PasswordAttemptWindow
		{
			get
			{
				Membership.Initialize();
				return Membership.Provider.PasswordAttemptWindow;
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x0600287D RID: 10365 RVA: 0x000B1E17 File Offset: 0x000B0E17
		public static int MinRequiredPasswordLength
		{
			get
			{
				Membership.Initialize();
				return Membership.Provider.MinRequiredPasswordLength;
			}
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x0600287E RID: 10366 RVA: 0x000B1E28 File Offset: 0x000B0E28
		public static int MinRequiredNonAlphanumericCharacters
		{
			get
			{
				Membership.Initialize();
				return Membership.Provider.MinRequiredNonAlphanumericCharacters;
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x0600287F RID: 10367 RVA: 0x000B1E39 File Offset: 0x000B0E39
		public static string PasswordStrengthRegularExpression
		{
			get
			{
				Membership.Initialize();
				return Membership.Provider.PasswordStrengthRegularExpression;
			}
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x06002880 RID: 10368 RVA: 0x000B1E4A File Offset: 0x000B0E4A
		// (set) Token: 0x06002881 RID: 10369 RVA: 0x000B1E56 File Offset: 0x000B0E56
		public static string ApplicationName
		{
			get
			{
				return Membership.Provider.ApplicationName;
			}
			set
			{
				Membership.Provider.ApplicationName = value;
			}
		}

		// Token: 0x06002882 RID: 10370 RVA: 0x000B1E63 File Offset: 0x000B0E63
		public static MembershipUser CreateUser(string username, string password)
		{
			return Membership.CreateUser(username, password, null);
		}

		// Token: 0x06002883 RID: 10371 RVA: 0x000B1E70 File Offset: 0x000B0E70
		public static MembershipUser CreateUser(string username, string password, string email)
		{
			MembershipCreateStatus membershipCreateStatus;
			MembershipUser membershipUser = Membership.CreateUser(username, password, email, null, null, true, out membershipCreateStatus);
			if (membershipUser == null)
			{
				throw new MembershipCreateUserException(membershipCreateStatus);
			}
			return membershipUser;
		}

		// Token: 0x06002884 RID: 10372 RVA: 0x000B1E96 File Offset: 0x000B0E96
		public static MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, out MembershipCreateStatus status)
		{
			return Membership.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, null, out status);
		}

		// Token: 0x06002885 RID: 10373 RVA: 0x000B1EA8 File Offset: 0x000B0EA8
		public static MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
		{
			if (!SecUtility.ValidateParameter(ref username, true, true, true, 0))
			{
				status = MembershipCreateStatus.InvalidUserName;
				return null;
			}
			if (!SecUtility.ValidatePasswordParameter(ref password, 0))
			{
				status = MembershipCreateStatus.InvalidPassword;
				return null;
			}
			if (!SecUtility.ValidateParameter(ref email, false, false, false, 0))
			{
				status = MembershipCreateStatus.InvalidEmail;
				return null;
			}
			if (!SecUtility.ValidateParameter(ref passwordQuestion, false, true, false, 0))
			{
				status = MembershipCreateStatus.InvalidQuestion;
				return null;
			}
			if (!SecUtility.ValidateParameter(ref passwordAnswer, false, true, false, 0))
			{
				status = MembershipCreateStatus.InvalidAnswer;
				return null;
			}
			return Membership.Provider.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x000B1F27 File Offset: 0x000B0F27
		public static bool ValidateUser(string username, string password)
		{
			return Membership.Provider.ValidateUser(username, password);
		}

		// Token: 0x06002887 RID: 10375 RVA: 0x000B1F35 File Offset: 0x000B0F35
		public static MembershipUser GetUser()
		{
			return Membership.GetUser(Membership.GetCurrentUserName(), true);
		}

		// Token: 0x06002888 RID: 10376 RVA: 0x000B1F42 File Offset: 0x000B0F42
		public static MembershipUser GetUser(bool userIsOnline)
		{
			return Membership.GetUser(Membership.GetCurrentUserName(), userIsOnline);
		}

		// Token: 0x06002889 RID: 10377 RVA: 0x000B1F4F File Offset: 0x000B0F4F
		public static MembershipUser GetUser(string username)
		{
			return Membership.GetUser(username, false);
		}

		// Token: 0x0600288A RID: 10378 RVA: 0x000B1F58 File Offset: 0x000B0F58
		public static MembershipUser GetUser(string username, bool userIsOnline)
		{
			SecUtility.CheckParameter(ref username, true, false, true, 0, "username");
			return Membership.Provider.GetUser(username, userIsOnline);
		}

		// Token: 0x0600288B RID: 10379 RVA: 0x000B1F76 File Offset: 0x000B0F76
		public static MembershipUser GetUser(object providerUserKey)
		{
			return Membership.GetUser(providerUserKey, false);
		}

		// Token: 0x0600288C RID: 10380 RVA: 0x000B1F7F File Offset: 0x000B0F7F
		public static MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			if (providerUserKey == null)
			{
				throw new ArgumentNullException("providerUserKey");
			}
			return Membership.Provider.GetUser(providerUserKey, userIsOnline);
		}

		// Token: 0x0600288D RID: 10381 RVA: 0x000B1F9B File Offset: 0x000B0F9B
		public static string GetUserNameByEmail(string emailToMatch)
		{
			SecUtility.CheckParameter(ref emailToMatch, false, false, false, 0, "emailToMatch");
			return Membership.Provider.GetUserNameByEmail(emailToMatch);
		}

		// Token: 0x0600288E RID: 10382 RVA: 0x000B1FB8 File Offset: 0x000B0FB8
		public static bool DeleteUser(string username)
		{
			SecUtility.CheckParameter(ref username, true, true, true, 0, "username");
			return Membership.Provider.DeleteUser(username, true);
		}

		// Token: 0x0600288F RID: 10383 RVA: 0x000B1FD6 File Offset: 0x000B0FD6
		public static bool DeleteUser(string username, bool deleteAllRelatedData)
		{
			SecUtility.CheckParameter(ref username, true, true, true, 0, "username");
			return Membership.Provider.DeleteUser(username, deleteAllRelatedData);
		}

		// Token: 0x06002890 RID: 10384 RVA: 0x000B1FF4 File Offset: 0x000B0FF4
		public static void UpdateUser(MembershipUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.Update();
		}

		// Token: 0x06002891 RID: 10385 RVA: 0x000B200C File Offset: 0x000B100C
		public static MembershipUserCollection GetAllUsers()
		{
			int num = 0;
			return Membership.GetAllUsers(0, int.MaxValue, out num);
		}

		// Token: 0x06002892 RID: 10386 RVA: 0x000B2028 File Offset: 0x000B1028
		public static MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
			if (pageIndex < 0)
			{
				throw new ArgumentException(SR.GetString("PageIndex_bad"), "pageIndex");
			}
			if (pageSize < 1)
			{
				throw new ArgumentException(SR.GetString("PageSize_bad"), "pageSize");
			}
			return Membership.Provider.GetAllUsers(pageIndex, pageSize, out totalRecords);
		}

		// Token: 0x06002893 RID: 10387 RVA: 0x000B2074 File Offset: 0x000B1074
		public static int GetNumberOfUsersOnline()
		{
			return Membership.Provider.GetNumberOfUsersOnline();
		}

		// Token: 0x06002894 RID: 10388 RVA: 0x000B2080 File Offset: 0x000B1080
		public static string GeneratePassword(int length, int numberOfNonAlphanumericCharacters)
		{
			if (length < 1 || length > 128)
			{
				throw new ArgumentException(SR.GetString("Membership_password_length_incorrect"));
			}
			if (numberOfNonAlphanumericCharacters > length || numberOfNonAlphanumericCharacters < 0)
			{
				throw new ArgumentException(SR.GetString("Membership_min_required_non_alphanumeric_characters_incorrect", new object[] { "numberOfNonAlphanumericCharacters" }));
			}
			string text;
			int num4;
			do
			{
				byte[] array = new byte[length];
				char[] array2 = new char[length];
				int num = 0;
				new RNGCryptoServiceProvider().GetBytes(array);
				for (int i = 0; i < length; i++)
				{
					int num2 = (int)(array[i] % 87);
					if (num2 < 10)
					{
						array2[i] = (char)(48 + num2);
					}
					else if (num2 < 36)
					{
						array2[i] = (char)(65 + num2 - 10);
					}
					else if (num2 < 62)
					{
						array2[i] = (char)(97 + num2 - 36);
					}
					else
					{
						array2[i] = Membership.punctuations[num2 - 62];
						num++;
					}
				}
				if (num < numberOfNonAlphanumericCharacters)
				{
					Random random = new Random();
					for (int j = 0; j < numberOfNonAlphanumericCharacters - num; j++)
					{
						int num3;
						do
						{
							num3 = random.Next(0, length);
						}
						while (!char.IsLetterOrDigit(array2[num3]));
						array2[num3] = Membership.punctuations[random.Next(0, Membership.punctuations.Length)];
					}
				}
				text = new string(array2);
			}
			while (CrossSiteScriptingValidation.IsDangerousString(text, out num4));
			return text;
		}

		// Token: 0x06002895 RID: 10389 RVA: 0x000B21C0 File Offset: 0x000B11C0
		private static void Initialize()
		{
			if (Membership.s_Initialized)
			{
				if (Membership.s_InitializeException != null)
				{
					throw Membership.s_InitializeException;
				}
				return;
			}
			else
			{
				if (Membership.s_InitializeException != null)
				{
					throw Membership.s_InitializeException;
				}
				if (HostingEnvironment.IsHosted)
				{
					HttpRuntime.CheckAspNetHostingPermission(AspNetHostingPermissionLevel.Low, "Feature_not_supported_at_this_level");
				}
				lock (Membership.s_lock)
				{
					if (Membership.s_Initialized)
					{
						if (Membership.s_InitializeException != null)
						{
							throw Membership.s_InitializeException;
						}
					}
					else
					{
						try
						{
							RuntimeConfig appConfig = RuntimeConfig.GetAppConfig();
							MembershipSection membership = appConfig.Membership;
							if (membership.DefaultProvider == null || membership.Providers == null || membership.Providers.Count < 1)
							{
								throw new ProviderException(SR.GetString("Def_membership_provider_not_specified"));
							}
							string hashAlgorithmType = membership.HashAlgorithmType;
							if (string.IsNullOrEmpty(hashAlgorithmType))
							{
								Membership.s_HashAlgorithmFromConfig = false;
								Membership.s_HashAlgorithmType = "SHA1";
								MachineKeySection machineKey = appConfig.MachineKey;
								if (machineKey != null && machineKey.Validation == MachineKeyValidation.MD5)
								{
									Membership.s_HashAlgorithmType = "MD5";
								}
							}
							else
							{
								Membership.s_HashAlgorithmType = hashAlgorithmType;
								Membership.s_HashAlgorithmFromConfig = true;
							}
							Membership.s_Providers = new MembershipProviderCollection();
							if (HostingEnvironment.IsHosted)
							{
								ProvidersHelper.InstantiateProviders(membership.Providers, Membership.s_Providers, typeof(MembershipProvider));
							}
							else
							{
								foreach (object obj2 in membership.Providers)
								{
									ProviderSettings providerSettings = (ProviderSettings)obj2;
									Type type = Type.GetType(providerSettings.Type, true, true);
									if (!typeof(MembershipProvider).IsAssignableFrom(type))
									{
										throw new ArgumentException(SR.GetString("Provider_must_implement_type", new object[] { typeof(MembershipProvider).ToString() }));
									}
									MembershipProvider membershipProvider = (MembershipProvider)Activator.CreateInstance(type);
									NameValueCollection parameters = providerSettings.Parameters;
									NameValueCollection nameValueCollection = new NameValueCollection(parameters.Count, StringComparer.Ordinal);
									foreach (object obj3 in parameters)
									{
										string text = (string)obj3;
										nameValueCollection[text] = parameters[text];
									}
									membershipProvider.Initialize(providerSettings.Name, nameValueCollection);
									Membership.s_Providers.Add(membershipProvider);
								}
							}
							Membership.s_Provider = Membership.s_Providers[membership.DefaultProvider];
							if (Membership.s_Provider == null)
							{
								throw new ConfigurationErrorsException(SR.GetString("Def_membership_provider_not_found"), membership.ElementInformation.Properties["defaultProvider"].Source, membership.ElementInformation.Properties["defaultProvider"].LineNumber);
							}
							Membership.s_UserIsOnlineTimeWindow = (int)membership.UserIsOnlineTimeWindow.TotalMinutes;
							Membership.s_Providers.SetReadOnly();
						}
						catch (Exception ex)
						{
							Membership.s_InitializeException = ex;
							throw;
						}
						Membership.s_Initialized = true;
					}
				}
				return;
			}
		}

		// Token: 0x06002896 RID: 10390 RVA: 0x000B2508 File Offset: 0x000B1508
		public static MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			SecUtility.CheckParameter(ref usernameToMatch, true, true, false, 0, "usernameToMatch");
			if (pageIndex < 0)
			{
				throw new ArgumentException(SR.GetString("PageIndex_bad"), "pageIndex");
			}
			if (pageSize < 1)
			{
				throw new ArgumentException(SR.GetString("PageSize_bad"), "pageSize");
			}
			return Membership.Provider.FindUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords);
		}

		// Token: 0x06002897 RID: 10391 RVA: 0x000B2568 File Offset: 0x000B1568
		public static MembershipUserCollection FindUsersByName(string usernameToMatch)
		{
			SecUtility.CheckParameter(ref usernameToMatch, true, true, false, 0, "usernameToMatch");
			int num = 0;
			return Membership.Provider.FindUsersByName(usernameToMatch, 0, int.MaxValue, out num);
		}

		// Token: 0x06002898 RID: 10392 RVA: 0x000B259C File Offset: 0x000B159C
		public static MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			SecUtility.CheckParameter(ref emailToMatch, false, false, false, 0, "emailToMatch");
			if (pageIndex < 0)
			{
				throw new ArgumentException(SR.GetString("PageIndex_bad"), "pageIndex");
			}
			if (pageSize < 1)
			{
				throw new ArgumentException(SR.GetString("PageSize_bad"), "pageSize");
			}
			return Membership.Provider.FindUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords);
		}

		// Token: 0x06002899 RID: 10393 RVA: 0x000B25FC File Offset: 0x000B15FC
		public static MembershipUserCollection FindUsersByEmail(string emailToMatch)
		{
			SecUtility.CheckParameter(ref emailToMatch, false, false, false, 0, "emailToMatch");
			int num = 0;
			return Membership.FindUsersByEmail(emailToMatch, 0, int.MaxValue, out num);
		}

		// Token: 0x0600289A RID: 10394 RVA: 0x000B262C File Offset: 0x000B162C
		private static string GetCurrentUserName()
		{
			if (HostingEnvironment.IsHosted)
			{
				HttpContext httpContext = HttpContext.Current;
				if (httpContext != null)
				{
					return httpContext.User.Identity.Name;
				}
			}
			IPrincipal currentPrincipal = Thread.CurrentPrincipal;
			if (currentPrincipal == null || currentPrincipal.Identity == null)
			{
				return string.Empty;
			}
			return currentPrincipal.Identity.Name;
		}

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x0600289B RID: 10395 RVA: 0x000B267C File Offset: 0x000B167C
		// (remove) Token: 0x0600289C RID: 10396 RVA: 0x000B2689 File Offset: 0x000B1689
		public static event MembershipValidatePasswordEventHandler ValidatingPassword
		{
			add
			{
				Membership.Provider.ValidatingPassword += value;
			}
			remove
			{
				Membership.Provider.ValidatingPassword -= value;
			}
		}

		// Token: 0x04001EB6 RID: 7862
		private static char[] punctuations = "!@#$%^&*()_-+=[{]};:>|./?".ToCharArray();

		// Token: 0x04001EB7 RID: 7863
		private static MembershipProviderCollection s_Providers;

		// Token: 0x04001EB8 RID: 7864
		private static MembershipProvider s_Provider;

		// Token: 0x04001EB9 RID: 7865
		private static int s_UserIsOnlineTimeWindow = 15;

		// Token: 0x04001EBA RID: 7866
		private static object s_lock = new object();

		// Token: 0x04001EBB RID: 7867
		private static bool s_Initialized = false;

		// Token: 0x04001EBC RID: 7868
		private static Exception s_InitializeException = null;

		// Token: 0x04001EBD RID: 7869
		private static string s_HashAlgorithmType;

		// Token: 0x04001EBE RID: 7870
		private static bool s_HashAlgorithmFromConfig;
	}
}
