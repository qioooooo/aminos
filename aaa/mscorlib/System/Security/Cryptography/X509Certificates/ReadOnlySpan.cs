using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008D4 RID: 2260
	internal struct ReadOnlySpan<T>
	{
		// Token: 0x06005288 RID: 21128 RVA: 0x0012A83B File Offset: 0x0012983B
		public ReadOnlySpan(ArraySegment<T> segment)
		{
			this._Segment = segment;
		}

		// Token: 0x06005289 RID: 21129 RVA: 0x0012A844 File Offset: 0x00129844
		public ReadOnlySpan(T[] array, int offset, int count)
		{
			this = new ReadOnlySpan<T>((array != null || offset != 0 || count != 0) ? new ArraySegment<T>(array, offset, count) : default(ArraySegment<T>));
		}

		// Token: 0x0600528A RID: 21130 RVA: 0x0012A874 File Offset: 0x00129874
		public ReadOnlySpan(T[] array)
		{
			this = new ReadOnlySpan<T>((array != null) ? new ArraySegment<T>(array) : default(ArraySegment<T>));
		}

		// Token: 0x17000E42 RID: 3650
		public T this[int index]
		{
			get
			{
				if (index < 0 || index >= this._Segment.Count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return this._Segment.Array[index + this._Segment.Offset];
			}
		}

		// Token: 0x17000E43 RID: 3651
		// (get) Token: 0x0600528C RID: 21132 RVA: 0x0012A8D7 File Offset: 0x001298D7
		public bool IsEmpty
		{
			get
			{
				return this._Segment.Count == 0;
			}
		}

		// Token: 0x17000E44 RID: 3652
		// (get) Token: 0x0600528D RID: 21133 RVA: 0x0012A8E7 File Offset: 0x001298E7
		public bool IsNull
		{
			get
			{
				return this._Segment.Array == null;
			}
		}

		// Token: 0x17000E45 RID: 3653
		// (get) Token: 0x0600528E RID: 21134 RVA: 0x0012A8F7 File Offset: 0x001298F7
		public int Length
		{
			get
			{
				return this._Segment.Count;
			}
		}

		// Token: 0x0600528F RID: 21135 RVA: 0x0012A904 File Offset: 0x00129904
		public ReadOnlySpan<T> Slice(int start)
		{
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			return new ReadOnlySpan<T>(this._Segment.Array, this._Segment.Offset + start, this._Segment.Count - start);
		}

		// Token: 0x06005290 RID: 21136 RVA: 0x0012A93A File Offset: 0x0012993A
		public ReadOnlySpan<T> Slice(int start, int length)
		{
			if (start < 0)
			{
				throw new InvalidOperationException();
			}
			if (length > this._Segment.Count - start)
			{
				throw new InvalidOperationException();
			}
			return new ReadOnlySpan<T>(this._Segment.Array, this._Segment.Offset + start, length);
		}

		// Token: 0x06005291 RID: 21137 RVA: 0x0012A97C File Offset: 0x0012997C
		public T[] ToArray()
		{
			T[] array = new T[this._Segment.Count];
			if (!this.IsEmpty)
			{
				Array.Copy(this._Segment.Array, this._Segment.Offset, array, 0, this._Segment.Count);
			}
			return array;
		}

		// Token: 0x06005292 RID: 21138 RVA: 0x0012A9CC File Offset: 0x001299CC
		public void CopyTo(Span<T> destination)
		{
			if (destination.Length < this.Length)
			{
				throw new InvalidOperationException("Destination too short");
			}
			if (!this.IsEmpty)
			{
				ArraySegment<T> arraySegment = destination.DangerousGetArraySegment();
				Array.Copy(this._Segment.Array, this._Segment.Offset, arraySegment.Array, arraySegment.Offset, this._Segment.Count);
			}
		}

		// Token: 0x06005293 RID: 21139 RVA: 0x0012AA38 File Offset: 0x00129A38
		public bool Overlaps(ReadOnlySpan<T> destination)
		{
			int num;
			return this.Overlaps(destination, out num);
		}

		// Token: 0x06005294 RID: 21140 RVA: 0x0012AA50 File Offset: 0x00129A50
		public bool Overlaps(ReadOnlySpan<T> destination, out int elementOffset)
		{
			elementOffset = 0;
			if (this.IsEmpty || destination.IsEmpty)
			{
				return false;
			}
			if (this._Segment.Array != destination._Segment.Array)
			{
				return false;
			}
			elementOffset = destination._Segment.Offset - this._Segment.Offset;
			if (elementOffset >= this._Segment.Count || elementOffset <= -destination._Segment.Count)
			{
				elementOffset = 0;
				return false;
			}
			return true;
		}

		// Token: 0x06005295 RID: 21141 RVA: 0x0012AACE File Offset: 0x00129ACE
		public ArraySegment<T> DangerousGetArraySegment()
		{
			return this._Segment;
		}

		// Token: 0x06005296 RID: 21142 RVA: 0x0012AAD6 File Offset: 0x00129AD6
		public static implicit operator ReadOnlySpan<T>(T[] array)
		{
			return new ReadOnlySpan<T>(array);
		}

		// Token: 0x04002A62 RID: 10850
		public static readonly Span<T> Empty = default(Span<T>);

		// Token: 0x04002A63 RID: 10851
		private ArraySegment<T> _Segment;
	}
}
