using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200015E RID: 350
	internal sealed class XPathSortComparer : IComparer<SortKey>
	{
		// Token: 0x060012FF RID: 4863 RVA: 0x000526A4 File Offset: 0x000516A4
		public XPathSortComparer(int size)
		{
			if (size <= 0)
			{
				size = 3;
			}
			this.expressions = new Query[size];
			this.comparers = new IComparer[size];
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x000526CB File Offset: 0x000516CB
		public XPathSortComparer()
			: this(3)
		{
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x000526D4 File Offset: 0x000516D4
		public void AddSort(Query evalQuery, IComparer comparer)
		{
			if (this.numSorts == this.expressions.Length)
			{
				Query[] array = new Query[this.numSorts * 2];
				IComparer[] array2 = new IComparer[this.numSorts * 2];
				for (int i = 0; i < this.numSorts; i++)
				{
					array[i] = this.expressions[i];
					array2[i] = this.comparers[i];
				}
				this.expressions = array;
				this.comparers = array2;
			}
			if (evalQuery.StaticType == XPathResultType.NodeSet || evalQuery.StaticType == XPathResultType.Any)
			{
				evalQuery = new StringFunctions(Function.FunctionType.FuncString, new Query[] { evalQuery });
			}
			this.expressions[this.numSorts] = evalQuery;
			this.comparers[this.numSorts] = comparer;
			this.numSorts++;
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06001302 RID: 4866 RVA: 0x00052792 File Offset: 0x00051792
		public int NumSorts
		{
			get
			{
				return this.numSorts;
			}
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x0005279A File Offset: 0x0005179A
		public Query Expression(int i)
		{
			return this.expressions[i];
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x000527A4 File Offset: 0x000517A4
		int IComparer<SortKey>.Compare(SortKey x, SortKey y)
		{
			for (int i = 0; i < x.NumKeys; i++)
			{
				int num = this.comparers[i].Compare(x[i], y[i]);
				if (num != 0)
				{
					return num;
				}
			}
			return x.OriginalPosition - y.OriginalPosition;
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x000527F4 File Offset: 0x000517F4
		internal XPathSortComparer Clone()
		{
			XPathSortComparer xpathSortComparer = new XPathSortComparer(this.numSorts);
			for (int i = 0; i < this.numSorts; i++)
			{
				xpathSortComparer.comparers[i] = this.comparers[i];
				xpathSortComparer.expressions[i] = (Query)this.expressions[i].Clone();
			}
			xpathSortComparer.numSorts = this.numSorts;
			return xpathSortComparer;
		}

		// Token: 0x04000BDA RID: 3034
		private const int minSize = 3;

		// Token: 0x04000BDB RID: 3035
		private Query[] expressions;

		// Token: 0x04000BDC RID: 3036
		private IComparer[] comparers;

		// Token: 0x04000BDD RID: 3037
		private int numSorts;
	}
}
