using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System
{
	// Token: 0x020000CE RID: 206
	[ComVisible(true)]
	public sealed class LocalDataStoreSlot
	{
		// Token: 0x06000B95 RID: 2965 RVA: 0x00023306 File Offset: 0x00022306
		internal LocalDataStoreSlot(LocalDataStoreMgr mgr, int slot)
		{
			this.m_mgr = mgr;
			this.m_slot = slot;
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000B96 RID: 2966 RVA: 0x0002331C File Offset: 0x0002231C
		internal LocalDataStoreMgr Manager
		{
			get
			{
				return this.m_mgr;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000B97 RID: 2967 RVA: 0x00023324 File Offset: 0x00022324
		internal int Slot
		{
			get
			{
				return this.m_slot;
			}
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x0002332C File Offset: 0x0002232C
		internal bool IsValid()
		{
			return LocalDataStoreSlot.m_helper.Get(ref this.m_slot) != -1;
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x00023344 File Offset: 0x00022344
		protected override void Finalize()
		{
			try
			{
				int slot = this.m_slot;
				bool flag = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					Monitor.ReliableEnter(this.m_mgr, ref flag);
					this.m_slot = -1;
					this.m_mgr.FreeDataSlot(slot);
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this.m_mgr);
					}
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x04000403 RID: 1027
		private static LdsSyncHelper m_helper = new LdsSyncHelper();

		// Token: 0x04000404 RID: 1028
		private LocalDataStoreMgr m_mgr;

		// Token: 0x04000405 RID: 1029
		private int m_slot;
	}
}
