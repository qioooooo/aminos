using System;
using System.Collections;
using System.Xml;

namespace System.Data.Common
{
	// Token: 0x02000149 RID: 329
	internal sealed class DoubleStorage : DataStorage
	{
		// Token: 0x06001542 RID: 5442 RVA: 0x00229DB4 File Offset: 0x002291B4
		internal DoubleStorage(DataColumn column)
			: base(column, typeof(double), 0.0)
		{
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x00229DE0 File Offset: 0x002291E0
		public override object Aggregate(int[] records, AggregateType kind)
		{
			bool flag = false;
			try
			{
				switch (kind)
				{
				case AggregateType.Sum:
				{
					double num = 0.0;
					foreach (int num2 in records)
					{
						if (!this.IsNull(num2))
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
					double num3 = 0.0;
					int num4 = 0;
					foreach (int num5 in records)
					{
						if (!this.IsNull(num5))
						{
							num3 += this.values[num5];
							num4++;
							flag = true;
						}
					}
					if (flag)
					{
						double num6 = num3 / (double)num4;
						return num6;
					}
					return this.NullValue;
				}
				case AggregateType.Min:
				{
					double num7 = double.MaxValue;
					foreach (int num8 in records)
					{
						if (!this.IsNull(num8))
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
					double num9 = double.MinValue;
					foreach (int num10 in records)
					{
						if (!this.IsNull(num10))
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
						if (!this.IsNull(num14))
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
				throw ExprException.Overflow(typeof(double));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x0022A100 File Offset: 0x00229500
		public override int Compare(int recordNo1, int recordNo2)
		{
			double num = this.values[recordNo1];
			double num2 = this.values[recordNo2];
			if (num == 0.0 || num2 == 0.0)
			{
				int num3 = base.CompareBits(recordNo1, recordNo2);
				if (num3 != 0)
				{
					return num3;
				}
			}
			return num.CompareTo(num2);
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x0022A150 File Offset: 0x00229550
		public override int CompareValueTo(int recordNo, object value)
		{
			if (this.NullValue == value)
			{
				if (this.IsNull(recordNo))
				{
					return 0;
				}
				return 1;
			}
			else
			{
				double num = this.values[recordNo];
				if (0.0 == num && this.IsNull(recordNo))
				{
					return -1;
				}
				return num.CompareTo((double)value);
			}
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x0022A1A0 File Offset: 0x002295A0
		public override object ConvertValue(object value)
		{
			if (this.NullValue != value)
			{
				if (value != null)
				{
					value = ((IConvertible)value).ToDouble(base.FormatProvider);
				}
				else
				{
					value = this.NullValue;
				}
			}
			return value;
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x0022A1DC File Offset: 0x002295DC
		public override void Copy(int recordNo1, int recordNo2)
		{
			base.CopyBits(recordNo1, recordNo2);
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x0022A204 File Offset: 0x00229604
		public override object Get(int record)
		{
			double num = this.values[record];
			if (num != 0.0)
			{
				return num;
			}
			return base.GetBits(record);
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x0022A234 File Offset: 0x00229634
		public override void Set(int record, object value)
		{
			if (this.NullValue == value)
			{
				this.values[record] = 0.0;
				base.SetNullBit(record, true);
				return;
			}
			this.values[record] = ((IConvertible)value).ToDouble(base.FormatProvider);
			base.SetNullBit(record, false);
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x0022A288 File Offset: 0x00229688
		public override void SetCapacity(int capacity)
		{
			double[] array = new double[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
			base.SetCapacity(capacity);
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x0022A2D0 File Offset: 0x002296D0
		public override object ConvertXmlToObject(string s)
		{
			return XmlConvert.ToDouble(s);
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x0022A2E8 File Offset: 0x002296E8
		public override string ConvertObjectToXml(object value)
		{
			return XmlConvert.ToString((double)value);
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x0022A300 File Offset: 0x00229700
		protected override object GetEmptyStorage(int recordCount)
		{
			return new double[recordCount];
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x0022A314 File Offset: 0x00229714
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			double[] array = (double[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, this.IsNull(record));
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x0022A344 File Offset: 0x00229744
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (double[])store;
			base.SetNullStorage(nullbits);
		}

		// Token: 0x04000C79 RID: 3193
		private const double defaultValue = 0.0;

		// Token: 0x04000C7A RID: 3194
		private double[] values;
	}
}
