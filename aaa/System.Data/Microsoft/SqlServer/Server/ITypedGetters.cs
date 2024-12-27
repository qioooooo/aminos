using System;
using System.Data;
using System.Data.SqlTypes;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x0200002A RID: 42
	internal interface ITypedGetters
	{
		// Token: 0x060000A4 RID: 164
		bool IsDBNull(int ordinal);

		// Token: 0x060000A5 RID: 165
		SqlDbType GetVariantType(int ordinal);

		// Token: 0x060000A6 RID: 166
		bool GetBoolean(int ordinal);

		// Token: 0x060000A7 RID: 167
		byte GetByte(int ordinal);

		// Token: 0x060000A8 RID: 168
		long GetBytes(int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length);

		// Token: 0x060000A9 RID: 169
		char GetChar(int ordinal);

		// Token: 0x060000AA RID: 170
		long GetChars(int ordinal, long fieldOffset, char[] buffer, int bufferOffset, int length);

		// Token: 0x060000AB RID: 171
		short GetInt16(int ordinal);

		// Token: 0x060000AC RID: 172
		int GetInt32(int ordinal);

		// Token: 0x060000AD RID: 173
		long GetInt64(int ordinal);

		// Token: 0x060000AE RID: 174
		float GetFloat(int ordinal);

		// Token: 0x060000AF RID: 175
		double GetDouble(int ordinal);

		// Token: 0x060000B0 RID: 176
		string GetString(int ordinal);

		// Token: 0x060000B1 RID: 177
		decimal GetDecimal(int ordinal);

		// Token: 0x060000B2 RID: 178
		DateTime GetDateTime(int ordinal);

		// Token: 0x060000B3 RID: 179
		Guid GetGuid(int ordinal);

		// Token: 0x060000B4 RID: 180
		SqlBoolean GetSqlBoolean(int ordinal);

		// Token: 0x060000B5 RID: 181
		SqlByte GetSqlByte(int ordinal);

		// Token: 0x060000B6 RID: 182
		SqlInt16 GetSqlInt16(int ordinal);

		// Token: 0x060000B7 RID: 183
		SqlInt32 GetSqlInt32(int ordinal);

		// Token: 0x060000B8 RID: 184
		SqlInt64 GetSqlInt64(int ordinal);

		// Token: 0x060000B9 RID: 185
		SqlSingle GetSqlSingle(int ordinal);

		// Token: 0x060000BA RID: 186
		SqlDouble GetSqlDouble(int ordinal);

		// Token: 0x060000BB RID: 187
		SqlMoney GetSqlMoney(int ordinal);

		// Token: 0x060000BC RID: 188
		SqlDateTime GetSqlDateTime(int ordinal);

		// Token: 0x060000BD RID: 189
		SqlDecimal GetSqlDecimal(int ordinal);

		// Token: 0x060000BE RID: 190
		SqlString GetSqlString(int ordinal);

		// Token: 0x060000BF RID: 191
		SqlBinary GetSqlBinary(int ordinal);

		// Token: 0x060000C0 RID: 192
		SqlGuid GetSqlGuid(int ordinal);

		// Token: 0x060000C1 RID: 193
		SqlChars GetSqlChars(int ordinal);

		// Token: 0x060000C2 RID: 194
		SqlBytes GetSqlBytes(int ordinal);

		// Token: 0x060000C3 RID: 195
		SqlXml GetSqlXml(int ordinal);

		// Token: 0x060000C4 RID: 196
		SqlBytes GetSqlBytesRef(int ordinal);

		// Token: 0x060000C5 RID: 197
		SqlChars GetSqlCharsRef(int ordinal);

		// Token: 0x060000C6 RID: 198
		SqlXml GetSqlXmlRef(int ordinal);
	}
}
