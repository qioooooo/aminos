using System;
using System.Collections;
using System.Text;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x02000088 RID: 136
	internal class CanonicalXmlElement : XmlElement, ICanonicalizableNode
	{
		// Token: 0x06000263 RID: 611 RVA: 0x0000DC5B File Offset: 0x0000CC5B
		public CanonicalXmlElement(string prefix, string localName, string namespaceURI, XmlDocument doc, bool defaultNodeSetInclusionState)
			: base(prefix, localName, namespaceURI, doc)
		{
			this.m_isInNodeSet = defaultNodeSetInclusionState;
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000264 RID: 612 RVA: 0x0000DC70 File Offset: 0x0000CC70
		// (set) Token: 0x06000265 RID: 613 RVA: 0x0000DC78 File Offset: 0x0000CC78
		public bool IsInNodeSet
		{
			get
			{
				return this.m_isInNodeSet;
			}
			set
			{
				this.m_isInNodeSet = value;
			}
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000DC84 File Offset: 0x0000CC84
		public void Write(StringBuilder strBuilder, DocPosition docPos, AncestralNamespaceContextManager anc)
		{
			Hashtable hashtable = new Hashtable();
			SortedList sortedList = new SortedList(new NamespaceSortOrder());
			SortedList sortedList2 = new SortedList(new AttributeSortOrder());
			XmlAttributeCollection attributes = this.Attributes;
			if (attributes != null)
			{
				foreach (object obj in attributes)
				{
					XmlAttribute xmlAttribute = (XmlAttribute)obj;
					if (((CanonicalXmlAttribute)xmlAttribute).IsInNodeSet || Utils.IsNamespaceNode(xmlAttribute) || Utils.IsXmlNamespaceNode(xmlAttribute))
					{
						if (Utils.IsNamespaceNode(xmlAttribute))
						{
							anc.TrackNamespaceNode(xmlAttribute, sortedList, hashtable);
						}
						else if (Utils.IsXmlNamespaceNode(xmlAttribute))
						{
							anc.TrackXmlNamespaceNode(xmlAttribute, sortedList, sortedList2, hashtable);
						}
						else if (this.IsInNodeSet)
						{
							sortedList2.Add(xmlAttribute, null);
						}
					}
				}
			}
			if (!Utils.IsCommittedNamespace(this, this.Prefix, this.NamespaceURI))
			{
				string text = ((this.Prefix.Length > 0) ? ("xmlns:" + this.Prefix) : "xmlns");
				XmlAttribute xmlAttribute2 = this.OwnerDocument.CreateAttribute(text);
				xmlAttribute2.Value = this.NamespaceURI;
				anc.TrackNamespaceNode(xmlAttribute2, sortedList, hashtable);
			}
			if (this.IsInNodeSet)
			{
				anc.GetNamespacesToRender(this, sortedList2, sortedList, hashtable);
				strBuilder.Append("<" + this.Name);
				foreach (object obj2 in sortedList.GetKeyList())
				{
					(obj2 as CanonicalXmlAttribute).Write(strBuilder, docPos, anc);
				}
				foreach (object obj3 in sortedList2.GetKeyList())
				{
					(obj3 as CanonicalXmlAttribute).Write(strBuilder, docPos, anc);
				}
				strBuilder.Append(">");
			}
			anc.EnterElementContext();
			anc.LoadUnrenderedNamespaces(hashtable);
			anc.LoadRenderedNamespaces(sortedList);
			XmlNodeList childNodes = this.ChildNodes;
			foreach (object obj4 in childNodes)
			{
				XmlNode xmlNode = (XmlNode)obj4;
				CanonicalizationDispatcher.Write(xmlNode, strBuilder, docPos, anc);
			}
			anc.ExitElementContext();
			if (this.IsInNodeSet)
			{
				strBuilder.Append("</" + this.Name + ">");
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000DF34 File Offset: 0x0000CF34
		public void WriteHash(HashAlgorithm hash, DocPosition docPos, AncestralNamespaceContextManager anc)
		{
			Hashtable hashtable = new Hashtable();
			SortedList sortedList = new SortedList(new NamespaceSortOrder());
			SortedList sortedList2 = new SortedList(new AttributeSortOrder());
			UTF8Encoding utf8Encoding = new UTF8Encoding(false);
			XmlAttributeCollection attributes = this.Attributes;
			if (attributes != null)
			{
				foreach (object obj in attributes)
				{
					XmlAttribute xmlAttribute = (XmlAttribute)obj;
					if (((CanonicalXmlAttribute)xmlAttribute).IsInNodeSet || Utils.IsNamespaceNode(xmlAttribute) || Utils.IsXmlNamespaceNode(xmlAttribute))
					{
						if (Utils.IsNamespaceNode(xmlAttribute))
						{
							anc.TrackNamespaceNode(xmlAttribute, sortedList, hashtable);
						}
						else if (Utils.IsXmlNamespaceNode(xmlAttribute))
						{
							anc.TrackXmlNamespaceNode(xmlAttribute, sortedList, sortedList2, hashtable);
						}
						else if (this.IsInNodeSet)
						{
							sortedList2.Add(xmlAttribute, null);
						}
					}
				}
			}
			if (!Utils.IsCommittedNamespace(this, this.Prefix, this.NamespaceURI))
			{
				string text = ((this.Prefix.Length > 0) ? ("xmlns:" + this.Prefix) : "xmlns");
				XmlAttribute xmlAttribute2 = this.OwnerDocument.CreateAttribute(text);
				xmlAttribute2.Value = this.NamespaceURI;
				anc.TrackNamespaceNode(xmlAttribute2, sortedList, hashtable);
			}
			if (this.IsInNodeSet)
			{
				anc.GetNamespacesToRender(this, sortedList2, sortedList, hashtable);
				byte[] array = utf8Encoding.GetBytes("<" + this.Name);
				hash.TransformBlock(array, 0, array.Length, array, 0);
				foreach (object obj2 in sortedList.GetKeyList())
				{
					(obj2 as CanonicalXmlAttribute).WriteHash(hash, docPos, anc);
				}
				foreach (object obj3 in sortedList2.GetKeyList())
				{
					(obj3 as CanonicalXmlAttribute).WriteHash(hash, docPos, anc);
				}
				array = utf8Encoding.GetBytes(">");
				hash.TransformBlock(array, 0, array.Length, array, 0);
			}
			anc.EnterElementContext();
			anc.LoadUnrenderedNamespaces(hashtable);
			anc.LoadRenderedNamespaces(sortedList);
			XmlNodeList childNodes = this.ChildNodes;
			foreach (object obj4 in childNodes)
			{
				XmlNode xmlNode = (XmlNode)obj4;
				CanonicalizationDispatcher.WriteHash(xmlNode, hash, docPos, anc);
			}
			anc.ExitElementContext();
			if (this.IsInNodeSet)
			{
				byte[] array = utf8Encoding.GetBytes("</" + this.Name + ">");
				hash.TransformBlock(array, 0, array.Length, array, 0);
			}
		}

		// Token: 0x040004DC RID: 1244
		private bool m_isInNodeSet;
	}
}
