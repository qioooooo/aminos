using System;
using System.Collections;

namespace System.Diagnostics
{
	// Token: 0x0200078C RID: 1932
	public class ProcessThreadCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06003BAF RID: 15279 RVA: 0x000FE170 File Offset: 0x000FD170
		protected ProcessThreadCollection()
		{
		}

		// Token: 0x06003BB0 RID: 15280 RVA: 0x000FE178 File Offset: 0x000FD178
		public ProcessThreadCollection(ProcessThread[] processThreads)
		{
			base.InnerList.AddRange(processThreads);
		}

		// Token: 0x17000E0B RID: 3595
		public ProcessThread this[int index]
		{
			get
			{
				return (ProcessThread)base.InnerList[index];
			}
		}

		// Token: 0x06003BB2 RID: 15282 RVA: 0x000FE19F File Offset: 0x000FD19F
		public int Add(ProcessThread thread)
		{
			return base.InnerList.Add(thread);
		}

		// Token: 0x06003BB3 RID: 15283 RVA: 0x000FE1AD File Offset: 0x000FD1AD
		public void Insert(int index, ProcessThread thread)
		{
			base.InnerList.Insert(index, thread);
		}

		// Token: 0x06003BB4 RID: 15284 RVA: 0x000FE1BC File Offset: 0x000FD1BC
		public int IndexOf(ProcessThread thread)
		{
			return base.InnerList.IndexOf(thread);
		}

		// Token: 0x06003BB5 RID: 15285 RVA: 0x000FE1CA File Offset: 0x000FD1CA
		public bool Contains(ProcessThread thread)
		{
			return base.InnerList.Contains(thread);
		}

		// Token: 0x06003BB6 RID: 15286 RVA: 0x000FE1D8 File Offset: 0x000FD1D8
		public void Remove(ProcessThread thread)
		{
			base.InnerList.Remove(thread);
		}

		// Token: 0x06003BB7 RID: 15287 RVA: 0x000FE1E6 File Offset: 0x000FD1E6
		public void CopyTo(ProcessThread[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}
	}
}
