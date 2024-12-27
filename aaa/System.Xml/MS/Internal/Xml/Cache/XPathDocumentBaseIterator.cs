using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.Cache
{
	// Token: 0x02000103 RID: 259
	internal abstract class XPathDocumentBaseIterator : XPathNodeIterator
	{
		// Token: 0x06000FEA RID: 4074 RVA: 0x00049441 File Offset: 0x00048441
		protected XPathDocumentBaseIterator(XPathDocumentNavigator ctxt)
		{
			this.ctxt = new XPathDocumentNavigator(ctxt);
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x00049455 File Offset: 0x00048455
		protected XPathDocumentBaseIterator(XPathDocumentBaseIterator iter)
		{
			this.ctxt = new XPathDocumentNavigator(iter.ctxt);
			this.pos = iter.pos;
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000FEC RID: 4076 RVA: 0x0004947A File Offset: 0x0004847A
		public override XPathNavigator Current
		{
			get
			{
				return this.ctxt;
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000FED RID: 4077 RVA: 0x00049482 File Offset: 0x00048482
		public override int CurrentPosition
		{
			get
			{
				return this.pos;
			}
		}

		// Token: 0x04000A8B RID: 2699
		protected XPathDocumentNavigator ctxt;

		// Token: 0x04000A8C RID: 2700
		protected int pos;
	}
}
