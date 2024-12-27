using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Web.Services.Description
{
	// Token: 0x02000130 RID: 304
	public class BasicProfileViolationEnumerator : IEnumerator<BasicProfileViolation>, IDisposable, IEnumerator
	{
		// Token: 0x06000956 RID: 2390 RVA: 0x00044ED3 File Offset: 0x00043ED3
		public BasicProfileViolationEnumerator(BasicProfileViolationCollection list)
		{
			this.list = list;
			this.idx = -1;
			this.end = list.Count - 1;
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x00044EF7 File Offset: 0x00043EF7
		public void Dispose()
		{
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x00044EF9 File Offset: 0x00043EF9
		public bool MoveNext()
		{
			if (this.idx >= this.end)
			{
				return false;
			}
			this.idx++;
			return true;
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000959 RID: 2393 RVA: 0x00044F1A File Offset: 0x00043F1A
		public BasicProfileViolation Current
		{
			get
			{
				return this.list[this.idx];
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x0600095A RID: 2394 RVA: 0x00044F2D File Offset: 0x00043F2D
		object IEnumerator.Current
		{
			get
			{
				return this.list[this.idx];
			}
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x00044F40 File Offset: 0x00043F40
		void IEnumerator.Reset()
		{
			this.idx = -1;
		}

		// Token: 0x040005FB RID: 1531
		private BasicProfileViolationCollection list;

		// Token: 0x040005FC RID: 1532
		private int idx;

		// Token: 0x040005FD RID: 1533
		private int end;
	}
}
