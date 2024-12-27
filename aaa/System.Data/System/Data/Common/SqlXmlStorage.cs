using System;
using System.Collections;
using System.Data.SqlTypes;
using System.Xml;

namespace System.Data.Common
{
	// Token: 0x0200019B RID: 411
	internal sealed class SqlXmlStorage : DataStorage
	{
		// Token: 0x06001835 RID: 6197 RVA: 0x0023673C File Offset: 0x00235B3C
		public SqlXmlStorage(DataColumn column)
			: base(column, typeof(SqlXml), SqlXml.Null, SqlXml.Null)
		{
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x00236764 File Offset: 0x00235B64
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
				throw ExprException.Overflow(typeof(SqlXml));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x06001837 RID: 6199 RVA: 0x00236804 File Offset: 0x00235C04
		public override int Compare(int recordNo1, int recordNo2)
		{
			return 0;
		}

		// Token: 0x06001838 RID: 6200 RVA: 0x00236814 File Offset: 0x00235C14
		public override int CompareValueTo(int recordNo, object value)
		{
			return 0;
		}

		// Token: 0x06001839 RID: 6201 RVA: 0x00236824 File Offset: 0x00235C24
		public override void Copy(int recordNo1, int recordNo2)
		{
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x0600183A RID: 6202 RVA: 0x00236844 File Offset: 0x00235C44
		public override object Get(int record)
		{
			return this.values[record];
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x0023685C File Offset: 0x00235C5C
		public override bool IsNull(int record)
		{
			return this.values[record].IsNull;
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x00236878 File Offset: 0x00235C78
		public override void Set(int record, object value)
		{
			if (value == DBNull.Value || value == null)
			{
				this.values[record] = SqlXml.Null;
				return;
			}
			this.values[record] = (SqlXml)value;
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x002368AC File Offset: 0x00235CAC
		public override void SetCapacity(int capacity)
		{
			SqlXml[] array = new SqlXml[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
		}

		// Token: 0x0600183E RID: 6206 RVA: 0x002368EC File Offset: 0x00235CEC
		public override object ConvertXmlToObject(string s)
		{
			XmlTextReader xmlTextReader = new XmlTextReader(s, XmlNodeType.Element, null);
			return new SqlXml(xmlTextReader);
		}

		// Token: 0x0600183F RID: 6207 RVA: 0x00236908 File Offset: 0x00235D08
		public override string ConvertObjectToXml(object value)
		{
			SqlXml sqlXml = (SqlXml)value;
			if (sqlXml.IsNull)
			{
				return ADP.StrEmpty;
			}
			return sqlXml.Value;
		}

		// Token: 0x06001840 RID: 6208 RVA: 0x00236930 File Offset: 0x00235D30
		protected override object GetEmptyStorage(int recordCount)
		{
			return new SqlXml[recordCount];
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x00236944 File Offset: 0x00235D44
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			SqlXml[] array = (SqlXml[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, this.IsNull(record));
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x00236974 File Offset: 0x00235D74
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (SqlXml[])store;
		}

		// Token: 0x04000D0D RID: 3341
		private SqlXml[] values;
	}
}
