using System;
using System.Collections;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace System.Data.Common
{
	// Token: 0x02000199 RID: 409
	internal sealed class SqlStringStorage : DataStorage
	{
		// Token: 0x06001811 RID: 6161 RVA: 0x00235D5C File Offset: 0x0023515C
		public SqlStringStorage(DataColumn column)
			: base(column, typeof(SqlString), SqlString.Null, SqlString.Null)
		{
		}

		// Token: 0x06001812 RID: 6162 RVA: 0x00235D90 File Offset: 0x00235190
		public override object Aggregate(int[] recordNos, AggregateType kind)
		{
			try
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
						if (!this.IsNull(recordNos[i]))
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
				throw ExprException.Overflow(typeof(SqlString));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x00235EEC File Offset: 0x002352EC
		public override int Compare(int recordNo1, int recordNo2)
		{
			return this.Compare(this.values[recordNo1], this.values[recordNo2]);
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x00235F24 File Offset: 0x00235324
		public int Compare(SqlString valueNo1, SqlString valueNo2)
		{
			if (valueNo1.IsNull && valueNo2.IsNull)
			{
				return 0;
			}
			if (valueNo1.IsNull)
			{
				return -1;
			}
			if (valueNo2.IsNull)
			{
				return 1;
			}
			return this.Table.Compare(valueNo1.Value, valueNo2.Value);
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x00235F74 File Offset: 0x00235374
		public override int CompareValueTo(int recordNo, object value)
		{
			return this.Compare(this.values[recordNo], (SqlString)value);
		}

		// Token: 0x06001816 RID: 6166 RVA: 0x00235FA0 File Offset: 0x002353A0
		public override object ConvertValue(object value)
		{
			if (value != null)
			{
				return SqlConvert.ConvertToSqlString(value);
			}
			return this.NullValue;
		}

		// Token: 0x06001817 RID: 6167 RVA: 0x00235FC4 File Offset: 0x002353C4
		public override void Copy(int recordNo1, int recordNo2)
		{
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x06001818 RID: 6168 RVA: 0x00235FF4 File Offset: 0x002353F4
		public override object Get(int record)
		{
			return this.values[record];
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x00236018 File Offset: 0x00235418
		public override int GetStringLength(int record)
		{
			SqlString sqlString = this.values[record];
			if (!sqlString.IsNull)
			{
				return sqlString.Value.Length;
			}
			return 0;
		}

		// Token: 0x0600181A RID: 6170 RVA: 0x00236050 File Offset: 0x00235450
		public override bool IsNull(int record)
		{
			return this.values[record].IsNull;
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x00236070 File Offset: 0x00235470
		public override void Set(int record, object value)
		{
			this.values[record] = SqlConvert.ConvertToSqlString(value);
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x00236094 File Offset: 0x00235494
		public override void SetCapacity(int capacity)
		{
			SqlString[] array = new SqlString[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x002360D4 File Offset: 0x002354D4
		public override object ConvertXmlToObject(string s)
		{
			SqlString sqlString = default(SqlString);
			string text = "<col>" + s + "</col>";
			StringReader stringReader = new StringReader(text);
			IXmlSerializable xmlSerializable = sqlString;
			using (XmlTextReader xmlTextReader = new XmlTextReader(stringReader))
			{
				xmlSerializable.ReadXml(xmlTextReader);
			}
			return (SqlString)xmlSerializable;
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x0023614C File Offset: 0x0023554C
		public override string ConvertObjectToXml(object value)
		{
			StringWriter stringWriter = new StringWriter(base.FormatProvider);
			using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
			{
				((IXmlSerializable)value).WriteXml(xmlTextWriter);
			}
			return stringWriter.ToString();
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x002361A8 File Offset: 0x002355A8
		protected override object GetEmptyStorage(int recordCount)
		{
			return new SqlString[recordCount];
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x002361BC File Offset: 0x002355BC
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			SqlString[] array = (SqlString[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, this.IsNull(record));
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x00236200 File Offset: 0x00235600
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (SqlString[])store;
		}

		// Token: 0x04000D08 RID: 3336
		private SqlString[] values;
	}
}
