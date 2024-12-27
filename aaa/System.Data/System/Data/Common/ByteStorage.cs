using System;
using System.Collections;
using System.Xml;

namespace System.Data.Common
{
	// Token: 0x02000115 RID: 277
	internal sealed class ByteStorage : DataStorage
	{
		// Token: 0x0600119C RID: 4508 RVA: 0x0021C52C File Offset: 0x0021B92C
		internal ByteStorage(DataColumn column)
			: base(column, typeof(byte), 0)
		{
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x0021C550 File Offset: 0x0021B950
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
						ulong num = 0UL;
						foreach (int num2 in records)
						{
							if (!this.IsNull(num2))
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
					case AggregateType.Mean:
					{
						long num3 = 0L;
						int num4 = 0;
						foreach (int num5 in records)
						{
							if (!this.IsNull(num5))
							{
								num3 += (long)(unchecked((ulong)this.values[num5]));
								unchecked
								{
									num4++;
									flag = true;
								}
							}
						}
						if (flag)
						{
							byte b = (byte)(num3 / unchecked((long)num4));
							return b;
						}
						return this.NullValue;
					}
					case AggregateType.Min:
					{
						byte b2 = byte.MaxValue;
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
						byte b3 = 0;
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
					throw ExprException.Overflow(typeof(byte));
				}
				throw ExceptionBuilder.AggregateException(kind, this.DataType);
			}
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x0021C858 File Offset: 0x0021BC58
		public override int Compare(int recordNo1, int recordNo2)
		{
			byte b = this.values[recordNo1];
			byte b2 = this.values[recordNo2];
			if (b == 0 || b2 == 0)
			{
				int num = base.CompareBits(recordNo1, recordNo2);
				if (num != 0)
				{
					return num;
				}
			}
			return b.CompareTo(b2);
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x0021C894 File Offset: 0x0021BC94
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
				byte b = this.values[recordNo];
				if (b == 0 && this.IsNull(recordNo))
				{
					return -1;
				}
				return b.CompareTo((byte)value);
			}
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x0021C8DC File Offset: 0x0021BCDC
		public override object ConvertValue(object value)
		{
			if (this.NullValue != value)
			{
				if (value != null)
				{
					value = ((IConvertible)value).ToByte(base.FormatProvider);
				}
				else
				{
					value = this.NullValue;
				}
			}
			return value;
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x0021C918 File Offset: 0x0021BD18
		public override void Copy(int recordNo1, int recordNo2)
		{
			base.CopyBits(recordNo1, recordNo2);
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x0021C940 File Offset: 0x0021BD40
		public override object Get(int record)
		{
			byte b = this.values[record];
			if (b != 0)
			{
				return b;
			}
			return base.GetBits(record);
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x0021C968 File Offset: 0x0021BD68
		public override void Set(int record, object value)
		{
			if (this.NullValue == value)
			{
				this.values[record] = 0;
				base.SetNullBit(record, true);
				return;
			}
			this.values[record] = ((IConvertible)value).ToByte(base.FormatProvider);
			base.SetNullBit(record, false);
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x0021C9B4 File Offset: 0x0021BDB4
		public override void SetCapacity(int capacity)
		{
			byte[] array = new byte[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
			base.SetCapacity(capacity);
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x0021C9FC File Offset: 0x0021BDFC
		public override object ConvertXmlToObject(string s)
		{
			return XmlConvert.ToByte(s);
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x0021CA14 File Offset: 0x0021BE14
		public override string ConvertObjectToXml(object value)
		{
			return XmlConvert.ToString((byte)value);
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x0021CA2C File Offset: 0x0021BE2C
		protected override object GetEmptyStorage(int recordCount)
		{
			return new byte[recordCount];
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x0021CA40 File Offset: 0x0021BE40
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			byte[] array = (byte[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, this.IsNull(record));
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x0021CA70 File Offset: 0x0021BE70
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (byte[])store;
			base.SetNullStorage(nullbits);
		}

		// Token: 0x04000B68 RID: 2920
		private const byte defaultValue = 0;

		// Token: 0x04000B69 RID: 2921
		private byte[] values;
	}
}
