using System;
using System.ComponentModel;

namespace System.Deployment.Application
{
	// Token: 0x02000060 RID: 96
	public class DownloadProgressChangedEventArgs : ProgressChangedEventArgs
	{
		// Token: 0x060002FA RID: 762 RVA: 0x0001141F File Offset: 0x0001041F
		internal DownloadProgressChangedEventArgs(int progressPercentage, object userState, long bytesCompleted, long bytesTotal, DeploymentProgressState downloadProgressState)
			: base(progressPercentage, userState)
		{
			this._bytesCompleted = bytesCompleted;
			this._bytesTotal = bytesTotal;
			this._deploymentProgressState = downloadProgressState;
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060002FB RID: 763 RVA: 0x00011440 File Offset: 0x00010440
		public long BytesDownloaded
		{
			get
			{
				return this._bytesCompleted;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060002FC RID: 764 RVA: 0x00011448 File Offset: 0x00010448
		public long TotalBytesToDownload
		{
			get
			{
				return this._bytesTotal;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060002FD RID: 765 RVA: 0x00011450 File Offset: 0x00010450
		public DeploymentProgressState State
		{
			get
			{
				return this._deploymentProgressState;
			}
		}

		// Token: 0x04000248 RID: 584
		private long _bytesCompleted;

		// Token: 0x04000249 RID: 585
		private long _bytesTotal;

		// Token: 0x0400024A RID: 586
		private DeploymentProgressState _deploymentProgressState;
	}
}
