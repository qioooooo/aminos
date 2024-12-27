using System;
using System.Collections;
using System.Xml;

namespace System.Data.Common
{
	// Token: 0x02000114 RID: 276
	internal sealed class BooleanStorage : DataStorage
	{
		// Token: 0x0600118E RID: 4494 RVA: 0x0021C1A0 File Offset: 0x0021B5A0
		internal BooleanStorage(DataColumn column)
			: base(column, typeof(bool), false)
		{
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x0021C1C4 File Offset: 0x0021B5C4
		public override object Aggregate(int[] records, AggregateType kind)
		{
			bool flag = false;
			try
			{
				switch (kind)
				{
				case AggregateType.Min:
				{
					bool flag2 = true;
					foreach (int num in records)
					{
						if (!this.IsNull(num))
						{
							flag2 = this.values[num] && flag2;
							flag = true;
						}
					}
					if (flag)
					{
						return flag2;
					}
					return this.NullValue;
				}
				case AggregateType.Max:
				{
					bool flag3 = false;
					foreach (int num2 in records)
					{
						if (!this.IsNull(num2))
						{
							flag3 = this.values[num2] || flag3;
							flag = true;
						}
					}
					if (flag)
					{
						return flag3;
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
				throw ExprException.Overflow(typeof(bool));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x0021C2F4 File Offset: 0x0021B6F4
		public override int Compare(int recordNo1, int recordNo2)
		{
			bool flag = this.values[recordNo1];
			bool flag2 = this.values[recordNo2];
			if (!flag || !flag2)
			{
				int num = base.CompareBits(recordNo1, recordNo2);
				if (num != 0)
				{
					return num;
				}
			}
			return flag.CompareTo(flag2);
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x0021C330 File Offset: 0x0021B730
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
				bool flag = this.values[recordNo];
				if (!flag && this.IsNull(recordNo))
				{
					return -1;
				}
				return flag.CompareTo((bool)value);
			}
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x0021C378 File Offset: 0x0021B778
		public override object ConvertValue(object value)
		{
			if (this.NullValue != value)
			{
				if (value != null)
				{
					value = ((IConvertible)value).ToBoolean(base.FormatProvider);
				}
				else
				{
					value = this.NullValue;
				}
			}
			return value;
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x0021C3B4 File Offset: 0x0021B7B4
		public override void Copy(int recordNo1, int recordNo2)
		{
			base.CopyBits(recordNo1, recordNo2);
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x0021C3DC File Offset: 0x0021B7DC
		public override object Get(int record)
		{
			bool flag = this.values[record];
			if (flag)
			{
				return flag;
			}
			return base.GetBits(record);
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x0021C404 File Offset: 0x0021B804
		public override void Set(int record, object value)
		{
			if (this.NullValue == value)
			{
				this.values[record] = false;
				base.SetNullBit(record, true);
				return;
			}
			this.values[record] = ((IConvertible)value).ToBoolean(base.FormatProvider);
			base.SetNullBit(record, false);
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x0021C450 File Offset: 0x0021B850
		public override void SetCapacity(int capacity)
		{
			bool[] array = new bool[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
			base.SetCapacity(capacity);
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x0021C498 File Offset: 0x0021B898
		public override object ConvertXmlToObject(string s)
		{
			return XmlConvert.ToBoolean(s);
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x0021C4B0 File Offset: 0x0021B8B0
		public override string ConvertObjectToXml(object value)
		{
			return XmlConvert.ToString((bool)value);
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x0021C4C8 File Offset: 0x0021B8C8
		protected override object GetEmptyStorage(int recordCount)
		{
			return new bool[recordCount];
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x0021C4DC File Offset: 0x0021B8DC
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			bool[] array = (bool[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, this.IsNull(record));
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x0021C50C File Offset: 0x0021B90C
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (bool[])store;
			base.SetNullStorage(nullbits);
		}

		// Token: 0x04000B66 RID: 2918
		private const bool defaultValue = false;

		// Token: 0x04000B67 RID: 2919
		private bool[] values;
	}
}
