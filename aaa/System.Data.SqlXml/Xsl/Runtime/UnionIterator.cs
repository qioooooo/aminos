using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x02000080 RID: 128
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct UnionIterator
	{
		// Token: 0x06000724 RID: 1828 RVA: 0x00025911 File Offset: 0x00024911
		public void Create(XmlQueryRuntime runtime)
		{
			this.runtime = runtime;
			this.state = UnionIterator.IteratorState.InitLeft;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00025924 File Offset: 0x00024924
		public SetIteratorResult MoveNext(XPathNavigator nestedNavigator)
		{
			switch (this.state)
			{
			case UnionIterator.IteratorState.InitLeft:
				this.navOther = nestedNavigator;
				this.state = UnionIterator.IteratorState.NeedRight;
				return SetIteratorResult.InitRightIterator;
			case UnionIterator.IteratorState.NeedLeft:
				this.navCurr = nestedNavigator;
				this.state = UnionIterator.IteratorState.LeftIsCurrent;
				break;
			case UnionIterator.IteratorState.NeedRight:
				this.navCurr = nestedNavigator;
				this.state = UnionIterator.IteratorState.RightIsCurrent;
				break;
			case UnionIterator.IteratorState.LeftIsCurrent:
				this.state = UnionIterator.IteratorState.NeedLeft;
				return SetIteratorResult.NeedLeftNode;
			case UnionIterator.IteratorState.RightIsCurrent:
				this.state = UnionIterator.IteratorState.NeedRight;
				return SetIteratorResult.NeedRightNode;
			}
			if (this.navCurr == null)
			{
				if (this.navOther == null)
				{
					return SetIteratorResult.NoMoreNodes;
				}
				this.Swap();
			}
			else if (this.navOther != null)
			{
				int num = this.runtime.ComparePosition(this.navOther, this.navCurr);
				if (num == 0)
				{
					if (this.state == UnionIterator.IteratorState.LeftIsCurrent)
					{
						this.state = UnionIterator.IteratorState.NeedLeft;
						return SetIteratorResult.NeedLeftNode;
					}
					this.state = UnionIterator.IteratorState.NeedRight;
					return SetIteratorResult.NeedRightNode;
				}
				else if (num < 0)
				{
					this.Swap();
				}
			}
			return SetIteratorResult.HaveCurrentNode;
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x000259F9 File Offset: 0x000249F9
		public XPathNavigator Current
		{
			get
			{
				return this.navCurr;
			}
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00025A04 File Offset: 0x00024A04
		private void Swap()
		{
			XPathNavigator xpathNavigator = this.navCurr;
			this.navCurr = this.navOther;
			this.navOther = xpathNavigator;
			if (this.state == UnionIterator.IteratorState.LeftIsCurrent)
			{
				this.state = UnionIterator.IteratorState.RightIsCurrent;
				return;
			}
			this.state = UnionIterator.IteratorState.LeftIsCurrent;
		}

		// Token: 0x040004A3 RID: 1187
		private XmlQueryRuntime runtime;

		// Token: 0x040004A4 RID: 1188
		private XPathNavigator navCurr;

		// Token: 0x040004A5 RID: 1189
		private XPathNavigator navOther;

		// Token: 0x040004A6 RID: 1190
		private UnionIterator.IteratorState state;

		// Token: 0x02000081 RID: 129
		private enum IteratorState
		{
			// Token: 0x040004A8 RID: 1192
			InitLeft,
			// Token: 0x040004A9 RID: 1193
			NeedLeft,
			// Token: 0x040004AA RID: 1194
			NeedRight,
			// Token: 0x040004AB RID: 1195
			LeftIsCurrent,
			// Token: 0x040004AC RID: 1196
			RightIsCurrent
		}
	}
}
