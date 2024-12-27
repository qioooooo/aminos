using System;
using System.Collections;
using System.Xml;

namespace System.Data.Common
{
	// Token: 0x02000160 RID: 352
	internal sealed class SByteStorage : DataStorage
	{
		// Token: 0x060015FC RID: 5628 RVA: 0x0022D214 File Offset: 0x0022C614
		public SByteStorage(DataColumn column)
			: base(column, typeof(sbyte), 0)
		{
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x0022D238 File Offset: 0x0022C638
		public override object Aggregate(int[] records, AggregateType kind)
		{
			bool flag = false;
			checked
			{
				try
				{
					switch (kind)
					{
					case AggregateType.Sum:
					{
						long num = 0L;
						foreach (int num2 in records)
						{
							if (!this.IsNull(num2))
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
					case AggregateType.Mean:
					{
						long num3 = 0L;
						int num4 = 0;
						foreach (int num5 in records)
						{
							if (!this.IsNull(num5))
							{
								num3 += unchecked((long)this.values[num5]);
								unchecked
								{
									num4++;
									flag = true;
								}
							}
						}
						if (flag)
						{
							sbyte b = (sbyte)(num3 / unchecked((long)num4));
							return b;
						}
						return this.NullValue;
					}
					case AggregateType.Min:
					{
						sbyte b2 = sbyte.MaxValue;
						foreach (int num6 in records)
						{
							if (!this.IsNull(num6))
							{
								b2 = Math.Min(this.values[num6], b2);
								flag = true;
							}
						}
						if (flag)
						{
							return b2;
						}
						return this.NullValue;
					}
					case AggregateType.Max:
					{
						sbyte b3 = sbyte.MinValue;
						foreach (int num7 in records)
						{
							if (!this.IsNull(num7))
							{
								b3 = Math.Max(this.values[num7], b3);
								flag = true;
							}
						}
						if (flag)
						{
							return b3;
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
						int num8 = 0;
						double num9 = 0.0;
						double num10 = 0.0;
						unchecked
						{
							foreach (int num11 in records)
							{
								if (!this.IsNull(num11))
								{
									num9 += (double)this.values[num11];
									num10 += (double)this.values[num11] * (double)this.values[num11];
									num8++;
								}
							}
							if (num8 <= 1)
							{
								return this.NullValue;
							}
							double num12 = (double)num8 * num10 - num9 * num9;
							double num13 = num12 / (num9 * num9);
							if (num13 < 1E-15 || num12 < 0.0)
							{
								num12 = 0.0;
							}
							else
							{
								num12 /= (double)(num8 * (num8 - 1));
							}
							if (kind == AggregateType.StDev)
							{
								return Math.Sqrt(num12);
							}
							return num12;
						}
					}
					}
				}
				catch (OverflowException)
				{
					throw ExprException.Overflow(typeof(sbyte));
				}
				throw ExceptionBuilder.AggregateException(kind, this.DataType);
			}
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x0022D53C File Offset: 0x0022C93C
		public override int Compare(int recordNo1, int recordNo2)
		{
			sbyte b = this.values[recordNo1];
			sbyte b2 = this.values[recordNo2];
			if (b.Equals(0) || b2.Equals(0))
			{
				int num = base.CompareBits(recordNo1, recordNo2);
				if (num != 0)
				{
					return num;
				}
			}
			return b.CompareTo(b2);
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x0022D588 File Offset: 0x0022C988
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
				sbyte b = this.values[recordNo];
				if (b == 0 && this.IsNull(recordNo))
				{
					return -1;
				}
				return b.CompareTo((sbyte)value);
			}
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x0022D5D0 File Offset: 0x0022C9D0
		public override object ConvertValue(object value)
		{
			if (this.NullValue != value)
			{
				if (value != null)
				{
					value = ((IConvertible)value).ToSByte(base.FormatProvider);
				}
				else
				{
					value = this.NullValue;
				}
			}
			return value;
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x0022D60C File Offset: 0x0022CA0C
		public override void Copy(int recordNo1, int recordNo2)
		{
			base.CopyBits(recordNo1, recordNo2);
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x06001602 RID: 5634 RVA: 0x0022D634 File Offset: 0x0022CA34
		public override object Get(int record)
		{
			sbyte b = this.values[record];
			if (!b.Equals(0))
			{
				return b;
			}
			return base.GetBits(record);
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x0022D664 File Offset: 0x0022CA64
		public override void Set(int record, object value)
		{
			if (this.NullValue == value)
			{
				this.values[record] = 0;
				base.SetNullBit(record, true);
				return;
			}
			this.values[record] = ((IConvertible)value).ToSByte(base.FormatProvider);
			base.SetNullBit(record, false);
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x0022D6B0 File Offset: 0x0022CAB0
		public override void SetCapacity(int capacity)
		{
			sbyte[] array = new sbyte[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
			base.SetCapacity(capacity);
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x0022D6F8 File Offset: 0x0022CAF8
		public override object ConvertXmlToObject(string s)
		{
			return XmlConvert.ToSByte(s);
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x0022D710 File Offset: 0x0022CB10
		public override string ConvertObjectToXml(object value)
		{
			return XmlConvert.ToString((sbyte)value);
		}

		// Token: 0x06001607 RID: 5639 RVA: 0x0022D728 File Offset: 0x0022CB28
		protected override object GetEmptyStorage(int recordCount)
		{
			return new sbyte[recordCount];
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x0022D73C File Offset: 0x0022CB3C
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			sbyte[] array = (sbyte[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, this.IsNull(record));
		}

		// Token: 0x06001609 RID: 5641 RVA: 0x0022D76C File Offset: 0x0022CB6C
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (sbyte[])store;
			base.SetNullStorage(nullbits);
		}

		// Token: 0x04000CC3 RID: 3267
		private const sbyte defaultValue = 0;

		// Token: 0x04000CC4 RID: 3268
		private sbyte[] values;
	}
}
