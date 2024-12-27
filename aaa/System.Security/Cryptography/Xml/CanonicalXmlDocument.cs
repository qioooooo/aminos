using System;
using System.Text;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x02000087 RID: 135
	internal class CanonicalXmlDocument : XmlDocument, ICanonicalizableNode
	{
		// Token: 0x06000254 RID: 596 RVA: 0x0000DA84 File Offset: 0x0000CA84
		public CanonicalXmlDocument(bool defaultNodeSetInclusionState, bool includeComments)
		{
			base.PreserveWhitespace = true;
			this.m_includeComments = includeComments;
			this.m_defaultNodeSetInclusionState = defaultNodeSetInclusionState;
			this.m_isInNodeSet = defaultNodeSetInclusionState;
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000255 RID: 597 RVA: 0x0000DAB5 File Offset: 0x0000CAB5
		// (set) Token: 0x06000256 RID: 598 RVA: 0x0000DABD File Offset: 0x0000CABD
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

		// Token: 0x06000257 RID: 599 RVA: 0x0000DAC8 File Offset: 0x0000CAC8
		public void Write(StringBuilder strBuilder, DocPosition docPos, AncestralNamespaceContextManager anc)
		{
			docPos = DocPosition.BeforeRootElement;
			foreach (object obj in this.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					CanonicalizationDispatcher.Write(xmlNode, strBuilder, DocPosition.InRootElement, anc);
					docPos = DocPosition.AfterRootElement;
				}
				else
				{
					CanonicalizationDispatcher.Write(xmlNode, strBuilder, docPos, anc);
				}
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000DB40 File Offset: 0x0000CB40
		public void WriteHash(HashAlgorithm hash, DocPosition docPos, AncestralNamespaceContextManager anc)
		{
			docPos = DocPosition.BeforeRootElement;
			foreach (object obj in this.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					CanonicalizationDispatcher.WriteHash(xmlNode, hash, DocPosition.InRootElement, anc);
					docPos = DocPosition.AfterRootElement;
				}
				else
				{
					CanonicalizationDispatcher.WriteHash(xmlNode, hash, docPos, anc);
				}
			}
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000DBB8 File Offset: 0x0000CBB8
		public override XmlElement CreateElement(string prefix, string localName, string namespaceURI)
		{
			return new CanonicalXmlElement(prefix, localName, namespaceURI, this, this.m_defaultNodeSetInclusionState);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000DBC9 File Offset: 0x0000CBC9
		public override XmlAttribute CreateAttribute(string prefix, string localName, string namespaceURI)
		{
			return new CanonicalXmlAttribute(prefix, localName, namespaceURI, this, this.m_defaultNodeSetInclusionState);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000DBDA File Offset: 0x0000CBDA
		protected override XmlAttribute CreateDefaultAttribute(string prefix, string localName, string namespaceURI)
		{
			return new CanonicalXmlAttribute(prefix, localName, namespaceURI, this, this.m_defaultNodeSetInclusionState);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000DBEB File Offset: 0x0000CBEB
		public override XmlText CreateTextNode(string text)
		{
			return new CanonicalXmlText(text, this, this.m_defaultNodeSetInclusionState);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000DBFA File Offset: 0x0000CBFA
		public override XmlWhitespace CreateWhitespace(string prefix)
		{
			return new CanonicalXmlWhitespace(prefix, this, this.m_defaultNodeSetInclusionState);
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000DC09 File Offset: 0x0000CC09
		public override XmlSignificantWhitespace CreateSignificantWhitespace(string text)
		{
			return new CanonicalXmlSignificantWhitespace(text, this, this.m_defaultNodeSetInclusionState);
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000DC18 File Offset: 0x0000CC18
		public override XmlProcessingInstruction CreateProcessingInstruction(string target, string data)
		{
			return new CanonicalXmlProcessingInstruction(target, data, this, this.m_defaultNodeSetInclusionState);
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000DC28 File Offset: 0x0000CC28
		public override XmlComment CreateComment(string data)
		{
			return new CanonicalXmlComment(data, this, this.m_defaultNodeSetInclusionState, this.m_includeComments);
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000DC3D File Offset: 0x0000CC3D
		public override XmlEntityReference CreateEntityReference(string name)
		{
			return new CanonicalXmlEntityReference(name, this, this.m_defaultNodeSetInclusionState);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000DC4C File Offset: 0x0000CC4C
		public override XmlCDataSection CreateCDataSection(string data)
		{
			return new CanonicalXmlCDataSection(data, this, this.m_defaultNodeSetInclusionState);
		}

		// Token: 0x040004D9 RID: 1241
		private bool m_defaultNodeSetInclusionState;

		// Token: 0x040004DA RID: 1242
		private bool m_includeComments;

		// Token: 0x040004DB RID: 1243
		private bool m_isInNodeSet;
	}
}
