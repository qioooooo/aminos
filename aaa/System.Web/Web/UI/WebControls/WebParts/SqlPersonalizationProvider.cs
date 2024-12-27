using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Security.Permissions;
using System.Web.DataAccess;
using System.Web.Util;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006FA RID: 1786
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class SqlPersonalizationProvider : PersonalizationProvider
	{
		// Token: 0x17001689 RID: 5769
		// (get) Token: 0x06005745 RID: 22341 RVA: 0x0015F75B File Offset: 0x0015E75B
		// (set) Token: 0x06005746 RID: 22342 RVA: 0x0015F77C File Offset: 0x0015E77C
		public override string ApplicationName
		{
			get
			{
				if (string.IsNullOrEmpty(this._applicationName))
				{
					this._applicationName = SecUtility.GetDefaultAppName();
				}
				return this._applicationName;
			}
			set
			{
				if (value != null && value.Length > 256)
				{
					throw new ProviderException(SR.GetString("PersonalizationProvider_ApplicationNameExceedMaxLength", new object[] { 256.ToString(CultureInfo.CurrentCulture) }));
				}
				this._applicationName = value;
			}
		}

		// Token: 0x06005747 RID: 22343 RVA: 0x0015F7D0 File Offset: 0x0015E7D0
		private SqlParameter CreateParameter(string name, SqlDbType dbType, object value)
		{
			return new SqlParameter(name, dbType)
			{
				Value = value
			};
		}

		// Token: 0x06005748 RID: 22344 RVA: 0x0015F7F0 File Offset: 0x0015E7F0
		private PersonalizationStateInfoCollection FindSharedState(string path, int pageIndex, int pageSize, out int totalRecords)
		{
			SqlConnectionHolder sqlConnectionHolder = null;
			SqlDataReader sqlDataReader = null;
			totalRecords = 0;
			PersonalizationStateInfoCollection personalizationStateInfoCollection2;
			try
			{
				try
				{
					sqlConnectionHolder = this.GetConnectionHolder();
					SqlConnection connection = sqlConnectionHolder.Connection;
					this.CheckSchemaVersion(connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_PersonalizationAdministration_FindState", connection);
					this.SetCommandTypeAndTimeout(sqlCommand);
					SqlParameterCollection parameters = sqlCommand.Parameters;
					SqlParameter sqlParameter = parameters.Add(new SqlParameter("AllUsersScope", SqlDbType.Bit));
					sqlParameter.Value = true;
					parameters.AddWithValue("ApplicationName", this.ApplicationName);
					parameters.AddWithValue("PageIndex", pageIndex);
					parameters.AddWithValue("PageSize", pageSize);
					SqlParameter sqlParameter2 = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter2.Direction = ParameterDirection.ReturnValue;
					parameters.Add(sqlParameter2);
					sqlParameter = parameters.Add("Path", SqlDbType.NVarChar);
					if (path != null)
					{
						sqlParameter.Value = path;
					}
					sqlParameter = parameters.Add("UserName", SqlDbType.NVarChar);
					sqlParameter = parameters.Add("InactiveSinceDate", SqlDbType.DateTime);
					sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SequentialAccess);
					PersonalizationStateInfoCollection personalizationStateInfoCollection = new PersonalizationStateInfoCollection();
					if (sqlDataReader != null)
					{
						if (sqlDataReader.HasRows)
						{
							while (sqlDataReader.Read())
							{
								string @string = sqlDataReader.GetString(0);
								DateTime dateTime = (sqlDataReader.IsDBNull(1) ? DateTime.MinValue : DateTime.SpecifyKind(sqlDataReader.GetDateTime(1), DateTimeKind.Utc));
								int num = (sqlDataReader.IsDBNull(2) ? 0 : sqlDataReader.GetInt32(2));
								int num2 = (sqlDataReader.IsDBNull(3) ? 0 : sqlDataReader.GetInt32(3));
								int num3 = (sqlDataReader.IsDBNull(4) ? 0 : sqlDataReader.GetInt32(4));
								personalizationStateInfoCollection.Add(new SharedPersonalizationStateInfo(@string, dateTime, num, num2, num3));
							}
						}
						sqlDataReader.Close();
						sqlDataReader = null;
					}
					if (sqlParameter2.Value != null && sqlParameter2.Value is int)
					{
						totalRecords = (int)sqlParameter2.Value;
					}
					personalizationStateInfoCollection2 = personalizationStateInfoCollection;
				}
				finally
				{
					if (sqlDataReader != null)
					{
						sqlDataReader.Close();
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
			return personalizationStateInfoCollection2;
		}

		// Token: 0x06005749 RID: 22345 RVA: 0x0015FA18 File Offset: 0x0015EA18
		public override PersonalizationStateInfoCollection FindState(PersonalizationScope scope, PersonalizationStateQuery query, int pageIndex, int pageSize, out int totalRecords)
		{
			PersonalizationProviderHelper.CheckPersonalizationScope(scope);
			PersonalizationProviderHelper.CheckPageIndexAndSize(pageIndex, pageSize);
			if (scope == PersonalizationScope.Shared)
			{
				string text = null;
				if (query != null)
				{
					text = StringUtil.CheckAndTrimString(query.PathToMatch, "query.PathToMatch", false, 256);
				}
				return this.FindSharedState(text, pageIndex, pageSize, out totalRecords);
			}
			string text2 = null;
			DateTime dateTime = PersonalizationAdministration.DefaultInactiveSinceDate;
			string text3 = null;
			if (query != null)
			{
				text2 = StringUtil.CheckAndTrimString(query.PathToMatch, "query.PathToMatch", false, 256);
				dateTime = query.UserInactiveSinceDate;
				text3 = StringUtil.CheckAndTrimString(query.UsernameToMatch, "query.UsernameToMatch", false, 256);
			}
			return this.FindUserState(text2, dateTime, text3, pageIndex, pageSize, out totalRecords);
		}

		// Token: 0x0600574A RID: 22346 RVA: 0x0015FAB0 File Offset: 0x0015EAB0
		private PersonalizationStateInfoCollection FindUserState(string path, DateTime inactiveSinceDate, string username, int pageIndex, int pageSize, out int totalRecords)
		{
			SqlConnectionHolder sqlConnectionHolder = null;
			SqlDataReader sqlDataReader = null;
			totalRecords = 0;
			PersonalizationStateInfoCollection personalizationStateInfoCollection2;
			try
			{
				try
				{
					sqlConnectionHolder = this.GetConnectionHolder();
					SqlConnection connection = sqlConnectionHolder.Connection;
					this.CheckSchemaVersion(connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_PersonalizationAdministration_FindState", connection);
					this.SetCommandTypeAndTimeout(sqlCommand);
					SqlParameterCollection parameters = sqlCommand.Parameters;
					SqlParameter sqlParameter = parameters.Add(new SqlParameter("AllUsersScope", SqlDbType.Bit));
					sqlParameter.Value = false;
					parameters.AddWithValue("ApplicationName", this.ApplicationName);
					parameters.AddWithValue("PageIndex", pageIndex);
					parameters.AddWithValue("PageSize", pageSize);
					SqlParameter sqlParameter2 = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter2.Direction = ParameterDirection.ReturnValue;
					parameters.Add(sqlParameter2);
					sqlParameter = parameters.Add("Path", SqlDbType.NVarChar);
					if (path != null)
					{
						sqlParameter.Value = path;
					}
					sqlParameter = parameters.Add("UserName", SqlDbType.NVarChar);
					if (username != null)
					{
						sqlParameter.Value = username;
					}
					sqlParameter = parameters.Add("InactiveSinceDate", SqlDbType.DateTime);
					if (inactiveSinceDate != PersonalizationAdministration.DefaultInactiveSinceDate)
					{
						sqlParameter.Value = inactiveSinceDate.ToUniversalTime();
					}
					sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SequentialAccess);
					PersonalizationStateInfoCollection personalizationStateInfoCollection = new PersonalizationStateInfoCollection();
					if (sqlDataReader != null)
					{
						if (sqlDataReader.HasRows)
						{
							while (sqlDataReader.Read())
							{
								string @string = sqlDataReader.GetString(0);
								DateTime dateTime = DateTime.SpecifyKind(sqlDataReader.GetDateTime(1), DateTimeKind.Utc);
								int @int = sqlDataReader.GetInt32(2);
								string string2 = sqlDataReader.GetString(3);
								DateTime dateTime2 = DateTime.SpecifyKind(sqlDataReader.GetDateTime(4), DateTimeKind.Utc);
								personalizationStateInfoCollection.Add(new UserPersonalizationStateInfo(@string, dateTime, @int, string2, dateTime2));
							}
						}
						sqlDataReader.Close();
						sqlDataReader = null;
					}
					if (sqlParameter2.Value != null && sqlParameter2.Value is int)
					{
						totalRecords = (int)sqlParameter2.Value;
					}
					personalizationStateInfoCollection2 = personalizationStateInfoCollection;
				}
				finally
				{
					if (sqlDataReader != null)
					{
						sqlDataReader.Close();
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
			return personalizationStateInfoCollection2;
		}

		// Token: 0x0600574B RID: 22347 RVA: 0x0015FCD0 File Offset: 0x0015ECD0
		private SqlConnectionHolder GetConnectionHolder()
		{
			SqlConnection sqlConnection = null;
			SqlConnectionHolder connection = SqlConnectionHelper.GetConnection(this._connectionString, true);
			if (connection != null)
			{
				sqlConnection = connection.Connection;
			}
			if (sqlConnection == null)
			{
				throw new ProviderException(SR.GetString("PersonalizationProvider_CantAccess", new object[] { this.Name }));
			}
			return connection;
		}

		// Token: 0x0600574C RID: 22348 RVA: 0x0015FD1C File Offset: 0x0015ED1C
		private int GetCountOfSharedState(string path)
		{
			SqlConnectionHolder sqlConnectionHolder = null;
			int num = 0;
			try
			{
				try
				{
					sqlConnectionHolder = this.GetConnectionHolder();
					SqlConnection connection = sqlConnectionHolder.Connection;
					this.CheckSchemaVersion(connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_PersonalizationAdministration_GetCountOfState", connection);
					this.SetCommandTypeAndTimeout(sqlCommand);
					SqlParameterCollection parameters = sqlCommand.Parameters;
					SqlParameter sqlParameter = parameters.Add(new SqlParameter("Count", SqlDbType.Int));
					sqlParameter.Direction = ParameterDirection.Output;
					sqlParameter = parameters.Add(new SqlParameter("AllUsersScope", SqlDbType.Bit));
					sqlParameter.Value = true;
					parameters.AddWithValue("ApplicationName", this.ApplicationName);
					sqlParameter = parameters.Add("Path", SqlDbType.NVarChar);
					if (path != null)
					{
						sqlParameter.Value = path;
					}
					sqlParameter = parameters.Add("UserName", SqlDbType.NVarChar);
					sqlParameter = parameters.Add("InactiveSinceDate", SqlDbType.DateTime);
					sqlCommand.ExecuteNonQuery();
					sqlParameter = sqlCommand.Parameters[0];
					if (sqlParameter != null && sqlParameter.Value != null && sqlParameter.Value is int)
					{
						num = (int)sqlParameter.Value;
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
			return num;
		}

		// Token: 0x0600574D RID: 22349 RVA: 0x0015FE70 File Offset: 0x0015EE70
		public override int GetCountOfState(PersonalizationScope scope, PersonalizationStateQuery query)
		{
			PersonalizationProviderHelper.CheckPersonalizationScope(scope);
			if (scope == PersonalizationScope.Shared)
			{
				string text = null;
				if (query != null)
				{
					text = StringUtil.CheckAndTrimString(query.PathToMatch, "query.PathToMatch", false, 256);
				}
				return this.GetCountOfSharedState(text);
			}
			string text2 = null;
			DateTime dateTime = PersonalizationAdministration.DefaultInactiveSinceDate;
			string text3 = null;
			if (query != null)
			{
				text2 = StringUtil.CheckAndTrimString(query.PathToMatch, "query.PathToMatch", false, 256);
				dateTime = query.UserInactiveSinceDate;
				text3 = StringUtil.CheckAndTrimString(query.UsernameToMatch, "query.UsernameToMatch", false, 256);
			}
			return this.GetCountOfUserState(text2, dateTime, text3);
		}

		// Token: 0x0600574E RID: 22350 RVA: 0x0015FEF8 File Offset: 0x0015EEF8
		private int GetCountOfUserState(string path, DateTime inactiveSinceDate, string username)
		{
			SqlConnectionHolder sqlConnectionHolder = null;
			int num = 0;
			try
			{
				try
				{
					sqlConnectionHolder = this.GetConnectionHolder();
					SqlConnection connection = sqlConnectionHolder.Connection;
					this.CheckSchemaVersion(connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_PersonalizationAdministration_GetCountOfState", connection);
					this.SetCommandTypeAndTimeout(sqlCommand);
					SqlParameterCollection parameters = sqlCommand.Parameters;
					SqlParameter sqlParameter = parameters.Add(new SqlParameter("Count", SqlDbType.Int));
					sqlParameter.Direction = ParameterDirection.Output;
					sqlParameter = parameters.Add(new SqlParameter("AllUsersScope", SqlDbType.Bit));
					sqlParameter.Value = false;
					parameters.AddWithValue("ApplicationName", this.ApplicationName);
					sqlParameter = parameters.Add("Path", SqlDbType.NVarChar);
					if (path != null)
					{
						sqlParameter.Value = path;
					}
					sqlParameter = parameters.Add("UserName", SqlDbType.NVarChar);
					if (username != null)
					{
						sqlParameter.Value = username;
					}
					sqlParameter = parameters.Add("InactiveSinceDate", SqlDbType.DateTime);
					if (inactiveSinceDate != PersonalizationAdministration.DefaultInactiveSinceDate)
					{
						sqlParameter.Value = inactiveSinceDate.ToUniversalTime();
					}
					sqlCommand.ExecuteNonQuery();
					sqlParameter = sqlCommand.Parameters[0];
					if (sqlParameter != null && sqlParameter.Value != null && sqlParameter.Value is int)
					{
						num = (int)sqlParameter.Value;
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
			return num;
		}

		// Token: 0x0600574F RID: 22351 RVA: 0x00160078 File Offset: 0x0015F078
		public override void Initialize(string name, NameValueCollection configSettings)
		{
			HttpRuntime.CheckAspNetHostingPermission(AspNetHostingPermissionLevel.Low, "Feature_not_supported_at_this_level");
			if (configSettings == null)
			{
				throw new ArgumentNullException("configSettings");
			}
			if (string.IsNullOrEmpty(name))
			{
				name = "SqlPersonalizationProvider";
			}
			if (string.IsNullOrEmpty(configSettings["description"]))
			{
				configSettings.Remove("description");
				configSettings.Add("description", SR.GetString("SqlPersonalizationProvider_Description"));
			}
			base.Initialize(name, configSettings);
			this._SchemaVersionCheck = 0;
			this._applicationName = configSettings["applicationName"];
			if (this._applicationName != null)
			{
				configSettings.Remove("applicationName");
				if (this._applicationName.Length > 256)
				{
					throw new ProviderException(SR.GetString("PersonalizationProvider_ApplicationNameExceedMaxLength", new object[] { 256.ToString(CultureInfo.CurrentCulture) }));
				}
			}
			string text = configSettings["connectionStringName"];
			if (string.IsNullOrEmpty(text))
			{
				throw new ProviderException(SR.GetString("PersonalizationProvider_NoConnection"));
			}
			configSettings.Remove("connectionStringName");
			string connectionString = SqlConnectionHelper.GetConnectionString(text, true, true);
			if (string.IsNullOrEmpty(connectionString))
			{
				throw new ProviderException(SR.GetString("PersonalizationProvider_BadConnection", new object[] { text }));
			}
			this._connectionString = connectionString;
			this._commandTimeout = SecUtility.GetIntValue(configSettings, "commandTimeout", -1, true, 0);
			configSettings.Remove("commandTimeout");
			if (configSettings.Count > 0)
			{
				string key = configSettings.GetKey(0);
				throw new ProviderException(SR.GetString("PersonalizationProvider_UnknownProp", new object[] { key, name }));
			}
		}

		// Token: 0x06005750 RID: 22352 RVA: 0x00160210 File Offset: 0x0015F210
		private void CheckSchemaVersion(SqlConnection connection)
		{
			string[] array = new string[] { "Personalization" };
			string text = "1";
			SecUtility.CheckSchemaVersion(this, connection, array, text, ref this._SchemaVersionCheck);
		}

		// Token: 0x06005751 RID: 22353 RVA: 0x00160244 File Offset: 0x0015F244
		private byte[] LoadPersonalizationBlob(SqlConnection connection, string path, string userName)
		{
			SqlCommand sqlCommand;
			if (userName != null)
			{
				sqlCommand = new SqlCommand("dbo.aspnet_PersonalizationPerUser_GetPageSettings", connection);
			}
			else
			{
				sqlCommand = new SqlCommand("dbo.aspnet_PersonalizationAllUsers_GetPageSettings", connection);
			}
			this.SetCommandTypeAndTimeout(sqlCommand);
			sqlCommand.Parameters.Add(this.CreateParameter("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
			sqlCommand.Parameters.Add(this.CreateParameter("@Path", SqlDbType.NVarChar, path));
			if (userName != null)
			{
				sqlCommand.Parameters.Add(this.CreateParameter("@UserName", SqlDbType.NVarChar, userName));
				sqlCommand.Parameters.Add(this.CreateParameter("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.UtcNow));
			}
			SqlDataReader sqlDataReader = null;
			try
			{
				sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SingleRow);
				if (sqlDataReader.Read())
				{
					int num = (int)sqlDataReader.GetBytes(0, 0L, null, 0, 0);
					byte[] array = new byte[num];
					sqlDataReader.GetBytes(0, 0L, array, 0, num);
					return array;
				}
			}
			finally
			{
				if (sqlDataReader != null)
				{
					sqlDataReader.Close();
				}
			}
			return null;
		}

		// Token: 0x06005752 RID: 22354 RVA: 0x00160348 File Offset: 0x0015F348
		protected override void LoadPersonalizationBlobs(WebPartManager webPartManager, string path, string userName, ref byte[] sharedDataBlob, ref byte[] userDataBlob)
		{
			sharedDataBlob = null;
			userDataBlob = null;
			SqlConnectionHolder sqlConnectionHolder = null;
			try
			{
				try
				{
					sqlConnectionHolder = this.GetConnectionHolder();
					SqlConnection connection = sqlConnectionHolder.Connection;
					this.CheckSchemaVersion(connection);
					sharedDataBlob = this.LoadPersonalizationBlob(connection, path, null);
					if (!string.IsNullOrEmpty(userName))
					{
						userDataBlob = this.LoadPersonalizationBlob(connection, path, userName);
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

		// Token: 0x06005753 RID: 22355 RVA: 0x001603C8 File Offset: 0x0015F3C8
		private void ResetPersonalizationState(SqlConnection connection, string path, string userName)
		{
			SqlCommand sqlCommand;
			if (userName != null)
			{
				sqlCommand = new SqlCommand("dbo.aspnet_PersonalizationPerUser_ResetPageSettings", connection);
			}
			else
			{
				sqlCommand = new SqlCommand("dbo.aspnet_PersonalizationAllUsers_ResetPageSettings", connection);
			}
			this.SetCommandTypeAndTimeout(sqlCommand);
			sqlCommand.Parameters.Add(this.CreateParameter("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
			sqlCommand.Parameters.Add(this.CreateParameter("@Path", SqlDbType.NVarChar, path));
			if (userName != null)
			{
				sqlCommand.Parameters.Add(this.CreateParameter("@UserName", SqlDbType.NVarChar, userName));
				sqlCommand.Parameters.Add(this.CreateParameter("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.UtcNow));
			}
			sqlCommand.ExecuteNonQuery();
		}

		// Token: 0x06005754 RID: 22356 RVA: 0x00160478 File Offset: 0x0015F478
		protected override void ResetPersonalizationBlob(WebPartManager webPartManager, string path, string userName)
		{
			SqlConnectionHolder sqlConnectionHolder = null;
			try
			{
				try
				{
					sqlConnectionHolder = this.GetConnectionHolder();
					SqlConnection connection = sqlConnectionHolder.Connection;
					this.CheckSchemaVersion(connection);
					this.ResetPersonalizationState(connection, path, userName);
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

		// Token: 0x06005755 RID: 22357 RVA: 0x001604D8 File Offset: 0x0015F4D8
		private int ResetAllState(PersonalizationScope scope)
		{
			SqlConnectionHolder sqlConnectionHolder = null;
			int num = 0;
			try
			{
				try
				{
					sqlConnectionHolder = this.GetConnectionHolder();
					SqlConnection connection = sqlConnectionHolder.Connection;
					this.CheckSchemaVersion(connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_PersonalizationAdministration_DeleteAllState", connection);
					this.SetCommandTypeAndTimeout(sqlCommand);
					SqlParameterCollection parameters = sqlCommand.Parameters;
					SqlParameter sqlParameter = parameters.Add(new SqlParameter("AllUsersScope", SqlDbType.Bit));
					sqlParameter.Value = scope == PersonalizationScope.Shared;
					parameters.AddWithValue("ApplicationName", this.ApplicationName);
					sqlParameter = parameters.Add(new SqlParameter("Count", SqlDbType.Int));
					sqlParameter.Direction = ParameterDirection.Output;
					sqlCommand.ExecuteNonQuery();
					sqlParameter = sqlCommand.Parameters[2];
					if (sqlParameter != null && sqlParameter.Value != null && sqlParameter.Value is int)
					{
						num = (int)sqlParameter.Value;
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
			return num;
		}

		// Token: 0x06005756 RID: 22358 RVA: 0x001605DC File Offset: 0x0015F5DC
		private int ResetSharedState(string[] paths)
		{
			int num = 0;
			if (paths == null)
			{
				num = this.ResetAllState(PersonalizationScope.Shared);
			}
			else
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				SqlConnection sqlConnection = null;
				try
				{
					bool flag = false;
					try
					{
						sqlConnectionHolder = this.GetConnectionHolder();
						sqlConnection = sqlConnectionHolder.Connection;
						this.CheckSchemaVersion(sqlConnection);
						SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_PersonalizationAdministration_ResetSharedState", sqlConnection);
						this.SetCommandTypeAndTimeout(sqlCommand);
						SqlParameterCollection parameters = sqlCommand.Parameters;
						SqlParameter sqlParameter = parameters.Add(new SqlParameter("Count", SqlDbType.Int));
						sqlParameter.Direction = ParameterDirection.Output;
						parameters.AddWithValue("ApplicationName", this.ApplicationName);
						sqlParameter = parameters.Add("Path", SqlDbType.NVarChar);
						foreach (string text in paths)
						{
							if (!flag && paths.Length > 1)
							{
								new SqlCommand("BEGIN TRANSACTION", sqlConnection).ExecuteNonQuery();
								flag = true;
							}
							sqlParameter.Value = text;
							sqlCommand.ExecuteNonQuery();
							SqlParameter sqlParameter2 = sqlCommand.Parameters[0];
							if (sqlParameter2 != null && sqlParameter2.Value != null && sqlParameter2.Value is int)
							{
								num += (int)sqlParameter2.Value;
							}
						}
						if (flag)
						{
							new SqlCommand("COMMIT TRANSACTION", sqlConnection).ExecuteNonQuery();
							flag = false;
						}
					}
					catch
					{
						if (flag)
						{
							new SqlCommand("ROLLBACK TRANSACTION", sqlConnection).ExecuteNonQuery();
							flag = false;
						}
						throw;
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
			return num;
		}

		// Token: 0x06005757 RID: 22359 RVA: 0x00160788 File Offset: 0x0015F788
		public override int ResetUserState(string path, DateTime userInactiveSinceDate)
		{
			path = StringUtil.CheckAndTrimString(path, "path", false, 256);
			string[] array = ((path == null) ? null : new string[] { path });
			return this.ResetUserState(SqlPersonalizationProvider.ResetUserStateMode.PerInactiveDate, userInactiveSinceDate, array, null);
		}

		// Token: 0x06005758 RID: 22360 RVA: 0x001607C8 File Offset: 0x0015F7C8
		public override int ResetState(PersonalizationScope scope, string[] paths, string[] usernames)
		{
			PersonalizationProviderHelper.CheckPersonalizationScope(scope);
			paths = PersonalizationProviderHelper.CheckAndTrimNonEmptyStringEntries(paths, "paths", false, false, 256);
			usernames = PersonalizationProviderHelper.CheckAndTrimNonEmptyStringEntries(usernames, "usernames", false, true, 256);
			if (scope == PersonalizationScope.Shared)
			{
				PersonalizationProviderHelper.CheckUsernamesInSharedScope(usernames);
				return this.ResetSharedState(paths);
			}
			PersonalizationProviderHelper.CheckOnlyOnePathWithUsers(paths, usernames);
			return this.ResetUserState(paths, usernames);
		}

		// Token: 0x06005759 RID: 22361 RVA: 0x00160824 File Offset: 0x0015F824
		private int ResetUserState(string[] paths, string[] usernames)
		{
			bool flag = paths != null && paths.Length != 0;
			bool flag2 = usernames != null && usernames.Length != 0;
			int num;
			if (!flag && !flag2)
			{
				num = this.ResetAllState(PersonalizationScope.User);
			}
			else if (!flag2)
			{
				num = this.ResetUserState(SqlPersonalizationProvider.ResetUserStateMode.PerPaths, PersonalizationAdministration.DefaultInactiveSinceDate, paths, usernames);
			}
			else
			{
				num = this.ResetUserState(SqlPersonalizationProvider.ResetUserStateMode.PerUsers, PersonalizationAdministration.DefaultInactiveSinceDate, paths, usernames);
			}
			return num;
		}

		// Token: 0x0600575A RID: 22362 RVA: 0x00160888 File Offset: 0x0015F888
		private int ResetUserState(SqlPersonalizationProvider.ResetUserStateMode mode, DateTime userInactiveSinceDate, string[] paths, string[] usernames)
		{
			SqlConnectionHolder sqlConnectionHolder = null;
			SqlConnection sqlConnection = null;
			int num = 0;
			try
			{
				bool flag = false;
				try
				{
					sqlConnectionHolder = this.GetConnectionHolder();
					sqlConnection = sqlConnectionHolder.Connection;
					this.CheckSchemaVersion(sqlConnection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_PersonalizationAdministration_ResetUserState", sqlConnection);
					this.SetCommandTypeAndTimeout(sqlCommand);
					SqlParameterCollection parameters = sqlCommand.Parameters;
					SqlParameter sqlParameter = parameters.Add(new SqlParameter("Count", SqlDbType.Int));
					sqlParameter.Direction = ParameterDirection.Output;
					parameters.AddWithValue("ApplicationName", this.ApplicationName);
					string text = ((paths != null && paths.Length > 0) ? paths[0] : null);
					if (mode == SqlPersonalizationProvider.ResetUserStateMode.PerInactiveDate)
					{
						if (userInactiveSinceDate != PersonalizationAdministration.DefaultInactiveSinceDate)
						{
							sqlParameter = parameters.Add("InactiveSinceDate", SqlDbType.DateTime);
							sqlParameter.Value = userInactiveSinceDate.ToUniversalTime();
						}
						if (text != null)
						{
							parameters.AddWithValue("Path", text);
						}
						sqlCommand.ExecuteNonQuery();
						SqlParameter sqlParameter2 = sqlCommand.Parameters[0];
						if (sqlParameter2 != null && sqlParameter2.Value != null && sqlParameter2.Value is int)
						{
							num = (int)sqlParameter2.Value;
						}
					}
					else if (mode == SqlPersonalizationProvider.ResetUserStateMode.PerPaths)
					{
						sqlParameter = parameters.Add("Path", SqlDbType.NVarChar);
						foreach (string text2 in paths)
						{
							if (!flag && paths.Length > 1)
							{
								new SqlCommand("BEGIN TRANSACTION", sqlConnection).ExecuteNonQuery();
								flag = true;
							}
							sqlParameter.Value = text2;
							sqlCommand.ExecuteNonQuery();
							SqlParameter sqlParameter3 = sqlCommand.Parameters[0];
							if (sqlParameter3 != null && sqlParameter3.Value != null && sqlParameter3.Value is int)
							{
								num += (int)sqlParameter3.Value;
							}
						}
					}
					else
					{
						if (text != null)
						{
							parameters.AddWithValue("Path", text);
						}
						sqlParameter = parameters.Add("UserName", SqlDbType.NVarChar);
						foreach (string text3 in usernames)
						{
							if (!flag && usernames.Length > 1)
							{
								new SqlCommand("BEGIN TRANSACTION", sqlConnection).ExecuteNonQuery();
								flag = true;
							}
							sqlParameter.Value = text3;
							sqlCommand.ExecuteNonQuery();
							SqlParameter sqlParameter4 = sqlCommand.Parameters[0];
							if (sqlParameter4 != null && sqlParameter4.Value != null && sqlParameter4.Value is int)
							{
								num += (int)sqlParameter4.Value;
							}
						}
					}
					if (flag)
					{
						new SqlCommand("COMMIT TRANSACTION", sqlConnection).ExecuteNonQuery();
						flag = false;
					}
				}
				catch
				{
					if (flag)
					{
						new SqlCommand("ROLLBACK TRANSACTION", sqlConnection).ExecuteNonQuery();
						flag = false;
					}
					throw;
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
			return num;
		}

		// Token: 0x0600575B RID: 22363 RVA: 0x00160B80 File Offset: 0x0015FB80
		private void SavePersonalizationState(SqlConnection connection, string path, string userName, byte[] state)
		{
			SqlCommand sqlCommand;
			if (userName != null)
			{
				sqlCommand = new SqlCommand("dbo.aspnet_PersonalizationPerUser_SetPageSettings", connection);
			}
			else
			{
				sqlCommand = new SqlCommand("dbo.aspnet_PersonalizationAllUsers_SetPageSettings", connection);
			}
			this.SetCommandTypeAndTimeout(sqlCommand);
			sqlCommand.Parameters.Add(this.CreateParameter("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
			sqlCommand.Parameters.Add(this.CreateParameter("@Path", SqlDbType.NVarChar, path));
			sqlCommand.Parameters.Add(this.CreateParameter("@PageSettings", SqlDbType.Image, state));
			sqlCommand.Parameters.Add(this.CreateParameter("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.UtcNow));
			if (userName != null)
			{
				sqlCommand.Parameters.Add(this.CreateParameter("@UserName", SqlDbType.NVarChar, userName));
			}
			sqlCommand.ExecuteNonQuery();
		}

		// Token: 0x0600575C RID: 22364 RVA: 0x00160C4C File Offset: 0x0015FC4C
		protected override void SavePersonalizationBlob(WebPartManager webPartManager, string path, string userName, byte[] dataBlob)
		{
			SqlConnectionHolder sqlConnectionHolder = null;
			try
			{
				try
				{
					sqlConnectionHolder = this.GetConnectionHolder();
					SqlConnection connection = sqlConnectionHolder.Connection;
					this.CheckSchemaVersion(connection);
					this.SavePersonalizationState(connection, path, userName, dataBlob);
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

		// Token: 0x0600575D RID: 22365 RVA: 0x00160CAC File Offset: 0x0015FCAC
		private void SetCommandTypeAndTimeout(SqlCommand command)
		{
			command.CommandType = CommandType.StoredProcedure;
			if (this._commandTimeout != -1)
			{
				command.CommandTimeout = this._commandTimeout;
			}
		}

		// Token: 0x04002F93 RID: 12179
		private const int maxStringLength = 256;

		// Token: 0x04002F94 RID: 12180
		private string _applicationName;

		// Token: 0x04002F95 RID: 12181
		private int _commandTimeout;

		// Token: 0x04002F96 RID: 12182
		private string _connectionString;

		// Token: 0x04002F97 RID: 12183
		private int _SchemaVersionCheck;

		// Token: 0x020006FB RID: 1787
		private enum ResetUserStateMode
		{
			// Token: 0x04002F99 RID: 12185
			PerInactiveDate,
			// Token: 0x04002F9A RID: 12186
			PerPaths,
			// Token: 0x04002F9B RID: 12187
			PerUsers
		}
	}
}
