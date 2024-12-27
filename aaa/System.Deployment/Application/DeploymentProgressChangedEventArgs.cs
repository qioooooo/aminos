using System;
using System.ComponentModel;

namespace System.Deployment.Application
{
	// Token: 0x02000042 RID: 66
	public class DeploymentProgressChangedEventArgs : ProgressChangedEventArgs
	{
		// Token: 0x0600021B RID: 539 RVA: 0x0000DC8A File Offset: 0x0000CC8A
		internal DeploymentProgressChangedEventArgs(int progressPercentage, object userState, long bytesCompleted, long bytesTotal, DeploymentProgressState state, string groupName)
			: base(progressPercentage, userState)
		{
			this._bytesCompleted = bytesCompleted;
			this._bytesTotal = bytesTotal;
			this._state = state;
			this._groupName = groupName;
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600021C RID: 540 RVA: 0x0000DCB3 File Offset: 0x0000CCB3
		public long BytesCompleted
		{
			get
			{
				return this._bytesCompleted;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600021D RID: 541 RVA: 0x0000DCBB File Offset: 0x0000CCBB
		public long BytesTotal
		{
			get
			{
				return this._bytesTotal;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600021E RID: 542 RVA: 0x0000DCC3 File Offset: 0x0000CCC3
		public DeploymentProgressState State
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600021F RID: 543 RVA: 0x0000DCCB File Offset: 0x0000CCCB
		public string Group
		{
			get
			{
				return this._groupName;
			}
		}

		// Token: 0x040001C4 RID: 452
		private readonly long _bytesCompleted;

		// Token: 0x040001C5 RID: 453
		private readonly long _bytesTotal;

		// Token: 0x040001C6 RID: 454
		private readonly DeploymentProgressState _state;

		// Token: 0x040001C7 RID: 455
		private readonly string _groupName;
	}
}
