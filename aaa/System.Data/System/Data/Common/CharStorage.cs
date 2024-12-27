using System;
using System.Collections;
using System.Xml;

namespace System.Data.Common
{
	// Token: 0x02000116 RID: 278
	internal sealed class CharStorage : DataStorage
	{
		// Token: 0x060011AA RID: 4522 RVA: 0x0021CA90 File Offset: 0x0021BE90
		internal CharStorage(DataColumn column)
			: base(column, typeof(char), '\0')
		{
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x0021CAB4 File Offset: 0x0021BEB4
		public override object Aggregate(int[] records, AggregateType kind)
		{
			bool flag = false;
			try
			{
				switch (kind)
				{
				case AggregateType.Min:
				{
					char c = char.MaxValue;
					foreach (int num in records)
					{
						if (!this.IsNull(num))
						{
							c = ((this.values[num] < c) ? this.values[num] : c);
							flag = true;
						}
					}
					if (flag)
					{
						return c;
					}
					return this.NullValue;
				}
				case AggregateType.Max:
				{
					char c2 = '\0';
					foreach (int num2 in records)
					{
						if (!this.IsNull(num2))
						{
							c2 = ((this.values[num2] > c2) ? this.values[num2] : c2);
							flag = true;
						}
					}
					if (flag)
					{
						return c2;
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
				throw ExprException.Overflow(typeof(char));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x0021CBFC File Offset: 0x0021BFFC
		public override int Compare(int recordNo1, int recordNo2)
		{
			char c = this.values[recordNo1];
			char c2 = this.values[recordNo2];
			if (c == '\0' || c2 == '\0')
			{
				int num = base.CompareBits(recordNo1, recordNo2);
				if (num != 0)
				{
					return num;
				}
			}
			return c.CompareTo(c2);
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x0021CC38 File Offset: 0x0021C038
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
				char c = this.values[recordNo];
				if (c == '\0' && this.IsNull(recordNo))
				{
					return -1;
				}
				return c.CompareTo((char)value);
			}
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x0021CC80 File Offset: 0x0021C080
		public override object ConvertValue(object value)
		{
			if (this.NullValue != value)
			{
				if (value != null)
				{
					value = ((IConvertible)value).ToChar(base.FormatProvider);
				}
				else
				{
					value = this.NullValue;
				}
			}
			return value;
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x0021CCBC File Offset: 0x0021C0BC
		public override void Copy(int recordNo1, int recordNo2)
		{
			base.CopyBits(recordNo1, recordNo2);
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x0021CCE4 File Offset: 0x0021C0E4
		public override object Get(int record)
		{
			char c = this.values[record];
			if (c != '\0')
			{
				return c;
			}
			return base.GetBits(record);
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x0021CD0C File Offset: 0x0021C10C
		public override void Set(int record, object value)
		{
			if (this.NullValue == value)
			{
				this.values[record] = '\0';
				base.SetNullBit(record, true);
				return;
			}
			char c = ((IConvertible)value).ToChar(base.FormatProvider);
			if ((c >= '\ud800' && c <= '\udfff') || (c < '!' && (c == '\t' || c == '\n' || c == '\r')))
			{
				throw ExceptionBuilder.ProblematicChars(c);
			}
			this.values[record] = c;
			base.SetNullBit(record, false);
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x0021CD84 File Offset: 0x0021C184
		public override void SetCapacity(int capacity)
		{
			char[] array = new char[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
			base.SetCapacity(capacity);
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x0021CDCC File Offset: 0x0021C1CC
		public override object ConvertXmlToObject(string s)
		{
			return XmlConvert.ToChar(s);
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x0021CDE4 File Offset: 0x0021C1E4
		public override string ConvertObjectToXml(object value)
		{
			return XmlConvert.ToString((char)value);
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x0021CDFC File Offset: 0x0021C1FC
		protected override object GetEmptyStorage(int recordCount)
		{
			return new char[recordCount];
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x0021CE10 File Offset: 0x0021C210
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			char[] array = (char[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, this.IsNull(record));
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x0021CE40 File Offset: 0x0021C240
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (char[])store;
			base.SetNullStorage(nullbits);
		}

		// Token: 0x04000B6A RID: 2922
		private const char defaultValue = '\0';

		// Token: 0x04000B6B RID: 2923
		private char[] values;
	}
}
