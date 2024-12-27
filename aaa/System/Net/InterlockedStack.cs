using System;
using System.Collections;

namespace System.Net
{
	// Token: 0x02000388 RID: 904
	internal sealed class InterlockedStack
	{
		// Token: 0x06001C27 RID: 7207 RVA: 0x0006A0BA File Offset: 0x000690BA
		internal InterlockedStack()
		{
		}

		// Token: 0x06001C28 RID: 7208 RVA: 0x0006A0D0 File Offset: 0x000690D0
		internal void Push(object pooledStream)
		{
			if (pooledStream == null)
			{
				throw new ArgumentNullException("pooledStream");
			}
			lock (this._stack.SyncRoot)
			{
				this._stack.Push(pooledStream);
				this._count = this._stack.Count;
			}
		}

		// Token: 0x06001C29 RID: 7209 RVA: 0x0006A134 File Offset: 0x00069134
		internal object Pop()
		{
			object obj2;
			lock (this._stack.SyncRoot)
			{
				object obj = null;
				if (0 < this._stack.Count)
				{
					obj = this._stack.Pop();
					this._count = this._stack.Count;
				}
				obj2 = obj;
			}
			return obj2;
		}

		// Token: 0x04001CC4 RID: 7364
		private readonly Stack _stack = new Stack();

		// Token: 0x04001CC5 RID: 7365
		private int _count;
	}
}
