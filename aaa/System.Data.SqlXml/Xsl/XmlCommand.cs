using System;
using System.Collections;
using System.IO;
using System.Xml.XPath;

namespace System.Xml.Xsl
{
	// Token: 0x0200000A RID: 10
	internal abstract class XmlCommand
	{
		// Token: 0x0600002A RID: 42
		public abstract void Execute(IXPathNavigable contextDocument, XmlResolver dataSources, XsltArgumentList argumentList, XmlWriter results);

		// Token: 0x0600002B RID: 43
		public abstract void Execute(IXPathNavigable contextDocument, XmlResolver dataSources, XsltArgumentList argumentList, TextWriter results);

		// Token: 0x0600002C RID: 44
		public abstract void Execute(IXPathNavigable contextDocument, XmlResolver dataSources, XsltArgumentList argumentList, Stream results);

		// Token: 0x0600002D RID: 45
		public abstract void Execute(XmlReader contextDocument, XmlResolver dataSources, XsltArgumentList argumentList, XmlWriter results);

		// Token: 0x0600002E RID: 46
		public abstract void Execute(XmlReader contextDocument, XmlResolver dataSources, XsltArgumentList argumentList, TextWriter results);

		// Token: 0x0600002F RID: 47
		public abstract void Execute(XmlReader contextDocument, XmlResolver dataSources, XsltArgumentList argumentList, Stream results);

		// Token: 0x06000030 RID: 48
		public abstract IList Evaluate(XmlReader contextDocument, XmlResolver dataSources, XsltArgumentList argumentList);
	}
}
