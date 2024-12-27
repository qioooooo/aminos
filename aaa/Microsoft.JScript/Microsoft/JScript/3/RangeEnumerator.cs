using System;
using System.Collections;

namespace Microsoft.JScript
{
	// Token: 0x020000FD RID: 253
	internal class RangeEnumerator : IEnumerator
	{
		// Token: 0x06000AE7 RID: 2791 RVA: 0x000546B6 File Offset: 0x000536B6
		internal RangeEnumerator(int start, int stop)
		{
			this.curr = start - 1;
			this.start = start;
			this.stop = stop;
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x000546D8 File Offset: 0x000536D8
		public virtual bool MoveNext()
		{
			return ++this.curr <= this.stop;
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000AE9 RID: 2793 RVA: 0x00054701 File Offset: 0x00053701
		public virtual object Current
		{
			get
			{
				return this.curr;
			}
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x0005470E File Offset: 0x0005370E
		public virtual void Reset()
		{
			this.curr = this.start;
		}

		// Token: 0x040006AA RID: 1706
		private int curr;

		// Token: 0x040006AB RID: 1707
		private int start;

		// Token: 0x040006AC RID: 1708
		private int stop;
	}
}
