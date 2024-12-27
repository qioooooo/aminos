using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000286 RID: 646
	public sealed class SqlMetaData
	{
		// Token: 0x060021C4 RID: 8644 RVA: 0x00269DD8 File Offset: 0x002691D8
		public SqlMetaData(string name, SqlDbType dbType)
		{
			this.Construct(name, dbType, false, false, SortOrder.Unspecified, -1);
		}

		// Token: 0x060021C5 RID: 8645 RVA: 0x00269DF8 File Offset: 0x002691F8
		public SqlMetaData(string name, SqlDbType dbType, bool useServerDefault, bool isUniqueKey, SortOrder columnSortOrder, int sortOrdinal)
		{
			this.Construct(name, dbType, useServerDefault, isUniqueKey, columnSortOrder, sortOrdinal);
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x00269E1C File Offset: 0x0026921C
		public SqlMetaData(string name, SqlDbType dbType, long maxLength)
		{
			this.Construct(name, dbType, maxLength, false, false, SortOrder.Unspecified, -1);
		}

		// Token: 0x060021C7 RID: 8647 RVA: 0x00269E3C File Offset: 0x0026923C
		public SqlMetaData(string name, SqlDbType dbType, long maxLength, bool useServerDefault, bool isUniqueKey, SortOrder columnSortOrder, int sortOrdinal)
		{
			this.Construct(name, dbType, maxLength, useServerDefault, isUniqueKey, columnSortOrder, sortOrdinal);
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x00269E60 File Offset: 0x00269260
		public SqlMetaData(string name, SqlDbType dbType, Type userDefinedType)
		{
			this.Construct(name, dbType, userDefinedType, null, false, false, SortOrder.Unspecified, -1);
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x00269E84 File Offset: 0x00269284
		public SqlMetaData(string name, SqlDbType dbType, Type userDefinedType, string serverTypeName)
		{
			this.Construct(name, dbType, userDefinedType, serverTypeName, false, false, SortOrder.Unspecified, -1);
		}

		// Token: 0x060021CA RID: 8650 RVA: 0x00269EA8 File Offset: 0x002692A8
		public SqlMetaData(string name, SqlDbType dbType, Type userDefinedType, string serverTypeName, bool useServerDefault, bool isUniqueKey, SortOrder columnSortOrder, int sortOrdinal)
		{
			this.Construct(name, dbType, userDefinedType, serverTypeName, useServerDefault, isUniqueKey, columnSortOrder, sortOrdinal);
		}

		// Token: 0x060021CB RID: 8651 RVA: 0x00269ED0 File Offset: 0x002692D0
		public SqlMetaData(string name, SqlDbType dbType, byte precision, byte scale)
		{
			this.Construct(name, dbType, precision, scale, false, false, SortOrder.Unspecified, -1);
		}

		// Token: 0x060021CC RID: 8652 RVA: 0x00269EF4 File Offset: 0x002692F4
		public SqlMetaData(string name, SqlDbType dbType, byte precision, byte scale, bool useServerDefault, bool isUniqueKey, SortOrder columnSortOrder, int sortOrdinal)
		{
			this.Construct(name, dbType, precision, scale, useServerDefault, isUniqueKey, columnSortOrder, sortOrdinal);
		}

		// Token: 0x060021CD RID: 8653 RVA: 0x00269F1C File Offset: 0x0026931C
		public SqlMetaData(string name, SqlDbType dbType, long maxLength, long locale, SqlCompareOptions compareOptions)
		{
			this.Construct(name, dbType, maxLength, locale, compareOptions, false, false, SortOrder.Unspecified, -1);
		}

		// Token: 0x060021CE RID: 8654 RVA: 0x00269F40 File Offset: 0x00269340
		public SqlMetaData(string name, SqlDbType dbType, long maxLength, long locale, SqlCompareOptions compareOptions, bool useServerDefault, bool isUniqueKey, SortOrder columnSortOrder, int sortOrdinal)
		{
			this.Construct(name, dbType, maxLength, locale, compareOptions, useServerDefault, isUniqueKey, columnSortOrder, sortOrdinal);
		}

		// Token: 0x060021CF RID: 8655 RVA: 0x00269F68 File Offset: 0x00269368
		public SqlMetaData(string name, SqlDbType dbType, string database, string owningSchema, string objectName, bool useServerDefault, bool isUniqueKey, SortOrder columnSortOrder, int sortOrdinal)
		{
			this.Construct(name, dbType, database, owningSchema, objectName, useServerDefault, isUniqueKey, columnSortOrder, sortOrdinal);
		}

		// Token: 0x060021D0 RID: 8656 RVA: 0x00269F90 File Offset: 0x00269390
		public SqlMetaData(string name, SqlDbType dbType, long maxLength, byte precision, byte scale, long locale, SqlCompareOptions compareOptions, Type userDefinedType)
			: this(name, dbType, maxLength, precision, scale, locale, compareOptions, userDefinedType, false, false, SortOrder.Unspecified, -1)
		{
		}

		// Token: 0x060021D1 RID: 8657 RVA: 0x00269FB4 File Offset: 0x002693B4
		public SqlMetaData(string name, SqlDbType dbType, long maxLength, byte precision, byte scale, long localeId, SqlCompareOptions compareOptions, Type userDefinedType, bool useServerDefault, bool isUniqueKey, SortOrder columnSortOrder, int sortOrdinal)
		{
			switch (dbType)
			{
			case SqlDbType.BigInt:
			case SqlDbType.Bit:
			case SqlDbType.DateTime:
			case SqlDbType.Float:
			case SqlDbType.Image:
			case SqlDbType.Int:
			case SqlDbType.Money:
			case SqlDbType.Real:
			case SqlDbType.UniqueIdentifier:
			case SqlDbType.SmallDateTime:
			case SqlDbType.SmallInt:
			case SqlDbType.SmallMoney:
			case SqlDbType.Timestamp:
			case SqlDbType.TinyInt:
			case SqlDbType.Xml:
			case SqlDbType.Date:
				this.Construct(name, dbType, useServerDefault, isUniqueKey, columnSortOrder, sortOrdinal);
				return;
			case SqlDbType.Binary:
			case SqlDbType.VarBinary:
				this.Construct(name, dbType, maxLength, useServerDefault, isUniqueKey, columnSortOrder, sortOrdinal);
				return;
			case SqlDbType.Char:
			case SqlDbType.NChar:
			case SqlDbType.NVarChar:
			case SqlDbType.VarChar:
				this.Construct(name, dbType, maxLength, localeId, compareOptions, useServerDefault, isUniqueKey, columnSortOrder, sortOrdinal);
				return;
			case SqlDbType.Decimal:
			case SqlDbType.Time:
			case SqlDbType.DateTime2:
			case SqlDbType.DateTimeOffset:
				this.Construct(name, dbType, precision, scale, useServerDefault, isUniqueKey, columnSortOrder, sortOrdinal);
				return;
			case SqlDbType.NText:
			case SqlDbType.Text:
				this.Construct(name, dbType, SqlMetaData.Max, localeId, compareOptions, useServerDefault, isUniqueKey, columnSortOrder, sortOrdinal);
				return;
			case SqlDbType.Variant:
				this.Construct(name, dbType, useServerDefault, isUniqueKey, columnSortOrder, sortOrdinal);
				return;
			case SqlDbType.Udt:
				this.Construct(name, dbType, userDefinedType, "", useServerDefault, isUniqueKey, columnSortOrder, sortOrdinal);
				return;
			}
			SQL.InvalidSqlDbTypeForConstructor(dbType);
		}

		// Token: 0x060021D2 RID: 8658 RVA: 0x0026A0F8 File Offset: 0x002694F8
		public SqlMetaData(string name, SqlDbType dbType, string database, string owningSchema, string objectName)
		{
			this.Construct(name, dbType, database, owningSchema, objectName, false, false, SortOrder.Unspecified, -1);
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x0026A11C File Offset: 0x0026951C
		internal SqlMetaData(string name, SqlDbType sqlDBType, long maxLength, byte precision, byte scale, long localeId, SqlCompareOptions compareOptions, string xmlSchemaCollectionDatabase, string xmlSchemaCollectionOwningSchema, string xmlSchemaCollectionName, bool partialLength, Type udtType)
		{
			this.AssertNameIsValid(name);
			this.m_strName = name;
			this.m_sqlDbType = sqlDBType;
			this.m_lMaxLength = maxLength;
			this.m_bPrecision = precision;
			this.m_bScale = scale;
			this.m_lLocale = localeId;
			this.m_eCompareOptions = compareOptions;
			this.m_XmlSchemaCollectionDatabase = xmlSchemaCollectionDatabase;
			this.m_XmlSchemaCollectionOwningSchema = xmlSchemaCollectionOwningSchema;
			this.m_XmlSchemaCollectionName = xmlSchemaCollectionName;
			this.m_bPartialLength = partialLength;
			this.m_udttype = udtType;
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x0026A194 File Offset: 0x00269594
		private SqlMetaData(string name, SqlDbType sqlDbType, long maxLength, byte precision, byte scale, long localeId, SqlCompareOptions compareOptions, bool partialLength)
		{
			this.AssertNameIsValid(name);
			this.m_strName = name;
			this.m_sqlDbType = sqlDbType;
			this.m_lMaxLength = maxLength;
			this.m_bPrecision = precision;
			this.m_bScale = scale;
			this.m_lLocale = localeId;
			this.m_eCompareOptions = compareOptions;
			this.m_bPartialLength = partialLength;
			this.m_udttype = null;
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x060021D5 RID: 8661 RVA: 0x0026A1F4 File Offset: 0x002695F4
		public SqlCompareOptions CompareOptions
		{
			get
			{
				return this.m_eCompareOptions;
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x060021D6 RID: 8662 RVA: 0x0026A208 File Offset: 0x00269608
		public DbType DbType
		{
			get
			{
				return SqlMetaData.sxm_rgSqlDbTypeToDbType[(int)this.m_sqlDbType];
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x060021D7 RID: 8663 RVA: 0x0026A224 File Offset: 0x00269624
		public bool IsUniqueKey
		{
			get
			{
				return this.m_isUniqueKey;
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x060021D8 RID: 8664 RVA: 0x0026A238 File Offset: 0x00269638
		public long LocaleId
		{
			get
			{
				return this.m_lLocale;
			}
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x060021D9 RID: 8665 RVA: 0x0026A24C File Offset: 0x0026964C
		public static long Max
		{
			get
			{
				return -1L;
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x060021DA RID: 8666 RVA: 0x0026A25C File Offset: 0x0026965C
		public long MaxLength
		{
			get
			{
				return this.m_lMaxLength;
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x060021DB RID: 8667 RVA: 0x0026A270 File Offset: 0x00269670
		public string Name
		{
			get
			{
				return this.m_strName;
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x060021DC RID: 8668 RVA: 0x0026A284 File Offset: 0x00269684
		public byte Precision
		{
			get
			{
				return this.m_bPrecision;
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x060021DD RID: 8669 RVA: 0x0026A298 File Offset: 0x00269698
		public byte Scale
		{
			get
			{
				return this.m_bScale;
			}
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x060021DE RID: 8670 RVA: 0x0026A2AC File Offset: 0x002696AC
		public SortOrder SortOrder
		{
			get
			{
				return this.m_columnSortOrder;
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x060021DF RID: 8671 RVA: 0x0026A2C0 File Offset: 0x002696C0
		public int SortOrdinal
		{
			get
			{
				return this.m_sortOrdinal;
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x060021E0 RID: 8672 RVA: 0x0026A2D4 File Offset: 0x002696D4
		public SqlDbType SqlDbType
		{
			get
			{
				return this.m_sqlDbType;
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x060021E1 RID: 8673 RVA: 0x0026A2E8 File Offset: 0x002696E8
		public Type Type
		{
			get
			{
				return this.m_udttype;
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x060021E2 RID: 8674 RVA: 0x0026A2FC File Offset: 0x002696FC
		public string TypeName
		{
			get
			{
				if (this.m_serverTypeName != null)
				{
					return this.m_serverTypeName;
				}
				if (this.SqlDbType == SqlDbType.Udt)
				{
					return this.UdtTypeName;
				}
				return SqlMetaData.sxm_rgDefaults[(int)this.SqlDbType].Name;
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x060021E3 RID: 8675 RVA: 0x0026A33C File Offset: 0x0026973C
		internal string ServerTypeName
		{
			get
			{
				return this.m_serverTypeName;
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x060021E4 RID: 8676 RVA: 0x0026A350 File Offset: 0x00269750
		public bool UseServerDefault
		{
			get
			{
				return this.m_useServerDefault;
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x060021E5 RID: 8677 RVA: 0x0026A364 File Offset: 0x00269764
		public string XmlSchemaCollectionDatabase
		{
			get
			{
				return this.m_XmlSchemaCollectionDatabase;
			}
		}

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x060021E6 RID: 8678 RVA: 0x0026A378 File Offset: 0x00269778
		public string XmlSchemaCollectionName
		{
			get
			{
				return this.m_XmlSchemaCollectionName;
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x060021E7 RID: 8679 RVA: 0x0026A38C File Offset: 0x0026978C
		public string XmlSchemaCollectionOwningSchema
		{
			get
			{
				return this.m_XmlSchemaCollectionOwningSchema;
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x060021E8 RID: 8680 RVA: 0x0026A3A0 File Offset: 0x002697A0
		internal bool IsPartialLength
		{
			get
			{
				return this.m_bPartialLength;
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x060021E9 RID: 8681 RVA: 0x0026A3B4 File Offset: 0x002697B4
		internal string UdtTypeName
		{
			get
			{
				if (this.SqlDbType != SqlDbType.Udt)
				{
					return null;
				}
				if (this.m_udttype == null)
				{
					return null;
				}
				return this.m_udttype.FullName;
			}
		}

		// Token: 0x060021EA RID: 8682 RVA: 0x0026A3E4 File Offset: 0x002697E4
		private void Construct(string name, SqlDbType dbType, bool useServerDefault, bool isUniqueKey, SortOrder columnSortOrder, int sortOrdinal)
		{
			this.AssertNameIsValid(name);
			this.ValidateSortOrder(columnSortOrder, sortOrdinal);
			if (dbType != SqlDbType.BigInt && SqlDbType.Bit != dbType && SqlDbType.DateTime != dbType && SqlDbType.Date != dbType && SqlDbType.DateTime2 != dbType && SqlDbType.DateTimeOffset != dbType && SqlDbType.Decimal != dbType && SqlDbType.Float != dbType && SqlDbType.Image != dbType && SqlDbType.Int != dbType && SqlDbType.Money != dbType && SqlDbType.NText != dbType && SqlDbType.Real != dbType && SqlDbType.SmallDateTime != dbType && SqlDbType.SmallInt != dbType && SqlDbType.SmallMoney != dbType && SqlDbType.Text != dbType && SqlDbType.Time != dbType && SqlDbType.Timestamp != dbType && SqlDbType.TinyInt != dbType && SqlDbType.UniqueIdentifier != dbType && SqlDbType.Variant != dbType && SqlDbType.Xml != dbType)
			{
				throw SQL.InvalidSqlDbTypeForConstructor(dbType);
			}
			this.SetDefaultsForType(dbType);
			if (SqlDbType.NText == dbType || SqlDbType.Text == dbType)
			{
				this.m_lLocale = (long)CultureInfo.CurrentCulture.LCID;
			}
			this.m_strName = name;
			this.m_useServerDefault = useServerDefault;
			this.m_isUniqueKey = isUniqueKey;
			this.m_columnSortOrder = columnSortOrder;
			this.m_sortOrdinal = sortOrdinal;
		}

		// Token: 0x060021EB RID: 8683 RVA: 0x0026A4BC File Offset: 0x002698BC
		private void Construct(string name, SqlDbType dbType, long maxLength, bool useServerDefault, bool isUniqueKey, SortOrder columnSortOrder, int sortOrdinal)
		{
			this.AssertNameIsValid(name);
			this.ValidateSortOrder(columnSortOrder, sortOrdinal);
			long num = 0L;
			if (SqlDbType.Char == dbType)
			{
				if (maxLength > 8000L || maxLength < 0L)
				{
					throw ADP.Argument(Res.GetString("ADP_InvalidDataLength2", new object[] { maxLength.ToString(CultureInfo.InvariantCulture) }), "maxLength");
				}
				num = (long)CultureInfo.CurrentCulture.LCID;
			}
			else if (SqlDbType.VarChar == dbType)
			{
				if ((maxLength > 8000L || maxLength < 0L) && maxLength != SqlMetaData.Max)
				{
					throw ADP.Argument(Res.GetString("ADP_InvalidDataLength2", new object[] { maxLength.ToString(CultureInfo.InvariantCulture) }), "maxLength");
				}
				num = (long)CultureInfo.CurrentCulture.LCID;
			}
			else if (SqlDbType.NChar == dbType)
			{
				if (maxLength > 4000L || maxLength < 0L)
				{
					throw ADP.Argument(Res.GetString("ADP_InvalidDataLength2", new object[] { maxLength.ToString(CultureInfo.InvariantCulture) }), "maxLength");
				}
				num = (long)CultureInfo.CurrentCulture.LCID;
			}
			else if (SqlDbType.NVarChar == dbType)
			{
				if ((maxLength > 4000L || maxLength < 0L) && maxLength != SqlMetaData.Max)
				{
					throw ADP.Argument(Res.GetString("ADP_InvalidDataLength2", new object[] { maxLength.ToString(CultureInfo.InvariantCulture) }), "maxLength");
				}
				num = (long)CultureInfo.CurrentCulture.LCID;
			}
			else if (SqlDbType.NText == dbType || SqlDbType.Text == dbType)
			{
				if (SqlMetaData.Max != maxLength)
				{
					throw ADP.Argument(Res.GetString("ADP_InvalidDataLength2", new object[] { maxLength.ToString(CultureInfo.InvariantCulture) }), "maxLength");
				}
				num = (long)CultureInfo.CurrentCulture.LCID;
			}
			else if (SqlDbType.Binary == dbType)
			{
				if (maxLength > 8000L || maxLength < 0L)
				{
					throw ADP.Argument(Res.GetString("ADP_InvalidDataLength2", new object[] { maxLength.ToString(CultureInfo.InvariantCulture) }), "maxLength");
				}
			}
			else if (SqlDbType.VarBinary == dbType)
			{
				if ((maxLength > 8000L || maxLength < 0L) && maxLength != SqlMetaData.Max)
				{
					throw ADP.Argument(Res.GetString("ADP_InvalidDataLength2", new object[] { maxLength.ToString(CultureInfo.InvariantCulture) }), "maxLength");
				}
			}
			else
			{
				if (SqlDbType.Image != dbType)
				{
					throw SQL.InvalidSqlDbTypeForConstructor(dbType);
				}
				if (SqlMetaData.Max != maxLength)
				{
					throw ADP.Argument(Res.GetString("ADP_InvalidDataLength2", new object[] { maxLength.ToString(CultureInfo.InvariantCulture) }), "maxLength");
				}
			}
			this.SetDefaultsForType(dbType);
			this.m_strName = name;
			this.m_lMaxLength = maxLength;
			this.m_lLocale = num;
			this.m_useServerDefault = useServerDefault;
			this.m_isUniqueKey = isUniqueKey;
			this.m_columnSortOrder = columnSortOrder;
			this.m_sortOrdinal = sortOrdinal;
		}

		// Token: 0x060021EC RID: 8684 RVA: 0x0026A790 File Offset: 0x00269B90
		private void Construct(string name, SqlDbType dbType, long maxLength, long locale, SqlCompareOptions compareOptions, bool useServerDefault, bool isUniqueKey, SortOrder columnSortOrder, int sortOrdinal)
		{
			this.AssertNameIsValid(name);
			this.ValidateSortOrder(columnSortOrder, sortOrdinal);
			if (SqlDbType.Char == dbType)
			{
				if (maxLength > 8000L || maxLength < 0L)
				{
					throw ADP.Argument(Res.GetString("ADP_InvalidDataLength2", new object[] { maxLength.ToString(CultureInfo.InvariantCulture) }), "maxLength");
				}
			}
			else if (SqlDbType.VarChar == dbType)
			{
				if ((maxLength > 8000L || maxLength < 0L) && maxLength != SqlMetaData.Max)
				{
					throw ADP.Argument(Res.GetString("ADP_InvalidDataLength2", new object[] { maxLength.ToString(CultureInfo.InvariantCulture) }), "maxLength");
				}
			}
			else if (SqlDbType.NChar == dbType)
			{
				if (maxLength > 4000L || maxLength < 0L)
				{
					throw ADP.Argument(Res.GetString("ADP_InvalidDataLength2", new object[] { maxLength.ToString(CultureInfo.InvariantCulture) }), "maxLength");
				}
			}
			else if (SqlDbType.NVarChar == dbType)
			{
				if ((maxLength > 4000L || maxLength < 0L) && maxLength != SqlMetaData.Max)
				{
					throw ADP.Argument(Res.GetString("ADP_InvalidDataLength2", new object[] { maxLength.ToString(CultureInfo.InvariantCulture) }), "maxLength");
				}
			}
			else
			{
				if (SqlDbType.NText != dbType && SqlDbType.Text != dbType)
				{
					throw SQL.InvalidSqlDbTypeForConstructor(dbType);
				}
				if (SqlMetaData.Max != maxLength)
				{
					throw ADP.Argument(Res.GetString("ADP_InvalidDataLength2", new object[] { maxLength.ToString(CultureInfo.InvariantCulture) }), "maxLength");
				}
			}
			if (SqlCompareOptions.BinarySort != compareOptions && (~(SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreNonSpace | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth) & compareOptions) != SqlCompareOptions.None)
			{
				throw ADP.InvalidEnumerationValue(typeof(SqlCompareOptions), (int)compareOptions);
			}
			this.SetDefaultsForType(dbType);
			this.m_strName = name;
			this.m_lMaxLength = maxLength;
			this.m_lLocale = locale;
			this.m_eCompareOptions = compareOptions;
			this.m_useServerDefault = useServerDefault;
			this.m_isUniqueKey = isUniqueKey;
			this.m_columnSortOrder = columnSortOrder;
			this.m_sortOrdinal = sortOrdinal;
		}

		// Token: 0x060021ED RID: 8685 RVA: 0x0026A978 File Offset: 0x00269D78
		private void Construct(string name, SqlDbType dbType, byte precision, byte scale, bool useServerDefault, bool isUniqueKey, SortOrder columnSortOrder, int sortOrdinal)
		{
			this.AssertNameIsValid(name);
			this.ValidateSortOrder(columnSortOrder, sortOrdinal);
			if (SqlDbType.Decimal == dbType)
			{
				if (precision > SqlDecimal.MaxPrecision || scale > precision)
				{
					throw SQL.PrecisionValueOutOfRange(precision);
				}
				if (scale > SqlDecimal.MaxScale)
				{
					throw SQL.ScaleValueOutOfRange(scale);
				}
			}
			else
			{
				if (SqlDbType.Time != dbType && SqlDbType.DateTime2 != dbType && SqlDbType.DateTimeOffset != dbType)
				{
					throw SQL.InvalidSqlDbTypeForConstructor(dbType);
				}
				if (scale > 7)
				{
					throw SQL.TimeScaleValueOutOfRange(scale);
				}
			}
			this.SetDefaultsForType(dbType);
			this.m_strName = name;
			this.m_bPrecision = precision;
			this.m_bScale = scale;
			if (SqlDbType.Decimal == dbType)
			{
				this.m_lMaxLength = (long)((ulong)SqlMetaData.__maxLenFromPrecision[(int)(precision - 1)]);
			}
			else
			{
				this.m_lMaxLength -= (long)((ulong)SqlMetaData.__maxVarTimeLenOffsetFromScale[(int)scale]);
			}
			this.m_useServerDefault = useServerDefault;
			this.m_isUniqueKey = isUniqueKey;
			this.m_columnSortOrder = columnSortOrder;
			this.m_sortOrdinal = sortOrdinal;
		}

		// Token: 0x060021EE RID: 8686 RVA: 0x0026AA4C File Offset: 0x00269E4C
		private void Construct(string name, SqlDbType dbType, Type userDefinedType, string serverTypeName, bool useServerDefault, bool isUniqueKey, SortOrder columnSortOrder, int sortOrdinal)
		{
			this.AssertNameIsValid(name);
			this.ValidateSortOrder(columnSortOrder, sortOrdinal);
			if (SqlDbType.Udt != dbType)
			{
				throw SQL.InvalidSqlDbTypeForConstructor(dbType);
			}
			if (userDefinedType == null)
			{
				throw ADP.ArgumentNull("userDefinedType");
			}
			this.SetDefaultsForType(SqlDbType.Udt);
			this.m_strName = name;
			this.m_lMaxLength = (long)SerializationHelperSql9.GetUdtMaxLength(userDefinedType);
			this.m_udttype = userDefinedType;
			this.m_serverTypeName = serverTypeName;
			this.m_useServerDefault = useServerDefault;
			this.m_isUniqueKey = isUniqueKey;
			this.m_columnSortOrder = columnSortOrder;
			this.m_sortOrdinal = sortOrdinal;
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x0026AAD0 File Offset: 0x00269ED0
		private void Construct(string name, SqlDbType dbType, string database, string owningSchema, string objectName, bool useServerDefault, bool isUniqueKey, SortOrder columnSortOrder, int sortOrdinal)
		{
			this.AssertNameIsValid(name);
			this.ValidateSortOrder(columnSortOrder, sortOrdinal);
			if (SqlDbType.Xml != dbType)
			{
				throw SQL.InvalidSqlDbTypeForConstructor(dbType);
			}
			if ((database != null || owningSchema != null) && objectName == null)
			{
				throw ADP.ArgumentNull("objectName");
			}
			this.SetDefaultsForType(SqlDbType.Xml);
			this.m_strName = name;
			this.m_XmlSchemaCollectionDatabase = database;
			this.m_XmlSchemaCollectionOwningSchema = owningSchema;
			this.m_XmlSchemaCollectionName = objectName;
			this.m_useServerDefault = useServerDefault;
			this.m_isUniqueKey = isUniqueKey;
			this.m_columnSortOrder = columnSortOrder;
			this.m_sortOrdinal = sortOrdinal;
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x0026AB58 File Offset: 0x00269F58
		private void AssertNameIsValid(string name)
		{
			if (name == null)
			{
				throw ADP.ArgumentNull("name");
			}
			if (128L < (long)name.Length)
			{
				throw SQL.NameTooLong("name");
			}
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x0026AB90 File Offset: 0x00269F90
		private void ValidateSortOrder(SortOrder columnSortOrder, int sortOrdinal)
		{
			if (SortOrder.Unspecified != columnSortOrder && columnSortOrder != SortOrder.Ascending && SortOrder.Descending != columnSortOrder)
			{
				throw SQL.InvalidSortOrder(columnSortOrder);
			}
			if (SortOrder.Unspecified == columnSortOrder != (-1 == sortOrdinal))
			{
				throw SQL.MustSpecifyBothSortOrderAndOrdinal(columnSortOrder, sortOrdinal);
			}
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x0026ABC4 File Offset: 0x00269FC4
		public short Adjust(short value)
		{
			if (SqlDbType.SmallInt != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			return value;
		}

		// Token: 0x060021F3 RID: 8691 RVA: 0x0026ABE4 File Offset: 0x00269FE4
		public int Adjust(int value)
		{
			if (SqlDbType.Int != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			return value;
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x0026AC00 File Offset: 0x0026A000
		public long Adjust(long value)
		{
			if (this.SqlDbType != SqlDbType.BigInt)
			{
				SqlMetaData.ThrowInvalidType();
			}
			return value;
		}

		// Token: 0x060021F5 RID: 8693 RVA: 0x0026AC1C File Offset: 0x0026A01C
		public float Adjust(float value)
		{
			if (SqlDbType.Real != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			return value;
		}

		// Token: 0x060021F6 RID: 8694 RVA: 0x0026AC3C File Offset: 0x0026A03C
		public double Adjust(double value)
		{
			if (SqlDbType.Float != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			return value;
		}

		// Token: 0x060021F7 RID: 8695 RVA: 0x0026AC58 File Offset: 0x0026A058
		public string Adjust(string value)
		{
			if (SqlDbType.Char == this.SqlDbType || SqlDbType.NChar == this.SqlDbType)
			{
				if (value != null && (long)value.Length < this.MaxLength)
				{
					value = value.PadRight((int)this.MaxLength);
				}
			}
			else if (SqlDbType.VarChar != this.SqlDbType && SqlDbType.NVarChar != this.SqlDbType && SqlDbType.Text != this.SqlDbType && SqlDbType.NText != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			if (value == null)
			{
				return null;
			}
			if ((long)value.Length > this.MaxLength && SqlMetaData.Max != this.MaxLength)
			{
				value = value.Remove((int)this.MaxLength, (int)((long)value.Length - this.MaxLength));
			}
			return value;
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x0026AD08 File Offset: 0x0026A108
		public decimal Adjust(decimal value)
		{
			if (SqlDbType.Decimal != this.SqlDbType && SqlDbType.Money != this.SqlDbType && SqlDbType.SmallMoney != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			if (SqlDbType.Decimal != this.SqlDbType)
			{
				this.VerifyMoneyRange(new SqlMoney(value));
				return value;
			}
			return this.InternalAdjustSqlDecimal(new SqlDecimal(value)).Value;
		}

		// Token: 0x060021F9 RID: 8697 RVA: 0x0026AD64 File Offset: 0x0026A164
		public DateTime Adjust(DateTime value)
		{
			if (SqlDbType.DateTime == this.SqlDbType || SqlDbType.SmallDateTime == this.SqlDbType)
			{
				this.VerifyDateTimeRange(value);
			}
			else
			{
				if (SqlDbType.DateTime2 == this.SqlDbType)
				{
					return new DateTime(this.InternalAdjustTimeTicks(value.Ticks));
				}
				if (SqlDbType.Date == this.SqlDbType)
				{
					return value.Date;
				}
				SqlMetaData.ThrowInvalidType();
			}
			return value;
		}

		// Token: 0x060021FA RID: 8698 RVA: 0x0026ADC4 File Offset: 0x0026A1C4
		public Guid Adjust(Guid value)
		{
			if (SqlDbType.UniqueIdentifier != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			return value;
		}

		// Token: 0x060021FB RID: 8699 RVA: 0x0026ADE4 File Offset: 0x0026A1E4
		public SqlBoolean Adjust(SqlBoolean value)
		{
			if (SqlDbType.Bit != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			return value;
		}

		// Token: 0x060021FC RID: 8700 RVA: 0x0026AE00 File Offset: 0x0026A200
		public SqlByte Adjust(SqlByte value)
		{
			if (SqlDbType.TinyInt != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			return value;
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x0026AE20 File Offset: 0x0026A220
		public SqlInt16 Adjust(SqlInt16 value)
		{
			if (SqlDbType.SmallInt != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			return value;
		}

		// Token: 0x060021FE RID: 8702 RVA: 0x0026AE40 File Offset: 0x0026A240
		public SqlInt32 Adjust(SqlInt32 value)
		{
			if (SqlDbType.Int != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			return value;
		}

		// Token: 0x060021FF RID: 8703 RVA: 0x0026AE5C File Offset: 0x0026A25C
		public SqlInt64 Adjust(SqlInt64 value)
		{
			if (this.SqlDbType != SqlDbType.BigInt)
			{
				SqlMetaData.ThrowInvalidType();
			}
			return value;
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x0026AE78 File Offset: 0x0026A278
		public SqlSingle Adjust(SqlSingle value)
		{
			if (SqlDbType.Real != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			return value;
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x0026AE98 File Offset: 0x0026A298
		public SqlDouble Adjust(SqlDouble value)
		{
			if (SqlDbType.Float != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			return value;
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x0026AEB4 File Offset: 0x0026A2B4
		public SqlMoney Adjust(SqlMoney value)
		{
			if (SqlDbType.Money != this.SqlDbType && SqlDbType.SmallMoney != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			if (!value.IsNull)
			{
				this.VerifyMoneyRange(value);
			}
			return value;
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x0026AEEC File Offset: 0x0026A2EC
		public SqlDateTime Adjust(SqlDateTime value)
		{
			if (SqlDbType.DateTime != this.SqlDbType && SqlDbType.SmallDateTime != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			if (!value.IsNull)
			{
				this.VerifyDateTimeRange(value.Value);
			}
			return value;
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x0026AF28 File Offset: 0x0026A328
		public SqlDecimal Adjust(SqlDecimal value)
		{
			if (SqlDbType.Decimal != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			return this.InternalAdjustSqlDecimal(value);
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x0026AF4C File Offset: 0x0026A34C
		public SqlString Adjust(SqlString value)
		{
			if (SqlDbType.Char == this.SqlDbType || SqlDbType.NChar == this.SqlDbType)
			{
				if (!value.IsNull && (long)value.Value.Length < this.MaxLength)
				{
					return new SqlString(value.Value.PadRight((int)this.MaxLength));
				}
			}
			else if (SqlDbType.VarChar != this.SqlDbType && SqlDbType.NVarChar != this.SqlDbType && SqlDbType.Text != this.SqlDbType && SqlDbType.NText != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			if (value.IsNull)
			{
				return value;
			}
			if ((long)value.Value.Length > this.MaxLength && SqlMetaData.Max != this.MaxLength)
			{
				value = new SqlString(value.Value.Remove((int)this.MaxLength, (int)((long)value.Value.Length - this.MaxLength)));
			}
			return value;
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x0026B030 File Offset: 0x0026A430
		public SqlBinary Adjust(SqlBinary value)
		{
			if (SqlDbType.Binary == this.SqlDbType || SqlDbType.Timestamp == this.SqlDbType)
			{
				if (!value.IsNull && (long)value.Length < this.MaxLength)
				{
					byte[] value2 = value.Value;
					byte[] array = new byte[this.MaxLength];
					Array.Copy(value2, array, value2.Length);
					Array.Clear(array, value2.Length, array.Length - value2.Length);
					return new SqlBinary(array);
				}
			}
			else if (SqlDbType.VarBinary != this.SqlDbType && SqlDbType.Image != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			if (value.IsNull)
			{
				return value;
			}
			if ((long)value.Length > this.MaxLength && SqlMetaData.Max != this.MaxLength)
			{
				byte[] value3 = value.Value;
				byte[] array2 = new byte[this.MaxLength];
				Array.Copy(value3, array2, (int)this.MaxLength);
				value = new SqlBinary(array2);
			}
			return value;
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x0026B10C File Offset: 0x0026A50C
		public SqlGuid Adjust(SqlGuid value)
		{
			if (SqlDbType.UniqueIdentifier != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			return value;
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x0026B12C File Offset: 0x0026A52C
		public SqlChars Adjust(SqlChars value)
		{
			if (SqlDbType.Char == this.SqlDbType || SqlDbType.NChar == this.SqlDbType)
			{
				if (value != null && !value.IsNull)
				{
					long length = value.Length;
					if (length < this.MaxLength)
					{
						if (value.MaxLength < this.MaxLength)
						{
							char[] array = new char[(int)this.MaxLength];
							Array.Copy(value.Buffer, array, (int)length);
							value = new SqlChars(array);
						}
						char[] buffer = value.Buffer;
						for (long num = length; num < this.MaxLength; num += 1L)
						{
							buffer[(int)(checked((IntPtr)num))] = ' ';
						}
						value.SetLength(this.MaxLength);
						return value;
					}
				}
			}
			else if (SqlDbType.VarChar != this.SqlDbType && SqlDbType.NVarChar != this.SqlDbType && SqlDbType.Text != this.SqlDbType && SqlDbType.NText != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			if (value == null || value.IsNull)
			{
				return value;
			}
			if (value.Length > this.MaxLength && SqlMetaData.Max != this.MaxLength)
			{
				value.SetLength(this.MaxLength);
			}
			return value;
		}

		// Token: 0x06002209 RID: 8713 RVA: 0x0026B234 File Offset: 0x0026A634
		public SqlBytes Adjust(SqlBytes value)
		{
			if (SqlDbType.Binary == this.SqlDbType || SqlDbType.Timestamp == this.SqlDbType)
			{
				if (value != null && !value.IsNull)
				{
					int num = (int)value.Length;
					if ((long)num < this.MaxLength)
					{
						if (value.MaxLength < this.MaxLength)
						{
							byte[] array = new byte[this.MaxLength];
							Array.Copy(value.Buffer, array, num);
							value = new SqlBytes(array);
						}
						byte[] buffer = value.Buffer;
						Array.Clear(buffer, num, buffer.Length - num);
						value.SetLength(this.MaxLength);
						return value;
					}
				}
			}
			else if (SqlDbType.VarBinary != this.SqlDbType && SqlDbType.Image != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			if (value == null || value.IsNull)
			{
				return value;
			}
			if (value.Length > this.MaxLength && SqlMetaData.Max != this.MaxLength)
			{
				value.SetLength(this.MaxLength);
			}
			return value;
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x0026B318 File Offset: 0x0026A718
		public SqlXml Adjust(SqlXml value)
		{
			if (SqlDbType.Xml != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			return value;
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x0026B338 File Offset: 0x0026A738
		public TimeSpan Adjust(TimeSpan value)
		{
			if (SqlDbType.Time != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			this.VerifyTimeRange(value);
			return new TimeSpan(this.InternalAdjustTimeTicks(value.Ticks));
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x0026B370 File Offset: 0x0026A770
		public DateTimeOffset Adjust(DateTimeOffset value)
		{
			if (SqlDbType.DateTimeOffset != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			return new DateTimeOffset(this.InternalAdjustTimeTicks(value.Ticks), value.Offset);
		}

		// Token: 0x0600220D RID: 8717 RVA: 0x0026B3A8 File Offset: 0x0026A7A8
		public object Adjust(object value)
		{
			if (value == null)
			{
				return null;
			}
			Type type = value.GetType();
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Empty:
				throw ADP.InvalidDataType(TypeCode.Empty);
			case TypeCode.Object:
				if (type == typeof(byte[]))
				{
					return this.Adjust((byte[])value);
				}
				if (type == typeof(char[]))
				{
					return this.Adjust((char[])value);
				}
				if (type == typeof(Guid))
				{
					return this.Adjust((Guid)value);
				}
				if (type == typeof(object))
				{
					throw ADP.InvalidDataType(TypeCode.UInt64);
				}
				if (type == typeof(SqlBinary))
				{
					return this.Adjust((SqlBinary)value);
				}
				if (type == typeof(SqlBoolean))
				{
					return this.Adjust((SqlBoolean)value);
				}
				if (type == typeof(SqlByte))
				{
					return this.Adjust((SqlByte)value);
				}
				if (type == typeof(SqlDateTime))
				{
					return this.Adjust((SqlDateTime)value);
				}
				if (type == typeof(SqlDouble))
				{
					return this.Adjust((SqlDouble)value);
				}
				if (type == typeof(SqlGuid))
				{
					return this.Adjust((SqlGuid)value);
				}
				if (type == typeof(SqlInt16))
				{
					return this.Adjust((SqlInt16)value);
				}
				if (type == typeof(SqlInt32))
				{
					return this.Adjust((SqlInt32)value);
				}
				if (type == typeof(SqlInt64))
				{
					return this.Adjust((SqlInt64)value);
				}
				if (type == typeof(SqlMoney))
				{
					return this.Adjust((SqlMoney)value);
				}
				if (type == typeof(SqlDecimal))
				{
					return this.Adjust((SqlDecimal)value);
				}
				if (type == typeof(SqlSingle))
				{
					return this.Adjust((SqlSingle)value);
				}
				if (type == typeof(SqlString))
				{
					return this.Adjust((SqlString)value);
				}
				if (type == typeof(SqlChars))
				{
					return this.Adjust((SqlChars)value);
				}
				if (type == typeof(SqlBytes))
				{
					return this.Adjust((SqlBytes)value);
				}
				if (type == typeof(SqlXml))
				{
					return this.Adjust((SqlXml)value);
				}
				if (type == typeof(TimeSpan))
				{
					return this.Adjust((TimeSpan)value);
				}
				if (type == typeof(DateTimeOffset))
				{
					return this.Adjust((DateTimeOffset)value);
				}
				throw ADP.UnknownDataType(type);
			case TypeCode.DBNull:
				return value;
			case TypeCode.Boolean:
				return this.Adjust((bool)value);
			case TypeCode.Char:
				return this.Adjust((char)value);
			case TypeCode.SByte:
				throw ADP.InvalidDataType(TypeCode.SByte);
			case TypeCode.Byte:
				return this.Adjust((byte)value);
			case TypeCode.Int16:
				return this.Adjust((short)value);
			case TypeCode.UInt16:
				throw ADP.InvalidDataType(TypeCode.UInt16);
			case TypeCode.Int32:
				return this.Adjust((int)value);
			case TypeCode.UInt32:
				throw ADP.InvalidDataType(TypeCode.UInt32);
			case TypeCode.Int64:
				return this.Adjust((long)value);
			case TypeCode.UInt64:
				throw ADP.InvalidDataType(TypeCode.UInt64);
			case TypeCode.Single:
				return this.Adjust((float)value);
			case TypeCode.Double:
				return this.Adjust((double)value);
			case TypeCode.Decimal:
				return this.Adjust((decimal)value);
			case TypeCode.DateTime:
				return this.Adjust((DateTime)value);
			case TypeCode.String:
				return this.Adjust((string)value);
			}
			throw ADP.UnknownDataTypeCode(type, Type.GetTypeCode(type));
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x0026B858 File Offset: 0x0026AC58
		internal static SqlMetaData GetNewSqlMetaDataFromOld(_SqlMetaData sqlMetaData, string name)
		{
			byte b;
			if (255 != sqlMetaData.precision)
			{
				b = sqlMetaData.precision;
			}
			else if (255 != sqlMetaData.metaType.Precision)
			{
				b = sqlMetaData.metaType.Precision;
			}
			else
			{
				b = 0;
			}
			byte b2;
			if (255 != sqlMetaData.scale)
			{
				b2 = sqlMetaData.scale;
			}
			else if (255 != sqlMetaData.metaType.Scale)
			{
				b2 = sqlMetaData.metaType.Scale;
			}
			else
			{
				b2 = 0;
			}
			if (sqlMetaData.metaType.SqlDbType == SqlDbType.TinyInt)
			{
				sqlMetaData.precision = 3;
				sqlMetaData.scale = 0;
			}
			else if (sqlMetaData.metaType.SqlDbType == SqlDbType.SmallInt)
			{
				sqlMetaData.precision = 5;
				sqlMetaData.scale = 0;
			}
			else if (sqlMetaData.metaType.SqlDbType == SqlDbType.Int)
			{
				sqlMetaData.precision = 10;
				sqlMetaData.scale = 0;
			}
			else if (sqlMetaData.metaType.SqlDbType == SqlDbType.BigInt)
			{
				sqlMetaData.precision = 19;
				sqlMetaData.scale = 0;
			}
			long num;
			if (SqlDbType.Variant == sqlMetaData.metaType.SqlDbType)
			{
				num = (long)sqlMetaData.length;
			}
			else if (sqlMetaData.metaType.IsPlp)
			{
				num = -1L;
			}
			else if (sqlMetaData.metaType.IsFixed)
			{
				num = (long)sqlMetaData.metaType.FixedLength;
			}
			else
			{
				num = (long)sqlMetaData.length;
			}
			if (num > 0L && (sqlMetaData.metaType.SqlDbType == SqlDbType.NChar || sqlMetaData.metaType.SqlDbType == SqlDbType.NVarChar || sqlMetaData.metaType.SqlDbType == SqlDbType.NText))
			{
				num /= 2L;
			}
			long num2;
			SqlCompareOptions sqlCompareOptions;
			if (sqlMetaData.collation != null)
			{
				num2 = (long)sqlMetaData.collation.LCID;
				sqlCompareOptions = sqlMetaData.collation.SqlCompareOptions;
			}
			else
			{
				num2 = 0L;
				sqlCompareOptions = SqlCompareOptions.None;
			}
			SqlMetaData sqlMetaData2;
			if (sqlMetaData.metaType.SqlDbType == SqlDbType.Udt)
			{
				SqlConnection.CheckGetExtendedUDTInfo(sqlMetaData, true);
				sqlMetaData2 = new SqlMetaData(name, SqlDbType.Udt, sqlMetaData.udtType);
			}
			else
			{
				sqlMetaData2 = new SqlMetaData(name, sqlMetaData.metaType.SqlDbType, num, b, b2, num2, sqlCompareOptions, sqlMetaData.xmlSchemaCollectionDatabase, sqlMetaData.xmlSchemaCollectionOwningSchema, sqlMetaData.xmlSchemaCollectionName, sqlMetaData.metaType.IsPlp, null);
			}
			return sqlMetaData2;
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x0026BA60 File Offset: 0x0026AE60
		public static SqlMetaData InferFromValue(object value, string name)
		{
			if (value == null)
			{
				throw ADP.ArgumentNull("value");
			}
			Type type = value.GetType();
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Empty:
				throw ADP.InvalidDataType(TypeCode.Empty);
			case TypeCode.Object:
				if (type == typeof(byte[]))
				{
					long num = (long)((byte[])value).Length;
					if (num < 1L)
					{
						num = 1L;
					}
					if (8000L < num)
					{
						num = SqlMetaData.Max;
					}
					return new SqlMetaData(name, SqlDbType.VarBinary, num);
				}
				if (type == typeof(char[]))
				{
					long num2 = (long)((char[])value).Length;
					if (num2 < 1L)
					{
						num2 = 1L;
					}
					if (4000L < num2)
					{
						num2 = SqlMetaData.Max;
					}
					return new SqlMetaData(name, SqlDbType.NVarChar, num2);
				}
				if (type == typeof(Guid))
				{
					return new SqlMetaData(name, SqlDbType.UniqueIdentifier);
				}
				if (type == typeof(object))
				{
					return new SqlMetaData(name, SqlDbType.Variant);
				}
				if (type == typeof(SqlBinary))
				{
					SqlBinary sqlBinary = (SqlBinary)value;
					long num3;
					if (!sqlBinary.IsNull)
					{
						num3 = (long)sqlBinary.Length;
						if (num3 < 1L)
						{
							num3 = 1L;
						}
						if (8000L < num3)
						{
							num3 = SqlMetaData.Max;
						}
					}
					else
					{
						num3 = SqlMetaData.sxm_rgDefaults[21].MaxLength;
					}
					return new SqlMetaData(name, SqlDbType.VarBinary, num3);
				}
				if (type == typeof(SqlBoolean))
				{
					return new SqlMetaData(name, SqlDbType.Bit);
				}
				if (type == typeof(SqlByte))
				{
					return new SqlMetaData(name, SqlDbType.TinyInt);
				}
				if (type == typeof(SqlDateTime))
				{
					return new SqlMetaData(name, SqlDbType.DateTime);
				}
				if (type == typeof(SqlDouble))
				{
					return new SqlMetaData(name, SqlDbType.Float);
				}
				if (type == typeof(SqlGuid))
				{
					return new SqlMetaData(name, SqlDbType.UniqueIdentifier);
				}
				if (type == typeof(SqlInt16))
				{
					return new SqlMetaData(name, SqlDbType.SmallInt);
				}
				if (type == typeof(SqlInt32))
				{
					return new SqlMetaData(name, SqlDbType.Int);
				}
				if (type == typeof(SqlInt64))
				{
					return new SqlMetaData(name, SqlDbType.BigInt);
				}
				if (type == typeof(SqlMoney))
				{
					return new SqlMetaData(name, SqlDbType.Money);
				}
				if (type == typeof(SqlDecimal))
				{
					SqlDecimal sqlDecimal = (SqlDecimal)value;
					byte b;
					byte b2;
					if (!sqlDecimal.IsNull)
					{
						b = sqlDecimal.Precision;
						b2 = sqlDecimal.Scale;
					}
					else
					{
						b = SqlMetaData.sxm_rgDefaults[5].Precision;
						b2 = SqlMetaData.sxm_rgDefaults[5].Scale;
					}
					return new SqlMetaData(name, SqlDbType.Decimal, b, b2);
				}
				if (type == typeof(SqlSingle))
				{
					return new SqlMetaData(name, SqlDbType.Real);
				}
				if (type == typeof(SqlString))
				{
					SqlString sqlString = (SqlString)value;
					if (!sqlString.IsNull)
					{
						long num4 = (long)sqlString.Value.Length;
						if (num4 < 1L)
						{
							num4 = 1L;
						}
						if (num4 > 4000L)
						{
							num4 = SqlMetaData.Max;
						}
						return new SqlMetaData(name, SqlDbType.NVarChar, num4, (long)sqlString.LCID, sqlString.SqlCompareOptions);
					}
					return new SqlMetaData(name, SqlDbType.NVarChar, SqlMetaData.sxm_rgDefaults[12].MaxLength);
				}
				else
				{
					if (type == typeof(SqlChars))
					{
						SqlChars sqlChars = (SqlChars)value;
						long num5;
						if (!sqlChars.IsNull)
						{
							num5 = sqlChars.Length;
							if (num5 < 1L)
							{
								num5 = 1L;
							}
							if (num5 > 4000L)
							{
								num5 = SqlMetaData.Max;
							}
						}
						else
						{
							num5 = SqlMetaData.sxm_rgDefaults[12].MaxLength;
						}
						return new SqlMetaData(name, SqlDbType.NVarChar, num5);
					}
					if (type == typeof(SqlBytes))
					{
						SqlBytes sqlBytes = (SqlBytes)value;
						long num6;
						if (!sqlBytes.IsNull)
						{
							num6 = sqlBytes.Length;
							if (num6 < 1L)
							{
								num6 = 1L;
							}
							else if (8000L < num6)
							{
								num6 = SqlMetaData.Max;
							}
						}
						else
						{
							num6 = SqlMetaData.sxm_rgDefaults[21].MaxLength;
						}
						return new SqlMetaData(name, SqlDbType.VarBinary, num6);
					}
					if (type == typeof(SqlXml))
					{
						return new SqlMetaData(name, SqlDbType.Xml);
					}
					if (type == typeof(TimeSpan))
					{
						return new SqlMetaData(name, SqlDbType.Time, 0, SqlMetaData.InferScaleFromTimeTicks(((TimeSpan)value).Ticks));
					}
					if (type == typeof(DateTimeOffset))
					{
						return new SqlMetaData(name, SqlDbType.DateTimeOffset, 0, SqlMetaData.InferScaleFromTimeTicks(((DateTimeOffset)value).Ticks));
					}
					throw ADP.UnknownDataType(type);
				}
				break;
			case TypeCode.DBNull:
				throw ADP.InvalidDataType(TypeCode.DBNull);
			case TypeCode.Boolean:
				return new SqlMetaData(name, SqlDbType.Bit);
			case TypeCode.Char:
				return new SqlMetaData(name, SqlDbType.NVarChar, 1L);
			case TypeCode.SByte:
				throw ADP.InvalidDataType(TypeCode.SByte);
			case TypeCode.Byte:
				return new SqlMetaData(name, SqlDbType.TinyInt);
			case TypeCode.Int16:
				return new SqlMetaData(name, SqlDbType.SmallInt);
			case TypeCode.UInt16:
				throw ADP.InvalidDataType(TypeCode.UInt16);
			case TypeCode.Int32:
				return new SqlMetaData(name, SqlDbType.Int);
			case TypeCode.UInt32:
				throw ADP.InvalidDataType(TypeCode.UInt32);
			case TypeCode.Int64:
				return new SqlMetaData(name, SqlDbType.BigInt);
			case TypeCode.UInt64:
				throw ADP.InvalidDataType(TypeCode.UInt64);
			case TypeCode.Single:
				return new SqlMetaData(name, SqlDbType.Real);
			case TypeCode.Double:
				return new SqlMetaData(name, SqlDbType.Float);
			case TypeCode.Decimal:
			{
				SqlDecimal sqlDecimal2 = new SqlDecimal((decimal)value);
				return new SqlMetaData(name, SqlDbType.Decimal, sqlDecimal2.Precision, sqlDecimal2.Scale);
			}
			case TypeCode.DateTime:
				return new SqlMetaData(name, SqlDbType.DateTime);
			case TypeCode.String:
			{
				long num7 = (long)((string)value).Length;
				if (num7 < 1L)
				{
					num7 = 1L;
				}
				if (4000L < num7)
				{
					num7 = SqlMetaData.Max;
				}
				return new SqlMetaData(name, SqlDbType.NVarChar, num7);
			}
			}
			throw ADP.UnknownDataTypeCode(type, Type.GetTypeCode(type));
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x0026C038 File Offset: 0x0026B438
		public bool Adjust(bool value)
		{
			if (SqlDbType.Bit != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			return value;
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x0026C054 File Offset: 0x0026B454
		public byte Adjust(byte value)
		{
			if (SqlDbType.TinyInt != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			return value;
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x0026C074 File Offset: 0x0026B474
		public byte[] Adjust(byte[] value)
		{
			if (SqlDbType.Binary == this.SqlDbType || SqlDbType.Timestamp == this.SqlDbType)
			{
				if (value != null && (long)value.Length < this.MaxLength)
				{
					byte[] array = new byte[this.MaxLength];
					Array.Copy(value, array, value.Length);
					Array.Clear(array, value.Length, array.Length - value.Length);
					return array;
				}
			}
			else if (SqlDbType.VarBinary != this.SqlDbType && SqlDbType.Image != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			if (value == null)
			{
				return null;
			}
			if ((long)value.Length > this.MaxLength && SqlMetaData.Max != this.MaxLength)
			{
				byte[] array2 = new byte[this.MaxLength];
				Array.Copy(value, array2, (int)this.MaxLength);
				value = array2;
			}
			return value;
		}

		// Token: 0x06002213 RID: 8723 RVA: 0x0026C124 File Offset: 0x0026B524
		public char Adjust(char value)
		{
			if (SqlDbType.Char == this.SqlDbType || SqlDbType.NChar == this.SqlDbType)
			{
				if (1L != this.MaxLength)
				{
					SqlMetaData.ThrowInvalidType();
				}
			}
			else if (1L > this.MaxLength || (SqlDbType.VarChar != this.SqlDbType && SqlDbType.NVarChar != this.SqlDbType && SqlDbType.Text != this.SqlDbType && SqlDbType.NText != this.SqlDbType))
			{
				SqlMetaData.ThrowInvalidType();
			}
			return value;
		}

		// Token: 0x06002214 RID: 8724 RVA: 0x0026C190 File Offset: 0x0026B590
		public char[] Adjust(char[] value)
		{
			if (SqlDbType.Char == this.SqlDbType || SqlDbType.NChar == this.SqlDbType)
			{
				if (value != null)
				{
					long num = (long)value.Length;
					if (num < this.MaxLength)
					{
						char[] array = new char[(int)this.MaxLength];
						Array.Copy(value, array, (int)num);
						for (long num2 = num; num2 < (long)array.Length; num2 += 1L)
						{
							array[(int)(checked((IntPtr)num2))] = ' ';
						}
						return array;
					}
				}
			}
			else if (SqlDbType.VarChar != this.SqlDbType && SqlDbType.NVarChar != this.SqlDbType && SqlDbType.Text != this.SqlDbType && SqlDbType.NText != this.SqlDbType)
			{
				SqlMetaData.ThrowInvalidType();
			}
			if (value == null)
			{
				return null;
			}
			if ((long)value.Length > this.MaxLength && SqlMetaData.Max != this.MaxLength)
			{
				char[] array2 = new char[this.MaxLength];
				Array.Copy(value, array2, (int)this.MaxLength);
				value = array2;
			}
			return value;
		}

		// Token: 0x06002215 RID: 8725 RVA: 0x0026C25C File Offset: 0x0026B65C
		internal static SqlMetaData GetPartialLengthMetaData(SqlMetaData md)
		{
			if (md.IsPartialLength)
			{
				return md;
			}
			if (md.SqlDbType == SqlDbType.Xml)
			{
				SqlMetaData.ThrowInvalidType();
			}
			if (md.SqlDbType == SqlDbType.NVarChar || md.SqlDbType == SqlDbType.VarChar || md.SqlDbType == SqlDbType.VarBinary)
			{
				return new SqlMetaData(md.Name, md.SqlDbType, SqlMetaData.Max, 0, 0, md.LocaleId, md.CompareOptions, null, null, null, true, md.Type);
			}
			return md;
		}

		// Token: 0x06002216 RID: 8726 RVA: 0x0026C2D4 File Offset: 0x0026B6D4
		private static void ThrowInvalidType()
		{
			throw ADP.InvalidMetaDataValue();
		}

		// Token: 0x06002217 RID: 8727 RVA: 0x0026C2E8 File Offset: 0x0026B6E8
		private void VerifyDateTimeRange(DateTime value)
		{
			if (SqlDbType.SmallDateTime == this.SqlDbType && (SqlMetaData.x_dtSmallMax < value || SqlMetaData.x_dtSmallMin > value))
			{
				SqlMetaData.ThrowInvalidType();
			}
		}

		// Token: 0x06002218 RID: 8728 RVA: 0x0026C320 File Offset: 0x0026B720
		private void VerifyMoneyRange(SqlMoney value)
		{
			if (SqlDbType.SmallMoney == this.SqlDbType && ((SqlMetaData.x_smSmallMax < value).Value || (SqlMetaData.x_smSmallMin > value).Value))
			{
				SqlMetaData.ThrowInvalidType();
			}
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x0026C368 File Offset: 0x0026B768
		private SqlDecimal InternalAdjustSqlDecimal(SqlDecimal value)
		{
			if (!value.IsNull && (value.Precision != this.Precision || value.Scale != this.Scale))
			{
				if (value.Scale != this.Scale)
				{
					value = SqlDecimal.AdjustScale(value, (int)(this.Scale - value.Scale), false);
				}
				return SqlDecimal.ConvertToPrecScale(value, (int)this.Precision, (int)this.Scale);
			}
			return value;
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x0026C3D8 File Offset: 0x0026B7D8
		private void VerifyTimeRange(TimeSpan value)
		{
			if (SqlDbType.Time == this.SqlDbType && (SqlMetaData.x_timeMin > value || value > SqlMetaData.x_timeMax))
			{
				SqlMetaData.ThrowInvalidType();
			}
		}

		// Token: 0x0600221B RID: 8731 RVA: 0x0026C410 File Offset: 0x0026B810
		private long InternalAdjustTimeTicks(long ticks)
		{
			return ticks / SqlMetaData.__unitTicksFromScale[(int)this.Scale] * SqlMetaData.__unitTicksFromScale[(int)this.Scale];
		}

		// Token: 0x0600221C RID: 8732 RVA: 0x0026C438 File Offset: 0x0026B838
		private static byte InferScaleFromTimeTicks(long ticks)
		{
			for (byte b = 0; b < 7; b += 1)
			{
				if (ticks / SqlMetaData.__unitTicksFromScale[(int)b] * SqlMetaData.__unitTicksFromScale[(int)b] == ticks)
				{
					return b;
				}
			}
			return 7;
		}

		// Token: 0x0600221D RID: 8733 RVA: 0x0026C46C File Offset: 0x0026B86C
		private void SetDefaultsForType(SqlDbType dbType)
		{
			if (SqlDbType.BigInt <= dbType && SqlDbType.DateTimeOffset >= dbType)
			{
				SqlMetaData sqlMetaData = SqlMetaData.sxm_rgDefaults[(int)dbType];
				this.m_sqlDbType = dbType;
				this.m_lMaxLength = sqlMetaData.MaxLength;
				this.m_bPrecision = sqlMetaData.Precision;
				this.m_bScale = sqlMetaData.Scale;
				this.m_lLocale = sqlMetaData.LocaleId;
				this.m_eCompareOptions = sqlMetaData.CompareOptions;
			}
		}

		// Token: 0x0400161E RID: 5662
		private const long x_lMax = -1L;

		// Token: 0x0400161F RID: 5663
		private const long x_lServerMaxUnicode = 4000L;

		// Token: 0x04001620 RID: 5664
		private const long x_lServerMaxANSI = 8000L;

		// Token: 0x04001621 RID: 5665
		private const long x_lServerMaxBinary = 8000L;

		// Token: 0x04001622 RID: 5666
		private const bool x_defaultUseServerDefault = false;

		// Token: 0x04001623 RID: 5667
		private const bool x_defaultIsUniqueKey = false;

		// Token: 0x04001624 RID: 5668
		private const SortOrder x_defaultColumnSortOrder = SortOrder.Unspecified;

		// Token: 0x04001625 RID: 5669
		private const int x_defaultSortOrdinal = -1;

		// Token: 0x04001626 RID: 5670
		private const SqlCompareOptions x_eDefaultStringCompareOptions = SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth;

		// Token: 0x04001627 RID: 5671
		private const byte MaxTimeScale = 7;

		// Token: 0x04001628 RID: 5672
		private string m_strName;

		// Token: 0x04001629 RID: 5673
		private long m_lMaxLength;

		// Token: 0x0400162A RID: 5674
		private SqlDbType m_sqlDbType;

		// Token: 0x0400162B RID: 5675
		private byte m_bPrecision;

		// Token: 0x0400162C RID: 5676
		private byte m_bScale;

		// Token: 0x0400162D RID: 5677
		private long m_lLocale;

		// Token: 0x0400162E RID: 5678
		private SqlCompareOptions m_eCompareOptions;

		// Token: 0x0400162F RID: 5679
		private string m_XmlSchemaCollectionDatabase;

		// Token: 0x04001630 RID: 5680
		private string m_XmlSchemaCollectionOwningSchema;

		// Token: 0x04001631 RID: 5681
		private string m_XmlSchemaCollectionName;

		// Token: 0x04001632 RID: 5682
		private string m_serverTypeName;

		// Token: 0x04001633 RID: 5683
		private bool m_bPartialLength;

		// Token: 0x04001634 RID: 5684
		private Type m_udttype;

		// Token: 0x04001635 RID: 5685
		private bool m_useServerDefault;

		// Token: 0x04001636 RID: 5686
		private bool m_isUniqueKey;

		// Token: 0x04001637 RID: 5687
		private SortOrder m_columnSortOrder;

		// Token: 0x04001638 RID: 5688
		private int m_sortOrdinal;

		// Token: 0x04001639 RID: 5689
		private static byte[] __maxLenFromPrecision = new byte[]
		{
			5, 5, 5, 5, 5, 5, 5, 5, 5, 9,
			9, 9, 9, 9, 9, 9, 9, 9, 9, 13,
			13, 13, 13, 13, 13, 13, 13, 13, 17, 17,
			17, 17, 17, 17, 17, 17, 17, 17
		};

		// Token: 0x0400163A RID: 5690
		private static byte[] __maxVarTimeLenOffsetFromScale = new byte[] { 2, 2, 2, 1, 1, 0, 0, 0 };

		// Token: 0x0400163B RID: 5691
		private static readonly DateTime x_dtSmallMax = new DateTime(2079, 6, 6, 23, 59, 29, 998);

		// Token: 0x0400163C RID: 5692
		private static readonly DateTime x_dtSmallMin = new DateTime(1899, 12, 31, 23, 59, 29, 999);

		// Token: 0x0400163D RID: 5693
		private static readonly SqlMoney x_smSmallMax = new SqlMoney(214748.3647m);

		// Token: 0x0400163E RID: 5694
		private static readonly SqlMoney x_smSmallMin = new SqlMoney(-214748.3648m);

		// Token: 0x0400163F RID: 5695
		private static readonly TimeSpan x_timeMin = TimeSpan.Zero;

		// Token: 0x04001640 RID: 5696
		private static readonly TimeSpan x_timeMax = new TimeSpan(863999999999L);

		// Token: 0x04001641 RID: 5697
		private static readonly long[] __unitTicksFromScale = new long[] { 10000000L, 1000000L, 100000L, 10000L, 1000L, 100L, 10L, 1L };

		// Token: 0x04001642 RID: 5698
		private static DbType[] sxm_rgSqlDbTypeToDbType = new DbType[]
		{
			DbType.Int64,
			DbType.Binary,
			DbType.Boolean,
			DbType.AnsiString,
			DbType.DateTime,
			DbType.Decimal,
			DbType.Double,
			DbType.Binary,
			DbType.Int32,
			DbType.Currency,
			DbType.String,
			DbType.String,
			DbType.String,
			DbType.Single,
			DbType.Guid,
			DbType.DateTime,
			DbType.Int16,
			DbType.Currency,
			DbType.AnsiString,
			DbType.Binary,
			DbType.Byte,
			DbType.Binary,
			DbType.AnsiString,
			DbType.Object,
			DbType.Object,
			DbType.Xml,
			DbType.String,
			DbType.String,
			DbType.String,
			DbType.Object,
			DbType.Object,
			DbType.Date,
			DbType.Time,
			DbType.DateTime2,
			DbType.DateTimeOffset
		};

		// Token: 0x04001643 RID: 5699
		internal static SqlMetaData[] sxm_rgDefaults = new SqlMetaData[]
		{
			new SqlMetaData("bigint", SqlDbType.BigInt, 8L, 19, 0, 0L, SqlCompareOptions.None, false),
			new SqlMetaData("binary", SqlDbType.Binary, 1L, 0, 0, 0L, SqlCompareOptions.None, false),
			new SqlMetaData("bit", SqlDbType.Bit, 1L, 1, 0, 0L, SqlCompareOptions.None, false),
			new SqlMetaData("char", SqlDbType.Char, 1L, 0, 0, 0L, SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth, false),
			new SqlMetaData("datetime", SqlDbType.DateTime, 8L, 23, 3, 0L, SqlCompareOptions.None, false),
			new SqlMetaData("decimal", SqlDbType.Decimal, 9L, 18, 0, 0L, SqlCompareOptions.None, false),
			new SqlMetaData("float", SqlDbType.Float, 8L, 53, 0, 0L, SqlCompareOptions.None, false),
			new SqlMetaData("image", SqlDbType.Image, -1L, 0, 0, 0L, SqlCompareOptions.None, false),
			new SqlMetaData("int", SqlDbType.Int, 4L, 10, 0, 0L, SqlCompareOptions.None, false),
			new SqlMetaData("money", SqlDbType.Money, 8L, 19, 4, 0L, SqlCompareOptions.None, false),
			new SqlMetaData("nchar", SqlDbType.NChar, 1L, 0, 0, 0L, SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth, false),
			new SqlMetaData("ntext", SqlDbType.NText, -1L, 0, 0, 0L, SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth, false),
			new SqlMetaData("nvarchar", SqlDbType.NVarChar, 4000L, 0, 0, 0L, SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth, false),
			new SqlMetaData("real", SqlDbType.Real, 4L, 24, 0, 0L, SqlCompareOptions.None, false),
			new SqlMetaData("uniqueidentifier", SqlDbType.UniqueIdentifier, 16L, 0, 0, 0L, SqlCompareOptions.None, false),
			new SqlMetaData("smalldatetime", SqlDbType.SmallDateTime, 4L, 16, 0, 0L, SqlCompareOptions.None, false),
			new SqlMetaData("smallint", SqlDbType.SmallInt, 2L, 5, 0, 0L, SqlCompareOptions.None, false),
			new SqlMetaData("smallmoney", SqlDbType.SmallMoney, 4L, 10, 4, 0L, SqlCompareOptions.None, false),
			new SqlMetaData("text", SqlDbType.Text, -1L, 0, 0, 0L, SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth, false),
			new SqlMetaData("timestamp", SqlDbType.Timestamp, 8L, 0, 0, 0L, SqlCompareOptions.None, false),
			new SqlMetaData("tinyint", SqlDbType.TinyInt, 1L, 3, 0, 0L, SqlCompareOptions.None, false),
			new SqlMetaData("varbinary", SqlDbType.VarBinary, 8000L, 0, 0, 0L, SqlCompareOptions.None, false),
			new SqlMetaData("varchar", SqlDbType.VarChar, 8000L, 0, 0, 0L, SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth, false),
			new SqlMetaData("sql_variant", SqlDbType.Variant, 8016L, 0, 0, 0L, SqlCompareOptions.None, false),
			new SqlMetaData("nvarchar", SqlDbType.NVarChar, 1L, 0, 0, 0L, SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth, false),
			new SqlMetaData("xml", SqlDbType.Xml, -1L, 0, 0, 0L, SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth, true),
			new SqlMetaData("nvarchar", SqlDbType.NVarChar, 1L, 0, 0, 0L, SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth, false),
			new SqlMetaData("nvarchar", SqlDbType.NVarChar, 4000L, 0, 0, 0L, SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth, false),
			new SqlMetaData("nvarchar", SqlDbType.NVarChar, 4000L, 0, 0, 0L, SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth, false),
			new SqlMetaData("udt", SqlDbType.Udt, 0L, 0, 0, 0L, SqlCompareOptions.None, false),
			new SqlMetaData("table", SqlDbType.Structured, 0L, 0, 0, 0L, SqlCompareOptions.None, false),
			new SqlMetaData("date", SqlDbType.Date, 3L, 10, 0, 0L, SqlCompareOptions.None, false),
			new SqlMetaData("time", SqlDbType.Time, 5L, 0, 7, 0L, SqlCompareOptions.None, false),
			new SqlMetaData("datetime2", SqlDbType.DateTime2, 8L, 0, 7, 0L, SqlCompareOptions.None, false),
			new SqlMetaData("datetimeoffset", SqlDbType.DateTimeOffset, 10L, 0, 7, 0L, SqlCompareOptions.None, false)
		};
	}
}
