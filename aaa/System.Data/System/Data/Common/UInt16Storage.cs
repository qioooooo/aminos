using System;
using System.Collections;
using System.Xml;

namespace System.Data.Common
{
	// Token: 0x02000168 RID: 360
	internal sealed class UInt16Storage : DataStorage
	{
		// Token: 0x0600164D RID: 5709 RVA: 0x0022FD70 File Offset: 0x0022F170
		public UInt16Storage(DataColumn column)
			: base(column, typeof(ushort), UInt16Storage.defaultValue)
		{
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x0022FD98 File Offset: 0x0022F198
		public override object Aggregate(int[] records, AggregateType kind)
		{
			bool flag = false;
			try
			{
				switch (kind)
				{
				case AggregateType.Sum:
				{
					ulong num = (ulong)UInt16Storage.defaultValue;
					checked
					{
						foreach (int num2 in records)
						{
							if (base.HasValue(num2))
							{
								num += unchecked((ulong)this.values[num2]);
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
					long num3 = (long)((ulong)UInt16Storage.defaultValue);
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
							ushort num6 = (ushort)(num3 / unchecked((long)num4));
							return num6;
						}
						return this.NullValue;
					}
				}
				case AggregateType.Min:
				{
					ushort num7 = ushort.MaxValue;
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
					ushort num9 = 0;
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
				throw ExprException.Overflow(typeof(ushort));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x002300CC File Offset: 0x0022F4CC
		public override int Compare(int recordNo1, int recordNo2)
		{
			ushort num = this.values[recordNo1];
			ushort num2 = this.values[recordNo2];
			if (num == UInt16Storage.defaultValue || num2 == UInt16Storage.defaultValue)
			{
				int num3 = base.CompareBits(recordNo1, recordNo2);
				if (num3 != 0)
				{
					return num3;
				}
			}
			return (int)(num - num2);
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x0023010C File Offset: 0x0022F50C
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
				ushort num = this.values[recordNo];
				if (UInt16Storage.defaultValue == num && !base.HasValue(recordNo))
				{
					return -1;
				}
				return num.CompareTo((ushort)value);
			}
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x00230158 File Offset: 0x0022F558
		public override object ConvertValue(object value)
		{
			if (this.NullValue != value)
			{
				if (value != null)
				{
					value = ((IConvertible)value).ToUInt16(base.FormatProvider);
				}
				else
				{
					value = this.NullValue;
				}
			}
			return value;
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x00230194 File Offset: 0x0022F594
		public override void Copy(int recordNo1, int recordNo2)
		{
			base.CopyBits(recordNo1, recordNo2);
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x002301BC File Offset: 0x0022F5BC
		public override object Get(int record)
		{
			ushort num = this.values[record];
			if (!num.Equals(UInt16Storage.defaultValue))
			{
				return num;
			}
			return base.GetBits(record);
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x002301F0 File Offset: 0x0022F5F0
		public override void Set(int record, object value)
		{
			if (this.NullValue == value)
			{
				this.values[record] = UInt16Storage.defaultValue;
				base.SetNullBit(record, true);
				return;
			}
			this.values[record] = ((IConvertible)value).ToUInt16(base.FormatProvider);
			base.SetNullBit(record, false);
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x00230240 File Offset: 0x0022F640
		public override void SetCapacity(int capacity)
		{
			ushort[] array = new ushort[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
			base.SetCapacity(capacity);
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x00230288 File Offset: 0x0022F688
		public override object ConvertXmlToObject(string s)
		{
			return XmlConvert.ToUInt16(s);
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x002302A0 File Offset: 0x0022F6A0
		public override string ConvertObjectToXml(object value)
		{
			return XmlConvert.ToString((ushort)value);
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x002302B8 File Offset: 0x0022F6B8
		protected override object GetEmptyStorage(int recordCount)
		{
			return new ushort[recordCount];
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x002302CC File Offset: 0x0022F6CC
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			ushort[] array = (ushort[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, !base.HasValue(record));
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x00230300 File Offset: 0x0022F700
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (ushort[])store;
			base.SetNullStorage(nullbits);
		}

		// Token: 0x04000CEF RID: 3311
		private static readonly ushort defaultValue;

		// Token: 0x04000CF0 RID: 3312
		private ushort[] values;
	}
}
