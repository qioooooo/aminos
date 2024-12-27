using System;
using System.Data.SqlTypes;

namespace System.Data.Design
{
	// Token: 0x020000C6 RID: 198
	internal class TypeConvertions
	{
		// Token: 0x0600089E RID: 2206 RVA: 0x0001ACB1 File Offset: 0x00019CB1
		private TypeConvertions()
		{
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0001ACBC File Offset: 0x00019CBC
		internal static Type SqlDbTypeToSqlType(SqlDbType sqlDbType)
		{
			for (int i = 1; i < TypeConvertions.sqlTypeToSqlDbTypeMap.Length; i += 2)
			{
				if (sqlDbType == (SqlDbType)TypeConvertions.sqlTypeToSqlDbTypeMap[i])
				{
					return (Type)TypeConvertions.sqlTypeToSqlDbTypeMap[i - 1];
				}
			}
			return null;
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0001ACFC File Offset: 0x00019CFC
		internal static Type DbTypeToUrtType(DbType dbType)
		{
			for (int i = 0; i < TypeConvertions.dbTypeToUrtTypeMap.Length; i += 2)
			{
				if (dbType == (DbType)TypeConvertions.dbTypeToUrtTypeMap[i])
				{
					return (Type)TypeConvertions.dbTypeToUrtTypeMap[i + 1];
				}
			}
			return null;
		}

		// Token: 0x04000C73 RID: 3187
		private static int[] oleDbToAdoPlusDirectionMap = new int[] { 1, 1, 2, 3, 3, 2, 4, 6 };

		// Token: 0x04000C74 RID: 3188
		private static int[] oleDbTypeToDbTypeMap = new int[]
		{
			8, 0, 20, 12, 128, 1, 11, 3, 129, 0,
			6, 4, 7, 5, 133, 5, 134, 6, 135, 6,
			14, 7, 5, 8, 0, 13, 10, 13, 64, 6,
			72, 9, 9, 13, 13, 13, 3, 11, 205, 1,
			201, 0, 203, 16, 131, 7, 138, 13, 4, 15,
			2, 10, 16, 14, 21, 20, 19, 19, 18, 18,
			17, 2, 204, 1, 200, 0, 139, 21, 202, 16,
			12, 13, 130, 16
		};

		// Token: 0x04000C75 RID: 3189
		private static object[] sqlTypeToSqlDbTypeMap = new object[]
		{
			typeof(SqlBinary),
			SqlDbType.Binary,
			typeof(SqlInt64),
			SqlDbType.BigInt,
			typeof(SqlBoolean),
			SqlDbType.Bit,
			typeof(SqlString),
			SqlDbType.Char,
			typeof(SqlDateTime),
			SqlDbType.DateTime,
			typeof(SqlDecimal),
			SqlDbType.Decimal,
			typeof(SqlDouble),
			SqlDbType.Float,
			typeof(SqlBinary),
			SqlDbType.Image,
			typeof(SqlInt32),
			SqlDbType.Int,
			typeof(SqlMoney),
			SqlDbType.Money,
			typeof(SqlString),
			SqlDbType.NChar,
			typeof(SqlString),
			SqlDbType.NText,
			typeof(SqlString),
			SqlDbType.NVarChar,
			typeof(SqlSingle),
			SqlDbType.Real,
			typeof(SqlDateTime),
			SqlDbType.SmallDateTime,
			typeof(SqlInt16),
			SqlDbType.SmallInt,
			typeof(SqlMoney),
			SqlDbType.SmallMoney,
			typeof(object),
			SqlDbType.Variant,
			typeof(SqlString),
			SqlDbType.VarChar,
			typeof(SqlString),
			SqlDbType.Text,
			typeof(SqlBinary),
			SqlDbType.Timestamp,
			typeof(SqlByte),
			SqlDbType.TinyInt,
			typeof(SqlBinary),
			SqlDbType.VarBinary,
			typeof(SqlString),
			SqlDbType.VarChar,
			typeof(SqlGuid),
			SqlDbType.UniqueIdentifier
		};

		// Token: 0x04000C76 RID: 3190
		private static object[] sqlTypeToUrtType = new object[]
		{
			typeof(SqlBinary),
			typeof(byte[]),
			typeof(SqlByte),
			typeof(byte),
			typeof(SqlDecimal),
			typeof(decimal),
			typeof(SqlDouble),
			typeof(double),
			typeof(SqlGuid),
			typeof(Guid),
			typeof(SqlString),
			typeof(string),
			typeof(SqlSingle),
			typeof(float),
			typeof(SqlDateTime),
			typeof(DateTime),
			typeof(SqlInt16),
			typeof(short),
			typeof(SqlInt32),
			typeof(int),
			typeof(SqlInt64),
			typeof(long),
			typeof(SqlMoney),
			typeof(decimal),
			typeof(object),
			typeof(object)
		};

		// Token: 0x04000C77 RID: 3191
		private static object[] dbTypeToUrtTypeMap = new object[]
		{
			DbType.AnsiString,
			typeof(string),
			DbType.AnsiStringFixedLength,
			typeof(string),
			DbType.Binary,
			typeof(byte[]),
			DbType.Boolean,
			typeof(bool),
			DbType.Byte,
			typeof(byte),
			DbType.Currency,
			typeof(decimal),
			DbType.Date,
			typeof(DateTime),
			DbType.DateTime,
			typeof(DateTime),
			DbType.DateTime2,
			typeof(DateTime),
			DbType.DateTimeOffset,
			typeof(DateTimeOffset),
			DbType.Decimal,
			typeof(decimal),
			DbType.Double,
			typeof(double),
			DbType.Guid,
			typeof(Guid),
			DbType.Int16,
			typeof(short),
			DbType.Int32,
			typeof(int),
			DbType.Int64,
			typeof(long),
			DbType.Object,
			typeof(object),
			DbType.SByte,
			typeof(byte),
			DbType.Single,
			typeof(float),
			DbType.String,
			typeof(string),
			DbType.StringFixedLength,
			typeof(string),
			DbType.Time,
			typeof(DateTime),
			DbType.UInt16,
			typeof(ushort),
			DbType.UInt32,
			typeof(uint),
			DbType.UInt64,
			typeof(ulong),
			DbType.VarNumeric,
			typeof(decimal)
		};
	}
}
