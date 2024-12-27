using System;
using System.Text;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x0200008E RID: 142
	internal class CanonicalXmlProcessingInstruction : XmlProcessingInstruction, ICanonicalizableNode
	{
		// Token: 0x06000282 RID: 642 RVA: 0x0000E5C3 File Offset: 0x0000D5C3
		public CanonicalXmlProcessingInstruction(string target, string data, XmlDocument doc, bool defaultNodeSetInclusionState)
			: base(target, data, doc)
		{
			this.m_isInNodeSet = defaultNodeSetInclusionState;
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000E5D6 File Offset: 0x0000D5D6
		// (set) Token: 0x06000284 RID: 644 RVA: 0x0000E5DE File Offset: 0x0000D5DE
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

		// Token: 0x06000285 RID: 645 RVA: 0x0000E5E8 File Offset: 0x0000D5E8
		public void Write(StringBuilder strBuilder, DocPosition docPos, AncestralNamespaceContextManager anc)
		{
			if (!this.IsInNodeSet)
			{
				return;
			}
			if (docPos == DocPosition.AfterRootElement)
			{
				strBuilder.Append('\n');
			}
			strBuilder.Append("<?");
			strBuilder.Append(this.Name);
			if (this.Value != null && this.Value.Length > 0)
			{
				strBuilder.Append(" " + this.Value);
			}
			strBuilder.Append("?>");
			if (docPos == DocPosition.BeforeRootElement)
			{
				strBuilder.Append('\n');
			}
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000E66C File Offset: 0x0000D66C
		public void WriteHash(HashAlgorithm hash, DocPosition docPos, AncestralNamespaceContextManager anc)
		{
			if (!this.IsInNodeSet)
			{
				return;
			}
			UTF8Encoding utf8Encoding = new UTF8Encoding(false);
			byte[] array;
			if (docPos == DocPosition.AfterRootElement)
			{
				array = utf8Encoding.GetBytes("(char) 10");
				hash.TransformBlock(array, 0, array.Length, array, 0);
			}
			array = utf8Encoding.GetBytes("<?");
			hash.TransformBlock(array, 0, array.Length, array, 0);
			array = utf8Encoding.GetBytes(this.Name);
			hash.TransformBlock(array, 0, array.Length, array, 0);
			if (this.Value != null && this.Value.Length > 0)
			{
				array = utf8Encoding.GetBytes(" " + this.Value);
				hash.TransformBlock(array, 0, array.Length, array, 0);
			}
			array = utf8Encoding.GetBytes("?>");
			hash.TransformBlock(array, 0, array.Length, array, 0);
			if (docPos == DocPosition.BeforeRootElement)
			{
				array = utf8Encoding.GetBytes("(char) 10");
				hash.TransformBlock(array, 0, array.Length, array, 0);
			}
		}

		// Token: 0x040004E3 RID: 1251
		private bool m_isInNodeSet;
	}
}
