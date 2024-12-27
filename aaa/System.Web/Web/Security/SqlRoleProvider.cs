using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Security.Permissions;
using System.Web.DataAccess;
using System.Web.Util;

namespace System.Web.Security
{
	// Token: 0x02000356 RID: 854
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class SqlRoleProvider : RoleProvider
	{
		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x0600299C RID: 10652 RVA: 0x000B8EE8 File Offset: 0x000B7EE8
		private int CommandTimeout
		{
			get
			{
				return this._CommandTimeout;
			}
		}

		// Token: 0x0600299D RID: 10653 RVA: 0x000B8EF0 File Offset: 0x000B7EF0
		public override void Initialize(string name, NameValueCollection config)
		{
			HttpRuntime.CheckAspNetHostingPermission(AspNetHostingPermissionLevel.Low, "Feature_not_supported_at_this_level");
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}
			if (string.IsNullOrEmpty(name))
			{
				name = "SqlRoleProvider";
			}
			if (string.IsNullOrEmpty(config["description"]))
			{
				config.Remove("description");
				config.Add("description", SR.GetString("RoleSqlProvider_description"));
			}
			base.Initialize(name, config);
			this._SchemaVersionCheck = 0;
			this._CommandTimeout = SecUtility.GetIntValue(config, "commandTimeout", 30, true, 0);
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
			config.Remove("connectionStringName");
			config.Remove("applicationName");
			config.Remove("commandTimeout");
			if (config.Count > 0)
			{
				string key = config.GetKey(0);
				if (!string.IsNullOrEmpty(key))
				{
					throw new ProviderException(SR.GetString("Provider_unrecognized_attribute", new object[] { key }));
				}
			}
		}

		// Token: 0x0600299E RID: 10654 RVA: 0x000B9088 File Offset: 0x000B8088
		private void CheckSchemaVersion(SqlConnection connection)
		{
			string[] array = new string[] { "Role Manager" };
			string text = "1";
			SecUtility.CheckSchemaVersion(this, connection, array, text, ref this._SchemaVersionCheck);
		}

		// Token: 0x0600299F RID: 10655 RVA: 0x000B90BC File Offset: 0x000B80BC
		public override bool IsUserInRole(string username, string roleName)
		{
			SecUtility.CheckParameter(ref roleName, true, true, true, 256, "roleName");
			SecUtility.CheckParameter(ref username, true, false, true, 256, "username");
			if (username.Length < 1)
			{
				return false;
			}
			bool flag;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_UsersInRoles_IsUserInRole", sqlConnectionHolder.Connection);
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandTimeout = this.CommandTimeout;
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
					sqlCommand.Parameters.Add(this.CreateInputParam("@RoleName", SqlDbType.NVarChar, roleName));
					sqlCommand.ExecuteNonQuery();
					switch (this.GetReturnValue(sqlCommand))
					{
					case 0:
						flag = false;
						break;
					case 1:
						flag = true;
						break;
					case 2:
						flag = false;
						break;
					case 3:
						flag = false;
						break;
					default:
						throw new ProviderException(SR.GetString("Provider_unknown_failure"));
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

		// Token: 0x060029A0 RID: 10656 RVA: 0x000B9244 File Offset: 0x000B8244
		public override string[] GetRolesForUser(string username)
		{
			SecUtility.CheckParameter(ref username, true, false, true, 256, "username");
			if (username.Length < 1)
			{
				return new string[0];
			}
			string[] array2;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_UsersInRoles_GetRolesForUser", sqlConnectionHolder.Connection);
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					SqlDataReader sqlDataReader = null;
					StringCollection stringCollection = new StringCollection();
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
					try
					{
						sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SequentialAccess);
						while (sqlDataReader.Read())
						{
							stringCollection.Add(sqlDataReader.GetString(0));
						}
					}
					catch
					{
						throw;
					}
					finally
					{
						if (sqlDataReader != null)
						{
							sqlDataReader.Close();
						}
					}
					if (stringCollection.Count > 0)
					{
						string[] array = new string[stringCollection.Count];
						stringCollection.CopyTo(array, 0);
						array2 = array;
					}
					else
					{
						switch (this.GetReturnValue(sqlCommand))
						{
						case 0:
							array2 = new string[0];
							break;
						case 1:
							array2 = new string[0];
							break;
						default:
							throw new ProviderException(SR.GetString("Provider_unknown_failure"));
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
			return array2;
		}

		// Token: 0x060029A1 RID: 10657 RVA: 0x000B9424 File Offset: 0x000B8424
		public override void CreateRole(string roleName)
		{
			SecUtility.CheckParameter(ref roleName, true, true, true, 256, "roleName");
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Roles_CreateRole", sqlConnectionHolder.Connection);
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandTimeout = this.CommandTimeout;
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@RoleName", SqlDbType.NVarChar, roleName));
					sqlCommand.ExecuteNonQuery();
					switch (this.GetReturnValue(sqlCommand))
					{
					case 0:
						break;
					case 1:
						throw new ProviderException(SR.GetString("Provider_role_already_exists", new object[] { roleName }));
					default:
						throw new ProviderException(SR.GetString("Provider_unknown_failure"));
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

		// Token: 0x060029A2 RID: 10658 RVA: 0x000B955C File Offset: 0x000B855C
		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			SecUtility.CheckParameter(ref roleName, true, true, true, 256, "roleName");
			bool flag;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Roles_DeleteRole", sqlConnectionHolder.Connection);
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandTimeout = this.CommandTimeout;
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@RoleName", SqlDbType.NVarChar, roleName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@DeleteOnlyIfRoleIsEmpty", SqlDbType.Bit, throwOnPopulatedRole ? 1 : 0));
					sqlCommand.ExecuteNonQuery();
					int returnValue = this.GetReturnValue(sqlCommand);
					if (returnValue == 2)
					{
						throw new ProviderException(SR.GetString("Role_is_not_empty"));
					}
					flag = returnValue == 0;
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

		// Token: 0x060029A3 RID: 10659 RVA: 0x000B9694 File Offset: 0x000B8694
		public override bool RoleExists(string roleName)
		{
			SecUtility.CheckParameter(ref roleName, true, true, true, 256, "roleName");
			bool flag;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Roles_RoleExists", sqlConnectionHolder.Connection);
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandTimeout = this.CommandTimeout;
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@RoleName", SqlDbType.NVarChar, roleName));
					sqlCommand.ExecuteNonQuery();
					switch (this.GetReturnValue(sqlCommand))
					{
					case 0:
						flag = false;
						break;
					case 1:
						flag = true;
						break;
					default:
						throw new ProviderException(SR.GetString("Provider_unknown_failure"));
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

		// Token: 0x060029A4 RID: 10660 RVA: 0x000B97B8 File Offset: 0x000B87B8
		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			SecUtility.CheckArrayParameter(ref roleNames, true, true, true, 256, "roleNames");
			SecUtility.CheckArrayParameter(ref usernames, true, true, true, 256, "usernames");
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
						int num = usernames.Length - i;
						while (num < usernames.Length && text.Length + usernames[num].Length + 1 < 4000)
						{
							text = text + "," + usernames[num];
							i--;
							num++;
						}
						int j = roleNames.Length;
						while (j > 0)
						{
							string text2 = roleNames[roleNames.Length - j];
							j--;
							num = roleNames.Length - j;
							while (num < roleNames.Length && text2.Length + roleNames[num].Length + 1 < 4000)
							{
								text2 = text2 + "," + roleNames[num];
								j--;
								num++;
							}
							if (!flag && (i > 0 || j > 0))
							{
								new SqlCommand("BEGIN TRANSACTION", sqlConnectionHolder.Connection).ExecuteNonQuery();
								flag = true;
							}
							this.AddUsersToRolesCore(sqlConnectionHolder.Connection, text, text2);
						}
					}
					if (flag)
					{
						new SqlCommand("COMMIT TRANSACTION", sqlConnectionHolder.Connection).ExecuteNonQuery();
						flag = false;
					}
				}
				catch
				{
					if (flag)
					{
						try
						{
							new SqlCommand("ROLLBACK TRANSACTION", sqlConnectionHolder.Connection).ExecuteNonQuery();
						}
						catch
						{
						}
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

		// Token: 0x060029A5 RID: 10661 RVA: 0x000B99B8 File Offset: 0x000B89B8
		private void AddUsersToRolesCore(SqlConnection conn, string usernames, string roleNames)
		{
			SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_UsersInRoles_AddUsersToRoles", conn);
			SqlDataReader sqlDataReader = null;
			SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
			string text = string.Empty;
			string text2 = string.Empty;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandTimeout = this.CommandTimeout;
			sqlParameter.Direction = ParameterDirection.ReturnValue;
			sqlCommand.Parameters.Add(sqlParameter);
			sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
			sqlCommand.Parameters.Add(this.CreateInputParam("@RoleNames", SqlDbType.NVarChar, roleNames));
			sqlCommand.Parameters.Add(this.CreateInputParam("@UserNames", SqlDbType.NVarChar, usernames));
			sqlCommand.Parameters.Add(this.CreateInputParam("@CurrentTimeUtc", SqlDbType.DateTime, DateTime.UtcNow));
			try
			{
				sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SingleRow);
				if (sqlDataReader.Read())
				{
					if (sqlDataReader.FieldCount > 0)
					{
						text = sqlDataReader.GetString(0);
					}
					if (sqlDataReader.FieldCount > 1)
					{
						text2 = sqlDataReader.GetString(1);
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
			switch (this.GetReturnValue(sqlCommand))
			{
			case 0:
				return;
			case 1:
				throw new ProviderException(SR.GetString("Provider_this_user_not_found", new object[] { text }));
			case 2:
				throw new ProviderException(SR.GetString("Provider_role_not_found", new object[] { text }));
			case 3:
				throw new ProviderException(SR.GetString("Provider_this_user_already_in_role", new object[] { text, text2 }));
			default:
				throw new ProviderException(SR.GetString("Provider_unknown_failure"));
			}
		}

		// Token: 0x060029A6 RID: 10662 RVA: 0x000B9B6C File Offset: 0x000B8B6C
		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			SecUtility.CheckArrayParameter(ref roleNames, true, true, true, 256, "roleNames");
			SecUtility.CheckArrayParameter(ref usernames, true, true, true, 256, "usernames");
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
						int num = usernames.Length - i;
						while (num < usernames.Length && text.Length + usernames[num].Length + 1 < 4000)
						{
							text = text + "," + usernames[num];
							i--;
							num++;
						}
						int j = roleNames.Length;
						while (j > 0)
						{
							string text2 = roleNames[roleNames.Length - j];
							j--;
							num = roleNames.Length - j;
							while (num < roleNames.Length && text2.Length + roleNames[num].Length + 1 < 4000)
							{
								text2 = text2 + "," + roleNames[num];
								j--;
								num++;
							}
							if (!flag && (i > 0 || j > 0))
							{
								new SqlCommand("BEGIN TRANSACTION", sqlConnectionHolder.Connection).ExecuteNonQuery();
								flag = true;
							}
							this.RemoveUsersFromRolesCore(sqlConnectionHolder.Connection, text, text2);
						}
					}
					if (flag)
					{
						new SqlCommand("COMMIT TRANSACTION", sqlConnectionHolder.Connection).ExecuteNonQuery();
						flag = false;
					}
				}
				catch
				{
					if (flag)
					{
						new SqlCommand("ROLLBACK TRANSACTION", sqlConnectionHolder.Connection).ExecuteNonQuery();
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

		// Token: 0x060029A7 RID: 10663 RVA: 0x000B9D50 File Offset: 0x000B8D50
		private void RemoveUsersFromRolesCore(SqlConnection conn, string usernames, string roleNames)
		{
			SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_UsersInRoles_RemoveUsersFromRoles", conn);
			SqlDataReader sqlDataReader = null;
			SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
			string text = string.Empty;
			string text2 = string.Empty;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandTimeout = this.CommandTimeout;
			sqlParameter.Direction = ParameterDirection.ReturnValue;
			sqlCommand.Parameters.Add(sqlParameter);
			sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
			sqlCommand.Parameters.Add(this.CreateInputParam("@UserNames", SqlDbType.NVarChar, usernames));
			sqlCommand.Parameters.Add(this.CreateInputParam("@RoleNames", SqlDbType.NVarChar, roleNames));
			try
			{
				sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SingleRow);
				if (sqlDataReader.Read())
				{
					if (sqlDataReader.FieldCount > 0)
					{
						text = sqlDataReader.GetString(0);
					}
					if (sqlDataReader.FieldCount > 1)
					{
						text2 = sqlDataReader.GetString(1);
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
			switch (this.GetReturnValue(sqlCommand))
			{
			case 0:
				return;
			case 1:
				throw new ProviderException(SR.GetString("Provider_this_user_not_found", new object[] { text }));
			case 2:
				throw new ProviderException(SR.GetString("Provider_role_not_found", new object[] { text2 }));
			case 3:
				throw new ProviderException(SR.GetString("Provider_this_user_already_not_in_role", new object[] { text, text2 }));
			default:
				throw new ProviderException(SR.GetString("Provider_unknown_failure"));
			}
		}

		// Token: 0x060029A8 RID: 10664 RVA: 0x000B9EE4 File Offset: 0x000B8EE4
		public override string[] GetUsersInRole(string roleName)
		{
			SecUtility.CheckParameter(ref roleName, true, true, true, 256, "roleName");
			string[] array;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_UsersInRoles_GetUsersInRoles", sqlConnectionHolder.Connection);
					SqlDataReader sqlDataReader = null;
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					StringCollection stringCollection = new StringCollection();
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@RoleName", SqlDbType.NVarChar, roleName));
					try
					{
						sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SequentialAccess);
						while (sqlDataReader.Read())
						{
							stringCollection.Add(sqlDataReader.GetString(0));
						}
					}
					catch
					{
						throw;
					}
					finally
					{
						if (sqlDataReader != null)
						{
							sqlDataReader.Close();
						}
					}
					if (stringCollection.Count < 1)
					{
						switch (this.GetReturnValue(sqlCommand))
						{
						case 0:
							array = new string[0];
							break;
						case 1:
							throw new ProviderException(SR.GetString("Provider_role_not_found", new object[] { roleName }));
						default:
							throw new ProviderException(SR.GetString("Provider_unknown_failure"));
						}
					}
					else
					{
						string[] array2 = new string[stringCollection.Count];
						stringCollection.CopyTo(array2, 0);
						array = array2;
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
			return array;
		}

		// Token: 0x060029A9 RID: 10665 RVA: 0x000BA0CC File Offset: 0x000B90CC
		public override string[] GetAllRoles()
		{
			string[] array2;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Roles_GetAllRoles", sqlConnectionHolder.Connection);
					StringCollection stringCollection = new StringCollection();
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					SqlDataReader sqlDataReader = null;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
					try
					{
						sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SequentialAccess);
						while (sqlDataReader.Read())
						{
							stringCollection.Add(sqlDataReader.GetString(0));
						}
					}
					catch
					{
						throw;
					}
					finally
					{
						if (sqlDataReader != null)
						{
							sqlDataReader.Close();
						}
					}
					string[] array = new string[stringCollection.Count];
					stringCollection.CopyTo(array, 0);
					array2 = array;
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
			return array2;
		}

		// Token: 0x060029AA RID: 10666 RVA: 0x000BA1F8 File Offset: 0x000B91F8
		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			SecUtility.CheckParameter(ref roleName, true, true, true, 256, "roleName");
			SecUtility.CheckParameter(ref usernameToMatch, true, true, false, 256, "usernameToMatch");
			string[] array;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					this.CheckSchemaVersion(sqlConnectionHolder.Connection);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_UsersInRoles_FindUsersInRole", sqlConnectionHolder.Connection);
					SqlDataReader sqlDataReader = null;
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					StringCollection stringCollection = new StringCollection();
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.Parameters.Add(this.CreateInputParam("@ApplicationName", SqlDbType.NVarChar, this.ApplicationName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@RoleName", SqlDbType.NVarChar, roleName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserNameToMatch", SqlDbType.NVarChar, usernameToMatch));
					try
					{
						sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SequentialAccess);
						while (sqlDataReader.Read())
						{
							stringCollection.Add(sqlDataReader.GetString(0));
						}
					}
					catch
					{
						throw;
					}
					finally
					{
						if (sqlDataReader != null)
						{
							sqlDataReader.Close();
						}
					}
					if (stringCollection.Count < 1)
					{
						switch (this.GetReturnValue(sqlCommand))
						{
						case 0:
							array = new string[0];
							break;
						case 1:
							throw new ProviderException(SR.GetString("Provider_role_not_found", new object[] { roleName }));
						default:
							throw new ProviderException(SR.GetString("Provider_unknown_failure"));
						}
					}
					else
					{
						string[] array2 = new string[stringCollection.Count];
						stringCollection.CopyTo(array2, 0);
						array = array2;
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
			return array;
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x060029AB RID: 10667 RVA: 0x000BA40C File Offset: 0x000B940C
		// (set) Token: 0x060029AC RID: 10668 RVA: 0x000BA414 File Offset: 0x000B9414
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

		// Token: 0x060029AD RID: 10669 RVA: 0x000BA440 File Offset: 0x000B9440
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

		// Token: 0x060029AE RID: 10670 RVA: 0x000BA468 File Offset: 0x000B9468
		private int GetReturnValue(SqlCommand cmd)
		{
			foreach (object obj in cmd.Parameters)
			{
				SqlParameter sqlParameter = (SqlParameter)obj;
				if (sqlParameter.Direction == ParameterDirection.ReturnValue && sqlParameter.Value != null && sqlParameter.Value is int)
				{
					return (int)sqlParameter.Value;
				}
			}
			return -1;
		}

		// Token: 0x04001F13 RID: 7955
		private string _AppName;

		// Token: 0x04001F14 RID: 7956
		private int _SchemaVersionCheck;

		// Token: 0x04001F15 RID: 7957
		private string _sqlConnectionString;

		// Token: 0x04001F16 RID: 7958
		private int _CommandTimeout;
	}
}
