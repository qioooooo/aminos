using System;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class SiteMapDesignerHierarchicalDataSourceView : DesignerHierarchicalDataSourceView
	{
		public SiteMapDesignerHierarchicalDataSourceView(SiteMapDataSourceDesigner owner, string viewPath)
			: base(owner, viewPath)
		{
			this._owner = owner;
		}

		public override IDataSourceSchema Schema
		{
			get
			{
				return SiteMapDataSourceDesigner.SiteMapHierarchicalSchema;
			}
		}

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

		private SiteMapDataSourceDesigner _owner;
	}
}
