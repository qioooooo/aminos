using System;
using System.Collections;
using System.Data;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ObjectDesignerDataSourceView : DesignerDataSourceView
	{
		public ObjectDesignerDataSourceView(ObjectDataSourceDesigner owner, string viewName)
			: base(owner, viewName)
		{
			this._owner = owner;
		}

		public override bool CanDelete
		{
			get
			{
				return this._owner.ObjectDataSource.DeleteMethod.Length > 0;
			}
		}

		public override bool CanInsert
		{
			get
			{
				return this._owner.ObjectDataSource.InsertMethod.Length > 0;
			}
		}

		public override bool CanPage
		{
			get
			{
				return this._owner.ObjectDataSource.EnablePaging;
			}
		}

		public override bool CanRetrieveTotalRowCount
		{
			get
			{
				return this._owner.ObjectDataSource.SelectCountMethod.Length > 0;
			}
		}

		public override bool CanSort
		{
			get
			{
				if (this._owner.ObjectDataSource.SortParameterName.Length > 0)
				{
					return true;
				}
				Type selectMethodReturnType = this._owner.SelectMethodReturnType;
				return selectMethodReturnType != null && (typeof(DataSet).IsAssignableFrom(selectMethodReturnType) || typeof(DataTable).IsAssignableFrom(selectMethodReturnType) || typeof(DataView).IsAssignableFrom(selectMethodReturnType));
			}
		}

		public override bool CanUpdate
		{
			get
			{
				return this._owner.ObjectDataSource.UpdateMethod.Length > 0;
			}
		}

		public override IDataSourceViewSchema Schema
		{
			get
			{
				DataTable[] array = this._owner.LoadSchema();
				if (array != null && array.Length > 0)
				{
					if (base.Name.Length == 0)
					{
						return new DataSetViewSchema(array[0]);
					}
					foreach (DataTable dataTable in array)
					{
						if (string.Equals(dataTable.TableName, base.Name, StringComparison.OrdinalIgnoreCase))
						{
							return new DataSetViewSchema(dataTable);
						}
					}
				}
				return null;
			}
		}

		public override IEnumerable GetDesignTimeData(int minimumRows, out bool isSampleData)
		{
			isSampleData = true;
			DataTable[] array = this._owner.LoadSchema();
			if (array != null && array.Length > 0)
			{
				if (base.Name.Length == 0)
				{
					return DesignTimeData.GetDesignTimeDataSource(DesignTimeData.CreateSampleDataTable(new DataView(array[0]), true), minimumRows);
				}
				foreach (DataTable dataTable in array)
				{
					if (string.Equals(dataTable.TableName, base.Name, StringComparison.OrdinalIgnoreCase))
					{
						return DesignTimeData.GetDesignTimeDataSource(DesignTimeData.CreateSampleDataTable(new DataView(dataTable), true), minimumRows);
					}
				}
			}
			return base.GetDesignTimeData(minimumRows, out isSampleData);
		}

		private ObjectDataSourceDesigner _owner;
	}
}
