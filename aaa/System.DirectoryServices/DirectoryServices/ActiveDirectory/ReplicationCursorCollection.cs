using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000DD RID: 221
	public class ReplicationCursorCollection : ReadOnlyCollectionBase
	{
		// Token: 0x060006DF RID: 1759 RVA: 0x00024AAA File Offset: 0x00023AAA
		internal ReplicationCursorCollection(DirectoryServer server)
		{
			this.server = server;
		}

		// Token: 0x170001A1 RID: 417
		public ReplicationCursor this[int index]
		{
			get
			{
				return (ReplicationCursor)base.InnerList[index];
			}
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00024ACC File Offset: 0x00023ACC
		public bool Contains(ReplicationCursor cursor)
		{
			if (cursor == null)
			{
				throw new ArgumentNullException("cursor");
			}
			return base.InnerList.Contains(cursor);
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00024AE8 File Offset: 0x00023AE8
		public int IndexOf(ReplicationCursor cursor)
		{
			if (cursor == null)
			{
				throw new ArgumentNullException("cursor");
			}
			return base.InnerList.IndexOf(cursor);
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x00024B04 File Offset: 0x00023B04
		public void CopyTo(ReplicationCursor[] values, int index)
		{
			base.InnerList.CopyTo(values, index);
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x00024B13 File Offset: 0x00023B13
		private int Add(ReplicationCursor cursor)
		{
			return base.InnerList.Add(cursor);
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x00024B24 File Offset: 0x00023B24
		internal void AddHelper(string partition, object cursors, bool advanced, IntPtr info)
		{
			int num;
			if (advanced)
			{
				num = ((DS_REPL_CURSORS_3)cursors).cNumCursors;
			}
			else
			{
				num = ((DS_REPL_CURSORS)cursors).cNumCursors;
			}
			IntPtr intPtr = (IntPtr)0;
			for (int i = 0; i < num; i++)
			{
				if (advanced)
				{
					intPtr = Utils.AddToIntPtr(info, Marshal.SizeOf(typeof(int)) * 2 + i * Marshal.SizeOf(typeof(DS_REPL_CURSOR_3)));
					DS_REPL_CURSOR_3 ds_REPL_CURSOR_ = new DS_REPL_CURSOR_3();
					Marshal.PtrToStructure(intPtr, ds_REPL_CURSOR_);
					ReplicationCursor replicationCursor = new ReplicationCursor(this.server, partition, ds_REPL_CURSOR_.uuidSourceDsaInvocationID, ds_REPL_CURSOR_.usnAttributeFilter, ds_REPL_CURSOR_.ftimeLastSyncSuccess, ds_REPL_CURSOR_.pszSourceDsaDN);
					this.Add(replicationCursor);
				}
				else
				{
					intPtr = Utils.AddToIntPtr(info, Marshal.SizeOf(typeof(int)) * 2 + i * Marshal.SizeOf(typeof(DS_REPL_CURSOR)));
					DS_REPL_CURSOR ds_REPL_CURSOR = new DS_REPL_CURSOR();
					Marshal.PtrToStructure(intPtr, ds_REPL_CURSOR);
					ReplicationCursor replicationCursor2 = new ReplicationCursor(this.server, partition, ds_REPL_CURSOR.uuidSourceDsaInvocationID, ds_REPL_CURSOR.usnAttributeFilter);
					this.Add(replicationCursor2);
				}
			}
		}

		// Token: 0x04000576 RID: 1398
		private DirectoryServer server;
	}
}
