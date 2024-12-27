using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Collections
{
	// Token: 0x0200025F RID: 607
	[ComVisible(true)]
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(Queue.QueueDebugView))]
	[Serializable]
	public class Queue : ICollection, IEnumerable, ICloneable
	{
		// Token: 0x0600184E RID: 6222 RVA: 0x0003F6E0 File Offset: 0x0003E6E0
		public Queue()
			: this(32, 2f)
		{
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x0003F6EF File Offset: 0x0003E6EF
		public Queue(int capacity)
			: this(capacity, 2f)
		{
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x0003F700 File Offset: 0x0003E700
		public Queue(int capacity, float growFactor)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if ((double)growFactor < 1.0 || (double)growFactor > 10.0)
			{
				throw new ArgumentOutOfRangeException("growFactor", Environment.GetResourceString("ArgumentOutOfRange_QueueGrowFactor", new object[] { 1, 10 }));
			}
			this._array = new object[capacity];
			this._head = 0;
			this._tail = 0;
			this._size = 0;
			this._growFactor = (int)(growFactor * 100f);
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x0003F7A8 File Offset: 0x0003E7A8
		public Queue(ICollection col)
			: this((col == null) ? 32 : col.Count)
		{
			if (col == null)
			{
				throw new ArgumentNullException("col");
			}
			foreach (object obj in col)
			{
				this.Enqueue(obj);
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06001852 RID: 6226 RVA: 0x0003F7F3 File Offset: 0x0003E7F3
		public virtual int Count
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x06001853 RID: 6227 RVA: 0x0003F7FC File Offset: 0x0003E7FC
		public virtual object Clone()
		{
			Queue queue = new Queue(this._size);
			queue._size = this._size;
			int num = this._size;
			int num2 = ((this._array.Length - this._head < num) ? (this._array.Length - this._head) : num);
			Array.Copy(this._array, this._head, queue._array, 0, num2);
			num -= num2;
			if (num > 0)
			{
				Array.Copy(this._array, 0, queue._array, this._array.Length - this._head, num);
			}
			queue._version = this._version;
			return queue;
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06001854 RID: 6228 RVA: 0x0003F89D File Offset: 0x0003E89D
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06001855 RID: 6229 RVA: 0x0003F8A0 File Offset: 0x0003E8A0
		public virtual object SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		// Token: 0x06001856 RID: 6230 RVA: 0x0003F8C4 File Offset: 0x0003E8C4
		public virtual void Clear()
		{
			if (this._head < this._tail)
			{
				Array.Clear(this._array, this._head, this._size);
			}
			else
			{
				Array.Clear(this._array, this._head, this._array.Length - this._head);
				Array.Clear(this._array, 0, this._tail);
			}
			this._head = 0;
			this._tail = 0;
			this._size = 0;
			this._version++;
		}

		// Token: 0x06001857 RID: 6231 RVA: 0x0003F950 File Offset: 0x0003E950
		public virtual void CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_RankMultiDimNotSupported"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			int length = array.Length;
			if (length - index < this._size)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			int num = this._size;
			if (num == 0)
			{
				return;
			}
			int num2 = ((this._array.Length - this._head < num) ? (this._array.Length - this._head) : num);
			Array.Copy(this._array, this._head, array, index, num2);
			num -= num2;
			if (num > 0)
			{
				Array.Copy(this._array, 0, array, index + this._array.Length - this._head, num);
			}
		}

		// Token: 0x06001858 RID: 6232 RVA: 0x0003FA2C File Offset: 0x0003EA2C
		public virtual void Enqueue(object obj)
		{
			if (this._size == this._array.Length)
			{
				int num = (int)((long)this._array.Length * (long)this._growFactor / 100L);
				if (num < this._array.Length + 4)
				{
					num = this._array.Length + 4;
				}
				this.SetCapacity(num);
			}
			this._array[this._tail] = obj;
			this._tail = (this._tail + 1) % this._array.Length;
			this._size++;
			this._version++;
		}

		// Token: 0x06001859 RID: 6233 RVA: 0x0003FAC0 File Offset: 0x0003EAC0
		public virtual IEnumerator GetEnumerator()
		{
			return new Queue.QueueEnumerator(this);
		}

		// Token: 0x0600185A RID: 6234 RVA: 0x0003FAC8 File Offset: 0x0003EAC8
		public virtual object Dequeue()
		{
			if (this._size == 0)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EmptyQueue"));
			}
			object obj = this._array[this._head];
			this._array[this._head] = null;
			this._head = (this._head + 1) % this._array.Length;
			this._size--;
			this._version++;
			return obj;
		}

		// Token: 0x0600185B RID: 6235 RVA: 0x0003FB3D File Offset: 0x0003EB3D
		public virtual object Peek()
		{
			if (this._size == 0)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EmptyQueue"));
			}
			return this._array[this._head];
		}

		// Token: 0x0600185C RID: 6236 RVA: 0x0003FB64 File Offset: 0x0003EB64
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public static Queue Synchronized(Queue queue)
		{
			if (queue == null)
			{
				throw new ArgumentNullException("queue");
			}
			return new Queue.SynchronizedQueue(queue);
		}

		// Token: 0x0600185D RID: 6237 RVA: 0x0003FB7C File Offset: 0x0003EB7C
		public virtual bool Contains(object obj)
		{
			int num = this._head;
			int size = this._size;
			while (size-- > 0)
			{
				if (obj == null)
				{
					if (this._array[num] == null)
					{
						return true;
					}
				}
				else if (this._array[num] != null && this._array[num].Equals(obj))
				{
					return true;
				}
				num = (num + 1) % this._array.Length;
			}
			return false;
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x0003FBDA File Offset: 0x0003EBDA
		internal object GetElement(int i)
		{
			return this._array[(this._head + i) % this._array.Length];
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x0003FBF4 File Offset: 0x0003EBF4
		public virtual object[] ToArray()
		{
			object[] array = new object[this._size];
			if (this._size == 0)
			{
				return array;
			}
			if (this._head < this._tail)
			{
				Array.Copy(this._array, this._head, array, 0, this._size);
			}
			else
			{
				Array.Copy(this._array, this._head, array, 0, this._array.Length - this._head);
				Array.Copy(this._array, 0, array, this._array.Length - this._head, this._tail);
			}
			return array;
		}

		// Token: 0x06001860 RID: 6240 RVA: 0x0003FC88 File Offset: 0x0003EC88
		private void SetCapacity(int capacity)
		{
			object[] array = new object[capacity];
			if (this._size > 0)
			{
				if (this._head < this._tail)
				{
					Array.Copy(this._array, this._head, array, 0, this._size);
				}
				else
				{
					Array.Copy(this._array, this._head, array, 0, this._array.Length - this._head);
					Array.Copy(this._array, 0, array, this._array.Length - this._head, this._tail);
				}
			}
			this._array = array;
			this._head = 0;
			this._tail = ((this._size == capacity) ? 0 : this._size);
			this._version++;
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x0003FD46 File Offset: 0x0003ED46
		public virtual void TrimToSize()
		{
			this.SetCapacity(this._size);
		}

		// Token: 0x0400097C RID: 2428
		private const int _MinimumGrow = 4;

		// Token: 0x0400097D RID: 2429
		private const int _ShrinkThreshold = 32;

		// Token: 0x0400097E RID: 2430
		private object[] _array;

		// Token: 0x0400097F RID: 2431
		private int _head;

		// Token: 0x04000980 RID: 2432
		private int _tail;

		// Token: 0x04000981 RID: 2433
		private int _size;

		// Token: 0x04000982 RID: 2434
		private int _growFactor;

		// Token: 0x04000983 RID: 2435
		private int _version;

		// Token: 0x04000984 RID: 2436
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x02000260 RID: 608
		[Serializable]
		private class SynchronizedQueue : Queue
		{
			// Token: 0x06001862 RID: 6242 RVA: 0x0003FD54 File Offset: 0x0003ED54
			internal SynchronizedQueue(Queue q)
			{
				this._q = q;
				this.root = this._q.SyncRoot;
			}

			// Token: 0x1700039A RID: 922
			// (get) Token: 0x06001863 RID: 6243 RVA: 0x0003FD74 File Offset: 0x0003ED74
			public override bool IsSynchronized
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700039B RID: 923
			// (get) Token: 0x06001864 RID: 6244 RVA: 0x0003FD77 File Offset: 0x0003ED77
			public override object SyncRoot
			{
				get
				{
					return this.root;
				}
			}

			// Token: 0x1700039C RID: 924
			// (get) Token: 0x06001865 RID: 6245 RVA: 0x0003FD80 File Offset: 0x0003ED80
			public override int Count
			{
				get
				{
					int count;
					lock (this.root)
					{
						count = this._q.Count;
					}
					return count;
				}
			}

			// Token: 0x06001866 RID: 6246 RVA: 0x0003FDC0 File Offset: 0x0003EDC0
			public override void Clear()
			{
				lock (this.root)
				{
					this._q.Clear();
				}
			}

			// Token: 0x06001867 RID: 6247 RVA: 0x0003FE00 File Offset: 0x0003EE00
			public override object Clone()
			{
				object obj2;
				lock (this.root)
				{
					obj2 = new Queue.SynchronizedQueue((Queue)this._q.Clone());
				}
				return obj2;
			}

			// Token: 0x06001868 RID: 6248 RVA: 0x0003FE4C File Offset: 0x0003EE4C
			public override bool Contains(object obj)
			{
				bool flag;
				lock (this.root)
				{
					flag = this._q.Contains(obj);
				}
				return flag;
			}

			// Token: 0x06001869 RID: 6249 RVA: 0x0003FE90 File Offset: 0x0003EE90
			public override void CopyTo(Array array, int arrayIndex)
			{
				lock (this.root)
				{
					this._q.CopyTo(array, arrayIndex);
				}
			}

			// Token: 0x0600186A RID: 6250 RVA: 0x0003FED0 File Offset: 0x0003EED0
			public override void Enqueue(object value)
			{
				lock (this.root)
				{
					this._q.Enqueue(value);
				}
			}

			// Token: 0x0600186B RID: 6251 RVA: 0x0003FF10 File Offset: 0x0003EF10
			public override object Dequeue()
			{
				object obj2;
				lock (this.root)
				{
					obj2 = this._q.Dequeue();
				}
				return obj2;
			}

			// Token: 0x0600186C RID: 6252 RVA: 0x0003FF50 File Offset: 0x0003EF50
			public override IEnumerator GetEnumerator()
			{
				IEnumerator enumerator;
				lock (this.root)
				{
					enumerator = this._q.GetEnumerator();
				}
				return enumerator;
			}

			// Token: 0x0600186D RID: 6253 RVA: 0x0003FF90 File Offset: 0x0003EF90
			public override object Peek()
			{
				object obj2;
				lock (this.root)
				{
					obj2 = this._q.Peek();
				}
				return obj2;
			}

			// Token: 0x0600186E RID: 6254 RVA: 0x0003FFD0 File Offset: 0x0003EFD0
			public override object[] ToArray()
			{
				object[] array;
				lock (this.root)
				{
					array = this._q.ToArray();
				}
				return array;
			}

			// Token: 0x0600186F RID: 6255 RVA: 0x00040010 File Offset: 0x0003F010
			public override void TrimToSize()
			{
				lock (this.root)
				{
					this._q.TrimToSize();
				}
			}

			// Token: 0x04000985 RID: 2437
			private Queue _q;

			// Token: 0x04000986 RID: 2438
			private object root;
		}

		// Token: 0x02000261 RID: 609
		[Serializable]
		private class QueueEnumerator : IEnumerator, ICloneable
		{
			// Token: 0x06001870 RID: 6256 RVA: 0x00040050 File Offset: 0x0003F050
			internal QueueEnumerator(Queue q)
			{
				this._q = q;
				this._version = this._q._version;
				this._index = 0;
				this.currentElement = this._q._array;
				if (this._q._size == 0)
				{
					this._index = -1;
				}
			}

			// Token: 0x06001871 RID: 6257 RVA: 0x000400A7 File Offset: 0x0003F0A7
			public object Clone()
			{
				return base.MemberwiseClone();
			}

			// Token: 0x06001872 RID: 6258 RVA: 0x000400B0 File Offset: 0x0003F0B0
			public virtual bool MoveNext()
			{
				if (this._version != this._q._version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				if (this._index < 0)
				{
					this.currentElement = this._q._array;
					return false;
				}
				this.currentElement = this._q.GetElement(this._index);
				this._index++;
				if (this._index == this._q._size)
				{
					this._index = -1;
				}
				return true;
			}

			// Token: 0x1700039D RID: 925
			// (get) Token: 0x06001873 RID: 6259 RVA: 0x0004013C File Offset: 0x0003F13C
			public virtual object Current
			{
				get
				{
					if (this.currentElement != this._q._array)
					{
						return this.currentElement;
					}
					if (this._index == 0)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
					}
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
				}
			}

			// Token: 0x06001874 RID: 6260 RVA: 0x0004018C File Offset: 0x0003F18C
			public virtual void Reset()
			{
				if (this._version != this._q._version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				if (this._q._size == 0)
				{
					this._index = -1;
				}
				else
				{
					this._index = 0;
				}
				this.currentElement = this._q._array;
			}

			// Token: 0x04000987 RID: 2439
			private Queue _q;

			// Token: 0x04000988 RID: 2440
			private int _index;

			// Token: 0x04000989 RID: 2441
			private int _version;

			// Token: 0x0400098A RID: 2442
			private object currentElement;
		}

		// Token: 0x02000262 RID: 610
		internal class QueueDebugView
		{
			// Token: 0x06001875 RID: 6261 RVA: 0x000401EA File Offset: 0x0003F1EA
			public QueueDebugView(Queue queue)
			{
				if (queue == null)
				{
					throw new ArgumentNullException("queue");
				}
				this.queue = queue;
			}

			// Token: 0x1700039E RID: 926
			// (get) Token: 0x06001876 RID: 6262 RVA: 0x00040207 File Offset: 0x0003F207
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public object[] Items
			{
				get
				{
					return this.queue.ToArray();
				}
			}

			// Token: 0x0400098B RID: 2443
			private Queue queue;
		}
	}
}
