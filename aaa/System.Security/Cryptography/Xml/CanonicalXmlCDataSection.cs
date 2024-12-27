using System;
using System.Text;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x02000090 RID: 144
	internal class CanonicalXmlCDataSection : XmlCDataSection, ICanonicalizableNode
	{
		// Token: 0x0600028C RID: 652 RVA: 0x0000E796 File Offset: 0x0000D796
		public CanonicalXmlCDataSection(string data, XmlDocument doc, bool defaultNodeSetInclusionState)
			: base(data, doc)
		{
			this.m_isInNodeSet = defaultNodeSetInclusionState;
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600028D RID: 653 RVA: 0x0000E7A7 File Offset: 0x0000D7A7
		// (set) Token: 0x0600028E RID: 654 RVA: 0x0000E7AF File Offset: 0x0000D7AF
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

		// Token: 0x0600028F RID: 655 RVA: 0x0000E7B8 File Offset: 0x0000D7B8
		public void Write(StringBuilder strBuilder, DocPosition docPos, AncestralNamespaceContextManager anc)
		{
			if (this.IsInNodeSet)
			{
				strBuilder.Append(Utils.EscapeCData(this.Data));
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000E7D4 File Offset: 0x0000D7D4
		public void WriteHash(HashAlgorithm hash, DocPosition docPos, AncestralNamespaceContextManager anc)
		{
			if (this.IsInNodeSet)
			{
				UTF8Encoding utf8Encoding = new UTF8Encoding(false);
				byte[] bytes = utf8Encoding.GetBytes(Utils.EscapeCData(this.Data));
				hash.TransformBlock(bytes, 0, bytes.Length, bytes, 0);
			}
		}

		// Token: 0x040004E5 RID: 1253
		private bool m_isInNodeSet;
	}
}
