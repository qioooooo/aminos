using System;
using System.Collections;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace System.Data.Common
{
	// Token: 0x02000192 RID: 402
	internal sealed class SqlDoubleStorage : DataStorage
	{
		// Token: 0x060017A8 RID: 6056 RVA: 0x00232F14 File Offset: 0x00232314
		public SqlDoubleStorage(DataColumn column)
			: base(column, typeof(SqlDouble), SqlDouble.Null, SqlDouble.Null)
		{
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x00232F48 File Offset: 0x00232348
		public override object Aggregate(int[] records, AggregateType kind)
		{
			bool flag = false;
			try
			{
				switch (kind)
				{
				case AggregateType.Sum:
				{
					SqlDouble sqlDouble = 0.0;
					foreach (int num in records)
					{
						if (!this.IsNull(num))
						{
							sqlDouble += this.values[num];
							flag = true;
						}
					}
					if (flag)
					{
						return sqlDouble;
					}
					return this.NullValue;
				}
				case AggregateType.Mean:
				{
					SqlDouble sqlDouble2 = 0.0;
					int num2 = 0;
					foreach (int num3 in records)
					{
						if (!this.IsNull(num3))
						{
							sqlDouble2 += this.values[num3];
							num2++;
							flag = true;
						}
					}
					if (flag)
					{
						SqlDouble sqlDouble3 = 0.0;
						sqlDouble3 = sqlDouble2 / (double)num2;
						return sqlDouble3;
					}
					return this.NullValue;
				}
				case AggregateType.Min:
				{
					SqlDouble sqlDouble4 = SqlDouble.MaxValue;
					foreach (int num4 in records)
					{
						if (!this.IsNull(num4))
						{
							if (SqlDouble.LessThan(this.values[num4], sqlDouble4).IsTrue)
							{
								sqlDouble4 = this.values[num4];
							}
							flag = true;
						}
					}
					if (flag)
					{
						return sqlDouble4;
					}
					return this.NullValue;
				}
				case AggregateType.Max:
				{
					SqlDouble sqlDouble5 = SqlDouble.MinValue;
					foreach (int num5 in records)
					{
						if (!this.IsNull(num5))
						{
							if (SqlDouble.GreaterThan(this.values[num5], sqlDouble5).IsTrue)
							{
								sqlDouble5 = this.values[num5];
							}
							flag = true;
						}
					}
					if (flag)
					{
						return sqlDouble5;
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
					SqlDouble sqlDouble6 = 0.0;
					SqlDouble sqlDouble7 = 0.0;
					SqlDouble sqlDouble8 = 0.0;
					SqlDouble sqlDouble9 = 0.0;
					foreach (int num7 in records)
					{
						if (!this.IsNull(num7))
						{
							sqlDouble8 += this.values[num7];
							sqlDouble9 += this.values[num7] * this.values[num7];
							num6++;
						}
					}
					if (num6 <= 1)
					{
						return this.NullValue;
					}
					sqlDouble6 = (double)num6 * sqlDouble9 - sqlDouble8 * sqlDouble8;
					sqlDouble7 = sqlDouble6 / (sqlDouble8 * sqlDouble8);
					if (sqlDouble7 < 1E-15 || sqlDouble6 < 0.0)
					{
						sqlDouble6 = 0.0;
					}
					else
					{
						sqlDouble6 /= (double)(num6 * (num6 - 1));
					}
					if (kind == AggregateType.StDev)
					{
						return Math.Sqrt(sqlDouble6.Value);
					}
					return sqlDouble6;
				}
				}
			}
			catch (OverflowException)
			{
				throw ExprException.Overflow(typeof(SqlDouble));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x002333A4 File Offset: 0x002327A4
		public override int Compare(int recordNo1, int recordNo2)
		{
			return this.values[recordNo1].CompareTo(this.values[recordNo2]);
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x002333D4 File Offset: 0x002327D4
		public override int CompareValueTo(int recordNo, object value)
		{
			return this.values[recordNo].CompareTo((SqlDouble)value);
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x002333F8 File Offset: 0x002327F8
		public override object ConvertValue(object value)
		{
			if (value != null)
			{
				return SqlConvert.ConvertToSqlDouble(value);
			}
			return this.NullValue;
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x0023341C File Offset: 0x0023281C
		public override void Copy(int recordNo1, int recordNo2)
		{
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x0023344C File Offset: 0x0023284C
		public override object Get(int record)
		{
			return this.values[record];
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x00233470 File Offset: 0x00232870
		public override bool IsNull(int record)
		{
			return this.values[record].IsNull;
		}

		// Token: 0x060017B0 RID: 6064 RVA: 0x00233490 File Offset: 0x00232890
		public override void Set(int record, object value)
		{
			this.values[record] = SqlConvert.ConvertToSqlDouble(value);
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x002334B4 File Offset: 0x002328B4
		public override void SetCapacity(int capacity)
		{
			SqlDouble[] array = new SqlDouble[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x002334F4 File Offset: 0x002328F4
		public override object ConvertXmlToObject(string s)
		{
			SqlDouble sqlDouble = default(SqlDouble);
			string text = "<col>" + s + "</col>";
			StringReader stringReader = new StringReader(text);
			IXmlSerializable xmlSerializable = sqlDouble;
			using (XmlTextReader xmlTextReader = new XmlTextReader(stringReader))
			{
				xmlSerializable.ReadXml(xmlTextReader);
			}
			return (SqlDouble)xmlSerializable;
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x0023356C File Offset: 0x0023296C
		public override string ConvertObjectToXml(object value)
		{
			StringWriter stringWriter = new StringWriter(base.FormatProvider);
			using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
			{
				((IXmlSerializable)value).WriteXml(xmlTextWriter);
			}
			return stringWriter.ToString();
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x002335C8 File Offset: 0x002329C8
		protected override object GetEmptyStorage(int recordCount)
		{
			return new SqlDouble[recordCount];
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x002335DC File Offset: 0x002329DC
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			SqlDouble[] array = (SqlDouble[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, this.IsNull(record));
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x00233620 File Offset: 0x00232A20
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (SqlDouble[])store;
		}

		// Token: 0x04000D01 RID: 3329
		private SqlDouble[] values;
	}
}
