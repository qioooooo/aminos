using System;
using System.Threading;

namespace System.EnterpriseServices
{
	// Token: 0x02000032 RID: 50
	internal class InterlockedStack
	{
		// Token: 0x060000F1 RID: 241 RVA: 0x000049B8 File Offset: 0x000039B8
		public InterlockedStack()
		{
			this._head = null;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000049C8 File Offset: 0x000039C8
		public void Push(object o)
		{
			InterlockedStack.Node node = new InterlockedStack.Node(o);
			object head;
			do
			{
				head = this._head;
				node.Next = (InterlockedStack.Node)head;
			}
			while (Interlocked.CompareExchange(ref this._head, node, head) != head);
			Interlocked.Increment(ref this._count);
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00004A0B File Offset: 0x00003A0B
		public int Count
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00004A14 File Offset: 0x00003A14
		public object Pop()
		{
			object head;
			for (;;)
			{
				head = this._head;
				if (head == null)
				{
					break;
				}
				object next = ((InterlockedStack.Node)head).Next;
				if (Interlocked.CompareExchange(ref this._head, next, head) == head)
				{
					goto Block_1;
				}
			}
			return null;
			Block_1:
			Interlocked.Decrement(ref this._count);
			return ((InterlockedStack.Node)head).Object;
		}

		// Token: 0x0400006B RID: 107
		private object _head;

		// Token: 0x0400006C RID: 108
		private int _count;

		// Token: 0x02000033 RID: 51
		private class Node
		{
			// Token: 0x060000F5 RID: 245 RVA: 0x00004A60 File Offset: 0x00003A60
			public Node(object o)
			{
				this.Object = o;
				this.Next = null;
			}

			// Token: 0x0400006D RID: 109
			public object Object;

			// Token: 0x0400006E RID: 110
			public InterlockedStack.Node Next;
		}
	}
}
