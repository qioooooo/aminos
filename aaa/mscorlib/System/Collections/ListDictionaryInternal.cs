using System;
using System.Threading;

namespace System.Collections
{
	// Token: 0x02000259 RID: 601
	[Serializable]
	internal class ListDictionaryInternal : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x17000385 RID: 901
		public object this[object key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
				}
				for (ListDictionaryInternal.DictionaryNode next = this.head; next != null; next = next.next)
				{
					if (next.key.Equals(key))
					{
						return next.value;
					}
				}
				return null;
			}
			set
			{
				if (key == null)
				{
					throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
				}
				if (!key.GetType().IsSerializable)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_NotSerializable"), "key");
				}
				if (value != null && !value.GetType().IsSerializable)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_NotSerializable"), "value");
				}
				this.version++;
				ListDictionaryInternal.DictionaryNode dictionaryNode = null;
				ListDictionaryInternal.DictionaryNode next = this.head;
				while (next != null && !next.key.Equals(key))
				{
					dictionaryNode = next;
					next = next.next;
				}
				if (next != null)
				{
					next.value = value;
					return;
				}
				ListDictionaryInternal.DictionaryNode dictionaryNode2 = new ListDictionaryInternal.DictionaryNode();
				dictionaryNode2.key = key;
				dictionaryNode2.value = value;
				if (dictionaryNode != null)
				{
					dictionaryNode.next = dictionaryNode2;
				}
				else
				{
					this.head = dictionaryNode2;
				}
				this.count++;
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06001828 RID: 6184 RVA: 0x0003EF9F File Offset: 0x0003DF9F
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06001829 RID: 6185 RVA: 0x0003EFA7 File Offset: 0x0003DFA7
		public ICollection Keys
		{
			get
			{
				return new ListDictionaryInternal.NodeKeyValueCollection(this, true);
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x0600182A RID: 6186 RVA: 0x0003EFB0 File Offset: 0x0003DFB0
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x0600182B RID: 6187 RVA: 0x0003EFB3 File Offset: 0x0003DFB3
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x0600182C RID: 6188 RVA: 0x0003EFB6 File Offset: 0x0003DFB6
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x0600182D RID: 6189 RVA: 0x0003EFB9 File Offset: 0x0003DFB9
		public object SyncRoot
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

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x0600182E RID: 6190 RVA: 0x0003EFDB File Offset: 0x0003DFDB
		public ICollection Values
		{
			get
			{
				return new ListDictionaryInternal.NodeKeyValueCollection(this, false);
			}
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x0003EFE4 File Offset: 0x0003DFE4
		public void Add(object key, object value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
			}
			if (!key.GetType().IsSerializable)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NotSerializable"), "key");
			}
			if (value != null && !value.GetType().IsSerializable)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NotSerializable"), "value");
			}
			this.version++;
			ListDictionaryInternal.DictionaryNode dictionaryNode = null;
			ListDictionaryInternal.DictionaryNode next;
			for (next = this.head; next != null; next = next.next)
			{
				if (next.key.Equals(key))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_AddingDuplicate__", new object[] { next.key, key }));
				}
				dictionaryNode = next;
			}
			if (next != null)
			{
				next.value = value;
				return;
			}
			ListDictionaryInternal.DictionaryNode dictionaryNode2 = new ListDictionaryInternal.DictionaryNode();
			dictionaryNode2.key = key;
			dictionaryNode2.value = value;
			if (dictionaryNode != null)
			{
				dictionaryNode.next = dictionaryNode2;
			}
			else
			{
				this.head = dictionaryNode2;
			}
			this.count++;
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x0003F0E8 File Offset: 0x0003E0E8
		public void Clear()
		{
			this.count = 0;
			this.head = null;
			this.version++;
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x0003F108 File Offset: 0x0003E108
		public bool Contains(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
			}
			for (ListDictionaryInternal.DictionaryNode next = this.head; next != null; next = next.next)
			{
				if (next.key.Equals(key))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x0003F154 File Offset: 0x0003E154
		public void CopyTo(Array array, int index)
		{
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
			if (array.Length - index < this.Count)
			{
				throw new ArgumentException(Environment.GetResourceString("ArgumentOutOfRange_Index"), "index");
			}
			for (ListDictionaryInternal.DictionaryNode next = this.head; next != null; next = next.next)
			{
				array.SetValue(new DictionaryEntry(next.key, next.value), index);
				index++;
			}
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x0003F1FB File Offset: 0x0003E1FB
		public IDictionaryEnumerator GetEnumerator()
		{
			return new ListDictionaryInternal.NodeEnumerator(this);
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x0003F203 File Offset: 0x0003E203
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new ListDictionaryInternal.NodeEnumerator(this);
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x0003F20C File Offset: 0x0003E20C
		public void Remove(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
			}
			this.version++;
			ListDictionaryInternal.DictionaryNode dictionaryNode = null;
			ListDictionaryInternal.DictionaryNode next = this.head;
			while (next != null && !next.key.Equals(key))
			{
				dictionaryNode = next;
				next = next.next;
			}
			if (next == null)
			{
				return;
			}
			if (next == this.head)
			{
				this.head = next.next;
			}
			else
			{
				dictionaryNode.next = next.next;
			}
			this.count--;
		}

		// Token: 0x04000968 RID: 2408
		private ListDictionaryInternal.DictionaryNode head;

		// Token: 0x04000969 RID: 2409
		private int version;

		// Token: 0x0400096A RID: 2410
		private int count;

		// Token: 0x0400096B RID: 2411
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x0200025A RID: 602
		private class NodeEnumerator : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x06001836 RID: 6198 RVA: 0x0003F299 File Offset: 0x0003E299
			public NodeEnumerator(ListDictionaryInternal list)
			{
				this.list = list;
				this.version = list.version;
				this.start = true;
				this.current = null;
			}

			// Token: 0x1700038D RID: 909
			// (get) Token: 0x06001837 RID: 6199 RVA: 0x0003F2C2 File Offset: 0x0003E2C2
			public object Current
			{
				get
				{
					return this.Entry;
				}
			}

			// Token: 0x1700038E RID: 910
			// (get) Token: 0x06001838 RID: 6200 RVA: 0x0003F2CF File Offset: 0x0003E2CF
			public DictionaryEntry Entry
			{
				get
				{
					if (this.current == null)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
					}
					return new DictionaryEntry(this.current.key, this.current.value);
				}
			}

			// Token: 0x1700038F RID: 911
			// (get) Token: 0x06001839 RID: 6201 RVA: 0x0003F304 File Offset: 0x0003E304
			public object Key
			{
				get
				{
					if (this.current == null)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
					}
					return this.current.key;
				}
			}

			// Token: 0x17000390 RID: 912
			// (get) Token: 0x0600183A RID: 6202 RVA: 0x0003F329 File Offset: 0x0003E329
			public object Value
			{
				get
				{
					if (this.current == null)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
					}
					return this.current.value;
				}
			}

			// Token: 0x0600183B RID: 6203 RVA: 0x0003F350 File Offset: 0x0003E350
			public bool MoveNext()
			{
				if (this.version != this.list.version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				if (this.start)
				{
					this.current = this.list.head;
					this.start = false;
				}
				else if (this.current != null)
				{
					this.current = this.current.next;
				}
				return this.current != null;
			}

			// Token: 0x0600183C RID: 6204 RVA: 0x0003F3C7 File Offset: 0x0003E3C7
			public void Reset()
			{
				if (this.version != this.list.version)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
				}
				this.start = true;
				this.current = null;
			}

			// Token: 0x0400096C RID: 2412
			private ListDictionaryInternal list;

			// Token: 0x0400096D RID: 2413
			private ListDictionaryInternal.DictionaryNode current;

			// Token: 0x0400096E RID: 2414
			private int version;

			// Token: 0x0400096F RID: 2415
			private bool start;
		}

		// Token: 0x0200025B RID: 603
		private class NodeKeyValueCollection : ICollection, IEnumerable
		{
			// Token: 0x0600183D RID: 6205 RVA: 0x0003F3FA File Offset: 0x0003E3FA
			public NodeKeyValueCollection(ListDictionaryInternal list, bool isKeys)
			{
				this.list = list;
				this.isKeys = isKeys;
			}

			// Token: 0x0600183E RID: 6206 RVA: 0x0003F410 File Offset: 0x0003E410
			void ICollection.CopyTo(Array array, int index)
			{
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
				if (array.Length - index < this.list.Count)
				{
					throw new ArgumentException(Environment.GetResourceString("ArgumentOutOfRange_Index"), "index");
				}
				for (ListDictionaryInternal.DictionaryNode dictionaryNode = this.list.head; dictionaryNode != null; dictionaryNode = dictionaryNode.next)
				{
					array.SetValue(this.isKeys ? dictionaryNode.key : dictionaryNode.value, index);
					index++;
				}
			}

			// Token: 0x17000391 RID: 913
			// (get) Token: 0x0600183F RID: 6207 RVA: 0x0003F4C4 File Offset: 0x0003E4C4
			int ICollection.Count
			{
				get
				{
					int num = 0;
					for (ListDictionaryInternal.DictionaryNode dictionaryNode = this.list.head; dictionaryNode != null; dictionaryNode = dictionaryNode.next)
					{
						num++;
					}
					return num;
				}
			}

			// Token: 0x17000392 RID: 914
			// (get) Token: 0x06001840 RID: 6208 RVA: 0x0003F4F0 File Offset: 0x0003E4F0
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000393 RID: 915
			// (get) Token: 0x06001841 RID: 6209 RVA: 0x0003F4F3 File Offset: 0x0003E4F3
			object ICollection.SyncRoot
			{
				get
				{
					return this.list.SyncRoot;
				}
			}

			// Token: 0x06001842 RID: 6210 RVA: 0x0003F500 File Offset: 0x0003E500
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new ListDictionaryInternal.NodeKeyValueCollection.NodeKeyValueEnumerator(this.list, this.isKeys);
			}

			// Token: 0x04000970 RID: 2416
			private ListDictionaryInternal list;

			// Token: 0x04000971 RID: 2417
			private bool isKeys;

			// Token: 0x0200025C RID: 604
			private class NodeKeyValueEnumerator : IEnumerator
			{
				// Token: 0x06001843 RID: 6211 RVA: 0x0003F513 File Offset: 0x0003E513
				public NodeKeyValueEnumerator(ListDictionaryInternal list, bool isKeys)
				{
					this.list = list;
					this.isKeys = isKeys;
					this.version = list.version;
					this.start = true;
					this.current = null;
				}

				// Token: 0x17000394 RID: 916
				// (get) Token: 0x06001844 RID: 6212 RVA: 0x0003F543 File Offset: 0x0003E543
				public object Current
				{
					get
					{
						if (this.current == null)
						{
							throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
						}
						if (!this.isKeys)
						{
							return this.current.value;
						}
						return this.current.key;
					}
				}

				// Token: 0x06001845 RID: 6213 RVA: 0x0003F57C File Offset: 0x0003E57C
				public bool MoveNext()
				{
					if (this.version != this.list.version)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
					}
					if (this.start)
					{
						this.current = this.list.head;
						this.start = false;
					}
					else if (this.current != null)
					{
						this.current = this.current.next;
					}
					return this.current != null;
				}

				// Token: 0x06001846 RID: 6214 RVA: 0x0003F5F3 File Offset: 0x0003E5F3
				public void Reset()
				{
					if (this.version != this.list.version)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumFailedVersion"));
					}
					this.start = true;
					this.current = null;
				}

				// Token: 0x04000972 RID: 2418
				private ListDictionaryInternal list;

				// Token: 0x04000973 RID: 2419
				private ListDictionaryInternal.DictionaryNode current;

				// Token: 0x04000974 RID: 2420
				private int version;

				// Token: 0x04000975 RID: 2421
				private bool isKeys;

				// Token: 0x04000976 RID: 2422
				private bool start;
			}
		}

		// Token: 0x0200025D RID: 605
		[Serializable]
		private class DictionaryNode
		{
			// Token: 0x04000977 RID: 2423
			public object key;

			// Token: 0x04000978 RID: 2424
			public object value;

			// Token: 0x04000979 RID: 2425
			public ListDictionaryInternal.DictionaryNode next;
		}
	}
}
