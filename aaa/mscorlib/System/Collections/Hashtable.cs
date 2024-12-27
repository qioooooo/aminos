using System;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System.Collections
{
	// Token: 0x02000250 RID: 592
	[DebuggerTypeProxy(typeof(Hashtable.HashtableDebugView))]
	[DebuggerDisplay("Count = {Count}")]
	[ComVisible(true)]
	[Serializable]
	public class Hashtable : IDictionary, ICollection, IEnumerable, ISerializable, IDeserializationCallback, ICloneable
	{
		// Token: 0x17000365 RID: 869
		// (get) Token: 0x060017B9 RID: 6073 RVA: 0x0003D0D7 File Offset: 0x0003C0D7
		// (set) Token: 0x060017BA RID: 6074 RVA: 0x0003D110 File Offset: 0x0003C110
		[Obsolete("Please use EqualityComparer property.")]
		protected IHashCodeProvider hcp
		{
			get
			{
				if (this._keycomparer is CompatibleComparer)
				{
					return ((CompatibleComparer)this._keycomparer).HashCodeProvider;
				}
				if (this._keycomparer == null)
				{
					return null;
				}
				throw new ArgumentException(Environment.GetResourceString("Arg_CannotMixComparisonInfrastructure"));
			}
			set
			{
				if (this._keycomparer is CompatibleComparer)
				{
					CompatibleComparer compatibleComparer = (CompatibleComparer)this._keycomparer;
					this._keycomparer = new CompatibleComparer(compatibleComparer.Comparer, value);
					return;
				}
				if (this._keycomparer == null)
				{
					this._keycomparer = new CompatibleComparer(null, value);
					return;
				}
				throw new ArgumentException(Environment.GetResourceString("Arg_CannotMixComparisonInfrastructure"));
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x060017BB RID: 6075 RVA: 0x0003D16E File Offset: 0x0003C16E
		// (set) Token: 0x060017BC RID: 6076 RVA: 0x0003D1A8 File Offset: 0x0003C1A8
		[Obsolete("Please use KeyComparer properties.")]
		protected IComparer comparer
		{
			get
			{
				if (this._keycomparer is CompatibleComparer)
				{
					return ((CompatibleComparer)this._keycomparer).Comparer;
				}
				if (this._keycomparer == null)
				{
					return null;
				}
				throw new ArgumentException(Environment.GetResourceString("Arg_CannotMixComparisonInfrastructure"));
			}
			set
			{
				if (this._keycomparer is CompatibleComparer)
				{
					CompatibleComparer compatibleComparer = (CompatibleComparer)this._keycomparer;
					this._keycomparer = new CompatibleComparer(value, compatibleComparer.HashCodeProvider);
					return;
				}
				if (this._keycomparer == null)
				{
					this._keycomparer = new CompatibleComparer(value, null);
					return;
				}
				throw new ArgumentException(Environment.GetResourceString("Arg_CannotMixComparisonInfrastructure"));
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x060017BD RID: 6077 RVA: 0x0003D206 File Offset: 0x0003C206
		protected IEqualityComparer EqualityComparer
		{
			get
			{
				return this._keycomparer;
			}
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x0003D20E File Offset: 0x0003C20E
		internal Hashtable(bool trash)
		{
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x0003D216 File Offset: 0x0003C216
		public Hashtable()
			: this(0, 1f)
		{
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x0003D224 File Offset: 0x0003C224
		public Hashtable(int capacity)
			: this(capacity, 1f)
		{
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x0003D234 File Offset: 0x0003C234
		public Hashtable(int capacity, float loadFactor)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (loadFactor < 0.1f || loadFactor > 1f)
			{
				throw new ArgumentOutOfRangeException("loadFactor", Environment.GetResourceString("ArgumentOutOfRange_HashtableLoadFactor", new object[] { 0.1, 1.0 }));
			}
			this.loadFactor = 0.72f * loadFactor;
			double num = (double)((float)capacity / this.loadFactor);
			if (num > 2147483647.0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_HTCapacityOverflow"));
			}
			int num2 = ((num > 11.0) ? HashHelpers.GetPrime((int)num) : 11);
			this.buckets = new Hashtable.bucket[num2];
			this.loadsize = (int)(this.loadFactor * (float)num2);
			this.isWriterInProgress = false;
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x0003D320 File Offset: 0x0003C320
		[Obsolete("Please use Hashtable(int, float, IEqualityComparer) instead.")]
		public Hashtable(int capacity, float loadFactor, IHashCodeProvider hcp, IComparer comparer)
			: this(capacity, loadFactor)
		{
			if (hcp == null && comparer == null)
			{
				this._keycomparer = null;
				return;
			}
			this._keycomparer = new CompatibleComparer(comparer, hcp);
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x0003D347 File Offset: 0x0003C347
		public Hashtable(int capacity, float loadFactor, IEqualityComparer equalityComparer)
			: this(capacity, loadFactor)
		{
			this._keycomparer = equalityComparer;
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x0003D358 File Offset: 0x0003C358
		[Obsolete("Please use Hashtable(IEqualityComparer) instead.")]
		public Hashtable(IHashCodeProvider hcp, IComparer comparer)
			: this(0, 1f, hcp, comparer)
		{
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x0003D368 File Offset: 0x0003C368
		public Hashtable(IEqualityComparer equalityComparer)
			: this(0, 1f, equalityComparer)
		{
		}

		// Token: 0x060017C6 RID: 6086 RVA: 0x0003D377 File Offset: 0x0003C377
		[Obsolete("Please use Hashtable(int, IEqualityComparer) instead.")]
		public Hashtable(int capacity, IHashCodeProvider hcp, IComparer comparer)
			: this(capacity, 1f, hcp, comparer)
		{
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x0003D387 File Offset: 0x0003C387
		public Hashtable(int capacity, IEqualityComparer equalityComparer)
			: this(capacity, 1f, equalityComparer)
		{
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x0003D396 File Offset: 0x0003C396
		public Hashtable(IDictionary d)
			: this(d, 1f)
		{
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x0003D3A4 File Offset: 0x0003C3A4
		public Hashtable(IDictionary d, float loadFactor)
			: this(d, loadFactor, null)
		{
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x0003D3AF File Offset: 0x0003C3AF
		[Obsolete("Please use Hashtable(IDictionary, IEqualityComparer) instead.")]
		public Hashtable(IDictionary d, IHashCodeProvider hcp, IComparer comparer)
			: this(d, 1f, hcp, comparer)
		{
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x0003D3BF File Offset: 0x0003C3BF
		public Hashtable(IDictionary d, IEqualityComparer equalityComparer)
			: this(d, 1f, equalityComparer)
		{
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x0003D3D0 File Offset: 0x0003C3D0
		[Obsolete("Please use Hashtable(IDictionary, float, IEqualityComparer) instead.")]
		public Hashtable(IDictionary d, float loadFactor, IHashCodeProvider hcp, IComparer comparer)
			: this((d != null) ? d.Count : 0, loadFactor, hcp, comparer)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d", Environment.GetResourceString("ArgumentNull_Dictionary"));
			}
			IDictionaryEnumerator enumerator = d.GetEnumerator();
			while (enumerator.MoveNext())
			{
				this.Add(enumerator.Key, enumerator.Value);
			}
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x0003D430 File Offset: 0x0003C430
		public Hashtable(IDictionary d, float loadFactor, IEqualityComparer equalityComparer)
			: this((d != null) ? d.Count : 0, loadFactor, equalityComparer)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d", Environment.GetResourceString("ArgumentNull_Dictionary"));
			}
			IDictionaryEnumerator enumerator = d.GetEnumerator();
			while (enumerator.MoveNext())
			{
				this.Add(enumerator.Key, enumerator.Value);
			}
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x0003D48C File Offset: 0x0003C48C
		protected Hashtable(SerializationInfo info, StreamingContext context)
		{
			this.m_siInfo = info;
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x0003D49C File Offset: 0x0003C49C
		private uint InitHash(object key, int hashsize, out uint seed, out uint incr)
		{
			uint num = (uint)(this.GetHash(key) & int.MaxValue);
			seed = num;
			incr = 1U + ((seed >> 5) + 1U) % (uint)(hashsize - 1);
			return num;
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x0003D4CA File Offset: 0x0003C4CA
		public virtual void Add(object key, object value)
		{
			this.Insert(key, value, true);
		}

		// Token: 0x060017D1 RID: 6097 RVA: 0x0003D4D8 File Offset: 0x0003C4D8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public virtual void Clear()
		{
			if (this.count == 0)
			{
				return;
			}
			Thread.BeginCriticalRegion();
			this.isWriterInProgress = true;
			for (int i = 0; i < this.buckets.Length; i++)
			{
				this.buckets[i].hash_coll = 0;
				this.buckets[i].key = null;
				this.buckets[i].val = null;
			}
			this.count = 0;
			this.occupancy = 0;
			this.UpdateVersion();
			this.isWriterInProgress = false;
			Thread.EndCriticalRegion();
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x0003D568 File Offset: 0x0003C568
		public virtual object Clone()
		{
			Hashtable.bucket[] array = this.buckets;
			Hashtable hashtable = new Hashtable(this.count, this._keycomparer);
			hashtable.version = this.version;
			hashtable.loadFactor = this.loadFactor;
			hashtable.count = 0;
			int i = array.Length;
			while (i > 0)
			{
				i--;
				object key = array[i].key;
				if (key != null && key != array)
				{
					hashtable[key] = array[i].val;
				}
			}
			return hashtable;
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x0003D5E7 File Offset: 0x0003C5E7
		public virtual bool Contains(object key)
		{
			return this.ContainsKey(key);
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x0003D5F0 File Offset: 0x0003C5F0
		public virtual bool ContainsKey(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
			}
			Hashtable.bucket[] array = this.buckets;
			uint num2;
			uint num3;
			uint num = this.InitHash(key, array.Length, out num2, out num3);
			int num4 = 0;
			int num5 = (int)(num2 % (uint)array.Length);
			for (;;)
			{
				Hashtable.bucket bucket = array[num5];
				if (bucket.key == null)
				{
					break;
				}
				if ((long)(bucket.hash_coll & 2147483647) == (long)((ulong)num) && this.KeyEquals(bucket.key, key))
				{
					return true;
				}
				num5 = (int)(((long)num5 + (long)((ulong)num3)) % (long)((ulong)array.Length));
				if (bucket.hash_coll >= 0 || ++num4 >= array.Length)
				{
					return false;
				}
			}
			return false;
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x0003D698 File Offset: 0x0003C698
		public virtual bool ContainsValue(object value)
		{
			if (value == null)
			{
				int num = this.buckets.Length;
				while (--num >= 0)
				{
					if (this.buckets[num].key != null && this.buckets[num].key != this.buckets && this.buckets[num].val == null)
					{
						return true;
					}
				}
			}
			else
			{
				int num2 = this.buckets.Length;
				while (--num2 >= 0)
				{
					object val = this.buckets[num2].val;
					if (val != null && val.Equals(value))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x0003D734 File Offset: 0x0003C734
		private void CopyKeys(Array array, int arrayIndex)
		{
			Hashtable.bucket[] array2 = this.buckets;
			int num = array2.Length;
			while (--num >= 0)
			{
				object key = array2[num].key;
				if (key != null && key != this.buckets)
				{
					array.SetValue(key, arrayIndex++);
				}
			}
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x0003D77C File Offset: 0x0003C77C
		private void CopyEntries(Array array, int arrayIndex)
		{
			Hashtable.bucket[] array2 = this.buckets;
			int num = array2.Length;
			while (--num >= 0)
			{
				object key = array2[num].key;
				if (key != null && key != this.buckets)
				{
					DictionaryEntry dictionaryEntry = new DictionaryEntry(key, array2[num].val);
					array.SetValue(dictionaryEntry, arrayIndex++);
				}
			}
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x0003D7E0 File Offset: 0x0003C7E0
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
			if (array.Length - arrayIndex < this.count)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ArrayPlusOffTooSmall"));
			}
			this.CopyEntries(array, arrayIndex);
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x0003D860 File Offset: 0x0003C860
		internal virtual KeyValuePairs[] ToKeyValuePairsArray()
		{
			KeyValuePairs[] array = new KeyValuePairs[this.count];
			int num = 0;
			Hashtable.bucket[] array2 = this.buckets;
			int num2 = array2.Length;
			while (--num2 >= 0)
			{
				object key = array2[num2].key;
				if (key != null && key != this.buckets)
				{
					array[num++] = new KeyValuePairs(key, array2[num2].val);
				}
			}
			return array;
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x0003D8C8 File Offset: 0x0003C8C8
		private void CopyValues(Array array, int arrayIndex)
		{
			Hashtable.bucket[] array2 = this.buckets;
			int num = array2.Length;
			while (--num >= 0)
			{
				object key = array2[num].key;
				if (key != null && key != this.buckets)
				{
					array.SetValue(array2[num].val, arrayIndex++);
				}
			}
		}

		// Token: 0x17000368 RID: 872
		public virtual object this[object key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
				}
				Hashtable.bucket[] array = this.buckets;
				uint num2;
				uint num3;
				uint num = this.InitHash(key, array.Length, out num2, out num3);
				int num4 = 0;
				int num5 = (int)(num2 % (uint)array.Length);
				Hashtable.bucket bucket;
				for (;;)
				{
					int num6 = 0;
					int num7;
					do
					{
						num7 = this.version;
						bucket = array[num5];
						if (++num6 % 8 == 0)
						{
							Thread.Sleep(1);
						}
					}
					while (this.isWriterInProgress || num7 != this.version);
					if (bucket.key == null)
					{
						break;
					}
					if ((long)(bucket.hash_coll & 2147483647) == (long)((ulong)num) && this.KeyEquals(bucket.key, key))
					{
						goto Block_7;
					}
					num5 = (int)(((long)num5 + (long)((ulong)num3)) % (long)((ulong)array.Length));
					if (bucket.hash_coll >= 0 || ++num4 >= array.Length)
					{
						goto IL_00D7;
					}
				}
				return null;
				Block_7:
				return bucket.val;
				IL_00D7:
				return null;
			}
			set
			{
				this.Insert(key, value, false);
			}
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x0003DA0C File Offset: 0x0003CA0C
		private void expand()
		{
			int prime = HashHelpers.GetPrime(this.buckets.Length * 2);
			this.rehash(prime);
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x0003DA30 File Offset: 0x0003CA30
		private void rehash()
		{
			this.rehash(this.buckets.Length);
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x0003DA40 File Offset: 0x0003CA40
		private void UpdateVersion()
		{
			this.version++;
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x0003DA54 File Offset: 0x0003CA54
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		private void rehash(int newsize)
		{
			this.occupancy = 0;
			Hashtable.bucket[] array = new Hashtable.bucket[newsize];
			for (int i = 0; i < this.buckets.Length; i++)
			{
				Hashtable.bucket bucket = this.buckets[i];
				if (bucket.key != null && bucket.key != this.buckets)
				{
					this.putEntry(array, bucket.key, bucket.val, bucket.hash_coll & int.MaxValue);
				}
			}
			Thread.BeginCriticalRegion();
			this.isWriterInProgress = true;
			this.buckets = array;
			this.loadsize = (int)(this.loadFactor * (float)newsize);
			this.UpdateVersion();
			this.isWriterInProgress = false;
			Thread.EndCriticalRegion();
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x0003DB07 File Offset: 0x0003CB07
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Hashtable.HashtableEnumerator(this, 3);
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x0003DB10 File Offset: 0x0003CB10
		public virtual IDictionaryEnumerator GetEnumerator()
		{
			return new Hashtable.HashtableEnumerator(this, 3);
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x0003DB19 File Offset: 0x0003CB19
		protected virtual int GetHash(object key)
		{
			if (this._keycomparer != null)
			{
				return this._keycomparer.GetHashCode(key);
			}
			return key.GetHashCode();
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x060017E4 RID: 6116 RVA: 0x0003DB36 File Offset: 0x0003CB36
		public virtual bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x060017E5 RID: 6117 RVA: 0x0003DB39 File Offset: 0x0003CB39
		public virtual bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x060017E6 RID: 6118 RVA: 0x0003DB3C File Offset: 0x0003CB3C
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x0003DB3F File Offset: 0x0003CB3F
		protected virtual bool KeyEquals(object item, object key)
		{
			if (object.ReferenceEquals(this.buckets, item))
			{
				return false;
			}
			if (this._keycomparer != null)
			{
				return this._keycomparer.Equals(item, key);
			}
			return item != null && item.Equals(key);
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x060017E8 RID: 6120 RVA: 0x0003DB73 File Offset: 0x0003CB73
		public virtual ICollection Keys
		{
			get
			{
				if (this.keys == null)
				{
					this.keys = new Hashtable.KeyCollection(this);
				}
				return this.keys;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x060017E9 RID: 6121 RVA: 0x0003DB8F File Offset: 0x0003CB8F
		public virtual ICollection Values
		{
			get
			{
				if (this.values == null)
				{
					this.values = new Hashtable.ValueCollection(this);
				}
				return this.values;
			}
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x0003DBAC File Offset: 0x0003CBAC
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		private void Insert(object key, object nvalue, bool add)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
			}
			if (this.count >= this.loadsize)
			{
				this.expand();
			}
			else if (this.occupancy > this.loadsize && this.count > 100)
			{
				this.rehash();
			}
			uint num2;
			uint num3;
			uint num = this.InitHash(key, this.buckets.Length, out num2, out num3);
			int num4 = 0;
			int num5 = -1;
			int num6 = (int)(num2 % (uint)this.buckets.Length);
			for (;;)
			{
				if (num5 == -1 && this.buckets[num6].key == this.buckets && this.buckets[num6].hash_coll < 0)
				{
					num5 = num6;
				}
				if (this.buckets[num6].key == null || (this.buckets[num6].key == this.buckets && ((long)this.buckets[num6].hash_coll & (long)((ulong)(-2147483648))) == 0L))
				{
					break;
				}
				if ((long)(this.buckets[num6].hash_coll & 2147483647) == (long)((ulong)num) && this.KeyEquals(this.buckets[num6].key, key))
				{
					goto Block_12;
				}
				if (num5 == -1 && this.buckets[num6].hash_coll >= 0)
				{
					Hashtable.bucket[] array = this.buckets;
					int num7 = num6;
					array[num7].hash_coll = array[num7].hash_coll | int.MinValue;
					this.occupancy++;
				}
				num6 = (int)(((long)num6 + (long)((ulong)num3)) % (long)((ulong)this.buckets.Length));
				if (++num4 >= this.buckets.Length)
				{
					goto Block_16;
				}
			}
			if (num5 != -1)
			{
				num6 = num5;
			}
			Thread.BeginCriticalRegion();
			this.isWriterInProgress = true;
			this.buckets[num6].val = nvalue;
			this.buckets[num6].key = key;
			Hashtable.bucket[] array2 = this.buckets;
			int num8 = num6;
			array2[num8].hash_coll = array2[num8].hash_coll | (int)num;
			this.count++;
			this.UpdateVersion();
			this.isWriterInProgress = false;
			Thread.EndCriticalRegion();
			return;
			Block_12:
			if (add)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_AddingDuplicate__", new object[]
				{
					this.buckets[num6].key,
					key
				}));
			}
			Thread.BeginCriticalRegion();
			this.isWriterInProgress = true;
			this.buckets[num6].val = nvalue;
			this.UpdateVersion();
			this.isWriterInProgress = false;
			Thread.EndCriticalRegion();
			return;
			Block_16:
			if (num5 != -1)
			{
				Thread.BeginCriticalRegion();
				this.isWriterInProgress = true;
				this.buckets[num5].val = nvalue;
				this.buckets[num5].key = key;
				Hashtable.bucket[] array3 = this.buckets;
				int num9 = num5;
				array3[num9].hash_coll = array3[num9].hash_coll | (int)num;
				this.count++;
				this.UpdateVersion();
				this.isWriterInProgress = false;
				Thread.EndCriticalRegion();
				return;
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_HashInsertFailed"));
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x0003DEC4 File Offset: 0x0003CEC4
		private void putEntry(Hashtable.bucket[] newBuckets, object key, object nvalue, int hashcode)
		{
			uint num = 1U + (((uint)hashcode >> 5) + 1U) % (uint)(newBuckets.Length - 1);
			int num2 = hashcode % newBuckets.Length;
			while (newBuckets[num2].key != null && newBuckets[num2].key != this.buckets)
			{
				if (newBuckets[num2].hash_coll >= 0)
				{
					int num3 = num2;
					newBuckets[num3].hash_coll = newBuckets[num3].hash_coll | int.MinValue;
					this.occupancy++;
				}
				num2 = (int)(((long)num2 + (long)((ulong)num)) % (long)((ulong)newBuckets.Length));
			}
			newBuckets[num2].val = nvalue;
			newBuckets[num2].key = key;
			int num4 = num2;
			newBuckets[num4].hash_coll = newBuckets[num4].hash_coll | hashcode;
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x0003DF80 File Offset: 0x0003CF80
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public virtual void Remove(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
			}
			uint num2;
			uint num3;
			uint num = this.InitHash(key, this.buckets.Length, out num2, out num3);
			int num4 = 0;
			int num5 = (int)(num2 % (uint)this.buckets.Length);
			for (;;)
			{
				Hashtable.bucket bucket = this.buckets[num5];
				if ((long)(bucket.hash_coll & 2147483647) == (long)((ulong)num) && this.KeyEquals(bucket.key, key))
				{
					break;
				}
				num5 = (int)(((long)num5 + (long)((ulong)num3)) % (long)((ulong)this.buckets.Length));
				if (bucket.hash_coll >= 0 || ++num4 >= this.buckets.Length)
				{
					return;
				}
			}
			Thread.BeginCriticalRegion();
			this.isWriterInProgress = true;
			Hashtable.bucket[] array = this.buckets;
			int num6 = num5;
			array[num6].hash_coll = array[num6].hash_coll & int.MinValue;
			if (this.buckets[num5].hash_coll != 0)
			{
				this.buckets[num5].key = this.buckets;
			}
			else
			{
				this.buckets[num5].key = null;
			}
			this.buckets[num5].val = null;
			this.count--;
			this.UpdateVersion();
			this.isWriterInProgress = false;
			Thread.EndCriticalRegion();
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x060017ED RID: 6125 RVA: 0x0003E0D5 File Offset: 0x0003D0D5
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

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x060017EE RID: 6126 RVA: 0x0003E0F7 File Offset: 0x0003D0F7
		public virtual int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x0003E0FF File Offset: 0x0003D0FF
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public static Hashtable Synchronized(Hashtable table)
		{
			if (table == null)
			{
				throw new ArgumentNullException("table");
			}
			return new Hashtable.SyncHashtable(table);
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x0003E118 File Offset: 0x0003D118
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("LoadFactor", this.loadFactor);
			info.AddValue("Version", this.version);
			if (this._keycomparer == null)
			{
				info.AddValue("Comparer", null, typeof(IComparer));
				info.AddValue("HashCodeProvider", null, typeof(IHashCodeProvider));
			}
			else if (this._keycomparer is CompatibleComparer)
			{
				CompatibleComparer compatibleComparer = this._keycomparer as CompatibleComparer;
				info.AddValue("Comparer", compatibleComparer.Comparer, typeof(IComparer));
				info.AddValue("HashCodeProvider", compatibleComparer.HashCodeProvider, typeof(IHashCodeProvider));
			}
			else
			{
				info.AddValue("KeyComparer", this._keycomparer, typeof(IEqualityComparer));
			}
			info.AddValue("HashSize", this.buckets.Length);
			object[] array = new object[this.count];
			object[] array2 = new object[this.count];
			this.CopyKeys(array, 0);
			this.CopyValues(array2, 0);
			info.AddValue("Keys", array, typeof(object[]));
			info.AddValue("Values", array2, typeof(object[]));
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x0003E260 File Offset: 0x0003D260
		public virtual void OnDeserialization(object sender)
		{
			if (this.buckets != null)
			{
				return;
			}
			if (this.m_siInfo == null)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_InvalidOnDeser"));
			}
			int num = 0;
			IComparer comparer = null;
			IHashCodeProvider hashCodeProvider = null;
			object[] array = null;
			object[] array2 = null;
			SerializationInfoEnumerator enumerator = this.m_siInfo.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string name;
				switch (name = enumerator.Name)
				{
				case "LoadFactor":
					this.loadFactor = this.m_siInfo.GetSingle("LoadFactor");
					break;
				case "HashSize":
					num = this.m_siInfo.GetInt32("HashSize");
					break;
				case "KeyComparer":
					this._keycomparer = (IEqualityComparer)this.m_siInfo.GetValue("KeyComparer", typeof(IEqualityComparer));
					break;
				case "Comparer":
					comparer = (IComparer)this.m_siInfo.GetValue("Comparer", typeof(IComparer));
					break;
				case "HashCodeProvider":
					hashCodeProvider = (IHashCodeProvider)this.m_siInfo.GetValue("HashCodeProvider", typeof(IHashCodeProvider));
					break;
				case "Keys":
					array = (object[])this.m_siInfo.GetValue("Keys", typeof(object[]));
					break;
				case "Values":
					array2 = (object[])this.m_siInfo.GetValue("Values", typeof(object[]));
					break;
				}
			}
			this.loadsize = (int)(this.loadFactor * (float)num);
			if (this._keycomparer == null && (comparer != null || hashCodeProvider != null))
			{
				this._keycomparer = new CompatibleComparer(comparer, hashCodeProvider);
			}
			this.buckets = new Hashtable.bucket[num];
			if (array == null)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_MissingKeys"));
			}
			if (array2 == null)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_MissingValues"));
			}
			if (array.Length != array2.Length)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_KeyValueDifferentSizes"));
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null)
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_NullKey"));
				}
				this.Insert(array[i], array2[i], true);
			}
			this.version = this.m_siInfo.GetInt32("Version");
			this.m_siInfo = null;
		}

		// Token: 0x04000940 RID: 2368
		private const string LoadFactorName = "LoadFactor";

		// Token: 0x04000941 RID: 2369
		private const string VersionName = "Version";

		// Token: 0x04000942 RID: 2370
		private const string ComparerName = "Comparer";

		// Token: 0x04000943 RID: 2371
		private const string HashCodeProviderName = "HashCodeProvider";

		// Token: 0x04000944 RID: 2372
		private const string HashSizeName = "HashSize";

		// Token: 0x04000945 RID: 2373
		private const string KeysName = "Keys";

		// Token: 0x04000946 RID: 2374
		private const string ValuesName = "Values";

		// Token: 0x04000947 RID: 2375
		private const string KeyComparerName = "KeyComparer";

		// Token: 0x04000948 RID: 2376
		private Hashtable.bucket[] buckets;

		// Token: 0x04000949 RID: 2377
		private int count;

		// Token: 0x0400094A RID: 2378
		private int occupancy;

		// Token: 0x0400094B RID: 2379
		private int loadsize;

		// Token: 0x0400094C RID: 2380
		private float loadFactor;

		// Token: 0x0400094D RID: 2381
		private volatile int version;

		// Token: 0x0400094E RID: 2382
		private volatile bool isWriterInProgress;

		// Token: 0x0400094F RID: 2383
		private ICollection keys;

		// Token: 0x04000950 RID: 2384
		private ICollection values;

		// Token: 0x04000951 RID: 2385
		private IEqualityComparer _keycomparer;

		// Token: 0x04000952 RID: 2386
		private object _syncRoot;

		// Token: 0x04000953 RID: 2387
		private SerializationInfo m_siInfo;

		// Token: 0x02000251 RID: 593
		private struct bucket
		{
			// Token: 0x04000954 RID: 2388
			public object key;

			// Token: 0x04000955 RID: 2389
			public object val;

			// Token: 0x04000956 RID: 2390
			public int hash_coll;
		}

		// Token: 0x02000252 RID: 594
		[Serializable]
		private class KeyCollection : ICollection, IEnumerable
		{
			// Token: 0x060017F2 RID: 6130 RVA: 0x0003E51D File Offset: 0x0003D51D
			internal KeyCollection(Hashtable hashtable)
			{
				this._hashtable = hashtable;
			}

			// Token: 0x060017F3 RID: 6131 RVA: 0x0003E52C File Offset: 0x0003D52C
			public virtual void CopyTo(Array array, int arrayIndex)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (array.Rank != 1)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_RankMultiDimNotSupported"));
				}
				if (arrayIndex < 0)
				{
					throw new ArgumentOutOfRangeException("arrayIndex", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (array.Length - arrayIndex < this._hashtable.count)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_ArrayPlusOffTooSmall"));
				}
				this._hashtable.CopyKeys(array, arrayIndex);
			}

			// Token: 0x060017F4 RID: 6132 RVA: 0x0003E5AB File Offset: 0x0003D5AB
			public virtual IEnumerator GetEnumerator()
			{
				return new Hashtable.HashtableEnumerator(this._hashtable, 1);
			}

			// Token: 0x17000370 RID: 880
			// (get) Token: 0x060017F5 RID: 6133 RVA: 0x0003E5B9 File Offset: 0x0003D5B9
			public virtual bool IsSynchronized
			{
				get
				{
					return this._hashtable.IsSynchronized;
				}
			}

			// Token: 0x17000371 RID: 881
			// (get) Token: 0x060017F6 RID: 6134 RVA: 0x0003E5C6 File Offset: 0x0003D5C6
			public virtual object SyncRoot
			{
				get
				{
					return this._hashtable.SyncRoot;
				}
			}

			// Token: 0x17000372 RID: 882
			// (get) Token: 0x060017F7 RID: 6135 RVA: 0x0003E5D3 File Offset: 0x0003D5D3
			public virtual int Count
			{
				get
				{
					return this._hashtable.count;
				}
			}

			// Token: 0x04000957 RID: 2391
			private Hashtable _hashtable;
		}

		// Token: 0x02000253 RID: 595
		[Serializable]
		private class ValueCollection : ICollection, IEnumerable
		{
			// Token: 0x060017F8 RID: 6136 RVA: 0x0003E5E0 File Offset: 0x0003D5E0
			internal ValueCollection(Hashtable hashtable)
			{
				this._hashtable = hashtable;
			}

			// Token: 0x060017F9 RID: 6137 RVA: 0x0003E5F0 File Offset: 0x0003D5F0
			public virtual void CopyTo(Array array, int arrayIndex)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (array.Rank != 1)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_RankMultiDimNotSupported"));
				}
				if (arrayIndex < 0)
				{
					throw new ArgumentOutOfRangeException("arrayIndex", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (array.Length - arrayIndex < this._hashtable.count)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_ArrayPlusOffTooSmall"));
				}
				this._hashtable.CopyValues(array, arrayIndex);
			}

			// Token: 0x060017FA RID: 6138 RVA: 0x0003E66F File Offset: 0x0003D66F
			public virtual IEnumerator GetEnumerator()
			{
				return new Hashtable.HashtableEnumerator(this._hashtable, 2);
			}

			// Token: 0x17000373 RID: 883
			// (get) Token: 0x060017FB RID: 6139 RVA: 0x0003E67D File Offset: 0x0003D67D
			public virtual bool IsSynchronized
			{
				get
				{
					return this._hashtable.IsSynchronized;
				}
			}

			// Token: 0x17000374 RID: 884
			// (get) Token: 0x060017FC RID: 6140 RVA: 0x0003E68A File Offset: 0x0003D68A
			public virtual object SyncRoot
			{
				get
				{
					return this._hashtable.SyncRoot;
				}
			}

			// Token: 0x17000375 RID: 885
			// (get) Token: 0x060017FD RID: 6141 RVA: 0x0003E697 File Offset: 0x0003D697
			public virtual int Count
			{
				get
				{
					return this._hashtable.count;
				}
			}

			// Token: 0x04000958 RID: 2392
			private Hashtable _hashtable;
		}

		// Token: 0x02000254 RID: 596
		[Serializable]
		private class SyncHashtable : Hashtable
		{
			// Token: 0x060017FE RID: 6142 RVA: 0x0003E6A4 File Offset: 0x0003D6A4
			internal SyncHashtable(Hashtable table)
				: base(false)
			{
				this._table = table;
			}

			// Token: 0x060017FF RID: 6143 RVA: 0x0003E6B4 File Offset: 0x0003D6B4
			internal SyncHashtable(SerializationInfo info, StreamingContext context)
				: base(info, context)
			{
				this._table = (Hashtable)info.GetValue("ParentTable", typeof(Hashtable));
				if (this._table == null)
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_InsufficientState"));
				}
			}

			// Token: 0x06001800 RID: 6144 RVA: 0x0003E701 File Offset: 0x0003D701
			public override void GetObjectData(SerializationInfo info, StreamingContext context)
			{
				if (info == null)
				{
					throw new ArgumentNullException("info");
				}
				info.AddValue("ParentTable", this._table, typeof(Hashtable));
			}

			// Token: 0x17000376 RID: 886
			// (get) Token: 0x06001801 RID: 6145 RVA: 0x0003E72C File Offset: 0x0003D72C
			public override int Count
			{
				get
				{
					return this._table.Count;
				}
			}

			// Token: 0x17000377 RID: 887
			// (get) Token: 0x06001802 RID: 6146 RVA: 0x0003E739 File Offset: 0x0003D739
			public override bool IsReadOnly
			{
				get
				{
					return this._table.IsReadOnly;
				}
			}

			// Token: 0x17000378 RID: 888
			// (get) Token: 0x06001803 RID: 6147 RVA: 0x0003E746 File Offset: 0x0003D746
			public override bool IsFixedSize
			{
				get
				{
					return this._table.IsFixedSize;
				}
			}

			// Token: 0x17000379 RID: 889
			// (get) Token: 0x06001804 RID: 6148 RVA: 0x0003E753 File Offset: 0x0003D753
			public override bool IsSynchronized
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700037A RID: 890
			public override object this[object key]
			{
				get
				{
					return this._table[key];
				}
				set
				{
					lock (this._table.SyncRoot)
					{
						this._table[key] = value;
					}
				}
			}

			// Token: 0x1700037B RID: 891
			// (get) Token: 0x06001807 RID: 6151 RVA: 0x0003E7AC File Offset: 0x0003D7AC
			public override object SyncRoot
			{
				get
				{
					return this._table.SyncRoot;
				}
			}

			// Token: 0x06001808 RID: 6152 RVA: 0x0003E7BC File Offset: 0x0003D7BC
			public override void Add(object key, object value)
			{
				lock (this._table.SyncRoot)
				{
					this._table.Add(key, value);
				}
			}

			// Token: 0x06001809 RID: 6153 RVA: 0x0003E804 File Offset: 0x0003D804
			public override void Clear()
			{
				lock (this._table.SyncRoot)
				{
					this._table.Clear();
				}
			}

			// Token: 0x0600180A RID: 6154 RVA: 0x0003E848 File Offset: 0x0003D848
			public override bool Contains(object key)
			{
				return this._table.Contains(key);
			}

			// Token: 0x0600180B RID: 6155 RVA: 0x0003E856 File Offset: 0x0003D856
			public override bool ContainsKey(object key)
			{
				return this._table.ContainsKey(key);
			}

			// Token: 0x0600180C RID: 6156 RVA: 0x0003E864 File Offset: 0x0003D864
			public override bool ContainsValue(object key)
			{
				bool flag;
				lock (this._table.SyncRoot)
				{
					flag = this._table.ContainsValue(key);
				}
				return flag;
			}

			// Token: 0x0600180D RID: 6157 RVA: 0x0003E8AC File Offset: 0x0003D8AC
			public override void CopyTo(Array array, int arrayIndex)
			{
				lock (this._table.SyncRoot)
				{
					this._table.CopyTo(array, arrayIndex);
				}
			}

			// Token: 0x0600180E RID: 6158 RVA: 0x0003E8F4 File Offset: 0x0003D8F4
			public override object Clone()
			{
				object obj;
				lock (this._table.SyncRoot)
				{
					obj = Hashtable.Synchronized((Hashtable)this._table.Clone());
				}
				return obj;
			}

			// Token: 0x0600180F RID: 6159 RVA: 0x0003E944 File Offset: 0x0003D944
			public override IDictionaryEnumerator GetEnumerator()
			{
				return this._table.GetEnumerator();
			}

			// Token: 0x1700037C RID: 892
			// (get) Token: 0x06001810 RID: 6160 RVA: 0x0003E954 File Offset: 0x0003D954
			public override ICollection Keys
			{
				get
				{
					ICollection keys;
					lock (this._table.SyncRoot)
					{
						keys = this._table.Keys;
					}
					return keys;
				}
			}

			// Token: 0x1700037D RID: 893
			// (get) Token: 0x06001811 RID: 6161 RVA: 0x0003E99C File Offset: 0x0003D99C
			public override ICollection Values
			{
				get
				{
					ICollection values;
					lock (this._table.SyncRoot)
					{
						values = this._table.Values;
					}
					return values;
				}
			}

			// Token: 0x06001812 RID: 6162 RVA: 0x0003E9E4 File Offset: 0x0003D9E4
			public override void Remove(object key)
			{
				lock (this._table.SyncRoot)
				{
					this._table.Remove(key);
				}
			}

			// Token: 0x06001813 RID: 6163 RVA: 0x0003EA28 File Offset: 0x0003DA28
			public override void OnDeserialization(object sender)
			{
			}

			// Token: 0x06001814 RID: 6164 RVA: 0x0003EA2A File Offset: 0x0003DA2A
			internal override KeyValuePairs[] ToKeyValuePairsArray()
			{
				return this._table.ToKeyValuePairsArray();
			}

			// Token: 0x04000959 RID: 2393
			protected Hashtable _table;
		}

		// Token: 0x02000255 RID: 597
		[Serializable]
		private class HashtableEnumerator : IDictionaryEnumerator, IEnumerator, ICloneable
		{
			// Token: 0x06001815 RID: 6165 RVA: 0x0003EA37 File Offset: 0x0003DA37
			internal HashtableEnumerator(Hashtable hashtable, int getObjRetType)
			{
				this.hashtable = hashtable;
				this.bucket = hashtable.buckets.Length;
				this.version = hashtable.version;
				this.current = false;
				this.getObjectRetType = getObjRetType;
			}

			// Token: 0x06001816 RID: 6166 RVA: 0x0003EA70 File Offset: 0x0003DA70
			public object Clone()
			{
				return base.MemberwiseClone();
			}

			// Token: 0x1700037E RID: 894
			// (get) Token: 0x06001817 RID: 6167 RVA: 0x0003EA78 File Offset: 0x0003DA78
			public virtual object Key
			{
				get
				{
					if (!this.current)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
					}
					return this.currentKey;
				}
			}

			// Token: 0x06001818 RID: 6168 RVA: 0x0003EA98 File Offset: 0x0003DA98
			public virtual bool MoveNext()
			{
				if (this.version != this.hashtable.version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				while (this.bucket > 0)
				{
					this.bucket--;
					object key = this.hashtable.buckets[this.bucket].key;
					if (key != null && key != this.hashtable.buckets)
					{
						this.currentKey = key;
						this.currentValue = this.hashtable.buckets[this.bucket].val;
						this.current = true;
						return true;
					}
				}
				this.current = false;
				return false;
			}

			// Token: 0x1700037F RID: 895
			// (get) Token: 0x06001819 RID: 6169 RVA: 0x0003EB47 File Offset: 0x0003DB47
			public virtual DictionaryEntry Entry
			{
				get
				{
					if (!this.current)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
					}
					return new DictionaryEntry(this.currentKey, this.currentValue);
				}
			}

			// Token: 0x17000380 RID: 896
			// (get) Token: 0x0600181A RID: 6170 RVA: 0x0003EB74 File Offset: 0x0003DB74
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
						return this.currentKey;
					}
					if (this.getObjectRetType == 2)
					{
						return this.currentValue;
					}
					return new DictionaryEntry(this.currentKey, this.currentValue);
				}
			}

			// Token: 0x17000381 RID: 897
			// (get) Token: 0x0600181B RID: 6171 RVA: 0x0003EBCF File Offset: 0x0003DBCF
			public virtual object Value
			{
				get
				{
					if (!this.current)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
					}
					return this.currentValue;
				}
			}

			// Token: 0x0600181C RID: 6172 RVA: 0x0003EBF0 File Offset: 0x0003DBF0
			public virtual void Reset()
			{
				if (this.version != this.hashtable.version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				this.current = false;
				this.bucket = this.hashtable.buckets.Length;
				this.currentKey = null;
				this.currentValue = null;
			}

			// Token: 0x0400095A RID: 2394
			internal const int Keys = 1;

			// Token: 0x0400095B RID: 2395
			internal const int Values = 2;

			// Token: 0x0400095C RID: 2396
			internal const int DictEntry = 3;

			// Token: 0x0400095D RID: 2397
			private Hashtable hashtable;

			// Token: 0x0400095E RID: 2398
			private int bucket;

			// Token: 0x0400095F RID: 2399
			private int version;

			// Token: 0x04000960 RID: 2400
			private bool current;

			// Token: 0x04000961 RID: 2401
			private int getObjectRetType;

			// Token: 0x04000962 RID: 2402
			private object currentKey;

			// Token: 0x04000963 RID: 2403
			private object currentValue;
		}

		// Token: 0x02000256 RID: 598
		internal class HashtableDebugView
		{
			// Token: 0x0600181D RID: 6173 RVA: 0x0003EC4A File Offset: 0x0003DC4A
			public HashtableDebugView(Hashtable hashtable)
			{
				if (hashtable == null)
				{
					throw new ArgumentNullException("hashtable");
				}
				this.hashtable = hashtable;
			}

			// Token: 0x17000382 RID: 898
			// (get) Token: 0x0600181E RID: 6174 RVA: 0x0003EC67 File Offset: 0x0003DC67
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public KeyValuePairs[] Items
			{
				get
				{
					return this.hashtable.ToKeyValuePairsArray();
				}
			}

			// Token: 0x04000964 RID: 2404
			private Hashtable hashtable;
		}
	}
}
