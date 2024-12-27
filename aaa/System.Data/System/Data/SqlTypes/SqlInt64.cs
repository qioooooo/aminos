using System;
using System.Data.Common;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes
{
	// Token: 0x02000352 RID: 850
	[XmlSchemaProvider("GetXsdType")]
	[Serializable]
	public struct SqlInt64 : INullable, IComparable, IXmlSerializable
	{
		// Token: 0x06002E0B RID: 11787 RVA: 0x002AC5D4 File Offset: 0x002AB9D4
		private SqlInt64(bool fNull)
		{
			this.m_fNotNull = false;
			this.m_value = 0L;
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x002AC5F0 File Offset: 0x002AB9F0
		public SqlInt64(long value)
		{
			this.m_value = value;
			this.m_fNotNull = true;
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06002E0D RID: 11789 RVA: 0x002AC60C File Offset: 0x002ABA0C
		public bool IsNull
		{
			get
			{
				return !this.m_fNotNull;
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06002E0E RID: 11790 RVA: 0x002AC624 File Offset: 0x002ABA24
		public long Value
		{
			get
			{
				if (this.m_fNotNull)
				{
					return this.m_value;
				}
				throw new SqlNullValueException();
			}
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x002AC648 File Offset: 0x002ABA48
		public static implicit operator SqlInt64(long x)
		{
			return new SqlInt64(x);
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x002AC65C File Offset: 0x002ABA5C
		public static explicit operator long(SqlInt64 x)
		{
			return x.Value;
		}

		// Token: 0x06002E11 RID: 11793 RVA: 0x002AC670 File Offset: 0x002ABA70
		public override string ToString()
		{
			if (!this.IsNull)
			{
				return this.m_value.ToString(null);
			}
			return SQLResource.NullString;
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x002AC698 File Offset: 0x002ABA98
		public static SqlInt64 Parse(string s)
		{
			if (s == SQLResource.NullString)
			{
				return SqlInt64.Null;
			}
			return new SqlInt64(long.Parse(s, null));
		}

		// Token: 0x06002E13 RID: 11795 RVA: 0x002AC6C4 File Offset: 0x002ABAC4
		public static SqlInt64 operator -(SqlInt64 x)
		{
			if (!x.IsNull)
			{
				return new SqlInt64(-x.m_value);
			}
			return SqlInt64.Null;
		}

		// Token: 0x06002E14 RID: 11796 RVA: 0x002AC6F0 File Offset: 0x002ABAF0
		public static SqlInt64 operator ~(SqlInt64 x)
		{
			if (!x.IsNull)
			{
				return new SqlInt64(~x.m_value);
			}
			return SqlInt64.Null;
		}

		// Token: 0x06002E15 RID: 11797 RVA: 0x002AC71C File Offset: 0x002ABB1C
		public static SqlInt64 operator +(SqlInt64 x, SqlInt64 y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlInt64.Null;
			}
			long num = x.m_value + y.m_value;
			if (SqlInt64.SameSignLong(x.m_value, y.m_value) && !SqlInt64.SameSignLong(x.m_value, num))
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlInt64(num);
		}

		// Token: 0x06002E16 RID: 11798 RVA: 0x002AC788 File Offset: 0x002ABB88
		public static SqlInt64 operator -(SqlInt64 x, SqlInt64 y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlInt64.Null;
			}
			long num = x.m_value - y.m_value;
			if (!SqlInt64.SameSignLong(x.m_value, y.m_value) && SqlInt64.SameSignLong(y.m_value, num))
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlInt64(num);
		}

		// Token: 0x06002E17 RID: 11799 RVA: 0x002AC7F4 File Offset: 0x002ABBF4
		public static SqlInt64 operator *(SqlInt64 x, SqlInt64 y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlInt64.Null;
			}
			bool flag = false;
			long num = x.m_value;
			long num2 = y.m_value;
			long num3 = 0L;
			if (num < 0L)
			{
				flag = true;
				num = -num;
			}
			if (num2 < 0L)
			{
				flag = !flag;
				num2 = -num2;
			}
			long num4 = num & SqlInt64.x_lLowIntMask;
			long num5 = (num >> 32) & SqlInt64.x_lLowIntMask;
			long num6 = num2 & SqlInt64.x_lLowIntMask;
			long num7 = (num2 >> 32) & SqlInt64.x_lLowIntMask;
			if (num5 != 0L && num7 != 0L)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			long num8 = num4 * num6;
			if (num8 < 0L)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			if (num5 != 0L)
			{
				num3 = num5 * num6;
				if (num3 < 0L || num3 > 9223372036854775807L)
				{
					throw new OverflowException(SQLResource.ArithOverflowMessage);
				}
			}
			else if (num7 != 0L)
			{
				num3 = num4 * num7;
				if (num3 < 0L || num3 > 9223372036854775807L)
				{
					throw new OverflowException(SQLResource.ArithOverflowMessage);
				}
			}
			num8 += num3 << 32;
			if (num8 < 0L)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			if (flag)
			{
				num8 = -num8;
			}
			return new SqlInt64(num8);
		}

		// Token: 0x06002E18 RID: 11800 RVA: 0x002AC914 File Offset: 0x002ABD14
		public static SqlInt64 operator /(SqlInt64 x, SqlInt64 y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlInt64.Null;
			}
			if (y.m_value == 0L)
			{
				throw new DivideByZeroException(SQLResource.DivideByZeroMessage);
			}
			if (x.m_value == -9223372036854775808L && y.m_value == -1L)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlInt64(x.m_value / y.m_value);
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x002AC98C File Offset: 0x002ABD8C
		public static SqlInt64 operator %(SqlInt64 x, SqlInt64 y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlInt64.Null;
			}
			if (y.m_value == 0L)
			{
				throw new DivideByZeroException(SQLResource.DivideByZeroMessage);
			}
			if (x.m_value == -9223372036854775808L && y.m_value == -1L)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlInt64(x.m_value % y.m_value);
		}

		// Token: 0x06002E1A RID: 11802 RVA: 0x002ACA04 File Offset: 0x002ABE04
		public static SqlInt64 operator &(SqlInt64 x, SqlInt64 y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlInt64(x.m_value & y.m_value);
			}
			return SqlInt64.Null;
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x002ACA40 File Offset: 0x002ABE40
		public static SqlInt64 operator |(SqlInt64 x, SqlInt64 y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlInt64(x.m_value | y.m_value);
			}
			return SqlInt64.Null;
		}

		// Token: 0x06002E1C RID: 11804 RVA: 0x002ACA7C File Offset: 0x002ABE7C
		public static SqlInt64 operator ^(SqlInt64 x, SqlInt64 y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlInt64(x.m_value ^ y.m_value);
			}
			return SqlInt64.Null;
		}

		// Token: 0x06002E1D RID: 11805 RVA: 0x002ACAB8 File Offset: 0x002ABEB8
		public static explicit operator SqlInt64(SqlBoolean x)
		{
			if (!x.IsNull)
			{
				return new SqlInt64((long)((ulong)x.ByteValue));
			}
			return SqlInt64.Null;
		}

		// Token: 0x06002E1E RID: 11806 RVA: 0x002ACAE4 File Offset: 0x002ABEE4
		public static implicit operator SqlInt64(SqlByte x)
		{
			if (!x.IsNull)
			{
				return new SqlInt64((long)((ulong)x.Value));
			}
			return SqlInt64.Null;
		}

		// Token: 0x06002E1F RID: 11807 RVA: 0x002ACB10 File Offset: 0x002ABF10
		public static implicit operator SqlInt64(SqlInt16 x)
		{
			if (!x.IsNull)
			{
				return new SqlInt64((long)x.Value);
			}
			return SqlInt64.Null;
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x002ACB3C File Offset: 0x002ABF3C
		public static implicit operator SqlInt64(SqlInt32 x)
		{
			if (!x.IsNull)
			{
				return new SqlInt64((long)x.Value);
			}
			return SqlInt64.Null;
		}

		// Token: 0x06002E21 RID: 11809 RVA: 0x002ACB68 File Offset: 0x002ABF68
		public static explicit operator SqlInt64(SqlSingle x)
		{
			if (x.IsNull)
			{
				return SqlInt64.Null;
			}
			float value = x.Value;
			if (value > 9.223372E+18f || value < -9.223372E+18f)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlInt64((long)value);
		}

		// Token: 0x06002E22 RID: 11810 RVA: 0x002ACBB0 File Offset: 0x002ABFB0
		public static explicit operator SqlInt64(SqlDouble x)
		{
			if (x.IsNull)
			{
				return SqlInt64.Null;
			}
			double value = x.Value;
			if (value > 9.223372036854776E+18 || value < -9.223372036854776E+18)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlInt64((long)value);
		}

		// Token: 0x06002E23 RID: 11811 RVA: 0x002ACC00 File Offset: 0x002AC000
		public static explicit operator SqlInt64(SqlMoney x)
		{
			if (!x.IsNull)
			{
				return new SqlInt64(x.ToInt64());
			}
			return SqlInt64.Null;
		}

		// Token: 0x06002E24 RID: 11812 RVA: 0x002ACC28 File Offset: 0x002AC028
		public static explicit operator SqlInt64(SqlDecimal x)
		{
			if (x.IsNull)
			{
				return SqlInt64.Null;
			}
			SqlDecimal sqlDecimal = x;
			sqlDecimal.AdjustScale((int)(-(int)sqlDecimal.m_bScale), false);
			if (sqlDecimal.m_bLen > 2)
			{
				throw new OverflowException(SQLResource.ConversionOverflowMessage);
			}
			long num2;
			if (sqlDecimal.m_bLen == 2)
			{
				ulong num = SqlDecimal.DWL(sqlDecimal.m_data1, sqlDecimal.m_data2);
				if (num > SqlDecimal.x_llMax && (sqlDecimal.IsPositive || num != 1UL + SqlDecimal.x_llMax))
				{
					throw new OverflowException(SQLResource.ConversionOverflowMessage);
				}
				num2 = (long)num;
			}
			else
			{
				num2 = (long)((ulong)sqlDecimal.m_data1);
			}
			if (!sqlDecimal.IsPositive)
			{
				num2 = -num2;
			}
			return new SqlInt64(num2);
		}

		// Token: 0x06002E25 RID: 11813 RVA: 0x002ACCD0 File Offset: 0x002AC0D0
		public static explicit operator SqlInt64(SqlString x)
		{
			if (!x.IsNull)
			{
				return new SqlInt64(long.Parse(x.Value, null));
			}
			return SqlInt64.Null;
		}

		// Token: 0x06002E26 RID: 11814 RVA: 0x002ACD00 File Offset: 0x002AC100
		private static bool SameSignLong(long x, long y)
		{
			return ((x ^ y) & long.MinValue) == 0L;
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x002ACD20 File Offset: 0x002AC120
		public static SqlBoolean operator ==(SqlInt64 x, SqlInt64 y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value == y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002E28 RID: 11816 RVA: 0x002ACD5C File Offset: 0x002AC15C
		public static SqlBoolean operator !=(SqlInt64 x, SqlInt64 y)
		{
			return !(x == y);
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x002ACD78 File Offset: 0x002AC178
		public static SqlBoolean operator <(SqlInt64 x, SqlInt64 y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value < y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002E2A RID: 11818 RVA: 0x002ACDB4 File Offset: 0x002AC1B4
		public static SqlBoolean operator >(SqlInt64 x, SqlInt64 y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value > y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002E2B RID: 11819 RVA: 0x002ACDF0 File Offset: 0x002AC1F0
		public static SqlBoolean operator <=(SqlInt64 x, SqlInt64 y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value <= y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002E2C RID: 11820 RVA: 0x002ACE30 File Offset: 0x002AC230
		public static SqlBoolean operator >=(SqlInt64 x, SqlInt64 y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value >= y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002E2D RID: 11821 RVA: 0x002ACE70 File Offset: 0x002AC270
		public static SqlInt64 OnesComplement(SqlInt64 x)
		{
			return ~x;
		}

		// Token: 0x06002E2E RID: 11822 RVA: 0x002ACE84 File Offset: 0x002AC284
		public static SqlInt64 Add(SqlInt64 x, SqlInt64 y)
		{
			return x + y;
		}

		// Token: 0x06002E2F RID: 11823 RVA: 0x002ACE98 File Offset: 0x002AC298
		public static SqlInt64 Subtract(SqlInt64 x, SqlInt64 y)
		{
			return x - y;
		}

		// Token: 0x06002E30 RID: 11824 RVA: 0x002ACEAC File Offset: 0x002AC2AC
		public static SqlInt64 Multiply(SqlInt64 x, SqlInt64 y)
		{
			return x * y;
		}

		// Token: 0x06002E31 RID: 11825 RVA: 0x002ACEC0 File Offset: 0x002AC2C0
		public static SqlInt64 Divide(SqlInt64 x, SqlInt64 y)
		{
			return x / y;
		}

		// Token: 0x06002E32 RID: 11826 RVA: 0x002ACED4 File Offset: 0x002AC2D4
		public static SqlInt64 Mod(SqlInt64 x, SqlInt64 y)
		{
			return x % y;
		}

		// Token: 0x06002E33 RID: 11827 RVA: 0x002ACEE8 File Offset: 0x002AC2E8
		public static SqlInt64 Modulus(SqlInt64 x, SqlInt64 y)
		{
			return x % y;
		}

		// Token: 0x06002E34 RID: 11828 RVA: 0x002ACEFC File Offset: 0x002AC2FC
		public static SqlInt64 BitwiseAnd(SqlInt64 x, SqlInt64 y)
		{
			return x & y;
		}

		// Token: 0x06002E35 RID: 11829 RVA: 0x002ACF10 File Offset: 0x002AC310
		public static SqlInt64 BitwiseOr(SqlInt64 x, SqlInt64 y)
		{
			return x | y;
		}

		// Token: 0x06002E36 RID: 11830 RVA: 0x002ACF24 File Offset: 0x002AC324
		public static SqlInt64 Xor(SqlInt64 x, SqlInt64 y)
		{
			return x ^ y;
		}

		// Token: 0x06002E37 RID: 11831 RVA: 0x002ACF38 File Offset: 0x002AC338
		public static SqlBoolean Equals(SqlInt64 x, SqlInt64 y)
		{
			return x == y;
		}

		// Token: 0x06002E38 RID: 11832 RVA: 0x002ACF4C File Offset: 0x002AC34C
		public static SqlBoolean NotEquals(SqlInt64 x, SqlInt64 y)
		{
			return x != y;
		}

		// Token: 0x06002E39 RID: 11833 RVA: 0x002ACF60 File Offset: 0x002AC360
		public static SqlBoolean LessThan(SqlInt64 x, SqlInt64 y)
		{
			return x < y;
		}

		// Token: 0x06002E3A RID: 11834 RVA: 0x002ACF74 File Offset: 0x002AC374
		public static SqlBoolean GreaterThan(SqlInt64 x, SqlInt64 y)
		{
			return x > y;
		}

		// Token: 0x06002E3B RID: 11835 RVA: 0x002ACF88 File Offset: 0x002AC388
		public static SqlBoolean LessThanOrEqual(SqlInt64 x, SqlInt64 y)
		{
			return x <= y;
		}

		// Token: 0x06002E3C RID: 11836 RVA: 0x002ACF9C File Offset: 0x002AC39C
		public static SqlBoolean GreaterThanOrEqual(SqlInt64 x, SqlInt64 y)
		{
			return x >= y;
		}

		// Token: 0x06002E3D RID: 11837 RVA: 0x002ACFB0 File Offset: 0x002AC3B0
		public SqlBoolean ToSqlBoolean()
		{
			return (SqlBoolean)this;
		}

		// Token: 0x06002E3E RID: 11838 RVA: 0x002ACFC8 File Offset: 0x002AC3C8
		public SqlByte ToSqlByte()
		{
			return (SqlByte)this;
		}

		// Token: 0x06002E3F RID: 11839 RVA: 0x002ACFE0 File Offset: 0x002AC3E0
		public SqlDouble ToSqlDouble()
		{
			return this;
		}

		// Token: 0x06002E40 RID: 11840 RVA: 0x002ACFF8 File Offset: 0x002AC3F8
		public SqlInt16 ToSqlInt16()
		{
			return (SqlInt16)this;
		}

		// Token: 0x06002E41 RID: 11841 RVA: 0x002AD010 File Offset: 0x002AC410
		public SqlInt32 ToSqlInt32()
		{
			return (SqlInt32)this;
		}

		// Token: 0x06002E42 RID: 11842 RVA: 0x002AD028 File Offset: 0x002AC428
		public SqlMoney ToSqlMoney()
		{
			return this;
		}

		// Token: 0x06002E43 RID: 11843 RVA: 0x002AD040 File Offset: 0x002AC440
		public SqlDecimal ToSqlDecimal()
		{
			return this;
		}

		// Token: 0x06002E44 RID: 11844 RVA: 0x002AD058 File Offset: 0x002AC458
		public SqlSingle ToSqlSingle()
		{
			return this;
		}

		// Token: 0x06002E45 RID: 11845 RVA: 0x002AD070 File Offset: 0x002AC470
		public SqlString ToSqlString()
		{
			return (SqlString)this;
		}

		// Token: 0x06002E46 RID: 11846 RVA: 0x002AD088 File Offset: 0x002AC488
		public int CompareTo(object value)
		{
			if (value is SqlInt64)
			{
				SqlInt64 sqlInt = (SqlInt64)value;
				return this.CompareTo(sqlInt);
			}
			throw ADP.WrongType(value.GetType(), typeof(SqlInt64));
		}

		// Token: 0x06002E47 RID: 11847 RVA: 0x002AD0C4 File Offset: 0x002AC4C4
		public int CompareTo(SqlInt64 value)
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

		// Token: 0x06002E48 RID: 11848 RVA: 0x002AD11C File Offset: 0x002AC51C
		public override bool Equals(object value)
		{
			if (!(value is SqlInt64))
			{
				return false;
			}
			SqlInt64 sqlInt = (SqlInt64)value;
			if (sqlInt.IsNull || this.IsNull)
			{
				return sqlInt.IsNull && this.IsNull;
			}
			return (this == sqlInt).Value;
		}

		// Token: 0x06002E49 RID: 11849 RVA: 0x002AD174 File Offset: 0x002AC574
		public override int GetHashCode()
		{
			if (!this.IsNull)
			{
				return this.Value.GetHashCode();
			}
			return 0;
		}

		// Token: 0x06002E4A RID: 11850 RVA: 0x002AD19C File Offset: 0x002AC59C
		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		// Token: 0x06002E4B RID: 11851 RVA: 0x002AD1AC File Offset: 0x002AC5AC
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			string attribute = reader.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance");
			if (attribute != null && XmlConvert.ToBoolean(attribute))
			{
				this.m_fNotNull = false;
				return;
			}
			this.m_value = XmlConvert.ToInt64(reader.ReadElementString());
			this.m_fNotNull = true;
		}

		// Token: 0x06002E4C RID: 11852 RVA: 0x002AD1F8 File Offset: 0x002AC5F8
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			if (this.IsNull)
			{
				writer.WriteAttributeString("xsi", "nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
				return;
			}
			writer.WriteString(XmlConvert.ToString(this.m_value));
		}

		// Token: 0x06002E4D RID: 11853 RVA: 0x002AD23C File Offset: 0x002AC63C
		public static XmlQualifiedName GetXsdType(XmlSchemaSet schemaSet)
		{
			return new XmlQualifiedName("long", "http://www.w3.org/2001/XMLSchema");
		}

		// Token: 0x04001CFE RID: 7422
		private bool m_fNotNull;

		// Token: 0x04001CFF RID: 7423
		private long m_value;

		// Token: 0x04001D00 RID: 7424
		private static readonly long x_lLowIntMask = (long)((ulong)(-1));

		// Token: 0x04001D01 RID: 7425
		private static readonly long x_lHighIntMask = -4294967296L;

		// Token: 0x04001D02 RID: 7426
		public static readonly SqlInt64 Null = new SqlInt64(true);

		// Token: 0x04001D03 RID: 7427
		public static readonly SqlInt64 Zero = new SqlInt64(0L);

		// Token: 0x04001D04 RID: 7428
		public static readonly SqlInt64 MinValue = new SqlInt64(long.MinValue);

		// Token: 0x04001D05 RID: 7429
		public static readonly SqlInt64 MaxValue = new SqlInt64(long.MaxValue);
	}
}
