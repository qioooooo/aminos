using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x0200006E RID: 110
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct AttributeIterator
	{
		// Token: 0x060006D6 RID: 1750 RVA: 0x00024987 File Offset: 0x00023987
		public void Create(XPathNavigator context)
		{
			this.navCurrent = XmlQueryRuntime.SyncToNavigator(this.navCurrent, context);
			this.needFirst = true;
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x000249A2 File Offset: 0x000239A2
		public bool MoveNext()
		{
			if (this.needFirst)
			{
				this.needFirst = !this.navCurrent.MoveToFirstAttribute();
				return !this.needFirst;
			}
			return this.navCurrent.MoveToNextAttribute();
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060006D8 RID: 1752 RVA: 0x000249D5 File Offset: 0x000239D5
		public XPathNavigator Current
		{
			get
			{
				return this.navCurrent;
			}
		}

		// Token: 0x04000442 RID: 1090
		private XPathNavigator navCurrent;

		// Token: 0x04000443 RID: 1091
		private bool needFirst;
	}
}
