using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Collections.ObjectModel
{
	// Token: 0x02000296 RID: 662
	[ComVisible(false)]
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(Mscorlib_CollectionDebugView<>))]
	[Serializable]
	public class Collection<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable
	{
		// Token: 0x06001A5C RID: 6748 RVA: 0x00045C9D File Offset: 0x00044C9D
		public Collection()
		{
			this.items = new List<T>();
		}

		// Token: 0x06001A5D RID: 6749 RVA: 0x00045CB0 File Offset: 0x00044CB0
		public Collection(IList<T> list)
		{
			if (list == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.list);
			}
			this.items = list;
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06001A5E RID: 6750 RVA: 0x00045CC8 File Offset: 0x00044CC8
		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06001A5F RID: 6751 RVA: 0x00045CD5 File Offset: 0x00044CD5
		protected IList<T> Items
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x1700040C RID: 1036
		public T this[int index]
		{
			get
			{
				return this.items[index];
			}
			set
			{
				if (this.items.IsReadOnly)
				{
					ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
				}
				if (index < 0 || index >= this.items.Count)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException();
				}
				this.SetItem(index, value);
			}
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x00045D20 File Offset: 0x00044D20
		public void Add(T item)
		{
			if (this.items.IsReadOnly)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
			int count = this.items.Count;
			this.InsertItem(count, item);
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x00045D55 File Offset: 0x00044D55
		public void Clear()
		{
			if (this.items.IsReadOnly)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
			this.ClearItems();
		}

		// Token: 0x06001A64 RID: 6756 RVA: 0x00045D71 File Offset: 0x00044D71
		public void CopyTo(T[] array, int index)
		{
			this.items.CopyTo(array, index);
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x00045D80 File Offset: 0x00044D80
		public bool Contains(T item)
		{
			return this.items.Contains(item);
		}

		// Token: 0x06001A66 RID: 6758 RVA: 0x00045D8E File Offset: 0x00044D8E
		public IEnumerator<T> GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x06001A67 RID: 6759 RVA: 0x00045D9B File Offset: 0x00044D9B
		public int IndexOf(T item)
		{
			return this.items.IndexOf(item);
		}

		// Token: 0x06001A68 RID: 6760 RVA: 0x00045DA9 File Offset: 0x00044DA9
		public void Insert(int index, T item)
		{
			if (index < 0 || index > this.items.Count)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_ListInsert);
			}
			this.InsertItem(index, item);
		}

		// Token: 0x06001A69 RID: 6761 RVA: 0x00045DD0 File Offset: 0x00044DD0
		public bool Remove(T item)
		{
			if (this.items.IsReadOnly)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
			int num = this.items.IndexOf(item);
			if (num < 0)
			{
				return false;
			}
			this.RemoveItem(num);
			return true;
		}

		// Token: 0x06001A6A RID: 6762 RVA: 0x00045E0C File Offset: 0x00044E0C
		public void RemoveAt(int index)
		{
			if (this.items.IsReadOnly)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
			if (index < 0 || index >= this.items.Count)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException();
			}
			this.RemoveItem(index);
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x00045E40 File Offset: 0x00044E40
		protected virtual void ClearItems()
		{
			this.items.Clear();
		}

		// Token: 0x06001A6C RID: 6764 RVA: 0x00045E4D File Offset: 0x00044E4D
		protected virtual void InsertItem(int index, T item)
		{
			this.items.Insert(index, item);
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x00045E5C File Offset: 0x00044E5C
		protected virtual void RemoveItem(int index)
		{
			this.items.RemoveAt(index);
		}

		// Token: 0x06001A6E RID: 6766 RVA: 0x00045E6A File Offset: 0x00044E6A
		protected virtual void SetItem(int index, T item)
		{
			this.items[index] = item;
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06001A6F RID: 6767 RVA: 0x00045E79 File Offset: 0x00044E79
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return this.items.IsReadOnly;
			}
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x00045E86 File Offset: 0x00044E86
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06001A71 RID: 6769 RVA: 0x00045E93 File Offset: 0x00044E93
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06001A72 RID: 6770 RVA: 0x00045E98 File Offset: 0x00044E98
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					ICollection collection = this.items as ICollection;
					if (collection != null)
					{
						this._syncRoot = collection.SyncRoot;
					}
					else
					{
						Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
					}
				}
				return this._syncRoot;
			}
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x00045EE4 File Offset: 0x00044EE4
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
			if (index < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (array.Length - index < this.Count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			T[] array2 = array as T[];
			if (array2 != null)
			{
				this.items.CopyTo(array2, index);
				return;
			}
			Type elementType = array.GetType().GetElementType();
			Type typeFromHandle = typeof(T);
			if (!elementType.IsAssignableFrom(typeFromHandle) && !typeFromHandle.IsAssignableFrom(elementType))
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
			}
			object[] array3 = array as object[];
			if (array3 == null)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
			}
			int count = this.items.Count;
			try
			{
				for (int i = 0; i < count; i++)
				{
					array3[index++] = this.items[i];
				}
			}
			catch (ArrayTypeMismatchException)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
			}
		}

		// Token: 0x17000410 RID: 1040
		object IList.this[int index]
		{
			get
			{
				return this.items[index];
			}
			set
			{
				Collection<T>.VerifyValueType(value);
				this[index] = (T)((object)value);
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06001A76 RID: 6774 RVA: 0x00046010 File Offset: 0x00045010
		bool IList.IsReadOnly
		{
			get
			{
				return this.items.IsReadOnly;
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06001A77 RID: 6775 RVA: 0x00046020 File Offset: 0x00045020
		bool IList.IsFixedSize
		{
			get
			{
				IList list = this.items as IList;
				return list != null && list.IsFixedSize;
			}
		}

		// Token: 0x06001A78 RID: 6776 RVA: 0x00046044 File Offset: 0x00045044
		int IList.Add(object value)
		{
			if (this.items.IsReadOnly)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
			Collection<T>.VerifyValueType(value);
			this.Add((T)((object)value));
			return this.Count - 1;
		}

		// Token: 0x06001A79 RID: 6777 RVA: 0x00046074 File Offset: 0x00045074
		bool IList.Contains(object value)
		{
			return Collection<T>.IsCompatibleObject(value) && this.Contains((T)((object)value));
		}

		// Token: 0x06001A7A RID: 6778 RVA: 0x0004608C File Offset: 0x0004508C
		int IList.IndexOf(object value)
		{
			if (Collection<T>.IsCompatibleObject(value))
			{
				return this.IndexOf((T)((object)value));
			}
			return -1;
		}

		// Token: 0x06001A7B RID: 6779 RVA: 0x000460A4 File Offset: 0x000450A4
		void IList.Insert(int index, object value)
		{
			if (this.items.IsReadOnly)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
			Collection<T>.VerifyValueType(value);
			this.Insert(index, (T)((object)value));
		}

		// Token: 0x06001A7C RID: 6780 RVA: 0x000460CD File Offset: 0x000450CD
		void IList.Remove(object value)
		{
			if (this.items.IsReadOnly)
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
			if (Collection<T>.IsCompatibleObject(value))
			{
				this.Remove((T)((object)value));
			}
		}

		// Token: 0x06001A7D RID: 6781 RVA: 0x000460F8 File Offset: 0x000450F8
		private static bool IsCompatibleObject(object value)
		{
			return value is T || (value == null && !typeof(T).IsValueType);
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x00046119 File Offset: 0x00045119
		private static void VerifyValueType(object value)
		{
			if (!Collection<T>.IsCompatibleObject(value))
			{
				ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(T));
			}
		}

		// Token: 0x040009EF RID: 2543
		private IList<T> items;

		// Token: 0x040009F0 RID: 2544
		[NonSerialized]
		private object _syncRoot;
	}
}
