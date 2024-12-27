using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x02000099 RID: 153
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct NodeRangeIterator
	{
		// Token: 0x06000767 RID: 1895 RVA: 0x00026740 File Offset: 0x00025740
		public void Create(XPathNavigator start, XmlNavigatorFilter filter, XPathNavigator end)
		{
			this.navCurrent = XmlQueryRuntime.SyncToNavigator(this.navCurrent, start);
			this.navEnd = XmlQueryRuntime.SyncToNavigator(this.navEnd, end);
			this.filter = filter;
			if (start.IsSamePosition(end))
			{
				this.state = ((!filter.IsFiltered(start)) ? NodeRangeIterator.IteratorState.HaveCurrentNoNext : NodeRangeIterator.IteratorState.NoNext);
				return;
			}
			this.state = ((!filter.IsFiltered(start)) ? NodeRangeIterator.IteratorState.HaveCurrent : NodeRangeIterator.IteratorState.NeedCurrent);
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x000267A8 File Offset: 0x000257A8
		public bool MoveNext()
		{
			switch (this.state)
			{
			case NodeRangeIterator.IteratorState.HaveCurrent:
				this.state = NodeRangeIterator.IteratorState.NeedCurrent;
				return true;
			case NodeRangeIterator.IteratorState.NeedCurrent:
				if (!this.filter.MoveToFollowing(this.navCurrent, this.navEnd))
				{
					if (this.filter.IsFiltered(this.navEnd))
					{
						this.state = NodeRangeIterator.IteratorState.NoNext;
						return false;
					}
					this.navCurrent.MoveTo(this.navEnd);
					this.state = NodeRangeIterator.IteratorState.NoNext;
				}
				return true;
			case NodeRangeIterator.IteratorState.HaveCurrentNoNext:
				this.state = NodeRangeIterator.IteratorState.NoNext;
				return true;
			default:
				return false;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000769 RID: 1897 RVA: 0x00026833 File Offset: 0x00025833
		public XPathNavigator Current
		{
			get
			{
				return this.navCurrent;
			}
		}

		// Token: 0x04000505 RID: 1285
		private XmlNavigatorFilter filter;

		// Token: 0x04000506 RID: 1286
		private XPathNavigator navCurrent;

		// Token: 0x04000507 RID: 1287
		private XPathNavigator navEnd;

		// Token: 0x04000508 RID: 1288
		private NodeRangeIterator.IteratorState state;

		// Token: 0x0200009A RID: 154
		private enum IteratorState
		{
			// Token: 0x0400050A RID: 1290
			HaveCurrent,
			// Token: 0x0400050B RID: 1291
			NeedCurrent,
			// Token: 0x0400050C RID: 1292
			HaveCurrentNoNext,
			// Token: 0x0400050D RID: 1293
			NoNext
		}
	}
}
