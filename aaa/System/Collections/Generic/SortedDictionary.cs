using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x0200023F RID: 575
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(System_DictionaryDebugView<, >))]
	[Serializable]
	public class SortedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary, ICollection, IEnumerable
	{
		// Token: 0x0600137E RID: 4990 RVA: 0x000415B2 File Offset: 0x000405B2
		public SortedDictionary()
			: this(null)
		{
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x000415BB File Offset: 0x000405BB
		public SortedDictionary(IDictionary<TKey, TValue> dictionary)
			: this(dictionary, null)
		{
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x000415C8 File Offset: 0x000405C8
		public SortedDictionary(IDictionary<TKey, TValue> dictionary, IComparer<TKey> comparer)
		{
			if (dictionary == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
			}
			this._set = new TreeSet<KeyValuePair<TKey, TValue>>(new SortedDictionary<TKey, TValue>.KeyValuePairComparer(comparer));
			foreach (KeyValuePair<TKey, TValue> keyValuePair in dictionary)
			{
				this._set.Add(keyValuePair);
			}
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x00041638 File Offset: 0x00040638
		public SortedDictionary(IComparer<TKey> comparer)
		{
			this._set = new TreeSet<KeyValuePair<TKey, TValue>>(new SortedDictionary<TKey, TValue>.KeyValuePairComparer(comparer));
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x00041651 File Offset: 0x00040651
		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
		{
			this._set.Add(keyValuePair);
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x00041660 File Offset: 0x00040660
		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> keyValuePair)
		{
			TreeSet<KeyValuePair<TKey, TValue>>.Node node = this._set.FindNode(keyValuePair);
			if (node == null)
			{
				return false;
			}
			if (keyValuePair.Value == null)
			{
				return node.Item.Value == null;
			}
			return EqualityComparer<TValue>.Default.Equals(node.Item.Value, keyValuePair.Value);
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x000416C4 File Offset: 0x000406C4
		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> keyValuePair)
		{
			TreeSet<KeyValuePair<TKey, TValue>>.Node node = this._set.FindNode(keyValuePair);
			if (node == null)
			{
				return false;
			}
			if (EqualityComparer<TValue>.Default.Equals(node.Item.Value, keyValuePair.Value))
			{
				this._set.Remove(keyValuePair);
				return true;
			}
			return false;
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001385 RID: 4997 RVA: 0x00041714 File Offset: 0x00040714
		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003F3 RID: 1011
		public TValue this[TKey key]
		{
			get
			{
				if (key == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
				}
				TreeSet<KeyValuePair<TKey, TValue>>.Node node = this._set.FindNode(new KeyValuePair<TKey, TValue>(key, default(TValue)));
				if (node == null)
				{
					ThrowHelper.ThrowKeyNotFoundException();
				}
				return node.Item.Value;
			}
			set
			{
				if (key == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
				}
				TreeSet<KeyValuePair<TKey, TValue>>.Node node = this._set.FindNode(new KeyValuePair<TKey, TValue>(key, default(TValue)));
				if (node == null)
				{
					this._set.Add(new KeyValuePair<TKey, TValue>(key, value));
					return;
				}
				node.Item = new KeyValuePair<TKey, TValue>(node.Item.Key, value);
				this._set.UpdateVersion();
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06001388 RID: 5000 RVA: 0x000417D5 File Offset: 0x000407D5
		public int Count
		{
			get
			{
				return this._set.Count;
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06001389 RID: 5001 RVA: 0x000417E2 File Offset: 0x000407E2
		public IComparer<TKey> Comparer
		{
			get
			{
				return ((SortedDictionary<TKey, TValue>.KeyValuePairComparer)this._set.Comparer).keyComparer;
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x0600138A RID: 5002 RVA: 0x000417F9 File Offset: 0x000407F9
		public SortedDictionary<TKey, TValue>.KeyCollection Keys
		{
			get
			{
				if (this.keys == null)
				{
					this.keys = new SortedDictionary<TKey, TValue>.KeyCollection(this);
				}
				return this.keys;
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x0600138B RID: 5003 RVA: 0x00041815 File Offset: 0x00040815
		ICollection<TKey> IDictionary<TKey, TValue>.Keys
		{
			get
			{
				return this.Keys;
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x0600138C RID: 5004 RVA: 0x0004181D File Offset: 0x0004081D
		public SortedDictionary<TKey, TValue>.ValueCollection Values
		{
			get
			{
				if (this.values == null)
				{
					this.values = new SortedDictionary<TKey, TValue>.ValueCollection(this);
				}
				return this.values;
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x0600138D RID: 5005 RVA: 0x00041839 File Offset: 0x00040839
		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			get
			{
				return this.Values;
			}
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x00041841 File Offset: 0x00040841
		public void Add(TKey key, TValue value)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			this._set.Add(new KeyValuePair<TKey, TValue>(key, value));
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x00041863 File Offset: 0x00040863
		public void Clear()
		{
			this._set.Clear();
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x00041870 File Offset: 0x00040870
		public bool ContainsKey(TKey key)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			return this._set.Contains(new KeyValuePair<TKey, TValue>(key, default(TValue)));
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x00041928 File Offset: 0x00040928
		public bool ContainsValue(TValue value)
		{
			bool found = false;
			if (value == null)
			{
				this._set.InOrderTreeWalk(delegate(TreeSet<KeyValuePair<TKey, TValue>>.Node node)
				{
					if (node.Item.Value == null)
					{
						found = true;
						return false;
					}
					return true;
				});
			}
			else
			{
				EqualityComparer<TValue> valueComparer = EqualityComparer<TValue>.Default;
				this._set.InOrderTreeWalk(delegate(TreeSet<KeyValuePair<TKey, TValue>>.Node node)
				{
					if (valueComparer.Equals(node.Item.Value, value))
					{
						found = true;
						return false;
					}
					return true;
				});
			}
			return found;
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x000419AD File Offset: 0x000409AD
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
		{
			this._set.CopyTo(array, index);
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x000419BC File Offset: 0x000409BC
		public SortedDictionary<TKey, TValue>.Enumerator GetEnumerator()
		{
			return new SortedDictionary<TKey, TValue>.Enumerator(this, 1);
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x000419C5 File Offset: 0x000409C5
		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return new SortedDictionary<TKey, TValue>.Enumerator(this, 1);
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x000419D4 File Offset: 0x000409D4
		public bool Remove(TKey key)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			return this._set.Remove(new KeyValuePair<TKey, TValue>(key, default(TValue)));
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x00041A0C File Offset: 0x00040A0C
		public bool TryGetValue(TKey key, out TValue value)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			TreeSet<KeyValuePair<TKey, TValue>>.Node node = this._set.FindNode(new KeyValuePair<TKey, TValue>(key, default(TValue)));
			if (node == null)
			{
				value = default(TValue);
				return false;
			}
			value = node.Item.Value;
			return true;
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x00041A63 File Offset: 0x00040A63
		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection)this._set).CopyTo(array, index);
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06001398 RID: 5016 RVA: 0x00041A72 File Offset: 0x00040A72
		bool IDictionary.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06001399 RID: 5017 RVA: 0x00041A75 File Offset: 0x00040A75
		bool IDictionary.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x0600139A RID: 5018 RVA: 0x00041A78 File Offset: 0x00040A78
		ICollection IDictionary.Keys
		{
			get
			{
				return this.Keys;
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x0600139B RID: 5019 RVA: 0x00041A80 File Offset: 0x00040A80
		ICollection IDictionary.Values
		{
			get
			{
				return this.Values;
			}
		}

		// Token: 0x170003FE RID: 1022
		object IDictionary.this[object key]
		{
			get
			{
				TValue tvalue;
				if (SortedDictionary<TKey, TValue>.IsCompatibleKey(key) && this.TryGetValue((TKey)((object)key), out tvalue))
				{
					return tvalue;
				}
				return null;
			}
			set
			{
				SortedDictionary<TKey, TValue>.VerifyKey(key);
				SortedDictionary<TKey, TValue>.VerifyValueType(value);
				this[(TKey)((object)key)] = (TValue)((object)value);
			}
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x00041AD5 File Offset: 0x00040AD5
		void IDictionary.Add(object key, object value)
		{
			SortedDictionary<TKey, TValue>.VerifyKey(key);
			SortedDictionary<TKey, TValue>.VerifyValueType(value);
			this.Add((TKey)((object)key), (TValue)((object)value));
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x00041AF5 File Offset: 0x00040AF5
		bool IDictionary.Contains(object key)
		{
			return SortedDictionary<TKey, TValue>.IsCompatibleKey(key) && this.ContainsKey((TKey)((object)key));
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x00041B0D File Offset: 0x00040B0D
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

		// Token: 0x060013A1 RID: 5025 RVA: 0x00041B30 File Offset: 0x00040B30
		private static bool IsCompatibleKey(object key)
		{
			if (key == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
			}
			return key is TKey;
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x00041B44 File Offset: 0x00040B44
		private static void VerifyValueType(object value)
		{
			if (value is TValue || (value == null && !typeof(TValue).IsValueType))
			{
				return;
			}
			ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(TValue));
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x00041B73 File Offset: 0x00040B73
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new SortedDictionary<TKey, TValue>.Enumerator(this, 2);
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x00041B81 File Offset: 0x00040B81
		void IDictionary.Remove(object key)
		{
			if (SortedDictionary<TKey, TValue>.IsCompatibleKey(key))
			{
				this.Remove((TKey)((object)key));
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x060013A5 RID: 5029 RVA: 0x00041B98 File Offset: 0x00040B98
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x060013A6 RID: 5030 RVA: 0x00041B9B File Offset: 0x00040B9B
		object ICollection.SyncRoot
		{
			get
			{
				return ((ICollection)this._set).SyncRoot;
			}
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x00041BA8 File Offset: 0x00040BA8
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new SortedDictionary<TKey, TValue>.Enumerator(this, 1);
		}

		// Token: 0x04001143 RID: 4419
		[NonSerialized]
		private SortedDictionary<TKey, TValue>.KeyCollection keys;

		// Token: 0x04001144 RID: 4420
		[NonSerialized]
		private SortedDictionary<TKey, TValue>.ValueCollection values;

		// Token: 0x04001145 RID: 4421
		private TreeSet<KeyValuePair<TKey, TValue>> _set;

		// Token: 0x02000240 RID: 576
		public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDisposable, IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x060013A8 RID: 5032 RVA: 0x00041BB6 File Offset: 0x00040BB6
			internal Enumerator(SortedDictionary<TKey, TValue> dictionary, int getEnumeratorRetType)
			{
				this.treeEnum = dictionary._set.GetEnumerator();
				this.getEnumeratorRetType = getEnumeratorRetType;
			}

			// Token: 0x060013A9 RID: 5033 RVA: 0x00041BD0 File Offset: 0x00040BD0
			public bool MoveNext()
			{
				return this.treeEnum.MoveNext();
			}

			// Token: 0x060013AA RID: 5034 RVA: 0x00041BDD File Offset: 0x00040BDD
			public void Dispose()
			{
				this.treeEnum.Dispose();
			}

			// Token: 0x17000401 RID: 1025
			// (get) Token: 0x060013AB RID: 5035 RVA: 0x00041BEA File Offset: 0x00040BEA
			public KeyValuePair<TKey, TValue> Current
			{
				get
				{
					return this.treeEnum.Current;
				}
			}

			// Token: 0x17000402 RID: 1026
			// (get) Token: 0x060013AC RID: 5036 RVA: 0x00041BF7 File Offset: 0x00040BF7
			internal bool NotStartedOrEnded
			{
				get
				{
					return this.treeEnum.NotStartedOrEnded;
				}
			}

			// Token: 0x060013AD RID: 5037 RVA: 0x00041C04 File Offset: 0x00040C04
			internal void Reset()
			{
				this.treeEnum.Reset();
			}

			// Token: 0x060013AE RID: 5038 RVA: 0x00041C11 File Offset: 0x00040C11
			void IEnumerator.Reset()
			{
				this.treeEnum.Reset();
			}

			// Token: 0x17000403 RID: 1027
			// (get) Token: 0x060013AF RID: 5039 RVA: 0x00041C20 File Offset: 0x00040C20
			object IEnumerator.Current
			{
				get
				{
					if (this.NotStartedOrEnded)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					if (this.getEnumeratorRetType == 2)
					{
						KeyValuePair<TKey, TValue> keyValuePair = this.Current;
						object obj = keyValuePair.Key;
						KeyValuePair<TKey, TValue> keyValuePair2 = this.Current;
						return new DictionaryEntry(obj, keyValuePair2.Value);
					}
					KeyValuePair<TKey, TValue> keyValuePair3 = this.Current;
					TKey key = keyValuePair3.Key;
					KeyValuePair<TKey, TValue> keyValuePair4 = this.Current;
					return new KeyValuePair<TKey, TValue>(key, keyValuePair4.Value);
				}
			}

			// Token: 0x17000404 RID: 1028
			// (get) Token: 0x060013B0 RID: 5040 RVA: 0x00041C9C File Offset: 0x00040C9C
			object IDictionaryEnumerator.Key
			{
				get
				{
					if (this.NotStartedOrEnded)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					KeyValuePair<TKey, TValue> keyValuePair = this.Current;
					return keyValuePair.Key;
				}
			}

			// Token: 0x17000405 RID: 1029
			// (get) Token: 0x060013B1 RID: 5041 RVA: 0x00041CCC File Offset: 0x00040CCC
			object IDictionaryEnumerator.Value
			{
				get
				{
					if (this.NotStartedOrEnded)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					KeyValuePair<TKey, TValue> keyValuePair = this.Current;
					return keyValuePair.Value;
				}
			}

			// Token: 0x17000406 RID: 1030
			// (get) Token: 0x060013B2 RID: 5042 RVA: 0x00041CFC File Offset: 0x00040CFC
			DictionaryEntry IDictionaryEnumerator.Entry
			{
				get
				{
					if (this.NotStartedOrEnded)
					{
						ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
					}
					KeyValuePair<TKey, TValue> keyValuePair = this.Current;
					object obj = keyValuePair.Key;
					KeyValuePair<TKey, TValue> keyValuePair2 = this.Current;
					return new DictionaryEntry(obj, keyValuePair2.Value);
				}
			}

			// Token: 0x04001146 RID: 4422
			internal const int KeyValuePair = 1;

			// Token: 0x04001147 RID: 4423
			internal const int DictEntry = 2;

			// Token: 0x04001148 RID: 4424
			private TreeSet<KeyValuePair<TKey, TValue>>.Enumerator treeEnum;

			// Token: 0x04001149 RID: 4425
			private int getEnumeratorRetType;
		}

		// Token: 0x02000241 RID: 577
		[DebuggerTypeProxy(typeof(System_DictionaryKeyCollectionDebugView<, >))]
		[DebuggerDisplay("Count = {Count}")]
		[Serializable]
		public sealed class KeyCollection : ICollection<TKey>, IEnumerable<TKey>, ICollection, IEnumerable
		{
			// Token: 0x060013B3 RID: 5043 RVA: 0x00041D43 File Offset: 0x00040D43
			public KeyCollection(SortedDictionary<TKey, TValue> dictionary)
			{
				if (dictionary == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
				}
				this.dictionary = dictionary;
			}

			// Token: 0x060013B4 RID: 5044 RVA: 0x00041D5B File Offset: 0x00040D5B
			public SortedDictionary<TKey, TValue>.KeyCollection.Enumerator GetEnumerator()
			{
				return new SortedDictionary<TKey, TValue>.KeyCollection.Enumerator(this.dictionary);
			}

			// Token: 0x060013B5 RID: 5045 RVA: 0x00041D68 File Offset: 0x00040D68
			IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
			{
				return new SortedDictionary<TKey, TValue>.KeyCollection.Enumerator(this.dictionary);
			}

			// Token: 0x060013B6 RID: 5046 RVA: 0x00041D7A File Offset: 0x00040D7A
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new SortedDictionary<TKey, TValue>.KeyCollection.Enumerator(this.dictionary);
			}

			// Token: 0x060013B7 RID: 5047 RVA: 0x00041DCC File Offset: 0x00040DCC
			public void CopyTo(TKey[] array, int index)
			{
				if (array == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
				}
				if (index < 0)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
				}
				if (array.Length - index < this.Count)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				this.dictionary._set.InOrderTreeWalk(delegate(TreeSet<KeyValuePair<TKey, TValue>>.Node node)
				{
					array[index++] = node.Item.Key;
					return true;
				});
			}

			// Token: 0x060013B8 RID: 5048 RVA: 0x00041E98 File Offset: 0x00040E98
			void ICollection.CopyTo(Array array, int index)
			{
				SortedDictionary<TKey, TValue>.KeyCollection.<>c__DisplayClassb CS$<>8__locals1 = new SortedDictionary<TKey, TValue>.KeyCollection.<>c__DisplayClassb();
				CS$<>8__locals1.index = index;
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
				if (CS$<>8__locals1.index < 0)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.arrayIndex, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
				}
				if (array.Length - CS$<>8__locals1.index < this.dictionary.Count)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				TKey[] array2 = array as TKey[];
				if (array2 != null)
				{
					this.CopyTo(array2, CS$<>8__locals1.index);
					return;
				}
				object[] objects = (object[])array;
				if (objects == null)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
				}
				try
				{
					this.dictionary._set.InOrderTreeWalk(delegate(TreeSet<KeyValuePair<TKey, TValue>>.Node node)
					{
						objects[CS$<>8__locals1.index++] = node.Item.Key;
						return true;
					});
				}
				catch (ArrayTypeMismatchException)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
				}
			}

			// Token: 0x17000407 RID: 1031
			// (get) Token: 0x060013B9 RID: 5049 RVA: 0x00041F8C File Offset: 0x00040F8C
			public int Count
			{
				get
				{
					return this.dictionary.Count;
				}
			}

			// Token: 0x17000408 RID: 1032
			// (get) Token: 0x060013BA RID: 5050 RVA: 0x00041F99 File Offset: 0x00040F99
			bool ICollection<TKey>.IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060013BB RID: 5051 RVA: 0x00041F9C File Offset: 0x00040F9C
			void ICollection<TKey>.Add(TKey item)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
			}

			// Token: 0x060013BC RID: 5052 RVA: 0x00041FA5 File Offset: 0x00040FA5
			void ICollection<TKey>.Clear()
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
			}

			// Token: 0x060013BD RID: 5053 RVA: 0x00041FAE File Offset: 0x00040FAE
			bool ICollection<TKey>.Contains(TKey item)
			{
				return this.dictionary.ContainsKey(item);
			}

			// Token: 0x060013BE RID: 5054 RVA: 0x00041FBC File Offset: 0x00040FBC
			bool ICollection<TKey>.Remove(TKey item)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_KeyCollectionSet);
				return false;
			}

			// Token: 0x17000409 RID: 1033
			// (get) Token: 0x060013BF RID: 5055 RVA: 0x00041FC6 File Offset: 0x00040FC6
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700040A RID: 1034
			// (get) Token: 0x060013C0 RID: 5056 RVA: 0x00041FC9 File Offset: 0x00040FC9
			object ICollection.SyncRoot
			{
				get
				{
					return ((ICollection)this.dictionary).SyncRoot;
				}
			}

			// Token: 0x0400114A RID: 4426
			private SortedDictionary<TKey, TValue> dictionary;

			// Token: 0x02000242 RID: 578
			public struct Enumerator : IEnumerator<TKey>, IDisposable, IEnumerator
			{
				// Token: 0x060013C1 RID: 5057 RVA: 0x00041FD6 File Offset: 0x00040FD6
				internal Enumerator(SortedDictionary<TKey, TValue> dictionary)
				{
					this.dictEnum = dictionary.GetEnumerator();
				}

				// Token: 0x060013C2 RID: 5058 RVA: 0x00041FE4 File Offset: 0x00040FE4
				public void Dispose()
				{
					this.dictEnum.Dispose();
				}

				// Token: 0x060013C3 RID: 5059 RVA: 0x00041FF1 File Offset: 0x00040FF1
				public bool MoveNext()
				{
					return this.dictEnum.MoveNext();
				}

				// Token: 0x1700040B RID: 1035
				// (get) Token: 0x060013C4 RID: 5060 RVA: 0x00042000 File Offset: 0x00041000
				public TKey Current
				{
					get
					{
						KeyValuePair<TKey, TValue> keyValuePair = this.dictEnum.Current;
						return keyValuePair.Key;
					}
				}

				// Token: 0x1700040C RID: 1036
				// (get) Token: 0x060013C5 RID: 5061 RVA: 0x00042020 File Offset: 0x00041020
				object IEnumerator.Current
				{
					get
					{
						if (this.dictEnum.NotStartedOrEnded)
						{
							ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
						}
						return this.Current;
					}
				}

				// Token: 0x060013C6 RID: 5062 RVA: 0x00042041 File Offset: 0x00041041
				void IEnumerator.Reset()
				{
					this.dictEnum.Reset();
				}

				// Token: 0x0400114B RID: 4427
				private SortedDictionary<TKey, TValue>.Enumerator dictEnum;
			}
		}

		// Token: 0x02000243 RID: 579
		[DebuggerDisplay("Count = {Count}")]
		[DebuggerTypeProxy(typeof(System_DictionaryValueCollectionDebugView<, >))]
		[Serializable]
		public sealed class ValueCollection : ICollection<TValue>, IEnumerable<TValue>, ICollection, IEnumerable
		{
			// Token: 0x060013C7 RID: 5063 RVA: 0x0004204E File Offset: 0x0004104E
			public ValueCollection(SortedDictionary<TKey, TValue> dictionary)
			{
				if (dictionary == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.dictionary);
				}
				this.dictionary = dictionary;
			}

			// Token: 0x060013C8 RID: 5064 RVA: 0x00042066 File Offset: 0x00041066
			public SortedDictionary<TKey, TValue>.ValueCollection.Enumerator GetEnumerator()
			{
				return new SortedDictionary<TKey, TValue>.ValueCollection.Enumerator(this.dictionary);
			}

			// Token: 0x060013C9 RID: 5065 RVA: 0x00042073 File Offset: 0x00041073
			IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
			{
				return new SortedDictionary<TKey, TValue>.ValueCollection.Enumerator(this.dictionary);
			}

			// Token: 0x060013CA RID: 5066 RVA: 0x00042085 File Offset: 0x00041085
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new SortedDictionary<TKey, TValue>.ValueCollection.Enumerator(this.dictionary);
			}

			// Token: 0x060013CB RID: 5067 RVA: 0x000420D8 File Offset: 0x000410D8
			public void CopyTo(TValue[] array, int index)
			{
				if (array == null)
				{
					ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
				}
				if (index < 0)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index);
				}
				if (array.Length - index < this.Count)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				this.dictionary._set.InOrderTreeWalk(delegate(TreeSet<KeyValuePair<TKey, TValue>>.Node node)
				{
					array[index++] = node.Item.Value;
					return true;
				});
			}

			// Token: 0x060013CC RID: 5068 RVA: 0x000421A4 File Offset: 0x000411A4
			void ICollection.CopyTo(Array array, int index)
			{
				SortedDictionary<TKey, TValue>.ValueCollection.<>c__DisplayClass14 CS$<>8__locals1 = new SortedDictionary<TKey, TValue>.ValueCollection.<>c__DisplayClass14();
				CS$<>8__locals1.index = index;
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
				if (CS$<>8__locals1.index < 0)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.arrayIndex, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
				}
				if (array.Length - CS$<>8__locals1.index < this.dictionary.Count)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
				}
				TValue[] array2 = array as TValue[];
				if (array2 != null)
				{
					this.CopyTo(array2, CS$<>8__locals1.index);
					return;
				}
				object[] objects = (object[])array;
				if (objects == null)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
				}
				try
				{
					this.dictionary._set.InOrderTreeWalk(delegate(TreeSet<KeyValuePair<TKey, TValue>>.Node node)
					{
						objects[CS$<>8__locals1.index++] = node.Item.Value;
						return true;
					});
				}
				catch (ArrayTypeMismatchException)
				{
					ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
				}
			}

			// Token: 0x1700040D RID: 1037
			// (get) Token: 0x060013CD RID: 5069 RVA: 0x00042298 File Offset: 0x00041298
			public int Count
			{
				get
				{
					return this.dictionary.Count;
				}
			}

			// Token: 0x1700040E RID: 1038
			// (get) Token: 0x060013CE RID: 5070 RVA: 0x000422A5 File Offset: 0x000412A5
			bool ICollection<TValue>.IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060013CF RID: 5071 RVA: 0x000422A8 File Offset: 0x000412A8
			void ICollection<TValue>.Add(TValue item)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
			}

			// Token: 0x060013D0 RID: 5072 RVA: 0x000422B1 File Offset: 0x000412B1
			void ICollection<TValue>.Clear()
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
			}

			// Token: 0x060013D1 RID: 5073 RVA: 0x000422BA File Offset: 0x000412BA
			bool ICollection<TValue>.Contains(TValue item)
			{
				return this.dictionary.ContainsValue(item);
			}

			// Token: 0x060013D2 RID: 5074 RVA: 0x000422C8 File Offset: 0x000412C8
			bool ICollection<TValue>.Remove(TValue item)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ValueCollectionSet);
				return false;
			}

			// Token: 0x1700040F RID: 1039
			// (get) Token: 0x060013D3 RID: 5075 RVA: 0x000422D2 File Offset: 0x000412D2
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000410 RID: 1040
			// (get) Token: 0x060013D4 RID: 5076 RVA: 0x000422D5 File Offset: 0x000412D5
			object ICollection.SyncRoot
			{
				get
				{
					return ((ICollection)this.dictionary).SyncRoot;
				}
			}

			// Token: 0x0400114C RID: 4428
			private SortedDictionary<TKey, TValue> dictionary;

			// Token: 0x02000244 RID: 580
			public struct Enumerator : IEnumerator<TValue>, IDisposable, IEnumerator
			{
				// Token: 0x060013D5 RID: 5077 RVA: 0x000422E2 File Offset: 0x000412E2
				internal Enumerator(SortedDictionary<TKey, TValue> dictionary)
				{
					this.dictEnum = dictionary.GetEnumerator();
				}

				// Token: 0x060013D6 RID: 5078 RVA: 0x000422F0 File Offset: 0x000412F0
				public void Dispose()
				{
					this.dictEnum.Dispose();
				}

				// Token: 0x060013D7 RID: 5079 RVA: 0x000422FD File Offset: 0x000412FD
				public bool MoveNext()
				{
					return this.dictEnum.MoveNext();
				}

				// Token: 0x17000411 RID: 1041
				// (get) Token: 0x060013D8 RID: 5080 RVA: 0x0004230C File Offset: 0x0004130C
				public TValue Current
				{
					get
					{
						KeyValuePair<TKey, TValue> keyValuePair = this.dictEnum.Current;
						return keyValuePair.Value;
					}
				}

				// Token: 0x17000412 RID: 1042
				// (get) Token: 0x060013D9 RID: 5081 RVA: 0x0004232C File Offset: 0x0004132C
				object IEnumerator.Current
				{
					get
					{
						if (this.dictEnum.NotStartedOrEnded)
						{
							ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_EnumOpCantHappen);
						}
						return this.Current;
					}
				}

				// Token: 0x060013DA RID: 5082 RVA: 0x0004234D File Offset: 0x0004134D
				void IEnumerator.Reset()
				{
					this.dictEnum.Reset();
				}

				// Token: 0x0400114D RID: 4429
				private SortedDictionary<TKey, TValue>.Enumerator dictEnum;
			}
		}

		// Token: 0x02000245 RID: 581
		[Serializable]
		internal class KeyValuePairComparer : Comparer<KeyValuePair<TKey, TValue>>
		{
			// Token: 0x060013DB RID: 5083 RVA: 0x0004235A File Offset: 0x0004135A
			public KeyValuePairComparer(IComparer<TKey> keyComparer)
			{
				if (keyComparer == null)
				{
					this.keyComparer = Comparer<TKey>.Default;
					return;
				}
				this.keyComparer = keyComparer;
			}

			// Token: 0x060013DC RID: 5084 RVA: 0x00042378 File Offset: 0x00041378
			public override int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
			{
				return this.keyComparer.Compare(x.Key, y.Key);
			}

			// Token: 0x0400114E RID: 4430
			internal IComparer<TKey> keyComparer;
		}
	}
}
