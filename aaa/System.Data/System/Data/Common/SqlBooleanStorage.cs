using System;
using System.Collections;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace System.Data.Common
{
	// Token: 0x0200018C RID: 396
	internal sealed class SqlBooleanStorage : DataStorage
	{
		// Token: 0x06001750 RID: 5968 RVA: 0x00231248 File Offset: 0x00230648
		public SqlBooleanStorage(DataColumn column)
			: base(column, typeof(SqlBoolean), SqlBoolean.Null, SqlBoolean.Null)
		{
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x0023127C File Offset: 0x0023067C
		public override object Aggregate(int[] records, AggregateType kind)
		{
			bool flag = false;
			try
			{
				switch (kind)
				{
				case AggregateType.Min:
				{
					SqlBoolean sqlBoolean = true;
					foreach (int num in records)
					{
						if (!this.IsNull(num))
						{
							sqlBoolean = SqlBoolean.And(this.values[num], sqlBoolean);
							flag = true;
						}
					}
					if (flag)
					{
						return sqlBoolean;
					}
					return this.NullValue;
				}
				case AggregateType.Max:
				{
					SqlBoolean sqlBoolean2 = false;
					foreach (int num2 in records)
					{
						if (!this.IsNull(num2))
						{
							sqlBoolean2 = SqlBoolean.Or(this.values[num2], sqlBoolean2);
							flag = true;
						}
					}
					if (flag)
					{
						return sqlBoolean2;
					}
					return this.NullValue;
				}
				case AggregateType.First:
					if (records.Length > 0)
					{
						return this.values[records[0]];
					}
					return this.NullValue;
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
				throw ExprException.Overflow(typeof(SqlBoolean));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x00231404 File Offset: 0x00230804
		public override int Compare(int recordNo1, int recordNo2)
		{
			return this.values[recordNo1].CompareTo(this.values[recordNo2]);
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x00231434 File Offset: 0x00230834
		public override int CompareValueTo(int recordNo, object value)
		{
			return this.values[recordNo].CompareTo((SqlBoolean)value);
		}

		// Token: 0x06001754 RID: 5972 RVA: 0x00231458 File Offset: 0x00230858
		public override object ConvertValue(object value)
		{
			if (value != null)
			{
				return SqlConvert.ConvertToSqlBoolean(value);
			}
			return this.NullValue;
		}

		// Token: 0x06001755 RID: 5973 RVA: 0x0023147C File Offset: 0x0023087C
		public override void Copy(int recordNo1, int recordNo2)
		{
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x06001756 RID: 5974 RVA: 0x002314AC File Offset: 0x002308AC
		public override object Get(int record)
		{
			return this.values[record];
		}

		// Token: 0x06001757 RID: 5975 RVA: 0x002314D0 File Offset: 0x002308D0
		public override bool IsNull(int record)
		{
			return this.values[record].IsNull;
		}

		// Token: 0x06001758 RID: 5976 RVA: 0x002314F0 File Offset: 0x002308F0
		public override void Set(int record, object value)
		{
			this.values[record] = SqlConvert.ConvertToSqlBoolean(value);
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x00231514 File Offset: 0x00230914
		public override void SetCapacity(int capacity)
		{
			SqlBoolean[] array = new SqlBoolean[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
		}

		// Token: 0x0600175A RID: 5978 RVA: 0x00231554 File Offset: 0x00230954
		public override object ConvertXmlToObject(string s)
		{
			SqlBoolean sqlBoolean = default(SqlBoolean);
			string text = "<col>" + s + "</col>";
			StringReader stringReader = new StringReader(text);
			IXmlSerializable xmlSerializable = sqlBoolean;
			using (XmlTextReader xmlTextReader = new XmlTextReader(stringReader))
			{
				xmlSerializable.ReadXml(xmlTextReader);
			}
			return (SqlBoolean)xmlSerializable;
		}

		// Token: 0x0600175B RID: 5979 RVA: 0x002315CC File Offset: 0x002309CC
		public override string ConvertObjectToXml(object value)
		{
			StringWriter stringWriter = new StringWriter(base.FormatProvider);
			using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
			{
				((IXmlSerializable)value).WriteXml(xmlTextWriter);
			}
			return stringWriter.ToString();
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x00231628 File Offset: 0x00230A28
		protected override object GetEmptyStorage(int recordCount)
		{
			return new SqlBoolean[recordCount];
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x0023163C File Offset: 0x00230A3C
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			SqlBoolean[] array = (SqlBoolean[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, this.IsNull(record));
		}

		// Token: 0x0600175E RID: 5982 RVA: 0x00231680 File Offset: 0x00230A80
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (SqlBoolean[])store;
		}

		// Token: 0x04000CFB RID: 3323
		private SqlBoolean[] values;
	}
}
