using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System
{
	// Token: 0x020000CF RID: 207
	internal class LocalDataStoreMgr
	{
		// Token: 0x06000B9B RID: 2971 RVA: 0x000233C4 File Offset: 0x000223C4
		public LocalDataStore CreateLocalDataStore()
		{
			LocalDataStore localDataStore = new LocalDataStore(this, this.m_SlotInfoTable.Length);
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.ReliableEnter(this, ref flag);
				this.m_ManagedLocalDataStores.Add(localDataStore);
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
			return localDataStore;
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0002341C File Offset: 0x0002241C
		public void DeleteLocalDataStore(LocalDataStore store)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.ReliableEnter(this, ref flag);
				this.m_ManagedLocalDataStores.Remove(store);
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x00023460 File Offset: 0x00022460
		public LocalDataStoreSlot AllocateDataSlot()
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			LocalDataStoreSlot localDataStoreSlot2;
			try
			{
				Monitor.ReliableEnter(this, ref flag);
				int num = this.m_SlotInfoTable.Length;
				if (this.m_FirstAvailableSlot < num)
				{
					LocalDataStoreSlot localDataStoreSlot = new LocalDataStoreSlot(this, this.m_FirstAvailableSlot);
					this.m_SlotInfoTable[this.m_FirstAvailableSlot] = 1;
					int num2 = this.m_FirstAvailableSlot + 1;
					while (num2 < num && (this.m_SlotInfoTable[num2] & 1) != 0)
					{
						num2++;
					}
					this.m_FirstAvailableSlot = num2;
					localDataStoreSlot2 = localDataStoreSlot;
				}
				else
				{
					int num3;
					if (num < 512)
					{
						num3 = num * 2;
					}
					else
					{
						num3 = num + 128;
					}
					byte[] array = new byte[num3];
					Array.Copy(this.m_SlotInfoTable, array, num);
					this.m_SlotInfoTable = array;
					LocalDataStoreSlot localDataStoreSlot = new LocalDataStoreSlot(this, num);
					this.m_SlotInfoTable[num] = 1;
					this.m_FirstAvailableSlot = num + 1;
					localDataStoreSlot2 = localDataStoreSlot;
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
			return localDataStoreSlot2;
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x00023548 File Offset: 0x00022548
		public LocalDataStoreSlot AllocateNamedDataSlot(string name)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			LocalDataStoreSlot localDataStoreSlot2;
			try
			{
				Monitor.ReliableEnter(this, ref flag);
				LocalDataStoreSlot localDataStoreSlot = this.AllocateDataSlot();
				this.m_KeyToSlotMap.Add(name, localDataStoreSlot);
				localDataStoreSlot2 = localDataStoreSlot;
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
			return localDataStoreSlot2;
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x00023598 File Offset: 0x00022598
		public LocalDataStoreSlot GetNamedDataSlot(string name)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			LocalDataStoreSlot localDataStoreSlot2;
			try
			{
				Monitor.ReliableEnter(this, ref flag);
				LocalDataStoreSlot localDataStoreSlot = (LocalDataStoreSlot)this.m_KeyToSlotMap[name];
				if (localDataStoreSlot == null)
				{
					localDataStoreSlot2 = this.AllocateNamedDataSlot(name);
				}
				else
				{
					localDataStoreSlot2 = localDataStoreSlot;
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
			return localDataStoreSlot2;
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x000235F4 File Offset: 0x000225F4
		public void FreeNamedDataSlot(string name)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.ReliableEnter(this, ref flag);
				this.m_KeyToSlotMap.Remove(name);
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x00023638 File Offset: 0x00022638
		internal void FreeDataSlot(int slot)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.ReliableEnter(this, ref flag);
				for (int i = 0; i < this.m_ManagedLocalDataStores.Count; i++)
				{
					((LocalDataStore)this.m_ManagedLocalDataStores[i]).SetDataInternal(slot, null, false);
				}
				this.m_SlotInfoTable[slot] = 0;
				if (slot < this.m_FirstAvailableSlot)
				{
					this.m_FirstAvailableSlot = slot;
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x000236B8 File Offset: 0x000226B8
		public void ValidateSlot(LocalDataStoreSlot slot)
		{
			if (slot == null || slot.Manager != this)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ALSInvalidSlot"));
			}
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x000236D6 File Offset: 0x000226D6
		internal int GetSlotTableLength()
		{
			return this.m_SlotInfoTable.Length;
		}

		// Token: 0x04000406 RID: 1030
		private const byte DataSlotOccupied = 1;

		// Token: 0x04000407 RID: 1031
		private const int InitialSlotTableSize = 64;

		// Token: 0x04000408 RID: 1032
		private const int SlotTableDoubleThreshold = 512;

		// Token: 0x04000409 RID: 1033
		private const int LargeSlotTableSizeIncrease = 128;

		// Token: 0x0400040A RID: 1034
		private byte[] m_SlotInfoTable = new byte[64];

		// Token: 0x0400040B RID: 1035
		private int m_FirstAvailableSlot;

		// Token: 0x0400040C RID: 1036
		private ArrayList m_ManagedLocalDataStores = new ArrayList();

		// Token: 0x0400040D RID: 1037
		private Hashtable m_KeyToSlotMap = new Hashtable();
	}
}
