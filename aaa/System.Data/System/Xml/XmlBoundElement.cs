using System;
using System.Data;
using System.Threading;

namespace System.Xml
{
	// Token: 0x0200038B RID: 907
	internal sealed class XmlBoundElement : XmlElement
	{
		// Token: 0x06002FF6 RID: 12278 RVA: 0x002B27CC File Offset: 0x002B1BCC
		internal XmlBoundElement(string prefix, string localName, string namespaceURI, XmlDocument doc)
			: base(prefix, localName, namespaceURI, doc)
		{
			this.state = ElementState.None;
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x06002FF7 RID: 12279 RVA: 0x002B27EC File Offset: 0x002B1BEC
		public override XmlAttributeCollection Attributes
		{
			get
			{
				this.AutoFoliate();
				return base.Attributes;
			}
		}

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x06002FF8 RID: 12280 RVA: 0x002B2808 File Offset: 0x002B1C08
		public override bool HasAttributes
		{
			get
			{
				return this.Attributes.Count > 0;
			}
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x06002FF9 RID: 12281 RVA: 0x002B2824 File Offset: 0x002B1C24
		public override XmlNode FirstChild
		{
			get
			{
				this.AutoFoliate();
				return base.FirstChild;
			}
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06002FFA RID: 12282 RVA: 0x002B2840 File Offset: 0x002B1C40
		internal XmlNode SafeFirstChild
		{
			get
			{
				return base.FirstChild;
			}
		}

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06002FFB RID: 12283 RVA: 0x002B2854 File Offset: 0x002B1C54
		public override XmlNode LastChild
		{
			get
			{
				this.AutoFoliate();
				return base.LastChild;
			}
		}

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06002FFC RID: 12284 RVA: 0x002B2870 File Offset: 0x002B1C70
		public override XmlNode PreviousSibling
		{
			get
			{
				XmlNode previousSibling = base.PreviousSibling;
				if (previousSibling == null)
				{
					XmlBoundElement xmlBoundElement = this.ParentNode as XmlBoundElement;
					if (xmlBoundElement != null)
					{
						xmlBoundElement.AutoFoliate();
						return base.PreviousSibling;
					}
				}
				return previousSibling;
			}
		}

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x06002FFD RID: 12285 RVA: 0x002B28A4 File Offset: 0x002B1CA4
		internal XmlNode SafePreviousSibling
		{
			get
			{
				return base.PreviousSibling;
			}
		}

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x06002FFE RID: 12286 RVA: 0x002B28B8 File Offset: 0x002B1CB8
		public override XmlNode NextSibling
		{
			get
			{
				XmlNode nextSibling = base.NextSibling;
				if (nextSibling == null)
				{
					XmlBoundElement xmlBoundElement = this.ParentNode as XmlBoundElement;
					if (xmlBoundElement != null)
					{
						xmlBoundElement.AutoFoliate();
						return base.NextSibling;
					}
				}
				return nextSibling;
			}
		}

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x06002FFF RID: 12287 RVA: 0x002B28EC File Offset: 0x002B1CEC
		internal XmlNode SafeNextSibling
		{
			get
			{
				return base.NextSibling;
			}
		}

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x06003000 RID: 12288 RVA: 0x002B2900 File Offset: 0x002B1D00
		public override bool HasChildNodes
		{
			get
			{
				this.AutoFoliate();
				return base.HasChildNodes;
			}
		}

		// Token: 0x06003001 RID: 12289 RVA: 0x002B291C File Offset: 0x002B1D1C
		public override XmlNode InsertBefore(XmlNode newChild, XmlNode refChild)
		{
			this.AutoFoliate();
			return base.InsertBefore(newChild, refChild);
		}

		// Token: 0x06003002 RID: 12290 RVA: 0x002B2938 File Offset: 0x002B1D38
		public override XmlNode InsertAfter(XmlNode newChild, XmlNode refChild)
		{
			this.AutoFoliate();
			return base.InsertAfter(newChild, refChild);
		}

		// Token: 0x06003003 RID: 12291 RVA: 0x002B2954 File Offset: 0x002B1D54
		public override XmlNode ReplaceChild(XmlNode newChild, XmlNode oldChild)
		{
			this.AutoFoliate();
			return base.ReplaceChild(newChild, oldChild);
		}

		// Token: 0x06003004 RID: 12292 RVA: 0x002B2970 File Offset: 0x002B1D70
		public override XmlNode AppendChild(XmlNode newChild)
		{
			this.AutoFoliate();
			return base.AppendChild(newChild);
		}

		// Token: 0x06003005 RID: 12293 RVA: 0x002B298C File Offset: 0x002B1D8C
		internal void RemoveAllChildren()
		{
			XmlNode nextSibling;
			for (XmlNode xmlNode = this.FirstChild; xmlNode != null; xmlNode = nextSibling)
			{
				nextSibling = xmlNode.NextSibling;
				this.RemoveChild(xmlNode);
			}
		}

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x06003006 RID: 12294 RVA: 0x002B29B8 File Offset: 0x002B1DB8
		// (set) Token: 0x06003007 RID: 12295 RVA: 0x002B29CC File Offset: 0x002B1DCC
		public override string InnerXml
		{
			get
			{
				return base.InnerXml;
			}
			set
			{
				this.RemoveAllChildren();
				XmlDataDocument xmlDataDocument = (XmlDataDocument)this.OwnerDocument;
				bool ignoreXmlEvents = xmlDataDocument.IgnoreXmlEvents;
				bool ignoreDataSetEvents = xmlDataDocument.IgnoreDataSetEvents;
				xmlDataDocument.IgnoreXmlEvents = true;
				xmlDataDocument.IgnoreDataSetEvents = true;
				base.InnerXml = value;
				xmlDataDocument.SyncTree(this);
				xmlDataDocument.IgnoreDataSetEvents = ignoreDataSetEvents;
				xmlDataDocument.IgnoreXmlEvents = ignoreXmlEvents;
			}
		}

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x06003008 RID: 12296 RVA: 0x002B2A24 File Offset: 0x002B1E24
		// (set) Token: 0x06003009 RID: 12297 RVA: 0x002B2A38 File Offset: 0x002B1E38
		internal DataRow Row
		{
			get
			{
				return this.row;
			}
			set
			{
				this.row = value;
			}
		}

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x0600300A RID: 12298 RVA: 0x002B2A4C File Offset: 0x002B1E4C
		internal bool IsFoliated
		{
			get
			{
				while (this.state == ElementState.Foliating || this.state == ElementState.Defoliating)
				{
					Thread.Sleep(0);
				}
				return this.state != ElementState.Defoliated;
			}
		}

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x0600300B RID: 12299 RVA: 0x002B2A80 File Offset: 0x002B1E80
		// (set) Token: 0x0600300C RID: 12300 RVA: 0x002B2A94 File Offset: 0x002B1E94
		internal ElementState ElementState
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
			}
		}

		// Token: 0x0600300D RID: 12301 RVA: 0x002B2AA8 File Offset: 0x002B1EA8
		internal void Foliate(ElementState newState)
		{
			XmlDataDocument xmlDataDocument = (XmlDataDocument)this.OwnerDocument;
			if (xmlDataDocument != null)
			{
				xmlDataDocument.Foliate(this, newState);
			}
		}

		// Token: 0x0600300E RID: 12302 RVA: 0x002B2ACC File Offset: 0x002B1ECC
		private void AutoFoliate()
		{
			XmlDataDocument xmlDataDocument = (XmlDataDocument)this.OwnerDocument;
			if (xmlDataDocument != null)
			{
				xmlDataDocument.Foliate(this, xmlDataDocument.AutoFoliationState);
			}
		}

		// Token: 0x0600300F RID: 12303 RVA: 0x002B2AF8 File Offset: 0x002B1EF8
		public override XmlNode CloneNode(bool deep)
		{
			XmlDataDocument xmlDataDocument = (XmlDataDocument)this.OwnerDocument;
			ElementState autoFoliationState = xmlDataDocument.AutoFoliationState;
			xmlDataDocument.AutoFoliationState = ElementState.WeakFoliation;
			XmlElement xmlElement;
			try
			{
				this.Foliate(ElementState.WeakFoliation);
				xmlElement = (XmlElement)base.CloneNode(deep);
			}
			finally
			{
				xmlDataDocument.AutoFoliationState = autoFoliationState;
			}
			return xmlElement;
		}

		// Token: 0x06003010 RID: 12304 RVA: 0x002B2B5C File Offset: 0x002B1F5C
		public override void WriteContentTo(XmlWriter w)
		{
			DataPointer dataPointer = new DataPointer((XmlDataDocument)this.OwnerDocument, this);
			try
			{
				dataPointer.AddPointer();
				XmlBoundElement.WriteBoundElementContentTo(dataPointer, w);
			}
			finally
			{
				dataPointer.SetNoLongerUse();
			}
		}

		// Token: 0x06003011 RID: 12305 RVA: 0x002B2BB0 File Offset: 0x002B1FB0
		public override void WriteTo(XmlWriter w)
		{
			DataPointer dataPointer = new DataPointer((XmlDataDocument)this.OwnerDocument, this);
			try
			{
				dataPointer.AddPointer();
				this.WriteRootBoundElementTo(dataPointer, w);
			}
			finally
			{
				dataPointer.SetNoLongerUse();
			}
		}

		// Token: 0x06003012 RID: 12306 RVA: 0x002B2C04 File Offset: 0x002B2004
		private void WriteRootBoundElementTo(DataPointer dp, XmlWriter w)
		{
			XmlDataDocument xmlDataDocument = (XmlDataDocument)this.OwnerDocument;
			w.WriteStartElement(dp.Prefix, dp.LocalName, dp.NamespaceURI);
			int attributeCount = dp.AttributeCount;
			bool flag = false;
			if (attributeCount > 0)
			{
				for (int i = 0; i < attributeCount; i++)
				{
					dp.MoveToAttribute(i);
					if (dp.Prefix == "xmlns" && dp.LocalName == "xsi")
					{
						flag = true;
					}
					XmlBoundElement.WriteTo(dp, w);
					dp.MoveToOwnerElement();
				}
			}
			if (!flag && xmlDataDocument.bLoadFromDataSet && xmlDataDocument.bHasXSINIL)
			{
				w.WriteAttributeString("xmlns", "xsi", "http://www.w3.org/2000/xmlns/", "http://www.w3.org/2001/XMLSchema-instance");
			}
			XmlBoundElement.WriteBoundElementContentTo(dp, w);
			if (dp.IsEmptyElement)
			{
				w.WriteEndElement();
				return;
			}
			w.WriteFullEndElement();
		}

		// Token: 0x06003013 RID: 12307 RVA: 0x002B2CD4 File Offset: 0x002B20D4
		private static void WriteBoundElementTo(DataPointer dp, XmlWriter w)
		{
			w.WriteStartElement(dp.Prefix, dp.LocalName, dp.NamespaceURI);
			int attributeCount = dp.AttributeCount;
			if (attributeCount > 0)
			{
				for (int i = 0; i < attributeCount; i++)
				{
					dp.MoveToAttribute(i);
					XmlBoundElement.WriteTo(dp, w);
					dp.MoveToOwnerElement();
				}
			}
			XmlBoundElement.WriteBoundElementContentTo(dp, w);
			if (dp.IsEmptyElement)
			{
				w.WriteEndElement();
				return;
			}
			w.WriteFullEndElement();
		}

		// Token: 0x06003014 RID: 12308 RVA: 0x002B2D44 File Offset: 0x002B2144
		private static void WriteBoundElementContentTo(DataPointer dp, XmlWriter w)
		{
			if (!dp.IsEmptyElement && dp.MoveToFirstChild())
			{
				do
				{
					XmlBoundElement.WriteTo(dp, w);
				}
				while (dp.MoveToNextSibling());
				dp.MoveToParent();
			}
		}

		// Token: 0x06003015 RID: 12309 RVA: 0x002B2D78 File Offset: 0x002B2178
		private static void WriteTo(DataPointer dp, XmlWriter w)
		{
			switch (dp.NodeType)
			{
			case XmlNodeType.Element:
				XmlBoundElement.WriteBoundElementTo(dp, w);
				return;
			case XmlNodeType.Attribute:
				if (!dp.IsDefault)
				{
					w.WriteStartAttribute(dp.Prefix, dp.LocalName, dp.NamespaceURI);
					if (dp.MoveToFirstChild())
					{
						do
						{
							XmlBoundElement.WriteTo(dp, w);
						}
						while (dp.MoveToNextSibling());
						dp.MoveToParent();
					}
					w.WriteEndAttribute();
					return;
				}
				break;
			case XmlNodeType.Text:
				w.WriteString(dp.Value);
				return;
			default:
				if (dp.GetNode() != null)
				{
					dp.GetNode().WriteTo(w);
				}
				break;
			}
		}

		// Token: 0x06003016 RID: 12310 RVA: 0x002B2E10 File Offset: 0x002B2210
		public override XmlNodeList GetElementsByTagName(string name)
		{
			XmlNodeList elementsByTagName = base.GetElementsByTagName(name);
			int count = elementsByTagName.Count;
			return elementsByTagName;
		}

		// Token: 0x04001DA2 RID: 7586
		private DataRow row;

		// Token: 0x04001DA3 RID: 7587
		private ElementState state;
	}
}
