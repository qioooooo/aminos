using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000DC RID: 220
	public class ReplicationCursor
	{
		// Token: 0x060006D7 RID: 1751 RVA: 0x00024978 File Offset: 0x00023978
		private ReplicationCursor()
		{
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x00024980 File Offset: 0x00023980
		internal ReplicationCursor(DirectoryServer server, string partition, Guid guid, long filter, long time, IntPtr dn)
		{
			this.partition = partition;
			this.invocationID = guid;
			this.USN = filter;
			this.syncTime = DateTime.FromFileTime(time);
			this.serverDN = Marshal.PtrToStringUni(dn);
			this.advanced = true;
			this.server = server;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x000249D1 File Offset: 0x000239D1
		internal ReplicationCursor(DirectoryServer server, string partition, Guid guid, long filter)
		{
			this.partition = partition;
			this.invocationID = guid;
			this.USN = filter;
			this.server = server;
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x000249F6 File Offset: 0x000239F6
		public string PartitionName
		{
			get
			{
				return this.partition;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x000249FE File Offset: 0x000239FE
		public Guid SourceInvocationId
		{
			get
			{
				return this.invocationID;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060006DC RID: 1756 RVA: 0x00024A06 File Offset: 0x00023A06
		public long UpToDatenessUsn
		{
			get
			{
				return this.USN;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x00024A0E File Offset: 0x00023A0E
		public string SourceServer
		{
			get
			{
				if (!this.advanced || (this.advanced && this.serverDN != null))
				{
					this.sourceServer = Utils.GetServerNameFromInvocationID(this.serverDN, this.SourceInvocationId, this.server);
				}
				return this.sourceServer;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x00024A4C File Offset: 0x00023A4C
		public DateTime LastSuccessfulSyncTime
		{
			get
			{
				if (this.advanced)
				{
					return this.syncTime;
				}
				if (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor == 0)
				{
					throw new PlatformNotSupportedException(Res.GetString("DSNotSupportOnClient"));
				}
				throw new PlatformNotSupportedException(Res.GetString("DSNotSupportOnDC"));
			}
		}

		// Token: 0x0400056E RID: 1390
		private string partition;

		// Token: 0x0400056F RID: 1391
		private Guid invocationID;

		// Token: 0x04000570 RID: 1392
		private long USN;

		// Token: 0x04000571 RID: 1393
		private string serverDN;

		// Token: 0x04000572 RID: 1394
		private DateTime syncTime;

		// Token: 0x04000573 RID: 1395
		private bool advanced;

		// Token: 0x04000574 RID: 1396
		private string sourceServer;

		// Token: 0x04000575 RID: 1397
		private DirectoryServer server;
	}
}
