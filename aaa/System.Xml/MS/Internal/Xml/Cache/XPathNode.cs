using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.Cache
{
	// Token: 0x02000109 RID: 265
	internal struct XPathNode
	{
		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06001030 RID: 4144 RVA: 0x0004A640 File Offset: 0x00049640
		public XPathNodeType NodeType
		{
			get
			{
				return (XPathNodeType)(this.props & 15U);
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06001031 RID: 4145 RVA: 0x0004A64B File Offset: 0x0004964B
		public string Prefix
		{
			get
			{
				return this.info.Prefix;
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06001032 RID: 4146 RVA: 0x0004A658 File Offset: 0x00049658
		public string LocalName
		{
			get
			{
				return this.info.LocalName;
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06001033 RID: 4147 RVA: 0x0004A665 File Offset: 0x00049665
		public string Name
		{
			get
			{
				if (this.Prefix.Length == 0)
				{
					return this.LocalName;
				}
				return this.Prefix + ":" + this.LocalName;
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06001034 RID: 4148 RVA: 0x0004A691 File Offset: 0x00049691
		public string NamespaceUri
		{
			get
			{
				return this.info.NamespaceUri;
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06001035 RID: 4149 RVA: 0x0004A69E File Offset: 0x0004969E
		public XPathDocument Document
		{
			get
			{
				return this.info.Document;
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06001036 RID: 4150 RVA: 0x0004A6AB File Offset: 0x000496AB
		public string BaseUri
		{
			get
			{
				return this.info.BaseUri;
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06001037 RID: 4151 RVA: 0x0004A6B8 File Offset: 0x000496B8
		public int LineNumber
		{
			get
			{
				return this.info.LineNumberBase + (int)((this.props & 16776192U) >> 10);
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06001038 RID: 4152 RVA: 0x0004A6D5 File Offset: 0x000496D5
		public int LinePosition
		{
			get
			{
				return this.info.LinePositionBase + (int)this.posOffset;
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06001039 RID: 4153 RVA: 0x0004A6E9 File Offset: 0x000496E9
		public int CollapsedLinePosition
		{
			get
			{
				return this.LinePosition + (int)(this.props >> 24);
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x0600103A RID: 4154 RVA: 0x0004A6FB File Offset: 0x000496FB
		public XPathNodePageInfo PageInfo
		{
			get
			{
				return this.info.PageInfo;
			}
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x0004A708 File Offset: 0x00049708
		public int GetRoot(out XPathNode[] pageNode)
		{
			return this.info.Document.GetRootNode(out pageNode);
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x0004A71B File Offset: 0x0004971B
		public int GetParent(out XPathNode[] pageNode)
		{
			pageNode = this.info.ParentPage;
			return (int)this.idxParent;
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x0004A730 File Offset: 0x00049730
		public int GetSibling(out XPathNode[] pageNode)
		{
			pageNode = this.info.SiblingPage;
			return (int)this.idxSibling;
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x0004A745 File Offset: 0x00049745
		public int GetSimilarElement(out XPathNode[] pageNode)
		{
			pageNode = this.info.SimilarElementPage;
			return (int)this.idxSimilar;
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x0004A75A File Offset: 0x0004975A
		public bool NameMatch(string localName, string namespaceName)
		{
			return this.info.LocalName == localName && this.info.NamespaceUri == namespaceName;
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x0004A77D File Offset: 0x0004977D
		public bool ElementMatch(string localName, string namespaceName)
		{
			return this.NodeType == XPathNodeType.Element && this.info.LocalName == localName && this.info.NamespaceUri == namespaceName;
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06001041 RID: 4161 RVA: 0x0004A7AC File Offset: 0x000497AC
		public bool IsXmlNamespaceNode
		{
			get
			{
				string localName = this.info.LocalName;
				return this.NodeType == XPathNodeType.Namespace && localName.Length == 3 && localName == "xml";
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06001042 RID: 4162 RVA: 0x0004A7E4 File Offset: 0x000497E4
		public bool HasSibling
		{
			get
			{
				return this.idxSibling != 0;
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06001043 RID: 4163 RVA: 0x0004A7F2 File Offset: 0x000497F2
		public bool HasCollapsedText
		{
			get
			{
				return (this.props & 128U) != 0U;
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06001044 RID: 4164 RVA: 0x0004A806 File Offset: 0x00049806
		public bool HasAttribute
		{
			get
			{
				return (this.props & 16U) != 0U;
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06001045 RID: 4165 RVA: 0x0004A817 File Offset: 0x00049817
		public bool HasContentChild
		{
			get
			{
				return (this.props & 32U) != 0U;
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06001046 RID: 4166 RVA: 0x0004A828 File Offset: 0x00049828
		public bool HasElementChild
		{
			get
			{
				return (this.props & 64U) != 0U;
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06001047 RID: 4167 RVA: 0x0004A83C File Offset: 0x0004983C
		public bool IsAttrNmsp
		{
			get
			{
				XPathNodeType nodeType = this.NodeType;
				return nodeType == XPathNodeType.Attribute || nodeType == XPathNodeType.Namespace;
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06001048 RID: 4168 RVA: 0x0004A85A File Offset: 0x0004985A
		public bool IsText
		{
			get
			{
				return XPathNavigator.IsText(this.NodeType);
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06001049 RID: 4169 RVA: 0x0004A867 File Offset: 0x00049867
		// (set) Token: 0x0600104A RID: 4170 RVA: 0x0004A87B File Offset: 0x0004987B
		public bool HasNamespaceDecls
		{
			get
			{
				return (this.props & 512U) != 0U;
			}
			set
			{
				if (value)
				{
					this.props |= 512U;
					return;
				}
				this.props &= 255U;
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x0600104B RID: 4171 RVA: 0x0004A8A5 File Offset: 0x000498A5
		public bool AllowShortcutTag
		{
			get
			{
				return (this.props & 256U) != 0U;
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x0600104C RID: 4172 RVA: 0x0004A8B9 File Offset: 0x000498B9
		public int LocalNameHashCode
		{
			get
			{
				return this.info.LocalNameHashCode;
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x0600104D RID: 4173 RVA: 0x0004A8C6 File Offset: 0x000498C6
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x0004A8CE File Offset: 0x000498CE
		public void Create(XPathNodePageInfo pageInfo)
		{
			this.info = new XPathNodeInfoAtom(pageInfo);
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x0004A8DC File Offset: 0x000498DC
		public void Create(XPathNodeInfoAtom info, XPathNodeType xptyp, int idxParent)
		{
			this.info = info;
			this.props = (uint)xptyp;
			this.idxParent = (ushort)idxParent;
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x0004A8F4 File Offset: 0x000498F4
		public void SetLineInfoOffsets(int lineNumOffset, int linePosOffset)
		{
			this.props |= (uint)((uint)lineNumOffset << 10);
			this.posOffset = (ushort)linePosOffset;
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x0004A90F File Offset: 0x0004990F
		public void SetCollapsedLineInfoOffset(int posOffset)
		{
			this.props |= (uint)((uint)posOffset << 24);
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x0004A922 File Offset: 0x00049922
		public void SetValue(string value)
		{
			this.value = value;
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x0004A92B File Offset: 0x0004992B
		public void SetEmptyValue(bool allowShortcutTag)
		{
			this.value = string.Empty;
			if (allowShortcutTag)
			{
				this.props |= 256U;
			}
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x0004A94D File Offset: 0x0004994D
		public void SetCollapsedValue(string value)
		{
			this.value = value;
			this.props |= 160U;
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x0004A968 File Offset: 0x00049968
		public void SetParentProperties(XPathNodeType xptyp)
		{
			if (xptyp == XPathNodeType.Attribute)
			{
				this.props |= 16U;
				return;
			}
			this.props |= 32U;
			if (xptyp == XPathNodeType.Element)
			{
				this.props |= 64U;
			}
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x0004A9A0 File Offset: 0x000499A0
		public void SetSibling(XPathNodeInfoTable infoTable, XPathNode[] pageSibling, int idxSibling)
		{
			this.idxSibling = (ushort)idxSibling;
			if (pageSibling != this.info.SiblingPage)
			{
				this.info = infoTable.Create(this.info.LocalName, this.info.NamespaceUri, this.info.Prefix, this.info.BaseUri, this.info.ParentPage, pageSibling, this.info.SimilarElementPage, this.info.Document, this.info.LineNumberBase, this.info.LinePositionBase);
			}
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x0004AA34 File Offset: 0x00049A34
		public void SetSimilarElement(XPathNodeInfoTable infoTable, XPathNode[] pageSimilar, int idxSimilar)
		{
			this.idxSimilar = (ushort)idxSimilar;
			if (pageSimilar != this.info.SimilarElementPage)
			{
				this.info = infoTable.Create(this.info.LocalName, this.info.NamespaceUri, this.info.Prefix, this.info.BaseUri, this.info.ParentPage, this.info.SiblingPage, pageSimilar, this.info.Document, this.info.LineNumberBase, this.info.LinePositionBase);
			}
		}

		// Token: 0x04000A9C RID: 2716
		private const uint NodeTypeMask = 15U;

		// Token: 0x04000A9D RID: 2717
		private const uint HasAttributeBit = 16U;

		// Token: 0x04000A9E RID: 2718
		private const uint HasContentChildBit = 32U;

		// Token: 0x04000A9F RID: 2719
		private const uint HasElementChildBit = 64U;

		// Token: 0x04000AA0 RID: 2720
		private const uint HasCollapsedTextBit = 128U;

		// Token: 0x04000AA1 RID: 2721
		private const uint AllowShortcutTagBit = 256U;

		// Token: 0x04000AA2 RID: 2722
		private const uint HasNmspDeclsBit = 512U;

		// Token: 0x04000AA3 RID: 2723
		private const uint LineNumberMask = 16776192U;

		// Token: 0x04000AA4 RID: 2724
		private const int LineNumberShift = 10;

		// Token: 0x04000AA5 RID: 2725
		private const int CollapsedPositionShift = 24;

		// Token: 0x04000AA6 RID: 2726
		public const int MaxLineNumberOffset = 16383;

		// Token: 0x04000AA7 RID: 2727
		public const int MaxLinePositionOffset = 65535;

		// Token: 0x04000AA8 RID: 2728
		public const int MaxCollapsedPositionOffset = 255;

		// Token: 0x04000AA9 RID: 2729
		private XPathNodeInfoAtom info;

		// Token: 0x04000AAA RID: 2730
		private ushort idxSibling;

		// Token: 0x04000AAB RID: 2731
		private ushort idxParent;

		// Token: 0x04000AAC RID: 2732
		private ushort idxSimilar;

		// Token: 0x04000AAD RID: 2733
		private ushort posOffset;

		// Token: 0x04000AAE RID: 2734
		private uint props;

		// Token: 0x04000AAF RID: 2735
		private string value;
	}
}
