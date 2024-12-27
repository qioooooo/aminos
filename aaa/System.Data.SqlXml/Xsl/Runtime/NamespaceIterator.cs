using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x0200006F RID: 111
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct NamespaceIterator
	{
		// Token: 0x060006D9 RID: 1753 RVA: 0x000249E0 File Offset: 0x000239E0
		public void Create(XPathNavigator context)
		{
			this.navStack.Reset();
			if (context.MoveToFirstNamespace(XPathNamespaceScope.All))
			{
				do
				{
					if (context.LocalName.Length != 0 || context.Value.Length != 0)
					{
						this.navStack.Push(context.Clone());
					}
				}
				while (context.MoveToNextNamespace(XPathNamespaceScope.All));
				context.MoveToParent();
			}
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x00024A3C File Offset: 0x00023A3C
		public bool MoveNext()
		{
			if (this.navStack.IsEmpty)
			{
				return false;
			}
			this.navCurrent = this.navStack.Pop();
			return true;
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x00024A5F File Offset: 0x00023A5F
		public XPathNavigator Current
		{
			get
			{
				return this.navCurrent;
			}
		}

		// Token: 0x04000444 RID: 1092
		private XPathNavigator navCurrent;

		// Token: 0x04000445 RID: 1093
		private XmlNavigatorStack navStack;
	}
}
