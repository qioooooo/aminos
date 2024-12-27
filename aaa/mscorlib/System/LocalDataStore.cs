using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System
{
	// Token: 0x020000CC RID: 204
	internal class LocalDataStore
	{
		// Token: 0x06000B8E RID: 2958 RVA: 0x0002318D File Offset: 0x0002218D
		public LocalDataStore(LocalDataStoreMgr mgr, int InitialCapacity)
		{
			if (mgr == null)
			{
				throw new ArgumentNullException("mgr");
			}
			this.m_Manager = mgr;
			this.m_DataTable = new object[InitialCapacity];
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x000231B8 File Offset: 0x000221B8
		public object GetData(LocalDataStoreSlot slot)
		{
			object obj = null;
			this.m_Manager.ValidateSlot(slot);
			int slot2 = slot.Slot;
			if (slot2 >= 0)
			{
				if (slot2 >= this.m_DataTable.Length)
				{
					return null;
				}
				obj = this.m_DataTable[slot2];
			}
			if (!slot.IsValid())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_SlotHasBeenFreed"));
			}
			return obj;
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x00023210 File Offset: 0x00022210
		public void SetData(LocalDataStoreSlot slot, object data)
		{
			this.m_Manager.ValidateSlot(slot);
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.ReliableEnter(this.m_Manager, ref flag);
				if (!slot.IsValid())
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_SlotHasBeenFreed"));
				}
				this.SetDataInternal(slot.Slot, data, true);
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this.m_Manager);
				}
			}
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x00023284 File Offset: 0x00022284
		internal void SetDataInternal(int slot, object data, bool bAlloc)
		{
			if (slot >= this.m_DataTable.Length)
			{
				if (!bAlloc)
				{
					return;
				}
				this.SetCapacity(this.m_Manager.GetSlotTableLength());
			}
			this.m_DataTable[slot] = data;
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x000232B0 File Offset: 0x000222B0
		private void SetCapacity(int capacity)
		{
			if (capacity < this.m_DataTable.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ALSInvalidCapacity"));
			}
			object[] array = new object[capacity];
			Array.Copy(this.m_DataTable, array, this.m_DataTable.Length);
			this.m_DataTable = array;
		}

		// Token: 0x04000400 RID: 1024
		private object[] m_DataTable;

		// Token: 0x04000401 RID: 1025
		private LocalDataStoreMgr m_Manager;

		// Token: 0x04000402 RID: 1026
		private int DONT_USE_InternalStore;
	}
}
