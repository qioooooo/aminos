using System;
using System.Collections;

namespace System.Xml
{
	// Token: 0x020000DA RID: 218
	internal class XmlElementList : XmlNodeList
	{
		// Token: 0x06000D7B RID: 3451 RVA: 0x0003BCE8 File Offset: 0x0003ACE8
		private XmlElementList(XmlNode parent)
		{
			this.rootNode = parent;
			this.curInd = -1;
			this.curElem = this.rootNode;
			this.changeCount = 0;
			this.empty = false;
			this.atomized = true;
			this.matchCount = -1;
			new XmlElementListListener(parent.Document, this);
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x0003BD40 File Offset: 0x0003AD40
		internal void ConcurrencyCheck(XmlNodeChangedEventArgs args)
		{
			if (!this.atomized)
			{
				XmlNameTable nameTable = this.rootNode.Document.NameTable;
				this.localName = nameTable.Add(this.localName);
				this.namespaceURI = nameTable.Add(this.namespaceURI);
				this.atomized = true;
			}
			if (this.IsMatch(args.Node))
			{
				this.changeCount++;
				this.curInd = -1;
				this.curElem = this.rootNode;
				if (args.Action == XmlNodeChangedAction.Insert)
				{
					this.empty = false;
				}
			}
			this.matchCount = -1;
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x0003BDD8 File Offset: 0x0003ADD8
		internal XmlElementList(XmlNode parent, string name)
			: this(parent)
		{
			XmlNameTable nameTable = parent.Document.NameTable;
			this.asterisk = nameTable.Add("*");
			this.name = nameTable.Add(name);
			this.localName = null;
			this.namespaceURI = null;
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x0003BE24 File Offset: 0x0003AE24
		internal XmlElementList(XmlNode parent, string localName, string namespaceURI)
			: this(parent)
		{
			XmlNameTable nameTable = parent.Document.NameTable;
			this.asterisk = nameTable.Add("*");
			this.localName = nameTable.Get(localName);
			this.namespaceURI = nameTable.Get(namespaceURI);
			if (this.localName == null || this.namespaceURI == null)
			{
				this.empty = true;
				this.atomized = false;
				this.localName = localName;
				this.namespaceURI = namespaceURI;
			}
			this.name = null;
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000D7F RID: 3455 RVA: 0x0003BEA2 File Offset: 0x0003AEA2
		internal int ChangeCount
		{
			get
			{
				return this.changeCount;
			}
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x0003BEAC File Offset: 0x0003AEAC
		private XmlNode NextElemInPreOrder(XmlNode curNode)
		{
			XmlNode xmlNode = curNode.FirstChild;
			if (xmlNode == null)
			{
				xmlNode = curNode;
				while (xmlNode != null && xmlNode != this.rootNode && xmlNode.NextSibling == null)
				{
					xmlNode = xmlNode.ParentNode;
				}
				if (xmlNode != null && xmlNode != this.rootNode)
				{
					xmlNode = xmlNode.NextSibling;
				}
			}
			if (xmlNode == this.rootNode)
			{
				xmlNode = null;
			}
			return xmlNode;
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0003BF04 File Offset: 0x0003AF04
		private XmlNode PrevElemInPreOrder(XmlNode curNode)
		{
			XmlNode xmlNode = curNode.PreviousSibling;
			while (xmlNode != null && xmlNode.LastChild != null)
			{
				xmlNode = xmlNode.LastChild;
			}
			if (xmlNode == null)
			{
				xmlNode = curNode.ParentNode;
			}
			if (xmlNode == this.rootNode)
			{
				xmlNode = null;
			}
			return xmlNode;
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x0003BF44 File Offset: 0x0003AF44
		private bool IsMatch(XmlNode curNode)
		{
			if (curNode.NodeType == XmlNodeType.Element)
			{
				if (this.name != null)
				{
					if (Ref.Equal(this.name, this.asterisk) || Ref.Equal(curNode.Name, this.name))
					{
						return true;
					}
				}
				else if ((Ref.Equal(this.localName, this.asterisk) || Ref.Equal(curNode.LocalName, this.localName)) && (Ref.Equal(this.namespaceURI, this.asterisk) || curNode.NamespaceURI == this.namespaceURI))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x0003BFDC File Offset: 0x0003AFDC
		private XmlNode GetMatchingNode(XmlNode n, bool bNext)
		{
			XmlNode xmlNode = n;
			do
			{
				if (bNext)
				{
					xmlNode = this.NextElemInPreOrder(xmlNode);
				}
				else
				{
					xmlNode = this.PrevElemInPreOrder(xmlNode);
				}
			}
			while (xmlNode != null && !this.IsMatch(xmlNode));
			return xmlNode;
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x0003C010 File Offset: 0x0003B010
		private XmlNode GetNthMatchingNode(XmlNode n, bool bNext, int nCount)
		{
			XmlNode xmlNode = n;
			for (int i = 0; i < nCount; i++)
			{
				xmlNode = this.GetMatchingNode(xmlNode, bNext);
				if (xmlNode == null)
				{
					return null;
				}
			}
			return xmlNode;
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x0003C03C File Offset: 0x0003B03C
		public XmlNode GetNextNode(XmlNode n)
		{
			if (this.empty)
			{
				return null;
			}
			XmlNode xmlNode = ((n == null) ? this.rootNode : n);
			return this.GetMatchingNode(xmlNode, true);
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x0003C068 File Offset: 0x0003B068
		public override XmlNode Item(int index)
		{
			if (this.rootNode == null || index < 0)
			{
				return null;
			}
			if (this.empty)
			{
				return null;
			}
			if (this.curInd == index)
			{
				return this.curElem;
			}
			int num = index - this.curInd;
			bool flag = num > 0;
			if (num < 0)
			{
				num = -num;
			}
			XmlNode nthMatchingNode;
			if ((nthMatchingNode = this.GetNthMatchingNode(this.curElem, flag, num)) != null)
			{
				this.curInd = index;
				this.curElem = nthMatchingNode;
				return this.curElem;
			}
			return null;
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000D87 RID: 3463 RVA: 0x0003C0DC File Offset: 0x0003B0DC
		public override int Count
		{
			get
			{
				if (this.empty)
				{
					return 0;
				}
				if (this.matchCount < 0)
				{
					int num = 0;
					int num2 = this.changeCount;
					XmlNode matchingNode = this.rootNode;
					while ((matchingNode = this.GetMatchingNode(matchingNode, true)) != null)
					{
						num++;
					}
					if (num2 != this.changeCount)
					{
						return num;
					}
					this.matchCount = num;
				}
				return this.matchCount;
			}
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x0003C136 File Offset: 0x0003B136
		public override IEnumerator GetEnumerator()
		{
			if (this.empty)
			{
				return new XmlEmptyElementListEnumerator(this);
			}
			return new XmlElementListEnumerator(this);
		}

		// Token: 0x04000949 RID: 2377
		private string asterisk;

		// Token: 0x0400094A RID: 2378
		private int changeCount;

		// Token: 0x0400094B RID: 2379
		private string name;

		// Token: 0x0400094C RID: 2380
		private string localName;

		// Token: 0x0400094D RID: 2381
		private string namespaceURI;

		// Token: 0x0400094E RID: 2382
		private XmlNode rootNode;

		// Token: 0x0400094F RID: 2383
		private int curInd;

		// Token: 0x04000950 RID: 2384
		private XmlNode curElem;

		// Token: 0x04000951 RID: 2385
		private bool empty;

		// Token: 0x04000952 RID: 2386
		private bool atomized;

		// Token: 0x04000953 RID: 2387
		private int matchCount;
	}
}
