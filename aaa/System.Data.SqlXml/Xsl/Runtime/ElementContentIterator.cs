using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x0200006C RID: 108
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct ElementContentIterator
	{
		// Token: 0x060006D0 RID: 1744 RVA: 0x00024897 File Offset: 0x00023897
		public void Create(XPathNavigator context, string localName, string ns)
		{
			this.navCurrent = XmlQueryRuntime.SyncToNavigator(this.navCurrent, context);
			this.localName = localName;
			this.ns = ns;
			this.needFirst = true;
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x000248C0 File Offset: 0x000238C0
		public bool MoveNext()
		{
			if (this.needFirst)
			{
				this.needFirst = !this.navCurrent.MoveToChild(this.localName, this.ns);
				return !this.needFirst;
			}
			return this.navCurrent.MoveToNext(this.localName, this.ns);
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060006D2 RID: 1746 RVA: 0x00024916 File Offset: 0x00023916
		public XPathNavigator Current
		{
			get
			{
				return this.navCurrent;
			}
		}

		// Token: 0x0400043B RID: 1083
		private string localName;

		// Token: 0x0400043C RID: 1084
		private string ns;

		// Token: 0x0400043D RID: 1085
		private XPathNavigator navCurrent;

		// Token: 0x0400043E RID: 1086
		private bool needFirst;
	}
}
