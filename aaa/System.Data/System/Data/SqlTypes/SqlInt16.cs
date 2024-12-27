using System;
using System.Data.Common;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes
{
	// Token: 0x02000350 RID: 848
	[XmlSchemaProvider("GetXsdType")]
	[Serializable]
	public struct SqlInt16 : INullable, IComparable, IXmlSerializable
	{
		// Token: 0x06002D84 RID: 11652 RVA: 0x002AAE94 File Offset: 0x002AA294
		private SqlInt16(bool fNull)
		{
			this.m_fNotNull = false;
			this.m_value = 0;
		}

		// Token: 0x06002D85 RID: 11653 RVA: 0x002AAEB0 File Offset: 0x002AA2B0
		public SqlInt16(short value)
		{
			this.m_value = value;
			this.m_fNotNull = true;
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06002D86 RID: 11654 RVA: 0x002AAECC File Offset: 0x002AA2CC
		public bool IsNull
		{
			get
			{
				return !this.m_fNotNull;
			}
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06002D87 RID: 11655 RVA: 0x002AAEE4 File Offset: 0x002AA2E4
		public short Value
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

		// Token: 0x06002D88 RID: 11656 RVA: 0x002AAF08 File Offset: 0x002AA308
		public static implicit operator SqlInt16(short x)
		{
			return new SqlInt16(x);
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x002AAF1C File Offset: 0x002AA31C
		public static explicit operator short(SqlInt16 x)
		{
			return x.Value;
		}

		// Token: 0x06002D8A RID: 11658 RVA: 0x002AAF30 File Offset: 0x002AA330
		public override string ToString()
		{
			if (!this.IsNull)
			{
				return this.m_value.ToString(null);
			}
			return SQLResource.NullString;
		}

		// Token: 0x06002D8B RID: 11659 RVA: 0x002AAF58 File Offset: 0x002AA358
		public static SqlInt16 Parse(string s)
		{
			if (s == SQLResource.NullString)
			{
				return SqlInt16.Null;
			}
			return new SqlInt16(short.Parse(s, null));
		}

		// Token: 0x06002D8C RID: 11660 RVA: 0x002AAF84 File Offset: 0x002AA384
		public static SqlInt16 operator -(SqlInt16 x)
		{
			if (!x.IsNull)
			{
				return new SqlInt16(-x.m_value);
			}
			return SqlInt16.Null;
		}

		// Token: 0x06002D8D RID: 11661 RVA: 0x002AAFB0 File Offset: 0x002AA3B0
		public static SqlInt16 operator ~(SqlInt16 x)
		{
			if (!x.IsNull)
			{
				return new SqlInt16(~x.m_value);
			}
			return SqlInt16.Null;
		}

		// Token: 0x06002D8E RID: 11662 RVA: 0x002AAFDC File Offset: 0x002AA3DC
		public static SqlInt16 operator +(SqlInt16 x, SqlInt16 y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlInt16.Null;
			}
			int num = (int)(x.m_value + y.m_value);
			if ((((num >> 15) ^ (num >> 16)) & 1) != 0)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlInt16((short)num);
		}

		// Token: 0x06002D8F RID: 11663 RVA: 0x002AB030 File Offset: 0x002AA430
		public static SqlInt16 operator -(SqlInt16 x, SqlInt16 y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlInt16.Null;
			}
			int num = (int)(x.m_value - y.m_value);
			if ((((num >> 15) ^ (num >> 16)) & 1) != 0)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlInt16((short)num);
		}

		// Token: 0x06002D90 RID: 11664 RVA: 0x002AB084 File Offset: 0x002AA484
		public static SqlInt16 operator *(SqlInt16 x, SqlInt16 y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlInt16.Null;
			}
			int num = (int)(x.m_value * y.m_value);
			int num2 = num & SqlInt16.O_MASKI2;
			if (num2 != 0 && num2 != SqlInt16.O_MASKI2)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlInt16((short)num);
		}

		// Token: 0x06002D91 RID: 11665 RVA: 0x002AB0E0 File Offset: 0x002AA4E0
		public static SqlInt16 operator /(SqlInt16 x, SqlInt16 y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlInt16.Null;
			}
			if (y.m_value == 0)
			{
				throw new DivideByZeroException(SQLResource.DivideByZeroMessage);
			}
			if (x.m_value == -32768 && y.m_value == -1)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlInt16(x.m_value / y.m_value);
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x002AB154 File Offset: 0x002AA554
		public static SqlInt16 operator %(SqlInt16 x, SqlInt16 y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlInt16.Null;
			}
			if (y.m_value == 0)
			{
				throw new DivideByZeroException(SQLResource.DivideByZeroMessage);
			}
			if (x.m_value == -32768 && y.m_value == -1)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlInt16(x.m_value % y.m_value);
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x002AB1C8 File Offset: 0x002AA5C8
		public static SqlInt16 operator &(SqlInt16 x, SqlInt16 y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlInt16(x.m_value & y.m_value);
			}
			return SqlInt16.Null;
		}

		// Token: 0x06002D94 RID: 11668 RVA: 0x002AB204 File Offset: 0x002AA604
		public static SqlInt16 operator |(SqlInt16 x, SqlInt16 y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlInt16((short)((ushort)x.m_value | (ushort)y.m_value));
			}
			return SqlInt16.Null;
		}

		// Token: 0x06002D95 RID: 11669 RVA: 0x002AB240 File Offset: 0x002AA640
		public static SqlInt16 operator ^(SqlInt16 x, SqlInt16 y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlInt16(x.m_value ^ y.m_value);
			}
			return SqlInt16.Null;
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x002AB27C File Offset: 0x002AA67C
		public static explicit operator SqlInt16(SqlBoolean x)
		{
			if (!x.IsNull)
			{
				return new SqlInt16((short)x.ByteValue);
			}
			return SqlInt16.Null;
		}

		// Token: 0x06002D97 RID: 11671 RVA: 0x002AB2A4 File Offset: 0x002AA6A4
		public static implicit operator SqlInt16(SqlByte x)
		{
			if (!x.IsNull)
			{
				return new SqlInt16((short)x.Value);
			}
			return SqlInt16.Null;
		}

		// Token: 0x06002D98 RID: 11672 RVA: 0x002AB2CC File Offset: 0x002AA6CC
		public static explicit operator SqlInt16(SqlInt32 x)
		{
			if (x.IsNull)
			{
				return SqlInt16.Null;
			}
			int value = x.Value;
			if (value > 32767 || value < -32768)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlInt16((short)value);
		}

		// Token: 0x06002D99 RID: 11673 RVA: 0x002AB314 File Offset: 0x002AA714
		public static explicit operator SqlInt16(SqlInt64 x)
		{
			if (x.IsNull)
			{
				return SqlInt16.Null;
			}
			long value = x.Value;
			if (value > 32767L || value < -32768L)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlInt16((short)value);
		}

		// Token: 0x06002D9A RID: 11674 RVA: 0x002AB35C File Offset: 0x002AA75C
		public static explicit operator SqlInt16(SqlSingle x)
		{
			if (x.IsNull)
			{
				return SqlInt16.Null;
			}
			float value = x.Value;
			if (value < -32768f || value > 32767f)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlInt16((short)value);
		}

		// Token: 0x06002D9B RID: 11675 RVA: 0x002AB3A4 File Offset: 0x002AA7A4
		public static explicit operator SqlInt16(SqlDouble x)
		{
			if (x.IsNull)
			{
				return SqlInt16.Null;
			}
			double value = x.Value;
			if (value < -32768.0 || value > 32767.0)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlInt16((short)value);
		}

		// Token: 0x06002D9C RID: 11676 RVA: 0x002AB3F4 File Offset: 0x002AA7F4
		public static explicit operator SqlInt16(SqlMoney x)
		{
			if (!x.IsNull)
			{
				return new SqlInt16(checked((short)x.ToInt32()));
			}
			return SqlInt16.Null;
		}

		// Token: 0x06002D9D RID: 11677 RVA: 0x002AB420 File Offset: 0x002AA820
		public static explicit operator SqlInt16(SqlDecimal x)
		{
			return (SqlInt16)((SqlInt32)x);
		}

		// Token: 0x06002D9E RID: 11678 RVA: 0x002AB438 File Offset: 0x002AA838
		public static explicit operator SqlInt16(SqlString x)
		{
			if (!x.IsNull)
			{
				return new SqlInt16(short.Parse(x.Value, null));
			}
			return SqlInt16.Null;
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x002AB468 File Offset: 0x002AA868
		public static SqlBoolean operator ==(SqlInt16 x, SqlInt16 y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value == y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x002AB4A4 File Offset: 0x002AA8A4
		public static SqlBoolean operator !=(SqlInt16 x, SqlInt16 y)
		{
			return !(x == y);
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x002AB4C0 File Offset: 0x002AA8C0
		public static SqlBoolean operator <(SqlInt16 x, SqlInt16 y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value < y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002DA2 RID: 11682 RVA: 0x002AB4FC File Offset: 0x002AA8FC
		public static SqlBoolean operator >(SqlInt16 x, SqlInt16 y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value > y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x002AB538 File Offset: 0x002AA938
		public static SqlBoolean operator <=(SqlInt16 x, SqlInt16 y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value <= y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x002AB578 File Offset: 0x002AA978
		public static SqlBoolean operator >=(SqlInt16 x, SqlInt16 y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value >= y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002DA5 RID: 11685 RVA: 0x002AB5B8 File Offset: 0x002AA9B8
		public static SqlInt16 OnesComplement(SqlInt16 x)
		{
			return ~x;
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x002AB5CC File Offset: 0x002AA9CC
		public static SqlInt16 Add(SqlInt16 x, SqlInt16 y)
		{
			return x + y;
		}

		// Token: 0x06002DA7 RID: 11687 RVA: 0x002AB5E0 File Offset: 0x002AA9E0
		public static SqlInt16 Subtract(SqlInt16 x, SqlInt16 y)
		{
			return x - y;
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x002AB5F4 File Offset: 0x002AA9F4
		public static SqlInt16 Multiply(SqlInt16 x, SqlInt16 y)
		{
			return x * y;
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x002AB608 File Offset: 0x002AAA08
		public static SqlInt16 Divide(SqlInt16 x, SqlInt16 y)
		{
			return x / y;
		}

		// Token: 0x06002DAA RID: 11690 RVA: 0x002AB61C File Offset: 0x002AAA1C
		public static SqlInt16 Mod(SqlInt16 x, SqlInt16 y)
		{
			return x % y;
		}

		// Token: 0x06002DAB RID: 11691 RVA: 0x002AB630 File Offset: 0x002AAA30
		public static SqlInt16 Modulus(SqlInt16 x, SqlInt16 y)
		{
			return x % y;
		}

		// Token: 0x06002DAC RID: 11692 RVA: 0x002AB644 File Offset: 0x002AAA44
		public static SqlInt16 BitwiseAnd(SqlInt16 x, SqlInt16 y)
		{
			return x & y;
		}

		// Token: 0x06002DAD RID: 11693 RVA: 0x002AB658 File Offset: 0x002AAA58
		public static SqlInt16 BitwiseOr(SqlInt16 x, SqlInt16 y)
		{
			return x | y;
		}

		// Token: 0x06002DAE RID: 11694 RVA: 0x002AB66C File Offset: 0x002AAA6C
		public static SqlInt16 Xor(SqlInt16 x, SqlInt16 y)
		{
			return x ^ y;
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x002AB680 File Offset: 0x002AAA80
		public static SqlBoolean Equals(SqlInt16 x, SqlInt16 y)
		{
			return x == y;
		}

		// Token: 0x06002DB0 RID: 11696 RVA: 0x002AB694 File Offset: 0x002AAA94
		public static SqlBoolean NotEquals(SqlInt16 x, SqlInt16 y)
		{
			return x != y;
		}

		// Token: 0x06002DB1 RID: 11697 RVA: 0x002AB6A8 File Offset: 0x002AAAA8
		public static SqlBoolean LessThan(SqlInt16 x, SqlInt16 y)
		{
			return x < y;
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x002AB6BC File Offset: 0x002AAABC
		public static SqlBoolean GreaterThan(SqlInt16 x, SqlInt16 y)
		{
			return x > y;
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x002AB6D0 File Offset: 0x002AAAD0
		public static SqlBoolean LessThanOrEqual(SqlInt16 x, SqlInt16 y)
		{
			return x <= y;
		}

		// Token: 0x06002DB4 RID: 11700 RVA: 0x002AB6E4 File Offset: 0x002AAAE4
		public static SqlBoolean GreaterThanOrEqual(SqlInt16 x, SqlInt16 y)
		{
			return x >= y;
		}

		// Token: 0x06002DB5 RID: 11701 RVA: 0x002AB6F8 File Offset: 0x002AAAF8
		public SqlBoolean ToSqlBoolean()
		{
			return (SqlBoolean)this;
		}

		// Token: 0x06002DB6 RID: 11702 RVA: 0x002AB710 File Offset: 0x002AAB10
		public SqlByte ToSqlByte()
		{
			return (SqlByte)this;
		}

		// Token: 0x06002DB7 RID: 11703 RVA: 0x002AB728 File Offset: 0x002AAB28
		public SqlDouble ToSqlDouble()
		{
			return this;
		}

		// Token: 0x06002DB8 RID: 11704 RVA: 0x002AB740 File Offset: 0x002AAB40
		public SqlInt32 ToSqlInt32()
		{
			return this;
		}

		// Token: 0x06002DB9 RID: 11705 RVA: 0x002AB758 File Offset: 0x002AAB58
		public SqlInt64 ToSqlInt64()
		{
			return this;
		}

		// Token: 0x06002DBA RID: 11706 RVA: 0x002AB770 File Offset: 0x002AAB70
		public SqlMoney ToSqlMoney()
		{
			return this;
		}

		// Token: 0x06002DBB RID: 11707 RVA: 0x002AB788 File Offset: 0x002AAB88
		public SqlDecimal ToSqlDecimal()
		{
			return this;
		}

		// Token: 0x06002DBC RID: 11708 RVA: 0x002AB7A0 File Offset: 0x002AABA0
		public SqlSingle ToSqlSingle()
		{
			return this;
		}

		// Token: 0x06002DBD RID: 11709 RVA: 0x002AB7B8 File Offset: 0x002AABB8
		public SqlString ToSqlString()
		{
			return (SqlString)this;
		}

		// Token: 0x06002DBE RID: 11710 RVA: 0x002AB7D0 File Offset: 0x002AABD0
		public int CompareTo(object value)
		{
			if (value is SqlInt16)
			{
				SqlInt16 sqlInt = (SqlInt16)value;
				return this.CompareTo(sqlInt);
			}
			throw ADP.WrongType(value.GetType(), typeof(SqlInt16));
		}

		// Token: 0x06002DBF RID: 11711 RVA: 0x002AB80C File Offset: 0x002AAC0C
		public int CompareTo(SqlInt16 value)
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

		// Token: 0x06002DC0 RID: 11712 RVA: 0x002AB864 File Offset: 0x002AAC64
		public override bool Equals(object value)
		{
			if (!(value is SqlInt16))
			{
				return false;
			}
			SqlInt16 sqlInt = (SqlInt16)value;
			if (sqlInt.IsNull || this.IsNull)
			{
				return sqlInt.IsNull && this.IsNull;
			}
			return (this == sqlInt).Value;
		}

		// Token: 0x06002DC1 RID: 11713 RVA: 0x002AB8BC File Offset: 0x002AACBC
		public override int GetHashCode()
		{
			if (!this.IsNull)
			{
				return this.Value.GetHashCode();
			}
			return 0;
		}

		// Token: 0x06002DC2 RID: 11714 RVA: 0x002AB8E4 File Offset: 0x002AACE4
		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		// Token: 0x06002DC3 RID: 11715 RVA: 0x002AB8F4 File Offset: 0x002AACF4
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			string attribute = reader.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance");
			if (attribute != null && XmlConvert.ToBoolean(attribute))
			{
				this.m_fNotNull = false;
				return;
			}
			this.m_value = XmlConvert.ToInt16(reader.ReadElementString());
			this.m_fNotNull = true;
		}

		// Token: 0x06002DC4 RID: 11716 RVA: 0x002AB940 File Offset: 0x002AAD40
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			if (this.IsNull)
			{
				writer.WriteAttributeString("xsi", "nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
				return;
			}
			writer.WriteString(XmlConvert.ToString(this.m_value));
		}

		// Token: 0x06002DC5 RID: 11717 RVA: 0x002AB984 File Offset: 0x002AAD84
		public static XmlQualifiedName GetXsdType(XmlSchemaSet schemaSet)
		{
			return new XmlQualifiedName("short", "http://www.w3.org/2001/XMLSchema");
		}

		// Token: 0x04001CEF RID: 7407
		private bool m_fNotNull;

		// Token: 0x04001CF0 RID: 7408
		private short m_value;

		// Token: 0x04001CF1 RID: 7409
		private static readonly int O_MASKI2 = -32768;

		// Token: 0x04001CF2 RID: 7410
		public static readonly SqlInt16 Null = new SqlInt16(true);

		// Token: 0x04001CF3 RID: 7411
		public static readonly SqlInt16 Zero = new SqlInt16(0);

		// Token: 0x04001CF4 RID: 7412
		public static readonly SqlInt16 MinValue = new SqlInt16(short.MinValue);

		// Token: 0x04001CF5 RID: 7413
		public static readonly SqlInt16 MaxValue = new SqlInt16(short.MaxValue);
	}
}
