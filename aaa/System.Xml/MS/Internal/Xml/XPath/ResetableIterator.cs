using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200011E RID: 286
	internal abstract class ResetableIterator : XPathNodeIterator
	{
		// Token: 0x06001124 RID: 4388 RVA: 0x0004D3E3 File Offset: 0x0004C3E3
		public ResetableIterator()
		{
			this.count = -1;
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x0004D3F2 File Offset: 0x0004C3F2
		protected ResetableIterator(ResetableIterator other)
		{
			this.count = other.count;
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x0004D406 File Offset: 0x0004C406
		protected void ResetCount()
		{
			this.count = -1;
		}

		// Token: 0x06001127 RID: 4391
		public abstract void Reset();

		// Token: 0x06001128 RID: 4392 RVA: 0x0004D410 File Offset: 0x0004C410
		public virtual bool MoveToPosition(int pos)
		{
			this.Reset();
			for (int i = this.CurrentPosition; i < pos; i++)
			{
				if (!this.MoveNext())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06001129 RID: 4393
		public abstract override int CurrentPosition { get; }
	}
}
