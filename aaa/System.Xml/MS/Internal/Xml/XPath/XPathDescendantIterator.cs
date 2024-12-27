using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000167 RID: 359
	internal class XPathDescendantIterator : XPathAxisIterator
	{
		// Token: 0x0600134A RID: 4938 RVA: 0x000536A1 File Offset: 0x000526A1
		public XPathDescendantIterator(XPathNavigator nav, XPathNodeType type, bool matchSelf)
			: base(nav, type, matchSelf)
		{
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x000536AC File Offset: 0x000526AC
		public XPathDescendantIterator(XPathNavigator nav, string name, string namespaceURI, bool matchSelf)
			: base(nav, name, namespaceURI, matchSelf)
		{
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x000536B9 File Offset: 0x000526B9
		public XPathDescendantIterator(XPathDescendantIterator it)
			: base(it)
		{
			this.level = it.level;
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x000536CE File Offset: 0x000526CE
		public override XPathNodeIterator Clone()
		{
			return new XPathDescendantIterator(this);
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x000536D8 File Offset: 0x000526D8
		public override bool MoveNext()
		{
			if (this.first)
			{
				this.first = false;
				if (this.matchSelf && this.Matches)
				{
					this.position = 1;
					return true;
				}
			}
			for (;;)
			{
				if (!this.nav.MoveToFirstChild())
				{
					while (this.level != 0)
					{
						if (this.nav.MoveToNext())
						{
							goto IL_0078;
						}
						this.nav.MoveToParent();
						this.level--;
					}
					break;
				}
				this.level++;
				IL_0078:
				if (this.Matches)
				{
					goto Block_7;
				}
			}
			return false;
			Block_7:
			this.position++;
			return true;
		}

		// Token: 0x04000BEE RID: 3054
		private int level;
	}
}
