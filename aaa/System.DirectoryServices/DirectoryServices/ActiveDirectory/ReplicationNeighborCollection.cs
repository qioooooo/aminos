using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000E3 RID: 227
	public class ReplicationNeighborCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06000701 RID: 1793 RVA: 0x00025080 File Offset: 0x00024080
		internal ReplicationNeighborCollection(DirectoryServer server)
		{
			this.server = server;
			Hashtable hashtable = new Hashtable();
			this.nameTable = Hashtable.Synchronized(hashtable);
		}

		// Token: 0x170001B5 RID: 437
		public ReplicationNeighbor this[int index]
		{
			get
			{
				return (ReplicationNeighbor)base.InnerList[index];
			}
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x000250BF File Offset: 0x000240BF
		public bool Contains(ReplicationNeighbor neighbor)
		{
			if (neighbor == null)
			{
				throw new ArgumentNullException("neighbor");
			}
			return base.InnerList.Contains(neighbor);
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x000250DB File Offset: 0x000240DB
		public int IndexOf(ReplicationNeighbor neighbor)
		{
			if (neighbor == null)
			{
				throw new ArgumentNullException("neighbor");
			}
			return base.InnerList.IndexOf(neighbor);
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x000250F7 File Offset: 0x000240F7
		public void CopyTo(ReplicationNeighbor[] neighbors, int index)
		{
			base.InnerList.CopyTo(neighbors, index);
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x00025106 File Offset: 0x00024106
		private int Add(ReplicationNeighbor neighbor)
		{
			return base.InnerList.Add(neighbor);
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x00025114 File Offset: 0x00024114
		internal void AddHelper(DS_REPL_NEIGHBORS neighbors, IntPtr info)
		{
			int cNumNeighbors = neighbors.cNumNeighbors;
			IntPtr intPtr = (IntPtr)0;
			for (int i = 0; i < cNumNeighbors; i++)
			{
				intPtr = Utils.AddToIntPtr(info, Marshal.SizeOf(typeof(int)) * 2 + i * Marshal.SizeOf(typeof(DS_REPL_NEIGHBOR)));
				ReplicationNeighbor replicationNeighbor = new ReplicationNeighbor(intPtr, this.server, this.nameTable);
				this.Add(replicationNeighbor);
			}
		}

		// Token: 0x040005A2 RID: 1442
		private DirectoryServer server;

		// Token: 0x040005A3 RID: 1443
		private Hashtable nameTable;
	}
}
