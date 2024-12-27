using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.Cache
{
	// Token: 0x02000104 RID: 260
	internal class XPathDocumentElementChildIterator : XPathDocumentBaseIterator
	{
		// Token: 0x06000FEE RID: 4078 RVA: 0x0004948A File Offset: 0x0004848A
		public XPathDocumentElementChildIterator(XPathDocumentNavigator parent, string name, string namespaceURI)
			: base(parent)
		{
			if (namespaceURI == null)
			{
				throw new ArgumentNullException("namespaceURI");
			}
			this.localName = parent.NameTable.Get(name);
			this.namespaceUri = namespaceURI;
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x000494BA File Offset: 0x000484BA
		public XPathDocumentElementChildIterator(XPathDocumentElementChildIterator iter)
			: base(iter)
		{
			this.localName = iter.localName;
			this.namespaceUri = iter.namespaceUri;
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x000494DB File Offset: 0x000484DB
		public override XPathNodeIterator Clone()
		{
			return new XPathDocumentElementChildIterator(this);
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x000494E4 File Offset: 0x000484E4
		public override bool MoveNext()
		{
			if (this.pos == 0)
			{
				if (!this.ctxt.MoveToChild(this.localName, this.namespaceUri))
				{
					return false;
				}
			}
			else if (!this.ctxt.MoveToNext(this.localName, this.namespaceUri))
			{
				return false;
			}
			this.pos++;
			return true;
		}

		// Token: 0x04000A8D RID: 2701
		private string localName;

		// Token: 0x04000A8E RID: 2702
		private string namespaceUri;
	}
}
