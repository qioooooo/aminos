using System;
using System.Collections.Generic;
using System.Data;

namespace System.Web.UI.Design
{
	public sealed class DataSetViewSchema : IDataSourceViewSchema
	{
		public DataSetViewSchema(DataTable dataTable)
		{
			if (dataTable == null)
			{
				throw new ArgumentNullException("dataTable");
			}
			this._dataTable = dataTable;
		}

		public string Name
		{
			get
			{
				return this._dataTable.TableName;
			}
		}

		public IDataSourceViewSchema[] GetChildren()
		{
			return null;
		}

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

		private DataTable _dataTable;
	}
}
