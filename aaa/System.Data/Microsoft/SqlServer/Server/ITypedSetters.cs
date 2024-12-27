using System;
using System.Data.SqlTypes;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x0200002C RID: 44
	internal interface ITypedSetters
	{
		// Token: 0x060000D8 RID: 216
		void SetDBNull(int ordinal);

		// Token: 0x060000D9 RID: 217
		void SetBoolean(int ordinal, bool value);

		// Token: 0x060000DA RID: 218
		void SetByte(int ordinal, byte value);

		// Token: 0x060000DB RID: 219
		void SetBytes(int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length);

		// Token: 0x060000DC RID: 220
		void SetChar(int ordinal, char value);

		// Token: 0x060000DD RID: 221
		void SetChars(int ordinal, long fieldOffset, char[] buffer, int bufferOffset, int length);

		// Token: 0x060000DE RID: 222
		void SetInt16(int ordinal, short value);

		// Token: 0x060000DF RID: 223
		void SetInt32(int ordinal, int value);

		// Token: 0x060000E0 RID: 224
		void SetInt64(int ordinal, long value);

		// Token: 0x060000E1 RID: 225
		void SetFloat(int ordinal, float value);

		// Token: 0x060000E2 RID: 226
		void SetDouble(int ordinal, double value);

		// Token: 0x060000E3 RID: 227
		[Obsolete("Not supported as of SMI v2.  Will be removed when v1 support dropped.  Use setter with offset.")]
		void SetString(int ordinal, string value);

		// Token: 0x060000E4 RID: 228
		void SetString(int ordinal, string value, int offset);

		// Token: 0x060000E5 RID: 229
		void SetDecimal(int ordinal, decimal value);

		// Token: 0x060000E6 RID: 230
		void SetDateTime(int ordinal, DateTime value);

		// Token: 0x060000E7 RID: 231
		void SetGuid(int ordinal, Guid value);

		// Token: 0x060000E8 RID: 232
		void SetSqlBoolean(int ordinal, SqlBoolean value);

		// Token: 0x060000E9 RID: 233
		void SetSqlByte(int ordinal, SqlByte value);

		// Token: 0x060000EA RID: 234
		void SetSqlInt16(int ordinal, SqlInt16 value);

		// Token: 0x060000EB RID: 235
		void SetSqlInt32(int ordinal, SqlInt32 value);

		// Token: 0x060000EC RID: 236
		void SetSqlInt64(int ordinal, SqlInt64 value);

		// Token: 0x060000ED RID: 237
		void SetSqlSingle(int ordinal, SqlSingle value);

		// Token: 0x060000EE RID: 238
		void SetSqlDouble(int ordinal, SqlDouble value);

		// Token: 0x060000EF RID: 239
		void SetSqlMoney(int ordinal, SqlMoney value);

		// Token: 0x060000F0 RID: 240
		void SetSqlDateTime(int ordinal, SqlDateTime value);

		// Token: 0x060000F1 RID: 241
		void SetSqlDecimal(int ordinal, SqlDecimal value);

		// Token: 0x060000F2 RID: 242
		[Obsolete("Not supported as of SMI v2.  Will be removed when v1 support dropped.  Use setter with offset.")]
		void SetSqlString(int ordinal, SqlString value);

		// Token: 0x060000F3 RID: 243
		void SetSqlString(int ordinal, SqlString value, int offset);

		// Token: 0x060000F4 RID: 244
		[Obsolete("Not supported as of SMI v2.  Will be removed when v1 support dropped.  Use setter with offset.")]
		void SetSqlBinary(int ordinal, SqlBinary value);

		// Token: 0x060000F5 RID: 245
		void SetSqlBinary(int ordinal, SqlBinary value, int offset);

		// Token: 0x060000F6 RID: 246
		void SetSqlGuid(int ordinal, SqlGuid value);

		// Token: 0x060000F7 RID: 247
		[Obsolete("Not supported as of SMI v2.  Will be removed when v1 support dropped.  Use setter with offset.")]
		void SetSqlChars(int ordinal, SqlChars value);

		// Token: 0x060000F8 RID: 248
		void SetSqlChars(int ordinal, SqlChars value, int offset);

		// Token: 0x060000F9 RID: 249
		[Obsolete("Not supported as of SMI v2.  Will be removed when v1 support dropped.  Use setter with offset.")]
		void SetSqlBytes(int ordinal, SqlBytes value);

		// Token: 0x060000FA RID: 250
		void SetSqlBytes(int ordinal, SqlBytes value, int offset);

		// Token: 0x060000FB RID: 251
		void SetSqlXml(int ordinal, SqlXml value);
	}
}
