using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System.Collections.Specialized
{
	// Token: 0x0200025C RID: 604
	[Serializable]
	public class OrderedDictionary : IOrderedDictionary, IDictionary, ICollection, IEnumerable, ISerializable, IDeserializationCallback
	{
		// Token: 0x060014D6 RID: 5334 RVA: 0x0004522A File Offset: 0x0004422A
		public OrderedDictionary()
			: this(0)
		{
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x00045233 File Offset: 0x00044233
		public OrderedDictionary(int capacity)
			: this(capacity, null)
		{
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x0004523D File Offset: 0x0004423D
		public OrderedDictionary(IEqualityComparer comparer)
			: this(0, comparer)
		{
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x00045247 File Offset: 0x00044247
		public OrderedDictionary(int capacity, IEqualityComparer comparer)
		{
			this._initialCapacity = capacity;
			this._comparer = comparer;
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x00045260 File Offset: 0x00044260
		private OrderedDictionary(OrderedDictionary dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			this._readOnly = true;
			this._objectsArray = dictionary._objectsArray;
			this._objectsTable = dictionary._objectsTable;
			this._comparer = dictionary._comparer;
			this._initialCapacity = dictionary._initialCapacity;
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x000452B8 File Offset: 0x000442B8
		protected OrderedDictionary(SerializationInfo info, StreamingContext context)
		{
			this._siInfo = info;
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x060014DC RID: 5340 RVA: 0x000452C7 File Offset: 0x000442C7
		public int Count
		{
			get
			{
				return this.objectsArray.Count;
			}
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x060014DD RID: 5341 RVA: 0x000452D4 File Offset: 0x000442D4
		bool IDictionary.IsFixedSize
		{
			get
			{
				return this._readOnly;
			}
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x060014DE RID: 5342 RVA: 0x000452DC File Offset: 0x000442DC
		public bool IsReadOnly
		{
			get
			{
				return this._readOnly;
			}
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x060014DF RID: 5343 RVA: 0x000452E4 File Offset: 0x000442E4
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x060014E0 RID: 5344 RVA: 0x000452E7 File Offset: 0x000442E7
		public ICollection Keys
		{
			get
			{
				return new OrderedDictionary.OrderedDictionaryKeyValueCollection(this.objectsArray, true);
			}
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x060014E1 RID: 5345 RVA: 0x000452F5 File Offset: 0x000442F5
		private ArrayList objectsArray
		{
			get
			{
				if (this._objectsArray == null)
				{
					this._objectsArray = new ArrayList(this._initialCapacity);
				}
				return this._objectsArray;
			}
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x060014E2 RID: 5346 RVA: 0x00045316 File Offset: 0x00044316
		private Hashtable objectsTable
		{
			get
			{
				if (this._objectsTable == null)
				{
					this._objectsTable = new Hashtable(this._initialCapacity, this._comparer);
				}
				return this._objectsTable;
			}
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x060014E3 RID: 5347 RVA: 0x0004533D File Offset: 0x0004433D
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

		// Token: 0x17000458 RID: 1112
		public object this[int index]
		{
			get
			{
				return ((DictionaryEntry)this.objectsArray[index]).Value;
			}
			set
			{
				if (this._readOnly)
				{
					throw new NotSupportedException(SR.GetString("OrderedDictionary_ReadOnly"));
				}
				if (index < 0 || index >= this.objectsArray.Count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				object key = ((DictionaryEntry)this.objectsArray[index]).Key;
				this.objectsArray[index] = new DictionaryEntry(key, value);
				this.objectsTable[key] = value;
			}
		}

		// Token: 0x17000459 RID: 1113
		public object this[object key]
		{
			get
			{
				return this.objectsTable[key];
			}
			set
			{
				if (this._readOnly)
				{
					throw new NotSupportedException(SR.GetString("OrderedDictionary_ReadOnly"));
				}
				if (this.objectsTable.Contains(key))
				{
					this.objectsTable[key] = value;
					this.objectsArray[this.IndexOfKey(key)] = new DictionaryEntry(key, value);
					return;
				}
				this.Add(key, value);
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x060014E8 RID: 5352 RVA: 0x0004547F File Offset: 0x0004447F
		public ICollection Values
		{
			get
			{
				return new OrderedDictionary.OrderedDictionaryKeyValueCollection(this.objectsArray, false);
			}
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x0004548D File Offset: 0x0004448D
		public void Add(object key, object value)
		{
			if (this._readOnly)
			{
				throw new NotSupportedException(SR.GetString("OrderedDictionary_ReadOnly"));
			}
			this.objectsTable.Add(key, value);
			this.objectsArray.Add(new DictionaryEntry(key, value));
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x000454CC File Offset: 0x000444CC
		public void Clear()
		{
			if (this._readOnly)
			{
				throw new NotSupportedException(SR.GetString("OrderedDictionary_ReadOnly"));
			}
			this.objectsTable.Clear();
			this.objectsArray.Clear();
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x000454FC File Offset: 0x000444FC
		public OrderedDictionary AsReadOnly()
		{
			return new OrderedDictionary(this);
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x00045504 File Offset: 0x00044504
		public bool Contains(object key)
		{
			return this.objectsTable.Contains(key);
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x00045512 File Offset: 0x00044512
		public void CopyTo(Array array, int index)
		{
			this.objectsTable.CopyTo(array, index);
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x00045524 File Offset: 0x00044524
		private int IndexOfKey(object key)
		{
			for (int i = 0; i < this.objectsArray.Count; i++)
			{
				object key2 = ((DictionaryEntry)this.objectsArray[i]).Key;
				if (this._comparer != null)
				{
					if (this._comparer.Equals(key2, key))
					{
						return i;
					}
				}
				else if (key2.Equals(key))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x00045588 File Offset: 0x00044588
		public void Insert(int index, object key, object value)
		{
			if (this._readOnly)
			{
				throw new NotSupportedException(SR.GetString("OrderedDictionary_ReadOnly"));
			}
			if (index > this.Count || index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.objectsTable.Add(key, value);
			this.objectsArray.Insert(index, new DictionaryEntry(key, value));
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x000455EC File Offset: 0x000445EC
		protected virtual void OnDeserialization(object sender)
		{
			if (this._siInfo == null)
			{
				throw new SerializationException(SR.GetString("Serialization_InvalidOnDeser"));
			}
			this._comparer = (IEqualityComparer)this._siInfo.GetValue("KeyComparer", typeof(IEqualityComparer));
			this._readOnly = this._siInfo.GetBoolean("ReadOnly");
			this._initialCapacity = this._siInfo.GetInt32("InitialCapacity");
			object[] array = (object[])this._siInfo.GetValue("ArrayList", typeof(object[]));
			if (array != null)
			{
				foreach (object obj in array)
				{
					DictionaryEntry dictionaryEntry;
					try
					{
						dictionaryEntry = (DictionaryEntry)obj;
					}
					catch
					{
						throw new SerializationException(SR.GetString("OrderedDictionary_SerializationMismatch"));
					}
					this.objectsArray.Add(dictionaryEntry);
					this.objectsTable.Add(dictionaryEntry.Key, dictionaryEntry.Value);
				}
			}
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x000456F4 File Offset: 0x000446F4
		public void RemoveAt(int index)
		{
			if (this._readOnly)
			{
				throw new NotSupportedException(SR.GetString("OrderedDictionary_ReadOnly"));
			}
			if (index >= this.Count || index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			object key = ((DictionaryEntry)this.objectsArray[index]).Key;
			this.objectsArray.RemoveAt(index);
			this.objectsTable.Remove(key);
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x00045764 File Offset: 0x00044764
		public void Remove(object key)
		{
			if (this._readOnly)
			{
				throw new NotSupportedException(SR.GetString("OrderedDictionary_ReadOnly"));
			}
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			int num = this.IndexOfKey(key);
			if (num < 0)
			{
				return;
			}
			this.objectsTable.Remove(key);
			this.objectsArray.RemoveAt(num);
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x000457BC File Offset: 0x000447BC
		public virtual IDictionaryEnumerator GetEnumerator()
		{
			return new OrderedDictionary.OrderedDictionaryEnumerator(this.objectsArray, 3);
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x000457CA File Offset: 0x000447CA
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new OrderedDictionary.OrderedDictionaryEnumerator(this.objectsArray, 3);
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x000457D8 File Offset: 0x000447D8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("KeyComparer", this._comparer, typeof(IEqualityComparer));
			info.AddValue("ReadOnly", this._readOnly);
			info.AddValue("InitialCapacity", this._initialCapacity);
			object[] array = new object[this.Count];
			this._objectsArray.CopyTo(array);
			info.AddValue("ArrayList", array);
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x00045854 File Offset: 0x00044854
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			this.OnDeserialization(sender);
		}

		// Token: 0x040011A5 RID: 4517
		private const string KeyComparerName = "KeyComparer";

		// Token: 0x040011A6 RID: 4518
		private const string ArrayListName = "ArrayList";

		// Token: 0x040011A7 RID: 4519
		private const string ReadOnlyName = "ReadOnly";

		// Token: 0x040011A8 RID: 4520
		private const string InitCapacityName = "InitialCapacity";

		// Token: 0x040011A9 RID: 4521
		private ArrayList _objectsArray;

		// Token: 0x040011AA RID: 4522
		private Hashtable _objectsTable;

		// Token: 0x040011AB RID: 4523
		private int _initialCapacity;

		// Token: 0x040011AC RID: 4524
		private IEqualityComparer _comparer;

		// Token: 0x040011AD RID: 4525
		private bool _readOnly;

		// Token: 0x040011AE RID: 4526
		private object _syncRoot;

		// Token: 0x040011AF RID: 4527
		private SerializationInfo _siInfo;

		// Token: 0x0200025D RID: 605
		private class OrderedDictionaryEnumerator : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x060014F7 RID: 5367 RVA: 0x0004585D File Offset: 0x0004485D
			internal OrderedDictionaryEnumerator(ArrayList array, int objectReturnType)
			{
				this._objects = array;
				this._objectReturnType = objectReturnType;
			}

			// Token: 0x1700045B RID: 1115
			// (get) Token: 0x060014F8 RID: 5368 RVA: 0x0004587C File Offset: 0x0004487C
			public object Current
			{
				get
				{
					if (this._objectReturnType == 1)
					{
						return ((DictionaryEntry)this._objects[this._index]).Key;
					}
					if (this._objectReturnType == 2)
					{
						return ((DictionaryEntry)this._objects[this._index]).Value;
					}
					return this.Entry;
				}
			}

			// Token: 0x1700045C RID: 1116
			// (get) Token: 0x060014F9 RID: 5369 RVA: 0x000458E4 File Offset: 0x000448E4
			public DictionaryEntry Entry
			{
				get
				{
					if (this._index < 0 || this._index >= this._objects.Count)
					{
						throw new InvalidOperationException();
					}
					return new DictionaryEntry(((DictionaryEntry)this._objects[this._index]).Key, ((DictionaryEntry)this._objects[this._index]).Value);
				}
			}

			// Token: 0x1700045D RID: 1117
			// (get) Token: 0x060014FA RID: 5370 RVA: 0x00045954 File Offset: 0x00044954
			public object Key
			{
				get
				{
					if (this._index < 0 || this._index >= this._objects.Count)
					{
						throw new InvalidOperationException();
					}
					return ((DictionaryEntry)this._objects[this._index]).Key;
				}
			}

			// Token: 0x1700045E RID: 1118
			// (get) Token: 0x060014FB RID: 5371 RVA: 0x000459A4 File Offset: 0x000449A4
			public object Value
			{
				get
				{
					if (this._index < 0 || this._index >= this._objects.Count)
					{
						throw new InvalidOperationException();
					}
					return ((DictionaryEntry)this._objects[this._index]).Value;
				}
			}

			// Token: 0x060014FC RID: 5372 RVA: 0x000459F1 File Offset: 0x000449F1
			public bool MoveNext()
			{
				this._index++;
				return this._index < this._objects.Count;
			}

			// Token: 0x060014FD RID: 5373 RVA: 0x00045A17 File Offset: 0x00044A17
			public void Reset()
			{
				this._index = -1;
			}

			// Token: 0x040011B0 RID: 4528
			internal const int Keys = 1;

			// Token: 0x040011B1 RID: 4529
			internal const int Values = 2;

			// Token: 0x040011B2 RID: 4530
			internal const int DictionaryEntry = 3;

			// Token: 0x040011B3 RID: 4531
			private int _index = -1;

			// Token: 0x040011B4 RID: 4532
			private ArrayList _objects;

			// Token: 0x040011B5 RID: 4533
			private int _objectReturnType;
		}

		// Token: 0x0200025E RID: 606
		private class OrderedDictionaryKeyValueCollection : ICollection, IEnumerable
		{
			// Token: 0x060014FE RID: 5374 RVA: 0x00045A20 File Offset: 0x00044A20
			public OrderedDictionaryKeyValueCollection(ArrayList array, bool isKeys)
			{
				this._objects = array;
				this.isKeys = isKeys;
			}

			// Token: 0x060014FF RID: 5375 RVA: 0x00045A38 File Offset: 0x00044A38
			void ICollection.CopyTo(Array array, int index)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				foreach (object obj in this._objects)
				{
					array.SetValue(this.isKeys ? ((DictionaryEntry)obj).Key : ((DictionaryEntry)obj).Value, index);
					index++;
				}
			}

			// Token: 0x1700045F RID: 1119
			// (get) Token: 0x06001500 RID: 5376 RVA: 0x00045AD8 File Offset: 0x00044AD8
			int ICollection.Count
			{
				get
				{
					return this._objects.Count;
				}
			}

			// Token: 0x17000460 RID: 1120
			// (get) Token: 0x06001501 RID: 5377 RVA: 0x00045AE5 File Offset: 0x00044AE5
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000461 RID: 1121
			// (get) Token: 0x06001502 RID: 5378 RVA: 0x00045AE8 File Offset: 0x00044AE8
			object ICollection.SyncRoot
			{
				get
				{
					return this._objects.SyncRoot;
				}
			}

			// Token: 0x06001503 RID: 5379 RVA: 0x00045AF5 File Offset: 0x00044AF5
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new OrderedDictionary.OrderedDictionaryEnumerator(this._objects, this.isKeys ? 1 : 2);
			}

			// Token: 0x040011B6 RID: 4534
			private ArrayList _objects;

			// Token: 0x040011B7 RID: 4535
			private bool isKeys;
		}
	}
}
