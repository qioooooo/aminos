using System;
using System.Data.Common;
using System.Data.SqlTypes;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x0200002E RID: 46
	internal abstract class SmiTypedGetterSetter : ITypedGettersV3, ITypedSettersV3
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600010D RID: 269
		internal abstract bool CanGet { get; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600010E RID: 270
		internal abstract bool CanSet { get; }

		// Token: 0x0600010F RID: 271 RVA: 0x001C87A4 File Offset: 0x001C7BA4
		public virtual bool IsDBNull(SmiEventSink sink, int ordinal)
		{
			if (!this.CanGet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x001C87C8 File Offset: 0x001C7BC8
		public virtual SmiMetaData GetVariantType(SmiEventSink sink, int ordinal)
		{
			if (!this.CanGet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x001C87EC File Offset: 0x001C7BEC
		public virtual bool GetBoolean(SmiEventSink sink, int ordinal)
		{
			if (!this.CanGet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x001C8810 File Offset: 0x001C7C10
		public virtual byte GetByte(SmiEventSink sink, int ordinal)
		{
			if (!this.CanGet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x001C8834 File Offset: 0x001C7C34
		public virtual long GetBytesLength(SmiEventSink sink, int ordinal)
		{
			if (!this.CanGet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x001C8858 File Offset: 0x001C7C58
		public virtual int GetBytes(SmiEventSink sink, int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length)
		{
			if (!this.CanGet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x001C887C File Offset: 0x001C7C7C
		public virtual long GetCharsLength(SmiEventSink sink, int ordinal)
		{
			if (!this.CanGet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x001C88A0 File Offset: 0x001C7CA0
		public virtual int GetChars(SmiEventSink sink, int ordinal, long fieldOffset, char[] buffer, int bufferOffset, int length)
		{
			if (!this.CanGet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x001C88C4 File Offset: 0x001C7CC4
		public virtual string GetString(SmiEventSink sink, int ordinal)
		{
			if (!this.CanGet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x001C88E8 File Offset: 0x001C7CE8
		public virtual short GetInt16(SmiEventSink sink, int ordinal)
		{
			if (!this.CanGet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x001C890C File Offset: 0x001C7D0C
		public virtual int GetInt32(SmiEventSink sink, int ordinal)
		{
			if (!this.CanGet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x001C8930 File Offset: 0x001C7D30
		public virtual long GetInt64(SmiEventSink sink, int ordinal)
		{
			if (!this.CanGet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x001C8954 File Offset: 0x001C7D54
		public virtual float GetSingle(SmiEventSink sink, int ordinal)
		{
			if (!this.CanGet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x001C8978 File Offset: 0x001C7D78
		public virtual double GetDouble(SmiEventSink sink, int ordinal)
		{
			if (!this.CanGet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x001C899C File Offset: 0x001C7D9C
		public virtual SqlDecimal GetSqlDecimal(SmiEventSink sink, int ordinal)
		{
			if (!this.CanGet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x001C89C0 File Offset: 0x001C7DC0
		public virtual DateTime GetDateTime(SmiEventSink sink, int ordinal)
		{
			if (!this.CanGet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x001C89E4 File Offset: 0x001C7DE4
		public virtual Guid GetGuid(SmiEventSink sink, int ordinal)
		{
			if (!this.CanGet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x001C8A08 File Offset: 0x001C7E08
		public virtual TimeSpan GetTimeSpan(SmiEventSink sink, int ordinal)
		{
			if (!this.CanGet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x001C8A2C File Offset: 0x001C7E2C
		public virtual DateTimeOffset GetDateTimeOffset(SmiEventSink sink, int ordinal)
		{
			if (!this.CanGet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x001C8A50 File Offset: 0x001C7E50
		internal virtual SmiTypedGetterSetter GetTypedGetterSetter(SmiEventSink sink, int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x001C8A64 File Offset: 0x001C7E64
		internal virtual bool NextElement(SmiEventSink sink)
		{
			if (!this.CanGet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x001C8A88 File Offset: 0x001C7E88
		public virtual void SetDBNull(SmiEventSink sink, int ordinal)
		{
			if (!this.CanSet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x001C8AAC File Offset: 0x001C7EAC
		public virtual void SetBoolean(SmiEventSink sink, int ordinal, bool value)
		{
			if (!this.CanSet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x001C8AD0 File Offset: 0x001C7ED0
		public virtual void SetByte(SmiEventSink sink, int ordinal, byte value)
		{
			if (!this.CanSet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x001C8AF4 File Offset: 0x001C7EF4
		public virtual int SetBytes(SmiEventSink sink, int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length)
		{
			if (!this.CanSet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x001C8B18 File Offset: 0x001C7F18
		public virtual void SetBytesLength(SmiEventSink sink, int ordinal, long length)
		{
			if (!this.CanSet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x001C8B3C File Offset: 0x001C7F3C
		public virtual int SetChars(SmiEventSink sink, int ordinal, long fieldOffset, char[] buffer, int bufferOffset, int length)
		{
			if (!this.CanSet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x001C8B60 File Offset: 0x001C7F60
		public virtual void SetCharsLength(SmiEventSink sink, int ordinal, long length)
		{
			if (!this.CanSet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x001C8B84 File Offset: 0x001C7F84
		public virtual void SetString(SmiEventSink sink, int ordinal, string value, int offset, int length)
		{
			if (!this.CanSet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x001C8BA8 File Offset: 0x001C7FA8
		public virtual void SetInt16(SmiEventSink sink, int ordinal, short value)
		{
			if (!this.CanSet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x001C8BCC File Offset: 0x001C7FCC
		public virtual void SetInt32(SmiEventSink sink, int ordinal, int value)
		{
			if (!this.CanSet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x001C8BF0 File Offset: 0x001C7FF0
		public virtual void SetInt64(SmiEventSink sink, int ordinal, long value)
		{
			if (!this.CanSet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x001C8C14 File Offset: 0x001C8014
		public virtual void SetSingle(SmiEventSink sink, int ordinal, float value)
		{
			if (!this.CanSet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x001C8C38 File Offset: 0x001C8038
		public virtual void SetDouble(SmiEventSink sink, int ordinal, double value)
		{
			if (!this.CanSet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x001C8C5C File Offset: 0x001C805C
		public virtual void SetSqlDecimal(SmiEventSink sink, int ordinal, SqlDecimal value)
		{
			if (!this.CanSet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x001C8C80 File Offset: 0x001C8080
		public virtual void SetDateTime(SmiEventSink sink, int ordinal, DateTime value)
		{
			if (!this.CanSet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x001C8CA4 File Offset: 0x001C80A4
		public virtual void SetGuid(SmiEventSink sink, int ordinal, Guid value)
		{
			if (!this.CanSet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x001C8CC8 File Offset: 0x001C80C8
		public virtual void SetTimeSpan(SmiEventSink sink, int ordinal, TimeSpan value)
		{
			if (!this.CanSet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x001C8CEC File Offset: 0x001C80EC
		public virtual void SetDateTimeOffset(SmiEventSink sink, int ordinal, DateTimeOffset value)
		{
			if (!this.CanSet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x001C8D10 File Offset: 0x001C8110
		public virtual void SetVariantMetaData(SmiEventSink sink, int ordinal, SmiMetaData metaData)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x001C8D24 File Offset: 0x001C8124
		internal virtual void NewElement(SmiEventSink sink)
		{
			if (!this.CanSet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x001C8D48 File Offset: 0x001C8148
		internal virtual void EndElements(SmiEventSink sink)
		{
			if (!this.CanSet)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidSmiCall);
			}
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}
	}
}
