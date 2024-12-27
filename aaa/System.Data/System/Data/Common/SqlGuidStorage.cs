using System;
using System.Collections;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace System.Data.Common
{
	// Token: 0x02000193 RID: 403
	internal sealed class SqlGuidStorage : DataStorage
	{
		// Token: 0x060017B7 RID: 6071 RVA: 0x0023363C File Offset: 0x00232A3C
		public SqlGuidStorage(DataColumn column)
			: base(column, typeof(SqlGuid), SqlGuid.Null, SqlGuid.Null)
		{
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x00233670 File Offset: 0x00232A70
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
				throw ExprException.Overflow(typeof(SqlGuid));
			}
			throw ExceptionBuilder.AggregateException(kind, this.DataType);
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x00233720 File Offset: 0x00232B20
		public override int Compare(int recordNo1, int recordNo2)
		{
			return this.values[recordNo1].CompareTo(this.values[recordNo2]);
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x00233750 File Offset: 0x00232B50
		public override int CompareValueTo(int recordNo, object value)
		{
			return this.values[recordNo].CompareTo((SqlGuid)value);
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x00233774 File Offset: 0x00232B74
		public override object ConvertValue(object value)
		{
			if (value != null)
			{
				return SqlConvert.ConvertToSqlGuid(value);
			}
			return this.NullValue;
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x00233798 File Offset: 0x00232B98
		public override void Copy(int recordNo1, int recordNo2)
		{
			this.values[recordNo2] = this.values[recordNo1];
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x002337C8 File Offset: 0x00232BC8
		public override object Get(int record)
		{
			return this.values[record];
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x002337EC File Offset: 0x00232BEC
		public override bool IsNull(int record)
		{
			return this.values[record].IsNull;
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x0023380C File Offset: 0x00232C0C
		public override void Set(int record, object value)
		{
			this.values[record] = SqlConvert.ConvertToSqlGuid(value);
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x00233830 File Offset: 0x00232C30
		public override void SetCapacity(int capacity)
		{
			SqlGuid[] array = new SqlGuid[capacity];
			if (this.values != null)
			{
				Array.Copy(this.values, 0, array, 0, Math.Min(capacity, this.values.Length));
			}
			this.values = array;
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x00233870 File Offset: 0x00232C70
		public override object ConvertXmlToObject(string s)
		{
			SqlGuid sqlGuid = default(SqlGuid);
			string text = "<col>" + s + "</col>";
			StringReader stringReader = new StringReader(text);
			IXmlSerializable xmlSerializable = sqlGuid;
			using (XmlTextReader xmlTextReader = new XmlTextReader(stringReader))
			{
				xmlSerializable.ReadXml(xmlTextReader);
			}
			return (SqlGuid)xmlSerializable;
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x002338E8 File Offset: 0x00232CE8
		public override string ConvertObjectToXml(object value)
		{
			StringWriter stringWriter = new StringWriter(base.FormatProvider);
			using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
			{
				((IXmlSerializable)value).WriteXml(xmlTextWriter);
			}
			return stringWriter.ToString();
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x00233944 File Offset: 0x00232D44
		protected override object GetEmptyStorage(int recordCount)
		{
			return new SqlGuid[recordCount];
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x00233958 File Offset: 0x00232D58
		protected override void CopyValue(int record, object store, BitArray nullbits, int storeIndex)
		{
			SqlGuid[] array = (SqlGuid[])store;
			array[storeIndex] = this.values[record];
			nullbits.Set(storeIndex, this.IsNull(record));
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x0023399C File Offset: 0x00232D9C
		protected override void SetStorage(object store, BitArray nullbits)
		{
			this.values = (SqlGuid[])store;
		}

		// Token: 0x04000D02 RID: 3330
		private SqlGuid[] values;
	}
}
