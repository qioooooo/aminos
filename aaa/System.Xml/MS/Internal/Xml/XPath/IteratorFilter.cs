using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000145 RID: 325
	internal class IteratorFilter : XPathNodeIterator
	{
		// Token: 0x06001244 RID: 4676 RVA: 0x0004FEAF File Offset: 0x0004EEAF
		internal IteratorFilter(XPathNodeIterator innerIterator, string name)
		{
			this.innerIterator = innerIterator;
			this.name = name;
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x0004FEC5 File Offset: 0x0004EEC5
		private IteratorFilter(IteratorFilter it)
		{
			this.innerIterator = it.innerIterator.Clone();
			this.name = it.name;
			this.position = it.position;
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x0004FEF6 File Offset: 0x0004EEF6
		public override XPathNodeIterator Clone()
		{
			return new IteratorFilter(this);
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06001247 RID: 4679 RVA: 0x0004FEFE File Offset: 0x0004EEFE
		public override XPathNavigator Current
		{
			get
			{
				return this.innerIterator.Current;
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06001248 RID: 4680 RVA: 0x0004FF0B File Offset: 0x0004EF0B
		public override int CurrentPosition
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x0004FF13 File Offset: 0x0004EF13
		public override bool MoveNext()
		{
			while (this.innerIterator.MoveNext())
			{
				if (this.innerIterator.Current.LocalName == this.name)
				{
					this.position++;
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000B88 RID: 2952
		private XPathNodeIterator innerIterator;

		// Token: 0x04000B89 RID: 2953
		private string name;

		// Token: 0x04000B8A RID: 2954
		private int position;
	}
}
