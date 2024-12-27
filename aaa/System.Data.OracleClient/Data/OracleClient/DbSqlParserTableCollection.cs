using System;
using System.Collections;

namespace System.Data.OracleClient
{
	// Token: 0x02000017 RID: 23
	internal sealed class DbSqlParserTableCollection : CollectionBase
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00056E74 File Offset: 0x00056274
		private Type ItemType
		{
			get
			{
				return typeof(DbSqlParserTable);
			}
		}

		// Token: 0x17000023 RID: 35
		internal DbSqlParserTable this[int i]
		{
			get
			{
				return (DbSqlParserTable)base.InnerList[i];
			}
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00056EAC File Offset: 0x000562AC
		internal DbSqlParserTable Add(DbSqlParserTable value)
		{
			this.OnValidate(value);
			base.InnerList.Add(value);
			return value;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00056ED0 File Offset: 0x000562D0
		internal DbSqlParserTable Add(string databaseName, string schemaName, string tableName, string correlationName)
		{
			DbSqlParserTable dbSqlParserTable = new DbSqlParserTable(databaseName, schemaName, tableName, correlationName);
			return this.Add(dbSqlParserTable);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00056EF0 File Offset: 0x000562F0
		protected override void OnValidate(object value)
		{
		}
	}
}
