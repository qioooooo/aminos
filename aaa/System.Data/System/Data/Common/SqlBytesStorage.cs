using System;
using System.Collections;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace System.Data.Common
{
	// Token: 0x0200018D RID: 397
	internal sealed class SqlBytesStorage : DataStorage
	{
		// Token: 0x0600175F RID: 5983 RVA: 0x0023169C File Offset: 0x00230A9C
		public SqlBytesStorage(DataColumn column)
			: base(column, typeof(SqlBytes), SqlBytes.Null, SqlBytes.Null)
		{
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x002316C4 File Offset: 0x00230AC4
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
				throw ExprException.Overflow(typeof(SqlBytes));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x06001761 RID: 5985 RVA: 0x00231764 File Offset: 0x00230B64
		public override int Compare(int recordNo1, int recordNo2)
		{
			return 0;
		}

		// Token: 0x06001762 RID: 5986 RVA: 0x00231774 File Offset: 0x00230B74
		public override int CompareValueTo(int recordNo, object value)
		{
			return 0;
		}

		// Token: 0x06001763 RID: 5987 RVA: 0x00231784 File Offset: 0x00230B84
		public override void Copy(int recordNo1, int recordNo2)
		{
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x002317A4 File Offset: 0x00230BA4
		public override object Get(int record)
		{
			return this.values[record];
		}

		// Token: 0x06001765 RID: 5989 RVA: 0x002317BC File Offset: 0x00230BBC
		public override bool IsNull(int record)
		{
			return this.values[record].IsNull;
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x002317D8 File Offset: 0x00230BD8
		public override void Set(int record, object value)
		{
			if (value == DBNull.Value || value == null)
			{
				this.values[record] = SqlBytes.Null;
				return;
			}
			this.values[record] = (SqlBytes)value;
		}

		// Token: 0x06001767 RID: 5991 RVA: 0x0023180C File Offset: 0x00230C0C
		public override void SetCapacity(int capacity)
		{
			SqlBytes[] array = new SqlBytes[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
		}

		// Token: 0x06001768 RID: 5992 RVA: 0x0023184C File Offset: 0x00230C4C
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
			return new SqlBytes((SqlBinary)xmlSerializable);
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x002318C4 File Offset: 0x00230CC4
		public override string ConvertObjectToXml(object value)
		{
			StringWriter stringWriter = new StringWriter(base.FormatProvider);
			using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
			{
				((IXmlSerializable)value).WriteXml(xmlTextWriter);
			}
			return stringWriter.ToString();
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x00231920 File Offset: 0x00230D20
		protected override object GetEmptyStorage(int recordCount)
		{
			return new SqlBytes[recordCount];
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x00231934 File Offset: 0x00230D34
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			SqlBytes[] array = (SqlBytes[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, this.IsNull(record));
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x00231964 File Offset: 0x00230D64
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (SqlBytes[])store;
		}

		// Token: 0x04000CFC RID: 3324
		private SqlBytes[] values;
	}
}
