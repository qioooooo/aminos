using System;
using System.Collections;
using System.Xml;

namespace System.Data.Common
{
	// Token: 0x02000169 RID: 361
	internal sealed class UInt32Storage : DataStorage
	{
		// Token: 0x0600165B RID: 5723 RVA: 0x00230320 File Offset: 0x0022F720
		public UInt32Storage(DataColumn column)
			: base(column, typeof(uint), UInt32Storage.defaultValue)
		{
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x00230348 File Offset: 0x0022F748
		public override object Aggregate(int[] records, AggregateType kind)
		{
			bool flag = false;
			try
			{
				switch (kind)
				{
				case AggregateType.Sum:
				{
					ulong num = (ulong)UInt32Storage.defaultValue;
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
					long num3 = (long)((ulong)UInt32Storage.defaultValue);
					int num4 = 0;
					foreach (int num5 in records)
					{
						if (base.HasValue(num5))
						{
							checked
							{
								num3 += (long)this.values[num5];
							}
							num4++;
							flag = true;
						}
					}
					checked
					{
						if (flag)
						{
							uint num6 = (uint)(num3 / unchecked((long)num4));
							return num6;
						}
						return this.NullValue;
					}
				}
				case AggregateType.Min:
				{
					uint num7 = uint.MaxValue;
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
					uint num9 = 0U;
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
				throw ExprException.Overflow(typeof(uint));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x0023067C File Offset: 0x0022FA7C
		public override int Compare(int recordNo1, int recordNo2)
		{
			uint num = this.values[recordNo1];
			uint num2 = this.values[recordNo2];
			if (num == UInt32Storage.defaultValue || num2 == UInt32Storage.defaultValue)
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

		// Token: 0x0600165E RID: 5726 RVA: 0x002306C8 File Offset: 0x0022FAC8
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
				uint num = this.values[recordNo];
				if (UInt32Storage.defaultValue == num && !base.HasValue(recordNo))
				{
					return -1;
				}
				return num.CompareTo((uint)value);
			}
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x00230714 File Offset: 0x0022FB14
		public override object ConvertValue(object value)
		{
			if (this.NullValue != value)
			{
				if (value != null)
				{
					value = ((IConvertible)value).ToUInt32(base.FormatProvider);
				}
				else
				{
					value = this.NullValue;
				}
			}
			return value;
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x00230750 File Offset: 0x0022FB50
		public override void Copy(int recordNo1, int recordNo2)
		{
			base.CopyBits(recordNo1, recordNo2);
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x00230778 File Offset: 0x0022FB78
		public override object Get(int record)
		{
			uint num = this.values[record];
			if (!num.Equals(UInt32Storage.defaultValue))
			{
				return num;
			}
			return base.GetBits(record);
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x002307AC File Offset: 0x0022FBAC
		public override void Set(int record, object value)
		{
			if (this.NullValue == value)
			{
				this.values[record] = UInt32Storage.defaultValue;
				base.SetNullBit(record, true);
				return;
			}
			this.values[record] = ((IConvertible)value).ToUInt32(base.FormatProvider);
			base.SetNullBit(record, false);
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x002307FC File Offset: 0x0022FBFC
		public override void SetCapacity(int capacity)
		{
			uint[] array = new uint[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
			base.SetCapacity(capacity);
		}

		// Token: 0x06001664 RID: 5732 RVA: 0x00230844 File Offset: 0x0022FC44
		public override object ConvertXmlToObject(string s)
		{
			return XmlConvert.ToUInt32(s);
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x0023085C File Offset: 0x0022FC5C
		public override string ConvertObjectToXml(object value)
		{
			return XmlConvert.ToString((uint)value);
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x00230874 File Offset: 0x0022FC74
		protected override object GetEmptyStorage(int recordCount)
		{
			return new uint[recordCount];
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x00230888 File Offset: 0x0022FC88
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			uint[] array = (uint[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, !base.HasValue(record));
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x002308BC File Offset: 0x0022FCBC
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (uint[])store;
			base.SetNullStorage(nullbits);
		}

		// Token: 0x04000CF1 RID: 3313
		private static readonly uint defaultValue;

		// Token: 0x04000CF2 RID: 3314
		private uint[] values;
	}
}
