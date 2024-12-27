using System;
using System.Collections;

namespace System.Data.OracleClient
{
	// Token: 0x02000015 RID: 21
	internal sealed class DbSqlParserColumnCollection : CollectionBase
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00056C8C File Offset: 0x0005608C
		private Type ItemType
		{
			get
			{
				return typeof(DbSqlParserColumn);
			}
		}

		// Token: 0x1700001C RID: 28
		internal DbSqlParserColumn this[int i]
		{
			get
			{
				return (DbSqlParserColumn)base.InnerList[i];
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00056CC4 File Offset: 0x000560C4
		internal DbSqlParserColumn Add(DbSqlParserColumn value)
		{
			this.OnValidate(value);
			base.InnerList.Add(value);
			return value;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00056CE8 File Offset: 0x000560E8
		internal DbSqlParserColumn Add(string databaseName, string schemaName, string tableName, string columnName, string alias)
		{
			DbSqlParserColumn dbSqlParserColumn = new DbSqlParserColumn(databaseName, schemaName, tableName, columnName, alias);
			return this.Add(dbSqlParserColumn);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00056D0C File Offset: 0x0005610C
		internal void Insert(int index, DbSqlParserColumn value)
		{
			base.InnerList.Insert(index, value);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00056D28 File Offset: 0x00056128
		protected override void OnValidate(object value)
		{
		}
	}
}
