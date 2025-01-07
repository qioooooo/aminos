using System;
using System.Collections;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class SiteMapDesignerDataSourceView : DesignerDataSourceView
	{
		public SiteMapDesignerDataSourceView(SiteMapDataSourceDesigner owner, string viewName)
			: base(owner, viewName)
		{
			this._owner = owner;
			this._siteMapDataSource = (SiteMapDataSource)this._owner.Component;
		}

		public override IDataSourceViewSchema Schema
		{
			get
			{
				return SiteMapDesignerDataSourceView._siteMapViewSchema;
			}
		}

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

		private static readonly SiteMapDataSourceDesigner.SiteMapDataSourceViewSchema _siteMapViewSchema = new SiteMapDataSourceDesigner.SiteMapDataSourceViewSchema();

		private SiteMapDataSourceDesigner _owner;

		private SiteMapDataSource _siteMapDataSource;
	}
}
