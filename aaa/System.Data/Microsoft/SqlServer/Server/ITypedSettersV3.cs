using System;
using System.Data.SqlTypes;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x0200002D RID: 45
	internal interface ITypedSettersV3
	{
		// Token: 0x060000FC RID: 252
		void SetVariantMetaData(SmiEventSink sink, int ordinal, SmiMetaData metaData);

		// Token: 0x060000FD RID: 253
		void SetDBNull(SmiEventSink sink, int ordinal);

		// Token: 0x060000FE RID: 254
		void SetBoolean(SmiEventSink sink, int ordinal, bool value);

		// Token: 0x060000FF RID: 255
		void SetByte(SmiEventSink sink, int ordinal, byte value);

		// Token: 0x06000100 RID: 256
		int SetBytes(SmiEventSink sink, int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length);

		// Token: 0x06000101 RID: 257
		void SetBytesLength(SmiEventSink sink, int ordinal, long length);

		// Token: 0x06000102 RID: 258
		int SetChars(SmiEventSink sink, int ordinal, long fieldOffset, char[] buffer, int bufferOffset, int length);

		// Token: 0x06000103 RID: 259
		void SetCharsLength(SmiEventSink sink, int ordinal, long length);

		// Token: 0x06000104 RID: 260
		void SetString(SmiEventSink sink, int ordinal, string value, int offset, int length);

		// Token: 0x06000105 RID: 261
		void SetInt16(SmiEventSink sink, int ordinal, short value);

		// Token: 0x06000106 RID: 262
		void SetInt32(SmiEventSink sink, int ordinal, int value);

		// Token: 0x06000107 RID: 263
		void SetInt64(SmiEventSink sink, int ordinal, long value);

		// Token: 0x06000108 RID: 264
		void SetSingle(SmiEventSink sink, int ordinal, float value);

		// Token: 0x06000109 RID: 265
		void SetDouble(SmiEventSink sink, int ordinal, double value);

		// Token: 0x0600010A RID: 266
		void SetSqlDecimal(SmiEventSink sink, int ordinal, SqlDecimal value);

		// Token: 0x0600010B RID: 267
		void SetDateTime(SmiEventSink sink, int ordinal, DateTime value);

		// Token: 0x0600010C RID: 268
		void SetGuid(SmiEventSink sink, int ordinal, Guid value);
	}
}
