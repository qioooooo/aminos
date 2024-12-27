using System;
using System.IO;
using System.Text;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x02000096 RID: 150
	internal class CanonicalXml
	{
		// Token: 0x060002BB RID: 699 RVA: 0x0000EC48 File Offset: 0x0000DC48
		internal CanonicalXml(Stream inputStream, bool includeComments, XmlResolver resolver, string strBaseUri)
		{
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			this.m_c14nDoc = new CanonicalXmlDocument(true, includeComments);
			this.m_c14nDoc.XmlResolver = resolver;
			this.m_c14nDoc.Load(Utils.PreProcessStreamInput(inputStream, resolver, strBaseUri));
			this.m_ancMgr = new C14NAncestralNamespaceContextManager();
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000ECA1 File Offset: 0x0000DCA1
		internal CanonicalXml(XmlDocument document, XmlResolver resolver)
			: this(document, resolver, false)
		{
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000ECAC File Offset: 0x0000DCAC
		internal CanonicalXml(XmlDocument document, XmlResolver resolver, bool includeComments)
		{
			if (document == null)
			{
				throw new ArgumentNullException("document");
			}
			this.m_c14nDoc = new CanonicalXmlDocument(true, includeComments);
			this.m_c14nDoc.XmlResolver = resolver;
			this.m_c14nDoc.Load(new XmlNodeReader(document));
			this.m_ancMgr = new C14NAncestralNamespaceContextManager();
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000ED04 File Offset: 0x0000DD04
		internal CanonicalXml(XmlNodeList nodeList, XmlResolver resolver, bool includeComments)
		{
			if (nodeList == null)
			{
				throw new ArgumentNullException("nodeList");
			}
			XmlDocument ownerDocument = Utils.GetOwnerDocument(nodeList);
			if (ownerDocument == null)
			{
				throw new ArgumentException("nodeList");
			}
			this.m_c14nDoc = new CanonicalXmlDocument(false, includeComments);
			this.m_c14nDoc.XmlResolver = resolver;
			this.m_c14nDoc.Load(new XmlNodeReader(ownerDocument));
			this.m_ancMgr = new C14NAncestralNamespaceContextManager();
			CanonicalXml.MarkInclusionStateForNodes(nodeList, ownerDocument, this.m_c14nDoc);
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000ED7C File Offset: 0x0000DD7C
		private static void MarkNodeAsIncluded(XmlNode node)
		{
			if (node is ICanonicalizableNode)
			{
				((ICanonicalizableNode)node).IsInNodeSet = true;
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000ED94 File Offset: 0x0000DD94
		private static void MarkInclusionStateForNodes(XmlNodeList nodeList, XmlDocument inputRoot, XmlDocument root)
		{
			CanonicalXmlNodeList canonicalXmlNodeList = new CanonicalXmlNodeList();
			CanonicalXmlNodeList canonicalXmlNodeList2 = new CanonicalXmlNodeList();
			canonicalXmlNodeList.Add(inputRoot);
			canonicalXmlNodeList2.Add(root);
			int num = 0;
			do
			{
				XmlNode xmlNode = canonicalXmlNodeList[num];
				XmlNode xmlNode2 = canonicalXmlNodeList2[num];
				XmlNodeList childNodes = xmlNode.ChildNodes;
				XmlNodeList childNodes2 = xmlNode2.ChildNodes;
				for (int i = 0; i < childNodes.Count; i++)
				{
					canonicalXmlNodeList.Add(childNodes[i]);
					canonicalXmlNodeList2.Add(childNodes2[i]);
					if (Utils.NodeInList(childNodes[i], nodeList))
					{
						CanonicalXml.MarkNodeAsIncluded(childNodes2[i]);
					}
					XmlAttributeCollection attributes = childNodes[i].Attributes;
					if (attributes != null)
					{
						for (int j = 0; j < attributes.Count; j++)
						{
							if (Utils.NodeInList(attributes[j], nodeList))
							{
								CanonicalXml.MarkNodeAsIncluded(childNodes2[i].Attributes.Item(j));
							}
						}
					}
				}
				num++;
			}
			while (num < canonicalXmlNodeList.Count);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000EEA4 File Offset: 0x0000DEA4
		internal byte[] GetBytes()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.m_c14nDoc.Write(stringBuilder, DocPosition.BeforeRootElement, this.m_ancMgr);
			UTF8Encoding utf8Encoding = new UTF8Encoding(false);
			return utf8Encoding.GetBytes(stringBuilder.ToString());
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000EEE0 File Offset: 0x0000DEE0
		internal byte[] GetDigestedBytes(HashAlgorithm hash)
		{
			this.m_c14nDoc.WriteHash(hash, DocPosition.BeforeRootElement, this.m_ancMgr);
			hash.TransformFinalBlock(new byte[0], 0, 0);
			byte[] array = (byte[])hash.Hash.Clone();
			hash.Initialize();
			return array;
		}

		// Token: 0x040004EA RID: 1258
		private CanonicalXmlDocument m_c14nDoc;

		// Token: 0x040004EB RID: 1259
		private C14NAncestralNamespaceContextManager m_ancMgr;
	}
}
