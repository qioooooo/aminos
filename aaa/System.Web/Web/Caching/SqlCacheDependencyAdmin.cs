using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.Caching
{
	// Token: 0x02000114 RID: 276
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.High)]
	public static class SqlCacheDependencyAdmin
	{
		// Token: 0x06000CB3 RID: 3251 RVA: 0x00032FD0 File Offset: 0x00031FD0
		internal static void SetupNotifications(int flags, string table, string connectionString)
		{
			SqlConnection sqlConnection = null;
			SqlCommand sqlCommand = null;
			bool flag = (flags & 9) != 0;
			bool flag2 = (flags & 2) != 0;
			if (flag)
			{
				bool flag3 = (flags & 8) != 0;
				if (table == null)
				{
					if (flag3)
					{
						throw new ArgumentException(SR.GetString("Cache_null_table_in_tables"), "tables");
					}
					throw new ArgumentNullException("table");
				}
				else if (table.Length == 0)
				{
					if (flag3)
					{
						throw new ArgumentException(SR.GetString("Cache_null_table_in_tables"), "tables");
					}
					throw new ArgumentException(SR.GetString("Cache_null_table"), "table");
				}
			}
			try
			{
				sqlConnection = new SqlConnection(connectionString);
				sqlConnection.Open();
				sqlCommand = new SqlCommand(null, sqlConnection);
				if (flag)
				{
					sqlCommand.CommandText = ((!flag2) ? "dbo.AspNet_SqlCacheRegisterTableStoredProcedure" : "dbo.AspNet_SqlCacheUnRegisterTableStoredProcedure");
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(new SqlParameter("@tableName", SqlDbType.NVarChar, table.Length));
					sqlCommand.Parameters[0].Value = table;
				}
				else if (!flag2)
				{
					sqlCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "/* Create notification table */ \nIF NOT EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = '{0}' AND type = 'U') \n   IF NOT EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = '{0}' AND type = 'U') \n      CREATE TABLE dbo.{0} (\n      tableName             NVARCHAR(450) NOT NULL PRIMARY KEY,\n      notificationCreated   DATETIME NOT NULL DEFAULT(GETDATE()),\n      changeId              INT NOT NULL DEFAULT(0)\n      )\n\n/* Create polling SP */\nIF NOT EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = '{1}' AND type = 'P') \n   IF NOT EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = '{1}' AND type = 'P') \n   EXEC('CREATE PROCEDURE dbo.{1} AS\n         SELECT tableName, changeId FROM dbo.{0}\n         RETURN 0')\n\n/* Create SP for registering a table. */ \nIF NOT EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = '{2}' AND type = 'P') \n   IF NOT EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = '{2}' AND type = 'P') \n   EXEC('CREATE PROCEDURE dbo.{2} \n             @tableName NVARCHAR(450) \n         AS\n         BEGIN\n\n         DECLARE @triggerName AS NVARCHAR(3000) \n         DECLARE @fullTriggerName AS NVARCHAR(3000)\n         DECLARE @canonTableName NVARCHAR(3000) \n         DECLARE @quotedTableName NVARCHAR(3000) \n\n         /* Create the trigger name */ \n         SET @triggerName = REPLACE(@tableName, ''['', ''__o__'') \n         SET @triggerName = REPLACE(@triggerName, '']'', ''__c__'') \n         SET @triggerName = @triggerName + ''{3}'' \n         SET @fullTriggerName = ''dbo.['' + @triggerName + '']'' \n\n         /* Create the cannonicalized table name for trigger creation */ \n         /* Do not touch it if the name contains other delimiters */ \n         IF (CHARINDEX(''.'', @tableName) <> 0 OR \n             CHARINDEX(''['', @tableName) <> 0 OR \n             CHARINDEX('']'', @tableName) <> 0) \n             SET @canonTableName = @tableName \n         ELSE \n             SET @canonTableName = ''['' + @tableName + '']'' \n\n         /* First make sure the table exists */ \n         IF (SELECT OBJECT_ID(@tableName, ''U'')) IS NULL \n         BEGIN \n             RAISERROR (''00000001'', 16, 1) \n             RETURN \n         END \n\n         BEGIN TRAN\n         /* Insert the value into the notification table */ \n         IF NOT EXISTS (SELECT tableName FROM dbo.{0} WITH (NOLOCK) WHERE tableName = @tableName) \n             IF NOT EXISTS (SELECT tableName FROM dbo.{0} WITH (TABLOCKX) WHERE tableName = @tableName) \n                 INSERT  dbo.{0} \n                 VALUES (@tableName, GETDATE(), 0)\n\n         /* Create the trigger */ \n         SET @quotedTableName = QUOTENAME(@tableName, '''''''') \n         IF NOT EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = @triggerName AND type = ''TR'') \n             IF NOT EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = @triggerName AND type = ''TR'') \n                 EXEC(''CREATE TRIGGER '' + @fullTriggerName + '' ON '' + @canonTableName +''\n                       FOR INSERT, UPDATE, DELETE AS BEGIN\n                       SET NOCOUNT ON\n                       EXEC dbo.{6} N'' + @quotedTableName + ''\n                       END\n                       '')\n         COMMIT TRAN\n         END\n   ')\n\n/* Create SP for updating the change Id of a table. */ \nIF NOT EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = '{6}' AND type = 'P') \n   IF NOT EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = '{6}' AND type = 'P') \n   EXEC('CREATE PROCEDURE dbo.{6} \n             @tableName NVARCHAR(450) \n         AS\n\n         BEGIN \n             UPDATE dbo.{0} WITH (ROWLOCK) SET changeId = changeId + 1 \n             WHERE tableName = @tableName\n         END\n   ')\n\n/* Create SP for unregistering a table. */ \nIF NOT EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = '{4}' AND type = 'P') \n   IF NOT EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = '{4}' AND type = 'P') \n   EXEC('CREATE PROCEDURE dbo.{4} \n             @tableName NVARCHAR(450) \n         AS\n         BEGIN\n\n         BEGIN TRAN\n         DECLARE @triggerName AS NVARCHAR(3000) \n         DECLARE @fullTriggerName AS NVARCHAR(3000)\n         SET @triggerName = REPLACE(@tableName, ''['', ''__o__'') \n         SET @triggerName = REPLACE(@triggerName, '']'', ''__c__'') \n         SET @triggerName = @triggerName + ''{3}'' \n         SET @fullTriggerName = ''dbo.['' + @triggerName + '']'' \n\n         /* Remove the table-row from the notification table */ \n         IF EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = ''{0}'' AND type = ''U'') \n             IF EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = ''{0}'' AND type = ''U'') \n             DELETE FROM dbo.{0} WHERE tableName = @tableName \n\n         /* Remove the trigger */ \n         IF EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = @triggerName AND type = ''TR'') \n             IF EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = @triggerName AND type = ''TR'') \n             EXEC(''DROP TRIGGER '' + @fullTriggerName) \n\n         COMMIT TRAN\n         END\n   ')\n\n/* Create SP for querying all registered table */ \nIF NOT EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = '{5}' AND type = 'P') \n   IF NOT EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = '{5}' AND type = 'P') \n   EXEC('CREATE PROCEDURE dbo.{5} \n         AS\n         SELECT tableName FROM dbo.{0}   ')\n\n/* Create roles and grant them access to SP  */ \nIF NOT EXISTS (SELECT name FROM sysusers WHERE issqlrole = 1 AND name = N'aspnet_ChangeNotification_ReceiveNotificationsOnlyAccess') \n    EXEC sp_addrole N'aspnet_ChangeNotification_ReceiveNotificationsOnlyAccess' \n\nGRANT EXECUTE ON dbo.{1} to aspnet_ChangeNotification_ReceiveNotificationsOnlyAccess\n\n", new object[] { "AspNet_SqlCacheTablesForChangeNotification", "AspNet_SqlCachePollingStoredProcedure", "AspNet_SqlCacheRegisterTableStoredProcedure", "_AspNet_SqlCacheNotification_Trigger", "AspNet_SqlCacheUnRegisterTableStoredProcedure", "AspNet_SqlCacheQueryRegisteredTablesStoredProcedure", "AspNet_SqlCacheUpdateChangeIdStoredProcedure" });
					sqlCommand.CommandType = CommandType.Text;
				}
				else
				{
					sqlCommand.CommandText = string.Format(CultureInfo.InvariantCulture, "/* Remove notification table */ \nIF EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = '{0}' AND type = 'U') \n    IF EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = '{0}' AND type = 'U') \n    BEGIN\n      /* First, unregister all registered tables */ \n      DECLARE tables_cursor CURSOR FOR \n      SELECT tableName FROM dbo.{0} \n      DECLARE @tableName AS NVARCHAR(450) \n\n      OPEN tables_cursor \n\n      /* Perform the first fetch. */ \n      FETCH NEXT FROM tables_cursor INTO @tableName \n\n      /* Check @@FETCH_STATUS to see if there are any more rows to fetch. */ \n      WHILE @@FETCH_STATUS = 0 \n      BEGIN \n          EXEC {3} @tableName \n\n          /* This is executed as long as the previous fetch succeeds. */ \n          FETCH NEXT FROM tables_cursor INTO @tableName \n      END \n      CLOSE tables_cursor \n      DEALLOCATE tables_cursor \n\n      /* Drop the table */\n      DROP TABLE dbo.{0} \n    END\n\n/* Remove polling SP */ \nIF EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = '{1}' AND type = 'P') \n    IF EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = '{1}' AND type = 'P') \n      DROP PROCEDURE dbo.{1} \n\n/* Remove SP that registers a table */ \nIF EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = '{2}' AND type = 'P') \n    IF EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = '{2}' AND type = 'P') \n      DROP PROCEDURE dbo.{2} \n\n/* Remove SP that unregisters a table */ \nIF EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = '{3}' AND type = 'P') \n    IF EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = '{3}' AND type = 'P') \n      DROP PROCEDURE dbo.{3} \n\n/* Remove SP that querys the registered table */ \nIF EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = '{4}' AND type = 'P') \n    IF EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = '{4}' AND type = 'P') \n      DROP PROCEDURE dbo.{4} \n\n/* Remove SP that updates the change Id of a table. */ \nIF EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = '{5}' AND type = 'P') \n    IF EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = '{5}' AND type = 'P') \n      DROP PROCEDURE dbo.{5} \n\n/* Drop roles */ \nIF EXISTS ( SELECT name FROM sysusers WHERE issqlrole = 1 AND name = 'aspnet_ChangeNotification_ReceiveNotificationsOnlyAccess') BEGIN\nCREATE TABLE #aspnet_RoleMembers \n( \n    Group_name      sysname, \n    Group_id        smallint, \n    Users_in_group  sysname, \n    User_id         smallint \n) \nINSERT INTO #aspnet_RoleMembers \nEXEC sp_helpuser 'aspnet_ChangeNotification_ReceiveNotificationsOnlyAccess' \n \nDECLARE @user_id smallint \nDECLARE @cmd nvarchar(500) \nDECLARE c1 CURSOR FORWARD_ONLY FOR  \n    SELECT User_id FROM #aspnet_RoleMembers \n  \nOPEN c1 \n  \nFETCH c1 INTO @user_id \nWHILE (@@fetch_status = 0)  \nBEGIN \n    SET @cmd = 'EXEC sp_droprolemember ''aspnet_ChangeNotification_ReceiveNotificationsOnlyAccess'',''' + USER_NAME(@user_id) + '''' \n    EXEC (@cmd) \n    FETCH c1 INTO @user_id \nEND \n \nclose c1 \ndeallocate c1 \n    EXEC sp_droprole 'aspnet_ChangeNotification_ReceiveNotificationsOnlyAccess'\nEND\n", new object[] { "AspNet_SqlCacheTablesForChangeNotification", "AspNet_SqlCachePollingStoredProcedure", "AspNet_SqlCacheRegisterTableStoredProcedure", "AspNet_SqlCacheUnRegisterTableStoredProcedure", "AspNet_SqlCacheQueryRegisteredTablesStoredProcedure", "AspNet_SqlCacheUpdateChangeIdStoredProcedure" });
					sqlCommand.CommandType = CommandType.Text;
				}
				sqlCommand.ExecuteNonQuery();
				sqlCommand.CommandText = string.Empty;
				if (HttpRuntime.IsAspNetAppDomain)
				{
					SqlCacheDependencyManager.UpdateAllDatabaseNotifState();
				}
			}
			catch (Exception ex)
			{
				SqlException ex2 = ex as SqlException;
				bool flag4 = true;
				if (ex2 != null)
				{
					if (ex2.Number == 2812)
					{
						if (!flag2)
						{
							if (table != null)
							{
								throw new DatabaseNotEnabledForNotificationException(SR.GetString("Database_not_enabled_for_notification", new object[] { sqlConnection.Database }));
							}
							throw;
						}
						else
						{
							if (table != null)
							{
								throw new DatabaseNotEnabledForNotificationException(SR.GetString("Cant_disable_table_sql_cache_dep"));
							}
							flag4 = false;
						}
					}
					else if (ex2.Number == 229 || ex2.Number == 262 || ex2.Number == 2760 || ex2.Number == 4613)
					{
						string text;
						if (!flag2)
						{
							if (table != null)
							{
								text = "Permission_denied_table_enable_notification";
							}
							else
							{
								text = "Permission_denied_database_enable_notification";
							}
						}
						else if (table != null)
						{
							text = "Permission_denied_table_disable_notification";
						}
						else
						{
							text = "Permission_denied_database_disable_notification";
						}
						if (table != null)
						{
							throw new HttpException(SR.GetString(text, new object[] { table }));
						}
						throw new HttpException(SR.GetString(text));
					}
					else if (ex2.Number == 50000 && ex2.Message == "00000001")
					{
						throw new HttpException(SR.GetString("Cache_dep_table_not_found", new object[] { table }));
					}
				}
				string text2;
				if (sqlCommand != null && sqlCommand.CommandText.Length != 0)
				{
					text2 = SR.GetString("Cant_connect_sql_cache_dep_database_admin_cmdtxt", new object[] { sqlCommand.CommandText });
				}
				else
				{
					text2 = SR.GetString("Cant_connect_sql_cache_dep_database_admin");
				}
				if (flag4)
				{
					throw new HttpException(text2, ex);
				}
			}
			finally
			{
				if (sqlConnection != null)
				{
					sqlConnection.Close();
				}
			}
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x0003337C File Offset: 0x0003237C
		public static void EnableNotifications(string connectionString)
		{
			SqlCacheDependencyAdmin.SetupNotifications(0, null, connectionString);
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x00033386 File Offset: 0x00032386
		public static void DisableNotifications(string connectionString)
		{
			SqlCacheDependencyAdmin.SetupNotifications(2, null, connectionString);
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x00033390 File Offset: 0x00032390
		public static void EnableTableForNotifications(string connectionString, string table)
		{
			SqlCacheDependencyAdmin.SetupNotifications(1, table, connectionString);
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x0003339C File Offset: 0x0003239C
		public static void EnableTableForNotifications(string connectionString, string[] tables)
		{
			if (tables == null)
			{
				throw new ArgumentNullException("tables");
			}
			foreach (string text in tables)
			{
				SqlCacheDependencyAdmin.SetupNotifications(8, text, connectionString);
			}
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x000333D3 File Offset: 0x000323D3
		public static void DisableTableForNotifications(string connectionString, string table)
		{
			SqlCacheDependencyAdmin.SetupNotifications(3, table, connectionString);
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x000333E0 File Offset: 0x000323E0
		public static void DisableTableForNotifications(string connectionString, string[] tables)
		{
			if (tables == null)
			{
				throw new ArgumentNullException("tables");
			}
			foreach (string text in tables)
			{
				SqlCacheDependencyAdmin.SetupNotifications(10, text, connectionString);
			}
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x00033418 File Offset: 0x00032418
		private static string[] GetEnabledTables(string connectionString)
		{
			SqlDataReader sqlDataReader = null;
			SqlConnection sqlConnection = null;
			ArrayList arrayList = new ArrayList();
			try
			{
				sqlConnection = new SqlConnection(connectionString);
				sqlConnection.Open();
				sqlDataReader = new SqlCommand("dbo.AspNet_SqlCacheQueryRegisteredTablesStoredProcedure", sqlConnection)
				{
					CommandType = CommandType.StoredProcedure
				}.ExecuteReader();
				while (sqlDataReader.Read())
				{
					arrayList.Add(sqlDataReader.GetString(0));
				}
			}
			catch (Exception ex)
			{
				SqlException ex2 = ex as SqlException;
				if (ex2 != null && ex2.Number == 2812)
				{
					throw new DatabaseNotEnabledForNotificationException(SR.GetString("Database_not_enabled_for_notification", new object[] { sqlConnection.Database }));
				}
				throw new HttpException(SR.GetString("Cant_get_enabled_tables_sql_cache_dep"), ex);
			}
			finally
			{
				try
				{
					if (sqlDataReader != null)
					{
						sqlDataReader.Close();
					}
					if (sqlConnection != null)
					{
						sqlConnection.Close();
					}
				}
				catch
				{
				}
			}
			return (string[])arrayList.ToArray(Type.GetType("System.String"));
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0003351C File Offset: 0x0003251C
		public static string[] GetTablesEnabledForNotifications(string connectionString)
		{
			return SqlCacheDependencyAdmin.GetEnabledTables(connectionString);
		}

		// Token: 0x04001479 RID: 5241
		internal const string SQL_CREATE_ENABLE_DATABASE_SP = "/* Create notification table */ \nIF NOT EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = '{0}' AND type = 'U') \n   IF NOT EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = '{0}' AND type = 'U') \n      CREATE TABLE dbo.{0} (\n      tableName             NVARCHAR(450) NOT NULL PRIMARY KEY,\n      notificationCreated   DATETIME NOT NULL DEFAULT(GETDATE()),\n      changeId              INT NOT NULL DEFAULT(0)\n      )\n\n/* Create polling SP */\nIF NOT EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = '{1}' AND type = 'P') \n   IF NOT EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = '{1}' AND type = 'P') \n   EXEC('CREATE PROCEDURE dbo.{1} AS\n         SELECT tableName, changeId FROM dbo.{0}\n         RETURN 0')\n\n/* Create SP for registering a table. */ \nIF NOT EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = '{2}' AND type = 'P') \n   IF NOT EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = '{2}' AND type = 'P') \n   EXEC('CREATE PROCEDURE dbo.{2} \n             @tableName NVARCHAR(450) \n         AS\n         BEGIN\n\n         DECLARE @triggerName AS NVARCHAR(3000) \n         DECLARE @fullTriggerName AS NVARCHAR(3000)\n         DECLARE @canonTableName NVARCHAR(3000) \n         DECLARE @quotedTableName NVARCHAR(3000) \n\n         /* Create the trigger name */ \n         SET @triggerName = REPLACE(@tableName, ''['', ''__o__'') \n         SET @triggerName = REPLACE(@triggerName, '']'', ''__c__'') \n         SET @triggerName = @triggerName + ''{3}'' \n         SET @fullTriggerName = ''dbo.['' + @triggerName + '']'' \n\n         /* Create the cannonicalized table name for trigger creation */ \n         /* Do not touch it if the name contains other delimiters */ \n         IF (CHARINDEX(''.'', @tableName) <> 0 OR \n             CHARINDEX(''['', @tableName) <> 0 OR \n             CHARINDEX('']'', @tableName) <> 0) \n             SET @canonTableName = @tableName \n         ELSE \n             SET @canonTableName = ''['' + @tableName + '']'' \n\n         /* First make sure the table exists */ \n         IF (SELECT OBJECT_ID(@tableName, ''U'')) IS NULL \n         BEGIN \n             RAISERROR (''00000001'', 16, 1) \n             RETURN \n         END \n\n         BEGIN TRAN\n         /* Insert the value into the notification table */ \n         IF NOT EXISTS (SELECT tableName FROM dbo.{0} WITH (NOLOCK) WHERE tableName = @tableName) \n             IF NOT EXISTS (SELECT tableName FROM dbo.{0} WITH (TABLOCKX) WHERE tableName = @tableName) \n                 INSERT  dbo.{0} \n                 VALUES (@tableName, GETDATE(), 0)\n\n         /* Create the trigger */ \n         SET @quotedTableName = QUOTENAME(@tableName, '''''''') \n         IF NOT EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = @triggerName AND type = ''TR'') \n             IF NOT EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = @triggerName AND type = ''TR'') \n                 EXEC(''CREATE TRIGGER '' + @fullTriggerName + '' ON '' + @canonTableName +''\n                       FOR INSERT, UPDATE, DELETE AS BEGIN\n                       SET NOCOUNT ON\n                       EXEC dbo.{6} N'' + @quotedTableName + ''\n                       END\n                       '')\n         COMMIT TRAN\n         END\n   ')\n\n/* Create SP for updating the change Id of a table. */ \nIF NOT EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = '{6}' AND type = 'P') \n   IF NOT EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = '{6}' AND type = 'P') \n   EXEC('CREATE PROCEDURE dbo.{6} \n             @tableName NVARCHAR(450) \n         AS\n\n         BEGIN \n             UPDATE dbo.{0} WITH (ROWLOCK) SET changeId = changeId + 1 \n             WHERE tableName = @tableName\n         END\n   ')\n\n/* Create SP for unregistering a table. */ \nIF NOT EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = '{4}' AND type = 'P') \n   IF NOT EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = '{4}' AND type = 'P') \n   EXEC('CREATE PROCEDURE dbo.{4} \n             @tableName NVARCHAR(450) \n         AS\n         BEGIN\n\n         BEGIN TRAN\n         DECLARE @triggerName AS NVARCHAR(3000) \n         DECLARE @fullTriggerName AS NVARCHAR(3000)\n         SET @triggerName = REPLACE(@tableName, ''['', ''__o__'') \n         SET @triggerName = REPLACE(@triggerName, '']'', ''__c__'') \n         SET @triggerName = @triggerName + ''{3}'' \n         SET @fullTriggerName = ''dbo.['' + @triggerName + '']'' \n\n         /* Remove the table-row from the notification table */ \n         IF EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = ''{0}'' AND type = ''U'') \n             IF EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = ''{0}'' AND type = ''U'') \n             DELETE FROM dbo.{0} WHERE tableName = @tableName \n\n         /* Remove the trigger */ \n         IF EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = @triggerName AND type = ''TR'') \n             IF EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = @triggerName AND type = ''TR'') \n             EXEC(''DROP TRIGGER '' + @fullTriggerName) \n\n         COMMIT TRAN\n         END\n   ')\n\n/* Create SP for querying all registered table */ \nIF NOT EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = '{5}' AND type = 'P') \n   IF NOT EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = '{5}' AND type = 'P') \n   EXEC('CREATE PROCEDURE dbo.{5} \n         AS\n         SELECT tableName FROM dbo.{0}   ')\n\n/* Create roles and grant them access to SP  */ \nIF NOT EXISTS (SELECT name FROM sysusers WHERE issqlrole = 1 AND name = N'aspnet_ChangeNotification_ReceiveNotificationsOnlyAccess') \n    EXEC sp_addrole N'aspnet_ChangeNotification_ReceiveNotificationsOnlyAccess' \n\nGRANT EXECUTE ON dbo.{1} to aspnet_ChangeNotification_ReceiveNotificationsOnlyAccess\n\n";

		// Token: 0x0400147A RID: 5242
		internal const string SQL_DISABLE_DATABASE = "/* Remove notification table */ \nIF EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = '{0}' AND type = 'U') \n    IF EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = '{0}' AND type = 'U') \n    BEGIN\n      /* First, unregister all registered tables */ \n      DECLARE tables_cursor CURSOR FOR \n      SELECT tableName FROM dbo.{0} \n      DECLARE @tableName AS NVARCHAR(450) \n\n      OPEN tables_cursor \n\n      /* Perform the first fetch. */ \n      FETCH NEXT FROM tables_cursor INTO @tableName \n\n      /* Check @@FETCH_STATUS to see if there are any more rows to fetch. */ \n      WHILE @@FETCH_STATUS = 0 \n      BEGIN \n          EXEC {3} @tableName \n\n          /* This is executed as long as the previous fetch succeeds. */ \n          FETCH NEXT FROM tables_cursor INTO @tableName \n      END \n      CLOSE tables_cursor \n      DEALLOCATE tables_cursor \n\n      /* Drop the table */\n      DROP TABLE dbo.{0} \n    END\n\n/* Remove polling SP */ \nIF EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = '{1}' AND type = 'P') \n    IF EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = '{1}' AND type = 'P') \n      DROP PROCEDURE dbo.{1} \n\n/* Remove SP that registers a table */ \nIF EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = '{2}' AND type = 'P') \n    IF EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = '{2}' AND type = 'P') \n      DROP PROCEDURE dbo.{2} \n\n/* Remove SP that unregisters a table */ \nIF EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = '{3}' AND type = 'P') \n    IF EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = '{3}' AND type = 'P') \n      DROP PROCEDURE dbo.{3} \n\n/* Remove SP that querys the registered table */ \nIF EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = '{4}' AND type = 'P') \n    IF EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = '{4}' AND type = 'P') \n      DROP PROCEDURE dbo.{4} \n\n/* Remove SP that updates the change Id of a table. */ \nIF EXISTS (SELECT name FROM sysobjects WITH (NOLOCK) WHERE name = '{5}' AND type = 'P') \n    IF EXISTS (SELECT name FROM sysobjects WITH (TABLOCKX) WHERE name = '{5}' AND type = 'P') \n      DROP PROCEDURE dbo.{5} \n\n/* Drop roles */ \nIF EXISTS ( SELECT name FROM sysusers WHERE issqlrole = 1 AND name = 'aspnet_ChangeNotification_ReceiveNotificationsOnlyAccess') BEGIN\nCREATE TABLE #aspnet_RoleMembers \n( \n    Group_name      sysname, \n    Group_id        smallint, \n    Users_in_group  sysname, \n    User_id         smallint \n) \nINSERT INTO #aspnet_RoleMembers \nEXEC sp_helpuser 'aspnet_ChangeNotification_ReceiveNotificationsOnlyAccess' \n \nDECLARE @user_id smallint \nDECLARE @cmd nvarchar(500) \nDECLARE c1 CURSOR FORWARD_ONLY FOR  \n    SELECT User_id FROM #aspnet_RoleMembers \n  \nOPEN c1 \n  \nFETCH c1 INTO @user_id \nWHILE (@@fetch_status = 0)  \nBEGIN \n    SET @cmd = 'EXEC sp_droprolemember ''aspnet_ChangeNotification_ReceiveNotificationsOnlyAccess'',''' + USER_NAME(@user_id) + '''' \n    EXEC (@cmd) \n    FETCH c1 INTO @user_id \nEND \n \nclose c1 \ndeallocate c1 \n    EXEC sp_droprole 'aspnet_ChangeNotification_ReceiveNotificationsOnlyAccess'\nEND\n";

		// Token: 0x0400147B RID: 5243
		internal const string DROP_MEMBERS = "CREATE TABLE #aspnet_RoleMembers \n( \n    Group_name      sysname, \n    Group_id        smallint, \n    Users_in_group  sysname, \n    User_id         smallint \n) \nINSERT INTO #aspnet_RoleMembers \nEXEC sp_helpuser 'aspnet_ChangeNotification_ReceiveNotificationsOnlyAccess' \n \nDECLARE @user_id smallint \nDECLARE @cmd nvarchar(500) \nDECLARE c1 CURSOR FORWARD_ONLY FOR  \n    SELECT User_id FROM #aspnet_RoleMembers \n  \nOPEN c1 \n  \nFETCH c1 INTO @user_id \nWHILE (@@fetch_status = 0)  \nBEGIN \n    SET @cmd = 'EXEC sp_droprolemember ''aspnet_ChangeNotification_ReceiveNotificationsOnlyAccess'',''' + USER_NAME(@user_id) + '''' \n    EXEC (@cmd) \n    FETCH c1 INTO @user_id \nEND \n \nclose c1 \ndeallocate c1 \n";

		// Token: 0x0400147C RID: 5244
		internal const string SQL_REGISTER_TABLE_SP = "AspNet_SqlCacheRegisterTableStoredProcedure";

		// Token: 0x0400147D RID: 5245
		internal const string SQL_REGISTER_TABLE_SP_DBO = "dbo.AspNet_SqlCacheRegisterTableStoredProcedure";

		// Token: 0x0400147E RID: 5246
		internal const string SQL_UNREGISTER_TABLE_SP = "AspNet_SqlCacheUnRegisterTableStoredProcedure";

		// Token: 0x0400147F RID: 5247
		internal const string SQL_UNREGISTER_TABLE_SP_DBO = "dbo.AspNet_SqlCacheUnRegisterTableStoredProcedure";

		// Token: 0x04001480 RID: 5248
		internal const string SQL_TRIGGER_NAME_POSTFIX = "_AspNet_SqlCacheNotification_Trigger";

		// Token: 0x04001481 RID: 5249
		internal const string SQL_QUERY_REGISTERED_TABLES_SP = "AspNet_SqlCacheQueryRegisteredTablesStoredProcedure";

		// Token: 0x04001482 RID: 5250
		internal const string SQL_QUERY_REGISTERED_TABLES_SP_DBO = "dbo.AspNet_SqlCacheQueryRegisteredTablesStoredProcedure";

		// Token: 0x04001483 RID: 5251
		internal const string SQL_UPDATE_CHANGE_ID_SP = "AspNet_SqlCacheUpdateChangeIdStoredProcedure";

		// Token: 0x04001484 RID: 5252
		private const int SETUP_TABLE = 1;

		// Token: 0x04001485 RID: 5253
		private const int SETUP_DISABLE = 2;

		// Token: 0x04001486 RID: 5254
		private const int SETUP_HTTPREQUEST = 4;

		// Token: 0x04001487 RID: 5255
		private const int SETUP_TABLES = 8;
	}
}
