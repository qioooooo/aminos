using System;
using System.Security.Permissions;
using System.Xml;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000693 RID: 1683
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class XmlHierarchicalDataSourceView : HierarchicalDataSourceView
	{
		// Token: 0x0600527C RID: 21116 RVA: 0x0014CFB0 File Offset: 0x0014BFB0
		internal XmlHierarchicalDataSourceView(XmlDataSource owner, string viewPath)
		{
			this._owner = owner;
			this._viewPath = viewPath;
		}

		// Token: 0x0600527D RID: 21117 RVA: 0x0014CFC8 File Offset: 0x0014BFC8
		public override IHierarchicalEnumerable Select()
		{
			XmlNode xmlDocument = this._owner.GetXmlDocument();
			XmlNodeList xmlNodeList = null;
			if (!string.IsNullOrEmpty(this._viewPath))
			{
				XmlNode xmlNode = xmlDocument.SelectSingleNode(this._viewPath);
				if (xmlNode != null)
				{
					xmlNodeList = xmlNode.ChildNodes;
				}
			}
			else if (this._owner.XPath.Length > 0)
			{
				xmlNodeList = xmlDocument.SelectNodes(this._owner.XPath);
			}
			else
			{
				xmlNodeList = xmlDocument.ChildNodes;
			}
			return new XmlHierarchicalEnumerable(xmlNodeList);
		}

		// Token: 0x04002DFB RID: 11771
		private XmlDataSource _owner;

		// Token: 0x04002DFC RID: 11772
		private string _viewPath;
	}
}
