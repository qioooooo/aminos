using System;
using System.Collections;

namespace Microsoft.JScript
{
	// Token: 0x020000E2 RID: 226
	internal class ListEnumerator : IEnumerator
	{
		// Token: 0x06000A0C RID: 2572 RVA: 0x0004BEDC File Offset: 0x0004AEDC
		internal ListEnumerator(ArrayList list)
		{
			this.curr = -1;
			this.list = list;
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x0004BEF4 File Offset: 0x0004AEF4
		public virtual bool MoveNext()
		{
			return ++this.curr < this.list.Count;
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000A0E RID: 2574 RVA: 0x0004BF1F File Offset: 0x0004AF1F
		public virtual object Current
		{
			get
			{
				return this.list[this.curr];
			}
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x0004BF32 File Offset: 0x0004AF32
		public virtual void Reset()
		{
			this.curr = -1;
		}

		// Token: 0x04000659 RID: 1625
		private int curr;

		// Token: 0x0400065A RID: 1626
		private ArrayList list;
	}
}
