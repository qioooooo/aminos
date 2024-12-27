using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x0200006B RID: 107
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct ContentIterator
	{
		// Token: 0x060006CD RID: 1741 RVA: 0x00024841 File Offset: 0x00023841
		public void Create(XPathNavigator context)
		{
			this.navCurrent = XmlQueryRuntime.SyncToNavigator(this.navCurrent, context);
			this.needFirst = true;
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0002485C File Offset: 0x0002385C
		public bool MoveNext()
		{
			if (this.needFirst)
			{
				this.needFirst = !this.navCurrent.MoveToFirstChild();
				return !this.needFirst;
			}
			return this.navCurrent.MoveToNext();
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x0002488F File Offset: 0x0002388F
		public XPathNavigator Current
		{
			get
			{
				return this.navCurrent;
			}
		}

		// Token: 0x04000439 RID: 1081
		private XPathNavigator navCurrent;

		// Token: 0x0400043A RID: 1082
		private bool needFirst;
	}
}
