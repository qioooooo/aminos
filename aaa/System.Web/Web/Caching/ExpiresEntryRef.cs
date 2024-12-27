using System;

namespace System.Web.Caching
{
	// Token: 0x0200011C RID: 284
	internal struct ExpiresEntryRef
	{
		// Token: 0x06000CE2 RID: 3298 RVA: 0x00035017 File Offset: 0x00034017
		internal ExpiresEntryRef(int pageIndex, int entryIndex)
		{
			this._ref = (uint)((pageIndex << 8) | (entryIndex & 255));
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x0003502A File Offset: 0x0003402A
		public override bool Equals(object value)
		{
			return value is ExpiresEntryRef && this._ref == ((ExpiresEntryRef)value)._ref;
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x00035049 File Offset: 0x00034049
		public static bool operator !=(ExpiresEntryRef r1, ExpiresEntryRef r2)
		{
			return r1._ref != r2._ref;
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x0003505E File Offset: 0x0003405E
		public static bool operator ==(ExpiresEntryRef r1, ExpiresEntryRef r2)
		{
			return r1._ref == r2._ref;
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x00035070 File Offset: 0x00034070
		public override int GetHashCode()
		{
			return (int)this._ref;
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x00035078 File Offset: 0x00034078
		internal int PageIndex
		{
			get
			{
				return (int)(this._ref >> 8);
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x00035090 File Offset: 0x00034090
		internal int Index
		{
			get
			{
				return (int)(this._ref & 255U);
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x000350AB File Offset: 0x000340AB
		internal bool IsInvalid
		{
			get
			{
				return this._ref == 0U;
			}
		}

		// Token: 0x040014B3 RID: 5299
		private const uint ENTRY_MASK = 255U;

		// Token: 0x040014B4 RID: 5300
		private const uint PAGE_MASK = 4294967040U;

		// Token: 0x040014B5 RID: 5301
		private const int PAGE_SHIFT = 8;

		// Token: 0x040014B6 RID: 5302
		internal static readonly ExpiresEntryRef INVALID = new ExpiresEntryRef(0, 0);

		// Token: 0x040014B7 RID: 5303
		private uint _ref;
	}
}
