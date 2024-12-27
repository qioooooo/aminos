using System;
using System.Data.SqlTypes;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000030 RID: 48
	internal sealed class MemoryRecordBuffer : SmiRecordBuffer
	{
		// Token: 0x06000186 RID: 390 RVA: 0x001C9368 File Offset: 0x001C8768
		internal MemoryRecordBuffer(SmiMetaData[] metaData)
		{
			this._buffer = new SqlRecordBuffer[metaData.Length];
			for (int i = 0; i < this._buffer.Length; i++)
			{
				this._buffer[i] = new SqlRecordBuffer(metaData[i]);
			}
		}

		// Token: 0x06000187 RID: 391 RVA: 0x001C93AC File Offset: 0x001C87AC
		public override bool IsDBNull(SmiEventSink sink, int ordinal)
		{
			return this._buffer[ordinal].IsNull;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x001C93C8 File Offset: 0x001C87C8
		public override SmiMetaData GetVariantType(SmiEventSink sink, int ordinal)
		{
			return this._buffer[ordinal].VariantType;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x001C93E4 File Offset: 0x001C87E4
		public override bool GetBoolean(SmiEventSink sink, int ordinal)
		{
			return this._buffer[ordinal].Boolean;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x001C9400 File Offset: 0x001C8800
		public override byte GetByte(SmiEventSink sink, int ordinal)
		{
			return this._buffer[ordinal].Byte;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x001C941C File Offset: 0x001C881C
		public override long GetBytesLength(SmiEventSink sink, int ordinal)
		{
			return this._buffer[ordinal].BytesLength;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x001C9438 File Offset: 0x001C8838
		public override int GetBytes(SmiEventSink sink, int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length)
		{
			return this._buffer[ordinal].GetBytes(fieldOffset, buffer, bufferOffset, length);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x001C945C File Offset: 0x001C885C
		public override long GetCharsLength(SmiEventSink sink, int ordinal)
		{
			return this._buffer[ordinal].CharsLength;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x001C9478 File Offset: 0x001C8878
		public override int GetChars(SmiEventSink sink, int ordinal, long fieldOffset, char[] buffer, int bufferOffset, int length)
		{
			return this._buffer[ordinal].GetChars(fieldOffset, buffer, bufferOffset, length);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x001C949C File Offset: 0x001C889C
		public override string GetString(SmiEventSink sink, int ordinal)
		{
			return this._buffer[ordinal].String;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x001C94B8 File Offset: 0x001C88B8
		public override short GetInt16(SmiEventSink sink, int ordinal)
		{
			return this._buffer[ordinal].Int16;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x001C94D4 File Offset: 0x001C88D4
		public override int GetInt32(SmiEventSink sink, int ordinal)
		{
			return this._buffer[ordinal].Int32;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x001C94F0 File Offset: 0x001C88F0
		public override long GetInt64(SmiEventSink sink, int ordinal)
		{
			return this._buffer[ordinal].Int64;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x001C950C File Offset: 0x001C890C
		public override float GetSingle(SmiEventSink sink, int ordinal)
		{
			return this._buffer[ordinal].Single;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x001C9528 File Offset: 0x001C8928
		public override double GetDouble(SmiEventSink sink, int ordinal)
		{
			return this._buffer[ordinal].Double;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x001C9544 File Offset: 0x001C8944
		public override SqlDecimal GetSqlDecimal(SmiEventSink sink, int ordinal)
		{
			return this._buffer[ordinal].SqlDecimal;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x001C9560 File Offset: 0x001C8960
		public override DateTime GetDateTime(SmiEventSink sink, int ordinal)
		{
			return this._buffer[ordinal].DateTime;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x001C957C File Offset: 0x001C897C
		public override Guid GetGuid(SmiEventSink sink, int ordinal)
		{
			return this._buffer[ordinal].Guid;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x001C9598 File Offset: 0x001C8998
		public override TimeSpan GetTimeSpan(SmiEventSink sink, int ordinal)
		{
			return this._buffer[ordinal].TimeSpan;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x001C95B4 File Offset: 0x001C89B4
		public override DateTimeOffset GetDateTimeOffset(SmiEventSink sink, int ordinal)
		{
			return this._buffer[ordinal].DateTimeOffset;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x001C95D0 File Offset: 0x001C89D0
		public override void SetDBNull(SmiEventSink sink, int ordinal)
		{
			this._buffer[ordinal].SetNull();
		}

		// Token: 0x0600019B RID: 411 RVA: 0x001C95EC File Offset: 0x001C89EC
		public override void SetBoolean(SmiEventSink sink, int ordinal, bool value)
		{
			this._buffer[ordinal].Boolean = value;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x001C9608 File Offset: 0x001C8A08
		public override void SetByte(SmiEventSink sink, int ordinal, byte value)
		{
			this._buffer[ordinal].Byte = value;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x001C9624 File Offset: 0x001C8A24
		public override int SetBytes(SmiEventSink sink, int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length)
		{
			return this._buffer[ordinal].SetBytes(fieldOffset, buffer, bufferOffset, length);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x001C9648 File Offset: 0x001C8A48
		public override void SetBytesLength(SmiEventSink sink, int ordinal, long length)
		{
			this._buffer[ordinal].BytesLength = length;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x001C9664 File Offset: 0x001C8A64
		public override int SetChars(SmiEventSink sink, int ordinal, long fieldOffset, char[] buffer, int bufferOffset, int length)
		{
			return this._buffer[ordinal].SetChars(fieldOffset, buffer, bufferOffset, length);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x001C9688 File Offset: 0x001C8A88
		public override void SetCharsLength(SmiEventSink sink, int ordinal, long length)
		{
			this._buffer[ordinal].CharsLength = length;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x001C96A4 File Offset: 0x001C8AA4
		public override void SetString(SmiEventSink sink, int ordinal, string value, int offset, int length)
		{
			this._buffer[ordinal].String = value.Substring(offset, length);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x001C96C8 File Offset: 0x001C8AC8
		public override void SetInt16(SmiEventSink sink, int ordinal, short value)
		{
			this._buffer[ordinal].Int16 = value;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x001C96E4 File Offset: 0x001C8AE4
		public override void SetInt32(SmiEventSink sink, int ordinal, int value)
		{
			this._buffer[ordinal].Int32 = value;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x001C9700 File Offset: 0x001C8B00
		public override void SetInt64(SmiEventSink sink, int ordinal, long value)
		{
			this._buffer[ordinal].Int64 = value;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x001C971C File Offset: 0x001C8B1C
		public override void SetSingle(SmiEventSink sink, int ordinal, float value)
		{
			this._buffer[ordinal].Single = value;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x001C9738 File Offset: 0x001C8B38
		public override void SetDouble(SmiEventSink sink, int ordinal, double value)
		{
			this._buffer[ordinal].Double = value;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x001C9754 File Offset: 0x001C8B54
		public override void SetSqlDecimal(SmiEventSink sink, int ordinal, SqlDecimal value)
		{
			this._buffer[ordinal].SqlDecimal = value;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x001C9770 File Offset: 0x001C8B70
		public override void SetDateTime(SmiEventSink sink, int ordinal, DateTime value)
		{
			this._buffer[ordinal].DateTime = value;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x001C978C File Offset: 0x001C8B8C
		public override void SetGuid(SmiEventSink sink, int ordinal, Guid value)
		{
			this._buffer[ordinal].Guid = value;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x001C97A8 File Offset: 0x001C8BA8
		public override void SetTimeSpan(SmiEventSink sink, int ordinal, TimeSpan value)
		{
			this._buffer[ordinal].TimeSpan = value;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x001C97C4 File Offset: 0x001C8BC4
		public override void SetDateTimeOffset(SmiEventSink sink, int ordinal, DateTimeOffset value)
		{
			this._buffer[ordinal].DateTimeOffset = value;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x001C97E0 File Offset: 0x001C8BE0
		public override void SetVariantMetaData(SmiEventSink sink, int ordinal, SmiMetaData metaData)
		{
			this._buffer[ordinal].VariantType = metaData;
		}

		// Token: 0x04000561 RID: 1377
		private SqlRecordBuffer[] _buffer;
	}
}
