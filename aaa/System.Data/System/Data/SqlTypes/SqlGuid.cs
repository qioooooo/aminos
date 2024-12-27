using System;
using System.Data.Common;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes
{
	// Token: 0x0200034F RID: 847
	[XmlSchemaProvider("GetXsdType")]
	[Serializable]
	public struct SqlGuid : INullable, IComparable, IXmlSerializable
	{
		// Token: 0x06002D5D RID: 11613 RVA: 0x002AA82C File Offset: 0x002A9C2C
		private SqlGuid(bool fNull)
		{
			this.m_value = null;
		}

		// Token: 0x06002D5E RID: 11614 RVA: 0x002AA840 File Offset: 0x002A9C40
		public SqlGuid(byte[] value)
		{
			if (value == null || value.Length != SqlGuid.SizeOfGuid)
			{
				throw new ArgumentException(SQLResource.InvalidArraySizeMessage);
			}
			this.m_value = new byte[SqlGuid.SizeOfGuid];
			value.CopyTo(this.m_value, 0);
		}

		// Token: 0x06002D5F RID: 11615 RVA: 0x002AA884 File Offset: 0x002A9C84
		internal SqlGuid(byte[] value, bool ignored)
		{
			if (value == null || value.Length != SqlGuid.SizeOfGuid)
			{
				throw new ArgumentException(SQLResource.InvalidArraySizeMessage);
			}
			this.m_value = value;
		}

		// Token: 0x06002D60 RID: 11616 RVA: 0x002AA8B0 File Offset: 0x002A9CB0
		public SqlGuid(string s)
		{
			this.m_value = new Guid(s).ToByteArray();
		}

		// Token: 0x06002D61 RID: 11617 RVA: 0x002AA8D4 File Offset: 0x002A9CD4
		public SqlGuid(Guid g)
		{
			this.m_value = g.ToByteArray();
		}

		// Token: 0x06002D62 RID: 11618 RVA: 0x002AA8F0 File Offset: 0x002A9CF0
		public SqlGuid(int a, short b, short c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k)
		{
			this = new SqlGuid(new Guid(a, b, c, d, e, f, g, h, i, j, k));
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06002D63 RID: 11619 RVA: 0x002AA91C File Offset: 0x002A9D1C
		public bool IsNull
		{
			get
			{
				return this.m_value == null;
			}
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06002D64 RID: 11620 RVA: 0x002AA934 File Offset: 0x002A9D34
		public Guid Value
		{
			get
			{
				if (this.IsNull)
				{
					throw new SqlNullValueException();
				}
				return new Guid(this.m_value);
			}
		}

		// Token: 0x06002D65 RID: 11621 RVA: 0x002AA95C File Offset: 0x002A9D5C
		public static implicit operator SqlGuid(Guid x)
		{
			return new SqlGuid(x);
		}

		// Token: 0x06002D66 RID: 11622 RVA: 0x002AA970 File Offset: 0x002A9D70
		public static explicit operator Guid(SqlGuid x)
		{
			return x.Value;
		}

		// Token: 0x06002D67 RID: 11623 RVA: 0x002AA984 File Offset: 0x002A9D84
		public byte[] ToByteArray()
		{
			byte[] array = new byte[SqlGuid.SizeOfGuid];
			this.m_value.CopyTo(array, 0);
			return array;
		}

		// Token: 0x06002D68 RID: 11624 RVA: 0x002AA9AC File Offset: 0x002A9DAC
		public override string ToString()
		{
			if (this.IsNull)
			{
				return SQLResource.NullString;
			}
			Guid guid = new Guid(this.m_value);
			return guid.ToString();
		}

		// Token: 0x06002D69 RID: 11625 RVA: 0x002AA9E4 File Offset: 0x002A9DE4
		public static SqlGuid Parse(string s)
		{
			if (s == SQLResource.NullString)
			{
				return SqlGuid.Null;
			}
			return new SqlGuid(s);
		}

		// Token: 0x06002D6A RID: 11626 RVA: 0x002AAA0C File Offset: 0x002A9E0C
		private static EComparison Compare(SqlGuid x, SqlGuid y)
		{
			int i = 0;
			while (i < SqlGuid.SizeOfGuid)
			{
				byte b = x.m_value[SqlGuid.x_rgiGuidOrder[i]];
				byte b2 = y.m_value[SqlGuid.x_rgiGuidOrder[i]];
				if (b != b2)
				{
					if (b >= b2)
					{
						return EComparison.GT;
					}
					return EComparison.LT;
				}
				else
				{
					i++;
				}
			}
			return EComparison.EQ;
		}

		// Token: 0x06002D6B RID: 11627 RVA: 0x002AAA58 File Offset: 0x002A9E58
		public static explicit operator SqlGuid(SqlString x)
		{
			if (!x.IsNull)
			{
				return new SqlGuid(x.Value);
			}
			return SqlGuid.Null;
		}

		// Token: 0x06002D6C RID: 11628 RVA: 0x002AAA80 File Offset: 0x002A9E80
		public static explicit operator SqlGuid(SqlBinary x)
		{
			if (!x.IsNull)
			{
				return new SqlGuid(x.Value);
			}
			return SqlGuid.Null;
		}

		// Token: 0x06002D6D RID: 11629 RVA: 0x002AAAA8 File Offset: 0x002A9EA8
		public static SqlBoolean operator ==(SqlGuid x, SqlGuid y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(SqlGuid.Compare(x, y) == EComparison.EQ);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002D6E RID: 11630 RVA: 0x002AAADC File Offset: 0x002A9EDC
		public static SqlBoolean operator !=(SqlGuid x, SqlGuid y)
		{
			return !(x == y);
		}

		// Token: 0x06002D6F RID: 11631 RVA: 0x002AAAF8 File Offset: 0x002A9EF8
		public static SqlBoolean operator <(SqlGuid x, SqlGuid y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(SqlGuid.Compare(x, y) == EComparison.LT);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002D70 RID: 11632 RVA: 0x002AAB2C File Offset: 0x002A9F2C
		public static SqlBoolean operator >(SqlGuid x, SqlGuid y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(SqlGuid.Compare(x, y) == EComparison.GT);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002D71 RID: 11633 RVA: 0x002AAB60 File Offset: 0x002A9F60
		public static SqlBoolean operator <=(SqlGuid x, SqlGuid y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlBoolean.Null;
			}
			EComparison ecomparison = SqlGuid.Compare(x, y);
			return new SqlBoolean(ecomparison == EComparison.LT || ecomparison == EComparison.EQ);
		}

		// Token: 0x06002D72 RID: 11634 RVA: 0x002AAB9C File Offset: 0x002A9F9C
		public static SqlBoolean operator >=(SqlGuid x, SqlGuid y)
		{
			if (x.IsNull || y.IsNull)
			{
				return SqlBoolean.Null;
			}
			EComparison ecomparison = SqlGuid.Compare(x, y);
			return new SqlBoolean(ecomparison == EComparison.GT || ecomparison == EComparison.EQ);
		}

		// Token: 0x06002D73 RID: 11635 RVA: 0x002AABDC File Offset: 0x002A9FDC
		public static SqlBoolean Equals(SqlGuid x, SqlGuid y)
		{
			return x == y;
		}

		// Token: 0x06002D74 RID: 11636 RVA: 0x002AABF0 File Offset: 0x002A9FF0
		public static SqlBoolean NotEquals(SqlGuid x, SqlGuid y)
		{
			return x != y;
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x002AAC04 File Offset: 0x002AA004
		public static SqlBoolean LessThan(SqlGuid x, SqlGuid y)
		{
			return x < y;
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x002AAC18 File Offset: 0x002AA018
		public static SqlBoolean GreaterThan(SqlGuid x, SqlGuid y)
		{
			return x > y;
		}

		// Token: 0x06002D77 RID: 11639 RVA: 0x002AAC2C File Offset: 0x002AA02C
		public static SqlBoolean LessThanOrEqual(SqlGuid x, SqlGuid y)
		{
			return x <= y;
		}

		// Token: 0x06002D78 RID: 11640 RVA: 0x002AAC40 File Offset: 0x002AA040
		public static SqlBoolean GreaterThanOrEqual(SqlGuid x, SqlGuid y)
		{
			return x >= y;
		}

		// Token: 0x06002D79 RID: 11641 RVA: 0x002AAC54 File Offset: 0x002AA054
		public SqlString ToSqlString()
		{
			return (SqlString)this;
		}

		// Token: 0x06002D7A RID: 11642 RVA: 0x002AAC6C File Offset: 0x002AA06C
		public SqlBinary ToSqlBinary()
		{
			return (SqlBinary)this;
		}

		// Token: 0x06002D7B RID: 11643 RVA: 0x002AAC84 File Offset: 0x002AA084
		public int CompareTo(object value)
		{
			if (value is SqlGuid)
			{
				SqlGuid sqlGuid = (SqlGuid)value;
				return this.CompareTo(sqlGuid);
			}
			throw ADP.WrongType(value.GetType(), typeof(SqlGuid));
		}

		// Token: 0x06002D7C RID: 11644 RVA: 0x002AACC0 File Offset: 0x002AA0C0
		public int CompareTo(SqlGuid value)
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

		// Token: 0x06002D7D RID: 11645 RVA: 0x002AAD18 File Offset: 0x002AA118
		public override bool Equals(object value)
		{
			if (!(value is SqlGuid))
			{
				return false;
			}
			SqlGuid sqlGuid = (SqlGuid)value;
			if (sqlGuid.IsNull || this.IsNull)
			{
				return sqlGuid.IsNull && this.IsNull;
			}
			return (this == sqlGuid).Value;
		}

		// Token: 0x06002D7E RID: 11646 RVA: 0x002AAD70 File Offset: 0x002AA170
		public override int GetHashCode()
		{
			if (!this.IsNull)
			{
				return this.Value.GetHashCode();
			}
			return 0;
		}

		// Token: 0x06002D7F RID: 11647 RVA: 0x002AAD9C File Offset: 0x002AA19C
		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		// Token: 0x06002D80 RID: 11648 RVA: 0x002AADAC File Offset: 0x002AA1AC
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			string attribute = reader.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance");
			if (attribute != null && XmlConvert.ToBoolean(attribute))
			{
				this.m_value = null;
				return;
			}
			this.m_value = new Guid(reader.ReadElementString()).ToByteArray();
		}

		// Token: 0x06002D81 RID: 11649 RVA: 0x002AADF8 File Offset: 0x002AA1F8
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			if (this.IsNull)
			{
				writer.WriteAttributeString("xsi", "nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
				return;
			}
			writer.WriteString(XmlConvert.ToString(new Guid(this.m_value)));
		}

		// Token: 0x06002D82 RID: 11650 RVA: 0x002AAE40 File Offset: 0x002AA240
		public static XmlQualifiedName GetXsdType(XmlSchemaSet schemaSet)
		{
			return new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
		}

		// Token: 0x04001CEB RID: 7403
		private static readonly int SizeOfGuid = 16;

		// Token: 0x04001CEC RID: 7404
		private static readonly int[] x_rgiGuidOrder = new int[]
		{
			10, 11, 12, 13, 14, 15, 8, 9, 6, 7,
			4, 5, 0, 1, 2, 3
		};

		// Token: 0x04001CED RID: 7405
		private byte[] m_value;

		// Token: 0x04001CEE RID: 7406
		public static readonly SqlGuid Null = new SqlGuid(true);
	}
}
