using System;
using System.Collections;
using System.Xml;

namespace System.Data.Common
{
	// Token: 0x02000167 RID: 359
	internal sealed class TimeSpanStorage : DataStorage
	{
		// Token: 0x0600163D RID: 5693 RVA: 0x0022F898 File Offset: 0x0022EC98
		public TimeSpanStorage(DataColumn column)
			: base(column, typeof(TimeSpan), TimeSpanStorage.defaultValue)
		{
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x0022F8C0 File Offset: 0x0022ECC0
		public override object Aggregate(int[] records, AggregateType kind)
		{
			bool flag = false;
			try
			{
				switch (kind)
				{
				case AggregateType.Min:
				{
					TimeSpan timeSpan = TimeSpan.MaxValue;
					foreach (int num in records)
					{
						if (!this.IsNull(num))
						{
							timeSpan = ((TimeSpan.Compare(this.values[num], timeSpan) < 0) ? this.values[num] : timeSpan);
							flag = true;
						}
					}
					if (flag)
					{
						return timeSpan;
					}
					return this.NullValue;
				}
				case AggregateType.Max:
				{
					TimeSpan timeSpan2 = TimeSpan.MinValue;
					foreach (int num2 in records)
					{
						if (!this.IsNull(num2))
						{
							timeSpan2 = ((TimeSpan.Compare(this.values[num2], timeSpan2) >= 0) ? this.values[num2] : timeSpan2);
							flag = true;
						}
					}
					if (flag)
					{
						return timeSpan2;
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
				}
			}
			catch (OverflowException)
			{
				throw ExprException.Overflow(typeof(TimeSpan));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x0022FA48 File Offset: 0x0022EE48
		public override int Compare(int recordNo1, int recordNo2)
		{
			TimeSpan timeSpan = this.values[recordNo1];
			TimeSpan timeSpan2 = this.values[recordNo2];
			if (timeSpan == TimeSpanStorage.defaultValue || timeSpan2 == TimeSpanStorage.defaultValue)
			{
				int num = base.CompareBits(recordNo1, recordNo2);
				if (num != 0)
				{
					return num;
				}
			}
			return TimeSpan.Compare(timeSpan, timeSpan2);
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x0022FAA8 File Offset: 0x0022EEA8
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
				TimeSpan timeSpan = this.values[recordNo];
				if (TimeSpanStorage.defaultValue == timeSpan && this.IsNull(recordNo))
				{
					return -1;
				}
				return timeSpan.CompareTo((TimeSpan)value);
			}
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x0022FB04 File Offset: 0x0022EF04
		private static TimeSpan ConvertToTimeSpan(object value)
		{
			Type type = value.GetType();
			if (type == typeof(string))
			{
				return TimeSpan.Parse((string)value);
			}
			if (type == typeof(int))
			{
				return new TimeSpan((long)((int)value));
			}
			if (type == typeof(long))
			{
				return new TimeSpan((long)value);
			}
			return (TimeSpan)value;
		}

		// Token: 0x06001642 RID: 5698 RVA: 0x0022FB6C File Offset: 0x0022EF6C
		public override object ConvertValue(object value)
		{
			if (this.NullValue != value)
			{
				if (value != null)
				{
					value = TimeSpanStorage.ConvertToTimeSpan(value);
				}
				else
				{
					value = this.NullValue;
				}
			}
			return value;
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x0022FBA0 File Offset: 0x0022EFA0
		public override void Copy(int recordNo1, int recordNo2)
		{
			base.CopyBits(recordNo1, recordNo2);
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x0022FBD8 File Offset: 0x0022EFD8
		public override object Get(int record)
		{
			TimeSpan timeSpan = this.values[record];
			if (timeSpan != TimeSpanStorage.defaultValue)
			{
				return timeSpan;
			}
			return base.GetBits(record);
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x0022FC14 File Offset: 0x0022F014
		public override void Set(int record, object value)
		{
			if (this.NullValue == value)
			{
				this.values[record] = TimeSpanStorage.defaultValue;
				base.SetNullBit(record, true);
				return;
			}
			this.values[record] = TimeSpanStorage.ConvertToTimeSpan(value);
			base.SetNullBit(record, false);
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x0022FC68 File Offset: 0x0022F068
		public override void SetCapacity(int capacity)
		{
			TimeSpan[] array = new TimeSpan[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
			base.SetCapacity(capacity);
		}

		// Token: 0x06001647 RID: 5703 RVA: 0x0022FCB0 File Offset: 0x0022F0B0
		public override object ConvertXmlToObject(string s)
		{
			return XmlConvert.ToTimeSpan(s);
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x0022FCC8 File Offset: 0x0022F0C8
		public override string ConvertObjectToXml(object value)
		{
			return XmlConvert.ToString((TimeSpan)value);
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x0022FCE0 File Offset: 0x0022F0E0
		protected override object GetEmptyStorage(int recordCount)
		{
			return new TimeSpan[recordCount];
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x0022FCF4 File Offset: 0x0022F0F4
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			TimeSpan[] array = (TimeSpan[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, this.IsNull(record));
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x0022FD38 File Offset: 0x0022F138
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (TimeSpan[])store;
			base.SetNullStorage(nullbits);
		}

		// Token: 0x04000CED RID: 3309
		private static readonly TimeSpan defaultValue = TimeSpan.Zero;

		// Token: 0x04000CEE RID: 3310
		private TimeSpan[] values;
	}
}
