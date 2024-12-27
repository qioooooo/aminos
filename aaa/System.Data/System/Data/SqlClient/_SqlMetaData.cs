using System;

namespace System.Data.SqlClient
{
	// Token: 0x02000329 RID: 809
	internal sealed class _SqlMetaData : SqlMetaDataPriv
	{
		// Token: 0x06002A70 RID: 10864 RVA: 0x0029CD14 File Offset: 0x0029C114
		internal _SqlMetaData(int ordinal)
		{
			this.ordinal = ordinal;
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06002A71 RID: 10865 RVA: 0x0029CD30 File Offset: 0x0029C130
		internal string serverName
		{
			get
			{
				return this.multiPartTableName.ServerName;
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06002A72 RID: 10866 RVA: 0x0029CD48 File Offset: 0x0029C148
		internal string catalogName
		{
			get
			{
				return this.multiPartTableName.CatalogName;
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06002A73 RID: 10867 RVA: 0x0029CD60 File Offset: 0x0029C160
		internal string schemaName
		{
			get
			{
				return this.multiPartTableName.SchemaName;
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06002A74 RID: 10868 RVA: 0x0029CD78 File Offset: 0x0029C178
		internal string tableName
		{
			get
			{
				return this.multiPartTableName.TableName;
			}
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06002A75 RID: 10869 RVA: 0x0029CD90 File Offset: 0x0029C190
		internal bool IsNewKatmaiDateTimeType
		{
			get
			{
				return SqlDbType.Date == this.type || SqlDbType.Time == this.type || SqlDbType.DateTime2 == this.type || SqlDbType.DateTimeOffset == this.type;
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06002A76 RID: 10870 RVA: 0x0029CDC8 File Offset: 0x0029C1C8
		internal bool IsLargeUdt
		{
			get
			{
				return this.type == SqlDbType.Udt && this.length == int.MaxValue;
			}
		}

		// Token: 0x04001BC5 RID: 7109
		internal string column;

		// Token: 0x04001BC6 RID: 7110
		internal string baseColumn;

		// Token: 0x04001BC7 RID: 7111
		internal MultiPartTableName multiPartTableName;

		// Token: 0x04001BC8 RID: 7112
		internal readonly int ordinal;

		// Token: 0x04001BC9 RID: 7113
		internal byte updatability;

		// Token: 0x04001BCA RID: 7114
		internal byte tableNum;

		// Token: 0x04001BCB RID: 7115
		internal bool isDifferentName;

		// Token: 0x04001BCC RID: 7116
		internal bool isKey;

		// Token: 0x04001BCD RID: 7117
		internal bool isHidden;

		// Token: 0x04001BCE RID: 7118
		internal bool isExpression;

		// Token: 0x04001BCF RID: 7119
		internal bool isIdentity;

		// Token: 0x04001BD0 RID: 7120
		internal bool isColumnSet;

		// Token: 0x04001BD1 RID: 7121
		internal byte op;

		// Token: 0x04001BD2 RID: 7122
		internal ushort operand;
	}
}
