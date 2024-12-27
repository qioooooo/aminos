using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Collections.Generic
{
	// Token: 0x02000234 RID: 564
	[DebuggerTypeProxy(typeof(System_DictionaryDebugView<, >))]
	[DebuggerDisplay("Count = {Count}")]
	[ComVisible(false)]
	[Serializable]
	public class SortedList<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary, ICollection, IEnumerable
	{
		// Token: 0x060012E7 RID: 4839 RVA: 0x0003FB32 File Offset: 0x0003EB32
		public SortedList()
		{
			this.keys = SortedList<TKey, TValue>.emptyKeys;
			this.values = SortedList<TKey, TValue>.emptyValues;
			this._size = 0;
			this.comparer = Comparer<TKey>.Default;
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x0003FB62 File Offset: 0x0003EB62
		public SortedList(int capacity)
		{
			if (capacity < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity, ExceptionResource.ArgumentOutOfRange_NeedNonNegNumRequired);
			}
			this.keys = new TKey[capacity];
			this.values = new TValue[capacity];
			this.comparer = Comparer<TKey>.Default;
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x0003FB99 File Offset: 0x0003EB99
		public SortedList(IComparer<TKey> comparer)
			: this()
		{
			if (comparer != null)
			{
				this.comparer = comparer;
			}
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x0003FBAB File Offset: 0x0003EBAB
		public SortedList(int capacity, IComparer<TKey> comparer)
			: this(comparer)
		{
			this.Capacity = capacity;
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x0003FBBB File Offset: 0x0003EBBB
		public SortedList(IDictionary<TKey, TValue> dictionary)
			: this(dictionary, null)
		{
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x0003FBC8 File Offset: 0x0003EBC8
		public SortedList(IDictionary<TKey, TValue> dictionary, IComparer<TKey> comparer)
			: this((dictionary != null) ? dictionary.Count : 0, comparer)
		{
			if (dictionary == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
			}
			dictionary.Keys.CopyTo(this.keys, 0);
			dictionary.Values.CopyTo(this.values, 0);
			Array.Sort<TKey, TValue>(this.keys, this.values, comparer);
			this._size = dictionary.Count;
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x0003FC34 File Offset: 0x0003EC34
		public void Add(TKey key, TValue value)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			int num = Array.BinarySearch<TKey>(this.keys, 0, this._size, key, this.comparer);
			if (num >= 0)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_AddingDuplicate);
			}
			this.Insert(~num, key, value);
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x0003FC7D File Offset: 0x0003EC7D
		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
		{
			this.Add(keyValuePair.Key, keyValuePair.Value);
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x0003FC94 File Offset: 0x0003EC94
		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
		{
			int num = this.IndexOfKey(keyValuePair.Key);
			return num >= 0 && EqualityComparer<TValue>.Default.Equals(this.values[num], keyValuePair.Value);
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x0003FCD8 File Offset: 0x0003ECD8
		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
		{
			int num = this.IndexOfKey(keyValuePair.Key);
			if (num >= 0 && EqualityComparer<TValue>.Default.Equals(this.values[num], keyValuePair.Value))
			{
				this.RemoveAt(num);
				return true;
			}
			return false;
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x060012F1 RID: 4849 RVA: 0x0003FD20 File Offset: 0x0003ED20
		// (set) Token: 0x060012F2 RID: 4850 RVA: 0x0003FD2C File Offset: 0x0003ED2C
		public int Capacity
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
						ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value, ExceptionResource.ArgumentOutOfRange_SmallCapacity);
					}
					if (value > 0)
					{
						TKey[] array = new TKey[value];
						TValue[] array2 = new TValue[value];
						if (this._size > 0)
						{
							Array.Copy(this.keys, 0, array, 0, this._size);
							Array.Copy(this.values, 0, array2, 0, this._size);
						}
						this.keys = array;
						this.values = array2;
						return;
					}
					this.keys = SortedList<TKey, TValue>.emptyKeys;
					this.values = SortedList<TKey, TValue>.emptyValues;
				}
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x060012F3 RID: 4851 RVA: 0x0003FDBE File Offset: 0x0003EDBE
		public IComparer<TKey> Comparer
		{
			get
			{
				return this.comparer;
			}
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x0003FDC6 File Offset: 0x0003EDC6
		void IDictionary.Add(object key, object value)
		{
			SortedList<TKey, TValue>.VerifyKey(key);
			SortedList<TKey, TValue>.VerifyValueType(value);
			this.Add((TKey)((object)key), (TValue)((object)value));
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x060012F5 RID: 4853 RVA: 0x0003FDE6 File Offset: 0x0003EDE6
		public int Count
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x060012F6 RID: 4854 RVA: 0x0003FDEE File Offset: 0x0003EDEE
		public IList<TKey> Keys
		{
			get
			{
				return this.GetKeyListHelper();
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x060012F7 RID: 4855 RVA: 0x0003FDF6 File Offset: 0x0003EDF6
		ICollection<TKey> IDictionary<TKey, TValue>.Keys
		{
			get
			{
				return this.GetKeyListHelper();
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x060012F8 RID: 4856 RVA: 0x0003FDFE File Offset: 0x0003EDFE
		ICollection IDictionary.Keys
		{
			get
			{
				return this.GetKeyListHelper();
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x060012F9 RID: 4857 RVA: 0x0003FE06 File Offset: 0x0003EE06
		public IList<TValue> Values
		{
			get
			{
				return this.GetValueListHelper();
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x060012FA RID: 4858 RVA: 0x0003FE0E File Offset: 0x0003EE0E
		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			get
			{
				return this.GetValueListHelper();
			}
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x060012FB RID: 4859 RVA: 0x0003FE16 File Offset: 0x0003EE16
		ICollection IDictionary.Values
		{
			get
			{
				return this.GetValueListHelper();
			}
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x0003FE1E File Offset: 0x0003EE1E
		private SortedList<TKey, TValue>.KeyList GetKeyListHelper()
		{
			if (this.keyList == null)
			{
				this.keyList = new SortedList<TKey, TValue>.KeyList(this);
			}
			return this.keyList;
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x0003FE3A File Offset: 0x0003EE3A
		private SortedList<TKey, TValue>.ValueList GetValueListHelper()
		{
			if (this.valueList == null)
			{
				this.valueList = new SortedList<TKey, TValue>.ValueList(this);
			}
			return this.valueList;
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x060012FE RID: 4862 RVA: 0x0003FE56 File Offset: 0x0003EE56
		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x060012FF RID: 4863 RVA: 0x0003FE59 File Offset: 0x0003EE59
		bool IDictionary.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06001300 RID: 4864 RVA: 0x0003FE5C File Offset: 0x0003EE5C
		bool IDictionary.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06001301 RID: 4865 RVA: 0x0003FE5F File Offset: 0x0003EE5F
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06001302 RID: 4866 RVA: 0x0003FE62 File Offset: 0x0003EE62
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

		// Token: 0x06001303 RID: 4867 RVA: 0x0003FE84 File Offset: 0x0003EE84
		public void Clear()
		{
			this.version++;
			Array.Clear(this.keys, 0, this._size);
			Array.Clear(this.values, 0, this._size);
			this._size = 0;
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x0003FEBF File Offset: 0x0003EEBF
		bool IDictionary.Contains(object key)
		{
			return SortedList<TKey, TValue>.IsCompatibleKey(key) && this.ContainsKey((TKey)((object)key));
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x0003FED7 File Offset: 0x0003EED7
		public bool ContainsKey(TKey key)
		{
			return this.IndexOfKey(key) >= 0;
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x0003FEE6 File Offset: 0x0003EEE6
		public bool ContainsValue(TValue value)
		{
			return this.IndexOfValue(value) >= 0;
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x0003FEF8 File Offset: 0x0003EEF8
		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (array == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
			}
			if (arrayIndex < 0 || arrayIndex > array.Length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.arrayIndex, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (array.Length - arrayIndex < this.Count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			for (int i = 0; i < this.Count; i++)
			{
				KeyValuePair<TKey, TValue> keyValuePair = new KeyValuePair<TKey, TValue>(this.keys[i], this.values[i]);
				array[arrayIndex + i] = keyValuePair;
			}
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x0003FF74 File Offset: 0x0003EF74
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
			if (array.Length - arrayIndex < this.Count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			KeyValuePair<TKey, TValue>[] array2 = array as KeyValuePair<TKey, TValue>[];
			if (array2 != null)
			{
				for (int i = 0; i < this.Count; i++)
				{
					array2[i + arrayIndex] = new KeyValuePair<TKey, TValue>(this.keys[i], this.values[i]);
				}
				return;
			}
			object[] array3 = array as object[];
			if (array3 == null)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
			}
			try
			{
				for (int j = 0; j < this.Count; j++)
				{
					array3[j + arrayIndex] = new KeyValuePair<TKey, TValue>(this.keys[j], this.values[j]);
				}
			}
			catch (ArrayTypeMismatchException)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
			}
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x00040080 File Offset: 0x0003F080
		private void EnsureCapacity(int min)
		{
			int num = ((this.keys.Length == 0) ? 4 : (this.keys.Length * 2));
			if (num < min)
			{
				num = min;
			}
			this.Capacity = num;
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x000400B2 File Offset: 0x0003F0B2
		private TValue GetByIndex(int index)
		{
			if (index < 0 || index >= this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
			}
			return this.values[index];
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x000400D6 File Offset: 0x0003F0D6
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return new SortedList<TKey, TValue>.Enumerator(this, 1);
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x000400E4 File Offset: 0x0003F0E4
		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return new SortedList<TKey, TValue>.Enumerator(this, 1);
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x000400F2 File Offset: 0x0003F0F2
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new SortedList<TKey, TValue>.Enumerator(this, 2);
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x00040100 File Offset: 0x0003F100
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new SortedList<TKey, TValue>.Enumerator(this, 1);
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x0004010E File Offset: 0x0003F10E
		private TKey GetKey(int index)
		{
			if (index < 0 || index >= this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
			}
			return this.keys[index];
		}

		// Token: 0x170003D8 RID: 984
		public TValue this[TKey key]
		{
			get
			{
				int num = this.IndexOfKey(key);
				if (num >= 0)
				{
					return this.values[num];
				}
				ThrowHelper.ThrowKeyNotFoundException();
				return default(TValue);
			}
			set
			{
				if (key == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
				}
				int num = Array.BinarySearch<TKey>(this.keys, 0, this._size, key, this.comparer);
				if (num >= 0)
				{
					this.values[num] = value;
					this.version++;
					return;
				}
				this.Insert(~num, key, value);
			}
		}

		// Token: 0x170003D9 RID: 985
		object IDictionary.this[object key]
		{
			get
			{
				if (SortedList<TKey, TValue>.IsCompatibleKey(key))
				{
					int num = this.IndexOfKey((TKey)((object)key));
					if (num >= 0)
					{
						return this.values[num];
					}
				}
				return null;
			}
			set
			{
				SortedList<TKey, TValue>.VerifyKey(key);
				SortedList<TKey, TValue>.VerifyValueType(value);
				this[(TKey)((object)key)] = (TValue)((object)value);
			}
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x00040224 File Offset: 0x0003F224
		public int IndexOfKey(TKey key)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			int num = Array.BinarySearch<TKey>(this.keys, 0, this._size, key, this.comparer);
			if (num < 0)
			{
				return -1;
			}
			return num;
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x00040260 File Offset: 0x0003F260
		public int IndexOfValue(TValue value)
		{
			return Array.IndexOf<TValue>(this.values, value, 0, this._size);
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x00040278 File Offset: 0x0003F278
		private void Insert(int index, TKey key, TValue value)
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

		// Token: 0x06001317 RID: 4887 RVA: 0x0004031C File Offset: 0x0003F31C
		public bool TryGetValue(TKey key, out TValue value)
		{
			int num = this.IndexOfKey(key);
			if (num >= 0)
			{
				value = this.values[num];
				return true;
			}
			value = default(TValue);
			return false;
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x00040354 File Offset: 0x0003F354
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this._size)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);
			}
			this._size--;
			if (index < this._size)
			{
				Array.Copy(this.keys, index + 1, this.keys, index, this._size - index);
				Array.Copy(this.values, index + 1, this.values, index, this._size - index);
			}
			this.keys[this._size] = default(TKey);
			this.values[this._size] = default(TValue);
			this.version++;
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x0004040C File Offset: 0x0003F40C
		public bool Remove(TKey key)
		{
			int num = this.IndexOfKey(key);
			if (num >= 0)
			{
				this.RemoveAt(num);
			}
			return num >= 0;
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x00040433 File Offset: 0x0003F433
		void IDictionary.Remove(object key)
		{
			if (SortedList<TKey, TValue>.IsCompatibleKey(key))
			{
				this.Remove((TKey)((object)key));
			}
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x0004044C File Offset: 0x0003F44C
		public void TrimExcess()
		{
			int num = (int)((double)this.keys.Length * 0.9);
			if (this._size < num)
			{
				this.Capacity = this._size;
			}
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x00040483 File Offset: 0x0003F483
		private static void VerifyKey(object key)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			if (!(key is TKey))
			{
				ThrowHelper.ThrowWrongKeyTypeArgumentException(key, typeof(TKey));
			}
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x000404A6 File Offset: 0x0003F4A6
		private static bool IsCompatibleKey(object key)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			return key is TKey;
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x000404BA File Offset: 0x0003F4BA
		private static void VerifyValueType(object value)
		{
			if (value is TValue || (value == null && !typeof(TValue).IsValueType))
			{
				return;
			}
			ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(TValue));
		}

		// Token: 0x040010F0 RID: 4336
		private const int _defaultCapacity = 4;

		// Token: 0x040010F1 RID: 4337
		private TKey[] keys;

		// Token: 0x040010F2 RID: 4338
		private TValue[] values;

		// Token: 0x040010F3 RID: 4339
		private int _size;

		// Token: 0x040010F4 RID: 4340
		private int version;

		// Token: 0x040010F5 RID: 4341
		private IComparer<TKey> comparer;

		// Token: 0x040010F6 RID: 4342
		private SortedList<TKey, TValue>.KeyList keyList;

		// Token: 0x040010F7 RID: 4343
		private SortedList<TKey, TValue>.ValueList valueList;

		// Token: 0x040010F8 RID: 4344
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x040010F9 RID: 4345
		private static TKey[] emptyKeys = new TKey[0];

		// Token: 0x040010FA RID: 4346
		private static TValue[] emptyValues = new TValue[0];

		// Token: 0x02000235 RID: 565
		[Serializable]
		private struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDisposable, IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x06001320 RID: 4896 RVA: 0x00040501 File Offset: 0x0003F501
			internal Enumerator(SortedList<TKey, TValue> sortedList, int getEnumeratorRetType)
			{
				this._sortedList = sortedList;
				this.index = 0;
				this.version = this._sortedList.version;
				this.getEnumeratorRetType = getEnumeratorRetType;
				this.key = default(TKey);
				this.value = default(TValue);
			}

			// Token: 0x06001321 RID: 4897 RVA: 0x00040541 File Offset: 0x0003F541
			public void Dispose()
			{
				this.index = 0;
				this.key = default(TKey);
				this.value = default(TValue);
			}

			// Token: 0x170003DA RID: 986
			// (get) Token: 0x06001322 RID: 4898 RVA: 0x00040562 File Offset: 0x0003F562
			object IDictionaryEnumerator.Key
			{
				get
				{
					if (this.index == 0 || this.index == this._sortedList.Count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return this.key;
				}
			}

			// Token: 0x06001323 RID: 4899 RVA: 0x00040594 File Offset: 0x0003F594
			public bool MoveNext()
			{
				if (this.version != this._sortedList.version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				if (this.index < this._sortedList.Count)
				{
					this.key = this._sortedList.keys[this.index];
					this.value = this._sortedList.values[this.index];
					this.index++;
					return true;
				}
				this.index = this._sortedList.Count + 1;
				this.key = default(TKey);
				this.value = default(TValue);
				return false;
			}

			// Token: 0x170003DB RID: 987
			// (get) Token: 0x06001324 RID: 4900 RVA: 0x00040644 File Offset: 0x0003F644
			DictionaryEntry IDictionaryEnumerator.Entry
			{
				get
				{
					if (this.index == 0 || this.index == this._sortedList.Count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return new DictionaryEntry(this.key, this.value);
				}
			}

			// Token: 0x170003DC RID: 988
			// (get) Token: 0x06001325 RID: 4901 RVA: 0x00040690 File Offset: 0x0003F690
			public KeyValuePair<TKey, TValue> Current
			{
				get
				{
					return new KeyValuePair<TKey, TValue>(this.key, this.value);
				}
			}

			// Token: 0x170003DD RID: 989
			// (get) Token: 0x06001326 RID: 4902 RVA: 0x000406A4 File Offset: 0x0003F6A4
			object IEnumerator.Current
			{
				get
				{
					if (this.index == 0 || this.index == this._sortedList.Count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					if (this.getEnumeratorRetType == 2)
					{
						return new DictionaryEntry(this.key, this.value);
					}
					return new KeyValuePair<TKey, TValue>(this.key, this.value);
				}
			}

			// Token: 0x170003DE RID: 990
			// (get) Token: 0x06001327 RID: 4903 RVA: 0x00040715 File Offset: 0x0003F715
			object IDictionaryEnumerator.Value
			{
				get
				{
					if (this.index == 0 || this.index == this._sortedList.Count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return this.value;
				}
			}

			// Token: 0x06001328 RID: 4904 RVA: 0x00040746 File Offset: 0x0003F746
			void IEnumerator.Reset()
			{
				if (this.version != this._sortedList.version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				this.index = 0;
				this.key = default(TKey);
				this.value = default(TValue);
			}

			// Token: 0x040010FB RID: 4347
			internal const int KeyValuePair = 1;

			// Token: 0x040010FC RID: 4348
			internal const int DictEntry = 2;

			// Token: 0x040010FD RID: 4349
			private SortedList<TKey, TValue> _sortedList;

			// Token: 0x040010FE RID: 4350
			private TKey key;

			// Token: 0x040010FF RID: 4351
			private TValue value;

			// Token: 0x04001100 RID: 4352
			private int index;

			// Token: 0x04001101 RID: 4353
			private int version;

			// Token: 0x04001102 RID: 4354
			private int getEnumeratorRetType;
		}

		// Token: 0x02000236 RID: 566
		[Serializable]
		private sealed class SortedListKeyEnumerator : IEnumerator<TKey>, IDisposable, IEnumerator
		{
			// Token: 0x06001329 RID: 4905 RVA: 0x00040781 File Offset: 0x0003F781
			internal SortedListKeyEnumerator(SortedList<TKey, TValue> sortedList)
			{
				this._sortedList = sortedList;
				this.version = sortedList.version;
			}

			// Token: 0x0600132A RID: 4906 RVA: 0x0004079C File Offset: 0x0003F79C
			public void Dispose()
			{
				this.index = 0;
				this.currentKey = default(TKey);
			}

			// Token: 0x0600132B RID: 4907 RVA: 0x000407B4 File Offset: 0x0003F7B4
			public bool MoveNext()
			{
				if (this.version != this._sortedList.version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				if (this.index < this._sortedList.Count)
				{
					this.currentKey = this._sortedList.keys[this.index];
					this.index++;
					return true;
				}
				this.index = this._sortedList.Count + 1;
				this.currentKey = default(TKey);
				return false;
			}

			// Token: 0x170003DF RID: 991
			// (get) Token: 0x0600132C RID: 4908 RVA: 0x0004083A File Offset: 0x0003F83A
			public TKey Current
			{
				get
				{
					return this.currentKey;
				}
			}

			// Token: 0x170003E0 RID: 992
			// (get) Token: 0x0600132D RID: 4909 RVA: 0x00040842 File Offset: 0x0003F842
			object IEnumerator.Current
			{
				get
				{
					if (this.index == 0 || this.index == this._sortedList.Count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return this.currentKey;
				}
			}

			// Token: 0x0600132E RID: 4910 RVA: 0x00040873 File Offset: 0x0003F873
			void IEnumerator.Reset()
			{
				if (this.version != this._sortedList.version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				this.index = 0;
				this.currentKey = default(TKey);
			}

			// Token: 0x04001103 RID: 4355
			private SortedList<TKey, TValue> _sortedList;

			// Token: 0x04001104 RID: 4356
			private int index;

			// Token: 0x04001105 RID: 4357
			private int version;

			// Token: 0x04001106 RID: 4358
			private TKey currentKey;
		}

		// Token: 0x02000237 RID: 567
		[Serializable]
		private sealed class SortedListValueEnumerator : IEnumerator<TValue>, IDisposable, IEnumerator
		{
			// Token: 0x0600132F RID: 4911 RVA: 0x000408A2 File Offset: 0x0003F8A2
			internal SortedListValueEnumerator(SortedList<TKey, TValue> sortedList)
			{
				this._sortedList = sortedList;
				this.version = sortedList.version;
			}

			// Token: 0x06001330 RID: 4912 RVA: 0x000408BD File Offset: 0x0003F8BD
			public void Dispose()
			{
				this.index = 0;
				this.currentValue = default(TValue);
			}

			// Token: 0x06001331 RID: 4913 RVA: 0x000408D4 File Offset: 0x0003F8D4
			public bool MoveNext()
			{
				if (this.version != this._sortedList.version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				if (this.index < this._sortedList.Count)
				{
					this.currentValue = this._sortedList.values[this.index];
					this.index++;
					return true;
				}
				this.index = this._sortedList.Count + 1;
				this.currentValue = default(TValue);
				return false;
			}

			// Token: 0x170003E1 RID: 993
			// (get) Token: 0x06001332 RID: 4914 RVA: 0x0004095A File Offset: 0x0003F95A
			public TValue Current
			{
				get
				{
					return this.currentValue;
				}
			}

			// Token: 0x170003E2 RID: 994
			// (get) Token: 0x06001333 RID: 4915 RVA: 0x00040962 File Offset: 0x0003F962
			object IEnumerator.Current
			{
				get
				{
					if (this.index == 0 || this.index == this._sortedList.Count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return this.currentValue;
				}
			}

			// Token: 0x06001334 RID: 4916 RVA: 0x00040993 File Offset: 0x0003F993
			void IEnumerator.Reset()
			{
				if (this.version != this._sortedList.version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				this.index = 0;
				this.currentValue = default(TValue);
			}

			// Token: 0x04001107 RID: 4359
			private SortedList<TKey, TValue> _sortedList;

			// Token: 0x04001108 RID: 4360
			private int index;

			// Token: 0x04001109 RID: 4361
			private int version;

			// Token: 0x0400110A RID: 4362
			private TValue currentValue;
		}

		// Token: 0x02000238 RID: 568
		[DebuggerDisplay("Count = {Count}")]
		[DebuggerTypeProxy(typeof(System_DictionaryKeyCollectionDebugView<, >))]
		[Serializable]
		private sealed class KeyList : IList<TKey>, ICollection<TKey>, IEnumerable<TKey>, ICollection, IEnumerable
		{
			// Token: 0x06001335 RID: 4917 RVA: 0x000409C2 File Offset: 0x0003F9C2
			internal KeyList(SortedList<TKey, TValue> dictionary)
			{
				this._dict = dictionary;
			}

			// Token: 0x170003E3 RID: 995
			// (get) Token: 0x06001336 RID: 4918 RVA: 0x000409D1 File Offset: 0x0003F9D1
			public int Count
			{
				get
				{
					return this._dict._size;
				}
			}

			// Token: 0x170003E4 RID: 996
			// (get) Token: 0x06001337 RID: 4919 RVA: 0x000409DE File Offset: 0x0003F9DE
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170003E5 RID: 997
			// (get) Token: 0x06001338 RID: 4920 RVA: 0x000409E1 File Offset: 0x0003F9E1
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170003E6 RID: 998
			// (get) Token: 0x06001339 RID: 4921 RVA: 0x000409E4 File Offset: 0x0003F9E4
			object ICollection.SyncRoot
			{
				get
				{
					return ((ICollection)this._dict).SyncRoot;
				}
			}

			// Token: 0x0600133A RID: 4922 RVA: 0x000409F1 File Offset: 0x0003F9F1
			public void Add(TKey key)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_SortedListNestedWrite);
			}

			// Token: 0x0600133B RID: 4923 RVA: 0x000409FA File Offset: 0x0003F9FA
			public void Clear()
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_SortedListNestedWrite);
			}

			// Token: 0x0600133C RID: 4924 RVA: 0x00040A03 File Offset: 0x0003FA03
			public bool Contains(TKey key)
			{
				return this._dict.ContainsKey(key);
			}

			// Token: 0x0600133D RID: 4925 RVA: 0x00040A11 File Offset: 0x0003FA11
			public void CopyTo(TKey[] array, int arrayIndex)
			{
				Array.Copy(this._dict.keys, 0, array, arrayIndex, this._dict.Count);
			}

			// Token: 0x0600133E RID: 4926 RVA: 0x00040A34 File Offset: 0x0003FA34
			void ICollection.CopyTo(Array array, int arrayIndex)
			{
				if (array != null && array.Rank != 1)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
				}
				try
				{
					Array.Copy(this._dict.keys, 0, array, arrayIndex, this._dict.Count);
				}
				catch (ArrayTypeMismatchException)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
				}
			}

			// Token: 0x0600133F RID: 4927 RVA: 0x00040A90 File Offset: 0x0003FA90
			public void Insert(int index, TKey value)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_SortedListNestedWrite);
			}

			// Token: 0x170003E7 RID: 999
			public TKey this[int index]
			{
				get
				{
					return this._dict.GetKey(index);
				}
				set
				{
					ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
				}
			}

			// Token: 0x06001342 RID: 4930 RVA: 0x00040AB0 File Offset: 0x0003FAB0
			public IEnumerator<TKey> GetEnumerator()
			{
				return new SortedList<TKey, TValue>.SortedListKeyEnumerator(this._dict);
			}

			// Token: 0x06001343 RID: 4931 RVA: 0x00040ABD File Offset: 0x0003FABD
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new SortedList<TKey, TValue>.SortedListKeyEnumerator(this._dict);
			}

			// Token: 0x06001344 RID: 4932 RVA: 0x00040ACC File Offset: 0x0003FACC
			public int IndexOf(TKey key)
			{
				if (key == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
				}
				int num = Array.BinarySearch<TKey>(this._dict.keys, 0, this._dict.Count, key, this._dict.comparer);
				if (num >= 0)
				{
					return num;
				}
				return -1;
			}

			// Token: 0x06001345 RID: 4933 RVA: 0x00040B17 File Offset: 0x0003FB17
			public bool Remove(TKey key)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_SortedListNestedWrite);
				return false;
			}

			// Token: 0x06001346 RID: 4934 RVA: 0x00040B21 File Offset: 0x0003FB21
			public void RemoveAt(int index)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_SortedListNestedWrite);
			}

			// Token: 0x0400110B RID: 4363
			private SortedList<TKey, TValue> _dict;
		}

		// Token: 0x02000239 RID: 569
		[DebuggerDisplay("Count = {Count}")]
		[DebuggerTypeProxy(typeof(System_DictionaryValueCollectionDebugView<, >))]
		[Serializable]
		private sealed class ValueList : IList<TValue>, ICollection<TValue>, IEnumerable<TValue>, ICollection, IEnumerable
		{
			// Token: 0x06001347 RID: 4935 RVA: 0x00040B2A File Offset: 0x0003FB2A
			internal ValueList(SortedList<TKey, TValue> dictionary)
			{
				this._dict = dictionary;
			}

			// Token: 0x170003E8 RID: 1000
			// (get) Token: 0x06001348 RID: 4936 RVA: 0x00040B39 File Offset: 0x0003FB39
			public int Count
			{
				get
				{
					return this._dict._size;
				}
			}

			// Token: 0x170003E9 RID: 1001
			// (get) Token: 0x06001349 RID: 4937 RVA: 0x00040B46 File Offset: 0x0003FB46
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170003EA RID: 1002
			// (get) Token: 0x0600134A RID: 4938 RVA: 0x00040B49 File Offset: 0x0003FB49
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170003EB RID: 1003
			// (get) Token: 0x0600134B RID: 4939 RVA: 0x00040B4C File Offset: 0x0003FB4C
			object ICollection.SyncRoot
			{
				get
				{
					return ((ICollection)this._dict).SyncRoot;
				}
			}

			// Token: 0x0600134C RID: 4940 RVA: 0x00040B59 File Offset: 0x0003FB59
			public void Add(TValue key)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_SortedListNestedWrite);
			}

			// Token: 0x0600134D RID: 4941 RVA: 0x00040B62 File Offset: 0x0003FB62
			public void Clear()
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_SortedListNestedWrite);
			}

			// Token: 0x0600134E RID: 4942 RVA: 0x00040B6B File Offset: 0x0003FB6B
			public bool Contains(TValue value)
			{
				return this._dict.ContainsValue(value);
			}

			// Token: 0x0600134F RID: 4943 RVA: 0x00040B79 File Offset: 0x0003FB79
			public void CopyTo(TValue[] array, int arrayIndex)
			{
				Array.Copy(this._dict.values, 0, array, arrayIndex, this._dict.Count);
			}

			// Token: 0x06001350 RID: 4944 RVA: 0x00040B9C File Offset: 0x0003FB9C
			void ICollection.CopyTo(Array array, int arrayIndex)
			{
				if (array != null && array.Rank != 1)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
				}
				try
				{
					Array.Copy(this._dict.values, 0, array, arrayIndex, this._dict.Count);
				}
				catch (ArrayTypeMismatchException)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
				}
			}

			// Token: 0x06001351 RID: 4945 RVA: 0x00040BF8 File Offset: 0x0003FBF8
			public void Insert(int index, TValue value)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_SortedListNestedWrite);
			}

			// Token: 0x170003EC RID: 1004
			public TValue this[int index]
			{
				get
				{
					return this._dict.GetByIndex(index);
				}
				set
				{
					ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_SortedListNestedWrite);
				}
			}

			// Token: 0x06001354 RID: 4948 RVA: 0x00040C18 File Offset: 0x0003FC18
			public IEnumerator<TValue> GetEnumerator()
			{
				return new SortedList<TKey, TValue>.SortedListValueEnumerator(this._dict);
			}

			// Token: 0x06001355 RID: 4949 RVA: 0x00040C25 File Offset: 0x0003FC25
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new SortedList<TKey, TValue>.SortedListValueEnumerator(this._dict);
			}

			// Token: 0x06001356 RID: 4950 RVA: 0x00040C32 File Offset: 0x0003FC32
			public int IndexOf(TValue value)
			{
				return Array.IndexOf<TValue>(this._dict.values, value, 0, this._dict.Count);
			}

			// Token: 0x06001357 RID: 4951 RVA: 0x00040C51 File Offset: 0x0003FC51
			public bool Remove(TValue value)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_SortedListNestedWrite);
				return false;
			}

			// Token: 0x06001358 RID: 4952 RVA: 0x00040C5B File Offset: 0x0003FC5B
			public void RemoveAt(int index)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_SortedListNestedWrite);
			}

			// Token: 0x0400110C RID: 4364
			private SortedList<TKey, TValue> _dict;
		}
	}
}
