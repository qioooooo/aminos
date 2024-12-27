using System;
using System.Text;
using System.Xml.XPath;

namespace MS.Internal.Xml.Cache
{
	// Token: 0x0200010D RID: 269
	internal sealed class XPathNodeInfoAtom
	{
		// Token: 0x0600107C RID: 4220 RVA: 0x0004B38A File Offset: 0x0004A38A
		public XPathNodeInfoAtom(XPathNodePageInfo pageInfo)
		{
			this.pageInfo = pageInfo;
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x0004B39C File Offset: 0x0004A39C
		public XPathNodeInfoAtom(string localName, string namespaceUri, string prefix, string baseUri, XPathNode[] pageParent, XPathNode[] pageSibling, XPathNode[] pageSimilar, XPathDocument doc, int lineNumBase, int linePosBase)
		{
			this.Init(localName, namespaceUri, prefix, baseUri, pageParent, pageSibling, pageSimilar, doc, lineNumBase, linePosBase);
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x0004B3C8 File Offset: 0x0004A3C8
		public void Init(string localName, string namespaceUri, string prefix, string baseUri, XPathNode[] pageParent, XPathNode[] pageSibling, XPathNode[] pageSimilar, XPathDocument doc, int lineNumBase, int linePosBase)
		{
			this.localName = localName;
			this.namespaceUri = namespaceUri;
			this.prefix = prefix;
			this.baseUri = baseUri;
			this.pageParent = pageParent;
			this.pageSibling = pageSibling;
			this.pageSimilar = pageSimilar;
			this.doc = doc;
			this.lineNumBase = lineNumBase;
			this.linePosBase = linePosBase;
			this.next = null;
			this.pageInfo = null;
			this.hashCode = 0;
			this.localNameHash = 0;
			for (int i = 0; i < this.localName.Length; i++)
			{
				this.localNameHash += (this.localNameHash << 7) ^ (int)this.localName[i];
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x0600107F RID: 4223 RVA: 0x0004B476 File Offset: 0x0004A476
		public XPathNodePageInfo PageInfo
		{
			get
			{
				return this.pageInfo;
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06001080 RID: 4224 RVA: 0x0004B47E File Offset: 0x0004A47E
		public string LocalName
		{
			get
			{
				return this.localName;
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06001081 RID: 4225 RVA: 0x0004B486 File Offset: 0x0004A486
		public string NamespaceUri
		{
			get
			{
				return this.namespaceUri;
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06001082 RID: 4226 RVA: 0x0004B48E File Offset: 0x0004A48E
		public string Prefix
		{
			get
			{
				return this.prefix;
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06001083 RID: 4227 RVA: 0x0004B496 File Offset: 0x0004A496
		public string BaseUri
		{
			get
			{
				return this.baseUri;
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06001084 RID: 4228 RVA: 0x0004B49E File Offset: 0x0004A49E
		public XPathNode[] SiblingPage
		{
			get
			{
				return this.pageSibling;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06001085 RID: 4229 RVA: 0x0004B4A6 File Offset: 0x0004A4A6
		public XPathNode[] SimilarElementPage
		{
			get
			{
				return this.pageSimilar;
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06001086 RID: 4230 RVA: 0x0004B4AE File Offset: 0x0004A4AE
		public XPathNode[] ParentPage
		{
			get
			{
				return this.pageParent;
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06001087 RID: 4231 RVA: 0x0004B4B6 File Offset: 0x0004A4B6
		public XPathDocument Document
		{
			get
			{
				return this.doc;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06001088 RID: 4232 RVA: 0x0004B4BE File Offset: 0x0004A4BE
		public int LineNumberBase
		{
			get
			{
				return this.lineNumBase;
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06001089 RID: 4233 RVA: 0x0004B4C6 File Offset: 0x0004A4C6
		public int LinePositionBase
		{
			get
			{
				return this.linePosBase;
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x0600108A RID: 4234 RVA: 0x0004B4CE File Offset: 0x0004A4CE
		public int LocalNameHashCode
		{
			get
			{
				return this.localNameHash;
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x0600108B RID: 4235 RVA: 0x0004B4D6 File Offset: 0x0004A4D6
		// (set) Token: 0x0600108C RID: 4236 RVA: 0x0004B4DE File Offset: 0x0004A4DE
		public XPathNodeInfoAtom Next
		{
			get
			{
				return this.next;
			}
			set
			{
				this.next = value;
			}
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x0004B4E8 File Offset: 0x0004A4E8
		public override int GetHashCode()
		{
			if (this.hashCode == 0)
			{
				int num = this.localNameHash;
				if (this.pageSibling != null)
				{
					num += (num << 7) ^ this.pageSibling[0].PageInfo.PageNumber;
				}
				if (this.pageParent != null)
				{
					num += (num << 7) ^ this.pageParent[0].PageInfo.PageNumber;
				}
				if (this.pageSimilar != null)
				{
					num += (num << 7) ^ this.pageSimilar[0].PageInfo.PageNumber;
				}
				this.hashCode = ((num == 0) ? 1 : num);
			}
			return this.hashCode;
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x0004B58C File Offset: 0x0004A58C
		public override bool Equals(object other)
		{
			XPathNodeInfoAtom xpathNodeInfoAtom = other as XPathNodeInfoAtom;
			return this.GetHashCode() == xpathNodeInfoAtom.GetHashCode() && this.localName == xpathNodeInfoAtom.localName && this.pageSibling == xpathNodeInfoAtom.pageSibling && this.namespaceUri == xpathNodeInfoAtom.namespaceUri && this.pageParent == xpathNodeInfoAtom.pageParent && this.pageSimilar == xpathNodeInfoAtom.pageSimilar && this.prefix == xpathNodeInfoAtom.prefix && this.baseUri == xpathNodeInfoAtom.baseUri && this.lineNumBase == xpathNodeInfoAtom.lineNumBase && this.linePosBase == xpathNodeInfoAtom.linePosBase;
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x0004B634 File Offset: 0x0004A634
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("hash=");
			stringBuilder.Append(this.GetHashCode());
			stringBuilder.Append(", ");
			if (this.localName.Length != 0)
			{
				stringBuilder.Append('{');
				stringBuilder.Append(this.namespaceUri);
				stringBuilder.Append('}');
				if (this.prefix.Length != 0)
				{
					stringBuilder.Append(this.prefix);
					stringBuilder.Append(':');
				}
				stringBuilder.Append(this.localName);
				stringBuilder.Append(", ");
			}
			if (this.pageParent != null)
			{
				stringBuilder.Append("parent=");
				stringBuilder.Append(this.pageParent[0].PageInfo.PageNumber);
				stringBuilder.Append(", ");
			}
			if (this.pageSibling != null)
			{
				stringBuilder.Append("sibling=");
				stringBuilder.Append(this.pageSibling[0].PageInfo.PageNumber);
				stringBuilder.Append(", ");
			}
			if (this.pageSimilar != null)
			{
				stringBuilder.Append("similar=");
				stringBuilder.Append(this.pageSimilar[0].PageInfo.PageNumber);
				stringBuilder.Append(", ");
			}
			stringBuilder.Append("lineNum=");
			stringBuilder.Append(this.lineNumBase);
			stringBuilder.Append(", ");
			stringBuilder.Append("linePos=");
			stringBuilder.Append(this.linePosBase);
			return stringBuilder.ToString();
		}

		// Token: 0x04000AB6 RID: 2742
		private string localName;

		// Token: 0x04000AB7 RID: 2743
		private string namespaceUri;

		// Token: 0x04000AB8 RID: 2744
		private string prefix;

		// Token: 0x04000AB9 RID: 2745
		private string baseUri;

		// Token: 0x04000ABA RID: 2746
		private XPathNode[] pageParent;

		// Token: 0x04000ABB RID: 2747
		private XPathNode[] pageSibling;

		// Token: 0x04000ABC RID: 2748
		private XPathNode[] pageSimilar;

		// Token: 0x04000ABD RID: 2749
		private XPathDocument doc;

		// Token: 0x04000ABE RID: 2750
		private int lineNumBase;

		// Token: 0x04000ABF RID: 2751
		private int linePosBase;

		// Token: 0x04000AC0 RID: 2752
		private int hashCode;

		// Token: 0x04000AC1 RID: 2753
		private int localNameHash;

		// Token: 0x04000AC2 RID: 2754
		private XPathNodeInfoAtom next;

		// Token: 0x04000AC3 RID: 2755
		private XPathNodePageInfo pageInfo;
	}
}
