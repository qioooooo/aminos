using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000052 RID: 82
	internal sealed class SqlRecordBuffer
	{
		// Token: 0x06000382 RID: 898 RVA: 0x001CF6E4 File Offset: 0x001CEAE4
		internal SqlRecordBuffer(SmiMetaData metaData)
		{
			this._isNull = true;
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000383 RID: 899 RVA: 0x001CF700 File Offset: 0x001CEB00
		internal bool IsNull
		{
			get
			{
				return this._isNull;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000384 RID: 900 RVA: 0x001CF714 File Offset: 0x001CEB14
		// (set) Token: 0x06000385 RID: 901 RVA: 0x001CF72C File Offset: 0x001CEB2C
		internal bool Boolean
		{
			get
			{
				return this._value._boolean;
			}
			set
			{
				this._value._boolean = value;
				this._type = SqlRecordBuffer.StorageType.Boolean;
				this._isNull = false;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000386 RID: 902 RVA: 0x001CF754 File Offset: 0x001CEB54
		// (set) Token: 0x06000387 RID: 903 RVA: 0x001CF76C File Offset: 0x001CEB6C
		internal byte Byte
		{
			get
			{
				return this._value._byte;
			}
			set
			{
				this._value._byte = value;
				this._type = SqlRecordBuffer.StorageType.Byte;
				this._isNull = false;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000388 RID: 904 RVA: 0x001CF794 File Offset: 0x001CEB94
		// (set) Token: 0x06000389 RID: 905 RVA: 0x001CF7AC File Offset: 0x001CEBAC
		internal DateTime DateTime
		{
			get
			{
				return this._value._dateTime;
			}
			set
			{
				this._value._dateTime = value;
				this._type = SqlRecordBuffer.StorageType.DateTime;
				this._isNull = false;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600038A RID: 906 RVA: 0x001CF7D4 File Offset: 0x001CEBD4
		// (set) Token: 0x0600038B RID: 907 RVA: 0x001CF7EC File Offset: 0x001CEBEC
		internal DateTimeOffset DateTimeOffset
		{
			get
			{
				return this._value._dateTimeOffset;
			}
			set
			{
				this._value._dateTimeOffset = value;
				this._type = SqlRecordBuffer.StorageType.DateTimeOffset;
				this._isNull = false;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600038C RID: 908 RVA: 0x001CF814 File Offset: 0x001CEC14
		// (set) Token: 0x0600038D RID: 909 RVA: 0x001CF82C File Offset: 0x001CEC2C
		internal double Double
		{
			get
			{
				return this._value._double;
			}
			set
			{
				this._value._double = value;
				this._type = SqlRecordBuffer.StorageType.Double;
				this._isNull = false;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600038E RID: 910 RVA: 0x001CF854 File Offset: 0x001CEC54
		// (set) Token: 0x0600038F RID: 911 RVA: 0x001CF86C File Offset: 0x001CEC6C
		internal Guid Guid
		{
			get
			{
				return this._value._guid;
			}
			set
			{
				this._value._guid = value;
				this._type = SqlRecordBuffer.StorageType.Guid;
				this._isNull = false;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000390 RID: 912 RVA: 0x001CF894 File Offset: 0x001CEC94
		// (set) Token: 0x06000391 RID: 913 RVA: 0x001CF8AC File Offset: 0x001CECAC
		internal short Int16
		{
			get
			{
				return this._value._int16;
			}
			set
			{
				this._value._int16 = value;
				this._type = SqlRecordBuffer.StorageType.Int16;
				this._isNull = false;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000392 RID: 914 RVA: 0x001CF8D4 File Offset: 0x001CECD4
		// (set) Token: 0x06000393 RID: 915 RVA: 0x001CF8EC File Offset: 0x001CECEC
		internal int Int32
		{
			get
			{
				return this._value._int32;
			}
			set
			{
				this._value._int32 = value;
				this._type = SqlRecordBuffer.StorageType.Int32;
				this._isNull = false;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000394 RID: 916 RVA: 0x001CF914 File Offset: 0x001CED14
		// (set) Token: 0x06000395 RID: 917 RVA: 0x001CF92C File Offset: 0x001CED2C
		internal long Int64
		{
			get
			{
				return this._value._int64;
			}
			set
			{
				this._value._int64 = value;
				this._type = SqlRecordBuffer.StorageType.Int64;
				this._isNull = false;
				if (this._isMetaSet)
				{
					this._isMetaSet = false;
					return;
				}
				this._metadata = null;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000396 RID: 918 RVA: 0x001CF96C File Offset: 0x001CED6C
		// (set) Token: 0x06000397 RID: 919 RVA: 0x001CF984 File Offset: 0x001CED84
		internal float Single
		{
			get
			{
				return this._value._single;
			}
			set
			{
				this._value._single = value;
				this._type = SqlRecordBuffer.StorageType.Single;
				this._isNull = false;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000398 RID: 920 RVA: 0x001CF9AC File Offset: 0x001CEDAC
		// (set) Token: 0x06000399 RID: 921 RVA: 0x001CFA10 File Offset: 0x001CEE10
		internal string String
		{
			get
			{
				if (SqlRecordBuffer.StorageType.String == this._type)
				{
					return (string)this._object;
				}
				if (SqlRecordBuffer.StorageType.CharArray == this._type)
				{
					return new string((char[])this._object, 0, (int)this.CharsLength);
				}
				Stream stream = new MemoryStream((byte[])this._object, false);
				return new SqlXml(stream).Value;
			}
			set
			{
				this._object = value;
				this._value._int64 = (long)value.Length;
				this._type = SqlRecordBuffer.StorageType.String;
				this._isNull = false;
				if (this._isMetaSet)
				{
					this._isMetaSet = false;
					return;
				}
				this._metadata = null;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600039A RID: 922 RVA: 0x001CFA5C File Offset: 0x001CEE5C
		// (set) Token: 0x0600039B RID: 923 RVA: 0x001CFA74 File Offset: 0x001CEE74
		internal SqlDecimal SqlDecimal
		{
			get
			{
				return (SqlDecimal)this._object;
			}
			set
			{
				this._object = value;
				this._type = SqlRecordBuffer.StorageType.SqlDecimal;
				this._isNull = false;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600039C RID: 924 RVA: 0x001CFA9C File Offset: 0x001CEE9C
		// (set) Token: 0x0600039D RID: 925 RVA: 0x001CFAB4 File Offset: 0x001CEEB4
		internal TimeSpan TimeSpan
		{
			get
			{
				return this._value._timeSpan;
			}
			set
			{
				this._value._timeSpan = value;
				this._type = SqlRecordBuffer.StorageType.TimeSpan;
				this._isNull = false;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600039E RID: 926 RVA: 0x001CFADC File Offset: 0x001CEEDC
		// (set) Token: 0x0600039F RID: 927 RVA: 0x001CFB04 File Offset: 0x001CEF04
		internal long BytesLength
		{
			get
			{
				if (SqlRecordBuffer.StorageType.String == this._type)
				{
					this.ConvertXmlStringToByteArray();
				}
				return this._value._int64;
			}
			set
			{
				if (0L == value)
				{
					this._value._int64 = value;
					this._object = new byte[0];
					this._type = SqlRecordBuffer.StorageType.ByteArray;
					this._isNull = false;
					return;
				}
				this._value._int64 = value;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x001CFB4C File Offset: 0x001CEF4C
		// (set) Token: 0x060003A1 RID: 929 RVA: 0x001CFB64 File Offset: 0x001CEF64
		internal long CharsLength
		{
			get
			{
				return this._value._int64;
			}
			set
			{
				if (0L == value)
				{
					this._value._int64 = value;
					this._object = new char[0];
					this._type = SqlRecordBuffer.StorageType.CharArray;
					this._isNull = false;
					return;
				}
				this._value._int64 = value;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x001CFBAC File Offset: 0x001CEFAC
		// (set) Token: 0x060003A3 RID: 931 RVA: 0x001CFCA4 File Offset: 0x001CF0A4
		internal SmiMetaData VariantType
		{
			get
			{
				switch (this._type)
				{
				case SqlRecordBuffer.StorageType.Boolean:
					return SmiMetaData.DefaultBit;
				case SqlRecordBuffer.StorageType.Byte:
					return SmiMetaData.DefaultTinyInt;
				case SqlRecordBuffer.StorageType.ByteArray:
					return SmiMetaData.DefaultVarBinary;
				case SqlRecordBuffer.StorageType.CharArray:
					return SmiMetaData.DefaultNVarChar;
				case SqlRecordBuffer.StorageType.DateTime:
					return SmiMetaData.DefaultDateTime;
				case SqlRecordBuffer.StorageType.DateTimeOffset:
					return SmiMetaData.DefaultDateTimeOffset;
				case SqlRecordBuffer.StorageType.Double:
					return SmiMetaData.DefaultFloat;
				case SqlRecordBuffer.StorageType.Guid:
					return SmiMetaData.DefaultUniqueIdentifier;
				case SqlRecordBuffer.StorageType.Int16:
					return SmiMetaData.DefaultSmallInt;
				case SqlRecordBuffer.StorageType.Int32:
					return SmiMetaData.DefaultInt;
				case SqlRecordBuffer.StorageType.Int64:
					return this._metadata ?? SmiMetaData.DefaultBigInt;
				case SqlRecordBuffer.StorageType.Single:
					return SmiMetaData.DefaultReal;
				case SqlRecordBuffer.StorageType.String:
					return this._metadata ?? SmiMetaData.DefaultNVarChar;
				case SqlRecordBuffer.StorageType.SqlDecimal:
					return new SmiMetaData(SqlDbType.Decimal, 17L, ((SqlDecimal)this._object).Precision, ((SqlDecimal)this._object).Scale, 0L, SqlCompareOptions.None, null);
				case SqlRecordBuffer.StorageType.TimeSpan:
					return SmiMetaData.DefaultTime;
				default:
					return null;
				}
			}
			set
			{
				this._metadata = value;
				this._isMetaSet = true;
			}
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x001CFCC0 File Offset: 0x001CF0C0
		internal int GetBytes(long fieldOffset, byte[] buffer, int bufferOffset, int length)
		{
			int num = (int)fieldOffset;
			if (SqlRecordBuffer.StorageType.String == this._type)
			{
				this.ConvertXmlStringToByteArray();
			}
			Buffer.BlockCopy((byte[])this._object, num, buffer, bufferOffset, length);
			return length;
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x001CFCF8 File Offset: 0x001CF0F8
		internal int GetChars(long fieldOffset, char[] buffer, int bufferOffset, int length)
		{
			int num = (int)fieldOffset;
			if (SqlRecordBuffer.StorageType.CharArray == this._type)
			{
				Array.Copy((char[])this._object, num, buffer, bufferOffset, length);
			}
			else
			{
				((string)this._object).CopyTo(num, buffer, bufferOffset, length);
			}
			return length;
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x001CFD40 File Offset: 0x001CF140
		internal int SetBytes(long fieldOffset, byte[] buffer, int bufferOffset, int length)
		{
			int num = (int)fieldOffset;
			if (this.IsNull || SqlRecordBuffer.StorageType.ByteArray != this._type)
			{
				if (num != 0)
				{
					throw ADP.ArgumentOutOfRange("fieldOffset");
				}
				this._object = new byte[length];
				this._type = SqlRecordBuffer.StorageType.ByteArray;
				this._isNull = false;
				this.BytesLength = (long)length;
			}
			else
			{
				if ((long)num > this.BytesLength)
				{
					throw ADP.ArgumentOutOfRange("fieldOffset");
				}
				if ((long)(num + length) > this.BytesLength)
				{
					int num2 = ((byte[])this._object).Length;
					if (num + length > num2)
					{
						byte[] array = new byte[Math.Max(num + length, 2 * num2)];
						Buffer.BlockCopy((byte[])this._object, 0, array, 0, (int)this.BytesLength);
						this._object = array;
					}
					this.BytesLength = (long)(num + length);
				}
			}
			Buffer.BlockCopy(buffer, bufferOffset, (byte[])this._object, num, length);
			return length;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x001CFE24 File Offset: 0x001CF224
		internal int SetChars(long fieldOffset, char[] buffer, int bufferOffset, int length)
		{
			int num = (int)fieldOffset;
			if (this.IsNull || (SqlRecordBuffer.StorageType.CharArray != this._type && SqlRecordBuffer.StorageType.String != this._type))
			{
				if (num != 0)
				{
					throw ADP.ArgumentOutOfRange("fieldOffset");
				}
				this._object = new char[length];
				this._type = SqlRecordBuffer.StorageType.CharArray;
				this._isNull = false;
				this.CharsLength = (long)length;
			}
			else
			{
				if ((long)num > this.CharsLength)
				{
					throw ADP.ArgumentOutOfRange("fieldOffset");
				}
				if (SqlRecordBuffer.StorageType.String == this._type)
				{
					this._object = ((string)this._object).ToCharArray();
					this._type = SqlRecordBuffer.StorageType.CharArray;
				}
				if ((long)(num + length) > this.CharsLength)
				{
					int num2 = ((char[])this._object).Length;
					if (num + length > num2)
					{
						char[] array = new char[Math.Max(num + length, 2 * num2)];
						Array.Copy((char[])this._object, 0L, array, 0L, this.CharsLength);
						this._object = array;
					}
					this.CharsLength = (long)(num + length);
				}
			}
			Array.Copy(buffer, bufferOffset, (char[])this._object, num, length);
			return length;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x001CFF3C File Offset: 0x001CF33C
		internal void SetNull()
		{
			this._isNull = true;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x001CFF50 File Offset: 0x001CF350
		private void ConvertXmlStringToByteArray()
		{
			string text = (string)this._object;
			byte[] array = new byte[2 + Encoding.Unicode.GetByteCount(text)];
			array[0] = byte.MaxValue;
			array[1] = 254;
			Encoding.Unicode.GetBytes(text, 0, text.Length, array, 2);
			this._object = array;
			this._value._int64 = (long)array.Length;
			this._type = SqlRecordBuffer.StorageType.ByteArray;
		}

		// Token: 0x0400061B RID: 1563
		private bool _isNull;

		// Token: 0x0400061C RID: 1564
		private SqlRecordBuffer.StorageType _type;

		// Token: 0x0400061D RID: 1565
		private SqlRecordBuffer.Storage _value;

		// Token: 0x0400061E RID: 1566
		private object _object;

		// Token: 0x0400061F RID: 1567
		private SmiMetaData _metadata;

		// Token: 0x04000620 RID: 1568
		private bool _isMetaSet;

		// Token: 0x02000053 RID: 83
		internal enum StorageType
		{
			// Token: 0x04000622 RID: 1570
			Boolean,
			// Token: 0x04000623 RID: 1571
			Byte,
			// Token: 0x04000624 RID: 1572
			ByteArray,
			// Token: 0x04000625 RID: 1573
			CharArray,
			// Token: 0x04000626 RID: 1574
			DateTime,
			// Token: 0x04000627 RID: 1575
			DateTimeOffset,
			// Token: 0x04000628 RID: 1576
			Double,
			// Token: 0x04000629 RID: 1577
			Guid,
			// Token: 0x0400062A RID: 1578
			Int16,
			// Token: 0x0400062B RID: 1579
			Int32,
			// Token: 0x0400062C RID: 1580
			Int64,
			// Token: 0x0400062D RID: 1581
			Single,
			// Token: 0x0400062E RID: 1582
			String,
			// Token: 0x0400062F RID: 1583
			SqlDecimal,
			// Token: 0x04000630 RID: 1584
			TimeSpan
		}

		// Token: 0x02000054 RID: 84
		[StructLayout(LayoutKind.Explicit)]
		internal struct Storage
		{
			// Token: 0x04000631 RID: 1585
			[FieldOffset(0)]
			internal bool _boolean;

			// Token: 0x04000632 RID: 1586
			[FieldOffset(0)]
			internal byte _byte;

			// Token: 0x04000633 RID: 1587
			[FieldOffset(0)]
			internal DateTime _dateTime;

			// Token: 0x04000634 RID: 1588
			[FieldOffset(0)]
			internal DateTimeOffset _dateTimeOffset;

			// Token: 0x04000635 RID: 1589
			[FieldOffset(0)]
			internal double _double;

			// Token: 0x04000636 RID: 1590
			[FieldOffset(0)]
			internal Guid _guid;

			// Token: 0x04000637 RID: 1591
			[FieldOffset(0)]
			internal short _int16;

			// Token: 0x04000638 RID: 1592
			[FieldOffset(0)]
			internal int _int32;

			// Token: 0x04000639 RID: 1593
			[FieldOffset(0)]
			internal long _int64;

			// Token: 0x0400063A RID: 1594
			[FieldOffset(0)]
			internal float _single;

			// Token: 0x0400063B RID: 1595
			[FieldOffset(0)]
			internal TimeSpan _timeSpan;
		}
	}
}
