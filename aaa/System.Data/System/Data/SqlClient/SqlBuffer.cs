using System;
using System.Data.SqlTypes;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Data.SqlClient
{
	// Token: 0x020002A8 RID: 680
	internal sealed class SqlBuffer
	{
		// Token: 0x060022C2 RID: 8898 RVA: 0x0026E0FC File Offset: 0x0026D4FC
		internal SqlBuffer()
		{
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x0026E110 File Offset: 0x0026D510
		private SqlBuffer(SqlBuffer value)
		{
			this._isNull = value._isNull;
			this._type = value._type;
			this._value = value._value;
			this._object = value._object;
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x060022C4 RID: 8900 RVA: 0x0026E154 File Offset: 0x0026D554
		internal bool IsEmpty
		{
			get
			{
				return SqlBuffer.StorageType.Empty == this._type;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x060022C5 RID: 8901 RVA: 0x0026E16C File Offset: 0x0026D56C
		internal bool IsNull
		{
			get
			{
				return this._isNull;
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x060022C6 RID: 8902 RVA: 0x0026E180 File Offset: 0x0026D580
		// (set) Token: 0x060022C7 RID: 8903 RVA: 0x0026E1B4 File Offset: 0x0026D5B4
		internal bool Boolean
		{
			get
			{
				this.ThrowIfNull();
				if (SqlBuffer.StorageType.Boolean == this._type)
				{
					return this._value._boolean;
				}
				return (bool)this.Value;
			}
			set
			{
				this._value._boolean = value;
				this._type = SqlBuffer.StorageType.Boolean;
				this._isNull = false;
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x060022C8 RID: 8904 RVA: 0x0026E1DC File Offset: 0x0026D5DC
		// (set) Token: 0x060022C9 RID: 8905 RVA: 0x0026E210 File Offset: 0x0026D610
		internal byte Byte
		{
			get
			{
				this.ThrowIfNull();
				if (SqlBuffer.StorageType.Byte == this._type)
				{
					return this._value._byte;
				}
				return (byte)this.Value;
			}
			set
			{
				this._value._byte = value;
				this._type = SqlBuffer.StorageType.Byte;
				this._isNull = false;
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x060022CA RID: 8906 RVA: 0x0026E238 File Offset: 0x0026D638
		internal byte[] ByteArray
		{
			get
			{
				this.ThrowIfNull();
				return this.SqlBinary.Value;
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x060022CB RID: 8907 RVA: 0x0026E25C File Offset: 0x0026D65C
		internal DateTime DateTime
		{
			get
			{
				this.ThrowIfNull();
				if (SqlBuffer.StorageType.Date == this._type)
				{
					return DateTime.MinValue.AddDays((double)this._value._int32);
				}
				if (SqlBuffer.StorageType.DateTime2 == this._type)
				{
					return new DateTime(SqlBuffer.GetTicksFromDateTime2Info(this._value._dateTime2Info));
				}
				if (SqlBuffer.StorageType.DateTime == this._type)
				{
					return SqlDateTime.ToDateTime(this._value._dateTimeInfo.daypart, this._value._dateTimeInfo.timepart);
				}
				return (DateTime)this.Value;
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x060022CC RID: 8908 RVA: 0x0026E2F0 File Offset: 0x0026D6F0
		internal decimal Decimal
		{
			get
			{
				this.ThrowIfNull();
				if (SqlBuffer.StorageType.Decimal == this._type)
				{
					if (this._value._numericInfo.data4 != 0 || this._value._numericInfo.scale > 28)
					{
						throw new OverflowException(SQLResource.ConversionOverflowMessage);
					}
					return new decimal(this._value._numericInfo.data1, this._value._numericInfo.data2, this._value._numericInfo.data3, !this._value._numericInfo.positive, this._value._numericInfo.scale);
				}
				else
				{
					if (SqlBuffer.StorageType.Money == this._type)
					{
						long num = this._value._int64;
						bool flag = false;
						if (num < 0L)
						{
							flag = true;
							num = -num;
						}
						return new decimal((int)(num & (long)((ulong)(-1))), (int)(num >> 32), 0, flag, 4);
					}
					return (decimal)this.Value;
				}
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x060022CD RID: 8909 RVA: 0x0026E3DC File Offset: 0x0026D7DC
		// (set) Token: 0x060022CE RID: 8910 RVA: 0x0026E410 File Offset: 0x0026D810
		internal double Double
		{
			get
			{
				this.ThrowIfNull();
				if (SqlBuffer.StorageType.Double == this._type)
				{
					return this._value._double;
				}
				return (double)this.Value;
			}
			set
			{
				this._value._double = value;
				this._type = SqlBuffer.StorageType.Double;
				this._isNull = false;
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x060022CF RID: 8911 RVA: 0x0026E438 File Offset: 0x0026D838
		internal Guid Guid
		{
			get
			{
				this.ThrowIfNull();
				return this.SqlGuid.Value;
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x060022D0 RID: 8912 RVA: 0x0026E45C File Offset: 0x0026D85C
		// (set) Token: 0x060022D1 RID: 8913 RVA: 0x0026E490 File Offset: 0x0026D890
		internal short Int16
		{
			get
			{
				this.ThrowIfNull();
				if (SqlBuffer.StorageType.Int16 == this._type)
				{
					return this._value._int16;
				}
				return (short)this.Value;
			}
			set
			{
				this._value._int16 = value;
				this._type = SqlBuffer.StorageType.Int16;
				this._isNull = false;
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x060022D2 RID: 8914 RVA: 0x0026E4B8 File Offset: 0x0026D8B8
		// (set) Token: 0x060022D3 RID: 8915 RVA: 0x0026E4EC File Offset: 0x0026D8EC
		internal int Int32
		{
			get
			{
				this.ThrowIfNull();
				if (SqlBuffer.StorageType.Int32 == this._type)
				{
					return this._value._int32;
				}
				return (int)this.Value;
			}
			set
			{
				this._value._int32 = value;
				this._type = SqlBuffer.StorageType.Int32;
				this._isNull = false;
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x060022D4 RID: 8916 RVA: 0x0026E514 File Offset: 0x0026D914
		// (set) Token: 0x060022D5 RID: 8917 RVA: 0x0026E548 File Offset: 0x0026D948
		internal long Int64
		{
			get
			{
				this.ThrowIfNull();
				if (SqlBuffer.StorageType.Int64 == this._type)
				{
					return this._value._int64;
				}
				return (long)this.Value;
			}
			set
			{
				this._value._int64 = value;
				this._type = SqlBuffer.StorageType.Int64;
				this._isNull = false;
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x060022D6 RID: 8918 RVA: 0x0026E570 File Offset: 0x0026D970
		// (set) Token: 0x060022D7 RID: 8919 RVA: 0x0026E5A4 File Offset: 0x0026D9A4
		internal float Single
		{
			get
			{
				this.ThrowIfNull();
				if (SqlBuffer.StorageType.Single == this._type)
				{
					return this._value._single;
				}
				return (float)this.Value;
			}
			set
			{
				this._value._single = value;
				this._type = SqlBuffer.StorageType.Single;
				this._isNull = false;
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x060022D8 RID: 8920 RVA: 0x0026E5CC File Offset: 0x0026D9CC
		internal string String
		{
			get
			{
				this.ThrowIfNull();
				if (SqlBuffer.StorageType.String == this._type)
				{
					return (string)this._object;
				}
				if (SqlBuffer.StorageType.SqlCachedBuffer == this._type)
				{
					return ((SqlCachedBuffer)this._object).ToString();
				}
				return (string)this.Value;
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x060022D9 RID: 8921 RVA: 0x0026E61C File Offset: 0x0026DA1C
		internal string KatmaiDateTimeString
		{
			get
			{
				this.ThrowIfNull();
				if (SqlBuffer.StorageType.Date == this._type)
				{
					return this.DateTime.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
				}
				if (SqlBuffer.StorageType.Time == this._type)
				{
					byte scale = this._value._timeInfo.scale;
					return new DateTime(this._value._timeInfo.ticks).ToString(SqlBuffer.__katmaiTimeFormatByScale[(int)scale], DateTimeFormatInfo.InvariantInfo);
				}
				if (SqlBuffer.StorageType.DateTime2 == this._type)
				{
					byte scale2 = this._value._dateTime2Info.timeInfo.scale;
					return this.DateTime.ToString(SqlBuffer.__katmaiDateTime2FormatByScale[(int)scale2], DateTimeFormatInfo.InvariantInfo);
				}
				if (SqlBuffer.StorageType.DateTimeOffset == this._type)
				{
					DateTimeOffset dateTimeOffset = this.DateTimeOffset;
					byte scale3 = this._value._dateTimeOffsetInfo.dateTime2Info.timeInfo.scale;
					return dateTimeOffset.ToString(SqlBuffer.__katmaiDateTimeOffsetFormatByScale[(int)scale3], DateTimeFormatInfo.InvariantInfo);
				}
				return (string)this.Value;
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x060022DA RID: 8922 RVA: 0x0026E720 File Offset: 0x0026DB20
		internal SqlString KatmaiDateTimeSqlString
		{
			get
			{
				if (SqlBuffer.StorageType.Date != this._type && SqlBuffer.StorageType.Time != this._type && SqlBuffer.StorageType.DateTime2 != this._type && SqlBuffer.StorageType.DateTimeOffset != this._type)
				{
					return (SqlString)this.SqlValue;
				}
				if (this.IsNull)
				{
					return SqlString.Null;
				}
				return new SqlString(this.KatmaiDateTimeString);
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x060022DB RID: 8923 RVA: 0x0026E77C File Offset: 0x0026DB7C
		internal TimeSpan Time
		{
			get
			{
				this.ThrowIfNull();
				if (SqlBuffer.StorageType.Time == this._type)
				{
					return new TimeSpan(this._value._timeInfo.ticks);
				}
				return (TimeSpan)this.Value;
			}
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x060022DC RID: 8924 RVA: 0x0026E7BC File Offset: 0x0026DBBC
		internal DateTimeOffset DateTimeOffset
		{
			get
			{
				this.ThrowIfNull();
				if (SqlBuffer.StorageType.DateTimeOffset == this._type)
				{
					TimeSpan timeSpan = new TimeSpan(0, (int)this._value._dateTimeOffsetInfo.offset, 0);
					return new DateTimeOffset(SqlBuffer.GetTicksFromDateTime2Info(this._value._dateTimeOffsetInfo.dateTime2Info) + timeSpan.Ticks, timeSpan);
				}
				return (DateTimeOffset)this.Value;
			}
		}

		// Token: 0x060022DD RID: 8925 RVA: 0x0026E824 File Offset: 0x0026DC24
		private static long GetTicksFromDateTime2Info(SqlBuffer.DateTime2Info dateTime2Info)
		{
			return (long)dateTime2Info.date * 864000000000L + dateTime2Info.timeInfo.ticks;
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x060022DE RID: 8926 RVA: 0x0026E850 File Offset: 0x0026DC50
		// (set) Token: 0x060022DF RID: 8927 RVA: 0x0026E880 File Offset: 0x0026DC80
		internal SqlBinary SqlBinary
		{
			get
			{
				if (SqlBuffer.StorageType.SqlBinary == this._type)
				{
					return (SqlBinary)this._object;
				}
				return (SqlBinary)this.SqlValue;
			}
			set
			{
				this._object = value;
				this._type = SqlBuffer.StorageType.SqlBinary;
				this._isNull = value.IsNull;
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x060022E0 RID: 8928 RVA: 0x0026E8B0 File Offset: 0x0026DCB0
		internal SqlBoolean SqlBoolean
		{
			get
			{
				if (SqlBuffer.StorageType.Boolean != this._type)
				{
					return (SqlBoolean)this.SqlValue;
				}
				if (this.IsNull)
				{
					return SqlBoolean.Null;
				}
				return new SqlBoolean(this._value._boolean);
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x060022E1 RID: 8929 RVA: 0x0026E8F0 File Offset: 0x0026DCF0
		internal SqlByte SqlByte
		{
			get
			{
				if (SqlBuffer.StorageType.Byte != this._type)
				{
					return (SqlByte)this.SqlValue;
				}
				if (this.IsNull)
				{
					return SqlByte.Null;
				}
				return new SqlByte(this._value._byte);
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x060022E2 RID: 8930 RVA: 0x0026E930 File Offset: 0x0026DD30
		// (set) Token: 0x060022E3 RID: 8931 RVA: 0x0026E96C File Offset: 0x0026DD6C
		internal SqlCachedBuffer SqlCachedBuffer
		{
			get
			{
				if (SqlBuffer.StorageType.SqlCachedBuffer != this._type)
				{
					return (SqlCachedBuffer)this.SqlValue;
				}
				if (this.IsNull)
				{
					return SqlCachedBuffer.Null;
				}
				return (SqlCachedBuffer)this._object;
			}
			set
			{
				this._object = value;
				this._type = SqlBuffer.StorageType.SqlCachedBuffer;
				this._isNull = value.IsNull;
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x060022E4 RID: 8932 RVA: 0x0026E994 File Offset: 0x0026DD94
		// (set) Token: 0x060022E5 RID: 8933 RVA: 0x0026E9D0 File Offset: 0x0026DDD0
		internal SqlXml SqlXml
		{
			get
			{
				if (SqlBuffer.StorageType.SqlXml != this._type)
				{
					return (SqlXml)this.SqlValue;
				}
				if (this.IsNull)
				{
					return SqlXml.Null;
				}
				return (SqlXml)this._object;
			}
			set
			{
				this._object = value;
				this._type = SqlBuffer.StorageType.SqlXml;
				this._isNull = value.IsNull;
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x060022E6 RID: 8934 RVA: 0x0026E9F8 File Offset: 0x0026DDF8
		internal SqlDateTime SqlDateTime
		{
			get
			{
				if (SqlBuffer.StorageType.DateTime != this._type)
				{
					return (SqlDateTime)this.SqlValue;
				}
				if (this.IsNull)
				{
					return SqlDateTime.Null;
				}
				return new SqlDateTime(this._value._dateTimeInfo.daypart, this._value._dateTimeInfo.timepart);
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x060022E7 RID: 8935 RVA: 0x0026EA50 File Offset: 0x0026DE50
		internal SqlDecimal SqlDecimal
		{
			get
			{
				if (SqlBuffer.StorageType.Decimal != this._type)
				{
					return (SqlDecimal)this.SqlValue;
				}
				if (this.IsNull)
				{
					return SqlDecimal.Null;
				}
				return new SqlDecimal(this._value._numericInfo.precision, this._value._numericInfo.scale, this._value._numericInfo.positive, this._value._numericInfo.data1, this._value._numericInfo.data2, this._value._numericInfo.data3, this._value._numericInfo.data4);
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x060022E8 RID: 8936 RVA: 0x0026EAF8 File Offset: 0x0026DEF8
		internal SqlDouble SqlDouble
		{
			get
			{
				if (SqlBuffer.StorageType.Double != this._type)
				{
					return (SqlDouble)this.SqlValue;
				}
				if (this.IsNull)
				{
					return SqlDouble.Null;
				}
				return new SqlDouble(this._value._double);
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x060022E9 RID: 8937 RVA: 0x0026EB38 File Offset: 0x0026DF38
		// (set) Token: 0x060022EA RID: 8938 RVA: 0x0026EB68 File Offset: 0x0026DF68
		internal SqlGuid SqlGuid
		{
			get
			{
				if (SqlBuffer.StorageType.SqlGuid == this._type)
				{
					return (SqlGuid)this._object;
				}
				return (SqlGuid)this.SqlValue;
			}
			set
			{
				this._object = value;
				this._type = SqlBuffer.StorageType.SqlGuid;
				this._isNull = value.IsNull;
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x060022EB RID: 8939 RVA: 0x0026EB98 File Offset: 0x0026DF98
		internal SqlInt16 SqlInt16
		{
			get
			{
				if (SqlBuffer.StorageType.Int16 != this._type)
				{
					return (SqlInt16)this.SqlValue;
				}
				if (this.IsNull)
				{
					return SqlInt16.Null;
				}
				return new SqlInt16(this._value._int16);
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x060022EC RID: 8940 RVA: 0x0026EBD8 File Offset: 0x0026DFD8
		internal SqlInt32 SqlInt32
		{
			get
			{
				if (SqlBuffer.StorageType.Int32 != this._type)
				{
					return (SqlInt32)this.SqlValue;
				}
				if (this.IsNull)
				{
					return SqlInt32.Null;
				}
				return new SqlInt32(this._value._int32);
			}
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x060022ED RID: 8941 RVA: 0x0026EC18 File Offset: 0x0026E018
		internal SqlInt64 SqlInt64
		{
			get
			{
				if (SqlBuffer.StorageType.Int64 != this._type)
				{
					return (SqlInt64)this.SqlValue;
				}
				if (this.IsNull)
				{
					return SqlInt64.Null;
				}
				return new SqlInt64(this._value._int64);
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x060022EE RID: 8942 RVA: 0x0026EC58 File Offset: 0x0026E058
		internal SqlMoney SqlMoney
		{
			get
			{
				if (SqlBuffer.StorageType.Money != this._type)
				{
					return (SqlMoney)this.SqlValue;
				}
				if (this.IsNull)
				{
					return SqlMoney.Null;
				}
				return new SqlMoney(this._value._int64, 1);
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x060022EF RID: 8943 RVA: 0x0026EC9C File Offset: 0x0026E09C
		internal SqlSingle SqlSingle
		{
			get
			{
				if (SqlBuffer.StorageType.Single != this._type)
				{
					return (SqlSingle)this.SqlValue;
				}
				if (this.IsNull)
				{
					return SqlSingle.Null;
				}
				return new SqlSingle(this._value._single);
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x060022F0 RID: 8944 RVA: 0x0026ECE0 File Offset: 0x0026E0E0
		internal SqlString SqlString
		{
			get
			{
				if (SqlBuffer.StorageType.String == this._type)
				{
					if (this.IsNull)
					{
						return SqlString.Null;
					}
					return new SqlString((string)this._object);
				}
				else
				{
					if (SqlBuffer.StorageType.SqlCachedBuffer != this._type)
					{
						return (SqlString)this.SqlValue;
					}
					SqlCachedBuffer sqlCachedBuffer = (SqlCachedBuffer)this._object;
					if (sqlCachedBuffer.IsNull)
					{
						return SqlString.Null;
					}
					return sqlCachedBuffer.ToSqlString();
				}
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x060022F1 RID: 8945 RVA: 0x0026ED4C File Offset: 0x0026E14C
		internal object SqlValue
		{
			get
			{
				switch (this._type)
				{
				case SqlBuffer.StorageType.Empty:
					return DBNull.Value;
				case SqlBuffer.StorageType.Boolean:
					return this.SqlBoolean;
				case SqlBuffer.StorageType.Byte:
					return this.SqlByte;
				case SqlBuffer.StorageType.DateTime:
					return this.SqlDateTime;
				case SqlBuffer.StorageType.Decimal:
					return this.SqlDecimal;
				case SqlBuffer.StorageType.Double:
					return this.SqlDouble;
				case SqlBuffer.StorageType.Int16:
					return this.SqlInt16;
				case SqlBuffer.StorageType.Int32:
					return this.SqlInt32;
				case SqlBuffer.StorageType.Int64:
					return this.SqlInt64;
				case SqlBuffer.StorageType.Money:
					return this.SqlMoney;
				case SqlBuffer.StorageType.Single:
					return this.SqlSingle;
				case SqlBuffer.StorageType.String:
					return this.SqlString;
				case SqlBuffer.StorageType.SqlBinary:
				case SqlBuffer.StorageType.SqlGuid:
					return this._object;
				case SqlBuffer.StorageType.SqlCachedBuffer:
				{
					SqlCachedBuffer sqlCachedBuffer = (SqlCachedBuffer)this._object;
					if (sqlCachedBuffer.IsNull)
					{
						return SqlXml.Null;
					}
					return sqlCachedBuffer.ToSqlXml();
				}
				case SqlBuffer.StorageType.SqlXml:
					if (this._isNull)
					{
						return SqlXml.Null;
					}
					return (SqlXml)this._object;
				case SqlBuffer.StorageType.Date:
				case SqlBuffer.StorageType.DateTime2:
					if (this._isNull)
					{
						return DBNull.Value;
					}
					return this.DateTime;
				case SqlBuffer.StorageType.DateTimeOffset:
					if (this._isNull)
					{
						return DBNull.Value;
					}
					return this.DateTimeOffset;
				case SqlBuffer.StorageType.Time:
					if (this._isNull)
					{
						return DBNull.Value;
					}
					return this.Time;
				default:
					return null;
				}
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x060022F2 RID: 8946 RVA: 0x0026EED8 File Offset: 0x0026E2D8
		internal object Value
		{
			get
			{
				if (this.IsNull)
				{
					return DBNull.Value;
				}
				switch (this._type)
				{
				case SqlBuffer.StorageType.Empty:
					return DBNull.Value;
				case SqlBuffer.StorageType.Boolean:
					return this.Boolean;
				case SqlBuffer.StorageType.Byte:
					return this.Byte;
				case SqlBuffer.StorageType.DateTime:
					return this.DateTime;
				case SqlBuffer.StorageType.Decimal:
					return this.Decimal;
				case SqlBuffer.StorageType.Double:
					return this.Double;
				case SqlBuffer.StorageType.Int16:
					return this.Int16;
				case SqlBuffer.StorageType.Int32:
					return this.Int32;
				case SqlBuffer.StorageType.Int64:
					return this.Int64;
				case SqlBuffer.StorageType.Money:
					return this.Decimal;
				case SqlBuffer.StorageType.Single:
					return this.Single;
				case SqlBuffer.StorageType.String:
					return this.String;
				case SqlBuffer.StorageType.SqlBinary:
					return this.ByteArray;
				case SqlBuffer.StorageType.SqlCachedBuffer:
					return ((SqlCachedBuffer)this._object).ToString();
				case SqlBuffer.StorageType.SqlGuid:
					return this.Guid;
				case SqlBuffer.StorageType.SqlXml:
				{
					SqlXml sqlXml = (SqlXml)this._object;
					return sqlXml.Value;
				}
				case SqlBuffer.StorageType.Date:
					return this.DateTime;
				case SqlBuffer.StorageType.DateTime2:
					return this.DateTime;
				case SqlBuffer.StorageType.DateTimeOffset:
					return this.DateTimeOffset;
				case SqlBuffer.StorageType.Time:
					return this.Time;
				default:
					return null;
				}
			}
		}

		// Token: 0x060022F3 RID: 8947 RVA: 0x0026F044 File Offset: 0x0026E444
		internal Type GetTypeFromStorageType(bool isSqlType)
		{
			if (isSqlType)
			{
				switch (this._type)
				{
				case SqlBuffer.StorageType.Empty:
					return null;
				case SqlBuffer.StorageType.Boolean:
					return typeof(SqlBoolean);
				case SqlBuffer.StorageType.Byte:
					return typeof(SqlByte);
				case SqlBuffer.StorageType.DateTime:
					return typeof(SqlDateTime);
				case SqlBuffer.StorageType.Decimal:
					return typeof(SqlDecimal);
				case SqlBuffer.StorageType.Double:
					return typeof(SqlDouble);
				case SqlBuffer.StorageType.Int16:
					return typeof(SqlInt16);
				case SqlBuffer.StorageType.Int32:
					return typeof(SqlInt32);
				case SqlBuffer.StorageType.Int64:
					return typeof(SqlInt64);
				case SqlBuffer.StorageType.Money:
					return typeof(SqlMoney);
				case SqlBuffer.StorageType.Single:
					return typeof(SqlSingle);
				case SqlBuffer.StorageType.String:
					return typeof(SqlString);
				case SqlBuffer.StorageType.SqlBinary:
					return typeof(object);
				case SqlBuffer.StorageType.SqlCachedBuffer:
					return typeof(SqlString);
				case SqlBuffer.StorageType.SqlGuid:
					return typeof(object);
				case SqlBuffer.StorageType.SqlXml:
					return typeof(SqlXml);
				}
			}
			else
			{
				switch (this._type)
				{
				case SqlBuffer.StorageType.Empty:
					return null;
				case SqlBuffer.StorageType.Boolean:
					return typeof(bool);
				case SqlBuffer.StorageType.Byte:
					return typeof(byte);
				case SqlBuffer.StorageType.DateTime:
					return typeof(DateTime);
				case SqlBuffer.StorageType.Decimal:
					return typeof(decimal);
				case SqlBuffer.StorageType.Double:
					return typeof(double);
				case SqlBuffer.StorageType.Int16:
					return typeof(short);
				case SqlBuffer.StorageType.Int32:
					return typeof(int);
				case SqlBuffer.StorageType.Int64:
					return typeof(long);
				case SqlBuffer.StorageType.Money:
					return typeof(decimal);
				case SqlBuffer.StorageType.Single:
					return typeof(float);
				case SqlBuffer.StorageType.String:
					return typeof(string);
				case SqlBuffer.StorageType.SqlBinary:
					return typeof(byte[]);
				case SqlBuffer.StorageType.SqlCachedBuffer:
					return typeof(string);
				case SqlBuffer.StorageType.SqlGuid:
					return typeof(Guid);
				case SqlBuffer.StorageType.SqlXml:
					return typeof(string);
				}
			}
			return null;
		}

		// Token: 0x060022F4 RID: 8948 RVA: 0x0026F24C File Offset: 0x0026E64C
		internal static SqlBuffer[] CreateBufferArray(int length)
		{
			SqlBuffer[] array = new SqlBuffer[length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new SqlBuffer();
			}
			return array;
		}

		// Token: 0x060022F5 RID: 8949 RVA: 0x0026F278 File Offset: 0x0026E678
		internal static SqlBuffer[] CloneBufferArray(SqlBuffer[] values)
		{
			SqlBuffer[] array = new SqlBuffer[values.Length];
			for (int i = 0; i < values.Length; i++)
			{
				array[i] = new SqlBuffer(values[i]);
			}
			return array;
		}

		// Token: 0x060022F6 RID: 8950 RVA: 0x0026F2A8 File Offset: 0x0026E6A8
		internal static void Clear(SqlBuffer[] values)
		{
			if (values != null)
			{
				for (int i = 0; i < values.Length; i++)
				{
					values[i].Clear();
				}
			}
		}

		// Token: 0x060022F7 RID: 8951 RVA: 0x0026F2D0 File Offset: 0x0026E6D0
		internal void Clear()
		{
			this._isNull = false;
			this._type = SqlBuffer.StorageType.Empty;
			this._object = null;
		}

		// Token: 0x060022F8 RID: 8952 RVA: 0x0026F2F4 File Offset: 0x0026E6F4
		internal void SetToDateTime(int daypart, int timepart)
		{
			this._value._dateTimeInfo.daypart = daypart;
			this._value._dateTimeInfo.timepart = timepart;
			this._type = SqlBuffer.StorageType.DateTime;
			this._isNull = false;
		}

		// Token: 0x060022F9 RID: 8953 RVA: 0x0026F334 File Offset: 0x0026E734
		internal void SetToDecimal(byte precision, byte scale, bool positive, int[] bits)
		{
			this._value._numericInfo.precision = precision;
			this._value._numericInfo.scale = scale;
			this._value._numericInfo.positive = positive;
			this._value._numericInfo.data1 = bits[0];
			this._value._numericInfo.data2 = bits[1];
			this._value._numericInfo.data3 = bits[2];
			this._value._numericInfo.data4 = bits[3];
			this._type = SqlBuffer.StorageType.Decimal;
			this._isNull = false;
		}

		// Token: 0x060022FA RID: 8954 RVA: 0x0026F3D4 File Offset: 0x0026E7D4
		internal void SetToMoney(long value)
		{
			this._value._int64 = value;
			this._type = SqlBuffer.StorageType.Money;
			this._isNull = false;
		}

		// Token: 0x060022FB RID: 8955 RVA: 0x0026F3FC File Offset: 0x0026E7FC
		internal void SetToNullOfType(SqlBuffer.StorageType storageType)
		{
			this._type = storageType;
			this._isNull = true;
			this._object = null;
		}

		// Token: 0x060022FC RID: 8956 RVA: 0x0026F420 File Offset: 0x0026E820
		internal void SetToString(string value)
		{
			this._object = value;
			this._type = SqlBuffer.StorageType.String;
			this._isNull = false;
		}

		// Token: 0x060022FD RID: 8957 RVA: 0x0026F444 File Offset: 0x0026E844
		internal void SetToDate(byte[] bytes)
		{
			this._type = SqlBuffer.StorageType.Date;
			this._value._int32 = SqlBuffer.GetDateFromByteArray(bytes, 0);
			this._isNull = false;
		}

		// Token: 0x060022FE RID: 8958 RVA: 0x0026F474 File Offset: 0x0026E874
		internal void SetToDate(DateTime date)
		{
			this._type = SqlBuffer.StorageType.Date;
			this._value._int32 = date.Subtract(DateTime.MinValue).Days;
			this._isNull = false;
		}

		// Token: 0x060022FF RID: 8959 RVA: 0x0026F4B0 File Offset: 0x0026E8B0
		internal void SetToTime(byte[] bytes, int length, byte scale)
		{
			this._type = SqlBuffer.StorageType.Time;
			SqlBuffer.FillInTimeInfo(ref this._value._timeInfo, bytes, length, scale);
			this._isNull = false;
		}

		// Token: 0x06002300 RID: 8960 RVA: 0x0026F4E0 File Offset: 0x0026E8E0
		internal void SetToTime(TimeSpan timeSpan, byte scale)
		{
			this._type = SqlBuffer.StorageType.Time;
			this._value._timeInfo.ticks = timeSpan.Ticks;
			this._value._timeInfo.scale = scale;
			this._isNull = false;
		}

		// Token: 0x06002301 RID: 8961 RVA: 0x0026F524 File Offset: 0x0026E924
		internal void SetToDateTime2(byte[] bytes, int length, byte scale)
		{
			this._type = SqlBuffer.StorageType.DateTime2;
			SqlBuffer.FillInTimeInfo(ref this._value._dateTime2Info.timeInfo, bytes, length - 3, scale);
			this._value._dateTime2Info.date = SqlBuffer.GetDateFromByteArray(bytes, length - 3);
			this._isNull = false;
		}

		// Token: 0x06002302 RID: 8962 RVA: 0x0026F574 File Offset: 0x0026E974
		internal void SetToDateTime2(DateTime dateTime, byte scale)
		{
			this._type = SqlBuffer.StorageType.DateTime2;
			this._value._dateTime2Info.timeInfo.ticks = dateTime.TimeOfDay.Ticks;
			this._value._dateTime2Info.timeInfo.scale = scale;
			this._value._dateTime2Info.date = dateTime.Subtract(DateTime.MinValue).Days;
			this._isNull = false;
		}

		// Token: 0x06002303 RID: 8963 RVA: 0x0026F5F0 File Offset: 0x0026E9F0
		internal void SetToDateTimeOffset(byte[] bytes, int length, byte scale)
		{
			this._type = SqlBuffer.StorageType.DateTimeOffset;
			SqlBuffer.FillInTimeInfo(ref this._value._dateTimeOffsetInfo.dateTime2Info.timeInfo, bytes, length - 5, scale);
			this._value._dateTimeOffsetInfo.dateTime2Info.date = SqlBuffer.GetDateFromByteArray(bytes, length - 5);
			this._value._dateTimeOffsetInfo.offset = (short)((int)bytes[length - 2] + ((int)bytes[length - 1] << 8));
			this._isNull = false;
		}

		// Token: 0x06002304 RID: 8964 RVA: 0x0026F668 File Offset: 0x0026EA68
		internal void SetToDateTimeOffset(DateTimeOffset dateTimeOffset, byte scale)
		{
			this._type = SqlBuffer.StorageType.DateTimeOffset;
			DateTime utcDateTime = dateTimeOffset.UtcDateTime;
			this._value._dateTimeOffsetInfo.dateTime2Info.timeInfo.ticks = utcDateTime.TimeOfDay.Ticks;
			this._value._dateTimeOffsetInfo.dateTime2Info.timeInfo.scale = scale;
			this._value._dateTimeOffsetInfo.dateTime2Info.date = utcDateTime.Subtract(DateTime.MinValue).Days;
			this._value._dateTimeOffsetInfo.offset = (short)dateTimeOffset.Offset.TotalMinutes;
			this._isNull = false;
		}

		// Token: 0x06002305 RID: 8965 RVA: 0x0026F71C File Offset: 0x0026EB1C
		private static void FillInTimeInfo(ref SqlBuffer.TimeInfo timeInfo, byte[] timeBytes, int length, byte scale)
		{
			long num = (long)((ulong)timeBytes[0] + ((ulong)timeBytes[1] << 8) + ((ulong)timeBytes[2] << 16));
			if (length > 3)
			{
				num += (long)((long)((ulong)timeBytes[3]) << 24);
			}
			if (length > 4)
			{
				num += (long)((long)((ulong)timeBytes[4]) << 32);
			}
			timeInfo.ticks = num * TdsEnums.TICKS_FROM_SCALE[(int)scale];
			timeInfo.scale = scale;
		}

		// Token: 0x06002306 RID: 8966 RVA: 0x0026F770 File Offset: 0x0026EB70
		private static int GetDateFromByteArray(byte[] buf, int offset)
		{
			return (int)buf[offset] + ((int)buf[offset + 1] << 8) + ((int)buf[offset + 2] << 16);
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x0026F794 File Offset: 0x0026EB94
		private void ThrowIfNull()
		{
			if (this.IsNull)
			{
				throw new SqlNullValueException();
			}
		}

		// Token: 0x0400167B RID: 5755
		private bool _isNull;

		// Token: 0x0400167C RID: 5756
		private SqlBuffer.StorageType _type;

		// Token: 0x0400167D RID: 5757
		private SqlBuffer.Storage _value;

		// Token: 0x0400167E RID: 5758
		private object _object;

		// Token: 0x0400167F RID: 5759
		private static string[] __katmaiDateTimeOffsetFormatByScale = new string[] { "yyyy-MM-dd HH:mm:ss zzz", "yyyy-MM-dd HH:mm:ss.f zzz", "yyyy-MM-dd HH:mm:ss.ff zzz", "yyyy-MM-dd HH:mm:ss.fff zzz", "yyyy-MM-dd HH:mm:ss.ffff zzz", "yyyy-MM-dd HH:mm:ss.fffff zzz", "yyyy-MM-dd HH:mm:ss.ffffff zzz", "yyyy-MM-dd HH:mm:ss.fffffff zzz" };

		// Token: 0x04001680 RID: 5760
		private static string[] __katmaiDateTime2FormatByScale = new string[] { "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm:ss.f", "yyyy-MM-dd HH:mm:ss.ff", "yyyy-MM-dd HH:mm:ss.fff", "yyyy-MM-dd HH:mm:ss.ffff", "yyyy-MM-dd HH:mm:ss.fffff", "yyyy-MM-dd HH:mm:ss.ffffff", "yyyy-MM-dd HH:mm:ss.fffffff" };

		// Token: 0x04001681 RID: 5761
		private static string[] __katmaiTimeFormatByScale = new string[] { "HH:mm:ss", "HH:mm:ss.f", "HH:mm:ss.ff", "HH:mm:ss.fff", "HH:mm:ss.ffff", "HH:mm:ss.fffff", "HH:mm:ss.ffffff", "HH:mm:ss.fffffff" };

		// Token: 0x020002A9 RID: 681
		internal enum StorageType
		{
			// Token: 0x04001683 RID: 5763
			Empty,
			// Token: 0x04001684 RID: 5764
			Boolean,
			// Token: 0x04001685 RID: 5765
			Byte,
			// Token: 0x04001686 RID: 5766
			DateTime,
			// Token: 0x04001687 RID: 5767
			Decimal,
			// Token: 0x04001688 RID: 5768
			Double,
			// Token: 0x04001689 RID: 5769
			Int16,
			// Token: 0x0400168A RID: 5770
			Int32,
			// Token: 0x0400168B RID: 5771
			Int64,
			// Token: 0x0400168C RID: 5772
			Money,
			// Token: 0x0400168D RID: 5773
			Single,
			// Token: 0x0400168E RID: 5774
			String,
			// Token: 0x0400168F RID: 5775
			SqlBinary,
			// Token: 0x04001690 RID: 5776
			SqlCachedBuffer,
			// Token: 0x04001691 RID: 5777
			SqlGuid,
			// Token: 0x04001692 RID: 5778
			SqlXml,
			// Token: 0x04001693 RID: 5779
			Date,
			// Token: 0x04001694 RID: 5780
			DateTime2,
			// Token: 0x04001695 RID: 5781
			DateTimeOffset,
			// Token: 0x04001696 RID: 5782
			Time
		}

		// Token: 0x020002AA RID: 682
		internal struct DateTimeInfo
		{
			// Token: 0x04001697 RID: 5783
			internal int daypart;

			// Token: 0x04001698 RID: 5784
			internal int timepart;
		}

		// Token: 0x020002AB RID: 683
		internal struct NumericInfo
		{
			// Token: 0x04001699 RID: 5785
			internal int data1;

			// Token: 0x0400169A RID: 5786
			internal int data2;

			// Token: 0x0400169B RID: 5787
			internal int data3;

			// Token: 0x0400169C RID: 5788
			internal int data4;

			// Token: 0x0400169D RID: 5789
			internal byte precision;

			// Token: 0x0400169E RID: 5790
			internal byte scale;

			// Token: 0x0400169F RID: 5791
			internal bool positive;
		}

		// Token: 0x020002AC RID: 684
		internal struct TimeInfo
		{
			// Token: 0x040016A0 RID: 5792
			internal long ticks;

			// Token: 0x040016A1 RID: 5793
			internal byte scale;
		}

		// Token: 0x020002AD RID: 685
		internal struct DateTime2Info
		{
			// Token: 0x040016A2 RID: 5794
			internal int date;

			// Token: 0x040016A3 RID: 5795
			internal SqlBuffer.TimeInfo timeInfo;
		}

		// Token: 0x020002AE RID: 686
		internal struct DateTimeOffsetInfo
		{
			// Token: 0x040016A4 RID: 5796
			internal SqlBuffer.DateTime2Info dateTime2Info;

			// Token: 0x040016A5 RID: 5797
			internal short offset;
		}

		// Token: 0x020002AF RID: 687
		[StructLayout(LayoutKind.Explicit)]
		internal struct Storage
		{
			// Token: 0x040016A6 RID: 5798
			[FieldOffset(0)]
			internal bool _boolean;

			// Token: 0x040016A7 RID: 5799
			[FieldOffset(0)]
			internal byte _byte;

			// Token: 0x040016A8 RID: 5800
			[FieldOffset(0)]
			internal SqlBuffer.DateTimeInfo _dateTimeInfo;

			// Token: 0x040016A9 RID: 5801
			[FieldOffset(0)]
			internal double _double;

			// Token: 0x040016AA RID: 5802
			[FieldOffset(0)]
			internal SqlBuffer.NumericInfo _numericInfo;

			// Token: 0x040016AB RID: 5803
			[FieldOffset(0)]
			internal short _int16;

			// Token: 0x040016AC RID: 5804
			[FieldOffset(0)]
			internal int _int32;

			// Token: 0x040016AD RID: 5805
			[FieldOffset(0)]
			internal long _int64;

			// Token: 0x040016AE RID: 5806
			[FieldOffset(0)]
			internal float _single;

			// Token: 0x040016AF RID: 5807
			[FieldOffset(0)]
			internal SqlBuffer.TimeInfo _timeInfo;

			// Token: 0x040016B0 RID: 5808
			[FieldOffset(0)]
			internal SqlBuffer.DateTime2Info _dateTime2Info;

			// Token: 0x040016B1 RID: 5809
			[FieldOffset(0)]
			internal SqlBuffer.DateTimeOffsetInfo _dateTimeOffsetInfo;
		}
	}
}
