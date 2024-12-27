using System;
using System.Text;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x0200008A RID: 138
	internal class CanonicalXmlText : XmlText, ICanonicalizableNode
	{
		// Token: 0x0600026D RID: 621 RVA: 0x0000E300 File Offset: 0x0000D300
		public CanonicalXmlText(string strData, XmlDocument doc, bool defaultNodeSetInclusionState)
			: base(strData, doc)
		{
			this.m_isInNodeSet = defaultNodeSetInclusionState;
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600026E RID: 622 RVA: 0x0000E311 File Offset: 0x0000D311
		// (set) Token: 0x0600026F RID: 623 RVA: 0x0000E319 File Offset: 0x0000D319
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

		// Token: 0x06000270 RID: 624 RVA: 0x0000E322 File Offset: 0x0000D322
		public void Write(StringBuilder strBuilder, DocPosition docPos, AncestralNamespaceContextManager anc)
		{
			if (this.IsInNodeSet)
			{
				strBuilder.Append(Utils.EscapeTextData(this.Value));
			}
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000E340 File Offset: 0x0000D340
		public void WriteHash(HashAlgorithm hash, DocPosition docPos, AncestralNamespaceContextManager anc)
		{
			if (this.IsInNodeSet)
			{
				UTF8Encoding utf8Encoding = new UTF8Encoding(false);
				byte[] bytes = utf8Encoding.GetBytes(Utils.EscapeTextData(this.Value));
				hash.TransformBlock(bytes, 0, bytes.Length, bytes, 0);
			}
		}

		// Token: 0x040004DE RID: 1246
		private bool m_isInNodeSet;
	}
}
