using System;
using System.Runtime.InteropServices;

namespace System.Collections.Generic
{
	// Token: 0x02000231 RID: 561
	[ComVisible(false)]
	public sealed class LinkedListNode<T>
	{
		// Token: 0x060012C4 RID: 4804 RVA: 0x0003F28B File Offset: 0x0003E28B
		public LinkedListNode(T value)
		{
			this.item = value;
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x0003F29A File Offset: 0x0003E29A
		internal LinkedListNode(LinkedList<T> list, T value)
		{
			this.list = list;
			this.item = value;
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x060012C6 RID: 4806 RVA: 0x0003F2B0 File Offset: 0x0003E2B0
		public LinkedList<T> List
		{
			get
			{
				return this.list;
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x060012C7 RID: 4807 RVA: 0x0003F2B8 File Offset: 0x0003E2B8
		public LinkedListNode<T> Next
		{
			get
			{
				if (this.next != null && this.next != this.list.head)
				{
					return this.next;
				}
				return null;
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x060012C8 RID: 4808 RVA: 0x0003F2DD File Offset: 0x0003E2DD
		public LinkedListNode<T> Previous
		{
			get
			{
				if (this.prev != null && this != this.list.head)
				{
					return this.prev;
				}
				return null;
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x060012C9 RID: 4809 RVA: 0x0003F2FD File Offset: 0x0003E2FD
		// (set) Token: 0x060012CA RID: 4810 RVA: 0x0003F305 File Offset: 0x0003E305
		public T Value
		{
			get
			{
				return this.item;
			}
			set
			{
				this.item = value;
			}
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x0003F30E File Offset: 0x0003E30E
		internal void Invalidate()
		{
			this.list = null;
			this.next = null;
			this.prev = null;
		}

		// Token: 0x040010DD RID: 4317
		internal LinkedList<T> list;

		// Token: 0x040010DE RID: 4318
		internal LinkedListNode<T> next;

		// Token: 0x040010DF RID: 4319
		internal LinkedListNode<T> prev;

		// Token: 0x040010E0 RID: 4320
		internal T item;
	}
}
