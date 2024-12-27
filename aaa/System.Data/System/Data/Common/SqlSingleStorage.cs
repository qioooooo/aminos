using System;
using System.Collections;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace System.Data.Common
{
	// Token: 0x02000198 RID: 408
	internal sealed class SqlSingleStorage : DataStorage
	{
		// Token: 0x06001802 RID: 6146 RVA: 0x00235634 File Offset: 0x00234A34
		public SqlSingleStorage(DataColumn column)
			: base(column, typeof(SqlSingle), SqlSingle.Null, SqlSingle.Null)
		{
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x00235668 File Offset: 0x00234A68
		public override object Aggregate(int[] records, AggregateType kind)
		{
			bool flag = false;
			try
			{
				switch (kind)
				{
				case AggregateType.Sum:
				{
					SqlSingle sqlSingle = 0f;
					foreach (int num in records)
					{
						if (!this.IsNull(num))
						{
							sqlSingle += this.values[num];
							flag = true;
						}
					}
					if (flag)
					{
						return sqlSingle;
					}
					return this.NullValue;
				}
				case AggregateType.Mean:
				{
					SqlDouble sqlDouble = 0.0;
					int num2 = 0;
					foreach (int num3 in records)
					{
						if (!this.IsNull(num3))
						{
							sqlDouble += this.values[num3].ToSqlDouble();
							num2++;
							flag = true;
						}
					}
					if (flag)
					{
						SqlSingle sqlSingle2 = 0f;
						sqlSingle2 = (sqlDouble / (double)num2).ToSqlSingle();
						return sqlSingle2;
					}
					return this.NullValue;
				}
				case AggregateType.Min:
				{
					SqlSingle sqlSingle3 = SqlSingle.MaxValue;
					foreach (int num4 in records)
					{
						if (!this.IsNull(num4))
						{
							if (SqlSingle.LessThan(this.values[num4], sqlSingle3).IsTrue)
							{
								sqlSingle3 = this.values[num4];
							}
							flag = true;
						}
					}
					if (flag)
					{
						return sqlSingle3;
					}
					return this.NullValue;
				}
				case AggregateType.Max:
				{
					SqlSingle sqlSingle4 = SqlSingle.MinValue;
					foreach (int num5 in records)
					{
						if (!this.IsNull(num5))
						{
							if (SqlSingle.GreaterThan(this.values[num5], sqlSingle4).IsTrue)
							{
								sqlSingle4 = this.values[num5];
							}
							flag = true;
						}
					}
					if (flag)
					{
						return sqlSingle4;
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
					SqlDouble sqlDouble2 = 0.0;
					SqlDouble sqlDouble3 = 0.0;
					SqlDouble sqlDouble4 = 0.0;
					SqlDouble sqlDouble5 = 0.0;
					foreach (int num7 in records)
					{
						if (!this.IsNull(num7))
						{
							sqlDouble4 += this.values[num7].ToSqlDouble();
							sqlDouble5 += this.values[num7].ToSqlDouble() * this.values[num7].ToSqlDouble();
							num6++;
						}
					}
					if (num6 <= 1)
					{
						return this.NullValue;
					}
					sqlDouble2 = (double)num6 * sqlDouble5 - sqlDouble4 * sqlDouble4;
					sqlDouble3 = sqlDouble2 / (sqlDouble4 * sqlDouble4);
					if (sqlDouble3 < 1E-15 || sqlDouble2 < 0.0)
					{
						sqlDouble2 = 0.0;
					}
					else
					{
						sqlDouble2 /= (double)(num6 * (num6 - 1));
					}
					if (kind == AggregateType.StDev)
					{
						return Math.Sqrt(sqlDouble2.Value);
					}
					return sqlDouble2;
				}
				}
			}
			catch (OverflowException)
			{
				throw ExprException.Overflow(typeof(SqlSingle));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x00235AC4 File Offset: 0x00234EC4
		public override int Compare(int recordNo1, int recordNo2)
		{
			return this.values[recordNo1].CompareTo(this.values[recordNo2]);
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x00235AF4 File Offset: 0x00234EF4
		public override int CompareValueTo(int recordNo, object value)
		{
			return this.values[recordNo].CompareTo((SqlSingle)value);
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x00235B18 File Offset: 0x00234F18
		public override object ConvertValue(object value)
		{
			if (value != null)
			{
				return SqlConvert.ConvertToSqlSingle(value);
			}
			return this.NullValue;
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x00235B3C File Offset: 0x00234F3C
		public override void Copy(int recordNo1, int recordNo2)
		{
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x00235B6C File Offset: 0x00234F6C
		public override object Get(int record)
		{
			return this.values[record];
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x00235B90 File Offset: 0x00234F90
		public override bool IsNull(int record)
		{
			return this.values[record].IsNull;
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x00235BB0 File Offset: 0x00234FB0
		public override void Set(int record, object value)
		{
			this.values[record] = SqlConvert.ConvertToSqlSingle(value);
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x00235BD4 File Offset: 0x00234FD4
		public override void SetCapacity(int capacity)
		{
			SqlSingle[] array = new SqlSingle[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x00235C14 File Offset: 0x00235014
		public override object ConvertXmlToObject(string s)
		{
			SqlSingle sqlSingle = default(SqlSingle);
			string text = "<col>" + s + "</col>";
			StringReader stringReader = new StringReader(text);
			IXmlSerializable xmlSerializable = sqlSingle;
			using (XmlTextReader xmlTextReader = new XmlTextReader(stringReader))
			{
				xmlSerializable.ReadXml(xmlTextReader);
			}
			return (SqlSingle)xmlSerializable;
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x00235C8C File Offset: 0x0023508C
		public override string ConvertObjectToXml(object value)
		{
			StringWriter stringWriter = new StringWriter(base.FormatProvider);
			using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
			{
				((IXmlSerializable)value).WriteXml(xmlTextWriter);
			}
			return stringWriter.ToString();
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x00235CE8 File Offset: 0x002350E8
		protected override object GetEmptyStorage(int recordCount)
		{
			return new SqlSingle[recordCount];
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x00235CFC File Offset: 0x002350FC
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			SqlSingle[] array = (SqlSingle[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, this.IsNull(record));
		}

		// Token: 0x06001810 RID: 6160 RVA: 0x00235D40 File Offset: 0x00235140
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (SqlSingle[])store;
		}

		// Token: 0x04000D07 RID: 3335
		private SqlSingle[] values;
	}
}
