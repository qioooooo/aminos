using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000BE RID: 190
	internal class XmlCachedSequenceWriter : XmlSequenceWriter
	{
		// Token: 0x0600094A RID: 2378 RVA: 0x0002BEA1 File Offset: 0x0002AEA1
		public XmlCachedSequenceWriter()
		{
			this.seqTyped = new XmlQueryItemSequence();
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600094B RID: 2379 RVA: 0x0002BEB4 File Offset: 0x0002AEB4
		public XmlQueryItemSequence ResultSequence
		{
			get
			{
				return this.seqTyped;
			}
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0002BEBC File Offset: 0x0002AEBC
		public override XmlRawWriter StartTree(XPathNodeType rootType, IXmlNamespaceResolver nsResolver, XmlNameTable nameTable)
		{
			this.doc = new XPathDocument(nameTable);
			this.writer = this.doc.LoadFromWriter(XPathDocument.LoadFlags.AtomizeNames | ((rootType == XPathNodeType.Root) ? XPathDocument.LoadFlags.None : XPathDocument.LoadFlags.Fragment), string.Empty);
			this.writer.NamespaceResolver = nsResolver;
			return this.writer;
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0002BEFB File Offset: 0x0002AEFB
		public override void EndTree()
		{
			this.writer.Close();
			this.seqTyped.Add(this.doc.CreateNavigator());
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0002BF1E File Offset: 0x0002AF1E
		public override void WriteItem(XPathItem item)
		{
			this.seqTyped.AddClone(item);
		}

		// Token: 0x040005BA RID: 1466
		private XmlQueryItemSequence seqTyped;

		// Token: 0x040005BB RID: 1467
		private XPathDocument doc;

		// Token: 0x040005BC RID: 1468
		private XmlRawWriter writer;
	}
}
