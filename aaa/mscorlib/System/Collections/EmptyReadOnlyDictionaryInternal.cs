using System;

namespace System.Collections
{
	// Token: 0x0200024D RID: 589
	[Serializable]
	internal sealed class EmptyReadOnlyDictionaryInternal : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x0600179F RID: 6047 RVA: 0x0003CEAD File Offset: 0x0003BEAD
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new EmptyReadOnlyDictionaryInternal.NodeEnumerator();
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x0003CEB4 File Offset: 0x0003BEB4
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
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x060017A1 RID: 6049 RVA: 0x0003CF26 File Offset: 0x0003BF26
		public int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x060017A2 RID: 6050 RVA: 0x0003CF29 File Offset: 0x0003BF29
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x060017A3 RID: 6051 RVA: 0x0003CF2C File Offset: 0x0003BF2C
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000359 RID: 857
		public object this[object key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
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
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ReadOnly"));
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x060017A6 RID: 6054 RVA: 0x0003CFC7 File Offset: 0x0003BFC7
		public ICollection Keys
		{
			get
			{
				return new object[0];
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x060017A7 RID: 6055 RVA: 0x0003CFCF File Offset: 0x0003BFCF
		public ICollection Values
		{
			get
			{
				return new object[0];
			}
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x0003CFD7 File Offset: 0x0003BFD7
		public bool Contains(object key)
		{
			return false;
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x0003CFDC File Offset: 0x0003BFDC
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
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ReadOnly"));
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x0003D057 File Offset: 0x0003C057
		public void Clear()
		{
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ReadOnly"));
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x060017AB RID: 6059 RVA: 0x0003D068 File Offset: 0x0003C068
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x060017AC RID: 6060 RVA: 0x0003D06B File Offset: 0x0003C06B
		public bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x0003D06E File Offset: 0x0003C06E
		public IDictionaryEnumerator GetEnumerator()
		{
			return new EmptyReadOnlyDictionaryInternal.NodeEnumerator();
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x0003D075 File Offset: 0x0003C075
		public void Remove(object key)
		{
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ReadOnly"));
		}

		// Token: 0x0200024F RID: 591
		private sealed class NodeEnumerator : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x060017B3 RID: 6067 RVA: 0x0003D08E File Offset: 0x0003C08E
			public bool MoveNext()
			{
				return false;
			}

			// Token: 0x17000361 RID: 865
			// (get) Token: 0x060017B4 RID: 6068 RVA: 0x0003D091 File Offset: 0x0003C091
			public object Current
			{
				get
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
				}
			}

			// Token: 0x060017B5 RID: 6069 RVA: 0x0003D0A2 File Offset: 0x0003C0A2
			public void Reset()
			{
			}

			// Token: 0x17000362 RID: 866
			// (get) Token: 0x060017B6 RID: 6070 RVA: 0x0003D0A4 File Offset: 0x0003C0A4
			public object Key
			{
				get
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
				}
			}

			// Token: 0x17000363 RID: 867
			// (get) Token: 0x060017B7 RID: 6071 RVA: 0x0003D0B5 File Offset: 0x0003C0B5
			public object Value
			{
				get
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
				}
			}

			// Token: 0x17000364 RID: 868
			// (get) Token: 0x060017B8 RID: 6072 RVA: 0x0003D0C6 File Offset: 0x0003C0C6
			public DictionaryEntry Entry
			{
				get
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
				}
			}
		}
	}
}
