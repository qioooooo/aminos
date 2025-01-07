using System;
using System.Collections;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class XmlDesignerDataSourceView : DesignerDataSourceView
	{
		public XmlDesignerDataSourceView(XmlDataSourceDesigner owner, string viewName)
			: base(owner, viewName)
		{
			this._owner = owner;
		}

		public override IDataSourceViewSchema Schema
		{
			get
			{
				XmlDataSource designTimeXmlDataSource = this._owner.GetDesignTimeXmlDataSource(string.Empty);
				if (designTimeXmlDataSource == null)
				{
					return null;
				}
				string text = designTimeXmlDataSource.XPath;
				if (text.Length == 0)
				{
					text = "/node()/node()";
				}
				IDataSourceSchema dataSourceSchema = new XmlDocumentSchema(designTimeXmlDataSource.GetXmlDocument(), text);
				if (dataSourceSchema != null)
				{
					IDataSourceViewSchema[] views = dataSourceSchema.GetViews();
					if (views != null && views.Length > 0)
					{
						return views[0];
					}
				}
				return null;
			}
		}

		public override IEnumerable GetDesignTimeData(int minimumRows, out bool isSampleData)
		{
			IEnumerable runtimeEnumerable = this._owner.GetRuntimeEnumerable(base.Name);
			if (runtimeEnumerable != null)
			{
				isSampleData = false;
				return runtimeEnumerable;
			}
			return base.GetDesignTimeData(minimumRows, out isSampleData);
		}

		private XmlDataSourceDesigner _owner;
	}
}
