using System;
using System.Collections.Generic;
using System.Data;

namespace System.Web.UI.Design
{
	// Token: 0x02000351 RID: 849
	public sealed class DataSetViewSchema : IDataSourceViewSchema
	{
		// Token: 0x06001FDD RID: 8157 RVA: 0x000B5BC2 File Offset: 0x000B4BC2
		public DataSetViewSchema(DataTable dataTable)
		{
			if (dataTable == null)
			{
				throw new ArgumentNullException("dataTable");
			}
			this._dataTable = dataTable;
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06001FDE RID: 8158 RVA: 0x000B5BDF File Offset: 0x000B4BDF
		public string Name
		{
			get
			{
				return this._dataTable.TableName;
			}
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x000B5BEC File Offset: 0x000B4BEC
		public IDataSourceViewSchema[] GetChildren()
		{
			return null;
		}

		// Token: 0x06001FE0 RID: 8160 RVA: 0x000B5BF0 File Offset: 0x000B4BF0
		public IDataSourceFieldSchema[] GetFields()
		{
			List<DataSetFieldSchema> list = new List<DataSetFieldSchema>();
			foreach (object obj in this._dataTable.Columns)
			{
				DataColumn dataColumn = (DataColumn)obj;
				if (dataColumn.ColumnMapping != MappingType.Hidden)
				{
					list.Add(new DataSetFieldSchema(dataColumn));
				}
			}
			return list.ToArray();
		}

		// Token: 0x040017C2 RID: 6082
		private DataTable _dataTable;
	}
}
