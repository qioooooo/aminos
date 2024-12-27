using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Collections.Generic
{
	// Token: 0x0200023A RID: 570
	[ComVisible(false)]
	[DebuggerTypeProxy(typeof(System_StackDebugView<>))]
	[DebuggerDisplay("Count = {Count}")]
	[Serializable]
	public class Stack<T> : IEnumerable<T>, ICollection, IEnumerable
	{
		// Token: 0x06001359 RID: 4953 RVA: 0x00040C64 File Offset: 0x0003FC64
		public Stack()
		{
			this._array = Stack<T>._emptyArray;
			this._size = 0;
			this._version = 0;
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x00040C85 File Offset: 0x0003FC85
		public Stack(int capacity)
		{
			if (capacity < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity, ExceptionResource.ArgumentOutOfRange_NeedNonNegNumRequired);
			}
			this._array = new T[capacity];
			this._size = 0;
			this._version = 0;
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x00040CB4 File Offset: 0x0003FCB4
		public Stack(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
			}
			ICollection<T> collection2 = collection as ICollection<T>;
			if (collection2 != null)
			{
				int count = collection2.Count;
				this._array = new T[count];
				collection2.CopyTo(this._array, 0);
				this._size = count;
				return;
			}
			this._size = 0;
			this._array = new T[4];
			foreach (T t in collection)
			{
				this.Push(t);
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x0600135C RID: 4956 RVA: 0x00040D50 File Offset: 0x0003FD50
		public int Count
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x0600135D RID: 4957 RVA: 0x00040D58 File Offset: 0x0003FD58
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x0600135E RID: 4958 RVA: 0x00040D5B File Offset: 0x0003FD5B
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

		// Token: 0x0600135F RID: 4959 RVA: 0x00040D7D File Offset: 0x0003FD7D
		public void Clear()
		{
			Array.Clear(this._array, 0, this._size);
			this._size = 0;
			this._version++;
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x00040DA8 File Offset: 0x0003FDA8
		public bool Contains(T item)
		{
			int size = this._size;
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			while (size-- > 0)
			{
				if (item == null)
				{
					if (this._array[size] == null)
					{
						return true;
					}
				}
				else if (this._array[size] != null && @default.Equals(this._array[size], item))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x00040E14 File Offset: 0x0003FE14
		public void CopyTo(T[] array, int arrayIndex)
		{
			if (array == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
			}
			if (arrayIndex < 0 || arrayIndex > array.Length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.arrayIndex, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (array.Length - arrayIndex < this._size)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			Array.Copy(this._array, 0, array, arrayIndex, this._size);
			Array.Reverse(array, arrayIndex, this._size);
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x00040E74 File Offset: 0x0003FE74
		void ICollection.CopyTo(Array array, int arrayIndex)
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
			if (arrayIndex < 0 || arrayIndex > array.Length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.arrayIndex, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (array.Length - arrayIndex < this._size)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			try
			{
				Array.Copy(this._array, 0, array, arrayIndex, this._size);
				Array.Reverse(array, arrayIndex, this._size);
			}
			catch (ArrayTypeMismatchException)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
			}
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x00040F14 File Offset: 0x0003FF14
		public Stack<T>.Enumerator GetEnumerator()
		{
			return new Stack<T>.Enumerator(this);
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x00040F1C File Offset: 0x0003FF1C
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new Stack<T>.Enumerator(this);
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x00040F29 File Offset: 0x0003FF29
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Stack<T>.Enumerator(this);
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x00040F38 File Offset: 0x0003FF38
		public void TrimExcess()
		{
			int num = (int)((double)this._array.Length * 0.9);
			if (this._size < num)
			{
				T[] array = new T[this._size];
				Array.Copy(this._array, 0, array, 0, this._size);
				this._array = array;
				this._version++;
			}
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x00040F98 File Offset: 0x0003FF98
		public T Peek()
		{
			if (this._size == 0)
			{
				ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EmptyStack);
			}
			return this._array[this._size - 1];
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x00040FBC File Offset: 0x0003FFBC
		public T Pop()
		{
			if (this._size == 0)
			{
				ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EmptyStack);
			}
			this._version++;
			T t = this._array[--this._size];
			this._array[this._size] = default(T);
			return t;
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x00041020 File Offset: 0x00040020
		public void Push(T item)
		{
			if (this._size == this._array.Length)
			{
				T[] array = new T[(this._array.Length == 0) ? 4 : (2 * this._array.Length)];
				Array.Copy(this._array, 0, array, 0, this._size);
				this._array = array;
			}
			this._array[this._size++] = item;
			this._version++;
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x000410A0 File Offset: 0x000400A0
		public T[] ToArray()
		{
			T[] array = new T[this._size];
			for (int i = 0; i < this._size; i++)
			{
				array[i] = this._array[this._size - i - 1];
			}
			return array;
		}

		// Token: 0x0400110D RID: 4365
		private const int _defaultCapacity = 4;

		// Token: 0x0400110E RID: 4366
		private T[] _array;

		// Token: 0x0400110F RID: 4367
		private int _size;

		// Token: 0x04001110 RID: 4368
		private int _version;

		// Token: 0x04001111 RID: 4369
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x04001112 RID: 4370
		private static T[] _emptyArray = new T[0];

		// Token: 0x0200023B RID: 571
		[Serializable]
		public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
		{
			// Token: 0x0600136C RID: 4972 RVA: 0x000410F4 File Offset: 0x000400F4
			internal Enumerator(Stack<T> stack)
			{
				this._stack = stack;
				this._version = this._stack._version;
				this._index = -2;
				this.currentElement = default(T);
			}

			// Token: 0x0600136D RID: 4973 RVA: 0x00041122 File Offset: 0x00040122
			public void Dispose()
			{
				this._index = -1;
			}

			// Token: 0x0600136E RID: 4974 RVA: 0x0004112C File Offset: 0x0004012C
			public bool MoveNext()
			{
				if (this._version != this._stack._version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
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
					this.currentElement = default(T);
				}
				return flag;
			}

			// Token: 0x170003F0 RID: 1008
			// (get) Token: 0x0600136F RID: 4975 RVA: 0x000411EF File Offset: 0x000401EF
			public T Current
			{
				get
				{
					if (this._index == -2)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumNotStarted);
					}
					if (this._index == -1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumEnded);
					}
					return this.currentElement;
				}
			}

			// Token: 0x170003F1 RID: 1009
			// (get) Token: 0x06001370 RID: 4976 RVA: 0x00041218 File Offset: 0x00040218
			object IEnumerator.Current
			{
				get
				{
					if (this._index == -2)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumNotStarted);
					}
					if (this._index == -1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumEnded);
					}
					return this.currentElement;
				}
			}

			// Token: 0x06001371 RID: 4977 RVA: 0x00041246 File Offset: 0x00040246
			void IEnumerator.Reset()
			{
				if (this._version != this._stack._version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				this._index = -2;
				this.currentElement = default(T);
			}

			// Token: 0x04001113 RID: 4371
			private Stack<T> _stack;

			// Token: 0x04001114 RID: 4372
			private int _index;

			// Token: 0x04001115 RID: 4373
			private int _version;

			// Token: 0x04001116 RID: 4374
			private T currentElement;
		}
	}
}
