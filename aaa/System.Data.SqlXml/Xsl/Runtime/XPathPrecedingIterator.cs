using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x02000095 RID: 149
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct XPathPrecedingIterator
	{
		// Token: 0x0600075C RID: 1884 RVA: 0x0002649C File Offset: 0x0002549C
		public void Create(XPathNavigator context, XmlNavigatorFilter filter)
		{
			XPathPrecedingDocOrderIterator xpathPrecedingDocOrderIterator = default(XPathPrecedingDocOrderIterator);
			xpathPrecedingDocOrderIterator.Create(context, filter);
			this.stack.Reset();
			while (xpathPrecedingDocOrderIterator.MoveNext())
			{
				XPathNavigator xpathNavigator = xpathPrecedingDocOrderIterator.Current;
				this.stack.Push(xpathNavigator.Clone());
			}
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x000264E7 File Offset: 0x000254E7
		public bool MoveNext()
		{
			if (this.stack.IsEmpty)
			{
				return false;
			}
			this.navCurrent = this.stack.Pop();
			return true;
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x0002650A File Offset: 0x0002550A
		public XPathNavigator Current
		{
			get
			{
				return this.navCurrent;
			}
		}

		// Token: 0x040004F6 RID: 1270
		private XmlNavigatorStack stack;

		// Token: 0x040004F7 RID: 1271
		private XPathNavigator navCurrent;
	}
}
