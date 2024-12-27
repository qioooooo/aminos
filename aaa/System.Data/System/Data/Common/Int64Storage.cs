using System;
using System.Collections;
using System.Xml;

namespace System.Data.Common
{
	// Token: 0x02000151 RID: 337
	internal sealed class Int64Storage : DataStorage
	{
		// Token: 0x0600157C RID: 5500 RVA: 0x0022B20C File Offset: 0x0022A60C
		internal Int64Storage(DataColumn column)
			: base(column, typeof(long), 0L)
		{
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x0022B234 File Offset: 0x0022A634
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
				}
				case AggregateType.Mean:
				{
					decimal num3 = 0m;
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
						long num6 = (long)(num3 / num4);
						return num6;
					}
					return this.NullValue;
				}
				case AggregateType.Min:
				{
					long num7 = long.MaxValue;
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
					long num9 = long.MinValue;
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
				throw ExprException.Overflow(typeof(long));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x0022B560 File Offset: 0x0022A960
		public override int Compare(int recordNo1, int recordNo2)
		{
			long num = this.values[recordNo1];
			long num2 = this.values[recordNo2];
			if (num == 0L || num2 == 0L)
			{
				int num3 = base.CompareBits(recordNo1, recordNo2);
				if (num3 != 0)
				{
					return num3;
				}
			}
			if (num < num2)
			{
				return -1;
			}
			if (num <= num2)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x0022B5A4 File Offset: 0x0022A9A4
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
				long num = this.values[recordNo];
				if (0L == num && !base.HasValue(recordNo))
				{
					return -1;
				}
				return num.CompareTo((long)value);
			}
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x0022B5F0 File Offset: 0x0022A9F0
		public override object ConvertValue(object value)
		{
			if (this.NullValue != value)
			{
				if (value != null)
				{
					value = ((IConvertible)value).ToInt64(base.FormatProvider);
				}
				else
				{
					value = this.NullValue;
				}
			}
			return value;
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x0022B62C File Offset: 0x0022AA2C
		public override void Copy(int recordNo1, int recordNo2)
		{
			base.CopyBits(recordNo1, recordNo2);
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x0022B654 File Offset: 0x0022AA54
		public override object Get(int record)
		{
			long num = this.values[record];
			if (num != 0L)
			{
				return num;
			}
			return base.GetBits(record);
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x0022B680 File Offset: 0x0022AA80
		public override void Set(int record, object value)
		{
			if (this.NullValue == value)
			{
				this.values[record] = 0L;
				base.SetNullBit(record, true);
				return;
			}
			this.values[record] = ((IConvertible)value).ToInt64(base.FormatProvider);
			base.SetNullBit(record, false);
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x0022B6CC File Offset: 0x0022AACC
		public override void SetCapacity(int capacity)
		{
			long[] array = new long[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
			base.SetCapacity(capacity);
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x0022B714 File Offset: 0x0022AB14
		public override object ConvertXmlToObject(string s)
		{
			return XmlConvert.ToInt64(s);
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x0022B72C File Offset: 0x0022AB2C
		public override string ConvertObjectToXml(object value)
		{
			return XmlConvert.ToString((long)value);
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x0022B744 File Offset: 0x0022AB44
		protected override object GetEmptyStorage(int recordCount)
		{
			return new long[recordCount];
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x0022B758 File Offset: 0x0022AB58
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			long[] array = (long[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, !base.HasValue(record));
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x0022B78C File Offset: 0x0022AB8C
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (long[])store;
			base.SetNullStorage(nullbits);
		}

		// Token: 0x04000C92 RID: 3218
		private const long defaultValue = 0L;

		// Token: 0x04000C93 RID: 3219
		private long[] values;
	}
}
