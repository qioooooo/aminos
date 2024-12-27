using System;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes
{
	// Token: 0x02000357 RID: 855
	[XmlSchemaProvider("GetXsdType")]
	[Serializable]
	public struct SqlString : INullable, IComparable, IXmlSerializable
	{
		// Token: 0x06002ED1 RID: 11985 RVA: 0x002AE9A4 File Offset: 0x002ADDA4
		private SqlString(bool fNull)
		{
			this.m_value = null;
			this.m_cmpInfo = null;
			this.m_lcid = 0;
			this.m_flag = SqlCompareOptions.None;
			this.m_fNotNull = false;
		}

		// Token: 0x06002ED2 RID: 11986 RVA: 0x002AE9D4 File Offset: 0x002ADDD4
		public SqlString(int lcid, SqlCompareOptions compareOptions, byte[] data, int index, int count, bool fUnicode)
		{
			this.m_lcid = lcid;
			SqlString.ValidateSqlCompareOptions(compareOptions);
			this.m_flag = compareOptions;
			if (data == null)
			{
				this.m_fNotNull = false;
				this.m_value = null;
				this.m_cmpInfo = null;
				return;
			}
			this.m_fNotNull = true;
			this.m_cmpInfo = null;
			if (fUnicode)
			{
				this.m_value = SqlString.x_UnicodeEncoding.GetString(data, index, count);
				return;
			}
			CultureInfo cultureInfo = new CultureInfo(this.m_lcid);
			Encoding encoding = Encoding.GetEncoding(cultureInfo.TextInfo.ANSICodePage);
			this.m_value = encoding.GetString(data, index, count);
		}

		// Token: 0x06002ED3 RID: 11987 RVA: 0x002AEA64 File Offset: 0x002ADE64
		public SqlString(int lcid, SqlCompareOptions compareOptions, byte[] data, bool fUnicode)
		{
			this = new SqlString(lcid, compareOptions, data, 0, data.Length, fUnicode);
		}

		// Token: 0x06002ED4 RID: 11988 RVA: 0x002AEA80 File Offset: 0x002ADE80
		public SqlString(int lcid, SqlCompareOptions compareOptions, byte[] data, int index, int count)
		{
			this = new SqlString(lcid, compareOptions, data, index, count, true);
		}

		// Token: 0x06002ED5 RID: 11989 RVA: 0x002AEA9C File Offset: 0x002ADE9C
		public SqlString(int lcid, SqlCompareOptions compareOptions, byte[] data)
		{
			this = new SqlString(lcid, compareOptions, data, 0, data.Length, true);
		}

		// Token: 0x06002ED6 RID: 11990 RVA: 0x002AEAB8 File Offset: 0x002ADEB8
		public SqlString(string data, int lcid, SqlCompareOptions compareOptions)
		{
			this.m_lcid = lcid;
			SqlString.ValidateSqlCompareOptions(compareOptions);
			this.m_flag = compareOptions;
			this.m_cmpInfo = null;
			if (data == null)
			{
				this.m_fNotNull = false;
				this.m_value = null;
				return;
			}
			this.m_fNotNull = true;
			this.m_value = data;
		}

		// Token: 0x06002ED7 RID: 11991 RVA: 0x002AEB00 File Offset: 0x002ADF00
		public SqlString(string data, int lcid)
		{
			this = new SqlString(data, lcid, SqlString.x_iDefaultFlag);
		}

		// Token: 0x06002ED8 RID: 11992 RVA: 0x002AEB1C File Offset: 0x002ADF1C
		public SqlString(string data)
		{
			this = new SqlString(data, CultureInfo.CurrentCulture.LCID, SqlString.x_iDefaultFlag);
		}

		// Token: 0x06002ED9 RID: 11993 RVA: 0x002AEB40 File Offset: 0x002ADF40
		private SqlString(int lcid, SqlCompareOptions compareOptions, string data, CompareInfo cmpInfo)
		{
			this.m_lcid = lcid;
			SqlString.ValidateSqlCompareOptions(compareOptions);
			this.m_flag = compareOptions;
			if (data == null)
			{
				this.m_fNotNull = false;
				this.m_value = null;
				this.m_cmpInfo = null;
				return;
			}
			this.m_value = data;
			this.m_cmpInfo = cmpInfo;
			this.m_fNotNull = true;
		}

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06002EDA RID: 11994 RVA: 0x002AEB90 File Offset: 0x002ADF90
		public bool IsNull
		{
			get
			{
				return !this.m_fNotNull;
			}
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06002EDB RID: 11995 RVA: 0x002AEBA8 File Offset: 0x002ADFA8
		public string Value
		{
			get
			{
				if (!this.IsNull)
				{
					return this.m_value;
				}
				throw new SqlNullValueException();
			}
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06002EDC RID: 11996 RVA: 0x002AEBCC File Offset: 0x002ADFCC
		public int LCID
		{
			get
			{
				if (!this.IsNull)
				{
					return this.m_lcid;
				}
				throw new SqlNullValueException();
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06002EDD RID: 11997 RVA: 0x002AEBF0 File Offset: 0x002ADFF0
		public CultureInfo CultureInfo
		{
			get
			{
				if (!this.IsNull)
				{
					return CultureInfo.GetCultureInfo(this.m_lcid);
				}
				throw new SqlNullValueException();
			}
		}

		// Token: 0x06002EDE RID: 11998 RVA: 0x002AEC18 File Offset: 0x002AE018
		private void SetCompareInfo()
		{
			if (this.m_cmpInfo == null)
			{
				this.m_cmpInfo = CultureInfo.GetCultureInfo(this.m_lcid).CompareInfo;
			}
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06002EDF RID: 11999 RVA: 0x002AEC44 File Offset: 0x002AE044
		public CompareInfo CompareInfo
		{
			get
			{
				if (!this.IsNull)
				{
					this.SetCompareInfo();
					return this.m_cmpInfo;
				}
				throw new SqlNullValueException();
			}
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06002EE0 RID: 12000 RVA: 0x002AEC6C File Offset: 0x002AE06C
		public SqlCompareOptions SqlCompareOptions
		{
			get
			{
				if (!this.IsNull)
				{
					return this.m_flag;
				}
				throw new SqlNullValueException();
			}
		}

		// Token: 0x06002EE1 RID: 12001 RVA: 0x002AEC90 File Offset: 0x002AE090
		public static implicit operator SqlString(string x)
		{
			return new SqlString(x);
		}

		// Token: 0x06002EE2 RID: 12002 RVA: 0x002AECA4 File Offset: 0x002AE0A4
		public static explicit operator string(SqlString x)
		{
			return x.Value;
		}

		// Token: 0x06002EE3 RID: 12003 RVA: 0x002AECB8 File Offset: 0x002AE0B8
		public override string ToString()
		{
			if (!this.IsNull)
			{
				return this.m_value;
			}
			return SQLResource.NullString;
		}

		// Token: 0x06002EE4 RID: 12004 RVA: 0x002AECDC File Offset: 0x002AE0DC
		public byte[] GetUnicodeBytes()
		{
			if (this.IsNull)
			{
				return null;
			}
			return SqlString.x_UnicodeEncoding.GetBytes(this.m_value);
		}

		// Token: 0x06002EE5 RID: 12005 RVA: 0x002AED04 File Offset: 0x002AE104
		public byte[] GetNonUnicodeBytes()
		{
			if (this.IsNull)
			{
				return null;
			}
			CultureInfo cultureInfo = new CultureInfo(this.m_lcid);
			Encoding encoding = Encoding.GetEncoding(cultureInfo.TextInfo.ANSICodePage);
			return encoding.GetBytes(this.m_value);
		}

		// Token: 0x06002EE6 RID: 12006 RVA: 0x002AED44 File Offset: 0x002AE144
		public static SqlString operator +(SqlString x, SqlString y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlString.Null;
			}
			if (x.m_lcid != y.m_lcid || x.m_flag != y.m_flag)
			{
				throw new SqlTypeException(SQLResource.ConcatDiffCollationMessage);
			}
			return new SqlString(x.m_lcid, x.m_flag, x.m_value + y.m_value, (x.m_cmpInfo == null) ? y.m_cmpInfo : x.m_cmpInfo);
		}

		// Token: 0x06002EE7 RID: 12007 RVA: 0x002AEDD4 File Offset: 0x002AE1D4
		private static SqlBoolean Compare(SqlString x, SqlString y, EComparison ecExpectedResult)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlBoolean.Null;
			}
			if (x.m_lcid != y.m_lcid || x.m_flag != y.m_flag)
			{
				throw new SqlTypeException(SQLResource.CompareDiffCollationMessage);
			}
			x.SetCompareInfo();
			y.SetCompareInfo();
			int num;
			if ((x.m_flag & SqlCompareOptions.BinarySort) != SqlCompareOptions.None)
			{
				num = SqlString.CompareBinary(x, y);
			}
			else if ((x.m_flag & SqlCompareOptions.BinarySort2) != SqlCompareOptions.None)
			{
				num = SqlString.CompareBinary2(x, y);
			}
			else
			{
				char[] array = x.m_value.ToCharArray();
				char[] array2 = y.m_value.ToCharArray();
				int i = array.Length;
				int num2 = array2.Length;
				while (i > 0)
				{
					if (array[i - 1] != ' ')
					{
						break;
					}
					i--;
				}
				while (num2 > 0 && array2[num2 - 1] == ' ')
				{
					num2--;
				}
				string text = ((i == array.Length) ? x.m_value : new string(array, 0, i));
				string text2 = ((num2 == array2.Length) ? y.m_value : new string(array2, 0, num2));
				CompareOptions compareOptions = SqlString.CompareOptionsFromSqlCompareOptions(x.m_flag);
				num = x.m_cmpInfo.Compare(text, text2, compareOptions);
			}
			bool flag;
			switch (ecExpectedResult)
			{
			case EComparison.LT:
				flag = num < 0;
				break;
			case EComparison.LE:
				flag = num <= 0;
				break;
			case EComparison.EQ:
				flag = num == 0;
				break;
			case EComparison.GE:
				flag = num >= 0;
				break;
			case EComparison.GT:
				flag = num > 0;
				break;
			default:
				return SqlBoolean.Null;
			}
			return new SqlBoolean(flag);
		}

		// Token: 0x06002EE8 RID: 12008 RVA: 0x002AEF68 File Offset: 0x002AE368
		public static explicit operator SqlString(SqlBoolean x)
		{
			if (!x.IsNull)
			{
				return new SqlString(x.Value.ToString());
			}
			return SqlString.Null;
		}

		// Token: 0x06002EE9 RID: 12009 RVA: 0x002AEF98 File Offset: 0x002AE398
		public static explicit operator SqlString(SqlByte x)
		{
			if (!x.IsNull)
			{
				return new SqlString(x.Value.ToString(null));
			}
			return SqlString.Null;
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x002AEFCC File Offset: 0x002AE3CC
		public static explicit operator SqlString(SqlInt16 x)
		{
			if (!x.IsNull)
			{
				return new SqlString(x.Value.ToString(null));
			}
			return SqlString.Null;
		}

		// Token: 0x06002EEB RID: 12011 RVA: 0x002AF000 File Offset: 0x002AE400
		public static explicit operator SqlString(SqlInt32 x)
		{
			if (!x.IsNull)
			{
				return new SqlString(x.Value.ToString(null));
			}
			return SqlString.Null;
		}

		// Token: 0x06002EEC RID: 12012 RVA: 0x002AF034 File Offset: 0x002AE434
		public static explicit operator SqlString(SqlInt64 x)
		{
			if (!x.IsNull)
			{
				return new SqlString(x.Value.ToString(null));
			}
			return SqlString.Null;
		}

		// Token: 0x06002EED RID: 12013 RVA: 0x002AF068 File Offset: 0x002AE468
		public static explicit operator SqlString(SqlSingle x)
		{
			if (!x.IsNull)
			{
				return new SqlString(x.Value.ToString(null));
			}
			return SqlString.Null;
		}

		// Token: 0x06002EEE RID: 12014 RVA: 0x002AF09C File Offset: 0x002AE49C
		public static explicit operator SqlString(SqlDouble x)
		{
			if (!x.IsNull)
			{
				return new SqlString(x.Value.ToString(null));
			}
			return SqlString.Null;
		}

		// Token: 0x06002EEF RID: 12015 RVA: 0x002AF0D0 File Offset: 0x002AE4D0
		public static explicit operator SqlString(SqlDecimal x)
		{
			if (!x.IsNull)
			{
				return new SqlString(x.ToString());
			}
			return SqlString.Null;
		}

		// Token: 0x06002EF0 RID: 12016 RVA: 0x002AF100 File Offset: 0x002AE500
		public static explicit operator SqlString(SqlMoney x)
		{
			if (!x.IsNull)
			{
				return new SqlString(x.ToString());
			}
			return SqlString.Null;
		}

		// Token: 0x06002EF1 RID: 12017 RVA: 0x002AF130 File Offset: 0x002AE530
		public static explicit operator SqlString(SqlDateTime x)
		{
			if (!x.IsNull)
			{
				return new SqlString(x.ToString());
			}
			return SqlString.Null;
		}

		// Token: 0x06002EF2 RID: 12018 RVA: 0x002AF160 File Offset: 0x002AE560
		public static explicit operator SqlString(SqlGuid x)
		{
			if (!x.IsNull)
			{
				return new SqlString(x.ToString());
			}
			return SqlString.Null;
		}

		// Token: 0x06002EF3 RID: 12019 RVA: 0x002AF190 File Offset: 0x002AE590
		public SqlString Clone()
		{
			if (this.IsNull)
			{
				return new SqlString(true);
			}
			SqlString sqlString = new SqlString(this.m_value, this.m_lcid, this.m_flag);
			return sqlString;
		}

		// Token: 0x06002EF4 RID: 12020 RVA: 0x002AF1C8 File Offset: 0x002AE5C8
		public static SqlBoolean operator ==(SqlString x, SqlString y)
		{
			return SqlString.Compare(x, y, EComparison.EQ);
		}

		// Token: 0x06002EF5 RID: 12021 RVA: 0x002AF1E0 File Offset: 0x002AE5E0
		public static SqlBoolean operator !=(SqlString x, SqlString y)
		{
			return !(x == y);
		}

		// Token: 0x06002EF6 RID: 12022 RVA: 0x002AF1FC File Offset: 0x002AE5FC
		public static SqlBoolean operator <(SqlString x, SqlString y)
		{
			return SqlString.Compare(x, y, EComparison.LT);
		}

		// Token: 0x06002EF7 RID: 12023 RVA: 0x002AF214 File Offset: 0x002AE614
		public static SqlBoolean operator >(SqlString x, SqlString y)
		{
			return SqlString.Compare(x, y, EComparison.GT);
		}

		// Token: 0x06002EF8 RID: 12024 RVA: 0x002AF22C File Offset: 0x002AE62C
		public static SqlBoolean operator <=(SqlString x, SqlString y)
		{
			return SqlString.Compare(x, y, EComparison.LE);
		}

		// Token: 0x06002EF9 RID: 12025 RVA: 0x002AF244 File Offset: 0x002AE644
		public static SqlBoolean operator >=(SqlString x, SqlString y)
		{
			return SqlString.Compare(x, y, EComparison.GE);
		}

		// Token: 0x06002EFA RID: 12026 RVA: 0x002AF25C File Offset: 0x002AE65C
		public static SqlString Concat(SqlString x, SqlString y)
		{
			return x + y;
		}

		// Token: 0x06002EFB RID: 12027 RVA: 0x002AF270 File Offset: 0x002AE670
		public static SqlString Add(SqlString x, SqlString y)
		{
			return x + y;
		}

		// Token: 0x06002EFC RID: 12028 RVA: 0x002AF284 File Offset: 0x002AE684
		public static SqlBoolean Equals(SqlString x, SqlString y)
		{
			return x == y;
		}

		// Token: 0x06002EFD RID: 12029 RVA: 0x002AF298 File Offset: 0x002AE698
		public static SqlBoolean NotEquals(SqlString x, SqlString y)
		{
			return x != y;
		}

		// Token: 0x06002EFE RID: 12030 RVA: 0x002AF2AC File Offset: 0x002AE6AC
		public static SqlBoolean LessThan(SqlString x, SqlString y)
		{
			return x < y;
		}

		// Token: 0x06002EFF RID: 12031 RVA: 0x002AF2C0 File Offset: 0x002AE6C0
		public static SqlBoolean GreaterThan(SqlString x, SqlString y)
		{
			return x > y;
		}

		// Token: 0x06002F00 RID: 12032 RVA: 0x002AF2D4 File Offset: 0x002AE6D4
		public static SqlBoolean LessThanOrEqual(SqlString x, SqlString y)
		{
			return x <= y;
		}

		// Token: 0x06002F01 RID: 12033 RVA: 0x002AF2E8 File Offset: 0x002AE6E8
		public static SqlBoolean GreaterThanOrEqual(SqlString x, SqlString y)
		{
			return x >= y;
		}

		// Token: 0x06002F02 RID: 12034 RVA: 0x002AF2FC File Offset: 0x002AE6FC
		public SqlBoolean ToSqlBoolean()
		{
			return (SqlBoolean)this;
		}

		// Token: 0x06002F03 RID: 12035 RVA: 0x002AF314 File Offset: 0x002AE714
		public SqlByte ToSqlByte()
		{
			return (SqlByte)this;
		}

		// Token: 0x06002F04 RID: 12036 RVA: 0x002AF32C File Offset: 0x002AE72C
		public SqlDateTime ToSqlDateTime()
		{
			return (SqlDateTime)this;
		}

		// Token: 0x06002F05 RID: 12037 RVA: 0x002AF344 File Offset: 0x002AE744
		public SqlDouble ToSqlDouble()
		{
			return (SqlDouble)this;
		}

		// Token: 0x06002F06 RID: 12038 RVA: 0x002AF35C File Offset: 0x002AE75C
		public SqlInt16 ToSqlInt16()
		{
			return (SqlInt16)this;
		}

		// Token: 0x06002F07 RID: 12039 RVA: 0x002AF374 File Offset: 0x002AE774
		public SqlInt32 ToSqlInt32()
		{
			return (SqlInt32)this;
		}

		// Token: 0x06002F08 RID: 12040 RVA: 0x002AF38C File Offset: 0x002AE78C
		public SqlInt64 ToSqlInt64()
		{
			return (SqlInt64)this;
		}

		// Token: 0x06002F09 RID: 12041 RVA: 0x002AF3A4 File Offset: 0x002AE7A4
		public SqlMoney ToSqlMoney()
		{
			return (SqlMoney)this;
		}

		// Token: 0x06002F0A RID: 12042 RVA: 0x002AF3BC File Offset: 0x002AE7BC
		public SqlDecimal ToSqlDecimal()
		{
			return (SqlDecimal)this;
		}

		// Token: 0x06002F0B RID: 12043 RVA: 0x002AF3D4 File Offset: 0x002AE7D4
		public SqlSingle ToSqlSingle()
		{
			return (SqlSingle)this;
		}

		// Token: 0x06002F0C RID: 12044 RVA: 0x002AF3EC File Offset: 0x002AE7EC
		public SqlGuid ToSqlGuid()
		{
			return (SqlGuid)this;
		}

		// Token: 0x06002F0D RID: 12045 RVA: 0x002AF404 File Offset: 0x002AE804
		private static void ValidateSqlCompareOptions(SqlCompareOptions compareOptions)
		{
			if ((compareOptions & SqlString.x_iValidSqlCompareOptionMask) != compareOptions)
			{
				throw new ArgumentOutOfRangeException("compareOptions");
			}
		}

		// Token: 0x06002F0E RID: 12046 RVA: 0x002AF428 File Offset: 0x002AE828
		public static CompareOptions CompareOptionsFromSqlCompareOptions(SqlCompareOptions compareOptions)
		{
			CompareOptions compareOptions2 = CompareOptions.None;
			SqlString.ValidateSqlCompareOptions(compareOptions);
			if ((compareOptions & (SqlCompareOptions.BinarySort | SqlCompareOptions.BinarySort2)) != SqlCompareOptions.None)
			{
				throw ADP.ArgumentOutOfRange("compareOptions");
			}
			if ((compareOptions & SqlCompareOptions.IgnoreCase) != SqlCompareOptions.None)
			{
				compareOptions2 |= CompareOptions.IgnoreCase;
			}
			if ((compareOptions & SqlCompareOptions.IgnoreNonSpace) != SqlCompareOptions.None)
			{
				compareOptions2 |= CompareOptions.IgnoreNonSpace;
			}
			if ((compareOptions & SqlCompareOptions.IgnoreKanaType) != SqlCompareOptions.None)
			{
				compareOptions2 |= CompareOptions.IgnoreKanaType;
			}
			if ((compareOptions & SqlCompareOptions.IgnoreWidth) != SqlCompareOptions.None)
			{
				compareOptions2 |= CompareOptions.IgnoreWidth;
			}
			return compareOptions2;
		}

		// Token: 0x06002F0F RID: 12047 RVA: 0x002AF478 File Offset: 0x002AE878
		private bool FBinarySort()
		{
			return !this.IsNull && (this.m_flag & (SqlCompareOptions.BinarySort | SqlCompareOptions.BinarySort2)) != SqlCompareOptions.None;
		}

		// Token: 0x06002F10 RID: 12048 RVA: 0x002AF4A4 File Offset: 0x002AE8A4
		private static int CompareBinary(SqlString x, SqlString y)
		{
			byte[] bytes = SqlString.x_UnicodeEncoding.GetBytes(x.m_value);
			byte[] bytes2 = SqlString.x_UnicodeEncoding.GetBytes(y.m_value);
			int num = bytes.Length;
			int num2 = bytes2.Length;
			int num3 = ((num < num2) ? num : num2);
			int i;
			for (i = 0; i < num3; i++)
			{
				if (bytes[i] < bytes2[i])
				{
					return -1;
				}
				if (bytes[i] > bytes2[i])
				{
					return 1;
				}
			}
			i = num3;
			int num4 = 32;
			if (num < num2)
			{
				while (i < num2)
				{
					int num5 = (int)bytes2[i + 1] << (int)(8 + bytes2[i]);
					if (num5 != num4)
					{
						if (num4 <= num5)
						{
							return -1;
						}
						return 1;
					}
					else
					{
						i += 2;
					}
				}
			}
			else
			{
				while (i < num)
				{
					int num5 = (int)bytes[i + 1] << (int)(8 + bytes[i]);
					if (num5 != num4)
					{
						if (num5 <= num4)
						{
							return -1;
						}
						return 1;
					}
					else
					{
						i += 2;
					}
				}
			}
			return 0;
		}

		// Token: 0x06002F11 RID: 12049 RVA: 0x002AF56C File Offset: 0x002AE96C
		private static int CompareBinary2(SqlString x, SqlString y)
		{
			char[] array = x.m_value.ToCharArray();
			char[] array2 = y.m_value.ToCharArray();
			int num = array.Length;
			int num2 = array2.Length;
			int num3 = ((num < num2) ? num : num2);
			for (int i = 0; i < num3; i++)
			{
				if (array[i] < array2[i])
				{
					return -1;
				}
				if (array[i] > array2[i])
				{
					return 1;
				}
			}
			char c = ' ';
			if (num < num2)
			{
				int i = num3;
				while (i < num2)
				{
					if (array2[i] != c)
					{
						if (c <= array2[i])
						{
							return -1;
						}
						return 1;
					}
					else
					{
						i++;
					}
				}
			}
			else
			{
				int i = num3;
				while (i < num)
				{
					if (array[i] != c)
					{
						if (array[i] <= c)
						{
							return -1;
						}
						return 1;
					}
					else
					{
						i++;
					}
				}
			}
			return 0;
		}

		// Token: 0x06002F12 RID: 12050 RVA: 0x002AF614 File Offset: 0x002AEA14
		public int CompareTo(object value)
		{
			if (value is SqlString)
			{
				SqlString sqlString = (SqlString)value;
				return this.CompareTo(sqlString);
			}
			throw ADP.WrongType(value.GetType(), typeof(SqlString));
		}

		// Token: 0x06002F13 RID: 12051 RVA: 0x002AF650 File Offset: 0x002AEA50
		public int CompareTo(SqlString value)
		{
			if (this.IsNull)
			{
				if (!value.IsNull)
				{
					return -1;
				}
				return 0;
			}
			else
			{
				if (value.IsNull)
				{
					return 1;
				}
				if (this < value)
				{
					return -1;
				}
				if (this > value)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x06002F14 RID: 12052 RVA: 0x002AF6A8 File Offset: 0x002AEAA8
		public override bool Equals(object value)
		{
			if (!(value is SqlString))
			{
				return false;
			}
			SqlString sqlString = (SqlString)value;
			if (sqlString.IsNull || this.IsNull)
			{
				return sqlString.IsNull && this.IsNull;
			}
			return (this == sqlString).Value;
		}

		// Token: 0x06002F15 RID: 12053 RVA: 0x002AF700 File Offset: 0x002AEB00
		public override int GetHashCode()
		{
			if (this.IsNull)
			{
				return 0;
			}
			byte[] array;
			if (this.FBinarySort())
			{
				array = SqlString.x_UnicodeEncoding.GetBytes(this.m_value.TrimEnd(new char[0]));
			}
			else
			{
				this.SetCompareInfo();
				CompareOptions compareOptions = SqlString.CompareOptionsFromSqlCompareOptions(this.m_flag);
				array = this.m_cmpInfo.GetSortKey(this.m_value.TrimEnd(new char[0]), compareOptions).KeyData;
			}
			return SqlBinary.HashByteArray(array, array.Length);
		}

		// Token: 0x06002F16 RID: 12054 RVA: 0x002AF77C File Offset: 0x002AEB7C
		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		// Token: 0x06002F17 RID: 12055 RVA: 0x002AF78C File Offset: 0x002AEB8C
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			string attribute = reader.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance");
			if (attribute != null && XmlConvert.ToBoolean(attribute))
			{
				this.m_fNotNull = false;
				return;
			}
			this.m_value = reader.ReadElementString();
			this.m_fNotNull = true;
		}

		// Token: 0x06002F18 RID: 12056 RVA: 0x002AF7D0 File Offset: 0x002AEBD0
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			if (this.IsNull)
			{
				writer.WriteAttributeString("xsi", "nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
				return;
			}
			writer.WriteString(this.m_value);
		}

		// Token: 0x06002F19 RID: 12057 RVA: 0x002AF80C File Offset: 0x002AEC0C
		public static XmlQualifiedName GetXsdType(XmlSchemaSet schemaSet)
		{
			return new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
		}

		// Token: 0x04001D33 RID: 7475
		private string m_value;

		// Token: 0x04001D34 RID: 7476
		private CompareInfo m_cmpInfo;

		// Token: 0x04001D35 RID: 7477
		private int m_lcid;

		// Token: 0x04001D36 RID: 7478
		private SqlCompareOptions m_flag;

		// Token: 0x04001D37 RID: 7479
		private bool m_fNotNull;

		// Token: 0x04001D38 RID: 7480
		public static readonly SqlString Null = new SqlString(true);

		// Token: 0x04001D39 RID: 7481
		internal static readonly UnicodeEncoding x_UnicodeEncoding = new UnicodeEncoding();

		// Token: 0x04001D3A RID: 7482
		public static readonly int IgnoreCase = 1;

		// Token: 0x04001D3B RID: 7483
		public static readonly int IgnoreWidth = 16;

		// Token: 0x04001D3C RID: 7484
		public static readonly int IgnoreNonSpace = 2;

		// Token: 0x04001D3D RID: 7485
		public static readonly int IgnoreKanaType = 8;

		// Token: 0x04001D3E RID: 7486
		public static readonly int BinarySort = 32768;

		// Token: 0x04001D3F RID: 7487
		public static readonly int BinarySort2 = 16384;

		// Token: 0x04001D40 RID: 7488
		private static readonly SqlCompareOptions x_iDefaultFlag = SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth;

		// Token: 0x04001D41 RID: 7489
		private static readonly CompareOptions x_iValidCompareOptionMask = CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth;

		// Token: 0x04001D42 RID: 7490
		internal static readonly SqlCompareOptions x_iValidSqlCompareOptionMask = SqlCompareOptions.IgnoreCase | SqlCompareOptions.IgnoreNonSpace | SqlCompareOptions.IgnoreKanaType | SqlCompareOptions.IgnoreWidth | SqlCompareOptions.BinarySort | SqlCompareOptions.BinarySort2;

		// Token: 0x04001D43 RID: 7491
		internal static readonly int x_lcidUSEnglish = 1033;

		// Token: 0x04001D44 RID: 7492
		private static readonly int x_lcidBinary = 33280;
	}
}
