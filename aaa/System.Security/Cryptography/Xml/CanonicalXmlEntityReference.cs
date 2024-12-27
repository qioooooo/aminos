using System;
using System.Text;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x0200008F RID: 143
	internal class CanonicalXmlEntityReference : XmlEntityReference, ICanonicalizableNode
	{
		// Token: 0x06000287 RID: 647 RVA: 0x0000E74E File Offset: 0x0000D74E
		public CanonicalXmlEntityReference(string name, XmlDocument doc, bool defaultNodeSetInclusionState)
			: base(name, doc)
		{
			this.m_isInNodeSet = defaultNodeSetInclusionState;
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000288 RID: 648 RVA: 0x0000E75F File Offset: 0x0000D75F
		// (set) Token: 0x06000289 RID: 649 RVA: 0x0000E767 File Offset: 0x0000D767
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

		// Token: 0x0600028A RID: 650 RVA: 0x0000E770 File Offset: 0x0000D770
		public void Write(StringBuilder strBuilder, DocPosition docPos, AncestralNamespaceContextManager anc)
		{
			if (this.IsInNodeSet)
			{
				CanonicalizationDispatcher.WriteGenericNode(this, strBuilder, docPos, anc);
			}
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000E783 File Offset: 0x0000D783
		public void WriteHash(HashAlgorithm hash, DocPosition docPos, AncestralNamespaceContextManager anc)
		{
			if (this.IsInNodeSet)
			{
				CanonicalizationDispatcher.WriteHashGenericNode(this, hash, docPos, anc);
			}
		}

		// Token: 0x040004E4 RID: 1252
		private bool m_isInNodeSet;
	}
}
