using System;
using System.Collections;
using System.Data.SqlClient;
using System.Globalization;
using System.Security;
using System.Security.Permissions;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web.Caching
{
	// Token: 0x0200010E RID: 270
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class SqlCacheDependency : CacheDependency
	{
		// Token: 0x06000C8C RID: 3212 RVA: 0x00031D1C File Offset: 0x00030D1C
		public SqlCacheDependency(string databaseEntryName, string tableName)
			: base(0, null, new string[] { SqlCacheDependency.GetDependKey(databaseEntryName, tableName) })
		{
			this._sql7DatabaseState = SqlCacheDependencyManager.AddRef(databaseEntryName);
			this._sql7DepInfo._database = databaseEntryName;
			this._sql7DepInfo._table = tableName;
			object obj = HttpRuntime.CacheInternal[SqlCacheDependency.GetDependKey(databaseEntryName, tableName)];
			if (obj == null)
			{
				this._sql7ChangeId = -1;
			}
			else
			{
				this._sql7ChangeId = (int)obj;
			}
			base.FinishInit();
			this.InitUniqueID();
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x00031D9D File Offset: 0x00030D9D
		protected override void DependencyDispose()
		{
			if (this._sql7DatabaseState != null)
			{
				SqlCacheDependencyManager.Release(this._sql7DatabaseState);
			}
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x00031DB4 File Offset: 0x00030DB4
		public SqlCacheDependency(SqlCommand sqlCmd)
		{
			HttpContext httpContext = HttpContext.Current;
			if (sqlCmd == null)
			{
				throw new ArgumentNullException("sqlCmd");
			}
			if (httpContext != null && httpContext.SqlDependencyCookie != null && sqlCmd.NotificationAutoEnlist)
			{
				throw new HttpException(SR.GetString("SqlCacheDependency_OutputCache_Conflict"));
			}
			this.CreateSqlDep(sqlCmd);
			this.InitUniqueID();
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x00031E0C File Offset: 0x00030E0C
		private void InitUniqueID()
		{
			if (this._sqlYukonDep != null)
			{
				this._uniqueID = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
				return;
			}
			if (this._sql7ChangeId == -1)
			{
				this._uniqueID = null;
				return;
			}
			this._uniqueID = string.Concat(new string[]
			{
				this._sql7DepInfo._database,
				":",
				this._sql7DepInfo._table,
				":",
				this._sql7ChangeId.ToString(CultureInfo.InvariantCulture)
			});
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x00031EA2 File Offset: 0x00030EA2
		public override string GetUniqueID()
		{
			return this._uniqueID;
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x00031EAC File Offset: 0x00030EAC
		private static void CheckPermission()
		{
			if (!SqlCacheDependency.s_hasSqlClientPermissionInited)
			{
				if (!HostingEnvironment.IsHosted)
				{
					try
					{
						new SqlClientPermission(PermissionState.Unrestricted).Demand();
						SqlCacheDependency.s_hasSqlClientPermission = true;
						goto IL_002E;
					}
					catch (SecurityException)
					{
						goto IL_002E;
					}
				}
				SqlCacheDependency.s_hasSqlClientPermission = Permission.HasSqlClientPermission();
				IL_002E:
				SqlCacheDependency.s_hasSqlClientPermissionInited = true;
			}
			if (!SqlCacheDependency.s_hasSqlClientPermission)
			{
				throw new HttpException(SR.GetString("SqlCacheDependency_permission_denied"));
			}
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x00031F14 File Offset: 0x00030F14
		private void OnSQL9SqlDependencyChanged(object sender, SqlNotificationEventArgs e)
		{
			base.NotifyDependencyChanged(sender, e);
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x00031F1E File Offset: 0x00030F1E
		private SqlCacheDependency()
		{
			this.CreateSqlDep(null);
			this.InitUniqueID();
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x00031F33 File Offset: 0x00030F33
		private void CreateSqlDep(SqlCommand sqlCmd)
		{
			this._sqlYukonDep = new SqlDependency();
			if (sqlCmd != null)
			{
				this._sqlYukonDep.AddCommandDependency(sqlCmd);
			}
			this._sqlYukonDep.OnChange += this.OnSQL9SqlDependencyChanged;
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x00031F68 File Offset: 0x00030F68
		internal static void ValidateOutputCacheDependencyString(string depString, bool page)
		{
			if (depString == null)
			{
				throw new HttpException(SR.GetString("Invalid_sqlDependency_argument", new object[] { depString }));
			}
			if (StringUtil.EqualsIgnoreCase(depString, "CommandNotification"))
			{
				if (!page)
				{
					throw new HttpException(SR.GetString("Attrib_Sql9_not_allowed"));
				}
			}
			else
			{
				SqlCacheDependency.ParseSql7OutputCacheDependency(depString);
			}
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x00031FBC File Offset: 0x00030FBC
		internal static CacheDependency CreateOutputCacheDependency(string depString)
		{
			if (depString == null)
			{
				throw new HttpException(SR.GetString("Invalid_sqlDependency_argument", new object[] { depString }));
			}
			if (StringUtil.EqualsIgnoreCase(depString, "CommandNotification"))
			{
				HttpContext httpContext = HttpContext.Current;
				SqlCacheDependency sqlCacheDependency = new SqlCacheDependency();
				httpContext.SqlDependencyCookie = sqlCacheDependency._sqlYukonDep.Id;
				return sqlCacheDependency;
			}
			ArrayList arrayList = SqlCacheDependency.ParseSql7OutputCacheDependency(depString);
			if (arrayList.Count == 1)
			{
				SqlCacheDependency.Sql7DependencyInfo sql7DependencyInfo = (SqlCacheDependency.Sql7DependencyInfo)arrayList[0];
				return SqlCacheDependency.CreateSql7SqlCacheDependencyForOutputCache(sql7DependencyInfo._database, sql7DependencyInfo._table, depString);
			}
			AggregateCacheDependency aggregateCacheDependency = new AggregateCacheDependency();
			for (int i = 0; i < arrayList.Count; i++)
			{
				SqlCacheDependency.Sql7DependencyInfo sql7DependencyInfo = (SqlCacheDependency.Sql7DependencyInfo)arrayList[i];
				aggregateCacheDependency.Add(new CacheDependency[] { SqlCacheDependency.CreateSql7SqlCacheDependencyForOutputCache(sql7DependencyInfo._database, sql7DependencyInfo._table, depString) });
			}
			return aggregateCacheDependency;
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x000320A0 File Offset: 0x000310A0
		private static SqlCacheDependency CreateSql7SqlCacheDependencyForOutputCache(string database, string table, string depString)
		{
			SqlCacheDependency sqlCacheDependency;
			try
			{
				sqlCacheDependency = new SqlCacheDependency(database, table);
			}
			catch (HttpException ex)
			{
				HttpException ex2 = new HttpException(SR.GetString("Invalid_sqlDependency_argument2", new object[] { depString, ex.Message }), ex);
				ex2.SetFormatter(new UseLastUnhandledErrorFormatter(ex2));
				throw ex2;
			}
			return sqlCacheDependency;
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x00032100 File Offset: 0x00031100
		private static string GetDependKey(string database, string tableName)
		{
			SqlCacheDependency.CheckPermission();
			if (database == null)
			{
				throw new ArgumentNullException("database");
			}
			if (tableName == null)
			{
				throw new ArgumentNullException("tableName");
			}
			if (tableName.Length == 0)
			{
				throw new ArgumentException(SR.GetString("Cache_null_table"));
			}
			string moniterKey = SqlCacheDependencyManager.GetMoniterKey(database, tableName);
			SqlCacheDependencyManager.EnsureTableIsRegisteredAndPolled(database, tableName);
			return moniterKey;
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x00032158 File Offset: 0x00031158
		private static string VerifyAndRemoveEscapeCharacters(string s)
		{
			bool flag = false;
			for (int i = 0; i < s.Length; i++)
			{
				if (flag)
				{
					if (s[i] != '\\' && s[i] != ':' && s[i] != ';')
					{
						throw new ArgumentException();
					}
					flag = false;
				}
				else if (s[i] == '\\')
				{
					if (i + 1 == s.Length)
					{
						throw new ArgumentException();
					}
					flag = true;
					s = s.Remove(i, 1);
					i--;
				}
			}
			return s;
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x000321D4 File Offset: 0x000311D4
		internal static ArrayList ParseSql7OutputCacheDependency(string outputCacheString)
		{
			bool flag = false;
			int num = 0;
			int num2 = -1;
			string text = null;
			ArrayList arrayList = null;
			ArrayList arrayList2;
			try
			{
				for (int i = 0; i < outputCacheString.Length + 1; i++)
				{
					if (flag)
					{
						flag = false;
					}
					else if (i != outputCacheString.Length && outputCacheString[i] == '\\')
					{
						flag = true;
					}
					else
					{
						if (i == outputCacheString.Length || outputCacheString[i] == ';')
						{
							if (text == null)
							{
								throw new ArgumentException();
							}
							int num3 = i - num2;
							if (num3 == 0)
							{
								throw new ArgumentException();
							}
							SqlCacheDependency.Sql7DependencyInfo sql7DependencyInfo = default(SqlCacheDependency.Sql7DependencyInfo);
							sql7DependencyInfo._database = SqlCacheDependency.VerifyAndRemoveEscapeCharacters(text);
							sql7DependencyInfo._table = SqlCacheDependency.VerifyAndRemoveEscapeCharacters(outputCacheString.Substring(num2, num3));
							if (arrayList == null)
							{
								arrayList = new ArrayList(1);
							}
							arrayList.Add(sql7DependencyInfo);
							num = i + 1;
							text = null;
						}
						if (i == outputCacheString.Length)
						{
							break;
						}
						if (outputCacheString[i] == ':')
						{
							if (text != null)
							{
								throw new ArgumentException();
							}
							int num3 = i - num;
							if (num3 == 0)
							{
								throw new ArgumentException();
							}
							text = outputCacheString.Substring(num, num3);
							num2 = i + 1;
						}
					}
				}
				arrayList2 = arrayList;
			}
			catch (ArgumentException)
			{
				throw new ArgumentException(SR.GetString("Invalid_sqlDependency_argument", new object[] { outputCacheString }));
			}
			return arrayList2;
		}

		// Token: 0x0400144B RID: 5195
		private const string SQL9_CACHE_DEPENDENCY_DIRECTIVE = "CommandNotification";

		// Token: 0x0400144C RID: 5196
		internal const string SQL9_OUTPUT_CACHE_DEPENDENCY_COOKIE = "MS.SqlDependencyCookie";

		// Token: 0x0400144D RID: 5197
		internal static bool s_hasSqlClientPermission;

		// Token: 0x0400144E RID: 5198
		internal static bool s_hasSqlClientPermissionInited;

		// Token: 0x0400144F RID: 5199
		private SqlDependency _sqlYukonDep;

		// Token: 0x04001450 RID: 5200
		private DatabaseNotifState _sql7DatabaseState;

		// Token: 0x04001451 RID: 5201
		private string _uniqueID;

		// Token: 0x04001452 RID: 5202
		private SqlCacheDependency.Sql7DependencyInfo _sql7DepInfo;

		// Token: 0x04001453 RID: 5203
		private int _sql7ChangeId;

		// Token: 0x0200010F RID: 271
		private struct Sql7DependencyInfo
		{
			// Token: 0x04001454 RID: 5204
			internal string _database;

			// Token: 0x04001455 RID: 5205
			internal string _table;
		}
	}
}
