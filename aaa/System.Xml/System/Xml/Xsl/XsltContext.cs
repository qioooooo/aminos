using System;
using System.Xml.XPath;

namespace System.Xml.Xsl
{
	// Token: 0x02000131 RID: 305
	public abstract class XsltContext : XmlNamespaceManager
	{
		// Token: 0x060011C4 RID: 4548 RVA: 0x0004E7ED File Offset: 0x0004D7ED
		protected XsltContext(NameTable table)
			: base(table)
		{
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x0004E7F6 File Offset: 0x0004D7F6
		protected XsltContext()
			: base(new NameTable())
		{
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x0004E803 File Offset: 0x0004D803
		internal XsltContext(bool dummy)
		{
		}

		// Token: 0x060011C7 RID: 4551
		public abstract IXsltContextVariable ResolveVariable(string prefix, string name);

		// Token: 0x060011C8 RID: 4552
		public abstract IXsltContextFunction ResolveFunction(string prefix, string name, XPathResultType[] ArgTypes);

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x060011C9 RID: 4553
		public abstract bool Whitespace { get; }

		// Token: 0x060011CA RID: 4554
		public abstract bool PreserveWhitespace(XPathNavigator node);

		// Token: 0x060011CB RID: 4555
		public abstract int CompareDocument(string baseUri, string nextbaseUri);
	}
}
