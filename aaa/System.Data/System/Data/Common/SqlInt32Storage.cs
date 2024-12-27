using System;
using System.Collections;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace System.Data.Common
{
	// Token: 0x02000195 RID: 405
	internal sealed class SqlInt32Storage : DataStorage
	{
		// Token: 0x060017D5 RID: 6101 RVA: 0x002340D8 File Offset: 0x002334D8
		public SqlInt32Storage(DataColumn column)
			: base(column, typeof(SqlInt32), SqlInt32.Null, SqlInt32.Null)
		{
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x0023410C File Offset: 0x0023350C
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
						SqlInt32 sqlInt3 = 0;
						sqlInt3 = (sqlInt2 / (long)num2).ToSqlInt32();
						return sqlInt3;
					}
					return this.NullValue;
				}
				case AggregateType.Min:
				{
					SqlInt32 sqlInt4 = SqlInt32.MaxValue;
					foreach (int num4 in records)
					{
						if (!this.IsNull(num4))
						{
							if (SqlInt32.LessThan(this.values[num4], sqlInt4).IsTrue)
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
					SqlInt32 sqlInt5 = SqlInt32.MinValue;
					foreach (int num5 in records)
					{
						if (!this.IsNull(num5))
						{
							if (SqlInt32.GreaterThan(this.values[num5], sqlInt5).IsTrue)
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
				throw ExprException.Overflow(typeof(SqlInt32));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x00234560 File Offset: 0x00233960
		public override int Compare(int recordNo1, int recordNo2)
		{
			return this.values[recordNo1].CompareTo(this.values[recordNo2]);
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x00234590 File Offset: 0x00233990
		public override int CompareValueTo(int recordNo, object value)
		{
			return this.values[recordNo].CompareTo((SqlInt32)value);
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x002345B4 File Offset: 0x002339B4
		public override object ConvertValue(object value)
		{
			if (value != null)
			{
				return SqlConvert.ConvertToSqlInt32(value);
			}
			return this.NullValue;
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x002345D8 File Offset: 0x002339D8
		public override void Copy(int recordNo1, int recordNo2)
		{
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x00234608 File Offset: 0x00233A08
		public override object Get(int record)
		{
			return this.values[record];
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x0023462C File Offset: 0x00233A2C
		public override bool IsNull(int record)
		{
			return this.values[record].IsNull;
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x0023464C File Offset: 0x00233A4C
		public override void Set(int record, object value)
		{
			this.values[record] = SqlConvert.ConvertToSqlInt32(value);
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x00234670 File Offset: 0x00233A70
		public override void SetCapacity(int capacity)
		{
			SqlInt32[] array = new SqlInt32[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x002346B0 File Offset: 0x00233AB0
		public override object ConvertXmlToObject(string s)
		{
			SqlInt32 sqlInt = default(SqlInt32);
			string text = "<col>" + s + "</col>";
			StringReader stringReader = new StringReader(text);
			IXmlSerializable xmlSerializable = sqlInt;
			using (XmlTextReader xmlTextReader = new XmlTextReader(stringReader))
			{
				xmlSerializable.ReadXml(xmlTextReader);
			}
			return (SqlInt32)xmlSerializable;
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x00234728 File Offset: 0x00233B28
		public override string ConvertObjectToXml(object value)
		{
			StringWriter stringWriter = new StringWriter(base.FormatProvider);
			using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
			{
				((IXmlSerializable)value).WriteXml(xmlTextWriter);
			}
			return stringWriter.ToString();
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x00234784 File Offset: 0x00233B84
		protected override object GetEmptyStorage(int recordCount)
		{
			return new SqlInt32[recordCount];
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x00234798 File Offset: 0x00233B98
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			SqlInt32[] array = (SqlInt32[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, this.IsNull(record));
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x002347DC File Offset: 0x00233BDC
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (SqlInt32[])store;
		}

		// Token: 0x04000D04 RID: 3332
		private SqlInt32[] values;
	}
}
