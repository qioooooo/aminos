using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Security.Permissions;
using System.Web.DataAccess;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web.Profile
{
	// Token: 0x02000313 RID: 787
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class SqlProfileProvider : ProfileProvider
	{
		// Token: 0x060026AD RID: 9901 RVA: 0x000A592C File Offset: 0x000A492C
		public override void Initialize(string name, NameValueCollection config)
		{
			HttpRuntime.CheckAspNetHostingPermission(AspNetHostingPermissionLevel.Low, "Feature_not_supported_at_this_level");
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}
			if (name == null || name.Length < 1)
			{
				name = "SqlProfileProvider";
			}
			if (string.IsNullOrEmpty(config["description"]))
			{
				config.Remove("description");
				config.Add("description", SR.GetString("ProfileSqlProvider_description"));
			}
			base.Initialize(name, config);
			this._SchemaVersionCheck = 0;
			string text = config["connectionStringName"];
			if (text == null || text.Length < 1)
			{
				throw new ProviderException(SR.GetString("Connection_name_not_specified"));
			}
			this._sqlConnectionString = SqlConnectionHelper.GetConnectionString(text, true, true);
			if (this._sqlConnectionString == null || this._sqlConnectionString.Length < 1)
			{
				throw new ProviderException(SR.GetString("Connection_string_not_found", new object[] { text }));
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
			this._CommandTimeout = SecUtility.GetIntValue(config, "commandTimeout", 30, true, 0);
			config.Remove("commandTimeout");
			config.Remove("connectionStringName");
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

		// Token: 0x060026AE RID: 9902 RVA: 0x000A5AC8 File Offset: 0x000A4AC8
		private void CheckSchemaVersion(SqlConnection connection)
		{
			string[] array = new string[] { "Profile" };
			string text = "1";
			SecUtility.CheckSchemaVersion(this, connection, array, text, ref this._SchemaVersionCheck);
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x060026AF RID: 9903 RVA: 0x000A5AFB File Offset: 0x000A4AFB
		// (set) Token: 0x060026B0 RID: 9904 RVA: 0x000A5B03 File Offset: 0x000A4B03
		public override string ApplicationName
		{
			get
			{
				return this._AppName;
			}
			set
			{
				if (value.Length > 256)
				{
					throw new ProviderException(SR.GetString("Provider_application_name_too_long"));
				}
				this._AppName = value;
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x060026B1 RID: 9905 RVA: 0x000A5B29 File Offset: 0x000A4B29
		private int CommandTimeout
		{
			get
			{
				return this._CommandTimeout;
			}
		}

		// Token: 0x060026B2 RID: 9906 RVA: 0x000A5B34 File Offset: 0x000A4B34
		public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext sc, SettingsPropertyCollection properties)
		{
			SettingsPropertyValueCollection settingsPropertyValueCollection = new SettingsPropertyValueCollection();
			if (properties.Count < 1)
			{
				return settingsPropertyValueCollection;
			}
			string text = (string)sc["UserName"];
			foreach (object obj in properties)
			{
				SettingsProperty settingsProperty = (SettingsProperty)obj;
				if (settingsProperty.SerializeAs == SettingsSerializeAs.ProviderSpecific)
				{
					if (settingsProperty.PropertyType.IsPrimitive || settingsProperty.PropertyType == typeof(string))
					{
						settingsProperty.SerializeAs = SettingsSerializeAs.String;
					}
					else
					{
						settingsProperty.SerializeAs = SettingsSerializeAs.Xml;
					}
				}
				settingsPropertyValueCollection.Add(new SettingsPropertyValue(settingsProperty));
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.GetPropertyValuesFromDatabase(text, settingsPropertyValueCollection);
			}
			return settingsPropertyValueCollection;
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x000A5BFC File Offset: 0x000A4BFC
		private void GetPropertyValuesFromDatabase(string userName, SettingsPropertyValueCollection svc)
		{
			if (HostingEnvironment.IsHosted && EtwTrace.IsTraceEnabled(4, 8))
			{
				EtwTrace.Trace(EtwTraceType.ETW_TYPE_PROFILE_BEGIN, HttpContext.Current.WorkerRequest);
			}
			HttpContext httpContext = HttpContext.Current;
			string[] array = null;
			string text = null;
			byte[] array2 = null;
			if (httpContext != null)
			{
				if (!httpContext.Request.IsAuthenticated)
				{
					string anonymousID = httpContext.Request.AnonymousID;
				}
				else
				{
					string name = httpContext.User.Identity.Name;
				}
			}
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				SqlDataReader sqlDataReader = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					sqlDataReader = new SqlCommand("dbo.aspnet_Profile_GetProperties", sqlConnectionHolder.Connection)
					{
						CommandTimeout = this.CommandTimeout,
						CommandType = CommandType.StoredProcedure,
						Parameters = 
						{
							this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName),
							this.CreateInputParam("@UserName", SqlDbType.NVarChar, userName),
							this.CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.UtcNow)
						}
					}.ExecuteReader(CommandBehavior.SingleRow);
					if (sqlDataReader.Read())
					{
						array = sqlDataReader.GetString(0).Split(new char[] { ':' });
						text = sqlDataReader.GetString(1);
						int num = (int)sqlDataReader.GetBytes(2, 0L, null, 0, 0);
						array2 = new byte[num];
						sqlDataReader.GetBytes(2, 0L, array2, 0, num);
					}
				}
				finally
				{
					if (sqlConnectionHolder != null)
					{
						sqlConnectionHolder.Close();
						sqlConnectionHolder = null;
					}
					if (sqlDataReader != null)
					{
						sqlDataReader.Close();
					}
				}
				ProfileModule.ParseDataFromDB(array, text, array2, svc);
				if (HostingEnvironment.IsHosted && EtwTrace.IsTraceEnabled(4, 8))
				{
					EtwTrace.Trace(EtwTraceType.ETW_TYPE_PROFILE_END, HttpContext.Current.WorkerRequest, userName);
				}
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x000A5DF0 File Offset: 0x000A4DF0
		public override void SetPropertyValues(SettingsContext sc, SettingsPropertyValueCollection properties)
		{
			string text = (string)sc["UserName"];
			bool flag = (bool)sc["IsAuthenticated"];
			if (text == null || text.Length < 1 || properties.Count < 1)
			{
				return;
			}
			string empty = string.Empty;
			string empty2 = string.Empty;
			byte[] array = null;
			ProfileModule.PrepareDataForSaving(ref empty, ref empty2, ref array, true, properties, flag);
			if (empty.Length == 0)
			{
				return;
			}
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					new SqlCommand("dbo.aspnet_Profile_SetProperties", sqlConnectionHolder.Connection)
					{
						CommandTimeout = this.CommandTimeout,
						CommandType = CommandType.StoredProcedure,
						Parameters = 
						{
							this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName),
							this.CreateInputParam("@UserName", SqlDbType.NVarChar, text),
							this.CreateInputParam("@PropertyNames", SqlDbType.NText, empty),
							this.CreateInputParam("@PropertyValuesString", SqlDbType.NText, empty2),
							this.CreateInputParam("@PropertyValuesBinary", SqlDbType.Image, array),
							this.CreateInputParam("@IsUserAnonymous", SqlDbType.Bit, !flag),
							this.CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.UtcNow)
						}
					}.ExecuteNonQuery();
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

		// Token: 0x060026B5 RID: 9909 RVA: 0x000A5FC4 File Offset: 0x000A4FC4
		private SqlParameter CreateInputParam(string paramName, SqlDbType dbType, object objValue)
		{
			SqlParameter sqlParameter = new SqlParameter(paramName, dbType);
			if (objValue == null)
			{
				objValue = string.Empty;
			}
			sqlParameter.Value = objValue;
			return sqlParameter;
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x000A5FEC File Offset: 0x000A4FEC
		public override int DeleteProfiles(ProfileInfoCollection profiles)
		{
			if (profiles == null)
			{
				throw new ArgumentNullException("profiles");
			}
			if (profiles.Count < 1)
			{
				throw new ArgumentException(SR.GetString("Parameter_collection_empty", new object[] { "profiles" }), "profiles");
			}
			string[] array = new string[profiles.Count];
			int num = 0;
			foreach (object obj in profiles)
			{
				ProfileInfo profileInfo = (ProfileInfo)obj;
				array[num++] = profileInfo.UserName;
			}
			return this.DeleteProfiles(array);
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x000A60A0 File Offset: 0x000A50A0
		public override int DeleteProfiles(string[] usernames)
		{
			SecUtility.CheckArrayParameter(ref usernames, true, true, true, 256, "usernames");
			int num = 0;
			bool flag = false;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					int i = usernames.Length;
					while (i > 0)
					{
						string text = usernames[usernames.Length - i];
						i--;
						int num2 = usernames.Length - i;
						while (num2 < usernames.Length && text.Length + usernames[num2].Length + 1 < 4000)
						{
							text = text + "," + usernames[num2];
							i--;
							num2++;
						}
						if (!flag && i > 0)
						{
							SqlCommand sqlCommand = new SqlCommand("BEGIN TRANSACTION", sqlConnectionHolder.Connection);
							sqlCommand.ExecuteNonQuery();
							flag = true;
						}
						object obj = new SqlCommand("dbo.aspnet_Profile_DeleteProfiles", sqlConnectionHolder.Connection)
						{
							CommandTimeout = this.CommandTimeout,
							CommandType = CommandType.StoredProcedure,
							Parameters = 
							{
								this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName),
								this.CreateInputParam("@UserNames", SqlDbType.NVarChar, text)
							}
						}.ExecuteScalar();
						if (obj != null && obj is int)
						{
							num += (int)obj;
						}
					}
					if (flag)
					{
						SqlCommand sqlCommand = new SqlCommand("COMMIT TRANSACTION", sqlConnectionHolder.Connection);
						sqlCommand.ExecuteNonQuery();
						flag = false;
					}
				}
				catch
				{
					if (flag)
					{
						SqlCommand sqlCommand2 = new SqlCommand("ROLLBACK TRANSACTION", sqlConnectionHolder.Connection);
						sqlCommand2.ExecuteNonQuery();
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

		// Token: 0x060026B8 RID: 9912 RVA: 0x000A628C File Offset: 0x000A528C
		public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
		{
			int num;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					object obj = new SqlCommand("dbo.aspnet_Profile_DeleteInactiveProfiles", sqlConnectionHolder.Connection)
					{
						CommandTimeout = this.CommandTimeout,
						CommandType = CommandType.StoredProcedure,
						Parameters = 
						{
							this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName),
							this.CreateInputParam("@ProfileAuthOptions", SqlDbType.Int, (int)authenticationOption),
							this.CreateInputParam("@InactiveSinceDate", SqlDbType.DateTime, userInactiveSinceDate.ToUniversalTime())
						}
					}.ExecuteScalar();
					if (obj == null || !(obj is int))
					{
						num = 0;
					}
					else
					{
						num = (int)obj;
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

		// Token: 0x060026B9 RID: 9913 RVA: 0x000A6384 File Offset: 0x000A5384
		public override int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
		{
			int num;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					object obj = new SqlCommand("dbo.aspnet_Profile_GetNumberOfInactiveProfiles", sqlConnectionHolder.Connection)
					{
						CommandTimeout = this.CommandTimeout,
						CommandType = CommandType.StoredProcedure,
						Parameters = 
						{
							this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName),
							this.CreateInputParam("@ProfileAuthOptions", SqlDbType.Int, (int)authenticationOption),
							this.CreateInputParam("@InactiveSinceDate", SqlDbType.DateTime, userInactiveSinceDate.ToUniversalTime())
						}
					}.ExecuteScalar();
					if (obj == null || !(obj is int))
					{
						num = 0;
					}
					else
					{
						num = (int)obj;
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

		// Token: 0x060026BA RID: 9914 RVA: 0x000A647C File Offset: 0x000A547C
		public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
		{
			return this.GetProfilesForQuery(new SqlParameter[0], authenticationOption, pageIndex, pageSize, out totalRecords);
		}

		// Token: 0x060026BB RID: 9915 RVA: 0x000A6490 File Offset: 0x000A5490
		public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
		{
			return this.GetProfilesForQuery(new SqlParameter[] { this.CreateInputParam("@InactiveSinceDate", SqlDbType.DateTime, userInactiveSinceDate.ToUniversalTime()) }, authenticationOption, pageIndex, pageSize, out totalRecords);
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x000A64CC File Offset: 0x000A54CC
		public override ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			SecUtility.CheckParameter(ref usernameToMatch, true, true, false, 256, "username");
			return this.GetProfilesForQuery(new SqlParameter[] { this.CreateInputParam("@UserNameToMatch", SqlDbType.NVarChar, usernameToMatch) }, authenticationOption, pageIndex, pageSize, out totalRecords);
		}

		// Token: 0x060026BD RID: 9917 RVA: 0x000A6514 File Offset: 0x000A5514
		public override ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
		{
			SecUtility.CheckParameter(ref usernameToMatch, true, true, false, 256, "username");
			return this.GetProfilesForQuery(new SqlParameter[]
			{
				this.CreateInputParam("@UserNameToMatch", SqlDbType.NVarChar, usernameToMatch),
				this.CreateInputParam("@InactiveSinceDate", SqlDbType.DateTime, userInactiveSinceDate.ToUniversalTime())
			}, authenticationOption, pageIndex, pageSize, out totalRecords);
		}

		// Token: 0x060026BE RID: 9918 RVA: 0x000A6578 File Offset: 0x000A5578
		private ProfileInfoCollection GetProfilesForQuery(SqlParameter[] args, ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
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
			ProfileInfoCollection profileInfoCollection2;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				SqlDataReader sqlDataReader = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Profile_GetProfiles", sqlConnectionHolder.Connection);
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@ProfileAuthOptions", SqlDbType.Int, (int)authenticationOption));
					sqlCommand.Parameters.Add(this.CreateInputParam("@PageIndex", SqlDbType.Int, pageIndex));
					sqlCommand.Parameters.Add(this.CreateInputParam("@PageSize", SqlDbType.Int, pageSize));
					foreach (SqlParameter sqlParameter in args)
					{
						sqlCommand.Parameters.Add(sqlParameter);
					}
					sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SequentialAccess);
					ProfileInfoCollection profileInfoCollection = new ProfileInfoCollection();
					while (sqlDataReader.Read())
					{
						string @string = sqlDataReader.GetString(0);
						bool boolean = sqlDataReader.GetBoolean(1);
						DateTime dateTime = DateTime.SpecifyKind(sqlDataReader.GetDateTime(2), DateTimeKind.Utc);
						DateTime dateTime2 = DateTime.SpecifyKind(sqlDataReader.GetDateTime(3), DateTimeKind.Utc);
						int @int = sqlDataReader.GetInt32(4);
						profileInfoCollection.Add(new ProfileInfo(@string, boolean, dateTime, dateTime2, @int));
					}
					totalRecords = profileInfoCollection.Count;
					if (sqlDataReader.NextResult() && sqlDataReader.Read())
					{
						totalRecords = sqlDataReader.GetInt32(0);
					}
					profileInfoCollection2 = profileInfoCollection;
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
			return profileInfoCollection2;
		}

		// Token: 0x04001DD3 RID: 7635
		private string _AppName;

		// Token: 0x04001DD4 RID: 7636
		private string _sqlConnectionString;

		// Token: 0x04001DD5 RID: 7637
		private int _SchemaVersionCheck;

		// Token: 0x04001DD6 RID: 7638
		private int _CommandTimeout;
	}
}
