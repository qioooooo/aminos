using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x0200008C RID: 140
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct DescendantMergeIterator
	{
		// Token: 0x06000744 RID: 1860 RVA: 0x00025FBB File Offset: 0x00024FBB
		public void Create(XmlNavigatorFilter filter, bool orSelf)
		{
			this.filter = filter;
			this.state = DescendantMergeIterator.IteratorState.NoPrevious;
			this.orSelf = orSelf;
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x00025FD4 File Offset: 0x00024FD4
		public IteratorResult MoveNext(XPathNavigator input)
		{
			if (this.state != DescendantMergeIterator.IteratorState.NeedDescendant)
			{
				if (input == null)
				{
					return IteratorResult.NoMoreNodes;
				}
				if (this.state != DescendantMergeIterator.IteratorState.NoPrevious && this.navRoot.IsDescendant(input))
				{
					return IteratorResult.NeedInputNode;
				}
				this.navCurrent = XmlQueryRuntime.SyncToNavigator(this.navCurrent, input);
				this.navRoot = XmlQueryRuntime.SyncToNavigator(this.navRoot, input);
				this.navEnd = XmlQueryRuntime.SyncToNavigator(this.navEnd, input);
				this.navEnd.MoveToNonDescendant();
				this.state = DescendantMergeIterator.IteratorState.NeedDescendant;
				if (this.orSelf && !this.filter.IsFiltered(input))
				{
					return IteratorResult.HaveCurrentNode;
				}
			}
			if (this.filter.MoveToFollowing(this.navCurrent, this.navEnd))
			{
				return IteratorResult.HaveCurrentNode;
			}
			this.state = DescendantMergeIterator.IteratorState.NeedCurrent;
			return IteratorResult.NeedInputNode;
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000746 RID: 1862 RVA: 0x0002608B File Offset: 0x0002508B
		public XPathNavigator Current
		{
			get
			{
				return this.navCurrent;
			}
		}

		// Token: 0x040004D6 RID: 1238
		private XmlNavigatorFilter filter;

		// Token: 0x040004D7 RID: 1239
		private XPathNavigator navCurrent;

		// Token: 0x040004D8 RID: 1240
		private XPathNavigator navRoot;

		// Token: 0x040004D9 RID: 1241
		private XPathNavigator navEnd;

		// Token: 0x040004DA RID: 1242
		private DescendantMergeIterator.IteratorState state;

		// Token: 0x040004DB RID: 1243
		private bool orSelf;

		// Token: 0x0200008D RID: 141
		private enum IteratorState
		{
			// Token: 0x040004DD RID: 1245
			NoPrevious,
			// Token: 0x040004DE RID: 1246
			NeedCurrent,
			// Token: 0x040004DF RID: 1247
			NeedDescendant
		}
	}
}
