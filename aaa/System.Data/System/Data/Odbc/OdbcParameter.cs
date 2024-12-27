using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data.Common;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Data.Odbc
{
	// Token: 0x020001F7 RID: 503
	[TypeConverter(typeof(OdbcParameter.OdbcParameterConverter))]
	public sealed class OdbcParameter : DbParameter, ICloneable, IDbDataParameter, IDataParameter
	{
		// Token: 0x06001BD6 RID: 7126 RVA: 0x0024A904 File Offset: 0x00249D04
		public OdbcParameter()
		{
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x0024A918 File Offset: 0x00249D18
		public OdbcParameter(string name, object value)
			: this()
		{
			this.ParameterName = name;
			this.Value = value;
		}

		// Token: 0x06001BD8 RID: 7128 RVA: 0x0024A93C File Offset: 0x00249D3C
		public OdbcParameter(string name, OdbcType type)
			: this()
		{
			this.ParameterName = name;
			this.OdbcType = type;
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x0024A960 File Offset: 0x00249D60
		public OdbcParameter(string name, OdbcType type, int size)
			: this()
		{
			this.ParameterName = name;
			this.OdbcType = type;
			this.Size = size;
		}

		// Token: 0x06001BDA RID: 7130 RVA: 0x0024A988 File Offset: 0x00249D88
		public OdbcParameter(string name, OdbcType type, int size, string sourcecolumn)
			: this()
		{
			this.ParameterName = name;
			this.OdbcType = type;
			this.Size = size;
			this.SourceColumn = sourcecolumn;
		}

		// Token: 0x06001BDB RID: 7131 RVA: 0x0024A9B8 File Offset: 0x00249DB8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public OdbcParameter(string parameterName, OdbcType odbcType, int size, ParameterDirection parameterDirection, bool isNullable, byte precision, byte scale, string srcColumn, DataRowVersion srcVersion, object value)
			: this()
		{
			this.ParameterName = parameterName;
			this.OdbcType = odbcType;
			this.Size = size;
			this.Direction = parameterDirection;
			this.IsNullable = isNullable;
			this.PrecisionInternal = precision;
			this.ScaleInternal = scale;
			this.SourceColumn = srcColumn;
			this.SourceVersion = srcVersion;
			this.Value = value;
		}

		// Token: 0x06001BDC RID: 7132 RVA: 0x0024AA18 File Offset: 0x00249E18
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public OdbcParameter(string parameterName, OdbcType odbcType, int size, ParameterDirection parameterDirection, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, bool sourceColumnNullMapping, object value)
			: this()
		{
			this.ParameterName = parameterName;
			this.OdbcType = odbcType;
			this.Size = size;
			this.Direction = parameterDirection;
			this.PrecisionInternal = precision;
			this.ScaleInternal = scale;
			this.SourceColumn = sourceColumn;
			this.SourceVersion = sourceVersion;
			this.SourceColumnNullMapping = sourceColumnNullMapping;
			this.Value = value;
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06001BDD RID: 7133 RVA: 0x0024AA78 File Offset: 0x00249E78
		// (set) Token: 0x06001BDE RID: 7134 RVA: 0x0024AAA4 File Offset: 0x00249EA4
		public override DbType DbType
		{
			get
			{
				if (this._userSpecifiedType)
				{
					return this._typemap._dbType;
				}
				return TypeMap._NVarChar._dbType;
			}
			set
			{
				if (this._typemap == null || this._typemap._dbType != value)
				{
					this.PropertyTypeChanging();
					this._typemap = TypeMap.FromDbType(value);
					this._userSpecifiedType = true;
				}
			}
		}

		// Token: 0x06001BDF RID: 7135 RVA: 0x0024AAE0 File Offset: 0x00249EE0
		public override void ResetDbType()
		{
			this.ResetOdbcType();
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06001BE0 RID: 7136 RVA: 0x0024AAF4 File Offset: 0x00249EF4
		// (set) Token: 0x06001BE1 RID: 7137 RVA: 0x0024AB20 File Offset: 0x00249F20
		[DbProviderSpecificTypeProperty(true)]
		[DefaultValue(OdbcType.NChar)]
		[RefreshProperties(RefreshProperties.All)]
		[ResCategory("DataCategory_Data")]
		[ResDescription("OdbcParameter_OdbcType")]
		public OdbcType OdbcType
		{
			get
			{
				if (this._userSpecifiedType)
				{
					return this._typemap._odbcType;
				}
				return TypeMap._NVarChar._odbcType;
			}
			set
			{
				if (this._typemap == null || this._typemap._odbcType != value)
				{
					this.PropertyTypeChanging();
					this._typemap = TypeMap.FromOdbcType(value);
					this._userSpecifiedType = true;
				}
			}
		}

		// Token: 0x06001BE2 RID: 7138 RVA: 0x0024AB5C File Offset: 0x00249F5C
		public void ResetOdbcType()
		{
			this.PropertyTypeChanging();
			this._typemap = null;
			this._userSpecifiedType = false;
		}

		// Token: 0x170003BA RID: 954
		// (set) Token: 0x06001BE3 RID: 7139 RVA: 0x0024AB80 File Offset: 0x00249F80
		internal bool HasChanged
		{
			set
			{
				this._hasChanged = value;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06001BE4 RID: 7140 RVA: 0x0024AB94 File Offset: 0x00249F94
		internal bool UserSpecifiedType
		{
			get
			{
				return this._userSpecifiedType;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06001BE5 RID: 7141 RVA: 0x0024ABA8 File Offset: 0x00249FA8
		// (set) Token: 0x06001BE6 RID: 7142 RVA: 0x0024ABC8 File Offset: 0x00249FC8
		[ResDescription("DbParameter_ParameterName")]
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
				if (this._parameterName != value)
				{
					this.PropertyChanging();
					this._parameterName = value;
				}
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06001BE7 RID: 7143 RVA: 0x0024ABF0 File Offset: 0x00249FF0
		// (set) Token: 0x06001BE8 RID: 7144 RVA: 0x0024AC04 File Offset: 0x0024A004
		[ResCategory("DataCategory_Data")]
		[DefaultValue(0)]
		[ResDescription("DbDataParameter_Precision")]
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

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06001BE9 RID: 7145 RVA: 0x0024AC18 File Offset: 0x0024A018
		// (set) Token: 0x06001BEA RID: 7146 RVA: 0x0024AC40 File Offset: 0x0024A040
		internal byte PrecisionInternal
		{
			get
			{
				byte b = this._precision;
				if (b == 0)
				{
					b = this.ValuePrecision(this.Value);
				}
				return b;
			}
			set
			{
				if (this._precision != value)
				{
					this.PropertyChanging();
					this._precision = value;
				}
			}
		}

		// Token: 0x06001BEB RID: 7147 RVA: 0x0024AC64 File Offset: 0x0024A064
		private bool ShouldSerializePrecision()
		{
			return 0 != this._precision;
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06001BEC RID: 7148 RVA: 0x0024AC80 File Offset: 0x0024A080
		// (set) Token: 0x06001BED RID: 7149 RVA: 0x0024AC94 File Offset: 0x0024A094
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

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06001BEE RID: 7150 RVA: 0x0024ACA8 File Offset: 0x0024A0A8
		// (set) Token: 0x06001BEF RID: 7151 RVA: 0x0024ACD4 File Offset: 0x0024A0D4
		internal byte ScaleInternal
		{
			get
			{
				byte b = this._scale;
				if (!this.ShouldSerializeScale(b))
				{
					b = this.ValueScale(this.Value);
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

		// Token: 0x06001BF0 RID: 7152 RVA: 0x0024AD08 File Offset: 0x0024A108
		private bool ShouldSerializeScale()
		{
			return this.ShouldSerializeScale(this._scale);
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x0024AD24 File Offset: 0x0024A124
		private bool ShouldSerializeScale(byte scale)
		{
			return this._hasScale && (scale != 0 || this.ShouldSerializePrecision());
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x0024AD48 File Offset: 0x0024A148
		private int GetColumnSize(object value, int offset, int ordinal)
		{
			if (ODBC32.SQL_C.NUMERIC == this._bindtype._sql_c && this._internalPrecision != 0)
			{
				return Math.Min((int)this._internalPrecision, 29);
			}
			int num = this._bindtype._columnSize;
			if (0 >= num)
			{
				if (ODBC32.SQL_C.NUMERIC == this._typemap._sql_c)
				{
					num = 62;
				}
				else
				{
					num = this._internalSize;
					if (!this._internalShouldSerializeSize || 1073741823 <= num || num < 0)
					{
						if (!this._internalShouldSerializeSize && (ParameterDirection.Output & this._internalDirection) != (ParameterDirection)0)
						{
							throw ADP.UninitializedParameterSize(ordinal, this._bindtype._type);
						}
						if (value == null || Convert.IsDBNull(value))
						{
							num = 0;
						}
						else if (value is string)
						{
							num = ((string)value).Length - offset;
							if ((ParameterDirection.Output & this._internalDirection) != (ParameterDirection)0 && 1073741823 <= this._internalSize)
							{
								num = Math.Max(num, 4096);
							}
							if (ODBC32.SQL_TYPE.CHAR == this._bindtype._sql_type || ODBC32.SQL_TYPE.VARCHAR == this._bindtype._sql_type || ODBC32.SQL_TYPE.LONGVARCHAR == this._bindtype._sql_type)
							{
								num = Encoding.Default.GetMaxByteCount(num);
							}
						}
						else if (value is char[])
						{
							num = ((char[])value).Length - offset;
							if ((ParameterDirection.Output & this._internalDirection) != (ParameterDirection)0 && 1073741823 <= this._internalSize)
							{
								num = Math.Max(num, 4096);
							}
							if (ODBC32.SQL_TYPE.CHAR == this._bindtype._sql_type || ODBC32.SQL_TYPE.VARCHAR == this._bindtype._sql_type || ODBC32.SQL_TYPE.LONGVARCHAR == this._bindtype._sql_type)
							{
								num = Encoding.Default.GetMaxByteCount(num);
							}
						}
						else if (value is byte[])
						{
							num = ((byte[])value).Length - offset;
							if ((ParameterDirection.Output & this._internalDirection) != (ParameterDirection)0 && 1073741823 <= this._internalSize)
							{
								num = Math.Max(num, 8192);
							}
						}
						num = Math.Max(2, num);
					}
				}
			}
			return num;
		}

		// Token: 0x06001BF3 RID: 7155 RVA: 0x0024AF1C File Offset: 0x0024A31C
		private int GetValueSize(object value, int offset)
		{
			if (ODBC32.SQL_C.NUMERIC == this._bindtype._sql_c && this._internalPrecision != 0)
			{
				return Math.Min((int)this._internalPrecision, 29);
			}
			int num = this._bindtype._columnSize;
			if (0 >= num)
			{
				bool flag = false;
				if (value is string)
				{
					num = ((string)value).Length - offset;
					flag = true;
				}
				else if (value is char[])
				{
					num = ((char[])value).Length - offset;
					flag = true;
				}
				else if (value is byte[])
				{
					num = ((byte[])value).Length - offset;
				}
				else
				{
					num = 0;
				}
				if (this._internalShouldSerializeSize && this._internalSize >= 0 && this._internalSize < num && this._bindtype == this._originalbindtype)
				{
					num = this._internalSize;
				}
				if (flag)
				{
					num *= 2;
				}
			}
			return num;
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x0024AFE4 File Offset: 0x0024A3E4
		private int GetParameterSize(object value, int offset, int ordinal)
		{
			int num = this._bindtype._bufferSize;
			if (0 >= num)
			{
				if (ODBC32.SQL_C.NUMERIC == this._typemap._sql_c)
				{
					num = 518;
				}
				else
				{
					num = this._internalSize;
					if (!this._internalShouldSerializeSize || 1073741823 <= num || num < 0)
					{
						if (num <= 0 && (ParameterDirection.Output & this._internalDirection) != (ParameterDirection)0)
						{
							throw ADP.UninitializedParameterSize(ordinal, this._bindtype._type);
						}
						if (value == null || Convert.IsDBNull(value))
						{
							if (this._bindtype._sql_c == ODBC32.SQL_C.WCHAR)
							{
								num = 2;
							}
							else
							{
								num = 0;
							}
						}
						else if (value is string)
						{
							num = (((string)value).Length - offset) * 2 + 2;
						}
						else if (value is char[])
						{
							num = (((char[])value).Length - offset) * 2 + 2;
						}
						else if (value is byte[])
						{
							num = ((byte[])value).Length - offset;
						}
						if ((ParameterDirection.Output & this._internalDirection) != (ParameterDirection)0 && 1073741823 <= this._internalSize)
						{
							num = Math.Max(num, 8192);
						}
					}
					else if (ODBC32.SQL_C.WCHAR == this._bindtype._sql_c)
					{
						if (value is string && num < ((string)value).Length && this._bindtype == this._originalbindtype)
						{
							num = ((string)value).Length;
						}
						num = num * 2 + 2;
					}
					else if (value is byte[] && num < ((byte[])value).Length && this._bindtype == this._originalbindtype)
					{
						num = ((byte[])value).Length;
					}
				}
			}
			return num;
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x0024B164 File Offset: 0x0024A564
		private byte GetParameterPrecision(object value)
		{
			if (this._internalPrecision != 0 && value is decimal)
			{
				if (this._internalPrecision < 29)
				{
					if (this._internalPrecision != 0)
					{
						byte precision = ((decimal)value).Precision;
						this._internalPrecision = Math.Max(this._internalPrecision, precision);
					}
					return this._internalPrecision;
				}
				return 29;
			}
			else
			{
				if (value == null || value is decimal || Convert.IsDBNull(value))
				{
					return 28;
				}
				return 0;
			}
		}

		// Token: 0x06001BF6 RID: 7158 RVA: 0x0024B1DC File Offset: 0x0024A5DC
		private byte GetParameterScale(object value)
		{
			if (!(value is decimal))
			{
				return this._internalScale;
			}
			byte b = (byte)((decimal.GetBits((decimal)value)[3] & 16711680) >> 16);
			if (this._internalScale > 0 && this._internalScale < b)
			{
				return this._internalScale;
			}
			return b;
		}

		// Token: 0x06001BF7 RID: 7159 RVA: 0x0024B22C File Offset: 0x0024A62C
		object ICloneable.Clone()
		{
			return new OdbcParameter(this);
		}

		// Token: 0x06001BF8 RID: 7160 RVA: 0x0024B240 File Offset: 0x0024A640
		private void CopyParameterInternal()
		{
			this._internalValue = this.Value;
			this._internalPrecision = (this.ShouldSerializePrecision() ? this.PrecisionInternal : this.ValuePrecision(this._internalValue));
			this._internalShouldSerializeSize = this.ShouldSerializeSize();
			this._internalSize = (this._internalShouldSerializeSize ? this.Size : this.ValueSize(this._internalValue));
			this._internalDirection = this.Direction;
			this._internalScale = (this.ShouldSerializeScale() ? this.ScaleInternal : this.ValueScale(this._internalValue));
			this._internalOffset = this.Offset;
			this._internalUserSpecifiedType = this.UserSpecifiedType;
		}

		// Token: 0x06001BF9 RID: 7161 RVA: 0x0024B2F0 File Offset: 0x0024A6F0
		private void CloneHelper(OdbcParameter destination)
		{
			this.CloneHelperCore(destination);
			destination._userSpecifiedType = this._userSpecifiedType;
			destination._typemap = this._typemap;
			destination._parameterName = this._parameterName;
			destination._precision = this._precision;
			destination._scale = this._scale;
			destination._hasScale = this._hasScale;
		}

		// Token: 0x06001BFA RID: 7162 RVA: 0x0024B34C File Offset: 0x0024A74C
		internal void ClearBinding()
		{
			if (!this._userSpecifiedType)
			{
				this._typemap = null;
			}
			this._bindtype = null;
		}

		// Token: 0x06001BFB RID: 7163 RVA: 0x0024B370 File Offset: 0x0024A770
		internal void PrepareForBind(OdbcCommand command, short ordinal, ref int parameterBufferSize)
		{
			this.CopyParameterInternal();
			object obj = this.ProcessAndGetParameterValue();
			int num = this._internalOffset;
			int num2 = this._internalSize;
			if (num > 0)
			{
				if (obj is string)
				{
					if (num > ((string)obj).Length)
					{
						throw ADP.OffsetOutOfRangeException();
					}
				}
				else if (obj is char[])
				{
					if (num > ((char[])obj).Length)
					{
						throw ADP.OffsetOutOfRangeException();
					}
				}
				else if (obj is byte[])
				{
					if (num > ((byte[])obj).Length)
					{
						throw ADP.OffsetOutOfRangeException();
					}
				}
				else
				{
					num = 0;
				}
			}
			ODBC32.SQL_TYPE sql_type = this._bindtype._sql_type;
			switch (sql_type)
			{
			case ODBC32.SQL_TYPE.WLONGVARCHAR:
			case ODBC32.SQL_TYPE.WVARCHAR:
			case ODBC32.SQL_TYPE.WCHAR:
				if (obj is char)
				{
					obj = obj.ToString();
					num2 = ((string)obj).Length;
					num = 0;
				}
				if (!command.Connection.TestTypeSupport(this._bindtype._sql_type))
				{
					if (ODBC32.SQL_TYPE.WCHAR == this._bindtype._sql_type)
					{
						this._bindtype = TypeMap._Char;
					}
					else if (ODBC32.SQL_TYPE.WVARCHAR == this._bindtype._sql_type)
					{
						this._bindtype = TypeMap._VarChar;
					}
					else if (ODBC32.SQL_TYPE.WLONGVARCHAR == this._bindtype._sql_type)
					{
						this._bindtype = TypeMap._Text;
					}
				}
				break;
			case ODBC32.SQL_TYPE.BIT:
			case ODBC32.SQL_TYPE.TINYINT:
				break;
			case ODBC32.SQL_TYPE.BIGINT:
				if (!command.Connection.IsV3Driver)
				{
					this._bindtype = TypeMap._VarChar;
					if (obj != null && !Convert.IsDBNull(obj))
					{
						obj = ((long)obj).ToString(CultureInfo.CurrentCulture);
						num2 = ((string)obj).Length;
						num = 0;
					}
				}
				break;
			default:
				switch (sql_type)
				{
				case ODBC32.SQL_TYPE.NUMERIC:
				case ODBC32.SQL_TYPE.DECIMAL:
					if (!command.Connection.IsV3Driver || !command.Connection.TestTypeSupport(ODBC32.SQL_TYPE.NUMERIC) || command.Connection.TestRestrictedSqlBindType(this._bindtype._sql_type))
					{
						this._bindtype = TypeMap._VarChar;
						if (obj != null && !Convert.IsDBNull(obj))
						{
							obj = ((decimal)obj).ToString(CultureInfo.CurrentCulture);
							num2 = ((string)obj).Length;
							num = 0;
						}
					}
					break;
				}
				break;
			}
			ODBC32.SQL_C sql_C = this._bindtype._sql_c;
			if (!command.Connection.IsV3Driver && sql_C == ODBC32.SQL_C.WCHAR)
			{
				sql_C = ODBC32.SQL_C.CHAR;
				if (obj != null && !Convert.IsDBNull(obj) && obj is string)
				{
					int lcid = CultureInfo.CurrentCulture.LCID;
					CultureInfo cultureInfo = new CultureInfo(lcid);
					Encoding encoding = Encoding.GetEncoding(cultureInfo.TextInfo.ANSICodePage);
					obj = encoding.GetBytes(obj.ToString());
					num2 = ((byte[])obj).Length;
				}
			}
			int parameterSize = this.GetParameterSize(obj, num, (int)ordinal);
			ODBC32.SQL_TYPE sql_type2 = this._bindtype._sql_type;
			if (sql_type2 != ODBC32.SQL_TYPE.WVARCHAR)
			{
				if (sql_type2 != ODBC32.SQL_TYPE.VARBINARY)
				{
					if (sql_type2 == ODBC32.SQL_TYPE.VARCHAR)
					{
						if (parameterSize > 8000)
						{
							this._bindtype = TypeMap._Text;
						}
					}
				}
				else if (parameterSize > 8000)
				{
					this._bindtype = TypeMap._Image;
				}
			}
			else if (parameterSize > 4000)
			{
				this._bindtype = TypeMap._NText;
			}
			this._prepared_Sql_C_Type = sql_C;
			this._preparedOffset = num;
			this._preparedSize = num2;
			this._preparedValue = obj;
			this._preparedBufferSize = parameterSize;
			this._preparedIntOffset = parameterBufferSize;
			this._preparedValueOffset = this._preparedIntOffset + IntPtr.Size;
			parameterBufferSize += parameterSize + IntPtr.Size;
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x0024B6B0 File Offset: 0x0024AAB0
		internal void Bind(OdbcStatementHandle hstmt, OdbcCommand command, short ordinal, CNativeBuffer parameterBuffer, bool allowReentrance)
		{
			ODBC32.SQL_C prepared_Sql_C_Type = this._prepared_Sql_C_Type;
			ODBC32.SQL_PARAM sql_PARAM = this.SqlDirectionFromParameterDirection();
			int preparedOffset = this._preparedOffset;
			int preparedSize = this._preparedSize;
			object obj = this._preparedValue;
			int valueSize = this.GetValueSize(obj, preparedOffset);
			int columnSize = this.GetColumnSize(obj, preparedOffset, (int)ordinal);
			byte parameterPrecision = this.GetParameterPrecision(obj);
			byte b = this.GetParameterScale(obj);
			HandleRef handleRef = parameterBuffer.PtrOffset(this._preparedValueOffset, this._preparedBufferSize);
			HandleRef handleRef2 = parameterBuffer.PtrOffset(this._preparedIntOffset, IntPtr.Size);
			if (ODBC32.SQL_C.NUMERIC == prepared_Sql_C_Type)
			{
				if (ODBC32.SQL_PARAM.INPUT_OUTPUT == sql_PARAM && obj is decimal && b < this._internalScale)
				{
					while (b < this._internalScale)
					{
						obj = (decimal)obj * 10m;
						b += 1;
					}
				}
				this.SetInputValue(obj, prepared_Sql_C_Type, valueSize, (int)parameterPrecision, 0, parameterBuffer);
				if (ODBC32.SQL_PARAM.INPUT != sql_PARAM)
				{
					parameterBuffer.WriteInt16(this._preparedValueOffset, (short)(((int)b << 8) | (int)parameterPrecision));
				}
			}
			else
			{
				this.SetInputValue(obj, prepared_Sql_C_Type, valueSize, preparedSize, preparedOffset, parameterBuffer);
			}
			if (!this._hasChanged && this._boundSqlCType == prepared_Sql_C_Type && this._boundParameterType == this._bindtype._sql_type && this._boundSize == columnSize && this._boundScale == (int)b && this._boundBuffer == handleRef.Handle && this._boundIntbuffer == handleRef2.Handle)
			{
				return;
			}
			ODBC32.RetCode retCode = hstmt.BindParameter(ordinal, (short)sql_PARAM, prepared_Sql_C_Type, this._bindtype._sql_type, (IntPtr)columnSize, (IntPtr)((int)b), handleRef, (IntPtr)this._preparedBufferSize, handleRef2);
			if (retCode != ODBC32.RetCode.SUCCESS)
			{
				if ("07006" == command.GetDiagSqlState())
				{
					Bid.Trace("<odbc.OdbcParameter.Bind|ERR> Call to BindParameter returned errorcode [07006]\n");
					command.Connection.FlagRestrictedSqlBindType(this._bindtype._sql_type);
					if (allowReentrance)
					{
						this.Bind(hstmt, command, ordinal, parameterBuffer, false);
						return;
					}
				}
				command.Connection.HandleError(hstmt, retCode);
			}
			this._hasChanged = false;
			this._boundSqlCType = prepared_Sql_C_Type;
			this._boundParameterType = this._bindtype._sql_type;
			this._boundSize = columnSize;
			this._boundScale = (int)b;
			this._boundBuffer = handleRef.Handle;
			this._boundIntbuffer = handleRef2.Handle;
			if (ODBC32.SQL_C.NUMERIC == prepared_Sql_C_Type)
			{
				OdbcDescriptorHandle descriptorHandle = command.GetDescriptorHandle(ODBC32.SQL_ATTR.APP_PARAM_DESC);
				retCode = descriptorHandle.SetDescriptionField1(ordinal, ODBC32.SQL_DESC.TYPE, (IntPtr)2L);
				if (retCode != ODBC32.RetCode.SUCCESS)
				{
					command.Connection.HandleError(hstmt, retCode);
				}
				int num = (int)parameterPrecision;
				retCode = descriptorHandle.SetDescriptionField1(ordinal, ODBC32.SQL_DESC.PRECISION, (IntPtr)num);
				if (retCode != ODBC32.RetCode.SUCCESS)
				{
					command.Connection.HandleError(hstmt, retCode);
				}
				num = (int)b;
				retCode = descriptorHandle.SetDescriptionField1(ordinal, ODBC32.SQL_DESC.SCALE, (IntPtr)num);
				if (retCode != ODBC32.RetCode.SUCCESS)
				{
					command.Connection.HandleError(hstmt, retCode);
				}
				retCode = descriptorHandle.SetDescriptionField2(ordinal, ODBC32.SQL_DESC.DATA_PTR, handleRef);
				if (retCode != ODBC32.RetCode.SUCCESS)
				{
					command.Connection.HandleError(hstmt, retCode);
				}
			}
		}

		// Token: 0x06001BFD RID: 7165 RVA: 0x0024B988 File Offset: 0x0024AD88
		internal void GetOutputValue(CNativeBuffer parameterBuffer)
		{
			if (this._hasChanged)
			{
				return;
			}
			if (this._bindtype != null && this._internalDirection != ParameterDirection.Input)
			{
				TypeMap bindtype = this._bindtype;
				this._bindtype = null;
				int num = (int)parameterBuffer.ReadIntPtr(this._preparedIntOffset);
				if (-1 == num)
				{
					this.Value = DBNull.Value;
					return;
				}
				if (0 <= num || num == -3)
				{
					this.Value = parameterBuffer.MarshalToManaged(this._preparedValueOffset, this._boundSqlCType, num);
					if (this._boundSqlCType == ODBC32.SQL_C.CHAR && this.Value != null && !Convert.IsDBNull(this.Value))
					{
						int lcid = CultureInfo.CurrentCulture.LCID;
						CultureInfo cultureInfo = new CultureInfo(lcid);
						Encoding encoding = Encoding.GetEncoding(cultureInfo.TextInfo.ANSICodePage);
						this.Value = encoding.GetString((byte[])this.Value);
					}
					if (bindtype != this._typemap && this.Value != null && !Convert.IsDBNull(this.Value) && this.Value.GetType() != this._typemap._type)
					{
						this.Value = decimal.Parse((string)this.Value, CultureInfo.CurrentCulture);
					}
				}
			}
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x0024BABC File Offset: 0x0024AEBC
		private object ProcessAndGetParameterValue()
		{
			object obj = this._internalValue;
			if (this._internalUserSpecifiedType)
			{
				if (obj != null && !Convert.IsDBNull(obj))
				{
					Type type = obj.GetType();
					if (!type.IsArray)
					{
						if (type == this._typemap._type)
						{
							goto IL_00C8;
						}
						try
						{
							obj = Convert.ChangeType(obj, this._typemap._type, null);
							goto IL_00C8;
						}
						catch (Exception ex)
						{
							if (!ADP.IsCatchableExceptionType(ex))
							{
								throw;
							}
							throw ADP.ParameterConversionFailed(obj, this._typemap._type, ex);
						}
					}
					if (type == typeof(char[]))
					{
						obj = new string((char[])obj);
					}
				}
			}
			else if (this._typemap == null)
			{
				if (obj == null || Convert.IsDBNull(obj))
				{
					this._typemap = TypeMap._NVarChar;
				}
				else
				{
					Type type2 = obj.GetType();
					this._typemap = TypeMap.FromSystemType(type2);
				}
			}
			IL_00C8:
			this._originalbindtype = (this._bindtype = this._typemap);
			return obj;
		}

		// Token: 0x06001BFF RID: 7167 RVA: 0x0024BBC4 File Offset: 0x0024AFC4
		private void PropertyChanging()
		{
			this._hasChanged = true;
		}

		// Token: 0x06001C00 RID: 7168 RVA: 0x0024BBD8 File Offset: 0x0024AFD8
		private void PropertyTypeChanging()
		{
			this.PropertyChanging();
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x0024BBEC File Offset: 0x0024AFEC
		internal void SetInputValue(object value, ODBC32.SQL_C sql_c_type, int cbsize, int sizeorprecision, int offset, CNativeBuffer parameterBuffer)
		{
			if (ParameterDirection.Input != this._internalDirection && ParameterDirection.InputOutput != this._internalDirection)
			{
				this._internalValue = null;
				parameterBuffer.WriteIntPtr(this._preparedIntOffset, (IntPtr)(-1));
				return;
			}
			if (value == null)
			{
				parameterBuffer.WriteIntPtr(this._preparedIntOffset, (IntPtr)(-5));
				return;
			}
			if (Convert.IsDBNull(value))
			{
				parameterBuffer.WriteIntPtr(this._preparedIntOffset, (IntPtr)(-1));
				return;
			}
			if (sql_c_type == ODBC32.SQL_C.WCHAR || sql_c_type == ODBC32.SQL_C.BINARY || sql_c_type == ODBC32.SQL_C.CHAR)
			{
				parameterBuffer.WriteIntPtr(this._preparedIntOffset, (IntPtr)cbsize);
			}
			else
			{
				parameterBuffer.WriteIntPtr(this._preparedIntOffset, IntPtr.Zero);
			}
			parameterBuffer.MarshalToNative(this._preparedValueOffset, value, sql_c_type, sizeorprecision, offset);
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x0024BCA8 File Offset: 0x0024B0A8
		private ODBC32.SQL_PARAM SqlDirectionFromParameterDirection()
		{
			switch (this._internalDirection)
			{
			case ParameterDirection.Input:
				return ODBC32.SQL_PARAM.INPUT;
			case ParameterDirection.Output:
			case ParameterDirection.ReturnValue:
				return ODBC32.SQL_PARAM.OUTPUT;
			case ParameterDirection.InputOutput:
				return ODBC32.SQL_PARAM.INPUT_OUTPUT;
			}
			return ODBC32.SQL_PARAM.INPUT;
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06001C03 RID: 7171 RVA: 0x0024BCE8 File Offset: 0x0024B0E8
		// (set) Token: 0x06001C04 RID: 7172 RVA: 0x0024BCFC File Offset: 0x0024B0FC
		[RefreshProperties(RefreshProperties.All)]
		[ResCategory("DataCategory_Data")]
		[TypeConverter(typeof(StringConverter))]
		[ResDescription("DbParameter_Value")]
		public override object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._coercedValue = null;
				this._value = value;
			}
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x0024BD18 File Offset: 0x0024B118
		private byte ValuePrecision(object value)
		{
			return this.ValuePrecisionCore(value);
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x0024BD2C File Offset: 0x0024B12C
		private byte ValueScale(object value)
		{
			return this.ValueScaleCore(value);
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x0024BD40 File Offset: 0x0024B140
		private int ValueSize(object value)
		{
			return this.ValueSizeCore(value);
		}

		// Token: 0x06001C08 RID: 7176 RVA: 0x0024BD54 File Offset: 0x0024B154
		private OdbcParameter(OdbcParameter source)
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

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06001C09 RID: 7177 RVA: 0x0024BD94 File Offset: 0x0024B194
		// (set) Token: 0x06001C0A RID: 7178 RVA: 0x0024BDA8 File Offset: 0x0024B1A8
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

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06001C0B RID: 7179 RVA: 0x0024BDBC File Offset: 0x0024B1BC
		// (set) Token: 0x06001C0C RID: 7180 RVA: 0x0024BDD8 File Offset: 0x0024B1D8
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("DbParameter_Direction")]
		[ResCategory("DataCategory_Data")]
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

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06001C0D RID: 7181 RVA: 0x0024BE28 File Offset: 0x0024B228
		// (set) Token: 0x06001C0E RID: 7182 RVA: 0x0024BE3C File Offset: 0x0024B23C
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

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06001C0F RID: 7183 RVA: 0x0024BE50 File Offset: 0x0024B250
		internal int Offset
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06001C10 RID: 7184 RVA: 0x0024BE60 File Offset: 0x0024B260
		// (set) Token: 0x06001C11 RID: 7185 RVA: 0x0024BE88 File Offset: 0x0024B288
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

		// Token: 0x06001C12 RID: 7186 RVA: 0x0024BEB8 File Offset: 0x0024B2B8
		private void ResetSize()
		{
			if (this._size != 0)
			{
				this.PropertyChanging();
				this._size = 0;
			}
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x0024BEDC File Offset: 0x0024B2DC
		private bool ShouldSerializeSize()
		{
			return 0 != this._size;
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06001C14 RID: 7188 RVA: 0x0024BEF8 File Offset: 0x0024B2F8
		// (set) Token: 0x06001C15 RID: 7189 RVA: 0x0024BF18 File Offset: 0x0024B318
		[ResDescription("DbParameter_SourceColumn")]
		[ResCategory("DataCategory_Update")]
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

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06001C16 RID: 7190 RVA: 0x0024BF2C File Offset: 0x0024B32C
		// (set) Token: 0x06001C17 RID: 7191 RVA: 0x0024BF40 File Offset: 0x0024B340
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

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06001C18 RID: 7192 RVA: 0x0024BF54 File Offset: 0x0024B354
		// (set) Token: 0x06001C19 RID: 7193 RVA: 0x0024BF74 File Offset: 0x0024B374
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

		// Token: 0x06001C1A RID: 7194 RVA: 0x0024BFBC File Offset: 0x0024B3BC
		private void CloneHelperCore(OdbcParameter destination)
		{
			destination._value = this._value;
			destination._direction = this._direction;
			destination._size = this._size;
			destination._sourceColumn = this._sourceColumn;
			destination._sourceVersion = this._sourceVersion;
			destination._sourceColumnNullMapping = this._sourceColumnNullMapping;
			destination._isNullable = this._isNullable;
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x0024C020 File Offset: 0x0024B420
		internal void CopyTo(DbParameter destination)
		{
			ADP.CheckArgumentNull(destination, "destination");
			this.CloneHelper((OdbcParameter)destination);
		}

		// Token: 0x06001C1C RID: 7196 RVA: 0x0024C044 File Offset: 0x0024B444
		internal object CompareExchangeParent(object value, object comparand)
		{
			object parent = this._parent;
			if (comparand == parent)
			{
				this._parent = value;
			}
			return parent;
		}

		// Token: 0x06001C1D RID: 7197 RVA: 0x0024C064 File Offset: 0x0024B464
		internal void ResetParent()
		{
			this._parent = null;
		}

		// Token: 0x06001C1E RID: 7198 RVA: 0x0024C078 File Offset: 0x0024B478
		public override string ToString()
		{
			return this.ParameterName;
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x0024C08C File Offset: 0x0024B48C
		private byte ValuePrecisionCore(object value)
		{
			if (value is decimal)
			{
				return ((decimal)value).Precision;
			}
			return 0;
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x0024C0B8 File Offset: 0x0024B4B8
		private byte ValueScaleCore(object value)
		{
			if (value is decimal)
			{
				return (byte)((decimal.GetBits((decimal)value)[3] & 16711680) >> 16);
			}
			return 0;
		}

		// Token: 0x06001C21 RID: 7201 RVA: 0x0024C0E8 File Offset: 0x0024B4E8
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

		// Token: 0x04001033 RID: 4147
		private bool _hasChanged;

		// Token: 0x04001034 RID: 4148
		private bool _userSpecifiedType;

		// Token: 0x04001035 RID: 4149
		private TypeMap _typemap;

		// Token: 0x04001036 RID: 4150
		private TypeMap _bindtype;

		// Token: 0x04001037 RID: 4151
		private string _parameterName;

		// Token: 0x04001038 RID: 4152
		private byte _precision;

		// Token: 0x04001039 RID: 4153
		private byte _scale;

		// Token: 0x0400103A RID: 4154
		private bool _hasScale;

		// Token: 0x0400103B RID: 4155
		private ODBC32.SQL_C _boundSqlCType;

		// Token: 0x0400103C RID: 4156
		private ODBC32.SQL_TYPE _boundParameterType;

		// Token: 0x0400103D RID: 4157
		private int _boundSize;

		// Token: 0x0400103E RID: 4158
		private int _boundScale;

		// Token: 0x0400103F RID: 4159
		private IntPtr _boundBuffer;

		// Token: 0x04001040 RID: 4160
		private IntPtr _boundIntbuffer;

		// Token: 0x04001041 RID: 4161
		private TypeMap _originalbindtype;

		// Token: 0x04001042 RID: 4162
		private byte _internalPrecision;

		// Token: 0x04001043 RID: 4163
		private bool _internalShouldSerializeSize;

		// Token: 0x04001044 RID: 4164
		private int _internalSize;

		// Token: 0x04001045 RID: 4165
		private ParameterDirection _internalDirection;

		// Token: 0x04001046 RID: 4166
		private byte _internalScale;

		// Token: 0x04001047 RID: 4167
		private int _internalOffset;

		// Token: 0x04001048 RID: 4168
		internal bool _internalUserSpecifiedType;

		// Token: 0x04001049 RID: 4169
		private object _internalValue;

		// Token: 0x0400104A RID: 4170
		private int _preparedOffset;

		// Token: 0x0400104B RID: 4171
		private int _preparedSize;

		// Token: 0x0400104C RID: 4172
		private int _preparedBufferSize;

		// Token: 0x0400104D RID: 4173
		private object _preparedValue;

		// Token: 0x0400104E RID: 4174
		private int _preparedIntOffset;

		// Token: 0x0400104F RID: 4175
		private int _preparedValueOffset;

		// Token: 0x04001050 RID: 4176
		private ODBC32.SQL_C _prepared_Sql_C_Type;

		// Token: 0x04001051 RID: 4177
		private object _value;

		// Token: 0x04001052 RID: 4178
		private object _parent;

		// Token: 0x04001053 RID: 4179
		private ParameterDirection _direction;

		// Token: 0x04001054 RID: 4180
		private int _size;

		// Token: 0x04001055 RID: 4181
		private string _sourceColumn;

		// Token: 0x04001056 RID: 4182
		private DataRowVersion _sourceVersion;

		// Token: 0x04001057 RID: 4183
		private bool _sourceColumnNullMapping;

		// Token: 0x04001058 RID: 4184
		private bool _isNullable;

		// Token: 0x04001059 RID: 4185
		private object _coercedValue;

		// Token: 0x020001F8 RID: 504
		internal sealed class OdbcParameterConverter : ExpandableObjectConverter
		{
			// Token: 0x06001C23 RID: 7203 RVA: 0x0024C154 File Offset: 0x0024B554
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
			}

			// Token: 0x06001C24 RID: 7204 RVA: 0x0024C178 File Offset: 0x0024B578
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == null)
				{
					throw ADP.ArgumentNull("destinationType");
				}
				if (destinationType == typeof(InstanceDescriptor) && value is OdbcParameter)
				{
					OdbcParameter odbcParameter = (OdbcParameter)value;
					int num = 0;
					if (OdbcType.NChar != odbcParameter.OdbcType)
					{
						num |= 1;
					}
					if (odbcParameter.ShouldSerializeSize())
					{
						num |= 2;
					}
					if (!ADP.IsEmpty(odbcParameter.SourceColumn))
					{
						num |= 4;
					}
					if (odbcParameter.Value != null)
					{
						num |= 8;
					}
					if (ParameterDirection.Input != odbcParameter.Direction || odbcParameter.IsNullable || odbcParameter.ShouldSerializePrecision() || odbcParameter.ShouldSerializeScale() || DataRowVersion.Current != odbcParameter.SourceVersion)
					{
						num |= 16;
					}
					if (odbcParameter.SourceColumnNullMapping)
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
							typeof(OdbcType)
						};
						array2 = new object[] { odbcParameter.ParameterName, odbcParameter.OdbcType };
						break;
					case 2:
					case 3:
						array = new Type[]
						{
							typeof(string),
							typeof(OdbcType),
							typeof(int)
						};
						array2 = new object[] { odbcParameter.ParameterName, odbcParameter.OdbcType, odbcParameter.Size };
						break;
					case 4:
					case 5:
					case 6:
					case 7:
						array = new Type[]
						{
							typeof(string),
							typeof(OdbcType),
							typeof(int),
							typeof(string)
						};
						array2 = new object[] { odbcParameter.ParameterName, odbcParameter.OdbcType, odbcParameter.Size, odbcParameter.SourceColumn };
						break;
					case 8:
						array = new Type[]
						{
							typeof(string),
							typeof(object)
						};
						array2 = new object[] { odbcParameter.ParameterName, odbcParameter.Value };
						break;
					default:
						if ((32 & num) == 0)
						{
							array = new Type[]
							{
								typeof(string),
								typeof(OdbcType),
								typeof(int),
								typeof(ParameterDirection),
								typeof(bool),
								typeof(byte),
								typeof(byte),
								typeof(string),
								typeof(DataRowVersion),
								typeof(object)
							};
							array2 = new object[] { odbcParameter.ParameterName, odbcParameter.OdbcType, odbcParameter.Size, odbcParameter.Direction, odbcParameter.IsNullable, odbcParameter.PrecisionInternal, odbcParameter.ScaleInternal, odbcParameter.SourceColumn, odbcParameter.SourceVersion, odbcParameter.Value };
						}
						else
						{
							array = new Type[]
							{
								typeof(string),
								typeof(OdbcType),
								typeof(int),
								typeof(ParameterDirection),
								typeof(byte),
								typeof(byte),
								typeof(string),
								typeof(DataRowVersion),
								typeof(bool),
								typeof(object)
							};
							array2 = new object[] { odbcParameter.ParameterName, odbcParameter.OdbcType, odbcParameter.Size, odbcParameter.Direction, odbcParameter.PrecisionInternal, odbcParameter.ScaleInternal, odbcParameter.SourceColumn, odbcParameter.SourceVersion, odbcParameter.SourceColumnNullMapping, odbcParameter.Value };
						}
						break;
					}
					ConstructorInfo constructor = typeof(OdbcParameter).GetConstructor(array);
					if (constructor != null)
					{
						return new InstanceDescriptor(constructor, array2);
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}
	}
}
