using System;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200009E RID: 158
	public class SyncFromAllServersErrorInformation
	{
		// Token: 0x06000537 RID: 1335 RVA: 0x0001E610 File Offset: 0x0001D610
		internal SyncFromAllServersErrorInformation(SyncFromAllServersErrorCategory category, int errorCode, string errorMessage, string sourceServer, string targetServer)
		{
			this.category = category;
			this.errorCode = errorCode;
			this.errorMessage = errorMessage;
			this.sourceServer = sourceServer;
			this.targetServer = targetServer;
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x0001E63D File Offset: 0x0001D63D
		public SyncFromAllServersErrorCategory ErrorCategory
		{
			get
			{
				return this.category;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x0001E645 File Offset: 0x0001D645
		public int ErrorCode
		{
			get
			{
				return this.errorCode;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x0001E64D File Offset: 0x0001D64D
		public string ErrorMessage
		{
			get
			{
				return this.errorMessage;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x0001E655 File Offset: 0x0001D655
		public string TargetServer
		{
			get
			{
				return this.targetServer;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x0001E65D File Offset: 0x0001D65D
		public string SourceServer
		{
			get
			{
				return this.sourceServer;
			}
		}

		// Token: 0x04000431 RID: 1073
		private SyncFromAllServersErrorCategory category;

		// Token: 0x04000432 RID: 1074
		private int errorCode;

		// Token: 0x04000433 RID: 1075
		private string errorMessage;

		// Token: 0x04000434 RID: 1076
		private string sourceServer;

		// Token: 0x04000435 RID: 1077
		private string targetServer;
	}
}
