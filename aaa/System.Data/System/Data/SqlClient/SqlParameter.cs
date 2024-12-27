using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Globalization;
using System.Reflection;
using System.Xml;
using Microsoft.SqlServer.Server;

namespace System.Data.SqlClient
{
	// Token: 0x02000305 RID: 773
	[TypeConverter(typeof(SqlParameter.SqlParameterConverter))]
	public sealed class SqlParameter : DbParameter, IDbDataParameter, IDataParameter, ICloneable
	{
		// Token: 0x0600281E RID: 10270 RVA: 0x0028D9C4 File Offset: 0x0028CDC4
		public SqlParameter()
		{
		}

		// Token: 0x0600281F RID: 10271 RVA: 0x0028D9D8 File Offset: 0x0028CDD8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public SqlParameter(string parameterName, SqlDbType dbType, int size, ParameterDirection direction, bool isNullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
			: this()
		{
			this.ParameterName = parameterName;
			this.SqlDbType = dbType;
			this.Size = size;
			this.Direction = direction;
			this.IsNullable = isNullable;
			this.PrecisionInternal = precision;
			this.ScaleInternal = scale;
			this.SourceColumn = sourceColumn;
			this.SourceVersion = sourceVersion;
			this.Value = value;
		}

		// Token: 0x06002820 RID: 10272 RVA: 0x0028DA38 File Offset: 0x0028CE38
		public SqlParameter(string parameterName, SqlDbType dbType, int size, ParameterDirection direction, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, bool sourceColumnNullMapping, object value, string xmlSchemaCollectionDatabase, string xmlSchemaCollectionOwningSchema, string xmlSchemaCollectionName)
		{
			this.ParameterName = parameterName;
			this.SqlDbType = dbType;
			this.Size = size;
			this.Direction = direction;
			this.PrecisionInternal = precision;
			this.ScaleInternal = scale;
			this.SourceColumn = sourceColumn;
			this.SourceVersion = sourceVersion;
			this.SourceColumnNullMapping = sourceColumnNullMapping;
			this.Value = value;
			this._xmlSchemaCollectionDatabase = xmlSchemaCollectionDatabase;
			this._xmlSchemaCollectionOwningSchema = xmlSchemaCollectionOwningSchema;
			this._xmlSchemaCollectionName = xmlSchemaCollectionName;
		}

		// Token: 0x06002821 RID: 10273 RVA: 0x0028DAB0 File Offset: 0x0028CEB0
		public SqlParameter(string parameterName, SqlDbType dbType)
			: this()
		{
			this.ParameterName = parameterName;
			this.SqlDbType = dbType;
		}

		// Token: 0x06002822 RID: 10274 RVA: 0x0028DAD4 File Offset: 0x0028CED4
		public SqlParameter(string parameterName, object value)
			: this()
		{
			this.ParameterName = parameterName;
			this.Value = value;
		}

		// Token: 0x06002823 RID: 10275 RVA: 0x0028DAF8 File Offset: 0x0028CEF8
		public SqlParameter(string parameterName, SqlDbType dbType, int size)
			: this()
		{
			this.ParameterName = parameterName;
			this.SqlDbType = dbType;
			this.Size = size;
		}

		// Token: 0x06002824 RID: 10276 RVA: 0x0028DB20 File Offset: 0x0028CF20
		public SqlParameter(string parameterName, SqlDbType dbType, int size, string sourceColumn)
			: this()
		{
			this.ParameterName = parameterName;
			this.SqlDbType = dbType;
			this.Size = size;
			this.SourceColumn = sourceColumn;
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06002825 RID: 10277 RVA: 0x0028DB50 File Offset: 0x0028CF50
		// (set) Token: 0x06002826 RID: 10278 RVA: 0x0028DB64 File Offset: 0x0028CF64
		internal SqlCollation Collation
		{
			get
			{
				return this._collation;
			}
			set
			{
				this._collation = value;
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06002827 RID: 10279 RVA: 0x0028DB78 File Offset: 0x0028CF78
		// (set) Token: 0x06002828 RID: 10280 RVA: 0x0028DB98 File Offset: 0x0028CF98
		[Browsable(false)]
		public SqlCompareOptions CompareInfo
		{
			get
			{
				SqlCollation collation = this._collation;
				if (collation != null)
				{
					return collation.SqlCompareOptions;
				}
				return SqlCompareOptions.None;
			}
			set
			{
				SqlCollation sqlCollation = this._collation;
				if (sqlCollation == null)
				{
					sqlCollation = (this._collation = new SqlCollation());
				}
				if ((value & SqlString.x_iValidSqlCompareOptionMask) != value)
				{
					throw ADP.ArgumentOutOfRange("CompareInfo");
				}
				sqlCollation.SqlCompareOptions = value;
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06002829 RID: 10281 RVA: 0x0028DBD8 File Offset: 0x0028CFD8
		// (set) Token: 0x0600282A RID: 10282 RVA: 0x0028DBF8 File Offset: 0x0028CFF8
		[ResCategory("DataCategory_Xml")]
		[ResDescription("SqlParameter_XmlSchemaCollectionDatabase")]
		public string XmlSchemaCollectionDatabase
		{
			get
			{
				string xmlSchemaCollectionDatabase = this._xmlSchemaCollectionDatabase;
				if (xmlSchemaCollectionDatabase == null)
				{
					return ADP.StrEmpty;
				}
				return xmlSchemaCollectionDatabase;
			}
			set
			{
				this._xmlSchemaCollectionDatabase = value;
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x0600282B RID: 10283 RVA: 0x0028DC0C File Offset: 0x0028D00C
		// (set) Token: 0x0600282C RID: 10284 RVA: 0x0028DC2C File Offset: 0x0028D02C
		[ResDescription("SqlParameter_XmlSchemaCollectionOwningSchema")]
		[ResCategory("DataCategory_Xml")]
		public string XmlSchemaCollectionOwningSchema
		{
			get
			{
				string xmlSchemaCollectionOwningSchema = this._xmlSchemaCollectionOwningSchema;
				if (xmlSchemaCollectionOwningSchema == null)
				{
					return ADP.StrEmpty;
				}
				return xmlSchemaCollectionOwningSchema;
			}
			set
			{
				this._xmlSchemaCollectionOwningSchema = value;
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x0600282D RID: 10285 RVA: 0x0028DC40 File Offset: 0x0028D040
		// (set) Token: 0x0600282E RID: 10286 RVA: 0x0028DC60 File Offset: 0x0028D060
		[ResCategory("DataCategory_Xml")]
		[ResDescription("SqlParameter_XmlSchemaCollectionName")]
		public string XmlSchemaCollectionName
		{
			get
			{
				string xmlSchemaCollectionName = this._xmlSchemaCollectionName;
				if (xmlSchemaCollectionName == null)
				{
					return ADP.StrEmpty;
				}
				return xmlSchemaCollectionName;
			}
			set
			{
				this._xmlSchemaCollectionName = value;
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x0600282F RID: 10287 RVA: 0x0028DC74 File Offset: 0x0028D074
		// (set) Token: 0x06002830 RID: 10288 RVA: 0x0028DC8C File Offset: 0x0028D08C
		public override DbType DbType
		{
			get
			{
				return this.GetMetaTypeOnly().DbType;
			}
			set
			{
				MetaType metaType = this._metaType;
				if (metaType == null || metaType.DbType != value || value == DbType.Date || value == DbType.Time)
				{
					this.PropertyTypeChanging();
					this._metaType = MetaType.GetMetaTypeFromDbType(value);
				}
			}
		}

		// Token: 0x06002831 RID: 10289 RVA: 0x0028DCC8 File Offset: 0x0028D0C8
		public override void ResetDbType()
		{
			this.ResetSqlDbType();
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x06002832 RID: 10290 RVA: 0x0028DCDC File Offset: 0x0028D0DC
		// (set) Token: 0x06002833 RID: 10291 RVA: 0x0028DCF0 File Offset: 0x0028D0F0
		internal MetaType InternalMetaType
		{
			get
			{
				return this._internalMetaType;
			}
			set
			{
				this._internalMetaType = value;
			}
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x06002834 RID: 10292 RVA: 0x0028DD04 File Offset: 0x0028D104
		// (set) Token: 0x06002835 RID: 10293 RVA: 0x0028DD24 File Offset: 0x0028D124
		[Browsable(false)]
		public int LocaleId
		{
			get
			{
				SqlCollation collation = this._collation;
				if (collation != null)
				{
					return collation.LCID;
				}
				return 0;
			}
			set
			{
				SqlCollation sqlCollation = this._collation;
				if (sqlCollation == null)
				{
					sqlCollation = (this._collation = new SqlCollation());
				}
				if ((long)value != (1048575L & (long)value))
				{
					throw ADP.ArgumentOutOfRange("LocaleId");
				}
				sqlCollation.LCID = value;
			}
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06002836 RID: 10294 RVA: 0x0028DD68 File Offset: 0x0028D168
		private SqlMetaData MetaData
		{
			get
			{
				MetaType metaTypeOnly = this.GetMetaTypeOnly();
				long num;
				if (metaTypeOnly.IsFixed)
				{
					num = (long)metaTypeOnly.FixedLength;
				}
				else if (this.Size > 0 || this.Size < 0)
				{
					num = (long)this.Size;
				}
				else
				{
					num = SmiMetaData.GetDefaultForType(metaTypeOnly.SqlDbType).MaxLength;
				}
				return new SqlMetaData(this.ParameterName, metaTypeOnly.SqlDbType, num, this.GetActualPrecision(), this.GetActualScale(), (long)this.LocaleId, this.CompareInfo, this.XmlSchemaCollectionDatabase, this.XmlSchemaCollectionOwningSchema, this.XmlSchemaCollectionName, metaTypeOnly.IsPlp, this._udtType);
			}
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06002837 RID: 10295 RVA: 0x0028DE04 File Offset: 0x0028D204
		internal bool SizeInferred
		{
			get
			{
				return 0 == this._size;
			}
		}

		// Token: 0x06002838 RID: 10296 RVA: 0x0028DE1C File Offset: 0x0028D21C
		internal SmiParameterMetaData MetaDataForSmi(out ParameterPeekAheadValue peekAhead)
		{
			peekAhead = null;
			MetaType metaType = this.ValidateTypeLengths(true);
			long num = (long)this.GetActualSize();
			long num2 = (long)this.Size;
			if (!metaType.IsLong)
			{
				if (SqlDbType.NChar == metaType.SqlDbType || SqlDbType.NVarChar == metaType.SqlDbType)
				{
					num /= 2L;
				}
				if (num > num2)
				{
					num2 = num;
				}
			}
			if (0L == num2)
			{
				if (SqlDbType.Binary == metaType.SqlDbType || SqlDbType.VarBinary == metaType.SqlDbType)
				{
					num2 = 8000L;
				}
				else if (SqlDbType.Char == metaType.SqlDbType || SqlDbType.VarChar == metaType.SqlDbType)
				{
					num2 = 8000L;
				}
				else if (SqlDbType.NChar == metaType.SqlDbType || SqlDbType.NVarChar == metaType.SqlDbType)
				{
					num2 = 4000L;
				}
			}
			else if ((num2 > 8000L && (SqlDbType.Binary == metaType.SqlDbType || SqlDbType.VarBinary == metaType.SqlDbType)) || (num2 > 8000L && (SqlDbType.Char == metaType.SqlDbType || SqlDbType.VarChar == metaType.SqlDbType)) || (num2 > 4000L && (SqlDbType.NChar == metaType.SqlDbType || SqlDbType.NVarChar == metaType.SqlDbType)))
			{
				num2 = -1L;
			}
			int num3 = this.LocaleId;
			if (num3 == 0 && metaType.IsCharType)
			{
				object coercedValue = this.GetCoercedValue();
				if (coercedValue is SqlString && !((SqlString)coercedValue).IsNull)
				{
					num3 = ((SqlString)coercedValue).LCID;
				}
				else
				{
					num3 = CultureInfo.CurrentCulture.LCID;
				}
			}
			SqlCompareOptions sqlCompareOptions = this.CompareInfo;
			if (sqlCompareOptions == SqlCompareOptions.None && metaType.IsCharType)
			{
				object coercedValue2 = this.GetCoercedValue();
				if (coercedValue2 is SqlString && !((SqlString)coercedValue2).IsNull)
				{
					sqlCompareOptions = ((SqlString)coercedValue2).SqlCompareOptions;
				}
				else
				{
					sqlCompareOptions = SmiMetaData.GetDefaultForType(metaType.SqlDbType).CompareOptions;
				}
			}
			string text = null;
			string text2 = null;
			string text3 = null;
			if (SqlDbType.Xml == metaType.SqlDbType)
			{
				text = this.XmlSchemaCollectionDatabase;
				text2 = this.XmlSchemaCollectionOwningSchema;
				text3 = this.XmlSchemaCollectionName;
			}
			else if (SqlDbType.Udt == metaType.SqlDbType || (SqlDbType.Structured == metaType.SqlDbType && !ADP.IsEmpty(this.TypeName)))
			{
				string[] array;
				if (SqlDbType.Udt == metaType.SqlDbType)
				{
					array = SqlParameter.ParseTypeName(this.UdtTypeName, true);
				}
				else
				{
					array = SqlParameter.ParseTypeName(this.TypeName, false);
				}
				if (1 == array.Length)
				{
					text3 = array[0];
				}
				else if (2 == array.Length)
				{
					text2 = array[0];
					text3 = array[1];
				}
				else
				{
					if (3 != array.Length)
					{
						throw ADP.ArgumentOutOfRange("names");
					}
					text = array[0];
					text2 = array[1];
					text3 = array[2];
				}
				if ((!ADP.IsEmpty(text) && 255 < text.Length) || (!ADP.IsEmpty(text2) && 255 < text2.Length) || (!ADP.IsEmpty(text3) && 255 < text3.Length))
				{
					throw ADP.ArgumentOutOfRange("names");
				}
			}
			byte b = this.GetActualPrecision();
			byte actualScale = this.GetActualScale();
			if (SqlDbType.Decimal == metaType.SqlDbType && b == 0)
			{
				b = 29;
			}
			List<SmiExtendedMetaData> list = null;
			SmiMetaDataPropertyCollection smiMetaDataPropertyCollection = null;
			if (SqlDbType.Structured == metaType.SqlDbType)
			{
				this.GetActualFieldsAndProperties(out list, out smiMetaDataPropertyCollection, out peekAhead);
			}
			return new SmiParameterMetaData(metaType.SqlDbType, num2, b, actualScale, (long)num3, sqlCompareOptions, null, SqlDbType.Structured == metaType.SqlDbType, list, smiMetaDataPropertyCollection, this.ParameterNameFixed, text, text2, text3, this.Direction);
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06002839 RID: 10297 RVA: 0x0028E154 File Offset: 0x0028D554
		// (set) Token: 0x0600283A RID: 10298 RVA: 0x0028E168 File Offset: 0x0028D568
		internal bool ParamaterIsSqlType
		{
			get
			{
				return this._isSqlParameterSqlType;
			}
			set
			{
				this._isSqlParameterSqlType = value;
			}
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x0600283B RID: 10299 RVA: 0x0028E17C File Offset: 0x0028D57C
		// (set) Token: 0x0600283C RID: 10300 RVA: 0x0028E19C File Offset: 0x0028D59C
		[ResDescription("SqlParameter_ParameterName")]
		[ResCategory("DataCategory_Data")]
		public override string ParameterName
		{
			get
			{
				string parameterName = this._parameterName;
				if (parameterName == null)
				{
					return ADP.StrEmpty;
				}
				return parameterName;
			}
			set
			{
				if (!ADP.IsEmpty(value) && value.Length >= 127 && ('@' != value[0] || value.Length > 127))
				{
					throw SQL.InvalidParameterNameLength(value);
				}
				if (this._parameterName != value)
				{
					this.PropertyChanging();
					this._parameterName = value;
					return;
				}
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x0600283D RID: 10301 RVA: 0x0028E1F4 File Offset: 0x0028D5F4
		internal string ParameterNameFixed
		{
			get
			{
				string text = this.ParameterName;
				if (0 < text.Length && '@' != text[0])
				{
					text = "@" + text;
				}
				return text;
			}
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x0600283E RID: 10302 RVA: 0x0028E22C File Offset: 0x0028D62C
		// (set) Token: 0x0600283F RID: 10303 RVA: 0x0028E240 File Offset: 0x0028D640
		[DefaultValue(0)]
		[ResDescription("DbDataParameter_Precision")]
		[ResCategory("DataCategory_Data")]
		public byte Precision
		{
			get
			{
				return this.PrecisionInternal;
			}
			set
			{
				this.PrecisionInternal = value;
			}
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x06002840 RID: 10304 RVA: 0x0028E254 File Offset: 0x0028D654
		// (set) Token: 0x06002841 RID: 10305 RVA: 0x0028E284 File Offset: 0x0028D684
		internal byte PrecisionInternal
		{
			get
			{
				byte b = this._precision;
				SqlDbType metaSqlDbTypeOnly = this.GetMetaSqlDbTypeOnly();
				if (b == 0 && SqlDbType.Decimal == metaSqlDbTypeOnly)
				{
					b = this.ValuePrecision(this.SqlValue);
				}
				return b;
			}
			set
			{
				SqlDbType sqlDbType = this.SqlDbType;
				if (SqlDbType.Float == sqlDbType)
				{
					if (value > 53)
					{
						throw SQL.PrecisionValueOutOfRange(value);
					}
				}
				else if (SqlDbType.Real == sqlDbType)
				{
					if (value > 24)
					{
						throw SQL.PrecisionValueOutOfRange(value);
					}
				}
				else if (value > 38)
				{
					throw SQL.PrecisionValueOutOfRange(value);
				}
				if (this._precision != value)
				{
					this.PropertyChanging();
					this._precision = value;
				}
			}
		}

		// Token: 0x06002842 RID: 10306 RVA: 0x0028E2DC File Offset: 0x0028D6DC
		private bool ShouldSerializePrecision()
		{
			return 0 != this._precision;
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x06002843 RID: 10307 RVA: 0x0028E2F8 File Offset: 0x0028D6F8
		// (set) Token: 0x06002844 RID: 10308 RVA: 0x0028E30C File Offset: 0x0028D70C
		[ResCategory("DataCategory_Data")]
		[ResDescription("DbDataParameter_Scale")]
		[DefaultValue(0)]
		public byte Scale
		{
			get
			{
				return this.ScaleInternal;
			}
			set
			{
				this.ScaleInternal = value;
			}
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x06002845 RID: 10309 RVA: 0x0028E320 File Offset: 0x0028D720
		// (set) Token: 0x06002846 RID: 10310 RVA: 0x0028E350 File Offset: 0x0028D750
		internal byte ScaleInternal
		{
			get
			{
				byte b = this._scale;
				SqlDbType metaSqlDbTypeOnly = this.GetMetaSqlDbTypeOnly();
				if (b == 0 && SqlDbType.Decimal == metaSqlDbTypeOnly)
				{
					b = this.ValueScale(this.SqlValue);
				}
				return b;
			}
			set
			{
				if (this._scale != value || !this._hasScale)
				{
					this.PropertyChanging();
					this._scale = value;
					this._hasScale = true;
				}
			}
		}

		// Token: 0x06002847 RID: 10311 RVA: 0x0028E384 File Offset: 0x0028D784
		private bool ShouldSerializeScale()
		{
			return 0 != this._scale;
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06002848 RID: 10312 RVA: 0x0028E3A0 File Offset: 0x0028D7A0
		// (set) Token: 0x06002849 RID: 10313 RVA: 0x0028E3B8 File Offset: 0x0028D7B8
		[ResCategory("DataCategory_Data")]
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("SqlParameter_SqlDbType")]
		[DbProviderSpecificTypeProperty(true)]
		public SqlDbType SqlDbType
		{
			get
			{
				return this.GetMetaTypeOnly().SqlDbType;
			}
			set
			{
				MetaType metaType = this._metaType;
				if ((SqlDbType)24 == value)
				{
					throw SQL.InvalidSqlDbType(value);
				}
				if (metaType == null || metaType.SqlDbType != value)
				{
					this.PropertyTypeChanging();
					this._metaType = MetaType.GetMetaTypeFromSqlDbType(value, value == SqlDbType.Structured);
				}
			}
		}

		// Token: 0x0600284A RID: 10314 RVA: 0x0028E3FC File Offset: 0x0028D7FC
		private bool ShouldSerializeSqlDbType()
		{
			return null != this._metaType;
		}

		// Token: 0x0600284B RID: 10315 RVA: 0x0028E418 File Offset: 0x0028D818
		public void ResetSqlDbType()
		{
			if (this._metaType != null)
			{
				this.PropertyTypeChanging();
				this._metaType = null;
			}
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x0600284C RID: 10316 RVA: 0x0028E43C File Offset: 0x0028D83C
		// (set) Token: 0x0600284D RID: 10317 RVA: 0x0028E4E0 File Offset: 0x0028D8E0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object SqlValue
		{
			get
			{
				if (this._udtLoadError != null)
				{
					throw this._udtLoadError;
				}
				if (this._value != null)
				{
					if (this._value == DBNull.Value)
					{
						return MetaType.GetNullSqlValue(this.GetMetaTypeOnly().SqlType);
					}
					if (this._value is INullable)
					{
						return this._value;
					}
					if (this._value is DateTime)
					{
						SqlDbType sqlDbType = this.GetMetaTypeOnly().SqlDbType;
						if (sqlDbType == SqlDbType.Date || sqlDbType == SqlDbType.DateTime2)
						{
							return this._value;
						}
					}
					return MetaType.GetSqlValueFromComVariant(this._value);
				}
				else
				{
					if (this._sqlBufferReturnValue != null)
					{
						return this._sqlBufferReturnValue.SqlValue;
					}
					return null;
				}
			}
			set
			{
				this.Value = value;
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x0600284E RID: 10318 RVA: 0x0028E4F4 File Offset: 0x0028D8F4
		// (set) Token: 0x0600284F RID: 10319 RVA: 0x0028E514 File Offset: 0x0028D914
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public string UdtTypeName
		{
			get
			{
				string udtTypeName = this._udtTypeName;
				if (udtTypeName == null)
				{
					return ADP.StrEmpty;
				}
				return udtTypeName;
			}
			set
			{
				this._udtTypeName = value;
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06002850 RID: 10320 RVA: 0x0028E528 File Offset: 0x0028D928
		// (set) Token: 0x06002851 RID: 10321 RVA: 0x0028E548 File Offset: 0x0028D948
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public string TypeName
		{
			get
			{
				string typeName = this._typeName;
				if (typeName == null)
				{
					return ADP.StrEmpty;
				}
				return typeName;
			}
			set
			{
				this._typeName = value;
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06002852 RID: 10322 RVA: 0x0028E55C File Offset: 0x0028D95C
		// (set) Token: 0x06002853 RID: 10323 RVA: 0x0028E5B0 File Offset: 0x0028D9B0
		[TypeConverter(typeof(StringConverter))]
		[ResCategory("DataCategory_Data")]
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("DbParameter_Value")]
		public override object Value
		{
			get
			{
				if (this._udtLoadError != null)
				{
					throw this._udtLoadError;
				}
				if (this._value != null)
				{
					return this._value;
				}
				if (this._sqlBufferReturnValue == null)
				{
					return null;
				}
				if (this.ParamaterIsSqlType)
				{
					return this._sqlBufferReturnValue.SqlValue;
				}
				return this._sqlBufferReturnValue.Value;
			}
			set
			{
				this._value = value;
				this._sqlBufferReturnValue = null;
				this._coercedValue = null;
				this._isSqlParameterSqlType = this._value is INullable;
				this._udtLoadError = null;
			}
		}

		// Token: 0x06002854 RID: 10324 RVA: 0x0028E5F0 File Offset: 0x0028D9F0
		internal int GetActualSize()
		{
			int num = 0;
			MetaType metaType = this.InternalMetaType;
			SqlDbType sqlDbType = metaType.SqlDbType;
			object coercedValue = this.GetCoercedValue();
			bool flag = false;
			if (ADP.IsNull(coercedValue) && !metaType.IsVarTime)
			{
				return 0;
			}
			if (sqlDbType == SqlDbType.Variant)
			{
				metaType = MetaType.GetMetaTypeFromValue(coercedValue);
				sqlDbType = MetaType.GetSqlDataType((int)metaType.TDSType, 0U, 0).SqlDbType;
				flag = true;
			}
			if (metaType.IsFixed)
			{
				return metaType.FixedLength;
			}
			int num2 = 0;
			SqlDbType sqlDbType2 = sqlDbType;
			switch (sqlDbType2)
			{
			case SqlDbType.Binary:
				goto IL_0162;
			case SqlDbType.Bit:
				goto IL_01EB;
			case SqlDbType.Char:
				break;
			default:
				switch (sqlDbType2)
				{
				case SqlDbType.Image:
					goto IL_0162;
				case SqlDbType.Int:
				case SqlDbType.Money:
					goto IL_01EB;
				case SqlDbType.NChar:
				case SqlDbType.NText:
				case SqlDbType.NVarChar:
					break;
				default:
					switch (sqlDbType2)
					{
					case SqlDbType.Text:
					case SqlDbType.VarChar:
						goto IL_0129;
					case SqlDbType.Timestamp:
					case SqlDbType.VarBinary:
						goto IL_0162;
					case SqlDbType.TinyInt:
					case SqlDbType.Variant:
					case (SqlDbType)24:
					case (SqlDbType)26:
					case (SqlDbType)27:
					case (SqlDbType)28:
					case SqlDbType.Date:
						goto IL_01EB;
					case SqlDbType.Xml:
						break;
					case SqlDbType.Udt:
						if (!ADP.IsNull(coercedValue))
						{
							num2 = AssemblyCache.GetLength(coercedValue);
							goto IL_01EB;
						}
						goto IL_01EB;
					case SqlDbType.Structured:
						num2 = -1;
						goto IL_01EB;
					case SqlDbType.Time:
						num = (flag ? 5 : MetaType.GetTimeSizeFromScale(this.GetActualScale()));
						goto IL_01EB;
					case SqlDbType.DateTime2:
						num = 3 + (flag ? 5 : MetaType.GetTimeSizeFromScale(this.GetActualScale()));
						goto IL_01EB;
					case SqlDbType.DateTimeOffset:
						num = 5 + (flag ? 5 : MetaType.GetTimeSizeFromScale(this.GetActualScale()));
						goto IL_01EB;
					default:
						goto IL_01EB;
					}
					break;
				}
				num2 = this.ValueSize(coercedValue);
				num = (this.ShouldSerializeSize() ? this.Size : 0);
				num = ((this.ShouldSerializeSize() && num <= num2) ? num : num2);
				if (num == -1)
				{
					num = num2;
				}
				num <<= 1;
				goto IL_01EB;
			}
			IL_0129:
			num2 = this.ValueSize(coercedValue);
			num = (this.ShouldSerializeSize() ? this.Size : 0);
			num = ((this.ShouldSerializeSize() && num <= num2) ? num : num2);
			if (num == -1)
			{
				num = num2;
				goto IL_01EB;
			}
			goto IL_01EB;
			IL_0162:
			num2 = this.ValueSize(coercedValue);
			num = (this.ShouldSerializeSize() ? this.Size : 0);
			num = ((this.ShouldSerializeSize() && num <= num2) ? num : num2);
			if (num == -1)
			{
				num = num2;
			}
			IL_01EB:
			if (flag && num2 > 8000)
			{
				throw SQL.ParameterInvalidVariant(this.ParameterName);
			}
			return num;
		}

		// Token: 0x06002855 RID: 10325 RVA: 0x0028E804 File Offset: 0x0028DC04
		object ICloneable.Clone()
		{
			return new SqlParameter(this);
		}

		// Token: 0x06002856 RID: 10326 RVA: 0x0028E818 File Offset: 0x0028DC18
		internal static object CoerceValue(object value, MetaType destinationType)
		{
			if (value != null && DBNull.Value != value)
			{
				Type type = value.GetType();
				bool flag = true;
				if (value is INullable && ((INullable)value).IsNull)
				{
					flag = false;
				}
				if (flag && typeof(object) != destinationType.ClassType && ((type != destinationType.ClassType && type != destinationType.SqlType) || SqlDbType.Xml == destinationType.SqlDbType))
				{
					try
					{
						if (typeof(string) == destinationType.ClassType)
						{
							if (typeof(SqlXml) == type)
							{
								value = MetaType.GetStringFromXml(((SqlXml)value).CreateReader());
							}
							else if (typeof(SqlString) != type)
							{
								if (typeof(XmlReader).IsAssignableFrom(type))
								{
									value = MetaType.GetStringFromXml((XmlReader)value);
								}
								else if (typeof(char[]) == type)
								{
									value = new string((char[])value);
								}
								else if (typeof(SqlChars) == type)
								{
									SqlChars sqlChars = (SqlChars)value;
									value = new string(sqlChars.Value);
								}
								else
								{
									value = Convert.ChangeType(value, destinationType.ClassType, null);
								}
							}
						}
						else if (DbType.Currency == destinationType.DbType && typeof(string) == type)
						{
							value = decimal.Parse((string)value, NumberStyles.Currency, null);
						}
						else if (typeof(SqlBytes) == type && typeof(byte[]) == destinationType.ClassType)
						{
							SqlBytes sqlBytes = (SqlBytes)value;
						}
						else if (typeof(string) == type && SqlDbType.Time == destinationType.SqlDbType)
						{
							value = TimeSpan.Parse((string)value);
						}
						else if (typeof(string) == type && SqlDbType.DateTimeOffset == destinationType.SqlDbType)
						{
							value = DateTimeOffset.Parse((string)value, null);
						}
						else if (typeof(DateTime) == type && SqlDbType.DateTimeOffset == destinationType.SqlDbType)
						{
							value = new DateTimeOffset((DateTime)value);
						}
						else if (243 != destinationType.TDSType || (!(value is DataTable) && !(value is DbDataReader) && !(value is IEnumerable<SqlDataRecord>)))
						{
							value = Convert.ChangeType(value, destinationType.ClassType, null);
						}
					}
					catch (Exception ex)
					{
						if (!ADP.IsCatchableExceptionType(ex))
						{
							throw;
						}
						throw ADP.ParameterConversionFailed(value, destinationType.ClassType, ex);
					}
				}
			}
			return value;
		}

		// Token: 0x06002857 RID: 10327 RVA: 0x0028EAA8 File Offset: 0x0028DEA8
		private void CloneHelper(SqlParameter destination)
		{
			this.CloneHelperCore(destination);
			destination._metaType = this._metaType;
			destination._collation = this._collation;
			destination._xmlSchemaCollectionDatabase = this._xmlSchemaCollectionDatabase;
			destination._xmlSchemaCollectionOwningSchema = this._xmlSchemaCollectionOwningSchema;
			destination._xmlSchemaCollectionName = this._xmlSchemaCollectionName;
			destination._udtTypeName = this._udtTypeName;
			destination._typeName = this._typeName;
			destination._udtLoadError = this._udtLoadError;
			destination._parameterName = this._parameterName;
			destination._precision = this._precision;
			destination._scale = this._scale;
			destination._sqlBufferReturnValue = this._sqlBufferReturnValue;
			destination._isSqlParameterSqlType = this._isSqlParameterSqlType;
			destination._internalMetaType = this._internalMetaType;
			destination.CoercedValue = this.CoercedValue;
		}

		// Token: 0x06002858 RID: 10328 RVA: 0x0028EB70 File Offset: 0x0028DF70
		internal byte GetActualPrecision()
		{
			if (!this.ShouldSerializePrecision())
			{
				return this.ValuePrecision(this.CoercedValue);
			}
			return this.PrecisionInternal;
		}

		// Token: 0x06002859 RID: 10329 RVA: 0x0028EB98 File Offset: 0x0028DF98
		internal byte GetActualScale()
		{
			if (this.ShouldSerializeScale())
			{
				return this.ScaleInternal;
			}
			if (this.GetMetaTypeOnly().IsVarTime)
			{
				return 7;
			}
			return this.ValueScale(this.CoercedValue);
		}

		// Token: 0x0600285A RID: 10330 RVA: 0x0028EBD0 File Offset: 0x0028DFD0
		internal int GetParameterSize()
		{
			if (!this.ShouldSerializeSize())
			{
				return this.ValueSize(this.CoercedValue);
			}
			return this.Size;
		}

		// Token: 0x0600285B RID: 10331 RVA: 0x0028EBF8 File Offset: 0x0028DFF8
		private void GetActualFieldsAndProperties(out List<SmiExtendedMetaData> fields, out SmiMetaDataPropertyCollection props, out ParameterPeekAheadValue peekAhead)
		{
			fields = null;
			props = null;
			peekAhead = null;
			object coercedValue = this.GetCoercedValue();
			if (coercedValue is DataTable)
			{
				DataTable dataTable = coercedValue as DataTable;
				if (dataTable.Columns.Count <= 0)
				{
					throw SQL.NotEnoughColumnsInStructuredType();
				}
				fields = new List<SmiExtendedMetaData>(dataTable.Columns.Count);
				bool[] array = new bool[dataTable.Columns.Count];
				bool flag = false;
				if (dataTable.PrimaryKey != null && 0 < dataTable.PrimaryKey.Length)
				{
					foreach (DataColumn dataColumn in dataTable.PrimaryKey)
					{
						array[dataColumn.Ordinal] = true;
						flag = true;
					}
				}
				for (int j = 0; j < dataTable.Columns.Count; j++)
				{
					fields.Add(MetaDataUtilsSmi.SmiMetaDataFromDataColumn(dataTable.Columns[j], dataTable));
					if (!flag && dataTable.Columns[j].Unique)
					{
						array[j] = true;
						flag = true;
					}
				}
				if (flag)
				{
					props = new SmiMetaDataPropertyCollection();
					props[SmiPropertySelector.UniqueKey] = new SmiUniqueKeyProperty(new List<bool>(array));
					return;
				}
			}
			else if (coercedValue is SqlDataReader)
			{
				fields = new List<SmiExtendedMetaData>(((SqlDataReader)coercedValue).GetInternalSmiMetaData());
				if (fields.Count <= 0)
				{
					throw SQL.NotEnoughColumnsInStructuredType();
				}
				bool[] array2 = new bool[fields.Count];
				bool flag2 = false;
				for (int k = 0; k < fields.Count; k++)
				{
					SmiQueryMetaData smiQueryMetaData = fields[k] as SmiQueryMetaData;
					if (smiQueryMetaData != null && !smiQueryMetaData.IsKey.IsNull && smiQueryMetaData.IsKey.Value)
					{
						array2[k] = true;
						flag2 = true;
					}
				}
				if (flag2)
				{
					props = new SmiMetaDataPropertyCollection();
					props[SmiPropertySelector.UniqueKey] = new SmiUniqueKeyProperty(new List<bool>(array2));
					return;
				}
			}
			else
			{
				if (coercedValue is IEnumerable<SqlDataRecord>)
				{
					IEnumerator<SqlDataRecord> enumerator = ((IEnumerable<SqlDataRecord>)coercedValue).GetEnumerator();
					try
					{
						if (!enumerator.MoveNext())
						{
							throw SQL.IEnumerableOfSqlDataRecordHasNoRows();
						}
						SqlDataRecord sqlDataRecord = enumerator.Current;
						int fieldCount = sqlDataRecord.FieldCount;
						if (0 < fieldCount)
						{
							bool[] array3 = new bool[fieldCount];
							bool[] array4 = new bool[fieldCount];
							bool[] array5 = new bool[fieldCount];
							int num = -1;
							bool flag3 = false;
							bool flag4 = false;
							int num2 = 0;
							SmiOrderProperty.SmiColumnOrder[] array6 = new SmiOrderProperty.SmiColumnOrder[fieldCount];
							fields = new List<SmiExtendedMetaData>(fieldCount);
							for (int l = 0; l < fieldCount; l++)
							{
								SqlMetaData sqlMetaData = sqlDataRecord.GetSqlMetaData(l);
								fields.Add(MetaDataUtilsSmi.SqlMetaDataToSmiExtendedMetaData(sqlMetaData));
								if (sqlMetaData.IsUniqueKey)
								{
									array3[l] = true;
									flag3 = true;
								}
								if (sqlMetaData.UseServerDefault)
								{
									array4[l] = true;
									flag4 = true;
								}
								array6[l].Order = sqlMetaData.SortOrder;
								if (SortOrder.Unspecified != sqlMetaData.SortOrder)
								{
									if (fieldCount <= sqlMetaData.SortOrdinal)
									{
										throw SQL.SortOrdinalGreaterThanFieldCount(l, sqlMetaData.SortOrdinal);
									}
									if (array5[sqlMetaData.SortOrdinal])
									{
										throw SQL.DuplicateSortOrdinal(sqlMetaData.SortOrdinal);
									}
									array6[l].SortOrdinal = sqlMetaData.SortOrdinal;
									array5[sqlMetaData.SortOrdinal] = true;
									if (sqlMetaData.SortOrdinal > num)
									{
										num = sqlMetaData.SortOrdinal;
									}
									num2++;
								}
							}
							if (flag3)
							{
								props = new SmiMetaDataPropertyCollection();
								props[SmiPropertySelector.UniqueKey] = new SmiUniqueKeyProperty(new List<bool>(array3));
							}
							if (flag4)
							{
								if (props == null)
								{
									props = new SmiMetaDataPropertyCollection();
								}
								props[SmiPropertySelector.DefaultFields] = new SmiDefaultFieldsProperty(new List<bool>(array4));
							}
							if (0 < num2)
							{
								if (num >= num2)
								{
									int num3 = 0;
									while (num3 < num2 && array5[num3])
									{
										num3++;
									}
									throw SQL.MissingSortOrdinal(num3);
								}
								if (props == null)
								{
									props = new SmiMetaDataPropertyCollection();
								}
								props[SmiPropertySelector.SortOrder] = new SmiOrderProperty(new List<SmiOrderProperty.SmiColumnOrder>(array6));
							}
							peekAhead = new ParameterPeekAheadValue();
							peekAhead.Enumerator = enumerator;
							peekAhead.FirstRecord = sqlDataRecord;
							enumerator = null;
							return;
						}
						throw SQL.NotEnoughColumnsInStructuredType();
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				if (coercedValue is DbDataReader)
				{
					DataTable schemaTable = ((DbDataReader)coercedValue).GetSchemaTable();
					if (schemaTable.Rows.Count <= 0)
					{
						throw SQL.NotEnoughColumnsInStructuredType();
					}
					int count = schemaTable.Rows.Count;
					fields = new List<SmiExtendedMetaData>(count);
					bool[] array7 = new bool[count];
					bool flag5 = false;
					int ordinal = schemaTable.Columns[SchemaTableColumn.IsKey].Ordinal;
					int ordinal2 = schemaTable.Columns[SchemaTableColumn.ColumnOrdinal].Ordinal;
					for (int m = 0; m < count; m++)
					{
						DataRow dataRow = schemaTable.Rows[m];
						SmiExtendedMetaData smiExtendedMetaData = MetaDataUtilsSmi.SmiMetaDataFromSchemaTableRow(dataRow);
						int n = m;
						if (!dataRow.IsNull(ordinal2))
						{
							n = (int)dataRow[ordinal2];
						}
						if (n >= count || n < 0)
						{
							throw SQL.InvalidSchemaTableOrdinals();
						}
						while (n > fields.Count)
						{
							fields.Add(null);
						}
						if (fields.Count == n)
						{
							fields.Add(smiExtendedMetaData);
						}
						else
						{
							if (fields[n] != null)
							{
								throw SQL.InvalidSchemaTableOrdinals();
							}
							fields[n] = smiExtendedMetaData;
						}
						if (!dataRow.IsNull(ordinal) && (bool)dataRow[ordinal])
						{
							array7[n] = true;
							flag5 = true;
						}
					}
					if (flag5)
					{
						props = new SmiMetaDataPropertyCollection();
						props[SmiPropertySelector.UniqueKey] = new SmiUniqueKeyProperty(new List<bool>(array7));
					}
				}
			}
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x0028F164 File Offset: 0x0028E564
		internal object GetCoercedValue()
		{
			object obj = this.CoercedValue;
			if (obj == null)
			{
				obj = SqlParameter.CoerceValue(this.Value, this._internalMetaType);
				this.CoercedValue = obj;
			}
			return obj;
		}

		// Token: 0x0600285D RID: 10333 RVA: 0x0028F198 File Offset: 0x0028E598
		private SqlDbType GetMetaSqlDbTypeOnly()
		{
			MetaType metaType = this._metaType;
			if (metaType == null)
			{
				metaType = MetaType.GetDefaultMetaType();
			}
			return metaType.SqlDbType;
		}

		// Token: 0x0600285E RID: 10334 RVA: 0x0028F1BC File Offset: 0x0028E5BC
		private MetaType GetMetaTypeOnly()
		{
			if (this._metaType != null)
			{
				return this._metaType;
			}
			if (this._value != null && DBNull.Value != this._value)
			{
				Type type = this._value.GetType();
				if (typeof(char) == type)
				{
					this._value = this._value.ToString();
					type = typeof(string);
				}
				else if (typeof(char[]) == type)
				{
					this._value = new string((char[])this._value);
					type = typeof(string);
				}
				return MetaType.GetMetaTypeFromType(type);
			}
			if (this._sqlBufferReturnValue != null)
			{
				Type typeFromStorageType = this._sqlBufferReturnValue.GetTypeFromStorageType(this._isSqlParameterSqlType);
				if (typeFromStorageType != null)
				{
					return MetaType.GetMetaTypeFromType(typeFromStorageType);
				}
			}
			return MetaType.GetDefaultMetaType();
		}

		// Token: 0x0600285F RID: 10335 RVA: 0x0028F288 File Offset: 0x0028E688
		internal void Prepare(SqlCommand cmd)
		{
			if (this._metaType == null)
			{
				throw ADP.PrepareParameterType(cmd);
			}
			if (!this.ShouldSerializeSize() && !this._metaType.IsFixed)
			{
				throw ADP.PrepareParameterSize(cmd);
			}
			if (!this.ShouldSerializePrecision() && !this.ShouldSerializeScale() && this._metaType.SqlDbType == SqlDbType.Decimal)
			{
				throw ADP.PrepareParameterScale(cmd, this.SqlDbType.ToString());
			}
		}

		// Token: 0x06002860 RID: 10336 RVA: 0x0028F2F8 File Offset: 0x0028E6F8
		private void PropertyChanging()
		{
			this._internalMetaType = null;
		}

		// Token: 0x06002861 RID: 10337 RVA: 0x0028F30C File Offset: 0x0028E70C
		private void PropertyTypeChanging()
		{
			this.PropertyChanging();
			this.CoercedValue = null;
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x0028F328 File Offset: 0x0028E728
		internal void SetSqlBuffer(SqlBuffer buff)
		{
			this._sqlBufferReturnValue = buff;
			this._value = null;
			this._coercedValue = null;
			this._udtLoadError = null;
		}

		// Token: 0x06002863 RID: 10339 RVA: 0x0028F354 File Offset: 0x0028E754
		internal void SetUdtLoadError(Exception e)
		{
			this._udtLoadError = e;
		}

		// Token: 0x06002864 RID: 10340 RVA: 0x0028F368 File Offset: 0x0028E768
		internal void Validate(int index, bool isCommandProc)
		{
			MetaType metaTypeOnly = this.GetMetaTypeOnly();
			this._internalMetaType = metaTypeOnly;
			if (ADP.IsDirection(this, ParameterDirection.Output) && !ADP.IsDirection(this, ParameterDirection.ReturnValue) && !metaTypeOnly.IsFixed && !this.ShouldSerializeSize() && (this._value == null || Convert.IsDBNull(this._value)) && this.SqlDbType != SqlDbType.Timestamp && this.SqlDbType != SqlDbType.Udt && !metaTypeOnly.IsVarTime)
			{
				throw ADP.UninitializedParameterSize(index, metaTypeOnly.ClassType);
			}
			if (metaTypeOnly.SqlDbType != SqlDbType.Udt && this.Direction != ParameterDirection.Output)
			{
				this.GetCoercedValue();
			}
			if (metaTypeOnly.SqlDbType == SqlDbType.Udt)
			{
				if (ADP.IsEmpty(this.UdtTypeName))
				{
					throw SQL.MustSetUdtTypeNameForUdtParams();
				}
			}
			else if (!ADP.IsEmpty(this.UdtTypeName))
			{
				throw SQL.UnexpectedUdtTypeNameForNonUdtParams();
			}
			if (metaTypeOnly.SqlDbType == SqlDbType.Structured)
			{
				if (!isCommandProc && ADP.IsEmpty(this.TypeName))
				{
					throw SQL.MustSetTypeNameForParam(metaTypeOnly.TypeName, this.ParameterName);
				}
				if (ParameterDirection.Input != this.Direction)
				{
					throw SQL.UnsupportedTVPOutputParameter(this.Direction, this.ParameterName);
				}
				if (DBNull.Value == this.GetCoercedValue())
				{
					throw SQL.DBNullNotSupportedForTVPValues(this.ParameterName);
				}
			}
			else if (!ADP.IsEmpty(this.TypeName))
			{
				throw SQL.UnexpectedTypeNameForNonStructParams(this.ParameterName);
			}
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x0028F4A8 File Offset: 0x0028E8A8
		internal MetaType ValidateTypeLengths(bool yukonOrNewer)
		{
			MetaType metaType = this.InternalMetaType;
			if (SqlDbType.Udt != metaType.SqlDbType && !metaType.IsFixed && !metaType.IsLong)
			{
				int actualSize = this.GetActualSize();
				int size = this.Size;
				int num = ((size > actualSize) ? size : actualSize);
				if (num > 8000 || size == -1 || actualSize == -1)
				{
					if (yukonOrNewer)
					{
						metaType = MetaType.GetMaxMetaTypeFromMetaType(metaType);
						this._metaType = metaType;
						this.InternalMetaType = metaType;
						if (!metaType.IsPlp)
						{
							if (metaType.SqlDbType == SqlDbType.Xml)
							{
								throw ADP.InvalidMetaDataValue();
							}
							if (metaType.SqlDbType == SqlDbType.NVarChar || metaType.SqlDbType == SqlDbType.VarChar || metaType.SqlDbType == SqlDbType.VarBinary)
							{
								this.Size = -1;
							}
						}
					}
					else
					{
						SqlDbType sqlDbType = metaType.SqlDbType;
						switch (sqlDbType)
						{
						case SqlDbType.Binary:
							break;
						case SqlDbType.Bit:
							return metaType;
						case SqlDbType.Char:
							goto IL_0111;
						default:
							switch (sqlDbType)
							{
							case SqlDbType.NChar:
							case SqlDbType.NVarChar:
								metaType = MetaType.GetMetaTypeFromSqlDbType(SqlDbType.NText, false);
								this._metaType = metaType;
								this.InternalMetaType = metaType;
								return metaType;
							case SqlDbType.NText:
								return metaType;
							default:
								switch (sqlDbType)
								{
								case SqlDbType.VarBinary:
									break;
								case SqlDbType.VarChar:
									goto IL_0111;
								default:
									return metaType;
								}
								break;
							}
							break;
						}
						metaType = MetaType.GetMetaTypeFromSqlDbType(SqlDbType.Image, false);
						this._metaType = metaType;
						this.InternalMetaType = metaType;
						return metaType;
						IL_0111:
						metaType = MetaType.GetMetaTypeFromSqlDbType(SqlDbType.Text, false);
						this._metaType = metaType;
						this.InternalMetaType = metaType;
					}
				}
			}
			return metaType;
		}

		// Token: 0x06002866 RID: 10342 RVA: 0x0028F5F8 File Offset: 0x0028E9F8
		private byte ValuePrecision(object value)
		{
			if (!(value is SqlDecimal))
			{
				return this.ValuePrecisionCore(value);
			}
			if (((SqlDecimal)value).IsNull)
			{
				return 0;
			}
			return ((SqlDecimal)value).Precision;
		}

		// Token: 0x06002867 RID: 10343 RVA: 0x0028F638 File Offset: 0x0028EA38
		private byte ValueScale(object value)
		{
			if (!(value is SqlDecimal))
			{
				return this.ValueScaleCore(value);
			}
			if (((SqlDecimal)value).IsNull)
			{
				return 0;
			}
			return ((SqlDecimal)value).Scale;
		}

		// Token: 0x06002868 RID: 10344 RVA: 0x0028F678 File Offset: 0x0028EA78
		private int ValueSize(object value)
		{
			if (value is SqlString)
			{
				if (((SqlString)value).IsNull)
				{
					return 0;
				}
				return ((SqlString)value).Value.Length;
			}
			else if (value is SqlChars)
			{
				if (((SqlChars)value).IsNull)
				{
					return 0;
				}
				return ((SqlChars)value).Value.Length;
			}
			else if (value is SqlBinary)
			{
				if (((SqlBinary)value).IsNull)
				{
					return 0;
				}
				return ((SqlBinary)value).Length;
			}
			else
			{
				if (!(value is SqlBytes))
				{
					return this.ValueSizeCore(value);
				}
				if (((SqlBytes)value).IsNull)
				{
					return 0;
				}
				return (int)((SqlBytes)value).Length;
			}
		}

		// Token: 0x06002869 RID: 10345 RVA: 0x0028F72C File Offset: 0x0028EB2C
		internal static string[] ParseTypeName(string typeName, bool isUdtTypeName)
		{
			string[] array;
			try
			{
				string text;
				if (isUdtTypeName)
				{
					text = "SQL_UDTTypeName";
				}
				else
				{
					text = "SQL_TypeName";
				}
				array = MultipartIdentifier.ParseMultipartIdentifier(typeName, "[\"", "]\"", '.', 3, true, text, true);
			}
			catch (ArgumentException)
			{
				if (isUdtTypeName)
				{
					throw SQL.InvalidUdt3PartNameFormat();
				}
				throw SQL.InvalidParameterTypeNameFormat();
			}
			return array;
		}

		// Token: 0x0600286A RID: 10346 RVA: 0x0028F794 File Offset: 0x0028EB94
		private SqlParameter(SqlParameter source)
			: this()
		{
			ADP.CheckArgumentNull(source, "source");
			source.CloneHelper(this);
			ICloneable cloneable = this._value as ICloneable;
			if (cloneable != null)
			{
				this._value = cloneable.Clone();
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x0600286B RID: 10347 RVA: 0x0028F7D4 File Offset: 0x0028EBD4
		// (set) Token: 0x0600286C RID: 10348 RVA: 0x0028F7E8 File Offset: 0x0028EBE8
		private object CoercedValue
		{
			get
			{
				return this._coercedValue;
			}
			set
			{
				this._coercedValue = value;
			}
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x0600286D RID: 10349 RVA: 0x0028F7FC File Offset: 0x0028EBFC
		// (set) Token: 0x0600286E RID: 10350 RVA: 0x0028F818 File Offset: 0x0028EC18
		[RefreshProperties(RefreshProperties.All)]
		[ResCategory("DataCategory_Data")]
		[ResDescription("DbParameter_Direction")]
		public override ParameterDirection Direction
		{
			get
			{
				ParameterDirection direction = this._direction;
				if (direction == (ParameterDirection)0)
				{
					return ParameterDirection.Input;
				}
				return direction;
			}
			set
			{
				if (this._direction != value)
				{
					switch (value)
					{
					case ParameterDirection.Input:
					case ParameterDirection.Output:
					case ParameterDirection.InputOutput:
					case ParameterDirection.ReturnValue:
						this.PropertyChanging();
						this._direction = value;
						return;
					}
					throw ADP.InvalidParameterDirection(value);
				}
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x0600286F RID: 10351 RVA: 0x0028F868 File Offset: 0x0028EC68
		// (set) Token: 0x06002870 RID: 10352 RVA: 0x0028F87C File Offset: 0x0028EC7C
		public override bool IsNullable
		{
			get
			{
				return this._isNullable;
			}
			set
			{
				this._isNullable = value;
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06002871 RID: 10353 RVA: 0x0028F890 File Offset: 0x0028EC90
		// (set) Token: 0x06002872 RID: 10354 RVA: 0x0028F8A4 File Offset: 0x0028ECA4
		[ResDescription("DbParameter_Offset")]
		[ResCategory("DataCategory_Data")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public int Offset
		{
			get
			{
				return this._offset;
			}
			set
			{
				if (value < 0)
				{
					throw ADP.InvalidOffsetValue(value);
				}
				this._offset = value;
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06002873 RID: 10355 RVA: 0x0028F8C4 File Offset: 0x0028ECC4
		// (set) Token: 0x06002874 RID: 10356 RVA: 0x0028F8EC File Offset: 0x0028ECEC
		[ResCategory("DataCategory_Data")]
		[ResDescription("DbParameter_Size")]
		public override int Size
		{
			get
			{
				int num = this._size;
				if (num == 0)
				{
					num = this.ValueSize(this.Value);
				}
				return num;
			}
			set
			{
				if (this._size != value)
				{
					if (value < -1)
					{
						throw ADP.InvalidSizeValue(value);
					}
					this.PropertyChanging();
					this._size = value;
				}
			}
		}

		// Token: 0x06002875 RID: 10357 RVA: 0x0028F91C File Offset: 0x0028ED1C
		private void ResetSize()
		{
			if (this._size != 0)
			{
				this.PropertyChanging();
				this._size = 0;
			}
		}

		// Token: 0x06002876 RID: 10358 RVA: 0x0028F940 File Offset: 0x0028ED40
		private bool ShouldSerializeSize()
		{
			return 0 != this._size;
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06002877 RID: 10359 RVA: 0x0028F95C File Offset: 0x0028ED5C
		// (set) Token: 0x06002878 RID: 10360 RVA: 0x0028F97C File Offset: 0x0028ED7C
		[ResCategory("DataCategory_Update")]
		[ResDescription("DbParameter_SourceColumn")]
		public override string SourceColumn
		{
			get
			{
				string sourceColumn = this._sourceColumn;
				if (sourceColumn == null)
				{
					return ADP.StrEmpty;
				}
				return sourceColumn;
			}
			set
			{
				this._sourceColumn = value;
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06002879 RID: 10361 RVA: 0x0028F990 File Offset: 0x0028ED90
		// (set) Token: 0x0600287A RID: 10362 RVA: 0x0028F9A4 File Offset: 0x0028EDA4
		public override bool SourceColumnNullMapping
		{
			get
			{
				return this._sourceColumnNullMapping;
			}
			set
			{
				this._sourceColumnNullMapping = value;
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x0600287B RID: 10363 RVA: 0x0028F9B8 File Offset: 0x0028EDB8
		// (set) Token: 0x0600287C RID: 10364 RVA: 0x0028F9D8 File Offset: 0x0028EDD8
		[ResCategory("DataCategory_Update")]
		[ResDescription("DbParameter_SourceVersion")]
		public override DataRowVersion SourceVersion
		{
			get
			{
				DataRowVersion sourceVersion = this._sourceVersion;
				if (sourceVersion == (DataRowVersion)0)
				{
					return DataRowVersion.Current;
				}
				return sourceVersion;
			}
			set
			{
				if (value <= DataRowVersion.Current)
				{
					if (value != DataRowVersion.Original && value != DataRowVersion.Current)
					{
						goto IL_0034;
					}
				}
				else if (value != DataRowVersion.Proposed && value != DataRowVersion.Default)
				{
					goto IL_0034;
				}
				this._sourceVersion = value;
				return;
				IL_0034:
				throw ADP.InvalidDataRowVersion(value);
			}
		}

		// Token: 0x0600287D RID: 10365 RVA: 0x0028FA20 File Offset: 0x0028EE20
		private void CloneHelperCore(SqlParameter destination)
		{
			destination._value = this._value;
			destination._direction = this._direction;
			destination._size = this._size;
			destination._offset = this._offset;
			destination._sourceColumn = this._sourceColumn;
			destination._sourceVersion = this._sourceVersion;
			destination._sourceColumnNullMapping = this._sourceColumnNullMapping;
			destination._isNullable = this._isNullable;
		}

		// Token: 0x0600287E RID: 10366 RVA: 0x0028FA90 File Offset: 0x0028EE90
		internal void CopyTo(DbParameter destination)
		{
			ADP.CheckArgumentNull(destination, "destination");
			this.CloneHelper((SqlParameter)destination);
		}

		// Token: 0x0600287F RID: 10367 RVA: 0x0028FAB4 File Offset: 0x0028EEB4
		internal object CompareExchangeParent(object value, object comparand)
		{
			object parent = this._parent;
			if (comparand == parent)
			{
				this._parent = value;
			}
			return parent;
		}

		// Token: 0x06002880 RID: 10368 RVA: 0x0028FAD4 File Offset: 0x0028EED4
		internal void ResetParent()
		{
			this._parent = null;
		}

		// Token: 0x06002881 RID: 10369 RVA: 0x0028FAE8 File Offset: 0x0028EEE8
		public override string ToString()
		{
			return this.ParameterName;
		}

		// Token: 0x06002882 RID: 10370 RVA: 0x0028FAFC File Offset: 0x0028EEFC
		private byte ValuePrecisionCore(object value)
		{
			if (value is decimal)
			{
				return ((decimal)value).Precision;
			}
			return 0;
		}

		// Token: 0x06002883 RID: 10371 RVA: 0x0028FB28 File Offset: 0x0028EF28
		private byte ValueScaleCore(object value)
		{
			if (value is decimal)
			{
				return (byte)((decimal.GetBits((decimal)value)[3] & 16711680) >> 16);
			}
			return 0;
		}

		// Token: 0x06002884 RID: 10372 RVA: 0x0028FB58 File Offset: 0x0028EF58
		private int ValueSizeCore(object value)
		{
			if (!ADP.IsNull(value))
			{
				string text = value as string;
				if (text != null)
				{
					return text.Length;
				}
				byte[] array = value as byte[];
				if (array != null)
				{
					return array.Length;
				}
				char[] array2 = value as char[];
				if (array2 != null)
				{
					return array2.Length;
				}
				if (value is byte || value is char)
				{
					return 1;
				}
			}
			return 0;
		}

		// Token: 0x04001950 RID: 6480
		private MetaType _metaType;

		// Token: 0x04001951 RID: 6481
		private SqlCollation _collation;

		// Token: 0x04001952 RID: 6482
		private string _xmlSchemaCollectionDatabase;

		// Token: 0x04001953 RID: 6483
		private string _xmlSchemaCollectionOwningSchema;

		// Token: 0x04001954 RID: 6484
		private string _xmlSchemaCollectionName;

		// Token: 0x04001955 RID: 6485
		private string _udtTypeName;

		// Token: 0x04001956 RID: 6486
		private string _typeName;

		// Token: 0x04001957 RID: 6487
		private Type _udtType;

		// Token: 0x04001958 RID: 6488
		private Exception _udtLoadError;

		// Token: 0x04001959 RID: 6489
		private string _parameterName;

		// Token: 0x0400195A RID: 6490
		private byte _precision;

		// Token: 0x0400195B RID: 6491
		private byte _scale;

		// Token: 0x0400195C RID: 6492
		private bool _hasScale;

		// Token: 0x0400195D RID: 6493
		private MetaType _internalMetaType;

		// Token: 0x0400195E RID: 6494
		private SqlBuffer _sqlBufferReturnValue;

		// Token: 0x0400195F RID: 6495
		private bool _isSqlParameterSqlType;

		// Token: 0x04001960 RID: 6496
		private object _value;

		// Token: 0x04001961 RID: 6497
		private object _parent;

		// Token: 0x04001962 RID: 6498
		private ParameterDirection _direction;

		// Token: 0x04001963 RID: 6499
		private int _size;

		// Token: 0x04001964 RID: 6500
		private int _offset;

		// Token: 0x04001965 RID: 6501
		private string _sourceColumn;

		// Token: 0x04001966 RID: 6502
		private DataRowVersion _sourceVersion;

		// Token: 0x04001967 RID: 6503
		private bool _sourceColumnNullMapping;

		// Token: 0x04001968 RID: 6504
		private bool _isNullable;

		// Token: 0x04001969 RID: 6505
		private object _coercedValue;

		// Token: 0x02000306 RID: 774
		internal sealed class SqlParameterConverter : ExpandableObjectConverter
		{
			// Token: 0x06002886 RID: 10374 RVA: 0x0028FBC4 File Offset: 0x0028EFC4
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return typeof(InstanceDescriptor) == destinationType || base.CanConvertTo(context, destinationType);
			}

			// Token: 0x06002887 RID: 10375 RVA: 0x0028FBE8 File Offset: 0x0028EFE8
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == null)
				{
					throw ADP.ArgumentNull("destinationType");
				}
				if (typeof(InstanceDescriptor) == destinationType && value is SqlParameter)
				{
					return this.ConvertToInstanceDescriptor(value as SqlParameter);
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}

			// Token: 0x06002888 RID: 10376 RVA: 0x0028FC34 File Offset: 0x0028F034
			private InstanceDescriptor ConvertToInstanceDescriptor(SqlParameter p)
			{
				int num = 0;
				if (p.ShouldSerializeSqlDbType())
				{
					num |= 1;
				}
				if (p.ShouldSerializeSize())
				{
					num |= 2;
				}
				if (!ADP.IsEmpty(p.SourceColumn))
				{
					num |= 4;
				}
				if (p.Value != null)
				{
					num |= 8;
				}
				if (ParameterDirection.Input != p.Direction || p.IsNullable || p.ShouldSerializePrecision() || p.ShouldSerializeScale() || DataRowVersion.Current != p.SourceVersion)
				{
					num |= 16;
				}
				if (p.SourceColumnNullMapping || !ADP.IsEmpty(p.XmlSchemaCollectionDatabase) || !ADP.IsEmpty(p.XmlSchemaCollectionOwningSchema) || !ADP.IsEmpty(p.XmlSchemaCollectionName))
				{
					num |= 32;
				}
				Type[] array;
				object[] array2;
				switch (num)
				{
				case 0:
				case 1:
					array = new Type[]
					{
						typeof(string),
						typeof(SqlDbType)
					};
					array2 = new object[] { p.ParameterName, p.SqlDbType };
					break;
				case 2:
				case 3:
					array = new Type[]
					{
						typeof(string),
						typeof(SqlDbType),
						typeof(int)
					};
					array2 = new object[] { p.ParameterName, p.SqlDbType, p.Size };
					break;
				case 4:
				case 5:
				case 6:
				case 7:
					array = new Type[]
					{
						typeof(string),
						typeof(SqlDbType),
						typeof(int),
						typeof(string)
					};
					array2 = new object[] { p.ParameterName, p.SqlDbType, p.Size, p.SourceColumn };
					break;
				case 8:
					array = new Type[]
					{
						typeof(string),
						typeof(object)
					};
					array2 = new object[] { p.ParameterName, p.Value };
					break;
				default:
					if ((32 & num) == 0)
					{
						array = new Type[]
						{
							typeof(string),
							typeof(SqlDbType),
							typeof(int),
							typeof(ParameterDirection),
							typeof(bool),
							typeof(byte),
							typeof(byte),
							typeof(string),
							typeof(DataRowVersion),
							typeof(object)
						};
						array2 = new object[] { p.ParameterName, p.SqlDbType, p.Size, p.Direction, p.IsNullable, p.PrecisionInternal, p.ScaleInternal, p.SourceColumn, p.SourceVersion, p.Value };
					}
					else
					{
						array = new Type[]
						{
							typeof(string),
							typeof(SqlDbType),
							typeof(int),
							typeof(ParameterDirection),
							typeof(byte),
							typeof(byte),
							typeof(string),
							typeof(DataRowVersion),
							typeof(bool),
							typeof(object),
							typeof(string),
							typeof(string),
							typeof(string)
						};
						array2 = new object[]
						{
							p.ParameterName, p.SqlDbType, p.Size, p.Direction, p.PrecisionInternal, p.ScaleInternal, p.SourceColumn, p.SourceVersion, p.SourceColumnNullMapping, p.Value,
							p.XmlSchemaCollectionDatabase, p.XmlSchemaCollectionOwningSchema, p.XmlSchemaCollectionName
						};
					}
					break;
				}
				ConstructorInfo constructor = typeof(SqlParameter).GetConstructor(array);
				return new InstanceDescriptor(constructor, array2);
			}
		}
	}
}
