using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Web.DataAccess;
using System.Web.Management;
using System.Web.Util;

namespace System.Web.Security
{
	// Token: 0x02000355 RID: 853
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class SqlMembershipProvider : MembershipProvider
	{
		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06002971 RID: 10609 RVA: 0x000B5FD9 File Offset: 0x000B4FD9
		public override bool EnablePasswordRetrieval
		{
			get
			{
				return this._EnablePasswordRetrieval;
			}
		}

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06002972 RID: 10610 RVA: 0x000B5FE1 File Offset: 0x000B4FE1
		public override bool EnablePasswordReset
		{
			get
			{
				return this._EnablePasswordReset;
			}
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06002973 RID: 10611 RVA: 0x000B5FE9 File Offset: 0x000B4FE9
		public override bool RequiresQuestionAndAnswer
		{
			get
			{
				return this._RequiresQuestionAndAnswer;
			}
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06002974 RID: 10612 RVA: 0x000B5FF1 File Offset: 0x000B4FF1
		public override bool RequiresUniqueEmail
		{
			get
			{
				return this._RequiresUniqueEmail;
			}
		}

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x06002975 RID: 10613 RVA: 0x000B5FF9 File Offset: 0x000B4FF9
		public override MembershipPasswordFormat PasswordFormat
		{
			get
			{
				return this._PasswordFormat;
			}
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x06002976 RID: 10614 RVA: 0x000B6001 File Offset: 0x000B5001
		public override int MaxInvalidPasswordAttempts
		{
			get
			{
				return this._MaxInvalidPasswordAttempts;
			}
		}

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x06002977 RID: 10615 RVA: 0x000B6009 File Offset: 0x000B5009
		public override int PasswordAttemptWindow
		{
			get
			{
				return this._PasswordAttemptWindow;
			}
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x06002978 RID: 10616 RVA: 0x000B6011 File Offset: 0x000B5011
		public override int MinRequiredPasswordLength
		{
			get
			{
				return this._MinRequiredPasswordLength;
			}
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06002979 RID: 10617 RVA: 0x000B6019 File Offset: 0x000B5019
		public override int MinRequiredNonAlphanumericCharacters
		{
			get
			{
				return this._MinRequiredNonalphanumericCharacters;
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x0600297A RID: 10618 RVA: 0x000B6021 File Offset: 0x000B5021
		public override string PasswordStrengthRegularExpression
		{
			get
			{
				return this._PasswordStrengthRegularExpression;
			}
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x0600297B RID: 10619 RVA: 0x000B6029 File Offset: 0x000B5029
		// (set) Token: 0x0600297C RID: 10620 RVA: 0x000B6031 File Offset: 0x000B5031
		public override string ApplicationName
		{
			get
			{
				return this._AppName;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw ExceptionUtil.PropertyNullOrEmpty("ApplicationName");
				}
				if (value.Length > 256)
				{
					throw new ProviderException(SR.GetString("Provider_application_name_too_long"));
				}
				this._AppName = value;
			}
		}

		// Token: 0x0600297D RID: 10621 RVA: 0x000B606C File Offset: 0x000B506C
		public override void Initialize(string name, NameValueCollection config)
		{
			HttpRuntime.CheckAspNetHostingPermission(AspNetHostingPermissionLevel.Low, "Feature_not_supported_at_this_level");
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}
			if (string.IsNullOrEmpty(name))
			{
				name = "SqlMembershipProvider";
			}
			if (string.IsNullOrEmpty(config["description"]))
			{
				config.Remove("description");
				config.Add("description", SR.GetString("MembershipSqlProvider_description"));
			}
			base.Initialize(name, config);
			this._SchemaVersionCheck = 0;
			this._EnablePasswordRetrieval = SecUtility.GetBooleanValue(config, "enablePasswordRetrieval", false);
			this._EnablePasswordReset = SecUtility.GetBooleanValue(config, "enablePasswordReset", true);
			this._RequiresQuestionAndAnswer = SecUtility.GetBooleanValue(config, "requiresQuestionAndAnswer", true);
			this._RequiresUniqueEmail = SecUtility.GetBooleanValue(config, "requiresUniqueEmail", true);
			this._MaxInvalidPasswordAttempts = SecUtility.GetIntValue(config, "maxInvalidPasswordAttempts", 5, false, 0);
			this._PasswordAttemptWindow = SecUtility.GetIntValue(config, "passwordAttemptWindow", 10, false, 0);
			this._MinRequiredPasswordLength = SecUtility.GetIntValue(config, "minRequiredPasswordLength", 7, false, 128);
			this._MinRequiredNonalphanumericCharacters = SecUtility.GetIntValue(config, "minRequiredNonalphanumericCharacters", 1, true, 128);
			this._PasswordStrengthRegularExpression = config["passwordStrengthRegularExpression"];
			if (this._PasswordStrengthRegularExpression != null)
			{
				this._PasswordStrengthRegularExpression = this._PasswordStrengthRegularExpression.Trim();
				if (this._PasswordStrengthRegularExpression.Length == 0)
				{
					goto IL_016C;
				}
				try
				{
					new Regex(this._PasswordStrengthRegularExpression);
					goto IL_016C;
				}
				catch (ArgumentException ex)
				{
					throw new ProviderException(ex.Message, ex);
				}
			}
			this._PasswordStrengthRegularExpression = string.Empty;
			IL_016C:
			if (this._MinRequiredNonalphanumericCharacters > this._MinRequiredPasswordLength)
			{
				throw new HttpException(SR.GetString("MinRequiredNonalphanumericCharacters_can_not_be_more_than_MinRequiredPasswordLength"));
			}
			this._CommandTimeout = SecUtility.GetIntValue(config, "commandTimeout", 30, true, 0);
			this._AppName = config["applicationName"];
			if (string.IsNullOrEmpty(this._AppName))
			{
				this._AppName = SecUtility.GetDefaultAppName();
			}
			if (this._AppName.Length > 256)
			{
				throw new ProviderException(SR.GetString("Provider_application_name_too_long"));
			}
			string text = config["passwordFormat"];
			if (text == null)
			{
				text = "Hashed";
			}
			string text2;
			if ((text2 = text) != null)
			{
				if (!(text2 == "Clear"))
				{
					if (!(text2 == "Encrypted"))
					{
						if (!(text2 == "Hashed"))
						{
							goto IL_024C;
						}
						this._PasswordFormat = MembershipPasswordFormat.Hashed;
					}
					else
					{
						this._PasswordFormat = MembershipPasswordFormat.Encrypted;
					}
				}
				else
				{
					this._PasswordFormat = MembershipPasswordFormat.Clear;
				}
				if (this.PasswordFormat == MembershipPasswordFormat.Hashed && this.EnablePasswordRetrieval)
				{
					throw new ProviderException(SR.GetString("Provider_can_not_retrieve_hashed_password"));
				}
				string text3 = config["connectionStringName"];
				if (text3 == null || text3.Length < 1)
				{
					throw new ProviderException(SR.GetString("Connection_name_not_specified"));
				}
				this._sqlConnectionString = SqlConnectionHelper.GetConnectionString(text3, true, true);
				if (this._sqlConnectionString == null || this._sqlConnectionString.Length < 1)
				{
					throw new ProviderException(SR.GetString("Connection_string_not_found", new object[] { text3 }));
				}
				config.Remove("connectionStringName");
				config.Remove("enablePasswordRetrieval");
				config.Remove("enablePasswordReset");
				config.Remove("requiresQuestionAndAnswer");
				config.Remove("applicationName");
				config.Remove("requiresUniqueEmail");
				config.Remove("maxInvalidPasswordAttempts");
				config.Remove("passwordAttemptWindow");
				config.Remove("commandTimeout");
				config.Remove("passwordFormat");
				config.Remove("name");
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
				return;
			}
			IL_024C:
			throw new ProviderException(SR.GetString("Provider_bad_password_format"));
		}

		// Token: 0x0600297E RID: 10622 RVA: 0x000B6444 File Offset: 0x000B5444
		private void CheckSchemaVersion(SqlConnection connection)
		{
			string[] array = new string[] { "Common", "Membership" };
			string text = "1";
			SecUtility.CheckSchemaVersion(this, connection, array, text, ref this._SchemaVersionCheck);
		}

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x0600297F RID: 10623 RVA: 0x000B647F File Offset: 0x000B547F
		private int CommandTimeout
		{
			get
			{
				return this._CommandTimeout;
			}
		}

		// Token: 0x06002980 RID: 10624 RVA: 0x000B6488 File Offset: 0x000B5488
		public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
		{
			if (!SecUtility.ValidateParameter(ref password, true, true, false, 128))
			{
				status = MembershipCreateStatus.InvalidPassword;
				return null;
			}
			string text = base.GenerateSalt();
			string text2 = base.EncodePassword(password, (int)this._PasswordFormat, text);
			if (text2.Length > 128)
			{
				status = MembershipCreateStatus.InvalidPassword;
				return null;
			}
			if (passwordAnswer != null)
			{
				passwordAnswer = passwordAnswer.Trim();
			}
			string text3;
			if (!string.IsNullOrEmpty(passwordAnswer))
			{
				if (passwordAnswer.Length > 128)
				{
					status = MembershipCreateStatus.InvalidAnswer;
					return null;
				}
				text3 = base.EncodePassword(passwordAnswer.ToLower(CultureInfo.InvariantCulture), (int)this._PasswordFormat, text);
			}
			else
			{
				text3 = passwordAnswer;
			}
			if (!SecUtility.ValidateParameter(ref text3, this.RequiresQuestionAndAnswer, true, false, 128))
			{
				status = MembershipCreateStatus.InvalidAnswer;
				return null;
			}
			if (!SecUtility.ValidateParameter(ref username, true, true, true, 256))
			{
				status = MembershipCreateStatus.InvalidUserName;
				return null;
			}
			if (!SecUtility.ValidateParameter(ref email, this.RequiresUniqueEmail, this.RequiresUniqueEmail, false, 256))
			{
				status = MembershipCreateStatus.InvalidEmail;
				return null;
			}
			if (!SecUtility.ValidateParameter(ref passwordQuestion, this.RequiresQuestionAndAnswer, true, false, 256))
			{
				status = MembershipCreateStatus.InvalidQuestion;
				return null;
			}
			if (providerUserKey != null && !(providerUserKey is Guid))
			{
				status = MembershipCreateStatus.InvalidProviderUserKey;
				return null;
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
			MembershipUser membershipUser;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					DateTime dateTime = this.RoundToSeconds(DateTime.UtcNow);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_CreateUser", sqlConnectionHolder.Connection);
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
					sqlCommand.Parameters.Add(this.CreateInputParam("@Password", SqlDbType.NVarChar, text2));
					sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordSalt", SqlDbType.NVarChar, text));
					sqlCommand.Parameters.Add(this.CreateInputParam("@Email", SqlDbType.NVarChar, email));
					sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordQuestion", SqlDbType.NVarChar, passwordQuestion));
					sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordAnswer", SqlDbType.NVarChar, text3));
					sqlCommand.Parameters.Add(this.CreateInputParam("@IsApproved", SqlDbType.Bit, isApproved));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UniqueEmail", SqlDbType.Int, this.RequiresUniqueEmail ? 1 : 0));
					sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordFormat", SqlDbType.Int, (int)this.PasswordFormat));
					sqlCommand.Parameters.Add(this.CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, dateTime));
					SqlParameter sqlParameter = this.CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, providerUserKey);
					sqlParameter.Direction = ParameterDirection.InputOutput;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.ExecuteNonQuery();
					int num2 = ((sqlParameter.Value != null) ? ((int)sqlParameter.Value) : (-1));
					if (num2 < 0 || num2 > 11)
					{
						num2 = 11;
					}
					status = (MembershipCreateStatus)num2;
					if (num2 != 0)
					{
						membershipUser = null;
					}
					else
					{
						providerUserKey = new Guid(sqlCommand.Parameters["@UserId"].Value.ToString());
						dateTime = dateTime.ToLocalTime();
						membershipUser = new MembershipUser(this.Name, username, providerUserKey, email, passwordQuestion, null, isApproved, false, dateTime, dateTime, dateTime, dateTime, new DateTime(1754, 1, 1));
					}
				}
				finally
				{
					if (sqlConnectionHolder != null)
					{
						sqlConnectionHolder.Close();
						sqlConnectionHolder = null;
					}
				}
			}
			catch
			{
				throw;
			}
			return membershipUser;
		}

		// Token: 0x06002981 RID: 10625 RVA: 0x000B6904 File Offset: 0x000B5904
		public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
		{
			SecUtility.CheckParameter(ref username, true, true, true, 256, "username");
			SecUtility.CheckParameter(ref password, true, true, false, 128, "password");
			string text;
			int num;
			if (!this.CheckPassword(username, password, false, false, out text, out num))
			{
				return false;
			}
			SecUtility.CheckParameter(ref newPasswordQuestion, this.RequiresQuestionAndAnswer, this.RequiresQuestionAndAnswer, false, 256, "newPasswordQuestion");
			if (newPasswordAnswer != null)
			{
				newPasswordAnswer = newPasswordAnswer.Trim();
			}
			SecUtility.CheckParameter(ref newPasswordAnswer, this.RequiresQuestionAndAnswer, this.RequiresQuestionAndAnswer, false, 128, "newPasswordAnswer");
			string text2;
			if (!string.IsNullOrEmpty(newPasswordAnswer))
			{
				text2 = base.EncodePassword(newPasswordAnswer.ToLower(CultureInfo.InvariantCulture), num, text);
			}
			else
			{
				text2 = newPasswordAnswer;
			}
			SecUtility.CheckParameter(ref text2, this.RequiresQuestionAndAnswer, this.RequiresQuestionAndAnswer, false, 128, "newPasswordAnswer");
			bool flag;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_ChangePasswordQuestionAndAnswer", sqlConnectionHolder.Connection);
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
					sqlCommand.Parameters.Add(this.CreateInputParam("@NewPasswordQuestion", SqlDbType.NVarChar, newPasswordQuestion));
					sqlCommand.Parameters.Add(this.CreateInputParam("@NewPasswordAnswer", SqlDbType.NVarChar, text2));
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.ExecuteNonQuery();
					int num2 = ((sqlParameter.Value != null) ? ((int)sqlParameter.Value) : (-1));
					if (num2 != 0)
					{
						throw new ProviderException(this.GetExceptionText(num2));
					}
					flag = num2 == 0;
				}
				finally
				{
					if (sqlConnectionHolder != null)
					{
						sqlConnectionHolder.Close();
						sqlConnectionHolder = null;
					}
				}
			}
			catch
			{
				throw;
			}
			return flag;
		}

		// Token: 0x06002982 RID: 10626 RVA: 0x000B6B30 File Offset: 0x000B5B30
		public override string GetPassword(string username, string passwordAnswer)
		{
			if (!this.EnablePasswordRetrieval)
			{
				throw new NotSupportedException(SR.GetString("Membership_PasswordRetrieval_not_supported"));
			}
			SecUtility.CheckParameter(ref username, true, true, true, 256, "username");
			string encodedPasswordAnswer = this.GetEncodedPasswordAnswer(username, passwordAnswer);
			SecUtility.CheckParameter(ref encodedPasswordAnswer, this.RequiresQuestionAndAnswer, this.RequiresQuestionAndAnswer, false, 128, "passwordAnswer");
			int num = 0;
			int num2 = 0;
			string passwordFromDB = this.GetPasswordFromDB(username, encodedPasswordAnswer, this.RequiresQuestionAndAnswer, out num, out num2);
			if (passwordFromDB != null)
			{
				return base.UnEncodePassword(passwordFromDB, num);
			}
			string exceptionText = this.GetExceptionText(num2);
			if (this.IsStatusDueToBadPassword(num2))
			{
				throw new MembershipPasswordException(exceptionText);
			}
			throw new ProviderException(exceptionText);
		}

		// Token: 0x06002983 RID: 10627 RVA: 0x000B6BD4 File Offset: 0x000B5BD4
		public override bool ChangePassword(string username, string oldPassword, string newPassword)
		{
			SecUtility.CheckParameter(ref username, true, true, true, 256, "username");
			SecUtility.CheckParameter(ref oldPassword, true, true, false, 128, "oldPassword");
			SecUtility.CheckParameter(ref newPassword, true, true, false, 128, "newPassword");
			string text = null;
			int num;
			if (!this.CheckPassword(username, oldPassword, false, false, out text, out num))
			{
				return false;
			}
			if (newPassword.Length < this.MinRequiredPasswordLength)
			{
				throw new ArgumentException(SR.GetString("Password_too_short", new object[]
				{
					"newPassword",
					this.MinRequiredPasswordLength.ToString(CultureInfo.InvariantCulture)
				}));
			}
			int num2 = 0;
			for (int i = 0; i < newPassword.Length; i++)
			{
				if (!char.IsLetterOrDigit(newPassword, i))
				{
					num2++;
				}
			}
			if (num2 < this.MinRequiredNonAlphanumericCharacters)
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
			string text2 = base.EncodePassword(newPassword, num, text);
			if (text2.Length > 128)
			{
				throw new ArgumentException(SR.GetString("Membership_password_too_long"), "newPassword");
			}
			ValidatePasswordEventArgs validatePasswordEventArgs = new ValidatePasswordEventArgs(username, newPassword, false);
			this.OnValidatingPassword(validatePasswordEventArgs);
			if (!validatePasswordEventArgs.Cancel)
			{
				bool flag;
				try
				{
					SqlConnectionHolder sqlConnectionHolder = null;
					try
					{
						sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
						this.CheckSchemaVersion(sqlConnectionHolder.Connection);
						SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_SetPassword", sqlConnectionHolder.Connection);
						sqlCommand.CommandTimeout = this.CommandTimeout;
						sqlCommand.CommandType = CommandType.StoredProcedure;
						sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
						sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
						sqlCommand.Parameters.Add(this.CreateInputParam("@NewPassword", SqlDbType.NVarChar, text2));
						sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordSalt", SqlDbType.NVarChar, text));
						sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordFormat", SqlDbType.Int, num));
						sqlCommand.Parameters.Add(this.CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.UtcNow));
						SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
						sqlParameter.Direction = ParameterDirection.ReturnValue;
						sqlCommand.Parameters.Add(sqlParameter);
						sqlCommand.ExecuteNonQuery();
						int num3 = ((sqlParameter.Value != null) ? ((int)sqlParameter.Value) : (-1));
						if (num3 != 0)
						{
							string exceptionText = this.GetExceptionText(num3);
							if (this.IsStatusDueToBadPassword(num3))
							{
								throw new MembershipPasswordException(exceptionText);
							}
							throw new ProviderException(exceptionText);
						}
						else
						{
							flag = true;
						}
					}
					finally
					{
						if (sqlConnectionHolder != null)
						{
							sqlConnectionHolder.Close();
							sqlConnectionHolder = null;
						}
					}
				}
				catch
				{
					throw;
				}
				return flag;
			}
			if (validatePasswordEventArgs.FailureInformation != null)
			{
				throw validatePasswordEventArgs.FailureInformation;
			}
			throw new ArgumentException(SR.GetString("Membership_Custom_Password_Validation_Failure"), "newPassword");
		}

		// Token: 0x06002984 RID: 10628 RVA: 0x000B6F44 File Offset: 0x000B5F44
		public override string ResetPassword(string username, string passwordAnswer)
		{
			if (!this.EnablePasswordReset)
			{
				throw new NotSupportedException(SR.GetString("Not_configured_to_support_password_resets"));
			}
			SecUtility.CheckParameter(ref username, true, true, true, 256, "username");
			int num;
			string text;
			int num2;
			string text2;
			int num3;
			int num4;
			bool flag;
			DateTime dateTime;
			DateTime dateTime2;
			this.GetPasswordWithFormat(username, false, out num, out text, out num2, out text2, out num3, out num4, out flag, out dateTime, out dateTime2);
			if (num != 0)
			{
				if (this.IsStatusDueToBadPassword(num))
				{
					throw new MembershipPasswordException(this.GetExceptionText(num));
				}
				throw new ProviderException(this.GetExceptionText(num));
			}
			else
			{
				if (passwordAnswer != null)
				{
					passwordAnswer = passwordAnswer.Trim();
				}
				string text3;
				if (!string.IsNullOrEmpty(passwordAnswer))
				{
					text3 = base.EncodePassword(passwordAnswer.ToLower(CultureInfo.InvariantCulture), num2, text2);
				}
				else
				{
					text3 = passwordAnswer;
				}
				SecUtility.CheckParameter(ref text3, this.RequiresQuestionAndAnswer, this.RequiresQuestionAndAnswer, false, 128, "passwordAnswer");
				string text4 = this.GeneratePassword();
				ValidatePasswordEventArgs validatePasswordEventArgs = new ValidatePasswordEventArgs(username, text4, false);
				this.OnValidatingPassword(validatePasswordEventArgs);
				if (!validatePasswordEventArgs.Cancel)
				{
					string text5;
					try
					{
						SqlConnectionHolder sqlConnectionHolder = null;
						try
						{
							sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
							this.CheckSchemaVersion(sqlConnectionHolder.Connection);
							SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_ResetPassword", sqlConnectionHolder.Connection);
							sqlCommand.CommandTimeout = this.CommandTimeout;
							sqlCommand.CommandType = CommandType.StoredProcedure;
							sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
							sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
							sqlCommand.Parameters.Add(this.CreateInputParam("@NewPassword", SqlDbType.NVarChar, base.EncodePassword(text4, num2, text2)));
							sqlCommand.Parameters.Add(this.CreateInputParam("@MaxInvalidPasswordAttempts", SqlDbType.Int, this.MaxInvalidPasswordAttempts));
							sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordAttemptWindow", SqlDbType.Int, this.PasswordAttemptWindow));
							sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordSalt", SqlDbType.NVarChar, text2));
							sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordFormat", SqlDbType.Int, num2));
							sqlCommand.Parameters.Add(this.CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.UtcNow));
							if (this.RequiresQuestionAndAnswer)
							{
								sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordAnswer", SqlDbType.NVarChar, text3));
							}
							SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
							sqlParameter.Direction = ParameterDirection.ReturnValue;
							sqlCommand.Parameters.Add(sqlParameter);
							sqlCommand.ExecuteNonQuery();
							num = ((sqlParameter.Value != null) ? ((int)sqlParameter.Value) : (-1));
							if (num != 0)
							{
								string exceptionText = this.GetExceptionText(num);
								if (this.IsStatusDueToBadPassword(num))
								{
									throw new MembershipPasswordException(exceptionText);
								}
								throw new ProviderException(exceptionText);
							}
							else
							{
								text5 = text4;
							}
						}
						finally
						{
							if (sqlConnectionHolder != null)
							{
								sqlConnectionHolder.Close();
								sqlConnectionHolder = null;
							}
						}
					}
					catch
					{
						throw;
					}
					return text5;
				}
				if (validatePasswordEventArgs.FailureInformation != null)
				{
					throw validatePasswordEventArgs.FailureInformation;
				}
				throw new ProviderException(SR.GetString("Membership_Custom_Password_Validation_Failure"));
			}
		}

		// Token: 0x06002985 RID: 10629 RVA: 0x000B7274 File Offset: 0x000B6274
		public override void UpdateUser(MembershipUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			string text = user.UserName;
			SecUtility.CheckParameter(ref text, true, true, true, 256, "UserName");
			text = user.Email;
			SecUtility.CheckParameter(ref text, this.RequiresUniqueEmail, this.RequiresUniqueEmail, false, 256, "Email");
			user.Email = text;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_UpdateUser", sqlConnectionHolder.Connection);
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, user.UserName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@Email", SqlDbType.NVarChar, user.Email));
					sqlCommand.Parameters.Add(this.CreateInputParam("@Comment", SqlDbType.NText, user.Comment));
					sqlCommand.Parameters.Add(this.CreateInputParam("@IsApproved", SqlDbType.Bit, user.IsApproved ? 1 : 0));
					sqlCommand.Parameters.Add(this.CreateInputParam("@LastLoginDate", SqlDbType.DateTime, user.LastLoginDate.ToUniversalTime()));
					sqlCommand.Parameters.Add(this.CreateInputParam("@LastActivityDate", SqlDbType.DateTime, user.LastActivityDate.ToUniversalTime()));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UniqueEmail", SqlDbType.Int, this.RequiresUniqueEmail ? 1 : 0));
					sqlCommand.Parameters.Add(this.CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.UtcNow));
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.ExecuteNonQuery();
					int num = ((sqlParameter.Value != null) ? ((int)sqlParameter.Value) : (-1));
					if (num != 0)
					{
						throw new ProviderException(this.GetExceptionText(num));
					}
				}
				finally
				{
					if (sqlConnectionHolder != null)
					{
						sqlConnectionHolder.Close();
						sqlConnectionHolder = null;
					}
				}
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x06002986 RID: 10630 RVA: 0x000B74F4 File Offset: 0x000B64F4
		public override bool ValidateUser(string username, string password)
		{
			if (SecUtility.ValidateParameter(ref username, true, true, true, 256) && SecUtility.ValidateParameter(ref password, true, true, false, 128) && this.CheckPassword(username, password, true, true))
			{
				PerfCounters.IncrementCounter(AppPerfCounter.MEMBER_SUCCESS);
				WebBaseEvent.RaiseSystemEvent(null, 4002, username);
				return true;
			}
			PerfCounters.IncrementCounter(AppPerfCounter.MEMBER_FAIL);
			WebBaseEvent.RaiseSystemEvent(null, 4006, username);
			return false;
		}

		// Token: 0x06002987 RID: 10631 RVA: 0x000B7558 File Offset: 0x000B6558
		public override bool UnlockUser(string username)
		{
			SecUtility.CheckParameter(ref username, true, true, true, 256, "username");
			bool flag;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_UnlockUser", sqlConnectionHolder.Connection);
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.ExecuteNonQuery();
					if (((sqlParameter.Value != null) ? ((int)sqlParameter.Value) : (-1)) == 0)
					{
						flag = true;
					}
					else
					{
						flag = false;
					}
				}
				finally
				{
					if (sqlConnectionHolder != null)
					{
						sqlConnectionHolder.Close();
						sqlConnectionHolder = null;
					}
				}
			}
			catch
			{
				throw;
			}
			return flag;
		}

		// Token: 0x06002988 RID: 10632 RVA: 0x000B766C File Offset: 0x000B666C
		public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			if (providerUserKey == null)
			{
				throw new ArgumentNullException("providerUserKey");
			}
			if (!(providerUserKey is Guid))
			{
				throw new ArgumentException(SR.GetString("Membership_InvalidProviderUserKey"), "providerUserKey");
			}
			SqlDataReader sqlDataReader = null;
			MembershipUser membershipUser;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_GetUserByUserId", sqlConnectionHolder.Connection);
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserId", SqlDbType.UniqueIdentifier, providerUserKey));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UpdateLastActivity", SqlDbType.Bit, userIsOnline));
					sqlCommand.Parameters.Add(this.CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.UtcNow));
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlDataReader = sqlCommand.ExecuteReader();
					if (sqlDataReader.Read())
					{
						string nullableString = this.GetNullableString(sqlDataReader, 0);
						string nullableString2 = this.GetNullableString(sqlDataReader, 1);
						string nullableString3 = this.GetNullableString(sqlDataReader, 2);
						bool boolean = sqlDataReader.GetBoolean(3);
						DateTime dateTime = sqlDataReader.GetDateTime(4).ToLocalTime();
						DateTime dateTime2 = sqlDataReader.GetDateTime(5).ToLocalTime();
						DateTime dateTime3 = sqlDataReader.GetDateTime(6).ToLocalTime();
						DateTime dateTime4 = sqlDataReader.GetDateTime(7).ToLocalTime();
						string nullableString4 = this.GetNullableString(sqlDataReader, 8);
						bool boolean2 = sqlDataReader.GetBoolean(9);
						DateTime dateTime5 = sqlDataReader.GetDateTime(10).ToLocalTime();
						membershipUser = new MembershipUser(this.Name, nullableString4, providerUserKey, nullableString, nullableString2, nullableString3, boolean, boolean2, dateTime, dateTime2, dateTime3, dateTime4, dateTime5);
					}
					else
					{
						membershipUser = null;
					}
				}
				finally
				{
					if (sqlDataReader != null)
					{
						sqlDataReader.Close();
						sqlDataReader = null;
					}
					if (sqlConnectionHolder != null)
					{
						sqlConnectionHolder.Close();
						sqlConnectionHolder = null;
					}
				}
			}
			catch
			{
				throw;
			}
			return membershipUser;
		}

		// Token: 0x06002989 RID: 10633 RVA: 0x000B7884 File Offset: 0x000B6884
		public override MembershipUser GetUser(string username, bool userIsOnline)
		{
			SecUtility.CheckParameter(ref username, true, false, true, 256, "username");
			SqlDataReader sqlDataReader = null;
			MembershipUser membershipUser;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_GetUserByName", sqlConnectionHolder.Connection);
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UpdateLastActivity", SqlDbType.Bit, userIsOnline));
					sqlCommand.Parameters.Add(this.CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.UtcNow));
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlDataReader = sqlCommand.ExecuteReader();
					if (sqlDataReader.Read())
					{
						string nullableString = this.GetNullableString(sqlDataReader, 0);
						string nullableString2 = this.GetNullableString(sqlDataReader, 1);
						string nullableString3 = this.GetNullableString(sqlDataReader, 2);
						bool boolean = sqlDataReader.GetBoolean(3);
						DateTime dateTime = sqlDataReader.GetDateTime(4).ToLocalTime();
						DateTime dateTime2 = sqlDataReader.GetDateTime(5).ToLocalTime();
						DateTime dateTime3 = sqlDataReader.GetDateTime(6).ToLocalTime();
						DateTime dateTime4 = sqlDataReader.GetDateTime(7).ToLocalTime();
						Guid guid = sqlDataReader.GetGuid(8);
						bool boolean2 = sqlDataReader.GetBoolean(9);
						DateTime dateTime5 = sqlDataReader.GetDateTime(10).ToLocalTime();
						membershipUser = new MembershipUser(this.Name, username, guid, nullableString, nullableString2, nullableString3, boolean, boolean2, dateTime, dateTime2, dateTime3, dateTime4, dateTime5);
					}
					else
					{
						membershipUser = null;
					}
				}
				finally
				{
					if (sqlDataReader != null)
					{
						sqlDataReader.Close();
						sqlDataReader = null;
					}
					if (sqlConnectionHolder != null)
					{
						sqlConnectionHolder.Close();
						sqlConnectionHolder = null;
					}
				}
			}
			catch
			{
				throw;
			}
			return membershipUser;
		}

		// Token: 0x0600298A RID: 10634 RVA: 0x000B7AA8 File Offset: 0x000B6AA8
		public override string GetUserNameByEmail(string email)
		{
			SecUtility.CheckParameter(ref email, false, false, false, 256, "email");
			string text2;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_GetUserByEmail", sqlConnectionHolder.Connection);
					string text = null;
					SqlDataReader sqlDataReader = null;
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@Email", SqlDbType.NVarChar, email));
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					try
					{
						sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SequentialAccess);
						if (sqlDataReader.Read())
						{
							text = this.GetNullableString(sqlDataReader, 0);
							if (this.RequiresUniqueEmail && sqlDataReader.Read())
							{
								throw new ProviderException(SR.GetString("Membership_more_than_one_user_with_email"));
							}
						}
					}
					finally
					{
						if (sqlDataReader != null)
						{
							sqlDataReader.Close();
						}
					}
					text2 = text;
				}
				finally
				{
					if (sqlConnectionHolder != null)
					{
						sqlConnectionHolder.Close();
						sqlConnectionHolder = null;
					}
				}
			}
			catch
			{
				throw;
			}
			return text2;
		}

		// Token: 0x0600298B RID: 10635 RVA: 0x000B7BEC File Offset: 0x000B6BEC
		public override bool DeleteUser(string username, bool deleteAllRelatedData)
		{
			SecUtility.CheckParameter(ref username, true, true, true, 256, "username");
			bool flag;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Users_DeleteUser", sqlConnectionHolder.Connection);
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
					if (deleteAllRelatedData)
					{
						sqlCommand.Parameters.Add(this.CreateInputParam("@TablesToDeleteFrom", SqlDbType.Int, 15));
					}
					else
					{
						sqlCommand.Parameters.Add(this.CreateInputParam("@TablesToDeleteFrom", SqlDbType.Int, 1));
					}
					SqlParameter sqlParameter = new SqlParameter("@NumTablesDeletedFrom", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.Output;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.ExecuteNonQuery();
					int num = ((sqlParameter.Value != null) ? ((int)sqlParameter.Value) : (-1));
					flag = num > 0;
				}
				finally
				{
					if (sqlConnectionHolder != null)
					{
						sqlConnectionHolder.Close();
						sqlConnectionHolder = null;
					}
				}
			}
			catch
			{
				throw;
			}
			return flag;
		}

		// Token: 0x0600298C RID: 10636 RVA: 0x000B7D54 File Offset: 0x000B6D54
		public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
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
			MembershipUserCollection membershipUserCollection = new MembershipUserCollection();
			totalRecords = 0;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_GetAllUsers", sqlConnectionHolder.Connection);
					SqlDataReader sqlDataReader = null;
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@PageIndex", SqlDbType.Int, pageIndex));
					sqlCommand.Parameters.Add(this.CreateInputParam("@PageSize", SqlDbType.Int, pageSize));
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					try
					{
						sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SequentialAccess);
						while (sqlDataReader.Read())
						{
							string nullableString = this.GetNullableString(sqlDataReader, 0);
							string nullableString2 = this.GetNullableString(sqlDataReader, 1);
							string nullableString3 = this.GetNullableString(sqlDataReader, 2);
							string nullableString4 = this.GetNullableString(sqlDataReader, 3);
							bool boolean = sqlDataReader.GetBoolean(4);
							DateTime dateTime = sqlDataReader.GetDateTime(5).ToLocalTime();
							DateTime dateTime2 = sqlDataReader.GetDateTime(6).ToLocalTime();
							DateTime dateTime3 = sqlDataReader.GetDateTime(7).ToLocalTime();
							DateTime dateTime4 = sqlDataReader.GetDateTime(8).ToLocalTime();
							Guid guid = sqlDataReader.GetGuid(9);
							bool boolean2 = sqlDataReader.GetBoolean(10);
							DateTime dateTime5 = sqlDataReader.GetDateTime(11).ToLocalTime();
							membershipUserCollection.Add(new MembershipUser(this.Name, nullableString, guid, nullableString2, nullableString3, nullableString4, boolean, boolean2, dateTime, dateTime2, dateTime3, dateTime4, dateTime5));
						}
					}
					finally
					{
						if (sqlDataReader != null)
						{
							sqlDataReader.Close();
						}
						if (sqlParameter.Value != null && sqlParameter.Value is int)
						{
							totalRecords = (int)sqlParameter.Value;
						}
					}
				}
				finally
				{
					if (sqlConnectionHolder != null)
					{
						sqlConnectionHolder.Close();
						sqlConnectionHolder = null;
					}
				}
			}
			catch
			{
				throw;
			}
			return membershipUserCollection;
		}

		// Token: 0x0600298D RID: 10637 RVA: 0x000B8010 File Offset: 0x000B7010
		public override int GetNumberOfUsersOnline()
		{
			int num2;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_GetNumberOfUsersOnline", sqlConnectionHolder.Connection);
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@MinutesSinceLastInActive", SqlDbType.Int, Membership.UserIsOnlineTimeWindow));
					sqlCommand.Parameters.Add(this.CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.UtcNow));
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.ExecuteNonQuery();
					int num = ((sqlParameter.Value != null) ? ((int)sqlParameter.Value) : (-1));
					num2 = num;
				}
				finally
				{
					if (sqlConnectionHolder != null)
					{
						sqlConnectionHolder.Close();
						sqlConnectionHolder = null;
					}
				}
			}
			catch
			{
				throw;
			}
			return num2;
		}

		// Token: 0x0600298E RID: 10638 RVA: 0x000B8130 File Offset: 0x000B7130
		public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			SecUtility.CheckParameter(ref usernameToMatch, true, true, false, 256, "usernameToMatch");
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
			MembershipUserCollection membershipUserCollection2;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				totalRecords = 0;
				SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
				sqlParameter.Direction = ParameterDirection.ReturnValue;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_FindUsersByName", sqlConnectionHolder.Connection);
					MembershipUserCollection membershipUserCollection = new MembershipUserCollection();
					SqlDataReader sqlDataReader = null;
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserNameToMatch", SqlDbType.NVarChar, usernameToMatch));
					sqlCommand.Parameters.Add(this.CreateInputParam("@PageIndex", SqlDbType.Int, pageIndex));
					sqlCommand.Parameters.Add(this.CreateInputParam("@PageSize", SqlDbType.Int, pageSize));
					sqlCommand.Parameters.Add(sqlParameter);
					try
					{
						sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SequentialAccess);
						while (sqlDataReader.Read())
						{
							string nullableString = this.GetNullableString(sqlDataReader, 0);
							string nullableString2 = this.GetNullableString(sqlDataReader, 1);
							string nullableString3 = this.GetNullableString(sqlDataReader, 2);
							string nullableString4 = this.GetNullableString(sqlDataReader, 3);
							bool boolean = sqlDataReader.GetBoolean(4);
							DateTime dateTime = sqlDataReader.GetDateTime(5).ToLocalTime();
							DateTime dateTime2 = sqlDataReader.GetDateTime(6).ToLocalTime();
							DateTime dateTime3 = sqlDataReader.GetDateTime(7).ToLocalTime();
							DateTime dateTime4 = sqlDataReader.GetDateTime(8).ToLocalTime();
							Guid guid = sqlDataReader.GetGuid(9);
							bool boolean2 = sqlDataReader.GetBoolean(10);
							DateTime dateTime5 = sqlDataReader.GetDateTime(11).ToLocalTime();
							membershipUserCollection.Add(new MembershipUser(this.Name, nullableString, guid, nullableString2, nullableString3, nullableString4, boolean, boolean2, dateTime, dateTime2, dateTime3, dateTime4, dateTime5));
						}
						membershipUserCollection2 = membershipUserCollection;
					}
					finally
					{
						if (sqlDataReader != null)
						{
							sqlDataReader.Close();
						}
						if (sqlParameter.Value != null && sqlParameter.Value is int)
						{
							totalRecords = (int)sqlParameter.Value;
						}
					}
				}
				finally
				{
					if (sqlConnectionHolder != null)
					{
						sqlConnectionHolder.Close();
						sqlConnectionHolder = null;
					}
				}
			}
			catch
			{
				throw;
			}
			return membershipUserCollection2;
		}

		// Token: 0x0600298F RID: 10639 RVA: 0x000B8418 File Offset: 0x000B7418
		public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			SecUtility.CheckParameter(ref emailToMatch, false, false, false, 256, "emailToMatch");
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
			MembershipUserCollection membershipUserCollection2;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				totalRecords = 0;
				SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
				sqlParameter.Direction = ParameterDirection.ReturnValue;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_FindUsersByEmail", sqlConnectionHolder.Connection);
					MembershipUserCollection membershipUserCollection = new MembershipUserCollection();
					SqlDataReader sqlDataReader = null;
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@EmailToMatch", SqlDbType.NVarChar, emailToMatch));
					sqlCommand.Parameters.Add(this.CreateInputParam("@PageIndex", SqlDbType.Int, pageIndex));
					sqlCommand.Parameters.Add(this.CreateInputParam("@PageSize", SqlDbType.Int, pageSize));
					sqlCommand.Parameters.Add(sqlParameter);
					try
					{
						sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SequentialAccess);
						while (sqlDataReader.Read())
						{
							string nullableString = this.GetNullableString(sqlDataReader, 0);
							string nullableString2 = this.GetNullableString(sqlDataReader, 1);
							string nullableString3 = this.GetNullableString(sqlDataReader, 2);
							string nullableString4 = this.GetNullableString(sqlDataReader, 3);
							bool boolean = sqlDataReader.GetBoolean(4);
							DateTime dateTime = sqlDataReader.GetDateTime(5).ToLocalTime();
							DateTime dateTime2 = sqlDataReader.GetDateTime(6).ToLocalTime();
							DateTime dateTime3 = sqlDataReader.GetDateTime(7).ToLocalTime();
							DateTime dateTime4 = sqlDataReader.GetDateTime(8).ToLocalTime();
							Guid guid = sqlDataReader.GetGuid(9);
							bool boolean2 = sqlDataReader.GetBoolean(10);
							DateTime dateTime5 = sqlDataReader.GetDateTime(11).ToLocalTime();
							membershipUserCollection.Add(new MembershipUser(this.Name, nullableString, guid, nullableString2, nullableString3, nullableString4, boolean, boolean2, dateTime, dateTime2, dateTime3, dateTime4, dateTime5));
						}
						membershipUserCollection2 = membershipUserCollection;
					}
					finally
					{
						if (sqlDataReader != null)
						{
							sqlDataReader.Close();
						}
						if (sqlParameter.Value != null && sqlParameter.Value is int)
						{
							totalRecords = (int)sqlParameter.Value;
						}
					}
				}
				finally
				{
					if (sqlConnectionHolder != null)
					{
						sqlConnectionHolder.Close();
						sqlConnectionHolder = null;
					}
				}
			}
			catch
			{
				throw;
			}
			return membershipUserCollection2;
		}

		// Token: 0x06002990 RID: 10640 RVA: 0x000B8700 File Offset: 0x000B7700
		private bool CheckPassword(string username, string password, bool updateLastLoginActivityDate, bool failIfNotApproved)
		{
			string text;
			int num;
			return this.CheckPassword(username, password, updateLastLoginActivityDate, failIfNotApproved, out text, out num);
		}

		// Token: 0x06002991 RID: 10641 RVA: 0x000B871C File Offset: 0x000B771C
		private bool CheckPassword(string username, string password, bool updateLastLoginActivityDate, bool failIfNotApproved, out string salt, out int passwordFormat)
		{
			SqlConnectionHolder sqlConnectionHolder = null;
			int num;
			string text;
			int num2;
			int num3;
			bool flag;
			DateTime dateTime;
			DateTime dateTime2;
			this.GetPasswordWithFormat(username, updateLastLoginActivityDate, out num, out text, out passwordFormat, out salt, out num2, out num3, out flag, out dateTime, out dateTime2);
			if (num != 0)
			{
				return false;
			}
			if (!flag && failIfNotApproved)
			{
				return false;
			}
			string text2 = base.EncodePassword(password, passwordFormat, salt);
			bool flag2 = text.Equals(text2);
			if (flag2 && num2 == 0 && num3 == 0)
			{
				return true;
			}
			try
			{
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_UpdateUserInfo", sqlConnectionHolder.Connection);
					DateTime utcNow = DateTime.UtcNow;
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
					sqlCommand.Parameters.Add(this.CreateInputParam("@IsPasswordCorrect", SqlDbType.Bit, flag2));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UpdateLastLoginActivityDate", SqlDbType.Bit, updateLastLoginActivityDate));
					sqlCommand.Parameters.Add(this.CreateInputParam("@MaxInvalidPasswordAttempts", SqlDbType.Int, this.MaxInvalidPasswordAttempts));
					sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordAttemptWindow", SqlDbType.Int, this.PasswordAttemptWindow));
					sqlCommand.Parameters.Add(this.CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, utcNow));
					sqlCommand.Parameters.Add(this.CreateInputParam("@LastLoginDate", SqlDbType.DateTime, flag2 ? utcNow : dateTime));
					sqlCommand.Parameters.Add(this.CreateInputParam("@LastActivityDate", SqlDbType.DateTime, flag2 ? utcNow : dateTime2));
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.ExecuteNonQuery();
					num = ((sqlParameter.Value != null) ? ((int)sqlParameter.Value) : (-1));
				}
				finally
				{
					if (sqlConnectionHolder != null)
					{
						sqlConnectionHolder.Close();
						sqlConnectionHolder = null;
					}
				}
			}
			catch
			{
				throw;
			}
			return flag2;
		}

		// Token: 0x06002992 RID: 10642 RVA: 0x000B8984 File Offset: 0x000B7984
		private void GetPasswordWithFormat(string username, bool updateLastLoginActivityDate, out int status, out string password, out int passwordFormat, out string passwordSalt, out int failedPasswordAttemptCount, out int failedPasswordAnswerAttemptCount, out bool isApproved, out DateTime lastLoginDate, out DateTime lastActivityDate)
		{
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				SqlDataReader sqlDataReader = null;
				SqlParameter sqlParameter = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_GetPasswordWithFormat", sqlConnectionHolder.Connection);
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UpdateLastLoginActivityDate", SqlDbType.Bit, updateLastLoginActivityDate));
					sqlCommand.Parameters.Add(this.CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.UtcNow));
					sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SingleRow);
					status = -1;
					if (sqlDataReader.Read())
					{
						password = sqlDataReader.GetString(0);
						passwordFormat = sqlDataReader.GetInt32(1);
						passwordSalt = sqlDataReader.GetString(2);
						failedPasswordAttemptCount = sqlDataReader.GetInt32(3);
						failedPasswordAnswerAttemptCount = sqlDataReader.GetInt32(4);
						isApproved = sqlDataReader.GetBoolean(5);
						lastLoginDate = sqlDataReader.GetDateTime(6);
						lastActivityDate = sqlDataReader.GetDateTime(7);
					}
					else
					{
						password = null;
						passwordFormat = 0;
						passwordSalt = null;
						failedPasswordAttemptCount = 0;
						failedPasswordAnswerAttemptCount = 0;
						isApproved = false;
						lastLoginDate = DateTime.UtcNow;
						lastActivityDate = DateTime.UtcNow;
					}
				}
				finally
				{
					if (sqlDataReader != null)
					{
						sqlDataReader.Close();
						sqlDataReader = null;
						status = ((sqlParameter.Value != null) ? ((int)sqlParameter.Value) : (-1));
					}
					if (sqlConnectionHolder != null)
					{
						sqlConnectionHolder.Close();
						sqlConnectionHolder = null;
					}
				}
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x06002993 RID: 10643 RVA: 0x000B8B74 File Offset: 0x000B7B74
		private string GetPasswordFromDB(string username, string passwordAnswer, bool requiresQuestionAndAnswer, out int passwordFormat, out int status)
		{
			string text2;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				SqlDataReader sqlDataReader = null;
				SqlParameter sqlParameter = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_GetPassword", sqlConnectionHolder.Connection);
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
					sqlCommand.Parameters.Add(this.CreateInputParam("@MaxInvalidPasswordAttempts", SqlDbType.Int, this.MaxInvalidPasswordAttempts));
					sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordAttemptWindow", SqlDbType.Int, this.PasswordAttemptWindow));
					sqlCommand.Parameters.Add(this.CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.UtcNow));
					if (requiresQuestionAndAnswer)
					{
						sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordAnswer", SqlDbType.NVarChar, passwordAnswer));
					}
					sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SingleRow);
					status = -1;
					string text;
					if (sqlDataReader.Read())
					{
						text = sqlDataReader.GetString(0);
						passwordFormat = sqlDataReader.GetInt32(1);
					}
					else
					{
						text = null;
						passwordFormat = 0;
					}
					text2 = text;
				}
				finally
				{
					if (sqlDataReader != null)
					{
						sqlDataReader.Close();
						sqlDataReader = null;
						status = ((sqlParameter.Value != null) ? ((int)sqlParameter.Value) : (-1));
					}
					if (sqlConnectionHolder != null)
					{
						sqlConnectionHolder.Close();
						sqlConnectionHolder = null;
					}
				}
			}
			catch
			{
				throw;
			}
			return text2;
		}

		// Token: 0x06002994 RID: 10644 RVA: 0x000B8D44 File Offset: 0x000B7D44
		private string GetEncodedPasswordAnswer(string username, string passwordAnswer)
		{
			if (passwordAnswer != null)
			{
				passwordAnswer = passwordAnswer.Trim();
			}
			if (string.IsNullOrEmpty(passwordAnswer))
			{
				return passwordAnswer;
			}
			int num;
			string text;
			int num2;
			string text2;
			int num3;
			int num4;
			bool flag;
			DateTime dateTime;
			DateTime dateTime2;
			this.GetPasswordWithFormat(username, false, out num, out text, out num2, out text2, out num3, out num4, out flag, out dateTime, out dateTime2);
			if (num == 0)
			{
				return base.EncodePassword(passwordAnswer.ToLower(CultureInfo.InvariantCulture), num2, text2);
			}
			throw new ProviderException(this.GetExceptionText(num));
		}

		// Token: 0x06002995 RID: 10645 RVA: 0x000B8DA4 File Offset: 0x000B7DA4
		public virtual string GeneratePassword()
		{
			return Membership.GeneratePassword((this.MinRequiredPasswordLength < 14) ? 14 : this.MinRequiredPasswordLength, this.MinRequiredNonAlphanumericCharacters);
		}

		// Token: 0x06002996 RID: 10646 RVA: 0x000B8DC8 File Offset: 0x000B7DC8
		private SqlParameter CreateInputParam(string paramName, SqlDbType dbType, object objValue)
		{
			SqlParameter sqlParameter = new SqlParameter(paramName, dbType);
			if (objValue == null)
			{
				sqlParameter.IsNullable = true;
				sqlParameter.Value = DBNull.Value;
			}
			else
			{
				sqlParameter.Value = objValue;
			}
			return sqlParameter;
		}

		// Token: 0x06002997 RID: 10647 RVA: 0x000B8DFC File Offset: 0x000B7DFC
		private string GetNullableString(SqlDataReader reader, int col)
		{
			if (!reader.IsDBNull(col))
			{
				return reader.GetString(col);
			}
			return null;
		}

		// Token: 0x06002998 RID: 10648 RVA: 0x000B8E10 File Offset: 0x000B7E10
		private string GetExceptionText(int status)
		{
			string text;
			switch (status)
			{
			case 0:
				return string.Empty;
			case 1:
				text = "Membership_UserNotFound";
				break;
			case 2:
				text = "Membership_WrongPassword";
				break;
			case 3:
				text = "Membership_WrongAnswer";
				break;
			case 4:
				text = "Membership_InvalidPassword";
				break;
			case 5:
				text = "Membership_InvalidQuestion";
				break;
			case 6:
				text = "Membership_InvalidAnswer";
				break;
			case 7:
				text = "Membership_InvalidEmail";
				break;
			default:
				if (status != 99)
				{
					text = "Provider_Error";
				}
				else
				{
					text = "Membership_AccountLockOut";
				}
				break;
			}
			return SR.GetString(text);
		}

		// Token: 0x06002999 RID: 10649 RVA: 0x000B8E9E File Offset: 0x000B7E9E
		private bool IsStatusDueToBadPassword(int status)
		{
			return (status >= 2 && status <= 6) || status == 99;
		}

		// Token: 0x0600299A RID: 10650 RVA: 0x000B8EAF File Offset: 0x000B7EAF
		private DateTime RoundToSeconds(DateTime dt)
		{
			return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
		}

		// Token: 0x04001F04 RID: 7940
		private const int PASSWORD_SIZE = 14;

		// Token: 0x04001F05 RID: 7941
		private string _sqlConnectionString;

		// Token: 0x04001F06 RID: 7942
		private bool _EnablePasswordRetrieval;

		// Token: 0x04001F07 RID: 7943
		private bool _EnablePasswordReset;

		// Token: 0x04001F08 RID: 7944
		private bool _RequiresQuestionAndAnswer;

		// Token: 0x04001F09 RID: 7945
		private string _AppName;

		// Token: 0x04001F0A RID: 7946
		private bool _RequiresUniqueEmail;

		// Token: 0x04001F0B RID: 7947
		private int _MaxInvalidPasswordAttempts;

		// Token: 0x04001F0C RID: 7948
		private int _CommandTimeout;

		// Token: 0x04001F0D RID: 7949
		private int _PasswordAttemptWindow;

		// Token: 0x04001F0E RID: 7950
		private int _MinRequiredPasswordLength;

		// Token: 0x04001F0F RID: 7951
		private int _MinRequiredNonalphanumericCharacters;

		// Token: 0x04001F10 RID: 7952
		private string _PasswordStrengthRegularExpression;

		// Token: 0x04001F11 RID: 7953
		private int _SchemaVersionCheck;

		// Token: 0x04001F12 RID: 7954
		private MembershipPasswordFormat _PasswordFormat;
	}
}
