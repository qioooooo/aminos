using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x02000070 RID: 112
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct AttributeContentIterator
	{
		// Token: 0x060006DC RID: 1756 RVA: 0x00024A67 File Offset: 0x00023A67
		public void Create(XPathNavigator context)
		{
			this.navCurrent = XmlQueryRuntime.SyncToNavigator(this.navCurrent, context);
			this.needFirst = true;
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x00024A82 File Offset: 0x00023A82
		public bool MoveNext()
		{
			if (this.needFirst)
			{
				this.needFirst = !XmlNavNeverFilter.MoveToFirstAttributeContent(this.navCurrent);
				return !this.needFirst;
			}
			return XmlNavNeverFilter.MoveToNextAttributeContent(this.navCurrent);
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x00024AB5 File Offset: 0x00023AB5
		public XPathNavigator Current
		{
			get
			{
				return this.navCurrent;
			}
		}

		// Token: 0x04000446 RID: 1094
		private XPathNavigator navCurrent;

		// Token: 0x04000447 RID: 1095
		private bool needFirst;
	}
}
