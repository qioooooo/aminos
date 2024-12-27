using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x0200022A RID: 554
	internal sealed class System_QueueDebugView<T>
	{
		// Token: 0x0600128B RID: 4747 RVA: 0x0003E5A4 File Offset: 0x0003D5A4
		public System_QueueDebugView(Queue<T> queue)
		{
			if (queue == null)
			{
				throw new ArgumentNullException("queue");
			}
			this.queue = queue;
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x0600128C RID: 4748 RVA: 0x0003E5C1 File Offset: 0x0003D5C1
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items
		{
			get
			{
				return this.queue.ToArray();
			}
		}

		// Token: 0x040010C6 RID: 4294
		private Queue<T> queue;
	}
}
