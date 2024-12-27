using System;
using System.Collections;
using System.Xml;

namespace System.Data.Common
{
	// Token: 0x02000124 RID: 292
	internal sealed class DateTimeOffsetStorage : DataStorage
	{
		// Token: 0x060012D3 RID: 4819 RVA: 0x00220190 File Offset: 0x0021F590
		internal DateTimeOffsetStorage(DataColumn column)
			: base(column, typeof(DateTimeOffset), DateTimeOffsetStorage.defaultValue)
		{
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x002201B8 File Offset: 0x0021F5B8
		public override object Aggregate(int[] records, AggregateType kind)
		{
			bool flag = false;
			try
			{
				switch (kind)
				{
				case AggregateType.Min:
				{
					DateTimeOffset dateTimeOffset = DateTimeOffset.MaxValue;
					foreach (int num in records)
					{
						if (base.HasValue(num))
						{
							dateTimeOffset = ((DateTimeOffset.Compare(this.values[num], dateTimeOffset) < 0) ? this.values[num] : dateTimeOffset);
							flag = true;
						}
					}
					if (flag)
					{
						return dateTimeOffset;
					}
					return this.NullValue;
				}
				case AggregateType.Max:
				{
					DateTimeOffset dateTimeOffset2 = DateTimeOffset.MinValue;
					foreach (int num2 in records)
					{
						if (base.HasValue(num2))
						{
							dateTimeOffset2 = ((DateTimeOffset.Compare(this.values[num2], dateTimeOffset2) >= 0) ? this.values[num2] : dateTimeOffset2);
							flag = true;
						}
					}
					if (flag)
					{
						return dateTimeOffset2;
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
					int num3 = 0;
					for (int k = 0; k < records.Length; k++)
					{
						if (base.HasValue(records[k]))
						{
							num3++;
						}
					}
					return num3;
				}
				}
			}
			catch (OverflowException)
			{
				throw ExprException.Overflow(typeof(DateTimeOffset));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x00220368 File Offset: 0x0021F768
		public override int Compare(int recordNo1, int recordNo2)
		{
			DateTimeOffset dateTimeOffset = this.values[recordNo1];
			DateTimeOffset dateTimeOffset2 = this.values[recordNo2];
			if (dateTimeOffset == DateTimeOffsetStorage.defaultValue || dateTimeOffset2 == DateTimeOffsetStorage.defaultValue)
			{
				int num = base.CompareBits(recordNo1, recordNo2);
				if (num != 0)
				{
					return num;
				}
			}
			return DateTimeOffset.Compare(dateTimeOffset, dateTimeOffset2);
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x002203C8 File Offset: 0x0021F7C8
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
				DateTimeOffset dateTimeOffset = this.values[recordNo];
				if (DateTimeOffsetStorage.defaultValue == dateTimeOffset && !base.HasValue(recordNo))
				{
					return -1;
				}
				return DateTimeOffset.Compare(dateTimeOffset, (DateTimeOffset)value);
			}
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x00220424 File Offset: 0x0021F824
		public override object ConvertValue(object value)
		{
			if (this.NullValue != value)
			{
				if (value != null)
				{
					value = (DateTimeOffset)value;
				}
				else
				{
					value = this.NullValue;
				}
			}
			return value;
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x00220458 File Offset: 0x0021F858
		public override void Copy(int recordNo1, int recordNo2)
		{
			base.CopyBits(recordNo1, recordNo2);
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x00220490 File Offset: 0x0021F890
		public override object Get(int record)
		{
			DateTimeOffset dateTimeOffset = this.values[record];
			if (dateTimeOffset != DateTimeOffsetStorage.defaultValue || base.HasValue(record))
			{
				return dateTimeOffset;
			}
			return this.NullValue;
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x002204D4 File Offset: 0x0021F8D4
		public override void Set(int record, object value)
		{
			if (this.NullValue == value)
			{
				this.values[record] = DateTimeOffsetStorage.defaultValue;
				base.SetNullBit(record, true);
				return;
			}
			this.values[record] = (DateTimeOffset)value;
			base.SetNullBit(record, false);
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x00220528 File Offset: 0x0021F928
		public override void SetCapacity(int capacity)
		{
			DateTimeOffset[] array = new DateTimeOffset[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
			base.SetCapacity(capacity);
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x00220570 File Offset: 0x0021F970
		public override object ConvertXmlToObject(string s)
		{
			return XmlConvert.ToDateTimeOffset(s);
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x00220588 File Offset: 0x0021F988
		public override string ConvertObjectToXml(object value)
		{
			return XmlConvert.ToString((DateTimeOffset)value);
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x002205A0 File Offset: 0x0021F9A0
		protected override object GetEmptyStorage(int recordCount)
		{
			return new DateTimeOffset[recordCount];
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x002205B4 File Offset: 0x0021F9B4
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			DateTimeOffset[] array = (DateTimeOffset[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, !base.HasValue(record));
		}

		// Token: 0x060012E0 RID: 4832 RVA: 0x002205F8 File Offset: 0x0021F9F8
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (DateTimeOffset[])store;
			base.SetNullStorage(nullbits);
		}

		// Token: 0x04000BB4 RID: 2996
		private static readonly DateTimeOffset defaultValue = DateTimeOffset.MinValue;

		// Token: 0x04000BB5 RID: 2997
		private DateTimeOffset[] values;
	}
}
