using System;
using System.Collections;
using System.Xml;

namespace System.Data.Common
{
	// Token: 0x0200014F RID: 335
	internal sealed class Int16Storage : DataStorage
	{
		// Token: 0x06001560 RID: 5472 RVA: 0x0022A6EC File Offset: 0x00229AEC
		internal Int16Storage(DataColumn column)
			: base(column, typeof(short), 0)
		{
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x0022A710 File Offset: 0x00229B10
		public override object Aggregate(int[] records, AggregateType kind)
		{
			bool flag = false;
			try
			{
				switch (kind)
				{
				case AggregateType.Sum:
				{
					long num = 0L;
					checked
					{
						foreach (int num2 in records)
						{
							if (base.HasValue(num2))
							{
								num += unchecked((long)this.values[num2]);
								flag = true;
							}
						}
						if (flag)
						{
							return num;
						}
						return this.NullValue;
					}
				}
				case AggregateType.Mean:
				{
					long num3 = 0L;
					int num4 = 0;
					foreach (int num5 in records)
					{
						if (base.HasValue(num5))
						{
							checked
							{
								num3 += unchecked((long)this.values[num5]);
							}
							num4++;
							flag = true;
						}
					}
					checked
					{
						if (flag)
						{
							short num6 = (short)(num3 / unchecked((long)num4));
							return num6;
						}
						return this.NullValue;
					}
				}
				case AggregateType.Min:
				{
					short num7 = short.MaxValue;
					foreach (int num8 in records)
					{
						if (base.HasValue(num8))
						{
							num7 = Math.Min(this.values[num8], num7);
							flag = true;
						}
					}
					if (flag)
					{
						return num7;
					}
					return this.NullValue;
				}
				case AggregateType.Max:
				{
					short num9 = short.MinValue;
					foreach (int num10 in records)
					{
						if (base.HasValue(num10))
						{
							num9 = Math.Max(this.values[num10], num9);
							flag = true;
						}
					}
					if (flag)
					{
						return num9;
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
					int num11 = 0;
					for (int m = 0; m < records.Length; m++)
					{
						if (base.HasValue(records[m]))
						{
							num11++;
						}
					}
					return num11;
				}
				case AggregateType.Var:
				case AggregateType.StDev:
				{
					int num11 = 0;
					double num12 = 0.0;
					double num13 = 0.0;
					foreach (int num14 in records)
					{
						if (base.HasValue(num14))
						{
							num12 += (double)this.values[num14];
							num13 += (double)this.values[num14] * (double)this.values[num14];
							num11++;
						}
					}
					if (num11 <= 1)
					{
						return this.NullValue;
					}
					double num15 = (double)num11 * num13 - num12 * num12;
					double num16 = num15 / (num12 * num12);
					if (num16 < 1E-15 || num15 < 0.0)
					{
						num15 = 0.0;
					}
					else
					{
						num15 /= (double)(num11 * (num11 - 1));
					}
					if (kind == AggregateType.StDev)
					{
						return Math.Sqrt(num15);
					}
					return num15;
				}
				}
			}
			catch (OverflowException)
			{
				throw ExprException.Overflow(typeof(short));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x0022AA40 File Offset: 0x00229E40
		public override int Compare(int recordNo1, int recordNo2)
		{
			short num = this.values[recordNo1];
			short num2 = this.values[recordNo2];
			if (num == 0 || num2 == 0)
			{
				int num3 = base.CompareBits(recordNo1, recordNo2);
				if (num3 != 0)
				{
					return num3;
				}
			}
			return (int)(num - num2);
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x0022AA78 File Offset: 0x00229E78
		public override int CompareValueTo(int recordNo, object value)
		{
			if (this.NullValue == value)
			{
				if (!base.HasValue(recordNo))
				{
					return 0;
				}
				return 1;
			}
			else
			{
				short num = this.values[recordNo];
				if (num == 0 && !base.HasValue(recordNo))
				{
					return -1;
				}
				return num.CompareTo((short)value);
			}
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x0022AAC0 File Offset: 0x00229EC0
		public override object ConvertValue(object value)
		{
			if (this.NullValue != value)
			{
				if (value != null)
				{
					value = ((IConvertible)value).ToInt16(base.FormatProvider);
				}
				else
				{
					value = this.NullValue;
				}
			}
			return value;
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x0022AAFC File Offset: 0x00229EFC
		public override void Copy(int recordNo1, int recordNo2)
		{
			base.CopyBits(recordNo1, recordNo2);
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x0022AB24 File Offset: 0x00229F24
		public override object Get(int record)
		{
			short num = this.values[record];
			if (num != 0)
			{
				return num;
			}
			return base.GetBits(record);
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x0022AB4C File Offset: 0x00229F4C
		public override void Set(int record, object value)
		{
			if (this.NullValue == value)
			{
				this.values[record] = 0;
				base.SetNullBit(record, true);
				return;
			}
			this.values[record] = ((IConvertible)value).ToInt16(base.FormatProvider);
			base.SetNullBit(record, false);
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x0022AB98 File Offset: 0x00229F98
		public override void SetCapacity(int capacity)
		{
			short[] array = new short[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
			base.SetCapacity(capacity);
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x0022ABE0 File Offset: 0x00229FE0
		public override object ConvertXmlToObject(string s)
		{
			return XmlConvert.ToInt16(s);
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x0022ABF8 File Offset: 0x00229FF8
		public override string ConvertObjectToXml(object value)
		{
			return XmlConvert.ToString((short)value);
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x0022AC10 File Offset: 0x0022A010
		protected override object GetEmptyStorage(int recordCount)
		{
			return new short[recordCount];
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x0022AC24 File Offset: 0x0022A024
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			short[] array = (short[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, !base.HasValue(record));
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x0022AC58 File Offset: 0x0022A058
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (short[])store;
			base.SetNullStorage(nullbits);
		}

		// Token: 0x04000C8E RID: 3214
		private const short defaultValue = 0;

		// Token: 0x04000C8F RID: 3215
		private short[] values;
	}
}
