using System;
using System.Xml.XPath;

namespace System.Xml
{
	// Token: 0x02000383 RID: 899
	internal sealed class DataDocumentXPathNavigator : XPathNavigator, IHasXmlNode
	{
		// Token: 0x06002F86 RID: 12166 RVA: 0x002B0C10 File Offset: 0x002B0010
		internal DataDocumentXPathNavigator(XmlDataDocument doc, XmlNode node)
		{
			this._curNode = new XPathNodePointer(this, doc, node);
			this._temp = new XPathNodePointer(this, doc, node);
			this._doc = doc;
		}

		// Token: 0x06002F87 RID: 12167 RVA: 0x002B0C48 File Offset: 0x002B0048
		private DataDocumentXPathNavigator(DataDocumentXPathNavigator other)
		{
			this._curNode = other._curNode.Clone(this);
			this._temp = other._temp.Clone(this);
			this._doc = other._doc;
		}

		// Token: 0x06002F88 RID: 12168 RVA: 0x002B0C8C File Offset: 0x002B008C
		public override XPathNavigator Clone()
		{
			return new DataDocumentXPathNavigator(this);
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06002F89 RID: 12169 RVA: 0x002B0CA0 File Offset: 0x002B00A0
		internal XPathNodePointer CurNode
		{
			get
			{
				return this._curNode;
			}
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06002F8A RID: 12170 RVA: 0x002B0CB4 File Offset: 0x002B00B4
		internal XmlDataDocument Document
		{
			get
			{
				return this._doc;
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06002F8B RID: 12171 RVA: 0x002B0CC8 File Offset: 0x002B00C8
		public override XPathNodeType NodeType
		{
			get
			{
				return this._curNode.NodeType;
			}
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06002F8C RID: 12172 RVA: 0x002B0CE0 File Offset: 0x002B00E0
		public override string LocalName
		{
			get
			{
				return this._curNode.LocalName;
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06002F8D RID: 12173 RVA: 0x002B0CF8 File Offset: 0x002B00F8
		public override string NamespaceURI
		{
			get
			{
				return this._curNode.NamespaceURI;
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06002F8E RID: 12174 RVA: 0x002B0D10 File Offset: 0x002B0110
		public override string Name
		{
			get
			{
				return this._curNode.Name;
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06002F8F RID: 12175 RVA: 0x002B0D28 File Offset: 0x002B0128
		public override string Prefix
		{
			get
			{
				return this._curNode.Prefix;
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06002F90 RID: 12176 RVA: 0x002B0D40 File Offset: 0x002B0140
		public override string Value
		{
			get
			{
				XPathNodeType nodeType = this._curNode.NodeType;
				if (nodeType == XPathNodeType.Element || nodeType == XPathNodeType.Root)
				{
					return this._curNode.InnerText;
				}
				return this._curNode.Value;
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06002F91 RID: 12177 RVA: 0x002B0D78 File Offset: 0x002B0178
		public override string BaseURI
		{
			get
			{
				return this._curNode.BaseURI;
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06002F92 RID: 12178 RVA: 0x002B0D90 File Offset: 0x002B0190
		public override string XmlLang
		{
			get
			{
				return this._curNode.XmlLang;
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06002F93 RID: 12179 RVA: 0x002B0DA8 File Offset: 0x002B01A8
		public override bool IsEmptyElement
		{
			get
			{
				return this._curNode.IsEmptyElement;
			}
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06002F94 RID: 12180 RVA: 0x002B0DC0 File Offset: 0x002B01C0
		public override XmlNameTable NameTable
		{
			get
			{
				return this._doc.NameTable;
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06002F95 RID: 12181 RVA: 0x002B0DD8 File Offset: 0x002B01D8
		public override bool HasAttributes
		{
			get
			{
				return this._curNode.AttributeCount > 0;
			}
		}

		// Token: 0x06002F96 RID: 12182 RVA: 0x002B0DF4 File Offset: 0x002B01F4
		public override string GetAttribute(string localName, string namespaceURI)
		{
			if (this._curNode.NodeType != XPathNodeType.Element)
			{
				return string.Empty;
			}
			this._temp.MoveTo(this._curNode);
			if (this._temp.MoveToAttribute(localName, namespaceURI))
			{
				return this._temp.Value;
			}
			return string.Empty;
		}

		// Token: 0x06002F97 RID: 12183 RVA: 0x002B0E48 File Offset: 0x002B0248
		public override string GetNamespace(string name)
		{
			return this._curNode.GetNamespace(name);
		}

		// Token: 0x06002F98 RID: 12184 RVA: 0x002B0E64 File Offset: 0x002B0264
		public override bool MoveToNamespace(string name)
		{
			return this._curNode.NodeType == XPathNodeType.Element && this._curNode.MoveToNamespace(name);
		}

		// Token: 0x06002F99 RID: 12185 RVA: 0x002B0E90 File Offset: 0x002B0290
		public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope)
		{
			return this._curNode.NodeType == XPathNodeType.Element && this._curNode.MoveToFirstNamespace(namespaceScope);
		}

		// Token: 0x06002F9A RID: 12186 RVA: 0x002B0EBC File Offset: 0x002B02BC
		public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope)
		{
			return this._curNode.NodeType == XPathNodeType.Namespace && this._curNode.MoveToNextNamespace(namespaceScope);
		}

		// Token: 0x06002F9B RID: 12187 RVA: 0x002B0EE8 File Offset: 0x002B02E8
		public override bool MoveToAttribute(string localName, string namespaceURI)
		{
			return this._curNode.NodeType == XPathNodeType.Element && this._curNode.MoveToAttribute(localName, namespaceURI);
		}

		// Token: 0x06002F9C RID: 12188 RVA: 0x002B0F14 File Offset: 0x002B0314
		public override bool MoveToFirstAttribute()
		{
			return this._curNode.NodeType == XPathNodeType.Element && this._curNode.MoveToNextAttribute(true);
		}

		// Token: 0x06002F9D RID: 12189 RVA: 0x002B0F40 File Offset: 0x002B0340
		public override bool MoveToNextAttribute()
		{
			return this._curNode.NodeType == XPathNodeType.Attribute && this._curNode.MoveToNextAttribute(false);
		}

		// Token: 0x06002F9E RID: 12190 RVA: 0x002B0F6C File Offset: 0x002B036C
		public override bool MoveToNext()
		{
			return this._curNode.NodeType != XPathNodeType.Attribute && this._curNode.MoveToNextSibling();
		}

		// Token: 0x06002F9F RID: 12191 RVA: 0x002B0F94 File Offset: 0x002B0394
		public override bool MoveToPrevious()
		{
			return this._curNode.NodeType != XPathNodeType.Attribute && this._curNode.MoveToPreviousSibling();
		}

		// Token: 0x06002FA0 RID: 12192 RVA: 0x002B0FBC File Offset: 0x002B03BC
		public override bool MoveToFirst()
		{
			return this._curNode.NodeType != XPathNodeType.Attribute && this._curNode.MoveToFirst();
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06002FA1 RID: 12193 RVA: 0x002B0FE4 File Offset: 0x002B03E4
		public override bool HasChildren
		{
			get
			{
				return this._curNode.HasChildren;
			}
		}

		// Token: 0x06002FA2 RID: 12194 RVA: 0x002B0FFC File Offset: 0x002B03FC
		public override bool MoveToFirstChild()
		{
			return this._curNode.MoveToFirstChild();
		}

		// Token: 0x06002FA3 RID: 12195 RVA: 0x002B1014 File Offset: 0x002B0414
		public override bool MoveToParent()
		{
			return this._curNode.MoveToParent();
		}

		// Token: 0x06002FA4 RID: 12196 RVA: 0x002B102C File Offset: 0x002B042C
		public override void MoveToRoot()
		{
			this._curNode.MoveToRoot();
		}

		// Token: 0x06002FA5 RID: 12197 RVA: 0x002B1044 File Offset: 0x002B0444
		public override bool MoveTo(XPathNavigator other)
		{
			if (other == null)
			{
				return false;
			}
			DataDocumentXPathNavigator dataDocumentXPathNavigator = other as DataDocumentXPathNavigator;
			if (dataDocumentXPathNavigator == null)
			{
				return false;
			}
			if (this._curNode.MoveTo(dataDocumentXPathNavigator.CurNode))
			{
				this._doc = this._curNode.Document;
				return true;
			}
			return false;
		}

		// Token: 0x06002FA6 RID: 12198 RVA: 0x002B108C File Offset: 0x002B048C
		public override bool MoveToId(string id)
		{
			return false;
		}

		// Token: 0x06002FA7 RID: 12199 RVA: 0x002B109C File Offset: 0x002B049C
		public override bool IsSamePosition(XPathNavigator other)
		{
			if (other == null)
			{
				return false;
			}
			DataDocumentXPathNavigator dataDocumentXPathNavigator = other as DataDocumentXPathNavigator;
			return dataDocumentXPathNavigator != null && this._doc == dataDocumentXPathNavigator.Document && this._curNode.IsSamePosition(dataDocumentXPathNavigator.CurNode);
		}

		// Token: 0x06002FA8 RID: 12200 RVA: 0x002B10DC File Offset: 0x002B04DC
		XmlNode IHasXmlNode.GetNode()
		{
			return this._curNode.Node;
		}

		// Token: 0x06002FA9 RID: 12201 RVA: 0x002B10F4 File Offset: 0x002B04F4
		public override XmlNodeOrder ComparePosition(XPathNavigator other)
		{
			if (other == null)
			{
				return XmlNodeOrder.Unknown;
			}
			DataDocumentXPathNavigator dataDocumentXPathNavigator = other as DataDocumentXPathNavigator;
			if (dataDocumentXPathNavigator == null || dataDocumentXPathNavigator.Document != this._doc)
			{
				return XmlNodeOrder.Unknown;
			}
			return this._curNode.ComparePosition(dataDocumentXPathNavigator.CurNode);
		}

		// Token: 0x04001D89 RID: 7561
		private XPathNodePointer _curNode;

		// Token: 0x04001D8A RID: 7562
		private XmlDataDocument _doc;

		// Token: 0x04001D8B RID: 7563
		private XPathNodePointer _temp;
	}
}
