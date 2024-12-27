using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Schema;
using MS.Internal.Xml.Cache;

namespace System.Xml.XPath
{
	// Token: 0x0200010F RID: 271
	public class XPathDocument : IXPathNavigable
	{
		// Token: 0x06001095 RID: 4245 RVA: 0x0004B9B3 File Offset: 0x0004A9B3
		internal XPathDocument()
		{
			this.nameTable = new NameTable();
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x0004B9C6 File Offset: 0x0004A9C6
		internal XPathDocument(XmlNameTable nameTable)
		{
			if (nameTable == null)
			{
				throw new ArgumentNullException("nameTable");
			}
			this.nameTable = nameTable;
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x0004B9E3 File Offset: 0x0004A9E3
		public XPathDocument(XmlReader reader)
			: this(reader, XmlSpace.Default)
		{
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x0004B9ED File Offset: 0x0004A9ED
		public XPathDocument(XmlReader reader, XmlSpace space)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this.LoadFromReader(reader, space);
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x0004BA0C File Offset: 0x0004AA0C
		public XPathDocument(TextReader textReader)
		{
			XmlTextReaderImpl xmlTextReaderImpl = this.SetupReader(new XmlTextReaderImpl(string.Empty, textReader));
			try
			{
				this.LoadFromReader(xmlTextReaderImpl, XmlSpace.Default);
			}
			finally
			{
				xmlTextReaderImpl.Close();
			}
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x0004BA54 File Offset: 0x0004AA54
		public XPathDocument(Stream stream)
		{
			XmlTextReaderImpl xmlTextReaderImpl = this.SetupReader(new XmlTextReaderImpl(string.Empty, stream));
			try
			{
				this.LoadFromReader(xmlTextReaderImpl, XmlSpace.Default);
			}
			finally
			{
				xmlTextReaderImpl.Close();
			}
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x0004BA9C File Offset: 0x0004AA9C
		public XPathDocument(string uri)
			: this(uri, XmlSpace.Default)
		{
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x0004BAA8 File Offset: 0x0004AAA8
		public XPathDocument(string uri, XmlSpace space)
		{
			XmlTextReaderImpl xmlTextReaderImpl = this.SetupReader(new XmlTextReaderImpl(uri));
			try
			{
				this.LoadFromReader(xmlTextReaderImpl, space);
			}
			finally
			{
				xmlTextReaderImpl.Close();
			}
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x0004BAEC File Offset: 0x0004AAEC
		internal XmlRawWriter LoadFromWriter(XPathDocument.LoadFlags flags, string baseUri)
		{
			return new XPathDocumentBuilder(this, null, baseUri, flags);
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x0004BAF8 File Offset: 0x0004AAF8
		internal void LoadFromReader(XmlReader reader, XmlSpace space)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			IXmlLineInfo xmlLineInfo = reader as IXmlLineInfo;
			if (xmlLineInfo == null || !xmlLineInfo.HasLineInfo())
			{
				xmlLineInfo = null;
			}
			this.hasLineInfo = xmlLineInfo != null;
			this.nameTable = reader.NameTable;
			XPathDocumentBuilder xpathDocumentBuilder = new XPathDocumentBuilder(this, xmlLineInfo, reader.BaseURI, XPathDocument.LoadFlags.None);
			try
			{
				bool flag = reader.ReadState == ReadState.Initial;
				int depth = reader.Depth;
				string text = this.nameTable.Get("http://www.w3.org/2000/xmlns/");
				if (!flag || reader.Read())
				{
					while (flag || reader.Depth >= depth)
					{
						switch (reader.NodeType)
						{
						case XmlNodeType.Element:
						{
							bool isEmptyElement = reader.IsEmptyElement;
							xpathDocumentBuilder.WriteStartElement(reader.Prefix, reader.LocalName, reader.NamespaceURI, reader.BaseURI);
							while (reader.MoveToNextAttribute())
							{
								string namespaceURI = reader.NamespaceURI;
								if (namespaceURI == text)
								{
									if (reader.Prefix.Length == 0)
									{
										xpathDocumentBuilder.WriteNamespaceDeclaration(string.Empty, reader.Value);
									}
									else
									{
										xpathDocumentBuilder.WriteNamespaceDeclaration(reader.LocalName, reader.Value);
									}
								}
								else
								{
									xpathDocumentBuilder.WriteStartAttribute(reader.Prefix, reader.LocalName, namespaceURI);
									xpathDocumentBuilder.WriteString(reader.Value, TextBlockType.Text);
									xpathDocumentBuilder.WriteEndAttribute();
								}
							}
							if (isEmptyElement)
							{
								xpathDocumentBuilder.WriteEndElement(true);
							}
							break;
						}
						case XmlNodeType.Text:
						case XmlNodeType.CDATA:
							xpathDocumentBuilder.WriteString(reader.Value, TextBlockType.Text);
							break;
						case XmlNodeType.EntityReference:
							reader.ResolveEntity();
							break;
						case XmlNodeType.ProcessingInstruction:
							xpathDocumentBuilder.WriteProcessingInstruction(reader.LocalName, reader.Value, reader.BaseURI);
							break;
						case XmlNodeType.Comment:
							xpathDocumentBuilder.WriteComment(reader.Value);
							break;
						case XmlNodeType.DocumentType:
						{
							SchemaInfo dtdSchemaInfo = XmlReader.GetDtdSchemaInfo(reader);
							if (dtdSchemaInfo != null)
							{
								xpathDocumentBuilder.CreateIdTables(dtdSchemaInfo);
							}
							break;
						}
						case XmlNodeType.Whitespace:
							goto IL_01C9;
						case XmlNodeType.SignificantWhitespace:
							if (reader.XmlSpace != XmlSpace.Preserve)
							{
								goto IL_01C9;
							}
							xpathDocumentBuilder.WriteString(reader.Value, TextBlockType.SignificantWhitespace);
							break;
						case XmlNodeType.EndElement:
							xpathDocumentBuilder.WriteEndElement(false);
							break;
						}
						IL_022B:
						if (!reader.Read())
						{
							break;
						}
						continue;
						IL_01C9:
						if (space == XmlSpace.Preserve && (!flag || reader.Depth != 0))
						{
							xpathDocumentBuilder.WriteString(reader.Value, TextBlockType.Whitespace);
							goto IL_022B;
						}
						goto IL_022B;
					}
				}
			}
			finally
			{
				xpathDocumentBuilder.Close();
			}
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x0004BD60 File Offset: 0x0004AD60
		public XPathNavigator CreateNavigator()
		{
			return new XPathDocumentNavigator(this.pageRoot, this.idxRoot, null, 0);
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x060010A0 RID: 4256 RVA: 0x0004BD75 File Offset: 0x0004AD75
		internal XmlNameTable NameTable
		{
			get
			{
				return this.nameTable;
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x060010A1 RID: 4257 RVA: 0x0004BD7D File Offset: 0x0004AD7D
		internal bool HasLineInfo
		{
			get
			{
				return this.hasLineInfo;
			}
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x0004BD85 File Offset: 0x0004AD85
		internal int GetCollapsedTextNode(out XPathNode[] pageText)
		{
			pageText = this.pageText;
			return this.idxText;
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x0004BD95 File Offset: 0x0004AD95
		internal void SetCollapsedTextNode(XPathNode[] pageText, int idxText)
		{
			this.pageText = pageText;
			this.idxText = idxText;
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x0004BDA5 File Offset: 0x0004ADA5
		internal int GetRootNode(out XPathNode[] pageRoot)
		{
			pageRoot = this.pageRoot;
			return this.idxRoot;
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x0004BDB5 File Offset: 0x0004ADB5
		internal void SetRootNode(XPathNode[] pageRoot, int idxRoot)
		{
			this.pageRoot = pageRoot;
			this.idxRoot = idxRoot;
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x0004BDC5 File Offset: 0x0004ADC5
		internal int GetXmlNamespaceNode(out XPathNode[] pageXmlNmsp)
		{
			pageXmlNmsp = this.pageXmlNmsp;
			return this.idxXmlNmsp;
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x0004BDD5 File Offset: 0x0004ADD5
		internal void SetXmlNamespaceNode(XPathNode[] pageXmlNmsp, int idxXmlNmsp)
		{
			this.pageXmlNmsp = pageXmlNmsp;
			this.idxXmlNmsp = idxXmlNmsp;
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x0004BDE5 File Offset: 0x0004ADE5
		internal void AddNamespace(XPathNode[] pageElem, int idxElem, XPathNode[] pageNmsp, int idxNmsp)
		{
			if (this.mapNmsp == null)
			{
				this.mapNmsp = new Dictionary<XPathNodeRef, XPathNodeRef>();
			}
			this.mapNmsp.Add(new XPathNodeRef(pageElem, idxElem), new XPathNodeRef(pageNmsp, idxNmsp));
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x0004BE14 File Offset: 0x0004AE14
		internal int LookupNamespaces(XPathNode[] pageElem, int idxElem, out XPathNode[] pageNmsp)
		{
			XPathNodeRef xpathNodeRef = new XPathNodeRef(pageElem, idxElem);
			if (this.mapNmsp == null || !this.mapNmsp.ContainsKey(xpathNodeRef))
			{
				pageNmsp = null;
				return 0;
			}
			xpathNodeRef = this.mapNmsp[xpathNodeRef];
			pageNmsp = xpathNodeRef.Page;
			return xpathNodeRef.Index;
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x0004BE62 File Offset: 0x0004AE62
		internal void AddIdElement(string id, XPathNode[] pageElem, int idxElem)
		{
			if (this.idValueMap == null)
			{
				this.idValueMap = new Dictionary<string, XPathNodeRef>();
			}
			if (!this.idValueMap.ContainsKey(id))
			{
				this.idValueMap.Add(id, new XPathNodeRef(pageElem, idxElem));
			}
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x0004BE98 File Offset: 0x0004AE98
		internal int LookupIdElement(string id, out XPathNode[] pageElem)
		{
			if (this.idValueMap == null || !this.idValueMap.ContainsKey(id))
			{
				pageElem = null;
				return 0;
			}
			XPathNodeRef xpathNodeRef = this.idValueMap[id];
			pageElem = xpathNodeRef.Page;
			return xpathNodeRef.Index;
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x0004BEDD File Offset: 0x0004AEDD
		private XmlTextReaderImpl SetupReader(XmlTextReaderImpl reader)
		{
			reader.EntityHandling = EntityHandling.ExpandEntities;
			reader.XmlValidatingReaderCompatibilityMode = true;
			return reader;
		}

		// Token: 0x04000AC8 RID: 2760
		private XPathNode[] pageText;

		// Token: 0x04000AC9 RID: 2761
		private XPathNode[] pageRoot;

		// Token: 0x04000ACA RID: 2762
		private XPathNode[] pageXmlNmsp;

		// Token: 0x04000ACB RID: 2763
		private int idxText;

		// Token: 0x04000ACC RID: 2764
		private int idxRoot;

		// Token: 0x04000ACD RID: 2765
		private int idxXmlNmsp;

		// Token: 0x04000ACE RID: 2766
		private XmlNameTable nameTable;

		// Token: 0x04000ACF RID: 2767
		private bool hasLineInfo;

		// Token: 0x04000AD0 RID: 2768
		private Dictionary<XPathNodeRef, XPathNodeRef> mapNmsp;

		// Token: 0x04000AD1 RID: 2769
		private Dictionary<string, XPathNodeRef> idValueMap;

		// Token: 0x02000110 RID: 272
		internal enum LoadFlags
		{
			// Token: 0x04000AD3 RID: 2771
			None,
			// Token: 0x04000AD4 RID: 2772
			AtomizeNames,
			// Token: 0x04000AD5 RID: 2773
			Fragment
		}
	}
}
