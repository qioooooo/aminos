using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008D5 RID: 2261
	internal struct Span<T>
	{
		// Token: 0x06005298 RID: 21144 RVA: 0x0012AAEB File Offset: 0x00129AEB
		public Span(ArraySegment<T> segment)
		{
			this._Segment = segment;
		}

		// Token: 0x06005299 RID: 21145 RVA: 0x0012AAF4 File Offset: 0x00129AF4
		public Span(T[] array, int offset, int count)
		{
			this = new Span<T>((array != null || offset != 0 || count != 0) ? new ArraySegment<T>(array, offset, count) : default(ArraySegment<T>));
		}

		// Token: 0x0600529A RID: 21146 RVA: 0x0012AB24 File Offset: 0x00129B24
		public Span(T[] array)
		{
			this = new Span<T>((array != null) ? new ArraySegment<T>(array) : default(ArraySegment<T>));
		}

		// Token: 0x17000E46 RID: 3654
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
			set
			{
				if (index < 0 || index >= this._Segment.Count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				this._Segment.Array[index + this._Segment.Offset] = value;
			}
		}

		// Token: 0x17000E47 RID: 3655
		// (get) Token: 0x0600529D RID: 21149 RVA: 0x0012ABC4 File Offset: 0x00129BC4
		public bool IsEmpty
		{
			get
			{
				return this._Segment.Count == 0;
			}
		}

		// Token: 0x17000E48 RID: 3656
		// (get) Token: 0x0600529E RID: 21150 RVA: 0x0012ABD4 File Offset: 0x00129BD4
		public int Length
		{
			get
			{
				return this._Segment.Count;
			}
		}

		// Token: 0x0600529F RID: 21151 RVA: 0x0012ABE1 File Offset: 0x00129BE1
		public Span<T> Slice(int start)
		{
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			return new Span<T>(this._Segment.Array, this._Segment.Offset + start, this._Segment.Count - start);
		}

		// Token: 0x060052A0 RID: 21152 RVA: 0x0012AC17 File Offset: 0x00129C17
		public Span<T> Slice(int start, int length)
		{
			if (start < 0 || length > this._Segment.Count - start)
			{
				throw new ArgumentOutOfRangeException();
			}
			return new Span<T>(this._Segment.Array, this._Segment.Offset + start, length);
		}

		// Token: 0x060052A1 RID: 21153 RVA: 0x0012AC54 File Offset: 0x00129C54
		public void Fill(T value)
		{
			for (int i = this._Segment.Offset; i < this._Segment.Count - this._Segment.Offset; i++)
			{
				this._Segment.Array[i] = value;
			}
		}

		// Token: 0x060052A2 RID: 21154 RVA: 0x0012ACA0 File Offset: 0x00129CA0
		public void Clear()
		{
			for (int i = this._Segment.Offset; i < this._Segment.Count - this._Segment.Offset; i++)
			{
				this._Segment.Array[i] = default(T);
			}
		}

		// Token: 0x060052A3 RID: 21155 RVA: 0x0012ACF4 File Offset: 0x00129CF4
		public T[] ToArray()
		{
			T[] array = new T[this._Segment.Count];
			if (!this.IsEmpty)
			{
				Array.Copy(this._Segment.Array, this._Segment.Offset, array, 0, this._Segment.Count);
			}
			return array;
		}

		// Token: 0x060052A4 RID: 21156 RVA: 0x0012AD44 File Offset: 0x00129D44
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

		// Token: 0x060052A5 RID: 21157 RVA: 0x0012ADB0 File Offset: 0x00129DB0
		public bool Overlaps(ReadOnlySpan<T> destination, out int elementOffset)
		{
			return this.Overlaps(destination, out elementOffset);
		}

		// Token: 0x060052A6 RID: 21158 RVA: 0x0012ADD2 File Offset: 0x00129DD2
		public ArraySegment<T> DangerousGetArraySegment()
		{
			return this._Segment;
		}

		// Token: 0x060052A7 RID: 21159 RVA: 0x0012ADDA File Offset: 0x00129DDA
		public static implicit operator Span<T>(T[] array)
		{
			return new Span<T>(array);
		}

		// Token: 0x060052A8 RID: 21160 RVA: 0x0012ADE2 File Offset: 0x00129DE2
		public static implicit operator ReadOnlySpan<T>(Span<T> span)
		{
			return new ReadOnlySpan<T>(span._Segment);
		}

		// Token: 0x060052A9 RID: 21161 RVA: 0x0012ADF0 File Offset: 0x00129DF0
		public T[] DangerousGetArrayForPinning()
		{
			return this._Segment.Array;
		}

		// Token: 0x04002A64 RID: 10852
		public static readonly Span<T> Empty = default(Span<T>);

		// Token: 0x04002A65 RID: 10853
		private ArraySegment<T> _Segment;
	}
}
