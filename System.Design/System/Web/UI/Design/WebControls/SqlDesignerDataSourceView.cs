using System;
using System.Collections;
using System.Data;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class SqlDesignerDataSourceView : DesignerDataSourceView
	{
		public SqlDesignerDataSourceView(SqlDataSourceDesigner owner, string viewName)
			: base(owner, viewName)
		{
			this._owner = owner;
		}

		public override bool CanDelete
		{
			get
			{
				return this._owner.SqlDataSource.DeleteCommand.Length > 0;
			}
		}

		public override bool CanInsert
		{
			get
			{
				return this._owner.SqlDataSource.InsertCommand.Length > 0;
			}
		}

		public override bool CanPage
		{
			get
			{
				return false;
			}
		}

		public override bool CanRetrieveTotalRowCount
		{
			get
			{
				return false;
			}
		}

		public override bool CanSort
		{
			get
			{
				return this._owner.SqlDataSource.DataSourceMode == SqlDataSourceMode.DataSet || this._owner.SqlDataSource.SortParameterName.Length > 0;
			}
		}

		public override bool CanUpdate
		{
			get
			{
				return this._owner.SqlDataSource.UpdateCommand.Length > 0;
			}
		}

		public override IDataSourceViewSchema Schema
		{
			get
			{
				DataTable dataTable = this._owner.LoadSchema();
				if (dataTable == null)
				{
					return null;
				}
				return new DataSetViewSchema(dataTable);
			}
		}

		public override IEnumerable GetDesignTimeData(int minimumRows, out bool isSampleData)
		{
			DataTable dataTable = this._owner.LoadSchema();
			if (dataTable != null)
			{
				isSampleData = true;
				return DesignTimeData.GetDesignTimeDataSource(DesignTimeData.CreateSampleDataTable(new DataView(dataTable), true), minimumRows);
			}
			return base.GetDesignTimeData(minimumRows, out isSampleData);
		}

		private SqlDataSourceDesigner _owner;
	}
}
