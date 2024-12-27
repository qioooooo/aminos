using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;

namespace MS.Internal.Xml.Cache
{
	// Token: 0x02000100 RID: 256
	internal sealed class XPathDocumentBuilder : XmlRawWriter
	{
		// Token: 0x06000FB9 RID: 4025 RVA: 0x0004859A File Offset: 0x0004759A
		public XPathDocumentBuilder(XPathDocument doc, IXmlLineInfo lineInfo, string baseUri, XPathDocument.LoadFlags flags)
		{
			this.nodePageFact.Init(256);
			this.nmspPageFact.Init(16);
			this.stkNmsp = new Stack<XPathNodeRef>();
			this.Initialize(doc, lineInfo, baseUri, flags);
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x000485D8 File Offset: 0x000475D8
		public void Initialize(XPathDocument doc, IXmlLineInfo lineInfo, string baseUri, XPathDocument.LoadFlags flags)
		{
			this.doc = doc;
			this.nameTable = doc.NameTable;
			this.atomizeNames = (flags & XPathDocument.LoadFlags.AtomizeNames) != XPathDocument.LoadFlags.None;
			this.idxParent = (this.idxSibling = 0);
			this.elemNameIndex = new XPathNodeRef[64];
			this.textBldr.Initialize(lineInfo);
			this.lineInfo = lineInfo;
			this.lineNumBase = 0;
			this.linePosBase = 0;
			this.infoTable = new XPathNodeInfoTable();
			XPathNode[] array;
			int num = this.NewNode(out array, XPathNodeType.Text, string.Empty, string.Empty, string.Empty, string.Empty);
			this.doc.SetCollapsedTextNode(array, num);
			this.idxNmsp = this.NewNamespaceNode(out this.pageNmsp, this.nameTable.Add("xml"), this.nameTable.Add("http://www.w3.org/XML/1998/namespace"), null, 0);
			this.doc.SetXmlNamespaceNode(this.pageNmsp, this.idxNmsp);
			if ((flags & XPathDocument.LoadFlags.Fragment) == XPathDocument.LoadFlags.None)
			{
				this.idxParent = this.NewNode(out this.pageParent, XPathNodeType.Root, string.Empty, string.Empty, string.Empty, baseUri);
				this.doc.SetRootNode(this.pageParent, this.idxParent);
				return;
			}
			this.doc.SetRootNode(this.nodePageFact.NextNodePage, this.nodePageFact.NextNodeIndex);
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x00048729 File Offset: 0x00047729
		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0004872B File Offset: 0x0004772B
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			this.WriteStartElement(prefix, localName, ns, string.Empty);
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x0004873C File Offset: 0x0004773C
		public void WriteStartElement(string prefix, string localName, string ns, string baseUri)
		{
			if (this.atomizeNames)
			{
				prefix = this.nameTable.Add(prefix);
				localName = this.nameTable.Add(localName);
				ns = this.nameTable.Add(ns);
			}
			this.AddSibling(XPathNodeType.Element, localName, ns, prefix, baseUri);
			this.pageParent = this.pageSibling;
			this.idxParent = this.idxSibling;
			this.idxSibling = 0;
			int num = this.pageParent[this.idxParent].LocalNameHashCode & 63;
			this.elemNameIndex[num] = this.LinkSimilarElements(this.elemNameIndex[num].Page, this.elemNameIndex[num].Index, this.pageParent, this.idxParent);
			if (this.elemIdMap != null)
			{
				this.idAttrName = (XmlQualifiedName)this.elemIdMap[new XmlQualifiedName(localName, prefix)];
			}
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x0004882A File Offset: 0x0004782A
		public override void WriteEndElement()
		{
			this.WriteEndElement(true);
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x00048833 File Offset: 0x00047833
		public override void WriteFullEndElement()
		{
			this.WriteEndElement(false);
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x0004883C File Offset: 0x0004783C
		internal override void WriteEndElement(string prefix, string localName, string namespaceName)
		{
			this.WriteEndElement(true);
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x00048845 File Offset: 0x00047845
		internal override void WriteFullEndElement(string prefix, string localName, string namespaceName)
		{
			this.WriteEndElement(false);
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x00048850 File Offset: 0x00047850
		public void WriteEndElement(bool allowShortcutTag)
		{
			if (!this.pageParent[this.idxParent].HasContentChild)
			{
				switch (this.textBldr.TextType)
				{
				case TextBlockType.Text:
					if (this.lineInfo != null)
					{
						if (this.textBldr.LineNumber != this.pageParent[this.idxParent].LineNumber)
						{
							break;
						}
						int num = this.textBldr.LinePosition - this.pageParent[this.idxParent].LinePosition;
						if (num < 0 || num > 255)
						{
							break;
						}
						this.pageParent[this.idxParent].SetCollapsedLineInfoOffset(num);
					}
					this.pageParent[this.idxParent].SetCollapsedValue(this.textBldr.ReadText());
					goto IL_0134;
				case TextBlockType.SignificantWhitespace:
				case TextBlockType.Whitespace:
					break;
				default:
					this.pageParent[this.idxParent].SetEmptyValue(allowShortcutTag);
					goto IL_0134;
				}
				this.CachedTextNode();
				this.pageParent[this.idxParent].SetValue(this.pageSibling[this.idxSibling].Value);
			}
			else if (this.textBldr.HasText)
			{
				this.CachedTextNode();
			}
			IL_0134:
			if (this.pageParent[this.idxParent].HasNamespaceDecls)
			{
				this.doc.AddNamespace(this.pageParent, this.idxParent, this.pageNmsp, this.idxNmsp);
				XPathNodeRef xpathNodeRef = this.stkNmsp.Pop();
				this.pageNmsp = xpathNodeRef.Page;
				this.idxNmsp = xpathNodeRef.Index;
			}
			this.pageSibling = this.pageParent;
			this.idxSibling = this.idxParent;
			this.idxParent = this.pageParent[this.idxParent].GetParent(out this.pageParent);
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x00048A2C File Offset: 0x00047A2C
		public override void WriteStartAttribute(string prefix, string localName, string namespaceName)
		{
			if (this.atomizeNames)
			{
				prefix = this.nameTable.Add(prefix);
				localName = this.nameTable.Add(localName);
				namespaceName = this.nameTable.Add(namespaceName);
			}
			this.AddSibling(XPathNodeType.Attribute, localName, namespaceName, prefix, string.Empty);
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x00048A7C File Offset: 0x00047A7C
		public override void WriteEndAttribute()
		{
			this.pageSibling[this.idxSibling].SetValue(this.textBldr.ReadText());
			if (this.idAttrName != null && this.pageSibling[this.idxSibling].LocalName == this.idAttrName.Name && this.pageSibling[this.idxSibling].Prefix == this.idAttrName.Namespace)
			{
				this.doc.AddIdElement(this.pageSibling[this.idxSibling].Value, this.pageParent, this.idxParent);
			}
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x00048B35 File Offset: 0x00047B35
		public override void WriteCData(string text)
		{
			this.WriteString(text, TextBlockType.Text);
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x00048B3F File Offset: 0x00047B3F
		public override void WriteComment(string text)
		{
			this.AddSibling(XPathNodeType.Comment, string.Empty, string.Empty, string.Empty, string.Empty);
			this.pageSibling[this.idxSibling].SetValue(text);
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x00048B73 File Offset: 0x00047B73
		public override void WriteProcessingInstruction(string name, string text)
		{
			this.WriteProcessingInstruction(name, text, string.Empty);
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x00048B84 File Offset: 0x00047B84
		public void WriteProcessingInstruction(string name, string text, string baseUri)
		{
			if (this.atomizeNames)
			{
				name = this.nameTable.Add(name);
			}
			this.AddSibling(XPathNodeType.ProcessingInstruction, name, string.Empty, string.Empty, baseUri);
			this.pageSibling[this.idxSibling].SetValue(text);
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x00048BD1 File Offset: 0x00047BD1
		public override void WriteWhitespace(string ws)
		{
			this.WriteString(ws, TextBlockType.Whitespace);
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x00048BDB File Offset: 0x00047BDB
		public override void WriteString(string text)
		{
			this.WriteString(text, TextBlockType.Text);
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x00048BE5 File Offset: 0x00047BE5
		public override void WriteChars(char[] buffer, int index, int count)
		{
			this.WriteString(new string(buffer, index, count), TextBlockType.Text);
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x00048BF6 File Offset: 0x00047BF6
		public override void WriteRaw(string data)
		{
			this.WriteString(data, TextBlockType.Text);
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x00048C00 File Offset: 0x00047C00
		public override void WriteRaw(char[] buffer, int index, int count)
		{
			this.WriteString(new string(buffer, index, count), TextBlockType.Text);
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x00048C11 File Offset: 0x00047C11
		public void WriteString(string text, TextBlockType textType)
		{
			this.textBldr.WriteTextBlock(text, textType);
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x00048C20 File Offset: 0x00047C20
		public override void WriteEntityRef(string name)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x00048C28 File Offset: 0x00047C28
		public override void WriteCharEntity(char ch)
		{
			char[] array = new char[] { ch };
			this.WriteString(new string(array), TextBlockType.Text);
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x00048C50 File Offset: 0x00047C50
		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			char[] array = new char[] { highChar, lowChar };
			this.WriteString(new string(array), TextBlockType.Text);
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x00048C7C File Offset: 0x00047C7C
		public override void Close()
		{
			if (this.textBldr.HasText)
			{
				this.CachedTextNode();
			}
			XPathNode[] array;
			int rootNode = this.doc.GetRootNode(out array);
			if (rootNode == this.nodePageFact.NextNodeIndex && array == this.nodePageFact.NextNodePage)
			{
				this.AddSibling(XPathNodeType.Text, string.Empty, string.Empty, string.Empty, string.Empty);
				this.pageSibling[this.idxSibling].SetValue(string.Empty);
			}
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x00048CFC File Offset: 0x00047CFC
		public override void Flush()
		{
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x00048CFE File Offset: 0x00047CFE
		internal override void WriteXmlDeclaration(XmlStandalone standalone)
		{
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x00048D00 File Offset: 0x00047D00
		internal override void WriteXmlDeclaration(string xmldecl)
		{
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x00048D02 File Offset: 0x00047D02
		internal override void StartElementContent()
		{
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x00048D04 File Offset: 0x00047D04
		internal override void WriteNamespaceDeclaration(string prefix, string namespaceName)
		{
			if (this.atomizeNames)
			{
				prefix = this.nameTable.Add(prefix);
			}
			namespaceName = this.nameTable.Add(namespaceName);
			XPathNode[] array = this.pageNmsp;
			int num = this.idxNmsp;
			while (num != 0 && array[num].LocalName != prefix)
			{
				num = array[num].GetSibling(out array);
			}
			XPathNode[] array2;
			int num2 = this.NewNamespaceNode(out array2, prefix, namespaceName, this.pageParent, this.idxParent);
			if (num != 0)
			{
				XPathNode[] array3 = this.pageNmsp;
				int sibling = this.idxNmsp;
				XPathNode[] array4 = array2;
				int num3 = num2;
				while (sibling != num || array3 != array)
				{
					XPathNode[] array5;
					int num4 = array3[sibling].GetParent(out array5);
					num4 = this.NewNamespaceNode(out array5, array3[sibling].LocalName, array3[sibling].Value, array5, num4);
					array4[num3].SetSibling(this.infoTable, array5, num4);
					array4 = array5;
					num3 = num4;
					sibling = array3[sibling].GetSibling(out array3);
				}
				num = array[num].GetSibling(out array);
				if (num != 0)
				{
					array4[num3].SetSibling(this.infoTable, array, num);
				}
			}
			else if (this.idxParent != 0)
			{
				array2[num2].SetSibling(this.infoTable, this.pageNmsp, this.idxNmsp);
			}
			else
			{
				this.doc.SetRootNode(array2, num2);
			}
			if (this.idxParent != 0)
			{
				if (!this.pageParent[this.idxParent].HasNamespaceDecls)
				{
					this.stkNmsp.Push(new XPathNodeRef(this.pageNmsp, this.idxNmsp));
					this.pageParent[this.idxParent].HasNamespaceDecls = true;
				}
				this.pageNmsp = array2;
				this.idxNmsp = num2;
			}
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x00048EDC File Offset: 0x00047EDC
		public void CreateIdTables(SchemaInfo schInfo)
		{
			foreach (object obj in schInfo.ElementDecls.Values)
			{
				SchemaElementDecl schemaElementDecl = (SchemaElementDecl)obj;
				if (schemaElementDecl.AttDefs != null)
				{
					foreach (object obj2 in schemaElementDecl.AttDefs.Values)
					{
						SchemaAttDef schemaAttDef = (SchemaAttDef)obj2;
						if (schemaAttDef.Datatype.TokenizedType == XmlTokenizedType.ID)
						{
							if (this.elemIdMap == null)
							{
								this.elemIdMap = new Hashtable();
							}
							this.elemIdMap.Add(schemaElementDecl.Name, schemaAttDef.Name);
							break;
						}
					}
				}
			}
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x00048FC8 File Offset: 0x00047FC8
		private XPathNodeRef LinkSimilarElements(XPathNode[] pagePrev, int idxPrev, XPathNode[] pageNext, int idxNext)
		{
			if (pagePrev != null)
			{
				pagePrev[idxPrev].SetSimilarElement(this.infoTable, pageNext, idxNext);
			}
			return new XPathNodeRef(pageNext, idxNext);
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x00048FEC File Offset: 0x00047FEC
		private int NewNamespaceNode(out XPathNode[] page, string prefix, string namespaceUri, XPathNode[] pageElem, int idxElem)
		{
			XPathNode[] array;
			int num;
			this.nmspPageFact.AllocateSlot(out array, out num);
			int num2;
			int num3;
			this.ComputeLineInfo(false, out num2, out num3);
			XPathNodeInfoAtom xpathNodeInfoAtom = this.infoTable.Create(prefix, string.Empty, string.Empty, string.Empty, pageElem, array, null, this.doc, this.lineNumBase, this.linePosBase);
			array[num].Create(xpathNodeInfoAtom, XPathNodeType.Namespace, idxElem);
			array[num].SetValue(namespaceUri);
			array[num].SetLineInfoOffsets(num2, num3);
			page = array;
			return num;
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x00049078 File Offset: 0x00048078
		private int NewNode(out XPathNode[] page, XPathNodeType xptyp, string localName, string namespaceUri, string prefix, string baseUri)
		{
			XPathNode[] array;
			int num;
			this.nodePageFact.AllocateSlot(out array, out num);
			int num2;
			int num3;
			this.ComputeLineInfo(XPathNavigator.IsText(xptyp), out num2, out num3);
			XPathNodeInfoAtom xpathNodeInfoAtom = this.infoTable.Create(localName, namespaceUri, prefix, baseUri, this.pageParent, array, array, this.doc, this.lineNumBase, this.linePosBase);
			array[num].Create(xpathNodeInfoAtom, xptyp, this.idxParent);
			array[num].SetLineInfoOffsets(num2, num3);
			page = array;
			return num;
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x000490FC File Offset: 0x000480FC
		private void ComputeLineInfo(bool isTextNode, out int lineNumOffset, out int linePosOffset)
		{
			if (this.lineInfo == null)
			{
				lineNumOffset = 0;
				linePosOffset = 0;
				return;
			}
			int num;
			int num2;
			if (isTextNode)
			{
				num = this.textBldr.LineNumber;
				num2 = this.textBldr.LinePosition;
			}
			else
			{
				num = this.lineInfo.LineNumber;
				num2 = this.lineInfo.LinePosition;
			}
			lineNumOffset = num - this.lineNumBase;
			if (lineNumOffset < 0 || lineNumOffset > 16383)
			{
				this.lineNumBase = num;
				lineNumOffset = 0;
			}
			linePosOffset = num2 - this.linePosBase;
			if (linePosOffset < 0 || linePosOffset > 65535)
			{
				this.linePosBase = num2;
				linePosOffset = 0;
			}
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x00049194 File Offset: 0x00048194
		private void AddSibling(XPathNodeType xptyp, string localName, string namespaceUri, string prefix, string baseUri)
		{
			if (this.textBldr.HasText)
			{
				this.CachedTextNode();
			}
			XPathNode[] array;
			int num = this.NewNode(out array, xptyp, localName, namespaceUri, prefix, baseUri);
			if (this.idxParent != 0)
			{
				this.pageParent[this.idxParent].SetParentProperties(xptyp);
				if (this.idxSibling != 0)
				{
					this.pageSibling[this.idxSibling].SetSibling(this.infoTable, array, num);
				}
			}
			this.pageSibling = array;
			this.idxSibling = num;
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x00049218 File Offset: 0x00048218
		private void CachedTextNode()
		{
			TextBlockType textType = this.textBldr.TextType;
			string text = this.textBldr.ReadText();
			this.AddSibling((XPathNodeType)textType, string.Empty, string.Empty, string.Empty, string.Empty);
			this.pageSibling[this.idxSibling].SetValue(text);
		}

		// Token: 0x04000A6E RID: 2670
		private const int ElementIndexSize = 64;

		// Token: 0x04000A6F RID: 2671
		private XPathDocumentBuilder.NodePageFactory nodePageFact;

		// Token: 0x04000A70 RID: 2672
		private XPathDocumentBuilder.NodePageFactory nmspPageFact;

		// Token: 0x04000A71 RID: 2673
		private XPathDocumentBuilder.TextBlockBuilder textBldr;

		// Token: 0x04000A72 RID: 2674
		private Stack<XPathNodeRef> stkNmsp;

		// Token: 0x04000A73 RID: 2675
		private XPathNodeInfoTable infoTable;

		// Token: 0x04000A74 RID: 2676
		private XPathDocument doc;

		// Token: 0x04000A75 RID: 2677
		private IXmlLineInfo lineInfo;

		// Token: 0x04000A76 RID: 2678
		private XmlNameTable nameTable;

		// Token: 0x04000A77 RID: 2679
		private bool atomizeNames;

		// Token: 0x04000A78 RID: 2680
		private XPathNode[] pageNmsp;

		// Token: 0x04000A79 RID: 2681
		private int idxNmsp;

		// Token: 0x04000A7A RID: 2682
		private XPathNode[] pageParent;

		// Token: 0x04000A7B RID: 2683
		private int idxParent;

		// Token: 0x04000A7C RID: 2684
		private XPathNode[] pageSibling;

		// Token: 0x04000A7D RID: 2685
		private int idxSibling;

		// Token: 0x04000A7E RID: 2686
		private int lineNumBase;

		// Token: 0x04000A7F RID: 2687
		private int linePosBase;

		// Token: 0x04000A80 RID: 2688
		private XmlQualifiedName idAttrName;

		// Token: 0x04000A81 RID: 2689
		private Hashtable elemIdMap;

		// Token: 0x04000A82 RID: 2690
		private XPathNodeRef[] elemNameIndex;

		// Token: 0x02000101 RID: 257
		private struct NodePageFactory
		{
			// Token: 0x06000FDF RID: 4063 RVA: 0x0004926F File Offset: 0x0004826F
			public void Init(int initialPageSize)
			{
				this.pageSize = initialPageSize;
				this.page = new XPathNode[this.pageSize];
				this.pageInfo = new XPathNodePageInfo(null, 1);
				this.page[0].Create(this.pageInfo);
			}

			// Token: 0x170003C0 RID: 960
			// (get) Token: 0x06000FE0 RID: 4064 RVA: 0x000492AD File Offset: 0x000482AD
			public XPathNode[] NextNodePage
			{
				get
				{
					return this.page;
				}
			}

			// Token: 0x170003C1 RID: 961
			// (get) Token: 0x06000FE1 RID: 4065 RVA: 0x000492B5 File Offset: 0x000482B5
			public int NextNodeIndex
			{
				get
				{
					return this.pageInfo.NodeCount;
				}
			}

			// Token: 0x06000FE2 RID: 4066 RVA: 0x000492C4 File Offset: 0x000482C4
			public void AllocateSlot(out XPathNode[] page, out int idx)
			{
				page = this.page;
				idx = this.pageInfo.NodeCount;
				if (++this.pageInfo.NodeCount >= this.page.Length)
				{
					if (this.pageSize < 65536)
					{
						this.pageSize *= 2;
					}
					this.page = new XPathNode[this.pageSize];
					this.pageInfo.NextPage = this.page;
					this.pageInfo = new XPathNodePageInfo(page, this.pageInfo.PageNumber + 1);
					this.page[0].Create(this.pageInfo);
				}
			}

			// Token: 0x04000A83 RID: 2691
			private XPathNode[] page;

			// Token: 0x04000A84 RID: 2692
			private XPathNodePageInfo pageInfo;

			// Token: 0x04000A85 RID: 2693
			private int pageSize;
		}

		// Token: 0x02000102 RID: 258
		private struct TextBlockBuilder
		{
			// Token: 0x06000FE3 RID: 4067 RVA: 0x00049374 File Offset: 0x00048374
			public void Initialize(IXmlLineInfo lineInfo)
			{
				this.lineInfo = lineInfo;
				this.textType = TextBlockType.None;
			}

			// Token: 0x170003C2 RID: 962
			// (get) Token: 0x06000FE4 RID: 4068 RVA: 0x00049384 File Offset: 0x00048384
			public TextBlockType TextType
			{
				get
				{
					return this.textType;
				}
			}

			// Token: 0x170003C3 RID: 963
			// (get) Token: 0x06000FE5 RID: 4069 RVA: 0x0004938C File Offset: 0x0004838C
			public bool HasText
			{
				get
				{
					return this.textType != TextBlockType.None;
				}
			}

			// Token: 0x170003C4 RID: 964
			// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x0004939A File Offset: 0x0004839A
			public int LineNumber
			{
				get
				{
					return this.lineNum;
				}
			}

			// Token: 0x170003C5 RID: 965
			// (get) Token: 0x06000FE7 RID: 4071 RVA: 0x000493A2 File Offset: 0x000483A2
			public int LinePosition
			{
				get
				{
					return this.linePos;
				}
			}

			// Token: 0x06000FE8 RID: 4072 RVA: 0x000493AC File Offset: 0x000483AC
			public void WriteTextBlock(string text, TextBlockType textType)
			{
				if (text.Length != 0)
				{
					if (this.textType == TextBlockType.None)
					{
						this.text = text;
						this.textType = textType;
						if (this.lineInfo != null)
						{
							this.lineNum = this.lineInfo.LineNumber;
							this.linePos = this.lineInfo.LinePosition;
							return;
						}
					}
					else
					{
						this.text += text;
						if (textType < this.textType)
						{
							this.textType = textType;
						}
					}
				}
			}

			// Token: 0x06000FE9 RID: 4073 RVA: 0x00049424 File Offset: 0x00048424
			public string ReadText()
			{
				if (this.textType == TextBlockType.None)
				{
					return string.Empty;
				}
				this.textType = TextBlockType.None;
				return this.text;
			}

			// Token: 0x04000A86 RID: 2694
			private IXmlLineInfo lineInfo;

			// Token: 0x04000A87 RID: 2695
			private TextBlockType textType;

			// Token: 0x04000A88 RID: 2696
			private string text;

			// Token: 0x04000A89 RID: 2697
			private int lineNum;

			// Token: 0x04000A8A RID: 2698
			private int linePos;
		}
	}
}
