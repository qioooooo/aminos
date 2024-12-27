using System;
using System.Data.SqlTypes;
using System.Diagnostics;
using Microsoft.SqlServer.Server;

namespace System.Data.SqlClient
{
	// Token: 0x02000336 RID: 822
	internal class TdsRecordBufferSetter : SmiRecordBuffer
	{
		// Token: 0x06002AF8 RID: 11000 RVA: 0x0029FDC0 File Offset: 0x0029F1C0
		internal TdsRecordBufferSetter(TdsParserStateObject stateObj, SmiMetaData md)
		{
			this._fieldSetters = new TdsValueSetter[md.FieldMetaData.Count];
			for (int i = 0; i < md.FieldMetaData.Count; i++)
			{
				this._fieldSetters[i] = new TdsValueSetter(stateObj, md.FieldMetaData[i]);
			}
			this._stateObj = stateObj;
			this._metaData = md;
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06002AF9 RID: 11001 RVA: 0x0029FE28 File Offset: 0x0029F228
		internal override bool CanGet
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06002AFA RID: 11002 RVA: 0x0029FE38 File Offset: 0x0029F238
		internal override bool CanSet
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002AFB RID: 11003 RVA: 0x0029FE48 File Offset: 0x0029F248
		public override void Close(SmiEventSink eventSink)
		{
		}

		// Token: 0x06002AFC RID: 11004 RVA: 0x0029FE58 File Offset: 0x0029F258
		public override void SetDBNull(SmiEventSink sink, int ordinal)
		{
			this._fieldSetters[ordinal].SetDBNull();
		}

		// Token: 0x06002AFD RID: 11005 RVA: 0x0029FE74 File Offset: 0x0029F274
		public override void SetBoolean(SmiEventSink sink, int ordinal, bool value)
		{
			this._fieldSetters[ordinal].SetBoolean(value);
		}

		// Token: 0x06002AFE RID: 11006 RVA: 0x0029FE90 File Offset: 0x0029F290
		public override void SetByte(SmiEventSink sink, int ordinal, byte value)
		{
			this._fieldSetters[ordinal].SetByte(value);
		}

		// Token: 0x06002AFF RID: 11007 RVA: 0x0029FEAC File Offset: 0x0029F2AC
		public override int SetBytes(SmiEventSink sink, int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length)
		{
			return this._fieldSetters[ordinal].SetBytes(fieldOffset, buffer, bufferOffset, length);
		}

		// Token: 0x06002B00 RID: 11008 RVA: 0x0029FED0 File Offset: 0x0029F2D0
		public override void SetBytesLength(SmiEventSink sink, int ordinal, long length)
		{
			this._fieldSetters[ordinal].SetBytesLength(length);
		}

		// Token: 0x06002B01 RID: 11009 RVA: 0x0029FEEC File Offset: 0x0029F2EC
		public override int SetChars(SmiEventSink sink, int ordinal, long fieldOffset, char[] buffer, int bufferOffset, int length)
		{
			return this._fieldSetters[ordinal].SetChars(fieldOffset, buffer, bufferOffset, length);
		}

		// Token: 0x06002B02 RID: 11010 RVA: 0x0029FF10 File Offset: 0x0029F310
		public override void SetCharsLength(SmiEventSink sink, int ordinal, long length)
		{
			this._fieldSetters[ordinal].SetCharsLength(length);
		}

		// Token: 0x06002B03 RID: 11011 RVA: 0x0029FF2C File Offset: 0x0029F32C
		public override void SetString(SmiEventSink sink, int ordinal, string value, int offset, int length)
		{
			this._fieldSetters[ordinal].SetString(value, offset, length);
		}

		// Token: 0x06002B04 RID: 11012 RVA: 0x0029FF4C File Offset: 0x0029F34C
		public override void SetInt16(SmiEventSink sink, int ordinal, short value)
		{
			this._fieldSetters[ordinal].SetInt16(value);
		}

		// Token: 0x06002B05 RID: 11013 RVA: 0x0029FF68 File Offset: 0x0029F368
		public override void SetInt32(SmiEventSink sink, int ordinal, int value)
		{
			this._fieldSetters[ordinal].SetInt32(value);
		}

		// Token: 0x06002B06 RID: 11014 RVA: 0x0029FF84 File Offset: 0x0029F384
		public override void SetInt64(SmiEventSink sink, int ordinal, long value)
		{
			this._fieldSetters[ordinal].SetInt64(value);
		}

		// Token: 0x06002B07 RID: 11015 RVA: 0x0029FFA0 File Offset: 0x0029F3A0
		public override void SetSingle(SmiEventSink sink, int ordinal, float value)
		{
			this._fieldSetters[ordinal].SetSingle(value);
		}

		// Token: 0x06002B08 RID: 11016 RVA: 0x0029FFBC File Offset: 0x0029F3BC
		public override void SetDouble(SmiEventSink sink, int ordinal, double value)
		{
			this._fieldSetters[ordinal].SetDouble(value);
		}

		// Token: 0x06002B09 RID: 11017 RVA: 0x0029FFD8 File Offset: 0x0029F3D8
		public override void SetSqlDecimal(SmiEventSink sink, int ordinal, SqlDecimal value)
		{
			this._fieldSetters[ordinal].SetSqlDecimal(value);
		}

		// Token: 0x06002B0A RID: 11018 RVA: 0x0029FFF4 File Offset: 0x0029F3F4
		public override void SetDateTime(SmiEventSink sink, int ordinal, DateTime value)
		{
			this._fieldSetters[ordinal].SetDateTime(value);
		}

		// Token: 0x06002B0B RID: 11019 RVA: 0x002A0010 File Offset: 0x0029F410
		public override void SetGuid(SmiEventSink sink, int ordinal, Guid value)
		{
			this._fieldSetters[ordinal].SetGuid(value);
		}

		// Token: 0x06002B0C RID: 11020 RVA: 0x002A002C File Offset: 0x0029F42C
		public override void SetTimeSpan(SmiEventSink sink, int ordinal, TimeSpan value)
		{
			this._fieldSetters[ordinal].SetTimeSpan(value);
		}

		// Token: 0x06002B0D RID: 11021 RVA: 0x002A0048 File Offset: 0x0029F448
		public override void SetDateTimeOffset(SmiEventSink sink, int ordinal, DateTimeOffset value)
		{
			this._fieldSetters[ordinal].SetDateTimeOffset(value);
		}

		// Token: 0x06002B0E RID: 11022 RVA: 0x002A0064 File Offset: 0x0029F464
		public override void SetVariantMetaData(SmiEventSink sink, int ordinal, SmiMetaData metaData)
		{
			this._fieldSetters[ordinal].SetVariantType(metaData);
		}

		// Token: 0x06002B0F RID: 11023 RVA: 0x002A0080 File Offset: 0x0029F480
		internal override void NewElement(SmiEventSink sink)
		{
			this._stateObj.Parser.WriteByte(1, this._stateObj);
		}

		// Token: 0x06002B10 RID: 11024 RVA: 0x002A00A4 File Offset: 0x0029F4A4
		internal override void EndElements(SmiEventSink sink)
		{
			this._stateObj.Parser.WriteByte(0, this._stateObj);
		}

		// Token: 0x06002B11 RID: 11025 RVA: 0x002A00C8 File Offset: 0x0029F4C8
		[Conditional("DEBUG")]
		private void CheckWritingToColumn(int ordinal)
		{
		}

		// Token: 0x06002B12 RID: 11026 RVA: 0x002A00D8 File Offset: 0x0029F4D8
		[Conditional("DEBUG")]
		private void SkipPossibleDefaultedColumns(int targetColumn)
		{
		}

		// Token: 0x06002B13 RID: 11027 RVA: 0x002A00E8 File Offset: 0x0029F4E8
		[Conditional("DEBUG")]
		internal void CheckSettingColumn(int ordinal)
		{
		}

		// Token: 0x04001C36 RID: 7222
		private TdsValueSetter[] _fieldSetters;

		// Token: 0x04001C37 RID: 7223
		private TdsParserStateObject _stateObj;

		// Token: 0x04001C38 RID: 7224
		private SmiMetaData _metaData;
	}
}
