using System;
using System.Collections;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020004AD RID: 1197
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class SiteMapDesignerDataSourceView : DesignerDataSourceView
	{
		// Token: 0x06002B61 RID: 11105 RVA: 0x000EF6BF File Offset: 0x000EE6BF
		public SiteMapDesignerDataSourceView(SiteMapDataSourceDesigner owner, string viewName)
			: base(owner, viewName)
		{
			this._owner = owner;
			this._siteMapDataSource = (SiteMapDataSource)this._owner.Component;
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06002B62 RID: 11106 RVA: 0x000EF6E6 File Offset: 0x000EE6E6
		public override IDataSourceViewSchema Schema
		{
			get
			{
				return SiteMapDesignerDataSourceView._siteMapViewSchema;
			}
		}

		// Token: 0x06002B63 RID: 11107 RVA: 0x000EF6F0 File Offset: 0x000EE6F0
		public override IEnumerable GetDesignTimeData(int minimumRows, out bool isSampleData)
		{
			string text = null;
			string text2 = null;
			SiteMapNodeCollection siteMapNodeCollection = null;
			text = this._siteMapDataSource.SiteMapProvider;
			text2 = this._siteMapDataSource.StartingNodeUrl;
			this._siteMapDataSource.Provider = this._owner.DesignTimeSiteMapProvider;
			try
			{
				this._siteMapDataSource.StartingNodeUrl = null;
				siteMapNodeCollection = ((SiteMapDataSourceView)((IDataSource)this._siteMapDataSource).GetView(base.Name)).Select(DataSourceSelectArguments.Empty) as SiteMapNodeCollection;
				isSampleData = false;
			}
			finally
			{
				this._siteMapDataSource.StartingNodeUrl = text2;
				this._siteMapDataSource.SiteMapProvider = text;
			}
			if (siteMapNodeCollection != null && siteMapNodeCollection.Count == 0)
			{
				isSampleData = true;
				return DesignTimeData.GetDesignTimeDataSource(DesignTimeData.CreateDummyDataBoundDataTable(), minimumRows);
			}
			return siteMapNodeCollection;
		}

		// Token: 0x04001D7A RID: 7546
		private static readonly SiteMapDataSourceDesigner.SiteMapDataSourceViewSchema _siteMapViewSchema = new SiteMapDataSourceDesigner.SiteMapDataSourceViewSchema();

		// Token: 0x04001D7B RID: 7547
		private SiteMapDataSourceDesigner _owner;

		// Token: 0x04001D7C RID: 7548
		private SiteMapDataSource _siteMapDataSource;
	}
}
