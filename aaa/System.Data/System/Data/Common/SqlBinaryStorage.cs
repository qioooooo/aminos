using System;
using System.Collections;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace System.Data.Common
{
	// Token: 0x0200018B RID: 395
	internal sealed class SqlBinaryStorage : DataStorage
	{
		// Token: 0x06001741 RID: 5953 RVA: 0x00230ECC File Offset: 0x002302CC
		public SqlBinaryStorage(DataColumn column)
			: base(column, typeof(SqlBinary), SqlBinary.Null, SqlBinary.Null)
		{
		}

		// Token: 0x06001742 RID: 5954 RVA: 0x00230F00 File Offset: 0x00230300
		public override object Aggregate(int[] records, AggregateType kind)
		{
			try
			{
				switch (kind)
				{
				case AggregateType.First:
					if (records.Length > 0)
					{
						return this.values[records[0]];
					}
					return null;
				case AggregateType.Count:
				{
					int num = 0;
					for (int i = 0; i < records.Length; i++)
					{
						if (!this.IsNull(records[i]))
						{
							num++;
						}
					}
					return num;
				}
				}
			}
			catch (OverflowException)
			{
				throw ExprException.Overflow(typeof(SqlBinary));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x06001743 RID: 5955 RVA: 0x00230FB0 File Offset: 0x002303B0
		public override int Compare(int recordNo1, int recordNo2)
		{
			return this.values[recordNo1].CompareTo(this.values[recordNo2]);
		}

		// Token: 0x06001744 RID: 5956 RVA: 0x00230FE0 File Offset: 0x002303E0
		public override int CompareValueTo(int recordNo, object value)
		{
			return this.values[recordNo].CompareTo((SqlBinary)value);
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x00231004 File Offset: 0x00230404
		public override object ConvertValue(object value)
		{
			if (value != null)
			{
				return SqlConvert.ConvertToSqlBinary(value);
			}
			return this.NullValue;
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x00231028 File Offset: 0x00230428
		public override void Copy(int recordNo1, int recordNo2)
		{
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x06001747 RID: 5959 RVA: 0x00231058 File Offset: 0x00230458
		public override object Get(int record)
		{
			return this.values[record];
		}

		// Token: 0x06001748 RID: 5960 RVA: 0x0023107C File Offset: 0x0023047C
		public override bool IsNull(int record)
		{
			return this.values[record].IsNull;
		}

		// Token: 0x06001749 RID: 5961 RVA: 0x0023109C File Offset: 0x0023049C
		public override void Set(int record, object value)
		{
			this.values[record] = SqlConvert.ConvertToSqlBinary(value);
		}

		// Token: 0x0600174A RID: 5962 RVA: 0x002310C0 File Offset: 0x002304C0
		public override void SetCapacity(int capacity)
		{
			SqlBinary[] array = new SqlBinary[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
		}

		// Token: 0x0600174B RID: 5963 RVA: 0x00231100 File Offset: 0x00230500
		public override object ConvertXmlToObject(string s)
		{
			SqlBinary sqlBinary = default(SqlBinary);
			string text = "<col>" + s + "</col>";
			StringReader stringReader = new StringReader(text);
			IXmlSerializable xmlSerializable = sqlBinary;
			using (XmlTextReader xmlTextReader = new XmlTextReader(stringReader))
			{
				xmlSerializable.ReadXml(xmlTextReader);
			}
			return (SqlBinary)xmlSerializable;
		}

		// Token: 0x0600174C RID: 5964 RVA: 0x00231178 File Offset: 0x00230578
		public override string ConvertObjectToXml(object value)
		{
			StringWriter stringWriter = new StringWriter(base.FormatProvider);
			using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
			{
				((IXmlSerializable)value).WriteXml(xmlTextWriter);
			}
			return stringWriter.ToString();
		}

		// Token: 0x0600174D RID: 5965 RVA: 0x002311D4 File Offset: 0x002305D4
		protected override object GetEmptyStorage(int recordCount)
		{
			return new SqlBinary[recordCount];
		}

		// Token: 0x0600174E RID: 5966 RVA: 0x002311E8 File Offset: 0x002305E8
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			SqlBinary[] array = (SqlBinary[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, this.IsNull(record));
		}

		// Token: 0x0600174F RID: 5967 RVA: 0x0023122C File Offset: 0x0023062C
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (SqlBinary[])store;
		}

		// Token: 0x04000CFA RID: 3322
		private SqlBinary[] values;
	}
}
