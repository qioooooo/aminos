using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Web.Util;

namespace System.Web.Management
{
	// Token: 0x020002D0 RID: 720
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.High)]
	public static class SqlServices
	{
		// Token: 0x060024CA RID: 9418 RVA: 0x0009D4D0 File Offset: 0x0009C4D0
		public static void Install(string server, string user, string password, string database, SqlFeatures features)
		{
			SqlServices.SetupApplicationServices(server, user, password, false, null, database, null, features, true);
		}

		// Token: 0x060024CB RID: 9419 RVA: 0x0009D4EC File Offset: 0x0009C4EC
		public static void Install(string server, string database, SqlFeatures features)
		{
			SqlServices.SetupApplicationServices(server, null, null, true, null, database, null, features, true);
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x0009D508 File Offset: 0x0009C508
		internal static void Install(string database, string dbFileName, string connectionString)
		{
			SqlServices.SetupApplicationServices(null, null, null, false, connectionString, database, dbFileName, SqlFeatures.All, true);
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x0009D528 File Offset: 0x0009C528
		public static void Install(string database, SqlFeatures features, string connectionString)
		{
			SqlServices.SetupApplicationServices(null, null, null, true, connectionString, database, null, features, true);
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x0009D544 File Offset: 0x0009C544
		public static void Uninstall(string server, string user, string password, string database, SqlFeatures features)
		{
			SqlServices.SetupApplicationServices(server, user, password, false, null, database, null, features, false);
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x0009D560 File Offset: 0x0009C560
		public static void Uninstall(string server, string database, SqlFeatures features)
		{
			SqlServices.SetupApplicationServices(server, null, null, true, null, database, null, features, false);
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x0009D57C File Offset: 0x0009C57C
		public static void Uninstall(string database, SqlFeatures features, string connectionString)
		{
			SqlServices.SetupApplicationServices(null, null, null, true, connectionString, database, null, features, false);
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x0009D597 File Offset: 0x0009C597
		public static void InstallSessionState(string server, string user, string password, string customDatabase, SessionStateType type)
		{
			SqlServices.SetupSessionState(server, user, password, false, null, customDatabase, type, true);
		}

		// Token: 0x060024D2 RID: 9426 RVA: 0x0009D5A7 File Offset: 0x0009C5A7
		public static void InstallSessionState(string server, string customDatabase, SessionStateType type)
		{
			SqlServices.SetupSessionState(server, null, null, true, null, customDatabase, type, true);
		}

		// Token: 0x060024D3 RID: 9427 RVA: 0x0009D5B6 File Offset: 0x0009C5B6
		public static void InstallSessionState(string customDatabase, SessionStateType type, string connectionString)
		{
			SqlServices.SetupSessionState(null, null, null, true, connectionString, customDatabase, type, true);
		}

		// Token: 0x060024D4 RID: 9428 RVA: 0x0009D5C5 File Offset: 0x0009C5C5
		public static void UninstallSessionState(string server, string user, string password, string customDatabase, SessionStateType type)
		{
			SqlServices.SetupSessionState(server, user, password, false, null, customDatabase, type, false);
		}

		// Token: 0x060024D5 RID: 9429 RVA: 0x0009D5D5 File Offset: 0x0009C5D5
		public static void UninstallSessionState(string server, string customDatabase, SessionStateType type)
		{
			SqlServices.SetupSessionState(server, null, null, true, null, customDatabase, type, false);
		}

		// Token: 0x060024D6 RID: 9430 RVA: 0x0009D5E4 File Offset: 0x0009C5E4
		public static void UninstallSessionState(string customDatabase, SessionStateType type, string connectionString)
		{
			SqlServices.SetupSessionState(null, null, null, true, connectionString, customDatabase, type, false);
		}

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x060024D7 RID: 9431 RVA: 0x0009D5F4 File Offset: 0x0009C5F4
		internal static ArrayList ApplicationServiceTables
		{
			get
			{
				ArrayList arrayList = new ArrayList();
				for (int i = 0; i < SqlServices.s_featureInfos.Length; i++)
				{
					arrayList.InsertRange(arrayList.Count, SqlServices.s_featureInfos[i]._tablesRemovedInUninstall);
				}
				return arrayList;
			}
		}

		// Token: 0x060024D8 RID: 9432 RVA: 0x0009D638 File Offset: 0x0009C638
		public static string GenerateSessionStateScripts(bool install, SessionStateType type, string customDatabase)
		{
			SqlServices.SessionStateParamCheck(type, ref customDatabase);
			string text = Path.Combine(HttpRuntime.AspInstallDirectory, install ? SqlServices.SESSION_STATE_INSTALL_FILE : SqlServices.SESSION_STATE_UNINSTALL_FILE);
			string text2 = File.ReadAllText(text);
			return SqlServices.FixContent(text2, customDatabase, null, true, type);
		}

		// Token: 0x060024D9 RID: 9433 RVA: 0x0009D678 File Offset: 0x0009C678
		private static ArrayList GetFiles(bool install, SqlFeatures features)
		{
			ArrayList arrayList = new ArrayList();
			bool flag = false;
			for (int i = 0; i < SqlServices.s_featureInfos.Length; i++)
			{
				string[] array = null;
				if ((SqlServices.s_featureInfos[i]._feature & features) == SqlServices.s_featureInfos[i]._feature)
				{
					if (install)
					{
						array = SqlServices.s_featureInfos[i]._installFiles;
					}
					else
					{
						array = SqlServices.s_featureInfos[i]._uninstallFiles;
					}
				}
				if (array != null)
				{
					foreach (string text in array)
					{
						if (text != null && (!(text == SqlServices.INSTALL_COMMON_SQL) || !flag))
						{
							arrayList.Add(text);
							if (!flag && text == SqlServices.INSTALL_COMMON_SQL)
							{
								flag = true;
							}
						}
					}
				}
			}
			return arrayList;
		}

		// Token: 0x060024DA RID: 9434 RVA: 0x0009D740 File Offset: 0x0009C740
		private static string FixContent(string content, string database, string dbFileName, bool sessionState, SessionStateType sessionStatetype)
		{
			if (database != null)
			{
				database = SqlServices.RemoveSquareBrackets(database);
			}
			if (sessionState)
			{
				if (sessionStatetype != SessionStateType.Temporary)
				{
					if (sessionStatetype == SessionStateType.Persisted)
					{
						content = content.Replace("'sstype_temp'", "'" + SqlServices.SSTYPE_PERSISTED + "'");
						content = content.Replace("[tempdb]", "[" + SqlServices.ASPSTATE_DB + "]");
					}
					else if (sessionStatetype == SessionStateType.Custom)
					{
						content = content.Replace("'sstype_temp'", "'" + SqlServices.SSTYPE_CUSTOM + "'");
						content = content.Replace("[tempdb]", "[" + database + "]");
						content = content.Replace("'ASPState'", "'" + database + "'");
						content = content.Replace("[ASPState]", "[" + database + "]");
					}
				}
			}
			else
			{
				content = content.Replace("'aspnetdb'", "'" + database.Replace("'", "''") + "'");
				content = content.Replace("[aspnetdb]", "[" + database + "]");
			}
			if (dbFileName != null)
			{
				if (dbFileName.Contains("[") || dbFileName.Contains("]") || dbFileName.Contains("'"))
				{
					throw new ArgumentException(SR.GetString("DbFileName_can_not_contain_invalid_chars"));
				}
				database = database.TrimStart(new char[] { '[' });
				database = database.TrimEnd(new char[] { ']' });
				string text = database + "_DAT";
				if (!char.IsLetter(text[0]))
				{
					text = "A" + text;
				}
				string text2 = string.Concat(new string[] { "ON ( NAME = ", text, ", FILENAME = ''", dbFileName, "'', SIZE = 10MB, FILEGROWTH = 5MB )" });
				content = content.Replace("SET @dboptions = N'/**/'", "SET @dboptions = N'" + text2 + "'");
			}
			return content;
		}

		// Token: 0x060024DB RID: 9435 RVA: 0x0009D95C File Offset: 0x0009C95C
		private static void ExecuteSessionFile(string file, string server, string database, string dbFileName, SqlConnection connection, bool isInstall, SessionStateType sessionStatetype)
		{
			SqlServices.ExecuteFile(file, server, database, dbFileName, connection, true, isInstall, sessionStatetype);
		}

		// Token: 0x060024DC RID: 9436 RVA: 0x0009D970 File Offset: 0x0009C970
		private static void ExecuteFile(string file, string server, string database, string dbFileName, SqlConnection connection, bool sessionState, bool isInstall, SessionStateType sessionStatetype)
		{
			string text = Path.Combine(HttpRuntime.AspInstallDirectory, file);
			string text2 = File.ReadAllText(text);
			string text3 = null;
			if (file.Equals(SqlServices.INSTALL_COMMON_SQL))
			{
				text2 = SqlServices.FixContent(text2, database, dbFileName, sessionState, sessionStatetype);
			}
			else
			{
				text2 = SqlServices.FixContent(text2, database, null, sessionState, sessionStatetype);
			}
			StringReader stringReader = new StringReader(text2);
			SqlCommand sqlCommand = new SqlCommand(null, connection);
			string text4;
			do
			{
				bool flag = false;
				text4 = stringReader.ReadLine();
				if (text4 == null)
				{
					flag = true;
				}
				else if (StringUtil.EqualsIgnoreCase(text4.Trim(), "GO"))
				{
					flag = true;
				}
				else
				{
					if (text3 != null)
					{
						text3 += "\n";
					}
					text3 += text4;
				}
				if (flag & (text3 != null))
				{
					sqlCommand.CommandText = text3;
					try
					{
						sqlCommand.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						SqlException ex2 = ex as SqlException;
						if (ex2 != null)
						{
							int num = -1;
							if (text3.IndexOf("sp_add_category", StringComparison.Ordinal) > -1)
							{
								num = 14261;
							}
							else if (text3.IndexOf("sp_delete_job", StringComparison.Ordinal) > -1)
							{
								num = 14262;
								if (sessionState && !isInstall)
								{
									throw new SqlExecutionException(SR.GetString("SQL_Services_Error_Deleting_Session_Job"), server, database, file, text3, ex2);
								}
							}
							if (ex2.Number != num)
							{
								throw new SqlExecutionException(SR.GetString("SQL_Services_Error_Executing_Command", new object[]
								{
									file,
									ex2.Number.ToString(CultureInfo.CurrentCulture),
									ex2.Message
								}), server, database, file, text3, ex2);
							}
						}
					}
					catch
					{
						throw;
					}
					text3 = null;
				}
			}
			while (text4 != null);
		}

		// Token: 0x060024DD RID: 9437 RVA: 0x0009DB14 File Offset: 0x0009CB14
		private static void ApplicationServicesParamCheck(SqlFeatures features, ref string database)
		{
			if (features == SqlFeatures.None)
			{
				return;
			}
			if ((features & SqlFeatures.All) != features)
			{
				throw new ArgumentException(SR.GetString("SQL_Services_Invalid_Feature"));
			}
			SqlServices.CheckDatabaseName(ref database);
		}

		// Token: 0x060024DE RID: 9438 RVA: 0x0009DB3C File Offset: 0x0009CB3C
		private static void CheckDatabaseName(ref string database)
		{
			if (database != null)
			{
				database = database.TrimEnd(new char[0]);
				if (database.Length == 0)
				{
					throw new ArgumentException(SR.GetString("SQL_Services_Database_Empty_Or_Space_Only_Arg"));
				}
				database = SqlServices.RemoveSquareBrackets(database);
				if (database.Contains("'") || database.Contains("[") || database.Contains("]"))
				{
					throw new ArgumentException(SR.GetString("SQL_Services_Database_contains_invalid_chars"));
				}
			}
			if (database == null)
			{
				database = SqlServices.DEFAULT_DB;
				return;
			}
			database = "[" + database + "]";
		}

		// Token: 0x060024DF RID: 9439 RVA: 0x0009DBD8 File Offset: 0x0009CBD8
		public static string GenerateApplicationServicesScripts(bool install, SqlFeatures features, string database)
		{
			StringBuilder stringBuilder = new StringBuilder();
			SqlServices.ApplicationServicesParamCheck(features, ref database);
			ArrayList files = SqlServices.GetFiles(install, features);
			foreach (object obj in files)
			{
				string text = (string)obj;
				string text2 = Path.Combine(HttpRuntime.AspInstallDirectory, text);
				string text3 = File.ReadAllText(text2);
				stringBuilder.Append(SqlServices.FixContent(text3, database, null, false, SessionStateType.Temporary));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060024E0 RID: 9440 RVA: 0x0009DC70 File Offset: 0x0009CC70
		private static string RemoveSquareBrackets(string database)
		{
			if (database != null && StringUtil.StringStartsWith(database, '[') && StringUtil.StringEndsWith(database, ']'))
			{
				return database.Substring(1, database.Length - 2);
			}
			return database;
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x0009DC9C File Offset: 0x0009CC9C
		private static void EnsureDatabaseExists(string database, SqlConnection sqlConnection)
		{
			string text = SqlServices.RemoveSquareBrackets(database);
			object obj = new SqlCommand("SELECT DB_ID(@database)", sqlConnection)
			{
				Parameters = 
				{
					new SqlParameter("@database", text)
				}
			}.ExecuteScalar();
			if (obj == null || obj == DBNull.Value)
			{
				throw new HttpException(SR.GetString("SQL_Services_Error_Cant_Uninstall_Nonexisting_Database", new object[] { text }));
			}
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x0009DD04 File Offset: 0x0009CD04
		private static void SetupApplicationServices(string server, string user, string password, bool trusted, string connectionString, string database, string dbFileName, SqlFeatures features, bool install)
		{
			SqlConnection sqlConnection = null;
			SqlServices.ApplicationServicesParamCheck(features, ref database);
			ArrayList files = SqlServices.GetFiles(install, features);
			try
			{
				sqlConnection = SqlServices.GetSqlConnection(server, user, password, trusted, connectionString);
				if (!install)
				{
					SqlServices.EnsureDatabaseExists(database, sqlConnection);
					string text = SqlServices.RemoveSquareBrackets(database);
					if (sqlConnection.Database != text)
					{
						sqlConnection.ChangeDatabase(text);
					}
					int num = 0;
					for (int i = 0; i < SqlServices.s_featureInfos.Length; i++)
					{
						if ((SqlServices.s_featureInfos[i]._feature & features) == SqlServices.s_featureInfos[i]._feature)
						{
							num |= SqlServices.s_featureInfos[i]._dataCheckBitMask;
						}
					}
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_AnyDataInTables", sqlConnection);
					sqlCommand.Parameters.Add(new SqlParameter("@TablesToCheck", num));
					sqlCommand.CommandType = CommandType.StoredProcedure;
					string text2 = null;
					try
					{
						text2 = sqlCommand.ExecuteScalar() as string;
					}
					catch (SqlException ex)
					{
						if (ex.Number != 2812)
						{
							throw;
						}
					}
					if (!string.IsNullOrEmpty(text2))
					{
						throw new NotSupportedException(SR.GetString("SQL_Services_Error_Cant_Uninstall_Nonempty_Table", new object[] { text2, database }));
					}
				}
				foreach (object obj in files)
				{
					string text3 = (string)obj;
					SqlServices.ExecuteFile(text3, server, database, dbFileName, sqlConnection, false, false, SessionStateType.Temporary);
				}
			}
			finally
			{
				if (sqlConnection != null)
				{
					try
					{
						sqlConnection.Close();
					}
					catch
					{
					}
					finally
					{
						sqlConnection = null;
					}
				}
			}
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x0009DF14 File Offset: 0x0009CF14
		private static void SessionStateParamCheck(SessionStateType type, ref string customDatabase)
		{
			if (type == SessionStateType.Custom && string.IsNullOrEmpty(customDatabase))
			{
				throw new ArgumentException(SR.GetString("SQL_Services_Error_missing_custom_database"), "customDatabase");
			}
			if (type != SessionStateType.Custom && customDatabase != null)
			{
				throw new ArgumentException(SR.GetString("SQL_Services_Error_Cant_use_custom_database"), "customDatabase");
			}
			SqlServices.CheckDatabaseName(ref customDatabase);
		}

		// Token: 0x060024E4 RID: 9444 RVA: 0x0009DF68 File Offset: 0x0009CF68
		private static void SetupSessionState(string server, string user, string password, bool trusted, string connectionString, string customDatabase, SessionStateType type, bool install)
		{
			SqlConnection sqlConnection = null;
			SqlServices.SessionStateParamCheck(type, ref customDatabase);
			try
			{
				sqlConnection = SqlServices.GetSqlConnection(server, user, password, trusted, connectionString);
				if (!install && type == SessionStateType.Custom)
				{
					SqlServices.EnsureDatabaseExists(customDatabase, sqlConnection);
				}
				SqlServices.ExecuteSessionFile(install ? SqlServices.SESSION_STATE_INSTALL_FILE : SqlServices.SESSION_STATE_UNINSTALL_FILE, server, customDatabase, null, sqlConnection, install, type);
			}
			finally
			{
				if (sqlConnection != null)
				{
					try
					{
						sqlConnection.Close();
					}
					catch
					{
					}
					finally
					{
						sqlConnection = null;
					}
				}
			}
		}

		// Token: 0x060024E5 RID: 9445 RVA: 0x0009DFFC File Offset: 0x0009CFFC
		private static string ConstructConnectionString(string server, string user, string password, bool trusted)
		{
			string text = null;
			if (string.IsNullOrEmpty(server))
			{
				throw ExceptionUtil.ParameterNullOrEmpty("server");
			}
			text = text + "server=" + server;
			if (trusted)
			{
				text += ";Trusted_Connection=true;";
			}
			else
			{
				if (string.IsNullOrEmpty(user))
				{
					throw ExceptionUtil.ParameterNullOrEmpty("user");
				}
				string text2 = text;
				text = string.Concat(new string[] { text2, ";UID=", user, ";PWD=", password, ";" });
			}
			return text;
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x0009E084 File Offset: 0x0009D084
		private static SqlConnection GetSqlConnection(string server, string user, string password, bool trusted, string connectionString)
		{
			if (connectionString == null)
			{
				connectionString = SqlServices.ConstructConnectionString(server, user, password, trusted);
			}
			SqlConnection sqlConnection;
			try
			{
				sqlConnection = new SqlConnection(connectionString);
				sqlConnection.Open();
			}
			catch (Exception ex)
			{
				sqlConnection = null;
				throw new HttpException(SR.GetString("SQL_Services_Cant_connect_sql_database"), ex);
			}
			return sqlConnection;
		}

		// Token: 0x04001C90 RID: 7312
		private static string INSTALL_COMMON_SQL = "InstallCommon.sql";

		// Token: 0x04001C91 RID: 7313
		private static SqlServices.FeatureInfo[] s_featureInfos = new SqlServices.FeatureInfo[]
		{
			new SqlServices.FeatureInfo(SqlFeatures.Membership, new string[]
			{
				SqlServices.INSTALL_COMMON_SQL,
				"InstallMembership.sql"
			}, new string[] { "UninstallMembership.sql" }, new string[] { "aspnet_Membership" }, 1),
			new SqlServices.FeatureInfo(SqlFeatures.Profile, new string[]
			{
				SqlServices.INSTALL_COMMON_SQL,
				"InstallProfile.sql"
			}, new string[] { "UninstallProfile.sql" }, new string[] { "aspnet_Profile" }, 4),
			new SqlServices.FeatureInfo(SqlFeatures.RoleManager, new string[]
			{
				SqlServices.INSTALL_COMMON_SQL,
				"InstallRoles.sql"
			}, new string[] { "UninstallRoles.sql" }, new string[] { "aspnet_Roles", "aspnet_UsersInRoles" }, 2),
			new SqlServices.FeatureInfo(SqlFeatures.Personalization, new string[]
			{
				SqlServices.INSTALL_COMMON_SQL,
				"InstallPersonalization.sql"
			}, new string[] { "UninstallPersonalization.sql" }, new string[] { "aspnet_PersonalizationPerUser", "aspnet_Paths", "aspnet_PersonalizationAllUsers" }, 8),
			new SqlServices.FeatureInfo(SqlFeatures.SqlWebEventProvider, new string[]
			{
				SqlServices.INSTALL_COMMON_SQL,
				"InstallWebEventSqlProvider.sql"
			}, new string[] { "UninstallWebEventSqlProvider.sql" }, new string[] { "aspnet_WebEvent_Events" }, 16),
			new SqlServices.FeatureInfo(SqlFeatures.All, new string[0], new string[] { "UninstallCommon.sql" }, new string[] { "aspnet_Applications", "aspnet_Users", "aspnet_SchemaVersions" }, int.MaxValue)
		};

		// Token: 0x04001C92 RID: 7314
		private static string DEFAULT_DB = "aspnetdb";

		// Token: 0x04001C93 RID: 7315
		private static string ASPSTATE_DB = "ASPState";

		// Token: 0x04001C94 RID: 7316
		private static string SSTYPE_PERSISTED = "sstype_persisted";

		// Token: 0x04001C95 RID: 7317
		private static string SSTYPE_CUSTOM = "sstype_custom";

		// Token: 0x04001C96 RID: 7318
		private static string SESSION_STATE_INSTALL_FILE = "InstallSqlState.sql";

		// Token: 0x04001C97 RID: 7319
		private static string SESSION_STATE_UNINSTALL_FILE = "UninstallSqlState.sql";

		// Token: 0x020002D1 RID: 721
		internal struct FeatureInfo
		{
			// Token: 0x060024E8 RID: 9448 RVA: 0x0009E34D File Offset: 0x0009D34D
			internal FeatureInfo(SqlFeatures feature, string[] installFiles, string[] uninstallFiles, string[] tablesRemovedInUninstall, int dataCheckBitMask)
			{
				this._feature = feature;
				this._installFiles = installFiles;
				this._uninstallFiles = uninstallFiles;
				this._tablesRemovedInUninstall = tablesRemovedInUninstall;
				this._dataCheckBitMask = dataCheckBitMask;
			}

			// Token: 0x04001C98 RID: 7320
			internal SqlFeatures _feature;

			// Token: 0x04001C99 RID: 7321
			internal string[] _installFiles;

			// Token: 0x04001C9A RID: 7322
			internal string[] _uninstallFiles;

			// Token: 0x04001C9B RID: 7323
			internal string[] _tablesRemovedInUninstall;

			// Token: 0x04001C9C RID: 7324
			internal int _dataCheckBitMask;
		}
	}
}
