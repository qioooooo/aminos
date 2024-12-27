using System;
using System.Collections;

namespace System.Data.Common
{
	// Token: 0x02000165 RID: 357
	internal sealed class StringStorage : DataStorage
	{
		// Token: 0x0600162D RID: 5677 RVA: 0x0022F538 File Offset: 0x0022E938
		public StringStorage(DataColumn column)
			: base(column, typeof(string), string.Empty)
		{
		}

		// Token: 0x0600162E RID: 5678 RVA: 0x0022F55C File Offset: 0x0022E95C
		public override object Aggregate(int[] recordNos, AggregateType kind)
		{
			switch (kind)
			{
			case AggregateType.Min:
			{
				int num = -1;
				int i;
				for (i = 0; i < recordNos.Length; i++)
				{
					if (!this.IsNull(recordNos[i]))
					{
						num = recordNos[i];
						break;
					}
				}
				if (num >= 0)
				{
					for (i++; i < recordNos.Length; i++)
					{
						if (!this.IsNull(recordNos[i]) && this.Compare(num, recordNos[i]) > 0)
						{
							num = recordNos[i];
						}
					}
					return this.Get(num);
				}
				return this.NullValue;
			}
			case AggregateType.Max:
			{
				int num2 = -1;
				int i;
				for (i = 0; i < recordNos.Length; i++)
				{
					if (!this.IsNull(recordNos[i]))
					{
						num2 = recordNos[i];
						break;
					}
				}
				if (num2 >= 0)
				{
					for (i++; i < recordNos.Length; i++)
					{
						if (this.Compare(num2, recordNos[i]) < 0)
						{
							num2 = recordNos[i];
						}
					}
					return this.Get(num2);
				}
				return this.NullValue;
			}
			case AggregateType.Count:
			{
				int num3 = 0;
				for (int i = 0; i < recordNos.Length; i++)
				{
					object obj = this.values[recordNos[i]];
					if (obj != null)
					{
						num3++;
					}
				}
				return num3;
			}
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x0022F678 File Offset: 0x0022EA78
		public override int Compare(int recordNo1, int recordNo2)
		{
			string text = this.values[recordNo1];
			string text2 = this.values[recordNo2];
			if (text == text2)
			{
				return 0;
			}
			if (text == null)
			{
				return -1;
			}
			if (text2 == null)
			{
				return 1;
			}
			return this.Table.Compare(text, text2);
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x0022F6B4 File Offset: 0x0022EAB4
		public override int CompareValueTo(int recordNo, object value)
		{
			string text = this.values[recordNo];
			if (text == null)
			{
				if (this.NullValue == value)
				{
					return 0;
				}
				return -1;
			}
			else
			{
				if (this.NullValue == value)
				{
					return 1;
				}
				return this.Table.Compare(text, (string)value);
			}
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x0022F6F8 File Offset: 0x0022EAF8
		public override object ConvertValue(object value)
		{
			if (this.NullValue != value)
			{
				if (value != null)
				{
					value = value.ToString();
				}
				else
				{
					value = this.NullValue;
				}
			}
			return value;
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x0022F724 File Offset: 0x0022EB24
		public override void Copy(int recordNo1, int recordNo2)
		{
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x0022F744 File Offset: 0x0022EB44
		public override object Get(int recordNo)
		{
			string text = this.values[recordNo];
			if (text != null)
			{
				return text;
			}
			return this.NullValue;
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x0022F768 File Offset: 0x0022EB68
		public override int GetStringLength(int record)
		{
			string text = this.values[record];
			if (text == null)
			{
				return 0;
			}
			return text.Length;
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x0022F78C File Offset: 0x0022EB8C
		public override bool IsNull(int record)
		{
			return null == this.values[record];
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x0022F7A4 File Offset: 0x0022EBA4
		public override void Set(int record, object value)
		{
			if (this.NullValue == value)
			{
				this.values[record] = null;
				return;
			}
			this.values[record] = value.ToString();
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x0022F7D4 File Offset: 0x0022EBD4
		public override void SetCapacity(int capacity)
		{
			string[] array = new string[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x0022F814 File Offset: 0x0022EC14
		public override object ConvertXmlToObject(string s)
		{
			return s;
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x0022F824 File Offset: 0x0022EC24
		public override string ConvertObjectToXml(object value)
		{
			return (string)value;
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x0022F838 File Offset: 0x0022EC38
		protected override object GetEmptyStorage(int recordCount)
		{
			return new string[recordCount];
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x0022F84C File Offset: 0x0022EC4C
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			string[] array = (string[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, this.IsNull(record));
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x0022F87C File Offset: 0x0022EC7C
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (string[])store;
		}

		// Token: 0x04000CE6 RID: 3302
		private string[] values;
	}
}
