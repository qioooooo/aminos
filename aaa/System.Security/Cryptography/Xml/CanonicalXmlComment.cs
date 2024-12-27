using System;
using System.Text;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x0200008D RID: 141
	internal class CanonicalXmlComment : XmlComment, ICanonicalizableNode
	{
		// Token: 0x0600027C RID: 636 RVA: 0x0000E484 File Offset: 0x0000D484
		public CanonicalXmlComment(string comment, XmlDocument doc, bool defaultNodeSetInclusionState, bool includeComments)
			: base(comment, doc)
		{
			this.m_isInNodeSet = defaultNodeSetInclusionState;
			this.m_includeComments = includeComments;
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600027D RID: 637 RVA: 0x0000E49D File Offset: 0x0000D49D
		// (set) Token: 0x0600027E RID: 638 RVA: 0x0000E4A5 File Offset: 0x0000D4A5
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

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600027F RID: 639 RVA: 0x0000E4AE File Offset: 0x0000D4AE
		public bool IncludeComments
		{
			get
			{
				return this.m_includeComments;
			}
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000E4B8 File Offset: 0x0000D4B8
		public void Write(StringBuilder strBuilder, DocPosition docPos, AncestralNamespaceContextManager anc)
		{
			if (!this.IsInNodeSet || !this.IncludeComments)
			{
				return;
			}
			if (docPos == DocPosition.AfterRootElement)
			{
				strBuilder.Append('\n');
			}
			strBuilder.Append("<!--");
			strBuilder.Append(this.Value);
			strBuilder.Append("-->");
			if (docPos == DocPosition.BeforeRootElement)
			{
				strBuilder.Append('\n');
			}
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000E514 File Offset: 0x0000D514
		public void WriteHash(HashAlgorithm hash, DocPosition docPos, AncestralNamespaceContextManager anc)
		{
			if (!this.IsInNodeSet || !this.IncludeComments)
			{
				return;
			}
			UTF8Encoding utf8Encoding = new UTF8Encoding(false);
			byte[] array = utf8Encoding.GetBytes("(char) 10");
			if (docPos == DocPosition.AfterRootElement)
			{
				hash.TransformBlock(array, 0, array.Length, array, 0);
			}
			array = utf8Encoding.GetBytes("<!--");
			hash.TransformBlock(array, 0, array.Length, array, 0);
			array = utf8Encoding.GetBytes(this.Value);
			hash.TransformBlock(array, 0, array.Length, array, 0);
			array = utf8Encoding.GetBytes("-->");
			hash.TransformBlock(array, 0, array.Length, array, 0);
			if (docPos == DocPosition.BeforeRootElement)
			{
				array = utf8Encoding.GetBytes("(char) 10");
				hash.TransformBlock(array, 0, array.Length, array, 0);
			}
		}

		// Token: 0x040004E1 RID: 1249
		private bool m_isInNodeSet;

		// Token: 0x040004E2 RID: 1250
		private bool m_includeComments;
	}
}
