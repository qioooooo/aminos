using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000DF RID: 223
	public class ReplicationFailureCollection : ReadOnlyCollectionBase
	{
		// Token: 0x060006ED RID: 1773 RVA: 0x00024D70 File Offset: 0x00023D70
		internal ReplicationFailureCollection(DirectoryServer server)
		{
			this.server = server;
			Hashtable hashtable = new Hashtable();
			this.nameTable = Hashtable.Synchronized(hashtable);
		}

		// Token: 0x170001A8 RID: 424
		public ReplicationFailure this[int index]
		{
			get
			{
				return (ReplicationFailure)base.InnerList[index];
			}
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x00024DAF File Offset: 0x00023DAF
		public bool Contains(ReplicationFailure failure)
		{
			if (failure == null)
			{
				throw new ArgumentNullException("failure");
			}
			return base.InnerList.Contains(failure);
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x00024DCB File Offset: 0x00023DCB
		public int IndexOf(ReplicationFailure failure)
		{
			if (failure == null)
			{
				throw new ArgumentNullException("failure");
			}
			return base.InnerList.IndexOf(failure);
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x00024DE7 File Offset: 0x00023DE7
		public void CopyTo(ReplicationFailure[] failures, int index)
		{
			base.InnerList.CopyTo(failures, index);
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00024DF6 File Offset: 0x00023DF6
		private int Add(ReplicationFailure failure)
		{
			return base.InnerList.Add(failure);
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x00024E04 File Offset: 0x00023E04
		internal void AddHelper(DS_REPL_KCC_DSA_FAILURES failures, IntPtr info)
		{
			int cNumEntries = failures.cNumEntries;
			IntPtr intPtr = (IntPtr)0;
			for (int i = 0; i < cNumEntries; i++)
			{
				intPtr = Utils.AddToIntPtr(info, Marshal.SizeOf(typeof(int)) * 2 + i * Marshal.SizeOf(typeof(DS_REPL_KCC_DSA_FAILURE)));
				ReplicationFailure replicationFailure = new ReplicationFailure(intPtr, this.server, this.nameTable);
				if (replicationFailure.LastErrorCode == 0)
				{
					replicationFailure.lastResult = ExceptionHelper.ERROR_DS_UNKNOWN_ERROR;
				}
				this.Add(replicationFailure);
			}
		}

		// Token: 0x0400057F RID: 1407
		private DirectoryServer server;

		// Token: 0x04000580 RID: 1408
		private Hashtable nameTable;
	}
}
