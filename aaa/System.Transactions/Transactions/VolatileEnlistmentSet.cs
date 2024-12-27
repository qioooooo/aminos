using System;

namespace System.Transactions
{
	// Token: 0x02000059 RID: 89
	internal struct VolatileEnlistmentSet
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600029C RID: 668 RVA: 0x000302D4 File Offset: 0x0002F6D4
		// (set) Token: 0x0600029D RID: 669 RVA: 0x000302E8 File Offset: 0x0002F6E8
		internal VolatileDemultiplexer VolatileDemux
		{
			get
			{
				return this.volatileDemux;
			}
			set
			{
				this.volatileDemux = value;
			}
		}

		// Token: 0x04000111 RID: 273
		internal InternalEnlistment[] volatileEnlistments;

		// Token: 0x04000112 RID: 274
		internal int volatileEnlistmentCount;

		// Token: 0x04000113 RID: 275
		internal int volatileEnlistmentSize;

		// Token: 0x04000114 RID: 276
		internal int dependentClones;

		// Token: 0x04000115 RID: 277
		internal int preparedVolatileEnlistments;

		// Token: 0x04000116 RID: 278
		private VolatileDemultiplexer volatileDemux;
	}
}
