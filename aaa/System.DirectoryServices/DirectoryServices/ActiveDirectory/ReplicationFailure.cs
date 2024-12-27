using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000DE RID: 222
	public class ReplicationFailure
	{
		// Token: 0x060006E6 RID: 1766 RVA: 0x00024C34 File Offset: 0x00023C34
		internal ReplicationFailure(IntPtr addr, DirectoryServer server, Hashtable table)
		{
			DS_REPL_KCC_DSA_FAILURE ds_REPL_KCC_DSA_FAILURE = new DS_REPL_KCC_DSA_FAILURE();
			Marshal.PtrToStructure(addr, ds_REPL_KCC_DSA_FAILURE);
			this.sourceDsaDN = Marshal.PtrToStringUni(ds_REPL_KCC_DSA_FAILURE.pszDsaDN);
			this.uuidDsaObjGuid = ds_REPL_KCC_DSA_FAILURE.uuidDsaObjGuid;
			this.timeFirstFailure = DateTime.FromFileTime(ds_REPL_KCC_DSA_FAILURE.ftimeFirstFailure);
			this.numFailures = ds_REPL_KCC_DSA_FAILURE.cNumFailures;
			this.lastResult = ds_REPL_KCC_DSA_FAILURE.dwLastResult;
			this.server = server;
			this.nameTable = table;
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x00024CA8 File Offset: 0x00023CA8
		public string SourceServer
		{
			get
			{
				if (this.sourceServer == null)
				{
					if (this.nameTable.Contains(this.SourceServerGuid))
					{
						this.sourceServer = (string)this.nameTable[this.SourceServerGuid];
					}
					else if (this.sourceDsaDN != null)
					{
						this.sourceServer = Utils.GetServerNameFromInvocationID(this.sourceDsaDN, this.SourceServerGuid, this.server);
						this.nameTable.Add(this.SourceServerGuid, this.sourceServer);
					}
				}
				return this.sourceServer;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060006E8 RID: 1768 RVA: 0x00024D3F File Offset: 0x00023D3F
		private Guid SourceServerGuid
		{
			get
			{
				return this.uuidDsaObjGuid;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x00024D47 File Offset: 0x00023D47
		public DateTime FirstFailureTime
		{
			get
			{
				return this.timeFirstFailure;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x00024D4F File Offset: 0x00023D4F
		public int ConsecutiveFailureCount
		{
			get
			{
				return this.numFailures;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060006EB RID: 1771 RVA: 0x00024D57 File Offset: 0x00023D57
		public int LastErrorCode
		{
			get
			{
				return this.lastResult;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x00024D5F File Offset: 0x00023D5F
		public string LastErrorMessage
		{
			get
			{
				return ExceptionHelper.GetErrorMessage(this.lastResult, false);
			}
		}

		// Token: 0x04000577 RID: 1399
		private string sourceDsaDN;

		// Token: 0x04000578 RID: 1400
		private Guid uuidDsaObjGuid;

		// Token: 0x04000579 RID: 1401
		private DateTime timeFirstFailure;

		// Token: 0x0400057A RID: 1402
		private int numFailures;

		// Token: 0x0400057B RID: 1403
		internal int lastResult;

		// Token: 0x0400057C RID: 1404
		private DirectoryServer server;

		// Token: 0x0400057D RID: 1405
		private string sourceServer;

		// Token: 0x0400057E RID: 1406
		private Hashtable nameTable;
	}
}
