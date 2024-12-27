using System;
using System.Collections;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000096 RID: 150
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	internal class ArraySubsetEnumerator : IEnumerator
	{
		// Token: 0x0600056D RID: 1389 RVA: 0x00016D2C File Offset: 0x00015D2C
		public ArraySubsetEnumerator(Array array, int count)
		{
			this.array = array;
			this.total = count;
			this.current = -1;
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00016D49 File Offset: 0x00015D49
		public bool MoveNext()
		{
			if (this.current < this.total - 1)
			{
				this.current++;
				return true;
			}
			return false;
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00016D6C File Offset: 0x00015D6C
		public void Reset()
		{
			this.current = -1;
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x00016D75 File Offset: 0x00015D75
		public object Current
		{
			get
			{
				if (this.current == -1)
				{
					throw new InvalidOperationException();
				}
				return this.array.GetValue(this.current);
			}
		}

		// Token: 0x040008CB RID: 2251
		private Array array;

		// Token: 0x040008CC RID: 2252
		private int total;

		// Token: 0x040008CD RID: 2253
		private int current;
	}
}
