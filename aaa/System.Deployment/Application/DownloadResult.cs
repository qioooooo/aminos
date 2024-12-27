using System;

namespace System.Deployment.Application
{
	// Token: 0x02000058 RID: 88
	internal class DownloadResult
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x0000FB19 File Offset: 0x0000EB19
		// (set) Token: 0x060002A4 RID: 676 RVA: 0x0000FB21 File Offset: 0x0000EB21
		public Uri ResponseUri
		{
			get
			{
				return this._responseUri;
			}
			set
			{
				this._responseUri = value;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000FB2A File Offset: 0x0000EB2A
		public ServerInformation ServerInformation
		{
			get
			{
				return this._serverInformation;
			}
		}

		// Token: 0x04000217 RID: 535
		private Uri _responseUri;

		// Token: 0x04000218 RID: 536
		private ServerInformation _serverInformation = new ServerInformation();
	}
}
