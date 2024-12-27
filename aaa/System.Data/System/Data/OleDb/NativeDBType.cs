using System;
using System.Data.Common;

namespace System.Data.OleDb
{
	// Token: 0x02000210 RID: 528
	internal sealed class NativeDBType
	{
		// Token: 0x06001D8C RID: 7564 RVA: 0x00252170 File Offset: 0x00251570
		internal static bool HasHighBit(short value)
		{
			return 0 != (-4096 & value);
		}

		// Token: 0x06001D8D RID: 7565 RVA: 0x0025218C File Offset: 0x0025158C
		private NativeDBType(byte maxpre, int fixlen, bool isfixed, bool islong, OleDbType enumOleDbType, short dbType, string dbstring, Type dataType, short wType, DbType enumDbType)
		{
			this.enumOleDbType = enumOleDbType;
			this.dbType = dbType;
			this.dbPart = ((-1 == fixlen) ? 7 : 5);
			this.isfixed = isfixed;
			this.islong = islong;
			this.maxpre = maxpre;
			this.fixlen = fixlen;
			this.wType = wType;
			this.dataSourceType = dbstring;
			this.dbString = new StringMemHandle(dbstring);
			this.dataType = dataType;
			this.enumDbType = enumDbType;
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06001D8E RID: 7566 RVA: 0x00252208 File Offset: 0x00251608
		internal bool IsVariableLength
		{
			get
			{
				return -1 == this.fixlen;
			}
		}

		// Token: 0x06001D8F RID: 7567 RVA: 0x00252220 File Offset: 0x00251620
		internal static NativeDBType FromDataType(OleDbType enumOleDbType)
		{
			if (enumOleDbType <= OleDbType.Filetime)
			{
				switch (enumOleDbType)
				{
				case OleDbType.Empty:
					return NativeDBType.D_Empty;
				case (OleDbType)1:
				case (OleDbType)15:
					break;
				case OleDbType.SmallInt:
					return NativeDBType.D_SmallInt;
				case OleDbType.Integer:
					return NativeDBType.D_Integer;
				case OleDbType.Single:
					return NativeDBType.D_Single;
				case OleDbType.Double:
					return NativeDBType.D_Double;
				case OleDbType.Currency:
					return NativeDBType.D_Currency;
				case OleDbType.Date:
					return NativeDBType.D_Date;
				case OleDbType.BSTR:
					return NativeDBType.D_BSTR;
				case OleDbType.IDispatch:
					return NativeDBType.D_IDispatch;
				case OleDbType.Error:
					return NativeDBType.D_Error;
				case OleDbType.Boolean:
					return NativeDBType.D_Boolean;
				case OleDbType.Variant:
					return NativeDBType.D_Variant;
				case OleDbType.IUnknown:
					return NativeDBType.D_IUnknown;
				case OleDbType.Decimal:
					return NativeDBType.D_Decimal;
				case OleDbType.TinyInt:
					return NativeDBType.D_TinyInt;
				case OleDbType.UnsignedTinyInt:
					return NativeDBType.D_UnsignedTinyInt;
				case OleDbType.UnsignedSmallInt:
					return NativeDBType.D_UnsignedSmallInt;
				case OleDbType.UnsignedInt:
					return NativeDBType.D_UnsignedInt;
				case OleDbType.BigInt:
					return NativeDBType.D_BigInt;
				case OleDbType.UnsignedBigInt:
					return NativeDBType.D_UnsignedBigInt;
				default:
					if (enumOleDbType == OleDbType.Filetime)
					{
						return NativeDBType.D_Filetime;
					}
					break;
				}
			}
			else
			{
				if (enumOleDbType == OleDbType.Guid)
				{
					return NativeDBType.D_Guid;
				}
				switch (enumOleDbType)
				{
				case OleDbType.Binary:
					return NativeDBType.D_Binary;
				case OleDbType.Char:
					return NativeDBType.D_Char;
				case OleDbType.WChar:
					return NativeDBType.D_WChar;
				case OleDbType.Numeric:
					return NativeDBType.D_Numeric;
				case (OleDbType)132:
				case (OleDbType)136:
				case (OleDbType)137:
					break;
				case OleDbType.DBDate:
					return NativeDBType.D_DBDate;
				case OleDbType.DBTime:
					return NativeDBType.D_DBTime;
				case OleDbType.DBTimeStamp:
					return NativeDBType.D_DBTimeStamp;
				case OleDbType.PropVariant:
					return NativeDBType.D_PropVariant;
				case OleDbType.VarNumeric:
					return NativeDBType.D_VarNumeric;
				default:
					switch (enumOleDbType)
					{
					case OleDbType.VarChar:
						return NativeDBType.D_VarChar;
					case OleDbType.LongVarChar:
						return NativeDBType.D_LongVarChar;
					case OleDbType.VarWChar:
						return NativeDBType.D_VarWChar;
					case OleDbType.LongVarWChar:
						return NativeDBType.D_LongVarWChar;
					case OleDbType.VarBinary:
						return NativeDBType.D_VarBinary;
					case OleDbType.LongVarBinary:
						return NativeDBType.D_LongVarBinary;
					}
					break;
				}
			}
			throw ODB.InvalidOleDbType(enumOleDbType);
		}

		// Token: 0x06001D90 RID: 7568 RVA: 0x002523F0 File Offset: 0x002517F0
		internal static NativeDBType FromSystemType(object value)
		{
			IConvertible convertible = value as IConvertible;
			if (convertible != null)
			{
				switch (convertible.GetTypeCode())
				{
				case TypeCode.Empty:
					return NativeDBType.D_Empty;
				case TypeCode.Object:
					return NativeDBType.D_Variant;
				case TypeCode.DBNull:
					throw ADP.InvalidDataType(TypeCode.DBNull);
				case TypeCode.Boolean:
					return NativeDBType.D_Boolean;
				case TypeCode.Char:
					return NativeDBType.D_Char;
				case TypeCode.SByte:
					return NativeDBType.D_TinyInt;
				case TypeCode.Byte:
					return NativeDBType.D_UnsignedTinyInt;
				case TypeCode.Int16:
					return NativeDBType.D_SmallInt;
				case TypeCode.UInt16:
					return NativeDBType.D_UnsignedSmallInt;
				case TypeCode.Int32:
					return NativeDBType.D_Integer;
				case TypeCode.UInt32:
					return NativeDBType.D_UnsignedInt;
				case TypeCode.Int64:
					return NativeDBType.D_BigInt;
				case TypeCode.UInt64:
					return NativeDBType.D_UnsignedBigInt;
				case TypeCode.Single:
					return NativeDBType.D_Single;
				case TypeCode.Double:
					return NativeDBType.D_Double;
				case TypeCode.Decimal:
					return NativeDBType.D_Decimal;
				case TypeCode.DateTime:
					return NativeDBType.D_DBTimeStamp;
				case TypeCode.String:
					return NativeDBType.D_VarWChar;
				}
				throw ADP.UnknownDataTypeCode(value.GetType(), convertible.GetTypeCode());
			}
			if (value is byte[])
			{
				return NativeDBType.D_VarBinary;
			}
			if (value is Guid)
			{
				return NativeDBType.D_Guid;
			}
			if (value is TimeSpan)
			{
				return NativeDBType.D_DBTime;
			}
			return NativeDBType.D_Variant;
		}

		// Token: 0x06001D91 RID: 7569 RVA: 0x00252514 File Offset: 0x00251914
		internal static NativeDBType FromDbType(DbType dbType)
		{
			switch (dbType)
			{
			case DbType.AnsiString:
				return NativeDBType.D_VarChar;
			case DbType.Binary:
				return NativeDBType.D_VarBinary;
			case DbType.Byte:
				return NativeDBType.D_UnsignedTinyInt;
			case DbType.Boolean:
				return NativeDBType.D_Boolean;
			case DbType.Currency:
				return NativeDBType.D_Currency;
			case DbType.Date:
				return NativeDBType.D_DBDate;
			case DbType.DateTime:
				return NativeDBType.D_DBTimeStamp;
			case DbType.Decimal:
				return NativeDBType.D_Decimal;
			case DbType.Double:
				return NativeDBType.D_Double;
			case DbType.Guid:
				return NativeDBType.D_Guid;
			case DbType.Int16:
				return NativeDBType.D_SmallInt;
			case DbType.Int32:
				return NativeDBType.D_Integer;
			case DbType.Int64:
				return NativeDBType.D_BigInt;
			case DbType.Object:
				return NativeDBType.D_Variant;
			case DbType.SByte:
				return NativeDBType.D_TinyInt;
			case DbType.Single:
				return NativeDBType.D_Single;
			case DbType.String:
				return NativeDBType.D_VarWChar;
			case DbType.Time:
				return NativeDBType.D_DBTime;
			case DbType.UInt16:
				return NativeDBType.D_UnsignedSmallInt;
			case DbType.UInt32:
				return NativeDBType.D_UnsignedInt;
			case DbType.UInt64:
				return NativeDBType.D_UnsignedBigInt;
			case DbType.VarNumeric:
				return NativeDBType.D_VarNumeric;
			case DbType.AnsiStringFixedLength:
				return NativeDBType.D_Char;
			case DbType.StringFixedLength:
				return NativeDBType.D_WChar;
			case DbType.Xml:
				return NativeDBType.D_Xml;
			}
			throw ADP.DbTypeNotSupported(dbType, typeof(OleDbType));
		}

		// Token: 0x06001D92 RID: 7570 RVA: 0x0025263C File Offset: 0x00251A3C
		internal static NativeDBType FromDBType(short dbType, bool isLong, bool isFixed)
		{
			if (dbType <= 64)
			{
				switch (dbType)
				{
				case 2:
					return NativeDBType.D_SmallInt;
				case 3:
					return NativeDBType.D_Integer;
				case 4:
					return NativeDBType.D_Single;
				case 5:
					return NativeDBType.D_Double;
				case 6:
					return NativeDBType.D_Currency;
				case 7:
					return NativeDBType.D_Date;
				case 8:
					return NativeDBType.D_BSTR;
				case 9:
					return NativeDBType.D_IDispatch;
				case 10:
					return NativeDBType.D_Error;
				case 11:
					return NativeDBType.D_Boolean;
				case 12:
					return NativeDBType.D_Variant;
				case 13:
					return NativeDBType.D_IUnknown;
				case 14:
					return NativeDBType.D_Decimal;
				case 15:
					break;
				case 16:
					return NativeDBType.D_TinyInt;
				case 17:
					return NativeDBType.D_UnsignedTinyInt;
				case 18:
					return NativeDBType.D_UnsignedSmallInt;
				case 19:
					return NativeDBType.D_UnsignedInt;
				case 20:
					return NativeDBType.D_BigInt;
				case 21:
					return NativeDBType.D_UnsignedBigInt;
				default:
					if (dbType == 64)
					{
						return NativeDBType.D_Filetime;
					}
					break;
				}
			}
			else
			{
				if (dbType == 72)
				{
					return NativeDBType.D_Guid;
				}
				switch (dbType)
				{
				case 128:
					if (isLong)
					{
						return NativeDBType.D_LongVarBinary;
					}
					if (!isFixed)
					{
						return NativeDBType.D_VarBinary;
					}
					return NativeDBType.D_Binary;
				case 129:
					if (isLong)
					{
						return NativeDBType.D_LongVarChar;
					}
					if (!isFixed)
					{
						return NativeDBType.D_VarChar;
					}
					return NativeDBType.D_Char;
				case 130:
					if (isLong)
					{
						return NativeDBType.D_LongVarWChar;
					}
					if (!isFixed)
					{
						return NativeDBType.D_VarWChar;
					}
					return NativeDBType.D_WChar;
				case 131:
					return NativeDBType.D_Numeric;
				case 132:
					return NativeDBType.D_Udt;
				case 133:
					return NativeDBType.D_DBDate;
				case 134:
					return NativeDBType.D_DBTime;
				case 135:
					return NativeDBType.D_DBTimeStamp;
				case 136:
					return NativeDBType.D_Chapter;
				case 138:
					return NativeDBType.D_PropVariant;
				case 139:
					return NativeDBType.D_VarNumeric;
				case 141:
					return NativeDBType.D_Xml;
				}
			}
			if ((4096 & dbType) != 0)
			{
				throw ODB.DBBindingGetVector();
			}
			return NativeDBType.D_Variant;
		}

		// Token: 0x040010D2 RID: 4306
		internal const short EMPTY = 0;

		// Token: 0x040010D3 RID: 4307
		internal const short NULL = 1;

		// Token: 0x040010D4 RID: 4308
		internal const short I2 = 2;

		// Token: 0x040010D5 RID: 4309
		internal const short I4 = 3;

		// Token: 0x040010D6 RID: 4310
		internal const short R4 = 4;

		// Token: 0x040010D7 RID: 4311
		internal const short R8 = 5;

		// Token: 0x040010D8 RID: 4312
		internal const short CY = 6;

		// Token: 0x040010D9 RID: 4313
		internal const short DATE = 7;

		// Token: 0x040010DA RID: 4314
		internal const short BSTR = 8;

		// Token: 0x040010DB RID: 4315
		internal const short IDISPATCH = 9;

		// Token: 0x040010DC RID: 4316
		internal const short ERROR = 10;

		// Token: 0x040010DD RID: 4317
		internal const short BOOL = 11;

		// Token: 0x040010DE RID: 4318
		internal const short VARIANT = 12;

		// Token: 0x040010DF RID: 4319
		internal const short IUNKNOWN = 13;

		// Token: 0x040010E0 RID: 4320
		internal const short DECIMAL = 14;

		// Token: 0x040010E1 RID: 4321
		internal const short I1 = 16;

		// Token: 0x040010E2 RID: 4322
		internal const short UI1 = 17;

		// Token: 0x040010E3 RID: 4323
		internal const short UI2 = 18;

		// Token: 0x040010E4 RID: 4324
		internal const short UI4 = 19;

		// Token: 0x040010E5 RID: 4325
		internal const short I8 = 20;

		// Token: 0x040010E6 RID: 4326
		internal const short UI8 = 21;

		// Token: 0x040010E7 RID: 4327
		internal const short FILETIME = 64;

		// Token: 0x040010E8 RID: 4328
		internal const short DBUTCDATETIME = 65;

		// Token: 0x040010E9 RID: 4329
		internal const short DBTIME_EX = 66;

		// Token: 0x040010EA RID: 4330
		internal const short GUID = 72;

		// Token: 0x040010EB RID: 4331
		internal const short BYTES = 128;

		// Token: 0x040010EC RID: 4332
		internal const short STR = 129;

		// Token: 0x040010ED RID: 4333
		internal const short WSTR = 130;

		// Token: 0x040010EE RID: 4334
		internal const short NUMERIC = 131;

		// Token: 0x040010EF RID: 4335
		internal const short UDT = 132;

		// Token: 0x040010F0 RID: 4336
		internal const short DBDATE = 133;

		// Token: 0x040010F1 RID: 4337
		internal const short DBTIME = 134;

		// Token: 0x040010F2 RID: 4338
		internal const short DBTIMESTAMP = 135;

		// Token: 0x040010F3 RID: 4339
		internal const short HCHAPTER = 136;

		// Token: 0x040010F4 RID: 4340
		internal const short PROPVARIANT = 138;

		// Token: 0x040010F5 RID: 4341
		internal const short VARNUMERIC = 139;

		// Token: 0x040010F6 RID: 4342
		internal const short XML = 141;

		// Token: 0x040010F7 RID: 4343
		internal const short VECTOR = 4096;

		// Token: 0x040010F8 RID: 4344
		internal const short ARRAY = 8192;

		// Token: 0x040010F9 RID: 4345
		internal const short BYREF = 16384;

		// Token: 0x040010FA RID: 4346
		internal const short RESERVED = -32768;

		// Token: 0x040010FB RID: 4347
		internal const short HighMask = -4096;

		// Token: 0x040010FC RID: 4348
		private const string S_BINARY = "DBTYPE_BINARY";

		// Token: 0x040010FD RID: 4349
		private const string S_BOOL = "DBTYPE_BOOL";

		// Token: 0x040010FE RID: 4350
		private const string S_BSTR = "DBTYPE_BSTR";

		// Token: 0x040010FF RID: 4351
		private const string S_CHAR = "DBTYPE_CHAR";

		// Token: 0x04001100 RID: 4352
		private const string S_CY = "DBTYPE_CY";

		// Token: 0x04001101 RID: 4353
		private const string S_DATE = "DBTYPE_DATE";

		// Token: 0x04001102 RID: 4354
		private const string S_DBDATE = "DBTYPE_DBDATE";

		// Token: 0x04001103 RID: 4355
		private const string S_DBTIME = "DBTYPE_DBTIME";

		// Token: 0x04001104 RID: 4356
		private const string S_DBTIMESTAMP = "DBTYPE_DBTIMESTAMP";

		// Token: 0x04001105 RID: 4357
		private const string S_DECIMAL = "DBTYPE_DECIMAL";

		// Token: 0x04001106 RID: 4358
		private const string S_ERROR = "DBTYPE_ERROR";

		// Token: 0x04001107 RID: 4359
		private const string S_FILETIME = "DBTYPE_FILETIME";

		// Token: 0x04001108 RID: 4360
		private const string S_GUID = "DBTYPE_GUID";

		// Token: 0x04001109 RID: 4361
		private const string S_I1 = "DBTYPE_I1";

		// Token: 0x0400110A RID: 4362
		private const string S_I2 = "DBTYPE_I2";

		// Token: 0x0400110B RID: 4363
		private const string S_I4 = "DBTYPE_I4";

		// Token: 0x0400110C RID: 4364
		private const string S_I8 = "DBTYPE_I8";

		// Token: 0x0400110D RID: 4365
		private const string S_IDISPATCH = "DBTYPE_IDISPATCH";

		// Token: 0x0400110E RID: 4366
		private const string S_IUNKNOWN = "DBTYPE_IUNKNOWN";

		// Token: 0x0400110F RID: 4367
		private const string S_LONGVARBINARY = "DBTYPE_LONGVARBINARY";

		// Token: 0x04001110 RID: 4368
		private const string S_LONGVARCHAR = "DBTYPE_LONGVARCHAR";

		// Token: 0x04001111 RID: 4369
		private const string S_NUMERIC = "DBTYPE_NUMERIC";

		// Token: 0x04001112 RID: 4370
		private const string S_PROPVARIANT = "DBTYPE_PROPVARIANT";

		// Token: 0x04001113 RID: 4371
		private const string S_R4 = "DBTYPE_R4";

		// Token: 0x04001114 RID: 4372
		private const string S_R8 = "DBTYPE_R8";

		// Token: 0x04001115 RID: 4373
		private const string S_UDT = "DBTYPE_UDT";

		// Token: 0x04001116 RID: 4374
		private const string S_UI1 = "DBTYPE_UI1";

		// Token: 0x04001117 RID: 4375
		private const string S_UI2 = "DBTYPE_UI2";

		// Token: 0x04001118 RID: 4376
		private const string S_UI4 = "DBTYPE_UI4";

		// Token: 0x04001119 RID: 4377
		private const string S_UI8 = "DBTYPE_UI8";

		// Token: 0x0400111A RID: 4378
		private const string S_VARBINARY = "DBTYPE_VARBINARY";

		// Token: 0x0400111B RID: 4379
		private const string S_VARCHAR = "DBTYPE_VARCHAR";

		// Token: 0x0400111C RID: 4380
		private const string S_VARIANT = "DBTYPE_VARIANT";

		// Token: 0x0400111D RID: 4381
		private const string S_VARNUMERIC = "DBTYPE_VARNUMERIC";

		// Token: 0x0400111E RID: 4382
		private const string S_WCHAR = "DBTYPE_WCHAR";

		// Token: 0x0400111F RID: 4383
		private const string S_WVARCHAR = "DBTYPE_WVARCHAR";

		// Token: 0x04001120 RID: 4384
		private const string S_WLONGVARCHAR = "DBTYPE_WLONGVARCHAR";

		// Token: 0x04001121 RID: 4385
		private const string S_XML = "DBTYPE_XML";

		// Token: 0x04001122 RID: 4386
		private const int FixedDbPart = 5;

		// Token: 0x04001123 RID: 4387
		private const int VarblDbPart = 7;

		// Token: 0x04001124 RID: 4388
		private static readonly NativeDBType D_Binary = new NativeDBType(byte.MaxValue, -1, true, false, OleDbType.Binary, 128, "DBTYPE_BINARY", typeof(byte[]), 128, DbType.Binary);

		// Token: 0x04001125 RID: 4389
		private static readonly NativeDBType D_Boolean = new NativeDBType(byte.MaxValue, 2, true, false, OleDbType.Boolean, 11, "DBTYPE_BOOL", typeof(bool), 11, DbType.Boolean);

		// Token: 0x04001126 RID: 4390
		private static readonly NativeDBType D_BSTR = new NativeDBType(byte.MaxValue, ADP.PtrSize, false, false, OleDbType.BSTR, 8, "DBTYPE_BSTR", typeof(string), 8, DbType.String);

		// Token: 0x04001127 RID: 4391
		private static readonly NativeDBType D_Char = new NativeDBType(byte.MaxValue, -1, true, false, OleDbType.Char, 129, "DBTYPE_CHAR", typeof(string), 130, DbType.AnsiStringFixedLength);

		// Token: 0x04001128 RID: 4392
		private static readonly NativeDBType D_Currency = new NativeDBType(19, 8, true, false, OleDbType.Currency, 6, "DBTYPE_CY", typeof(decimal), 6, DbType.Currency);

		// Token: 0x04001129 RID: 4393
		private static readonly NativeDBType D_Date = new NativeDBType(byte.MaxValue, 8, true, false, OleDbType.Date, 7, "DBTYPE_DATE", typeof(DateTime), 7, DbType.DateTime);

		// Token: 0x0400112A RID: 4394
		private static readonly NativeDBType D_DBDate = new NativeDBType(byte.MaxValue, 6, true, false, OleDbType.DBDate, 133, "DBTYPE_DBDATE", typeof(DateTime), 133, DbType.Date);

		// Token: 0x0400112B RID: 4395
		private static readonly NativeDBType D_DBTime = new NativeDBType(byte.MaxValue, 6, true, false, OleDbType.DBTime, 134, "DBTYPE_DBTIME", typeof(TimeSpan), 134, DbType.Time);

		// Token: 0x0400112C RID: 4396
		private static readonly NativeDBType D_DBTimeStamp = new NativeDBType(byte.MaxValue, 16, true, false, OleDbType.DBTimeStamp, 135, "DBTYPE_DBTIMESTAMP", typeof(DateTime), 135, DbType.DateTime);

		// Token: 0x0400112D RID: 4397
		private static readonly NativeDBType D_Decimal = new NativeDBType(28, 16, true, false, OleDbType.Decimal, 14, "DBTYPE_DECIMAL", typeof(decimal), 14, DbType.Decimal);

		// Token: 0x0400112E RID: 4398
		private static readonly NativeDBType D_Error = new NativeDBType(byte.MaxValue, 4, true, false, OleDbType.Error, 10, "DBTYPE_ERROR", typeof(int), 10, DbType.Int32);

		// Token: 0x0400112F RID: 4399
		private static readonly NativeDBType D_Filetime = new NativeDBType(byte.MaxValue, 8, true, false, OleDbType.Filetime, 64, "DBTYPE_FILETIME", typeof(DateTime), 64, DbType.DateTime);

		// Token: 0x04001130 RID: 4400
		private static readonly NativeDBType D_Guid = new NativeDBType(byte.MaxValue, 16, true, false, OleDbType.Guid, 72, "DBTYPE_GUID", typeof(Guid), 72, DbType.Guid);

		// Token: 0x04001131 RID: 4401
		private static readonly NativeDBType D_TinyInt = new NativeDBType(3, 1, true, false, OleDbType.TinyInt, 16, "DBTYPE_I1", typeof(short), 16, DbType.SByte);

		// Token: 0x04001132 RID: 4402
		private static readonly NativeDBType D_SmallInt = new NativeDBType(5, 2, true, false, OleDbType.SmallInt, 2, "DBTYPE_I2", typeof(short), 2, DbType.Int16);

		// Token: 0x04001133 RID: 4403
		private static readonly NativeDBType D_Integer = new NativeDBType(10, 4, true, false, OleDbType.Integer, 3, "DBTYPE_I4", typeof(int), 3, DbType.Int32);

		// Token: 0x04001134 RID: 4404
		private static readonly NativeDBType D_BigInt = new NativeDBType(19, 8, true, false, OleDbType.BigInt, 20, "DBTYPE_I8", typeof(long), 20, DbType.Int64);

		// Token: 0x04001135 RID: 4405
		private static readonly NativeDBType D_IDispatch = new NativeDBType(byte.MaxValue, ADP.PtrSize, true, false, OleDbType.IDispatch, 9, "DBTYPE_IDISPATCH", typeof(object), 9, DbType.Object);

		// Token: 0x04001136 RID: 4406
		private static readonly NativeDBType D_IUnknown = new NativeDBType(byte.MaxValue, ADP.PtrSize, true, false, OleDbType.IUnknown, 13, "DBTYPE_IUNKNOWN", typeof(object), 13, DbType.Object);

		// Token: 0x04001137 RID: 4407
		private static readonly NativeDBType D_LongVarBinary = new NativeDBType(byte.MaxValue, -1, false, true, OleDbType.LongVarBinary, 128, "DBTYPE_LONGVARBINARY", typeof(byte[]), 128, DbType.Binary);

		// Token: 0x04001138 RID: 4408
		private static readonly NativeDBType D_LongVarChar = new NativeDBType(byte.MaxValue, -1, false, true, OleDbType.LongVarChar, 129, "DBTYPE_LONGVARCHAR", typeof(string), 130, DbType.AnsiString);

		// Token: 0x04001139 RID: 4409
		private static readonly NativeDBType D_Numeric = new NativeDBType(28, 19, true, false, OleDbType.Numeric, 131, "DBTYPE_NUMERIC", typeof(decimal), 131, DbType.Decimal);

		// Token: 0x0400113A RID: 4410
		private static readonly NativeDBType D_PropVariant = new NativeDBType(byte.MaxValue, NativeOledbWrapper.SizeOfPROPVARIANT, true, false, OleDbType.PropVariant, 138, "DBTYPE_PROPVARIANT", typeof(object), 12, DbType.Object);

		// Token: 0x0400113B RID: 4411
		private static readonly NativeDBType D_Single = new NativeDBType(7, 4, true, false, OleDbType.Single, 4, "DBTYPE_R4", typeof(float), 4, DbType.Single);

		// Token: 0x0400113C RID: 4412
		private static readonly NativeDBType D_Double = new NativeDBType(15, 8, true, false, OleDbType.Double, 5, "DBTYPE_R8", typeof(double), 5, DbType.Double);

		// Token: 0x0400113D RID: 4413
		private static readonly NativeDBType D_UnsignedTinyInt = new NativeDBType(3, 1, true, false, OleDbType.UnsignedTinyInt, 17, "DBTYPE_UI1", typeof(byte), 17, DbType.Byte);

		// Token: 0x0400113E RID: 4414
		private static readonly NativeDBType D_UnsignedSmallInt = new NativeDBType(5, 2, true, false, OleDbType.UnsignedSmallInt, 18, "DBTYPE_UI2", typeof(int), 18, DbType.UInt16);

		// Token: 0x0400113F RID: 4415
		private static readonly NativeDBType D_UnsignedInt = new NativeDBType(10, 4, true, false, OleDbType.UnsignedInt, 19, "DBTYPE_UI4", typeof(long), 19, DbType.UInt32);

		// Token: 0x04001140 RID: 4416
		private static readonly NativeDBType D_UnsignedBigInt = new NativeDBType(20, 8, true, false, OleDbType.UnsignedBigInt, 21, "DBTYPE_UI8", typeof(decimal), 21, DbType.UInt64);

		// Token: 0x04001141 RID: 4417
		private static readonly NativeDBType D_VarBinary = new NativeDBType(byte.MaxValue, -1, false, false, OleDbType.VarBinary, 128, "DBTYPE_VARBINARY", typeof(byte[]), 128, DbType.Binary);

		// Token: 0x04001142 RID: 4418
		private static readonly NativeDBType D_VarChar = new NativeDBType(byte.MaxValue, -1, false, false, OleDbType.VarChar, 129, "DBTYPE_VARCHAR", typeof(string), 130, DbType.AnsiString);

		// Token: 0x04001143 RID: 4419
		private static readonly NativeDBType D_Variant = new NativeDBType(byte.MaxValue, ODB.SizeOf_Variant, true, false, OleDbType.Variant, 12, "DBTYPE_VARIANT", typeof(object), 12, DbType.Object);

		// Token: 0x04001144 RID: 4420
		private static readonly NativeDBType D_VarNumeric = new NativeDBType(byte.MaxValue, 16, true, false, OleDbType.VarNumeric, 139, "DBTYPE_VARNUMERIC", typeof(decimal), 14, DbType.VarNumeric);

		// Token: 0x04001145 RID: 4421
		private static readonly NativeDBType D_WChar = new NativeDBType(byte.MaxValue, -1, true, false, OleDbType.WChar, 130, "DBTYPE_WCHAR", typeof(string), 130, DbType.StringFixedLength);

		// Token: 0x04001146 RID: 4422
		private static readonly NativeDBType D_VarWChar = new NativeDBType(byte.MaxValue, -1, false, false, OleDbType.VarWChar, 130, "DBTYPE_WVARCHAR", typeof(string), 130, DbType.String);

		// Token: 0x04001147 RID: 4423
		private static readonly NativeDBType D_LongVarWChar = new NativeDBType(byte.MaxValue, -1, false, true, OleDbType.LongVarWChar, 130, "DBTYPE_WLONGVARCHAR", typeof(string), 130, DbType.String);

		// Token: 0x04001148 RID: 4424
		private static readonly NativeDBType D_Chapter = new NativeDBType(byte.MaxValue, ADP.PtrSize, false, false, OleDbType.Empty, 136, "DBTYPE_UDT", typeof(IDataReader), 136, DbType.Object);

		// Token: 0x04001149 RID: 4425
		private static readonly NativeDBType D_Empty = new NativeDBType(byte.MaxValue, 0, false, false, OleDbType.Empty, 0, "", null, 0, DbType.Object);

		// Token: 0x0400114A RID: 4426
		private static readonly NativeDBType D_Xml = new NativeDBType(byte.MaxValue, -1, false, false, OleDbType.VarWChar, 141, "DBTYPE_XML", typeof(string), 130, DbType.String);

		// Token: 0x0400114B RID: 4427
		private static readonly NativeDBType D_Udt = new NativeDBType(byte.MaxValue, -1, false, false, OleDbType.VarBinary, 132, "DBTYPE_BINARY", typeof(byte[]), 128, DbType.Binary);

		// Token: 0x0400114C RID: 4428
		internal static readonly NativeDBType Default = NativeDBType.D_VarWChar;

		// Token: 0x0400114D RID: 4429
		internal static readonly byte MaximumDecimalPrecision = NativeDBType.D_Decimal.maxpre;

		// Token: 0x0400114E RID: 4430
		internal readonly OleDbType enumOleDbType;

		// Token: 0x0400114F RID: 4431
		internal readonly DbType enumDbType;

		// Token: 0x04001150 RID: 4432
		internal readonly short dbType;

		// Token: 0x04001151 RID: 4433
		internal readonly short wType;

		// Token: 0x04001152 RID: 4434
		internal readonly Type dataType;

		// Token: 0x04001153 RID: 4435
		internal readonly int dbPart;

		// Token: 0x04001154 RID: 4436
		internal readonly bool isfixed;

		// Token: 0x04001155 RID: 4437
		internal readonly bool islong;

		// Token: 0x04001156 RID: 4438
		internal readonly byte maxpre;

		// Token: 0x04001157 RID: 4439
		internal readonly int fixlen;

		// Token: 0x04001158 RID: 4440
		internal readonly string dataSourceType;

		// Token: 0x04001159 RID: 4441
		internal readonly StringMemHandle dbString;
	}
}
