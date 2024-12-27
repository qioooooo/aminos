using System;
using System.Collections;
using System.Xml;

namespace System.Data.Common
{
	// Token: 0x02000148 RID: 328
	internal sealed class DecimalStorage : DataStorage
	{
		// Token: 0x06001534 RID: 5428 RVA: 0x0022974C File Offset: 0x00228B4C
		internal DecimalStorage(DataColumn column)
			: base(column, typeof(decimal), DecimalStorage.defaultValue)
		{
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x00229774 File Offset: 0x00228B74
		public override object Aggregate(int[] records, AggregateType kind)
		{
			bool flag = false;
			try
			{
				switch (kind)
				{
				case AggregateType.Sum:
				{
					decimal num = DecimalStorage.defaultValue;
					foreach (int num2 in records)
					{
						if (base.HasValue(num2))
						{
							num += this.values[num2];
							flag = true;
						}
					}
					if (flag)
					{
						return num;
					}
					return this.NullValue;
				}
				case AggregateType.Mean:
				{
					decimal num3 = DecimalStorage.defaultValue;
					int num4 = 0;
					foreach (int num5 in records)
					{
						if (base.HasValue(num5))
						{
							num3 += this.values[num5];
							num4++;
							flag = true;
						}
					}
					if (flag)
					{
						decimal num6 = num3 / num4;
						return num6;
					}
					return this.NullValue;
				}
				case AggregateType.Min:
				{
					decimal num7 = decimal.MaxValue;
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
					decimal num9 = decimal.MinValue;
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
					return base.Aggregate(records, kind);
				case AggregateType.Var:
				case AggregateType.StDev:
				{
					int num11 = 0;
					double num12 = (double)DecimalStorage.defaultValue;
					double num13 = (double)DecimalStorage.defaultValue;
					double num14 = (double)DecimalStorage.defaultValue;
					double num15 = (double)DecimalStorage.defaultValue;
					foreach (int num16 in records)
					{
						if (base.HasValue(num16))
						{
							num14 += (double)this.values[num16];
							num15 += (double)this.values[num16] * (double)this.values[num16];
							num11++;
						}
					}
					if (num11 <= 1)
					{
						return this.NullValue;
					}
					num12 = (double)num11 * num15 - num14 * num14;
					num13 = num12 / (num14 * num14);
					if (num13 < 1E-15 || num12 < 0.0)
					{
						num12 = 0.0;
					}
					else
					{
						num12 /= (double)(num11 * (num11 - 1));
					}
					if (kind == AggregateType.StDev)
					{
						return Math.Sqrt(num12);
					}
					return num12;
				}
				}
			}
			catch (OverflowException)
			{
				throw ExprException.Overflow(typeof(decimal));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x00229B00 File Offset: 0x00228F00
		public override int Compare(int recordNo1, int recordNo2)
		{
			decimal num = this.values[recordNo1];
			decimal num2 = this.values[recordNo2];
			if (num == DecimalStorage.defaultValue || num2 == DecimalStorage.defaultValue)
			{
				int num3 = base.CompareBits(recordNo1, recordNo2);
				if (num3 != 0)
				{
					return num3;
				}
			}
			return decimal.Compare(num, num2);
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x00229B60 File Offset: 0x00228F60
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
				decimal num = this.values[recordNo];
				if (DecimalStorage.defaultValue == num && !base.HasValue(recordNo))
				{
					return -1;
				}
				return decimal.Compare(num, (decimal)value);
			}
		}

		// Token: 0x06001538 RID: 5432 RVA: 0x00229BBC File Offset: 0x00228FBC
		public override object ConvertValue(object value)
		{
			if (this.NullValue != value)
			{
				if (value != null)
				{
					value = ((IConvertible)value).ToDecimal(base.FormatProvider);
				}
				else
				{
					value = this.NullValue;
				}
			}
			return value;
		}

		// Token: 0x06001539 RID: 5433 RVA: 0x00229BF8 File Offset: 0x00228FF8
		public override void Copy(int recordNo1, int recordNo2)
		{
			base.CopyBits(recordNo1, recordNo2);
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x00229C30 File Offset: 0x00229030
		public override object Get(int record)
		{
			if (!base.HasValue(record))
			{
				return this.NullValue;
			}
			return this.values[record];
		}

		// Token: 0x0600153B RID: 5435 RVA: 0x00229C64 File Offset: 0x00229064
		public override void Set(int record, object value)
		{
			if (this.NullValue == value)
			{
				this.values[record] = DecimalStorage.defaultValue;
				base.SetNullBit(record, true);
				return;
			}
			this.values[record] = ((IConvertible)value).ToDecimal(base.FormatProvider);
			base.SetNullBit(record, false);
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x00229CC4 File Offset: 0x002290C4
		public override void SetCapacity(int capacity)
		{
			decimal[] array = new decimal[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
			base.SetCapacity(capacity);
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x00229D0C File Offset: 0x0022910C
		public override object ConvertXmlToObject(string s)
		{
			return XmlConvert.ToDecimal(s);
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x00229D24 File Offset: 0x00229124
		public override string ConvertObjectToXml(object value)
		{
			return XmlConvert.ToString((decimal)value);
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x00229D3C File Offset: 0x0022913C
		protected override object GetEmptyStorage(int recordCount)
		{
			return new decimal[recordCount];
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x00229D50 File Offset: 0x00229150
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			decimal[] array = (decimal[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, !base.HasValue(record));
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x00229D94 File Offset: 0x00229194
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (decimal[])store;
			base.SetNullStorage(nullbits);
		}

		// Token: 0x04000C77 RID: 3191
		private static readonly decimal defaultValue;

		// Token: 0x04000C78 RID: 3192
		private decimal[] values;
	}
}
