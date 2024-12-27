using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000E5 RID: 229
	public class ReplicationOperationCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06000710 RID: 1808 RVA: 0x000252DC File Offset: 0x000242DC
		internal ReplicationOperationCollection(DirectoryServer server)
		{
			this.server = server;
			Hashtable hashtable = new Hashtable();
			this.nameTable = Hashtable.Synchronized(hashtable);
		}

		// Token: 0x170001BD RID: 445
		public ReplicationOperation this[int index]
		{
			get
			{
				return (ReplicationOperation)base.InnerList[index];
			}
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0002531B File Offset: 0x0002431B
		public bool Contains(ReplicationOperation operation)
		{
			if (operation == null)
			{
				throw new ArgumentNullException("operation");
			}
			return base.InnerList.Contains(operation);
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x00025337 File Offset: 0x00024337
		public int IndexOf(ReplicationOperation operation)
		{
			if (operation == null)
			{
				throw new ArgumentNullException("operation");
			}
			return base.InnerList.IndexOf(operation);
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x00025353 File Offset: 0x00024353
		public void CopyTo(ReplicationOperation[] operations, int index)
		{
			base.InnerList.CopyTo(operations, index);
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x00025362 File Offset: 0x00024362
		private int Add(ReplicationOperation operation)
		{
			return base.InnerList.Add(operation);
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x00025370 File Offset: 0x00024370
		internal void AddHelper(DS_REPL_PENDING_OPS operations, IntPtr info)
		{
			int cNumPendingOps = operations.cNumPendingOps;
			IntPtr intPtr = (IntPtr)0;
			for (int i = 0; i < cNumPendingOps; i++)
			{
				intPtr = Utils.AddToIntPtr(info, Marshal.SizeOf(typeof(DS_REPL_PENDING_OPS)) + i * Marshal.SizeOf(typeof(DS_REPL_OP)));
				ReplicationOperation replicationOperation = new ReplicationOperation(intPtr, this.server, this.nameTable);
				this.Add(replicationOperation);
			}
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x000253DC File Offset: 0x000243DC
		internal ReplicationOperation GetFirstOperation()
		{
			ReplicationOperation replicationOperation = (ReplicationOperation)base.InnerList[0];
			base.InnerList.RemoveAt(0);
			return replicationOperation;
		}

		// Token: 0x040005AE RID: 1454
		private DirectoryServer server;

		// Token: 0x040005AF RID: 1455
		private Hashtable nameTable;
	}
}
