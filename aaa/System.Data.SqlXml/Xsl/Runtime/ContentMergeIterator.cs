using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x02000071 RID: 113
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct ContentMergeIterator
	{
		// Token: 0x060006DF RID: 1759 RVA: 0x00024ABD File Offset: 0x00023ABD
		public void Create(XmlNavigatorFilter filter)
		{
			this.filter = filter;
			this.navStack.Reset();
			this.state = ContentMergeIterator.IteratorState.NeedCurrent;
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00024AD8 File Offset: 0x00023AD8
		public IteratorResult MoveNext(XPathNavigator input)
		{
			return this.MoveNext(input, true);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00024AE4 File Offset: 0x00023AE4
		internal IteratorResult MoveNext(XPathNavigator input, bool isContent)
		{
			switch (this.state)
			{
			case ContentMergeIterator.IteratorState.NeedCurrent:
				if (input == null)
				{
					return IteratorResult.NoMoreNodes;
				}
				this.navCurrent = XmlQueryRuntime.SyncToNavigator(this.navCurrent, input);
				if (isContent ? this.filter.MoveToContent(this.navCurrent) : this.filter.MoveToFollowingSibling(this.navCurrent))
				{
					this.state = ContentMergeIterator.IteratorState.HaveCurrentNeedNext;
				}
				return IteratorResult.NeedInputNode;
			case ContentMergeIterator.IteratorState.HaveCurrentNeedNext:
				if (input == null)
				{
					this.state = ContentMergeIterator.IteratorState.HaveCurrentNoNext;
					return IteratorResult.HaveCurrentNode;
				}
				this.navNext = XmlQueryRuntime.SyncToNavigator(this.navNext, input);
				if (isContent ? this.filter.MoveToContent(this.navNext) : this.filter.MoveToFollowingSibling(this.navNext))
				{
					this.state = ContentMergeIterator.IteratorState.HaveCurrentHaveNext;
					return this.DocOrderMerge();
				}
				return IteratorResult.NeedInputNode;
			case ContentMergeIterator.IteratorState.HaveCurrentNoNext:
			case ContentMergeIterator.IteratorState.HaveCurrentHaveNext:
				if (isContent ? (!this.filter.MoveToNextContent(this.navCurrent)) : (!this.filter.MoveToFollowingSibling(this.navCurrent)))
				{
					if (this.navStack.IsEmpty)
					{
						if (this.state == ContentMergeIterator.IteratorState.HaveCurrentNoNext)
						{
							return IteratorResult.NoMoreNodes;
						}
						this.navCurrent = XmlQueryRuntime.SyncToNavigator(this.navCurrent, this.navNext);
						this.state = ContentMergeIterator.IteratorState.HaveCurrentNeedNext;
						return IteratorResult.NeedInputNode;
					}
					else
					{
						this.navCurrent = this.navStack.Pop();
					}
				}
				if (this.state == ContentMergeIterator.IteratorState.HaveCurrentNoNext)
				{
					return IteratorResult.HaveCurrentNode;
				}
				return this.DocOrderMerge();
			default:
				return IteratorResult.NoMoreNodes;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x00024C3E File Offset: 0x00023C3E
		public XPathNavigator Current
		{
			get
			{
				return this.navCurrent;
			}
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x00024C48 File Offset: 0x00023C48
		private IteratorResult DocOrderMerge()
		{
			XmlNodeOrder xmlNodeOrder = this.navCurrent.ComparePosition(this.navNext);
			if (xmlNodeOrder == XmlNodeOrder.Before || xmlNodeOrder == XmlNodeOrder.Unknown)
			{
				return IteratorResult.HaveCurrentNode;
			}
			if (xmlNodeOrder == XmlNodeOrder.After)
			{
				this.navStack.Push(this.navCurrent);
				this.navCurrent = this.navNext;
				this.navNext = null;
			}
			this.state = ContentMergeIterator.IteratorState.HaveCurrentNeedNext;
			return IteratorResult.NeedInputNode;
		}

		// Token: 0x04000448 RID: 1096
		private XmlNavigatorFilter filter;

		// Token: 0x04000449 RID: 1097
		private XPathNavigator navCurrent;

		// Token: 0x0400044A RID: 1098
		private XPathNavigator navNext;

		// Token: 0x0400044B RID: 1099
		private XmlNavigatorStack navStack;

		// Token: 0x0400044C RID: 1100
		private ContentMergeIterator.IteratorState state;

		// Token: 0x02000072 RID: 114
		private enum IteratorState
		{
			// Token: 0x0400044E RID: 1102
			NeedCurrent,
			// Token: 0x0400044F RID: 1103
			HaveCurrentNeedNext,
			// Token: 0x04000450 RID: 1104
			HaveCurrentNoNext,
			// Token: 0x04000451 RID: 1105
			HaveCurrentHaveNext
		}
	}
}
