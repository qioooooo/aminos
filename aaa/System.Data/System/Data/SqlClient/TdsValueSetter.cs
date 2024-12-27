using System;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Text;
using Microsoft.SqlServer.Server;

namespace System.Data.SqlClient
{
	// Token: 0x02000337 RID: 823
	internal class TdsValueSetter
	{
		// Token: 0x06002B14 RID: 11028 RVA: 0x002A00F8 File Offset: 0x0029F4F8
		internal TdsValueSetter(TdsParserStateObject stateObj, SmiMetaData md)
		{
			this._stateObj = stateObj;
			this._metaData = md;
			this._isPlp = MetaDataUtilsSmi.IsPlpFormat(md);
			this._plpUnknownSent = false;
			this._encoder = null;
		}

		// Token: 0x06002B15 RID: 11029 RVA: 0x002A0134 File Offset: 0x0029F534
		internal void SetDBNull()
		{
			if (this._isPlp)
			{
				this._stateObj.Parser.WriteUnsignedLong(ulong.MaxValue, this._stateObj);
				return;
			}
			switch (this._metaData.SqlDbType)
			{
			case SqlDbType.BigInt:
			case SqlDbType.Bit:
			case SqlDbType.DateTime:
			case SqlDbType.Decimal:
			case SqlDbType.Float:
			case SqlDbType.Int:
			case SqlDbType.Money:
			case SqlDbType.Real:
			case SqlDbType.UniqueIdentifier:
			case SqlDbType.SmallDateTime:
			case SqlDbType.SmallInt:
			case SqlDbType.SmallMoney:
			case SqlDbType.TinyInt:
			case SqlDbType.Date:
			case SqlDbType.Time:
			case SqlDbType.DateTime2:
			case SqlDbType.DateTimeOffset:
				this._stateObj.Parser.WriteByte(0, this._stateObj);
				return;
			case SqlDbType.Binary:
			case SqlDbType.Char:
			case SqlDbType.Image:
			case SqlDbType.NChar:
			case SqlDbType.NText:
			case SqlDbType.NVarChar:
			case SqlDbType.Text:
			case SqlDbType.Timestamp:
			case SqlDbType.VarBinary:
			case SqlDbType.VarChar:
				this._stateObj.Parser.WriteShort(65535, this._stateObj);
				return;
			case SqlDbType.Variant:
				this._stateObj.Parser.WriteInt(0, this._stateObj);
				break;
			case (SqlDbType)24:
			case SqlDbType.Xml:
			case (SqlDbType)26:
			case (SqlDbType)27:
			case (SqlDbType)28:
			case SqlDbType.Udt:
			case SqlDbType.Structured:
				break;
			default:
				return;
			}
		}

		// Token: 0x06002B16 RID: 11030 RVA: 0x002A024C File Offset: 0x0029F64C
		internal void SetBoolean(bool value)
		{
			if (SqlDbType.Variant == this._metaData.SqlDbType)
			{
				this._stateObj.Parser.WriteSqlVariantHeader(3, 50, 0, this._stateObj);
			}
			else
			{
				this._stateObj.Parser.WriteByte((byte)this._metaData.MaxLength, this._stateObj);
			}
			if (value)
			{
				this._stateObj.Parser.WriteByte(1, this._stateObj);
				return;
			}
			this._stateObj.Parser.WriteByte(0, this._stateObj);
		}

		// Token: 0x06002B17 RID: 11031 RVA: 0x002A02D8 File Offset: 0x0029F6D8
		internal void SetByte(byte value)
		{
			if (SqlDbType.Variant == this._metaData.SqlDbType)
			{
				this._stateObj.Parser.WriteSqlVariantHeader(3, 48, 0, this._stateObj);
			}
			else
			{
				this._stateObj.Parser.WriteByte((byte)this._metaData.MaxLength, this._stateObj);
			}
			this._stateObj.Parser.WriteByte(value, this._stateObj);
		}

		// Token: 0x06002B18 RID: 11032 RVA: 0x002A034C File Offset: 0x0029F74C
		internal int SetBytes(long fieldOffset, byte[] buffer, int bufferOffset, int length)
		{
			this.SetBytesNoOffsetHandling(fieldOffset, buffer, bufferOffset, length);
			return length;
		}

		// Token: 0x06002B19 RID: 11033 RVA: 0x002A0368 File Offset: 0x0029F768
		private void SetBytesNoOffsetHandling(long fieldOffset, byte[] buffer, int bufferOffset, int length)
		{
			if (this._isPlp)
			{
				if (!this._plpUnknownSent)
				{
					this._stateObj.Parser.WriteUnsignedLong(18446744073709551614UL, this._stateObj);
					this._plpUnknownSent = true;
				}
				this._stateObj.Parser.WriteInt(length, this._stateObj);
				this._stateObj.Parser.WriteByteArray(buffer, length, bufferOffset, this._stateObj);
				return;
			}
			if (SqlDbType.Variant == this._metaData.SqlDbType)
			{
				this._stateObj.Parser.WriteSqlVariantHeader(4 + length, 165, 2, this._stateObj);
			}
			this._stateObj.Parser.WriteShort(length, this._stateObj);
			this._stateObj.Parser.WriteByteArray(buffer, length, bufferOffset, this._stateObj);
		}

		// Token: 0x06002B1A RID: 11034 RVA: 0x002A043C File Offset: 0x0029F83C
		internal void SetBytesLength(long length)
		{
			if (0L == length)
			{
				if (this._isPlp)
				{
					this._stateObj.Parser.WriteLong(0L, this._stateObj);
					this._plpUnknownSent = true;
				}
				else
				{
					if (SqlDbType.Variant == this._metaData.SqlDbType)
					{
						this._stateObj.Parser.WriteSqlVariantHeader(4, 165, 2, this._stateObj);
					}
					this._stateObj.Parser.WriteShort(0, this._stateObj);
				}
			}
			if (this._plpUnknownSent)
			{
				this._stateObj.Parser.WriteInt(0, this._stateObj);
				this._plpUnknownSent = false;
			}
		}

		// Token: 0x06002B1B RID: 11035 RVA: 0x002A04E0 File Offset: 0x0029F8E0
		internal int SetChars(long fieldOffset, char[] buffer, int bufferOffset, int length)
		{
			if (MetaDataUtilsSmi.IsAnsiType(this._metaData.SqlDbType))
			{
				if (this._encoder == null)
				{
					this._encoder = this._stateObj.Parser._defaultEncoding.GetEncoder();
				}
				byte[] array = new byte[this._encoder.GetByteCount(buffer, bufferOffset, length, false)];
				this._encoder.GetBytes(buffer, bufferOffset, length, array, 0, false);
				this.SetBytesNoOffsetHandling(fieldOffset, array, 0, array.Length);
			}
			else if (this._isPlp)
			{
				if (!this._plpUnknownSent)
				{
					this._stateObj.Parser.WriteUnsignedLong(18446744073709551614UL, this._stateObj);
					this._plpUnknownSent = true;
				}
				this._stateObj.Parser.WriteInt(length * ADP.CharSize, this._stateObj);
				this._stateObj.Parser.WriteCharArray(buffer, length, bufferOffset, this._stateObj);
			}
			else if (SqlDbType.Variant == this._metaData.SqlDbType)
			{
				this._stateObj.Parser.WriteSqlVariantValue(new string(buffer, bufferOffset, length), length, 0, this._stateObj);
			}
			else
			{
				this._stateObj.Parser.WriteShort(length * ADP.CharSize, this._stateObj);
				this._stateObj.Parser.WriteCharArray(buffer, length, bufferOffset, this._stateObj);
			}
			return length;
		}

		// Token: 0x06002B1C RID: 11036 RVA: 0x002A0634 File Offset: 0x0029FA34
		internal void SetCharsLength(long length)
		{
			if (0L == length)
			{
				if (this._isPlp)
				{
					this._stateObj.Parser.WriteLong(0L, this._stateObj);
					this._plpUnknownSent = true;
				}
				else
				{
					this._stateObj.Parser.WriteShort(0, this._stateObj);
				}
			}
			if (this._plpUnknownSent)
			{
				this._stateObj.Parser.WriteInt(0, this._stateObj);
				this._plpUnknownSent = false;
			}
			this._encoder = null;
		}

		// Token: 0x06002B1D RID: 11037 RVA: 0x002A06B4 File Offset: 0x0029FAB4
		internal void SetString(string value, int offset, int length)
		{
			if (MetaDataUtilsSmi.IsAnsiType(this._metaData.SqlDbType))
			{
				byte[] array;
				if (offset == 0 && value.Length <= length)
				{
					array = this._stateObj.Parser._defaultEncoding.GetBytes(value);
				}
				else
				{
					char[] array2 = value.ToCharArray(offset, length);
					array = this._stateObj.Parser._defaultEncoding.GetBytes(array2);
				}
				this.SetBytes(0L, array, 0, array.Length);
				this.SetBytesLength((long)array.Length);
				return;
			}
			if (SqlDbType.Variant == this._metaData.SqlDbType)
			{
				SqlCollation sqlCollation = new SqlCollation();
				sqlCollation.LCID = checked((int)this._variantType.LocaleId);
				sqlCollation.SqlCompareOptions = this._variantType.CompareOptions;
				if (length * ADP.CharSize > 8000)
				{
					byte[] array3;
					if (offset == 0 && value.Length <= length)
					{
						array3 = this._stateObj.Parser._defaultEncoding.GetBytes(value);
					}
					else
					{
						array3 = this._stateObj.Parser._defaultEncoding.GetBytes(value.ToCharArray(offset, length));
					}
					this._stateObj.Parser.WriteSqlVariantHeader(9 + array3.Length, 167, 7, this._stateObj);
					this._stateObj.Parser.WriteUnsignedInt(sqlCollation.info, this._stateObj);
					this._stateObj.Parser.WriteByte(sqlCollation.sortId, this._stateObj);
					this._stateObj.Parser.WriteShort(array3.Length, this._stateObj);
					this._stateObj.Parser.WriteByteArray(array3, array3.Length, 0, this._stateObj);
				}
				else
				{
					this._stateObj.Parser.WriteSqlVariantHeader(9 + length * ADP.CharSize, 231, 7, this._stateObj);
					this._stateObj.Parser.WriteUnsignedInt(sqlCollation.info, this._stateObj);
					this._stateObj.Parser.WriteByte(sqlCollation.sortId, this._stateObj);
					this._stateObj.Parser.WriteShort(length * ADP.CharSize, this._stateObj);
					this._stateObj.Parser.WriteString(value, length, offset, this._stateObj);
				}
				this._variantType = null;
				return;
			}
			if (this._isPlp)
			{
				this._stateObj.Parser.WriteLong((long)(length * ADP.CharSize), this._stateObj);
				this._stateObj.Parser.WriteInt(length * ADP.CharSize, this._stateObj);
				this._stateObj.Parser.WriteString(value, length, offset, this._stateObj);
				if (length != 0)
				{
					this._stateObj.Parser.WriteInt(0, this._stateObj);
					return;
				}
			}
			else
			{
				this._stateObj.Parser.WriteShort(length * ADP.CharSize, this._stateObj);
				this._stateObj.Parser.WriteString(value, length, offset, this._stateObj);
			}
		}

		// Token: 0x06002B1E RID: 11038 RVA: 0x002A099C File Offset: 0x0029FD9C
		internal void SetInt16(short value)
		{
			if (SqlDbType.Variant == this._metaData.SqlDbType)
			{
				this._stateObj.Parser.WriteSqlVariantHeader(4, 52, 0, this._stateObj);
			}
			else
			{
				this._stateObj.Parser.WriteByte((byte)this._metaData.MaxLength, this._stateObj);
			}
			this._stateObj.Parser.WriteShort((int)value, this._stateObj);
		}

		// Token: 0x06002B1F RID: 11039 RVA: 0x002A0A10 File Offset: 0x0029FE10
		internal void SetInt32(int value)
		{
			if (SqlDbType.Variant == this._metaData.SqlDbType)
			{
				this._stateObj.Parser.WriteSqlVariantHeader(6, 56, 0, this._stateObj);
			}
			else
			{
				this._stateObj.Parser.WriteByte((byte)this._metaData.MaxLength, this._stateObj);
			}
			this._stateObj.Parser.WriteInt(value, this._stateObj);
		}

		// Token: 0x06002B20 RID: 11040 RVA: 0x002A0A84 File Offset: 0x0029FE84
		internal void SetInt64(long value)
		{
			if (SqlDbType.Variant == this._metaData.SqlDbType)
			{
				if (this._variantType == null)
				{
					this._stateObj.Parser.WriteSqlVariantHeader(10, 127, 0, this._stateObj);
					this._stateObj.Parser.WriteLong(value, this._stateObj);
					return;
				}
				this._stateObj.Parser.WriteSqlVariantHeader(10, 60, 0, this._stateObj);
				this._stateObj.Parser.WriteInt((int)(value >> 32), this._stateObj);
				this._stateObj.Parser.WriteInt((int)value, this._stateObj);
				this._variantType = null;
				return;
			}
			else
			{
				this._stateObj.Parser.WriteByte((byte)this._metaData.MaxLength, this._stateObj);
				if (SqlDbType.SmallMoney == this._metaData.SqlDbType)
				{
					this._stateObj.Parser.WriteInt((int)value, this._stateObj);
					return;
				}
				if (SqlDbType.Money == this._metaData.SqlDbType)
				{
					this._stateObj.Parser.WriteInt((int)(value >> 32), this._stateObj);
					this._stateObj.Parser.WriteInt((int)value, this._stateObj);
					return;
				}
				this._stateObj.Parser.WriteLong(value, this._stateObj);
				return;
			}
		}

		// Token: 0x06002B21 RID: 11041 RVA: 0x002A0BD8 File Offset: 0x0029FFD8
		internal void SetSingle(float value)
		{
			if (SqlDbType.Variant == this._metaData.SqlDbType)
			{
				this._stateObj.Parser.WriteSqlVariantHeader(6, 59, 0, this._stateObj);
			}
			else
			{
				this._stateObj.Parser.WriteByte((byte)this._metaData.MaxLength, this._stateObj);
			}
			this._stateObj.Parser.WriteFloat(value, this._stateObj);
		}

		// Token: 0x06002B22 RID: 11042 RVA: 0x002A0C4C File Offset: 0x002A004C
		internal void SetDouble(double value)
		{
			if (SqlDbType.Variant == this._metaData.SqlDbType)
			{
				this._stateObj.Parser.WriteSqlVariantHeader(10, 62, 0, this._stateObj);
			}
			else
			{
				this._stateObj.Parser.WriteByte((byte)this._metaData.MaxLength, this._stateObj);
			}
			this._stateObj.Parser.WriteDouble(value, this._stateObj);
		}

		// Token: 0x06002B23 RID: 11043 RVA: 0x002A0CC0 File Offset: 0x002A00C0
		internal void SetSqlDecimal(SqlDecimal value)
		{
			if (SqlDbType.Variant == this._metaData.SqlDbType)
			{
				this._stateObj.Parser.WriteSqlVariantHeader(21, 108, 2, this._stateObj);
				this._stateObj.Parser.WriteByte(value.Precision, this._stateObj);
				this._stateObj.Parser.WriteByte(value.Scale, this._stateObj);
				this._stateObj.Parser.WriteSqlDecimal(value, this._stateObj);
				return;
			}
			this._stateObj.Parser.WriteByte(checked((byte)MetaType.MetaDecimal.FixedLength), this._stateObj);
			this._stateObj.Parser.WriteSqlDecimal(SqlDecimal.ConvertToPrecScale(value, (int)this._metaData.Precision, (int)this._metaData.Scale), this._stateObj);
		}

		// Token: 0x06002B24 RID: 11044 RVA: 0x002A0D9C File Offset: 0x002A019C
		internal void SetDateTime(DateTime value)
		{
			if (SqlDbType.Variant == this._metaData.SqlDbType)
			{
				TdsDateTime tdsDateTime = MetaType.FromDateTime(value, 8);
				this._stateObj.Parser.WriteSqlVariantHeader(10, 61, 0, this._stateObj);
				this._stateObj.Parser.WriteInt(tdsDateTime.days, this._stateObj);
				this._stateObj.Parser.WriteInt(tdsDateTime.time, this._stateObj);
				return;
			}
			this._stateObj.Parser.WriteByte((byte)this._metaData.MaxLength, this._stateObj);
			if (SqlDbType.SmallDateTime == this._metaData.SqlDbType)
			{
				TdsDateTime tdsDateTime2 = MetaType.FromDateTime(value, (byte)this._metaData.MaxLength);
				this._stateObj.Parser.WriteShort(tdsDateTime2.days, this._stateObj);
				this._stateObj.Parser.WriteShort(tdsDateTime2.time, this._stateObj);
				return;
			}
			if (SqlDbType.DateTime == this._metaData.SqlDbType)
			{
				TdsDateTime tdsDateTime3 = MetaType.FromDateTime(value, (byte)this._metaData.MaxLength);
				this._stateObj.Parser.WriteInt(tdsDateTime3.days, this._stateObj);
				this._stateObj.Parser.WriteInt(tdsDateTime3.time, this._stateObj);
				return;
			}
			int days = value.Subtract(DateTime.MinValue).Days;
			if (SqlDbType.DateTime2 == this._metaData.SqlDbType)
			{
				long num = value.TimeOfDay.Ticks / TdsEnums.TICKS_FROM_SCALE[(int)this._metaData.Scale];
				this._stateObj.Parser.WriteByteArray(BitConverter.GetBytes(num), (int)this._metaData.MaxLength - 3, 0, this._stateObj);
			}
			this._stateObj.Parser.WriteByteArray(BitConverter.GetBytes(days), 3, 0, this._stateObj);
		}

		// Token: 0x06002B25 RID: 11045 RVA: 0x002A0F84 File Offset: 0x002A0384
		internal void SetGuid(Guid value)
		{
			byte[] array = value.ToByteArray();
			if (SqlDbType.Variant == this._metaData.SqlDbType)
			{
				this._stateObj.Parser.WriteSqlVariantHeader(18, 36, 0, this._stateObj);
			}
			else
			{
				this._stateObj.Parser.WriteByte((byte)this._metaData.MaxLength, this._stateObj);
			}
			this._stateObj.Parser.WriteByteArray(array, array.Length, 0, this._stateObj);
		}

		// Token: 0x06002B26 RID: 11046 RVA: 0x002A1004 File Offset: 0x002A0404
		internal void SetTimeSpan(TimeSpan value)
		{
			byte b;
			byte b2;
			if (SqlDbType.Variant == this._metaData.SqlDbType)
			{
				b = MetaType.MetaTime.Scale;
				b2 = (byte)MetaType.MetaTime.FixedLength;
				this._stateObj.Parser.WriteSqlVariantHeader(8, 41, 1, this._stateObj);
				this._stateObj.Parser.WriteByte(b, this._stateObj);
			}
			else
			{
				b = this._metaData.Scale;
				b2 = (byte)this._metaData.MaxLength;
				this._stateObj.Parser.WriteByte(b2, this._stateObj);
			}
			long num = value.Ticks / TdsEnums.TICKS_FROM_SCALE[(int)b];
			this._stateObj.Parser.WriteByteArray(BitConverter.GetBytes(num), (int)b2, 0, this._stateObj);
		}

		// Token: 0x06002B27 RID: 11047 RVA: 0x002A10C8 File Offset: 0x002A04C8
		internal void SetDateTimeOffset(DateTimeOffset value)
		{
			byte b;
			byte b2;
			if (SqlDbType.Variant == this._metaData.SqlDbType)
			{
				b = MetaType.MetaDateTimeOffset.Scale;
				b2 = (byte)MetaType.MetaDateTimeOffset.FixedLength;
				this._stateObj.Parser.WriteSqlVariantHeader(13, 43, 1, this._stateObj);
				this._stateObj.Parser.WriteByte(b, this._stateObj);
			}
			else
			{
				b = this._metaData.Scale;
				b2 = (byte)this._metaData.MaxLength;
				this._stateObj.Parser.WriteByte(b2, this._stateObj);
			}
			DateTime utcDateTime = value.UtcDateTime;
			long num = utcDateTime.TimeOfDay.Ticks / TdsEnums.TICKS_FROM_SCALE[(int)b];
			int days = utcDateTime.Subtract(DateTime.MinValue).Days;
			short num2 = (short)value.Offset.TotalMinutes;
			this._stateObj.Parser.WriteByteArray(BitConverter.GetBytes(num), (int)(b2 - 5), 0, this._stateObj);
			this._stateObj.Parser.WriteByteArray(BitConverter.GetBytes(days), 3, 0, this._stateObj);
			this._stateObj.Parser.WriteByte((byte)(num2 & 255), this._stateObj);
			this._stateObj.Parser.WriteByte((byte)((num2 >> 8) & 255), this._stateObj);
		}

		// Token: 0x06002B28 RID: 11048 RVA: 0x002A1228 File Offset: 0x002A0628
		internal void SetVariantType(SmiMetaData value)
		{
			this._variantType = value;
		}

		// Token: 0x06002B29 RID: 11049 RVA: 0x002A123C File Offset: 0x002A063C
		[Conditional("DEBUG")]
		private void CheckSettingOffset(long offset)
		{
		}

		// Token: 0x04001C39 RID: 7225
		private TdsParserStateObject _stateObj;

		// Token: 0x04001C3A RID: 7226
		private SmiMetaData _metaData;

		// Token: 0x04001C3B RID: 7227
		private bool _isPlp;

		// Token: 0x04001C3C RID: 7228
		private bool _plpUnknownSent;

		// Token: 0x04001C3D RID: 7229
		private Encoder _encoder;

		// Token: 0x04001C3E RID: 7230
		private SmiMetaData _variantType;
	}
}
