using System;
using System.Text;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x0200008B RID: 139
	internal class CanonicalXmlWhitespace : XmlWhitespace, ICanonicalizableNode
	{
		// Token: 0x06000272 RID: 626 RVA: 0x0000E37C File Offset: 0x0000D37C
		public CanonicalXmlWhitespace(string strData, XmlDocument doc, bool defaultNodeSetInclusionState)
			: base(strData, doc)
		{
			this.m_isInNodeSet = defaultNodeSetInclusionState;
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0000E38D File Offset: 0x0000D38D
		// (set) Token: 0x06000274 RID: 628 RVA: 0x0000E395 File Offset: 0x0000D395
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

		// Token: 0x06000275 RID: 629 RVA: 0x0000E39E File Offset: 0x0000D39E
		public void Write(StringBuilder strBuilder, DocPosition docPos, AncestralNamespaceContextManager anc)
		{
			if (this.IsInNodeSet && docPos == DocPosition.InRootElement)
			{
				strBuilder.Append(Utils.EscapeWhitespaceData(this.Value));
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000E3C0 File Offset: 0x0000D3C0
		public void WriteHash(HashAlgorithm hash, DocPosition docPos, AncestralNamespaceContextManager anc)
		{
			if (this.IsInNodeSet && docPos == DocPosition.InRootElement)
			{
				UTF8Encoding utf8Encoding = new UTF8Encoding(false);
				byte[] bytes = utf8Encoding.GetBytes(Utils.EscapeWhitespaceData(this.Value));
				hash.TransformBlock(bytes, 0, bytes.Length, bytes, 0);
			}
		}

		// Token: 0x040004DF RID: 1247
		private bool m_isInNodeSet;
	}
}
