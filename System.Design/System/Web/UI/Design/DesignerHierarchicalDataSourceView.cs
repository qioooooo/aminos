using System;

namespace System.Web.UI.Design
{
	public abstract class DesignerHierarchicalDataSourceView
	{
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

		public IHierarchicalDataSourceDesigner DataSourceDesigner
		{
			get
			{
				return this._owner;
			}
		}

		public string Path
		{
			get
			{
				return this._path;
			}
		}

		public virtual IDataSourceSchema Schema
		{
			get
			{
				return null;
			}
		}

		public virtual IHierarchicalEnumerable GetDesignTimeData(out bool isSampleData)
		{
			isSampleData = true;
			return null;
		}

		private string _path;

		private IHierarchicalDataSourceDesigner _owner;
	}
}
