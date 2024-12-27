using System;
using System.Collections;
using System.Collections.Generic;

namespace System
{
	// Token: 0x02000014 RID: 20
	internal sealed class SZArrayHelper
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x0000490E File Offset: 0x0000390E
		private SZArrayHelper()
		{
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004916 File Offset: 0x00003916
		internal IEnumerator<T> GetEnumerator<T>()
		{
			return new SZArrayHelper.SZGenericArrayEnumerator<T>(this as T[]);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00004924 File Offset: 0x00003924
		private void CopyTo<T>(T[] array, int index)
		{
			if (array != null && array.Rank != 1)
			{
				throw new ArgumentException(Environment.GetResourceString("Rank_MultiDimNotSupported"));
			}
			T[] array2 = this as T[];
			Array.Copy(array2, 0, array, index, array2.Length);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00004960 File Offset: 0x00003960
		internal int get_Count<T>()
		{
			T[] array = this as T[];
			return array.Length;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004978 File Offset: 0x00003978
		internal T get_Item<T>(int index)
		{
			T[] array = this as T[];
			if (index >= array.Length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException();
			}
			return array[index];
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000049A0 File Offset: 0x000039A0
		internal void set_Item<T>(int index, T value)
		{
			T[] array = this as T[];
			if (index >= array.Length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException();
			}
			array[index] = value;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000049C7 File Offset: 0x000039C7
		private void Add<T>(T value)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000049D8 File Offset: 0x000039D8
		private bool Contains<T>(T value)
		{
			T[] array = this as T[];
			return Array.IndexOf<T>(array, value) != -1;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000049F9 File Offset: 0x000039F9
		private bool get_IsReadOnly<T>()
		{
			return true;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000049FC File Offset: 0x000039FC
		private void Clear<T>()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_ReadOnlyCollection"));
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004A10 File Offset: 0x00003A10
		private int IndexOf<T>(T value)
		{
			T[] array = this as T[];
			return Array.IndexOf<T>(array, value);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004A2B File Offset: 0x00003A2B
		private void Insert<T>(int index, T value)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00004A3C File Offset: 0x00003A3C
		private bool Remove<T>(T value)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00004A4D File Offset: 0x00003A4D
		private void RemoveAt<T>(int index)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_FixedSizeCollection"));
		}

		// Token: 0x02000017 RID: 23
		[Serializable]
		private sealed class SZGenericArrayEnumerator<T> : IEnumerator<T>, IDisposable, IEnumerator
		{
			// Token: 0x060000C7 RID: 199 RVA: 0x00004A5E File Offset: 0x00003A5E
			internal SZGenericArrayEnumerator(T[] array)
			{
				this._array = array;
				this._index = -1;
				this._endIndex = array.Length;
			}

			// Token: 0x060000C8 RID: 200 RVA: 0x00004A7D File Offset: 0x00003A7D
			public bool MoveNext()
			{
				if (this._index < this._endIndex)
				{
					this._index++;
					return this._index < this._endIndex;
				}
				return false;
			}

			// Token: 0x17000014 RID: 20
			// (get) Token: 0x060000C9 RID: 201 RVA: 0x00004AAC File Offset: 0x00003AAC
			public T Current
			{
				get
				{
					if (this._index < 0)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
					}
					if (this._index >= this._endIndex)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
					}
					return this._array[this._index];
				}
			}

			// Token: 0x17000015 RID: 21
			// (get) Token: 0x060000CA RID: 202 RVA: 0x00004B01 File Offset: 0x00003B01
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x060000CB RID: 203 RVA: 0x00004B0E File Offset: 0x00003B0E
			void IEnumerator.Reset()
			{
				this._index = -1;
			}

			// Token: 0x060000CC RID: 204 RVA: 0x00004B17 File Offset: 0x00003B17
			public void Dispose()
			{
			}

			// Token: 0x04000041 RID: 65
			private T[] _array;

			// Token: 0x04000042 RID: 66
			private int _index;

			// Token: 0x04000043 RID: 67
			private int _endIndex;
		}
	}
}
