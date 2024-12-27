using System;
using System.Collections;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace System.Data.Common
{
	// Token: 0x02000194 RID: 404
	internal sealed class SqlInt16Storage : DataStorage
	{
		// Token: 0x060017C6 RID: 6086 RVA: 0x002339B8 File Offset: 0x00232DB8
		public SqlInt16Storage(DataColumn column)
			: base(column, typeof(SqlInt16), SqlInt16.Null, SqlInt16.Null)
		{
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x002339EC File Offset: 0x00232DEC
		public override object Aggregate(int[] records, AggregateType kind)
		{
			bool flag = false;
			try
			{
				switch (kind)
				{
				case AggregateType.Sum:
				{
					SqlInt64 sqlInt = 0L;
					foreach (int num in records)
					{
						if (!this.IsNull(num))
						{
							sqlInt += this.values[num];
							flag = true;
						}
					}
					if (flag)
					{
						return sqlInt;
					}
					return this.NullValue;
				}
				case AggregateType.Mean:
				{
					SqlInt64 sqlInt2 = 0L;
					int num2 = 0;
					foreach (int num3 in records)
					{
						if (!this.IsNull(num3))
						{
							sqlInt2 += this.values[num3].ToSqlInt64();
							num2++;
							flag = true;
						}
					}
					if (flag)
					{
						SqlInt16 sqlInt3 = 0;
						sqlInt3 = (sqlInt2 / (long)num2).ToSqlInt16();
						return sqlInt3;
					}
					return this.NullValue;
				}
				case AggregateType.Min:
				{
					SqlInt16 sqlInt4 = SqlInt16.MaxValue;
					foreach (int num4 in records)
					{
						if (!this.IsNull(num4))
						{
							if (SqlInt16.LessThan(this.values[num4], sqlInt4).IsTrue)
							{
								sqlInt4 = this.values[num4];
							}
							flag = true;
						}
					}
					if (flag)
					{
						return sqlInt4;
					}
					return this.NullValue;
				}
				case AggregateType.Max:
				{
					SqlInt16 sqlInt5 = SqlInt16.MinValue;
					foreach (int num5 in records)
					{
						if (!this.IsNull(num5))
						{
							if (SqlInt16.GreaterThan(this.values[num5], sqlInt5).IsTrue)
							{
								sqlInt5 = this.values[num5];
							}
							flag = true;
						}
					}
					if (flag)
					{
						return sqlInt5;
					}
					return this.NullValue;
				}
				case AggregateType.First:
					if (records.Length > 0)
					{
						return this.values[records[0]];
					}
					return null;
				case AggregateType.Count:
				{
					int num6 = 0;
					for (int m = 0; m < records.Length; m++)
					{
						if (!this.IsNull(records[m]))
						{
							num6++;
						}
					}
					return num6;
				}
				case AggregateType.Var:
				case AggregateType.StDev:
				{
					int num6 = 0;
					SqlDouble sqlDouble = 0.0;
					SqlDouble sqlDouble2 = 0.0;
					SqlDouble sqlDouble3 = 0.0;
					SqlDouble sqlDouble4 = 0.0;
					foreach (int num7 in records)
					{
						if (!this.IsNull(num7))
						{
							sqlDouble3 += this.values[num7].ToSqlDouble();
							sqlDouble4 += this.values[num7].ToSqlDouble() * this.values[num7].ToSqlDouble();
							num6++;
						}
					}
					if (num6 <= 1)
					{
						return this.NullValue;
					}
					sqlDouble = (double)num6 * sqlDouble4 - sqlDouble3 * sqlDouble3;
					sqlDouble2 = sqlDouble / (sqlDouble3 * sqlDouble3);
					if (sqlDouble2 < 1E-15 || sqlDouble < 0.0)
					{
						sqlDouble = 0.0;
					}
					else
					{
						sqlDouble /= (double)(num6 * (num6 - 1));
					}
					if (kind == AggregateType.StDev)
					{
						return Math.Sqrt(sqlDouble.Value);
					}
					return sqlDouble;
				}
				}
			}
			catch (OverflowException)
			{
				throw ExprException.Overflow(typeof(SqlInt16));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x00233E40 File Offset: 0x00233240
		public override int Compare(int recordNo1, int recordNo2)
		{
			return this.values[recordNo1].CompareTo(this.values[recordNo2]);
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x00233E70 File Offset: 0x00233270
		public override int CompareValueTo(int recordNo, object value)
		{
			return this.values[recordNo].CompareTo((SqlInt16)value);
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x00233E94 File Offset: 0x00233294
		public override object ConvertValue(object value)
		{
			if (value != null)
			{
				return SqlConvert.ConvertToSqlInt16(value);
			}
			return this.NullValue;
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x00233EB8 File Offset: 0x002332B8
		public override void Copy(int recordNo1, int recordNo2)
		{
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x00233EE8 File Offset: 0x002332E8
		public override object Get(int record)
		{
			return this.values[record];
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x00233F0C File Offset: 0x0023330C
		public override bool IsNull(int record)
		{
			return this.values[record].IsNull;
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x00233F2C File Offset: 0x0023332C
		public override void Set(int record, object value)
		{
			this.values[record] = SqlConvert.ConvertToSqlInt16(value);
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x00233F50 File Offset: 0x00233350
		public override void SetCapacity(int capacity)
		{
			SqlInt16[] array = new SqlInt16[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x00233F90 File Offset: 0x00233390
		public override object ConvertXmlToObject(string s)
		{
			SqlInt16 sqlInt = default(SqlInt16);
			string text = "<col>" + s + "</col>";
			StringReader stringReader = new StringReader(text);
			IXmlSerializable xmlSerializable = sqlInt;
			using (XmlTextReader xmlTextReader = new XmlTextReader(stringReader))
			{
				xmlSerializable.ReadXml(xmlTextReader);
			}
			return (SqlInt16)xmlSerializable;
		}

		// Token: 0x060017D1 RID: 6097 RVA: 0x00234008 File Offset: 0x00233408
		public override string ConvertObjectToXml(object value)
		{
			StringWriter stringWriter = new StringWriter(base.FormatProvider);
			using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
			{
				((IXmlSerializable)value).WriteXml(xmlTextWriter);
			}
			return stringWriter.ToString();
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x00234064 File Offset: 0x00233464
		protected override object GetEmptyStorage(int recordCount)
		{
			return new SqlInt16[recordCount];
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x00234078 File Offset: 0x00233478
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			SqlInt16[] array = (SqlInt16[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, this.IsNull(record));
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x002340BC File Offset: 0x002334BC
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (SqlInt16[])store;
		}

		// Token: 0x04000D03 RID: 3331
		private SqlInt16[] values;
	}
}
