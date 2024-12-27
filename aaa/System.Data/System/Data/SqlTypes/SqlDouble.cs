using System;
using System.Data.Common;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes
{
	// Token: 0x0200034A RID: 842
	[XmlSchemaProvider("GetXsdType")]
	[Serializable]
	public struct SqlDouble : INullable, IComparable, IXmlSerializable
	{
		// Token: 0x06002CF7 RID: 11511 RVA: 0x002A8F9C File Offset: 0x002A839C
		private SqlDouble(bool fNull)
		{
			this.m_fNotNull = false;
			this.m_value = 0.0;
		}

		// Token: 0x06002CF8 RID: 11512 RVA: 0x002A8FC0 File Offset: 0x002A83C0
		public SqlDouble(double value)
		{
			if (double.IsInfinity(value) || double.IsNaN(value))
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			this.m_value = value;
			this.m_fNotNull = true;
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06002CF9 RID: 11513 RVA: 0x002A8FF8 File Offset: 0x002A83F8
		public bool IsNull
		{
			get
			{
				return !this.m_fNotNull;
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06002CFA RID: 11514 RVA: 0x002A9010 File Offset: 0x002A8410
		public double Value
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

		// Token: 0x06002CFB RID: 11515 RVA: 0x002A9034 File Offset: 0x002A8434
		public static implicit operator SqlDouble(double x)
		{
			return new SqlDouble(x);
		}

		// Token: 0x06002CFC RID: 11516 RVA: 0x002A9048 File Offset: 0x002A8448
		public static explicit operator double(SqlDouble x)
		{
			return x.Value;
		}

		// Token: 0x06002CFD RID: 11517 RVA: 0x002A905C File Offset: 0x002A845C
		public override string ToString()
		{
			if (!this.IsNull)
			{
				return this.m_value.ToString(null);
			}
			return SQLResource.NullString;
		}

		// Token: 0x06002CFE RID: 11518 RVA: 0x002A9084 File Offset: 0x002A8484
		public static SqlDouble Parse(string s)
		{
			if (s == SQLResource.NullString)
			{
				return SqlDouble.Null;
			}
			return new SqlDouble(double.Parse(s, CultureInfo.InvariantCulture));
		}

		// Token: 0x06002CFF RID: 11519 RVA: 0x002A90B4 File Offset: 0x002A84B4
		public static SqlDouble operator -(SqlDouble x)
		{
			if (!x.IsNull)
			{
				return new SqlDouble(-x.m_value);
			}
			return SqlDouble.Null;
		}

		// Token: 0x06002D00 RID: 11520 RVA: 0x002A90E0 File Offset: 0x002A84E0
		public static SqlDouble operator +(SqlDouble x, SqlDouble y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlDouble.Null;
			}
			double num = x.m_value + y.m_value;
			if (double.IsInfinity(num))
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlDouble(num);
		}

		// Token: 0x06002D01 RID: 11521 RVA: 0x002A9130 File Offset: 0x002A8530
		public static SqlDouble operator -(SqlDouble x, SqlDouble y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlDouble.Null;
			}
			double num = x.m_value - y.m_value;
			if (double.IsInfinity(num))
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlDouble(num);
		}

		// Token: 0x06002D02 RID: 11522 RVA: 0x002A9180 File Offset: 0x002A8580
		public static SqlDouble operator *(SqlDouble x, SqlDouble y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlDouble.Null;
			}
			double num = x.m_value * y.m_value;
			if (double.IsInfinity(num))
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlDouble(num);
		}

		// Token: 0x06002D03 RID: 11523 RVA: 0x002A91D0 File Offset: 0x002A85D0
		public static SqlDouble operator /(SqlDouble x, SqlDouble y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlDouble.Null;
			}
			if (y.m_value == 0.0)
			{
				throw new DivideByZeroException(SQLResource.DivideByZeroMessage);
			}
			double num = x.m_value / y.m_value;
			if (double.IsInfinity(num))
			{
				throw new OverflowException(SQLResource.ArithOverflowMessage);
			}
			return new SqlDouble(num);
		}

		// Token: 0x06002D04 RID: 11524 RVA: 0x002A923C File Offset: 0x002A863C
		public static explicit operator SqlDouble(SqlBoolean x)
		{
			if (!x.IsNull)
			{
				return new SqlDouble((double)x.ByteValue);
			}
			return SqlDouble.Null;
		}

		// Token: 0x06002D05 RID: 11525 RVA: 0x002A9268 File Offset: 0x002A8668
		public static implicit operator SqlDouble(SqlByte x)
		{
			if (!x.IsNull)
			{
				return new SqlDouble((double)x.Value);
			}
			return SqlDouble.Null;
		}

		// Token: 0x06002D06 RID: 11526 RVA: 0x002A9294 File Offset: 0x002A8694
		public static implicit operator SqlDouble(SqlInt16 x)
		{
			if (!x.IsNull)
			{
				return new SqlDouble((double)x.Value);
			}
			return SqlDouble.Null;
		}

		// Token: 0x06002D07 RID: 11527 RVA: 0x002A92C0 File Offset: 0x002A86C0
		public static implicit operator SqlDouble(SqlInt32 x)
		{
			if (!x.IsNull)
			{
				return new SqlDouble((double)x.Value);
			}
			return SqlDouble.Null;
		}

		// Token: 0x06002D08 RID: 11528 RVA: 0x002A92EC File Offset: 0x002A86EC
		public static implicit operator SqlDouble(SqlInt64 x)
		{
			if (!x.IsNull)
			{
				return new SqlDouble((double)x.Value);
			}
			return SqlDouble.Null;
		}

		// Token: 0x06002D09 RID: 11529 RVA: 0x002A9318 File Offset: 0x002A8718
		public static implicit operator SqlDouble(SqlSingle x)
		{
			if (!x.IsNull)
			{
				return new SqlDouble((double)x.Value);
			}
			return SqlDouble.Null;
		}

		// Token: 0x06002D0A RID: 11530 RVA: 0x002A9344 File Offset: 0x002A8744
		public static implicit operator SqlDouble(SqlMoney x)
		{
			if (!x.IsNull)
			{
				return new SqlDouble(x.ToDouble());
			}
			return SqlDouble.Null;
		}

		// Token: 0x06002D0B RID: 11531 RVA: 0x002A936C File Offset: 0x002A876C
		public static implicit operator SqlDouble(SqlDecimal x)
		{
			if (!x.IsNull)
			{
				return new SqlDouble(x.ToDouble());
			}
			return SqlDouble.Null;
		}

		// Token: 0x06002D0C RID: 11532 RVA: 0x002A9394 File Offset: 0x002A8794
		public static explicit operator SqlDouble(SqlString x)
		{
			if (x.IsNull)
			{
				return SqlDouble.Null;
			}
			return SqlDouble.Parse(x.Value);
		}

		// Token: 0x06002D0D RID: 11533 RVA: 0x002A93BC File Offset: 0x002A87BC
		public static SqlBoolean operator ==(SqlDouble x, SqlDouble y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value == y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002D0E RID: 11534 RVA: 0x002A93F8 File Offset: 0x002A87F8
		public static SqlBoolean operator !=(SqlDouble x, SqlDouble y)
		{
			return !(x == y);
		}

		// Token: 0x06002D0F RID: 11535 RVA: 0x002A9414 File Offset: 0x002A8814
		public static SqlBoolean operator <(SqlDouble x, SqlDouble y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value < y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002D10 RID: 11536 RVA: 0x002A9450 File Offset: 0x002A8850
		public static SqlBoolean operator >(SqlDouble x, SqlDouble y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value > y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002D11 RID: 11537 RVA: 0x002A948C File Offset: 0x002A888C
		public static SqlBoolean operator <=(SqlDouble x, SqlDouble y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value <= y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002D12 RID: 11538 RVA: 0x002A94CC File Offset: 0x002A88CC
		public static SqlBoolean operator >=(SqlDouble x, SqlDouble y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_value >= y.m_value);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002D13 RID: 11539 RVA: 0x002A950C File Offset: 0x002A890C
		public static SqlDouble Add(SqlDouble x, SqlDouble y)
		{
			return x + y;
		}

		// Token: 0x06002D14 RID: 11540 RVA: 0x002A9520 File Offset: 0x002A8920
		public static SqlDouble Subtract(SqlDouble x, SqlDouble y)
		{
			return x - y;
		}

		// Token: 0x06002D15 RID: 11541 RVA: 0x002A9534 File Offset: 0x002A8934
		public static SqlDouble Multiply(SqlDouble x, SqlDouble y)
		{
			return x * y;
		}

		// Token: 0x06002D16 RID: 11542 RVA: 0x002A9548 File Offset: 0x002A8948
		public static SqlDouble Divide(SqlDouble x, SqlDouble y)
		{
			return x / y;
		}

		// Token: 0x06002D17 RID: 11543 RVA: 0x002A955C File Offset: 0x002A895C
		public static SqlBoolean Equals(SqlDouble x, SqlDouble y)
		{
			return x == y;
		}

		// Token: 0x06002D18 RID: 11544 RVA: 0x002A9570 File Offset: 0x002A8970
		public static SqlBoolean NotEquals(SqlDouble x, SqlDouble y)
		{
			return x != y;
		}

		// Token: 0x06002D19 RID: 11545 RVA: 0x002A9584 File Offset: 0x002A8984
		public static SqlBoolean LessThan(SqlDouble x, SqlDouble y)
		{
			return x < y;
		}

		// Token: 0x06002D1A RID: 11546 RVA: 0x002A9598 File Offset: 0x002A8998
		public static SqlBoolean GreaterThan(SqlDouble x, SqlDouble y)
		{
			return x > y;
		}

		// Token: 0x06002D1B RID: 11547 RVA: 0x002A95AC File Offset: 0x002A89AC
		public static SqlBoolean LessThanOrEqual(SqlDouble x, SqlDouble y)
		{
			return x <= y;
		}

		// Token: 0x06002D1C RID: 11548 RVA: 0x002A95C0 File Offset: 0x002A89C0
		public static SqlBoolean GreaterThanOrEqual(SqlDouble x, SqlDouble y)
		{
			return x >= y;
		}

		// Token: 0x06002D1D RID: 11549 RVA: 0x002A95D4 File Offset: 0x002A89D4
		public SqlBoolean ToSqlBoolean()
		{
			return (SqlBoolean)this;
		}

		// Token: 0x06002D1E RID: 11550 RVA: 0x002A95EC File Offset: 0x002A89EC
		public SqlByte ToSqlByte()
		{
			return (SqlByte)this;
		}

		// Token: 0x06002D1F RID: 11551 RVA: 0x002A9604 File Offset: 0x002A8A04
		public SqlInt16 ToSqlInt16()
		{
			return (SqlInt16)this;
		}

		// Token: 0x06002D20 RID: 11552 RVA: 0x002A961C File Offset: 0x002A8A1C
		public SqlInt32 ToSqlInt32()
		{
			return (SqlInt32)this;
		}

		// Token: 0x06002D21 RID: 11553 RVA: 0x002A9634 File Offset: 0x002A8A34
		public SqlInt64 ToSqlInt64()
		{
			return (SqlInt64)this;
		}

		// Token: 0x06002D22 RID: 11554 RVA: 0x002A964C File Offset: 0x002A8A4C
		public SqlMoney ToSqlMoney()
		{
			return (SqlMoney)this;
		}

		// Token: 0x06002D23 RID: 11555 RVA: 0x002A9664 File Offset: 0x002A8A64
		public SqlDecimal ToSqlDecimal()
		{
			return (SqlDecimal)this;
		}

		// Token: 0x06002D24 RID: 11556 RVA: 0x002A967C File Offset: 0x002A8A7C
		public SqlSingle ToSqlSingle()
		{
			return (SqlSingle)this;
		}

		// Token: 0x06002D25 RID: 11557 RVA: 0x002A9694 File Offset: 0x002A8A94
		public SqlString ToSqlString()
		{
			return (SqlString)this;
		}

		// Token: 0x06002D26 RID: 11558 RVA: 0x002A96AC File Offset: 0x002A8AAC
		public int CompareTo(object value)
		{
			if (value is SqlDouble)
			{
				SqlDouble sqlDouble = (SqlDouble)value;
				return this.CompareTo(sqlDouble);
			}
			throw ADP.WrongType(value.GetType(), typeof(SqlDouble));
		}

		// Token: 0x06002D27 RID: 11559 RVA: 0x002A96E8 File Offset: 0x002A8AE8
		public int CompareTo(SqlDouble value)
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

		// Token: 0x06002D28 RID: 11560 RVA: 0x002A9740 File Offset: 0x002A8B40
		public override bool Equals(object value)
		{
			if (!(value is SqlDouble))
			{
				return false;
			}
			SqlDouble sqlDouble = (SqlDouble)value;
			if (sqlDouble.IsNull || this.IsNull)
			{
				return sqlDouble.IsNull && this.IsNull;
			}
			return (this == sqlDouble).Value;
		}

		// Token: 0x06002D29 RID: 11561 RVA: 0x002A9798 File Offset: 0x002A8B98
		public override int GetHashCode()
		{
			if (!this.IsNull)
			{
				return this.Value.GetHashCode();
			}
			return 0;
		}

		// Token: 0x06002D2A RID: 11562 RVA: 0x002A97C0 File Offset: 0x002A8BC0
		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		// Token: 0x06002D2B RID: 11563 RVA: 0x002A97D0 File Offset: 0x002A8BD0
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			string attribute = reader.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance");
			if (attribute != null && XmlConvert.ToBoolean(attribute))
			{
				this.m_fNotNull = false;
				return;
			}
			this.m_value = XmlConvert.ToDouble(reader.ReadElementString());
			this.m_fNotNull = true;
		}

		// Token: 0x06002D2C RID: 11564 RVA: 0x002A981C File Offset: 0x002A8C1C
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			if (this.IsNull)
			{
				writer.WriteAttributeString("xsi", "nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
				return;
			}
			writer.WriteString(XmlConvert.ToString(this.m_value));
		}

		// Token: 0x06002D2D RID: 11565 RVA: 0x002A9860 File Offset: 0x002A8C60
		public static XmlQualifiedName GetXsdType(XmlSchemaSet schemaSet)
		{
			return new XmlQualifiedName("double", "http://www.w3.org/2001/XMLSchema");
		}

		// Token: 0x04001CD7 RID: 7383
		private bool m_fNotNull;

		// Token: 0x04001CD8 RID: 7384
		private double m_value;

		// Token: 0x04001CD9 RID: 7385
		public static readonly SqlDouble Null = new SqlDouble(true);

		// Token: 0x04001CDA RID: 7386
		public static readonly SqlDouble Zero = new SqlDouble(0.0);

		// Token: 0x04001CDB RID: 7387
		public static readonly SqlDouble MinValue = new SqlDouble(double.MinValue);

		// Token: 0x04001CDC RID: 7388
		public static readonly SqlDouble MaxValue = new SqlDouble(double.MaxValue);
	}
}
