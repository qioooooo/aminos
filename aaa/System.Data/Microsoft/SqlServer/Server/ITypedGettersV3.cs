using System;
using System.Data.SqlTypes;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x0200002B RID: 43
	internal interface ITypedGettersV3
	{
		// Token: 0x060000C7 RID: 199
		bool IsDBNull(SmiEventSink sink, int ordinal);

		// Token: 0x060000C8 RID: 200
		SmiMetaData GetVariantType(SmiEventSink sink, int ordinal);

		// Token: 0x060000C9 RID: 201
		bool GetBoolean(SmiEventSink sink, int ordinal);

		// Token: 0x060000CA RID: 202
		byte GetByte(SmiEventSink sink, int ordinal);

		// Token: 0x060000CB RID: 203
		long GetBytesLength(SmiEventSink sink, int ordinal);

		// Token: 0x060000CC RID: 204
		int GetBytes(SmiEventSink sink, int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length);

		// Token: 0x060000CD RID: 205
		long GetCharsLength(SmiEventSink sink, int ordinal);

		// Token: 0x060000CE RID: 206
		int GetChars(SmiEventSink sink, int ordinal, long fieldOffset, char[] buffer, int bufferOffset, int length);

		// Token: 0x060000CF RID: 207
		string GetString(SmiEventSink sink, int ordinal);

		// Token: 0x060000D0 RID: 208
		short GetInt16(SmiEventSink sink, int ordinal);

		// Token: 0x060000D1 RID: 209
		int GetInt32(SmiEventSink sink, int ordinal);

		// Token: 0x060000D2 RID: 210
		long GetInt64(SmiEventSink sink, int ordinal);

		// Token: 0x060000D3 RID: 211
		float GetSingle(SmiEventSink sink, int ordinal);

		// Token: 0x060000D4 RID: 212
		double GetDouble(SmiEventSink sink, int ordinal);

		// Token: 0x060000D5 RID: 213
		SqlDecimal GetSqlDecimal(SmiEventSink sink, int ordinal);

		// Token: 0x060000D6 RID: 214
		DateTime GetDateTime(SmiEventSink sink, int ordinal);

		// Token: 0x060000D7 RID: 215
		Guid GetGuid(SmiEventSink sink, int ordinal);
	}
}
