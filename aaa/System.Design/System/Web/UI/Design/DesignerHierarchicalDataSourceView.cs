using System;

namespace System.Web.UI.Design
{
	// Token: 0x0200035C RID: 860
	public abstract class DesignerHierarchicalDataSourceView
	{
		// Token: 0x06002049 RID: 8265 RVA: 0x000B6681 File Offset: 0x000B5681
		protected DesignerHierarchicalDataSourceView(IHierarchicalDataSourceDesigner owner, string viewPath)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			if (viewPath == null)
			{
				throw new ArgumentNullException("viewPath");
			}
			this._owner = owner;
			this._path = viewPath;
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x0600204A RID: 8266 RVA: 0x000B66B3 File Offset: 0x000B56B3
		public IHierarchicalDataSourceDesigner DataSourceDesigner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x0600204B RID: 8267 RVA: 0x000B66BB File Offset: 0x000B56BB
		public string Path
		{
			get
			{
				return this._path;
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x0600204C RID: 8268 RVA: 0x000B66C3 File Offset: 0x000B56C3
		public virtual IDataSourceSchema Schema
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x000B66C6 File Offset: 0x000B56C6
		public virtual IHierarchicalEnumerable GetDesignTimeData(out bool isSampleData)
		{
			isSampleData = true;
			return null;
		}

		// Token: 0x040017CF RID: 6095
		private string _path;

		// Token: 0x040017D0 RID: 6096
		private IHierarchicalDataSourceDesigner _owner;
	}
}
