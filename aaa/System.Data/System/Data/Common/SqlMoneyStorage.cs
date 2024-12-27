using System;
using System.Collections;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace System.Data.Common
{
	// Token: 0x02000197 RID: 407
	internal sealed class SqlMoneyStorage : DataStorage
	{
		// Token: 0x060017F3 RID: 6131 RVA: 0x00234F14 File Offset: 0x00234314
		public SqlMoneyStorage(DataColumn column)
			: base(column, typeof(SqlMoney), SqlMoney.Null, SqlMoney.Null)
		{
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x00234F48 File Offset: 0x00234348
		public override object Aggregate(int[] records, AggregateType kind)
		{
			bool flag = false;
			try
			{
				switch (kind)
				{
				case AggregateType.Sum:
				{
					SqlDecimal sqlDecimal = 0L;
					foreach (int num in records)
					{
						if (!this.IsNull(num))
						{
							sqlDecimal += this.values[num];
							flag = true;
						}
					}
					if (flag)
					{
						return sqlDecimal;
					}
					return this.NullValue;
				}
				case AggregateType.Mean:
				{
					SqlDecimal sqlDecimal2 = 0L;
					int num2 = 0;
					foreach (int num3 in records)
					{
						if (!this.IsNull(num3))
						{
							sqlDecimal2 += this.values[num3].ToSqlDecimal();
							num2++;
							flag = true;
						}
					}
					if (flag)
					{
						SqlMoney sqlMoney = 0L;
						sqlMoney = (sqlDecimal2 / (long)num2).ToSqlMoney();
						return sqlMoney;
					}
					return this.NullValue;
				}
				case AggregateType.Min:
				{
					SqlMoney sqlMoney2 = SqlMoney.MaxValue;
					foreach (int num4 in records)
					{
						if (!this.IsNull(num4))
						{
							if (SqlMoney.LessThan(this.values[num4], sqlMoney2).IsTrue)
							{
								sqlMoney2 = this.values[num4];
							}
							flag = true;
						}
					}
					if (flag)
					{
						return sqlMoney2;
					}
					return this.NullValue;
				}
				case AggregateType.Max:
				{
					SqlMoney sqlMoney3 = SqlMoney.MinValue;
					foreach (int num5 in records)
					{
						if (!this.IsNull(num5))
						{
							if (SqlMoney.GreaterThan(this.values[num5], sqlMoney3).IsTrue)
							{
								sqlMoney3 = this.values[num5];
							}
							flag = true;
						}
					}
					if (flag)
					{
						return sqlMoney3;
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
				throw ExprException.Overflow(typeof(SqlMoney));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x0023539C File Offset: 0x0023479C
		public override int Compare(int recordNo1, int recordNo2)
		{
			return this.values[recordNo1].CompareTo(this.values[recordNo2]);
		}

		// Token: 0x060017F6 RID: 6134 RVA: 0x002353CC File Offset: 0x002347CC
		public override int CompareValueTo(int recordNo, object value)
		{
			return this.values[recordNo].CompareTo((SqlMoney)value);
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x002353F0 File Offset: 0x002347F0
		public override object ConvertValue(object value)
		{
			if (value != null)
			{
				return SqlConvert.ConvertToSqlMoney(value);
			}
			return this.NullValue;
		}

		// Token: 0x060017F8 RID: 6136 RVA: 0x00235414 File Offset: 0x00234814
		public override void Copy(int recordNo1, int recordNo2)
		{
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x00235444 File Offset: 0x00234844
		public override object Get(int record)
		{
			return this.values[record];
		}

		// Token: 0x060017FA RID: 6138 RVA: 0x00235468 File Offset: 0x00234868
		public override bool IsNull(int record)
		{
			return this.values[record].IsNull;
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x00235488 File Offset: 0x00234888
		public override void Set(int record, object value)
		{
			this.values[record] = SqlConvert.ConvertToSqlMoney(value);
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x002354AC File Offset: 0x002348AC
		public override void SetCapacity(int capacity)
		{
			SqlMoney[] array = new SqlMoney[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x002354EC File Offset: 0x002348EC
		public override object ConvertXmlToObject(string s)
		{
			SqlMoney sqlMoney = default(SqlMoney);
			string text = "<col>" + s + "</col>";
			StringReader stringReader = new StringReader(text);
			IXmlSerializable xmlSerializable = sqlMoney;
			using (XmlTextReader xmlTextReader = new XmlTextReader(stringReader))
			{
				xmlSerializable.ReadXml(xmlTextReader);
			}
			return (SqlMoney)xmlSerializable;
		}

		// Token: 0x060017FE RID: 6142 RVA: 0x00235564 File Offset: 0x00234964
		public override string ConvertObjectToXml(object value)
		{
			StringWriter stringWriter = new StringWriter(base.FormatProvider);
			using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
			{
				((IXmlSerializable)value).WriteXml(xmlTextWriter);
			}
			return stringWriter.ToString();
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x002355C0 File Offset: 0x002349C0
		protected override object GetEmptyStorage(int recordCount)
		{
			return new SqlMoney[recordCount];
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x002355D4 File Offset: 0x002349D4
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			SqlMoney[] array = (SqlMoney[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, this.IsNull(record));
		}

		// Token: 0x06001801 RID: 6145 RVA: 0x00235618 File Offset: 0x00234A18
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (SqlMoney[])store;
		}

		// Token: 0x04000D06 RID: 3334
		private SqlMoney[] values;
	}
}
