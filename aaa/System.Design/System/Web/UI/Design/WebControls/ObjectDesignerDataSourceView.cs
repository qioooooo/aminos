using System;
using System.Collections;
using System.Data;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200048B RID: 1163
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ObjectDesignerDataSourceView : DesignerDataSourceView
	{
		// Token: 0x06002A3D RID: 10813 RVA: 0x000E86E6 File Offset: 0x000E76E6
		public ObjectDesignerDataSourceView(ObjectDataSourceDesigner owner, string viewName)
			: base(owner, viewName)
		{
			this._owner = owner;
		}

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06002A3E RID: 10814 RVA: 0x000E86F7 File Offset: 0x000E76F7
		public override bool CanDelete
		{
			get
			{
				return this._owner.ObjectDataSource.DeleteMethod.Length > 0;
			}
		}

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06002A3F RID: 10815 RVA: 0x000E8711 File Offset: 0x000E7711
		public override bool CanInsert
		{
			get
			{
				return this._owner.ObjectDataSource.InsertMethod.Length > 0;
			}
		}

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06002A40 RID: 10816 RVA: 0x000E872B File Offset: 0x000E772B
		public override bool CanPage
		{
			get
			{
				return this._owner.ObjectDataSource.EnablePaging;
			}
		}

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x06002A41 RID: 10817 RVA: 0x000E873D File Offset: 0x000E773D
		public override bool CanRetrieveTotalRowCount
		{
			get
			{
				return this._owner.ObjectDataSource.SelectCountMethod.Length > 0;
			}
		}

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x06002A42 RID: 10818 RVA: 0x000E8758 File Offset: 0x000E7758
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

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x06002A43 RID: 10819 RVA: 0x000E87C6 File Offset: 0x000E77C6
		public override bool CanUpdate
		{
			get
			{
				return this._owner.ObjectDataSource.UpdateMethod.Length > 0;
			}
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x06002A44 RID: 10820 RVA: 0x000E87E0 File Offset: 0x000E77E0
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

		// Token: 0x06002A45 RID: 10821 RVA: 0x000E8854 File Offset: 0x000E7854
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

		// Token: 0x04001CDB RID: 7387
		private ObjectDataSourceDesigner _owner;
	}
}
