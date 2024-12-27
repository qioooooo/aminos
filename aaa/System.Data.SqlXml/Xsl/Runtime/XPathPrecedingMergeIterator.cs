using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x02000097 RID: 151
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct XPathPrecedingMergeIterator
	{
		// Token: 0x06000763 RID: 1891 RVA: 0x000265D7 File Offset: 0x000255D7
		public void Create(XmlNavigatorFilter filter)
		{
			this.filter = filter;
			this.state = XPathPrecedingMergeIterator.IteratorState.NeedCandidateCurrent;
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x000265E8 File Offset: 0x000255E8
		public IteratorResult MoveNext(XPathNavigator input)
		{
			switch (this.state)
			{
			case XPathPrecedingMergeIterator.IteratorState.NeedCandidateCurrent:
				if (input == null)
				{
					return IteratorResult.NoMoreNodes;
				}
				this.navCurrent = XmlQueryRuntime.SyncToNavigator(this.navCurrent, input);
				this.state = XPathPrecedingMergeIterator.IteratorState.HaveCandidateCurrent;
				return IteratorResult.NeedInputNode;
			case XPathPrecedingMergeIterator.IteratorState.HaveCandidateCurrent:
				if (input == null)
				{
					this.state = XPathPrecedingMergeIterator.IteratorState.HaveCurrentNoNext;
				}
				else
				{
					if (this.navCurrent.ComparePosition(input) != XmlNodeOrder.Unknown)
					{
						this.navCurrent = XmlQueryRuntime.SyncToNavigator(this.navCurrent, input);
						return IteratorResult.NeedInputNode;
					}
					this.navNext = XmlQueryRuntime.SyncToNavigator(this.navNext, input);
					this.state = XPathPrecedingMergeIterator.IteratorState.HaveCurrentHaveNext;
				}
				this.PushAncestors();
				break;
			}
			if (!this.navStack.IsEmpty)
			{
				while (!this.filter.MoveToFollowing(this.navCurrent, this.navStack.Peek()))
				{
					this.navCurrent.MoveTo(this.navStack.Pop());
					if (this.navStack.IsEmpty)
					{
						goto IL_00D6;
					}
				}
				return IteratorResult.HaveCurrentNode;
			}
			IL_00D6:
			if (this.state == XPathPrecedingMergeIterator.IteratorState.HaveCurrentNoNext)
			{
				this.state = XPathPrecedingMergeIterator.IteratorState.NeedCandidateCurrent;
				return IteratorResult.NoMoreNodes;
			}
			this.navCurrent = XmlQueryRuntime.SyncToNavigator(this.navCurrent, this.navNext);
			this.state = XPathPrecedingMergeIterator.IteratorState.HaveCandidateCurrent;
			return IteratorResult.HaveCurrentNode;
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000765 RID: 1893 RVA: 0x000266FC File Offset: 0x000256FC
		public XPathNavigator Current
		{
			get
			{
				return this.navCurrent;
			}
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x00026704 File Offset: 0x00025704
		private void PushAncestors()
		{
			this.navStack.Reset();
			do
			{
				this.navStack.Push(this.navCurrent.Clone());
			}
			while (this.navCurrent.MoveToParent());
			this.navStack.Pop();
		}

		// Token: 0x040004FB RID: 1275
		private XmlNavigatorFilter filter;

		// Token: 0x040004FC RID: 1276
		private XPathPrecedingMergeIterator.IteratorState state;

		// Token: 0x040004FD RID: 1277
		private XPathNavigator navCurrent;

		// Token: 0x040004FE RID: 1278
		private XPathNavigator navNext;

		// Token: 0x040004FF RID: 1279
		private XmlNavigatorStack navStack;

		// Token: 0x02000098 RID: 152
		private enum IteratorState
		{
			// Token: 0x04000501 RID: 1281
			NeedCandidateCurrent,
			// Token: 0x04000502 RID: 1282
			HaveCandidateCurrent,
			// Token: 0x04000503 RID: 1283
			HaveCurrentHaveNext,
			// Token: 0x04000504 RID: 1284
			HaveCurrentNoNext
		}
	}
}
