using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Collections.ObjectModel
{
	// Token: 0x02000297 RID: 663
	[ComVisible(false)]
	[DebuggerTypeProxy(typeof(Mscorlib_CollectionDebugView<>))]
	[DebuggerDisplay("Count = {Count}")]
	[Serializable]
	public class ReadOnlyCollection<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable
	{
		// Token: 0x06001A7F RID: 6783 RVA: 0x00046133 File Offset: 0x00045133
		public ReadOnlyCollection(IList<T> list)
		{
			if (list == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.list);
			}
			this.list = list;
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06001A80 RID: 6784 RVA: 0x0004614B File Offset: 0x0004514B
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		// Token: 0x17000414 RID: 1044
		public T this[int index]
		{
			get
			{
				return this.list[index];
			}
		}

		// Token: 0x06001A82 RID: 6786 RVA: 0x00046166 File Offset: 0x00045166
		public bool Contains(T value)
		{
			return this.list.Contains(value);
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x00046174 File Offset: 0x00045174
		public void CopyTo(T[] array, int index)
		{
			this.list.CopyTo(array, index);
		}

		// Token: 0x06001A84 RID: 6788 RVA: 0x00046183 File Offset: 0x00045183
		public IEnumerator<T> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x06001A85 RID: 6789 RVA: 0x00046190 File Offset: 0x00045190
		public int IndexOf(T value)
		{
			return this.list.IndexOf(value);
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06001A86 RID: 6790 RVA: 0x0004619E File Offset: 0x0004519E
		protected IList<T> Items
		{
			get
			{
				return this.list;
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06001A87 RID: 6791 RVA: 0x000461A6 File Offset: 0x000451A6
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000417 RID: 1047
		T IList<T>.this[int index]
		{
			get
			{
				return this.list[index];
			}
			set
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x000461C0 File Offset: 0x000451C0
		void ICollection<T>.Add(T value)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x000461C9 File Offset: 0x000451C9
		void ICollection<T>.Clear()
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x000461D2 File Offset: 0x000451D2
		void IList<T>.Insert(int index, T value)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x06001A8D RID: 6797 RVA: 0x000461DB File Offset: 0x000451DB
		bool ICollection<T>.Remove(T value)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			return false;
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x000461E5 File Offset: 0x000451E5
		void IList<T>.RemoveAt(int index)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x000461EE File Offset: 0x000451EE
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06001A90 RID: 6800 RVA: 0x000461FB File Offset: 0x000451FB
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06001A91 RID: 6801 RVA: 0x00046200 File Offset: 0x00045200
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					ICollection collection = this.list as ICollection;
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

		// Token: 0x06001A92 RID: 6802 RVA: 0x0004624C File Offset: 0x0004524C
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
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.arrayIndex, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (array.Length - index < this.Count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			T[] array2 = array as T[];
			if (array2 != null)
			{
				this.list.CopyTo(array2, index);
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
			int count = this.list.Count;
			try
			{
				for (int i = 0; i < count; i++)
				{
					array3[index++] = this.list[i];
				}
			}
			catch (ArrayTypeMismatchException)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
			}
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06001A93 RID: 6803 RVA: 0x00046350 File Offset: 0x00045350
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06001A94 RID: 6804 RVA: 0x00046353 File Offset: 0x00045353
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700041C RID: 1052
		object IList.this[int index]
		{
			get
			{
				return this.list[index];
			}
			set
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x00046372 File Offset: 0x00045372
		int IList.Add(object value)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			return -1;
		}

		// Token: 0x06001A98 RID: 6808 RVA: 0x0004637C File Offset: 0x0004537C
		void IList.Clear()
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x00046385 File Offset: 0x00045385
		private static bool IsCompatibleObject(object value)
		{
			return value is T || (value == null && !typeof(T).IsValueType);
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x000463A6 File Offset: 0x000453A6
		bool IList.Contains(object value)
		{
			return ReadOnlyCollection<T>.IsCompatibleObject(value) && this.Contains((T)((object)value));
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x000463BE File Offset: 0x000453BE
		int IList.IndexOf(object value)
		{
			if (ReadOnlyCollection<T>.IsCompatibleObject(value))
			{
				return this.IndexOf((T)((object)value));
			}
			return -1;
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x000463D6 File Offset: 0x000453D6
		void IList.Insert(int index, object value)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x000463DF File Offset: 0x000453DF
		void IList.Remove(object value)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x000463E8 File Offset: 0x000453E8
		void IList.RemoveAt(int index)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x000463F1 File Offset: 0x000453F1
		private static void VerifyValueType(object value)
		{
			if (!ReadOnlyCollection<T>.IsCompatibleObject(value))
			{
				ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(T));
			}
		}

		// Token: 0x040009F1 RID: 2545
		private IList<T> list;

		// Token: 0x040009F2 RID: 2546
		[NonSerialized]
		private object _syncRoot;
	}
}
