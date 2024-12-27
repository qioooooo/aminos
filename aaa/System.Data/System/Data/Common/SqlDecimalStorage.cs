using System;
using System.Collections;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace System.Data.Common
{
	// Token: 0x02000191 RID: 401
	internal sealed class SqlDecimalStorage : DataStorage
	{
		// Token: 0x06001799 RID: 6041 RVA: 0x00232800 File Offset: 0x00231C00
		public SqlDecimalStorage(DataColumn column)
			: base(column, typeof(SqlDecimal), SqlDecimal.Null, SqlDecimal.Null)
		{
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x00232834 File Offset: 0x00231C34
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
							sqlDecimal2 += this.values[num3];
							num2++;
							flag = true;
						}
					}
					if (flag)
					{
						SqlDecimal sqlDecimal3 = 0L;
						sqlDecimal3 = sqlDecimal2 / (long)num2;
						return sqlDecimal3;
					}
					return this.NullValue;
				}
				case AggregateType.Min:
				{
					SqlDecimal sqlDecimal4 = SqlDecimal.MaxValue;
					foreach (int num4 in records)
					{
						if (!this.IsNull(num4))
						{
							if (SqlDecimal.LessThan(this.values[num4], sqlDecimal4).IsTrue)
							{
								sqlDecimal4 = this.values[num4];
							}
							flag = true;
						}
					}
					if (flag)
					{
						return sqlDecimal4;
					}
					return this.NullValue;
				}
				case AggregateType.Max:
				{
					SqlDecimal sqlDecimal5 = SqlDecimal.MinValue;
					foreach (int num5 in records)
					{
						if (!this.IsNull(num5))
						{
							if (SqlDecimal.GreaterThan(this.values[num5], sqlDecimal5).IsTrue)
							{
								sqlDecimal5 = this.values[num5];
							}
							flag = true;
						}
					}
					if (flag)
					{
						return sqlDecimal5;
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
				throw ExprException.Overflow(typeof(SqlDecimal));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x00232C7C File Offset: 0x0023207C
		public override int Compare(int recordNo1, int recordNo2)
		{
			return this.values[recordNo1].CompareTo(this.values[recordNo2]);
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x00232CAC File Offset: 0x002320AC
		public override int CompareValueTo(int recordNo, object value)
		{
			return this.values[recordNo].CompareTo((SqlDecimal)value);
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x00232CD0 File Offset: 0x002320D0
		public override object ConvertValue(object value)
		{
			if (value != null)
			{
				return SqlConvert.ConvertToSqlDecimal(value);
			}
			return this.NullValue;
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x00232CF4 File Offset: 0x002320F4
		public override void Copy(int recordNo1, int recordNo2)
		{
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x00232D24 File Offset: 0x00232124
		public override object Get(int record)
		{
			return this.values[record];
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x00232D48 File Offset: 0x00232148
		public override bool IsNull(int record)
		{
			return this.values[record].IsNull;
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x00232D68 File Offset: 0x00232168
		public override void Set(int record, object value)
		{
			this.values[record] = SqlConvert.ConvertToSqlDecimal(value);
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x00232D8C File Offset: 0x0023218C
		public override void SetCapacity(int capacity)
		{
			SqlDecimal[] array = new SqlDecimal[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x00232DCC File Offset: 0x002321CC
		public override object ConvertXmlToObject(string s)
		{
			SqlDecimal sqlDecimal = default(SqlDecimal);
			string text = "<col>" + s + "</col>";
			StringReader stringReader = new StringReader(text);
			IXmlSerializable xmlSerializable = sqlDecimal;
			using (XmlTextReader xmlTextReader = new XmlTextReader(stringReader))
			{
				xmlSerializable.ReadXml(xmlTextReader);
			}
			return (SqlDecimal)xmlSerializable;
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x00232E44 File Offset: 0x00232244
		public override string ConvertObjectToXml(object value)
		{
			StringWriter stringWriter = new StringWriter(base.FormatProvider);
			using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
			{
				((IXmlSerializable)value).WriteXml(xmlTextWriter);
			}
			return stringWriter.ToString();
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x00232EA0 File Offset: 0x002322A0
		protected override object GetEmptyStorage(int recordCount)
		{
			return new SqlDecimal[recordCount];
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x00232EB4 File Offset: 0x002322B4
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			SqlDecimal[] array = (SqlDecimal[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, this.IsNull(record));
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x00232EF8 File Offset: 0x002322F8
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (SqlDecimal[])store;
		}

		// Token: 0x04000D00 RID: 3328
		private SqlDecimal[] values;
	}
}
