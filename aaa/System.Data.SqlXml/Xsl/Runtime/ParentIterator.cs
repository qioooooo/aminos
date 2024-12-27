using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x0200008E RID: 142
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct ParentIterator
	{
		// Token: 0x06000747 RID: 1863 RVA: 0x00026093 File Offset: 0x00025093
		public void Create(XPathNavigator context, XmlNavigatorFilter filter)
		{
			this.navCurrent = XmlQueryRuntime.SyncToNavigator(this.navCurrent, context);
			this.haveCurrent = this.navCurrent.MoveToParent() && !filter.IsFiltered(this.navCurrent);
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x000260CC File Offset: 0x000250CC
		public bool MoveNext()
		{
			if (this.haveCurrent)
			{
				this.haveCurrent = false;
				return true;
			}
			return false;
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x000260E0 File Offset: 0x000250E0
		public XPathNavigator Current
		{
			get
			{
				return this.navCurrent;
			}
		}

		// Token: 0x040004E0 RID: 1248
		private XPathNavigator navCurrent;

		// Token: 0x040004E1 RID: 1249
		private bool haveCurrent;
	}
}
