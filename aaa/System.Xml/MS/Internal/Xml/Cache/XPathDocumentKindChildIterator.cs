using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.Cache
{
	// Token: 0x02000105 RID: 261
	internal class XPathDocumentKindChildIterator : XPathDocumentBaseIterator
	{
		// Token: 0x06000FF2 RID: 4082 RVA: 0x0004953E File Offset: 0x0004853E
		public XPathDocumentKindChildIterator(XPathDocumentNavigator parent, XPathNodeType typ)
			: base(parent)
		{
			this.typ = typ;
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x0004954E File Offset: 0x0004854E
		public XPathDocumentKindChildIterator(XPathDocumentKindChildIterator iter)
			: base(iter)
		{
			this.typ = iter.typ;
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x00049563 File Offset: 0x00048563
		public override XPathNodeIterator Clone()
		{
			return new XPathDocumentKindChildIterator(this);
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0004956C File Offset: 0x0004856C
		public override bool MoveNext()
		{
			if (this.pos == 0)
			{
				if (!this.ctxt.MoveToChild(this.typ))
				{
					return false;
				}
			}
			else if (!this.ctxt.MoveToNext(this.typ))
			{
				return false;
			}
			this.pos++;
			return true;
		}

		// Token: 0x04000A8F RID: 2703
		private XPathNodeType typ;
	}
}
