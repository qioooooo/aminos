using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008D6 RID: 2262
	internal struct ReadOnlyMemory<T>
	{
		// Token: 0x060052AB RID: 21163 RVA: 0x0012AE0A File Offset: 0x00129E0A
		public ReadOnlyMemory(ArraySegment<T> segment)
		{
			this._Segment = segment;
		}

		// Token: 0x060052AC RID: 21164 RVA: 0x0012AE14 File Offset: 0x00129E14
		public ReadOnlyMemory(T[] array, int offset, int count)
		{
			this = new ReadOnlyMemory<T>((array != null || offset != 0 || count != 0) ? new ArraySegment<T>(array, offset, count) : default(ArraySegment<T>));
		}

		// Token: 0x060052AD RID: 21165 RVA: 0x0012AE44 File Offset: 0x00129E44
		public ReadOnlyMemory(T[] array)
		{
			this = new ReadOnlyMemory<T>((array != null) ? new ArraySegment<T>(array) : default(ArraySegment<T>));
		}

		// Token: 0x17000E49 RID: 3657
		// (get) Token: 0x060052AE RID: 21166 RVA: 0x0012AE6C File Offset: 0x00129E6C
		public bool IsEmpty
		{
			get
			{
				return this._Segment.Count == 0;
			}
		}

		// Token: 0x17000E4A RID: 3658
		// (get) Token: 0x060052AF RID: 21167 RVA: 0x0012AE8C File Offset: 0x00129E8C
		public int Length
		{
			get
			{
				return this._Segment.Count;
			}
		}

		// Token: 0x17000E4B RID: 3659
		// (get) Token: 0x060052B0 RID: 21168 RVA: 0x0012AEA7 File Offset: 0x00129EA7
		public ReadOnlySpan<T> Span
		{
			get
			{
				return new ReadOnlySpan<T>(this._Segment);
			}
		}

		// Token: 0x060052B1 RID: 21169 RVA: 0x0012AEB4 File Offset: 0x00129EB4
		public ReadOnlyMemory<T> Slice(int start)
		{
			if (start < 0)
			{
				throw new InvalidOperationException();
			}
			return new ReadOnlyMemory<T>(this._Segment.Array, this._Segment.Offset + start, this._Segment.Count - start);
		}

		// Token: 0x060052B2 RID: 21170 RVA: 0x0012AF00 File Offset: 0x00129F00
		public ReadOnlyMemory<T> Slice(int start, int length)
		{
			if (start < 0)
			{
				throw new InvalidOperationException();
			}
			if (length > this._Segment.Count - start)
			{
				throw new InvalidOperationException();
			}
			return new ReadOnlyMemory<T>(this._Segment.Array, this._Segment.Offset + start, length);
		}

		// Token: 0x060052B3 RID: 21171 RVA: 0x0012AF54 File Offset: 0x00129F54
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

		// Token: 0x060052B4 RID: 21172 RVA: 0x0012AFC8 File Offset: 0x00129FC8
		public static implicit operator ReadOnlyMemory<T>(T[] array)
		{
			return new ReadOnlyMemory<T>(array);
		}

		// Token: 0x060052B5 RID: 21173 RVA: 0x0012AFD0 File Offset: 0x00129FD0
		public static implicit operator ArraySegment<T>(ReadOnlyMemory<T> memory)
		{
			return memory._Segment;
		}

		// Token: 0x060052B6 RID: 21174 RVA: 0x0012AFD9 File Offset: 0x00129FD9
		public static implicit operator ReadOnlyMemory<T>(ArraySegment<T> segment)
		{
			return new ReadOnlyMemory<T>(segment);
		}

		// Token: 0x04002A66 RID: 10854
		private readonly ArraySegment<T> _Segment;
	}
}
