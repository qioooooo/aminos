using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System.Collections.Specialized
{
	// Token: 0x02000256 RID: 598
	[Serializable]
	public abstract class NameObjectCollectionBase : ICollection, IEnumerable, ISerializable, IDeserializationCallback
	{
		// Token: 0x0600147F RID: 5247 RVA: 0x000440DA File Offset: 0x000430DA
		protected NameObjectCollectionBase()
			: this(NameObjectCollectionBase.defaultComparer)
		{
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x000440E7 File Offset: 0x000430E7
		protected NameObjectCollectionBase(IEqualityComparer equalityComparer)
		{
			this._keyComparer = ((equalityComparer == null) ? NameObjectCollectionBase.defaultComparer : equalityComparer);
			this.Reset();
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x00044106 File Offset: 0x00043106
		protected NameObjectCollectionBase(int capacity, IEqualityComparer equalityComparer)
			: this(equalityComparer)
		{
			this.Reset(capacity);
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x00044116 File Offset: 0x00043116
		[Obsolete("Please use NameObjectCollectionBase(IEqualityComparer) instead.")]
		protected NameObjectCollectionBase(IHashCodeProvider hashProvider, IComparer comparer)
		{
			this._keyComparer = new CompatibleComparer(comparer, hashProvider);
			this.Reset();
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x00044131 File Offset: 0x00043131
		[Obsolete("Please use NameObjectCollectionBase(Int32, IEqualityComparer) instead.")]
		protected NameObjectCollectionBase(int capacity, IHashCodeProvider hashProvider, IComparer comparer)
		{
			this._keyComparer = new CompatibleComparer(comparer, hashProvider);
			this.Reset(capacity);
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x0004414D File Offset: 0x0004314D
		protected NameObjectCollectionBase(int capacity)
		{
			this._keyComparer = StringComparer.InvariantCultureIgnoreCase;
			this.Reset(capacity);
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x00044167 File Offset: 0x00043167
		internal NameObjectCollectionBase(DBNull dummy)
		{
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x0004416F File Offset: 0x0004316F
		protected NameObjectCollectionBase(SerializationInfo info, StreamingContext context)
		{
			this._serializationInfo = info;
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x00044180 File Offset: 0x00043180
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("ReadOnly", this._readOnly);
			if (this._keyComparer == NameObjectCollectionBase.defaultComparer)
			{
				info.AddValue("HashProvider", CompatibleComparer.DefaultHashCodeProvider, typeof(IHashCodeProvider));
				info.AddValue("Comparer", CompatibleComparer.DefaultComparer, typeof(IComparer));
			}
			else if (this._keyComparer == null)
			{
				info.AddValue("HashProvider", null, typeof(IHashCodeProvider));
				info.AddValue("Comparer", null, typeof(IComparer));
			}
			else if (this._keyComparer is CompatibleComparer)
			{
				CompatibleComparer compatibleComparer = (CompatibleComparer)this._keyComparer;
				info.AddValue("HashProvider", compatibleComparer.HashCodeProvider, typeof(IHashCodeProvider));
				info.AddValue("Comparer", compatibleComparer.Comparer, typeof(IComparer));
			}
			else
			{
				info.AddValue("KeyComparer", this._keyComparer, typeof(IEqualityComparer));
			}
			int count = this._entriesArray.Count;
			info.AddValue("Count", count);
			string[] array = new string[count];
			object[] array2 = new object[count];
			for (int i = 0; i < count; i++)
			{
				NameObjectCollectionBase.NameObjectEntry nameObjectEntry = (NameObjectCollectionBase.NameObjectEntry)this._entriesArray[i];
				array[i] = nameObjectEntry.Key;
				array2[i] = nameObjectEntry.Value;
			}
			info.AddValue("Keys", array, typeof(string[]));
			info.AddValue("Values", array2, typeof(object[]));
			info.AddValue("Version", this._version);
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x00044334 File Offset: 0x00043334
		public virtual void OnDeserialization(object sender)
		{
			if (this._keyComparer != null)
			{
				return;
			}
			if (this._serializationInfo == null)
			{
				throw new SerializationException();
			}
			SerializationInfo serializationInfo = this._serializationInfo;
			this._serializationInfo = null;
			bool flag = false;
			int num = 0;
			string[] array = null;
			object[] array2 = null;
			IHashCodeProvider hashCodeProvider = null;
			IComparer comparer = null;
			bool flag2 = false;
			int num2 = 0;
			SerializationInfoEnumerator enumerator = serializationInfo.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string name;
				switch (name = enumerator.Name)
				{
				case "ReadOnly":
					flag = serializationInfo.GetBoolean("ReadOnly");
					break;
				case "HashProvider":
					hashCodeProvider = (IHashCodeProvider)serializationInfo.GetValue("HashProvider", typeof(IHashCodeProvider));
					break;
				case "Comparer":
					comparer = (IComparer)serializationInfo.GetValue("Comparer", typeof(IComparer));
					break;
				case "KeyComparer":
					this._keyComparer = (IEqualityComparer)serializationInfo.GetValue("KeyComparer", typeof(IEqualityComparer));
					break;
				case "Count":
					num = serializationInfo.GetInt32("Count");
					break;
				case "Keys":
					array = (string[])serializationInfo.GetValue("Keys", typeof(string[]));
					break;
				case "Values":
					array2 = (object[])serializationInfo.GetValue("Values", typeof(object[]));
					break;
				case "Version":
					flag2 = true;
					num2 = serializationInfo.GetInt32("Version");
					break;
				}
			}
			if (this._keyComparer == null)
			{
				if (comparer == null || hashCodeProvider == null)
				{
					throw new SerializationException();
				}
				this._keyComparer = new CompatibleComparer(comparer, hashCodeProvider);
			}
			if (array == null || array2 == null)
			{
				throw new SerializationException();
			}
			this.Reset(num);
			for (int i = 0; i < num; i++)
			{
				this.BaseAdd(array[i], array2[i]);
			}
			this._readOnly = flag;
			if (flag2)
			{
				this._version = num2;
			}
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x00044590 File Offset: 0x00043590
		private void Reset()
		{
			this._entriesArray = new ArrayList();
			this._entriesTable = new Hashtable(this._keyComparer);
			this._nullKeyEntry = null;
			this._version++;
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x000445C3 File Offset: 0x000435C3
		private void Reset(int capacity)
		{
			this._entriesArray = new ArrayList(capacity);
			this._entriesTable = new Hashtable(capacity, this._keyComparer);
			this._nullKeyEntry = null;
			this._version++;
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x000445F8 File Offset: 0x000435F8
		private NameObjectCollectionBase.NameObjectEntry FindEntry(string key)
		{
			if (key != null)
			{
				return (NameObjectCollectionBase.NameObjectEntry)this._entriesTable[key];
			}
			return this._nullKeyEntry;
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x0600148C RID: 5260 RVA: 0x00044615 File Offset: 0x00043615
		// (set) Token: 0x0600148D RID: 5261 RVA: 0x0004461D File Offset: 0x0004361D
		internal IEqualityComparer Comparer
		{
			get
			{
				return this._keyComparer;
			}
			set
			{
				this._keyComparer = value;
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x0600148E RID: 5262 RVA: 0x00044626 File Offset: 0x00043626
		// (set) Token: 0x0600148F RID: 5263 RVA: 0x0004462E File Offset: 0x0004362E
		protected bool IsReadOnly
		{
			get
			{
				return this._readOnly;
			}
			set
			{
				this._readOnly = value;
			}
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x00044637 File Offset: 0x00043637
		protected bool BaseHasKeys()
		{
			return this._entriesTable.Count > 0;
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x00044648 File Offset: 0x00043648
		protected void BaseAdd(string name, object value)
		{
			if (this._readOnly)
			{
				throw new NotSupportedException(SR.GetString("CollectionReadOnly"));
			}
			NameObjectCollectionBase.NameObjectEntry nameObjectEntry = new NameObjectCollectionBase.NameObjectEntry(name, value);
			if (name != null)
			{
				if (this._entriesTable[name] == null)
				{
					this._entriesTable.Add(name, nameObjectEntry);
				}
			}
			else if (this._nullKeyEntry == null)
			{
				this._nullKeyEntry = nameObjectEntry;
			}
			this._entriesArray.Add(nameObjectEntry);
			this._version++;
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x000446C0 File Offset: 0x000436C0
		protected void BaseRemove(string name)
		{
			if (this._readOnly)
			{
				throw new NotSupportedException(SR.GetString("CollectionReadOnly"));
			}
			if (name != null)
			{
				this._entriesTable.Remove(name);
				for (int i = this._entriesArray.Count - 1; i >= 0; i--)
				{
					if (this._keyComparer.Equals(name, this.BaseGetKey(i)))
					{
						this._entriesArray.RemoveAt(i);
					}
				}
			}
			else
			{
				this._nullKeyEntry = null;
				for (int j = this._entriesArray.Count - 1; j >= 0; j--)
				{
					if (this.BaseGetKey(j) == null)
					{
						this._entriesArray.RemoveAt(j);
					}
				}
			}
			this._version++;
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x00044774 File Offset: 0x00043774
		protected void BaseRemoveAt(int index)
		{
			if (this._readOnly)
			{
				throw new NotSupportedException(SR.GetString("CollectionReadOnly"));
			}
			string text = this.BaseGetKey(index);
			if (text != null)
			{
				this._entriesTable.Remove(text);
			}
			else
			{
				this._nullKeyEntry = null;
			}
			this._entriesArray.RemoveAt(index);
			this._version++;
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x000447D3 File Offset: 0x000437D3
		protected void BaseClear()
		{
			if (this._readOnly)
			{
				throw new NotSupportedException(SR.GetString("CollectionReadOnly"));
			}
			this.Reset();
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x000447F4 File Offset: 0x000437F4
		protected object BaseGet(string name)
		{
			NameObjectCollectionBase.NameObjectEntry nameObjectEntry = this.FindEntry(name);
			if (nameObjectEntry == null)
			{
				return null;
			}
			return nameObjectEntry.Value;
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x00044814 File Offset: 0x00043814
		protected void BaseSet(string name, object value)
		{
			if (this._readOnly)
			{
				throw new NotSupportedException(SR.GetString("CollectionReadOnly"));
			}
			NameObjectCollectionBase.NameObjectEntry nameObjectEntry = this.FindEntry(name);
			if (nameObjectEntry != null)
			{
				nameObjectEntry.Value = value;
				this._version++;
				return;
			}
			this.BaseAdd(name, value);
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x00044864 File Offset: 0x00043864
		protected object BaseGet(int index)
		{
			NameObjectCollectionBase.NameObjectEntry nameObjectEntry = (NameObjectCollectionBase.NameObjectEntry)this._entriesArray[index];
			return nameObjectEntry.Value;
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x0004488C File Offset: 0x0004388C
		protected string BaseGetKey(int index)
		{
			NameObjectCollectionBase.NameObjectEntry nameObjectEntry = (NameObjectCollectionBase.NameObjectEntry)this._entriesArray[index];
			return nameObjectEntry.Key;
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x000448B4 File Offset: 0x000438B4
		protected void BaseSet(int index, object value)
		{
			if (this._readOnly)
			{
				throw new NotSupportedException(SR.GetString("CollectionReadOnly"));
			}
			NameObjectCollectionBase.NameObjectEntry nameObjectEntry = (NameObjectCollectionBase.NameObjectEntry)this._entriesArray[index];
			nameObjectEntry.Value = value;
			this._version++;
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x00044900 File Offset: 0x00043900
		public virtual IEnumerator GetEnumerator()
		{
			return new NameObjectCollectionBase.NameObjectKeysEnumerator(this);
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x0600149B RID: 5275 RVA: 0x00044908 File Offset: 0x00043908
		public virtual int Count
		{
			get
			{
				return this._entriesArray.Count;
			}
		}

		// Token: 0x0600149C RID: 5276 RVA: 0x00044918 File Offset: 0x00043918
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(SR.GetString("Arg_MultiRank"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("IndexOutOfRange", new object[] { index.ToString(CultureInfo.CurrentCulture) }));
			}
			if (array.Length - index < this._entriesArray.Count)
			{
				throw new ArgumentException(SR.GetString("Arg_InsufficientSpace"));
			}
			foreach (object obj in this)
			{
				array.SetValue(obj, index++);
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x0600149D RID: 5277 RVA: 0x000449C4 File Offset: 0x000439C4
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

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x0600149E RID: 5278 RVA: 0x000449E6 File Offset: 0x000439E6
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x000449EC File Offset: 0x000439EC
		protected string[] BaseGetAllKeys()
		{
			int count = this._entriesArray.Count;
			string[] array = new string[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = this.BaseGetKey(i);
			}
			return array;
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x00044A24 File Offset: 0x00043A24
		protected object[] BaseGetAllValues()
		{
			int count = this._entriesArray.Count;
			object[] array = new object[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = this.BaseGet(i);
			}
			return array;
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x00044A5C File Offset: 0x00043A5C
		protected object[] BaseGetAllValues(Type type)
		{
			int count = this._entriesArray.Count;
			object[] array = (object[])Array.CreateInstance(type, count);
			for (int i = 0; i < count; i++)
			{
				array[i] = this.BaseGet(i);
			}
			return array;
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x060014A2 RID: 5282 RVA: 0x00044A99 File Offset: 0x00043A99
		public virtual NameObjectCollectionBase.KeysCollection Keys
		{
			get
			{
				if (this._keys == null)
				{
					this._keys = new NameObjectCollectionBase.KeysCollection(this);
				}
				return this._keys;
			}
		}

		// Token: 0x04001187 RID: 4487
		private const string ReadOnlyName = "ReadOnly";

		// Token: 0x04001188 RID: 4488
		private const string CountName = "Count";

		// Token: 0x04001189 RID: 4489
		private const string ComparerName = "Comparer";

		// Token: 0x0400118A RID: 4490
		private const string HashCodeProviderName = "HashProvider";

		// Token: 0x0400118B RID: 4491
		private const string KeysName = "Keys";

		// Token: 0x0400118C RID: 4492
		private const string ValuesName = "Values";

		// Token: 0x0400118D RID: 4493
		private const string KeyComparerName = "KeyComparer";

		// Token: 0x0400118E RID: 4494
		private const string VersionName = "Version";

		// Token: 0x0400118F RID: 4495
		private bool _readOnly;

		// Token: 0x04001190 RID: 4496
		private ArrayList _entriesArray;

		// Token: 0x04001191 RID: 4497
		private IEqualityComparer _keyComparer;

		// Token: 0x04001192 RID: 4498
		private Hashtable _entriesTable;

		// Token: 0x04001193 RID: 4499
		private NameObjectCollectionBase.NameObjectEntry _nullKeyEntry;

		// Token: 0x04001194 RID: 4500
		private NameObjectCollectionBase.KeysCollection _keys;

		// Token: 0x04001195 RID: 4501
		private SerializationInfo _serializationInfo;

		// Token: 0x04001196 RID: 4502
		private int _version;

		// Token: 0x04001197 RID: 4503
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x04001198 RID: 4504
		private static StringComparer defaultComparer = StringComparer.InvariantCultureIgnoreCase;

		// Token: 0x02000257 RID: 599
		internal class NameObjectEntry
		{
			// Token: 0x060014A4 RID: 5284 RVA: 0x00044AC1 File Offset: 0x00043AC1
			internal NameObjectEntry(string name, object value)
			{
				this.Key = name;
				this.Value = value;
			}

			// Token: 0x04001199 RID: 4505
			internal string Key;

			// Token: 0x0400119A RID: 4506
			internal object Value;
		}

		// Token: 0x02000258 RID: 600
		[Serializable]
		internal class NameObjectKeysEnumerator : IEnumerator
		{
			// Token: 0x060014A5 RID: 5285 RVA: 0x00044AD7 File Offset: 0x00043AD7
			internal NameObjectKeysEnumerator(NameObjectCollectionBase coll)
			{
				this._coll = coll;
				this._version = this._coll._version;
				this._pos = -1;
			}

			// Token: 0x060014A6 RID: 5286 RVA: 0x00044B00 File Offset: 0x00043B00
			public bool MoveNext()
			{
				if (this._version != this._coll._version)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
				}
				if (this._pos < this._coll.Count - 1)
				{
					this._pos++;
					return true;
				}
				this._pos = this._coll.Count;
				return false;
			}

			// Token: 0x060014A7 RID: 5287 RVA: 0x00044B67 File Offset: 0x00043B67
			public void Reset()
			{
				if (this._version != this._coll._version)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
				}
				this._pos = -1;
			}

			// Token: 0x17000444 RID: 1092
			// (get) Token: 0x060014A8 RID: 5288 RVA: 0x00044B93 File Offset: 0x00043B93
			public object Current
			{
				get
				{
					if (this._pos >= 0 && this._pos < this._coll.Count)
					{
						return this._coll.BaseGetKey(this._pos);
					}
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumOpCantHappen"));
				}
			}

			// Token: 0x0400119B RID: 4507
			private int _pos;

			// Token: 0x0400119C RID: 4508
			private NameObjectCollectionBase _coll;

			// Token: 0x0400119D RID: 4509
			private int _version;
		}

		// Token: 0x02000259 RID: 601
		[Serializable]
		public class KeysCollection : ICollection, IEnumerable
		{
			// Token: 0x060014A9 RID: 5289 RVA: 0x00044BD2 File Offset: 0x00043BD2
			internal KeysCollection(NameObjectCollectionBase coll)
			{
				this._coll = coll;
			}

			// Token: 0x060014AA RID: 5290 RVA: 0x00044BE1 File Offset: 0x00043BE1
			public virtual string Get(int index)
			{
				return this._coll.BaseGetKey(index);
			}

			// Token: 0x17000445 RID: 1093
			public string this[int index]
			{
				get
				{
					return this.Get(index);
				}
			}

			// Token: 0x060014AC RID: 5292 RVA: 0x00044BF8 File Offset: 0x00043BF8
			public IEnumerator GetEnumerator()
			{
				return new NameObjectCollectionBase.NameObjectKeysEnumerator(this._coll);
			}

			// Token: 0x17000446 RID: 1094
			// (get) Token: 0x060014AD RID: 5293 RVA: 0x00044C05 File Offset: 0x00043C05
			public int Count
			{
				get
				{
					return this._coll.Count;
				}
			}

			// Token: 0x060014AE RID: 5294 RVA: 0x00044C14 File Offset: 0x00043C14
			void ICollection.CopyTo(Array array, int index)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (array.Rank != 1)
				{
					throw new ArgumentException(SR.GetString("Arg_MultiRank"));
				}
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("IndexOutOfRange", new object[] { index.ToString(CultureInfo.CurrentCulture) }));
				}
				if (array.Length - index < this._coll.Count)
				{
					throw new ArgumentException(SR.GetString("Arg_InsufficientSpace"));
				}
				foreach (object obj in this)
				{
					array.SetValue(obj, index++);
				}
			}

			// Token: 0x17000447 RID: 1095
			// (get) Token: 0x060014AF RID: 5295 RVA: 0x00044CC0 File Offset: 0x00043CC0
			object ICollection.SyncRoot
			{
				get
				{
					return ((ICollection)this._coll).SyncRoot;
				}
			}

			// Token: 0x17000448 RID: 1096
			// (get) Token: 0x060014B0 RID: 5296 RVA: 0x00044CCD File Offset: 0x00043CCD
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x0400119E RID: 4510
			private NameObjectCollectionBase _coll;
		}
	}
}
