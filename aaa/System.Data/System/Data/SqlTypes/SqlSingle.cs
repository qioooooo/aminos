using System;
using System.Data.Common;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes
{
	// Token: 0x02000355 RID: 853
	[XmlSchemaProvider("GetXsdType")]
	[Serializable]
	public struct SqlSingle : INullable, IComparable, IXmlSerializable
	{
		// Token: 0x06002E98 RID: 11928 RVA: 0x002AE070 File Offset: 0x002AD470
		private SqlSingle(bool fNull)
		{
			this.m_fNotNull = false;
			this.m_value = 0f;
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x002AE090 File Offset: 0x002AD490
		public SqlSingle(float value)
		{
			if (float.IsInfinity(value) || float.IsNaN(value))
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			this.m_fNotNull = true;
			this.m_value = value;
		}

		// Token: 0x06002E9A RID: 11930 RVA: 0x002AE0C8 File Offset: 0x002AD4C8
		public SqlSingle(double value)
		{
			this = new SqlSingle((float)value);
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06002E9B RID: 11931 RVA: 0x002AE0E0 File Offset: 0x002AD4E0
		public bool IsNull
		{
			get
			{
				return !this.m_fNotNull;
			}
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06002E9C RID: 11932 RVA: 0x002AE0F8 File Offset: 0x002AD4F8
		public float Value
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

		// Token: 0x06002E9D RID: 11933 RVA: 0x002AE11C File Offset: 0x002AD51C
		public static implicit operator SqlSingle(float x)
		{
			return new SqlSingle(x);
		}

		// Token: 0x06002E9E RID: 11934 RVA: 0x002AE130 File Offset: 0x002AD530
		public static explicit operator float(SqlSingle x)
		{
			return x.Value;
		}

		// Token: 0x06002E9F RID: 11935 RVA: 0x002AE144 File Offset: 0x002AD544
		public override string ToString()
		{
			if (!this.IsNull)
			{
				return this.m_value.ToString(null);
			}
			return SQLResource.NullString;
		}

		// Token: 0x06002EA0 RID: 11936 RVA: 0x002AE16C File Offset: 0x002AD56C
		public static SqlSingle Parse(string s)
		{
			if (s == SQLResource.NullString)
			{
				return SqlSingle.Null;
			}
			return new SqlSingle(float.Parse(s, CultureInfo.InvariantCulture));
		}

		// Token: 0x06002EA1 RID: 11937 RVA: 0x002AE19C File Offset: 0x002AD59C
		public static SqlSingle operator -(SqlSingle x)
		{
			if (!x.IsNull)
			{
				return new SqlSingle(-x.m_value);
			}
			return SqlSingle.Null;
		}

		// Token: 0x06002EA2 RID: 11938 RVA: 0x002AE1C8 File Offset: 0x002AD5C8
		public static SqlSingle operator +(SqlSingle x, SqlSingle y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlSingle.Null;
			}
			float num = x.m_value + y.m_value;
			if (float.IsInfinity(num))
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlSingle(num);
		}

		// Token: 0x06002EA3 RID: 11939 RVA: 0x002AE218 File Offset: 0x002AD618
		public static SqlSingle operator -(SqlSingle x, SqlSingle y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlSingle.Null;
			}
			float num = x.m_value - y.m_value;
			if (float.IsInfinity(num))
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlSingle(num);
		}

		// Token: 0x06002EA4 RID: 11940 RVA: 0x002AE268 File Offset: 0x002AD668
		public static SqlSingle operator *(SqlSingle x, SqlSingle y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlSingle.Null;
			}
			float num = x.m_value * y.m_value;
			if (float.IsInfinity(num))
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlSingle(num);
		}

		// Token: 0x06002EA5 RID: 11941 RVA: 0x002AE2B8 File Offset: 0x002AD6B8
		public static SqlSingle operator /(SqlSingle x, SqlSingle y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlSingle.Null;
			}
			if (y.m_value == 0f)
			{
				throw new DivideByZeroException(SQLResource.DivideByZeroMessage);
			}
			float num = x.m_value / y.m_value;
			if (float.IsInfinity(num))
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlSingle(num);
		}

		// Token: 0x06002EA6 RID: 11942 RVA: 0x002AE320 File Offset: 0x002AD720
		public static explicit operator SqlSingle(SqlBoolean x)
		{
			if (!x.IsNull)
			{
				return new SqlSingle((float)x.ByteValue);
			}
			return SqlSingle.Null;
		}

		// Token: 0x06002EA7 RID: 11943 RVA: 0x002AE34C File Offset: 0x002AD74C
		public static implicit operator SqlSingle(SqlByte x)
		{
			if (!x.IsNull)
			{
				return new SqlSingle((float)x.Value);
			}
			return SqlSingle.Null;
		}

		// Token: 0x06002EA8 RID: 11944 RVA: 0x002AE378 File Offset: 0x002AD778
		public static implicit operator SqlSingle(SqlInt16 x)
		{
			if (!x.IsNull)
			{
				return new SqlSingle((float)x.Value);
			}
			return SqlSingle.Null;
		}

		// Token: 0x06002EA9 RID: 11945 RVA: 0x002AE3A4 File Offset: 0x002AD7A4
		public static implicit operator SqlSingle(SqlInt32 x)
		{
			if (!x.IsNull)
			{
				return new SqlSingle((float)x.Value);
			}
			return SqlSingle.Null;
		}

		// Token: 0x06002EAA RID: 11946 RVA: 0x002AE3D0 File Offset: 0x002AD7D0
		public static implicit operator SqlSingle(SqlInt64 x)
		{
			if (!x.IsNull)
			{
				return new SqlSingle((float)x.Value);
			}
			return SqlSingle.Null;
		}

		// Token: 0x06002EAB RID: 11947 RVA: 0x002AE3FC File Offset: 0x002AD7FC
		public static implicit operator SqlSingle(SqlMoney x)
		{
			if (!x.IsNull)
			{
				return new SqlSingle(x.ToDouble());
			}
			return SqlSingle.Null;
		}

		// Token: 0x06002EAC RID: 11948 RVA: 0x002AE424 File Offset: 0x002AD824
		public static implicit operator SqlSingle(SqlDecimal x)
		{
			if (!x.IsNull)
			{
				return new SqlSingle(x.ToDouble());
			}
			return SqlSingle.Null;
		}

		// Token: 0x06002EAD RID: 11949 RVA: 0x002AE44C File Offset: 0x002AD84C
		public static explicit operator SqlSingle(SqlDouble x)
		{
			if (!x.IsNull)
			{
				return new SqlSingle(x.Value);
			}
			return SqlSingle.Null;
		}

		// Token: 0x06002EAE RID: 11950 RVA: 0x002AE474 File Offset: 0x002AD874
		public static explicit operator SqlSingle(SqlString x)
		{
			if (x.IsNull)
			{
				return SqlSingle.Null;
			}
			return SqlSingle.Parse(x.Value);
		}

		// Token: 0x06002EAF RID: 11951 RVA: 0x002AE49C File Offset: 0x002AD89C
		public static SqlBoolean operator ==(SqlSingle x, SqlSingle y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value == y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x002AE4D8 File Offset: 0x002AD8D8
		public static SqlBoolean operator !=(SqlSingle x, SqlSingle y)
		{
			return !(x == y);
		}

		// Token: 0x06002EB1 RID: 11953 RVA: 0x002AE4F4 File Offset: 0x002AD8F4
		public static SqlBoolean operator <(SqlSingle x, SqlSingle y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value < y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002EB2 RID: 11954 RVA: 0x002AE530 File Offset: 0x002AD930
		public static SqlBoolean operator >(SqlSingle x, SqlSingle y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value > y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002EB3 RID: 11955 RVA: 0x002AE56C File Offset: 0x002AD96C
		public static SqlBoolean operator <=(SqlSingle x, SqlSingle y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value <= y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002EB4 RID: 11956 RVA: 0x002AE5AC File Offset: 0x002AD9AC
		public static SqlBoolean operator >=(SqlSingle x, SqlSingle y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value >= y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002EB5 RID: 11957 RVA: 0x002AE5EC File Offset: 0x002AD9EC
		public static SqlSingle Add(SqlSingle x, SqlSingle y)
		{
			return x + y;
		}

		// Token: 0x06002EB6 RID: 11958 RVA: 0x002AE600 File Offset: 0x002ADA00
		public static SqlSingle Subtract(SqlSingle x, SqlSingle y)
		{
			return x - y;
		}

		// Token: 0x06002EB7 RID: 11959 RVA: 0x002AE614 File Offset: 0x002ADA14
		public static SqlSingle Multiply(SqlSingle x, SqlSingle y)
		{
			return x * y;
		}

		// Token: 0x06002EB8 RID: 11960 RVA: 0x002AE628 File Offset: 0x002ADA28
		public static SqlSingle Divide(SqlSingle x, SqlSingle y)
		{
			return x / y;
		}

		// Token: 0x06002EB9 RID: 11961 RVA: 0x002AE63C File Offset: 0x002ADA3C
		public static SqlBoolean Equals(SqlSingle x, SqlSingle y)
		{
			return x == y;
		}

		// Token: 0x06002EBA RID: 11962 RVA: 0x002AE650 File Offset: 0x002ADA50
		public static SqlBoolean NotEquals(SqlSingle x, SqlSingle y)
		{
			return x != y;
		}

		// Token: 0x06002EBB RID: 11963 RVA: 0x002AE664 File Offset: 0x002ADA64
		public static SqlBoolean LessThan(SqlSingle x, SqlSingle y)
		{
			return x < y;
		}

		// Token: 0x06002EBC RID: 11964 RVA: 0x002AE678 File Offset: 0x002ADA78
		public static SqlBoolean GreaterThan(SqlSingle x, SqlSingle y)
		{
			return x > y;
		}

		// Token: 0x06002EBD RID: 11965 RVA: 0x002AE68C File Offset: 0x002ADA8C
		public static SqlBoolean LessThanOrEqual(SqlSingle x, SqlSingle y)
		{
			return x <= y;
		}

		// Token: 0x06002EBE RID: 11966 RVA: 0x002AE6A0 File Offset: 0x002ADAA0
		public static SqlBoolean GreaterThanOrEqual(SqlSingle x, SqlSingle y)
		{
			return x >= y;
		}

		// Token: 0x06002EBF RID: 11967 RVA: 0x002AE6B4 File Offset: 0x002ADAB4
		public SqlBoolean ToSqlBoolean()
		{
			return (SqlBoolean)this;
		}

		// Token: 0x06002EC0 RID: 11968 RVA: 0x002AE6CC File Offset: 0x002ADACC
		public SqlByte ToSqlByte()
		{
			return (SqlByte)this;
		}

		// Token: 0x06002EC1 RID: 11969 RVA: 0x002AE6E4 File Offset: 0x002ADAE4
		public SqlDouble ToSqlDouble()
		{
			return this;
		}

		// Token: 0x06002EC2 RID: 11970 RVA: 0x002AE6FC File Offset: 0x002ADAFC
		public SqlInt16 ToSqlInt16()
		{
			return (SqlInt16)this;
		}

		// Token: 0x06002EC3 RID: 11971 RVA: 0x002AE714 File Offset: 0x002ADB14
		public SqlInt32 ToSqlInt32()
		{
			return (SqlInt32)this;
		}

		// Token: 0x06002EC4 RID: 11972 RVA: 0x002AE72C File Offset: 0x002ADB2C
		public SqlInt64 ToSqlInt64()
		{
			return (SqlInt64)this;
		}

		// Token: 0x06002EC5 RID: 11973 RVA: 0x002AE744 File Offset: 0x002ADB44
		public SqlMoney ToSqlMoney()
		{
			return (SqlMoney)this;
		}

		// Token: 0x06002EC6 RID: 11974 RVA: 0x002AE75C File Offset: 0x002ADB5C
		public SqlDecimal ToSqlDecimal()
		{
			return (SqlDecimal)this;
		}

		// Token: 0x06002EC7 RID: 11975 RVA: 0x002AE774 File Offset: 0x002ADB74
		public SqlString ToSqlString()
		{
			return (SqlString)this;
		}

		// Token: 0x06002EC8 RID: 11976 RVA: 0x002AE78C File Offset: 0x002ADB8C
		public int CompareTo(object value)
		{
			if (value is SqlSingle)
			{
				SqlSingle sqlSingle = (SqlSingle)value;
				return this.CompareTo(sqlSingle);
			}
			throw ADP.WrongType(value.GetType(), typeof(SqlSingle));
		}

		// Token: 0x06002EC9 RID: 11977 RVA: 0x002AE7C8 File Offset: 0x002ADBC8
		public int CompareTo(SqlSingle value)
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

		// Token: 0x06002ECA RID: 11978 RVA: 0x002AE820 File Offset: 0x002ADC20
		public override bool Equals(object value)
		{
			if (!(value is SqlSingle))
			{
				return false;
			}
			SqlSingle sqlSingle = (SqlSingle)value;
			if (sqlSingle.IsNull || this.IsNull)
			{
				return sqlSingle.IsNull && this.IsNull;
			}
			return (this == sqlSingle).Value;
		}

		// Token: 0x06002ECB RID: 11979 RVA: 0x002AE878 File Offset: 0x002ADC78
		public override int GetHashCode()
		{
			if (!this.IsNull)
			{
				return this.Value.GetHashCode();
			}
			return 0;
		}

		// Token: 0x06002ECC RID: 11980 RVA: 0x002AE8A0 File Offset: 0x002ADCA0
		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		// Token: 0x06002ECD RID: 11981 RVA: 0x002AE8B0 File Offset: 0x002ADCB0
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			string attribute = reader.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance");
			if (attribute != null && XmlConvert.ToBoolean(attribute))
			{
				this.m_fNotNull = false;
				return;
			}
			this.m_value = XmlConvert.ToSingle(reader.ReadElementString());
			this.m_fNotNull = true;
		}

		// Token: 0x06002ECE RID: 11982 RVA: 0x002AE8FC File Offset: 0x002ADCFC
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			if (this.IsNull)
			{
				writer.WriteAttributeString("xsi", "nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
				return;
			}
			writer.WriteString(XmlConvert.ToString(this.m_value));
		}

		// Token: 0x06002ECF RID: 11983 RVA: 0x002AE940 File Offset: 0x002ADD40
		public static XmlQualifiedName GetXsdType(XmlSchemaSet schemaSet)
		{
			return new XmlQualifiedName("float", "http://www.w3.org/2001/XMLSchema");
		}

		// Token: 0x04001D25 RID: 7461
		private bool m_fNotNull;

		// Token: 0x04001D26 RID: 7462
		private float m_value;

		// Token: 0x04001D27 RID: 7463
		public static readonly SqlSingle Null = new SqlSingle(true);

		// Token: 0x04001D28 RID: 7464
		public static readonly SqlSingle Zero = new SqlSingle(0f);

		// Token: 0x04001D29 RID: 7465
		public static readonly SqlSingle MinValue = new SqlSingle(float.MinValue);

		// Token: 0x04001D2A RID: 7466
		public static readonly SqlSingle MaxValue = new SqlSingle(float.MaxValue);
	}
}
