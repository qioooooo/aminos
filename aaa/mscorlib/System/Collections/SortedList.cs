using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Collections
{
	// Token: 0x02000264 RID: 612
	[DebuggerTypeProxy(typeof(SortedList.SortedListDebugView))]
	[DebuggerDisplay("Count = {Count}")]
	[ComVisible(true)]
	[Serializable]
	public class SortedList : IDictionary, ICollection, IEnumerable, ICloneable
	{
		// Token: 0x0600187E RID: 6270 RVA: 0x0004027A File Offset: 0x0003F27A
		public SortedList()
		{
			this.keys = SortedList.emptyArray;
			this.values = SortedList.emptyArray;
			this._size = 0;
			this.comparer = new Comparer(CultureInfo.CurrentCulture);
		}

		// Token: 0x0600187F RID: 6271 RVA: 0x000402B0 File Offset: 0x0003F2B0
		public SortedList(int initialCapacity)
		{
			if (initialCapacity < 0)
			{
				throw new ArgumentOutOfRangeException("initialCapacity", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			this.keys = new object[initialCapacity];
			this.values = new object[initialCapacity];
			this.comparer = new Comparer(CultureInfo.CurrentCulture);
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x00040304 File Offset: 0x0003F304
		public SortedList(IComparer comparer)
			: this()
		{
			if (comparer != null)
			{
				this.comparer = comparer;
			}
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x00040316 File Offset: 0x0003F316
		public SortedList(IComparer comparer, int capacity)
			: this(comparer)
		{
			this.Capacity = capacity;
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x00040326 File Offset: 0x0003F326
		public SortedList(IDictionary d)
			: this(d, null)
		{
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x00040330 File Offset: 0x0003F330
		public SortedList(IDictionary d, IComparer comparer)
			: this(comparer, (d != null) ? d.Count : 0)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d", Environment.GetResourceString("ArgumentNull_Dictionary"));
			}
			d.Keys.CopyTo(this.keys, 0);
			d.Values.CopyTo(this.values, 0);
			Array.Sort(this.keys, this.values, comparer);
			this._size = d.Count;
		}

		// Token: 0x06001884 RID: 6276 RVA: 0x000403AC File Offset: 0x0003F3AC
		public virtual void Add(object key, object value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
			}
			int num = Array.BinarySearch(this.keys, 0, this._size, key, this.comparer);
			if (num >= 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_AddingDuplicate__", new object[]
				{
					this.GetKey(num),
					key
				}));
			}
			this.Insert(~num, key, value);
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06001885 RID: 6277 RVA: 0x0004041F File Offset: 0x0003F41F
		// (set) Token: 0x06001886 RID: 6278 RVA: 0x0004042C File Offset: 0x0003F42C
		public virtual int Capacity
		{
			get
			{
				return this.keys.Length;
			}
			set
			{
				if (value != this.keys.Length)
				{
					if (value < this._size)
					{
						throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_SmallCapacity"));
					}
					if (value > 0)
					{
						object[] array = new object[value];
						object[] array2 = new object[value];
						if (this._size > 0)
						{
							Array.Copy(this.keys, 0, array, 0, this._size);
							Array.Copy(this.values, 0, array2, 0, this._size);
						}
						this.keys = array;
						this.values = array2;
						return;
					}
					this.keys = SortedList.emptyArray;
					this.values = SortedList.emptyArray;
				}
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06001887 RID: 6279 RVA: 0x000404CD File Offset: 0x0003F4CD
		public virtual int Count
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06001888 RID: 6280 RVA: 0x000404D5 File Offset: 0x0003F4D5
		public virtual ICollection Keys
		{
			get
			{
				return this.GetKeyList();
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06001889 RID: 6281 RVA: 0x000404DD File Offset: 0x0003F4DD
		public virtual ICollection Values
		{
			get
			{
				return this.GetValueList();
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x0600188A RID: 6282 RVA: 0x000404E5 File Offset: 0x0003F4E5
		public virtual bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x0600188B RID: 6283 RVA: 0x000404E8 File Offset: 0x0003F4E8
		public virtual bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x0600188C RID: 6284 RVA: 0x000404EB File Offset: 0x0003F4EB
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x0600188D RID: 6285 RVA: 0x000404EE File Offset: 0x0003F4EE
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

		// Token: 0x0600188E RID: 6286 RVA: 0x00040510 File Offset: 0x0003F510
		public virtual void Clear()
		{
			this.version++;
			Array.Clear(this.keys, 0, this._size);
			Array.Clear(this.values, 0, this._size);
			this._size = 0;
		}

		// Token: 0x0600188F RID: 6287 RVA: 0x0004054C File Offset: 0x0003F54C
		public virtual object Clone()
		{
			SortedList sortedList = new SortedList(this._size);
			Array.Copy(this.keys, 0, sortedList.keys, 0, this._size);
			Array.Copy(this.values, 0, sortedList.values, 0, this._size);
			sortedList._size = this._size;
			sortedList.version = this.version;
			sortedList.comparer = this.comparer;
			return sortedList;
		}

		// Token: 0x06001890 RID: 6288 RVA: 0x000405BC File Offset: 0x0003F5BC
		public virtual bool Contains(object key)
		{
			return this.IndexOfKey(key) >= 0;
		}

		// Token: 0x06001891 RID: 6289 RVA: 0x000405CB File Offset: 0x0003F5CB
		public virtual bool ContainsKey(object key)
		{
			return this.IndexOfKey(key) >= 0;
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x000405DA File Offset: 0x0003F5DA
		public virtual bool ContainsValue(object value)
		{
			return this.IndexOfValue(value) >= 0;
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x000405EC File Offset: 0x0003F5EC
		public virtual void CopyTo(Array array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array", Environment.GetResourceString("ArgumentNull_Array"));
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_RankMultiDimNotSupported"));
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (array.Length - arrayIndex < this.Count)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ArrayPlusOffTooSmall"));
			}
			for (int i = 0; i < this.Count; i++)
			{
				DictionaryEntry dictionaryEntry = new DictionaryEntry(this.keys[i], this.values[i]);
				array.SetValue(dictionaryEntry, i + arrayIndex);
			}
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x0004069C File Offset: 0x0003F69C
		internal virtual KeyValuePairs[] ToKeyValuePairsArray()
		{
			KeyValuePairs[] array = new KeyValuePairs[this.Count];
			for (int i = 0; i < this.Count; i++)
			{
				array[i] = new KeyValuePairs(this.keys[i], this.values[i]);
			}
			return array;
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x000406E0 File Offset: 0x0003F6E0
		private void EnsureCapacity(int min)
		{
			int num = ((this.keys.Length == 0) ? 16 : (this.keys.Length * 2));
			if (num < min)
			{
				num = min;
			}
			this.Capacity = num;
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x00040713 File Offset: 0x0003F713
		public virtual object GetByIndex(int index)
		{
			if (index < 0 || index >= this._size)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			return this.values[index];
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x0004073F File Offset: 0x0003F73F
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new SortedList.SortedListEnumerator(this, 0, this._size, 3);
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x0004074F File Offset: 0x0003F74F
		public virtual IDictionaryEnumerator GetEnumerator()
		{
			return new SortedList.SortedListEnumerator(this, 0, this._size, 3);
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x0004075F File Offset: 0x0003F75F
		public virtual object GetKey(int index)
		{
			if (index < 0 || index >= this._size)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			return this.keys[index];
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x0004078B File Offset: 0x0003F78B
		public virtual IList GetKeyList()
		{
			if (this.keyList == null)
			{
				this.keyList = new SortedList.KeyList(this);
			}
			return this.keyList;
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x000407A7 File Offset: 0x0003F7A7
		public virtual IList GetValueList()
		{
			if (this.valueList == null)
			{
				this.valueList = new SortedList.ValueList(this);
			}
			return this.valueList;
		}

		// Token: 0x170003AB RID: 939
		public virtual object this[object key]
		{
			get
			{
				int num = this.IndexOfKey(key);
				if (num >= 0)
				{
					return this.values[num];
				}
				return null;
			}
			set
			{
				if (key == null)
				{
					throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
				}
				int num = Array.BinarySearch(this.keys, 0, this._size, key, this.comparer);
				if (num >= 0)
				{
					this.values[num] = value;
					this.version++;
					return;
				}
				this.Insert(~num, key, value);
			}
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x00040850 File Offset: 0x0003F850
		public virtual int IndexOfKey(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
			}
			int num = Array.BinarySearch(this.keys, 0, this._size, key, this.comparer);
			if (num < 0)
			{
				return -1;
			}
			return num;
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x00040896 File Offset: 0x0003F896
		public virtual int IndexOfValue(object value)
		{
			return Array.IndexOf<object>(this.values, value, 0, this._size);
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x000408AC File Offset: 0x0003F8AC
		private void Insert(int index, object key, object value)
		{
			if (this._size == this.keys.Length)
			{
				this.EnsureCapacity(this._size + 1);
			}
			if (index < this._size)
			{
				Array.Copy(this.keys, index, this.keys, index + 1, this._size - index);
				Array.Copy(this.values, index, this.values, index + 1, this._size - index);
			}
			this.keys[index] = key;
			this.values[index] = value;
			this._size++;
			this.version++;
		}

		// Token: 0x060018A1 RID: 6305 RVA: 0x00040948 File Offset: 0x0003F948
		public virtual void RemoveAt(int index)
		{
			if (index < 0 || index >= this._size)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			this._size--;
			if (index < this._size)
			{
				Array.Copy(this.keys, index + 1, this.keys, index, this._size - index);
				Array.Copy(this.values, index + 1, this.values, index, this._size - index);
			}
			this.keys[this._size] = null;
			this.values[this._size] = null;
			this.version++;
		}

		// Token: 0x060018A2 RID: 6306 RVA: 0x000409F4 File Offset: 0x0003F9F4
		public virtual void Remove(object key)
		{
			int num = this.IndexOfKey(key);
			if (num >= 0)
			{
				this.RemoveAt(num);
			}
		}

		// Token: 0x060018A3 RID: 6307 RVA: 0x00040A14 File Offset: 0x0003FA14
		public virtual void SetByIndex(int index, object value)
		{
			if (index < 0 || index >= this._size)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			this.values[index] = value;
			this.version++;
		}

		// Token: 0x060018A4 RID: 6308 RVA: 0x00040A4F File Offset: 0x0003FA4F
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public static SortedList Synchronized(SortedList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			return new SortedList.SyncSortedList(list);
		}

		// Token: 0x060018A5 RID: 6309 RVA: 0x00040A65 File Offset: 0x0003FA65
		public virtual void TrimToSize()
		{
			this.Capacity = this._size;
		}

		// Token: 0x0400098D RID: 2445
		private const int _defaultCapacity = 16;

		// Token: 0x0400098E RID: 2446
		private object[] keys;

		// Token: 0x0400098F RID: 2447
		private object[] values;

		// Token: 0x04000990 RID: 2448
		private int _size;

		// Token: 0x04000991 RID: 2449
		private int version;

		// Token: 0x04000992 RID: 2450
		private IComparer comparer;

		// Token: 0x04000993 RID: 2451
		private SortedList.KeyList keyList;

		// Token: 0x04000994 RID: 2452
		private SortedList.ValueList valueList;

		// Token: 0x04000995 RID: 2453
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x04000996 RID: 2454
		private static object[] emptyArray = new object[0];

		// Token: 0x02000265 RID: 613
		[Serializable]
		private class SyncSortedList : SortedList
		{
			// Token: 0x060018A7 RID: 6311 RVA: 0x00040A80 File Offset: 0x0003FA80
			internal SyncSortedList(SortedList list)
			{
				this._list = list;
				this._root = list.SyncRoot;
			}

			// Token: 0x170003AC RID: 940
			// (get) Token: 0x060018A8 RID: 6312 RVA: 0x00040A9C File Offset: 0x0003FA9C
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

			// Token: 0x170003AD RID: 941
			// (get) Token: 0x060018A9 RID: 6313 RVA: 0x00040ADC File Offset: 0x0003FADC
			public override object SyncRoot
			{
				get
				{
					return this._root;
				}
			}

			// Token: 0x170003AE RID: 942
			// (get) Token: 0x060018AA RID: 6314 RVA: 0x00040AE4 File Offset: 0x0003FAE4
			public override bool IsReadOnly
			{
				get
				{
					return this._list.IsReadOnly;
				}
			}

			// Token: 0x170003AF RID: 943
			// (get) Token: 0x060018AB RID: 6315 RVA: 0x00040AF1 File Offset: 0x0003FAF1
			public override bool IsFixedSize
			{
				get
				{
					return this._list.IsFixedSize;
				}
			}

			// Token: 0x170003B0 RID: 944
			// (get) Token: 0x060018AC RID: 6316 RVA: 0x00040AFE File Offset: 0x0003FAFE
			public override bool IsSynchronized
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170003B1 RID: 945
			public override object this[object key]
			{
				get
				{
					object obj;
					lock (this._root)
					{
						obj = this._list[key];
					}
					return obj;
				}
				set
				{
					lock (this._root)
					{
						this._list[key] = value;
					}
				}
			}

			// Token: 0x060018AF RID: 6319 RVA: 0x00040B88 File Offset: 0x0003FB88
			public override void Add(object key, object value)
			{
				lock (this._root)
				{
					this._list.Add(key, value);
				}
			}

			// Token: 0x170003B2 RID: 946
			// (get) Token: 0x060018B0 RID: 6320 RVA: 0x00040BC8 File Offset: 0x0003FBC8
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
			}

			// Token: 0x060018B1 RID: 6321 RVA: 0x00040C08 File Offset: 0x0003FC08
			public override void Clear()
			{
				lock (this._root)
				{
					this._list.Clear();
				}
			}

			// Token: 0x060018B2 RID: 6322 RVA: 0x00040C48 File Offset: 0x0003FC48
			public override object Clone()
			{
				object obj;
				lock (this._root)
				{
					obj = this._list.Clone();
				}
				return obj;
			}

			// Token: 0x060018B3 RID: 6323 RVA: 0x00040C88 File Offset: 0x0003FC88
			public override bool Contains(object key)
			{
				bool flag;
				lock (this._root)
				{
					flag = this._list.Contains(key);
				}
				return flag;
			}

			// Token: 0x060018B4 RID: 6324 RVA: 0x00040CCC File Offset: 0x0003FCCC
			public override bool ContainsKey(object key)
			{
				bool flag;
				lock (this._root)
				{
					flag = this._list.ContainsKey(key);
				}
				return flag;
			}

			// Token: 0x060018B5 RID: 6325 RVA: 0x00040D10 File Offset: 0x0003FD10
			public override bool ContainsValue(object key)
			{
				bool flag;
				lock (this._root)
				{
					flag = this._list.ContainsValue(key);
				}
				return flag;
			}

			// Token: 0x060018B6 RID: 6326 RVA: 0x00040D54 File Offset: 0x0003FD54
			public override void CopyTo(Array array, int index)
			{
				lock (this._root)
				{
					this._list.CopyTo(array, index);
				}
			}

			// Token: 0x060018B7 RID: 6327 RVA: 0x00040D94 File Offset: 0x0003FD94
			public override object GetByIndex(int index)
			{
				object byIndex;
				lock (this._root)
				{
					byIndex = this._list.GetByIndex(index);
				}
				return byIndex;
			}

			// Token: 0x060018B8 RID: 6328 RVA: 0x00040DD8 File Offset: 0x0003FDD8
			public override IDictionaryEnumerator GetEnumerator()
			{
				IDictionaryEnumerator enumerator;
				lock (this._root)
				{
					enumerator = this._list.GetEnumerator();
				}
				return enumerator;
			}

			// Token: 0x060018B9 RID: 6329 RVA: 0x00040E18 File Offset: 0x0003FE18
			public override object GetKey(int index)
			{
				object key;
				lock (this._root)
				{
					key = this._list.GetKey(index);
				}
				return key;
			}

			// Token: 0x060018BA RID: 6330 RVA: 0x00040E5C File Offset: 0x0003FE5C
			public override IList GetKeyList()
			{
				IList keyList;
				lock (this._root)
				{
					keyList = this._list.GetKeyList();
				}
				return keyList;
			}

			// Token: 0x060018BB RID: 6331 RVA: 0x00040E9C File Offset: 0x0003FE9C
			public override IList GetValueList()
			{
				IList valueList;
				lock (this._root)
				{
					valueList = this._list.GetValueList();
				}
				return valueList;
			}

			// Token: 0x060018BC RID: 6332 RVA: 0x00040EDC File Offset: 0x0003FEDC
			public override int IndexOfKey(object key)
			{
				int num;
				lock (this._root)
				{
					num = this._list.IndexOfKey(key);
				}
				return num;
			}

			// Token: 0x060018BD RID: 6333 RVA: 0x00040F20 File Offset: 0x0003FF20
			public override int IndexOfValue(object value)
			{
				int num;
				lock (this._root)
				{
					num = this._list.IndexOfValue(value);
				}
				return num;
			}

			// Token: 0x060018BE RID: 6334 RVA: 0x00040F64 File Offset: 0x0003FF64
			public override void RemoveAt(int index)
			{
				lock (this._root)
				{
					this._list.RemoveAt(index);
				}
			}

			// Token: 0x060018BF RID: 6335 RVA: 0x00040FA4 File Offset: 0x0003FFA4
			public override void Remove(object key)
			{
				lock (this._root)
				{
					this._list.Remove(key);
				}
			}

			// Token: 0x060018C0 RID: 6336 RVA: 0x00040FE4 File Offset: 0x0003FFE4
			public override void SetByIndex(int index, object value)
			{
				lock (this._root)
				{
					this._list.SetByIndex(index, value);
				}
			}

			// Token: 0x060018C1 RID: 6337 RVA: 0x00041024 File Offset: 0x00040024
			internal override KeyValuePairs[] ToKeyValuePairsArray()
			{
				return this._list.ToKeyValuePairsArray();
			}

			// Token: 0x060018C2 RID: 6338 RVA: 0x00041034 File Offset: 0x00040034
			public override void TrimToSize()
			{
				lock (this._root)
				{
					this._list.TrimToSize();
				}
			}

			// Token: 0x04000997 RID: 2455
			private SortedList _list;

			// Token: 0x04000998 RID: 2456
			private object _root;
		}

		// Token: 0x02000266 RID: 614
		[Serializable]
		private class SortedListEnumerator : IDictionaryEnumerator, IEnumerator, ICloneable
		{
			// Token: 0x060018C3 RID: 6339 RVA: 0x00041074 File Offset: 0x00040074
			internal SortedListEnumerator(SortedList sortedList, int index, int count, int getObjRetType)
			{
				this.sortedList = sortedList;
				this.index = index;
				this.startIndex = index;
				this.endIndex = index + count;
				this.version = sortedList.version;
				this.getObjectRetType = getObjRetType;
				this.current = false;
			}

			// Token: 0x060018C4 RID: 6340 RVA: 0x000410C0 File Offset: 0x000400C0
			public object Clone()
			{
				return base.MemberwiseClone();
			}

			// Token: 0x170003B3 RID: 947
			// (get) Token: 0x060018C5 RID: 6341 RVA: 0x000410C8 File Offset: 0x000400C8
			public virtual object Key
			{
				get
				{
					if (this.version != this.sortedList.version)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
					}
					if (!this.current)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
					}
					return this.key;
				}
			}

			// Token: 0x060018C6 RID: 6342 RVA: 0x00041118 File Offset: 0x00040118
			public virtual bool MoveNext()
			{
				if (this.version != this.sortedList.version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				if (this.index < this.endIndex)
				{
					this.key = this.sortedList.keys[this.index];
					this.value = this.sortedList.values[this.index];
					this.index++;
					this.current = true;
					return true;
				}
				this.key = null;
				this.value = null;
				this.current = false;
				return false;
			}

			// Token: 0x170003B4 RID: 948
			// (get) Token: 0x060018C7 RID: 6343 RVA: 0x000411B4 File Offset: 0x000401B4
			public virtual DictionaryEntry Entry
			{
				get
				{
					if (this.version != this.sortedList.version)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
					}
					if (!this.current)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
					}
					return new DictionaryEntry(this.key, this.value);
				}
			}

			// Token: 0x170003B5 RID: 949
			// (get) Token: 0x060018C8 RID: 6344 RVA: 0x00041210 File Offset: 0x00040210
			public virtual object Current
			{
				get
				{
					if (!this.current)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
					}
					if (this.getObjectRetType == 1)
					{
						return this.key;
					}
					if (this.getObjectRetType == 2)
					{
						return this.value;
					}
					return new DictionaryEntry(this.key, this.value);
				}
			}

			// Token: 0x170003B6 RID: 950
			// (get) Token: 0x060018C9 RID: 6345 RVA: 0x0004126C File Offset: 0x0004026C
			public virtual object Value
			{
				get
				{
					if (this.version != this.sortedList.version)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
					}
					if (!this.current)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
					}
					return this.value;
				}
			}

			// Token: 0x060018CA RID: 6346 RVA: 0x000412BC File Offset: 0x000402BC
			public virtual void Reset()
			{
				if (this.version != this.sortedList.version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				this.index = this.startIndex;
				this.current = false;
				this.key = null;
				this.value = null;
			}

			// Token: 0x04000999 RID: 2457
			internal const int Keys = 1;

			// Token: 0x0400099A RID: 2458
			internal const int Values = 2;

			// Token: 0x0400099B RID: 2459
			internal const int DictEntry = 3;

			// Token: 0x0400099C RID: 2460
			private SortedList sortedList;

			// Token: 0x0400099D RID: 2461
			private object key;

			// Token: 0x0400099E RID: 2462
			private object value;

			// Token: 0x0400099F RID: 2463
			private int index;

			// Token: 0x040009A0 RID: 2464
			private int startIndex;

			// Token: 0x040009A1 RID: 2465
			private int endIndex;

			// Token: 0x040009A2 RID: 2466
			private int version;

			// Token: 0x040009A3 RID: 2467
			private bool current;

			// Token: 0x040009A4 RID: 2468
			private int getObjectRetType;
		}

		// Token: 0x02000267 RID: 615
		[Serializable]
		private class KeyList : IList, ICollection, IEnumerable
		{
			// Token: 0x060018CB RID: 6347 RVA: 0x0004130D File Offset: 0x0004030D
			internal KeyList(SortedList sortedList)
			{
				this.sortedList = sortedList;
			}

			// Token: 0x170003B7 RID: 951
			// (get) Token: 0x060018CC RID: 6348 RVA: 0x0004131C File Offset: 0x0004031C
			public virtual int Count
			{
				get
				{
					return this.sortedList._size;
				}
			}

			// Token: 0x170003B8 RID: 952
			// (get) Token: 0x060018CD RID: 6349 RVA: 0x00041329 File Offset: 0x00040329
			public virtual bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170003B9 RID: 953
			// (get) Token: 0x060018CE RID: 6350 RVA: 0x0004132C File Offset: 0x0004032C
			public virtual bool IsFixedSize
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170003BA RID: 954
			// (get) Token: 0x060018CF RID: 6351 RVA: 0x0004132F File Offset: 0x0004032F
			public virtual bool IsSynchronized
			{
				get
				{
					return this.sortedList.IsSynchronized;
				}
			}

			// Token: 0x170003BB RID: 955
			// (get) Token: 0x060018D0 RID: 6352 RVA: 0x0004133C File Offset: 0x0004033C
			public virtual object SyncRoot
			{
				get
				{
					return this.sortedList.SyncRoot;
				}
			}

			// Token: 0x060018D1 RID: 6353 RVA: 0x00041349 File Offset: 0x00040349
			public virtual int Add(object key)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SortedListNestedWrite"));
			}

			// Token: 0x060018D2 RID: 6354 RVA: 0x0004135A File Offset: 0x0004035A
			public virtual void Clear()
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SortedListNestedWrite"));
			}

			// Token: 0x060018D3 RID: 6355 RVA: 0x0004136B File Offset: 0x0004036B
			public virtual bool Contains(object key)
			{
				return this.sortedList.Contains(key);
			}

			// Token: 0x060018D4 RID: 6356 RVA: 0x00041379 File Offset: 0x00040379
			public virtual void CopyTo(Array array, int arrayIndex)
			{
				if (array != null && array.Rank != 1)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_RankMultiDimNotSupported"));
				}
				Array.Copy(this.sortedList.keys, 0, array, arrayIndex, this.sortedList.Count);
			}

			// Token: 0x060018D5 RID: 6357 RVA: 0x000413B5 File Offset: 0x000403B5
			public virtual void Insert(int index, object value)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SortedListNestedWrite"));
			}

			// Token: 0x170003BC RID: 956
			public virtual object this[int index]
			{
				get
				{
					return this.sortedList.GetKey(index);
				}
				set
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_KeyCollectionSet"));
				}
			}

			// Token: 0x060018D8 RID: 6360 RVA: 0x000413E5 File Offset: 0x000403E5
			public virtual IEnumerator GetEnumerator()
			{
				return new SortedList.SortedListEnumerator(this.sortedList, 0, this.sortedList.Count, 1);
			}

			// Token: 0x060018D9 RID: 6361 RVA: 0x00041400 File Offset: 0x00040400
			public virtual int IndexOf(object key)
			{
				if (key == null)
				{
					throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
				}
				int num = Array.BinarySearch(this.sortedList.keys, 0, this.sortedList.Count, key, this.sortedList.comparer);
				if (num >= 0)
				{
					return num;
				}
				return -1;
			}

			// Token: 0x060018DA RID: 6362 RVA: 0x00041455 File Offset: 0x00040455
			public virtual void Remove(object key)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SortedListNestedWrite"));
			}

			// Token: 0x060018DB RID: 6363 RVA: 0x00041466 File Offset: 0x00040466
			public virtual void RemoveAt(int index)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SortedListNestedWrite"));
			}

			// Token: 0x040009A5 RID: 2469
			private SortedList sortedList;
		}

		// Token: 0x02000268 RID: 616
		[Serializable]
		private class ValueList : IList, ICollection, IEnumerable
		{
			// Token: 0x060018DC RID: 6364 RVA: 0x00041477 File Offset: 0x00040477
			internal ValueList(SortedList sortedList)
			{
				this.sortedList = sortedList;
			}

			// Token: 0x170003BD RID: 957
			// (get) Token: 0x060018DD RID: 6365 RVA: 0x00041486 File Offset: 0x00040486
			public virtual int Count
			{
				get
				{
					return this.sortedList._size;
				}
			}

			// Token: 0x170003BE RID: 958
			// (get) Token: 0x060018DE RID: 6366 RVA: 0x00041493 File Offset: 0x00040493
			public virtual bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170003BF RID: 959
			// (get) Token: 0x060018DF RID: 6367 RVA: 0x00041496 File Offset: 0x00040496
			public virtual bool IsFixedSize
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170003C0 RID: 960
			// (get) Token: 0x060018E0 RID: 6368 RVA: 0x00041499 File Offset: 0x00040499
			public virtual bool IsSynchronized
			{
				get
				{
					return this.sortedList.IsSynchronized;
				}
			}

			// Token: 0x170003C1 RID: 961
			// (get) Token: 0x060018E1 RID: 6369 RVA: 0x000414A6 File Offset: 0x000404A6
			public virtual object SyncRoot
			{
				get
				{
					return this.sortedList.SyncRoot;
				}
			}

			// Token: 0x060018E2 RID: 6370 RVA: 0x000414B3 File Offset: 0x000404B3
			public virtual int Add(object key)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SortedListNestedWrite"));
			}

			// Token: 0x060018E3 RID: 6371 RVA: 0x000414C4 File Offset: 0x000404C4
			public virtual void Clear()
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SortedListNestedWrite"));
			}

			// Token: 0x060018E4 RID: 6372 RVA: 0x000414D5 File Offset: 0x000404D5
			public virtual bool Contains(object value)
			{
				return this.sortedList.ContainsValue(value);
			}

			// Token: 0x060018E5 RID: 6373 RVA: 0x000414E3 File Offset: 0x000404E3
			public virtual void CopyTo(Array array, int arrayIndex)
			{
				if (array != null && array.Rank != 1)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_RankMultiDimNotSupported"));
				}
				Array.Copy(this.sortedList.values, 0, array, arrayIndex, this.sortedList.Count);
			}

			// Token: 0x060018E6 RID: 6374 RVA: 0x0004151F File Offset: 0x0004051F
			public virtual void Insert(int index, object value)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SortedListNestedWrite"));
			}

			// Token: 0x170003C2 RID: 962
			public virtual object this[int index]
			{
				get
				{
					return this.sortedList.GetByIndex(index);
				}
				set
				{
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_SortedListNestedWrite"));
				}
			}

			// Token: 0x060018E9 RID: 6377 RVA: 0x0004154F File Offset: 0x0004054F
			public virtual IEnumerator GetEnumerator()
			{
				return new SortedList.SortedListEnumerator(this.sortedList, 0, this.sortedList.Count, 2);
			}

			// Token: 0x060018EA RID: 6378 RVA: 0x00041569 File Offset: 0x00040569
			public virtual int IndexOf(object value)
			{
				return Array.IndexOf<object>(this.sortedList.values, value, 0, this.sortedList.Count);
			}

			// Token: 0x060018EB RID: 6379 RVA: 0x00041588 File Offset: 0x00040588
			public virtual void Remove(object value)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SortedListNestedWrite"));
			}

			// Token: 0x060018EC RID: 6380 RVA: 0x00041599 File Offset: 0x00040599
			public virtual void RemoveAt(int index)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SortedListNestedWrite"));
			}

			// Token: 0x040009A6 RID: 2470
			private SortedList sortedList;
		}

		// Token: 0x02000269 RID: 617
		internal class SortedListDebugView
		{
			// Token: 0x060018ED RID: 6381 RVA: 0x000415AA File Offset: 0x000405AA
			public SortedListDebugView(SortedList sortedList)
			{
				if (sortedList == null)
				{
					throw new ArgumentNullException("sortedList");
				}
				this.sortedList = sortedList;
			}

			// Token: 0x170003C3 RID: 963
			// (get) Token: 0x060018EE RID: 6382 RVA: 0x000415C7 File Offset: 0x000405C7
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public KeyValuePairs[] Items
			{
				get
				{
					return this.sortedList.ToKeyValuePairsArray();
				}
			}

			// Token: 0x040009A7 RID: 2471
			private SortedList sortedList;
		}
	}
}
