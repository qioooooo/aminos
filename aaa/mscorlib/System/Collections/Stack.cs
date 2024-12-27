using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Collections
{
	// Token: 0x0200026A RID: 618
	[ComVisible(true)]
	[DebuggerTypeProxy(typeof(Stack.StackDebugView))]
	[DebuggerDisplay("Count = {Count}")]
	[Serializable]
	public class Stack : ICollection, IEnumerable, ICloneable
	{
		// Token: 0x060018EF RID: 6383 RVA: 0x000415D4 File Offset: 0x000405D4
		public Stack()
		{
			this._array = new object[10];
			this._size = 0;
			this._version = 0;
		}

		// Token: 0x060018F0 RID: 6384 RVA: 0x000415F8 File Offset: 0x000405F8
		public Stack(int initialCapacity)
		{
			if (initialCapacity < 0)
			{
				throw new ArgumentOutOfRangeException("initialCapacity", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (initialCapacity < 10)
			{
				initialCapacity = 10;
			}
			this._array = new object[initialCapacity];
			this._size = 0;
			this._version = 0;
		}

		// Token: 0x060018F1 RID: 6385 RVA: 0x00041648 File Offset: 0x00040648
		public Stack(ICollection col)
			: this((col == null) ? 32 : col.Count)
		{
			if (col == null)
			{
				throw new ArgumentNullException("col");
			}
			foreach (object obj in col)
			{
				this.Push(obj);
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x060018F2 RID: 6386 RVA: 0x00041693 File Offset: 0x00040693
		public virtual int Count
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x060018F3 RID: 6387 RVA: 0x0004169B File Offset: 0x0004069B
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x060018F4 RID: 6388 RVA: 0x0004169E File Offset: 0x0004069E
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

		// Token: 0x060018F5 RID: 6389 RVA: 0x000416C0 File Offset: 0x000406C0
		public virtual void Clear()
		{
			Array.Clear(this._array, 0, this._size);
			this._size = 0;
			this._version++;
		}

		// Token: 0x060018F6 RID: 6390 RVA: 0x000416EC File Offset: 0x000406EC
		public virtual object Clone()
		{
			Stack stack = new Stack(this._size);
			stack._size = this._size;
			Array.Copy(this._array, 0, stack._array, 0, this._size);
			stack._version = this._version;
			return stack;
		}

		// Token: 0x060018F7 RID: 6391 RVA: 0x00041738 File Offset: 0x00040738
		public virtual bool Contains(object obj)
		{
			int size = this._size;
			while (size-- > 0)
			{
				if (obj == null)
				{
					if (this._array[size] == null)
					{
						return true;
					}
				}
				else if (this._array[size] != null && this._array[size].Equals(obj))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060018F8 RID: 6392 RVA: 0x00041784 File Offset: 0x00040784
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
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (array.Length - index < this._size)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			int i = 0;
			if (array is object[])
			{
				object[] array2 = (object[])array;
				while (i < this._size)
				{
					array2[i + index] = this._array[this._size - i - 1];
					i++;
				}
				return;
			}
			while (i < this._size)
			{
				array.SetValue(this._array[this._size - i - 1], i + index);
				i++;
			}
		}

		// Token: 0x060018F9 RID: 6393 RVA: 0x0004184F File Offset: 0x0004084F
		public virtual IEnumerator GetEnumerator()
		{
			return new Stack.StackEnumerator(this);
		}

		// Token: 0x060018FA RID: 6394 RVA: 0x00041857 File Offset: 0x00040857
		public virtual object Peek()
		{
			if (this._size == 0)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EmptyStack"));
			}
			return this._array[this._size - 1];
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x00041880 File Offset: 0x00040880
		public virtual object Pop()
		{
			if (this._size == 0)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EmptyStack"));
			}
			this._version++;
			object obj = this._array[--this._size];
			this._array[this._size] = null;
			return obj;
		}

		// Token: 0x060018FC RID: 6396 RVA: 0x000418DC File Offset: 0x000408DC
		public virtual void Push(object obj)
		{
			if (this._size == this._array.Length)
			{
				object[] array = new object[2 * this._array.Length];
				Array.Copy(this._array, 0, array, 0, this._size);
				this._array = array;
			}
			this._array[this._size++] = obj;
			this._version++;
		}

		// Token: 0x060018FD RID: 6397 RVA: 0x0004194B File Offset: 0x0004094B
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public static Stack Synchronized(Stack stack)
		{
			if (stack == null)
			{
				throw new ArgumentNullException("stack");
			}
			return new Stack.SyncStack(stack);
		}

		// Token: 0x060018FE RID: 6398 RVA: 0x00041964 File Offset: 0x00040964
		public virtual object[] ToArray()
		{
			object[] array = new object[this._size];
			for (int i = 0; i < this._size; i++)
			{
				array[i] = this._array[this._size - i - 1];
			}
			return array;
		}

		// Token: 0x040009A8 RID: 2472
		private const int _defaultCapacity = 10;

		// Token: 0x040009A9 RID: 2473
		private object[] _array;

		// Token: 0x040009AA RID: 2474
		private int _size;

		// Token: 0x040009AB RID: 2475
		private int _version;

		// Token: 0x040009AC RID: 2476
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x0200026B RID: 619
		[Serializable]
		private class SyncStack : Stack
		{
			// Token: 0x060018FF RID: 6399 RVA: 0x000419A3 File Offset: 0x000409A3
			internal SyncStack(Stack stack)
			{
				this._s = stack;
				this._root = stack.SyncRoot;
			}

			// Token: 0x170003C7 RID: 967
			// (get) Token: 0x06001900 RID: 6400 RVA: 0x000419BE File Offset: 0x000409BE
			public override bool IsSynchronized
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170003C8 RID: 968
			// (get) Token: 0x06001901 RID: 6401 RVA: 0x000419C1 File Offset: 0x000409C1
			public override object SyncRoot
			{
				get
				{
					return this._root;
				}
			}

			// Token: 0x170003C9 RID: 969
			// (get) Token: 0x06001902 RID: 6402 RVA: 0x000419CC File Offset: 0x000409CC
			public override int Count
			{
				get
				{
					int count;
					lock (this._root)
					{
						count = this._s.Count;
					}
					return count;
				}
			}

			// Token: 0x06001903 RID: 6403 RVA: 0x00041A0C File Offset: 0x00040A0C
			public override bool Contains(object obj)
			{
				bool flag;
				lock (this._root)
				{
					flag = this._s.Contains(obj);
				}
				return flag;
			}

			// Token: 0x06001904 RID: 6404 RVA: 0x00041A50 File Offset: 0x00040A50
			public override object Clone()
			{
				object obj;
				lock (this._root)
				{
					obj = new Stack.SyncStack((Stack)this._s.Clone());
				}
				return obj;
			}

			// Token: 0x06001905 RID: 6405 RVA: 0x00041A9C File Offset: 0x00040A9C
			public override void Clear()
			{
				lock (this._root)
				{
					this._s.Clear();
				}
			}

			// Token: 0x06001906 RID: 6406 RVA: 0x00041ADC File Offset: 0x00040ADC
			public override void CopyTo(Array array, int arrayIndex)
			{
				lock (this._root)
				{
					this._s.CopyTo(array, arrayIndex);
				}
			}

			// Token: 0x06001907 RID: 6407 RVA: 0x00041B1C File Offset: 0x00040B1C
			public override void Push(object value)
			{
				lock (this._root)
				{
					this._s.Push(value);
				}
			}

			// Token: 0x06001908 RID: 6408 RVA: 0x00041B5C File Offset: 0x00040B5C
			public override object Pop()
			{
				object obj;
				lock (this._root)
				{
					obj = this._s.Pop();
				}
				return obj;
			}

			// Token: 0x06001909 RID: 6409 RVA: 0x00041B9C File Offset: 0x00040B9C
			public override IEnumerator GetEnumerator()
			{
				IEnumerator enumerator;
				lock (this._root)
				{
					enumerator = this._s.GetEnumerator();
				}
				return enumerator;
			}

			// Token: 0x0600190A RID: 6410 RVA: 0x00041BDC File Offset: 0x00040BDC
			public override object Peek()
			{
				object obj;
				lock (this._root)
				{
					obj = this._s.Peek();
				}
				return obj;
			}

			// Token: 0x0600190B RID: 6411 RVA: 0x00041C1C File Offset: 0x00040C1C
			public override object[] ToArray()
			{
				object[] array;
				lock (this._root)
				{
					array = this._s.ToArray();
				}
				return array;
			}

			// Token: 0x040009AD RID: 2477
			private Stack _s;

			// Token: 0x040009AE RID: 2478
			private object _root;
		}

		// Token: 0x0200026C RID: 620
		[Serializable]
		private class StackEnumerator : IEnumerator, ICloneable
		{
			// Token: 0x0600190C RID: 6412 RVA: 0x00041C5C File Offset: 0x00040C5C
			internal StackEnumerator(Stack stack)
			{
				this._stack = stack;
				this._version = this._stack._version;
				this._index = -2;
				this.currentElement = null;
			}

			// Token: 0x0600190D RID: 6413 RVA: 0x00041C8B File Offset: 0x00040C8B
			public object Clone()
			{
				return base.MemberwiseClone();
			}

			// Token: 0x0600190E RID: 6414 RVA: 0x00041C94 File Offset: 0x00040C94
			public virtual bool MoveNext()
			{
				if (this._version != this._stack._version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				bool flag;
				if (this._index == -2)
				{
					this._index = this._stack._size - 1;
					flag = this._index >= 0;
					if (flag)
					{
						this.currentElement = this._stack._array[this._index];
					}
					return flag;
				}
				if (this._index == -1)
				{
					return false;
				}
				flag = --this._index >= 0;
				if (flag)
				{
					this.currentElement = this._stack._array[this._index];
				}
				else
				{
					this.currentElement = null;
				}
				return flag;
			}

			// Token: 0x170003CA RID: 970
			// (get) Token: 0x0600190F RID: 6415 RVA: 0x00041D53 File Offset: 0x00040D53
			public virtual object Current
			{
				get
				{
					if (this._index == -2)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
					}
					if (this._index == -1)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
					}
					return this.currentElement;
				}
			}

			// Token: 0x06001910 RID: 6416 RVA: 0x00041D8E File Offset: 0x00040D8E
			public virtual void Reset()
			{
				if (this._version != this._stack._version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				this._index = -2;
				this.currentElement = null;
			}

			// Token: 0x040009AF RID: 2479
			private Stack _stack;

			// Token: 0x040009B0 RID: 2480
			private int _index;

			// Token: 0x040009B1 RID: 2481
			private int _version;

			// Token: 0x040009B2 RID: 2482
			private object currentElement;
		}

		// Token: 0x0200026D RID: 621
		internal class StackDebugView
		{
			// Token: 0x06001911 RID: 6417 RVA: 0x00041DC2 File Offset: 0x00040DC2
			public StackDebugView(Stack stack)
			{
				if (stack == null)
				{
					throw new ArgumentNullException("stack");
				}
				this.stack = stack;
			}

			// Token: 0x170003CB RID: 971
			// (get) Token: 0x06001912 RID: 6418 RVA: 0x00041DDF File Offset: 0x00040DDF
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public object[] Items
			{
				get
				{
					return this.stack.ToArray();
				}
			}

			// Token: 0x040009B3 RID: 2483
			private Stack stack;
		}
	}
}
