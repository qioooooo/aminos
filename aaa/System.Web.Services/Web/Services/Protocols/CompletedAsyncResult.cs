using System;
using System.Threading;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200008E RID: 142
	internal class CompletedAsyncResult : IAsyncResult
	{
		// Token: 0x060003B5 RID: 949 RVA: 0x00012B51 File Offset: 0x00011B51
		internal CompletedAsyncResult(object asyncState, bool completedSynchronously)
		{
			this.asyncState = asyncState;
			this.completedSynchronously = completedSynchronously;
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x00012B67 File Offset: 0x00011B67
		public object AsyncState
		{
			get
			{
				return this.asyncState;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x00012B6F File Offset: 0x00011B6F
		public bool CompletedSynchronously
		{
			get
			{
				return this.completedSynchronously;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x00012B77 File Offset: 0x00011B77
		public bool IsCompleted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x00012B7A File Offset: 0x00011B7A
		public WaitHandle AsyncWaitHandle
		{
			get
			{
				return null;
			}
		}

		// Token: 0x04000393 RID: 915
		private object asyncState;

		// Token: 0x04000394 RID: 916
		private bool completedSynchronously;
	}
}
