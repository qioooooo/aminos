using System;
using System.Collections;
using System.Data;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020004E5 RID: 1253
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class SqlDesignerDataSourceView : DesignerDataSourceView
	{
		// Token: 0x06002CFB RID: 11515 RVA: 0x000FE5A9 File Offset: 0x000FD5A9
		public SqlDesignerDataSourceView(SqlDataSourceDesigner owner, string viewName)
			: base(owner, viewName)
		{
			this._owner = owner;
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x06002CFC RID: 11516 RVA: 0x000FE5BA File Offset: 0x000FD5BA
		public override bool CanDelete
		{
			get
			{
				return this._owner.SqlDataSource.DeleteCommand.Length > 0;
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06002CFD RID: 11517 RVA: 0x000FE5D4 File Offset: 0x000FD5D4
		public override bool CanInsert
		{
			get
			{
				return this._owner.SqlDataSource.InsertCommand.Length > 0;
			}
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06002CFE RID: 11518 RVA: 0x000FE5EE File Offset: 0x000FD5EE
		public override bool CanPage
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06002CFF RID: 11519 RVA: 0x000FE5F1 File Offset: 0x000FD5F1
		public override bool CanRetrieveTotalRowCount
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06002D00 RID: 11520 RVA: 0x000FE5F4 File Offset: 0x000FD5F4
		public override bool CanSort
		{
			get
			{
				return this._owner.SqlDataSource.DataSourceMode == SqlDataSourceMode.DataSet || this._owner.SqlDataSource.SortParameterName.Length > 0;
			}
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06002D01 RID: 11521 RVA: 0x000FE623 File Offset: 0x000FD623
		public override bool CanUpdate
		{
			get
			{
				return this._owner.SqlDataSource.UpdateCommand.Length > 0;
			}
		}

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06002D02 RID: 11522 RVA: 0x000FE640 File Offset: 0x000FD640
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

		// Token: 0x06002D03 RID: 11523 RVA: 0x000FE664 File Offset: 0x000FD664
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

		// Token: 0x04001EB3 RID: 7859
		private SqlDataSourceDesigner _owner;
	}
}
