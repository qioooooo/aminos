using System;
using System.Data;

namespace System.Web.UI.Design
{
	public sealed class DataSetSchema : IDataSourceSchema
	{
		public DataSetSchema(DataSet dataSet)
		{
			if (dataSet == null)
			{
				throw new ArgumentNullException("dataSet");
			}
			this._dataSet = dataSet;
		}

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

		private DataSet _dataSet;
	}
}
