using System;
using System.Data.Common;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes
{
	// Token: 0x02000341 RID: 833
	[XmlSchemaProvider("GetXsdType")]
	[Serializable]
	public struct SqlBoolean : INullable, IComparable, IXmlSerializable
	{
		// Token: 0x06002B70 RID: 11120 RVA: 0x002A219C File Offset: 0x002A159C
		public SqlBoolean(bool value)
		{
			this.m_value = (value ? 2 : 1);
		}

		// Token: 0x06002B71 RID: 11121 RVA: 0x002A21B8 File Offset: 0x002A15B8
		public SqlBoolean(int value)
		{
			this = new SqlBoolean(value, false);
		}

		// Token: 0x06002B72 RID: 11122 RVA: 0x002A21D0 File Offset: 0x002A15D0
		private SqlBoolean(int value, bool fNull)
		{
			if (fNull)
			{
				this.m_value = 0;
				return;
			}
			this.m_value = ((value != 0) ? 2 : 1);
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06002B73 RID: 11123 RVA: 0x002A21F8 File Offset: 0x002A15F8
		public bool IsNull
		{
			get
			{
				return this.m_value == 0;
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x06002B74 RID: 11124 RVA: 0x002A2210 File Offset: 0x002A1610
		public bool Value
		{
			get
			{
				switch (this.m_value)
				{
				case 1:
					return false;
				case 2:
					return true;
				default:
					throw new SqlNullValueException();
				}
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06002B75 RID: 11125 RVA: 0x002A2240 File Offset: 0x002A1640
		public bool IsTrue
		{
			get
			{
				return this.m_value == 2;
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06002B76 RID: 11126 RVA: 0x002A2258 File Offset: 0x002A1658
		public bool IsFalse
		{
			get
			{
				return this.m_value == 1;
			}
		}

		// Token: 0x06002B77 RID: 11127 RVA: 0x002A2270 File Offset: 0x002A1670
		public static implicit operator SqlBoolean(bool x)
		{
			return new SqlBoolean(x);
		}

		// Token: 0x06002B78 RID: 11128 RVA: 0x002A2284 File Offset: 0x002A1684
		public static explicit operator bool(SqlBoolean x)
		{
			return x.Value;
		}

		// Token: 0x06002B79 RID: 11129 RVA: 0x002A2298 File Offset: 0x002A1698
		public static SqlBoolean operator !(SqlBoolean x)
		{
			switch (x.m_value)
			{
			case 1:
				return SqlBoolean.True;
			case 2:
				return SqlBoolean.False;
			default:
				return SqlBoolean.Null;
			}
		}

		// Token: 0x06002B7A RID: 11130 RVA: 0x002A22D0 File Offset: 0x002A16D0
		public static bool operator true(SqlBoolean x)
		{
			return x.IsTrue;
		}

		// Token: 0x06002B7B RID: 11131 RVA: 0x002A22E4 File Offset: 0x002A16E4
		public static bool operator false(SqlBoolean x)
		{
			return x.IsFalse;
		}

		// Token: 0x06002B7C RID: 11132 RVA: 0x002A22F8 File Offset: 0x002A16F8
		public static SqlBoolean operator &(SqlBoolean x, SqlBoolean y)
		{
			if (x.m_value == 1 || y.m_value == 1)
			{
				return SqlBoolean.False;
			}
			if (x.m_value == 2 && y.m_value == 2)
			{
				return SqlBoolean.True;
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002B7D RID: 11133 RVA: 0x002A2340 File Offset: 0x002A1740
		public static SqlBoolean operator |(SqlBoolean x, SqlBoolean y)
		{
			if (x.m_value == 2 || y.m_value == 2)
			{
				return SqlBoolean.True;
			}
			if (x.m_value == 1 && y.m_value == 1)
			{
				return SqlBoolean.False;
			}
			return SqlBoolean.Null;
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06002B7E RID: 11134 RVA: 0x002A2388 File Offset: 0x002A1788
		public byte ByteValue
		{
			get
			{
				if (this.IsNull)
				{
					throw new SqlNullValueException();
				}
				if (this.m_value != 2)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x06002B7F RID: 11135 RVA: 0x002A23B0 File Offset: 0x002A17B0
		public override string ToString()
		{
			if (!this.IsNull)
			{
				return this.Value.ToString(null);
			}
			return SQLResource.NullString;
		}

		// Token: 0x06002B80 RID: 11136 RVA: 0x002A23DC File Offset: 0x002A17DC
		public static SqlBoolean Parse(string s)
		{
			if (s == null)
			{
				return new SqlBoolean(bool.Parse(s));
			}
			if (s == SQLResource.NullString)
			{
				return SqlBoolean.Null;
			}
			s = s.TrimStart(new char[0]);
			char c = s[0];
			if (char.IsNumber(c) || '-' == c || '+' == c)
			{
				return new SqlBoolean(int.Parse(s, null));
			}
			return new SqlBoolean(bool.Parse(s));
		}

		// Token: 0x06002B81 RID: 11137 RVA: 0x002A244C File Offset: 0x002A184C
		public static SqlBoolean operator ~(SqlBoolean x)
		{
			return !x;
		}

		// Token: 0x06002B82 RID: 11138 RVA: 0x002A2460 File Offset: 0x002A1860
		public static SqlBoolean operator ^(SqlBoolean x, SqlBoolean y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value != y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002B83 RID: 11139 RVA: 0x002A24A0 File Offset: 0x002A18A0
		public static explicit operator SqlBoolean(SqlByte x)
		{
			if (!x.IsNull)
			{
				return new SqlBoolean(x.Value != 0);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002B84 RID: 11140 RVA: 0x002A24D0 File Offset: 0x002A18D0
		public static explicit operator SqlBoolean(SqlInt16 x)
		{
			if (!x.IsNull)
			{
				return new SqlBoolean(x.Value != 0);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002B85 RID: 11141 RVA: 0x002A2500 File Offset: 0x002A1900
		public static explicit operator SqlBoolean(SqlInt32 x)
		{
			if (!x.IsNull)
			{
				return new SqlBoolean(x.Value != 0);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002B86 RID: 11142 RVA: 0x002A2530 File Offset: 0x002A1930
		public static explicit operator SqlBoolean(SqlInt64 x)
		{
			if (!x.IsNull)
			{
				return new SqlBoolean(x.Value != 0L);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002B87 RID: 11143 RVA: 0x002A2560 File Offset: 0x002A1960
		public static explicit operator SqlBoolean(SqlDouble x)
		{
			if (!x.IsNull)
			{
				return new SqlBoolean(x.Value != 0.0);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002B88 RID: 11144 RVA: 0x002A2598 File Offset: 0x002A1998
		public static explicit operator SqlBoolean(SqlSingle x)
		{
			if (!x.IsNull)
			{
				return new SqlBoolean((double)x.Value != 0.0);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002B89 RID: 11145 RVA: 0x002A25D0 File Offset: 0x002A19D0
		public static explicit operator SqlBoolean(SqlMoney x)
		{
			if (!x.IsNull)
			{
				return x != SqlMoney.Zero;
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002B8A RID: 11146 RVA: 0x002A25F8 File Offset: 0x002A19F8
		public static explicit operator SqlBoolean(SqlDecimal x)
		{
			if (!x.IsNull)
			{
				return new SqlBoolean(x.m_data1 != 0U || x.m_data2 != 0U || x.m_data3 != 0U || x.m_data4 != 0U);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002B8B RID: 11147 RVA: 0x002A2644 File Offset: 0x002A1A44
		public static explicit operator SqlBoolean(SqlString x)
		{
			if (!x.IsNull)
			{
				return SqlBoolean.Parse(x.Value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002B8C RID: 11148 RVA: 0x002A266C File Offset: 0x002A1A6C
		public static SqlBoolean operator ==(SqlBoolean x, SqlBoolean y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value == y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002B8D RID: 11149 RVA: 0x002A26A8 File Offset: 0x002A1AA8
		public static SqlBoolean operator !=(SqlBoolean x, SqlBoolean y)
		{
			return !(x == y);
		}

		// Token: 0x06002B8E RID: 11150 RVA: 0x002A26C4 File Offset: 0x002A1AC4
		public static SqlBoolean operator <(SqlBoolean x, SqlBoolean y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value < y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002B8F RID: 11151 RVA: 0x002A2700 File Offset: 0x002A1B00
		public static SqlBoolean operator >(SqlBoolean x, SqlBoolean y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value > y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002B90 RID: 11152 RVA: 0x002A273C File Offset: 0x002A1B3C
		public static SqlBoolean operator <=(SqlBoolean x, SqlBoolean y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value <= y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002B91 RID: 11153 RVA: 0x002A277C File Offset: 0x002A1B7C
		public static SqlBoolean operator >=(SqlBoolean x, SqlBoolean y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value >= y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002B92 RID: 11154 RVA: 0x002A27BC File Offset: 0x002A1BBC
		public static SqlBoolean OnesComplement(SqlBoolean x)
		{
			return ~x;
		}

		// Token: 0x06002B93 RID: 11155 RVA: 0x002A27D0 File Offset: 0x002A1BD0
		public static SqlBoolean And(SqlBoolean x, SqlBoolean y)
		{
			return x & y;
		}

		// Token: 0x06002B94 RID: 11156 RVA: 0x002A27E4 File Offset: 0x002A1BE4
		public static SqlBoolean Or(SqlBoolean x, SqlBoolean y)
		{
			return x | y;
		}

		// Token: 0x06002B95 RID: 11157 RVA: 0x002A27F8 File Offset: 0x002A1BF8
		public static SqlBoolean Xor(SqlBoolean x, SqlBoolean y)
		{
			return x ^ y;
		}

		// Token: 0x06002B96 RID: 11158 RVA: 0x002A280C File Offset: 0x002A1C0C
		public static SqlBoolean Equals(SqlBoolean x, SqlBoolean y)
		{
			return x == y;
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x002A2820 File Offset: 0x002A1C20
		public static SqlBoolean NotEquals(SqlBoolean x, SqlBoolean y)
		{
			return x != y;
		}

		// Token: 0x06002B98 RID: 11160 RVA: 0x002A2834 File Offset: 0x002A1C34
		public static SqlBoolean GreaterThan(SqlBoolean x, SqlBoolean y)
		{
			return x > y;
		}

		// Token: 0x06002B99 RID: 11161 RVA: 0x002A2848 File Offset: 0x002A1C48
		public static SqlBoolean LessThan(SqlBoolean x, SqlBoolean y)
		{
			return x < y;
		}

		// Token: 0x06002B9A RID: 11162 RVA: 0x002A285C File Offset: 0x002A1C5C
		public static SqlBoolean GreaterThanOrEquals(SqlBoolean x, SqlBoolean y)
		{
			return x >= y;
		}

		// Token: 0x06002B9B RID: 11163 RVA: 0x002A2870 File Offset: 0x002A1C70
		public static SqlBoolean LessThanOrEquals(SqlBoolean x, SqlBoolean y)
		{
			return x <= y;
		}

		// Token: 0x06002B9C RID: 11164 RVA: 0x002A2884 File Offset: 0x002A1C84
		public SqlByte ToSqlByte()
		{
			return (SqlByte)this;
		}

		// Token: 0x06002B9D RID: 11165 RVA: 0x002A289C File Offset: 0x002A1C9C
		public SqlDouble ToSqlDouble()
		{
			return (SqlDouble)this;
		}

		// Token: 0x06002B9E RID: 11166 RVA: 0x002A28B4 File Offset: 0x002A1CB4
		public SqlInt16 ToSqlInt16()
		{
			return (SqlInt16)this;
		}

		// Token: 0x06002B9F RID: 11167 RVA: 0x002A28CC File Offset: 0x002A1CCC
		public SqlInt32 ToSqlInt32()
		{
			return (SqlInt32)this;
		}

		// Token: 0x06002BA0 RID: 11168 RVA: 0x002A28E4 File Offset: 0x002A1CE4
		public SqlInt64 ToSqlInt64()
		{
			return (SqlInt64)this;
		}

		// Token: 0x06002BA1 RID: 11169 RVA: 0x002A28FC File Offset: 0x002A1CFC
		public SqlMoney ToSqlMoney()
		{
			return (SqlMoney)this;
		}

		// Token: 0x06002BA2 RID: 11170 RVA: 0x002A2914 File Offset: 0x002A1D14
		public SqlDecimal ToSqlDecimal()
		{
			return (SqlDecimal)this;
		}

		// Token: 0x06002BA3 RID: 11171 RVA: 0x002A292C File Offset: 0x002A1D2C
		public SqlSingle ToSqlSingle()
		{
			return (SqlSingle)this;
		}

		// Token: 0x06002BA4 RID: 11172 RVA: 0x002A2944 File Offset: 0x002A1D44
		public SqlString ToSqlString()
		{
			return (SqlString)this;
		}

		// Token: 0x06002BA5 RID: 11173 RVA: 0x002A295C File Offset: 0x002A1D5C
		public int CompareTo(object value)
		{
			if (value is SqlBoolean)
			{
				SqlBoolean sqlBoolean = (SqlBoolean)value;
				return this.CompareTo(sqlBoolean);
			}
			throw ADP.WrongType(value.GetType(), typeof(SqlBoolean));
		}

		// Token: 0x06002BA6 RID: 11174 RVA: 0x002A2998 File Offset: 0x002A1D98
		public int CompareTo(SqlBoolean value)
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
				if (this.ByteValue < value.ByteValue)
				{
					return -1;
				}
				if (this.ByteValue > value.ByteValue)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x06002BA7 RID: 11175 RVA: 0x002A29E8 File Offset: 0x002A1DE8
		public override bool Equals(object value)
		{
			if (!(value is SqlBoolean))
			{
				return false;
			}
			SqlBoolean sqlBoolean = (SqlBoolean)value;
			if (sqlBoolean.IsNull || this.IsNull)
			{
				return sqlBoolean.IsNull && this.IsNull;
			}
			return (this == sqlBoolean).Value;
		}

		// Token: 0x06002BA8 RID: 11176 RVA: 0x002A2A40 File Offset: 0x002A1E40
		public override int GetHashCode()
		{
			if (!this.IsNull)
			{
				return this.Value.GetHashCode();
			}
			return 0;
		}

		// Token: 0x06002BA9 RID: 11177 RVA: 0x002A2A68 File Offset: 0x002A1E68
		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		// Token: 0x06002BAA RID: 11178 RVA: 0x002A2A78 File Offset: 0x002A1E78
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			string attribute = reader.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance");
			if (attribute != null && XmlConvert.ToBoolean(attribute))
			{
				this.m_value = 0;
				return;
			}
			this.m_value = (XmlConvert.ToBoolean(reader.ReadElementString()) ? 2 : 1);
		}

		// Token: 0x06002BAB RID: 11179 RVA: 0x002A2AC0 File Offset: 0x002A1EC0
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			if (this.IsNull)
			{
				writer.WriteAttributeString("xsi", "nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
				return;
			}
			writer.WriteString((this.m_value == 2) ? "true" : "false");
		}

		// Token: 0x06002BAC RID: 11180 RVA: 0x002A2B0C File Offset: 0x002A1F0C
		public static XmlQualifiedName GetXsdType(XmlSchemaSet schemaSet)
		{
			return new XmlQualifiedName("boolean", "http://www.w3.org/2001/XMLSchema");
		}

		// Token: 0x04001C51 RID: 7249
		private const byte x_Null = 0;

		// Token: 0x04001C52 RID: 7250
		private const byte x_False = 1;

		// Token: 0x04001C53 RID: 7251
		private const byte x_True = 2;

		// Token: 0x04001C54 RID: 7252
		private byte m_value;

		// Token: 0x04001C55 RID: 7253
		public static readonly SqlBoolean True = new SqlBoolean(true);

		// Token: 0x04001C56 RID: 7254
		public static readonly SqlBoolean False = new SqlBoolean(false);

		// Token: 0x04001C57 RID: 7255
		public static readonly SqlBoolean Null = new SqlBoolean(0, true);

		// Token: 0x04001C58 RID: 7256
		public static readonly SqlBoolean Zero = new SqlBoolean(0);

		// Token: 0x04001C59 RID: 7257
		public static readonly SqlBoolean One = new SqlBoolean(1);
	}
}
