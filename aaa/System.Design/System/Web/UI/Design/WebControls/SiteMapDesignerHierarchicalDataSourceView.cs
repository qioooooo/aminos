using System;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020004AE RID: 1198
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class SiteMapDesignerHierarchicalDataSourceView : DesignerHierarchicalDataSourceView
	{
		// Token: 0x06002B65 RID: 11109 RVA: 0x000EF7B8 File Offset: 0x000EE7B8
		public SiteMapDesignerHierarchicalDataSourceView(SiteMapDataSourceDesigner owner, string viewPath)
			: base(owner, viewPath)
		{
			this._owner = owner;
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06002B66 RID: 11110 RVA: 0x000EF7C9 File Offset: 0x000EE7C9
		public override IDataSourceSchema Schema
		{
			get
			{
				return SiteMapDataSourceDesigner.SiteMapHierarchicalSchema;
			}
		}

		// Token: 0x06002B67 RID: 11111 RVA: 0x000EF7D0 File Offset: 0x000EE7D0
		public override IHierarchicalEnumerable GetDesignTimeData(out bool isSampleData)
		{
			string text = null;
			string text2 = null;
			IHierarchicalEnumerable hierarchicalEnumerable = null;
			isSampleData = true;
			text = this._owner.SiteMapDataSource.SiteMapProvider;
			text2 = this._owner.SiteMapDataSource.StartingNodeUrl;
			this._owner.SiteMapDataSource.Provider = this._owner.DesignTimeSiteMapProvider;
			try
			{
				this._owner.SiteMapDataSource.StartingNodeUrl = null;
				hierarchicalEnumerable = ((IHierarchicalDataSource)this._owner.SiteMapDataSource).GetHierarchicalView(base.Path).Select();
				isSampleData = false;
			}
			finally
			{
				this._owner.SiteMapDataSource.StartingNodeUrl = text2;
				this._owner.SiteMapDataSource.SiteMapProvider = text;
			}
			return hierarchicalEnumerable;
		}

		// Token: 0x04001D7D RID: 7549
		private SiteMapDataSourceDesigner _owner;
	}
}
