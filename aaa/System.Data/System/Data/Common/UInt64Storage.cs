using System;
using System.Collections;
using System.Xml;

namespace System.Data.Common
{
	// Token: 0x0200016A RID: 362
	internal sealed class UInt64Storage : DataStorage
	{
		// Token: 0x06001669 RID: 5737 RVA: 0x002308DC File Offset: 0x0022FCDC
		public UInt64Storage(DataColumn column)
			: base(column, typeof(ulong), UInt64Storage.defaultValue)
		{
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x00230904 File Offset: 0x0022FD04
		public override object Aggregate(int[] records, AggregateType kind)
		{
			bool flag = false;
			try
			{
				switch (kind)
				{
				case AggregateType.Sum:
				{
					ulong num = UInt64Storage.defaultValue;
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
					decimal num3 = UInt64Storage.defaultValue;
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
						ulong num6 = (ulong)(num3 / num4);
						return num6;
					}
					return this.NullValue;
				}
				case AggregateType.Min:
				{
					ulong num7 = ulong.MaxValue;
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
					ulong num9 = 0UL;
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
							num12 += this.values[num14];
							num13 += this.values[num14] * this.values[num14];
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
				throw ExprException.Overflow(typeof(ulong));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x00230C2C File Offset: 0x0023002C
		public override int Compare(int recordNo1, int recordNo2)
		{
			ulong num = this.values[recordNo1];
			ulong num2 = this.values[recordNo2];
			if (num.Equals(UInt64Storage.defaultValue) || num2.Equals(UInt64Storage.defaultValue))
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

		// Token: 0x0600166C RID: 5740 RVA: 0x00230C84 File Offset: 0x00230084
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
				ulong num = this.values[recordNo];
				if (UInt64Storage.defaultValue == num && !base.HasValue(recordNo))
				{
					return -1;
				}
				return num.CompareTo((ulong)value);
			}
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x00230CD0 File Offset: 0x002300D0
		public override object ConvertValue(object value)
		{
			if (this.NullValue != value)
			{
				if (value != null)
				{
					value = ((IConvertible)value).ToUInt64(base.FormatProvider);
				}
				else
				{
					value = this.NullValue;
				}
			}
			return value;
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x00230D0C File Offset: 0x0023010C
		public override void Copy(int recordNo1, int recordNo2)
		{
			base.CopyBits(recordNo1, recordNo2);
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x00230D34 File Offset: 0x00230134
		public override object Get(int record)
		{
			ulong num = this.values[record];
			if (!num.Equals(UInt64Storage.defaultValue))
			{
				return num;
			}
			return base.GetBits(record);
		}

		// Token: 0x06001670 RID: 5744 RVA: 0x00230D68 File Offset: 0x00230168
		public override void Set(int record, object value)
		{
			if (this.NullValue == value)
			{
				this.values[record] = UInt64Storage.defaultValue;
				base.SetNullBit(record, true);
				return;
			}
			this.values[record] = ((IConvertible)value).ToUInt64(base.FormatProvider);
			base.SetNullBit(record, false);
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x00230DB8 File Offset: 0x002301B8
		public override void SetCapacity(int capacity)
		{
			ulong[] array = new ulong[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
			base.SetCapacity(capacity);
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x00230E00 File Offset: 0x00230200
		public override object ConvertXmlToObject(string s)
		{
			return XmlConvert.ToUInt64(s);
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x00230E18 File Offset: 0x00230218
		public override string ConvertObjectToXml(object value)
		{
			return XmlConvert.ToString((ulong)value);
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x00230E30 File Offset: 0x00230230
		protected override object GetEmptyStorage(int recordCount)
		{
			return new ulong[recordCount];
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x00230E44 File Offset: 0x00230244
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			ulong[] array = (ulong[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, !base.HasValue(record));
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x00230E78 File Offset: 0x00230278
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (ulong[])store;
			base.SetNullStorage(nullbits);
		}

		// Token: 0x04000CF3 RID: 3315
		private static readonly ulong defaultValue;

		// Token: 0x04000CF4 RID: 3316
		private ulong[] values;
	}
}
