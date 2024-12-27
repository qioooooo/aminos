using System;
using System.Data;

namespace System.Web.UI.Design
{
	// Token: 0x02000350 RID: 848
	public sealed class DataSetSchema : IDataSourceSchema
	{
		// Token: 0x06001FDB RID: 8155 RVA: 0x000B5B5C File Offset: 0x000B4B5C
		public DataSetSchema(DataSet dataSet)
		{
			if (dataSet == null)
			{
				throw new ArgumentNullException("dataSet");
			}
			this._dataSet = dataSet;
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x000B5B7C File Offset: 0x000B4B7C
		public IDataSourceViewSchema[] GetViews()
		{
			DataTableCollection tables = this._dataSet.Tables;
			DataSetViewSchema[] array = new DataSetViewSchema[tables.Count];
			for (int i = 0; i < tables.Count; i++)
			{
				array[i] = new DataSetViewSchema(tables[i]);
			}
			return array;
		}

		// Token: 0x040017C1 RID: 6081
		private DataSet _dataSet;
	}
}
