using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Collections
{
	// Token: 0x02000236 RID: 566
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(ArrayList.ArrayListDebugView))]
	[ComVisible(true)]
	[Serializable]
	public class ArrayList : IList, ICollection, IEnumerable, ICloneable
	{
		// Token: 0x060015E0 RID: 5600 RVA: 0x00038212 File Offset: 0x00037212
		internal ArrayList(bool trash)
		{
		}

		// Token: 0x060015E1 RID: 5601 RVA: 0x0003821A File Offset: 0x0003721A
		public ArrayList()
		{
			this._items = ArrayList.emptyArray;
		}

		// Token: 0x060015E2 RID: 5602 RVA: 0x00038230 File Offset: 0x00037230
		public ArrayList(int capacity)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity", Environment.GetResourceString("ArgumentOutOfRange_MustBeNonNegNum", new object[] { "capacity" }));
			}
			this._items = new object[capacity];
		}

		// Token: 0x060015E3 RID: 5603 RVA: 0x00038278 File Offset: 0x00037278
		public ArrayList(ICollection c)
		{
			if (c == null)
			{
				throw new ArgumentNullException("c", Environment.GetResourceString("ArgumentNull_Collection"));
			}
			this._items = new object[c.Count];
			this.AddRange(c);
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x060015E4 RID: 5604 RVA: 0x000382B0 File Offset: 0x000372B0
		// (set) Token: 0x060015E5 RID: 5605 RVA: 0x000382BC File Offset: 0x000372BC
		public virtual int Capacity
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
						throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_SmallCapacity"));
					}
					if (value > 0)
					{
						object[] array = new object[value];
						if (this._size > 0)
						{
							Array.Copy(this._items, 0, array, 0, this._size);
						}
						this._items = array;
						return;
					}
					this._items = new object[4];
				}
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x060015E6 RID: 5606 RVA: 0x0003832E File Offset: 0x0003732E
		public virtual int Count
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x060015E7 RID: 5607 RVA: 0x00038336 File Offset: 0x00037336
		public virtual bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x060015E8 RID: 5608 RVA: 0x00038339 File Offset: 0x00037339
		public virtual bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x060015E9 RID: 5609 RVA: 0x0003833C File Offset: 0x0003733C
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x060015EA RID: 5610 RVA: 0x0003833F File Offset: 0x0003733F
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

		// Token: 0x170002F7 RID: 759
		public virtual object this[int index]
		{
			get
			{
				if (index < 0 || index >= this._size)
				{
					throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				return this._items[index];
			}
			set
			{
				if (index < 0 || index >= this._size)
				{
					throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				this._items[index] = value;
				this._version++;
			}
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x000383C8 File Offset: 0x000373C8
		public static ArrayList Adapter(IList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			return new ArrayList.IListWrapper(list);
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x000383E0 File Offset: 0x000373E0
		public virtual int Add(object value)
		{
			if (this._size == this._items.Length)
			{
				this.EnsureCapacity(this._size + 1);
			}
			this._items[this._size] = value;
			this._version++;
			return this._size++;
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x00038438 File Offset: 0x00037438
		public virtual void AddRange(ICollection c)
		{
			this.InsertRange(this._size, c);
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x00038448 File Offset: 0x00037448
		public virtual int BinarySearch(int index, int count, object value, IComparer comparer)
		{
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (this._size - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			return Array.BinarySearch(this._items, index, count, value, comparer);
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x000384A8 File Offset: 0x000374A8
		public virtual int BinarySearch(object value)
		{
			return this.BinarySearch(0, this.Count, value, null);
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x000384B9 File Offset: 0x000374B9
		public virtual int BinarySearch(object value, IComparer comparer)
		{
			return this.BinarySearch(0, this.Count, value, comparer);
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x000384CA File Offset: 0x000374CA
		public virtual void Clear()
		{
			if (this._size > 0)
			{
				Array.Clear(this._items, 0, this._size);
				this._size = 0;
			}
			this._version++;
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x000384FC File Offset: 0x000374FC
		public virtual object Clone()
		{
			ArrayList arrayList = new ArrayList(this._size);
			arrayList._size = this._size;
			arrayList._version = this._version;
			Array.Copy(this._items, 0, arrayList._items, 0, this._size);
			return arrayList;
		}

		// Token: 0x060015F5 RID: 5621 RVA: 0x00038548 File Offset: 0x00037548
		public virtual bool Contains(object item)
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
			for (int j = 0; j < this._size; j++)
			{
				if (this._items[j] != null && this._items[j].Equals(item))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060015F6 RID: 5622 RVA: 0x000385A5 File Offset: 0x000375A5
		public virtual void CopyTo(Array array)
		{
			this.CopyTo(array, 0);
		}

		// Token: 0x060015F7 RID: 5623 RVA: 0x000385AF File Offset: 0x000375AF
		public virtual void CopyTo(Array array, int arrayIndex)
		{
			if (array != null && array.Rank != 1)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_RankMultiDimNotSupported"));
			}
			Array.Copy(this._items, 0, array, arrayIndex, this._size);
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x000385E4 File Offset: 0x000375E4
		public virtual void CopyTo(int index, Array array, int arrayIndex, int count)
		{
			if (this._size - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (array != null && array.Rank != 1)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_RankMultiDimNotSupported"));
			}
			Array.Copy(this._items, index, array, arrayIndex, count);
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x0003863C File Offset: 0x0003763C
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

		// Token: 0x060015FA RID: 5626 RVA: 0x00038679 File Offset: 0x00037679
		public static IList FixedSize(IList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			return new ArrayList.FixedSizeList(list);
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x0003868F File Offset: 0x0003768F
		public static ArrayList FixedSize(ArrayList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			return new ArrayList.FixedSizeArrayList(list);
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x000386A5 File Offset: 0x000376A5
		public virtual IEnumerator GetEnumerator()
		{
			return new ArrayList.ArrayListEnumeratorSimple(this);
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x000386B0 File Offset: 0x000376B0
		public virtual IEnumerator GetEnumerator(int index, int count)
		{
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (this._size - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			return new ArrayList.ArrayListEnumerator(this, index, count);
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x00038708 File Offset: 0x00037708
		public virtual int IndexOf(object value)
		{
			return Array.IndexOf(this._items, value, 0, this._size);
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x0003871D File Offset: 0x0003771D
		public virtual int IndexOf(object value, int startIndex)
		{
			if (startIndex > this._size)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			return Array.IndexOf(this._items, value, startIndex, this._size - startIndex);
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x00038754 File Offset: 0x00037754
		public virtual int IndexOf(object value, int startIndex, int count)
		{
			if (startIndex > this._size)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (count < 0 || startIndex > this._size - count)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
			}
			return Array.IndexOf(this._items, value, startIndex, count);
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x000387B4 File Offset: 0x000377B4
		public virtual void Insert(int index, object value)
		{
			if (index < 0 || index > this._size)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_ArrayListInsert"));
			}
			if (this._size == this._items.Length)
			{
				this.EnsureCapacity(this._size + 1);
			}
			if (index < this._size)
			{
				Array.Copy(this._items, index, this._items, index + 1, this._size - index);
			}
			this._items[index] = value;
			this._size++;
			this._version++;
		}

		// Token: 0x06001602 RID: 5634 RVA: 0x0003884C File Offset: 0x0003784C
		public virtual void InsertRange(int index, ICollection c)
		{
			if (c == null)
			{
				throw new ArgumentNullException("c", Environment.GetResourceString("ArgumentNull_Collection"));
			}
			if (index < 0 || index > this._size)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			int count = c.Count;
			if (count > 0)
			{
				this.EnsureCapacity(this._size + count);
				if (index < this._size)
				{
					Array.Copy(this._items, index, this._items, index + count, this._size - index);
				}
				object[] array = new object[count];
				c.CopyTo(array, 0);
				array.CopyTo(this._items, index);
				this._size += count;
				this._version++;
			}
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x0003890A File Offset: 0x0003790A
		public virtual int LastIndexOf(object value)
		{
			return this.LastIndexOf(value, this._size - 1, this._size);
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x00038921 File Offset: 0x00037921
		public virtual int LastIndexOf(object value, int startIndex)
		{
			if (startIndex >= this._size)
			{
				throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			return this.LastIndexOf(value, startIndex, startIndex + 1);
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x0003894C File Offset: 0x0003794C
		public virtual int LastIndexOf(object value, int startIndex, int count)
		{
			if (this._size == 0)
			{
				return -1;
			}
			if (startIndex < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((startIndex < 0) ? "startIndex" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (startIndex >= this._size || count > startIndex + 1)
			{
				throw new ArgumentOutOfRangeException((startIndex >= this._size) ? "startIndex" : "count", Environment.GetResourceString("ArgumentOutOfRange_BiggerThanCollection"));
			}
			return Array.LastIndexOf(this._items, value, startIndex, count);
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x000389CD File Offset: 0x000379CD
		public static IList ReadOnly(IList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			return new ArrayList.ReadOnlyList(list);
		}

		// Token: 0x06001607 RID: 5639 RVA: 0x000389E3 File Offset: 0x000379E3
		public static ArrayList ReadOnly(ArrayList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			return new ArrayList.ReadOnlyArrayList(list);
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x000389FC File Offset: 0x000379FC
		public virtual void Remove(object obj)
		{
			int num = this.IndexOf(obj);
			if (num >= 0)
			{
				this.RemoveAt(num);
			}
		}

		// Token: 0x06001609 RID: 5641 RVA: 0x00038A1C File Offset: 0x00037A1C
		public virtual void RemoveAt(int index)
		{
			if (index < 0 || index >= this._size)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			this._size--;
			if (index < this._size)
			{
				Array.Copy(this._items, index + 1, this._items, index, this._size - index);
			}
			this._items[this._size] = null;
			this._version++;
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x00038A9C File Offset: 0x00037A9C
		public virtual void RemoveRange(int index, int count)
		{
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (this._size - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (count > 0)
			{
				int i = this._size;
				this._size -= count;
				if (index < this._size)
				{
					Array.Copy(this._items, index + count, this._items, index, this._size - index);
				}
				while (i > this._size)
				{
					this._items[--i] = null;
				}
				this._version++;
			}
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x00038B54 File Offset: 0x00037B54
		public static ArrayList Repeat(object value, int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			ArrayList arrayList = new ArrayList((count > 4) ? count : 4);
			for (int i = 0; i < count; i++)
			{
				arrayList.Add(value);
			}
			return arrayList;
		}

		// Token: 0x0600160C RID: 5644 RVA: 0x00038B9D File Offset: 0x00037B9D
		public virtual void Reverse()
		{
			this.Reverse(0, this.Count);
		}

		// Token: 0x0600160D RID: 5645 RVA: 0x00038BAC File Offset: 0x00037BAC
		public virtual void Reverse(int index, int count)
		{
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (this._size - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			Array.Reverse(this._items, index, count);
			this._version++;
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x00038C18 File Offset: 0x00037C18
		public virtual void SetRange(int index, ICollection c)
		{
			if (c == null)
			{
				throw new ArgumentNullException("c", Environment.GetResourceString("ArgumentNull_Collection"));
			}
			int count = c.Count;
			if (index < 0 || index > this._size - count)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (count > 0)
			{
				c.CopyTo(this._items, index);
				this._version++;
			}
		}

		// Token: 0x0600160F RID: 5647 RVA: 0x00038C88 File Offset: 0x00037C88
		public virtual ArrayList GetRange(int index, int count)
		{
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (this._size - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			return new ArrayList.Range(this, index, count);
		}

		// Token: 0x06001610 RID: 5648 RVA: 0x00038CE0 File Offset: 0x00037CE0
		public virtual void Sort()
		{
			this.Sort(0, this.Count, Comparer.Default);
		}

		// Token: 0x06001611 RID: 5649 RVA: 0x00038CF4 File Offset: 0x00037CF4
		public virtual void Sort(IComparer comparer)
		{
			this.Sort(0, this.Count, comparer);
		}

		// Token: 0x06001612 RID: 5650 RVA: 0x00038D04 File Offset: 0x00037D04
		public virtual void Sort(int index, int count, IComparer comparer)
		{
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (this._size - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			Array.Sort(this._items, index, count, comparer);
			this._version++;
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x00038D70 File Offset: 0x00037D70
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public static IList Synchronized(IList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			return new ArrayList.SyncIList(list);
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x00038D86 File Offset: 0x00037D86
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public static ArrayList Synchronized(ArrayList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			return new ArrayList.SyncArrayList(list);
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x00038D9C File Offset: 0x00037D9C
		public virtual object[] ToArray()
		{
			object[] array = new object[this._size];
			Array.Copy(this._items, 0, array, 0, this._size);
			return array;
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x00038DCC File Offset: 0x00037DCC
		public virtual Array ToArray(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			Array array = Array.CreateInstance(type, this._size);
			Array.Copy(this._items, 0, array, 0, this._size);
			return array;
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x00038E09 File Offset: 0x00037E09
		public virtual void TrimToSize()
		{
			this.Capacity = this._size;
		}

		// Token: 0x04000906 RID: 2310
		private const int _defaultCapacity = 4;

		// Token: 0x04000907 RID: 2311
		private object[] _items;

		// Token: 0x04000908 RID: 2312
		private int _size;

		// Token: 0x04000909 RID: 2313
		private int _version;

		// Token: 0x0400090A RID: 2314
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x0400090B RID: 2315
		private static readonly object[] emptyArray = new object[0];

		// Token: 0x02000237 RID: 567
		[Serializable]
		private class IListWrapper : ArrayList
		{
			// Token: 0x06001619 RID: 5657 RVA: 0x00038E24 File Offset: 0x00037E24
			internal IListWrapper(IList list)
			{
				this._list = list;
				this._version = 0;
			}

			// Token: 0x170002F8 RID: 760
			// (get) Token: 0x0600161A RID: 5658 RVA: 0x00038E3A File Offset: 0x00037E3A
			// (set) Token: 0x0600161B RID: 5659 RVA: 0x00038E47 File Offset: 0x00037E47
			public override int Capacity
			{
				get
				{
					return this._list.Count;
				}
				set
				{
					if (value < this._list.Count)
					{
						throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_SmallCapacity"));
					}
				}
			}

			// Token: 0x170002F9 RID: 761
			// (get) Token: 0x0600161C RID: 5660 RVA: 0x00038E6C File Offset: 0x00037E6C
			public override int Count
			{
				get
				{
					return this._list.Count;
				}
			}

			// Token: 0x170002FA RID: 762
			// (get) Token: 0x0600161D RID: 5661 RVA: 0x00038E79 File Offset: 0x00037E79
			public override bool IsReadOnly
			{
				get
				{
					return this._list.IsReadOnly;
				}
			}

			// Token: 0x170002FB RID: 763
			// (get) Token: 0x0600161E RID: 5662 RVA: 0x00038E86 File Offset: 0x00037E86
			public override bool IsFixedSize
			{
				get
				{
					return this._list.IsFixedSize;
				}
			}

			// Token: 0x170002FC RID: 764
			// (get) Token: 0x0600161F RID: 5663 RVA: 0x00038E93 File Offset: 0x00037E93
			public override bool IsSynchronized
			{
				get
				{
					return this._list.IsSynchronized;
				}
			}

			// Token: 0x170002FD RID: 765
			public override object this[int index]
			{
				get
				{
					return this._list[index];
				}
				set
				{
					this._list[index] = value;
					this._version++;
				}
			}

			// Token: 0x170002FE RID: 766
			// (get) Token: 0x06001622 RID: 5666 RVA: 0x00038ECB File Offset: 0x00037ECB
			public override object SyncRoot
			{
				get
				{
					return this._list.SyncRoot;
				}
			}

			// Token: 0x06001623 RID: 5667 RVA: 0x00038ED8 File Offset: 0x00037ED8
			public override int Add(object obj)
			{
				int num = this._list.Add(obj);
				this._version++;
				return num;
			}

			// Token: 0x06001624 RID: 5668 RVA: 0x00038F01 File Offset: 0x00037F01
			public override void AddRange(ICollection c)
			{
				this.InsertRange(this.Count, c);
			}

			// Token: 0x06001625 RID: 5669 RVA: 0x00038F10 File Offset: 0x00037F10
			public override int BinarySearch(int index, int count, object value, IComparer comparer)
			{
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._list.Count - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				if (comparer == null)
				{
					comparer = Comparer.Default;
				}
				int i = index;
				int num = index + count - 1;
				while (i <= num)
				{
					int num2 = (i + num) / 2;
					int num3 = comparer.Compare(value, this._list[num2]);
					if (num3 == 0)
					{
						return num2;
					}
					if (num3 < 0)
					{
						num = num2 - 1;
					}
					else
					{
						i = num2 + 1;
					}
				}
				return ~i;
			}

			// Token: 0x06001626 RID: 5670 RVA: 0x00038FAE File Offset: 0x00037FAE
			public override void Clear()
			{
				if (this._list.IsFixedSize)
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
				}
				this._list.Clear();
				this._version++;
			}

			// Token: 0x06001627 RID: 5671 RVA: 0x00038FE6 File Offset: 0x00037FE6
			public override object Clone()
			{
				return new ArrayList.IListWrapper(this._list);
			}

			// Token: 0x06001628 RID: 5672 RVA: 0x00038FF3 File Offset: 0x00037FF3
			public override bool Contains(object obj)
			{
				return this._list.Contains(obj);
			}

			// Token: 0x06001629 RID: 5673 RVA: 0x00039001 File Offset: 0x00038001
			public override void CopyTo(Array array, int index)
			{
				this._list.CopyTo(array, index);
			}

			// Token: 0x0600162A RID: 5674 RVA: 0x00039010 File Offset: 0x00038010
			public override void CopyTo(int index, Array array, int arrayIndex, int count)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (index < 0 || arrayIndex < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "arrayIndex", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (count < 0)
				{
					throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (array.Length - arrayIndex < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				if (this._list.Count - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				if (array.Rank != 1)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_RankMultiDimNotSupported"));
				}
				for (int i = index; i < index + count; i++)
				{
					array.SetValue(this._list[i], arrayIndex++);
				}
			}

			// Token: 0x0600162B RID: 5675 RVA: 0x000390EA File Offset: 0x000380EA
			public override IEnumerator GetEnumerator()
			{
				return this._list.GetEnumerator();
			}

			// Token: 0x0600162C RID: 5676 RVA: 0x000390F8 File Offset: 0x000380F8
			public override IEnumerator GetEnumerator(int index, int count)
			{
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._list.Count - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				return new ArrayList.IListWrapper.IListWrapperEnumWrapper(this, index, count);
			}

			// Token: 0x0600162D RID: 5677 RVA: 0x00039155 File Offset: 0x00038155
			public override int IndexOf(object value)
			{
				return this._list.IndexOf(value);
			}

			// Token: 0x0600162E RID: 5678 RVA: 0x00039163 File Offset: 0x00038163
			public override int IndexOf(object value, int startIndex)
			{
				return this.IndexOf(value, startIndex, this._list.Count - startIndex);
			}

			// Token: 0x0600162F RID: 5679 RVA: 0x0003917C File Offset: 0x0003817C
			public override int IndexOf(object value, int startIndex, int count)
			{
				if (startIndex < 0 || startIndex > this._list.Count)
				{
					throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				if (count < 0 || startIndex > this._list.Count - count)
				{
					throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
				}
				int num = startIndex + count;
				if (value == null)
				{
					for (int i = startIndex; i < num; i++)
					{
						if (this._list[i] == null)
						{
							return i;
						}
					}
					return -1;
				}
				for (int j = startIndex; j < num; j++)
				{
					if (this._list[j] != null && this._list[j].Equals(value))
					{
						return j;
					}
				}
				return -1;
			}

			// Token: 0x06001630 RID: 5680 RVA: 0x0003922F File Offset: 0x0003822F
			public override void Insert(int index, object obj)
			{
				this._list.Insert(index, obj);
				this._version++;
			}

			// Token: 0x06001631 RID: 5681 RVA: 0x0003924C File Offset: 0x0003824C
			public override void InsertRange(int index, ICollection c)
			{
				if (c == null)
				{
					throw new ArgumentNullException("c", Environment.GetResourceString("ArgumentNull_Collection"));
				}
				if (index < 0 || index > this._list.Count)
				{
					throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				if (c.Count > 0)
				{
					ArrayList arrayList = this._list as ArrayList;
					if (arrayList != null)
					{
						arrayList.InsertRange(index, c);
					}
					else
					{
						foreach (object obj in c)
						{
							this._list.Insert(index++, obj);
						}
					}
					this._version++;
				}
			}

			// Token: 0x06001632 RID: 5682 RVA: 0x000392F0 File Offset: 0x000382F0
			public override int LastIndexOf(object value)
			{
				return this.LastIndexOf(value, this._list.Count - 1, this._list.Count);
			}

			// Token: 0x06001633 RID: 5683 RVA: 0x00039311 File Offset: 0x00038311
			public override int LastIndexOf(object value, int startIndex)
			{
				return this.LastIndexOf(value, startIndex, startIndex + 1);
			}

			// Token: 0x06001634 RID: 5684 RVA: 0x00039320 File Offset: 0x00038320
			public override int LastIndexOf(object value, int startIndex, int count)
			{
				if (this._list.Count == 0)
				{
					return -1;
				}
				if (startIndex < 0 || startIndex >= this._list.Count)
				{
					throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				if (count < 0 || count > startIndex + 1)
				{
					throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
				}
				int num = startIndex - count + 1;
				if (value == null)
				{
					for (int i = startIndex; i >= num; i--)
					{
						if (this._list[i] == null)
						{
							return i;
						}
					}
					return -1;
				}
				for (int j = startIndex; j >= num; j--)
				{
					if (this._list[j] != null && this._list[j].Equals(value))
					{
						return j;
					}
				}
				return -1;
			}

			// Token: 0x06001635 RID: 5685 RVA: 0x000393DC File Offset: 0x000383DC
			public override void Remove(object value)
			{
				int num = this.IndexOf(value);
				if (num >= 0)
				{
					this.RemoveAt(num);
				}
			}

			// Token: 0x06001636 RID: 5686 RVA: 0x000393FC File Offset: 0x000383FC
			public override void RemoveAt(int index)
			{
				this._list.RemoveAt(index);
				this._version++;
			}

			// Token: 0x06001637 RID: 5687 RVA: 0x00039418 File Offset: 0x00038418
			public override void RemoveRange(int index, int count)
			{
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._list.Count - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				if (count > 0)
				{
					this._version++;
				}
				while (count > 0)
				{
					this._list.RemoveAt(index);
					count--;
				}
			}

			// Token: 0x06001638 RID: 5688 RVA: 0x00039498 File Offset: 0x00038498
			public override void Reverse(int index, int count)
			{
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._list.Count - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				int i = index;
				int num = index + count - 1;
				while (i < num)
				{
					object obj = this._list[i];
					this._list[i++] = this._list[num];
					this._list[num--] = obj;
				}
				this._version++;
			}

			// Token: 0x06001639 RID: 5689 RVA: 0x00039544 File Offset: 0x00038544
			public override void SetRange(int index, ICollection c)
			{
				if (c == null)
				{
					throw new ArgumentNullException("c", Environment.GetResourceString("ArgumentNull_Collection"));
				}
				if (index < 0 || index > this._list.Count - c.Count)
				{
					throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				if (c.Count > 0)
				{
					foreach (object obj in c)
					{
						this._list[index++] = obj;
					}
					this._version++;
				}
			}

			// Token: 0x0600163A RID: 5690 RVA: 0x000395D8 File Offset: 0x000385D8
			public override ArrayList GetRange(int index, int count)
			{
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._list.Count - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				return new ArrayList.Range(this, index, count);
			}

			// Token: 0x0600163B RID: 5691 RVA: 0x00039638 File Offset: 0x00038638
			public override void Sort(int index, int count, IComparer comparer)
			{
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._list.Count - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				object[] array = new object[count];
				this.CopyTo(index, array, 0, count);
				Array.Sort(array, 0, count, comparer);
				for (int i = 0; i < count; i++)
				{
					this._list[i + index] = array[i];
				}
				this._version++;
			}

			// Token: 0x0600163C RID: 5692 RVA: 0x000396D4 File Offset: 0x000386D4
			public override object[] ToArray()
			{
				object[] array = new object[this.Count];
				this._list.CopyTo(array, 0);
				return array;
			}

			// Token: 0x0600163D RID: 5693 RVA: 0x000396FC File Offset: 0x000386FC
			public override Array ToArray(Type type)
			{
				if (type == null)
				{
					throw new ArgumentNullException("type");
				}
				Array array = Array.CreateInstance(type, this._list.Count);
				this._list.CopyTo(array, 0);
				return array;
			}

			// Token: 0x0600163E RID: 5694 RVA: 0x00039737 File Offset: 0x00038737
			public override void TrimToSize()
			{
			}

			// Token: 0x0400090C RID: 2316
			private IList _list;

			// Token: 0x02000238 RID: 568
			[Serializable]
			private sealed class IListWrapperEnumWrapper : IEnumerator, ICloneable
			{
				// Token: 0x0600163F RID: 5695 RVA: 0x00039739 File Offset: 0x00038739
				private IListWrapperEnumWrapper()
				{
				}

				// Token: 0x06001640 RID: 5696 RVA: 0x00039744 File Offset: 0x00038744
				internal IListWrapperEnumWrapper(ArrayList.IListWrapper listWrapper, int startIndex, int count)
				{
					this._en = listWrapper.GetEnumerator();
					this._initialStartIndex = startIndex;
					this._initialCount = count;
					while (startIndex-- > 0 && this._en.MoveNext())
					{
					}
					this._remaining = count;
					this._firstCall = true;
				}

				// Token: 0x06001641 RID: 5697 RVA: 0x00039798 File Offset: 0x00038798
				public object Clone()
				{
					return new ArrayList.IListWrapper.IListWrapperEnumWrapper
					{
						_en = (IEnumerator)((ICloneable)this._en).Clone(),
						_initialStartIndex = this._initialStartIndex,
						_initialCount = this._initialCount,
						_remaining = this._remaining,
						_firstCall = this._firstCall
					};
				}

				// Token: 0x06001642 RID: 5698 RVA: 0x000397F8 File Offset: 0x000387F8
				public bool MoveNext()
				{
					if (this._firstCall)
					{
						this._firstCall = false;
						return this._remaining-- > 0 && this._en.MoveNext();
					}
					if (this._remaining < 0)
					{
						return false;
					}
					bool flag = this._en.MoveNext();
					return flag && this._remaining-- > 0;
				}

				// Token: 0x170002FF RID: 767
				// (get) Token: 0x06001643 RID: 5699 RVA: 0x00039866 File Offset: 0x00038866
				public object Current
				{
					get
					{
						if (this._firstCall)
						{
							throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
						}
						if (this._remaining < 0)
						{
							throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
						}
						return this._en.Current;
					}
				}

				// Token: 0x06001644 RID: 5700 RVA: 0x000398A4 File Offset: 0x000388A4
				public void Reset()
				{
					this._en.Reset();
					int initialStartIndex = this._initialStartIndex;
					while (initialStartIndex-- > 0 && this._en.MoveNext())
					{
					}
					this._remaining = this._initialCount;
					this._firstCall = true;
				}

				// Token: 0x0400090D RID: 2317
				private IEnumerator _en;

				// Token: 0x0400090E RID: 2318
				private int _remaining;

				// Token: 0x0400090F RID: 2319
				private int _initialStartIndex;

				// Token: 0x04000910 RID: 2320
				private int _initialCount;

				// Token: 0x04000911 RID: 2321
				private bool _firstCall;
			}
		}

		// Token: 0x02000239 RID: 569
		[Serializable]
		private class SyncArrayList : ArrayList
		{
			// Token: 0x06001645 RID: 5701 RVA: 0x000398EB File Offset: 0x000388EB
			internal SyncArrayList(ArrayList list)
				: base(false)
			{
				this._list = list;
				this._root = list.SyncRoot;
			}

			// Token: 0x17000300 RID: 768
			// (get) Token: 0x06001646 RID: 5702 RVA: 0x00039908 File Offset: 0x00038908
			// (set) Token: 0x06001647 RID: 5703 RVA: 0x00039948 File Offset: 0x00038948
			public override int Capacity
			{
				get
				{
					int capacity;
					lock (this._root)
					{
						capacity = this._list.Capacity;
					}
					return capacity;
				}
				set
				{
					lock (this._root)
					{
						this._list.Capacity = value;
					}
				}
			}

			// Token: 0x17000301 RID: 769
			// (get) Token: 0x06001648 RID: 5704 RVA: 0x00039988 File Offset: 0x00038988
			public override int Count
			{
				get
				{
					int count;
					lock (this._root)
					{
						count = this._list.Count;
					}
					return count;
				}
			}

			// Token: 0x17000302 RID: 770
			// (get) Token: 0x06001649 RID: 5705 RVA: 0x000399C8 File Offset: 0x000389C8
			public override bool IsReadOnly
			{
				get
				{
					return this._list.IsReadOnly;
				}
			}

			// Token: 0x17000303 RID: 771
			// (get) Token: 0x0600164A RID: 5706 RVA: 0x000399D5 File Offset: 0x000389D5
			public override bool IsFixedSize
			{
				get
				{
					return this._list.IsFixedSize;
				}
			}

			// Token: 0x17000304 RID: 772
			// (get) Token: 0x0600164B RID: 5707 RVA: 0x000399E2 File Offset: 0x000389E2
			public override bool IsSynchronized
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000305 RID: 773
			public override object this[int index]
			{
				get
				{
					object obj;
					lock (this._root)
					{
						obj = this._list[index];
					}
					return obj;
				}
				set
				{
					lock (this._root)
					{
						this._list[index] = value;
					}
				}
			}

			// Token: 0x17000306 RID: 774
			// (get) Token: 0x0600164E RID: 5710 RVA: 0x00039A6C File Offset: 0x00038A6C
			public override object SyncRoot
			{
				get
				{
					return this._root;
				}
			}

			// Token: 0x0600164F RID: 5711 RVA: 0x00039A74 File Offset: 0x00038A74
			public override int Add(object value)
			{
				int num;
				lock (this._root)
				{
					num = this._list.Add(value);
				}
				return num;
			}

			// Token: 0x06001650 RID: 5712 RVA: 0x00039AB8 File Offset: 0x00038AB8
			public override void AddRange(ICollection c)
			{
				lock (this._root)
				{
					this._list.AddRange(c);
				}
			}

			// Token: 0x06001651 RID: 5713 RVA: 0x00039AF8 File Offset: 0x00038AF8
			public override int BinarySearch(object value)
			{
				int num;
				lock (this._root)
				{
					num = this._list.BinarySearch(value);
				}
				return num;
			}

			// Token: 0x06001652 RID: 5714 RVA: 0x00039B3C File Offset: 0x00038B3C
			public override int BinarySearch(object value, IComparer comparer)
			{
				int num;
				lock (this._root)
				{
					num = this._list.BinarySearch(value, comparer);
				}
				return num;
			}

			// Token: 0x06001653 RID: 5715 RVA: 0x00039B80 File Offset: 0x00038B80
			public override int BinarySearch(int index, int count, object value, IComparer comparer)
			{
				int num;
				lock (this._root)
				{
					num = this._list.BinarySearch(index, count, value, comparer);
				}
				return num;
			}

			// Token: 0x06001654 RID: 5716 RVA: 0x00039BC8 File Offset: 0x00038BC8
			public override void Clear()
			{
				lock (this._root)
				{
					this._list.Clear();
				}
			}

			// Token: 0x06001655 RID: 5717 RVA: 0x00039C08 File Offset: 0x00038C08
			public override object Clone()
			{
				object obj;
				lock (this._root)
				{
					obj = new ArrayList.SyncArrayList((ArrayList)this._list.Clone());
				}
				return obj;
			}

			// Token: 0x06001656 RID: 5718 RVA: 0x00039C54 File Offset: 0x00038C54
			public override bool Contains(object item)
			{
				bool flag;
				lock (this._root)
				{
					flag = this._list.Contains(item);
				}
				return flag;
			}

			// Token: 0x06001657 RID: 5719 RVA: 0x00039C98 File Offset: 0x00038C98
			public override void CopyTo(Array array)
			{
				lock (this._root)
				{
					this._list.CopyTo(array);
				}
			}

			// Token: 0x06001658 RID: 5720 RVA: 0x00039CD8 File Offset: 0x00038CD8
			public override void CopyTo(Array array, int index)
			{
				lock (this._root)
				{
					this._list.CopyTo(array, index);
				}
			}

			// Token: 0x06001659 RID: 5721 RVA: 0x00039D18 File Offset: 0x00038D18
			public override void CopyTo(int index, Array array, int arrayIndex, int count)
			{
				lock (this._root)
				{
					this._list.CopyTo(index, array, arrayIndex, count);
				}
			}

			// Token: 0x0600165A RID: 5722 RVA: 0x00039D5C File Offset: 0x00038D5C
			public override IEnumerator GetEnumerator()
			{
				IEnumerator enumerator;
				lock (this._root)
				{
					enumerator = this._list.GetEnumerator();
				}
				return enumerator;
			}

			// Token: 0x0600165B RID: 5723 RVA: 0x00039D9C File Offset: 0x00038D9C
			public override IEnumerator GetEnumerator(int index, int count)
			{
				IEnumerator enumerator;
				lock (this._root)
				{
					enumerator = this._list.GetEnumerator(index, count);
				}
				return enumerator;
			}

			// Token: 0x0600165C RID: 5724 RVA: 0x00039DE0 File Offset: 0x00038DE0
			public override int IndexOf(object value)
			{
				int num;
				lock (this._root)
				{
					num = this._list.IndexOf(value);
				}
				return num;
			}

			// Token: 0x0600165D RID: 5725 RVA: 0x00039E24 File Offset: 0x00038E24
			public override int IndexOf(object value, int startIndex)
			{
				int num;
				lock (this._root)
				{
					num = this._list.IndexOf(value, startIndex);
				}
				return num;
			}

			// Token: 0x0600165E RID: 5726 RVA: 0x00039E68 File Offset: 0x00038E68
			public override int IndexOf(object value, int startIndex, int count)
			{
				int num;
				lock (this._root)
				{
					num = this._list.IndexOf(value, startIndex, count);
				}
				return num;
			}

			// Token: 0x0600165F RID: 5727 RVA: 0x00039EAC File Offset: 0x00038EAC
			public override void Insert(int index, object value)
			{
				lock (this._root)
				{
					this._list.Insert(index, value);
				}
			}

			// Token: 0x06001660 RID: 5728 RVA: 0x00039EEC File Offset: 0x00038EEC
			public override void InsertRange(int index, ICollection c)
			{
				lock (this._root)
				{
					this._list.InsertRange(index, c);
				}
			}

			// Token: 0x06001661 RID: 5729 RVA: 0x00039F2C File Offset: 0x00038F2C
			public override int LastIndexOf(object value)
			{
				int num;
				lock (this._root)
				{
					num = this._list.LastIndexOf(value);
				}
				return num;
			}

			// Token: 0x06001662 RID: 5730 RVA: 0x00039F70 File Offset: 0x00038F70
			public override int LastIndexOf(object value, int startIndex)
			{
				int num;
				lock (this._root)
				{
					num = this._list.LastIndexOf(value, startIndex);
				}
				return num;
			}

			// Token: 0x06001663 RID: 5731 RVA: 0x00039FB4 File Offset: 0x00038FB4
			public override int LastIndexOf(object value, int startIndex, int count)
			{
				int num;
				lock (this._root)
				{
					num = this._list.LastIndexOf(value, startIndex, count);
				}
				return num;
			}

			// Token: 0x06001664 RID: 5732 RVA: 0x00039FF8 File Offset: 0x00038FF8
			public override void Remove(object value)
			{
				lock (this._root)
				{
					this._list.Remove(value);
				}
			}

			// Token: 0x06001665 RID: 5733 RVA: 0x0003A038 File Offset: 0x00039038
			public override void RemoveAt(int index)
			{
				lock (this._root)
				{
					this._list.RemoveAt(index);
				}
			}

			// Token: 0x06001666 RID: 5734 RVA: 0x0003A078 File Offset: 0x00039078
			public override void RemoveRange(int index, int count)
			{
				lock (this._root)
				{
					this._list.RemoveRange(index, count);
				}
			}

			// Token: 0x06001667 RID: 5735 RVA: 0x0003A0B8 File Offset: 0x000390B8
			public override void Reverse(int index, int count)
			{
				lock (this._root)
				{
					this._list.Reverse(index, count);
				}
			}

			// Token: 0x06001668 RID: 5736 RVA: 0x0003A0F8 File Offset: 0x000390F8
			public override void SetRange(int index, ICollection c)
			{
				lock (this._root)
				{
					this._list.SetRange(index, c);
				}
			}

			// Token: 0x06001669 RID: 5737 RVA: 0x0003A138 File Offset: 0x00039138
			public override ArrayList GetRange(int index, int count)
			{
				ArrayList range;
				lock (this._root)
				{
					range = this._list.GetRange(index, count);
				}
				return range;
			}

			// Token: 0x0600166A RID: 5738 RVA: 0x0003A17C File Offset: 0x0003917C
			public override void Sort()
			{
				lock (this._root)
				{
					this._list.Sort();
				}
			}

			// Token: 0x0600166B RID: 5739 RVA: 0x0003A1BC File Offset: 0x000391BC
			public override void Sort(IComparer comparer)
			{
				lock (this._root)
				{
					this._list.Sort(comparer);
				}
			}

			// Token: 0x0600166C RID: 5740 RVA: 0x0003A1FC File Offset: 0x000391FC
			public override void Sort(int index, int count, IComparer comparer)
			{
				lock (this._root)
				{
					this._list.Sort(index, count, comparer);
				}
			}

			// Token: 0x0600166D RID: 5741 RVA: 0x0003A240 File Offset: 0x00039240
			public override object[] ToArray()
			{
				object[] array;
				lock (this._root)
				{
					array = this._list.ToArray();
				}
				return array;
			}

			// Token: 0x0600166E RID: 5742 RVA: 0x0003A280 File Offset: 0x00039280
			public override Array ToArray(Type type)
			{
				Array array;
				lock (this._root)
				{
					array = this._list.ToArray(type);
				}
				return array;
			}

			// Token: 0x0600166F RID: 5743 RVA: 0x0003A2C4 File Offset: 0x000392C4
			public override void TrimToSize()
			{
				lock (this._root)
				{
					this._list.TrimToSize();
				}
			}

			// Token: 0x04000912 RID: 2322
			private ArrayList _list;

			// Token: 0x04000913 RID: 2323
			private object _root;
		}

		// Token: 0x0200023A RID: 570
		[Serializable]
		private class SyncIList : IList, ICollection, IEnumerable
		{
			// Token: 0x06001670 RID: 5744 RVA: 0x0003A304 File Offset: 0x00039304
			internal SyncIList(IList list)
			{
				this._list = list;
				this._root = list.SyncRoot;
			}

			// Token: 0x17000307 RID: 775
			// (get) Token: 0x06001671 RID: 5745 RVA: 0x0003A320 File Offset: 0x00039320
			public virtual int Count
			{
				get
				{
					int count;
					lock (this._root)
					{
						count = this._list.Count;
					}
					return count;
				}
			}

			// Token: 0x17000308 RID: 776
			// (get) Token: 0x06001672 RID: 5746 RVA: 0x0003A360 File Offset: 0x00039360
			public virtual bool IsReadOnly
			{
				get
				{
					return this._list.IsReadOnly;
				}
			}

			// Token: 0x17000309 RID: 777
			// (get) Token: 0x06001673 RID: 5747 RVA: 0x0003A36D File Offset: 0x0003936D
			public virtual bool IsFixedSize
			{
				get
				{
					return this._list.IsFixedSize;
				}
			}

			// Token: 0x1700030A RID: 778
			// (get) Token: 0x06001674 RID: 5748 RVA: 0x0003A37A File Offset: 0x0003937A
			public virtual bool IsSynchronized
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700030B RID: 779
			public virtual object this[int index]
			{
				get
				{
					object obj;
					lock (this._root)
					{
						obj = this._list[index];
					}
					return obj;
				}
				set
				{
					lock (this._root)
					{
						this._list[index] = value;
					}
				}
			}

			// Token: 0x1700030C RID: 780
			// (get) Token: 0x06001677 RID: 5751 RVA: 0x0003A404 File Offset: 0x00039404
			public virtual object SyncRoot
			{
				get
				{
					return this._root;
				}
			}

			// Token: 0x06001678 RID: 5752 RVA: 0x0003A40C File Offset: 0x0003940C
			public virtual int Add(object value)
			{
				int num;
				lock (this._root)
				{
					num = this._list.Add(value);
				}
				return num;
			}

			// Token: 0x06001679 RID: 5753 RVA: 0x0003A450 File Offset: 0x00039450
			public virtual void Clear()
			{
				lock (this._root)
				{
					this._list.Clear();
				}
			}

			// Token: 0x0600167A RID: 5754 RVA: 0x0003A490 File Offset: 0x00039490
			public virtual bool Contains(object item)
			{
				bool flag;
				lock (this._root)
				{
					flag = this._list.Contains(item);
				}
				return flag;
			}

			// Token: 0x0600167B RID: 5755 RVA: 0x0003A4D4 File Offset: 0x000394D4
			public virtual void CopyTo(Array array, int index)
			{
				lock (this._root)
				{
					this._list.CopyTo(array, index);
				}
			}

			// Token: 0x0600167C RID: 5756 RVA: 0x0003A514 File Offset: 0x00039514
			public virtual IEnumerator GetEnumerator()
			{
				IEnumerator enumerator;
				lock (this._root)
				{
					enumerator = this._list.GetEnumerator();
				}
				return enumerator;
			}

			// Token: 0x0600167D RID: 5757 RVA: 0x0003A554 File Offset: 0x00039554
			public virtual int IndexOf(object value)
			{
				int num;
				lock (this._root)
				{
					num = this._list.IndexOf(value);
				}
				return num;
			}

			// Token: 0x0600167E RID: 5758 RVA: 0x0003A598 File Offset: 0x00039598
			public virtual void Insert(int index, object value)
			{
				lock (this._root)
				{
					this._list.Insert(index, value);
				}
			}

			// Token: 0x0600167F RID: 5759 RVA: 0x0003A5D8 File Offset: 0x000395D8
			public virtual void Remove(object value)
			{
				lock (this._root)
				{
					this._list.Remove(value);
				}
			}

			// Token: 0x06001680 RID: 5760 RVA: 0x0003A618 File Offset: 0x00039618
			public virtual void RemoveAt(int index)
			{
				lock (this._root)
				{
					this._list.RemoveAt(index);
				}
			}

			// Token: 0x04000914 RID: 2324
			private IList _list;

			// Token: 0x04000915 RID: 2325
			private object _root;
		}

		// Token: 0x0200023B RID: 571
		[Serializable]
		private class FixedSizeList : IList, ICollection, IEnumerable
		{
			// Token: 0x06001681 RID: 5761 RVA: 0x0003A658 File Offset: 0x00039658
			internal FixedSizeList(IList l)
			{
				this._list = l;
			}

			// Token: 0x1700030D RID: 781
			// (get) Token: 0x06001682 RID: 5762 RVA: 0x0003A667 File Offset: 0x00039667
			public virtual int Count
			{
				get
				{
					return this._list.Count;
				}
			}

			// Token: 0x1700030E RID: 782
			// (get) Token: 0x06001683 RID: 5763 RVA: 0x0003A674 File Offset: 0x00039674
			public virtual bool IsReadOnly
			{
				get
				{
					return this._list.IsReadOnly;
				}
			}

			// Token: 0x1700030F RID: 783
			// (get) Token: 0x06001684 RID: 5764 RVA: 0x0003A681 File Offset: 0x00039681
			public virtual bool IsFixedSize
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000310 RID: 784
			// (get) Token: 0x06001685 RID: 5765 RVA: 0x0003A684 File Offset: 0x00039684
			public virtual bool IsSynchronized
			{
				get
				{
					return this._list.IsSynchronized;
				}
			}

			// Token: 0x17000311 RID: 785
			public virtual object this[int index]
			{
				get
				{
					return this._list[index];
				}
				set
				{
					this._list[index] = value;
				}
			}

			// Token: 0x17000312 RID: 786
			// (get) Token: 0x06001688 RID: 5768 RVA: 0x0003A6AE File Offset: 0x000396AE
			public virtual object SyncRoot
			{
				get
				{
					return this._list.SyncRoot;
				}
			}

			// Token: 0x06001689 RID: 5769 RVA: 0x0003A6BB File Offset: 0x000396BB
			public virtual int Add(object obj)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x0600168A RID: 5770 RVA: 0x0003A6CC File Offset: 0x000396CC
			public virtual void Clear()
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x0600168B RID: 5771 RVA: 0x0003A6DD File Offset: 0x000396DD
			public virtual bool Contains(object obj)
			{
				return this._list.Contains(obj);
			}

			// Token: 0x0600168C RID: 5772 RVA: 0x0003A6EB File Offset: 0x000396EB
			public virtual void CopyTo(Array array, int index)
			{
				this._list.CopyTo(array, index);
			}

			// Token: 0x0600168D RID: 5773 RVA: 0x0003A6FA File Offset: 0x000396FA
			public virtual IEnumerator GetEnumerator()
			{
				return this._list.GetEnumerator();
			}

			// Token: 0x0600168E RID: 5774 RVA: 0x0003A707 File Offset: 0x00039707
			public virtual int IndexOf(object value)
			{
				return this._list.IndexOf(value);
			}

			// Token: 0x0600168F RID: 5775 RVA: 0x0003A715 File Offset: 0x00039715
			public virtual void Insert(int index, object obj)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x06001690 RID: 5776 RVA: 0x0003A726 File Offset: 0x00039726
			public virtual void Remove(object value)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x06001691 RID: 5777 RVA: 0x0003A737 File Offset: 0x00039737
			public virtual void RemoveAt(int index)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x04000916 RID: 2326
			private IList _list;
		}

		// Token: 0x0200023C RID: 572
		[Serializable]
		private class FixedSizeArrayList : ArrayList
		{
			// Token: 0x06001692 RID: 5778 RVA: 0x0003A748 File Offset: 0x00039748
			internal FixedSizeArrayList(ArrayList l)
			{
				this._list = l;
				this._version = this._list._version;
			}

			// Token: 0x17000313 RID: 787
			// (get) Token: 0x06001693 RID: 5779 RVA: 0x0003A768 File Offset: 0x00039768
			public override int Count
			{
				get
				{
					return this._list.Count;
				}
			}

			// Token: 0x17000314 RID: 788
			// (get) Token: 0x06001694 RID: 5780 RVA: 0x0003A775 File Offset: 0x00039775
			public override bool IsReadOnly
			{
				get
				{
					return this._list.IsReadOnly;
				}
			}

			// Token: 0x17000315 RID: 789
			// (get) Token: 0x06001695 RID: 5781 RVA: 0x0003A782 File Offset: 0x00039782
			public override bool IsFixedSize
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000316 RID: 790
			// (get) Token: 0x06001696 RID: 5782 RVA: 0x0003A785 File Offset: 0x00039785
			public override bool IsSynchronized
			{
				get
				{
					return this._list.IsSynchronized;
				}
			}

			// Token: 0x17000317 RID: 791
			public override object this[int index]
			{
				get
				{
					return this._list[index];
				}
				set
				{
					this._list[index] = value;
					this._version = this._list._version;
				}
			}

			// Token: 0x17000318 RID: 792
			// (get) Token: 0x06001699 RID: 5785 RVA: 0x0003A7C0 File Offset: 0x000397C0
			public override object SyncRoot
			{
				get
				{
					return this._list.SyncRoot;
				}
			}

			// Token: 0x0600169A RID: 5786 RVA: 0x0003A7CD File Offset: 0x000397CD
			public override int Add(object obj)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x0600169B RID: 5787 RVA: 0x0003A7DE File Offset: 0x000397DE
			public override void AddRange(ICollection c)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x0600169C RID: 5788 RVA: 0x0003A7EF File Offset: 0x000397EF
			public override int BinarySearch(int index, int count, object value, IComparer comparer)
			{
				return this._list.BinarySearch(index, count, value, comparer);
			}

			// Token: 0x17000319 RID: 793
			// (get) Token: 0x0600169D RID: 5789 RVA: 0x0003A801 File Offset: 0x00039801
			// (set) Token: 0x0600169E RID: 5790 RVA: 0x0003A80E File Offset: 0x0003980E
			public override int Capacity
			{
				get
				{
					return this._list.Capacity;
				}
				set
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
				}
			}

			// Token: 0x0600169F RID: 5791 RVA: 0x0003A81F File Offset: 0x0003981F
			public override void Clear()
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x060016A0 RID: 5792 RVA: 0x0003A830 File Offset: 0x00039830
			public override object Clone()
			{
				return new ArrayList.FixedSizeArrayList(this._list)
				{
					_list = (ArrayList)this._list.Clone()
				};
			}

			// Token: 0x060016A1 RID: 5793 RVA: 0x0003A860 File Offset: 0x00039860
			public override bool Contains(object obj)
			{
				return this._list.Contains(obj);
			}

			// Token: 0x060016A2 RID: 5794 RVA: 0x0003A86E File Offset: 0x0003986E
			public override void CopyTo(Array array, int index)
			{
				this._list.CopyTo(array, index);
			}

			// Token: 0x060016A3 RID: 5795 RVA: 0x0003A87D File Offset: 0x0003987D
			public override void CopyTo(int index, Array array, int arrayIndex, int count)
			{
				this._list.CopyTo(index, array, arrayIndex, count);
			}

			// Token: 0x060016A4 RID: 5796 RVA: 0x0003A88F File Offset: 0x0003988F
			public override IEnumerator GetEnumerator()
			{
				return this._list.GetEnumerator();
			}

			// Token: 0x060016A5 RID: 5797 RVA: 0x0003A89C File Offset: 0x0003989C
			public override IEnumerator GetEnumerator(int index, int count)
			{
				return this._list.GetEnumerator(index, count);
			}

			// Token: 0x060016A6 RID: 5798 RVA: 0x0003A8AB File Offset: 0x000398AB
			public override int IndexOf(object value)
			{
				return this._list.IndexOf(value);
			}

			// Token: 0x060016A7 RID: 5799 RVA: 0x0003A8B9 File Offset: 0x000398B9
			public override int IndexOf(object value, int startIndex)
			{
				return this._list.IndexOf(value, startIndex);
			}

			// Token: 0x060016A8 RID: 5800 RVA: 0x0003A8C8 File Offset: 0x000398C8
			public override int IndexOf(object value, int startIndex, int count)
			{
				return this._list.IndexOf(value, startIndex, count);
			}

			// Token: 0x060016A9 RID: 5801 RVA: 0x0003A8D8 File Offset: 0x000398D8
			public override void Insert(int index, object obj)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x060016AA RID: 5802 RVA: 0x0003A8E9 File Offset: 0x000398E9
			public override void InsertRange(int index, ICollection c)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x060016AB RID: 5803 RVA: 0x0003A8FA File Offset: 0x000398FA
			public override int LastIndexOf(object value)
			{
				return this._list.LastIndexOf(value);
			}

			// Token: 0x060016AC RID: 5804 RVA: 0x0003A908 File Offset: 0x00039908
			public override int LastIndexOf(object value, int startIndex)
			{
				return this._list.LastIndexOf(value, startIndex);
			}

			// Token: 0x060016AD RID: 5805 RVA: 0x0003A917 File Offset: 0x00039917
			public override int LastIndexOf(object value, int startIndex, int count)
			{
				return this._list.LastIndexOf(value, startIndex, count);
			}

			// Token: 0x060016AE RID: 5806 RVA: 0x0003A927 File Offset: 0x00039927
			public override void Remove(object value)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x060016AF RID: 5807 RVA: 0x0003A938 File Offset: 0x00039938
			public override void RemoveAt(int index)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x060016B0 RID: 5808 RVA: 0x0003A949 File Offset: 0x00039949
			public override void RemoveRange(int index, int count)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x060016B1 RID: 5809 RVA: 0x0003A95A File Offset: 0x0003995A
			public override void SetRange(int index, ICollection c)
			{
				this._list.SetRange(index, c);
				this._version = this._list._version;
			}

			// Token: 0x060016B2 RID: 5810 RVA: 0x0003A97C File Offset: 0x0003997C
			public override ArrayList GetRange(int index, int count)
			{
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this.Count - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				return new ArrayList.Range(this, index, count);
			}

			// Token: 0x060016B3 RID: 5811 RVA: 0x0003A9D4 File Offset: 0x000399D4
			public override void Reverse(int index, int count)
			{
				this._list.Reverse(index, count);
				this._version = this._list._version;
			}

			// Token: 0x060016B4 RID: 5812 RVA: 0x0003A9F4 File Offset: 0x000399F4
			public override void Sort(int index, int count, IComparer comparer)
			{
				this._list.Sort(index, count, comparer);
				this._version = this._list._version;
			}

			// Token: 0x060016B5 RID: 5813 RVA: 0x0003AA15 File Offset: 0x00039A15
			public override object[] ToArray()
			{
				return this._list.ToArray();
			}

			// Token: 0x060016B6 RID: 5814 RVA: 0x0003AA22 File Offset: 0x00039A22
			public override Array ToArray(Type type)
			{
				return this._list.ToArray(type);
			}

			// Token: 0x060016B7 RID: 5815 RVA: 0x0003AA30 File Offset: 0x00039A30
			public override void TrimToSize()
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
			}

			// Token: 0x04000917 RID: 2327
			private ArrayList _list;
		}

		// Token: 0x0200023D RID: 573
		[Serializable]
		private class ReadOnlyList : IList, ICollection, IEnumerable
		{
			// Token: 0x060016B8 RID: 5816 RVA: 0x0003AA41 File Offset: 0x00039A41
			internal ReadOnlyList(IList l)
			{
				this._list = l;
			}

			// Token: 0x1700031A RID: 794
			// (get) Token: 0x060016B9 RID: 5817 RVA: 0x0003AA50 File Offset: 0x00039A50
			public virtual int Count
			{
				get
				{
					return this._list.Count;
				}
			}

			// Token: 0x1700031B RID: 795
			// (get) Token: 0x060016BA RID: 5818 RVA: 0x0003AA5D File Offset: 0x00039A5D
			public virtual bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700031C RID: 796
			// (get) Token: 0x060016BB RID: 5819 RVA: 0x0003AA60 File Offset: 0x00039A60
			public virtual bool IsFixedSize
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700031D RID: 797
			// (get) Token: 0x060016BC RID: 5820 RVA: 0x0003AA63 File Offset: 0x00039A63
			public virtual bool IsSynchronized
			{
				get
				{
					return this._list.IsSynchronized;
				}
			}

			// Token: 0x1700031E RID: 798
			public virtual object this[int index]
			{
				get
				{
					return this._list[index];
				}
				set
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
				}
			}

			// Token: 0x1700031F RID: 799
			// (get) Token: 0x060016BF RID: 5823 RVA: 0x0003AA8F File Offset: 0x00039A8F
			public virtual object SyncRoot
			{
				get
				{
					return this._list.SyncRoot;
				}
			}

			// Token: 0x060016C0 RID: 5824 RVA: 0x0003AA9C File Offset: 0x00039A9C
			public virtual int Add(object obj)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x060016C1 RID: 5825 RVA: 0x0003AAAD File Offset: 0x00039AAD
			public virtual void Clear()
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x060016C2 RID: 5826 RVA: 0x0003AABE File Offset: 0x00039ABE
			public virtual bool Contains(object obj)
			{
				return this._list.Contains(obj);
			}

			// Token: 0x060016C3 RID: 5827 RVA: 0x0003AACC File Offset: 0x00039ACC
			public virtual void CopyTo(Array array, int index)
			{
				this._list.CopyTo(array, index);
			}

			// Token: 0x060016C4 RID: 5828 RVA: 0x0003AADB File Offset: 0x00039ADB
			public virtual IEnumerator GetEnumerator()
			{
				return this._list.GetEnumerator();
			}

			// Token: 0x060016C5 RID: 5829 RVA: 0x0003AAE8 File Offset: 0x00039AE8
			public virtual int IndexOf(object value)
			{
				return this._list.IndexOf(value);
			}

			// Token: 0x060016C6 RID: 5830 RVA: 0x0003AAF6 File Offset: 0x00039AF6
			public virtual void Insert(int index, object obj)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x060016C7 RID: 5831 RVA: 0x0003AB07 File Offset: 0x00039B07
			public virtual void Remove(object value)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x060016C8 RID: 5832 RVA: 0x0003AB18 File Offset: 0x00039B18
			public virtual void RemoveAt(int index)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x04000918 RID: 2328
			private IList _list;
		}

		// Token: 0x0200023E RID: 574
		[Serializable]
		private class ReadOnlyArrayList : ArrayList
		{
			// Token: 0x060016C9 RID: 5833 RVA: 0x0003AB29 File Offset: 0x00039B29
			internal ReadOnlyArrayList(ArrayList l)
			{
				this._list = l;
			}

			// Token: 0x17000320 RID: 800
			// (get) Token: 0x060016CA RID: 5834 RVA: 0x0003AB38 File Offset: 0x00039B38
			public override int Count
			{
				get
				{
					return this._list.Count;
				}
			}

			// Token: 0x17000321 RID: 801
			// (get) Token: 0x060016CB RID: 5835 RVA: 0x0003AB45 File Offset: 0x00039B45
			public override bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000322 RID: 802
			// (get) Token: 0x060016CC RID: 5836 RVA: 0x0003AB48 File Offset: 0x00039B48
			public override bool IsFixedSize
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000323 RID: 803
			// (get) Token: 0x060016CD RID: 5837 RVA: 0x0003AB4B File Offset: 0x00039B4B
			public override bool IsSynchronized
			{
				get
				{
					return this._list.IsSynchronized;
				}
			}

			// Token: 0x17000324 RID: 804
			public override object this[int index]
			{
				get
				{
					return this._list[index];
				}
				set
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
				}
			}

			// Token: 0x17000325 RID: 805
			// (get) Token: 0x060016D0 RID: 5840 RVA: 0x0003AB77 File Offset: 0x00039B77
			public override object SyncRoot
			{
				get
				{
					return this._list.SyncRoot;
				}
			}

			// Token: 0x060016D1 RID: 5841 RVA: 0x0003AB84 File Offset: 0x00039B84
			public override int Add(object obj)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x060016D2 RID: 5842 RVA: 0x0003AB95 File Offset: 0x00039B95
			public override void AddRange(ICollection c)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x060016D3 RID: 5843 RVA: 0x0003ABA6 File Offset: 0x00039BA6
			public override int BinarySearch(int index, int count, object value, IComparer comparer)
			{
				return this._list.BinarySearch(index, count, value, comparer);
			}

			// Token: 0x17000326 RID: 806
			// (get) Token: 0x060016D4 RID: 5844 RVA: 0x0003ABB8 File Offset: 0x00039BB8
			// (set) Token: 0x060016D5 RID: 5845 RVA: 0x0003ABC5 File Offset: 0x00039BC5
			public override int Capacity
			{
				get
				{
					return this._list.Capacity;
				}
				set
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
				}
			}

			// Token: 0x060016D6 RID: 5846 RVA: 0x0003ABD6 File Offset: 0x00039BD6
			public override void Clear()
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x060016D7 RID: 5847 RVA: 0x0003ABE8 File Offset: 0x00039BE8
			public override object Clone()
			{
				return new ArrayList.ReadOnlyArrayList(this._list)
				{
					_list = (ArrayList)this._list.Clone()
				};
			}

			// Token: 0x060016D8 RID: 5848 RVA: 0x0003AC18 File Offset: 0x00039C18
			public override bool Contains(object obj)
			{
				return this._list.Contains(obj);
			}

			// Token: 0x060016D9 RID: 5849 RVA: 0x0003AC26 File Offset: 0x00039C26
			public override void CopyTo(Array array, int index)
			{
				this._list.CopyTo(array, index);
			}

			// Token: 0x060016DA RID: 5850 RVA: 0x0003AC35 File Offset: 0x00039C35
			public override void CopyTo(int index, Array array, int arrayIndex, int count)
			{
				this._list.CopyTo(index, array, arrayIndex, count);
			}

			// Token: 0x060016DB RID: 5851 RVA: 0x0003AC47 File Offset: 0x00039C47
			public override IEnumerator GetEnumerator()
			{
				return this._list.GetEnumerator();
			}

			// Token: 0x060016DC RID: 5852 RVA: 0x0003AC54 File Offset: 0x00039C54
			public override IEnumerator GetEnumerator(int index, int count)
			{
				return this._list.GetEnumerator(index, count);
			}

			// Token: 0x060016DD RID: 5853 RVA: 0x0003AC63 File Offset: 0x00039C63
			public override int IndexOf(object value)
			{
				return this._list.IndexOf(value);
			}

			// Token: 0x060016DE RID: 5854 RVA: 0x0003AC71 File Offset: 0x00039C71
			public override int IndexOf(object value, int startIndex)
			{
				return this._list.IndexOf(value, startIndex);
			}

			// Token: 0x060016DF RID: 5855 RVA: 0x0003AC80 File Offset: 0x00039C80
			public override int IndexOf(object value, int startIndex, int count)
			{
				return this._list.IndexOf(value, startIndex, count);
			}

			// Token: 0x060016E0 RID: 5856 RVA: 0x0003AC90 File Offset: 0x00039C90
			public override void Insert(int index, object obj)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x060016E1 RID: 5857 RVA: 0x0003ACA1 File Offset: 0x00039CA1
			public override void InsertRange(int index, ICollection c)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x060016E2 RID: 5858 RVA: 0x0003ACB2 File Offset: 0x00039CB2
			public override int LastIndexOf(object value)
			{
				return this._list.LastIndexOf(value);
			}

			// Token: 0x060016E3 RID: 5859 RVA: 0x0003ACC0 File Offset: 0x00039CC0
			public override int LastIndexOf(object value, int startIndex)
			{
				return this._list.LastIndexOf(value, startIndex);
			}

			// Token: 0x060016E4 RID: 5860 RVA: 0x0003ACCF File Offset: 0x00039CCF
			public override int LastIndexOf(object value, int startIndex, int count)
			{
				return this._list.LastIndexOf(value, startIndex, count);
			}

			// Token: 0x060016E5 RID: 5861 RVA: 0x0003ACDF File Offset: 0x00039CDF
			public override void Remove(object value)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x060016E6 RID: 5862 RVA: 0x0003ACF0 File Offset: 0x00039CF0
			public override void RemoveAt(int index)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x060016E7 RID: 5863 RVA: 0x0003AD01 File Offset: 0x00039D01
			public override void RemoveRange(int index, int count)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x060016E8 RID: 5864 RVA: 0x0003AD12 File Offset: 0x00039D12
			public override void SetRange(int index, ICollection c)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x060016E9 RID: 5865 RVA: 0x0003AD24 File Offset: 0x00039D24
			public override ArrayList GetRange(int index, int count)
			{
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this.Count - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				return new ArrayList.Range(this, index, count);
			}

			// Token: 0x060016EA RID: 5866 RVA: 0x0003AD7C File Offset: 0x00039D7C
			public override void Reverse(int index, int count)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x060016EB RID: 5867 RVA: 0x0003AD8D File Offset: 0x00039D8D
			public override void Sort(int index, int count, IComparer comparer)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x060016EC RID: 5868 RVA: 0x0003AD9E File Offset: 0x00039D9E
			public override object[] ToArray()
			{
				return this._list.ToArray();
			}

			// Token: 0x060016ED RID: 5869 RVA: 0x0003ADAB File Offset: 0x00039DAB
			public override Array ToArray(Type type)
			{
				return this._list.ToArray(type);
			}

			// Token: 0x060016EE RID: 5870 RVA: 0x0003ADB9 File Offset: 0x00039DB9
			public override void TrimToSize()
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
			}

			// Token: 0x04000919 RID: 2329
			private ArrayList _list;
		}

		// Token: 0x0200023F RID: 575
		[Serializable]
		private sealed class ArrayListEnumerator : IEnumerator, ICloneable
		{
			// Token: 0x060016EF RID: 5871 RVA: 0x0003ADCA File Offset: 0x00039DCA
			internal ArrayListEnumerator(ArrayList list, int index, int count)
			{
				this.list = list;
				this.startIndex = index;
				this.index = index - 1;
				this.endIndex = this.index + count;
				this.version = list._version;
				this.currentElement = null;
			}

			// Token: 0x060016F0 RID: 5872 RVA: 0x0003AE0A File Offset: 0x00039E0A
			public object Clone()
			{
				return base.MemberwiseClone();
			}

			// Token: 0x060016F1 RID: 5873 RVA: 0x0003AE14 File Offset: 0x00039E14
			public bool MoveNext()
			{
				if (this.version != this.list._version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				if (this.index < this.endIndex)
				{
					this.currentElement = this.list[++this.index];
					return true;
				}
				this.index = this.endIndex + 1;
				return false;
			}

			// Token: 0x17000327 RID: 807
			// (get) Token: 0x060016F2 RID: 5874 RVA: 0x0003AE88 File Offset: 0x00039E88
			public object Current
			{
				get
				{
					if (this.index < this.startIndex)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
					}
					if (this.index > this.endIndex)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
					}
					return this.currentElement;
				}
			}

			// Token: 0x060016F3 RID: 5875 RVA: 0x0003AED7 File Offset: 0x00039ED7
			public void Reset()
			{
				if (this.version != this.list._version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				this.index = this.startIndex - 1;
			}

			// Token: 0x0400091A RID: 2330
			private ArrayList list;

			// Token: 0x0400091B RID: 2331
			private int index;

			// Token: 0x0400091C RID: 2332
			private int endIndex;

			// Token: 0x0400091D RID: 2333
			private int version;

			// Token: 0x0400091E RID: 2334
			private object currentElement;

			// Token: 0x0400091F RID: 2335
			private int startIndex;
		}

		// Token: 0x02000240 RID: 576
		[Serializable]
		private class Range : ArrayList
		{
			// Token: 0x060016F4 RID: 5876 RVA: 0x0003AF0A File Offset: 0x00039F0A
			internal Range(ArrayList list, int index, int count)
				: base(false)
			{
				this._baseList = list;
				this._baseIndex = index;
				this._baseSize = count;
				this._baseVersion = list._version;
				this._version = list._version;
			}

			// Token: 0x060016F5 RID: 5877 RVA: 0x0003AF40 File Offset: 0x00039F40
			private void InternalUpdateRange()
			{
				if (this._baseVersion != this._baseList._version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_UnderlyingArrayListChanged"));
				}
			}

			// Token: 0x060016F6 RID: 5878 RVA: 0x0003AF65 File Offset: 0x00039F65
			private void InternalUpdateVersion()
			{
				this._baseVersion++;
				this._version++;
			}

			// Token: 0x060016F7 RID: 5879 RVA: 0x0003AF84 File Offset: 0x00039F84
			public override int Add(object value)
			{
				this.InternalUpdateRange();
				this._baseList.Insert(this._baseIndex + this._baseSize, value);
				this.InternalUpdateVersion();
				return this._baseSize++;
			}

			// Token: 0x060016F8 RID: 5880 RVA: 0x0003AFC8 File Offset: 0x00039FC8
			public override void AddRange(ICollection c)
			{
				this.InternalUpdateRange();
				if (c == null)
				{
					throw new ArgumentNullException("c");
				}
				int count = c.Count;
				if (count > 0)
				{
					this._baseList.InsertRange(this._baseIndex + this._baseSize, c);
					this.InternalUpdateVersion();
					this._baseSize += count;
				}
			}

			// Token: 0x060016F9 RID: 5881 RVA: 0x0003B024 File Offset: 0x0003A024
			public override int BinarySearch(int index, int count, object value, IComparer comparer)
			{
				this.InternalUpdateRange();
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._baseSize - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				int num = this._baseList.BinarySearch(this._baseIndex + index, count, value, comparer);
				if (num >= 0)
				{
					return num - this._baseIndex;
				}
				return num + this._baseIndex;
			}

			// Token: 0x17000328 RID: 808
			// (get) Token: 0x060016FA RID: 5882 RVA: 0x0003B0A7 File Offset: 0x0003A0A7
			// (set) Token: 0x060016FB RID: 5883 RVA: 0x0003B0B4 File Offset: 0x0003A0B4
			public override int Capacity
			{
				get
				{
					return this._baseList.Capacity;
				}
				set
				{
					if (value < this.Count)
					{
						throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_SmallCapacity"));
					}
				}
			}

			// Token: 0x060016FC RID: 5884 RVA: 0x0003B0D4 File Offset: 0x0003A0D4
			public override void Clear()
			{
				this.InternalUpdateRange();
				if (this._baseSize != 0)
				{
					this._baseList.RemoveRange(this._baseIndex, this._baseSize);
					this.InternalUpdateVersion();
					this._baseSize = 0;
				}
			}

			// Token: 0x060016FD RID: 5885 RVA: 0x0003B108 File Offset: 0x0003A108
			public override object Clone()
			{
				this.InternalUpdateRange();
				return new ArrayList.Range(this._baseList, this._baseIndex, this._baseSize)
				{
					_baseList = (ArrayList)this._baseList.Clone()
				};
			}

			// Token: 0x060016FE RID: 5886 RVA: 0x0003B14C File Offset: 0x0003A14C
			public override bool Contains(object item)
			{
				this.InternalUpdateRange();
				if (item == null)
				{
					for (int i = 0; i < this._baseSize; i++)
					{
						if (this._baseList[this._baseIndex + i] == null)
						{
							return true;
						}
					}
					return false;
				}
				for (int j = 0; j < this._baseSize; j++)
				{
					if (this._baseList[this._baseIndex + j] != null && this._baseList[this._baseIndex + j].Equals(item))
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x060016FF RID: 5887 RVA: 0x0003B1D0 File Offset: 0x0003A1D0
			public override void CopyTo(Array array, int index)
			{
				this.InternalUpdateRange();
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
				if (array.Length - index < this._baseSize)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				this._baseList.CopyTo(this._baseIndex, array, index, this._baseSize);
			}

			// Token: 0x06001700 RID: 5888 RVA: 0x0003B25C File Offset: 0x0003A25C
			public override void CopyTo(int index, Array array, int arrayIndex, int count)
			{
				this.InternalUpdateRange();
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (array.Rank != 1)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_RankMultiDimNotSupported"));
				}
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (array.Length - arrayIndex < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				if (this._baseSize - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				this._baseList.CopyTo(this._baseIndex + index, array, arrayIndex, count);
			}

			// Token: 0x17000329 RID: 809
			// (get) Token: 0x06001701 RID: 5889 RVA: 0x0003B30E File Offset: 0x0003A30E
			public override int Count
			{
				get
				{
					this.InternalUpdateRange();
					return this._baseSize;
				}
			}

			// Token: 0x1700032A RID: 810
			// (get) Token: 0x06001702 RID: 5890 RVA: 0x0003B31C File Offset: 0x0003A31C
			public override bool IsReadOnly
			{
				get
				{
					return this._baseList.IsReadOnly;
				}
			}

			// Token: 0x1700032B RID: 811
			// (get) Token: 0x06001703 RID: 5891 RVA: 0x0003B329 File Offset: 0x0003A329
			public override bool IsFixedSize
			{
				get
				{
					return this._baseList.IsFixedSize;
				}
			}

			// Token: 0x1700032C RID: 812
			// (get) Token: 0x06001704 RID: 5892 RVA: 0x0003B336 File Offset: 0x0003A336
			public override bool IsSynchronized
			{
				get
				{
					return this._baseList.IsSynchronized;
				}
			}

			// Token: 0x06001705 RID: 5893 RVA: 0x0003B343 File Offset: 0x0003A343
			public override IEnumerator GetEnumerator()
			{
				return this.GetEnumerator(0, this._baseSize);
			}

			// Token: 0x06001706 RID: 5894 RVA: 0x0003B354 File Offset: 0x0003A354
			public override IEnumerator GetEnumerator(int index, int count)
			{
				this.InternalUpdateRange();
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._baseSize - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				return this._baseList.GetEnumerator(this._baseIndex + index, count);
			}

			// Token: 0x06001707 RID: 5895 RVA: 0x0003B3C0 File Offset: 0x0003A3C0
			public override ArrayList GetRange(int index, int count)
			{
				this.InternalUpdateRange();
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._baseSize - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				return new ArrayList.Range(this, index, count);
			}

			// Token: 0x1700032D RID: 813
			// (get) Token: 0x06001708 RID: 5896 RVA: 0x0003B41E File Offset: 0x0003A41E
			public override object SyncRoot
			{
				get
				{
					return this._baseList.SyncRoot;
				}
			}

			// Token: 0x06001709 RID: 5897 RVA: 0x0003B42C File Offset: 0x0003A42C
			public override int IndexOf(object value)
			{
				this.InternalUpdateRange();
				int num = this._baseList.IndexOf(value, this._baseIndex, this._baseSize);
				if (num >= 0)
				{
					return num - this._baseIndex;
				}
				return -1;
			}

			// Token: 0x0600170A RID: 5898 RVA: 0x0003B468 File Offset: 0x0003A468
			public override int IndexOf(object value, int startIndex)
			{
				this.InternalUpdateRange();
				if (startIndex < 0)
				{
					throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (startIndex > this._baseSize)
				{
					throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				int num = this._baseList.IndexOf(value, this._baseIndex + startIndex, this._baseSize - startIndex);
				if (num >= 0)
				{
					return num - this._baseIndex;
				}
				return -1;
			}

			// Token: 0x0600170B RID: 5899 RVA: 0x0003B4E0 File Offset: 0x0003A4E0
			public override int IndexOf(object value, int startIndex, int count)
			{
				this.InternalUpdateRange();
				if (startIndex < 0 || startIndex > this._baseSize)
				{
					throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				if (count < 0 || startIndex > this._baseSize - count)
				{
					throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_Count"));
				}
				int num = this._baseList.IndexOf(value, this._baseIndex + startIndex, count);
				if (num >= 0)
				{
					return num - this._baseIndex;
				}
				return -1;
			}

			// Token: 0x0600170C RID: 5900 RVA: 0x0003B560 File Offset: 0x0003A560
			public override void Insert(int index, object value)
			{
				this.InternalUpdateRange();
				if (index < 0 || index > this._baseSize)
				{
					throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				this._baseList.Insert(this._baseIndex + index, value);
				this.InternalUpdateVersion();
				this._baseSize++;
			}

			// Token: 0x0600170D RID: 5901 RVA: 0x0003B5C0 File Offset: 0x0003A5C0
			public override void InsertRange(int index, ICollection c)
			{
				this.InternalUpdateRange();
				if (index < 0 || index > this._baseSize)
				{
					throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				if (c == null)
				{
					throw new ArgumentNullException("c");
				}
				int count = c.Count;
				if (count > 0)
				{
					this._baseList.InsertRange(this._baseIndex + index, c);
					this._baseSize += count;
					this.InternalUpdateVersion();
				}
			}

			// Token: 0x0600170E RID: 5902 RVA: 0x0003B638 File Offset: 0x0003A638
			public override int LastIndexOf(object value)
			{
				this.InternalUpdateRange();
				int num = this._baseList.LastIndexOf(value, this._baseIndex + this._baseSize - 1, this._baseSize);
				if (num >= 0)
				{
					return num - this._baseIndex;
				}
				return -1;
			}

			// Token: 0x0600170F RID: 5903 RVA: 0x0003B67B File Offset: 0x0003A67B
			public override int LastIndexOf(object value, int startIndex)
			{
				return this.LastIndexOf(value, startIndex, startIndex + 1);
			}

			// Token: 0x06001710 RID: 5904 RVA: 0x0003B688 File Offset: 0x0003A688
			public override int LastIndexOf(object value, int startIndex, int count)
			{
				this.InternalUpdateRange();
				if (this._baseSize == 0)
				{
					return -1;
				}
				if (startIndex >= this._baseSize)
				{
					throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				if (startIndex < 0)
				{
					throw new ArgumentOutOfRangeException("startIndex", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				int num = this._baseList.LastIndexOf(value, this._baseIndex + startIndex, count);
				if (num >= 0)
				{
					return num - this._baseIndex;
				}
				return -1;
			}

			// Token: 0x06001711 RID: 5905 RVA: 0x0003B700 File Offset: 0x0003A700
			public override void RemoveAt(int index)
			{
				this.InternalUpdateRange();
				if (index < 0 || index >= this._baseSize)
				{
					throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				this._baseList.RemoveAt(this._baseIndex + index);
				this.InternalUpdateVersion();
				this._baseSize--;
			}

			// Token: 0x06001712 RID: 5906 RVA: 0x0003B75C File Offset: 0x0003A75C
			public override void RemoveRange(int index, int count)
			{
				this.InternalUpdateRange();
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._baseSize - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				if (count > 0)
				{
					this._baseList.RemoveRange(this._baseIndex + index, count);
					this.InternalUpdateVersion();
					this._baseSize -= count;
				}
			}

			// Token: 0x06001713 RID: 5907 RVA: 0x0003B7E0 File Offset: 0x0003A7E0
			public override void Reverse(int index, int count)
			{
				this.InternalUpdateRange();
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._baseSize - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				this._baseList.Reverse(this._baseIndex + index, count);
				this.InternalUpdateVersion();
			}

			// Token: 0x06001714 RID: 5908 RVA: 0x0003B850 File Offset: 0x0003A850
			public override void SetRange(int index, ICollection c)
			{
				this.InternalUpdateRange();
				if (index < 0 || index >= this._baseSize)
				{
					throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				this._baseList.SetRange(this._baseIndex + index, c);
				if (c.Count > 0)
				{
					this.InternalUpdateVersion();
				}
			}

			// Token: 0x06001715 RID: 5909 RVA: 0x0003B8A8 File Offset: 0x0003A8A8
			public override void Sort(int index, int count, IComparer comparer)
			{
				this.InternalUpdateRange();
				if (index < 0 || count < 0)
				{
					throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._baseSize - index < count)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
				}
				this._baseList.Sort(this._baseIndex + index, count, comparer);
				this.InternalUpdateVersion();
			}

			// Token: 0x1700032E RID: 814
			public override object this[int index]
			{
				get
				{
					this.InternalUpdateRange();
					if (index < 0 || index >= this._baseSize)
					{
						throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
					}
					return this._baseList[this._baseIndex + index];
				}
				set
				{
					this.InternalUpdateRange();
					if (index < 0 || index >= this._baseSize)
					{
						throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
					}
					this._baseList[this._baseIndex + index] = value;
					this.InternalUpdateVersion();
				}
			}

			// Token: 0x06001718 RID: 5912 RVA: 0x0003B9A8 File Offset: 0x0003A9A8
			public override object[] ToArray()
			{
				this.InternalUpdateRange();
				object[] array = new object[this._baseSize];
				Array.Copy(this._baseList._items, this._baseIndex, array, 0, this._baseSize);
				return array;
			}

			// Token: 0x06001719 RID: 5913 RVA: 0x0003B9E8 File Offset: 0x0003A9E8
			public override Array ToArray(Type type)
			{
				this.InternalUpdateRange();
				if (type == null)
				{
					throw new ArgumentNullException("type");
				}
				Array array = Array.CreateInstance(type, this._baseSize);
				this._baseList.CopyTo(this._baseIndex, array, 0, this._baseSize);
				return array;
			}

			// Token: 0x0600171A RID: 5914 RVA: 0x0003BA30 File Offset: 0x0003AA30
			public override void TrimToSize()
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_RangeCollection"));
			}

			// Token: 0x04000920 RID: 2336
			private ArrayList _baseList;

			// Token: 0x04000921 RID: 2337
			private int _baseIndex;

			// Token: 0x04000922 RID: 2338
			private int _baseSize;

			// Token: 0x04000923 RID: 2339
			private int _baseVersion;
		}

		// Token: 0x02000241 RID: 577
		[Serializable]
		private sealed class ArrayListEnumeratorSimple : IEnumerator, ICloneable
		{
			// Token: 0x0600171B RID: 5915 RVA: 0x0003BA44 File Offset: 0x0003AA44
			internal ArrayListEnumeratorSimple(ArrayList list)
			{
				this.list = list;
				this.index = -1;
				this.version = list._version;
				this.isArrayList = list.GetType() == typeof(ArrayList);
				this.currentElement = ArrayList.ArrayListEnumeratorSimple.dummyObject;
			}

			// Token: 0x0600171C RID: 5916 RVA: 0x0003BA94 File Offset: 0x0003AA94
			public object Clone()
			{
				return base.MemberwiseClone();
			}

			// Token: 0x0600171D RID: 5917 RVA: 0x0003BA9C File Offset: 0x0003AA9C
			public bool MoveNext()
			{
				if (this.version != this.list._version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				if (this.isArrayList)
				{
					if (this.index < this.list._size - 1)
					{
						this.currentElement = this.list._items[++this.index];
						return true;
					}
					this.currentElement = ArrayList.ArrayListEnumeratorSimple.dummyObject;
					this.index = this.list._size;
					return false;
				}
				else
				{
					if (this.index < this.list.Count - 1)
					{
						this.currentElement = this.list[++this.index];
						return true;
					}
					this.index = this.list.Count;
					this.currentElement = ArrayList.ArrayListEnumeratorSimple.dummyObject;
					return false;
				}
			}

			// Token: 0x1700032F RID: 815
			// (get) Token: 0x0600171E RID: 5918 RVA: 0x0003BB84 File Offset: 0x0003AB84
			public object Current
			{
				get
				{
					object obj = this.currentElement;
					if (ArrayList.ArrayListEnumeratorSimple.dummyObject != obj)
					{
						return obj;
					}
					if (this.index == -1)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
					}
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
				}
			}

			// Token: 0x0600171F RID: 5919 RVA: 0x0003BBCA File Offset: 0x0003ABCA
			public void Reset()
			{
				if (this.version != this.list._version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				this.currentElement = ArrayList.ArrayListEnumeratorSimple.dummyObject;
				this.index = -1;
			}

			// Token: 0x04000924 RID: 2340
			private ArrayList list;

			// Token: 0x04000925 RID: 2341
			private int index;

			// Token: 0x04000926 RID: 2342
			private int version;

			// Token: 0x04000927 RID: 2343
			private object currentElement;

			// Token: 0x04000928 RID: 2344
			[NonSerialized]
			private bool isArrayList;

			// Token: 0x04000929 RID: 2345
			private static object dummyObject = new object();
		}

		// Token: 0x02000242 RID: 578
		internal class ArrayListDebugView
		{
			// Token: 0x06001721 RID: 5921 RVA: 0x0003BC0D File Offset: 0x0003AC0D
			public ArrayListDebugView(ArrayList arrayList)
			{
				if (arrayList == null)
				{
					throw new ArgumentNullException("arrayList");
				}
				this.arrayList = arrayList;
			}

			// Token: 0x17000330 RID: 816
			// (get) Token: 0x06001722 RID: 5922 RVA: 0x0003BC2A File Offset: 0x0003AC2A
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public object[] Items
			{
				get
				{
					return this.arrayList.ToArray();
				}
			}

			// Token: 0x0400092A RID: 2346
			private ArrayList arrayList;
		}
	}
}
