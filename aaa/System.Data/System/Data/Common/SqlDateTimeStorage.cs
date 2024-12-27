using System;
using System.Collections;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace System.Data.Common
{
	// Token: 0x02000190 RID: 400
	internal sealed class SqlDateTimeStorage : DataStorage
	{
		// Token: 0x0600178A RID: 6026 RVA: 0x00232380 File Offset: 0x00231780
		public SqlDateTimeStorage(DataColumn column)
			: base(column, typeof(SqlDateTime), SqlDateTime.Null, SqlDateTime.Null)
		{
		}

		// Token: 0x0600178B RID: 6027 RVA: 0x002323B4 File Offset: 0x002317B4
		public override object Aggregate(int[] records, AggregateType kind)
		{
			bool flag = false;
			try
			{
				switch (kind)
				{
				case AggregateType.Min:
				{
					SqlDateTime sqlDateTime = SqlDateTime.MaxValue;
					foreach (int num in records)
					{
						if (!this.IsNull(num))
						{
							if (SqlDateTime.LessThan(this.values[num], sqlDateTime).IsTrue)
							{
								sqlDateTime = this.values[num];
							}
							flag = true;
						}
					}
					if (flag)
					{
						return sqlDateTime;
					}
					return this.NullValue;
				}
				case AggregateType.Max:
				{
					SqlDateTime sqlDateTime2 = SqlDateTime.MinValue;
					foreach (int num2 in records)
					{
						if (!this.IsNull(num2))
						{
							if (SqlDateTime.GreaterThan(this.values[num2], sqlDateTime2).IsTrue)
							{
								sqlDateTime2 = this.values[num2];
							}
							flag = true;
						}
					}
					if (flag)
					{
						return sqlDateTime2;
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
						if (!this.IsNull(records[k]))
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
				throw ExprException.Overflow(typeof(SqlDateTime));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x0600178C RID: 6028 RVA: 0x0023256C File Offset: 0x0023196C
		public override int Compare(int recordNo1, int recordNo2)
		{
			return this.values[recordNo1].CompareTo(this.values[recordNo2]);
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x0023259C File Offset: 0x0023199C
		public override int CompareValueTo(int recordNo, object value)
		{
			return this.values[recordNo].CompareTo((SqlDateTime)value);
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x002325C0 File Offset: 0x002319C0
		public override object ConvertValue(object value)
		{
			if (value != null)
			{
				return SqlConvert.ConvertToSqlDateTime(value);
			}
			return this.NullValue;
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x002325E4 File Offset: 0x002319E4
		public override void Copy(int recordNo1, int recordNo2)
		{
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x00232614 File Offset: 0x00231A14
		public override object Get(int record)
		{
			return this.values[record];
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x00232638 File Offset: 0x00231A38
		public override bool IsNull(int record)
		{
			return this.values[record].IsNull;
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x00232658 File Offset: 0x00231A58
		public override void Set(int record, object value)
		{
			this.values[record] = SqlConvert.ConvertToSqlDateTime(value);
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x0023267C File Offset: 0x00231A7C
		public override void SetCapacity(int capacity)
		{
			SqlDateTime[] array = new SqlDateTime[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x002326BC File Offset: 0x00231ABC
		public override object ConvertXmlToObject(string s)
		{
			SqlDateTime sqlDateTime = default(SqlDateTime);
			string text = "<col>" + s + "</col>";
			StringReader stringReader = new StringReader(text);
			IXmlSerializable xmlSerializable = sqlDateTime;
			using (XmlTextReader xmlTextReader = new XmlTextReader(stringReader))
			{
				xmlSerializable.ReadXml(xmlTextReader);
			}
			return (SqlDateTime)xmlSerializable;
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x00232734 File Offset: 0x00231B34
		public override string ConvertObjectToXml(object value)
		{
			StringWriter stringWriter = new StringWriter(base.FormatProvider);
			using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
			{
				((IXmlSerializable)value).WriteXml(xmlTextWriter);
			}
			return stringWriter.ToString();
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x00232790 File Offset: 0x00231B90
		protected override object GetEmptyStorage(int recordCount)
		{
			return new SqlDateTime[recordCount];
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x002327A4 File Offset: 0x00231BA4
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			SqlDateTime[] array = (SqlDateTime[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(record, this.IsNull(record));
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x002327E4 File Offset: 0x00231BE4
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (SqlDateTime[])store;
		}

		// Token: 0x04000CFF RID: 3327
		private SqlDateTime[] values;
	}
}
