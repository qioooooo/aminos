using System;
using System.Threading;

namespace System.Collections.Specialized
{
	// Token: 0x02000251 RID: 593
	[Serializable]
	public class ListDictionary : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x0600145B RID: 5211 RVA: 0x0004396F File Offset: 0x0004296F
		public ListDictionary()
		{
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x00043977 File Offset: 0x00042977
		public ListDictionary(IComparer comparer)
		{
			this.comparer = comparer;
		}

		// Token: 0x1700042E RID: 1070
		public object this[object key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key", SR.GetString("ArgumentNull_Key"));
				}
				ListDictionary.DictionaryNode dictionaryNode = this.head;
				if (this.comparer == null)
				{
					while (dictionaryNode != null)
					{
						object key2 = dictionaryNode.key;
						if (key2 != null && key2.Equals(key))
						{
							return dictionaryNode.value;
						}
						dictionaryNode = dictionaryNode.next;
					}
				}
				else
				{
					while (dictionaryNode != null)
					{
						object key3 = dictionaryNode.key;
						if (key3 != null && this.comparer.Compare(key3, key) == 0)
						{
							return dictionaryNode.value;
						}
						dictionaryNode = dictionaryNode.next;
					}
				}
				return null;
			}
			set
			{
				if (key == null)
				{
					throw new ArgumentNullException("key", SR.GetString("ArgumentNull_Key"));
				}
				this.version++;
				ListDictionary.DictionaryNode dictionaryNode = null;
				ListDictionary.DictionaryNode next;
				for (next = this.head; next != null; next = next.next)
				{
					object key2 = next.key;
					if ((this.comparer == null) ? key2.Equals(key) : (this.comparer.Compare(key2, key) == 0))
					{
						break;
					}
					dictionaryNode = next;
				}
				if (next != null)
				{
					next.value = value;
					return;
				}
				ListDictionary.DictionaryNode dictionaryNode2 = new ListDictionary.DictionaryNode();
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

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x0600145F RID: 5215 RVA: 0x00043AC4 File Offset: 0x00042AC4
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06001460 RID: 5216 RVA: 0x00043ACC File Offset: 0x00042ACC
		public ICollection Keys
		{
			get
			{
				return new ListDictionary.NodeKeyValueCollection(this, true);
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06001461 RID: 5217 RVA: 0x00043AD5 File Offset: 0x00042AD5
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06001462 RID: 5218 RVA: 0x00043AD8 File Offset: 0x00042AD8
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06001463 RID: 5219 RVA: 0x00043ADB File Offset: 0x00042ADB
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06001464 RID: 5220 RVA: 0x00043ADE File Offset: 0x00042ADE
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

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06001465 RID: 5221 RVA: 0x00043B00 File Offset: 0x00042B00
		public ICollection Values
		{
			get
			{
				return new ListDictionary.NodeKeyValueCollection(this, false);
			}
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x00043B0C File Offset: 0x00042B0C
		public void Add(object key, object value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", SR.GetString("ArgumentNull_Key"));
			}
			this.version++;
			ListDictionary.DictionaryNode dictionaryNode = null;
			for (ListDictionary.DictionaryNode next = this.head; next != null; next = next.next)
			{
				object key2 = next.key;
				if ((this.comparer == null) ? key2.Equals(key) : (this.comparer.Compare(key2, key) == 0))
				{
					throw new ArgumentException(SR.GetString("Argument_AddingDuplicate"));
				}
				dictionaryNode = next;
			}
			ListDictionary.DictionaryNode dictionaryNode2 = new ListDictionary.DictionaryNode();
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

		// Token: 0x06001467 RID: 5223 RVA: 0x00043BC5 File Offset: 0x00042BC5
		public void Clear()
		{
			this.count = 0;
			this.head = null;
			this.version++;
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x00043BE4 File Offset: 0x00042BE4
		public bool Contains(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", SR.GetString("ArgumentNull_Key"));
			}
			for (ListDictionary.DictionaryNode next = this.head; next != null; next = next.next)
			{
				object key2 = next.key;
				if ((this.comparer == null) ? key2.Equals(key) : (this.comparer.Compare(key2, key) == 0))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x00043C4C File Offset: 0x00042C4C
		public void CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (array.Length - index < this.count)
			{
				throw new ArgumentException(SR.GetString("Arg_InsufficientSpace"));
			}
			for (ListDictionary.DictionaryNode next = this.head; next != null; next = next.next)
			{
				array.SetValue(new DictionaryEntry(next.key, next.value), index);
				index++;
			}
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x00043CD5 File Offset: 0x00042CD5
		public IDictionaryEnumerator GetEnumerator()
		{
			return new ListDictionary.NodeEnumerator(this);
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x00043CDD File Offset: 0x00042CDD
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new ListDictionary.NodeEnumerator(this);
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x00043CE8 File Offset: 0x00042CE8
		public void Remove(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", SR.GetString("ArgumentNull_Key"));
			}
			this.version++;
			ListDictionary.DictionaryNode dictionaryNode = null;
			ListDictionary.DictionaryNode next;
			for (next = this.head; next != null; next = next.next)
			{
				object key2 = next.key;
				if ((this.comparer == null) ? key2.Equals(key) : (this.comparer.Compare(key2, key) == 0))
				{
					break;
				}
				dictionaryNode = next;
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

		// Token: 0x04001174 RID: 4468
		private ListDictionary.DictionaryNode head;

		// Token: 0x04001175 RID: 4469
		private int version;

		// Token: 0x04001176 RID: 4470
		private int count;

		// Token: 0x04001177 RID: 4471
		private IComparer comparer;

		// Token: 0x04001178 RID: 4472
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x02000252 RID: 594
		private class NodeEnumerator : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x0600146D RID: 5229 RVA: 0x00043D91 File Offset: 0x00042D91
			public NodeEnumerator(ListDictionary list)
			{
				this.list = list;
				this.version = list.version;
				this.start = true;
				this.current = null;
			}

			// Token: 0x17000436 RID: 1078
			// (get) Token: 0x0600146E RID: 5230 RVA: 0x00043DBA File Offset: 0x00042DBA
			public object Current
			{
				get
				{
					return this.Entry;
				}
			}

			// Token: 0x17000437 RID: 1079
			// (get) Token: 0x0600146F RID: 5231 RVA: 0x00043DC7 File Offset: 0x00042DC7
			public DictionaryEntry Entry
			{
				get
				{
					if (this.current == null)
					{
						throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumOpCantHappen"));
					}
					return new DictionaryEntry(this.current.key, this.current.value);
				}
			}

			// Token: 0x17000438 RID: 1080
			// (get) Token: 0x06001470 RID: 5232 RVA: 0x00043DFC File Offset: 0x00042DFC
			public object Key
			{
				get
				{
					if (this.current == null)
					{
						throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumOpCantHappen"));
					}
					return this.current.key;
				}
			}

			// Token: 0x17000439 RID: 1081
			// (get) Token: 0x06001471 RID: 5233 RVA: 0x00043E21 File Offset: 0x00042E21
			public object Value
			{
				get
				{
					if (this.current == null)
					{
						throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumOpCantHappen"));
					}
					return this.current.value;
				}
			}

			// Token: 0x06001472 RID: 5234 RVA: 0x00043E48 File Offset: 0x00042E48
			public bool MoveNext()
			{
				if (this.version != this.list.version)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
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

			// Token: 0x06001473 RID: 5235 RVA: 0x00043EBF File Offset: 0x00042EBF
			public void Reset()
			{
				if (this.version != this.list.version)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
				}
				this.start = true;
				this.current = null;
			}

			// Token: 0x04001179 RID: 4473
			private ListDictionary list;

			// Token: 0x0400117A RID: 4474
			private ListDictionary.DictionaryNode current;

			// Token: 0x0400117B RID: 4475
			private int version;

			// Token: 0x0400117C RID: 4476
			private bool start;
		}

		// Token: 0x02000253 RID: 595
		private class NodeKeyValueCollection : ICollection, IEnumerable
		{
			// Token: 0x06001474 RID: 5236 RVA: 0x00043EF2 File Offset: 0x00042EF2
			public NodeKeyValueCollection(ListDictionary list, bool isKeys)
			{
				this.list = list;
				this.isKeys = isKeys;
			}

			// Token: 0x06001475 RID: 5237 RVA: 0x00043F08 File Offset: 0x00042F08
			void ICollection.CopyTo(Array array, int index)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				for (ListDictionary.DictionaryNode dictionaryNode = this.list.head; dictionaryNode != null; dictionaryNode = dictionaryNode.next)
				{
					array.SetValue(this.isKeys ? dictionaryNode.key : dictionaryNode.value, index);
					index++;
				}
			}

			// Token: 0x1700043A RID: 1082
			// (get) Token: 0x06001476 RID: 5238 RVA: 0x00043F78 File Offset: 0x00042F78
			int ICollection.Count
			{
				get
				{
					int num = 0;
					for (ListDictionary.DictionaryNode dictionaryNode = this.list.head; dictionaryNode != null; dictionaryNode = dictionaryNode.next)
					{
						num++;
					}
					return num;
				}
			}

			// Token: 0x1700043B RID: 1083
			// (get) Token: 0x06001477 RID: 5239 RVA: 0x00043FA4 File Offset: 0x00042FA4
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700043C RID: 1084
			// (get) Token: 0x06001478 RID: 5240 RVA: 0x00043FA7 File Offset: 0x00042FA7
			object ICollection.SyncRoot
			{
				get
				{
					return this.list.SyncRoot;
				}
			}

			// Token: 0x06001479 RID: 5241 RVA: 0x00043FB4 File Offset: 0x00042FB4
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new ListDictionary.NodeKeyValueCollection.NodeKeyValueEnumerator(this.list, this.isKeys);
			}

			// Token: 0x0400117D RID: 4477
			private ListDictionary list;

			// Token: 0x0400117E RID: 4478
			private bool isKeys;

			// Token: 0x02000254 RID: 596
			private class NodeKeyValueEnumerator : IEnumerator
			{
				// Token: 0x0600147A RID: 5242 RVA: 0x00043FC7 File Offset: 0x00042FC7
				public NodeKeyValueEnumerator(ListDictionary list, bool isKeys)
				{
					this.list = list;
					this.isKeys = isKeys;
					this.version = list.version;
					this.start = true;
					this.current = null;
				}

				// Token: 0x1700043D RID: 1085
				// (get) Token: 0x0600147B RID: 5243 RVA: 0x00043FF7 File Offset: 0x00042FF7
				public object Current
				{
					get
					{
						if (this.current == null)
						{
							throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumOpCantHappen"));
						}
						if (!this.isKeys)
						{
							return this.current.value;
						}
						return this.current.key;
					}
				}

				// Token: 0x0600147C RID: 5244 RVA: 0x00044030 File Offset: 0x00043030
				public bool MoveNext()
				{
					if (this.version != this.list.version)
					{
						throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
					}
					if (this.start)
					{
						this.current = this.list.head;
						this.start = false;
					}
					else
					{
						this.current = this.current.next;
					}
					return this.current != null;
				}

				// Token: 0x0600147D RID: 5245 RVA: 0x0004409F File Offset: 0x0004309F
				public void Reset()
				{
					if (this.version != this.list.version)
					{
						throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
					}
					this.start = true;
					this.current = null;
				}

				// Token: 0x0400117F RID: 4479
				private ListDictionary list;

				// Token: 0x04001180 RID: 4480
				private ListDictionary.DictionaryNode current;

				// Token: 0x04001181 RID: 4481
				private int version;

				// Token: 0x04001182 RID: 4482
				private bool isKeys;

				// Token: 0x04001183 RID: 4483
				private bool start;
			}
		}

		// Token: 0x02000255 RID: 597
		[Serializable]
		private class DictionaryNode
		{
			// Token: 0x04001184 RID: 4484
			public object key;

			// Token: 0x04001185 RID: 4485
			public object value;

			// Token: 0x04001186 RID: 4486
			public ListDictionary.DictionaryNode next;
		}
	}
}
