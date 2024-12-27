using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Collections.Generic
{
	// Token: 0x02000232 RID: 562
	[ComVisible(false)]
	[DebuggerTypeProxy(typeof(System_QueueDebugView<>))]
	[DebuggerDisplay("Count = {Count}")]
	[Serializable]
	public class Queue<T> : IEnumerable<T>, ICollection, IEnumerable
	{
		// Token: 0x060012CC RID: 4812 RVA: 0x0003F325 File Offset: 0x0003E325
		public Queue()
		{
			this._array = Queue<T>._emptyArray;
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x0003F338 File Offset: 0x0003E338
		public Queue(int capacity)
		{
			if (capacity < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity, ExceptionResource.ArgumentOutOfRange_NeedNonNegNumRequired);
			}
			this._array = new T[capacity];
			this._head = 0;
			this._tail = 0;
			this._size = 0;
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x0003F370 File Offset: 0x0003E370
		public Queue(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
			}
			this._array = new T[4];
			this._size = 0;
			this._version = 0;
			foreach (T t in collection)
			{
				this.Enqueue(t);
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x060012CF RID: 4815 RVA: 0x0003F3E0 File Offset: 0x0003E3E0
		public int Count
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x060012D0 RID: 4816 RVA: 0x0003F3E8 File Offset: 0x0003E3E8
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x060012D1 RID: 4817 RVA: 0x0003F3EB File Offset: 0x0003E3EB
		object ICollection.SyncRoot
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

		// Token: 0x060012D2 RID: 4818 RVA: 0x0003F410 File Offset: 0x0003E410
		public void Clear()
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

		// Token: 0x060012D3 RID: 4819 RVA: 0x0003F49C File Offset: 0x0003E49C
		public void CopyTo(T[] array, int arrayIndex)
		{
			if (array == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
			}
			if (arrayIndex < 0 || arrayIndex > array.Length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.arrayIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}
			int num = array.Length;
			if (num - arrayIndex < this._size)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			int num2 = ((num - arrayIndex < this._size) ? (num - arrayIndex) : this._size);
			if (num2 == 0)
			{
				return;
			}
			int num3 = ((this._array.Length - this._head < num2) ? (this._array.Length - this._head) : num2);
			Array.Copy(this._array, this._head, array, arrayIndex, num3);
			num2 -= num3;
			if (num2 > 0)
			{
				Array.Copy(this._array, 0, array, arrayIndex + this._array.Length - this._head, num2);
			}
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x0003F558 File Offset: 0x0003E558
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
			}
			if (array.Rank != 1)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
			}
			if (array.GetLowerBound(0) != 0)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
			}
			int length = array.Length;
			if (index < 0 || index > length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
			}
			if (length - index < this._size)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			int num = ((length - index < this._size) ? (length - index) : this._size);
			if (num == 0)
			{
				return;
			}
			try
			{
				int num2 = ((this._array.Length - this._head < num) ? (this._array.Length - this._head) : num);
				Array.Copy(this._array, this._head, array, index, num2);
				num -= num2;
				if (num > 0)
				{
					Array.Copy(this._array, 0, array, index + this._array.Length - this._head, num);
				}
			}
			catch (ArrayTypeMismatchException)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
			}
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x0003F650 File Offset: 0x0003E650
		public void Enqueue(T item)
		{
			if (this._size == this._array.Length)
			{
				int num = (int)((long)this._array.Length * 200L / 100L);
				if (num < this._array.Length + 4)
				{
					num = this._array.Length + 4;
				}
				this.SetCapacity(num);
			}
			this._array[this._tail] = item;
			this._tail = (this._tail + 1) % this._array.Length;
			this._size++;
			this._version++;
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x0003F6E7 File Offset: 0x0003E6E7
		public Queue<T>.Enumerator GetEnumerator()
		{
			return new Queue<T>.Enumerator(this);
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x0003F6EF File Offset: 0x0003E6EF
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new Queue<T>.Enumerator(this);
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x0003F6FC File Offset: 0x0003E6FC
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Queue<T>.Enumerator(this);
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x0003F70C File Offset: 0x0003E70C
		public T Dequeue()
		{
			if (this._size == 0)
			{
				ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EmptyQueue);
			}
			T t = this._array[this._head];
			this._array[this._head] = default(T);
			this._head = (this._head + 1) % this._array.Length;
			this._size--;
			this._version++;
			return t;
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x0003F788 File Offset: 0x0003E788
		public T Peek()
		{
			if (this._size == 0)
			{
				ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EmptyQueue);
			}
			return this._array[this._head];
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x0003F7AC File Offset: 0x0003E7AC
		public bool Contains(T item)
		{
			int num = this._head;
			int size = this._size;
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			while (size-- > 0)
			{
				if (item == null)
				{
					if (this._array[num] == null)
					{
						return true;
					}
				}
				else if (this._array[num] != null && @default.Equals(this._array[num], item))
				{
					return true;
				}
				num = (num + 1) % this._array.Length;
			}
			return false;
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x0003F82C File Offset: 0x0003E82C
		internal T GetElement(int i)
		{
			return this._array[(this._head + i) % this._array.Length];
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x0003F84C File Offset: 0x0003E84C
		public T[] ToArray()
		{
			T[] array = new T[this._size];
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

		// Token: 0x060012DE RID: 4830 RVA: 0x0003F8E0 File Offset: 0x0003E8E0
		private void SetCapacity(int capacity)
		{
			T[] array = new T[capacity];
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

		// Token: 0x060012DF RID: 4831 RVA: 0x0003F9A0 File Offset: 0x0003E9A0
		public void TrimExcess()
		{
			int num = (int)((double)this._array.Length * 0.9);
			if (this._size < num)
			{
				this.SetCapacity(this._size);
			}
		}

		// Token: 0x040010E1 RID: 4321
		private const int _MinimumGrow = 4;

		// Token: 0x040010E2 RID: 4322
		private const int _ShrinkThreshold = 32;

		// Token: 0x040010E3 RID: 4323
		private const int _GrowFactor = 200;

		// Token: 0x040010E4 RID: 4324
		private const int _DefaultCapacity = 4;

		// Token: 0x040010E5 RID: 4325
		private T[] _array;

		// Token: 0x040010E6 RID: 4326
		private int _head;

		// Token: 0x040010E7 RID: 4327
		private int _tail;

		// Token: 0x040010E8 RID: 4328
		private int _size;

		// Token: 0x040010E9 RID: 4329
		private int _version;

		// Token: 0x040010EA RID: 4330
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x040010EB RID: 4331
		private static T[] _emptyArray = new T[0];

		// Token: 0x02000233 RID: 563
		[Serializable]
		public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
		{
			// Token: 0x060012E1 RID: 4833 RVA: 0x0003F9E4 File Offset: 0x0003E9E4
			internal Enumerator(Queue<T> q)
			{
				this._q = q;
				this._version = this._q._version;
				this._index = -1;
				this._currentElement = default(T);
			}

			// Token: 0x060012E2 RID: 4834 RVA: 0x0003FA11 File Offset: 0x0003EA11
			public void Dispose()
			{
				this._index = -2;
				this._currentElement = default(T);
			}

			// Token: 0x060012E3 RID: 4835 RVA: 0x0003FA28 File Offset: 0x0003EA28
			public bool MoveNext()
			{
				if (this._version != this._q._version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				if (this._index == -2)
				{
					return false;
				}
				this._index++;
				if (this._index == this._q._size)
				{
					this._index = -2;
					this._currentElement = default(T);
					return false;
				}
				this._currentElement = this._q.GetElement(this._index);
				return true;
			}

			// Token: 0x170003C8 RID: 968
			// (get) Token: 0x060012E4 RID: 4836 RVA: 0x0003FAAA File Offset: 0x0003EAAA
			public T Current
			{
				get
				{
					if (this._index < 0)
					{
						if (this._index == -1)
						{
							ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumNotStarted);
						}
						else
						{
							ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumEnded);
						}
					}
					return this._currentElement;
				}
			}

			// Token: 0x170003C9 RID: 969
			// (get) Token: 0x060012E5 RID: 4837 RVA: 0x0003FAD4 File Offset: 0x0003EAD4
			object IEnumerator.Current
			{
				get
				{
					if (this._index < 0)
					{
						if (this._index == -1)
						{
							ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumNotStarted);
						}
						else
						{
							ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumEnded);
						}
					}
					return this._currentElement;
				}
			}

			// Token: 0x060012E6 RID: 4838 RVA: 0x0003FB03 File Offset: 0x0003EB03
			void IEnumerator.Reset()
			{
				if (this._version != this._q._version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				this._index = -1;
				this._currentElement = default(T);
			}

			// Token: 0x040010EC RID: 4332
			private Queue<T> _q;

			// Token: 0x040010ED RID: 4333
			private int _index;

			// Token: 0x040010EE RID: 4334
			private int _version;

			// Token: 0x040010EF RID: 4335
			private T _currentElement;
		}
	}
}
