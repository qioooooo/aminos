using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x02000082 RID: 130
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct IntersectIterator
	{
		// Token: 0x06000728 RID: 1832 RVA: 0x00025A43 File Offset: 0x00024A43
		public void Create(XmlQueryRuntime runtime)
		{
			this.runtime = runtime;
			this.state = IntersectIterator.IteratorState.InitLeft;
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00025A54 File Offset: 0x00024A54
		public SetIteratorResult MoveNext(XPathNavigator nestedNavigator)
		{
			switch (this.state)
			{
			case IntersectIterator.IteratorState.InitLeft:
				this.navLeft = nestedNavigator;
				this.state = IntersectIterator.IteratorState.NeedRight;
				return SetIteratorResult.InitRightIterator;
			case IntersectIterator.IteratorState.NeedLeft:
				this.navLeft = nestedNavigator;
				break;
			case IntersectIterator.IteratorState.NeedRight:
				this.navRight = nestedNavigator;
				break;
			case IntersectIterator.IteratorState.NeedLeftAndRight:
				this.navLeft = nestedNavigator;
				this.state = IntersectIterator.IteratorState.NeedRight;
				return SetIteratorResult.NeedRightNode;
			case IntersectIterator.IteratorState.HaveCurrent:
				this.state = IntersectIterator.IteratorState.NeedLeftAndRight;
				return SetIteratorResult.NeedLeftNode;
			}
			if (this.navLeft == null || this.navRight == null)
			{
				return SetIteratorResult.NoMoreNodes;
			}
			int num = this.runtime.ComparePosition(this.navLeft, this.navRight);
			if (num < 0)
			{
				this.state = IntersectIterator.IteratorState.NeedLeft;
				return SetIteratorResult.NeedLeftNode;
			}
			if (num > 0)
			{
				this.state = IntersectIterator.IteratorState.NeedRight;
				return SetIteratorResult.NeedRightNode;
			}
			this.state = IntersectIterator.IteratorState.HaveCurrent;
			return SetIteratorResult.HaveCurrentNode;
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600072A RID: 1834 RVA: 0x00025B0B File Offset: 0x00024B0B
		public XPathNavigator Current
		{
			get
			{
				return this.navLeft;
			}
		}

		// Token: 0x040004AD RID: 1197
		private XmlQueryRuntime runtime;

		// Token: 0x040004AE RID: 1198
		private XPathNavigator navLeft;

		// Token: 0x040004AF RID: 1199
		private XPathNavigator navRight;

		// Token: 0x040004B0 RID: 1200
		private IntersectIterator.IteratorState state;

		// Token: 0x02000083 RID: 131
		private enum IteratorState
		{
			// Token: 0x040004B2 RID: 1202
			InitLeft,
			// Token: 0x040004B3 RID: 1203
			NeedLeft,
			// Token: 0x040004B4 RID: 1204
			NeedRight,
			// Token: 0x040004B5 RID: 1205
			NeedLeftAndRight,
			// Token: 0x040004B6 RID: 1206
			HaveCurrent
		}
	}
}
