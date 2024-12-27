using System;
using System.Collections;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace System.Data.Common
{
	// Token: 0x0200018E RID: 398
	internal sealed class SqlByteStorage : DataStorage
	{
		// Token: 0x0600176D RID: 5997 RVA: 0x00231980 File Offset: 0x00230D80
		public SqlByteStorage(DataColumn column)
			: base(column, typeof(SqlByte), SqlByte.Null, SqlByte.Null)
		{
		}

		// Token: 0x0600176E RID: 5998 RVA: 0x002319B4 File Offset: 0x00230DB4
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
						SqlByte sqlByte = 0;
						sqlByte = (sqlInt2 / (long)num2).ToSqlByte();
						return sqlByte;
					}
					return this.NullValue;
				}
				case AggregateType.Min:
				{
					SqlByte sqlByte2 = SqlByte.MaxValue;
					foreach (int num4 in records)
					{
						if (!this.IsNull(num4))
						{
							if (SqlByte.LessThan(this.values[num4], sqlByte2).IsTrue)
							{
								sqlByte2 = this.values[num4];
							}
							flag = true;
						}
					}
					if (flag)
					{
						return sqlByte2;
					}
					return this.NullValue;
				}
				case AggregateType.Max:
				{
					SqlByte sqlByte3 = SqlByte.MinValue;
					foreach (int num5 in records)
					{
						if (!this.IsNull(num5))
						{
							if (SqlByte.GreaterThan(this.values[num5], sqlByte3).IsTrue)
							{
								sqlByte3 = this.values[num5];
							}
							flag = true;
						}
					}
					if (flag)
					{
						return sqlByte3;
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
				throw ExprException.Overflow(typeof(SqlByte));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x00231E08 File Offset: 0x00231208
		public override int Compare(int recordNo1, int recordNo2)
		{
			return this.values[recordNo1].CompareTo(this.values[recordNo2]);
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x00231E38 File Offset: 0x00231238
		public override int CompareValueTo(int recordNo, object value)
		{
			return this.values[recordNo].CompareTo((SqlByte)value);
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x00231E5C File Offset: 0x0023125C
		public override object ConvertValue(object value)
		{
			if (value != null)
			{
				return SqlConvert.ConvertToSqlByte(value);
			}
			return this.NullValue;
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x00231E80 File Offset: 0x00231280
		public override void Copy(int recordNo1, int recordNo2)
		{
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x00231EB0 File Offset: 0x002312B0
		public override object Get(int record)
		{
			return this.values[record];
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x00231ED4 File Offset: 0x002312D4
		public override bool IsNull(int record)
		{
			return this.values[record].IsNull;
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x00231EF4 File Offset: 0x002312F4
		public override void Set(int record, object value)
		{
			this.values[record] = SqlConvert.ConvertToSqlByte(value);
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x00231F18 File Offset: 0x00231318
		public override void SetCapacity(int capacity)
		{
			SqlByte[] array = new SqlByte[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
		}

		// Token: 0x06001777 RID: 6007 RVA: 0x00231F58 File Offset: 0x00231358
		public override object ConvertXmlToObject(string s)
		{
			SqlByte sqlByte = default(SqlByte);
			string text = "<col>" + s + "</col>";
			StringReader stringReader = new StringReader(text);
			IXmlSerializable xmlSerializable = sqlByte;
			using (XmlTextReader xmlTextReader = new XmlTextReader(stringReader))
			{
				xmlSerializable.ReadXml(xmlTextReader);
			}
			return (SqlByte)xmlSerializable;
		}

		// Token: 0x06001778 RID: 6008 RVA: 0x00231FD0 File Offset: 0x002313D0
		public override string ConvertObjectToXml(object value)
		{
			StringWriter stringWriter = new StringWriter(base.FormatProvider);
			using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
			{
				((IXmlSerializable)value).WriteXml(xmlTextWriter);
			}
			return stringWriter.ToString();
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x0023202C File Offset: 0x0023142C
		protected override object GetEmptyStorage(int recordCount)
		{
			return new SqlByte[recordCount];
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x00232040 File Offset: 0x00231440
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			SqlByte[] array = (SqlByte[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(record, this.IsNull(record));
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x00232080 File Offset: 0x00231480
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (SqlByte[])store;
		}

		// Token: 0x04000CFD RID: 3325
		private SqlByte[] values;
	}
}
