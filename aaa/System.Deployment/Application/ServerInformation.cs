using System;

namespace System.Deployment.Application
{
	// Token: 0x02000057 RID: 87
	internal class ServerInformation
	{
		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600029C RID: 668 RVA: 0x0000FADE File Offset: 0x0000EADE
		// (set) Token: 0x0600029D RID: 669 RVA: 0x0000FAE6 File Offset: 0x0000EAE6
		public string Server
		{
			get
			{
				return this._server;
			}
			set
			{
				this._server = value;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600029E RID: 670 RVA: 0x0000FAEF File Offset: 0x0000EAEF
		// (set) Token: 0x0600029F RID: 671 RVA: 0x0000FAF7 File Offset: 0x0000EAF7
		public string PoweredBy
		{
			get
			{
				return this._poweredBy;
			}
			set
			{
				this._poweredBy = value;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x0000FB00 File Offset: 0x0000EB00
		// (set) Token: 0x060002A1 RID: 673 RVA: 0x0000FB08 File Offset: 0x0000EB08
		public string AspNetVersion
		{
			get
			{
				return this._aspNetVersion;
			}
			set
			{
				this._aspNetVersion = value;
			}
		}

		// Token: 0x04000214 RID: 532
		private string _server;

		// Token: 0x04000215 RID: 533
		private string _poweredBy;

		// Token: 0x04000216 RID: 534
		private string _aspNetVersion;
	}
}
