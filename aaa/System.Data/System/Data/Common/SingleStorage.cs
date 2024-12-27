using System;
using System.Collections;
using System.Xml;

namespace System.Data.Common
{
	// Token: 0x02000163 RID: 355
	internal sealed class SingleStorage : DataStorage
	{
		// Token: 0x0600160C RID: 5644 RVA: 0x0022D8E0 File Offset: 0x0022CCE0
		public SingleStorage(DataColumn column)
			: base(column, typeof(float), 0f)
		{
		}

		// Token: 0x0600160D RID: 5645 RVA: 0x0022D908 File Offset: 0x0022CD08
		public override object Aggregate(int[] records, AggregateType kind)
		{
			bool flag = false;
			try
			{
				switch (kind)
				{
				case AggregateType.Sum:
				{
					float num = 0f;
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
							num3 += (double)this.values[num5];
							num4++;
							flag = true;
						}
					}
					if (flag)
					{
						float num6 = (float)(num3 / (double)num4);
						return num6;
					}
					return this.NullValue;
				}
				case AggregateType.Min:
				{
					float num7 = float.MaxValue;
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
					float num9 = float.MinValue;
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
				throw ExprException.Overflow(typeof(float));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x0022DC1C File Offset: 0x0022D01C
		public override int Compare(int recordNo1, int recordNo2)
		{
			float num = this.values[recordNo1];
			float num2 = this.values[recordNo2];
			if (num == 0f || num2 == 0f)
			{
				int num3 = base.CompareBits(recordNo1, recordNo2);
				if (num3 != 0)
				{
					return num3;
				}
			}
			return num.CompareTo(num2);
		}

		// Token: 0x0600160F RID: 5647 RVA: 0x0022DC64 File Offset: 0x0022D064
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
				float num = this.values[recordNo];
				if (0f == num && this.IsNull(recordNo))
				{
					return -1;
				}
				return num.CompareTo((float)value);
			}
		}

		// Token: 0x06001610 RID: 5648 RVA: 0x0022DCB0 File Offset: 0x0022D0B0
		public override object ConvertValue(object value)
		{
			if (this.NullValue != value)
			{
				if (value != null)
				{
					value = ((IConvertible)value).ToSingle(base.FormatProvider);
				}
				else
				{
					value = this.NullValue;
				}
			}
			return value;
		}

		// Token: 0x06001611 RID: 5649 RVA: 0x0022DCEC File Offset: 0x0022D0EC
		public override void Copy(int recordNo1, int recordNo2)
		{
			base.CopyBits(recordNo1, recordNo2);
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x06001612 RID: 5650 RVA: 0x0022DD14 File Offset: 0x0022D114
		public override object Get(int record)
		{
			float num = this.values[record];
			if (num != 0f)
			{
				return num;
			}
			return base.GetBits(record);
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x0022DD40 File Offset: 0x0022D140
		public override void Set(int record, object value)
		{
			if (this.NullValue == value)
			{
				this.values[record] = 0f;
				base.SetNullBit(record, true);
				return;
			}
			this.values[record] = ((IConvertible)value).ToSingle(base.FormatProvider);
			base.SetNullBit(record, false);
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x0022DD90 File Offset: 0x0022D190
		public override void SetCapacity(int capacity)
		{
			float[] array = new float[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
			base.SetCapacity(capacity);
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x0022DDD8 File Offset: 0x0022D1D8
		public override object ConvertXmlToObject(string s)
		{
			return XmlConvert.ToSingle(s);
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x0022DDF0 File Offset: 0x0022D1F0
		public override string ConvertObjectToXml(object value)
		{
			return XmlConvert.ToString((float)value);
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x0022DE08 File Offset: 0x0022D208
		protected override object GetEmptyStorage(int recordCount)
		{
			return new float[recordCount];
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x0022DE1C File Offset: 0x0022D21C
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			float[] array = (float[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, this.IsNull(record));
		}

		// Token: 0x06001619 RID: 5657 RVA: 0x0022DE4C File Offset: 0x0022D24C
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (float[])store;
			base.SetNullStorage(nullbits);
		}

		// Token: 0x04000CE4 RID: 3300
		private const float defaultValue = 0f;

		// Token: 0x04000CE5 RID: 3301
		private float[] values;
	}
}
