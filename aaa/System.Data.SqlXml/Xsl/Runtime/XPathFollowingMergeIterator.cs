using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x02000092 RID: 146
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct XPathFollowingMergeIterator
	{
		// Token: 0x06000754 RID: 1876 RVA: 0x00026295 File Offset: 0x00025295
		public void Create(XmlNavigatorFilter filter)
		{
			this.filter = filter;
			this.state = XPathFollowingMergeIterator.IteratorState.NeedCandidateCurrent;
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x000262A8 File Offset: 0x000252A8
		public IteratorResult MoveNext(XPathNavigator input)
		{
			switch (this.state)
			{
			case XPathFollowingMergeIterator.IteratorState.NeedCandidateCurrent:
				break;
			case XPathFollowingMergeIterator.IteratorState.HaveCandidateCurrent:
				if (input == null)
				{
					this.state = XPathFollowingMergeIterator.IteratorState.HaveCurrentNoNext;
					return this.MoveFirst();
				}
				if (!this.navCurrent.IsDescendant(input))
				{
					this.state = XPathFollowingMergeIterator.IteratorState.HaveCurrentNeedNext;
					goto IL_0064;
				}
				break;
			case XPathFollowingMergeIterator.IteratorState.HaveCurrentNeedNext:
				goto IL_0064;
			default:
				if (!this.filter.MoveToFollowing(this.navCurrent, null))
				{
					return this.MoveFailed();
				}
				return IteratorResult.HaveCurrentNode;
			}
			if (input == null)
			{
				return IteratorResult.NoMoreNodes;
			}
			this.navCurrent = XmlQueryRuntime.SyncToNavigator(this.navCurrent, input);
			this.state = XPathFollowingMergeIterator.IteratorState.HaveCandidateCurrent;
			return IteratorResult.NeedInputNode;
			IL_0064:
			if (input == null)
			{
				this.state = XPathFollowingMergeIterator.IteratorState.HaveCurrentNoNext;
				return this.MoveFirst();
			}
			if (this.navCurrent.ComparePosition(input) != XmlNodeOrder.Unknown)
			{
				return IteratorResult.NeedInputNode;
			}
			this.navNext = XmlQueryRuntime.SyncToNavigator(this.navNext, input);
			this.state = XPathFollowingMergeIterator.IteratorState.HaveCurrentHaveNext;
			return this.MoveFirst();
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000756 RID: 1878 RVA: 0x00026377 File Offset: 0x00025377
		public XPathNavigator Current
		{
			get
			{
				return this.navCurrent;
			}
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00026380 File Offset: 0x00025380
		private IteratorResult MoveFailed()
		{
			if (this.state == XPathFollowingMergeIterator.IteratorState.HaveCurrentNoNext)
			{
				this.state = XPathFollowingMergeIterator.IteratorState.NeedCandidateCurrent;
				return IteratorResult.NoMoreNodes;
			}
			this.state = XPathFollowingMergeIterator.IteratorState.HaveCandidateCurrent;
			XPathNavigator xpathNavigator = this.navCurrent;
			this.navCurrent = this.navNext;
			this.navNext = xpathNavigator;
			return IteratorResult.NeedInputNode;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x000263C1 File Offset: 0x000253C1
		private IteratorResult MoveFirst()
		{
			if (!XPathFollowingIterator.MoveFirst(this.filter, this.navCurrent))
			{
				return this.MoveFailed();
			}
			return IteratorResult.HaveCurrentNode;
		}

		// Token: 0x040004EA RID: 1258
		private XmlNavigatorFilter filter;

		// Token: 0x040004EB RID: 1259
		private XPathFollowingMergeIterator.IteratorState state;

		// Token: 0x040004EC RID: 1260
		private XPathNavigator navCurrent;

		// Token: 0x040004ED RID: 1261
		private XPathNavigator navNext;

		// Token: 0x02000093 RID: 147
		private enum IteratorState
		{
			// Token: 0x040004EF RID: 1263
			NeedCandidateCurrent,
			// Token: 0x040004F0 RID: 1264
			HaveCandidateCurrent,
			// Token: 0x040004F1 RID: 1265
			HaveCurrentNeedNext,
			// Token: 0x040004F2 RID: 1266
			HaveCurrentHaveNext,
			// Token: 0x040004F3 RID: 1267
			HaveCurrentNoNext
		}
	}
}
