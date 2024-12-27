using System;
using System.Data.Common;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes
{
	// Token: 0x02000351 RID: 849
	[XmlSchemaProvider("GetXsdType")]
	[Serializable]
	public struct SqlInt32 : INullable, IComparable, IXmlSerializable
	{
		// Token: 0x06002DC7 RID: 11719 RVA: 0x002AB9EC File Offset: 0x002AADEC
		private SqlInt32(bool fNull)
		{
			this.m_fNotNull = false;
			this.m_value = 0;
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x002ABA08 File Offset: 0x002AAE08
		public SqlInt32(int value)
		{
			this.m_value = value;
			this.m_fNotNull = true;
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06002DC9 RID: 11721 RVA: 0x002ABA24 File Offset: 0x002AAE24
		public bool IsNull
		{
			get
			{
				return !this.m_fNotNull;
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06002DCA RID: 11722 RVA: 0x002ABA3C File Offset: 0x002AAE3C
		public int Value
		{
			get
			{
				if (this.IsNull)
				{
					throw new SqlNullValueException();
				}
				return this.m_value;
			}
		}

		// Token: 0x06002DCB RID: 11723 RVA: 0x002ABA60 File Offset: 0x002AAE60
		public static implicit operator SqlInt32(int x)
		{
			return new SqlInt32(x);
		}

		// Token: 0x06002DCC RID: 11724 RVA: 0x002ABA74 File Offset: 0x002AAE74
		public static explicit operator int(SqlInt32 x)
		{
			return x.Value;
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x002ABA88 File Offset: 0x002AAE88
		public override string ToString()
		{
			if (!this.IsNull)
			{
				return this.m_value.ToString(null);
			}
			return SQLResource.NullString;
		}

		// Token: 0x06002DCE RID: 11726 RVA: 0x002ABAB0 File Offset: 0x002AAEB0
		public static SqlInt32 Parse(string s)
		{
			if (s == SQLResource.NullString)
			{
				return SqlInt32.Null;
			}
			return new SqlInt32(int.Parse(s, null));
		}

		// Token: 0x06002DCF RID: 11727 RVA: 0x002ABADC File Offset: 0x002AAEDC
		public static SqlInt32 operator -(SqlInt32 x)
		{
			if (!x.IsNull)
			{
				return new SqlInt32(-x.m_value);
			}
			return SqlInt32.Null;
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x002ABB08 File Offset: 0x002AAF08
		public static SqlInt32 operator ~(SqlInt32 x)
		{
			if (!x.IsNull)
			{
				return new SqlInt32(~x.m_value);
			}
			return SqlInt32.Null;
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x002ABB34 File Offset: 0x002AAF34
		public static SqlInt32 operator +(SqlInt32 x, SqlInt32 y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlInt32.Null;
			}
			int num = x.m_value + y.m_value;
			if (SqlInt32.SameSignInt(x.m_value, y.m_value) && !SqlInt32.SameSignInt(x.m_value, num))
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlInt32(num);
		}

		// Token: 0x06002DD2 RID: 11730 RVA: 0x002ABBA0 File Offset: 0x002AAFA0
		public static SqlInt32 operator -(SqlInt32 x, SqlInt32 y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlInt32.Null;
			}
			int num = x.m_value - y.m_value;
			if (!SqlInt32.SameSignInt(x.m_value, y.m_value) && SqlInt32.SameSignInt(y.m_value, num))
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlInt32(num);
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x002ABC0C File Offset: 0x002AB00C
		public static SqlInt32 operator *(SqlInt32 x, SqlInt32 y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlInt32.Null;
			}
			long num = (long)x.m_value * (long)y.m_value;
			long num2 = num & SqlInt32.x_lBitNotIntMax;
			if (num2 != 0L && num2 != SqlInt32.x_lBitNotIntMax)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlInt32((int)num);
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x002ABC6C File Offset: 0x002AB06C
		public static SqlInt32 operator /(SqlInt32 x, SqlInt32 y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlInt32.Null;
			}
			if (y.m_value == 0)
			{
				throw new DivideByZeroException(SQLResource.DivideByZeroMessage);
			}
			if ((long)x.m_value == SqlInt32.x_iIntMin && y.m_value == -1)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlInt32(x.m_value / y.m_value);
		}

		// Token: 0x06002DD5 RID: 11733 RVA: 0x002ABCE0 File Offset: 0x002AB0E0
		public static SqlInt32 operator %(SqlInt32 x, SqlInt32 y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlInt32.Null;
			}
			if (y.m_value == 0)
			{
				throw new DivideByZeroException(SQLResource.DivideByZeroMessage);
			}
			if ((long)x.m_value == SqlInt32.x_iIntMin && y.m_value == -1)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlInt32(x.m_value % y.m_value);
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x002ABD54 File Offset: 0x002AB154
		public static SqlInt32 operator &(SqlInt32 x, SqlInt32 y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlInt32(x.m_value & y.m_value);
			}
			return SqlInt32.Null;
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x002ABD90 File Offset: 0x002AB190
		public static SqlInt32 operator |(SqlInt32 x, SqlInt32 y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlInt32(x.m_value | y.m_value);
			}
			return SqlInt32.Null;
		}

		// Token: 0x06002DD8 RID: 11736 RVA: 0x002ABDCC File Offset: 0x002AB1CC
		public static SqlInt32 operator ^(SqlInt32 x, SqlInt32 y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlInt32(x.m_value ^ y.m_value);
			}
			return SqlInt32.Null;
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x002ABE08 File Offset: 0x002AB208
		public static explicit operator SqlInt32(SqlBoolean x)
		{
			if (!x.IsNull)
			{
				return new SqlInt32((int)x.ByteValue);
			}
			return SqlInt32.Null;
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x002ABE30 File Offset: 0x002AB230
		public static implicit operator SqlInt32(SqlByte x)
		{
			if (!x.IsNull)
			{
				return new SqlInt32((int)x.Value);
			}
			return SqlInt32.Null;
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x002ABE58 File Offset: 0x002AB258
		public static implicit operator SqlInt32(SqlInt16 x)
		{
			if (!x.IsNull)
			{
				return new SqlInt32((int)x.Value);
			}
			return SqlInt32.Null;
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x002ABE80 File Offset: 0x002AB280
		public static explicit operator SqlInt32(SqlInt64 x)
		{
			if (x.IsNull)
			{
				return SqlInt32.Null;
			}
			long value = x.Value;
			if (value > 2147483647L || value < -2147483648L)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlInt32((int)value);
		}

		// Token: 0x06002DDD RID: 11741 RVA: 0x002ABEC8 File Offset: 0x002AB2C8
		public static explicit operator SqlInt32(SqlSingle x)
		{
			if (x.IsNull)
			{
				return SqlInt32.Null;
			}
			float value = x.Value;
			if (value > 2.1474836E+09f || value < -2.1474836E+09f)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlInt32((int)value);
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x002ABF10 File Offset: 0x002AB310
		public static explicit operator SqlInt32(SqlDouble x)
		{
			if (x.IsNull)
			{
				return SqlInt32.Null;
			}
			double value = x.Value;
			if (value > 2147483647.0 || value < -2147483648.0)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlInt32((int)value);
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x002ABF60 File Offset: 0x002AB360
		public static explicit operator SqlInt32(SqlMoney x)
		{
			if (!x.IsNull)
			{
				return new SqlInt32(x.ToInt32());
			}
			return SqlInt32.Null;
		}

		// Token: 0x06002DE0 RID: 11744 RVA: 0x002ABF88 File Offset: 0x002AB388
		public static explicit operator SqlInt32(SqlDecimal x)
		{
			if (x.IsNull)
			{
				return SqlInt32.Null;
			}
			x.AdjustScale((int)(-(int)x.Scale), true);
			long num = (long)((ulong)x.m_data1);
			if (!x.IsPositive)
			{
				num = -num;
			}
			if (x.m_bLen > 1 || num > 2147483647L || num < -2147483648L)
			{
				throw new OverflowException(SQLResource.ConversionOverflowMessage);
			}
			return new SqlInt32((int)num);
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x002ABFF8 File Offset: 0x002AB3F8
		public static explicit operator SqlInt32(SqlString x)
		{
			if (!x.IsNull)
			{
				return new SqlInt32(int.Parse(x.Value, null));
			}
			return SqlInt32.Null;
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x002AC028 File Offset: 0x002AB428
		private static bool SameSignInt(int x, int y)
		{
			return ((long)(x ^ y) & (long)((ulong)int.MinValue)) == 0L;
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x002AC044 File Offset: 0x002AB444
		public static SqlBoolean operator ==(SqlInt32 x, SqlInt32 y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value == y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x002AC080 File Offset: 0x002AB480
		public static SqlBoolean operator !=(SqlInt32 x, SqlInt32 y)
		{
			return !(x == y);
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x002AC09C File Offset: 0x002AB49C
		public static SqlBoolean operator <(SqlInt32 x, SqlInt32 y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value < y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002DE6 RID: 11750 RVA: 0x002AC0D8 File Offset: 0x002AB4D8
		public static SqlBoolean operator >(SqlInt32 x, SqlInt32 y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value > y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x002AC114 File Offset: 0x002AB514
		public static SqlBoolean operator <=(SqlInt32 x, SqlInt32 y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value <= y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x002AC154 File Offset: 0x002AB554
		public static SqlBoolean operator >=(SqlInt32 x, SqlInt32 y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value >= y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x002AC194 File Offset: 0x002AB594
		public static SqlInt32 OnesComplement(SqlInt32 x)
		{
			return ~x;
		}

		// Token: 0x06002DEA RID: 11754 RVA: 0x002AC1A8 File Offset: 0x002AB5A8
		public static SqlInt32 Add(SqlInt32 x, SqlInt32 y)
		{
			return x + y;
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x002AC1BC File Offset: 0x002AB5BC
		public static SqlInt32 Subtract(SqlInt32 x, SqlInt32 y)
		{
			return x - y;
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x002AC1D0 File Offset: 0x002AB5D0
		public static SqlInt32 Multiply(SqlInt32 x, SqlInt32 y)
		{
			return x * y;
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x002AC1E4 File Offset: 0x002AB5E4
		public static SqlInt32 Divide(SqlInt32 x, SqlInt32 y)
		{
			return x / y;
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x002AC1F8 File Offset: 0x002AB5F8
		public static SqlInt32 Mod(SqlInt32 x, SqlInt32 y)
		{
			return x % y;
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x002AC20C File Offset: 0x002AB60C
		public static SqlInt32 Modulus(SqlInt32 x, SqlInt32 y)
		{
			return x % y;
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x002AC220 File Offset: 0x002AB620
		public static SqlInt32 BitwiseAnd(SqlInt32 x, SqlInt32 y)
		{
			return x & y;
		}

		// Token: 0x06002DF1 RID: 11761 RVA: 0x002AC234 File Offset: 0x002AB634
		public static SqlInt32 BitwiseOr(SqlInt32 x, SqlInt32 y)
		{
			return x | y;
		}

		// Token: 0x06002DF2 RID: 11762 RVA: 0x002AC248 File Offset: 0x002AB648
		public static SqlInt32 Xor(SqlInt32 x, SqlInt32 y)
		{
			return x ^ y;
		}

		// Token: 0x06002DF3 RID: 11763 RVA: 0x002AC25C File Offset: 0x002AB65C
		public static SqlBoolean Equals(SqlInt32 x, SqlInt32 y)
		{
			return x == y;
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x002AC270 File Offset: 0x002AB670
		public static SqlBoolean NotEquals(SqlInt32 x, SqlInt32 y)
		{
			return x != y;
		}

		// Token: 0x06002DF5 RID: 11765 RVA: 0x002AC284 File Offset: 0x002AB684
		public static SqlBoolean LessThan(SqlInt32 x, SqlInt32 y)
		{
			return x < y;
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x002AC298 File Offset: 0x002AB698
		public static SqlBoolean GreaterThan(SqlInt32 x, SqlInt32 y)
		{
			return x > y;
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x002AC2AC File Offset: 0x002AB6AC
		public static SqlBoolean LessThanOrEqual(SqlInt32 x, SqlInt32 y)
		{
			return x <= y;
		}

		// Token: 0x06002DF8 RID: 11768 RVA: 0x002AC2C0 File Offset: 0x002AB6C0
		public static SqlBoolean GreaterThanOrEqual(SqlInt32 x, SqlInt32 y)
		{
			return x >= y;
		}

		// Token: 0x06002DF9 RID: 11769 RVA: 0x002AC2D4 File Offset: 0x002AB6D4
		public SqlBoolean ToSqlBoolean()
		{
			return (SqlBoolean)this;
		}

		// Token: 0x06002DFA RID: 11770 RVA: 0x002AC2EC File Offset: 0x002AB6EC
		public SqlByte ToSqlByte()
		{
			return (SqlByte)this;
		}

		// Token: 0x06002DFB RID: 11771 RVA: 0x002AC304 File Offset: 0x002AB704
		public SqlDouble ToSqlDouble()
		{
			return this;
		}

		// Token: 0x06002DFC RID: 11772 RVA: 0x002AC31C File Offset: 0x002AB71C
		public SqlInt16 ToSqlInt16()
		{
			return (SqlInt16)this;
		}

		// Token: 0x06002DFD RID: 11773 RVA: 0x002AC334 File Offset: 0x002AB734
		public SqlInt64 ToSqlInt64()
		{
			return this;
		}

		// Token: 0x06002DFE RID: 11774 RVA: 0x002AC34C File Offset: 0x002AB74C
		public SqlMoney ToSqlMoney()
		{
			return this;
		}

		// Token: 0x06002DFF RID: 11775 RVA: 0x002AC364 File Offset: 0x002AB764
		public SqlDecimal ToSqlDecimal()
		{
			return this;
		}

		// Token: 0x06002E00 RID: 11776 RVA: 0x002AC37C File Offset: 0x002AB77C
		public SqlSingle ToSqlSingle()
		{
			return this;
		}

		// Token: 0x06002E01 RID: 11777 RVA: 0x002AC394 File Offset: 0x002AB794
		public SqlString ToSqlString()
		{
			return (SqlString)this;
		}

		// Token: 0x06002E02 RID: 11778 RVA: 0x002AC3AC File Offset: 0x002AB7AC
		public int CompareTo(object value)
		{
			if (value is SqlInt32)
			{
				SqlInt32 sqlInt = (SqlInt32)value;
				return this.CompareTo(sqlInt);
			}
			throw ADP.WrongType(value.GetType(), typeof(SqlInt32));
		}

		// Token: 0x06002E03 RID: 11779 RVA: 0x002AC3E8 File Offset: 0x002AB7E8
		public int CompareTo(SqlInt32 value)
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

		// Token: 0x06002E04 RID: 11780 RVA: 0x002AC440 File Offset: 0x002AB840
		public override bool Equals(object value)
		{
			if (!(value is SqlInt32))
			{
				return false;
			}
			SqlInt32 sqlInt = (SqlInt32)value;
			if (sqlInt.IsNull || this.IsNull)
			{
				return sqlInt.IsNull && this.IsNull;
			}
			return (this == sqlInt).Value;
		}

		// Token: 0x06002E05 RID: 11781 RVA: 0x002AC498 File Offset: 0x002AB898
		public override int GetHashCode()
		{
			if (!this.IsNull)
			{
				return this.Value.GetHashCode();
			}
			return 0;
		}

		// Token: 0x06002E06 RID: 11782 RVA: 0x002AC4C0 File Offset: 0x002AB8C0
		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x002AC4D0 File Offset: 0x002AB8D0
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			string attribute = reader.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance");
			if (attribute != null && XmlConvert.ToBoolean(attribute))
			{
				this.m_fNotNull = false;
				return;
			}
			this.m_value = XmlConvert.ToInt32(reader.ReadElementString());
			this.m_fNotNull = true;
		}

		// Token: 0x06002E08 RID: 11784 RVA: 0x002AC51C File Offset: 0x002AB91C
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			if (this.IsNull)
			{
				writer.WriteAttributeString("xsi", "nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
				return;
			}
			writer.WriteString(XmlConvert.ToString(this.m_value));
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x002AC560 File Offset: 0x002AB960
		public static XmlQualifiedName GetXsdType(XmlSchemaSet schemaSet)
		{
			return new XmlQualifiedName("int", "http://www.w3.org/2001/XMLSchema");
		}

		// Token: 0x04001CF6 RID: 7414
		private bool m_fNotNull;

		// Token: 0x04001CF7 RID: 7415
		private int m_value;

		// Token: 0x04001CF8 RID: 7416
		private static readonly long x_iIntMin = -2147483648L;

		// Token: 0x04001CF9 RID: 7417
		private static readonly long x_lBitNotIntMax = -2147483648L;

		// Token: 0x04001CFA RID: 7418
		public static readonly SqlInt32 Null = new SqlInt32(true);

		// Token: 0x04001CFB RID: 7419
		public static readonly SqlInt32 Zero = new SqlInt32(0);

		// Token: 0x04001CFC RID: 7420
		public static readonly SqlInt32 MinValue = new SqlInt32(int.MinValue);

		// Token: 0x04001CFD RID: 7421
		public static readonly SqlInt32 MaxValue = new SqlInt32(int.MaxValue);
	}
}
