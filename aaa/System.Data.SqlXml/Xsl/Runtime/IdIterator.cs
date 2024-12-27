using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000AA RID: 170
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct IdIterator
	{
		// Token: 0x060007F6 RID: 2038 RVA: 0x00028629 File Offset: 0x00027629
		public void Create(XPathNavigator context, string value)
		{
			this.navCurrent = XmlQueryRuntime.SyncToNavigator(this.navCurrent, context);
			this.idrefs = XmlConvert.SplitString(value);
			this.idx = -1;
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x00028650 File Offset: 0x00027650
		public bool MoveNext()
		{
			for (;;)
			{
				this.idx++;
				if (this.idx >= this.idrefs.Length)
				{
					break;
				}
				if (this.navCurrent.MoveToId(this.idrefs[this.idx]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x0002868D File Offset: 0x0002768D
		public XPathNavigator Current
		{
			get
			{
				return this.navCurrent;
			}
		}

		// Token: 0x04000566 RID: 1382
		private XPathNavigator navCurrent;

		// Token: 0x04000567 RID: 1383
		private string[] idrefs;

		// Token: 0x04000568 RID: 1384
		private int idx;
	}
}
