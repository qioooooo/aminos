using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System.Collections.Generic
{
	// Token: 0x02000276 RID: 630
	[DebuggerDisplay("Count = {Count}")]
	[ComVisible(false)]
	[DebuggerTypeProxy(typeof(Mscorlib_DictionaryDebugView<, >))]
	[Serializable]
	public class Dictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary, ICollection, IEnumerable, ISerializable, IDeserializationCallback
	{
		// Token: 0x06001940 RID: 6464 RVA: 0x000421AD File Offset: 0x000411AD
		public Dictionary()
			: this(0, null)
		{
		}

		// Token: 0x06001941 RID: 6465 RVA: 0x000421B7 File Offset: 0x000411B7
		public Dictionary(int capacity)
			: this(capacity, null)
		{
		}

		// Token: 0x06001942 RID: 6466 RVA: 0x000421C1 File Offset: 0x000411C1
		public Dictionary(IEqualityComparer<TKey> comparer)
			: this(0, comparer)
		{
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x000421CB File Offset: 0x000411CB
		public Dictionary(int capacity, IEqualityComparer<TKey> comparer)
		{
			if (capacity < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity);
			}
			if (capacity > 0)
			{
				this.Initialize(capacity);
			}
			if (comparer == null)
			{
				comparer = EqualityComparer<TKey>.Default;
			}
			this.comparer = comparer;
		}

		// Token: 0x06001944 RID: 6468 RVA: 0x000421FA File Offset: 0x000411FA
		public Dictionary(IDictionary<TKey, TValue> dictionary)
			: this(dictionary, null)
		{
		}

		// Token: 0x06001945 RID: 6469 RVA: 0x00042204 File Offset: 0x00041204
		public Dictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
			: this((dictionary != null) ? dictionary.Count : 0, comparer)
		{
			if (dictionary == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
			}
			foreach (KeyValuePair<TKey, TValue> keyValuePair in dictionary)
			{
				this.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x06001946 RID: 6470 RVA: 0x00042278 File Offset: 0x00041278
		protected Dictionary(SerializationInfo info, StreamingContext context)
		{
			this.m_siInfo = info;
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06001947 RID: 6471 RVA: 0x00042287 File Offset: 0x00041287
		public IEqualityComparer<TKey> Comparer
		{
			get
			{
				return this.comparer;
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06001948 RID: 6472 RVA: 0x0004228F File Offset: 0x0004128F
		public int Count
		{
			get
			{
				return this.count - this.freeCount;
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06001949 RID: 6473 RVA: 0x0004229E File Offset: 0x0004129E
		public Dictionary<TKey, TValue>.KeyCollection Keys
		{
			get
			{
				if (this.keys == null)
				{
					this.keys = new Dictionary<TKey, TValue>.KeyCollection(this);
				}
				return this.keys;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x0600194A RID: 6474 RVA: 0x000422BA File Offset: 0x000412BA
		ICollection<TKey> IDictionary<TKey, TValue>.Keys
		{
			get
			{
				if (this.keys == null)
				{
					this.keys = new Dictionary<TKey, TValue>.KeyCollection(this);
				}
				return this.keys;
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x0600194B RID: 6475 RVA: 0x000422D6 File Offset: 0x000412D6
		public Dictionary<TKey, TValue>.ValueCollection Values
		{
			get
			{
				if (this.values == null)
				{
					this.values = new Dictionary<TKey, TValue>.ValueCollection(this);
				}
				return this.values;
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x0600194C RID: 6476 RVA: 0x000422F2 File Offset: 0x000412F2
		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			get
			{
				if (this.values == null)
				{
					this.values = new Dictionary<TKey, TValue>.ValueCollection(this);
				}
				return this.values;
			}
		}

		// Token: 0x170003DA RID: 986
		public TValue this[TKey key]
		{
			get
			{
				int num = this.FindEntry(key);
				if (num >= 0)
				{
					return this.entries[num].value;
				}
				ThrowHelper.ThrowKeyNotFoundException();
				return default(TValue);
			}
			set
			{
				this.Insert(key, value, false);
			}
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x00042354 File Offset: 0x00041354
		public void Add(TKey key, TValue value)
		{
			this.Insert(key, value, true);
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x0004235F File Offset: 0x0004135F
		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
		{
			this.Add(keyValuePair.Key, keyValuePair.Value);
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x00042378 File Offset: 0x00041378
		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
		{
			int num = this.FindEntry(keyValuePair.Key);
			return num >= 0 && EqualityComparer<TValue>.Default.Equals(this.entries[num].value, keyValuePair.Value);
		}

		// Token: 0x06001952 RID: 6482 RVA: 0x000423C0 File Offset: 0x000413C0
		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
		{
			int num = this.FindEntry(keyValuePair.Key);
			if (num >= 0 && EqualityComparer<TValue>.Default.Equals(this.entries[num].value, keyValuePair.Value))
			{
				this.Remove(keyValuePair.Key);
				return true;
			}
			return false;
		}

		// Token: 0x06001953 RID: 6483 RVA: 0x00042414 File Offset: 0x00041414
		public void Clear()
		{
			if (this.count > 0)
			{
				for (int i = 0; i < this.buckets.Length; i++)
				{
					this.buckets[i] = -1;
				}
				Array.Clear(this.entries, 0, this.count);
				this.freeList = -1;
				this.count = 0;
				this.freeCount = 0;
				this.version++;
			}
		}

		// Token: 0x06001954 RID: 6484 RVA: 0x0004247B File Offset: 0x0004147B
		public bool ContainsKey(TKey key)
		{
			return this.FindEntry(key) >= 0;
		}

		// Token: 0x06001955 RID: 6485 RVA: 0x0004248C File Offset: 0x0004148C
		public bool ContainsValue(TValue value)
		{
			if (value == null)
			{
				for (int i = 0; i < this.count; i++)
				{
					if (this.entries[i].hashCode >= 0 && this.entries[i].value == null)
					{
						return true;
					}
				}
			}
			else
			{
				EqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
				for (int j = 0; j < this.count; j++)
				{
					if (this.entries[j].hashCode >= 0 && @default.Equals(this.entries[j].value, value))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001956 RID: 6486 RVA: 0x0004252C File Offset: 0x0004152C
		private void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
		{
			if (array == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
			}
			if (index < 0 || index > array.Length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (array.Length - index < this.Count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			int num = this.count;
			Dictionary<TKey, TValue>.Entry[] array2 = this.entries;
			for (int i = 0; i < num; i++)
			{
				if (array2[i].hashCode >= 0)
				{
					array[index++] = new KeyValuePair<TKey, TValue>(array2[i].key, array2[i].value);
				}
			}
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x000425BE File Offset: 0x000415BE
		public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
		{
			return new Dictionary<TKey, TValue>.Enumerator(this, 2);
		}

		// Token: 0x06001958 RID: 6488 RVA: 0x000425C7 File Offset: 0x000415C7
		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return new Dictionary<TKey, TValue>.Enumerator(this, 2);
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x000425D8 File Offset: 0x000415D8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.info);
			}
			info.AddValue("Version", this.version);
			info.AddValue("Comparer", this.comparer, typeof(IEqualityComparer<TKey>));
			info.AddValue("HashSize", (this.buckets == null) ? 0 : this.buckets.Length);
			if (this.buckets != null)
			{
				KeyValuePair<TKey, TValue>[] array = new KeyValuePair<TKey, TValue>[this.Count];
				this.CopyTo(array, 0);
				info.AddValue("KeyValuePairs", array, typeof(KeyValuePair<TKey, TValue>[]));
			}
		}

		// Token: 0x0600195A RID: 6490 RVA: 0x0004266C File Offset: 0x0004166C
		private int FindEntry(TKey key)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			if (this.buckets != null)
			{
				int num = this.comparer.GetHashCode(key) & int.MaxValue;
				for (int i = this.buckets[num % this.buckets.Length]; i >= 0; i = this.entries[i].next)
				{
					if (this.entries[i].hashCode == num && this.comparer.Equals(this.entries[i].key, key))
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x00042704 File Offset: 0x00041704
		private void Initialize(int capacity)
		{
			int prime = HashHelpers.GetPrime(capacity);
			this.buckets = new int[prime];
			for (int i = 0; i < this.buckets.Length; i++)
			{
				this.buckets[i] = -1;
			}
			this.entries = new Dictionary<TKey, TValue>.Entry[prime];
			this.freeList = -1;
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x00042754 File Offset: 0x00041754
		private void Insert(TKey key, TValue value, bool add)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			if (this.buckets == null)
			{
				this.Initialize(0);
			}
			int num = this.comparer.GetHashCode(key) & int.MaxValue;
			int num2 = num % this.buckets.Length;
			for (int i = this.buckets[num2]; i >= 0; i = this.entries[i].next)
			{
				if (this.entries[i].hashCode == num && this.comparer.Equals(this.entries[i].key, key))
				{
					if (add)
					{
						ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_AddingDuplicate);
					}
					this.entries[i].value = value;
					this.version++;
					return;
				}
			}
			int num3;
			if (this.freeCount > 0)
			{
				num3 = this.freeList;
				this.freeList = this.entries[num3].next;
				this.freeCount--;
			}
			else
			{
				if (this.count == this.entries.Length)
				{
					this.Resize();
					num2 = num % this.buckets.Length;
				}
				num3 = this.count;
				this.count++;
			}
			this.entries[num3].hashCode = num;
			this.entries[num3].next = this.buckets[num2];
			this.entries[num3].key = key;
			this.entries[num3].value = value;
			this.buckets[num2] = num3;
			this.version++;
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x000428F0 File Offset: 0x000418F0
		public virtual void OnDeserialization(object sender)
		{
			if (this.m_siInfo == null)
			{
				return;
			}
			int @int = this.m_siInfo.GetInt32("Version");
			int int2 = this.m_siInfo.GetInt32("HashSize");
			this.comparer = (IEqualityComparer<TKey>)this.m_siInfo.GetValue("Comparer", typeof(IEqualityComparer<TKey>));
			if (int2 != 0)
			{
				this.buckets = new int[int2];
				for (int i = 0; i < this.buckets.Length; i++)
				{
					this.buckets[i] = -1;
				}
				this.entries = new Dictionary<TKey, TValue>.Entry[int2];
				this.freeList = -1;
				KeyValuePair<TKey, TValue>[] array = (KeyValuePair<TKey, TValue>[])this.m_siInfo.GetValue("KeyValuePairs", typeof(KeyValuePair<TKey, TValue>[]));
				if (array == null)
				{
					ThrowHelper.ThrowSerializationException(ExceptionResource.Serialization_MissingKeyValuePairs);
				}
				for (int j = 0; j < array.Length; j++)
				{
					if (array[j].Key == null)
					{
						ThrowHelper.ThrowSerializationException(ExceptionResource.Serialization_NullKey);
					}
					this.Insert(array[j].Key, array[j].Value, true);
				}
			}
			else
			{
				this.buckets = null;
			}
			this.version = @int;
			this.m_siInfo = null;
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x00042A20 File Offset: 0x00041A20
		private void Resize()
		{
			int prime = HashHelpers.GetPrime(this.count * 2);
			int[] array = new int[prime];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = -1;
			}
			Dictionary<TKey, TValue>.Entry[] array2 = new Dictionary<TKey, TValue>.Entry[prime];
			Array.Copy(this.entries, 0, array2, 0, this.count);
			for (int j = 0; j < this.count; j++)
			{
				int num = array2[j].hashCode % prime;
				array2[j].next = array[num];
				array[num] = j;
			}
			this.buckets = array;
			this.entries = array2;
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x00042ABC File Offset: 0x00041ABC
		public bool Remove(TKey key)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			if (this.buckets != null)
			{
				int num = this.comparer.GetHashCode(key) & int.MaxValue;
				int num2 = num % this.buckets.Length;
				int num3 = -1;
				for (int i = this.buckets[num2]; i >= 0; i = this.entries[i].next)
				{
					if (this.entries[i].hashCode == num && this.comparer.Equals(this.entries[i].key, key))
					{
						if (num3 < 0)
						{
							this.buckets[num2] = this.entries[i].next;
						}
						else
						{
							this.entries[num3].next = this.entries[i].next;
						}
						this.entries[i].hashCode = -1;
						this.entries[i].next = this.freeList;
						this.entries[i].key = default(TKey);
						this.entries[i].value = default(TValue);
						this.freeList = i;
						this.freeCount++;
						this.version++;
						return true;
					}
					num3 = i;
				}
			}
			return false;
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x00042C24 File Offset: 0x00041C24
		public bool TryGetValue(TKey key, out TValue value)
		{
			int num = this.FindEntry(key);
			if (num >= 0)
			{
				value = this.entries[num].value;
				return true;
			}
			value = default(TValue);
			return false;
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06001961 RID: 6497 RVA: 0x00042C5E File Offset: 0x00041C5E
		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x00042C61 File Offset: 0x00041C61
		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x00042C6C File Offset: 0x00041C6C
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
			if (index < 0 || index > array.Length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (array.Length - index < this.Count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			KeyValuePair<TKey, TValue>[] array2 = array as KeyValuePair<TKey, TValue>[];
			if (array2 != null)
			{
				this.CopyTo(array2, index);
				return;
			}
			if (array is DictionaryEntry[])
			{
				DictionaryEntry[] array3 = array as DictionaryEntry[];
				Dictionary<TKey, TValue>.Entry[] array4 = this.entries;
				for (int i = 0; i < this.count; i++)
				{
					if (array4[i].hashCode >= 0)
					{
						array3[index++] = new DictionaryEntry(array4[i].key, array4[i].value);
					}
				}
				return;
			}
			object[] array5 = array as object[];
			if (array5 == null)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
			}
			try
			{
				int num = this.count;
				Dictionary<TKey, TValue>.Entry[] array6 = this.entries;
				for (int j = 0; j < num; j++)
				{
					if (array6[j].hashCode >= 0)
					{
						array5[index++] = new KeyValuePair<TKey, TValue>(array6[j].key, array6[j].value);
					}
				}
			}
			catch (ArrayTypeMismatchException)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
			}
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x00042DE0 File Offset: 0x00041DE0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Dictionary<TKey, TValue>.Enumerator(this, 2);
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06001965 RID: 6501 RVA: 0x00042DEE File Offset: 0x00041DEE
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06001966 RID: 6502 RVA: 0x00042DF1 File Offset: 0x00041DF1
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

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06001967 RID: 6503 RVA: 0x00042E13 File Offset: 0x00041E13
		bool IDictionary.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06001968 RID: 6504 RVA: 0x00042E16 File Offset: 0x00041E16
		bool IDictionary.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06001969 RID: 6505 RVA: 0x00042E19 File Offset: 0x00041E19
		ICollection IDictionary.Keys
		{
			get
			{
				return this.Keys;
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x0600196A RID: 6506 RVA: 0x00042E21 File Offset: 0x00041E21
		ICollection IDictionary.Values
		{
			get
			{
				return this.Values;
			}
		}

		// Token: 0x170003E2 RID: 994
		object IDictionary.this[object key]
		{
			get
			{
				if (Dictionary<TKey, TValue>.IsCompatibleKey(key))
				{
					int num = this.FindEntry((TKey)((object)key));
					if (num >= 0)
					{
						return this.entries[num].value;
					}
				}
				return null;
			}
			set
			{
				Dictionary<TKey, TValue>.VerifyKey(key);
				Dictionary<TKey, TValue>.VerifyValueType(value);
				this[(TKey)((object)key)] = (TValue)((object)value);
			}
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x00042E8A File Offset: 0x00041E8A
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

		// Token: 0x0600196E RID: 6510 RVA: 0x00042EAD File Offset: 0x00041EAD
		private static bool IsCompatibleKey(object key)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			return key is TKey;
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x00042EC1 File Offset: 0x00041EC1
		private static void VerifyValueType(object value)
		{
			if (value is TValue || (value == null && !typeof(TValue).IsValueType))
			{
				return;
			}
			ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(TValue));
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x00042EF0 File Offset: 0x00041EF0
		void IDictionary.Add(object key, object value)
		{
			Dictionary<TKey, TValue>.VerifyKey(key);
			Dictionary<TKey, TValue>.VerifyValueType(value);
			this.Add((TKey)((object)key), (TValue)((object)value));
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x00042F10 File Offset: 0x00041F10
		bool IDictionary.Contains(object key)
		{
			return Dictionary<TKey, TValue>.IsCompatibleKey(key) && this.ContainsKey((TKey)((object)key));
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x00042F28 File Offset: 0x00041F28
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new Dictionary<TKey, TValue>.Enumerator(this, 1);
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x00042F36 File Offset: 0x00041F36
		void IDictionary.Remove(object key)
		{
			if (Dictionary<TKey, TValue>.IsCompatibleKey(key))
			{
				this.Remove((TKey)((object)key));
			}
		}

		// Token: 0x040009B7 RID: 2487
		private const string VersionName = "Version";

		// Token: 0x040009B8 RID: 2488
		private const string HashSizeName = "HashSize";

		// Token: 0x040009B9 RID: 2489
		private const string KeyValuePairsName = "KeyValuePairs";

		// Token: 0x040009BA RID: 2490
		private const string ComparerName = "Comparer";

		// Token: 0x040009BB RID: 2491
		private int[] buckets;

		// Token: 0x040009BC RID: 2492
		private Dictionary<TKey, TValue>.Entry[] entries;

		// Token: 0x040009BD RID: 2493
		private int count;

		// Token: 0x040009BE RID: 2494
		private int version;

		// Token: 0x040009BF RID: 2495
		private int freeList;

		// Token: 0x040009C0 RID: 2496
		private int freeCount;

		// Token: 0x040009C1 RID: 2497
		private IEqualityComparer<TKey> comparer;

		// Token: 0x040009C2 RID: 2498
		private Dictionary<TKey, TValue>.KeyCollection keys;

		// Token: 0x040009C3 RID: 2499
		private Dictionary<TKey, TValue>.ValueCollection values;

		// Token: 0x040009C4 RID: 2500
		private object _syncRoot;

		// Token: 0x040009C5 RID: 2501
		private SerializationInfo m_siInfo;

		// Token: 0x02000277 RID: 631
		private struct Entry
		{
			// Token: 0x040009C6 RID: 2502
			public int hashCode;

			// Token: 0x040009C7 RID: 2503
			public int next;

			// Token: 0x040009C8 RID: 2504
			public TKey key;

			// Token: 0x040009C9 RID: 2505
			public TValue value;
		}

		// Token: 0x02000278 RID: 632
		[Serializable]
		public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDisposable, IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x06001974 RID: 6516 RVA: 0x00042F4D File Offset: 0x00041F4D
			internal Enumerator(Dictionary<TKey, TValue> dictionary, int getEnumeratorRetType)
			{
				this.dictionary = dictionary;
				this.version = dictionary.version;
				this.index = 0;
				this.getEnumeratorRetType = getEnumeratorRetType;
				this.current = default(KeyValuePair<TKey, TValue>);
			}

			// Token: 0x06001975 RID: 6517 RVA: 0x00042F7C File Offset: 0x00041F7C
			public bool MoveNext()
			{
				if (this.version != this.dictionary.version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				while (this.index < this.dictionary.count)
				{
					if (this.dictionary.entries[this.index].hashCode >= 0)
					{
						this.current = new KeyValuePair<TKey, TValue>(this.dictionary.entries[this.index].key, this.dictionary.entries[this.index].value);
						this.index++;
						return true;
					}
					this.index++;
				}
				this.index = this.dictionary.count + 1;
				this.current = default(KeyValuePair<TKey, TValue>);
				return false;
			}

			// Token: 0x170003E3 RID: 995
			// (get) Token: 0x06001976 RID: 6518 RVA: 0x0004305B File Offset: 0x0004205B
			public KeyValuePair<TKey, TValue> Current
			{
				get
				{
					return this.current;
				}
			}

			// Token: 0x06001977 RID: 6519 RVA: 0x00043063 File Offset: 0x00042063
			public void Dispose()
			{
			}

			// Token: 0x170003E4 RID: 996
			// (get) Token: 0x06001978 RID: 6520 RVA: 0x00043068 File Offset: 0x00042068
			object IEnumerator.Current
			{
				get
				{
					if (this.index == 0 || this.index == this.dictionary.count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					if (this.getEnumeratorRetType == 1)
					{
						return new DictionaryEntry(this.current.Key, this.current.Value);
					}
					return new KeyValuePair<TKey, TValue>(this.current.Key, this.current.Value);
				}
			}

			// Token: 0x06001979 RID: 6521 RVA: 0x000430ED File Offset: 0x000420ED
			void IEnumerator.Reset()
			{
				if (this.version != this.dictionary.version)
				{
					ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
				}
				this.index = 0;
				this.current = default(KeyValuePair<TKey, TValue>);
			}

			// Token: 0x170003E5 RID: 997
			// (get) Token: 0x0600197A RID: 6522 RVA: 0x0004311C File Offset: 0x0004211C
			DictionaryEntry IDictionaryEnumerator.Entry
			{
				get
				{
					if (this.index == 0 || this.index == this.dictionary.count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return new DictionaryEntry(this.current.Key, this.current.Value);
				}
			}

			// Token: 0x170003E6 RID: 998
			// (get) Token: 0x0600197B RID: 6523 RVA: 0x00043172 File Offset: 0x00042172
			object IDictionaryEnumerator.Key
			{
				get
				{
					if (this.index == 0 || this.index == this.dictionary.count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return this.current.Key;
				}
			}

			// Token: 0x170003E7 RID: 999
			// (get) Token: 0x0600197C RID: 6524 RVA: 0x000431A8 File Offset: 0x000421A8
			object IDictionaryEnumerator.Value
			{
				get
				{
					if (this.index == 0 || this.index == this.dictionary.count + 1)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					return this.current.Value;
				}
			}

			// Token: 0x040009CA RID: 2506
			internal const int DictEntry = 1;

			// Token: 0x040009CB RID: 2507
			internal const int KeyValuePair = 2;

			// Token: 0x040009CC RID: 2508
			private Dictionary<TKey, TValue> dictionary;

			// Token: 0x040009CD RID: 2509
			private int version;

			// Token: 0x040009CE RID: 2510
			private int index;

			// Token: 0x040009CF RID: 2511
			private KeyValuePair<TKey, TValue> current;

			// Token: 0x040009D0 RID: 2512
			private int getEnumeratorRetType;
		}

		// Token: 0x02000279 RID: 633
		[DebuggerDisplay("Count = {Count}")]
		[DebuggerTypeProxy(typeof(Mscorlib_DictionaryKeyCollectionDebugView<, >))]
		[Serializable]
		public sealed class KeyCollection : ICollection<TKey>, IEnumerable<TKey>, ICollection, IEnumerable
		{
			// Token: 0x0600197D RID: 6525 RVA: 0x000431DE File Offset: 0x000421DE
			public KeyCollection(Dictionary<TKey, TValue> dictionary)
			{
				if (dictionary == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
				}
				this.dictionary = dictionary;
			}

			// Token: 0x0600197E RID: 6526 RVA: 0x000431F6 File Offset: 0x000421F6
			public Dictionary<TKey, TValue>.KeyCollection.Enumerator GetEnumerator()
			{
				return new Dictionary<TKey, TValue>.KeyCollection.Enumerator(this.dictionary);
			}

			// Token: 0x0600197F RID: 6527 RVA: 0x00043204 File Offset: 0x00042204
			public void CopyTo(TKey[] array, int index)
			{
				if (array == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
				}
				if (index < 0 || index > array.Length)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
				}
				if (array.Length - index < this.dictionary.Count)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				int count = this.dictionary.count;
				Dictionary<TKey, TValue>.Entry[] entries = this.dictionary.entries;
				for (int i = 0; i < count; i++)
				{
					if (entries[i].hashCode >= 0)
					{
						array[index++] = entries[i].key;
					}
				}
			}

			// Token: 0x170003E8 RID: 1000
			// (get) Token: 0x06001980 RID: 6528 RVA: 0x0004328F File Offset: 0x0004228F
			public int Count
			{
				get
				{
					return this.dictionary.Count;
				}
			}

			// Token: 0x170003E9 RID: 1001
			// (get) Token: 0x06001981 RID: 6529 RVA: 0x0004329C File Offset: 0x0004229C
			bool ICollection<TKey>.IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06001982 RID: 6530 RVA: 0x0004329F File Offset: 0x0004229F
			void ICollection<TKey>.Add(TKey item)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
			}

			// Token: 0x06001983 RID: 6531 RVA: 0x000432A8 File Offset: 0x000422A8
			void ICollection<TKey>.Clear()
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
			}

			// Token: 0x06001984 RID: 6532 RVA: 0x000432B1 File Offset: 0x000422B1
			bool ICollection<TKey>.Contains(TKey item)
			{
				return this.dictionary.ContainsKey(item);
			}

			// Token: 0x06001985 RID: 6533 RVA: 0x000432BF File Offset: 0x000422BF
			bool ICollection<TKey>.Remove(TKey item)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
				return false;
			}

			// Token: 0x06001986 RID: 6534 RVA: 0x000432C9 File Offset: 0x000422C9
			IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
			{
				return new Dictionary<TKey, TValue>.KeyCollection.Enumerator(this.dictionary);
			}

			// Token: 0x06001987 RID: 6535 RVA: 0x000432DB File Offset: 0x000422DB
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new Dictionary<TKey, TValue>.KeyCollection.Enumerator(this.dictionary);
			}

			// Token: 0x06001988 RID: 6536 RVA: 0x000432F0 File Offset: 0x000422F0
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
				if (index < 0 || index > array.Length)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
				}
				if (array.Length - index < this.dictionary.Count)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				TKey[] array2 = array as TKey[];
				if (array2 != null)
				{
					this.CopyTo(array2, index);
					return;
				}
				object[] array3 = array as object[];
				if (array3 == null)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
				}
				int count = this.dictionary.count;
				Dictionary<TKey, TValue>.Entry[] entries = this.dictionary.entries;
				try
				{
					for (int i = 0; i < count; i++)
					{
						if (entries[i].hashCode >= 0)
						{
							array3[index++] = entries[i].key;
						}
					}
				}
				catch (ArrayTypeMismatchException)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
				}
			}

			// Token: 0x170003EA RID: 1002
			// (get) Token: 0x06001989 RID: 6537 RVA: 0x000433E8 File Offset: 0x000423E8
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170003EB RID: 1003
			// (get) Token: 0x0600198A RID: 6538 RVA: 0x000433EB File Offset: 0x000423EB
			object ICollection.SyncRoot
			{
				get
				{
					return ((ICollection)this.dictionary).SyncRoot;
				}
			}

			// Token: 0x040009D1 RID: 2513
			private Dictionary<TKey, TValue> dictionary;

			// Token: 0x0200027A RID: 634
			[Serializable]
			public struct Enumerator : IEnumerator<TKey>, IDisposable, IEnumerator
			{
				// Token: 0x0600198B RID: 6539 RVA: 0x000433F8 File Offset: 0x000423F8
				internal Enumerator(Dictionary<TKey, TValue> dictionary)
				{
					this.dictionary = dictionary;
					this.version = dictionary.version;
					this.index = 0;
					this.currentKey = default(TKey);
				}

				// Token: 0x0600198C RID: 6540 RVA: 0x00043420 File Offset: 0x00042420
				public void Dispose()
				{
				}

				// Token: 0x0600198D RID: 6541 RVA: 0x00043424 File Offset: 0x00042424
				public bool MoveNext()
				{
					if (this.version != this.dictionary.version)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
					}
					while (this.index < this.dictionary.count)
					{
						if (this.dictionary.entries[this.index].hashCode >= 0)
						{
							this.currentKey = this.dictionary.entries[this.index].key;
							this.index++;
							return true;
						}
						this.index++;
					}
					this.index = this.dictionary.count + 1;
					this.currentKey = default(TKey);
					return false;
				}

				// Token: 0x170003EC RID: 1004
				// (get) Token: 0x0600198E RID: 6542 RVA: 0x000434DD File Offset: 0x000424DD
				public TKey Current
				{
					get
					{
						return this.currentKey;
					}
				}

				// Token: 0x170003ED RID: 1005
				// (get) Token: 0x0600198F RID: 6543 RVA: 0x000434E5 File Offset: 0x000424E5
				object IEnumerator.Current
				{
					get
					{
						if (this.index == 0 || this.index == this.dictionary.count + 1)
						{
							ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
						}
						return this.currentKey;
					}
				}

				// Token: 0x06001990 RID: 6544 RVA: 0x00043516 File Offset: 0x00042516
				void IEnumerator.Reset()
				{
					if (this.version != this.dictionary.version)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
					}
					this.index = 0;
					this.currentKey = default(TKey);
				}

				// Token: 0x040009D2 RID: 2514
				private Dictionary<TKey, TValue> dictionary;

				// Token: 0x040009D3 RID: 2515
				private int index;

				// Token: 0x040009D4 RID: 2516
				private int version;

				// Token: 0x040009D5 RID: 2517
				private TKey currentKey;
			}
		}

		// Token: 0x0200027B RID: 635
		[DebuggerTypeProxy(typeof(Mscorlib_DictionaryValueCollectionDebugView<, >))]
		[DebuggerDisplay("Count = {Count}")]
		[Serializable]
		public sealed class ValueCollection : ICollection<TValue>, IEnumerable<TValue>, ICollection, IEnumerable
		{
			// Token: 0x06001991 RID: 6545 RVA: 0x00043545 File Offset: 0x00042545
			public ValueCollection(Dictionary<TKey, TValue> dictionary)
			{
				if (dictionary == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
				}
				this.dictionary = dictionary;
			}

			// Token: 0x06001992 RID: 6546 RVA: 0x0004355D File Offset: 0x0004255D
			public Dictionary<TKey, TValue>.ValueCollection.Enumerator GetEnumerator()
			{
				return new Dictionary<TKey, TValue>.ValueCollection.Enumerator(this.dictionary);
			}

			// Token: 0x06001993 RID: 6547 RVA: 0x0004356C File Offset: 0x0004256C
			public void CopyTo(TValue[] array, int index)
			{
				if (array == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
				}
				if (index < 0 || index > array.Length)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
				}
				if (array.Length - index < this.dictionary.Count)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				int count = this.dictionary.count;
				Dictionary<TKey, TValue>.Entry[] entries = this.dictionary.entries;
				for (int i = 0; i < count; i++)
				{
					if (entries[i].hashCode >= 0)
					{
						array[index++] = entries[i].value;
					}
				}
			}

			// Token: 0x170003EE RID: 1006
			// (get) Token: 0x06001994 RID: 6548 RVA: 0x000435F7 File Offset: 0x000425F7
			public int Count
			{
				get
				{
					return this.dictionary.Count;
				}
			}

			// Token: 0x170003EF RID: 1007
			// (get) Token: 0x06001995 RID: 6549 RVA: 0x00043604 File Offset: 0x00042604
			bool ICollection<TValue>.IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06001996 RID: 6550 RVA: 0x00043607 File Offset: 0x00042607
			void ICollection<TValue>.Add(TValue item)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
			}

			// Token: 0x06001997 RID: 6551 RVA: 0x00043610 File Offset: 0x00042610
			bool ICollection<TValue>.Remove(TValue item)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
				return false;
			}

			// Token: 0x06001998 RID: 6552 RVA: 0x0004361A File Offset: 0x0004261A
			void ICollection<TValue>.Clear()
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
			}

			// Token: 0x06001999 RID: 6553 RVA: 0x00043623 File Offset: 0x00042623
			bool ICollection<TValue>.Contains(TValue item)
			{
				return this.dictionary.ContainsValue(item);
			}

			// Token: 0x0600199A RID: 6554 RVA: 0x00043631 File Offset: 0x00042631
			IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
			{
				return new Dictionary<TKey, TValue>.ValueCollection.Enumerator(this.dictionary);
			}

			// Token: 0x0600199B RID: 6555 RVA: 0x00043643 File Offset: 0x00042643
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new Dictionary<TKey, TValue>.ValueCollection.Enumerator(this.dictionary);
			}

			// Token: 0x0600199C RID: 6556 RVA: 0x00043658 File Offset: 0x00042658
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
				if (index < 0 || index > array.Length)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
				}
				if (array.Length - index < this.dictionary.Count)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				TValue[] array2 = array as TValue[];
				if (array2 != null)
				{
					this.CopyTo(array2, index);
					return;
				}
				object[] array3 = array as object[];
				if (array3 == null)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
				}
				int count = this.dictionary.count;
				Dictionary<TKey, TValue>.Entry[] entries = this.dictionary.entries;
				try
				{
					for (int i = 0; i < count; i++)
					{
						if (entries[i].hashCode >= 0)
						{
							array3[index++] = entries[i].value;
						}
					}
				}
				catch (ArrayTypeMismatchException)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
				}
			}

			// Token: 0x170003F0 RID: 1008
			// (get) Token: 0x0600199D RID: 6557 RVA: 0x00043750 File Offset: 0x00042750
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170003F1 RID: 1009
			// (get) Token: 0x0600199E RID: 6558 RVA: 0x00043753 File Offset: 0x00042753
			object ICollection.SyncRoot
			{
				get
				{
					return ((ICollection)this.dictionary).SyncRoot;
				}
			}

			// Token: 0x040009D6 RID: 2518
			private Dictionary<TKey, TValue> dictionary;

			// Token: 0x0200027C RID: 636
			[Serializable]
			public struct Enumerator : IEnumerator<TValue>, IDisposable, IEnumerator
			{
				// Token: 0x0600199F RID: 6559 RVA: 0x00043760 File Offset: 0x00042760
				internal Enumerator(Dictionary<TKey, TValue> dictionary)
				{
					this.dictionary = dictionary;
					this.version = dictionary.version;
					this.index = 0;
					this.currentValue = default(TValue);
				}

				// Token: 0x060019A0 RID: 6560 RVA: 0x00043788 File Offset: 0x00042788
				public void Dispose()
				{
				}

				// Token: 0x060019A1 RID: 6561 RVA: 0x0004378C File Offset: 0x0004278C
				public bool MoveNext()
				{
					if (this.version != this.dictionary.version)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
					}
					while (this.index < this.dictionary.count)
					{
						if (this.dictionary.entries[this.index].hashCode >= 0)
						{
							this.currentValue = this.dictionary.entries[this.index].value;
							this.index++;
							return true;
						}
						this.index++;
					}
					this.index = this.dictionary.count + 1;
					this.currentValue = default(TValue);
					return false;
				}

				// Token: 0x170003F2 RID: 1010
				// (get) Token: 0x060019A2 RID: 6562 RVA: 0x00043845 File Offset: 0x00042845
				public TValue Current
				{
					get
					{
						return this.currentValue;
					}
				}

				// Token: 0x170003F3 RID: 1011
				// (get) Token: 0x060019A3 RID: 6563 RVA: 0x0004384D File Offset: 0x0004284D
				object IEnumerator.Current
				{
					get
					{
						if (this.index == 0 || this.index == this.dictionary.count + 1)
						{
							ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
						}
						return this.currentValue;
					}
				}

				// Token: 0x060019A4 RID: 6564 RVA: 0x0004387E File Offset: 0x0004287E
				void IEnumerator.Reset()
				{
					if (this.version != this.dictionary.version)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumFailedVersion);
					}
					this.index = 0;
					this.currentValue = default(TValue);
				}

				// Token: 0x040009D7 RID: 2519
				private Dictionary<TKey, TValue> dictionary;

				// Token: 0x040009D8 RID: 2520
				private int index;

				// Token: 0x040009D9 RID: 2521
				private int version;

				// Token: 0x040009DA RID: 2522
				private TValue currentValue;
			}
		}
	}
}
