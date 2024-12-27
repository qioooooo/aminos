using System;
using System.Text;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x02000089 RID: 137
	internal class CanonicalXmlAttribute : XmlAttribute, ICanonicalizableNode
	{
		// Token: 0x06000268 RID: 616 RVA: 0x0000E224 File Offset: 0x0000D224
		public CanonicalXmlAttribute(string prefix, string localName, string namespaceURI, XmlDocument doc, bool defaultNodeSetInclusionState)
			: base(prefix, localName, namespaceURI, doc)
		{
			this.IsInNodeSet = defaultNodeSetInclusionState;
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000269 RID: 617 RVA: 0x0000E239 File Offset: 0x0000D239
		// (set) Token: 0x0600026A RID: 618 RVA: 0x0000E241 File Offset: 0x0000D241
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

		// Token: 0x0600026B RID: 619 RVA: 0x0000E24A File Offset: 0x0000D24A
		public void Write(StringBuilder strBuilder, DocPosition docPos, AncestralNamespaceContextManager anc)
		{
			strBuilder.Append(" " + this.Name + "=\"");
			strBuilder.Append(Utils.EscapeAttributeValue(this.Value));
			strBuilder.Append("\"");
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000E288 File Offset: 0x0000D288
		public void WriteHash(HashAlgorithm hash, DocPosition docPos, AncestralNamespaceContextManager anc)
		{
			UTF8Encoding utf8Encoding = new UTF8Encoding(false);
			byte[] array = utf8Encoding.GetBytes(" " + this.Name + "=\"");
			hash.TransformBlock(array, 0, array.Length, array, 0);
			array = utf8Encoding.GetBytes(Utils.EscapeAttributeValue(this.Value));
			hash.TransformBlock(array, 0, array.Length, array, 0);
			array = utf8Encoding.GetBytes("\"");
			hash.TransformBlock(array, 0, array.Length, array, 0);
		}

		// Token: 0x040004DD RID: 1245
		private bool m_isInNodeSet;
	}
}
