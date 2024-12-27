using System;
using System.Collections;
using System.Xml;

namespace System.Data.Common
{
	// Token: 0x02000123 RID: 291
	internal sealed class DateTimeStorage : DataStorage
	{
		// Token: 0x060012C4 RID: 4804 RVA: 0x0021FB4C File Offset: 0x0021EF4C
		internal DateTimeStorage(DataColumn column)
			: base(column, typeof(DateTime), DateTimeStorage.defaultValue)
		{
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x0021FB74 File Offset: 0x0021EF74
		public override object Aggregate(int[] records, AggregateType kind)
		{
			bool flag = false;
			try
			{
				switch (kind)
				{
				case AggregateType.Min:
				{
					DateTime dateTime = DateTime.MaxValue;
					foreach (int num in records)
					{
						if (base.HasValue(num))
						{
							dateTime = ((DateTime.Compare(this.values[num], dateTime) < 0) ? this.values[num] : dateTime);
							flag = true;
						}
					}
					if (flag)
					{
						return dateTime;
					}
					return this.NullValue;
				}
				case AggregateType.Max:
				{
					DateTime dateTime2 = DateTime.MinValue;
					foreach (int num2 in records)
					{
						if (base.HasValue(num2))
						{
							dateTime2 = ((DateTime.Compare(this.values[num2], dateTime2) >= 0) ? this.values[num2] : dateTime2);
							flag = true;
						}
					}
					if (flag)
					{
						return dateTime2;
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
				throw ExprException.Overflow(typeof(DateTime));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x0021FD24 File Offset: 0x0021F124
		public override int Compare(int recordNo1, int recordNo2)
		{
			DateTime dateTime = this.values[recordNo1];
			DateTime dateTime2 = this.values[recordNo2];
			if (dateTime == DateTimeStorage.defaultValue || dateTime2 == DateTimeStorage.defaultValue)
			{
				int num = base.CompareBits(recordNo1, recordNo2);
				if (num != 0)
				{
					return num;
				}
			}
			return DateTime.Compare(dateTime, dateTime2);
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x0021FD84 File Offset: 0x0021F184
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
				DateTime dateTime = this.values[recordNo];
				if (DateTimeStorage.defaultValue == dateTime && !base.HasValue(recordNo))
				{
					return -1;
				}
				return DateTime.Compare(dateTime, (DateTime)value);
			}
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x0021FDE0 File Offset: 0x0021F1E0
		public override object ConvertValue(object value)
		{
			if (this.NullValue != value)
			{
				if (value != null)
				{
					value = ((IConvertible)value).ToDateTime(base.FormatProvider);
				}
				else
				{
					value = this.NullValue;
				}
			}
			return value;
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x0021FE1C File Offset: 0x0021F21C
		public override void Copy(int recordNo1, int recordNo2)
		{
			base.CopyBits(recordNo1, recordNo2);
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x0021FE54 File Offset: 0x0021F254
		public override object Get(int record)
		{
			DateTime dateTime = this.values[record];
			if (dateTime != DateTimeStorage.defaultValue || base.HasValue(record))
			{
				return dateTime;
			}
			return this.NullValue;
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x0021FE98 File Offset: 0x0021F298
		public override void Set(int record, object value)
		{
			if (this.NullValue == value)
			{
				this.values[record] = DateTimeStorage.defaultValue;
				base.SetNullBit(record, true);
				return;
			}
			DateTime dateTime = ((IConvertible)value).ToDateTime(base.FormatProvider);
			DateTime dateTime2;
			switch (base.DateTimeMode)
			{
			case DataSetDateTime.Local:
				if (dateTime.Kind == DateTimeKind.Local)
				{
					dateTime2 = dateTime;
				}
				else if (dateTime.Kind == DateTimeKind.Utc)
				{
					dateTime2 = dateTime.ToLocalTime();
				}
				else
				{
					dateTime2 = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
				}
				break;
			case DataSetDateTime.Unspecified:
			case DataSetDateTime.UnspecifiedLocal:
				dateTime2 = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
				break;
			case DataSetDateTime.Utc:
				if (dateTime.Kind == DateTimeKind.Utc)
				{
					dateTime2 = dateTime;
				}
				else if (dateTime.Kind == DateTimeKind.Local)
				{
					dateTime2 = dateTime.ToUniversalTime();
				}
				else
				{
					dateTime2 = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
				}
				break;
			default:
				throw ExceptionBuilder.InvalidDateTimeMode(base.DateTimeMode);
			}
			this.values[record] = dateTime2;
			base.SetNullBit(record, false);
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x0021FF88 File Offset: 0x0021F388
		public override void SetCapacity(int capacity)
		{
			DateTime[] array = new DateTime[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
			base.SetCapacity(capacity);
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x0021FFD0 File Offset: 0x0021F3D0
		public override object ConvertXmlToObject(string s)
		{
			object obj;
			if (base.DateTimeMode == DataSetDateTime.UnspecifiedLocal)
			{
				obj = XmlConvert.ToDateTime(s, XmlDateTimeSerializationMode.Unspecified);
			}
			else
			{
				obj = XmlConvert.ToDateTime(s, XmlDateTimeSerializationMode.RoundtripKind);
			}
			return obj;
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x00220004 File Offset: 0x0021F404
		public override string ConvertObjectToXml(object value)
		{
			string text;
			if (base.DateTimeMode == DataSetDateTime.UnspecifiedLocal)
			{
				text = XmlConvert.ToString((DateTime)value, XmlDateTimeSerializationMode.Local);
			}
			else
			{
				text = XmlConvert.ToString((DateTime)value, XmlDateTimeSerializationMode.RoundtripKind);
			}
			return text;
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x00220038 File Offset: 0x0021F438
		protected override object GetEmptyStorage(int recordCount)
		{
			return new DateTime[recordCount];
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x0022004C File Offset: 0x0021F44C
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			DateTime[] array = (DateTime[])store;
			bool flag = !base.HasValue(record);
			if (flag || (base.DateTimeMode & DataSetDateTime.Local) == (DataSetDateTime)0)
			{
				array[storeIndex] = this.values[record];
			}
			else
			{
				array[storeIndex] = this.values[record].ToUniversalTime();
			}
			nullbits.Set(storeIndex, flag);
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x002200C0 File Offset: 0x0021F4C0
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (DateTime[])store;
			base.SetNullStorage(nullbits);
			if (base.DateTimeMode == DataSetDateTime.UnspecifiedLocal)
			{
				for (int i = 0; i < this.values.Length; i++)
				{
					if (base.HasValue(i))
					{
						this.values[i] = DateTime.SpecifyKind(this.values[i].ToLocalTime(), DateTimeKind.Unspecified);
					}
				}
				return;
			}
			if (base.DateTimeMode == DataSetDateTime.Local)
			{
				for (int j = 0; j < this.values.Length; j++)
				{
					if (base.HasValue(j))
					{
						this.values[j] = this.values[j].ToLocalTime();
					}
				}
			}
		}

		// Token: 0x04000BB2 RID: 2994
		private static readonly DateTime defaultValue = DateTime.MinValue;

		// Token: 0x04000BB3 RID: 2995
		private DateTime[] values;
	}
}
