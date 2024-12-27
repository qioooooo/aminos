using System;
using System.Collections;
using System.Xml;

namespace System.Data.Common
{
	// Token: 0x02000150 RID: 336
	internal sealed class Int32Storage : DataStorage
	{
		// Token: 0x0600156E RID: 5486 RVA: 0x0022AC78 File Offset: 0x0022A078
		internal Int32Storage(DataColumn column)
			: base(column, typeof(int), 0)
		{
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x0022AC9C File Offset: 0x0022A09C
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
							int num6 = (int)(num3 / unchecked((long)num4));
							return num6;
						}
						return this.NullValue;
					}
				}
				case AggregateType.Min:
				{
					int num7 = int.MaxValue;
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
					int num9 = int.MinValue;
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
				throw ExprException.Overflow(typeof(int));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x0022AFCC File Offset: 0x0022A3CC
		public override int Compare(int recordNo1, int recordNo2)
		{
			int num = this.values[recordNo1];
			int num2 = this.values[recordNo2];
			if (num == 0 || num2 == 0)
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

		// Token: 0x06001571 RID: 5489 RVA: 0x0022B00C File Offset: 0x0022A40C
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
				int num = this.values[recordNo];
				if (num == 0 && !base.HasValue(recordNo))
				{
					return -1;
				}
				return num.CompareTo((int)value);
			}
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x0022B054 File Offset: 0x0022A454
		public override object ConvertValue(object value)
		{
			if (this.NullValue != value)
			{
				if (value != null)
				{
					value = ((IConvertible)value).ToInt32(base.FormatProvider);
				}
				else
				{
					value = this.NullValue;
				}
			}
			return value;
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x0022B090 File Offset: 0x0022A490
		public override void Copy(int recordNo1, int recordNo2)
		{
			base.CopyBits(recordNo1, recordNo2);
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x0022B0B8 File Offset: 0x0022A4B8
		public override object Get(int record)
		{
			int num = this.values[record];
			if (num != 0)
			{
				return num;
			}
			return base.GetBits(record);
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x0022B0E0 File Offset: 0x0022A4E0
		public override void Set(int record, object value)
		{
			if (this.NullValue == value)
			{
				this.values[record] = 0;
				base.SetNullBit(record, true);
				return;
			}
			this.values[record] = ((IConvertible)value).ToInt32(base.FormatProvider);
			base.SetNullBit(record, false);
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x0022B12C File Offset: 0x0022A52C
		public override void SetCapacity(int capacity)
		{
			int[] array = new int[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
			base.SetCapacity(capacity);
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x0022B174 File Offset: 0x0022A574
		public override object ConvertXmlToObject(string s)
		{
			return XmlConvert.ToInt32(s);
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x0022B18C File Offset: 0x0022A58C
		public override string ConvertObjectToXml(object value)
		{
			return XmlConvert.ToString((int)value);
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x0022B1A4 File Offset: 0x0022A5A4
		protected override object GetEmptyStorage(int recordCount)
		{
			return new int[recordCount];
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x0022B1B8 File Offset: 0x0022A5B8
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			int[] array = (int[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, !base.HasValue(record));
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x0022B1EC File Offset: 0x0022A5EC
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (int[])store;
			base.SetNullStorage(nullbits);
		}

		// Token: 0x04000C90 RID: 3216
		private const int defaultValue = 0;

		// Token: 0x04000C91 RID: 3217
		private int[] values;
	}
}
