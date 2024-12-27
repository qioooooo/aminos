using System;

namespace System.Data.OracleClient
{
	// Token: 0x02000013 RID: 19
	internal sealed class DbSqlParserColumn
	{
		// Token: 0x060000B8 RID: 184 RVA: 0x00056A94 File Offset: 0x00055E94
		internal DbSqlParserColumn(string databaseName, string schemaName, string tableName, string columnName, string alias)
		{
			this._databaseName = databaseName;
			this._schemaName = schemaName;
			this._tableName = tableName;
			this._columnName = columnName;
			this._alias = alias;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00056ACC File Offset: 0x00055ECC
		internal string ColumnName
		{
			get
			{
				if (this._columnName != null)
				{
					return this._columnName;
				}
				return string.Empty;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00056AF0 File Offset: 0x00055EF0
		internal string DatabaseName
		{
			get
			{
				if (this._databaseName != null)
				{
					return this._databaseName;
				}
				return string.Empty;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00056B14 File Offset: 0x00055F14
		internal bool IsAliased
		{
			get
			{
				return this._alias != null;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00056B30 File Offset: 0x00055F30
		internal bool IsExpression
		{
			get
			{
				return this._columnName == null;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00056B48 File Offset: 0x00055F48
		internal bool IsKey
		{
			get
			{
				return this._isKey;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00056B5C File Offset: 0x00055F5C
		internal bool IsUnique
		{
			get
			{
				return this._isUnique;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00056B70 File Offset: 0x00055F70
		internal string SchemaName
		{
			get
			{
				if (this._schemaName != null)
				{
					return this._schemaName;
				}
				return string.Empty;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00056B94 File Offset: 0x00055F94
		internal string TableName
		{
			get
			{
				if (this._tableName != null)
				{
					return this._tableName;
				}
				return string.Empty;
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00056BB8 File Offset: 0x00055FB8
		internal void CopySchemaInfoFrom(DbSqlParserColumn completedColumn)
		{
			this._databaseName = completedColumn.DatabaseName;
			this._schemaName = completedColumn.SchemaName;
			this._tableName = completedColumn.TableName;
			this._columnName = completedColumn.ColumnName;
			this._isKey = completedColumn.IsKey;
			this._isUnique = completedColumn.IsUnique;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00056C10 File Offset: 0x00056010
		internal void CopySchemaInfoFrom(DbSqlParserTable table)
		{
			this._databaseName = table.DatabaseName;
			this._schemaName = table.SchemaName;
			this._tableName = table.TableName;
			this._isKey = false;
			this._isUnique = false;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00056C50 File Offset: 0x00056050
		internal void SetConstraint(DbSqlParserColumn.ConstraintType constraintType)
		{
			switch (constraintType)
			{
			case DbSqlParserColumn.ConstraintType.PrimaryKey:
				this._isKey = true;
				return;
			case DbSqlParserColumn.ConstraintType.UniqueKey:
			case DbSqlParserColumn.ConstraintType.UniqueConstraint:
				this._isUnique = (this._isKey = true);
				return;
			default:
				return;
			}
		}

		// Token: 0x04000144 RID: 324
		private bool _isKey;

		// Token: 0x04000145 RID: 325
		private bool _isUnique;

		// Token: 0x04000146 RID: 326
		private string _databaseName;

		// Token: 0x04000147 RID: 327
		private string _schemaName;

		// Token: 0x04000148 RID: 328
		private string _tableName;

		// Token: 0x04000149 RID: 329
		private string _columnName;

		// Token: 0x0400014A RID: 330
		private string _alias;

		// Token: 0x02000014 RID: 20
		internal enum ConstraintType
		{
			// Token: 0x0400014C RID: 332
			PrimaryKey = 1,
			// Token: 0x0400014D RID: 333
			UniqueKey,
			// Token: 0x0400014E RID: 334
			UniqueConstraint
		}
	}
}
