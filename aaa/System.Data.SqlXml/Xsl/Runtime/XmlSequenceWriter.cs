using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000BD RID: 189
	internal abstract class XmlSequenceWriter
	{
		// Token: 0x06000946 RID: 2374
		public abstract XmlRawWriter StartTree(XPathNodeType rootType, IXmlNamespaceResolver nsResolver, XmlNameTable nameTable);

		// Token: 0x06000947 RID: 2375
		public abstract void EndTree();

		// Token: 0x06000948 RID: 2376
		public abstract void WriteItem(XPathItem item);
	}
}
