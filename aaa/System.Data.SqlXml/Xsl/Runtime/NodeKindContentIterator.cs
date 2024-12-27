using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x0200006D RID: 109
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct NodeKindContentIterator
	{
		// Token: 0x060006D3 RID: 1747 RVA: 0x0002491E File Offset: 0x0002391E
		public void Create(XPathNavigator context, XPathNodeType nodeType)
		{
			this.navCurrent = XmlQueryRuntime.SyncToNavigator(this.navCurrent, context);
			this.nodeType = nodeType;
			this.needFirst = true;
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00024940 File Offset: 0x00023940
		public bool MoveNext()
		{
			if (this.needFirst)
			{
				this.needFirst = !this.navCurrent.MoveToChild(this.nodeType);
				return !this.needFirst;
			}
			return this.navCurrent.MoveToNext(this.nodeType);
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x0002497F File Offset: 0x0002397F
		public XPathNavigator Current
		{
			get
			{
				return this.navCurrent;
			}
		}

		// Token: 0x0400043F RID: 1087
		private XPathNodeType nodeType;

		// Token: 0x04000440 RID: 1088
		private XPathNavigator navCurrent;

		// Token: 0x04000441 RID: 1089
		private bool needFirst;
	}
}
