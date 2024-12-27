using System;

namespace System
{
	// Token: 0x02000018 RID: 24
	[Serializable]
	public struct ArraySegment<T>
	{
		// Token: 0x060000CD RID: 205 RVA: 0x00004B19 File Offset: 0x00003B19
		public ArraySegment(T[] array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			this._array = array;
			this._offset = 0;
			this._count = array.Length;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00004B40 File Offset: 0x00003B40
		public ArraySegment(T[] array, int offset, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (array.Length - offset < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			this._array = array;
			this._offset = offset;
			this._count = count;
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00004BBA File Offset: 0x00003BBA
		public T[] Array
		{
			get
			{
				return this._array;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00004BC2 File Offset: 0x00003BC2
		public int Offset
		{
			get
			{
				return this._offset;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00004BCA File Offset: 0x00003BCA
		public int Count
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00004BD2 File Offset: 0x00003BD2
		public override int GetHashCode()
		{
			return this._array.GetHashCode() ^ this._offset ^ this._count;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00004BED File Offset: 0x00003BED
		public override bool Equals(object obj)
		{
			return obj is ArraySegment<T> && this.Equals((ArraySegment<T>)obj);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00004C05 File Offset: 0x00003C05
		public bool Equals(ArraySegment<T> obj)
		{
			return obj._array == this._array && obj._offset == this._offset && obj._count == this._count;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00004C36 File Offset: 0x00003C36
		public static bool operator ==(ArraySegment<T> a, ArraySegment<T> b)
		{
			return a.Equals(b);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00004C40 File Offset: 0x00003C40
		public static bool operator !=(ArraySegment<T> a, ArraySegment<T> b)
		{
			return !(a == b);
		}

		// Token: 0x04000044 RID: 68
		private T[] _array;

		// Token: 0x04000045 RID: 69
		private int _offset;

		// Token: 0x04000046 RID: 70
		private int _count;
	}
}
