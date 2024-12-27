using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x02000089 RID: 137
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct PrecedingSiblingDocOrderIterator
	{
		// Token: 0x06000737 RID: 1847 RVA: 0x00025C78 File Offset: 0x00024C78
		public void Create(XPathNavigator context, XmlNavigatorFilter filter)
		{
			this.filter = filter;
			this.navCurrent = XmlQueryRuntime.SyncToNavigator(this.navCurrent, context);
			this.navEnd = XmlQueryRuntime.SyncToNavigator(this.navEnd, context);
			this.needFirst = true;
			this.useCompPos = this.filter.IsFiltered(context);
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00025CCC File Offset: 0x00024CCC
		public bool MoveNext()
		{
			if (this.needFirst)
			{
				if (!this.navCurrent.MoveToParent())
				{
					return false;
				}
				if (!this.filter.MoveToContent(this.navCurrent))
				{
					return false;
				}
				this.needFirst = false;
			}
			else if (!this.filter.MoveToFollowingSibling(this.navCurrent))
			{
				return false;
			}
			if (this.useCompPos)
			{
				return this.navCurrent.ComparePosition(this.navEnd) == XmlNodeOrder.Before;
			}
			if (this.navCurrent.IsSamePosition(this.navEnd))
			{
				this.useCompPos = true;
				return false;
			}
			return true;
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000739 RID: 1849 RVA: 0x00025D5D File Offset: 0x00024D5D
		public XPathNavigator Current
		{
			get
			{
				return this.navCurrent;
			}
		}

		// Token: 0x040004C6 RID: 1222
		private XmlNavigatorFilter filter;

		// Token: 0x040004C7 RID: 1223
		private XPathNavigator navCurrent;

		// Token: 0x040004C8 RID: 1224
		private XPathNavigator navEnd;

		// Token: 0x040004C9 RID: 1225
		private bool needFirst;

		// Token: 0x040004CA RID: 1226
		private bool useCompPos;
	}
}
