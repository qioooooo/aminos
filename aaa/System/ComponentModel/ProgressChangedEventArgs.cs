using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000126 RID: 294
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ProgressChangedEventArgs : EventArgs
	{
		// Token: 0x06000963 RID: 2403 RVA: 0x0001F6EC File Offset: 0x0001E6EC
		public ProgressChangedEventArgs(int progressPercentage, object userState)
		{
			this.progressPercentage = progressPercentage;
			this.userState = userState;
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000964 RID: 2404 RVA: 0x0001F702 File Offset: 0x0001E702
		[SRDescription("Async_ProgressChangedEventArgs_ProgressPercentage")]
		public int ProgressPercentage
		{
			get
			{
				return this.progressPercentage;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000965 RID: 2405 RVA: 0x0001F70A File Offset: 0x0001E70A
		[SRDescription("Async_ProgressChangedEventArgs_UserState")]
		public object UserState
		{
			get
			{
				return this.userState;
			}
		}

		// Token: 0x04000A0C RID: 2572
		private readonly int progressPercentage;

		// Token: 0x04000A0D RID: 2573
		private readonly object userState;
	}
}
