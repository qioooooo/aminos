using System;
using System.Collections;
using System.Security.Permissions;
using System.Xml;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000691 RID: 1681
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class XmlDataSourceView : DataSourceView
	{
		// Token: 0x06005273 RID: 21107 RVA: 0x0014CCD5 File Offset: 0x0014BCD5
		public XmlDataSourceView(XmlDataSource owner, string name)
			: base(owner, name)
		{
			this._owner = owner;
		}

		// Token: 0x06005274 RID: 21108 RVA: 0x0014CCE8 File Offset: 0x0014BCE8
		protected internal override IEnumerable ExecuteSelect(DataSourceSelectArguments arguments)
		{
			arguments.RaiseUnsupportedCapabilitiesError(this);
			XmlNode xmlDocument = this._owner.GetXmlDocument();
			XmlNodeList xmlNodeList;
			if (this._owner.XPath.Length != 0)
			{
				xmlNodeList = xmlDocument.SelectNodes(this._owner.XPath);
			}
			else
			{
				xmlNodeList = xmlDocument.SelectNodes("/node()/node()");
			}
			return new XmlDataSourceView.XmlDataSourceNodeDescriptorEnumeration(xmlNodeList);
		}

		// Token: 0x06005275 RID: 21109 RVA: 0x0014CD42 File Offset: 0x0014BD42
		public IEnumerable Select(DataSourceSelectArguments arguments)
		{
			return this.ExecuteSelect(arguments);
		}

		// Token: 0x04002DF8 RID: 11768
		private XmlDataSource _owner;

		// Token: 0x02000692 RID: 1682
		private class XmlDataSourceNodeDescriptorEnumeration : ICollection, IEnumerable
		{
			// Token: 0x06005276 RID: 21110 RVA: 0x0014CD4B File Offset: 0x0014BD4B
			public XmlDataSourceNodeDescriptorEnumeration(XmlNodeList nodes)
			{
				this._nodes = nodes;
			}

			// Token: 0x06005277 RID: 21111 RVA: 0x0014CEDC File Offset: 0x0014BEDC
			IEnumerator IEnumerable.GetEnumerator()
			{
				XmlDataSourceView.XmlDataSourceNodeDescriptorEnumeration.GetEnumerator>d__0 getEnumerator>d__ = new XmlDataSourceView.XmlDataSourceNodeDescriptorEnumeration.GetEnumerator>d__0(0);
				getEnumerator>d__.<>4__this = this;
				return getEnumerator>d__;
			}

			// Token: 0x170014FB RID: 5371
			// (get) Token: 0x06005278 RID: 21112 RVA: 0x0014CEF8 File Offset: 0x0014BEF8
			int ICollection.Count
			{
				get
				{
					if (this._count == -1)
					{
						this._count = 0;
						foreach (object obj in this._nodes)
						{
							XmlNode xmlNode = (XmlNode)obj;
							if (xmlNode.NodeType == XmlNodeType.Element)
							{
								this._count++;
							}
						}
					}
					return this._count;
				}
			}

			// Token: 0x170014FC RID: 5372
			// (get) Token: 0x06005279 RID: 21113 RVA: 0x0014CF78 File Offset: 0x0014BF78
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170014FD RID: 5373
			// (get) Token: 0x0600527A RID: 21114 RVA: 0x0014CF7B File Offset: 0x0014BF7B
			object ICollection.SyncRoot
			{
				get
				{
					return null;
				}
			}

			// Token: 0x0600527B RID: 21115 RVA: 0x0014CF80 File Offset: 0x0014BF80
			void ICollection.CopyTo(Array array, int index)
			{
				foreach (object obj in ((IEnumerable)this))
				{
					array.SetValue(obj, index++);
				}
			}

			// Token: 0x04002DF9 RID: 11769
			private XmlNodeList _nodes;

			// Token: 0x04002DFA RID: 11770
			private int _count = -1;
		}
	}
}
