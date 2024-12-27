using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.Web.DataAccess;
using System.Web.Hosting;
using System.Web.Management;
using System.Web.Util;

namespace System.Web.Security
{
	// Token: 0x02000318 RID: 792
	[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ActiveDirectoryMembershipProvider : MembershipProvider
	{
		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x060026E6 RID: 9958 RVA: 0x000A699F File Offset: 0x000A599F
		// (set) Token: 0x060026E7 RID: 9959 RVA: 0x000A69BF File Offset: 0x000A59BF
		public override string ApplicationName
		{
			get
			{
				if (!this.initialized)
				{
					throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
				}
				return this.appName;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("ADMembership_Setting_ApplicationName_not_supported"));
			}
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x060026E8 RID: 9960 RVA: 0x000A69D0 File Offset: 0x000A59D0
		public ActiveDirectoryConnectionProtection CurrentConnectionProtection
		{
			get
			{
				if (!this.initialized)
				{
					throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
				}
				return this.directoryInfo.ConnectionProtection;
			}
		}

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x060026E9 RID: 9961 RVA: 0x000A69F5 File Offset: 0x000A59F5
		public override MembershipPasswordFormat PasswordFormat
		{
			get
			{
				return MembershipPasswordFormat.Hashed;
			}
		}

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x060026EA RID: 9962 RVA: 0x000A69F8 File Offset: 0x000A59F8
		public override bool EnablePasswordRetrieval
		{
			get
			{
				if (!this.initialized)
				{
					throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
				}
				return this.enablePasswordRetrieval;
			}
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x060026EB RID: 9963 RVA: 0x000A6A18 File Offset: 0x000A5A18
		public override bool EnablePasswordReset
		{
			get
			{
				if (!this.initialized)
				{
					throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
				}
				return this.enablePasswordReset;
			}
		}

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x060026EC RID: 9964 RVA: 0x000A6A38 File Offset: 0x000A5A38
		public bool EnableSearchMethods
		{
			get
			{
				if (!this.initialized)
				{
					throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
				}
				return this.enableSearchMethods;
			}
		}

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x060026ED RID: 9965 RVA: 0x000A6A58 File Offset: 0x000A5A58
		public override bool RequiresQuestionAndAnswer
		{
			get
			{
				if (!this.initialized)
				{
					throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
				}
				return this.requiresQuestionAndAnswer;
			}
		}

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x060026EE RID: 9966 RVA: 0x000A6A78 File Offset: 0x000A5A78
		public override bool RequiresUniqueEmail
		{
			get
			{
				if (!this.initialized)
				{
					throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
				}
				return this.requiresUniqueEmail;
			}
		}

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x060026EF RID: 9967 RVA: 0x000A6A98 File Offset: 0x000A5A98
		public override int MaxInvalidPasswordAttempts
		{
			get
			{
				if (!this.initialized)
				{
					throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
				}
				return this.maxInvalidPasswordAttempts;
			}
		}

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x060026F0 RID: 9968 RVA: 0x000A6AB8 File Offset: 0x000A5AB8
		public override int PasswordAttemptWindow
		{
			get
			{
				if (!this.initialized)
				{
					throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
				}
				return this.passwordAttemptWindow;
			}
		}

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x060026F1 RID: 9969 RVA: 0x000A6AD8 File Offset: 0x000A5AD8
		public int PasswordAnswerAttemptLockoutDuration
		{
			get
			{
				if (!this.initialized)
				{
					throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
				}
				return this.passwordAnswerAttemptLockoutDuration;
			}
		}

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x060026F2 RID: 9970 RVA: 0x000A6AF8 File Offset: 0x000A5AF8
		public override int MinRequiredPasswordLength
		{
			get
			{
				if (!this.initialized)
				{
					throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
				}
				return this.minRequiredPasswordLength;
			}
		}

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x060026F3 RID: 9971 RVA: 0x000A6B18 File Offset: 0x000A5B18
		public override int MinRequiredNonAlphanumericCharacters
		{
			get
			{
				if (!this.initialized)
				{
					throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
				}
				return this.minRequiredNonalphanumericCharacters;
			}
		}

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x060026F4 RID: 9972 RVA: 0x000A6B38 File Offset: 0x000A5B38
		public override string PasswordStrengthRegularExpression
		{
			get
			{
				if (!this.initialized)
				{
					throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
				}
				return this.passwordStrengthRegularExpression;
			}
		}

		// Token: 0x060026F5 RID: 9973 RVA: 0x000A6B58 File Offset: 0x000A5B58
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.Assert, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public override void Initialize(string name, NameValueCollection config)
		{
			if (HostingEnvironment.IsHosted)
			{
				HttpRuntime.CheckAspNetHostingPermission(AspNetHostingPermissionLevel.Low, "Feature_not_supported_at_this_level");
			}
			if (this.initialized)
			{
				return;
			}
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}
			if (string.IsNullOrEmpty(name))
			{
				name = "AspNetActiveDirectoryMembershipProvider";
			}
			if (string.IsNullOrEmpty(config["description"]))
			{
				config.Remove("description");
				config.Add("description", SR.GetString("ADMembership_Description"));
			}
			base.Initialize(name, config);
			this.appName = config["applicationName"];
			if (string.IsNullOrEmpty(this.appName))
			{
				this.appName = SecUtility.GetDefaultAppName();
			}
			if (this.appName.Length > 256)
			{
				throw new ProviderException(SR.GetString("Provider_application_name_too_long"));
			}
			string text = config["connectionStringName"];
			if (string.IsNullOrEmpty(text))
			{
				throw new ProviderException(SR.GetString("Connection_name_not_specified"));
			}
			this.adConnectionString = this.GetConnectionString(text, true);
			if (string.IsNullOrEmpty(this.adConnectionString))
			{
				throw new ProviderException(SR.GetString("Connection_string_not_found", new object[] { text }));
			}
			string text2 = config["connectionProtection"];
			if (text2 == null)
			{
				text2 = "Secure";
			}
			else if (string.Compare(text2, "Secure", StringComparison.Ordinal) != 0 && string.Compare(text2, "None", StringComparison.Ordinal) != 0)
			{
				throw new ProviderException(SR.GetString("ADMembership_InvalidConnectionProtection", new object[] { text2 }));
			}
			string text3 = config["connectionUsername"];
			if (text3 != null && text3.Length == 0)
			{
				throw new ProviderException(SR.GetString("ADMembership_Connection_username_must_not_be_empty"));
			}
			string text4 = config["connectionPassword"];
			if (text4 != null && text4.Length == 0)
			{
				throw new ProviderException(SR.GetString("ADMembership_Connection_password_must_not_be_empty"));
			}
			if ((text3 != null && text4 == null) || (text4 != null && text3 == null))
			{
				throw new ProviderException(SR.GetString("ADMembership_Username_and_password_reqd"));
			}
			NetworkCredential networkCredential = new NetworkCredential(text3, text4);
			int intValue = SecUtility.GetIntValue(config, "clientSearchTimeout", -1, false, 0);
			int intValue2 = SecUtility.GetIntValue(config, "serverSearchTimeout", -1, false, 0);
			this.enableSearchMethods = SecUtility.GetBooleanValue(config, "enableSearchMethods", false);
			this.requiresUniqueEmail = SecUtility.GetBooleanValue(config, "requiresUniqueEmail", false);
			this.enablePasswordReset = SecUtility.GetBooleanValue(config, "enablePasswordReset", false);
			this.requiresQuestionAndAnswer = SecUtility.GetBooleanValue(config, "requiresQuestionAndAnswer", false);
			this.minRequiredPasswordLength = SecUtility.GetIntValue(config, "minRequiredPasswordLength", 7, false, 128);
			this.minRequiredNonalphanumericCharacters = SecUtility.GetIntValue(config, "minRequiredNonalphanumericCharacters", 1, true, 128);
			this.passwordStrengthRegularExpression = config["passwordStrengthRegularExpression"];
			if (this.passwordStrengthRegularExpression != null)
			{
				this.passwordStrengthRegularExpression = this.passwordStrengthRegularExpression.Trim();
				if (this.passwordStrengthRegularExpression.Length == 0)
				{
					goto IL_02DD;
				}
				try
				{
					new Regex(this.passwordStrengthRegularExpression);
					goto IL_02DD;
				}
				catch (ArgumentException ex)
				{
					throw new ProviderException(ex.Message, ex);
				}
			}
			this.passwordStrengthRegularExpression = string.Empty;
			IL_02DD:
			if (this.minRequiredNonalphanumericCharacters > this.minRequiredPasswordLength)
			{
				throw new HttpException(SR.GetString("MinRequiredNonalphanumericCharacters_can_not_be_more_than_MinRequiredPasswordLength"));
			}
			using (new ApplicationImpersonationContext())
			{
				this.directoryInfo = new DirectoryInformation(this.adConnectionString, networkCredential, text2, intValue, intValue2, this.enablePasswordReset);
				this.syntaxes.Add("attributeMapUsername", "DirectoryString");
				this.syntaxes.Add("attributeMapEmail", "DirectoryString");
				this.syntaxes.Add("attributeMapPasswordQuestion", "DirectoryString");
				this.syntaxes.Add("attributeMapPasswordAnswer", "DirectoryString");
				this.syntaxes.Add("attributeMapFailedPasswordAnswerCount", "Integer");
				this.syntaxes.Add("attributeMapFailedPasswordAnswerTime", "Integer8");
				this.syntaxes.Add("attributeMapFailedPasswordAnswerLockoutTime", "Integer8");
				this.attributesInUse.Add("objectclass", null);
				this.attributesInUse.Add("objectsid", null);
				this.attributesInUse.Add("comment", null);
				this.attributesInUse.Add("whencreated", null);
				this.attributesInUse.Add("pwdlastset", null);
				this.attributesInUse.Add("msds-user-account-control-computed", null);
				this.attributesInUse.Add("lockouttime", null);
				if (this.directoryInfo.DirectoryType == DirectoryType.AD)
				{
					this.attributesInUse.Add("useraccountcontrol", null);
				}
				else
				{
					this.attributesInUse.Add("msds-useraccountdisabled", null);
				}
				this.userObjectAttributes = this.GetUserObjectAttributes();
				int rangeUpperForSchemaAttribute;
				string text5 = this.GetAttributeMapping(config, "attributeMapUsername", out rangeUpperForSchemaAttribute);
				if (text5 != null)
				{
					this.attributeMapUsername = text5;
					if (rangeUpperForSchemaAttribute != -1)
					{
						if (rangeUpperForSchemaAttribute < this.maxUsernameLength)
						{
							this.maxUsernameLength = rangeUpperForSchemaAttribute;
						}
						if (rangeUpperForSchemaAttribute < this.maxUsernameLengthForCreation)
						{
							this.maxUsernameLengthForCreation = rangeUpperForSchemaAttribute;
						}
					}
				}
				this.attributesInUse.Add(this.attributeMapUsername, null);
				if (StringUtil.EqualsIgnoreCase(this.attributeMapUsername, "sAMAccountName"))
				{
					this.usernameIsSAMAccountName = true;
					this.usernameIsUPN = false;
				}
				text5 = this.GetAttributeMapping(config, "attributeMapEmail", out rangeUpperForSchemaAttribute);
				if (text5 != null)
				{
					this.attributeMapEmail = text5;
					if (rangeUpperForSchemaAttribute != -1 && rangeUpperForSchemaAttribute < this.maxEmailLength)
					{
						this.maxEmailLength = rangeUpperForSchemaAttribute;
					}
				}
				this.attributesInUse.Add(this.attributeMapEmail, null);
				rangeUpperForSchemaAttribute = this.GetRangeUpperForSchemaAttribute("comment");
				if (rangeUpperForSchemaAttribute != -1 && rangeUpperForSchemaAttribute < this.maxCommentLength)
				{
					this.maxCommentLength = rangeUpperForSchemaAttribute;
				}
				if (this.enablePasswordReset)
				{
					if (!this.requiresQuestionAndAnswer)
					{
						throw new ProviderException(SR.GetString("ADMembership_PasswordReset_without_question_not_supported"));
					}
					this.maxInvalidPasswordAttempts = SecUtility.GetIntValue(config, "maxInvalidPasswordAttempts", 5, false, 0);
					this.passwordAttemptWindow = SecUtility.GetIntValue(config, "passwordAttemptWindow", 10, false, 0);
					this.passwordAnswerAttemptLockoutDuration = SecUtility.GetIntValue(config, "passwordAnswerAttemptLockoutDuration", 30, false, 0);
					this.attributeMapFailedPasswordAnswerCount = this.GetAttributeMapping(config, "attributeMapFailedPasswordAnswerCount", out rangeUpperForSchemaAttribute);
					if (this.attributeMapFailedPasswordAnswerCount != null)
					{
						this.attributesInUse.Add(this.attributeMapFailedPasswordAnswerCount, null);
					}
					this.attributeMapFailedPasswordAnswerTime = this.GetAttributeMapping(config, "attributeMapFailedPasswordAnswerTime", out rangeUpperForSchemaAttribute);
					if (this.attributeMapFailedPasswordAnswerTime != null)
					{
						this.attributesInUse.Add(this.attributeMapFailedPasswordAnswerTime, null);
					}
					this.attributeMapFailedPasswordAnswerLockoutTime = this.GetAttributeMapping(config, "attributeMapFailedPasswordAnswerLockoutTime", out rangeUpperForSchemaAttribute);
					if (this.attributeMapFailedPasswordAnswerLockoutTime != null)
					{
						this.attributesInUse.Add(this.attributeMapFailedPasswordAnswerLockoutTime, null);
					}
					if (this.attributeMapFailedPasswordAnswerCount == null || this.attributeMapFailedPasswordAnswerTime == null || this.attributeMapFailedPasswordAnswerLockoutTime == null)
					{
						throw new ProviderException(SR.GetString("ADMembership_BadPasswordAnswerMappings_not_specified"));
					}
				}
				this.attributeMapPasswordQuestion = this.GetAttributeMapping(config, "attributeMapPasswordQuestion", out rangeUpperForSchemaAttribute);
				if (this.attributeMapPasswordQuestion != null)
				{
					if (rangeUpperForSchemaAttribute != -1 && rangeUpperForSchemaAttribute < this.maxPasswordQuestionLength)
					{
						this.maxPasswordQuestionLength = rangeUpperForSchemaAttribute;
					}
					this.attributesInUse.Add(this.attributeMapPasswordQuestion, null);
				}
				this.attributeMapPasswordAnswer = this.GetAttributeMapping(config, "attributeMapPasswordAnswer", out rangeUpperForSchemaAttribute);
				if (this.attributeMapPasswordAnswer != null)
				{
					if (rangeUpperForSchemaAttribute != -1 && rangeUpperForSchemaAttribute < this.maxPasswordAnswerLength)
					{
						this.maxPasswordAnswerLength = rangeUpperForSchemaAttribute;
					}
					this.attributesInUse.Add(this.attributeMapPasswordAnswer, null);
				}
				if (this.requiresQuestionAndAnswer && (this.attributeMapPasswordQuestion == null || this.attributeMapPasswordAnswer == null))
				{
					throw new ProviderException(SR.GetString("ADMembership_PasswordQuestionAnswerMapping_not_specified"));
				}
				if (this.directoryInfo.DirectoryType == DirectoryType.ADAM)
				{
					this.authTypeForValidation = AuthType.Basic;
				}
				else
				{
					this.authTypeForValidation = this.directoryInfo.GetLdapAuthenticationTypes(this.directoryInfo.ConnectionProtection, CredentialsType.NonWindows);
				}
				if (this.directoryInfo.DirectoryType == DirectoryType.AD)
				{
					if (this.enablePasswordReset)
					{
						this.directoryInfo.SelectServer();
					}
					this.directoryInfo.InitializeDomainAndForestName();
				}
			}
			this.connection = this.directoryInfo.CreateNewLdapConnection(this.authTypeForValidation);
			config.Remove("name");
			config.Remove("applicationName");
			config.Remove("connectionStringName");
			config.Remove("requiresUniqueEmail");
			config.Remove("enablePasswordReset");
			config.Remove("requiresQuestionAndAnswer");
			config.Remove("attributeMapPasswordQuestion");
			config.Remove("attributeMapPasswordAnswer");
			config.Remove("attributeMapUsername");
			config.Remove("attributeMapEmail");
			config.Remove("connectionProtection");
			config.Remove("connectionUsername");
			config.Remove("connectionPassword");
			config.Remove("clientSearchTimeout");
			config.Remove("serverSearchTimeout");
			config.Remove("enableSearchMethods");
			config.Remove("maxInvalidPasswordAttempts");
			config.Remove("passwordAttemptWindow");
			config.Remove("passwordAnswerAttemptLockoutDuration");
			config.Remove("attributeMapFailedPasswordAnswerCount");
			config.Remove("attributeMapFailedPasswordAnswerTime");
			config.Remove("attributeMapFailedPasswordAnswerLockoutTime");
			config.Remove("minRequiredPasswordLength");
			config.Remove("minRequiredNonalphanumericCharacters");
			config.Remove("passwordStrengthRegularExpression");
			if (config.Count > 0)
			{
				string key = config.GetKey(0);
				if (!string.IsNullOrEmpty(key))
				{
					throw new ProviderException(SR.GetString("Provider_unrecognized_attribute", new object[] { key }));
				}
			}
			this.initialized = true;
		}

		// Token: 0x060026F6 RID: 9974 RVA: 0x000A7480 File Offset: 0x000A6480
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.Assert, Unrestricted = true)]
		public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
		{
			status = MembershipCreateStatus.Success;
			MembershipUser membershipUser = null;
			if (!this.initialized)
			{
				throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
			}
			if (providerUserKey != null)
			{
				throw new NotSupportedException(SR.GetString("ADMembership_Setting_UserId_not_supported"));
			}
			if (passwordQuestion != null && this.attributeMapPasswordQuestion == null)
			{
				throw new NotSupportedException(SR.GetString("ADMembership_PasswordQ_not_supported"));
			}
			if (passwordAnswer != null && this.attributeMapPasswordAnswer == null)
			{
				throw new NotSupportedException(SR.GetString("ADMembership_PasswordA_not_supported"));
			}
			if (!SecUtility.ValidateParameter(ref username, true, true, true, this.maxUsernameLengthForCreation))
			{
				status = MembershipCreateStatus.InvalidUserName;
				return null;
			}
			if (this.usernameIsUPN && username.IndexOf('\\') != -1)
			{
				status = MembershipCreateStatus.InvalidUserName;
				return null;
			}
			if (!this.ValidatePassword(password, this.maxPasswordLength))
			{
				status = MembershipCreateStatus.InvalidPassword;
				return null;
			}
			if (!SecUtility.ValidateParameter(ref email, this.RequiresUniqueEmail, true, false, this.maxEmailLength))
			{
				status = MembershipCreateStatus.InvalidEmail;
				return null;
			}
			if (!SecUtility.ValidateParameter(ref passwordQuestion, this.RequiresQuestionAndAnswer, true, false, this.maxPasswordQuestionLength))
			{
				status = MembershipCreateStatus.InvalidQuestion;
				return null;
			}
			if (!SecUtility.ValidateParameter(ref passwordAnswer, this.RequiresQuestionAndAnswer, true, false, this.maxPasswordAnswerLength))
			{
				status = MembershipCreateStatus.InvalidAnswer;
				return null;
			}
			string text;
			if (!string.IsNullOrEmpty(passwordAnswer))
			{
				text = this.Encrypt(passwordAnswer);
				if (this.maxPasswordAnswerLength > 0 && text.Length > this.maxPasswordAnswerLength)
				{
					status = MembershipCreateStatus.InvalidAnswer;
					return null;
				}
			}
			else
			{
				text = passwordAnswer;
			}
			if (password.Length < this.MinRequiredPasswordLength)
			{
				status = MembershipCreateStatus.InvalidPassword;
				return null;
			}
			int num = 0;
			for (int i = 0; i < password.Length; i++)
			{
				if (!char.IsLetterOrDigit(password, i))
				{
					num++;
				}
			}
			if (num < this.MinRequiredNonAlphanumericCharacters)
			{
				status = MembershipCreateStatus.InvalidPassword;
				return null;
			}
			if (this.PasswordStrengthRegularExpression.Length > 0 && !Regex.IsMatch(password, this.PasswordStrengthRegularExpression))
			{
				status = MembershipCreateStatus.InvalidPassword;
				return null;
			}
			ValidatePasswordEventArgs validatePasswordEventArgs = new ValidatePasswordEventArgs(username, password, true);
			this.OnValidatingPassword(validatePasswordEventArgs);
			if (validatePasswordEventArgs.Cancel)
			{
				status = MembershipCreateStatus.InvalidPassword;
				return null;
			}
			try
			{
				DirectoryEntryHolder directoryEntry = ActiveDirectoryConnectionHelper.GetDirectoryEntry(this.directoryInfo, this.directoryInfo.CreationContainerDN, true);
				DirectoryEntry directoryEntry2 = null;
				DirectoryEntry directoryEntry3 = null;
				try
				{
					directoryEntry2 = directoryEntry.DirectoryEntry;
					directoryEntry2.AuthenticationType |= AuthenticationTypes.FastBind;
					directoryEntry3 = directoryEntry2.Children.Add(this.GetEscapedRdn("CN=" + username), "user");
					if (this.directoryInfo.DirectoryType == DirectoryType.AD)
					{
						string text2 = null;
						bool flag = false;
						if (this.usernameIsSAMAccountName)
						{
							text2 = username;
							flag = true;
						}
						else
						{
							int domainControllerLevel = this.GetDomainControllerLevel(directoryEntry2.Options.GetCurrentServerName());
							if (domainControllerLevel != 2)
							{
								text2 = this.GenerateAccountName();
								flag = true;
							}
						}
						if (flag)
						{
							directoryEntry3.Properties["sAMAccountName"].Value = text2;
						}
					}
					if (this.usernameIsUPN)
					{
						if (this.directoryInfo.DirectoryType == DirectoryType.AD && !this.IsUpnUnique(username))
						{
							status = MembershipCreateStatus.DuplicateUserName;
							return null;
						}
						directoryEntry3.Properties["userPrincipalName"].Value = username;
					}
					if (email != null)
					{
						if (this.RequiresUniqueEmail && !this.IsEmailUnique(directoryEntry2, username, email, false))
						{
							status = MembershipCreateStatus.DuplicateEmail;
							return null;
						}
						directoryEntry3.Properties[this.attributeMapEmail].Value = email;
					}
					if (passwordQuestion != null)
					{
						directoryEntry3.Properties[this.attributeMapPasswordQuestion].Value = passwordQuestion;
					}
					if (passwordAnswer != null)
					{
						directoryEntry3.Properties[this.attributeMapPasswordAnswer].Value = text;
					}
					try
					{
						directoryEntry3.CommitChanges();
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode == -2147019886 || ex.ErrorCode == -2147016691)
						{
							status = MembershipCreateStatus.DuplicateUserName;
							return null;
						}
						if (ex.ErrorCode != -2147024865 || !(ex is DirectoryServicesCOMException))
						{
							throw;
						}
						DirectoryServicesCOMException ex2 = ex as DirectoryServicesCOMException;
						if (ex2.ExtendedError == 1315)
						{
							status = MembershipCreateStatus.InvalidUserName;
							return null;
						}
						throw;
					}
					try
					{
						this.SetPasswordPortIfApplicable(directoryEntry3);
						directoryEntry3.Invoke("SetPassword", new object[] { password });
						if (isApproved)
						{
							if (this.directoryInfo.DirectoryType == DirectoryType.AD)
							{
								int num2 = (int)PropertyManager.GetPropertyValue(directoryEntry3, "userAccountControl");
								num2 &= -35;
								directoryEntry3.Properties["userAccountControl"].Value = num2;
							}
							else
							{
								directoryEntry3.Properties["msDS-UserAccountDisabled"].Value = false;
							}
							directoryEntry3.CommitChanges();
						}
						else if (this.directoryInfo.DirectoryType == DirectoryType.ADAM)
						{
							directoryEntry3.Properties["msDS-UserAccountDisabled"].Value = true;
							directoryEntry3.CommitChanges();
						}
						if (this.directoryInfo.DirectoryType == DirectoryType.ADAM)
						{
							DirectoryEntry directoryEntry4 = new DirectoryEntry(this.directoryInfo.GetADsPath("CN=Readers,CN=Roles," + this.directoryInfo.ADAMPartitionDN), this.directoryInfo.GetUsername(), this.directoryInfo.GetPassword(), this.directoryInfo.AuthenticationTypes);
							directoryEntry4.Properties["member"].Add(PropertyManager.GetPropertyValue(directoryEntry3, "distinguishedName"));
							directoryEntry4.CommitChanges();
						}
					}
					catch (COMException)
					{
						directoryEntry2.Children.Remove(directoryEntry3);
						throw;
					}
					catch (ProviderException)
					{
						directoryEntry2.Children.Remove(directoryEntry3);
						throw;
					}
					catch (TargetInvocationException ex3)
					{
						directoryEntry2.Children.Remove(directoryEntry3);
						if (!(ex3.InnerException is COMException))
						{
							throw;
						}
						COMException ex4 = (COMException)ex3.InnerException;
						int errorCode = ex4.ErrorCode;
						if (errorCode == -2147022651 || errorCode == -2147016657 || errorCode == -2147023571 || errorCode == -2147023569)
						{
							status = MembershipCreateStatus.InvalidPassword;
							return null;
						}
						if (errorCode == -2147463155 && this.directoryInfo.DirectoryType == DirectoryType.ADAM)
						{
							throw new ProviderException(SR.GetString("ADMembership_No_secure_conn_for_password"));
						}
						throw;
					}
					DirectoryEntry directoryEntry5 = null;
					bool flag2 = false;
					string text3;
					membershipUser = this.FindUser(directoryEntry3, "(objectClass=*)", global::System.DirectoryServices.SearchScope.Base, false, out directoryEntry5, out flag2, out text3);
				}
				finally
				{
					if (directoryEntry3 != null)
					{
						directoryEntry3.Dispose();
					}
					directoryEntry.Close();
				}
			}
			catch
			{
				throw;
			}
			return membershipUser;
		}

		// Token: 0x060026F7 RID: 9975 RVA: 0x000A7AF8 File Offset: 0x000A6AF8
		[DirectoryServicesPermission(SecurityAction.Assert, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
		{
			if (!this.initialized)
			{
				throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
			}
			if (newPasswordQuestion != null && this.attributeMapPasswordQuestion == null)
			{
				throw new NotSupportedException(SR.GetString("ADMembership_PasswordQ_not_supported"));
			}
			if (newPasswordAnswer != null && this.attributeMapPasswordAnswer == null)
			{
				throw new NotSupportedException(SR.GetString("ADMembership_PasswordA_not_supported"));
			}
			this.CheckUserName(ref username, this.maxUsernameLength, "username");
			this.CheckPassword(password, this.maxPasswordLength, "password");
			SecUtility.CheckParameter(ref newPasswordQuestion, this.RequiresQuestionAndAnswer, true, false, this.maxPasswordQuestionLength, "newPasswordQuestion");
			this.CheckPasswordAnswer(ref newPasswordAnswer, this.RequiresQuestionAndAnswer, this.maxPasswordAnswerLength, "newPasswordAnswer");
			string text;
			if (!string.IsNullOrEmpty(newPasswordAnswer))
			{
				text = this.Encrypt(newPasswordAnswer);
				if (this.maxPasswordAnswerLength > 0 && text.Length > this.maxPasswordAnswerLength)
				{
					throw new ArgumentException(SR.GetString("ADMembership_Parameter_too_long", new object[] { "newPasswordAnswer" }), "newPasswordAnswer");
				}
			}
			else
			{
				text = newPasswordAnswer;
			}
			try
			{
				DirectoryEntryHolder directoryEntry = ActiveDirectoryConnectionHelper.GetDirectoryEntry(this.directoryInfo, this.directoryInfo.ContainerDN, true);
				DirectoryEntry directoryEntry2 = directoryEntry.DirectoryEntry;
				DirectoryEntry directoryEntry3 = null;
				bool flag = false;
				try
				{
					string text3;
					if (this.EnablePasswordReset)
					{
						MembershipUser membershipUser;
						if (this.directoryInfo.DirectoryType == DirectoryType.AD && this.usernameIsUPN && username.IndexOf('@') == -1)
						{
							string text2 = null;
							membershipUser = this.FindUserAndSAMAccountName(directoryEntry2, string.Concat(new string[]
							{
								"(",
								this.attributeMapUsername,
								"=",
								this.GetEscapedFilterValue(username),
								")"
							}), out directoryEntry3, out flag, out text2);
							text3 = this.directoryInfo.DomainName + "\\" + text2;
						}
						else
						{
							membershipUser = this.FindUser(directoryEntry2, string.Concat(new string[]
							{
								"(",
								this.attributeMapUsername,
								"=",
								this.GetEscapedFilterValue(username),
								")"
							}), out directoryEntry3, out flag);
							text3 = username;
						}
						if (membershipUser == null)
						{
							return false;
						}
						if (membershipUser.IsLockedOut)
						{
							return false;
						}
					}
					else
					{
						if (this.directoryInfo.DirectoryType == DirectoryType.AD && this.usernameIsUPN && username.IndexOf('@') == -1)
						{
							string text4 = null;
							directoryEntry3 = this.FindUserEntryAndSAMAccountName(directoryEntry2, string.Concat(new string[]
							{
								"(",
								this.attributeMapUsername,
								"=",
								this.GetEscapedFilterValue(username),
								")"
							}), out text4);
							text3 = this.directoryInfo.DomainName + "\\" + text4;
						}
						else
						{
							directoryEntry3 = this.FindUserEntry(directoryEntry2, string.Concat(new string[]
							{
								"(",
								this.attributeMapUsername,
								"=",
								this.GetEscapedFilterValue(username),
								")"
							}));
							text3 = username;
						}
						if (directoryEntry3 == null)
						{
							return false;
						}
					}
					if (!this.ValidateCredentials(text3, password))
					{
						return false;
					}
					if (this.EnablePasswordReset && flag)
					{
						directoryEntry3.Properties[this.attributeMapFailedPasswordAnswerCount].Value = 0;
						directoryEntry3.Properties[this.attributeMapFailedPasswordAnswerTime].Value = 0;
						directoryEntry3.Properties[this.attributeMapFailedPasswordAnswerLockoutTime].Value = 0;
					}
					if (newPasswordQuestion == null)
					{
						if (this.attributeMapPasswordQuestion != null && directoryEntry3.Properties.Contains(this.attributeMapPasswordQuestion))
						{
							directoryEntry3.Properties[this.attributeMapPasswordQuestion].Clear();
						}
					}
					else
					{
						directoryEntry3.Properties[this.attributeMapPasswordQuestion].Value = newPasswordQuestion;
					}
					if (newPasswordAnswer == null)
					{
						if (this.attributeMapPasswordAnswer != null && directoryEntry3.Properties.Contains(this.attributeMapPasswordAnswer))
						{
							directoryEntry3.Properties[this.attributeMapPasswordAnswer].Clear();
						}
					}
					else
					{
						directoryEntry3.Properties[this.attributeMapPasswordAnswer].Value = text;
					}
					directoryEntry3.CommitChanges();
				}
				finally
				{
					if (directoryEntry3 != null)
					{
						directoryEntry3.Dispose();
					}
					directoryEntry.Close();
				}
			}
			catch
			{
				throw;
			}
			return true;
		}

		// Token: 0x060026F8 RID: 9976 RVA: 0x000A7F70 File Offset: 0x000A6F70
		public override string GetPassword(string username, string passwordAnswer)
		{
			throw new NotSupportedException(SR.GetString("ADMembership_PasswordRetrieval_not_supported_AD"));
		}

		// Token: 0x060026F9 RID: 9977 RVA: 0x000A7F84 File Offset: 0x000A6F84
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.Assert, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override bool ChangePassword(string username, string oldPassword, string newPassword)
		{
			if (!this.initialized)
			{
				throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
			}
			this.CheckUserName(ref username, this.maxUsernameLength, "username");
			this.CheckPassword(oldPassword, this.maxPasswordLength, "oldPassword");
			this.CheckPassword(newPassword, this.maxPasswordLength, "newPassword");
			if (newPassword.Length < this.MinRequiredPasswordLength)
			{
				throw new ArgumentException(SR.GetString("Password_too_short", new object[]
				{
					"newPassword",
					this.MinRequiredPasswordLength.ToString(CultureInfo.InvariantCulture)
				}));
			}
			int num = 0;
			for (int i = 0; i < newPassword.Length; i++)
			{
				if (!char.IsLetterOrDigit(newPassword, i))
				{
					num++;
				}
			}
			if (num < this.MinRequiredNonAlphanumericCharacters)
			{
				throw new ArgumentException(SR.GetString("Password_need_more_non_alpha_numeric_chars", new object[]
				{
					"newPassword",
					this.MinRequiredNonAlphanumericCharacters.ToString(CultureInfo.InvariantCulture)
				}));
			}
			if (this.PasswordStrengthRegularExpression.Length > 0 && !Regex.IsMatch(newPassword, this.PasswordStrengthRegularExpression))
			{
				throw new ArgumentException(SR.GetString("Password_does_not_match_regular_expression", new object[] { "newPassword" }));
			}
			ValidatePasswordEventArgs validatePasswordEventArgs = new ValidatePasswordEventArgs(username, newPassword, false);
			this.OnValidatingPassword(validatePasswordEventArgs);
			if (!validatePasswordEventArgs.Cancel)
			{
				try
				{
					DirectoryEntryHolder directoryEntry = ActiveDirectoryConnectionHelper.GetDirectoryEntry(this.directoryInfo, this.directoryInfo.ContainerDN, true);
					DirectoryEntry directoryEntry2 = directoryEntry.DirectoryEntry;
					DirectoryEntry directoryEntry3 = null;
					bool flag = false;
					try
					{
						string text2;
						if (this.EnablePasswordReset)
						{
							MembershipUser membershipUser;
							if (this.directoryInfo.DirectoryType == DirectoryType.AD && this.usernameIsUPN && username.IndexOf('@') == -1)
							{
								string text = null;
								membershipUser = this.FindUserAndSAMAccountName(directoryEntry2, string.Concat(new string[]
								{
									"(",
									this.attributeMapUsername,
									"=",
									this.GetEscapedFilterValue(username),
									")"
								}), out directoryEntry3, out flag, out text);
								text2 = this.directoryInfo.DomainName + "\\" + text;
							}
							else
							{
								membershipUser = this.FindUser(directoryEntry2, string.Concat(new string[]
								{
									"(",
									this.attributeMapUsername,
									"=",
									this.GetEscapedFilterValue(username),
									")"
								}), out directoryEntry3, out flag);
								text2 = username;
							}
							if (membershipUser == null)
							{
								return false;
							}
							if (membershipUser.IsLockedOut)
							{
								return false;
							}
						}
						else
						{
							if (this.directoryInfo.DirectoryType == DirectoryType.AD && this.usernameIsUPN && username.IndexOf('@') == -1)
							{
								string text3 = null;
								directoryEntry3 = this.FindUserEntryAndSAMAccountName(directoryEntry2, string.Concat(new string[]
								{
									"(",
									this.attributeMapUsername,
									"=",
									this.GetEscapedFilterValue(username),
									")"
								}), out text3);
								text2 = this.directoryInfo.DomainName + "\\" + text3;
							}
							else
							{
								directoryEntry3 = this.FindUserEntry(directoryEntry2, string.Concat(new string[]
								{
									"(",
									this.attributeMapUsername,
									"=",
									this.GetEscapedFilterValue(username),
									")"
								}));
								text2 = username;
							}
							if (directoryEntry3 == null)
							{
								return false;
							}
						}
						directoryEntry3.Username = (this.usernameIsSAMAccountName ? (this.directoryInfo.DomainName + "\\" + text2) : text2);
						directoryEntry3.Password = oldPassword;
						directoryEntry3.AuthenticationType = this.directoryInfo.GetAuthenticationTypes(this.directoryInfo.ConnectionProtection, (this.directoryInfo.DirectoryType == DirectoryType.AD) ? CredentialsType.Windows : CredentialsType.NonWindows);
						try
						{
							this.SetPasswordPortIfApplicable(directoryEntry3);
							directoryEntry3.Invoke("ChangePassword", new object[] { oldPassword, newPassword });
						}
						catch (COMException ex)
						{
							if (ex.ErrorCode == -2147023570)
							{
								return false;
							}
							throw;
						}
						catch (TargetInvocationException ex2)
						{
							if (!(ex2.InnerException is COMException))
							{
								throw;
							}
							COMException ex3 = (COMException)ex2.InnerException;
							int errorCode = ex3.ErrorCode;
							if (errorCode == -2147022651 || errorCode == -2147016657 || errorCode == -2147023571 || errorCode == -2147023569)
							{
								throw new MembershipPasswordException(SR.GetString("Membership_InvalidPassword"), ex3);
							}
							if (errorCode == -2147463155 && this.directoryInfo.DirectoryType == DirectoryType.ADAM)
							{
								throw new ProviderException(SR.GetString("ADMembership_No_secure_conn_for_password"));
							}
							throw;
						}
						if (this.EnablePasswordReset && flag)
						{
							directoryEntry3.Username = this.directoryInfo.GetUsername();
							directoryEntry3.Password = this.directoryInfo.GetPassword();
							directoryEntry3.AuthenticationType = this.directoryInfo.AuthenticationTypes;
							this.ResetBadPasswordAnswerAttributes(directoryEntry3);
						}
					}
					finally
					{
						if (directoryEntry3 != null)
						{
							directoryEntry3.Dispose();
						}
						directoryEntry.Close();
					}
				}
				catch
				{
					throw;
				}
				return true;
			}
			if (validatePasswordEventArgs.FailureInformation != null)
			{
				throw validatePasswordEventArgs.FailureInformation;
			}
			throw new ArgumentException(SR.GetString("Membership_Custom_Password_Validation_Failure"), "newPassword");
		}

		// Token: 0x060026FA RID: 9978 RVA: 0x000A851C File Offset: 0x000A751C
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.Assert, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override string ResetPassword(string username, string passwordAnswer)
		{
			string text = null;
			if (!this.initialized)
			{
				throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
			}
			if (!this.EnablePasswordReset)
			{
				throw new NotSupportedException(SR.GetString("Not_configured_to_support_password_resets"));
			}
			this.CheckUserName(ref username, this.maxUsernameLength, "username");
			this.CheckPasswordAnswer(ref passwordAnswer, this.RequiresQuestionAndAnswer, this.maxPasswordAnswerLength, "passwordAnswer");
			try
			{
				DirectoryEntryHolder directoryEntry = ActiveDirectoryConnectionHelper.GetDirectoryEntry(this.directoryInfo, this.directoryInfo.ContainerDN, true);
				DirectoryEntry directoryEntry2 = directoryEntry.DirectoryEntry;
				DirectoryEntry directoryEntry3 = null;
				bool flag = false;
				try
				{
					MembershipUser membershipUser = this.FindUser(directoryEntry2, string.Concat(new string[]
					{
						"(",
						this.attributeMapUsername,
						"=",
						this.GetEscapedFilterValue(username),
						")"
					}), out directoryEntry3, out flag);
					if (membershipUser == null)
					{
						throw new ProviderException(SR.GetString("Membership_UserNotFound"));
					}
					if (membershipUser.IsLockedOut)
					{
						throw new MembershipPasswordException(SR.GetString("Membership_AccountLockOut"));
					}
					string text2 = this.Decrypt((string)PropertyManager.GetPropertyValue(directoryEntry3, this.attributeMapPasswordAnswer));
					if (!StringUtil.EqualsIgnoreCase(passwordAnswer, text2))
					{
						this.UpdateBadPasswordAnswerAttributes(directoryEntry3);
						throw new MembershipPasswordException(SR.GetString("Membership_WrongAnswer"));
					}
					if (flag)
					{
						this.ResetBadPasswordAnswerAttributes(directoryEntry3);
					}
					this.SetPasswordPortIfApplicable(directoryEntry3);
					text = this.GeneratePassword();
					ValidatePasswordEventArgs validatePasswordEventArgs = new ValidatePasswordEventArgs(username, text, false);
					this.OnValidatingPassword(validatePasswordEventArgs);
					if (validatePasswordEventArgs.Cancel)
					{
						if (validatePasswordEventArgs.FailureInformation != null)
						{
							throw validatePasswordEventArgs.FailureInformation;
						}
						throw new ProviderException(SR.GetString("Membership_Custom_Password_Validation_Failure"));
					}
					else
					{
						directoryEntry3.Invoke("SetPassword", new object[] { text });
					}
				}
				catch (TargetInvocationException ex)
				{
					if (!(ex.InnerException is COMException))
					{
						throw;
					}
					COMException ex2 = (COMException)ex.InnerException;
					int errorCode = ex2.ErrorCode;
					if (errorCode == -2147022651 || errorCode == -2147016657 || errorCode == -2147023571 || errorCode == -2147023569)
					{
						throw new ProviderException(SR.GetString("ADMembership_Generated_password_not_complex"), ex2);
					}
					if (errorCode == -2147463155 && this.directoryInfo.DirectoryType == DirectoryType.ADAM)
					{
						throw new ProviderException(SR.GetString("ADMembership_No_secure_conn_for_password"));
					}
					throw;
				}
				finally
				{
					if (directoryEntry3 != null)
					{
						directoryEntry3.Dispose();
					}
					directoryEntry.Close();
				}
			}
			catch
			{
				throw;
			}
			return text;
		}

		// Token: 0x060026FB RID: 9979 RVA: 0x000A87C0 File Offset: 0x000A77C0
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.Assert, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override bool UnlockUser(string username)
		{
			if (!this.initialized)
			{
				throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
			}
			this.CheckUserName(ref username, this.maxUsernameLength, "username");
			try
			{
				DirectoryEntryHolder directoryEntry = ActiveDirectoryConnectionHelper.GetDirectoryEntry(this.directoryInfo, this.directoryInfo.ContainerDN, true);
				DirectoryEntry directoryEntry2 = directoryEntry.DirectoryEntry;
				DirectoryEntry directoryEntry3 = null;
				try
				{
					directoryEntry3 = this.FindUserEntry(directoryEntry2, string.Concat(new string[]
					{
						"(",
						this.attributeMapUsername,
						"=",
						this.GetEscapedFilterValue(username),
						")"
					}));
					if (directoryEntry3 == null)
					{
						return false;
					}
					directoryEntry3.Properties["lockoutTime"].Value = 0;
					if (this.EnablePasswordReset)
					{
						directoryEntry3.Properties[this.attributeMapFailedPasswordAnswerCount].Value = 0;
						directoryEntry3.Properties[this.attributeMapFailedPasswordAnswerTime].Value = 0;
						directoryEntry3.Properties[this.attributeMapFailedPasswordAnswerLockoutTime].Value = 0;
					}
					directoryEntry3.CommitChanges();
				}
				finally
				{
					if (directoryEntry3 != null)
					{
						directoryEntry3.Dispose();
					}
					directoryEntry.Close();
				}
			}
			catch
			{
				throw;
			}
			return true;
		}

		// Token: 0x060026FC RID: 9980 RVA: 0x000A8938 File Offset: 0x000A7938
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.Assert, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public override void UpdateUser(MembershipUser user)
		{
			bool flag = true;
			bool flag2 = true;
			bool flag3 = true;
			if (!this.initialized)
			{
				throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
			}
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			ActiveDirectoryMembershipUser activeDirectoryMembershipUser = user as ActiveDirectoryMembershipUser;
			if (activeDirectoryMembershipUser != null)
			{
				flag = activeDirectoryMembershipUser.emailModified;
				flag2 = activeDirectoryMembershipUser.commentModified;
				flag3 = activeDirectoryMembershipUser.isApprovedModified;
			}
			string userName = user.UserName;
			this.CheckUserName(ref userName, this.maxUsernameLength, "UserName");
			string email = user.Email;
			if (flag)
			{
				SecUtility.CheckParameter(ref email, this.RequiresUniqueEmail, true, false, this.maxEmailLength, "Email");
			}
			if (flag2 && user.Comment != null)
			{
				if (user.Comment.Length == 0)
				{
					throw new ArgumentException(SR.GetString("Parameter_can_not_be_empty", new object[] { "Comment" }), "Comment");
				}
				if (this.maxCommentLength > 0 && user.Comment.Length > this.maxCommentLength)
				{
					throw new ArgumentException(SR.GetString("Parameter_too_long", new object[]
					{
						"Comment",
						this.maxCommentLength.ToString(CultureInfo.InvariantCulture)
					}), "Comment");
				}
			}
			try
			{
				DirectoryEntryHolder directoryEntry = ActiveDirectoryConnectionHelper.GetDirectoryEntry(this.directoryInfo, this.directoryInfo.ContainerDN, true);
				DirectoryEntry directoryEntry2 = directoryEntry.DirectoryEntry;
				DirectoryEntry directoryEntry3 = null;
				try
				{
					directoryEntry3 = this.FindUserEntry(directoryEntry2, string.Concat(new string[]
					{
						"(",
						this.attributeMapUsername,
						"=",
						this.GetEscapedFilterValue(user.UserName),
						")"
					}));
					if (directoryEntry3 == null)
					{
						throw new ProviderException(SR.GetString("Membership_UserNotFound"));
					}
					if (flag || flag2 || flag3)
					{
						if (flag)
						{
							if (email == null)
							{
								if (directoryEntry3.Properties.Contains(this.attributeMapEmail))
								{
									directoryEntry3.Properties[this.attributeMapEmail].Clear();
								}
							}
							else
							{
								if (this.RequiresUniqueEmail && !this.IsEmailUnique(null, user.UserName, email, true))
								{
									throw new ProviderException(SR.GetString("Membership_DuplicateEmail"));
								}
								directoryEntry3.Properties[this.attributeMapEmail].Value = email;
							}
						}
						if (flag2)
						{
							if (user.Comment == null)
							{
								if (directoryEntry3.Properties.Contains("comment"))
								{
									directoryEntry3.Properties["comment"].Clear();
								}
							}
							else
							{
								directoryEntry3.Properties["comment"].Value = user.Comment;
							}
						}
						if (flag3)
						{
							if (this.directoryInfo.DirectoryType == DirectoryType.AD)
							{
								int num = (int)PropertyManager.GetPropertyValue(directoryEntry3, "userAccountControl");
								if (user.IsApproved)
								{
									num &= -3;
								}
								else
								{
									num |= 2;
								}
								directoryEntry3.Properties["userAccountControl"].Value = num;
							}
							else
							{
								directoryEntry3.Properties["msDS-UserAccountDisabled"].Value = !user.IsApproved;
							}
						}
						directoryEntry3.CommitChanges();
						if (activeDirectoryMembershipUser != null)
						{
							activeDirectoryMembershipUser.emailModified = false;
							activeDirectoryMembershipUser.commentModified = false;
							activeDirectoryMembershipUser.isApprovedModified = false;
						}
					}
				}
				finally
				{
					if (directoryEntry3 != null)
					{
						directoryEntry3.Dispose();
					}
					directoryEntry.Close();
				}
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x060026FD RID: 9981 RVA: 0x000A8CB8 File Offset: 0x000A7CB8
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override bool ValidateUser(string username, string password)
		{
			if (this.ValidateUserCore(username, password))
			{
				PerfCounters.IncrementCounter(AppPerfCounter.MEMBER_SUCCESS);
				WebBaseEvent.RaiseSystemEvent(null, 4002, username);
				return true;
			}
			PerfCounters.IncrementCounter(AppPerfCounter.MEMBER_FAIL);
			WebBaseEvent.RaiseSystemEvent(null, 4006, username);
			return false;
		}

		// Token: 0x060026FE RID: 9982 RVA: 0x000A8CF0 File Offset: 0x000A7CF0
		[DirectoryServicesPermission(SecurityAction.Assert, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		private bool ValidateUserCore(string username, string password)
		{
			if (!this.initialized)
			{
				throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
			}
			if (!SecUtility.ValidateParameter(ref username, true, true, true, this.maxUsernameLength))
			{
				return false;
			}
			if (this.usernameIsUPN && username.IndexOf('\\') != -1)
			{
				return false;
			}
			if (!this.ValidatePassword(password, this.maxPasswordLength))
			{
				return false;
			}
			bool flag = false;
			try
			{
				DirectoryEntryHolder directoryEntry = ActiveDirectoryConnectionHelper.GetDirectoryEntry(this.directoryInfo, this.directoryInfo.ContainerDN, true);
				DirectoryEntry directoryEntry2 = directoryEntry.DirectoryEntry;
				DirectoryEntry directoryEntry3 = null;
				bool flag2 = false;
				try
				{
					string text2;
					if (this.EnablePasswordReset)
					{
						MembershipUser membershipUser;
						if (this.directoryInfo.DirectoryType == DirectoryType.AD && this.usernameIsUPN && username.IndexOf('@') == -1)
						{
							string text = null;
							membershipUser = this.FindUserAndSAMAccountName(directoryEntry2, string.Concat(new string[]
							{
								"(",
								this.attributeMapUsername,
								"=",
								this.GetEscapedFilterValue(username),
								")"
							}), out directoryEntry3, out flag2, out text);
							text2 = this.directoryInfo.DomainName + "\\" + text;
						}
						else
						{
							membershipUser = this.FindUser(directoryEntry2, string.Concat(new string[]
							{
								"(",
								this.attributeMapUsername,
								"=",
								this.GetEscapedFilterValue(username),
								")"
							}), out directoryEntry3, out flag2);
							text2 = username;
						}
						if (membershipUser == null)
						{
							return false;
						}
						if (membershipUser.IsLockedOut)
						{
							return false;
						}
					}
					else
					{
						if (this.directoryInfo.DirectoryType == DirectoryType.AD && this.usernameIsUPN && username.IndexOf('@') == -1)
						{
							string text3 = null;
							directoryEntry3 = this.FindUserEntryAndSAMAccountName(directoryEntry2, string.Concat(new string[]
							{
								"(",
								this.attributeMapUsername,
								"=",
								this.GetEscapedFilterValue(username),
								")"
							}), out text3);
							text2 = this.directoryInfo.DomainName + "\\" + text3;
						}
						else
						{
							directoryEntry3 = this.FindUserEntry(directoryEntry2, string.Concat(new string[]
							{
								"(",
								this.attributeMapUsername,
								"=",
								this.GetEscapedFilterValue(username),
								")"
							}));
							text2 = username;
						}
						if (directoryEntry3 == null)
						{
							return false;
						}
					}
					flag = this.ValidateCredentials(text2, password);
					if (this.EnablePasswordReset && flag && flag2)
					{
						this.ResetBadPasswordAnswerAttributes(directoryEntry3);
					}
				}
				finally
				{
					if (directoryEntry3 != null)
					{
						directoryEntry3.Dispose();
					}
					directoryEntry.Close();
				}
			}
			catch
			{
				throw;
			}
			return flag;
		}

		// Token: 0x060026FF RID: 9983 RVA: 0x000A8FCC File Offset: 0x000A7FCC
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.Assert, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			MembershipUser membershipUser = null;
			if (!this.initialized)
			{
				throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
			}
			if (providerUserKey == null)
			{
				throw new ArgumentNullException("providerUserKey");
			}
			if (!(providerUserKey is SecurityIdentifier))
			{
				throw new ArgumentException(SR.GetString("ADMembership_InvalidProviderUserKey"), "providerUserKey");
			}
			try
			{
				DirectoryEntryHolder directoryEntry = ActiveDirectoryConnectionHelper.GetDirectoryEntry(this.directoryInfo, this.directoryInfo.ContainerDN, true);
				DirectoryEntry directoryEntry2 = directoryEntry.DirectoryEntry;
				try
				{
					SecurityIdentifier securityIdentifier = providerUserKey as SecurityIdentifier;
					StringBuilder stringBuilder = new StringBuilder();
					int binaryLength = securityIdentifier.BinaryLength;
					byte[] array = new byte[binaryLength];
					securityIdentifier.GetBinaryForm(array, 0);
					for (int i = 0; i < binaryLength; i++)
					{
						stringBuilder.Append("\\");
						stringBuilder.Append(array[i].ToString("x2", NumberFormatInfo.InvariantInfo));
					}
					bool flag = false;
					DirectoryEntry directoryEntry3;
					membershipUser = this.FindUser(directoryEntry2, string.Concat(new string[]
					{
						"(",
						this.attributeMapUsername,
						"=*)(objectSid=",
						stringBuilder.ToString(),
						")"
					}), out directoryEntry3, out flag);
				}
				finally
				{
					directoryEntry.Close();
				}
			}
			catch
			{
				throw;
			}
			return membershipUser;
		}

		// Token: 0x06002700 RID: 9984 RVA: 0x000A911C File Offset: 0x000A811C
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.Assert, Unrestricted = true)]
		public override MembershipUser GetUser(string username, bool userIsOnline)
		{
			MembershipUser membershipUser = null;
			if (!this.initialized)
			{
				throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
			}
			this.CheckUserName(ref username, this.maxUsernameLength, "username");
			try
			{
				DirectoryEntryHolder directoryEntry = ActiveDirectoryConnectionHelper.GetDirectoryEntry(this.directoryInfo, this.directoryInfo.ContainerDN, true);
				DirectoryEntry directoryEntry2 = directoryEntry.DirectoryEntry;
				try
				{
					bool flag = false;
					DirectoryEntry directoryEntry3;
					membershipUser = this.FindUser(directoryEntry2, string.Concat(new string[]
					{
						"(",
						this.attributeMapUsername,
						"=",
						this.GetEscapedFilterValue(username),
						")"
					}), out directoryEntry3, out flag);
				}
				finally
				{
					directoryEntry.Close();
				}
			}
			catch
			{
				throw;
			}
			return membershipUser;
		}

		// Token: 0x06002701 RID: 9985 RVA: 0x000A91F0 File Offset: 0x000A81F0
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.Assert, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override string GetUserNameByEmail(string email)
		{
			if (!this.initialized)
			{
				throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
			}
			SecUtility.CheckParameter(ref email, false, true, false, this.maxEmailLength, "email");
			string text = null;
			try
			{
				DirectoryEntryHolder directoryEntry = ActiveDirectoryConnectionHelper.GetDirectoryEntry(this.directoryInfo, this.directoryInfo.ContainerDN, true);
				DirectoryEntry directoryEntry2 = directoryEntry.DirectoryEntry;
				SearchResultCollection searchResultCollection = null;
				try
				{
					DirectorySearcher directorySearcher = new DirectorySearcher(directoryEntry2);
					if (email != null)
					{
						directorySearcher.Filter = string.Concat(new string[]
						{
							"(&(objectCategory=person)(objectClass=user)(",
							this.attributeMapUsername,
							"=*)(",
							this.attributeMapEmail,
							"=",
							this.GetEscapedFilterValue(email),
							"))"
						});
					}
					else
					{
						directorySearcher.Filter = string.Concat(new string[] { "(&(objectCategory=person)(objectClass=user)(", this.attributeMapUsername, "=*)(!(", this.attributeMapEmail, "=*)))" });
					}
					directorySearcher.SearchScope = global::System.DirectoryServices.SearchScope.Subtree;
					directorySearcher.PropertiesToLoad.Add(this.attributeMapUsername);
					if (this.directoryInfo.ClientSearchTimeout != -1)
					{
						directorySearcher.ClientTimeout = new TimeSpan(0, this.directoryInfo.ClientSearchTimeout, 0);
					}
					if (this.directoryInfo.ServerSearchTimeout != -1)
					{
						directorySearcher.ServerPageTimeLimit = new TimeSpan(0, this.directoryInfo.ServerSearchTimeout, 0);
					}
					searchResultCollection = directorySearcher.FindAll();
					bool flag = false;
					foreach (object obj in searchResultCollection)
					{
						SearchResult searchResult = (SearchResult)obj;
						if (!flag)
						{
							text = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, this.attributeMapUsername);
							flag = true;
							if (!this.RequiresUniqueEmail)
							{
								break;
							}
						}
						else
						{
							if (this.RequiresUniqueEmail)
							{
								throw new ProviderException(SR.GetString("Membership_more_than_one_user_with_email"));
							}
							break;
						}
					}
				}
				finally
				{
					if (searchResultCollection != null)
					{
						searchResultCollection.Dispose();
					}
					directoryEntry.Close();
				}
			}
			catch
			{
				throw;
			}
			return text;
		}

		// Token: 0x06002702 RID: 9986 RVA: 0x000A9444 File Offset: 0x000A8444
		[DirectoryServicesPermission(SecurityAction.Assert, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public override bool DeleteUser(string username, bool deleteAllRelatedData)
		{
			if (!this.initialized)
			{
				throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
			}
			this.CheckUserName(ref username, this.maxUsernameLength, "username");
			try
			{
				DirectoryEntryHolder directoryEntry = ActiveDirectoryConnectionHelper.GetDirectoryEntry(this.directoryInfo, this.directoryInfo.CreationContainerDN, true);
				DirectoryEntry directoryEntry2 = directoryEntry.DirectoryEntry;
				directoryEntry2.AuthenticationType |= AuthenticationTypes.FastBind;
				DirectoryEntry directoryEntry3 = null;
				try
				{
					string text;
					directoryEntry3 = this.FindUserEntry(directoryEntry2, string.Concat(new string[]
					{
						"(",
						this.attributeMapUsername,
						"=",
						this.GetEscapedFilterValue(username),
						")"
					}), global::System.DirectoryServices.SearchScope.OneLevel, false, out text);
					if (directoryEntry3 == null)
					{
						return false;
					}
					directoryEntry2.Children.Remove(directoryEntry3);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode == -2147016656)
					{
						return false;
					}
					throw;
				}
				finally
				{
					if (directoryEntry3 != null)
					{
						directoryEntry3.Dispose();
					}
					directoryEntry.Close();
				}
			}
			catch
			{
				throw;
			}
			return true;
		}

		// Token: 0x06002703 RID: 9987 RVA: 0x000A9568 File Offset: 0x000A8568
		public virtual string GeneratePassword()
		{
			return Membership.GeneratePassword((this.MinRequiredPasswordLength < 14) ? 14 : this.MinRequiredPasswordLength, this.MinRequiredNonAlphanumericCharacters);
		}

		// Token: 0x06002704 RID: 9988 RVA: 0x000A9589 File Offset: 0x000A8589
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.Assert, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
			return this.FindUsersByName("*", pageIndex, pageSize, out totalRecords);
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x000A9599 File Offset: 0x000A8599
		public override int GetNumberOfUsersOnline()
		{
			throw new NotSupportedException(SR.GetString("ADMembership_OnlineUsers_not_supported"));
		}

		// Token: 0x06002706 RID: 9990 RVA: 0x000A95AC File Offset: 0x000A85AC
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.Assert, Unrestricted = true)]
		public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			if (!this.initialized)
			{
				throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
			}
			if (!this.EnableSearchMethods)
			{
				throw new NotSupportedException(SR.GetString("ADMembership_Provider_SearchMethods_not_supported"));
			}
			SecUtility.CheckParameter(ref usernameToMatch, true, true, true, this.maxUsernameLength, "usernameToMatch");
			if (pageIndex < 0)
			{
				throw new ArgumentException(SR.GetString("PageIndex_bad"), "pageIndex");
			}
			if (pageSize < 1)
			{
				throw new ArgumentException(SR.GetString("PageSize_bad"), "pageSize");
			}
			long num = (long)pageIndex * (long)pageSize + (long)pageSize - 1L;
			if (num > 2147483647L)
			{
				throw new ArgumentException(SR.GetString("PageIndex_PageSize_bad"), "pageIndex and pageSize");
			}
			MembershipUserCollection membershipUserCollection;
			try
			{
				DirectoryEntryHolder directoryEntry = ActiveDirectoryConnectionHelper.GetDirectoryEntry(this.directoryInfo, this.directoryInfo.ContainerDN, true);
				DirectoryEntry directoryEntry2 = directoryEntry.DirectoryEntry;
				try
				{
					totalRecords = 0;
					membershipUserCollection = this.FindUsers(directoryEntry2, string.Concat(new string[]
					{
						"(",
						this.attributeMapUsername,
						"=",
						this.GetEscapedFilterValue(usernameToMatch, false),
						")"
					}), this.attributeMapUsername, pageIndex, pageSize, out totalRecords);
				}
				finally
				{
					directoryEntry.Close();
				}
			}
			catch
			{
				throw;
			}
			return membershipUserCollection;
		}

		// Token: 0x06002707 RID: 9991 RVA: 0x000A96F8 File Offset: 0x000A86F8
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.Assert, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			if (!this.initialized)
			{
				throw new InvalidOperationException(SR.GetString("ADMembership_Provider_not_initialized"));
			}
			if (!this.EnableSearchMethods)
			{
				throw new NotSupportedException(SR.GetString("ADMembership_Provider_SearchMethods_not_supported"));
			}
			SecUtility.CheckParameter(ref emailToMatch, false, true, false, this.maxEmailLength, "emailToMatch");
			if (pageIndex < 0)
			{
				throw new ArgumentException(SR.GetString("PageIndex_bad"), "pageIndex");
			}
			if (pageSize < 1)
			{
				throw new ArgumentException(SR.GetString("PageSize_bad"), "pageSize");
			}
			long num = (long)pageIndex * (long)pageSize + (long)pageSize - 1L;
			if (num > 2147483647L)
			{
				throw new ArgumentException(SR.GetString("PageIndex_PageSize_bad"), "pageIndex and pageSize");
			}
			MembershipUserCollection membershipUserCollection;
			try
			{
				DirectoryEntryHolder directoryEntry = ActiveDirectoryConnectionHelper.GetDirectoryEntry(this.directoryInfo, this.directoryInfo.ContainerDN, true);
				DirectoryEntry directoryEntry2 = directoryEntry.DirectoryEntry;
				try
				{
					totalRecords = 0;
					string text;
					if (emailToMatch != null)
					{
						text = string.Concat(new string[]
						{
							"(",
							this.attributeMapUsername,
							"=*)(",
							this.attributeMapEmail,
							"=",
							this.GetEscapedFilterValue(emailToMatch, false),
							")"
						});
					}
					else
					{
						text = string.Concat(new string[] { "(", this.attributeMapUsername, "=*)(!(", this.attributeMapEmail, "=*))" });
					}
					membershipUserCollection = this.FindUsers(directoryEntry2, text, this.attributeMapEmail, pageIndex, pageSize, out totalRecords);
				}
				finally
				{
					directoryEntry.Close();
				}
			}
			catch
			{
				throw;
			}
			return membershipUserCollection;
		}

		// Token: 0x06002708 RID: 9992 RVA: 0x000A98A4 File Offset: 0x000A88A4
		private bool ValidateCredentials(string username, string password)
		{
			bool flag = false;
			NetworkCredential networkCredential = (this.usernameIsSAMAccountName ? new NetworkCredential(username, password, this.directoryInfo.DomainName) : DirectoryInformation.GetCredentialsWithDomain(new NetworkCredential(username, password)));
			if (this.directoryInfo.ConcurrentBindSupported)
			{
				try
				{
					this.connection.Bind(networkCredential);
					return true;
				}
				catch (LdapException ex)
				{
					if (ex.ErrorCode == 49)
					{
						return false;
					}
					throw;
				}
			}
			LdapConnection ldapConnection = this.directoryInfo.CreateNewLdapConnection(this.authTypeForValidation);
			try
			{
				ldapConnection.Bind(networkCredential);
				flag = true;
			}
			catch (LdapException ex2)
			{
				if (ex2.ErrorCode != 49)
				{
					throw;
				}
				flag = false;
			}
			finally
			{
				ldapConnection.Dispose();
			}
			return flag;
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x000A9970 File Offset: 0x000A8970
		private DirectoryEntry FindUserEntryAndSAMAccountName(DirectoryEntry containerEntry, string filter, out string sAMAccountName)
		{
			return this.FindUserEntry(containerEntry, filter, global::System.DirectoryServices.SearchScope.Subtree, true, out sAMAccountName);
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x000A9980 File Offset: 0x000A8980
		private DirectoryEntry FindUserEntry(DirectoryEntry containerEntry, string filter)
		{
			string text;
			return this.FindUserEntry(containerEntry, filter, global::System.DirectoryServices.SearchScope.Subtree, false, out text);
		}

		// Token: 0x0600270B RID: 9995 RVA: 0x000A999C File Offset: 0x000A899C
		private DirectoryEntry FindUserEntry(DirectoryEntry containerEntry, string filter, global::System.DirectoryServices.SearchScope searchScope, bool retrieveSAMAccountName, out string sAMAccountName)
		{
			DirectorySearcher directorySearcher = new DirectorySearcher(containerEntry);
			directorySearcher.SearchScope = searchScope;
			directorySearcher.Filter = "(&(objectCategory=person)(objectClass=user)" + filter + ")";
			if (this.directoryInfo.ClientSearchTimeout != -1)
			{
				directorySearcher.ClientTimeout = new TimeSpan(0, this.directoryInfo.ClientSearchTimeout, 0);
			}
			if (this.directoryInfo.ServerSearchTimeout != -1)
			{
				directorySearcher.ServerPageTimeLimit = new TimeSpan(0, this.directoryInfo.ServerSearchTimeout, 0);
			}
			if (retrieveSAMAccountName)
			{
				directorySearcher.PropertiesToLoad.Add("sAMAccountName");
			}
			SearchResult searchResult = directorySearcher.FindOne();
			sAMAccountName = null;
			if (searchResult != null)
			{
				if (retrieveSAMAccountName)
				{
					sAMAccountName = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, "sAMAccountName");
				}
				return searchResult.GetDirectoryEntry();
			}
			return null;
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x000A9A5B File Offset: 0x000A8A5B
		private MembershipUser FindUserAndSAMAccountName(DirectoryEntry containerEntry, string filter, out DirectoryEntry userEntry, out bool resetBadPasswordAnswerAttributes, out string sAMAccountName)
		{
			return this.FindUser(containerEntry, filter, global::System.DirectoryServices.SearchScope.Subtree, true, out userEntry, out resetBadPasswordAnswerAttributes, out sAMAccountName);
		}

		// Token: 0x0600270D RID: 9997 RVA: 0x000A9A6C File Offset: 0x000A8A6C
		private MembershipUser FindUser(DirectoryEntry containerEntry, string filter, out DirectoryEntry userEntry, out bool resetBadPasswordAnswerAttributes)
		{
			string text;
			return this.FindUser(containerEntry, filter, global::System.DirectoryServices.SearchScope.Subtree, false, out userEntry, out resetBadPasswordAnswerAttributes, out text);
		}

		// Token: 0x0600270E RID: 9998 RVA: 0x000A9A88 File Offset: 0x000A8A88
		private MembershipUser FindUser(DirectoryEntry containerEntry, string filter, global::System.DirectoryServices.SearchScope searchScope, bool retrieveSAMAccountName, out DirectoryEntry userEntry, out bool resetBadPasswordAnswerAttributes, out string sAMAccountName)
		{
			MembershipUser membershipUser = null;
			DirectorySearcher directorySearcher = new DirectorySearcher(containerEntry);
			directorySearcher.SearchScope = searchScope;
			directorySearcher.Filter = "(&(objectCategory=person)(objectClass=user)" + filter + ")";
			if (this.directoryInfo.ClientSearchTimeout != -1)
			{
				directorySearcher.ClientTimeout = new TimeSpan(0, this.directoryInfo.ClientSearchTimeout, 0);
			}
			if (this.directoryInfo.ServerSearchTimeout != -1)
			{
				directorySearcher.ServerPageTimeLimit = new TimeSpan(0, this.directoryInfo.ServerSearchTimeout, 0);
			}
			directorySearcher.PropertiesToLoad.Add(this.attributeMapUsername);
			directorySearcher.PropertiesToLoad.Add("objectSid");
			directorySearcher.PropertiesToLoad.Add(this.attributeMapEmail);
			directorySearcher.PropertiesToLoad.Add("comment");
			directorySearcher.PropertiesToLoad.Add("whenCreated");
			directorySearcher.PropertiesToLoad.Add("pwdLastSet");
			directorySearcher.PropertiesToLoad.Add("msDS-User-Account-Control-Computed");
			directorySearcher.PropertiesToLoad.Add("lockoutTime");
			if (retrieveSAMAccountName)
			{
				directorySearcher.PropertiesToLoad.Add("sAMAccountName");
			}
			if (this.attributeMapPasswordQuestion != null)
			{
				directorySearcher.PropertiesToLoad.Add(this.attributeMapPasswordQuestion);
			}
			if (this.directoryInfo.DirectoryType == DirectoryType.AD)
			{
				directorySearcher.PropertiesToLoad.Add("userAccountControl");
			}
			else
			{
				directorySearcher.PropertiesToLoad.Add("msDS-UserAccountDisabled");
			}
			if (this.EnablePasswordReset)
			{
				directorySearcher.PropertiesToLoad.Add(this.attributeMapFailedPasswordAnswerCount);
				directorySearcher.PropertiesToLoad.Add(this.attributeMapFailedPasswordAnswerTime);
				directorySearcher.PropertiesToLoad.Add(this.attributeMapFailedPasswordAnswerLockoutTime);
			}
			SearchResult searchResult = directorySearcher.FindOne();
			resetBadPasswordAnswerAttributes = false;
			sAMAccountName = null;
			if (searchResult != null)
			{
				membershipUser = this.GetMembershipUserFromSearchResult(searchResult);
				userEntry = searchResult.GetDirectoryEntry();
				if (retrieveSAMAccountName)
				{
					sAMAccountName = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, "sAMAccountName");
				}
				if (this.EnablePasswordReset && searchResult.Properties.Contains(this.attributeMapFailedPasswordAnswerCount))
				{
					resetBadPasswordAnswerAttributes = (int)PropertyManager.GetSearchResultPropertyValue(searchResult, this.attributeMapFailedPasswordAnswerCount) > 0;
				}
			}
			else
			{
				userEntry = null;
			}
			return membershipUser;
		}

		// Token: 0x0600270F RID: 9999 RVA: 0x000A9CA4 File Offset: 0x000A8CA4
		private MembershipUserCollection FindUsers(DirectoryEntry containerEntry, string filter, string sortKey, int pageIndex, int pageSize, out int totalRecords)
		{
			MembershipUserCollection membershipUserCollection = new MembershipUserCollection();
			int num = (pageIndex + 1) * pageSize;
			int num2 = num - pageSize + 1;
			DirectorySearcher directorySearcher = new DirectorySearcher(containerEntry);
			directorySearcher.SearchScope = global::System.DirectoryServices.SearchScope.Subtree;
			directorySearcher.Filter = "(&(objectCategory=person)(objectClass=user)" + filter + ")";
			if (this.directoryInfo.ClientSearchTimeout != -1)
			{
				directorySearcher.ClientTimeout = new TimeSpan(0, this.directoryInfo.ClientSearchTimeout, 0);
			}
			if (this.directoryInfo.ServerSearchTimeout != -1)
			{
				directorySearcher.ServerPageTimeLimit = new TimeSpan(0, this.directoryInfo.ServerSearchTimeout, 0);
			}
			directorySearcher.PropertiesToLoad.Add(this.attributeMapUsername);
			directorySearcher.PropertiesToLoad.Add("objectSid");
			directorySearcher.PropertiesToLoad.Add(this.attributeMapEmail);
			directorySearcher.PropertiesToLoad.Add("comment");
			directorySearcher.PropertiesToLoad.Add("whenCreated");
			directorySearcher.PropertiesToLoad.Add("pwdLastSet");
			directorySearcher.PropertiesToLoad.Add("msDS-User-Account-Control-Computed");
			directorySearcher.PropertiesToLoad.Add("lockoutTime");
			if (this.attributeMapPasswordQuestion != null)
			{
				directorySearcher.PropertiesToLoad.Add(this.attributeMapPasswordQuestion);
			}
			if (this.directoryInfo.DirectoryType == DirectoryType.AD)
			{
				directorySearcher.PropertiesToLoad.Add("userAccountControl");
			}
			else
			{
				directorySearcher.PropertiesToLoad.Add("msDS-UserAccountDisabled");
			}
			if (this.EnablePasswordReset)
			{
				directorySearcher.PropertiesToLoad.Add(this.attributeMapFailedPasswordAnswerCount);
				directorySearcher.PropertiesToLoad.Add(this.attributeMapFailedPasswordAnswerTime);
				directorySearcher.PropertiesToLoad.Add(this.attributeMapFailedPasswordAnswerLockoutTime);
			}
			directorySearcher.PageSize = 512;
			directorySearcher.Sort = new SortOption(sortKey, SortDirection.Ascending);
			SearchResultCollection searchResultCollection = directorySearcher.FindAll();
			try
			{
				int num3 = 0;
				totalRecords = 0;
				foreach (object obj in searchResultCollection)
				{
					SearchResult searchResult = (SearchResult)obj;
					num3++;
					if (num3 >= num2 && num3 <= num)
					{
						membershipUserCollection.Add(this.GetMembershipUserFromSearchResult(searchResult));
					}
				}
				totalRecords = num3;
			}
			finally
			{
				searchResultCollection.Dispose();
			}
			return membershipUserCollection;
		}

		// Token: 0x06002710 RID: 10000 RVA: 0x000A9EF4 File Offset: 0x000A8EF4
		private void CheckPasswordAnswer(ref string passwordAnswer, bool checkForNull, int maxSize, string paramName)
		{
			if (passwordAnswer == null)
			{
				if (checkForNull)
				{
					throw new ArgumentNullException(paramName);
				}
				return;
			}
			else
			{
				passwordAnswer = passwordAnswer.Trim();
				if (passwordAnswer.Length < 1)
				{
					throw new ArgumentException(SR.GetString("Parameter_can_not_be_empty", new object[] { paramName }), paramName);
				}
				if (maxSize > 0 && passwordAnswer.Length > maxSize)
				{
					throw new ArgumentException(SR.GetString("ADMembership_Parameter_too_long", new object[] { paramName }), paramName);
				}
				return;
			}
		}

		// Token: 0x06002711 RID: 10001 RVA: 0x000A9F70 File Offset: 0x000A8F70
		private bool ValidatePassword(string password, int maxSize)
		{
			return password != null && password.Trim().Length >= 1 && (maxSize <= 0 || password.Length <= maxSize);
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x000A9F98 File Offset: 0x000A8F98
		private void CheckPassword(string password, int maxSize, string paramName)
		{
			if (password == null)
			{
				throw new ArgumentNullException(paramName);
			}
			if (password.Trim().Length < 1)
			{
				throw new ArgumentException(SR.GetString("Parameter_can_not_be_empty", new object[] { paramName }), paramName);
			}
			if (maxSize > 0 && password.Length > maxSize)
			{
				throw new ArgumentException(SR.GetString("Parameter_too_long", new object[]
				{
					paramName,
					maxSize.ToString(CultureInfo.InvariantCulture)
				}), paramName);
			}
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x000AA014 File Offset: 0x000A9014
		private void CheckUserName(ref string username, int maxSize, string paramName)
		{
			SecUtility.CheckParameter(ref username, true, true, true, maxSize, paramName);
			if (this.usernameIsUPN && username.IndexOf('\\') != -1)
			{
				throw new ArgumentException(SR.GetString("ADMembership_UPN_contains_backslash", new object[] { paramName }), paramName);
			}
		}

		// Token: 0x06002714 RID: 10004 RVA: 0x000AA060 File Offset: 0x000A9060
		private int GetDomainControllerLevel(string serverName)
		{
			int num = 0;
			DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://" + serverName + "/RootDSE", this.directoryInfo.GetUsername(), this.directoryInfo.GetPassword(), this.directoryInfo.AuthenticationTypes);
			string text = (string)directoryEntry.Properties["domainControllerFunctionality"].Value;
			if (text != null)
			{
				num = int.Parse(text, NumberFormatInfo.InvariantInfo);
			}
			return num;
		}

		// Token: 0x06002715 RID: 10005 RVA: 0x000AA0D4 File Offset: 0x000A90D4
		private void UpdateBadPasswordAnswerAttributes(DirectoryEntry userEntry)
		{
			bool flag = false;
			DateTime utcNow = DateTime.UtcNow;
			if (userEntry.Properties.Contains(this.attributeMapFailedPasswordAnswerTime))
			{
				DateTime dateTimeFromLargeInteger = this.GetDateTimeFromLargeInteger((NativeComInterfaces.IAdsLargeInteger)PropertyManager.GetPropertyValue(userEntry, this.attributeMapFailedPasswordAnswerTime));
				TimeSpan timeSpan = utcNow.Subtract(dateTimeFromLargeInteger);
				flag = timeSpan <= new TimeSpan(0, this.PasswordAttemptWindow, 0);
			}
			int num = 0;
			if (userEntry.Properties.Contains(this.attributeMapFailedPasswordAnswerCount))
			{
				num = (int)PropertyManager.GetPropertyValue(userEntry, this.attributeMapFailedPasswordAnswerCount);
			}
			int num2;
			if (flag && num > 0)
			{
				num2 = num + 1;
			}
			else
			{
				num2 = 1;
			}
			userEntry.Properties[this.attributeMapFailedPasswordAnswerCount].Value = num2;
			userEntry.Properties[this.attributeMapFailedPasswordAnswerTime].Value = this.GetLargeIntegerFromDateTime(utcNow);
			if (num2 >= this.maxInvalidPasswordAttempts)
			{
				userEntry.Properties[this.attributeMapFailedPasswordAnswerLockoutTime].Value = this.GetLargeIntegerFromDateTime(utcNow);
			}
			userEntry.CommitChanges();
		}

		// Token: 0x06002716 RID: 10006 RVA: 0x000AA1D4 File Offset: 0x000A91D4
		private void ResetBadPasswordAnswerAttributes(DirectoryEntry userEntry)
		{
			userEntry.Properties[this.attributeMapFailedPasswordAnswerCount].Value = 0;
			userEntry.Properties[this.attributeMapFailedPasswordAnswerTime].Value = 0;
			userEntry.Properties[this.attributeMapFailedPasswordAnswerLockoutTime].Value = 0;
			userEntry.CommitChanges();
		}

		// Token: 0x06002717 RID: 10007 RVA: 0x000AA23C File Offset: 0x000A923C
		private MembershipUser GetMembershipUserFromSearchResult(SearchResult res)
		{
			string text = (string)PropertyManager.GetSearchResultPropertyValue(res, this.attributeMapUsername);
			byte[] array = (byte[])PropertyManager.GetSearchResultPropertyValue(res, "objectSid");
			object obj = new SecurityIdentifier(array, 0);
			string text2 = (res.Properties.Contains(this.attributeMapEmail) ? ((string)res.Properties[this.attributeMapEmail][0]) : null);
			string text3 = null;
			if (this.attributeMapPasswordQuestion != null && res.Properties.Contains(this.attributeMapPasswordQuestion))
			{
				text3 = (string)PropertyManager.GetSearchResultPropertyValue(res, this.attributeMapPasswordQuestion);
			}
			string text4 = (res.Properties.Contains("comment") ? ((string)res.Properties["comment"][0]) : null);
			bool flag = false;
			bool flag2;
			if (this.directoryInfo.DirectoryType == DirectoryType.AD)
			{
				int num = (int)PropertyManager.GetSearchResultPropertyValue(res, "userAccountControl");
				flag2 = (num & 2) == 0;
				if (res.Properties.Contains("msDS-User-Account-Control-Computed"))
				{
					int num2 = (int)PropertyManager.GetSearchResultPropertyValue(res, "msDS-User-Account-Control-Computed");
					if ((num2 & 16) != 0)
					{
						flag = true;
					}
				}
				else if (res.Properties.Contains("lockoutTime"))
				{
					DateTime dateTime = DateTime.FromFileTimeUtc((long)PropertyManager.GetSearchResultPropertyValue(res, "lockoutTime"));
					TimeSpan timeSpan = DateTime.UtcNow.Subtract(dateTime);
					flag = timeSpan <= this.directoryInfo.ADLockoutDuration;
				}
			}
			else
			{
				flag2 = true;
				if (res.Properties.Contains("msDS-UserAccountDisabled"))
				{
					flag2 = !(bool)PropertyManager.GetSearchResultPropertyValue(res, "msDS-UserAccountDisabled");
				}
				int num3 = (int)PropertyManager.GetSearchResultPropertyValue(res, "msDS-User-Account-Control-Computed");
				if ((num3 & 16) != 0)
				{
					flag = true;
				}
			}
			DateTime dateTime2 = this.DefaultLastLockoutDate;
			if (flag)
			{
				dateTime2 = DateTime.FromFileTime((long)PropertyManager.GetSearchResultPropertyValue(res, "lockoutTime"));
			}
			if (this.EnablePasswordReset && res.Properties.Contains(this.attributeMapFailedPasswordAnswerLockoutTime))
			{
				DateTime dateTime3 = DateTime.FromFileTimeUtc((long)PropertyManager.GetSearchResultPropertyValue(res, this.attributeMapFailedPasswordAnswerLockoutTime));
				TimeSpan timeSpan2 = DateTime.UtcNow.Subtract(dateTime3);
				bool flag3 = timeSpan2 <= new TimeSpan(0, this.PasswordAnswerAttemptLockoutDuration, 0);
				if (flag3)
				{
					if (flag)
					{
						if (DateTime.Compare(dateTime3, DateTime.FromFileTimeUtc((long)PropertyManager.GetSearchResultPropertyValue(res, "lockoutTime"))) > 0)
						{
							dateTime2 = DateTime.FromFileTime((long)PropertyManager.GetSearchResultPropertyValue(res, this.attributeMapFailedPasswordAnswerLockoutTime));
						}
					}
					else
					{
						flag = true;
						dateTime2 = DateTime.FromFileTime((long)PropertyManager.GetSearchResultPropertyValue(res, this.attributeMapFailedPasswordAnswerLockoutTime));
					}
				}
			}
			DateTime dateTime4 = ((DateTime)PropertyManager.GetSearchResultPropertyValue(res, "whenCreated")).ToLocalTime();
			DateTime minValue = DateTime.MinValue;
			DateTime minValue2 = DateTime.MinValue;
			DateTime dateTime5 = DateTime.FromFileTime((long)PropertyManager.GetSearchResultPropertyValue(res, "pwdLastSet"));
			return new ActiveDirectoryMembershipUser(this.Name, text, array, obj, text2, text3, text4, flag2, flag, dateTime4, minValue, minValue2, dateTime5, dateTime2, true);
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x000AA550 File Offset: 0x000A9550
		private string GetEscapedRdn(string rdn)
		{
			NativeComInterfaces.IAdsPathname adsPathname = (NativeComInterfaces.IAdsPathname)new NativeComInterfaces.Pathname();
			return adsPathname.GetEscapedElement(0, rdn);
		}

		// Token: 0x06002719 RID: 10009 RVA: 0x000AA570 File Offset: 0x000A9570
		internal string GetEscapedFilterValue(string filterValue)
		{
			return this.GetEscapedFilterValue(filterValue, true);
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x000AA590 File Offset: 0x000A9590
		internal string GetEscapedFilterValue(string filterValue, bool escapeWildChar)
		{
			char[] array = new char[] { '(', ')', '*', '\\' };
			char[] array2 = new char[] { '(', ')', '\\' };
			int num = (escapeWildChar ? filterValue.IndexOfAny(array) : filterValue.IndexOfAny(array2));
			if (num != -1)
			{
				StringBuilder stringBuilder = new StringBuilder(2 * filterValue.Length);
				stringBuilder.Append(filterValue.Substring(0, num));
				for (int i = num; i < filterValue.Length; i++)
				{
					char c = filterValue[i];
					switch (c)
					{
					case '(':
						stringBuilder.Append("\\28");
						break;
					case ')':
						stringBuilder.Append("\\29");
						break;
					case '*':
						if (escapeWildChar)
						{
							stringBuilder.Append("\\2A");
						}
						else
						{
							stringBuilder.Append("*");
						}
						break;
					default:
						if (c != '\\')
						{
							stringBuilder.Append(filterValue[i]);
						}
						else if (escapeWildChar || filterValue.Length - i < 3 || filterValue[i + 1] != '2' || (filterValue[i + 2] != 'A' && filterValue[i + 2] != 'a'))
						{
							stringBuilder.Append("\\5C");
						}
						else
						{
							stringBuilder.Append("\\");
						}
						break;
					}
				}
				return stringBuilder.ToString();
			}
			return filterValue;
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x000AA730 File Offset: 0x000A9730
		private string GenerateAccountName()
		{
			char[] array = new char[]
			{
				'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
				'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
				'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
				'U', 'V'
			};
			char[] array2 = new char[20];
			byte[] array3 = new byte[12];
			RNGCryptoServiceProvider rngcryptoServiceProvider = new RNGCryptoServiceProvider();
			rngcryptoServiceProvider.GetBytes(array3);
			uint num = 0U;
			uint num2 = 0U;
			uint num3 = 0U;
			for (int i = 0; i < 4; i++)
			{
				num |= (uint)((uint)array3[i] << 8 * i);
			}
			for (int j = 0; j < 4; j++)
			{
				num2 |= (uint)((uint)array3[4 + j] << 8 * j);
			}
			for (int k = 0; k < 4; k++)
			{
				num3 |= (uint)((uint)array3[8 + k] << 8 * k);
			}
			array2[0] = '$';
			for (int l = 1; l <= 6; l++)
			{
				array2[l] = array[(int)((UIntPtr)(num & 31U))];
				num >>= 5;
			}
			array2[7] = '-';
			for (int m = 8; m <= 13; m++)
			{
				array2[m] = array[(int)((UIntPtr)(num2 & 31U))];
				num2 >>= 5;
			}
			for (int n = 13; n <= 19; n++)
			{
				array2[n] = array[(int)((UIntPtr)(num3 & 31U))];
				num3 >>= 5;
			}
			return new string(array2);
		}

		// Token: 0x0600271C RID: 10012 RVA: 0x000AA858 File Offset: 0x000A9858
		private void SetPasswordPortIfApplicable(DirectoryEntry userEntry)
		{
			if (this.directoryInfo.DirectoryType == DirectoryType.ADAM)
			{
				try
				{
					if (this.directoryInfo.ConnectionProtection == ActiveDirectoryConnectionProtection.Ssl && this.directoryInfo.PortSpecified)
					{
						userEntry.Options.PasswordPort = this.directoryInfo.Port;
						userEntry.Options.PasswordEncoding = PasswordEncodingMethod.PasswordEncodingSsl;
					}
					else if (this.directoryInfo.ConnectionProtection == ActiveDirectoryConnectionProtection.SignAndSeal || this.directoryInfo.ConnectionProtection == ActiveDirectoryConnectionProtection.None)
					{
						userEntry.Options.PasswordPort = this.directoryInfo.Port;
						userEntry.Options.PasswordEncoding = PasswordEncodingMethod.PasswordEncodingClear;
					}
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147463160)
					{
						throw;
					}
					if (this.directoryInfo.Port != 636 || this.directoryInfo.ConnectionProtection != ActiveDirectoryConnectionProtection.Ssl)
					{
						throw new ProviderException(SR.GetString("ADMembership_unable_to_set_password_port"));
					}
				}
			}
		}

		// Token: 0x0600271D RID: 10013 RVA: 0x000AA948 File Offset: 0x000A9948
		private bool IsUpnUnique(string username)
		{
			DirectoryEntry directoryEntry = new DirectoryEntry("GC://" + this.directoryInfo.ForestName, this.directoryInfo.GetUsername(), this.directoryInfo.GetPassword(), this.directoryInfo.AuthenticationTypes);
			DirectorySearcher directorySearcher = new DirectorySearcher(directoryEntry);
			directorySearcher.Filter = "(&(objectCategory=person)(objectClass=user)(userPrincipalName=" + this.GetEscapedFilterValue(username) + "))";
			directorySearcher.SearchScope = global::System.DirectoryServices.SearchScope.Subtree;
			if (this.directoryInfo.ClientSearchTimeout != -1)
			{
				directorySearcher.ClientTimeout = new TimeSpan(0, this.directoryInfo.ClientSearchTimeout, 0);
			}
			if (this.directoryInfo.ServerSearchTimeout != -1)
			{
				directorySearcher.ServerPageTimeLimit = new TimeSpan(0, this.directoryInfo.ServerSearchTimeout, 0);
			}
			bool flag;
			try
			{
				flag = directorySearcher.FindOne() == null;
			}
			finally
			{
				directoryEntry.Dispose();
			}
			return flag;
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x000AAA2C File Offset: 0x000A9A2C
		private bool IsEmailUnique(DirectoryEntry containerEntry, string username, string email, bool existing)
		{
			bool flag = false;
			if (containerEntry == null)
			{
				containerEntry = new DirectoryEntry(this.directoryInfo.GetADsPath(this.directoryInfo.ContainerDN), this.directoryInfo.GetUsername(), this.directoryInfo.GetPassword(), this.directoryInfo.AuthenticationTypes);
				flag = true;
			}
			DirectorySearcher directorySearcher = new DirectorySearcher(containerEntry);
			if (existing)
			{
				directorySearcher.Filter = string.Concat(new string[]
				{
					"(&(objectCategory=person)(objectClass=user)(",
					this.attributeMapUsername,
					"=*)(",
					this.attributeMapEmail,
					"=",
					this.GetEscapedFilterValue(email),
					")(!(",
					this.GetEscapedRdn("cn=" + this.GetEscapedFilterValue(username)),
					")))"
				});
			}
			else
			{
				directorySearcher.Filter = string.Concat(new string[]
				{
					"(&(objectCategory=person)(objectClass=user)(",
					this.attributeMapUsername,
					"=*)(",
					this.attributeMapEmail,
					"=",
					this.GetEscapedFilterValue(email),
					"))"
				});
			}
			directorySearcher.SearchScope = global::System.DirectoryServices.SearchScope.Subtree;
			if (this.directoryInfo.ClientSearchTimeout != -1)
			{
				directorySearcher.ClientTimeout = new TimeSpan(0, this.directoryInfo.ClientSearchTimeout, 0);
			}
			if (this.directoryInfo.ServerSearchTimeout != -1)
			{
				directorySearcher.ServerPageTimeLimit = new TimeSpan(0, this.directoryInfo.ServerSearchTimeout, 0);
			}
			bool flag2;
			try
			{
				flag2 = directorySearcher.FindOne() == null;
			}
			finally
			{
				if (flag)
				{
					containerEntry.Dispose();
					containerEntry = null;
				}
			}
			return flag2;
		}

		// Token: 0x0600271F RID: 10015 RVA: 0x000AABD4 File Offset: 0x000A9BD4
		private string GetConnectionString(string connectionStringName, bool appLevel)
		{
			if (string.IsNullOrEmpty(connectionStringName))
			{
				return null;
			}
			RuntimeConfig runtimeConfig = (appLevel ? RuntimeConfig.GetAppConfig() : RuntimeConfig.GetConfig());
			ConnectionStringSettings connectionStringSettings = runtimeConfig.ConnectionStrings.ConnectionStrings[connectionStringName];
			if (connectionStringSettings == null)
			{
				throw new ProviderException(SR.GetString("Connection_string_not_found", new object[] { connectionStringName }));
			}
			return connectionStringSettings.ConnectionString;
		}

		// Token: 0x06002720 RID: 10016 RVA: 0x000AAC34 File Offset: 0x000A9C34
		private string GetAttributeMapping(NameValueCollection config, string valueName, out int maxLength)
		{
			string text = config[valueName];
			maxLength = -1;
			if (text == null)
			{
				return null;
			}
			text = text.Trim();
			if (text.Length == 0)
			{
				throw new ProviderException(SR.GetString("ADMembership_Schema_mappings_must_not_be_empty", new object[] { valueName }));
			}
			return this.GetValidatedSchemaMapping(valueName, text, out maxLength);
		}

		// Token: 0x06002721 RID: 10017 RVA: 0x000AAC88 File Offset: 0x000A9C88
		private string GetValidatedSchemaMapping(string valueName, string attributeName, out int maxLength)
		{
			if (string.Compare(valueName, "attributeMapUsername", StringComparison.Ordinal) == 0)
			{
				if (this.directoryInfo.DirectoryType == DirectoryType.AD)
				{
					if (!StringUtil.EqualsIgnoreCase(attributeName, "sAMAccountName") && !StringUtil.EqualsIgnoreCase(attributeName, "userPrincipalName"))
					{
						throw new ProviderException(SR.GetString("ADMembership_Username_mapping_invalid"));
					}
				}
				else if (!StringUtil.EqualsIgnoreCase(attributeName, "userPrincipalName"))
				{
					throw new ProviderException(SR.GetString("ADMembership_Username_mapping_invalid_ADAM"));
				}
			}
			else
			{
				if (this.attributesInUse.Contains(attributeName))
				{
					throw new ProviderException(SR.GetString("ADMembership_mapping_not_unique", new object[] { valueName, attributeName }));
				}
				if (!this.userObjectAttributes.Contains(attributeName))
				{
					throw new ProviderException(SR.GetString("ADMembership_MappedAttribute_does_not_exist_on_user", new object[] { attributeName, valueName }));
				}
			}
			try
			{
				DirectoryEntry directoryEntry = new DirectoryEntry(this.directoryInfo.GetADsPath("schema") + "/" + attributeName, this.directoryInfo.GetUsername(), this.directoryInfo.GetPassword(), this.directoryInfo.AuthenticationTypes);
				string text = (string)directoryEntry.InvokeGet("Syntax");
				if (!StringUtil.EqualsIgnoreCase(text, (string)this.syntaxes[valueName]))
				{
					throw new ProviderException(SR.GetString("ADMembership_Wrong_syntax", new object[]
					{
						valueName,
						(string)this.syntaxes[valueName]
					}));
				}
				maxLength = -1;
				if (StringUtil.EqualsIgnoreCase(text, "DirectoryString"))
				{
					try
					{
						maxLength = (int)directoryEntry.InvokeGet("MaxRange");
					}
					catch (TargetInvocationException ex)
					{
						if (!(ex.InnerException is COMException) || ((COMException)ex.InnerException).ErrorCode != -2147463155)
						{
							throw;
						}
					}
				}
				if (string.Compare(valueName, "attributeMapUsername", StringComparison.Ordinal) != 0)
				{
					bool flag = (bool)directoryEntry.InvokeGet("MultiValued");
					if (flag)
					{
						throw new ProviderException(SR.GetString("ADMembership_attribute_not_single_valued", new object[] { valueName }));
					}
				}
			}
			catch (COMException ex2)
			{
				if (ex2.ErrorCode == -2147463168)
				{
					throw new ProviderException(SR.GetString("ADMembership_MappedAttribute_does_not_exist", new object[] { attributeName, valueName }), ex2);
				}
				throw;
			}
			return attributeName;
		}

		// Token: 0x06002722 RID: 10018 RVA: 0x000AAF00 File Offset: 0x000A9F00
		private int GetRangeUpperForSchemaAttribute(string attributeName)
		{
			int num = -1;
			DirectoryEntry directoryEntry = new DirectoryEntry(this.directoryInfo.GetADsPath("schema") + "/" + attributeName, this.directoryInfo.GetUsername(), this.directoryInfo.GetPassword(), this.directoryInfo.AuthenticationTypes);
			try
			{
				num = (int)directoryEntry.InvokeGet("MaxRange");
			}
			catch (TargetInvocationException ex)
			{
				if (!(ex.InnerException is COMException) || ((COMException)ex.InnerException).ErrorCode != -2147463155)
				{
					throw;
				}
			}
			return num;
		}

		// Token: 0x06002723 RID: 10019 RVA: 0x000AAFA0 File Offset: 0x000A9FA0
		private Hashtable GetUserObjectAttributes()
		{
			DirectoryEntry directoryEntry = new DirectoryEntry(this.directoryInfo.GetADsPath("schema") + "/user", this.directoryInfo.GetUsername(), this.directoryInfo.GetPassword(), this.directoryInfo.AuthenticationTypes);
			object obj = null;
			bool flag = false;
			Hashtable hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
			try
			{
				obj = directoryEntry.InvokeGet("MandatoryProperties");
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode != -2147463155)
				{
					throw;
				}
				flag = true;
			}
			if (!flag)
			{
				if (obj is ICollection)
				{
					using (IEnumerator enumerator = ((ICollection)obj).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj2 = enumerator.Current;
							string text = (string)obj2;
							if (!hashtable.Contains(text))
							{
								hashtable.Add(text, null);
							}
						}
						goto IL_00E2;
					}
				}
				if (!hashtable.Contains(obj))
				{
					hashtable.Add(obj, null);
				}
			}
			IL_00E2:
			flag = false;
			try
			{
				obj = directoryEntry.InvokeGet("OptionalProperties");
			}
			catch (COMException ex2)
			{
				if (ex2.ErrorCode != -2147463155)
				{
					throw;
				}
				flag = true;
			}
			if (!flag)
			{
				if (obj is ICollection)
				{
					using (IEnumerator enumerator2 = ((ICollection)obj).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj3 = enumerator2.Current;
							string text2 = (string)obj3;
							if (!hashtable.Contains(text2))
							{
								hashtable.Add(text2, null);
							}
						}
						return hashtable;
					}
				}
				if (!hashtable.Contains(obj))
				{
					hashtable.Add(obj, null);
				}
			}
			return hashtable;
		}

		// Token: 0x06002724 RID: 10020 RVA: 0x000AB158 File Offset: 0x000AA158
		private DateTime GetDateTimeFromLargeInteger(NativeComInterfaces.IAdsLargeInteger largeIntValue)
		{
			long num = largeIntValue.HighPart * 4294967296L + (long)((ulong)((uint)largeIntValue.LowPart));
			return DateTime.FromFileTimeUtc(num);
		}

		// Token: 0x06002725 RID: 10021 RVA: 0x000AB188 File Offset: 0x000AA188
		private NativeComInterfaces.IAdsLargeInteger GetLargeIntegerFromDateTime(DateTime dateTimeValue)
		{
			long num = dateTimeValue.ToFileTimeUtc();
			NativeComInterfaces.IAdsLargeInteger adsLargeInteger = (NativeComInterfaces.IAdsLargeInteger)new NativeComInterfaces.LargeInteger();
			adsLargeInteger.HighPart = (long)((int)(num >> 32));
			adsLargeInteger.LowPart = (long)((int)(num & (long)((ulong)(-1))));
			return adsLargeInteger;
		}

		// Token: 0x06002726 RID: 10022 RVA: 0x000AB1C4 File Offset: 0x000AA1C4
		private string Encrypt(string clearTextString)
		{
			byte[] bytes = Encoding.Unicode.GetBytes(clearTextString);
			byte[] array = new byte[16];
			new RNGCryptoServiceProvider().GetBytes(array);
			byte[] array2 = new byte[array.Length + bytes.Length];
			Buffer.BlockCopy(array, 0, array2, 0, array.Length);
			Buffer.BlockCopy(bytes, 0, array2, array.Length, bytes.Length);
			return Convert.ToBase64String(this.EncryptPassword(array2));
		}

		// Token: 0x06002727 RID: 10023 RVA: 0x000AB224 File Offset: 0x000AA224
		private string Decrypt(string encryptedString)
		{
			byte[] array = Convert.FromBase64String(encryptedString);
			byte[] array2 = this.DecryptPassword(array);
			return Encoding.Unicode.GetString(array2, 16, array2.Length - 16);
		}

		// Token: 0x04001DE4 RID: 7652
		private const int UF_ACCOUNT_DISABLED = 2;

		// Token: 0x04001DE5 RID: 7653
		private const int UF_LOCKOUT = 16;

		// Token: 0x04001DE6 RID: 7654
		private const int AD_SALT_SIZE_IN_BYTES = 16;

		// Token: 0x04001DE7 RID: 7655
		private const int PASSWORD_SIZE = 14;

		// Token: 0x04001DE8 RID: 7656
		private bool initialized;

		// Token: 0x04001DE9 RID: 7657
		private string adConnectionString;

		// Token: 0x04001DEA RID: 7658
		private bool enablePasswordRetrieval;

		// Token: 0x04001DEB RID: 7659
		private bool enablePasswordReset;

		// Token: 0x04001DEC RID: 7660
		private bool enableSearchMethods;

		// Token: 0x04001DED RID: 7661
		private bool requiresQuestionAndAnswer;

		// Token: 0x04001DEE RID: 7662
		private string appName;

		// Token: 0x04001DEF RID: 7663
		private bool requiresUniqueEmail;

		// Token: 0x04001DF0 RID: 7664
		private int maxInvalidPasswordAttempts;

		// Token: 0x04001DF1 RID: 7665
		private int passwordAttemptWindow;

		// Token: 0x04001DF2 RID: 7666
		private int passwordAnswerAttemptLockoutDuration;

		// Token: 0x04001DF3 RID: 7667
		private int minRequiredPasswordLength;

		// Token: 0x04001DF4 RID: 7668
		private int minRequiredNonalphanumericCharacters;

		// Token: 0x04001DF5 RID: 7669
		private string passwordStrengthRegularExpression;

		// Token: 0x04001DF6 RID: 7670
		private DirectoryInformation directoryInfo;

		// Token: 0x04001DF7 RID: 7671
		private string attributeMapUsername = "userPrincipalName";

		// Token: 0x04001DF8 RID: 7672
		private string attributeMapEmail = "mail";

		// Token: 0x04001DF9 RID: 7673
		private string attributeMapPasswordQuestion;

		// Token: 0x04001DFA RID: 7674
		private string attributeMapPasswordAnswer;

		// Token: 0x04001DFB RID: 7675
		private string attributeMapFailedPasswordAnswerCount;

		// Token: 0x04001DFC RID: 7676
		private string attributeMapFailedPasswordAnswerTime;

		// Token: 0x04001DFD RID: 7677
		private string attributeMapFailedPasswordAnswerLockoutTime;

		// Token: 0x04001DFE RID: 7678
		private int maxUsernameLength = 256;

		// Token: 0x04001DFF RID: 7679
		private int maxUsernameLengthForCreation = 64;

		// Token: 0x04001E00 RID: 7680
		private int maxPasswordLength = 128;

		// Token: 0x04001E01 RID: 7681
		private int maxCommentLength = 1024;

		// Token: 0x04001E02 RID: 7682
		private int maxEmailLength = 256;

		// Token: 0x04001E03 RID: 7683
		private int maxPasswordQuestionLength = 256;

		// Token: 0x04001E04 RID: 7684
		private int maxPasswordAnswerLength = 128;

		// Token: 0x04001E05 RID: 7685
		private readonly DateTime DefaultLastLockoutDate = new DateTime(1754, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x04001E06 RID: 7686
		private Hashtable syntaxes = new Hashtable();

		// Token: 0x04001E07 RID: 7687
		private Hashtable attributesInUse = new Hashtable(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04001E08 RID: 7688
		private Hashtable userObjectAttributes;

		// Token: 0x04001E09 RID: 7689
		private AuthType authTypeForValidation;

		// Token: 0x04001E0A RID: 7690
		private LdapConnection connection;

		// Token: 0x04001E0B RID: 7691
		private bool usernameIsSAMAccountName;

		// Token: 0x04001E0C RID: 7692
		private bool usernameIsUPN = true;
	}
}
