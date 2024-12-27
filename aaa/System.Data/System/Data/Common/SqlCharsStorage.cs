using System;
using System.Collections;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace System.Data.Common
{
	// Token: 0x0200018F RID: 399
	internal sealed class SqlCharsStorage : DataStorage
	{
		// Token: 0x0600177C RID: 6012 RVA: 0x0023209C File Offset: 0x0023149C
		public SqlCharsStorage(DataColumn column)
			: base(column, typeof(SqlChars), SqlChars.Null, SqlChars.Null)
		{
		}

		// Token: 0x0600177D RID: 6013 RVA: 0x002320C4 File Offset: 0x002314C4
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
				throw ExprException.Overflow(typeof(SqlChars));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x00232164 File Offset: 0x00231564
		public override int Compare(int recordNo1, int recordNo2)
		{
			return 0;
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x00232174 File Offset: 0x00231574
		public override int CompareValueTo(int recordNo, object value)
		{
			return 0;
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x00232184 File Offset: 0x00231584
		public override void Copy(int recordNo1, int recordNo2)
		{
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x002321A4 File Offset: 0x002315A4
		public override object Get(int record)
		{
			return this.values[record];
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x002321BC File Offset: 0x002315BC
		public override bool IsNull(int record)
		{
			return this.values[record].IsNull;
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x002321D8 File Offset: 0x002315D8
		public override void Set(int record, object value)
		{
			if (value == DBNull.Value || value == null)
			{
				this.values[record] = SqlChars.Null;
				return;
			}
			this.values[record] = (SqlChars)value;
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x0023220C File Offset: 0x0023160C
		public override void SetCapacity(int capacity)
		{
			SqlChars[] array = new SqlChars[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x0023224C File Offset: 0x0023164C
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
			return new SqlChars((SqlString)xmlSerializable);
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x002322C4 File Offset: 0x002316C4
		public override string ConvertObjectToXml(object value)
		{
			StringWriter stringWriter = new StringWriter(base.FormatProvider);
			using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
			{
				((IXmlSerializable)value).WriteXml(xmlTextWriter);
			}
			return stringWriter.ToString();
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x00232320 File Offset: 0x00231720
		protected override object GetEmptyStorage(int recordCount)
		{
			return new SqlChars[recordCount];
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x00232334 File Offset: 0x00231734
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			SqlChars[] array = (SqlChars[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, this.IsNull(record));
		}

		// Token: 0x06001789 RID: 6025 RVA: 0x00232364 File Offset: 0x00231764
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (SqlChars[])store;
		}

		// Token: 0x04000CFE RID: 3326
		private SqlChars[] values;
	}
}
