using System;

namespace System.Web.Caching
{
	// Token: 0x02000115 RID: 277
	internal struct UsageEntryRef
	{
		// Token: 0x06000CBC RID: 3260 RVA: 0x00033524 File Offset: 0x00032524
		internal UsageEntryRef(int pageIndex, int entryIndex)
		{
			this._ref = (uint)((pageIndex << 8) | (entryIndex & 255));
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x00033537 File Offset: 0x00032537
		public override bool Equals(object value)
		{
			return value is UsageEntryRef && this._ref == ((UsageEntryRef)value)._ref;
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x00033556 File Offset: 0x00032556
		public static bool operator ==(UsageEntryRef r1, UsageEntryRef r2)
		{
			return r1._ref == r2._ref;
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x00033568 File Offset: 0x00032568
		public static bool operator !=(UsageEntryRef r1, UsageEntryRef r2)
		{
			return r1._ref != r2._ref;
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x0003357D File Offset: 0x0003257D
		public override int GetHashCode()
		{
			return (int)this._ref;
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x00033588 File Offset: 0x00032588
		internal int PageIndex
		{
			get
			{
				return (int)(this._ref >> 8);
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x000335A0 File Offset: 0x000325A0
		internal int Ref1Index
		{
			get
			{
				return (int)((sbyte)(this._ref & 255U));
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x000335BC File Offset: 0x000325BC
		internal int Ref2Index
		{
			get
			{
				int num = (int)((sbyte)(this._ref & 255U));
				return -num;
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x000335D9 File Offset: 0x000325D9
		internal bool IsRef1
		{
			get
			{
				return (sbyte)(this._ref & 255U) > 0;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x000335EB File Offset: 0x000325EB
		internal bool IsRef2
		{
			get
			{
				return (sbyte)(this._ref & 255U) < 0;
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x000335FD File Offset: 0x000325FD
		internal bool IsInvalid
		{
			get
			{
				return this._ref == 0U;
			}
		}

		// Token: 0x04001488 RID: 5256
		private const uint ENTRY_MASK = 255U;

		// Token: 0x04001489 RID: 5257
		private const uint PAGE_MASK = 4294967040U;

		// Token: 0x0400148A RID: 5258
		private const int PAGE_SHIFT = 8;

		// Token: 0x0400148B RID: 5259
		internal static readonly UsageEntryRef INVALID = new UsageEntryRef(0, 0);

		// Token: 0x0400148C RID: 5260
		private uint _ref;
	}
}
