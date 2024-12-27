using System;
using System.Data.Common;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes
{
	// Token: 0x02000353 RID: 851
	[XmlSchemaProvider("GetXsdType")]
	[Serializable]
	public struct SqlMoney : INullable, IComparable, IXmlSerializable
	{
		// Token: 0x06002E4F RID: 11855 RVA: 0x002AD2B8 File Offset: 0x002AC6B8
		private SqlMoney(bool fNull)
		{
			this.m_fNotNull = false;
			this.m_value = 0L;
		}

		// Token: 0x06002E50 RID: 11856 RVA: 0x002AD2D4 File Offset: 0x002AC6D4
		internal SqlMoney(long value, int ignored)
		{
			this.m_value = value;
			this.m_fNotNull = true;
		}

		// Token: 0x06002E51 RID: 11857 RVA: 0x002AD2F0 File Offset: 0x002AC6F0
		public SqlMoney(int value)
		{
			this.m_value = (long)value * SqlMoney.x_lTickBase;
			this.m_fNotNull = true;
		}

		// Token: 0x06002E52 RID: 11858 RVA: 0x002AD314 File Offset: 0x002AC714
		public SqlMoney(long value)
		{
			if (value < SqlMoney.MinLong || value > SqlMoney.MaxLong)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			this.m_value = value * SqlMoney.x_lTickBase;
			this.m_fNotNull = true;
		}

		// Token: 0x06002E53 RID: 11859 RVA: 0x002AD350 File Offset: 0x002AC750
		public SqlMoney(decimal value)
		{
			SqlDecimal sqlDecimal = new SqlDecimal(value);
			sqlDecimal.AdjustScale(SqlMoney.x_iMoneyScale - (int)sqlDecimal.Scale, true);
			if (sqlDecimal.m_data3 != 0U || sqlDecimal.m_data4 != 0U)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			bool isPositive = sqlDecimal.IsPositive;
			ulong num = (ulong)sqlDecimal.m_data1 + ((ulong)sqlDecimal.m_data2 << 32);
			if ((isPositive && num > 9223372036854775807UL) || (!isPositive && num > 9223372036854775808UL))
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			this.m_value = (long)(isPositive ? num : (-(long)num));
			this.m_fNotNull = true;
		}

		// Token: 0x06002E54 RID: 11860 RVA: 0x002AD3F4 File Offset: 0x002AC7F4
		public SqlMoney(double value)
		{
			this = new SqlMoney(new decimal(value));
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06002E55 RID: 11861 RVA: 0x002AD410 File Offset: 0x002AC810
		public bool IsNull
		{
			get
			{
				return !this.m_fNotNull;
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06002E56 RID: 11862 RVA: 0x002AD428 File Offset: 0x002AC828
		public decimal Value
		{
			get
			{
				if (this.m_fNotNull)
				{
					return this.ToDecimal();
				}
				throw new SqlNullValueException();
			}
		}

		// Token: 0x06002E57 RID: 11863 RVA: 0x002AD44C File Offset: 0x002AC84C
		public decimal ToDecimal()
		{
			if (this.IsNull)
			{
				throw new SqlNullValueException();
			}
			bool flag = false;
			long num = this.m_value;
			if (this.m_value < 0L)
			{
				flag = true;
				num = -this.m_value;
			}
			return new decimal((int)num, (int)(num >> 32), 0, flag, (byte)SqlMoney.x_iMoneyScale);
		}

		// Token: 0x06002E58 RID: 11864 RVA: 0x002AD498 File Offset: 0x002AC898
		public long ToInt64()
		{
			if (this.IsNull)
			{
				throw new SqlNullValueException();
			}
			long num = this.m_value / (SqlMoney.x_lTickBase / 10L);
			bool flag = num >= 0L;
			long num2 = num % 10L;
			num /= 10L;
			if (num2 >= 5L)
			{
				if (flag)
				{
					num += 1L;
				}
				else
				{
					num -= 1L;
				}
			}
			return num;
		}

		// Token: 0x06002E59 RID: 11865 RVA: 0x002AD4F0 File Offset: 0x002AC8F0
		internal long ToSqlInternalRepresentation()
		{
			if (this.IsNull)
			{
				throw new SqlNullValueException();
			}
			return this.m_value;
		}

		// Token: 0x06002E5A RID: 11866 RVA: 0x002AD514 File Offset: 0x002AC914
		public int ToInt32()
		{
			return checked((int)this.ToInt64());
		}

		// Token: 0x06002E5B RID: 11867 RVA: 0x002AD528 File Offset: 0x002AC928
		public double ToDouble()
		{
			return decimal.ToDouble(this.ToDecimal());
		}

		// Token: 0x06002E5C RID: 11868 RVA: 0x002AD540 File Offset: 0x002AC940
		public static implicit operator SqlMoney(decimal x)
		{
			return new SqlMoney(x);
		}

		// Token: 0x06002E5D RID: 11869 RVA: 0x002AD554 File Offset: 0x002AC954
		public static explicit operator SqlMoney(double x)
		{
			return new SqlMoney(x);
		}

		// Token: 0x06002E5E RID: 11870 RVA: 0x002AD568 File Offset: 0x002AC968
		public static implicit operator SqlMoney(long x)
		{
			return new SqlMoney(new decimal(x));
		}

		// Token: 0x06002E5F RID: 11871 RVA: 0x002AD580 File Offset: 0x002AC980
		public static explicit operator decimal(SqlMoney x)
		{
			return x.Value;
		}

		// Token: 0x06002E60 RID: 11872 RVA: 0x002AD594 File Offset: 0x002AC994
		public override string ToString()
		{
			if (this.IsNull)
			{
				return SQLResource.NullString;
			}
			return this.ToDecimal().ToString("#0.00##", null);
		}

		// Token: 0x06002E61 RID: 11873 RVA: 0x002AD5C4 File Offset: 0x002AC9C4
		public static SqlMoney Parse(string s)
		{
			SqlMoney @null;
			decimal num;
			if (s == SQLResource.NullString)
			{
				@null = SqlMoney.Null;
			}
			else if (decimal.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowParentheses | NumberStyles.AllowDecimalPoint | NumberStyles.AllowCurrencySymbol, NumberFormatInfo.InvariantInfo, out num))
			{
				@null = new SqlMoney(num);
			}
			else
			{
				@null = new SqlMoney(decimal.Parse(s, NumberStyles.Currency, NumberFormatInfo.CurrentInfo));
			}
			return @null;
		}

		// Token: 0x06002E62 RID: 11874 RVA: 0x002AD61C File Offset: 0x002ACA1C
		public static SqlMoney operator -(SqlMoney x)
		{
			if (x.IsNull)
			{
				return SqlMoney.Null;
			}
			if (x.m_value == SqlMoney.MinLong)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlMoney(-x.m_value, 0);
		}

		// Token: 0x06002E63 RID: 11875 RVA: 0x002AD660 File Offset: 0x002ACA60
		public static SqlMoney operator +(SqlMoney x, SqlMoney y)
		{
			SqlMoney sqlMoney;
			try
			{
				sqlMoney = ((x.IsNull || y.IsNull) ? SqlMoney.Null : new SqlMoney(checked(x.m_value + y.m_value), 0));
			}
			catch (OverflowException)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return sqlMoney;
		}

		// Token: 0x06002E64 RID: 11876 RVA: 0x002AD6CC File Offset: 0x002ACACC
		public static SqlMoney operator -(SqlMoney x, SqlMoney y)
		{
			SqlMoney sqlMoney;
			try
			{
				sqlMoney = ((x.IsNull || y.IsNull) ? SqlMoney.Null : new SqlMoney(checked(x.m_value - y.m_value), 0));
			}
			catch (OverflowException)
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return sqlMoney;
		}

		// Token: 0x06002E65 RID: 11877 RVA: 0x002AD738 File Offset: 0x002ACB38
		public static SqlMoney operator *(SqlMoney x, SqlMoney y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlMoney(decimal.Multiply(x.ToDecimal(), y.ToDecimal()));
			}
			return SqlMoney.Null;
		}

		// Token: 0x06002E66 RID: 11878 RVA: 0x002AD778 File Offset: 0x002ACB78
		public static SqlMoney operator /(SqlMoney x, SqlMoney y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlMoney(decimal.Divide(x.ToDecimal(), y.ToDecimal()));
			}
			return SqlMoney.Null;
		}

		// Token: 0x06002E67 RID: 11879 RVA: 0x002AD7B8 File Offset: 0x002ACBB8
		public static explicit operator SqlMoney(SqlBoolean x)
		{
			if (!x.IsNull)
			{
				return new SqlMoney((int)x.ByteValue);
			}
			return SqlMoney.Null;
		}

		// Token: 0x06002E68 RID: 11880 RVA: 0x002AD7E0 File Offset: 0x002ACBE0
		public static implicit operator SqlMoney(SqlByte x)
		{
			if (!x.IsNull)
			{
				return new SqlMoney((int)x.Value);
			}
			return SqlMoney.Null;
		}

		// Token: 0x06002E69 RID: 11881 RVA: 0x002AD808 File Offset: 0x002ACC08
		public static implicit operator SqlMoney(SqlInt16 x)
		{
			if (!x.IsNull)
			{
				return new SqlMoney((int)x.Value);
			}
			return SqlMoney.Null;
		}

		// Token: 0x06002E6A RID: 11882 RVA: 0x002AD830 File Offset: 0x002ACC30
		public static implicit operator SqlMoney(SqlInt32 x)
		{
			if (!x.IsNull)
			{
				return new SqlMoney(x.Value);
			}
			return SqlMoney.Null;
		}

		// Token: 0x06002E6B RID: 11883 RVA: 0x002AD858 File Offset: 0x002ACC58
		public static implicit operator SqlMoney(SqlInt64 x)
		{
			if (!x.IsNull)
			{
				return new SqlMoney(x.Value);
			}
			return SqlMoney.Null;
		}

		// Token: 0x06002E6C RID: 11884 RVA: 0x002AD880 File Offset: 0x002ACC80
		public static explicit operator SqlMoney(SqlSingle x)
		{
			if (!x.IsNull)
			{
				return new SqlMoney((double)x.Value);
			}
			return SqlMoney.Null;
		}

		// Token: 0x06002E6D RID: 11885 RVA: 0x002AD8AC File Offset: 0x002ACCAC
		public static explicit operator SqlMoney(SqlDouble x)
		{
			if (!x.IsNull)
			{
				return new SqlMoney(x.Value);
			}
			return SqlMoney.Null;
		}

		// Token: 0x06002E6E RID: 11886 RVA: 0x002AD8D4 File Offset: 0x002ACCD4
		public static explicit operator SqlMoney(SqlDecimal x)
		{
			if (!x.IsNull)
			{
				return new SqlMoney(x.Value);
			}
			return SqlMoney.Null;
		}

		// Token: 0x06002E6F RID: 11887 RVA: 0x002AD8FC File Offset: 0x002ACCFC
		public static explicit operator SqlMoney(SqlString x)
		{
			if (!x.IsNull)
			{
				return new SqlMoney(decimal.Parse(x.Value, NumberStyles.Currency, null));
			}
			return SqlMoney.Null;
		}

		// Token: 0x06002E70 RID: 11888 RVA: 0x002AD930 File Offset: 0x002ACD30
		public static SqlBoolean operator ==(SqlMoney x, SqlMoney y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value == y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002E71 RID: 11889 RVA: 0x002AD96C File Offset: 0x002ACD6C
		public static SqlBoolean operator !=(SqlMoney x, SqlMoney y)
		{
			return !(x == y);
		}

		// Token: 0x06002E72 RID: 11890 RVA: 0x002AD988 File Offset: 0x002ACD88
		public static SqlBoolean operator <(SqlMoney x, SqlMoney y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value < y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002E73 RID: 11891 RVA: 0x002AD9C4 File Offset: 0x002ACDC4
		public static SqlBoolean operator >(SqlMoney x, SqlMoney y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value > y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002E74 RID: 11892 RVA: 0x002ADA00 File Offset: 0x002ACE00
		public static SqlBoolean operator <=(SqlMoney x, SqlMoney y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value <= y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x002ADA40 File Offset: 0x002ACE40
		public static SqlBoolean operator >=(SqlMoney x, SqlMoney y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value >= y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x002ADA80 File Offset: 0x002ACE80
		public static SqlMoney Add(SqlMoney x, SqlMoney y)
		{
			return x + y;
		}

		// Token: 0x06002E77 RID: 11895 RVA: 0x002ADA94 File Offset: 0x002ACE94
		public static SqlMoney Subtract(SqlMoney x, SqlMoney y)
		{
			return x - y;
		}

		// Token: 0x06002E78 RID: 11896 RVA: 0x002ADAA8 File Offset: 0x002ACEA8
		public static SqlMoney Multiply(SqlMoney x, SqlMoney y)
		{
			return x * y;
		}

		// Token: 0x06002E79 RID: 11897 RVA: 0x002ADABC File Offset: 0x002ACEBC
		public static SqlMoney Divide(SqlMoney x, SqlMoney y)
		{
			return x / y;
		}

		// Token: 0x06002E7A RID: 11898 RVA: 0x002ADAD0 File Offset: 0x002ACED0
		public static SqlBoolean Equals(SqlMoney x, SqlMoney y)
		{
			return x == y;
		}

		// Token: 0x06002E7B RID: 11899 RVA: 0x002ADAE4 File Offset: 0x002ACEE4
		public static SqlBoolean NotEquals(SqlMoney x, SqlMoney y)
		{
			return x != y;
		}

		// Token: 0x06002E7C RID: 11900 RVA: 0x002ADAF8 File Offset: 0x002ACEF8
		public static SqlBoolean LessThan(SqlMoney x, SqlMoney y)
		{
			return x < y;
		}

		// Token: 0x06002E7D RID: 11901 RVA: 0x002ADB0C File Offset: 0x002ACF0C
		public static SqlBoolean GreaterThan(SqlMoney x, SqlMoney y)
		{
			return x > y;
		}

		// Token: 0x06002E7E RID: 11902 RVA: 0x002ADB20 File Offset: 0x002ACF20
		public static SqlBoolean LessThanOrEqual(SqlMoney x, SqlMoney y)
		{
			return x <= y;
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x002ADB34 File Offset: 0x002ACF34
		public static SqlBoolean GreaterThanOrEqual(SqlMoney x, SqlMoney y)
		{
			return x >= y;
		}

		// Token: 0x06002E80 RID: 11904 RVA: 0x002ADB48 File Offset: 0x002ACF48
		public SqlBoolean ToSqlBoolean()
		{
			return (SqlBoolean)this;
		}

		// Token: 0x06002E81 RID: 11905 RVA: 0x002ADB60 File Offset: 0x002ACF60
		public SqlByte ToSqlByte()
		{
			return (SqlByte)this;
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x002ADB78 File Offset: 0x002ACF78
		public SqlDouble ToSqlDouble()
		{
			return this;
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x002ADB90 File Offset: 0x002ACF90
		public SqlInt16 ToSqlInt16()
		{
			return (SqlInt16)this;
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x002ADBA8 File Offset: 0x002ACFA8
		public SqlInt32 ToSqlInt32()
		{
			return (SqlInt32)this;
		}

		// Token: 0x06002E85 RID: 11909 RVA: 0x002ADBC0 File Offset: 0x002ACFC0
		public SqlInt64 ToSqlInt64()
		{
			return (SqlInt64)this;
		}

		// Token: 0x06002E86 RID: 11910 RVA: 0x002ADBD8 File Offset: 0x002ACFD8
		public SqlDecimal ToSqlDecimal()
		{
			return this;
		}

		// Token: 0x06002E87 RID: 11911 RVA: 0x002ADBF0 File Offset: 0x002ACFF0
		public SqlSingle ToSqlSingle()
		{
			return this;
		}

		// Token: 0x06002E88 RID: 11912 RVA: 0x002ADC08 File Offset: 0x002AD008
		public SqlString ToSqlString()
		{
			return (SqlString)this;
		}

		// Token: 0x06002E89 RID: 11913 RVA: 0x002ADC20 File Offset: 0x002AD020
		public int CompareTo(object value)
		{
			if (value is SqlMoney)
			{
				SqlMoney sqlMoney = (SqlMoney)value;
				return this.CompareTo(sqlMoney);
			}
			throw ADP.WrongType(value.GetType(), typeof(SqlMoney));
		}

		// Token: 0x06002E8A RID: 11914 RVA: 0x002ADC5C File Offset: 0x002AD05C
		public int CompareTo(SqlMoney value)
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

		// Token: 0x06002E8B RID: 11915 RVA: 0x002ADCB4 File Offset: 0x002AD0B4
		public override bool Equals(object value)
		{
			if (!(value is SqlMoney))
			{
				return false;
			}
			SqlMoney sqlMoney = (SqlMoney)value;
			if (sqlMoney.IsNull || this.IsNull)
			{
				return sqlMoney.IsNull && this.IsNull;
			}
			return (this == sqlMoney).Value;
		}

		// Token: 0x06002E8C RID: 11916 RVA: 0x002ADD0C File Offset: 0x002AD10C
		public override int GetHashCode()
		{
			if (!this.IsNull)
			{
				return this.m_value.GetHashCode();
			}
			return 0;
		}

		// Token: 0x06002E8D RID: 11917 RVA: 0x002ADD30 File Offset: 0x002AD130
		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		// Token: 0x06002E8E RID: 11918 RVA: 0x002ADD40 File Offset: 0x002AD140
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			string attribute = reader.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance");
			if (attribute != null && XmlConvert.ToBoolean(attribute))
			{
				this.m_fNotNull = false;
				return;
			}
			SqlMoney sqlMoney = new SqlMoney(XmlConvert.ToDecimal(reader.ReadElementString()));
			this.m_fNotNull = sqlMoney.m_fNotNull;
			this.m_value = sqlMoney.m_value;
		}

		// Token: 0x06002E8F RID: 11919 RVA: 0x002ADDA0 File Offset: 0x002AD1A0
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			if (this.IsNull)
			{
				writer.WriteAttributeString("xsi", "nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
				return;
			}
			writer.WriteString(XmlConvert.ToString(this.ToDecimal()));
		}

		// Token: 0x06002E90 RID: 11920 RVA: 0x002ADDE4 File Offset: 0x002AD1E4
		public static XmlQualifiedName GetXsdType(XmlSchemaSet schemaSet)
		{
			return new XmlQualifiedName("decimal", "http://www.w3.org/2001/XMLSchema");
		}

		// Token: 0x04001D06 RID: 7430
		private bool m_fNotNull;

		// Token: 0x04001D07 RID: 7431
		private long m_value;

		// Token: 0x04001D08 RID: 7432
		internal static readonly int x_iMoneyScale = 4;

		// Token: 0x04001D09 RID: 7433
		private static readonly long x_lTickBase = 10000L;

		// Token: 0x04001D0A RID: 7434
		private static readonly double x_dTickBase = (double)SqlMoney.x_lTickBase;

		// Token: 0x04001D0B RID: 7435
		private static readonly long MinLong = long.MinValue / SqlMoney.x_lTickBase;

		// Token: 0x04001D0C RID: 7436
		private static readonly long MaxLong = long.MaxValue / SqlMoney.x_lTickBase;

		// Token: 0x04001D0D RID: 7437
		public static readonly SqlMoney Null = new SqlMoney(true);

		// Token: 0x04001D0E RID: 7438
		public static readonly SqlMoney Zero = new SqlMoney(0);

		// Token: 0x04001D0F RID: 7439
		public static readonly SqlMoney MinValue = new SqlMoney(long.MinValue, 0);

		// Token: 0x04001D10 RID: 7440
		public static readonly SqlMoney MaxValue = new SqlMoney(long.MaxValue, 0);
	}
}
