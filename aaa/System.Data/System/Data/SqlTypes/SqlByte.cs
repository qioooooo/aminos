using System;
using System.Data.Common;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes
{
	// Token: 0x02000342 RID: 834
	[XmlSchemaProvider("GetXsdType")]
	[Serializable]
	public struct SqlByte : INullable, IComparable, IXmlSerializable
	{
		// Token: 0x06002BAE RID: 11182 RVA: 0x002A2B70 File Offset: 0x002A1F70
		private SqlByte(bool fNull)
		{
			this.m_fNotNull = false;
			this.m_value = 0;
		}

		// Token: 0x06002BAF RID: 11183 RVA: 0x002A2B8C File Offset: 0x002A1F8C
		public SqlByte(byte value)
		{
			this.m_value = value;
			this.m_fNotNull = true;
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06002BB0 RID: 11184 RVA: 0x002A2BA8 File Offset: 0x002A1FA8
		public bool IsNull
		{
			get
			{
				return !this.m_fNotNull;
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06002BB1 RID: 11185 RVA: 0x002A2BC0 File Offset: 0x002A1FC0
		public byte Value
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

		// Token: 0x06002BB2 RID: 11186 RVA: 0x002A2BE4 File Offset: 0x002A1FE4
		public static implicit operator SqlByte(byte x)
		{
			return new SqlByte(x);
		}

		// Token: 0x06002BB3 RID: 11187 RVA: 0x002A2BF8 File Offset: 0x002A1FF8
		public static explicit operator byte(SqlByte x)
		{
			return x.Value;
		}

		// Token: 0x06002BB4 RID: 11188 RVA: 0x002A2C0C File Offset: 0x002A200C
		public override string ToString()
		{
			if (!this.IsNull)
			{
				return this.m_value.ToString(null);
			}
			return SQLResource.NullString;
		}

		// Token: 0x06002BB5 RID: 11189 RVA: 0x002A2C34 File Offset: 0x002A2034
		public static SqlByte Parse(string s)
		{
			if (s == SQLResource.NullString)
			{
				return SqlByte.Null;
			}
			return new SqlByte(byte.Parse(s, null));
		}

		// Token: 0x06002BB6 RID: 11190 RVA: 0x002A2C60 File Offset: 0x002A2060
		public static SqlByte operator ~(SqlByte x)
		{
			if (!x.IsNull)
			{
				return new SqlByte(~x.m_value);
			}
			return SqlByte.Null;
		}

		// Token: 0x06002BB7 RID: 11191 RVA: 0x002A2C8C File Offset: 0x002A208C
		public static SqlByte operator +(SqlByte x, SqlByte y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlByte.Null;
			}
			int num = (int)(x.m_value + y.m_value);
			if ((num & SqlByte.x_iBitNotByteMax) != 0)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlByte((byte)num);
		}

		// Token: 0x06002BB8 RID: 11192 RVA: 0x002A2CDC File Offset: 0x002A20DC
		public static SqlByte operator -(SqlByte x, SqlByte y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlByte.Null;
			}
			int num = (int)(x.m_value - y.m_value);
			if ((num & SqlByte.x_iBitNotByteMax) != 0)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlByte((byte)num);
		}

		// Token: 0x06002BB9 RID: 11193 RVA: 0x002A2D2C File Offset: 0x002A212C
		public static SqlByte operator *(SqlByte x, SqlByte y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlByte.Null;
			}
			int num = (int)(x.m_value * y.m_value);
			if ((num & SqlByte.x_iBitNotByteMax) != 0)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlByte((byte)num);
		}

		// Token: 0x06002BBA RID: 11194 RVA: 0x002A2D7C File Offset: 0x002A217C
		public static SqlByte operator /(SqlByte x, SqlByte y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlByte.Null;
			}
			if (y.m_value != 0)
			{
				return new SqlByte(x.m_value / y.m_value);
			}
			throw new DivideByZeroException(SQLResource.DivideByZeroMessage);
		}

		// Token: 0x06002BBB RID: 11195 RVA: 0x002A2DCC File Offset: 0x002A21CC
		public static SqlByte operator %(SqlByte x, SqlByte y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlByte.Null;
			}
			if (y.m_value != 0)
			{
				return new SqlByte(x.m_value % y.m_value);
			}
			throw new DivideByZeroException(SQLResource.DivideByZeroMessage);
		}

		// Token: 0x06002BBC RID: 11196 RVA: 0x002A2E1C File Offset: 0x002A221C
		public static SqlByte operator &(SqlByte x, SqlByte y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlByte(x.m_value & y.m_value);
			}
			return SqlByte.Null;
		}

		// Token: 0x06002BBD RID: 11197 RVA: 0x002A2E58 File Offset: 0x002A2258
		public static SqlByte operator |(SqlByte x, SqlByte y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlByte(x.m_value | y.m_value);
			}
			return SqlByte.Null;
		}

		// Token: 0x06002BBE RID: 11198 RVA: 0x002A2E94 File Offset: 0x002A2294
		public static SqlByte operator ^(SqlByte x, SqlByte y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlByte(x.m_value ^ y.m_value);
			}
			return SqlByte.Null;
		}

		// Token: 0x06002BBF RID: 11199 RVA: 0x002A2ED0 File Offset: 0x002A22D0
		public static explicit operator SqlByte(SqlBoolean x)
		{
			if (!x.IsNull)
			{
				return new SqlByte(x.ByteValue);
			}
			return SqlByte.Null;
		}

		// Token: 0x06002BC0 RID: 11200 RVA: 0x002A2EF8 File Offset: 0x002A22F8
		public static explicit operator SqlByte(SqlMoney x)
		{
			if (!x.IsNull)
			{
				return new SqlByte(checked((byte)x.ToInt32()));
			}
			return SqlByte.Null;
		}

		// Token: 0x06002BC1 RID: 11201 RVA: 0x002A2F24 File Offset: 0x002A2324
		public static explicit operator SqlByte(SqlInt16 x)
		{
			if (x.IsNull)
			{
				return SqlByte.Null;
			}
			if (x.Value > 255 || x.Value < 0)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			if (!x.IsNull)
			{
				return new SqlByte((byte)x.Value);
			}
			return SqlByte.Null;
		}

		// Token: 0x06002BC2 RID: 11202 RVA: 0x002A2F80 File Offset: 0x002A2380
		public static explicit operator SqlByte(SqlInt32 x)
		{
			if (x.IsNull)
			{
				return SqlByte.Null;
			}
			if (x.Value > 255 || x.Value < 0)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			if (!x.IsNull)
			{
				return new SqlByte((byte)x.Value);
			}
			return SqlByte.Null;
		}

		// Token: 0x06002BC3 RID: 11203 RVA: 0x002A2FDC File Offset: 0x002A23DC
		public static explicit operator SqlByte(SqlInt64 x)
		{
			if (x.IsNull)
			{
				return SqlByte.Null;
			}
			if (x.Value > 255L || x.Value < 0L)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			if (!x.IsNull)
			{
				return new SqlByte((byte)x.Value);
			}
			return SqlByte.Null;
		}

		// Token: 0x06002BC4 RID: 11204 RVA: 0x002A303C File Offset: 0x002A243C
		public static explicit operator SqlByte(SqlSingle x)
		{
			if (x.IsNull)
			{
				return SqlByte.Null;
			}
			if (x.Value > 255f || x.Value < 0f)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			if (!x.IsNull)
			{
				return new SqlByte((byte)x.Value);
			}
			return SqlByte.Null;
		}

		// Token: 0x06002BC5 RID: 11205 RVA: 0x002A309C File Offset: 0x002A249C
		public static explicit operator SqlByte(SqlDouble x)
		{
			if (x.IsNull)
			{
				return SqlByte.Null;
			}
			if (x.Value > 255.0 || x.Value < 0.0)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			if (!x.IsNull)
			{
				return new SqlByte((byte)x.Value);
			}
			return SqlByte.Null;
		}

		// Token: 0x06002BC6 RID: 11206 RVA: 0x002A3104 File Offset: 0x002A2504
		public static explicit operator SqlByte(SqlDecimal x)
		{
			return (SqlByte)((SqlInt32)x);
		}

		// Token: 0x06002BC7 RID: 11207 RVA: 0x002A311C File Offset: 0x002A251C
		public static explicit operator SqlByte(SqlString x)
		{
			if (!x.IsNull)
			{
				return new SqlByte(byte.Parse(x.Value, null));
			}
			return SqlByte.Null;
		}

		// Token: 0x06002BC8 RID: 11208 RVA: 0x002A314C File Offset: 0x002A254C
		public static SqlBoolean operator ==(SqlByte x, SqlByte y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value == y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002BC9 RID: 11209 RVA: 0x002A3188 File Offset: 0x002A2588
		public static SqlBoolean operator !=(SqlByte x, SqlByte y)
		{
			return !(x == y);
		}

		// Token: 0x06002BCA RID: 11210 RVA: 0x002A31A4 File Offset: 0x002A25A4
		public static SqlBoolean operator <(SqlByte x, SqlByte y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value < y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002BCB RID: 11211 RVA: 0x002A31E0 File Offset: 0x002A25E0
		public static SqlBoolean operator >(SqlByte x, SqlByte y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value > y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002BCC RID: 11212 RVA: 0x002A321C File Offset: 0x002A261C
		public static SqlBoolean operator <=(SqlByte x, SqlByte y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value <= y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002BCD RID: 11213 RVA: 0x002A325C File Offset: 0x002A265C
		public static SqlBoolean operator >=(SqlByte x, SqlByte y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value >= y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002BCE RID: 11214 RVA: 0x002A329C File Offset: 0x002A269C
		public static SqlByte OnesComplement(SqlByte x)
		{
			return ~x;
		}

		// Token: 0x06002BCF RID: 11215 RVA: 0x002A32B0 File Offset: 0x002A26B0
		public static SqlByte Add(SqlByte x, SqlByte y)
		{
			return x + y;
		}

		// Token: 0x06002BD0 RID: 11216 RVA: 0x002A32C4 File Offset: 0x002A26C4
		public static SqlByte Subtract(SqlByte x, SqlByte y)
		{
			return x - y;
		}

		// Token: 0x06002BD1 RID: 11217 RVA: 0x002A32D8 File Offset: 0x002A26D8
		public static SqlByte Multiply(SqlByte x, SqlByte y)
		{
			return x * y;
		}

		// Token: 0x06002BD2 RID: 11218 RVA: 0x002A32EC File Offset: 0x002A26EC
		public static SqlByte Divide(SqlByte x, SqlByte y)
		{
			return x / y;
		}

		// Token: 0x06002BD3 RID: 11219 RVA: 0x002A3300 File Offset: 0x002A2700
		public static SqlByte Mod(SqlByte x, SqlByte y)
		{
			return x % y;
		}

		// Token: 0x06002BD4 RID: 11220 RVA: 0x002A3314 File Offset: 0x002A2714
		public static SqlByte Modulus(SqlByte x, SqlByte y)
		{
			return x % y;
		}

		// Token: 0x06002BD5 RID: 11221 RVA: 0x002A3328 File Offset: 0x002A2728
		public static SqlByte BitwiseAnd(SqlByte x, SqlByte y)
		{
			return x & y;
		}

		// Token: 0x06002BD6 RID: 11222 RVA: 0x002A333C File Offset: 0x002A273C
		public static SqlByte BitwiseOr(SqlByte x, SqlByte y)
		{
			return x | y;
		}

		// Token: 0x06002BD7 RID: 11223 RVA: 0x002A3350 File Offset: 0x002A2750
		public static SqlByte Xor(SqlByte x, SqlByte y)
		{
			return x ^ y;
		}

		// Token: 0x06002BD8 RID: 11224 RVA: 0x002A3364 File Offset: 0x002A2764
		public static SqlBoolean Equals(SqlByte x, SqlByte y)
		{
			return x == y;
		}

		// Token: 0x06002BD9 RID: 11225 RVA: 0x002A3378 File Offset: 0x002A2778
		public static SqlBoolean NotEquals(SqlByte x, SqlByte y)
		{
			return x != y;
		}

		// Token: 0x06002BDA RID: 11226 RVA: 0x002A338C File Offset: 0x002A278C
		public static SqlBoolean LessThan(SqlByte x, SqlByte y)
		{
			return x < y;
		}

		// Token: 0x06002BDB RID: 11227 RVA: 0x002A33A0 File Offset: 0x002A27A0
		public static SqlBoolean GreaterThan(SqlByte x, SqlByte y)
		{
			return x > y;
		}

		// Token: 0x06002BDC RID: 11228 RVA: 0x002A33B4 File Offset: 0x002A27B4
		public static SqlBoolean LessThanOrEqual(SqlByte x, SqlByte y)
		{
			return x <= y;
		}

		// Token: 0x06002BDD RID: 11229 RVA: 0x002A33C8 File Offset: 0x002A27C8
		public static SqlBoolean GreaterThanOrEqual(SqlByte x, SqlByte y)
		{
			return x >= y;
		}

		// Token: 0x06002BDE RID: 11230 RVA: 0x002A33DC File Offset: 0x002A27DC
		public SqlBoolean ToSqlBoolean()
		{
			return (SqlBoolean)this;
		}

		// Token: 0x06002BDF RID: 11231 RVA: 0x002A33F4 File Offset: 0x002A27F4
		public SqlDouble ToSqlDouble()
		{
			return this;
		}

		// Token: 0x06002BE0 RID: 11232 RVA: 0x002A340C File Offset: 0x002A280C
		public SqlInt16 ToSqlInt16()
		{
			return this;
		}

		// Token: 0x06002BE1 RID: 11233 RVA: 0x002A3424 File Offset: 0x002A2824
		public SqlInt32 ToSqlInt32()
		{
			return this;
		}

		// Token: 0x06002BE2 RID: 11234 RVA: 0x002A343C File Offset: 0x002A283C
		public SqlInt64 ToSqlInt64()
		{
			return this;
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x002A3454 File Offset: 0x002A2854
		public SqlMoney ToSqlMoney()
		{
			return this;
		}

		// Token: 0x06002BE4 RID: 11236 RVA: 0x002A346C File Offset: 0x002A286C
		public SqlDecimal ToSqlDecimal()
		{
			return this;
		}

		// Token: 0x06002BE5 RID: 11237 RVA: 0x002A3484 File Offset: 0x002A2884
		public SqlSingle ToSqlSingle()
		{
			return this;
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x002A349C File Offset: 0x002A289C
		public SqlString ToSqlString()
		{
			return (SqlString)this;
		}

		// Token: 0x06002BE7 RID: 11239 RVA: 0x002A34B4 File Offset: 0x002A28B4
		public int CompareTo(object value)
		{
			if (value is SqlByte)
			{
				SqlByte sqlByte = (SqlByte)value;
				return this.CompareTo(sqlByte);
			}
			throw ADP.WrongType(value.GetType(), typeof(SqlByte));
		}

		// Token: 0x06002BE8 RID: 11240 RVA: 0x002A34F0 File Offset: 0x002A28F0
		public int CompareTo(SqlByte value)
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

		// Token: 0x06002BE9 RID: 11241 RVA: 0x002A3548 File Offset: 0x002A2948
		public override bool Equals(object value)
		{
			if (!(value is SqlByte))
			{
				return false;
			}
			SqlByte sqlByte = (SqlByte)value;
			if (sqlByte.IsNull || this.IsNull)
			{
				return sqlByte.IsNull && this.IsNull;
			}
			return (this == sqlByte).Value;
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x002A35A0 File Offset: 0x002A29A0
		public override int GetHashCode()
		{
			if (!this.IsNull)
			{
				return this.Value.GetHashCode();
			}
			return 0;
		}

		// Token: 0x06002BEB RID: 11243 RVA: 0x002A35C8 File Offset: 0x002A29C8
		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		// Token: 0x06002BEC RID: 11244 RVA: 0x002A35D8 File Offset: 0x002A29D8
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			string attribute = reader.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance");
			if (attribute != null && XmlConvert.ToBoolean(attribute))
			{
				this.m_fNotNull = false;
				return;
			}
			this.m_value = XmlConvert.ToByte(reader.ReadElementString());
			this.m_fNotNull = true;
		}

		// Token: 0x06002BED RID: 11245 RVA: 0x002A3624 File Offset: 0x002A2A24
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			if (this.IsNull)
			{
				writer.WriteAttributeString("xsi", "nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
				return;
			}
			writer.WriteString(XmlConvert.ToString(this.m_value));
		}

		// Token: 0x06002BEE RID: 11246 RVA: 0x002A3668 File Offset: 0x002A2A68
		public static XmlQualifiedName GetXsdType(XmlSchemaSet schemaSet)
		{
			return new XmlQualifiedName("unsignedByte", "http://www.w3.org/2001/XMLSchema");
		}

		// Token: 0x04001C5A RID: 7258
		private bool m_fNotNull;

		// Token: 0x04001C5B RID: 7259
		private byte m_value;

		// Token: 0x04001C5C RID: 7260
		private static readonly int x_iBitNotByteMax = -256;

		// Token: 0x04001C5D RID: 7261
		public static readonly SqlByte Null = new SqlByte(true);

		// Token: 0x04001C5E RID: 7262
		public static readonly SqlByte Zero = new SqlByte(0);

		// Token: 0x04001C5F RID: 7263
		public static readonly SqlByte MinValue = new SqlByte(0);

		// Token: 0x04001C60 RID: 7264
		public static readonly SqlByte MaxValue = new SqlByte(byte.MaxValue);
	}
}
