using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x02000090 RID: 144
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct AncestorDocOrderIterator
	{
		// Token: 0x0600074D RID: 1869 RVA: 0x00026164 File Offset: 0x00025164
		public void Create(XPathNavigator context, XmlNavigatorFilter filter, bool orSelf)
		{
			AncestorIterator ancestorIterator = default(AncestorIterator);
			ancestorIterator.Create(context, filter, orSelf);
			this.stack.Reset();
			while (ancestorIterator.MoveNext())
			{
				XPathNavigator xpathNavigator = ancestorIterator.Current;
				this.stack.Push(xpathNavigator.Clone());
			}
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x000261B0 File Offset: 0x000251B0
		public bool MoveNext()
		{
			if (this.stack.IsEmpty)
			{
				return false;
			}
			this.navCurrent = this.stack.Pop();
			return true;
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600074F RID: 1871 RVA: 0x000261D3 File Offset: 0x000251D3
		public XPathNavigator Current
		{
			get
			{
				return this.navCurrent;
			}
		}

		// Token: 0x040004E5 RID: 1253
		private XmlNavigatorStack stack;

		// Token: 0x040004E6 RID: 1254
		private XPathNavigator navCurrent;
	}
}
