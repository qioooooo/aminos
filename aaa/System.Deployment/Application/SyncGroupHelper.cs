using System;
using System.ComponentModel;
using System.Threading;

namespace System.Deployment.Application
{
	// Token: 0x02000044 RID: 68
	internal class SyncGroupHelper : IDownloadNotification
	{
		// Token: 0x06000220 RID: 544 RVA: 0x0000DCD3 File Offset: 0x0000CCD3
		public SyncGroupHelper(string groupName, object userState, AsyncOperation asyncOp, SendOrPostCallback progressReporterDelegate)
		{
			if (groupName == null)
			{
				throw new ArgumentNullException("groupName");
			}
			this.groupName = groupName;
			this.userState = userState;
			this.asyncOperation = asyncOp;
			this.progressReporter = progressReporterDelegate;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000DD06 File Offset: 0x0000CD06
		public void SetComplete()
		{
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000DD08 File Offset: 0x0000CD08
		public void CancelAsync()
		{
			this._cancellationPending = true;
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0000DD11 File Offset: 0x0000CD11
		public bool CancellationPending
		{
			get
			{
				return this._cancellationPending;
			}
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000DD1C File Offset: 0x0000CD1C
		public void DownloadModified(object sender, DownloadEventArgs e)
		{
			if (this._cancellationPending)
			{
				((FileDownloader)sender).Cancel();
			}
			this.asyncOperation.Post(this.progressReporter, new DeploymentProgressChangedEventArgs(e.Progress, this.userState, e.BytesCompleted, e.BytesTotal, DeploymentProgressState.DownloadingApplicationFiles, this.groupName));
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000DD71 File Offset: 0x0000CD71
		public void DownloadCompleted(object sender, DownloadEventArgs e)
		{
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000226 RID: 550 RVA: 0x0000DD73 File Offset: 0x0000CD73
		public string Group
		{
			get
			{
				return this.groupName;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000227 RID: 551 RVA: 0x0000DD7B File Offset: 0x0000CD7B
		public object UserState
		{
			get
			{
				return this.userState;
			}
		}

		// Token: 0x040001CC RID: 460
		private readonly string groupName;

		// Token: 0x040001CD RID: 461
		private readonly object userState;

		// Token: 0x040001CE RID: 462
		private readonly AsyncOperation asyncOperation;

		// Token: 0x040001CF RID: 463
		private readonly SendOrPostCallback progressReporter;

		// Token: 0x040001D0 RID: 464
		private bool _cancellationPending;
	}
}
