using System;

namespace System.Threading
{
	// Token: 0x0200014E RID: 334
	internal sealed class OverlappedDataCacheLine
	{
		// Token: 0x06001283 RID: 4739 RVA: 0x00033EDC File Offset: 0x00032EDC
		internal OverlappedDataCacheLine()
		{
			this.m_items = new OverlappedData[16];
			new object();
			for (short num = 0; num < 16; num += 1)
			{
				this.m_items[(int)num] = new OverlappedData(this);
				this.m_items[(int)num].m_slot = num;
			}
			new object();
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x00033F34 File Offset: 0x00032F34
		~OverlappedDataCacheLine()
		{
			this.m_removed = true;
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06001285 RID: 4741 RVA: 0x00033F64 File Offset: 0x00032F64
		// (set) Token: 0x06001286 RID: 4742 RVA: 0x00033F6C File Offset: 0x00032F6C
		internal bool Removed
		{
			get
			{
				return this.m_removed;
			}
			set
			{
				this.m_removed = value;
			}
		}

		// Token: 0x04000630 RID: 1584
		internal const short CacheSize = 16;

		// Token: 0x04000631 RID: 1585
		internal OverlappedData[] m_items;

		// Token: 0x04000632 RID: 1586
		internal OverlappedDataCacheLine m_next;

		// Token: 0x04000633 RID: 1587
		private bool m_removed;
	}
}
