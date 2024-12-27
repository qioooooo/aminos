using System;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x02000084 RID: 132
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct DifferenceIterator
	{
		// Token: 0x0600072B RID: 1835 RVA: 0x00025B13 File Offset: 0x00024B13
		public void Create(XmlQueryRuntime runtime)
		{
			this.runtime = runtime;
			this.state = DifferenceIterator.IteratorState.InitLeft;
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00025B24 File Offset: 0x00024B24
		public SetIteratorResult MoveNext(XPathNavigator nestedNavigator)
		{
			switch (this.state)
			{
			case DifferenceIterator.IteratorState.InitLeft:
				this.navLeft = nestedNavigator;
				this.state = DifferenceIterator.IteratorState.NeedRight;
				return SetIteratorResult.InitRightIterator;
			case DifferenceIterator.IteratorState.NeedLeft:
				this.navLeft = nestedNavigator;
				break;
			case DifferenceIterator.IteratorState.NeedRight:
				this.navRight = nestedNavigator;
				break;
			case DifferenceIterator.IteratorState.NeedLeftAndRight:
				this.navLeft = nestedNavigator;
				this.state = DifferenceIterator.IteratorState.NeedRight;
				return SetIteratorResult.NeedRightNode;
			case DifferenceIterator.IteratorState.HaveCurrent:
				this.state = DifferenceIterator.IteratorState.NeedLeft;
				return SetIteratorResult.NeedLeftNode;
			}
			if (this.navLeft == null)
			{
				return SetIteratorResult.NoMoreNodes;
			}
			if (this.navRight != null)
			{
				int num = this.runtime.ComparePosition(this.navLeft, this.navRight);
				if (num == 0)
				{
					this.state = DifferenceIterator.IteratorState.NeedLeftAndRight;
					return SetIteratorResult.NeedLeftNode;
				}
				if (num > 0)
				{
					this.state = DifferenceIterator.IteratorState.NeedRight;
					return SetIteratorResult.NeedRightNode;
				}
			}
			this.state = DifferenceIterator.IteratorState.HaveCurrent;
			return SetIteratorResult.HaveCurrentNode;
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x00025BDA File Offset: 0x00024BDA
		public XPathNavigator Current
		{
			get
			{
				return this.navLeft;
			}
		}

		// Token: 0x040004B7 RID: 1207
		private XmlQueryRuntime runtime;

		// Token: 0x040004B8 RID: 1208
		private XPathNavigator navLeft;

		// Token: 0x040004B9 RID: 1209
		private XPathNavigator navRight;

		// Token: 0x040004BA RID: 1210
		private DifferenceIterator.IteratorState state;

		// Token: 0x02000085 RID: 133
		private enum IteratorState
		{
			// Token: 0x040004BC RID: 1212
			InitLeft,
			// Token: 0x040004BD RID: 1213
			NeedLeft,
			// Token: 0x040004BE RID: 1214
			NeedRight,
			// Token: 0x040004BF RID: 1215
			NeedLeftAndRight,
			// Token: 0x040004C0 RID: 1216
			HaveCurrent
		}
	}
}
