using System;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml
{
	// Token: 0x020000D9 RID: 217
	public class XmlElement : XmlLinkedNode
	{
		// Token: 0x06000D43 RID: 3395 RVA: 0x0003B450 File Offset: 0x0003A450
		internal XmlElement(XmlName name, bool empty, XmlDocument doc)
			: base(doc)
		{
			this.parentNode = null;
			if (!doc.IsLoading)
			{
				XmlDocument.CheckName(name.Prefix);
				XmlDocument.CheckName(name.LocalName);
			}
			if (name.LocalName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("Xdom_Empty_LocalName"));
			}
			if (name.Prefix.Length >= 3 && !doc.IsLoading && string.Compare(name.Prefix, 0, "xml", 0, 3, StringComparison.OrdinalIgnoreCase) == 0)
			{
				throw new ArgumentException(Res.GetString("Xdom_Ele_Prefix"));
			}
			this.name = name;
			if (empty)
			{
				this.lastChild = this;
			}
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x0003B4F3 File Offset: 0x0003A4F3
		protected internal XmlElement(string prefix, string localName, string namespaceURI, XmlDocument doc)
			: this(doc.AddXmlName(prefix, localName, namespaceURI, null), true, doc)
		{
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000D45 RID: 3397 RVA: 0x0003B509 File Offset: 0x0003A509
		// (set) Token: 0x06000D46 RID: 3398 RVA: 0x0003B511 File Offset: 0x0003A511
		internal XmlName XmlName
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x0003B51C File Offset: 0x0003A51C
		public override XmlNode CloneNode(bool deep)
		{
			XmlDocument ownerDocument = this.OwnerDocument;
			bool isLoading = ownerDocument.IsLoading;
			ownerDocument.IsLoading = true;
			XmlElement xmlElement = ownerDocument.CreateElement(this.Prefix, this.LocalName, this.NamespaceURI);
			ownerDocument.IsLoading = isLoading;
			if (xmlElement.IsEmpty != this.IsEmpty)
			{
				xmlElement.IsEmpty = this.IsEmpty;
			}
			if (this.HasAttributes)
			{
				foreach (object obj in this.Attributes)
				{
					XmlAttribute xmlAttribute = (XmlAttribute)obj;
					XmlAttribute xmlAttribute2 = (XmlAttribute)xmlAttribute.CloneNode(true);
					if (xmlAttribute is XmlUnspecifiedAttribute && !xmlAttribute.Specified)
					{
						((XmlUnspecifiedAttribute)xmlAttribute2).SetSpecified(false);
					}
					xmlElement.Attributes.InternalAppendAttribute(xmlAttribute2);
				}
			}
			if (deep)
			{
				xmlElement.CopyChildren(ownerDocument, this, deep);
			}
			return xmlElement;
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000D48 RID: 3400 RVA: 0x0003B614 File Offset: 0x0003A614
		public override string Name
		{
			get
			{
				return this.name.Name;
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000D49 RID: 3401 RVA: 0x0003B621 File Offset: 0x0003A621
		public override string LocalName
		{
			get
			{
				return this.name.LocalName;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000D4A RID: 3402 RVA: 0x0003B62E File Offset: 0x0003A62E
		public override string NamespaceURI
		{
			get
			{
				return this.name.NamespaceURI;
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000D4B RID: 3403 RVA: 0x0003B63B File Offset: 0x0003A63B
		// (set) Token: 0x06000D4C RID: 3404 RVA: 0x0003B648 File Offset: 0x0003A648
		public override string Prefix
		{
			get
			{
				return this.name.Prefix;
			}
			set
			{
				this.name = this.name.OwnerDocument.AddXmlName(value, this.LocalName, this.NamespaceURI, this.SchemaInfo);
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000D4D RID: 3405 RVA: 0x0003B673 File Offset: 0x0003A673
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.Element;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000D4E RID: 3406 RVA: 0x0003B676 File Offset: 0x0003A676
		public override XmlNode ParentNode
		{
			get
			{
				return this.parentNode;
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000D4F RID: 3407 RVA: 0x0003B67E File Offset: 0x0003A67E
		public override XmlDocument OwnerDocument
		{
			get
			{
				return this.name.OwnerDocument;
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000D50 RID: 3408 RVA: 0x0003B68B File Offset: 0x0003A68B
		internal override bool IsContainer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x0003B690 File Offset: 0x0003A690
		internal override XmlNode AppendChildForLoad(XmlNode newChild, XmlDocument doc)
		{
			XmlNodeChangedEventArgs insertEventArgsForLoad = doc.GetInsertEventArgsForLoad(newChild, this);
			if (insertEventArgsForLoad != null)
			{
				doc.BeforeEvent(insertEventArgsForLoad);
			}
			XmlLinkedNode xmlLinkedNode = (XmlLinkedNode)newChild;
			if (this.lastChild == null || this.lastChild == this)
			{
				xmlLinkedNode.next = xmlLinkedNode;
				this.lastChild = xmlLinkedNode;
				xmlLinkedNode.SetParentForLoad(this);
			}
			else
			{
				XmlLinkedNode xmlLinkedNode2 = this.lastChild;
				xmlLinkedNode.next = xmlLinkedNode2.next;
				xmlLinkedNode2.next = xmlLinkedNode;
				this.lastChild = xmlLinkedNode;
				if (xmlLinkedNode2.IsText && xmlLinkedNode.IsText)
				{
					XmlNode.NestTextNodes(xmlLinkedNode2, xmlLinkedNode);
				}
				else
				{
					xmlLinkedNode.SetParentForLoad(this);
				}
			}
			if (insertEventArgsForLoad != null)
			{
				doc.AfterEvent(insertEventArgsForLoad);
			}
			return xmlLinkedNode;
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000D52 RID: 3410 RVA: 0x0003B72B File Offset: 0x0003A72B
		// (set) Token: 0x06000D53 RID: 3411 RVA: 0x0003B736 File Offset: 0x0003A736
		public bool IsEmpty
		{
			get
			{
				return this.lastChild == this;
			}
			set
			{
				if (value)
				{
					if (this.lastChild != this)
					{
						this.RemoveAllChildren();
						this.lastChild = this;
						return;
					}
				}
				else if (this.lastChild == this)
				{
					this.lastChild = null;
				}
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000D54 RID: 3412 RVA: 0x0003B762 File Offset: 0x0003A762
		// (set) Token: 0x06000D55 RID: 3413 RVA: 0x0003B775 File Offset: 0x0003A775
		internal override XmlLinkedNode LastNode
		{
			get
			{
				if (this.lastChild != this)
				{
					return this.lastChild;
				}
				return null;
			}
			set
			{
				this.lastChild = value;
			}
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x0003B780 File Offset: 0x0003A780
		internal override bool IsValidChildType(XmlNodeType type)
		{
			switch (type)
			{
			case XmlNodeType.Element:
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
			case XmlNodeType.EntityReference:
			case XmlNodeType.ProcessingInstruction:
			case XmlNodeType.Comment:
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
				return true;
			}
			return false;
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000D57 RID: 3415 RVA: 0x0003B7D4 File Offset: 0x0003A7D4
		public override XmlAttributeCollection Attributes
		{
			get
			{
				if (this.attributes == null)
				{
					lock (this.OwnerDocument.objLock)
					{
						if (this.attributes == null)
						{
							this.attributes = new XmlAttributeCollection(this);
						}
					}
				}
				return this.attributes;
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000D58 RID: 3416 RVA: 0x0003B830 File Offset: 0x0003A830
		public virtual bool HasAttributes
		{
			get
			{
				return this.attributes != null && this.attributes.Count > 0;
			}
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x0003B84C File Offset: 0x0003A84C
		public virtual string GetAttribute(string name)
		{
			XmlAttribute attributeNode = this.GetAttributeNode(name);
			if (attributeNode != null)
			{
				return attributeNode.Value;
			}
			return string.Empty;
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x0003B870 File Offset: 0x0003A870
		public virtual void SetAttribute(string name, string value)
		{
			XmlAttribute xmlAttribute = this.GetAttributeNode(name);
			if (xmlAttribute == null)
			{
				xmlAttribute = this.OwnerDocument.CreateAttribute(name);
				xmlAttribute.Value = value;
				this.Attributes.InternalAppendAttribute(xmlAttribute);
				return;
			}
			xmlAttribute.Value = value;
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x0003B8B1 File Offset: 0x0003A8B1
		public virtual void RemoveAttribute(string name)
		{
			if (this.HasAttributes)
			{
				this.Attributes.RemoveNamedItem(name);
			}
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x0003B8C8 File Offset: 0x0003A8C8
		public virtual XmlAttribute GetAttributeNode(string name)
		{
			if (this.HasAttributes)
			{
				return this.Attributes[name];
			}
			return null;
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x0003B8E0 File Offset: 0x0003A8E0
		public virtual XmlAttribute SetAttributeNode(XmlAttribute newAttr)
		{
			if (newAttr.OwnerElement != null)
			{
				throw new InvalidOperationException(Res.GetString("Xdom_Attr_InUse"));
			}
			return (XmlAttribute)this.Attributes.SetNamedItem(newAttr);
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x0003B90B File Offset: 0x0003A90B
		public virtual XmlAttribute RemoveAttributeNode(XmlAttribute oldAttr)
		{
			if (this.HasAttributes)
			{
				return this.Attributes.Remove(oldAttr);
			}
			return null;
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x0003B923 File Offset: 0x0003A923
		public virtual XmlNodeList GetElementsByTagName(string name)
		{
			return new XmlElementList(this, name);
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x0003B92C File Offset: 0x0003A92C
		public virtual string GetAttribute(string localName, string namespaceURI)
		{
			XmlAttribute attributeNode = this.GetAttributeNode(localName, namespaceURI);
			if (attributeNode != null)
			{
				return attributeNode.Value;
			}
			return string.Empty;
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x0003B954 File Offset: 0x0003A954
		public virtual string SetAttribute(string localName, string namespaceURI, string value)
		{
			XmlAttribute xmlAttribute = this.GetAttributeNode(localName, namespaceURI);
			if (xmlAttribute == null)
			{
				xmlAttribute = this.OwnerDocument.CreateAttribute(string.Empty, localName, namespaceURI);
				xmlAttribute.Value = value;
				this.Attributes.InternalAppendAttribute(xmlAttribute);
			}
			else
			{
				xmlAttribute.Value = value;
			}
			return value;
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x0003B99E File Offset: 0x0003A99E
		public virtual void RemoveAttribute(string localName, string namespaceURI)
		{
			this.RemoveAttributeNode(localName, namespaceURI);
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x0003B9A9 File Offset: 0x0003A9A9
		public virtual XmlAttribute GetAttributeNode(string localName, string namespaceURI)
		{
			if (this.HasAttributes)
			{
				return this.Attributes[localName, namespaceURI];
			}
			return null;
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x0003B9C4 File Offset: 0x0003A9C4
		public virtual XmlAttribute SetAttributeNode(string localName, string namespaceURI)
		{
			XmlAttribute xmlAttribute = this.GetAttributeNode(localName, namespaceURI);
			if (xmlAttribute == null)
			{
				xmlAttribute = this.OwnerDocument.CreateAttribute(string.Empty, localName, namespaceURI);
				this.Attributes.InternalAppendAttribute(xmlAttribute);
			}
			return xmlAttribute;
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x0003BA00 File Offset: 0x0003AA00
		public virtual XmlAttribute RemoveAttributeNode(string localName, string namespaceURI)
		{
			if (this.HasAttributes)
			{
				XmlAttribute attributeNode = this.GetAttributeNode(localName, namespaceURI);
				this.Attributes.Remove(attributeNode);
				return attributeNode;
			}
			return null;
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x0003BA2E File Offset: 0x0003AA2E
		public virtual XmlNodeList GetElementsByTagName(string localName, string namespaceURI)
		{
			return new XmlElementList(this, localName, namespaceURI);
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x0003BA38 File Offset: 0x0003AA38
		public virtual bool HasAttribute(string name)
		{
			return this.GetAttributeNode(name) != null;
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x0003BA47 File Offset: 0x0003AA47
		public virtual bool HasAttribute(string localName, string namespaceURI)
		{
			return this.GetAttributeNode(localName, namespaceURI) != null;
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x0003BA57 File Offset: 0x0003AA57
		public override void WriteTo(XmlWriter w)
		{
			if (base.GetType() == typeof(XmlElement))
			{
				XmlElement.WriteElementTo(w, this);
				return;
			}
			this.WriteStartElement(w);
			if (this.IsEmpty)
			{
				w.WriteEndElement();
				return;
			}
			this.WriteContentTo(w);
			w.WriteFullEndElement();
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x0003BA98 File Offset: 0x0003AA98
		private static void WriteElementTo(XmlWriter writer, XmlElement e)
		{
			XmlNode xmlNode = e;
			XmlNode xmlNode2 = e;
			for (;;)
			{
				e = xmlNode2 as XmlElement;
				if (e != null && e.GetType() == typeof(XmlElement))
				{
					e.WriteStartElement(writer);
					if (e.IsEmpty)
					{
						writer.WriteEndElement();
					}
					else
					{
						if (e.lastChild != null)
						{
							xmlNode2 = e.FirstChild;
							continue;
						}
						writer.WriteFullEndElement();
					}
				}
				else
				{
					xmlNode2.WriteTo(writer);
				}
				while (xmlNode2 != xmlNode && xmlNode2 == xmlNode2.ParentNode.LastChild)
				{
					xmlNode2 = xmlNode2.ParentNode;
					writer.WriteFullEndElement();
				}
				if (xmlNode2 == xmlNode)
				{
					break;
				}
				xmlNode2 = xmlNode2.NextSibling;
			}
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x0003BB30 File Offset: 0x0003AB30
		private void WriteStartElement(XmlWriter w)
		{
			w.WriteStartElement(this.Prefix, this.LocalName, this.NamespaceURI);
			if (this.HasAttributes)
			{
				XmlAttributeCollection xmlAttributeCollection = this.Attributes;
				for (int i = 0; i < xmlAttributeCollection.Count; i++)
				{
					XmlAttribute xmlAttribute = xmlAttributeCollection[i];
					xmlAttribute.WriteTo(w);
				}
			}
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x0003BB84 File Offset: 0x0003AB84
		public override void WriteContentTo(XmlWriter w)
		{
			for (XmlNode xmlNode = this.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				xmlNode.WriteTo(w);
			}
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x0003BBAB File Offset: 0x0003ABAB
		public virtual XmlNode RemoveAttributeAt(int i)
		{
			if (this.HasAttributes)
			{
				return this.attributes.RemoveAt(i);
			}
			return null;
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x0003BBC3 File Offset: 0x0003ABC3
		public virtual void RemoveAllAttributes()
		{
			if (this.HasAttributes)
			{
				this.attributes.RemoveAll();
			}
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x0003BBD8 File Offset: 0x0003ABD8
		public override void RemoveAll()
		{
			base.RemoveAll();
			this.RemoveAllAttributes();
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x0003BBE6 File Offset: 0x0003ABE6
		internal void RemoveAllChildren()
		{
			base.RemoveAll();
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000D71 RID: 3441 RVA: 0x0003BBEE File Offset: 0x0003ABEE
		public override IXmlSchemaInfo SchemaInfo
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000D72 RID: 3442 RVA: 0x0003BBF6 File Offset: 0x0003ABF6
		// (set) Token: 0x06000D73 RID: 3443 RVA: 0x0003BC00 File Offset: 0x0003AC00
		public override string InnerXml
		{
			get
			{
				return base.InnerXml;
			}
			set
			{
				this.RemoveAllChildren();
				XmlLoader xmlLoader = new XmlLoader();
				xmlLoader.LoadInnerXmlElement(this, value);
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000D74 RID: 3444 RVA: 0x0003BC21 File Offset: 0x0003AC21
		// (set) Token: 0x06000D75 RID: 3445 RVA: 0x0003BC2C File Offset: 0x0003AC2C
		public override string InnerText
		{
			get
			{
				return base.InnerText;
			}
			set
			{
				XmlLinkedNode lastNode = this.LastNode;
				if (lastNode != null && lastNode.NodeType == XmlNodeType.Text && lastNode.next == lastNode)
				{
					lastNode.Value = value;
					return;
				}
				this.RemoveAllChildren();
				this.AppendChild(this.OwnerDocument.CreateTextNode(value));
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000D76 RID: 3446 RVA: 0x0003BC76 File Offset: 0x0003AC76
		public override XmlNode NextSibling
		{
			get
			{
				if (this.parentNode != null && this.parentNode.LastNode != this)
				{
					return this.next;
				}
				return null;
			}
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x0003BC96 File Offset: 0x0003AC96
		internal override void SetParent(XmlNode node)
		{
			this.parentNode = node;
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000D78 RID: 3448 RVA: 0x0003BC9F File Offset: 0x0003AC9F
		internal override XPathNodeType XPNodeType
		{
			get
			{
				return XPathNodeType.Element;
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000D79 RID: 3449 RVA: 0x0003BCA2 File Offset: 0x0003ACA2
		internal override string XPLocalName
		{
			get
			{
				return this.LocalName;
			}
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x0003BCAC File Offset: 0x0003ACAC
		internal override string GetXPAttribute(string localName, string ns)
		{
			if (ns == this.OwnerDocument.strReservedXmlns)
			{
				return null;
			}
			XmlAttribute attributeNode = this.GetAttributeNode(localName, ns);
			if (attributeNode != null)
			{
				return attributeNode.Value;
			}
			return string.Empty;
		}

		// Token: 0x04000946 RID: 2374
		private XmlName name;

		// Token: 0x04000947 RID: 2375
		private XmlAttributeCollection attributes;

		// Token: 0x04000948 RID: 2376
		private XmlLinkedNode lastChild;
	}
}
