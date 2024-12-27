using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data.Common;
using System.Globalization;
using System.Reflection;

namespace System.Data.OleDb
{
	// Token: 0x02000232 RID: 562
	[TypeConverter(typeof(OleDbParameter.OleDbParameterConverter))]
	public sealed class OleDbParameter : DbParameter, ICloneable, IDbDataParameter, IDataParameter
	{
		// Token: 0x06001FC7 RID: 8135 RVA: 0x0025FD60 File Offset: 0x0025F160
		public OleDbParameter()
		{
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x0025FD74 File Offset: 0x0025F174
		public OleDbParameter(string name, object value)
			: this()
		{
			this.ParameterName = name;
			this.Value = value;
		}

		// Token: 0x06001FC9 RID: 8137 RVA: 0x0025FD98 File Offset: 0x0025F198
		public OleDbParameter(string name, OleDbType dataType)
			: this()
		{
			this.ParameterName = name;
			this.OleDbType = dataType;
		}

		// Token: 0x06001FCA RID: 8138 RVA: 0x0025FDBC File Offset: 0x0025F1BC
		public OleDbParameter(string name, OleDbType dataType, int size)
			: this()
		{
			this.ParameterName = name;
			this.OleDbType = dataType;
			this.Size = size;
		}

		// Token: 0x06001FCB RID: 8139 RVA: 0x0025FDE4 File Offset: 0x0025F1E4
		public OleDbParameter(string name, OleDbType dataType, int size, string srcColumn)
			: this()
		{
			this.ParameterName = name;
			this.OleDbType = dataType;
			this.Size = size;
			this.SourceColumn = srcColumn;
		}

		// Token: 0x06001FCC RID: 8140 RVA: 0x0025FE14 File Offset: 0x0025F214
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public OleDbParameter(string parameterName, OleDbType dbType, int size, ParameterDirection direction, bool isNullable, byte precision, byte scale, string srcColumn, DataRowVersion srcVersion, object value)
			: this()
		{
			this.ParameterName = parameterName;
			this.OleDbType = dbType;
			this.Size = size;
			this.Direction = direction;
			this.IsNullable = isNullable;
			this.PrecisionInternal = precision;
			this.ScaleInternal = scale;
			this.SourceColumn = srcColumn;
			this.SourceVersion = srcVersion;
			this.Value = value;
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x0025FE74 File Offset: 0x0025F274
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public OleDbParameter(string parameterName, OleDbType dbType, int size, ParameterDirection direction, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, bool sourceColumnNullMapping, object value)
			: this()
		{
			this.ParameterName = parameterName;
			this.OleDbType = dbType;
			this.Size = size;
			this.Direction = direction;
			this.PrecisionInternal = precision;
			this.ScaleInternal = scale;
			this.SourceColumn = sourceColumn;
			this.SourceVersion = sourceVersion;
			this.SourceColumnNullMapping = sourceColumnNullMapping;
			this.Value = value;
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06001FCE RID: 8142 RVA: 0x0025FED4 File Offset: 0x0025F2D4
		internal int ChangeID
		{
			get
			{
				return this._changeID;
			}
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06001FCF RID: 8143 RVA: 0x0025FEE8 File Offset: 0x0025F2E8
		// (set) Token: 0x06001FD0 RID: 8144 RVA: 0x0025FF08 File Offset: 0x0025F308
		public override DbType DbType
		{
			get
			{
				return this.GetBindType(this.Value).enumDbType;
			}
			set
			{
				NativeDBType metaType = this._metaType;
				if (metaType == null || metaType.enumDbType != value)
				{
					this.PropertyTypeChanging();
					this._metaType = NativeDBType.FromDbType(value);
				}
			}
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x0025FF3C File Offset: 0x0025F33C
		public override void ResetDbType()
		{
			this.ResetOleDbType();
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06001FD2 RID: 8146 RVA: 0x0025FF50 File Offset: 0x0025F350
		// (set) Token: 0x06001FD3 RID: 8147 RVA: 0x0025FF70 File Offset: 0x0025F370
		[ResCategory("DataCategory_Data")]
		[DbProviderSpecificTypeProperty(true)]
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("OleDbParameter_OleDbType")]
		public OleDbType OleDbType
		{
			get
			{
				return this.GetBindType(this.Value).enumOleDbType;
			}
			set
			{
				NativeDBType metaType = this._metaType;
				if (metaType == null || metaType.enumOleDbType != value)
				{
					this.PropertyTypeChanging();
					this._metaType = NativeDBType.FromDataType(value);
				}
			}
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x0025FFA4 File Offset: 0x0025F3A4
		private bool ShouldSerializeOleDbType()
		{
			return null != this._metaType;
		}

		// Token: 0x06001FD5 RID: 8149 RVA: 0x0025FFC0 File Offset: 0x0025F3C0
		public void ResetOleDbType()
		{
			if (this._metaType != null)
			{
				this.PropertyTypeChanging();
				this._metaType = null;
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06001FD6 RID: 8150 RVA: 0x0025FFE4 File Offset: 0x0025F3E4
		// (set) Token: 0x06001FD7 RID: 8151 RVA: 0x00260004 File Offset: 0x0025F404
		[ResCategory("DataCategory_Data")]
		[ResDescription("DbParameter_ParameterName")]
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

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06001FD8 RID: 8152 RVA: 0x0026002C File Offset: 0x0025F42C
		// (set) Token: 0x06001FD9 RID: 8153 RVA: 0x00260040 File Offset: 0x0025F440
		[ResDescription("DbDataParameter_Precision")]
		[DefaultValue(0)]
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

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06001FDA RID: 8154 RVA: 0x00260054 File Offset: 0x0025F454
		// (set) Token: 0x06001FDB RID: 8155 RVA: 0x0026007C File Offset: 0x0025F47C
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

		// Token: 0x06001FDC RID: 8156 RVA: 0x002600A0 File Offset: 0x0025F4A0
		private bool ShouldSerializePrecision()
		{
			return 0 != this._precision;
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06001FDD RID: 8157 RVA: 0x002600BC File Offset: 0x0025F4BC
		// (set) Token: 0x06001FDE RID: 8158 RVA: 0x002600D0 File Offset: 0x0025F4D0
		[ResCategory("DataCategory_Data")]
		[DefaultValue(0)]
		[ResDescription("DbDataParameter_Scale")]
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

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06001FDF RID: 8159 RVA: 0x002600E4 File Offset: 0x0025F4E4
		// (set) Token: 0x06001FE0 RID: 8160 RVA: 0x00260110 File Offset: 0x0025F510
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

		// Token: 0x06001FE1 RID: 8161 RVA: 0x00260144 File Offset: 0x0025F544
		private bool ShouldSerializeScale()
		{
			return this.ShouldSerializeScale(this._scale);
		}

		// Token: 0x06001FE2 RID: 8162 RVA: 0x00260160 File Offset: 0x0025F560
		private bool ShouldSerializeScale(byte scale)
		{
			return this._hasScale && (scale != 0 || this.ShouldSerializePrecision());
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x00260184 File Offset: 0x0025F584
		object ICloneable.Clone()
		{
			return new OleDbParameter(this);
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x00260198 File Offset: 0x0025F598
		private void CloneHelper(OleDbParameter destination)
		{
			this.CloneHelperCore(destination);
			destination._metaType = this._metaType;
			destination._parameterName = this._parameterName;
			destination._precision = this._precision;
			destination._scale = this._scale;
			destination._hasScale = this._hasScale;
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x002601E8 File Offset: 0x0025F5E8
		private void PropertyChanging()
		{
			this._changeID++;
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x00260204 File Offset: 0x0025F604
		private void PropertyTypeChanging()
		{
			this.PropertyChanging();
			this._coerceMetaType = null;
			this.CoercedValue = null;
		}

		// Token: 0x06001FE7 RID: 8167 RVA: 0x00260228 File Offset: 0x0025F628
		internal bool BindParameter(int index, Bindings bindings)
		{
			object obj = this.Value;
			NativeDBType bindType = this.GetBindType(obj);
			if (bindType.enumOleDbType == OleDbType.Empty)
			{
				throw ODB.UninitializedParameters(index, bindType.enumOleDbType);
			}
			this._coerceMetaType = bindType;
			obj = OleDbParameter.CoerceValue(obj, bindType);
			this.CoercedValue = obj;
			ParameterDirection direction = this.Direction;
			byte b;
			if (this.ShouldSerializePrecision())
			{
				b = this.PrecisionInternal;
			}
			else
			{
				b = this.ValuePrecision(obj);
			}
			if (b == 0)
			{
				b = bindType.maxpre;
			}
			byte b2;
			if (this.ShouldSerializeScale())
			{
				b2 = this.ScaleInternal;
			}
			else
			{
				b2 = this.ValueScale(obj);
			}
			int num = (int)bindType.wType;
			int num2;
			int num3;
			if (bindType.islong)
			{
				num2 = ADP.PtrSize;
				if (this.ShouldSerializeSize())
				{
					num3 = this.Size;
				}
				else if (129 == bindType.dbType)
				{
					num3 = int.MaxValue;
				}
				else if (130 == bindType.dbType)
				{
					num3 = 1073741823;
				}
				else
				{
					num3 = int.MaxValue;
				}
				num |= 16384;
			}
			else if (bindType.IsVariableLength)
			{
				if (!this.ShouldSerializeSize() && ADP.IsDirection(this, ParameterDirection.Output))
				{
					throw ADP.UninitializedParameterSize(index, this._coerceMetaType.dataType);
				}
				bool flag;
				if (this.ShouldSerializeSize())
				{
					num3 = this.Size;
					flag = false;
				}
				else
				{
					num3 = this.ValueSize(obj);
					flag = true;
				}
				if (0 < num3)
				{
					if (130 == bindType.wType)
					{
						num2 = Math.Min(num3, 1073741822) * 2 + 2;
					}
					else
					{
						num2 = num3;
					}
					if (flag && 129 == bindType.dbType)
					{
						num3 = Math.Min(num3, 1073741822) * 2;
					}
					if (8192 < num2)
					{
						num2 = ADP.PtrSize;
						num |= 16384;
					}
				}
				else if (num3 == 0)
				{
					if (130 == num)
					{
						num2 = 2;
					}
					else
					{
						num2 = 0;
					}
				}
				else
				{
					if (-1 != num3)
					{
						throw ADP.InvalidSizeValue(num3);
					}
					num2 = ADP.PtrSize;
					num |= 16384;
				}
			}
			else
			{
				num2 = bindType.fixlen;
				num3 = num2;
			}
			bindings.CurrentIndex = index;
			bindings.DataSourceType = bindType.dbString.DangerousGetHandle();
			bindings.Name = ADP.PtrZero;
			bindings.ParamSize = new IntPtr(num3);
			bindings.Flags = OleDbParameter.GetBindFlags(direction);
			bindings.Ordinal = (IntPtr)(index + 1);
			bindings.Part = bindType.dbPart;
			bindings.ParamIO = OleDbParameter.GetBindDirection(direction);
			bindings.Precision = b;
			bindings.Scale = b2;
			bindings.DbType = num;
			bindings.MaxLen = num2;
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oledb.struct.tagDBPARAMBINDINFO|INFO|ADV> index=%d, parameterName='%ls'\n", index, this.ParameterName);
				Bid.Trace("<oledb.struct.tagDBBINDING|INFO|ADV>\n");
			}
			return this.IsParameterComputed();
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x002604B0 File Offset: 0x0025F8B0
		private static object CoerceValue(object value, NativeDBType destinationType)
		{
			if (value != null && DBNull.Value != value && typeof(object) != destinationType.dataType)
			{
				Type type = value.GetType();
				if (type != destinationType.dataType)
				{
					try
					{
						if (typeof(string) != destinationType.dataType || typeof(char[]) != type)
						{
							if (6 == destinationType.dbType && typeof(string) == type)
							{
								value = decimal.Parse((string)value, NumberStyles.Currency, null);
							}
							else
							{
								value = Convert.ChangeType(value, destinationType.dataType, null);
							}
						}
					}
					catch (Exception ex)
					{
						if (!ADP.IsCatchableExceptionType(ex))
						{
							throw;
						}
						throw ADP.ParameterConversionFailed(value, destinationType.dataType, ex);
					}
				}
			}
			return value;
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x00260590 File Offset: 0x0025F990
		private NativeDBType GetBindType(object value)
		{
			NativeDBType nativeDBType = this._metaType;
			if (nativeDBType == null)
			{
				if (ADP.IsNull(value))
				{
					nativeDBType = NativeDBType.Default;
				}
				else
				{
					nativeDBType = NativeDBType.FromSystemType(value);
				}
			}
			return nativeDBType;
		}

		// Token: 0x06001FEA RID: 8170 RVA: 0x002605C0 File Offset: 0x0025F9C0
		internal object GetCoercedValue()
		{
			object obj = this.CoercedValue;
			if (obj == null)
			{
				obj = OleDbParameter.CoerceValue(this.Value, this._coerceMetaType);
				this.CoercedValue = obj;
			}
			return obj;
		}

		// Token: 0x06001FEB RID: 8171 RVA: 0x002605F4 File Offset: 0x0025F9F4
		internal bool IsParameterComputed()
		{
			NativeDBType metaType = this._metaType;
			return metaType == null || (!this.ShouldSerializeSize() && metaType.IsVariableLength) || 14 == metaType.dbType || (131 == metaType.dbType && (!this.ShouldSerializeScale() || !this.ShouldSerializePrecision()));
		}

		// Token: 0x06001FEC RID: 8172 RVA: 0x0026064C File Offset: 0x0025FA4C
		internal void Prepare(OleDbCommand cmd)
		{
			if (this._metaType == null)
			{
				throw ADP.PrepareParameterType(cmd);
			}
			if (!this.ShouldSerializeSize() && this._metaType.IsVariableLength)
			{
				throw ADP.PrepareParameterSize(cmd);
			}
			if (!this.ShouldSerializePrecision() && !this.ShouldSerializeScale() && (14 == this._metaType.wType || 131 == this._metaType.wType))
			{
				throw ADP.PrepareParameterScale(cmd, this._metaType.wType.ToString("G", CultureInfo.InvariantCulture));
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06001FED RID: 8173 RVA: 0x002606DC File Offset: 0x0025FADC
		// (set) Token: 0x06001FEE RID: 8174 RVA: 0x002606F0 File Offset: 0x0025FAF0
		[TypeConverter(typeof(StringConverter))]
		[RefreshProperties(RefreshProperties.All)]
		[ResDescription("DbParameter_Value")]
		[ResCategory("DataCategory_Data")]
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

		// Token: 0x06001FEF RID: 8175 RVA: 0x0026070C File Offset: 0x0025FB0C
		private byte ValuePrecision(object value)
		{
			return this.ValuePrecisionCore(value);
		}

		// Token: 0x06001FF0 RID: 8176 RVA: 0x00260720 File Offset: 0x0025FB20
		private byte ValueScale(object value)
		{
			return this.ValueScaleCore(value);
		}

		// Token: 0x06001FF1 RID: 8177 RVA: 0x00260734 File Offset: 0x0025FB34
		private int ValueSize(object value)
		{
			return this.ValueSizeCore(value);
		}

		// Token: 0x06001FF2 RID: 8178 RVA: 0x00260748 File Offset: 0x0025FB48
		private static int GetBindDirection(ParameterDirection direction)
		{
			return (int)(ParameterDirection.InputOutput & direction);
		}

		// Token: 0x06001FF3 RID: 8179 RVA: 0x00260758 File Offset: 0x0025FB58
		private static int GetBindFlags(ParameterDirection direction)
		{
			return (int)(ParameterDirection.InputOutput & direction);
		}

		// Token: 0x06001FF4 RID: 8180 RVA: 0x00260768 File Offset: 0x0025FB68
		private OleDbParameter(OleDbParameter source)
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

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06001FF5 RID: 8181 RVA: 0x002607A8 File Offset: 0x0025FBA8
		// (set) Token: 0x06001FF6 RID: 8182 RVA: 0x002607BC File Offset: 0x0025FBBC
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

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06001FF7 RID: 8183 RVA: 0x002607D0 File Offset: 0x0025FBD0
		// (set) Token: 0x06001FF8 RID: 8184 RVA: 0x002607EC File Offset: 0x0025FBEC
		[ResCategory("DataCategory_Data")]
		[ResDescription("DbParameter_Direction")]
		[RefreshProperties(RefreshProperties.All)]
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

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06001FF9 RID: 8185 RVA: 0x0026083C File Offset: 0x0025FC3C
		// (set) Token: 0x06001FFA RID: 8186 RVA: 0x00260850 File Offset: 0x0025FC50
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

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06001FFB RID: 8187 RVA: 0x00260864 File Offset: 0x0025FC64
		internal int Offset
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06001FFC RID: 8188 RVA: 0x00260874 File Offset: 0x0025FC74
		// (set) Token: 0x06001FFD RID: 8189 RVA: 0x0026089C File Offset: 0x0025FC9C
		[ResDescription("DbParameter_Size")]
		[ResCategory("DataCategory_Data")]
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

		// Token: 0x06001FFE RID: 8190 RVA: 0x002608CC File Offset: 0x0025FCCC
		private void ResetSize()
		{
			if (this._size != 0)
			{
				this.PropertyChanging();
				this._size = 0;
			}
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x002608F0 File Offset: 0x0025FCF0
		private bool ShouldSerializeSize()
		{
			return 0 != this._size;
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06002000 RID: 8192 RVA: 0x0026090C File Offset: 0x0025FD0C
		// (set) Token: 0x06002001 RID: 8193 RVA: 0x0026092C File Offset: 0x0025FD2C
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

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06002002 RID: 8194 RVA: 0x00260940 File Offset: 0x0025FD40
		// (set) Token: 0x06002003 RID: 8195 RVA: 0x00260954 File Offset: 0x0025FD54
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

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06002004 RID: 8196 RVA: 0x00260968 File Offset: 0x0025FD68
		// (set) Token: 0x06002005 RID: 8197 RVA: 0x00260988 File Offset: 0x0025FD88
		[ResDescription("DbParameter_SourceVersion")]
		[ResCategory("DataCategory_Update")]
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

		// Token: 0x06002006 RID: 8198 RVA: 0x002609D0 File Offset: 0x0025FDD0
		private void CloneHelperCore(OleDbParameter destination)
		{
			destination._value = this._value;
			destination._direction = this._direction;
			destination._size = this._size;
			destination._sourceColumn = this._sourceColumn;
			destination._sourceVersion = this._sourceVersion;
			destination._sourceColumnNullMapping = this._sourceColumnNullMapping;
			destination._isNullable = this._isNullable;
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x00260A34 File Offset: 0x0025FE34
		internal void CopyTo(DbParameter destination)
		{
			ADP.CheckArgumentNull(destination, "destination");
			this.CloneHelper((OleDbParameter)destination);
		}

		// Token: 0x06002008 RID: 8200 RVA: 0x00260A58 File Offset: 0x0025FE58
		internal object CompareExchangeParent(object value, object comparand)
		{
			object parent = this._parent;
			if (comparand == parent)
			{
				this._parent = value;
			}
			return parent;
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x00260A78 File Offset: 0x0025FE78
		internal void ResetParent()
		{
			this._parent = null;
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x00260A8C File Offset: 0x0025FE8C
		public override string ToString()
		{
			return this.ParameterName;
		}

		// Token: 0x0600200B RID: 8203 RVA: 0x00260AA0 File Offset: 0x0025FEA0
		private byte ValuePrecisionCore(object value)
		{
			if (value is decimal)
			{
				return ((decimal)value).Precision;
			}
			return 0;
		}

		// Token: 0x0600200C RID: 8204 RVA: 0x00260ACC File Offset: 0x0025FECC
		private byte ValueScaleCore(object value)
		{
			if (value is decimal)
			{
				return (byte)((decimal.GetBits((decimal)value)[3] & 16711680) >> 16);
			}
			return 0;
		}

		// Token: 0x0600200D RID: 8205 RVA: 0x00260AFC File Offset: 0x0025FEFC
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

		// Token: 0x04001435 RID: 5173
		private NativeDBType _metaType;

		// Token: 0x04001436 RID: 5174
		private int _changeID;

		// Token: 0x04001437 RID: 5175
		private string _parameterName;

		// Token: 0x04001438 RID: 5176
		private byte _precision;

		// Token: 0x04001439 RID: 5177
		private byte _scale;

		// Token: 0x0400143A RID: 5178
		private bool _hasScale;

		// Token: 0x0400143B RID: 5179
		private NativeDBType _coerceMetaType;

		// Token: 0x0400143C RID: 5180
		private object _value;

		// Token: 0x0400143D RID: 5181
		private object _parent;

		// Token: 0x0400143E RID: 5182
		private ParameterDirection _direction;

		// Token: 0x0400143F RID: 5183
		private int _size;

		// Token: 0x04001440 RID: 5184
		private string _sourceColumn;

		// Token: 0x04001441 RID: 5185
		private DataRowVersion _sourceVersion;

		// Token: 0x04001442 RID: 5186
		private bool _sourceColumnNullMapping;

		// Token: 0x04001443 RID: 5187
		private bool _isNullable;

		// Token: 0x04001444 RID: 5188
		private object _coercedValue;

		// Token: 0x02000233 RID: 563
		internal sealed class OleDbParameterConverter : ExpandableObjectConverter
		{
			// Token: 0x0600200F RID: 8207 RVA: 0x00260B68 File Offset: 0x0025FF68
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return typeof(InstanceDescriptor) == destinationType || base.CanConvertTo(context, destinationType);
			}

			// Token: 0x06002010 RID: 8208 RVA: 0x00260B8C File Offset: 0x0025FF8C
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == null)
				{
					throw ADP.ArgumentNull("destinationType");
				}
				if (typeof(InstanceDescriptor) == destinationType && value is OleDbParameter)
				{
					return this.ConvertToInstanceDescriptor(value as OleDbParameter);
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}

			// Token: 0x06002011 RID: 8209 RVA: 0x00260BD8 File Offset: 0x0025FFD8
			private InstanceDescriptor ConvertToInstanceDescriptor(OleDbParameter p)
			{
				int num = 0;
				if (p.ShouldSerializeOleDbType())
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
				if (p.SourceColumnNullMapping)
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
						typeof(OleDbType)
					};
					array2 = new object[] { p.ParameterName, p.OleDbType };
					break;
				case 2:
				case 3:
					array = new Type[]
					{
						typeof(string),
						typeof(OleDbType),
						typeof(int)
					};
					array2 = new object[] { p.ParameterName, p.OleDbType, p.Size };
					break;
				case 4:
				case 5:
				case 6:
				case 7:
					array = new Type[]
					{
						typeof(string),
						typeof(OleDbType),
						typeof(int),
						typeof(string)
					};
					array2 = new object[] { p.ParameterName, p.OleDbType, p.Size, p.SourceColumn };
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
							typeof(OleDbType),
							typeof(int),
							typeof(ParameterDirection),
							typeof(bool),
							typeof(byte),
							typeof(byte),
							typeof(string),
							typeof(DataRowVersion),
							typeof(object)
						};
						array2 = new object[] { p.ParameterName, p.OleDbType, p.Size, p.Direction, p.IsNullable, p.PrecisionInternal, p.ScaleInternal, p.SourceColumn, p.SourceVersion, p.Value };
					}
					else
					{
						array = new Type[]
						{
							typeof(string),
							typeof(OleDbType),
							typeof(int),
							typeof(ParameterDirection),
							typeof(byte),
							typeof(byte),
							typeof(string),
							typeof(DataRowVersion),
							typeof(bool),
							typeof(object)
						};
						array2 = new object[] { p.ParameterName, p.OleDbType, p.Size, p.Direction, p.PrecisionInternal, p.ScaleInternal, p.SourceColumn, p.SourceVersion, p.SourceColumnNullMapping, p.Value };
					}
					break;
				}
				ConstructorInfo constructor = typeof(OleDbParameter).GetConstructor(array);
				return new InstanceDescriptor(constructor, array2);
			}
		}
	}
}
