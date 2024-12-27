using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;

namespace System.Collections.Generic
{
	// Token: 0x0200028E RID: 654
	[DebuggerTypeProxy(typeof(Mscorlib_CollectionDebugView<>))]
	[DebuggerDisplay("Count = {Count}")]
	[Serializable]
	public class List<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable
	{
		// Token: 0x060019F1 RID: 6641 RVA: 0x000441FB File Offset: 0x000431FB
		public List()
		{
			this._items = List<T>._emptyArray;
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x0004420E File Offset: 0x0004320E
		public List(int capacity)
		{
			if (capacity < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity, ExceptionResource.ArgumentOutOfRange_SmallCapacity);
			}
			this._items = new T[capacity];
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x00044230 File Offset: 0x00043230
		public List(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
			}
			ICollection<T> collection2 = collection as ICollection<T>;
			if (collection2 != null)
			{
				int count = collection2.Count;
				this._items = new T[count];
				collection2.CopyTo(this._items, 0);
				this._size = count;
				return;
			}
			this._size = 0;
			this._items = new T[4];
			foreach (T t in collection)
			{
				this.Add(t);
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x060019F4 RID: 6644 RVA: 0x000442CC File Offset: 0x000432CC
		// (set) Token: 0x060019F5 RID: 6645 RVA: 0x000442D8 File Offset: 0x000432D8
		public int Capacity
		{
			get
			{
				return this._items.Length;
			}
			set
			{
				if (value != this._items.Length)
				{
					if (value < this._size)
					{
						ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value, ExceptionResource.ArgumentOutOfRange_SmallCapacity);
					}
					if (value > 0)
					{
						T[] array = new T[value];
						if (this._size > 0)
						{
							Array.Copy(this._items, 0, array, 0, this._size);
						}
						this._items = array;
						return;
					}
					this._items = List<T>._emptyArray;
				}
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x060019F6 RID: 6646 RVA: 0x0004433D File Offset: 0x0004333D
		public int Count
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x060019F7 RID: 6647 RVA: 0x00044345 File Offset: 0x00043345
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x060019F8 RID: 6648 RVA: 0x00044348 File Offset: 0x00043348
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x060019F9 RID: 6649 RVA: 0x0004434B File Offset: 0x0004334B
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x060019FA RID: 6650 RVA: 0x0004434E File Offset: 0x0004334E
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x060019FB RID: 6651 RVA: 0x00044351 File Offset: 0x00043351
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

		// Token: 0x17000404 RID: 1028
		public T this[int index]
		{
			get
			{
				if (index >= this._size)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException();
				}
				return this._items[index];
			}
			set
			{
				if (index >= this._size)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException();
				}
				this._items[index] = value;
				this._version++;
			}
		}

		// Token: 0x060019FE RID: 6654 RVA: 0x000443BA File Offset: 0x000433BA
		private static bool IsCompatibleObject(object value)
		{
			return value is T || (value == null && !typeof(T).IsValueType);
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x000443DB File Offset: 0x000433DB
		private static void VerifyValueType(object value)
		{
			if (!List<T>.IsCompatibleObject(value))
			{
				ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(T));
			}
		}

		// Token: 0x17000405 RID: 1029
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				List<T>.VerifyValueType(value);
				this[index] = (T)((object)value);
			}
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x00044418 File Offset: 0x00043418
		public void Add(T item)
		{
			if (this._size == this._items.Length)
			{
				this.EnsureCapacity(this._size + 1);
			}
			this._items[this._size++] = item;
			this._version++;
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x0004446E File Offset: 0x0004346E
		int IList.Add(object item)
		{
			List<T>.VerifyValueType(item);
			this.Add((T)((object)item));
			return this.Count - 1;
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x0004448A File Offset: 0x0004348A
		public void AddRange(IEnumerable<T> collection)
		{
			this.InsertRange(this._size, collection);
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x00044499 File Offset: 0x00043499
		public ReadOnlyCollection<T> AsReadOnly()
		{
			return new ReadOnlyCollection<T>(this);
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x000444A1 File Offset: 0x000434A1
		public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
		{
			if (index < 0 || count < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException((index < 0) ? ExceptionArgument.index : ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (this._size - index < count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			return Array.BinarySearch<T>(this._items, index, count, item, comparer);
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x000444DD File Offset: 0x000434DD
		public int BinarySearch(T item)
		{
			return this.BinarySearch(0, this.Count, item, null);
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x000444EE File Offset: 0x000434EE
		public int BinarySearch(T item, IComparer<T> comparer)
		{
			return this.BinarySearch(0, this.Count, item, comparer);
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x000444FF File Offset: 0x000434FF
		public void Clear()
		{
			if (this._size > 0)
			{
				Array.Clear(this._items, 0, this._size);
				this._size = 0;
			}
			this._version++;
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x00044534 File Offset: 0x00043534
		public bool Contains(T item)
		{
			if (item == null)
			{
				for (int i = 0; i < this._size; i++)
				{
					if (this._items[i] == null)
					{
						return true;
					}
				}
				return false;
			}
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int j = 0; j < this._size; j++)
			{
				if (@default.Equals(this._items[j], item))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x000445A0 File Offset: 0x000435A0
		bool IList.Contains(object item)
		{
			return List<T>.IsCompatibleObject(item) && this.Contains((T)((object)item));
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x000445B8 File Offset: 0x000435B8
		public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
		{
			if (converter == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.converter);
			}
			List<TOutput> list = new List<TOutput>(this._size);
			for (int i = 0; i < this._size; i++)
			{
				list._items[i] = converter(this._items[i]);
			}
			list._size = this._size;
			return list;
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x00044617 File Offset: 0x00043617
		public void CopyTo(T[] array)
		{
			this.CopyTo(array, 0);
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x00044624 File Offset: 0x00043624
		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			if (array != null && array.Rank != 1)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
			}
			try
			{
				Array.Copy(this._items, 0, array, arrayIndex, this._size);
			}
			catch (ArrayTypeMismatchException)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
			}
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x00044674 File Offset: 0x00043674
		public void CopyTo(int index, T[] array, int arrayIndex, int count)
		{
			if (this._size - index < count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			Array.Copy(this._items, index, array, arrayIndex, count);
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x00044699 File Offset: 0x00043699
		public void CopyTo(T[] array, int arrayIndex)
		{
			Array.Copy(this._items, 0, array, arrayIndex, this._size);
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x000446B0 File Offset: 0x000436B0
		private void EnsureCapacity(int min)
		{
			if (this._items.Length < min)
			{
				int num = ((this._items.Length == 0) ? 4 : (this._items.Length * 2));
				if (num < min)
				{
					num = min;
				}
				this.Capacity = num;
			}
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x000446ED File Offset: 0x000436ED
		public bool Exists(Predicate<T> match)
		{
			return this.FindIndex(match) != -1;
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x000446FC File Offset: 0x000436FC
		public T Find(Predicate<T> match)
		{
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			for (int i = 0; i < this._size; i++)
			{
				if (match(this._items[i]))
				{
					return this._items[i];
				}
			}
			return default(T);
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x00044750 File Offset: 0x00043750
		public List<T> FindAll(Predicate<T> match)
		{
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			List<T> list = new List<T>();
			for (int i = 0; i < this._size; i++)
			{
				if (match(this._items[i]))
				{
					list.Add(this._items[i]);
				}
			}
			return list;
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x000447A4 File Offset: 0x000437A4
		public int FindIndex(Predicate<T> match)
		{
			return this.FindIndex(0, this._size, match);
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x000447B4 File Offset: 0x000437B4
		public int FindIndex(int startIndex, Predicate<T> match)
		{
			return this.FindIndex(startIndex, this._size - startIndex, match);
		}

		// Token: 0x06001A17 RID: 6679 RVA: 0x000447C8 File Offset: 0x000437C8
		public int FindIndex(int startIndex, int count, Predicate<T> match)
		{
			if (startIndex > this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}
			if (count < 0 || startIndex > this._size - count)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_Count);
			}
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			int num = startIndex + count;
			for (int i = startIndex; i < num; i++)
			{
				if (match(this._items[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06001A18 RID: 6680 RVA: 0x00044830 File Offset: 0x00043830
		public T FindLast(Predicate<T> match)
		{
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			for (int i = this._size - 1; i >= 0; i--)
			{
				if (match(this._items[i]))
				{
					return this._items[i];
				}
			}
			return default(T);
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x00044883 File Offset: 0x00043883
		public int FindLastIndex(Predicate<T> match)
		{
			return this.FindLastIndex(this._size - 1, this._size, match);
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x0004489A File Offset: 0x0004389A
		public int FindLastIndex(int startIndex, Predicate<T> match)
		{
			return this.FindLastIndex(startIndex, startIndex + 1, match);
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x000448A8 File Offset: 0x000438A8
		public int FindLastIndex(int startIndex, int count, Predicate<T> match)
		{
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			if (this._size == 0)
			{
				if (startIndex != -1)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
				}
			}
			else if (startIndex >= this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}
			if (count < 0 || startIndex - count + 1 < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_Count);
			}
			int num = startIndex - count;
			for (int i = startIndex; i > num; i--)
			{
				if (match(this._items[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06001A1C RID: 6684 RVA: 0x00044924 File Offset: 0x00043924
		public void ForEach(Action<T> action)
		{
			if (action == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			for (int i = 0; i < this._size; i++)
			{
				action(this._items[i]);
			}
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x0004495D File Offset: 0x0004395D
		public List<T>.Enumerator GetEnumerator()
		{
			return new List<T>.Enumerator(this);
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x00044965 File Offset: 0x00043965
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new List<T>.Enumerator(this);
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x00044972 File Offset: 0x00043972
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new List<T>.Enumerator(this);
		}

		// Token: 0x06001A20 RID: 6688 RVA: 0x00044980 File Offset: 0x00043980
		public List<T> GetRange(int index, int count)
		{
			if (index < 0 || count < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException((index < 0) ? ExceptionArgument.index : ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (this._size - index < count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			List<T> list = new List<T>(count);
			Array.Copy(this._items, index, list._items, 0, count);
			list._size = count;
			return list;
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x000449DA File Offset: 0x000439DA
		public int IndexOf(T item)
		{
			return Array.IndexOf<T>(this._items, item, 0, this._size);
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x000449EF File Offset: 0x000439EF
		int IList.IndexOf(object item)
		{
			if (List<T>.IsCompatibleObject(item))
			{
				return this.IndexOf((T)((object)item));
			}
			return -1;
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x00044A07 File Offset: 0x00043A07
		public int IndexOf(T item, int index)
		{
			if (index > this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
			}
			return Array.IndexOf<T>(this._items, item, index, this._size - index);
		}

		// Token: 0x06001A24 RID: 6692 RVA: 0x00044A30 File Offset: 0x00043A30
		public int IndexOf(T item, int index, int count)
		{
			if (index > this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
			}
			if (count < 0 || index > this._size - count)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_Count);
			}
			return Array.IndexOf<T>(this._items, item, index, count);
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x00044A6C File Offset: 0x00043A6C
		public void Insert(int index, T item)
		{
			if (index > this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_ListInsert);
			}
			if (this._size == this._items.Length)
			{
				this.EnsureCapacity(this._size + 1);
			}
			if (index < this._size)
			{
				Array.Copy(this._items, index, this._items, index + 1, this._size - index);
			}
			this._items[index] = item;
			this._size++;
			this._version++;
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x00044AF8 File Offset: 0x00043AF8
		void IList.Insert(int index, object item)
		{
			List<T>.VerifyValueType(item);
			this.Insert(index, (T)((object)item));
		}

		// Token: 0x06001A27 RID: 6695 RVA: 0x00044B10 File Offset: 0x00043B10
		public void InsertRange(int index, IEnumerable<T> collection)
		{
			if (collection == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
			}
			if (index > this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
			}
			ICollection<T> collection2 = collection as ICollection<T>;
			if (collection2 != null)
			{
				int count = collection2.Count;
				if (count > 0)
				{
					this.EnsureCapacity(this._size + count);
					if (index < this._size)
					{
						Array.Copy(this._items, index, this._items, index + count, this._size - index);
					}
					if (this == collection2)
					{
						Array.Copy(this._items, 0, this._items, index, index);
						Array.Copy(this._items, index + count, this._items, index * 2, this._size - index);
					}
					else
					{
						T[] array = new T[count];
						collection2.CopyTo(array, 0);
						array.CopyTo(this._items, index);
					}
					this._size += count;
				}
			}
			else
			{
				foreach (T t in collection)
				{
					this.Insert(index++, t);
				}
			}
			this._version++;
		}

		// Token: 0x06001A28 RID: 6696 RVA: 0x00044C3C File Offset: 0x00043C3C
		public int LastIndexOf(T item)
		{
			return this.LastIndexOf(item, this._size - 1, this._size);
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x00044C53 File Offset: 0x00043C53
		public int LastIndexOf(T item, int index)
		{
			if (index >= this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
			}
			return this.LastIndexOf(item, index, index + 1);
		}

		// Token: 0x06001A2A RID: 6698 RVA: 0x00044C74 File Offset: 0x00043C74
		public int LastIndexOf(T item, int index, int count)
		{
			if (this._size == 0)
			{
				return -1;
			}
			if (index < 0 || count < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException((index < 0) ? ExceptionArgument.index : ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (index >= this._size || count > index + 1)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException((index >= this._size) ? ExceptionArgument.index : ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_BiggerThanCollection);
			}
			return Array.LastIndexOf<T>(this._items, item, index, count);
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x00044CD8 File Offset: 0x00043CD8
		public bool Remove(T item)
		{
			int num = this.IndexOf(item);
			if (num >= 0)
			{
				this.RemoveAt(num);
				return true;
			}
			return false;
		}

		// Token: 0x06001A2C RID: 6700 RVA: 0x00044CFB File Offset: 0x00043CFB
		void IList.Remove(object item)
		{
			if (List<T>.IsCompatibleObject(item))
			{
				this.Remove((T)((object)item));
			}
		}

		// Token: 0x06001A2D RID: 6701 RVA: 0x00044D14 File Offset: 0x00043D14
		public int RemoveAll(Predicate<T> match)
		{
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			int num = 0;
			while (num < this._size && !match(this._items[num]))
			{
				num++;
			}
			if (num >= this._size)
			{
				return 0;
			}
			int i = num + 1;
			while (i < this._size)
			{
				while (i < this._size && match(this._items[i]))
				{
					i++;
				}
				if (i < this._size)
				{
					this._items[num++] = this._items[i++];
				}
			}
			Array.Clear(this._items, num, this._size - num);
			int num2 = this._size - num;
			this._size = num;
			this._version++;
			return num2;
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x00044DE8 File Offset: 0x00043DE8
		public void RemoveAt(int index)
		{
			if (index >= this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException();
			}
			this._size--;
			if (index < this._size)
			{
				Array.Copy(this._items, index + 1, this._items, index, this._size - index);
			}
			this._items[this._size] = default(T);
			this._version++;
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x00044E60 File Offset: 0x00043E60
		public void RemoveRange(int index, int count)
		{
			if (index < 0 || count < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException((index < 0) ? ExceptionArgument.index : ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (this._size - index < count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			if (count > 0)
			{
				this._size -= count;
				if (index < this._size)
				{
					Array.Copy(this._items, index + count, this._items, index, this._size - index);
				}
				Array.Clear(this._items, this._size, count);
				this._version++;
			}
		}

		// Token: 0x06001A30 RID: 6704 RVA: 0x00044EEF File Offset: 0x00043EEF
		public void Reverse()
		{
			this.Reverse(0, this.Count);
		}

		// Token: 0x06001A31 RID: 6705 RVA: 0x00044F00 File Offset: 0x00043F00
		public void Reverse(int index, int count)
		{
			if (index < 0 || count < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException((index < 0) ? ExceptionArgument.index : ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (this._size - index < count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			Array.Reverse(this._items, index, count);
			this._version++;
		}

		// Token: 0x06001A32 RID: 6706 RVA: 0x00044F52 File Offset: 0x00043F52
		public void Sort()
		{
			this.Sort(0, this.Count, null);
		}

		// Token: 0x06001A33 RID: 6707 RVA: 0x00044F62 File Offset: 0x00043F62
		public void Sort(IComparer<T> comparer)
		{
			this.Sort(0, this.Count, comparer);
		}

		// Token: 0x06001A34 RID: 6708 RVA: 0x00044F74 File Offset: 0x00043F74
		public void Sort(int index, int count, IComparer<T> comparer)
		{
			if (index < 0 || count < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException((index < 0) ? ExceptionArgument.index : ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (this._size - index < count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
			}
			Array.Sort<T>(this._items, index, count, comparer);
			this._version++;
		}

		// Token: 0x06001A35 RID: 6709 RVA: 0x00044FC8 File Offset: 0x00043FC8
		public void Sort(Comparison<T> comparison)
		{
			if (comparison == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			if (this._size > 0)
			{
				IComparer<T> comparer = new Array.FunctorComparer<T>(comparison);
				Array.Sort<T>(this._items, 0, this._size, comparer);
			}
		}

		// Token: 0x06001A36 RID: 6710 RVA: 0x00045004 File Offset: 0x00044004
		public T[] ToArray()
		{
			T[] array = new T[this._size];
			Array.Copy(this._items, 0, array, 0, this._size);
			return array;
		}

		// Token: 0x06001A37 RID: 6711 RVA: 0x00045034 File Offset: 0x00044034
		public void TrimExcess()
		{
			int num = (int)((double)this._items.Length * 0.9);
			if (this._size < num)
			{
				this.Capacity = this._size;
			}
		}

		// Token: 0x06001A38 RID: 6712 RVA: 0x0004506C File Offset: 0x0004406C
		public bool TrueForAll(Predicate<T> match)
		{
			if (match == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
			}
			for (int i = 0; i < this._size; i++)
			{
				if (!match(this._items[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040009E3 RID: 2531
		private const int _defaultCapacity = 4;

		// Token: 0x040009E4 RID: 2532
		private T[] _items;

		// Token: 0x040009E5 RID: 2533
		private int _size;

		// Token: 0x040009E6 RID: 2534
		private int _version;

		// Token: 0x040009E7 RID: 2535
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x040009E8 RID: 2536
		private static T[] _emptyArray = new T[0];

		// Token: 0x0200028F RID: 655
		[Serializable]
		public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
		{
			// Token: 0x06001A3A RID: 6714 RVA: 0x000450B7 File Offset: 0x000440B7
			internal Enumerator(List<T> list)
			{
				this.list = list;
				this.index = 0;
				this.version = list._version;
				this.current = default(T);
			}

			// Token: 0x06001A3B RID: 6715 RVA: 0x000450DF File Offset: 0x000440DF
			public void Dispose()
			{
			}

			// Token: 0x06001A3C RID: 6716 RVA: 0x000450E4 File Offset: 0x000440E4
			public bool MoveNext()
			{
				List<T> list = this.list;
				if (this.version == list._version && this.index < list._size)
				{
					this.current = list._items[this.index];
					this.index++;
					return true;
				}
				return this.MoveNextRare();
			}

			// Token: 0x06001A3D RID: 6717 RVA: 0x00045141 File Offset: 0x00044141
			private bool MoveNextRare()
			{
				if (this.version != this.list._version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				this.index = this.list._size + 1;
				this.current = default(T);
				return false;
			}

			// Token: 0x17000406 RID: 1030
			// (get) Token: 0x06001A3E RID: 6718 RVA: 0x0004517D File Offset: 0x0004417D
			public T Current
			{
				get
				{
					return this.current;
				}
			}

			// Token: 0x17000407 RID: 1031
			// (get) Token: 0x06001A3F RID: 6719 RVA: 0x00045185 File Offset: 0x00044185
			object IEnumerator.Current
			{
				get
				{
					if (this.index == 0 || this.index == this.list._size + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return this.Current;
				}
			}

			// Token: 0x06001A40 RID: 6720 RVA: 0x000451B6 File Offset: 0x000441B6
			void IEnumerator.Reset()
			{
				if (this.version != this.list._version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				this.index = 0;
				this.current = default(T);
			}

			// Token: 0x040009E9 RID: 2537
			private List<T> list;

			// Token: 0x040009EA RID: 2538
			private int index;

			// Token: 0x040009EB RID: 2539
			private int version;

			// Token: 0x040009EC RID: 2540
			private T current;
		}
	}
}
