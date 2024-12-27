using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Globalization;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000040 RID: 64
	internal class SmiStorageMetaData : SmiExtendedMetaData
	{
		// Token: 0x06000256 RID: 598 RVA: 0x001CC970 File Offset: 0x001CBD70
		[Obsolete("Not supported as of SMI v2.  Will be removed when v1 support dropped. Use ctor without columns param.")]
		internal SmiStorageMetaData(SqlDbType dbType, long maxLength, byte precision, byte scale, long localeId, SqlCompareOptions compareOptions, Type userDefinedType, SmiMetaData[] columns, string name, string typeSpecificNamePart1, string typeSpecificNamePart2, string typeSpecificNamePart3, bool allowsDBNull, string serverName, string catalogName, string schemaName, string tableName, string columnName, SqlBoolean isKey, bool isIdentity)
			: this(dbType, maxLength, precision, scale, localeId, compareOptions, userDefinedType, name, typeSpecificNamePart1, typeSpecificNamePart2, typeSpecificNamePart3, allowsDBNull, serverName, catalogName, schemaName, tableName, columnName, isKey, isIdentity)
		{
		}

		// Token: 0x06000257 RID: 599 RVA: 0x001CC9A8 File Offset: 0x001CBDA8
		internal SmiStorageMetaData(SqlDbType dbType, long maxLength, byte precision, byte scale, long localeId, SqlCompareOptions compareOptions, Type userDefinedType, string name, string typeSpecificNamePart1, string typeSpecificNamePart2, string typeSpecificNamePart3, bool allowsDBNull, string serverName, string catalogName, string schemaName, string tableName, string columnName, SqlBoolean isKey, bool isIdentity)
			: this(dbType, maxLength, precision, scale, localeId, compareOptions, userDefinedType, false, null, null, name, typeSpecificNamePart1, typeSpecificNamePart2, typeSpecificNamePart3, allowsDBNull, serverName, catalogName, schemaName, tableName, columnName, isKey, isIdentity)
		{
		}

		// Token: 0x06000258 RID: 600 RVA: 0x001CC9E4 File Offset: 0x001CBDE4
		internal SmiStorageMetaData(SqlDbType dbType, long maxLength, byte precision, byte scale, long localeId, SqlCompareOptions compareOptions, Type userDefinedType, bool isMultiValued, IList<SmiExtendedMetaData> fieldMetaData, SmiMetaDataPropertyCollection extendedProperties, string name, string typeSpecificNamePart1, string typeSpecificNamePart2, string typeSpecificNamePart3, bool allowsDBNull, string serverName, string catalogName, string schemaName, string tableName, string columnName, SqlBoolean isKey, bool isIdentity)
			: this(dbType, maxLength, precision, scale, localeId, compareOptions, userDefinedType, null, isMultiValued, fieldMetaData, extendedProperties, name, typeSpecificNamePart1, typeSpecificNamePart2, typeSpecificNamePart3, allowsDBNull, serverName, catalogName, schemaName, tableName, columnName, isKey, isIdentity, false)
		{
		}

		// Token: 0x06000259 RID: 601 RVA: 0x001CCA24 File Offset: 0x001CBE24
		internal SmiStorageMetaData(SqlDbType dbType, long maxLength, byte precision, byte scale, long localeId, SqlCompareOptions compareOptions, Type userDefinedType, string udtAssemblyQualifiedName, bool isMultiValued, IList<SmiExtendedMetaData> fieldMetaData, SmiMetaDataPropertyCollection extendedProperties, string name, string typeSpecificNamePart1, string typeSpecificNamePart2, string typeSpecificNamePart3, bool allowsDBNull, string serverName, string catalogName, string schemaName, string tableName, string columnName, SqlBoolean isKey, bool isIdentity, bool isColumnSet)
			: base(dbType, maxLength, precision, scale, localeId, compareOptions, userDefinedType, udtAssemblyQualifiedName, isMultiValued, fieldMetaData, extendedProperties, name, typeSpecificNamePart1, typeSpecificNamePart2, typeSpecificNamePart3)
		{
			this._allowsDBNull = allowsDBNull;
			this._serverName = serverName;
			this._catalogName = catalogName;
			this._schemaName = schemaName;
			this._tableName = tableName;
			this._columnName = columnName;
			this._isKey = isKey;
			this._isIdentity = isIdentity;
			this._isColumnSet = isColumnSet;
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600025A RID: 602 RVA: 0x001CCA9C File Offset: 0x001CBE9C
		internal bool AllowsDBNull
		{
			get
			{
				return this._allowsDBNull;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600025B RID: 603 RVA: 0x001CCAB0 File Offset: 0x001CBEB0
		internal string ServerName
		{
			get
			{
				return this._serverName;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600025C RID: 604 RVA: 0x001CCAC4 File Offset: 0x001CBEC4
		internal string CatalogName
		{
			get
			{
				return this._catalogName;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600025D RID: 605 RVA: 0x001CCAD8 File Offset: 0x001CBED8
		internal string SchemaName
		{
			get
			{
				return this._schemaName;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600025E RID: 606 RVA: 0x001CCAEC File Offset: 0x001CBEEC
		internal string TableName
		{
			get
			{
				return this._tableName;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600025F RID: 607 RVA: 0x001CCB00 File Offset: 0x001CBF00
		internal string ColumnName
		{
			get
			{
				return this._columnName;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000260 RID: 608 RVA: 0x001CCB14 File Offset: 0x001CBF14
		internal SqlBoolean IsKey
		{
			get
			{
				return this._isKey;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000261 RID: 609 RVA: 0x001CCB28 File Offset: 0x001CBF28
		internal bool IsIdentity
		{
			get
			{
				return this._isIdentity;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000262 RID: 610 RVA: 0x001CCB3C File Offset: 0x001CBF3C
		internal bool IsColumnSet
		{
			get
			{
				return this._isColumnSet;
			}
		}

		// Token: 0x06000263 RID: 611 RVA: 0x001CCB50 File Offset: 0x001CBF50
		internal override string TraceString(int indent)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}{1}         AllowsDBNull={2}\n\t{1}           ServerName='{3}'\n\t{1}          CatalogName='{4}'\n\t{1}           SchemaName='{5}'\n\t{1}            TableName='{6}'\n\t{1}           ColumnName='{7}'\n\t{1}                IsKey={8}\n\t{1}           IsIdentity={9}\n\t", new object[]
			{
				base.TraceString(indent),
				new string(' ', indent),
				this.AllowsDBNull,
				(this.ServerName != null) ? this.ServerName : "<null>",
				(this.CatalogName != null) ? this.CatalogName : "<null>",
				(this.SchemaName != null) ? this.SchemaName : "<null>",
				(this.TableName != null) ? this.TableName : "<null>",
				(this.ColumnName != null) ? this.ColumnName : "<null>",
				this.IsKey,
				this.IsIdentity
			});
		}

		// Token: 0x040005D1 RID: 1489
		private bool _allowsDBNull;

		// Token: 0x040005D2 RID: 1490
		private string _serverName;

		// Token: 0x040005D3 RID: 1491
		private string _catalogName;

		// Token: 0x040005D4 RID: 1492
		private string _schemaName;

		// Token: 0x040005D5 RID: 1493
		private string _tableName;

		// Token: 0x040005D6 RID: 1494
		private string _columnName;

		// Token: 0x040005D7 RID: 1495
		private SqlBoolean _isKey;

		// Token: 0x040005D8 RID: 1496
		private bool _isIdentity;

		// Token: 0x040005D9 RID: 1497
		private bool _isColumnSet;
	}
}
