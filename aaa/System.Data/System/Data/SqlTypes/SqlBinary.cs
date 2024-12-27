using System;
using System.Data.Common;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes
{
	// Token: 0x02000340 RID: 832
	[XmlSchemaProvider("GetXsdType")]
	[Serializable]
	public struct SqlBinary : INullable, IComparable, IXmlSerializable
	{
		// Token: 0x06002B4A RID: 11082 RVA: 0x002A1A94 File Offset: 0x002A0E94
		private SqlBinary(bool fNull)
		{
			this.m_value = null;
		}

		// Token: 0x06002B4B RID: 11083 RVA: 0x002A1AA8 File Offset: 0x002A0EA8
		public SqlBinary(byte[] value)
		{
			if (value == null)
			{
				this.m_value = null;
				return;
			}
			this.m_value = new byte[value.Length];
			value.CopyTo(this.m_value, 0);
		}

		// Token: 0x06002B4C RID: 11084 RVA: 0x002A1ADC File Offset: 0x002A0EDC
		internal SqlBinary(byte[] value, bool ignored)
		{
			if (value == null)
			{
				this.m_value = null;
				return;
			}
			this.m_value = value;
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06002B4D RID: 11085 RVA: 0x002A1AFC File Offset: 0x002A0EFC
		public bool IsNull
		{
			get
			{
				return this.m_value == null;
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06002B4E RID: 11086 RVA: 0x002A1B14 File Offset: 0x002A0F14
		public byte[] Value
		{
			get
			{
				if (this.IsNull)
				{
					throw new SqlNullValueException();
				}
				byte[] array = new byte[this.m_value.Length];
				this.m_value.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x17000712 RID: 1810
		public byte this[int index]
		{
			get
			{
				if (this.IsNull)
				{
					throw new SqlNullValueException();
				}
				return this.m_value[index];
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x06002B50 RID: 11088 RVA: 0x002A1B70 File Offset: 0x002A0F70
		public int Length
		{
			get
			{
				if (!this.IsNull)
				{
					return this.m_value.Length;
				}
				throw new SqlNullValueException();
			}
		}

		// Token: 0x06002B51 RID: 11089 RVA: 0x002A1B94 File Offset: 0x002A0F94
		public static implicit operator SqlBinary(byte[] x)
		{
			return new SqlBinary(x);
		}

		// Token: 0x06002B52 RID: 11090 RVA: 0x002A1BA8 File Offset: 0x002A0FA8
		public static explicit operator byte[](SqlBinary x)
		{
			return x.Value;
		}

		// Token: 0x06002B53 RID: 11091 RVA: 0x002A1BBC File Offset: 0x002A0FBC
		public override string ToString()
		{
			if (!this.IsNull)
			{
				return "SqlBinary(" + this.m_value.Length.ToString(CultureInfo.InvariantCulture) + ")";
			}
			return SQLResource.NullString;
		}

		// Token: 0x06002B54 RID: 11092 RVA: 0x002A1BFC File Offset: 0x002A0FFC
		public static SqlBinary operator +(SqlBinary x, SqlBinary y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlBinary.Null;
			}
			byte[] array = new byte[x.Value.Length + y.Value.Length];
			x.Value.CopyTo(array, 0);
			y.Value.CopyTo(array, x.Value.Length);
			return new SqlBinary(array);
		}

		// Token: 0x06002B55 RID: 11093 RVA: 0x002A1C64 File Offset: 0x002A1064
		private static EComparison PerformCompareByte(byte[] x, byte[] y)
		{
			int num = ((x.Length < y.Length) ? x.Length : y.Length);
			int i = 0;
			while (i < num)
			{
				if (x[i] != y[i])
				{
					if (x[i] < y[i])
					{
						return EComparison.LT;
					}
					return EComparison.GT;
				}
				else
				{
					i++;
				}
			}
			if (x.Length == y.Length)
			{
				return EComparison.EQ;
			}
			byte b = 0;
			if (x.Length < y.Length)
			{
				for (i = num; i < y.Length; i++)
				{
					if (y[i] != b)
					{
						return EComparison.LT;
					}
				}
			}
			else
			{
				for (i = num; i < x.Length; i++)
				{
					if (x[i] != b)
					{
						return EComparison.GT;
					}
				}
			}
			return EComparison.EQ;
		}

		// Token: 0x06002B56 RID: 11094 RVA: 0x002A1CE8 File Offset: 0x002A10E8
		public static explicit operator SqlBinary(SqlGuid x)
		{
			if (!x.IsNull)
			{
				return new SqlBinary(x.ToByteArray());
			}
			return SqlBinary.Null;
		}

		// Token: 0x06002B57 RID: 11095 RVA: 0x002A1D10 File Offset: 0x002A1110
		public static SqlBoolean operator ==(SqlBinary x, SqlBinary y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlBoolean.Null;
			}
			return new SqlBoolean(SqlBinary.PerformCompareByte(x.Value, y.Value) == EComparison.EQ);
		}

		// Token: 0x06002B58 RID: 11096 RVA: 0x002A1D50 File Offset: 0x002A1150
		public static SqlBoolean operator !=(SqlBinary x, SqlBinary y)
		{
			return !(x == y);
		}

		// Token: 0x06002B59 RID: 11097 RVA: 0x002A1D6C File Offset: 0x002A116C
		public static SqlBoolean operator <(SqlBinary x, SqlBinary y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlBoolean.Null;
			}
			return new SqlBoolean(SqlBinary.PerformCompareByte(x.Value, y.Value) == EComparison.LT);
		}

		// Token: 0x06002B5A RID: 11098 RVA: 0x002A1DAC File Offset: 0x002A11AC
		public static SqlBoolean operator >(SqlBinary x, SqlBinary y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlBoolean.Null;
			}
			return new SqlBoolean(SqlBinary.PerformCompareByte(x.Value, y.Value) == EComparison.GT);
		}

		// Token: 0x06002B5B RID: 11099 RVA: 0x002A1DEC File Offset: 0x002A11EC
		public static SqlBoolean operator <=(SqlBinary x, SqlBinary y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlBoolean.Null;
			}
			EComparison ecomparison = SqlBinary.PerformCompareByte(x.Value, y.Value);
			return new SqlBoolean(ecomparison == EComparison.LT || ecomparison == EComparison.EQ);
		}

		// Token: 0x06002B5C RID: 11100 RVA: 0x002A1E34 File Offset: 0x002A1234
		public static SqlBoolean operator >=(SqlBinary x, SqlBinary y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlBoolean.Null;
			}
			EComparison ecomparison = SqlBinary.PerformCompareByte(x.Value, y.Value);
			return new SqlBoolean(ecomparison == EComparison.GT || ecomparison == EComparison.EQ);
		}

		// Token: 0x06002B5D RID: 11101 RVA: 0x002A1E80 File Offset: 0x002A1280
		public static SqlBinary Add(SqlBinary x, SqlBinary y)
		{
			return x + y;
		}

		// Token: 0x06002B5E RID: 11102 RVA: 0x002A1E94 File Offset: 0x002A1294
		public static SqlBinary Concat(SqlBinary x, SqlBinary y)
		{
			return x + y;
		}

		// Token: 0x06002B5F RID: 11103 RVA: 0x002A1EA8 File Offset: 0x002A12A8
		public static SqlBoolean Equals(SqlBinary x, SqlBinary y)
		{
			return x == y;
		}

		// Token: 0x06002B60 RID: 11104 RVA: 0x002A1EBC File Offset: 0x002A12BC
		public static SqlBoolean NotEquals(SqlBinary x, SqlBinary y)
		{
			return x != y;
		}

		// Token: 0x06002B61 RID: 11105 RVA: 0x002A1ED0 File Offset: 0x002A12D0
		public static SqlBoolean LessThan(SqlBinary x, SqlBinary y)
		{
			return x < y;
		}

		// Token: 0x06002B62 RID: 11106 RVA: 0x002A1EE4 File Offset: 0x002A12E4
		public static SqlBoolean GreaterThan(SqlBinary x, SqlBinary y)
		{
			return x > y;
		}

		// Token: 0x06002B63 RID: 11107 RVA: 0x002A1EF8 File Offset: 0x002A12F8
		public static SqlBoolean LessThanOrEqual(SqlBinary x, SqlBinary y)
		{
			return x <= y;
		}

		// Token: 0x06002B64 RID: 11108 RVA: 0x002A1F0C File Offset: 0x002A130C
		public static SqlBoolean GreaterThanOrEqual(SqlBinary x, SqlBinary y)
		{
			return x >= y;
		}

		// Token: 0x06002B65 RID: 11109 RVA: 0x002A1F20 File Offset: 0x002A1320
		public SqlGuid ToSqlGuid()
		{
			return (SqlGuid)this;
		}

		// Token: 0x06002B66 RID: 11110 RVA: 0x002A1F38 File Offset: 0x002A1338
		public int CompareTo(object value)
		{
			if (value is SqlBinary)
			{
				SqlBinary sqlBinary = (SqlBinary)value;
				return this.CompareTo(sqlBinary);
			}
			throw ADP.WrongType(value.GetType(), typeof(SqlBinary));
		}

		// Token: 0x06002B67 RID: 11111 RVA: 0x002A1F74 File Offset: 0x002A1374
		public int CompareTo(SqlBinary value)
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

		// Token: 0x06002B68 RID: 11112 RVA: 0x002A1FCC File Offset: 0x002A13CC
		public override bool Equals(object value)
		{
			if (!(value is SqlBinary))
			{
				return false;
			}
			SqlBinary sqlBinary = (SqlBinary)value;
			if (sqlBinary.IsNull || this.IsNull)
			{
				return sqlBinary.IsNull && this.IsNull;
			}
			return (this == sqlBinary).Value;
		}

		// Token: 0x06002B69 RID: 11113 RVA: 0x002A2024 File Offset: 0x002A1424
		internal static int HashByteArray(byte[] rgbValue, int length)
		{
			if (length <= 0)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < length; i++)
			{
				int num2 = (num >> 28) & 255;
				num <<= 4;
				num = num ^ (int)rgbValue[i] ^ num2;
			}
			return num;
		}

		// Token: 0x06002B6A RID: 11114 RVA: 0x002A2060 File Offset: 0x002A1460
		public override int GetHashCode()
		{
			if (this.IsNull)
			{
				return 0;
			}
			int num = this.m_value.Length;
			while (num > 0 && this.m_value[num - 1] == 0)
			{
				num--;
			}
			return SqlBinary.HashByteArray(this.m_value, num);
		}

		// Token: 0x06002B6B RID: 11115 RVA: 0x002A20A4 File Offset: 0x002A14A4
		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		// Token: 0x06002B6C RID: 11116 RVA: 0x002A20B4 File Offset: 0x002A14B4
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			string attribute = reader.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance");
			if (attribute != null && XmlConvert.ToBoolean(attribute))
			{
				this.m_value = null;
				return;
			}
			string text = reader.ReadElementString();
			if (text == null)
			{
				this.m_value = new byte[0];
				return;
			}
			text = text.Trim();
			if (text.Length == 0)
			{
				this.m_value = new byte[0];
				return;
			}
			this.m_value = Convert.FromBase64String(text);
		}

		// Token: 0x06002B6D RID: 11117 RVA: 0x002A2124 File Offset: 0x002A1524
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			if (this.IsNull)
			{
				writer.WriteAttributeString("xsi", "nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
				return;
			}
			writer.WriteString(Convert.ToBase64String(this.m_value));
		}

		// Token: 0x06002B6E RID: 11118 RVA: 0x002A2168 File Offset: 0x002A1568
		public static XmlQualifiedName GetXsdType(XmlSchemaSet schemaSet)
		{
			return new XmlQualifiedName("base64Binary", "http://www.w3.org/2001/XMLSchema");
		}

		// Token: 0x04001C4F RID: 7247
		private byte[] m_value;

		// Token: 0x04001C50 RID: 7248
		public static readonly SqlBinary Null = new SqlBinary(true);
	}
}
