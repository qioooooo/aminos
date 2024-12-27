using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x0200008F RID: 143
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct AncestorIterator
	{
		// Token: 0x0600074A RID: 1866 RVA: 0x000260E8 File Offset: 0x000250E8
		public void Create(XPathNavigator context, XmlNavigatorFilter filter, bool orSelf)
		{
			this.filter = filter;
			this.navCurrent = XmlQueryRuntime.SyncToNavigator(this.navCurrent, context);
			this.haveCurrent = orSelf && !this.filter.IsFiltered(this.navCurrent);
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x00026123 File Offset: 0x00025123
		public bool MoveNext()
		{
			if (this.haveCurrent)
			{
				this.haveCurrent = false;
				return true;
			}
			while (this.navCurrent.MoveToParent())
			{
				if (!this.filter.IsFiltered(this.navCurrent))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600074C RID: 1868 RVA: 0x00026159 File Offset: 0x00025159
		public XPathNavigator Current
		{
			get
			{
				return this.navCurrent;
			}
		}

		// Token: 0x040004E2 RID: 1250
		private XmlNavigatorFilter filter;

		// Token: 0x040004E3 RID: 1251
		private XPathNavigator navCurrent;

		// Token: 0x040004E4 RID: 1252
		private bool haveCurrent;
	}
}
